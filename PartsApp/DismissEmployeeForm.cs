using PartsApp.Models;
using System;
using System.Windows.Forms;

namespace PartsApp
{
    public partial class DismissEmployeeForm : Form
    {
        private readonly Employee _employee;
        private readonly DateTime _dismissalDate = DateTime.Now;

        public DismissEmployeeForm(Employee employee)
        {
            _employee = employee;
            InitializeComponent();
            nameLabel.Text += employee.FullName;
            hireDateTimeLabel.Text += employee.HireDate?.ToString("d");
            accessLevelLabel.Text += employee.AccessLayer;
            dismissalDateTimeLabel.Text += _dismissalDate.ToString("d");
        }

        private void OnAcceptDismissalButton(object sender, EventArgs e)
        {
            try
            {
                _employee.DismissalDate = DateTime.Now;
                PartsDAL.UpdateEmployee(_employee);
                                
                acceptDismissalButton.Visible = false;
                dismissalActionLabel.Text = "Блокировка доступа подтверждена.";
            }
            catch (Exception)
            {
                MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                return;
            }
        }
    }
}
