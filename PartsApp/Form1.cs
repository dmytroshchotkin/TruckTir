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
        /// Список для выпадающего списка при поиске в searchTextBox.
        /// </summary>
        IList<SparePart> searchSpList;                                      
        /// <summary>
        /// Коллекция для запоминания объектов с изм. наценкой.
        /// </summary>
        IDictionary<int, IDictionary<int, double>> changeMarkupBufferDict;    
        /// <summary>
        /// Коллекция для вывода в таблицах.
        /// </summary>
        IList<SparePart> SpList, origSpList;                                
        
        /// <summary>
        /// Переменная для запоминания введенного поль-лем текста в searchTextBox.
        /// </summary>
        string userText;
        /// <summary>
        /// Авторизованный пользователь.
        /// </summary>
        public static Employee CurEmployee { get; set; }


        public Form1()
        {
            InitializeComponent();

            searchSpList = new List<SparePart>();
            changeMarkupBufferDict = new Dictionary<int, IDictionary<int, double>>();
            SpList = origSpList = new List<SparePart>();
        }//

        private void Form1_Load(object sender, EventArgs e)
        {
            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);

            #region Настройки таблиц.

            /*Закомментированные строки выполнены через дизайнер.*/
            partsDataGridView.AutoGenerateColumns = false;
            extPartsDataGridView.AutoGenerateColumns = false;
            

            //SupplierExtCol.DataPropertyName = "OperationDetails.Purchase.Contragent.ContragentName";
            //PurchaseIdExtCol.DataPropertyName = "OperationDetails.Purchase.OperationId";
            //ArticulExtCol.DataPropertyName = "OperationDetails.SparePart.Articul";
            //PurchaseDateExtCol.DataPropertyName = "OperationDetails.Purchase.OperationDate";
            //StorageAddressExtCol.DataPropertyName = "StorageAddress";
            //MeasureUnitExtCol.DataPropertyName = "OperationDetails.SparePart.MeasureUnit";
            //AvailabilityExtCol.DataPropertyName = "OperationDetails.Count";
            //SellingPriceExtCol.DataPropertyName = "SellingPrice";
            //NoteExtCol.DataPropertyName = "OperationDetails.Purchase.Description";
            
            //extPartsDataGridView.DataMember = "AvailabilityList";

            #endregion

            

            //Выводим окно авторизации.
            CurEmployee = PartsDAL.FindEmployees().First();
            //new AuthorizationForm().ShowDialog(this);
            userNameLabel.Text = String.Format("{0} {1}", CurEmployee.LastName, CurEmployee.FirstName);

            
            PartsDAL.RegistrateUDFs();
            /* Пробная зона */
            /////////////////////////////////////////////////////////////////////////////            


            //////////////////////////////////////////////////////////////////////////////
        }//Form1_Load

        #region Работа с Excel.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void addToDbFromExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (openExcelFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    partsDataGridView.Cursor = Cursors.WaitCursor;
            //    progressBar.Value += progressBar.Step; // нужно просто для визуального отображения начала работы. 
            //    foreach (var str in openExcelFileDialog.FileNames)
            //    {
            //        PartsDAL.AddSparePartsFromExcelFile(str);                   
            //        progressBar.Value += 100 / openExcelFileDialog.FileNames.Length;            
            //    }
            //    System.Threading.Thread t = new System.Threading.Thread(() =>
            //    {
            //        System.Threading.Thread.Sleep(1000);
            //        progressBar.Value = 0;
            //    });
            //    t.Start();
            //    partsDataGridView.Cursor = Cursors.Default;
            //}
        }//addToDbFromExcelToolStripMenuItem_Click

        private void saveInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Визуальное отображение работы.
            //progressBar.Value = progressBar.Maximum / 2;
            new System.Threading.Thread(beginSaveInExcel).Start(SpList); //Сделать по нормальному вызов с потоком.
            
        }//saveInExcelToolStripMenuItem_Click

        private void beginSaveInExcel(object spareParts)
        {            
            /*STUB*/
            if (spareParts is IList<SparePart>)
                saveInExcel(spareParts as IList<SparePart>);
        }//beginSaveInExcel

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
            //excelApp.Cells[row, column + 5] = "Сумма";

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
                ExcelApp.Cells[row, column + 4] = spareParts[i].AvailabilityList[0].OperationDetails.Count;
                //excelApp.Cells[row, column + 5] = availabilityList[i].Price;                
                //excelApp.Cells[row, column + 5] = availabilityList[i].Price * availabilityList[i].Count;
                ExcelApp.Cells[row, column + 5] = spareParts[i].AvailabilityList[0].SellingPrice;                
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
             IEnumerable<DataGridViewRow> selectedRows = partsDataGridView.SelectedCells.Cast<DataGridViewCell>()
                                                                                        .Select(cell => cell.OwningRow).Distinct();

            List<SparePart> sparePartsList = new List<SparePart>();
            foreach (DataGridViewRow row in selectedRows)
            {
                int sparePartId = Convert.ToInt32(row.Cells[SparePartIdCol.Name].Value);
                sparePartsList.Add(origSpList.First(s => s.SparePartId == sparePartId));
            }//foreach

            ExcelSaveSparePartPriceList(sparePartsList);
        }//SpPriceListToExcelToolStripMenuItem_Click

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
            if (String.IsNullOrWhiteSpace(searchTextBox.Text))
            {   
                autoCompleteListBox.Visible = false;
                return;
            }//if 

            //В зависимости от значения checkBox, выводим либо товар только в наличии, либо весь товар в базе.
            if (onlyAvaliabilityCheckBox.CheckState == CheckState.Unchecked)
                searchSpList = PartsDAL.SearchSpByTitleOrArticulOrManufacturerToDisplay(searchTextBox.Text, 10);
            else searchSpList = PartsDAL.SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(searchTextBox.Text, 10);

            ///*Выпадающий список в searchTextBox*/
            
            if (searchSpList.Count > 0)
            {
                OutputFormattedDropDownList(searchSpList);
            }//if
            else autoCompleteListBox.Visible = false; //Если ничего не найдено, убрать вып. список.

            //Запоминаем введенный текст.
            userText = searchTextBox.Text;
        }//searchTextBox_TextChanged
        
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            #region Нажатие клавиши "Вниз".

            if (e.KeyCode == Keys.Down)
            {
                if (autoCompleteListBox.Visible == false) return;

                //Если выбран последний эл-нт списка, вернуть начальное значение и убрать выделение в listBox-е. 
                if (autoCompleteListBox.SelectedIndex == autoCompleteListBox.Items.Count - 1)
                {
                    searchTextBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                    searchTextBox.SelectionStart = searchTextBox.Text.Length; //переводим каретку в конец строки.
                    return;
                }//if

                autoCompleteListBox.SelectedIndex += 1;
                ChangeSearchTextBoxTextWithoutTextChangedEvent(autoCompleteListBox.SelectedItem.ToString());
                return;
            }//if

            #endregion
            #region Нажатие клавиши "Вверх".

            if (e.KeyCode == Keys.Up)
            {
                if (autoCompleteListBox.Visible == false) return;

                //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
                if (autoCompleteListBox.SelectedIndex == -1)
                {
                    autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1;
                    ChangeSearchTextBoxTextWithoutTextChangedEvent(autoCompleteListBox.SelectedItem.ToString());
                    return;
                }
                //Если выбран верхний эл-нт вып. списка, вернуть введенную ранее пользователем строку.
                if (autoCompleteListBox.SelectedIndex == 0)
                {
                    //searchTextBox.Text = customerText;
                    searchTextBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                    searchTextBox.SelectionStart = searchTextBox.Text.Length; //переводим каретку в конец строки.
                    e.Handled = true;
                }//if
                else
                {
                    autoCompleteListBox.SelectedIndex -= 1;
                    ChangeSearchTextBoxTextWithoutTextChangedEvent(autoCompleteListBox.SelectedItem.ToString());
                }//else
                return;
            }//if 


            #endregion
            #region Нажатие клавиши "Enter".

            if (e.KeyCode == Keys.Enter)
            {
                //Если ничего не введено, то находим весь товар из базы.
                if (String.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    if (onlyAvaliabilityCheckBox.Checked)
                        ChangeDataSource(PartsDAL.SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(String.Empty));
                    else
                        ChangeDataSource(PartsDAL.SearchSpByTitleOrArticulOrManufacturerToDisplay(String.Empty));

                    return;
                }//if

                //Если нет элементов удовлетворяющих поиску, выводим сообщение об этом.
                if (searchSpList.Count == 0)
                {
                    toolTip.Show("Нет элементов удовлетворяющих поиску.", this, new Point(searchTextBox.Location.X, componentPanel.Location.Y), 2000);
                    return;
                }//if

                //распапсиваем введённую или выбранную строку.
                string[] titleOrArtOrManuf = searchTextBox.Text.Split(new string[] { "   " }, StringSplitOptions.RemoveEmptyEntries);
                
                //если выбор не из вып. списка.
                if (titleOrArtOrManuf.Length == 1)
                {
                    if (onlyAvaliabilityCheckBox.Checked)
                        ChangeDataSource(PartsDAL.SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(titleOrArtOrManuf[0]));
                    else 
                        ChangeDataSource(PartsDAL.SearchSpByTitleOrArticulOrManufacturerToDisplay(titleOrArtOrManuf[0]));

                    autoCompleteListBox.Visible = false;
                    return;
                }//if
                //вводим более корректное значение для отображение.
                ChangeSearchTextBoxTextWithoutTextChangedEvent(titleOrArtOrManuf[0]); 
                //Если имеются точное совпадение в введенном тексте и коллекции эл-тов вып. списка.
                foreach (var sparePart in searchSpList)
                {
                    if ((sparePart.Articul == titleOrArtOrManuf[0].Trim() && sparePart.Title == titleOrArtOrManuf[1].Trim()))
                    {
                        //если точное совпадение найдено.
                        ChangeDataSource(new List<SparePart>() { sparePart });
                        autoCompleteListBox.Visible = false;
                        return;
                    }//if 
                }//foreach

                userText = null;
                autoCompleteListBox.Visible = false;
                return;
            }//if

            #endregion
        }//searchTextBox_KeyDown

        private void onlyAvaliabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            searchTextBox_TextChanged(sender, e);
        }//onlyAvaliabilityCheckBox_CheckedChanged

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (String.IsNullOrEmpty(userText))
                    userText = searchTextBox.Text;

                ChangeSearchTextBoxTextWithoutTextChangedEvent(autoCompleteListBox.SelectedItem.ToString());
                searchTextBox.Focus();

            }//if
            else
            {
                searchTextBox_KeyDown(searchTextBox, new KeyEventArgs(Keys.Enter));
            }//else
        }//autoCompleteListBox_MouseDown

        /// <summary>
        /// Присваиваем searchTextBox текст выбранный из выпадающего списка, без вызова события TextChanged.
        /// </summary>
        /// <param name="text">Текст который будет вставлен в searchTextBox.</param>
        private void ChangeSearchTextBoxTextWithoutTextChangedEvent(string text)
        {
            searchTextBox.TextChanged -= searchTextBox_TextChanged;
            searchTextBox.Text = text; 
            searchTextBox.TextChanged += searchTextBox_TextChanged;
        }//ChangeSearchTextBoxTextWithoutTextChangedEvent

        /// <summary>
        /// Выводит на экран фоматированный выпадающий список из переданной коллекции SparePart.
        /// </summary>
        /// <param name="spList">Коллекция которая будет выведена в выпадающем списке.</param>
        private void OutputFormattedDropDownList(IList<SparePart> spList)
        {
            autoCompleteListBox.Items.Clear();
            //Форматируем вывод.
            //Находим максимальную ширину каждого параметра.
            int articulMaxLenght = spList.Select(sp => sp.Articul).Max(art => art.Length);
            int titlelMaxLenght  = spList.Select(sp => sp.Title).Max(title => title.Length);
            int manufMaxLenght = 0;
            var sparePartsManufacturers = spList.Select(sp => sp.Manufacturer).Where(manuf => manuf != null);
            if (sparePartsManufacturers.Count() > 0)
                manufMaxLenght = sparePartsManufacturers.Max(man => man.Length);
            //Задаём нужный формат для выводимых строк.
            string artCol   = String.Format("{{0, {0}}}", -articulMaxLenght);
            string titleCol = String.Format("{{1, {0}}}", -titlelMaxLenght);
            string manufCol = String.Format("{{2, {0}}}", -manufMaxLenght);

            string searchSparePart;
            for (int i = 0; i < spList.Count; ++i)
            {
                searchSparePart = String.Format(artCol + "   " + titleCol + "   " + manufCol, spList[i].Articul, spList[i].Title, spList[i].Manufacturer);
                autoCompleteListBox.Items.Add(searchSparePart);
            }//for
            autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
            autoCompleteListBox.Visible = true;

        }//OutputFormattedDropDownList
































////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Методы связанные с изменением Наценки.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                markupComboBox_SelectedIndexChanged(sender, null);                                
            }//if
        }//markupComboBox_PreviewKeyDown 

        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (partsDataGridView.SelectedCells.Count == 0) 
                return;

            
            //выделяем строки всех выделенных клеток.
            foreach (DataGridViewCell cell in partsDataGridView.SelectedCells)    cell.OwningRow.Selected = true;
            foreach (DataGridViewCell cell in extPartsDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            //узнаем процент заданной наценки.

            try
            {                
                float markup = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                //Если выделены только строки в partsDataGridView.
                if (extPartsDataGridView.SelectedRows.Count == 0)
                    partsDataGridViewMarkupChange(markup);

                else
                    extPartsDataGridViewMarkupChange(markup); //Если есть выделенные строки в extPartsDataGridView.

                //Делаем доступными кнопки "Сохранить изменения" и "Отменить изменения"
                saveChangesButton.Enabled = cancelChangesButton.Enabled = true; 
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
                PartsDAL.UpdateSparePartMarkup(changeMarkupBufferDict);
                //Действия осущ-мые при удачной записи в базу.
                saveChangesButton.Enabled = cancelChangesButton.Enabled = false; //делаем кнопки недоступными.

                //Перезаписываем начальный список.            
                origSpList = SparePart.GetNewSparePartsList(SpList);

                changeMarkupBufferDict.Clear(); //Очищаем словарь запчастей с измененной наценкой.
            }//try			
            catch (System.Data.SQLite.SQLiteException ex)
            {
                if (ex.Message == "database is locked\r\ndatabase is locked") /*ERROR!!! корявое сообщение*/
                    MessageBox.Show("Вероятно кто-то другой сейчас осуществляет запись в базу\nПопробуйте ещё раз.", "База данных занята в данный момент." );
                else MessageBox.Show(String.Format("Ошибка записи изменения наценки\n{0}", ex.Message));
            }//catch    

            Cursor = Cursors.Default;
        }//saveChangesButton_Click

        private void cancelChangesButton_Click(object sender, EventArgs e)
        {
            saveChangesButton.Enabled = cancelChangesButton.Enabled = false; //делаем кнопку недоступной.
             
            //Отменяем все изменения.
            ChangeDataSource(origSpList);

            changeMarkupBufferDict.Clear(); //Очищаем словарь запчастей с измененной наценкой.
        }//cancelChangesButton_Click

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void partsDataGridViewMarkupChange(float markup)
        {
            //Модифицировать!!! Сделать изменение через DataGridView, без циклов.
            //Находим все SP с изменяемой наценкой. 
            foreach (DataGridViewRow row in partsDataGridView.SelectedRows)
            {
                List<Availability> availList = (row.DataBoundItem as SparePart).AvailabilityList;
                if (availList.Count > 0)
                {
                    //Меняем наценку во всем списке этого товара в наличии.
                    availList.ForEach(av => av.Markup = markup);
                    //запоминем объекты Availability наценка кот. изменилась.                    
                    availList.ForEach(av => SaveMarkupChangeToBuffer(av));
                    //Меняем значение наценки в соотв. ячейках доп. таблицы.
                    foreach (DataGridViewRow extRow in extPartsDataGridView.Rows)
                        extRow.Cells[MarkupCol.Index].Value = Markup.GetDescription(markup); 
                    //Присваиваем новое значение столбцу 'ЦенаПродажи'.
                    row.Cells[SellingPriceCol.Name].Value = Availability.GetMaxSellingPrice(availList); 
                }//if                                                      
            }//foreach   
            extPartsDataGridView.Invalidate(); //Обновляем отображение столбцов в extPartsDataGridView.
        }//partsDataGridViewMarkupChange

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void extPartsDataGridViewMarkupChange(float markup)
        {
            //Находим все SP с изменяемой наценкой. 
            foreach (DataGridViewRow row in extPartsDataGridView.SelectedRows)
            {
                Availability avail = row.DataBoundItem as Availability;
                avail.Markup = markup;
                row.Cells[MarkupCol.Index].Value = Markup.GetDescription(markup); //Меняем тип наценки.
                //Заполняем столбец 'Цена продажи' в главной таблице.
                SetMaxValueToSellingPriceColumn(avail.OperationDetails.SparePart);
                //запоминем объекты Availability наценка кот. изменилась.
                SaveMarkupChangeToBuffer(avail);
                extPartsDataGridView.InvalidateCell(row.Cells[MarkupCol.Index]); //Обновляем измененную ячейку.
            }//foreach   
        }//extPartsDataGridViewMarkupChange

        /// <summary>
        /// Метод сохраняющий в буфер изменения связанные с наценкой. 
        /// </summary>
        /// <param name="sparePartId">Id запчасти с изменяемой наценкой.</param>
        /// <param name="saleId">Id прихода с изменяемой наценкой.</param>
        /// <param name="markup">Наценка на которую нужно изменить старое значение.</param>
        private void SaveMarkupChangeToBuffer(Availability avail)
        {
            int sparePartId = avail.OperationDetails.SparePart.SparePartId;
            int purchaseId = avail.OperationDetails.Purchase.OperationId;

            if (changeMarkupBufferDict.ContainsKey(sparePartId)) //Если уже есть такой SparePartId.
            {
                if (changeMarkupBufferDict[sparePartId].ContainsKey(purchaseId)) //если уже есть такой PurchaseId. 
                    (changeMarkupBufferDict[sparePartId])[purchaseId] = avail.Markup;
                else //если у данной SparePartId ещё нет такой PurchaseId.
                    (changeMarkupBufferDict[sparePartId]).Add(new KeyValuePair<int, double>(purchaseId, avail.Markup));
            }//if
            else //Если ещё нет данной SparePartId
            {
                IDictionary<int, double> dict = new Dictionary<int, double>();
                dict.Add(new KeyValuePair<int, double>(purchaseId, avail.Markup));
                changeMarkupBufferDict.Add(new KeyValuePair<int, IDictionary<int, double>>(sparePartId, dict));
            }//else
        }//SaveMarkupChangeToBuffer

        /// <summary>
        /// Проверяет одинаков ли Процент Наценки у всех эл-тов переданного списка запчастей. 
        /// </summary>
        /// <param name="availabilityList">Список проверяемых запчастей</param>
        /// <returns></returns>
        private bool IsSameMarkup(IList<SparePart> spareParts)
        {
            //Проверяем не одинаковая ли у всех записей цена продажи и процент наценки.
            bool isSameMarkup = true;
            for (int i = 0; i < spareParts.Count - 1; ++i)
            {
                for (int j = i + 1; j < spareParts.Count; ++j)
                {
                    //if (sparePartsList[i].Markup != sparePartsList[j].Markup) isSameMarkup = false;
                }//for j
                if (isSameMarkup == false) break;
            }//for i

            return isSameMarkup;
        }//IsSamePriceAndMarkup



















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Обработчики событий для талбиц.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Событие для окончания отображения Фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void partsDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            //Проверяем отображается ли сейчас photoPictureBox.
            if (photoPictureBox.Visible == true) 
                photoPictureBox.Visible = false;
        }//partsDataGridView_CellMouseLeave

        //Событие для отображения Фотографии.
        private void partsDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Если клетка находится в колонке Photo и при этом не является заголовком.
                if (e.ColumnIndex == partsDataGridView.Columns[PhotoCol.Name].Index && e.RowIndex != partsDataGridView.Columns[PhotoCol.Name].HeaderCell.RowIndex)
                {
                    DataGridViewCell cell = partsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //проверяем есть ли фото у данного эл-та.
                    if (cell.Value.ToString() == String.Empty) return;

                    if (System.IO.File.Exists(cell.Value.ToString()))
                    {
                        photoPictureBox.Image = new Bitmap(cell.Value.ToString());
                        #region Aльтернативный способ отображения клеток.
                        //1)
                        ////вычисляем положение клетки, для задания положения отображения Фотографии.
                        //int dispayedRows = partsDataGridView.DisplayedRowCount(true);
                        //if (partsDataGridView.Rows[e.RowIndex + dispayedRows/2].Displayed == false) //если клетка находится ниже половины отображаемых клеток, отображать Фото вверх.
                        //{
                        //    Rectangle rect = partsDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        //    photoPictureBox.Location = new Point(rect.X + rect.Width, rect.Y - photoPictureBox.PreferredSize.Height);
                        //}//if   
                        //else //иначе отображать вниз.
                        //{
                        //    Rectangle rect = partsDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex + 1, true);
                        //    photoPictureBox.Location = new Point(rect.X + rect.Width, rect.Y);
                        //} //else       

                        //2)отображение картинки всегда в правом углу DataGridView.                
                        //photoPictureBox.Location = new Point(partsDataGridView.Width - photoPictureBox.PreferredSize.Width, partsDataGridView.Location.Y);
                        #endregion

                        //Задаём выводимый на экран размер фото. 
                        System.Drawing.Size photoSize = new System.Drawing.Size(625, 450); //размер взят случайный. 

                        if (photoPictureBox.PreferredSize.Width <= photoSize.Width && photoPictureBox.PreferredSize.Height <= photoSize.Height)
                            photoPictureBox.Size = photoPictureBox.PreferredSize;
                        else
                        {
                            photoPictureBox.Image = ResizeOrigImg(photoPictureBox.Image, photoSize.Width, photoSize.Height);
                            photoPictureBox.Size = photoPictureBox.PreferredSize;
                        }
                        photoPictureBox.Visible = true;
                    }
                }//if
            }
            catch
            {
                //По-моему эта обработка нужна, для игнорирования ошибки. Проверить!
            }
        }//partsDataGridView_CellMouseEnter

        //Событие для отображения расширенной информации о Наличии запчасти.
        private void partsDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Если заголовок то ничего не делаем.
            if (e.RowIndex == partsDataGridView.Columns[0].HeaderCell.RowIndex)
                return;

            //Если ЛКМ
            if (e.Button == MouseButtons.Left)
            {
                excRateNumericUpDown.Enabled = true;
                markupComboBox.Enabled = true; //Делаем доступным функционал изменения наценки. 
            }//if
            //Если ПКМ, выводим контекстное меню.
            else
            {
                //Находим позицию в таблице, где был сделан клик.
                Point cellLocation = partsDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                Point location = new Point(cellLocation.X + e.X, cellLocation.Y + e.Y);
                //Выводим контекстное меню.
                partsDGVContextMenuStrip.Show(partsDataGridView, location);
            }//else
        }//partsDataGridView_CellMouseClick

        //Событие исп-ся для регулирования ширины RowHeaders.
        private void partsDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            //Заполняем столбец 'Цена продажи' и 'Наличие'.
            foreach (DataGridViewRow row in partsDataGridView.Rows)
            {
                SparePart sp = row.DataBoundItem as SparePart;
                if (sp.AvailabilityList.Count != 0)
                {
                    row.Cells[AvaliabilityCol.Index].Value = Availability.GetTotalCount(sp.AvailabilityList);
                    row.Cells[SellingPriceCol.Index].Value = Availability.GetMaxSellingPrice(sp.AvailabilityList);
                }//if
            }//foreach   

            //Обновляем rowsCountLabel по количеству строк. 
            rowsCountLabel.Text = partsDataGridView.Rows.Count.ToString();

            //обработка размера RowHeaders.
            int i, count = partsDataGridView.Rows.Count;
            for (i = 0; count != 0; ++i)
            {
                count /= 10;
            }//for    
            partsDataGridView.RowHeadersWidth = 41 + ((i - 1) * 7); //41 - изначальный размер RowHeaders

            changeMarkupBufferDict.Clear(); //очищаем список деталей с измененной наценкой. 
            saveChangesButton.Enabled = cancelChangesButton.Enabled = false;
            Deselection();

            //Устанавливаем постоянную позицию для отображения Фото.           
            DataGridViewCell cell2 = partsDataGridView.Columns[1].HeaderCell;
            Rectangle rect = partsDataGridView.GetCellDisplayRectangle(cell2.ColumnIndex, cell2.RowIndex, true);
            photoPictureBox.Location = new Point(rect.X + rect.Width + 10, partsDataGridView.Location.Y);
        }//partsDataGridView_DataSourceChanged

        /// <summary>
        /// Нумерация строк partsDataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void partsDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridView.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridView.Rows[index].HeaderCell.Value = indexStr;
        }//partsDataGridView_RowPrePaint



        /// <summary>
        /// Cобытие изменения DataSource в таблице доп. информации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void extPartsDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (extPartsDataGridView.DataSource == null)
                return;

            //обработка размера RowHeaders.
            int i, count = extPartsDataGridView.Rows.Count;
            for (i = 0; count != 0; ++i)
            {
                count /= 10;
            }//for    
            extPartsDataGridView.RowHeadersWidth = 41 + ((i - 1) * 7); //41 - изначальный размер RowHeaders

            //changeMarkupBufferDict.Clear(); //очищаем список деталей с измененной наценкой. 
            //убираем выделение строк.
            extPartsDataGridView.ClearSelection();
        }//extPartsDataGridView_DataSourceChanged

        private void extPartsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            DataGridViewColumn col = grid.Columns[e.ColumnIndex];

            if (row.DataBoundItem != null)
                if (col.DataPropertyName.Contains("."))
                {
                    string[] props = col.DataPropertyName.Split('.');
                    Type type = row.DataBoundItem.GetType();
                    System.Reflection.PropertyInfo propInfo = type.GetProperty(props[0]);
                    object val = propInfo.GetValue(row.DataBoundItem, null);
                    for (int i = 1; i < props.Length; i++)
                    {
                        propInfo = val.GetType().GetProperty(props[i]);
                        val = propInfo.GetValue(val, null);
                    }//for
                    e.Value = val;
                }//if
        }//extPartsDataGridView_CellFormatting

        private void extPartsDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }//extPartsDataGridView_RowsAdded

        private void extPartsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            StorageAddressExtCol.Visible = NoteExtCol.Visible = false;
            foreach (DataGridViewRow row in extPartsDataGridView.Rows)
            {
                Availability avail = row.DataBoundItem as Availability;

                row.Cells[MarkupCol.Index].Value = Markup.GetDescription(avail.Markup); //Заполняем ячейки столбца 'Тип наценки'

                //Делаем видимыми соотв. столбцы если в св-вах 'Адрес хранилища' и 'Примечание по поставке' есть данные.
                if (avail.StorageAddress != null)
                    StorageAddressExtCol.Visible = true;

                if (avail.OperationDetails.Purchase.Description != null)
                    NoteExtCol.Visible = true;
            }//foreach

            extPartsDataGridView.ClearSelection();//Убираем выделение ячейки.
        }//extPartsDataGridView_DataBindingComplete

        private void extPartsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == SellingPriceExtCol.Index)
            {
                DataGridViewRow extRow = extPartsDataGridView.Rows[e.RowIndex];

                Availability avail = extRow.DataBoundItem as Availability;
                extRow.Cells[MarkupCol.Index].Value = Markup.GetDescription(avail.Markup);//меняем тип наценки.

                //Обновляем ячейки 'Цена продажи' и 'Тип наценки'.
                extPartsDataGridView.InvalidateCell(extRow.Cells[e.ColumnIndex]);
                extPartsDataGridView.InvalidateCell(extRow.Cells[MarkupCol.Index]);

                //Обновляем столбец 'Цена продажи' в главной таблице.
                SetMaxValueToSellingPriceColumn(avail.OperationDetails.SparePart);
            }//if            
        }//extPartsDataGridView_CellEndEdit







        /// <summary>
        /// Задает макс. значение в необходимую ячейку столбца "SellingPriceCol".
        /// </summary>
        /// <param name="sparePart">Товар, в соотв. строке в таблице которого меняется цена продажи.</param>
        private void SetMaxValueToSellingPriceColumn(SparePart sparePart)
        {
            foreach (DataGridViewRow mainRow in partsDataGridView.Rows)
            {
                if (Convert.ToInt32(mainRow.Cells[SparePartIdCol.Index].Value) == sparePart.SparePartId)
                {
                    mainRow.Cells[SellingPriceCol.Index].Value = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);
                    break;
                }//if
            }//foreach
        }//SetMaxValueToSellingPriceColumn

        /// <summary>
        /// Метод изменения источника данных для обоих dataGridView.
        /// </summary>
        /// <param name="availabilityList">Новый источник данных для partsDataGridView.</param>
        private void ChangeDataSource(IList<SparePart> spareParts)
        {
            SpList = SparePart.GetNewSparePartsList(spareParts);
            origSpList = spareParts;

            BindingSource binding = new BindingSource();
            //binding.SuspendBinding();
            binding.DataSource = SpList;
            //binding.ResumeBinding();

            //Очищаем и заполняем DataSource новымы значениями.
            //partsDataGridView.DataSource = extPartsDataGridView.DataSource = null; /*Выдаёт ошибку при раскомментировании*/
            partsDataGridView.DataSource = extPartsDataGridView.DataSource = binding;

            
        }//ChangeDataSource

        /// <summary>
        /// Осуществляет действия необходимые при сбросе выделения.
        /// </summary>
        private void Deselection()
        {
            excRateNumericUpDown.Value = 1;
            excRateNumericUpDown.Enabled = false;
            //Очищаем строку и делаем функционал изменения наценки недоступным.
            markupComboBox.Text = String.Empty;
            markupComboBox.Enabled = false;

            extPartsDataGridView.ClearSelection();
        }//Deselection        

        /// <summary>
        /// Возвращает новый Image на основе переданного, с пропорционального уменьшения размеров до заданных.
        /// </summary>
        /// <param name="image">Image на основе которого возв-ся новый Image с измененным размером.</param>
        /// <param name="nWidth">Предположительная ширина нового Image.</param>
        /// <param name="nHeight">Предположительная высота нового Image.</param>
        /// <returns></returns>
        public Image ResizeOrigImg(Image image, int nWidth, int nHeight)
        {
            int newWidth, newHeight;

            var coefH = (double)nHeight / (double)image.Height;
            var coefW = (double)nWidth / (double)image.Width;
            if (coefW >= coefH)
            {
                newHeight = (int)(image.Height * coefH);
                newWidth = (int)(image.Width * coefH);
            }//if
            else
            {
                newHeight = (int)(image.Height * coefW);
                newWidth = (int)(image.Width * coefW);
            }//else

            Image result = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(result))
            {

                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(image, 0, 0, newWidth, newHeight);
                g.Dispose();
            }//using

            return result;
        }//ResizeOrigImg







////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void excRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ////Если нет выделенных строк, то выходим.
            //if (partsDataGridView.SelectedCells.Count == 0) return;

            ////выделяем строки всех выделенных клеток.
            //foreach (DataGridViewCell cell in partsDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            //foreach (DataGridViewCell cell in extPartsDataGridView.SelectedCells) cell.OwningRow.Selected = true;

            //foreach (DataGridViewRow row in partsDataGridView.SelectedRows)
            //{
            //    int sparePartId = Convert.ToInt32(row.Cells[SparePartIdCol.Name].Value);
            //    //Ищем все записи с нужным SparaPartId.
            //    foreach (var sparePart in ExtSpList)
            //    {
            //        if (sparePart.SparePartId == sparePartId)
            //        {
            //            //sparePart.ExcRate = (double)excRateNumericUpDown.Value;
            //        }//if
            //    }//foreach

            //    //Находим запись в SpList с данным SparePartId.
            //    foreach (var sparePart in SpList)
            //    {
            //        if (sparePart.SparePartId == sparePartId)
            //        {
            //            //sparePart.ExcRate = (double)excRateNumericUpDown.Value;
            //            row.Cells[SellingPriceCol.Name].Value = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);
            //            break;
            //        }//if
            //        partsDataGridView.InvalidateCell(row.Cells[SellingPriceCol.Name]);
            //    }//foreach
            //}//foreach   
            ////Обновляем отображение столбцов в extPartsDataGridView.
            //extPartsDataGridView.Invalidate();    

        }//excRateNumericUpDown_ValueChanged
        


        //События инициируемые для сброса выделения строк в partsDataGridView
        private void menuStrip_Click(object sender, EventArgs e)
        {
            Deselection();
        }//menuStrip_Click
        private void componentPanel_Click(object sender, EventArgs e)
        {
            Deselection();
        }//componentPanel_Click  
        private void partsStatusStrip_Click(object sender, EventArgs e)
        {
            Deselection();
        }      
        private void extPartsStatusStrip_Click(object sender, EventArgs e)
        {
            Deselection();
        }//extPartsStatusStrip_Click
        private void extPartsGroupBox_Click(object sender, System.EventArgs e)
        {
            extPartsDataGridView.ClearSelection(); //Убираем все выделения.
        }

        



        #region Методы вызова дополнительных окон.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        private void addNewSpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddSparePartForm().Show(this);
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
            new AddSparePartForm(Convert.ToInt32(partsDataGridView.SelectedCells[0].OwningRow.Cells[SparePartIdCol.Name].Value)).Show();
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
            int sparePartId = Convert.ToInt32(partsDataGridView.SelectedCells[0].OwningRow.Cells[SparePartIdCol.Name].Value);
            new SparePartOperationsInfoForm(sparePartId).Show();
        }//

        private void ViewInfoByContragentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Открываем форму инф-ции по поставщикам или клиетам в зависимости от выбранного меню.
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem == ViewInfoBySuppliersToolStripMenuItem)
                new ContragentOperationsInfoForm(typeof(Supplier)).Show();
            else
                new ContragentOperationsInfoForm(typeof(Customer)).Show();
        }



        



        

       












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
//http://stackoverflow.com/questions/1516252/how-to-programmatically-set-cell-value-in-datagridview
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