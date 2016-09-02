using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using PartsApp.Models;
using PartsApp.SupportClasses;

namespace PartsApp
{
    /*Задания*/
    //Передавать inTotal в метод распечатки в Excel.

    public partial class PurchaseForm : Form
    {
        bool _isCellEditError = false;
        DataGridViewCell _lastEditCell;



        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            storageComboBox.SelectedIndex  = 0;
            currencyComboBox.SelectedIndex = 0;

            //Заполняем список автоподстановки для ввода контрагента.
            supplierTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindSuppliers().Select(c => c.ContragentName).ToArray());

            //Устанавливаем параметры дат, для DateTimePicker.            
            purchaseDateTimePicker.MaxDate = purchaseDateTimePicker.Value = DateTime.Now;
            
            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);
            
            buyerAgentTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
        }//PurchaseForm_Load

        private void storageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (storageComboBox.SelectedIndex != 0)
                storageAdressStarLabel.Visible = storageAdressLabel.Visible = storageAdressBackPanel.Visible = true;
            else
            {
                storageAdressStarLabel.Visible = storageAdressLabel.Visible = storageAdressBackPanel.Visible = false;
                storageAdressBackPanel.BackColor = SystemColors.Control;
                storageAdressTextBox.Clear();
            }//else
        }//storageComboBox_SelectedIndexChanged

        private void storageAdressTextBox_Leave(object sender, EventArgs e)
        {
            if (storageAdressTextBox.Visible)
            {
                if (String.IsNullOrWhiteSpace(storageAdressTextBox.Text))
                    storageAdressBackPanel.BackColor = Color.Red;
                else 
                    storageAdressBackPanel.BackColor = SystemColors.Control;
            }//if
        }//storageAdressTextBox_Leave

        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void supplierTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                supplierTextBox_Leave(sender, null);
                purchaseDataGridView.Select(); //переводим фокус на таблицу приходов.
            }//if
        }//supplierTextBox_PreviewKeyDown

        private void supplierTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(supplierTextBox.Text))
            {
                supplierBackPanel.BackColor = supplierStarLabel.ForeColor = Color.Red;
                supplierTextBox.Clear();
                toolTip.Show("Введите имя/название поставщика", this, supplierBackPanel.Location, 2000);
            }//if
            else
            {
                supplierStarLabel.ForeColor = Color.Black;
                supplierBackPanel.BackColor = SystemColors.Control;

                //Если такой контрагент в базе отсутствует, выводим сообщение об этом.
                string supplier = supplierTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == supplierTextBox.Text.Trim().ToLower());
                if (supplier == null)
                    toolTip.Show("Такого клиента нет в базе! Он будет добавлен.", this, supplierBackPanel.Location, 2000);
                else
                    supplierTextBox.Text = supplier; //Выводим корректное имя контрагента.
            }//else  
        }//supplierTextBox_Leave

        private void buyerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(buyerTextBox.Text))
            {
                buyerBackPanel.BackColor = buyerStarLabel.ForeColor = Color.Red;
                buyerTextBox.Clear();
                toolTip.Show("Введите имя/название покупателя", this, buyerBackPanel.Location, 2000);
            }//if
            else
            {
                buyerStarLabel.ForeColor = Color.Black;
                buyerBackPanel.BackColor = SystemColors.Control;
            }//else
        }//buyerTextBox_Leave





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /*Нумерация строк purchaseDataGridView*/
        private void purchaseDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
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
        }//purchaseDataGridView_RowPrePaint

        #region Методы работы с таблицей.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*События идут в порядке их возможного вызова.*/

        private void purchaseDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //Делаем ReadOnly ячейки 'Цена', 'Кол-во' и 'Цена Продажи'.
            dgv[SellingPriceCol.Index, e.RowIndex].ReadOnly = dgv[CountCol.Index, e.RowIndex].ReadOnly = dgv[PriceCol.Index, e.RowIndex].ReadOnly = true;
        }//purchaseDataGridView_RowsAdded

        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void purchaseDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _lastEditCell = purchaseDataGridView[e.ColumnIndex, e.RowIndex]; //запоминаем текущую ячейку как последнюю редактируемую.

            //Обрабатываем ввод в ячейку 'Название' или 'Артикул'.
            if (_lastEditCell.OwningColumn == TitleCol || _lastEditCell.OwningColumn == ArticulCol)
                autoCompleteListBox.Location = GetCellBelowLocation(_lastEditCell); //устанавливаем позицию вып. списка.
        }//purchaseDataGridView_CellBeginEdit

        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в ячейку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void purchaseDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {            
            DataGridViewCell cell = purchaseDataGridView.CurrentCell;

            if (cell.OwningColumn == TitleCol || cell.OwningColumn == ArticulCol)
            {
                //Если ячейка редактируется первый раз, подписываем её на события обработки ввода.
                if (cell.Tag == null)
                {
                    TextBox textBoxCell = e.Control as TextBox;
                    cell.Tag = textBoxCell; //Запоминаем editing control в Tag ячейки.

                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                    textBoxCell.TextChanged    += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                }//if
            }//if
        }//purchaseDataGridView_EditingControlShowing

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
            autoCompleteListBox.DataSource = null;

            TextBox textBox = (TextBox)sender;
            if (String.IsNullOrEmpty(textBox.Text) == false)
            {
                //Находим подходящий по вводу товар.
                List<int> existingSparePartsIdsList = purchaseDataGridView.Rows.Cast<DataGridViewRow>().Where(r => r.Tag != null).Select(r => (r.Tag as SparePart).SparePartId).ToList(); //Id-ки уже введенного товара.
                List<SparePart> searchSparePartsList = (_lastEditCell.OwningColumn == TitleCol)
                    ? PartsDAL.SearchSparePartsByTitle(textBox.Text, existingSparePartsIdsList, false, 10 )
                    : PartsDAL.SearchSparePartsByArticul(textBox.Text, existingSparePartsIdsList, false, 10);

                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    //Заполняем вып. список новыми объектами.
                    autoCompleteListBox.DataSource = searchSparePartsList;
                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                    autoCompleteListBox.ClearSelected();
                }//if
            }//if
        }//dataGridViewTextBoxCell_TextChanged

        private void purchaseDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isCellEditError)
            {
                DataGridViewCell cell = purchaseDataGridView[e.ColumnIndex, e.RowIndex];

                if (cell.OwningColumn == TitleCol || cell.OwningColumn == ArticulCol) //Если редактируется артикул или название товара. 
                    TitleOrArticulCellFilled(cell);
                else if (cell.OwningColumn == CountCol)                            //Если редактируется кол-во. 
                    CountCellFilled(cell);
                else if (cell.OwningColumn == PriceCol)
                    PriceCellFilled(cell);
                else if (cell.OwningColumn == SellingPriceCol)                     //Если редактируется цена продажи. 
                    SellingPriceCellFilled(cell);
            }//if
        }//purchaseDataGridView_CellEndEdit                            

        private void purchaseDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //Если ошибка редактирования ячейки 'Title' или 'Articul', то возвращаем фокус обратно на ячейку (фокус теряется при выборе из вып. списка).
            if (_isCellEditError == true)
            {
                _isCellEditError = false;
                purchaseDataGridView.CurrentCell = _lastEditCell;

                //Включаем режим редактирования ячейки, не инициируя при этом соотв. события.
                purchaseDataGridView.CellBeginEdit         -= purchaseDataGridView_CellBeginEdit;
                purchaseDataGridView.EditingControlShowing -= purchaseDataGridView_EditingControlShowing;
                purchaseDataGridView.BeginEdit(true);
                purchaseDataGridView.CellBeginEdit         += purchaseDataGridView_CellBeginEdit;
                purchaseDataGridView.EditingControlShowing += purchaseDataGridView_EditingControlShowing;

                //ставим каретку в конец текста. 
                TextBox textBoxCell = _lastEditCell.Tag as TextBox;
                textBoxCell.SelectionStart = textBoxCell.Text.Length;
            }//if
        }//purchaseDataGridView_SelectionChanged

        private void purchaseDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                        purchaseDataGridView.SelectAll();
                    else
                        purchaseDataGridView.Rows[e.RowIndex].Selected = true;

                    //Выводим контекстное меню.
                    Point location = purchaseDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    purchaseContextMenuStrip.Show(purchaseDataGridView, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if 
        }//purchaseDataGridView_CellMouseClick        

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Выделяем строки всех выделенных ячеек.           
            purchaseDataGridView.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            //Удаляем все выбранные строки, eсли это не последняя строка (предназнач. для ввода нового товара в список), удаляем её.
            int lastRowIndx = purchaseDataGridView.Rows.Count - 1;
            purchaseDataGridView.SelectedRows.Cast<DataGridViewRow>().Where(r => r.Index != lastRowIndx).ToList().ForEach(r => purchaseDataGridView.Rows.Remove(r));

            FillTheInTotal(); //Заполняем общую сумму операции.
        }//removeToolStripMenuItem_Click





        #region Вспомогательные методы.
//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

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
                purchaseDataGridView_SelectionChanged(null, null);
        }//KeyUpPress


        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Артикул' и 'Название'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void TitleOrArticulCellFilled(DataGridViewCell cell)
        {
            if (cell.Value != null)
            {
                //Если есть такой товар в базе.
                if (autoCompleteListBox.Items.Count > 0)
                {
                    //если выбор сделан из выпадающего списка.
                    if (autoCompleteListBox.SelectedItem != null)
                        AutoCompleteRowInfo(cell, autoCompleteListBox.SelectedItem as SparePart); //Заполняем строку данными о товаре.                        
                    else  //если выбор не из вып. списка.
                        CellEndEditWrong(cell, "Выберите товар из списка.");
                }//if
                else
                {
                    _isCellEditError = true;
                    //Если такого товара нет в базе, даём возможность добавить его.
                    if (DialogResult.Yes == MessageBox.Show("Нет такого товара в базе. Добавить?", null, MessageBoxButtons.YesNo))
                        if (new AddSparePartForm().ShowDialog() == DialogResult.OK)
                            dataGridViewTextBoxCell_TextChanged(cell.Tag, null); //обновляем вып. список.
                }//else
            }//if

            //Если нет ошибки завершения редактирования ячейки, производим необх. действия.
            if (!_isCellEditError)
                CellEndEditCorrect(cell);
        }//TitleOrArticulCellFilled

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Количество'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void CountCellFilled(DataGridViewCell cell)
        {
            //Проверяем корректность ввода.
            string measureUnit = cell.OwningRow.Cells[MeasureUnitCol.Index].Value.ToString();
            if (!IsCountCellValueCorrect(cell, measureUnit))
            {
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000); //выводим всплывающее окно с сообщением об ошибке.
                cell.Value = null;
            }//if            

            FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
        }//CountCellFilled

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Цена'. 
        /// </summary>
        /// <param name="cell">Редактируемая ячейка.</param>
        private void PriceCellFilled(DataGridViewCell cell)
        {
            try
            {
                float price = Convert.ToSingle(cell.Value);
                if (price <= 0)
                    throw new Exception();  //ввод значения не более 0 также является ошибкой.

                cell.Value = price; //Перезаписываем установленную цену, для её форматированного вывода в ячейке.
                cell.OwningRow.Cells[SellingPriceCol.Index].Value = null; //очищаем цену продажи для дальнейшей установки дефолтного значения.
            }//try
            catch
            {
                //выводим всплывающее окно с сообщением об ошибке и очищаем ввод.
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                cell.Value = null;                
            }//catch

            SetMarkupAndSellingPriceCells(cell); //Записываем значения в ячейки 'Наценка' и 'ЦенаПродажи'.
            cell.OwningRow.Cells[SellingPriceCol.Index].ReadOnly = (cell.Value == null);//Задаём уровень доступа для ячейки 'ЦенаПродажи', в зависимости от корректности записи в ячейку 'Цена'.
            FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
        }//PriceCellFilled

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Цена продажи'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void SellingPriceCellFilled(DataGridViewCell cell)
        {            
            try
            {
                float sellingPrice = Convert.ToSingle(cell.Value);
                if (sellingPrice <= 0)
                    throw new Exception();  //ввод значения не более 0 также является ошибкой.
                   
                //Если цена продажи меньше или равна закупочной, требуем подтверждения.
                float price = Convert.ToSingle(cell.OwningRow.Cells[PriceCol.Index].Value);
                if (sellingPrice <= price)
                    if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        throw new Exception();
            }//try
            catch
            {
                //выводим всплывающее окно с сообщением об ошибке и очищаем ввод.
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                cell.Value = null;
            }//catch    

            SetMarkupAndSellingPriceCells(cell.OwningRow.Cells[PriceCol.Index]); //Записываем значения в ячейки 'Наценка' и 'ЦенаПродажи'.
        }//SellingPriceCellFilled

        /// <summary>
        /// Задает значения в ячейки 'Наценка' и 'ЦенаПродажи'. 
        /// </summary>
        /// <param name="priceCell">Ячейка столбца 'Цена'.</param>
        private void  SetMarkupAndSellingPriceCells(DataGridViewCell priceCell)
        {
            /*ERROR!!! Можно ли сделать метод проще?*/
            DataGridViewCell markupCell = priceCell.OwningRow.Cells[MarkupCol.Index];
            DataGridViewCell sellPriceCell = priceCell.OwningRow.Cells[SellingPriceCol.Index];            
            if (priceCell.Value != null)
            {
                float price  = (float)priceCell.Value;
                float markup = (markupCell.Tag != null) ? (float)markupCell.Tag : (float)Markup.Types.Retail; //Присваиваем дефолтную наценку, если она не установлена ранее.
                //рассчитываем цену продажи.
                float sellPrice = (sellPriceCell.Value == null) ? price + (price * markup / 100) : Convert.ToSingle(sellPriceCell.Value);

                sellPriceCell.Value = (float)(Math.Ceiling(sellPrice / 0.5) * 0.5); //Округляем в большую сторону с точностью до 0,5.                             
                markup = ((float)sellPriceCell.Value * 100 / price) - 100;  //расчитываем наценку исходя из установленной цены продажи.                    
                markupCell.Value = Models.Markup.GetDescription(markup);    //выводим тип наценки.
                markupCell.Tag = markup;                                    //запоминаем числовое значение наценки.
            }//if
            else
                markupCell.Value = markupCell.Tag = sellPriceCell.Value = null;   //если цена не установлена, обнуляем значения наценки и цены продажи.
        }//SetMarkupAndSellingPriceCells

        /// <summary>
        /// Автозаполнение строки соотв. инф-цией.
        /// </summary>
        /// <param name="countCell">Заполняемая ячейка.</param>
        /// <param name="titleAndArticul">Массив строк с артикулом и названием.</param>
        private void AutoCompleteRowInfo(DataGridViewCell cell, SparePart sparePart)
        {
            FillThePurchaseDGV(cell.OwningRow, sparePart);

            cell.OwningRow.Cells[PriceCol.Index].ReadOnly = cell.OwningRow.Cells[CountCol.Index].ReadOnly = false;
            cell.OwningRow.Cells[TitleCol.Index].ReadOnly = cell.OwningRow.Cells[ArticulCol.Index].ReadOnly = true;

            autoCompleteListBox.Visible = false;

            #region Увеличение purchaseGroupBox.
            //if (saleDataGridView.PreferredSize.Height > saleDataGridView.Size.Height)
            //{
            //    MessageBox.Show("bigger");
            //    int height = saleDataGridView.Rows[0].Cells["Title"].Size.Height;
            //    saleGroupBox.Size = new Size(saleGroupBox.Width, saleGroupBox.Height + height);
            //}
            #endregion
        }//AutoCompleteRowInfo

        /// <summary>
        /// Действия при некорректном завершении редактирования ячейки.
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="toolTipText">Текст всплывающей подсказки</param>
        private void CellEndEditWrong(DataGridViewCell cell, string toolTipText)
        {
            toolTip.Show(toolTipText, this, GetCellBelowLocation(cell), 1000);
            _isCellEditError = true;
        }//CellEndEditWrong

        /// <summary>
        /// Действия при корректном завершении редактирования ячейки.
        /// </summary>
        /// <param name="cell">Ячейка</param>
        private void CellEndEditCorrect(DataGridViewCell cell)
        {
            //Отписываем editing control от событий обработки ввода.
            TextBox textBoxCell = cell.Tag as TextBox;
            textBoxCell.TextChanged -= dataGridViewTextBoxCell_TextChanged;
            textBoxCell.PreviewKeyDown -= dataGridViewTextBoxCell_PreviewKeyDown;
            cell.Tag = null;
        }//CellEndEditCorrect

        /// <summary>
        /// Заполняет осн. таблицу данными.
        /// </summary>
        /// <param name="row">Заполняемая строка.</param>
        /// <param name="sparePart">Данные для заполнения строки.</param>
        private void FillThePurchaseDGV(DataGridViewRow row, SparePart sparePart)
        {
            row.Tag = sparePart;

            row.Cells[TitleCol.Index].Value = sparePart.Title;
            row.Cells[ArticulCol.Index].Value = sparePart.Articul;
            row.Cells[MeasureUnitCol.Index].Value = sparePart.MeasureUnit;
        }//FillThePurchaseDGV

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

            //Ввод значения не более 0, является ошибкой. 
            if (count <= 0)
                return false;

            //Проверяем является ли введенное число корректным для продажи, т.е. кратно ли оно минимальной единице продажи.     
            if (count % Models.MeasureUnit.GetMinUnitSale(measureUnit) != 0)
                return false;

            return true;
        }//IsCountCellValueCorrect

        /// <summary>
        /// Заполняет ячейку 'Сумма' заданной строки и общую сумму.
        /// </summary>
        /// <param name="row">Строка дял которой производятся вычисления и заполнение.</param>
        private void FillTheSumCell(DataGridViewRow row)
        {
            if (row.Cells[CountCol.Index].Value != null && row.Cells[PriceCol.Index].Value != null)
            {
                float price = Convert.ToSingle(row.Cells[PriceCol.Index].Value);
                float count = Convert.ToSingle(row.Cells[CountCol.Index].Value);

                row.Cells[SumCol.Index].Value = price * count;
            }//if
            else
            {
                row.Cells[SumCol.Index].Value = null;//очищаем ячейку. 
            }//else

            FillTheInTotal(); //Заполняем общую сумму операции.
        }//FillTheSumCell

        /// <summary>
        /// Заполняет InTotalLabel корретным значением.
        /// </summary>
        private void FillTheInTotal()
        {
            float inTotal = 0;
            foreach (DataGridViewRow row in purchaseDataGridView.Rows)
            {
                //Если в строке указана и цена и количестов.
                if (row.Cells[SumCol.Index].Value != null)
                {
                    float price = Convert.ToSingle(row.Cells[PriceCol.Index].Value);
                    float count = Convert.ToSingle(row.Cells[CountCol.Index].Value);
                    inTotal += price * count;
                }//if
            }//foreach

            //Заполняем InTotalLabel расчитанным значением.
            inTotalNumberLabel.Text = String.Format("{0}(руб)", Math.Round(inTotal, 2, MidpointRounding.AwayFromZero));
        }//FillTheInTotal


        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из purchaseDataGridView. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = purchaseDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = purchaseDataGridView.Location;
            Point gbLoc = purchaseGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);        
        }//GetCellBelowLocation









//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
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
                purchaseDataGridView_SelectionChanged(null, null);
                _isCellEditError = true;
            }//if
            else
            {
                //Делаем автозаполнение строки, выбранным объектом.   
                _isCellEditError = false;
                purchaseDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(_lastEditCell.ColumnIndex, _lastEditCell.RowIndex));
            }//else
        }//autoCompleteListBox_MouseDown

        private void autoCompleteListBox_DataSourceChanged(object sender, EventArgs e)
        {
            if (autoCompleteListBox.DataSource != null)
            {
                List<SparePart> spList = autoCompleteListBox.DataSource as List<SparePart>;
                //Форматируем вывод.
                //Находим максимальную ширину каждого параметра.
                int articulMaxLenght = spList.Max(sp => sp.Articul.Length);
                int titlelMaxLenght  = spList.Max(sp => sp.Title.Length);

                //Запоминаем ширину всех столбцов.
                autoCompleteListBox.Tag = new Tuple<int, int>(articulMaxLenght, titlelMaxLenght);

                autoCompleteListBox.Visible = true;
            }//if
            else
                autoCompleteListBox.Visible = false;
        }//autoCompleteListBox_DataSourceChanged

        private void autoCompleteListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            //Находим максимальную ширину каждого параметра.            
            Tuple<int, int> columnsWidth = autoCompleteListBox.Tag as Tuple<int, int>;
            int articulMaxLenght = columnsWidth.Item1;
            int titlelMaxLenght  = columnsWidth.Item2;

            //Задаём нужный формат для выводимых строк.
            string artCol   = String.Format("{{0, {0}}}", -articulMaxLenght);
            string titleCol = String.Format("{{1, {0}}}", -titlelMaxLenght);

            SparePart sparePart = e.ListItem as SparePart;
            e.Value = String.Format(artCol + "   " + titleCol, sparePart.Articul, sparePart.Title);
        }//autoCompleteListBox_Format

        #endregion


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы вывода инф-ции в Excel.

        private void BeginLoadPurchaseToExcelFile(object availList)
        {
            LoadPurchaseToExcelFile(availList as List<Availability>);
        }//BeginLoadPurchaseToExcelFile
     
        /// <summary>
        /// Метод вывода приходной информации в Excel-файл.
        /// </summary>
        /// <param name="sale">Информация о приходе.</param>
        /// <param name="availabilityList">Список оприходованных товаров.</param>
        private void LoadPurchaseToExcelFile(List<Availability> availList)
        {
            Purchase purchase = availList[0].OperationDetails.Operation as Purchase;
            List<SparePart> sparePartsList = availList.Select(av => av.OperationDetails.SparePart).ToList();
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin   = 7;
            ExcelWorkSheet.PageSetup.RightMargin  = 7;
            ExcelWorkSheet.PageSetup.TopMargin    = 10;
            ExcelWorkSheet.PageSetup.BottomMargin = 10;

            int row = 1, column = 1;

            //Выводим Id и Дату. 
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Приходная накладная №{0} от {1}г.", purchase.OperationId, purchase.OperationDate.ToString("dd/MM/yyyy"));

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}", 
                                                         supplierLabel.Text + " " + supplierTextBox.Text,
                                                         buyerLabel.Text + " " + buyerTextBox.Text);

            #region Вывод таблицы товаров.

            row += 2;
            //Выводим заголовок.
            ExcelApp.Cells[row, column]     = "Произв.";
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

            SetColumnsWidth(sparePartsList, (ExcelApp.Cells[row, column + 2] as Excel.Range), (ExcelApp.Cells[row, column + 1] as Excel.Range), (ExcelApp.Cells[row, column] as Excel.Range));
            //Выводим список товаров.
            foreach (Availability avail in availList)            
            {
                ++row;
                string title = avail.OperationDetails.SparePart.Title, articul = avail.OperationDetails.SparePart.Articul;
                ExcelApp.Cells[row, column + 1] = articul;
                ExcelApp.Cells[row, column + 2] = title;
                
                //Выравнивание диапазона строк.
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
                if (articul.Length > articulColWidth || title.Length > titleColWidth)
                {
                    ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
                    //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
                    if (articul.Length > articulColWidth && title.Length <= titleColWidth)
                        (ExcelApp.Cells[row, column + 2] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    if (articul.Length <= articulColWidth && title.Length > titleColWidth)
                        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }//if

                ExcelApp.Cells[row, column] = avail.OperationDetails.SparePart.Manufacturer;

                ExcelApp.Cells[row, column + 3] = avail.OperationDetails.SparePart.MeasureUnit;

                ExcelApp.Cells[row, column + 4] = avail.OperationDetails.Count;
                ExcelApp.Cells[row, column + 5] = avail.OperationDetails.Price;
                ExcelApp.Cells[row, column + 6] = avail.OperationDetails.Price * avail.OperationDetails.Count;
            }//foreach

            //Обводим талицу рамкой. 
            excelCells = ExcelWorkSheet.get_Range("A" + (row - sparePartsList.Count + 1).ToString(), "G" + row.ToString());
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

            /*ERROR Передавать имена агентов параметром и закрывать owner-форму не здесь, а в OkButton_Click*/
            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas"; //моноширинный шрифт
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}", 
                                                         supplierAgentLabel.Text + " " + supplierAgentTextBox.Text,
                                                         buyerAgentLabel.Text + " " + buyerAgentTextBox.Text);
            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;

            ExcelApp.Cells[row, column].Value = "                                                                                                                                                                                                                                 ";//longEmptyString.ToString();
            (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
            //Выводим заметку
            row++;
            // объединим область ячеек  строки "вместе"
            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.WrapText = true;
            excelCells.Value = purchase.Description;//descriptionRichTextBox.Text;
            AutoFitMergedCellRowHeight((ExcelApp.Cells[row, column] as Excel.Range));

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;


            //Закрываем форму (будет ошибка при отладке закрытия не из того потока), здесь потому что, если закрыть в okButton_click не будет выводится inTotal в Excel.
            this.Close();
        }//LoadPurchaseToExcelFile  

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleCol">Столбец "Название".</param>
        /// <param name="articulCol">Столбец "Артикул".</param>
        /// <param name="manufCol">Столбец "Производитель".</param>
        private void SetColumnsWidth(IList<SparePart> spareParts, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = spareParts.Select(sp => sp.Manufacturer).Where(man => man != null);
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

        private void currencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyComboBox.Text == "руб")
            {
                excRateNumericUpDown.Value = 1;
                excRateNumericUpDown.Enabled = false;
                purchaseDataGridView.Enabled = true;
                currencyComboBox.Enabled = false;
                helpLabel.Dispose();
            }//if
            else excRateNumericUpDown.Enabled = true;

            markupCheckBox.Enabled = true;
        }//currencyComboBox_SelectedIndexChanged

        private void excRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (excRateNumericUpDown.Value > excRateNumericUpDown.Minimum)
            {
                purchaseDataGridView.Enabled = true;
            }
            else purchaseDataGridView.Enabled = false;
            //foreach (DataGridViewRow row in purchaseDataGridView.Rows)
            //{
            //    if (row.Cells["Price"].Value != null)
            //    {
            //        row.Cells["Price"].Value = Math.Round(((double)row.Cells["Price"].Value * (double)excRateNumericUpDown.Value), 2, MidpointRounding.AwayFromZero);                                                           
            //        double price = Convert.ToDouble(row.Cells["Price"].Value);
            //        double excRate = (double)excRateNumericUpDown.Value;
            //        double newPrice = Math.Round(price * excRate, 2, MidpointRounding.AwayFromZero);
            //        row.Cells["Price"].Value = newPrice;
            //        if (row.Cells["Count"].Value != null)
            //        {
            //            //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
            //            if (row.Cells["Sum"].Value != null)
            //                inTotal -= Convert.ToDouble((row.Cells["Sum"].Value));

            //            row.Cells["Sum"].Value = newPrice * Convert.ToDouble(row.Cells["Count"].Value);
            //            inTotal += newPrice * Convert.ToDouble(row.Cells["Count"].Value);
            //            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
            //        }//if
            //    }//if
            //}//foreach
        }//excRateNumericUpDown_ValueChanged

        private void excRateNumericUpDown_Leave(object sender, EventArgs e)
        {
            if (excRateNumericUpDown.Value > excRateNumericUpDown.Minimum)
            {
                excRateNumericUpDown.Enabled = false;
                //purchaseDataGridView.Enabled = true;
                currencyComboBox.Enabled = false;
                helpLabel.Dispose();
            }//if
            else
                toolTip.Show("Выберите курс к рос. рублю", this, excRateNumericUpDown.Location, 3000);
        }//excRateNumericUpDown_Leave       



        #region Методы связанные с изменением Наценки.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        private void markupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Задаем видимость столбцов 'ТипНаценки' и 'ЦенаПродажи' в зависимости от состояния checkedBox.
            MarkupCol.Visible = SellingPriceCol.Visible = markupComboBox.Visible = markupCheckBox.Checked;                 
        }//markupCheckBox_CheckedChanged     

        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                markupComboBox_SelectedIndexChanged(sender, e);
        }//markupComboBox_PreviewKeyDown 

        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //Если нет выделенных строк, то выходим.
            if (purchaseDataGridView.SelectedCells.Count == 0)
                return;

            //выделяем строки всех выделенных клеток.
            purchaseDataGridView.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);

            try
            {
                //узнаем процент заданной наценки.
                float markupValue = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                string markupType = Models.Markup.GetDescription(markupValue);

                //Обновляем таблицу.
                foreach (DataGridViewRow row in purchaseDataGridView.SelectedRows)
                {
                    //Если указана цена.
                    if (row.Cells[PriceCol.Index].Value != null)
                    {                        
                        row.Cells[SellingPriceCol.Index].Value = null; //Очищаем цену продажи для корректного заполнения. 
                        row.Cells[MarkupCol.Index].Tag = markupValue;
                        //Выставляем значенияб округленные с точностью 0,5 наценки и цены продажи. 
                        SetMarkupAndSellingPriceCells(row.Cells[PriceCol.Index]);
                    }//if                    
                }//foreach
            }//try
            catch
            {
                toolTip.Show("Введено некорректное значение.", this, markupComboBox.Location, 2000);
            }//catch
        }//markupComboBox_SelectedIndexChanged





//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Возвращает объект типа Operation, созданный из данных формы.
        /// </summary>
        /// <returns></returns>
        public Purchase CreatePurchaseFromForm()
        {
            //Находим контрагента. Если такого ещё нет в базе, то создаем новый объект.
            IContragent supplier = PartsDAL.FindSuppliers(supplierTextBox.Text.Trim());
            supplier = (supplier == null) ? new Supplier(0, supplierTextBox.Text.Trim(), null, null, null, null) : supplier;

            Purchase purchase = new Purchase
            (
                employee           : Form1.CurEmployee,
                contragent         : supplier,
                contragentEmployee : (!String.IsNullOrWhiteSpace(supplierAgentTextBox.Text)) ? supplierAgentTextBox.Text.Trim() : null,
                operationDate      : purchaseDateTimePicker.Value,
                description        : (!String.IsNullOrWhiteSpace(descriptionRichTextBox.Text)) ? descriptionRichTextBox.Text.Trim() : null,
                operDetList        : null                                               
            );

            return purchase;
        }//CreatePurchaseFromForm

        /// <summary>
        /// Возвращает список объектов типа OperationDetails, созданный из данных таблицы продаж.
        /// </summary>
        /// <returns></returns>
        private List<Availability> CreateAvailabilityListFromForm()
        {
            List<Availability> availList = new List<Availability>();
            Purchase purchase = CreatePurchaseFromForm();
            foreach(DataGridViewRow row in purchaseDataGridView.Rows)
            {
                //Если строка не пустая.
                if (row.Tag != null)
                {
                    float count = Convert.ToSingle(row.Cells[CountCol.Index].Value);
                    float price = Convert.ToSingle(row.Cells[PriceCol.Index].Value);


                    SparePart sparePart = row.Tag as SparePart;                    
                    OperationDetails operDet = new OperationDetails(sparePart, purchase, count, price);

                    Availability avail = new Availability
                    (
                        operationDetails : operDet,
                        storageAddress   : (String.IsNullOrWhiteSpace(storageAdressTextBox.Text)) ? null : storageAdressTextBox.Text.Trim(),
                        markup: (row.Cells[MarkupCol.Index].Tag != null) ? Convert.ToSingle(row.Cells[MarkupCol.Index].Tag) : 0
                    );
                    availList.Add(avail);
                }//if
            }//foreach

            return availList;
        }//CreateAvailabilityListFromForm

        /// <summary>
        /// Возвращает true если все обязательные поля корректно заполнены, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool IsRequiredFieldsValid()
        {
            //Находим все BackPanel-контролы на форме. 
            List<Control> curAccBackControls = this.GetAllControls(typeof(Panel), "BackPanel");

            supplierTextBox_Leave(null, null);
            buyerTextBox_Leave(null, null);

            //Если хоть один контрол не прошел валидацию, возв-ем false.
            if (curAccBackControls.Any(backPanel => backPanel.BackColor == Color.Red))
                return false;

            //Если таблица не заполнена или не везде указана цена или кол-во.
            if (purchaseDataGridView.Rows.Cast<DataGridViewRow>().All(r => r.Tag == null) || purchaseDataGridView.Rows.Cast<DataGridViewRow>().Any(r => r.Tag != null && (r.Cells[PriceCol.Index].Value == null || r.Cells[CountCol.Index].Value == null)))
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
                {
                    this.Close();
                }
            }//if
        }//cancelButton_MouseClick

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Если всё заполненно корректно.
                if (IsRequiredFieldsValid())
                {
                    List<Availability> availList = CreateAvailabilityListFromForm();

                    try
                    {                        
                        availList[0].OperationDetails.Operation.OperationId = PartsDAL.AddPurchase(availList);
                    }//try
                    catch (Exception)
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        return;
                    }//catch 

                    //LoadsaleToExcelFile(availList, availabilityList);
                    /*!!!*/
                    new System.Threading.Thread(BeginLoadPurchaseToExcelFile).Start(availList); //Сделать по нормальному вызов с потоком.

                    this.Visible = false;
                    //this.Close();
                }//if
            }//if
        }//



        

        


    }//PurchaseForm
    

}//namespace

/*Задачи*/
//0)!!!Учет выставленного курса валюты.
//1)Сделать редактирование ввода и вывода цены, до двух десятичных знаков!
//2)Добавить возможность задавать наценку и цену продажи вручную.
//3)Грузить список валют из базы!
//4)Сделать наценку отдельным классом.

/*Будущие задачи*/
//1)Сделать нормальную обработку неправильного ввода в dataGridViewCell, а именно возврат курсора в очищенную клетку.
//2)Выпадающий список(listBox) в dgv в первый раз принимает неправильный размер.
//3)Посмотреть модификацию вып. списков с DisplayMember и ValueMember.
//4)Чтобы при изменении валюты в inTotalNumberLabel сразу изменялось обозначение (руб) на выбранное. 
//5)Добавить возможность вариации валюты.
//6)Сделать удаление всех выделенных строк.
//7)Сделать автоувеличение PurchaseGroupBox.
//8)Улучшить вывод в Excel в частности:
     //8.1)Колонка "Сумма" при большом числе выводит что-то типо "8е+23", сделать нормально.   
//9)Добавить возможность добавления новой валюты в базу.