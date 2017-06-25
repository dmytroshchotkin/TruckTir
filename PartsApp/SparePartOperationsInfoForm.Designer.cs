namespace PartsApp
{
    partial class SparePartOperationsInfoForm
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
            this.OperationsInfoDGV = new System.Windows.Forms.DataGridView();
            this.OperationTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperationIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContragentEmployeeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchaseCheckBox = new System.Windows.Forms.CheckBox();
            this.SaleCheckBox = new System.Windows.Forms.CheckBox();
            this.OperationInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.ArticulLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).BeginInit();
            this.OperationInfoGroupBox.SuspendLayout();
            this.SuspendLayout();
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
            this.UnitCol,
            this.CountCol,
            this.PriceCol,
            this.SumCol});
            this.OperationsInfoDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsInfoDGV.Location = new System.Drawing.Point(3, 16);
            this.OperationsInfoDGV.Name = "OperationsInfoDGV";
            this.OperationsInfoDGV.Size = new System.Drawing.Size(785, 255);
            this.OperationsInfoDGV.TabIndex = 0;
            // 
            // OperationTypeCol
            // 
            this.OperationTypeCol.HeaderText = "Тип операции";
            this.OperationTypeCol.MinimumWidth = 50;
            this.OperationTypeCol.Name = "OperationTypeCol";
            this.OperationTypeCol.ReadOnly = true;
            this.OperationTypeCol.Width = 60;
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
            this.DateCol.HeaderText = "Дата";
            this.DateCol.Name = "DateCol";
            this.DateCol.ReadOnly = true;
            this.DateCol.Width = 70;
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
            // UnitCol
            // 
            this.UnitCol.HeaderText = "Ед. изм.";
            this.UnitCol.MinimumWidth = 35;
            this.UnitCol.Name = "UnitCol";
            this.UnitCol.ReadOnly = true;
            this.UnitCol.Width = 35;
            // 
            // CountCol
            // 
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.Width = 50;
            // 
            // PriceCol
            // 
            this.PriceCol.HeaderText = "Цена (руб.)";
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            this.PriceCol.Width = 50;
            // 
            // SumCol
            // 
            this.SumCol.HeaderText = "Сумма";
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            // 
            // PurchaseCheckBox
            // 
            this.PurchaseCheckBox.AutoSize = true;
            this.PurchaseCheckBox.Checked = true;
            this.PurchaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PurchaseCheckBox.Location = new System.Drawing.Point(145, 0);
            this.PurchaseCheckBox.Name = "PurchaseCheckBox";
            this.PurchaseCheckBox.Size = new System.Drawing.Size(63, 17);
            this.PurchaseCheckBox.TabIndex = 1;
            this.PurchaseCheckBox.Text = "Приход";
            this.PurchaseCheckBox.UseVisualStyleBackColor = true;
            this.PurchaseCheckBox.CheckedChanged += new System.EventHandler(this.PurchaseCheckBox_CheckedChanged);
            // 
            // SaleCheckBox
            // 
            this.SaleCheckBox.AutoSize = true;
            this.SaleCheckBox.Checked = true;
            this.SaleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaleCheckBox.Location = new System.Drawing.Point(214, 0);
            this.SaleCheckBox.Name = "SaleCheckBox";
            this.SaleCheckBox.Size = new System.Drawing.Size(62, 17);
            this.SaleCheckBox.TabIndex = 2;
            this.SaleCheckBox.Text = "Расход";
            this.SaleCheckBox.UseVisualStyleBackColor = true;
            this.SaleCheckBox.CheckedChanged += new System.EventHandler(this.SaleCheckBox_CheckedChanged);
            // 
            // OperationInfoGroupBox
            // 
            this.OperationInfoGroupBox.Controls.Add(this.OperationsInfoDGV);
            this.OperationInfoGroupBox.Controls.Add(this.SaleCheckBox);
            this.OperationInfoGroupBox.Controls.Add(this.PurchaseCheckBox);
            this.OperationInfoGroupBox.Location = new System.Drawing.Point(12, 68);
            this.OperationInfoGroupBox.Name = "OperationInfoGroupBox";
            this.OperationInfoGroupBox.Size = new System.Drawing.Size(791, 274);
            this.OperationInfoGroupBox.TabIndex = 3;
            this.OperationInfoGroupBox.TabStop = false;
            // 
            // ArticulLabel
            // 
            this.ArticulLabel.AutoSize = true;
            this.ArticulLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ArticulLabel.Location = new System.Drawing.Point(193, 16);
            this.ArticulLabel.Name = "ArticulLabel";
            this.ArticulLabel.Size = new System.Drawing.Size(53, 20);
            this.ArticulLabel.TabIndex = 4;
            this.ArticulLabel.Text = "Articul";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleLabel.Location = new System.Drawing.Point(193, 39);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(34, 16);
            this.TitleLabel.TabIndex = 5;
            this.TitleLabel.Text = "Title";
            // 
            // SparePartOperationsInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 352);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.ArticulLabel);
            this.Controls.Add(this.OperationInfoGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SparePartOperationsInfoForm";
            this.Text = "Форма просмотра передвижения товара";            
            ((System.ComponentModel.ISupportInitialize)(this.OperationsInfoDGV)).EndInit();
            this.OperationInfoGroupBox.ResumeLayout(false);
            this.OperationInfoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView OperationsInfoDGV;
        private System.Windows.Forms.CheckBox PurchaseCheckBox;
        private System.Windows.Forms.CheckBox SaleCheckBox;
        private System.Windows.Forms.GroupBox OperationInfoGroupBox;
        private System.Windows.Forms.Label ArticulLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContragentEmployeeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
    }
}