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

namespace PartsApp
{
    /*Задания*/
    //Заменить имена полей, чтобы начинались с подчеркивания.
    //Убрать заполнения ячейки Арткикул или Название при пролистывании вып. списка.
    //Убрать столбец extPrice из доп. таблицы.
    //Задать форматы столбцов через дизайнер.
    //Удалить лишние столбцы из таблиц.
    public partial class SaleForm2 : Form
    {
        List<SparePart> sparePartsList = new List<SparePart>();

        List<OperationDetails> _operDetList = new List<OperationDetails>();

        /// <summary>
        /// Список найденных в базе товаров по последнему запросу.
        /// </summary>
        List<SparePart> searchSparePartsList; /*ERROR можно ли убрать?*/

        /// <summary>
        /// Для запоминания редактируемой в данный момент ячейки.
        /// </summary>
        TextBox textBoxCell;

        DataGridViewCell lastEditCell;

        /// <summary>
        /// Переменная для запоминания изначального ввода.
        /// </summary>
        string userText;

        bool isCellEditError     = false;
        bool textChangedEvent    = false;
        bool previewKeyDownEvent = false;


        public SaleForm2()
        {
            InitializeComponent();
        }

        private void SaleForm2_Load(object sender, EventArgs e)
        {

        }//


        #region Методы работы с таблицей.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        #region Методы работы с осн. таблицей.
//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void saleDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Эта строка нужна потому что новые столбцы SellingPrice и Count почему то становятся открытыми для записи.
            /*!!!*/
            //saleDataGridView.Rows[e.RowIndex].Cells[SellingPrice.Name].ReadOnly = saleDataGridView.Rows[e.RowIndex].Cells["Count"].ReadOnly = true;            
        }//saleDataGridView_RowsAdded

        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void saleDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

            lastEditCell = cell;
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                ////Запрещаем ввод в новую строку, если в предыдущей не были введены кол-во и цена продажи.
                //if (countCell.OwningRow.Index != 0)
                //    if (saleDataGridView.Rows[countCell.OwningRow.Index - 1].Cells[SellingPrice.Index].Value == null ||
                //        saleDataGridView.Rows[countCell.OwningRow.Index - 1].Cells[Count.Index].Style.ForeColor == Color.Gray)
                //        e.Cancel = true;

                autoCompleteListBox.Location = GetCellBelowLocation(cell);

                extDataGridView.Columns[extCount.Name].ReadOnly = false; //Разрешаем ввод кол-ва в доп. таблице.
            }//if

            ////Запрещаем ввод цены, пока не введено кол-во.
            //if (countCell.OwningColumn == SellingPrice && countCell.OwningRow.Cells[Count.Index].Style.ForeColor == Color.Gray)
            //    e.Cancel = true;

            //Обрабатываем ввод Количества.
            if (cell.OwningColumn == Count)
            {
                SetCustomValueToCell(cell, null); //очищаем ячейку для ввода значения пользователем.
            }//if
        }//saleDataGridView_CellBeginEdit

        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в клетку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saleDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)saleDataGridView.CurrentCell;

            //
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                textBoxCell = e.Control as TextBox;
                if (previewKeyDownEvent == false)
                {
                    previewKeyDownEvent = true;
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
            TextBox textBox = sender as TextBox;

            if (e.KeyCode == Keys.Down)
            {
                isCellEditError = true;
                if (searchSparePartsList.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Items.Count == 0) return; //может не надо это действие.
                if (autoCompleteListBox.Visible == false) return;
                //Если выбран последний эл-нт списка, вернуть начальное значение и убрать выделение в listBox-е. 
                if (autoCompleteListBox.SelectedIndex == autoCompleteListBox.Items.Count - 1)
                {
                    textBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                    return;
                }//if
                //Если выбирается первый эл-нт выпадающего списка, запоминаем введенную ранее пользователем строку.
                if (autoCompleteListBox.SelectedIndex == -1)
                    userText = textBox.Text;

                autoCompleteListBox.SelectedIndex += 1;
                return;
            }//if

            if (e.KeyCode == Keys.Up)
            {
                isCellEditError = true;
                if (searchSparePartsList.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Items.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Visible == false) return;
                //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
                if (autoCompleteListBox.SelectedIndex == -1)
                {
                    userText = textBox.Text;
                    autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1;
                }//if
                //Если выбран верхний эл-нт вып. списка, вернуть введенную ранее пользователем строку.
                else if (autoCompleteListBox.SelectedIndex == 0)
                {
                    textBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                }//if
                else autoCompleteListBox.SelectedIndex -= 1;
                //Если это нулевая строка, то при нажатии Up не происходит событие SelectionChanged, и при выборе из вып. списка каретка ставитс в начало строки, что затрудняет дальнейший ввод поль-лю. Мы вызываем событие искусствунно и ставим каретку в конец строки.                               
                if (lastEditCell.OwningRow.Index == 0)
                    saleDataGridView_SelectionChanged(sender, null);

                return;
            }//if    

            //Продолжается ввод.
            if (textChangedEvent == false)
                textChangedEvent = true;
        }//dataGridViewTextBoxCell_PreviewKeyDown

        private void dataGridViewTextBoxCell_TextChanged(object sender, EventArgs e)
        {
            if (textChangedEvent == false) 
                return;

            /* Эта проверка нужна потому что в редких случаях по непонятным причинам TextChanged срабатывает на столбцы Count или др. на которых работать не должен
                Это случается когда вводишь что-то в столбец Title, а потом стираешь до пустой строки и вводишь что-то в столбец Count.*/
            /*!!!*/
            if (lastEditCell.OwningColumn != Title && lastEditCell.OwningColumn != Articul)
                return;

            TextBox textBox = (TextBox)sender;
            if (!String.IsNullOrWhiteSpace(textBox.Text))
            {
                //Находим подходящий по вводу товар.
                List<int> sparePartsIdList = sparePartsList.Select(sp => sp.SparePartId).ToList(); //Id уже введенного товара.
                searchSparePartsList = (lastEditCell.OwningColumn == Title) ? PartsDAL.SearchSparePartsAvaliablityByTitle(textBox.Text.Trim(), 10, sparePartsIdList)
                                                                            : PartsDAL.SearchSparePartsAvaliablityByArticul(textBox.Text.Trim(), 10, sparePartsIdList);

                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    autoCompleteListBox.Items.Clear();
                    //Заполняем вып. список новыми объектами.
                    searchSparePartsList.ForEach(sp => autoCompleteListBox.Items.Add(sp));                                                                     


                    autoCompleteListBox.DisplayMember = (lastEditCell.OwningColumn == Title) ? "Title" : "Articul";
                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                    autoCompleteListBox.Visible = true;
                }//if
                else 
                    autoCompleteListBox.Visible = false; //Если ничего не найдено, убрать вып. список.
            }//if
            else 
                autoCompleteListBox.Visible = false; //Если ничего не введено, убрать вып. список.
        }//dataGridViewTextBoxCell_TextChanged

        private void saleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isCellEditError)
                return;

            autoCompleteListBox.Visible = false;          
            DataGridViewCell cell = saleDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.OwningColumn == Title || cell.OwningColumn == Articul) //Если редактируется артикул или название товара. 
                TitleOrArticulCellFilled(cell);
            else if (cell.OwningColumn == Count)                            //Если редактируется кол-во. 
                CountCellFilled(cell);
            else if (cell.OwningColumn == SellingPrice)                     //Если редактируется цена продажи. 
                SellingPriceCellFilled(cell);                 
        }//saleDataGridView_CellEndEdit 

        private void saleDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isCellEditError == true)
            {
                isCellEditError = false;
                saleDataGridView.CurrentCell = lastEditCell;

                saleDataGridView.CellBeginEdit -= saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing -= saleDataGridView_EditingControlShowing;
                saleDataGridView.BeginEdit(true);
                saleDataGridView.CellBeginEdit += saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing += saleDataGridView_EditingControlShowing;

                textBoxCell.SelectionStart = textBoxCell.Text.Length;

                //Находим соотв. товар и обновляем доп. таблицу.
                int sparePartId = Convert.ToInt32(saleDataGridView.CurrentCell.OwningRow.Cells[SparePartId.Index].Value);
                FillTheExtDGV()
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
                    {
                        //Если строка пустая не делаем ничего.
                        if (saleDataGridView.Rows[e.RowIndex].Cells[SparePartId.Index].Value == null) 
                            return;

                        saleDataGridView.Rows[e.RowIndex].Cells[SparePartId.Index].Selected = true;
                    }//else
                    Point location = saleDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    saleContextMenuStrip.Show(saleDataGridView, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if 
        }//saleDataGridView_CellMouseClick     

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Удаляем все выбранные строки и соотв. им объекты.
            foreach (DataGridViewRow row in saleDataGridView.SelectedRows)
            {
                int sparePartId = Convert.ToInt32(row.Cells[SparePartId.Index].Value);

                SparePart removedSparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);
                sparePartsList.Remove(removedSparePart);

                _operDetList.RemoveAll(od => od.SparePart.SparePartId == sparePartId);
                
                saleDataGridView.Rows.Remove(lastEditCell.OwningRow); //Удаляем строку из таблицы.

                //Очищаем доп. таблицу.
                extDataGridView.Rows.Clear();
            }//foreach
            
            FillTheInTotal(); //Заполняем общую сумму операции.
        }//removeToolStripMenuItem_Click

        
        #region Вспомогательные методы.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Артикул' и 'Название'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void TitleOrArticulCellFilled(DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;

            //убираем события с заполненной клетки.
            if (textBoxCell != null)
            {
                textChangedEvent = previewKeyDownEvent = false;
                textBoxCell.TextChanged -= dataGridViewTextBoxCell_TextChanged;
                textBoxCell.PreviewKeyDown -= dataGridViewTextBoxCell_PreviewKeyDown;
            }//if

            //Если есть такой товар в базе.
            if (searchSparePartsList.Count > 0)
            {
                //если выбор сделан из выпадающего списка.
                if (autoCompleteListBox.SelectedItem != null)
                {
                    SparePart sparePart = autoCompleteListBox.SelectedItem as SparePart;                    
                    AutoCompleteRowInfo(cell, sparePart); //Заполняем строку данными о товаре.
                }//if
                else  //если выбор не из вып. списка.
                {
                    if (searchSparePartsList.Count == 1) //если этот товар уникален.
                    {
                        //находим из списка нужную запчасть. /*ERROR Кажется может быть ошибка идентификации введенного товара*/
                        SparePart sparePart = searchSparePartsList[0];
                        AutoCompleteRowInfo(cell, sparePart);
                    }//if 
                    else //если в вып. списке > 1 товара.
                    {
                        toolTip.Show("Выберите товар из списка.", this, GetCellBelowLocation(cell), 1000);
                        isCellEditError = true; 
                        autoCompleteListBox.Visible = true;
                        if (previewKeyDownEvent == false)
                        {
                            previewKeyDownEvent = true;
                            textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                            textBoxCell.TextChanged    += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                        }//if                                    
                    }//else  
                }//else
            }//if
            else
            {
                toolTip.Show("Нет такого товара в наличии.", this, GetCellBelowLocation(lastEditCell), 1000);
                lastEditCell.Value = null;
                isCellEditError = true;
            }//else
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
                FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
                AutoChoisePurchases(cell);         //Автовыбор приходов с которых осущ. продажа.
            }//if            
            else
            {
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000); //выводим всплывающее окно с сообщением об ошибке.
                SetDefaultValueToCell(cell); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                lastEditCell = cell; /*ERROR!!! надо ли?*/
                //Возвращаем дефолтные значения во всех строках доп. таблицы.
                foreach (DataGridViewRow extRow in extDataGridView.Rows)
                {
                    SetDefaultValueToCell(extRow.Cells[extCount.Index]);
                    int sparePartId = Convert.ToInt32(cell.OwningRow.Cells[SparePartId.Index].Value);
                    FillTheOperDetList(sparePartId, extRow.Cells[extCount.Index]); //Запоминаем изменение в список.    
                }//if
            }//catch  
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
                    float price = Convert.ToSingle(cell.Value);
                    if (price == 0) throw new Exception();  //ввод нуля также является ошибкой.

                    int sparePartId = Convert.ToInt32(cell.OwningRow.Cells[SparePartId.Index].Value);
                    SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);
                    //Если цена продажи хотя бы где-то ниже закупочной требуем подтверждения действий.                         
                    if (sparePart.AvailabilityList.Any(av => av.OperationDetails.Price >= price))
                        if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            throw new Exception();


                    FillTheSumCell(cell.OwningRow);    //Заполняем и столбец 'Сумма'.
                }//try
                catch
                {
                    //выводим всплывающее окно с сообщением об ошибке.
                    toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                    //Очищаем ввод.
                    cell.Value = null;
                    isCellEditError = true;
                    lastEditCell = cell;
                }//catch
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

                sparePartsList.Add(sparePart); //Добавляем в список.

                cell.OwningRow.Cells[SellingPrice.Index].ReadOnly = cell.OwningRow.Cells[Count.Index].ReadOnly = false;
                cell.OwningRow.Cells[Title.Index].ReadOnly = cell.OwningRow.Cells[Articul.Index].ReadOnly = true;

                userText = null;
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

            //Ввод значения меньше 1, или больше чем есть в наличии является ошибкой. 
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
            row.Cells[SparePartId.Index].Value = sparePart.SparePartId;
            row.Cells[Title.Index].Value = sparePart.Title;
            row.Cells[Articul.Index].Value = sparePart.Articul;
            row.Cells[Unit.Index].Value = sparePart.MeasureUnit;

            row.Cells[Count.Index].Tag = Availability.GetTotalCount(sparePart.AvailabilityList); //Заполняем кол-во и запоминаем в Tag.
            SetDefaultValueToCell(row.Cells[Count.Index]); //Задаем серый цвет и дефолтное значение данной ячейке.
        }//FillTheSaleDGV

        /// <summary>
        /// Заполняет данными таблицу доп. инф-ции.
        /// </summary>
        /// <param name="availList">Список приходов данного товара в наличии.</param>
        private void FillTheExtDGV(List<Availability> availList)
        {
            foreach (Availability avail in availList)
            {
                int rowIndx = extDataGridView.Rows.Add();
                DataGridViewRow row = extDataGridView.Rows[rowIndx];

                row.Cells[extSupplier.Index].Value      = avail.OperationDetails.Operation.Contragent.ContragentName;
                row.Cells[extTitle.Index].Value         = avail.OperationDetails.SparePart.Title;
                row.Cells[extArticul.Index].Value       = avail.OperationDetails.SparePart.Articul;
                row.Cells[extUnit.Index].Value          = avail.OperationDetails.SparePart.MeasureUnit;
                row.Cells[extStorageAdress.Index].Value = avail.StorageAddress;
                row.Cells[extPrice.Index].Value         = avail.OperationDetails.Price;
                row.Cells[extMarkup.Index].Value        = Models.Markup.GetDescription(avail.Markup);
                row.Cells[extSellingPrice.Index].Value  = avail.SellingPrice;
                row.Cells[extPurchaseId.Index].Value    = avail.OperationDetails.Operation.OperationId;
                row.Cells[extPurchaseDate.Index].Value  = avail.OperationDetails.Operation.OperationDate;

                //Заполняем ячейку 'Кол-во' либо ранее установленным значением, иначе общим кол-вом по данному приходу в наличии. 
                OperationDetails operDet = _operDetList.FirstOrDefault(od => od.SparePart.SparePartId == avail.OperationDetails.SparePart.SparePartId
                                                                    && od.Operation.OperationId == avail.OperationDetails.Operation.OperationId);
                DataGridViewCell extCountCell = row.Cells[extCount.Index];
                if (operDet == null)
                {
                    extCountCell.Tag = avail.OperationDetails.Count; //заполняем ячейку значением и запоминаем это дефолтное значение в Tag.
                    SetDefaultValueToCell(extCountCell); //Задаем серый цвет и дефолтное значение данной ячейке.
                }//if
                else
                {
                    SetCustomValueToCell(extCountCell, operDet.Count); //Задаем значение ячейки.
                }//else
            }//foreach            

            //Если отпускная цена у всех приходов одинаковая, выводим её в saleDGV и если она уже не указана.
            if (lastEditCell.OwningRow.Cells[SellingPrice.Name].Value == null)
                if (!availList.Any(av => av.SellingPrice != availList[0].SellingPrice))
                    lastEditCell.OwningRow.Cells[SellingPrice.Name].Value = availList[0].SellingPrice;

            //Сортируем таблицу по дате прихода.
            extDataGridView.Sort(extPurchaseDate, ListSortDirection.Ascending);
            extDataGridView.ClearSelection();
        }//FillTheExtDGV


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
            int sparePartId = Convert.ToInt32(cell.OwningRow.Cells[SparePartId.Index].Value);
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
                    SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);
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
            if (row.Cells[SellingPrice.Index].Value != null && row.Cells[Count.Index].Value != null)
            {
                float price = Convert.ToSingle(row.Cells[SellingPrice.Index].Value);
                float sellCount = Convert.ToSingle(row.Cells[Count.Index].Value);

                row.Cells[Sum.Index].Value = price * sellCount;

                FillTheInTotal(); //Заполняем общую сумму операции.
            }//if
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
                if (row.Cells[SellingPrice.Index].Value != null && row.Cells[Count.Index].Value != null)
                {
                    float price = Convert.ToSingle(row.Cells[SellingPrice.Index].Value);
                    float sellCount = Convert.ToSingle(row.Cells[Count.Index].Value);
                    inTotal += price * sellCount;
                }//if
            }//foreach

            //Заполняем InTotalLabel расчитанным значением.
            inTotalNumberLabel.Text = String.Format("{0}(руб)", Math.Round(inTotal, 2, MidpointRounding.AwayFromZero));
        }//FillTheInTotal

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




        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Методы работы с выпадающим списком.

        private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
        {
            isCellEditError = true;
        }//autoCompleteListBox_MouseHover

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (String.IsNullOrEmpty(userText))
                    userText = textBoxCell.Text;
                saleDataGridView_SelectionChanged(null, null);
                isCellEditError = true;
            }
            else
            {
                isCellEditError = false;
                saleDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(lastEditCell.ColumnIndex, lastEditCell.RowIndex));
            }//else
        }//autoCompleteListBox_MouseDown

        private void autoCompleteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textChangedEvent = false;
            if (autoCompleteListBox.SelectedIndex != -1)
                textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
        }//autoCompleteListBox_SelectedIndexChanged


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
                DataGridViewRow row = lastEditCell.OwningRow;
                DataGridViewCell extCountCell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewCell countCell = row.Cells[Count.Index];
                //Проверяем корректность ввода.
                int sparePartId = Convert.ToInt32(row.Cells[SparePartId.Index].Value);
                string measureUnit = extCountCell.OwningRow.Cells[extUnit.Index].Value.ToString();                
                if (IsCountCellValueCorrect(extCountCell, measureUnit))
                {
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.
                    FillTheSumCell(row);                       //Заполняем столбец 'Сумма'.
                    
                }//if
                else
                {
                    toolTip.Show("Введены некорректные данные", this, GetExtCellBelowLocation(extCountCell), 1000);  //выводим всплывающее окно с сообщением об ошибке.
                    SetDefaultValueToCell(extCountCell); //Возвращаем серый цвет и дефолтное значение данной ячейке.
                    SaleDGVCountColumnUpdate(countCell); //Обновляем ячеку 'Кол-во' в таблице продаж.                    
                }//else       

                FillTheOperDetList(sparePartId, extCountCell); //Запоминаем изменение в список.    
            }//if       
        }//extDataGridView_CellEndEdit  






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
            Point dgvLoc = extDataGridView.Location;
            Point gbLoc = extGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation







//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion











//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var operDet in _operDetList)
            {
                string str = String.Format("{0} -- {1} :  {2}", operDet.SparePart.SparePartId, operDet.Operation.OperationId, operDet.Count);
                sb.Append(str);
                sb.Append("\n");
            }//foreach

            MessageBox.Show(sb.ToString());
        }
    }//Form2

}//namespace