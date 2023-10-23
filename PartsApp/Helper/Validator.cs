using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Helper
{
    public static class Validator
    {
        /// <summary>
        /// Returns DateTime value if it is successfully parsed using any culture or default if parsing failed
        /// </summary>
        /// <param name="datetimeToString"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string datetimeToString)
        {
            if (DateTime.TryParse(datetimeToString, CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            if (DateTime.TryParse(datetimeToString, new CultureInfo("ru-RU"), DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }

            foreach (var c in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (DateTime.TryParse(datetimeToString, c, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
            }

            return default;
        }
    }
}
