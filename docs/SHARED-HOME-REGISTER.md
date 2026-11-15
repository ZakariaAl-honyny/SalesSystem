# سجل البيوت المشتركة (Shared Home Register)

> **الغرض:** حصر **كل** كود مشترك في النظام — بيته الواحد، ما يعرضه، ومن يستهلكه فعلاً، وكل مخالفة قائمة بالأرقام. هذا الملف هو **قائمة العمل الوحيدة** لموجة «وحّد وطبّق على النظام كله».
>
> **القانون الحاكم:** `AGENTS.md` §2.30 — `RULE-N169` (المكان الواحد) · `RULE-N170` (ابحث قبل أن تكتب) · `RULE-N171` (معيار التعديل الواحد) · `RULE-N172` (سقف التكرار).
>
> **البيوت:** `SalesSystem.Desktop/Global Classes/` · `SalesSystem.Desktop/Helpers/` + `SalesSystem.Common/Helpers/` + `SalesSystem.Business/Helpers/`.
>
> **حدود هذا الملف:** يحمل **الحصر** و**المخالفة** و**مكان الإصلاح**. تفاصيل شاشات التنسيق (67 شاشة × 240 مخالفة) مصدرها الوحيد `docs/FORMAT-COVERAGE.md`، ولوحة توزيع العمل في `docs/FORMAT-PROGRESS.md` — لا تُنسخ هنا (`RULE-N174` بالقياس).
>
> **آخر تحديث:** 2026-11-15 · **المرجع الملزم للقياس: §10.6.5** (القياس النهائي بعد الموجات 1–12) · البناء **0 أخطاء / 125 تحذيراً**. §3–§8 محضر تاريخي لموجة 2026-11-14، و§10.5/§10.6.1–§10.6.2 تجاوزها §10.6.5.

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
| `GAP-09` | حذف `FocusHighlight = 0xD6E4F0` واستهلاك `ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)` — والمفتاح نفسه صار مفتاحاً واحداً للتحديد والحقل النشط (2026-11-15) | `Helpers/ReportUiStandardizer.cs` |
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
| **GAP-02** | تمرير `Color.FromArgb(0xD6, 0xE4, 0xF0)` حرفياً إلى `WireFocusHighlight` بدل `ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)` — ✅ **مغلقة (2026-11-15): صفر موضع حرفي في الحل كله**، والشاشات الـ58 كلها تمرّر `ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)` بعد دمج مفتاح الحقل النشط في مفتاح التحديد | `RULE-N54b` · `RULE-N112` | **56** | **0** | `ui-agent` | S | W1–W7 | ✅ مكتمل |
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
| `ReportUiStandardizer.cs` | 97 | `FocusHighlight = (0xD6, 0xE4, 0xF0)` — **يكرّر `ThemePalette.SelectionRowColor`** | يُحذف ويُستهلك `ThemePalette.DefaultFor` |
| `clsInputFeedback.cs` | 26 | `RejectFlashBack = (255, 220, 220)` — لون حالة | ثابتاً مسمّى في `GreenThemeHelper` |

**ملاحظة:** `ColorSuggestionHelper` **بريء** — كل قواعده تقرأ `ThemePalette` (`ContrastRatioRaw` · `MinimumContrastFor` · `ContrastPartnerColor` · `DefaultFor`)، وقائمة اقتراحاته كتالوج اختيارات لا افتراضيات ثيم. وحساب WCAG يعيش **مرة واحدة** في `ThemePalette.RelativeLuminance`/`ContrastRatioRaw` — لا نسخة ثانية.

---

### 4.2 تفصيل `GAP-02` — 56 ملفاً (دفعات 7 × 8)

`Screens/**` — كل ملف يستدعي `KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0))` أو يعرّف `FocusColor` ثابتاً بالقيمة نفسها. الإصلاح **متطابق حرفياً** في كل ملف:
`Color.FromArgb(0xD6, 0xE4, 0xF0)` → `ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)`.
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

---

## 9. موجة 2026-11-15 — البيت الواحد لتلوين الأسطر + باك-إند الإعدادات

### 9.1 بيت جديد: حالة السطر في الشبكة

| # | البيت | ماذا انتقل إليه | المستهلكون | القياس |
|---|---|---|---|---|
| H-01 | `Helpers/GridRowHelper.cs` | قراءة خلية بالاسم **المنطقي** (العمود بالاسم ثم `DataPropertyName`) + `ApplyInactiveRowStyle` (ترميم وراثة السطر ثم تلوين غير النشط) | 8 ماسترات: الصناديق · الفئات · العملاء · الموردون · الوحدات · المخازن · الموظفون · الأصناف · المستخدمون (+ شاشة الوحدات بقاعدة «الوحدة الأساسية») | `row.Cells["<حقل منطقي>"]` في الماسترات = **0** · `Color.MistyRose`/`DarkRed` فيها = **0** · كتلة تلوين مكرّرة = **0** |
| H-02 | `Helpers/GreenThemeHelper.cs` | `InactiveRowBack` · `InactiveRowFore` (دلالة «سجل غير نشط» لا «حقل معطّل») | `GridRowHelper` وحده | صفر لون حرفي في `GridRowHelper` |

**السبب المقيس:** `DataGridViewCellCollection` يحلّ بالاسم لا بـ`DataPropertyName`، فـ`Cells["IsActive"]` رمت `ArgumentException` وأسقطت تحميل الشاشة (سجل 2026-11-15). القاعدة الجديدة `RULE-N179` في `AGENTS.md` §2.33.

### 9.2 باك-إند الإعدادات (Stage B بموافقة مالك مكتوبة 2026-11-15)

| # | الصف | البيت / العقد | ملاحظة |
|---|---|---|---|
| S-01 | التحقق من مجموعة الألوان + التطبيق الذرّي | `ThemePalette.ValidateAll` · `ThemePalette.TryApplyAll` (`string?` = أول رفض عربي أو `null`) | نُقلا من `MockThemeColorDesktopService` — صفر نسخة ثانية (`RULE-N172`) |
| S-02 | التحقق من إعدادات التنسيق + التطبيق الذرّي | `clsFormat.ValidateAll` · `clsFormat.TryApplyAll` | كانا في بيتهما من قبل — الخدمة الحقيقية تستهلكهما فقط |
| S-03 | الحفظ الحقيقي | `Services/Direct/DirectThemeColorService` · `DirectFormatSettingsService` → `IStoreSettingsService` (قائم) → `SystemSettingsRepository` → جدول `SystemSettings` | بلا كيان جديد · بلا ترحيل · بلا خدمة BLL جديدة |
| S-04 | التحميل عند الإقلاع | `Program.cs` (وضع حقيقي فقط) → `ThemePalette.TryApplyAll` / `clsFormat.TryApplyAll` قبل شاشة الدخول | فشل التحميل = سجل + قيم افتراضية، لا حجب إقلاع |

### 9.3 فجوات مسجَّلة للفحص (لم تُصلح)

| # | الفجوة | الموضع | الأثر | المالك المقترح |
|---|---|---|---|---|
| P-13 | `IReportRepository` · `IPrintService` · `IPrintDataService` غير مسجَّلة في DI، و`PrintingBootstrapper.Initialize()` لا يُستدعى | `Program.cs` | الوضع الحقيقي: التقارير وفاتورة البيع/الطباعة تُفشل بالحلّ | `fast-agent` (سطر DI) — بعد موافقة الوحدة |
| P-14 | تغيير «fail-closed» لتهيئة قاعدة البيانات (رسالة + `Environment.ExitCode = 1`) موجود في شجرة العمل من موجة متزامنة ولم تمرّ بمراجعة هذه الموجة | `Program.cs:399-437` | سلوك إقلاع جديد يحتاج اعتماد المالك صريحاً | قرار مالك |
| P-15 | `Color.MistyRose`/`DarkRed` باقية كألوان حالة في `frmListSales` · `frmAccounts` · `frmJournalEntries` · `frmAddUpdateSalesInvoice:2960` | 4 شاشات | مخالفة `RULE-N54` (لون غير معلن) — لا انهيار | `ui-agent` (S) |

---

## 10. لوحة المهام المفتوحة — مراجعة وحدة بوحدة (2026-11-15)

> **الطريقة:** 95 شاشة + 96 مصمّماً في `Screens/**` قُرئت وحدة بوحدة بأربعة مراجعين مستقلين (`code-reviewer` قراءة فقط)، وكل بند أدناه **مقيس بـ`file:line`** لا مقدَّر. البناء بعد كل موجة = **0 أخطاء**.
>
> **الأولوية:** `أ` = فقدان بيانات/مال أو شاشة لا تُفتح · `ب` = سلوك خاطئ مرئي · `ج` = بيت واحد/نظافة.
> **المرحلة:** `A` = قابل للتنفيذ الآن داخل الـMock/الشاشة · `B` = يحتاج باك-إند أو عضواً في عقد ⇒ **مجمَّد** حتى موافقة الوحدة المكتوبة (`RULE-N177`).

### 10.1 مفتوح — المرحلة A (شاشة/محاكاة، بلا باك-إند)

| # | المهمة | الموضع | العيب المقيس | المالك | الحجم | الأهمية |
|---|---|---|---|---|---|---|
| **T-01** | استيراد الأصناف يقرأ السعر والتكلفة والرصيد الافتتاحي والتصنيف من الملف ثم **لا يرسلها**، ويثبّت `CategoryId: 1` — ونص النجاح «تم اعتماد N صنفاً» كاذب | `Screens/Products/Forms/frmImportProductsExcel.cs:308-315` (+148-152، +373-379) | كميات وأسعار تضيع صامتاً | `ui-agent` | M | أ |
| **T-02** | `frmPurchaseReturn` يرسل `UnitCost` (صفر دائماً) بدل `UnitPrice` | `Screens/Returns/Forms/frmPurchaseReturn.cs:427` | تكلفة سطر المرتجع تُحفظ صفراً | `ui-agent` | S | أ |
| **T-03** | «تعديل» في سندات **صرف الموردين** يفتح سنداً جديداً فارغاً (القبض أُصلح) | `Screens/Payments/Forms/frmListSupplierPayments.cs:521-525` | عنوان كاذب + مستند مكرر | `ui-agent` (S) — أو تعطيل الزر بلافتة صادقة حتى العقد | S | أ/ب |
| **T-04** | فشل `PostAsync` بعد نجاح `CreateAsync` في المرتجعات يبقي `_invoice` قديماً ⇒ `F4` ثانية تُنشئ مرتجعاً **ثانياً** | `frmSalesReturn.cs` · `frmPurchaseReturn.cs` (مسار الحفظ/الترحيل) | ازدواج أثر مخزون/رصيد | `ui-agent` | S | أ |
| **T-05** | مسودات الجرد والتحويل لا تُفتح بعد الحفظ (لا قائمة محاضر، و`GetAllAsync`/`GetByIdAsync` بلا مستهلك) | `IStocktakingDesktopService` · `IWarehouseTransferDesktopService` | دورة المستند ناقصة طرفها | `ui-agent` (شاشة قائمة) | M | ب |
| **T-06** | `MockStockLedger` يسجّل الحركة بنوع `Adjustment` وبلا مستند لأن مستهلكيه الستة لا يمرّرون السياق | 6 ملفات Mock (فاتورة بيع/شراء، مرتجعان، جرد، تحويل) | دفتر الحركات يظهر كل شيء «تسوية» | `ui-agent` | S | ج |
| **T-07** | `Apply` لا ينقص `Batches.QuantityRemaining` ⇒ انحراف بين الدفعات والرصيد بعد الترحيل | `Services/Mock/MockStockLedger.cs` | تقارير الدفعات/FIFO لا ترى الترحيل | `ui-agent` | M | ب |
| **T-08** | `MockReportDesktopService.BuildMovements` يشتق حركات ثانية من الفواتير بدل `MockInventoryData.Movements` | `Services/Mock/MockReportDesktopService.cs` (~680+) | بيتان للحركة (`RULE-N169`) | `ui-agent` | S | ج |
| **T-09** | `frmInventory` تعرض مجموعات Mock موازية بأسماء/حالات لا تطابق البذور | `Screens/Inventory/Forms/frmInventory.cs:215-282` | رصيد معروض ≠ الرصيد الحقيقي | `ui-agent` | M | ب |
| **T-10** | أزرار «ترحيل/إلغاء التسوية» مفعّلة على أي سطر؛ النقر على غير المطابق يصمت | `frmListInventoryAdjustments.cs:333-335,405,442` | آلة حالة ناقصة | `ui-agent` | S | ج |
| **T-11** | `frmInventoryAddProduct` غير موصول بأي شاشة (مسجَّل في DI بلا مستدعٍ) بعد أن صار حفظه حقيقياً | `Program.cs:379` · `frmMain` | ميزة يتيمة | `ui-agent` / قرار | S | ج |
| **T-12** | 4 نوافذ طباعة مخزون بلا مستدعٍ وبيانات مصنوعة (`INV-2026-001` · `TR-2026-001`) و«نجاح طباعة» بلا طابعة | `frmStocktakingPrint*.cs` · `frmWarehouseTransferPrint.cs` | واجهات كاذبة | `printing` / قرار | M | ب |
| **T-13** | 7 نماذج شراء ميتة مسجَّلة في DI ببيانات وهمية (`frmPurchaseItem/Discount/Currency/OtherDetails/ItemInfo/InvoicePrint` · `frmSupplierStatement`) | `Program.cs:381-391` | واجهات معروضة بلا وظيفة | قرار مالك (ربط أو سحب) | M | ج |
| **T-14** | `frmDailyClosureReport` يستدعي **نفس** استدعاء ملخص الصناديق ⇒ «الإقفال اليومي» = إجماليات كل الزمن | `Screens/Reports/Forms/CashBox/frmDailyClosureReport.cs:43` | قراءة مالية مضلِّلة | `ui-agent` (لافتة صادقة) + عقد `B` | S | أ |
| **T-15** | `frmProfitByCustomerReport` بلا عمود ربح، و`profit` محسوب «المتبقي» ثم غير مستخدم | `.../Sales/frmProfitByCustomerReport.cs:62` | تقرير بعنوان كاذب | قرار مالك (تسمية أو عقد ربح) | S | ب |
| **T-16** | 7 تقارير ترسل رسائل عبر `INotificationService` بينما 24 عبر `clsDialogs` | `frmCustomerBalance` · `frmSupplierBalance` · `frmInventory` · `frmLowStock` · `frmProductMovement` · `frmPurchases` · `frmSales` | بيتان للرسالة | `ui-agent` | S | ج |
| **T-17** | `frmPurchasesReport` يعيد تنفيذ تنسيق المال/التاريخ في `CellFormatting` | `.../Purchases/frmPurchasesReport.cs:138-154` | نسخة ثانية من `GridFormatHelper` | `ui-agent` | S | ج |
| **T-18** | تلوين الرصيد أحمر/أخضر مكرّر في 4 تقارير بلا بيت | `frmCustomerBalance` · `frmSupplierBalance` · `frmInventory` · `frmLowStock` | `RULE-N172` | `ui-agent` (يضيفه لـ`GridFormatHelper`) | S | ج |
| **T-19** | `frmTrialBalanceReport` 8 أعمدة مالية بلا أي تنسيق | `.../Accounting/frmTrialBalanceReport.cs:76-84` | بلا فواصل آلاف | `ui-agent` | S | ج |
| **T-20** | 18 تقريراً لا تتحقق `من <= إلى` | قائمة في تقرير المراجعة (18 موضعاً) | «لا توجد بيانات» بلا رسالة | `ui-agent` (بيت واحد) | S | ب |
| **T-21** | نسب (`TaxRate`/`ProfitMargin`) منسّقة `Money` ⇒ «15.00» كمبلغ | `frmVatReport.cs:80` · `frmProductProfitabilityReport.cs:72` · `frmSalesByProductReport.cs:82` | دلالة خاطئة | `ui-agent` | S | ج |
| **T-22** | تنسيق تاريخ موزّع على 3 آليات + 7 تقارير بلا تنسيق | 7 تقارير | `RULE-N169` | `ui-agent` | S | ج |
| **T-23** | معالجات موصولة فارغة في 4 تقارير (`label4_Click` · `loadingPanel_Paint` · `ValueChanged` بلا فعل) | `frmAccountStatementReport` · `frmWorkingCapitalReport` · `frmWarehouseMovementReport` · `frmProfitByCustomerReport` | كود ميت يخدع المصمّم | `ui-agent` | S | ج |
| **T-24** | `frmCashBoxTransactions` ترتّب `DocNo` نصياً فيحكم الرصيد الجاري | `Screens/CashBoxes/Forms/frmCashBoxTransactions.cs:451,495` | رصيد جارٍ بترتيب خاطئ داخل اليوم | `ui-agent` | S | ب |
| **T-25** | «الرصيد» في كشف العميل يُلوَّن من النص المنسّق (`TryParse` على "1,234.50") | `Screens/Customers/Forms/frmCustomerStatement.cs:603` | لون محايد دائماً | `ui-agent` | S | ج |
| **T-26** | أعداد الملخص خام (`ToString`) في 4 شاشات بدل `clsFormat.Number` | `frmUserMaster:533` · `frmCashBoxMaster:468` · `frmAccounts:737` · `frmCashBoxTransactions:470` | إعدادات الأعداد لا تصل | `ui-agent` | S | ج |
| **T-27** | `ctrlDashboard` يستخدم `{…:N2}` بدل `clsFormat.Money` | `Screens/Dashboard/Controls/ctrlDashboard.cs:156-166` | تنسيق لا يتبع الإعدادات | `ui-agent` | S | ج |
| **T-28** | 5 نسخ من `_ToNullIfEmpty` + 3 من `_Clamp` رغم وجود `StringExtensions.ToNullIfEmpty` | العملاء · الموردون · الحسابات · القيود · الموظفون | `RULE-N171` | `ui-agent` | S | ج |
| **T-29** | `CashCustomerName` و`_GetStatusText` مكرّران بين شاشات القوائم | `frmListSales:13` · `frmListReturns:13` · `frmListPurchases` | نص/منطق مكرّر | `ui-agent` | S | ج |
| **T-30** | رسالة «حذف (تعطيل)» في وحدات الصنف بينما التنفيذ `DeleteStrategy.Permanent` | `frmProductUnitMaster.cs:995` مقابل `1004` | رسالة تخالف الفعل | `ui-agent` | S | ب |
| **T-31** | `<count>` بـ`ToString()` في منتقي الأصناف | `frmProductSelection.cs:142,336` | `clsFormat.Number` | `ui-agent` | S | ج |
| **T-32** | معالج فارغ `dgvProducts_ChangeUICues` | `frmProductMaster.cs:1405-1408` | كود ميت | `ui-agent` | S | ج |
| **T-33** | 50 لوناً حرفياً في 21 شاشة (`MistyRose` 13 · `DarkRed` 13 · `Red` 6 · `Green` 4 · `ForestGreen` 3 …) — و`RULE-N179` يمنع `MistyRose`/`DarkRed`/`LemonChiffon` في الماسترات صراحةً | 21 ملفاً في `Screens/**` | بيوت الألوان لا تصلها | `ui-agent` ×2 | M | ج |
| **T-34** | `ThemeHelper` يحمل 4 ألوان حرفية باقية (`SidebarDark` · `TextDark` · `ButtonForeOnSolid` · `GhostHoverBack`) و`ExportHelper:194` `LastResortDateFormat` | بيتان | `RULE-N54a` | `fast-agent` | S | ج |
| **T-35** | `Esc` في `frmListPayments` يعيد «هذا الشهر» بينما الافتراضي و«مسح التصفية» = «4 أشهر» | `frmListPayments.cs:394-417` مقابل `107-116` | افتراضان لنفس التصفية | `ui-agent` | S | ج |
| **T-36** | `ShowDialog()` بلا مالك في `frmListPayments:788` (أُصلح) و`frmListSupplierPayments:433` (أُصلح) — يُتحقق | — | — | — | — | ✅ |
| **T-37** | مجلد فارغ `Screens/Payments/NewFolder` | — | نظافة | `fast-agent` | S | ج |

### 10.2 مفتوح — المرحلة B (عقد/باك-إند، **مجمَّد** حتى موافقة الوحدة المكتوبة)

| # | المهمة | الموضع | لماذا B | المالك |
|---|---|---|---|---|
| **T-38** | `ISalesReturnDesktopService` و`IPurchaseReturnDesktopService` بلا `UpdateAsync` ⇒ تعديل مسودة ينشئ مستنداً ثانياً (الشاشة محصَّنة الآن بحاجز المسودة فقط) | `Services/Interfaces/ISalesReturn*` · `IPurchaseReturn*` | عضو عقد جديد ⇒ `Direct*` مجمَّد | `backend-architect` |
| **T-39** | `ISupplierPaymentDesktopService` بلا `UpdateAsync` ولا مسار تعديل في `frmSupplierPaymentDialog` | نفس المبدأ | عقد + حفظ حقيقي | `backend-architect` + `ui-agent` |
| **T-40** | `IStocktakingDesktopService` · `IWarehouseTransferDesktopService` بلا تنفيذ حقيقي (Mock فقط) | `Program.cs` فرع Real | يحتاج BLL + EF + ترحيل | `database-engineer` → `backend-architect` |
| **T-41** | إقفال يومي حقيقي (عقد بتاريخ واحد) — التقرير اليومي الآن = ملخص الصناديق | `IFinancialReportDesktopService` | عقد تقرير جديد | `backend-architect` |
| **T-42** | ربح حسب العميل (عمود ربح حقيقي) | `ISalesReportDesktopService` | حقل DTO من الخدمة | `backend-architect` |
| **T-43** | تصنيف «متداول/غير متداول» لرأس المال العامل، وإشارة الرصيد الافتتاحي حسب `AccountNature` في دفتر الأستاذ، ومجاميع قائمة الدخل من الإشارة لا من نص عربي | `frmWorkingCapitalReport:65` · `frmGeneralLedgerReport:61` · `frmIncomeStatementReport:87-88` | منطق BLL | `backend-architect` |
| **T-44** | صيغ `N0/N1/N2/N3/F2` حرفية في BLL/DataAccess (23 موضعاً، 18 في `FinancialReportService`) + `PrintHelper`/`InvoicePrinter` | `Business/**` · `DataAccess/Printing/**` · `Desktop/Printing/**` | الباك-إند مجمَّد | `printing` + `backend-architect` |
| **T-45** | `IReportRepository` · `IPrintService` · `IPrintDataService` غير مسجَّلة في DI، و`PrintingBootstrapper.Initialize()` لا يُستدعى | `Program.cs` | P-13 أعلاه — الوضع الحقيقي يفشل بالحلّ | `fast-agent` (بعد موافقة) |
| **T-46** | مخاطر أمنية إنتاجية: تجاوز كلمة السر عند `PasswordHash` الفارغ · كلمة سر افتراضية مكتوبة · حفظ+ترحيل في معاملتين · `PermanentDeleteAsync` موصول · connection string نصي وDPAPI ميت | `Business/**` · `DataAccess/**` · `appsettings.json` | الباك-إند مجمَّد | `security-auditor` (مراجعة) + `backend-architect` |

### 10.3 قرارات المالك المطلوبة (لا تُنفَّذ بلا نص)

| # | القرار | الخيارات |
|---|---|---|
| **D-01** | `GAP-06` — تصدير Excel: أي الشاشات؟ | 36 شاشة: قوائم ⇐ تُصدَّر / محرّرات ⇐ لا (اقتراح) |
| **D-02** | 5 شبكات إدخال بـ`AllowUserToAddRows = true` خارج الاستثناءين الموثَّقين (`frmInventoryAddProduct.dgvUnits` · `frmStocktakingNew.dgvLines` · `frmPurchaseInvoice.dgvItems` · `frmSalesReturn.dgvItems` · `frmPurchaseReturn.dgvItems`) | توسيع `RULE-N22a` أو إبقاء الاستثناءين |
| **D-03** | 7 نماذج شراء ميتة (T-13) | ربط / سحب من DI والمشروع |
| **D-04** | 4 نوافذ طباعة مخزون بلا مستدعٍ (T-12) | توصيل / سحب |
| **D-05** | `frmProfitByCustomerReport` (T-15) | تسمية «المستحق حسب العميل» الآن / انتظار عقد الربح |
| **D-06** | `MockAuthService : IAuthService` بلا مستهلك (GAP-24) | إزالة / احتياط |
| **D-07** | موافقة مكتوبة لأي وحدة في §10.2 | نص + تاريخ (`RULE-N166`/`RULE-N177`) |

### 10.4 منفَّذ في موجة 2026-11-15 (مرجع، ليست مهام)

| # | ما نُفِّذ | القياس بعد التنفيذ |
|---|---|---|
| F-01 | `Helpers/GridRowHelper.cs` (بيت قراءة الخلية + تلوين غير النشط) واستهلاكه في 8 ماسترات + `frmCashBoxMaster` + `frmCategoryMaster` | `Cells["<حقل منطقي>"]` في الماسترات = 0 · الشاشتان تُفتحان بلا استثناء |
| F-02 | `clsDialogs` يطبّع رسالة/عنواناً فارغاً ⇒ 39 موضع `Error!` صارت آمنة بتعديل واحد | صفر صندوق فارغ |
| F-03 | فاتورة البيع: خصم السطر يُقرأ بـ`TryParseCellDecimal` · الإلغاء يحترم `Result` · المدفوع لا يُصفَّر · 4 معالجات ميتة أُزيلت | صفر `Convert.ToDecimal` على خلية |
| F-04 | فاتورة الشراء: خصم السطر يُرسل (`DiscountType.Amount`) · تعديل = `UpdateAsync` لا إنشاء · حاجز المسودة · الأزرار تُعطَّل لا تُخفى | إجمالي معروض = محفوظ |
| F-05 | المرتجعات: حاجز المسودة · `F3/F4` محروسان · ربط الطرف بعد تحميل القوائم · تحقق كمية/سعر | صفر `Error!` · لا مستند مكرر بـ`F4` |
| F-06 | كشف العميل: الفترة = أول الشهر الحالي (لا تاريخ مثبَّت) وتُضبط قبل التحميل | لا `2026,9,1` |
| F-07 | السندات/المصروفات: «تعديل» يفتح المستند المحدد · الحوار بمالك · سباق القوائم · منع حفظ سند غير مسودة · عمود مكرّر أُزيل | — |
| F-08 | المحاكاة: `Apply` يسجّل حركة في `Movements` · رفض تعطيل مخزن عليه أرصدة · رفض حذف وحدة مستخدمة · `SetDefaultAsync` يرفض بدل نجاح كاذب | صفر نجاح كاذب |
| F-09 | المخزون: FIFO وباركود وإضافة صنف تعمل من العقود لا من صفوف مصنوعة | صفر صفوف مصنوعة |
| F-10 | التحويل والجرد: زر «ترحيل» معلن في المصمّم · لا إغلاق عند حفظ المسودة · منع مستند ثانٍ | دورة مسودة→ترحيل قابلة للاستخدام |
| F-11 | الإعدادات: حفظ حقيقي في الوضع الحقيقي (المرحلة ب مكتملة) · التصفية بالمؤشر لا بنص عربي | — |
| F-12 | البناء بعد كل موجة | **0 أخطاء** (127 تحذيراً) |

### 10.5 إعادة قياس بعد التزامات المالك `1b7ba1d` · `e713a6a` · `622025b` · `c5c872c`

> ⚠️ **هذا الجدول تجاوزه الزمن — المرجع الحالي هو §10.6** (قياس 2026-11-15 بعد موجة الإغلاق). يُبقى للتاريخ فقط.
>
> الجدول أعلاه كُتب قبل تلك الالتزامات. القياس التالي **بعدها** (2026-11-15، البناء **0 أخطاء / 149 تحذيراً**):

| البند | القياس الآن | الحالة |
|---|---|---|
| `frmInventoryAddProduct` | حُذفت من الشجرة و`Program.cs` وصفر مرجع (`RULE-N102`) | ✅ أُغلق بـ`c5c872c` (بند `T-11`) |
| نسخ `_ToNullIfEmpty` الخاصة | **0** | ✅ أُغلق (بند `T-28`) |
| ألوان حرفية في الشاشات | **24 في 8 ملفات** (كانت 50/21): `frmInventory` 9 · `frmAddUpdateSalesInvoice` 4 · `frmImportProductsExcel` 3 · `frmInventoryList` 2 · `frmCustomerBalanceReport` 2 · `frmSupplierBalanceReport` 2 · `frmPurchaseInvoice` 1 · `frmInventoryBarcode` 1 | 🔄 متبقٍّ (بند `T-33`) |
| `ThemeHelper` | **4** ألوان حرفية | 🔄 متبقٍّ (`T-34`) |
| `T-01` استيراد الأصناف | `CategoryId: 1, // Mock` باقٍ · لا أسعار ولا رصيد افتتاحي | 🔴 مفتوح |
| `T-02` تكلفة مرتجع الشراء | `l.UnitCost` ما زال يُرسل (سطر 429) بينما السطر يعرض `UnitPrice` | 🔴 مفتوح |
| `T-14` الإقفال اليومي | ما زال `GetCashBoxSummaryAsync()` | 🔴 مفتوح |
| `T-16` رسائل التقارير | **42** استدعاء `_notification` في `Screens/Reports` | 🔴 مفتوح |
| `T-17` تنسيق `frmPurchasesReport` | 3 مواضع `CellFormatting` | 🔴 مفتوح |
| `T-19` `frmTrialBalanceReport` | **0** `GridFormatHelper` | 🔴 مفتوح |
| `T-20` تحقق `من <= إلى` | **0** في 31 تقريراً | 🔴 مفتوح |
| `T-21` أنواع النسب | **0** `Percent`/`Ratio` في التقارير الثلاثة | 🔴 مفتوح |
| `T-27` لوحة التحكم | **6** `:N2` | 🔴 مفتوح |
| `T-06` سياق الحركة في المحاكاة | كل مستهلكي `Apply` ما زالوا بلا نوع/مخزن/مستند (6 مواضع) | 🔴 مفتوح |
| `T-08` مصدرا الحركات | `MockReportDesktopService` ما زال يشتق حركاته | 🔴 مفتوح |
| `T-09` `frmInventory` | 6 مراجع لمجموعات مصنوعة | 🔴 مفتوح |
| `T-37` مجلدات فارغة | `Screens/Payments/NewFolder` + 16 مجلد `Controls` فارغ (هياكل) | 🔄 `NewFolder` فقط يُحذف |

---

### 10.6 القياس الحالي — 2026-11-15 بعد موجة الإغلاق (المرجع الملزم)

> **البناء:** `dotnet build .\SalesSystem.slnx` ⇒ **0 أخطاء / 125 تحذيراً**. كل رقم أدناه **مقيس بأمر على الشجرة الحالية**، لا مقدَّر ولا منقول.

#### 10.6.1 أُغلق بالقياس (كان مفتوحاً في §10.1/§10.5)

| # | البند | القياس الآن | الدليل |
|---|---|---|---|
| T-01 | استيراد الأصناف | `CategoryId: 1` **= 0** (`CategoryId: category.Id`) · الأسعار/التكلفة/الباركود تُرسل عبر `AddProductUnitAsync` · الرصيد الافتتاحي **يُرفض بصدق** ويُعطَّل الاستيراد | `frmImportProductsExcel.cs` |
| T-02 | تكلفة مرتجع الشراء | `l.UnitCost` في الطلب **= 0** | `frmPurchaseReturn.cs` |
| T-06 | سياق الحركة | الستة يمرّرون النوع/المخزن/المستند/التاريخ | `MockStockLedger.Apply` |
| T-08 | مصدرا الحركات | التقرير يقرأ `MockInventoryData.Movements` | `MockReportDesktopService` |
| T-09 | `frmInventory` | المجموعات المصنوعة **= 0**؛ تقرأ عقود الرصيد/الجرد/التحويل | `frmInventory.cs` |
| T-10 | آلة حالة التسوية | «ترحيل/إلغاء» مشروطان بحالة السطر | `frmListInventoryAdjustments` |
| T-14 | الإقفال اليومي | لافتة صدق دائمة «يعرض ملخص الصناديق الإجمالي حتى يُعتمد عقد الإقفال اليومي» | `frmDailyClosureReport` |
| T-16 | رسائل التقارير | `_notification` في `Screens/**` **= 0** (كان 42) | — |
| T-17 | `frmPurchasesReport` | `CellFormatting` **= 0** فيها | — |
| T-19 | `frmTrialBalanceReport` | `GridFormatHelper` **= 1** | — |
| T-21 | أنواع النسب | `GridFormatKind.Ratio` **= 3** (الثلاثة كلها بلا ضرب ×100) | — |
| T-24 | ترتيب `DocNo` | عمود تسلسل رقمي يحكم الرصيد الجاري | `frmCashBoxTransactions` |
| T-25 | لون رصيد الكشف | من قيمة الصف الرقمية لا من النص المنسّق | `frmCustomerStatement` |
| T-26 | أعداد الملخصات | `clsFormat.Number` في الأربعة | — |
| T-27 | لوحة التحكم | `:N2` في `Screens/**` **= 0** | — |
| T-30 | رسالة الوحدة | تطابق `DeleteStrategy` | `frmProductUnitMaster` |
| T-31 | عدّاد المنتقي | `clsFormat.Number` | `frmProductSelection` |
| T-32 | معالج فارغ | حُذف | `frmProductMaster` |
| T-33 | ألوان الشاشات | **= 0** لون دلالي حرفي (كان 50 في 21 ملفاً). الوحيد الباقي `XLColor.LightGray` وهو **لون Excel** لا ثيم | — |
| T-35 | افتراض تصفية `frmListPayments` | افتراض واحد | — |
| T-37 | `NewFolder` | حُذف | — |
| RULE-N179 | `Cells["<حقل منطقي>"]` | **= 0** في كل `Screens/**` | — |
| **T-47** | `_ToNullIfEmpty` المكرّرة | **= 0** (كانت 18) — استُهلك `StringExtensions.ToNullIfEmpty` والتقليم محفوظ صراحةً (`x?.Trim().ToNullIfEmpty()`) | `Helpers` + 4 شاشات |
| **T-48** | `_Clamp(NumericUpDown, decimal)` | **= 0** (كانت 5 نسخ) — بيت جديد `Helpers\NumericInputHelper.cs` (`Clamp` + حمولة `decimal?`) | 5 ماسترات |
| **T-49** | نصوص المستند المكرّرة | **= 0** (`private const CashCustomerName` و`private _GetStatusText` = 0) — بيت جديد `Helpers\DocumentTextHelper.cs` (`CashCustomerName` · `CashCustomerNameOr` · `StatusText`) | 3 شاشات قوائم |
| **T-50** | آخر `CellFormatting` في التقارير | **= 0** — `frmProductMovementReport` يستخدم `GridFormatHelper.ApplyColumns/ApplyColumn` | — |
| **T-51** | نطاق زمني معكوس | `IsDateRangeValid` **= 19** استدعاءً (كانت 6) — 13 تقريراً جديداً يرفض النطاق المقلوب قبل أي استعلام برسالة واحدة من `clsValidation` | 13 تقريراً |
| **T-52** | حرفيات `ThemeHelper`/`ExportHelper` | `ThemeHelper` **4 TODO** موثَّقة (لا مفتاح مطابق — قرار مالك) · `ExportHelper` يقرأ الحيّ أولاً والثابت احتياطاً موثَّقاً | بيتان |
| البيوت | `MessageBox` خارج `clsDialogs` **= 0** · `ex.Message` للمستخدم **= 0** · `ExportHelper` في 32/32 تقريراً · `GreenThemeHelper.Apply(this)` في كل نموذج غير تقريري **= 0 ناقص** · تسجيل DI **94/94** بأسماء مؤهَّلة | — | — |

#### 10.6.2 ما زال مفتوحاً — المرحلة A (قابل للتنفيذ الآن)

| # | المهمة | الموضع (مقيس) | العيب | المالك | الحجم |
|---|---|---|---|---|---|
| **T-52** | `ThemeHelper` يحمل **4** ألوان حرفية موسومة `TODO(ThemePalette key)` — لا مفتاح مطابق في `ThemePalette` (التحقق: `SidebarDark (33,43,54)` · `TextDark (50,50,50)` · `ButtonForeOnSolid White` · `GhostHoverBack (240,240,240)` — كلها بلا مفتاح دوري مطابق) | `Helpers\ThemeHelper.cs:29,46,60,134` | **يحتاج قرار مالك**: إضافة مفتاح `textOnSolid` + `sidebarDark` + `hoverBack` إلى `ThemePalette` (`RULE-N54a`) — لا يُخترع مفتاح بلا أمر | `ui-agent` بعد قرار | S |
| ~~**T-55**~~ | ✅ **مغلق** — `DocumentTextHelper.StatusOptions` أُضيف واستُهلك | — | العضو الناقص صار موجوداً، و`T-57` بُني عليه | ✅ | — |
| **T-56** | `frmInventory` يقارن `StatusName` **نصاً** (`RowFilterHelper.EqualsText`) وعقد الخدمة يعيد «مرحّل» بالشدّة بينما نص البيت «مرحل» ⇒ توحيده يغيّر نتيجة التصفية (`frmInventory.cs:1102` `e.Value.ToString() switch`) | `frmInventory.cs:62,64,636,641,1102` | **مجمَّد**: الإصلاح الصحيح = عمود `Status` رقمي أو `StatusName` من البيت في العقد (`RULE-N177`) | `backend-architect` بعد موافقة | S |
| ~~**T-57**~~ | ✅ **مغلق (الموجة 11)** — 7 شاشات تُبنى قوائم حالتها من `StatusOptions` بعد `InitializeComponent()`، والقائمة الثابتة أُزيلت من المصمّمات | §11.12 | كان `RULE-N169` | ✅ | — |
| **T-54** | `_ClampToPickerRange(DateTime)` — منطق حصر تاريخ داخل مدى `DateTimePicker` (نسخة واحدة، غير مكرّرة) | `frmJournalEntries.cs:964` | لا مخالفة الآن؛ يُرقّى إلى بيت تواريخ إن تكرّر في شاشة ثانية | يُراقَب | — |

> **ملاحظة قياس:** `_ToNullIfEmpty` **= 0** · `_Clamp` **= 0** (المتبقي `_ClampToPickerRange` وهو دالة أخرى) · `private const CashCustomerName` **= 0** · `private _GetStatusText` **= 0** · `CellFormatting` في التقارير **= 0** · `IsDateRangeValid` **= 21** استدعاءً · `NumericInputHelper` **13** مرجعاً · `DocumentTextHelper` **48** مرجعاً · `clsValidation.EndOfDay` **21** استدعاءً. **(كل هذه الأرقام تجاوزها §10.6.5 بعد الموجات 9–12.)**
>
> **T-53 (مغلق):** كل **عرض/تلوين** للحالة صار من البيت أو من قيمة `Status` الرقمية (`frmListSales:749-750` · `frmListPurchases:758-759` · `frmJournalEntries:1596` · `frmAddUpdateInventoryAdjustment:44,257,626` · `frmPurchaseInvoice:372` · `frmSalesReturn:329` · `frmPurchaseReturn:338`) — **صفر مقارنة بنص عربي** في تلوين الحالة. أُغلق الباقي بـ`T-55`/`T-57` (8 مواضع قوائم خيارات)؛ يبقى موضع واحد في `frmAddUpdateSalesInvoice` (ملصق مركّب لا نص حالة — لا يُغيَّر) و4 مواضع في `frmInventory` مجمَّدة بالعقد (`T-56`).

#### 10.6.3 ما زال مفتوحاً — المرحلة B (مجمَّد، `RULE-N177`)

كما في §10.2: `T-38`…`T-46` بلا تغيير — كلها تحتاج عضواً في عقد أو باك-إند، وتنتظر موافقة الوحدة **المكتوبة**.

#### 10.6.4 قرارات مالك معلَّقة (لا تُنفَّذ بلا نص)

`D-01` تصدير Excel (36 شاشة) · `D-02` شبكات `AllowUserToAddRows = true` (**3** في الكود الآن) · `D-03` 7 نماذج شراء ميتة · `D-04` 4 نوافذ طباعة مخزون · `D-05` تقرير الربح حسب العميل · `D-06` `MockAuthService` بلا مستهلك · `D-07` موافقة أي وحدة في §10.2.


#### 10.6.5 القياس النهائي — بعد موجة الإصلاح الكاملة (2026-11-15، المرجع الحالي)

> **البناء:** `dotnet build .\SalesSystem.slnx` ⇒ **0 أخطاء / 125 تحذيراً** (التحذيرات ليست بوابة).
> **كل رقم مقيس بأمر على الشجرة الحالية** — لا مقدَّر ولا منقول. الأمر المستخدم موثَّق في §10.6.

| المقياس | القيمة الآن | كان | الحكم |
|---|---|---|---|
| `clsValidation.EndOfDay` (بيت «نهاية اليوم») | **1** في البيت + **21** استدعاءً في الشاشات | تعبير مكرّر في 20 ملفاً بصيغتين | ✅ `H-07` |
| `AddDays(1).AddSeconds(-1)` (يُسقط آخر ثانية) | **0** | 11 موضعاً | ✅ |
| `AddDays(1).AddTicks(-1)` مكتوب في شاشة | **0** تنفيذي (5 تعليقات توضيحية فقط) | 8 | ✅ |
| `clsValidation.IsDateRangeValid` | **21** استدعاءً | 19 | ✅ `T-20` مقيَّد بصدق (الشاشات التي لا تملك مدىً لا تستهلكه) |
| `_notification` / `INotificationService` في `Screens/**` | **0** | 42 | ✅ `T-16` |
| `MessageBox` خارج `clsDialogs` | **0** | 0 | ✅ |
| `ex.Message` للمستخدم | **0** (تطابق واحد في تعليق توثيقي) | 0 | ✅ |
| `Cells["<حقل منطقي>"]` | **0** (20 تطابقاً كلها أسماء أعمدة مصمّم أو تعليقات) | — | ✅ `RULE-N179` |
| `GridRowHelper` | **46** مرجعاً | 8 | ✅ |
| `GridFormatHelper` | **83** مرجعاً | 0 | ✅ `H-05` |
| `CellFormatting` **مكتوب يدوياً في التقارير** | **0** (المتبقي تعليق واحد) | 3 | ✅ `T-17` |
| `CellFormatting` في الشاشات غير التقريرية | **19** معالجاً — **كلها تلوين حالة/شارة** لا تنسيق مال/تاريخ | 21 | ⚠️ ليست بيتاً مكرّراً: `GridFormatHelper` يغطّي التنسيق وتلوين الرصيد، لا شارة الحالة |
| `":N2"` في **وقت التشغيل** داخل شاشة | **0** (المتبقي 14 في `*.Designer.cs` = تصريح تصميم مقبول `RULE-N134`) | 15 | ✅ |
| `DocumentTextHelper` | **48** مرجعاً · `StatusOptions` في **7** شاشات قوائم | 0 | ✅ `T-49`/`T-55`/`T-57` |
| `NumericInputHelper` | **13** مرجعاً | 0 | ✅ `T-48` |
| `MessageBox` / ألوان دلالية حرفية / صيغة يدوية في التقارير الـ31 | **0** | — | ✅ |
| `_ToNullIfEmpty` / `_Clamp` / `_GetStatusText` / `private const CashCustomerName` | **0 · 0 · 0 · 0** | 18 · 5 · 2 · 3 | ✅ |

---

## 11. موجة الإصلاح ملف‑بملف (2026-11-15) — حكم لكل ملف

> **القاعدة:** كل ملف يُقرأ ويُصلح **مرة واحدة**. `✅ منتهٍ` = لا مشكلة قائمة في هذا الملف من فئات §11.0. `⚠️` = بند مفتوح مذكور بجانبه، وسببه إما باك-إند مجمَّد (`RULE-N177`) أو قرار مالك.
>
> **الترتيب:** الموجات بترتيب التنفيذ الفعلي 1 → 12. **البناء بعد كل موجة:** `dotnet build .\SalesSystem.slnx` ⇒ **0 أخطاء**.
>
> **حدود كل وكيل:** لا `Business/**` · لا `DataAccess/**` · لا `Models/**` · لا `DTO/**` · لا `Services/Direct/**` · لا `Program.cs` في أي موجة (قفل الباك-إند `RULE-N177`) — إلا موجة الـMock التي تعمل داخل `Services/Mock/**` وحدها.

### 11.0 فئات العيب المعتمدة في هذه الموجة

| الفئة | التعريف | مثال مقيس |
|---|---|---|
| **1 · مال/بيانات** | قيمة تُعرض ولا تُرسل · حقل يُسقط صامتاً · افتراضي يمحو إدخال المستخدم · `decimal` كنص · صيغة مكتوبة بخط اليد | `frmProductMaster:652` فشل إنشاء الوحدة يُعرض بعده «تمت إضافة الصنف بنجاح» |
| **2 · نجاح كاذب** | رسالة نجاح بلا نداء خدمة أو مع `Result` فاشل · `Result` مُهمَل · معالج فارغ · صفوف مصنوعة | `frmOpeningStock:484` ترحيل جزئي يُعلَن «نجاحاً» |
| **3 · انهيار/كسر** | `Cells["<حقل منطقي>"]` بدل `GridRowHelper` · `async void` بلا `try/catch` · `Cursor` لا يُستعاد · `Convert.ToDecimal(DBNull)` · `(int)` على `short` | `frmOpeningStock:196` إسقاط الشاشة |
| **4 · بيت مكرّر** | لون حرفي · `MessageBox` خارج `clsDialogs` · `ex.Message` للمستخدم · نسخة خاصة من سلوك بيت · نص حالة حرفي · صيغة `N2`/`yyyy-MM-dd` يدوية | `_Clamp` ×5 · `_GetStatusText` ×2 · `CellFormatting` التقارير |
| **5 · آلة حالة** | تعطيل/ترحيل/إلغاء مفعّل على سطر غير مطابق · تحرير مستند مُرحَّل · حفظ ثانٍ يُنشئ مستنداً مكرراً · `Enter` داخل شبكة السطور يُرحّل | `frmProductMaster:875` «تعطيل» على سجل معطّل |

### 11.1 البيوت الجديدة في هذه الموجة

| # | البيت | السطح العام | المستهلكون | القياس |
|---|---|---|---|---|
| H-03 | `Helpers\NumericInputHelper.cs` | `Clamp(NumericUpDown, decimal)` · `Clamp(NumericUpDown, decimal?, decimal = 0m)` | 5 ماسترات | نسخ `_Clamp` الخاصة = **0** · 13 مرجعاً |
| H-04 | `Helpers\DocumentTextHelper.cs` | `CashCustomerName` · `CashCustomerNameOr(string?)` · `StatusText(byte)` · `StatusOptions` | 3 شاشات قوائم + القيود + 5 شاشات مستندات + 7 شاشات تصفية | `private const CashCustomerName` = 0 · 48 مرجعاً |
| H-05 | `Helpers\GridFormatHelper` | `ApplyColumns` · `ApplyColumn` · `ApplyBalanceColouring` (+ أنواع `GridFormatKind`) | **كل** التقارير الـ31 + شاشات المستندات | 83 مرجعاً · `CellFormatting` يدوي في التقارير = 0 |
| H-06 | `Global Classes\clsValidation` | `IsDateRangeValid` · `DateRangeErrorMessage` · **`EndOfDay(DateTime)`** | 21 استدعاءً | حراسات نطاق محلية = 0 |
| H-07 | `Global Classes\clsValidation.EndOfDay` | آخر لحظة في اليوم (حدّ أعلى شامل) | **21** استدعاءً في 20 ملفاً | تعبير «نهاية اليوم» مكتوب يدوياً في شاشة = **0** |

### 11.2 الموجة 1 — بيت الرسائل + قراءة الخلية

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Purchases\Forms\frmPurchaseInvoice.cs` | ✅ منتهٍ | `_notification` (Toast) → `clsDialogs` في 11 موضعاً — بيت رسائل واحد |
| `Screens\Returns\Forms\frmPurchaseReturn.cs` | ✅ منتهٍ | `_notification` → `clsDialogs`، وحُذفت رسالة خطأ مكرّرة قبل تحذير ترحيل تفصيلي |
| `Screens\Returns\Forms\frmSalesReturn.cs` | ✅ منتهٍ | مثله (11 ← 0 استدعاء Toast) |
| `Screens\Payments\Forms\frmSupplierPaymentDialog.cs` | ✅ منتهٍ | `Notification.Show*` → `clsDialogs` (5 مواضع) |
| `Screens\Products\Forms\frmImportProductsExcel.cs` | ✅ منتهٍ | `row.Cells["Status"]` → `GridRowHelper.GetText(row,"Status")` (`RULE-N179`) |

### 11.3 الموجة 2 — إزالة التكرار (`RULE-N171`/`N172`)

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Accounting\Forms\frmAccounts.cs` | ✅ منتهٍ | حُذفت `_ToNullIfEmpty` المحلية (كانت تُقلّم) → `x?.Trim().ToNullIfEmpty()` |
| `Screens\Accounting\Forms\frmJournalEntries.cs` | ✅ منتهٍ | مثلها + `_StatusText` المحلية → `DocumentTextHelper.StatusText` |
| `Screens\Customers\Forms\frmCustomerMaster.cs` | ✅ منتهٍ | حُذفت `_ToNullIfEmpty` + `_Clamp` → البيتان |
| `Screens\Suppliers\Forms\frmSupplierMaster.cs` | ✅ منتهٍ | مثلها |
| `Screens\Products\Forms\frmProductMaster.cs` | ✅ منتهٍ | `_Clamp` ×5 → `NumericInputHelper.Clamp` |
| `Screens\Products\Forms\frmProductUnitMaster.cs` | ✅ منتهٍ | مثلها |
| `Screens\Employees\Forms\frmEmployeeMaster.cs` | ✅ منتهٍ | مثلها |
| `Screens\Sales\Forms\frmListSales.cs` | ✅ منتهٍ | `CashCustomerName` + `_GetStatusText` → `DocumentTextHelper`، وتلوين الحالة صار بقيمة `Status` الرقمية، وقائمة الحالة من `StatusOptions` |
| `Screens\Returns\Forms\frmListReturns.cs` | ✅ منتهٍ | `CashCustomerName` + `_CustomerText` → `CashCustomerNameOr` |
| `Screens\Purchases\Forms\frmListPurchases.cs` | ✅ منتهٍ | `_GetStatusText` → `StatusText`، وتلوين الحالة بالرقم |

### 11.4 الموجة 3 — التقارير والبيوت

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Reports\Forms\Inventory\frmProductMovementReport.cs` | ✅ منتهٍ | آخر `CellFormatting` يدوي أُزيل → `GridFormatHelper.ApplyColumns/ApplyColumn` |
| 13 تقريراً: `frmAccountStatementReport` · `frmCashFlowReport` · `frmGeneralLedgerReport` · `frmIncomeStatementReport` · `frmDetailedStockLedgerReport` · `frmWarehouseMovementReport` · `frmPurchasesByProductReport` · `frmPurchasesBySupplierReport` · `frmDailySalesReport` · `frmProfitByCustomerReport` · `frmReturnsReport` · `frmSalesByCategoryReport` · `frmSalesByCustomerReport` | ✅ منتهٍ | حراسة النطاق المقلوب من البيت قبل أي استعلام (`IsDateRangeValid` 6 → 19) |
| `Helpers\ThemeHelper.cs` | ⚠️ قرار مالك `T-52` | بقيت **4** ألوان حرفية موسومة `TODO(ThemePalette key)` لأن `ThemePalette` لا يملك مفتاحاً مطابقاً |
| `Helpers\ExportHelper.cs` | ✅ منتهٍ | يقرأ `clsFormat` الحيّ أولاً، والثابت احتياطيّ موثَّق بالعربية |

### 11.5 الموجة 4 — نصوص المستند

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Helpers\DocumentTextHelper.cs` | ✅ منتهٍ | أُضيف `StatusOptions` (زوج قيمة↔نص) فوق `StatusText` — صفر نص مكرّر |
| `Screens\Accounting\Forms\frmJournalEntries.cs` | ✅ منتهٍ | قائمة الحالة من `StatusOptions` (توحّد النص مع الترويسة) |
| `Screens\Inventory\Forms\frmAddUpdateInventoryAdjustment.cs` | ✅ منتهٍ | 3 نصوص «مسودة» → `StatusText(1)` |
| `Screens\Purchases\Forms\frmPurchaseInvoice.cs` · `Screens\Returns\Forms\frmSalesReturn.cs` · `frmPurchaseReturn.cs` | ✅ منتهٍ | مفتاح الحالة → `StatusText(_invoice.Status)` بدل 4 فروع نصية |
| `Screens\Inventory\Forms\frmInventory.cs` | ⚠️ مجمَّد `T-56` | يقارن `StatusName` نص العقد («مرحّل» ≠ «مرحل») — يحتاج عمود `Status` رقمي أو نصاً من البيت في العقد |
| `Screens\Sales\Forms\frmAddUpdateSalesInvoice.cs` | ⚠️ بند C | ملصق مركّب («تم الترحيل — مقفلة») لا نص حالة مفرد — لا يُغيَّر |

### 11.6 الموجة 5 — دليل الأصناف والمخزون (12 ملفاً)

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Products\Forms\frmProductMaster.cs` | ✅ منتهٍ | «تعطيل» مشروط بالسجل النشط (`RULE-N92`) · فشل إنشاء الوحدة الرئيسية وسطر السعر يُعلَن بتحذير عربي بدل «تمت الإضافة بنجاح» |
| `Screens\Products\Forms\frmProductUnitMaster.cs` | ✅ منتهٍ | «حذف» معطّل على الوحدة الأساسية (كان مفعّلاً ثم يرفض) |
| `Screens\Products\Forms\frmProductSelection.cs` | ✅ منتهٍ | `clsFormat.Quantity` بدل `"G"` · F5 يحدّث شبكة الوحدات · `SelectedProduct` من DTO الحقيقي لا مُختلق |
| `Screens\Products\Forms\frmImportProductsExcel.cs` | ✅ منتهٍ | ورقة فارغة → رسالة صادقة بدل `NullReference` يُفسَّر «الملف مفتوح» |
| `Screens\Inventory\Forms\frmInventory.cs` | ⚠️ `T-56` فقط | عمود «الكود» يُملأ بالباركود (كان فراغاً فتصفية الكود بلا نتائج) · قاموسا حالة مدمجان · **الموجة 12:** صيغ الأعمدة من `clsFormat` |
| `Screens\Inventory\Forms\frmInventoryList.cs` | ✅ منتهٍ | `(decimal)e.Value` غير محروس → `is not decimal` |
| `Screens\Inventory\Forms\frmInventoryBarcode.cs` | ✅ منتهٍ | كان يطبع باركود الصنف بدل الباركود الممسوح |
| `Screens\Inventory\Forms\frmInventoryFifo.cs` | ✅ منتهٍ | `"yyyy-MM-dd"` → `clsFormat.DateToShort` |
| `Screens\Inventory\Forms\frmListInventoryAdjustments.cs` | ✅ منتهٍ | `CreatedAt` حقيقي + مفتاح تصفية · فشل التصفية لا يُسقطها إلى «الكل» · قائمة السياق تخضع لآلة الحالة · `"yyyy-MM-dd HH:mm"` → `clsFormat.DateToLong` (الموجة 12) |
| `Screens\Inventory\Forms\frmListInventoryBatches.cs` | ✅ منتهٍ | سقف 50 صنفاً صار مُعلناً · شرطان متناقضان (منتهية/قاربت) · نص «منتهي» الحرفي أُزيل |
| `Screens\Inventory\Forms\frmListInventoryTransactions.cs` | ✅ منتهٍ | `RowFilter` محروس · سقف 500 حركة مُعلن |
| `Screens\Inventory\Forms\frmOpeningStock.cs` | ⚠️ بند `B` | `DBNull` لا يُسقط الشاشة · سطر بلا وحدة لا يُضاف · الترحيل الجزئي يُعلَن ولا يُرحَّل. **تاريخ المستند لا يُرسل** — طلب الإنشاء بلا حقل تاريخ (DTO مجمَّد) |

### 11.7 الموجة 6 — بقية المخزون + فاتورة/قائمة المشتريات

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Inventory\Forms\frmStocktakingNew.cs` | ✅ منتهٍ | عمودا الصنف/الباركود للقراءة فقط · كمية غير رقمية تُعلَن بتلميح بدل مسح صامت · `Color.DimGray` → `StatusMuted` · رسالة الاعتماد «X من أصل Y» · منع حذف سطر بعد الحفظ · `clsFormat.TryParseQuantity` |
| `Screens\Inventory\Forms\frmWarehouseTransferNew.cs` | ✅ منتهٍ | تسريب GDI (`new Font` لكل خلية) → خط واحد · كمية غير رقمية تُعلَن · منع حذف سطر بعد الحفظ |
| `Screens\Inventory\Forms\frmAddUpdateInventoryAdjustment.cs` | ✅ منتهٍ | افتراض `(short)0`/`(byte)1` مُختلق → رفض عربي · «تم حفظ المسودة» لا تُعلَن إن فشلت إعادة التحميل · إلغاء المسودة اليتيمة لا يُفرّغ المعرّف إلا بنجاح (منع مستند مكرر) · فشل تحميل المستودعات يُعلَن · الإغلاق يسأل عند بنود غير محفوظة |
| `Screens\Inventory\Forms\frmStocktakingPrint.cs` · `frmStocktakingPrintAdjustment.cs` · `frmStocktakingPrintCard.cs` · `frmWarehouseTransferPrint.cs` | ⚠️ `D-04` فقط | «تمت الطباعة بنجاح» بلا طابعة → معلومات صادقة «معاينة… لم تُرسَل أي نسخة». البيانات/الأرقام المصنوعة وغياب المستدعي قرار مالك مؤجل |
| `Screens\Purchases\Forms\frmPurchaseInvoice.cs` | ✅ منتهٍ | انهيار `InvalidCastException` عند كل حفظ (`short` `SelectedValue`) · المستودع المحفوظ يُفتح مختاراً · `F5` لا يُفقد المستودع · تحميل المستند الكامل قبل التحرير · `ProductUnitId` الحقيقي · التكلفة تُرسل سعراً · `EndEdit` قبل القراءة · رفض السطر غير المكتمل · رفض مدفوع > إجمالي وخصم > أصناف · `RULE-N153` · `GridFormatHelper` · تسريب GDI أُزيل |
| `Screens\Purchases\Forms\frmListPurchases.cs` | ✅ منتهٍ | «إلى» كان يوماً زائداً · تصفية المبلغ تقبل الفاصل العشري · تسريب GDI في `CellFormatting` أُزيل |

### 11.8 الموجة 7 — المبيعات والمرتجعات

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Sales\Forms\frmAddUpdateSalesInvoice.cs` | ✅ منتهٍ | نوع الدفع لا يُستعاد عند فتح مسودة آجلة ⇒ **آجلة تُحفظ نقدية** — أُصلح · «فاتورة جديدة» لا تورّث «آجل» · خصم السطر يُقيَّد إلى (الكمية×السعر) في 3 مسارات · `_ValidateForm`/`Confirm` داخل `try/catch` · باركود غير موجود يُعلَن · `txtNotes.Validating` لا يفرض حقلاً اختيارياً · نص الحالة من `DocumentTextHelper` · 3 معالجات ميتة أُزيلت |
| `Screens\Sales\Forms\frmListSales.cs` | ✅ منتهٍ | إعادة التحميل كانت **قبل** فتح المحرر ⇒ فاتورة جديدة لا تظهر حتى `F5` — صارت عند إغلاق المحرر · تسريب GDI أُزيل |
| `Screens\Returns\Forms\frmSalesReturn.cs` | ✅ منتهٍ | انهيار `(int)cmbWarehouse.SelectedValue` (`short`) · `F5` يمسح المستودع · **فتح مسودة ثم حفظ كان يُنشئ مرتجعاً ثانياً** · سطر بلا صنف يُسقط من الطلب ويبقى في المجموع · `_lines[e.RowIndex]` في صف الإدخال الجديد · `finally` يُفعّل زراً مخفياً · «عميل نقدي» (Id 0) كان يمنع ترحيل مرتجع نقدي |
| `Screens\Returns\Forms\frmPurchaseReturn.cs` | ✅ منتهٍ | نفس الثمانية بلا استثناء |
| `Screens\Returns\Forms\frmListReturns.cs` | ✅ منتهٍ | `"0.##"` يدوي → `clsFormat.Money` · «تم الترحيل» من مسار ثانٍ → `StatusText` · القائمة لا تتحدّث بعد الحفظ |

### 11.9 الموجة 8 — السندات والمصروفات ودليل البيانات والكشوف والمنتقيات والمحاسبة

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\CustomerReceipts\Forms\frmAddUpdateCustomerReceipt.cs` | ⚠️ بند `B` | حقول التعديل غير الموجودة في `UpdateCustomerReceiptRequest` (العميل/التاريخ) **تُقفل في التعديل** بدل أن تُهمَل بصمت · طريقة الدفع المحمَّلة تُعاد إرسالها بدل `PaymentMethod = 1` (كان يحوّل الآجل نقدياً) · حقول غير قابلة للتحرير بعد الترحيل |
| `Screens\CustomerReceipts\Forms\frmListCustomerReceipts.cs` | ✅ منتهٍ | «تعديل» يفتح السند المحدد · تسلسل التصفية الرقمي · حالة الرفض تُعلَن |
| `Screens\Payments\Forms\frmListPayments.cs` | ✅ منتهٍ | افتراض تصفية واحد (`Esc` لا يعيد «هذا الشهر» بينما الافتراضي 4 أشهر) · `ShowDialog` بمالك · تصفية رقمية |
| `Screens\Payments\Forms\frmListSupplierPayments.cs` | ⚠️ `T-39` | «تعديل» لم يبقَ يفتح سنداً فارغاً — صار معطّلاً مع لافتة صادقة حتى يوجد `UpdateAsync` في العقد (مجمَّد) |
| `Screens\Payments\Forms\frmSupplierPaymentDialog.cs` | ✅ منتهٍ | `Notification.Show*` → `clsDialogs` · `ShowDialog(this)` · سباق القوائم |
| `Screens\Expenses\Forms\frmAddUpdateExpense.cs` | ✅ منتهٍ | `ExpenseAccountId: 0` كان يمحو الحساب · سند مُرحَّل لا يُعدَّل · تاريخ واحد للتصفية |
| `Screens\Expenses\Forms\frmListExpenses.cs` | ✅ منتهٍ | عمود مكرّر أُزيل · التصفية تُطبَّق مرة واحدة · حالة رقمية |
| `Screens\Categories\Forms\frmCategoryMaster.cs` | ✅ منتهٍ | آلة حالة مركزية واحدة (`RULE-N150`) · «تعطيل» على سجل معطّل ممنوع · ملخص بـ`clsFormat` |
| `Screens\Units\Forms\frmUnitMaster.cs` | ✅ منتهٍ | «تعطيل» على سجل معطّل ممنوع (`RULE-N92`) · ملخص بـ`clsFormat` |
| `Screens\Warehouses\Forms\frmWarehouseMaster.cs` | ✅ منتهٍ | `_busy` في 4 معالجات (كانت تسمح بحفظين) · تعطيل معطّل ممنوع · قراءة شبكة بالاسم لا بالفهرس · ملخص |
| `Screens\CashBoxes\Forms\frmCashBoxMaster.cs` | ✅ منتهٍ | `_busy` · تعطيل معطّل ممنوع · ترتيب رقمي للصندوق |
| `Screens\CashBoxes\Forms\frmCashBoxTransactions.cs` | ✅ منتهٍ | `"N2"` → `clsFormat` · `RowFilter` محروس · عمود تسلسل رقمي يحكم الرصيد الجاري (`T-24`) |
| `Screens\Customers\Forms\frmCustomerMaster.cs` | ✅ منتهٍ | زر «إضافة» يبقى مفعّلاً أثناء الحفظ ⇒ تكرار سجل ممكن — أُصلح · حد الائتمان بنمط مال |
| `Screens\Suppliers\Forms\frmSupplierMaster.cs` | ⚠️ `DTO` مجمَّد | **فقد بيانات:** `SupplierDto.CategoryId` يُعاد `null` فيمحو التصنيف عند التعديل — الإصلاح في العقد لا في الشاشة |
| `Screens\Employees\Forms\frmEmployeeMaster.cs` | ✅ منتهٍ | `_busy` + `NumericInputHelper.Clamp` |
| `Screens\Users\Forms\frmUserMaster.cs` | ⚠️ `T-38`(ب) | **فقد بيانات:** `UserDto.DefaultCashBoxId` يُعاد `null` — أُصلح في الشاشة بحفظ القيمة · «إعادة تعيين كلمة المرور» ما زالت `TODO` (تحتاج عضواً في العقد) |
| `Screens\Customers\Forms\frmCustomerStatement.cs` | ⚠️ قرار مالك | 12 إصلاحاً (نطاق مقلوب مرفوض · «إلى» شامل بالتاريخ بلا حيلة تِكس · فشل التحميل لا يُبتلع · الأعمار تُقاس إلى تاريخ الكشف · `CellFormatting` عبر `GridRowHelper` · `lblNoData` صار مرئياً) · البند المفتوح: **هل الداخل النقدي هو `PaidAmount` أم السندات؟** (ازدواج محتمل — قرار مالك) |
| `Screens\Customers\Forms\frmCustomerSelection.cs` | ✅ منتهٍ | كان يُعيد `CustomerDto` **مُفبركاً من نص الشبكة** (حد الائتمان والرصيد يُصفَّران) → صار يُعيد DTO الخدمة الحقيقي |
| `Screens\Suppliers\Forms\frmSupplierSelection.cs` | ✅ منتهٍ | نفس العيب ونفس الإصلاح عبر `ISupplierDesktopService` |
| `Screens\Accounting\Forms\frmAccounts.cs` | ✅ منتهٍ | 11 إصلاحاً: الحساب النظامي للقراءة فقط (تعديل/حذف معطّلان ومُعلَنان) · `RULE-N147` (Enter يتبع الوضع) · `CategoryId` لا يُمحى · الحساب الأب لا يُنقل للجذر · رصيد افتتاحي على حساب غير ورقة مرفوض · `GridFormatHelper` للتاريخ والمال |
| `Screens\Accounting\Forms\frmJournalEntries.cs` | ✅ منتهٍ | 10 إصلاحات: «إلى» كان يوماً زائداً · «مسح التصفية» يُعيد الاستعلام · `ReferenceType/ReferenceId` لا يُصفَّران · الترحيل ممنوع مع تعديلات غير محفوظة · `Delete` داخل الشبكة يحذف السطر لا القيد · تلوين/قائمة الحالة من البيت · `EndOfDay` (الموجة 12) |

### 11.10 الموجة 9 — الإعدادات والمستخدمون ولوحة التحكم والهيكل والدخول والحاسبة ونقطة البيع

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Settings\Forms\frmChangeSystemColorsSettings.cs` (+ مصمّم) | ⚠️ `T-52` فقط | إغلاق يهمل مسودّة → سؤال تأكيد · `Ctrl+V` يبتلع حقل HEX → يُلصق ويُطبَّق · قائمة السياق تعمل على السطر المنقور · «لا توجد بيانات» صار مرئياً (`lblNoData` فوق الشبكة) · `Enter`/`AcceptButton` يتبع الشبكة (`RULE-N153`) · قراءة القيمة المخزّنة بـ`TryParseColorText` (لا إسقاط صامت) · `_HasUnsavedChanges()` واحدة |
| `Screens\Settings\Forms\frmFormatingSetting.cs` (+ مصمّم) | ✅ منتهٍ | «حفظ وتطبيق» لا يُفعَّل بلا تغيير · لا نداء خدمة بلا تغيير · `AcceptButton` يتبع الحالة · إغلاق/Esc/F5 يسأل عن المسودّة · F5 محروس بـ`_isBusy` · المحرّرات تُقفل أثناء الحفظ · مسار حالة واحد `_ApplyState()` |
| `Screens\Users\Forms\frmPasswordChange.cs` | ⚠️ تكافؤ Mock | حارس هوية في الوضعين · الوضع الإداري لم يبقَ مسدوداً · رفض الخدمة لم يبقَ يُفرّغ الحقلين ويوجَّه للحقل الخطأ. **البند:** `AuthService` الحقيقي يطلب الكلمة الحالية دائماً بينما `MockAuthDesktopService` يمنح إعفاءً إدارياً — فرق `RULE-N167` |
| `Screens\Users\Forms\frmBackup.cs` | ✅ منتهٍ | 3 نصوص صادقة بدل «لا توجد نسخ احتياطية بعد» عند فشل التحميل · `_SetBusy` في مساري التحديث وما بعد الإنشاء (كانا يسمحان بعمليتين متزامنتين) · `BackupFileNameHelper` وحده للتسمية/التاريخ |
| `Screens\Dashboard\Controls\ctrlDashboard.cs` | ✅ منتهٍ | 9 إصلاحات: اشتراكات `EventBus` بمراجع قوية (كانت `WeakReference` تُجمع فيتوقف التحديث الحيّ) · حارس تحميل متوازٍ · `InvokeRequired`/`BeginInvoke` · تجميع الأحداث بنافذة بدل إسقاطها · `ClearCards()` عند الفشل (لا أرقام قديمة) · إلغاء عند `Dispose` · «ملخص الأرقام» يحترم إخفاء البطاقات · صفر `:N2` |
| `Screens\Dashboard\Forms\frmDashboard.cs` (+ مصمّم) | ✅ منتهٍ | غلاف رقيق كما هو (`RULE-N66`) · لا يلمس المكوّن بعد الإغلاق · `ToolTip` على «إغلاق» |
| `Screens\frmLogin.cs` | ✅ منتهٍ | إغلاق `frmMain` كان يترك **عملية غير مرئية** → `FormClosed` يُنهي التطبيق · كلمة المرور تُفرَّغ بعد الدخول |
| `Screens\Tools\Forms\frmCalculator.cs` | ⚠️ قرار مالك | `decimal` لا `double` · القسمة على صفر برسالة عربية صريحة · تجاوز المدى يُعلَن بدل صمت · `Equals` لا يترك الضغط بلا أثر · حقل `_serviceProvider` الميت أُزيل. **البند:** لا يوجد منسّق `clsFormat` بدقة كاملة (إضافة عضو = قرار) ولا زر نسخ |
| `Screens\frmMain.cs` (+ مصمّم) | ✅ منتهٍ | «إدارة المخزون/التحويل/الجرد» كانت تُفتح في الوضع الحقيقي وتفشل (عقود Mock فقط) → تُعطَّل بتلميح عربي واحد `ApplyMenuState()` · `ShowItemToolTips` معلن في المصمّم · لون نص القوائم من اللوحة الحيّة · لاحقة «وضع التجربة» لا تتراكم · `Logout`/`ChangePassword` محروسان · **تدقيق 72 عنصر قائمة: كلها تشير إلى نموذج موجود ومسجَّل**، صفر مرجع لنماذج `D-03` |
| `Screens\POS\Forms\frmPOS.cs` (+ مصمّم) | ⚠️ بند `B` | 16 إصلاحاً: كمية ≤ 0 كانت تُحوَّل صامتاً إلى `0.001` · خصم السطر لا يُعاد تقييده بعد تخفيض الكمية ⇒ **انهيار `Math.Clamp`** · خصم فاتورة > المجموع يُرفض عربياً بدل تقييد صامت · المدفوع > الصافي يُفحص في النقدي والآجل · `TryReadDecimal` المحلية → `clsFormat.TryParse*` · كمية صفر في الشبكة تُرفض · سعر 0 يُرفض · تعديل بعد حفظ مسودة يستوجب تأكيداً صريحاً · `RULE-N153` (`SyncAcceptButton`) · حارس `_isBusy` · `Delete` لا يحذف سطراً والمؤشر على زر · ناشر `EventBus` محروس (منع فاتورة ثانية) · `GridFormatHelper` للأعمدة · لون زر التحديث «المصمّم = التشغيل». **البند:** لا فحص مخزون مسبق (يحتاج مصدراً حياً) |

### 11.11 الموجة 10 — التقارير الـ31 (تمريرة تحقق وإصلاح كاملة)

> **الإطار الموحّد (31/31):** `ReportUiStandardizer.ApplyForm` + `ReportNavigationHelper.Attach` كآخر سطرين · تصدير `ExportHelper` بحارس صفر صفوف · تنسيق عبر `GridFormatHelper` · صفر `_notification` · صفر `MessageBox` · صفر `CellFormatting` يدوي · كميات `N3` ومبالغ `N2` ونسب `Ratio`.

| المجموعة | الملفات | الحكم | ما نُفِّذ |
|---|---|---|---|
| **المبيعات (8)** | `frmDailySalesReport` · `frmSalesReport` · `frmSalesByCustomerReport` · `frmSalesByCategoryReport` · `frmSalesByProductReport` · `frmProductProfitabilityReport` · `frmProfitByCustomerReport` · `frmReturnsReport` | ✅ منتهٍ (1 بند قرار) | تاريخ خام بوقت → `DateDayMonthYear` (×2) · `frmSalesReport` كان يحجب عموداً باسم غير موجود فيبقى `Id` التقني ظاهراً · `Value!` بلا فحص · «عدد السجلات» يبقى 0 فوق شبكة ممتلئة · `frmProfitByCustomerReport`: متغيّر «ربح» ميت + تقرير بلا ربح → **لافتة عقد صادقة** (`lblContractNote` معلنة في المصمّم) وعمود «المستحق» · «إجمالي المرتجعات» كان بلا مبلغ |
| **المخزون (7)** | `frmInventoryReport` · `frmStockBalanceReport` · `frmLowStockReport` · `frmExpiredProductsReport` · `frmProductMovementReport` · `frmDetailedStockLedgerReport` · `frmWarehouseMovementReport` | ✅ منتهٍ | عناوين إنجليزية وأرقام خام → عربية + `Money`/`Quantity` · `ProductId`/`WarehouseId`/`ConversionFactor`/`ProductCode` محجوبة · **يوم زائد** في دفتر الأستاذ التفصيلي وحركات المستودع (`AddDays(1)`) · عدّادات `ToString` → `clsFormat.Number` · نوع الحركة إنجليزي → خريطة عربية واحدة · لونا `Color.White` ميتان أُزيلا. **بند:** 4 شاشات لا تملك منتقي تاريخ ⇒ لا تحتاج حارس نطاق (مقيس، لا مفترض) |
| **المحاسبة (8)** | `frmAccountStatementReport` · `frmGeneralLedgerReport` · `frmTrialBalanceReport` · `frmVatReport` · `frmBalanceSheetReport` · `frmCashFlowReport` · `frmIncomeStatementReport` · `frmWorkingCapitalReport` | ✅ منتهٍ (2 بند `B`) | كشف الحساب: قاعدة إشارة مكرّرة تُنتج **رقمين متعارضين** في شاشة واحدة → الملخص يقرأ رصيد آخر سطر · عدّاد ومجاميع من البيت · تواريخ بلا تنسيق في 4 تقارير · «متوازن» بلا طرف مقارنة معروض → يُعرض · «عدد السجلات» يبقى 0 · **`frmIncomeStatementReport`:** مجاميعه كانت تُرشَّح على نصوص تصنيف غير موجودة ⇒ **صفر دائماً** → أُسقط الحساب المحلي الكاذب وأُحيل إلى سطور الإجماليات (`T-43`) · **`frmWorkingCapitalReport`:** «متداول» بلا تصنيف في العقد → تسمية صادقة + تنبيه · معامل النسبة كان يُعرض مبلغاً → `clsFormat.Ratio`/`Ratio` |
| **المشتريات والعملاء/الموردون والصندوق (8)** | `frmPurchasesReport` · `frmPurchasesByProductReport` · `frmPurchasesBySupplierReport` · `frmAgingReport` · `frmCustomerBalanceReport` · `frmSupplierBalanceReport` · `frmCashBoxSummaryReport` · `frmDailyClosureReport` | ✅ منتهٍ | `frmCustomerBalanceReport`: **أرقام كاذبة** (إجمالي مبيعات/مدفوعات = 0.00 دائماً من خصائص توافق قديم) → أُخفيت الأعمدة الصفرية والملخص يجمع الرصيد الحقيقي · `cmbCustomer` كان بلا معالج (تصفية ميتة) → تصفية فورية · لون حرفي · `frmAgingReport` عدّاد خام · `frmSupplierBalanceReport` عنوانان إنجليزيان وبلا تنسيق · `frmCashBoxSummaryReport`: `async void` غير محروس + `null!` + عدّاد خام + crash عند الإغلاق أثناء التحميل · `frmDailyClosureReport`: **لافتة صدق دائمة** مثبَّتة في المُنشئ قبل آخر سطرين (كانت تُضيع في مسار الاستثناء) |
| **معالجات ميتة** | `frmWorkingCapitalReport` · `frmProfitByCustomerReport` · `frmWarehouseMovementReport` | ✅ منتهٍ | 3 معالجات فارغة موصولة في المصمّم أُزيلت (المعالج + الربطة) |

### 11.12 الموجة 11 — إغلاق `T-57` + تدقيق `D-03`

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `frmListPurchases` · `frmListReturns` · `frmListExpenses` · `frmCashBoxTransactions` · `frmListInventoryAdjustments` (مصمّم + `.cs`) | ✅ منتهٍ `T-57` | القائمة الثابتة أُزيلت من المصمّم، والتصفية تُبنى من `DocumentTextHelper.StatusOptions` بعد `InitializeComponent()` · **`frmListReturns`** كانت تعرض خيارين فقط → ثلاثة · `frmListPayments` و`frmListCustomerReceipts` كانتا تربطان الفهرس القديم بقيم 2/3 ⇒ «مسودة» تُظهر المرحّل — صُحِّح إلى القيمة الرقمية |
| 7 نماذج شراء (`frmPurchaseItem` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` · `frmPurchaseItemInfo` · `frmPurchaseInvoicePrint` · `frmSupplierStatement`) | ⚠️ `D-03` | **تدقيق مكتمل، لا تنفيذ:** كلها `ميت — مرجع واحد (Program.cs)` (الأسطر 390-395 و400)، صفر مرجع من `frmMain` أو أي شاشة · بيانات مصنوعة مقيسة (`INV-2026-001` · `MockSubtotal = 150000` · عملات بأسعار صرف حرفية) و«تمت طباعة فاتورة 5003 بنجاح» بلا طابعة. **التوصية:** سحبها من DI وحذف 14 ملفاً (لا `.resx`) — والاستثناء الوحيد الذي يستحق قراراً منفصلاً هو `frmSupplierStatement`. **لم يُحذف ملف واحد بلا قرار مالك** |

### 11.13 الموجة 12 — طبقة المحاكاة + بيت «نهاية اليوم» + بقايا

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Services\Mock\MockInventoryAdjustmentDesktopService.cs` | ✅ منتهٍ (Major المراجعة) | `CancelAsync` كان يعكس أثر إضافة مرحّلة **بلا حارس مخزون** ⇒ رصيد سالب في وضع التجربة · أُضيف `HasSufficientStock` + رفض عربي بنفس نص الجرد |
| `Services\Mock\MockStocktakingDesktopService.cs` | ✅ منتهٍ | إلغاء **المسودة** كان مرفوضاً بينما الشاشة تُلغيه وتُغلق ⇒ صار مسموحاً بلا حركة رصيد (مطابق `InventoryAdjustment.Cancel()` في النموذج الحقيقي) · الملغاة تُرفض «ملغي بالفعل» |
| `Services\Mock\MockReportDesktopService.cs` | ✅ منتهٍ | «المستحق» لم يكن يساوي مجموع شرائح الأعمار (يقارن كل `Due` مقابل `Due > 0` فقط) → صار مجموع الشرائح بعد التقريب · `thresholdDays < 0` كان يُقيَّد صامتاً → رفض عربي · نوع طرف غير معروف كان يُعيد تقرير موردين كاملاً → قائمة فارغة بنجاح · أرصدة الموردين من متبقي الفواتير المرحّلة |
| `Services\Mock\MockSalesReportDesktopService.cs` | ✅ منتهٍ | `NetAmount = NetTotal − Discount` والـ`NetTotal` صافٍ أصلاً ⇒ **الخصم يُطرح مرتين** في وضع التجربة → `TotalAmount` قبل الخصم و`NetAmount` = الصافي المحفوظ |
| `Services\Mock\MockData.cs` · `MockDashboardDesktopService.cs` | ✅ منتهٍ | أرصدة العملاء لم تكن تساوي متبقي فواتيرهم (عميل 4: 45,000 مقابل 12,500) ⇒ رقمان متناقضان في شاشتين → البذور وُوِمت مع المستندات (المستندات هي الحاكم، صفر اختلاق وصفر حذف) · `TotalReceivables/TotalPayables` من المرحّل فقط |
| `Global Classes\clsValidation.cs` | ✅ منتهٍ `H-07` | أُضيف `EndOfDay(DateTime)` — بيت واحد لتعبير كان مكرّراً في 20 ملفاً بصيغتين، إحداهما (`AddSeconds(-1)`) **تُسقط آخر ثانية** من اليوم الأخير |
| 20 ملفاً في `Screens/**` | ✅ منتهٍ | استهلكت `clsValidation.EndOfDay` (21 موضعاً) — `grep` على التعبير اليدوي = 0 تنفيذي |
| `Screens\POS\Forms\frmPOS.cs` | ✅ منتهٍ | فشل `GetUnitsAsync` كان يُعرَض «لا توجد وحدة بيع معرّفة» → رسالة الخدمة الحقيقية، والرسالة القديمة باقية للحالة الفارغة الناجحة فقط |
| `Screens\Reports\Forms\Accounting\frmGeneralLedgerReport.cs` | ✅ منتهٍ | `accountId <= 0` كان يُبلَّغ في تسمية فقط → حوار تحذير عربي مطابق لشاشة كشف الحساب |
| `Screens\Customers\Forms\frmCustomerStatement.Designer.cs` | ✅ منتهٍ | `lblNoData` و`dgvReport` كلاهما `Dock=Fill` ⇒ «لا توجد حركات» لا تُرى أبداً → `Anchor` + `BringToFront` (بلا إعادة ترتيب `Controls` — `RULE-N125`) |
| `frmPurchasesReport.cs` · `frmSupplierBalanceReport.cs` | ✅ منتهٍ | أسماء أعمدة ميتة (`WarehouseName` · `Phone`) أُزيلت |
| `Screens\Inventory\Forms\frmInventory.cs` | ⚠️ `T-56` فقط | `"N3"`/`"N2"` في مواصفة الأعمدة → `clsFormat.QuantityPattern`/`MoneyPattern` |

### 11.14 ملخّص الحكم النهائي

| الحكم | العدد | الملفات |
|---|---|---|
| ✅ **منتهٍ** (لا عيب قائم من فئات §11.0) | **كل ما لم يُذكر في الصفّين التاليين** | كل الشاشات والمكوّنات والتقارير الـ31 والبيوت المذكورة في §11.2–§11.13 |
| ⚠️ **مفتوح بسبب باك‑إند مجمَّد (`RULE-N177`)** | **10** | `frmOpeningStock` (تاريخ المستند) · `frmSupplierMaster` (`CategoryId => null`) · `frmListSupplierPayments` (`T-39`) · `frmAddUpdateCustomerReceipt` (حقول التعديل) · `frmPasswordChange` (تكافؤ الرفض) · `frmInventory` (`T-56`) · `frmPOS` (فحص المخزون المسبق) · `frmIncomeStatementReport` + `frmWorkingCapitalReport` (`T-43`) · `frmUserMaster` (إعادة تعيين كلمة المرور) |
| ⚠️ **مفتوح بسبب قرار مالك** | **6** | `Helpers\ThemeHelper` (`T-52`) · `frmCalculator` (منسّق دقة كاملة + زر نسخ) · `frmCustomerStatement` (ازدواج النقد) · `frmBackup` (تنبيه إعادة التشغيل بعد الاستعادة) · 7 نماذج `D-03` · 4 نوافذ طباعة `D-04` |

**ملاحظة صدق:** لا ملف في هذه الموجة يحمل «✅ منتهٍ» مع بند مفتوح يمكن إصلاحه داخل الشاشة — كل `⚠️` أعلاه إما عقد/باك‑إند مجمَّد أو قرار مالك مكتوب هنا. ونقطتان لا تُصلَح في شاشة ولا يُدَّعى إغلاقهما: `frmAddUpdateSalesInvoice` (`ExpiryDateFormat = "yyyy/MM/dd"` خاص داخل الشاشة — `clsFormat` لا يملك عضو **نمط تاريخ** لـ`DateTimePicker`، فتحويله يغيّر شكل العرض = قرار مالك) و`frmSupplierStatement` (مرشّح قرار `D-03`).
