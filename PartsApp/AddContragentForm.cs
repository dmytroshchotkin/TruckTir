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
    public partial class AddContragentForm : Form
    {
        IContragent _contragent;

        public AddContragentForm(IContragent contragent)
        {
            InitializeComponent();

            _contragent = contragent;
            string contragentType = (contragent is Supplier) ? "поставщик" : "клиент";
            this.Text = String.Format("Форма добавления нового {0}а", contragentType);
            descrLabel.Text += String.Format("{0}е :", contragentType);
        }
        private void AddcontragentForm_Load(object sender, EventArgs e)
        {
            bottomPanel.Location = new Point(bottomPanel.Location.X, bottomPanel.Location.Y - contactInfoPanel.Size.Height);
            codeMaskedTextBox.SelectionStart = 1;            
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

        private void contragentNameTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(contragentNameTextBox.Text))
            {
                contragentNameStarLabel.ForeColor = contragentNameBackPanel.BackColor = Color.Red;

                toolTip.SetToolTip(contragentNameTextBox, "Введите название компании или ФИО поставщика");
                toolTip.Show("Введите название компании или ФИО поставщика", this, contragentNameBackPanel.Location, 3000);
            }
            else //если название введено корректно
            {
                IContragent contragent = (_contragent is Customer) ? PartsDAL.FindCustomers(contragentNameTextBox.Text.Trim()) 
                                                                   : PartsDAL.FindSuppliers(contragentNameTextBox.Text.Trim());  
                //проверяем есть ли уже такое ContragentName в базе.
                if (contragent != null)
                {
                    contragentNameStarLabel.ForeColor = contragentNameBackPanel.BackColor = Color.Red;

                    toolTip.SetToolTip(contragentNameTextBox, "Введите другое название компании или ФИО поставщика");
                    toolTip.Show("Такое имя(название) уже есть в базе.", this, contragentNameBackPanel.Location, 3000);
                }
                else
                {
                    contragentNameStarLabel.ForeColor = Color.Black;
                    contragentNameBackPanel.BackColor = SystemColors.Control;
                    toolTip.SetToolTip(contragentNameTextBox, String.Empty);
                }//else
            }//else
        }//contragentNameTextBox_Leave

        //Событие для установления каретки в начало codeMaskedTextBox.
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

        private void entityComboBox_Leave(object sender, EventArgs e)
        {
            if (entityComboBox.SelectedIndex == -1)
            {
                entityStarLabel.ForeColor = entityBackPanel.BackColor = Color.Red;

                toolTip.SetToolTip(entityComboBox, "Необходимо выбрать Юр. или Физ. лицо");
                toolTip.Show("Необходимо выбрать Юр. или Физ. лицо", this, entityBackPanel.Location, 5000);
            }
            else //если название введено правильно
            {
                entityStarLabel.ForeColor = Color.Black;
                entityBackPanel.BackColor = SystemColors.Control;
                toolTip.SetToolTip(entityComboBox, String.Empty);
            }//else
        }//entityComboBox_Leave

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                entityComboBox_Leave(sender, e);
                contragentNameTextBox_Leave(sender, e);
                codeMaskedTextBox_Leave(sender, e);
                //Если все данные введены корректно
                if (entityBackPanel.BackColor != Color.Red && contragentNameBackPanel.BackColor != Color.Red
                    && codeBackPanel.BackColor != Color.Red)
                {
                    _contragent.Entity = entityComboBox.Text;
                    _contragent.ContragentName = contragentNameTextBox.Text.Trim();
                    _contragent.Code = (codeMaskedTextBox.Text == String.Empty) ? null : codeMaskedTextBox.Text;
                    _contragent.Description = (String.IsNullOrWhiteSpace(descrRichTextBox.Text)) ? null : descrRichTextBox.Text.Trim();
                    _contragent.ContactInfo = GetContactInfo();

                    if (_contragent is Supplier)
                        PartsDAL.AddSupplier(_contragent as Supplier);
                    if (_contragent is Customer)
                        PartsDAL.AddCustomer(_contragent as Customer);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }//if
            }//if
        }//okButton_MouseClick

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
        }//cancelButton_MouseClick



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
                        if (String.IsNullOrWhiteSpace(textBox.Text)) continue;

                        //Находим имя текущего контрола соответствующее имени свойства класса ContactInfo.
                        string propertyName = char.ToUpper(textBox.Name[0]).ToString() + textBox.Name.Substring(1, textBox.Name.IndexOf("TextBox")-1);

                        //Присваиваем значение свойству propertyName из соответствующего textBox.
                        Type type = typeof(ContactInfo);
                        System.Reflection.PropertyInfo property = type.GetProperty(propertyName);
                        property.SetValue(contactInfo, textBox.Text.Trim());
                    }//if
                }//foreach    
                //добавляем запись в таблицу ContactInfo.
                contactInfo.ContactInfoId = PartsDAL.AddContactInfo(contactInfo);
                return contactInfo;
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

        

        
        
    }//AddcontragentForm

}//namespace

//Сделать contactInfoPanel -- кастомным контролом.