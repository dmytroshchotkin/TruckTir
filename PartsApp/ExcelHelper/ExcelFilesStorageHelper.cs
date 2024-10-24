using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.ExcelHelper
{
    internal static class ExcelFilesStorageHelper
    {
        internal static readonly string TempSalesFilesPath = Path.Combine(Path.GetTempPath(), "TruckTir\\Продажи");
        internal static readonly string TempPurchasesFilesPath = Path.Combine(Path.GetTempPath(), "TruckTir\\Приходы");

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
    }
}
