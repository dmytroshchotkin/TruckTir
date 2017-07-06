using PartsApp.Models;
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

namespace PartsApp
{
    public partial class OperationsInfoForm : Form
    {
        public OperationsInfoForm()
        {
            InitializeComponent();
        }

        private void OperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Устанавливаем стартовый период в месяц. (Убираем а затем добавляем событие ValueChanged на BeginDateDTP, для того чтобы метод DatesDTP_ValueChanged не вызвался дважды.)
            BeginDateDTP.ValueChanged -= new System.EventHandler(this.DatesDTP_ValueChanged);
            BeginDateDTP.Value = DateTime.Today.AddMonths(-1);
            EndDateDTP.Value = DateTime.Now;
            BeginDateDTP.ValueChanged += new System.EventHandler(this.DatesDTP_ValueChanged);
        }//

        /// <summary>
        /// Выводим список операций соответствующих установленным требованиям по дате, и типу операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesDTP_ValueChanged(object sender, EventArgs e)
        {
            FillTheOperationDGV(); //Заполняем таблицу операций.
        }//DatesDTP_ValueChanged


        /// <summary>
        /// Заполняем таблицу операций для выделенного сотрудника.
        /// </summary>
        private void FillTheOperationDGV()
        {
            OperationsInfoDGV.Rows.Clear(); //Очищаем список операций.

            //Находим начальную и конечную дату требуемых операций.
            DateTime? beginDate = BeginDateDTP.Enabled ? BeginDateDTP.Value : (DateTime?)null;
            DateTime? endDate = EndDateDTP.Enabled ? EndDateDTP.Value : (DateTime?)null;
            //Выводим список операций соответствующий заданным требованиям.
            List<IOperation> operList = PartsDAL.FindOperations(beginDate, endDate);
            FillTheOperationDGV(operList);

            //Изменяем видимость строк по типу операции.
            OperationsCheckBox_CheckedChanged(null, null);
        }//FillTheOperationDGV

        /// <summary>
        /// Заполняет таблицу операций переданной инф-цией.
        /// </summary>
        /// <param name="operList">Инф-ция для заполнения таблицы.</param>
        private void FillTheOperationDGV(IList<IOperation> operList)
        {
            foreach (IOperation operat in operList.OrderByDescending(p => p.OperationDate))
            {
                int rowIndx = OperationsInfoDGV.Rows.Add();
                DataGridViewRow row = OperationsInfoDGV.Rows[rowIndx];

                row.Cells[OperationTypeCol.Index].Value = (operat.GetType() == typeof(Sale)) ? "Расход" : "Приход";
                row.DefaultCellStyle.BackColor = (operat.GetType() == typeof(Sale)) ? Color.LightGreen : Color.Khaki;//Color.Pink;
                row.Cells[OperationIdCol.Index].Value = operat.OperationId;
                row.Cells[DateCol.Index].Value = operat.OperationDate.ToShortDateString();
                row.Cells[EmployeeCol.Index].Value = (operat.Employee != null) ? operat.Employee.GetShortFullName() : null;
                row.Cells[ContragentCol.Index].Value = operat.Contragent.ContragentName;
                row.Cells[ContragentEmployeeCol.Index].Value = operat.ContragentEmployee;
                row.Cells[TotalSumCol.Index].Value = operat.OperationDetailsList.Sum(od => od.Sum);

                row.Tag = operat;
            }//foreach
        }//FillTheOperationDGV

        /// <summary>
        /// Изменяет видимость строк по типу операции, в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Меняем видимость требуемых строк в зависимотси от установленных требований для данного типа операций.
            foreach (DataGridViewRow row in OperationsInfoDGV.Rows)
            {
                row.Visible = (row.Cells[OperationTypeCol.Index].Value == "Приход" ? PurchaseCheckBox.Checked : SaleCheckBox.Checked);
            }//foreach

            //Выводим кол-во видимых строк.
            OperationsCoubtLabel.Text = OperationsInfoDGV.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
        }//OperationsCheckBox_CheckedChanged    

        /// <summary>
        /// Осуществляет изменения данных в таблице деталей операции в зависимости от выбранной операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsInfoDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            OperationDetailsDGV.Rows.Clear(); //Очищаем таблицу доп. инф-ции от старых данных.

            //Если есть выбранная строка.
            if (OperationsInfoDGV.SelectedRows.Count != 0)
            {
                IOperation oper = (OperationsInfoDGV.Rows[e.RowIndex].Tag as IOperation);//Находим нужную операцию
                //Выводим инф-цию в таблицу доп. инф-ции по данной операции.
                FillTheOperationDetailsDGV(oper.OperationDetailsList);
            }//if
        }//OperationsInfoDGV_RowEnter

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
            }//foreach
        }//FillTheOperationDetailsDGV

        /// <summary>
        /// Изменяем доступность DTP в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Находим нужный DGV
            DateTimePicker dtp = (sender as CheckBox).Name == this.BeginDateCheckBox.Name ? BeginDateDTP : EndDateDTP;
            dtp.Enabled = !dtp.Enabled;

            //Заполняем таблицу операций.
            FillTheOperationDGV();
        }//BeginDateCheckBox_CheckedChanged

        #region **************************************Вывод в Excel**********************************************************************
        //==============================================================================================================================================================================

        /// <summary>
        /// Выводит в Excel выделенную накладную. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExcelOutputButton_Click(object sender, EventArgs e)
        {
            //Если выделена строка приходной накладной, то формируем в Excel приходную накладную, иначе расходную накладную.
            DataGridViewRow row = OperationsInfoDGV.SelectedRows[0];
            if (row.Cells[OperationTypeCol.Name].Value.ToString() == "Приход")
            {
                Purchase purchase = row.Tag as Purchase;
                saveInExcelAsync(purchase.OperationDetailsList, "Truck Tir");
            }//if
            else
            {
                Sale sale = row.Tag as Sale;
                saveInExcelAsync(sale, "Truck Tir");
            }//else

        }//ExcelOutputButton_Click

        #region *************************************************Вывод приходов********************************************************
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------


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

            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;

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
                                                         "Принял : " + Form1.CurEmployee.LastName + " " + Form1.CurEmployee.FirstName);
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
            excelCells.Value = String.Format("Приходная накладная №{0} от {1}г.", purchase.OperationId, purchase.OperationDate.ToString("dd/MM/yyyy"));
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



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region *************************************************Вывод расходов********************************************************
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        /// <param name="agent">Фирма-продавец.</param>
        private async void saveInExcelAsync(Sale sale, string agent)
        {
            try
            {
                await Task.Factory.StartNew(() => saveInExcel(sale, agent));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }//saveInExcelAsync   

        /// <summary>
        /// Метод вывода расходной информации в Excel-файл.
        /// </summary>
        /// <param name="purchase">Информация о расходе.</param>
        /// <param name="agent">Фирма-продавец.</param>
        private void saveInExcel(Sale sale, string agent)
        {
            IList<OperationDetails> operDetList = sale.OperationDetailsList;

            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;

            int row = 1, column = 1;
            //Выводим Id и Дату. 
            OperationIdAndDateExcelOutput(ExcelWorkSheet, sale, row, column);

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         "Продавец : " + agent,
                                                         "Покупатель : " + sale.Contragent.ContragentName);

            //Заполняем таблицу.
            FillTheExcelList(ExcelWorkSheet, operDetList, ref row, column);

            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         "Выписал : " + Form1.CurEmployee.LastName + " " + Form1.CurEmployee.FirstName,
                                                         "Принял : " + sale.ContragentEmployee);


            row += 2;
            //Выводим заметку к операции.
            DescriptionExcelOutput(ExcelWorkSheet, sale.Description, ref row, column);

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = ExcelApp.UserControl = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
        }//saveInExcel  


        /// <summary>
        /// Заполняет заданную строку Id операции и датой.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="purchase">Объект операции.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private void OperationIdAndDateExcelOutput(Excel.Worksheet ExcelWorkSheet, Sale sale, int row, int column)
        {
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Расходная накладная №{0} от {1}г.", sale.OperationId, sale.OperationDate.ToString("dd/MM/yyyy"));
        }//OperationIdAndDateExcelOutput








        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion




        //==============================================================================================================================================================================
        #endregion

        
    }//OperationsInfoForm
}//namespace
