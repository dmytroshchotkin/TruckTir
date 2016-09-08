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


        /// <summary>
        /// Заполняет RowHeaders переданной строки текущим номером по порядку.
        /// </summary>
        /// <param name="row">Нумеруемая строка.</param>
        private static void RowNumerate(DataGridViewRow row)
        {
            DataGridView dgv = row.DataGridView;
            string rowNumber = (row.Index + 1).ToString();
            object headerCellValue = dgv.Rows[row.Index].HeaderCell.Value;
            //Если RowHeadersCell не заполнена или индекс строки изменен, присваиваем новый номер строке.
            if (headerCellValue == null || headerCellValue.ToString() != rowNumber)
                dgv.Rows[row.Index].HeaderCell.Value = rowNumber;
        }//RowNumerate
        /// <summary>
        /// Заполняет RowHeaders переданной строки текущим номером по порядку и устанавливает его ширину.
        /// </summary>
        /// <param name="row">Нумеруемая строка.</param>
        public static void RowsNumerateAndAutoSize(DataGridViewRow row)
        {
            RowNumerate(row);                           //Нумеруем строку.
            RowHeadersWidthAutoSize(row.DataGridView);  //Задаём размер RowHeaders.
        }//RowsNumerateAndAutoSize
        /// <summary>
        /// Заполняет RowHeaders строк переданной таблицы текущими номероми по порядку и устанавливает его ширину.
        /// </summary>
        /// <param name="dgv">Таблица с нумеруемыми строками.</param>
        public static void RowsNumerateAndAutoSize(DataGridView dgv)
        {
            
            foreach (DataGridViewRow row in dgv.Rows)
                RowNumerate(row);            //Нумеруем строку.                    

            RowHeadersWidthAutoSize(dgv);    //Задаём размер RowHeaders.
        }//RowsNumerateAndAutoSize
        /// <summary>
        /// Устанавливает ширину RowHeaders переденной таблицы.
        /// </summary>
        /// <param name="dgv">Таблица.</param>
        public static void RowHeadersWidthAutoSize(DataGridView dgv)
        {
            //Если необходимо меняем ширину RowHeaders в зависимости от кол-ва строк в таблице.
            int defaultRowHeadersWidth = 41;
            int oneDigitWidth = 7; //Ширина одного разряда числа (определена методом тыка).
            int newRowHeadersWidth = defaultRowHeadersWidth + (oneDigitWidth * (dgv.Rows.Count.ToString().Length - 1));
            if (dgv.RowHeadersWidth != newRowHeadersWidth) //Проверка необходима, потому что изменение RowHeadersWidth приводит к инициированию события OnPaint, а сл-но к бесконечному циклу. 
                dgv.RowHeadersWidth = newRowHeadersWidth;
        }//RowHeadersWidthAutoSize


    }//EnumerableExtensions



}//namespace
