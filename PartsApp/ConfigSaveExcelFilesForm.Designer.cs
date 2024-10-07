using System.Windows.Forms;

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
            this.ChangeSalesDirectoryButton = new System.Windows.Forms.Button();
            this.ChangePurchasesDirectoryButton = new System.Windows.Forms.Button();
            this.CurrentSalesPathTextBox = new System.Windows.Forms.TextBox();
            this.CurrentPurchasesPathTextBox = new System.Windows.Forms.TextBox();
            this.SaveSalesCheckBox = new System.Windows.Forms.CheckBox();
            this.SavePurchasesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ChangeSalesDirectoryButton
            // 
            this.ChangeSalesDirectoryButton.Location = new System.Drawing.Point(247, 29);
            this.ChangeSalesDirectoryButton.Margin = new System.Windows.Forms.Padding(2);
            this.ChangeSalesDirectoryButton.Name = "ChangeSalesDirectoryButton";
            this.ChangeSalesDirectoryButton.Size = new System.Drawing.Size(26, 20);
            this.ChangeSalesDirectoryButton.TabIndex = 1;
            this.ChangeSalesDirectoryButton.Text = "...";
            this.ChangeSalesDirectoryButton.UseVisualStyleBackColor = true;
            this.ChangeSalesDirectoryButton.Click += new System.EventHandler(this.OnChangeSalesExcelDirectoryButtonClick);
            // 
            // ChangePurchasesDirectoryButton
            // 
            this.ChangePurchasesDirectoryButton.Location = new System.Drawing.Point(247, 78);
            this.ChangePurchasesDirectoryButton.Margin = new System.Windows.Forms.Padding(2);
            this.ChangePurchasesDirectoryButton.Name = "ChangePurchasesDirectoryButton";
            this.ChangePurchasesDirectoryButton.Size = new System.Drawing.Size(26, 20);
            this.ChangePurchasesDirectoryButton.TabIndex = 6;
            this.ChangePurchasesDirectoryButton.Text = "...";
            this.ChangePurchasesDirectoryButton.UseVisualStyleBackColor = true;
            this.ChangePurchasesDirectoryButton.Click += new System.EventHandler(this.OnChangePurchasesDirectoryClick);
            // 
            // CurrentSalesPathTextBox
            // 
            this.CurrentSalesPathTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.CurrentSalesPathTextBox.Location = new System.Drawing.Point(14, 28);
            this.CurrentSalesPathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CurrentSalesPathTextBox.Name = "CurrentSalesPathTextBox";
            this.CurrentSalesPathTextBox.Size = new System.Drawing.Size(229, 20);
            this.CurrentSalesPathTextBox.TabIndex = 8;
            // 
            // CurrentPurchasesPathTextBox
            // 
            this.CurrentPurchasesPathTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.CurrentPurchasesPathTextBox.Location = new System.Drawing.Point(14, 78);
            this.CurrentPurchasesPathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.CurrentPurchasesPathTextBox.Name = "CurrentPurchasesPathTextBox";
            this.CurrentPurchasesPathTextBox.Size = new System.Drawing.Size(229, 20);
            this.CurrentPurchasesPathTextBox.TabIndex = 10;
            // 
            // SaveSalesCheckBox
            // 
            this.SaveSalesCheckBox.AutoSize = true;
            this.SaveSalesCheckBox.Location = new System.Drawing.Point(14, 12);
            this.SaveSalesCheckBox.Name = "SaveSalesCheckBox";
            this.SaveSalesCheckBox.Size = new System.Drawing.Size(196, 17);
            this.SaveSalesCheckBox.TabIndex = 13;
            this.SaveSalesCheckBox.Text = "Сохранять расходные накладные";
            this.SaveSalesCheckBox.UseVisualStyleBackColor = true;
            // 
            // SavePurchasesCheckBox
            // 
            this.SavePurchasesCheckBox.AutoSize = true;
            this.SavePurchasesCheckBox.Location = new System.Drawing.Point(14, 62);
            this.SavePurchasesCheckBox.Name = "SavePurchasesCheckBox";
            this.SavePurchasesCheckBox.Size = new System.Drawing.Size(196, 17);
            this.SavePurchasesCheckBox.TabIndex = 14;
            this.SavePurchasesCheckBox.Text = "Сохранять приходные накладные";
            this.SavePurchasesCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfigSaveExcelFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 152);
            this.Controls.Add(this.SavePurchasesCheckBox);
            this.Controls.Add(this.SaveSalesCheckBox);
            this.Controls.Add(this.CurrentPurchasesPathTextBox);
            this.Controls.Add(this.CurrentSalesPathTextBox);
            this.Controls.Add(this.ChangePurchasesDirectoryButton);
            this.Controls.Add(this.ChangeSalesDirectoryButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ConfigSaveExcelFilesForm";
            this.Text = "Настройки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ChangeSalesDirectoryButton;
        private System.Windows.Forms.Button ChangePurchasesDirectoryButton;
        private System.Windows.Forms.TextBox CurrentSalesPathTextBox;
        private System.Windows.Forms.TextBox CurrentPurchasesPathTextBox;
        private System.Windows.Forms.CheckBox SaveSalesCheckBox;
        private System.Windows.Forms.CheckBox SavePurchasesCheckBox;
    }
}