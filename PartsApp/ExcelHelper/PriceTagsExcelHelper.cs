using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PartsApp.ExcelHelper
{
    internal static class PriceTagsExcelHelper
    {
        #region Вывод в Excel товара из таблицы.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        internal static async void SaveInExcelAsync(IList<SparePart> spareParts)
        {
            try
            {
                await Task.Factory.StartNew(() => SaveInExcel(spareParts));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }

        /// <summary>
        /// Вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        private static void SaveInExcel(IList<SparePart> spareParts)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);   //Таблица.

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;

            //Заполняем таблицу.
            FillExcelList(ExcelWorkSheet, spareParts);

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = ExcelApp.UserControl = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.                 
        }

        /// <summary>
        /// Заполняем Excel инф-цией из переданного списка.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="sparePart">Список товаров.</param>
        private static void FillExcelList(Excel.Worksheet ExcelWorkSheet, IList<SparePart> spareParts)
        {
            //Выводим заголовок.
            int row = 1, column = 1;
            FillTheTitlesRow(ExcelWorkSheet, row, column);

            //Устанавливаем ширину колонок.
            int titleColWidth = 35, articulColWidth = 20;
            SetColumnsWidth(spareParts, ExcelWorkSheet.Cells[row, column + 2], ExcelWorkSheet.Cells[row, column + 1], ExcelWorkSheet.Cells[row, column], titleColWidth, articulColWidth);

            //Заполняем таблицу списком товаров.
            foreach (SparePart sparePart in spareParts)
            {
                FillExcelRow(ExcelWorkSheet, sparePart, ++row, column, titleColWidth, articulColWidth);
            }

            //Обводим талицу рамкой. 
            ExcelWorkSheet.get_Range("A" + (row - spareParts.Count + 1).ToString(), "F" + row.ToString()).Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
        }

        /// <summary>
        /// Заполняет строку заголовками для таблицы.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillTheTitlesRow(Excel.Worksheet ExcelWorkSheet, int row, int column)
        {
            //Заполняем заголовки строк.
            ExcelWorkSheet.Cells[row, column] = "Произв.";
            ExcelWorkSheet.Cells[row, column + 1] = "Артикул";
            ExcelWorkSheet.Cells[row, column + 2] = "Название";
            ExcelWorkSheet.Cells[row, column + 3] = "Ед. изм.";
            ExcelWorkSheet.Cells[row, column + 4] = "Кол-во";
            ExcelWorkSheet.Cells[row, column + 5] = "Цена";

            //Настраиваем вид клеток.
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString());
            excelCells.Font.Bold = true;
            excelCells.Font.Size = 12;
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack; //Обводим заголовки таблицы рамкой.
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium; //Устанавливаем стиль и толщину линии
        }

        /// <summary>
        /// Заполянет строку данными из переданного объекта.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="sparePart">Объект товара.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        /// <param name="titleColWidth">ширина столбца 'Название'.</param>
        /// <param name="articulColWidth">ширина столбца 'Артикул'.</param>
        private static void FillExcelRow(Excel.Worksheet ExcelWorkSheet, SparePart sparePart, int row, int column, int titleColWidth, int articulColWidth)
        {
            ExcelWorkSheet.Cells[row, column + 2] = sparePart.Title;
            ExcelWorkSheet.Cells[row, column + 1] = sparePart.Articul;
            //Выравнивание диапазона строк.
            ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
            ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
            if (sparePart.Articul.Length > articulColWidth || sparePart.Title.Length > titleColWidth)
            {
                IncreaseRowHeight(ExcelWorkSheet, sparePart, row, column, titleColWidth, articulColWidth);
            }

            ExcelWorkSheet.Cells[row, column] = sparePart.Manufacturer;
            ExcelWorkSheet.Cells[row, column + 3] = sparePart.MeasureUnit;
            ExcelWorkSheet.Cells[row, column + 4] = sparePart.AvailabilityList.Sum(av => av.OperationDetails.Count);
            if (sparePart.AvailabilityList.Count > 0)
            {
                ExcelWorkSheet.Cells[row, column + 5] = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);
            }
        }


        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleCol">Столбец "Название".</param>
        /// <param name="articulCol">Столбец "Артикул".</param>
        /// <param name="manufCol">Столбец "Производитель".</param>
        private static void SetColumnsWidth(IList<SparePart> spareParts, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol, int titleColWidth, int articulColWidth)
        {
            //Устанавливаем ширину первой Колонок
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = spareParts.Select(sp => sp.Manufacturer).Where(man => man != null);
            if (sparePartsManufacturers.Count() > 0)
            {
                maxManufLenght = sparePartsManufacturers.Max(man => man.Length);
            }

            if (maxManufLenght < manufColWidth)
            {
                int different = manufColWidth - maxManufLenght; //разница между дефолтной шириной столбца и фактической.
                titleColWidth += (manufColWidth - different < minManufColWidth) ? minManufColWidth : different;
                manufColWidth = (manufColWidth - different < minManufColWidth) ? minManufColWidth : manufColWidth - different;
            }
            manufCol.Columns.ColumnWidth = manufColWidth;
            articulCol.Columns.ColumnWidth = articulColWidth;
            titleCol.Columns.ColumnWidth = titleColWidth;
        }

        /// <summary>
        /// Увеличивает ширину строки.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="sparePart">Объкт товара.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        /// <param name="titleColWidth">Ширина столбца для Названия товара.</param>
        /// <param name="articulColWidth">Ширина столбца для Артикула товара.</param>
        private static void IncreaseRowHeight(Excel.Worksheet ExcelWorkSheet, SparePart sparePart, int row, int column, int titleColWidth, int articulColWidth)
        {
            ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
            if (sparePart.Articul.Length > articulColWidth && sparePart.Title.Length <= titleColWidth)
            {
                ExcelWorkSheet.Cells[row, column + 2].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            }
            if (sparePart.Articul.Length <= articulColWidth && sparePart.Title.Length > titleColWidth)
            {
                ExcelWorkSheet.Cells[row, column + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            }
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Вывод ценников.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции для распечатки ценников.
        /// </summary>
        /// <param name="sparePartsList">Список товаров для вывода в Excel.</param>
        internal static async void ExcelSaveSparePartPriceListAsync(IList<SparePart> sparePartsList)
        {
            try
            {
                await Task.Factory.StartNew(() => ExcelSaveSparePartPriceList(sparePartsList));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }

        /// <summary>
        /// Вывод в Excel инф-ции для распечатки ценников.
        /// </summary>
        /// <param name="sparePartsList">Список товаров для вывода в Excel.</param>
        private static void ExcelSaveSparePartPriceList(IList<SparePart> sparePartsList)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); ;
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

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
                {
                    row = FillExcelSheetPriceList(sparePartsList[i], row, 3, ExcelWorkSheet);
                }

                row += 2;
            }
            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = ExcelApp.UserControl = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
        }

        private static int FillExcelSheetPriceList(SparePart sparePart, int startRow, int column, Excel.Worksheet ExcelWorkSheet)
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
            {
                ExcelWorkSheet.Cells[row, column].HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            }

            //Выводим Розничную цену.
            row += 2;
            if (sparePart.AvailabilityList.Count > 0)
            {
                ExcelWorkSheet.Cells[row, column] = String.Format("{0:0.00} руб", Availability.GetMaxSellingPrice(sparePart.AvailabilityList));
            }
            Excel.Range excelCells = ExcelWorkSheet.get_Range(columnChar + row.ToString());
            excelCells.Font.Size = 24;
            //Выравниваем по центру.
            ExcelWorkSheet.Cells[row, column].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            //Обводим рамкой. 
            ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString()).Font.Bold = true;
            excelCells = ExcelWorkSheet.get_Range(columnChar + startRow.ToString(), columnChar + row.ToString());
            excelCells.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlRgbColor.rgbBlack);

            return row;
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion
    }
}
