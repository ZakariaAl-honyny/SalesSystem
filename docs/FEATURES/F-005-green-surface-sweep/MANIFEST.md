# F-005 - MANIFEST (inventory by command - RULE-N201)

Generated: 2026-11-19

Command producing the raw data: `Get-ChildItem SalesSystem\SalesSystem.Desktop -Recurse -Filter *.Designer.cs` (90 files) then per-file regex extraction of `BackColor`/`ForeColor`/control declarations.

## The three numbers (must reconcile at closing)

| Discovered | In scope | Exempt (with reason) |
|---|---|---|
| 90 | 85 | 5 |

Closing condition: **in scope = done + remaining**, and done must equal 85.

## Exempt files (with reason)

| File | Reason |
|---|---|
| frmStocktakingPrint.Designer.cs | Print-preview surface - the paper must stay white (RULE-N121/printing) |
| frmStocktakingPrintCard.Designer.cs | Print-preview surface - the paper must stay white (RULE-N121/printing) |
| frmStocktakingPrintAdjustment.Designer.cs | Print-preview surface - the paper must stay white (RULE-N121/printing) |
| frmWarehouseTransferPrint.Designer.cs | Print-preview surface - the paper must stay white (RULE-N121/printing) |
| frmPurchaseInvoicePrint.Designer.cs | Print-preview surface - the paper must stay white (RULE-N121/printing) |

## In-scope inventory

Columns: Inputs / InputsWhite (target = all) | Containers / WhiteContainers (target = 0) | FormBack | ShortArgb (RULE-N204 violations) | Wave | Status

| # | File | Inputs | White | Conts | WhiteConts | FormBackColor | ShortArgb | Wave | Status |
|---|---|---|---|---|---|---|---|---|---|
| 1 | ctrlCrudToolbar.Designer.cs | 0 | 0 | 1 | 0 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 2 | ctrlCustomerCard.Designer.cs | 0 | 0 | 2 | 0 | Color.FromArgb(170, 220, 170) | 26 | W4/W6 | todo |
| 3 | ctrlCustomerCardWithFilter.Designer.cs | 1 | 1 | 1 | 0 | Color.FromArgb(170, 220, 170) | 8 | W4/W6 | todo |
| 4 | ctrlDashboard.Designer.cs | 0 | 0 | 10 | 10 | Color.FromArgb(240, 242, 245) | 31 | W4/W6 | todo |
| 5 | ctrlLineEntryPanel.Designer.cs | 7 | 7 | 2 | 1 | System.Drawing.Color.White | 0 | W4/W6 | todo |
| 6 | frmAccountStatementReport.Designer.cs | 4 | 4 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 7 | frmAddUpdateInventoryAdjustment.Designer.cs | 9 | 9 | 5 | 5 | Color.FromArgb(240, 242, 245) | 32 | W3 | todo |
| 8 | frmAddUpdatePurchaseInvoice.Designer.cs | 6 | 6 | 9 | 6 | Color.FromArgb(240, 242, 245) | 27 | W2 | todo |
| 9 | frmAddUpdateSalesInvoice.Designer.cs | 15 | 13 | 9 | 9 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W2 | todo |
| 10 | frmAgingReport.Designer.cs | 0 | 0 | 16 | 15 | Color.FromArgb(240, 242, 245) | 22 | W5 | todo |
| 11 | frmBackup.Designer.cs | 4 | 4 | 12 | 10 | Color.FromArgb(240, 242, 245) | 34 | W4/W6 | todo |
| 12 | frmCalculator.Designer.cs | 1 | 1 | 1 | 1 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 13 | frmCashBoxSummaryReport.Designer.cs | 0 | 0 | 16 | 15 | Color.FromArgb(240, 242, 245) | 22 | W5 | todo |
| 14 | frmCashFlowReport.Designer.cs | 2 | 2 | 18 | 17 | Color.FromArgb(240, 242, 245) | 27 | W5 | todo |
| 15 | frmChangeSystemColorsSettings.Designer.cs | 6 | 5 | 16 | 12 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 16 | frmCRUDCashBoxMaster.Designer.cs | 6 | 6 | 6 | 1 | Color.FromArgb(240, 242, 245) | 22 | W1 | done |
| 17 | frmCRUDCategoryMaster.Designer.cs | 6 | 6 | 6 | 1 | Color.FromArgb(240, 242, 245) | 22 | W1 | done |
| 18 | frmCRUDCExpensesMaster.Designer.cs | 10 | 10 | 7 | 0 | Color.FromArgb(170, 220, 170) | 46 | W4/W6 | todo |
| 19 | frmCRUDCProductMaster.Designer.cs | 14 | 14 | 7 | 0 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W-ref | reference - no change |
| 20 | frmCRUDCProductUnitMaster.Designer.cs | 9 | 9 | 6 | 2 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W1 | done |
| 21 | frmCRUDCSupplierMaster.Designer.cs | 10 | 10 | 10 | 10 | Color.FromArgb(240, 242, 245) | 28 | W1 | done |
| 22 | frmCRUDCustomerMaster.Designer.cs | 10 | 10 | 10 | 10 | Color.FromArgb(240, 242, 245) | 28 | W1 | done |
| 23 | frmCRUDCustomerReceiptsMaster.Designer.cs | 11 | 11 | 8 | 0 | Color.FromArgb(170, 220, 170) | 40 | W4/W6 | todo |
| 24 | frmCRUDUnitMaster.Designer.cs | 6 | 6 | 6 | 1 | Color.FromArgb(240, 242, 245) | 22 | W1 | done |
| 25 | frmCRUDUserMaster.Designer.cs | 6 | 6 | 6 | 1 | Color.FromArgb(240, 242, 245) | 22 | W1 | done |
| 26 | frmCRUDWarehouseMaster.Designer.cs | 8 | 8 | 6 | 1 | Color.FromArgb(240, 242, 245) | 23 | W1 | done |
| 27 | frmCustomerBalanceReport.Designer.cs | 1 | 1 | 18 | 17 | Color.FromArgb(240, 242, 245) | 26 | W5 | todo |
| 28 | frmCustomerSelection.Designer.cs | 1 | 1 | 9 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 29 | frmCustomerStatement.Designer.cs | 3 | 0 | 10 | 8 | Color.FromArgb(240, 242, 245) | 40 | W4/W6 | todo |
| 30 | frmDailyClosureReport.Designer.cs | 0 | 0 | 16 | 15 | Color.FromArgb(240, 242, 245) | 22 | W5 | todo |
| 31 | frmDailySalesReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 32 | frmDashboard.Designer.cs | 0 | 0 | 1 | 0 | Color.FromArgb(240, 242, 245) | 4 | W4/W6 | todo |
| 33 | frmDetailedStockLedgerReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 34 | frmExpiredProductsReport.Designer.cs | 0 | 0 | 16 | 15 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 35 | frmFindCustomer.Designer.cs | 0 | 0 | 1 | 0 | Color.FromArgb(170, 220, 170) | 5 | W4/W6 | todo |
| 36 | frmFormatingSetting.Designer.cs | 8 | 8 | 10 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 37 | frmImportProductsExcel.Designer.cs | 2 | 2 | 6 | 6 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 38 | frmIncomeStatementReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 39 | frmInventory.Designer.cs | 5 | 3 | 5 | 4 | Color.FromArgb(240, 242, 245) | 51 | W3 | todo |
| 40 | frmInventoryBarcode.Designer.cs | 4 | 4 | 4 | 4 | Color.FromArgb(240, 242, 245) | 17 | W4/W6 | todo |
| 41 | frmInventoryFifo.Designer.cs | 0 | 0 | 8 | 5 | Color.FromArgb(240, 242, 245) | 23 | W4/W6 | todo |
| 42 | frmInventoryList.Designer.cs | 5 | 3 | 6 | 5 | Color.FromArgb(240, 242, 245) | 36 | W3 | todo |
| 43 | frmInventoryReport.Designer.cs | 1 | 1 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 44 | frmListCashBoxTransactions.Designer.cs | 6 | 6 | 8 | 7 | Color.FromArgb(240, 242, 245) | 36 | W4/W6 | todo |
| 45 | frmListInventoryAdjustments.Designer.cs | 5 | 5 | 7 | 6 | Color.FromArgb(240, 242, 245) | 18 | W3 | todo |
| 46 | frmListInventoryBatches.Designer.cs | 6 | 6 | 8 | 7 | Color.FromArgb(240, 242, 245) | 24 | W3 | todo |
| 47 | frmListInventoryTransactions.Designer.cs | 5 | 5 | 7 | 6 | Color.FromArgb(240, 242, 245) | 20 | W3 | todo |
| 48 | frmListPayments.Designer.cs | 5 | 5 | 6 | 5 | Color.FromArgb(240, 242, 245) | 47 | W2 | todo |
| 49 | frmListPurchases.Designer.cs | 5 | 5 | 7 | 6 | (none) | 0 | W2 | todo |
| 50 | frmListReturns.Designer.cs | 5 | 5 | 8 | 7 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W2 | todo |
| 51 | frmListSales.Designer.cs | 5 | 5 | 9 | 8 | Color.FromArgb(240, 242, 245) | 41 | W2 | todo |
| 52 | frmListSupplierPayments.Designer.cs | 5 | 5 | 8 | 7 | Color.FromArgb(240, 242, 245) | 35 | W2 | todo |
| 53 | frmLogin.Designer.cs | 2 | 2 | 1 | 1 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 54 | frmLowStockReport.Designer.cs | 0 | 0 | 16 | 15 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 55 | frmMain.Designer.cs | 0 | 0 | 0 | 0 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 56 | frmOpeningStock.Designer.cs | 7 | 7 | 7 | 0 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 57 | frmPasswordChange.Designer.cs | 3 | 3 | 2 | 2 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 58 | frmPOS.Designer.cs | 11 | 11 | 29 | 28 | Color.FromArgb(240, 242, 245) | 53 | W4/W6 | todo |
| 59 | frmProductMovementReport.Designer.cs | 3 | 3 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 60 | frmProductProfitabilityReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 61 | frmProductSelection.Designer.cs | 1 | 1 | 9 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 62 | frmProfitByCustomerReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 63 | frmPurchaseDiscount.Designer.cs | 1 | 1 | 11 | 4 | Color.FromArgb(240, 242, 245) | 23 | W4/W6 | todo |
| 64 | frmPurchaseInvoiceSelection.Designer.cs | 4 | 4 | 9 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 65 | frmPurchaseItem.Designer.cs | 7 | 7 | 7 | 2 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 66 | frmPurchaseItemInfo.Designer.cs | 0 | 0 | 10 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 67 | frmPurchaseOtherDetails.Designer.cs | 5 | 5 | 11 | 5 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 68 | frmPurchasesByProductReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 69 | frmPurchasesBySupplierReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 70 | frmPurchasesReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 71 | frmReturnsReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 72 | frmSalesByCategoryReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 73 | frmSalesByCustomerReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 74 | frmSalesByProductReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 75 | frmSalesInvoiceSelection.Designer.cs | 4 | 4 | 9 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 76 | frmSalesReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 77 | frmShowCustomerCard.Designer.cs | 0 | 0 | 1 | 0 | Color.FromArgb(170, 220, 170) | 7 | W4/W6 | todo |
| 78 | frmStockBalanceReport.Designer.cs | 0 | 0 | 16 | 15 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 79 | frmStocktakingNew.Designer.cs | 6 | 5 | 5 | 5 | Color.FromArgb(240, 242, 245) | 34 | W3 | todo |
| 80 | frmSupplierBalanceReport.Designer.cs | 0 | 0 | 16 | 15 | Color.FromArgb(240, 242, 245) | 24 | W5 | todo |
| 81 | frmSupplierPaymentDialog.Designer.cs | 5 | 5 | 4 | 4 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W2 | todo |
| 82 | frmSupplierSelection.Designer.cs | 1 | 1 | 9 | 8 | System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170)) | 0 | W4/W6 | todo |
| 83 | frmSupplierStatement.Designer.cs | 5 | 3 | 7 | 6 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W4/W6 | todo |
| 84 | frmWarehouseMovementReport.Designer.cs | 2 | 2 | 18 | 17 | System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245)) | 0 | W5 | todo |
| 85 | frmWarehouseTransferNew.Designer.cs | 7 | 6 | 4 | 4 | Color.FromArgb(240, 242, 245) | 29 | W3 | todo |

## Totals as measured (before the wave)

- In-scope inputs: 328 (white: 314)
- In-scope containers: 863 (white: 717)
- In-scope short-form FromArgb: 1097
- In-scope with retired neutral form background (240,242,245): 68
