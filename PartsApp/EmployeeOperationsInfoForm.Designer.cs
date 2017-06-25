namespace PartsApp
{
    partial class EmployeeOperationsInfoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.EmployeeGroupBox = new System.Windows.Forms.GroupBox();
            this.EmployeeListBox = new System.Windows.Forms.ListBox();
            this.ActivEmployeesCheckBox = new System.Windows.Forms.CheckBox();
            this.InactiveEmployeesCheckBox = new System.Windows.Forms.CheckBox();
            this.BottomSplitContainer = new System.Windows.Forms.SplitContainer();
            this.OperationsGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OperationsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.OperationsCountStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PurchaseCheckBox = new System.Windows.Forms.CheckBox();
            this.SaleCheckBox = new System.Windows.Forms.CheckBox();
            this.BeginDatePanel = new System.Windows.Forms.Panel();
            this.BeginDateCheckBox = new System.Windows.Forms.CheckBox();
            this.BeginDateDTP = new System.Windows.Forms.DateTimePicker();
            this.EndDatePanel = new System.Windows.Forms.Panel();
            this.EndDateCheckBox = new System.Windows.Forms.CheckBox();
            this.EndDateDTP = new System.Windows.Forms.DateTimePicker();
            this.OperationDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.OperationDetailsDGV = new System.Windows.Forms.DataGridView();
            this.ManufacturerCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.OperationsInfoDGV = new System.Windows.Forms.DataGridView();
            this.OperationTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.EmployeeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BottomSplitContainer)).BeginInit();
            this.BottomSplitContainer.Panel1.SuspendLayout();
            this.BottomSplitContainer.Panel2.SuspendLayout();
            this.BottomSplitContainer.SuspendLayout();
            this.OperationsGroupBox.SuspendLayout();
            this.OperationsStatusStrip.SuspendLayout();
            this.BeginDatePanel.SuspendLayout();
            this.EndDatePanel.SuspendLayout();
            this.OperationDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.EmployeeGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.BottomSplitContainer);
            this.splitContainer1.Size = new System.Drawing.Size(834, 685);
            this.splitContainer1.SplitterDistance = 129;
            this.splitContainer1.TabIndex = 0;
            // 
            // EmployeeGroupBox
            // 
            this.EmployeeGroupBox.Controls.Add(this.EmployeeListBox);
            this.EmployeeGroupBox.Controls.Add(this.ActivEmployeesCheckBox);
            this.EmployeeGroupBox.Controls.Add(this.InactiveEmployeesCheckBox);
            this.EmployeeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmployeeGroupBox.Location = new System.Drawing.Point(0, 0);
            this.EmployeeGroupBox.Name = "EmployeeGroupBox";
            this.EmployeeGroupBox.Size = new System.Drawing.Size(834, 129);
            this.EmployeeGroupBox.TabIndex = 1;
            this.EmployeeGroupBox.TabStop = false;
            this.EmployeeGroupBox.Text = "Сотрудники";
            // 
            // EmployeeListBox
            // 
            this.EmployeeListBox.DisplayMember = "FullName";
            this.EmployeeListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmployeeListBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.EmployeeListBox.Location = new System.Drawing.Point(3, 16);
            this.EmployeeListBox.Name = "EmployeeListBox";
            this.EmployeeListBox.Size = new System.Drawing.Size(828, 110);
            this.EmployeeListBox.TabIndex = 0;
            this.EmployeeListBox.ValueMember = "EmployeeId";
            // 
            // ActivEmployeesCheckBox
            // 
            this.ActivEmployeesCheckBox.AutoSize = true;
            this.ActivEmployeesCheckBox.Checked = true;
            this.ActivEmployeesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActivEmployeesCheckBox.Location = new System.Drawing.Point(86, 0);
            this.ActivEmployeesCheckBox.Name = "ActivEmployeesCheckBox";
            this.ActivEmployeesCheckBox.Size = new System.Drawing.Size(76, 17);
            this.ActivEmployeesCheckBox.TabIndex = 1;
            this.ActivEmployeesCheckBox.Text = "Активные";
            this.toolTip1.SetToolTip(this.ActivEmployeesCheckBox, "Отображать дейстующих сотрудников");
            this.ActivEmployeesCheckBox.UseVisualStyleBackColor = true;
            this.ActivEmployeesCheckBox.Visible = false;
            // 
            // InactiveEmployeesCheckBox
            // 
            this.InactiveEmployeesCheckBox.AutoSize = true;
            this.InactiveEmployeesCheckBox.Checked = true;
            this.InactiveEmployeesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.InactiveEmployeesCheckBox.Location = new System.Drawing.Point(179, 0);
            this.InactiveEmployeesCheckBox.Name = "InactiveEmployeesCheckBox";
            this.InactiveEmployeesCheckBox.Size = new System.Drawing.Size(92, 17);
            this.InactiveEmployeesCheckBox.TabIndex = 2;
            this.InactiveEmployeesCheckBox.Text = "Не активные";
            this.toolTip1.SetToolTip(this.InactiveEmployeesCheckBox, "Отображать не действующих сотрудников. Неактивным считается сотрудник у которого " +
        "в профиле заполнено поле \'Дата Увольнения\'.");
            this.InactiveEmployeesCheckBox.UseVisualStyleBackColor = true;
            this.InactiveEmployeesCheckBox.Visible = false;
            // 
            // BottomSplitContainer
            // 
            this.BottomSplitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BottomSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.BottomSplitContainer.Name = "BottomSplitContainer";
            this.BottomSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // BottomSplitContainer.Panel1
            // 
            this.BottomSplitContainer.Panel1.Controls.Add(this.OperationsGroupBox);
            // 
            // BottomSplitContainer.Panel2
            // 
            this.BottomSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.BottomSplitContainer.Panel2.Controls.Add(this.OperationDetailsGroupBox);
            this.BottomSplitContainer.Size = new System.Drawing.Size(834, 552);
            this.BottomSplitContainer.SplitterDistance = 280;
            this.BottomSplitContainer.SplitterWidth = 2;
            this.BottomSplitContainer.TabIndex = 0;
            // 
            // OperationsGroupBox
            // 
            this.OperationsGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.OperationsGroupBox.Controls.Add(this.OperationsInfoDGV);
            this.OperationsGroupBox.Controls.Add(this.label2);
            this.OperationsGroupBox.Controls.Add(this.label1);
            this.OperationsGroupBox.Controls.Add(this.OperationsStatusStrip);
            this.OperationsGroupBox.Controls.Add(this.PurchaseCheckBox);
            this.OperationsGroupBox.Controls.Add(this.SaleCheckBox);
            this.OperationsGroupBox.Controls.Add(this.BeginDatePanel);
            this.OperationsGroupBox.Controls.Add(this.EndDatePanel);
            this.OperationsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OperationsGroupBox.Name = "OperationsGroupBox";
            this.OperationsGroupBox.Size = new System.Drawing.Size(834, 280);
            this.OperationsGroupBox.TabIndex = 0;
            this.OperationsGroupBox.TabStop = false;
            this.OperationsGroupBox.Text = "Операции";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(476, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "до :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(312, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "от :";
            // 
            // OperationsStatusStrip
            // 
            this.OperationsStatusStrip.AutoSize = false;
            this.OperationsStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.OperationsCountStatusLabel});
            this.OperationsStatusStrip.Location = new System.Drawing.Point(3, 259);
            this.OperationsStatusStrip.Name = "OperationsStatusStrip";
            this.OperationsStatusStrip.Size = new System.Drawing.Size(828, 18);
            this.OperationsStatusStrip.TabIndex = 1;
            this.OperationsStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(89, 13);
            this.toolStripStatusLabel1.Text = "Всего док-тов :";
            // 
            // OperationsCountStatusLabel
            // 
            this.OperationsCountStatusLabel.Name = "OperationsCountStatusLabel";
            this.OperationsCountStatusLabel.Size = new System.Drawing.Size(0, 13);
            // 
            // PurchaseCheckBox
            // 
            this.PurchaseCheckBox.AutoSize = true;
            this.PurchaseCheckBox.Checked = true;
            this.PurchaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PurchaseCheckBox.Location = new System.Drawing.Point(124, 0);
            this.PurchaseCheckBox.Name = "PurchaseCheckBox";
            this.PurchaseCheckBox.Size = new System.Drawing.Size(63, 17);
            this.PurchaseCheckBox.TabIndex = 3;
            this.PurchaseCheckBox.Text = "Приход";
            this.PurchaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaleCheckBox
            // 
            this.SaleCheckBox.AutoSize = true;
            this.SaleCheckBox.Checked = true;
            this.SaleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaleCheckBox.Location = new System.Drawing.Point(193, 0);
            this.SaleCheckBox.Name = "SaleCheckBox";
            this.SaleCheckBox.Size = new System.Drawing.Size(62, 17);
            this.SaleCheckBox.TabIndex = 4;
            this.SaleCheckBox.Text = "Расход";
            this.SaleCheckBox.UseVisualStyleBackColor = true;
            // 
            // BeginDatePanel
            // 
            this.BeginDatePanel.Controls.Add(this.BeginDateCheckBox);
            this.BeginDatePanel.Controls.Add(this.BeginDateDTP);
            this.BeginDatePanel.Location = new System.Drawing.Point(342, -1);
            this.BeginDatePanel.Name = "BeginDatePanel";
            this.BeginDatePanel.Size = new System.Drawing.Size(108, 18);
            this.BeginDatePanel.TabIndex = 7;
            // 
            // BeginDateCheckBox
            // 
            this.BeginDateCheckBox.AutoSize = true;
            this.BeginDateCheckBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BeginDateCheckBox.Checked = true;
            this.BeginDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BeginDateCheckBox.Location = new System.Drawing.Point(59, 2);
            this.BeginDateCheckBox.Name = "BeginDateCheckBox";
            this.BeginDateCheckBox.Size = new System.Drawing.Size(15, 14);
            this.BeginDateCheckBox.TabIndex = 6;
            this.BeginDateCheckBox.UseVisualStyleBackColor = false;
            this.BeginDateCheckBox.CheckedChanged += new System.EventHandler(this.DatesCheckBox_CheckedChanged);
            // 
            // BeginDateDTP
            // 
            this.BeginDateDTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BeginDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.BeginDateDTP.Location = new System.Drawing.Point(0, 0);
            this.BeginDateDTP.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.BeginDateDTP.Name = "BeginDateDTP";
            this.BeginDateDTP.Size = new System.Drawing.Size(108, 20);
            this.BeginDateDTP.TabIndex = 5;
            this.toolTip1.SetToolTip(this.BeginDateDTP, "Начальная дата");
            this.BeginDateDTP.Value = new System.DateTime(2017, 9, 21, 0, 0, 0, 0);
            this.BeginDateDTP.ValueChanged += new System.EventHandler(this.DatesDTP_ValueChanged);
            // 
            // EndDatePanel
            // 
            this.EndDatePanel.Controls.Add(this.EndDateCheckBox);
            this.EndDatePanel.Controls.Add(this.EndDateDTP);
            this.EndDatePanel.Location = new System.Drawing.Point(507, -1);
            this.EndDatePanel.Name = "EndDatePanel";
            this.EndDatePanel.Size = new System.Drawing.Size(108, 18);
            this.EndDatePanel.TabIndex = 9;
            // 
            // EndDateCheckBox
            // 
            this.EndDateCheckBox.AutoSize = true;
            this.EndDateCheckBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.EndDateCheckBox.Checked = true;
            this.EndDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EndDateCheckBox.Location = new System.Drawing.Point(59, 2);
            this.EndDateCheckBox.Name = "EndDateCheckBox";
            this.EndDateCheckBox.Size = new System.Drawing.Size(15, 14);
            this.EndDateCheckBox.TabIndex = 6;
            this.EndDateCheckBox.UseVisualStyleBackColor = false;
            this.EndDateCheckBox.CheckedChanged += new System.EventHandler(this.DatesCheckBox_CheckedChanged);
            // 
            // EndDateDTP
            // 
            this.EndDateDTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EndDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.EndDateDTP.Location = new System.Drawing.Point(0, 0);
            this.EndDateDTP.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.EndDateDTP.Name = "EndDateDTP";
            this.EndDateDTP.Size = new System.Drawing.Size(108, 20);
            this.EndDateDTP.TabIndex = 5;
            this.toolTip1.SetToolTip(this.EndDateDTP, "Начальная дата");
            this.EndDateDTP.Value = new System.DateTime(2017, 9, 21, 0, 0, 0, 0);
            // 
            // OperationDetailsGroupBox
            // 
            this.OperationDetailsGroupBox.Controls.Add(this.OperationDetailsDGV);
            this.OperationDetailsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationDetailsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OperationDetailsGroupBox.Name = "OperationDetailsGroupBox";
            this.OperationDetailsGroupBox.Size = new System.Drawing.Size(834, 270);
            this.OperationDetailsGroupBox.TabIndex = 2;
            this.OperationDetailsGroupBox.TabStop = false;
            this.OperationDetailsGroupBox.Text = "Доп. инф-ция по операции.";
            // 
            // OperationDetailsDGV
            // 
            this.OperationDetailsDGV.AllowUserToAddRows = false;
            this.OperationDetailsDGV.AllowUserToDeleteRows = false;
            this.OperationDetailsDGV.AllowUserToResizeRows = false;
            this.OperationDetailsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OperationDetailsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ManufacturerCol,
            this.ArticulCol,
            this.TitleCol,
            this.MeasureUnitCol,
            this.CountCol,
            this.PriceCol,
            this.SumCol});
            this.OperationDetailsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationDetailsDGV.Location = new System.Drawing.Point(3, 16);
            this.OperationDetailsDGV.Name = "OperationDetailsDGV";
            this.OperationDetailsDGV.Size = new System.Drawing.Size(828, 251);
            this.OperationDetailsDGV.TabIndex = 0;
            // 
            // ManufacturerCol
            // 
            this.ManufacturerCol.HeaderText = "Производитель";
            this.ManufacturerCol.Name = "ManufacturerCol";
            this.ManufacturerCol.ReadOnly = true;
            // 
            // ArticulCol
            // 
            this.ArticulCol.HeaderText = "Артикул";
            this.ArticulCol.Name = "ArticulCol";
            this.ArticulCol.ReadOnly = true;
            // 
            // TitleCol
            // 
            this.TitleCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TitleCol.HeaderText = "Название";
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            // 
            // MeasureUnitCol
            // 
            this.MeasureUnitCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.MeasureUnitCol.HeaderText = "Ед. изм.";
            this.MeasureUnitCol.MinimumWidth = 35;
            this.MeasureUnitCol.Name = "MeasureUnitCol";
            this.MeasureUnitCol.ReadOnly = true;
            this.MeasureUnitCol.Width = 35;
            // 
            // CountCol
            // 
            this.CountCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CountCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.Width = 50;
            // 
            // PriceCol
            // 
            this.PriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.PriceCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.PriceCol.HeaderText = "Цена (руб.)";
            this.PriceCol.MinimumWidth = 50;
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            this.PriceCol.Width = 75;
            // 
            // SumCol
            // 
            this.SumCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "C2";
            dataGridViewCellStyle6.NullValue = null;
            this.SumCol.DefaultCellStyle = dataGridViewCellStyle6;
            this.SumCol.HeaderText = "Сумма (руб.)";
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            // 
            // OperationsInfoDGV
            // 
            this.OperationsInfoDGV.AllowUserToAddRows = false;
            this.OperationsInfoDGV.AllowUserToDeleteRows = false;
            this.OperationsInfoDGV.AllowUserToResizeRows = false;
            this.OperationsInfoDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OperationsInfoDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OperationTypeCol,
            this.OperationIdCol,
            this.DateCol,
            this.EmployeeCol,
            this.ContragentCol,
            this.ContragentEmployeeCol,
            this.DescriptionCol,
            this.TotalSumCol});
            this.OperationsInfoDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsInfoDGV.Location = new System.Drawing.Point(3, 16);
            this.OperationsInfoDGV.MultiSelect = false;
            this.OperationsInfoDGV.Name = "OperationsInfoDGV";
            this.OperationsInfoDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OperationsInfoDGV.Size = new System.Drawing.Size(828, 243);
            this.OperationsInfoDGV.TabIndex = 10;
            // 
            // OperationTypeCol
            // 
            this.OperationTypeCol.HeaderText = "Тип операции";
            this.OperationTypeCol.Name = "OperationTypeCol";
            this.OperationTypeCol.ReadOnly = true;
            // 
            // OperationIdCol
            // 
            this.OperationIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OperationIdCol.HeaderText = "№ операции";
            this.OperationIdCol.Name = "OperationIdCol";
            this.OperationIdCol.ReadOnly = true;
            this.OperationIdCol.Width = 60;
            // 
            // DateCol
            // 
            this.DateCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Format = "dd.MM.yyyy \'г.\'   HH:mm";
            dataGridViewCellStyle1.NullValue = null;
            this.DateCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.DateCol.HeaderText = "Дата";
            this.DateCol.MinimumWidth = 80;
            this.DateCol.Name = "DateCol";
            this.DateCol.ReadOnly = true;
            this.DateCol.Width = 110;
            // 
            // EmployeeCol
            // 
            this.EmployeeCol.HeaderText = "Сотрудник";
            this.EmployeeCol.Name = "EmployeeCol";
            this.EmployeeCol.ReadOnly = true;
            // 
            // ContragentCol
            // 
            this.ContragentCol.HeaderText = "Контрагент";
            this.ContragentCol.Name = "ContragentCol";
            this.ContragentCol.ReadOnly = true;
            // 
            // ContragentEmployeeCol
            // 
            this.ContragentEmployeeCol.HeaderText = "Представитель контрагента";
            this.ContragentEmployeeCol.Name = "ContragentEmployeeCol";
            this.ContragentEmployeeCol.ReadOnly = true;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.HeaderText = "Комментарий";
            this.DescriptionCol.Name = "DescriptionCol";
            this.DescriptionCol.ReadOnly = true;
            // 
            // TotalSumCol
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.TotalSumCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.TotalSumCol.HeaderText = "Сумма (руб.)";
            this.TotalSumCol.MinimumWidth = 100;
            this.TotalSumCol.Name = "TotalSumCol";
            this.TotalSumCol.ReadOnly = true;
            // 
            // EmployeeOperationsInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 685);
            this.Controls.Add(this.splitContainer1);
            this.Name = "EmployeeOperationsInfoForm";
            this.Text = "Операции сотрудников";
            this.Load += new System.EventHandler(this.EmployeeOperationsInfoForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.EmployeeGroupBox.ResumeLayout(false);
            this.EmployeeGroupBox.PerformLayout();
            this.BottomSplitContainer.Panel1.ResumeLayout(false);
            this.BottomSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BottomSplitContainer)).EndInit();
            this.BottomSplitContainer.ResumeLayout(false);
            this.OperationsGroupBox.ResumeLayout(false);
            this.OperationsGroupBox.PerformLayout();
            this.OperationsStatusStrip.ResumeLayout(false);
            this.OperationsStatusStrip.PerformLayout();
            this.BeginDatePanel.ResumeLayout(false);
            this.BeginDatePanel.PerformLayout();
            this.EndDatePanel.ResumeLayout(false);
            this.EndDatePanel.PerformLayout();
            this.OperationDetailsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer BottomSplitContainer;
        private System.Windows.Forms.GroupBox OperationsGroupBox;
        private System.Windows.Forms.StatusStrip OperationsStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel OperationsCountStatusLabel;
        private System.Windows.Forms.CheckBox SaleCheckBox;
        private System.Windows.Forms.CheckBox PurchaseCheckBox;
        private System.Windows.Forms.DateTimePicker BeginDateDTP;
        private System.Windows.Forms.CheckBox BeginDateCheckBox;
        private System.Windows.Forms.Panel BeginDatePanel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel EndDatePanel;
        private System.Windows.Forms.CheckBox EndDateCheckBox;
        private System.Windows.Forms.DateTimePicker EndDateDTP;
        private System.Windows.Forms.GroupBox OperationDetailsGroupBox;
        private System.Windows.Forms.DataGridView OperationDetailsDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ManufacturerCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
        private System.Windows.Forms.GroupBox EmployeeGroupBox;
        private System.Windows.Forms.ListBox EmployeeListBox;
        private System.Windows.Forms.CheckBox ActivEmployeesCheckBox;
        private System.Windows.Forms.CheckBox InactiveEmployeesCheckBox;
        private System.Windows.Forms.DataGridView OperationsInfoDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSumCol;
    }
}