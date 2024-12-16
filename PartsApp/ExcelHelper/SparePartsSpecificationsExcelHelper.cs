using ClosedXML.Excel;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartsApp.ExcelHelper
{
    internal static class SparePartsSpecificationsExcelHelper
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
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Товары");

                //Настраиваем горизонтальные и вертикальные границы области печати.
                worksheet.PageSetup.Margins.Top = worksheet.PageSetup.Margins.Bottom = worksheet.PageSetup.Margins.Left = worksheet.PageSetup.Margins.Right = 7;

                //Заполняем таблицу.
                FillExcelList(worksheet, spareParts);

                string filePath = ExcelFilesStorageHelper.TempSparePartsListsFilesPath;
                ExcelFilesStorageHelper.SaveWorkbookToTempDirectoryAndOpenForPreview(workbook, filePath);
            }
        }

        /// <summary>
        /// Заполняем Excel инф-цией из переданного списка.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="sparePart">Список товаров.</param>
        private static void FillExcelList(IXLWorksheet worksheet, IList<SparePart> spareParts)
        {
            //Выводим заголовок.
            int row = 1, column = 1;
            FillTheTitlesRow(worksheet, row, column);

            //Устанавливаем ширину колонок.
            int titleColumnWidth = 35, articulColumnWidth = 20;
            SetColumnsWidth(spareParts, worksheet, column, titleColumnWidth, articulColumnWidth);

            //Заполняем таблицу списком товаров.
            foreach (SparePart sparePart in spareParts)
            {
                FillExcelRow(worksheet, sparePart, ++row, column);
            }

            //Обводим таблицу рамкой. 
            var range = worksheet.Range(row - spareParts.Count + 1, column, row, column + 5);
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.OutsideBorderColor = XLColor.Black;
        }

        /// <summary>
        /// Заполняет строку заголовками для таблицы.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillTheTitlesRow(IXLWorksheet worksheet, int row, int column)
        {
            //Заполняем заголовки строк.
            worksheet.Cell(row, column).Value = "Произв.";
            worksheet.Cell(row, column + 1).Value = "Артикул";
            worksheet.Cell(row, column + 2).Value = "Название";
            worksheet.Cell(row, column + 3).Value = "Ед. изм.";
            worksheet.Cell(row, column + 4).Value = "Кол-во";
            worksheet.Cell(row, column + 5).Value = "Цена";

            //Настраиваем вид клеток.
            var titleRange = worksheet.Range(row, column, row, column + 5);
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 12;
            //Обводим заголовки таблицы рамкой.
            titleRange.Style.Border.OutsideBorderColor = XLColor.Black;
            //Устанавливаем стиль и толщину линии
            titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
        }

        /// <summary>
        /// Заполнянет строку данными из переданного объекта.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="sparePart">Объект товара.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillExcelRow(IXLWorksheet worksheet, SparePart sparePart, int row, int column)
        {
            worksheet.Cell(row, column + 2).Value = sparePart.Title;
            worksheet.Cell(row, column + 1).Value = sparePart.Articul;

            //Выравнивание диапазона строк.
            var rowRange = worksheet.Range(row, column, row, column + 5);
            rowRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            rowRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rowRange.Style.Alignment.WrapText = true;

            worksheet.Cell(row, column).Value = sparePart.Manufacturer;
            worksheet.Cell(row, column + 3).Value = sparePart.MeasureUnit;
            worksheet.Cell(row, column + 4).Value = sparePart.AvailabilityList.Sum(av => av.OperationDetails.Count);
            if (sparePart.AvailabilityList.Count > 0)
            {
                worksheet.Cell(row, column + 5).Value = Availability.GetMaxSellingPrice(sparePart.AvailabilityList);
            }
        }

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="spareParts">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleColumnWidth">Столбец "Название".</param>
        /// <param name="articulColumnWidth">Столбец "Артикул".</param>
        private static void SetColumnsWidth(IList<SparePart> spareParts, IXLWorksheet worksheet, int column, int titleColumnWidth, int articulColumnWidth)
        {
            //Устанавливаем начальную ширину колонок
            int manufacturerColumnWidth = 15, minManufacturerColumnWidth = 8;

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufacturerLength = spareParts.Select(sp => sp.Manufacturer).Where(m => m != null).DefaultIfEmpty("").Max(m => m.Length);
            if (maxManufacturerLength < manufacturerColumnWidth)
            {
                int difference = manufacturerColumnWidth - maxManufacturerLength;
                titleColumnWidth += (manufacturerColumnWidth - difference < minManufacturerColumnWidth) ? minManufacturerColumnWidth : difference;
                manufacturerColumnWidth = (manufacturerColumnWidth - difference < minManufacturerColumnWidth) ? minManufacturerColumnWidth : manufacturerColumnWidth - difference;
            }

            worksheet.Column(column).Width = manufacturerColumnWidth;
            worksheet.Column(column + 1).Width = articulColumnWidth;
            worksheet.Column(column + 2).Width = titleColumnWidth;
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion
    }
}
