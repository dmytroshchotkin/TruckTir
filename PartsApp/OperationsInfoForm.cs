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
    public partial class OperationsInfoForm : Form
    {
        public OperationsInfoForm()
        {
            InitializeComponent();
        }

        private void OperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Устанавливаем стартовый период в месяц. (Убираем а затем добавляем событие ValueChanged на BeginDateDTP, для того чтобы метод DatesDTP_ValueChanged не вызвался дважды.)
            BeginDateDTP.ValueChanged -= new System.EventHandler(this.DatesDTP_ValueChanged);
            BeginDateDTP.Value = DateTime.Today.AddMonths(-1);
            EndDateDTP.Value = DateTime.Now;
            BeginDateDTP.ValueChanged += new System.EventHandler(this.DatesDTP_ValueChanged);
        }//

        /// <summary>
        /// Выводим список операций соответствующих установленным требованиям по дате, и типу операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesDTP_ValueChanged(object sender, EventArgs e)
        {
            FillTheOperationDGV(); //Заполняем таблицу операций.
        }//DatesDTP_ValueChanged


        /// <summary>
        /// Заполняем таблицу операций для выделенного сотрудника.
        /// </summary>
        private void FillTheOperationDGV()
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем список операций.

            //Находим начальную и конечную дату требуемых операций.
            DateTime? beginDate = BeginDateDTP.Enabled ? BeginDateDTP.Value : (DateTime?)null;
            DateTime? endDate = EndDateDTP.Enabled ? EndDateDTP.Value : (DateTime?)null;
            //Выводим список операций соответствующий заданным требованиям.
            List<IOperation> operList = PartsDAL.FindOperations(beginDate, endDate);
            FillTheOperationDGV(operList);

            //Изменяем видимость строк по типу операции.
            OperationsCheckBox_CheckedChanged(null, null);
        }//FillTheOperationDGV

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
            }//foreach
        }//FillTheOperationDGV

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
            }//foreach

            //Выводим кол-во видимых строк.
            OperationsCoubtLabel.Text = OperationsInfoDGV.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
        }//OperationsCheckBox_CheckedChanged    

        /// <summary>
        /// Осуществляет изменения данных в таблице деталей операции в зависимости от выбранной операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsInfoDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            OperationDetailsDGV.Rows.Clear(); //Очищаем таблицу доп. инф-ции от старых данных.

            //Если есть выбранная строка.
            if (OperationsInfoDGV.SelectedRows.Count != 0)
            {
                IOperation oper = (OperationsInfoDGV.Rows[e.RowIndex].Tag as IOperation);//Находим нужную операцию
                //Выводим инф-цию в таблицу доп. инф-ции по данной операции.
                FillTheOperationDetailsDGV(oper.OperationDetailsList);
            }//if
        }//OperationsInfoDGV_RowEnter

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
            }//foreach
        }//FillTheOperationDetailsDGV

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
        }//BeginDateCheckBox_CheckedChanged

    }//OperationsInfoForm
}//namespace
