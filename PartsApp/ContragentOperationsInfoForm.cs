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
        /// <summary>
        /// Ключ - Id контрагента. Значение - список операций данного контрагента.
        /// </summary>
        Dictionary<int, List<IOperation>> _contragentsOperations;


        public ContragentOperationsInfoForm(Type contragType)
        {
            InitializeComponent();

            _contragType = contragType;
            _contragentsOperations = new Dictionary<int, List<IOperation>>();
        }//


        private void ContragentOperationsInfoForm_Load(object sender, EventArgs e)
        {            
            //Стартовая инициализация формы.
            FormInitialize();
        }//ContragentOperationsInfoForm_Load

        /// <summary>
        /// Метод стартовой инициализации формы.
        /// </summary>
        private void FormInitialize()
        {            
            //Заполняем таблицы инф-цией в зависимости от типа операции.
            List<IContragent> contragList = null;
            if (_contragType == typeof(Supplier))
            {
                contragList = PartsDAL.FindSuppliers().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Поставщики";
                OperationsGroupBox.Text = "Поставки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по поставкам.";
            }//if
            else
            {
                contragList = PartsDAL.FindCustomers().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Покупатели";
                OperationsGroupBox.Text = "Покупки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по покупкам.";
            }//else
            
            ContragentsListBox.DataSource = contragList;            
        }//FormInitialize

        private void ContragentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int contragId = (int)ContragentsListBox.SelectedValue;

            //Если инф-ции об операциях данного контрагента ещё нет в коллекции, находим её в базе и добавляем в коллекцию.
            List<IOperation> operList;
            if (_contragentsOperations.TryGetValue(contragId, out operList) == false)
            {
                //Заполняем таблицу Операций.
                List<IOperation> operationsList = (_contragType == typeof(Supplier)) ? PartsDAL.FindPurchases(contragId) : PartsDAL.FindSales(contragId, null);
                FillTheOperationsInfoDGV(operationsList);
                _contragentsOperations.Add(contragId, operationsList);//добавляем в коллекцию.                
            }//if
            else
                FillTheOperationsInfoDGV(operList);
        }//ContragentsListBox_SelectedIndexChanged

        /// <summary>
        /// Заполняет таблицу Операций данными из переданного списка.
        /// </summary>
        /// <param name="operationsList">Список операций для заполнения.</param>
        private void FillTheOperationsInfoDGV(List<IOperation> operationsList)
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем таблицу операций от старых данных.

            foreach (IOperation oper in operationsList)
            {
                OperationsInfoDGV.SelectionChanged -= OperationsInfoDGV_SelectionChanged; //Отключаем вызов события, т.к. оно инициируется сразу при добавлении строки в таблицу, что вызывает ошибку выполнения.
                int rowIndx = OperationsInfoDGV.Rows.Add();                
                DataGridViewRow row = OperationsInfoDGV.Rows[rowIndx];

                row.Cells[OperationIdCol.Index].Value = oper.OperationId;
                row.Cells[DateCol.Index].Value = oper.OperationDate;
                row.Cells[EmployeeCol.Index].Value = (oper.Employee != null) ? oper.Employee.GetShortFullName() : null;
                row.Cells[ContragentEmployeeCol.Index].Value = oper.ContragentEmployee;
                row.Cells[TotalSumCol.Index].Value = oper.OperationDetails.Sum(sp => sp.Price * sp.Count);

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
                int operId = (int)OperationsInfoDGV.SelectedRows[0].Cells[OperationIdCol.Index].Value; //Находим Id выбранной операции.                
                int contragId = (int)ContragentsListBox.SelectedValue;
                IOperation oper = _contragentsOperations[contragId].First(op => op.OperationId == operId); //Находим нужную операцию
                
                //Выводим инф-цию в таблицу доп. инф-ции по данной операции.
                FillTheOperationDetailsDGV(oper.OperationDetails);
            }//if
        }//OperationsInfoDGV_SelectionChanged

        /// <summary>
        /// Заполняет таблицу доп. инф-ции по Операции данными из переданного списка.
        /// </summary>
        /// <param name="operDetList">Список операций для заполнения.</param>
        private void FillTheOperationDetailsDGV(IList<SparePart> operDetList)
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
