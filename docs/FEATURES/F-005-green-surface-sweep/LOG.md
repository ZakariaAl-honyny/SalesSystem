# F-005 — سجل الجولات (LOG)

## الجولة 1 — 2026-11-19 — W1: ثمانية مصمّمات `frmCRUD*Master`

### الهدف
تطبيق شكل `frmCRUDCProductMaster` (المرجع المعتمد من المالك بنصّه) على بقية عائلة `frmCRUD*Master`، وإغلاق شكوى «الشاشة بيضاء والحقول غير واضحة».

### القياس قبل الموجة (أمر: قراءة 90 `*.Designer.cs`)

| القياس | الرقم |
|---|---|
| مصمّمات مكتشَفة | 90 |
| داخل النطاق / معفاة (نوافذ معاينة الطباعة) | 85 / 5 |
| خلفية نموذج رمادي الجيل الثاني `240,242,245` | 68 |
| حاويات معلنة `Color.White` | 742 من 893 |
| حقول إدخال بلا `Color.White` | 14 من 328 |
| ألوان بالصيغة المختصرة (`RULE-N204`) | 1206 |
| ماسترات `frmCRUD*` على الأخضر الحيّ قبل الموجة | 3 من 11 |

### الجولة (3 `ui-agent` + 1 `fast-agent`)

| الوكيل | الملفات | الناتج |
|---|---|---|
| `ui-agent` A | `frmCRUDCSupplierMaster` · `frmCRUDCustomerMaster` | خلفية `170,220,170` · 10/10 حاوية خضراء · 10/10 حقول بيضاء · 28 صيغة مختصرة ← 0 |
| `ui-agent` B | `frmCRUDCProductUnitMaster` · `frmCRUDCashBoxMaster` · `frmCRUDCategoryMaster` | نفس المعايير · 6/6 و9/9 حقول بيضاء |
| `ui-agent` C | `frmCRUDUnitMaster` · `frmCRUDUserMaster` · `frmCRUDWarehouseMaster` | نفس المعايير |
| `fast-agent` | 5 ملفات | توحيد تعارضين مكتشفين بين الوكلاء |

### التعارضات المكتشفة والإصلاح (قبل البناء الثاني)

1. **`ColumnHeadersDefaultCellStyle.SelectionBackColor`**: وكيل كتب `255,255,180` (بيج) وآخران كتبا `14,165,103` (أخضر الرأس). **الحسم بالأمر:** `GreenThemeHelper.ApplyGrid` يفرض `GridHeaderColor` على رأس العمود + `SelectionForeColor = White` ⇒ البيج كان يُنتج أبيض-على-بيج بتباين 1.1. صُحّح في `frmCRUDCSupplierMaster` و`frmCRUDCustomerMaster`، وصُحّح **البند 18 في `BRIEF.md`** (صار 18 + 18b) لأنه كان هو الخطأ الجذري.
2. **`btnToggleFilters.FlatStyle`**: 5 ملفات `Flat` و3 ملفات `Popup` (المرجع `Popup`). وُحِّد على **`Flat`** (`D-06`) وتُرك المرجع بلا مسّ (`D-08`) — فرق بصري بسيط يُحسم في الشهادة.

### البناء

| المرحلة | النتيجة |
|---|---|
| بعد الموجة الأولى (3 وكلاء) | **0 Errors / 95 Warning(s)** |
| بعد إصلاح التعارضين (`fast-agent`) | **0 Errors / 95 Warning(s)** |

التحذيرات كلها `CS0169` سابقة (حقول أعمدة معلنة وغير مستخدمة في مصمّمات قديمة) — لا تحذير جديد من الموجة.

### المراجعة (`code-reviewer` — قراءة فقط، مرة واحدة على الشريحة)

**Pass with notes — 0 Blocker · 0 Major · 3 Minor**

| # | البند | الحسم |
|---|---|---|
| 1 | `frmCRUDCProductUnitMaster:121` — `lblProduct.ForeColor = (30,86,160)` أزرق خارج اللوحة، **سابق للموجة** (مثبت في `HEAD`) | يبقى · يُسجَّل بنداً مفتوحاً (`O-1`) |
| 2 | `frmCRUDCustomerMaster:425` — `btnShowCard` رُسم بلون `btnInfo` بدل الاحتياطي | يُعتمد `D-10` |
| 3 | `frmCRUDCProductMaster` (المرجع) يخالف الجدول في `btnToggleFilters` (`Popup` + نص `14,82,52`) | قرار المالك في الشهادة (`O-2`) |

**ما تحقّق إيجاباً بالقياس:** المرجع لم يُمسّ (سطر واحد فقط: إعادة تسمية الصنف) · صفر تغيير في `Dock/Anchor/Size/Location/TabIndex/Text/Font/Name/ReadOnly/SelectionMode/AllowUserToAddRows/DataSource/الأحداث` · صفر صيغة مختصرة · صفر مرجع `Helpers/` داخل مصمّم · صفر لون حالة معلن · كل القيم من اللوحة الحيّة · كل حقول الإدخال بيضاء · كل شبكة مطابقة لما يفرضه `ApplyGrid`.

### العائق (لم يُغلق)

**لم يُنفَّذ أي التزام (`commit`).** شجرة العمل كانت — قبل هذه الموجة — تحمل **96** مدخلاً غير ملتزم من موجة سابقة (إعادة تسمية `frm*Master → frmCRUD*Master`: **21** حذفاً · **22** ملفاً جديداً · **53** تعديلاً). التزام ملفات F-005 وحدها يترك `HEAD` بحذف/إضافة نصف إعادة التسمية فينكسر البناء على `HEAD`. القرار للمالك: التزام الموجتين معاً كموجة واحدة، أو فصل إعادة التسمية أولاً في التزامها الخاص.

### التالي

**W2** — المستندات والقوائم (8 ملفات) — **معلّقة على شهادة المالك البصرية لـW1** (`RULE-N184`/§2.39)، وعلى قرار الالتزام أعلاه.

**قائمة تعارضات يجب أن يعرفها كل وكيل W2 (تُضاف إلى كل brief):**
1. `ColumnHeadersDefaultCellStyle.SelectionBackColor = 14,165,103` + `SelectionForeColor = White` — **لا بيج على الرأس**.
2. `DefaultCellStyle.BackColor = White` (الخلايا بيضاء) · `dgv.BackgroundColor = 220,240,220` (منطقة الشبكة) — لا تُخلط.
3. `btnToggleFilters.FlatStyle = FlatStyle.Flat` · `ForeColor = 30,41,59`.
4. الحاويات تُعلَن صراحةً (لا Ambient): `Panel/FlowLayoutPanel/TableLayoutPanel` = `235,240,235` · `GroupBox` = `235,245,235`.

## الجولة 2 — 2026-11-19 — W2: المستندات والقوائم (6 مصمّمات)

| # | الملف | ما نُفِّذ |
|---|---|---|
| 1 | `Sales/Forms/frmAddUpdateSalesInvoice.Designer.cs` | خلفية `170,220,170` · 9/9 حاويات خضراء · 16/16 حقلاً أبيض · `dgvItems` رأس `14,165,103`+أبيض وتحديد `255,255,180` · `FlatStyle` لأربع قوائم كانت تُرسم رمادية · صفر قيمة متقاعدة |
| 2 | `Sales/Forms/frmListSales.Designer.cs` | خلفية + 9 حاويات + 5/5 حقول + شبكة كاملة + أزرار الأدوار |
| 3 | `Returns/Forms/frmListReturns.Designer.cs` | 7 حاويات بيضاء ← خضراء · شبكتان (`dgvSales`/`dgvPurchases`) مطابقتان |
| 4 | `Purchases/Forms/frmAddUpdatePurchaseInvoice.Designer.cs` | خلفية · 4 صناديق + 5 ألواح خضراء · 7/7 حقول بيضاء · رأس شبكة كان `52,58,64` صار `14,165,103` |
| 5 | `Purchases/Forms/frmListPurchases.Designer.cs` | **خلفية النموذج كانت غير معلنة أصلاً** ← أُضيفت · 6 حاويات · `lblMode` من `16,150,96` |
| 6 | `Payments/Forms/frmListPayments.Designer.cs` | **75** لوناً مختصراً ← الصيغة القانونية · خلفية من `240,242,245` · شبكتان |

**تصحيحات قياس (بأمر — `RULE-N195`):** عدّاد «الصيغة المختصرة» السابق (1206) استخدم نمطاً يشترط مسافة بعد الفاصلة، فلم يرَ `Color.FromArgb(240,242,245)` بلا مسافات — **الرقم الصحيح بعد الالتزام الأول: 924 موضعاً في 34 مصمّماً** (والرقم قبل الموجتين لم يُعد بمسطرة متسامحة، فلا يُقارن). كذلك صفّ `frmListPayments` في `MANIFEST.md` («0 لون») كان خاطئاً — صحّحه الوكيل بالقياس (75 لوناً).

**تعارضات حُسمت بين وكلاء W2:** `btnView` / أزرار النطاقات = `btnSecondary` (صف 12) في الملفات الأربعة · `btnRefresh` = `btnInfo` · `btnCancel` الخاص بإلغاء فاتورة شراء = أحمر `btnDelete` (الفعل مدمّر لا الاسم) · `pnlHeader` = `235,240,235` في الملفات الأربعة · `TabPage` تُعامَل كحاوية فتُعلَن `235,240,235`.

**البناء بعد W2:** **0 Errors / 112 Warning(s)** (تزايدي؛ التحذيرات كلها `CS0169` سابقة — يُعاد القياس بـ`--no-incremental` عند الإغلاق).

**BRIEF مُستكمل بقاعدتين من W2:** `RadioButton` يأخذ خلفية حاويته لا `White` · `Label` بـ`Dock` عريض = سطح بار لا حقل.

## الجولة 3 — 2026-11-19 — W3: المخزون (8 مصمّمات)

| # | الملف | ما نُفِّذ |
|---|---|---|
| 1 | `Inventory/Forms/frmInventory.Designer.cs` | 51 صيغة مختصرة ← 0 · **7** حاويات بيضاء ← 0 · 5/5 حقول · 3 شبكات كاملة |
| 2 | `Inventory/Forms/frmInventoryList.Designer.cs` | 36 ← 0 · 5 حاويات ← 0 · 5/5 حقول |
| 3 | `Inventory/Forms/frmListInventoryTransactions.Designer.cs` | 56 استبدالاً · 5 أسطر `ForeColor` مضافة |
| 4 | `Inventory/Forms/frmListInventoryBatches.Designer.cs` | 50 استبدالاً + 15 سطر لون مضاف (أزرار كانت بلا لون) |
| 5 | `Inventory/Forms/frmListInventoryAdjustments.Designer.cs` | 48 استبدالاً · 11 زراً كاملة |
| 6 | `Inventory/Forms/frmAddUpdateInventoryAdjustment.Designer.cs` | 61 استبدالاً · 9/9 حقول |
| 7 | `Inventory/Forms/frmStocktakingNew.Designer.cs` | 5 صناديق خضراء · 6/6 حقول (`dtpDate` أُضيف) |
| 8 | `Inventory/Forms/frmWarehouseTransferNew.Designer.cs` | 3 صناديق + `pnlFooter` + `lblNote` سطحاً · 7/7 حقول |

**تصحيح قياس ثالث (`RULE-N195`):** «بلا أي تصريح لون» في briefين كان مبالغة — الملفات كانت تحمل 45–56 تصريحاً **بالصيغة المختصرة** لا خالية من الألوان. القاعدة العملية: القياس يتم بالنمط المتسامح `\d+,\s*\d+` لا بالنمط الذي يشترط مسافة.

**جديد: «المصمّم يتبع وقت التشغيل عند فرضه» (القاعدة 10) طُبِّقت مرتين:** `frmListInventoryBatches` أزراره الخمسة تُلوَّن من `_ApplyUiTheme` في `.cs` (Neutral/Secondary) فأُخذت قيم وقت التشغيل حرفياً · `lblRecordsCount` يفرضه `frmInventoryList.cs:113` = `TextStrong`.

**البناء بعد W3:** `--no-incremental` = **0 Errors / 112 Warning(s)** — كلها سابقة (`CS0169` حقول أعمدة · `CS8604/CS8602` في `Business`/`DataAccess`/code-behind) وصفر تحذير من المصمّمات.

## قرارات الأدوار الموحّدة (تُطبَّق على W4 وما بعدها — `D-11`)

| العنصر | اللون |
|---|---|
| `btnSaveDraft` · `btnPost` · `btnAddLine` (إن لم يفرض الكود غيره) | `25,135,84` (صف 7 — الحفظ) |
| `btnPickXxx` (اختيار فاتورة/صنف/عميل) | `23,162,184` (صف 10) |
| `btnView` · `btnRangeToday/Month/30` · `btnToggleFilters` | `248,249,250` + نص `30,41,59` (صف 12) |
| `btnCancel` الخاص بإلغاء مستند مرحّل | `220,53,69` (الفعل مدمّر) |
| `pnlHeader` · `TabPage` | `235,240,235` |
| `RadioButton` | خلفية حاويته |
| `Label` بعرض `Dock` | سطح `235,240,235` + نص `14,82,52` |
| `FlatAppearance.BorderColor` بقيمة `222,226,230` (متقاعدة) | `205,225,205` |
| `lblSummary` | `14,82,52` |
| أي عنصر يفرض الكود لونه وقت التشغيل | **قيمة وقت التشغيل حرفياً** (القاعدة 10) |

## الجولة 4 — 2026-11-20 — W5 + W6 + W7: بقية الشاشات والتقارير (61 مصمّماً)

| الموجة | المصمّمات | الناتج |
|---|---|---|
| **W5** | `frmCalculator` · `frmImportProductsExcel` · `frmInventoryBarcode` · `frmInventoryFifo` · `frmLogin` · `frmPasswordChange` · `frmBackup` · `frmChangeSystemColorsSettings` · `frmFormatingSetting` · `frmListCashBoxTransactions` · `frmListSupplierPayments` · `frmSupplierPaymentDialog` · `frmMain` · `ctrlDashboard` · `ctrlCrudToolbar` · `ctrlLineEntryPanel` · `ctrlCustomerCard` · `ctrlCustomerCardWithFilter` | 18 ملفاً · حقول بيضاء · أسطح خضراء · «عيّنات» شاشة الألوان **لم تُمسّ** (تُرسم من `ThemePalette` وقت التشغيل) |
| **W6** | 16 تقريراً (المبيعات 8 · المشتريات 3 · الصندوق 2 · المالية 3) | صفر حاوية بيضاء · شريط `pnlReportNav` مُعلَن · `pnlSummary` = `235,245,235` (يفرضه `ApplyReferenceStrips`) |
| **W7** | 10 تقارير (العملاء والموردون 2 · المخزون 7 + `frmAgingReport`) | نفسه · `frmExpiredProductsReport`/`frmLowStockReport`/`frmStockBalanceReport` بلا `gbFilter` **كما كانت** (لم يُضَف عنصر) |
| **إصلاحات** | 5 مصمّمات `ctrl*` (جذور) · `frmDashboard` · 5 ملفات صيغة | `GreenThemeHelper.Apply(Control)` يفرض `FormBackground` على جذر أي `UserControl` ⇒ الجذور صارت `170,220,170` (كانت `235,240,235`) — «المصمّم = التشغيل» مقيس من البيت الواحد لا مُفترض |

### الأعداد الثلاثة عند الإغلاق (`RULE-N201` — مقيسة بأمر في هذه الجولة)

| البند | قبل F-005 | بعد F-005 |
|---|---|---|
| مصمّمات مكتشَفة | 90 | 90 |
| داخل النطاق / معفاة (نوافذ معاينة الطباعة الخمس) | 85 / 5 | 85 / 5 |
| منجز / متبقٍ | 0 / 85 | **85 / 0** |
| حاويات معلنة `Color.White` | **742** | **6** = 5 في `frmPurchaseInvoicePrint` (ورق معاينة — معفى) + 1 `pnlSwatch` (عيّنة لون مقصودة) ⇒ **0 في كل مصمّم داخل النطاق** |
| ألوان بالصيغة المختصرة `Color.FromArgb(n, n, n)` | **1206** | **109** — **كلها داخل نوافذ معاينة الطباعة الخمس** ⇒ **0 في النطاق** |
| ألوان مسمّاة غير مؤهَّلة | 103+ | **43** — كلها في نوافذ المعاينة الخمس ⇒ **0 في النطاق** |
| خلفية نموذج رمادي الجيل الثاني `240,242,245` | 73 | **0** في المصمّمات (بقيت `pnlHost` واحدة في `frmDashboard` مغطّاة بـ`ctrlDashboard` بـ`Dock=Fill` وتُعاد تلوينها وقت التشغيل) |

**البوابة:** `dotnet build --no-incremental` = **0 Errors / 112 Warning(s)** — كلها `CS0169/CS0649/CS8604/CS8602` سابقة، **صفر** تحذير من أي مصمّم مُسّ.

**عائق مُبلَّغ (لم أُصلحه — ملك مهمة `TASK-022`):** `frmAddUpdatePurchaseInvoice.Designer.cs` أعاد بناءه وكيل متزامن (أزال `pnlHeader`/`pnlFooter`، أضاف `lblQuickInfo`، وأعاد بعض الألوان للصيغة المختصرة) بينما بقي `.cs` يشير إليهما ⇒ كُسر البناء. أُصلح **أصل البناء فقط** (سطران: `pnlHeader.Enabled` → `gbInvoiceInfo.Enabled` و`pnlFooter.Enabled` → `gbTotals.Enabled` — استثناء `RULE-N178` (أ))، **ولم يُلتزم** الملفان: يبقيان لمالكهما.

**قاعدة جديدة وُلدت من W5 (`D-12`):** أي `UserControl` يستهلك `GreenThemeHelper.Apply(this)` ⇒ جذره في المصمّم **يجب** أن يكون `170,220,170` لا `235,240,235`؛ وإلا صار «المصمّم ≠ التشغيل» رغم أن كل أبنائه صحيحة.
