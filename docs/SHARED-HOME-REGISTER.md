# سجل البيوت المشتركة (Shared Home Register)

> **الغرض:** حصر **كل** كود مشترك في النظام — بيته الواحد، ما يعرضه، ومن يستهلكه فعلاً، وكل مخالفة قائمة بالأرقام. هذا الملف هو **قائمة العمل الوحيدة** لموجة «وحّد وطبّق على النظام كله».
>
> **القانون الحاكم:** `AGENTS.md` §2.30 — `RULE-N169` (المكان الواحد) · `RULE-N170` (ابحث قبل أن تكتب) · `RULE-N171` (معيار التعديل الواحد) · `RULE-N172` (سقف التكرار).
>
> **البيوت:** `SalesSystem.Desktop/Global Classes/` · `SalesSystem.Desktop/Helpers/` + `SalesSystem.Common/Helpers/` + `SalesSystem.Business/Helpers/`.
>
> **حدود هذا الملف:** يحمل **الحصر** و**المخالفة** و**مكان الإصلاح**. تفاصيل شاشات التنسيق (67 شاشة × 240 مخالفة) مصدرها الوحيد `docs/FORMAT-COVERAGE.md`، ولوحة توزيع العمل في `docs/FORMAT-PROGRESS.md` — لا تُنسخ هنا (`RULE-N174` بالقياس).
>
> **آخر تحديث:** 2026-11-14 · الأساس `b380cd3` · **مراجعة على سبعة أبعاد مقيسة بالأمر على الكود** + **W0 مُنفَّذة والبناء أخضر (0 أخطاء).**

---

## 0. نتيجة المراجعة في سطر

| البُعد | النتيجة |
|---|---|
| البيوت نفسها | 9 سليمة · 8 جزئية · 1 ميت — **11 مخالفة داخل البيوت** (أُغلقت في W0) |
| الثيم والألوان | **56** ملفاً يكتب لون الحقل النشط حرفياً + **51** لوناً دلالياً حرفياً |
| التنسيق | **240** موضعاً (`FORMAT-COVERAGE`) |
| لوحة المفاتيح | **1** شاشة ناقصة فقط (`frmLogin`) — سليم عداها (**أُغلقت في W0**) |
| الحوارات/التحقق/التغذية | **3** شاشات تفقيط ناقصة · **20** شاشة بلا تغذية بصرية |
| التصفية/الشبكة/التقرير | **13** شبكة بلا `ContextMenuStrip` · **37** حقلاً بلا `Color.White` |
| الطبقات غير الشاشية | **12** لوناً في `Controls/` + `Forms/` · **3** صيغة طباعة · **3** تنسيق مزدوج في الـMock |

**ثبت بالقياس أن هذه سليمة تماماً (صفر مخالفة):** `MessageBox.Show` خارج `clsDialogs` · `RowFilter` خام · `ex.Message` للمستخدم · `ApplyGrid` في الشبكات غير التقريرية (48/48) · `ReportUiStandardizer` (31/31) · `ReportNavigationHelper` (31/31) · تسجيل DI (95/95 نموذجاً + `ctrlDashboard`) · `WireGlobalShortcuts` (64/64) · `gbFilter` في الشاشات غير التقريرية (28/28 مع `btnToggleFilters` + `btnClearFilters`).

### 0.1 حالة W0 — «أساس البيوت» (منفَّذة · بناء 0 أخطاء)

| البند | ما نُفِّذ | الملف |
|---|---|---|
| `GAP-09` | حذف `form.BackColor = Color.White` من `ApplyMainFormStyle` · `ApplyDataGridViewStyle` صار تفويضاً لـ`GreenThemeHelper.ApplyGrid` · `ApplyToolbarStyle` → `BarBackground` · `ContentBg` → `GroupBackground` · `TextPrimary` → `TextStrong` · حذف `FocusHighlight` المكرّر واستهلاك `ThemePalette.SelectionRowColor` حيّاً | `Helpers/ThemeHelper.cs` |
| `GAP-09` | حذف `FocusHighlight = 0xD6E4F0` واستهلاك `ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)` | `Helpers/ReportUiStandardizer.cs` |
| `GAP-09` | `RejectFlashBack` → `GreenThemeHelper.RejectBackground` | `Global Classes/clsInputFeedback.cs` |
| `GAP-03` (تأسيس) | إضافة **7 ثوابت حالة** في البيت الواحد: `StatusSuccess` · `StatusDanger` · `StatusWarning` · `StatusInfo` · `StatusNeutral` · `StatusMuted` · `RejectBackground` — **لم تُوصَّل بأي شاشة بعد (W8)** | `Helpers/GreenThemeHelper.cs` |
| `GAP-05` | `SystemColors.ControlText` → `Color.FromArgb(33, 37, 41)` | `Screens/frmMain.Designer.cs` |
| `GAP-07` | صيغة Excel تُبنى من قيم `clsFormat` الحيّة + `ResolveDateNumberFormat` | `Helpers/ExportHelper.cs` |
| `GAP-16` | إضافة `WireArrowNavigation` — `frmLogin` صارت 6/6 | `Screens/frmLogin.cs` |
| `GAP-17` | إنشاء `MockAuthDesktopService` (3 أعضاء · تكافؤ رفض عربي · بلا BLL) | `Services/Mock/MockAuthDesktopService.cs` |
| `GAP-18` | إعلان `lblStatus` في المصمّم وإضافته داخل `pnlFooter` (`RULE-N145`) | `Screens/Users/Forms/frmBackup.Designer.cs` |
| `GAP-13` | **إيجابية كاذبة** — لا كود، السطر تعليق. مغلقة | — |

---

## 1. حصر البيوت — `Global Classes/` (5 ملفات · 1,157 سطراً)

| # | الملف | الأسطر | الأسطر العامة | السطح العام | ملفات مستهلكة | مراجع | الحكم |
|---|---|---|---|---|---|---|---|
| 1 | `clsDialogs.cs` | 138 | `clsDialogs` | 12 دالة: `Info`×2 · `Success`×2 · `Warning`×2 · `Error`×3 · `Confirm`×2 · `ConfirmEdit` | **88** | **863** | ✅ **مكتمل** |
| 2 | `clsFormat.cs` | 657 | `clsFormat` · `FormatSettingKind` · `FormatSettingDefinition` · `FormatOption` | `Definitions` · `Get/Options/Snapshot/TrySet/TryApplyAll/ResetToDefaults` · `Validate/ValidateAll` · `Money/Quantity/Number/Percent` · `DateToShort/DateToDayMonthYear/DateToLong` · `TryParseMoney/TryParseQuantity` · `IsOn` | 29 | 149 | 🔄 **جزئي** — `GAP-04` |
| 3 | `clsGlobal.cs` | 99 | `clsGlobal` (**internal**) | `frmMdiMain` · `_OpenForm(Form)` | 6 | 10 | ✅ **مكتمل** |
| 4 | `clsInputFeedback.cs` | 199 | `clsInputFeedback` | `ShowRejectTooltip` · `ShowInfoTooltip` · `ShowSuccessTooltip` | 13 | 16 | 🔄 **جزئي** — `GAP-08b` · لون حرفي `GAP-09` |
| 5 | `clsValidation.cs` | 64 | `clsValidation` | `ValidateEmail` · `IsRequired` · `IsDigits` · `IsPhone` · `IsPositiveDecimal` · `TryParseDecimal` | **0** | **0** | ⛔ **ميت** — `GAP-01` |

---

## 2. حصر البيوت — `Helpers/` (13 ملفاً · 6,585 سطراً)

| # | الملف | الأسطر | السطح العام | ملفات مستهلكة | مراجع | الحكم |
|---|---|---|---|---|---|---|
| 6 | `KeyboardHelper.cs` | 775 | 12 دالة توصيل (`WireEnterAsTab` · `WireSelectAllOnFocus` · `WireFocusHighlight` · `RefreshFocusHighlight` · `WireArrowNavigation` · `WireEnterToClick`×2 · `WireGlobalShortcuts` · `WireModeEnter` · `WireButtonDisabledVisual` · `WireDisabledVisual` · `WireDisabledVisuals`) | **65** | **405** | ✅ **مكتمل** |
| 7 | `GreenThemeHelper.cs` | 444 | 14 خاصية لون + `Apply(Form)` · `Apply(Control)` · `ApplyGrid` · `ApplyHeader` | **73** | **379** | 🔄 **جزئي** — لا يحمل ثوابت الحالة الدلالية (`GAP-03`) |
| 8 | `ThemePalette.cs` | 679 | 24 مفتاح لون + `Definitions` · `DefaultFor` · `Get/TryGet/TrySet` · `ResetToDefaults` · `Snapshot` · `ApplyToOpenForms` · `ContrastRatio` · `MinimumContrastFor` · `ContrastPartnerKey/Color` · `ToHex/TryParseHex/TryParseColorText` | 4 | 86 | ✅ **مكتمل** — المالك الوحيد لحساب WCAG |
| 9 | `ThemeHelper.cs` | 176 | 6 ألوان + 7 دوال تنسيق | 38 | 267 | ⛔ **مخالف** — 15 لوناً حرفياً + يفرض أبيض (`GAP-09`) |
| 10 | `FilterPanelHelper.cs` | 442 | `Attach(Form, IServiceProvider)` | 48 | 48 | ✅ **مكتمل** |
| 11 | `ExportHelper.cs` | 151 | `ExportToExcel` · `ExportToCsv` | 43 | 43 | 🔄 **جزئي** — `GAP-07` |
| 12 | `RowFilterHelper.cs` | 57 | `Escape` · `Like` | 26 | 93 | ✅ **مكتمل** |
| 13 | `ReportUiStandardizer.cs` | 2,023 | `Apply(Control)` · `ApplyForm(Form)` · `ApplyReferenceLook(Form)` | 31 | 62 | 🔄 **جزئي** — لون مكرّر `GAP-09` |
| 14 | `ReportNavigationHelper.cs` | 751 | `Attach(Form, IServiceProvider)` | 31 | 31 | ✅ **مكتمل** |
| 15 | `LiveTafqeetHelper.cs` | 297 | `Attach(ToolTip, params (Control, string, ReadMode)[])` · `ReadMode` | 19 | 55 | 🔄 **جزئي** — `GAP-08` |
| 16 | `ArabicNumberToWords.cs` | 288 | `ToWords(amount, currency, subUnit, appendOnly)` | 4 | 4 | ✅ **مكتمل** |
| 17 | `ColorSuggestionHelper.cs` | 387 | `For` · `LadderFor` · `BestFor` · `IsAcceptable` · `ReadableForeFor` · `ArabicNameFor` | 1 | 6 | ✅ **مكتمل** — يستهلك `ThemePalette` في كل قواعده |
| 18 | `BackupFileNameHelper.cs` | 115 | `Extension` · `NoDateText` · `FallbackDatabaseName` · `Build` · `TryGetCreatedAt` · `GetCreatedAtText` | 2 | 8 | 🔄 **موجة أخرى** (غير مُلتزَم) |

**الإجمالي: 18 ملفاً · 7,742 سطراً · 1,573 مرجعاً من 520 ملفاً.**

---

## 3. جدول الصحة الإجمالي

| الحكم | البيوت | العدد |
|---|---|---|
| ✅ **مكتمل** | `clsDialogs` · `clsGlobal` · `ThemePalette` · `KeyboardHelper` · `FilterPanelHelper` · `RowFilterHelper` · `ReportNavigationHelper` · `ArabicNumberToWords` · `ColorSuggestionHelper` | **9** |
| 🔄 **جزئي** | `clsFormat` · `clsInputFeedback` · `GreenThemeHelper` · `ExportHelper` · `ReportUiStandardizer` · `LiveTafqeetHelper` · `BackupFileNameHelper` | **7** |
| ⛔ **مخالف/ميت** | `ThemeHelper` · `clsValidation` | **2** |

---

## 4. سجل المخالفات — GAP (كلها مقيسة)

> نطاق القياس: كل `*.cs` في `SalesSystem.Desktop` **عدا** `Helpers/` و`Global Classes/` (المصدر). المصمّمات تُقاس منفصلة لأن القيم الحرفية فيها **إلزامية** (`RULE-N134`)، فـ**3,582 موضعاً في 96 مصمّماً سليمة**.

| # | المخالفة | القاعدة | ملفات | مواضع | المالك | الحجم | الموجة | الحالة |
|---|---|---|---|---|---|---|---|---|
| **GAP-01** | `clsValidation` بلا أي مستهلك في الحل كله — 64 سطراً ميتة. **القرار (المالك): يُستهلك ولا يُحذف** — يُعتمد في W8 عبر `GAP-08b` | `RULE-N170` | 1 | 0 مستهلك | `ui-agent` | S | W8 | 🔄 قرار مثبَّت |
| **GAP-02** | تمرير `Color.FromArgb(0xD6, 0xE4, 0xF0)` حرفياً إلى `WireFocusHighlight` بدل `ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)` | `RULE-N54b` · `RULE-N112` | **56** | **56** | `ui-agent` ×7 دفعات | S | W1–W7 | ⬜ |
| **GAP-03** | ألوان الحالة الدلالية حرفياً ومتناقضة — **51 موضعاً في 16 ملفاً** بالتفصيل في §4.3 | `RULE-N54` · `RULE-N172` | 16 | 51 | `ui-agent` | M | W8 | ⬜ |
| **GAP-04** | صيغ رقمية/تاريخية حرفية بدل `clsFormat` — **240 موضعاً** (`FORMAT-COVERAGE`) | `RULE-N179` (جديد) | 67 شاشة | 240 | 46 وكيلاً (`FORMAT-PROGRESS`) | L | W9–W12 | ⬜ |
| **GAP-05** | `frmMain.Designer.cs:945` `ForeColor = SystemColors.ControlText` — العنصر الوحيد على لون نظام | `RULE-N141` | 1 | 1 | `fast-agent` | S | W0 | ✅ مكتمل |
| **GAP-06** | 36 شاشة شبكة بلا `ExportHelper` — **تحتاج حكماً لكل شاشة** لا إصلاحاً أعمى (قائمة ⇐ تُصدَّر / محرّر ⇐ لا) | `RULE-N169` | 36 | — | `ui-agent` | M | W8 | ⬜ |
| **GAP-07** | `ExportHelper.cs:111` يثبّت المنازل: `format is "N3" or "F3" ? "#,##0.000" : "#,##0.00"` — البيت نفسه يخالف القانون · و`:91` `"yyyy-mm-dd"` ثابتة | `RULE-N169` · `RULE-N179` | 1 | 2 | `fast-agent` | S | W0 | ✅ مكتمل |
| **GAP-08** | تفقيط ناقص في شاشات إدخال مالي: `frmProductMaster` (5 حقول) · `frmProductUnitMaster` (5) · `frmInventoryBarcode` (1) — وتحتاج حكماً: `frmWarehouseTransferNew` · `frmStocktakingNew` · `frmCashBoxTransactions` | `RULE-N97` | 3 | 11 حقلاً | `ui-agent` | M | W8 | ⬜ |
| **GAP-08b** | 20 شاشة لديها `errorProvider` بلا `clsInputFeedback` (تغذية رفض بصرية) — التفصيل §4.5. **حاملة استهلاك `clsValidation` (GAP-01)** | `RULE-N169` | 20 | — | `ui-agent` | M | W8 | ⬜ |
| **GAP-09** | ألوان حرفية **داخل البيوت** تناقض «المكان الواحد» — التفصيل §4.1 (15 في `ThemeHelper` · 2 لون مكرّر · 1 في `clsInputFeedback`) | `RULE-N54a` · `RULE-N171` | 4 | 18 | `ui-agent` | M | W0 | ✅ مكتمل |
| **GAP-10** | صيغة حرفية في طبقة الطباعة: `PrintHelper.cs:33-34` (`"N2"` · `"N3"`) · `InvoicePrinter.cs:261` (`"N0"`) | `RULE-N179` | 2 | 3 | `printing` | S | W8 | ⬜ |
| **GAP-11** | تنسيق مزدوج في الـMock: `MockFinancialReportDesktopService.cs:964` · `MockJournalEntryDesktopService.cs:77,183` | `RULE-N179` | 2 | 3 | `implement-agent` | S | W8 | ⬜ |
| **GAP-12** | `MoneyTextBox.cs:47` `Text = "0.00"` — قيمة أولية حرفية | `RULE-N179` | 1 | 1 | `ui-agent` | S | W8 | ⬜ |
| **GAP-13** | ~~`frmDashboard.Designer.cs:11` `UseVisualStyleBackColor = true`~~ — **إيجابية كاذبة**: السطر تعليق يشرح القاعدة، والكود الوحيد `btnClose.UseVisualStyleBackColor = false` وهو صحيح، و`BackColor` معلن `(170,220,170)`. **مغلقة بلا إصلاح** | `RULE-N141` | 0 | 0 | — | — | W0 | ❌ مغلقة (كاذبة) |
| **GAP-14** | 13 شبكة غير تقريرية بلا `ContextMenuStrip` — التفصيل §4.6 | `RULE-N23` | 13 | — | `ui-agent` | S | W8 | ⬜ |
| **GAP-15** | 37 حقلاً في 18 مصمّماً بلا `Color.White` معلن — التفصيل §4.7 | `RULE-N143` | 18 | 37 | `ui-agent` | S | W8 | ⬜ |
| **GAP-16** | `frmLogin` تستدعي 5 من 6 — ينقصها `WireArrowNavigation` (صار 6/6) | `RULE-N40`/`N95` | 1 | 1 | `fast-agent` | S | W0 | ✅ مكتمل |
| **GAP-17** | `IAuthDesktopService` بلا تنفيذ وهمي: `Program.cs:107` يشير إلى `MockAuthDesktopService` غير الموجود (كان `MockAuthService : IAuthService` فقط) — كسر بناء + فجوة تغطية `RULE-N173` | `RULE-N173` · `RULE-N167` | 1 | 1 | `ui-agent` | S | W0 | ✅ مكتمل |
| **GAP-18** | `frmBackup.cs` يستخدم `lblStatus` ($179$ · $549$) والمصمّم لا يُعلنه — عنصر دائم غير معلن في `*.Designer.cs` وكسر بناء | `RULE-N145` · `RULE-N130` | 2 | 2 | `ui-agent` | S | W0 | ✅ مكتمل |

**الإجمالي: 19 مخالفة (منها 1 إيجابية كاذبة) · 6 أساسية (W0) ✅ · 7 دفعات لون (W1–W7) · 6 حزم (W8) · 4 موجات تنسيق (W9–W12).**

---

### 4.1 تفصيل `GAP-09` — ألوان حرفية داخل البيوت

| الملف | السطر | المخالفة | القرار |
|---|---|---|---|
| `ThemeHelper.cs` | 18 | `BorderColor = ColorTranslator.FromHtml("#DEE2E6")` | يُنقل مفتاحاً إلى `ThemePalette` |
| `ThemeHelper.cs` | 20 | `SidebarBg = Color.FromArgb(33, 43, 54)` | يُنقل إلى `ThemePalette` |
| `ThemeHelper.cs` | 21 | `ContentBg = Color.White` | `ThemePalette` |
| `ThemeHelper.cs` | 22 | `TextPrimary = "#212529"` | `ThemePalette` |
| `ThemeHelper.cs` | 23 | `TextSecondary = (108, 117, 125)` | `ThemePalette` |
| `ThemeHelper.cs` | 24 | `TextDarkGray = (50, 50, 50)` | `ThemePalette` |
| `ThemeHelper.cs` | 35 | `form.BackColor = Color.White` — **يمحو خلفية المصمّم الخضراء** | يُستهلك `GreenThemeHelper.FormBackground` |
| `ThemeHelper.cs` | 89 · 109 · 122 · 126 · 132 · 137 | 6 ألوان حرفية للشبكة/الزر | `ThemePalette` |
| `ThemeHelper.cs` | 147 | `FocusHighlight = (255, 255, 180)` — **يكرّر `ThemePalette.SelectionRowColor`** | يُحذف ويُستهلك المفتاح |
| `ReportUiStandardizer.cs` | 97 | `FocusHighlight = (0xD6, 0xE4, 0xF0)` — **يكرّر `ThemePalette.ActiveFieldColor`** | يُحذف ويُستهلك `ThemePalette.DefaultFor` |
| `clsInputFeedback.cs` | 26 | `RejectFlashBack = (255, 220, 220)` — لون حالة | ثابتاً مسمّى في `GreenThemeHelper` |

**ملاحظة:** `ColorSuggestionHelper` **بريء** — كل قواعده تقرأ `ThemePalette` (`ContrastRatioRaw` · `MinimumContrastFor` · `ContrastPartnerColor` · `DefaultFor`)، وقائمة اقتراحاته كتالوج اختيارات لا افتراضيات ثيم. وحساب WCAG يعيش **مرة واحدة** في `ThemePalette.RelativeLuminance`/`ContrastRatioRaw` — لا نسخة ثانية.

---

### 4.2 تفصيل `GAP-02` — 56 ملفاً (دفعات 7 × 8)

`Screens/**` — كل ملف يستدعي `KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0))` أو يعرّف `FocusColor` ثابتاً بالقيمة نفسها. الإصلاح **متطابق حرفياً** في كل ملف:
`Color.FromArgb(0xD6, 0xE4, 0xF0)` → `ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)`.
الأمثلة المؤكَّدة: `frmCustomerStatement.cs:54` · `frmJournalEntries.cs:110` · `frmListSales.cs:17` · `frmListPurchases.cs:21` · `frmStocktakingNew.cs:49`. العدد الكامل يُستخرج بأمر واحد وقت التنفيذ (§6).

---

### 4.3 تفصيل `GAP-03` — 51 لوناً حرفياً في 16 ملفاً

| اللون | العدد | الموقع | التصنيف |
|---|---|---|---|
| `(190,195,200)` | 5 | `frmStocktakingPrint:136` · `frmStocktakingPrintAdjustment:128` · `frmStocktakingPrintCard:156` · `frmWarehouseTransferPrint:134` · `frmPurchaseInvoicePrint:143` | (أ) ثيم — خط/حد |
| `(39,174,96)` | 4 | `frmCustomerStatement:456,576,594,606` | (ب) أخضر دلالي |
| `(25,135,84)` | 4 | `frmStocktakingNew:35` · `frmWarehouseTransferNew:35` · `frmListPurchases:765` · `frmListSales:756` | (ب) أخضر دلالي |
| `(52,73,94)` | 4 | `frmCustomerStatement:457,579,607,619` | (ب) محايد داكن |
| `(176,42,55)` | 3 | `frmListInventoryBatches:578` · `frmListPurchases:766` · `frmListSales:757` | (ب) أحمر دلالي |
| `(192,57,43)` | 3 | `frmCustomerStatement:454,586,604` | (ب) أحمر دلالي |
| `(255,200,200)` | 2 | `frmInventoryReport:171` · `frmLowStockReport:120` | (ب) خلفية تحذير |
| `(13,110,253)` | 2 | `frmListPurchases:767` · `frmListSales:758` | (ب) أزرق دلالي |
| `(33,37,41)` | 2 | `frmJournalEntries:1041,1042` | (أ) نص |
| `(122,90,0)` | 2 | `frmJournalEntries:1029,1030` | (ب) تحذير |
| `(241,242,246)` · `(108,117,125)` · `(41,128,185)` | 1+1+1 | `frmCustomerStatement:561,562,575` | (أ)+(ب) |
| `(211,84,0)` · `(142,68,173)` | 1+1 | `frmCustomerStatement:577,578` | (ب) |
| `(255,252,224)` · `(192,0,0)` | 1+1 | `frmJournalEntries:1028,1144` | (ب) |
| `(173,181,189)` | 1 | `frmWarehouseTransferNew:36` | (أ) |
| `(255,250,205)` | 1 | `frmProductUnitMaster:370` | (ب) |
| `(220,53,69)` | 1 | `frmStocktakingNew:36` | (ب) |
| `(255,252,230)` | 1 | `frmListSales:384` | (ب) |
| `(33,43,54)` · `(46,204,113)` · `(231,76,60)` · `(230,126,34)` · `(52,152,219)` · `(220,220,220)` | 6 | `Forms/ToastForm.cs:36-39,51,58` | (ب) إشعارات |
| `(33,150,243)` · `(180,Color.White)` | 2 | `Controls/Common/LoadingOverlayControl.cs:33,64` | (ج) شفافية — مقبول |
| `(224,227,231)` | 1 | `Controls/Common/SearchBarControl.cs:109` | (أ) |

**الخلاصة:** **4 أخضر و5 أحمر و3 أزرق** لنفس المعنى الدلالي — أي أن نفس «الرصيد المدين» يظهر بأربعة أحمر مختلفة. الإصلاح: كتلة ثوابت واحدة في `GreenThemeHelper` (`StatusSuccess` · `StatusDanger` · `StatusWarning` · `StatusInfo` · `StatusNeutral` · `StatusMuted` + خلفياتها) ثم استهلاكها.

---

### 4.4 تفصيل `GAP-08` — التفقيط

| الشاشة | حقول رقمية | الحكم |
|---|---|---|
| `frmProductMaster` | 5 (`NumericUpDown`) | ❌ يجب التفقيط (أسعار) |
| `frmProductUnitMaster` | 5 | ❌ يجب التفقيط (أسعار وحدات) |
| `frmInventoryBarcode` | 1 | ⚠️ يحتاج حكماً (كمية لا مبلغ) |
| `frmWarehouseTransferNew` · `frmStocktakingNew` · `frmCashBoxTransactions` | `TextBox` مالي | ⚠️ يحتاج حكماً |

---

### 4.5 تفصيل `GAP-08b` — `errorProvider` بلا تغذية بصرية (20)

`frmLogin` · `frmAccounts` · `frmJournalEntries` · `frmAddUpdateCustomerReceipt` · `frmAddUpdateExpense` · `frmAddUpdateInventoryAdjustment` · `frmInventoryAddProduct` · `frmInventoryBarcode` · `frmOpeningStock` · `frmStocktakingNew` · `frmSupplierPaymentDialog` · `frmPOS` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseInvoice` · `frmPurchaseItem` · `frmPurchaseReturn` · `frmSalesReturn` · `frmAddUpdateSalesInvoice` · `frmPasswordChange`.

---

### 4.6 تفصيل `GAP-14` — شبكات بلا `ContextMenuStrip` (13)

`frmAddUpdateCustomerReceipt` · `frmCustomerSelection` · `frmAddUpdateExpense` · `frmInventoryAddProduct` · `frmPOS` · `frmImportProductsExcel` · `frmProductMaster` · `frmProductSelection` · `frmProductUnitMaster` · `frmPurchaseInvoice` · `frmPurchaseReturn` · `frmSalesReturn` · `frmSupplierSelection`.

> شبكات المستندات (`frmPurchaseInvoice` · `frmPurchaseReturn` · `frmSalesReturn` · `frmPOS`) شبكات إدخال سطور — `ContextMenuStrip` فيها قرار مالك لا إصلاحاً تلقائياً.

---

### 4.7 تفصيل `GAP-15` — حقول بلا `Color.White` في المصمّم (37 حقلاً / 18 مصمّماً)

| المصمّم | حقول | معلن أبيض | الفرق |
|---|---|---|---|
| `frmProductMaster.Designer.cs` | 14 | 3 | **11** |
| `frmProductUnitMaster.Designer.cs` | 9 | 1 | **8** |
| `frmPurchaseInvoice.Designer.cs` | 6 | 0 | **6** |
| `frmEmployeeMaster.Designer.cs` | 10 | 4 | **6** |
| `frmSalesReturn.Designer.cs` | 5 | 0 | **5** |
| `frmPurchaseReturn.Designer.cs` | 5 | 0 | **5** |
| `frmListPayments.Designer.cs` | 5 | 0 | **5** |
| `frmListSales.Designer.cs` | 5 | 1 | **4** |
| `frmWarehouseMaster.Designer.cs` | 8 | 4 | **4** |
| `frmListPurchases.Designer.cs` | 5 | 1 | **4** |
| `frmListReturns.Designer.cs` | 5 | 2 | **3** |
| `frmUserMaster` · `frmUnitMaster` · `frmCashBoxMaster` · `frmCategoryMaster` | — | — | 2 لكلٍّ |
| `frmListSupplierPayments` · `frmCustomerSelection` · `frmPurchaseItem` | — | — | 1 لكلٍّ |

> إجمالي حقول الإدخال في المصمّمات: **367**. المعلن أبيض: **404** تصريحاً (يشمل شبكات/لوحات). **الصافي الناقص 37 حقلاً.**

---

## 5. خطة التطبيق (الموجات)

| الموجة | النطاق | الوكلاء | معيار الإغلاق |
|---|---|---|---|
| **W0 — أساس البيوت** | `GAP-09` (ألوان `ThemeHelper`/`ReportUiStandardizer`/`clsInputFeedback` → `ThemePalette`) · `GAP-01` قرار `clsValidation` · `GAP-05` · `GAP-13` · `GAP-16` · `GAP-07` | `ui-agent` + `fast-agent` | بناء 0 أخطاء |
| **W1–W7 — لون الحقل النشط** | `GAP-02` — 56 ملفاً، 7 دفعات × 8 | `ui-agent` ×7 | بناء بعد كل دفعة (`RULE-N157`) |
| **W8 — ثوابت وحزم** | `GAP-03` ثوابت الحالة · `GAP-06` تصدير · `GAP-08` تفقيط · `GAP-08b` تغذية · `GAP-14` قوائم سياق · `GAP-15` حقول بيضاء · `GAP-10` طباعة · `GAP-11` Mock · `GAP-12` `MoneyTextBox` | 4 وكلاء/موجة | بناء 0 أخطاء |
| **W9–W12 — شاشات التنسيق** | `GAP-04` — 67 شاشة / 46 وكيلاً (`FORMAT-PROGRESS`) | `ui-agent` | بناء بعد كل موجة |
| **W13 — التحقق** | `mock-qa` (تغطية) + `code-reviewer` واحد على الشريحة كاملة | — | 0 Blocker · 0 Major |

### 5.1 حالة التنفيذ الفعلية (2026-11-14 — البناء **0 أخطاء**)

| الموجة | الحالة | ما نُفِّذ فعلاً |
|---|---|---|
| **W0** | ✅ | `GAP-05` · `GAP-07` · `GAP-09` (الأساس) · `GAP-13` (كاذبة) · `GAP-16` · `GAP-17` · `GAP-18` |
| **W0.2** | ✅ | `GAP-19` (`ThemeHelper`: صفر قيمة مكرّرة + `BorderColor→GridLine` + `TextSecondary→StatusMuted`) · `GAP-20` (`ExportHelper` يقرأ `clsFormat` في المسارين، احتياطي واحد موثَّق) · إضافة 5 ثوابت خلفيات حالة |
| **W1–W2** | ✅ | `GAP-02` — **55 ملفاً** (7 دفعات: 4+3) · صفر `0xD6E4F0` في `Screens/**` · الحقول `static readonly` صارت خصائص حيّة |
| **W3** | ✅ | **تكافؤ الرفض**: `ف-4` · `ف-6`…`ف-19` في 14 ملف Mock + دورة مخزون صحيحة (خصم عند الترحيل · عكس عند الإلغاء) |
| **W4** | ✅ | `GAP-03`/`GAP-23` (18 ملفاً → ثوابت الحالة) · `GAP-08` (تفقيط على شاشتَي الأسعار؛ `frmInventoryBarcode` قرار: حقل كمية ⇒ لا تفقيط) · `GAP-11` · `GAP-12` |
| **W5** | ✅ | `GAP-14` — `cmsRows` على 7 شبكات (شاشتان بلا شبكة ⇒ لا ينطبق) |
| **W6–W7** | ✅ | `GAP-01` + `GAP-08b` — 20 شاشة: كل `SetError` مقترن بـ`ShowRejectTooltip` · `clsValidation` صار مستهلكاً (`IsRequired` · `IsPositiveDecimal` · `TryParseDecimal`) |
| **W8** | ✅ | `GAP-15` — **74 سطر أبيض** في 17 مصمّماً (قياس المراجع 37 كان خاطئاً: صيغة `System.Drawing.Color.White` لم تُحتسب) · مطابقة `MockAuthDesktopService` لقانون الدخول المحدَّث |
| **W9–W10** | ✅ | `GAP-04` — **49 شاشة** في 4 دفعات (`docs/FORMAT-PROGRESS.md`) · صفر صيغة عرض حرفية في الشاشات |
| **W11** | ✅ | `code-reviewer` واحد: **0 Blocker · 2 Major · 7 Minor** — Major‑1/Major‑2 (دورة المخزون) وMajor‑3 (تكرار المقارنة) أُغلقت، والبنود الأربعة ذات الدلالة أُغلقت |
| **W12** | ⏳ | `mock-qa` إعادة تصديق بعد الموجة — **لم تُنفَّذ بعد**؛ بوابة 0c تنتظرها |

### 5.2 البنود المفتوحة بعد الموجة

| # | البند | المالك | لماذا مفتوح |
|---|---|---|---|
| 1 | `GAP-06` — 36 شاشة بلا `ExportHelper` | `ui-agent` | **قرار مالك**: أي شاشة قائمة تُصدَّر وأي محرّر لا. لم يُنفَّذ تصدير أعمى |
| 2 | `GAP-10`/`GAP-21` — صيغ حرفية في `Business/**` + `DataAccess/**` + `Desktop/Printing` | `printing` + `backend-architect` | **الباك-إند مجمَّد** (`RULE-N177`) حتى الموافقة المكتوبة |
| 3 | `GAP-24` — `MockAuthService : IAuthService` بلا مستهلك | `fast-agent` | الحذف يحتاج قرار المالك (احتياط أو إزالة) |
| 4 | `clsFormat.MoneyPattern/QuantityPattern/NumberPattern/Ratio` | `ui-agent` | يلزم لإزالة صيغ `DefaultCellStyle.Format` من 14 شاشة (تفصيله في `FORMAT-PROGRESS.md` §4) |
| 5 | `MockStockLedger` مُعلَن داخل `MockSalesInvoiceDesktopService.cs` | `ui-agent` | يحتاج ملفاً مستقلاً في `Services/Mock/` — بيت واحد باسمه |
| 6 | إعادة تصديق `mock-qa` | — | بوابة 0c: لا تسليم Stage A للمالك قبل حكمها |

---

## 6. بروتوكول التحديث والتنفيذ

1. أي مخالفة جديدة تُضاف كصف `GAP-xx` **بقياس فعلي** (ملفات · مواضع) لا بتقدير.
2. `الحالة`: `⬜ لم يبدأ` · `🔄 جارٍ` · `✅ مكتمل`.
3. استخراج قائمة دفعة `GAP-02` التنفيذية:
   `Select-String -Path Screens\**\*.cs -Pattern 'FromArgb\(0xD6,\s*0xE4,\s*0xF0\)' -List`
4. **ممنوع** نسخ أرقام `docs/MOCK-COVERAGE.md` أو `docs/FORMAT-COVERAGE.md` هنا (`RULE-N174`).
5. كل `Mock*` يمسّه عمل ⇒ تحديث `docs/MOCK-COVERAGE.md` في نفس الموجة.
6. **قفل الباك-إند ساري** (`RULE-N177`): لا `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` قبل شهادة تغطية + موافقة مكتوبة. كل عمل هذا السجل **داخل Stage A** — `UseMockData = true` ولا حفظ حقيقي.

---

## 7. دليل مراجعة المالك (يدوي)

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | أي شاشة إدخال | `Tab` بين الحقول | الحقل المركَّز ياخذ لوناً واحداً في كل الشاشات |
| 2 | `frmChangeSystemColorsSettings` | غيّر «لون الحقل النشط» \(+\) احفظ | الحقل المركَّز في الشاشات المفتوحة يتغيّر فوراً |
| 3 | `frmFormatingSetting` | منازل المبالغ \(\to\) 3 \(+\) احفظ | كل مبلغ في كل شبكة يظهر 3 منازل بلا `F5` |
| 4 | `frmCustomerStatement` | اعرض كشفاً | المدين أحمر والدائن أخضر — **باللون نفسه** في `frmListSales` و`frmJournalEntries` |
| 5 | `frmProductMaster` | أضف صنفاً بسعر | يظهر تلميح التفقيط بالحروف عند إدخال السعر |
| 6 | `frmListSales` | صدّر Excel | الملف يفتح والأرقام بمنازل الإعداد |
| 7 | `frmDashboard` | افتحها | لا رمادي — الخلفية خضراء كالمصمّم |
| 8 | أي شبكة | انقر يميناً على سطر | قائمة السياق تظهر (حيث تنطبق) |
| 9 | `frmLogin` | `Tab` بين الحقول | ينزل للأمام لا يعكس |
| 10 | أي تقرير | تنقّل | الشريط السفلي يعمل والنموذج الحالي لا يُغلق |
| 11 | أي رسالة خطأ | أدخل بيانات ناقصة | رسالة عربية موحّدة + تلميح رفض على الحقل |

---

## 8. جولة المراجعين المستقلين (2026-11-14)

> مراجعتان قراءة-فقط على النظام كله: `code-reviewer` (قانون المكان الواحد) و`mock-qa` (تغطية الوضع التجريبي). كل رقم أدناه **مقيس بأمر**، لا مقدَّر. النتائج **مُدمجة** في هذا السجل كفجوات `GAP-19`…`GAP-24` وتصحيحات على فجوات قائمة.

### 8.1 تصحيحات على السجل نفسه

| البند | ما قاله السجل | ما قاسه المراجع | الإجراء |
|---|---|---|---|
| `GAP-09` | ✅ مكتمل | مغلق فعلاً في `ReportUiStandardizer` (0 لتر) و`clsInputFeedback` (0 لتر)، **لكن `ThemeHelper` ما زال يحمل 5 لترات لونية + 4× `Color.White`** | **يُعاد فتحه جزئياً** → `GAP-19` |
| `GAP-07` | ✅ مكتمل | المسار الأساسي سليم، لكن `ExportHelper.cs:147` `FallbackDateFormat = "yyyy-mm-dd"` بقي حرفياً ويُستهلك في 3 مواضع | **يُعاد فتحه جزئياً** → `GAP-20` |
| `GAP-10` | 3 مواضع · مالك `printing` | **26 موضعاً**: `PrintHelper` (2) · `InvoicePrinter` (1) · **`Business\FinancialReportService` (18)** · `Business\Printing\PrintDataService` (1) · `DataAccess\Printing\A4InvoiceDocument` (1) · `ThermalReceiptGenerator` (3) | **يُوسَّع** → `GAP-21`، والمالك `printing` + `backend-architect` |
| `GAP-15` | 37 حقلاً / 18 مصمّماً | قياس المراجع (368 حقلاً · 996 تصريح أبيض) **لا يعيد إنتاج** 37/18 ولا ينفيه — يحتاج عدّاً حقلاً بحقل | **معلَّق على قياس دقيق** قبل W8 — لا يُبنى عليه إصلاح أعمى |
| `GAP-02` | 56 ملفاً | 55 ملفاً بالعدّ الحالي؛ **المواضع الحيّة 3 فقط مقابل 44 حرفياً** | يُضاف الرقم للسجل؛ التنفيذ 55 |
| أرقام الاستهلاك | `clsDialogs` 88/863 · `clsGlobal` 6/10 · `clsFormat` 29 · `KeyboardHelper` 65/405 | **90/867 · 7/13 · 32 · 70/419** | تُحدَّث في التحديث القادم |
| `GAP-13` | إيجابية كاذبة | مؤكَّدة: `UseVisualStyleBackColor = true` = **0** في المشروع | تبقى مغلقة |

### 8.2 فجوات جديدة من مراجعة المكان الواحد

| # | المخالفة | القاعدة | ملفات | مواضع | المالك | الحجم | الموجة | الحالة |
|---|---|---|---|---|---|---|---|---|
| **GAP-19** | `ThemeHelper` ما زال يحمل 5 لترات (`BorderColor #DEE2E6` · `SidebarBg (33,43,54)` · `TextSecondary (108,117,125)` · `TextDarkGray (50,50,50)` · `GhostHoverBack (240,240,240)`) + `btn.ForeColor = Color.White` **مكتوبة 4 مرات** بدل ثابت واحد — تكرار داخل البيت نفسه | `RULE-N54a` · `RULE-N171` | 1 | 9 | `ui-agent` | S | W0.2 | ⬜ |
| **GAP-20** | `ExportHelper.cs:147` `FallbackDateFormat = "yyyy-mm-dd"` — مسار الرجوع لا يقرأ `clsFormat` (يُستهلك في 3 مواضع) | `RULE-N169` · `RULE-N179` | 1 | 1 | `fast-agent` | S | W0.2 | ⬜ |
| **GAP-21** | صيغ `N0/N1/N2/N3/F2` حرفية في **BLL وDataAccess** — 23 موضعاً خارج الـDesktop (18 منها في `FinancialReportService.cs:137-275`) | `RULE-N179` | 4 | 23 | `printing` + `backend-architect` | M | W8 | ⬜ |
| **GAP-23** | 9 لترات ألوان حالة/ثيم خارج `GreenThemeHelper`: `Forms\ToastForm.cs` (6) · `Controls\Common\LoadingOverlayControl.cs` (2) · `Controls\Common\SearchBarControl.cs:109` (1) — **أربعة أخضر وأحمرين** لنفس المعنى | `RULE-N54` · `RULE-N172` | 3 | 9 | `ui-agent` | S | W8 | ⬜ |
| **GAP-24** | `MockAuthService : IAuthService` (BLL) بلا مستهلك في أي وضع بعد إنشاء `MockAuthDesktopService` — مرشّح للإزالة | `RULE-N170` | 1 | 1 | `fast-agent` | S | بانتظار قرار | ⬜ |

> **قرار بنيوي معلَّق (`RULE-N54a`):** المراجع يرى أن ثوابت الحالة السبع (`StatusSuccess`…) يجب أن تعيش في `ThemePalette` لا في `GreenThemeHelper`، لأن `ThemePalette` هو مالك **قيم** الألوان. التنفيذ الحالي أضافها في `GreenThemeHelper` بـ`FromArgb` محلي لتجنّب ripple على `Definitions`. **يبقى قرار المالك** — لا يُغيَّر بلا أمر صريح.

### 8.3 إعادة تصديق تغطية الوضع التجريبي (`mock-qa`)

| القياس | الناتج |
|---|---|
| عقود البيانات | **33** (37 واجهة − 4 تحتية) |
| `Mock*DesktopService` | **33** — مطابقة 1:1، لا عقد يتيم ولا Mock يتيم |
| مسجَّل في فرع `UseMockData` | **30** + **3** «Mock-only by design» (`IEmployee` · `IThemeColor` · `IFormatSettings`) · **0** مسجَّل في فرع Direct دائماً |
| بيانات أولية | **33/33** (14 ملف `*Data` + `MockData`) |
| تكافؤ الرفض كامل | **18** · ناقص **15** · غير مؤهل **0** |
| الحكم الإجمالي | **`مؤهل جزئياً — تكافؤ الرفض ناقص`** (تحسّن: لم تبقَ وحدة مجمَّدة) |

**خرجت من التجديد:** `Users` و`Backup` (كانتا `غير مؤهل`) — بفضل `MockAuthDesktopService` · `MockBackupDesktopService` + `MockBackupData`.

**فجوات التكافؤ المفتوحة (كلها `RULE-N167`، والمالك `ui-agent`):**

| الفجوة | الملف | المطلوب |
|---|---|---|
| ف-4 | `MockSupplierPaymentDesktopService.cs` | رفض «المدفوع > المتبقي» · `CancelAsync` تحذف بدل حالة إلغاء (`RULE-004`) |
| ف-6 / ف-7 | `MockSalesInvoiceDesktopService.cs` · `MockPurchaseInvoiceDesktopService.cs` | كمية `0` · مدفوع > الإجمالي · بنود فارغة · مخزون سالب · خصم المخزون |
| ف-8 / ف-9 | `MockSalesReturnDesktopService.cs` · `MockPurchaseReturnDesktopService.cs` | كمية `0` · إرجاع > المُباع/المشترى |
| ف-10 | `MockCustomerReceiptDesktopService.cs` | مبلغ `0` · مبلغ > المتبقي |
| ف-11 | `MockExpenseDesktopService.cs` | مبلغ `0` أو سالب |
| ف-12 | `MockInventoryAdjustmentDesktopService.cs` | كمية سطر `0` · خصم > الرصيد |
| ف-13 … ف-17 | `MockProduct` · `MockCustomer` · `MockSupplier` · `MockWarehouse` · `MockCashBox` | تكرار الاسم (والباكود/الهاتف حيث ينطبق) |
| ف-18 | `MockInventoryBatchDesktopService.cs` | حقول إلزامية · كمية `0` |
| ف-19 | `MockSettingsDesktopService.cs` | دالتا الكتابة تنجحان دائماً (لا مستهلك اليوم — أولوية منخفضة) |

> إغلاق هذه الفجوات **داخل الـMock فقط** (`RULE-N176`) — لا باك-إند. وتُحدَّث `docs/MOCK-COVERAGE.md` في الموجة نفسها (`RULE-N174`) بالأرقام الجديدة **18 / 15 / 0**.

### 8.4 ثبت سليم بالقياس (لا عمل مطلوب)

`MessageBox.Show` خارج `clsDialogs` = **0** · `RowFilter` خام = **0/26** · لترات التعطيل (224/150) في الشاشات = **0** · منطق طي التصفية خارج `FilterPanelHelper` = **0** · إعادة تنفيذ التفقيط = **0** · حساب WCAG خارج `ThemePalette` = **0** · `SystemColors` حقيقي = **0** · `UseVisualStyleBackColor = true` = **0** · مراجع Helpers داخل المصمّمات = **65 كلها تعليقات** · إنشاء عناصر دائمة داخل Helpers = **0** · `SalesSystem.Business` في `Screens/**` = **0** · `HttpClient` = **0** · `new frmXxx()` = **0** · `Direct*Service` أغلفة رقيقة (أكبرها 245 سطراً، 0–1 حلقة منطق) · `clsDialogs`/`KeyboardHelper`/`clsFormat`/`LiveTafqeetHelper`/`clsInputFeedback`/`clsGlobal`/`BackupFileNameHelper`/`ColorSuggestionHelper`/`ArabicNumberToWords` **كلها مستهلكة — لا بيوت ميتة عدا `clsValidation`** (مُعتمدة في W8) · `BackupFileNameHelper` صيغة طابع زمني لا صيغة عرض (**مقبولة**).

### 8.5 موجات التنفيذ المعدَّلة

| الموجة | النطاق | الوكلاء |
|---|---|---|
| **W0** ✅ | `GAP-05` · `GAP-07` · `GAP-09` (الأساس) · `GAP-13` (كاذبة) · `GAP-16` · `GAP-17` · `GAP-18` | منفَّذة · **0 أخطاء** |
| **W0.2** | `GAP-19` · `GAP-20` | `ui-agent` + `fast-agent` |
| **W1–W2** | `GAP-02` — 55 ملفاً · 7 دفعات × 8 (4 متوازية ثم 3) | `ui-agent` ×7 |
| **W3** | **تكافؤ الرفض** — ف-4 · ف-6…ف-19 (14 ملف Mock) + تحديث `MOCK-COVERAGE` | `ui-agent` ×4 |
| **W4** | `GAP-03` ثوابت الحالة · `GAP-23` · `GAP-08` تفقيط · `GAP-08b`+`GAP-01` اعتماد `clsValidation` · `GAP-14` قوائم سياق | `ui-agent` ×4 |
| **W5** | `GAP-06` تصدير · `GAP-15` حقول بيضاء (بعد قياس دقيق) · `GAP-12` `MoneyTextBox` | `ui-agent` |
| **W6** | `GAP-10`+`GAP-21` صيغ الطباعة وBLL · `GAP-11` · `GAP-24` | `printing` + `backend-architect` + `fast-agent` |
| **W7–W10** | `GAP-04` — 67 شاشة / 240 موضعاً (`FORMAT-COVERAGE`) | `ui-agent` ×4/موجة |
| **W11** | `mock-qa` إعادة تصديق + `code-reviewer` واحد | — |
