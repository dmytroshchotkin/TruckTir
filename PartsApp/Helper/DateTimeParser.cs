using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    public static class DateTimeParser
    {
        /// <summary>
        /// Returns DateTime value if it is successfully parsed using any culture
        /// </summary>
        /// <param name="datetimeString"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string datetimeString)
        {
            if (DateTime.TryParse(datetimeString, CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            if (DateTime.TryParse(datetimeString, new CultureInfo("ru-RU"), DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }

            foreach (var c in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (DateTime.TryParse(datetimeString, c, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
            }

            throw new ArgumentException($"Cannot parse DateTime from {datetimeString}");
        }
    }
}
