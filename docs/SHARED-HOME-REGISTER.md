# سجل البيوت المشتركة (Shared Home Register)

> **الغرض:** حصر **كل** كود مشترك في النظام — بيته الواحد، ما يعرضه، ومن يستهلكه فعلاً، وكل مخالفة قائمة بالأرقام. هذا الملف هو **قائمة العمل الوحيدة** لموجة «وحّد وطبّق على النظام كله».
>
> **القانون الحاكم:** `AGENTS.md` §2.30 — `RULE-N169` (المكان الواحد) · `RULE-N170` (ابحث قبل أن تكتب) · `RULE-N171` (معيار التعديل الواحد) · `RULE-N172` (سقف التكرار).
>
> **البيوت:** `SalesSystem.Desktop/Global Classes/` · `SalesSystem.Desktop/Helpers/` + `SalesSystem.Common/Helpers/` + `SalesSystem.Business/Helpers/`.
>
> **حدود هذا الملف:** يحمل **الحصر** و**المخالفة** و**مكان الإصلاح**. تفاصيل شاشات التنسيق (67 شاشة × 240 مخالفة) مصدرها الوحيد `docs/FORMAT-COVERAGE.md`، ولوحة توزيع العمل في `docs/FORMAT-PROGRESS.md` — لا تُنسخ هنا (`RULE-N174` بالقياس).
>
> **آخر تحديث:** 2026-11-16 (الموجتان 16 + بوابة الشاشة التجريبية) · **المرجع الملزم للقياس: §10.6.5** · **قائمة العمل الحقيقية: §10.6.2** (Stage A) و**§10.2** (Stage B مجمَّد) · البناء **0 أخطاء / 86 تحذيراً** (بناء كامل `--no-incremental` — كان 118 قبل الموجة 15، والموجتان 16 لم تُضيفا تحذيراً واحداً). **`AGENTS.md` §2.39 صار قانوناً:** لا تعميم لأي سلوك قبل شهادة شاشة تجريبية واحدة 100% (`RULE-N184`…`N187`) — والشاشة التجريبية الحالية `frmProductMaster` **غير مشهودة بعد**. §3–§8 محضر تاريخي لموجة 2026-11-14، و§0 و§10.1 لقطتان أوليتان تجاوزهما §10.6.1/§10.6.2.

---

## 0. نتيجة المراجعة في سطر

> ⛔ **لقطة التدقيق الأولى (قبل الموجات 1–13) — تُقرأ للتاريخ فقط.** الحالة الملزمة الآن: **§10.6.5** (القياس) و**§10.6.1** (مغلق) و**§10.6.2** (مفتوح Stage A) و**§10.2** (Stage B مجمَّد). أرقام هذا القسم وُجدت في التدقيق الأول ثم أُغلقت في الموجات التالية.

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
| P-15 | ~~`Color.MistyRose`/`DarkRed` باقية كألوان حالة~~ | — | ✅ **مغلق (قياس 2026-11-15):** `Color.MistyRose|DarkRed|LemonChiffon|ForestGreen|DimGray` في `Screens/**` **= 0** — التلوين صار من `GridFormatHelper`/`GreenThemeHelper` | ✅ |

---

## 10. لوحة المهام المفتوحة — مراجعة وحدة بوحدة (2026-11-15)

> **الطريقة:** 95 شاشة + 96 مصمّماً في `Screens/**` قُرئت وحدة بوحدة بأربعة مراجعين مستقلين (`code-reviewer` قراءة فقط)، وكل بند أدناه **مقيس بـ`file:line`** لا مقدَّر. البناء بعد كل موجة = **0 أخطاء**.
>
> **الأولوية:** `أ` = فقدان بيانات/مال أو شاشة لا تُفتح · `ب` = سلوك خاطئ مرئي · `ج` = بيت واحد/نظافة.
> **المرحلة:** `A` = قابل للتنفيذ الآن داخل الـMock/الشاشة · `B` = يحتاج باك-إند أو عضواً في عقد ⇒ **مجمَّد** حتى موافقة الوحدة المكتوبة (`RULE-N177`).

### 10.1 مفتوح — المرحلة A (شاشة/محاكاة، بلا باك-إند)

> ⛔ **هذا الجدول لقطة صباح 2026-11-15 — وليس المرآة الحالية.** صفوفه الكثيرة أُغلقت فعلاً في الموجات 1–13، والمرجع الملزم الآن: **§10.6.1** (مغلق بالقياس) · **§10.6.2** (مفتوح Stage A — قائمة العمل الحقيقية) · **§10.6.3/§10.2** (Stage B مجمَّد) · **§10.6.5** (القياس النهائي). الصفوف المغلقة (T-02 · T-06 · T-08 … T-37) تُقرأ **للتاريخ فقط**؛ لا تُعدّ عملاً مفتوحاً.

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
| **T-11** | ~~`frmInventoryAddProduct` غير موصول بأي شاشة~~ | `Program.cs:379` · `frmMain` | **✅ مغلق** — حُذفت الشاشة (15 نموذج مخزون الآن) وصفر مرجع (`RULE-N102`) | — | — | ✅ |
| **T-12** | 4 نوافذ طباعة مخزون بلا مستدعٍ وبيانات مصنوعة (`INV-2026-001` · `TR-2026-001`) و«نجاح طباعة» بلا طابعة | `frmStocktakingPrint*.cs` · `frmWarehouseTransferPrint.cs` | واجهات كاذبة | `printing` / قرار | M | ب |
| **T-13** | 7 نماذج شراء ميتة مسجَّلة في DI ببيانات وهمية (`frmPurchaseItem` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` · `frmPurchaseItemInfo` · `frmPurchaseInvoicePrint` · `frmSupplierStatement`) | **مقيس:** `Program.cs:390,391,392,393,394,395,400` (لا `381-391`) | **تدقيق 2026-11-15 مكتمل:** كلها `ميت — مرجع واحد (Program.cs)` · **صفر** مرجع من `frmMain` (72 عنصر قائمة مُدقَّق) أو من أي شاشة · `frmPurchaseInvoice` ينفّذ وظائفها داخلياً بخدمات حقيقية ⇒ أي ربط = تكرار `RULE-N172` | قرار مالك — **التوصية: سحبها من DI وحذف 14 ملفاً** (لا `.resx`) | M | ج |
| **T-14** | `frmDailyClosureReport` يستدعي **نفس** استدعاء ملخص الصناديق ⇒ «الإقفال اليومي» = إجماليات كل الزمن | `Screens/Reports/Forms/CashBox/frmDailyClosureReport.cs:43` | قراءة مالية مضلِّلة | `ui-agent` (لافتة صادقة) + عقد `B` | S | أ |
| **T-15** | `frmProfitByCustomerReport` بلا عمود ربح، و`profit` محسوب «المتبقي» ثم غير مستخدم | `.../Sales/frmProfitByCustomerReport.cs:62` | تقرير بعنوان كاذب | قرار مالك (تسمية أو عقد ربح) | S | ب |
| **T-16** | 7 تقارير ترسل رسائل عبر `INotificationService` بينما 24 عبر `clsDialogs` | `frmCustomerBalance` · `frmSupplierBalance` · `frmInventory` · `frmLowStock` · `frmProductMovement` · `frmPurchases` · `frmSales` | بيتان للرسالة | `ui-agent` | S | ج |
| **T-17** | `frmPurchasesReport` يعيد تنفيذ تنسيق المال/التاريخ في `CellFormatting` | `.../Purchases/frmPurchasesReport.cs:138-154` | نسخة ثانية من `GridFormatHelper` | `ui-agent` | S | ج |
| **T-18** | تلوين الرصيد أحمر/أخضر مكرّر في 4 تقارير بلا بيت | `frmCustomerBalance` · `frmSupplierBalance` · `frmInventory` · `frmLowStock` | `RULE-N172` | `ui-agent` (يضيفه لـ`GridFormatHelper`) | S | ج |
| **T-19** | ~~`frmTrialBalanceReport` 8 أعمدة مالية بلا أي تنسيق~~ | `.../Accounting/frmTrialBalanceReport.cs` | **✅ مغلق + تصحيح قياس:** الـDTO يحمل **6** حقول مالية لا 8، وكلها منسّقة (`GridFormatHelper` 6/6) | — | — | ✅ |
| **T-20** | ~~18 تقريراً لا تتحقق `من <= إلى`~~ — **تصحيح مقيس:** 12 تقريراً كانت ناقصة الحارس، **4 منها لا تملك منتقي تاريخ أصلاً** (`frmInventoryReport` · `frmStockBalanceReport` · `frmLowStockReport` · `frmExpiredProductsReport`) فلا حارس لها، و**3 تعمل بتاريخ واحد** (`frmTrialBalanceReport` · `frmBalanceSheetReport` · `frmWorkingCapitalReport` · `frmCashBoxSummaryReport`) | الحارس الآن: `clsValidation.IsDateRangeValid` = **21** استدعاءً + `EndOfDay` = **21** | **✅ مغلق بصدق** — لم يُقحم حارس وهمي على شاشة بلا مدى | — | — | ✅ |
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
| **T-65** | **خصم مزدوج في الباك-إند الحقيقي:** `DailySalesSummaryDto.NetAmount = g.Sum(si => si.NetTotal - si.DiscountAmount)` و`NetTotal` صافٍ أصلاً ⇒ «الصافي» ينقص بمقدار الخصم مرتين. **أُصلح في المحاكاة (`MockSalesReportDesktopService` — §11.13، الموجة 12) ولم يُصلح في BLL** | `SalesSystem.Business\Services\SalesReportService.cs:177` | منطق BLL · `frmDailySalesReport` يعرض `Sum(NetAmount)` ⇒ رقم مالي خاطئ في الوضع الحقيقي | `backend-architect` بعد موافقة |
| **T-66** | **«المستحق» ≠ مجموع شرائح الأعمار في المستودع الحقيقي:** `totalDue = Σ RemainingAmount` (يشمل ≤ 0) بينما الشرائح تُجمع لـ`RemainingAmount > 0` فقط | `SalesSystem.DataAccess\Repositories\ReportRepository.cs:572-573` (+`579-580` شرط التخطي) | نفس عيب المحاكاة الذي أُغلق — ما زال قائماً في EF | `backend-architect` بعد موافقة |
| **T-67** | عضو عقد يعيد `null` دائماً: `StockReportDto.ProductCode => null` (موضعان) و`PurchaseInvoiceDto.ProductCode => null` ⇒ العمود لا يُملأ أبداً (لا ضرر مرئي الآن لأن التقريرين يُخفيانه) | `SalesSystem.DTO\Responses\ReportDtos.cs:388,480` · `PurchaseInvoiceDtos.cs:67` | تنظيف عقد: `RULE-N110` (عضو بلا سلوك مقابل) | `backend-architect` بعد موافقة |

### 10.3 قرارات المالك المطلوبة (لا تُنفَّذ بلا نص)

| # | القرار | الخيارات |
|---|---|---|
| **D-01** | `GAP-06` — تصدير Excel: أي الشاشات؟ | 36 شاشة: قوائم ⇐ تُصدَّر / محرّرات ⇐ لا (اقتراح) |
| **D-02** | شبكات إدخال بـ`AllowUserToAddRows = true` خارج الاستثناءين الموثَّقين — **مقيس الآن: 4** (`frmStocktakingNew` · `frmPurchaseInvoice` · `frmSalesReturn` · `frmPurchaseReturn`) بعد حذف `frmInventoryAddProduct` و`frmPOS` (البند كان يعدّ 5) | توسيع `RULE-N22a` أو إبقاء الاستثناءين |
| **D-03** | 7 نماذج شراء ميتة (T-13) — **تدقيق مكتمل 2026-11-15:** كلها `ميت — مرجع واحد (Program.cs)`، صفر مستدعٍ، وبياناتها مصنوعة (`INV-2026-001` · `MockSubtotal = 150000` · «تمت طباعة فاتورة 5003 بنجاح» بلا طابعة). وظائفها منفَّذة فعلاً داخل `frmPurchaseInvoice` بخدمات حقيقية | **التوصية: سحب من DI + حذف 14 ملفاً** (`Program.cs:390-395,400`) · الاستثناء الوحيد الذي يستحق قراراً منفصلاً: `frmSupplierStatement` (تغطّي وظيفة غير موجودة في القائمة — إن أردتها فالمسار الصحيح إعادة بنائها Stage A بعقد `ISupplierStatementDesktopService`) |
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
| **T-52** | حرفيات `ThemeHelper`/`ExportHelper` — ⚠️ **قرار مالك، لا يُغلق بالكود** | `ThemeHelper` **4 TODO** موثَّقة (لا مفتاح مطابق في `ThemePalette`) · `ExportHelper` يقرأ الحيّ أولاً والثابت احتياطاً موثَّقاً — **الحالة الحقيقية في §10.6.2** | بيتان |
| **T-54** | `_ClampToPickerRange(DateTime)` — نسخة واحدة غير مكرّرة | **يُراقَب** — لا ثاني لها حتى اليوم | §10.6.2 |
| **T-58** | `ShowLoading(bool)` مكرّر **31** مرة و`SetHeader` **30** مرة (بيتان بنفس الجسم في 31 تقريراً) | **= 0 جسم مكرّر** — بيت واحد `ReportUiStandardizer.ShowLoading(Form?, Control?, bool)` + `SetHeader(DataGridView?, string, string)` · **31** تفويضاً لـ`ShowLoading` و**30** لـ`SetHeader` · `loadingPanel.BringToFront` في `Screens/**` **= 0** | `Helpers\ReportUiStandardizer.cs` (الموجة 13) |
| **T-59** | ~~T-23 «معالجات فارغة = 0»~~ — **تصحيح قياس:** الفحص السطري أخطأ 5 معالجات فارغة **متعددة الأسطر** | **= 0** بفحص متعدد الأسطر: حُذفت 4 من `frmAccountStatementReport` (`loadingPanel_Paint` · `loadingPanel_Paint_1` · `label4_Click` · `dtpTo_ValueChanged`) وواحدة من `frmGeneralLedgerReport` (`nudAccountId_ValueChanged`) **مع وصلات المصمّم** — ولا واحدة منها كانت تحمل سلوكاً (التحديث عبر `btnRefresh`/`Load`) | الموجة 13 |
| **T-60** | **محاكاة كاذبة:** سند قبض `1001` للعميل **1** مخصَّص على الفاتورة **1002** التي تخصّ العميل **2**، بمبلغ 45,000 > متبقيها | **= 0** — صار السند 1001 (17,500) مخصَّصاً على فاتورته **1007** للعميل نفسه، والمتبقي 0 = 0؛ ورصيد العميل في «الأرصدة» = «كشف الحساب» = 0 | `MockCustomerReceiptsData.cs` |
| **T-61** | تواريخ سندات القبض مطلقة (2026-09) بينما فواتير المحاكاة نسبية (`Today.AddDays(-n)`) ⇒ سند يسبق فاتورته بشهرين | **= 0** — كل سند مرحّل صار على يوم فاتورته، والمسودات/الملغى بقي ترتيبها الداخلي (تنبيه: الترتيب الرقمي الكامل مستحيل ما دامت المسودات في أيلول) | `MockCustomerReceiptsData.cs` |
| **T-62** | تعليق `IStocktakingDesktopService.CancelAsync` يصف سلوكاً قديماً (المسودة لم تكن تُلغى) | **= 0** — التعليق يصرّح: المسودة تُلغى بلا حركة رصيد · المرحّل يُعكس بحارس كفاية · الملغي يُرفض | `Services\Interfaces\IStocktakingDesktopService.cs:27-33` |
| **T-63** | سندات صرف الموردين `2003`/`2004` تدّعي سداد فاتورة محددة بينما `PurchaseInvoiceId = null` | **= 0** — النص صار «دفعة على الحساب» (والأرصدة تتسق لأن الخدمة تطرح المدفوعات من متبقي الفواتير المرحّلة) | `MockSupplierPaymentsData.cs` |
| البيوت | `MessageBox` خارج `clsDialogs` **= 0** · `ex.Message` للمستخدم **= 0** · `ExportHelper` في 32/32 تقريراً · `GreenThemeHelper.Apply(this)` في كل نموذج غير تقريري **= 0 ناقص** · تسجيل DI **94/94** بأسماء مؤهَّلة | — | — |
| **T-68** | **`_cache.Clear()` قبل فحص `IsSuccess`** — عند فشل إعادة التحميل تبقى الذاكرة فارغة والمحرر يحتفظ بالسجل ⇒ «تعديل + حفظ» يقرأ `IsActive` (وحقلاً محفوظاً) كأنه مفقود | **= 0** في **9 شاشات** (`frmCustomerMaster` · `frmEmployeeMaster` · `frmCashBoxMaster` · `frmProductMaster` · `frmCategoryMaster` · `frmUnitMaster` · `frmSupplierMaster` · `frmWarehouseMaster` · `frmUserMaster`) — نُقل المسح **داخل** فرع النجاح · `frmAccounts` كان سليماً (تحقق ثم `return` قبل المسح) | الموجة 14 |
| **T-69** | `cbStatusFilter` بلا `DisplayMember` ⇒ `ListControl` يعرض `StatusOption { Value = 0, Text = … }` | **= 0** — سطر واحد `DisplayMember = nameof(StatusOption.Text)` | `frmJournalEntries.cs:249` |
| **T-70** | **حارس تركيز ميت:** `Form.ActiveControl` يعيد الحاوية (`pnlFields`/`gbFilter`) لا الحقل ⇒ `ActiveControl is TextBoxBase` لا يتحقق أبداً، و`Delete` أثناء الكتابة ينفّذ إجراء السجل | **= 0** حارس ميت · بيت واحد `KeyboardHelper.IsTextInputFocused(Form?)` فوق `FindFocusedControl` القائم · يستهلكه `frmAccounts` · `frmJournalEntries` · `frmCustomerMaster` (+ `WireGlobalShortcuts`) | `Helpers\KeyboardHelper.cs:286` |
| **T-71** | **`WireGlobalShortcuts` يبتلع المفتاح دائماً** ⇒ `Delete` لا يحذف حرفاً في أي حقل نصي بأي شاشة تسجّله (والحارس في الشاشات وُجد لهذا الغرض بالضبط) | **= 0** — الإعفاء مقصور على `Keys.Delete` عندما يكون التركيز في حقل إدخال؛ بقية المفاتيح (F2/F3/F4/F5/Ctrl+S/Esc) تُبتلع كما هي | `Helpers\KeyboardHelper.cs:571` |
| **T-75** | `using` مكرّر (CS0105) — **2** لا واحدة | **= 0** — `PrintService.cs:9` (`using SalesSystem.DataAccess.Printing;`) و`IInventoryDesktopService.cs:5` (`using SalesSystem.DTO;`) حُذفا والسابق باقٍ | الموجة 15 |
| **T-76** | عضو يخفي عضواً موروثاً (CS0108) — `ProductName` يخفي `Control.ProductName` | **= 0** — صار `SelectedProductName`؛ والقياس أثبت **صفر** مستهلك للخاصية | `frmPurchaseItem.cs:107` |
| **T-77** | حقل يُكتب ولا يُقرأ (CS0414) — `_isBindingLookups` | **= 0** — الحقل حُذف و**3** كتل `try/finally` فُكّت إلى عباراتها؛ القياس: لا معالج `cmbBranch_SelectedIndexChanged` أصلاً ⇒ الحارس كان نيّة ميتة، والحذف صفر أثر سلوكي | `frmUserMaster.cs:37,259,583,716` |
| **T-78** | `Task` غير مُنتظَر (CS4014) | **= 0** — `() => _ = _ChooseProductAsync()` وهو اصطلاح الملف نفسه (السطران 372 · 388) | `frmInventoryFifo.cs:70` |
| **T-79** | حقول مصمّم ميتة (CS0169) — `dataGridViewCellStyle1/2/3` **×9 تقارير = 27 حقلاً** | **= 0** — حُذفت الحقول غير المستخدمة وحدها؛ أنماط الشبكة الحقيقية (`dataGridViewCellStyle4/5/6` داخل `InitializeComponent`) لم تُمسّ، و`RULE-N134` سليم | 9 مصمّمات في `Screens\Reports\Forms\` |
| **T-80** | **لا تغذية للحقل المطلوب عند مغادرته:** التحقق يعمل **عند الحفظ فقط** — لا أيقونة ولا تنبيه حين يترك المستخدم حقلاً مطلوباً فارغاً، فيكتشف الخطأ متأخراً بعد ملء الشاشة | **= 0 لـ`frmProductMaster`** — بيت واحد `Helpers\RequiredFieldHelper.cs` (`Attach`/`Validate`/`FirstMissing`/`Clear` + `RequiredField`) يضع **أيقونة ✗ وخلفية `SelectionRowColor` معاً** عند المغادرة/الحفظ ويزيلهما فور صحّة القيمة، واللون حيّ من `ThemePalette` (`RULE-N180`…`N183`) — والبقية `T-81` | `Helpers\RequiredFieldHelper.cs` + `frmProductMaster.cs:105,826,927,1004,1006` (بوابة الحفظ في `_SaveProductAsync` لا في `_ValidateForm` المحذوفة) |

#### 10.6.2 ما زال مفتوحاً — المرحلة A (قابل للتنفيذ الآن)

| # | المهمة | الموضع (مقيس) | العيب | المالك | الحجم |
|---|---|---|---|---|---|
| **T-52** | `ThemeHelper` يحمل **4** ألوان حرفية موسومة `TODO(ThemePalette key)` — لا مفتاح مطابق في `ThemePalette` (التحقق: `SidebarDark (33,43,54)` · `TextDark (50,50,50)` · `ButtonForeOnSolid White` · `GhostHoverBack (240,240,240)` — كلها بلا مفتاح دوري مطابق) | `Helpers\ThemeHelper.cs:29,46,60,134` | **يحتاج قرار مالك**: إضافة مفتاح `textOnSolid` + `sidebarDark` + `hoverBack` إلى `ThemePalette` (`RULE-N54a`) — لا يُخترع مفتاح بلا أمر | `ui-agent` بعد قرار | S |
| ~~**T-55**~~ | ✅ **مغلق** — `DocumentTextHelper.StatusOptions` أُضيف واستُهلك | — | العضو الناقص صار موجوداً، و`T-57` بُني عليه | ✅ | — |
| **T-56** | `frmInventory` يقارن `StatusName` **نصاً** (`RowFilterHelper.EqualsText`) وعقد الخدمة يعيد «مرحّل» بالشدّة بينما نص البيت «مرحل» ⇒ توحيده يغيّر نتيجة التصفية (`frmInventory.cs:1102` `e.Value.ToString() switch`) | `frmInventory.cs:62,64,636,641,1102` | **مجمَّد**: الإصلاح الصحيح = عمود `Status` رقمي أو `StatusName` من البيت في العقد (`RULE-N177`) | `backend-architect` بعد موافقة | S |
| ~~**T-57**~~ | ✅ **مغلق (الموجة 11)** — 7 شاشات تُبنى قوائم حالتها من `StatusOptions` بعد `InitializeComponent()`، والقائمة الثابتة أُزيلت من المصمّمات | §11.12 | كان `RULE-N169` | ✅ | — |
| **T-54** | `_ClampToPickerRange(DateTime)` — منطق حصر تاريخ داخل مدى `DateTimePicker` (نسخة واحدة، غير مكرّرة) | `frmJournalEntries.cs:964` | لا مخالفة الآن؛ يُرقّى إلى بيت تواريخ إن تكرّر في شاشة ثانية | يُراقَب | — |
| **T-64** | **نص «وضع التجربة» مكرّر:** `private const string RealModeNotice` بنفس النص حرفياً في 3 شاشات، و`MockOnlyNotice` بنص مختلف قليلاً في شاشة رابعة | `frmEmployeeMaster.cs:27` · `frmStocktakingNew.cs:35` · `frmWarehouseTransferNew.cs:36` · `frmInventory.cs:47` | `RULE-N169` — النص المشترك مكانه `DocumentTextHelper` (مثل `StatusOptions`)؛ **لم يُنفَّذ في الموجة 13** لأن توحيد الصياغة يغيّر نصاً يراه المالك ⇒ يُنفَّذ بقراره | `ui-agent` بعد قرار | S |
| **T-73** | **8 نسخ محلية من حارس التركيز باقية:** `_IsTextInputFocused()` في 7 ماسترات + `_IsTextEditingFocused()` في `frmProductUnitMaster` — كلها **صحيحة وظيفياً اليوم** (لا عيب سلوكي)، لكنها دين `RULE-N172` (3+ نسخ) وتتباين دلالياً: نسخة `frmCustomerMaster` كانت قائمة أزرار يدوية تُسقط `txtId`، ونسخة `frmProductUnitMaster` تنزل سلسلة الحاويات | `frmCashBoxMaster:122` · `frmCategoryMaster:124` · `frmProductMaster:136` · `frmProductUnitMaster:125` · `frmSupplierMaster:123` · `frmUnitMaster:115` · `frmUserMaster:129` · `frmWarehouseMaster:124` | تُحذف كلها وتُستهلك `KeyboardHelper.IsTextInputFocused(this)` — تعديل متطابق ⇒ **S-Batch** على دفعتين (4 + 4) · لا خطر سلوكي لأن مسار الاختصار يمرّ بالبيت أولاً | `ui-agent` | S |
| **T-74** | **14 نموذجاً `frm*` مسجَّلاً في DI ولا يفتحه أحد** (بحث حدودي `(?<![A-Za-z0-9_])frmX(?![A-Za-z0-9_])` على كل `*.cs` ما عدا ملفَّي النموذج وتصريح `Program.cs`): `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseOtherDetails` · `frmPurchaseInvoicePrint` · `frmCustomerSelection` · `frmCustomerStatement` · `frmSupplierSelection` · `frmSupplierStatement` · `frmStocktakingPrint` · `frmStocktakingPrintCard` · `frmStocktakingPrintAdjustment` · `frmWarehouseTransferPrint` — وشاشة `frmPurchaseItem` تحمل تعليقاً يصف «النموذج الأب الذي يقرأ نتائج الحوار» **ولا أب لها في الحل** | §11.17 | `RULE-N102` كود ميت: إما تُفتح من القائمة أو تُحذف من المشروع ومن DI — **قرار مالك** لأنه يغيّر سطح التطبيق (`D-03` مجموعة فرعية منه) | `ui-agent` بعد قرار | S (14 ملفاً × 3) |
| **T-81** | **تعميم `T-80` على بقية الشاشات:** القياس = **28 شاشة · 187 موضع `errorProvider.SetError`** (بعد إزالة مواضع الشاشة التجريبية؛ 182/27 باستثناء `frmPasswordChange`) كلها تتحقق **عند الحفظ فقط**؛ و**0** شاشة تُغذّي الحقل عند مغادرته. **الدَين المقيس:** `_FocusFirstMissingField` في **9** · `_RequiredFieldsValid` في **4** · `_ValidateForm` في **20**. الترتيب المحسوب: `frmProductUnitMaster` · `frmSupplierMaster` · `frmCustomerMaster` · `frmCategoryMaster` · `frmUnitMaster` · `frmWarehouseMaster` · `frmCashBoxMaster` · `frmUserMaster` · `frmEmployeeMaster` · ثم الدلائل/المستندات | `Screens\*\Forms\*.cs` (البيت جاهز: `RequiredFieldHelper`) | كل شاشة تُعلن قواعدها بسطر `Attach` واحد وتُعزّل نسختها الخاصة (`_RequiredFieldsValid`/`_FocusFirstMissingField`) — **لا لون ولا `SetError` جديد داخل الشاشة**. **محجوب ببوابة §2.39 (`RULE-N184`):** لا تُلمس أي شاشة قبل شهادة `frmProductMaster` 100% (`D-11`) | `ui-agent` — شريحة لكل شاشة (`RULE-N156`)، **ممنوع** «إصلاح كل الشاشات» | S/M (شريحة لكل شاشة) |

> **ملاحظة قياس:** `_ToNullIfEmpty` **= 0** · `_Clamp` **= 0** (المتبقي `_ClampToPickerRange` وهو دالة أخرى) · `private const CashCustomerName` **= 0** · `private _GetStatusText` **= 0** · `CellFormatting` في التقارير **= 0** · `IsDateRangeValid` **= 21** استدعاءً · `NumericInputHelper` **13** مرجعاً · `DocumentTextHelper` **48** مرجعاً · `clsValidation.EndOfDay` **21** استدعاءً. **(كل هذه الأرقام تجاوزها §10.6.5 بعد الموجات 9–12.)**
>
> **T-53 (مغلق):** كل **عرض/تلوين** للحالة صار من البيت أو من قيمة `Status` الرقمية (`frmListSales:749-750` · `frmListPurchases:758-759` · `frmJournalEntries:1596` · `frmAddUpdateInventoryAdjustment:44,257,626` · `frmPurchaseInvoice:372` · `frmSalesReturn:329` · `frmPurchaseReturn:338`) — **صفر مقارنة بنص عربي** في تلوين الحالة. أُغلق الباقي بـ`T-55`/`T-57` (8 مواضع قوائم خيارات)؛ يبقى موضع واحد في `frmAddUpdateSalesInvoice` (ملصق مركّب لا نص حالة — لا يُغيَّر) و4 مواضع في `frmInventory` مجمَّدة بالعقد (`T-56`).

#### 10.6.3 ما زال مفتوحاً — المرحلة B (مجمَّد، `RULE-N177`)

كما في §10.2: `T-38`…`T-46` بلا تغيير — كلها تحتاج عضواً في عقد أو باك-إند، وتنتظر موافقة الوحدة **المكتوبة**.

#### 10.6.4 قرارات مالك معلَّقة (لا تُنفَّذ بلا نص)

`D-01` تصدير Excel (36 شاشة) · `D-02` شبكات `AllowUserToAddRows = true` (**4** في الكود الآن) · `D-03` 7 نماذج شراء ميتة · `D-04` 4 نوافذ طباعة مخزون · `D-05` تقرير الربح حسب العميل · `D-06` `MockAuthService` بلا مستهلك · `D-07` موافقة أي وحدة في §10.2 · **`D-08` (جديد)** توحيد نص «وضع التجربة» في 4 شاشات (`T-64`) — يغيّر نصاً مرئياً · **`D-09` (جديد)** **النقر على سطر في الشبكة أثناء وضع الإضافة يمحو ما كتبه المستخدم بلا سؤال** — السلوك موجود في **كل** الماسترات بما فيها الشاشة المرجعية `frmProductMaster`، فإصلاحه حارس واحد مشترك يستهلكه الجميع (`RULE-N171`) وتغيير سلوك مرئي: **إبقاء السلوك** / **تأكيد قبل الإفراغ** / **تجاهل النقر ما دام `EntryMode.Adding`** · **`D-10` (جديد)** **`T-74` — 14 نموذجاً مسجَّلاً في DI ولا يفتحه أحد** (القياس الحدودي في الموجة 15): إما تُربط من القائمة/الشاشة الأم أو تُحذف من المشروع ومن DI (`RULE-N102`) — `D-03` مجموعة فرعية منه وعدّه «7» كان أقل من المقيس · **`D-11` (جديد)** **تعميم تغذية الحقل المطلوب (`T-81`)** — **صار بوابة قانونية لا قراراً فقط:** `AGENTS.md` §2.39 (`RULE-N184`) تمنع لمس أي شاشة ثانية قبل **شهادة 100%** للشاشة التجريبية `frmProductMaster` بتأكيد المالك المكتوب (شرط `RULE-N185` السادس). ومعه يُحسم سؤالان: هل يبقى اللون الأصفر أيضاً على الحقل **المركَّز** الفارغ (اليوم يُرى الرمز وحده لأن اللون نفسه هو لون التركيز)، وهل يُضاف تلميح `clsInputFeedback` بجانب الأيقونة أم تكفي الأيقونة.


#### 10.6.5 القياس النهائي — بعد موجة الإصلاح الكاملة (2026-11-15، المرجع الحالي)

> **البناء:** `dotnet build .\SalesSystem.slnx --no-incremental` ⇒ **0 أخطاء / 86 تحذيراً** (كان 125 ⇒ 118 في الموجة 14 ⇒ 86 في الموجة 15). التحذيرات ليست بوابة؛ البوابة **0 أخطاء**.
> **طريقة العدّ:** مجموع التحذيرات من ملخّص البناء الرسمي، والمثيلات المميَّزة بفتح السجل مرة واحدة على `ملف:سطر:رمز` — لأن الأرقام لا تُقارن بين بناء تزايدي وكامل (`§12` في برومبت الـOrchestrator).
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
| `ReportUiStandardizer.ShowLoading` (بيت التحميل) | **2** عضوان في البيت · **31** استدعاءً من التقارير · **0** جسم مكرّر · `loadingPanel.BringToFront` في الشاشات = **0** | 31 نسخة من 4 أسطر | ✅ `T-58` |
| `ReportUiStandardizer.SetHeader` (بيت العناوين) | **30** استدعاءً · **0** جسم مكرّر (تقرير واحد لا يملك العضو أصلاً) | 30 نسخة | ✅ `T-58` |
| معالجات فارغة (`void X(...) { }`) بفحص **متعدد الأسطر** | **0** | 5 (كان الفحص السطري يعدّها 0 — قياس خاطئ) | ✅ `T-59` |
| تخصيص سند قبض لفاتورة **عميل آخر** في المحاكاة | **0** | 1 (السند 1001 → الفاتورة 1002) | ✅ `T-60` |
| `MessageBox` / ألوان دلالية حرفية / صيغة يدوية في التقارير الـ31 | **0** | — | ✅ |
| `_ToNullIfEmpty` / `_Clamp` / `_GetStatusText` / `private const CashCustomerName` | **0 · 0 · 0 · 0** | 18 · 5 · 2 · 3 | ✅ |
| `_cache.Clear()` قبل فحص `IsSuccess` | **0** في **9 شاشات** أُصلحت (`frmAccounts` كان سليماً) | 9 | ✅ `T-68` |
| حارس تركيز ميت `ActiveControl is TextBoxBase` | **0** | 2 (`frmAccounts` · `frmJournalEntries`) | ✅ `T-70` |
| `KeyboardHelper.IsTextInputFocused` (بيت حارس التركيز) | **1** تعريف + **4** مستهلكين (بيت `WireGlobalShortcuts` + 3 شاشات) | 11 نسخة محلية متفاوتة | ✅ `T-70` |
| نسخ حارس تركيز محلية باقية (`_IsTextInputFocused`/`_IsTextEditingFocused`) | **8** (دين `RULE-N172` — لا عيب سلوكي) | 11 قبل الموجة | ⚠️ `T-73` مفتوح |
| `ComboBox` مملوء بكائنات بلا `DisplayMember` | **0** في `frmJournalEntries` (كان `cbStatusFilter`) | 1 | ✅ `T-69` |
| `Delete` مبتلَع داخل حقل إدخال | **0** — الإعفاء في البيت مقصور على `Keys.Delete` | كل شاشة تسجّل `Keys.Delete` | ✅ `T-71` |
| `using` مكرّر (`CS0105`) | **0** | 2 | ✅ `T-75` |
| عضو يخفي عضواً موروثاً (`CS0108`) | **0** | 1 | ✅ `T-76` |
| حقل يُكتب ولا يُقرأ (`CS0414`) | **0** | 1 | ✅ `T-77` |
| `Task` غير مُنتظَر (`CS4014`) | **0** | 1 | ✅ `T-78` |
| حقول مصمّم ميتة (`CS0169`) | **0** | 27 (9 تقارير × 3) | ✅ `T-79` |
| **تغذية الحقل المطلوب عند مغادرته** (أيقونة ✗ + لون `SelectionRowColor` معاً) | **1 شاشة** (`frmProductMaster`) عبر بيت واحد `RequiredFieldHelper` — **0** شاشة أخرى | **0** شاشة في النظام كله | 🔄 `T-80` مغلق للشاشة النموذج · `T-81` محجوب بمراجعة المالك (`D-11`) |
| `errorProvider.SetError` داخل الشاشات (التحقق عند الحفظ فقط) | **187** موضعاً في **28** شاشة — كلها عند الحفظ، **0** منها عند المغادرة (كان **199 / 28** قبل إزالة مواضع الشاشة التجريبية · و**182 / 27** باستثناء `frmPasswordChange`) | 199 · 28 | ⚠️ `T-81` — الانحراف **مقيس** بطريقة عدّ مذكورة هنا لا مفترض |
| دَين نسخ حكم «مطلوب» في بقية الشاشات | `_FocusFirstMissingField` **9** شاشات · `_RequiredFieldsValid` **4** · `_ValidateForm` **20** | — | ⚠️ `T-81` — مجمَّد ببوابة `RULE-N184` حتى شهادة الشاشة التجريبية |
| `RequiredFieldHelper` | **1** تعريف · **1** مستهلك (`frmProductMaster`) · **5** مواضع استهلاك (`Attach` · `Validate` · `FirstMissing` · `Clear`×2) · **0** لون رفض محلي · **0** ARGB مكتوب | 0 | ✅ `RULE-N180`…`N183` |
| **عيوب الشاشة التجريبية المفتوحة** (`PILOT-1`…`PILOT-4`) | **0** — أُصلحت كلها **في الشاشة التجريبية** قبل أي تعميم | — | ✅ `RULE-N184`/`N186` · الشهادة (تأكيد المالك) معلّقة |
| نسخ حكم «مطلوب» في الشاشة التجريبية (`_RequiredFieldsValid` · `_FocusFirstMissingField` · `_ValidateForm`) | **0 · 0 · 0** | 1 · 1 · 1 | ✅ `RULE-N172`/`N184` |
| الشاشات التي تُغذّي الحقل المطلوب عند مغادرته | **1** من **29** (`frmProductMaster` = التجريبية) | 0 | 🔄 `T-81` **محجوب ببوابة §2.39** |
| **مجموع التحذيرات** (بناء كامل `--no-incremental`) | **86** تحذيراً · **76** مثيلاً مميَّزاً (`ملف:سطر:رمز`) | 118 تحذيراً · 108 مثيلاً | ✅ الموجة 15 أنقصت **32** |
| عائلة nullable (`CS8602` 43 · `CS8604` 14 · `CS8618` 13 · `CS8601` 2 · `CS8605` 1 · `CS8600` 1) | **74** مثيلاً — **لم تُمسّ** | 74 | ⚠️ موجة مستقلة: الإصلاح يضيف فحوص `null` فعلية ⇒ سلوك، لا يدخل ضمن تنظيف التحذيرات |
| `CS8981` — صنف ترحيل اسمه حرفياً `update` | **2** | 2 | ⚠️ مجمَّد `RULE-N177` (داخل `Migrations/**`) |

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
| H-08 | `Helpers\NavigationOptions.cs` | `EnterFollowsDisplayOrder` · `EnterExitsToModeButton` · `RowBandTolerancePixels` · `ResetToDefaults` — **قيم التنقل الافتراضية الحيّة** (`RULE-N192`) | `KeyboardHelper.WireEnterAsTab(scope, primaryButton)` حصراً | مراجع داخل `Screens/**` = **0** · قراءات حيّة في `KeyboardHelper` = 4 |
| H-09 | `Helpers\RequiredFieldHelper.BlocksEnterNavigation(Control?)` | عضو واحد يُضاف إلى البيت القائم: «أهذا الحقل المطلوب فارغ الآن؟» — يوسم (✗ + ومضة) ويُرجع `true` | `KeyboardHelper` (فرع `WireEnterAsTab` ثنائي الوسيط) وحده | مراجع داخل `Screens/**` = **0** · حكم «فارغ» داخل `KeyboardHelper` = **0** (لا `IsNullOrWhiteSpace` ولا `SelectedValue`) · التعريف + استهلاك واحد فقط |

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

### 11.14 ملخّص الحكم حتى الموجة 12

> الموجة 13 (§11.15) جاءت **بعد** هذا الملخّص — تُقرأ معه، وأرقامها في §10.6.5.

| الحكم | العدد | الملفات |
|---|---|---|
| ✅ **منتهٍ** (لا عيب قائم من فئات §11.0) | **كل ما لم يُذكر في الصفّين التاليين** | كل الشاشات والمكوّنات والتقارير الـ31 والبيوت المذكورة في §11.2–§11.13 |
| ⚠️ **مفتوح بسبب باك‑إند مجمَّد (`RULE-N177`)** | **10** | `frmOpeningStock` (تاريخ المستند) · `frmSupplierMaster` (`CategoryId => null`) · `frmListSupplierPayments` (`T-39`) · `frmAddUpdateCustomerReceipt` (حقول التعديل) · `frmPasswordChange` (تكافؤ الرفض) · `frmInventory` (`T-56`) · `frmPOS` (فحص المخزون المسبق) · `frmIncomeStatementReport` + `frmWorkingCapitalReport` (`T-43`) · `frmUserMaster` (إعادة تعيين كلمة المرور) |
| ⚠️ **مفتوح بسبب قرار مالك** | **7** | `Helpers\ThemeHelper` (`T-52`) · `frmCalculator` (منسّق دقة كاملة + زر نسخ) · `frmCustomerStatement` (ازدواج النقد) · `frmBackup` (تنبيه إعادة التشغيل بعد الاستعادة) · **`T-64` نص «وضع التجربة» في 4 شاشات (`D-08`)** · 7 نماذج `D-03` · 4 نوافذ طباعة `D-04` |

**ملاحظة صدق:** لا ملف في هذه الموجة يحمل «✅ منتهٍ» مع بند مفتوح يمكن إصلاحه داخل الشاشة — كل `⚠️` أعلاه إما عقد/باك‑إند مجمَّد أو قرار مالك مكتوب هنا. ونقطتان لا تُصلَح في شاشة ولا يُدَّعى إغلاقهما: `frmAddUpdateSalesInvoice` (`ExpiryDateFormat = "yyyy/MM/dd"` خاص داخل الشاشة — `clsFormat` لا يملك عضو **نمط تاريخ** لـ`DateTimePicker`، فتحويله يغيّر شكل العرض = قرار مالك) و`frmSupplierStatement` (مرشّح قرار `D-03`).

### 11.15 الموجة 13 — بيت التحميل/العناوين + إصلاح محاكاة السندات (3 وكلاء)

> **سبب الموجة:** مراجعة هذا السجل نفسه كشفت **تناقضاً بين اللوحة والكود**: §10.6.1 يقول «معالجات فارغة = 0» بينما الفحص السطري أخطأ **5** معالجات فارغة متعددة الأسطر؛ و§10.6.1 يقول «`SetHeader`/`ShowLoading` بيت واحد» بينما الجسم نفسه مكرّر في **31** تقريراً؛ ومحاكاة سندات القبض تخصّص سنداً لفاتورة **عميل آخر**. البناء بعد الموجة = **0 أخطاء / 118 تحذيراً**.

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Helpers\ReportUiStandardizer.cs` | ✅ منتهٍ `T-58` | عضوان جديدان: `ShowLoading(Form?, Control?, bool)` (بحارس `IsDisposed/Disposing` واحد لكل التقارير) و`SetHeader(DataGridView?, string, string)` — **بيت واحد** بدل 60 نسخة |
| 31 تقريراً في `Screens\Reports\Forms\**` | ✅ منتهٍ `T-58` | حُذف جسم `ShowLoading` المكرّر من **31** ملفاً و`SetHeader` من **30**، وصار كل منها **تفويض سطر واحد** (قرار متعمَّد: صفر تغيير في 127 موضع استدعاء ⇒ صفر خطر انحدار) · `loadingPanel.BringToFront` في الشاشات = **0** |
| `Screens\Reports\Forms\Accounting\frmAccountStatementReport.cs` (+ مصمّم) | ✅ منتهٍ `T-59` | حُذفت 4 معالجات فارغة **متعددة الأسطر** (`loadingPanel_Paint` · `loadingPanel_Paint_1` · `label4_Click` · `dtpTo_ValueChanged`) مع وصلاتها — ولا واحدة تحمل سلوكاً (التحديث عبر `btnRefresh → LoadReportAsync`) |
| `Screens\Reports\Forms\Accounting\frmGeneralLedgerReport.cs` (+ مصمّم) | ✅ منتهٍ `T-59` | `nudAccountId_ValueChanged` الفارغ ووصلته أُزيلا (التحميل عبر `Load`/`F5`) |
| `Services\Mock\MockCustomerReceiptsData.cs` | ✅ منتهٍ `T-60`/`T-61` | السند **1001** كان مخصَّصاً للفاتورة **1002** التي تخصّ **عميلاً آخر** (ومبلغ 45,000 > متبقيها) ⇒ صار مخصَّصاً على فاتورة العميل نفسه **1007** بمبلغ **17,500**، فرصيد «أرصدة العملاء» = «كشف الحساب» = 0 · وتواريخ السندات المرحّلة صارت على يوم فاتورتها بدل أيلول المطلق (المسودات/الملغى بقي ترتيبها) |
| `Services\Mock\MockSupplierPaymentsData.cs` | ✅ منتهٍ `T-63` | السندان `2003`/`2004` كانا يدّعيان سداد فاتورة محددة بينما `PurchaseInvoiceId = null` ⇒ النص صار «دفعة على الحساب» (والأرصدة تتسق: الخدمة تطرح المدفوعات من متبقي الفواتير المرحّلة) |
| `Services\Interfaces\IStocktakingDesktopService.cs` | ✅ منتهٍ `T-62` | تعليق `CancelAsync` صار يطابق الكود: المسودة تُلغى بلا حركة · المرحّل يُعكس بحارس كفاية · الملغي يُرفض |

**حكم المراجعة المستقلة (`code-reviewer` على دلتا الموجة):** **مقبول** — صفر Blocker · صفر Major · ملاحظتان صغيرتان أُغلقتا (هذا السطر يُثبت الحكم ولا يستبق المراجعة؛ سطر «نتيجة المراجعة: صفر Blocker» السابق أُعيد صياغته). ما تبقّى مفتوحاً مسجَّل في §10.6.2 (`T-52` قرار مالك · `T-54` مراقَب · `T-56` مجمَّد بالعقد · **`T-64`** نص «وضع التجربة» المكرّر في 4 شاشات = قرار مالك `D-08`) وفي §10.2 (`T-65` · `T-66` · `T-67` مجمَّدة بالباك-إند).

### 11.16 الموجة 14 — الشاشات الخمس المُبلَّغ عنها «0 بايت» + صنف عيب موزَّع على 9 شاشات (7 وكلاء)

> **سبب الموجة:** المالك أبلغ أن خمسة ملفات `frm*.cs` «**0 بايت — الصنف بلا code-behind**». **القياس أثبت أن البلاغ غير صحيح من أوله إلى آخره:** صفر ملف `.cs` بحجم 0 في `SalesSystem.Desktop`، والملفات الخمسة كاملة بمصمّماتها. الموجة صارت **مراجعة عيوب حقيقية** لا إعادة إنشاء — وهذا هو الدرس الأول: **البلاغ دعوى تُقاس قبل أن تُنفَّذ**.

**القياس الأول (قبل أي تعديل):**

| الملف | الحجم | الأسطر | المصمّم | الصف |
|---|---|---|---|---|
| `frmAccounts.cs` | 53,757 B | 1,222 | 40,695 B / 969 L | ✅ كامل |
| `frmJournalEntries.cs` | 70,735 B | 1,695 | 54,526 B / 1,241 L | ✅ كامل |
| `frmCashBoxMaster.cs` | 42,713 B | 969 | 29,169 B / 697 L + `.resx` | ✅ كامل |
| `frmCustomerMaster.cs` | 47,460 B | 1,086 | 38,843 B / 909 L + `.resx` | ✅ كامل |
| `frmEmployeeMaster.cs` | 46,449 B | 1,034 | 31,951 B / 742 L + `.resx` | ✅ كامل |

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Screens\Customers\Forms\frmCustomerMaster.cs` | ✅ منتهٍ `T-68`/`T-70` | `_customersById.Clear()` نُقل **داخل** فرع النجاح (كان قبله) ⇒ أُغلق **تعطيل صامت لعميل حيّ**؛ وحُذفت `_IsTextInputFocused()` المحلية (قائمتها اليدوية **تُسقط `txtId`** وهو `ReadOnly`/`TabStop=false` لكنه يقبل التركيز بالفأرة) واستُهلك بيت `KeyboardHelper` |
| `Screens\Employees\Forms\frmEmployeeMaster.cs` | ✅ منتهٍ `T-68` | نفس المسح قبل التحقق (أثره: أزرار معكوسة `RULE-N92` — «حذف» معطّل و«إعادة تنشيط» مفعّل على موظف نشط — لا تعطيل صامت لأن `_BuildEmployeeDto` يقرأ `IsActive` من `_selected`) |
| `Screens\Accounting\Forms\frmJournalEntries.cs` | ✅ منتهٍ `T-69`/`T-70` | `cbStatusFilter.DisplayMember` أُضيف — كانت القائمة تُعرض `StatusOption { Value = 0, Text = كل الحالات }` بدل النص العربي؛ والحارس الميت `ActiveControl is TextBoxBase` استُبدل ببيت `KeyboardHelper` |
| `Screens\Accounting\Forms\frmAccounts.cs` | ✅ منتهٍ `T-70` | نفس الحارس الميت في `_HandleDeleteShortcut` ⇒ كان `Delete` أثناء الكتابة يفتح **تأكيد تعطيل الحساب** |
| `Screens\CashBoxes\Forms\frmCashBoxMaster.cs` | ✅ منتهٍ `T-68` | مسح الكاش قبل التحقق ⇒ أزرار معكوسة + رسالة «تعذر العثور على بيانات الصندوق المحدد» على سجل معروض (لا تعطيل صامت: `UpdateCashBoxRequest` لا يحمل `IsActive`) |
| `Screens\Products\Forms\frmProductMaster.cs` | ✅ منتهٍ `T-68` | نفس العيب — أثره **إعادة تنشيط صامتة** لصنف معطّل (احتياط `IsActive: true`) لا تعطيل |
| `Screens\Categories\Forms\frmCategoryMaster.cs` | ✅ منتهٍ `T-68` | **تعطيل صامت مُثبَت** لمجموعة نشطة برسالة «تم تعديل المجموعة بنجاح» |
| `Screens\Units\Forms\frmUnitMaster.cs` | ✅ منتهٍ `T-68` | **تعطيل صامت مُثبَت** لوحدة نشطة |
| `Screens\Suppliers\Forms\frmSupplierMaster.cs` | ✅ منتهٍ `T-68` | تعطيل صامت **+ محو `CategoryId`** المحفوظ عند التعديل |
| `Screens\Warehouses\Forms\frmWarehouseMaster.cs` | ✅ منتهٍ `T-68` | أزرار معكوسة + حجب إعادة التنشيط برسالة خاطئة (لا تعطيل: احتياط `IsActive: true`) |
| `Screens\Users\Forms\frmUserMaster.cs` | ✅ منتهٍ `T-68` | **محو `DefaultCashBoxId`** المحفوظ عند التعديل + أزرار معكوسة |
| `Helpers\KeyboardHelper.cs` | ✅ منتهٍ `T-70`/`T-71` | عضو عام جديد `IsTextInputFocused(Form?)` فوق `FindFocusedControl` الخاص القائم (TextBoxBase · NumericUpDown · ComboBox · DateTimePicker · محرِّر خلية شبكة) — **بيت واحد** بدل 11 نسخة · و`WireGlobalShortcuts` لم يعد يبتلع `Delete` عندما يكون التركيز في حقل إدخال (كان `Delete` **لا يحذف حرفاً في أي حقل** بكل شاشة تسجّله، بينما حارس الشاشة موجود أصلاً لهذا الغرض) |

**صنف العيب (blast radius) — قيس قبل الإغلاق:** العيب نفسه (`Clear()` قبل `IsSuccess`) وُجد في **9 شاشات** لا في واحدة، و`ActiveControl` الميت في **شاشتين** مع **10** نسخ محلية صحيحة-لكن-مكرّرة للحارس الصحيح. الشاشتان الخارجيتان عن نطاق الموجة (`frmSupplierMaster` · `frmWarehouseMaster` · `frmUserMaster` …) أُصلحتا في **دفعتين متطابقتين** (4 + 3) احتراماً لسقف `RULE-N157`، و`frmAccounts` كان سليماً أصلاً (يتحقق ويعود قبل المسح).

**حكم المراجعة المستقلة:** **مقبول مع ملاحظات** — صفر Blocker · صفر Major · **6 Minors** لم يستوجب أيّها تعديل كوداً في هذه الموجة:
`frmListReturns` تغيّر سلوك `Delete` داخل حقل التصفية من «مسح التصفية» إلى «حذف حرف» (اتجاهه صحيح ومُدرَج في قائمة الاختبار) · `frmCalculator` حقل العرض `ReadOnly` لم يعد `Delete` عليه ينفّذ «مسح» (حالة نادرة، مقبولة) · حراسات الشاشات الثلاث صارت **دفاعاً ثانياً** بعد إعفاء البيت · لا حارس `IsDisposed` في العضو الجديد · **8 نسخ محلية باقية** = `T-73` · عند فشل التحميل تبقى الشبكة فارغة بينما الذاكرة تحتفظ بالقديم (لا خطر مالي).

**البناء بعد الموجة:** `dotnet build .\SalesSystem.slnx --no-incremental` ⇒ **0 أخطاء / 118 تحذيراً** (الرقم الكامل المقارن — البناء التزايدي يبلّغ 96 فقط لأنه يعيد تحذيرات المشاريع المُعاد ترجمتها وحده).

**تحديث سجل الوكلاء (`.opencode/agents/`):** أُصلح تناقض داخلي في برومبت الـOrchestrator (§5 يسمح لـ`ui-agent` بكتابة `Helpers/**` بينما §10.1 يمنعه مطلقاً ⇒ صار «شريحة البيت المشترك» صريحة)، وأُضيفت خطوة **تحقق من الدعوى** (خطوة 0) وخطوة **قياس نصف قطر العيب** (9b/9c)، وأربعة فخاخ مقيسة جديدة، وفخّ عدم مقارنة أعداد التحذيرات بين بناء تزايدي وكامل. و`code-reviewer` أُعيد بناؤه: كان يطلب `MessageBox.Show` صريحاً في معالجات `async void` (مخالف لبيت `clsDialogs`)، وكان يفتقد فحص النطاق، والدعوى، والسبب الجذري، ونصف القطر.

### 11.17 الموجة 15 — سجل بناء المالك: 8 أخطاء من **نسخة ثانية قديمة** + 32 تحذيراً حقيقياً أُغلقت (4 وكلاء + مراجعة واحدة)

> **سبب الموجة:** المالك لصق — **مرتين** — سجل بناء فيه **8 أخطاء** و4 تحذيرات، وكل مساراته `C:\Users\ALlahabi\Desktop\Sales Management System\...` لا مسار المستودع. القياس قبل أي تعديل (خطوة 0 في قانون الـOrchestrator: **الدعوى تُقاس ولا تُنفَّذ**).

**القياس الأول — من أين جاء السجل فعلاً؟**

| البند | القياس |
|---|---|
| شجرة المستودع (OneDrive) | `dotnet build --no-incremental` ⇒ **0 أخطاء / 118 تحذيراً** — الأخطاء الثمانية غير موجودة فيها |
| مجلد البناء في السجل | **نسخة git ثانية** في `C:\Users\ALlahabi\Desktop\...` — `HEAD c5c872c` (**3 إيداعات خلف master**)، **169** ملفاً معدَّلاً + **10** غير متعقَّبة، و`git diff origin/master` = **195** مساراً |
| سبب الأخطاء الثمانية | **ملفان** موجودان في المستودع وغائبان هناك — `Helpers\DocumentTextHelper.cs` و`Helpers\NumericInputHelper.cs` — و`clsValidation.cs` هناك **بلا `EndOfDay`** أصلاً. النسخ اليدوي للملفات المعدَّلة لا يجلب الملفات **الجديدة**، فبقي كل شاشة تستدعي بيراً غير موجود |
| الحكم | **الأخطاء الثمانية لم تكن في المستودع قط**؛ والتحذيرات الأربعة **كانت حقيقية فيه** ⇒ عُوملت على مستوى **الصنف** لا الشاشة (`RULE-N155`) |

| خطأ السجل | سببه المقيس |
|---|---|
| `CS0103 DocumentTextHelper` ×3 (`frmJournalEntries:250,1693`) + `CS8130` ×2 (السطر 250 نفسه) | `Helpers\DocumentTextHelper.cs` غائب في النسخة الثانية |
| `CS0103 NumericInputHelper` ×2 (`frmEmployeeMaster:521` · `frmCustomerMaster:667`) | `Helpers\NumericInputHelper.cs` غائب في النسخة الثانية |
| `CS1501 EndOfDay` (`frmJournalEntries:615`) | نسخة `clsValidation` هناك أقدم — لا تحتوي العضو |

**ما أُصلح فعلاً في المستودع — الأصناف الخمسة كاملةً (14 ملفاً، 32 مثيلاً):**

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `DataAccess\Printing\PrintService.cs` | ✅ `T-75` | حُذف `using SalesSystem.DataAccess.Printing;` المكرّر (السطر 9) — **استثناء (ج) `RULE-N178`**: تصحيح نظافة بلا سلوك، داخل باك-إند مجمَّد، وهو المساس الوحيد بـ`DataAccess/**` |
| `Services\Interfaces\IInventoryDesktopService.cs` | ✅ `T-75` | حُذف `using SalesSystem.DTO;` المكرّر (السطر 5) |
| `Screens\Purchases\Forms\frmPurchaseItem.cs` | ✅ `T-76` | `ProductName` ⇒ `SelectedProductName` (كان يخفي `Control.ProductName`) — والقياس: **صفر** مستهلك للخاصية في كل الحل |
| `Screens\Users\Forms\frmUserMaster.cs` | ✅ `T-77` | حُذف `_isBindingLookups` (يُكتب 3 مرات ولا يُقرأ) وفُكّت **3** كتل `try/finally` إلى عباراتها — لا معالج `cmbBranch_SelectedIndexChanged` موجود أصلاً ⇒ الحارس نيّة ميتة، والحذف صفر أثر سلوكي |
| `Screens\Inventory\Forms\frmInventoryFifo.cs` | ✅ `T-78` | `() => _ = _ChooseProductAsync()` — اصطلاح الملف نفسه (372 · 388) |
| **9** مصمّمات تقارير (`frmAccountStatementReport` · `frmDailySalesReport` · `frmProductProfitabilityReport` · `frmProfitByCustomerReport` · `frmReturnsReport` · `frmSalesByCategoryReport` · `frmSalesByCustomerReport` · `frmSalesByProductReport` · `frmSalesReport`) | ✅ `T-79` | حُذفت **27** تصريحاً ميتاً (`dataGridViewCellStyle1/2/3` × 9) — والأنماط الحقيقية `4/5/6` داخل `InitializeComponent` لم تُمسّ، و`RULE-N134` سليم |

**صنف العيب (blast radius) — مقيس قبل الإغلاق:** التحذيرات الأربعة في سجل المالك كانت في الحقيقة **32 مثيلاً** على **14 ملفاً**: `CS0169` **27** · `CS0105` **2** · `CS0108` **1** · `CS0414` **1** · `CS4014` **1**. أُغلقت **كلها**، ولم يُترك أي مثيل. **لم تُمسّ** عائلة nullable (`CS8602` 43 · `CS8604` 14 · `CS8618` 13 · `CS8601` 2 · `CS8605` 1 · `CS8600` 1 = **74**) لأن إصلاحها يضيف فحوص `null` فعلية ⇒ سلوك لا تنظيف، و`CS8981` (**2**) داخل `Migrations/**` ⇒ مجمَّد `RULE-N177`.

**حكم المراجعة المستقلة (`code-reviewer` على دلتا الموجة):** **مقبول مع ملاحظات** — **0 Blocker · 0 Major · 2 Minors**، وكلتاهما وُثِّقت لا تُصلَح بالكود هنا:
(1) المساس بـ`DataAccess/**` تدوينه كاستثناء (ج) `RULE-N178` — دُوِّن أعلاه وفي تقرير التسليم · (2) تعليق `frmPurchaseItem` يصف «النموذج الأب» **ولا أب له في الحل** ⇒ صار `T-74`/`D-10`.

**قياس إضافي مقيس في هذه الموجة — `T-74`:** بحث حدودي (`(?<![A-Za-z0-9_])frmX(?![A-Za-z0-9_])`) على كل `*.cs` ما عدا ملفَّي النموذج وتصريح `Program.cs` ⇒ **14** نموذجاً مسجَّلاً في DI **لا يفتحه أحد**: `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseOtherDetails` · `frmPurchaseInvoicePrint` · `frmCustomerSelection` · `frmCustomerStatement` · `frmSupplierSelection` · `frmSupplierStatement` · `frmStocktakingPrint` · `frmStocktakingPrintCard` · `frmStocktakingPrintAdjustment` · `frmWarehouseTransferPrint` — قرار المالك `D-10` (فتح أو حذف، `RULE-N102`).

**البناء بعد الموجة:** `dotnet build .\SalesSystem.slnx --no-incremental` ⇒ **0 أخطاء / 86 تحذيراً** (كان 118) — والأصناف الخمسة **صفر**.

**مزامنة نسخة المالك الثانية:** أُخِذت لقطة قابلة للاسترجاع (`git stash push -u` ⇒ `stash@{0}: pre-sync snapshot 2026-11-16 by orchestrator`) ثم `git reset --hard origin/master` ⇒ النسخة صارت مطابقة للمستودع وبناؤها **0 أخطاء / 118 تحذيراً** (ثم إلى `86` بعد هذه الموجة). الاسترجاع إن أراد: `git stash pop`.

### 11.18 الموجة 16 — تغذية الحقل المطلوب: أيقونة ✗ **و**لون التحديد معاً (`frmProductMaster` أولاً بقرار المالك)

> **طلب المالك (2026-11-16):** «تحسين تجربة الاستخدام بلون التحديد للحقل المطلوب: عندما يترك المستخدم الحقل بلا إدخال تظهر أيقونة مثل `errorProvider`، وتُحدَّث مع كل `Enter`… **جرّب `frmProductMaster` أولاً وأراجع النتيجة**، وسجّل هذا السلوك في `AGENTS.md`». الهدف المعلن: أن يفهم المستخدم **في اللحظة نفسها** أن هناك خطأ **وأن الشاشة ما زالت تفاعلية** — لذا **إشارتان معاً لا واحدة**.

**القياس قبل الكتابة (خطوة 0 — الدعوى تُقاس):**

| البند | القياس |
|---|---|
| هل يوجد بيت لهذا السلوك؟ | **لا** — `RequiredFieldHelper`/`InvalidFieldHelper`/`ValidationFeedback` **غير موجودة**؛ `clsValidation` تُعيد `bool` فقط، و`clsInputFeedback` تلميحات فقط، و`KeyboardHelper` لون التركيز فقط (`RULE-N170` وجد فراغاً حقيقياً) |
| هل الشاشة تملك الأيقونة؟ | **نعم** — `errorProvider` **مُعلَن في مصمّمها** (`L84/825/932`) ومستخدَم في 13 موضعاً… **لكن عند الحفظ فقط** |
| هل اللون موحّد أصلاً؟ | **نعم** — **45** شاشة تمرّر `ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)` إلى `WireFocusHighlight`، و**صفر** شاشة تكتب `(255,255,180)` في code-behind |
| قطر العيب (blast radius) | **28 شاشة · 199 موضع `errorProvider.SetError`** — كلها تتحقق عند الحفظ ولا واحدة تُغذّي عند المغادرة ⇒ **صنف** لا شاشة |

| الملف | الحكم | ما نُفِّذ |
|---|---|---|
| `Helpers\RequiredFieldHelper.cs` | **جديد** | بيت واحد: `RequiredField(Control, string Message, Func<bool>? When)` + `Attach` · `Validate` · `Clear`. التقييم يُضبط فيه **`SetError` والخلفية في نفس اللحظة**، واللون يُقرأ **حيّاً** `ThemePalette.Get(SelectionRowColor)` عند كل تقييم، والخلفية تُستعاد إلى **لون المصمّم الملتقَط** لا إلى أبيض، والحالة في `ConditionalWeakTable` (لا تسريب نماذج — نفس نمط `KeyboardHelper.MarkWired`)، وكل معالج داخل `Safe(...)` فلا استثناء يفلت، و`IsEmpty` مصدر وحيد لحكم «فارغ» (نص/`MaskedTextBox` · `ComboBox` بتحويل دفاعي **بلا `(int)SelectedValue`** فيُحترم `short`) |
| `Screens\Products\Forms\frmProductMaster.cs` | **4 إدخالات فقط** | (1) `Attach` بعد `WireModeEnter` — **بعد** `WireFocusHighlight` لأن ترتيب اشتراك `Leave` يحسم مَن يرسم الخلفية أخيراً · (2) `Clear` في `_ApplyModeState` عند مغادرة وضع التحرير · (3) `Clear` بعد `errorProvider.Clear()` في `_ResetForm` · (4) `Validate` أول سطر في `_ValidateForm` ليظهر اللون أيضاً عند محاولة الحفظ |
| `AGENTS.md` | **§2.38 جديد** | `RULE-N180` (الإشارتان معاً في التقييم نفسه) · `RULE-N181` (البيت الواحد) · `RULE-N182` (لا لون/عنصر جديد + لون المصمّم هو المستعاد) · `RULE-N183` (المعطّل لا يُوسَم + ترتيب الاستدعاء الملزم) + صف في §2.20 (18) و§2.27 و§2.30 و§2.21-17 |

**التعامل مع تظليل الحقل النشط (الفخّ الحقيقي في هذه الموجة):** `KeyboardHelper.WireFocusHighlight` يستعيد الخلفية في `Leave` أيضاً، فلولا الترتيب لمسح لون الرفض. الحلّ: `Attach` بعد `WireFocusHighlight` (WinForms ينفّذ معالجات الحدث نفسه بترتيب الاشتراك) — والشرط مكتوب في **XML doc للـ`Attach`** وفي **تعليق موقع الاستدعاء** حتى لا يُعاد ترتيبه لاحقاً. وأُضيف `EnabledChanged` لأن `KeyboardHelper.WireDisabledVisuals` يلتقط «اللون الطبيعي» عند التعطيل: بلا هذا المعالج قد يُلتقَط **الأصفر** كأنه لون طبيعي فيبقى بعد إعادة التفعيل.

**البناء:** `dotnet build .\SalesSystem.slnx --no-incremental` ⇒ **0 أخطاء / 86 تحذيراً** — **مطابق تماماً لخط الأساس**: لم يُضف الملف الجديد تحذيراً واحداً (المثيلات المميَّزة **76** كما هي؛ `CS8618` عند `frmProductMaster.cs:76` قديم خاص بمنشئ المصمّم).

#### 11.18.1 بوابة الشاشة التجريبية — عيوب الشاشة النموذج `PILOT-1…4` (قرار المالك 2026-11-16)

> **قرار المالك:** «قبل أن نطبّق أي كود على كل شاشات النظام يجب أن نختبره في مكان واحد — شاشة واحدة — ونتأكد أنه يعمل 100% بلا مشاكل». صار قانوناً في `AGENTS.md` §2.39 (`RULE-N184`…`N187`)، ومعه تُصبح **`frmProductMaster` الشاشة التجريبية** و**بقية الـ28 شاشة ممنوعة المساس** حتى الشهادة.

| # | العيب المكتشف في الشاشة التجريبية | البرهان المقيس | الإصلاح في الشاشة نفسها |
|---|---|---|---|
| **PILOT-1** | فحص `numReorderLevel.Value < 0` داخل التحقق **لا يمكن أن يتحقق أبداً** وسجلّ التحذير مرتبط به | `numReorderLevel.Minimum` غير مُعلَن في المصمّم (**0** تصريح) وغير مضبوط في الكود (**0**) ⇒ الافتراضي `0` | حُذف الفحص الميت (صفر أثر سلوكي لأن الشرط مستحيل) |
| **PILOT-2** | **حكم «مطلوب» في ثلاثة بيوت على شاشة واحدة:** `_RequiredFieldsValid()` + `_FocusFirstMissingField()` + `RequiredFieldHelper.IsEmpty` — و`RULE-N172` يحرّم **نسختين** لسلوك تحقّقي | 3 تعريفات بدلالة متطابقة: `L163/L178` + `IsEmpty` في الـHelper | أُضيف `RequiredFieldHelper.FirstMissing(form)` وحُذفت الدالتان الخاصّتان ⇒ **الحكم في بيت واحد**، والشاشة تستهلك `Validate` + `FirstMissing` فقط |
| **PILOT-3** | حارس `cmbCategory.SelectedValue is not int … return;` **بعد** التحقق = غير قابل للتنفيذ | `_ValidateForm` تُرجع `false` عند عدم صلاحية المجموعة، والففح يقع بعدها | حُذف الحارس وبقيت **قراءة القيمة** التي يستهلكها الطلب |
| **PILOT-4** | **`_ValidateForm` كلها ميتة** ⇒ تحذير «يرجى تصحيح الأخطاء الموضحة قبل المتابعة.» **لم يظهر قط لأي مستخدم** (كان ميتاً **قبل** هذه الموجة أيضاً: `_RequiredFieldsValid` كانت تفحص الشروط الثلاثة نفسها أولاً) | مستدعٍ واحد بعد بوابة الحقول المطلوبة، وجسمها بعد حذف `PILOT-1` لا يحوي إلا الشروط الثلاثة نفسها ⇒ `isValid` لا يصير `false` أبداً | حُذفت الطريقة ونداؤها، وبقي تعليق واحد يصرّح أن التحقق في البيت الواحد |
| **PILOT-5** | `errorProvider.SetError(cmbMainUnit, "")` متبقٍّ في `_ResetForm` بعد `errorProvider.Clear()` وبعد `RequiredFieldHelper.Clear(this)` — نسخة ثانية من مسح الأيقونة داخل الشاشة | مراجعة الدلتا: `RULE-N181` تحصر `SetError` في البيت، و`RULE-N185`(5) تعدّ النسخة غير القابلة للتنفيذ عيباً | حُذف السطر ⇒ **`errorProvider.SetError` في الشاشة التجريبية = 0**، و`errorProvider.` كله = `Clear()` واحد + وسيط `Attach` |

**قياس ما بعد إصلاح الشاشة التجريبية:** `_RequiredFieldsValid` = **0** · `_FocusFirstMissingField` = **0** · `_ValidateForm` = **0** · `SetError` برسالة عربية داخل الشاشة = **0** · `RequiredFieldHelper` = **5** مواضع استهلاك (`Attach` · `Validate` · `FirstMissing` · `Clear`×2) · الواجهة العامة للـHelper = `Attach` · `Validate` · `FirstMissing` · `Clear` (+`RequiredField`) ولا أكثر.

#### 11.18.2 قرارا المالك 2026-11-16 («غيّر اللون إلى الأحمر» ثم «مثل كلمة المرور في `frmLogin`») — الحالة النهائية: ومضة رفض واحدة في النظام

> **مسار القرار (ثلاث جولات، مسجَّل بصراحة):** **(1)** لون الرفض كان **نفس** `SelectionRowColor` (الأصفر) المستخدم لتظليل الحقل النشط ⇒ «حقل مركَّز» و«حقل به خطأ» غير قابلين للتمييز ⇒ قرار «غيّر اللون إلى الأحمر»، فنُفِّذ **حمر ثابت** بمفتاح لوحة جديد. **(2)** المالك وجد أن السلوك المطلوب هو **طريقة حقل كلمة المرور في `frmLogin`**، ثم أبلغ أن الومضة يجب أن تكون **مع كل Enter** وتعود للون الأساسي. **(3)** القياس أثبت أن **`clsInputFeedback.ShowRejectTooltip`** هو البيت القائم فعلاً (98 موضعاً تنفيذياً في 34 ملفاً: 32 شاشة + `MoneyTextBox` + `RequiredFieldHelper`): ومضة `RejectBackground` **200 ms** + تلميح منبثق بالرسالة + صوت + **استعادة لون المصمّم تلقائياً** — أي أن «الأحمر الثابت» كان **تصميماً خاطئاً من عندنا**، واللون الذي أضفناه كان **نسخة ثانية** من لون الرفض القائم (`GreenThemeHelper.RejectBackground` = `255,220,220`).

| البند | ما نُفِّذ (الحالة النهائية) |
|---|---|
| **بيت اللون الواحد** | المفتاح صار **`ThemePalette.RejectBackgroundColor`** = `"rejectBackgroundColor"` · افتراضيه **`Color.FromArgb(255, 220, 220)` = `#FFDCDC`** — **نفس** القيمة الحرفية القديمة ⇒ صفر تغيّر بصري في الـ98 موقعاً · داخل «حالات العرض» ⇒ اللوحة تبقى **24 لوناً** والصف يظهر تلقائياً في `frmChangeSystemColorsSettings` باسم «خلفية الرفض والخطأ» · قاعدة تباين `Add(RejectBackgroundColor, GridCellTextColor, BodyTextMinimumContrast)` |
| **`GreenThemeHelper`** | `RejectBackground` صار **قراءة حيّة** من المفتاح (`=> ThemePalette.Get(...)`) بدل ثابت مكتوب ⇒ تغيير الصف من شاشة الألوان يسري على **كل** مواضع الرفض في النظام (أثر واسع، مقصود، ويُختبر مرة واحدة بعد الشهادة) |
| **الـHelper: صفر لون** | حُذف من `RequiredFieldHelper` **كل** طلاء خلفية: `OnEnter` · `RestoreOriginal` · `OriginalBack` · وكتابات `BackColor` في `Evaluate`/`OnValueChanged`/`OnEnabledChanged` ⇒ **`BackColor` = 0 و`ThemePalette.` = 0** داخله. السلوك: `SetError` (أيقونة + رسالة) **+** `ShowRejectTooltip` (ومضة + تلميح + صوت) |
| **`PILOT-6`** (عيب، انتهى مع إلغاء الأحمر الثابت) | الحقل الذي يُصحَّح **وهو مركَّز** كان يبقى أحمر حتى المغادرة — صار غير ذي موضوع بعد أن حُلّت الخلفية، ولم يبقَ منه أي كود |
| **`PILOT-7`** (توافق رجعي — باقٍ ومهم) | `ValidateAll` يطلب **كل** مفتاح ⇒ أي مفتاح جديد كان سيُسقط **كل** لوحة محفوظة عند الإقلاع. الحل: `ThemePalette.TryApplyStored` — يبني الافتراضيات ثم يُركّب المحفوظ (المفاتيح المعروفة فقط، والقيم غير القابلة للقراءة **تُتجاهل**) ثم يستدعي `TryApplyAll` الذرّي · `Program.cs` يستخدمه. **`ValidateAll`/`TryApplyAll` لم يتغيّرا** (`RULE-N167`) |
| **`PILOT-8`** (سلوك Enter) | كان `OnEnter` يصبغ **أي** حقل فارغ بمجرد الدخول إليه **بلا أيقونة**، وتتراكم حقول حمراء ⇒ صار: **الدخول وحده لا يُوسم**، والوسم عند المغادرة أو الحفظ، و**علامة واحدة فقط** تنتقل مع Enter (`UnmarkAllExcept`) فيعود السابق إلى لون المصمّم · ومحاولة الحفظ تُومض/تُصفّر **مرة واحدة** لأول ناقص فقط |
| **`PILOT-9`** (عيب حقيقي في تصميمنا) | **لونان للرفض في النظام** (مفتاحنا `#FFCDD2` مقابل `RejectBackground` `#FFDCDC`) + **رسالة غير ظاهرة** (الأيقونة وحدها) ⇒ أُلغيت النسخة الثانية، وصار الرفض يستهلك البيت القائم **بنصّه** ليظهر مثل `frmLogin`: أيقونة + رسالة + ومضة + صوت |
| **حدود معروفة** | (أ) `frmLogin` يعرض الرسالة أيضاً في **ليبل داخل الشاشة** (`lblError`)؛ الشاشة التجريبية لا تملك ليبل ⇒ الرسالة تظهر في **التلميح المنبثق + تلميح الأيقونة** فقط — تُضاف لو قرر المالك إضافة ليبل (عنصر دائم يُعلَن في المصمّم `RULE-N145`). (ب) إعادة المؤشر إلى حقل **خلال 200 مللي ثانية** من الومضة قد تُعيد لون المصمّم فوق الأصفر لحظياً — سلوك قائم في كل مواقع الرفض بالـSystem |

**حالة البوابة:** 🔄 **غير مشهودة** — الشرط السادس من `RULE-N185` (تأكيد المالك 100%) معلّق. **صفر شاشة أخرى مُستهدَفة**، و`T-81` باقٍ محجوباً.

---

### 11.19 موجة 2026-11-16 — لوحة الألوان المحايدة (شاشة تجريبية + بروتوكول التغيير من مكان واحد)

**قرار المالك:** الشاشة كانت كلها بدرجات أخضر متقاربة (خلفية · حاويات · صناديق · أشرطة) فاندمجت العناصر وضاع التباين وأُجهدت العين. **الحل المعتمد:** لوحة محايدة مهنية + **بروتوكول تغيير من مكان واحد** (`§2.40` · `RULE-N188`…`RULE-N190`) حتى لا يكلّف أي تغيير لاحق مسّ 95 مصمّماً.

| الملف | الحالة | ما نُفِّذ |
|---|---|---|
| `Helpers/ThemePalette.cs` | ✅ | **13** قيمة افتراضية جديدة (خلفية `240,242,245` · شريط `16,150,96` · نص مميّز `33,37,41` · صناديق/أشرطة/شبكة أبيض · رؤوس `52,58,64` · خطوط `222,226,230` · متبادلة `248,249,250` · تحديد `#FFF3CD` · معطّل `233,236,239`/`108,117,125` · رفض `#F8D7DA`) + **`RetiredDefaults` (13 زوجاً)** + `internal IsRetiredDefault(key, color)` |
| `Helpers/GreenThemeHelper.cs` | ✅ | استهلاك `IsRetiredDefault` في `IsThemeColor` (داخل `ContainerRoleKeys`) و`IsPaletteOwnedColor` (**موضعان**) — فالمصمّم القديم يبقى «لون ثيم» يُعاد تلوينه؛ + تصحيح تعليق `TextStrong` |
| `Helpers/KeyboardHelper.cs` | ✅ | تعليقان (228 · 340) كانا يذكران `#FFFFB4` كافتراضي — صارا `#FFF3CD` (`RULE-N174`: التوثيق يتبع القيمة) |
| `Screens/Products/Forms/frmProductMaster.Designer.cs` | ✅ | **11** قيمة لونية نُسِّخت إلى الافتراضيات الجديدة — وفُصِل الدوران المتشابهان: `lblMode` → **زمردي** `16,150,96`، ونمط رؤوس الأعمدة `dataGridViewCellStyle5` → **فحمي** `52,58,64` · `grpMainUnit` كان أخضر مصمّماً قديماً `200,230,200` → أبيض |
| **بقية المصمّمات** | ⚠️ **94 ملفاً** | لم تُمسّ بقرار المالك (شاشة تجريبية أولاً). **وقت التشغيل صحيح فيها كلها بفضل `RetiredDefaults`**، والناقص **معاينة المصمّم فقط** (`RULE-N134`/`RULE-N140`) — تُنفَّذ في **3 موجات × 4 وكلاء × ≤8 ملفات** بعد شهادة المالك (`§2.40`). **القياس: 2,553 موضعاً في 95 ملفاً** |
| `ColorSuggestionHelper.cs` | ✅ بلا مسّ | معجم ألوان **لا** افتراضيات — «أخضر زمردي» `16,150,96` و«فحمي» `33,37,41` موجودان فيه أصلاً فاقتراحاته متسقة مع اللوحة الجديدة |
| `GreenThemeHelper.Status*` / `InactiveRow*` | ✅ بلا مسّ | ألوان **حالة دلالية** لا تصميم (لا تُعلَن في المصمّمات ولا تتغيّر مع اللوحة) |

**قياسات هذه الموجة (أمر الإنتاج مذكور):**

| القياس | الأمر | النتيجة |
|---|---|---|
| بقايا القيم المتقاعدة في الشاشة التجريبية | `Select-String '170, 220, 170\|235, 240, 235\|220, 240, 220\|205, 225, 205\|245, 252, 245\|255, 255, 180\|14, 82, 52\|200, 230, 200\|14, 165, 103'` | **0** |
| القيمتان الجديدتان لشريط العنوان/رؤوس الأعمدة | `Select-String '16, 150, 96'` = 1 · `'52, 58, 64'` = 2 | ✅ مطابق للمتوقّع |
| عدد أسطر مصمّم الشاشة التجريبية | `Get-Content … .Count` | **938** (بلا تغيير) |
| نطاق الملفات المعدّلة | `git status --short` | **4** ملفات، كلها في الموجة |
| التباين بعد التغيير (WCAG) | حساب مباشر | أبيض/زمردي **3.78** (3.0) · مميّز/رمادي **13.75** (4.5) · خلية/أبيض **15.43** (4.5) · تحديد **13.92** (4.5) · رفض **11.55** (4.5) · معطّل **3.95** (2.0) · رأس/أبيض **11.51** (3.0) — **كلها فوق الحدود، و`ContrastRules` لم يُمسّ** |
| البناء | `dotnet build .\SalesSystem.slnx -p:ArtifactsPath=…` | **0 Errors** · 86 Warnings (**بناء جزئي — غير قابل للمقارنة بـ118**) |

**بنود مفتوحة من هذه الموجة (لا تُنسى):**

| # | البند | الحجم | المالك المقترح |
|---|---|---|---|
| `PAL-1` | مزامنة **94** ملف `*.Designer.cs` مع الافتراضيات (`RULE-N134`/`N140`) — 2,553 موضعاً، منها 429 لِلَون واحد بدورين | **L** | 3 موجات × 4 `ui-agent`s (≤8 ملفات لكل وكيل) — بعد شهادة المالك |
| `PAL-2` | لوحة **محفوظة** في `SystemSettings` (وضع حقيقي) تتغلّب على الافتراضيات عند الإقلاع ⇒ المالك يضغط «استعادة الافتراضيات» مرة واحدة | — | إجراء مالك (موثَّق في §2.40) |
| `PAL-3` | `frmPurchaseOtherDetails`/`frmPurchaseItem`/`frmPurchaseDiscount` تحمل `(255,255,180)` خلفية **`Label`** (لا حاوية) ⇒ `Walk` لا يُعيد تلوين التسميات فتبقى بالأصفر القديم (`PAL-1` يعالجها لأنها من المصمّمات) | يُغلق مع `PAL-1` | `ui-agent` |
| `PAL-4` | تعليقات تذكر «الثيم الأخضر» كاسم تاريخي في `GreenThemeHelper` (6) · `ReportUiStandardizer` (7) · `ThemeHelper` (4) · `ReportNavigationHelper` (1) | **S** | تُترك عمداً: الاسم التاريخي مقصود (§2.19) — لا تُعدَّل |

---

### 11.20 الموجة 17 — تنقل `Enter` بترتيب العرض + بيت واحد لقيم التنقل الافتراضية (2026-11-16)

> **بلاغ المالك (نصّه):** «I test frmProductMaster and find one Problem here the Navation by Enter must move by ordering row by row and after move to last faild must go to the buttong depened on scrren Mode» — ثم: «but we can set Helper for change default Navigation or anything from one Place ... register this in Agents.md for any edit in the future for consume less time and less cost of dovlepment».
>
> **تنبيه ترقيم:** هذه الموجة بدأت بأرقام `RULE-N188…N190` ثم **أُعيد ترقيمها إلى `RULE-N191…N193`** بعد أن نشرت موجة اللوحة (§11.19) `RULE-N188…N190` لنفسها — لا تعارض الآن، والملفات تحمل الأرقام النهائية فقط.

| الملف | التغيير | التفصيل |
|---|---|---|
| `Helpers\KeyboardHelper.cs` | **واجهة ثانية + إصلاح بلوكر** | `WireEnterAsTab(Control scope, Func<Button?> primaryButton)` + `PreviewKeyDown`→`IsInputKey` (على الحقل **وأبنائه الداخلية**) · ترتيب العرض بإحداثيات الشاشة · تسليم آخر حقل لزر الوضع **تركيزاً بلا تنفيذ** · القائمة المنسدلة المفتوحة تملك `Enter` · `if (e.Handled) return` · حارسا توصيل مزدوج (`EnterNavigationWired` · `EnterInputKeyDeclared`) |
| `Helpers\NavigationOptions.cs` | **جديد — بيت واحد (H-08)** | `RULE-N192` كتطبيق مرجعي: الافتراضيات معلنة مرة واحدة، تُقرأ حيّة عند كل ضغطة، لا شاشة تقرأها أو تضبطها |
| `Screens\Products\Forms\frmProductMaster.cs` | **سطر واحد** | `WireEnterAsTab(pnlFields, _PrimaryActionButton)` — الشاشة التجريبية الوحيدة (`RULE-N184`) |
| `AGENTS.md` | **3 قواعد** | `RULE-N191` (§2.18) · `RULE-N192` (§2.30) · `RULE-N193` (§2.33) + تحديث §2.20-1 و§2.27 و§2.30 و§2.39 |

**`PILOT-10…13` — عيوب وُجدت في الشاشة التجريبية وأُصلحت فيها:**

| # | العيب | البرهان المقيس | الإصلاح |
|---|---|---|---|
| **PILOT-10** | **`Enter` لا يصل إلى `KeyDown` أصلاً** — فكل معالجات `Enter` في الشاشة (ومنها ترتيب التنقل) كانت **كوداً ميتاً**، و`Enter` ينقر زر الوضع مباشرة | مصدر إطار العمل (مقروء في هذه الجولة): `Control.PreProcessControlMessageInternal` يرفع `PreviewKeyDown` أولاً ثم `PreProcessMessage`→`ProcessDialogKey`؛ `Form.ProcessDialogKey` عند `Return` = `button.PerformClick()` ويعيد `true`؛ `TextBoxBase.IsInputKey` لا يعيد `true` لـ`Enter`؛ و`UpDownBase` يضع التركيز في `_upDownEdit` فيُرفع الحدث على الابن لا على `NumericUpDown` | `PreviewKeyDown` يُعلن `Enter` مفتاح إدخال على الحقل **وعلى أبنائه الداخلية** ⇒ يُرفع `KeyDown` ويُستهلك بـ`SuppressKeyPress` قبل أي معالجة حوارية (`RULE-N193`) |
| **PILOT-11** | الترتيب يتبع `TabIndex` **لا الصفوف المعروضة**: `grpMainUnit` يُزار ككتلة واحدة فتُقطع الصفوف | تسلسل `TabIndex` المقروء من المصمّم: `cmbCategory(1) txtName(2) txtForeignName(3) grpMainUnit(4) numReorderLevel(5) …` بينما `grpMainUnit` يشغل الصفوف الرأسية الوسطى | ترتيب العرض يُقاس بـ`PointToScreen` (صفاً صفاً، وفي RTL يمين→يسار) — والقياس حيّ عند كل ضغطة فيتبع وضع الشاشة |
| **PILOT-12** | بعد آخر حقل كان `Enter` **يدور إلى أول حقل** (`wrap: true`) فلا يبلغ زر الوضع أبداً | `SelectNextControl(..., wrap: true)` في الواجهة القديمة | التسليم لزر الوضع حسب `_PrimaryActionButton()` — **تركيز فقط بلا `PerformClick`** (`EnterExitsToModeButton`) |
| **PILOT-13** | معالجا `Enter` داخل الشاشة (`txtMainUnitBarcode` `RULE-N42` · `txtSearch` `RULE-N59`) كانا **ميتين** لنفس سبب `PILOT-10` | `KeyDown` لم يكن يُرفع قط عند وجود `AcceptButton` | صارا حيَّين — وأُضيف `if (e.Handled) return;` حتى لا يُنفَّذ معالج الشاشة **مع** نقل التركيز في الضغطة نفسها |

**القياسات (أُنتجت في تشغيل هذه الموجة — `RULE-N174`):**

| القياس | الطريقة | القيمة |
|---|---|---|
| مواضع `WireEnterAsTab` في `Screens/**` | `Select-String -Pattern 'KeyboardHelper\.WireEnterAsTab\('` | **59** = **58** أحادي الوسيط + **1** ثنائي (`frmProductMaster.cs:85`) |
| شاشات تضبط `AcceptButton` | `Select-String -Pattern 'AcceptButton\s*='` (بلا مصمّمات) | **40** شاشة · **59** موضع إسناد — كتلة `RULE-N193` المتأثرة |
| `NavigationOptions` داخل `Screens/**` | `Select-String` | **0** (وممنوع أن يتغيّر) |
| `Math.Max(8` داخل `KeyboardHelper` | `Select-String` | **0** (القيمة في البيت الواحد) |
| البناء | `dotnet build .\SalesSystem.slnx --no-incremental` | **0 أخطاء / 86 تحذيراً** · المثيَّلات المميّزة **76** = خط الأساس |

**حالة هذه الموجة:** 🔄 **الشاشة التجريبية غير مشهودة** — التعميم على الـ58 موضعاً الأخرى **محجوب** حتى تأكيد المالك 100% (`RULE-N184`/`RULE-N185`). وما ينتظر الشهادة معاً: `T-81` (28 شاشة · 187 موضع `SetError`) + تعميم واجهة `Enter` ثنائية الوسيط + `RULE-N193` على الشاشات الـ40.

**شرطان ملزمان في كل شاشة عند الترحيل (من مراجعة الكود لهذه الموجة):**
1. **إحدى الواجهتين لا الاثنتان:** الشاشة التي تنتقل إلى `WireEnterAsTab(scope, primaryButton)` **تحذف** استدعاءها الأحادي الوسيط، وإلا فالحارس يمنع الاشتراك المزدوج بصمت ويفوز **الأول**.
2. **كل معالج `Enter` خاص بالشاشة يضبط `e.Handled = true`** قبل عمله، وإلا نُفِّذ أمر الشاشة (باركود/بحث) **ونقل التركيز معاً** في الضغطة نفسها.

---

### 11.21 الموجة 18 — الحقل المطلوب الفارغ يحجب `Enter` ويُبقي المؤشر عليه (2026-11-16)

> **بلاغ المالك (نصّه):** «the Last note is required faild is empty prevent Enter Vavigation because the user press enter and the Input errror and message will show but the curcer aleady moved to the next faild».

**السبب المقيس:** `KeyboardHelper.MoveNextByDisplayOrder` ينقل التركيز **أولاً** (`.Focus()` على الحقل التالي)، وتقييم `RequiredFieldHelper` معلَّق على `Leave` — فيُطلق **بعد** مغادرة الحقل ⇒ الأيقونة والرسالة والومضة تظهر على حقل غادره المؤشر للتوّ.

| الملف | التغيير | التفصيل |
|---|---|---|
| `Helpers\RequiredFieldHelper.cs` | **عضو عام واحد (+58)** | `BlocksEnterNavigation(Control?)` — يطابق الحقل **ثم أسلافه** (العنصر المركّب يضع التركيز في ابنه الداخلي) على قواعد `Registry[form].Rules`، ثم يعيد `!Evaluate(...)` بالافتراضيات (`collapseOthers: true` · `flash: true`) — **نفس تقييم `Leave` حرفياً**، فلا حكم «فارغ» ثانٍ (`RULE-N172`). لا قاعدة / مُلغى / بلا نموذج / أي استثناء ⇒ `false` (فشل مفتوح). صفر لون وصفر `BackColor` داخله |
| `Helpers\KeyboardHelper.cs` | **استهلاك واحد (+9)** | داخل معالج `Enter` في `WireEnterDisplayOrder`: **بعد** `if (e.Handled) return` و**بعد** حارس `ComboBox { DroppedDown: true }` و**قبل** `MoveNextByDisplayOrder` ⇒ `e.Handled = true` + `e.SuppressKeyPress = true` والتركيز يبقى |
| `AGENTS.md` | **قاعدة جديدة + إشارات** | `RULE-N194` (§2.38) · إشارة في `RULE-N191` (§2.18) · `§2.20` بند 18 · جدول `§2.27` · سطر `PILOT-14` في سجل §2.39 |
| `docs\SHARED-HOME-REGISTER.md` | هذا القسم + `H-09` | — |

**`PILOT-14` — العيب في الشاشة التجريبية وأُصلح فيها:**

| # | العيب | البرهان المقيس | الإصلاح |
|---|---|---|---|
| **PILOT-14** | `Enter` على حقل مطلوب **فارغ** كان ينقل المؤشر إلى التالي ثم تُطلق `Leave` فتظهر ✗ والومضة على الحقل السابق — «الرسالة تظهر والمؤشر انتقل» (نصّ المالك) | قراءة المسار: `MoveNextByDisplayOrder` ⇒ `.Focus()` على التالي (L278 · L290 · L324 · L332) ثم `field.Leave` ⇒ `Evaluate` (`RequiredFieldHelper.Attach`) | الحكم يُسأل **قبل** النقل من البيت الواحد: الحقل الفارغ يبقى محفوظ التركيز مع ✗ + الرسالة + الومضة؛ والملء يزيل العلامة فوراً ويعود `Enter` للانتقال |

**القياسات (أُنتجت في هذه الموجة):**

| القياس | الطريقة | القيمة |
|---|---|---|
| مواضع `BlocksEnterNavigation` | `Select-String` في `Helpers/` | **2** = تعريف (`RequiredFieldHelper.cs:216`) + استهلاك واحد (`KeyboardHelper.cs:194`) |
| مراجعها داخل `Screens/**` | `Select-String -Pattern 'BlocksEnterNavigation'` | **0** (وممنوع أن يتغيّر — الشاشة تُعلن قواعدها في `Attach` فقط) |
| حكم «فارغ» داخل `KeyboardHelper` | `Select-String -Pattern 'IsNullOrWhiteSpace\|IsNullOrEmpty'` | **0** (لا نسخة ثانية للحكم) |
| نطاق الموجة | `git status --short` / `git diff --stat` | **ملفان** (+58 و+9) — لا مصمّم ولا شاشة |
| البناء | `dotnet build .\SalesSystem.slnx --no-incremental` | **0 أخطاء / 86 تحذيراً** · المثيَّلات المميّزة **76** = خط الأساس |
| تنسيق الأسطر | فحص CRLF/bare-LF | `RequiredFieldHelper.cs` أُعيد توحيده **CRLF** (507 سطراً · bare-LF = 0) — مطابق لبقية البيت |

**حدود مقصودة (يجب أن يعرفها المالك — من مراجعة الكود):**

| # | الحد | لماذا |
|---|---|---|
| 1 | **`Tab` / `Shift+Tab` / الفأرة لا تُحجب** | الطلب كان عن `Enter`؛ الحجب في فرع `Enter` وحده. الخروج بـ`Tab` يُظهر العلامة كما قبل التعديل |
| 2 | **آخر حقل مطلوب فارغ ⇒ `Enter` لا يُسلِّم لزر الوضع** | نتيجة مباشرة للحجب: املأ الحقل أو استخدم `Tab` |
| 3 | **ومضة/صوت مع كل ضغطة على الحقل الناقص** | `Evaluate` يُعاد كل ضغطة؛ لا تسريب مؤقّتات (`clsInputFeedback` يوقف مؤقّت الاستعادة) — تنبيه مقصود |
| 4 | **`BlocksEnterNavigation` يسأل ويوسم معاً** | يُعيد استخدام `Evaluate` بدل نسخة نقية ثانية؛ إن ظهر مستدعٍ ثانٍ يُفصل داخل البيت نفسه |

**حالة الموجة:** 🔄 **الشاشة التجريبية غير مشهودة** — لا تعميم على أي شاشة (`RULE-N184`)، و`T-81` وتعميم `Enter` على 58 موضعاً ينتظران تأكيد المالك 100%.

---

### 11.22 الموجة 19 — الجيل الثالث للوحة ألوان النظام (أزرق «المحاسب سوفت») + إصلاح ملكية أزرار اللوحة (2026-11-16)

> **طلب المالك (نصّه):** «I want to test the new Theme … refresh inside this Path about "المحاسب سوفت" Theme … give me summary and what is next? if you can edit color from one place no Problem — I want to test is better or not».

**ما وجدناه في المصدر المقروء** (`docs/all new Anylysis…/salessystemanlysis_app` — 212 ملفاً · 18 مجلداً · Next.js): الاسم «المحاسب سوفت» يظهر **مرة واحدة** كنص رأس ورقة `ZReportModal.tsx:86`، والاسم السائد في التطبيق «المحاسب الذكي». **لا مبدّل ثيمات إطلاقاً** (صفر `ThemeContext` · صفر `darkMode` · لا `tailwind.config` · Tailwind v4 عبر `@import "tailwindcss"`)، والهوية البصرية مقيسة بعدّ فعلي: `#0277BD` **144** موضعاً · `#01579B` **40** · `#0288D1` **10** · `#0F2D4A` **13** · خلفية `#EEF2F6`/`#F0F4F9` · رؤوس شبكة `#0F2D4A` بخط سفلي `#0369A1` وتحديد `#BAE6FD` · عائلات Tailwind: `slate` **7,963** · `sky` **1,674** · `emerald` **1,423** · `amber` **920** · `rose` **839**.

| الملف | التغيير | التفصيل |
|---|---|---|
| `Helpers\ThemePalette.cs` | **+96/−25 ثم +40/−17** | (أ) `AllDefinitions`: **17** من 24 قيمة إلى اللوحة الزرقاء · (ب) `RetiredDefaults`: **13 → 30** زوجاً (الجيل الأخضر + الجيل المحايد) · (ج) **`ButtonKeyFor`** مُحلِّل واحد (`internal`) · (د) `RepaintThemeButton` أُعيد بناؤه عليه · (هـ) `RepaintThemeButtons` صار `internal` |
| `Helpers\GreenThemeHelper.cs` | **+16/−1** | استدعاء واحد داخل `Apply(Form)` بعد `Apply((Control)form)` — صفر لون مضاف (`RULE-N112`) |
| `Screens\Products\Forms\frmProductMaster.Designer.cs` | **23 سطراً** | 13 سطحاً لونياً + 10 أزرار ⇒ المصمّم = التشغيل على الشاشة التجريبية (`RULE-N134`/`N140`/`N144`) |
| `AGENTS.md` | جدول §2.19 + §2.40 | قيم الجيل الثالث + أرقام الموجة + القاعدة الجديدة لأزرار اللوحة |
| `docs\SHARED-HOME-REGISTER.md` | هذا القسم + `PAL-5…PAL-7` | — |

**عيب وُجد وأُصلح في نفس الموجة (`RULE-N186` — البيت الواحد لا الشاشة):**

| # | العيب | البرهان المقيس | الإصلاح |
|---|---|---|---|
| **PAL-4b** | **أول تقاعد لمفاتيح أزرار في تاريخ اللوحة** ⇒ زر بلون افتراضي متقاعد كان يبقى **مجمّداً للأبد** في 91 مصمّماً: `RepaintThemeButton` كان يستشير `PreviousLive` (القيم التي طبّقتها اللوحة) ولا يعرف `RetiredDefaults` | `GreenThemeHelper.Walk` لا حالة `Button` فيه (3 مطابقات لـ`Walk(` كلها حاويات) · `ThemeHelper.ApplyButtonStyle` = **0 استدعاء في `Screens/**`** ⇒ الزر يرسم لون مصمّمه · الجيلان السابقان قاعدا حاويات/شبكات فقط فلا اختبار سابق يكشفه | `ThemePalette.ButtonKeyFor`: مطابقة بترتيب **حيّ ← افتراضي مشحون ← آخر قيمة طُبّقت ← متقاعد**، وأي لون خارجها = `null` ⇒ **لا كتابة** (اللون المخصّص لا يُمسّ — `RULE-N142`) · + استدعاء واحد من `GreenThemeHelper.Apply(Form)` ⇒ **الأزرار تتبع اللوحة عند كل فتح شاشة** |

**القياسات (أُنتجت في تشغيل هذه الموجة — `RULE-N174`):**

| القياس | الطريقة | القيمة |
|---|---|---|
| مصمّمات تحمل قيماً متقاعدة | مسح `*.Designer.cs` عبر **26** قيمة متقاعدة | **91** من **95** · **3,294** موضعاً |
| أكثر القيم بقاءً | نفس المسح | `14,82,52` **774** · `235,240,235` **570** · `14,165,103` **423** · `33,37,41` **341** · `235,245,235` **308** · `108,117,125` **146** |
| ألوان أزرار متقاعدة في المصمّمات | نفس المسح | `108,117,125` **146** · `23,162,184` **72** · `248,249,250` **65** · `220,53,69` **31** · `25,135,84` **30** · `13,110,253` **13** · `255,193,7` **2** |
| `RetiredDefaults` بعد الموجة | عدّ الأزواج | **30** (13 + 17) · صفر زوج مكرّر (مفتاح+قيمة) |
| تباين الأزواج السبعة (`ContrastRatioRaw`) | مراجعة الكود المستقلة | headerText/header **14.05** (3.0) · gridHeaderText/gridHeader **14.05** (3.0) · cellText/bg **17.85** (4.5) · selection/cellText **13.45** (4.5) · reject/cellText **13.37** (4.5) · textStrong/form **15.87** (4.5) · disabledFore/disabledBack **3.86** (2.0) — **7/7 ناجحة** |
| البناء | `dotnet build .\SalesSystem.slnx` | **0 أخطاء** / 65 تحذيراً (تراكمي — غير قابل للمقارنة بموجة `--no-incremental`) |

**حدود مقصودة (يجب أن يعرفها المالك):**

| # | الحد | لماذا |
|---|---|---|
| `PAL-5` | **94 مصمّماً لم تُزامَن** — وقت التشغيل صحيح (اللوحة تُعيد التلوين عبر `RetiredDefaults`) ومعاينة المصمّم تبقى بالجيل السابق | `RULE-N190` يجعل المزامنة شريحة مستقلة بموافقة المالك؛ دفعات ≤ 8 ملفات |
| `PAL-6` | **ألوان مخصّصة في الأزرار لا تُمسّ** (مثل `40,167,69` · `0,123,255` · `255,152,0` · `111,66,193` في المصمّمات الأخرى) | ليست افتراضيات لوحة ⇒ `ButtonKeyFor` يعيد `null`؛ تُحسم شاشة بشاشة عند المزامنة (`RULE-N142`) |
| `PAL-7` | **الوضع الحقيقي يقرأ لوحة محفوظة** في `SystemSettings` وتتغلّب على الافتراضيات الجديدة | اضغط «استعادة الافتراضيات» مرة واحدة؛ وضع التجربة لا يتأثر (§2.40) |

**الجولة 3 + 4 (بعد مراجعة المالك لأكواد المصدر — نفس اليوم):** أرسل المالك وصفاً سطراً بسطر لـ`MohasebSoftDashboard.tsx` + `globals.css`. القياس على 212 ملفاً أثبت **5 قيم عندنا لا تطابق** ⇒ صُحّحت (وكل قيمة قديمة دخلت `RetiredDefaults`): `FrmMainColor` → `(240,244,249)` `#F0F4F9` (المصدر `bg-[#f0f4f9]`، سطر 172) · `HeaderBarColor` → `(10,75,140)` `#0A4B8C` (بداية تدرّج الشريط) · `TextStrongColor` و`GridCellTextColor` → `(30,41,59)` `#1E293B` (`text-slate-800` — 592 موضعاً) · `BtnInfoButtonColor` → `(2,132,199)` `#0284C7` (`sky-600` — 168 موضعاً، وهو `hover:bg-sky-600` المذكور حرفياً؛ كان `cyan-600` بـ6 مواضع فقط). ومصمّم الشاشة التجريبية صار **29 سطراً لونياً**.

**عيب مقروئية وُجد في نفس الجولة (`PAL-6b`):** زر «تعديل» الكهرماني `#F59E0B` بنص **أبيض** يقيس **2.15** — تحت حد 3.0 للنص الكبير أصلاً. أُصلح في الشاشة التجريبية: النص `(30,41,59)` ⇒ **8.31**. **والعيب نظامي:** كل زر تحذير في الـ94 مصمّماً الأخرى يحمل `255,193,7` متقاعداً بنص أبيض ⇒ تباين **< 1.7**؛ يُصلح شريحة لكل شاشة في موجة `PAL-5`.

**حالة الموجة:** 🔄 **تجريبية — بانتظار شهادة المالك على `frmProductMaster`**؛ لا تعميم على أي مصمّم آخر (`RULE-N184`).

#### 11.22.1 الجولة 5 — تدرّج شريط العنوان (قرار المالك «I want grident of blue color»)

| الملف | التغيير | التفصيل |
|---|---|---|
| `Helpers\ThemePalette.cs` | **+36/−4** | 3 مفاتيح تدرّج جديدة في مجموعة «الشاشة الرئيسية» ⇒ `AllDefinitions` **27** · `ContrastRules` **12 → 18** (ثلاثة أزواج في الاتجاهين تحمي النص الأبيض على المحطات الثلاث) · `RetiredDefaults` **35** بلا تغيير |
| `Helpers\GreenThemeHelper.cs` | **+96/−7** بعد إغلاق ملاحظات المراجعة | `ApplyHeader` يستدعي `ApplyHeaderGradient` الذي يبني صورة `512×1` بـ`LinearGradientBrush`+`ColorBlend` ويُخزّنها لكل `Label` في `ConditionalWeakTable` (إعادة البناء عند تغيّر أي محطة فقط) |
| `Screens\Settings\Forms\frmChangeSystemColorsSettings.cs` | **1 سطر** | تعليق «الـ24» → «الـ27» |
| `AGENTS.md` | §2.19 + §2.20 + §2.40 + §9 | الشريط صار تدرّجاً في جدول القيم، والانحراف 1 أُعيدت صياغته كاستثناء موثَّق، وموجة ثالثة بأرقامها المقيسة |
| `docs\TASKS\TASK-002-*.md` | الجولة 5 + قسمها التفصيلي | — |

**القياسات (في تشغيل هذه الموجة):** مصمّمات تحت `Screens/**` **95** · منها **91** تحمل مسمّى رأس (`79 lblMode` + `12 lblTitle`، منها **31** تقريراً) + `ctrlDashboard` — **نفس** مجموعة أهداف `ApplyHeader` القديمة · **0** مصمّم مُسّ · `Color.FromArgb` في `GreenThemeHelper.cs` = **24** (ألوان حالة سابقة، **صفر** مضاف) · البناء **0 Errors / 86 Warning** (بناء كامل).

**ثلاث ملاحظات مراجعة أُغلقت في نفس الموجة:** تعليق يبرّر بخطر غير موجود · توقيع XOR غير أحاديّ ⇒ مقارنة ثلاثة `int` · محطتا الوسط والبداية بلا قاعدة تباين ⇒ 6 قواعد بحد 3.0.

**قيد مسجَّل (`PAL-8`):** `ContrastRules` شريك واحد لكل مفتاح، فالمحطات الثلاث محمية كأطراف (كل محطة ↔ النص الأبيض) و`HeaderTextColor` يُقاس مقابل آخر شريك (الوسط) لا مقابل الثلاثة — لا يمنع حفظ شريط غير مقروء، لكن أداة الاقتراح ترى شريكاً واحداً.

#### 11.22.2 الجولة 6 — العودة الكاملة للأخضر الأصلي وإزالة التدرّج (قرار المالك 2026-11-16)

> **نصّ القرار:** «return to green color the color and remove grident of blue color» — وبسؤاله «أي أخضر؟» اختار **«الأخضر الأصلي الكامل»** (لا الشريط الأخضر مع خلفية محايدة).

| الملف | التغيير | التفصيل |
|---|---|---|
| `Helpers\ThemePalette.cs` | حذف 3 مفاتيح + 3 صفوف + 6 قواعد · إعادة 24 قيمة + **18** زوجاً متقاعداً | القيم مستخرجة من `git show 3da957c:…ThemePalette.cs` (`RULE-N195`) ⇒ `AllDefinitions` **24** · `ContrastRules` **12** · `RetiredDefaults` **35 → 53** |
| `Helpers\GreenThemeHelper.cs` | **−96 سطراً** | حُذف `ApplyHeaderGradient` · `BuildHeaderGradient` · `HeaderGradientState` · `HeaderGradients` · `HeaderGradientPixels` · الاستدعاء · توثيق التدرّج ⇒ `ApplyHeader` عاد `BackColor`/`ForeColor`/`TextAlign` فقط |
| `Screens\Products\Forms\frmProductMaster.Designer.cs` | 52 سطراً لونياً | أُعيدت مزامنته للقيم الخضراء الحرفية ⇒ **المصمّم = التشغيل** · `btnEdit.ForeColor` الداكن بقي (إصلاح مقروئية `PAL-6b`) |
| `Screens\Settings\Forms\frmChangeSystemColorsSettings.cs` | سطر واحد | تعليق 27 → 24 |
| `AGENTS.md` | §2.19 + §2.20 + §2.40 + §9 | جدول القيم أخضر + تاريخ الألواح الأربعة + موجة رابعة بأرقامها + إلغاء موجة التدرّج |
| `docs\TASKS\TASK-002-*.md` | الرأس + الجولة 6 | القيم والأعداد والبناء |

**القياسات (بأمر في هذه الجولة):** `(14,165,103)` **426** موضعاً في **90** ملفاً · `(14,82,52)` **776** في **89** · `(235,240,235)` **571** في **83** · `(220,240,220)` **91** في **73** · `(255,255,180)` **83** في **75** · القيم الزرقاء `(10,75,140)` و`(186,230,253)` = **0** · صفر ذكر لكلمة `Gradient` في الحل · `Color.FromArgb` في `GreenThemeHelper.cs` = **24** بلا تغيير · البناء **0 Errors / 86 Warning** (بناء كامل) · **صفر تغيير في قاعدة البيانات أو الباك-إند**.

**أثر جانبي إيجابي:** دَين `PAL-5` (94 مصمّماً غير مزامَن) **سقط تلقائياً** — لأن المصمّمات بقيت على القيم الخضراء الأصلية طوال موجات الأزرق، فعادت مطابقة للافتراضيات بلا مسّها.

**ملاحظات تُبلَّغ للمالك:** أبيض الشريط على `(14,165,103)` = **3.18** (≥ 3.0 للنص الكبير؛ `frmPOS` و`frmFormatingSetting` بـ`12 Bold` على الحدّ الأدنى) · زر التحذير `(255,193,7)` بنص داكن **8.31** · التعطيل **2.24** · **والمصيدة القديمة:** في الوضع الحقيقي لوحة محفوظة في `SystemSettings` تتغلّب على الافتراضيات ⇒ «استعادة الافتراضيات» مرة واحدة.

---

### 11.23 تعميم الشاشة التجريبية — الموجات 11–18 (2026-11-16) · الملف الحيّ `docs/MangeSubaggentsAndTracProgress.md`

> **اللوحة هي المصدر:** كل شاشة وموجة وأرقامها في `docs/MangeSubaggentsAndTracProgress.md` (`RULE-N174` — لا تُنسخ هنا). هذا الصف يثبت **بيوت الكود المشترك** التي مُسَّت في الموجات، لا الشاشات.

| الملف (البيت) | التغيير | التفصيل |
|---|---|---|
| `Helpers\KeyboardHelper.cs` | `IsEnterNavigationInput` يستثني `TextBox { Multiline && AcceptsReturn }` | إصلاح **صنف** لا حالة (`§4` خطوة 9b): `Enter` في الملاحظات كان يُنقل تركيزاً بدل سطر جديد. القياس: **12** حقلاً عديد الأسطر في **12** مصمّماً، منها **8** شُحنت في الموجات 11–17 ⇒ كان عيباً حيّاً. **لم يُستثنَ `Multiline` وحده** لأن `TextBox.IsInputKey` لا يعدّ `Enter` مفتاح إدخال إلا مع `AcceptsReturn`، فاستثناء `Multiline` وحده كان سيجعل `AcceptButton` يبتلع `Enter` في **11** حقل ملاحظات آخر (**9** من شاشاتها تضبط `AcceptButton`) — أي عيب أسوأ من المُصلَح |
| `Screens\**\frm*.cs` (33 شاشة) | ترحيل النمط فقط | الاستهلاك عبر الـHelpers: `KeyboardHelper.WireEnterAsTab(scope, _PrimaryActionButton)` + `RequiredFieldHelper.Attach/Validate/FirstMissing/Clear` + `KeyboardHelper.IsTextInputFocused(this)` — **صفر نسخة محلية** من حارس التركيز أو حكم «مطلوب» في الشاشات المرحَّلة |
| `docs\MangeSubaggentsAndTracProgress.md` | اللوحة الحيّة | §0 أرقام مقيسة · §5 سجل الموجات · §5.1 الموجة التالية · §6 جرد 94 صفاً |

**القياسات (بأمر في نفس التشغيل):** نماذج `frm*` **94** · ثنائي **33** · أحادي قديم **25** · معفى **36** · `RequiredFieldHelper` **26** شاشة · `_RequiredFieldsValid` **0** · `_FocusFirstMissingField` **0** · `_ValidateForm` **2** (`frmSalesReturn` · `frmSupplierPaymentDialog`) · نسخ محلية لحارس الحقل **0** · `NavigationOptions` في `Screens/**` **0** · `BlocksEnterNavigation` في `Screens/**` **0** · البناء `--no-incremental` = **0 Errors / 86 Warning** (خط الأساس) · **صفر ملف باك-إند** (`RULE-N177`).

**عيب مسجَّل لم يُصلَح (قرار مالك مطلوب):** `PAL-6` — **86** زراً في **56** مصمّماً من 95 تحمل `BackColor` لا يملكه أي مفتاح أزرار في اللوحة (**35** منها على `16,150,96`، ومنها `btnRefresh` في التقارير الـ31) ⇒ لا تُعاد تلوينها عند تغيير اللوحة. التسجيل لا الاستبدال الأعمى.

---

## 12. موجة 2026-11-17 — حقول دليل الحسابات الهرمية (`TASK-008`)

> **الطلب (نصّه):** «update all project code 4.1 Accounts — دليل الحسابات الهرمي» + سلّم الأطوال الرسمي (1 · 2 · 4 · 8) + «remove cuurencey» + «I will do migrations you write code and stop».

**نُفِّذت على 7 وكلاء في ثلاث موجات، كل ملف بمالك واحد (`RULE-N158`).**

| الملف | التغيير | التفصيل |
|---|---|---|
| `Models\Accounting\Account.cs` | +2 عمود +3 سمات مشتقة | `byte? NormalBalance` · `bool AllowCostCenter` · `IsSubAccount` · `EffectiveNormalBalance` · `FinancialStatement` (get-only ⇒ غير مُسقَّطة) + وسائط اختيارية في `Create`/`Update` بتحقق عربي |
| `DataAccess\Configurations\AccountConfiguration.cs` | سطران | `tinyint` nullable · `bit` required + `HasDefaultValue(false)` — لم يُمسّ CHECK ولا الفهرس المفلتر ولا الـFK |
| `DTO\AccountDtos.cs` + `Requests\AccountingRequests.cs` | إضافي فقط | الـDTO يحمل **المخزَّن** (`0` = مشتق) + `EffectiveNormalBalance` + `NormalBalanceDisplay` («(تلقائي)») + `AccountCodePreviewDto` + وسائط اختيارية في الطلبين |
| `Business\Helpers\AccountCategoryHelper.cs` | **ملف جديد** | بيت واحد لتصنيف الدفاتر المساعدة — 5 بادئات على السلّم (`1103` عميل · `2101` مورّد · `1107` موظف · `1102` بنك · `1101` صندوق) + احتياط بالاسم |
| `Business\Services\AccountService.cs` | جوهر الشريحة | `TreePath` · `Balance` بإشارة الطبيعة الفعّالة · `FinancialStatement` · `CategoryLabel` · `GetNextChildCodeAsync` · إزالة تقييدات المستوى · **القيود المرحَّلة فقط في الرصيد** · توقيع الافتتاحي بـ`EffectiveNormalBalance` · 3 رقابات تعديل · `includeInactive` يعمل |
| `Business\Services\AccountCodeGeneratorService.cs` | حارس | السلّم الرسمي كما هو + زيادة عند التصادم + `SemaphoreSlim` |
| `Desktop\Services\{Interfaces,Direct,Mock}` | عقد | `GetNextChildCodeAsync` في الـ3 + تكافؤ رفض (37 نداء) |
| `Desktop\Services\Mock\MockAccountsData.cs` | إعادة بناء | **42** حساباً على السلّم (1=4 · 2=11 · 4=21 · 8=6) · `AddChildAccount`/`AddCashBoxAccount`/`NextId` |
| `Screens\Accounting\Forms\frmAccounts*.cs` | الشاشة | `cmbNormalBalance` + `chkAllowCostCenter` + 4 أعمدة (المسار/الرصيد/القائمة المالية/التصنيف) + معاينة كود الابن الحيّة |
| `Mock\*.cs` (4 ملفات) | إعادة ترقيم | أكواد السلّم القديمة (`1111/1121/1112`) ⇒ **0** بقايا · `AccountId` البذور أُعيد ربطه · كود الصندوق عبر `NextChildCode` |
| `docs\**` + `AGENTS.md` | توثيق | `database-schema.md` §4.1 (19 قسماً منفَّذاً) · `TASK-008` جولة 3 · `MOCK-COVERAGE` 1.3 · `ACTIVE` · §9 |

**أعطال وُجدت وأُصلحت في نفس الموجة (`RULE-N186`):**

| # | العيب | البرهان المقيس | الإصلاح |
|---|---|---|---|
| `ACC-UI-1` | أعمدة شبكة `frmAccounts` كانت **معلنة بلا `Columns.Add`** ⇒ الشبكة تُولّد 15 عموداً تلقائياً و`Cells["colNameAr"]` يرمي داخل `catch` صامت | قراءة المصمّم: 9 حقول معلنة · 0 `Columns.AddRange` | كل الأعمدة معلنة ومضافة + `AutoGenerateColumns = false` |
| `ACC-SEM-1` | الـDTO يُرجع **الفعّالي** لا **المخزَّن** ⇒ خيار «تلقائي» لا يعود كما اختاره المالك | `AccountService.cs:70` كان `EffectiveNormalBalance` | فاصل صريح: مخزَّن `0` = مشتق |
| `ACC-FIN-1` | الرصيد يضم **المسودات والملغاة** | `LoadBalancesAsync` بلا تصفية حالة | `Status == Posted` في نفس الاستعلام الواحد |
| `ACC-FIN-2` | الافتتاحي يُوقَّع بـ`Nature` لا بالطبيعة الفعّالة ⇒ حساب مضاد على الطرف الخاطئ | `account.IsDebitNormal()` | `EffectiveNormalBalance == 1` |
| `ACC-SEED-1` | `AccountId` في بذور القيود يشير لحسابات أخرى بعد الترقيم | «رأس المال» ⇒ «أصول متداولة» | 9 بنود أُعيد ربطها بالكود الصحيح |

**القياسات (بأمر في هذه الموجة):** `dotnet build .\SalesSystem.slnx` = **0 Errors / 118 Warning** · خصائص `Accounts` في اللقطة = **19** (كانت 17) · `CreatedByUserId` في اللقطة = **0** و`CreatedByUserName` = **27** · حسابـات الـMock = **42** صفاً بصفر مخالفة سلّم · نداءات الرفض في `MockAccountDesktopService` = **37** · مراجعة الكود = `0 Blocker · 4 Major · 5 Minor` (الأربعة أُصلحت) · شهادة `mock-qa` = `مؤهل جزئياً — تكافؤ الرفض ناقص` (`ف-AC-1…3` مغلقة · `ف-AC-5…9` مفتوحة، كلها S وفي طبقة الـMock وحدها).

**حدود مقصودة:** `Balance` يُجمَّع في C# من تحميل واحد (لا `GROUP BY` في SQL) — كافٍ لـMVP · **الترحيل ملك المالك** («I will do migrations») فلم يُنشئ أي وكيل ترحيلاً ولا حذفه · انحراف دلالات بذور الـMock مقابل `AccountingSeeder` (`2201` مقابل `3101`) يحتاج توحيد أسماء عربية.

---

## 13. موجة 2026-11-17 — ⛔ **ملغاة**: نقل أزرار العمليات إلى داخل `ctrlAccountEditor` (`TASK-009`)

> **طلب المالك:** «Convert buttons to ctrlAccountEditor.cs … and use ctrlAccountEditor.cs inside frmAccounts» — تطابقاً مع `docs/acounts.md` الذي يضع أزرار اللوحة اليمنى.
>
> **⛔ قرار المالك بعد المعاينة (2026-11-17):** «no I do not want ctrlAccountEditor convert to inside frmAccounts and removed ctrlAccountEditor» ثم اختار **«رجوع كامل للموجة»**. **الحالة الفعلية الآن = HEAD حرفياً:** الشريط في `gbCommands` برأس `frmAccounts`، و`ctrlAccountEditor` يحمل زرَّي «حفظ/إلغاء» الداخليَّين، وصفر `SetActions`/`PrimaryActionButton`. **الجدول أدناه سجلّ لما جرى ثم أُلغي — لا يصف الحالة الحالية.**

| الملف | كان (ثم أُلغي) | الحالة بعد الرجوع |
|---|---|---|
| `Controls\Common\ctrlCrudToolbar.cs` | عضو عام `PrimaryActionButton => btnSave` | ⛔ محذوف — `git diff HEAD` فارغ |
| `Screens\Accounting\Controls\ctrlAccountEditor.cs` | `SetActions` + خمسة أحداث مُمرَّرة + حذف `btnSave/btnCancel` | ⛔ عاد كما في HEAD (9 مواضع `btnSave/btnCancel`) |
| `Screens\Accounting\Controls\ctrlAccountEditor.Designer.cs` | `ctrlCrudToolbar1` بدل `flpActions` | ⛔ عاد (52 موضعاً) · صيغة `Convert.ToInt32` سليمة |
| `Screens\Accounting\Forms\frmAccounts.cs` | حذف خاصية `Toolbar` + `Editor.SetActions` | ⛔ عاد: `Toolbar` + 7 اشتراكات + `Editor.SetMode` |
| `Screens\Accounting\Forms\frmAccounts.Designer.cs` | حذف `ctrlCrudToolbar1` · `gbCommands` 72 | ⛔ عاد: الشريط داخل `gbCommands` (118) · 12 موضعاً |

**القياسات بعد الرجوع (بأمر في هذه الموجة):** `git diff HEAD` للملفات الخمسة = **فارغ** · `git status --short` لها = **فارغ** · `PrimaryActionButton` = **0** · `SetActions` = **0** · `ctrlCrudToolbar1` في `frmAccounts*` = **12** · `btnSave|btnCancel|flpActions` في `ctrlAccountEditor.*` = **9 / 52** · تعديلات المالك الآخر سليمة (`cbAccountType` 13/15 · `chkAllowCostCenter` 5/15 · `cbNormalBalance` 13/14 · `colFinancialStatement`+`cbNatureFilter` = 22) · البناء `--no-incremental` = **0 Error / 118 Warning**.

**درس مسجَّل:** نُقل الهيكل بلا شهادة الشاشة التجريبية ⇒ رُفض وأُعيد في اليوم نفسه. أي إعادة تخطيط لهذه الشاشة تُعرض أولاً على المالك **قبل** التنفيذ (`RULE-N184` منطقاً).

---

## 14. موجة 2026-11-17 — ✅ **إزالة الوحدة المحاسبية من V1** (`TASK-011`)

> **أوامر المالك:** «remove accounting from MVP v1» · «remove all Accounting code» · «and remove all migrations» · «use many subagents». **18 وكيلاً في 7 موجات**، مالك واحد لكل ملف (`RULE-N158`/`RULE-N159`).

| الملف (البيت/الطبقة) | التغيير | التفصيل |
|---|---|---|
| `Models\Accounting\**` | **حذف 11** | 6 كيانات + 5 تعدادات |
| `Models\Customer|Supplier|CashBox|Expense` | إزالة `AccountId`/`ExpenseAccountId` + التنقّل | لا عمود حساب في أي كيان باقٍ |
| `Business\Accounting\**` + 3 خدمات + 7 واجهات + `AccountCategoryHelper` | **حذف 16** | صفر `IAccountingIntegrationService`/`IAccountLinkService` |
| `Business\Services` (11 خدمة) | فصل الاقتران | `SalesService` · `SalesReturnService` · `PurchaseService` · `PurchaseReturnService` · `CustomerReceiptService` · `SupplierPaymentService` · `ExpenseService` · `InventoryAdjustmentService` · `CashBoxService` · `CustomerService` · `SupplierService` — **حدود المعاملة والمخزون والمال لم تُمسّ** |
| `CashBoxService` + `CashBoxReportService` | **إعادة بناء دفتر الصندوق** | لا يوجد جدول `CashTransactions`: الرصيد كان مجموع سندات القبض − الصرف ⇒ صار **مبيعات نقدية + سندات قبض − مدفوعات موردين − مصروفات** (المرحّل فقط) |
| `DataAccess` | 6 تكوينات + `AccountingSeeder` + `DbContext` + `UnitOfWork` + `DbSeeder` | صفر جدول محاسبي في الموديل · البذور بلا حسابات |
| `DataAccess\Migrations\**` | **حذف كل الترحيلات** | بأمر المالك «and remove all migrations» — يولّد `InitialCreate` بنفسه |
| `DTO` | **حذف 10 + إضافة 1** | `AccountStatementDto` **أُعيد** لأنه كان محذوفاً بالخطأ ويستهلكه `frmAccountStatementReport` (الشاشة المرجعية للتخطيط) |
| `Services\{Interfaces,Direct,Mock}` | حذف العقود والتنفيذات + إعادة بناء Mock التقارير | `IFinancialReportDesktopService` **12 → 5** |
| `Screens\Accounting\**` | **حذف 12** | `frmAccounts` · `frmAccountsTree` · `frmJournalEntries` · `ctrlTreeOfAccounts` |
| `Screens\Reports\...\Accounting` | **حذف 4 تقارير** | ميزان المراجعة · دفتر الأستاذ · الميزانية العمومية · رأس المال العامل — وبقيت 4 تعمل من المستندات |
| `Program.cs` · `frmMain(.Designer)` · `ReportNavigationHelper` | تنظيف | صفر `Accounting` في `Program.cs` · قائمة «المحاسبة» محذوفة · التقارير المالية 4 عناصر |
| 9 شاشات/عناصر | إزالة حقول الحساب | `frmCashBoxMaster`(+Designer) · `frmCashBoxTransactions` · `frmAddUpdateExpense` · `frmListExpenses`(+Designer) · `ctrlCustomerCard`(+Designer) · `frmCustomerStatement` · `frmAddUpdateSalesInvoice`(+Designer) |

**القياسات (بأمر في نفس التشغيل):** `dotnet build .\SalesSystem.slnx --no-incremental` = **0 Errors / 100 Warnings** · 157 مساراً (**84 محذوفاً · 72 معدّلاً · 1 مُضاف**) · `ToTable` فريدة = **34** · صفر مرجع لكل من: `Models.Accounting` · `JournalEntr` · `SystemAccountMapping` · `AccountService` · `frmAccounts` · `frmJournalEntries` · `ReceiptVoucher|PaymentVoucher` · `Accounting` في `Program.cs`.

**عيوب وُجدت وأُصلحت في الموجة نفسها (`RULE-N186`):**
| # | العيب | الإصلاح |
|---|---|---|
| `ACC-DEL-1` | حذف `AccountStatementDto` مع ملف `JournalEntryDtos.cs` يكسر **الشاشة المرجعية** للتخطيط | أُعيد من `git show 5f87fb2` حرفياً في ملف مستقل |
| `ACC-CASH-1` | `MockCashBoxDesktopService.TransferAsync` يُرجع `Success()` بلا أثر مالي | رفض عربي مطابق للوضع الحقيقي |
| `ACC-CASH-2` | `DirectCashBoxService.TransferAsync` ينادي دالة BLL محذوفة | رفض صريح في الوضعين |
| `ACC-STMT-1` | كشف حساب العميل كان **يُضاعف** الحركة (فاتورة + قيد) | حُذف مصدر القيود؛ بقي 3 مصادر (فواتير/مرتجعات/سندات) |

**مفتوح:** إسقاط 8 أعمدة + 6 جداول على القاعدة الحيّة + توليد ترحيل = **ملك المالك** (`RULE-N198`) — تفصيل في `TASK-011` §13.4.


