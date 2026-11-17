# TASK-009 — نقل أزرار العمليات إلى `ctrlAccountEditor`

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-009` |
| `الحالة` | ⛔ **ملغاة بقرار المالك (2026-11-17)** — «no I do not want …» ثم اختار «رجوع كامل»؛ أُعيدت الحالة إلى HEAD حرفياً |
| `التاريخ` | `2026-11-17` |
| `الحجم` | **M** — 5 ملفات · لا تغيير عقد خدمة · لا مساس بمالية/مخزون |
| `المرحلة` | **مرحلة واحدة** (إعادة تخطيط وتوصيل واجهة قائمة — ليست مسار بيانات جديداً ⇒ لا بوابة Stage A/B) |
| `الوكيل المالك` | `ui-agent` (وكيل واحد لكل الملفات الخمسة — عقد واحد: المحرّر + حاضره) |
| `طلب المالك` | «I want to test to Convert buttons to ctrlAccountEditor.cs … and use ctrlAccountEditor.cs inside frmAccounts» |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |

## الهدف

تطابق `docs/acounts.md` الذي يضع **أزرار العمليات داخل لوحة الإدخال اليمنى**: يُضمَّن `ctrlCrudToolbar` **داخل** `ctrlAccountEditor`، ويُزال من رأس `frmAccounts`، وتبقى **كل** منطق العمليات في الشاشة الحاضنة (`RULE-N100`).

## داخل النطاق (قائمة مغلقة)

1. `SalesSystem.Desktop/Controls/Common/ctrlCrudToolbar.cs` — **إضافة فقط**: `public Button? PrimaryActionButton => btnSave;`
2. `SalesSystem.Desktop/Screens/Accounting/Controls/ctrlAccountEditor.cs`
3. `SalesSystem.Desktop/Screens/Accounting/Controls/ctrlAccountEditor.Designer.cs`
4. `SalesSystem.Desktop/Screens/Accounting/Forms/frmAccounts.cs`
5. `SalesSystem.Desktop/Screens/Accounting/Forms/frmAccounts.Designer.cs`

## خارج النطاق (ممنوع بالاسم)

`Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Program.cs` · `Helpers/**` · `Global Classes/**` · `ctrlTreeOfAccounts.*` · `frmAccountsTree.*` · `ctrlAccountEditor.resx` (إلا إن فرضه المصمّم) · أي شاشة أخرى.

## العقد الجديد للمحرّر (إضافي، لا يكسر شيئاً)

```csharp
public void SetActions(CrudMode mode, bool hasSelection, bool allowDelete); // يُغذّي الشريط المضمَّن
public event EventHandler? AddRequested;
public event EventHandler? EditRequested;
public event EventHandler? DeleteRequested;
public event EventHandler? RefreshRequested;
public event EventHandler? CloseRequested;
// SaveRequested / CancelRequested يبقيان كما هما (يُمرَّران من الشريط)
```

- `SetMode(CrudMode)` يبقى عامّاً ومستخدَماً داخلياً؛ `SetActions` تناديه ثم تُغذّي الشريط.
- `_PrimaryEditorButton()` تُعيد `ctrlCrudToolbar1.PrimaryActionButton` (تسليم `Enter` — `RULE-N191`).
- يُحذف **كل** ما يخصّ `btnSave`/`btnCancel`/`flpActions` من المحرّر (تكرار ممنوع — `RULE-N172`)، ويبقى `lblValidationHint`.
- `lblValidationHint` يدخل ضمن تحقّق `_ApplyModeState` كما هو (رسالة النظام/الحساب النظامي).

## تعريف الإنجاز

- `ctrlAccountEditor` يحتوي `ctrlCrudToolbar1` مُعلَناً في `*.Designer.cs` (بلا بناء وقت تشغيل — `RULE-N145`)، و`pnlActions` = شريط الأوامر + سطر التحقق.
- `frmAccounts` بلا `ctrlCrudToolbar1`، و`gbCommands` يحوي `btnPermanentDelete` وحده.
- توصيل الأحداث في `frmAccounts`: `Editor.AddRequested/EditRequested/DeleteRequested/RefreshRequested/CloseRequested` — وأسماء المعالجات الحالية تبقى (F2/F3/Delete/F5/Esc تعمل بلا تغيير).
- `Editor.SetActions(_mode, hasSelection, allowDelete)` بدل `Toolbar.SetMode/HasSelection/AllowDelete/ShowSave/ShowClose`.
- الحالة تُغذّى من `_ApplyModeState` وحدها (§2.36 / `RULE-N150`).
- صفر `Toolbar.` متبقٍ في `frmAccounts.cs` · صفر `btnSave`/`btnCancel` في `ctrlAccountEditor.*` · `PrimaryActionButton` في `ctrlCrudToolbar` = 1.
- المصمّم = التشغيل (`RULE-N134`/`N204`): أي لون مكتوب بصيغة `Convert.ToInt32`، وصفر مرجع لـ`Helpers` داخل `*.Designer.cs` (`RULE-N128`).

## قيود تنفيذية

- **لا بناء ولا تشغيل** — الـOrchestrator يبني (`RULE-N117`/`RULE-N159`).
- لا `MessageBox` مباشر (البيت `clsDialogs`) · لا `try/catch` جديد (`RULE-N205`).
- أي ملف خارج القائمة ⇒ أبلِغ ولا تلمس.

## الملفات المحجوزة لمالك آخر

`frmAccounts*` داخل نطاق `TASK-008` (مفتوحة في `docs/TASKS/ACTIVE.md`) — **المالك أذن صراحةً** بهذه الموجة (طلبه أعلاه)، والتعديل **مقصور على إزالة الشريط وإعادة التوصيل** لا على منطق العمليات.

## سجل الجولات

### الجولة 1 — 2026-11-17 — التفويض

- الملفات الخمسة مُقاسة ومستقرّة قبل التفويض (`ctrlAccountEditor.cs` 957 سطراً · `ctrlAccountEditor.Designer.cs` 25,013 بايت · `frmAccounts.Designer.cs` 34,952 بايت · `ctrlCrudToolbar.cs` 11,439 بايت).
- الشريط الحالي: `frmAccounts.Designer.cs:137` `gbCommands.Controls.Add(ctrlCrudToolbar1)` · `frmAccounts.cs:73` `Toolbar => ctrlCrudToolbar1` · توصيل 7 أحداث (`frmAccounts.cs:148‑157`).
- حالة الإدخال في `_ApplyModeState` (`frmAccounts.cs:~413‑417`): `Toolbar.SetMode/HasSelection/AllowDelete/ShowSave/ShowClose`.

### الجولة 2 — 2026-11-17 — التنفيذ والبناء

| البند | النتيجة (مقيسة بأمر في هذه الجولة) |
|---|---|
| الملفات | **5/5** في القائمة المغلقة، وصفر ملف خارجها |
| `ctrlCrudToolbar.cs` | إضافة عضو عام واحد: `PrimaryActionButton => btnSave` (سطر 161) |
| `ctrlAccountEditor.cs` | عقد جديد: `SetActions(mode, hasSelection, allowDelete)` + `Add/Edit/Delete/Refresh/Close` مُمرَّرة؛ `SaveRequested`/`CancelRequested` من الشريط المضمَّن؛ الحذف: `btnSave`/`btnCancel`/`flpActions` + `WireButtonDisabledVisual` الخاص بهما + معالجاهما |
| `ctrlAccountEditor.Designer.cs` | `ctrlCrudToolbar1` مُعلَن داخل `pnlActions` (Dock=Bottom · 460×46) · ترتيب الإرساء: سطر التحقق أعلى الشريط (`RULE-N125`) |
| `frmAccounts.cs` | `Toolbar` خاصية + 7 اشتراكات + 5 أسطر `Toolbar.SetMode/HasSelection/AllowDelete/ShowSave/ShowClose` ⇒ صفر · التوصيل من `Editor.*` بنفس أسماء المعالجات · `_ApplyModeState` = `Editor.SetActions(_mode, hasSelection, hasSelection && !systemAccount && !busy)` |
| `frmAccounts.Designer.cs` | `ctrlCrudToolbar1` = صفر · `gbCommands` 118 ⇒ 72 · `flpSecondary` يُعيد موضعه |
| `PrimaryActionButton` | **1** تعريف + **1** استهلاك (`ctrlAccountEditor.cs:671` — تسليم `Enter` بعد آخر حقل) |
| فحوص البقايا | `ctrlCrudToolbar1` في `frmAccounts*` = **0** · `Toolbar\.` في `frmAccounts.cs` = **0** · `btnSave\|btnCancel\|flpActions` في `ctrlAccountEditor.*` = **0** · في `ctrlAccountEditor.resx` = **0** · `MessageBox` في الملفات الخمسة = **0** |
| البناء | `dotnet build .\SalesSystem.slnx` = **0 Error(s) / 118 Warning(s)** (بناء كامل — نفس رقم الأساس، التحذيرات ليست بوابة `RULE-N117`) |
| الحالة | ✅ منفَّذ — بانتظار اختبار المالك اليدوي |

**انحرافات مقصودة (مُسجَّلة):** `ShowSave` داخل `SetActions` = `mode != Browsing && !الحساب نظامي` (يُكافئ `btnSave.Enabled = editable` القديم، والشريط لا يملك مدخل `AllowSave`) · «إلغاء التعديل» يظهر معطّلاً في وضع العرض (`ctrlCrudToolbar` بلا مدخل إخفاء، وسلوكه قبل الموجة نفسه) · `Editor.SetMode` المكرّر حُذف من `_ApplyModeState` لأن `SetActions` تضبطه (تغذية واحدة).

**خطر مفتوح:** عرض أزرار الشريط ~938px داخل محرّر بعرض أدنى 340px ⇒ عند تضييق الشاشة تُقتطع أزرار من اليسار. الحل المقترح (شريحة مستقلة): `ctrlCrudToolbar.Designer` — أزرار أضيق أو `WrapContents`/تمرير أفقي.

### الجولة 3 — 2026-11-17 — **رجوع كامل (Revert)** بطلب المالك

**نصّ المالك:** «no I do not want ctrlAccountEditor convert to inside frmAccounts and removed ctrlAccountEditor» ⇒ ثم اختار صراحةً **«رجوع كامل للموجة»**: الشريط يعود إلى `gbCommands` في رأس `frmAccounts`، و`ctrlAccountEditor` يعود يحمل زرَّي «حفظ/إلغاء» الداخليَّين، ويبقى مكوّناً مستقلاً يُستخدم داخل الشاشة (لا حذف).

| البند | النتيجة (مقيسة بأمر في هذه الجولة) |
|---|---|
| الملفات | **5/5** عادت إلى محتوى `HEAD` حرفياً |
| `git diff HEAD` للملفات الخمسة | **فارغ** · `git status --short` لها = **فارغ** |
| `PrimaryActionButton` | **0** (حُذف من `ctrlCrudToolbar.cs`) |
| `SetActions` | **0** (في `ctrlAccountEditor.cs` و`frmAccounts.cs`) |
| `ctrlCrudToolbar1` في `frmAccounts.cs`/`.Designer.cs` | **12** موضعاً (خاصية + 7 اشتراكات + إنشاء/إضافة/إعلان/كتلة) |
| `btnSave`/`btnCancel`/`flpActions` في `ctrlAccountEditor.*` | عائدة: **9** في `.cs` · **52** في `.Designer.cs` |
| تعديلات المالك الآخر | سليمة: `cbAccountType` (13/15) · `chkAllowCostCenter` (5/15) · `cbNormalBalance` (13/14) · `_LoadAccountTypeOptions` · `GroupToPostingWarningMessage` · `colFinancialStatement`/`cbNatureFilter` = **22** |
| البناء | `dotnet build .\SalesSystem.slnx --no-incremental` = **0 Error(s) / 118 Warning(s)** (بناء كامل — نفس رقم الأساس) |
| ملاحظة سياق | HEAD تحرّك إلى `70b7cc2` (شريحة مالك آخر) **قبل** الرجوع، فلم تُفقد أي تعديلات غير مُثبَّتة — تحقّق: `git status` للملفات الخمسة فارغ |

**انحراف موثَّق في التنفيذ:** أُعيد الملف كاملاً من `HEAD` (عبر `git show`، بلا `checkout/restore`) بدل لصق الكتل المحذوفة فقط، لأن نسخة الموجة كانت قد أخلّت بصيغة `Convert.ToInt32` (`RULE-N204`) وبسطر `Anchor` لـ`chkAllowCostCenter`؛ والرجوع الكامل هو «قبل TASK-009 حرفياً» بدليل `git diff` الفارغ.

**الخطر المفتوح في الجولة 2** (اقتطاع أزرار الشريط عند تضييق الحاضن) **سقط مع الرجوع** — الشريط عاد إلى عرض الشاشة الكامل (1426px).


