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
        private bool _isCorrectClose = false;

        public AuthorizationForm()
        {
            InitializeComponent();
            //Заполняем выпадающий список контрола для ввода ФИО.
            IList<Employee> employees = PartsDAL.FindEmployees();
            foreach (Employee employee in employees)
                loginTextBox.AutoCompleteCustomSource.Add(employee.Login);

        }

        private void AuthorizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_isCorrectClose == false)
                    e.Cancel = true;
            }
        }

        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Owner.Close();
            }
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Проверяем введенные данные.
                string inputPasswordHash = PasswordClass.GetHashString(passwordTextBox.Text.Trim());
                try
                {
                    var employees = PartsDAL.FindEmployees().Where(empl => empl.Login == loginTextBox.Text.Trim());
                    Employee employee = employees.Where(empl => empl.Password == inputPasswordHash).First();

                    Form1.CurEmployee = employee;
                    _isCorrectClose = true;
                    this.Close();
                }
                catch
                {
                    toolTip.Show("Введены неверные данные.", this, okButton.Location, 3000);
                }
                //var employeesList = PartsDAL.FindAllEmployees().Where(empl => empl.GetFullName() == fullNameTextBox.Text.Trim() && empl.Password == inputPasswordHash).First();

            }
        }

    }
}