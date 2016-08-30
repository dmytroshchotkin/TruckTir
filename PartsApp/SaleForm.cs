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
using PartsApp.SupportClasses;
using Excel = Microsoft.Office.Interop.Excel;

namespace PartsApp
{
    /*Задания*/
    //Изменить имена столбцов.
    //Убрать столбец extPrice из доп. таблицы.
    //Передавать inTotal в метод распечатки в Excel.
    //Добавить столбец 'Производитель'? и поиск по нему.

    public partial class SaleForm : Form
    {
        /// <summary>
        /// Список продаваемого товара, по конкретным приходам.
        /// </summary>
        List<OperationDetails> _operDetList = new List<OperationDetails>();
        /// <summary>
        /// Последняя редактируемая ячейка.
        /// </summary>
        DataGridViewCell _lastEditCell;
        /// <summary>
        /// Переменная для хранения инф-ции о том была ли ошибка редактирования ячейки.
        /// </summary>
        bool _isCellEditError = false;


        public SaleForm()
        {
            InitializeComponent();
        }//

        private void SaleForm_Load(object sender, EventArgs e)
        {
            //Устанавливаем даты для DateTimePicker.
            saleDateTimePicker.MaxDate = DateTime.Now.Date.AddDays(7);
            saleDateTimePicker.MinDate = saleDateTimePicker.Value = DateTime.Now;

            //Заполняем список автоподстановки для ввода контрагента.
            customerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindCustomers().Select(c => c.ContragentName).ToArray());

            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);

            sellerAgentTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
        }//SaleForm_Load


        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void customerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                customerTextBox_Leave(sender, null);
                saleDataGridView.Select(); //переводим фокус на таблицу продаж.
            }//if
        }//SellerTextBox_PreviewKeyDown

        private void customerTextBox_Leave(object sender, EventArgs e)
        {            
            if (String.IsNullOrWhiteSpace(customerTextBox.Text))
            {
                customerBackPanel.BackColor = customerStarLabel.ForeColor = Color.Red;
                customerTextBox.Clear();
                toolTip.Show("Введите имя/название клиента", this, customerBackPanel.Location, 2000);
            }//if
            else
            {
                customerStarLabel.ForeColor = Color.Black;
                customerBackPanel.BackColor = SystemColors.Control;
      
                //Если такой клиен в базе отсутствует, выводим сообщение об этом.
                string customer = customerTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == customerTextBox.Text.Trim().ToLower());
                if (customer == null)
                    toolTip.Show("Такого клиента нет в базе! Он будет добавлен.", this, customerBackPanel.Location, 2000);
                else
                    customerTextBox.Text = customer; //Выводим корректное имя контрагента. 
            }//else            
        }//customerTextBox_Leave

        private void sellerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(sellerTextBox.Text))
            {
                sellerBackPanel.BackColor = sellerStarLabel.ForeColor = Color.Red;
                sellerTextBox.Clear();
                toolTip.Show("Введите имя/название продавца", this, sellerBackPanel.Location, 2000);
            }//if
            else
            {
                sellerStarLabel.ForeColor = Color.Black;
                sellerBackPanel.BackColor = SystemColors.Control;
            }//else
        }//sellerTextBox_Leave





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы работы с таблицей.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Нумерация строк в таблице.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saleDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //Нумерация строк.
            DataGridView dgv = sender as DataGridView;

            //Если RowHeadersCell не заполнена или индекс строки изменен, присваиваем новый номер строке.
            string rowNumber = (e.RowIndex + 1).ToString();      
            object headerCellValue = dgv.Rows[e.RowIndex].HeaderCell.Value;
            if (headerCellValue == null || headerCellValue.ToString() != rowNumber)
            {
                dgv.Rows[e.RowIndex].HeaderCell.Value = rowNumber;

                //Если необходимо меняем ширину RowHeaders в зависимости от кол-ва строк в таблице.
                int defaultRowHeadersWidth = 41;
                int oneDigitWidth = 7; //Ширина одного разряда числа (определена методом тыка).
                int newRowHeadersWidth = defaultRowHeadersWidth + (oneDigitWidth * (dgv.Rows.Count.ToString().Length - 1));
                if (dgv.RowHeadersWidth != newRowHeadersWidth) //Проверка необходима, потому что изменение RowHeadersWidth приводит к инициированию события OnPaint, а сл-но к бесконечному циклу. 
                    dgv.RowHeadersWidth = newRowHeadersWidth;
            }//if
        }//saleDataGridView_RowPrePaint

        #region Методы работы с осн. таблицей.
//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void saleDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {            
            saleDataGridView[SellingPrice.Index, e.RowIndex].ReadOnly = saleDataGridView[Count.Index, e.RowIndex].ReadOnly = true;
        }//saleDataGridView_RowsAdded

        private void saleDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Находим соотв. выбранному товару данные и обновляем доп. таблицу.
            SparePart sparePart = saleDataGridView.Rows[e.RowIndex].Tag as SparePart;
            if (sparePart != null)
                FillTheExtDGV(sparePart.AvailabilityList);
            else
                extDataGridView.Rows.Clear();
        }//saleDataGridView_RowEnter

        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void saleDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _lastEditCell = saleDataGridView[e.ColumnIndex, e.RowIndex]; //запоминаем текущую ячейку как последнюю редактируемую.

            //Обрабатываем ввод в ячейку 'Название' или 'Артикул'.
            if (_lastEditCell.OwningColumn == Title || _lastEditCell.OwningColumn == Articul)
                autoCompleteListBox.Location = GetCellBelowLocation(_lastEditCell); //устанавливаем позицию вып. списка.

            //Обрабатываем ввод в ячейку 'Количествo'.
            if (_lastEditCell.OwningColumn == Count)
                SetCustomValueToCell(_lastEditCell, null); //очищаем ячейку для ввода значения пользователем.
        }//saleDataGridView_CellBeginEdit

        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в ячейку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saleDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = saleDataGridView.CurrentCell;
                       
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                //Если ячейка редактируется первый раз, подписываем её на события обработки ввода.
                if (cell.Tag == null) 
                {
                    TextBox textBoxCell = e.Control as TextBox;
                    cell.Tag = textBoxCell; //Запоминаем editing control в Tag ячейки.

                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                    textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                }//if
            }//if
        }//saleDataGridView_EditingControlShowing

        /// <summary>
        /// Метод обработки нажатия клавиш в ячейках осн. таблицы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewTextBoxCell_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Down:
                    KeyDownPress();
                    break;
                case Keys.Up:
                    KeyUpPress();
                    break;
            }//switch
        }//dataGridViewTextBoxCell_PreviewKeyDown

        private void dataGridViewTextBoxCell_TextChanged(object sender, EventArgs e)
        {
            autoCompleteListBox.Visible = false;
            autoCompleteListBox.Items.Clear();

            TextBox textBox = (TextBox)sender;
            if (!String.IsNullOrWhiteSpace(textBox.Text))
            {
                //Находим подходящий по вводу товар.                
                List<int> existingSparePartsIdsList = saleDataGridView.Rows.Cast<DataGridViewRow>().Where(r => r.Tag != null).Select(r => (r.Tag as SparePart).SparePartId).ToList(); //Id-ки уже введенного товара.
                List<SparePart>  searchSparePartsList = (_lastEditCell.OwningColumn == Title) 
                                    ? PartsDAL.SearchSparePartsAvaliablityByTitle(textBox.Text.Trim(), 10, existingSparePartsIdsList)
                                    : PartsDAL.SearchSparePartsAvaliablityByArticul(textBox.Text.Trim(), 10, existingSparePartsIdsList);

                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {                    
                    //Заполняем вып. список новыми объектами.
                    searchSparePartsList.ForEach(sp => autoCompleteListBox.Items.Add(sp));                                                                     

                    autoCompleteListBox.DisplayMember = (_lastEditCell.OwningColumn == Title) ? "Title" : "Articul";                    
                    autoCompleteListBox.Visible = true;
                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                }//if
            }//if
        }//dataGridViewTextBoxCell_TextChanged

        private void saleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isCellEditError)
            {
                DataGridViewCell cell = saleDataGridView[e.ColumnIndex, e.RowIndex];

                if (cell.OwningColumn == Title || cell.OwningColumn == Articul) //Если редактируется артикул или название товара. 
                    TitleOrArticulCellFilled(cell);
                else if (cell.OwningColumn == Count)                            //Если редактируется кол-во. 
                    CountCellFilled(cell);
                else if (cell.OwningColumn == SellingPrice)                     //Если редактируется цена продажи. 
                    SellingPriceCellFilled(cell);
            }//if
        }//saleDataGridView_CellEndEdit 

        private void saleDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //Если ошибка редактирования ячейки 'Title' или 'Articul', то возвращаем фокус обратно на ячейку (фокус теряется при выборе из вып. списка).
            if (_isCellEditError == true)
            {
                _isCellEditError = false;
                saleDataGridView.CurrentCell = _lastEditCell;

                //Включаем режим редактирования ячейки, не инициируя при этом соотв. события.
                saleDataGridView.CellBeginEdit -= saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing -= saleDataGridView_EditingControlShowing;
                saleDataGridView.BeginEdit(true);
                saleDataGridView.CellBeginEdit += saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing += saleDataGridView_EditingControlShowing;

                //ставим каретку в конец текста. 
                TextBox textBoxCell = _lastEditCell.Tag as TextBox;
                textBoxCell.SelectionStart = textBoxCell.Text.Length;
            }//if
        }//saleDataGridView_SelectionChanged

        private void saleDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                        saleDataGridView.SelectAll();
                    else
                        saleDataGridView.Rows[e.RowIndex].Selected = true;

                    //Выводим контекстное меню.
                    Point location = saleDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    saleContextMenuStrip.Show(saleDataGridView, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if 
        }//saleDataGridView_CellMouseClick     

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Выделяем строки всех выделенных ячеек.           
            saleDataGridView.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            //Удаляем все выбранные строки и соотв. им объекты.
            foreach (DataGridViewRow row in saleDataGridView.SelectedRows)
            {
                //Если строка не пустая, очищаем соотв ей список приходов.
                if (row.Tag != null)
                    _operDetList.RemoveAll(od => od.SparePart.SparePartId == (row.Tag as SparePart).SparePartId); //Очищаем список от соотв. объектов.

                //Если это не последняя строка (предназнач. для ввода нового товара в список), удаляем её.
                if (row.Index != saleDataGridView.Rows.Count-1)
                    saleDataGridView.Rows.Remove(row);   
            }//foreach

            extDataGridView.Rows.Clear(); //Очищаем доп. таблицу.
            FillTheInTotal(); //Заполняем общую сумму операции.
        }//removeToolStripMenuItem_Click
        

        #region Вспомогательные методы.
//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Артикул' и 'Название'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void TitleOrArticulCellFilled(DataGridViewCell cell)
        {
            autoCompleteListBox.Visible = false;

            if (cell.Value != null)
            {
                //Если есть такой товар в базе.
                if (autoCompleteListBox.Items.Count > 0)
                {
                    //если выбор сделан из выпадающего списка.
                    if (autoCompleteListBox.SelectedItem != null)
                    {
                        SparePart sparePart = autoCompleteListBox.SelectedItem as SparePart;
                        AutoCompleteRowInfo(cell, sparePart); //Заполняем строку данными о товаре.                        
                    }//if
                    else  //если выбор не из вып. списка.
                    {
                        toolTip.Show("Выберите товар из списка.", this, GetCellBelowLocation(cell), 1000);
                        _isCellEditError = true;
                        autoCompleteListBox.Visible = true;
                    }//else
                }//if
                else
                {
                    toolTip.Show("Нет такого товара в наличии.", this, GetCellBelowLocation(cell), 1000);
                    _isCellEditError = true;
                }//else
            }//if

            //Если нет ошибки редактирования ячейки, то отписываем editing control от событий обработки ввода.
            if (!_isCellEditError)
            {                
                TextBox textBoxCell = cell.Tag as TextBox;
                textBoxCell.TextChanged -= dataGridViewTextBoxCell_TextChanged;
                textBoxCell.PreviewKeyDown -= dataGridViewTextBoxCell_PreviewKeyDown;
                cell.Tag = null;
            }//if
        }//TitleOrArticulCellFilled

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Количество'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void CountCellFilled(DataGridViewCell cell)
        {
            //Проверяем корректность ввода.
            string measureUnit = cell.OwningRow.Cells[Unit.Index].Value.ToString();
            if (IsCountCellValueCorrect(cell, measureUnit))
            {                
                AutoChoisePurchases(cell);         //Автовыбор приходов с которых осущ. продажа.
            }//if            
            else
            {
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000); //выводим всплывающее окно с сообщением об ошибке.
                SetDefaultValueToCell(cell); //Возвращаем серый цвет и дефолтное значение данной ячейке.

                //Возвращаем дефолтные значения во всех строках доп. таблицы.
                SetDefaultValuesToExtDataGridView((cell.OwningRow.Tag as SparePart).SparePartId);
            }//else
            FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
        }//CountCellFilled

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Цена продажи'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void SellingPriceCellFilled(DataGridViewCell cell)
        {
            if (cell.Value != null) //Если строка не пустая, проверить корректность ввода.
            {
                try
                {
                    float sellPrice = Convert.ToSingle(cell.Value);
                    if (sellPrice == 0) 
                        throw new Exception();  //ввод нуля также является ошибкой.

                    int sparePartId = (cell.OwningRow.Tag as SparePart).SparePartId;
                    SparePart sparePart = saleDataGridView.Rows.Cast<DataGridViewRow>().First(r => r.Tag != null && (r.Tag as SparePart).SparePartId == sparePartId).Tag as SparePart;
                    //Если цена продажи хотя бы где-то ниже закупочной требуем подтверждения действий.                         
                    if (sparePart.AvailabilityList.Any(av => av.OperationDetails.Price >= sellPrice))
                        if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            throw new Exception();

                    cell.Value = sellPrice; //Перезаписываем установленную цену, для её форматированного вывода в ячейке.
                }//try
                catch
                {
                    //выводим всплывающее окно с сообщением об ошибке и очищаем ввод.
                    toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                    cell.Value = null;
                }//catch

                FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
            }//if     
        }//SellingPriceCellFilled

        /// <summary>
        /// Автозаполнение строки соотв. инф-цией.
        /// </summary>
        /// <param name="countCell">Заполняемая ячейка.</param>
        /// <param name="titleAndArticul">Массив строк с артикулом и названием.</param>
        private void AutoCompleteRowInfo(DataGridViewCell cell, SparePart sparePart)
        {
            //Если такой товар найден в вып. списке.
            if (sparePart != null)
            {
                FillTheBothDGV(cell.OwningRow, sparePart);

                cell.OwningRow.Cells[SellingPrice.Index].ReadOnly = cell.OwningRow.Cells[Count.Index].ReadOnly = false;
                cell.OwningRow.Cells[Title.Index].ReadOnly = cell.OwningRow.Cells[Articul.Index].ReadOnly = true;

                #region Увеличение saleGroupBox.
                //if (saleDataGridView.PreferredSize.Height > saleDataGridView.Size.Height)
                //{
                //    MessageBox.Show("bigger");
                //    int height = saleDataGridView.Rows[0].Cells["Title"].Size.Height;
                //    saleGroupBox.Size = new Size(saleGroupBox.Width, saleGroupBox.Height + height);
                //}
                #endregion
            }//if
        }//AutoCompleteRowInfo

        /// <summary>
        /// Возвращает число или генерирует исключение если введенное значение в ячейку 'Кол-во' некорректно.
        /// </summary>
        /// <param name="countCell">Ячейка столбца 'Кол-во'.</param>
        /// <returns></returns>
        private bool IsCountCellValueCorrect(DataGridViewCell countCell, string measureUnit)
        {
            float count;
            //Если введено не числовое значение, это ошибка.
            if (countCell.Value == null || (Single.TryParse(countCell.Value.ToString(), out count) == false))
                return false;

            //Ввод значения не более 0, или больше чем есть в наличии является ошибкой. 
            float totalCount = Convert.ToSingle(countCell.Tag);
            if (count <= 0 || count > totalCount)
                return false;

            //Проверяем является ли введенное число корректным для продажи, т.е. кратно ли оно минимальной единице продажи.     
            if (count % Models.MeasureUnit.GetMinUnitSale(measureUnit) != 0)
                return false;

            return true;
        }//IsCountCellValueCorrect

        /// <summary>
        /// Заполняет обе таблицы необх. данными
        /// </summary>
        /// <param name="row">Заполняемая строка в осн. таблице.</param>
        /// <param name="sparePart">Данные для заполнения.</param>
        private void FillTheBothDGV(DataGridViewRow row, SparePart sparePart)
        {
            //Заполняем осн. таблицу.
            FillTheSaleDGV(row, sparePart);
            //Очищаем доп. таблицу и заполняем её новой инф-цией.
            extDataGridView.Rows.Clear();
            FillTheExtDGV(sparePart.AvailabilityList);
        }//FillTheBothDGV

        /// <summary>
        /// Заполняет осн. таблицу данными.
        /// </summary>
        /// <param name="row">Заполняемая строка.</param>
        /// <param name="sparePart">Данные для заполнения строки.</param>
        private void FillTheSaleDGV(DataGridViewRow row, SparePart sparePart)
        {
            row.Tag = sparePart;

            row.Cells[Title.Index].Value    = sparePart.Title;
            row.Cells[Articul.Index].Value  = sparePart.Articul;
            row.Cells[Unit.Index].Value     = sparePart.MeasureUnit;

            row.Cells[Count.Index].Tag = Availability.GetTotalCount(sparePart.AvailabilityList); //Заполняем кол-во и запоминаем в Tag.
            SetDefaultValueToCell(row.Cells[Count.Index]); //Задаем серый цвет и дефолтное значение данной ячейке.

            //Если отпускная цена не указана поль-лем и если у всех приходов она одинаковая, выводим её в saleDGV.
            if (row.Cells[SellingPrice.Name].Value == null)
                if (!sparePart.AvailabilityList.Any(av => av.SellingPrice != sparePart.AvailabilityList[0].SellingPrice))
                    row.Cells[SellingPrice.Name].Value = sparePart.AvailabilityList[0].SellingPrice;
        }//FillTheSaleDGV


        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из saleDataGridView. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = saleDataGridView.Location;
            Point gbLoc = saleGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation

        /// <summary>
        /// Метод автовыбора прихода с которого осуществляется продажа (Всегда самые старые приходы).
        /// </summary>
        /// <param name="extCountCell">Ячейка столбца 'Кол-во'.</param>
        private void AutoChoisePurchases(DataGridViewCell cell)
        {
            //Узнаем введенное кол-во в saleDGV.
            float sellCount = Convert.ToSingle(cell.Value);
            int sparePartId = (cell.OwningRow.Tag as SparePart).SparePartId;
            //Очищаем все записи с соотв. SparePartId из списка приходов.
            _operDetList.RemoveAll(od => od.SparePart.SparePartId == sparePartId);
            

            //Перебираем по строкам из extDGV.
            foreach (DataGridViewRow row in extDataGridView.Rows)
            {
                float extAvailCount = Convert.ToSingle(row.Cells[extCount.Index].Tag); //количество в наличии в данном приходе.                
                
                if (sellCount > 0)
                {
                    int purchaseId = Convert.ToInt32(row.Cells[extPurchaseId.Index].Value);
                    float curSellValue = (sellCount > extAvailCount) ? extAvailCount  : sellCount;

                    DataGridViewCell extCountCell = row.Cells[extCount.Index];
                    SetCustomValueToCell(extCountCell, curSellValue); //задаём значение для ячейки.
                    FillTheOperDetList(sparePartId, extCountCell);     
                    sellCount -= extAvailCount;                   
                }//if
                else
                {
                    SetDefaultValueToCell(row.Cells[extCount.Index]); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                }//else
            }//foreach
        }//AutoChoisePurchases

        /// <summary>
        /// Меняет количество или добавляет новое значение в список деталей операции. 
        /// </summary>
        /// <param name="sparePartId">Id товара.</param>
        /// <param name="extCountCell">Ячейка стобца 'Кол-во' доп. таблицы.</param>
        private void FillTheOperDetList(int sparePartId, DataGridViewCell extCountCell)
        {
            int purchaseId = Convert.ToInt32(extCountCell.OwningRow.Cells[extPurchaseId.Index].Value);
            float sellCount = (extCountCell.Style.ForeColor == Color.Black) ? Convert.ToSingle(extCountCell.Value) : 0; 
            //Находим, если есть соотв. объект в списке.
            OperationDetails operDet = _operDetList.FirstOrDefault(od => od.SparePart.SparePartId == sparePartId
                                                                      && od.Operation.OperationId == purchaseId);

            //Если объект есть, меняем у него св-во Count, иначе создаём новый объект.
            if (operDet == null)
            {
                if (sellCount > 0)
                {
                    SparePart sparePart = saleDataGridView.Rows.Cast<DataGridViewRow>().First(r => r.Tag != null && (r.Tag as SparePart).SparePartId == sparePartId).Tag as SparePart;                   
                    IOperation purch = sparePart.AvailabilityList.First(av => av.OperationDetails.Operation.OperationId == purchaseId).OperationDetails.Operation;
                    
                    _operDetList.Add(new OperationDetails(sparePart, purch, sellCount, 0));
                }//if
            }//if
            else
            {
                //Если такой объект есть в списке, и прод. кол-во > 0, то обновляем кол-во, иначе удаляем из списка.
                if (sellCount > 0)
                    operDet.Count = sellCount;
                else
                    _operDetList.Remove(operDet);
            }//else
        }//FillTheOperDetList

        /// <summary>
        /// Заполняет ячейку 'Сумма' заданной строки и общую сумму.
        /// </summary>
        /// <param name="row">Строка дял которой производятся вычисления и заполнение.</param>
        private void FillTheSumCell(DataGridViewRow row)
        {
            if (row.Cells[Count.Index].Style.ForeColor == Color.Black && row.Cells[SellingPrice.Index].Value != null)
            {
                float sellPrice = Convert.ToSingle(row.Cells[SellingPrice.Index].Value);
                float sellCount = Convert.ToSingle(row.Cells[Count.Index].Value);

                row.Cells[Sum.Index].Value = sellPrice * sellCount;                
            }//if
            else
            {
                row.Cells[Sum.Index].Value = null;//очищаем ячейку. 
            }//else

            FillTheInTotal(); //Заполняем общую сумму операции.
        }//FillTheSumCell

        /// <summary>
        /// Заполняет InTotalLabel корретным значением.
        /// </summary>
        private void FillTheInTotal()
        {
            float inTotal = 0;
            foreach (DataGridViewRow row in saleDataGridView.Rows)
            {
                //Если в строке указана и цена и количестов.
                if (row.Cells[Sum.Index].Value != null)
                {
                    float sellPrice = Convert.ToSingle(row.Cells[SellingPrice.Index].Value);
                    float sellCount = Convert.ToSingle(row.Cells[Count.Index].Value);
                    inTotal += sellPrice * sellCount;
                }//if
            }//foreach

            //Заполняем InTotalLabel расчитанным значением.
            inTotalNumberLabel.Text = String.Format("{0}(руб)", Math.Round(inTotal, 2, MidpointRounding.AwayFromZero));
        }//FillTheInTotal

        /// <summary>
        /// Возвращает дефолтные значения во все ячейки столбца 'Кол-во' доп. таблицы.
        /// </summary>
        /// <param name="sparePartId">Ид товара.</param>
        private void SetDefaultValuesToExtDataGridView(int sparePartId)
        {
            foreach (DataGridViewRow extRow in extDataGridView.Rows)
            {
                SetDefaultValueToCell(extRow.Cells[extCount.Index]);           //Записываем дефолтное значение в ячейку.
                FillTheOperDetList(sparePartId, extRow.Cells[extCount.Index]); //Запоминаем изменение в список.    
            }//foreach
        }//SetDefaultValuesToExtDataGridView

        /// <summary>
        /// Записывает дефолтное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetDefaultValueToCell(DataGridViewCell cell)
        {
            cell.Style.ForeColor = Color.Gray;
            cell.Value           = cell.Tag;
        }//SetDefaultValueToCell


        /// <summary>
        /// Записывает кастомное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetCustomValueToCell(DataGridViewCell cell, object value)
        {
            cell.Style.ForeColor = Color.Black;
            cell.Value = value;
        }//SetCustomValueToCell

        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Down.
        /// </summary>
        private void KeyDownPress()
        {
            _isCellEditError = true;
            //Если выбран последний эл-нт списка, вернуть начальное значение и убрать выделение в listBox-е. 
            if (autoCompleteListBox.SelectedIndex == autoCompleteListBox.Items.Count - 1)
                autoCompleteListBox.ClearSelected();
            else
                autoCompleteListBox.SelectedIndex += 1;
        }//KeyDownPress

        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Up.
        /// </summary>
        private void KeyUpPress()
        {
            _isCellEditError = true;
            //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
            if (autoCompleteListBox.SelectedIndex == -1)
            {
                autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1;
            }//if
            else
            {
                if (autoCompleteListBox.SelectedIndex == 0)
                    autoCompleteListBox.ClearSelected();
                else
                    autoCompleteListBox.SelectedIndex -= 1;
            }//else

            //Если это нулевая строка, то при нажатии Up не происходит событие SelectionChanged, и при выборе из вып. списка каретка ставится в начало строки, что затрудняет дальнейший ввод поль-лю. Мы вызываем событие искусственно и ставим каретку в конец строки.                               
            if (_lastEditCell.OwningRow.Index == 0)
                saleDataGridView_SelectionChanged(null, null);
        }//KeyUpPress


//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Методы работы с выпадающим списком.

        /// <summary>
        /// Обработчик для того, чтобы не срабатывало событие CellEndEdit при клике мышкой по вып. спику.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
        {
            _isCellEditError = true;
        }//autoCompleteListBox_MouseHover

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                //Возвращаем фокус на ячейку для кот. выводится вып. список.                
                saleDataGridView_SelectionChanged(null, null);
                _isCellEditError = true;
            }//if
            else
            {
                //Делаем автозаполнение строки, выбранным объектом.   
                _isCellEditError = false;
                saleDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(_lastEditCell.ColumnIndex, _lastEditCell.RowIndex));                
            }//else
        }//autoCompleteListBox_MouseDown


        #endregion

        












//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Методы работы с доп. таблицей.
//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void extDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == extCount.Index)
            {
                DataGridViewCell cell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                SetCustomValueToCell(cell, null); //Очищаем ячейку для ввода знвчения поль-лем.
            }//if
        }//extDataGridView_CellBeginEdit

        private void extDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Если редактировалась колонка "Кол-во". хотя по идее все остальные readOnly.
            if (extDataGridView.Columns[e.ColumnIndex] == extCount)
            {
                DataGridViewRow row = saleDataGridView.CurrentRow;
                DataGridViewCell extCountCell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewCell countCell = row.Cells[Count.Index];
                //Проверяем корректность ввода.
                int sparePartId = (row.Tag as SparePart).SparePartId;
                string measureUnit = extCountCell.OwningRow.Cells[extUnit.Index].Value.ToString();                
                if (IsCountCellValueCorrect(extCountCell, measureUnit))
                {
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.                                      
                }//if
                else
                {
                    toolTip.Show("Введены некорректные данные", this, GetExtCellBelowLocation(extCountCell), 1000);  //выводим всплывающее окно с сообщением об ошибке.
                    SetDefaultValueToCell(extCountCell); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.                    
                }//else       

                FillTheSumCell(row);                           //Заполняем столбец 'Сумма'.
                FillTheOperDetList(sparePartId, extCountCell); //Запоминаем изменение в список.    
            }//if       
        }//extDataGridView_CellEndEdit  

        private void extDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            markupComboBox.Enabled = (extDataGridView.SelectedCells.Count > 0); //Если есть выделенные клетки делаем доступной изменение наценки.
        }//extDataGridView_SelectionChanged

        private void extGroupBox_Click(object sender, EventArgs e)
        {
            extDataGridView.ClearSelection();
        }//extGroupBox_Click

        /// <summary>
        /// Заполняет данными таблицу доп. инф-ции.
        /// </summary>
        /// <param name="availList">Список приходов данного товара в наличии.</param>
        private void FillTheExtDGV(List<Availability> availList)
        {
            //Очищаем предварительно таблицу.
            extDataGridView.Rows.Clear();
            extStorageAdress.Visible = NoteExtCol.Visible = false;
            //Заполняем таблицу новыми данными.
            foreach (Availability avail in availList)
            {
                int rowIndx = extDataGridView.Rows.Add();
                DataGridViewRow row = extDataGridView.Rows[rowIndx];

                row.Cells[extSupplier.Index].Value      = avail.OperationDetails.Operation.Contragent.ContragentName;
                row.Cells[extUnit.Index].Value          = avail.OperationDetails.SparePart.MeasureUnit;
                row.Cells[extStorageAdress.Index].Value = avail.StorageAddress;
                row.Cells[extPrice.Index].Value         = avail.OperationDetails.Price;
                row.Cells[extMarkup.Index].Value        = Models.Markup.GetDescription(avail.Markup);
                row.Cells[extSellingPrice.Index].Value  = avail.SellingPrice;
                row.Cells[extPurchaseId.Index].Value    = avail.OperationDetails.Operation.OperationId;
                row.Cells[extPurchaseDate.Index].Value  = avail.OperationDetails.Operation.OperationDate;
                row.Cells[NoteExtCol.Index].Value       = avail.OperationDetails.Operation.Description;

                //Делаем видимыми соотв. столбцы если в св-вах 'Адрес хранилища' и 'Примечание по поставке' есть данные.                
                if (avail.StorageAddress != null)
                    extStorageAdress.Visible = true;

                if (avail.OperationDetails.Operation.Description != null)
                    NoteExtCol.Visible = true;

                //Заполняем ячейку 'Кол-во' либо ранее установленным значением, иначе общим кол-вом по данному приходу в наличии. 
                OperationDetails operDet = _operDetList.FirstOrDefault(od => od.SparePart.SparePartId == avail.OperationDetails.SparePart.SparePartId
                                                                    && od.Operation.OperationId == avail.OperationDetails.Operation.OperationId);

                DataGridViewCell extCountCell = row.Cells[extCount.Index];
                extCountCell.Tag = avail.OperationDetails.Count; //заполняем ячейку значением и запоминаем это дефолтное значение в Tag.
                if (operDet == null)
                {
                    SetDefaultValueToCell(extCountCell); //Задаем серый цвет и дефолтное значение данной ячейке.
                }//if
                else
                {
                    SetCustomValueToCell(extCountCell, operDet.Count); //Задаем значение ячейки.
                }//else
            }//foreach            

            //Сортируем таблицу по дате прихода.
            extDataGridView.Sort(extPurchaseDate, ListSortDirection.Ascending);
            extDataGridView.ClearSelection();
        }//FillTheExtDGV

        /// <summary>
        /// Обновляет значение ячейки 'Кол-во' в таблице продаж, после изменений в доп. таблице.
        /// </summary>
        /// <param name="countCell">Соотв. ячейка 'Кол-во' в осн. таблице.</param>
        private void SaleDGVCountColumnUpdate(DataGridViewCell countCell)
        {
            //Находим общее кол-во данного продаваемого товара.
            float extSellCount = 0;
            foreach (DataGridViewRow extRow in extDataGridView.Rows)
            {
                if (extRow.Cells[extCount.Index].Style.ForeColor == Color.Black)
                    extSellCount += Convert.ToSingle(extRow.Cells[extCount.Index].Value);
            }//foreach
                            
            //Если есть кастомный ввод.
            if (extSellCount > 0)
                SetCustomValueToCell(countCell, extSellCount); //Обновляем "кол-во" в таблице продаж.
            else 
                SetDefaultValueToCell(countCell); //Задаём дефолтное значения для ячейки.   
        }//SaleDGVCountColumnUpdate

        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из extSaleDataGridView. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetExtCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = extDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc  = extDataGridView.Location;
            Point gbLoc   = extGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation



        #region Методы связанные с изменением наценки.
//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                markupComboBox_SelectedIndexChanged(sender, e);
        }//markupComboBox_PreviewKeyDown


        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (extDataGridView.SelectedCells.Count == 0) 
                return;

            //выделяем строки всех выделенных клеток.
            extDataGridView.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            
            try
            {
                //узнаем процент заданной наценки.
                float markupValue = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                string markupType = Models.Markup.GetDescription(markupValue);

                //Обновляем таблицу.
                foreach (DataGridViewRow row in extDataGridView.SelectedRows)
                {
                    row.Cells[extMarkup.Index].Value = markupType;

                    float price     = (float)row.Cells[extPrice.Index].Value;
                    float sellPrice = (float)Math.Round(price + (price * markupValue / 100), 2, MidpointRounding.AwayFromZero);
                    row.Cells[extSellingPrice.Index].Value = sellPrice;
                }//foreach
            }//try
            catch
            {
                toolTip.Show("Введено некорректное значение.", this, markupComboBox.Location, 2000);
            }//catch
        }//markupComboBox_SelectedIndexChanged






//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion



//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion











//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы вывода инф-ции в Excel.

        private void BeginLoadSaleToExcelFile(object sale)
        {
            if (sale is Sale)
                LoadSaleToExcelFile(sale as Sale);
        }//BeginLoadsaleToExcelFile

        /// <summary>
        /// Метод вывода расходной информации в Excel-файл.
        /// </summary>
        /// <param name="sale">Информация о расходе.</param>
        /// <param name="availabilityList">Список проданного товара.</param>
        private void LoadSaleToExcelFile(Sale sale)
        {
            IList<OperationDetails> operDetList = sale.OperationDetailsList;

            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 10;

            int row = 1, column = 1;
            //Выводим Id и Дату. 
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Расходная накладная №{0} от {1}г.", sale.OperationId, sale.OperationDate.ToString("dd/MM/yyyy"));

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         sellerLabel.Text + " " + sellerTextBox.Text,
                                                         customerLabel.Text + " " + customerTextBox.Text);

            #region Вывод таблицы товаров.

            row += 2;
            //Выводим заголовок.
            ExcelApp.Cells[row, column] = "Произв.";
            ExcelApp.Cells[row, column + 1] = "Артикул";
            ExcelApp.Cells[row, column + 2] = "Название";
            ExcelApp.Cells[row, column + 3] = "Ед. изм.";
            ExcelApp.Cells[row, column + 4] = "Кол-во";
            ExcelApp.Cells[row, column + 5] = "Цена";
            ExcelApp.Cells[row, column + 6] = "Сумма";

            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Font.Bold = true;
            excelCells.Font.Size = 12;
            //Уменьшаем ширину колонки "Ед. изм."
            (ExcelApp.Cells[row, column + 3] as Excel.Range).Cells.VerticalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            (ExcelApp.Cells[row, column + 3] as Excel.Range).Columns.ColumnWidth = 5;
            //Обводим заголовки таблицы рамкой. 
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
            //Устанавливаем стиль и толщину линии
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            //int manufColWidth = 15, minManufColWidth = 8; //  15 -- Взято методом тыка.

            SetColumnsWidth(operDetList, (ExcelApp.Cells[row, column + 2] as Excel.Range), (ExcelApp.Cells[row, column + 1] as Excel.Range), (ExcelApp.Cells[row, column] as Excel.Range));

            //Выводим список товаров.
            foreach (OperationDetails operDet in operDetList)
            {
                ++row;
                ExcelApp.Cells[row, column + 2] = operDet.SparePart.Title;
                ExcelApp.Cells[row, column + 1] = operDet.SparePart.Articul;
                //Выравнивание диапазона строк.
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
                if (operDet.SparePart.Articul.Length > articulColWidth || operDet.SparePart.Title.Length > titleColWidth)
                {
                    ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
                    //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
                    if (operDet.SparePart.Articul.Length > articulColWidth && operDet.SparePart.Title.Length <= titleColWidth)
                        (ExcelApp.Cells[row, column + 2] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    if (operDet.SparePart.Articul.Length <= articulColWidth && operDet.SparePart.Title.Length > titleColWidth)
                        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }//if

                ExcelApp.Cells[row, column] = operDet.SparePart.Manufacturer;

                ExcelApp.Cells[row, column + 3] = operDet.SparePart.MeasureUnit;
                float count = operDet.Count;
                float price = operDet.Price;
                ExcelApp.Cells[row, column + 4] = count;
                ExcelApp.Cells[row, column + 5] = price;
                ExcelApp.Cells[row, column + 6] = price * count;
            }//foreach

            //Обводим талицу рамкой. 
            excelCells = ExcelWorkSheet.get_Range("A" + (row - operDetList.Count + 1).ToString(), "G" + row.ToString());
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            //Выводим "Итого".
            ++row;
            //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            int indent = 0; //отступ
            if (inTotalNumberLabel.Text.Length <= 9)
                indent = 1;

            ExcelApp.Cells[row, column + 4 + indent] = inTotalLabel.Text;
            ExcelApp.Cells[row, column + 5 + indent] = inTotalNumberLabel.Text;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Underline = true;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Size = (ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Size = 12;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Bold = (ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Bold = true;

            #endregion

            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         sellerAgentLabel.Text + " " + sellerAgentTextBox.Text,
                                                         customerAgentLabel.Text + " " + customerAgentTextBox.Text);

            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;
            //ширина подобрана методом тыка.
            ExcelApp.Cells[row, column].Value = "                                                                                                                                                                                                                                    ";//longEmptyString.ToString();
            (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
            //Выводим заметку
            row++;
            // объединим область ячеек  строки "вместе"
            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.WrapText = true;
            excelCells.Value = sale.Description;//descriptionRichTextBox.Text;
            AutoFitMergedCellRowHeight((ExcelApp.Cells[row, column] as Excel.Range));

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;

            this.Close();
        }//LoadSaleToExcelFile  

        private void AutoFitMergedCellRowHeight(Excel.Range rng)
        {
            double mergedCellRgWidth = 0;
            double rngWidth, possNewRowHeight;

            if (rng.MergeCells)
            {
                // здесь использована самописная функция перевода стиля R1C1 в A1                
                if (xlRCtoA1(rng.Row, rng.Column) == xlRCtoA1(rng.Range["A1"].Row, rng.Range["A1"].Column))
                {
                    rng = rng.MergeArea;
                    if (rng.Rows.Count == 1 && rng.WrapText == true)
                    {
                        (rng.Parent as Excel._Worksheet).Application.ScreenUpdating = false;
                        rngWidth = rng.Cells.Item[1, 1].ColumnWidth;
                        mergedCellRgWidth = GetRangeWidth(rng);
                        rng.MergeCells = false;
                        rng.Cells.Item[1, 1].ColumnWidth = mergedCellRgWidth;
                        rng.EntireRow.AutoFit();
                        possNewRowHeight = rng.RowHeight;
                        rng.Cells.Item[1, 1].ColumnWidth = rngWidth;
                        rng.MergeCells = true;
                        rng.RowHeight = possNewRowHeight;
                        (rng.Parent as Excel._Worksheet).Application.ScreenUpdating = true;
                    }//if
                }//if                
            }//if
        }//AutoFitMergedCellRowHeight

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleCol">Столбец "Название".</param>
        /// <param name="articulCol">Столбец "Артикул".</param>
        /// <param name="manufCol">Столбец "Производитель".</param>
        private void SetColumnsWidth(IList<OperationDetails> operDetList, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = operDetList.Select(od => od.SparePart.Manufacturer).Where(man => man != null);
            if (sparePartsManufacturers.Count() > 0)
                maxManufLenght = sparePartsManufacturers.Max(man => man.Length);

            if (maxManufLenght < manufColWidth)
            {
                int different = manufColWidth - maxManufLenght; //разница между дефолтной шириной столбца и фактической.
                titleColWidth += (manufColWidth - different < minManufColWidth) ? minManufColWidth : different;
                manufColWidth = (manufColWidth - different < minManufColWidth) ? minManufColWidth : manufColWidth - different;
            }//if

            manufCol.Columns.ColumnWidth = manufColWidth;
            articulCol.Columns.ColumnWidth = articulColWidth;
            titleCol.Columns.ColumnWidth = titleColWidth;
        }//SetColumnsWidth

        /// <summary>
        /// Возвращает ширину заданной области.
        /// </summary>
        /// <param name="rng">Область ширина которой считается.</param>
        /// <returns></returns>
        private double GetRangeWidth(Excel.Range rng)
        {
            double rngWidth = 0;
            for (int i = 1; i <= rng.Columns.Count; ++i)
            {
                rngWidth += rng.Cells.Item[1, i].ColumnWidth;
            }//for
            return rngWidth;
        }//GetRangeWidth

        private string xlRCtoA1(int ARow, int ACol, bool RowAbsolute = false, bool ColAbsolute = false)
        {
            int A1 = 'A' - 1;  // номер "A" минус 1 (65 - 1 = 64)
            int AZ = 'Z' - A1; // кол-во букв в англ. алфавите (90 - 64 = 26)

            int t, m;
            string S;

            t = ACol / AZ; // целая часть
            m = (ACol % AZ); // остаток?
            if (m == 0)
                t--;
            if (t > 0)
                S = Convert.ToString((char)(A1 + t));
            else S = String.Empty;

            if (m == 0)
                t = AZ;
            else t = m;

            S = S + (char)(A1 + t);

            //весь адрес.
            if (ColAbsolute) S = '$' + S;
            if (RowAbsolute) S = S + '$';

            S = S + ARow.ToString();
            return S;
        }//xlRCtoA1








        #endregion


        /// <summary>
        /// Возвращает объект типа Sale, созданный из данных формы.
        /// </summary>
        /// <returns></returns>
        private Sale CreateSaleFromForm()
        {
            //Находим контрагента. Если такого ещё нет в базе, то создаем новый объект.
            IContragent customer = PartsDAL.FindCustomers(customerTextBox.Text.Trim());
            customer = (customer == null) ? new Supplier(0, customerTextBox.Text.Trim(), null, null, null, null) : customer;

             Sale sale = new Sale
            (
                employee            : Form1.CurEmployee,
                contragent          : customer,
                contragentEmployee  : (!String.IsNullOrWhiteSpace(customerAgentTextBox.Text)) ? customerAgentTextBox.Text.Trim() : null,
                operationDate       : saleDateTimePicker.Value,
                description         : (!String.IsNullOrWhiteSpace(descriptionRichTextBox.Text)) ? descriptionRichTextBox.Text.Trim() : null,
                operDetList         : CreateOperationDetailsListFromForm()
            );
            //Присваиваем 'Операцию' для каждого OperationDetails.
            sale.OperationDetailsList.ToList().ForEach(od => od.Operation = sale); 
            
            return sale;
        }//CreateSaleFromForm

        /// <summary>
        /// Возвращает список объектов типа OperationDetails, созданный из данных таблицы продаж.
        /// </summary>
        /// <returns></returns>
        private List<OperationDetails> CreateOperationDetailsListFromForm()
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();
            foreach (DataGridViewRow row in saleDataGridView.Rows)
            {
                //Если строка не пустая.
                if (row.Tag != null)
                {
                    float count = Convert.ToSingle(row.Cells[Count.Index].Value);
                    float sellPrice = Convert.ToSingle(row.Cells[SellingPrice.Index].Value);

                    SparePart sparePart = row.Tag as SparePart;
                    operDetList.Add(new OperationDetails(sparePart, null, count, sellPrice));
                }//if
            }//foreach

            return operDetList;
        }//CreateOperationDetailsListFromForm

        /// <summary>
        /// Возвращает true если все обязательные поля корректно заполнены, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool IsRequiredFieldsValid()
        {
            //Находим все BackPanel-контролы на форме. 
            List<Control> curAccBackControls = this.GetAllControls(typeof(Panel), "BackPanel");

            customerTextBox_Leave(null, null);
            sellerTextBox_Leave(null, null);

            //Если хоть один контрол не прошел валидацию, возв-ем false.
            if (curAccBackControls.Any(backPanel => backPanel.BackColor == Color.Red))
                return false;

            //Если таблица не заполнена или не везде указана цена или кол-во.
            if (_operDetList.Count == 0 || saleDataGridView.Rows.Cast<DataGridViewRow>().Any(r => r.Tag != null && (r.Cells[SellingPrice.Index].Value == null || r.Cells[Count.Index].Style.ForeColor == Color.Gray)))
            {
                toolTip.Show("Таблица не заполнена или не везде указана цена или количество товара", this, okButton.Location, 3000);
                return false;
            }//if

            return true;            
        }//IsRequiredAddingAreaFieldsValid




        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.Close();
            }//if
        }//cancelButton_MouseClick
        
        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Если всё заполненно корректно.
                if (IsRequiredFieldsValid())
                {
                    Sale sale = CreateSaleFromForm();

                    try
                    {
                        sale.OperationId = PartsDAL.AddSale(sale, _operDetList);
                    }//try
                    catch (Exception)
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        return;
                    }//catch 

                    //LoadsaleToExcelFile(sale, availabilityList);
                    /*!!!*/
                    new System.Threading.Thread(BeginLoadSaleToExcelFile).Start(sale); //Сделать по нормальному вызов с потоком.

                    this.Visible = false;
                    //this.Close();
                }//if
            }//if
        }//

        
    }//Form2

}//namespace
