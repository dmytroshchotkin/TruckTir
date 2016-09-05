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
        }//KeyDownPress

        /// <summary>
        /// Выполняет необходимые действия при нажатии юзером Keys.Up.
        /// </summary>
        public static void KeyUpPress(ListBox autoCompleteListBox)
        {
            if (autoCompleteListBox.SelectedIndex == -1)
            {
                autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1; //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
            }//if
            else
            {
                if (autoCompleteListBox.SelectedIndex == 0)
                    autoCompleteListBox.ClearSelected();
                else
                    autoCompleteListBox.SelectedIndex -= 1;
            }//else
        }//KeyUpPress
    }//AutoCompleteListBox


}//namespace
