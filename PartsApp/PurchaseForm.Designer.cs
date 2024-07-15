﻿namespace PartsApp
{
    partial class PurchaseForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.purchaseIdLabel = new System.Windows.Forms.Label();
            this.purchaseDateLabel = new System.Windows.Forms.Label();
            this.supplierLabel = new System.Windows.Forms.Label();
            this.buyerLabel = new System.Windows.Forms.Label();
            this.storageLabel = new System.Windows.Forms.Label();
            this.storageAdressLabel = new System.Windows.Forms.Label();
            this.purchaseGroupBox = new System.Windows.Forms.GroupBox();
            this.PurchaseDGV = new System.Windows.Forms.DataGridView();
            this.currencyLabel = new System.Windows.Forms.Label();
            this.excRateLabel = new System.Windows.Forms.Label();
            this.inTotalLabel = new System.Windows.Forms.Label();
            this.supplierAgentLabel = new System.Windows.Forms.Label();
            this.buyerAgentLabel = new System.Windows.Forms.Label();
            this.purchaseIdTextBox = new System.Windows.Forms.TextBox();
            this.purchaseDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.supplierStarLabel = new System.Windows.Forms.Label();
            this.supplierBackPanel = new System.Windows.Forms.Panel();
            this.supplierTextBox = new System.Windows.Forms.TextBox();
            this.buyerBackPanel = new System.Windows.Forms.Panel();
            this.buyerTextBox = new System.Windows.Forms.TextBox();
            this.buyerStarLabel = new System.Windows.Forms.Label();
            this.storageComboBox = new System.Windows.Forms.ComboBox();
            this.storageAdressTextBox = new System.Windows.Forms.TextBox();
            this.currencyComboBox = new System.Windows.Forms.ComboBox();
            this.inTotalNumberLabel = new System.Windows.Forms.Label();
            this.supplierAgentTextBox = new System.Windows.Forms.TextBox();
            this.buyerAgentTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.storageAdressStarLabel = new System.Windows.Forms.Label();
            this.storageAdressBackPanel = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.markupCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCompleteListBox = new System.Windows.Forms.ListBox();
            this.currencyBackPanel = new System.Windows.Forms.Panel();
            this.excRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.helpLabel = new System.Windows.Forms.Label();
            this.markupComboBox = new System.Windows.Forms.ComboBox();
            this.purchaseContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.SparePartIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StorageCellCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MarkupCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PurchaseDGV)).BeginInit();
            this.supplierBackPanel.SuspendLayout();
            this.buyerBackPanel.SuspendLayout();
            this.storageAdressBackPanel.SuspendLayout();
            this.currencyBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.excRateNumericUpDown)).BeginInit();
            this.purchaseContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // purchaseIdLabel
            // 
            this.purchaseIdLabel.AutoSize = true;
            this.purchaseIdLabel.Location = new System.Drawing.Point(296, 23);
            this.purchaseIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.purchaseIdLabel.Name = "purchaseIdLabel";
            this.purchaseIdLabel.Size = new System.Drawing.Size(96, 16);
            this.purchaseIdLabel.TabIndex = 0;
            this.purchaseIdLabel.Text = "Накладная №";
            // 
            // purchaseDateLabel
            // 
            this.purchaseDateLabel.AutoSize = true;
            this.purchaseDateLabel.Location = new System.Drawing.Point(580, 23);
            this.purchaseDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.purchaseDateLabel.Name = "purchaseDateLabel";
            this.purchaseDateLabel.Size = new System.Drawing.Size(28, 16);
            this.purchaseDateLabel.TabIndex = 1;
            this.purchaseDateLabel.Text = "от :";
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(44, 79);
            this.supplierLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(85, 16);
            this.supplierLabel.TabIndex = 2;
            this.supplierLabel.Text = "Поставщик :";
            // 
            // buyerLabel
            // 
            this.buyerLabel.AutoSize = true;
            this.buyerLabel.Location = new System.Drawing.Point(44, 134);
            this.buyerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buyerLabel.Name = "buyerLabel";
            this.buyerLabel.Size = new System.Drawing.Size(92, 16);
            this.buyerLabel.TabIndex = 3;
            this.buyerLabel.Text = "Покупатель :";
            // 
            // storageLabel
            // 
            this.storageLabel.AutoSize = true;
            this.storageLabel.Location = new System.Drawing.Point(663, 79);
            this.storageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.storageLabel.Name = "storageLabel";
            this.storageLabel.Size = new System.Drawing.Size(113, 16);
            this.storageLabel.TabIndex = 4;
            this.storageLabel.Text = "Осн. / вирт. скл. :";
            this.toolTip.SetToolTip(this.storageLabel, "Основной / виртуальный склад");
            // 
            // storageAdressLabel
            // 
            this.storageAdressLabel.AutoSize = true;
            this.storageAdressLabel.Location = new System.Drawing.Point(663, 130);
            this.storageAdressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.storageAdressLabel.Name = "storageAdressLabel";
            this.storageAdressLabel.Size = new System.Drawing.Size(53, 16);
            this.storageAdressLabel.TabIndex = 5;
            this.storageAdressLabel.Text = "Адрес :";
            this.storageAdressLabel.Visible = false;
            // 
            // purchaseGroupBox
            // 
            this.purchaseGroupBox.Controls.Add(this.PurchaseDGV);
            this.purchaseGroupBox.Location = new System.Drawing.Point(29, 240);
            this.purchaseGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.purchaseGroupBox.Name = "purchaseGroupBox";
            this.purchaseGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.purchaseGroupBox.Size = new System.Drawing.Size(1139, 260);
            this.purchaseGroupBox.TabIndex = 6;
            this.purchaseGroupBox.TabStop = false;
            this.purchaseGroupBox.Text = "Лист прихода.";
            // 
            // PurchaseDGV
            // 
            this.PurchaseDGV.AllowUserToResizeRows = false;
            this.PurchaseDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PurchaseDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PurchaseDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SparePartIdCol,
            this.ArticulCol,
            this.TitleCol,
            this.StorageCellCol,
            this.MeasureUnitCol,
            this.CountCol,
            this.PriceCol,
            this.SumCol,
            this.MarkupCol,
            this.SellingPriceCol});
            this.PurchaseDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PurchaseDGV.Enabled = false;
            this.PurchaseDGV.Location = new System.Drawing.Point(4, 19);
            this.PurchaseDGV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PurchaseDGV.Name = "PurchaseDGV";
            this.PurchaseDGV.RowHeadersWidth = 51;
            this.PurchaseDGV.Size = new System.Drawing.Size(1131, 237);
            this.PurchaseDGV.TabIndex = 0;
            this.PurchaseDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.PurchaseDGV_CellBeginEdit);
            this.PurchaseDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.PurchaseDGV_CellEndEdit);
            this.PurchaseDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PurchaseDGV_CellMouseClick);
            this.PurchaseDGV.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.PurchaseDGV_EditingControlShowing);
            this.PurchaseDGV.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.PurchaseDGV_RowPostPaint);
            this.PurchaseDGV.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.PurchaseDGV_RowsAdded);
            this.PurchaseDGV.SelectionChanged += new System.EventHandler(this.PurchaseDGV_SelectionChanged);
            // 
            // currencyLabel
            // 
            this.currencyLabel.AutoSize = true;
            this.currencyLabel.Location = new System.Drawing.Point(163, 220);
            this.currencyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currencyLabel.Name = "currencyLabel";
            this.currencyLabel.Size = new System.Drawing.Size(63, 16);
            this.currencyLabel.TabIndex = 7;
            this.currencyLabel.Text = "Валюта :";
            // 
            // excRateLabel
            // 
            this.excRateLabel.AutoSize = true;
            this.excRateLabel.Location = new System.Drawing.Point(319, 220);
            this.excRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.excRateLabel.Name = "excRateLabel";
            this.excRateLabel.Size = new System.Drawing.Size(128, 16);
            this.excRateLabel.TabIndex = 8;
            this.excRateLabel.Text = "Курс к рос. рублю :";
            this.toolTip.SetToolTip(this.excRateLabel, "Курс по отношению к российскому рублю");
            this.excRateLabel.Visible = false;
            // 
            // inTotalLabel
            // 
            this.inTotalLabel.AutoSize = true;
            this.inTotalLabel.Location = new System.Drawing.Point(943, 503);
            this.inTotalLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inTotalLabel.Name = "inTotalLabel";
            this.inTotalLabel.Size = new System.Drawing.Size(52, 16);
            this.inTotalLabel.TabIndex = 9;
            this.inTotalLabel.Text = "Итого :";
            // 
            // supplierAgentLabel
            // 
            this.supplierAgentLabel.AutoSize = true;
            this.supplierAgentLabel.Location = new System.Drawing.Point(140, 654);
            this.supplierAgentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.supplierAgentLabel.Name = "supplierAgentLabel";
            this.supplierAgentLabel.Size = new System.Drawing.Size(70, 16);
            this.supplierAgentLabel.TabIndex = 10;
            this.supplierAgentLabel.Text = "Выписал :";
            // 
            // buyerAgentLabel
            // 
            this.buyerAgentLabel.AutoSize = true;
            this.buyerAgentLabel.Location = new System.Drawing.Point(673, 654);
            this.buyerAgentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buyerAgentLabel.Name = "buyerAgentLabel";
            this.buyerAgentLabel.Size = new System.Drawing.Size(62, 16);
            this.buyerAgentLabel.TabIndex = 11;
            this.buyerAgentLabel.Text = "Принял :";
            // 
            // purchaseIdTextBox
            // 
            this.purchaseIdTextBox.Location = new System.Drawing.Point(407, 20);
            this.purchaseIdTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.purchaseIdTextBox.Name = "purchaseIdTextBox";
            this.purchaseIdTextBox.ReadOnly = true;
            this.purchaseIdTextBox.Size = new System.Drawing.Size(132, 22);
            this.purchaseIdTextBox.TabIndex = 12;
            // 
            // purchaseDateTimePicker
            // 
            this.purchaseDateTimePicker.CustomFormat = "";
            this.purchaseDateTimePicker.Location = new System.Drawing.Point(620, 20);
            this.purchaseDateTimePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.purchaseDateTimePicker.MinDate = new System.DateTime(2015, 10, 10, 0, 0, 0, 0);
            this.purchaseDateTimePicker.Name = "purchaseDateTimePicker";
            this.purchaseDateTimePicker.Size = new System.Drawing.Size(265, 22);
            this.purchaseDateTimePicker.TabIndex = 13;
            this.purchaseDateTimePicker.Value = new System.DateTime(2015, 10, 13, 0, 0, 0, 0);
            // 
            // supplierStarLabel
            // 
            this.supplierStarLabel.AutoSize = true;
            this.supplierStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.supplierStarLabel.Location = new System.Drawing.Point(27, 68);
            this.supplierStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.supplierStarLabel.Name = "supplierStarLabel";
            this.supplierStarLabel.Size = new System.Drawing.Size(23, 30);
            this.supplierStarLabel.TabIndex = 14;
            this.supplierStarLabel.Text = "*";
            // 
            // supplierBackPanel
            // 
            this.supplierBackPanel.Controls.Add(this.supplierTextBox);
            this.supplierBackPanel.Location = new System.Drawing.Point(147, 69);
            this.supplierBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.supplierBackPanel.Name = "supplierBackPanel";
            this.supplierBackPanel.Size = new System.Drawing.Size(267, 30);
            this.supplierBackPanel.TabIndex = 15;
            // 
            // supplierTextBox
            // 
            this.supplierTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.supplierTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.supplierTextBox.Location = new System.Drawing.Point(3, 2);
            this.supplierTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.supplierTextBox.Name = "supplierTextBox";
            this.supplierTextBox.Size = new System.Drawing.Size(260, 22);
            this.supplierTextBox.TabIndex = 0;
            this.supplierTextBox.Leave += new System.EventHandler(this.supplierTextBox_Leave);
            this.supplierTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.supplierTextBox_PreviewKeyDown);
            // 
            // buyerBackPanel
            // 
            this.buyerBackPanel.Controls.Add(this.buyerTextBox);
            this.buyerBackPanel.Location = new System.Drawing.Point(144, 124);
            this.buyerBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buyerBackPanel.Name = "buyerBackPanel";
            this.buyerBackPanel.Size = new System.Drawing.Size(267, 30);
            this.buyerBackPanel.TabIndex = 16;
            // 
            // buyerTextBox
            // 
            this.buyerTextBox.AutoCompleteCustomSource.AddRange(new string[] {
            "Truck Tir",
            "ФЛП Тунеев А. С."});
            this.buyerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.buyerTextBox.Location = new System.Drawing.Point(3, 2);
            this.buyerTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buyerTextBox.Name = "buyerTextBox";
            this.buyerTextBox.Size = new System.Drawing.Size(260, 22);
            this.buyerTextBox.TabIndex = 0;
            this.buyerTextBox.Text = "Truck Tir";
            this.buyerTextBox.Leave += new System.EventHandler(this.buyerTextBox_Leave);
            // 
            // buyerStarLabel
            // 
            this.buyerStarLabel.AutoSize = true;
            this.buyerStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buyerStarLabel.Location = new System.Drawing.Point(27, 123);
            this.buyerStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buyerStarLabel.Name = "buyerStarLabel";
            this.buyerStarLabel.Size = new System.Drawing.Size(23, 30);
            this.buyerStarLabel.TabIndex = 17;
            this.buyerStarLabel.Text = "*";
            // 
            // storageComboBox
            // 
            this.storageComboBox.DisplayMember = "Осн. скл.";
            this.storageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.storageComboBox.FormattingEnabled = true;
            this.storageComboBox.Items.AddRange(new object[] {
            "Осн. скл.",
            "Вирт. скл."});
            this.storageComboBox.Location = new System.Drawing.Point(800, 75);
            this.storageComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageComboBox.Name = "storageComboBox";
            this.storageComboBox.Size = new System.Drawing.Size(115, 24);
            this.storageComboBox.TabIndex = 18;
            this.storageComboBox.SelectedIndexChanged += new System.EventHandler(this.storageComboBox_SelectedIndexChanged);
            // 
            // storageAdressTextBox
            // 
            this.storageAdressTextBox.Location = new System.Drawing.Point(3, 2);
            this.storageAdressTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageAdressTextBox.Name = "storageAdressTextBox";
            this.storageAdressTextBox.Size = new System.Drawing.Size(277, 22);
            this.storageAdressTextBox.TabIndex = 19;
            this.storageAdressTextBox.Leave += new System.EventHandler(this.storageAdressTextBox_Leave);
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
            this.currencyComboBox.Location = new System.Drawing.Point(3, 2);
            this.currencyComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.currencyComboBox.Name = "currencyComboBox";
            this.currencyComboBox.Size = new System.Drawing.Size(65, 24);
            this.currencyComboBox.TabIndex = 20;
            this.currencyComboBox.SelectedIndexChanged += new System.EventHandler(this.currencyComboBox_SelectedIndexChanged);
            // 
            // inTotalNumberLabel
            // 
            this.inTotalNumberLabel.AutoSize = true;
            this.inTotalNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTotalNumberLabel.Location = new System.Drawing.Point(1007, 503);
            this.inTotalNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inTotalNumberLabel.Name = "inTotalNumberLabel";
            this.inTotalNumberLabel.Size = new System.Drawing.Size(53, 17);
            this.inTotalNumberLabel.TabIndex = 22;
            this.inTotalNumberLabel.Text = "0 (руб)";
            // 
            // supplierAgentTextBox
            // 
            this.supplierAgentTextBox.Location = new System.Drawing.Point(225, 654);
            this.supplierAgentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.supplierAgentTextBox.Name = "supplierAgentTextBox";
            this.supplierAgentTextBox.Size = new System.Drawing.Size(315, 22);
            this.supplierAgentTextBox.TabIndex = 23;
            // 
            // buyerAgentTextBox
            // 
            this.buyerAgentTextBox.Location = new System.Drawing.Point(749, 654);
            this.buyerAgentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buyerAgentTextBox.Name = "buyerAgentTextBox";
            this.buyerAgentTextBox.ReadOnly = true;
            this.buyerAgentTextBox.Size = new System.Drawing.Size(308, 22);
            this.buyerAgentTextBox.TabIndex = 24;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(444, 720);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 28);
            this.okButton.TabIndex = 25;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(677, 720);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 28);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // storageAdressStarLabel
            // 
            this.storageAdressStarLabel.AutoSize = true;
            this.storageAdressStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.storageAdressStarLabel.Location = new System.Drawing.Point(648, 118);
            this.storageAdressStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.storageAdressStarLabel.Name = "storageAdressStarLabel";
            this.storageAdressStarLabel.Size = new System.Drawing.Size(23, 30);
            this.storageAdressStarLabel.TabIndex = 27;
            this.storageAdressStarLabel.Text = "*";
            this.storageAdressStarLabel.Visible = false;
            // 
            // storageAdressBackPanel
            // 
            this.storageAdressBackPanel.Controls.Add(this.storageAdressTextBox);
            this.storageAdressBackPanel.Location = new System.Drawing.Point(739, 121);
            this.storageAdressBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageAdressBackPanel.Name = "storageAdressBackPanel";
            this.storageAdressBackPanel.Size = new System.Drawing.Size(284, 30);
            this.storageAdressBackPanel.TabIndex = 28;
            this.storageAdressBackPanel.Visible = false;
            // 
            // markupCheckBox
            // 
            this.markupCheckBox.AutoSize = true;
            this.markupCheckBox.Enabled = false;
            this.markupCheckBox.Location = new System.Drawing.Point(801, 219);
            this.markupCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.markupCheckBox.Name = "markupCheckBox";
            this.markupCheckBox.Size = new System.Drawing.Size(163, 20);
            this.markupCheckBox.TabIndex = 1;
            this.markupCheckBox.Text = "установить наценку";
            this.markupCheckBox.UseVisualStyleBackColor = true;
            this.markupCheckBox.CheckedChanged += new System.EventHandler(this.markupCheckBox_CheckedChanged);
            // 
            // autoCompleteListBox
            // 
            this.autoCompleteListBox.BackColor = System.Drawing.SystemColors.Menu;
            this.autoCompleteListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.autoCompleteListBox.FormattingEnabled = true;
            this.autoCompleteListBox.ItemHeight = 17;
            this.autoCompleteListBox.Location = new System.Drawing.Point(433, 111);
            this.autoCompleteListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.autoCompleteListBox.Name = "autoCompleteListBox";
            this.autoCompleteListBox.Size = new System.Drawing.Size(139, 21);
            this.autoCompleteListBox.TabIndex = 29;
            this.autoCompleteListBox.Visible = false;
            this.autoCompleteListBox.DataSourceChanged += new System.EventHandler(this.autoCompleteListBox_DataSourceChanged);
            this.autoCompleteListBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.autoCompleteListBox_Format);
            this.autoCompleteListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.autoCompleteListBox_MouseDown);
            this.autoCompleteListBox.MouseHover += new System.EventHandler(this.autoCompleteListBox_MouseHover);
            // 
            // currencyBackPanel
            // 
            this.currencyBackPanel.Controls.Add(this.currencyComboBox);
            this.currencyBackPanel.Location = new System.Drawing.Point(227, 212);
            this.currencyBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.currencyBackPanel.Name = "currencyBackPanel";
            this.currencyBackPanel.Size = new System.Drawing.Size(72, 31);
            this.currencyBackPanel.TabIndex = 30;
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
            this.excRateNumericUpDown.Location = new System.Drawing.Point(465, 218);
            this.excRateNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.excRateNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.excRateNumericUpDown.Name = "excRateNumericUpDown";
            this.excRateNumericUpDown.Size = new System.Drawing.Size(61, 22);
            this.excRateNumericUpDown.TabIndex = 21;
            this.excRateNumericUpDown.Visible = false;
            this.excRateNumericUpDown.ValueChanged += new System.EventHandler(this.excRateNumericUpDown_ValueChanged);
            this.excRateNumericUpDown.Leave += new System.EventHandler(this.excRateNumericUpDown_Leave);
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpLabel.Location = new System.Drawing.Point(225, 192);
            this.helpLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(292, 17);
            this.helpLabel.TabIndex = 31;
            this.helpLabel.Text = "Выберите валюту и курс к рос. рублю";
            // 
            // markupComboBox
            // 
            this.markupComboBox.DisplayMember = "Value";
            this.markupComboBox.FormattingEnabled = true;
            this.markupComboBox.Location = new System.Drawing.Point(979, 217);
            this.markupComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.markupComboBox.Name = "markupComboBox";
            this.markupComboBox.Size = new System.Drawing.Size(160, 24);
            this.markupComboBox.TabIndex = 32;
            this.markupComboBox.ValueMember = "Key";
            this.markupComboBox.Visible = false;
            this.markupComboBox.SelectedIndexChanged += new System.EventHandler(this.markupComboBox_SelectedIndexChanged);
            this.markupComboBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.markupComboBox_PreviewKeyDown);
            // 
            // purchaseContextMenuStrip
            // 
            this.purchaseContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.purchaseContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.purchaseContextMenuStrip.Name = "purchaseContextMenuStrip";
            this.purchaseContextMenuStrip.Size = new System.Drawing.Size(133, 28);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.removeToolStripMenuItem.Text = "удалить";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Location = new System.Drawing.Point(33, 578);
            this.descriptionRichTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(1129, 47);
            this.descriptionRichTextBox.TabIndex = 33;
            this.descriptionRichTextBox.Text = "";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(29, 559);
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(102, 16);
            this.descriptionLabel.TabIndex = 34;
            this.descriptionLabel.Text = "Комментарий :";
            // 
            // SparePartIdCol
            // 
            this.SparePartIdCol.HeaderText = "Ид";
            this.SparePartIdCol.MinimumWidth = 6;
            this.SparePartIdCol.Name = "SparePartIdCol";
            this.SparePartIdCol.Visible = false;
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
            this.TitleCol.HeaderText = "Название";
            this.TitleCol.MinimumWidth = 6;
            this.TitleCol.Name = "TitleCol";
            // 
            // StorageCellCol
            // 
            this.StorageCellCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StorageCellCol.HeaderText = "Склад";
            this.StorageCellCol.MinimumWidth = 6;
            this.StorageCellCol.Name = "StorageCellCol";
            this.StorageCellCol.ReadOnly = true;
            this.StorageCellCol.Width = 80;
            // 
            // MeasureUnitCol
            // 
            this.MeasureUnitCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle1;
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
            this.CountCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.MinimumWidth = 6;
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.ToolTipText = "Количество";
            this.CountCol.Width = 80;
            // 
            // PriceCol
            // 
            this.PriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.PriceCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.PriceCol.HeaderText = "Цена";
            this.PriceCol.MinimumWidth = 100;
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            // 
            // SumCol
            // 
            this.SumCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.SumCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.SumCol.HeaderText = "Сумма";
            this.SumCol.MinimumWidth = 100;
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            // 
            // MarkupCol
            // 
            this.MarkupCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MarkupCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.MarkupCol.HeaderText = "Наценка";
            this.MarkupCol.MinimumWidth = 6;
            this.MarkupCol.Name = "MarkupCol";
            this.MarkupCol.ReadOnly = true;
            this.MarkupCol.Visible = false;
            this.MarkupCol.Width = 93;
            // 
            // SellingPriceCol
            // 
            this.SellingPriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "C2";
            dataGridViewCellStyle5.NullValue = null;
            this.SellingPriceCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.SellingPriceCol.HeaderText = "Цена продажи (руб)";
            this.SellingPriceCol.MinimumWidth = 100;
            this.SellingPriceCol.Name = "SellingPriceCol";
            this.SellingPriceCol.ReadOnly = true;
            this.SellingPriceCol.Visible = false;
            this.SellingPriceCol.Width = 150;
            // 
            // PurchaseForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 775);
            this.Controls.Add(this.autoCompleteListBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionRichTextBox);
            this.Controls.Add(this.markupComboBox);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.excRateNumericUpDown);
            this.Controls.Add(this.currencyBackPanel);
            this.Controls.Add(this.markupCheckBox);
            this.Controls.Add(this.storageAdressBackPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.buyerAgentTextBox);
            this.Controls.Add(this.supplierAgentTextBox);
            this.Controls.Add(this.inTotalNumberLabel);
            this.Controls.Add(this.storageComboBox);
            this.Controls.Add(this.buyerBackPanel);
            this.Controls.Add(this.supplierBackPanel);
            this.Controls.Add(this.purchaseDateTimePicker);
            this.Controls.Add(this.purchaseIdTextBox);
            this.Controls.Add(this.buyerAgentLabel);
            this.Controls.Add(this.supplierAgentLabel);
            this.Controls.Add(this.inTotalLabel);
            this.Controls.Add(this.excRateLabel);
            this.Controls.Add(this.currencyLabel);
            this.Controls.Add(this.purchaseGroupBox);
            this.Controls.Add(this.storageAdressLabel);
            this.Controls.Add(this.storageLabel);
            this.Controls.Add(this.buyerLabel);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.purchaseDateLabel);
            this.Controls.Add(this.purchaseIdLabel);
            this.Controls.Add(this.supplierStarLabel);
            this.Controls.Add(this.buyerStarLabel);
            this.Controls.Add(this.storageAdressStarLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "PurchaseForm";
            this.Text = "Форма прихода товара.";
            this.Load += new System.EventHandler(this.PurchaseForm_Load);
            this.purchaseGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PurchaseDGV)).EndInit();
            this.supplierBackPanel.ResumeLayout(false);
            this.supplierBackPanel.PerformLayout();
            this.buyerBackPanel.ResumeLayout(false);
            this.buyerBackPanel.PerformLayout();
            this.storageAdressBackPanel.ResumeLayout(false);
            this.storageAdressBackPanel.PerformLayout();
            this.currencyBackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.excRateNumericUpDown)).EndInit();
            this.purchaseContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label purchaseIdLabel;
        private System.Windows.Forms.Label purchaseDateLabel;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.Label buyerLabel;
        private System.Windows.Forms.Label storageLabel;
        private System.Windows.Forms.Label storageAdressLabel;
        private System.Windows.Forms.GroupBox purchaseGroupBox;
        private System.Windows.Forms.DataGridView PurchaseDGV;
        private System.Windows.Forms.Label currencyLabel;
        private System.Windows.Forms.Label excRateLabel;
        private System.Windows.Forms.Label inTotalLabel;
        private System.Windows.Forms.Label supplierAgentLabel;
        private System.Windows.Forms.Label buyerAgentLabel;
        private System.Windows.Forms.TextBox purchaseIdTextBox;
        private System.Windows.Forms.DateTimePicker purchaseDateTimePicker;
        private System.Windows.Forms.Label supplierStarLabel;
        private System.Windows.Forms.Panel supplierBackPanel;
        private System.Windows.Forms.TextBox supplierTextBox;
        private System.Windows.Forms.Panel buyerBackPanel;
        private System.Windows.Forms.TextBox buyerTextBox;
        private System.Windows.Forms.Label buyerStarLabel;
        private System.Windows.Forms.ComboBox storageComboBox;
        private System.Windows.Forms.TextBox storageAdressTextBox;
        private System.Windows.Forms.ComboBox currencyComboBox;
        private System.Windows.Forms.Label inTotalNumberLabel;
        private System.Windows.Forms.TextBox supplierAgentTextBox;
        private System.Windows.Forms.TextBox buyerAgentTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label storageAdressStarLabel;
        private System.Windows.Forms.Panel storageAdressBackPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox markupCheckBox;
        private System.Windows.Forms.ListBox autoCompleteListBox;
        private System.Windows.Forms.Panel currencyBackPanel;
        private System.Windows.Forms.NumericUpDown excRateNumericUpDown;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.ComboBox markupComboBox;
        private System.Windows.Forms.ContextMenuStrip purchaseContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn SparePartIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StorageCellCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MarkupCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPriceCol;
    }
}