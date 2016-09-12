using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PartsApp.SupportClasses
{
    public static class ControlValidation
    {
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="toolTip"></param>
        /// <param name="inputControl">Контрол ввода инф-ции.</param>
        public static void WrongValueInput(ToolTip toolTip, Control inputControl)
        {
            Point location = GetControlLocation(inputControl);
            string alertMessage = ControlValidation.GetAlertMessage(inputControl);
            WrongValueInput(toolTip, inputControl, location, alertMessage, 2000);
        }//WrongValueInput
        
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="toolTip"></param>
        /// <param name="inputControl">Контрол ввода инф-ции.</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        public static void WrongValueInput(ToolTip toolTip, Control inputControl, string toolTipMessage)
        {
            Point location = GetControlLocation(inputControl);

            WrongValueInput(toolTip, inputControl, location, toolTipMessage, 2000);
        }//WrongValueInput
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param>
        public static void WrongValueInput(ToolTip toolTip, Control inputControl, string toolTipMessage, int toolTipShowTime)
        {
            Point location = GetControlLocation(inputControl);

            WrongValueInput(toolTip, inputControl, location, toolTipMessage, toolTipShowTime);
        }//WrongValueInput 
        /// <summary>
        ///  Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="toolTipLocation">Позиция где будет показано всплывающее сообщение</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param
        public static void WrongValueInput(ToolTip toolTip, Control inputControl, Point toolTipLocation, string toolTipMessage, int toolTipShowTime)
        {
            Control backControl = inputControl.Parent as Panel;
            Control starControl = FindStarLabel(inputControl); //Находим соответствующую контролу StarLabel.

            //Если StarControl есть, меняем его цвет.
            if (starControl != null)
                starControl.ForeColor = Color.Red;
            backControl.BackColor = Color.Red;

            toolTip.SetToolTip(inputControl, toolTipMessage);
            toolTip.Show(toolTipMessage, inputControl.FindForm(), toolTipLocation, toolTipShowTime);
        }//WrongValueInput

        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены корректные данные.
        /// </summary>
        /// <param name="toolTip"></param>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        public static void CorrectValueInput(ToolTip toolTip, Control inputControl)
        {
            Panel backPanel = inputControl.Parent as Panel;
            Label starLabel = FindStarLabel(inputControl); //Находим соответствующую контролу StarLabel.

            if (starLabel != null)
                starLabel.ForeColor = Color.Black;
            backPanel.BackColor = SystemColors.Control;
            toolTip.SetToolTip(inputControl, String.Empty);
        }//CorrectValueInput
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены корректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        public static void CorrectValueInput(ToolTip toolTip, Control inputControl, string toolTipMessage)
        {
            CorrectValueInput(toolTip, inputControl, toolTipMessage, 2000);
        }//CorrectValueInput
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены корректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param>
        public static void CorrectValueInput(ToolTip toolTip, Control inputControl, string toolTipMessage, int toolTipShowTime)
        {
            Control backControl = inputControl.Parent as Panel;
            Control starControl = FindStarLabel(inputControl); //Находим соответствующую контролу StarLabel.
            Point location = GetControlLocation(backControl);

            if (starControl != null)
                starControl.ForeColor = Color.Black;
            backControl.BackColor = SystemColors.Control;

            toolTip.SetToolTip(inputControl, toolTipMessage);
            toolTip.Show(toolTipMessage, inputControl.FindForm(), location, toolTipShowTime);
            //toolTip.Show(toolTipMessage, inputControl, toolTipShowTime);
        }//CorrectValueInput


        /// <summary>
        /// Возвращает позицию переданного контрола относительно формы.
        /// </summary>
        /// <param name="control">Контрол, позицию которого относительно формы, необх-мо найти.</param>
        /// <returns></returns>
        public static Point GetControlLocation(Control control)
        {
            //Смещается при BorderStyle.None
            Form form = control.FindForm();
            //Находим высоту заголовка. Почему-то она одинаковая при BorderStyle.None и других.
            int captionHeight = (form.FormBorderStyle != FormBorderStyle.None) ? SystemInformation.CaptionHeight : 0;

            var borderWidth = (form.Size.Width - form.ClientSize.Width) / 2;
            var borderHeight = (form.Size.Height - form.ClientSize.Height - captionHeight) / 2;
            //Находим абсолютную позицию заданной точки (контрола) на экране.
            var absPos = control.PointToScreen(new Point(0, 0));
            //Находим позицию заданной точки (контрола) относительно формы.
            var relPos = form.PointToClient(absPos);

            return new Point(relPos.X + borderWidth, relPos.Y + borderHeight + captionHeight);
        }//GetControlLocation

        /// <summary>
        /// Возвращает StarLabel ассоциируемый с переданным TextBox-ом.
        /// </summary>
        /// <param name="control">TextBox для которого находится StarLabel.</param>
        /// <returns></returns>
        public static Label FindStarLabel(Control control)
        {
            string controlFullType = control.GetType().ToString();                              //System.Windows.Forms.TextBox
            string controlType = controlFullType.Substring(controlFullType.LastIndexOf('.') + 1);  //TextBox 

            string fieldsBeginName = control.Name.Substring(0, control.Name.IndexOf(controlType)); // "FirstName"
            string starLabelName = fieldsBeginName + "StarLabel";                                  // "FirstName" + "StarLabel"
            return control.FindForm().Controls.Find(starLabelName, true).FirstOrDefault() as Label;
        }//FindStarLabel
        /// <summary>
        /// Возвращает BackPanel ассоциируемый с переданным TextBox-ом.
        /// </summary>
        /// <param name="control">TextBox для которого находится StarLabel.</param>
        /// <returns></returns>
        public static Panel FindBackPanel(Control control)
        {
            string controlFullType = control.GetType().ToString();                              //System.Windows.Forms.TextBox
            string controlType = controlFullType.Substring(controlFullType.LastIndexOf('.') + 1); //TextBox

            string fieldsBeginName = control.Name.Substring(0, control.Name.IndexOf(controlType));  // "FirstName"
            string starLabelName = fieldsBeginName + "BackPanel";                                 // "FirstName" + "BackPanel"
            return control.FindForm().Controls.Find(starLabelName, true).FirstOrDefault() as Panel;
        }//FindBackPanel

        /// <summary>
        /// Возвращает строку предупреждения найденную по имени контрола ассоциируемого с TextBox-ом.
        /// </summary>
        /// <param name="control">TextBox по имени ассоциируемого контроллера которого возвращается предупреждающая строка.</param>
        /// <returns></returns>
        public static string GetAlertMessage(Control control)
        {
            string controlFullType = control.GetType().ToString();                              //System.Windows.Forms.TextBox
            string controlType = controlFullType.Substring(controlFullType.LastIndexOf('.') + 1); //TextBox
            //Находим имя контрола ассоциируемого с переданным TextBox-ом.
            string fieldsBeginName = control.Name.Substring(0, control.Name.IndexOf(controlType));  // "FirstName"
            string labelName = fieldsBeginName + "Label";                                           // "FirstNameLabel"
            //Находим Label по найденному имени. 
            Label nameLabel = control.FindForm().Controls.Find(labelName, true).FirstOrDefault() as Label;
            //Корректируем выводимый текст.
            string correctLabelText = nameLabel.Text.Substring(0, nameLabel.Text.Length - 2);    //Вместо "Имя :" получаем "Имя".

            return String.Format("Заполните поле \"{0}\"", correctLabelText);
        }//GetToolTipAlertMessage

        ///// <summary>
        ///// Менят BackColor переданного контрола на красный на 200мс.
        ///// </summary>
        ///// <param name="control">Контрол BackColor которого будет изменен.</param>
        //public static void ControlBlink(Control control)
        //{
        //    if (control.BackColor == Color.Red)
        //        return;

        //    control.BackColor = Color.Red;

        //    var dt = new System.Windows.Threading.DispatcherTimer();

        //    dt.Interval = TimeSpan.FromMilliseconds(200);
        //    dt.Tick += delegate
        //    {
        //        control.BackColor = SystemColors.Control;
        //        dt.Stop();
        //    };

        //    dt.Start();
        //}//ControlBlink

        /// <summary>
        /// Вовращает true, если св-во Text переданного контрола пустое, иначе false. В зависимости от результата визуально выделяет StarLabel и BackPanel контрола.
        /// </summary>
        /// <param name="control">Проверяемый и визуально выделяемый контрол.</param>
        /// <returns></returns>
        public static bool IsInputControlEmpty(Control control, ToolTip toolTip)
        {
            Panel backPanel = control.Parent as Panel;
            Label starLabel = FindStarLabel(control); //Находим соответствующую контролу StarLabel.

            if (String.IsNullOrWhiteSpace(control.Text))
            {
                string alertMessage = GetAlertMessage(control);
                WrongValueInput(toolTip, control, alertMessage);
                return true;
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(toolTip, control);
                return false;
            }//else 
        }//IsInputControlEmpty
    }//ControlValidation


}//namespace
