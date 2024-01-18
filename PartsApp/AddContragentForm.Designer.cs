namespace PartsApp
{
    partial class AddContragentForm
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
            this.entityComboBox = new System.Windows.Forms.ComboBox();
            this.entityLabel = new System.Windows.Forms.Label();
            this.contragentNameLabel = new System.Windows.Forms.Label();
            this.codeLabel = new System.Windows.Forms.Label();
            this.contragentNameStarLabel = new System.Windows.Forms.Label();
            this.contragentNameBackPanel = new System.Windows.Forms.Panel();
            this.contragentNameTextBox = new System.Windows.Forms.TextBox();
            this.codeBackPanel = new System.Windows.Forms.Panel();
            this.codeMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.addContactInfoButton = new System.Windows.Forms.Button();
            this.contactInfoPanel = new System.Windows.Forms.Panel();
            this.roomLabel = new System.Windows.Forms.Label();
            this.roomTextBox = new System.Windows.Forms.TextBox();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.websiteTextBox = new System.Windows.Forms.TextBox();
            this.extPhoneTextBox = new System.Windows.Forms.TextBox();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.houseTextBox = new System.Windows.Forms.TextBox();
            this.streetTextBox = new System.Windows.Forms.TextBox();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.regionTextBox = new System.Windows.Forms.TextBox();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.streetLabel = new System.Windows.Forms.Label();
            this.extPhoneLabel = new System.Windows.Forms.Label();
            this.websiteLabel = new System.Windows.Forms.Label();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.houseLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.regionLabel = new System.Windows.Forms.Label();
            this.countryLabel = new System.Windows.Forms.Label();
            this.descrRichTextBox = new System.Windows.Forms.RichTextBox();
            this.descrLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BalanceLabel = new System.Windows.Forms.Label();
            this.BalanceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.FilledBalanceLabel = new System.Windows.Forms.Label();
            this.contragentNameBackPanel.SuspendLayout();
            this.codeBackPanel.SuspendLayout();
            this.contactInfoPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BalanceNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // entityComboBox
            // 
            this.entityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entityComboBox.FormattingEnabled = true;
            this.entityComboBox.Items.AddRange(new object[] {
            "Физ. лицо",
            "Юр.  лицо"});
            this.entityComboBox.Location = new System.Drawing.Point(191, 25);
            this.entityComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.entityComboBox.Name = "entityComboBox";
            this.entityComboBox.Size = new System.Drawing.Size(116, 24);
            this.entityComboBox.TabIndex = 0;
            // 
            // entityLabel
            // 
            this.entityLabel.AutoSize = true;
            this.entityLabel.Location = new System.Drawing.Point(37, 28);
            this.entityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.entityLabel.Name = "entityLabel";
            this.entityLabel.Size = new System.Drawing.Size(104, 16);
            this.entityLabel.TabIndex = 1;
            this.entityLabel.Text = "Юр./Физ. лицо :";
            // 
            // contragentNameLabel
            // 
            this.contragentNameLabel.AutoSize = true;
            this.contragentNameLabel.Location = new System.Drawing.Point(37, 79);
            this.contragentNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.contragentNameLabel.Name = "contragentNameLabel";
            this.contragentNameLabel.Size = new System.Drawing.Size(119, 16);
            this.contragentNameLabel.TabIndex = 2;
            this.contragentNameLabel.Text = "ФИО (Компания) :";
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(37, 128);
            this.codeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(85, 16);
            this.codeLabel.TabIndex = 3;
            this.codeLabel.Text = "ИНН/ОКПО :";
            // 
            // contragentNameStarLabel
            // 
            this.contragentNameStarLabel.AutoSize = true;
            this.contragentNameStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.contragentNameStarLabel.Location = new System.Drawing.Point(21, 68);
            this.contragentNameStarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.contragentNameStarLabel.Name = "contragentNameStarLabel";
            this.contragentNameStarLabel.Size = new System.Drawing.Size(23, 30);
            this.contragentNameStarLabel.TabIndex = 59;
            this.contragentNameStarLabel.Text = "*";
            // 
            // contragentNameBackPanel
            // 
            this.contragentNameBackPanel.Controls.Add(this.contragentNameTextBox);
            this.contragentNameBackPanel.Location = new System.Drawing.Point(188, 69);
            this.contragentNameBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contragentNameBackPanel.Name = "contragentNameBackPanel";
            this.contragentNameBackPanel.Size = new System.Drawing.Size(285, 30);
            this.contragentNameBackPanel.TabIndex = 61;
            // 
            // contragentNameTextBox
            // 
            this.contragentNameTextBox.Location = new System.Drawing.Point(3, 2);
            this.contragentNameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contragentNameTextBox.Name = "contragentNameTextBox";
            this.contragentNameTextBox.Size = new System.Drawing.Size(279, 22);
            this.contragentNameTextBox.TabIndex = 62;
            this.contragentNameTextBox.Leave += new System.EventHandler(this.contragentNameTextBox_Leave);
            // 
            // codeBackPanel
            // 
            this.codeBackPanel.Controls.Add(this.codeMaskedTextBox);
            this.codeBackPanel.Location = new System.Drawing.Point(188, 114);
            this.codeBackPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.codeBackPanel.Name = "codeBackPanel";
            this.codeBackPanel.Size = new System.Drawing.Size(103, 30);
            this.codeBackPanel.TabIndex = 61;
            // 
            // codeMaskedTextBox
            // 
            this.codeMaskedTextBox.Location = new System.Drawing.Point(3, 2);
            this.codeMaskedTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.codeMaskedTextBox.Mask = "0000000099";
            this.codeMaskedTextBox.Name = "codeMaskedTextBox";
            this.codeMaskedTextBox.Size = new System.Drawing.Size(96, 22);
            this.codeMaskedTextBox.TabIndex = 69;
            this.codeMaskedTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.codeMaskedTextBox_MouseClick);
            this.codeMaskedTextBox.Leave += new System.EventHandler(this.codeMaskedTextBox_Leave);
            // 
            // addContactInfoButton
            // 
            this.addContactInfoButton.Location = new System.Drawing.Point(28, 174);
            this.addContactInfoButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.addContactInfoButton.Name = "addContactInfoButton";
            this.addContactInfoButton.Size = new System.Drawing.Size(337, 27);
            this.addContactInfoButton.TabIndex = 62;
            this.addContactInfoButton.Text = "Добавить контактную информацию";
            this.addContactInfoButton.UseVisualStyleBackColor = true;
            this.addContactInfoButton.Click += new System.EventHandler(this.addContactInfoButton_Click);
            // 
            // contactInfoPanel
            // 
            this.contactInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contactInfoPanel.Controls.Add(this.roomLabel);
            this.contactInfoPanel.Controls.Add(this.roomTextBox);
            this.contactInfoPanel.Controls.Add(this.emailTextBox);
            this.contactInfoPanel.Controls.Add(this.websiteTextBox);
            this.contactInfoPanel.Controls.Add(this.extPhoneTextBox);
            this.contactInfoPanel.Controls.Add(this.phoneTextBox);
            this.contactInfoPanel.Controls.Add(this.houseTextBox);
            this.contactInfoPanel.Controls.Add(this.streetTextBox);
            this.contactInfoPanel.Controls.Add(this.cityTextBox);
            this.contactInfoPanel.Controls.Add(this.regionTextBox);
            this.contactInfoPanel.Controls.Add(this.countryTextBox);
            this.contactInfoPanel.Controls.Add(this.emailLabel);
            this.contactInfoPanel.Controls.Add(this.streetLabel);
            this.contactInfoPanel.Controls.Add(this.extPhoneLabel);
            this.contactInfoPanel.Controls.Add(this.websiteLabel);
            this.contactInfoPanel.Controls.Add(this.phoneLabel);
            this.contactInfoPanel.Controls.Add(this.houseLabel);
            this.contactInfoPanel.Controls.Add(this.cityLabel);
            this.contactInfoPanel.Controls.Add(this.regionLabel);
            this.contactInfoPanel.Controls.Add(this.countryLabel);
            this.contactInfoPanel.Location = new System.Drawing.Point(28, 201);
            this.contactInfoPanel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.contactInfoPanel.Name = "contactInfoPanel";
            this.contactInfoPanel.Size = new System.Drawing.Size(657, 312);
            this.contactInfoPanel.TabIndex = 63;
            this.contactInfoPanel.Visible = false;
            // 
            // roomLabel
            // 
            this.roomLabel.AutoSize = true;
            this.roomLabel.Location = new System.Drawing.Point(409, 74);
            this.roomLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.roomLabel.Name = "roomLabel";
            this.roomLabel.Size = new System.Drawing.Size(76, 16);
            this.roomLabel.TabIndex = 22;
            this.roomLabel.Text = "Квартира :";
            // 
            // roomTextBox
            // 
            this.roomTextBox.Location = new System.Drawing.Point(413, 94);
            this.roomTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.roomTextBox.Name = "roomTextBox";
            this.roomTextBox.Size = new System.Drawing.Size(132, 22);
            this.roomTextBox.TabIndex = 21;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(377, 276);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(264, 22);
            this.emailTextBox.TabIndex = 20;
            // 
            // websiteTextBox
            // 
            this.websiteTextBox.Location = new System.Drawing.Point(27, 276);
            this.websiteTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.websiteTextBox.Name = "websiteTextBox";
            this.websiteTextBox.Size = new System.Drawing.Size(255, 22);
            this.websiteTextBox.TabIndex = 19;
            // 
            // extPhoneTextBox
            // 
            this.extPhoneTextBox.Location = new System.Drawing.Point(220, 201);
            this.extPhoneTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.extPhoneTextBox.Name = "extPhoneTextBox";
            this.extPhoneTextBox.Size = new System.Drawing.Size(165, 22);
            this.extPhoneTextBox.TabIndex = 17;
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(27, 201);
            this.phoneTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(165, 22);
            this.phoneTextBox.TabIndex = 16;
            // 
            // houseTextBox
            // 
            this.houseTextBox.Location = new System.Drawing.Point(220, 94);
            this.houseTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.houseTextBox.Name = "houseTextBox";
            this.houseTextBox.Size = new System.Drawing.Size(165, 22);
            this.houseTextBox.TabIndex = 15;
            // 
            // streetTextBox
            // 
            this.streetTextBox.Location = new System.Drawing.Point(27, 96);
            this.streetTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.streetTextBox.Name = "streetTextBox";
            this.streetTextBox.Size = new System.Drawing.Size(168, 22);
            this.streetTextBox.TabIndex = 14;
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(413, 31);
            this.cityTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(228, 22);
            this.cityTextBox.TabIndex = 13;
            // 
            // regionTextBox
            // 
            this.regionTextBox.Location = new System.Drawing.Point(220, 31);
            this.regionTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.regionTextBox.Name = "regionTextBox";
            this.regionTextBox.Size = new System.Drawing.Size(165, 22);
            this.regionTextBox.TabIndex = 12;
            // 
            // countryTextBox
            // 
            this.countryTextBox.Location = new System.Drawing.Point(27, 31);
            this.countryTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.countryTextBox.Name = "countryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(165, 22);
            this.countryTextBox.TabIndex = 11;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(373, 256);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(118, 16);
            this.emailLabel.TabIndex = 9;
            this.emailLabel.Text = "Адрес эл. почты :";
            // 
            // streetLabel
            // 
            this.streetLabel.AutoSize = true;
            this.streetLabel.Location = new System.Drawing.Point(23, 74);
            this.streetLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.streetLabel.Name = "streetLabel";
            this.streetLabel.Size = new System.Drawing.Size(54, 16);
            this.streetLabel.TabIndex = 8;
            this.streetLabel.Text = "Улица :";
            // 
            // extPhoneLabel
            // 
            this.extPhoneLabel.AutoSize = true;
            this.extPhoneLabel.Location = new System.Drawing.Point(216, 181);
            this.extPhoneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.extPhoneLabel.Name = "extPhoneLabel";
            this.extPhoneLabel.Size = new System.Drawing.Size(102, 16);
            this.extPhoneLabel.TabIndex = 7;
            this.extPhoneLabel.Text = "Доп. телефон :";
            // 
            // websiteLabel
            // 
            this.websiteLabel.AutoSize = true;
            this.websiteLabel.Location = new System.Drawing.Point(23, 256);
            this.websiteLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.websiteLabel.Name = "websiteLabel";
            this.websiteLabel.Size = new System.Drawing.Size(48, 16);
            this.websiteLabel.TabIndex = 6;
            this.websiteLabel.Text = "Сайт  :";
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(23, 181);
            this.phoneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(73, 16);
            this.phoneLabel.TabIndex = 4;
            this.phoneLabel.Text = "Телефон :";
            // 
            // houseLabel
            // 
            this.houseLabel.AutoSize = true;
            this.houseLabel.Location = new System.Drawing.Point(216, 74);
            this.houseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.houseLabel.Name = "houseLabel";
            this.houseLabel.Size = new System.Drawing.Size(39, 16);
            this.houseLabel.TabIndex = 3;
            this.houseLabel.Text = "Дом :";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(409, 11);
            this.cityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(52, 16);
            this.cityLabel.TabIndex = 2;
            this.cityLabel.Text = "Город :";
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(216, 11);
            this.regionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(124, 16);
            this.regionLabel.TabIndex = 1;
            this.regionLabel.Text = "Регион (область) :";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(23, 11);
            this.countryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(61, 16);
            this.countryLabel.TabIndex = 0;
            this.countryLabel.Text = "Страна :";
            // 
            // descrRichTextBox
            // 
            this.descrRichTextBox.Location = new System.Drawing.Point(13, 26);
            this.descrRichTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.descrRichTextBox.Name = "descrRichTextBox";
            this.descrRichTextBox.Size = new System.Drawing.Size(660, 67);
            this.descrRichTextBox.TabIndex = 64;
            this.descrRichTextBox.Text = "";
            // 
            // descrLabel
            // 
            this.descrLabel.AutoSize = true;
            this.descrLabel.Location = new System.Drawing.Point(13, 2);
            this.descrLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.descrLabel.Name = "descrLabel";
            this.descrLabel.Size = new System.Drawing.Size(110, 16);
            this.descrLabel.TabIndex = 65;
            this.descrLabel.Text = "Комментарий о ";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(123, 112);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 28);
            this.okButton.TabIndex = 66;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(467, 112);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 28);
            this.cancelButton.TabIndex = 67;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cancelButton_MouseClick);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.descrLabel);
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.descrRichTextBox);
            this.bottomPanel.Controls.Add(this.okButton);
            this.bottomPanel.Location = new System.Drawing.Point(9, 527);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(677, 151);
            this.bottomPanel.TabIndex = 68;
            // 
            // BalanceLabel
            // 
            this.BalanceLabel.AutoSize = true;
            this.BalanceLabel.Location = new System.Drawing.Point(400, 128);
            this.BalanceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BalanceLabel.Name = "BalanceLabel";
            this.BalanceLabel.Size = new System.Drawing.Size(61, 16);
            this.BalanceLabel.TabIndex = 69;
            this.BalanceLabel.Text = "Баланс :";
            this.BalanceLabel.Visible = false;
            // 
            // BalanceNumericUpDown
            // 
            this.BalanceNumericUpDown.Location = new System.Drawing.Point(475, 126);
            this.BalanceNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BalanceNumericUpDown.Name = "BalanceNumericUpDown";
            this.BalanceNumericUpDown.Size = new System.Drawing.Size(101, 22);
            this.BalanceNumericUpDown.TabIndex = 70;
            this.BalanceNumericUpDown.Visible = false;
            // 
            // filledBalanceLabel
            // 
            this.FilledBalanceLabel.AutoSize = true;
            this.FilledBalanceLabel.Location = new System.Drawing.Point(475, 128);
            this.FilledBalanceLabel.Name = "filledBalanceLabel";
            this.FilledBalanceLabel.Size = new System.Drawing.Size(14, 16);
            this.FilledBalanceLabel.TabIndex = 71;
            // 
            // AddContragentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(711, 695);
            this.Controls.Add(this.FilledBalanceLabel);
            this.Controls.Add(this.BalanceNumericUpDown);
            this.Controls.Add(this.BalanceLabel);
            this.Controls.Add(this.entityComboBox);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.contactInfoPanel);
            this.Controls.Add(this.addContactInfoButton);
            this.Controls.Add(this.codeBackPanel);
            this.Controls.Add(this.contragentNameBackPanel);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.contragentNameLabel);
            this.Controls.Add(this.entityLabel);
            this.Controls.Add(this.contragentNameStarLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "AddContragentForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.AddcontragentForm_Load);
            this.contragentNameBackPanel.ResumeLayout(false);
            this.contragentNameBackPanel.PerformLayout();
            this.codeBackPanel.ResumeLayout(false);
            this.codeBackPanel.PerformLayout();
            this.contactInfoPanel.ResumeLayout(false);
            this.contactInfoPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BalanceNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox entityComboBox;
        private System.Windows.Forms.Label entityLabel;
        private System.Windows.Forms.Label contragentNameLabel;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.Label contragentNameStarLabel;
        private System.Windows.Forms.Panel contragentNameBackPanel;
        private System.Windows.Forms.Panel codeBackPanel;
        private System.Windows.Forms.TextBox contragentNameTextBox;
        private System.Windows.Forms.Button addContactInfoButton;
        private System.Windows.Forms.Panel contactInfoPanel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.TextBox websiteTextBox;
        private System.Windows.Forms.TextBox extPhoneTextBox;
        private System.Windows.Forms.TextBox phoneTextBox;
        private System.Windows.Forms.TextBox houseTextBox;
        private System.Windows.Forms.TextBox streetTextBox;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.TextBox regionTextBox;
        private System.Windows.Forms.TextBox countryTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label streetLabel;
        private System.Windows.Forms.Label extPhoneLabel;
        private System.Windows.Forms.Label websiteLabel;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.Label houseLabel;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.RichTextBox descrRichTextBox;
        private System.Windows.Forms.Label descrLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.MaskedTextBox codeMaskedTextBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label roomLabel;
        private System.Windows.Forms.TextBox roomTextBox;
        private System.Windows.Forms.Label BalanceLabel;
        private System.Windows.Forms.NumericUpDown BalanceNumericUpDown;
        private System.Windows.Forms.Label FilledBalanceLabel;
    }
}