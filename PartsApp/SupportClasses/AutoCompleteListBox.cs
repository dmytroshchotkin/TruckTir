using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartsApp.SupportClasses
{
    public static class AutoCompleteListBox
    {
        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Down.
        /// </summary>
        public static void KeyDownPress(ListBox autoCompleteListBox)
        {
            //Если выбран последний эл-нт списка, вернуть начальное значение и убрать выделение в listBox-е. 
            if (autoCompleteListBox.SelectedIndex == autoCompleteListBox.Items.Count - 1)
                autoCompleteListBox.ClearSelected();
            else
                autoCompleteListBox.SelectedIndex += 1;
        }

        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Up.
        /// </summary>
        public static void KeyUpPress(ListBox autoCompleteListBox)
        {
            if (autoCompleteListBox.SelectedIndex == -1)
            {
                autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1; //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
            }
            else
            {
                if (autoCompleteListBox.SelectedIndex == 0)
                    autoCompleteListBox.ClearSelected();
                else
                    autoCompleteListBox.SelectedIndex -= 1;
            }
        }

        /// <summary>
        /// Производит необходимые действия при смене DataSource.
        /// </summary>
        /// <param name="autoCompleteListBox">Выпадающий список.</param>
        public static void DataSourceChanged(ListBox autoCompleteListBox)
        {
            if (autoCompleteListBox.DataSource != null)
            {
                List<Models.SparePart> spList = autoCompleteListBox.DataSource as List<Models.SparePart>;
                //Форматируем вывод.
                //Находим максимальную ширину каждого параметра.
                int articulMaxLenght = spList.Max(sp => sp.Articul.Length);
                int titlelMaxLenght = spList.Max(sp => sp.Title.Length);
                int manufMaxLenght = spList.Select(sp => sp.Manufacturer).Where(m => m != null).DefaultIfEmpty(String.Empty).Max(m => m.Length);

                //Запоминаем ширину всех столбцов.
                autoCompleteListBox.Tag = new Tuple<int, int, int>(articulMaxLenght, titlelMaxLenght, manufMaxLenght);

                autoCompleteListBox.Visible = true;
            }
            else
                autoCompleteListBox.Visible = false;
        }

        /// <summary>
        /// Форматирование вывода в ListBox.
        /// </summary>
        /// <param name="autoCompleteListBox">Выпадающий список.</param>
        /// <param name="e"></param>       
        public static void OutputFormatting(ListBox autoCompleteListBox, ListControlConvertEventArgs e)
        {
            //Находим максимальную ширину каждого параметра.            
            Tuple<int, int, int> columnsWidth = autoCompleteListBox.Tag as Tuple<int, int, int>;
            int articulMaxLenght = columnsWidth.Item1;
            int titlelMaxLenght = columnsWidth.Item2;
            int manufMaxLenght = columnsWidth.Item3;

            //Задаём нужный формат для выводимых строк.
            string artCol = String.Format("{{0, {0}}}", -articulMaxLenght);
            string titleCol = String.Format("{{1, {0}}}", -titlelMaxLenght);
            string manufCol = String.Format("{{2, {0}}}", -manufMaxLenght);

            Models.SparePart sparePart = e.ListItem as Models.SparePart;
            e.Value = String.Format(artCol + "   " + titleCol + "   " + manufCol, sparePart.Articul, sparePart.Title, sparePart.Manufacturer);
        }
    }

}