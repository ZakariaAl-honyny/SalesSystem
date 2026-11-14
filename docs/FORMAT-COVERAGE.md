# FORMAT-COVERAGE — سجل تطبيق إعدادات التنسيق (Single Source of Truth)

> **المرجع الملزم:** `AGENTS.md` §2.20 البند **16** (إعدادات التنسيق من مكان واحد) · §2.30 (`RULE-N169`…`RULE-N172` قانون المكان الواحد · **`RULE-N179`** قاعدة المرور الإلزامي عبر `clsFormat`) · §4 FORBIDDEN.
> **البيت الواحد للتنسيق:** `SalesSystem.Desktop/Global Classes/clsFormat.cs` (+ `Helpers/GridFormatHelper.cs` للشبكات) · **الشاشة:** `frmFormatingSetting`.
> **مالك السجل:** الـOrchestrator — يُحدَّث في **نفس الموجة** التي تمسّ `clsFormat` أو `GridFormatHelper` أو أي شاشة تعرض رقماً.
> **سجل تقدّم الوكلاء المرافق:** `docs/FORMAT-PROGRESS.md` (كل وكيل يُحدّث صفّه هناك فقط).
> **النسخة:** 1.0 — 2026-11-14 — كل رقم أدناه مبني على **عدّ فعلي** لملفات المشروع (لا ادّعاء).

---

## 1) ما الذي يصدّقه هذا السجل؟

**القاعدة (`RULE-N179`) — قرار المالك 2026-11-14:**

> **كل رقم أو تاريخ يُعرض للمستخدم يمرّ عبر `clsFormat` حصراً.**
> ممنوع في أي شاشة أو خدمة أو مساعد: `:N2` · `:N3` · `:N0` · `ToString("N2")` · `ToString("N3")` · `DefaultCellStyle.Format = "N2"` · `"#,##0.00"` · `"0.00"` كنص عرض.

**السبب:** إعدادات التنسيق في `frmFormatingSetting` لا معنى لها إذا كان التنسيق مكتوباً حرفياً في الشاشات — تُحفظ القيم ولا يقرأها أحد. الإعداد الذي لا يصل إلى الشاشة **إعداد معطوب**، لا ميزة ناقصة.

| الشرط | الدليل المادي |
|---|---|
| الشاشة تستدعي `clsFormat.*` لكل قيمة تُعرض | `grep "clsFormat\." frm*.cs` |
| لا صيغة حرفية باقية في كود الشاشة | `grep ":N2\|:N3\|\"N2\"\|\"N3\"" frm*.cs` = 0 |
| لا صيغة شبكة حرفية في المصمّم | `grep "Format = \"N" frm*.Designer.cs` = 0 |
| كل شبكة رقمية تُوصَّل بـ`GridFormatHelper` | استدعاء واحد لكل `DataGridView` |
| الشاشة تتحدّث بعد الحفظ بلا إعادة تشغيل | تنفّذ واجهة `IFormatAware` وتُستدعى من `clsFormat.ApplyToOpenForms()` |

---

## 2) التشخيص — لماذا لا يظهر التغيير في الشاشات؟

**الملاحظة كما رصدها المالك:** تغيير منازل المبالغ/الكميات أو فاصل الآلاف أو الأرقام العربية في `frmFormatingSetting` **لا يُغيّر شيئاً** في شاشات النظام.

**هذا صحيح ومُثبَت بالعدّ. أربعة أسباب جذرية:**

### السبب 1 — التنسيق مكتوب **حرفياً** في الشاشات (السبب الغالب)

| القياس | القيمة |
|---|---|
| استدعاءات `clsFormat` في الشاشات الـ95 | **139** |
| صيغ حرفية في كود الشاشات (`:N2` · `"N3"` · `ToString("Nx")` · `"0.00"`) | **186** |
| شاشات تستخدم `clsFormat` | **27** من 95 |
| شاشات فيها صيغ حرفية | **58** من 95 |

> **المعنى:** الإعداد يُقرأ في 139 موضعاً ويُتجاهَل في 186 موضعاً. تغيير المنازل من 2 إلى 3 يُصلح ثلث الشاشات ويترك الباقي كما هو — فيبدو للمالك أن «الإعداد لا يعمل».

**أمثلة موثّقة (ملف · سطر):**

| الملف | السطر | النص |
|---|---|---|
| `Screens/Sales/Forms/frmAddUpdateSalesInvoice.cs` | 2929 | `الإجمالي: {netTotal:N2}` |
| `Screens/POS/Forms/frmPOS.cs` | 648-650 | `المجموع الفرعي: {subTotal:N2}` |
| `Screens/Accounting/Forms/frmJournalEntries.cs` | 560 · 1140 · 1199 | `إجمالي مدين: {totalDebit:N2}` |
| `Screens/Inventory/Forms/frmInventory.cs` | 263-264 | `Format = "N3"` / `Format = "N2"` في تعريف الأعمدة |
| `Screens/Reports/Forms/Sales/frmSalesReport.cs` | 99 · 123 | `{totalSales:N2}` + `DefaultCellStyle.Format = "N2"` |
| `Screens/Customers/Forms/frmCustomerMaster.cs` | 502 | `المديونية: {balance:N2}` |

### السبب 2 — **لا جسر** بين `clsFormat` وشبكات البيانات (`DataGridView`)

`DataGridView` لا يعرف `clsFormat`. صيغة العمود تُثبَّت بـ `DefaultCellStyle.Format = "N2"`، وهي **قيم .NET حرفية** لا تقرأ الإعداد الحيّ.

| القياس | القيمة |
|---|---|
| صيغ شبكة مثبَّتة في المصمّدات | **38** في **10** مصمّمات |

> **دليل قاطع:** حتى الشاشة المرجعية `frmAddUpdateSalesInvoice` — وهي من أكثر الشاشات التزاماً (4 استدعاءات `clsFormat`) — شبكتها ما زالت `dataGridViewCellStyle6.Format = "N3"` و`dataGridViewCellStyle7/8/9.Format = "N2"` في **`frmAddUpdateSalesInvoice.Designer.cs`**. أي أن سطور الفاتورة **لا تتأثر بالإعداد إطلاقاً** رغم أن المجاميع أسفلها تتأثر — تناقض بصري مباشر يراه المالك.

**وما لا صيغة له أسوأ:** أعمدة رقمية تُضاف بلا `Format` إطلاقاً (مثل `frmProductMaster` · `frmProductUnitMaster` · `frmAccounts` · `frmImportProductsExcel`) → تُعرض الكسور كما هي (`1500.5000000`) بلا تجميع ولا منازل.

### السبب 3 — **لا تحديث للشاشات المفتوحة** بعد الحفظ

`ThemePalette.ApplyToOpenForms()` موجود ويُعيد تلوين كل الشاشات المفتوحة فوراً عند تغيير الألوان. **لا نظير له في `clsFormat`.**

نتيجة ذلك: رسالة النجاح في `frmFormatingSetting.cs:513` تقول حرفياً:

> «ستظهر القيم الجديدة في الشاشات **عند تحديث بياناتها (F5)** — بلا حاجة لإعادة تشغيل التطبيق.»

أي أن التصميم الحالي **يعترف** بأن شيئاً لا يتغيّر فوراً، ويطلب من المالك أن يضغط F5 في كل شاشة. وهذه الجملة نفسها **غير صحيحة** في 48 شاشة، لأنها لا تستخدم `clsFormat` أصلاً — F5 لن يُغيّر شيئاً فيها.

### السبب 4 — التصدير والطباعة خارج `clsFormat` تماماً

| الملف | السطر | المشكلة |
|---|---|---|
| `Helpers/ExportHelper.cs` | 111 | `format is "N3" or "F3" ? "#,##0.000" : "#,##0.00"` — خريطة صيغ ثابتة |
| `Printing/Core/PrintHelper.cs` | 33-34 | `amount.ToString("N2")` · `qty.ToString("N3")` |
| `Printing/Core/InvoicePrinter.cs` | 261 | `itemCount.ToString("N0")` |
| `Services/Mock/MockFinancialReportDesktopService.cs` | 964 | `Money(value).ToString("N2")` — تنسيق مزدوج: يُنسّق بـ`clsFormat` ثم يُعيد تنسيقه حرفياً فيلغي الإعداد |
| `Services/Mock/MockJournalEntryDesktopService.cs` | 77 · 183 | `{totalDebit:N2}` داخل رسالة رفض عربية |
| `Controls/Common/MoneyTextBox.cs` | 47 | `Text = "0.00"` |

**المجموع الكلي للنقاط المخالفة: 240** = 186 (كود الشاشات) + 38 (مصمّمات الشبكات) + 16 (طباعة/تصدير/خدمات).

### السبب 5 (مهم للمالك) — الإعدادات **لا تُحفظ** بعد إغلاق التطبيق

`clsFormat` مخزنه في الذاكرة فقط، و`IFormatSettingsDesktopService` مسجَّل على `MockFormatSettingsDesktopService` **دائماً** (`Program.cs:101`). لا BLL ولا جدول `SystemSettings` — وهذا **مقصود** في المرحلة أ ومحمي بقفل الباك-إند (`RULE-N177`): 0 موافقات مكتوبة.

> **النتيجة الصريحة:** بعد إصلاح الشاشات، سيظهر التغيير فوراً ويبقى أثناء التشغيل — **لكنه يُفقد عند إغلاق التطبيق** حتى تُوافق كتابةً على المرحلة ب (`RULE-N166`/`RULE-N177`).

---

## 3) تقييم `clsFormat` نفسه (مراجعة الكود)

| # | البند | الحكم |
|---|---|---|
| 1 | التعريفات والمفاتيح والافتراضيات في مكان واحد (`Definitions`) | ✅ سليم — لا نسخة ثانية في الشاشة ولا في الخدمة |
| 2 | التحقق الوحيد `ValidateAll` + التطبيق الذرّي `TryApplyAll` (لا دفعة نصف مطبَّقة) | ✅ سليم ويطابق `RULE-N167` |
| 3 | القارئ الوحيد `_Read` (مسودّة ← حيّ ← افتراضي) + `_NumberFormat` | ✅ سليم |
| 4 | قراءة الأرقام العربية والفواصل `TryParseCore` | ✅ سليم ومُستهلك من `clsValidation` |
| 5 | `ResetToDefaults()` يُعدّل `_values` **في مكانه** بينما `TryApplyAll` يستبدل المرجع | ⚠️ تفاوت: قارئ متزامن قد يرى حالة نصف مستعادة |
| 6 | لا حدث تغيير ولا `ApplyToOpenForms()` | ❌ **ناقص** — السبب 3 |
| 7 | لا مُزوِّد صيغة للشبكات/التصدير (مثل `MoneyPattern` → `"N2"`) | ❌ **ناقص** — السبب 2 |
| 8 | `DateToShort` الافتراضي `dd/MMM/yyyy` مع `CultureInfo.InvariantCulture` | ❌ **عيب**: اسم الشهر يظهر إنجليزياً (`Nov`) في واجهة عربية. يحتاج خريطة أشهر عربية أو ثقافة `ar` |
| 9 | `TrySet` معرَّض بلا مستهلك | ⚠️ ميت — يُستهلك أو يُزال عند المساس (`RULE-N123` روحاً) |
| 10 | `_values` ساكنة بلا قفل | ➖ مقبول (خيط واجهة واحد) — يُدوَّن ولا يُصلَح |

---

## 4) سجل الشاشات — 95 نموذجاً `frm*`

**مفتاح الحالة:**

| الرمز | المعنى |
|---|---|
| ✅ متوافق | يستخدم `clsFormat` ولا صيغة حرفية ولا صيغة شبكة |
| ⚠️ جزئي | يستخدم `clsFormat` لكن بقي فيه صيغة حرفية أو شبكة مثبَّتة |
| ❌ غير مطبَّق | لا يستخدم `clsFormat` وفيه صيغة حرفية → **الإعداد لا يؤثر فيه إطلاقاً** |
| ⚠️ شبكة بلا صيغة | لا صيغة حرفية لكن شبكته تعرض `decimal` بلا أي تنسيق |
| ➖ لا ينطبق | لا يعرض رقماً/تاريخاً (شاشة إدخال نصي أو هيكلية) |

**الخلاصة:** **✅ 12** · **⚠️ جزئي 15** · **❌ غير مطبَّق 48** · **⚠️ شبكة بلا صيغة 4** · **➖ لا ينطبق 16** — المجموع **95**.

| # | الشاشة | الوحدة | `clsFormat` | صيغ حرفية | صيغ شبكة (مصمّم) | الحالة | **الإجراء المطلوب** |
|---|---|---|---|---|---|---|---|
| 1 | `frmAccounts` | Accounting | 0 | 0 | 0 | ⚠️ شبكة بلا صيغة | توصيل GridFormatHelper |
| 2 | `frmJournalEntries` | Accounting | 0 | 7 | 8 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 3 | `frmCashBoxMaster` | CashBoxes | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 4 | `frmCashBoxTransactions` | CashBoxes | 3 | 2 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 5 | `frmCategoryMaster` | Categories | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 6 | `frmAddUpdateCustomerReceipt` | CustomerReceipts | 1 | 0 | 0 | ✅ متوافق | — |
| 7 | `frmListCustomerReceipts` | CustomerReceipts | 2 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 8 | `frmCustomerMaster` | Customers | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 9 | `frmCustomerSelection` | Customers | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 10 | `frmCustomerStatement` | Customers | 12 | 4 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 11 | `frmDashboard` | Dashboard | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 12 | `frmEmployeeMaster` | Employees | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 13 | `frmAddUpdateExpense` | Expenses | 1 | 0 | 0 | ✅ متوافق | — |
| 14 | `frmListExpenses` | Expenses | 4 | 0 | 0 | ✅ متوافق | — |
| 15 | `frmLogin` | frmLogin.cs/frmLogin.cs | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 16 | `frmMain` | frmMain.cs/frmMain.cs | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 17 | `frmAddUpdateInventoryAdjustment` | Inventory | 2 | 4 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 18 | `frmInventory` | Inventory | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 19 | `frmInventoryAddProduct` | Inventory | 0 | 0 | 3 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 20 | `frmInventoryBarcode` | Inventory | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 21 | `frmInventoryFifo` | Inventory | 2 | 3 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 22 | `frmInventoryList` | Inventory | 0 | 5 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 23 | `frmListInventoryAdjustments` | Inventory | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 24 | `frmListInventoryBatches` | Inventory | 0 | 6 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 25 | `frmListInventoryTransactions` | Inventory | 0 | 5 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 26 | `frmOpeningStock` | Inventory | 2 | 0 | 3 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 27 | `frmStocktakingNew` | Inventory | 3 | 0 | 3 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 28 | `frmStocktakingPrint` | Inventory | 0 | 6 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 29 | `frmStocktakingPrintAdjustment` | Inventory | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 30 | `frmStocktakingPrintCard` | Inventory | 0 | 3 | 4 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 31 | `frmWarehouseTransferNew` | Inventory | 0 | 0 | 2 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 32 | `frmWarehouseTransferPrint` | Inventory | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 33 | `frmListPayments` | Payments | 0 | 7 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 34 | `frmListSupplierPayments` | Payments | 1 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 35 | `frmSupplierPaymentDialog` | Payments | 1 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 36 | `frmPOS` | POS | 0 | 7 | 4 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 37 | `frmImportProductsExcel` | Products | 0 | 0 | 0 | ⚠️ شبكة بلا صيغة | توصيل GridFormatHelper |
| 38 | `frmProductMaster` | Products | 0 | 0 | 0 | ⚠️ شبكة بلا صيغة | توصيل GridFormatHelper |
| 39 | `frmProductSelection` | Products | 0 | 0 | 1 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 40 | `frmProductUnitMaster` | Products | 0 | 0 | 0 | ⚠️ شبكة بلا صيغة | توصيل GridFormatHelper |
| 41 | `frmListPurchases` | Purchases | 8 | 0 | 0 | ✅ متوافق | — |
| 42 | `frmPurchaseCurrency` | Purchases | 0 | 5 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 43 | `frmPurchaseDiscount` | Purchases | 4 | 0 | 0 | ✅ متوافق | — |
| 44 | `frmPurchaseInvoice` | Purchases | 4 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 45 | `frmPurchaseInvoicePrint` | Purchases | 7 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 46 | `frmPurchaseItem` | Purchases | 1 | 0 | 0 | ✅ متوافق | — |
| 47 | `frmPurchaseItemInfo` | Purchases | 7 | 1 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 48 | `frmPurchaseOtherDetails` | Purchases | 1 | 0 | 0 | ✅ متوافق | — |
| 49 | `frmSupplierStatement` | Purchases | 7 | 2 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 50 | `frmAccountStatementReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 51 | `frmAgingReport` | Reports/Forms | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 52 | `frmBalanceSheetReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 53 | `frmCashBoxSummaryReport` | Reports/Forms | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 54 | `frmCashFlowReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 55 | `frmCustomerBalanceReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 56 | `frmDailyClosureReport` | Reports/Forms | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 57 | `frmDailySalesReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 58 | `frmDetailedStockLedgerReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 59 | `frmExpiredProductsReport` | Reports/Forms | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 60 | `frmGeneralLedgerReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 61 | `frmIncomeStatementReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 62 | `frmInventoryReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 63 | `frmLowStockReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 64 | `frmProductMovementReport` | Reports/Forms | 0 | 4 | 4 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 65 | `frmProductProfitabilityReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 66 | `frmProfitByCustomerReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 67 | `frmPurchasesByProductReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 68 | `frmPurchasesBySupplierReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 69 | `frmPurchasesReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 70 | `frmReturnsReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 71 | `frmSalesByCategoryReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 72 | `frmSalesByCustomerReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 73 | `frmSalesByProductReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 74 | `frmSalesReport` | Reports/Forms | 0 | 4 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 75 | `frmStockBalanceReport` | Reports/Forms | 0 | 3 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 76 | `frmSupplierBalanceReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 77 | `frmTrialBalanceReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 78 | `frmVatReport` | Reports/Forms | 0 | 2 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 79 | `frmWarehouseMovementReport` | Reports/Forms | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 80 | `frmWorkingCapitalReport` | Reports/Forms | 0 | 5 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 81 | `frmListReturns` | Returns | 2 | 4 | 0 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 82 | `frmPurchaseReturn` | Returns | 1 | 0 | 0 | ✅ متوافق | — |
| 83 | `frmSalesReturn` | Returns | 1 | 0 | 0 | ✅ متوافق | — |
| 84 | `frmAddUpdateSalesInvoice` | Sales | 4 | 13 | 6 | ⚠️ جزئي | إزالة الصيغ الحرفية + توصيل الشبكة |
| 85 | `frmListSales` | Sales | 7 | 0 | 0 | ✅ متوافق | — |
| 86 | `frmChangeSystemColorsSettings` | Settings | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 87 | `frmFormatingSetting` | Settings | 48 | 0 | 0 | ✅ متوافق | — |
| 88 | `frmSupplierMaster` | Suppliers | 0 | 1 | 0 | ❌ غير مطبَّق | نقل كل تنسيق إلى clsFormat |
| 89 | `frmSupplierSelection` | Suppliers | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 90 | `frmCalculator` | Tools | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 91 | `frmUnitMaster` | Units | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 92 | `frmBackup` | Users | 3 | 0 | 0 | ✅ متوافق | — |
| 93 | `frmPasswordChange` | Users | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 94 | `frmUserMaster` | Users | 0 | 0 | 0 | ➖ لا ينطبق | — |
| 95 | `frmWarehouseMaster` | Warehouses | 0 | 0 | 0 | ➖ لا ينطبق | — |

> **ملاحظة عدّ:** «صيغ حرفية» = `:N0..N9` + `"N0..N9"` + `ToString("Nx")` + `"0.00…"` داخل كود الشاشة. «صيغ شبكة» = `Format = "…"` داخل `*.Designer.cs`. الأرقام تُعاد قراءتها في كل موجة (`RULE-N120`).

---

## 5) الفجوات المفتوحة

| البند | الفجوة | النطاق | المالك | الحالة |
|---|---|---|---|---|
| ف-ت1 | لا مُزوِّد صيغة حيّ ولا حدث تغيير ولا `ApplyToOpenForms()` في `clsFormat` | `Global Classes/clsFormat.cs` | `ui-agent` | **مفتوح — أساس** |
| ف-ت2 | لا `GridFormatHelper` — لا جسر بين الإعداد وشبكات البيانات | `Helpers/GridFormatHelper.cs` (جديد) | `ui-agent` | **مفتوح — أساس** |
| ف-ت3 | 48 شاشة لا تستخدم `clsFormat` إطلاقاً | `Screens/**` | `ui-agent` (وكيل لكل شاشة) | **مفتوح** |
| ف-ت4 | 15 شاشة جزئية + 38 صيغة شبكة في 10 مصمّمات | `Screens/**` | `ui-agent` | **مفتوح** |
| ف-ت5 | التصدير والطباعة والخدمات الوهمية خارج `clsFormat` (16 موضعاً) | `Helpers/ExportHelper.cs` · `Printing/**` · `Services/Mock/**` · `Controls/**` | `ui-agent` / `printing` | **مفتوح** |
| ف-ت6 | `frmFormatingSetting` لا تُحدّث الشاشات المفتوحة بعد الحفظ، ورسالتها تعِد بما لا يحدث | `Screens/Settings/Forms/frmFormatingSetting.cs` | `ui-agent` | **مفتوح** |
| ف-ت7 | اسم الشهر يظهر إنجليزياً في الواجهة العربية (`InvariantCulture` + `MMM`) | `clsFormat._FormatDate` | `ui-agent` | **مفتوح** |
| ف-ت8 | لا حفظ دائم — الإعدادات تُفقد عند إغلاق التطبيق | `Program.cs:101` (Mock دائماً) | **مقفل** — يحتاج موافقة مكتوبة (`RULE-N177`) | 🔒 **المرحلة ب** |

---

## 6) خطة الإصلاح — الشرائح والملاك

| الشريحة | الحجم | النطاق | الوكيل | تعتمد على |
|---|---|---|---|---|
| **F1 — الأساس** | L | `clsFormat` (مُزوِّد الصيغة · الحدث · `IFormatAware` · `ApplyToOpenForms` · تصحيح `ResetToDefaults` · أشهر عربية) + `Helpers/GridFormatHelper.cs` (جديد) + `frmFormatingSetting.cs` (استدعاء التحديث بعد الحفظ) | `ui-agent` × 1 | — |
| **F2 — الشاشات غير المطبَّقة (48)** | L | وكيل واحد لكل شاشة · موجات ≤ 4 وكلاء | `ui-agent` × 12 موجة | F1 |
| **F3 — الشاشات الجزئية (15) + شبكات المصمّم (10)** | L | وكيل واحد لكل شاشة | `ui-agent` × 4 موجات | F1 |
| **F4 — التصدير والطباعة** | M | `ExportHelper` · `PrintHelper` · `InvoicePrinter` · `MoneyTextBox` | `ui-agent` + `printing` | F1 |
| **F5 — الخدمات الوهمية** | S | `MockFinancialReport` (تنسيق مزدوج) · `MockJournalEntry` (رسائل الرفض) | `fast-agent` | F1 |
| **F6 — إعادة التصديق** | — | `mock-qa` يتحقق ويعيد الحكم · تحديث هذا السجل | `mock-qa` | F1–F5 |

> **قاعدة الموجة:** لا تبدأ F2 قبل خروج F1 ببناء أخضر — لأن الشاشات ستستهلك عقداً جديداً (`GridFormatHelper` + `IFormatAware`)؛ إصلاح شاشة قبل وجود البيت الواحد يُنتج نسخة ثانية من الصيغة (`RULE-N171`).

---

## 7) دليل تحقق المالك (عربي)

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmFormatingSetting` | غيّر «منازل المبالغ» من 2 إلى 3 واحفظ | تُفتح شاشة مفتوحة (مثل `frmListSales`) وتتحدّث المجاميع فوراً إلى 3 منازل — **بلا F5** |
| 2 | `frmAddUpdateSalesInvoice` | غيّر «منازل الكميات» إلى 0 | عمود الكمية في الشبكة يعرض أعداداً صحيحة |
| 3 | أي شاشة | أوقف «فاصل الآلاف» | يختفي الفاصل من كل المبالغ في الشاشات المفتوحة |
| 4 | أي شاشة | فعّل «أرقام عربية» | كل الأرقام المعروضة تصبح ٠١٢٣٤٥٦٧٨٩ |
| 5 | `frmListSales` | غيّر صيغة التاريخ القصير إلى `yyyy/MM/dd` | عمود التاريخ يتغيّر فوراً |
| 6 | أي تصدير Excel | صدّر بعد تغيير المنازل | ملف Excel يحمل نفس منازل الشاشة |
| 7 | طباعة فاتورة | اطبع بعد تغيير المنازل | المبالغ في الطباعة بنفس منازل الشاشة |
| 8 | `frmFormatingSetting` | اضغط «استعادة الافتراضي» | كل الشاشات تعود إلى 2 منازل للأموال و3 للكميات |
| 9 | التطبيق | أغلق التطبيق وأعد فتحه | ⚠️ **تعود القيم الافتراضية** — فجوة ف-ت8 مقصودة (المرحلة ب) |
| 10 | `frmFormatingSetting` | أدخل فاصلة كسر «,» مع تفعيل فاصل الآلاف | رفض عربي ولا حفظ — التحقق يعمل |

---

## 8) بروتوكول التحديث

1. أي موجة تمسّ `clsFormat` أو `GridFormatHelper` أو أي شاشة تعرض رقماً ⇒ **يُحدَّث هذا السجل في نفس الموجة** (`RULE-N174` روحاً).
2. الأرقام لا تُكتب بالحدس — تُعاد قراءتها من الملفات، والوكيل يذكر الأمر الذي استخدمه.
3. **لا تُنسخ أرقام هذا السجل** في `AGENTS.md` ولا في أي شاشة — يُستهلك بالمرجع (`RULE-N169`).
4. حالة كل شريحة تُسجَّل في `docs/FORMAT-PROGRESS.md` (سجل الوكلاء) — **هذا الملف** يحمل الحقائق، وذاك يحمل التقدّم.
5. لا تُعلن شاشة «متوافقة» قبل: صفر صيغة حرفية + صفر صيغة شبكة + توصيل `GridFormatHelper` + بناء **0 Errors**.
