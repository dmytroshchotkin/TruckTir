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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            this.autoCompleteListBox = new System.Windows.Forms.ListBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.markupLabel = new System.Windows.Forms.Label();
            this.markupComboBox = new System.Windows.Forms.ComboBox();
            this.inTotalNumberLabel = new System.Windows.Forms.Label();
            this.customerBackPanel = new System.Windows.Forms.Panel();
            this.customerTextBox = new System.Windows.Forms.TextBox();
            this.sellerBackPanel = new System.Windows.Forms.Panel();
            this.sellerTextBox = new System.Windows.Forms.TextBox();
            this.saleDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.saleIdTextBox = new System.Windows.Forms.TextBox();
            this.inTotalLabel = new System.Windows.Forms.Label();
            this.saleGroupBox = new System.Windows.Forms.GroupBox();
            this.SaleDGV = new System.Windows.Forms.DataGridView();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerLabel = new System.Windows.Forms.Label();
            this.sellerLabel = new System.Windows.Forms.Label();
            this.saleDateLabel = new System.Windows.Forms.Label();
            this.saleIdLabel = new System.Windows.Forms.Label();
            this.sellerStarLabel = new System.Windows.Forms.Label();
            this.customerStarLabel = new System.Windows.Forms.Label();
            this.extGroupBox = new System.Windows.Forms.GroupBox();
            this.ExtSaleDGV = new System.Windows.Forms.DataGridView();
            this.ExtSupplierCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtPurchaseDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtPurchaseIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtStorageAdressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtMeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtCountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtMarkupCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtSellingPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtNoteCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.customerAgentTextBox = new System.Windows.Forms.TextBox();
            this.sellerAgentTextBox = new System.Windows.Forms.TextBox();
            this.customerAgentLabel = new System.Windows.Forms.Label();
            this.sellerAgentLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PaidCheckBox = new System.Windows.Forms.CheckBox();
            this.PaidNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.saleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrencyLabel = new System.Windows.Forms.Label();
            this.customerBackPanel.SuspendLayout();
            this.sellerBackPanel.SuspendLayout();
            this.saleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaleDGV)).BeginInit();
            this.extGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExtSaleDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaidNumericUpDown)).BeginInit();
            this.saleContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoCompleteListBox
            // 
            this.autoCompleteListBox.BackColor = System.Drawing.SystemColors.Menu;
            this.autoCompleteListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoCompleteListBox.FormattingEnabled = true;
            this.autoCompleteListBox.Location = new System.Drawing.Point(408, 74);
            this.autoCompleteListBox.Name = "autoCompleteListBox";
            this.autoCompleteListBox.Size = new System.Drawing.Size(190, 30);
            this.autoCompleteListBox.TabIndex = 119;
            this.autoCompleteListBox.Visible = false;
            this.autoCompleteListBox.DataSourceChanged += new System.EventHandler(this.autoCompleteListBox_DataSourceChanged);
            this.autoCompleteListBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.autoCompleteListBox_Format);
            this.autoCompleteListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.autoCompleteListBox_MouseDown);
            this.autoCompleteListBox.MouseHover += new System.EventHandler(this.autoCompleteListBox_MouseHover);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(20, 614);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(83, 13);
            this.descriptionLabel.TabIndex = 124;
            this.descriptionLabel.Text = "Комментарий :";
            // 
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Location = new System.Drawing.Point(23, 630);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(848, 30);
            this.descriptionRichTextBox.TabIndex = 123;
            this.descriptionRichTextBox.Text = "";
            // 
            // markupLabel
            // 
            this.markupLabel.AutoSize = true;
            this.markupLabel.Location = new System.Drawing.Point(194, 425);
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
            this.markupComboBox.Location = new System.Drawing.Point(254, 422);
            this.markupComboBox.Name = "markupComboBox";
            this.markupComboBox.Size = new System.Drawing.Size(121, 21);
            this.markupComboBox.TabIndex = 121;
            this.markupComboBox.ValueMember = "Key";
            this.markupComboBox.SelectedIndexChanged += new System.EventHandler(this.markupComboBox_SelectedIndexChanged);
            this.markupComboBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.markupComboBox_PreviewKeyDown);
            // 
            // inTotalNumberLabel
            // 
            this.inTotalNumberLabel.AutoSize = true;
            this.inTotalNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTotalNumberLabel.Location = new System.Drawing.Point(758, 376);
            this.inTotalNumberLabel.Name = "inTotalNumberLabel";
            this.inTotalNumberLabel.Size = new System.Drawing.Size(28, 13);
            this.inTotalNumberLabel.TabIndex = 118;
            this.inTotalNumberLabel.Text = "0,00";
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
            this.inTotalLabel.Location = new System.Drawing.Point(710, 376);
            this.inTotalLabel.Name = "inTotalLabel";
            this.inTotalLabel.Size = new System.Drawing.Size(43, 13);
            this.inTotalLabel.TabIndex = 110;
            this.inTotalLabel.Text = "Итого :";
            // 
            // saleGroupBox
            // 
            this.saleGroupBox.Controls.Add(this.SaleDGV);
            this.saleGroupBox.Location = new System.Drawing.Point(23, 126);
            this.saleGroupBox.Name = "saleGroupBox";
            this.saleGroupBox.Size = new System.Drawing.Size(854, 247);
            this.saleGroupBox.TabIndex = 107;
            this.saleGroupBox.TabStop = false;
            this.saleGroupBox.Text = "Лист прихода.";
            // 
            // SaleDGV
            // 
            this.SaleDGV.AllowUserToDeleteRows = false;
            this.SaleDGV.AllowUserToResizeRows = false;
            this.SaleDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SaleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SaleDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ArticulCol,
            this.TitleCol,
            this.MeasureUnitCol,
            this.CountCol,
            this.SellingPriceCol,
            this.SumCol});
            this.SaleDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaleDGV.Location = new System.Drawing.Point(3, 16);
            this.SaleDGV.Name = "SaleDGV";
            this.SaleDGV.Size = new System.Drawing.Size(848, 228);
            this.SaleDGV.TabIndex = 0;
            this.SaleDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.SaleDGV_CellBeginEdit);
            this.SaleDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SaleDGV_CellEndEdit);
            this.SaleDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SaleDGV_CellMouseClick);
            this.SaleDGV.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.SaleDGV_EditingControlShowing);
            this.SaleDGV.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.SaleDGV_RowEnter);
            this.SaleDGV.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DGV_RowPostPaint);
            this.SaleDGV.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.SaleDGV_RowsAdded);
            this.SaleDGV.SelectionChanged += new System.EventHandler(this.SaleDGV_SelectionChanged);
            // 
            // ArticulCol
            // 
            this.ArticulCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ArticulCol.HeaderText = "Артикул";
            this.ArticulCol.MinimumWidth = 130;
            this.ArticulCol.Name = "ArticulCol";
            this.ArticulCol.Width = 130;
            // 
            // TitleCol
            // 
            this.TitleCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TitleCol.HeaderText = "Название";
            this.TitleCol.MinimumWidth = 100;
            this.TitleCol.Name = "TitleCol";
            // 
            // MeasureUnitCol
            // 
            this.MeasureUnitCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle33;
            this.MeasureUnitCol.HeaderText = "Ед. изм.";
            this.MeasureUnitCol.MinimumWidth = 35;
            this.MeasureUnitCol.Name = "MeasureUnitCol";
            this.MeasureUnitCol.ReadOnly = true;
            this.MeasureUnitCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MeasureUnitCol.ToolTipText = "Единица измерения";
            this.MeasureUnitCol.Width = 35;
            // 
            // CountCol
            // 
            this.CountCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CountCol.DefaultCellStyle = dataGridViewCellStyle34;
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.ToolTipText = "Количество";
            this.CountCol.Width = 66;
            // 
            // SellingPriceCol
            // 
            this.SellingPriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle35.Format = "C2";
            dataGridViewCellStyle35.NullValue = null;
            this.SellingPriceCol.DefaultCellStyle = dataGridViewCellStyle35;
            this.SellingPriceCol.HeaderText = "Цена продажи";
            this.SellingPriceCol.MinimumWidth = 60;
            this.SellingPriceCol.Name = "SellingPriceCol";
            this.SellingPriceCol.ReadOnly = true;
            this.SellingPriceCol.Width = 96;
            // 
            // SumCol
            // 
            this.SumCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle36.Format = "C2";
            dataGridViewCellStyle36.NullValue = null;
            this.SumCol.DefaultCellStyle = dataGridViewCellStyle36;
            this.SumCol.HeaderText = "Сумма";
            this.SumCol.MinimumWidth = 60;
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            this.SumCol.Width = 66;
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
            this.extGroupBox.Controls.Add(this.ExtSaleDGV);
            this.extGroupBox.Location = new System.Drawing.Point(20, 441);
            this.extGroupBox.Name = "extGroupBox";
            this.extGroupBox.Size = new System.Drawing.Size(857, 170);
            this.extGroupBox.TabIndex = 102;
            this.extGroupBox.TabStop = false;
            this.extGroupBox.Text = "Лист расширенного выбора";
            // 
            // ExtSaleDGV
            // 
            this.ExtSaleDGV.AllowUserToAddRows = false;
            this.ExtSaleDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ExtSaleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExtSaleDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExtSupplierCol,
            this.ExtPurchaseDateCol,
            this.ExtPurchaseIdCol,
            this.ExtStorageAdressCol,
            this.ExtMeasureUnitCol,
            this.ExtCountCol,
            this.ExtPriceCol,
            this.ExtMarkupCol,
            this.ExtSellingPriceCol,
            this.ExtNoteCol});
            this.ExtSaleDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExtSaleDGV.Location = new System.Drawing.Point(3, 16);
            this.ExtSaleDGV.Name = "ExtSaleDGV";
            this.ExtSaleDGV.Size = new System.Drawing.Size(851, 151);
            this.ExtSaleDGV.TabIndex = 0;
            this.ExtSaleDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.ExtSaleDGV_CellBeginEdit);
            this.ExtSaleDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExtSaleDGV_CellEndEdit);
            this.ExtSaleDGV.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DGV_RowPostPaint);
            this.ExtSaleDGV.SelectionChanged += new System.EventHandler(this.ExtSaleDGV_SelectionChanged);
            // 
            // ExtSupplierCol
            // 
            this.ExtSupplierCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExtSupplierCol.HeaderText = "Поставщик";
            this.ExtSupplierCol.Name = "ExtSupplierCol";
            this.ExtSupplierCol.ReadOnly = true;
            // 
            // ExtPurchaseDateCol
            // 
            this.ExtPurchaseDateCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle37.Format = "dd.MM.yyyy \'г.\'   HH:mm";
            this.ExtPurchaseDateCol.DefaultCellStyle = dataGridViewCellStyle37;
            this.ExtPurchaseDateCol.HeaderText = "Дата прихода";
            this.ExtPurchaseDateCol.Name = "ExtPurchaseDateCol";
            this.ExtPurchaseDateCol.ReadOnly = true;
            this.ExtPurchaseDateCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExtPurchaseDateCol.Width = 80;
            // 
            // ExtPurchaseIdCol
            // 
            this.ExtPurchaseIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ExtPurchaseIdCol.HeaderText = "Номер прихода";
            this.ExtPurchaseIdCol.Name = "ExtPurchaseIdCol";
            this.ExtPurchaseIdCol.ReadOnly = true;
            this.ExtPurchaseIdCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExtPurchaseIdCol.Visible = false;
            // 
            // ExtStorageAdressCol
            // 
            this.ExtStorageAdressCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ExtStorageAdressCol.HeaderText = "Адресс хранилища";
            this.ExtStorageAdressCol.MinimumWidth = 100;
            this.ExtStorageAdressCol.Name = "ExtStorageAdressCol";
            this.ExtStorageAdressCol.ReadOnly = true;
            this.ExtStorageAdressCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtMeasureUnitCol
            // 
            this.ExtMeasureUnitCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ExtMeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle38;
            this.ExtMeasureUnitCol.HeaderText = "Ед. изм.";
            this.ExtMeasureUnitCol.MinimumWidth = 35;
            this.ExtMeasureUnitCol.Name = "ExtMeasureUnitCol";
            this.ExtMeasureUnitCol.ReadOnly = true;
            this.ExtMeasureUnitCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ExtMeasureUnitCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExtMeasureUnitCol.ToolTipText = "Единица измерения";
            this.ExtMeasureUnitCol.Width = 35;
            // 
            // ExtCountCol
            // 
            this.ExtCountCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ExtCountCol.DefaultCellStyle = dataGridViewCellStyle39;
            this.ExtCountCol.HeaderText = "Кол-во";
            this.ExtCountCol.MinimumWidth = 100;
            this.ExtCountCol.Name = "ExtCountCol";
            this.ExtCountCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExtCountCol.ToolTipText = "Количество";
            // 
            // ExtPriceCol
            // 
            this.ExtPriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ExtPriceCol.HeaderText = "Цена";
            this.ExtPriceCol.MinimumWidth = 100;
            this.ExtPriceCol.Name = "ExtPriceCol";
            this.ExtPriceCol.ReadOnly = true;
            this.ExtPriceCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExtPriceCol.ToolTipText = "Цена закупки";
            this.ExtPriceCol.Visible = false;
            // 
            // ExtMarkupCol
            // 
            this.ExtMarkupCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ExtMarkupCol.HeaderText = "Наценка";
            this.ExtMarkupCol.MinimumWidth = 100;
            this.ExtMarkupCol.Name = "ExtMarkupCol";
            this.ExtMarkupCol.ReadOnly = true;
            this.ExtMarkupCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtSellingPriceCol
            // 
            this.ExtSellingPriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle40.Format = "C2";
            dataGridViewCellStyle40.NullValue = null;
            this.ExtSellingPriceCol.DefaultCellStyle = dataGridViewCellStyle40;
            this.ExtSellingPriceCol.HeaderText = "Цена продажи";
            this.ExtSellingPriceCol.MinimumWidth = 100;
            this.ExtSellingPriceCol.Name = "ExtSellingPriceCol";
            this.ExtSellingPriceCol.ReadOnly = true;
            this.ExtSellingPriceCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtNoteCol
            // 
            this.ExtNoteCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ExtNoteCol.HeaderText = "Примечание по поставке";
            this.ExtNoteCol.Name = "ExtNoteCol";
            this.ExtNoteCol.ReadOnly = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(512, 706);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 101;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(337, 706);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 100;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // customerAgentTextBox
            // 
            this.customerAgentTextBox.Location = new System.Drawing.Point(583, 673);
            this.customerAgentTextBox.Name = "customerAgentTextBox";
            this.customerAgentTextBox.Size = new System.Drawing.Size(232, 20);
            this.customerAgentTextBox.TabIndex = 99;
            // 
            // sellerAgentTextBox
            // 
            this.sellerAgentTextBox.Location = new System.Drawing.Point(190, 673);
            this.sellerAgentTextBox.Name = "sellerAgentTextBox";
            this.sellerAgentTextBox.ReadOnly = true;
            this.sellerAgentTextBox.Size = new System.Drawing.Size(237, 20);
            this.sellerAgentTextBox.TabIndex = 98;
            // 
            // customerAgentLabel
            // 
            this.customerAgentLabel.AutoSize = true;
            this.customerAgentLabel.Location = new System.Drawing.Point(526, 673);
            this.customerAgentLabel.Name = "customerAgentLabel";
            this.customerAgentLabel.Size = new System.Drawing.Size(51, 13);
            this.customerAgentLabel.TabIndex = 97;
            this.customerAgentLabel.Text = "Принял :";
            // 
            // sellerAgentLabel
            // 
            this.sellerAgentLabel.AutoSize = true;
            this.sellerAgentLabel.Location = new System.Drawing.Point(126, 673);
            this.sellerAgentLabel.Name = "sellerAgentLabel";
            this.sellerAgentLabel.Size = new System.Drawing.Size(58, 13);
            this.sellerAgentLabel.TabIndex = 96;
            this.sellerAgentLabel.Text = "Выписал :";
            // 
            // PaidCheckBox
            // 
            this.PaidCheckBox.AutoSize = true;
            this.PaidCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PaidCheckBox.Checked = true;
            this.PaidCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PaidCheckBox.Location = new System.Drawing.Point(635, 402);
            this.PaidCheckBox.Name = "PaidCheckBox";
            this.PaidCheckBox.Size = new System.Drawing.Size(75, 17);
            this.PaidCheckBox.TabIndex = 125;
            this.PaidCheckBox.Text = "Оплачено";
            this.toolTip.SetToolTip(this.PaidCheckBox, "Если галочка стоит, значит накладная оплачена полностью, иначе укажите оплаченную" +
        " сумму.");
            this.PaidCheckBox.UseVisualStyleBackColor = true;
            this.PaidCheckBox.CheckedChanged += new System.EventHandler(this.PaidCheckBox_CheckedChanged);
            // 
            // PaidNumericUpDown
            // 
            this.PaidNumericUpDown.DecimalPlaces = 2;
            this.PaidNumericUpDown.Enabled = false;
            this.PaidNumericUpDown.Location = new System.Drawing.Point(716, 399);
            this.PaidNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.PaidNumericUpDown.Name = "PaidNumericUpDown";
            this.PaidNumericUpDown.Size = new System.Drawing.Size(81, 20);
            this.PaidNumericUpDown.TabIndex = 126;
            this.toolTip.SetToolTip(this.PaidNumericUpDown, "Укажите оплаченную сумму");
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
            // CurrencyLabel
            // 
            this.CurrencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CurrencyLabel.Location = new System.Drawing.Point(782, 376);
            this.CurrencyLabel.Name = "CurrencyLabel";
            this.CurrencyLabel.Size = new System.Drawing.Size(35, 13);
            this.CurrencyLabel.TabIndex = 0;
            this.CurrencyLabel.Text = "(руб)";
            // 
            // SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 733);
            this.Controls.Add(this.CurrencyLabel);
            this.Controls.Add(this.inTotalNumberLabel);
            this.Controls.Add(this.PaidNumericUpDown);
            this.Controls.Add(this.PaidCheckBox);
            this.Controls.Add(this.autoCompleteListBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionRichTextBox);
            this.Controls.Add(this.markupLabel);
            this.Controls.Add(this.markupComboBox);
            this.Controls.Add(this.customerBackPanel);
            this.Controls.Add(this.sellerBackPanel);
            this.Controls.Add(this.saleDateTimePicker);
            this.Controls.Add(this.saleIdTextBox);
            this.Controls.Add(this.inTotalLabel);
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
            this.customerBackPanel.ResumeLayout(false);
            this.customerBackPanel.PerformLayout();
            this.sellerBackPanel.ResumeLayout(false);
            this.sellerBackPanel.PerformLayout();
            this.saleGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SaleDGV)).EndInit();
            this.extGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExtSaleDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaidNumericUpDown)).EndInit();
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
        private System.Windows.Forms.Label inTotalNumberLabel;
        private System.Windows.Forms.Panel customerBackPanel;
        private System.Windows.Forms.TextBox customerTextBox;
        private System.Windows.Forms.Panel sellerBackPanel;
        private System.Windows.Forms.TextBox sellerTextBox;
        private System.Windows.Forms.DateTimePicker saleDateTimePicker;
        private System.Windows.Forms.TextBox saleIdTextBox;
        private System.Windows.Forms.Label inTotalLabel;
        private System.Windows.Forms.GroupBox saleGroupBox;
        private System.Windows.Forms.DataGridView SaleDGV;
        private System.Windows.Forms.Label customerLabel;
        private System.Windows.Forms.Label sellerLabel;
        private System.Windows.Forms.Label saleDateLabel;
        private System.Windows.Forms.Label saleIdLabel;
        private System.Windows.Forms.Label sellerStarLabel;
        private System.Windows.Forms.Label customerStarLabel;
        private System.Windows.Forms.GroupBox extGroupBox;
        private System.Windows.Forms.DataGridView ExtSaleDGV;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox customerAgentTextBox;
        private System.Windows.Forms.TextBox sellerAgentTextBox;
        private System.Windows.Forms.Label customerAgentLabel;
        private System.Windows.Forms.Label sellerAgentLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip saleContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtSupplierCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtPurchaseDateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtPurchaseIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtStorageAdressCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtMeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtCountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtPriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtMarkupCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtSellingPriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtNoteCol;
        private System.Windows.Forms.CheckBox PaidCheckBox;
        private System.Windows.Forms.NumericUpDown PaidNumericUpDown;
        private System.Windows.Forms.Label CurrencyLabel;
    }
}