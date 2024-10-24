using PartsApp.Models;
using PartsApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartsApp
{
    public partial class ConfigSaveExcelFilesForm : Form
    {
        private const string _salesPathSetting = "SalesFilesSavePath";
        private const string _purchasesPathSetting = "PurchasesFilesSavePath";

        public ConfigSaveExcelFilesForm()
        {
            InitializeComponent();
            DisplayCurrentSaveExcelFilesDirectories();
        }

        private void DisplayCurrentSaveExcelFilesDirectories()
        {
            var salesPath = ConfigurationManager.AppSettings[_salesPathSetting];
            CurrentSalesPathContentLabel.Text = salesPath;

            var purchasesPath = ConfigurationManager.AppSettings[_purchasesPathSetting];
            if (!string.IsNullOrWhiteSpace(purchasesPath))
            {
                CurrentPurchasesPathContentLabel.Text = purchasesPath;
                SetSavingPurchasesCheckBox.Checked = true;
            }
        }

        private void OnChangeSalesExcelDirectoryButtonClick(object sender, EventArgs e)
        {
            ChooseNewDirectory(_salesPathSetting);
        }

        private void OnChangePurchasesDirectoryClick(object sender, EventArgs e)
        {
            ChooseNewDirectory(_purchasesPathSetting);
        }

        private void ChooseNewDirectory(string setting)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку:";
                folderDialog.ShowNewFolderButton = true;

                var result = folderDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string newPath = folderDialog.SelectedPath;
                    var input = AcceptChangeOfDirectory(newPath);
                    if (input == DialogResult.Yes)
                    {
                        UpdateConfigFile(newPath, setting);
                        UpdateContentLabel(newPath, setting);
                    }
                }
            }
        }

        private void UpdateConfigFile(string newPath, string setting)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[setting].Value = newPath;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void UpdateContentLabel(string newPath, string setting)
        {
            if (setting == _salesPathSetting)
            {
                CurrentSalesPathContentLabel.Text = newPath;
            }
            else if (setting == _purchasesPathSetting)
            {
                CurrentPurchasesPathContentLabel.Text = newPath;
            }
        }

        private DialogResult AcceptChangeOfDirectory(string newPath)
        {
            return MessageBox.Show(
            $"{newPath}\n\nУстановить эту папку для сохранения файлов Excel?",
                    "Подтвердите изменение папки",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
        }

        private void OnSetSavingPurchasesCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (SetSavingPurchasesCheckBox.Checked) 
            {
                CurrentPurchasesPathContentLabel.Visible = ChangePurchasesDirectoryButton.Visible = DeletePurchasesPathButton.Visible = true;
            }
            else
            {
                CurrentPurchasesPathContentLabel.Visible = ChangePurchasesDirectoryButton.Visible = DeletePurchasesPathButton.Visible = false;
            }
        }

        private void DeletePurchasePathData()
        {
            UpdateConfigFile(string.Empty, _purchasesPathSetting);
            CurrentPurchasesPathContentLabel.Text = string.Empty;
        }

        private void OnDeletePurchasesPathButtonClick(object sender, EventArgs e)
        {
            DeletePurchasePathData();
            SetSavingPurchasesCheckBox.Checked = false;
        }

        private void OnConfigSaveExcelFilesFormClosing(object sender, EventArgs e)
        {
            if (!SetSavingPurchasesCheckBox.Checked)
            {
                DeletePurchasePathData();
            }            
        }
    }
}
