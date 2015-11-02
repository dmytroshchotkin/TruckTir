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
        string beginfilePath = null;        //переменные не будут равны null если требуется скопировать файл в нужную папку.
        string endFilePath = null;
              
        //bool textChangeEvent = true;        //переменная равна true -- если у события manufacturerTextBox.TextChanged есть подписчик.

        //AutoCompleteStringCollection manufColl;

        public AddSparePartForm()
        {
            InitializeComponent();
            //manufColl = new AutoCompleteStringCollection();
        }

        private void AddSparePartForm_Load(object sender, EventArgs e)
        {
            //добавляем все варианты выбора единицы измерения.
            unitComboBox.DataSource = PartsDAL.FindAllUnitsOfMeasure();
            unitComboBox.SelectedItem = "шт.";

            manufacturerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllManufacturersName());
        }//Form1_Load   

        private void articulTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(articulTextBox.Text)) //если артикул не введен.
            {
                articulStarLabel.ForeColor = articulTextBoxBackPanel.BackColor = Color.Red;
                toolTip.SetToolTip(articulTextBox, "Введите артикул");
                toolTip.Show("Введите артикул", this, articulTextBoxBackPanel.Location, 5000);
                articulTextBox.Clear();
            }//if
            else
                if (PartsDAL.FindSparePartsIdByArticul(articulTextBox.Text).Count > 0) //если такой артикул уже есть 
                {
                    articulStarLabel.ForeColor = articulTextBoxBackPanel.BackColor = Color.Yellow;
                    toolTip.SetToolTip(articulTextBox, "Такой артикул уже есть в базе");
                    toolTip.Show("Такой артикул уже есть в базе", this, articulTextBoxBackPanel.Location, 5000);

                    //Добавление всей остальной информации на форму из товара с таким же артикулом                    
                }//if                
                else //если артикул введен правильно
                {
                    articulStarLabel.ForeColor = Color.Black;
                    articulTextBoxBackPanel.BackColor = SystemColors.Control;
                    toolTip.SetToolTip(articulTextBox, String.Empty);
                }//else

            if (String.IsNullOrWhiteSpace(titleTextBox.Text) == false)
                titleTextBox_Leave(sender, e);
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
            //если добавляется новая ед.изм.
            if (unitComboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (unitComboBox.Text == String.Empty)
                {
                    unitStarLabel.ForeColor = unitComboBoxBackPanel.BackColor = Color.Red;
                    toolTip.SetToolTip(unitComboBox, "Введите новую единицу измерения");
                    toolTip.Show("Введите новую единицу измерения", this, unitComboBoxBackPanel.Location, 5000);
                }//if
                else if (unitComboBox.Items.Contains(unitComboBox.Text))//если введена уже существующая ед.изм.
                {
                    unitStarLabel.ForeColor = Color.Black;
                    unitComboBoxBackPanel.BackColor = SystemColors.Control;
                    toolTip.SetToolTip(unitComboBox, String.Empty);
                    toolTip.Show("Такая единица измерения уже существует!", this, unitComboBoxBackPanel.Location, 5000);

                    string text = unitComboBox.Text;
                    unitComboBox.Leave -= unitComboBox_Leave;
                    unitComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    unitComboBox.Leave += unitComboBox_Leave;

                    unitComboBox.Text = text;
                }
                else //если tilte введен правильно
                {
                    unitStarLabel.ForeColor = Color.Black;
                    unitComboBoxBackPanel.BackColor = SystemColors.Control;
                    toolTip.SetToolTip(unitComboBox, String.Empty);
                }//else
            }//if
        }//unitComboBox_Leave

        //Событие для добавления новой единицы измерения в БД.
        private void addUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            unitComboBox.Text = String.Empty;
            unitComboBox.Focus();
        }//addUnitToolStripMenuItem_Click
        //Событие добавление запчастей в список взаимозаменяемых запчастей. 
        private void addInterchSpButton_Click(object sender, EventArgs e)
        {
            //interchSpDataGridView.Rows.Add();
            SparePart sp = new SparePart();
            sp.Articul = interchSpDataGridView.Rows[0].Cells[0].Value.ToString();
            sp.Title = interchSpDataGridView.Rows[0].Cells[1].Value.ToString();


           string str = String.Format("{0} {1} ", sp.Articul, sp.Title);
           MessageBox.Show(str);
        }//addInterchSpButton_Click

        private void addPhotoButton_Click(object sender, EventArgs e)
        {
            if (photoOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(photoOpenFileDialog.FileName);

                toolTip.SetToolTip(photoPictureBox, fileName);
                //Проверяем находится ли фото в нужной папке. 
                string path = @"Товар\" + toolTip.GetToolTip(photoPictureBox);

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
                        if (DialogResult.Cancel == MessageBox.Show("Этот файл или файл с таким именем уже существует в папке \"Товар\".\nЕсли данное фото, является правильным, нажмите \"Ok\".\nИначе нажмите \"Отмена\" и измените имя выбираемого файла и попробуйте ещё раз.", "Совпадение имен файлов", MessageBoxButtons.OKCancel))
                            deselectToolStripMenuItem_Click(sender, e);
                    }//if
                    //Если файл не находится в нужной папке, и при этом нет совпадения имен, копируем его.
                    else
                    {
                        photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                        beginfilePath = photoOpenFileDialog.FileName;
                        endFilePath = System.IO.Path.GetFullPath(path);
                    }//else

            }//if
        }//addPhotoButton_Click

        private void interchSpDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {          
            //int column = interchSpDataGridView.CurrentCell.ColumnIndex;
            //string headerText = interchSpDataGridView.Columns[column].HeaderText;
           
            //if (headerText.Equals("Артикул"))
            //{
            //    spareParts.Clear();
            //    for (int i = 0; i <= 10; i++)
            //    {
            //        SparePart sp = new SparePart
            //        {
            //            Photo = "Photo",
            //            Articul = "articul" + i.ToString(),
            //            Title = "title" + i.ToString(),
            //            Description = "title" + i.ToString(),
            //            //Category = "title" + i.ToString(),
            //            Manufacturer = manufacturerTextBox.Text,
            //        };
            //        spareParts.Add(sp);
            //    }//for

            //    TextBox tb = e.Control as TextBox;

            //    AutoCompleteStringCollection str = new AutoCompleteStringCollection();
            //    foreach(var sp in spareParts)
            //        str.Add(sp.Articul);

            //    if (tb != null)
            //    {
            //        tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //        tb.AutoCompleteCustomSource = str;
            //        tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //    }
            //}//if
        }//interchSpDataGridView_EditingControlShowing

        private void interchSpDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    DataGridViewCell cell = interchSpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    SparePart sp = new SparePart();
            //    foreach (var spr in spareParts)
            //        if (spr.Articul == cell.Value.ToString())
            //            sp = spr;

            //    interchSpDataGridView.Rows[e.RowIndex].Cells[1].Value = sp.Title;
            //    interchSpDataGridView.Rows[e.RowIndex].Cells[2].Value = sp.Description;
            //    interchSpDataGridView.Rows[e.RowIndex].Cells[3].Value = sp.Manufacturer;
            //    //interchSpDataGridView.Rows[e.RowIndex].Cells[4].Value = sp.Category;


            //}//if
        }//interchSpDataGridView_CellEndEdit
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

        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
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
                    SparePart sparePart = new SparePart();
                    //Проверяем наличие фото.
                    if (photoPictureBox.Image == null)
                        sparePart.Photo = null;
                    else
                    {
                        if (beginfilePath != null) //если false значит фото уже есть в нужной папке и мы просто записываем относительный путь иначе вначале копируем файл.  
                        {
                            System.IO.File.Copy(beginfilePath, endFilePath);
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
                        if (PartsDAL.SearchManufacturersName(manufacturerTextBox.Text, 1).Length == 0)
                            sparePart.ManufacturerId = PartsDAL.AddManufacturer(manufacturerTextBox.Text.Trim());
                        else
                            sparePart.ManufacturerId = PartsDAL.FindManufacturersIdByName(manufacturerTextBox.Text)[0]; //!!! Кроется опасность путаницы в случае одинакового имени производителей, необходимо будет внести добавление в базу для избежания потенциальной угрозы!
                    }//else
                    //Вставляем ед. изм. 
                    //if (unitComboBox.DropDownStyle == ComboBoxStyle.DropDown) //если вставляется новое значение в бд.
                    //PartsDAL.AddUnitOfMeasure();
                    sparePart.Unit = unitComboBox.SelectedValue.ToString();

                    PartsDAL.AddSparePart(sparePart);

                    this.Close();
                }//if
            }//if
        }






    }//AddSparePartForm
}

/*Задачи*/
//!!! Решить проблему с "попытка записи в защищенную область памяти" или сделать listbox выпадающим списком.
//1)Добавить возможность выбора множества категорий (как в MovieDB).
//2)Добавить воззможность добавлять новую ед. изм. в базу.


/*Долгим ковырянием проблему решил, пересобрал проект заново. По одной все формы исключил и добавил снова.
 * Короче проблема была в кривой сборке*/