using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PartsApp
{
    public partial class Form1 : Form
    {
        IList<SparePart> searchSpList;                                      //для выпадающего списка в searchTextBox.
        IDictionary<int, IDictionary<int, double>> changeMarkupBufferDict;  //для изменения наценки.  
        IList<SparePart> SpList, origSpList;                                //для вывода в partsDataGridView.  
        IList<SparePart> ExtSpList, origExtSpList;                          //для вывода в extPartsDataGridView.  
        bool textChangeEvent;                                               //есть ли подписчик на searchTextBox_TextChanged

        public Form1()
        {
            InitializeComponent();

            searchSpList = new List<SparePart>();
            changeMarkupBufferDict = new Dictionary<int, IDictionary<int, double>>();
            SpList = origSpList = new List<SparePart>();
            ExtSpList = origExtSpList = new List<SparePart>();

            textChangeEvent = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Создаём коллекцию SparePart присваиваем её DataGridView и работаем с ней для инициализации 
              нашей коллекции для поиска по базе данных*/
            partsDataGridView.DataSource = SpList = PartsDAL.FindAllSparePartsAvaliableToDisplay();
            origSpList = PartsDAL.FindAllSparePartsAvaliableToDisplay();

            ExtSpList = PartsDAL.FindAvaliabilityBySparePartId(SpList);
            origExtSpList = PartsDAL.FindAvaliabilityBySparePartId(SpList);            
            
            //Устанавливаем постоянную позицию для отображения Фото.
            //DataGridViewCell cell2 = partsDataGridView.Rows[0].Cells["Photo"];
            DataGridViewCell cell2 = partsDataGridView.Columns[1].HeaderCell;
            Rectangle rect = partsDataGridView.GetCellDisplayRectangle(cell2.ColumnIndex, cell2.RowIndex, true);
            photoPictureBox.Location = new Point(rect.X + rect.Width + 10, partsDataGridView.Location.Y);

            //Вносим все типы наценок в markupComboBox             
            markupComboBox.Items.AddRange(PartsDAL.FindAllMarkups().Select(markup => markup.Value).ToArray<string>());

            /////////////////////////////////////////////////////////////////////////////
            /* Пробная зона */

          

            //////////////////////////////////////////////////////////////////////////////
        }//Form1_Load

        //Поиск по БД.
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            searchTextBox.AutoCompleteCustomSource.Clear();

            if (String.IsNullOrWhiteSpace(searchTextBox.Text)) return;
           // MessageBox.Show(searchTextBox.AutoCompleteCustomSource.Count.ToString());
            //if (searchTextBox.AutoCompleteCustomSource.Count >= 10) return;
            
            //В зависимости от значения checkBox, выводим либо товар только в наличии, либо весь товар в базе.
            if (onlyAvaliabilityCheckBox.CheckState == CheckState.Unchecked)
                searchSpList = PartsDAL.SearchSpByTitleOrArticulToDisplay(searchTextBox.Text, 10);
            else searchSpList = PartsDAL.SearchSpAvaliabilityByTitleOrArticulToDisplay(searchTextBox.Text, 10);

            ///*Выпадающий список в searchTextBox*/
            string articul, title;//, manuf;
            var strCol = new AutoCompleteStringCollection();
            for (int i = 0; i < searchSpList.Count; ++i)
            {
                title = String.Format(searchSpList[i].Title.Trim() + "   " + searchSpList[i].Articul.Trim() + "   " + searchSpList[i].Manufacturer);
                articul = String.Format(searchSpList[i].Articul.Trim() + "   " + searchSpList[i].Title.Trim() + "   " + searchSpList[i].Manufacturer);
                //manuf = String.Format(searchSpList[i].Manufacturer + " " + searchSpList[i].Title + " " + searchSpList[i].Articul);
                strCol.AddRange(new string[] { title, articul });
            }//for
            searchTextBox.AutoCompleteCustomSource = strCol;
        }//searchTextBox_TextChanged

        private void searchTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Если идет выбор с выпадающего списка.
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                searchTextBox.TextChanged -= searchTextBox_TextChanged;
                textChangeEvent = false;
                return;
            }//if
            //Если ввод условия поиска завершен.
            if (e.KeyCode == Keys.Enter)
            {                
                if (searchSpList.Count == 0) return;

                searchTextBox.TextChanged -= searchTextBox_TextChanged;
                textChangeEvent = false;
                string[] titleOrArticul = searchTextBox.Text.Split(new string[] {"   "}, StringSplitOptions.None);
                //если выбор не из вып. списка.
                if (titleOrArticul.Length == 1)
                {
                    if (onlyAvaliabilityCheckBox.Checked == false)
                        ChangeDataSource(PartsDAL.SearchSpByTitleOrArticulToDisplay(titleOrArticul[0]));
                    else ChangeDataSource(PartsDAL.SearchSpAvaliabilityByTitleOrArticulToDisplay(titleOrArticul[0]));
                    return;
                }
                //Если имеются точное совпадение в введенном тексте и коллекции эл-тов вып. списка.
                //if (searchTextBox.Text == searchTextBox.Text.TrimStart()) //возможная модификация.
                foreach (var sparePart in searchSpList)
                {
                    if ((sparePart.Articul.Trim() == titleOrArticul[0].Trim() && sparePart.Title.Trim() == titleOrArticul[1].Trim())
                        || (sparePart.Articul.Trim() == titleOrArticul[1].Trim() && sparePart.Title.Trim() == titleOrArticul[0].Trim()))
                    { 
                        //если точное совпадение найдено.
                        ChangeDataSource(new List<SparePart>() { sparePart });
                        return;
                    }//if 
                }//foreach
                //Если список вып. меню меньше максимума, значит вся подходящая инф-ция уже загружена. 
                //if (searchSpList.Count < 20)
                    //ChangeDataSource(searchSpList);
                //else 
                //{
                    //В зависимости от checkBox поиск ведется либо только по товару в наличии, либо по всему товару в базе.
                    var spareParts = onlyAvaliabilityCheckBox.Checked ? PartsDAL.SearchSpByTitleAndArticulToDisplay(titleOrArticul[0], titleOrArticul[1]) : PartsDAL.SearchSpAvaliabilityByTitleAndArticulToDisplay(titleOrArticul[0], titleOrArticul[1]);
                    if (spareParts.Count == 0)
                        spareParts = onlyAvaliabilityCheckBox.Checked ? PartsDAL.SearchSpByTitleAndArticulToDisplay(titleOrArticul[1], titleOrArticul[0]) : PartsDAL.SearchSpAvaliabilityByTitleAndArticulToDisplay(titleOrArticul[1], titleOrArticul[0]);
                    ChangeDataSource(spareParts);
                //}//else
                return;
            }//if
            //Продолжается ввод.
            if (textChangeEvent == false)
            {
                searchTextBox.TextChanged += searchTextBox_TextChanged;
                textChangeEvent = true;
            }
            //searchTextBox.Text.TrimStart();
        }//searchTextBox_PreviewKeyDown

        private void onlyAvaliabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            searchTextBox_TextChanged(sender, e);
        }//onlyAvaliabilityCheckBox_CheckedChanged

        //Работа с Excel.
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
        //private void saveInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    partsDataGridView.Cursor = Cursors.WaitCursor;
        //    progressBar.Value += progressBar.Step;

        //    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
        //    //Книга.
        //    ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    //Таблица.
        //    ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

        //    for (int i = 0; i < partsDataGridView.Rows.Count; i++)
        //    {
        //        if (i > partsDataGridView.Rows.Count / 2) progressBar.Value = progressBar.Maximum/2; //увеличиваем ProgressBar

        //        for (int j = 0; j < partsDataGridView.ColumnCount; j++)
        //        {
        //            if (i == 0) //если заголовок
        //            {                                                   
        //                ExcelApp.Cells[i + 1, j + 1] = partsDataGridView.Columns[j].HeaderText;
        //                //делаем шрифт заголовка жирным
        //                (ExcelWorkSheet.Cells[i + 1, j + 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true; 
        //            }//if

        //            ExcelApp.Cells[i + 2, j + 1] = partsDataGridView.Rows[i].Cells[j].Value;
        //        }//for
        //    }//for

        //    //Визуальное отображение работы.
        //    progressBar.Value = progressBar.Maximum;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        System.Threading.Thread.Sleep(1000);
        //        progressBar.Value = 0;
        //    });
        //    t.Start();
        //    partsDataGridView.Cursor = Cursors.Default;
        //    //Вызываем нашу созданную эксельку.
        //    ExcelApp.Visible = true;
        //    ExcelApp.UserControl = true;
        //}//saveInExcelToolStripMenuItem_Click

        //private void saveInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    partsDataGridView.Cursor = Cursors.WaitCursor;
        //    progressBar.Value += progressBar.Step;

        //    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
        //    //Книга.
        //    ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    //Таблица.
        //    ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

        //    bool isCol = false;
        //    //Поля которые не требуется распечатать. 
        //    string[] fields = new string[] { "Photo", "Id", "Price", "SellingPrice" };
        //    for (int i = 0; i < partsDataGridView.Rows.Count; i++)
        //    {
        //        if (i > partsDataGridView.Rows.Count / 2) progressBar.Value = progressBar.Maximum / 2; //увеличиваем ProgressBar

        //        for (int j = 0; j < partsDataGridView.ColumnCount; j++)
        //        {
        //            //Проверка нужный ли это столбец.
        //            foreach (var field in fields)                        
        //                if (partsDataGridView.Columns[j].Name == field) { isCol = true; break; }
        //            if (isCol == true) { isCol = false; continue; }

        //            if (i == 0) //если заголовок
        //            {
        //                ExcelApp.Cells[i + 1, j + 1] = partsDataGridView.Columns[j].HeaderText;
        //                //делаем шрифт заголовка жирным
        //                (ExcelWorkSheet.Cells[i + 1, j + 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
        //            }//if

        //            ExcelApp.Cells[i + 2, j + 1] = partsDataGridView.Rows[i].Cells[j].Value;                    
        //        }//for
        //    }//for

        //    //Визуальное отображение работы.
        //    progressBar.Value = progressBar.Maximum;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        System.Threading.Thread.Sleep(1000);
        //        progressBar.Value = 0;
        //    });
        //    t.Start();
        //    partsDataGridView.Cursor = Cursors.Default;
        //    //Вызываем нашу созданную эксельку.
        //    ExcelApp.Visible = true;
        //    ExcelApp.UserControl = true;
        //}//saveInExcelToolStripMenuItem_Click

        private void saveInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////Визуальное отображение работы.
            //partsDataGridView.Cursor = Cursors.WaitCursor;
            //progressBar.Value += progressBar.Step;

            //Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            ////Книга.
            //ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            ////Таблица.
            //ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            ////Microsoft.Office.Interop.Excel.Range rg = null;
            ////rg = ObjWorkSheet.get_Range("A1", "A1"); //Можно выбрать диапазон, например ("A1", "C1"), т.е. первый 3 столбца
            ////rg.ColumnWidth = yourWidth;

            ////_workSheet.get_Range(FirstRange, LastRange).EntireColumn.AutoFit(); //для столбца
            ////_workSheet.get_Range(FirstRange, LastRange).EntireRow.AutoFit();//для строки

            //bool isCol = false;
            ////Поля которые не требуется распечатать. 
            //string[] fields = new string[] { "Photo", "Id", "Price", "SellingPrice" };
            //for (int i = 0; i < partsDataGridView.Rows.Count; i++)
            //{
            //    if (i > partsDataGridView.Rows.Count / 2) progressBar.Value = progressBar.Maximum / 2; //увеличиваем ProgressBar

            //    for (int j = 0; j < partsDataGridView.ColumnCount; j++)
            //    {
            //        //Проверка нужный ли это столбец.
            //        foreach (var field in fields)
            //            if (partsDataGridView.Columns[j].Name == field) { isCol = true; break; }
            //        if (isCol == true)
            //        {
            //            isCol = false;
            //            (ExcelWorkSheet.Cells[i + 1, j + 1] as Microsoft.Office.Interop.Excel.Range).ColumnWidth = 0; //Убираем ненужные ячейки, задавая нулевую ширину.                      
            //            continue;
            //        }//if
            //        //если заголовок
            //        if (i == 0) 
            //        {
            //            ExcelApp.Cells[i + 1, j + 1] = partsDataGridView.Columns[j].HeaderText;
            //            (ExcelWorkSheet.Cells[i + 1, j + 1] as Microsoft.Office.Interop.Excel.Range).Font.Bold = true; //делаем шрифт заголовка жирным                    
            //        }//if                    
            //        (ExcelWorkSheet.Cells[i + 1, j + 1] as Microsoft.Office.Interop.Excel.Range).EntireColumn.AutoFit(); //Ширина колонки
            //        ExcelApp.Cells[i + 2, j + 1] = partsDataGridView.Rows[i].Cells[j].Value;
            //    }//for
            //}//for

            ////Визуальное отображение работы.
            //progressBar.Value = progressBar.Maximum;
            //System.Threading.Thread t = new System.Threading.Thread(() =>
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    progressBar.Value = 0;
            //});
            //t.Start();
            //partsDataGridView.Cursor = Cursors.Default;
            ////Вызываем нашу созданную эксельку.
            //ExcelApp.Visible = true;
            //ExcelApp.UserControl = true;
        }//saveInExcelToolStripMenuItem_Click

        /*Нумерация строк partsDataGridView*/
        private void partsDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridView.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridView.Rows[index].HeaderCell.Value = indexStr;
        }//partsDataGridView_RowPrePaint

        //Событие исп-ся для регулирования ширины RowHeaders.
        private void partsDataGridView_DataSourceChanged(object sender, EventArgs e)
        {           
            //скрываем столбцы не нужные пользователю.
            this.HidePartsDataGridViewColumns();

            //Обновляем rowsCountLabel по количеству строк. 
            rowsCountLabel.Text = partsDataGridView.Rows.Count.ToString();
            ////обнуление значения наценки, без вызова ValueChanged.
            //markupNumericUpDown.ValueChanged -= markupNumericUpDown_ValueChanged;
            //markupNumericUpDown.Value = 0;
            //markupNumericUpDown.ValueChanged += markupNumericUpDown_ValueChanged;

            //обработка размера RowHeaders.
            int i, count = partsDataGridView.Rows.Count;
            for (i = 0; count != 0; ++i)
            {
                count /= 10;
            }//for    
            partsDataGridView.RowHeadersWidth = 41 + ((i - 1) * 7); //41 - изначальный размер RowHeaders

            changeMarkupBufferDict.Clear(); //очищаем список деталей с измененной наценкой. 
            Deselection();
        }//partsDataGridView_DataSourceChanged
        //событие изменения DataSource в таблице доп. информации.
        private void extPartsDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (extPartsDataGridView.DataSource == null) return;
                HideExtPartsDataGridViewColumns(); 

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
        //События обработки изменения markupNumericUpDown     

        

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
            if (partsDataGridView.SelectedCells.Count == 0) return;

            saveChangesButton.Enabled = true; //сделать доступной кнопку "Сохранить изменения"
            cancelChangesButton.Enabled = true; //сделать доступной кнопку "Отменить изменения"
            //выделяем строки всех выделенных клеток.
            foreach (DataGridViewCell cell in partsDataGridView.SelectedCells)    cell.OwningRow.Selected = true;
            foreach (DataGridViewCell cell in extPartsDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            //узнаем процент заданной наценки.
            double markup = 0;
            try
            {
                markup = MarkupTypes.GetMarkupValue(markupComboBox.Text);
                //Если выделены только строки в partsDataGridView.
                if (extPartsDataGridView.SelectedRows.Count == 0)
                {
                    partsDataGridViewMarkupChange(markup);
                }//if
                //Если есть выделенные строки в extPartsDataGridView.
                else
                {
                    extPartsDataGridViewMarkupChange(markup);
                }//else
            }//try
            catch 
            { 
                toolTip.Show("Введены некорректные значения", this, markupComboBox.Location, 2000); 
            }//catch            
        }//markupComboBox_SelectedIndexChanged

        ///// <summary>
        ///// Возвращает выбранное поль-лем значение наценки. При вводе не числового значения выбрасывает ошибку.
        ///// </summary>
        ///// <returns></returns>
        //private double GetMarkupValue(string markupType)
        //{
        //    double markup = 0;
        //    //Проверяем выбранное или введенное значение наценки на наличие в базе.
        //    try
        //    {
        //        markup = PartsDAL.FindMarkupValue(markupType);
        //    }//try
        //    //Если значение введено вручную и не содержится в базе.    
        //    catch (InvalidOperationException)
        //    {
        //        //Проверяем является введенное поль-лем значение числом.
        //        markup = Convert.ToDouble(markupComboBox.Text);              
        //    }//catch

        //    return markup;
        //}//GetMarkupValue
        ///// <summary>
        ///// Возвращает тип наценки по заданному значению. 
        ///// </summary>
        ///// <param name="markup">Заданная наценка.</param>
        ///// <returns></returns>
        //private string GetMarkupType(double markup)
        //{
        //    string markupType = null;
        //    //Проверяем выбранное или введенное значение наценки на наличие в базе.
        //    try
        //    {
        //        markupType = PartsDAL.FindMarkupType(markup);
        //    }//try
        //    //Если значение введено вручную и не содержится в базе.    
        //    catch (InvalidOperationException)
        //    {
        //        if (markup > 0) 
        //            markupType = "Другая наценка";
        //        else if (markup < 0)
        //            markupType = "Уценка";
        //    }//catch

        //    return markupType;
        //}//GetMarkupType

        private void saveChangesButton_Click(object sender, EventArgs e)
        {                      
            //Визуальное выделение.
/*!!!*/     //Task.Factory.StartNew(() => { progressBar.Maximum / 2; }); //Не пополняется прогресс бар, возм-но нужно запустить в отд. потоке.
            progressBar.Value = progressBar.Maximum / 2;
            Cursor = Cursors.WaitCursor;

            try
            {
                PartsDAL.UpdateSparePartMarkup(changeMarkupBufferDict);
                //Действия осущ-мые при удачной записи в базу.
                saveChangesButton.Enabled = cancelChangesButton.Enabled = false; //делаем кнопки недоступными.
                progressBar.Value = progressBar.Maximum;
                //Перезаписываем начальный список.            
                origSpList = Cloner.Clone(SpList);
                origExtSpList = Cloner.Clone(ExtSpList);

                changeMarkupBufferDict.Clear(); //Очищаем словарь запчастей с измененной наценкой.
            }//try			
            catch (System.Data.SQLite.SQLiteException ex)
            {
                if (ex.Message == "database is locked\r\ndatabase is locked")
                    MessageBox.Show("Вероятно кто-то другой сейчас осуществляет запись в базу\nПопробуйте ещё раз.", "База данных занята в данный момент." );
                else MessageBox.Show(String.Format("Ошибка записи изменения наценки\n{0}", ex.Message));
            }//catch    

            
            progressBar.Value = 0;
            Cursor = Cursors.Default;
        }//saveChangesButton_Click

        private void cancelChangesButton_Click(object sender, EventArgs e)
        {
            saveChangesButton.Enabled = cancelChangesButton.Enabled = false; //делаем кнопку недоступной.
             
            //Отменяем все изменения.
            SpList = (List<SparePart>)Cloner.Clone(origSpList);
            ExtSpList = (List<SparePart>)Cloner.Clone(origExtSpList);

            partsDataGridView.DataSource = SpList;

            changeMarkupBufferDict.Clear(); //Очищаем словарь запчастей с измененной наценкой.

        }//cancelChangesButton_Click

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void partsDataGridViewMarkupChange(double markup)
        {
            //Модифицировать!!! Сделать изменение через DataGridView, без циклов.
            //Находим все SP с изменяемой наценкой. 
            foreach (DataGridViewRow row in partsDataGridView.SelectedRows)
            {
                if (row.Cells["Avaliability"].Value.ToString() == "0") continue;
                int sparePartId = Convert.ToInt32(row.Cells["SparePartId"].Value);
                //Находим запись в SpList с данным SparePartId.
                foreach (var sparePart in SpList)
                {
                    if (sparePart.SparePartId == sparePartId)
                    {
                        sparePart.Markup = markup;
                        break;
                    }
                }//foreach
                //Ищем все записи с нужным SparaPartId.
                foreach (var sparePart in ExtSpList)
                {
                    if (sparePart.SparePartId == sparePartId)
                    {
                        sparePart.Markup = markup;
                        sparePart.MarkupType = MarkupTypes.GetMarkupType(markup);//MarkupTypes.GetMarkupType(markup);
                        SaveMarkupChangeToBuffer(sparePart.SparePartId, sparePart.PurchaseId, markup);
                    }//if
                }//foreach

                partsDataGridView.InvalidateCell(row.Cells["SellingPrice"]);
            }//foreach   
            //Обновляем отображение столбцов в extPartsDataGridView.
            extPartsDataGridView.Invalidate();
        }//partsDataGridViewMarkupChange

        /// <summary>
        /// Осущ-ние действий вызванных изменением наценки.
        /// </summary>
        /// <param name="markup">Наценка на которую требуется изменить.</param>
        private void extPartsDataGridViewMarkupChange(double markup)
        {
            //IList<SparePart> spareParts = new List<SparePart>(); //список для всех запчастей с изменяемой наценкой.
            //Находим Id запчастей с изменяемой наценкой, т.к. SpId у всех вхождений одинаковый, берем его у первого вхождения.
            int sparePartId = Convert.ToInt32(extPartsDataGridView.SelectedRows[0].Cells["SparePartId"].Value);
            //Находим все SP с изменяемой наценкой. 
            foreach (DataGridViewRow row in extPartsDataGridView.SelectedRows)
            {
                int purchaseId = Convert.ToInt32(row.Cells["PurchaseId"].Value);
                //Ищем все записи с нужным SparaPartId.
                foreach (var sparePart in ExtSpList)
                {
                    if (sparePart.SparePartId == sparePartId && sparePart.PurchaseId == purchaseId)
                    {
                        sparePart.Markup = markup;
                        sparePart.MarkupType = MarkupTypes.GetMarkupType(markup);
                        SaveMarkupChangeToBuffer(sparePartId, purchaseId, markup);
                    }//if
                }//foreach                
            }//foreach   
            //Если одинаковя Наценка у всех SparePart с данным Id.
            SparePart sP = null;
            //Находим запись в SpList с данным SparePartId.
            foreach (var sparePart in SpList)
                if (sparePart.SparePartId == sparePartId)
                {
                    sP = sparePart;
                    break;
                }
            if (IsSameMarkup(FindSparePartsFromExtSpListBySparePartId(sparePartId)) == true)
                sP.Markup = markup;
            else sP.Markup = null;
            //Обновляем отображение столбцов в extPartsDataGridView.
            partsDataGridView.Invalidate();
            extPartsDataGridView.Invalidate();

        }//extPartsDataGridViewMarkupChange

        /// <summary>
        /// Метод сохраняющий в буфер изменения связанные с наценкой. 
        /// </summary>
        /// <param name="sparePartId">Id запчасти с изменяемой наценкой.</param>
        /// <param name="purchaseId">Id прихода с изменяемой наценкой.</param>
        /// <param name="markup">Наценка на которую нужно изменить старое значение.</param>
        private void SaveMarkupChangeToBuffer(int sparePartId, int purchaseId, double markup)
        {
            if (changeMarkupBufferDict.ContainsKey(sparePartId)) //Если уже есть такой SparePartId.
            {
                if (changeMarkupBufferDict[sparePartId].ContainsKey(purchaseId)) //если уже есть такой PurchaseId. 
                    (changeMarkupBufferDict[sparePartId])[purchaseId] = markup;
                else //если у данной SparePartId ещё нет такой PurchaseId.
                    (changeMarkupBufferDict[sparePartId]).Add(new KeyValuePair<int, double>(purchaseId, markup));
            }//if
            else //Если ещё нет данной SparePartId
            {
                IDictionary<int, double> dict = new Dictionary<int, double>();
                dict.Add(new KeyValuePair<int, double>(purchaseId, markup));
                changeMarkupBufferDict.Add(new KeyValuePair<int, IDictionary<int, double>>(sparePartId, dict));
            }//else
        }//SaveMarkupChangeToBuffer

        /// <summary>
        /// Проверяет одинаков ли Процент Наценки у всех эл-тов переданного списка запчастей. 
        /// </summary>
        /// <param name="spareParts">Список проверяемых запчастей</param>
        /// <returns></returns>
        private bool IsSameMarkup(IList<SparePart> spareParts)
        {
            //Проверяем не одинаковая ли у всех записей цена продажи и процент наценки.
            bool isSameMarkup = true;
            for (int i = 0; i < spareParts.Count - 1; ++i)
            {
                for (int j = i + 1; j < spareParts.Count; ++j)
                {
                    if (spareParts[i].Markup != spareParts[j].Markup) isSameMarkup = false;
                }//for j
                if (isSameMarkup == false) break;
            }//for i

            return isSameMarkup;
        }//IsSamePriceAndMarkup

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void excRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (partsDataGridView.SelectedCells.Count == 0) return;

            //выделяем строки всех выделенных клеток.
            foreach (DataGridViewCell cell in partsDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            foreach (DataGridViewCell cell in extPartsDataGridView.SelectedCells) cell.OwningRow.Selected = true;

            foreach (DataGridViewRow row in partsDataGridView.SelectedRows)
            {
                int sparePartId = Convert.ToInt32(row.Cells["SparePartId"].Value);
                //Ищем все записи с нужным SparaPartId.
                foreach (var sparePart in ExtSpList)
                {
                    if (sparePart.SparePartId == sparePartId)
                    {
                        sparePart.ExcRate = (double)excRateNumericUpDown.Value;
                    }//if
                }//foreach
                //Находим запись в SpList с данным SparePartId.
                foreach (var sparePart in SpList)
                    if (sparePart.SparePartId == sparePartId)
                    {
                        sparePart.ExcRate = (double)excRateNumericUpDown.Value;
                        break;
                    }
                partsDataGridView.InvalidateCell(row.Cells["SellingPrice"]);
            }//foreach   
            //Обновляем отображение столбцов в extPartsDataGridView.
            extPartsDataGridView.Invalidate();    

        }//excRateNumericUpDown_ValueChanged
        
        //Событие для отображения расширенной информации о Наличии запчасти.
        private void partsDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Если заголовок то ничего не делаем.
            if (e.RowIndex == partsDataGridView.Columns[0].HeaderCell.RowIndex) return;

            //Если ЛКМ
            if (e.Button == MouseButtons.Left)
            {
                excRateNumericUpDown.Enabled = true;
                markupComboBox.Enabled = true; //Делаем доступным функционал изменения наценки. 

                //Находим список всех приходов в наличии искомой запчасти.
                int sparePartId = Convert.ToInt32(partsDataGridView.Rows[e.RowIndex].Cells["SparePartId"].Value);
                extPartsDataGridView.DataSource = FindSparePartsFromExtSpListBySparePartId(sparePartId);
            }//if
            //Если ПКМ, выводим контекстное меню.
            else
            {
                //Очищаем все выделения в таблице, и выделяем выбранную только клетку.
                partsDataGridView.ClearSelection();
                partsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                //Находим позицию в таблице, где был сделан клик.
                Point cellLocation = partsDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                Point location = new Point(cellLocation.X + e.X, cellLocation.Y + e.Y);
                //Выводим контекстное меню.
                partsDGVContextMenuStrip.Show(partsDataGridView, location);
            }//else
        }//partsDataGridView_CellMouseClick

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
        //Событие для отображения Фотографии.
        private void partsDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Если клетка находится в колонке Photo и при этом не является заголовком.
                if (e.ColumnIndex == partsDataGridView.Columns["Photo"].Index && e.RowIndex != partsDataGridView.Columns["Photo"].HeaderCell.RowIndex)
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
        //Событие для окончания отображения Фотографии.
        private void partsDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            //Проверяем отображается ли сейчас photoPictureBox.
            if (photoPictureBox.Visible == true) photoPictureBox.Visible = false;
        }//partsDataGridView_CellMouseLeave

        /// <summary>
        /// Метод изменения источника данных для обоих dataGridView.
        /// </summary>
        /// <param name="spareParts">Новый источник данных для partsDataGridView.</param>
        private void ChangeDataSource(IList<SparePart> spareParts)
        {
            partsDataGridView.DataSource = SpList = Cloner.Clone(spareParts);
            origSpList = Cloner.Clone(spareParts);

            ExtSpList = PartsDAL.FindAvaliabilityBySparePartId(SpList);
            origExtSpList = PartsDAL.FindAvaliabilityBySparePartId(SpList); 
        }//ChangeDataSource
        //Метод для скрытия столбцов кот. не нужно видеть поль-лю.
        /// <summary>
        /// Скрывает заданные в методе столбцы таблицы PartsDataGridView.
        /// </summary>
        private void HidePartsDataGridViewColumns()
        {
            partsDataGridView.Columns["SparePartId"].Visible    = false;
            partsDataGridView.Columns["ExtInfoId"].Visible      = false;
            partsDataGridView.Columns["Count"].Visible          = false;
            partsDataGridView.Columns["virtCount"].Visible      = false;
            partsDataGridView.Columns["Price"].Visible          = false;
            //partsDataGridView.Columns["Markup"].Visible         = false;
            partsDataGridView.Columns["MarkupType"].Visible     = false;
            partsDataGridView.Columns["PurchaseId"].Visible     = false;
            partsDataGridView.Columns["SupplierName"].Visible   = false;
            partsDataGridView.Columns["ManufacturerId"].Visible = false;
            partsDataGridView.Columns["StorageAdress"].Visible  = false;

            //устанавливаем размеры столбцов.
            partsDataGridView.Columns["Photo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            partsDataGridView.Columns["Articul"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            partsDataGridView.Columns["Title"].MinimumWidth = 100;
            partsDataGridView.Columns["Description"].MinimumWidth = 100;
            partsDataGridView.Columns["Manufacturer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            partsDataGridView.Columns["Unit"].MinimumWidth = 35;
            partsDataGridView.Columns["Unit"].Width = 35;

            partsDataGridView.Columns["Avaliability"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            partsDataGridView.Columns["SellingPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }//HidePartsDataGridViewColumns
        /// <summary>
        /// Скрывает заданные в методе столбцы таблицы ExtPartsDataGridView.
        /// </summary>
        private void HideExtPartsDataGridViewColumns()
        {
            extPartsDataGridView.Columns["Photo"].Visible = false;
            extPartsDataGridView.Columns["SparePartId"].Visible = false;
            extPartsDataGridView.Columns["ExtInfoId"].Visible = false;
            extPartsDataGridView.Columns["Count"].Visible = false;
            extPartsDataGridView.Columns["VirtCount"].Visible = false;
            extPartsDataGridView.Columns["Price"].Visible = false;
            //extPartsDataGridView.Columns["Markup"].Visible = false;
            //partsDataGridView.Columns["MarkupType"].Visible = false;
            //extPartsDataGridView.Columns["PurchaseId"].Visible = false;
            //extPartsDataGridView.Columns["StorageAdress"].Visible = false;
            extPartsDataGridView.Columns["ManufacturerId"].Visible = false;
            extPartsDataGridView.Columns["Manufacturer"].Visible = false;
            extPartsDataGridView.Columns["Description"].Visible = false;
            extPartsDataGridView.Columns["PurchaseId"].Visible = false;

            //устанавливаем размеры столбцов.
            extPartsDataGridView.Columns["Photo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;           
            extPartsDataGridView.Columns["Articul"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            extPartsDataGridView.Columns["Title"].MinimumWidth = 100;

            extPartsDataGridView.Columns["Unit"].MinimumWidth = 35;
            extPartsDataGridView.Columns["Unit"].Width = 35;

            extPartsDataGridView.Columns["Avaliability"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            extPartsDataGridView.Columns["StorageAdress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            extPartsDataGridView.Columns["MarkupType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            extPartsDataGridView.Columns["SupplierName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            extPartsDataGridView.Columns["SellingPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }//HideExtPartsDataGridViewColumns

        //Метод для сброса выделений
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

            partsDataGridView.ClearSelection(); //Очищаем буфер выбранных эл-тов
            extPartsDataGridView.DataSource = null;
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
        /// <summary>
        /// Возвращает из списка ExtSpList список запчастей с заданным SparePartId.
        /// </summary>
        /// <param name="sparePartId">Id искомой запчасти</param>
        /// <returns></returns>
        private List<SparePart> FindSparePartsFromExtSpListBySparePartId(int sparePartId)
        {
            return (from sp in ExtSpList
                    where sp.SparePartId == sparePartId
                    select sp).ToList<SparePart>();
        }//FindSparePartsFromExtSpListBySparePartId


        #region Методы вызова дополнительных окон.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        private void addNewSpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddSparePartForm().Show(this);
        }//addNewSpToolStripMenuItem_Click

        private void addNewSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddContragentForm("поставщик").Show();
        }//addNewSupplierToolStripMenuItem_Click

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddContragentForm("клиент").Show();             
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
            new AddSparePartForm(Convert.ToInt32(partsDataGridView.SelectedCells[0].OwningRow.Cells["SparePartId"].Value)).Show();
        }













////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }//Form1

    public static class Cloner
    {
        public static object BinaryClone(object something, int size)
        {
            object obj;
            var serializer = new System.Xml.Serialization.XmlSerializer(something.GetType());
            Byte[] bytes = new Byte[size];
            using (var tempStream = new System.IO.MemoryStream(bytes))
            {
                serializer.Serialize(tempStream, something);
                tempStream.Seek(0, System.IO.SeekOrigin.Begin);
                obj = serializer.Deserialize(tempStream);
                tempStream.Close();
            }//using
            return obj;
        }//BinaryClone
        public static IList<SparePart> Clone(IList<SparePart> spareParts)
        {
            IList<SparePart> _spareParts = new List<SparePart>(spareParts.Count);

            for (int i = 0; i < spareParts.Count; ++i)
            {
                _spareParts.Add(new SparePart(spareParts[i]));
            }

            return _spareParts;
        }//Clone
    }//Cloner

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
//7)Добавить вывод и работу с полем Category.
//8)Добавить сортировку по многим параметрам.

/*Сам столкнулся с этим когда работают два процесса. Один создаёт буфер, второй пишет в него. 
 Ошибка возникает, когда первый процесс внезапно сменяет буфер, второй в это время "промахивается".*/


/*Рефакторинг*/
//1)saveChangesButton_Click