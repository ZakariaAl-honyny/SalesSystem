# F-003 — شجرة دليل الحسابات (`ctrlTreeOfAccounts`)

| الحقل | القيمة |
|---|---|
| `ID` | `F-003` |
| `الحجم` | L (عنصر تحكم + نافذة + دمج لاحق في `frmAccounts`) |
| `المرحلة` | `A` (Mock · واجهة فقط) |
| `الحالة` | قيد التنفيذ — العنصر والنافذة فقط، والدمج في الشاشة مؤجَّل |
| `الوكيل المالك` | `ui-agent` |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |
| `أول جولة` | `2026-11-17` |

## ما تفعله هذه الميزة (سطران)

`ctrlTreeOfAccounts` يعرض دليل الحسابات **شجرةً هرمية** (أب ← أبناء) من `IAccountDesktopService.GetTreeAsync`، ويسمح باختيار أي حساب بسرعة (بحث بالكود/الاسم + إبراز المسار)، ويُتيح عملياته من قائمة سياق: إضافة فرعي · تعديل · حذف (بتأكيد).
تُستهلك أولاً في نافذة مستقلة `frmAccountsTree`، ثم تُدمج داخل `frmAccounts` **بعد** إقفال `TASK-008` (مالك الشاشة الآن).

## ⚠️ لا تكسر هذه

1. **`frmAccounts.cs` / `frmAccounts.Designer.cs` خارج النطاق** — مملوكة لـ`TASK-008` (مفتوحة في `docs/TASKS/ACTIVE.md`، نطاقها يشمل `Screens/Accounting/Forms/frmAccounts*`). صفر مساس حتى تُقفل.
2. **صفر باك-إند**: لا `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/Direct/**` (الباك-إند مجمَّد — `RULE-N177`).
3. **العقد يُستهلك كما هو**: `GetTreeAsync()` → `AccountTreeNodeDto` (Id · AccountCode · NameAr · Nature · IsLeaf · CategoryId · Level · ColorCode · Description · Children · ParentAccountCode · TreePath) · `GetByIdAsync` · `CreateAsync(CreateAccountRequest)` · `UpdateAsync(int, UpdateAccountRequest)` · `DeleteAsync` · `PermanentDeleteAsync` · `GetNextChildCodeAsync(parentId)`.
4. **التحرير بيته `frmAccounts`** (`RULE-N100`): الشجرة تُطلق `AddRequested`/`EditRequested` وتفتح الشاشة؛ لا تُبنى نافذة تحرير حساب ثانية.
5. **الحذف النهائي** يُعرض على حساب غير نظامي فقط وبعد `clsDialogs.Confirm` صريح؛ والحذف العادي `DeleteAsync`.

## أين يعيش كودها؟

| الملف | الدور |
|---|---|
| `Screens/Accounting/Controls/ctrlTreeOfAccounts.cs` (+`.Designer.cs`·`.resx`) | الشجرة + البحث + شريط الأدوات + قائمة السياق + الأحداث |
| `Screens/Accounting/Forms/frmAccountsTree.cs` (+`.Designer.cs`·`.resx`) | غلاف رقيق للاختبار الفوري (شجرة + إغلاق) |
| `Program.cs` | سطر `AddTransient<frmAccountsTree>()` (مالك مستقل) |

## الحالة الآن

العنصر والنافذة قيد التنفيذ؛ الدمج في `frmAccounts` **مؤجَّل** حتى إقفال `TASK-008` ويُسجَّل بنداً مفتوحاً في `LOG`.
