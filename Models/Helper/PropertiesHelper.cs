using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models.Helper
{
    public static class PropertiesHelper
    {
        /// <summary>
        /// Возвращает заданное описание для данного перечислителя.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}
