# TASK-010 — دمج `ctrlAccountEditor` داخل `frmAccounts` وحذف المكوّن

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-010` |
| `التاريخ` | `2026-11-17` |
| `الحجم` | **L** — نقل 957 سطراً إلى شاشة 1,165 سطراً + مصمّم 738 سطراً · لا تغيير عقد خدمة · لا مساس بمالية/مخزون |
| `المرحلة` | مرحلة واحدة (إعادة هيكلة واجهة قائمة — لا مسار بيانات جديد) · **صفر باك-إند** (`RULE-N177`) |
| `الوكيل المالك` | `ui-agent` (الموجة 1: `frmAccounts.*`) → `fast-agent` (الموجة 2: حذف المكوّن) |
| `طلب المالك` | «no I want to remove ctrlAccountEditor convert to inside frmAccounts and remove ctrlAccountEditor» |
| `نقطة الرجوع` | `HEAD = 70b7cc2` يحوي `ctrlAccountEditor.*` كاملاً ⇒ الحذف قابل للاسترجاع |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |

## الهدف

إلغاء الطبقة الوسيطة: الحقول والتحقق وآلة الحالة التي كانت في `ctrlAccountEditor` تنتقل **إلى `frmAccounts` نفسه**، ثم تُحذف ملفات المكوّن الثلاثة وتسجيله.

## الأساس المقيس قبل البدء (بأمر)

| البند | القياس |
|---|---|
| مستهلكو `ctrlAccountEditor` | **1 فقط**: `frmAccounts.cs:77` · `frmAccounts.Designer.cs:70,369,376‑384,712` — صفر في `Program.cs`، صفر في `.csproj`، صفر شاشة أخرى |
| أحجام | `ctrlAccountEditor.cs` 48,897 بايت (~957 سطراً) · `.Designer.cs` 25,013 · `.resx` 6,142 |
| المستضيف | `frmAccounts.cs` 1,165 سطراً · `frmAccounts.Designer.cs` 738 سطراً |
| التحقق | `RequiredFieldHelper.Attach(FindForm(), …)` في المكوّن ⇒ بعد الدمج يصبح `Attach(this, …)` (النموذج نفسه) |

## داخل النطاق

**الموجة 1 — `ui-agent` (ملفان):**
1. `SalesSystem.Desktop/Screens/Accounting/Forms/frmAccounts.Designer.cs`
2. `SalesSystem.Desktop/Screens/Accounting/Forms/frmAccounts.cs`

**الموجة 2 — `fast-agent` (حذف، بعد بناء أخضر):**
3. حذف `Controls/ctrlAccountEditor.cs` · `.Designer.cs` · `.resx`
4. تنظيف أي `using`/مرجع متبقٍّ (لا تسجيل DI قائم — تحقّق قبل الحذف)

## خارج النطاق (ممنوع بالاسم)

`Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Program.cs` · `Helpers/**` · `Global Classes/**` · `ctrlTreeOfAccounts.*` · `frmAccountsTree.*` · `ctrlCrudToolbar.*` · أي شاشة أخرى · `docs/**` (الـOrchestrator يكتب).

## الشكل المطلوب (لا اجتهاد)

داخل `pnlEditorHost` (بعد إزالة `ctrlAccountEditor1`):

```
pnlEditorHost (Dock=Fill · AutoScroll=true)
├── gbAccountData   Dock=Top · 666 · «بيانات الحساب» — الحقول الـ14 كما هي بالحرف
└── pnlActions      Dock=Top ·  84 · lblValidationHint (Top) + flpActions (Bottom: btnSave · btnCancel)
```
- ترتيب `Controls.Add`: `pnlActions` **أولاً** ثم `gbAccountData` (يُرسى آخر عنصر مُضاف أولاً — `RULE-N125`) ⇒ `gbAccountData` أعلى و`pnlActions` تحته.
- يُعاد استخدام `toolTip` الموجود في الشاشة؛ ويُضاف `errorProvider` **مُعلَناً** في مصمّم الشاشة (`ContainerControl = this` · `BlinkStyle = NeverBlink` · `RightToLeft = true`) — `RULE-N145`.
- كل الألوان/القياسات/التلميحات منقولة **حرفياً** من `ctrlAccountEditor.Designer.cs` (`Convert.ToInt32` — `RULE-N204`) · صفر مرجع `Helpers` في المصمّم (`RULE-N128`).

## الدمج بلا تكرار (`RULE-N172`) — إلزامي

`frmAccounts.cs` يملك أصلاً: `_mode` · `_editId` · `_accountsById` · `_ApplyModeState` · `_LoadAsync` · `_OnSaveRequested` · `_OnCancelRequested` · `_StartNewEntry` · `_OnRecordSelected` · `_ResetForm`.
⇒ **يُدمَج ولا يُلصَق**: قاموس معرّفات واحد · `_mode` واحد · `_ApplyModeState` واحدة تُغذّي الحقول والشريط معاً · آلة حالة واحدة. أي عضو مكرّر (مثل `_allAccounts` مقابل قائمة الشاشة) يُوحَّد في عضو واحد.

ما ينتقل من المكوّن: `BindParents` · `LoadAccount` · `ResetEditor` · `TryBuildCreate` · `TryBuildUpdate` · `_ValidateEditor` · `_IsDescendantOf` · `_BuildExclusionSet` · `RefreshCodePreviewAsync` · `LoadParentsAsync` · `_SelectParent` · `_SelectedParentId` · `_SelectNature` · `_SelectNormalBalance` · `_Load*Options` · `_SetValidationHint` · `_ClearValidation` · `_AttachRequiredFields` · معالجات `cbParent_SelectedIndexChanged` / `cbAccountType_SelectedIndexChanged` / `btnSave_Click` / `btnCancel_Click` + خيارات القوائم (`NatureOption` · `NormalBalanceOption` · `ParentOption` · `AccountTypeOption`) + الرسائل العربية الثمانية.
ما يسقط: `Attach(IServiceProvider)` (الشاشة تملك `_serviceProvider`) · `SetMode` (يُستدعى داخلياً) · `SaveRequested`/`CancelRequested` (يصيران نداءً مباشراً) · خاصية `Editor`.

## تعريف الإنجاز

1. صفر `ctrlAccountEditor` في `frmAccounts.cs`/`.Designer.cs` (الموجة 1)، وصفر في الحل كله (الموجة 2).
2. الشاشة تعمل بنفس السلوك: الوضع · التحقق (✗ + ومضة + حجب `Enter` على الحقل المطلوب) · معاينة الكود · قائمة الآباء المؤهَّلة (بلا الحساب وفروعه) · منع تحويل تجميعي له فروع على مستوى التنبيه · «حساب نظامي» بلا حفظ.
3. صفر عضو مكرّر: `_accountsById` واحد · `_mode` واحد · `_ApplyModeState` واحدة · صفر `_allAccounts` إن كان يمكن الاعتماد على `_accountsById.Values`.
4. `dotnet build` (الـOrchestrator) = **0 Errors** بعد كل موجة.
5. صفر `MessageBox` مباشر · صفر `try/catch` جديد (`RULE-N205`) · صفر لون خارج اللوحة.

## سجل الجولات

### الجولة 1 — 2026-11-17 — التفويض
قياسات أعلاه بأمر (`Select-String` على `ctrlAccountEditor` في الحل = 1 مستهلك · `Get-Item` للأحجام · عدّ أسطر الملفين).

### الجولة 2 — 2026-11-17 — الموجة 1: الدمج داخل الشاشة

| البند | النتيجة (مقيسة بأمر) |
|---|---|
| `frmAccounts.cs` | **1,165 ⇒ 1,804** سطراً (84,436 بايت) |
| `frmAccounts.Designer.cs` | **738 ⇒ ؟** سطراً (57,333 بايت) · `gbAccountData` (Dock=Top, 1072×666) + `pnlActions` (Dock=Top, 1072×84) داخل `pnlEditorHost` · ترتيب `Controls.Add`: `pnlActions` ثم `gbAccountData` (`RULE-N125`) |
| الحقول المنقولة | 14 حقلاً + `lblValidationHint` + `flpActions` + `btnSave` + `btnCancel` + `errorProvider` (مُعلَن في المصمّم · `ContainerControl = this` · `BlinkStyle = NeverBlink`) |
| آلة الحالة | `_ApplyModeState` = **1** (واحدة) · `_mode` = **1** · صفر `_allAccounts` (قاموس `_accountsById` واحد) |
| مفاتيح `Enter` | `WireEnterAsTab(gbAccountData, _PrimaryEditorButton)` **أولاً** (س174) ثم `WireEnterAsTab(this)` (س175) — «الأول يفوز» (`RULE-N191`/`N193`) |
| التحقق | `RequiredFieldHelper.Attach(this, errorProvider, new RequiredField(txtNameAr, …))` — النموذج نفسه (س566) |
| المصمّم | `Color.FromArgb(` مختصر = **0** · مراجع `Helpers` = **0** |
| البناء | `dotnet build SalesSystem.Desktop.csproj` = **0 Error(s)** |

### الجولة 3 — 2026-11-17 — الموجة 2: حذف المكوّن

| البند | النتيجة (مقيسة بأمر) |
|---|---|
| فحص المستهلكين قبل الحذف | **6** نتائج كلها داخل ملفات المكوّن نفسه · صفر في `frmAccounts.*` · صفر في `Program.cs` · صفر في `.csproj` |
| الحذف | `git rm` متتبَّع: `D` لـ`ctrlAccountEditor.cs` · `.Designer.cs` · `.resx` |
| ملاحظة | `ctrlAccountEditor.resx` كان معدَّلاً محلياً (تعليق ResX القياسي فقط · صفر إدخال مورد) ⇒ `git rm -f` عليه وحده · الأصل في `HEAD = 70b7cc2` |
| بقايا المصدر | `ctrlAccountEditor` في `SalesSystem/**` (بلا `bin`/`obj`) = **0** · `Editor\.` في `frmAccounts.cs` = **0** |
| `.csproj.user` | حُذفت منه كتلة `Compile Update="…ctrlAccountEditor.cs"` (ملف متجاهَل — صفر أثر على المستودع) |
| البناء بعد الحذف | `dotnet build .\SalesSystem.slnx --no-incremental` = **0 Error(s) / 118 Warning(s)** (رقم الأساس) |
| المتبقي | `Screens/Accounting/Controls/` = `ctrlTreeOfAccounts.*` فقط |

**الحالة:** ✅ منفَّذ بالكامل (دمج + حذف) — بانتظار الاختبار اليدوي للمالك. **لا كومِت ولا دفع.**

