# لوحة تقدّم توحيد التنسيق (Format Progress Board)

> **الغرض:** لوحة العمل الوحيدة لموجة `GAP-04` / `RULE-N179` — «كل رقم وتاريخ يراه المستخدم يُنتج في بيت واحد: `Global Classes/clsFormat.cs`».
>
> **المصادر:** التفاصيل الكاملة لمواضع المخالفة في `docs/FORMAT-COVERAGE.md`، والحصر والفجوات في `docs/SHARED-HOME-REGISTER.md` §4. هذا الملف **لا ينسخ أرقامهما** — يضيف فقط حالة التنفيذ وتوزيع الدفعات (`RULE-N174`).
>
> **البيت الواحد:** `SalesSystem.Desktop/Global Classes/clsFormat.cs` — `Money` · `Quantity` · `Number` · `Percent` · `DateToShort` · `DateToDayMonthYear` · `DateToLong` · `Get/Options/Definitions` · `Validate/TryApplyAll`.

---

## 1. الحكم

| البند | الحالة |
|---|---|
| التوحيد على مستوى الشاشات | ✅ **منفَّذ** — 49 ملفاً في أربع دفعات، والبناء **0 أخطاء** |
| ما بقي داخل الشاشات | ⛔ **0 موضع عرض** يستخدم صيغة حرفية (كل ما تبقّى سلاسل آلية أو صيغ أعمدة) |
| ما بقي في البيت نفسه | ⏳ **عضو ناقص**: `clsFormat` لا يعرّض **نمط العمود** (`MoneyPattern`/`QuantityPattern`/`NumberPattern`) — وهو المطلوب لإزالة صيغ `DefaultCellStyle.Format` من الشاشات |
| الطبقات الأخرى | 🔒 **مجمَّد** (`RULE-N177`): `Business/**` و`DataAccess/**` تحملان صيغاً حرفية ولا تُمسّ قبل موافقة المالك المكتوبة |

---

## 2. الدفعات المنفَّذة

| الدفعة | الشاشات | النطاق |
|---|---|---|
| **B1** | 12 | `frmListInventoryBatches` · `frmListInventoryTransactions` · `frmAddUpdateSalesInvoice` · `frmPOS` · `frmStocktakingPrint` · `frmPurchaseCurrency` · `frmJournalEntries` · `frmStocktakingPrintAdjustment` · `frmListReturns` · `frmPurchasesReport` · `frmStocktakingPrintCard` · `frmProductMovementReport` |
| **B2** | 12 | `frmListCustomerReceipts` · `frmListPayments` · `frmListInventoryAdjustments` · `frmInventoryList` · `frmCustomerStatement` · `frmSalesReport` · `frmPurchasesByProductReport` · `frmLowStockReport` · `frmPurchasesBySupplierReport` · `frmInventoryReport` · `frmSupplierBalanceReport` · `frmStockBalanceReport` |
| **B3** | 12 | `frmSalesByCustomerReport` · `frmSalesByProductReport` · `frmSupplierMaster` · `frmSalesByCategoryReport` · `frmDailySalesReport` · `frmProductProfitabilityReport` · `frmProfitByCustomerReport` · `frmPurchaseInvoicePrint` · `frmWarehouseTransferPrint` · `frmSupplierStatement` · `frmPurchaseItemInfo` · `frmInventory` |
| **B4** | 13 | `frmCustomerMaster` · `frmCashBoxTransactions` · `frmListExpenses` · `frmEmployeeMaster` · `frmVatReport` · `frmTrialBalanceReport` · `frmCustomerBalanceReport` · `frmWorkingCapitalReport` · `frmIncomeStatementReport` · `frmAccountStatementReport` · `frmBalanceSheetReport` · `frmCashFlowReport` · `frmGeneralLedgerReport` |

**إجمالي: 49 شاشة · 4 دفعات · 0 أخطاء بناء.**

---

## 3. ما تُرك بقصد — ونوعه

| النوع | المثال | لماذا لا يُحوَّل الآن |
|---|---|---|
| **صيغة عمود شبكة** | `dgv.Columns["…"].DefaultCellStyle.Format = "N2"` | `Helpers/ExportHelper.ResolveNumberFormat` يقرأ النص كوسم نوع (كمية ↔ مبلغ) ليُصدّر Excel بمنازل صحيحة؛ استبداله يحتاج **عضو نمط** في `clsFormat` + Helper في `Helpers/`. العمل داخل 5 شاشات = تكرار ممنوع (`RULE-N172`) |
| **مفتاح ISO للتصفية** | `RowFilter = "CreatedAt >= #yyyy-MM-dd#"` · `SortKey` · `"0.###"` للبحث | سلسلة آلية تُقارَن نصياً، ليست عرضاً للمستخدم؛ تحويلها يكسر التصفية |
| **اسم ملف تصدير** | `$"Sales_{DateTime.Now:yyyyMMdd}.xlsx"` | اسم آلي لا يُرى في الشاشة |
| **رقم/تسلسل مستند** | `_currentEntryNumber` · `ReturnNo` | معرّف لا قيمة منسّقة |
| **`ToString("G")` داخل حقل تحرير** | خلايا كمية في `frmAddUpdateSalesInvoice` | القيمة تُكتب ثم تُقرأ بـ`TryParse` — تحويلها يكسر القراءة |
| **نسبة التداول** | `CurrentRatio` (معامل 1.85) | لا `Percent` (يضرب ×100) ولا `Number` (يقرّب لصحيح) — يحتاج `clsFormat.Ratio` في البيت |

---

## 4. العمل المتبقي (بالترتيب)

| # | البند | البيت | المالك | الحجم |
|---|---|---|---|---|
| 1 | `clsFormat.MoneyPattern` · `QuantityPattern` · `NumberPattern` · `Ratio` | `Global Classes/clsFormat.cs` | `ui-agent` | S |
| 2 | `Helpers/GridFormatHelper.ApplyColumn(dgv, column, FormatKind)` يستهلكها + يعلن الدلالة لـ`ExportHelper` | `Helpers/` | `ui-agent` | S |
| 3 | استبدال صيغ الأعمدة الحرفية في 14 شاشة بسطر واحد لكل عمود | `Screens/**` | `ui-agent` ×2 دفعات | S |
| 4 | صيغ `Business/**` + `DataAccess/**` (تقارير مالية وطباعة) | باك-إند | `backend-architect` + `printing` | M — **مجمَّد حتى موافقة المالك المكتوبة** (`RULE-N177`) |

---

## 5. قائمة تحقق المالك

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | «إعدادات ← إعدادات التنسيق»: منازل المبالغ = 3 · الكميات = 1 · أرقام عربية = نعم | حفظ ناجح |
| 2 | افتح `frmListSales` ثم `frmCustomerStatement` ثم `frmTrialBalanceReport` | كل مبلغ بثلاث منازل والفاصل الحيّ — ولا شاشة تخالف الأخرى |
| 3 | افتح `frmListInventoryBatches` | الكمية بمنزلة واحدة (إعداد الكميات) والمبلغ بثلاث (إعداد المبالغ) |
| 4 | أعِد المنازل إلى 2/3 وأعد فتح الشاشات | القيم تعود — **صفر قيمة مثبّتة في الشاشة** |
| 5 | صدّر أي تقرير إلى Excel | الأرقام أرقاماً لا نصوصاً، والمنازل تتبع الإعداد |
| 6 | أي تاريخ في أي شبكة | يتبع «صيغة التاريخ القصير» الحيّة |

---

> **قاعدة:** لا تُفتح دفعة جديدة قبل إغلاق سابقتها، ولا يُمسّ الباك-إند قبل الموافقة المكتوبة (`RULE-N177`).
