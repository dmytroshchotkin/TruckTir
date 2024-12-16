using ClosedXML.Excel;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartsApp.ExcelHelper
{
    internal static class PriceTagsExcelHelper
    {      
        #region Вывод ценников.
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Асинхронный вывод в Excel инф-ции для распечатки ценников.
        /// </summary>
        /// <param name="sparePartsList">Список товаров для вывода в Excel.</param>
        internal static async void SaveInExcelAsync(IList<SparePart> sparePartsList)
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
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Товары");

                //Настраиваем горизонтальные и вертикальные границы области печати.
                worksheet.PageSetup.Margins.Top = worksheet.PageSetup.Margins.Bottom = worksheet.PageSetup.Margins.Left = worksheet.PageSetup.Margins.Right = 7;
                
                //задаем ширину второго столбца, для того чтобы корректно выделять рамкой ценники.
                worksheet.Column("B").Width = 1;

                //Заполняем Excel-файл, по 2 записи на строку.
                int row = 1;
                for (int i = 0; i < sparePartsList.Count; ++i)
                {
                    FillExcelSheetPriceList(sparePartsList[i], row, 1, worksheet);
                    if (++i < sparePartsList.Count)
                    {
                        row = FillExcelSheetPriceList(sparePartsList[i], row, 3, worksheet);
                    }

                    row += 2;
                }

                string filePath = ExcelFilesStorageHelper.TempPriceTagsFilesPath;
                ExcelFilesStorageHelper.SaveWorkbookToTempDirectoryAndOpenForPreview(workbook, filePath);
            }
        }

        private static int FillExcelSheetPriceList(SparePart sparePart, int startRow, int column, IXLWorksheet worksheet)
        {
            int row = startRow;
            int columnWidth = 50;

            //задаём ширину столбца.
            worksheet.Column(column).Width = columnWidth;
            //Выводим Артикул.
            worksheet.Cell(row, column).Value = sparePart.Articul;
            row += 2;
            //Выводим Название.
            worksheet.Cell(row, column).Value = sparePart.Title;
            var titleRange = worksheet.Range(row - 2, column, row, column);
            titleRange.Style.Font.FontSize = 12;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //Если не влазиет в строку, делаем перенос и центрируем по вертикали.
            if (sparePart.Title.Length > columnWidth - 5)
            {
                worksheet.Cell(row, column).Style.Alignment.WrapText = true;
            }

            row += 2;

            if (sparePart.AvailabilityList.Count > 0)
            {
                worksheet.Cell(row, column).Value = $"{Availability.GetMaxSellingPrice(sparePart.AvailabilityList):0.00} руб";
            }

            //Выводим Розничную цену.
            var priceCell = worksheet.Cell(row, column);
            priceCell.Style.Font.FontSize = 24;
            priceCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //Обводим рамкой. 
            var fullRange = worksheet.Range(startRow, column, row, column);
            fullRange.Style.Font.Bold = true;
            fullRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.OutsideBorderColor = XLColor.Black;

            return row;
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion
    }
}
