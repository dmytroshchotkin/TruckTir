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
            this.entityComboBox.Location = new System.Drawing.Point(143, 20);
            this.entityComboBox.Name = "entityComboBox";
            this.entityComboBox.Size = new System.Drawing.Size(88, 21);
            this.entityComboBox.TabIndex = 0;
            // 
            // entityLabel
            // 
            this.entityLabel.AutoSize = true;
            this.entityLabel.Location = new System.Drawing.Point(28, 23);
            this.entityLabel.Name = "entityLabel";
            this.entityLabel.Size = new System.Drawing.Size(89, 13);
            this.entityLabel.TabIndex = 1;
            this.entityLabel.Text = "Юр./Физ. лицо :";
            // 
            // contragentNameLabel
            // 
            this.contragentNameLabel.AutoSize = true;
            this.contragentNameLabel.Location = new System.Drawing.Point(28, 64);
            this.contragentNameLabel.Name = "contragentNameLabel";
            this.contragentNameLabel.Size = new System.Drawing.Size(100, 13);
            this.contragentNameLabel.TabIndex = 2;
            this.contragentNameLabel.Text = "ФИО (Компания) :";
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(28, 104);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(73, 13);
            this.codeLabel.TabIndex = 3;
            this.codeLabel.Text = "ИНН/ОКПО :";
            // 
            // contragentNameStarLabel
            // 
            this.contragentNameStarLabel.AutoSize = true;
            this.contragentNameStarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.contragentNameStarLabel.Location = new System.Drawing.Point(16, 55);
            this.contragentNameStarLabel.Name = "contragentNameStarLabel";
            this.contragentNameStarLabel.Size = new System.Drawing.Size(20, 25);
            this.contragentNameStarLabel.TabIndex = 59;
            this.contragentNameStarLabel.Text = "*";
            // 
            // contragentNameBackPanel
            // 
            this.contragentNameBackPanel.Controls.Add(this.contragentNameTextBox);
            this.contragentNameBackPanel.Location = new System.Drawing.Point(141, 56);
            this.contragentNameBackPanel.Name = "contragentNameBackPanel";
            this.contragentNameBackPanel.Size = new System.Drawing.Size(214, 24);
            this.contragentNameBackPanel.TabIndex = 61;
            // 
            // contragentNameTextBox
            // 
            this.contragentNameTextBox.Location = new System.Drawing.Point(2, 2);
            this.contragentNameTextBox.Name = "contragentNameTextBox";
            this.contragentNameTextBox.Size = new System.Drawing.Size(210, 20);
            this.contragentNameTextBox.TabIndex = 62;
            this.contragentNameTextBox.Leave += new System.EventHandler(this.contragentNameTextBox_Leave);
            // 
            // codeBackPanel
            // 
            this.codeBackPanel.Controls.Add(this.codeMaskedTextBox);
            this.codeBackPanel.Location = new System.Drawing.Point(141, 93);
            this.codeBackPanel.Name = "codeBackPanel";
            this.codeBackPanel.Size = new System.Drawing.Size(77, 24);
            this.codeBackPanel.TabIndex = 61;
            // 
            // codeMaskedTextBox
            // 
            this.codeMaskedTextBox.Location = new System.Drawing.Point(2, 2);
            this.codeMaskedTextBox.Mask = "0000000099";
            this.codeMaskedTextBox.Name = "codeMaskedTextBox";
            this.codeMaskedTextBox.Size = new System.Drawing.Size(73, 20);
            this.codeMaskedTextBox.TabIndex = 69;
            this.codeMaskedTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.codeMaskedTextBox_MouseClick);
            this.codeMaskedTextBox.Leave += new System.EventHandler(this.codeMaskedTextBox_Leave);
            // 
            // addContactInfoButton
            // 
            this.addContactInfoButton.Location = new System.Drawing.Point(21, 141);
            this.addContactInfoButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.addContactInfoButton.Name = "addContactInfoButton";
            this.addContactInfoButton.Size = new System.Drawing.Size(253, 22);
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
            this.contactInfoPanel.Location = new System.Drawing.Point(21, 163);
            this.contactInfoPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.contactInfoPanel.Name = "contactInfoPanel";
            this.contactInfoPanel.Size = new System.Drawing.Size(493, 254);
            this.contactInfoPanel.TabIndex = 63;
            this.contactInfoPanel.Visible = false;
            // 
            // roomLabel
            // 
            this.roomLabel.AutoSize = true;
            this.roomLabel.Location = new System.Drawing.Point(307, 60);
            this.roomLabel.Name = "roomLabel";
            this.roomLabel.Size = new System.Drawing.Size(61, 13);
            this.roomLabel.TabIndex = 22;
            this.roomLabel.Text = "Квартира :";
            // 
            // roomTextBox
            // 
            this.roomTextBox.Location = new System.Drawing.Point(310, 76);
            this.roomTextBox.Name = "roomTextBox";
            this.roomTextBox.Size = new System.Drawing.Size(100, 20);
            this.roomTextBox.TabIndex = 21;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(283, 224);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(199, 20);
            this.emailTextBox.TabIndex = 20;
            // 
            // websiteTextBox
            // 
            this.websiteTextBox.Location = new System.Drawing.Point(20, 224);
            this.websiteTextBox.Name = "websiteTextBox";
            this.websiteTextBox.Size = new System.Drawing.Size(192, 20);
            this.websiteTextBox.TabIndex = 19;
            // 
            // extPhoneTextBox
            // 
            this.extPhoneTextBox.Location = new System.Drawing.Point(165, 163);
            this.extPhoneTextBox.Name = "extPhoneTextBox";
            this.extPhoneTextBox.Size = new System.Drawing.Size(125, 20);
            this.extPhoneTextBox.TabIndex = 17;
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(20, 163);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(125, 20);
            this.phoneTextBox.TabIndex = 16;
            // 
            // houseTextBox
            // 
            this.houseTextBox.Location = new System.Drawing.Point(165, 76);
            this.houseTextBox.Name = "houseTextBox";
            this.houseTextBox.Size = new System.Drawing.Size(125, 20);
            this.houseTextBox.TabIndex = 15;
            // 
            // streetTextBox
            // 
            this.streetTextBox.Location = new System.Drawing.Point(20, 78);
            this.streetTextBox.Name = "streetTextBox";
            this.streetTextBox.Size = new System.Drawing.Size(127, 20);
            this.streetTextBox.TabIndex = 14;
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(310, 25);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(172, 20);
            this.cityTextBox.TabIndex = 13;
            // 
            // regionTextBox
            // 
            this.regionTextBox.Location = new System.Drawing.Point(165, 25);
            this.regionTextBox.Name = "regionTextBox";
            this.regionTextBox.Size = new System.Drawing.Size(125, 20);
            this.regionTextBox.TabIndex = 12;
            // 
            // countryTextBox
            // 
            this.countryTextBox.Location = new System.Drawing.Point(20, 25);
            this.countryTextBox.Name = "countryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(125, 20);
            this.countryTextBox.TabIndex = 11;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(280, 208);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(95, 13);
            this.emailLabel.TabIndex = 9;
            this.emailLabel.Text = "Адрес эл. почты :";
            // 
            // streetLabel
            // 
            this.streetLabel.AutoSize = true;
            this.streetLabel.Location = new System.Drawing.Point(17, 60);
            this.streetLabel.Name = "streetLabel";
            this.streetLabel.Size = new System.Drawing.Size(45, 13);
            this.streetLabel.TabIndex = 8;
            this.streetLabel.Text = "Улица :";
            // 
            // extPhoneLabel
            // 
            this.extPhoneLabel.AutoSize = true;
            this.extPhoneLabel.Location = new System.Drawing.Point(162, 147);
            this.extPhoneLabel.Name = "extPhoneLabel";
            this.extPhoneLabel.Size = new System.Drawing.Size(83, 13);
            this.extPhoneLabel.TabIndex = 7;
            this.extPhoneLabel.Text = "Доп. телефон :";
            // 
            // websiteLabel
            // 
            this.websiteLabel.AutoSize = true;
            this.websiteLabel.Location = new System.Drawing.Point(17, 208);
            this.websiteLabel.Name = "websiteLabel";
            this.websiteLabel.Size = new System.Drawing.Size(40, 13);
            this.websiteLabel.TabIndex = 6;
            this.websiteLabel.Text = "Сайт  :";
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(17, 147);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(58, 13);
            this.phoneLabel.TabIndex = 4;
            this.phoneLabel.Text = "Телефон :";
            // 
            // houseLabel
            // 
            this.houseLabel.AutoSize = true;
            this.houseLabel.Location = new System.Drawing.Point(162, 60);
            this.houseLabel.Name = "houseLabel";
            this.houseLabel.Size = new System.Drawing.Size(36, 13);
            this.houseLabel.TabIndex = 3;
            this.houseLabel.Text = "Дом :";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(307, 9);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(43, 13);
            this.cityLabel.TabIndex = 2;
            this.cityLabel.Text = "Город :";
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(162, 9);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(99, 13);
            this.regionLabel.TabIndex = 1;
            this.regionLabel.Text = "Регион (область) :";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(17, 9);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(49, 13);
            this.countryLabel.TabIndex = 0;
            this.countryLabel.Text = "Страна :";
            // 
            // descrRichTextBox
            // 
            this.descrRichTextBox.Location = new System.Drawing.Point(10, 21);
            this.descrRichTextBox.Name = "descrRichTextBox";
            this.descrRichTextBox.Size = new System.Drawing.Size(496, 55);
            this.descrRichTextBox.TabIndex = 64;
            this.descrRichTextBox.Text = "";
            // 
            // descrLabel
            // 
            this.descrLabel.AutoSize = true;
            this.descrLabel.Location = new System.Drawing.Point(10, 2);
            this.descrLabel.Name = "descrLabel";
            this.descrLabel.Size = new System.Drawing.Size(89, 13);
            this.descrLabel.TabIndex = 65;
            this.descrLabel.Text = "Комментарий о ";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(92, 91);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 66;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.okButton_MouseClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(350, 91);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
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
            this.bottomPanel.Location = new System.Drawing.Point(7, 428);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(508, 123);
            this.bottomPanel.TabIndex = 68;
            // 
            // BalanceLabel
            // 
            this.BalanceLabel.AutoSize = true;
            this.BalanceLabel.Location = new System.Drawing.Point(300, 104);
            this.BalanceLabel.Name = "BalanceLabel";
            this.BalanceLabel.Size = new System.Drawing.Size(50, 13);
            this.BalanceLabel.TabIndex = 69;
            this.BalanceLabel.Text = "Баланс :";
            // 
            // BalanceNumericUpDown
            // 
            this.BalanceNumericUpDown.Location = new System.Drawing.Point(356, 102);
            this.BalanceNumericUpDown.Name = "BalanceNumericUpDown";
            this.BalanceNumericUpDown.Size = new System.Drawing.Size(76, 20);
            this.BalanceNumericUpDown.TabIndex = 70;
            this.BalanceNumericUpDown.Minimum = decimal.MinValue;
            this.BalanceNumericUpDown.Maximum = decimal.MaxValue;
            // 
            // AddContragentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(533, 565);
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
    }
}