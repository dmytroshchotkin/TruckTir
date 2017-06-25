using PartsApp.Models;
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
    public partial class EmployeeOperationsInfoForm : Form
    {
        public EmployeeOperationsInfoForm()
        {
            InitializeComponent();
        }

        private void EmployeeOperationsInfoForm_Load(object sender, EventArgs e)
        {
            //Находим список всех сотрудников (сортируем по фамилии и имени) и делаем источником данных для ListBox.
            EmployeeListBox.DataSource = PartsDAL.FindEmployees().OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName).ToList();

            OperationsDGV.DataSource = PartsDAL.FindSales(Form1.CurEmployee, new DateTime(2017, 1, 1), null);        
        }//EmployeeOperationsInfoForm_Load



        /// <summary>
        /// Изменяем доступность DTP в зависимости от состояния CheckBox-ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Находим нужный DGV
            DateTimePicker dtp = (sender as CheckBox).Name == this.BeginDateCheckBox.Name ? BeginDateDTP : EndDateDTP;
            dtp.Enabled = !dtp.Enabled;
        }//BeginDateCheckBox_CheckedChanged

        
    }//EmployeeOperationsInfoForm

}//namespace
