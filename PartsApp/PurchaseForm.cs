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
    public partial class PurchaseForm : Form
    {
        bool _isCellEditError = false;
        DataGridViewCell _lastEditCell;


        public PurchaseForm()
        {
            InitializeComponent();
        }//


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
                PurchaseDGV.Select(); //переводим фокус на таблицу приходов.
            }//if
        }//ContragentTextBox_PreviewKeyDown

        private void supplierTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(supplierTextBox.Text))
            {
                ControlValidation.WrongValueInput(toolTip, supplierTextBox);
            }//if
            else
            {                
                //Если такой контрагент в базе отсутствует, выводим сообщение об этом.
                string supplier = supplierTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == supplierTextBox.Text.Trim().ToLower());
                if (supplier != null)
                {
                    ControlValidation.CorrectValueInput(toolTip, supplierTextBox);
                    supplierTextBox.Text = supplier; //Выводим корректное имя контрагента.
                }//if
                else
                {
                    ControlValidation.WrongValueInput(toolTip, supplierTextBox, "Такого клиента нет в базе! Он будет добавлен.", Color.Yellow);
                }//else
            }//else  
        }//AgentTextBox_Leave

        private void buyerTextBox_Leave(object sender, EventArgs e)
        {
            ControlValidation.IsInputControlEmpty(buyerTextBox, toolTip);            
        }//buyerTextBox_Leave





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
                
        #region Методы работы с таблицей.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PurchaseDGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Нумерация строк.
            EnumerableExtensions.RowsNumerateAndAutoSize(PurchaseDGV.Rows[e.RowIndex]);
        }//

        /*События идут в порядке их возможного вызова.*/

        private void PurchaseDGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //Делаем ReadOnly ячейки 'Цена', 'Кол-во' и 'Цена Продажи'.
            dgv[SellingPriceCol.Index, e.RowIndex].ReadOnly = dgv[CountCol.Index, e.RowIndex].ReadOnly = dgv[PriceCol.Index, e.RowIndex].ReadOnly = true;
        }//PurchaseDGV_RowsAdded

        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PurchaseDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _lastEditCell = PurchaseDGV[e.ColumnIndex, e.RowIndex]; //запоминаем текущую ячейку как последнюю редактируемую.

            //Обрабатываем ввод в ячейку 'Название' или 'Артикул'.
            if (_lastEditCell.OwningColumn == TitleCol || _lastEditCell.OwningColumn == ArticulCol)
                autoCompleteListBox.Location = GetCellBelowLocation(_lastEditCell); //устанавливаем позицию вып. списка.
        }//PurchaseDGV_CellBeginEdit

        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в ячейку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PurchaseDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {            
            DataGridViewCell cell = PurchaseDGV.CurrentCell;

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
        }//PurchaseDGV_EditingControlShowing

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
                    _isCellEditError = true;
                    AutoCompleteListBox.KeyDownPress(autoCompleteListBox);
                    break;
                case Keys.Up:
                    _isCellEditError = true;
                    AutoCompleteListBox.KeyUpPress(autoCompleteListBox);
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
                List<int> existingSparePartsIdsList = PurchaseDGV.Rows.Cast<DataGridViewRow>().Where(r => r.Tag != null).Select(r => (r.Tag as SparePart).SparePartId).ToList(); //Id-ки уже введенного товара.
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

        private void PurchaseDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isCellEditError)
            {
                DataGridViewCell cell = PurchaseDGV[e.ColumnIndex, e.RowIndex];

                if (cell.OwningColumn == TitleCol || cell.OwningColumn == ArticulCol) //Если редактируется артикул или название товара. 
                    TitleOrArticulCellFilled(cell);
                else if (cell.OwningColumn == CountCol)                            //Если редактируется кол-во. 
                    CountCellFilled(cell);
                else if (cell.OwningColumn == PriceCol)
                    PriceCellFilled(cell);
                else if (cell.OwningColumn == SellingPriceCol)                     //Если редактируется цена продажи. 
                    SellingPriceCellFilled(cell);
            }//if
        }//PurchaseDGV_CellEndEdit                            

        private void PurchaseDGV_SelectionChanged(object sender, EventArgs e)
        {
            //Если ошибка редактирования ячейки 'Title' или 'Articul', то возвращаем фокус обратно на ячейку (фокус теряется при выборе из вып. списка).
            if (_isCellEditError == true)
            {
                _isCellEditError = false;
                PurchaseDGV.CurrentCell = _lastEditCell;

                //Включаем режим редактирования ячейки, не инициируя при этом соотв. события.
                PurchaseDGV.CellBeginEdit         -= PurchaseDGV_CellBeginEdit;
                PurchaseDGV.EditingControlShowing -= PurchaseDGV_EditingControlShowing;
                PurchaseDGV.BeginEdit(true);
                PurchaseDGV.CellBeginEdit         += PurchaseDGV_CellBeginEdit;
                PurchaseDGV.EditingControlShowing += PurchaseDGV_EditingControlShowing;

                //ставим каретку в конец текста. 
                TextBox textBoxCell = _lastEditCell.Tag as TextBox;
                textBoxCell.SelectionStart = textBoxCell.Text.Length;
            }//if
        }//PurchaseDGV_SelectionChanged

        private void PurchaseDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                        PurchaseDGV.SelectAll();
                    else
                        PurchaseDGV.Rows[e.RowIndex].Selected = true;

                    //Выводим контекстное меню.
                    Point location = PurchaseDGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    purchaseContextMenuStrip.Show(PurchaseDGV, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if 
        }//PurchaseDGV_CellMouseClick        

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Выделяем строки всех выделенных ячеек.           
            PurchaseDGV.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            //Удаляем все выбранные строки, eсли это не последняя строка (предназнач. для ввода нового товара в список), удаляем её.
            int lastRowIndx = PurchaseDGV.Rows.Count - 1;
            PurchaseDGV.SelectedRows.Cast<DataGridViewRow>().Where(r => r.Index != lastRowIndx).ToList().ForEach(r => PurchaseDGV.Rows.Remove(r));

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
                        if (new SparePartForm().ShowDialog() == DialogResult.OK)
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
            //if (saleDGV.PreferredSize.Height > saleDGV.Size.Height)
            //{
            //    MessageBox.Show("bigger");
            //    int height = saleDGV.Rows[0].Cells["Title"].Size.Height;
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
        /// <param name="extRow">Заполняемая строка.</param>
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
        /// <param name="extRow">Строка дял которой производятся вычисления и заполнение.</param>
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
            foreach (DataGridViewRow row in PurchaseDGV.Rows)
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
        /// Возвращает абсолютный location области сразу под позицией клетки из PurchaseDGV. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = PurchaseDGV.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = PurchaseDGV.Location;
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
                PurchaseDGV_SelectionChanged(null, null);
                _isCellEditError = true;
            }//if
            else
            {
                //Делаем автозаполнение строки, выбранным объектом.   
                _isCellEditError = false;
                PurchaseDGV_CellEndEdit(null, new DataGridViewCellEventArgs(_lastEditCell.ColumnIndex, _lastEditCell.RowIndex));
            }//else
        }//autoCompleteListBox_MouseDown

        private void autoCompleteListBox_DataSourceChanged(object sender, EventArgs e)
        {
            AutoCompleteListBox.DataSourceChanged(autoCompleteListBox);
        }//autoCompleteListBox_DataSourceChanged

        /// <summary>
        /// Форматирование вывода в ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            AutoCompleteListBox.OutputFormatting(autoCompleteListBox, e);
        }//autoCompleteListBox_Format

        #endregion


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы вывода инф-ции в Excel.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        /// <param name="agent">Фирма-покупатель.</param>
        private async void saveInExcelAsync(IList<OperationDetails> operDetList, string agent)
        {
            try
            {
                await Task.Factory.StartNew(() => saveInExcel(operDetList, agent));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }//saveInExcelAsync        
     
        /// <summary>
        /// Метод вывода приходной информации в Excel-файл.
        /// </summary>
        /// <param name="availabilityList">Список оприходованных товаров.</param>
        /// <param name="agent">Фирма-покупатель.</param>
        private void saveInExcel(IList<OperationDetails> operDetList, string agent)
        {
            Purchase purchase = operDetList[0].Operation as Purchase;
            List<SparePart> sparePartsList = operDetList.Select(od => od.SparePart).ToList();

            Excel.Application ExcelApp     = new Excel.Application();
            Excel.Workbook ExcelWorkBook   = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.
            
            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin  = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin  = 7;

            int row = 1, column = 1;

            //Выводим Id и Дату. 
            OperationIdAndDateExcelOutput(ExcelWorkSheet, purchase, row, column);

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}",
                                                         "Поставщик : " + purchase.Contragent.ContragentName,
                                                         "Покупатель : " + agent);

            //Заполняем таблицу.
            FillTheExcelList(ExcelWorkSheet, operDetList, ref row, column);

            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas"; //моноширинный шрифт
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}",
                                                         "Выписал : " + purchase.ContragentEmployee,
                                                         "Принял : " +  Form1.CurEmployee.LastName + " " + Form1.CurEmployee.FirstName);
            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;

            //Выводим заметку к операции.
            DescriptionExcelOutput(ExcelWorkSheet, purchase.Description, ref row, column);

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = ExcelApp.UserControl = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.            
        }//saveInExcel  


        /// <summary>
        /// Заполняем Excel инф-цией из переданного списка.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="operDetList">Список деталей операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private void FillTheExcelList(Excel.Worksheet ExcelWorkSheet, IList<OperationDetails> operDetList, ref int row, int column)
        {
            row += 2;
            //Выводим заголовок.
            FillTheTitlesRow(ExcelWorkSheet, row, column);

            //Уменьшаем ширину колонки "Ед. изм."
            ExcelWorkSheet.Cells[row, column + 3].VerticalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            ExcelWorkSheet.Cells[row, column + 3].Columns.ColumnWidth = 5;

            //Устанавливаем ширину столбцов.
            int titleColWidth = 30, articulColWidth = 20; // -- Взято методом тыка.  
            SetColumnsWidth(operDetList, ExcelWorkSheet.Cells[row, column + 2], ExcelWorkSheet.Cells[row, column + 1], ExcelWorkSheet.Cells[row, column]);

            //Выводим список товаров.
            float inTotal = 0;
            foreach (OperationDetails operDet in operDetList)
            {
                FillExcelRow(ExcelWorkSheet, operDet, ++row, column, titleColWidth, articulColWidth);
                inTotal += operDet.Price * operDet.Count;
            }//foreach

            //Обводим талицу рамкой. 
            ExcelWorkSheet.get_Range("A" + (row - operDetList.Count + 1).ToString(), "G" + row.ToString()).Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            ++row;
            //Выводим 'Итого'.
            InTotalExcelOutput(ExcelWorkSheet, inTotal, row, column);
        }//FillTheExcelList

        /// <summary>
        /// Заполняет строку заголовками для таблицы.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private void FillTheTitlesRow(Excel.Worksheet ExcelWorkSheet, int row, int column)
        {
            //Заполняем заголовки строк.
            ExcelWorkSheet.Cells[row, column] = "Произв.";
            ExcelWorkSheet.Cells[row, column + 1] = "Артикул";
            ExcelWorkSheet.Cells[row, column + 2] = "Название";
            ExcelWorkSheet.Cells[row, column + 3] = "Ед. изм.";
            ExcelWorkSheet.Cells[row, column + 4] = "Кол-во";
            ExcelWorkSheet.Cells[row, column + 5] = "Цена";
            ExcelWorkSheet.Cells[row, column + 6] = "Сумма";

            //Настраиваем вид клеток.
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Font.Bold = true;
            excelCells.Font.Size = 12;
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack; //Обводим заголовки таблицы рамкой.            
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium; //Устанавливаем стиль и толщину линии
        }//FillTheTitlesRow

        /// <summary>
        /// Заполянет строку данными из переданного объекта.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="sparePart">Объект товара.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        /// <param name="titleColWidth">ширина столбца 'Название'.</param>
        /// <param name="articulColWidth">ширина столбца 'Артикул'.</param>
        private void FillExcelRow(Excel.Worksheet ExcelWorkSheet, OperationDetails operDet, int row, int column, int titleColWidth, int articulColWidth)
        {
            ExcelWorkSheet.Cells[row, column + 1] = operDet.SparePart.Articul;
            ExcelWorkSheet.Cells[row, column + 2] = operDet.SparePart.Title;
            //Выравнивание диапазона строк.
            ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).VerticalAlignment = Excel.Constants.xlTop;
            ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
            if (operDet.SparePart.Articul.Length > articulColWidth || operDet.SparePart.Title.Length > titleColWidth)
                IncreaseRowHeight(ExcelWorkSheet, operDet.SparePart, row, column, titleColWidth, articulColWidth);

            ExcelWorkSheet.Cells[row, column] = operDet.SparePart.Manufacturer;
            ExcelWorkSheet.Cells[row, column + 3] = operDet.SparePart.MeasureUnit;
            ExcelWorkSheet.Cells[row, column + 4] = operDet.Count;
            ExcelWorkSheet.Cells[row, column + 5] = operDet.Price;
            ExcelWorkSheet.Cells[row, column + 6] = operDet.Price * operDet.Count;
        }//FillExcelRow


        /// <summary>
        /// Увеличивает ширину строки.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="sparePart">Объкт товара.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        /// <param name="titleColWidth">Ширина столбца для Названия товара.</param>
        /// <param name="articulColWidth">Ширина столбца для Артикула товара.</param>
        private void IncreaseRowHeight(Excel.Worksheet ExcelWorkSheet, SparePart sparePart, int row, int column, int titleColWidth, int articulColWidth)
        {
            ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
            if (sparePart.Articul.Length > articulColWidth && sparePart.Title.Length <= titleColWidth)
                ExcelWorkSheet.Cells[row, column + 2].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            if (sparePart.Articul.Length <= articulColWidth && sparePart.Title.Length > titleColWidth)
                ExcelWorkSheet.Cells[row, column + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        }//IncreaseRowHeight

        /// <summary>
        /// Выводим 'Итого' в заданной клетке.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист.</param>
        /// <param name="inTotal">Общая сумма операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private void InTotalExcelOutput(Excel.Worksheet ExcelWorkSheet, float inTotal, int row, int column)
        {
            //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            int indent = 0; //отступ
            if (inTotal.ToString("0.00").Length <= 9)
                indent = 1;

            ExcelWorkSheet.Cells[row, column + 4 + indent] = "Итого : ";
            ExcelWorkSheet.Cells[row, column + 5 + indent] = inTotal.ToString("0.00");
            ExcelWorkSheet.Cells[row, column + 5 + indent].Font.Underline = true;
            ExcelWorkSheet.Cells[row, column + 5 + indent].Font.Size = ExcelWorkSheet.Cells[row, column + 4 + indent].Font.Size = 12;
            ExcelWorkSheet.Cells[row, column + 5 + indent].Font.Bold = ExcelWorkSheet.Cells[row, column + 4 + indent].Font.Bold = true;
        }//InTotalExcelOutput

        /// <summary>
        /// Заполняет заданную строку Id операции и датой.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="purchase">Объект операции.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private void OperationIdAndDateExcelOutput(Excel.Worksheet ExcelWorkSheet, Purchase purchase, int row, int column)
        {
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Расходная накладная №{0} от {1}г.", purchase.OperationId, purchase.OperationDate.ToString("dd/MM/yyyy"));
        }//OperationIdAndDateExcelOutput

        /// <summary>
        /// Выводит заметку об операции.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="description">заметка</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private void DescriptionExcelOutput(Excel.Worksheet ExcelWorkSheet, string description, ref int row, int column)
        {
            if (description != null)
            {
                //Делаем визуальное отделение информации от заметки, с помощью линии.
                ExcelWorkSheet.Cells[row, column].Value = "                                                                                                                                                                                                                                 ";//longEmptyString.ToString();
                ExcelWorkSheet.Cells[row, column].Font.Underline = true;
                //Выводим заметку
                row++;
                // объединим область ячеек  строки "вместе"? для вывода операции.
                Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
                excelCells.Merge(true);
                excelCells.WrapText = true;
                excelCells.Value = description;
                AutoFitMergedCellRowHeight((ExcelWorkSheet.Cells[row, column] as Excel.Range));
            }//if
        }//DescriptionExcelOutput


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







/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void currencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyComboBox.Text == "руб")
            {
                excRateNumericUpDown.Value = 1;
                excRateNumericUpDown.Enabled = false;
                PurchaseDGV.Enabled = true;
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
                PurchaseDGV.Enabled = true;
            }
            else PurchaseDGV.Enabled = false;
            //foreach (DataGridViewRow extRow in PurchaseDGV.Rows)
            //{
            //    if (extRow.Cells["Price"].Value != null)
            //    {
            //        extRow.Cells["Price"].Value = Math.Round(((double)extRow.Cells["Price"].Value * (double)excRateNumericUpDown.Value), 2, MidpointRounding.AwayFromZero);                                                           
            //        double price = Convert.ToDouble(extRow.Cells["Price"].Value);
            //        double excRate = (double)excRateNumericUpDown.Value;
            //        double newPrice = Math.Round(price * excRate, 2, MidpointRounding.AwayFromZero);
            //        extRow.Cells["Price"].Value = newPrice;
            //        if (extRow.Cells["Count"].Value != null)
            //        {
            //            //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
            //            if (extRow.Cells["Sum"].Value != null)
            //                inTotal -= Convert.ToDouble((extRow.Cells["Sum"].Value));

            //            extRow.Cells["Sum"].Value = newPrice * Convert.ToDouble(extRow.Cells["Count"].Value);
            //            inTotal += newPrice * Convert.ToDouble(extRow.Cells["Count"].Value);
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
                //PurchaseDGV.Enabled = true;
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
            if (PurchaseDGV.SelectedCells.Count == 0)
                return;

            //выделяем строки всех выделенных клеток.
            PurchaseDGV.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);

            try
            {
                //узнаем процент заданной наценки.
                float markupValue = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                string markupType = Models.Markup.GetDescription(markupValue);

                //Обновляем таблицу.
                foreach (DataGridViewRow row in PurchaseDGV.SelectedRows)
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
            foreach(DataGridViewRow row in PurchaseDGV.Rows)
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
                        markup           : (row.Cells[MarkupCol.Index].Tag != null) ? Convert.ToSingle(row.Cells[MarkupCol.Index].Tag) : 0
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
            if (PurchaseDGV.Rows.Cast<DataGridViewRow>().All(r => r.Tag == null) || PurchaseDGV.Rows.Cast<DataGridViewRow>().Any(r => r.Tag != null && (r.Cells[PriceCol.Index].Value == null || r.Cells[CountCol.Index].Value == null)))
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
        }//CancelButton_MouseClick

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

                    //Выводим в Excel.
                    saveInExcelAsync(availList.Select(av => av.OperationDetails).ToList(), buyerAgentTextBox.Text.Trim());

                    this.Close();
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