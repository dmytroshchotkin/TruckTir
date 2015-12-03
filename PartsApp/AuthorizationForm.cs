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
    public partial class AuthorizationForm : Form
    {
        private bool _isCorrectClose = false;

        public AuthorizationForm()
        {
            InitializeComponent();
            //Заполняем выпадающий список контрола для ввода ФИО.
            IList<Employee> employees = PartsDAL.FindAllEmployees();
            foreach (Employee employee in employees)
                fullNameTextBox.AutoCompleteCustomSource.Add(employee.GetFullName());

        }

        private void AuthorizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_isCorrectClose == false)
                    e.Cancel = true;
            }//if
        }//AuthorizationForm_FormClosing


        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Owner.Close();
            }//if
        }//cancelButton_MouseClick

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Проверяем введенные данные.
                string inputPasswordHash = PasswordClass.GetHashString(passwordTextBox.Text.Trim());
                try
                {
                    var employees = PartsDAL.FindAllEmployees().Where(empl => empl.GetFullName() == fullNameTextBox.Text.Trim());
                    Employee employee = employees.Where(empl => empl.Password == inputPasswordHash).First();

                    Form1.CurEmployee = employee;
                    _isCorrectClose = true;
                    this.Close();
                }//try
                catch 
                {
                    toolTip.Show("Введены неверные данные.", this, okButton.Location, 3000); 
                }
                //var employees = PartsDAL.FindAllEmployees().Where(empl => empl.GetFullName() == fullNameTextBox.Text.Trim() && empl.Password == inputPasswordHash).First();

/*!!!*/  //Необходимо добавить какое-то различие для сотрудников с одинаковыми именами.
                
            }//if
        }

        


    }//AuthorizationForm
}//namespace
