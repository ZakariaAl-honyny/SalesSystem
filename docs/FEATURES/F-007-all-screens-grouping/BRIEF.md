# F-007 — القالب الملزم لبرومبت كل وكيل

> يُنسخ هذا القالب في برومبت كل `ui-agent` مع تعبئة: اسم الشاشة · مسار المصمّم · جدول الكتل · الارتفاعات. لا يُرسل برومبت بلا هذه الحقول العشرة.

## الحقول الإلزامية

| الحقل | القيمة |
|---|---|
| `Goal` | إحاطة كل كتلة وظيفية في `<الشاشة>` بصندوق `GroupBox` عربي مُعنون على نمط `frmAddUpdateSalesInvoice` — بلا تغيير سلوكي |
| `Stage` | `—` (واجهة فقط — لا مسار بيانات، لا Mock، لا باك-إند) |
| `In scope` | **ملف واحد فقط**: `<مسار>.Designer.cs` |
| `Out of scope` | `.cs` · `.resx` · كل شاشة أخرى · المرجع · `Helpers/**` · `Controls/**` · `Program.cs` · `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `AGENTS.md` · `docs/**` |
| `Contract` | لا إعادة تسمية: كل الأسماء القائمة تعيش (خصوصاً `pnlFilters` + `gbFilter` — يتبنّاهما `FilterPanelHelper` بالاسم) |
| `Task record` | `docs/FEATURES/F-007-all-screens-grouping/` (`README` · `PLAN` · `MANIFEST` · `DECISIONS`) — يُقرأ ولا يُكتب |
| `Schema impact` | `none` |
| `Single home` | قيم المرجع حرفياً: `BackColor 170,220,170` · `ForeColor 33,37,41` · `Tahoma 10 Bold` · `Padding (6,4,6,4)` · `TabStop = false` — بصيغة `Convert.ToInt32` فقط (`RULE-N204`). لا Helper جديد ولا دالة جديدة |
| `Done when` | الصناديق معلنة بأسمائها وعناوينها في المصمّم، و`Controls.Add` بترتيب إرساء صحيح، والشبكة مرئية بالارتفاع المطلوب، وصفر تغيير في `.cs` |
| `Validation` | **لا تبنِ ولا تشغّل ولا تكتب اختبارات** — الـOrchestrator يبني (`RULE-N159`/`RULE-N117`) |

## النمط الملزم

```csharp
// 1) إعلان
private System.Windows.Forms.GroupBox gbFields;
// 2) إنشاء
gbFields = new GroupBox();
// 3) تهيئة — داخل InitializeComponent قبل الاستخدام
gbFields.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
gbFields.Controls.Add(pnlFields);
gbFields.Dock = DockStyle.Top;                 // Top / Bottom / Fill حسب الكتلة
gbFields.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
gbFields.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
gbFields.Name = "gbFields";
gbFields.Padding = new Padding(6, 4, 6, 4);
gbFields.Size = new Size(<عرض>, <ارتفاع المحتوى + 34>);
gbFields.TabIndex = <تبويب اللوحة المحتضاة>;
gbFields.TabStop = false;
gbFields.Text = "بيانات العميل";
gbFields.SuspendLayout();
```

- العنصر المحتضن: `Dock = Field` داخلياً، ويُحذف سطره من `Controls.Add` العلوي ويُستبدل بالصندوق **في نفس الموضع**.
- `pnlFilters` **لا يُحتضن** (يحتضن `gbFilter` ويغيّر `FilterPanelHelper` ارتفاعه أثناء الطيّ).
- شريط العنوان (`pnlHeader`/`lblMode`) وشريط الحالة (`lblQuickInfo`/`lblInvoiceSummary`) وطبقات الفراغ (`emptyState`/`lblNoData`) تبقى عارية (`D-01`/`F-006/D-03`).

## المصائد (مقاسة — تُنسخ في كل برومبت)

- `RULE-N125`: الإرساء يُحسب بعكس ترتيب `Controls.Add` — لا تُعِد ترتيب الأشقاء، استبدل في نفس الفتحة.
- `RULE-N137`: ممنوع صندوق داخل صندوق · ممنوع صندوق فارغ.
- `RULE-N145`: كل صندوق دائم يُعلَن في المصمّم — ممنوع إنشاؤه وقت التشغيل.
- `RULE-N128`: ممنوع أي إشارة إلى `Helpers/` داخل المصمّم.
- `RULE-N129`: لا تلمس عنصراً `BackColor.A < 255`.
- `RULE-N204`: لا `Color.FromArgb(a, b, c)` مختصر في المصمّم.
- `RULE-N179`: لا `Cells["<اسم منطقي>"]` — لا تلمس الشبكات.
- **قياس الكروم:** عنوان `Tahoma 10 Bold` + `Padding` يحجز **25–34** بكسل لا 22 (`F-006/D-07`).
- **`pnlFields`** تُستخدم نطاقاً لـ`KeyboardHelper.WireEnterAsTab` في 13 شاشتة — الاحتضان آمن، لكن لا تغيّر اسمها.

## قائمة الاختبار اليدوي (تُلحق بكل تقرير)

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `<الشاشة>` | افتحها | الترتيب من أعلى: العنوان ← الكتل بعناوينها العربية بالترتيب المطلوب |
| 2 | `<الشاشة>` | افحص كل صندوق | عنوان عربي واضح يقول ما البيانات ولماذا · لا صندوق فارغ |
| 3 | `<الشاشة>` | العمليات الأساسية (جديد/تعديل/حفظ/حذف/تصفية) | تعمل كما قبل — صفر تغيير سلوكي |
| 4 | `<الشاشة>` | صغّر النافذة إلى الحد الأدنى | لا تداخل ولا قصّ · الشبكة تبقى مقروءة |
| 5 | مصمّم VS | افتح `*.Designer.cs` | نفس الصناديق والعناوين والألوان كما في التشغيل (`RULE-N134`) |

**ذيل التقرير الإلزامي:** `لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)`
