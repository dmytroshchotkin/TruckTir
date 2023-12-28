using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure;
using PartsApp.Models;
using PartsApp.SupportClasses;

namespace PartsApp
{
    public partial class SparePartForm : Form
    {
        /// <summary>
        /// Объект для редактирования или записи в базу нового.
        /// </summary>
        SparePart _sparePart;

        public SparePartForm()
        {
            InitializeComponent();
            _sparePart = new SparePart();
        }
        public SparePartForm(SparePart sparePart)
        {
            InitializeComponent();
            _sparePart = sparePart;

            FillFornFromSparePart(_sparePart); //Заполняем поля формы данными из объетка.
        }

        private void AddSparePartForm_Load(object sender, EventArgs e)
        {
            //добавляем все варианты выбора единицы измерения.
            MeasureUnitComboBox.DataSource = Models.MeasureUnit.GetDescriptions();
            MeasureUnitComboBox.SelectedItem = (_sparePart == null) ? null : _sparePart.MeasureUnit;

            //Добавляем в выпадающий список всех Производителей. /*ERROR!!!*/            
            ManufacturerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllManufacturersName());
        }

        #region Методы проверки корректности ввода.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Валидация ввода артикула.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticulTextBox_Leave(object sender, EventArgs e)
        {
            //если артикул введен.
            if (!String.IsNullOrWhiteSpace(ArticulTextBox.Text))
            {
                string text = ArticulTextBox.Text.Trim();
                //Если введенный артикул уже есть в базе, выдаём предупреждение, но позволяем дальнейший ввод.
                if ((_sparePart.SparePartId != 0 && _sparePart.Articul.ToLower() == text.ToLower()) || PartsDAL.FindSparePartsByArticul(text).Count == 0)
                {
                    ControlValidation.CorrectValueInput(toolTip, ArticulTextBox);
                }
                else
                {
                    ControlValidation.WrongValueInput(toolTip, ArticulTextBox, "Такой артикул уже есть в базе", Color.Yellow);
                }
                   
            }
            else //Если артикул не введен.
            {
                ControlValidation.WrongValueInput(toolTip, ArticulTextBox);
            }
            //Если Title не пустой, проверяем уникальность заполнения связки Артикул-Название.
            if (String.IsNullOrWhiteSpace(TitleTextBox.Text) == false)
            {
                TitleTextBox_Leave(null, null);
            }
        }

        private void TitleTextBox_Leave(object sender, EventArgs e)
        {
            //Если Title введен.
            if (!String.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                //Если в базе есть ещё объекты с таким Артикулом.
                if (ArticulBackPanel.BackColor == Color.Yellow)
                {
                    //Если связка Артикул-Название не уникальны, выводим сообщение об ошибке.
                    if (PartsDAL.FindSparePartsByArticul(ArticulTextBox.Text.Trim()).Any(sp => sp.Title.ToLower() == TitleTextBox.Text.Trim().ToLower()))
                    {
                        ControlValidation.WrongValueInput(toolTip, TitleTextBox, "Такая связка Артикул-Название уже есть в базе");
                    }
                    else
                    {
                        ControlValidation.CorrectValueInput(toolTip, TitleTextBox);
                    }
                }
                else   //если tilte введен правильно            
                {
                    ControlValidation.CorrectValueInput(toolTip, TitleTextBox);
                }
            }
            else //если Title не введен
            {
                ControlValidation.WrongValueInput(toolTip, TitleTextBox);
            }
        }

        private void ManufacturerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MeasureUnitComboBox.Select(); //переводим фокус на др. контрол, и инициируем тем самым событие OnLeave.
            }
        }

        private void ManufacturerTextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ManufacturerTextBox.Text))
            {
                //Если такой производитель в базе отсутствует, выводим сообщение об этом.
                string text = ManufacturerTextBox.Text.Trim().ToLower();
                string manuf = ManufacturerTextBox.AutoCompleteCustomSource.Cast<string>().ToList().FirstOrDefault(c => c.ToLower() == text);
                //Если нет такого Производителя в базе, выводим сообщение.
                if (manuf == null)
                {
                    toolTip.Show("Такого производителя нет в базе! Он будет добавлен.", this, ManufacturerTextBox.Location, 2000);
                }
                else
                {
                    ManufacturerTextBox.Text = manuf; //Выводим корректное имя контрагента.
                }
            }
        }

        //Проверить вылеты.
        private void MeasureUnitComboBox_Leave(object sender, EventArgs e)
        {
            if (MeasureUnitComboBox.SelectedIndex == -1)
            {
                ControlValidation.WrongValueInput(toolTip, MeasureUnitComboBox, "Выберите ед. изм.");
            }
            else
            {
                ControlValidation.CorrectValueInput(toolTip, MeasureUnitComboBox);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы работы с Фото.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Выбор фото данной единицы товара.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPhotoButton_Click(object sender, EventArgs e)
        {
            /*ERROR привести в порядок.*/
            if (PhotoOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(PhotoOpenFileDialog.FileName); //находим имя файла.
                string fullPath = System.IO.Path.GetFullPath(SparePartRepository.PhotoFolder + fileName); //абсолютный путь файла.
                //Если файл с таким именем уже есть в папке 'Товар', выводим сообщение об этом. 
                if (fullPath != PhotoOpenFileDialog.FileName && System.IO.File.Exists(fullPath))
                {
                    PhotoPictureBox.Image = new Bitmap(Image.FromFile(fullPath), PhotoPictureBox.Size);
                    //Если файл в нужной папке не является подходящим, то очищаем pictureBox.
                    if (DialogResult.Cancel == MessageBox.Show("Этот файл или файл с таким именем уже существует в папке \"Товар\".\nЕсли данное фото, является правильным, нажмите \"Ok\".\nИначе нажмите \"Отмена\" измените имя выбираемого файла и попробуйте ещё раз.", "Совпадение имен файлов", MessageBoxButtons.OKCancel))
                    {
                        DeselectToolStripMenuItem_Click(null, null);
                    }
                }
                else
                {
                    PhotoPictureBox.Image = new Bitmap(Image.FromFile(PhotoOpenFileDialog.FileName), PhotoPictureBox.Size);
                }
                toolTip.SetToolTip(PhotoPictureBox, fileName);   //задаём имя файла во всплывающую подсказку.
            }
        }

        /// <summary>
        /// Вызов контекстного меню для photoPictureBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            //Если ПКМ.
            if (e.Button == MouseButtons.Right)
            {
                //Если photoPictureBox не пустой.
                if (PhotoPictureBox.Image != null)
                {
                    PhotoContextMenuStrip.Show(PhotoPictureBox, e.Location); //Выводим контекстное меню.
                }
            }
        }

        /// <summary>
        /// Событие для отмены выбора фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhotoPictureBox.Image = null;
            PhotoOpenFileDialog.FileName = String.Empty;
            toolTip.SetToolTip(PhotoPictureBox, String.Empty);
        }

        /// <summary>
        /// Копирует фото в папку 'Товар', если фото с таким названием ещё нет.
        /// </summary>
        /// <param name="photoPath">Относительный путь к фото.</param>
        private void CopyPhotoToTheFolder(string photoPath)
        {
            //Проверяем наличие фото.
            if (PhotoPictureBox.Image != null)
            {
                string fullPath = System.IO.Path.GetFullPath(photoPath);
                //Если фото ещё нет в папке 'Товар', копируем его туда.
                if (!System.IO.File.Exists(fullPath))
                {
                    FilesStorageHelper.CopyFileSafely(PhotoOpenFileDialog.FileName, fullPath);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


        /// <summary>
        /// Заполняет форму данными из переданного объекта.
        /// </summary>
        /// <param name="sparePart">Объект, данными которого будет заполнена форма.</param>
        private void FillFornFromSparePart(SparePart sparePart)
        {
            //Заполняем все поля на форме.
            ArticulTextBox.Text = _sparePart.Articul;
            TitleTextBox.Text = _sparePart.Title;
            ManufacturerTextBox.Text = _sparePart.Manufacturer;
            DescrRichTextBox.Text = _sparePart.Description;
            MeasureUnitComboBox.SelectedItem = _sparePart.MeasureUnit;

            //Заполняем фото, если оно есть в соотв. папке.
            if (_sparePart.Photo != null && System.IO.File.Exists(System.IO.Path.GetFullPath(_sparePart.Photo)))
            {
                PhotoPictureBox.Image = new Bitmap(Image.FromFile(_sparePart.Photo), PhotoPictureBox.Size);
                toolTip.SetToolTip(PhotoPictureBox, System.IO.Path.GetFileName(_sparePart.Photo));
            }
        }

        /// <summary>
        /// Заполняет объект типа SparePart информацией из формы. 
        /// </summary>
        /// <param name="employee">Товар, который будет заполнен инф-цией из формы.</param>
        private void FillTheSparePartFromForm(SparePart sparePart)
        {
            sparePart.Photo = (PhotoPictureBox.Image != null) ? SparePartRepository.PhotoFolder + toolTip.GetToolTip(PhotoPictureBox) : null;
            sparePart.Articul = ArticulTextBox.Text.Trim();
            sparePart.Title = TitleTextBox.Text.Trim();
            sparePart.Description = (!String.IsNullOrWhiteSpace(DescrRichTextBox.Text)) ? DescrRichTextBox.Text.Trim() : null;
            sparePart.Manufacturer = (!String.IsNullOrWhiteSpace(ManufacturerTextBox.Text)) ? ManufacturerTextBox.Text.Trim() : null;
            sparePart.MeasureUnit = MeasureUnitComboBox.SelectedValue.ToString();
        }

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
            ArticulTextBox_Leave(null, null);
            TitleTextBox_Leave(null, null);
            MeasureUnitComboBox_Leave(null, null);

            //Если хоть один не прошел валидацию, возв-ем false.
            return !curAccBackControls.Any(backPanel => backPanel.BackColor == Color.Red);
        }

        private void CancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void OkButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Если все корректно.
                if (IsRequiredFieldsValid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    FillTheSparePartFromForm(_sparePart); //Заполняем объект данными с формы.

                    try
                    {
                        //Редактируем существующий объект или добавляем новый.
                        if (_sparePart.SparePartId != 0)
                        {
                            PartsDAL.UpdateSparePart(_sparePart);
                        }
                        else
                        {
                            PartsDAL.AddSparePart(_sparePart);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    CopyPhotoToTheFolder(_sparePart.Photo); //Копируем фото в папку 'Товар', если необходимо.
                    this.Close();
                }
            }
        }
    }
}