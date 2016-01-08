﻿using System;
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
            FillTheOperationDGV(PartsDAL.FindAllPurchases());
            FillTheOperationDetailsDGV(PartsDAL.FindAllSparePartsAvaliableToDisplay());
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
            SetFilterPosition(OperationId, operationIdFilterTextBox);
            
        }//SetFiltersPosition
        /// <summary>
        /// Устанавливает размер и позицию заданного фильтующего контрола.
        /// </summary>
        /// <param name="column">Столбец в таблице, с которым ассоциируется контрол.</param>
        /// <param name="filterControl">Фильрующий контрол, размер и позиция которого задается.</param>
        private void SetFilterPosition(DataGridViewTextBoxColumn column, Control filterControl)
        {
            int x, y;
            x = operationDataGridView.GetCellDisplayRectangle(column.Index, -1, false).Location.X;
            y = operationSplitContainer.Panel1.Height - filterControl.Height;
            filterControl.Location = new Point(x, y);
            filterControl.Width = column.Width;
        }//SetFilterPosition
        /// <summary>
        /// Заполняет таблицу коллекцией переданных эл-тов.
        /// </summary>
        /// <param name="purchases">Коллекция, эл-тами которой заполняется таблица.</param>
        private void FillTheOperationDGV(IList<Purchase> purchases)
        {            
            operationDataGridView.Rows.Add(purchases.Count);

            for (int i = 0; i < purchases.Count; ++i)
            {
                Purchase purchase = purchases[i];
                DataGridViewRow row = operationDataGridView.Rows[i];

                row.Cells[Contragent.Name].Value = PartsDAL.FindSupplierById(purchase.SupplierId).ContragentName;

                if (purchase.EmployeeId != null)
                {
                    Employee employee = PartsDAL.FindEmployeeById((int)purchase.EmployeeId);
                    row.Cells[Employee.Name].Value = employee.GetShortFullName();
                    row.Cells[Employee.Name].ToolTipText = employee.GetFullName();
                    row.Cells[Employee.Name].Tag = employee.EmployeeId;
                }//if
                
                row.Cells[Date.Name].Value = purchase.PurchaseDate.ToShortDateString();
                row.Cells[InTotal.Name].Value = PartsDAL.FindTotalSumOfPurchase(purchase.PurchaseId);
                row.Cells[Currency.Name].Value = purchase.Currency;
                row.Cells[ExcRate.Name].Value = purchase.ExcRate;
                row.Cells[Description.Name].Value = purchase.Description;
                row.Cells[ContragentEmployee.Name].Value = purchase.SupplierEmployee;
                //row.Cells[Storage.Name].Value = purchase.;
                row.Cells[OperationId.Name].Value = purchase.PurchaseId;
            }//for


        }//FillTheOperationDGV
        /// <summary>
        /// Заполняет operationDetailsDGV коллекцией переданных эл-тов.
        /// </summary>
        /// <param name="spareParts">Коллекция, эл-тами которой заполняется таблица.</param>
        private void FillTheOperationDetailsDGV(IList<SparePart> spareParts)
        {
            operationDetailsDGV.Rows.Add(spareParts.Count);

            for (int i = 0; i < spareParts.Count; ++i)
            {
                SparePart sparePart = spareParts[i];
                DataGridViewRow row = operationDetailsDGV.Rows[i];

                row.Cells[Manufacturer.Name].Value = sparePart.Manufacturer;
                row.Cells[Articul.Name].Value = sparePart.Articul;
                row.Cells[Title.Name].Value = sparePart.Title;
                row.Cells[Unit.Name].Value = sparePart.Unit;
                row.Cells[Count.Name].Value = sparePart.Count;
                row.Cells[Price.Name].Value = sparePart.Price;
     /*!!!*/    row.Cells[Sum.Name].Value = sparePart.Price * sparePart.Count;
            }//for
        }//FillTheOperationDetailsDGV

        private void operationDataGridView_Resize(object sender, EventArgs e)
        {
            SetFiltersPosition();
        }

        #region Обработка фильтров запроса.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void FiltersRequestProcessing()
        {
            Purchase purchase = new Purchase();

            purchase.SupplierEmployee = contragentFilterTextBox.Text.Trim();
            purchase.EmployeeId = (!String.IsNullOrWhiteSpace(employeeFilterTextBox.Text)) ? Convert.ToInt32(employeeFilterTextBox.Text.Trim()) : (int?)null; 
            string operationId = operationIdFilterTextBox.Text.Trim();


            PartsDAL.FindPurchasesByParameters(operationId, employee, contragent);

        }//FiltersRequestProcessing

        private void contragentFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrWhiteSpace((sender as TextBox).Text) == false)
                    FiltersRequestProcessing();
                    
            }//if
        }//contragentFilterTextBox_KeyDown

        private void employeeFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }//if
        }//employeeFilterTextBox_KeyDown

        private void operationIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }//if
        }




























        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion




    }//OperationInfoForm
}//namespace
