namespace PartsApp
{
    partial class OperationsInfoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OperationsGroupBox = new System.Windows.Forms.GroupBox();
            this.OperationsInfoDGV = new System.Windows.Forms.DataGridView();
            this.OperationTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OperationsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.OperationsCountStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.OperationsCoubtLabel = new System.Windows.Forms.ToolStripStatusLabel();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.OperationsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).BeginInit();
            this.OperationsStatusStrip.SuspendLayout();
            this.BeginDatePanel.SuspendLayout();
            this.EndDatePanel.SuspendLayout();
            this.OperationDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.LightCoral;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.OperationsGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OperationDetailsGroupBox);
            this.splitContainer1.Size = new System.Drawing.Size(864, 619);
            this.splitContainer1.SplitterDistance = 403;
            this.splitContainer1.TabIndex = 0;
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
            this.OperationsGroupBox.Size = new System.Drawing.Size(864, 403);
            this.OperationsGroupBox.TabIndex = 1;
            this.OperationsGroupBox.TabStop = false;
            this.OperationsGroupBox.Text = "Операции";
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
            this.OperationsInfoDGV.Size = new System.Drawing.Size(858, 366);
            this.OperationsInfoDGV.TabIndex = 10;
            this.OperationsInfoDGV.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OperationsInfoDGV_RowEnter);
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
            dataGridViewCellStyle7.Format = "dd.MM.yyyy \'г.\'   HH:mm";
            dataGridViewCellStyle7.NullValue = null;
            this.DateCol.DefaultCellStyle = dataGridViewCellStyle7;
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
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "C2";
            dataGridViewCellStyle8.NullValue = null;
            this.TotalSumCol.DefaultCellStyle = dataGridViewCellStyle8;
            this.TotalSumCol.HeaderText = "Сумма (руб.)";
            this.TotalSumCol.MinimumWidth = 100;
            this.TotalSumCol.Name = "TotalSumCol";
            this.TotalSumCol.ReadOnly = true;
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
            this.OperationsCountStatusLabel,
            this.OperationsCoubtLabel});
            this.OperationsStatusStrip.Location = new System.Drawing.Point(3, 382);
            this.OperationsStatusStrip.Name = "OperationsStatusStrip";
            this.OperationsStatusStrip.Size = new System.Drawing.Size(858, 18);
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
            // OperationsCoubtLabel
            // 
            this.OperationsCoubtLabel.Name = "OperationsCoubtLabel";
            this.OperationsCoubtLabel.Size = new System.Drawing.Size(13, 13);
            this.OperationsCoubtLabel.Text = "0";
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
            this.PurchaseCheckBox.CheckedChanged += new System.EventHandler(this.OperationsCheckBox_CheckedChanged);
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
            this.SaleCheckBox.CheckedChanged += new System.EventHandler(this.OperationsCheckBox_CheckedChanged);
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
            this.toolTip1.SetToolTip(this.BeginDateCheckBox, "Если панель выбора даты отключена, значит ограничения по нижней дате не установле" +
        "но.");
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
            this.toolTip1.SetToolTip(this.EndDateCheckBox, "Если панель выбора даты отключена, значит ограничения по верхней дате не установл" +
        "ено.");
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
            this.toolTip1.SetToolTip(this.EndDateDTP, "Конечная дата");
            this.EndDateDTP.Value = new System.DateTime(2017, 9, 21, 0, 0, 0, 0);
            this.EndDateDTP.ValueChanged += new System.EventHandler(this.DatesDTP_ValueChanged);
            // 
            // OperationDetailsGroupBox
            // 
            this.OperationDetailsGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.OperationDetailsGroupBox.Controls.Add(this.OperationDetailsDGV);
            this.OperationDetailsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationDetailsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OperationDetailsGroupBox.Name = "OperationDetailsGroupBox";
            this.OperationDetailsGroupBox.Size = new System.Drawing.Size(864, 212);
            this.OperationDetailsGroupBox.TabIndex = 3;
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
            this.OperationDetailsDGV.Size = new System.Drawing.Size(858, 193);
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle9;
            this.MeasureUnitCol.HeaderText = "Ед. изм.";
            this.MeasureUnitCol.MinimumWidth = 35;
            this.MeasureUnitCol.Name = "MeasureUnitCol";
            this.MeasureUnitCol.ReadOnly = true;
            this.MeasureUnitCol.Width = 35;
            // 
            // CountCol
            // 
            this.CountCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CountCol.DefaultCellStyle = dataGridViewCellStyle10;
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.Width = 50;
            // 
            // PriceCol
            // 
            this.PriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            this.PriceCol.DefaultCellStyle = dataGridViewCellStyle11;
            this.PriceCol.HeaderText = "Цена (руб.)";
            this.PriceCol.MinimumWidth = 50;
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            this.PriceCol.Width = 75;
            // 
            // SumCol
            // 
            this.SumCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "C2";
            dataGridViewCellStyle12.NullValue = null;
            this.SumCol.DefaultCellStyle = dataGridViewCellStyle12;
            this.SumCol.HeaderText = "Сумма (руб.)";
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            // 
            // OperationsInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 619);
            this.Controls.Add(this.splitContainer1);
            this.Name = "OperationsInfoForm";
            this.Text = "Форма операций";
            this.Load += new System.EventHandler(this.OperationsInfoForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.OperationsGroupBox.ResumeLayout(false);
            this.OperationsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).EndInit();
            this.OperationsStatusStrip.ResumeLayout(false);
            this.OperationsStatusStrip.PerformLayout();
            this.BeginDatePanel.ResumeLayout(false);
            this.BeginDatePanel.PerformLayout();
            this.EndDatePanel.ResumeLayout(false);
            this.EndDatePanel.PerformLayout();
            this.OperationDetailsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox OperationsGroupBox;
        private System.Windows.Forms.DataGridView OperationsInfoDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSumCol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip OperationsStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel OperationsCountStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel OperationsCoubtLabel;
        private System.Windows.Forms.CheckBox PurchaseCheckBox;
        private System.Windows.Forms.CheckBox SaleCheckBox;
        private System.Windows.Forms.Panel BeginDatePanel;
        private System.Windows.Forms.CheckBox BeginDateCheckBox;
        private System.Windows.Forms.DateTimePicker BeginDateDTP;
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
        private System.Windows.Forms.ToolTip toolTip1;
    }
}