using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace PartsApp.SupportClasses
{
    public static class EnumerableExtensions
    {
        public static T MaxBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new Tuple<T, R>(t, evaluate(t)))
                .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) > 0 ? next : max).Item1;
        }//MaxBy

        public static T MinBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new Tuple<T, R>(t, evaluate(t)))
                .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) < 0 ? next : max).Item1;
        }//MinBy

        /// <summary>
        /// Возвращает заданное описание для данного перечислителя.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }//ToDescription


        /// <summary>
        /// Возвращает список всех контролов заданного типа, расположенных на форме.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type">Тип контролов которые требуется найти.</param>
        /// <returns></returns>
        public static List<Control> GetAllControls(this Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type).ToList();
        }//GetAllControls

        /// <summary>
        /// Возвращает список всех контролов заданного типа и имеющие совпадение в имени, расположенных на форме.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type">Тип контролов которые требуется найти.</param>
        /// <param name="searchName">Полное или частичное имя контрола.</param>
        /// <returns></returns>
        public static List<Control> GetAllControls(this Control control, Type type, string searchName)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type && c.Name.Contains(searchName)).ToList();
        }//GetAllControls
    }//EnumerableExtensions



}//namespace
