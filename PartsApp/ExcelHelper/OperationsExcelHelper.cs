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

namespace PartsApp.ExcelHelper
{
    public static class OperationsExcelHelper
    {        
        /// <summary>
        /// Асинхронный вывод в Excel инф-ции из переданного списка товаров.
        /// </summary>
        /// <param name="sparePart">Список товаров для вывода в Excel.</param>
        /// <param name="agent">Фирма-покупатель.</param>
        internal static async void SaveInExcelAsync(IList<OperationDetails> operDetList, string agent)
        {
            try
            {
                await Task.Factory.StartNew(() => SaveInExcel(operDetList, agent));
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
        private static void SaveInExcel(IList<OperationDetails> operDetList, string agent)
        {
            //Purchase purchase = operDetList[0].Operation as Purchase;
            var operation = operDetList[0].Operation;

            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            ExcelWorkBook.Windows[1].Caption = GetValidExcelBookTitle(GetOperationTitle(operation));
            Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.
            ExcelWorkSheet.PageSetup.Zoom = false;
            ExcelWorkSheet.PageSetup.FitToPagesWide = 1;

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.TopMargin = ExcelWorkSheet.PageSetup.BottomMargin = 7;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin = 7;

            int row = 1, column = 1;

            //Выводим Id и Дату. 
            OperationIdAndDateExcelOutput(ExcelWorkSheet, operation, row);

            //Выводим поставщика и покупателя / продавца и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = operation is Purchase ? GetPurchaseAgentsDescriptionForDocHeader(operation, agent) : GetSaleAgentsDescriptionForDocHeader(operation, agent);

            //Заполняем таблицу.
            FillTheExcelList(ExcelWorkSheet, operDetList, ref row, column);

            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas"; //моноширинный шрифт
            ExcelApp.Cells[row, column] = operation is Purchase ? GetPurchaseAgentsDescriptionForDocVisas(operation) : GetSaleAgentsDescriptionForDocVisas(operation);
            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;

            //Выводим заметку к операции.
            DescriptionExcelOutput(ExcelWorkSheet, operation.Description, ref row, column);

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = ExcelApp.UserControl = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.            
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
        /// <param name="ExcelWorkSheet">Рабочая страница</param>
        /// <param name="operDetList">Список деталей операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void FillTheExcelList(Excel.Worksheet ExcelWorkSheet, IList<OperationDetails> operDetList, ref int row, int column)
        {
            row += 2;
            //Выводим заголовок.
            FillTheTitlesRow(ExcelWorkSheet, row, column);

            //Уменьшаем ширину колонки "Ед. изм."
            ExcelWorkSheet.Cells[row, column + 4].VerticalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            ExcelWorkSheet.Cells[row, column + 4].Columns.ColumnWidth = 5;

            //Устанавливаем ширину столбцов.
            int titleColWidth = 30, articulColWidth = 20; // -- Взято методом тыка.  
            SetColumnsWidth(operDetList, ExcelWorkSheet.Cells[row, column + 3], ExcelWorkSheet.Cells[row, column + 2], ExcelWorkSheet.Cells[row, column]);

            //Выводим список товаров.
            float inTotal = 0;
            foreach (OperationDetails operDet in operDetList)
            {
                FillExcelRow(ExcelWorkSheet, operDet, ++row, column, titleColWidth, articulColWidth);
                inTotal += operDet.Price * operDet.Count;
            }
            //Обводим талицу рамкой. 
            ExcelWorkSheet.get_Range("A" + (row - operDetList.Count + 1).ToString(), "H" + row.ToString()).Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            ++row;
            //Выводим 'Итого'.
            InTotalExcelOutput(ExcelWorkSheet, inTotal, row, column);
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
            ExcelWorkSheet.Cells[row, column + 1] = "Склад";
            ExcelWorkSheet.Cells[row, column + 2] = "Артикул";
            ExcelWorkSheet.Cells[row, column + 3] = "Название";
            ExcelWorkSheet.Cells[row, column + 4] = "Ед. изм.";
            ExcelWorkSheet.Cells[row, column + 5] = "Кол-во";
            ExcelWorkSheet.Cells[row, column + 6] = "Цена";
            ExcelWorkSheet.Cells[row, column + 7] = "Сумма";

            //Настраиваем вид клеток.
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "H" + row.ToString());
            excelCells.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
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
        private static void FillExcelRow(Excel.Worksheet ExcelWorkSheet, OperationDetails operDet, int row, int column, int titleColWidth, int articulColWidth)
        {
            SetStringExcelNumberFormatForArticulAndStorageCell(ExcelWorkSheet, row, column);
            // Устанавливаем перенос по словам для всей строки
            ExcelWorkSheet.Rows[row].WrapText = true;

            ExcelWorkSheet.Cells[row, column + 2] = operDet.SparePart.Articul;
            ExcelWorkSheet.Cells[row, column + 3] = operDet.SparePart.Title;
            //Выравнивание диапазона строк.
            ExcelWorkSheet.get_Range("A" + row.ToString(), "H" + row.ToString()).VerticalAlignment = Excel.Constants.xlTop;
            ExcelWorkSheet.get_Range("A" + row.ToString(), "H" + row.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;                     

            ExcelWorkSheet.Cells[row, column] = operDet.SparePart.Manufacturer;
            ExcelWorkSheet.Cells[row, column + 1] = operDet.SparePart.StorageCell;
            ExcelWorkSheet.Cells[row, column + 4] = operDet.SparePart.MeasureUnit;
            ExcelWorkSheet.Cells[row, column + 5] = operDet.Count;
            ExcelWorkSheet.Cells[row, column + 6] = operDet.Price;
            ExcelWorkSheet.Cells[row, column + 7] = operDet.Price * operDet.Count;
        }

        private static void SetStringExcelNumberFormatForArticulAndStorageCell(Excel.Worksheet excelWorkSheet, int row, int column)
        {
            excelWorkSheet.Cells[row, column + 1].NumberFormat = excelWorkSheet.Cells[row, column + 2].NumberFormat = "@";
        }

        /// <summary>
        /// Выводим 'Итого' в заданной клетке.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист.</param>
        /// <param name="inTotal">Общая сумма операции.</param>
        /// <param name="row">Индекс строки.</param>
        /// <param name="column">Индекс столбца.</param>
        private static void InTotalExcelOutput(Excel.Worksheet ExcelWorkSheet, float inTotal, int row, int column)
        {
            //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            int indent = 0; //отступ
            if (inTotal.ToString("0.00").Length <= 9)
            {
                indent = 1;
            }

            ExcelWorkSheet.Cells[row, column + 5 + indent] = "Итого : ";
            ExcelWorkSheet.Cells[row, column + 6 + indent] = inTotal.ToString("0.00");
            ExcelWorkSheet.Cells[row, column + 6 + indent].Font.Underline = true;
            ExcelWorkSheet.Cells[row, column + 6 + indent].Font.Size = ExcelWorkSheet.Cells[row, column + 5 + indent].Font.Size = 12;
            ExcelWorkSheet.Cells[row, column + 6 + indent].Font.Bold = ExcelWorkSheet.Cells[row, column + 5 + indent].Font.Bold = true;
            ExcelWorkSheet.get_Range("G" + row.ToString(), "H" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        }

        /// <summary>
        /// Заполняет заданную строку Id операции и датой.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="operation">Объект операции.</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private static void OperationIdAndDateExcelOutput(Excel.Worksheet ExcelWorkSheet, IOperation operation, int row)
        {
            string titlePattern = operation is Purchase ? "Приходная накладная №{0} от {1}г." : "Расходная накладная №{0} от {1}г.";

            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "H" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format(titlePattern, operation.OperationId, operation.OperationDate.ToString("dd/MM/yyyy"));
        }

        /// <summary>
        /// Выводит заметку об операции.
        /// </summary>
        /// <param name="ExcelWorkSheet">Рабочий лист</param>
        /// <param name="description">заметка</param>
        /// <param name="row">Индекс строки</param>
        /// <param name="column">Индекс столбца</param>
        private static void DescriptionExcelOutput(Excel.Worksheet ExcelWorkSheet, string description, ref int row, int column)
        {
            if (description != null)
            {
                //Делаем визуальное отделение информации от заметки, с помощью линии.
                ExcelWorkSheet.Cells[row, column].Value = "                                                                                                                                                                                                                                 ";//longEmptyString.ToString();
                ExcelWorkSheet.Cells[row, column].Font.Underline = true;
                //Выводим заметку
                row++;
                // объединим область ячеек  строки "вместе"? для вывода операции.
                Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "H" + row.ToString());
                excelCells.Merge(true);
                excelCells.WrapText = true;
                excelCells.Value = description;
                AutoFitMergedCellRowHeight((ExcelWorkSheet.Cells[row, column] as Excel.Range));
            }
        }

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleCol">Столбец "Название".</param>
        /// <param name="articulCol">Столбец "Артикул".</param>
        /// <param name="manufCol">Столбец "Производитель".</param>
        private static void SetColumnsWidth(IList<OperationDetails> operDetList, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = operDetList.Select(od => od.SparePart.Manufacturer).Where(man => man != null);
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

        private static void AutoFitMergedCellRowHeight(Excel.Range rng)
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
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает ширину заданной области.
        /// </summary>
        /// <param name="rng">Область ширина которой считается.</param>
        /// <returns></returns>
        private static double GetRangeWidth(Excel.Range rng)
        {
            double rngWidth = 0;
            for (int i = 1; i <= rng.Columns.Count; ++i)
            {
                rngWidth += rng.Cells.Item[1, i].ColumnWidth;
            }
            return rngWidth;
        }

        private static string xlRCtoA1(int ARow, int ACol, bool RowAbsolute = false, bool ColAbsolute = false)
        {
            int A1 = 'A' - 1;  // номер "A" минус 1 (65 - 1 = 64)
            int AZ = 'Z' - A1; // кол-во букв в англ. алфавите (90 - 64 = 26)

            int t, m;
            string S;

            t = ACol / AZ; // целая часть
            m = (ACol % AZ); // остаток?
            if (m == 0)
            {
                t--;
            }
            if (t > 0)
            {
                S = Convert.ToString((char)(A1 + t));
            }
            else
            {
                S = String.Empty;
            }

            if (m == 0)
            {
                t = AZ;
            }
            else
            {
                t = m;
            }

            S = S + (char)(A1 + t);

            //весь адрес.
            if (ColAbsolute)
            {
                S = '$' + S;
            }
            if (RowAbsolute)
            {
                S = S + '$';
            }

            S = S + ARow.ToString();
            return S;
        }

        public static string GetValidExcelBookTitle(string title)
        {
            if (TrySetDefaultTitle(title, out string defaultTitle))
            {
                return defaultTitle;
            }

            var forbiddenSymbols = new char[] { '/', '\\', '|', ':', '*', '?', '"', '<', '>', ',', '\'', '~', '`', '?' };
            var validatedString = new StringBuilder();

            foreach (var c in title)
            {
                if (forbiddenSymbols.Contains(c))
                {
                    validatedString.Append(' ');
                }
                else
                {
                    validatedString.Append(c);
                }
            }

            string clearedTitle = Regex.Replace(validatedString.ToString(), @"\s+", " ");

            return TrySetDefaultTitle(clearedTitle, out string defaultT) ? defaultT : clearedTitle;
        }

        private static bool TrySetDefaultTitle(string title, out string result)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                result = $"{DateTime.Now:dd-MM-yyyy}_PartsApp";
                return true;
            }

            result = default;
            return false;
        }

        private static string GetOperationTitle(IOperation operation)
        {
            string paymentTypeInfo = null;
            if (operation is Sale sale && !sale.PaidCash)
            {
                paymentTypeInfo = "_безнал";
                
            }

            return $"№{operation.OperationId}_{operation.OperationDate:dd-MM-yyyy}_{operation.Contragent.ContragentName}{paymentTypeInfo}";
        }
    }
}
