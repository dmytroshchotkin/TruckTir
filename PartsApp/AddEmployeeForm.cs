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
using Models.Helper;

namespace PartsApp
{
    public partial class AddEmployeeForm : Form
    {
        readonly Employee _editEmployee;

        const string employeePhotoFolder = @"Сотрудники\";
        readonly DateTime companyFoundingDate = new DateTime(2000, 1, 1);
        const int minAge = 16, maxAge = 80;


        public AddEmployeeForm()
        {
            InitializeComponent();
            //Устанавливаем значения для дат.            
            birthDateTimePicker.MinDate = new DateTime(DateTime.Today.Year - maxAge, 1, 1);
            birthDateTimePicker.MaxDate = new DateTime(DateTime.Today.Year - minAge, 12, 31);
            birthDateTimePicker.ValueChanged += birthDateTimePicker_ValueChanged;

        }

        public AddEmployeeForm(Employee editEmployee)
        {
            InitializeComponent();

            _editEmployee = editEmployee;
            FillTheForm(_editEmployee);
            birthDateTimePicker.ValueChanged += birthDateTimePicker_ValueChanged;
        }

        private void AddEmployeeForm_Load(object sender, EventArgs e)
        {
            bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
        }

        private void addContactInfoButton_Click(object sender, EventArgs e)
        {
            //Проверяем есть ли уже введенная информация. 
            if (contactInfoPanel.Visible == true)
            {
                if (IsThereContactInfo() == true)
                {
                    return;
                }
            }

            contactInfoPanel.Visible = !contactInfoPanel.Visible;
            if (contactInfoPanel.Visible == false)
            {
                bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
            }
            else
            {
                bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y + contactInfoPanel.Size.Height);
            }
        }

        #region Методы проверки корректности ввода.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void lastNameTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                WrongValueInput(lastNameTextBox, lastNameBackPanel, lastNameStarLabel, "Введите фамилию.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(lastNameTextBox, lastNameBackPanel, lastNameStarLabel);
            }
        }

        private void firstNameTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                WrongValueInput(firstNameTextBox, firstNameBackPanel, firstNameStarLabel, "Введите имя.", 3000);
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(firstNameTextBox, firstNameBackPanel, firstNameStarLabel);
            }
        }

        private void passportNumTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(passportNumTextBox.Text))
            {
                CorrectValueInput(passportNumTextBox, passportNumBackPanel, passportNumBackPanel);
            }
            else
            {
                if (PartsDAL.FindEmployees().Where(empl => empl.PassportNum == passportNumTextBox.Text.Trim()).Count() > 0) //Если такой номер паспорта уже имеется в базе.
                {
                    //Если редактируется существующий сотрудник, и паспортные данные не изменены, то всё корректно. Иначе проверяем на совпадение с другими паспортными данными.
                    if (_editEmployee != null && _editEmployee.PassportNum == passportNumTextBox.Text.Trim())
                    {
                        CorrectValueInput(passportNumTextBox, passportNumBackPanel, passportNumBackPanel);
                    }
                    else
                    {
                        WrongValueInput(passportNumTextBox, passportNumBackPanel, passportNumBackPanel, "Такие паспортные данные уже имеются в базе.", 3000);
                    }
                }
                else//если фамилия введена правильно
                {
                    CorrectValueInput(passportNumTextBox, passportNumBackPanel, passportNumBackPanel);
                }
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            passwordAgainTextBox.Enabled = true;

        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                Point location = new Point(bottomPanel.Location.X + passwordBackPanel.Location.X, bottomPanel.Location.Y + passwordBackPanel.Location.Y);
                WrongValueInput(passwordTextBox, passwordBackPanel, passwordStarLabel, location, "Введите пароль", 3000);
                passwordAgainTextBox.Clear();
                passwordAgainTextBox.Enabled = false;
            }
            else //если фамилия введена правильно
            {
                CorrectValueInput(passwordTextBox, passwordBackPanel, passwordStarLabel);
            }
        }

        private void passwordAgainTextBox_Leave(object sender, EventArgs e)
        {
            Point location = new Point(bottomPanel.Location.X + passwordAgainBackPanel.Location.X, bottomPanel.Location.Y + passwordAgainBackPanel.Location.Y);
            //Проверяем повторный ввод пароля на корректность.
            if (String.IsNullOrWhiteSpace(passwordAgainTextBox.Text))
            {
                WrongValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel, location, "Повторите пароль", 3000);
            }
            else if (passwordAgainTextBox.Text != passwordTextBox.Text)
            {
                WrongValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel, location, "Пароли не совпадают", 3000);
            }
            else
            {
                CorrectValueInput(passwordAgainTextBox, passwordAgainBackPanel, passwordAgainStarLabel);
            }
        }

        private void loginTextBox_Leave(object sender, EventArgs e)
        {
            Point location = new Point(bottomPanel.Location.X + loginBackPanel.Location.X, bottomPanel.Location.Y + loginBackPanel.Location.Y);
            if (String.IsNullOrWhiteSpace(loginTextBox.Text))
            {
                WrongValueInput(loginTextBox, loginBackPanel, loginStarLabel, location, "Введите имя (логин) учетной записи.", 3000);
            }
            else
            {
                if (PartsDAL.FindEmployees().Where(empl => empl.Login == loginTextBox.Text.Trim()).Count() > 0) //Если такой логин уже имеется в базе.
                {
                    //Если редактируется существующий сотрудник, и логин не изменен, то всё корректно. Иначе проверяем на совпадение с другими логинами в базе.
                    if (_editEmployee != null && _editEmployee.Login == loginTextBox.Text.Trim())
                    {
                        CorrectValueInput(loginTextBox, loginBackPanel, loginStarLabel);
                    }
                    else
                    {
                        WrongValueInput(loginTextBox, loginBackPanel, loginStarLabel, location, "Такой логин уже существует, введите другой.", 3000);
                    }
                }
                else//если фамилия введена правильно
                {
                    CorrectValueInput(loginTextBox, loginBackPanel, loginStarLabel);
                }
            }
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
                CorrectValueInput(accessLayerComboBox, accessLayerBackPanel, accessLayerStarLabel);
            }
        }

        private void birthDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            hireDateTimePicker.Enabled = true;

            //Возраст сотрудника принимаемого на работу должен быть не меньше minAge, поэтому ставим ограничения на возм-ть выбора даты. 
            if (companyFoundingDate.Year - birthDateTimePicker.Value.Year < minAge)
            {
                hireDateTimePicker.MinDate = new DateTime(birthDateTimePicker.Value.Year + minAge, 1, 1);
            }
            else
            {
                hireDateTimePicker.MinDate = companyFoundingDate;
            }
            hireDateTimePicker.MaxDate = DateTime.Today;
        }



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
        }

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
        }

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
        }

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
        }

        /// <summary>
        /// Возвращает Id контактной информации если она введена, иначе возвращает null.
        /// </summary>
        /// <returns></returns>
        private ContactInfo GetContactInfo()
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
                        if (String.IsNullOrWhiteSpace(textBox.Text))
                        {
                            continue;
                        }

                        //Находим имя текущего контрола соответствующее имени свойства класса ContactInfo.
                        string propertyName = char.ToUpper(textBox.Name[0]).ToString() + textBox.Name.Substring(1, textBox.Name.IndexOf("TextBox") - 1);

                        //Присваиваем значение свойству propertyName из соответствующего textBox.
                        Type type = typeof(ContactInfo);
                        var property = type.GetProperty(propertyName);
                        property.SetValue(contactInfo, textBox.Text.Trim());
                    }
                }
                return contactInfo;
            }
            return null;
        }

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
                    }
                }
            }
            return false;
        }



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
                string path = employeePhotoFolder + toolTip.GetToolTip(photoPictureBox);

                if (System.IO.Path.GetFullPath(path) == photoOpenFileDialog.FileName)
                {
                    //Если фото выбрано, то подгоняем его размер под PictureBox и добавляем всплывающую подсказку.
                    photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                    toolTip.SetToolTip(photoPictureBox, fileName);
                }                //если выбранное фото не находится в нужной папке. 
                else
                {
                    if (System.IO.File.Exists(System.IO.Path.GetFullPath(path))) //проверяем есть ли фото с таким именем в нужной папке. 
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(System.IO.Path.GetFullPath(path)), photoPictureBox.Size);
                        //Если файл в нужной папке не является подходящим, то очищаем pictureBox.
                        if (DialogResult.Cancel == MessageBox.Show("Этот файл или файл с таким именем уже существует в папке \"Сотрудники\".\nЕсли данное фото, является правильным, нажмите \"Ok\".\nИначе нажмите \"Отмена\" измените имя выбираемого файла и попробуйте ещё раз.", "Совпадение имен файлов", MessageBoxButtons.OKCancel))
                        {
                            deselectToolStripMenuItem_Click(sender, e);
                        }
                    }                    //Если файл не находится в нужной папке, и при этом нет совпадения имен, копируем его.
                    else
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                        //записываем конечный путь файла всв-во tag.
                        photoPictureBox.Tag = System.IO.Path.GetFullPath(path);
                    }
                }
            }
        }

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

        #region Методы связанные с модификацией и просмотром инф-ции о сотруднике.

        /// <summary>
        /// Заполняет форму информацией о заданном сотруднике.
        /// </summary>
        /// <param name="employee">Сотрудник чьей информацией заполняется форма.</param>
        private void FillTheForm(Employee employee)
        {            
            lastNameTextBox.Text = employee.LastName;
            firstNameTextBox.Text = employee.FirstName;
            middleNameTextBox.Text = employee.MiddleName;

            hireDateLabel.Text += employee.HireDate?.ToString("d");
            hireDateTimePicker.Value = (DateTime)employee.HireDate;
            hireDateTimePicker.Visible = false;

            birthDateTimePicker.Value = (DateTime)employee.BirthDate;
            if (employee.DismissalDate != default)
            {
                birthDateLabel.Text += employee.BirthDate?.ToString("d");
                birthDateTimePicker.Visible = false;
            }

            descrRichTextBox.Text = employee.Note;
            passportNumTextBox.Text = employee.PassportNum;
            titleTextBox.Text = employee.Title;
            loginTextBox.Text = employee.Login;
            accessLayerComboBox.SelectedItem = employee.AccessLayer;
            FillTheContactInfoPanel(employee.ContactInfo); //Заполняем контактную информацию.
                                                           //Проверяем наличие фото.
                                                           //photoPictureBox.Image = (employee.Photo != null) ? new Bitmap(Image.FromFile(employee.Photo), photoPictureBox.Size) : null;
            if (employee.Photo != null)
            {
                if (System.IO.File.Exists(System.IO.Path.GetFullPath(employee.Photo)))
                {
                    photoPictureBox.Image = new Bitmap(Image.FromFile(employee.Photo), photoPictureBox.Size);
                    toolTip.SetToolTip(photoPictureBox, System.IO.Path.GetFileName(employee.Photo));
                }
            }//else //если путь фото указан, но такого фото уже нет в папке.
             //{
             //   employee.Photo = null;                 
             //}            }


            if (employee.DismissalDate != default)
            {
                dismissalDateLabel.Text += employee.DismissalDate?.ToString("d");
                dismissalDateLabel.Visible = true;

                DisableAccessControls();
            }
            else
            {
                SetTheAccessLayerConstraints(employee);                    
            }   
        }

        /// <summary>
        /// Отключает элементы управления, связанные с установкой / проверкой логина и пароля
        /// </summary>
        private void DisableAccessControls()
        {
            accessLayerComboBox.Enabled = false;
            loginBackPanel.Enabled = false;
            loginLabel.Enabled = false;
            loginStarLabel.Enabled = false;
            passwordAgainBackPanel.Enabled = false;
            passwordBackPanel.Enabled = false;
            accessLayerLabel.Enabled = false;
            passwordAgainLabel.Enabled = false;
            passwordLabel.Enabled = false;
            passwordStarLabel.Enabled = false;
            accessLayerStarLabel.Enabled = false;
            passwordAgainStarLabel.Enabled = false;
        }

        /// <summary>
        /// Метод заполнения ContactInfoPanel информацией из заданного ContactInfo.
        /// </summary>
        /// <param name="contactInfo">Oбъект по которому заполняются поля в ContactInfoPanel.</param>
        private void FillTheContactInfoPanel(ContactInfo contactInfo)
        {
            if (contactInfo != null)
            {
                countryTextBox.Text = contactInfo.Country;
                regionTextBox.Text = contactInfo.Region;
                cityTextBox.Text = contactInfo.City;
                streetTextBox.Text = contactInfo.Street;
                houseTextBox.Text = contactInfo.House;
                roomTextBox.Text = contactInfo.Room;
                phoneTextBox.Text = contactInfo.Phone;
                ExtPhoneTextBox.Text = contactInfo.ExtPhone;
                emailTextBox.Text = contactInfo.Email; ;
                websiteTextBox.Text = contactInfo.Website;
            }
        }

        /// <summary>
        /// Задаёт ограничения модификации формы исходя из уровня доступа переданного сотрудника.
        /// </summary>
        /// <param name="employee">Сотрудник, исходя из уровня доступа которого задаются ограничения.</param>
        private void SetTheAccessLayerConstraints(Employee employee)
        {
            //Если редактируемый юзер это и есть тот кто сейчас авторизован
            if (employee == Form1.CurEmployee)
            {
                //Если права "Обычные" -- может редактировать только пароль и логин.
                if (employee.AccessLayer == Employee.AccessLayers.User.ToDescription())
                {
                    foreach (Control control in this.Controls)
                    {
                        control.Enabled = false;
                    }

                    bottomPanel.Enabled = true;
                    accessLayerComboBox.Enabled = descrRichTextBox.Visible = descrLabel.Visible = false;
                }
                else //если права "Админ" -- может редактировать всё.
                {
                    passwordTextBox.Text = passwordAgainTextBox.Text = employee.Password;
                }
            }
            else //Если редактируемый юзер не является авторизованным юзером
            {
                //если права "Админ" -- может редактировать всё, кроме пароля и логина.
                if (employee.AccessLayer == Employee.AccessLayers.Admin.ToDescription())
                {
                    passwordTextBox.Text = passwordAgainTextBox.Text = employee.Password;
                    loginTextBox.Visible = loginLabel.Visible = loginStarLabel.Visible = false;
                    passwordTextBox.Visible = passwordAgainTextBox.Visible = false;
                    passwordAgainLabel.Visible = passwordLabel.Visible = false;
                    passwordAgainStarLabel.Visible = passwordStarLabel.Visible = false;
                }
            }
        }

        /// <summary>
        /// Метод обновляющий данные переданного сотрудника в базе.
        /// </summary>
        /// <param name="employee">Сотрудник чьи данные обновляются.</param>
        private void UpdateEmployee(Employee employee)
        {
            //Если редактируемый юзер это и есть тот кто сейчас авторизован
            if (employee.EmployeeId == Form1.CurEmployee.EmployeeId)
            {
                //Если права "Админ"  
                if (Form1.CurEmployee.AccessLayer == Employee.AccessLayers.Admin.ToDescription())
                {
                    //Если пароль не менялся, обновляем без пароля, иначе обновляем полностью.
                    if (passwordTextBox.Text.Trim() == Form1.CurEmployee.Password)
                    {
                        PartsDAL.UpdateEmployeeWithoutPassword(employee);
                    }
                    else
                    {
                        PartsDAL.UpdateEmployee(employee);
                    }
                }                //если права "Обычные"
                else
                {
                    //Если введен новый пароль, то обновляем его в базе, иначе ничего не делаем.
                    if (passwordTextBox.Text.Trim() != Form1.CurEmployee.Password)
                    {
                        PartsDAL.UpdateEmployee(employee);
                    }
                }
                Form1.CurEmployee = employee;
            }
            else //Если редактируемый юзер не является авторизованным юзером
            {
                //если права "Админ" 
                if (Form1.CurEmployee.AccessLayer == Employee.AccessLayers.Admin.ToDescription())
                {
                    PartsDAL.UpdateEmployeeWithoutPassword(employee);
                }
            }
        }
        #endregion

        /// <summary>
        /// Заполняет объект типа Employee информацией из формы. 
        /// </summary>
        /// <param name="employee">Сотрудник, который будет заполнен инф-цией из формы.</param>
        private Employee GetEmployeeFromForm()
        {
            string photoPath = null;            //Проверяем наличие фото.
            if (photoPictureBox.Image != null)
            {
                if (photoPictureBox.Tag != null) //если false значит фото уже есть в нужной папке и мы просто записываем относительный путь иначе сначала копируем файл.  
                {
                    string destFilePath = photoPictureBox.Tag as string;
                    System.IO.File.Copy(photoOpenFileDialog.FileName, destFilePath);
                }
                photoPath = employeePhotoFolder + toolTip.GetToolTip(photoPictureBox);
            }
            Employee employee = new Employee
            (
                employeeId: 0,
                photo: photoPath,
                lastName: lastNameTextBox.Text.Trim(),
                firstName: firstNameTextBox.Text.Trim(),
                middleName: (!String.IsNullOrWhiteSpace(middleNameTextBox.Text)) ? middleNameTextBox.Text.Trim() : null,
                birthDate: birthDateTimePicker.Value,
                hireDate: hireDateTimePicker.Value,
                dismissalDate: null,
                note: (!String.IsNullOrWhiteSpace(descrRichTextBox.Text)) ? descrRichTextBox.Text.Trim() : null,
                passportNum: (!String.IsNullOrWhiteSpace(passportNumTextBox.Text)) ? passportNumTextBox.Text.Trim() : null,
                title: (!String.IsNullOrWhiteSpace(titleTextBox.Text)) ? titleTextBox.Text.Trim() : null,
                accessLayer: accessLayerComboBox.SelectedItem as string,
                contactInfo: GetContactInfo(),
                login: loginTextBox.Text.Trim(),
                password: PasswordClass.GetHashString(passwordTextBox.Text.Trim())//получаем хэш введенного пароля.
            );

            return employee;
        }

        /// <summary>
        /// Возвращает true если все необходимые данные введены корректно, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool CheckAllConditionsForWrightValues()
        {
            if (_editEmployee?.DismissalDate.HasValue == true && _editEmployee.DismissalDate != default)
            {
                return true;
            }

            //Проверяем корректность ввода необходимых данных.
            lastNameTextBox_Leave(null, null);
            firstNameTextBox_Leave(null, null);

            //passportNumTextBox_Leave  (null, null);
            loginTextBox_Leave(null, null);
            passwordTextBox_Leave(null, null);
            passwordAgainTextBox_Leave(null, null);
            accessLayerComboBox_SelectedIndexChanged(null, null);

            //Проверяем удовлетворяют ли они всем условиям.
            if (lastNameBackPanel.BackColor != Color.Red && firstNameBackPanel.BackColor != Color.Red
                && passwordBackPanel.BackColor != Color.Red && passwordAgainBackPanel.BackColor != Color.Red
                && accessLayerBackPanel.BackColor != Color.Red && passportNumBackPanel.BackColor != Color.Red
                && loginBackPanel.BackColor != Color.Red)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CheckAllConditionsForWrightValues() == true)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Employee employee = GetEmployeeFromForm();
                    try
                    {                
                        if (_editEmployee is null)
                        {
                            PartsDAL.AddEmployee(employee);
                        }
                        else
                        {                            
                            employee.EmployeeId = _editEmployee.EmployeeId;
                            employee.DismissalDate = _editEmployee.DismissalDate;
                            UpdateEmployee(employee);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
/*http://www.internet-technologies.ru/articles/article_1807.html -- шифрование.*/