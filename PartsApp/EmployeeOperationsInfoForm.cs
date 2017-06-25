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

namespace PartsApp
{
    public partial class EmployeeOperationsInfoForm : Form
    {
        public EmployeeOperationsInfoForm()
        {
            InitializeComponent();
        }

        private void EmployeeOperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Находим список всех сотрудников (сортируем по фамилии и имени) и делаем источником данных для ListBox.
            EmployeeListBox.DataSource = PartsDAL.FindEmployees().OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();
     
        }//EmployeeOperationsInfoForm_Load



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
        }//BeginDateCheckBox_CheckedChanged

        /// <summary>
        /// Выводим список операций соответствующих установленным требованиям по дате, сотруднику и типу операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesDTP_ValueChanged(object sender, EventArgs e)
        {
            //Находим начальную и конечную дату требуемых операций.
            DateTime? beginDate = BeginDateDTP.Enabled ? BeginDateDTP.Value : (DateTime?)null;
            DateTime? endDate   = EndDateDTP.Enabled   ? EndDateDTP.Value   : (DateTime?)null;
            //Выводим список операций соответствующий заданным требованиям.
            List<IOperation> operList = PartsDAL.FindOperations(EmployeeListBox.SelectedItem as Employee, beginDate, endDate);
            FillTheOperationDGV(operList);

            //Скрываем, если необходимо ненужный тип операций.
        }//DatesDTP_ValueChanged

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
                row.DefaultCellStyle.BackColor          = (operat.GetType() == typeof(Sale)) ? Color.LightGreen : Color.Khaki;//Color.Pink;
                row.Cells[OperationIdCol.Index].Value   = operat.OperationId;
                row.Cells[DateCol.Index].Value          = operat.OperationDate.ToShortDateString();
                row.Cells[EmployeeCol.Index].Value      = (operat.Employee != null) ? operat.Employee.GetShortFullName() : null;
                row.Cells[ContragentCol.Index].Value    = operat.Contragent.ContragentName;
                row.Cells[ContragentEmployeeCol.Index].Value = operat.ContragentEmployee;
            }//foreach
        }//FillTheOperationDGV

        /// <summary>
        /// Изменяет видимость строк по типу операции, в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Узнаём тип операции.
            CheckBox operCheckBox = sender as CheckBox;
            string operType = operCheckBox.Name == PurchaseCheckBox.Name ? "Приход" : "Расход";
            //Меняем видимость требуемых строк.
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Cells[OperationTypeCol.Index].Value.ToString() == operType);
                    row.Visible = operCheckBox.Checked;
            }//foreach
        }//


    }//EmployeeOperationsInfoForm

}//namespace
