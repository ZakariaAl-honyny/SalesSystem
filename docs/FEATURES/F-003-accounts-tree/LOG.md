# F-003 — LOG

## الجولة 1 — 2026-11-17 — العنصر + النافذة + التسجيل

**نصّ المالك:** «I want to Make ctrlTreeOfAccounts for frmAccounts for Helpe account as tree and CRUD on any account find the for show account as tree and select any account easly».

| البند | القياس |
|---|---|
| ملفات جديدة | **6**: `Screens/Accounting/Controls/ctrlTreeOfAccounts.{cs,Designer.cs,resx}` (760 · 469 · 75 سطراً) + `Screens/Accounting/Forms/frmAccountsTree.{cs,Designer.cs,resx}` (99 · 91 · 69) |
| العقد المستهلك | `IAccountDesktopService.GetTreeAsync` → `AccountTreeNodeDto` (`Children` هرمية) · `GetByIdAsync` (لحكم `IsSystem`) · `DeleteAsync` · `PermanentDeleteAsync` |
| البحث | «الكود»/«الاسم» + تصفية فورية (250 ms) مع **إبقاء الأسلاف** وتوسيع النتيجة · بلا مطابقة ⇒ عقدة واحدة «لا توجد حسابات مطابقة» |
| الحذف النهائي | معطّل لحساب نظامي · معطّل لحساب له أبناء (من الشجرة الكاملة لا المقلَّمة) · **فشل مغلق** إن تعذّر التحقق من بيانات الحساب · تأكيد أقوى افتراضه «لا» |
| آلة الحالة | `_ApplyActionState()` واحدة تُستدعى بعد كل تغيير (`RULE-N150` نظير) · القائمة والأزرار تنادي نفس المعالجات |
| `RULE-N205` | **4** حدود فقط: `RefreshAsync` · `_EnsureAccountInfoAsync` · معالجا الحذف — و`finally` لإعادة المؤشر والحالة |
| الالتزامات | 70 + 13 `Convert.ToInt32` في المصمّمين · **0** صيغة مختصرة · **0** `MessageBox` · **0** `ex.Message` · `GreenThemeHelper.Apply(this)` آخر سطر · WFO1000 على الخصائص العامة |
| التسجيل | `Program.cs`: `AddTransient<frmAccountsTree>()` + `AddTransient<…ctrlTreeOfAccounts>()` (`RULE-N67`) · `frmMain`: بند «شجرة دليل الحسابات» بعد «دليل الحسابات» |
| البناء | **Build succeeded · 0 Error(s)** |

### قيود نطاق مُعلنة (لم تُنفَّذ — ولم تُخفَ)

1. **الدمج داخل `frmAccounts` مؤجَّل**: الشاشة ومصمّمها مملوكان لـ`TASK-008` المفتوحة (`docs/TASKS/ACTIVE.md` صف 3) — ملف واحد = مالك واحد لكل موجة (`RULE-N158`). يُنفَّذ بعد إقفالها.
2. **استهداف حساب بالمعرّف عند الفتح**: لا تُعرّض `frmAccounts` أي `SelectById`/`LoadAccount(id)`؛ الحدثان `AddChildRequested`/`EditRequested` يفتحان الشاشة فقط. **بند مفتوح** يُغلق مع اندماج الشجرة.
3. `Program.cs` + `frmMain*` **لم تُدرَج في كومِت هذه الشريحة** لأنها تحمل تغييرات مهام أخرى مفتوحة في الشجرة نفسها — تعمل في شجرة العمل وتُكمَل عند تنظيفها.

### بنود اختبار (15) — ملخّص

تحميل الشجرة · عقد مجموعة عريضة/ورقية عادية · توسيع/طيّ · بحث بالكود وبالاسم (مع الأسلاف) · لا مطابقة ⇒ عقدة باهتة · اختيار حساب يُفعّل «تعديل/حذف/إضافة فرعي» · رفض الحذف النهائي (نظامي · له أبناء) · إلغاء التأكيد لا يغيّر شيئاً · حذف ناجح يُحدّث الشجرة · «إضافة/تعديل» يفتحان `frmAccounts` بلا إغلاق الشجرة · قائمة السياق بنفس الحالات · «تحديث» يستعيد الاختيار · المصمّم = التشغيل بلا رمادي.

---

## الجولة 2 — 2026-11-17 — شريط CRUD مشترك + تنسيق شاشة المرجع

| البند | ما جرى |
|---|---|
| `Controls/Common/ctrlCrudToolbar` | **جديد (3 ملفات)**: أزرار `+ إضافة` (25,135,84) · `تعديل` (255,193,7 بنص داكن) · `حفظ` (مخفي افتراضياً) · `إلغاء التعديل` · `حذف` (220,53,69) · `تحديث بيانات` · `خروج` — بارتفاع 40 وخط `Tahoma 10 Bold` **مطابقة لصورة `frmAddUpdateSalesInvoice`** · `CrudMode { Browsing, Adding, Editing }` · `HasSelection` · `AllowDelete` · `ShowSave` · سبعة أحداث · **صفر منطق بيانات** |
| `ctrlTreeOfAccounts` | أُعيد تنسيقه: «أوامر الحسابات» (الشريط + صف ثانوي: توسيع/طيّ/حذف نهائي) · «بحث وتصفية» · «شجرة الحسابات» · «ملخص» — والتفعيل كله من `_ApplyActionState()` وحدها |
| `frmAccountsTree` | شريط عنوان زمردي مسطّح كشاشة المرجع |

## الجولة 3 — 2026-11-17 — محرّر الحساب كعنصر تحكم

`Screens/Accounting/Controls/ctrlAccountEditor` (**جديد 3 ملفات**): الحساب الأب · اسم الحساب (مطلوب عبر `RequiredFieldHelper`) · الاسم الأجنبي · رقم الحساب (للقراءة فقط) · نوع الحساب · الحساب الختامي (`NormalBalance`: تلقائي/مدين/دائن) · «حساب ورقى» · «حساب نظامي» · «يسمح بمركز تكلفة» · وصف · ملاحظات.
- **قراءات فقط**: `GetAllAsync` (الآباء، مع استبعاد الحساب وفروعه) و`GetNextChildCodeAsync` (معاينة الكود) — `grep CreateAsync|UpdateAsync|DeleteAsync` في العنصر = **0**.
- `SetMode(CrudMode)` + `LoadAccount` + `ResetEditor(parentId)` + `TryBuildCreate/Update` + `SaveRequested/CancelRequested`.

## الجولة 4 — 2026-11-17 — دمج العناصر الثلاثة داخل `frmAccounts` + ترتيب الشجرة بالطبائع

**بموافقة المالك على تجاوز ملكية الملف** (الشاشة كانت مملوكة لـ`TASK-008` المفتوحة؛ نصّ الأمر: «as controls and use them frmAccounts … and use them like Image inside frmAccounts») — موجبة واحدة، مالك واحد، والملف اختُصر: `frmAccounts.cs` **1396 → 1109** سطراً · مصمّمه **1053 → 739**.

| البند | النتيجة |
|---|---|
| التخطيط | العنوان ← «أوامر الحسابات» (شريط CRUD) ← «بحث وتصفية» ← [شجرة \| محرّر] ← «بيانات الحسابات» ← «الإجماليات» — بلا صندوق داخل صندوق (`RULE-N137`) |
| الربط | شجرة ⇄ شبكة ⇄ محرّر عبر دالة واحدة `_OnRecordSelected(id)` · الحفظ يقرأ الطلب من `ctrlAccountEditor.TryBuild*` · `_ApplyModeState()` وحدها تضبط `Enabled/Visible` |
| المحذوف (مع دليل `grep` = 0 لكل اسم) | 21 حقلاً + 6 أزرار + `cmsTree` + `errorProvider` + `liveTip` + 15 دالة/حالة قديمة انتقلت ملكيتها للعناصر |
| عيوب البناء التي وُلدت وأُغلقت | `CS0246` (`using SalesSystem.DTO.Requests`) و`CS1503` (تدفّق النضد للطلبات) — أُصلحت في ملفها بحدّها الواحد |
| **ترتيب الشجرة بالطبائع** | المستوى الأول = **خمس عقد ثابتة**: «الأصول (1) — N حساباً» · «الخصوم (2)» · «حقوق الملكية (3)» · «الإيرادات (4)» · «المصروفات (5)» (عريضة، أخضر داكن، تعدّ فروعها) · تحتها الهيكل الحقيقي بأي عمق · `NatureSelected` + `GetSubtreeIds(id)` + `GetNatureIds(nature)` · اختيار طبيعة ⇒ `AccountSelected(null)` · حارس **«لا إضافة تحت حساب حركة نهائي»** |
| البناء | **Build succeeded · 0 Error(s)** |

**سلوكيات فُقدت عن الشاشة القديمة (مُعلنة، لا مخفية):** إدخال «الرصيد الافتتاحي» من المحرّر (يبقى من `frmOpeningStock`/القيد الافتتاحي) · `Ctrl+S` (لا وصول لزر الحفظ داخل الشريط) · `Enter` لا ينقر زر وضع لأن `AcceptButton = null` بقصد (منع حفظ صامت) · بند «عرض الشجرة» النصي (الشجرة على الشاشة الآن).

**قواعد «الخلفية» التي لا تُكتب في الواجهة (`RULE-N177`):** توليد `AccountCode` · `Level = parent.Level + 1` · اشتقاق `FinancialStatement` من `Nature` · افتراضي `IsSystem` · `CreatedByUserName/CreatedAt` من الجلسة · حارس التزامن على توليد الكود — بيتها `AccountCodeGeneratorService`/`AccountService`، والواجهة **تعرض فقط** ما يعيده `GetNextChildCodeAsync`.

---

## الجولة 5 — 2026-11-17 — تحقّق «المنطق الخلفي» + إظهار ما يحسبه النظام للقراءة فقط

**طلب المالك:** شرح خمس قواعد خلفية (توليد الكود · المستوى · القائمة المالية · `IsSystem` · التدقيق) + حارس التزامن + تحقّق صامت يمنع التفريع تحت حساب حركة.

### التحقّق بالقياس (لا بالذاكرة — `RULE-N195`/`N196`)

| القاعدة | الحالة | الدليل (ملف:سطر) |
|---|---|---|
| توليد `AccountCode` بعروض المستوى (2 ⇒ رقم · 3 ⇒ رقمان · 4 ⇒ 4 أرقام · أعمق ⇒ زيادة) | ✅ **منفّذة** | `Business/Services/AccountCodeGeneratorService.cs:11-15, 31-101, 109` (`FormatChildCode`) |
| **حارس التزامن** على التوليد | ✅ **منفّذ** | نفس الملف:22 `SemaphoreSlim(1,1)` + `WaitAsync`/`Release` |
| `Level = parent.Level + 1` (الجذر = 1) ورفض > 10 | ✅ | `AccountService.cs:165-166` (إنشاء) · `311-312` (تعديل) |
| `FinancialStatement` مشتق من `Nature` (1..3 ⇒ ميزانية · 4..5 ⇒ دخل) | ✅ | `Models/Accounting/Account.cs:82` — خاصية محسوبة `Nature <= 3 ? 1 : 2` |
| `IsSystem = 0` لكل حساب يُنشئه المستخدم | ✅ | `AccountService.cs:193` — `isSystem: false` مفروض (يتجاهل الطلب) |
| بيانات التدقيق (`CreatedByUserName`/`UpdatedByUserName`) من الجلسة | ✅ | `DataAccess/SalesDbContext.cs:103-153` — ختم مركزي في `SaveChanges` من `ICurrentUserProvider` (لا في الشاشة ولا في الخدمة) |
| **تحقّق صامت:** رفض التفريع تحت حساب حركة (`IsLeaf = 1`) | ✅ خلفياً + ✅ واجهياً | `AccountService.cs:116, 147` (معاينة/إنشاء) · `280` (تعديل) — ورسالة عربية في الشجرة والمحرّر |

**النتيجة: الخمسة كلها + حارس التزامن + التحقق الصامت منفَّذة فعلاً.** لم أكتب سطر باك-إند واحد (`RULE-N178`: لا مسّ لما يعمل).

### الناقص الحقيقي (واحد فقط)

**الواجهة كانت لا تُظهر ما قرّره النظام.** أُضيف بلوك **«محسوب تلقائياً بواسطة النظام»** داخل `ctrlAccountEditor` (4 صفوف للقراءة فقط): **المستوى** (في الإضافة = مستوى الأب + 1) · **القائمة المالية** (مرآة عرض لقاعدة الكيان، تقرأ `Nature` في الإضافة و`FinancialStatement` في التعديل) · **حساب نظامي** · **تاريخ الإنشاء ومُنشئه** (صف معطّل بتلميح — لأن `AccountDto` لا يحمل حقول التدقيق؛ ولم نُضفها للعقد، الباك-إند مجمَّد).

**البناء:** `Build succeeded · 0 Error(s)`.

**بند مفتوح (تغطية الوضع التجريبي):** حكم `IAccountDesktopService` في `docs/MOCK-COVERAGE.md` = **`مؤهل جزئياً — تكافؤ الرفض ناقص`** (`ف-AC-5`…`ف-AC-9`). هذه ليست أعطال منطق خلفي بل **تكافؤ Mock** (رفض الكود المكرر/الورقة كأب/الحد الأقصى للعمق في `MockAccountDesktopService` وبياناته) — تُغلق في شريحة Mock خاصة، وهي شرط أي عمل باك-إند مستقبلي على الوحدة.

---

## الجولة 6 — 2026-11-17 — الشجرة عرض فقط: بلا أزرار وبلا تصفية

**نصّ المالك:** «no this for view tree of account no buttons or filter for show accounts on tree shap» — ثم «I use it inside frmaccounts».

| البند | ما جرى |
|---|---|
| `ctrlTreeOfAccounts` | أُعيد بناؤه **عرضاً شجرياً للقراءة فقط**: صندوق واحد «شجرة الحسابات» بداخله `tvAccounts` يملؤه (`Dock=Fill`). **حُذف**: `ctrlCrudToolbar` · صف توسيع/طيّ/حذف نهائي · «بحث وتصفية» (`cbSearchBy`/`txtSearch`/`searchTimer`) · `pnlSummary`/`lblSummary` · `cmsAccounts` · `toolTip1` |
| الواجهة العامة | بقيت: `Attach` · `RefreshAsync` · `SelectAccount` · `SelectedAccountId` · `SelectedAccount` · `AccountSelected`. **حُذفت** (صفر مستهلك بعد الحذف): `ShowCrudActions` · `AddChildRequested` · `EditRequested` · `NatureSelected` · `GetSubtreeIds` · `GetNatureIds` · `ExpandAll` · `CollapseAll` |
| المستوى الأول | خمس عقد ثابتة (أصول 1 · خصوم 2 · حقوق ملكية 3 · إيرادات 4 · مصروفات 5) بنص «الاسم (N) — عدد الحسابات»، وتحت كل واحدة هرمية الحسابات الكاملة بأي عمق — بلا حد للعمق وبلا تصفية |
| `frmAccounts` | حُذف توصيل `Tree.AddChildRequested`/`Tree.EditRequested` ومعالجاهما، وسطر `ShowCrudActions = false` من المصمّم — الشجرة تُبلّغ عن الاختيار فقط، والأوامر من شريط الشاشة |
| `frmAccountsTree` | صار **نافذة عرض مستقلة** فقط: بلا فتح `frmAccounts` من الشجرة، وبلا `using` لم تعد لازمة |
| القياس | `ctrlTreeOfAccounts.cs` = **392** سطراً (كان 1011) · المصمّم **69** (كان 428) · `resx` **66** (كان 76) · `grep "ctrlCrudToolbar|txtSearch|cbSearchBy|lblSummary|ContextMenu|DeleteAsync|PermanentDelete"` داخل العنصر = **0** |
| البناء | `dotnet build SalesSystem.Desktop.csproj` = **0 Error(s)** |
| خارج النطاق | `ctrlAccountEditor*` مملوك لشريحة جارية لمالك آخر — **لم يُمسّ** في هذه الجولة (التعديل العابر أُعيد إلى حالته) |

**اختبار يدوي مختصر:** افتح «دليل الحسابات» ⇒ الشجرة يسار بلا أزرار/تصفية · تظهر العقد الخمس أعلى الشجرة · وسّع «الأصول (1)» ⇒ الحسابات وفروعها بأي عمق · اختر حساباً ⇒ يُعبَّأ المحرّر يميناً ويُحدَّد سطره في الشبكة · طيّ/توسيع بقائمة `+`/`−` · إعادة فتح `frmAccountsTree` ⇒ نفس الشجرة بلا أي أمر.

---

## الجولة 7 — 2026-11-17 — الشجرة تُصفّي الشبكة (طلب المالك في `docs/acounts.md`)

**المصدر:** «بمجرد النقر على أي عقدة … **وفي نفس الوقت** تتفاعل مع جدول العرض لتصفية أو التركيز على الحسابات التابعة لتلك العقدة». قبل هذه الجولة كان الاختيار يفتح السجل فقط، والشبكة لا تتغيّر — **فجوة مقيسة** (`frmAccounts.cs:838` ⇒ `_OnRecordSelected` وحده).

| البند | ما جرى |
|---|---|
| `ctrlTreeOfAccounts` (البيت الواحد للهيكل) | **أُعيد** ما حُذف في الجولة 6 بعد أن ظهر مستهلك حقيقي له: `GetSubtreeIds(int)` · `GetNatureIds(byte)` · زائد `SelectedNature` (`byte?`) و`ClearSelection()` — والمشي على `_roots` (شجرة الخدمة) لا على `TreeView`، بدالة تجميع واحدة `_CollectIds`. و`NatureDisplayName` صارت `public static NatureName` ليستهلكها `frmAccounts` |
| `frmAccounts` | حالة واحدة `_treeFilterIds`/`_treeFilterLabel` تُضاف إلى **نفس** `conditions` في `_ApplyFilter()` بشرط `Id IN (…)` (فارغة ⇒ `Id = -1`) — فتتقاطع بـ`AND` مع التصفية الفورية. عقدة حساب ⇒ تصفية + فتح السجل · عقدة طبيعة ⇒ تصفية فقط بلا فتح · إفراغ التحديد ⇒ رفع التصفية |
| المسح | `_ClearFilterInputs()` تُفرّغ تصفية الشجرة و`Tree.ClearSelection()` داخل حارس `_syncingSelection` ⇒ «مسح التصفية» و`Esc` و`F5` تعيد الكل. ونقر الشبكة **لا** يُصفّي (المزامنة تحت الحارس) |
| إلغاء تكرار | خيارات «نوع الحساب» في صندوق التصفية صارت تُبنى من `ctrlTreeOfAccounts.NatureName` — **قائمة تسميات واحدة** في النظام (`RULE-N172`) |
| الملخص | `lblSummary` يضيف «— العقدة: <الكود — الاسم>» عند وجود تصفية شجرية (`RULE-N61`) |
| **بند مقيس لم يُغلق** | تصفية العقدة تعرض **النشط فقط** لأن `_roots` تأتي من `GetTreeAsync` المرشَّح بـ`IsActive` — فحساب معطّل داخل الفرع لا يظهر بتصفية عقدته حتى مع «إظهار غير النشط». يُغلق بقرار (تصفية بالمعرّفات من القائمة الكاملة `_accounts` بدل `_roots`) |
| البناء | **Build succeeded · 0 Error(s)** |

## الجولة 8 — 2026-11-17 — حذف بلوك «محسوب تلقائياً بواسطة النظام»

**نصّ المالك:** «delete from UI محسوب تلقائياً بواسطة النظام».

| البند | القياس |
|---|---|
| محذوف من المصمّم | **11** عنصراً: `lblDerivedCaption` + أربعة أزواج (تسمية + صندوق) + زوج «يقبل القيود» — أُزيلت تعريفاتها و`Controls.Add` وكتل خصائصها وتلميحاتها |
| محذوف من الكود | `_RefreshDerivedFields()` (**5** مواضع استدعاء) · `NoValueText` (9 مراجع) · `_DerivedLevelForNewAccount` · `_DescribeFinancialStatement` · `_FinancialStatementFor` · `_SelectedNatureValue` (استدعاؤه الوحيد كان داخل البلوك) · معالج `cbNature_SelectedIndexChanged` (كان جسمه سطراً واحداً هو `_RefreshDerivedFields`) — **كل حذف سبقه عدّ مراجع** (`RULE-N195`) |
| بقي كما هو | `cbAccountType` ⇒ `IsLeaf` · `TryBuildCreate/Update` · معاينة الكود · `RequiredFieldHelper` · `GreenThemeHelper.Apply(this)` آخر سطر · صفر `catch` جديد (`RULE-N205`) |
| القياس بعد الحذف | `Derived` = **0** · `_RefreshDerivedFields` = **0** · `NoValueText` = **0** في الملفين وفي `Screens/Accounting`. السطور: المصمّم **674 → 485** · الكود **1071 → 957** |
| أثر جانبي مقيس | آخر عنصر باقٍ `txtNotes` (`Y=390`, ارتفاع 56 ⇒ `Bottom=446`) داخل `gbAccountData` (ارتفاع 666) ⇒ **فراغ 220 بكسل** أسفل الصندوق. **لم أُغيّر الهندسة** (تضييقها يمسّ حجم المحرّر و`pnlActions` — قرار مالك) |
| البناء | **Build succeeded · 0 Error(s)** (`99` تحذيراً في بناء تزايدي) |

**قياس تخطيطي يخصّ الشاشة كلها (لم يُلمس):** `ctrlAccountEditor1` ارتفاعه **750** (`MinimumSize`) داخل `pnlEditorHost` ارتفاعه **~350** (`ClientSize 900 − 550` محجوزة للكتل الثابتة) — أي أن الصف السفلي من «بيانات الحساب» وشريط «حفظ/إلغاء» **تحتاج تمريراً**؛ والعلاج المقيس: `pnlEditorHost.AutoScroll = true` (موجود) أو نشر المحرّر على عمودين (العنصر عرضه 460 داخل حاوية 1072 ⇒ 600 بكسل مهدرة أفقياً).

