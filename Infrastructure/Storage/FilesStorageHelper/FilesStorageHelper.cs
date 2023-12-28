using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class FilesStorageHelper
    {
        public static void CopyFileSafely(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(destinationFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.Copy(sourceFilePath, destinationFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while copying '{sourceFilePath}' into '{destinationFilePath}':\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

    }
}
