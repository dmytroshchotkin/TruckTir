using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PartsApp.OperationsDataCheck;
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
        List<IContragent> _contragents = new List<IContragent>();


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
            if (_contragType == typeof(Supplier))
            {
                _contragents = PartsDAL.FindSuppliers().Cast<IContragent>().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Поставщики";
                OperationsGroupBox.Text = "Поставки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по поставкам.";
            }
            else
            {
                _contragents = PartsDAL.FindCustomers().Cast<IContragent>().OrderBy(s => s.ContragentName).ToList();
                ContragentsGroupBox.Text = "Покупатели";
                OperationsGroupBox.Text = "Покупки";
                OperationDetailsGroupBox.Text = "Доп. инф-ция по покупкам.";
                AddPaidCashColumnToSalesOperationsGroupBox();
            }

            EnabledContragentsCheckBox.Checked = true;            
        }

        private void AddPaidCashColumnToSalesOperationsGroupBox()
        {
            OperationsInfoDGV.Columns.Add(PaidCashCol);
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
            // предотвращаем IndexOutOfRangeException, если клик по таблице без выбранных ячеек
            // закрываем редактирование "поставщика", зарезервированного для оформления возвратов
            if (e.Button != MouseButtons.Right || ContragentsListView.SelectedItems.Count == 0 || !CanEditContragent())
            {
                return;
            }

            var contragent = GetSelectedContragentFromDB();
            HandleToolStripMenuOptions(contragent.Enabled);
            DisplayToolStripMenuBelowSelectedContragent(e);
        }

        private bool CanEditContragent()
        {
            var contragent = ContragentsListView.SelectedItems[0].Tag as IContragent;
            return DataCheck.CanEditContragent(contragent);            
        }

        private void HandleToolStripMenuOptions(bool contragentEnabled)
        {
            if (!contragentEnabled)
            {
                disableContragentToolStripMenuItem.Visible = false;
                enableContragentToolStripMenuItem.Visible = true;
            }
            else
            {
                disableContragentToolStripMenuItem.Visible = true;
                enableContragentToolStripMenuItem.Visible = false;
            }            
        }

        private void DisplayToolStripMenuBelowSelectedContragent(MouseEventArgs e)
        {
            Rectangle rect = ContragentsListView.GetItemRect(ContragentsListView.SelectedIndices[0]);
            rect.Y += ContragentsGroupBox.Location.Y;

            if (e.Y >= rect.Top && e.Y <= rect.Bottom)
            {
                editContragentContextMenuStrip.Show(ContragentsListView, e.Location, ToolStripDropDownDirection.BelowRight);
            }
        }

        private void EditContragentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var contragent = GetSelectedContragentFromDB();

            //Передаём в форму 'свежую'инф-цию из базы, на случай если она обновилась.
            new AddContragentForm(contragent).Show();            
        }

        private void OnDisableOrEnableContragentToolStripMenuItemClick(object sender, EventArgs e)
        {
            var contragent = GetSelectedContragentFromCache();
            var input = contragent.Enabled ? ShowDisableContragentMessageBox(contragent) : ShowEnableContragentMessageBox(contragent);

            if (input == DialogResult.Yes)
            {
                UpdateContragentEnability(contragent);
            }
        }

        private void UpdateContragentEnability(IContragent contragent)
        {
            try
            {
                if (contragent.Enabled)
                {
                    PartsDAL.DisableContragent(contragent);
                }
                else
                {
                    PartsDAL.EnableContragent(contragent);
                }
                UpdateContragentData(contragent);
                SetContentsOfContragentsListView(GetContragents());
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка обновления данных, повторите попытку позже");
            }
        }

        private void UpdateContragentData(IContragent contragent)
        {
            _contragents.Remove(_contragents.Find(c => c.ContragentId == contragent.ContragentId));
            _contragents.Add(GetContragentFromDB(contragent));
        }

        private DialogResult ShowDisableContragentMessageBox(IContragent contragent)
        {
            return MessageBox.Show(
                    $"Заблокировать {(contragent is Supplier ? "поставщика" : "покупателя")} {contragent.ContragentName}?" +
                    $"\n\nПосле блокировки этот контрагент станет недоступен для проведения новых операций.",
                    "Блокировка контрагента",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
        }        

        private DialogResult ShowEnableContragentMessageBox(IContragent contragent)
        {
            return MessageBox.Show(
                    $"Разблокировать {(contragent is Supplier ? "поставщика" : "покупателя")} {contragent.ContragentName}?",
                    "Разблокировка контрагента",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
        }

        private IContragent GetSelectedContragentFromDB()
        {
            if (ContragentsListView.SelectedItems[0].Tag is IContragent contragent)
            {
                return GetContragentFromDB(contragent);
            }

            return default;
        }

        private IContragent GetContragentFromDB(IContragent contragent)
        {
            if (contragent is Supplier)
            {
                return PartsDAL.FindSuppliers(contragent.ContragentId);
            }

            if (contragent is Customer)
            {
                return PartsDAL.FindCustomers(contragent.ContragentId);
            }

            return default;
        }

        private IContragent GetSelectedContragentFromCache()
        {
            if (ContragentsListView.SelectedItems[0].Tag is IContragent contragent)
            {
                return _contragents.Find(c => c.ContragentId == contragent.ContragentId);
            }

            return default;
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
                {
                    PartsDAL.UpdatePurchase(operId, descr);
                }
                else
                {
                    PartsDAL.UpdateSale(operId, descr);
                }

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
            FillPaidCashColumnInOperationsInfoDGVForSales();
        }

        private void FillPaidCashColumnInOperationsInfoDGVForSales()
        {
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                if (row.Tag is Sale sale)
                {
                    string paidCash = sale.PaidCash ? "нал." : "безнал.";
                    row.Cells[PaidCashCol.Index].Value = paidCash;
                }
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

        private void OnContragentsCheckBoxesCheckedChanged(object sender, EventArgs e)
        {
            var contragents = GetContragents();
            SetContentsOfContragentsListView(contragents);
        }

       private List<IContragent> GetContragents()
        {
            return GetContragents(EnabledContragentsCheckBox.Checked, DisabledContragentsCheckBox.Checked);
        }

        private List<IContragent> GetContragents(bool selectEnabledContragents, bool selectDisabledContragents)
        {
            var contragents = new List<IContragent>();

            if (selectEnabledContragents)
            {
                contragents.AddRange(GetEnabledContragentsFromCache());
            }

            if (selectDisabledContragents)
            {
                contragents.AddRange(GetDisabledContragentsFromCache());
            }

            return contragents;
        }

        private List<IContragent> GetEnabledContragentsFromCache()
        {
            return _contragents
                .Where(c => c.Enabled)
                .OrderBy(c => c.ContragentName).ToList();
        }

        private List<IContragent> GetDisabledContragentsFromCache()
        {
            return _contragents
                .Where(c => !c.Enabled)
                .OrderBy(c => c.ContragentName).ToList();
        }

        private void SetContentsOfContragentsListView(List<IContragent> contragents)
        {
            ContragentsListView.Items.Clear();
            OperationsInfoDGV.Rows.Clear();

            foreach (var c in contragents)
            {
                var item = new ListViewItem(c.ContragentName) { Tag = c };
                item.SubItems.Add(c.Balance.ToString("0.00"));
                ContragentsListView.Items.Add(item);
            }
        }
    }
}
/*Будущие задачи*/
//Сделать выбор периода за кот. ищется инф-ция.