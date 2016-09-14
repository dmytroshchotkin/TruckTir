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
using PartsApp.SupportClasses;
using PartsApp.Models;

namespace PartsApp
{
    public partial class Form1 : Form
    {                                    
        /// <summary>
        /// Список объектов с измененной наценкой.
        /// </summary>
        List<Availability> _changedMarkupList;
        /// <summary>
        /// Авторизованный пользователь.
        /// </summary>
        public static Employee CurEmployee { get; set; }


        public Form1()
        {
            InitializeComponent();

            _changedMarkupList = new List<Availability>();
        }//

        private void Form1_Load(object sender, EventArgs e)
        {
            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);

            #region Настройки таблиц.

            /*Закомментированные строки выполнены через дизайнер.*/
            PartsDGV.AutoGenerateColumns    = false;
            ExtPartsDGV.AutoGenerateColumns = false;
            

            //SupplierExtCol.DataPropertyName       = "OperationDetails.Operation.Contragent.ContragentName";
            //PurchaseIdExtCol.DataPropertyName     = "OperationDetails.Operation.OperationId";
            //ArticulExtCol.DataPropertyName        = "OperationDetails.SparePart.Articul";
            //PurchaseDateExtCol.DataPropertyName   = "OperationDetails.Operation.OperationDate";
            //StorageAddressExtCol.DataPropertyName = "StorageAddress";
            //MeasureUnitExtCol.DataPropertyName    = "OperationDetails.SparePart.MeasureUnit";
            //AvailabilityExtCol.DataPropertyName   = "OperationDetails.Count";
            //SellingPriceExtCol.DataPropertyName   = "SellingPrice";
            //NoteExtCol.DataPropertyName           = "OperationDetails.Operation.Description";
            
            //extPartsDGV.DataMember = "AvailabilityList";

            #endregion
            
            //Выводим окно авторизации.
            CurEmployee = PartsDAL.FindEmployees().First();
            //new AuthorizationForm().ShowDialog(this);
            userNameLabel.Text = String.Format("{0} {1}", CurEmployee.LastName, CurEmployee.FirstName);

            PartsDAL.RegistrateUDFs(); //Регистрируем в СУБД user-defined functions.

            /* Пробная зона */
            /////////////////////////////////////////////////////////////////////////////            


            //////////////////////////////////////////////////////////////////////////////
        }//Form1_Load

        #region Работа с Excel.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private void saveInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Находим соотв. объекты SparePart для всех выведенных в таблице строк.
            List<SparePart> sparePartsList = PartsDGV.Rows.Cast<DataGridViewRow>().Select(r => r.DataBoundItem as SparePart).ToList();
            //Выводим в Excel.
            saveInExcelAsync(sparePartsList);
            //new System.Threading.Thread(beginSaveInExcel).Start(sparePartsList); //Сделать по нормальному вызов с потоком.
            
        }//saveInExcelToolStripMenuItem_Click

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="spareParts">Список товаров для вывода в Excel.</param>
        private async void saveInExcelAsync(IList<SparePart> spareParts)
        {
            try
            {
                await Task.Factory.StartNew(() => saveInExcel(spareParts));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }//saveInExcelAsync
        /// <summary>
        /// Вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="spareParts">Список товаров для вывода в Excel.</param>
        private void saveInExcel(IList<SparePart> spareParts)
        {            
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //Настраиваем горизонтальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin = 7;
            ExcelWorkSheet.PageSetup.RightMargin = 7;

            #region Вывод таблицы товаров.

            //Выводим заголовок.
            int row = 1, column = 1;
            ExcelApp.Cells[row, column] = "Произв.";
            ExcelApp.Cells[row, column + 1] = "Артикул";
            ExcelApp.Cells[row, column + 2] = "Название";
            ExcelApp.Cells[row, column + 3] = "Ед. изм.";
            ExcelApp.Cells[row, column + 4] = "Кол-во";
            ExcelApp.Cells[row, column + 5] = "Цена";
            //excelApp.Cells[extRow, column + 5] = "Сумма";

            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString());
            excelCells.Font.Bold = true;
            excelCells.Font.Size = 12;
            //Обводим заголовки таблицы рамкой. 
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
            //Устанавливаем стиль и толщину линии
            //excelCells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

            //Устанавливаем ширину первой Колонок
            double titleColWidth = 35; //50 -- Взято методом тыка.  
            int articulColWidth = 20;            
            //int manufColWidth = 15, minManufColWidth = 8; //  15 -- Взято методом тыка.

            SetColumnsWidth(spareParts, (ExcelApp.Cells[row, column + 2] as Excel.Range), (ExcelApp.Cells[row, column + 1] as Excel.Range), (ExcelApp.Cells[row, column] as Excel.Range));
            //Выводим список товаров.
            for (int i = 0; i < spareParts.Count; ++i)
            {
                ++row;
                ExcelApp.Cells[row, column + 2] = spareParts[i].Title;
                ExcelApp.Cells[row, column + 1] = spareParts[i].Articul;
                //Выравнивание диапазона строк.
                ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
                ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
                if (spareParts[i].Articul.Length > articulColWidth || spareParts[i].Title.Length > titleColWidth)
                {
                    ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;                    
                    //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
                    if (spareParts[i].Articul.Length > articulColWidth && spareParts[i].Title.Length <= titleColWidth)
                        (ExcelApp.Cells[row, column + 2] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    if (spareParts[i].Articul.Length <= articulColWidth && spareParts[i].Title.Length > titleColWidth)
                        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }//if
                
                ExcelApp.Cells[row, column] = spareParts[i].Manufacturer;

                ExcelApp.Cells[row, column + 3] = spareParts[i].MeasureUnit;
                ExcelApp.Cells[row, column + 4] = spareParts[i].AvailabilityList.Sum(av => av.OperationDetails.Count);
                //excelApp.Cells[extRow, column + 5] = availabilityList[i].Price;                
                //excelApp.Cells[extRow, column + 5] = availabilityList[i].Price * availabilityList[i].Count;
                if (spareParts[i].AvailabilityList.Count > 0)
                    ExcelApp.Cells[row, column + 5] = Availability.GetMaxSellingPrice(spareParts[i].AvailabilityList);      
            }//for

            //Обводим талицу рамкой. 
            excelCells = ExcelWorkSheet.get_Range("A" + (row - spareParts.Count + 1).ToString(), "F" + row.ToString());
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            #endregion

            //Визуальное отображение работы.
            //progressBar.Value = progressBar.Maximum;

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;
        
        }//saveInExcel

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
            double titleColWidth = 35; // -- Взято методом тыка.  
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




        private void SpPriceListToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Находим все строки в которых есть выделенные ячейки.
            IEnumerable<DataGridViewRow> selectedRows = PartsDGV.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            //Находим соотв. объекты из выделенных строк.
            List<SparePart> sparePartsList = selectedRows.Select(r => r.DataBoundItem as SparePart).ToList();
            //Выводим в Excel.
            ExcelSaveSparePartPriceListAsync(sparePartsList);
        }//SpPriceListToExcelToolStripMenuItem_Click

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции для распечатки ценников.
        /// </summary>
        /// <param name="sparePartsList">Список товаров для вывода в Excel.</param>
        private async void ExcelSaveSparePartPriceListAsync(IList<SparePart> sparePartsList)
        {
            try
            {
                await Task.Factory.StartNew(() => ExcelSaveSparePartPriceList(sparePartsList));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }//ExcelSaveSparePartPriceListAsync

        /// <summary>
        /// Вывод в Excel инф-ции для распечатки ценников.
        /// </summary>
        /// <param name="sparePartsList">Список товаров для вывода в Excel.</param>
        private void ExcelSaveSparePartPriceList(IList<SparePart> sparePartsList)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); ;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;
            ExcelWorkSheet.Columns["B"].ColumnWidth = 1; //задаем ширину второго столбца, для того чтобы корректно выделять рамкой ценники.

            //Заполняем Excel-файл, по 2 записи на строку.
            int row = 1;
            for (int i = 0; i < sparePartsList.Count; ++i)
            {
                FillExcelSheetPriceList(sparePartsList[i], row, 1, ExcelWorkSheet);
                if (++i < sparePartsList.Count)
                    row = FillExcelSheetPriceList(sparePartsList[i], row, 3, ExcelWorkSheet);

                row += 2;                
            }//for
            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;
        }//ExcelSaveSparePartPriceList

        private int FillExcelSheetPriceList(SparePart sparePart, int startRow, int column, 
                                             Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet)
        {
            int row = startRow, columnWidth = 50;
            string columnChar = (column == 1) ? "A" : "C";
            ExcelWorkSheet.Cells[row, column].Columns.ColumnWidth = columnWidth; //задаём ширину столбца.
                        
            ExcelWorkSheet.Cells[row, column] = sparePart.Articul; //Выводим Артикул.
            row += 2;
            ExcelWorkSheet.Cells[row, column] = sparePart.Title; //Выводим Название.
            ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString()).Font.Size = 12;
            ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            //Если не влазиет в строку, делаем перенос.
            if (sparePart.Title.Length > columnWidth - 5)
                ExcelWorkSheet.Cells[row, column].HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;

            //Выводим Розничную цену.
            row += 2;
            if (sparePart.AvailabilityList.Count > 0)
                ExcelWorkSheet.Cells[row, column] = String.Format("{0:0.00} руб", Availability.GetMaxSellingPrice(sparePart.AvailabilityList));
            Excel.Range excelCells = ExcelWorkSheet.get_Range(columnChar + row.ToString());
            excelCells.Font.Size = 24;
            //Выравниваем по центру.
            ExcelWorkSheet.Cells[row, column].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            //Обводим рамкой. 
            ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString()).Font.Bold = true;
            excelCells = ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString());
            excelCells.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlRgbColor.rgbBlack);

            return row;
        }//FillExcelSheetPriceList
        








////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion        

        #region Методы связанные с поиском товара.       
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            autoCompleteListBox.DataSource = null;

            if (!String.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                //В зависимости от значения checkBox, выводим либо товар только в наличии, либо весь товар в базе.
                List<SparePart> searchSparePartsList = PartsDAL.SearchSpareParts(searchTextBox.Text.Trim(), onlyAvaliabilityCheckBox.Checked, 10);

                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    //Заполняем вып. список новыми объектами.
                    autoCompleteListBox.DataSource = searchSparePartsList;
                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                    autoCompleteListBox.ClearSelected();
                }//if
            }//if
        }//searchTextBox_TextChanged

        
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; //Перехватываем событие для того чтобы каретка не меняла свою позицию в строке.

            switch (e.KeyCode)
            {
                case Keys.Down:
                    AutoCompleteListBox.KeyDownPress(autoCompleteListBox);
                    break;
                case Keys.Up:
                    AutoCompleteListBox.KeyUpPress(autoCompleteListBox);                    
                    break;
                case Keys.Enter:
                    KeyEnterPress();
                    break;
            }//switch
        }//searchTextBox_KeyDown


        private void onlyAvaliabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            searchTextBox_TextChanged(sender, e);
        }//onlyAvaliabilityCheckBox_CheckedChanged

        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Enter.
        /// </summary>
        private void KeyEnterPress()
        {
            //Если нет элементов удовлетворяющих поиску, выводим сообщение об этом.
            if (autoCompleteListBox.Items.Count == 0 && !String.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                toolTip.Show("Нет элементов удовлетворяющих поиску.", this, new Point(searchTextBox.Location.X, componentPanel.Location.Y), 2000);
            }//if
            else
            {
                //Если есть выбранный элемент, выводим его.
                if (autoCompleteListBox.SelectedItem != null)
                    ChangeDataSource(new List<SparePart>() { autoCompleteListBox.SelectedItem as SparePart });
                else //Если выбранного элемента нет
                {
                    //Если вып. список заполнен меньше макс. кол-ва, заполняем таблицу эл-ми вып. списка.
                    if (autoCompleteListBox.Items.Count > 0 && autoCompleteListBox.Items.Count < 10)
                        ChangeDataSource(autoCompleteListBox.DataSource as List<SparePart>);
                    else
                        ChangeDataSource(PartsDAL.SearchSpareParts(searchTextBox.Text.Trim(), onlyAvaliabilityCheckBox.Checked));
                }//else
            }//else
        }//KeyEnterPress

        



        #region Методы работы с вып. списком.
//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

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

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
                searchTextBox.Focus();
            else
                searchTextBox_KeyDown(searchTextBox, new KeyEventArgs(Keys.Enter));
        }//autoCompleteListBox_MouseDown




//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion





























        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы связанные с изменением Наценки.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void markupComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                markupComboBox_SelectedIndexChanged(sender, null);
                e.SuppressKeyPress = true; //Для предотвращения звука некорректного ввода.
            }//if
        }//markupComboBox_KeyDown

        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {            
            try
            {
                //узнаем процент заданной наценки.
                float markup = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                MarkupChanged(markup); //Меняем наценку.
            }//try                
            catch 
            { 
                toolTip.Show("Введены некорректные значения", this, markupComboBox.Location, 2000); 
            }//catch            
        }//markupComboBox_SelectedIndexChanged

        private void saveChangesButton_Click(object sender, EventArgs e)
        {                      
            Cursor = Cursors.WaitCursor;
            try
            {
                PartsDAL.UpdateSparePartMarkup(_changedMarkupList);
                //Действия осущ-мые при удачной записи в базу.
                SaveAndCancelChangesButtonsSetEnable(false); //делаем кнопки недоступными.
            }//try			
            catch (System.Data.SQLite.SQLiteException ex)
            {
                if (ex.Message == "database is locked\r\ndatabase is locked") 
                    MessageBox.Show("Вероятно кто-то другой сейчас осуществляет запись в базу\nПопробуйте ещё раз.", "База данных занята в данный момент." );
                else 
                    MessageBox.Show(String.Format("Ошибка записи изменения наценки\n{0}", ex.Message));
            }//catch    

            Cursor = Cursors.Default;
        }//saveChangesButton_Click

        private void cancelChangesButton_Click(object sender, EventArgs e)
        {
            /*ERROR Не нравиться мне код метода*/

            _changedMarkupList.ForEach(av => av.Markup = (float)av.Tag); //Присваиваем первоначальную наценку всем необх. объектам.

            //Меняем значение наценки в соотв. ячейках доп. таблицы.
            foreach (DataGridViewRow extRow in ExtPartsDGV.Rows)
                extRow.Cells[MarkupExtCol.Index].Value = Markup.GetDescription((extRow.DataBoundItem as Availability).Markup);

            ExtPartsDGV.InvalidateColumn(SellingPriceExtCol.Index); //обновляем столбец 'Цена продажи' в доп. таблице.

            //Обновляем значения в ячейке 'Цена продажи' осн. таблицы.
            foreach (DataGridViewRow row in PartsDGV.Rows)
            {
                SparePart sparePart = row.DataBoundItem as SparePart;
                Availability avail = _changedMarkupList.FirstOrDefault(av => av.OperationDetails.SparePart.SparePartId == sparePart.SparePartId);
                if (avail != null)
                    row.Cells[SellingPriceCol.Name].Value = Availability.GetMaxSellingPrice(sparePart.AvailabilityList); //Присваиваем новое значение столбцу 'ЦенаПродажи'.
            }//foreach

            SaveAndCancelChangesButtonsSetEnable(false);
        }//cancelChangesButton_Click

        private void SaveAndCancelChangesButtonsSetEnable(bool enabled)
        {
            saveChangesButton.Enabled = cancelChangesButton.Enabled = enabled;
            //Если кнопка стала недоступной, то очищаем список объетов с изм. наценкой.
            if (enabled == false)
                _changedMarkupList.Clear();
        }//SaveAndCancelChangesButtonsSetEnable

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void PartsDGVMarkupChange(float markup)
        {
            //Находим весь товар с изменяемой наценкой. 
            foreach (DataGridViewRow row in PartsDGV.SelectedRows)
            {
                List<Availability> availList = (row.DataBoundItem as SparePart).AvailabilityList;
                if (availList.Count > 0)
                {                    
                    availList.ForEach(av => MarkupChanged(av, markup)); //Меняем наценку во всем cоотв. объектах.
                    //Меняем значение наценки в соотв. ячейках доп. таблицы.
                    ExtPartsDGV.Rows.Cast<DataGridViewRow>().ToList().ForEach(r => r.Cells[MarkupExtCol.Index].Value = Markup.GetDescription(markup));
                    //Присваиваем новое значение столбцу 'ЦенаПродажи'.
                    row.Cells[SellingPriceCol.Name].Value = Availability.GetMaxSellingPrice(availList); 
                }//if                                                      
            }//foreach   

            ExtPartsDGV.InvalidateColumn(SellingPriceExtCol.Index); //обновляем столбец 'Цена продажи' в доп. таблице.
        }//PartsDGVMarkupChange

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void ExtPartsDGVMarkupChange(float markup)
        {
            //Находим все SP с изменяемой наценкой. 
            foreach (DataGridViewRow extRow in ExtPartsDGV.SelectedRows)
            {
                Availability avail = extRow.DataBoundItem as Availability;
                MarkupChanged(avail, markup); //запоминем объекты Availability наценка кот. изменилась.

                extRow.Cells[MarkupExtCol.Index].Value = Markup.GetDescription(markup); //Меняем тип наценки.
                ExtPartsDGV.InvalidateCell(extRow.Cells[SellingPriceExtCol.Index]); //Обновляем измененную ячейку
            }//foreach  

            //Заполняем столбец 'Цена продажи' в главной таблице.
            SparePart sparePart = (ExtPartsDGV.SelectedRows[0].DataBoundItem as Availability).OperationDetails.SparePart;
            SetMaxValueToSellingPriceColumn(sparePart);
        }//ExtPartsDGVMarkupChange

        /// <summary>
        /// Производятся необходимые действия при изменении наценки объекта типа Availability.
        /// </summary>
        /// <param name="markup">Новая наценка.</param>
        private void MarkupChanged(float markup)
        {
            //Выделяем строки всех выделенных ячеек.            
            foreach (DataGridViewCell cell in PartsDGV.SelectedCells)    cell.OwningRow.Selected = true;    //partsDGV.SelectedCells.Cast<DataGridViewCell>().ToList().ForEach(c => c.OwningRow.Selected = true);
            foreach (DataGridViewCell cell in ExtPartsDGV.SelectedCells) cell.OwningRow.Selected = true;

            //В зависимотсти от того, есть ли выделенные ячейки в доп. таблице, вызываем соотв. метод измененния наценки.
            if (ExtPartsDGV.SelectedRows.Count == 0)
                PartsDGVMarkupChange(markup);
            else
                ExtPartsDGVMarkupChange(markup);
        }//MarkupChanged

        /// <summary>
        /// Производятся необходимые действия при изменении наценки объекта типа Availability.
        /// </summary>
        /// <param name="avail">Объект с изменяемой наценкой.</param>
        /// <param name="markup">Новая наценка.</param>
        private void MarkupChanged(Availability avail, float markup)
        {
            //Если изменена дефолтная наценка, запоминаем её в Tag объекта.
            if (avail.Tag == null)
                avail.Tag = avail.Markup;
            avail.Markup = markup;

            //Если такого объекта ещё нет в списке.
            if (!_changedMarkupList.Contains(avail))
            {
                //Если новая наценка не равна первоначальной, добавляем объект в список.
                if (avail.Markup != (float)avail.Tag)
                    _changedMarkupList.Add(avail);
            }//if 
            else //Если такой объект уже есть в списке.
            {
                //Если новая наценка равна первоначальной, удаляем объект из списка.
                if (avail.Markup == (float)avail.Tag)
                    _changedMarkupList.Remove(avail);
            }//else

            //Если в списке товаров с изм. наценкой есть хоть один объект, делаем доступными кнопки Сохранения и Отмены, иначе делаем недоступными.
            SaveAndCancelChangesButtonsSetEnable(_changedMarkupList.Count > 0); 
        }//MarkupChanged



















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Обработчики событий для талбиц.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Событие для окончания отображения Фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void partsDGV_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            //Проверяем отображается ли сейчас photoPictureBox.
            if (photoPictureBox.Visible == true) 
                photoPictureBox.Visible = false;
        }//partsDGV_CellMouseLeave

        /// <summary>
        /// Событие для отображения Фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void partsDGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Если клетка находится в колонке Photo и при этом не является заголовком.
            if (e.ColumnIndex == PhotoCol.Index && e.RowIndex != -1)
            {
                DataGridViewCell cell = PartsDGV[e.ColumnIndex, e.RowIndex];
                //Если у данного объекта есть фото.
                if (cell.Value != null)
                {
                    if (System.IO.File.Exists(cell.Value.ToString()))
                    {
                        photoPictureBox.Image = new Bitmap(cell.Value.ToString());

                        //Задаём выводимый на экран размер фото. 
                        Size photoSize = new System.Drawing.Size(625, 450); //размер взят случайный. 

                        //Если картинка больше размером чем заданный размер, то подгоняем её под заданный размер.
                        if (photoPictureBox.PreferredSize.Width > photoSize.Width || photoPictureBox.PreferredSize.Height > photoSize.Height)
                            photoPictureBox.Image = ResizeOrigImg(photoPictureBox.Image, photoSize.Width, photoSize.Height);

                        photoPictureBox.Size = photoPictureBox.PreferredSize;
                        photoPictureBox.Visible = true;
                    }//if
                    else
                    { 
                        /*ERROR*/
                        //Удаление данных о фото у объкта, если такого фото уже нет в папке.
                    }//else
                }//if
            }//if
        }//partsDGV_CellMouseEnter

        //Событие для отображения расширенной информации о Наличии запчасти.
        private void partsDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Если клик сделан не по заголовку таблицы.
            if (e.RowIndex != -1)
            {
                //Если ПКМ, выводим контекстное меню.
                if (e.Button == MouseButtons.Right)
                {
                    PartsDGV[e.ColumnIndex, e.RowIndex].Selected = true;
                    //Находим позицию в таблице, где был сделан клик и выводим контекстное меню.
                    Rectangle cellRect = PartsDGV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);                    
                    partsDGVContextMenuStrip.Show(PartsDGV, new Point(cellRect.X, cellRect.Bottom));                
                }//if                
            }//if                   
        }//partsDGV_CellMouseClick

        //Событие исп-ся для регулирования ширины RowHeaders.
        private void partsDGV_DataSourceChanged(object sender, EventArgs e)
        {
            excRateNumericUpDown.Enabled = markupComboBox.Enabled = true; //Делаем доступным функционал изменения наценки и перерасчета по валюте.
            FillColumns();                                                //Заполняем столбец 'Цена продажи' и 'Наличие'.  
            rowsCountLabel.Text = PartsDGV.Rows.Count.ToString();         //Обновляем rowsCountLabel по количеству строк.
            EnumerableExtensions.RowsNumerateAndAutoSize(PartsDGV);       //Нумерация строк.    

            SaveAndCancelChangesButtonsSetEnable(false);
            ExtPartsDGVClearSelection(null, null); //сбрасываем выделение в доп. таблице.
        }//partsDGV_DataSourceChanged

        private void partsDGV_Sorted(object sender, EventArgs e)
        {
            FillColumns();                                           //Заполняем ячейки столбцов 'Цена продажи' и 'Наличие'.
            EnumerableExtensions.RowsNumerateAndAutoSize(PartsDGV);  //Нумеруем строки.
        }//partsDGV_Sorted


        /// <summary>
        /// Метод для корректной binding-привязки вложенных эл-тов объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void extPartsDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid      = (DataGridView)sender;
            DataGridViewRow row    = grid.Rows[e.RowIndex];
            DataGridViewColumn col = grid.Columns[e.ColumnIndex];

            if (row.DataBoundItem != null)
            {
                if (col.DataPropertyName.Contains("."))
                {
                    string[] props = col.DataPropertyName.Split('.');
                    Type type = row.DataBoundItem.GetType();
                    System.Reflection.PropertyInfo propInfo = type.GetProperty(props[0]);
                    object val = propInfo.GetValue(row.DataBoundItem, null);
                    for (int i = 1; i < props.Length; i++)
                    {
                        Type valueType = val.GetType();
                        propInfo = valueType.GetProperty(props[i]);
                        val = propInfo.GetValue(val, null);
                    }//for
                    e.Value = val;
                }//if
            }//if
        }//extPartsDGV_CellFormatting

        private void extPartsDGV_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            StorageAddressExtCol.Visible = NoteExtCol.Visible = false;
            foreach (DataGridViewRow row in ExtPartsDGV.Rows)
            {
                Availability avail = row.DataBoundItem as Availability;

                row.Cells[MarkupExtCol.Index].Value = Markup.GetDescription(avail.Markup); //Заполняем ячейки столбца 'Тип наценки'

                //Делаем видимыми соотв. столбцы если в св-вах 'Адрес хранилища' и 'Примечание по поставке' есть данные.
                if (avail.StorageAddress != null)
                    StorageAddressExtCol.Visible = true;

                if (avail.OperationDetails.Operation.Description != null)
                    NoteExtCol.Visible = true;
            }//foreach

            EnumerableExtensions.RowsNumerateAndAutoSize(ExtPartsDGV); //Нумерация строк.
            ExtPartsDGV.ClearSelection();                              //Убираем выделение ячейки.
        }//extPartsDGV_DataBindingComplete

        private void extPartsDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == SellingPriceExtCol.Index)
            {
                DataGridViewRow extRow = ExtPartsDGV.Rows[e.RowIndex];

                Availability avail = extRow.DataBoundItem as Availability;
                extRow.Cells[MarkupExtCol.Index].Value = Markup.GetDescription(avail.Markup);//меняем тип наценки.

                //Обновляем ячейки 'Цена продажи' и 'Тип наценки'.
                ExtPartsDGV.InvalidateCell(extRow.Cells[e.ColumnIndex]);
                ExtPartsDGV.InvalidateCell(extRow.Cells[MarkupExtCol.Index]);

                //Обновляем столбец 'Цена продажи' в главной таблице.
                SetMaxValueToSellingPriceColumn(avail.OperationDetails.SparePart);
            }//if            
        }//extPartsDGV_CellEndEdit







        /// <summary>
        /// Задает макс. значение в необходимую ячейку столбца "SellingPriceCol".
        /// </summary>
        /// <param name="sparePart">Товар, в соотв. строке в таблице которого меняется цена продажи.</param>
        private void SetMaxValueToSellingPriceColumn(SparePart sparePart)
        {
            foreach (DataGridViewRow mainRow in PartsDGV.Rows)
            {
                if (Convert.ToInt32(mainRow.Cells[SparePartIdCol.Index].Value) == sparePart.SparePartId)
                {
                    mainRow.Cells[SellingPriceCol.Index].Value = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);
                    break;
                }//if
            }//foreach
        }//SetMaxValueToSellingPriceColumn

        /// <summary>
        /// Метод изменения источника данных для обоих dgv.
        /// </summary>
        /// <param name="availabilityList">Новый источник данных для partsDGV.</param>
        private void ChangeDataSource(IList<SparePart> spareParts)
        {
            autoCompleteListBox.Visible = false;
            //Заполняем DataSource новымы значениями.
            PartsDGV.DataSource = ExtPartsDGV.DataSource = new BindingSource(new SortableBindingList<SparePart>(spareParts), null);            
        }//ChangeDataSource

        /// <summary>
        /// Заполняем ячейки столбцов 'Цена продажи' и 'Наличие'.
        /// </summary>
        private void FillColumns()
        {
            foreach (DataGridViewRow row in PartsDGV.Rows)
            {
                SparePart sp = row.DataBoundItem as SparePart;
                if (sp.AvailabilityList.Count != 0)
                {
                    row.Cells[AvaliabilityCol.Index].Value = Availability.GetTotalCount(sp.AvailabilityList);
                    row.Cells[SellingPriceCol.Index].Value = Availability.GetMaxSellingPrice(sp.AvailabilityList);
                }//if
            }//foreach  
        }//FillColumns

        /// <summary>
        /// Осуществляет действия необходимые при сбросе выделения.
        /// </summary>
        private void ExtPartsDGVClearSelection(object sender, EventArgs e)
        {
            ////Очищаем строку и делаем функционал изменения наценки недоступным.
            //excRateNumericUpDown.Value = 1;
            //markupComboBox.Text = String.Empty;  
            //excRateNumericUpDown.Enabled = markupComboBox.Enabled = false;
                        
            ExtPartsDGV.ClearSelection();
        }//ExtPartsDGVClearSelection        

        /// <summary>
        /// Возвращает новый Image на основе переданного, с пропорционального уменьшения размеров до заданных.
        /// </summary>
        /// <param name="image">Image на основе которого возв-ся новый Image с измененным размером.</param>
        /// <param name="nWidth">Предположительная ширина нового Image.</param>
        /// <param name="nHeight">Предположительная высота нового Image.</param>
        /// <returns></returns>
        public Image ResizeOrigImg(Image image, int nWidth, int nHeight)
        {
            double coefH = (double)nHeight / (double)image.Height;
            double coefW = (double)nWidth / (double)image.Width;

            int newWidth  = (coefW >= coefH) ? (int)(image.Width * coefH)  : (int)(image.Width * coefW);
            int newHeight = (coefW >= coefH) ? (int)(image.Height * coefH) : (int)(image.Height * coefW);

            Image result = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(image, 0, 0, newWidth, newHeight);
                g.Dispose();
            }//using

            return result;
        }//ResizeOrigImg







////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void excRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //Если есть выделенные строки.
            if (PartsDGV.SelectedCells.Count != 0)
            {
                //выделяем строки всех выделенных клеток.
                foreach (DataGridViewCell cell in PartsDGV.SelectedCells) cell.OwningRow.Selected    = true;
                foreach (DataGridViewCell cell in ExtPartsDGV.SelectedCells) cell.OwningRow.Selected = true;

                decimal rate = excRateNumericUpDown.Value; //Находим установленный курс.
                foreach (DataGridViewRow row in PartsDGV.SelectedRows)
                {
                    SparePart sparePart = row.DataBoundItem as SparePart; //Находим соотв. строке объект.
                    float selPrice = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);

                    //Присваиваем новое значение в ячейку 'Цена продажи'.
                    row.Cells[SellingPriceCol.Name].Value = (decimal)selPrice / rate;
                }//foreach     
            }//if
        }//excRateNumericUpDown_ValueChanged
        
        /// <summary>
        /// Сброс выделения в доп. таблице.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void extPartsGroupBox_Click(object sender, System.EventArgs e)
        {
            ExtPartsDGV.ClearSelection();
        }//extPartsGroupBox_Click

        



        #region Методы вызова дополнительных окон.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        private void addNewSpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SparePartForm().Show(this);
        }//addNewSpToolStripMenuItem_Click

        private void addNewSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddContragentForm(new Supplier()).Show();
        }//addNewSupplierToolStripMenuItem_Click

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddContragentForm(new Customer()).Show();             
        }//addNewCustomerToolStripMenuItem_Click

        private void addNewPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PurchaseForm().Show(this);
        }//addNewPurchaseToolStripMenuItem_Click

        private void addNewSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SaleForm().Show(this);
        }//addNewSaleToolStripMenuItem_Click

        private void editSparePartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparePart sparePart = PartsDGV.SelectedCells[0].OwningRow.DataBoundItem as SparePart;
            new SparePartForm(sparePart).Show();
        }//editSparePartToolStripMenuItem_Click

        


        private void addNewEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddEmployeeForm().ShowDialog();
        }//addNewEmployeeToolStripMenuItem_Click

        private void editEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddEmployeeForm(CurEmployee).ShowDialog();
        }

        private void посмотретьПередвижениеТовараToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparePart sparePart = PartsDGV.SelectedCells[0].OwningRow.DataBoundItem as SparePart;
            new SparePartOperationsInfoForm(sparePart).Show();
        }//

        private void ViewInfoByContragentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Открываем форму инф-ции по поставщикам или клиетам в зависимости от выбранного меню.
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            Type contragentType = (menuItem == ViewSuppliersInfoToolStripMenuItem) ? typeof(Supplier) : typeof(Customer);       
            new ContragentOperationsInfoForm(contragentType).Show();
        }//

        









        



        

       












////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

    }//Form1

}//namespace

/*Задачи*/
//1) Выгрузка инф-ции в Excel.
//2) Добавить в extPartsDGV поля "поставщик" и "адрес" или "название" вирт. склада.
//3) Добавление с и выгрузка в excel файл.
//4) Сделать отдельную таблицу MarkupsTypes и реализовать добавление/удаление в неё.
//5) Реализовать задание наценки вручную.  

/*Будущие задачи*/
//1) Посмотреть способ изменения DataGridView.Rows[x].Cells[y].Value без изменения DataSource.
//http://stackoverflow.com/questions/1516252/how-to-programmatically-set-countCell-value-in-datagridview
//2)Добавить поиск по Manufacturer.
//3)Сделать запись в БД регистронезависимой (или поиск по БД).
//4)Сделать чтобы markupComboBox становился доступным при выделении клеток (без клика)
//5)Сделать корректное отображение progressBar.
//6)Вывод красным цветом значения '0' в Наличии.
//8)Добавить сортировку по многим параметрам.

/*Сам столкнулся с этим когда работают два процесса. Один создаёт буфер, второй пишет в него. 
 Ошибка возникает, когда первый процесс внезапно сменяет буфер, второй в это время "промахивается".*/


/*Рефакторинг*/
//1)saveChangesButton_Click