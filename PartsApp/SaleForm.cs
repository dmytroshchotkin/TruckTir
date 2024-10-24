﻿using System;
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
using Models.Helper;
using Infrastructure.Storage.Repositories;
using PartsApp.ExcelHelper;

namespace PartsApp
{
    /*Задания*/
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
        }
        private void SaleForm_Load(object sender, EventArgs e)
        {
            //Устанавливаем даты для DateTimePicker.
            saleDateTimePicker.MaxDate = DateTime.Now.Date.AddDays(7);
            saleDateTimePicker.MinDate = saleDateTimePicker.Value = DateTime.Now;

            //Заполняем список автоподстановки для ввода контрагента.
            var customers = PartsDAL.FindCustomers().Where(a => a.Enabled).Select(c => c.ContragentName).ToArray();
            customerTextBox.AutoCompleteCustomSource.AddRange(customers);

            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);

            sellerAgentTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
        }

        #region Валидация вводимых данных.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void customerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                customerTextBox_Leave(sender, null);
                SaleDGV.Select(); //переводим фокус на таблицу продаж.
            }
        }
        private void customerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(customerTextBox.Text))
            {
                ControlValidation.WrongValueInput(toolTip, customerTextBox);
            }
            else
            {
                //Если такой контрагент в базе отсутствует, выводим сообщение об этом.
                string customer = customerTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == customerTextBox.Text.Trim().ToLower());
                if (customer != null)
                {
                    ControlValidation.CorrectValueInput(toolTip, customerTextBox);
                    customerTextBox.Text = customer; //Выводим корректное имя контрагента.
                }
                else
                {
                    ControlValidation.WrongValueInput(toolTip, customerTextBox, "Такого клиента нет в базе! Он будет добавлен.", Color.Yellow);
                }
            }
        }
        private void sellerTextBox_Leave(object sender, EventArgs e)
        {
            ControlValidation.IsInputControlEmpty(sellerTextBox, toolTip);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы работы с таблицей.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void DGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Нумерация строк.
            DataGridView dgv = sender as DataGridView;
            EnumerableExtensions.RowsNumerateAndAutoSize(dgv.Rows[e.RowIndex]);
        }
        #region Методы работы с осн. таблицей.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void SaleDGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SaleDGV[SellingPriceCol.Index, e.RowIndex].ReadOnly = SaleDGV[CountCol.Index, e.RowIndex].ReadOnly = true;
        }
        private void SaleDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Находим соотв. выбранному товару данные и обновляем доп. таблицу.
            SparePart sparePart = SaleDGV.Rows[e.RowIndex].Tag as SparePart;
            if (sparePart != null)
            {
                FillTheExtDGV(sparePart.AvailabilityList);
            }
            else
            {
                ExtSaleDGV.Rows.Clear();
            }
        }

        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void SaleDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _lastEditCell = SaleDGV[e.ColumnIndex, e.RowIndex]; //запоминаем текущую ячейку как последнюю редактируемую.

            //Обрабатываем ввод в ячейку 'Название' или 'Артикул'.
            if (_lastEditCell.OwningColumn == TitleCol || _lastEditCell.OwningColumn == ArticulCol)
            {
                autoCompleteListBox.Location = GetCellBelowLocation(_lastEditCell); //устанавливаем позицию вып. списка.
            }

            //Обрабатываем ввод в ячейку 'Количествo'.
            if (_lastEditCell.OwningColumn == CountCol)
            {
                SetCustomValueToCell(_lastEditCell, null); //очищаем ячейку для ввода значения пользователем.
            }
        }

        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в ячейку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaleDGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = SaleDGV.CurrentCell;

            if (cell.OwningColumn == TitleCol || cell.OwningColumn == ArticulCol)
            {
                //Если ячейка редактируется первый раз, подписываем её на события обработки ввода.
                if (cell.Tag == null)
                {
                    TextBox textBoxCell = e.Control as TextBox;
                    cell.Tag = textBoxCell; //Запоминаем editing control в Tag ячейки.

                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                    textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                }
            }
        }

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
            }
        }

        private void dataGridViewTextBoxCell_TextChanged(object sender, EventArgs e)
        {
            autoCompleteListBox.DataSource = null;

            TextBox textBox = (TextBox)sender;
            if (!String.IsNullOrWhiteSpace(textBox.Text))
            {
                //Находим подходящий по вводу товар.                
                List<int> existingSparePartsIdsList = SaleDGV.Rows.Cast<DataGridViewRow>().Where(r => r.Tag != null).Select(r => (r.Tag as SparePart).SparePartId).ToList(); //Id-ки уже введенного товара.
                List<SparePart> searchSparePartsList = (_lastEditCell.OwningColumn == TitleCol)
                                    ? PartsDAL.SearchSparePartsByTitle(textBox.Text.Trim(), existingSparePartsIdsList, true, 10)
                                    : PartsDAL.SearchSparePartsByArticul(textBox.Text.Trim(), existingSparePartsIdsList, true, 10);

                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    //Заполняем вып. список новыми объектами.
                    autoCompleteListBox.DataSource = searchSparePartsList;
                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                    autoCompleteListBox.ClearSelected();
                }
            }
        }

        private void SaleDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isCellEditError)
            {
                DataGridViewCell cell = SaleDGV[e.ColumnIndex, e.RowIndex];

                if (cell.OwningColumn == TitleCol || cell.OwningColumn == ArticulCol) //Если редактируется артикул или название товара. 
                {
                    TitleOrArticulCellFilled(cell);
                }
                else if (cell.OwningColumn == CountCol)                            //Если редактируется кол-во. 
                {
                    CountCellFilled(cell);
                }
                else if (cell.OwningColumn == SellingPriceCol)                     //Если редактируется цена продажи. 
                {
                    SellingPriceCellFilled(cell);
                }
            }
        }

        private void SaleDGV_SelectionChanged(object sender, EventArgs e)
        {
            //Если ошибка редактирования ячейки 'Title' или 'Articul', то возвращаем фокус обратно на ячейку (фокус теряется при выборе из вып. списка).
            if (_isCellEditError == true)
            {
                _isCellEditError = false;
                SaleDGV.CurrentCell = _lastEditCell;

                //Включаем режим редактирования ячейки, не инициируя при этом соотв. события.
                SaleDGV.CellBeginEdit -= SaleDGV_CellBeginEdit;
                SaleDGV.EditingControlShowing -= SaleDGV_EditingControlShowing;
                SaleDGV.BeginEdit(true);
                SaleDGV.CellBeginEdit += SaleDGV_CellBeginEdit;
                SaleDGV.EditingControlShowing += SaleDGV_EditingControlShowing;

                //ставим каретку в конец текста. 
                TextBox textBoxCell = _lastEditCell.Tag as TextBox;
                textBoxCell.SelectionStart = textBoxCell.Text.Length;
            }
        }
        private void SaleDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                    {
                        SaleDGV.SelectAll();
                    }
                    else
                    {
                        SaleDGV.Rows[e.RowIndex].Selected = true;
                    }

                    //Выводим контекстное меню.
                    Point location = SaleDGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    saleContextMenuStrip.Show(SaleDGV, location, ToolStripDropDownDirection.BelowRight);
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Выделяем строки всех выделенных ячеек.           
            SaleDGV.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            //Удаляем все выбранные строки и соотв. им объекты.
            foreach (DataGridViewRow row in SaleDGV.SelectedRows)
            {
                //Если строка не пустая, очищаем соотв ей список приходов.
                if (row.Tag != null)
                {
                    _operDetList.RemoveAll(od => od.SparePart.SparePartId == (row.Tag as SparePart).SparePartId); //Очищаем список от соотв. объектов.
                }

                //Если это не последняя строка (предназнач. для ввода нового товара в список), удаляем её.
                if (row.Index != SaleDGV.Rows.Count - 1)
                {
                    SaleDGV.Rows.Remove(row);
                }
            }
            ExtSaleDGV.Rows.Clear(); //Очищаем доп. таблицу.
            FillTheInTotal(); //Заполняем общую сумму операции.
        }

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
                //Если есть такой товар в наличии.
                if (autoCompleteListBox.Items.Count > 0)
                {
                    //если выбор сделан из выпадающего списка.
                    if (autoCompleteListBox.SelectedItem != null)
                    {
                        AutoCompleteRowInfo(cell, autoCompleteListBox.SelectedItem as SparePart); //Заполняем строку данными о товаре.
                    }
                    else  //если выбор не из вып. списка.
                    {
                        CellEndEditWrong(cell, "Выберите товар из списка.");
                    }
                }
                else //если нет такого товара в наличии.
                {
                    CellEndEditWrong(cell, "Нет такого товара в наличии.");
                }
            }
            //Если нет ошибки завершения редактирования ячейки, производим необх. действия.
            if (!_isCellEditError)
            {
                CellEndEditCorrect(cell);
            }
        }

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Количество'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void CountCellFilled(DataGridViewCell cell)
        {
            //Проверяем корректность ввода.
            string measureUnit = cell.OwningRow.Cells[MeasureUnitCol.Index].Value.ToString();
            if (IsCountCellValueCorrect(cell, measureUnit))
            {
                AutoChoisePurchases(cell);         //Автовыбор приходов с которых осущ. продажа.
            }
            else
            {
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000); //выводим всплывающее окно с сообщением об ошибке.
                SetDefaultValueToCell(cell); //Возвращаем серый цвет и дефолтное значение данной ячейке.

                //Возвращаем дефолтные значения во всех строках доп. таблицы.
                SetDefaultValuesToExtSaleDGV((cell.OwningRow.Tag as SparePart).SparePartId);
            }
            FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
        }

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Цена продажи'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void SellingPriceCellFilled(DataGridViewCell cell)
        {
            try
            {
                float sellPrice = Convert.ToSingle(cell.Value);
                if (sellPrice == 0)
                {
                    throw new Exception();  //ввод нуля также является ошибкой.                
                }

                int sparePartId = (cell.OwningRow.Tag as SparePart).SparePartId;
                SparePart sparePart = SaleDGV.Rows.Cast<DataGridViewRow>().First(r => r.Tag != null && (r.Tag as SparePart).SparePartId == sparePartId).Tag as SparePart;

                //Если юзер не обладает правами админа, то запрещаем ему выставлять цену продажи ниже чем с наценкой "Крупный опт".
                if (Form1.CurEmployee.AccessLayer == Employee.AccessLayers.User.ToDescription())
                {
                    //Если установленная юзером цена продажи ниже чем цена продажи данного товара с наценкой "Крупный опт" хотя бы по одному приходу.
                    if (sparePart.AvailabilityList.Any(av => (av.OperationDetails.Price + (av.OperationDetails.Price * (float)Markup.Types.LargeWholesale / 100) > sellPrice)))
                    {
                        throw new Exception();
                    }
                }
                //Если цена продажи хотя бы где-то ниже закупочной требуем подтверждения действий.                         
                if (sparePart.AvailabilityList.Any(av => av.OperationDetails.Price >= sellPrice))
                {
                    if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception();
                    }
                }

                cell.Value = sellPrice; //Перезаписываем установленную цену, для её форматированного вывода в ячейке.
            }
            catch
            {
                //выводим всплывающее окно с сообщением об ошибке и очищаем ввод.
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                cell.Value = null;
            }
            FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'. 
        }

        /// <summary>
        /// Автозаполнение строки соотв. инф-цией.
        /// </summary>
        /// <param name="countCell">Заполняемая ячейка.</param>
        /// <param name="titleAndArticul">Массив строк с артикулом и названием.</param>
        private void AutoCompleteRowInfo(DataGridViewCell cell, SparePart sparePart)
        {
            FillTheBothDGV(cell.OwningRow, sparePart);

            cell.OwningRow.Cells[SellingPriceCol.Index].ReadOnly = cell.OwningRow.Cells[CountCol.Index].ReadOnly = false;
            cell.OwningRow.Cells[TitleCol.Index].ReadOnly = cell.OwningRow.Cells[ArticulCol.Index].ReadOnly = true;

            autoCompleteListBox.Visible = false;

            #region Увеличение saleGroupBox.
            //if (saleDGV.PreferredSize.Height > saleDGV.Size.Height)
            //{
            //    MessageBox.Show("bigger");
            //    int height = saleDGV.Rows[0].Cells["Title"].Size.Height;
            //    saleGroupBox.Size = new Size(saleGroupBox.Width, saleGroupBox.Height + height);
            //}
            #endregion
        }

        /// <summary>
        /// Действия при некорректном завершении редактирования ячейки.
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="toolTipText">Текст всплывающей подсказки</param>
        private void CellEndEditWrong(DataGridViewCell cell, string toolTipText)
        {
            toolTip.Show(toolTipText, this, GetCellBelowLocation(cell), 1000);
            _isCellEditError = true;
        }

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
        }

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
            {
                return false;
            }

            //Ввод значения не более 0, или больше чем есть в наличии является ошибкой. 
            float totalCount = Convert.ToSingle(countCell.Tag);
            if (count <= 0 || count > totalCount)
            {
                return false;
            }

            //Проверяем является ли введенное число корректным для продажи, т.е. кратно ли оно минимальной единице продажи.     
            if (count % Models.MeasureUnit.GetMinUnitSale(measureUnit) != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Заполняет обе таблицы необх. данными
        /// </summary>
        /// <param name="extRow">Заполняемая строка в осн. таблице.</param>
        /// <param name="sparePart">Данные для заполнения.</param>
        private void FillTheBothDGV(DataGridViewRow row, SparePart sparePart)
        {
            //Заполняем осн. таблицу.
            FillTheSaleDGV(row, sparePart);
            //Очищаем доп. таблицу и заполняем её новой инф-цией.
            ExtSaleDGV.Rows.Clear();
            FillTheExtDGV(sparePart.AvailabilityList);
        }

        /// <summary>
        /// Заполняет осн. таблицу данными.
        /// </summary>
        /// <param name="extRow">Заполняемая строка.</param>
        /// <param name="sparePart">Данные для заполнения строки.</param>
        private void FillTheSaleDGV(DataGridViewRow row, SparePart sparePart)
        {
            row.Tag = sparePart;

            row.Cells[TitleCol.Index].Value = sparePart.Title;
            row.Cells[ArticulCol.Index].Value = sparePart.Articul;
            row.Cells[MeasureUnitCol.Index].Value = sparePart.MeasureUnit;
            row.Cells[StorageCellCol.Index].Value = sparePart.StorageCell;

            row.Cells[CountCol.Index].Tag = Availability.GetTotalCount(sparePart.AvailabilityList); //Заполняем кол-во и запоминаем в Tag.
            SetDefaultValueToCell(row.Cells[CountCol.Index]); //Задаем серый цвет и дефолтное значение данной ячейке.

            //Если отпускная цена не указана поль-лем и если у всех приходов она одинаковая, выводим её в saleDGV.
            if (row.Cells[SellingPriceCol.Index].Value == null)
            {
                if (!sparePart.AvailabilityList.Any(av => av.SellingPrice != sparePart.AvailabilityList[0].SellingPrice))
                {
                    row.Cells[SellingPriceCol.Index].Value = Math.Ceiling(sparePart.AvailabilityList[0].SellingPrice / 0.5) * 0.5; //Округляем в большую сторону с точностью до 0,5. //ERROR округление станет лишним, после того как полностью обновится список товара в наличии. //sparePart.AvailabilityList[0].SellingPrice
                }
            }
        }

        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из saleDGV. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = SaleDGV.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = SaleDGV.Location;
            Point gbLoc = saleGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }

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
            foreach (DataGridViewRow row in ExtSaleDGV.Rows)
            {
                float extAvailCount = Convert.ToSingle(row.Cells[ExtCountCol.Index].Tag); //количество в наличии в данном приходе.                

                if (sellCount > 0)
                {
                    int purchaseId = Convert.ToInt32(row.Cells[ExtPurchaseIdCol.Index].Value);
                    float curSellValue = (sellCount > extAvailCount) ? extAvailCount : sellCount;

                    DataGridViewCell extCountCell = row.Cells[ExtCountCol.Index];
                    SetCustomValueToCell(extCountCell, curSellValue); //задаём значение для ячейки.
                    FillTheOperDetList(sparePartId, extCountCell);
                    sellCount -= extAvailCount;
                }
                else
                {
                    SetDefaultValueToCell(row.Cells[ExtCountCol.Index]); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                }
            }
        }

        /// <summary>
        /// Меняет количество или добавляет новое значение в список деталей операции. 
        /// </summary>
        /// <param name="sparePartId">Id товара.</param>
        /// <param name="extCountCell">Ячейка стобца 'Кол-во' доп. таблицы.</param>
        private void FillTheOperDetList(int sparePartId, DataGridViewCell extCountCell)
        {
            int purchaseId = Convert.ToInt32(extCountCell.OwningRow.Cells[ExtPurchaseIdCol.Index].Value);
            float sellCount = (extCountCell.Style.ForeColor == Color.Black) ? Convert.ToSingle(extCountCell.Value) : 0;
            //Находим, если есть соотв. объект в списке.
            OperationDetails operDet = _operDetList.FirstOrDefault(od => od.SparePart.SparePartId == sparePartId
                                                                      && od.Operation.OperationId == purchaseId);

            //Если объект есть, меняем у него св-во Count, иначе создаём новый объект.
            if (operDet == null)
            {
                if (sellCount > 0)
                {
                    SparePart sparePart = SaleDGV.Rows.Cast<DataGridViewRow>().First(r => r.Tag != null && (r.Tag as SparePart).SparePartId == sparePartId).Tag as SparePart;
                    IOperation purch = sparePart.AvailabilityList.First(av => av.OperationDetails.Operation.OperationId == purchaseId).OperationDetails.Operation;

                    _operDetList.Add(new OperationDetails(sparePart, purch, sellCount, 0));
                }
            }
            else
            {
                //Если такой объект есть в списке, и прод. кол-во > 0, то обновляем кол-во, иначе удаляем из списка.
                if (sellCount > 0)
                {
                    operDet.Count = sellCount;
                }
                else
                {
                    _operDetList.Remove(operDet);
                }
            }
        }

        /// <summary>
        /// Заполняет ячейку 'Сумма' заданной строки и общую сумму.
        /// </summary>
        /// <param name="extRow">Строка дял которой производятся вычисления и заполнение.</param>
        private void FillTheSumCell(DataGridViewRow row)
        {
            if (row.Cells[CountCol.Index].Style.ForeColor == Color.Black && row.Cells[SellingPriceCol.Index].Value != null)
            {
                float sellPrice = Convert.ToSingle(row.Cells[SellingPriceCol.Index].Value);
                float sellCount = Convert.ToSingle(row.Cells[CountCol.Index].Value);

                row.Cells[SumCol.Index].Value = sellPrice * sellCount;
            }
            else
            {
                row.Cells[SumCol.Index].Value = null;//очищаем ячейку. 
            }
            FillTheInTotal(); //Заполняем общую сумму операции.
        }

        /// <summary>
        /// Заполняет InTotalLabel корретным значением.
        /// </summary>
        private void FillTheInTotal()
        {
            float inTotal = 0;
            foreach (DataGridViewRow row in SaleDGV.Rows)
            {
                //Если в строке заполнена ячейка 'Сумма'.
                if (row.Cells[SumCol.Index].Value != null)
                {
                    inTotal += Convert.ToSingle(row.Cells[SumCol.Index].Value);
                }
            }
            //Заполняем InTotalLabel расчитанным значением.
            inTotalNumberLabel.Text = (Math.Round(inTotal, 2, MidpointRounding.AwayFromZero)).ToString("0.00");

            CurrencyLabel.Left = inTotalNumberLabel.Right - 4; //Перемещаем Label указывающий валюту.

            //меняем значение фактической оплаченной суммы если, галочка о полной оплате стоит.
            if (PaidCheckBox.Checked == true)
            {
                PaidNumericUpDown.Value = (decimal)inTotal;
            }
        }

        /// <summary>
        /// Возвращает дефолтные значения во все ячейки столбца 'Кол-во' доп. таблицы.
        /// </summary>
        /// <param name="sparePartId">Ид товара.</param>Vf
        private void SetDefaultValuesToExtSaleDGV(int sparePartId)
        {
            foreach (DataGridViewRow extRow in ExtSaleDGV.Rows)
            {
                SetDefaultValueToCell(extRow.Cells[ExtCountCol.Index]);           //Записываем дефолтное значение в ячейку.
                FillTheOperDetList(sparePartId, extRow.Cells[ExtCountCol.Index]); //Запоминаем изменение в список.    
            }
        }

        /// <summary>
        /// Записывает дефолтное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetDefaultValueToCell(DataGridViewCell cell)
        {
            cell.Style.ForeColor = Color.Gray;
            cell.Value = cell.Tag;
        }

        /// <summary>
        /// Записывает кастомное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetCustomValueToCell(DataGridViewCell cell, object value)
        {
            cell.Style.ForeColor = Color.Black;
            cell.Value = value;
        }


        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Методы работы с выпадающим списком.
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Обработчик для того, чтобы не срабатывало событие CellEndEdit при клике мышкой по вып. спику.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
        {
            _isCellEditError = true;
        }

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                //Возвращаем фокус на ячейку для кот. выводится вып. список.                
                SaleDGV_SelectionChanged(null, null);
                _isCellEditError = true;
            }
            else
            {
                //Делаем автозаполнение строки, выбранным объектом.   
                _isCellEditError = false;
                SaleDGV_CellEndEdit(null, new DataGridViewCellEventArgs(_lastEditCell.ColumnIndex, _lastEditCell.RowIndex));
            }
        }

        private void autoCompleteListBox_DataSourceChanged(object sender, EventArgs e)
        {
            AutoCompleteListBox.DataSourceChanged(autoCompleteListBox);
        }
        /// <summary>
        /// Форматирование вывода в ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            AutoCompleteListBox.OutputFormatting(autoCompleteListBox, e);
        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Методы работы с доп. таблицей.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void ExtSaleDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == ExtCountCol.Index)
            {
                DataGridViewCell cell = ExtSaleDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                SetCustomValueToCell(cell, null); //Очищаем ячейку для ввода знвчения поль-лем.
            }
        }

        private void ExtSaleDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Если редактировалась колонка "Кол-во". хотя по идее все остальные readOnly.
            if (ExtSaleDGV.Columns[e.ColumnIndex] == ExtCountCol)
            {
                DataGridViewRow row = SaleDGV.CurrentRow;
                DataGridViewCell extCountCell = ExtSaleDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewCell countCell = row.Cells[CountCol.Index];
                //Проверяем корректность ввода.
                int sparePartId = (row.Tag as SparePart).SparePartId;
                string measureUnit = extCountCell.OwningRow.Cells[ExtMeasureUnitCol.Index].Value.ToString();
                if (IsCountCellValueCorrect(extCountCell, measureUnit))
                {
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.                                      
                }
                else
                {
                    toolTip.Show("Введены некорректные данные", this, GetExtCellBelowLocation(extCountCell), 1000);  //выводим всплывающее окно с сообщением об ошибке.
                    SetDefaultValueToCell(extCountCell); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.                    
                }
                FillTheSumCell(row);                           //Заполняем столбец 'Сумма'.
                FillTheOperDetList(sparePartId, extCountCell); //Запоминаем изменение в список.    
            }
        }

        private void ExtSaleDGV_SelectionChanged(object sender, EventArgs e)
        {
            markupComboBox.Enabled = (ExtSaleDGV.SelectedCells.Count > 0); //Если есть выделенные клетки делаем доступной изменение наценки.
        }

        private void extGroupBox_Click(object sender, EventArgs e)
        {
            ExtSaleDGV.ClearSelection();
        }

        /// <summary>
        /// Заполняет данными таблицу доп. инф-ции.
        /// </summary>
        /// <param name="availList">Список приходов данного товара в наличии.</param>
        private void FillTheExtDGV(List<Availability> availList)
        {
            //Очищаем предварительно таблицу.
            ExtSaleDGV.Rows.Clear();
            ExtStorageAdressCol.Visible = ExtNoteCol.Visible = false;
            //Заполняем таблицу новыми данными.
            foreach (Availability avail in availList)
            {
                int rowIndx = ExtSaleDGV.Rows.Add();
                DataGridViewRow row = ExtSaleDGV.Rows[rowIndx];

                row.Cells[ExtSupplierCol.Index].Value = avail.OperationDetails.Operation.Contragent.ContragentName;
                row.Cells[ExtMeasureUnitCol.Index].Value = avail.OperationDetails.SparePart.MeasureUnit;
                row.Cells[ExtPriceCol.Index].Value = avail.OperationDetails.Price;
                row.Cells[ExtMarkupCol.Index].Value = Models.Markup.GetDescription(avail.Markup);
                row.Cells[ExtSellingPriceCol.Index].Value = avail.SellingPrice;
                row.Cells[ExtPurchaseIdCol.Index].Value = avail.OperationDetails.Operation.OperationId;
                row.Cells[ExtPurchaseDateCol.Index].Value = avail.OperationDetails.Operation.OperationDate;
                row.Cells[ExtNoteCol.Index].Value = avail.OperationDetails.Operation.Description;

                //Делаем видимыми соотв. столбец, если в св-ве 'Примечание по поставке' есть данные.   
                if (avail.OperationDetails.Operation.Description != null)
                {
                    ExtNoteCol.Visible = true;
                }
                //Заполняем ячейку 'Кол-во' либо ранее установленным значением, иначе общим кол-вом по данному приходу в наличии. 
                OperationDetails operDet = _operDetList.FirstOrDefault(od => od.SparePart.SparePartId == avail.OperationDetails.SparePart.SparePartId
                                                                    && od.Operation.OperationId == avail.OperationDetails.Operation.OperationId);

                DataGridViewCell extCountCell = row.Cells[ExtCountCol.Index];
                extCountCell.Tag = avail.OperationDetails.Count; //заполняем ячейку значением и запоминаем это дефолтное значение в Tag.

                if (operDet == null)
                {
                    SetDefaultValueToCell(extCountCell); //Задаем серый цвет и дефолтное значение данной ячейке.
                }
                else
                {
                    SetCustomValueToCell(extCountCell, operDet.Count); //Задаем значение ячейки.
                }
            }
            //Сортируем таблицу по дате прихода.
            ExtSaleDGV.Sort(ExtPurchaseDateCol, ListSortDirection.Ascending);
            ExtSaleDGV.ClearSelection();
        }

        /// <summary>
        /// Обновляет значение ячейки 'Кол-во' в таблице продаж, после изменений в доп. таблице.
        /// </summary>
        /// <param name="countCell">Соотв. ячейка 'Кол-во' в осн. таблице.</param>
        private void SaleDGVCountColumnUpdate(DataGridViewCell countCell)
        {
            //Находим общее кол-во данного продаваемого товара.
            float extSellCount = 0;
            foreach (DataGridViewRow extRow in ExtSaleDGV.Rows)
            {
                if (extRow.Cells[ExtCountCol.Index].Style.ForeColor == Color.Black)
                {
                    extSellCount += Convert.ToSingle(extRow.Cells[ExtCountCol.Index].Value);
                }
            }
            //Если есть кастомный ввод.
            if (extSellCount > 0)
            {
                SetCustomValueToCell(countCell, extSellCount); //Обновляем "кол-во" в таблице продаж.
            }
            else
            {
                SetDefaultValueToCell(countCell); //Задаём дефолтное значения для ячейки.   
            }
        }

        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из extSaleDGV. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetExtCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = ExtSaleDGV.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = ExtSaleDGV.Location;
            Point gbLoc = extGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }


        #region Методы связанные с изменением наценки.
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                markupComboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (ExtSaleDGV.SelectedCells.Count == 0)
            {
                return;
            }

            //выделяем строки всех выделенных клеток.
            ExtSaleDGV.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);

            try
            {
                //узнаем процент заданной наценки.
                float markupValue = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                string markupType = Models.Markup.GetDescription(markupValue);

                //Обновляем таблицу.
                foreach (DataGridViewRow row in ExtSaleDGV.SelectedRows)
                {
                    row.Cells[ExtMarkupCol.Index].Value = markupType;

                    float price = (float)row.Cells[ExtPriceCol.Index].Value;
                    float sellPrice = (float)Math.Round(price + (price * markupValue / 100), 2, MidpointRounding.AwayFromZero);
                    row.Cells[ExtSellingPriceCol.Index].Value = sellPrice;
                }
            }
            catch
            {
                toolTip.Show("Введено некорректное значение.", this, markupComboBox.Location, 2000);
            }
        }


        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion


        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion        

        private void PaidCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PaidNumericUpDown.Enabled = !PaidCheckBox.Checked;

            //Если галочка стоит, то выводим оплаченную сумму как сумму по всей накладной, иначе выводим 0.
            PaidNumericUpDown.Value = (PaidCheckBox.Checked ? Convert.ToDecimal(inTotalNumberLabel.Text) : 0);
        }

        /// <summary>
        /// Возвращает объект типа Sale, созданный из данных формы.
        /// </summary>
        /// <returns></returns>
        private Sale CreateSaleFromForm()
        {
            //Находим контрагента. Если такого ещё нет в базе, то создаем новый объект.
            IContragent customer = PartsDAL.FindCustomers(customerTextBox.Text.Trim());
            customer = (customer == null) ? new Customer(0, customerTextBox.Text.Trim(), null, null, null, null, 0) : customer;

            //Если внесена сумма отличающаяся от требуемой (галочка выключена), меняем баланс клиента.
            if (PaidCheckBox.Checked == false)
            {
                customer.Balance += (double)PaidNumericUpDown.Value - Convert.ToDouble(inTotalNumberLabel.Text);
            }


            Sale sale = new Sale
            (
                employee: Form1.CurEmployee,
                contragent: customer,
                contragentEmployee: (!String.IsNullOrWhiteSpace(customerAgentTextBox.Text)) ? customerAgentTextBox.Text.Trim() : null,
                operationDate: saleDateTimePicker.Value,
                description: (!String.IsNullOrWhiteSpace(descriptionRichTextBox.Text)) ? descriptionRichTextBox.Text.Trim() : null,
                operDetList: CreateOperationDetailsListFromForm(),
                paidCash: IsSalePaidCash()
            );
            //Присваиваем 'Операцию' для каждого OperationDetails.
            sale.OperationDetailsList.ToList().ForEach(od => od.Operation = sale);

            return sale;
        }

        private bool IsSalePaidCash()
        {
            return !PaidNonCashCheckBox.Checked;
        }

        /// <summary>
        /// Возвращает список объектов типа OperationDetails, созданный из данных таблицы продаж.
        /// </summary>
        /// <returns></returns>
        private List<OperationDetails> CreateOperationDetailsListFromForm()
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();
            foreach (DataGridViewRow row in SaleDGV.Rows)
            {
                //Если строка не пустая.
                if (row.Tag != null)
                {
                    float count = Convert.ToSingle(row.Cells[CountCol.Index].Value);
                    float sellPrice = Convert.ToSingle(row.Cells[SellingPriceCol.Index].Value);

                    SparePart sparePart = row.Tag as SparePart;
                    operDetList.Add(new OperationDetails(sparePart, null, count, sellPrice));
                }
            }
            return operDetList;
        }

        /// <summary>
        /// Возвращает true если все обязательные поля корректно заполнены, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfRequiredFieldsValid()
        {
            //Находим все BackPanel-контролы на форме. 
            List<Control> curAccBackControls = this.GetAllControls(typeof(Panel), "BackPanel");

            customerTextBox_Leave(null, null);
            sellerTextBox_Leave(null, null);

            //Если хоть один контрол не прошел валидацию, возв-ем false.
            if (curAccBackControls.Any(backPanel => backPanel.BackColor == Color.Red))
            {
                return false;
            }

            //Если таблица не заполнена или не везде указана цена или кол-во.
            if (_operDetList.Count == 0 || SaleDGV.Rows.Cast<DataGridViewRow>().Any(r => r.Tag != null && (r.Cells[SellingPriceCol.Index].Value == null || r.Cells[CountCol.Index].Style.ForeColor == Color.Gray)))
            {
                toolTip.Show("Таблица не заполнена или не везде указана цена или количество товара", this, okButton.Location, 3000);
                return false;
            }
            return true;
        }

        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (!CheckIfRequiredFieldsValid() || CheckIfSPsAvailabilityChanged())
            {
                return;
            }

            Sale sale = CreateSaleFromForm();
            try
            {
                sale.OperationId = PartsDAL.AddSale(sale, _operDetList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Операция завершена неправильно! Попробуйте ещё раз.\n\nОшибка:\n{ex.InnerException?.Message ?? ex.Message}");
                return;
            }
            OperationsExcelHelper.SaveInExcelAsync(sale.OperationDetailsList, sellerTextBox.Text.Trim());
            Close();
        }

        private bool CheckIfSPsAvailabilityChanged()
        {
            var spsWithChangedAvailabilities = GetSPsWithChangedAvailability();
            if (!spsWithChangedAvailabilities.Any())
            {
                return false;
            }

            DeleteRowsForSPs(spsWithChangedAvailabilities);
            DisplayInvalidAvailabilityMessageBox(spsWithChangedAvailabilities);
            return true;
        }

        private void DeleteRowsForSPs(IEnumerable<SparePart> spareParts)
        {
            var rowsToDelete = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in SaleDGV.Rows)
            {
                if (row.Tag is SparePart sp && spareParts.Contains(sp))
                {
                    rowsToDelete.Add(row);
                }
            }

            RemoveRowsFromSaleDGV(rowsToDelete);
        }

        private List<SparePart> GetSPsWithChangedAvailability()
        {
            var result = new List<SparePart>();
            foreach (DataGridViewRow row in SaleDGV.Rows)
            {
                if (row.Tag is SparePart sp)
                {
                    var updatedSp = SparePartRepository.FindSparePart(sp.SparePartId);
                    if (!DoSPsHaveEqualAvailability(sp, updatedSp))
                    {
                        result.Add(sp);
                    }
                }
            }

            return result;
        }

        private bool DoSPsHaveEqualAvailability(SparePart sp1, SparePart sp2)
        {
            foreach (var a in sp1.AvailabilityList)
            {
                if (!sp2.AvailabilityList.Exists(av => av.OperationDetails.Operation.OperationId == a.OperationDetails.Operation.OperationId
                && a.OperationDetails.Count == av.OperationDetails.Count))
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveRowsFromSaleDGV(IEnumerable<DataGridViewRow> rows)
        {
            foreach (var row in rows)
            {
                if (row.Tag is SparePart sp)
                {
                    _operDetList.RemoveAll(od => od.SparePart.SparePartId == sp.SparePartId);
                    SaleDGV.Rows.Remove(row);
                    ExtSaleDGV.Rows.Clear();
                    FillTheInTotal();
                }
            }
        }

        private void DisplayInvalidAvailabilityMessageBox(IEnumerable<SparePart> spareParts)
        {
            var spArticlesAndTitles = new StringBuilder();
            foreach (var sp in spareParts)
            {
                spArticlesAndTitles.Append($"\n{sp.Articul}\n{sp.Title}\n");
            }

            MessageBox.Show($"Изменилось количество товара:\n{spArticlesAndTitles}\nПовторите выбор товара!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}