namespace SalesSystem.DTO;

public record PrintSettingsDto(
    string ThermalPrinterName,
    string A4PrinterName,
    string LogoPath,
    string StoreTaxNumber,
    decimal TaxRate,
    bool AutoPrintOnPost,
    string ReceiptHeader,
    string ReceiptFooter,
    int EscPosCodePage,
    string PaperSize,
    int PrintCopies,
    bool ShowBalanceOnPrint,
    bool PrintSignature,
    bool ShowLogo = true,
    string FooterNote = "",
    bool PrintBarcode = false,
    bool PrintQRCode = false,
    bool PrintCompanyAddress = true);

public record StoreSettingsDto(
    int Id,
    string StoreName,
    string? Phone,
    string? Address,
    string? LogoPath,
    string? Email,
    decimal DefaultTaxRate, // DEPRECATED
    bool IsTaxEnabled,      // DEPRECATED
    string? TaxNumber,
    bool EnableStockAlerts,
    bool AllowNegativeStock,
    string InvoicePrefix,    // DEPRECATED
    string? BackupPath = null,
    string? BackupScheduleTime = "02:00",
    int BackupRetentionDays = 30,
    string? UpdateServerUrl = null,
    string? SignaturePath = null);

// ═══════════════════════════════════════════════════════
// Configuration Screens DTOs
// ═══════════════════════════════════════════════════════

public record CompanySettingsDto(
    int Id,
    string CompanyName,
    string? Phone,
    string? Email,
    string? Address,
    string? TaxNumber,
    string? LogoPath
);
