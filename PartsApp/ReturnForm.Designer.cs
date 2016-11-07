namespace PartsApp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.purchaseIdLabel = new System.Windows.Forms.Label();
            this.purchaseDateLabel = new System.Windows.Forms.Label();
            this.supplierLabel = new System.Windows.Forms.Label();
            this.buyerLabel = new System.Windows.Forms.Label();
            this.purchaseGroupBox = new System.Windows.Forms.GroupBox();
            this.PurchaseDGV = new System.Windows.Forms.DataGridView();
            this.SparePartIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArticulCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureUnitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MarkupCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.inTotalNumberLabel = new System.Windows.Forms.Label();
            this.supplierAgentTextBox = new System.Windows.Forms.TextBox();
            this.buyerAgentTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.purchaseContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.purchaseGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PurchaseDGV)).BeginInit();
            this.supplierBackPanel.SuspendLayout();
            this.buyerBackPanel.SuspendLayout();
            this.purchaseContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // purchaseIdLabel
            // 
            this.purchaseIdLabel.AutoSize = true;
            this.purchaseIdLabel.Location = new System.Drawing.Point(222, 19);
            this.purchaseIdLabel.Name = "purchaseIdLabel";
            this.purchaseIdLabel.Size = new System.Drawing.Size(77, 13);
            this.purchaseIdLabel.TabIndex = 0;
            this.purchaseIdLabel.Text = "Накладная №";
            // 
            // purchaseDateLabel
            // 
            this.purchaseDateLabel.AutoSize = true;
            this.purchaseDateLabel.Location = new System.Drawing.Point(435, 19);
            this.purchaseDateLabel.Name = "purchaseDateLabel";
            this.purchaseDateLabel.Size = new System.Drawing.Size(24, 13);
            this.purchaseDateLabel.TabIndex = 1;
            this.purchaseDateLabel.Text = "от :";
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(33, 64);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(71, 13);
            this.supplierLabel.TabIndex = 2;
            this.supplierLabel.Text = "Поставщик :";
            // 
            // buyerLabel
            // 
            this.buyerLabel.AutoSize = true;
            this.buyerLabel.Location = new System.Drawing.Point(33, 109);
            this.buyerLabel.Name = "buyerLabel";
            this.buyerLabel.Size = new System.Drawing.Size(73, 13);
            this.buyerLabel.TabIndex = 3;
            this.buyerLabel.Text = "Покупатель :";
            // 
            // purchaseGroupBox
            // 
            this.purchaseGroupBox.Controls.Add(this.PurchaseDGV);
            this.purchaseGroupBox.Location = new System.Drawing.Point(25, 155);
            this.purchaseGroupBox.Name = "purchaseGroupBox";
            this.purchaseGroupBox.Size = new System.Drawing.Size(854, 211);
            this.purchaseGroupBox.TabIndex = 6;
            this.purchaseGroupBox.TabStop = false;
            this.purchaseGroupBox.Text = "Лист прихода.";
            // 
            // PurchaseDGV
            // 
            this.PurchaseDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PurchaseDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PurchaseDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SparePartIdCol,
            this.ArticulCol,
            this.TitleCol,
            this.MeasureUnitCol,
            this.CountCol,
            this.PriceCol,
            this.SumCol,
            this.MarkupCol,
            this.SellingPriceCol});
            this.PurchaseDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PurchaseDGV.Enabled = false;
            this.PurchaseDGV.Location = new System.Drawing.Point(3, 16);
            this.PurchaseDGV.Name = "PurchaseDGV";
            this.PurchaseDGV.Size = new System.Drawing.Size(848, 192);
            this.PurchaseDGV.TabIndex = 0;
            // 
            // SparePartIdCol
            // 
            this.SparePartIdCol.HeaderText = "Ид";
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
            this.TitleCol.Name = "TitleCol";
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
            this.CountCol.Name = "CountCol";
            this.CountCol.ReadOnly = true;
            this.CountCol.ToolTipText = "Количество";
            this.CountCol.Width = 66;
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
            this.MarkupCol.Name = "MarkupCol";
            this.MarkupCol.ReadOnly = true;
            this.MarkupCol.Visible = false;
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
            // 
            // inTotalLabel
            // 
            this.inTotalLabel.AutoSize = true;
            this.inTotalLabel.Location = new System.Drawing.Point(710, 369);
            this.inTotalLabel.Name = "inTotalLabel";
            this.inTotalLabel.Size = new System.Drawing.Size(43, 13);
            this.inTotalLabel.TabIndex = 9;
            this.inTotalLabel.Text = "Итого :";
            // 
            // supplierAgentLabel
            // 
            this.supplierAgentLabel.AutoSize = true;
            this.supplierAgentLabel.Location = new System.Drawing.Point(105, 473);
            this.supplierAgentLabel.Name = "supplierAgentLabel";
            this.supplierAgentLabel.Size = new System.Drawing.Size(58, 13);
            this.supplierAgentLabel.TabIndex = 10;
            this.supplierAgentLabel.Text = "Выписал :";
            // 
            // buyerAgentLabel
            // 
            this.buyerAgentLabel.AutoSize = true;
            this.buyerAgentLabel.Location = new System.Drawing.Point(505, 473);
            this.buyerAgentLabel.Name = "buyerAgentLabel";
            this.buyerAgentLabel.Size = new System.Drawing.Size(51, 13);
            this.buyerAgentLabel.TabIndex = 11;
            this.buyerAgentLabel.Text = "Принял :";
            // 
            // purchaseIdTextBox
            // 
            this.purchaseIdTextBox.Location = new System.Drawing.Point(305, 16);
            this.purchaseIdTextBox.Name = "purchaseIdTextBox";
            this.purchaseIdTextBox.ReadOnly = true;
            this.purchaseIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.purchaseIdTextBox.TabIndex = 12;
            // 
            // purchaseDateTimePicker
            // 
            this.purchaseDateTimePicker.CustomFormat = "";
            this.purchaseDateTimePicker.Location = new System.Drawing.Point(465, 16);
            this.purchaseDateTimePicker.MinDate = new System.DateTime(2015, 10, 10, 0, 0, 0, 0);
            this.purchaseDateTimePicker.Name = "purchaseDateTimePicker";
            this.purchaseDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.purchaseDateTimePicker.TabIndex = 13;
            this.purchaseDateTimePicker.Value = new System.DateTime(2015, 10, 13, 0, 0, 0, 0);
            // 
            // supplierStarLabel
            // 
            this.supplierStarLabel.AutoSize = true;
            this.supplierStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.supplierStarLabel.Location = new System.Drawing.Point(20, 55);
            this.supplierStarLabel.Name = "supplierStarLabel";
            this.supplierStarLabel.Size = new System.Drawing.Size(20, 25);
            this.supplierStarLabel.TabIndex = 14;
            this.supplierStarLabel.Text = "*";
            // 
            // supplierBackPanel
            // 
            this.supplierBackPanel.Controls.Add(this.supplierTextBox);
            this.supplierBackPanel.Location = new System.Drawing.Point(110, 56);
            this.supplierBackPanel.Name = "supplierBackPanel";
            this.supplierBackPanel.Size = new System.Drawing.Size(200, 24);
            this.supplierBackPanel.TabIndex = 15;
            // 
            // supplierTextBox
            // 
            this.supplierTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.supplierTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.supplierTextBox.Location = new System.Drawing.Point(2, 2);
            this.supplierTextBox.Name = "supplierTextBox";
            this.supplierTextBox.Size = new System.Drawing.Size(196, 20);
            this.supplierTextBox.TabIndex = 0;
            // 
            // buyerBackPanel
            // 
            this.buyerBackPanel.Controls.Add(this.buyerTextBox);
            this.buyerBackPanel.Location = new System.Drawing.Point(108, 101);
            this.buyerBackPanel.Name = "buyerBackPanel";
            this.buyerBackPanel.Size = new System.Drawing.Size(200, 24);
            this.buyerBackPanel.TabIndex = 16;
            // 
            // buyerTextBox
            // 
            this.buyerTextBox.AutoCompleteCustomSource.AddRange(new string[] {
            "Truck Tir",
            "ФЛП Тунеев А. С."});
            this.buyerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.buyerTextBox.Location = new System.Drawing.Point(2, 2);
            this.buyerTextBox.Name = "buyerTextBox";
            this.buyerTextBox.Size = new System.Drawing.Size(196, 20);
            this.buyerTextBox.TabIndex = 0;
            this.buyerTextBox.Text = "Truck Tir";
            // 
            // buyerStarLabel
            // 
            this.buyerStarLabel.AutoSize = true;
            this.buyerStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buyerStarLabel.Location = new System.Drawing.Point(20, 100);
            this.buyerStarLabel.Name = "buyerStarLabel";
            this.buyerStarLabel.Size = new System.Drawing.Size(20, 25);
            this.buyerStarLabel.TabIndex = 17;
            this.buyerStarLabel.Text = "*";
            // 
            // inTotalNumberLabel
            // 
            this.inTotalNumberLabel.AutoSize = true;
            this.inTotalNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inTotalNumberLabel.Location = new System.Drawing.Point(758, 369);
            this.inTotalNumberLabel.Name = "inTotalNumberLabel";
            this.inTotalNumberLabel.Size = new System.Drawing.Size(39, 13);
            this.inTotalNumberLabel.TabIndex = 22;
            this.inTotalNumberLabel.Text = "0 (руб)";
            // 
            // supplierAgentTextBox
            // 
            this.supplierAgentTextBox.Location = new System.Drawing.Point(169, 473);
            this.supplierAgentTextBox.Name = "supplierAgentTextBox";
            this.supplierAgentTextBox.ReadOnly = true;
            this.supplierAgentTextBox.Size = new System.Drawing.Size(237, 20);
            this.supplierAgentTextBox.TabIndex = 23;
            // 
            // buyerAgentTextBox
            // 
            this.buyerAgentTextBox.Location = new System.Drawing.Point(562, 473);
            this.buyerAgentTextBox.Name = "buyerAgentTextBox";
            this.buyerAgentTextBox.Size = new System.Drawing.Size(232, 20);
            this.buyerAgentTextBox.TabIndex = 24;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(333, 527);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 25;
            this.okButton.Text = "Ок";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(508, 527);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
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
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Location = new System.Drawing.Point(25, 412);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(848, 39);
            this.descriptionRichTextBox.TabIndex = 33;
            this.descriptionRichTextBox.Text = "";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(22, 396);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(83, 13);
            this.descriptionLabel.TabIndex = 34;
            this.descriptionLabel.Text = "Комментарий :";
            // 
            // ReturnForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 560);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionRichTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.buyerAgentTextBox);
            this.Controls.Add(this.supplierAgentTextBox);
            this.Controls.Add(this.inTotalNumberLabel);
            this.Controls.Add(this.buyerBackPanel);
            this.Controls.Add(this.supplierBackPanel);
            this.Controls.Add(this.purchaseDateTimePicker);
            this.Controls.Add(this.purchaseIdTextBox);
            this.Controls.Add(this.buyerAgentLabel);
            this.Controls.Add(this.supplierAgentLabel);
            this.Controls.Add(this.inTotalLabel);
            this.Controls.Add(this.purchaseGroupBox);
            this.Controls.Add(this.buyerLabel);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.purchaseDateLabel);
            this.Controls.Add(this.purchaseIdLabel);
            this.Controls.Add(this.supplierStarLabel);
            this.Controls.Add(this.buyerStarLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReturnForm";
            this.Text = "Форма прихода товара.";
            this.purchaseGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PurchaseDGV)).EndInit();
            this.supplierBackPanel.ResumeLayout(false);
            this.supplierBackPanel.PerformLayout();
            this.buyerBackPanel.ResumeLayout(false);
            this.buyerBackPanel.PerformLayout();
            this.purchaseContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label purchaseIdLabel;
        private System.Windows.Forms.Label purchaseDateLabel;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.Label buyerLabel;
        private System.Windows.Forms.GroupBox purchaseGroupBox;
        private System.Windows.Forms.DataGridView PurchaseDGV;
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
        private System.Windows.Forms.Label inTotalNumberLabel;
        private System.Windows.Forms.TextBox supplierAgentTextBox;
        private System.Windows.Forms.TextBox buyerAgentTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip purchaseContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn SparePartIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArticulCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureUnitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MarkupCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPriceCol;
    }
}