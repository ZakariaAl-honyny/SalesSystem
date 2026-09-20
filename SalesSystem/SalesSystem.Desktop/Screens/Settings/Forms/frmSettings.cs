using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Settings;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    public partial class frmSettings : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private Dictionary<string, string> _originalSettings = new();

        public frmSettings()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmSettings(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async void frmSettings_Load(object sender, EventArgs e)
        {
            try
            {
                await _LoadSettingsAsync();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
                System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Load
        // ════════════════════════════════════════════════════════════════════════════════

        private async Task _LoadSettingsAsync()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<ISettingsApiService>().GetAllSystemSettingsAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    _originalSettings = new Dictionary<string, string>(result.Value);
                    _MapFromDictionary(result.Value);
                }
                else
                {
                    MessageBox.Show(
                        "فشل في تحميل إعدادات النظام: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الإعدادات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Save
        // ════════════════════════════════════════════════════════════════════════════════

        private async void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            btnSave.Enabled = false;
            try
            {
                var settings = _BuildDictionary();
                var result = await _serviceProvider.GetRequiredService<ISettingsApiService>().UpdateSystemSettingsAsync(settings);

                if (result.IsSuccess)
                {
                    _originalSettings = new Dictionary<string, string>(settings);
                    _serviceProvider.GetRequiredService<ISettingsApiService>().RefreshCache();

                    MessageBox.Show(
                        "تم حفظ إعدادات النظام بنجاح.",
                        "تم الحفظ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "فشل في حفظ إعدادات النظام: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ الإعدادات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Backup Tab
        // ════════════════════════════════════════════════════════════════════════════════

        private async void btnBrowseBackupPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtBackupPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private async void btnCreateBackup_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            btnCreateBackup.Enabled = false;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IBackupApiService>().CreateBackupAsync();

                if (result.IsSuccess)
                {
                    MessageBox.Show(
                        "تم إنشاء النسخة الاحتياطية بنجاح.",
                        "تم",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    await _RefreshBackupListAsync();
                }
                else
                {
                    MessageBox.Show(
                        "فشل في إنشاء النسخة الاحتياطية: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إنشاء النسخة الاحتياطية: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreateBackup.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private async void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            if (lstBackupFiles.SelectedItem == null)
            {
                MessageBox.Show("الرجاء اختيار نسخة احتياطية من القائمة.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "هل أنت متأكد من استعادة النسخة الاحتياطية؟ سيتم استبدال جميع البيانات الحالية.",
                "تأكيد استعادة",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            this.Cursor = Cursors.WaitCursor;
            btnRestoreBackup.Enabled = false;
            try
            {
                var fileName = lstBackupFiles.SelectedItem.ToString();
                var result = await _serviceProvider.GetRequiredService<IBackupApiService>().RestoreBackupAsync(fileName!);

                if (result.IsSuccess)
                {
                    MessageBox.Show(
                        "تم استعادة النسخة الاحتياطية بنجاح.",
                        "تم",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "فشل في استعادة النسخة الاحتياطية: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء استعادة النسخة الاحتياطية: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRestoreBackup.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private async Task _RefreshBackupListAsync()
        {
            var result = await _serviceProvider.GetRequiredService<IBackupApiService>().GetBackupListAsync();

            lstBackupFiles.Items.Clear();
            if (result.IsSuccess && result.Value != null)
            {
                foreach (var file in result.Value)
                {
                    lstBackupFiles.Items.Add(file);
                }
            }
        }

        // ════════════════════════════════════════════════════════════════════════════════
        // Dictionary Mapping
        // ════════════════════════════════════════════════════════════════════════════════

        private void _MapFromDictionary(Dictionary<string, string> settings)
        {
            // General tab
            chkAutoCreateJE.Checked = _ParseBool(settings, "AutoCreateJournalEntry", true);
            chkAllowNegativeCash.Checked = _ParseBool(settings, "AllowNegativeCash");
            chkEnableAttachments.Checked = _ParseBool(settings, "EnableAttachments", true);
            chkEnableNotifications.Checked = _ParseBool(settings, "EnableNotifications", true);
            numDefaultSalesTax.Value = _ParseInt(settings, "DefaultSalesTax");
            numDefaultPurchaseTax.Value = _ParseInt(settings, "DefaultPurchaseTax");
            numDefaultBranch.Value = _ParseInt(settings, "DefaultBranch", 1);
            numDefaultWarehouse.Value = _ParseInt(settings, "DefaultWarehouse", 1);

            // Inventory tab
            chkAllowNegativeStock.Checked = _ParseBool(settings, "AllowNegativeStock");
            chkEnableFefo.Checked = _ParseBool(settings, "EnableFefo");
            chkRequireBatchOnPurchase.Checked = _ParseBool(settings, "RequireBatchOnPurchase");
            chkRequireExpiryOnPurchase.Checked = _ParseBool(settings, "RequireExpiryOnPurchase");
            numStockAlertDays.Value = _ParseInt(settings, "StockAlertDays", 5);

            // Sales tab
            chkAutoPostInvoices.Checked = _ParseBool(settings, "AutoPostInvoices", true);
            chkAllowDrafts.Checked = _ParseBool(settings, "AllowDrafts", true);
            chkShowProfitInInvoice.Checked = _ParseBool(settings, "ShowProfitInInvoice", true);
            chkPreventBelowRetailPrice.Checked = _ParseBool(settings, "PreventBelowRetailPrice");
            chkAllowBelowCostSale.Checked = _ParseBool(settings, "AllowBelowCostSale");
            numDefaultCashCustomerId.Value = _ParseInt(settings, "DefaultCashCustomerId", 1);
            chkHideTaxInSales.Checked = _ParseBool(settings, "HideTaxInSales");
            chkShowExpiryInInvoices.Checked = _ParseBool(settings, "ShowExpiryInInvoices", true);
            chkAutoPrintAfterPosting.Checked = _ParseBool(settings, "AutoPrintAfterPosting");

            // Purchases tab
            chkPurchaseAutoPost.Checked = _ParseBool(settings, "PurchaseAutoPost", true);
            numDefaultCashSupplierId.Value = _ParseInt(settings, "DefaultCashSupplierId", 1);
            chkHideTaxInPurchases.Checked = _ParseBool(settings, "HideTaxInPurchases");

            // Barcode tab
            chkAutoGenerateBarcode.Checked = _ParseBool(settings, "AutoGenerateBarcode", true);
            chkAllowDuplicateBarcode.Checked = _ParseBool(settings, "AllowDuplicateBarcode");

            // Print tab
            txtThermalPrinterName.Text = _GetValue(settings, "ThermalPrinterName");
            txtA4PrinterName.Text = _GetValue(settings, "A4PrinterName");
            txtLogoPath.Text = _GetValue(settings, "LogoPath");
            txtStoreTaxNumber.Text = _GetValue(settings, "StoreTaxNumber");
            chkShowLogo.Checked = _ParseBool(settings, "ShowLogo");
            txtFooterNote.Text = _GetValue(settings, "FooterNote");
            numPrintCopies.Value = _ParseInt(settings, "PrintCopies", 1);
            chkShowBalanceOnPrint.Checked = _ParseBool(settings, "ShowBalanceOnPrint");
            chkPrintSignature.Checked = _ParseBool(settings, "PrintSignature");
            txtPaperSize.Text = _GetValue(settings, "PaperSize", "A4");
            chkPrintBarcode.Checked = _ParseBool(settings, "PrintBarcode");
            chkPrintQRCode.Checked = _ParseBool(settings, "PrintQRCode");
            chkPrintCompanyAddress.Checked = _ParseBool(settings, "PrintCompanyAddress", true);

            // Notifications tab
            chkLowStockAlert.Checked = _ParseBool(settings, "LowStockAlert", true);
            chkExpiryAlert.Checked = _ParseBool(settings, "ExpiryAlert", true);
            numExpiryAlertDays.Value = _ParseInt(settings, "ExpiryAlertDays", 30);
            chkCreditLimitAlert.Checked = _ParseBool(settings, "CreditLimitAlert", true);

            // Accounting tab
            chkAccountingAutoJE.Checked = _ParseBool(settings, "AutoCreateJournalEntry", true);

            // Backup tab â€” load backup list
            _ = _RefreshBackupListAsync();
        }

        private Dictionary<string, string> _BuildDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                // General
                ["AutoCreateJournalEntry"] = chkAutoCreateJE.Checked.ToString().ToLower(),
                ["AllowNegativeCash"] = chkAllowNegativeCash.Checked.ToString().ToLower(),
                ["EnableAttachments"] = chkEnableAttachments.Checked.ToString().ToLower(),
                ["EnableNotifications"] = chkEnableNotifications.Checked.ToString().ToLower(),
                ["DefaultSalesTax"] = ((int)numDefaultSalesTax.Value).ToString(),
                ["DefaultPurchaseTax"] = ((int)numDefaultPurchaseTax.Value).ToString(),
                ["DefaultBranch"] = ((int)numDefaultBranch.Value).ToString(),
                ["DefaultWarehouse"] = ((int)numDefaultWarehouse.Value).ToString(),

                // Inventory
                ["AllowNegativeStock"] = chkAllowNegativeStock.Checked.ToString().ToLower(),
                ["EnableFefo"] = chkEnableFefo.Checked.ToString().ToLower(),
                ["RequireBatchOnPurchase"] = chkRequireBatchOnPurchase.Checked.ToString().ToLower(),
                ["RequireExpiryOnPurchase"] = chkRequireExpiryOnPurchase.Checked.ToString().ToLower(),
                ["StockAlertDays"] = ((int)numStockAlertDays.Value).ToString(),

                // Sales
                ["AutoPostInvoices"] = chkAutoPostInvoices.Checked.ToString().ToLower(),
                ["AllowDrafts"] = chkAllowDrafts.Checked.ToString().ToLower(),
                ["ShowProfitInInvoice"] = chkShowProfitInInvoice.Checked.ToString().ToLower(),
                ["PreventBelowRetailPrice"] = chkPreventBelowRetailPrice.Checked.ToString().ToLower(),
                ["AllowBelowCostSale"] = chkAllowBelowCostSale.Checked.ToString().ToLower(),
                ["DefaultCashCustomerId"] = ((int)numDefaultCashCustomerId.Value).ToString(),
                ["HideTaxInSales"] = chkHideTaxInSales.Checked.ToString().ToLower(),
                ["ShowExpiryInInvoices"] = chkShowExpiryInInvoices.Checked.ToString().ToLower(),
                ["AutoPrintAfterPosting"] = chkAutoPrintAfterPosting.Checked.ToString().ToLower(),

                // Purchases
                ["PurchaseAutoPost"] = chkPurchaseAutoPost.Checked.ToString().ToLower(),
                ["DefaultCashSupplierId"] = ((int)numDefaultCashSupplierId.Value).ToString(),
                ["HideTaxInPurchases"] = chkHideTaxInPurchases.Checked.ToString().ToLower(),

                // Barcode
                ["AutoGenerateBarcode"] = chkAutoGenerateBarcode.Checked.ToString().ToLower(),
                ["AllowDuplicateBarcode"] = chkAllowDuplicateBarcode.Checked.ToString().ToLower(),

                // Print
                ["ThermalPrinterName"] = txtThermalPrinterName.Text,
                ["A4PrinterName"] = txtA4PrinterName.Text,
                ["LogoPath"] = txtLogoPath.Text,
                ["StoreTaxNumber"] = txtStoreTaxNumber.Text,
                ["ShowLogo"] = chkShowLogo.Checked.ToString().ToLower(),
                ["FooterNote"] = txtFooterNote.Text,
                ["PrintCopies"] = ((int)numPrintCopies.Value).ToString(),
                ["ShowBalanceOnPrint"] = chkShowBalanceOnPrint.Checked.ToString().ToLower(),
                ["PrintSignature"] = chkPrintSignature.Checked.ToString().ToLower(),
                ["PaperSize"] = txtPaperSize.Text,
                ["PrintBarcode"] = chkPrintBarcode.Checked.ToString().ToLower(),
                ["PrintQRCode"] = chkPrintQRCode.Checked.ToString().ToLower(),
                ["PrintCompanyAddress"] = chkPrintCompanyAddress.Checked.ToString().ToLower(),

                // Notifications
                ["LowStockAlert"] = chkLowStockAlert.Checked.ToString().ToLower(),
                ["ExpiryAlert"] = chkExpiryAlert.Checked.ToString().ToLower(),
                ["ExpiryAlertDays"] = ((int)numExpiryAlertDays.Value).ToString(),
                ["CreditLimitAlert"] = chkCreditLimitAlert.Checked.ToString().ToLower(),

                // Accounting
                ["AutoCreateJournalEntry"] = chkAccountingAutoJE.Checked.ToString().ToLower(),
            };

            return dict;
        }

        private static bool _ParseBool(Dictionary<string, string> settings, string key, bool defaultValue = false)
        {
            if (settings.TryGetValue(key, out var raw) && bool.TryParse(raw, out var result))
                return result;
            return defaultValue;
        }

        private static int _ParseInt(Dictionary<string, string> settings, string key, int defaultValue = 0)
        {
            if (settings.TryGetValue(key, out var raw) && int.TryParse(raw, out var result))
                return result;
            return defaultValue;
        }

        private static string _GetValue(Dictionary<string, string> settings, string key, string defaultValue = "")
        {
            return settings.TryGetValue(key, out var raw) ? raw : defaultValue;
        }
    }
}
