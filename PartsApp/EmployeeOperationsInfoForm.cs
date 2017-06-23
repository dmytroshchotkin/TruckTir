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
