using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PartsApp.Models;

namespace PartsApp
{
    /*Задания*/
    //Нумерация строк.
    //Вывод общего кол-ва строк в таблице.
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
        }

        private void ContragentOperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Стартовая инициализация формы.
            FormInitialize();
        }

        /// <summary>
        /// Метод стартовой инициализации формы.
        /// </summary>
        private void FormInitialize()
        {
            //Заполняем таблицы инф-цией в зависимости от типа операции.
            List<IContragent> contragList = null;
            if (_contragType == typeof(Supplier))
            {
                contragList = PartsDAL.FindSuppliers().Cast<IContragent>().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Поставщики";
                OperationsGroupBox.Text = "Поставки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по поставкам.";
            }
            else
            {
                contragList = PartsDAL.FindCustomers().Cast<IContragent>().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Покупатели";
                OperationsGroupBox.Text = "Покупки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по покупкам.";
            }
            //Заполняем лист контрагентами.
            foreach (IContragent contrag in contragList)
            {
                ListViewItem item = new ListViewItem(contrag.ContragentName);
                item.SubItems.Add(contrag.Balance == null ? null : ((double)contrag.Balance).ToString("0.00"));
                item.Tag = contrag;
                ContragentsListView.Items.Add(item);
            }
        }

        private void ContragentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ContragentsListView.SelectedItems.Count != 0)
            {
                //int contragId = (int)ContragentsListBox.SelectedValue;
                int contragId = (ContragentsListView.SelectedItems[0].Tag as IContragent).ContragentId;
                //Если инф-ции об операциях данного контрагента ещё нет в коллекции, находим её в базе и добавляем в коллекцию.                
                if (_contragentsOperations.TryGetValue(contragId, out List<IOperation> operList) == false)
                {
                    if (operList == null)
                    {
                        operList = new List<IOperation>();
                        if (_contragType == typeof(Supplier))
                        {
                            var purchases = PartsDAL.FindPurchases(contragId, null);
                            operList.AddRange(purchases);
                        }

                        if (_contragType == typeof(Customer))
                        {
                            var sales = PartsDAL.FindSales(contragId, null);
                            operList.AddRange(sales);
                        }

                        _contragentsOperations.Add(contragId, operList);//добавляем в коллекцию.    
                    }
                }
                FillTheOperationsInfoDGV(operList); //Заполняем таблицу Операций.
            }
        }

        private void ContragentsListBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Если ПКМ по выделенному объекту, выводим контекстное меню.
            if (e.Button == MouseButtons.Right)
            {
                Rectangle rect = ContragentsListView.GetItemRect(ContragentsListView.SelectedIndices[0]);
                rect.Y += ContragentsGroupBox.Location.Y;

                if (e.Y >= rect.Top && e.Y <= rect.Bottom)
                    editContragentContextMenuStrip.Show(ContragentsListView, e.Location, ToolStripDropDownDirection.BelowRight);
            }
        }

        private void EditContragentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContragentsListView.SelectedItems[0].Tag is IContragent contragent)
            {
                if (contragent is Supplier)
                {
                    contragent = PartsDAL.FindSuppliers(contragent.ContragentId);
                }

                if (contragent is Customer)
                {
                    contragent = PartsDAL.FindCustomers(contragent.ContragentId);
                }

                //Передаём в форму 'свежую'инф-цию из базы, на случай если она обновилась.
                new AddContragentForm(contragent).Show();
            }
        }


        private void OperationsInfoDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Если ПКМ по ячейке столбца 'Комментарий', выводи контекстное меню.
            if (e.Button == MouseButtons.Right && e.ColumnIndex == DescriptionCol.Index)
            {
                Rectangle rect = OperationsInfoDGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point loc = new Point(rect.Left + e.X, rect.Top + e.Y);
                editOperDescriptContextMenuStrip.Show(OperationsInfoDGV, loc, ToolStripDropDownDirection.BelowRight);
                OperationsInfoDGV.Rows[e.RowIndex].Cells[DescriptionCol.Index].Selected = true;
            }
        }

        private void editOperDescriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Открываем ячейку для редактирования. (Вся строка становится не readonly)
            DataGridViewCell cell = OperationsInfoDGV.SelectedRows[0].Cells[DescriptionCol.Index];
            cell.ReadOnly = false;
            OperationsInfoDGV.BeginEdit(false);  //Активируем для редактирования
        }

        private void OperationsInfoDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Делаем недоступными для редактирования все ячейки столбца 'Комментарий'.
            OperationsInfoDGV.Columns[DescriptionCol.Index].ReadOnly = true;

            //Обновляем запись в базе.
            DataGridViewCell cell = OperationsInfoDGV[e.ColumnIndex, e.RowIndex];
            int operId = (int)OperationsInfoDGV[OperationIdCol.Index, e.RowIndex].Value;
            string descr = (cell.Value == null || String.IsNullOrWhiteSpace(cell.Value.ToString())) ? null : cell.Value.ToString().Trim();
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_contragType == typeof(Supplier))
                    PartsDAL.UpdatePurchase(operId, descr);
                else
                    PartsDAL.UpdateSale(operId, descr);

                (cell.OwningRow.Tag as IOperation).Description = descr;
                Cursor = Cursors.Default;
            }
            catch
            {
                MessageBox.Show("Не удалось редактировать запись. Попробуйте ещё раз.");
                cell.Value = (cell.OwningRow.Tag as IOperation).Description;
            }
        }

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

                row.Tag = oper;
                row.Cells[OperationIdCol.Index].Value = oper.OperationId;
                row.Cells[DateCol.Index].Value = oper.OperationDate;
                row.Cells[EmployeeCol.Index].Value = (oper.Employee != null) ? oper.Employee.GetShortFullName() : null;
                row.Cells[ContragentEmployeeCol.Index].Value = oper.ContragentEmployee;
                row.Cells[DescriptionCol.Index].Value = oper.Description;
                row.Cells[TotalSumCol.Index].Value = oper.OperationDetailsList.Sum(od => od.Sum);

                OperationsInfoDGV.ClearSelection();
                OperationsInfoDGV.SelectionChanged += OperationsInfoDGV_SelectionChanged;
            }
        }

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
                int contragId = (ContragentsListView.SelectedItems[0].Tag as IContragent).ContragentId;
                IOperation oper = _contragentsOperations[contragId].First(op => op.OperationId == operId); //Находим нужную операцию

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
    }
}
/*Будущие задачи*/
//Сделать выбор периода за кот. ищется инф-ция.