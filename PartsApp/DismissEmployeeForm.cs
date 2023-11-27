using PartsApp.Models;
using System;
using System.Windows.Forms;

namespace PartsApp
{
    public partial class DismissEmployeeForm : Form
    {
        private readonly Employee _employee;

        public DismissEmployeeForm(Employee employee)
        {
            _employee = employee;
            InitializeComponent();
            nameLabel.Text += employee.FullName;
            hireDateTimeLabel.Text += employee.HireDate?.ToString("d");
            accessLevelLabel.Text += employee.AccessLayer;
            dismissalDateTimePicker.MinDate = (DateTime)employee.HireDate;  
            dismissalDateTimePicker.MaxDate = DateTime.Now.AddMonths(1);
        }

        private void OnAcceptDismissalButton(object sender, EventArgs e)
        {
            try
            {
                DateTime dismissalDate = dismissalDateTimePicker.Checked ? dismissalDateTimePicker.Value : DateTime.Now;
                _employee.DismissalDate = dismissalDate;
                PartsDAL.UpdateEmployee(_employee);
                                
                acceptDismissalButton.Visible = false;

                dismissalActionLabel.Text = "Увольнение подтверждено";
                dismissalDateTimeLabel.Text += _employee.DismissalDate?.ToString("d");
                dismissalDateTimePicker.Visible = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                return;
            }
        }
    }
}
