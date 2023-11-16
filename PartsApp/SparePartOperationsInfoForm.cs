﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Storage;
using PartsApp.Models;

namespace PartsApp
{
    public partial class SparePartOperationsInfoForm : Form
    {
        public SparePartOperationsInfoForm()
        {
            InitializeComponent();
        }//

        public SparePartOperationsInfoForm(SparePart sparePart)
        {
            InitializeComponent();

            List<IOperation> operList = PartsDAL.FindOperations(sparePart);
            
            //Заполняем таблицу.
            FillTheOperationDGV(operList, sparePart.SparePartId);
        }//

        /// <summary>
        /// Заполняет таблицу операций переданной инф-цией.
        /// </summary>
        /// <param name="operList">Инф-ция для заполнения таблицы.</param>
        /// <param name="operList">Ид выводимого в таблицу товара.</param>
        private void FillTheOperationDGV(IList<IOperation> operList, int sparePartId)
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

                OperationDetails operDet = operat.OperationDetailsList.First(od => od.SparePart.SparePartId == sparePartId);
                row.Cells[UnitCol.Index].Value  = operDet.SparePart.MeasureUnit;
                row.Cells[CountCol.Index].Value = operDet.Count;
                row.Cells[PriceCol.Index].Value = operDet.Price;
                row.Cells[SumCol.Index].Value   = operDet.Price * operDet.Count;

                //Выводим название и артикул запчасти.
                ArticulLabel.Text = operDet.SparePart.Articul;
                TitleLabel.Text   = operDet.SparePart.Title;
            }//foreach
        }//FillTheOperationDGV

        private void SaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Cells[OperationTypeCol.Index].Value.ToString() == "Расход")
                    row.Visible = SaleCheckBox.Checked;
            }//foreach

        }//SaleCheckBox_CheckedChanged

        private void PurchaseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Cells[OperationTypeCol.Index].Value.ToString() == "Приход")
                    row.Visible = PurchaseCheckBox.Checked;
            }//foreach
        }








    }//SparePartOperationsInfoForm
}//namespace


/**/
//Вывод даты вместе со временем.