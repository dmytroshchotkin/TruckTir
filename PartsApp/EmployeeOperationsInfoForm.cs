using Infrastructure.Storage;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models.Helper;
using PartsApp.SupportClasses;

namespace PartsApp
{
    public partial class EmployeeOperationsInfoForm : Form
    {
        private List<Employee> _employees;
        private Employee _selectedEmployee;
        private EmployeeOperationsCache _operationsCache;

        public EmployeeOperationsInfoForm()
        {
            InitializeComponent();

            if (Form1.CurEmployee.IsAdmin)
            {
                EnableEditingContextMenu();
            }
        }

        private void EmployeeOperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Находим список всех сотрудников (сортируем по фамилии и имени) и делаем источником данных для ListBox.
            _employees = PartsDAL.FindEmployees().OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
            _operationsCache = new EmployeeOperationsCache();
            ActiveEmployeesCheckBox.Checked = true;

            //Устанавливаем стартовый период в месяц.
            EndDateDTP.Value = DateTime.Now;
            BeginDateDTP.Value = DateTime.Today.AddMonths(-1);

            // подписываем DateTimePicker'ы на события после того, как им были присвоены начальные значения,
            // чтобы не грузить операции до выбора сотрудника юзером
            BeginDateCheckBox.CheckedChanged += new EventHandler(DatesCheckBox_CheckedChanged);
            EndDateCheckBox.CheckedChanged += new EventHandler(DatesCheckBox_CheckedChanged);
            BeginDateDTP.ValueChanged += new EventHandler(DatesDTP_ValueChanged);
        }

        #region Вывод списков сотрудников (активных, уволенных, всех) и редактирование
        private void OnEmployeeListBoxMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Form1.CurEmployee.IsAdmin && _selectedEmployee != null)
            {
                HandleToolStripMenuOptions(_selectedEmployee.IsDismissed);
            }
        }

        private void HandleToolStripMenuOptions(bool isDismissed)
        {
            if (isDismissed)
            {
                DismissalToolStripMenuItem.Visible = false;
            }
            else
            {
                DismissalToolStripMenuItem.Visible = true;
            }

            EmployeeEditingContextMenu.Show();
        }

        private void EnableEditingContextMenu()
        {
            EmployeeEditingContextMenu.Items.Add(EditToolStripMenuItem);
            EmployeeEditingContextMenu.Items.Add(DismissalToolStripMenuItem);
            EmployeeListBox.MouseDown += new MouseEventHandler(OnEmployeeListBoxMouseDown);
        }

        private void OnDismissalOptionClick(object sender, EventArgs e)
        {
            if (_selectedEmployee != null)
            {
                var dismissForm = new DismissEmployeeForm(_selectedEmployee);
                dismissForm.ShowDialog();

                if (_selectedEmployee.IsDismissed)
                {
                    UpdateActiveEmployeesIfChecked();
                }
            }
        }

        private void OnEditingOptionClick(object sender, EventArgs e)
        {
            if (_selectedEmployee != null)
            {
                var editingForm = new AddEmployeeForm(_selectedEmployee);
                editingForm.ShowDialog();

                _selectedEmployee = editingForm.EditEmployee;
                ReplaceSelectedEmployeeDataAfterEditing();
                OnEmployeesCheckBoxesCheckedChanged(null, null);
            }
        }

        private void ReplaceSelectedEmployeeDataAfterEditing()
        {
            _employees = _employees.Where(emp => emp.EmployeeId != _selectedEmployee.EmployeeId).ToList();
            _employees.Add(_selectedEmployee);
        }

        private void OnEmployeesCheckBoxesCheckedChanged(object sender, EventArgs e)
        {
            var employees = GetEmployees(ActiveEmployeesCheckBox.Checked, InactiveEmployeesCheckBox.Checked);
            EmployeeListBox.DataSource = employees.Any() ? employees : null;
            ClearEmployeeListBox();
            ResetSelectedEmployee();
        }

        private List<Employee> GetEmployees(bool selectActiveEmployees, bool selectInactiveEmployees)
        {
            var employees = new List<Employee>();

            if (selectActiveEmployees)
            {
                employees.AddRange(GetActiveEmployees());
            }

            if (selectInactiveEmployees)
            {
                employees.AddRange(GetInactiveEmployees());
            }           

            return employees;
        }

        /// <summary>
        /// Очищает DataSource и обновляет настройки для ListBox 
        /// </summary>
        private void ClearEmployeeListBox()
        {
            if (EmployeeListBox.DataSource is null)
            {
                EmployeeListBox.Items.Clear();
                EmployeeListBox.DisplayMember = "FullName";
                EmployeeListBox.ValueMember = "EmployeeId";
            }
        }

        /// <summary>
        /// Удаляет источник данных для таблицы операций ранее выбранного сотрудника, если список сотрудников пуст 
        /// </summary>
        private void ResetSelectedEmployee()
        {
            if (EmployeeListBox.DataSource is null && _selectedEmployee != null)
            {
                _selectedEmployee = null;
            }
        }

        private void UpdateActiveEmployeesIfChecked()
        {
            if (ActiveEmployeesCheckBox.Checked && !InactiveEmployeesCheckBox.Checked)
            {
                EmployeeListBox.DataSource = GetActiveEmployees();
            }
        }

        private List<Employee> GetAllEmployees()
        {
            return _employees.OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
        }

        private List<Employee> GetInactiveEmployees()
        {
            return _employees
                .Where(e => e.IsDismissed)
                .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
        }

        private List<Employee> GetActiveEmployees()
        {
            return _employees
                .Where(e => !e.IsDismissed)
                .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
        }
        #endregion

        #region Вывод операций по сотрудникам
        /// <summary>
        /// Изменяем доступность DTP в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool changePeriod = true;
            //Находим нужный DGV
            DateTimePicker dtp = (sender as CheckBox).Name == this.BeginDateCheckBox.Name ? BeginDateDTP : EndDateDTP;
            dtp.Enabled = !dtp.Enabled;

            //Заполняем таблицу операций.
            FillTheOperationDGV(changePeriod);
        }

        /// <summary>
        /// Выводим список операций соответствующих установленным требованиям по дате, сотруднику и типу операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesDTP_ValueChanged(object sender, EventArgs e)
        {
            bool changePeriod = true;
            FillTheOperationDGV(changePeriod); //Заполняем таблицу операций.
        }

        /// <summary>
        /// Заполняем таблицу операций для выделенного сотрудника.
        /// </summary>
        /// <param name="changePeriod"></param> - передаём true, если необходимо выбрать операции за другой период;
        ///                                       дефолт - период не изменяется при переключении между сотрудниками  
        private void FillTheOperationDGV(bool changePeriod = false)
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем список операций.
            OperationDetailsDGV.Rows.Clear();

            if (_selectedEmployee != null)
            {
                //Находим начальную и конечную дату требуемых операций.                
                DateTime? beginDate = BeginDateDTP.Enabled ? BeginDateDTP.Value : (DateTime?)null;
                DateTime? endDate = EndDateDTP.Enabled ? EndDateDTP.Value : (DateTime?)null;
                //Выводим список операций соответствующий заданным требованиям.
                var operList = _operationsCache.GetOperations(EmployeeListBox.SelectedItem as Employee, beginDate, endDate, changePeriod);
                if (operList.Any())
                {
                    FillTheOperationDGV(operList);                    
                }
                //Изменяем видимость строк по типу операции.
                OperationsCheckBox_CheckedChanged(null, null);
            }
        }

        /// <summary>
        /// Заполняет таблицу операций переданной инф-цией.
        /// </summary>
        /// <param name="operList">Инф-ция для заполнения таблицы.</param>
        private void FillTheOperationDGV(IList<IOperation> operList)
        {
            foreach (IOperation operat in operList.OrderByDescending(p => p.OperationDate))
            {
                int rowIndx = OperationsInfoDGV.Rows.Add();
                DataGridViewRow row = OperationsInfoDGV.Rows[rowIndx];

                row.Cells[OperationTypeCol.Index].Value = (operat.GetType() == typeof(Sale)) ? "Расход" : "Приход";
                row.DefaultCellStyle.BackColor = (operat.GetType() == typeof(Sale)) ? Color.LightGreen : Color.Khaki;//Color.Pink;
                row.Cells[OperationIdCol.Index].Value = operat.OperationId;
                row.Cells[DateCol.Index].Value = operat.OperationDate.ToShortDateString();
                row.Cells[EmployeeCol.Index].Value = (operat.Employee != null) ? operat.Employee.GetShortFullName() : null;
                row.Cells[ContragentCol.Index].Value = operat.Contragent.ContragentName;
                row.Cells[ContragentEmployeeCol.Index].Value = operat.ContragentEmployee;
                row.Cells[TotalSumCol.Index].Value = operat.OperationDetailsList.Sum(od => od.Sum);

                row.Tag = operat;
            }
        }

        /// <summary>
        /// Изменяет видимость строк по типу операции, в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Меняем видимость требуемых строк в зависимотси от установленных требований для данного типа операций.
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                row.Visible = (row.Cells[OperationTypeCol.Index].Value == "Приход" ? PurchaseCheckBox.Checked : SaleCheckBox.Checked);
            }
            //Выводим кол-во видимых строк.
            OperationsCountLabel.Text = _operationsCache.GetEmployeeOperationsCount(_selectedEmployee.EmployeeId).ToString();
        }

        private void EmployeeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedEmployee = EmployeeListBox.SelectedItem as Employee;
            FillTheOperationDGV(); //Заполняем таблицу операций.
        }

        private void OperationsInfoDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            OperationDetailsDGV.Rows.Clear(); //Очищаем таблицу доп. инф-ции от старых данных.

            //Если есть выбранная строка.
            if (OperationsInfoDGV.SelectedRows.Count != 0)
            {
                IOperation oper = (OperationsInfoDGV.Rows[e.RowIndex].Tag as IOperation);//Находим нужную операцию
                //Выводим инф-цию в таблицу доп. инф-ции по данной операции.
                FillTheOperationDetailsDGV(oper.OperationDetailsList);
            }
        }

        /// <summary>
        /// Заполняет таблицу доп. инф-ции по Операции данными из переданного списка.
        /// </summary>
        /// <param name="operDetList">Список операций для заполнения.</param>
        private void FillTheOperationDetailsDGV(IList<OperationDetails> operDetList)
        {
            foreach (OperationDetails operDet in operDetList)
            {
                int rowIndx = OperationDetailsDGV.Rows.Add();
                DataGridViewRow row = OperationDetailsDGV.Rows[rowIndx];

                row.Cells[ManufacturerCol.Index].Value = operDet.SparePart.Manufacturer;
                row.Cells[ArticulCol.Index].Value = operDet.SparePart.Articul;
                row.Cells[TitleCol.Index].Value = operDet.SparePart.Title;
                row.Cells[MeasureUnitCol.Index].Value = operDet.SparePart.MeasureUnit;
                row.Cells[CountCol.Index].Value = operDet.Count;
                row.Cells[PriceCol.Index].Value = operDet.Price;
                row.Cells[SumCol.Index].Value = operDet.Count * operDet.Price;
            }
        }
        #endregion
    }
}