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
    public partial class SparePartForm : Form
    {
        SparePart editSparePart = null;                  //Переменная требуемая для модификации данных уже сущ-щего товара.
        const string sparePartPhotoFolder = @"Товар\";   /*ERROR перенести в метод?*/


        public SparePartForm()
        {
            InitializeComponent();
        }
        public SparePartForm(int sparePartModifyId)
        {
            InitializeComponent();
            editSparePart = PartsDAL.FindSparePart(sparePartModifyId);

            FillFornFromSparePart(editSparePart); //Заполняем поля формы данными из объетка.
        }//AddSparePartForm


        private void AddSparePartForm_Load(object sender, EventArgs e)
        {
            //добавляем все варианты выбора единицы измерения.

            MeasureUnitComboBox.DataSource = Models.MeasureUnit.GetDescriptions();
            if (editSparePart == null)
                MeasureUnitComboBox.SelectedIndex = -1;
            else MeasureUnitComboBox.SelectedItem = editSparePart.MeasureUnit;
            //Добавляем в выпадающий список всех Производителей.
            /*!!!*/
            manufacturerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllManufacturersName());

        }//Form1_Load   



        #region Методы проверки корректности ввода.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Валидация ввода артикула.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void articulTextBox_Leave(object sender, EventArgs e)
        {
            //если артикул введен.
            if (!String.IsNullOrWhiteSpace(articulTextBox.Text)) 
            {
                string text = articulTextBox.Text.Trim();
                //Если введенный артикул уже есть в базе, выдаём предупреждение, но позволяем дальнейший ввод.
                if ((editSparePart != null && editSparePart.Articul.ToLower() == text.ToLower()) || PartsDAL.FindSparePartsByArticul(text).Count == 0)
                    ControlValidation.CorrectValueInput(toolTip, articulTextBox);
                else
                    ControlValidation.WrongValueInput(toolTip, articulTextBox, "Такой артикул уже есть в базе", Color.Yellow);             
            }//if
            else //Если артикул не введен.
            {
                ControlValidation.WrongValueInput(toolTip, articulTextBox);                                    
            }//else

            //Если Title не пустой, проверяем уникальность заполнения связки Артикул-Название.
            if (String.IsNullOrWhiteSpace(titleTextBox.Text) == false)
                titleTextBox_Leave(null, null);
        }//articulTextBox_Leave

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(titleTextBox.Text)) //если title не введен.
            {
                ControlValidation.WrongValueInput(toolTip, titleTextBox);                
            }//if
            //Если в базе есть ещё объекты с таким Артикулом.
            else if (articulTextBoxBackPanel.BackColor == Color.Yellow)  
            {
                //Если связка Артикул-Название не уникальны, выводим сообщение об ошибке.
                if (PartsDAL.FindSparePartsByArticul(articulTextBox.Text.Trim()).Any(sp => sp.Title.ToLower() == titleTextBox.Text.Trim().ToLower()))
                    ControlValidation.WrongValueInput(toolTip, titleTextBox, "Такая связка Артикул-Название уже есть в базе");
            }//if                
            else   //если tilte введен правильно            
                ControlValidation.CorrectValueInput(toolTip, titleTextBox);            
        }//titleTextBox_Leave

        private void manufacturerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                manufacturerTextBox_Leave(sender, null);
                MeasureUnitComboBox.Select(); //переводим фокус
            }//if
        }//manufacturerTextBox_PreviewKeyDown

        private void manufacturerTextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(manufacturerTextBox.Text))
            {
                //Если такой производитель в базе отсутствует, выводим сообщение об этом.
                string text = manufacturerTextBox.Text.Trim().ToLower();
                string manuf = manufacturerTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == text);
                if (manuf == null)
                    toolTip.Show("Такого производителя нет в базе! Он будет добавлен.", this, manufacturerTextBox.Location, 2000);
                else
                    manufacturerTextBox.Text = manuf; //Выводим корректное имя контрагента.
            }//if
        }//manufacturerTextBox_Leave


        //Проверить вылеты.
        private void unitComboBox_Leave(object sender, EventArgs e)
        {
            if (MeasureUnitComboBox.SelectedIndex == -1)
                ControlValidation.WrongValueInput(toolTip, MeasureUnitComboBox, "Выберите ед. изм.");
            else
                ControlValidation.CorrectValueInput(toolTip, MeasureUnitComboBox);
        }//unitComboBox_Leave








/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы работы с Фото.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Выбор фото данной единицы товара.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPhotoButton_Click(object sender, EventArgs e)
        {
            /*ERROR привести в порядок.*/
            if (photoOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(photoOpenFileDialog.FileName); //находим имя файла.

                toolTip.SetToolTip(photoPictureBox, fileName);                              //задаём имя файла во всплывающую подсказку.
                //Проверяем находится ли фото в нужной папке. 
                string path = sparePartPhotoFolder + fileName;

                //Проверяем, есть ли уже выбранное фото в папке 'Товар'.
                if (System.IO.Path.GetFullPath(path) == photoOpenFileDialog.FileName)
                {
                    //Если фото выбрано, то подгоняем его размер под PictureBox и добавляем всплывающую подсказку.
                    photoPictureBox.Image = new Bitmap(Image.FromFile(photoOpenFileDialog.FileName), photoPictureBox.Size);
                }//if
                else
                {
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
                        photoPictureBox.Tag = System.IO.Path.GetFullPath(path); //записываем конечный путь файла в св-во tag.
                    }//else
                }//else
            }//if
        }//addPhotoButton_Click

        /// <summary>
        /// Вызов контекстного меню для photoPictureBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void photoPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            //Если ПКМ.
            if (e.Button == MouseButtons.Right)
            {
                //Если photoPictureBox не пустой.
                if (photoPictureBox.Image != null)
                    photoContextMenuStrip.Show(photoPictureBox, e.Location); //Выводим контекстное меню.
            }//if
        }//photoPictureBox_MouseClick

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
        }//deselectToolStripMenuItem_Click 






/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

                
              

        /// <summary>
        /// Заполняет форму данными из переданного объекта.
        /// </summary>
        /// <param name="sparePart">Объект, данными которого будет заполнена форма.</param>
        private void FillFornFromSparePart(SparePart sparePart)
        {
            //Заполняем все поля на форме.
            articulTextBox.Text      = editSparePart.Articul;
            titleTextBox.Text        = editSparePart.Title;
            manufacturerTextBox.Text = editSparePart.Manufacturer;
            descrRichTextBox.Text    = editSparePart.Description;
            MeasureUnitComboBox.SelectedItem = editSparePart.MeasureUnit;
            
            //Заполняем фото, если оно есть в соотв. папке.
            if (editSparePart.Photo != null)
            {
                if (System.IO.File.Exists(System.IO.Path.GetFullPath(editSparePart.Photo)))
                {
                    photoPictureBox.Image = new Bitmap(Image.FromFile(editSparePart.Photo), photoPictureBox.Size);
                    toolTip.SetToolTip(photoPictureBox, System.IO.Path.GetFileName(editSparePart.Photo));
                }//if
            }//if            
        }//FillFornFromSparePart

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
                }//if
                sparePart.Photo = sparePartPhotoFolder + toolTip.GetToolTip(photoPictureBox);
            }//else

            sparePart.Articul      = articulTextBox.Text.Trim();
            sparePart.Title        = titleTextBox.Text.Trim();
            sparePart.Description  = (!String.IsNullOrWhiteSpace(descrRichTextBox.Text)) ? descrRichTextBox.Text.Trim() : null;
            sparePart.Manufacturer = (!String.IsNullOrWhiteSpace(manufacturerTextBox.Text)) ? manufacturerTextBox.Text.Trim() : null;
            sparePart.MeasureUnit  = MeasureUnitComboBox.SelectedValue.ToString();
        }//FillTheSparePartFromForm



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
            articulTextBox_Leave(null, null);
            titleTextBox_Leave(null, null);
            unitComboBox_Leave(null, null);

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
                }//if
            }//if
        }//cancelButton_MouseClick

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Если все корректно.
                if (IsRequiredFieldsValid())
                {
                    this.Cursor = Cursors.WaitCursor;

                    SparePart sparePart = new SparePart();
                    FillTheSparePartFromForm(sparePart); //Заполняем объект данными с формы.

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
        }//

        

        


        



    }//AddSparePartForm
}//namespace

/*Задачи*/
//!!! Решить проблему с "попытка записи в защищенную область памяти" или сделать listbox выпадающим списком.
//1)Добавить возможность выбора множества категорий (как в MovieDB).
//2)Добавить воззможность добавлять новую ед. изм. в базу.