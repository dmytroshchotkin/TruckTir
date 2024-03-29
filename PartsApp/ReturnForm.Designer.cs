﻿namespace PartsApp
{
    partial class ReturnForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.operationIdLabel = new System.Windows.Forms.Label();
            this.purchaseDateLabel = new System.Windows.Forms.Label();
            this.AgentLabel = new System.Windows.Forms.Label();
            this.ContragentLabel = new System.Windows.Forms.Label();
            this.ReturnGroupBox = new System.Windows.Forms.GroupBox();
            this.ReturnDGV = new System.Windows.Forms.DataGridView();
            this.SparePartIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inTotalLabel = new System.Windows.Forms.Label();
            this.ContragentEmployeeLabel = new System.Windows.Forms.Label();
            this.AgentEmployeerLabel = new System.Windows.Forms.Label();
            this.operationIdTextBox = new System.Windows.Forms.TextBox();
            this.OperationDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.AgentBackPanel = new System.Windows.Forms.Panel();
            this.AgentTextBox = new System.Windows.Forms.TextBox();
            this.ContragentBackPanel = new System.Windows.Forms.Panel();
            this.ContragentTextBox = new System.Windows.Forms.TextBox();
            this.ContragentStarLabel = new System.Windows.Forms.Label();
            this.inTotalNumberLabel = new System.Windows.Forms.Label();
            this.ContragentEmployeeTextBox = new System.Windows.Forms.TextBox();
            this.AgentEmployeerTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.purchaseContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noteRichTextBox = new System.Windows.Forms.RichTextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.AgentStarLabel = new System.Windows.Forms.Label();
            this.ReturnGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReturnDGV)).BeginInit();
            this.AgentBackPanel.SuspendLayout();
            this.ContragentBackPanel.SuspendLayout();
            this.purchaseContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // operationIdLabel
            // 
            this.operationIdLabel.AutoSize = true;
            this.operationIdLabel.Location = new System.Drawing.Point(179, 15);
            this.operationIdLabel.Name = "operationIdLabel";
            this.operationIdLabel.Size = new System.Drawing.Size(135, 13);
            this.operationIdLabel.TabIndex = 0;
            this.operationIdLabel.Text = "Возврат по накладной №";
            // 
            // purchaseDateLabel
            // 
            this.purchaseDateLabel.AutoSize = true;
            this.purchaseDateLabel.Location = new System.Drawing.Point(450, 15);
            this.purchaseDateLabel.Name = "purchaseDateLabel";
            this.purchaseDateLabel.Size = new System.Drawing.Size(24, 13);
            this.purchaseDateLabel.TabIndex = 1;
            this.purchaseDateLabel.Text = "от :";
            // 
            // AgentLabel
            // 
            this.AgentLabel.AutoSize = true;
            this.AgentLabel.Location = new System.Drawing.Point(23, 64);
            this.AgentLabel.Name = "AgentLabel";
            this.AgentLabel.Size = new System.Drawing.Size(72, 13);
            this.AgentLabel.TabIndex = 2;
            this.AgentLabel.Text = "Получатель :";
            // 
            // ContragentLabel
            // 
            this.ContragentLabel.AutoSize = true;
            this.ContragentLabel.Location = new System.Drawing.Point(23, 109);
            this.ContragentLabel.Name = "ContragentLabel";
            this.ContragentLabel.Size = new System.Drawing.Size(49, 13);
            this.ContragentLabel.TabIndex = 3;
            this.ContragentLabel.Text = "Клиент :";
            // 
            // ReturnGroupBox
            // 
            this.ReturnGroupBox.Controls.Add(this.ReturnDGV);
            this.ReturnGroupBox.Location = new System.Drawing.Point(15, 155);
            this.ReturnGroupBox.Name = "ReturnGroupBox";
            this.ReturnGroupBox.Size = new System.Drawing.Size(854, 311);
            this.ReturnGroupBox.TabIndex = 6;
            this.ReturnGroupBox.TabStop = false;
            this.ReturnGroupBox.Text = "Лист возврата.";
            // 
            // ReturnDGV
            // 
            this.ReturnDGV.AllowUserToAddRows = false;
            this.ReturnDGV.AllowUserToDeleteRows = false;
            this.ReturnDGV.AllowUserToResizeRows = false;
            this.ReturnDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ReturnDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReturnDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SparePartIdCol,
            this.ArticulCol,
            this.TitleCol,
            this.MeasureUnitCol,
            this.CountCol,
            this.PriceCol,
            this.SumCol});
            this.ReturnDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReturnDGV.Location = new System.Drawing.Point(3, 16);
            this.ReturnDGV.Name = "ReturnDGV";
            this.ReturnDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ReturnDGV.Size = new System.Drawing.Size(848, 292);
            this.ReturnDGV.TabIndex = 0;
            this.ReturnDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.ReturnDGV_CellBeginEdit);
            this.ReturnDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ReturnDGV_CellEndEdit);
            this.ReturnDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ReturnDGV_CellFormatting);
            this.ReturnDGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ReturnDGV_DataError);
            // 
            // SparePartIdCol
            // 
            this.SparePartIdCol.HeaderText = "Ид";
            this.SparePartIdCol.Name = "SparePartIdCol";
            this.SparePartIdCol.ReadOnly = true;
            this.SparePartIdCol.Visible = false;
            // 
            // ArticulCol
            // 
            this.ArticulCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ArticulCol.DataPropertyName = "SparePart.Articul";
            this.ArticulCol.HeaderText = "Артикул";
            this.ArticulCol.MinimumWidth = 130;
            this.ArticulCol.Name = "ArticulCol";
            this.ArticulCol.ReadOnly = true;
            this.ArticulCol.Width = 130;
            // 
            // TitleCol
            // 
            this.TitleCol.DataPropertyName = "SparePart.Title";
            this.TitleCol.HeaderText = "Название";
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            // 
            // MeasureUnitCol
            // 
            this.MeasureUnitCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MeasureUnitCol.DataPropertyName = "SparePart.MeasureUnit";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureUnitCol.DefaultCellStyle = dataGridViewCellStyle9;
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
            this.CountCol.DataPropertyName = "Count";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Gray;
            this.CountCol.DefaultCellStyle = dataGridViewCellStyle10;
            this.CountCol.HeaderText = "Кол-во";
            this.CountCol.Name = "CountCol";
            this.CountCol.ToolTipText = "Количество";
            this.CountCol.Width = 66;
            // 
            // PriceCol
            // 
            this.PriceCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.PriceCol.DataPropertyName = "Price";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.Format = "C2";
            dataGridViewCellStyle11.NullValue = null;
            this.PriceCol.DefaultCellStyle = dataGridViewCellStyle11;
            this.PriceCol.HeaderText = "Цена";
            this.PriceCol.MinimumWidth = 100;
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            this.PriceCol.ToolTipText = " Цена возврата";
            // 
            // SumCol
            // 
            this.SumCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "C2";
            dataGridViewCellStyle12.NullValue = null;
            this.SumCol.DefaultCellStyle = dataGridViewCellStyle12;
            this.SumCol.HeaderText = "Сумма";
            this.SumCol.MinimumWidth = 100;
            this.SumCol.Name = "SumCol";
            this.SumCol.ReadOnly = true;
            this.SumCol.ToolTipText = "Сумма возврата";
            // 
            // inTotalLabel
            // 
            this.inTotalLabel.AutoSize = true;
            this.inTotalLabel.Location = new System.Drawing.Point(697, 467);
            this.inTotalLabel.Name = "inTotalLabel";
            this.inTotalLabel.Size = new System.Drawing.Size(43, 13);
            this.inTotalLabel.TabIndex = 9;
            this.inTotalLabel.Text = "Итого :";
            // 
            // ContragentEmployeeLabel
            // 
            this.ContragentEmployeeLabel.AutoSize = true;
            this.ContragentEmployeeLabel.Location = new System.Drawing.Point(92, 571);
            this.ContragentEmployeeLabel.Name = "ContragentEmployeeLabel";
            this.ContragentEmployeeLabel.Size = new System.Drawing.Size(58, 13);
            this.ContragentEmployeeLabel.TabIndex = 10;
            this.ContragentEmployeeLabel.Text = "Выписал :";
            // 
            // AgentEmployeerLabel
            // 
            this.AgentEmployeerLabel.AutoSize = true;
            this.AgentEmployeerLabel.Location = new System.Drawing.Point(492, 571);
            this.AgentEmployeerLabel.Name = "AgentEmployeerLabel";
            this.AgentEmployeerLabel.Size = new System.Drawing.Size(51, 13);
            this.AgentEmployeerLabel.TabIndex = 11;
            this.AgentEmployeerLabel.Text = "Принял :";
            // 
            // operationIdTextBox
            // 
            this.operationIdTextBox.Location = new System.Drawing.Point(320, 12);
            this.operationIdTextBox.Name = "operationIdTextBox";
            this.operationIdTextBox.ReadOnly = true;
            this.operationIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.operationIdTextBox.TabIndex = 12;
            this.operationIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OperationDateTimePicker
            // 
            this.OperationDateTimePicker.CustomFormat = "";
            this.OperationDateTimePicker.Location = new System.Drawing.Point(480, 12);
            this.OperationDateTimePicker.MinDate = new System.DateTime(2015, 10, 10, 0, 0, 0, 0);
            this.OperationDateTimePicker.Name = "OperationDateTimePicker";
            this.OperationDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.OperationDateTimePicker.TabIndex = 13;
            this.OperationDateTimePicker.Value = new System.DateTime(2015, 10, 13, 0, 0, 0, 0);
            // 
            // AgentBackPanel
            // 
            this.AgentBackPanel.Controls.Add(this.AgentTextBox);
            this.AgentBackPanel.Location = new System.Drawing.Point(100, 56);
            this.AgentBackPanel.Name = "AgentBackPanel";
            this.AgentBackPanel.Size = new System.Drawing.Size(200, 24);
            this.AgentBackPanel.TabIndex = 15;
            // 
            // AgentTextBox
            // 
            this.AgentTextBox.Location = new System.Drawing.Point(2, 2);
            this.AgentTextBox.Name = "AgentTextBox";
            this.AgentTextBox.Size = new System.Drawing.Size(196, 20);
            this.AgentTextBox.TabIndex = 0;
            this.AgentTextBox.Text = "Truck Tir";
            this.AgentTextBox.Leave += new System.EventHandler(this.AgentTextBox_Leave);
            // 
            // ContragentBackPanel
            // 
            this.ContragentBackPanel.Controls.Add(this.ContragentTextBox);
            this.ContragentBackPanel.Location = new System.Drawing.Point(98, 101);
            this.ContragentBackPanel.Name = "ContragentBackPanel";
            this.ContragentBackPanel.Size = new System.Drawing.Size(200, 24);
            this.ContragentBackPanel.TabIndex = 16;
            // 
            // ContragentTextBox
            // 
            this.ContragentTextBox.AutoCompleteCustomSource.AddRange(new string[] {
            "Truck Tir",
            "ФЛП Тунеев А. С."});
            this.ContragentTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ContragentTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ContragentTextBox.Location = new System.Drawing.Point(2, 2);
            this.ContragentTextBox.Name = "ContragentTextBox";
            this.ContragentTextBox.ReadOnly = true;
            this.ContragentTextBox.Size = new System.Drawing.Size(196, 20);
            this.ContragentTextBox.TabIndex = 0;
            this.ContragentTextBox.Leave += new System.EventHandler(this.ContragentTextBox_Leave);
            this.ContragentTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ContragentTextBox_PreviewKeyDown);
            // 
            // ContragentStarLabel
            // 
            this.ContragentStarLabel.AutoSize = true;
            this.ContragentStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ContragentStarLabel.Location = new System.Drawing.Point(10, 100);
            this.ContragentStarLabel.Name = "ContragentStarLabel";
            this.ContragentStarLabel.Size = new System.Drawing.Size(20, 25);
            this.ContragentStarLabel.TabIndex = 17;
            this.ContragentStarLabel.Text = "*";
            // 
            // inTotalNumberLabel
            // 
            this.inTotalNumberLabel.AutoSize = true;
            this.inTotalNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTotalNumberLabel.Location = new System.Drawing.Point(745, 467);
            this.inTotalNumberLabel.Name = "inTotalNumberLabel";
            this.inTotalNumberLabel.Size = new System.Drawing.Size(39, 13);
            this.inTotalNumberLabel.TabIndex = 22;
            this.inTotalNumberLabel.Text = "0 (руб)";
            // 
            // ContragentEmployeeTextBox
            // 
            this.ContragentEmployeeTextBox.Location = new System.Drawing.Point(156, 571);
            this.ContragentEmployeeTextBox.Name = "ContragentEmployeeTextBox";
            this.ContragentEmployeeTextBox.Size = new System.Drawing.Size(237, 20);
            this.ContragentEmployeeTextBox.TabIndex = 23;
            // 
            // AgentEmployeerTextBox
            // 
            this.AgentEmployeerTextBox.Location = new System.Drawing.Point(549, 571);
            this.AgentEmployeerTextBox.Name = "AgentEmployeerTextBox";
            this.AgentEmployeerTextBox.ReadOnly = true;
            this.AgentEmployeerTextBox.Size = new System.Drawing.Size(232, 20);
            this.AgentEmployeerTextBox.TabIndex = 24;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(320, 625);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 25;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(495, 625);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // purchaseContextMenuStrip
            // 
            this.purchaseContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.purchaseContextMenuStrip.Name = "purchaseContextMenuStrip";
            this.purchaseContextMenuStrip.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "удалить";
            // 
            // noteRichTextBox
            // 
            this.noteRichTextBox.Location = new System.Drawing.Point(12, 510);
            this.noteRichTextBox.Name = "noteRichTextBox";
            this.noteRichTextBox.Size = new System.Drawing.Size(848, 39);
            this.noteRichTextBox.TabIndex = 33;
            this.noteRichTextBox.Text = "";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(9, 494);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(83, 13);
            this.descriptionLabel.TabIndex = 34;
            this.descriptionLabel.Text = "Комментарий :";
            // 
            // AgentStarLabel
            // 
            this.AgentStarLabel.AutoSize = true;
            this.AgentStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AgentStarLabel.Location = new System.Drawing.Point(10, 55);
            this.AgentStarLabel.Name = "AgentStarLabel";
            this.AgentStarLabel.Size = new System.Drawing.Size(20, 25);
            this.AgentStarLabel.TabIndex = 14;
            this.AgentStarLabel.Text = "*";
            // 
            // ReturnForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 660);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.noteRichTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.AgentEmployeerTextBox);
            this.Controls.Add(this.ContragentEmployeeTextBox);
            this.Controls.Add(this.inTotalNumberLabel);
            this.Controls.Add(this.ContragentBackPanel);
            this.Controls.Add(this.AgentBackPanel);
            this.Controls.Add(this.OperationDateTimePicker);
            this.Controls.Add(this.operationIdTextBox);
            this.Controls.Add(this.AgentEmployeerLabel);
            this.Controls.Add(this.ContragentEmployeeLabel);
            this.Controls.Add(this.inTotalLabel);
            this.Controls.Add(this.ReturnGroupBox);
            this.Controls.Add(this.ContragentLabel);
            this.Controls.Add(this.AgentLabel);
            this.Controls.Add(this.purchaseDateLabel);
            this.Controls.Add(this.operationIdLabel);
            this.Controls.Add(this.AgentStarLabel);
            this.Controls.Add(this.ContragentStarLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReturnForm";
            this.Text = "Форма возврата товара.";
            this.Load += new System.EventHandler(this.ReturnForm_Load);
            this.ReturnGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ReturnDGV)).EndInit();
            this.AgentBackPanel.ResumeLayout(false);
            this.AgentBackPanel.PerformLayout();
            this.ContragentBackPanel.ResumeLayout(false);
            this.ContragentBackPanel.PerformLayout();
            this.purchaseContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label operationIdLabel;
        private System.Windows.Forms.Label purchaseDateLabel;
        private System.Windows.Forms.Label AgentLabel;
        private System.Windows.Forms.Label ContragentLabel;
        private System.Windows.Forms.GroupBox ReturnGroupBox;
        private System.Windows.Forms.DataGridView ReturnDGV;
        private System.Windows.Forms.Label inTotalLabel;
        private System.Windows.Forms.Label ContragentEmployeeLabel;
        private System.Windows.Forms.Label AgentEmployeerLabel;
        private System.Windows.Forms.TextBox operationIdTextBox;
        private System.Windows.Forms.DateTimePicker OperationDateTimePicker;
        private System.Windows.Forms.Panel AgentBackPanel;
        private System.Windows.Forms.TextBox AgentTextBox;
        private System.Windows.Forms.Panel ContragentBackPanel;
        private System.Windows.Forms.TextBox ContragentTextBox;
        private System.Windows.Forms.Label ContragentStarLabel;
        private System.Windows.Forms.Label inTotalNumberLabel;
        private System.Windows.Forms.TextBox ContragentEmployeeTextBox;
        private System.Windows.Forms.TextBox AgentEmployeerTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip purchaseContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.RichTextBox noteRichTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label AgentStarLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn SparePartIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
    }
}