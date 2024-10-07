using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.ExcelHelper
{
    internal static class ExcelFilesStorageHelper
    {
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
    }
}
