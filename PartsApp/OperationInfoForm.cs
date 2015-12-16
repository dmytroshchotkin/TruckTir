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
    public partial class OperationInfoForm : Form
    {
        public OperationInfoForm()
        {
            InitializeComponent();
        }

        private void OperationInfoForm_Load(object sender, EventArgs e)
        {
            operationDateFilterTimePicker.Value = DateTime.Now;
            SetFiltersPosition();

        }//OperationInfoForm_Load

        /// <summary>
        /// Устанавливает размер и позицию всех фильтрующих поиск контролов.
        /// </summary>
        private void SetFiltersPosition()
        {
            SetFilterPosition(Contragent, contragentFilterTextBox);
            SetFilterPosition(Employee,   employeeFilterTextBox);
            SetFilterPosition(Date,       operationDateFilterTimePicker);
            SetFilterPosition(Currency,   currencyFilterComboBox);
            SetFilterPosition(Storage,    storageFilterComboBox);

            //int x, y;
            //x = operationDataGridView.GetCellDisplayRectangle(Contragent.Index, 0, false).Location.X;
            //y = operationSplitContainer.Panel1.Height - contragentFilterTextBox.Height;
            //contragentFilterTextBox.Location = new Point(x, y);
            //contragentFilterTextBox.Width = Contragent.Width;

            //x = operationDataGridView.GetCellDisplayRectangle(Employee.Index, 0, false).Location.X;
            //y = operationSplitContainer.Panel1.Height - employeeFilterTextBox.Height;
            //employeeFilterTextBox.Location = new Point(x, y);
            //employeeFilterTextBox.Width = Employee.Width;

            //x = operationDataGridView.GetCellDisplayRectangle(Date.Index, 0, false).Location.X;
            //y = operationSplitContainer.Panel1.Height - operationDateFilterTimePicker.Height;
            //operationDateFilterTimePicker.Location = new Point(x, y);
            //operationDateFilterTimePicker.Width = Date.Width;

            //x = operationDataGridView.GetCellDisplayRectangle(Currency.Index, 0, false).Location.X;
            //y = operationSplitContainer.Panel1.Height - currencyFilterComboBox.Height;
            //currencyFilterComboBox.Location = new Point(x, y);
            //currencyFilterComboBox.Width = Currency.Width;

            //x = operationDataGridView.GetCellDisplayRectangle(Storage.Index, 0, false).Location.X;
            //y = operationSplitContainer.Panel1.Height - storageFilterComboBox.Height;
            //storageFilterComboBox.Location = new Point(x, y);
            //storageFilterComboBox.Width = Storage.Width;
        }//SetFiltersPosition
        /// <summary>
        /// Устанавливает размер и позицию заданного фильтующего контрола.
        /// </summary>
        /// <param name="column">Столбец в таблице, с которым ассоциируется контрол.</param>
        /// <param name="filterControl">Фильрующий контрол, размер и позиция которого задается.</param>
        private void SetFilterPosition(DataGridViewTextBoxColumn column, Control filterControl)
        {
            int x, y;
            x = operationDataGridView.GetCellDisplayRectangle(column.Index, 0, false).Location.X;
            y = operationSplitContainer.Panel1.Height - filterControl.Height;
            filterControl.Location = new Point(x, y);
            filterControl.Width = column.Width;
        }//SetFilterPosition

        private void FillTheOperationDGV(IList<Purchase> purchases)
        {            
            operationDataGridView.Rows.Add(purchases.Count);

            for (int i = 0; i < purchases.Count; ++i )
            {
                operationDataGridView.Rows[i].Cells[Contragent.Name].Value = PartsDAL.FindSupplierByPurchaseId(purchases[i].SupplierId).ContragentName;
            }//for



        }//FillTheOperationDGV

        private void operationDataGridView_Resize(object sender, EventArgs e)
        {
            SetFiltersPosition();
        }



    }//OperationInfoForm
}//namespace
