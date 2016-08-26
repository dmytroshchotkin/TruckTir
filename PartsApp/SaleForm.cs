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
        /// <summary>
        /// Список для хранения общих сведений по операции.
        /// </summary>
        List<OperationDetails> operDetList = new List<OperationDetails>();
        /// <summary>
        /// Список для хранения деталей операции по приходам. 
        /// </summary>
        List<Availability> _availList = new List<Availability>();

        List<SparePart> sparePartsList = new List<SparePart>(); /*ERROR!!! убрать поле*/

        IList<SparePart> searchSparePartsList = new List<SparePart>();

        bool isCellEditError = false;
        DataGridViewCell lastEditCell;

        TextBox textBoxCell;
        bool textChangedEvent = false;
        bool previewKeyDownEvent = false;
        string userText;

        double inTotal;
        /// <summary>
        /// Строка для запоминания количества товара в наличии для конкретного приходе.
        /// </summary>
        string fullExtCount;
        string fullSaleCount;

        /*****/
        public SaleForm()
        {
            InitializeComponent();
        }//

        /*****/
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

        /*****/
        #region Валидация вводимых данных.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*****/
        private void customerTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                customerTextBox_Leave(sender, null);
            }//if
        }//SellerTextBox_PreviewKeyDown

        /****/
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
        
        /****/
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

        /******/
        private void saleDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Эта строка нужна потому что новые столбцы SellingPrice и Count почему то становятся открытыми для записи.
/*!!!*/      saleDataGridView.Rows[e.RowIndex].Cells[SellingPrice.Name].ReadOnly = saleDataGridView.Rows[e.RowIndex].Cells["Count"].ReadOnly = true;
        }//saleDataGridView_RowsAdded
        /******/
        #region Обработка событий добавления товара в списки.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*События идут в порядке их возможного вызова.*/
        /******/
        /// <summary>
        /// Событие для установки listBox в нужную позицию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> /******//******/
        private void saleDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];
            lastEditCell = cell;
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                //Запрещаем ввод в новую строку, если в предыдущей не были введены кол-во и цена продажи.
                if (cell.OwningRow.Index != 0)
                    if (saleDataGridView.Rows[cell.OwningRow.Index - 1].Cells[SellingPrice.Index].Value == null ||
                        saleDataGridView.Rows[cell.OwningRow.Index - 1].Cells[Count.Index].Style.ForeColor == Color.Gray)
                        e.Cancel = true;

                autoCompleteListBox.Location = GetCellBelowLocation(cell);

                extDataGridView.Columns[extCount.Name].ReadOnly = false; //Разрешаем ввод кол-ва в доп. таблице.
            }//if
            //Запрещаем ввод цены, пока не введено кол-во.
            if (cell.OwningColumn == SellingPrice && cell.OwningRow.Cells[Count.Index].Style.ForeColor == Color.Gray)
                e.Cancel = true;

            //Обрабатываем ввод Количества.
            if (cell.OwningColumn == Count)
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
        /******/
        /// <summary>
        /// Событие для добавления обработчиков на ввод текста в клетку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>/******/
        private void saleDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {            
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)saleDataGridView.CurrentCell;

            //
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                textBoxCell = e.Control as TextBox;
                if (previewKeyDownEvent == false)
                {
                    previewKeyDownEvent = true;
                    textBoxCell.PreviewKeyDown += new PreviewKeyDownEventHandler(dataGridViewTextBoxCell_PreviewKeyDown);
                    textBoxCell.TextChanged += new EventHandler(dataGridViewTextBoxCell_TextChanged);
                }//if
            }//if
        }//saleDataGridView_EditingControlShowing
        /******/
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
        /******/
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
                List<int> sparePartsIdList = sparePartsList.Select(sp => sp.SparePartId).ToList();
                if (lastEditCell.OwningColumn == Title)
                    searchSparePartsList = PartsDAL.SearchSparePartsAvaliablityByTitle(textBox.Text, 10, sparePartsIdList);
                else if (lastEditCell.OwningColumn == Articul)
                    searchSparePartsList = PartsDAL.SearchSparePartsAvaliablityByArticul(textBox.Text, 10, sparePartsIdList);
                //Если совпадения найдены, вывести вып. список.
                if (searchSparePartsList.Count > 0)
                {
                    autoCompleteListBox.Items.Clear();
                    string str = null;
                    foreach (var sparePart in searchSparePartsList)
                    {
                        if (lastEditCell.OwningColumn == Title)
                            str = sparePart.Title + "     " + sparePart.Articul;
                        else if (lastEditCell.OwningColumn == Articul)
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
        /******/
        private void autoCompleteListBox_MouseHover(object sender, EventArgs e)
        {
            isCellEditError = true;            
        }
        /******/
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
        /******/
        private void autoCompleteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {           
            textChangedEvent = false;
            if (autoCompleteListBox.SelectedIndex != -1)
                textBoxCell.Text = autoCompleteListBox.SelectedItem.ToString();
        }//autoCompleteListBox_SelectedIndexChanged
        /******/
        private void saleDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {            
            if (isCellEditError) 
                return;

            autoCompleteListBox.Visible = false;
            //autoCompleteListBox.Items.Clear();
            DataGridViewRow row = saleDataGridView.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            //Если редактировался Артикул или Название
            #region Articul Or Title
            if (cell.OwningColumn == Title || cell.OwningColumn == Articul)
            {
                if (cell.Value == null) 
                    return;

                //убираем события с заполненной клетки.
                if (textBoxCell != null)
                {
                    textChangedEvent = previewKeyDownEvent = false;
                     
                    textBoxCell.TextChanged    -= dataGridViewTextBoxCell_TextChanged;
                    textBoxCell.PreviewKeyDown -= dataGridViewTextBoxCell_PreviewKeyDown;
                }//if

                //Если есть такой товар в базе.
                if (searchSparePartsList.Count > 0)
                {
                    string[] titleAndArticul = (cell.Value as string).Split(new string[] { "     " }, StringSplitOptions.RemoveEmptyEntries);
                    string title, articul;
                    //если выбор сделан из выпадающего списка.
                    if (titleAndArticul.Length == 2)
                    {

                        title   = (cell.OwningColumn.Index == Title.Index) ? titleAndArticul[0] : titleAndArticul[1];
                        articul = (cell.OwningColumn.Index == Title.Index) ? titleAndArticul[1] : titleAndArticul[0];        
                        
                        //находим из списка нужную запчасть.
                        SparePart sparePart = searchSparePartsList.FirstOrDefault(sp => sp.Title   == title.Trim() 
                                                                                     && sp.Articul == articul.Trim());
                        //Если такой товар найден в вып. списке.
                        if (sparePart != null)
                        {
                            FillTheBothDGV(row, sparePart);

                            sparePartsList.Add(sparePart); //Добавляем в список.

                            row.Cells[SellingPrice.Index].ReadOnly = row.Cells[Count.Index].ReadOnly   = false;
                            row.Cells[Title.Index].ReadOnly        = row.Cells[Articul.Index].ReadOnly = true;

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
                                //находим из списка нужную запчасть. /*ERROR Кажется ошибка идентификации введенного товара*/
                                SparePart sparePart = searchSparePartsList[0];
                                /*ERROR!!! Скомпановать код!*/
                                //if (sparePartsList.Count > 0) //если введенный товар именно тот что в списке.
                                //{
                                FillTheBothDGV(row, sparePart);
                                sparePartsList.Add(sparePart); //добавляем в список.

                                row.Cells[SellingPrice.Index].ReadOnly = row.Cells[Count.Index].ReadOnly = false;
                                row.Cells[Title.Index].ReadOnly = row.Cells[Articul.Index].ReadOnly = true;

                                userText = null;
                                //}//if
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
            if (cell.OwningColumn == SellingPrice)
            {
                if (cell.Value != null) //Если строка не пустая, проверить корректность ввода.
                {
                    try
                    {
                        int sparePartId = Convert.ToInt32(row.Cells[SparePartId.Index].Value);
                        SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);

                        float price = Convert.ToSingle(cell.Value);
                        if (price == 0) throw new Exception();  //ввод нуля также является ошибкой.

                        //Если цена продажи хотя бы где-то ниже закупочной требуем подтверждения действий.                         
                        if (sparePart.AvailabilityList.Any(av => av.OperationDetails.Price >= price))
                            if (MessageBox.Show("Цена продажи ниже или равна закупочной!. Всё верно?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                                throw new Exception();                                  

                        //Заполняем цену во всех выбранных приходах.
                        foreach (Availability avail in sparePart.AvailabilityList)
                            avail.SellingPrice = price;

                        ////Если цена вводится в той же строке.
                        //int sparePartId = Convert.ToInt32(countCell.OwningRow.Cells["SparePartId"].Value);
                        //if (sparePartId != currentSparePart.SparePartId)
                        //{
                        //    foreach (var sparePart in sparePartsList)
                        //        if (sparePart.SparePartId == sparePartId)
                        //            currentSparePart = sparePart;                            
                        //}//if

                        //Округляем Price до 2-х десятичных знаков.
                        price = (float)Math.Round(price, 2, MidpointRounding.AwayFromZero);
                        //currentSparePart.Price = sellPrice; //Запоминаем цену в Price, т.к. SellingPrice не заполнится, потому что Price == null (см. SparePart.SellingPrice.Set()).
                        //foreach (var sp in extCurrentSparePartsList)
                        //    sp.SellingPrice = sellPrice;

                        cell.Value = String.Format("{0:N2}", price);

                        if (row.Cells[Count.Index].Value != null)
                        {
                            //Узнаем была ли уже до этого введена цена, для изменения строки "итого".
                            if (cell.OwningRow.Cells[Sum.Index].Value != null)
                                inTotal -= Convert.ToDouble((cell.OwningRow.Cells[Sum.Index].Value));

                            float totalCount = sparePart.AvailabilityList.Sum(av => av.OperationDetails.Count);
                            double sum = Math.Round(price * totalCount, 2, MidpointRounding.AwayFromZero);
                            cell.OwningRow.Cells["Sum"].Value = String.Format("{0:N2}", sum);//sum; //sellPrice * currentSparePart.Count;
                            inTotal += sum; //sellPrice * currentSparePart.Count;
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
            if (cell.OwningColumn == Count)
            {
                try
                {
                    float count = TestInputValueInSaleDGV(cell);

                    //Если введено корректное значение, блокируем возм-ть ввода в extDGV.
                    extDataGridView.Columns[extCount.Name].ReadOnly = true;

                    //Находим нужный товар.
                    int sparePartId = Convert.ToInt32(row.Cells[SparePartId.Index].Value);
                    SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);

                    //if (sparePartId != currentSparePart.SparePartId)
                    //{
                    //    foreach (var sparePart in sparePartsList)
                    //        if (sparePart.SparePartId == sparePartId)
                    //            currentSparePart = sparePart;
                    //}//if
                    //currentSparePart.AvailabilityList[0].OperationDetails.Count = sellCount;
                    //Запрещаем изменение кол-ва после первого корректного ввода.
                    row.Cells[Count.Index].ReadOnly = true;

                    if (row.Cells[Price.Index].Value != null)
                    {
                        if (cell.OwningRow.Cells[Sum.Index].Value != null)
                            inTotal -= Convert.ToDouble((cell.OwningRow.Cells[Sum.Index].Value));

                        float price = Convert.ToSingle(row.Cells[Price.Index].Value);
                        double sum = Math.Round((double)(price * count), 2, MidpointRounding.AwayFromZero);
                        cell.OwningRow.Cells[Sum.Index].Value = sum; //(double)currentSparePart.Price * sellCount;
                        inTotal += sum; //(double)currentSparePart.Price * sellCount;
                        inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);

                        //countCell.OwningRow.Cells[SellingPrice.Name].ReadOnly = true;//countCell.OwningRow.Cells["Count"].ReadOnly = true;
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
        /******/
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
        /******/
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saleDataGridView.AreAllCellsSelected(false) == true)
            {
                if (DialogResult.Yes == MessageBox.Show("Вы хотите полностью очистить список?", "", MessageBoxButtons.YesNo))
                {
                    saleDataGridView.Rows.Clear();
                    extDataGridView.Rows.Clear();
                    sparePartsList.Clear();
                    //extSpareParts.Clear();
                    searchSparePartsList.Clear(); //надо ли?

                    //очищаем "Итого".
                    inTotal = 0;
                }//if
            }//if
            else
            {
                int sparePartId = Convert.ToInt32(lastEditCell.OwningRow.Cells["SparePartId"].Value);
                //Очищаем saleDGV.
                for (int i = 0; i < sparePartsList.Count; ++i)
                    if (sparePartsList[i].SparePartId == sparePartId)
                    {
                        sparePartsList.RemoveAt(i);     /*ERROR!!!! Тут Кроется большая потенциальная ошибка, потому что при удалении уменьшается счетчик в оп-ре for. Работает корректно пока удаляется только один элемент из списка.*/
                        break;
                    }//if

                //исправляем "Итого".
                if (lastEditCell.OwningRow.Cells[Sum.Name].Value != null)
                    inTotal -= Convert.ToDouble((lastEditCell.OwningRow.Cells[Sum.Name].Value));

                //Удаляем строку.
                saleDataGridView.Rows.Remove(lastEditCell.OwningRow);
                

                ////Очищаем extDGV, если удаляется текущая редактируемая строка.
                //if (currentSparePart.SparePartId == sparePartId) 
                //    extDataGridView.Rows.Clear();
                ////Запоминаем объекты которые нужно удалить из списка.
                //var removesSp = new List<SparePart>();
                //for (int i = 0; i < extSpareParts.Count; ++i)
                //    if (extSpareParts[i].SparePartId == sparePartId)
                //        removesSp.Add(extSpareParts[i]);

                ////Удаляем эти объекты
                //foreach (var sp in removesSp)
                //    extSpareParts.Remove(sp);
            }//else

            //Выводим "Итого".
            inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
        }//removeToolStripMenuItem_Click




        #region Обработка событий работы с дополнительным списком товаров.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /******/
        private void extDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == extCount.Index)
            {
                DataGridViewCell cell = extDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                //if (String.IsNullOrEmpty(fullExtCount))
                fullExtCount = cell.Value as string; //если первый ввод, запоминаем полное кол-во.
                cell.Value = null; //очищаем для ввода пользователем.
                cell.Style.SelectionForeColor = Color.White;
                cell.Style.ForeColor = Color.Black;
            }
        }//extDataGridView_CellBeginEdit
        /******/
        private void extDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Если редактировалась колонка "Кол-во". хотя по идее все остальные readOnly.
            if (extDataGridView.Columns[e.ColumnIndex] == extCount)
            {
                DataGridViewRow row = extDataGridView.Rows[e.RowIndex];
                DataGridViewCell cell = row.Cells[e.ColumnIndex];
                try
                {
                    double count = TestInputValueInExtDGV(cell);
                    cell.ReadOnly = true;

                    
                    //int purchaseId = Convert.ToInt32(row.Cells[extPurchaseId.Index].Value);                    
                    //SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);
                    //Availability avail = sparePart.AvailabilityList.First(av => av.OperationDetails.Operation.OperationId == purchaseId);
                    
                    //Если товар с вирт. склада.
                    if (row.Cells[extStorageAdress.Index].Value != null)
                        cell.Value = String.Format("({0})", count);
                    else
                        cell.Value = count;

                    //Обновляем ячеку 'Кол-во' в таблице продаж.
                    SaleDGVCountColumnInvalidate(row);
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

        /******/
        /// <summary>
        /// Обновляет значение ячейки 'Кол-во' в таблице продаж, после изменений в доп. таблице.
        /// </summary>
        /// <param name="extCount">Измененная строка в доп. таблице.</param>/******/
        private void SaleDGVCountColumnInvalidate(DataGridViewRow extRow)
        {
            //Находим нужную клетку в табл. продаж. /*ERROR находить ячейку нормально!*/
            DataGridViewRow row = saleDataGridView.Rows[saleDataGridView.Rows.Count - 2];
            var saleCountCell = row.Cells[Count.Index]; //Находим клетку "количество" предпоследней строки в таблице, потому, что из доп. таблицы изименятся может только она, т.к. все другие будут заблокированны. 

            int sparePartId = Convert.ToInt32(saleCountCell.OwningRow.Cells[SparePartId.Index].Value);

            //Обновляем "кол-во" в таблице продаж.
            saleCountCell.ReadOnly = true;
            List<Availability> availList = _availList.Where(av => av.OperationDetails.SparePart.SparePartId == sparePartId).ToList();
            saleCountCell.Value = Availability.GetTotalCount(availList);

            //Обновляем содержимое столбца "Сумма" в saleDGV, если цена уже указана.
            if (row.Cells[Price.Index] != null)
            {
                //Если отпусная цена уже выставленна запоминаем её в выбранном приходе.
                extRow.Cells[extSellingPrice.Index].Value = row.Cells[SellingPrice.Index].Value;                

                if (saleCountCell.OwningRow.Cells["Sum"].Value != null)
                    inTotal -= Convert.ToDouble((saleCountCell.OwningRow.Cells["Sum"].Value));

                double price = Convert.ToDouble(row.Cells[SellingPrice.Index].Value);
                float count = availList.Sum(av => av.OperationDetails.Count);
                double sum = Math.Round(price * count, 2, MidpointRounding.AwayFromZero);
                row.Cells[Sum.Index].Value = sum; 
                inTotal += sum; 
                inTotalNumberLabel.Text = String.Format("{0}({1})", inTotal, currencyComboBox.Text);
            }//if
            saleCountCell.Style.ForeColor = Color.Black;
            saleCountCell.Style.SelectionForeColor = Color.White;
        
        }//SaleDGVCountColumnUpdate
        /******/
        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из extSaleDataGridView. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>/******/
        private Point GetExtCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = extDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc = extDataGridView.Location;
            Point gbLoc = extGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);
        }//GetCellBelowLocation
        /******/
        /// <summary>
        /// Возвращает число или выбрасывает исключение если value заданной клетки не проходит хоть одну проверку на соответствие допустимому значение количества для данного прихода.
        /// </summary>
        /// <param name="value">Проверяемое значение</param>/******/
        private double TestInputValueInExtDGV(DataGridViewCell cell)
        {
            double count = Convert.ToDouble(cell.Value);
            if (count <= 0) throw new Exception();            //ввод нуля или отриц. значения также является ошибкой.

            //проверяем чтобы sellCount было не больше количества на складе.
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
        /******/
        /// <summary>
        /// Возвращает абсолютный location области сразу под позицией клетки из saleDataGridView. 
        /// </summary>
        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
        /// <returns></returns>
        private Point GetCellBelowLocation(DataGridViewCell cell)
        {
            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true).Location;
            Point dgvLoc  = saleDataGridView.Location;
            Point gbLoc   = saleGroupBox.Location;
            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + cell.Size.Height);        
        }//GetCellBelowLocation
        /******/
        private void FillTheBothDGV(DataGridViewRow row, SparePart sparePart)
        {
            //!!!Сделать ограничение выпадающего списка только по товару имеющимся в наличии.

            FillTheSaleDGV(row, sparePart);
            //Если количество приходов в наличии больше чем один, то выводим информацию в extDGV.
            //if (PartsDAL.FindCountOfEntrySparePartInAvaliability(currentSparePart.SparePartId) > 1)
            //Очищаем таблицу.
            extDataGridView.Rows.Clear();

            FillTheExtDGV(sparePart.AvailabilityList);                
        }//FillTheBothDGV
        /******/
        private void FillTheSaleDGV(DataGridViewRow row, SparePart sparePart)
        {
            row.Cells[SparePartId.Index].Value  = sparePart.SparePartId;
            row.Cells[Title.Index].Value        = sparePart.Title;
            row.Cells[Articul.Index].Value      = sparePart.Articul;
            row.Cells[Unit.Index].Value         = sparePart.MeasureUnit;
               
            row.Cells[Count.Index].Style.ForeColor = Color.Gray;
            row.Cells[Count.Index].Style.SelectionForeColor = Color.Gray;
            row.Cells[Count.Index].Value = Availability.GetTotalCount(sparePart.AvailabilityList); ;  
          
        }//FillTheSaleDGV
        /******/
        /// <summary>
        /// Заполняет данными таблицу доп. инф-ции.
        /// </summary>
        /// <param name="sparePart">Список приходов данного товара в наличии.</param>
        private void FillTheExtDGV(List<Availability> availList)
        {            
            foreach (Availability avail in availList)
            {
                int rowIndx = extDataGridView.Rows.Add();
                DataGridViewRow row = extDataGridView.Rows[rowIndx];

                row.Cells[extSupplier.Index].Value  = avail.OperationDetails.Operation.Contragent.ContragentName;
                row.Cells[extTitle.Index].Value     = avail.OperationDetails.SparePart.Title;
                row.Cells[extArticul.Index].Value   = avail.OperationDetails.SparePart.Articul;
                row.Cells[extUnit.Index].Value      = avail.OperationDetails.SparePart.MeasureUnit;

                //Делаем для ячейки серый цвет.
                row.Cells[extCount.Index].Style.ForeColor = row.Cells[extCount.Index].Style.SelectionForeColor = Color.Gray;
                row.Cells[extCount.Index].Value = avail.OperationDetails.Count;

                row.Cells[extStorageAdress.Index].Value = avail.StorageAddress;
                row.Cells[extPrice.Index].Value = avail.OperationDetails.Price;
               
                row.Cells[extMarkup.Index].Value        = Models.Markup.GetDescription(avail.Markup);
                row.Cells[extSellingPrice.Index].Value  = avail.SellingPrice;
                row.Cells[extPurchaseId.Index].Value    = avail.OperationDetails.Operation.OperationId;
                row.Cells[extPurchaseDate.Index].Value  = avail.OperationDetails.Operation.OperationDate;
            }//foreach            
            
            //Если отпускная цена у всех приходов одинаковая, выводим её в saleDGV.
            if (!availList.Any(av => av.SellingPrice != availList[0].SellingPrice))
                lastEditCell.OwningRow.Cells[SellingPrice.Name].Value = availList[0].SellingPrice;

            //Сортируем таблицу по дате прихода.
            extDataGridView.Sort(extPurchaseDate, ListSortDirection.Ascending);
            extDataGridView.ClearSelection();
        }//FillTheExtDGV
        /******/
        /// <summary>
        /// Возвращает число или выбрасывает исключение если value заданной клетки не проходит хоть одну проверку на соответствие допустимому значение количества данного товара на основном складе.
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <returns></returns>
        private float TestInputValueInSaleDGV(DataGridViewCell cell)
        {
            float count = Convert.ToSingle(cell.Value);
            //ввод нуля или отриц. значения также является ошибкой. 
            if (count <= 0) 
                throw new Exception(); 

            //Проверяем также чтобы sellCount было не больше количества на  основном складе.
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
        /******/
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
                //SparePart extCurrentSparePart = new SparePart();
                //extCurrentSparePart.SparePartId = currentSparePart.SparePartId;
                //extCurrentSparePart.PurchaseId = purchaseId;
                //extCurrentSparePart.Price = Convert.ToDouble(row.Cells[extPrice.Name].Value);

                //extSpareParts.Add(extCurrentSparePart);
                ////Если продаваемое количество больше чем в данном приходе.
                //if (sellCount > extAvailCount)
                //{
                //    extCurrentSparePart.Count = extAvailCount;
                //    sellCount -= extAvailCount;
                //}//if
                //else
                //{
                //    row.Cells[extCount.Name].Value = extCurrentSparePart.Count = sellCount;
                //    break;
                //}//else    
            
                ////Если отпусная цена уже выставленна запоминаем её в каждом автовыбранном приходе.
                //if (currentSparePart.Price != null)
                //    extCurrentSparePart.SellingPrice = currentSparePart.Price;
            }//foreach
        }//AutoChoisePurchases


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /******/
        #region Методы вывода инф-ции в Excel.

        private void BeginLoadSaleToExcelFile(object sale)
        {
            if (sale is Sale)
                LoadSaleToExcelFile(sale as Sale);
        }//BeginLoadsaleToExcelFile

        /// <summary>
        /// Метод вывода расходной информации в Excel-файл.
        /// </summary>
        /// <param name="sale">Информация о расходе.</param>
        /// <param name="availabilityList">Список проданного товара.</param>
        private void LoadSaleToExcelFile(Sale sale)
        {
            IList<OperationDetails> operDetList = sale.OperationDetailsList;

            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value); //Книга.
            Excel.Worksheet ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1); //Таблица.
                        
            //Настраиваем горизонтальные и вертикальные границы области печати.
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelWorkSheet.PageSetup.RightMargin  = 7;
            ExcelWorkSheet.PageSetup.TopMargin  = ExcelWorkSheet.PageSetup.BottomMargin = 10;

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

            SetColumnsWidth(operDetList, (ExcelApp.Cells[row, column + 2] as Excel.Range), (ExcelApp.Cells[row, column + 1] as Excel.Range), (ExcelApp.Cells[row, column] as Excel.Range));

            //Выводим список товаров.
            foreach (OperationDetails operDet in operDetList)
            {
                ++row;
                ExcelApp.Cells[row, column + 2] = operDet.SparePart.Title;
                ExcelApp.Cells[row, column + 1] = operDet.SparePart.Articul;
                //Выравнивание диапазона строк.
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.VerticalAlignment = Excel.Constants.xlTop;
                ExcelWorkSheet.get_Range("A" + row.ToString(), "G" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //Если Title или Articul не влазиет в одну строку, увеличиваем высоту.
                if (operDet.SparePart.Articul.Length > articulColWidth || operDet.SparePart.Title.Length > titleColWidth)
                {
                    ExcelWorkSheet.get_Range("B" + row.ToString(), "C" + row.ToString()).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignDistributed;
                    //Проверки для выравнивания по левой стороне, если содержимое только одного из столбцов не влазиет в одну строку.
                    if (operDet.SparePart.Articul.Length > articulColWidth && operDet.SparePart.Title.Length <= titleColWidth)
                        (ExcelApp.Cells[row, column + 2] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    if (operDet.SparePart.Articul.Length <= articulColWidth && operDet.SparePart.Title.Length > titleColWidth)
                        (ExcelApp.Cells[row, column + 1] as Excel.Range).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }//if

                ExcelApp.Cells[row, column] = operDet.SparePart.Manufacturer;

                ExcelApp.Cells[row, column + 3] = operDet.SparePart.MeasureUnit;
                float count = operDet.Count;
                float price = operDet.Price;
                ExcelApp.Cells[row, column + 4] = count;
                ExcelApp.Cells[row, column + 5] = price;
                ExcelApp.Cells[row, column + 6] = price * count;
            }//foreach

            //Обводим талицу рамкой. 
            excelCells = ExcelWorkSheet.get_Range("A" + (row - operDetList.Count + 1).ToString(), "G" + row.ToString());
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
        private void SetColumnsWidth(IList<OperationDetails> operDetList, Excel.Range titleCol, Excel.Range articulCol, Excel.Range manufCol)
        {
            //Устанавливаем ширину первой Колонок
            double titleColWidth = 30; // -- Взято методом тыка.  
            int articulColWidth = 20;
            int manufColWidth = 15, minManufColWidth = 8; //  -- Взято методом тыка.

            //Проверяем по факту максимальную длину колонки Manufacturer и если она меньше заявленной длины, дополняем лишнее в Title
            int maxManufLenght = 0;
            var sparePartsManufacturers = operDetList.Select(od => od.SparePart.Manufacturer).Where(man => man != null);
            if (sparePartsManufacturers.Count() > 0)
                maxManufLenght = sparePartsManufacturers.Max(man => man.Length);

            if (maxManufLenght < manufColWidth)
            {
                int different = manufColWidth - maxManufLenght; //разница между дефолтной шириной столбца и фактической.
                titleColWidth += (manufColWidth - different < minManufColWidth) ? minManufColWidth : different;
                manufColWidth = (manufColWidth - different < minManufColWidth) ? minManufColWidth : manufColWidth - different;
            }//if

            manufCol.Columns.ColumnWidth    = manufColWidth;
            articulCol.Columns.ColumnWidth  = articulColWidth;
            titleCol.Columns.ColumnWidth    = titleColWidth;
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

        /******/
        private void extDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //Если есть выделенные клетки делаем доступной изменение наценки.
            markupComboBox.Enabled = (extDataGridView.SelectedCells.Count > 0);
        }//extDataGridView_SelectionChanged
        /******/
        private void extGroupBox_Click(object sender, EventArgs e)
        {
            extDataGridView.ClearSelection();
        }//extGroupBox_Click
        /******/
        private void markupComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                markupComboBox_SelectedIndexChanged(sender, e);
        }//markupComboBox_PreviewKeyDown

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
        /******/
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

        /*****/
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
            }//if 
        }//saleDataGridView_CellMouseClick        


        /******/
        /// <summary>
        /// Возвращает объект типа Sale, созданный из данных формы.
        /// </summary>
        /// <returns></returns>
        public Sale CreateSaleFromForm()
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();
            foreach (DataGridViewRow row in saleDataGridView.Rows)
            {
                float count = Convert.ToSingle(row.Cells[Count.Index].Value);
                float price = Convert.ToSingle(row.Cells[Price.Index].Value);

                int sparePartId = Convert.ToInt32(row.Cells[SparePartId.Index].Value);
                SparePart sparePart = sparePartsList.First(sp => sp.SparePartId == sparePartId);
                OperationDetails od = new OperationDetails(sparePart, null, count, price);
                operDetList.Add(od);
            }//foreach


            Sale sale = new Sale
            (
                employee: Form1.CurEmployee,
                contragent: PartsDAL.FindCustomers().Where(s => s.ContragentName == customerTextBox.Text).First(), /*!!!ERROR!!!*/
                contragentEmployee: (!String.IsNullOrWhiteSpace(customerAgentTextBox.Text)) ? customerAgentTextBox.Text.Trim() : null,
                operationDate: saleDateTimePicker.Value,
                description: (!String.IsNullOrWhiteSpace(descriptionRichTextBox.Text)) ? descriptionRichTextBox.Text.Trim() : null,
                operDetList: operDetList
            );

            //operDetList.ForEach(od => od.Operation = sale);

            return sale;
        }//CreateSaleFromForm
        /******/
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
        /******/
        private void okButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                customerTextBox_Leave(null, null);
                //sellerTextBox_Leave(null, null);

                if (sellerBackPanel.BackColor != Color.Red && customerBackPanel.BackColor != Color.Red
                    && sparePartsList.Count != 0)
                {   
                    //Проверяем везде ли установлена цена и кол-во.                    
                    foreach (DataGridViewRow row in saleDataGridView.Rows)
                    {
                        if (row.Cells[Price.Index].Value == null || row.Cells[Count.Index].Value == null)
                        {
                            toolTip.Show("Не везде указана цена или количество товара", this, okButton.Location, 3000);
                            return;
                        }
                    }//foreach

                    Sale sale = CreateSaleFromForm();
                    
                    try
                    {
                        //sale.OperationId = PartsDAL.AddSale(sale);
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
//        /// <param name="countCell">Клетка под чьей location необходимо вернуть</param>
//        /// <returns></returns>
//        private Point GetCellBelowLocation(DataGridViewCell countCell)
//        {
//            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(countCell.ColumnIndex, countCell.RowIndex, true).Location;
//            Point dgvLoc = saleDataGridView.Location;
//            Point gbLoc = saleGroupBox.Location;
//            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y + countCell.Size.Height);
//        }//GetCellBelowLocation

//        /// <summary>
//        /// Возвращает абсолютный location области на позиции клетки из saleDataGridView. 
//        /// </summary>
//        /// <param name="countCell">Клетка чей location необходимо вернуть</param>
//        /// <returns></returns>
//        private Point GetCellLocation(DataGridViewCell countCell)
//        {
//            Point cellLoc = saleDataGridView.GetCellDisplayRectangle(countCell.ColumnIndex, countCell.RowIndex, true).Location;
//            Point dgvLoc = saleDataGridView.Location;
//            Point gbLoc = saleGroupBox.Location;
//            return new Point(cellLoc.X + dgvLoc.X + gbLoc.X, cellLoc.Y + dgvLoc.Y + gbLoc.Y);
//        }











//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        #endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#endregion