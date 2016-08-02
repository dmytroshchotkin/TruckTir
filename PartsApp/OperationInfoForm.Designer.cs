namespace PartsApp
{
    partial class OperationInfoForm
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
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tablesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.operationSplitContainer = new System.Windows.Forms.SplitContainer();
            this.operationIdFilterTextBox = new System.Windows.Forms.TextBox();
            this.storageFilterComboBox = new System.Windows.Forms.ComboBox();
            this.operationDateFilterTimePicker = new System.Windows.Forms.DateTimePicker();
            this.employeeFilterTextBox = new System.Windows.Forms.TextBox();
            this.contragentFilterTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.operationDataGridView = new System.Windows.Forms.DataGridView();
            this.operationStatusStrip = new System.Windows.Forms.StatusStrip();
            this.operationToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationRowsCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationPrintToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.operationDetailsDGV = new System.Windows.Forms.DataGridView();
            this.Manufacturer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Articul = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operationDetailsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.operationDetailsToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationDetailsRowsCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationDetailsPrintToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Contragent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Employee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Storage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablesSplitContainer)).BeginInit();
            this.tablesSplitContainer.Panel1.SuspendLayout();
            this.tablesSplitContainer.Panel2.SuspendLayout();
            this.tablesSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationSplitContainer)).BeginInit();
            this.operationSplitContainer.Panel1.SuspendLayout();
            this.operationSplitContainer.Panel2.SuspendLayout();
            this.operationSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGridView)).BeginInit();
            this.operationStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationDetailsDGV)).BeginInit();
            this.operationDetailsStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tablesSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(839, 762);
            this.mainSplitContainer.SplitterDistance = 292;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // tablesSplitContainer
            // 
            this.tablesSplitContainer.BackColor = System.Drawing.Color.Red;
            this.tablesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.tablesSplitContainer.Name = "tablesSplitContainer";
            this.tablesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // tablesSplitContainer.Panel1
            // 
            this.tablesSplitContainer.Panel1.Controls.Add(this.operationSplitContainer);
            // 
            // tablesSplitContainer.Panel2
            // 
            this.tablesSplitContainer.Panel2.Controls.Add(this.splitContainer2);
            this.tablesSplitContainer.Size = new System.Drawing.Size(839, 466);
            this.tablesSplitContainer.SplitterDistance = 250;
            this.tablesSplitContainer.TabIndex = 0;
            // 
            // operationSplitContainer
            // 
            this.operationSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.operationSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationSplitContainer.IsSplitterFixed = true;
            this.operationSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.operationSplitContainer.Name = "operationSplitContainer";
            this.operationSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // operationSplitContainer.Panel1
            // 
            this.operationSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.operationSplitContainer.Panel1.Controls.Add(this.operationIdFilterTextBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.storageFilterComboBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.operationDateFilterTimePicker);
            this.operationSplitContainer.Panel1.Controls.Add(this.employeeFilterTextBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.contragentFilterTextBox);
            // 
            // operationSplitContainer.Panel2
            // 
            this.operationSplitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.operationSplitContainer.Size = new System.Drawing.Size(839, 250);
            this.operationSplitContainer.SplitterDistance = 25;
            this.operationSplitContainer.SplitterWidth = 1;
            this.operationSplitContainer.TabIndex = 0;
            // 
            // operationIdFilterTextBox
            // 
            this.operationIdFilterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.operationIdFilterTextBox.Location = new System.Drawing.Point(563, 3);
            this.operationIdFilterTextBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.operationIdFilterTextBox.Name = "operationIdFilterTextBox";
            this.operationIdFilterTextBox.Size = new System.Drawing.Size(64, 20);
            this.operationIdFilterTextBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.operationIdFilterTextBox, "Введите № операции.");
            this.operationIdFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.operationIdTextBox_KeyDown);
            // 
            // storageFilterComboBox
            // 
            this.storageFilterComboBox.FormattingEnabled = true;
            this.storageFilterComboBox.Location = new System.Drawing.Point(496, 3);
            this.storageFilterComboBox.Name = "storageFilterComboBox";
            this.storageFilterComboBox.Size = new System.Drawing.Size(61, 21);
            this.storageFilterComboBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.storageFilterComboBox, "Выберите склад ");
            // 
            // operationDateFilterTimePicker
            // 
            this.operationDateFilterTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.operationDateFilterTimePicker.Location = new System.Drawing.Point(224, 4);
            this.operationDateFilterTimePicker.MinimumSize = new System.Drawing.Size(4, 21);
            this.operationDateFilterTimePicker.Name = "operationDateFilterTimePicker";
            this.operationDateFilterTimePicker.Size = new System.Drawing.Size(80, 21);
            this.operationDateFilterTimePicker.TabIndex = 2;
            this.toolTip.SetToolTip(this.operationDateFilterTimePicker, "Выберите дату операции");
            // 
            // employeeFilterTextBox
            // 
            this.employeeFilterTextBox.Location = new System.Drawing.Point(133, 2);
            this.employeeFilterTextBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.employeeFilterTextBox.Name = "employeeFilterTextBox";
            this.employeeFilterTextBox.Size = new System.Drawing.Size(76, 20);
            this.employeeFilterTextBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.employeeFilterTextBox, "Введите фамилию имя сотрудника");
            this.employeeFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.employeeFilterTextBox_KeyDown);
            // 
            // contragentFilterTextBox
            // 
            this.contragentFilterTextBox.Location = new System.Drawing.Point(37, 2);
            this.contragentFilterTextBox.MinimumSize = new System.Drawing.Size(4, 21);
            this.contragentFilterTextBox.Name = "contragentFilterTextBox";
            this.contragentFilterTextBox.Size = new System.Drawing.Size(90, 20);
            this.contragentFilterTextBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.contragentFilterTextBox, "Введите Имя/Название контрагента");
            this.contragentFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contragentFilterTextBox_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.operationDataGridView);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.operationStatusStrip);
            this.splitContainer1.Panel2MinSize = 15;
            this.splitContainer1.Size = new System.Drawing.Size(839, 224);
            this.splitContainer1.SplitterDistance = 195;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // operationDataGridView
            // 
            this.operationDataGridView.AllowUserToAddRows = false;
            this.operationDataGridView.AllowUserToDeleteRows = false;
            this.operationDataGridView.AllowUserToResizeRows = false;
            this.operationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Contragent,
            this.Employee,
            this.Date,
            this.InTotal,
            this.ContragentEmployee,
            this.Storage,
            this.OperationId,
            this.Description});
            this.operationDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationDataGridView.Location = new System.Drawing.Point(0, 0);
            this.operationDataGridView.Name = "operationDataGridView";
            this.operationDataGridView.Size = new System.Drawing.Size(839, 195);
            this.operationDataGridView.TabIndex = 0;
            this.operationDataGridView.Resize += new System.EventHandler(this.operationDataGridView_Resize);
            // 
            // operationStatusStrip
            // 
            this.operationStatusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationToolStripStatusLabel,
            this.operationRowsCountLabel,
            this.operationPrintToolStripButton});
            this.operationStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.operationStatusStrip.Name = "operationStatusStrip";
            this.operationStatusStrip.Size = new System.Drawing.Size(839, 28);
            this.operationStatusStrip.TabIndex = 0;
            this.operationStatusStrip.Text = "statusStrip1";
            // 
            // operationToolStripStatusLabel
            // 
            this.operationToolStripStatusLabel.Name = "operationToolStripStatusLabel";
            this.operationToolStripStatusLabel.Size = new System.Drawing.Size(91, 23);
            this.operationToolStripStatusLabel.Text = "Всего записей :";
            // 
            // operationRowsCountLabel
            // 
            this.operationRowsCountLabel.Name = "operationRowsCountLabel";
            this.operationRowsCountLabel.Size = new System.Drawing.Size(13, 23);
            this.operationRowsCountLabel.Text = "0";
            // 
            // operationPrintToolStripButton
            // 
            this.operationPrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.operationPrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.operationPrintToolStripButton.Margin = new System.Windows.Forms.Padding(450, 2, 0, 0);
            this.operationPrintToolStripButton.Name = "operationPrintToolStripButton";
            this.operationPrintToolStripButton.Size = new System.Drawing.Size(78, 26);
            this.operationPrintToolStripButton.Text = "Распечатать";
            this.operationPrintToolStripButton.ToolTipText = "Распечатать данные в Excel файл";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.operationDetailsDGV);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.operationDetailsStatusStrip);
            this.splitContainer2.Panel2MinSize = 15;
            this.splitContainer2.Size = new System.Drawing.Size(839, 212);
            this.splitContainer2.SplitterDistance = 183;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 2;
            // 
            // operationDetailsDGV
            // 
            this.operationDetailsDGV.AllowUserToAddRows = false;
            this.operationDetailsDGV.AllowUserToDeleteRows = false;
            this.operationDetailsDGV.AllowUserToResizeRows = false;
            this.operationDetailsDGV.ColumnHeadersHeight = 25;
            this.operationDetailsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.operationDetailsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Manufacturer,
            this.Articul,
            this.Title,
            this.Unit,
            this.Count,
            this.Price,
            this.Sum});
            this.operationDetailsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationDetailsDGV.Location = new System.Drawing.Point(0, 0);
            this.operationDetailsDGV.Name = "operationDetailsDGV";
            this.operationDetailsDGV.Size = new System.Drawing.Size(839, 183);
            this.operationDetailsDGV.TabIndex = 1;
            // 
            // Manufacturer
            // 
            this.Manufacturer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Manufacturer.HeaderText = "Производитель";
            this.Manufacturer.Name = "Manufacturer";
            this.Manufacturer.Width = 111;
            // 
            // Articul
            // 
            this.Articul.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Articul.HeaderText = "Артикул";
            this.Articul.MinimumWidth = 100;
            this.Articul.Name = "Articul";
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.HeaderText = "Название";
            this.Title.MinimumWidth = 100;
            this.Title.Name = "Title";
            // 
            // Unit
            // 
            this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Unit.HeaderText = "Ед. изм.";
            this.Unit.Name = "Unit";
            this.Unit.Width = 74;
            // 
            // Count
            // 
            this.Count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Count.HeaderText = "Кол-во";
            this.Count.Name = "Count";
            this.Count.Width = 66;
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Price.HeaderText = "Цена";
            this.Price.Name = "Price";
            this.Price.Width = 58;
            // 
            // Sum
            // 
            this.Sum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Sum.HeaderText = "Сумма";
            this.Sum.Name = "Sum";
            this.Sum.Width = 66;
            // 
            // operationDetailsStatusStrip
            // 
            this.operationDetailsStatusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationDetailsStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationDetailsToolStripStatusLabel,
            this.operationDetailsRowsCountLabel,
            this.operationDetailsPrintToolStripButton});
            this.operationDetailsStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.operationDetailsStatusStrip.Name = "operationDetailsStatusStrip";
            this.operationDetailsStatusStrip.Size = new System.Drawing.Size(839, 28);
            this.operationDetailsStatusStrip.TabIndex = 1;
            this.operationDetailsStatusStrip.Text = "statusStrip1";
            // 
            // operationDetailsToolStripStatusLabel
            // 
            this.operationDetailsToolStripStatusLabel.BackColor = System.Drawing.SystemColors.Control;
            this.operationDetailsToolStripStatusLabel.Name = "operationDetailsToolStripStatusLabel";
            this.operationDetailsToolStripStatusLabel.Size = new System.Drawing.Size(91, 23);
            this.operationDetailsToolStripStatusLabel.Text = "Всего записей :";
            // 
            // operationDetailsRowsCountLabel
            // 
            this.operationDetailsRowsCountLabel.BackColor = System.Drawing.SystemColors.Control;
            this.operationDetailsRowsCountLabel.Name = "operationDetailsRowsCountLabel";
            this.operationDetailsRowsCountLabel.Size = new System.Drawing.Size(13, 23);
            this.operationDetailsRowsCountLabel.Text = "0";
            // 
            // operationDetailsPrintToolStripButton
            // 
            this.operationDetailsPrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.operationDetailsPrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.operationDetailsPrintToolStripButton.Margin = new System.Windows.Forms.Padding(450, 2, 0, 0);
            this.operationDetailsPrintToolStripButton.Name = "operationDetailsPrintToolStripButton";
            this.operationDetailsPrintToolStripButton.Size = new System.Drawing.Size(78, 26);
            this.operationDetailsPrintToolStripButton.Text = "Распечатать";
            this.operationDetailsPrintToolStripButton.ToolTipText = "Распечатать данные в Excel файл";
            // 
            // Contragent
            // 
            this.Contragent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Contragent.HeaderText = "Контрагент";
            this.Contragent.Name = "Contragent";
            this.Contragent.Width = 90;
            // 
            // Employee
            // 
            this.Employee.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Employee.HeaderText = "Сотрудник";
            this.Employee.Name = "Employee";
            this.Employee.Width = 85;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Date.HeaderText = "Дата";
            this.Date.MinimumWidth = 80;
            this.Date.Name = "Date";
            this.Date.Width = 80;
            // 
            // InTotal
            // 
            this.InTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.InTotal.HeaderText = "Сумма";
            this.InTotal.MinimumWidth = 50;
            this.InTotal.Name = "InTotal";
            this.InTotal.Width = 50;
            // 
            // ContragentEmployee
            // 
            this.ContragentEmployee.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ContragentEmployee.HeaderText = "Предст-ль контр-та";
            this.ContragentEmployee.MinimumWidth = 62;
            this.ContragentEmployee.Name = "ContragentEmployee";
            this.ContragentEmployee.ToolTipText = "Представитель контрагента";
            this.ContragentEmployee.Width = 62;
            // 
            // Storage
            // 
            this.Storage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Storage.HeaderText = "Склад";
            this.Storage.Name = "Storage";
            this.Storage.Width = 63;
            // 
            // OperationId
            // 
            this.OperationId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.OperationId.HeaderText = "№ операции";
            this.OperationId.MinimumWidth = 60;
            this.OperationId.Name = "OperationId";
            this.OperationId.Width = 60;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Описание";
            this.Description.Name = "Description";
            // 
            // OperationInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 762);
            this.Controls.Add(this.mainSplitContainer);
            this.MinimumSize = new System.Drawing.Size(855, 800);
            this.Name = "OperationInfoForm";
            this.Text = "OperationInfoForm";
            this.Load += new System.EventHandler(this.OperationInfoForm_Load);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tablesSplitContainer.Panel1.ResumeLayout(false);
            this.tablesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablesSplitContainer)).EndInit();
            this.tablesSplitContainer.ResumeLayout(false);
            this.operationSplitContainer.Panel1.ResumeLayout(false);
            this.operationSplitContainer.Panel1.PerformLayout();
            this.operationSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.operationSplitContainer)).EndInit();
            this.operationSplitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGridView)).EndInit();
            this.operationStatusStrip.ResumeLayout(false);
            this.operationStatusStrip.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.operationDetailsDGV)).EndInit();
            this.operationDetailsStatusStrip.ResumeLayout(false);
            this.operationDetailsStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer tablesSplitContainer;
        private System.Windows.Forms.DataGridView operationDataGridView;
        private System.Windows.Forms.DataGridView operationDetailsDGV;
        private System.Windows.Forms.TextBox operationIdFilterTextBox;
        private System.Windows.Forms.SplitContainer operationSplitContainer;
        private System.Windows.Forms.TextBox employeeFilterTextBox;
        private System.Windows.Forms.TextBox contragentFilterTextBox;
        private System.Windows.Forms.ComboBox storageFilterComboBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DateTimePicker operationDateFilterTimePicker;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip operationStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel operationToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel operationRowsCountLabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.StatusStrip operationDetailsStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel operationDetailsToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel operationDetailsRowsCountLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Manufacturer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Articul;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sum;
        private System.Windows.Forms.ToolStripButton operationPrintToolStripButton;
        private System.Windows.Forms.ToolStripButton operationDetailsPrintToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contragent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Employee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn InTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Storage;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}