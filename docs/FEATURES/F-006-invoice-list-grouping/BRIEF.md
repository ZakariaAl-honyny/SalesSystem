# F-006 — BRIEF (الموجز الملزم لكل وكيل)

> يُقرأ مع `README.md` في هذا المجلد. الشاشة المرجعية: `Screens/All Invoices Screens/Sales/frmAddUpdateSalesInvoice.cs` (+ مصمّمها).

## الحقول الإلزامية العشرة

| الحقل | القيمة |
|---|---|
| `Goal` | إحاطة كل كتلة وظيفية في الشاشة المذكورة بصندوق `GroupBox` عربي مُعنون، على نمط `frmAddUpdateSalesInvoice`، بلا أي تغيير سلوكي |
| `Stage` | `—` — واجهة فقط، ليست مسار بيانات ⇒ لا Stage A/B ولا بوابة Mock |
| `In scope` | **قائمة مغلقة**: `frm*.Designer.cs` للشاشة المذكورة في برومبت الوكيل فقط |
| `Out of scope` | `.cs` للشاشة (إلا إذا انكسر البناء — يُبلَّغ ولا يُعدَّل بلا إذن) · `resx` · المرجع · `frmPOS` · `frmAddUpdatePurchaseInvoice` · `frmPurchaseInvoicePrint` · كل `frm*Report` · `Helpers/**` · `Program.cs` · `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `AGENTS.md` · `docs/**` |
| `Contract` | **لا تغيير**: أسماء كل العناصر تبقى كما هي (`pnlHeader`, `pnlToolbar`, `flpToolbar`, `pnlFilters`, `gbFilter`, `btnToggleFilters`, `pnlSummary`, `flpSummary`, `pnlFooter`, `pnlGrid`, `tabReturns`, `dgv*`, `emptyState`, `lblNoData`, `lblInvoiceSummary`) |
| `Task record` | `docs/FEATURES/F-006-invoice-list-grouping/README.md` + `BRIEF.md` + صفّك في `MANIFEST.md` — **اقرأها أولاً ولا تكتب فيها**؛ أعِد فروقك في تقريرك |
| `Schema impact` | `none` |
| `Single home` | الألوان والخط من قيم المرجع الحرفية عبر `GreenThemeHelper`/`ThemePalette` — **لا قيمة لون جديدة** · لا Helper جديد · لا دالة جديدة |
| `Done when` | الصناديق المذكورة معلنة في المصمّم بأسمائها وعناوينها العربية وقيم المرجع، و`Controls.Add` بترتيب إرساء صحيح، والشبكة مرئية بارتفاع كافٍ، وصفر تغيير في `.cs` |
| `Validation` | **لا تبنِ ولا تشغّل ولا تكتب اختبارات** — الـOrchestrator يبني بعد الموجة (`RULE-N159`/`RULE-N117`) |

## نمط التنفيذ الملزم (لكل صندوق)

```csharp
// 1) إعلان جديد في منطقة الإعلانات
private System.Windows.Forms.GroupBox gbToolbar;
// 2) التهيئة
gbToolbar = new GroupBox();
// 3) داخل InitializeComponent — قبل استخدامه
gbToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
gbToolbar.Controls.Add(pnlToolbar);
gbToolbar.Dock = DockStyle.Top;
gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
gbToolbar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
gbToolbar.Name = "gbToolbar";
gbToolbar.Padding = new Padding(6, 4, 6, 4);
gbToolbar.Size = new Size(<عرض>, <ارتفاع اللوحة + 22>);
gbToolbar.TabIndex = <تبويب مناسب>;
gbToolbar.TabStop = false;
gbToolbar.Text = "أوامر الشاشة";
```

- اللوحة المحتضاة: `Dock = DockStyle.Fill` **داخلها**، وخارج الصندوق لا تُضاف إلى `Controls.Add(this)`.
- **الترتيب**: `Controls.Add(this.gbX)` يحل مكان `Controls.Add(this.pnlX)` في **نفس الموضع** من كتلة `Controls.Add` (RULE-N125 — لا تُعِد ترتيب بقية العناصر، ولا تدفع عنصر `Fill` إلى آخر المجموعة).
- الصناديق: `BackColor 170,220,170` · `ForeColor 33,37,41` · `Tahoma 10 Bold` · `Padding (6,4,6,4)` — قيم المرجع حرفياً.
- `Size` ارتفاعه = ارتفاع اللوحة المحتضاة + **22** (ارتفاع عنوان الصندوق). `ClientSize` **لا يتغيّر**؛ الشبكة تتناقص بمقدار عناوين الصناديق المضافة فقط.
- لا تستخدم الشكل المختصر `Color.FromArgb(170, 220, 170)` — **`RULE-N204`** يفرض `Convert.ToInt32`.

## المصائد (إلزامية — مُقاسة في هذا المستودع)

- `RULE-N125`: لا تُعِد ترتيب `Controls` لعنصر مراسٍ — ترتيب الإرساء يتحدد بعكس ترتيب الإضافة.
- `RULE-N137`: ممنوع صندوق داخل صندوق · وممنوع صندوق فارغ.
- `RULE-N145`: كل عنصر دائم يُعلَن في `*.Designer.cs` — ممنوع إنشاء صندوق وقت التشغيل.
- `RULE-N128`: ممنوع أي إشارة إلى `Helpers/` داخل المصمّم.
- `RULE-N129`: لا تلمس عناصر `BackColor.A < 255` (طبقات التحميل الشفافة).
- `RULE-N179`: لا `Cells["<اسم منطقي>"]` — لا علاقة لهذه الموجة، لكن لا تلمس الشبكات.
- `emptyState` / `lblNoData` / `lblInvoiceSummary` تُترك في مواضعها الحالية **خارج** الصناديق (شريط حالة/طبقة فراغ — سابقة `lblQuickInfo` في المرجع)، وسلوك `BringToFront` في `.cs` يبقى سليماً.
- `pnlFilters` و`gbFilter` **لا يُلمسان**: `FilterPanelHelper` يتبنّاهما بالاسم ويغيّر ارتفاع `pnlFilters` أثناء الطيّ.

## التقاطعات

- لا ملف يملكه وكيلان في نفس الموجة (`RULE-N158`).
- `F-005` (السطح الأخضر) **مجمَّدة على هذه الملفات الخمسة** أثناء هذه الموجة — لا لون خارج قيم المرجع.
- `TASK-023` (نقل مجلد الفواتير) **منتهية النقل** — لا `git mv` هنا.

## المراجعة والتسليم

- الـOrchestrator يبني بعد الموجة؛ الخطأ يُوجَّه لمالك الملف، لا يُصلحه وكيل آخر.
- مراجعة واحدة `code-reviewer` على الشريحة الكاملة.
- تقرير الوكيل: الملفات · القرارات · الأرقام المقيسة (ارتفاع الشبكة قبل/بعد · عدد الصناديق · صفر تغيير في `.cs`) · المخاطر · `لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)`.
