# المهمة الكبيرة الجارية (ACTIVE BIG TASK)

> **يُقرأ أولاً من كل وكيل — بلا أن يُطلب منه.** هذا الملف مؤشر فقط؛ التفاصيل في ملف التتبع (`RULE-N202`).
> **لا يُحذف ولا يُنقل** — مساره ثابت حتى تعرف كل الوكلاء أين تنظر.

| الحقل | القيمة |
|---|---|
| `الحالة` | **خمس مهمات كبيرة مفتوحة** — `1` (تعميم التنقل) · `2` (فصل `Users`) · `4` (بطاقات الكيانات) · `7` (تحويل المستندات إلى `frm*Master`) · `8` (`TASK-014` صندوق إضافة الصنف). **أُغلقت 2026-11-17:** `3` و`5` (سقطتا مع إزالة المحاسبة) و`6` (إزالة المحاسبة — الكود مُزال) |
| `آخر تحديث` | `2026-11-18` |

## المهام المفتوحة

| # | المهمة | ملف التتبع | الحالة | ملاحظة النطاق |
|---|---|---|---|---|
| 1 | تعميم موجات التنقل بـ`Enter` + تغذية الحقل المطلوب على كل شاشات `Screens/**` (94 نموذجاً) — موجة واحدة لكل شاشة | `docs/MangeSubaggentsAndTracProgress.md` | **مفتوحة** — تعميم بعد شهادة `frmProductMaster` (2026-11-16) | `Screens/**` |
| 2 | **فصل وحدة `Users` عن بقية الجداول** — لا FK إلى `Users` من أي جدول، وهوية المنفّذ تُسجَّل نصاً `nvarchar(100)` من جلسة الواجهة (قرار المالك 2026-11-17) | `docs/TASKS/TASK-007-users-module-decoupling.md` | **مفتوحة** — التصميم موثَّق، والتنفيذ على موجات | `Common` · `Models` · `DataAccess` · `Desktop/Services` — **ولا يمسّ `Screens/**` في موجاته الأولى** |
| 3 | **حقول دليل الحسابات الهرمية** — `NormalBalance` · `AllowCostCenter` (**بلا عمود عملة** — قرار المالك) + الشجرة المشتقة + تصنيف الحسابات الخاصة + مولّد الأكواد | `docs/TASKS/TASK-008-accounts-hierarchy-fields.md` | ⛔ **سقطت (2026-11-17)** — كودها (`AccountService` · `AccountCategoryHelper` · `frmAccounts`) حُذف مع إزالة الوحدة المحاسبية (`TASK-011` §13)؛ الوثيقة محفوظة لإعادة البناء في V2 | — (مُغلقة: لا ملف باقٍ) |

| 4 | **بطاقات الكيانات وأدوات البحث** (`ctrl*Card` · `ctrl*CardWithFilter` · `frmShow*Card` · `frmFind*`) — وحدة واحدة في كل مرة، لا تُغادر وحدة قبل 100% | `docs/FEATURES/F-002-entity-cards/` (`README` · `PLAN` · `USAGE` · `DECISIONS` · `LOG`) | **مفتوحة** — تبدأ بـ`Customers` (أمر المالك 2026-11-17: «work on Customer folder after you finished 100% move to next») | `Screens/<Module>/{Controls,Forms}` + سطر تسجيل واحد في `Program.cs` لكل نموذج — **لا `Business` · لا `DataAccess` · لا `Models` · لا `DTO` · لا `Services/**`** |
| 5 | **شجرة دليل الحسابات** (`ctrlTreeOfAccounts` + `frmAccountsTree`) — عرض هرمي · بحث بالكود/الاسم · اختيار أي حساب · CRUD من قائمة السياق | `docs/FEATURES/F-003-accounts-tree/README.md` | ⛔ **سقطت (2026-11-17)** — `ctrlTreeOfAccounts` و`frmAccountsTree` حُذفا مع إزالة الوحدة المحاسبية (`TASK-011` §13) | — (مُغلقة: لا ملف باقٍ) |

| 6 | **إخراج الوحدة المحاسبية من V1 إلى V2 (Schema V3)** — نقل 6 جداول (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) وبقاء `CashBoxes` و`Expenses` | `docs/TASKS/TASK-011-schema-v3-accounting-deferred.md` | **✅ إزالة الكود منفَّذة (2026-11-17)** — 84 ملفاً محذوفاً · 72 معدّلاً · البناء **0 أخطاء / 100 تحذير** · كتالوج الموديل **34** جدولاً · صفر مرجع محاسبي. ⏸ **المتبقي على المالك وحده:** إسقاط 8 أعمدة + 6 جداول على القاعدة الحيّة، وتوليد `InitialCreate` (كل الترحيلات أُزيلت بأمره «and remove all migrations» — `RULE-N198`: لا وكيل ينفّذ `DROP` ولا `dotnet ef database update`) | `Models/Accounting` · `Business/{Accounting,Services}` · `DataAccess/{Configurations,Seeders,Migrations}` · `Services/{Interfaces,Direct,Mock}` · `Screens/Accounting/**` · `DTO/**` · `Program.cs` · `frmMain` — **كلها نُفِّذت** |

| 7 | **تحويل شاشات المستندات إلى `frm*Master`** — `Expenses` · `CustomerReceipts` بنمط `frmProductMaster` (المستران قائمان) · **والمرتجعات رُجِّعت إلى الفصل بنمط `frmAddUpdateSalesInvoice` + `frmListSales` بأمر المالك (`D-10`)** | `docs/FEATURES/F-004-document-master-screens/` (`README` · `BRIEF` · `PLAN` · `DECISIONS` · `LOG` · `MANIFEST`) | **مفتوحة** — ✅ `Expenses` و`CustomerReceipts` مدمجتان · ✅ المرتجعات مُسترجعة ومُطابَقة 2026-11-18 (`0 Errors` · مراجعتان) · ⏳ بانتظار اختبار المالك | `Screens/{Returns,Expenses,CustomerReceipts}/**` + `Program.cs` (كتلة DI) + `frmMain` + `clsGlobal` + 3 شاشات مستدعية (`D-08`) — **لا `Business` · لا `DataAccess` · لا `Models` · لا `DTO` · لا `Services/**`** |

| 8 | **صندوق «إضافة صنف» فوق الشبكة في المشتريات والمرتجعات** بنمط `frmAddUpdateSalesInvoice` — باركود ⇒ كمية ⇒ سعر ⇒ خصم ⇒ `Enter` يُضيف السطر ويعيد المؤشر إلى الباركود | `docs/TASKS/TASK-014-line-entry-panel-purchases-returns.md` | **مفتوحة** (2026-11-18) | `Screens/Purchases/Forms/frmPurchaseInvoice*` · `Screens/Returns/Forms/frmSalesReturn.cs` · `Screens/Returns/Forms/frmPurchaseReturn.cs` (**حُدِّث الهدف 2026-11-18 بعد `F-004/D-10`: المستران المحذوفان لم يعودا موجودين**) · `Helpers/LineEntryPanelHelper.cs` (جديد) — **لا `Business` · لا `DataAccess` · لا `Models` · لا `DTO` · لا `Services/**` · ولا `frmAddUpdateSalesInvoice` ولا `frmOpeningStock`** |

> **قاعدة التعايش:** أي وكيل يعمل على المهمة 2 **لا يلمس** `Screens/**` قبل الموجة 3، وأي وكيل يعمل على المهمة 1 لا يلمس `Models`/`DataAccess`/`Common`. ملف واحد = مالك واحد لكل موجة (`RULE-N158`). المهمة 4 تعمل في **مجلد وحدة واحدة** تحت `Screens/<Module>/` + ملف `Program.cs` بمالك واحد لكل موجة — فلا تتقاطع مع 1/2/3 ما دامت لا تخرج من مجلد وحدتها.
> **تجميد المحاسبة (قرار V3 — 2026-11-17):** المهام المتعلقة بالحسابات — `F-003` (شجرة الحسابات) · `TASK-008` (حقول الهرمية) · `TASK-009`/`TASK-010` (محرر الحسابات) — **سقطت لا مجرد مجمَّدة**: كودها حُذف من المستودع مع إزالة الوحدة المحاسبية (`TASK-011` §13، أمر المالك «remove all Accounting code»). التعريفات محفوظة في `docs/database-schema-v2-deferred.md` لإعادة البناء في V2، ولا عمل محاسبي جديد في V1.

## إن كانت الحالة «مفتوحة»

1. **اقرأ ملف التتبع الخاص بمهمتك كاملاً قبل أول سطر** — هو مصدر الـBrief (النطاق · الوكلاء · جرد الملفات · الموافقات).
2. **لا تكتب فيه** — الـOrchestrator وحده يكتب؛ أعِد في تقريرك: صفّك في لوحة الوكلاء · الملفات التي لمستها · الدليل (`docs/GOVERNANCE/EVIDENCE.md`).
3. **لا تلمس ملفاً في «خارج النطاق»** — ولو بدا ناقصاً أو خاطئاً؛ أبلِغ عنه.
4. **الحالة «مغلقة»** ⇒ اقرأ آخر جولة في الملف قبل أي تعديل مستقبلي — لا تبدأ من الصفر.

## من يفتحها ويغلقها

**الـOrchestrator فقط:** يكتب اسم المهمة ومسار التتبع عند البدء، ويعيد الحالة إلى «لا توجد مهمة كبيرة مفتوحة» عند الإغلاق. **هذا المؤشر يبقى دائماً موجوداً**، وملف التتبع **يُغلق ثم يُؤرشف في `docs/TASKS/ARCHIVE/`** (سجل مراجعة — لا يُحذف) — `docs/GOVERNANCE/BIG-TASK-TRACKING.md`.
