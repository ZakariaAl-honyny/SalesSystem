# F-007 — الجرد الكامل (`MANIFEST`) — `RULE-N201`

> **مصدر البيانات:** أمر واحد على `Screens/**/frm*.Designer.cs` يستخرج عدد `new GroupBox()` وسلسلة `Controls.Add` العليا لكل مصمّم (2026-11-20). لا رقم في هذا الملف من الذاكرة.

## 1) المطابقة العددية

| الفئة | العدد |
|---|---|
| إجمالي نماذج `frm*` | **85** |
| تحتاج عملاً (`F-007`) | **37** |
| منفَّذة في `F-006` | **5** |
| المرجع | **1** |
| مُجمَّعة أصلاً | **5** |
| معفاة بسبب | **37** |
| **المجموع** | **85** ✅ |

## 2) الشاشات التي تحتاج عملاً (37) — صف لكل ملف

| # | الشاشة | GB الآن | الكتل العليا المقيسة | المطلوب | الحجم | الموجة |
|---|---|---|---|---|---|---|
| 1 | `frmCRUDCustomerMaster` | 5 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `gbToolbar` · **`pnlFields`** · `lblMode` | «بيانات العميل» | S | `W1` |
| 2 | `frmCRUDCSupplierMaster` | 5 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `gbToolbar` · **`pnlFields`** · `lblMode` | «بيانات المورّد» | S | `W1` |
| 3 | `frmCRUDCProductMaster` | 2 | `pnlGrid` · `pnlSummary` · `pnlFilters` · **`pnlButtons`** · `pnlFields` · `lblMode` | «أوامر الشاشة» | S | `W1` |
| 4 | `frmCRUDCProductUnitMaster` | 1 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `pnlButtons` · `pnlFields` · `lblMode` | أوامر · بيانات · الأصناف · ملخص | M | `W1` |
| 5 | `frmCRUDCategoryMaster` | 1 | نفس السابق | أوامر · بيانات · الفئات · ملخص | M | `W2` |
| 6 | `frmCRUDUnitMaster` | 1 | نفس السابق | أوامر · بيانات · الوحدات · ملخص | M | `W2` |
| 7 | `frmCRUDWarehouseMaster` | 1 | نفس السابق | أوامر · بيانات · المستودعات · ملخص | M | `W2` |
| 8 | `frmCRUDCashBoxMaster` | 1 | نفس السابق | أوامر · بيانات · الصناديق · ملخص | M | `W2` |
| 9 | `frmCRUDUserMaster` | 1 | نفس السابق | أوامر · بيانات · المستخدمون · ملخص | M | `W3` |
| 10 | `frmCRUDCExpensesMaster` | 2 | نفس السابق | أوامر · بيانات · المصروفات · ملخص | M | `W3` |
| 11 | `frmCRUDCustomerReceiptsMaster` | 3 | نفس السابق | أوامر · بيانات (الباقي مجمَّع) | M | `W3` |
| 12 | `frmListPayments` | 1 | `tabPayments` · `pnlSummary` · `pnlFilters` · `pnlHeader` | «المدفوعات» · «ملخص النتائج» | S | `W4` |
| 13 | `frmListSupplierPayments` | 1 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | أوامر · المدفوعات · ملخص · تلميحات | M | `W4` |
| 14 | `frmListCashBoxTransactions` | 2 | `pnlGrid` · `pnlFooter` · `pnlSummary` · `pnlFilters` · `gbToolbar` · `pnlHeader` | الحركات · ملخص · تلميحات | M | `W4` |
| 15 | `frmBackup` | 4 | `pnlGrid` · `pnlSummary` · `pnlFooter` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | النسخ الاحتياطية · ملخص · تلميحات | M | `W4` |
| 16 | `frmInventoryList` | 1 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | أوامر · المخزون · ملخص | M | `W5` |
| 17 | `frmListInventoryAdjustments` | 4 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `gbCommands` · `pnlHeader` | التسويات · ملخص | S | `W5` |
| 18 | `frmListInventoryBatches` | 3 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `gbCommands` · `pnlHeader` | الدفعات · ملخص | S | `W5` |
| 19 | `frmListInventoryTransactions` | 4 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `gbCommands` · `pnlHeader` | الحركات · ملخص | S | `W5` |
| 20 | `frmInventory` | 1 | `tabMain` · `pnlSummary` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | أوامر · التبويبات · ملخص | M | `W6` |
| 21 | `frmCustomerSelection` | 1 | `pnlGrid` · `pnlSummary` · `pnlFooter` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | أوامر · العملاء · ملخص · تلميحات | M | `W7` |
| 22 | `frmProductSelection` | 1 | نفس السابق | أوامر · الأصناف · ملخص · تلميحات | M | `W7` |
| 23 | `frmSupplierSelection` | 1 | نفس السابق | أوامر · الموردون · ملخص · تلميحات | M | `W7` |
| 24 | `frmSupplierStatement` | 1 | `pnlGrid` · `pnlSummary` · `pnlFilters` · `pnlToolbar` · `pnlHeader` | أوامر · كشف المورّد · ملخص | M | `W7` |
| 25 | `frmCustomerStatement` | **0** | `splitMain` · `pnlFooter` · `pnlStats` · `pnlFilter` · `pnlHeader` | تصفية · إحصاءات · كشف العميل · حركات · تلميحات | **L** | `W8` |
| 26 | `frmPOS` | 4 | `gbCart` · `gbTotals` · **`pnlActions`** · `gbCustomerPayment` · `gbSalesCommands` · `pnlHeader` | «أوامر البيع» | S | `W9` |
| 27 | `frmWarehouseTransferNew` | 3 | `gbItems` · **`pnlFooter`** · `grpHeader` · `gbToolbar` · `lblMode` | «تلميحات التحويل» | S | `W9` |
| 28 | `frmInventoryFifo` | 3 | `gbGrid` · **`pnlFooter`** · **`pnlInfo`** · `pnlTitle` | «ملخص الطبقات» · التلميحات | S | `W9` |
| 29 | `frmImportProductsExcel` | 3 | `gbPreview` · **`pnlSummary`** · **`pnlFooter`** · `gbOptions` · `gbFile` · `lblMode` | ملخص الاستيراد · تلميحات | S | `W9` |
| 30 | `frmPurchaseItem` | 1 | **`pnlBody`** · **`pnlFooter`** · `pnlTitle` | «بيانات الصنف» · أوامر النافذة | S | `W10` |
| 31 | `frmPurchaseDiscount` | 2 | **`pnlBody`** · **`pnlFooter`** · `pnlTitle` | «الخصم» · أوامر النافذة | S | `W10` |
| 32 | `frmPurchaseOtherDetails` | 3 | **`pnlBody`** · `pnlFooter` · `pnlTitle` | «بيانات إضافية» | S | `W10` |
| 33 | `frmPurchaseItemInfo` | 3 | `gbHistory` · **`pnlFooter`** · `gbCosts` · `gbProductInfo` · `pnlTitle` | «تلميحات الصنف» | S | `W10` |
| 34 | `frmSupplierPaymentDialog` | 1 | `gbPaymentInfo` · **`pnlFooter`** | «تلميحات السداد» | S | `W11` |
| 35 | `frmChangeSystemColorsSettings` | 4 | `gbColors` · `gbPreview` · **`pnlFooter`** · **`pnlSummary`** · `pnlFilters` · `gbToolbar` · `pnlHeader` | ملخص الألوان · تلميحات | S | `W12` |
| 36 | `frmFormatingSetting` | 3 | `gbPreview` · **`pnlSummary`** · **`pnlFooter`** · `gbDates` · `gbNumbers` · `pnlHeader` | ملخص التنسيق · تلميحات | S | `W12` |
| 37 | `frmPasswordChange` | 1 | `gbPassword` · `lblError` · **`pnlFooter`** · `lblMode` | «تلميحات كلمة المرور» | S | `W12` |

> **الغامق** = الكتلة العارية المطلوب إحاطتها. `pnlFilters` يبقى عارياً في كل الصفوف (يحتضن `gbFilter` — `FilterPanelHelper`).

## 3) مُجمَّعة أصلاً — لا عمل (5)

| الشاشة | الصناديق القائمة |
|---|---|
| `frmAddUpdatePurchaseInvoice` | `gbToolbar` · `gbInvoiceInfo` · `gbEntry` (في `ctrlLineEntryPanel`) · `gbItems` · `gbTotals` |
| `frmAddUpdateInventoryAdjustment` | `gbToolbar` · `groupHeader` · `groupLines` · `gbTotals` |
| `frmInventoryBarcode` | `gbToolbar` · `gbInfo` · `gbPreview` |
| `frmOpeningStock` | `gbToolbar` · `gbInfo` · `gbEntry` · `gbItems` · `gbTotals` |
| `frmStocktakingNew` | `gbToolbar` · `grpHeader` · `grpCommittee` · `gbItems` · `grpTotals` |

## 4) معفاة بسبب (37)

| المجموعة | العدد | السبب |
|---|---|---|
| `Screens/Reports/**` | **26** | تُوحَّد بأربعة صناديق عبر `ReportUiStandardizer` — `RULE-N121` |
| `frmPurchaseInvoicePrint` · `frmStocktakingPrint` · `frmStocktakingPrintAdjustment` · `frmStocktakingPrintCard` · `frmWarehouseTransferPrint` | **5** | معاينة/طباعة للقراءة فقط (`pnlPreviewCard` + `pnlFooter` + `pnlHeader`) |
| `frmMain` | 1 | هيكل MDI — `msMainMenu` فقط |
| `frmDashboard` | 1 | غلاف رقيق (4 ك.ب) يحتضن `ctrlDashboard` |
| `frmCalculator` | 1 | أداة صغيرة (4 ك.ب) — شاشة عرض أرقام |
| `frmLogin` | 1 | كتلة واحدة (`gbLogin`) — القاعدة مستوفاة |
| `frmFindCustomer` · `frmShowCustomerCard` | 2 | غلافان رقيقان (5 ك.ب) حول `ctrlCustomerCard*` |
| **المجموع** | **37** | |

## 5) فحص البقايا بعد إغلاق كل مرحلة

| الفحص | الهدف | يُملأ |
|---|---|---|
| `new GroupBox()` في شاشات المرحلة | 0 كتلة عارية متبقية | ⏳ |
| مراجع `pnl*` في `.cs` | صفر تغيير | ⏳ |
| `.cs`/`.resx` معدَّلة | **0** | ⏳ |
| صندوق داخل صندوق · صندوق فارغ | **0** | ⏳ |
| `Color.FromArgb(` المختصر في الأسطر المضافة | **0** | ⏳ |
| `ClientSize`/`MinimumSize` | بلا تغيير | ⏳ |
