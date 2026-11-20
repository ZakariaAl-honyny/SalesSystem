# TASK-022 — صندوق «إضافة صنف» وترتيب مكوّنات محرّر فاتورة الشراء على مرجع فاتورة البيع

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-022` |
| `العنوان` | مطابقة `frmAddUpdatePurchaseInvoice` لبنية `frmAddUpdateSalesInvoice`: صندوق `gbEntry` «إضافة صنف» + ترتيب المكوّنات + `lblQuickInfo` |
| `الحجم` | **M** — شاشة واحدة (مصمّم + code-behind) + مصمّم المكوّن المشترك، بلا تغيير عقد |
| `المرحلة` | `—` (واجهة فقط — ليست مسار بيانات جديد ⇒ لا بوابة Mock) |
| `الحالة` | **منفَّذة — بانتظار اختبار المالك** (المراجعة أُغلقت) |
| `الوكيل المالك` | `ui-agent` |
| `أثر قاعدة البيانات` | `لا شيء` |
| `التاريخ` | `2026-11-20` |

## الهدف

محرّر فاتورة الشراء/المرتجع يعرض حقول الإدخال بنفس بنية محرّر فاتورة البيع المرجعي:
عنوان ← «أوامر الفاتورة» ← «بيانات الفاتورة» ← **«إضافة صنف»** ← «الأصناف» ← «الإجماليات» + شريط `lblQuickInfo`.

## القياس قبل التنفيذ (`RULE-N195`)

| ما قِيس | القيمة | الأمر |
|---|---|---|
| `frmAddUpdateSalesInvoice` (المرجع) | `gbToolbar` + `gbInvoiceInfo` + `gbEntry` + `gbItems` + `gbTotals` + `lblQuickInfo` — 1,525 سطراً | `Read` + عدّ `GroupBox()` |
| `frmAddUpdatePurchaseInvoice` | `gbInvoiceInfo` **داخل `pnlHeader` عارٍ** · `gbTotals` **داخل `pnlFooter` عارٍ** · `ctrlLineEntry` عارٍ على النموذج · لا `lblQuickInfo` — 809 أسطر | `Read` + عدّ `GroupBox()` |
| صندوق «إضافة صنف» في الشراء | موجود **داخل** `ctrlLineEntryPanel` لكن بخلفية **بيضاء** (`White`) ونص `14,82,52` بخلاف مرجع البيع `235,245,235` / `33,37,41` | `ctrlLineEntryPanel.Designer.cs:64,68` |
| مستهلكو `ctrlLineEntryPanel` | **1** فقط: `frmAddUpdatePurchaseInvoice` (بعد دمج المرتجعات) | `grep ctrlLineEntryPanel` |

## الملفات (قائمة مغلقة)

| # | الملف | العمل |
|---|---|---|
| 1 | `SalesSystem.Desktop/Screens/Purchases/Forms/frmAddUpdatePurchaseInvoice.Designer.cs` | إزالة `pnlHeader`/`pnlFooter` · `gbInvoiceInfo` مرساة `Top` · `gbTotals` مرساة `Bottom` · `ctrlLineEntry` يبقى `Top` بينهما · إضافة `lblQuickInfo` (`Bottom`) · ترتيب `Controls.Add` كالمرجع · `ClientSize` يحفظ ارتفاع الشبكة |
| 2 | `SalesSystem.Desktop/Screens/Purchases/Forms/frmAddUpdatePurchaseInvoice.cs` | `pnlHeader.Enabled` → `gbInvoiceInfo.Enabled` · `pnlFooter.Enabled` → `gbTotals.Enabled` · تعبئة `lblQuickInfo.Text` داخل `CalculateTotals` في الفرعين (عادٍ + مرتجع) |
| 3 | `SalesSystem.Desktop/Controls/Common/ctrlLineEntryPanel.Designer.cs` | `gbEntry.BackColor` ← `235,245,235` · `ForeColor` ← `33,37,41` (مطابقة `gbEntry` المرجعي حرفياً) |

## خارج النطاق (ممنوع بالاسم)

- `frmAddUpdateSalesInvoice*` (هي المرجع — لا تُمسّ) · `frmPOS*` · `frmList*` · `frm*Selection`
- `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Program.cs` · `Helpers/**`
- `AGENTS.md` · `docs/**` (الـOrchestrator وحده يكتبها)
- أي تغيير في `DataGridView` (`ReadOnly`/`SelectionMode`/`AllowUserToAddRows`) أو في منطق الحساب/الحفظ/الترحيل

## العقود

لا تغيير — `ctrlLineEntry` وبقية الأسماء تبقى كما هي (`RULE-N158`: code-behind يعتمد عليها).

## البيت الواحد

`gbEntry` «إضافة صنف» يبقى في بيته: `Controls/Common/ctrlLineEntryPanel` — ولا تُنشأ نسخة ثانية في الشاشة (`RULE-N169`/`RULE-N172`).

## القواعد الحاكمة

`RULE-N135…N138` (ترتيب وتجميع) · `RULE-N134`/`RULE-N140` (المصمّم = التشغيل) · `RULE-N204` (صيغة `Convert.ToInt32`) · `RULE-N145` (عنصر دائم يُعلن في المصمّم) · `RULE-N125` (ترتيب الإرساء) · `RULE-N144` (مرجع الألوان).

## الموجات

| الموجة | الوكيل | الملفات | البناء |
|---|---|---|---|
| W1 | `ui-agent` | 1–3 | ⏳ |

## الأدلة (إلزامي — `docs/GOVERNANCE/EVIDENCE.md`)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| البناء قبل الموجة | `0 Errors / 112 Warning(s)` (تزايدي) | `dotnet build .\SalesSystem.slnx -p:ArtifactsPath=…` | ✅ |
| البناء بعد الموجة | `0 Errors / 95 Warning(s)` (تزايدي — **غير قابل للمقارنة** بعدد ما قبل الموجة) | نفس الأمر | ✅ |
| بقايا `pnlHeader`/`pnlFooter` في ملفَّي الشاشة | **0** | `Select-String -Pattern "pnlHeader\|pnlFooter"` | ✅ |
| `FlowDirection` داخل صندوق الإدخال | `LeftToRight` (كان `RightToLeft`) — **هذا هو إصلاح ترتيب الحقول**: داخل حاوية RTL يُعكس اتجاه `FlowLayoutPanel`، فكانت الحقول تُعرض معكوسة عن مرجع البيع | `grep FlowDirection ctrlLineEntryPanel.Designer.cs` | ✅ |
| `gbEntry` في المكوّن | `235,245,235` / `33,37,41` = `gbEntry` المرجعي حرفياً | `grep gbEntry\.(BackColor\|ForeColor)` | ✅ |
| ملفات الموجة | **3** ملفات فقط (`+81 / −84`) | `git diff --stat` | ✅ |

## المراجعة

`code-reviewer` (2026-11-20): **مقبول مع ملاحظات — 0 Blocker · 1 Major سابق للشريحة · 2 Minor · «ready to commit»**.

| # | الشدة | الملاحظة | الحسم |
|---|---|---|---|
| 1 | Major (سابق) | شريط «أوامر الفاتورة»: مجموع العشرة + الهوامش = **1,395** في لوحة **1,134** ⇒ `btnClose` (`x=−105`) و`btnRemoveItem` (`x=−253`) خارج الشريط المرئي — **كان كذلك قبل الشريحة** (كانا مقصوصين من الجهة الأخرى) | ✅ مسجَّل كبند متابعة مستقل: `docs/TASKS/TASK-024-purchase-invoice-toolbar-overflow.md` — لا يُغلق على هذه الشريحة |
| 2 | Minor | تقرير التسليم قال «جذر المكوّن `235,240,235`» والصحيح أن جذر `ctrlLineEntryPanel` صار `170,220,170` (لون الشاشة الحاضنة) و`235,240,235` هي `pnlEntry.BackColor` | ✅ تصحيح صياغة — القيمة الفعلية صحيحة ومطابقة للوحة |
| 3 | Minor | `pnlEntry` المرجعي يحمل `BorderStyle = FixedSingle` والمكوّن لا يحمله | ⏳ مؤجَّل إلى `F-005` (المكوّن مسجَّل عندها `todo`) |

**تحقّقات مقبولة بالقراءة:** ترتيب الإرساء حسابياً مطابق للمرجع (lblMode 0–46 · gbToolbar 46–116 · gbInvoiceInfo 116–244 · ctrlLineEntry 244–348 · gbItems 348–570 · gbTotals 570–782 · lblQuickInfo 782–823) · صفر بقايا للألواح · `lblQuickInfo` في الفرعين بنفس نص المرجع · صفر تغيير سلوكي · صفر مخالفة `RULE-N204` · ادعاء `FlowDirection` **مُثبت** (المرجع لا يُعلن الاتجاه ⇒ LeftToRight افتراضاً؛ والمشتريات كانت `RightToLeft` + RTL ⇒ انعكاس مزدوج = ترتيب معكوس).

## الاختبار اليدوي

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmAddUpdatePurchaseInvoice` | فتح الشاشة من قائمة المشتريات | الترتيب من أعلى: عنوان ← أوامر الفاتورة ← بيانات الفاتورة ← **إضافة صنف** ← الأصناف ← شريط الملخّص ← الإجماليات. صندوق «إضافة صنف» بخلفية `235,245,235` كنظيره في فاتورة البيع |
| 2 | نفسها | إضافة سطرين صنف (باركود + كمية + سعر) | يظهر «عدد الأصناف: 2 \| إجمالي الكمية: … \| الإجمالي: … \| المدفوع: … \| المتبقي: …» ويتحدث مع أي تغيير |
| 3 | نفسها | وضع «مرتجع شراء» ثم إضافة سطر | شريط الملخّص يعرض الإجمالي = مجموع السطور والمدفوع 0.00 |
| 4 | نفسها | فتح فاتورة مرحّلة/ملغاة | حقول «بيانات الفاتورة» و«الإجماليات» و«إضافة صنف» كلها معطّلة (رمزياً ونصياً) |
| 5 | نفسها | تغيير حجم النافذة | لا تداخل ولا تغطية: `gbTotals` أسفل، `lblQuickInfo` أسفلها، والشبكة تتمدد |

## القرارات والمخاطر

- **قرار المالك (2026-11-20):** «gbEntry «إضافة صنف» like frmAddUpdateSalesInvoice for Invoices screens … components order» — النطاق المختار: **محرّر المشتريات وحده** (POS له معياره §2.25).
- إزالة `pnlHeader`/`pnlFooter` تغيّر مرجعين في code-behind — يُعالجان في نفس الموجة.
- `ClientSize` يرتفع بـ43 بكسل ليبقى ارتفاع الشبكة كما هو بعد إضافة شريط الملخّص.
- `ctrlLineEntryPanel` مُدرج في `F-005` كـ`todo` — تغيير لونَي `gbEntry` هنا يخدم F-005 ولا يتعارض معها (نفس القيم المستهدفة).

## سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء المهمة بعد قياس المرجع والمحرّر |
