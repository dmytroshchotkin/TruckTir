namespace PartsApp
{
    partial class AddSparePartForm
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
            this.descrRichTextBox = new System.Windows.Forms.RichTextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.unitLabel = new System.Windows.Forms.Label();
            this.unitComboBoxBackPanel = new System.Windows.Forms.Panel();
            this.unitComboBox = new System.Windows.Forms.ComboBox();
            this.unitContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addUnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titleTextBoxBackPanel = new System.Windows.Forms.Panel();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.articulTextBoxBackPanel = new System.Windows.Forms.Panel();
            this.articulTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.unitStarLabel = new System.Windows.Forms.Label();
            this.titleStarLabel = new System.Windows.Forms.Label();
            this.articulStarLabel = new System.Windows.Forms.Label();
            this.manufacturerTextBox = new System.Windows.Forms.TextBox();
            this.manufacturerLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.addPhotoButton = new System.Windows.Forms.Button();
            this.photoPictureBox = new System.Windows.Forms.PictureBox();
            this.photoContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.articulLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.photoOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.unitComboBoxBackPanel.SuspendLayout();
            this.unitContextMenuStrip.SuspendLayout();
            this.titleTextBoxBackPanel.SuspendLayout();
            this.articulTextBoxBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photoPictureBox)).BeginInit();
            this.photoContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // descrRichTextBox
            // 
            this.descrRichTextBox.Location = new System.Drawing.Point(45, 299);
            this.descrRichTextBox.Name = "descrRichTextBox";
            this.descrRichTextBox.Size = new System.Drawing.Size(578, 71);
            this.descrRichTextBox.TabIndex = 42;
            this.descrRichTextBox.Text = "";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(42, 283);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 41;
            this.descriptionLabel.Text = "Описание:";
            // 
            // unitLabel
            // 
            this.unitLabel.AutoSize = true;
            this.unitLabel.Location = new System.Drawing.Point(42, 156);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(49, 13);
            this.unitLabel.TabIndex = 55;
            this.unitLabel.Text = "Ед. изм.";
            this.toolTip.SetToolTip(this.unitLabel, "Единица измерения товара");
            // 
            // unitComboBoxBackPanel
            // 
            this.unitComboBoxBackPanel.Controls.Add(this.unitComboBox);
            this.unitComboBoxBackPanel.Location = new System.Drawing.Point(130, 156);
            this.unitComboBoxBackPanel.Name = "unitComboBoxBackPanel";
            this.unitComboBoxBackPanel.Size = new System.Drawing.Size(89, 25);
            this.unitComboBoxBackPanel.TabIndex = 67;
            // 
            // unitComboBox
            // 
            this.unitComboBox.ContextMenuStrip = this.unitContextMenuStrip;
            this.unitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitComboBox.FormattingEnabled = true;
            this.unitComboBox.Location = new System.Drawing.Point(2, 2);
            this.unitComboBox.Name = "unitComboBox";
            this.unitComboBox.Size = new System.Drawing.Size(85, 21);
            this.unitComboBox.TabIndex = 32;
            this.unitComboBox.Leave += new System.EventHandler(this.unitComboBox_Leave);
            // 
            // unitContextMenuStrip
            // 
            this.unitContextMenuStrip.Enabled = false;
            this.unitContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUnitToolStripMenuItem});
            this.unitContextMenuStrip.Name = "unitContextMenuStrip";
            this.unitContextMenuStrip.Size = new System.Drawing.Size(275, 26);
            // 
            // addUnitToolStripMenuItem
            // 
            this.addUnitToolStripMenuItem.Enabled = false;
            this.addUnitToolStripMenuItem.Name = "addUnitToolStripMenuItem";
            this.addUnitToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.addUnitToolStripMenuItem.Text = "Добавить единицу измерения в базу";
            this.addUnitToolStripMenuItem.Click += new System.EventHandler(this.addUnitToolStripMenuItem_Click);
            // 
            // titleTextBoxBackPanel
            // 
            this.titleTextBoxBackPanel.Controls.Add(this.titleTextBox);
            this.titleTextBoxBackPanel.Location = new System.Drawing.Point(130, 57);
            this.titleTextBoxBackPanel.Name = "titleTextBoxBackPanel";
            this.titleTextBoxBackPanel.Size = new System.Drawing.Size(196, 24);
            this.titleTextBoxBackPanel.TabIndex = 66;
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(2, 2);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(192, 20);
            this.titleTextBox.TabIndex = 24;
            this.titleTextBox.Leave += new System.EventHandler(this.titleTextBox_Leave);
            // 
            // articulTextBoxBackPanel
            // 
            this.articulTextBoxBackPanel.Controls.Add(this.articulTextBox);
            this.articulTextBoxBackPanel.Location = new System.Drawing.Point(130, 8);
            this.articulTextBoxBackPanel.Name = "articulTextBoxBackPanel";
            this.articulTextBoxBackPanel.Size = new System.Drawing.Size(196, 24);
            this.articulTextBoxBackPanel.TabIndex = 65;
            // 
            // articulTextBox
            // 
            this.articulTextBox.Location = new System.Drawing.Point(2, 2);
            this.articulTextBox.Name = "articulTextBox";
            this.articulTextBox.Size = new System.Drawing.Size(192, 20);
            this.articulTextBox.TabIndex = 24;
            this.articulTextBox.Leave += new System.EventHandler(this.articulTextBox_Leave);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(358, 400);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 64;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(160, 400);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 63;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // unitStarLabel
            // 
            this.unitStarLabel.AutoSize = true;
            this.unitStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.unitStarLabel.Location = new System.Drawing.Point(25, 148);
            this.unitStarLabel.Name = "unitStarLabel";
            this.unitStarLabel.Size = new System.Drawing.Size(20, 25);
            this.unitStarLabel.TabIndex = 60;
            this.unitStarLabel.Text = "*";
            // 
            // titleStarLabel
            // 
            this.titleStarLabel.AutoSize = true;
            this.titleStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleStarLabel.Location = new System.Drawing.Point(25, 61);
            this.titleStarLabel.Name = "titleStarLabel";
            this.titleStarLabel.Size = new System.Drawing.Size(20, 25);
            this.titleStarLabel.TabIndex = 59;
            this.titleStarLabel.Text = "*";
            // 
            // articulStarLabel
            // 
            this.articulStarLabel.AutoSize = true;
            this.articulStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.articulStarLabel.Location = new System.Drawing.Point(25, 12);
            this.articulStarLabel.Name = "articulStarLabel";
            this.articulStarLabel.Size = new System.Drawing.Size(20, 25);
            this.articulStarLabel.TabIndex = 58;
            this.articulStarLabel.Text = "*";
            // 
            // manufacturerTextBox
            // 
            this.manufacturerTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.manufacturerTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.manufacturerTextBox.Location = new System.Drawing.Point(130, 111);
            this.manufacturerTextBox.Name = "manufacturerTextBox";
            this.manufacturerTextBox.Size = new System.Drawing.Size(194, 20);
            this.manufacturerTextBox.TabIndex = 54;
            this.manufacturerTextBox.TextChanged += new System.EventHandler(this.manufacturerTextBox_TextChanged);
            this.manufacturerTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.manufacturerTextBox_PreviewKeyDown);
            // 
            // manufacturerLabel
            // 
            this.manufacturerLabel.AutoSize = true;
            this.manufacturerLabel.Location = new System.Drawing.Point(42, 111);
            this.manufacturerLabel.Name = "manufacturerLabel";
            this.manufacturerLabel.Size = new System.Drawing.Size(89, 13);
            this.manufacturerLabel.TabIndex = 53;
            this.manufacturerLabel.Text = "Производитель:";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(42, 68);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(60, 13);
            this.titleLabel.TabIndex = 50;
            this.titleLabel.Text = "Название:";
            // 
            // addPhotoButton
            // 
            this.addPhotoButton.Location = new System.Drawing.Point(420, 242);
            this.addPhotoButton.Name = "addPhotoButton";
            this.addPhotoButton.Size = new System.Drawing.Size(130, 23);
            this.addPhotoButton.TabIndex = 49;
            this.addPhotoButton.Text = "Добавить фото";
            this.addPhotoButton.UseVisualStyleBackColor = true;
            this.addPhotoButton.Click += new System.EventHandler(this.addPhotoButton_Click);
            // 
            // photoPictureBox
            // 
            this.photoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photoPictureBox.ContextMenuStrip = this.photoContextMenuStrip;
            this.photoPictureBox.Location = new System.Drawing.Point(347, 12);
            this.photoPictureBox.Name = "photoPictureBox";
            this.photoPictureBox.Size = new System.Drawing.Size(276, 224);
            this.photoPictureBox.TabIndex = 48;
            this.photoPictureBox.TabStop = false;
            // 
            // photoContextMenuStrip
            // 
            this.photoContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deselectToolStripMenuItem});
            this.photoContextMenuStrip.Name = "photoContextMenuStrip";
            this.photoContextMenuStrip.Size = new System.Drawing.Size(188, 26);
            // 
            // deselectToolStripMenuItem
            // 
            this.deselectToolStripMenuItem.Name = "deselectToolStripMenuItem";
            this.deselectToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.deselectToolStripMenuItem.Text = "Убрать фотографию";
            this.deselectToolStripMenuItem.Click += new System.EventHandler(this.deselectToolStripMenuItem_Click);
            // 
            // articulLabel
            // 
            this.articulLabel.AutoSize = true;
            this.articulLabel.Location = new System.Drawing.Point(42, 19);
            this.articulLabel.Name = "articulLabel";
            this.articulLabel.Size = new System.Drawing.Size(51, 13);
            this.articulLabel.TabIndex = 47;
            this.articulLabel.Text = "Артикул:";
            // 
            // photoOpenFileDialog
            // 
            this.photoOpenFileDialog.Filter = "Image files (*.png;*.jpg;*jpeg)|*.png;*.jpg;*jpeg";
            this.photoOpenFileDialog.InitialDirectory = "D:\\\\";
            // 
            // AddSparePartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 438);
            this.Controls.Add(this.unitComboBoxBackPanel);
            this.Controls.Add(this.titleTextBoxBackPanel);
            this.Controls.Add(this.articulTextBoxBackPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.descrRichTextBox);
            this.Controls.Add(this.unitStarLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.titleStarLabel);
            this.Controls.Add(this.articulStarLabel);
            this.Controls.Add(this.unitLabel);
            this.Controls.Add(this.manufacturerTextBox);
            this.Controls.Add(this.manufacturerLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.addPhotoButton);
            this.Controls.Add(this.photoPictureBox);
            this.Controls.Add(this.articulLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddSparePartForm";
            this.Text = "Форма добавления новой единицы товара";
            this.Load += new System.EventHandler(this.AddSparePartForm_Load);
            this.unitComboBoxBackPanel.ResumeLayout(false);
            this.unitContextMenuStrip.ResumeLayout(false);
            this.titleTextBoxBackPanel.ResumeLayout(false);
            this.titleTextBoxBackPanel.PerformLayout();
            this.articulTextBoxBackPanel.ResumeLayout(false);
            this.articulTextBoxBackPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photoPictureBox)).EndInit();
            this.photoContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel unitComboBoxBackPanel;
        private System.Windows.Forms.ComboBox unitComboBox;
        private System.Windows.Forms.Panel titleTextBoxBackPanel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Panel articulTextBoxBackPanel;
        private System.Windows.Forms.TextBox articulTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RichTextBox descrRichTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label unitStarLabel;
        private System.Windows.Forms.Label titleStarLabel;
        private System.Windows.Forms.Label articulStarLabel;
        private System.Windows.Forms.Label unitLabel;
        private System.Windows.Forms.TextBox manufacturerTextBox;
        private System.Windows.Forms.Label manufacturerLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button addPhotoButton;
        private System.Windows.Forms.PictureBox photoPictureBox;
        private System.Windows.Forms.Label articulLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog photoOpenFileDialog;
        private System.Windows.Forms.ContextMenuStrip unitContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addUnitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip photoContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deselectToolStripMenuItem;
    }
}