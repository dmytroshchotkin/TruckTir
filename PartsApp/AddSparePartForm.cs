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
    public partial class AddSparePartForm : Form
    {
        SparePart editSparePart = null;                  //Переменная требуемая для модификации данных уже сущ-щего товара.
        const string sparePartPhotoFolder = @"Товар\";


        public AddSparePartForm()
        {
            InitializeComponent();
        }
        public AddSparePartForm(int sparePartModifyId)
        {
            InitializeComponent();
            editSparePart = PartsDAL.FindSparePartById(sparePartModifyId); 

            //Заполняем все поля в форме по заданному Id.
            articulTextBox.Text = editSparePart.Articul;
            titleTextBox.Text = editSparePart.Title;
            manufacturerTextBox.Text = editSparePart.Manufacturer;
            unitComboBox.SelectedItem = editSparePart.Unit;
            if (editSparePart.Photo != null)
            {
                if (System.IO.File.Exists(System.IO.Path.GetFullPath(editSparePart.Photo)))
                {
                    photoPictureBox.Image = new Bitmap(Image.FromFile(editSparePart.Photo), photoPictureBox.Size);
                    toolTip.SetToolTip(photoPictureBox, System.IO.Path.GetFileName(editSparePart.Photo));
                }//if
            }//if

            descrRichTextBox.Text = editSparePart.Description;
        }//AddSparePartForm

        #region Методы проверки корректности ввода.

        private void AddSparePartForm_Load(object sender, EventArgs e)
        {
            //добавляем все варианты выбора единицы измерения.
            unitComboBox.DataSource = PartsDAL.FindAllUnitsOfMeasure();
            if (editSparePart == null)
                unitComboBox.SelectedIndex = -1;
            else unitComboBox.SelectedItem = editSparePart.Unit;
            //Добавляем в выпадающий список всех Производителей.
/*!!!*/     manufacturerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllManufacturersName());


        }//Form1_Load   

        private void articulTextBox_Leave(object sender, EventArgs e)
        {
            //если артикул не введен.
            if (String.IsNullOrWhiteSpace(articulTextBox.Text)) 
            {
                WrongValueInput(articulTextBox, articulTextBoxBackPanel, articulStarLabel, "Введите артикул", 5000);
                articulTextBox.Clear();
            }//if
            else //Если артикул введен.
            {
                //если такой артикул уже есть в базе
                if (PartsDAL.FindSparePartsIdByArticul(articulTextBox.Text).Count > 0)
                {
                    //если (доб-ся новая ед. товара или (редактируется уже существующая, но артикул изменен)), выводим предупреждение, но разрешаем дальнейший ввод инф-ции.
                    if (editSparePart == null || (editSparePart != null && editSparePart.Articul != articulTextBox.Text))
                    {
                        articulStarLabel.ForeColor = articulTextBoxBackPanel.BackColor = Color.Yellow;
                        toolTip.SetToolTip(articulTextBox, "Такой артикул уже есть в базе");
                        toolTip.Show("Такой артикул уже есть в базе", this, articulTextBoxBackPanel.Location, 5000);
                    }//if
                    else //если артикул введен правильно
                    {
                        CorrectValueInput(articulTextBox, articulTextBoxBackPanel, articulStarLabel);
                    }//else
                }//if                
                else //если артикул введен правильно
                {
                    CorrectValueInput(articulTextBox, articulTextBoxBackPanel, articulStarLabel);
                }//else

                //Проверяем корректность Title (если не пустой) после корректного ввода Articul.
                if (String.IsNullOrWhiteSpace(titleTextBox.Text) == false)
                    titleTextBox_Leave(sender, e);
                    
            }//else
        }//articulTextBox_Leave

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(titleTextBox.Text)) //если title не введен.
            {
                titleStarLabel.ForeColor = titleTextBoxBackPanel.BackColor = Color.Red;
                toolTip.SetToolTip(titleTextBox, "Введите название");
                toolTip.Show("Введите название", this, titleTextBoxBackPanel.Location, 5000);
                titleTextBox.Clear();
                return;
            }//if

            if (articulTextBoxBackPanel.BackColor == Color.Yellow) //если есть такой артикул. 
            {
                foreach (var sparePart in PartsDAL.FindSparePartsByArticul(articulTextBox.Text))
                    if (sparePart.Title == titleTextBox.Text)
                    {
                        titleStarLabel.ForeColor = titleTextBoxBackPanel.BackColor = Color.Red;
                        toolTip.SetToolTip(titleTextBox, "Такое название уже есть в базе");
                        toolTip.Show("Такое название уже есть в базе", this, titleTextBoxBackPanel.Location, 5000);
                        return;
                    }//if
            }//if                
           
            //если tilte введен правильно
            titleStarLabel.ForeColor = Color.Black;
            titleTextBoxBackPanel.BackColor = SystemColors.Control;
            toolTip.SetToolTip(titleTextBox, String.Empty);         
        }//titleTextBox_Leave

        //Проверить вылеты.
        private void unitComboBox_Leave(object sender, EventArgs e)
        {
            //Если добавляется новая ед.изм.
            if (unitComboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (String.IsNullOrWhiteSpace(unitComboBox.Text))
                {
                    WrongValueInput(unitComboBox, unitComboBoxBackPanel, unitStarLabel, "Введите новую единицу измерения", 5000);
                }//if
                else
                    if (unitComboBox.Items.Contains(unitComboBox.Text))//если введена уже существующая ед.изм.
                    {
                        CorrectValueInput(unitComboBox, unitComboBoxBackPanel, unitStarLabel);
                        toolTip.Show("Такая единица измерения уже существует!", this, unitComboBoxBackPanel.Location, 5000);

                        string text = unitComboBox.Text;
                        unitComboBox.Leave -= unitComboBox_Leave;
                        unitComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        unitComboBox.Leave += unitComboBox_Leave;

                        unitComboBox.Text = text;
                    }// else if
                    else //если title введен правильно
                    {
                        CorrectValueInput(unitComboBox, unitComboBoxBackPanel, unitStarLabel);
                    }//else
            }//if
            else //Если новая ед. изм. не добавляется
            {
                if (String.IsNullOrWhiteSpace(unitComboBox.Text))
                    WrongValueInput(unitComboBox, unitComboBoxBackPanel, unitStarLabel, "Выберите ед. изм.", 2000);
                else
                    CorrectValueInput(unitComboBox, unitComboBoxBackPanel, unitStarLabel);
            }//else
        }//unitComboBox_Leave



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






        #endregion
        
        
        //Событие для добавления новой единицы измерения в БД.
        private void addUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            unitComboBox.Text = String.Empty;
            unitComboBox.Focus();
        }//addUnitToolStripMenuItem_Click

        private void addPhotoButton_Click(object sender, EventArgs e)
        {
            if (photoOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(photoOpenFileDialog.FileName);

                toolTip.SetToolTip(photoPictureBox, fileName);
                //Проверяем находится ли фото в нужной папке. 
                string path = sparePartPhotoFolder + toolTip.GetToolTip(photoPictureBox);

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
                        if (DialogResult.Cancel == MessageBox.Show("Этот файл или файл с таким именем уже существует в папке \"Товар\".\nЕсли данное фото, является правильным, нажмите \"Ok\".\nИначе нажмите \"Отмена\" измените имя выбираемого файла и попробуйте ещё раз.", "Совпадение имен файлов", MessageBoxButtons.OKCancel))
                            deselectToolStripMenuItem_Click(sender, e);
                    }//if
                    //Если файл не находится в нужной папке, и при этом нет совпадения имен, копируем его.
                    else
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                        //записываем конечный путь файла всв-во tag.
                        photoPictureBox.Tag = System.IO.Path.GetFullPath(path);
                    }//else

            }//if
        }//addPhotoButton_Click
        
        //Событие для отмены выбора фотографии.
        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            photoPictureBox.Image = null;
            photoOpenFileDialog.FileName = String.Empty;
            toolTip.SetToolTip(photoPictureBox, String.Empty);
        }//deselectToolStripMenuItem_Click
       
        private void manufacturerTextBox_TextChanged(object sender, EventArgs e)
        {
            //manufColl.Clear();

            //if (manufacturerTextBox.Text == String.Empty) return;

            //string[] str = PartsDAL.SearchManufacturersName(manufacturerTextBox.Text, 10);
            //manufColl.AddRange(str);

            //manufacturerTextBox.AutoCompleteCustomSource = coll;
        }//manufacturerTextBox_TextChanged

        private void manufacturerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            ////Если идет выбор с выпадающего списка.
            //if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            //{
            //    manufacturerTextBox.TextChanged -= manufacturerTextBox_TextChanged;
            //    textChangeEvent = false;
            //    return;
            //}//if
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //if (supplierTextBox.AutoCompleteCustomSource.Count == 0) return;
            //    manufacturerTextBox.TextChanged -= manufacturerTextBox_TextChanged;
            //    textChangeEvent = false;
            //    return;
            //}
            ////Продолжается ввод.
            //if (textChangeEvent == false)
            //{
            //    manufacturerTextBox.TextChanged += manufacturerTextBox_TextChanged;
            //    textChangeEvent = true;
            //}//if
         
        }//manufacturerTextBox_PreviewKeyDown

        /// <summary>
        /// Заполняет объект типа SparePart информацией из формы. 
        /// </summary>
        /// <param name="employee">Товар, который будет заполнен инф-цией из формы.</param>
        private void FillTheSparePartFromForm(SparePart sparePart)
        {
            //Проверяем наличие фото.
            if (photoPictureBox.Image != null)
            {
                if (photoPictureBox.Tag != null) //если false значит фото уже есть в нужной папке и мы просто записываем относительный путь иначе сначала копируем файл.  
                {
                    string destFilePath = photoPictureBox.Tag as string;
                    System.IO.File.Copy(photoOpenFileDialog.FileName, destFilePath);
                }
                sparePart.Photo = sparePartPhotoFolder + toolTip.GetToolTip(photoPictureBox);
            }//else

            sparePart.Articul = articulTextBox.Text.Trim();
            sparePart.Title = titleTextBox.Text.Trim();
            sparePart.Description = (!String.IsNullOrWhiteSpace(descrRichTextBox.Text)) ? descrRichTextBox.Text.Trim() : null;
            sparePart.ExtInfoId = null;
            //добаляем manufacturer
            if (String.IsNullOrWhiteSpace(manufacturerTextBox.Text) == false)
            {
                //Если такого ManufacturerName нет в базе, значит добавить.
                if (PartsDAL.FindManufacturersIdByName(manufacturerTextBox.Text.Trim()).Count == 0)
                    sparePart.ManufacturerId = PartsDAL.AddManufacturer(manufacturerTextBox.Text.Trim());
                else
                    sparePart.ManufacturerId = PartsDAL.FindManufacturersIdByName(manufacturerTextBox.Text.Trim())[0]; //!!! Кроется опасность путаницы в случае одинакового имени производителей, необходимо будет внести добавление в базу для избежания потенциальной угрозы!
            }//else

            //Вставляем ед. изм. 
            //if (unitComboBox.DropDownStyle == ComboBoxStyle.DropDown) //если вставляется новое значение в бд.
            //PartsDAL.AddUnitOfMeasure();
            sparePart.Unit = unitComboBox.SelectedValue.ToString();
        }//FillTheSparePartFromForm

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
        }

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Проверяем корректность ввода необходимых данных.
                articulTextBox_Leave(sender, e);
                titleTextBox_Leave(sender, e);
                unitComboBox_Leave(sender, e);

                //Если все корректно.
                if (articulTextBoxBackPanel.BackColor != Color.Red && titleTextBoxBackPanel.BackColor != Color.Red
                    && unitComboBoxBackPanel.BackColor != Color.Red)
                {
                    this.Cursor = Cursors.WaitCursor;

                    SparePart sparePart = new SparePart();
                    FillTheSparePartFromForm(sparePart);

                    try
                    {
                        //Проверяем добавляется новая ед. товара или модиф-ся уже сущ-щая.
                        if (editSparePart == null)
                            PartsDAL.AddSparePart(sparePart);
                        else
                        {
                            sparePart.SparePartId = editSparePart.SparePartId;
                            PartsDAL.UpdateSparePart(sparePart);
                        }//else
                    }//try
                    catch
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        this.Cursor = Cursors.Default;
                        return;
                    }//catch

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }//if
            }//if
        }


        



    }//AddSparePartForm
}//namespace

/*Задачи*/
//!!! Решить проблему с "попытка записи в защищенную область памяти" или сделать listbox выпадающим списком.
//1)Добавить возможность выбора множества категорий (как в MovieDB).
//2)Добавить воззможность добавлять новую ед. изм. в базу.