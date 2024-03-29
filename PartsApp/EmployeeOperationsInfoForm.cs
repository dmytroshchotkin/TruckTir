﻿using Infrastructure.Storage;
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
using Infrastructure.Storage.Repositories;

namespace PartsApp
{
    public partial class EmployeeOperationsInfoForm : Form
    {
        private List<Employee> _employees;
        private Employee _selectedEmployee;

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
                HandleToolStripMenuOptions(_selectedEmployee.IsDisabled);
            }
        }

        private void HandleToolStripMenuOptions(bool isDisabled)
        {
            if (isDisabled)
            {
                DisableToolStripMenuItem.Visible = false;
                EnableToolStripMenuItem.Visible = true;
            }
            else
            {
                DisableToolStripMenuItem.Visible = true;
                EnableToolStripMenuItem.Visible = false;
            }

            EmployeeEditingContextMenu.Show();
        }

        private void EnableEditingContextMenu()
        {
            EmployeeEditingContextMenu.Items.Add(EditToolStripMenuItem);
            EmployeeEditingContextMenu.Items.Add(DisableToolStripMenuItem);
            EmployeeEditingContextMenu.Items.Add(EnableToolStripMenuItem);
            EmployeeListBox.MouseDown += new MouseEventHandler(OnEmployeeListBoxMouseDown);
        }

        private void OnDisableOptionClick(object sender, EventArgs e)
        {
            if (_selectedEmployee != null)
            {
                var disableEmployeeForm = new DisableEmployeeForm(_selectedEmployee);
                disableEmployeeForm.ShowDialog();

                if (_selectedEmployee.IsDisabled)
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

                SetContentsOfEmployeeListBox(GetEmployees());
            }
        }

        private void OnEnableOptionClick(object sender, EventArgs e)
        {
            if (_selectedEmployee != null)
            {
                var input = ShowEnableUserMessageBox();
                if (input == DialogResult.Yes)
                {
                    _selectedEmployee.DisableDate = default;

                    try
                    {
                        PartsDAL.UpdateEmployeeWithoutPassword(_selectedEmployee);
                        SetContentsOfEmployeeListBox(GetEmployees());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка обновления данных, повторите попытку позже");
                    }
                }
            }
        }

        private DialogResult ShowEnableUserMessageBox()
        {
            return MessageBox.Show(
                    $"Вернуть доступ сотруднику {_selectedEmployee.FullName}? " +
                    $"\n\nПраво доступа: {_selectedEmployee.AccessLayer}" +
                    $"\nПринят на работу: {_selectedEmployee.HireDate?.ToString("d")}" +
                    $"\nУволен: {_selectedEmployee.DisableDate?.ToString("d")}",

                    "Восстановление доступа",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
        }

        private void OnEmployeesCheckBoxesCheckedChanged(object sender, EventArgs e)
        {
            var employees = GetEmployees();
            SetContentsOfEmployeeListBox(employees);
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

        private List<Employee> GetEmployees()
        {
            return GetEmployees(ActiveEmployeesCheckBox.Checked, InactiveEmployeesCheckBox.Checked);
        }

        /// <summary>
        /// Заполняет listbox сотрудниками выбранного типа
        /// </summary>
        private void SetContentsOfEmployeeListBox(List<Employee> employees)
        {
            EmployeeListBox.DataSource = employees.Any() ? employees : null;
            ClearEmployeeListBox();
            ResetSelectedEmployee();
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

        private List<Employee> GetInactiveEmployees()
        {
            return _employees
                .Where(e => e.IsDisabled)
                .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
        }

        private List<Employee> GetActiveEmployees()
        {
            return _employees
                .Where(e => !e.IsDisabled)
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
            //Находим нужный DGV
            DateTimePicker dtp = (sender as CheckBox).Name == this.BeginDateCheckBox.Name ? BeginDateDTP : EndDateDTP;
            dtp.Enabled = !dtp.Enabled;

            //Заполняем таблицу операций.
            FillTheOperationDGV();
        }

        /// <summary>
        /// Выводим список операций соответствующих установленным требованиям по дате, сотруднику и типу операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesDTP_ValueChanged(object sender, EventArgs e)
        {
            FillTheOperationDGV(); //Заполняем таблицу операций.
        }

        /// <summary>
        /// Заполняем таблицу операций для выделенного сотрудника.
        /// </summary>
        private void FillTheOperationDGV()
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем список операций.
            OperationDetailsDGV.Rows.Clear();

            if (_selectedEmployee != null)
            {
                //Находим начальную и конечную дату требуемых операций.
                DateTime? beginDate = BeginDateDTP.Enabled ? BeginDateDTP.Value : (DateTime?)null;
                DateTime? endDate = EndDateDTP.Enabled ? EndDateDTP.Value : (DateTime?)null;
                //Выводим список операций соответствующий заданным требованиям.
                List<IOperation> operList = FindOperations(EmployeeListBox.SelectedItem as Employee, beginDate, endDate);
                FillTheOperationDGV(operList);

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
            OperationsCoubtLabel.Text = OperationsInfoDGV.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
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

        /// <summary>
        /// Возвращает список всех операций осуществлённых данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        private static List<IOperation> FindOperations(Employee emp, DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operationsList = new List<IOperation>();

            PurchaseRepository.FindPurchases(emp, startDate, endDate).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            SaleRepository.FindSales(emp, startDate, endDate).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }
        #endregion
    }
}