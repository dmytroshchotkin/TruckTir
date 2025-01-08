using Microsoft.Office.Interop.Excel;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace PartsApp.ExcelHelper
{
    public static class OperationsExcelHelper
    {
        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        /// <param name="agent">Фирма-покупатель.</param>
        internal static async void SaveInExcelAsync(IList<OperationDetails> operDetList, string agent, string directory, bool printPreview)
        {
            try
            {
                await Task.Factory.StartNew(() => SaveInExcel(operDetList, agent, directory, printPreview));
            }
            catch
            {
                MessageBox.Show("Ошибка вывода в Excel");
            }
        }

        /// <summary>
        /// Метод вывода приходной информации в Excel-файл.
        /// </summary>
        /// <param name="availabilityList">Список оприходованных товаров.</param>
        /// <param name="agent">Фирма-покупатель.</param>
        private static void SaveInExcel(IList<OperationDetails> operDetList, string agent, string directory, bool saveSingleDocWithPreview)
        {
            var operation = operDetList[0].Operation;

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add();
                worksheet.PageSetup.PageOrientation = ClosedXML.Excel.XLPageOrientation.Portrait;
                worksheet.PageSetup.AdjustTo(100);
                worksheet.PageSetup.PagesWide = 1; // ширина, равная границе печати

                //Настраиваем горизонтальные и вертикальные границы области печати.
                worksheet.PageSetup.Margins.Top = worksheet.PageSetup.Margins.Bottom = worksheet.PageSetup.Margins.Left = worksheet.PageSetup.Margins.Right = 7;

                int row = 1, column = 1;

                //Выводим Id и Дату.
                OperationIdAndDateExcelOutput(worksheet, operation, ref row);

                //Выводим поставщика и покупателя / продавца и покупателя после отступа строки
                row += 2;
                worksheet.Cell(row, column).Style.Font.FontName = "Consolas";
                worksheet.Cell(row, column).Value = operation is Purchase ? GetPurchaseAgentsDescriptionForDocHeader(operation, agent) : GetSaleAgentsDescriptionForDocHeader(operation, agent);

                //Заполняем таблицу.
                FillTheExcelList(worksheet, operDetList, ref row, column);

                // Выводим имена агентов.
                row += 2;
                worksheet.Cell(row, column).Style.Font.FontName = "Consolas";
                worksheet.Cell(row, column).Value = operation is Purchase ? GetPurchaseAgentsDescriptionForDocVisas(operation) : GetSaleAgentsDescriptionForDocVisas(operation);

                //Выводим заметку к операции.
                row += 2;

                DescriptionExcelOutput(worksheet, operation.Description, ref row, column);

                var filePath = Path.Combine(directory, GetOperationTitle(operation));
                filePath = Path.ChangeExtension(filePath, ExcelFilesStorageHelper.ExcelFilesExtension);

                ExcelFilesStorageHelper.SaveExcelFile(workbook, filePath, saveSingleDocWithPreview);
            }
        }

        private static string GetPurchaseAgentsDescriptionForDocHeader(IOperation purchase, string agent)
        {
            return String.Format("\t\t{0,-50}{1}",
                                 "Поставщик : " + purchase.Contragent.ContragentName,
                                 "Покупатель : " + agent);
        }

        private static string GetSaleAgentsDescriptionForDocHeader(IOperation sale, string agent)
        {
            return String.Format("\t\t{0,-40}{1}",
                                 "Продавец : " + agent,
                                 "Покупатель : " + sale.Contragent.ContragentName);
        }

        private static string GetPurchaseAgentsDescriptionForDocVisas(IOperation purchase)
        {
            return String.Format("\t\t{0,-50}{1}",
                                 "Выписал : " + purchase.ContragentEmployee,
                                 "Принял : " + Form1.CurEmployee.LastName + " " + Form1.CurEmployee.FirstName);
        }

        private static string GetSaleAgentsDescriptionForDocVisas(IOperation sale)
        {
            return String.Format("\t\t{0,-40}{1}",
                                 "Выписал : " + Form1.CurEmployee.LastName + " " + Form1.CurEmployee.FirstName,
                                 "Принял : " + sale.ContragentEmployee);
        }

        /// <summary>
        /// Заполняем Excel инф-цией из переданного списка.
        /// </summary>
        /// <param name="worksheet">Рабочая страница</param>
        /// <param name="operDetList">Список деталей операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillTheExcelList(IXLWorksheet worksheet, IList<OperationDetails> operDetList, ref int row, int column)
        {
            row += 2;

            //Выводим заголовок.
            FillTheTitlesRow(worksheet, row, column);

            //Устанавливаем ширину столбцов.
            worksheet.Column(column + 4).Width = 5;
            SetColumnsWidth(operDetList, worksheet.Column(column + 3), worksheet.Column(column + 2), worksheet.Column(column));

            int titleColWidth = 30, articulColWidth = 20;
            float inTotal = 0;

            //Выводим список товаров.
            foreach (var operDet in operDetList)
            {
                FillExcelRow(worksheet, operDet, ++row, column, titleColWidth, articulColWidth);
                inTotal += operDet.Price * operDet.Count;
            }

            //Обводим таблицу рамкой. 
            worksheet.Range($"A{row - operDetList.Count + 1}:H{row}").Style.Border.SetOutsideBorder(ClosedXML.Excel.XLBorderStyleValues.Thin).Border.SetInsideBorder(ClosedXML.Excel.XLBorderStyleValues.Thin);

            ++row;

            //Выводим 'Итого'.
            InTotalExcelOutput(worksheet, inTotal, row, column);
        }

        /// <summary>
        /// Заполняет строку заголовками для таблицы.
        /// </summary>
        /// <param name="worksheet">Рабочий лист.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillTheTitlesRow(IXLWorksheet worksheet, int row, int column)
        {
            //Заполняем заголовки строк.
            worksheet.Cell(row, column).Value = "Произв.";
            worksheet.Cell(row, column + 1).Value = "Склад";
            worksheet.Cell(row, column + 2).Value = "Артикул";
            worksheet.Cell(row, column + 3).Value = "Название";
            worksheet.Cell(row, column + 4).Value = "Ед. изм.";
            worksheet.Cell(row, column + 5).Value = "Кол-во";
            worksheet.Cell(row, column + 6).Value = "Цена";
            worksheet.Cell(row, column + 7).Value = "Сумма";

            //Настраиваем вид ячеек заголовков
            var headerRange = worksheet.Range($"A{row}:H{row}");
            headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
            headerRange.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
            headerRange.Style.Alignment.WrapText = true;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontSize = 12;
            headerRange.Style.Border.OutsideBorder = headerRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Medium; //Обводим заголовки таблицы рамкой.
        }

        /// <summary>
        /// Заполянет строку данными из переданного объекта.
        /// </summary>
        /// <param name="worksheet">Рабочая страница</param>
        /// <param name="sparePart">Объект товара.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        /// <param name="titleColWidth">ширина столбца 'Название'.</param>
        /// <param name="articulColWidth">ширина столбца 'Артикул'.</param>
        private static void FillExcelRow(IXLWorksheet worksheet, OperationDetails operDet, int row, int column, int titleColWidth, int articulColWidth)
        {
            SetStringExcelNumberFormatForArticulAndStorageCell(worksheet, row, column);
            // Устанавливаем перенос по словам для всей строки
            worksheet.Row(row).Style.Alignment.WrapText = true;

            worksheet.Cell(row, column).Value = operDet.SparePart.Manufacturer;
            worksheet.Cell(row, column + 1).Value = operDet.SparePart.StorageCell;
            worksheet.Cell(row, column + 2).Value = operDet.SparePart.Articul;
            worksheet.Cell(row, column + 3).Value = operDet.SparePart.Title;
            worksheet.Cell(row, column + 4).Value = operDet.SparePart.MeasureUnit;
            worksheet.Cell(row, column + 5).Value = operDet.Count;
            worksheet.Cell(row, column + 6).Value = operDet.Price;
            worksheet.Cell(row, column + 7).Value = operDet.Price * operDet.Count;

            //Выравнивание диапазона строк.
            var range = worksheet.Range($"A{row}:H{row}");
            range.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
            range.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
        }

        private static void SetStringExcelNumberFormatForArticulAndStorageCell(IXLWorksheet worksheet, int row, int column)
        {
            //worksheet.Cell(row, column + 1).DataType = worksheet.Cell(row, column + 2).DataType = ClosedXML.Excel.XLDataType.Text; 
        }

        /// <summary>
        /// Выводим 'Итого' в заданной клетке.
        /// </summary>
        /// <param name="worksheet">Рабочий лист.</param>
        /// <param name="inTotal">Общая сумма операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void InTotalExcelOutput(IXLWorksheet worksheet, float inTotal, int row, int column)
        {
            //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            int indent = 0; //отступ
            if (inTotal.ToString("0.00").Length <= 9)
            {
                indent = 1;
            }

            worksheet.Cell(row, column + 5 + indent).Value = "Итого : ";
            worksheet.Cell(row, column + 5 + indent).Style.Font.Bold = true;
            worksheet.Cell(row, column + 5 + indent).Style.Font.FontSize = 12;

            worksheet.Cell(row, column + 6 + indent).Value = inTotal.ToString("0.00");
            worksheet.Cell(row, column + 6 + indent).Style.Font.Bold = true;
            worksheet.Cell(row, column + 6 + indent).Style.Font.FontSize = 12;
            worksheet.Cell(row, column + 6 + indent).Style.Font.Underline = XLFontUnderlineValues.Single;

            worksheet.Range(row, column + 5 + indent, row, column + 6 + indent).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        /// <summary>
        /// Заполняет заданную строку Id операции и датой.
        /// </summary>
        /// <param name="worksheet">Рабочий лист</param>
        /// <param name="operation">Объект операции.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private static void OperationIdAndDateExcelOutput(IXLWorksheet worksheet, IOperation operation, ref int row)
        {
            string titlePattern = operation is Purchase ? "Приходная накладная №{0} от {1}г." : "Расходная накладная №{0} от {1}г.";

            var titleRange = worksheet.Range(row, 1, row, 8);
            titleRange.Merge();

            titleRange.Value = string.Format(titlePattern, operation.OperationId, operation.OperationDate.ToString("dd/MM/yyyy"));
            titleRange.Style.Font.FontSize = 18;
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.Underline = XLFontUnderlineValues.Single;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        /// <summary>
        /// Выводит заметку об операции.
        /// </summary>
        /// <param name="worksheet">Рабочий лист</param>
        /// <param name="description">заметка</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private static void DescriptionExcelOutput(IXLWorksheet worksheet, string description, ref int row, int column)
        {
            if (description != null)
            {
                //Делаем визуальное отделение информации от заметки с помощью пустой строки.
                worksheet.Cell(row, column).Value = new string(' ', 200);
                worksheet.Cell(row, column).Style.Font.Underline = XLFontUnderlineValues.Single;
                row++;

                // объединяем ячейки, присваиваем значение description объединённой ячейке и настраиваем перенос
                var descriptionRange = worksheet.Range(row, column, row, column + 7);
                descriptionRange.Merge();
                descriptionRange.Value = description;
                descriptionRange.Style.Alignment.WrapText = true;

                worksheet.Row(row).AdjustToContents();
            }
        }

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleColumn">Столбец "Название".</param>
        /// <param name="articulColumn">Столбец "Артикул".</param>
        /// <param name="manufacturerColumn">Столбец "Производитель".</param>
        private static void SetColumnsWidth(IList<OperationDetails> operDetList, IXLColumn titleColumn, IXLColumn articulColumn, IXLColumn manufacturerColumn)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufacturerColumnLength = 0;
            var sparePartsManufacturers = operDetList.Select(od => od.SparePart.Manufacturer).Where(m => m != null);
            if (sparePartsManufacturers.Count() > 0)
            {
                maxManufacturerColumnLength = sparePartsManufacturers.Max(m => m.Length);
            }

            if (maxManufacturerColumnLength < manufColWidth)
            {
                int different = manufColWidth - maxManufacturerColumnLength; //разница между дефолтной шириной столбца и фактической.
                titleColWidth += (manufColWidth - different < minManufColWidth) ? minManufColWidth : different;
                manufColWidth = (manufColWidth - different < minManufColWidth) ? minManufColWidth : manufColWidth - different;
            }

            manufacturerColumn.Width = manufColWidth;
            articulColumn.Width = articulColWidth;
            titleColumn.Width = titleColWidth;
        }

        private static string GetOperationTitle(IOperation operation)
        {
            string paymentTypeInfo = null;
            if (operation is Sale sale && !sale.PaidCash)
            {
                paymentTypeInfo = "_безнал";

            }

            return $"№{operation.OperationId}_{operation.OperationDate:dd-MM-yyyy}{(string.IsNullOrWhiteSpace(paymentTypeInfo) ? "" : paymentTypeInfo)}";
        }
    }
}
