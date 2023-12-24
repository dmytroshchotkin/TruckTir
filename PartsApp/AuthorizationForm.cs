using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PartsApp.Models;
using PartsApp.SupportClasses;

namespace PartsApp
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
            //Заполняем выпадающий список контрола для ввода ФИО.
            IList<Employee> employees = PartsDAL.FindEmployees();
            foreach (Employee employee in employees)
            {
                loginTextBox.AutoCompleteCustomSource.Add(employee.Login);
            }
        }

        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Owner.Close();
            }
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Проверяем введенные данные.
                string inputPasswordHash = PasswordClass.GetHashString(passwordTextBox.Text.Trim());

                var employees = PartsDAL.FindEmployees().Where(empl => empl.Login == loginTextBox.Text.Trim());
                Employee employee = employees.FirstOrDefault(empl => empl.Password == inputPasswordHash);

                if (employee is null || employee.IsDismissed)
                {
                    toolTip.Show("Введены неверные данные.", this, okButton.Location, 3000);
                }
                else
                {
                    Form1.CurEmployee = employee;
                    Close();
                }
            }
        }
    }
}