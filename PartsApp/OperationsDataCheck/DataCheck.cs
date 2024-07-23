using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.OperationsDataCheck
{
    internal static class DataCheck
    {
        /// <summary>
        /// Имя, зарезервированное для оформления возвратов 
        /// </summary>
        internal const string ReturnContragentName = "Возврат";

        internal static bool CanEditContragent(IContragent contragent)
        {
            return contragent.ContragentName != ReturnContragentName;
        }
    }
}
