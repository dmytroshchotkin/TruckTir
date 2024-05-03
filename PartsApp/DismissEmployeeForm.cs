using PartsApp.Models;
using System;
using System.Windows.Forms;

namespace PartsApp
{
    public partial class DisableEmployeeForm : Form
    {
        private readonly Employee _employee;
        private readonly DateTime _disableDate = DateTime.Now;

        public DisableEmployeeForm(Employee employee)
        {
            _employee = employee;
            InitializeComponent();
            nameLabel.Text += employee.FullName;
            hireDateTimeLabel.Text += employee.HireDate?.ToString("d");
            accessLevelLabel.Text += employee.AccessLayer;
            disableDateTimeLabel.Text += _disableDate.ToString("d");
        }

        private void OnAcceptDisableButton(object sender, EventArgs e)
        {
            try
            {
                _employee.DisableDate = DateTime.Now;
                PartsDAL.UpdateEmployee(_employee);
                                
                acceptDisableButton.Visible = false;
                disableActionLabel.Text = "Блокировка доступа подтверждена.";
            }
            catch (Exception)
            {
                MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                return;
            }
        }
    }
}
