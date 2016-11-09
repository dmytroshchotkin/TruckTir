using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            CustomerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindCustomers().Select(c => c.ContragentName).ToArray());

            AgentEmployeerTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
        }//ReturnForm_Load


        private void CustomerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CustomerTextBox.Text))
            {
                CustomerBackPanel.BackColor = CustomerStarLabel.ForeColor = Color.Red;
                CustomerTextBox.Clear();
                toolTip.Show("Введите имя/название клиента", this, CustomerBackPanel.Location, 2000);
            }//if
            else
            {
                CustomerStarLabel.ForeColor = Color.Black;
                CustomerBackPanel.BackColor = SystemColors.Control;

                //Если такой клиен в базе отсутствует, выводим сообщение об этом.
                string customer = CustomerTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == CustomerTextBox.Text.Trim().ToLower());
                if (customer == null)
                    toolTip.Show("Такого клиента нет в базе! Он будет добавлен.", this, CustomerBackPanel.Location, 2000);
                else
                    CustomerTextBox.Text = customer; //Выводим корректное имя контрагента. 
            }//else      
        }//CustomerTextBox_Leave











        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {

        }

        



    }//ReturnForm   

}//namespace
