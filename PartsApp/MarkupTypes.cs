using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    /// <summary>
    /// Класс со всеми видами наценки. !!!Сделать лучше перечисление, или продумать по другому.!!!
    /// </summary>
    class MarkupTypes
    {
        /// <summary>
        /// Коллекция всех типов наценки
        /// </summary>
        public static List<KeyValuePair<string, double>> markupTypes = new List<KeyValuePair<string, double>>()
            {
                new KeyValuePair<string, double>("Розница", 100),
                new KeyValuePair<string, double>("Мелкий опт", 70),
                new KeyValuePair<string, double>("Средний опт", 50),
                new KeyValuePair<string, double>("Крупный опт", 30),            
            };

        public static double GetMarkupValue(string markupType)
        {
            double? markup = null;
            //узнаем процент заданной наценки.
            foreach (var markType in markupTypes)
                if (markType.Key == markupType) { markup = markType.Value; break; }
            //если наценка задавалась вручную (нужна проверка корректности ввода)
            if (markup == null)
                markup = Convert.ToDouble(markupType);

            return (double)markup;
        }//GetMarkupValue
        public static string GetMarkupType(double? markupValue)
        {
            if (markupValue == null) return null;
            string markupType = null;
            //узнаем процент заданной наценки.
            foreach (var markType in markupTypes)
                if (markType.Value == markupValue) { markupType = markType.Key; break; }
            //если наценка задавалась вручную (нужна проверка корректности ввода)
            if (markupType == null)
                markupType = "Другая наценка";
            return markupType;
        }//GetMarkupValue

    }//MarkupTypes


}//namespace
