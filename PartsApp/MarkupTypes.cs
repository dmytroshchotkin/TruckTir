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
        //public static double GetMarkupValue(string markupType)
        //{
        //    double? markup = null;
        //    //узнаем процент заданной наценки.
        //    foreach (var markType in markupTypes)
        //        if (markType.Key == markupType) { markup = markType.Value; break; }
        //    //если наценка задавалась вручную (нужна проверка корректности ввода)
        //    if (markup == null)
        //        markup = Convert.ToDouble(markupType);

        //    return (double)markup;
        //}//GetMarkupValue
        //public static string GetMarkupType(double? markupValue)
        //{
        //    if (markupValue == null) return null;
        //    string markupType = null;
        //    //узнаем процент заданной наценки.
        //    foreach (var markType in markupTypes)
        //        if (markType.Value == markupValue) { markupType = markType.Key; break; }
        //    //если наценка задавалась вручную (нужна проверка корректности ввода)
        //    if (markupType == null)
        //        markupType = "Другая наценка";
        //    return markupType;
        //}//GetMarkupValue

        /// <summary>
        /// Возвращает выбранное поль-лем значение наценки. При вводе не числового значения выбрасывает ошибку.
        /// </summary>
        /// <returns></returns>
        public static double GetMarkupValue(string markupType)
        {
            double markup = 0;
            //Проверяем выбранное или введенное значение наценки на наличие в базе.
            try
            {
                markup = PartsDAL.FindMarkupValue(markupType);
            }//try
            //Если значение введено вручную и не содержится в базе.    
            catch (InvalidOperationException)
            {
                //Проверяем является введенное поль-лем значение числом.
                markup = Convert.ToDouble(markupType);
            }//catch

            return markup;
        }//GetMarkupValue
        /// <summary>
        /// Возвращает тип наценки по заданному значению. 
        /// </summary>
        /// <param name="markup">Заданная наценка.</param>
        /// <returns></returns>
        public static string GetMarkupType(double markup)
        {
            string markupType = null;
            //Проверяем выбранное или введенное значение наценки на наличие в базе.
            try
            {
                markupType = PartsDAL.FindMarkupType(markup);
            }//try
            //Если значение введено вручную и не содержится в базе.    
            catch (InvalidOperationException)
            {
                if (markup > 0)
                    markupType = "Другая наценка";
                else if (markup < 0)
                    markupType = "Уценка";
            }//catch

            return markupType;
        }//GetMarkupType
    }//MarkupTypes


}//namespace
