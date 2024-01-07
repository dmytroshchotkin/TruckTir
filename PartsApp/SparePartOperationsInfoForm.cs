using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Storage;
using Infrastructure.Storage.Repositories;
using PartsApp.Models;

namespace PartsApp
{
    public partial class SparePartOperationsInfoForm : Form
    {
        public SparePartOperationsInfoForm()
        {
            InitializeComponent();
        }

        public SparePartOperationsInfoForm(SparePart sparePart)
        {
            InitializeComponent();

            List<IOperation> operList = FindOperations(sparePart);

            //Заполняем таблицу.
            FillTheOperationDGV(operList, sparePart.SparePartId);
        }

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
                row.DefaultCellStyle.BackColor = (operat.GetType() == typeof(Sale)) ? Color.LightGreen : Color.Khaki;//Color.Pink;
                row.Cells[OperationIdCol.Index].Value = operat.OperationId;
                row.Cells[DateCol.Index].Value = operat.OperationDate.ToShortDateString();
                row.Cells[EmployeeCol.Index].Value = (operat.Employee != null) ? operat.Employee.GetShortFullName() : null;
                row.Cells[ContragentCol.Index].Value = operat.Contragent.ContragentName;
                row.Cells[ContragentEmployeeCol.Index].Value = operat.ContragentEmployee;

                OperationDetails operDet = operat.OperationDetailsList.First(od => od.SparePart.SparePartId == sparePartId);
                row.Cells[UnitCol.Index].Value = operDet.SparePart.MeasureUnit;
                row.Cells[CountCol.Index].Value = operDet.Count;
                row.Cells[PriceCol.Index].Value = operDet.Price;
                row.Cells[SumCol.Index].Value = operDet.Price * operDet.Count;

                //Выводим название и артикул запчасти.
                ArticulLabel.Text = operDet.SparePart.Articul;
                TitleLabel.Text = operDet.SparePart.Title;
            }
        }

        private void SaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Cells[OperationTypeCol.Index].Value.ToString() == "Расход")
                {
                    row.Visible = SaleCheckBox.Checked;
                }
            }
        }

        private void PurchaseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Cells[OperationTypeCol.Index].Value.ToString() == "Приход")
                {
                    row.Visible = PurchaseCheckBox.Checked;
                }
            }
        }

        /// <summary>
        /// Возвращает список всех операций производимых с заданным товаром.
        /// </summary>
        /// <param name="sparePartId">Ид искомого товара.</param>
        /// <returns></returns>
        private static List<IOperation> FindOperations(SparePart sparePart)
        {
            List<IOperation> operationsList = new List<IOperation>();

            PurchaseRepository.FindPurchases(sparePart).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            SaleRepository.FindSales(sparePart).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }
    }
}

/**/
//Вывод даты вместе со временем.