# F-003 — شجرة دليل الحسابات (`ctrlTreeOfAccounts`)

| الحقل | القيمة |
|---|---|
| `ID` | `F-003` |
| `الحجم` | L (عنصر تحكم + نافذة + دمج لاحق في `frmAccounts`) |
| `المرحلة` | `A` (Mock · واجهة فقط) |
| `الحالة` | قيد التنفيذ — العنصر **عرض شجري فقط** ومدمج في `frmAccounts`؛ `TASK-008` يملك بقية الشاشة |
| `الوكيل المالك` | `ui-agent` |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |
| `أول جولة` | `2026-11-17` |

## ما تفعله هذه الميزة (سطران)

`ctrlTreeOfAccounts` يعرض دليل الحسابات **شجرةً هرمية للقراءة فقط** (أب ← أبناء) من `IAccountDesktopService.GetTreeAsync`: خمس عقد طبائع ثابتة في المستوى الأول، ثم الهيكل الحقيقي بأي عمق، والاختيار يُبلَّغ بحدث واحد `AccountSelected` لتعبئة المحرّر وتحديد سطر الشبكة في `frmAccounts`.
**لا أزرار ولا تصفية ولا شريط ملخص داخله** — الأوامر كلها ملك الشاشة الحاضنة (`RULE-N100`/`RULE-N169`).

## ⚠️ لا تكسر هذه

1. **`frmAccounts.cs` / `frmAccounts.Designer.cs` خارج النطاق** — مملوكة لـ`TASK-008` (مفتوحة في `docs/TASKS/ACTIVE.md`، نطاقها يشمل `Screens/Accounting/Forms/frmAccounts*`). صفر مساس حتى تُقفل.
2. **صفر باك-إند**: لا `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/Direct/**` (الباك-إند مجمَّد — `RULE-N177`).
3. **العقد يُستهلك كما هو**: `GetTreeAsync()` → `AccountTreeNodeDto` (Id · AccountCode · NameAr · Nature · IsLeaf · CategoryId · Level · ColorCode · Description · Children · ParentAccountCode · TreePath) · `GetByIdAsync` · `CreateAsync(CreateAccountRequest)` · `UpdateAsync(int, UpdateAccountRequest)` · `DeleteAsync` · `PermanentDeleteAsync` · `GetNextChildCodeAsync(parentId)`.
4. **التحرير بيته `frmAccounts`** (`RULE-N100`): الشجرة تعرض وتُبلّغ عن الاختيار فقط، والشاشة الحاضنة تنفّذ إنشاء/تعديل/حذف من شريطها المشترك.
5. **لا حذف من الشجرة**: `DeleteAsync`/`PermanentDeleteAsync` تُستهلك من `frmAccounts` وحدها؛ داخل العنصر = صفر نداء كتابة.

## أين يعيش كودها؟

| الملف | الدور |
|---|---|
| `Screens/Accounting/Controls/ctrlTreeOfAccounts.cs` (+`.Designer.cs`·`.resx`) | الشجرة (عرض شجري فقط) + حدث `AccountSelected` + `SelectAccount` |
| `Screens/Accounting/Forms/frmAccountsTree.cs` (+`.Designer.cs`·`.resx`) | غلاف رقيق للاختبار الفوري (شجرة + إغلاق) |
| `Program.cs` | سطر `AddTransient<frmAccountsTree>()` (مالك مستقل) |

## الحالة الآن

العنصر مدمج داخل `frmAccounts` (شجرة يسار · محرّر يمين · شبكة أسفل) و`frmAccountsTree` نافذة عرض مستقلة. **لا أزرار ولا تصفية داخل الشجرة** — الأوامر والتصفية في الشاشة الحاضنة. ما يبقى مفتوحاً: تكافؤ رفض الـMock (`ف-AC-5`…`ف-AC-9`) وأي عمل باك-إند (مجمَّد — `RULE-N177`).
