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
            this.operationIdTextBox = new System.Windows.Forms.TextBox();
            this.operationIdLabel = new System.Windows.Forms.Label();
            this.tablesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.operationSplitContainer = new System.Windows.Forms.SplitContainer();
            this.storageFilterComboBox = new System.Windows.Forms.ComboBox();
            this.currencyFilterComboBox = new System.Windows.Forms.ComboBox();
            this.operationDateFilterTimePicker = new System.Windows.Forms.DateTimePicker();
            this.employeeFilterTextBox = new System.Windows.Forms.TextBox();
            this.contragentFilterTextBox = new System.Windows.Forms.TextBox();
            this.operationDataGridView = new System.Windows.Forms.DataGridView();
            this.operationDetailsDGV = new System.Windows.Forms.DataGridView();
            this.Manufacturer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Articul = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Contragent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Employee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Currency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExcRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Storage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationDetailsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.operationIdTextBox);
            this.mainSplitContainer.Panel1.Controls.Add(this.operationIdLabel);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tablesSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(838, 742);
            this.mainSplitContainer.SplitterDistance = 286;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // operationIdTextBox
            // 
            this.operationIdTextBox.Location = new System.Drawing.Point(135, 43);
            this.operationIdTextBox.Name = "operationIdTextBox";
            this.operationIdTextBox.Size = new System.Drawing.Size(64, 20);
            this.operationIdTextBox.TabIndex = 1;
            // 
            // operationIdLabel
            // 
            this.operationIdLabel.AutoSize = true;
            this.operationIdLabel.Location = new System.Drawing.Point(58, 46);
            this.operationIdLabel.Name = "operationIdLabel";
            this.operationIdLabel.Size = new System.Drawing.Size(71, 13);
            this.operationIdLabel.TabIndex = 0;
            this.operationIdLabel.Text = "Операция №";
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
            this.tablesSplitContainer.Panel2.Controls.Add(this.operationDetailsDGV);
            this.tablesSplitContainer.Size = new System.Drawing.Size(838, 452);
            this.tablesSplitContainer.SplitterDistance = 243;
            this.tablesSplitContainer.TabIndex = 0;
            // 
            // operationSplitContainer
            // 
            this.operationSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.operationSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.operationSplitContainer.Name = "operationSplitContainer";
            this.operationSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // operationSplitContainer.Panel1
            // 
            this.operationSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.operationSplitContainer.Panel1.Controls.Add(this.storageFilterComboBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.currencyFilterComboBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.operationDateFilterTimePicker);
            this.operationSplitContainer.Panel1.Controls.Add(this.employeeFilterTextBox);
            this.operationSplitContainer.Panel1.Controls.Add(this.contragentFilterTextBox);
            // 
            // operationSplitContainer.Panel2
            // 
            this.operationSplitContainer.Panel2.Controls.Add(this.operationDataGridView);
            this.operationSplitContainer.Size = new System.Drawing.Size(838, 243);
            this.operationSplitContainer.SplitterDistance = 25;
            this.operationSplitContainer.SplitterWidth = 1;
            this.operationSplitContainer.TabIndex = 0;
            // 
            // storageFilterComboBox
            // 
            this.storageFilterComboBox.FormattingEnabled = true;
            this.storageFilterComboBox.Location = new System.Drawing.Point(716, 1);
            this.storageFilterComboBox.Name = "storageFilterComboBox";
            this.storageFilterComboBox.Size = new System.Drawing.Size(61, 21);
            this.storageFilterComboBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.storageFilterComboBox, "Выберите склад ");
            // 
            // currencyFilterComboBox
            // 
            this.currencyFilterComboBox.FormattingEnabled = true;
            this.currencyFilterComboBox.Location = new System.Drawing.Point(400, 2);
            this.currencyFilterComboBox.Name = "currencyFilterComboBox";
            this.currencyFilterComboBox.Size = new System.Drawing.Size(76, 21);
            this.currencyFilterComboBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.currencyFilterComboBox, "Выберите валюту в которой происходила операция");
            // 
            // operationDateFilterTimePicker
            // 
            this.operationDateFilterTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.operationDateFilterTimePicker.Location = new System.Drawing.Point(248, 2);
            this.operationDateFilterTimePicker.Name = "operationDateFilterTimePicker";
            this.operationDateFilterTimePicker.Size = new System.Drawing.Size(80, 20);
            this.operationDateFilterTimePicker.TabIndex = 2;
            this.toolTip.SetToolTip(this.operationDateFilterTimePicker, "Выберите дату операции");
            // 
            // employeeFilterTextBox
            // 
            this.employeeFilterTextBox.Location = new System.Drawing.Point(142, 5);
            this.employeeFilterTextBox.Name = "employeeFilterTextBox";
            this.employeeFilterTextBox.Size = new System.Drawing.Size(100, 20);
            this.employeeFilterTextBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.employeeFilterTextBox, "Введите фамилию имя сотрудника");
            // 
            // contragentFilterTextBox
            // 
            this.contragentFilterTextBox.Location = new System.Drawing.Point(36, 2);
            this.contragentFilterTextBox.Name = "contragentFilterTextBox";
            this.contragentFilterTextBox.Size = new System.Drawing.Size(100, 20);
            this.contragentFilterTextBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.contragentFilterTextBox, "Введите Имя/Название контрагента");
            // 
            // operationDataGridView
            // 
            this.operationDataGridView.AllowUserToAddRows = false;
            this.operationDataGridView.AllowUserToDeleteRows = false;
            this.operationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Contragent,
            this.Employee,
            this.Date,
            this.InTotal,
            this.Currency,
            this.ExcRate,
            this.ContragentEmployee,
            this.Storage,
            this.OperationId,
            this.Description});
            this.operationDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationDataGridView.Location = new System.Drawing.Point(0, 0);
            this.operationDataGridView.Name = "operationDataGridView";
            this.operationDataGridView.Size = new System.Drawing.Size(838, 217);
            this.operationDataGridView.TabIndex = 0;
            this.operationDataGridView.Resize += new System.EventHandler(this.operationDataGridView_Resize);
            // 
            // operationDetailsDGV
            // 
            this.operationDetailsDGV.AllowUserToAddRows = false;
            this.operationDetailsDGV.AllowUserToDeleteRows = false;
            this.operationDetailsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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
            this.operationDetailsDGV.Size = new System.Drawing.Size(838, 205);
            this.operationDetailsDGV.TabIndex = 1;
            // 
            // Manufacturer
            // 
            this.Manufacturer.HeaderText = "Производитель";
            this.Manufacturer.Name = "Manufacturer";
            // 
            // Articul
            // 
            this.Articul.HeaderText = "Артикул";
            this.Articul.Name = "Articul";
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
            // Contragent
            // 
            this.Contragent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Contragent.HeaderText = "Контрагент";
            this.Contragent.Name = "Contragent";
            this.Contragent.Width = 90;
            // 
            // Employee
            // 
            this.Employee.HeaderText = "Сотрудник";
            this.Employee.Name = "Employee";
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
            this.InTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.InTotal.HeaderText = "Сумма";
            this.InTotal.Name = "InTotal";
            this.InTotal.Width = 66;
            // 
            // Currency
            // 
            this.Currency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Currency.HeaderText = "Валюта";
            this.Currency.Name = "Currency";
            this.Currency.Width = 70;
            // 
            // ExcRate
            // 
            this.ExcRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ExcRate.HeaderText = "Курс";
            this.ExcRate.Name = "ExcRate";
            this.ExcRate.Width = 56;
            // 
            // ContragentEmployee
            // 
            this.ContragentEmployee.HeaderText = "Представитель контрагента";
            this.ContragentEmployee.Name = "ContragentEmployee";
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
            this.OperationId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.OperationId.HeaderText = "№ операции";
            this.OperationId.Name = "OperationId";
            this.OperationId.Width = 87;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Description.HeaderText = "Описание";
            this.Description.Name = "Description";
            this.Description.Width = 82;
            // 
            // OperationInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 742);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "OperationInfoForm";
            this.Text = "OperationInfoForm";
            this.Load += new System.EventHandler(this.OperationInfoForm_Load);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationDetailsDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer tablesSplitContainer;
        private System.Windows.Forms.DataGridView operationDataGridView;
        private System.Windows.Forms.DataGridView operationDetailsDGV;
        private System.Windows.Forms.TextBox operationIdTextBox;
        private System.Windows.Forms.Label operationIdLabel;
        private System.Windows.Forms.SplitContainer operationSplitContainer;
        private System.Windows.Forms.TextBox employeeFilterTextBox;
        private System.Windows.Forms.TextBox contragentFilterTextBox;
        private System.Windows.Forms.ComboBox storageFilterComboBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox currencyFilterComboBox;
        private System.Windows.Forms.DateTimePicker operationDateFilterTimePicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Manufacturer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Articul;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contragent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Employee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn InTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExcRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Storage;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}