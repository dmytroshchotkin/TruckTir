namespace PartsApp
{
    partial class ConfigSaveExcelFilesForm
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
            this.CurrentSalesPathLabel = new System.Windows.Forms.Label();
            this.ChangeSalesDirectoryButton = new System.Windows.Forms.Button();
            this.CurrentSalesPathContentLabel = new System.Windows.Forms.Label();
            this.SetSavingPurchasesCheckBox = new System.Windows.Forms.CheckBox();
            this.CurrentPurchasesPathContentLabel = new System.Windows.Forms.Label();
            this.ChangePurchasesDirectoryButton = new System.Windows.Forms.Button();
            this.DeletePurchasesPathButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentSalesPathLabel
            // 
            this.CurrentSalesPathLabel.AutoSize = true;
            this.CurrentSalesPathLabel.Location = new System.Drawing.Point(15, 15);
            this.CurrentSalesPathLabel.Name = "CurrentSalesPathLabel";
            this.CurrentSalesPathLabel.Size = new System.Drawing.Size(300, 16);
            this.CurrentSalesPathLabel.TabIndex = 0;
            this.CurrentSalesPathLabel.Text = "Папка для сохранения расходных накладных:";
            // 
            // ChangeSalesDirectoryButton
            // 
            this.ChangeSalesDirectoryButton.Location = new System.Drawing.Point(15, 60);
            this.ChangeSalesDirectoryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChangeSalesDirectoryButton.Name = "ChangeSalesDirectoryButton";
            this.ChangeSalesDirectoryButton.Size = new System.Drawing.Size(117, 25);
            this.ChangeSalesDirectoryButton.TabIndex = 1;
            this.ChangeSalesDirectoryButton.Text = "Изменить";
            this.ChangeSalesDirectoryButton.UseVisualStyleBackColor = true;
            this.ChangeSalesDirectoryButton.Click += new System.EventHandler(this.OnChangeSalesExcelDirectoryButtonClick);
            // 
            // CurrentSalesPathContentLabel
            // 
            this.CurrentSalesPathContentLabel.AutoSize = true;
            this.CurrentSalesPathContentLabel.Location = new System.Drawing.Point(15, 33);
            this.CurrentSalesPathContentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentSalesPathContentLabel.Name = "CurrentSalesPathContentLabel";
            this.CurrentSalesPathContentLabel.Size = new System.Drawing.Size(0, 16);
            this.CurrentSalesPathContentLabel.TabIndex = 2;
            // 
            // SetSavingPurchasesCheckBox
            // 
            this.SetSavingPurchasesCheckBox.AutoSize = true;
            this.SetSavingPurchasesCheckBox.Location = new System.Drawing.Point(15, 120);
            this.SetSavingPurchasesCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.SetSavingPurchasesCheckBox.Name = "SetSavingPurchasesCheckBox";
            this.SetSavingPurchasesCheckBox.Size = new System.Drawing.Size(246, 20);
            this.SetSavingPurchasesCheckBox.TabIndex = 3;
            this.SetSavingPurchasesCheckBox.Text = "Сохранять приходные накладные";
            this.SetSavingPurchasesCheckBox.UseVisualStyleBackColor = true;
            this.SetSavingPurchasesCheckBox.CheckedChanged += new System.EventHandler(this.OnSetSavingPurchasesCheckBoxCheckedChanged);
            // 
            // CurrentPurchasesPathContentLabel
            // 
            this.CurrentPurchasesPathContentLabel.AutoSize = true;
            this.CurrentPurchasesPathContentLabel.Location = new System.Drawing.Point(15, 140);
            this.CurrentPurchasesPathContentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentPurchasesPathContentLabel.Name = "CurrentPurchasesPathContentLabel";
            this.CurrentPurchasesPathContentLabel.Size = new System.Drawing.Size(0, 16);
            this.CurrentPurchasesPathContentLabel.TabIndex = 5;
            this.CurrentPurchasesPathContentLabel.Visible = false;
            // 
            // ChangePurchasesDirectoryButton
            // 
            this.ChangePurchasesDirectoryButton.Location = new System.Drawing.Point(15, 167);
            this.ChangePurchasesDirectoryButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChangePurchasesDirectoryButton.Name = "ChangePurchasesDirectoryButton";
            this.ChangePurchasesDirectoryButton.Size = new System.Drawing.Size(117, 25);
            this.ChangePurchasesDirectoryButton.TabIndex = 6;
            this.ChangePurchasesDirectoryButton.Text = "Изменить";
            this.ChangePurchasesDirectoryButton.UseVisualStyleBackColor = true;
            this.ChangePurchasesDirectoryButton.Visible = false;
            this.ChangePurchasesDirectoryButton.Click += new System.EventHandler(this.OnChangePurchasesDirectoryClick);
            // 
            // DeletePurchasesPathButton
            // 
            this.DeletePurchasesPathButton.Location = new System.Drawing.Point(158, 167);
            this.DeletePurchasesPathButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeletePurchasesPathButton.Name = "DeletePurchasesPathButton";
            this.DeletePurchasesPathButton.Size = new System.Drawing.Size(117, 25);
            this.DeletePurchasesPathButton.TabIndex = 7;
            this.DeletePurchasesPathButton.Text = "Удалить";
            this.DeletePurchasesPathButton.UseVisualStyleBackColor = true;
            this.DeletePurchasesPathButton.Visible = false;
            this.DeletePurchasesPathButton.Click += new System.EventHandler(this.OnDeletePurchasesPathButtonClick);
            // 
            // ConfigSaveExcelFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 205);
            this.Controls.Add(this.DeletePurchasesPathButton);
            this.Controls.Add(this.ChangePurchasesDirectoryButton);
            this.Controls.Add(this.CurrentPurchasesPathContentLabel);
            this.Controls.Add(this.SetSavingPurchasesCheckBox);
            this.Controls.Add(this.CurrentSalesPathContentLabel);
            this.Controls.Add(this.ChangeSalesDirectoryButton);
            this.Controls.Add(this.CurrentSalesPathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "ConfigSaveExcelFilesForm";
            this.Text = "Выбор папки для сохранения Excel файлов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnConfigSaveExcelFilesFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentSalesPathLabel;
        private System.Windows.Forms.Button ChangeSalesDirectoryButton;
        private System.Windows.Forms.Label CurrentSalesPathContentLabel;
        private System.Windows.Forms.CheckBox SetSavingPurchasesCheckBox;
        private System.Windows.Forms.Label CurrentPurchasesPathContentLabel;
        private System.Windows.Forms.Button ChangePurchasesDirectoryButton;
        private System.Windows.Forms.Button DeletePurchasesPathButton;
    }
}