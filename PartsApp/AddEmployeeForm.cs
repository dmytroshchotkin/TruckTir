﻿using System;
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
    public partial class AddEmployeeForm : Form
    {
        string beginFilePath = null;        //переменные не будут равны null если требуется скопировать файл в нужную папку.
        string endFilePath = null;


        public AddEmployeeForm()
        {
            InitializeComponent();
            //Устанавливаем значения для дат.
            int maxAge = 80, minAge = 16;
            birthDateTimePicker.MinDate = new DateTime(DateTime.Today.Year - maxAge, 1, 1);
            birthDateTimePicker.MaxDate = new DateTime(DateTime.Today.Year - minAge, 12, 31);

            hireDateTimePicker.MinDate = new DateTime(2000, 1, 1);
            hireDateTimePicker.MaxDate = DateTime.Today;
        }

        private void AddEmployeeForm_Load(object sender, EventArgs e)
        {
            bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
        }

        private void addContactInfoButton_Click(object sender, EventArgs e)
        {
            //Проверяем есть ли уже введенная информация. 
            if (contactInfoPanel.Visible == true)
                if (IsThereContactInfo() == true) return;

            contactInfoPanel.Visible = !contactInfoPanel.Visible;
            if (contactInfoPanel.Visible == false)
                bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
            else
                bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y + contactInfoPanel.Size.Height);
        }

        #region Методы проверки корректности ввода.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="backControl">Контрол исп-мый как рамка</param>
        /// <param name="starControl">Контрол указания обязательного для заполнения поля (звездочка)</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param>
        private void WrongValueInput(Control inputControl, Control backControl, Control starControl, string toolTipMessage, int toolTipShowTime)
        {
            starControl.ForeColor = backControl.BackColor = Color.Red;
            toolTip.SetToolTip(inputControl, toolTipMessage);
            toolTip.Show(toolTipMessage, this, backControl.Location, toolTipShowTime);
        }//wrongValueInput
        /// <summary>
        ///  Метод выдачи визуального сообщения о том что введены некорректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="backControl">Контрол исп-мый как рамка</param>
        /// <param name="starControl">Контрол указания обязательного для заполнения поля (звездочка)</param>
        /// <param name="toolTipLocation">Позиция где будет показано всплывающее сообщение</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param
        private void WrongValueInput(Control inputControl, Control backControl, Control starControl, Point toolTipLocation, string toolTipMessage, int toolTipShowTime)
        {
            starControl.ForeColor = backControl.BackColor = Color.Red;
            toolTip.SetToolTip(inputControl, toolTipMessage);
            toolTip.Show(toolTipMessage, this, toolTipLocation, toolTipShowTime);
        }//wrongValueInput
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены корректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="backControl">Контрол исп-мый как рамка</param>
        /// <param name="starControl">Контрол указания обязательного для заполнения поля (звездочка)</param>
        private void CorrectValueInput(Control inputControl, Control backControl, Control starControl)
        {
            starControl.ForeColor = Color.Black;
            backControl.BackColor = SystemColors.Control;
            toolTip.SetToolTip(inputControl, String.Empty);
        }//CorrectValueInput
        /// <summary>
        /// Метод выдачи визуального сообщения о том что введены корректные данные.
        /// </summary>
        /// <param name="inputControl">Контрол ввода инф-ции</param>
        /// <param name="backControl">Контрол исп-мый как рамка</param>
        /// <param name="starControl">Контрол указания обязательного для заполнения поля (звездочка)</param>
        /// <param name="toolTipMessage">Всплывающее сообщение.</param>
        /// <param name="toolTipShowTime">Длительность демонстрации всплывающего сообщения (Мс). Должно быть больше 0. </param>
        private void CorrectValueInput(Control inputControl, Control backControl, Control starControl, string toolTipMessage, int toolTipShowTime)
        {
            starControl.ForeColor = Color.Black;
            backControl.BackColor = SystemColors.Control;
            toolTip.SetToolTip(inputControl, toolTipMessage);
            toolTip.Show(toolTipMessage, this, backControl.Location, toolTipShowTime);
        }//CorrectValueInput

        /// <summary>
        /// Возвращает Id контактной информации если она введена, иначе возвращает null.
        /// </summary>
        /// <returns></returns>
        private int? GetContactInfoId()
        {
            //Если ContactInfoPanel развернута.
            if (contactInfoPanel.Visible == true && IsThereContactInfo() == true)
            {
                //Если есть введенная инф-ция
                ContactInfo contactInfo = new ContactInfo();
                foreach (var control in contactInfoPanel.Controls)
                {
                    if (control is TextBox)
                    {
                        var textBox = control as TextBox;
                        if (String.IsNullOrWhiteSpace(textBox.Text)) continue;

                        //Находим имя текущего контрола соответствующее имени свойства класса ContactInfo.
                        string propertyName = char.ToUpper(textBox.Name[0]).ToString() + textBox.Name.Substring(1, textBox.Name.IndexOf("TextBox") - 1);

                        //Присваиваем значение свойству propertyName из соответствующего textBox.
                        Type type = typeof(ContactInfo);
                        var property = type.GetProperty(propertyName);
                        property.SetValue(contactInfo, textBox.Text.Trim());
                    }//if
                }//foreach    
                //добавляем запись в таблицу ContactInfo.
                int contactInfoId = PartsDAL.AddContactInfo(contactInfo);
                return contactInfoId;
            }//if
            return null;
        }//GetContactInfoId
        /// <summary>
        /// Возвращает true если в contactInfoPanel введена какая-то инф-ция, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool IsThereContactInfo()
        {
            foreach (var control in contactInfoPanel.Controls)
            {
                if (control is TextBox)
                {
                    if (String.IsNullOrWhiteSpace((control as TextBox).Text) == false)
                    {
                        return true;
                    }//if
                }//if
            }//foreach
            return false;
        }//isThereContactInfo


        private void lastNameTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                WrongValueInput(lastNameTextBox, lastNameBackPanel, lastNameStarLabel, "Введите фамилию.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(lastNameTextBox, lastNameBackPanel, lastNameStarLabel);
            }//else
        }//lastNameTextBox_Leave

        private void firstNameTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                WrongValueInput(firstNameTextBox, firstNameBackPanel, firstNameStarLabel, "Введите имя.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(firstNameTextBox, firstNameBackPanel, firstNameStarLabel);
            }//else
        }//firstNameTextBox_Leave
        
        private void passportNumTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(passportNumTextBox.Text))
            {
                WrongValueInput(passportNumTextBox, passportNumBackPanel, passportNumStarLabel, "Введите серию и номер паспорта.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(passportNumTextBox, passportNumBackPanel, passportNumStarLabel);
            }//else
        }//passportNumTextBox_Leave

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                Point location = new Point(bottomPanel.Location.X + passwordBackPanel.Location.X, bottomPanel.Location.Y + passwordBackPanel.Location.Y);
                WrongValueInput(passwordTextBox, passwordBackPanel, passwordStarLabel, location, "Введите пароль", 3000);
                passwordAgainTextBox.Enabled = false;
            }//if
            else //если фамилия введена правильно
            {
                CorrectValueInput(passwordTextBox, passwordBackPanel, passwordStarLabel);
                passwordAgainTextBox.Enabled = true;
            }//else
        }//passwordTextBox_Leave

        private void passwordAgainTextBox_Leave(object sender, EventArgs e)
        {
            Point location = new Point(bottomPanel.Location.X + passwordAgainBackPanel.Location.X, bottomPanel.Location.Y + passwordAgainBackPanel.Location.Y);
            //Проверяем повторный ввод пароля на корректность.
            if (String.IsNullOrWhiteSpace(passwordAgainTextBox.Text))
            {                
                WrongValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel, location, "Повторите пароль", 3000);
            }//if
            else if (passwordAgainTextBox.Text != passwordTextBox.Text)
            {
                WrongValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel, location, "Пароли не совпадают", 3000);
            }
            else
            {
                CorrectValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel);
            }//else
        }

        private void accessLayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accessLayerComboBox.SelectedIndex == -1)
            {
                Point location = new Point(bottomPanel.Location.X + accessLayerBackPanel.Location.X, bottomPanel.Location.Y + accessLayerBackPanel.Location.Y);
                WrongValueInput(accessLayerComboBox, accessLayerBackPanel, accessLayerStarLabel, location, "Выберите уровень доступа данного сотрудника.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(accessLayerComboBox, accessLayerComboBox, accessLayerComboBox);
            }//else
        }// accessLayerComboBox_SelectedIndexChanged






///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Событие нажатия на кнопку для выбора фото сотрудника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEmployeePhotoButton_Click(object sender, EventArgs e)
        {
            if (photoOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(photoOpenFileDialog.FileName);

                toolTip.SetToolTip(photoPictureBox, fileName);
                //Проверяем находится ли фото в нужной папке. 
                string path = @"Сотрудники\" + toolTip.GetToolTip(photoPictureBox);

                if (System.IO.Path.GetFullPath(path) == photoOpenFileDialog.FileName)
                {
                    //Если фото выбрано, то подгоняем его размер под PictureBox и добавляем всплывающую подсказку.
                    photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                    toolTip.SetToolTip(photoPictureBox, fileName);
                }//if
                //если выбранное фото не находится в нужной папке. 
                else
                    if (System.IO.File.Exists(System.IO.Path.GetFullPath(path))) //проверяем есть ли фото с таким именем в нужной папке. 
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(System.IO.Path.GetFullPath(path)), photoPictureBox.Size);
                        //Если файл в нужной папке не является подходящим, то очищаем pictureBox.
                        if (DialogResult.Cancel == MessageBox.Show("Этот файл или файл с таким именем уже существует в папке \"Сотрудники\".\nЕсли данное фото, является правильным, нажмите \"Ok\".\nИначе нажмите \"Отмена\" измените имя выбираемого файла и попробуйте ещё раз.", "Совпадение имен файлов", MessageBoxButtons.OKCancel))
                            deselectToolStripMenuItem_Click(sender, e);
                    }//if
                    //Если файл не находится в нужной папке, и при этом нет совпадения имен, копируем его.
                    else
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                        beginFilePath = photoOpenFileDialog.FileName;
                        endFilePath = System.IO.Path.GetFullPath(path);
                    }//else

            }//if
        }//addEmployeePhotoButton_Click
        /// <summary>
        /// Событие для отмены выбора фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            photoPictureBox.Image = null;
            photoOpenFileDialog.FileName = String.Empty;
            toolTip.SetToolTip(photoPictureBox, String.Empty);
        }


        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Проверяем корректность ввода необходимых данных.
                lastNameTextBox_Leave(sender, e);                
                firstNameTextBox_Leave(sender, e);
                passwordTextBox_Leave(sender, e);
                passwordAgainTextBox_Leave(sender, e);
                accessLayerComboBox_SelectedIndexChanged(sender, e);
                //Если все корректно.
                if (articulTextBoxBackPanel.BackColor != Color.Red && titleTextBoxBackPanel.BackColor != Color.Red
                    && unitComboBoxBackPanel.BackColor != Color.Red)
                {
                    SparePart sparePart = new SparePart();
                    //Проверяем наличие фото.
                    if (photoPictureBox.Image == null)
                        sparePart.Photo = null;
                    else
                    {
                        if (beginFilePath != null) //если false значит фото уже есть в нужной папке и мы просто записываем относительный путь иначе вначале копируем файл.  
                        {
                            System.IO.File.Copy(beginFilePath, endFilePath);
                        }
                        sparePart.Photo = @"Товар\" + toolTip.GetToolTip(photoPictureBox);
                    }//else

                    sparePart.Articul = articulTextBox.Text.Trim();
                    sparePart.Title = titleTextBox.Text.Trim();
                    if (String.IsNullOrWhiteSpace(descrRichTextBox.Text) == false)
                        sparePart.Description = descrRichTextBox.Text.Trim();
                    else sparePart.Description = null;
                    sparePart.ExtInfoId = null;
                    //добаляем manufacturer
                    if (String.IsNullOrWhiteSpace(manufacturerTextBox.Text))
                        sparePart.ManufacturerId = null;
                    else //Если такого ManufacturerName нет в базе, значит добавить.
                    {
                        if (PartsDAL.FindManufacturersIdByName(manufacturerTextBox.Text.Trim()).Count == 0)
                            sparePart.ManufacturerId = PartsDAL.AddManufacturer(manufacturerTextBox.Text.Trim());
                        else
                            sparePart.ManufacturerId = PartsDAL.FindManufacturersIdByName(manufacturerTextBox.Text.Trim())[0]; //!!! Кроется опасность путаницы в случае одинакового имени производителей, необходимо будет внести добавление в базу для избежания потенциальной угрозы!
                    }//else
                    //Вставляем ед. изм. 
                    //if (unitComboBox.DropDownStyle == ComboBoxStyle.DropDown) //если вставляется новое значение в бд.
                    //PartsDAL.AddUnitOfMeasure();
                    sparePart.Unit = unitComboBox.SelectedValue.ToString();

                    //Проверяем добавляется новая ед. товара или модиф-ся уже сущ-щая.
                    if (editSparePart == null)
                        PartsDAL.AddSparePart(sparePart);
                    else
                    {
                        sparePart.SparePartId = editSparePart.SparePartId;
                        PartsDAL.UpdateSparePart(sparePart);
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }//if
            }//if
        }

        

        
        

        

        




    }//AddEmployeeForm
}//namespace
