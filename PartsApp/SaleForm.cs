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
using PartsApp.Models;

namespace PartsApp
{
    public partial class SaleForm : Form
    {
        IList<SparePart> spareParts = new List<SparePart>();
        IList<SparePart> extSpareParts = new List<SparePart>();
        SparePart currentSparePart = new SparePart();

        IList<SparePart> searchSparePartsList = new List<SparePart>();
        IList<SparePart> extCurrentSparePartsList = new List<SparePart>();
        bool isCellEditError = false;
        DataGridViewCell lastEditCell;

        TextBox textBoxCell;
        bool textChangedEvent = false;
        bool previewKeyDownEvent = false;
        string userText;

        double inTotal;
        IList<int> sparePartsId = new List<int>();   //коллекция для хранения Id того товара, что уже есть в таблице.

        string fullExtCount, fullSaleCount;    //переменная для запоминания полного количества конкретного прихода в extDGV.

        public SaleForm()
        {
            InitializeComponent();
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {           
            //Устанавливаем даты для DateTimePicker.
            saleDateTimePicker.MaxDate = DateTime.Now.Date.AddDays(7);
            saleDateTimePicker.MinDate = saleDateTimePicker.Value = DateTime.Now;

            currencyComboBox.SelectedItem = "руб";
/*!!!*/     customerTextBox.AutoCompleteCustomSource.AddRange(PartsDAL.FindAllCustomersName()); //находим сразу всех, вместо подгрузки по вводу.

            //Вносим все типы наценок в markupComboBox             
            markupComboBox.DataSource = new BindingSource(Models.Markup.GetValues(), null);

            sellerAgentTextBox.Text = String.Format("{0} {1}", Form1.CurEmployee.LastName, Form1.CurEmployee.FirstName);
            sellerAgentTextBox.ReadOnly = true;
        }//saleForm_Load

        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void customerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                customerTextBox_Leave(sender, null);
            }//if
        }//SellerTextBox_PreviewKeyDown

        private void customerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(customerTextBox.Text))
            {
                customerBackPanel.BackColor = customerStarLabel.ForeColor = Color.Red;
                customerTextBox.Clear();
                toolTip.Show("Введите имя/название клиента", this, customerBackPanel.Location, 2000);
                return;
            }//if
            if (customerTextBox.AutoCompleteCustomSource.Contains(customerTextBox.Text)) //Если есть такой клиент в базе
            {
                customerStarLabel.ForeColor = Color.Black;
                customerBackPanel.BackColor = SystemColors.Control;
                //receiverTextBox.Focus();
                sellerLabel.Focus(); //убираем фокус с customerTextBox контрола.
            }//if
            else //если такой поставщик в базе отсутствует.
            {
                customerBackPanel.BackColor = customerStarLabel.ForeColor = Color.Red;
                if (MessageBox.Show("Добавить нового клиента?", "Такого клиента нет в базе!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IContragent customer = new Customer();
                    if (new AddContragentForm(customer).ShowDialog() == DialogResult.OK)
                    {
                        //неэкономно обновляем список клиентов.
                        customerTextBox.Leave -= customerTextBox_Leave;
/*!!!*/                 customerTextBox.AutoCompleteCustomSource.Add(customer.ContragentName);
                        customerTextBox.Text = customer.ContragentName;
                        customerTextBox.Leave += customerTextBox_Leave;
                    }//if
                }//if
            }//else
        }//customerTextBox_Leave

        private void sellerTextBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(sellerTextBox.Text))
            {
                sellerBackPanel.BackColor = sellerStarLabel.ForeColor = Color.Red;
                sellerTextBox.Clear();
                toolTip.Show("Введите имя/название продавца", this, sellerBackPanel.Location, 2000);
            }//if
            else
            {
                sellerStarLabel.ForeColor = Color.Black;
                sellerBackPanel.BackColor = SystemColors.Control;
            }//else
        }//sellerTextBox_Leave

        




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /*Нумерация строк saleDataGridView*/
        private void partsDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = dataGridView.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                dataGridView.Rows[index].HeaderCell.Value = indexStr;

             

/*!!!*/      if (index+1 == 10)    //предусмотрен вариант расширения столбца нумерации строк только до 2 цифр!
                dataGridView.RowHeadersWidth = 41 + 7; //((i - 1) * 7); //41 - изначальный размер RowHeaders
        }//partsDataGridView_RowPrePaint

        private void saleDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Эта строка нужна потому что новые столбцы SellingPrice и Count почему то становятся открытыми для записи.
/*!!!*/      saleDataGridView.Rows[e.RowIndex].Cells[SellingPrice.Name].ReadOnly = saleDataGridView.Rows[e.RowIndex].Cells["Count"].ReadOnly = true;
        }//saleDataGridView_RowsAdded

        #region Обработка событий добавления товара в списки.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*События идут в порядке их возможного вызова.*/

        // Событие для установки listBox в нужную позицию. //
        private void saleDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];
            lastEditCell = cell;
            if (cell.OwningColumn.Name == "Title" || cell.OwningColumn.Name == "Articul")
            {
                //Запрещаем ввод в новую строку, если в предыдущей не были введены кол-во и цена продажи.
                if (cell.OwningRow.Index != 0)
                    if (saleDataGridView.Rows[cell.OwningRow.Index - 1].Cells[SellingPrice.Name].Value == null ||
                        saleDataGridView.Rows[cell.OwningRow.Index - 1].Cells["Count"].Style.ForeColor == Color.Gray)
                        e.Cancel = true;

                autoCompleteListBox.Location = GetCellBelowLocation(cell);

                extDataGridView.Columns[extCount.Name].ReadOnly = false; //Разрешаем ввод кол-ва в доп. таблице.
            }
            //Запрещаем ввод цены, пока не введено кол-во.
            if (cell.OwningColumn.Name == SellingPrice.Name && cell.OwningRow.Cells["Count"].Style.ForeColor == Color.Gray)
                e.Cancel = true;

            //Обрабатываем ввод Количества.
            if (cell.OwningColumn.Name == "Count")
            {
                //Запрещаем ввод кол-ва в данной таблице, если товар только на виртуальном складе. (выбирать в доп. таблице)
                if ((cell.Value as string).IndexOf('(') == 0)  //корявая проверка на вирт. скл., если нулевой индекс это открывающая скобка, значит товар в наличии только на вирт. скл.
                    e.Cancel = true;
                else
                {
                    fullSaleCount = cell.Value as string;
                    cell.Value = null; //очищаем для ввода пользователем.
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.SelectionForeColor = Color.White;
                }
            }//if
        }//saleDataGridView_CellBeginEdit

        //Событие для добавления обработчиков на ввод текста в клетку. //
        private void saleDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {            
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)saleDataGridView.CurrentCell;

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
        }//saleDataGridView_EditingControlShowing

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
                    textBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                    return;
                }
                //Если выбирается первый эл-нт выпадающего списка, запоминаем введенную ранее пользователем строку.
                if (autoCompleteListBox.SelectedIndex == -1)
                    userText = textBox.Text;

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
                    userText = textBox.Text;
                    autoCompleteListBox.SelectedIndex = autoCompleteListBox.Items.Count - 1;
                }
                //Если выбран верхний эл-нт вып. списка, вернуть введенную ранее пользователем строку.
                else if (autoCompleteListBox.SelectedIndex == 0)
                {
                    textBox.Text = userText;
                    autoCompleteListBox.ClearSelected();
                }//if
                else autoCompleteListBox.SelectedIndex -= 1;
                //Если это нулевая строка, то при нажатии Up не происходит событие SelectionChanged, и при выборе из вып. списка каретка ставитс в начало строки, что затрудняет дальнейший ввод поль-лю. Мы вызываем событие искусствунно и ставим каретку в конец строки.                               
                if (lastEditCell.OwningRow.Index == 0) 
                    saleDataGridView_SelectionChanged(sender, null); 

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
                    searchSparePartsList = PartsDAL.SearchSparePartsAvaliablityByTitle(textBox.Text, 10, sparePartsId);
                else if (lastEditCell.OwningColumn.Name == "Articul")
                    searchSparePartsList = PartsDAL.SearchSparePartsAvaliablityByArticul(textBox.Text, 10, sparePartsId);
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
                if (String.IsNullOrEmpty(userText))
                    userText = textBoxCell.Text;
                saleDataGridView_SelectionChanged(null, null);
                isCellEditError = true; 
            }
            else 
            { 
            //    textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
            //    textChangedEvent = true; 
                isCellEditError = false; 
                //dataGridViewTextBoxCell_PreviewKeyDown(textBoxCell, new PreviewKeyDownEventArgs(Keys.Enter));
                //saleDataGridView.Rows[lastEditCell.RowIndex + 1].Cells["Title"].Selected = true;
                saleDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(lastEditCell.ColumnIndex, lastEditCell.RowIndex));
            }
        }

        private void autoCompleteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {           
            textChangedEvent = false;
            if (autoCompleteListBox.SelectedIndex != -1)
                textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
        }//autoCompleteListBox_SelectedIndexChanged

        //Привести в порядок метод!!!!
        private void saleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            if (isCellEditError) return;

            autoCompleteListBox.Visible = false;
            //autoCompleteListBox.Items.Clear();
            DataGridViewRow row = saleDataGridView.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            //Если редактировался Артикул или Название
            #region Articul Or Title
            if (cell.OwningColumn.Name == "Title" || cell.OwningColumn.Name == "Articul")
            {
                if (cell.Value == null) return;
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

                            FillTheBothDGV(row);

                            spareParts.Add(currentSparePart);

                            //Добавляем Id товара в список добавленных в таблицу, для избежания дальнейшего вывода в вып. списке.
                            sparePartsId.Add(currentSparePart.SparePartId);
                            cell.OwningRow.Cells[SellingPrice.Name].ReadOnly = cell.OwningRow.Cells["Count"].ReadOnly   = false;
                            cell.OwningRow.Cells["Title"].ReadOnly = cell.OwningRow.Cells["Articul"].ReadOnly = true;

                            userText = null;
                            #region Увеличение saleGroupBox.
                            //if (saleDataGridView.PreferredSize.Height > saleDataGridView.Size.Height)
                            //{
                            //    MessageBox.Show("bigger");
                            //    int height = saleDataGridView.Rows[0].Cells["Title"].Size.Height;
                            //    saleGroupBox.Size = new Size(saleGroupBox.Width, saleGroupBox.Height + height);
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
                                var sparePartsList = searchSparePartsList;//(from sp in searchSparePartsList
                                                                            //where sp.Title == titleAndArticul[0] || sp.Articul == titleAndArticul[0]
                                                                            //select sp).ToList<SparePart>();

                                if (sparePartsList.Count > 0) //если введенный товар именно тот что в списке.
                                {
                                    currentSparePart = sparePartsList[0];
                                    FillTheBothDGV(row);
                                    spareParts.Add(currentSparePart);

                                    row.Cells["SparePartId"].Value = currentSparePart.SparePartId;
                                    row.Cells["Title"].Value = currentSparePart.Title;
                                    row.Cells["Articul"].Value = currentSparePart.Articul;
                                    row.Cells["Unit"].Value = currentSparePart.MeasureUnit;

                                    //Добавляем Id товара в список добавленных в таблицу, для избежания дальнейшего вывода в вып. списке.
                                    sparePartsId.Add(currentSparePart.SparePartId);

                                    cell.OwningRow.Cells[SellingPrice.Name].ReadOnly = cell.OwningRow.Cells["Count"].ReadOnly = false;
                                    cell.OwningRow.Cells["Title"].ReadOnly = cell.OwningRow.Cells["Articul"].ReadOnly = true;

                                    userText = null;
                                }//if
                            }//if (если этот товар уникален)
                            else
                            {
                                toolTip.Show("Выберите товар из списка.", this, GetCellBelowLocation(cell), 1000);
                                isCellEditError = true; autoCompleteListBox.Visible = true;
                                if (previewKeyDownEvent == false)
                                {
                                    previewKeyDownEvent = true;
                                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                                    textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                                }//if                                    
                            }//else  
                        }//if
                }//if
                else
                {
                    toolTip.Show("Нет такого товара в наличии.", this, GetCellBelowLocation(lastEditCell), 1000);
                    lastEditCell.Value = null;
                    isCellEditError = true;
                    return;                   
                }//else
            }//if
            #endregion
            //Если редактируется цена
            #region Count Or SellingPrice.
            #region SellingPrice
            if (cell.OwningColumn.Name == SellingPrice.Name)
            {
                if (cell.Value != null) //Если строка не пустая, проверить корректность ввода.
                {
                    try
                    {
                        double price = Convert.ToDouble(cell.Value);
                        if (price == 0) throw new Exception();            //ввод нуля также является ошибкой.

                        //Если цена продажи хотя бы где-то ниже закупочной требуем подтверждения действий.
                        foreach (var sparePart in extSpareParts)
                            if (sparePart.SparePartId == currentSparePart.SparePartId && sparePart.Price >= price)
                                if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    break; //получили подтверждение, выполняем действия.
                                else throw new Exception(); //поль-ль отменил свои действия.                                   

                        //Заполняем цену во всех выбранных приходах.
                        foreach (var sparePart in extSpareParts)
                            if (sparePart.SparePartId == currentSparePart.SparePartId)
                                sparePart.SellingPrice = price;

                        //Если цена вводится в той же строке.
                        int sparePartId = Convert.ToInt32(cell.OwningRow.Cells["SparePartId"].Value);
                        if (sparePartId != currentSparePart.SparePartId)
                        {
                            foreach (var sparePart in spareParts)
                                if (sparePart.SparePartId == sparePartId)
                                    currentSparePart = sparePart;                            
                        }//if

                        //Округляем Price до 2-х десятичных знаков.
                        price = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                        currentSparePart.Price = price; //Запоминаем цену в Price, т.к. SellingPrice не заполнится, потому что Price == null (см. SparePart.SellingPrice.Set()).
                        foreach (var sp in extCurrentSparePartsList)
                        {
                            sp.SellingPrice = price;
                        }
                        cell.Value = String.Format("{0:N2}", price);

                        if (currentSparePart.Count != 0 || currentSparePart.VirtCount != 0)
                        {
                            //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
                            if (cell.OwningRow.Cells["Sum"].Value != null)
                                inTotal -= Convert.ToDouble((cell.OwningRow.Cells["Sum"].Value));


                            double sum = Math.Round(price * (currentSparePart.Count + currentSparePart.VirtCount), 2, MidpointRounding.AwayFromZero);
                            cell.OwningRow.Cells["Sum"].Value = String.Format("{0:N2}", sum);//sum; //price * currentSparePart.Count;
                            inTotal += sum; //price * currentSparePart.Count;
                            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);

                            cell.OwningRow.Cells[SellingPrice.Name].ReadOnly = cell.OwningRow.Cells["Count"].ReadOnly = true;
                            
                        }//if                        
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
            #region Count
            if (cell.OwningColumn.Name == "Count")
            {
                try
                {
                    double count = TestInputValueInSaleDGV(cell);

                    //Если введено корректное значение, блокируем возм-ть ввода в extDGV.
                    extDataGridView.Columns[extCount.Name].ReadOnly = true;

                    //Находим нужный товар из списка.
                    int sparePartId = Convert.ToInt32(cell.OwningRow.Cells["SparePartId"].Value);
                    if (sparePartId != currentSparePart.SparePartId)
                    {
                        foreach (var sparePart in spareParts)
                            if (sparePart.SparePartId == sparePartId)
                                currentSparePart = sparePart;
                    }//if
                    currentSparePart.Count = count;
                    //Запрещаем изменение кол-ва после первого корректного ввода.
                    cell.OwningRow.Cells["Count"].ReadOnly = true;

                    if (currentSparePart.Price != null)
                    {
                        if (cell.OwningRow.Cells["Sum"].Value != null)
                            inTotal -= Convert.ToDouble((cell.OwningRow.Cells["Sum"].Value));

                        double sum = Math.Round((double)currentSparePart.Price * count, 2, MidpointRounding.AwayFromZero);
                        cell.OwningRow.Cells["Sum"].Value = sum; //(double)currentSparePart.Price * count;
                        inTotal += sum; //(double)currentSparePart.Price * count;
                        inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);

                        //cell.OwningRow.Cells[SellingPrice.Name].ReadOnly = true;//cell.OwningRow.Cells["Count"].ReadOnly = true;
                        //currentSparePart = null;
                    }//if

                    AutoChoisePurchases();
                }//try
                catch
                {
                    //выводим всплывающее окно с сообщением об ошибке.
                    toolTip.Show("Введены некорректные данные", this, GetCellBelowLocation(cell), 1000);
                    //Очищаем ввод.
                    cell.Style.ForeColor = cell.Style.SelectionForeColor = Color.Gray;                    
                    cell.Value = fullSaleCount;
                    //isCellEditError = true;
                    lastEditCell = cell;
                }//catch  
                           
            }//if
            #endregion
            #endregion
        }//saleDataGridView_CellEndEdit                                

        private void saleDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isCellEditError == true)
            {
                isCellEditError = false;
                saleDataGridView.CurrentCell = lastEditCell;                
                //if (lastEditCell.ReadOnly) lastEditCell.ReadOnly = false;

                saleDataGridView.CellBeginEdit -= saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing -= saleDataGridView_EditingControlShowing;
                saleDataGridView.BeginEdit(true);
                saleDataGridView.CellBeginEdit += saleDataGridView_CellBeginEdit;
                saleDataGridView.EditingControlShowing += saleDataGridView_EditingControlShowing;

                textBoxCell.SelectionStart = textBoxCell.Text.Length;                
            }//if
        }//saleDataGridView_SelectionChanged

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saleDataGridView.AreAllCellsSelected(false) == true)
            {
                if (DialogResult.Yes == MessageBox.Show("Вы хотите полностью очистить список?", "", MessageBoxButtons.YesNo))
                {
                    saleDataGridView.Rows.Clear();
                    extDataGridView.Rows.Clear();
                    spareParts.Clear();
                    extSpareParts.Clear();
                    searchSparePartsList.Clear(); //надо ли?

                    sparePartsId.Clear();

                    //очищаем "Итого".
                    inTotal = 0;
                }//if
            }//if
            else
            {
                int sparePartId = Convert.ToInt32(lastEditCell.OwningRow.Cells["SparePartId"].Value);
                //Очищаем saleDGV.
                for (int i = 0; i < spareParts.Count; ++i)
                    if (spareParts[i].SparePartId == sparePartId)
                    {
                        spareParts.RemoveAt(i);     /*!!!! Тут Кроется большая потенциальная ошибка, потому что при удалении уменьшается счетчик в оп-ре for. Работает корректно пока удаляется только один элемент из списка.*/
                        sparePartsId.RemoveAt(i);
                        break;
                    }//if

                //исправляем "Итого".
                if (lastEditCell.OwningRow.Cells[Sum.Name].Value != null)
                    inTotal -= Convert.ToDouble((lastEditCell.OwningRow.Cells[Sum.Name].Value));

                //Удаляем строку.
                saleDataGridView.Rows.Remove(lastEditCell.OwningRow);
                

                //Очищаем extDGV, если удаляется текущая редактируемая строка.
                if (currentSparePart.SparePartId == sparePartId) 
                    extDataGridView.Rows.Clear();
                //Запоминаем объекты которые нужно удалить из списка.
                var removesSp = new List<SparePart>();
                for (int i = 0; i < extSpareParts.Count; ++i)
                    if (extSpareParts[i].SparePartId == sparePartId)
                        removesSp.Add(extSpareParts[i]);

                //Удаляем эти объекты
                foreach (var sp in removesSp)
                    extSpareParts.Remove(sp);
            }//else

            //Выводим "Итого".
            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
        }//removeToolStripMenuItem_Click




        #region Обработка событий работы с дополнительным списком товаров.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void extDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (extDataGridView.Columns[e.ColumnIndex].Name == "extCount")
            {
                DataGridViewCell cell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                //if (String.IsNullOrEmpty(fullExtCount))
                fullExtCount = cell.Value as string; //если первый ввод, запоминаем полное кол-во.
                cell.Value = null; //очищаем для ввода пользователем.
                cell.Style.SelectionForeColor = Color.White;
                cell.Style.ForeColor = Color.Black;
            }
        }//extDataGridView_CellBeginEdit

        private void extDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Если редактировалась колонка "Кол-во". хотя по идее все остальные readOnly.
            if (extDataGridView.Columns[e.ColumnIndex].Name == "extCount")
            {
                DataGridViewCell cell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                try
                {
                    double count = TestInputValueInExtDGV(cell);
                    cell.ReadOnly = true;

                    //Создаем переменную с информацией о данном конкретном приходе.                    
                    SparePart extCurrentSparePart  = PartsDAL.FindSparePartAvaliability(currentSparePart.SparePartId, Convert.ToInt32(cell.OwningRow.Cells[extPurchaseId.Name].Value));
                    //Очищаем, для запоминания данных введенных поль-лем.
                    extCurrentSparePart.VirtCount = extCurrentSparePart.Count = 0;
                    //Находим нужную клетку в табл. продаж.
                    var saleCountCell = saleDataGridView.Rows[saleDataGridView.Rows.Count - 2].Cells["Count"]; //Находим клетку "количество" предпоследней строки в таблице, потому, что из доп. таблицы изименятся может только она, т.к. все другие будут заблокированны. 
                    //Если содержит скобку то товар с вирт. склада.
                    if (fullExtCount.Contains('('))
                    {
                        currentSparePart.VirtCount += count;
                        extCurrentSparePart.VirtCount = count;
                        cell.Value = extCurrentSparePart.Avaliability;
                    }
                    else
                    {
                        currentSparePart.Count += count;
                        extCurrentSparePart.Count = count;
                    }

                    extSpareParts.Add(extCurrentSparePart);
                    extCurrentSparePartsList.Add(extCurrentSparePart);
                    //Обновляем "кол-во" в таблице продаж.
                    saleCountCell.ReadOnly = true;
                    saleCountCell.Value = currentSparePart.Avaliability;
                    //Обновляем содержимое столбца "Сумма" в saleDGV, если цена уже указана.
                    if (currentSparePart.Price != null)
                    {
                        //Если отпусная цена уже выставленна запоминаем её в выбранном приходе.
                        extCurrentSparePart.SellingPrice = currentSparePart.Price;

                        if (saleCountCell.OwningRow.Cells["Sum"].Value != null)
                            inTotal -= Convert.ToDouble((saleCountCell.OwningRow.Cells["Sum"].Value));

                        double sum = Math.Round((double)currentSparePart.Price * (currentSparePart.Count + currentSparePart.VirtCount), 2, MidpointRounding.AwayFromZero);
                        saleCountCell.OwningRow.Cells["Sum"].Value = sum; //(double)currentSparePart.SellingPrice * count;
                        inTotal += sum; //(double)currentSparePart.SellingPrice * count;
                        inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
                    }//if
                    saleCountCell.Style.ForeColor = Color.Black;
                    saleCountCell.Style.SelectionForeColor = Color.White;
                }//try
                catch
                {
                    //выводим всплывающее окно с сообщением об ошибке.
                    toolTip.Show("Введены некорректные данные", this, GetExtCellBelowLocation(cell), 1000);
                    //Очищаем ввод.
                    cell.Value = fullExtCount;
                    cell.Style.SelectionForeColor = cell.Style.ForeColor = Color.Gray;
                }//catch                           
            }//if            
        }//extDataGridView_CellEndEdit  


        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из extSaleDataGridView. 
        /// </summary>
        /// <param name="cell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetExtCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = extDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = extDataGridView.Location;
            Point gbLoc = extGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation

        /// <summary>
        /// Возвращает число или выбрасывает исключение если value заданной клетки не проходит хоть одну проверку на соответствие допустимому значение количества для данного прихода.
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        private double TestInputValueInExtDGV(DataGridViewCell cell)
        {
            double count = Convert.ToDouble(cell.Value);
            if (count <= 0) throw new Exception();            //ввод нуля или отриц. значения также является ошибкой.

            //проверяем чтобы count было не больше количества на складе.
            double countAvaliability = Convert.ToDouble(System.Text.RegularExpressions.Regex.Replace(fullExtCount, @"[^\d]+", "")); //Достаём число из строки, строка возможна типа "(20)", поэтому исп-ся рег. выр.
            if (count > countAvaliability) throw new Exception();
            //Проверяем является ли введенное число корректным для продажи, т.е. соответствует ли оно минимальному 
            string unitOfMeasure = cell.OwningRow.Cells["extUnit"].Value as string;
            if (count % Models.MeasureUnit.GetMinUnitSale(unitOfMeasure) != 0)
                throw new Exception();
            return count;
        }//TestInputValueInExtDGV
       













///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion








        /////////////////////////////////Вспомогательные методы./////////////////////////
        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из saleDataGridView. 
        /// </summary>
        /// <param name="cell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = saleDataGridView.Location;
            Point gbLoc = saleGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);        
        }//GetCellBelowLocation

        private void FillTheBothDGV(DataGridViewRow row)
        {
            //!!!Сделать ограничение выпадающего списка только по товару имеющимся в наличии.

            FillTheSaleDGV(row);
            //Если количество приходов в наличии больше чем один, то выводим информацию в extDGV.
            //if (PartsDAL.FindCountOfEntrySparePartInAvaliability(currentSparePart.SparePartId) > 1)
            //Очищаем таблицу.
            extDataGridView.Rows.Clear();

            FillTheExtDGV(currentSparePart.SparePartId);                
        }//FillTheBothDGV

        private void FillTheSaleDGV(DataGridViewRow row)
        {                               
            row.Cells["SparePartId"].Value = currentSparePart.SparePartId;
            row.Cells["Title"].Value = currentSparePart.Title;
            row.Cells["Articul"].Value = currentSparePart.Articul;
            row.Cells["Unit"].Value = currentSparePart.MeasureUnit;

            PartsDAL.FindUniqueSparePartsAvaliabilityCount(currentSparePart); //находим общее кол-во   
            row.Cells["Count"].Style.ForeColor = Color.Gray;
            row.Cells["Count"].Style.SelectionForeColor = Color.Gray;
            row.Cells["Count"].Value = currentSparePart.Avaliability;  
          
            //очищаем "количество" текущей единицы для дальнейшего заполнения поль-лем
            currentSparePart.Count = currentSparePart.VirtCount = 0;
        }//FillTheSaleDGV

        private void FillTheExtDGV(int sparePartId)
        {            
            //Находим все записи товара с табл. Avaliability с данным Id. 
            var sparePartsAvaliability = PartsDAL.FindAvaliabilityBySparePartId(sparePartId);
            //Создаем нужное кол-во строк в таблице.
            extDataGridView.Rows.Add(sparePartsAvaliability.Count);

            extCurrentSparePartsList.Clear();
            //Добавляем все приходы данного товара из Avaliability в список и доп. таблицу.
            for(int i=0; i<sparePartsAvaliability.Count; ++i)
            {
                extDataGridView.Rows[i].Cells[extSupplier.Name].Value = sparePartsAvaliability[i].SupplierName;
                extDataGridView.Rows[i].Cells["extTitle"].Value = sparePartsAvaliability[i].Title;
                extDataGridView.Rows[i].Cells["extArticul"].Value = sparePartsAvaliability[i].Articul;
                extDataGridView.Rows[i].Cells["extUnit"].Value = sparePartsAvaliability[i].MeasureUnit;

                extDataGridView.Rows[i].Cells["extCount"].Style.ForeColor = Color.Gray;
                extDataGridView.Rows[i].Cells["extCount"].Style.SelectionForeColor = Color.Gray;
                extDataGridView.Rows[i].Cells["extCount"].Value = sparePartsAvaliability[i].Avaliability;

                extDataGridView.Rows[i].Cells["extStorageAdress"].Value = sparePartsAvaliability[i].StorageAdress;
                extDataGridView.Rows[i].Cells["extPrice"].Value = sparePartsAvaliability[i].Price;
               // extDataGridView.Rows[i].Cells["extMarkup"].Value = sparePartsAvaliability[i].Markup;
                if (sparePartsAvaliability[i].Markup != null)
                {
                    //Находим тип наценки.
                    extDataGridView.Rows[i].Cells["extMarkup"].Value = Models.Markup.GetDescription(Convert.ToSingle(sparePartsAvaliability[i].Markup));
                }//if
                extDataGridView.Rows[i].Cells["extSellingPrice"].Value = sparePartsAvaliability[i].SellingPrice;
                extDataGridView.Rows[i].Cells["extPurchaseId"].Value = sparePartsAvaliability[i].PurchaseId;
                extDataGridView.Rows[i].Cells["extPurchaseDate"].Value = PartsDAL.FindPurchase(sparePartsAvaliability[i].PurchaseId).OperationDate.ToShortDateString();
            }//for            
            
            //Если отпускная цена у всех приходов одинаковая, выводим её в saleDGV.
            if (sparePartsAvaliability.Count(sp => sp.SellingPrice == sparePartsAvaliability[0].SellingPrice) == sparePartsAvaliability.Count)
            {
                lastEditCell.OwningRow.Cells[SellingPrice.Name].Value = sparePartsAvaliability[0].SellingPrice;
                currentSparePart.Price = sparePartsAvaliability[0].SellingPrice; //Запоминаем цену в Price, т.к. SellingPrice не заполнится, потому что Price == null (см. SparePart.SellingPrice.Set()).
            }

            //extDataGridView.Sort(extDataGridView.Columns[extPurchaseId.Name], ListSortDirection.Ascending); //Сл. сортировка полностью перекрывает предыдущую.
            extDataGridView.Sort(extDataGridView.Columns[extPurchaseDate.Name], ListSortDirection.Ascending);
            extDataGridView.ClearSelection();
        }//FillTheExtDGV

        /// <summary>
        /// Возвращает число или выбрасывает исключение если value заданной клетки не проходит хоть одну проверку на соответствие допустимому значение количества данного товара на основном складе.
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <returns></returns>
        private double TestInputValueInSaleDGV(DataGridViewCell cell)
        {
            double count = Convert.ToDouble(cell.Value);
            //ввод нуля или отриц. значения также является ошибкой. 
            if (count <= 0) 
                throw new Exception(); 

            //Проверяем также чтобы count было не больше количества на  основном складе.
            double mainStorageCount = 0;
            if (fullSaleCount.Contains('('))
                mainStorageCount = Convert.ToDouble(fullSaleCount.Substring(0, fullSaleCount.IndexOf('(')));
            else mainStorageCount = Convert.ToDouble(fullSaleCount);

            if (count > mainStorageCount)
                throw new Exception(); 

            //Проверяем является ли введенное число корректным для продажи, т.е. соответствует ли оно минимальному 
            string unitOfMeasure = cell.OwningRow.Cells["Unit"].Value.ToString();
            if (count % Models.MeasureUnit.GetMinUnitSale(unitOfMeasure) != 0)
                throw new Exception();

            return count;            
        }//

        /// <summary>
        /// Метод автовыбора прихода с которого осуществляется продажа (Всегда самые старые приходы).
        /// </summary>
        private void AutoChoisePurchases()
        {
            //Узнаем введенное кол-во в saleDGV.
            double sellCount = Convert.ToDouble(lastEditCell.OwningRow.Cells[Count.Name].Value);
            double extSellCount;
            //Перебираем по строкам из extDGV.
            foreach (DataGridViewRow row in extDataGridView.Rows)
            {
                try { extSellCount = Convert.ToDouble(row.Cells[extCount.Name].Value); }
                catch { continue; } //если вирт. склад -- переходим на сл. строку.

                int purchaseId = Convert.ToInt32(row.Cells[extPurchaseId.Name].Value);
                row.Cells[extCount.Name].Style.ForeColor = Color.Black;
                row.Cells[extCount.Name].Style.SelectionForeColor = Color.White;

                //Находим нужный товар в списке.                
                SparePart extCurrentSparePart = new SparePart();
                extCurrentSparePart.SparePartId = currentSparePart.SparePartId;
                extCurrentSparePart.PurchaseId = purchaseId;
                extCurrentSparePart.Price = Convert.ToDouble(row.Cells[extPrice.Name].Value);

                extSpareParts.Add(extCurrentSparePart);
                //Если продаваемое количество больше чем в данном приходе.
                if (sellCount > extSellCount)
                {
                    extCurrentSparePart.Count = extSellCount;
                    sellCount -= extSellCount;
                }//if
                else
                {
                    row.Cells[extCount.Name].Value = extCurrentSparePart.Count = sellCount;
                    break;
                }//else    
            
                //Если отпусная цена уже выставленна запоминаем её в каждом автовыбранном приходе.
                if (currentSparePart.Price != null)
                    extCurrentSparePart.SellingPrice = currentSparePart.Price;
            }//foreach
        }//AutoChoisePurchases


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Методы вывода инф-ции в Excel.

        private void BeginLoadSaleToExcelFile(object sale)
        {
            if (sale is Sale)
                LoadSaleToExcelFile(sale as Sale, spareParts);
        }//BeginLoadsaleToExcelFile

        ///// <summary>
        ///// Метод вывода приходной информации в Excel-файл.
        ///// </summary>
        ///// <param name="sale">Информация о приходе.</param>
        ///// <param name="availabilityList">Список оприходованных товаров.</param>
        //private void LoadSaleToExcelFile(Sale sale, IList<SparePart> availabilityList)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    Excel.Workbook ExcelWorkBook;
        //    Excel.Worksheet ExcelWorkSheet;
        //    //Книга.
        //    ExcelWorkBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    //Таблица.
        //    ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

        //    int row = 1, column = 1;

        //    //Выводим Id и Дату. 
        //    excelApp.Cells[row, column] = String.Format("Расходная накладная №{0} от {1}г.", sale.SaleId, sale.SaleDate.ToString("dd/MM/yyyy"));
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Bold = true;
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 18;

        //    //Выводим поставщика.
        //    row += 2;
        //    excelApp.Cells[row, column] = String.Format("Продавец:    \t{0}", sellerTextBox.Text);//PartsDAL.FindSellerNameById(sale.SellerId));
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 12;

        //    //Выводим покупателя.
        //    row += 2;
        //    excelApp.Cells[row, column] = String.Format("Покупатель:  \t{0}", customerTextBox.Text);
        //    (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Size = 12;

        //    //Выводим таблицу товаров.
        //    //Выводим заголовок.
        //    row += 2;
        //    excelApp.Cells[row, column] = "Название";
        //    excelApp.Cells[row, column + 1] = "Ед. изм.";
        //    excelApp.Cells[row, column + 2] = "Кол-во";
        //    excelApp.Cells[row, column + 3] = "Цена";
        //    excelApp.Cells[row, column + 4] = "Сумма";

        //    Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "E" + row.ToString());
        //    excelCells.Font.Bold = true;
        //    excelCells.Font.Size = 12;
        //    //Обводим заголовки таблицы рамкой. 
        //    excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;
        //    //Устанавливаем стиль и толщину линии
        //    excelCells.Borders.Weight = Excel.XlBorderWeight.xlMedium;

        //    //Устанавливаем ширину первой Колонки для Title.
        //    double width = 45; //45 -- Взято методом тыка.           
        //    (excelApp.Cells[row, column] as Excel.Range).Columns.ColumnWidth = width;
        //    //Выводим список товаров.
        //    for (int i = 0; i < availabilityList.Count; ++i)
        //    {
        //        ++row;
        //        excelApp.Cells[row, column] = availabilityList[i].Title;
        //        //Если Title не влазиет в одну строку, увеличиваем высоту.
        //        if (availabilityList[i].Title.Length > width)
        //        {
        //            (excelApp.Cells[row, column] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
        //            ExcelWorkSheet.get_Range("B" + row.ToString(), "E" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
        //        }
        //        excelApp.Cells[row, column + 1] = availabilityList[i].Unit;
        //        excelApp.Cells[row, column + 2] = availabilityList[i].Count;
        //        excelApp.Cells[row, column + 3] = availabilityList[i].Price;
        //        excelApp.Cells[row, column + 4] = availabilityList[i].Price * availabilityList[i].Count;
        //        //Выравнивание диапазона строк.
        //        ExcelWorkSheet.get_Range("B" + row.ToString(), "E" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        //    }//for

        //    //Обводим талицу рамкой. 
        //    excelCells = ExcelWorkSheet.get_Range("A" + (row - availabilityList.Count + 1).ToString(), "E" + row.ToString());
        //    excelCells.Borders.ColorIndex = Excel.XlRgbColor.rgbBlack;

        //    //Выводим "Итого".
        //    ++row;
        //    //В зависимости от длины выводимой "Итого" размещаем её или точно под колонкой "сумма" или левее.
        //    int indent = 0; //отступ
        //    if (inTotalNumberLabel.Text.Length <= 9)
        //        indent = 1;

        //    excelApp.Cells[row, column + 2 + indent] = inTotalLabel.Text;
        //    excelApp.Cells[row, column + 3 + indent] = inTotalNumberLabel.Text;
        //    (excelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Underline = true;
        //    (excelApp.Cells[row, column + 2 + indent] as Excel.Range).Font.Size = (excelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Size = 12;
        //    (excelApp.Cells[row, column + 2 + indent] as Excel.Range).Font.Bold = (excelApp.Cells[row, column + 3 + indent] as Excel.Range).Font.Bold = true;

        //    //Выводим имена агентов.
        //    row += 2;
        //    excelApp.Cells[row, column] = String.Format("\t{0} {1} ", sellerAgentLabel.Text, sellerAgentTextBox.Text);
        //    excelApp.Cells[row, column + 1] = String.Format("{0} {1}", customerAgentLabel.Text, customerAgentTextBox.Text);

        //    //Вызываем нашу созданную эксельку.
        //    excelApp.Visible = true;
        //    ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
        //    excelApp.UserControl = true;  //что меняет эта настройка?

        //    this.Close();
        //}//LoadsaleToExcelFile

        /// <summary>
        /// Метод вывода расходной информации в Excel-файл.
        /// </summary>
        /// <param name="sale">Информация о расходе.</param>
        /// <param name="availabilityList">Список проданного товара.</param>
        private void LoadSaleToExcelFile(Sale sale, IList<SparePart> spareParts)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin = 7;
            ExcelWorkSheet.PageSetup.RightMargin = 7;
            ExcelWorkSheet.PageSetup.TopMargin = 10;
            ExcelWorkSheet.PageSetup.BottomMargin = 10;

            int row = 1, column = 1;            
            //Выводим Id и Дату. 
            Excel.Range excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.Font.Bold = true;
            excelCells.Font.Underline = true;
            excelCells.Font.Size = 18;
            excelCells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelCells.Value = String.Format("Расходная накладная №{0} от {1}г.", sale.OperationId, sale.OperationDate.ToString("dd/MM/yyyy"));

            //Выводим поставщика и покупателя.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         sellerLabel.Text + " " + sellerTextBox.Text,
                                                         customerLabel.Text + " " + customerTextBox.Text);
            
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

                ExcelApp.Cells[row, column + 3] = spareParts[i].MeasureUnit;
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
            
            //Выводим имена агентов.
            row += 2;
            ExcelApp.Cells[row, column].Font.Name = "Consolas";
            ExcelApp.Cells[row, column] = String.Format("\t\t{0,-40}{1}",
                                                         sellerAgentLabel.Text + " " + sellerAgentTextBox.Text,
                                                         customerAgentLabel.Text + " " + customerAgentTextBox.Text);

            //Делаем визуальное отделение информации от заметки, с помощью линии.
            row += 2;
            //ширина подобрана методом тыка.
            ExcelApp.Cells[row, column].Value = "                                                                                                                                                                                                                                    ";//longEmptyString.ToString();
            (ExcelWorkSheet.Cells[row, column] as Excel.Range).Font.Underline = true;
            //Выводим заметку
            row++;
            // объединим область ячеек  строки "вместе"
            excelCells = ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString());
            excelCells.Merge(true);
            excelCells.WrapText = true;
            excelCells.Value = sale.Description;//descriptionRichTextBox.Text;
            AutoFitMergedCellRowHeight((ExcelApp.Cells[row, column] as Excel.Range));

            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelWorkBook.PrintPreview(); //открываем окно предварительного просмотра.
            ExcelApp.UserControl = true;

            this.Close();
        }//LoadSaleToExcelFile  

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
        /// Устанавливает ширину столбцов.
        /// </summary>
        /// <param name="availabilityList">Коллекция эл-тов заполняюхий таблицу</param>
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

        private void extDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //Если есть выделенные клетки делаем доступной изменение наценки.
            if (extDataGridView.SelectedCells.Count > 0)
                markupComboBox.Enabled = true;
            else markupComboBox.Enabled = false;
        }//extDataGridView_SelectionChanged

        private void extGroupBox_Click(object sender, EventArgs e)
        {
            extDataGridView.ClearSelection();
        }

        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                markupComboBox_SelectedIndexChanged(sender, e);
        }

        private void currencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyComboBox.Text == "руб")
            {
                excRateNumericUpDown.Value = 1;
                excRateNumericUpDown.Enabled = false;
                excRateLabel.Visible = false;
                excRateNumericUpDown.Visible = false;
                saleDataGridView.Enabled = true;
                currencyComboBox.Enabled = false;
            }//if
            else
            {
                excRateNumericUpDown.Enabled = true;
                excRateLabel.Visible = true;
                excRateNumericUpDown.Visible = true;
            }
            //markupCheckBox.Enabled = true;
        }//currencyComboBox_SelectedIndexChanged
         
        private void markupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если нет выделенных строк, то выходим.
            if (extDataGridView.SelectedCells.Count == 0) return;

            //выделяем строки всех выделенных клеток.
            foreach (DataGridViewCell cell in extDataGridView.SelectedCells) cell.OwningRow.Selected = true;
            //узнаем процент заданной наценки.
            try
            {
                float markupValue = (markupComboBox.SelectedValue != null) ? Convert.ToSingle(markupComboBox.SelectedValue) : Convert.ToSingle(markupComboBox.Text.Trim());
                string markupType = Models.Markup.GetDescription(markupValue);

                foreach (DataGridViewRow row in extDataGridView.SelectedRows)
                {
                    row.Cells[extMarkup.Name].Value = markupType;

                    double price = Convert.ToDouble(row.Cells[extPrice.Name].Value);
                    double sellPrice = Math.Round(price + (price * markupValue / 100), 2, MidpointRounding.AwayFromZero);
                    row.Cells[extSellingPrice.Name].Value = sellPrice;
                }//foreach
            }//try
            catch
            {
                toolTip.Show("Введено некорректное значение.", this, markupComboBox.Location, 2000);
            }//catch
        }//markupComboBox_SelectedIndexChanged

        private void saleDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                {
                    if (e.RowIndex == -1)
                        saleDataGridView.SelectAll();
                    else
                    {
                        lastEditCell = saleDataGridView.Rows[e.RowIndex].HeaderCell;
                        //Если строка пустая не делаем ничего.
                        if (lastEditCell.OwningRow.Cells["SparePartId"].Value == null) return;

                        lastEditCell.OwningRow.Selected = true;
                    }
                    Point location = saleDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                    location.X += e.Location.X;
                    location.Y += e.Location.Y;
                    saleContextMenuStrip.Show(saleDataGridView, location, ToolStripDropDownDirection.BelowRight);
                }//if                
            }//if (Button.Right)
        }//saleDataGridView_CellMouseClick        

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
                customerTextBox_Leave(null, null);
                //sellerTextBox_Leave(null, null);

                if (sellerBackPanel.BackColor != Color.Red && customerBackPanel.BackColor != Color.Red
                    && spareParts.Count != 0)
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
                    
                    Sale sale = new Sale();
                    sale.Employee = Form1.CurEmployee;
/*!!!*/             sale.Contragent = PartsDAL.FindCustomers().Where(c => c.ContragentName == customerTextBox.Text).First();
                    sale.ContragentEmployee = (String.IsNullOrWhiteSpace(customerAgentTextBox.Text) == false) ? customerAgentTextBox.Text.Trim() : null;
                    sale.OperationDate = saleDateTimePicker.Value;
                    sale.Description = (String.IsNullOrWhiteSpace(descriptionRichTextBox.Text) == false) ? descriptionRichTextBox.Text.Trim() : null;
                    
                    try
                    {
                        sale.OperationId = PartsDAL.AddSale(spareParts, extSpareParts, sale);
                    }//try
                    catch(Exception)
                    {
                        MessageBox.Show("Операция завершена неправильно! Попробуйте ещё раз.");
                        return;
                    }//catch 

                    //LoadsaleToExcelFile(sale, availabilityList);
/*!!!*/             new System.Threading.Thread(BeginLoadSaleToExcelFile).Start(sale); //Сделать по нормальному вызов с потоком.

                    this.Visible = false;
                    //this.Close();
                }//if
            }//if
        }
        

        
        
        


                                                                             

        

       





    }//class SaleForm
}//namespace




/*Задачи*/
//1)SaleDateTime выставить в Now.
//2)Нумерация строк общая для двух DGV


/*Будущие задачи*/
//1)Добавить возм-ть выбора валюты.
//2)Добавить учет скидок.

#region Попытка с подставлением TextBox-а.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        private void SaleForm_Load(object sender, EventArgs e)
//        {
//            //saleDataGridView.ClearSelection();
//            currencyComboBox.SelectedItem = "руб";

///*!!!*/     saleDateTimePicker.MaxDate = DateTime.Now.Date; //можно обойти с помощью изменения даты в Виндовс, лучше проверять по интернету.
//            saleDateTimePicker.Value = DateTime.Now.Date;
//        }

//        #region Методы связанные с обработкой ввода в saleDataGridView.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//        private void saleDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
//        {
//            //MessageBox.Show("Cell Enter");
//            int row = e.RowIndex;
//            int column = e.ColumnIndex;
//            //if (row == 0) return;

//            if (saleDataGridView.Columns[column].Name == "Title" || saleDataGridView.Columns[column].Name == "Articul")
//            {
//                SubstituteCellOnTextBox(column, row);
//            }//if

//        }//saleDataGridView_CellEnter

//        private void saleDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
//        {
//            e.Cancel = true;
//            int row = e.RowIndex;
//            int column = e.ColumnIndex;
//            //if (row == 0) return;

//            if (saleDataGridView.Columns[column].Name == "Title" || saleDataGridView.Columns[column].Name == "Articul")
//            {
//                SubstituteCellOnTextBox(column, row);
//            }//if
//        }//saleDataGridView_CellBeginEdit


//        private void saleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
//        {
//            MessageBox.Show("Cell End Edit!!");
//        }//saleDataGridView_CellEndEdit

//        private void substitTextBox_TextChanged(object sender, EventArgs e)
//        {
//            if (String.IsNullOrWhiteSpace(substitTextBox.Text) == false)
//            {
//                if (lastEditCell.OwningColumn.Name == "Title")
//                    searchSparePartsList = PartsDAL.SearchSparePartsByTitle(substitTextBox.Text, 10, sparePartsId);
//                else if (lastEditCell.OwningColumn.Name == "Articul")
//                    searchSparePartsList = PartsDAL.SearchSparePartsByArticul(substitTextBox.Text, 10, sparePartsId);
//                //Если совпадения найдены, вывести вып. список.
//                if (searchSparePartsList.Count > 0)
//                {
//                    autoCompleteListBox.Items.Clear();
//                    string str = null;
//                    foreach (var avail in searchSparePartsList)
//                    {
//                        if (lastEditCell.OwningColumn.Name == "Title")
//                            str = avail.Title + "   " + avail.Articul;
//                        else if (lastEditCell.OwningColumn.Name == "Articul")
//                            str = avail.Articul + "   " + avail.Title;

//                        autoCompleteListBox.Items.Add(str);
//                    }//foreach                                                                        

//                    autoCompleteListBox.Size = autoCompleteListBox.PreferredSize;
//                    autoCompleteListBox.Visible = true;
//                }//if
//                else autoCompleteListBox.Visible = false; //Если ничего не найдено, убрать вып. список.
//            }//if
//            else autoCompleteListBox.Visible = false; //Если ничего не введено, убрать вып. список.
//        }//substitTextBox_TextChanged

//        //private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
//        //{
//        //    isCellEditError = true;
//        //}//autoCompleteListBox_MouseHover

//        //private void autoCompleteListBox_MouseDown(object sender, MouseEventArgs e)
//        //{
//        //    if (e.Clicks == 1)
//        //    {
//        //        if (String.IsNullOrEmpty(customerText))
//        //            customerText = textBoxCell.Text;
//        //        saleDataGridView_SelectionChanged(null, null);
//        //        isCellEditError = true;
//        //    }
//        //    else
//        //    {
//        //        //    textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
//        //        //    textChangedEvent = true; 
//        //        isCellEditError = false;
//        //        //dataGridViewTextBoxCell_PreviewKeyDown(textBoxCell, new PreviewKeyDownEventArgs(Keys.Enter));
//        //        //saleDataGridView.Rows[lastEditCell.RowIndex + 1].Cells["Title"].Selected = true;
//        //        saleDataGridView_CellEndEdit(null, new DataGridViewCellEventArgs(lastEditCell.ColumnIndex, lastEditCell.RowIndex));
//        //    }
//        //}//autoCompleteListBox_MouseDown

//        //private void autoCompleteListBox_SelectedIndexChanged(object sender, EventArgs e)
//        //{
//        //    textChangedEvent = false;
//        //    if (autoCompleteListBox.SelectedIndex != -1)
//        //        textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
//        //}//autoCompleteListBox_SelectedIndexChanged

//        private void substitTextBox_Leave(object sender, EventArgs e)
//        {
//            //Если TextBox не пустой, то переносим его содержимое в нужную клетку и очищаем TextBox.
//            if (String.IsNullOrWhiteSpace(substitTextBox.Text) == false)
//            {
//                lastEditCell.Value = substitTextBox.Text;
//                substitTextBox.Clear();
//            }
//            //При переходе на любой другой контрол необх-мо сделать TextBox невидимым, но это вызывет бесконечный цикл вызовов события Leave, поэтому его необх-мо отключить.
//            substitTextBox.Leave -= substitTextBox_Leave;
//            substitTextBox.Visible = false;
//            substitTextBox.Leave += substitTextBox_Leave;
//        }//substitTextBox_Leave







//        /// <summary>
//        /// Метод подставляющий TextBox под заданную клетку.
//        /// </summary>
//        /// <param name="column">Индекс столбца</param>
//        /// <param name="row">Индекс строки</param>
//        private void SubstituteCellOnTextBox(int column, int row)
//        {
//            Rectangle rect = saleDataGridView.GetCellDisplayRectangle(column, row, false);
//            lastEditCell = saleDataGridView.Rows[row].Cells[column];
//            substitTextBox.Location = GetCellLocation(lastEditCell);
//            substitTextBox.Size = rect.Size;
//            substitTextBox.Width -= 1;
//            substitTextBox.Visible = true;
//            substitTextBox.Focus();        
//        }

//        /// <summary>
//        /// Возвращает абсолютный location области сразу под позицией клетки из saleDataGridView. 
//        /// </summary>
//        /// <param name="cell">Клетка под чьей location необходимо вернуть</param>
//        /// <returns></returns>
//        private Point GetCellBelowLocation(DataGridViewCell cell)
//        {
//            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
//            Point dgvLoc = saleDataGridView.Location;
//            Point gbLoc = saleGroupBox.Location;
//            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
//        }//GetCellBelowLocation

//        /// <summary>
//        /// Возвращает абсолютный location области на позиции клетки из saleDataGridView. 
//        /// </summary>
//        /// <param name="cell">Клетка чей location необходимо вернуть</param>
//        /// <returns></returns>
//        private Point GetCellLocation(DataGridViewCell cell)
//        {
//            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
//            Point dgvLoc = saleDataGridView.Location;
//            Point gbLoc = saleGroupBox.Location;
//            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y);
//        }











//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        #endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#endregion