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
using PartsApp.Models;

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

            ReturnDGV.AutoGenerateColumns = false;
            /////////////////////////////////////////////////////////
            
            var sales = PartsDAL.FindSale(15);
            sales.OperationDetailsList.ToList().ForEach(od => od.Tag = od.Count); //Запоминаем в Tag каждого объекта его начальное значение количества.
            ReturnDGV.DataSource = sales.OperationDetailsList;
                        
            /////////////////////////////////////////////////////////
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
            //Если такого клиента нет в базе, выдаём ошибку.
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

        #region Методы работы с таблицей.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод для корректной binding-привязки вложенных эл-тов объекта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            DataGridViewColumn col = grid.Columns[e.ColumnIndex];

            if (row.DataBoundItem != null)
            {
                if (col.DataPropertyName.Contains("."))
                {
                    string[] props = col.DataPropertyName.Split('.');
                    Type type = row.DataBoundItem.GetType();
                    System.Reflection.PropertyInfo propInfo = type.GetProperty(props[0]);
                    object val = propInfo.GetValue(row.DataBoundItem, null);
                    for (int i = 1; i < props.Length; i++)
                    {
                        Type valueType = val.GetType();
                        propInfo = valueType.GetProperty(props[i]);
                        val = propInfo.GetValue(val, null);
                    }//for
                    e.Value = val;
                }//if
            }//if
        }//ReturnDGV_CellFormatting

        /// <summary>
        /// Событие для обработки начала ввода в ячейку "Количество".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void ReturnDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //Обрабатываем ввод в ячейку 'Количествo'.
            ReturnDGV[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
        }//ReturnDGV_CellBeginEdit

        /// <summary>
        /// Валидация ввода в ячейку "Количество".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CountCellFilled(ReturnDGV[e.ColumnIndex, e.RowIndex]);
        }//SaleDGV_CellEndEdit

        /// <summary>
        /// Событие для обработки стандартного сообщения об ошибке.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnDGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            CountCellFilled(ReturnDGV[e.ColumnIndex, e.RowIndex]);
            e.Cancel = false;
        }//ReturnDGV_DataError


        /// <summary>
        /// Производит необх. действия при окончании редактирования ячейки столбца 'Количество'.
        /// </summary>
        /// <param name="extCountCell">Редактируемая ячейка.</param>
        private void CountCellFilled(DataGridViewCell cell)
        {
            //Проверяем корректность ввода.
            string measureUnit = (cell.OwningRow.DataBoundItem as OperationDetails).SparePart.MeasureUnit;
            int lastCorrectRowIndex = ReturnDGV.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[CountCol.Index].Style.ForeColor == Color.Black).Count() - 1;

            if (!IsCountCellValueCorrect(cell, measureUnit))            
            {
                toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000); //выводим всплывающее окно с сообщением об ошибке.                
                //Если ячейка была корректно заполнена, перемещаем её вниз.
                if (cell.RowIndex < lastCorrectRowIndex)
                {                    
                    var list = ReturnDGV.DataSource as List<OperationDetails>;
                    OperationDetails operDet = cell.OwningRow.DataBoundItem as OperationDetails;
                    list.Remove(operDet);
                    list.Insert(lastCorrectRowIndex, operDet);
                    cell = ReturnDGV[CountCol.Index, lastCorrectRowIndex];
                }//if                 
                SetDefaultValueToCell(cell); //Возвращаем серый цвет и дефолтное значение данной ячейке.
            }//if
            else
            {
                //Если индекс строки не равен необходимому, перемещаем её вверх.
                if (cell.RowIndex != lastCorrectRowIndex)
                {
                    var list = ReturnDGV.DataSource as List<OperationDetails>;
                    OperationDetails operDet = cell.OwningRow.DataBoundItem as OperationDetails;
                    list.Remove(operDet);
                    list.Insert(lastCorrectRowIndex, operDet);
                    ReturnDGV[CountCol.Index, cell.RowIndex].Style.ForeColor = Color.Gray; //Возвращаем дефолтный цвет в ячейку строки на который был осущ-лен ввод.
                    cell = ReturnDGV[CountCol.Index, lastCorrectRowIndex];
                    cell.Style.ForeColor = Color.Black;
                }//if
            }//else

            //Заполняем ячейки столбца 'Сумма' и считаем 'итого' 
            FillTheInTotal();  
        }//CountCellFilled

        /// <summary>
        /// Возвращает число или генерирует исключение если введенное значение в ячейку 'Кол-во' некорректно.
        /// </summary>
        /// <param name="countCell">Ячейка столбца 'Кол-во'.</param>
        /// <returns></returns>
        private bool IsCountCellValueCorrect(DataGridViewCell countCell, string measureUnit)
        {
            float count;
            //Если введено не числовое значение, это ошибка.
            if (countCell.Value == null || (Single.TryParse(countCell.Value.ToString(), out count) == false))
                return false;

            //Ввод значения не более 0, или больше чем было приобретено, является ошибкой. 
            float totalCount = (float)(countCell.OwningRow.DataBoundItem as OperationDetails).Tag;
            if (count <= 0 || count > totalCount)
                return false;

            //Проверяем является ли введенное число корректным для продажи, т.е. кратно ли оно минимальной единице продажи.     
            if (count % Models.MeasureUnit.GetMinUnitSale(measureUnit) != 0)
                return false;

            return true;
        }//IsCountCellValueCorrect

        /// <summary>
        /// Записывает дефолтное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetDefaultValueToCell(DataGridViewCell cell)
        {
            cell.Style.ForeColor = Color.Gray;
            cell.Value = (cell.OwningRow.DataBoundItem as OperationDetails).Tag;
        }//SetDefaultValueToCell

        /// <summary>
        /// Записывает кастомное значения в переданную ячейку.
        /// </summary>
        /// <param name="cell">Ячейка.</param>
        private void SetCustomValueToCell(DataGridViewCell cell, object value)
        {
            cell.Style.ForeColor = Color.Black;
            cell.Value = value;
        }//SetCustomValueToCell

        /// <summary>
        /// Заполняет InTotalLabel корретным значением.
        /// </summary>
        private void FillTheInTotal()
        {
            float inTotal = 0;
            foreach (DataGridViewRow row in ReturnDGV.Rows)
            {
                if (row.Cells[CountCol.Index].Style.ForeColor == Color.Black)
                {
                    OperationDetails operDet = row.DataBoundItem as OperationDetails;

                    row.Cells[SumCol.Index].Value = operDet.Sum;
                    inTotal += operDet.Sum;
                }//if
                else
                {
                    row.Cells[SumCol.Index].Value = null;
                }//else
            }//foreach

            //Заполняем InTotalLabel расчитанным значением.
            inTotalNumberLabel.Text = String.Format("{0}(руб)", Math.Round(inTotal, 2, MidpointRounding.AwayFromZero));
        }//FillTheInTotal

        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из saleDGV. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = ReturnDGV.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc  = ReturnDGV.Location;
            Point gbLoc   = ReturnGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation






/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion





        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {

        }

        

        

        

        

        

        



    }//ReturnForm   

}//namespace
