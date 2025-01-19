using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartsApp.ExcelHelper
{
    internal static class ExcelFilesStorageHelper
    {
        internal static readonly string TempSalesFilesPath = GetTempFilesPath("Продажи");
        internal static readonly string TempPurchasesFilesPath = GetTempFilesPath("Приходы");

        internal static string SalesFilesPath { get; private set; } = GetSalesFilesPath();
        internal static string PurchasesFilesPath { get; private set; } = GetPurchasesFilesPath();

        internal static readonly string ExcelFilesExtension = "xlsx";

        internal static void SaveMultipleOperationsInExcel(List<IOperation> operations, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var operation in operations)
            {
                OperationsExcelHelper.SaveInExcelAsync(operation.OperationDetailsList, "Truck Tir", directory, false);
            }
        }

        internal static void TryOpenDirectory(string path)
        {
            if (Directory.Exists(path) && !IsDirectoryAlreadyOpened(path))
            {
                Process.Start("explorer.exe", path);
            }
        }

        internal static bool IsDirectoryAlreadyOpened(string path)
        {
            var processes = Process.GetProcessesByName("explorer");
            foreach (var p in processes)
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(path))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Возвращает полный путь к папке, включающий заданный юзером путь или дефолтную папку в Temp и дату операции
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        internal static string GetDirectoryByOperationTypeAndDate(IOperation operation)
        {
            string directory = default;
            if (operation is Sale sale)
            {
                directory = GetSaleDirectoryByDate(sale);
            }
            else if (operation is Purchase purchase)
            {
                directory = GetPurchaseDirectoryByDate(purchase);
            }

            return directory;
        }

        internal static void TryAddDuplicateNumberToFileName(ref string path)
        {
            int duplicateNumber = 1;
            string pathWithExtension = $"{path}.{ExcelFilesStorageHelper.ExcelFilesExtension}";

            while (File.Exists(pathWithExtension))
            {
                duplicateNumber++;
                pathWithExtension = $"{path}({duplicateNumber}).{ExcelFilesStorageHelper.ExcelFilesExtension}";
            }

            if (duplicateNumber > 1)
            {
                path = $"{path}({duplicateNumber})";
            }
        }

        internal static void TryAddDuplicateNumberToDirectoryName(ref string path)
        {
            int duplicateNumber = 1;
            string pathWithDuplicateNumber = path;

            while (Directory.Exists(pathWithDuplicateNumber))
            {
                duplicateNumber++;
                pathWithDuplicateNumber = $"{path}({duplicateNumber})";
            }

            if (duplicateNumber > 1)
            {
                path = $"{path}({duplicateNumber})";
            }
        }

        internal static string GetNewDirectoryInput()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите или создайте папку для сохранения документов:";
                folderDialog.ShowNewFolderButton = true;

                var inputResult = folderDialog.ShowDialog();
                if (inputResult == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string result = folderDialog.SelectedPath;
                    var input = AcceptChangeOfDirectory(result);
                    if (input == DialogResult.Yes)
                    {
                        return result;
                    }
                }
                return default;
            }
        }

        internal static void UpdateExcelFilesPaths()
        {
            UpdateSalesFilesPath();
            UpdatePurchasesFilesPath();
        }

        internal static void UpdateSalesFilesPath()
        {
            SalesFilesPath = GetSalesFilesPath();
        }

        internal static void UpdatePurchasesFilesPath()
        {
            PurchasesFilesPath = GetPurchasesFilesPath();
        }

        private static DialogResult AcceptChangeOfDirectory(string newPath)
        {
            return MessageBox.Show(
            $"{newPath}\n\nСохранить файлы Excel в эту папку?",
                    "Подтвердите выбор папки",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
        }

        private static string GetSalesFilesPath()
        {
            string savingPath = ConfigurationManager.AppSettings["SalesFilesSavePath"];
            if (!string.IsNullOrWhiteSpace(savingPath) && Directory.Exists(savingPath))
            {
                return savingPath;
            }
            else
            {
                return TempSalesFilesPath;
            }
        }

        private static string GetPurchasesFilesPath()
        {
            string savingPath = ConfigurationManager.AppSettings["PurchasesFilesSavePath"];
            if (!string.IsNullOrWhiteSpace(savingPath) && Directory.Exists(savingPath))
            {
                return savingPath;
            }
            else
            {
                return TempPurchasesFilesPath;
            }
        }

        private static string GetSaleDirectoryByDate(Sale sale)
        {
            string directory;
            string salePath = ConfigurationManager.AppSettings["SalesFilesSavePath"];
            if (string.IsNullOrWhiteSpace(salePath))
            {
                directory = Path.Combine(ExcelFilesStorageHelper.TempSalesFilesPath, sale.OperationDate.ToString("dd-MM-yyyy"));
            }
            else
            {
                directory = Path.Combine(salePath, sale.OperationDate.ToString("dd-MM-yyyy"));
            }

            return directory;
        }

        private static string GetPurchaseDirectoryByDate(Purchase purchase)
        {
            string directory;
            string purchasePath = ConfigurationManager.AppSettings["PurchasesFilesSavePath"];
            if (string.IsNullOrWhiteSpace(purchasePath))
            {
                directory = Path.Combine(ExcelFilesStorageHelper.TempPurchasesFilesPath, purchase.OperationDate.ToString("dd-MM-yyyy"));
            }
            else
            {
                directory = Path.Combine(purchasePath, purchase.OperationDate.ToString("dd-MM-yyyy"));
            }

            return directory;
        }

        private static string GetTempFilesPath(string directoryTitle)
        {
            string path = Path.Combine(Path.GetTempPath(), $"TruckTir\\{directoryTitle}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static void CleanupTempDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), "TruckTir");
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                foreach (var d in di.GetDirectories())
                {
                    foreach (var f in d.GetFiles())
                    {
                        try
                        {
                            f.Delete();
                        }
                        catch (IOException) { }
                    }
                }
            }
        }
    }
}
