namespace PartsApp
{
    partial class SparePartForm
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
            this.DescrRichTextBox = new System.Windows.Forms.RichTextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.MeasureUnitLabel = new System.Windows.Forms.Label();
            this.MeasureUnitBackPanel = new System.Windows.Forms.Panel();
            this.MeasureUnitComboBox = new System.Windows.Forms.ComboBox();
            this.TitleBackPanel = new System.Windows.Forms.Panel();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.ArticulBackPanel = new System.Windows.Forms.Panel();
            this.ArticulTextBox = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.MeasureUnitStarLabel = new System.Windows.Forms.Label();
            this.TitleStarLabel = new System.Windows.Forms.Label();
            this.ArticulStarLabel = new System.Windows.Forms.Label();
            this.ManufacturerTextBox = new System.Windows.Forms.TextBox();
            this.ManufacturerLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.AddPhotoButton = new System.Windows.Forms.Button();
            this.PhotoPictureBox = new System.Windows.Forms.PictureBox();
            this.PhotoContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ArticulLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PhotoOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.storageCellLabel = new System.Windows.Forms.Label();
            this.StorageCellTextBox = new System.Windows.Forms.TextBox();
            this.MeasureUnitBackPanel.SuspendLayout();
            this.TitleBackPanel.SuspendLayout();
            this.ArticulBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhotoPictureBox)).BeginInit();
            this.PhotoContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // DescrRichTextBox
            // 
            this.DescrRichTextBox.Location = new System.Drawing.Point(60, 368);
            this.DescrRichTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DescrRichTextBox.Name = "DescrRichTextBox";
            this.DescrRichTextBox.Size = new System.Drawing.Size(769, 86);
            this.DescrRichTextBox.TabIndex = 42;
            this.DescrRichTextBox.Text = "";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(56, 348);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(78, 16);
            this.DescriptionLabel.TabIndex = 41;
            this.DescriptionLabel.Text = "Описание :";
            // 
            // MeasureUnitLabel
            // 
            this.MeasureUnitLabel.AutoSize = true;
            this.MeasureUnitLabel.Location = new System.Drawing.Point(56, 192);
            this.MeasureUnitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MeasureUnitLabel.Name = "MeasureUnitLabel";
            this.MeasureUnitLabel.Size = new System.Drawing.Size(66, 16);
            this.MeasureUnitLabel.TabIndex = 55;
            this.MeasureUnitLabel.Text = "Ед. изм. :";
            this.toolTip.SetToolTip(this.MeasureUnitLabel, "Единица измерения товара");
            // 
            // MeasureUnitBackPanel
            // 
            this.MeasureUnitBackPanel.Controls.Add(this.MeasureUnitComboBox);
            this.MeasureUnitBackPanel.Location = new System.Drawing.Point(177, 182);
            this.MeasureUnitBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MeasureUnitBackPanel.Name = "MeasureUnitBackPanel";
            this.MeasureUnitBackPanel.Size = new System.Drawing.Size(119, 31);
            this.MeasureUnitBackPanel.TabIndex = 67;
            // 
            // MeasureUnitComboBox
            // 
            this.MeasureUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MeasureUnitComboBox.FormattingEnabled = true;
            this.MeasureUnitComboBox.Location = new System.Drawing.Point(3, 2);
            this.MeasureUnitComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MeasureUnitComboBox.Name = "MeasureUnitComboBox";
            this.MeasureUnitComboBox.Size = new System.Drawing.Size(112, 24);
            this.MeasureUnitComboBox.TabIndex = 32;
            this.MeasureUnitComboBox.Leave += new System.EventHandler(this.MeasureUnitComboBox_Leave);
            // 
            // TitleBackPanel
            // 
            this.TitleBackPanel.Controls.Add(this.TitleTextBox);
            this.TitleBackPanel.Location = new System.Drawing.Point(177, 75);
            this.TitleBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TitleBackPanel.Name = "TitleBackPanel";
            this.TitleBackPanel.Size = new System.Drawing.Size(261, 30);
            this.TitleBackPanel.TabIndex = 66;
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(3, 2);
            this.TitleTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(255, 22);
            this.TitleTextBox.TabIndex = 24;
            this.TitleTextBox.Leave += new System.EventHandler(this.TitleTextBox_Leave);
            // 
            // ArticulBackPanel
            // 
            this.ArticulBackPanel.Controls.Add(this.ArticulTextBox);
            this.ArticulBackPanel.Location = new System.Drawing.Point(177, 15);
            this.ArticulBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ArticulBackPanel.Name = "ArticulBackPanel";
            this.ArticulBackPanel.Size = new System.Drawing.Size(261, 30);
            this.ArticulBackPanel.TabIndex = 65;
            // 
            // ArticulTextBox
            // 
            this.ArticulTextBox.Location = new System.Drawing.Point(3, 2);
            this.ArticulTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ArticulTextBox.Name = "ArticulTextBox";
            this.ArticulTextBox.Size = new System.Drawing.Size(255, 22);
            this.ArticulTextBox.TabIndex = 24;
            this.ArticulTextBox.Leave += new System.EventHandler(this.ArticulTextBox_Leave);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(477, 492);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(100, 28);
            this.CancelButton.TabIndex = 64;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CancelButton_MouseClick);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(213, 492);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 28);
            this.OkButton.TabIndex = 63;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OkButton_MouseClick);
            // 
            // MeasureUnitStarLabel
            // 
            this.MeasureUnitStarLabel.AutoSize = true;
            this.MeasureUnitStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MeasureUnitStarLabel.Location = new System.Drawing.Point(33, 182);
            this.MeasureUnitStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MeasureUnitStarLabel.Name = "MeasureUnitStarLabel";
            this.MeasureUnitStarLabel.Size = new System.Drawing.Size(23, 30);
            this.MeasureUnitStarLabel.TabIndex = 60;
            this.MeasureUnitStarLabel.Text = "*";
            // 
            // TitleStarLabel
            // 
            this.TitleStarLabel.AutoSize = true;
            this.TitleStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleStarLabel.Location = new System.Drawing.Point(33, 75);
            this.TitleStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TitleStarLabel.Name = "TitleStarLabel";
            this.TitleStarLabel.Size = new System.Drawing.Size(23, 30);
            this.TitleStarLabel.TabIndex = 59;
            this.TitleStarLabel.Text = "*";
            // 
            // ArticulStarLabel
            // 
            this.ArticulStarLabel.AutoSize = true;
            this.ArticulStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ArticulStarLabel.Location = new System.Drawing.Point(33, 15);
            this.ArticulStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ArticulStarLabel.Name = "ArticulStarLabel";
            this.ArticulStarLabel.Size = new System.Drawing.Size(23, 30);
            this.ArticulStarLabel.TabIndex = 58;
            this.ArticulStarLabel.Text = "*";
            // 
            // ManufacturerTextBox
            // 
            this.ManufacturerTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ManufacturerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ManufacturerTextBox.Location = new System.Drawing.Point(177, 133);
            this.ManufacturerTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ManufacturerTextBox.Name = "ManufacturerTextBox";
            this.ManufacturerTextBox.Size = new System.Drawing.Size(257, 22);
            this.ManufacturerTextBox.TabIndex = 54;
            this.ManufacturerTextBox.Leave += new System.EventHandler(this.ManufacturerTextBox_Leave);
            this.ManufacturerTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ManufacturerTextBox_PreviewKeyDown);
            // 
            // ManufacturerLabel
            // 
            this.ManufacturerLabel.AutoSize = true;
            this.ManufacturerLabel.Location = new System.Drawing.Point(56, 137);
            this.ManufacturerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ManufacturerLabel.Name = "ManufacturerLabel";
            this.ManufacturerLabel.Size = new System.Drawing.Size(117, 16);
            this.ManufacturerLabel.TabIndex = 53;
            this.ManufacturerLabel.Text = "Производитель :";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(56, 84);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(79, 16);
            this.TitleLabel.TabIndex = 50;
            this.TitleLabel.Text = "Название :";
            // 
            // AddPhotoButton
            // 
            this.AddPhotoButton.Location = new System.Drawing.Point(560, 298);
            this.AddPhotoButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AddPhotoButton.Name = "AddPhotoButton";
            this.AddPhotoButton.Size = new System.Drawing.Size(173, 28);
            this.AddPhotoButton.TabIndex = 49;
            this.AddPhotoButton.Text = "Добавить фото";
            this.AddPhotoButton.UseVisualStyleBackColor = true;
            this.AddPhotoButton.Click += new System.EventHandler(this.AddPhotoButton_Click);
            // 
            // PhotoPictureBox
            // 
            this.PhotoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PhotoPictureBox.Location = new System.Drawing.Point(463, 15);
            this.PhotoPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PhotoPictureBox.Name = "PhotoPictureBox";
            this.PhotoPictureBox.Size = new System.Drawing.Size(367, 275);
            this.PhotoPictureBox.TabIndex = 48;
            this.PhotoPictureBox.TabStop = false;
            this.PhotoPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PhotoPictureBox_MouseClick);
            // 
            // PhotoContextMenuStrip
            // 
            this.PhotoContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.PhotoContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeselectToolStripMenuItem});
            this.PhotoContextMenuStrip.Name = "photoContextMenuStrip";
            this.PhotoContextMenuStrip.Size = new System.Drawing.Size(220, 28);
            // 
            // DeselectToolStripMenuItem
            // 
            this.DeselectToolStripMenuItem.Name = "DeselectToolStripMenuItem";
            this.DeselectToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.DeselectToolStripMenuItem.Text = "Убрать фотографию";
            this.DeselectToolStripMenuItem.Click += new System.EventHandler(this.DeselectToolStripMenuItem_Click);
            // 
            // ArticulLabel
            // 
            this.ArticulLabel.AutoSize = true;
            this.ArticulLabel.Location = new System.Drawing.Point(56, 23);
            this.ArticulLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ArticulLabel.Name = "ArticulLabel";
            this.ArticulLabel.Size = new System.Drawing.Size(68, 16);
            this.ArticulLabel.TabIndex = 47;
            this.ArticulLabel.Text = "Артикул :";
            // 
            // PhotoOpenFileDialog
            // 
            this.PhotoOpenFileDialog.Filter = "Image files (*.png;*.jpg;*jpeg)|*.png;*.jpg;*jpeg";
            this.PhotoOpenFileDialog.InitialDirectory = "D:\\\\";
            // 
            // storageCellLabel
            // 
            this.storageCellLabel.AutoSize = true;
            this.storageCellLabel.Location = new System.Drawing.Point(56, 244);
            this.storageCellLabel.Name = "storageCellLabel";
            this.storageCellLabel.Size = new System.Drawing.Size(47, 16);
            this.storageCellLabel.TabIndex = 68;
            this.storageCellLabel.Text = "Склад";
            // 
            // storageCellTextBox
            // 
            this.StorageCellTextBox.Location = new System.Drawing.Point(177, 238);
            this.StorageCellTextBox.MaxLength = 100;
            this.StorageCellTextBox.Name = "storageCellTextBox";
            this.StorageCellTextBox.Size = new System.Drawing.Size(261, 22);
            this.StorageCellTextBox.TabIndex = 69;
            // 
            // SparePartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 539);
            this.Controls.Add(this.StorageCellTextBox);
            this.Controls.Add(this.storageCellLabel);
            this.Controls.Add(this.MeasureUnitBackPanel);
            this.Controls.Add(this.TitleBackPanel);
            this.Controls.Add(this.ArticulBackPanel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.DescrRichTextBox);
            this.Controls.Add(this.MeasureUnitStarLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.TitleStarLabel);
            this.Controls.Add(this.ArticulStarLabel);
            this.Controls.Add(this.MeasureUnitLabel);
            this.Controls.Add(this.ManufacturerTextBox);
            this.Controls.Add(this.ManufacturerLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.AddPhotoButton);
            this.Controls.Add(this.PhotoPictureBox);
            this.Controls.Add(this.ArticulLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "SparePartForm";
            this.Text = "Форма добавления новой единицы товара";
            this.Load += new System.EventHandler(this.AddSparePartForm_Load);
            this.MeasureUnitBackPanel.ResumeLayout(false);
            this.TitleBackPanel.ResumeLayout(false);
            this.TitleBackPanel.PerformLayout();
            this.ArticulBackPanel.ResumeLayout(false);
            this.ArticulBackPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhotoPictureBox)).EndInit();
            this.PhotoContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MeasureUnitBackPanel;
        private System.Windows.Forms.ComboBox MeasureUnitComboBox;
        private System.Windows.Forms.Panel TitleBackPanel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Panel ArticulBackPanel;
        private System.Windows.Forms.TextBox ArticulTextBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.RichTextBox DescrRichTextBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Label MeasureUnitStarLabel;
        private System.Windows.Forms.Label TitleStarLabel;
        private System.Windows.Forms.Label ArticulStarLabel;
        private System.Windows.Forms.Label MeasureUnitLabel;
        private System.Windows.Forms.TextBox ManufacturerTextBox;
        private System.Windows.Forms.Label ManufacturerLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button AddPhotoButton;
        private System.Windows.Forms.PictureBox PhotoPictureBox;
        private System.Windows.Forms.Label ArticulLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog PhotoOpenFileDialog;
        private System.Windows.Forms.ContextMenuStrip PhotoContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeselectToolStripMenuItem;
        private System.Windows.Forms.Label storageCellLabel;
        private System.Windows.Forms.TextBox StorageCellTextBox;
    }
}