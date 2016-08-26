namespace PartsApp
{
    partial class SaleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.autoCompleteListBox = new System.Windows.Forms.ListBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.markupLabel = new System.Windows.Forms.Label();
            this.markupComboBox = new System.Windows.Forms.ComboBox();
            this.excRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.currencyBackPanel = new System.Windows.Forms.Panel();
            this.currencyComboBox = new System.Windows.Forms.ComboBox();
            this.inTotalNumberLabel = new System.Windows.Forms.Label();
            this.customerBackPanel = new System.Windows.Forms.Panel();
            this.customerTextBox = new System.Windows.Forms.TextBox();
            this.sellerBackPanel = new System.Windows.Forms.Panel();
            this.sellerTextBox = new System.Windows.Forms.TextBox();
            this.saleDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.saleIdTextBox = new System.Windows.Forms.TextBox();
            this.inTotalLabel = new System.Windows.Forms.Label();
            this.excRateLabel = new System.Windows.Forms.Label();
            this.currencyLabel = new System.Windows.Forms.Label();
            this.saleGroupBox = new System.Windows.Forms.GroupBox();
            this.saleDataGridView = new System.Windows.Forms.DataGridView();
            this.SparePartId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Articul = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Markup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerLabel = new System.Windows.Forms.Label();
            this.sellerLabel = new System.Windows.Forms.Label();
            this.saleDateLabel = new System.Windows.Forms.Label();
            this.saleIdLabel = new System.Windows.Forms.Label();
            this.sellerStarLabel = new System.Windows.Forms.Label();
            this.customerStarLabel = new System.Windows.Forms.Label();
            this.extGroupBox = new System.Windows.Forms.GroupBox();
            this.extDataGridView = new System.Windows.Forms.DataGridView();
            this.extSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPurchaseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPurchaseId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extArticul = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extStorageAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extMarkup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extSellingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.customerAgentTextBox = new System.Windows.Forms.TextBox();
            this.sellerAgentTextBox = new System.Windows.Forms.TextBox();
            this.customerAgentLabel = new System.Windows.Forms.Label();
            this.sellerAgentLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.saleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.excRateNumericUpDown)).BeginInit();
            this.currencyBackPanel.SuspendLayout();
            this.customerBackPanel.SuspendLayout();
            this.sellerBackPanel.SuspendLayout();
            this.saleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saleDataGridView)).BeginInit();
            this.extGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extDataGridView)).BeginInit();
            this.saleContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoCompleteListBox
            // 
            this.autoCompleteListBox.FormattingEnabled = true;
            this.autoCompleteListBox.Location = new System.Drawing.Point(326, 81);
            this.autoCompleteListBox.Name = "autoCompleteListBox";
            this.autoCompleteListBox.Size = new System.Drawing.Size(105, 30);
            this.autoCompleteListBox.TabIndex = 119;
            this.autoCompleteListBox.Visible = false;
            this.autoCompleteListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.autoCompleteListBox_MouseDown);
            this.autoCompleteListBox.MouseHover += new System.EventHandler(this.autoCompleteListBox_MouseHover);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(20, 624);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(83, 13);
            this.descriptionLabel.TabIndex = 124;
            this.descriptionLabel.Text = "Комментарий :";
            // 
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Location = new System.Drawing.Point(23, 640);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(848, 30);
            this.descriptionRichTextBox.TabIndex = 123;
            this.descriptionRichTextBox.Text = "";
            // 
            // markupLabel
            // 
            this.markupLabel.AutoSize = true;
            this.markupLabel.Location = new System.Drawing.Point(544, 437);
            this.markupLabel.Name = "markupLabel";
            this.markupLabel.Size = new System.Drawing.Size(54, 13);
            this.markupLabel.TabIndex = 122;
            this.markupLabel.Text = "Наценка:";
            // 
            // markupComboBox
            // 
            this.markupComboBox.DisplayMember = "Value";
            this.markupComboBox.Enabled = false;
            this.markupComboBox.FormattingEnabled = true;
            this.markupComboBox.Location = new System.Drawing.Point(604, 434);
            this.markupComboBox.Name = "markupComboBox";
            this.markupComboBox.Size = new System.Drawing.Size(121, 21);
            this.markupComboBox.TabIndex = 121;
            this.markupComboBox.ValueMember = "Key";
            this.markupComboBox.SelectedIndexChanged += new System.EventHandler(this.markupComboBox_SelectedIndexChanged);
            this.markupComboBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.markupComboBox_PreviewKeyDown);
            // 
            // excRateNumericUpDown
            // 
            this.excRateNumericUpDown.DecimalPlaces = 2;
            this.excRateNumericUpDown.Enabled = false;
            this.excRateNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.excRateNumericUpDown.Location = new System.Drawing.Point(350, 138);
            this.excRateNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.excRateNumericUpDown.Name = "excRateNumericUpDown";
            this.excRateNumericUpDown.Size = new System.Drawing.Size(46, 20);
            this.excRateNumericUpDown.TabIndex = 117;
            this.excRateNumericUpDown.Visible = false;
            // 
            // currencyBackPanel
            // 
            this.currencyBackPanel.Controls.Add(this.currencyComboBox);
            this.currencyBackPanel.Location = new System.Drawing.Point(171, 133);
            this.currencyBackPanel.Name = "currencyBackPanel";
            this.currencyBackPanel.Size = new System.Drawing.Size(54, 25);
            this.currencyBackPanel.TabIndex = 120;
            this.currencyBackPanel.Visible = false;
            // 
            // currencyComboBox
            // 
            this.currencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currencyComboBox.FormattingEnabled = true;
            this.currencyComboBox.Items.AddRange(new object[] {
            "руб",
            "грн",
            "евр",
            "дол"});
            this.currencyComboBox.Location = new System.Drawing.Point(2, 2);
            this.currencyComboBox.Name = "currencyComboBox";
            this.currencyComboBox.Size = new System.Drawing.Size(50, 21);
            this.currencyComboBox.TabIndex = 20;
            // 
            // inTotalNumberLabel
            // 
            this.inTotalNumberLabel.AutoSize = true;
            this.inTotalNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTotalNumberLabel.Location = new System.Drawing.Point(758, 406);
            this.inTotalNumberLabel.Name = "inTotalNumberLabel";
            this.inTotalNumberLabel.Size = new System.Drawing.Size(39, 13);
            this.inTotalNumberLabel.TabIndex = 118;
            this.inTotalNumberLabel.Text = "0 (руб)";
            // 
            // customerBackPanel
            // 
            this.customerBackPanel.Controls.Add(this.customerTextBox);
            this.customerBackPanel.Location = new System.Drawing.Point(109, 92);
            this.customerBackPanel.Name = "customerBackPanel";
            this.customerBackPanel.Size = new System.Drawing.Size(200, 24);
            this.customerBackPanel.TabIndex = 115;
            // 
            // customerTextBox
            // 
            this.customerTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.customerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.customerTextBox.Location = new System.Drawing.Point(2, 2);
            this.customerTextBox.Name = "customerTextBox";
            this.customerTextBox.Size = new System.Drawing.Size(196, 20);
            this.customerTextBox.TabIndex = 0;
            this.customerTextBox.Leave += new System.EventHandler(this.customerTextBox_Leave);
            this.customerTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.customerTextBox_PreviewKeyDown);
            // 
            // sellerBackPanel
            // 
            this.sellerBackPanel.Controls.Add(this.sellerTextBox);
            this.sellerBackPanel.Location = new System.Drawing.Point(111, 47);
            this.sellerBackPanel.Name = "sellerBackPanel";
            this.sellerBackPanel.Size = new System.Drawing.Size(200, 24);
            this.sellerBackPanel.TabIndex = 114;
            // 
            // sellerTextBox
            // 
            this.sellerTextBox.AutoCompleteCustomSource.AddRange(new string[] {
            "Truck Tir",
            "ФЛП Тунеев А. С."});
            this.sellerTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.sellerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.sellerTextBox.Location = new System.Drawing.Point(2, 2);
            this.sellerTextBox.Name = "sellerTextBox";
            this.sellerTextBox.Size = new System.Drawing.Size(196, 20);
            this.sellerTextBox.TabIndex = 0;
            this.sellerTextBox.Text = "Truck Tir";
            this.sellerTextBox.Leave += new System.EventHandler(this.sellerTextBox_Leave);
            // 
            // saleDateTimePicker
            // 
            this.saleDateTimePicker.CustomFormat = "";
            this.saleDateTimePicker.Location = new System.Drawing.Point(466, 7);
            this.saleDateTimePicker.Name = "saleDateTimePicker";
            this.saleDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.saleDateTimePicker.TabIndex = 112;
            this.saleDateTimePicker.Value = new System.DateTime(2016, 7, 26, 13, 49, 6, 0);
            // 
            // saleIdTextBox
            // 
            this.saleIdTextBox.Location = new System.Drawing.Point(306, 7);
            this.saleIdTextBox.Name = "saleIdTextBox";
            this.saleIdTextBox.ReadOnly = true;
            this.saleIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.saleIdTextBox.TabIndex = 111;
            // 
            // inTotalLabel
            // 
            this.inTotalLabel.AutoSize = true;
            this.inTotalLabel.Location = new System.Drawing.Point(710, 406);
            this.inTotalLabel.Name = "inTotalLabel";
            this.inTotalLabel.Size = new System.Drawing.Size(43, 13);
            this.inTotalLabel.TabIndex = 110;
            this.inTotalLabel.Text = "Итого :";
            // 
            // excRateLabel
            // 
            this.excRateLabel.AutoSize = true;
            this.excRateLabel.Location = new System.Drawing.Point(240, 140);
            this.excRateLabel.Name = "excRateLabel";
            this.excRateLabel.Size = new System.Drawing.Size(104, 13);
            this.excRateLabel.TabIndex = 109;
            this.excRateLabel.Text = "Курс к рос. рублю :";
            this.excRateLabel.Visible = false;
            // 
            // currencyLabel
            // 
            this.currencyLabel.AutoSize = true;
            this.currencyLabel.Location = new System.Drawing.Point(123, 140);
            this.currencyLabel.Name = "currencyLabel";
            this.currencyLabel.Size = new System.Drawing.Size(51, 13);
            this.currencyLabel.TabIndex = 108;
            this.currencyLabel.Text = "Валюта :";
            this.currencyLabel.Visible = false;
            // 
            // saleGroupBox
            // 
            this.saleGroupBox.Controls.Add(this.saleDataGridView);
            this.saleGroupBox.Location = new System.Drawing.Point(23, 156);
            this.saleGroupBox.Name = "saleGroupBox";
            this.saleGroupBox.Size = new System.Drawing.Size(854, 247);
            this.saleGroupBox.TabIndex = 107;
            this.saleGroupBox.TabStop = false;
            this.saleGroupBox.Text = "Лист прихода.";
            // 
            // saleDataGridView
            // 
            this.saleDataGridView.AllowUserToDeleteRows = false;
            this.saleDataGridView.AllowUserToResizeRows = false;
            this.saleDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.saleDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.saleDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SparePartId,
            this.Articul,
            this.Title,
            this.Unit,
            this.Count,
            this.Price,
            this.Markup,
            this.SellingPrice,
            this.Sum});
            this.saleDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saleDataGridView.Location = new System.Drawing.Point(3, 16);
            this.saleDataGridView.Name = "saleDataGridView";
            this.saleDataGridView.Size = new System.Drawing.Size(848, 228);
            this.saleDataGridView.TabIndex = 0;
            this.saleDataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.saleDataGridView_CellBeginEdit);
            this.saleDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.saleDataGridView_CellEndEdit);
            this.saleDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.saleDataGridView_CellMouseClick);
            this.saleDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.saleDataGridView_EditingControlShowing);
            this.saleDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.saleDataGridView_RowEnter);
            this.saleDataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.saleDataGridView_RowPrePaint);
            this.saleDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.saleDataGridView_RowsAdded);
            this.saleDataGridView.SelectionChanged += new System.EventHandler(this.saleDataGridView_SelectionChanged);
            // 
            // SparePartId
            // 
            this.SparePartId.HeaderText = "Ид";
            this.SparePartId.Name = "SparePartId";
            this.SparePartId.Visible = false;
            // 
            // Articul
            // 
            this.Articul.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Articul.HeaderText = "Артикул";
            this.Articul.MinimumWidth = 130;
            this.Articul.Name = "Articul";
            this.Articul.Width = 130;
            // 
            // Title
            // 
            this.Title.HeaderText = "Название";
            this.Title.Name = "Title";
            // 
            // Unit
            // 
            this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Unit.HeaderText = "Ед. изм.";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.ToolTipText = "Единица измерения";
            this.Unit.Width = 69;
            // 
            // Count
            // 
            this.Count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Count.HeaderText = "Кол-во";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            this.Count.ToolTipText = "Количество";
            this.Count.Width = 66;
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Price.HeaderText = "Цена";
            this.Price.MinimumWidth = 100;
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Visible = false;
            // 
            // Markup
            // 
            this.Markup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Markup.HeaderText = "Наценка";
            this.Markup.Name = "Markup";
            this.Markup.Visible = false;
            // 
            // SellingPrice
            // 
            this.SellingPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SellingPrice.HeaderText = "Цена продажи (руб)";
            this.SellingPrice.MinimumWidth = 100;
            this.SellingPrice.Name = "SellingPrice";
            this.SellingPrice.ReadOnly = true;
            this.SellingPrice.Width = 120;
            // 
            // Sum
            // 
            this.Sum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Sum.HeaderText = "Сумма";
            this.Sum.MinimumWidth = 100;
            this.Sum.Name = "Sum";
            this.Sum.ReadOnly = true;
            // 
            // customerLabel
            // 
            this.customerLabel.AutoSize = true;
            this.customerLabel.Location = new System.Drawing.Point(34, 100);
            this.customerLabel.Name = "customerLabel";
            this.customerLabel.Size = new System.Drawing.Size(73, 13);
            this.customerLabel.TabIndex = 106;
            this.customerLabel.Text = "Покупатель :";
            // 
            // sellerLabel
            // 
            this.sellerLabel.AutoSize = true;
            this.sellerLabel.Location = new System.Drawing.Point(34, 55);
            this.sellerLabel.Name = "sellerLabel";
            this.sellerLabel.Size = new System.Drawing.Size(63, 13);
            this.sellerLabel.TabIndex = 105;
            this.sellerLabel.Text = "Продавец :";
            // 
            // saleDateLabel
            // 
            this.saleDateLabel.AutoSize = true;
            this.saleDateLabel.Location = new System.Drawing.Point(436, 10);
            this.saleDateLabel.Name = "saleDateLabel";
            this.saleDateLabel.Size = new System.Drawing.Size(24, 13);
            this.saleDateLabel.TabIndex = 104;
            this.saleDateLabel.Text = "от :";
            // 
            // saleIdLabel
            // 
            this.saleIdLabel.AutoSize = true;
            this.saleIdLabel.Location = new System.Drawing.Point(223, 10);
            this.saleIdLabel.Name = "saleIdLabel";
            this.saleIdLabel.Size = new System.Drawing.Size(77, 13);
            this.saleIdLabel.TabIndex = 103;
            this.saleIdLabel.Text = "Накладная №";
            // 
            // sellerStarLabel
            // 
            this.sellerStarLabel.AutoSize = true;
            this.sellerStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sellerStarLabel.Location = new System.Drawing.Point(21, 46);
            this.sellerStarLabel.Name = "sellerStarLabel";
            this.sellerStarLabel.Size = new System.Drawing.Size(20, 25);
            this.sellerStarLabel.TabIndex = 113;
            this.sellerStarLabel.Text = "*";
            // 
            // customerStarLabel
            // 
            this.customerStarLabel.AutoSize = true;
            this.customerStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.customerStarLabel.Location = new System.Drawing.Point(21, 91);
            this.customerStarLabel.Name = "customerStarLabel";
            this.customerStarLabel.Size = new System.Drawing.Size(20, 25);
            this.customerStarLabel.TabIndex = 116;
            this.customerStarLabel.Text = "*";
            // 
            // extGroupBox
            // 
            this.extGroupBox.Controls.Add(this.extDataGridView);
            this.extGroupBox.Location = new System.Drawing.Point(20, 451);
            this.extGroupBox.Name = "extGroupBox";
            this.extGroupBox.Size = new System.Drawing.Size(857, 170);
            this.extGroupBox.TabIndex = 102;
            this.extGroupBox.TabStop = false;
            this.extGroupBox.Text = "Лист расширенного выбора";
            // 
            // extDataGridView
            // 
            this.extDataGridView.AllowUserToAddRows = false;
            this.extDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.extDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.extDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.extSupplier,
            this.extPurchaseDate,
            this.extPurchaseId,
            this.extTitle,
            this.extArticul,
            this.extStorageAdress,
            this.extUnit,
            this.extCount,
            this.extPrice,
            this.extMarkup,
            this.extSellingPrice});
            this.extDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extDataGridView.Location = new System.Drawing.Point(3, 16);
            this.extDataGridView.Name = "extDataGridView";
            this.extDataGridView.Size = new System.Drawing.Size(851, 151);
            this.extDataGridView.TabIndex = 0;
            this.extDataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.extDataGridView_CellBeginEdit);
            this.extDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.extDataGridView_CellEndEdit);
            this.extDataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.saleDataGridView_RowPrePaint);
            this.extDataGridView.SelectionChanged += new System.EventHandler(this.extDataGridView_SelectionChanged);
            // 
            // extSupplier
            // 
            this.extSupplier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.extSupplier.HeaderText = "Поставщик";
            this.extSupplier.Name = "extSupplier";
            this.extSupplier.ReadOnly = true;
            this.extSupplier.Width = 90;
            // 
            // extPurchaseDate
            // 
            this.extPurchaseDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.extPurchaseDate.HeaderText = "Дата прихода";
            this.extPurchaseDate.Name = "extPurchaseDate";
            this.extPurchaseDate.ReadOnly = true;
            this.extPurchaseDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extPurchaseDate.Width = 83;
            // 
            // extPurchaseId
            // 
            this.extPurchaseId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.extPurchaseId.HeaderText = "Номер прихода";
            this.extPurchaseId.Name = "extPurchaseId";
            this.extPurchaseId.ReadOnly = true;
            this.extPurchaseId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extPurchaseId.Visible = false;
            // 
            // extTitle
            // 
            this.extTitle.HeaderText = "Название";
            this.extTitle.Name = "extTitle";
            this.extTitle.ReadOnly = true;
            this.extTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extTitle.Visible = false;
            // 
            // extArticul
            // 
            this.extArticul.HeaderText = "Артикул";
            this.extArticul.Name = "extArticul";
            this.extArticul.ReadOnly = true;
            this.extArticul.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extArticul.Visible = false;
            // 
            // extStorageAdress
            // 
            this.extStorageAdress.HeaderText = "Адресс хранилища";
            this.extStorageAdress.MinimumWidth = 100;
            this.extStorageAdress.Name = "extStorageAdress";
            this.extStorageAdress.ReadOnly = true;
            this.extStorageAdress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // extUnit
            // 
            this.extUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.extUnit.HeaderText = "Ед. изм.";
            this.extUnit.Name = "extUnit";
            this.extUnit.ReadOnly = true;
            this.extUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extUnit.ToolTipText = "Единица измерения";
            this.extUnit.Width = 55;
            // 
            // extCount
            // 
            this.extCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.extCount.HeaderText = "Кол-во";
            this.extCount.MinimumWidth = 100;
            this.extCount.Name = "extCount";
            this.extCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extCount.ToolTipText = "Количество";
            // 
            // extPrice
            // 
            this.extPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.extPrice.HeaderText = "Цена";
            this.extPrice.MinimumWidth = 100;
            this.extPrice.Name = "extPrice";
            this.extPrice.ReadOnly = true;
            this.extPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.extPrice.ToolTipText = "Цена закупки";
            // 
            // extMarkup
            // 
            this.extMarkup.HeaderText = "Наценка";
            this.extMarkup.MinimumWidth = 100;
            this.extMarkup.Name = "extMarkup";
            this.extMarkup.ReadOnly = true;
            this.extMarkup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // extSellingPrice
            // 
            this.extSellingPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.extSellingPrice.HeaderText = "Цена продажи";
            this.extSellingPrice.MinimumWidth = 100;
            this.extSellingPrice.Name = "extSellingPrice";
            this.extSellingPrice.ReadOnly = true;
            this.extSellingPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(509, 717);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 101;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(334, 717);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 100;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // customerAgentTextBox
            // 
            this.customerAgentTextBox.Location = new System.Drawing.Point(580, 684);
            this.customerAgentTextBox.Name = "customerAgentTextBox";
            this.customerAgentTextBox.Size = new System.Drawing.Size(232, 20);
            this.customerAgentTextBox.TabIndex = 99;
            // 
            // sellerAgentTextBox
            // 
            this.sellerAgentTextBox.Location = new System.Drawing.Point(187, 684);
            this.sellerAgentTextBox.Name = "sellerAgentTextBox";
            this.sellerAgentTextBox.ReadOnly = true;
            this.sellerAgentTextBox.Size = new System.Drawing.Size(237, 20);
            this.sellerAgentTextBox.TabIndex = 98;
            // 
            // customerAgentLabel
            // 
            this.customerAgentLabel.AutoSize = true;
            this.customerAgentLabel.Location = new System.Drawing.Point(523, 684);
            this.customerAgentLabel.Name = "customerAgentLabel";
            this.customerAgentLabel.Size = new System.Drawing.Size(51, 13);
            this.customerAgentLabel.TabIndex = 97;
            this.customerAgentLabel.Text = "Принял :";
            // 
            // sellerAgentLabel
            // 
            this.sellerAgentLabel.AutoSize = true;
            this.sellerAgentLabel.Location = new System.Drawing.Point(123, 684);
            this.sellerAgentLabel.Name = "sellerAgentLabel";
            this.sellerAgentLabel.Size = new System.Drawing.Size(58, 13);
            this.sellerAgentLabel.TabIndex = 96;
            this.sellerAgentLabel.Text = "Выписал :";
            // 
            // saleContextMenuStrip
            // 
            this.saleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.saleContextMenuStrip.Name = "saleContextMenuStrip";
            this.saleContextMenuStrip.Size = new System.Drawing.Size(119, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.removeToolStripMenuItem.Text = "Удалить";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 747);
            this.Controls.Add(this.autoCompleteListBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionRichTextBox);
            this.Controls.Add(this.markupLabel);
            this.Controls.Add(this.markupComboBox);
            this.Controls.Add(this.excRateNumericUpDown);
            this.Controls.Add(this.currencyBackPanel);
            this.Controls.Add(this.inTotalNumberLabel);
            this.Controls.Add(this.customerBackPanel);
            this.Controls.Add(this.sellerBackPanel);
            this.Controls.Add(this.saleDateTimePicker);
            this.Controls.Add(this.saleIdTextBox);
            this.Controls.Add(this.inTotalLabel);
            this.Controls.Add(this.excRateLabel);
            this.Controls.Add(this.currencyLabel);
            this.Controls.Add(this.saleGroupBox);
            this.Controls.Add(this.customerLabel);
            this.Controls.Add(this.sellerLabel);
            this.Controls.Add(this.saleDateLabel);
            this.Controls.Add(this.saleIdLabel);
            this.Controls.Add(this.sellerStarLabel);
            this.Controls.Add(this.customerStarLabel);
            this.Controls.Add(this.extGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.customerAgentTextBox);
            this.Controls.Add(this.sellerAgentTextBox);
            this.Controls.Add(this.customerAgentLabel);
            this.Controls.Add(this.sellerAgentLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SaleForm";
            this.Text = "Форма продаж.";
            this.Load += new System.EventHandler(this.SaleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.excRateNumericUpDown)).EndInit();
            this.currencyBackPanel.ResumeLayout(false);
            this.customerBackPanel.ResumeLayout(false);
            this.customerBackPanel.PerformLayout();
            this.sellerBackPanel.ResumeLayout(false);
            this.sellerBackPanel.PerformLayout();
            this.saleGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.saleDataGridView)).EndInit();
            this.extGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extDataGridView)).EndInit();
            this.saleContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox autoCompleteListBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label markupLabel;
        private System.Windows.Forms.ComboBox markupComboBox;
        private System.Windows.Forms.NumericUpDown excRateNumericUpDown;
        private System.Windows.Forms.Panel currencyBackPanel;
        private System.Windows.Forms.ComboBox currencyComboBox;
        private System.Windows.Forms.Label inTotalNumberLabel;
        private System.Windows.Forms.Panel customerBackPanel;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.Panel sellerBackPanel;
        private System.Windows.Forms.TextBox sellerTextBox;
        private System.Windows.Forms.DateTimePicker saleDateTimePicker;
        private System.Windows.Forms.TextBox saleIdTextBox;
        private System.Windows.Forms.Label inTotalLabel;
        private System.Windows.Forms.Label excRateLabel;
        private System.Windows.Forms.Label currencyLabel;
        private System.Windows.Forms.GroupBox saleGroupBox;
        private System.Windows.Forms.DataGridView saleDataGridView;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.Label sellerLabel;
        private System.Windows.Forms.Label saleDateLabel;
        private System.Windows.Forms.Label saleIdLabel;
        private System.Windows.Forms.Label sellerStarLabel;
        private System.Windows.Forms.Label customerStarLabel;
        private System.Windows.Forms.GroupBox extGroupBox;
        private System.Windows.Forms.DataGridView extDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn extSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn extPurchaseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn extPurchaseId;
        private System.Windows.Forms.DataGridViewTextBoxColumn extTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn extArticul;
        private System.Windows.Forms.DataGridViewTextBoxColumn extStorageAdress;
        private System.Windows.Forms.DataGridViewTextBoxColumn extUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn extCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn extPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn extMarkup;
        private System.Windows.Forms.DataGridViewTextBoxColumn extSellingPrice;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox customerAgentTextBox;
        private System.Windows.Forms.TextBox sellerAgentTextBox;
        private System.Windows.Forms.Label customerAgentLabel;
        private System.Windows.Forms.Label sellerAgentLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip saleContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn SparePartId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Articul;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Markup;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sum;
    }
}