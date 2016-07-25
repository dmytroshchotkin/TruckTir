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
    public partial class ContragentOperationsInfoForm : Form
    {
        /// <summary>
        /// Тип контрагента
        /// </summary>
        Type _contragType;

        public ContragentOperationsInfoForm(Type contragType)
        {
            InitializeComponent();
            _contragType = contragType; 
        }//

        private void ContragentOperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Заполняем таблицы инф-цией в зависимости от типа операции.
            List<IContragent> contragList = new List<IContragent>();
            if (_contragType == typeof(Supplier))
            {
                contragList = PartsDAL.FindSuppliers().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Поставщики";
                OperationsGroupBox.Text = "Поставки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по поставке.";
            }//if
            else
            {
                contragList = PartsDAL.FindCustomers().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Покупатели";
                OperationsGroupBox.Text = "Покупки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по покупке.";
            }//else

            ContragentsListBox.DataSource = contragList;  
        }//ContragentOperationsInfoForm_Load

        private void ContragentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {           
            //Находим Id выбранного контрагента.
            //int contragId = (int)ContragentsListBox.SelectedValue;   
            int contragId = ((IContragent)ContragentsListBox.SelectedValue).ContragentId;
            //Заполняем таблицу Операций.
            List<IOperation> operationsList = (_contragType == typeof(Supplier)) ? PartsDAL.FindPurchases(contragId) : PartsDAL.FindSales(contragId, null);
            FillTheOperationsInfoDGV(operationsList);
        }//

        /// <summary>
        /// Заполняет таблицу Операций данными из переданного списка.
        /// </summary>
        /// <param name="operationsList">Список операций для заполнения.</param>
        private void FillTheOperationsInfoDGV(List<IOperation> operationsList)
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем таблицу операций от старых данных.

            foreach (IOperation oper in operationsList)
            {
                OperationsInfoDGV.SelectionChanged -= OperationsInfoDGV_SelectionChanged;
                int rowIndx = OperationsInfoDGV.Rows.Add();                
                DataGridViewRow row = OperationsInfoDGV.Rows[rowIndx];

                row.Cells[OperationIdCol.Index].Value = oper.OperationId;
                row.Cells[DateCol.Index].Value = oper.OperationDate;
                row.Cells[EmployeeCol.Index].Value = (oper.Employee != null) ? oper.Employee.GetShortFullName() : null;
                row.Cells[ContragentEmployeeCol.Index].Value = oper.ContragentEmployee;

                OperationsInfoDGV.ClearSelection();
                OperationsInfoDGV.SelectionChanged += OperationsInfoDGV_SelectionChanged;
            }//foreach
            
        }//FillTheOperationsInfoDGV

        /// <summary>
        /// Выводим доп. инф-цию по выбранной операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsInfoDGV_SelectionChanged(object sender, EventArgs e)
        {
            OperationDetailsDGV.Rows.Clear(); //Очищаем таблицу доп. инф-ции от старых данных.
            //Если есть выбранная строка.
            if (OperationsInfoDGV.SelectedRows.Count != 0)
            {
                DataGridViewCell cell = OperationsInfoDGV.SelectedRows[0].Cells[OperationIdCol.Index];
                //Находим Id выбранной операции.
                int operId = (int)cell.Value;
                //Выводим инф-цию в таблицу доп. инф-ции по данной операции.
                var operDetList = (_contragType == typeof(Supplier)) ? PartsDAL.FindPurchaseDetails(operId) : PartsDAL.FindSaleDetails(operId);
                FillTheOperationDetailsDGV(operDetList);
            }//if
        }//OperationsInfoDGV_SelectionChanged

        /// <summary>
        /// Заполняет таблицу доп. инф-ции по Операции данными из переданного списка.
        /// </summary>
        /// <param name="operDetList">Список операций для заполнения.</param>
        private void FillTheOperationDetailsDGV(List<SparePart> operDetList)
        {            
            foreach (SparePart sparePart in operDetList)
            {
                int rowIndx = OperationDetailsDGV.Rows.Add();
                DataGridViewRow row = OperationDetailsDGV.Rows[rowIndx];

                row.Cells[ManufacturerCol.Index].Value = sparePart.Manufacturer;
                row.Cells[ArticulCol.Index].Value = sparePart.Articul;
                row.Cells[TitleCol.Index].Value = sparePart.Title;
                row.Cells[MeasureUnitCol.Index].Value = sparePart.Unit;
                row.Cells[CountCol.Index].Value = sparePart.Count;
                row.Cells[PriceCol.Index].Value = sparePart.Price;
                row.Cells[SumCol.Index].Value = sparePart.Count * sparePart.Price;
            }//foreach                          

        }//FillTheOperationDetailsDGV



    }//ContragentOperationsInfoForm

}//namespace
