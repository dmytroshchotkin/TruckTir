using PartsApp.ExcelHelper;
using System;
using System.Configuration;
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

            SaveSalesCheckBox.CheckedChanged += OnSaveSalesCheckBoxCheckedChanged;
            SavePurchasesCheckBox.CheckedChanged += OnSavePurchasesCheckBoxCheckedChanged;
        }

        private void DisplayCurrentSaveExcelFilesDirectories()
        {
            DisplayCurrentSalesSaveExcelFileDirectory();
            DisplayCurrentPurchasesSaveExcelFileDirectory();
        }

        private void DisplayCurrentSalesSaveExcelFileDirectory()
        {
            var salesPath = ConfigurationManager.AppSettings[_salesPathSetting];
            if (!string.IsNullOrWhiteSpace(salesPath))
            {
                CurrentSalesPathTextBox.Text = salesPath;
                SaveSalesCheckBox.Checked = true;
                ChangeSalesDirectoryButton.Enabled = true;
            }
            else
            {
                CurrentSalesPathTextBox.Text = ExcelFilesStorageHelper.SalesFilesPath;
                CurrentSalesPathTextBox.Enabled = false;
                SaveSalesCheckBox.Checked = false;
                ChangeSalesDirectoryButton.Enabled = false;
            }
        }

        private void DisplayCurrentPurchasesSaveExcelFileDirectory()
        {
            var purchasesPath = ConfigurationManager.AppSettings[_purchasesPathSetting];
            if (!string.IsNullOrWhiteSpace(purchasesPath))
            {
                CurrentPurchasesPathTextBox.Text = purchasesPath;
                SavePurchasesCheckBox.Checked = true;
                ChangePurchasesDirectoryButton.Enabled = true;
            }
            else
            {
                CurrentPurchasesPathTextBox.Text = ExcelFilesStorageHelper.PurchasesFilesPath;
                CurrentPurchasesPathTextBox.Enabled = false;
                SavePurchasesCheckBox.Checked = false;
                ChangePurchasesDirectoryButton.Enabled = false;
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
                        ExcelFilesStorageHelper.UpdateExcelFilesPaths();
                        UpdateContentLabel(newPath, setting);
                        MessageBox.Show("Папка успешно изменена");
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
                CurrentSalesPathTextBox.Text = newPath;
            }
            else if (setting == _purchasesPathSetting)
            {
                CurrentPurchasesPathTextBox.Text = newPath;
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

        private void OnSaveSalesCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (SaveSalesCheckBox.Checked)
            {
                CurrentSalesPathTextBox.Enabled = true;
                ChangeSalesDirectoryButton.Enabled = true;
            }
            else
            {                
                CurrentSalesPathTextBox.Text = ExcelFilesStorageHelper.TempSalesFilesPath;
                UpdateConfigFile(string.Empty, _salesPathSetting);
                ExcelFilesStorageHelper.UpdateExcelFilesPaths();

                CurrentSalesPathTextBox.Enabled = false;
                ChangeSalesDirectoryButton.Enabled = false;
            }
        }

        private void OnSavePurchasesCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (SavePurchasesCheckBox.Checked)
            {
                CurrentPurchasesPathTextBox.Enabled = true;
                ChangePurchasesDirectoryButton.Enabled = true;
            }
            else
            {
                CurrentPurchasesPathTextBox.Text = ExcelFilesStorageHelper.TempPurchasesFilesPath;
                UpdateConfigFile(string.Empty, _purchasesPathSetting);
                ExcelFilesStorageHelper.UpdateExcelFilesPaths();

                CurrentPurchasesPathTextBox.Enabled = false;
                ChangePurchasesDirectoryButton.Enabled = false;
            }
        }
    }
}
