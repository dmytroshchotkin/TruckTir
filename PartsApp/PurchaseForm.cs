using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PartsApp
{
    public partial class PurchaseForm : Form
    {
        IList<SparePart> spareParts = new List<SparePart>();
        SparePart currentSparePart = new SparePart();

        IList<SparePart> searchSparePartsList = new List<SparePart>();
        bool isCellEditError = false;
        DataGridViewCell lastEditCell;

        TextBox textBoxCell;
        bool textChangedEvent = false;
        bool previewKeyDownEvent = false;
        string _userText;

        double inTotal;
        IList<int> sparePartsId = new List<int>();   //коллекция для хранения Id того товара, что уже есть в таблице.


        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            storageComboBox.SelectedItem = PartsApp.SparePart.MainStorage;

            supplierTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllSuppliersName());

            //Устанавливаем параметры дат, для DateTimePicker.            
            purchaseDateTimePicker.MaxDate = purchaseDateTimePicker.Value = DateTime.Now;
            
            //Вносим все типы наценок в markupComboBox             
            markupComboBox.Items.AddRange(PartsDAL.FindAllMarkups().Select(markup => markup.Value).ToArray<string>());

            currencyComboBox.SelectedItem = "руб";

            buyerAgentTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
            buyerAgentTextBox.ReadOnly = true;
        }//PurchaseForm_Load

        private void storageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (storageComboBox.SelectedItem.ToString() == PartsApp.SparePart.VirtStorage)
                storageAdressStarLabel.Visible = storageAdressLabel.Visible = storageAdressBackPanel.Visible = true;
            else
            {
                storageAdressStarLabel.Visible = storageAdressLabel.Visible = storageAdressBackPanel.Visible = false;
                storageAdressBackPanel.BackColor = SystemColors.Control;
                storageAdressTextBox.Clear();
            }
        }//storageComboBox_SelectedIndexChanged

        private void storageAdressTextBox_Leave(object sender, EventArgs e)
        {
            if (storageAdressTextBox.Visible)
            {
                if (String.IsNullOrWhiteSpace(storageAdressTextBox.Text))
                    storageAdressBackPanel.BackColor = Color.Red;
                else storageAdressBackPanel.BackColor = SystemColors.Control;
            }
        }//storageAdressTextBox_Leave

        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void supplierTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                supplierTextBox_Leave(sender, null);
            }//if
        }//supplierTextBox_PreviewKeyDown

        private void supplierTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(supplierTextBox.Text))
            {
                supplierBackPanel.BackColor = supplierStarLabel.ForeColor = Color.Red;
                supplierTextBox.Clear();
                return;
            }//if
            if (supplierTextBox.AutoCompleteCustomSource.Contains(supplierTextBox.Text)) //Если есть такой поставщик в базе
            {
                supplierStarLabel.ForeColor = Color.Black;
                supplierBackPanel.BackColor = SystemColors.Control;
                //receiverTextBox.Focus();
                supplierLabel.Focus(); //убираем фокус с supplierTextBox контрола.
            }//if
            else //если такой поставщик в базе отсутствует.
            {
                supplierBackPanel.BackColor = supplierStarLabel.ForeColor = Color.Red;
                if (MessageBox.Show("Добавить нового поставщика?", "Такого поставщика нет в базе!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IContragent supplier = new Supplier();
                    if (new AddContragentForm(supplier).ShowDialog() == DialogResult.OK)
                    {

                        supplierTextBox.Leave -= supplierTextBox_Leave;
                        supplierTextBox.AutoCompleteCustomSource.Add(supplier.ContragentName);
                        supplierTextBox.Text = supplier.ContragentName;
                        supplierTextBox.Leave += supplierTextBox_Leave;
                    }//if
                }//if
            }//else
        }

        private void buyerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(buyerTextBox.Text))
            {
                buyerBackPanel.BackColor = buyerStarLabel.ForeColor = Color.Red;
                buyerTextBox.Clear();
                toolTip.Show("Введите имя/название покупателя", this, buyerBackPanel.Location, 2000);
            }//if
            else
            {
                buyerStarLabel.ForeColor = Color.Black;
                buyerBackPanel.BackColor = SystemColors.Control;
            }//else
        }//buyerTextBox_Leave





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        /*Нумерация строк purchaseDataGridView*/
        private void partsDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridView.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridView.Rows[index].HeaderCell.Value = indexStr;

             

/*!!!*/      if (index+1 == 10)    //предусмотрен вариант расширения столбца нумерации строк только до 2 цифр!
                purchaseDataGridView.RowHeadersWidth = 41 + 7; //((i - 1) * 7); //41 - изначальный размер RowHeaders
        }//partsDataGridView_RowPrePaint

        private void purchaseDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Эта строка нужна потому что новые столбцы Price и Count почему то становятся открытыми для записи.
/*!!!*/      purchaseDataGridView.Rows[e.RowIndex].Cells["Price"].ReadOnly = purchaseDataGridView.Rows[e.RowIndex].Cells["Count"].ReadOnly = true;
        }//purchaseDataGridView_RowsAdded

        #region Обработка событий добавления товара в список.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*События идут в порядке их возможного вызова.*/

        // Событие для установки listBox в нужную позицию. //
        private void purchaseDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];
            lastEditCell = cell;
            if (cell.OwningColumn.Name == "Title" || cell.OwningColumn.Name == "Articul")
            {
                autoCompleteListBox.Location = GetCellBelowLocation(cell);               
            }                    
        }//purchaseDataGridView_CellBeginEdit

        //Событие для добавления обработчиков на ввод текста в клетку. //
        private void purchaseDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {            
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)purchaseDataGridView.CurrentCell;

            //
            if (cell.OwningColumn.Name == "Title" || cell.OwningColumn.Name == "Articul")
            {
                textBoxCell = e.Control as TextBox;
                //if (textBoxCell != null) //Нужна ли эта проверка?
                if (previewKeyDownEvent == false)
                {
                    previewKeyDownEvent = true;
                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                    textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                }
            }//if
        }//purchaseDataGridView_EditingControlShowing

        private void dataGridViewTextBoxCell_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.KeyCode == Keys.Down)
            {                                                
                isCellEditError = true;
                if (searchSparePartsList.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Items.Count == 0) return; //может не надо это действие.
                if (autoCompleteListBox.Visible == false) return;
                //Если выбран последний эл-нт списка, вернуть начальное значение и убрать выделение в listBox-е. 
                if (autoCompleteListBox.SelectedIndex == autoCompleteListBox.Items.Count - 1)
                {
                    textBox.Text = _userText;
                    autoCompleteListBox.ClearSelected();
                    return;
                }
                //Если выбирается первый эл-нт выпадающего списка, запоминаем введенную ранее пользователем строку.
                if (autoCompleteListBox.SelectedIndex == -1)
                    _userText = textBox.Text;

                autoCompleteListBox.SelectedIndex += 1;
                return;
            }//if

            if (e.KeyCode == Keys.Up)
            {
                isCellEditError = true;
                if (searchSparePartsList.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Items.Count == 0) return;//может не надо это действие.
                if (autoCompleteListBox.Visible == false) return;
                //Если нет выбранных эл-тов в вып. списке, выбрать последний его эл-нт.
                if (autoCompleteListBox.SelectedIndex == -1)
                {
                    _userText = textBox.Text;
                    autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1;
                }
                //Если выбран верхний эл-нт вып. списка, вернуть введенную ранее пользователем строку.
                else if (autoCompleteListBox.SelectedIndex == 0)
                {
                    textBox.Text = _userText;
                    autoCompleteListBox.ClearSelected();
                }//if
                else autoCompleteListBox.SelectedIndex -= 1;
                //Если это нулевая строка, то при нажатии Up не происходит событие SelectionChanged, и при выборе из вып. списка каретка ставитс в начало строки, что затрудняет дальнейший ввод поль-лю. Мы вызываем событие искусствунно и ставим каретку в конец строки.                               
                if (lastEditCell.OwningRow.Index == 0) 
                    purchaseDataGridView_SelectionChanged(sender, null); 

                return;
            }//if    

            //Если ввод условия поиска завершен.

            //Продолжается ввод.
            if (textChangedEvent == false)
            {
                //textBox.TextChanged += dataGridViewTextBoxCell_TextChanged;
                textChangedEvent = true;
            }


    
        }//dataGridViewTextBoxCell_PreviewKeyDown

        private void dataGridViewTextBoxCell_TextChanged(object sender, EventArgs e)
        {
            if (textChangedEvent == false) return;

            /* Эта проверка нужна потому что в редких случаях по непонятным причинам TextChanged срабатывает на столбцы Count или др. на которых работать не должен
                Это случается когда вводишь что-то в столбец Title, а потом стираешь до пустой строки и вводишь что-то в столбец Count.*/
/*!!!*/         if (lastEditCell.OwningColumn.Name != "Title" && lastEditCell.OwningColumn.Name != "Articul")
                    return;
            
            TextBox textBox = (TextBox)sender;
            if (String.IsNullOrEmpty(textBox.Text) == false)
            {
                if (lastEditCell.OwningColumn.Name == "Title")
                    searchSparePartsList = PartsDAL.SearchSparePartsByTitle(textBox.Text, 10, sparePartsId);
                else if (lastEditCell.OwningColumn.Name == "Articul")
                    searchSparePartsList = PartsDAL.SearchSparePartsByArticul(textBox.Text, 10, sparePartsId);
                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    autoCompleteListBox.Items.Clear();
                    string str = null;
                    foreach (var sparePart in searchSparePartsList)
                    {
                        if (lastEditCell.OwningColumn.Name == "Title")
                            str = sparePart.Title + "     " + sparePart.Articul;
                        else if (lastEditCell.OwningColumn.Name == "Articul")
                               str = sparePart.Articul + "     " + sparePart.Title;

/*!!!! Ошибка!*/        autoCompleteListBox.Items.Add(str);
                    }//foreach                                                                        

                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
                    autoCompleteListBox.Visible = true;
                }//if
                else autoCompleteListBox.Visible = false; //Если ничего не найдено, убрать вып. список.
            }//if
            else autoCompleteListBox.Visible = false; //Если ничего не введено, убрать вып. список.
        }//dataGridViewTextBoxCell_TextChanged

        private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
        {
            isCellEditError = true;            
        }

        private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (String.IsNullOrEmpty(_userText))
                    _userText = textBoxCell.Text;
                purchaseDataGridView_SelectionChanged(null, null);
                isCellEditError = true; 
            }
            else 
            { 
            //    textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
            //    textChangedEvent = true; 
                isCellEditError = false; 
                //dataGridViewTextBoxCell_PreviewKeyDown(textBoxCell, new PreviewKeyDownEventArgs(Keys.Enter));
                //purchaseDataGridView.Rows[lastEditCell.RowIndex + 1].Cells["Title"].Selected = true;
                purchaseDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(lastEditCell.ColumnIndex, lastEditCell.RowIndex));
            }
        }

        private void autoCompleteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {           
            textChangedEvent = false;
            if (autoCompleteListBox.SelectedIndex != -1)
                textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
        }//autoCompleteListBox_SelectedIndexChanged

        //Привести в порядок метод!!!!
        private void purchaseDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isCellEditError) 
                return;

            autoCompleteListBox.Visible = false;
            //autoCompleteListBox.Items.Clear();
            DataGridViewRow row = purchaseDataGridView.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            if (cell.Value == null)
                return;

            //Если редактировался Артикул или Название
            #region Articul Or Title
            if (cell.OwningColumn.Name == "Title" || cell.OwningColumn.Name == "Articul")
            {
                //убираем события с заполненной клетки.
                if (textBoxCell != null)
                {
                    textChangedEvent = previewKeyDownEvent = false;
                     
                    textBoxCell.TextChanged    -= dataGridViewTextBoxCell_TextChanged;
                    textBoxCell.PreviewKeyDown -= dataGridViewTextBoxCell_PreviewKeyDown;
                }
                //Если есть такой товар в базе.
                if (searchSparePartsList.Count > 0)
                {
                    string[] titleAndArticul = (cell.Value as string).Split(new string[] { "     " }, StringSplitOptions.RemoveEmptyEntries);
                    string title, articul;
                    //если выбор сделан из выпадающего списка.
                    if (titleAndArticul.Length == 2)
                    {

                        title   = cell.OwningColumn.Name == "Title" ? titleAndArticul[0] : titleAndArticul[1];
                        articul = cell.OwningColumn.Name == "Title" ? titleAndArticul[1] : titleAndArticul[0];        
                        
                        //находим из списка нужную запчасть.
                        var sparePartsList = (from sp in searchSparePartsList
                                              where sp.Title.Trim() == title.Trim() && sp.Articul.Trim() == articul.Trim()
                                              select sp).ToList<SparePart>();

                        //Если такой товар найден в вып. списке.
                        if (sparePartsList.Count > 0)
                        {
                            currentSparePart = sparePartsList[0];
                            spareParts.Add(currentSparePart);

                            row.Cells["SparePartId"].Value  = currentSparePart.SparePartId;
                            row.Cells["Title"].Value        = currentSparePart.Title;
                            row.Cells["Articul"].Value      = currentSparePart.Articul;
                            row.Cells["Unit"].Value         = currentSparePart.Unit;

                            //Добавляем Id товара в список добавленных в таблицу, для избежания дальнейшего вывода в вып. списке.
                            sparePartsId.Add(currentSparePart.SparePartId);

                            cell.OwningRow.Cells["Price"].ReadOnly = cell.OwningRow.Cells["Count"].ReadOnly   = false;
                            cell.OwningRow.Cells["Title"].ReadOnly = cell.OwningRow.Cells["Articul"].ReadOnly = true;

                            _userText = null;
                            #region Увеличение PurchaseGroupBox.
                            //if (purchaseDataGridView.PreferredSize.Height > purchaseDataGridView.Size.Height)
                            //{
                            //    MessageBox.Show("bigger");
                            //    int height = purchaseDataGridView.Rows[0].Cells["Title"].Size.Height;
                            //    purchaseGroupBox.Size = new Size(purchaseGroupBox.Width, purchaseGroupBox.Height + height);
                            //}
                            #endregion
                        }//if
                    }//if
                    else  //если выбор не из вып. списка.
                        if (titleAndArticul.Length == 1)
                        {
                            if (searchSparePartsList.Count == 1) //если этот товар уникален.
                            {
                                //находим из списка нужную запчасть.
                                var sparePartsList = (from sp in searchSparePartsList
                                                      where sp.Title == titleAndArticul[0] || sp.Articul == titleAndArticul[0]
                                                      select sp).ToList<SparePart>();

                                if (sparePartsList.Count > 0) //если введенный товар именно тот что в списке.
                                {
                                    currentSparePart = sparePartsList[0];
                                    spareParts.Add(currentSparePart);

                                    row.Cells["SparePartId"].Value = currentSparePart.SparePartId;
                                    row.Cells["Title"].Value = currentSparePart.Title;
                                    row.Cells["Articul"].Value = currentSparePart.Articul;
                                    row.Cells["Unit"].Value = currentSparePart.Unit;

                                    //Добавляем Id товара в список добавленных в таблицу, для избежания дальнейшего вывода в вып. списке.
                                    sparePartsId.Add(currentSparePart.SparePartId);

                                    cell.OwningRow.Cells["Price"].ReadOnly = cell.OwningRow.Cells["Count"].ReadOnly = false;
                                    cell.OwningRow.Cells["Title"].ReadOnly = cell.OwningRow.Cells["Articul"].ReadOnly = true;

                                    _userText = null;
                                }//if
                                else
                                { 
                                    //Если в вып. списке есть единственный вариант, но совпадения с ним нет.
                                    if (DialogResult.Yes == MessageBox.Show("Добавить новую единицу товара в базу?",
                                                    "Выберите из списка или добавьте новую единицу в базу!",
                                                    MessageBoxButtons.YesNo))
                                    {
                                        //Если выбран вариант добавить единицу товара в базу данных.
                                        if (DialogResult.OK == new AddSparePartForm().ShowDialog(this))
                                        {
                                            //Если единица товара добавлена в базу.
                                            lastEditCell.Value = null;
                                            isCellEditError = true;
                                            return;
                                        }//if
                                        else
                                        {
                                            // Если единица товара не добавлена в базу.
                                            isCellEditError = true; autoCompleteListBox.Visible = true;
                                            if (previewKeyDownEvent == false)
                                            {
                                                previewKeyDownEvent = true;
                                                textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                                textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                            }//if
                                        }//else
                                    }//if
                                    else
                                    {
                                        isCellEditError = true; autoCompleteListBox.Visible = true;
                                        if (previewKeyDownEvent == false)
                                        {
                                            previewKeyDownEvent = true;
                                            textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                            textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                        }//if
                                    }//else
                                }//else
                            }//if (если этот товар уникален)
                            else
                            {
                                //Если в вып. списке есть единственный вариант, но совпадения с ним нет.
                                if (DialogResult.Yes == MessageBox.Show("Добавить новую единицу товара в базу?",
                                                "Выберите из списка или добавьте новую единицу в базу!",
                                                MessageBoxButtons.YesNo))
                                {
                                    //Если выбран вариант добавить единицу товара в базу данных.
                                    if (DialogResult.OK == new AddSparePartForm().ShowDialog(this))
                                    {
                                        //Если единица товара добавлена в базу.
                                        autoCompleteListBox.Visible = true;
                                        lastEditCell.Value = null;
                                        isCellEditError = true;
                                        if (previewKeyDownEvent == false)
                                        {
                                            previewKeyDownEvent = true;
                                            textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                            textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                        }//if
                                        return;
                                    }//if
                                    else
                                    {
                                        // Если единица товара не добавлена в базу.
                                        isCellEditError = true; autoCompleteListBox.Visible = true;
                                        if (previewKeyDownEvent == false)
                                        {
                                            previewKeyDownEvent = true;
                                            textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                            textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                        }//if
                                    }//else
                                }//if 
                                else 
                                {
                                    isCellEditError = true; autoCompleteListBox.Visible = true;
                                    if (previewKeyDownEvent == false)
                                    {
                                        previewKeyDownEvent = true;
                                        textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                        textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                    }//if
                                }
                            }//else  
                        }//if
                }//if
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Нет такого товара в базе, добавить новую единицу в базу?", 
                                                            "Товар не найден", MessageBoxButtons.YesNo))
                    {
                        //Если выбран вариант добавить единицу товара в базу данных.
                        if (DialogResult.OK == new AddSparePartForm().ShowDialog(this))
                        {
                            //Если единица товара добавлена в базу.
                            lastEditCell.Value = null;
                            isCellEditError = true;
                            return;
                        }
                        else
                        {
                            // Если единица товара не добавлена в базу.
                            //textBoxCell.Clear();
                            lastEditCell.Value = null;
                            isCellEditError = true;
                            return;
                        }

                    }
                    else
                    {
                        //Если выбран вариант не добавлять единицу товара в базу данных.
                        //textBoxCell.Clear();
                        lastEditCell.Value = null;
                        isCellEditError = true;
                        return;
                    }                
                }//else
            }//if
            #endregion
            //Если редактируется цена
            #region Count Or Price.
            if (cell.OwningColumn.Name == "Price")
            {
                if (cell.Value != null) //Если строка не пустая, проверить корректность ввода.
                {
                    try
                    {
                        double price = Convert.ToDouble(cell.Value);
                        if (price == 0) throw new Exception();            //ввод нуля также является ошибкой.

                        //Если цена вводится в той же строке.
                        int sparePartId = Convert.ToInt32(cell.OwningRow.Cells["SparePartId"].Value);
                        if (sparePartId != currentSparePart.SparePartId)
                        {
                            currentSparePart = spareParts.Where(sparePart => sparePart.SparePartId == sparePartId).First();
                            //foreach (var sparePart in spareParts)
                                //if (sparePart.SparePartId == sparePartId)
                                    //currentSparePart = sparePart;                            
                        }

                        //Округляем Price до 2-х десятичных знаков.
                        price = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                        currentSparePart.Price = price;
                        cell.Value = String.Format("{0:N2}", price);

                        amountCalculation(cell.OwningRow);

                        //Присваиваем автоматическую наценку равную розничной цене. 
/*!!!*/                 RowMarkupChanges(row, "Розница"); //!!!Костыль! Необх-мо пометить какую-то запись в табл. Markups как дефолтную и присваивать её здесь.
                    }//try
                    catch
                    {
                        //выводим всплывающее окно с сообщением об ошибке.
                        toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);                        
                        //Очищаем ввод.
                        cell.Value = null;
                        isCellEditError = true;
                        lastEditCell = cell;
                    }//catch
                }//if                
            }//if
            if (cell.OwningColumn.Name == "Count")
            {
                if (cell.Value != null) //Если строка не пустая, проверить корректность ввода.
                {
                    try
                    {
                        double count = Convert.ToDouble(cell.Value);
                        if (count == 0) throw new Exception();            //ввод нуля также является ошибкой.
                        if (count % PartsDAL.FindMinUnitSaleOfUnit(cell.OwningRow.Cells["Unit"].Value as string) != 0)
                            throw new Exception();

                        int sparePartId = Convert.ToInt32(cell.OwningRow.Cells["SparePartId"].Value);
                        if (sparePartId != currentSparePart.SparePartId)
                        {
                            currentSparePart = spareParts.Where(sparePart => sparePart.SparePartId == sparePartId).First();
                            //foreach (var sparePart in spareParts)
                              //  if (sparePart.SparePartId == sparePartId)
                                //    currentSparePart = sparePart;
                        }//if
                        currentSparePart.Count = count;

                        amountCalculation(cell.OwningRow);
                    }//try
                    catch
                    {
                        //выводим всплывающее окно с сообщением об ошибке.
                        toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                        //Очищаем ввод.
                        cell.Value = null;
                        isCellEditError = true;
                        lastEditCell = cell;
                    }//catch
                }//if            
            }//if
            #endregion
            //Если ред-ся цена продажи.
            #region SellingPrice.
            if (cell.OwningColumn.Name == SellingPrice.Name)
            {
               ////////////         
            
            }//if (SellingPrice)


            #endregion
        }//purchaseDataGridView_CellEndEdit                                
        /// <summary>
        /// Метод расчета суммы в передаваемой строке.
        /// </summary>
        /// <param name="row">Строка, в которой требуется рассчитать сумму</param>
        private void amountCalculation(DataGridViewRow row)
        {
            if (currentSparePart.Count != 0 && currentSparePart.Price != null)
            {
                //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
                if (row.Cells[Sum.Name].Value != null)
                    inTotal -= Convert.ToDouble((row.Cells[Sum.Name].Value));

                //Рассчитываем сумму и отображаем в таблице.
                double sum = Math.Round((double)currentSparePart.Price * currentSparePart.Count, 2, MidpointRounding.AwayFromZero);
                row.Cells[Sum.Name].Value = String.Format("{0:N2}", sum);

                //Меняем значение "Итого".
                inTotal += sum;
                inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);

                //Запрещаем дальнейшее редактирование кол-ва и цены.
                row.Cells[Price.Name].ReadOnly = row.Cells[Count.Name].ReadOnly = true;
            }//if        
        }//amountCalculation()
        private void purchaseDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isCellEditError == true)
            {
                isCellEditError = false;
                purchaseDataGridView.CurrentCell = lastEditCell;                
                //if (lastEditCell.ReadOnly) lastEditCell.ReadOnly = false;

                purchaseDataGridView.CellBeginEdit -= purchaseDataGridView_CellBeginEdit;
                purchaseDataGridView.EditingControlShowing -= purchaseDataGridView_EditingControlShowing;
                purchaseDataGridView.BeginEdit(true);
                purchaseDataGridView.CellBeginEdit += purchaseDataGridView_CellBeginEdit;
                purchaseDataGridView.EditingControlShowing += purchaseDataGridView_EditingControlShowing;

                textBoxCell.SelectionStart = textBoxCell.Text.Length;                
            }//if
        }//purchaseDataGridView_SelectionChanged

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (purchaseDataGridView.AreAllCellsSelected(false) == true)
            {
                if (DialogResult.Yes == MessageBox.Show("Вы хотите полностью очистить список?", "", MessageBoxButtons.YesNo))
                {
                    purchaseDataGridView.Rows.Clear();
                    spareParts.Clear();
                    searchSparePartsList.Clear(); //надо ли?
                    sparePartsId.Clear();

                    //очищаем "Итого".
                    inTotal = 0;
                }//if
            }//if
            else
            {
                int sparePartId = Convert.ToInt32(lastEditCell.OwningRow.Cells["SparePartId"].Value);
                for (int i = 0; i < spareParts.Count; ++i)
                    if (spareParts[i].SparePartId == sparePartId)
                    {
                        spareParts.RemoveAt(i);
                        sparePartsId.RemoveAt(i);
                    }//if

                //исправляем "Итого".
                if (lastEditCell.OwningRow.Cells[Sum.Name].Value != null)
                    inTotal -= Convert.ToDouble((lastEditCell.OwningRow.Cells[Sum.Name].Value));

                //Удаляем строку.
                purchaseDataGridView.Rows.Remove(lastEditCell.OwningRow);
            }//else

            //Выводим "Итого".
            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
        }//removeToolStripMenuItem_Click













        /////////////////////////////////Вспомогательные методы./////////////////////////
        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из purchaseDataGridView. 
        /// </summary>
        /// <param name="cell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = purchaseDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = purchaseDataGridView.Location;
            Point gbLoc = purchaseGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);        
        }//GetCellBelowLocation









//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы вывода инф-ции в Excel.

        private void BeginLoadPurchaseToExcelFile(object purchase)
        {
            if (purchase is Purchase)
                LoadPurchaseToExcelFile(purchase as Purchase, spareParts);
        }//BeginLoadPurchaseToExcelFile
        ///// <summary>
        ///// Метод вывода приходной информации в Excel-файл.
        ///// </summary>
        ///// <param name="sale">Информация о приходе.</param>
        ///// <param name="spareParts">Список оприходованных товаров.</param>
        //private void LoadPurchaseToExcelFile(Purchase sale, IList<SparePart> spareParts)
        //{
        //    Excel.Application ExcelApp = new Excel.Application();
        //    Excel.Workbook ExcelWorkBook;
        //    Excel.Worksheet ExcelWorkSheet;
        //    //Книга.
        //    ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    //Таблица.
        //    ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

        //    int row = 1, column = 1;

        //    //Выводим Id и Дату. 
        //    ExcelApp.Cells[row, column] = String.Format("Приходная накладная №{0} от {1}г.", sale.PurchaseId, sale.PurchaseDate.ToString("dd/MM/yyyy"));
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Bold = true;
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 18;

        //    //Выводим поставщика.
        //    row += 2;
        //    ExcelApp.Cells[row, column] = String.Format("Поставщик:   \t{0}", supplierTextBox.Text);//PartsDAL.FindSupplierNameById(sale.SupplierId));
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 12;

        //    //Выводим покупателя.
        //    row += 2;
        //    ExcelApp.Cells[row, column] = String.Format("Покупатель:  \t{0}", buyerTextBox.Text);
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 12;

        //    //Выводим таблицу товаров.
        //    //Выводим заголовок.
        //    row += 2;
        //    ExcelApp.Cells[row, column] = "Название";
        //    ExcelApp.Cells[row, column+1] = "Ед. изм.";
        //    ExcelApp.Cells[row, column+2] = "Кол-во";
        //    ExcelApp.Cells[row, column+3] = "Цена";
        //    ExcelApp.Cells[row, column+4] = "Сумма";

        //    Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "E" + row.ToString());
        //    excelCells.Font.Bold = true;
        //    excelCells.Font.Size = 12;
        //    //Обводим заголовки таблицы рамкой. 
        //    excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
        //    //Устанавливаем стиль и толщину линии
        //    //excelCells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        //    excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

        //    //Устанавливаем ширину первой Колонки для Title.
        //    double width = 45; //45 -- Взято методом тыка.           
        //    (ExcelApp.Cells[row, column] as Excel.Range).Columns.ColumnWidth = width;
        //    //Выводим список товаров.
        //    for (int i = 0; i < spareParts.Count; ++i)
        //    {
        //        ++row;                
        //        ExcelApp.Cells[row, column] = spareParts[i].Title;
        //        //Если Title не влазиет в одну строку, увеличиваем высоту.
        //        if (spareParts[i].Title.Length > width)
        //        {
        //            (ExcelApp.Cells[row, column] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
        //            ExcelWorkSheet.get_Range("B" + row.ToString(), "E" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
        //        }
        //        ExcelApp.Cells[row, column + 1] = spareParts[i].Unit;
        //        ExcelApp.Cells[row, column + 2] = spareParts[i].Count;
        //        ExcelApp.Cells[row, column + 3] = spareParts[i].Price;
        //        ExcelApp.Cells[row, column + 4] = spareParts[i].Price * spareParts[i].Count;
        //        //Выравнивание диапазона строк.
        //        ExcelWorkSheet.get_Range("B" + row.ToString(), "E" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; 
        //    }//for

        //    //Обводим талицу рамкой. 
        //    excelCells = ExcelWorkSheet.get_Range("A" + (row - spareParts.Count + 1).ToString(), "E" + row.ToString());
        //    excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

        //    //Выводим "Итого".
        //    ++row;            
        //    //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
        //    int indent = 0; //отступ
        //    if (inTotalNumberLabel.Text.Length <= 9)
        //        indent = 1;

        //    ExcelApp.Cells[row, column + 2 + indent] = inTotalLabel.Text;
        //    ExcelApp.Cells[row, column + 3 + indent] = inTotalNumberLabel.Text; //inTotal;// 
        //    (ExcelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Underline = true;
        //    (ExcelApp.Cells[row, column + 2 + indent] as Excel.Range).Font.Size = (ExcelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Size = 12;
        //    (ExcelApp.Cells[row, column + 2 + indent] as Excel.Range).Font.Bold = (ExcelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Bold = true;

        //    //Выводим имена агентов.
        //    row += 2;
        //    ExcelApp.Cells[row, column]     = String.Format("\t{0} {1} ", supplierAgentLabel.Text, supplierAgentTextBox.Text);
        //    ExcelApp.Cells[row, column + 1] = String.Format("{0} {1}",  buyerAgentLabel.Text, buyerAgentTextBox.Text);

        //    //Вызываем нашу созданную эксельку.
        //    ExcelApp.Visible = true;
        //    ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
        //    ExcelApp.UserControl = true;

        //    this.Close();
        //}//LoadPurchaseToExcelFile        
        /// <summary>
        /// Метод вывода приходной информации в Excel-файл.
        /// </summary>
        /// <param name="sale">Информация о приходе.</param>
        /// <param name="spareParts">Список оприходованных товаров.</param>
        private void LoadPurchaseToExcelFile(Purchase purchase, IList<SparePart> spareParts)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin   = 7;
            ExcelWorkSheet.PageSetup.RightMargin  = 7;
            ExcelWorkSheet.PageSetup.TopMargin    = 10;
            ExcelWorkSheet.PageSetup.BottomMargin = 10;

            int row = 1, column = 1;

            //Выводим Id и Дату. 
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Приходная накладная №{0} от {1}г.", purchase.OperationId, purchase.OperationDate.ToString("dd/MM/yyyy"));

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}", 
                                                         supplierLabel.Text + " " + supplierTextBox.Text,
                                                         buyerLabel.Text + " " + buyerTextBox.Text);

            #region Вывод таблицы товаров.
            row += 2;
            //Выводим заголовок.
            ExcelApp.Cells[row, column] = "Произв.";
            ExcelApp.Cells[row, column + 1] = "Артикул";
            ExcelApp.Cells[row, column + 2] = "Название";
            ExcelApp.Cells[row, column + 3] = "Ед. изм.";
            ExcelApp.Cells[row, column + 4] = "Кол-во";
            ExcelApp.Cells[row, column + 5] = "Цена";
            ExcelApp.Cells[row, column + 6] = "Сумма";
            
            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Font.Bold = true;
            excelCells.Font.Size = 12;
            //Уменьшаем ширину колонки "Ед. изм."
            (ExcelApp.Cells[row, column + 3] as Excel.Range).Cells.VerticalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            (ExcelApp.Cells[row, column + 3] as Excel.Range).Columns.ColumnWidth = 5;
            //Обводим заголовки таблицы рамкой. 
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
            //Устанавливаем стиль и толщину линии
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            //int manufColWidth = 15, minManufColWidth = 8; //  15 -- Взято методом тыка.

            SetColumnsWidth(spareParts, (ExcelApp.Cells[row, column + 2] as Excel.Range), (ExcelApp.Cells[row, column + 1] as Excel.Range), (ExcelApp.Cells[row, column] as Excel.Range));
            //Выводим список товаров.
            for (int i = 0; i < spareParts.Count; ++i)
            {
                ++row;
                ExcelApp.Cells[row, column + 2] = spareParts[i].Title;
                ExcelApp.Cells[row, column + 1] = spareParts[i].Articul;
                //Выравнивание диапазона строк.
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
                if (spareParts[i].Articul.Length > articulColWidth || spareParts[i].Title.Length > titleColWidth)
                {
                    ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
                    //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
                    if (spareParts[i].Articul.Length > articulColWidth && spareParts[i].Title.Length <= titleColWidth)
                        (ExcelApp.Cells[row, column + 2] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    if (spareParts[i].Articul.Length <= articulColWidth && spareParts[i].Title.Length > titleColWidth)
                        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }//if

                ExcelApp.Cells[row, column] = spareParts[i].Manufacturer;

                ExcelApp.Cells[row, column + 3] = spareParts[i].Unit;
                ExcelApp.Cells[row, column + 4] = spareParts[i].Count;
                ExcelApp.Cells[row, column + 5] = spareParts[i].Price;
                ExcelApp.Cells[row, column + 6] = spareParts[i].Price * spareParts[i].Count;
            }//for

            //Обводим талицу рамкой. 
            excelCells = ExcelWorkSheet.get_Range("A" + (row - spareParts.Count + 1).ToString(), "G" + row.ToString());
            excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            //Выводим "Итого".
            ++row;
            //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            int indent = 0; //отступ
            if (inTotalNumberLabel.Text.Length <= 9)
                indent = 1;

            ExcelApp.Cells[row, column + 4 + indent] = inTotalLabel.Text;
            ExcelApp.Cells[row, column + 5 + indent] = inTotalNumberLabel.Text;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Underline = true;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Size = (ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Size = 12;
            (ExcelApp.Cells[row, column + 5 + indent] as Excel.Range).Font.Bold = (ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Bold = true;

            #endregion

            #region Вывод таблицы товаров старый.
            
            ////Выводим заголовок.
            //row += 2;
            //ExcelApp.Cells[row, column] = "Произв.";
            //ExcelApp.Cells[row, column + 1] = "Название";
            //ExcelApp.Cells[row, column + 2] = "Ед. изм.";
            //ExcelApp.Cells[row, column + 3] = "Кол-во";
            //ExcelApp.Cells[row, column + 4] = "Цена";
            //ExcelApp.Cells[row, column + 5] = "Сумма";

            //excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString());
            //excelCells.Font.Bold = true;
            //excelCells.Font.Size = 12;
            ////Обводим заголовки таблицы рамкой. 
            //excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
            ////Устанавливаем стиль и толщину линии
            //excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

            ////Устанавливаем ширину первой Колонки для Title.
            //double titleColWidth = 50; //50 -- Взято методом тыка.         
            //int manufColWidth = 15;    //15 -- Взято методом тыка.
            //(ExcelApp.Cells[row, column] as Excel.Range).Columns.ColumnWidth = manufColWidth; //titleColWidth;
            //(ExcelApp.Cells[row, column + 1] as Excel.Range).Columns.ColumnWidth = titleColWidth; //manufColWidth;
            ////Выводим список товаров.
            //for (int i = 0; i < spareParts.Count; ++i)
            //{
            //    ++row;
            //    ExcelApp.Cells[row, column + 1] = spareParts[i].Title;
            //    //Если Title не влазиет в одну строку, увеличиваем высоту.
            //    if (spareParts[i].Title.Length > titleColWidth)
            //    {
            //        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
            //        ExcelWorkSheet.get_Range("A" + row.ToString(), "F" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
            //    }
            //    ExcelApp.Cells[row, column] = spareParts[i].Manufacturer;
            //    ExcelApp.Cells[row, column + 2] = spareParts[i].Unit;
            //    ExcelApp.Cells[row, column + 3] = spareParts[i].Count;
            //    ExcelApp.Cells[row, column + 4] = spareParts[i].Price;
            //    ExcelApp.Cells[row, column + 5] = spareParts[i].Price * spareParts[i].Count;
            //    //Выравнивание диапазона строк.
            //    ExcelWorkSheet.get_Range("C" + row.ToString(), "F" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            //}//for

            ////Обводим талицу рамкой. 
            //excelCells = ExcelWorkSheet.get_Range("A" + (row - spareParts.Count + 1).ToString(), "F" + row.ToString());
            //excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

            ////Выводим "Итого".
            //++row;
            ////В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
            //int indent = 0; //отступ
            //if (inTotalNumberLabel.Text.Length <= 9)
            //    indent = 1;

            //ExcelApp.Cells[row,  column + 3 + indent] = inTotalLabel.Text;
            //ExcelApp.Cells[row,  column + 4 + indent] = inTotalNumberLabel.Text;
            //(ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Underline = true;
            //(ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Size = (ExcelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Size = 12;
            //(ExcelApp.Cells[row, column + 4 + indent] as Excel.Range).Font.Bold = (ExcelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Bold = true;

            #endregion

            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas"; //моноширинный шрифт
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-50}{1}", 
                                                         supplierAgentLabel.Text + " " + supplierAgentTextBox.Text,
                                                         buyerAgentLabel.Text + " " + buyerAgentTextBox.Text);
            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;

            ExcelApp.Cells[row, column].Value = "                                                                                                                                                                                                                                 ";//longEmptyString.ToString();
            (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
            //Выводим заметку
            row++;
            // объединим область ячеек  строки "вместе"
            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.WrapText = true;
            excelCells.Value = purchase.Description;//descriptionRichTextBox.Text;
            AutoFitMergedCellRowHeight((ExcelApp.Cells[row, column] as Excel.Range));

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;

            this.Close();
        }//LoadPurchaseToExcelFile  

        /// <summary>
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="spareParts">Коллекция эл-тов заполняюхий таблицу</param>
        /// <param name="titleCol">Столбец "Название".</param>
        /// <param name="articulCol">Столбец "Артикул".</param>
        /// <param name="manufCol">Столбец "Производитель".</param>
        private void SetColumnsWidth(IList<SparePart> spareParts, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = spareParts.Select(sp => sp.Manufacturer).Where(man => man != null);
            if (sparePartsManufacturers.Count() > 0)
                maxManufLenght = sparePartsManufacturers.Max(man => man.Length);

            if (maxManufLenght < manufColWidth)
            {
                int different = manufColWidth - maxManufLenght; //разница между дефолтной шириной столбца и фактической.
                titleColWidth += (manufColWidth - different < minManufColWidth) ? minManufColWidth : different;
                manufColWidth = (manufColWidth - different < minManufColWidth) ? minManufColWidth : manufColWidth - different;
            }//if

            manufCol.Columns.ColumnWidth = manufColWidth;
            articulCol.Columns.ColumnWidth = articulColWidth;
            titleCol.Columns.ColumnWidth = titleColWidth;
        }//SetColumnsWidth


        private void AutoFitMergedCellRowHeight(Excel.Range rng)
        {
            double mergedCellRgWidth = 0;
            double rngWidth, possNewRowHeight;

            if (rng.MergeCells)
            {
                // здесь использована самописная функция перевода стиля R1C1 в A1                
                if (xlRCtoA1(rng.Row, rng.Column) == xlRCtoA1(rng.Range["A1"].Row, rng.Range["A1"].Column))
                {
                    rng = rng.MergeArea;
                    if (rng.Rows.Count == 1 && rng.WrapText == true)
                    {
                        (rng.Parent as Excel._Worksheet).Application.ScreenUpdating = false;
                        rngWidth = rng.Cells.Item[1, 1].ColumnWidth;
                        mergedCellRgWidth = GetRangeWidth(rng);
                        rng.MergeCells = false;
                        rng.Cells.Item[1, 1].ColumnWidth = mergedCellRgWidth;
                        rng.EntireRow.AutoFit();
                        possNewRowHeight = rng.RowHeight;
                        rng.Cells.Item[1, 1].ColumnWidth = rngWidth;
                        rng.MergeCells = true;
                        rng.RowHeight = possNewRowHeight;
                        (rng.Parent as Excel._Worksheet).Application.ScreenUpdating = true;
                    }//if
                }//if                
            }//if
        }//AutoFitMergedCellRowHeight


        /// <summary>
        /// Возвращает ширину заданной области.
        /// </summary>
        /// <param name="rng">Область ширина которой считается.</param>
        /// <returns></returns>
        private double GetRangeWidth(Excel.Range rng)
        {
            double rngWidth = 0;
            for (int i = 1; i <= rng.Columns.Count; ++i)
            {
                rngWidth += rng.Cells.Item[1, i].ColumnWidth;
            }//for
            return rngWidth;
        }//GetRangeWidth

        private string xlRCtoA1(int ARow, int ACol, bool RowAbsolute = false, bool ColAbsolute = false)
        {
            int A1 = 'A' - 1;  // номер "A" минус 1 (65 - 1 = 64)
            int AZ = 'Z' - A1; // кол-во букв в англ. алфавите (90 - 64 = 26)

            int t, m;
            string S;

            t = ACol / AZ; // целая часть
            m = (ACol % AZ); // остаток?
            if (m == 0)
                t--;
            if (t > 0)
                S = Convert.ToString((char)(A1 + t));
            else S = String.Empty;

            if (m == 0)
                t = AZ;
            else t = m;

            S = S + (char)(A1 + t);

            //весь адрес.
            if (ColAbsolute) S = '$' + S;
            if (RowAbsolute) S = S + '$';

            S = S + ARow.ToString();
            return S;
        }//xlRCtoA1








        #endregion

        private void currencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyComboBox.Text == "руб")
            {
                excRateNumericUpDown.Value = 1;
                excRateNumericUpDown.Enabled = false;
                purchaseDataGridView.Enabled = true;
                currencyComboBox.Enabled = false;
                helpLabel.Dispose();
            }//if
            else excRateNumericUpDown.Enabled = true;

            markupCheckBox.Enabled = true;
        }//currencyComboBox_SelectedIndexChanged

        private void excRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (excRateNumericUpDown.Value > excRateNumericUpDown.Minimum)
            {
                purchaseDataGridView.Enabled = true;
            }
            else purchaseDataGridView.Enabled = false;
            //foreach (DataGridViewRow row in purchaseDataGridView.Rows)
            //{
            //    if (row.Cells["Price"].Value != null)
            //    {
            //        row.Cells["Price"].Value = Math.Round(((double)row.Cells["Price"].Value * (double)excRateNumericUpDown.Value), 2, MidpointRounding.AwayFromZero);                                                           
            //        double price = Convert.ToDouble(row.Cells["Price"].Value);
            //        double excRate = (double)excRateNumericUpDown.Value;
            //        double newPrice = Math.Round(price * excRate, 2, MidpointRounding.AwayFromZero);
            //        row.Cells["Price"].Value = newPrice;
            //        if (row.Cells["Count"].Value != null)
            //        {
            //            //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
            //            if (row.Cells["Sum"].Value != null)
            //                inTotal -= Convert.ToDouble((row.Cells["Sum"].Value));

            //            row.Cells["Sum"].Value = newPrice * Convert.ToDouble(row.Cells["Count"].Value);
            //            inTotal += newPrice * Convert.ToDouble(row.Cells["Count"].Value);
            //            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
            //        }//if
            //    }//if
            //}//foreach
        }//excRateNumericUpDown_ValueChanged

        private void excRateNumericUpDown_Leave(object sender, EventArgs e)
        {
            if (excRateNumericUpDown.Value > excRateNumericUpDown.Minimum)
            {
                excRateNumericUpDown.Enabled = false;
                //purchaseDataGridView.Enabled = true;
                currencyComboBox.Enabled = false;
                helpLabel.Dispose();
            }//if
            else toolTip.Show("Выберите курс к рос. рублю", this, excRateNumericUpDown.Location, 3000);
        }//excRateNumericUpDown_Leave       
        //Модифицировать.
        private void markupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (markupCheckBox.CheckState == CheckState.Checked)
            //Если ничего не введено то ничего не включаем.
            if (spareParts.Count == 0) 
            {
                markupCheckBox.CheckedChanged -= markupCheckBox_CheckedChanged;
                markupCheckBox.CheckState = CheckState.Unchecked;
                markupCheckBox.CheckedChanged += markupCheckBox_CheckedChanged;
                toolTip.Show("Введите данные в таблицу", this, markupCheckBox.Location, 3000);
                return; 
            }//if
            //Если в таблице есть данные, проверяем везде ли указана цена и количество.
            foreach (var sparePart in spareParts)
                if (sparePart.Price == null || sparePart.Count == 0)
                {
                    markupCheckBox.CheckedChanged -= markupCheckBox_CheckedChanged;
                    markupCheckBox.CheckState = CheckState.Unchecked;
                    markupCheckBox.CheckedChanged += markupCheckBox_CheckedChanged;
                    toolTip.Show("Не везде указана цена или количество товара", this, markupCheckBox.Location, 3000);
                    return;
                }//if

            bool visible = purchaseDataGridView.Columns["Markup"].Visible;

            purchaseDataGridView.Columns["Markup"].Visible = !visible;
            purchaseDataGridView.Columns["SellingPrice"].Visible = !visible;
            markupComboBox.Visible = !visible;
        }//markupCheckBox_CheckedChanged     

        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                markupComboBox_SelectedIndexChanged(sender, e);
        }//markupComboBox_PreviewKeyDown 

        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (purchaseDataGridView.SelectedCells.Count == 0) return;

            //выделяем строки всех выделенных клеток.
            foreach (DataGridViewCell cell in purchaseDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            try
            {
                //узнаем процент заданной наценки.
                //double markup = MarkupTypes.GetMarkupValue(markupComboBox.Text);

                foreach (DataGridViewRow row in purchaseDataGridView.SelectedRows)
                {
                    int sparePartId = (row.Cells["SparePartId"].Value == null) ? -1 : Convert.ToInt32(row.Cells["SparePartId"].Value);

                    if (sparePartId != -1)
                    {
                        //если не указана цена, то наценку не присваиваем.
                        if (row.Cells["Price"].Value == null || row.Cells["Count"].Value == null)
                        {
                            toolTip.Show("В одной или нескольких выбранных строках не указана цена или количество", this, markupCheckBox.Location, 2000);
                            continue;
                        }

                        RowMarkupChanges(row, markupComboBox.Text);
                        //row.Cells["Markup"].Value = MarkupTypes.GetMarkupType(markup);

                        //foreach (SparePart sparePart in spareParts)
                        //    if (sparePart.SparePartId == sparePartId)
                        //    {
                        //        sparePart.Markup = markup;
                        //        //sparePart.ExcRate = (double)excRateNumericUpDown.Value;
                        //        row.Cells["SellingPrice"].Value = sparePart.SellingPrice;
                        //    }
                    }//if
                }//foreach
            }//try
            catch
            {
                toolTip.Show("Введено некорректное значение.", this, markupComboBox.Location, 2000);
            }
        }//markupComboBox_SelectedIndexChanged

        /// <summary>
        /// Метод изменения наценки на заданную.
        /// </summary>
        /// <param name="row">Строка в которой происходит из-ние наценки.</param>
        /// <param name="MarkupType">Тип наценки присваиваемая данной строке.</param>
        private void RowMarkupChanges(DataGridViewRow row, string MarkupType)
        {
            double markup = MarkupTypes.GetMarkupValue(MarkupType);

            row.Cells["Markup"].Value = MarkupTypes.GetMarkupType(markup);

            foreach (SparePart sparePart in spareParts)
            {
                if (sparePart.SparePartId == Convert.ToInt32(row.Cells["SparePartId"].Value));
                {
                    sparePart.Markup = markup;
                    //sparePart.ExcRate = (double)excRateNumericUpDown.Value;
                    row.Cells["SellingPrice"].Value = sparePart.SellingPrice;
                }//if
            }//foreach
        }//RowMarkupChanges

        private void purchaseDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                        purchaseDataGridView.SelectAll();
                    else
                    {
                        lastEditCell = purchaseDataGridView.Rows[e.RowIndex].HeaderCell;
                        //Если строка пустая не делаем ничего.
                        if (lastEditCell.OwningRow.Cells["SparePartId"].Value == null) return;

                        lastEditCell.OwningRow.Selected = true;
                    }
                    Point location = purchaseDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    purchaseContextMenuStrip.Show(purchaseDataGridView, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if
        }//purchaseDataGridView_CellMouseClick        

        private void cancelButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Данные не будут внесены в базу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }//if
        }//cancelButton_MouseClick

        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                supplierTextBox_Leave(null, null);
                storageAdressTextBox_Leave(null, null);
                //receiverTextBox_Leave(null, null);
                storageAdressTextBox_Leave(null, null);

                if (supplierBackPanel.BackColor != Color.Red && buyerBackPanel.BackColor != Color.Red
                    && spareParts.Count != 0 && storageAdressBackPanel.BackColor != Color.Red)
                {   
                    //Проверяем везде ли установлена цена и кол-во. 
                    foreach (var sparePart in spareParts)
                    {
                        if (sparePart.Count == 0 || sparePart.Price == null)
                        {                            
                            toolTip.Show("Не везде указана цена или количество товара", this, okButton.Location, 3000);
                            return;
                        }
                    }//foreach
                    
                    Purchase purchase = new Purchase();
                    purchase.Employee = Form1.CurEmployee;
/*!!!*/             purchase.Contragent = PartsDAL.FindSuppliers().Where(s => s.ContragentName == supplierTextBox.Text).First();
                    purchase.ContragentEmployee = (!String.IsNullOrWhiteSpace(supplierAgentTextBox.Text)) ? supplierAgentTextBox.Text.Trim() : null;
                    purchase.OperationDate = purchaseDateTimePicker.Value;
                    purchase.Currency = currencyComboBox.SelectedItem.ToString();
                    purchase.ExcRate = (double)excRateNumericUpDown.Value;
                    purchase.Description = (!String.IsNullOrWhiteSpace(descriptionRichTextBox.Text)) ? descriptionRichTextBox.Text.Trim() : null;
                    purchase.OperationDetails = spareParts;

                    try
                    {
                        purchase.OperationId = PartsDAL.AddPurchase(purchase);
                    }//try
                    catch(Exception)
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        return;
                    }//catch 

                    //LoadPurchaseToExcelFile(sale, spareParts);
/*!!!*/             new System.Threading.Thread(BeginLoadPurchaseToExcelFile).Start(purchase); //Сделать по нормальному вызов с потоком.

                    this.Visible = false;
                    //this.Close();
                }//if
            }//if
        }

        

        


    }//PurchaseForm
    

}//namespace

/*Задачи*/
//0)!!!Учет выставленного курса валюты.
//1)Сделать редактирование ввода и вывода цены, до двух десятичных знаков!
//2)Добавить возможность задавать наценку и цену продажи вручную.
//3)Грузить список валют из базы!
//4)Сделать наценку отдельным классом.

/*Будущие задачи*/
//1)Сделать нормальную обработку неправильного ввода в dataGridViewCell, а именно возврат курсора в очищенную клетку.
//2)Выпадающий список(listBox) в dataGridView в первый раз принимает неправильный размер.
//3)Посмотреть модификацию вып. списков с DisplayMember и ValueMember.
//4)Чтобы при изменении валюты в inTotalNumberLabel сразу изменялось обозначение (руб) на выбранное. 
//5)Добавить возможность вариации валюты.
//6)Сделать удаление всех выделенных строк.
//7)Сделать автоувеличение PurchaseGroupBox.
//8)Улучшить вывод в Excel в частности:
     //8.1)Колонка "Сумма" при большом числе выводит что-то типо "8е+23", сделать нормально.   
//9)Добавить возможность добавления новой валюты в базу.

#region программный перевод DateTime в Utc.
/*DateTime dt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsInterval = dt.Subtract(dt1970);
            Int32 iSeconds = Convert.ToInt32(tsInterval.TotalSeconds);
            MessageBox.Show(iSeconds.ToString());

            TimeSpan ts = TimeSpan.FromSeconds(iSeconds); //с ним все в порядке
            DateTime dt2 = new DateTime(1970, 1, 1); //вроде с этой даты отсчет идет(?)
            dt2 += ts; //но не прибавляется, дата той же остается
            MessageBox.Show(dt2.ToString());*/
#endregion