using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PartsApp.SupportClasses;

namespace PartsApp
{
    public partial class ReturnForm : Form
    {
        public ReturnForm()
        {
            InitializeComponent();
        }//

        private void ReturnForm_Load(object sender, EventArgs e)
        {
            //Устанавливаем даты для DateTimePicker.
            OperationDateTimePicker.MaxDate = DateTime.Now.Date.AddDays(7);
            OperationDateTimePicker.MinDate = DateTime.Now.Date.AddDays(-7);
            OperationDateTimePicker.Value   = DateTime.Now;

            //Заполняем список автоподстановки для ввода контрагента.
            ContragentTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindCustomers().Select(c => c.ContragentName).ToArray());

            AgentEmployeerTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);            
        }//ReturnForm_Load


        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ContragentTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ContragentTextBox_Leave(sender, null);
                descriptionRichTextBox.Select(); //переводим фокус на на другой объект.
            }//if
        }//ContragentTextBox_PreviewKeyDown

        private void ContragentTextBox_Leave(object sender, EventArgs e)
        {
            //Если такого клиента нет в базе.
            string customer = ContragentTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == ContragentTextBox.Text.Trim().ToLower());
            if (customer != null)
            {
                ControlValidation.CorrectValueInput(toolTip, ContragentTextBox);
                ContragentTextBox.Text = customer; //Выводим корректное имя контрагента.
            }//if
            else
            {                
                ControlValidation.WrongValueInput(toolTip, ContragentTextBox, "Поле \"Клиент\" заполнено некорректно");
            }//else      
        }//ContragentTextBox_Leave

        private void AgentTextBox_Leave(object sender, EventArgs e)
        {
            ControlValidation.IsInputControlEmpty(AgentTextBox, toolTip);
        }//AgentTextBox_Leave




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion





        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {

        }

        

        



    }//ReturnForm   

}//namespace
