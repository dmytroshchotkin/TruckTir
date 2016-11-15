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
    public partial class AddContragentForm : Form
    {
        IContragent _contragent;
        /// <summary>
        /// тип контрагента на русском.
        /// </summary>
        string _contragentType;

        public AddContragentForm(IContragent contragent)
        {
            InitializeComponent();

            _contragent = contragent;
            _contragentType = (_contragent is Supplier) ? "поставщик" : "клиент";
        }//

        private void AddcontragentForm_Load(object sender, EventArgs e)
        {
            
            this.Text = String.Format("Форма {0} {1}а", (_contragent.ContragentId == 0) ? "добавления" : "редактирования",_contragentType);
            descrLabel.Text += String.Format("{0}е :", _contragentType);

            bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
            codeMaskedTextBox.SelectionStart = 1; 
           
            //Если у переданного объекта задан Id, то происходит редактирование контрагента.
            if (_contragent.ContragentId != 0)
                FillFormFromObject();//Заполняем форму инф-цией.
        }//AddcontragentForm_Load

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
        }//addContactInfoButton_Click

        private void contragentNameTextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(contragentNameTextBox.Text))
            {
                IContragent contragent = (_contragent is Customer) ? PartsDAL.FindCustomers(contragentNameTextBox.Text.Trim())
                                                                   : PartsDAL.FindSuppliers(contragentNameTextBox.Text.Trim());

                string text = contragentNameTextBox.Text.Trim().ToLower();
                //Если контрагент с таким именем уже есть в базе и это не его редактирование, выдаём ошибку.
                if ((_contragent.ContragentId != 0 && _contragent.ContragentName.ToLower() == text) || contragent == null)
                    ControlValidation.CorrectValueInput(toolTip, contragentNameTextBox);
                else
                    ControlValidation.WrongValueInput(toolTip, contragentNameTextBox, String.Format("Введите другое название или ФИО {0}а", _contragentType));
            }//if
            else //если название введено некорректно
                ControlValidation.WrongValueInput(toolTip, contragentNameTextBox);
        }//contragentNameTextBox_Leave

        /// <summary>
        /// Событие для установления каретки в начало codeMaskedTextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void codeMaskedTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            //Если клик производится по пустой области, каретка передвигается к концу ранее введенного текста
            //или в начало textbox-а, если ранее ничего введено не было.
            //Если клик производится по уже заполненной области, то каретка там и остаётся.
            if (codeMaskedTextBox.SelectionStart > codeMaskedTextBox.Text.Length)
                codeMaskedTextBox.SelectionStart = codeMaskedTextBox.Text.Length;                        
        }//codeMaskedTextBox_MouseClick

        private void codeMaskedTextBox_Leave(object sender, EventArgs e)
        {
            if (codeMaskedTextBox.Text != String.Empty)
            {
                //Проверяем корректность ввода.
                if (codeMaskedTextBox.MaskCompleted == true && codeMaskedTextBox.Text.Length != 9)
                {

                    //Проверяем существует ли уже такой code в базе.
                    if (PartsDAL.IsSupplierCodeExist(codeMaskedTextBox.Text) == true)
                    {
                        codeBackPanel.BackColor = Color.Red;
                        toolTip.SetToolTip(codeMaskedTextBox, "Такой ИНН/ОКПО уже есть в базе!");
                        toolTip.Show("Такой ИНН/ОКПО уже есть в базе!", this, codeBackPanel.Location, 5000);
                    }
                    else
                    {
                        codeBackPanel.BackColor = SystemColors.Control;
                        toolTip.SetToolTip(codeMaskedTextBox, String.Empty);
                    }
                }//if
                else
                {
                    codeBackPanel.BackColor = Color.Red;
                    toolTip.SetToolTip(codeMaskedTextBox, "Введенный ИНН/ОКПО является некорректным!");
                    toolTip.Show("Введенный ИНН/ОКПО является некорректным!", this, codeBackPanel.Location, 5000);
                }
            }//if
            else 
            {
                codeBackPanel.BackColor = SystemColors.Control;
                toolTip.SetToolTip(codeMaskedTextBox, String.Empty);
            }
        }//codeMaskedTextBox_Leave




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
                    /*ERROR!!! Рефлексия не нужна.*/
                    if (control is TextBox)
                    {
                        var textBox = control as TextBox;
                        if (String.IsNullOrWhiteSpace(textBox.Text)) continue;

                        //Находим имя текущего контрола соответствующее имени свойства класса ContactInfo.
                        string propertyName = char.ToUpper(textBox.Name[0]).ToString() + textBox.Name.Substring(1, textBox.Name.IndexOf("TextBox")-1);

                        //Присваиваем значение свойству propertyName из соответствующего textBox.
                        Type type = typeof(ContactInfo);
                        System.Reflection.PropertyInfo property = type.GetProperty(propertyName);
                        property.SetValue(contactInfo, textBox.Text.Trim());
                    }//if
                }//foreach    

                return contactInfo;
            }//if
            return null;
        }//GetContactInfo

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

        /// <summary>
        /// Заполняет форму инф-цией переданного в конст-ор объекта.
        /// </summary>
        private void FillFormFromObject()
        {
            //Заполняем форму данными объекта  
            contragentNameTextBox.Text =   _contragent.ContragentName; 
            codeMaskedTextBox.Text     =   _contragent.Code; 
            entityComboBox.Text        =   _contragent.Entity; 
            descrRichTextBox.Text      =   _contragent.Description;
            FillTheContactInfoPanel(_contragent.ContactInfo);           
        }//FillFormFromObject

        /// <summary>
        /// Метод заполнения ContactInfoPanel информацией из заданного ContactInfo.
        /// </summary>
        /// <param name="contactInfo">Oбъект по которому заполняются поля в ContactInfoPanel.</param>
        private void FillTheContactInfoPanel(ContactInfo contactInfo)
        {
            if (contactInfo != null)
            {
                countryTextBox.Text  = contactInfo.Country;
                regionTextBox.Text   = contactInfo.Region;
                cityTextBox.Text     = contactInfo.City;
                streetTextBox.Text   = contactInfo.Street;
                houseTextBox.Text    = contactInfo.House;
                roomTextBox.Text     = contactInfo.Room;
                phoneTextBox.Text    = contactInfo.Phone;
                extPhoneTextBox.Text = contactInfo.ExtPhone;
                emailTextBox.Text    = contactInfo.Email; ;
                websiteTextBox.Text  = contactInfo.Website;
            }//if
        }//FillTheContactInfoPanel   

        /// <summary>
        /// Возвращает объект заполненный данными с формы.
        /// </summary>
        /// <returns></returns>
        private IContragent GetContragentFromForm()
        {            
            //Находим данные с формы.
            int id               = _contragent.ContragentId;
            string name          = contragentNameTextBox.Text.Trim();
            string code          = (codeMaskedTextBox.Text == String.Empty) ? null : codeMaskedTextBox.Text;
            string entity        = (entityComboBox.SelectedItem != null) ? entityComboBox.Text : null;
            string description   = (String.IsNullOrWhiteSpace(descrRichTextBox.Text)) ? null : descrRichTextBox.Text.Trim();
            ContactInfo contInfo = GetContactInfo();

            //возвращаем объект в зависимости от его типа.
            return (_contragent is Supplier) ? (IContragent) new Supplier(id, name, code, entity, contInfo, description)
                                             : (IContragent) new Customer(id, name, code, entity, contInfo, description);

        }//GetContragentFromForm()

        /// <summary>
        /// Возвращает true если все обязательные поля корректно заполнены, иначе false.
        /// </summary>
        /// <returns></returns>
        private bool IsRequiredFieldsValid()
        {
            ////Находим все BackPanel-контролы на форме. 
            List<Control> curAccBackControls = this.GetAllControls(typeof(Panel), "BackPanel");

            ////Проверяем все необходимые контролы.
            //curAccBackControls.ForEach(backPanel => ControlValidation.IsInputControlEmpty(backPanel.Controls[0], toolTip));

            contragentNameTextBox_Leave(null, null);
            codeMaskedTextBox_Leave(null, null);

            //Если хоть один не прошел валидацию, возв-ем false.
            return !curAccBackControls.Any(backPanel => backPanel.BackColor == Color.Red);
        }//IsRequiredAddingAreaFieldsValid



        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }//if
        }//CancelButton_MouseClick

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                //Если все данные введены корректно
                if (IsRequiredFieldsValid())
                {             
                    //Присваиваем объект заполненный данными с формы.
                    _contragent = GetContragentFromForm();

                    //Добавляем новую запись или редактируем существующую.
                    if (_contragent.ContragentId != 0)
                        PartsDAL.AddContragent(_contragent);
                    else
                        PartsDAL.UpdateContragent(_contragent);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }//if
            }//if
        }//OkButton_MouseClick
        

    }//AddcontragentForm

}//namespace

//Сделать contactInfoPanel -- кастомным контролом.