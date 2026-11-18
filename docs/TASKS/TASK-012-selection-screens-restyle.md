# TASK-012 — تطبيق شكل `frmProductMaster` على شاشات `frm*Selection`

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-012` |
| `العنوان` | إعادة تنسيق المنتقيات الثلاث (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection`) على نمط `frmProductMaster` |
| `الحجم` | L — ثلاث شاشات كاملة، كل واحدة شريحة مستقلة لوكيل واحد (`RULE-N156`) |
| `المرحلة` | `—` — تعديل UI/UX على شاشات قائمة، **لا مسار بيانات جديد** ⇒ المرحلة الواحدة مستثناة من بوابة المرحلتين (`RULE-N165`) |
| `الحالة` | مفتوحة |
| `الوكيل المالك` | `ui-agent` — ثلاثة وكلاء متوازين، وكيل واحد لكل شاشة |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) — صفر ملف في `Models/**` · `DataAccess/**` · `Migrations/**` |
| `التاريخ` | `2026-11-18` |
| `طلب المالك` | «frm*Selection like frmProductMaster styling» + قراره في هذه الجولة: **«All three — restyle each»** و**«Full reference skeleton»** |

## 1. الهدف

تخرج المنتقيات الثلاث بنفس لغة `frmProductMaster` البصرية والإنشائية: شريط عنوان أخضر `lblMode` · شريط أوامر · صندوق «بحث وتصفية» `gbFilter` · شريط ملخص `pnlSummary` · شبكة بألوان الجيل الرابع · تخطيط `Dock` بدل الإحداثيات المطلقة · وصفر لون متقاعد أو بصيغة `FromArgb` مختصرة في المصمّمات.

## 2. خارج النطاق (ممنوع بالاسم)

- `SalesSystem/Business/**` · `SalesSystem/DataAccess/**` · `SalesSystem/Models/**` · `SalesSystem/DTO/**` · `SalesSystem.Desktop/Services/**` (بما فيها `Direct/**` و`Mock/**`) — الباك-إند مقفل (`RULE-N177`).
- `SalesSystem.Desktop/Helpers/**` — **ممنوع تعديل أي Helper**؛ كل ما تحتاجه الشاشات موجود ومُقيس في §4.
- `Program.cs` — التسجيلات الثلاثة قائمة و`frmProductSelection` مسجَّلة بالاسم المؤهَّل؛ **لا سطر جديد**.
- أي شاشة أخرى، وأي `frm*` غير المنتقيات الثلاث.
- **`AcceptButton` و`WireEnterAsTab`**: لا تحويل إلى الصيغة ثنائية الوسيط ولا حذف `AcceptButton` — التعميم مجمَّد بقرار `RULE-N184` وسطر «منتظرة الفتح #2» في `docs/FEATURES/INDEX.md` حتى شهادة `F-001`. المسموح: الإبلاغ عنه في المخاطر (§8).
- `AutoScaleMode` — يُترك كما هو في كل شاشة (تغييره يعرّض تخطيطاً يعمل لانحدار DPI بلا طلب).
- أي اسم عنصر قائم، وأي معالج حدث قائم، وأي عقد عام (`SelectedProduct` · `SelectedUnit` · `WarehouseId` · `SelectedCustomer` · `SelectedSupplier` · `DialogResult`).

## 3. الملفات (قائمة مغلقة — 6 ملفات، مالك واحد لكل ملف)

| # | الملف | الوكيل | العمل |
|---|---|---|---|
| 1 | `SalesSystem.Desktop/Screens/Products/Forms/frmProductSelection.cs` | `ui-agent` A | شكل + `btnClearFilters` + `FilterPanelHelper` |
| 2 | `SalesSystem.Desktop/Screens/Products/Forms/frmProductSelection.Designer.cs` | `ui-agent` A | الهيكل + ألوان الجيل الرابع |
| 3 | `SalesSystem.Desktop/Screens/Customers/Forms/frmCustomerSelection.cs` | `ui-agent` B | شكل + `lblMode` + `pnlSummary` + `btnClearFilters` |
| 4 | `SalesSystem.Desktop/Screens/Customers/Forms/frmCustomerSelection.Designer.cs` | `ui-agent` B | الهيكل + ألوان + **إصلاح صيغة `RULE-N204`** |
| 5 | `SalesSystem.Desktop/Screens/Suppliers/Forms/frmSupplierSelection.cs` | `ui-agent` C | شكل + `lblMode` + `pnlSummary` + `btnClearFilters` |
| 6 | `SalesSystem.Desktop/Screens/Suppliers/Forms/frmSupplierSelection.Designer.cs` | `ui-agent` C | الهيكل + ألوان + **إصلاح صيغة `RULE-N204`** |

`frm*.resx` لا يُمسّ (لا موارد جديدة).

## 4. العقود

**لا تغيير.** ما تقرأه الشاشات المستدعية ويجب أن يبقى حرفياً:

| الشاشة | الأعضاء العامة | المستهلكون (مقيسون) |
|---|---|---|
| `frmProductSelection` | `ProductDto? SelectedProduct` · `ProductUnitDto? SelectedUnit` · `short WarehouseId` · `DialogResult.OK/Cancel` | `frmAddUpdateSalesInvoice` (موضعان) · `frmOpeningStock` · `frmInventoryFifo` · `frmAddUpdateInventoryAdjustment` |
| `frmCustomerSelection` | `CustomerDto? SelectedCustomer` | لا مستهلك في الكود (مسجَّلة في DI فقط — `frmCustomerMaster` تفتح `frmFindCustomer`) |
| `frmSupplierSelection` | `SupplierDto? SelectedSupplier` | لا مستهلك في الكود (مسجَّلة في DI فقط) |

**البيت الواحد المستهلك (لا جديد، ولا نسخة ثانية):**

| البيت | الاستخدام في هذه الموجة |
|---|---|
| `Helpers/GreenThemeHelper.cs` | `ApplyHeader(lblMode)` · `ApplyGrid(dgv)` لكل شبكة · `TextStrong` لشريط الملخص · `Apply(this)` آخر سطر |
| `Helpers/ThemePalette.cs` | مصدر كل قيمة لون (الجدول §5.2) — لا لون مكتوب من خارج هذه القيم |
| `Helpers/FilterPanelHelper.cs` | `Attach(form, serviceProvider)` — يتبنّى `btnToggleFilters` **المُعلَن في المصمّم** (`RULE-N145`) |
| `Helpers/KeyboardHelper.cs` | الاستدعاءات الستة القائمة كما هي + إضافة `btnClearFilters` إلى `WireButtonDisabledVisual` |
| `Global Classes/clsFormat.cs` | `clsFormat.Number(...)` لعدّ النتائج |
| `Global Classes/clsDialogs.cs` | كل رسالة — لا `MessageBox` |

## 5. مواصفة الشكل الهدف (ملزمة للثلاثة — المصدر الوحيد)

### 5.1 الهيكل (شكل القائمة في `RULE-N79` + ترتيب §2.34)

كل شاشة تُبنى من ست مناطق بإرساء `Dock` فقط، و**بلا `Location` مطلق لأي عنصر دائم**:

| # | العنصر | النوع | الإرساء | الارتفاع/العرض | الألوان والخط |
|---|---|---|---|---|---|
| 1 | `lblMode` | `Label` | `Top` | h = 44 | خلفية `14,165,103` · نص أبيض · `Tahoma 14 Bold` · `TextAlign=MiddleCenter` |
| 2 | `pnlButtons` | `FlowLayoutPanel` | `Top` | h = 60 · `Padding 10` | خلفية `235,240,235` — بداخله `btnSelect` و`btnCancel` |
| 3 | `pnlFilters` | `Panel` | `Top` | h = 112 · `Padding 6,4,6,4` | بداخله `gbFilter` (Dock `Fill`، النص «بحث وتصفية»، `Padding 8`، `Tahoma 10`) و`btnToggleFilters` (Dock `Left`، 140×104، خلفية `248,249,250`، نص `14,82,52`، `FlatStyle=Popup`، `Tahoma 9 Bold`) |
| 4 | `pnlSummary` | `Panel` | `Bottom` | h = 32 | بداخله `lblRecordsCount` Dock `Fill` · `Tahoma 10 Bold` · نص `14,82,52` · `MiddleCenter` · النص «النتائج: N» |
| 5 | `pnlGrid` | `Panel` | `Fill` | — · `Padding 0,10,0,0` | يحتضن الشبكات وتسميات الحالة |
| 6 | النموذج | — | — | — | خلفية `170,220,170` · `Tahoma 10` |

**ترتيب `Controls.Add` على النموذج (إلزامي — `RULE-N125`):** `pnlGrid` ← `pnlSummary` ← `pnlFilters` ← `pnlButtons` ← `lblMode` (المضاف أخيراً يُرسى أولاً، فيأخذ الشريط أعلاه). القاعدة نفسها داخل `pnlGrid`: تُضاف العناصر بترتيب بصري معكوس (السفلي أولاً) فيظهر آخرها في الأعلى.

**داخل `pnlFilters`:** `pnlFilters.Controls.Add(gbFilter);` ثم `pnlFilters.Controls.Add(btnToggleFilters);` بهذا الترتيب حصراً (يبقى الزر مقتطعاً على اليسار ويملأ الصندوق الباقي — نفس ما يفعله `frmProductMaster`).

**داخل `gbFilter`:** `txtSearch` (خلفية بيضاء · `FixedSingle` · `Tahoma 10` · عرض ≈ 400) + `lblSearch` «بحث:» + `btnClearFilters` «مسح التصفية» (خلفية `108,117,125` · نص أبيض · `Flat` · `Tahoma 9 Bold` · `Enabled=false` في المصمّم).

**الشبكات:** كل شبكة داخل `pnlGrid` بإرساء (Top/Fill حسب الشاشة) — بلا `Location`/`Size` مطلقين.

**الشبكات الملحقة بـ`pnlGrid`:** `lblNoData` (و`lblNoUnits` و`lblSelectedProduct` في شاشة المنتجات) تُبقى معلنة، وتُرسى `Bottom` (h = 24) بخلفية `245,252,245` و`TextAlign=MiddleCenter`، و`lblSelectedProduct` تُرسى `Top` (h = 28) بـ`Tahoma 11 Bold` ونص `14,82,52`. **لا تُحذف ولا يُغيَّر اسمها** — الكود يبدّل `Visible` عليها.

**`ClientSize` المستهدف** (يُثبَّت حرفياً — النموذج `FixedDialog`): المنتجات `940×780` · العملاء `900×640` · الموردون `900×640`.

**`TabIndex` بترتيب بصري:** `txtSearch`=0 · `btnClearFilters`=1 · `btnToggleFilters`=2 · الشبكة الأولى=3 · `btnSelect`=4 · `btnCancel`=5. الشبكات تبقى `TabStop=false` حيث هي كذلك اليوم.

### 5.2 ألوان الشبكة (قيم حرفية = افتراضيات `ThemePalette` — `RULE-N134`/`RULE-N204`)

| الخاصية | القيمة |
|---|---|
| `BackgroundColor` | `220,240,220` |
| `GridColor` | `205,225,205` |
| `AlternatingRowsDefaultCellStyle.BackColor` | `245,252,245` |
| `ColumnHeadersDefaultCellStyle.BackColor` / `.SelectionBackColor` | `14,165,103` |
| `ColumnHeadersDefaultCellStyle.ForeColor` / `.SelectionForeColor` | `Color.White` |
| `ColumnHeadersDefaultCellStyle.Font` | `Tahoma 10 Bold` · `ColumnHeadersHeight = 45` |
| `DefaultCellStyle.BackColor` | `Color.White` |
| `DefaultCellStyle.ForeColor` / `.SelectionForeColor` | `33,37,41` |
| `DefaultCellStyle.SelectionBackColor` | `255,255,180` |
| `DefaultCellStyle.Font` | `Tahoma 10` · `RowTemplate.Height = 32` |
| `EnableHeadersVisualStyles` | `false` · `BorderStyle=None` · `RowHeadersVisible=false` |

**أزرار الأوامر:**

| الزر | خلفية | نص | ملاحظات |
|---|---|---|---|
| `btnSelect` («اختيار» / «تحديد») | `25,135,84` (`BtnSaveButtonColor` — الفعل الإثباتي) | أبيض | `Flat` · `BorderSize=0` · `Tahoma 10 Bold` · 118×40 · `Cursor=Hand` |
| `btnCancel` («إلغاء») | `108,117,125` (`BtnCancelButtonColor`) | أبيض | كما أعلاه |
| `btnClearFilters` | `108,117,125` | أبيض | `Tahoma 9 Bold` |
| `btnToggleFilters` | `248,249,250` | `14,82,52` | `FlatStyle=Popup` |

> **قاعدة الصيغة (`RULE-N204`):** كل لون في `*.Designer.cs` يُكتب
> `System.Drawing.Color.FromArgb(Convert.ToInt32(n), Convert.ToInt32(n), Convert.ToInt32(n))`، والألوان المسمّاة مؤهَّلة `System.Drawing.Color.White`.
> **ممنوعان:** `Color.FromArgb(248, 249, 250)` المختصر، و`((int)(((byte)(n))))`.
> **تصحيح بعد المراجعة (`RULE-N195`/`RULE-N196`):** `frmProductSelection.Designer.cs` **و`frmSupplierSelection.Designer.cs`** كانا يستوفيان الصيغة بالفعل (المورد: 20 استدعاء `FromArgb` كلها بصيغة `Convert.ToInt32`). **`frmCustomerSelection.Designer.cs` وحده** كان مخالفاً (11 موضعاً بالصيغة المختصرة + 7 ألوان مسمّاة غير مؤهَّلة) فحُوِّل بالكامل. الفجوة الحقيقية في المورد كانت **قيم جيل ثالث متقاعدة + تخطيط مطلق**، لا الصيغة.

### 5.3 كود الشاشة (`frm*.cs`)

1. `ApplyGreenTheme()` — الهيئة القياسية:
   ```csharp
   GreenThemeHelper.ApplyHeader(lblMode);
   GreenThemeHelper.ApplyGrid(dgvX);          // لكل شبكة في الشاشة
   lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;
   GreenThemeHelper.Apply(this);              // ← الأخير دائماً (RULE-N50)
   ```
2. `FilterPanelHelper.Attach(this, <provider>);` — **آخر سطر في المُنشئ العام**: `frmProductSelection` تُمرِّر خاصيتها `ServiceProvider`، والعميل/المورد تُمرِّران `_serviceProvider` (الأداة null-safe).
3. `btnClearFilters` — آلة حالة واحدة (`RULE-N148`):
   - `btnClearFilters.Enabled = !string.IsNullOrWhiteSpace(txtSearch.Text)` داخل `_ApplyFilter` (وفي شاشة المنتجات: أيضاً عند وجود عدّ تصفية نشط) — تُحدَّث بعد كل تغيير.
   - `Click` ⇒ تُفرَّغ `txtSearch` وحدها وتُعاد التصفية **بلا إعادة تحميل** ولا لمس للحالة المحددة.
   - تُضاف إلى `KeyboardHelper.WireButtonDisabledVisual(btnSelect, btnClearFilters)`.
4. عدّ النتائج: `lblRecordsCount.Text = $"النتائج: {clsFormat.Number(<count>)}";` بعد كل تحميل وبعد كل تصفية. (شاشة المنتجات: استبدال `lblRecordsCount.Text = clsFormat.Number(...)` القائمين — وهما موضعان).
5. **صفر تغيير** في: منطق التحميل، `RowFilterHelper`، الاختيار، `DialogResult`، رسائل `clsDialogs`، `ContextMenuStrip`، ترتيب `_AdvanceToUnitsAsync`/`_CommitFromUnitsAsync`.

## 6. الموجات

| الموجة | الوكيل | الملفات | البناء |
|---|---|---|---|
| W1 | `ui-agent` A · `ui-agent` B · `ui-agent` C (متوازية) | 1–2 · 3–4 · 5–6 | 0 أخطاء (يشغّله الـOrchestrator بعد الموجة) |

ثلاثة وكلاء ≤ أربعة (`RULE-N159`) · مجموعات ملفات منفصلة تماماً (`RULE-N158`) · لا وكيل يبني.

## 7. الأدلة (مقيسة في `c58c5b2` قبل الموجة — `RULE-N195`/`RULE-N196`)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| عدد المنتقيات | 3 | `Get-ChildItem -Recurse -Filter "frm*Selection*"` | 9 ملفات = 3 شاشات × (cs + Designer + resx) |
| أسطر الكود/المصمّم | `frmProductSelection` 580/434 · `frmCustomerSelection` 229/241 · `frmSupplierSelection` 229/240 | `(Get-Content $f).Count` | مرجع `frmProductMaster.cs` = **1,385** (مطابق لِما يسجّله `AGENTS.md` §2.39) |
| القيم المتقاعدة في المصمّمات | المنتجات: `52,58,64`×4 · `255,243,205`×2 · `248,249,250`×2 · `222,226,230`×2 · `33,37,41`×9 · `16,150,96`×1 · `240,242,245`×1 · `40,167,69`×1 · العملاء والموردون: نفس العائلة بلا `lblMode` | regex `FromArgb` مجمَّعاً | تقرير الوكيل قبل/بعد = صفر |
| صيغة `RULE-N204` | **المنتجات والمورد يستوفيانها قبل الموجة** (`FromArgb` بصيغة `Convert.ToInt32`: 100 و61 بعداً) · **العميل وحده كان مخالفاً**: 11 صيغة مختصرة + 7 ألوان مسمّاة غير مؤهَّلة ⇒ صفر الآن | `Select-String -Pattern "Color\.FromArgb\(\s*\d"` / `"Convert.ToInt32"` | بعد الموجة: مختصر = **0** في الثلاثة · `byteCast` = **0** · `Convert.ToInt32` = 100 / 67 / 61 |
| المستهلكون | المنتجات: 5 مواضع في 4 شاشات · العميل/المورد: **صفر** | `Select-String -Pattern "frmProductSelection\|frmCustomerSelection\|frmSupplierSelection"` | العميل والمورد مسجَّلان في `Program.cs` فقط ⇒ ميتان وظيفياً (يُبلَّغ، لا يُحذف) |
| الأداة المشتركة | `FilterPanelHelper.Attach(Form?, IServiceProvider?)` — null-safe، يتبنّى `btnToggleFilters` المُعلَن | `Helpers/FilterPanelHelper.cs:88` | `pnlFilters` + `gbFilter` + `btnToggleFilters` بأسماء ثابتة |
| البناء قبل الموجة | **0 Errors / 100 Warnings** | `dotnet build .\SalesSystem.slnx -p:ArtifactsPath=…` | نظيف، الشجرة نظيفة على `c58c5b2` |

## 8. المخاطر والقرارات

- **`Enter` داخل حقل البحث لا يصل إلى `KeyDown`** في الشاشات الثلاث لأن `AcceptButton` ينقره أولاً (`RULE-N193`) ⇒ المساران `txtSearch_KeyDown`/`KeyPress` (ومنهما `_AdvanceToUnitsAsync` من حقل البحث في شاشة المنتجات) **غير قابلين للتنفيذ اليوم**. **لا يُصلح في هذه الموجة** — التعميم مجمَّد بـ`RULE-N184` (سطر «منتظرة الفتح #2»). يُسجَّل بنداً مفتوحاً.
- **العميل والمورد ميتان وظيفياً** (لا مستهلك) ومُدرجان للاستبدال بـ`ctrl*CardWithFilter` بعد `F-002` (`docs/FEATURES/INDEX.md` سطر 3) ⇒ إعادة تنسيقهما استثمار قصير الأجل بقرار المالك الصريح في هذه الجولة.
- **`btnSelect` كان `40,167,69`** — لون خارج `ThemePalette` ⇒ يُعيَّن إلى `BtnSaveButtonColor` (`25,135,84`) لأنه الفعل الإثباتي، واللقب «حفظ» في اللوحة هو أقرب مفتاح دلالي.
- **`AutoScaleMode` يبقى كما هو** (`None` في الثلاث) رغم أن المرجع `Font` — تغييره بلا طلب يعرّض تخطيطاً عاملاً لانحدار DPI.
- **ملاحظة قياس خارج النطاق:** `frmProductMaster.Designer.cs` يحمل 936 سطراً بفواصل `\r\r\n` (CR مزدوج) فيتضاعف عدّ `Get-Content` إلى 1,873 — عيب صيغة ملف في المرجع نفسه، لا يُمسّ في هذه الموجة ويُبلَّغ للمالك.
- **ملاحظة قياس خارج النطاق:** افتراضي `rejectBackgroundColor` في `ThemePalette.cs` هو `255,220,220` بينما جدول §2.19 في `AGENTS.md` يقول `#F8D7DA` = `248,215,218` ⇒ فرق وثيقة/كود يُبلَّغ ولا يُغيَّر هنا.

## 9. الاختبار اليدوي

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | الثلاث | افتح الشاشة | شريط عنوان أخضر `14,165,103` بنص أبيض، خلفية النموذج خضراء فاتحة، لا رمادي في أي سطح |
| 2 | الثلاث | انظر إلى ترتيب الكتل من الأعلى | العنوان ← الأوامر ← «بحث وتصفية» ← الشبكة ← «النتائج: N» |
| 3 | الثلاث | اكتب في حقل البحث | تصفية فورية، العدّ يتغير، ويُفعَّل «مسح التصفية» |
| 4 | الثلاث | اضغط «مسح التصفية» | يعود البحث فارغاً والشبكة كاملة **بلا إعادة تحميل** والزر يعود معطّلاً |
| 5 | الثلاث | اضغط «إخفاء التصفية» / `F9` | تُطوى اللوحة إلى 34 وتُستعاد، والحالة تُحفظ بعد إغلاق الشاشة وإعادة فتحها |
| 6 | الثلاث | نفّذ الاختيار ثم أعد فتح الشاشة | `DialogResult.OK` والقيمة المختارة كما قبل التعديل (لا كسر عقد) |
| 7 | المنتجات | اختر صنفاً ثم وحدة | شبكة الوحدات تُحمَّل، «المنتج المحدد: …» يظهر، والسعر منسَّق `N2` |
| 8 | المنتجات | اختر وحدة بكمية صفر | رسالة الرفض العربية تظهر ولا يُغلق النموذج (`RULE-N167` سلوك قائم) |
| 9 | الثلاث | `Esc` | يُلغي ويغلق |
| 10 | الثلاث | افتح المصمّم في VS وقارن بالتشغيل | نفس الألوان (`RULE-N134`)، وصفر `FromArgb` مختصر في المصمّم (`RULE-N204`) |

## 10. المراجعة والنتيجة المقيسة

**`code-reviewer` (مرور واحد على الشريحة المنتهية — `RULE-N162` بوابة 3):** `0 Blocker · 0 Major · 4 Minor` — أُغلقت الأربعة:

| # | الملاحظة | الحالة |
|---|---|---|
| 1 | سجل المهمة ادّعى أن المورد كان «صفر `Convert.ToInt32`» — **غير صحيح** (كان 20 `FromArgb` بصيغة معتمدة) | ✅ صُحّح §5.2 و§7 |
| 2 | `FlatAppearance.MouseOverBackColor` غائب في مصمّم المورد بينما هو في الشقيقتين والمرجع | ✅ أُضيف `(240,240,240)` بنفس الصيغة |
| 3 | `lblNoData.ForeColor` في المورد `33,37,41` بدل `14,82,52` في الشقيقتين | ✅ وُحِّد إلى `14,82,52` |
| 4 | لا `ToolTip` في مصمّم المورد بينما الشقيقتان تحملانه | ✅ أُعلن `tooltip` مربوطاً بـ`components` + 6 تلميحات |

**البناء بعد الموجة (`--no-incremental`):** **0 Errors / 90 Warning(s)** ← الأساس قبل الموجة **0 Errors / 100 Warning(s)**. لا تحذير جديداً في ملفات الشريحة: التحذيرات الوحيدة التي تسمّي الملفات الستة هي `frmProductSelection.cs:42` CS8618 ×3 و**سابقة للموجة** (قائمة في `HEAD` على نفس السطر ونفس الحقول الثلاثة). فرق `89 → 90` لا يعود إلى هذه الشريحة (تسمية تحذيرات الملفات الستة لم تتغير بين القياسين) — الشجرة تحمل في الوقت نفسه تعديلات جلسة موازية خارج نطاق المهمة.

**قياس المصمّمات بعد الموجة (الأمر: regex على `FromArgb`):**

| المقياس | المنتجات | العميل | المورد |
|---|---|---|---|
| صيغة مختصرة `Color.FromArgb(a, b, c)` | 0 | 0 | 0 |
| `((int)(((byte)(n))))` | 0 | 0 | 0 |
| `Location` مطلق | 3 (داخل `gbFilter` فقط) | 3 (داخل `gbFilter` فقط) | 3 (داخل `gbFilter` فقط) |
| قيم الجيل الثاني/الثالث المتقاعدة | 0 | 0 | 0 |
| `pnlFilters` · `gbFilter` · `btnToggleFilters` · `pnlSummary` · `lblRecordsCount` · `pnlGrid` · `lblMode` | كلها معلنة | كلها معلنة | كلها معلنة |

## 12. تعديل قرار المالك (2026-11-18) — الهيكل الهدف صار `frmListSales` لا `frm*Master`

**نص المالك:** «Returns UI & UX the same like `frmAddUpdateSalesInvoice` & `frmListSales` not like `frm*Master` … so now rewrite code the same like …».

**الأثر على هذه المهمة:** §5 أعلاه (هيكل الماستر: `lblMode` + `pnlButtons` + `pnlFilters` + `pnlSummary` + `pnlGrid`) **متجاوَز** للمنتقيات الثلاث. الهيكل المعتمد صار هيكل القائمة المقيس من `frmListSales.Designer.cs`:

| العنصر | الإرساء | الارتفاع | الأبناء |
|---|---|---|---|
| `pnlHeader` | Top | 56 | `lblMode` (Fill) |
| `pnlToolbar` | Top | 64 | `flpToolbar` → `btnSelect` + `btnCancel` |
| `pnlFilters` | Top | 110 | `gbFilter` (Fill، 102) + `btnToggleFilters` (Left، 140×102) |
| `pnlSummary` | Bottom | 46 | `flpSummary` → `lblRecordsCount` |
| `pnlFooter` | Bottom | 58 | `lblHint` (تلميح لوحة المفاتيح) |
| `pnlGrid` | Fill | — | الشبكات + حالات الفراغ |

`Controls.Add`: `pnlGrid` ← `pnlSummary` ← `pnlFooter` ← `pnlFilters` ← `pnlToolbar` ← `pnlHeader` (نفس ترتيب المرجع) · `pnlButtons` **حُذف نهائياً**.

**ما نُفِّذ في هذه الموجة:** المصمّمات الثلاثة أُعيد بناؤها على الهيكل أعلاه **بلا أي تغيير في الكود الخلفي** (`git diff --numstat` للـ`.cs` = لا سطر) — لأن إعادة التسمية لم تمسّ أي عضو يستدعيه الكود؛ وبقيت `WireSearchToGrid` وتركيز `txtSearch` عند الفتح (`TASK-015`) و`FilterPanelHelper.Attach` آخر سطر و`ApplyGreenTheme` بهيئتها.

**البناء:** **0 Errors / 72 Warning(s)** · **المراجعة:** لم تُعَد بعد على هذا الهيكل (**بند مفتوح**).

**أثر خارج هذه المهمة:** الوحدات التي حوّلتها `F-004` إلى `frm*Master` (`frmSalesReturnMaster` · `frmPurchaseReturnMaster` · `frmCustomerReceiptsMaster` · `frmExpensesMaster`) صارت **مخالفة لقرار المالك هذا** — تحتاج قراراً وإعادة بناء على نمط `frmList*` + `frmAddUpdate*`. تُسجَّل ولا تُنفَّذ هنا.

## 13. سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-18` | إنشاء المهمة بعد قياس الشجرة على `c58c5b2` وبناء أساسي **0 أخطاء / 100 تحذير** |
| `2026-11-18` | موجة W1: ثلاثة وكلاء متوازين (شاشة لكل وكيل) + `fast-agent` واحد لإغلاق ملاحظات المراجعة الأربع · بناء `--no-incremental` **0 أخطاء / 89 تحذيراً** |
