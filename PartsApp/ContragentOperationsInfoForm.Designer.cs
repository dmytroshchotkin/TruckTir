namespace PartsApp
{
    partial class ContragentOperationsInfoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ContragentsGroupBox = new System.Windows.Forms.GroupBox();
            this.ContragentsListView = new System.Windows.Forms.ListView();
            this.ContragentCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BalanceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DisabledContragentsCheckBox = new System.Windows.Forms.CheckBox();
            this.EnabledContragentsCheckBox = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.OperationsGroupBox = new System.Windows.Forms.GroupBox();
            this.OperationsInfoDGV = new System.Windows.Forms.DataGridView();
            this.OperationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.OperationDetailsDGV = new System.Windows.Forms.DataGridView();
            this.ManufacturerCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaidCashCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.editContragentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editContragentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableContragentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableContragentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editOperDescriptContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editOperDescriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ContragentsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.OperationsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).BeginInit();
            this.OperationDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).BeginInit();
            this.editContragentContextMenuStrip.SuspendLayout();
            this.editOperDescriptContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ContragentsGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(985, 843);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // ContragentsGroupBox
            // 
            this.ContragentsGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.ContragentsGroupBox.Controls.Add(this.ContragentsListView);
            this.ContragentsGroupBox.Controls.Add(this.DisabledContragentsCheckBox);
            this.ContragentsGroupBox.Controls.Add(this.EnabledContragentsCheckBox);
            this.ContragentsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContragentsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ContragentsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ContragentsGroupBox.Name = "ContragentsGroupBox";
            this.ContragentsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ContragentsGroupBox.Size = new System.Drawing.Size(985, 354);
            this.ContragentsGroupBox.TabIndex = 1;
            this.ContragentsGroupBox.TabStop = false;
            this.ContragentsGroupBox.Text = "Контрагенты";
            // 
            // ContragentsListView
            // 
            this.ContragentsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ContragentCol,
            this.BalanceCol});
            this.ContragentsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContragentsListView.FullRowSelect = true;
            this.ContragentsListView.GridLines = true;
            this.ContragentsListView.HideSelection = false;
            this.ContragentsListView.Location = new System.Drawing.Point(4, 19);
            this.ContragentsListView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ContragentsListView.MultiSelect = false;
            this.ContragentsListView.Name = "ContragentsListView";
            this.ContragentsListView.Size = new System.Drawing.Size(977, 331);
            this.ContragentsListView.TabIndex = 1;
            this.ContragentsListView.UseCompatibleStateImageBehavior = false;
            this.ContragentsListView.View = System.Windows.Forms.View.Details;
            this.ContragentsListView.SelectedIndexChanged += new System.EventHandler(this.ContragentsListBox_SelectedIndexChanged);
            this.ContragentsListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContragentsListBox_MouseDown);
            // 
            // ContragentCol
            // 
            this.ContragentCol.Text = "Контрагент";
            this.ContragentCol.Width = 600;
            // 
            // BalanceCol
            // 
            this.BalanceCol.Text = "Баланс";
            this.BalanceCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.BalanceCol.Width = 99;
            // 
            // DisabledContragentsCheckBox
            // 
            this.DisabledContragentsCheckBox.AutoSize = true;
            this.DisabledContragentsCheckBox.Location = new System.Drawing.Point(245, 0);
            this.DisabledContragentsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DisabledContragentsCheckBox.Name = "DisabledContragentsCheckBox";
            this.DisabledContragentsCheckBox.Size = new System.Drawing.Size(110, 20);
            this.DisabledContragentsCheckBox.TabIndex = 3;
            this.DisabledContragentsCheckBox.Text = "Неактивные";
            this.DisabledContragentsCheckBox.UseVisualStyleBackColor = true;
            this.DisabledContragentsCheckBox.CheckedChanged += new System.EventHandler(this.OnContragentsCheckBoxesCheckedChanged);
            // 
            // EnabledContragentsCheckBox
            // 
            this.EnabledContragentsCheckBox.AutoSize = true;
            this.EnabledContragentsCheckBox.Location = new System.Drawing.Point(133, 0);
            this.EnabledContragentsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnabledContragentsCheckBox.Name = "EnabledContragentsCheckBox";
            this.EnabledContragentsCheckBox.Size = new System.Drawing.Size(93, 20);
            this.EnabledContragentsCheckBox.TabIndex = 2;
            this.EnabledContragentsCheckBox.Text = "Активные";
            this.EnabledContragentsCheckBox.UseVisualStyleBackColor = true;
            this.EnabledContragentsCheckBox.CheckedChanged += new System.EventHandler(this.OnContragentsCheckBoxesCheckedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.OperationsGroupBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.OperationDetailsGroupBox);
            this.splitContainer2.Size = new System.Drawing.Size(985, 484);
            this.splitContainer2.SplitterDistance = 251;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 1;
            // 
            // OperationsGroupBox
            // 
            this.OperationsGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.OperationsGroupBox.Controls.Add(this.OperationsInfoDGV);
            this.OperationsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OperationsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationsGroupBox.Name = "OperationsGroupBox";
            this.OperationsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationsGroupBox.Size = new System.Drawing.Size(985, 251);
            this.OperationsGroupBox.TabIndex = 0;
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
            this.OperationIdCol,
            this.DateCol,
            this.EmployeeCol,
            this.ContragentEmployeeCol,
            this.DescriptionCol,
            this.TotalSumCol});
            this.OperationsInfoDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsInfoDGV.Location = new System.Drawing.Point(4, 19);
            this.OperationsInfoDGV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationsInfoDGV.MultiSelect = false;
            this.OperationsInfoDGV.Name = "OperationsInfoDGV";
            this.OperationsInfoDGV.RowHeadersWidth = 51;
            this.OperationsInfoDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OperationsInfoDGV.Size = new System.Drawing.Size(977, 228);
            this.OperationsInfoDGV.TabIndex = 1;
            this.OperationsInfoDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.OperationsInfoDGV_CellEndEdit);
            this.OperationsInfoDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OperationsInfoDGV_CellMouseClick);
            this.OperationsInfoDGV.SelectionChanged += new System.EventHandler(this.OperationsInfoDGV_SelectionChanged);
            // 
            // OperationIdCol
            // 
            this.OperationIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OperationIdCol.HeaderText = "№ операции";
            this.OperationIdCol.MinimumWidth = 6;
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
            this.EmployeeCol.MinimumWidth = 6;
            this.EmployeeCol.Name = "EmployeeCol";
            this.EmployeeCol.ReadOnly = true;
            this.EmployeeCol.Width = 125;
            // 
            // ContragentEmployeeCol
            // 
            this.ContragentEmployeeCol.HeaderText = "Представитель контрагента";
            this.ContragentEmployeeCol.MinimumWidth = 6;
            this.ContragentEmployeeCol.Name = "ContragentEmployeeCol";
            this.ContragentEmployeeCol.ReadOnly = true;
            this.ContragentEmployeeCol.Width = 125;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.HeaderText = "Комментарий";
            this.DescriptionCol.MinimumWidth = 6;
            this.DescriptionCol.Name = "DescriptionCol";
            this.DescriptionCol.ReadOnly = true;
            this.DescriptionCol.Width = 125;
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
            this.TotalSumCol.Width = 125;
            //
            //  PaidCashCol
            //
            PaidCashCol.HeaderText = "Тип платежа";
            PaidCashCol.MinimumWidth = 80;
            PaidCashCol.Name = "PaidCashCol";
            PaidCashCol.ReadOnly = true;
            PaidCashCol.Width = 100;
            // 
            // OperationDetailsGroupBox
            // 
            this.OperationDetailsGroupBox.Controls.Add(this.OperationDetailsDGV);
            this.OperationDetailsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationDetailsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.OperationDetailsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationDetailsGroupBox.Name = "OperationDetailsGroupBox";
            this.OperationDetailsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationDetailsGroupBox.Size = new System.Drawing.Size(985, 231);
            this.OperationDetailsGroupBox.TabIndex = 1;
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
            this.OperationDetailsDGV.Location = new System.Drawing.Point(4, 19);
            this.OperationDetailsDGV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OperationDetailsDGV.Name = "OperationDetailsDGV";
            this.OperationDetailsDGV.RowHeadersWidth = 51;
            this.OperationDetailsDGV.Size = new System.Drawing.Size(977, 208);
            this.OperationDetailsDGV.TabIndex = 0;
            // 
            // ManufacturerCol
            // 
            this.ManufacturerCol.HeaderText = "Производитель";
            this.ManufacturerCol.MinimumWidth = 6;
            this.ManufacturerCol.Name = "ManufacturerCol";
            this.ManufacturerCol.ReadOnly = true;
            this.ManufacturerCol.Width = 125;
            // 
            // ArticulCol
            // 
            this.ArticulCol.HeaderText = "Артикул";
            this.ArticulCol.MinimumWidth = 6;
            this.ArticulCol.Name = "ArticulCol";
            this.ArticulCol.ReadOnly = true;
            this.ArticulCol.Width = 125;
            // 
            // TitleCol
            // 
            this.TitleCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TitleCol.HeaderText = "Название";
            this.TitleCol.MinimumWidth = 6;
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
            this.CountCol.MinimumWidth = 6;
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
            this.SumCol.MinimumWidth = 6;
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            this.SumCol.Width = 125;
            // 
            // editContragentContextMenuStrip
            // 
            this.editContragentContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.editContragentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editContragentToolStripMenuItem,
            this.disableContragentToolStripMenuItem,
            this.enableContragentToolStripMenuItem});
            this.editContragentContextMenuStrip.Name = "editContragentContextMenuStrip";
            this.editContragentContextMenuStrip.Size = new System.Drawing.Size(192, 76);
            // 
            // editContragentToolStripMenuItem
            // 
            this.editContragentToolStripMenuItem.Name = "editContragentToolStripMenuItem";
            this.editContragentToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.editContragentToolStripMenuItem.Text = "Редактировать";
            this.editContragentToolStripMenuItem.Click += new System.EventHandler(this.EditContragentToolStripMenuItem_Click);
            // 
            // disableContragentToolStripMenuItem
            // 
            this.disableContragentToolStripMenuItem.Name = "disableContragentToolStripMenuItem";
            this.disableContragentToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.disableContragentToolStripMenuItem.Text = "Заблокировать";
            this.disableContragentToolStripMenuItem.Click += new System.EventHandler(this.OnDisableOrEnableContragentToolStripMenuItemClick);
            // 
            // enableContragentToolStripMenuItem
            // 
            this.enableContragentToolStripMenuItem.Name = "enableContragentToolStripMenuItem";
            this.enableContragentToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.enableContragentToolStripMenuItem.Text = "Разблокировать";
            this.enableContragentToolStripMenuItem.Click += new System.EventHandler(this.OnDisableOrEnableContragentToolStripMenuItemClick);
            // 
            // editOperDescriptContextMenuStrip
            // 
            this.editOperDescriptContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.editOperDescriptContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editOperDescriptToolStripMenuItem});
            this.editOperDescriptContextMenuStrip.Name = "editOperDescriptContextMenuStrip";
            this.editOperDescriptContextMenuStrip.Size = new System.Drawing.Size(281, 28);
            // 
            // editOperDescriptToolStripMenuItem
            // 
            this.editOperDescriptToolStripMenuItem.Name = "editOperDescriptToolStripMenuItem";
            this.editOperDescriptToolStripMenuItem.Size = new System.Drawing.Size(280, 24);
            this.editOperDescriptToolStripMenuItem.Text = "Редактировать комментарий";
            this.editOperDescriptToolStripMenuItem.Click += new System.EventHandler(this.editOperDescriptToolStripMenuItem_Click);
            // 
            // ContragentOperationsInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(985, 843);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ContragentOperationsInfoForm";
            this.Text = "Операции контрагентов";
            this.Load += new System.EventHandler(this.ContragentOperationsInfoForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ContragentsGroupBox.ResumeLayout(false);
            this.ContragentsGroupBox.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.OperationsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).EndInit();
            this.OperationDetailsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationDetailsDGV)).EndInit();
            this.editContragentContextMenuStrip.ResumeLayout(false);
            this.editOperDescriptContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox ContragentsGroupBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox OperationsGroupBox;
        private System.Windows.Forms.DataGridView OperationsInfoDGV;
        private System.Windows.Forms.DataGridView OperationDetailsDGV;
        private System.Windows.Forms.GroupBox OperationDetailsGroupBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ManufacturerCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
        private System.Windows.Forms.ContextMenuStrip editContragentContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editContragentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableContragentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableContragentToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaidCashCol;
        private System.Windows.Forms.ContextMenuStrip editOperDescriptContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editOperDescriptToolStripMenuItem;
        private System.Windows.Forms.ListView ContragentsListView;
        private System.Windows.Forms.ColumnHeader ContragentCol;
        private System.Windows.Forms.ColumnHeader BalanceCol;
        private System.Windows.Forms.CheckBox EnabledContragentsCheckBox;
        private System.Windows.Forms.CheckBox DisabledContragentsCheckBox;
    }
}