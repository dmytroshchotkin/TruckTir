namespace PartsApp
{
    partial class DisableEmployeeForm
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
            this.disableActionLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.hireDateTimeLabel = new System.Windows.Forms.Label();
            this.accessLevelLabel = new System.Windows.Forms.Label();
            this.acceptDisableButton = new System.Windows.Forms.Button();
            this.disableDateTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // disableActionLabel
            // 
            this.disableActionLabel.AutoSize = true;
            this.disableActionLabel.Location = new System.Drawing.Point(19, 21);
            this.disableActionLabel.Name = "disableActionLabel";
            this.disableActionLabel.Size = new System.Drawing.Size(260, 16);
            this.disableActionLabel.TabIndex = 0;
            this.disableActionLabel.Text = "Подтвердите блокировку доступа для сотрудника:";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(19, 69);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(39, 16);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Имя: ";
            // 
            // hireDateTimeLabel
            // 
            this.hireDateTimeLabel.AutoSize = true;
            this.hireDateTimeLabel.Location = new System.Drawing.Point(19, 85);
            this.hireDateTimeLabel.Name = "hireDateTimeLabel";
            this.hireDateTimeLabel.Size = new System.Drawing.Size(127, 16);
            this.hireDateTimeLabel.TabIndex = 2;
            this.hireDateTimeLabel.Text = "Принят на работу ";
            // 
            // accessLevelLabel
            // 
            this.accessLevelLabel.AutoSize = true;
            this.accessLevelLabel.Location = new System.Drawing.Point(19, 101);
            this.accessLevelLabel.Name = "accessLevelLabel";
            this.accessLevelLabel.Size = new System.Drawing.Size(112, 16);
            this.accessLevelLabel.TabIndex = 3;
            this.accessLevelLabel.Text = "Право доступа: ";
            // 
            // acceptDisableButton
            // 
            this.acceptDisableButton.Location = new System.Drawing.Point(22, 184);
            this.acceptDisableButton.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.acceptDisableButton.Name = "acceptDisableButton";
            this.acceptDisableButton.Size = new System.Drawing.Size(257, 27);
            this.acceptDisableButton.TabIndex = 4;
            this.acceptDisableButton.Text = "Заблокировать";
            this.acceptDisableButton.UseVisualStyleBackColor = true;
            this.acceptDisableButton.Click += new System.EventHandler(this.OnAcceptDisableButton);
            // 
            // disableDateTimeLabel
            // 
            this.disableDateTimeLabel.AutoSize = true;
            this.disableDateTimeLabel.Location = new System.Drawing.Point(19, 143);
            this.disableDateTimeLabel.Name = "disableDateTimeLabel";
            this.disableDateTimeLabel.Size = new System.Drawing.Size(207, 16);
            this.disableDateTimeLabel.TabIndex = 5;
            this.disableDateTimeLabel.Text = "Доступ будет заблокирован ";
            // 
            // DisableEmployeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 253);
            this.Controls.Add(this.disableDateTimeLabel);
            this.Controls.Add(this.acceptDisableButton);
            this.Controls.Add(this.accessLevelLabel);
            this.Controls.Add(this.hireDateTimeLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.disableActionLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.Name = "DisableEmployeeForm";
            this.Text = "Заблокировать доступ для сотрудника";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label disableActionLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label hireDateTimeLabel;
        private System.Windows.Forms.Label accessLevelLabel;
        private System.Windows.Forms.Button acceptDisableButton;
        private System.Windows.Forms.Label disableDateTimeLabel;
    }
}