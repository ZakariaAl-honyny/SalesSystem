# المهمة الكبيرة الجارية (ACTIVE BIG TASK)

> **يُقرأ أولاً من كل وكيل — بلا أن يُطلب منه.** هذا الملف مؤشر فقط؛ التفاصيل في ملف التتبع (`RULE-N202`).
> **لا يُحذف ولا يُنقل** — مساره ثابت حتى تعرف كل الوكلاء أين تنظر.

| الحقل | القيمة |
|---|---|
| `الحالة` | **أربع مهمات كبيرة مفتوحة** |
| `آخر تحديث` | `2026-11-17` |

## المهام المفتوحة

| # | المهمة | ملف التتبع | الحالة | ملاحظة النطاق |
|---|---|---|---|---|
| 1 | تعميم موجات التنقل بـ`Enter` + تغذية الحقل المطلوب على كل شاشات `Screens/**` (94 نموذجاً) — موجة واحدة لكل شاشة | `docs/MangeSubaggentsAndTracProgress.md` | **مفتوحة** — تعميم بعد شهادة `frmProductMaster` (2026-11-16) | `Screens/**` |
| 2 | **فصل وحدة `Users` عن بقية الجداول** — لا FK إلى `Users` من أي جدول، وهوية المنفّذ تُسجَّل نصاً `nvarchar(100)` من جلسة الواجهة (قرار المالك 2026-11-17) | `docs/TASKS/TASK-007-users-module-decoupling.md` | **مفتوحة** — التصميم موثَّق، والتنفيذ على موجات | `Common` · `Models` · `DataAccess` · `Desktop/Services` — **ولا يمسّ `Screens/**` في موجاته الأولى** |
| 3 | **حقول دليل الحسابات الهرمية** — `NormalBalance` · `AllowCostCenter` (**بلا عمود عملة** — قرار المالك) + الشجرة المشتقة (`TreePath` · `FinancialStatement` · `IsSubAccount` · `Balance` · `CategoryLabel`) + تصنيف الحسابات الخاصة + مولّد الأكواد | `docs/TASKS/TASK-008-accounts-hierarchy-fields.md` | **مفتوحة** — الكود **منفَّذ** (بناء 0 أخطاء) والوثيقة محدَّثة؛ **الترحيل مسؤولية المالك** («I will do migrations») | `Models/Accounting` · `DataAccess/Configurations` · `DTO` · `Business` · `Services/{Interfaces,Direct,Mock}` · `Screens/Accounting/Forms/frmAccounts*` — **ولا يمسّ `Seeders/**` ولا `CostCenter` ولا `Bank` ولا `Migrations/**`** |

| 4 | **بطاقات الكيانات وأدوات البحث** (`ctrl*Card` · `ctrl*CardWithFilter` · `frmShow*Card` · `frmFind*`) — وحدة واحدة في كل مرة، لا تُغادر وحدة قبل 100% | `docs/FEATURES/F-002-entity-cards/` (`README` · `PLAN` · `USAGE` · `DECISIONS` · `LOG`) | **مفتوحة** — تبدأ بـ`Customers` (أمر المالك 2026-11-17: «work on Customer folder after you finished 100% move to next») | `Screens/<Module>/{Controls,Forms}` + سطر تسجيل واحد في `Program.cs` لكل نموذج — **لا `Business` · لا `DataAccess` · لا `Models` · لا `DTO` · لا `Services/**`** |
| 5 | **شجرة دليل الحسابات** (`ctrlTreeOfAccounts` + `frmAccountsTree`) — عرض هرمي · بحث بالكود/الاسم · اختيار أي حساب · CRUD من قائمة السياق | `docs/FEATURES/F-003-accounts-tree/README.md` | **مفتوحة** — العنصر والنافذة فقط؛ **الدمج في `frmAccounts` مؤجَّل** حتى إقفال المهمة 3 (مالك الشاشة) | `Screens/Accounting/{Controls,Forms}` + سطر DI — **لا `Screens/Accounting/Forms/frmAccounts*` ولا باك-إند** |

> **قاعدة التعايش:** أي وكيل يعمل على المهمة 2 **لا يلمس** `Screens/**` قبل الموجة 3، وأي وكيل يعمل على المهمة 1 لا يلمس `Models`/`DataAccess`/`Common`. ملف واحد = مالك واحد لكل موجة (`RULE-N158`). المهمة 4 تعمل في **مجلد وحدة واحدة** تحت `Screens/<Module>/` + ملف `Program.cs` بمالك واحد لكل موجة — فلا تتقاطع مع 1/2/3 ما دامت لا تخرج من مجلد وحدتها.

## إن كانت الحالة «مفتوحة»

1. **اقرأ ملف التتبع الخاص بمهمتك كاملاً قبل أول سطر** — هو مصدر الـBrief (النطاق · الوكلاء · جرد الملفات · الموافقات).
2. **لا تكتب فيه** — الـOrchestrator وحده يكتب؛ أعِد في تقريرك: صفّك في لوحة الوكلاء · الملفات التي لمستها · الدليل (`docs/GOVERNANCE/EVIDENCE.md`).
3. **لا تلمس ملفاً في «خارج النطاق»** — ولو بدا ناقصاً أو خاطئاً؛ أبلِغ عنه.
4. **الحالة «مغلقة»** ⇒ اقرأ آخر جولة في الملف قبل أي تعديل مستقبلي — لا تبدأ من الصفر.

## من يفتحها ويغلقها

**الـOrchestrator فقط:** يكتب اسم المهمة ومسار التتبع عند البدء، ويعيد الحالة إلى «لا توجد مهمة كبيرة مفتوحة» عند الإغلاق. **هذا المؤشر يبقى دائماً موجوداً**، وملف التتبع **يُغلق ثم يُؤرشف في `docs/TASKS/ARCHIVE/`** (سجل مراجعة — لا يُحذف) — `docs/GOVERNANCE/BIG-TASK-TRACKING.md`.
