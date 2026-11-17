# TASK-011 — إخراج الوحدة المحاسبية من V1 إلى V2 (Schema V3)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-011` |
| `العنوان` | نقل الجداول المحاسبية الستة من V1 إلى V2 (V3) — **الوثائق ✅ · إزالة الكود مؤجلة** |
| `الحجم` | **L** — 6 جداول · كيانات وتكوينات وخدمات وشاشات وبذور · مساس بمسار الفواتير والسندات القائم |
| `المرحلة` | **وثائق / هيكل** — لا Stage A ولا Stage B: قرار تقليص نطاق V1 |
| `الحالة` | **الوثائق ✅ · إزالة الكود ✅ (2026-11-17) · إزالة الجداول على القاعدة الحيّة ⏸ ملك المالك** |
| `الوكيل المالك` | `orchestrator` — نفّذت الإزالة 7 موجات (18 وكيلاً) |
| `أثر قاعدة البيانات` | **لم يُنفَّذ أي `DROP` من أي وكيل ولا `dotnet ef`** · وأُزيل مجلد `Migrations/` **بأمر المالك** «and remove all migrations» ⇒ لا لقطة ولا ترحيل في المستودع. كتالوج الموديل بعد الإزالة = **34** جدولاً (`ToTable` فريدة) وصفر جدول محاسبي |
| `التاريخ` | `2026-11-17` |
| `ملف التتبع` | هذا الملف — وهو مُدرَج في `docs/TASKS/ACTIVE.md` |

---

## 1. الطلب (نص المالك الحرفي — `2026-11-17`)

> «remove these to v2: … شجرة الحسابات (`Accounts`) · قيود اليومية (`JournalEntries` و `JournalEntryLines`) · سندات القبض والصرف اليدوية (`ReceiptVouchers`, `PaymentVouchers`) … جداول الربط المحاسبي (`SystemAccountMappings`)»
>
> «remove accounting from MVP v1»
>
> «and current v2 -> v3»

**المسوّغ (قرار المالك):** النظام صار **`Pure POS & Inventory`** — بيع → خصم مخزون → تسجيل في الصندوق → رصيد العميل من فواتيره. **لا مدين ولا دائن في أي شاشة من شاشات V1**، فكل ما يخدم القيد المزدوج يخرج من V1 إلى V2.

> **ملاحظة توثيقية مهمة:** الطلب أعلاه **قرار تقليص نطاق**، و**ليس** أمر تنفيذ إزالة. الإزالة الفعلية (كود + جداول) **لم تُنفَّذ** وهي موجة منفصلة تحتاج **موافقة `DROP` مكتوبة ومؤرّخة** — لأن القاعدة تحمل بيانات عميل حقيقي في السوق (`AGENTS.md` §2.42).

---

## 2. القرار — الإصدار الثالث (V3)

| البند | V1 (MVP) | V2 (مؤجَّل) | الفرق |
|---|---|---|---|
| **الجداول** | **34** جدولاً هدفاً | **29** جدولاً مؤجلاً | + **6** جداول محاسبية نُقلت من V1 |
| **المقيس اليوم في `SalesDbContextModelSnapshot.cs`** | — | — | **40** `ToTable` = 34 + 6 |
| **الوحدة** | «مبيعات ومخزون صرف» | المحاسبة كاملة | `Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings` |

**الوثائق المحدَّثة (تمّت في هذه الموجة — ليست من نطاق هذا الوكيل):**

| الملف | ما تغيّر |
|---|---|
| `docs/database-schema.md` | **V3**: §3 (المخزون) · §4 (`Module 4` — الأصول الثابتة/المحاسبة) · §8 (قائمة المؤجَّل) · §8.1 (جدول «دين الترحيل» المقيس) · §11 الصف 4 |
| `docs/database-schema-v2-deferred.md` | **V2** + قسم جديد **«المجموعة AC — المحاسبة»** (السطر 570) يحمل الجداول الستة بتعريفاتها الكاملة |

**وضع الكود اليوم (مقيس 2026-11-17):** **قائم كما هو** — لا ملف حُذف ولا `DROP` نُفِّذ. الإزالة موجة لاحقة بموافقة مستقلة.

---

## 3. الجداول

### 3.1 المنقولة إلى V2 (ستة)

| # | الجدول | سبب النقل |
|---|---|---|
| 1 | `Accounts` | شجرة حسابات — لا معنى لها بلا قيود |
| 2 | `JournalEntries` | القيد المزدوج — لا مدين/دائن في V1 |
| 3 | `JournalEntryLines` | سطور القيد — تابعة لـ`JournalEntries` |
| 4 | `ReceiptVouchers` | سند قبض **يدوي** — V1 يسجّل التحصيل من فاتورته |
| 5 | `PaymentVouchers` | سند صرف **يدوي** — V1 يسجّل الدفع من فاتورته |
| 6 | `SystemAccountMappings` | خرائط ربط الوظائف بالحسابات — لا حسابات في V1 |

### 3.2 الباقية في V1 (اثنان — تشغيليان لا محاسبيان)

| # | الجدول | السبب |
|---|---|---|
| 1 | `CashBoxes` | **تشغيلي**: خزينة البيع/الدفع في `frmPOS` والفواتير والمصروفات — يُستهلك يومياً |
| 2 | `Expenses` | **تشغيلي**: مصروف نقدي من الصندوق (`frmListExpenses` · `frmAddUpdateExpense`) |

> **قيد مقيس:** `CashBoxes` و`Expenses` يحملان **مفتاحاً أجنبياً إلى `Accounts`** اليوم (`CashBoxes.AccountId` · `Expenses.ExpenseAccountId`) — ولهذا **لا يمكن `DROP TABLE Accounts` قبل فصل العمودين** (الخطوة 3 في §6).

---

## 4. حجم الأثر المقيس (Blast radius — `RULE-N195`/`RULE-N196`)

| الطبقة | العدد المقيس | التفصيل |
|---|---|---|
| `SalesSystem.Models/Accounting/` | **11 ملفاً** | 6 كيانات (`Account` · `JournalEntry` · `JournalEntryLine` · `ReceiptVoucher` · `PaymentVoucher` · `SystemAccountMapping`) + 5 enums |
| `SalesSystem.Business/Accounting/` | **5 ملفات** | `AccountingIntegrationService` · `JournalEntryNumberGenerator` · `JournalEntryNumberResult` · `JournalEntryService` · `SystemAccountService` |
| `Business/Services` (ملفات الحسابات) | **3 ملفات** | `AccountService.cs` · `AccountCodeGeneratorService.cs` · `AccountLinkService.cs` |
| `Business/Interfaces/Services` | **7 ملفات** | `IAccountService` · `IAccountCodeGeneratorService` · `IAccountLinkService` · `IAccountingIntegrationService` · `IJournalEntryService` · `IJournalEntryNumberGenerator` · `ISystemAccountService` |
| `DataAccess/Seeders` | **1 ملف** | `AccountingSeeder.cs` |
| `Screens/Accounting/Forms` | **3 نماذج** | `frmAccounts` · `frmAccountsTree` · `frmJournalEntries` (+ `Controls/ctrlTreeOfAccounts.cs` — عمل ميزة `F-003`، مصيره معلَّق على عودة المحاسبة في V2) |
| `Desktop/Services` | **7 ملفات** | `Interfaces`: `IAccountDesktopService` · `IJournalEntryDesktopService` · `Direct`: `DirectAccountService` · `DirectJournalEntryService` · `Mock`: `MockAccountDesktopService` · `MockJournalEntryDesktopService` · `MockAccountsData` · `MockJournalEntriesData` |
| التكوينات | **6 ملفات** (مقيسة عبر `AccountId`) | `AccountConfiguration` · `JournalEntryLineConfiguration` · `ReceiptVoucherConfiguration` · `PaymentVoucherConfiguration` · `SystemAccountMappingConfiguration` · + `CashBoxConfiguration`/`CustomerConfiguration`/`SupplierConfiguration`/`ExpenseConfiguration` (خارج الوحدة لكنها مرتبطة) |
| **أعمدة `NOT NULL` إلى `Accounts`** | **7 أعمدة `AccountId` + 1 عمود `ExpenseAccountId`** | `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings` · `CashBoxes` · `Customers` · `Suppliers` — كلها `IsRequired()` في اللقطة · و`Expenses.ExpenseAccountId` كذلك |
| **الاقتران بالكود القائم (الأخطر)** | **10 خدمات V1 تنادي الباك-إند المحاسبي** | `IAccountingIntegrationService` في 7: `SalesService` · `SalesReturnService` · `PurchaseService` · `PurchaseReturnService` · `CustomerReceiptService` · `SupplierPaymentService` · `ExpenseService` — و`IAccountLinkService` في 3: `CashBoxService` · `CustomerService` · `SupplierService` |
| `Program.cs` | **مراجع DI** | تهيئة/تسجيل الخدمات المحاسبية و`AccountingSeeder` — تُراجَع في موجة الإزالة |
| اللقطة الأساسية | **40** `ToTable` | الهدف بعد الإزالة = **34** |

> **معنى الرقم الأخطر:** إزالة المحاسبة **لا تُختصر بحذف ملفات** — عشرة خدمات V1 من **الحلقة الأساسية** (بيع/شراء/مرتجع/سندات/مصروفات/صندوق/عملاء/موردون) تناديها اليوم. لذلك **الخطوة الأولى إلزاماً** هي فصل الاقتران، لا الحذف.

---

## 5. خارج النطاق (ممنوع بالاسم في هذه الموجة)

- **أي حذف أو `DROP` أو `ALTER` أو ترحيل** — ممنوع بلا موافقة `DROP` مكتوبة (`RULE-N198`).
- `SalesSystem.Business/**` · `SalesSystem.DataAccess/**` · `SalesSystem.Models/**` · `SalesSystem.DTO/**` · `Services/Direct/**` · `Services/Mock/**` · `Migrations/**` · `Seeders/**`.
- `Screens/**` — بما فيها شاشات المحاسبة الثلاث (`frmAccounts` · `frmAccountsTree` · `frmJournalEntries`) و`ctrlTreeOfAccounts`.
- `Program.cs` · `appsettings.json` · أي `.csproj` · `SalesSystem.slnx` (`RULE-N114`).
- `docs/MOCK-COVERAGE.md` · `docs/FEATURES/**` · `AGENTS.md` — يملكها الـOrchestrator.
- **المهام المتجمدة بقرار V3** — لا عمل جديد عليها: `F-003` (شجرة الحسابات) · `TASK-008` (حقول الهرمية) · `TASK-009`/`TASK-010` (محرر الحسابات).

---

## 6. الترتيب الملزم لموجة الإزالة المستقبلية (لا يُخالَف)

> **بوابة الدخول:** موافقة مالك **مكتوبة ومؤرّخة** بصيغة صريحة (مثال: «موافق على DROP الجداول المحاسبية») + `database-engineer` وحده ينفّذ الترحيل + قراءة SQL المولَّد (`RULE-N198` §4).

| # | الخطوة | الشرط قبل الانتقال | المخاطرة المُدارة |
|---|---|---|---|
| 1 | **نسخة احتياطية كاملة** للقاعدة + فحص إمكانية الاستعادة | نسخة مُتحقَّق من قراءتها (ليست صفر بايت) | لا رجوع بلا نسخة |
| 2 | **فصل الكود عن المحاسبة** — إزالة كل نداء إلى `IAccountingIntegrationService` / `IAccountLinkService` من الخدمات العشر + إزالة تسجيلات DI | `dotnet build` = 0 أخطاء **والوظائف التشغيلية سليمة** (بيع/شراء/مرتجع/سند/مصروف/صندوق) | كسر الفاتورة أو الصندوق = الأثر الأكبر |
| 3 | **ترحيل الأعمدة** — إزالة/إبطال `AccountId` من `CashBoxes` · `Customers` · `Suppliers` و`ExpenseAccountId` من `Expenses` | صفر مرجع للعمود في الكود + صفر FK إليه | يُفقد ربط الحساب (مقبول في V1) |
| 4 | **ترحيل الجداول** — `DROP` للستة بترتيب التوابع (السطور قبل الرؤوس: `JournalEntryLines` → `JournalEntries` · `SystemAccountMappings` · `ReceiptVouchers` · `PaymentVouchers` → ثم `Accounts` أخيراً) | صفر FK متبقٍّ إلى `Accounts` | ترتيب خاطئ = فشل الترحيل |
| 5 | **حذف الخدمات والشاشات والبذور** — الوحدة المحاسبية كاملة (`Business/Accounting` · 3 خدمات · 7 واجهات · 6 كيانات · 5 enums · 6 تكوينات · `AccountingSeeder` · 3 شاشات + عقدا Desktop + Direct + Mock + بيانات وهمية) | صفر مرجع في `Program.cs` و`frmMain` | شاشات يتيمة أو سطر DI ميت |
| 6 | **الإغلاق** — `dotnet build --no-incremental` = **0 أخطاء** + اللقطة `ToTable` = **34** + **قائمة الاختبار اليدوي** (§10) منفَّذة وموقَّعة من المالك | الأعداد الثلاثة مطابقة (`RULE-N201`) | لا إغلاق بادعاء بلا رقم |

> **ممنوع في الموجة:** `DROP` بلا نسخة احتياطية · حذف جدول قبل توابعه · حذف كود قبل فصل الاقتران · أي `CASCADE` · أي `UPDATE` جماعي لإعادة كتابة قيم قائمة (`RULE-N198`).

---

## 7. الحالة

| البند | الحالة |
|---|---|
| قرار الإخراج من V1 (V3) | ✅ **منفَّذ في الوثائق** |
| `docs/database-schema.md` (V3) | ✅ محدَّث (§3 · §4 · §8 · §8.1 · §11 صف 4) |
| `docs/database-schema-v2-deferred.md` (V2 + المجموعة AC) | ✅ محدَّث |
| إزالة الكود من المستودع | ✅ **منفَّذ 2026-11-17** — 84 ملفاً محذوفاً · 72 معدّلاً · 1 مُضاف (`AccountStatementDto` أُعيد لشاشة محفوظة) · البناء **0 أخطاء** |
| إزالة الجداول (`DROP`) | ⏸ **على القاعدة الحيّة: ملك المالك** — لا وكيل نفّذ `DROP` ولا `dotnet ef`؛ والترحيلات أُزيلت كلها بأمر «and remove all migrations» فيولّدها المالك من موديل 34 جدولاً |
| المهام المحاسبية الأخرى (`F-003` · `TASK-008` · `TASK-009` · `TASK-010`) | ⛔ **سقطت مع إزالة الوحدة** — شاشاتها وكودها لم يعودا في المستودع (`F-003` = `frmAccountsTree` + `ctrlTreeOfAccounts`: محذوفان) |

---

## 8. الأدلة (`docs/GOVERNANCE/EVIDENCE.md`)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| جداول اللقطة الأساسية | **40** | `(Select-String -Path .../SalesDbContextModelSnapshot.cs -Pattern 'ToTable\("([^"]+)"' -AllMatches).Matches.Count` | `40` |
| ملفات `Models/Accounting` | **11** | `(Get-ChildItem "SalesSystem/SalesSystem.Models/Accounting" -Filter *.cs -Recurse).Count` | `11` |
| ملفات `Business/Accounting` | **5** | `(Get-ChildItem "SalesSystem/SalesSystem.Business/Accounting" -Filter *.cs -Recurse).Count` | `5` |
| خدمات الحسابات في `Business/Services` | **3** | `Get-ChildItem .../Business/Services -Filter "*Account*.cs"` | `AccountCodeGeneratorService.cs` · `AccountLinkService.cs` · `AccountService.cs` |
| البذور المحاسبية | **1** | `Get-ChildItem .../DataAccess/Seeders -Filter *.cs -Name` | `AccountingSeeder.cs` |
| شاشات المحاسبة | **3** | `Get-ChildItem .../Screens/Accounting -Filter *.cs -Recurse` | `frmAccounts` · `frmAccountsTree` · `frmJournalEntries` (+`ctrlTreeOfAccounts`) |
| أعمدة `int AccountId` في اللقطة | **7** | `Select-String -Pattern 'Property<int\("AccountId"\)'` | `JournalEntryLine` · `PaymentVoucher` · `ReceiptVoucher` · `SystemAccountMapping` · `CashBox` · `Customer` · `Supplier` |
| عمود `ExpenseAccountId` | **1** (`NOT NULL`) | `Select-String -Pattern 'ExpenseAccount'` | داخل `Expense` + `IsRequired()` في `HasOne(Account,"ExpenseAccount")` |
| خدمات V1 تنادي `IAccountingIntegrationService` | **7** | `Select-String -Pattern "IAccountingIntegrationService"` | `SalesService` · `SalesReturnService` · `PurchaseService` · `PurchaseReturnService` · `CustomerReceiptService` · `SupplierPaymentService` · `ExpenseService` |
| خدمات V1 تنادي `IAccountLinkService` | **3** | `Select-String -Pattern "IAccountLinkService"` | `CashBoxService` · `CustomerService` · `SupplierService` |

---

## 9. المراجعة

`code-reviewer`: **لم تُطلب** — لا كود في هذه الموجة (وثائق + قرار نطاق فقط). المراجعة تُستدعى على موجة الإزالة (الخطوات 2–6 في §6) لا على هذا الصف.

---

## 10. الاختبار اليدوي (يُنفَّذ **بعد** موجة الإزالة — لا اليوم)

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmMain` | افتح القائمة الرئيسية | لا عنصر «دليل الحسابات» ولا «قيود اليومية» — ولا عنصر ميت ينقر إلى شاشة محذوفة |
| 2 | `frmPOS` | بيع نقدي كامل ثم إتمام | الفاتورة تُرحَّل · المخزون يُخصم · الصندوق يزيد · **بلا رسالة خطأ محاسبية** |
| 3 | `frmAddUpdateSalesInvoice` | فاتورة آجلة ثم ترحيل | الرصيد على العميل يزيد بمقدار صافي الفاتورة ولا قيد يُنشأ |
| 4 | `frmPurchaseInvoice` | فاتورة شراء ثم ترحيل | المخزون يزيد · رصيد المورّد يزيد · بلا خطأ |
| 5 | `frmSalesReturn` / `frmPurchaseReturn` | مرتجع كامل | عكس المخزون والرصيد صحيح |
| 6 | `frmAddUpdateCustomerReceipt` | سند قبض من فاتورة | الصندوق يزيد ورصيد العميل ينقص |
| 7 | `frmSupplierPaymentDialog` | سند صرف لمورّد | الصندوق ينقص ورصيد المورّد ينقص |
| 8 | `frmCashBoxMaster` | إضافة/تعديل صندوق ثم حفظ | لا حقل حساب ولا رسالة تطلبه |
| 9 | `frmAddUpdateExpense` | مصروف نقدي ثم حفظ | الصندوق ينقص بلا اختيار حساب |
| 10 | `frmCustomerMaster` / `frmSupplierMaster` | إضافة عميل/مورّد ثم حفظ | يُحفظ بلا حساب مرتبط وبلا خطأ |
| 11 | القاعدة | `SELECT name FROM sys.tables` مقارنة بالهدف | **34** جدولاً · صفر من الستة |
| 12 | البناء | `dotnet build --no-incremental` | **0 أخطاء** |

---

## 11. القرارات والمخاطر وما لم يُخترع

| # | البند | التفصيل |
|---|---|---|
| 1 | **القرار** | المحاسبة خارج V1 (`Pure POS & Inventory`). `CashBoxes` و`Expenses` **باقيان** لأنهما تشغيليان — ولهذا بقيت فجوتان في ترقيم المجموعة AC (`AC.2` · `AC.7`) ولم يُحذف سطر من تعريفاتهما |
| 2 | **خطر رئيسي** | **10 خدمات V1 تنادي الباك-إند المحاسبي** اليوم — أي حذف قبل الفصل يكسر الفاتورة/الصندوق. لذلك §6 يبدأ بالفصل لا بالحذف |
| 3 | **خطر ثانٍ** | **8 أعمدة `NOT NULL`** إلى `Accounts` (7 `AccountId` + `ExpenseAccountId`) تمنع `DROP TABLE Accounts` قبل ترحيل الأعمدة |
| 4 | **قيد الحوكمة** | القرار **لا يفتح** أي عمل محاسبي جديد في V1، و**لا يلغي** عملاً مغلقاً — يحكم التغيير التالي (`RULE-N178`) |
| 5 | **تجميد المهام** | `F-003` (شجرة الحسابات) · `TASK-008` (حقول الهرمية) · `TASK-009`/`TASK-010` (محرر الحسابات) **مجمَّدة** بقرار V3 حتى يقرّر المالك عودة المحاسبة في V2 — وحالتها في `docs/TASKS/INDEX.md` و`docs/FEATURES/INDEX.md` تبقى كما هي حتى يؤكّدها المالك |
| 6 | **فرق عددي لم أُخفِه** | صيغة البريف قالت «**6** أعمدة `NOT NULL` باسم `AccountId`»؛ **القياس على اللقطة أعطى 7** باسم `AccountId` + **1** باسم `ExpenseAccountId` = **8**. لم أخترع رقماً ولم أُسقط عموداً: الرقم المستخدم في §6 هو **8** لأن أقل من ذلك يترك FK معلَّقاً ويفشل `DROP` |
| 7 | **ما لم أتحقق منه** | لم أفتح ملفات `docs/database-schema.md` §3/§4/§8/§8.1/§11 سطراً سطراً للتحقق من محتواها — أُخذت كـ«محدَّثة» من أمر الموجة؛ والتحقق النهائي منها مسؤولية الـOrchestrator |
| 8 | **لم يُنفَّذ** | صفر حذف · صفر `DROP` · صفر ترحيل · صفر بناء · صفر `git` |

---

## 12. سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-17` | إنشاء المهمة — توثيق قرار V3 (إخراج المحاسبة من V1) وجرد الأثر المقيس والترتيب الملزم لموجة الإزالة |

---

## 13. جولة التنفيذ — 2026-11-17 (إزالة الوحدة فعلياً من المستودع)

> **أوامر المالك الحرفية في هذه الجولة:** «remove accounting from MVP v1» · «remove all Accounting code» · «and remove all migrations» · «use many subagents for remove Accounting code from Project code» · «puah before satart».

### 13.1 الموجات (18 وكيلاً · 7 موجات · مالك واحد لكل ملف — `RULE-N158`/`RULE-N159`)

| الموجة | الوكلاء | ما نُفِّذ |
|---|---|---|
| 0 (خط الأساس) | Orchestrator | دفع العمل المعلَّق كاملاً (283 مساراً) قبل البدء + دمج `origin/master` (موجة Enter 18) ⇒ `d48080d..5f87fb2` · الشجرة نظيفة · `local == origin` |
| 1 | `explore` (قراءة فقط) | جرد كامل: **148** مساراً (80 حذفاً + 68 تعديلاً) · 136 مرجعاً · جدول الأعمدة الثمانية · سطور `Program.cs` و`frmMain` |
| 2 | `database-engineer` · `backend-architect` ×2 · `ui-agent` | الكيانات والتكوينات والبذور وحذف `Migrations/` · Business + واجهاته · DTO + عقود Desktop + `Direct` · الـMock وبذوره |
| 3 | `fast-agent` · `backend-architect` ×2 · `ui-agent` | حذف الشاشات والتقارير والسندات · **إعادة بناء دفتر الصندوق من المستندات** · تقارير BLL + إعادة `AccountStatementDto` · عقود التقارير على الـDesktop |
| 4 | `fast-agent` · `backend-architect` · `ui-agent` ×2 | `using` ناقص · فصل `SalesService` · القوائم و`Program.cs` و`ReportNavigationHelper` · حقول الحساب في 9 شاشات |
| 5 | `backend-architect` | إصلاح **7 أخطاء بناء** (`IGenericRepository` بلا `.Where` · `DateOnly` · تكرار `autoPrint` · `return` مفقود) |
| 6 | `fast-agent` | `DirectCashBoxService.TransferAsync` (دالة BLL محذوفة) |
| 7 | `fast-agent` ×2 | **تكافؤ الـMock:** `MockCashBoxDesktopService.TransferAsync` كان يُرجع `Success()` بلا أثر مالي ⇒ صار يرفض بنفس نص الوضع الحقيقي · تنظيف تعليقات كاذبة في 10 ملفات |

### 13.2 المقيس (أمر في نفس التشغيل)

| البند | القيمة | الأمر/المصدر |
|---|---|---|
| البناء الكامل | **0 Errors / 100 Warnings** | `dotnet build .\SalesSystem.slnx --no-incremental` |
| جداول الموديل | **34** (`ToTable` فريدة) وصفر محاسبي | `Select-String Configurations/*.cs -Pattern 'ToTable\("([^"]+)"'` ⇒ 34 فريدة |
| `DbSet` في `SalesDbContext` | **32** | `Expenses` و`CompanySettings` بلا `DbSet` (تُسلك عبر المستودعات) — لا نقص |
| ملفات الترحيل | **0** | `(Get-ChildItem Migrations -Filter *.cs).Count` — بأمر المالك |
| `SalesSystem.Models.Accounting` | **0** | grep |
| `JournalEntr` · `SystemAccountMapping` · `ReceiptVoucher|PaymentVoucher` | **0 · 0 · 0** | grep |
| `AccountService|AccountCodeGenerator|AccountLinkService` | **0** | grep |
| `frmAccounts` · `frmJournalEntries` | **0 · 0** | grep (حسّاس لحالة الأحرف — `frmAccountStatementReport` شاشة محفوظة ولا تُعدّ) |
| `Accounting` في `Program.cs` | **0** | grep |
| صافي الملفات | **157** (84 حذفاً · 72 تعديلاً · 1 إضافة) | `git status --porcelain` |

### 13.3 قرارات اتُّخذت في الجولة (تُبلَّغ للمالك)

| # | القرار | السبب المقيس |
|---|---|---|
| 1 | **دفتر الصندوق أُعيد بناؤه من المستندات** | لم يكن هناك جدول `CashTransactions`؛ رصيد الصندوق كان **حرفياً** مجموع سندات القبض − سندات الصرف، وفاتورة البيع النقدي كانت تُنشئ سند صرف. المعادلة الجديدة: مبيعات نقدية (`CashBoxId` + `Posted` + `Cash` + `PaidAmount`) + سندات القبض − سندات الصرف − المصروفات، والمرحّل فقط |
| 2 | **التحويل بين الصناديق غير مدعوم** | كان مُنفَّذاً على سندين (صرف + قبض) وزرّه غير مستخدم في أي شاشة ⇒ يرفض الآن برسالة عربية **متطابقة** في الوضعين (`RULE-N167`) |
| 3 | **`AccountStatementDto` أُعيد بناؤه** | حُذف بالخطأ داخل `JournalEntryDtos.cs` وهو مُستهلك في `frmAccountStatementReport` (**الشاشة المرجعية لتخطيط التقارير الـ31** — `RULE-N134`/`RULE-N146`) |
| 4 | **4 تقارير حُذفت · 4 بقيت** | حُذفت: ميزان المراجعة · دفتر الأستاذ · الميزانية العمومية · رأس المال العامل (كلها من دليل الحسابات/القيود). بقيت: قائمة الدخل · التدفقات النقدية · ضريبة القيمة المضافة · كشف حساب — كلها من الفواتير/السندات/المصروفات |
| 5 | **إزالة أسطر «قيد يومية» من كشف حساب العميل** | كانت **تُضاعف** نفس الحركة (الفاتورة + السند + القيد) — الإزالة تُصلح عيب ازدواج قائماً |
| 6 | **خمسة خدمات لم تكن في البريف** | المقيس **10** خدمات تنادي الباك-إند المحاسبي: أُضيفت `SalesReturnService` · `PurchaseReturnService` · `CashBoxService` · `CustomerService` · `SupplierService` إلى نطاق الفصل — بدونهن لا تُبنى ولا تُحفظ الأطراف |
| 7 | **البريف قال «V3 → 28 جدولاً» والهدف 34** | الرقمان في البريف متناقضان؛ الصحيح **34** = 40 − 6، وهو ما تؤكده `§8` من `docs/database-schema.md` وبوابة الإغلاق نفسها |

### 13.4 عوائق مفتوحة (لا تُخفى — `RULE-N196`)

| # | العائق | الأثر | من يملكه |
|---|---|---|---|
| `OPEN-1` | **القاعدة الحيّة ما زالت تحمل 8 أعمدة `NOT NULL` إلى `Accounts` و6 جداول** | إدخال عميل/مورّد/صندوق/مصروف **سيفشل** على قاعدة قديمة حتى تُسقط الأعمدة أو تُعاد إنشاء القاعدة | **المالك** (`RULE-N198`) |
| `OPEN-2` | **لا ترحيل في المستودع** | `dotnet ef migrations add InitialCreate` على موديل 34 جدولاً — يولّدها المالك («I will do migrations») | **المالك** |
| `OPEN-3` | `CashBoxService.TransferAsync` ما زالت `public` في الصنف وخارج الواجهة | كود غير مستهلك | شريحة S لاحقة |
| `OPEN-4` | `PurchaseService.cs` ~489: عنوان كتلة «Record cash transaction…» فوق `if` صار جسماً فارغاً | تعليق مضلّل + `if` فارغ | شريحة S لاحقة |
| `OPEN-5` | `ReportExportService` يحمل مفاتيح تصدير لتقارير محذوفة | كود ميت لا يكسر البناء | شريحة S لاحقة |
| `OPEN-6` | عدة ملفات مصمّم تعلن حقول أعمدة شبكة بلا إضافة (نمط قديم في `frmCashBoxMaster`/`frmCategoryMaster`/`frmUserMaster`) | تحذير `CS0169` قديم — ليس من هذه الموجة | مسجَّل، خارج النطاق |

---

## 14. المراجعة والشهادة (2026-11-17)

| البوابة | الحكم | التفصيل |
|---|---|---|
| `code-reviewer` (مرور واحد) | **Changes required** — 1 Blocker · 6 Major · 7 Minor | **Blocker `B1` أُصلح في نفس الموجة:** `ExpenseService.PostAsync`/`CancelAsync` كانا يمرّران `includePaths: "ExpenseAccount"` لتنقّل **محذوف** ⇒ `InvalidOperationException` عند **ترحيل/إلغاء أي مصروف** في الوضع الحقيقي (بناء أخضر يكشفه). صار `"CashBox"` وحده، وصفر `ExpenseAccount` في الملف |
| `mock-qa` (بوابة 0c) | **`مؤهل جزئياً`** — 31 عقداً · 31/31 لها Mock مسجَّل · **0 عقد غير مؤهل** | `IFinancialReportDesktopService` = 5 دوال بالضبط في العقد و`Direct` و`Mock`. **شاشة واحدة غير مؤهلة:** `frmVatReport` — بذورها بلا فاتورة ضريبية **مرحّلة** فشبكتها فارغة دائماً (`G-1`) |

**بنود المراجعة المفتوحة (مسجَّلة بأعدادها — `RULE-N196`):**

| # | الخطورة | الموضع | العيب |
|---|---|---|---|
| `M1` | Major | `CashBoxReportService.cs:50-77` | مقارنة `DateTime` (بوقت) مع `DateTime.UtcNow.Date` ⇒ **مبيعات/سندات/مصروفات اليوم مستبعدة** من تقريري ملخّص الصندوق والإقفال اليومي |
| `M2` | Major | `CashBoxService.cs:282-306` · `FinancialReportService.cs:92-98` | دفتر الصندوق الحقيقي يحتسب النقدي فقط (`PaymentType == Cash`) ⇒ **مدفوعات جزئية · شراء نقدي · مرتجعات نقدية غير محسوبة**، والـMock يحتسبها ⇒ فرق Mock/Real |
| `M3` | Major | `MockCashBoxReportDesktopService.cs` | صيغة مختلفة عن الوضع الحقيقي في ملخّص الصندوق/قائمة الدخل/التدفق (`RULE-N167`) |
| `M4` | Major | `CustomerService.cs:280-288` · `SupplierDtos.cs:10` · `ReportRepository.cs:136-180` | `CurrentBalance` **صفر ثابت** في الوضع الحقيقي (الحساب من المستندات نُفِّذ في الـMock فقط) · وسندات صرف بلا تصفية حالة |
| `M5` | Major | `SalesService.cs:577-584` · `PurchaseService.cs:500-503` | بلوك «الطباعة التلقائية» يقرأ الإعداد ويسجّل فقط — **لا يطبع** (تعليق يزعم العكس) |
| `M6` | Major | `frmCashBoxTransactions.cs:40,56-58` | الشاشة تبني الحركات من سندات القبض + المصروفات فقط ⇒ **ثلاث صيغ** لرصيد الصندوق في تطبيق واحد |
| `m1`…`m7` | Minor | `SalesService`/`PurchaseService`/`SalesReturnService` | تعليقات قديمة · حقن `ICashBoxService` غير مستخدم · `userId.ToString()` في عمود الاسم · `CreateCashBoxRequest` بلا مستهلك · `Sum` في الذاكرة · `ReportRepository` بلا تصفية حالة · بلاطات لوحة التحكم صفر ثابت |

---

## 15. جولة تنظيف `SalesSystem.Desktop` (2026-11-17 — أمر المالك: «remove all Accounting code at frist at SalesSystem.Desktop»)

| البند | قبل | بعد |
|---|---|---|
| مجلد التقارير | `Screens\Reports\Forms\Accounting\` (12 ملفاً — 4 تقارير **محفوظة**) | **`Financial\`** — نقل خالص (`git` سجّلها **12 rename**، الأحجام متطابقة، والـ`namespace` لم يتغيّر `RULE-N74`) |
| تصنيف القائمة | «التقارير المالية والمحاسبية» | **«التقارير المالية»** — في `frmMain.Designer` (النص) و`frmMain.cs` (التعليق) و`ReportNavigationHelper` (تصنيف الكتالوج) |
| تلميح زر الترحيل | يوعد بـ**ترحيل القيد المحاسبي** | يصف الأثر الحقيقي: خصم مخزون + تحديث رصيد العميل، **ودون أي قيد محاسبي** |
| `GreenThemeHelper.StatusJournal` | لون حالة باسم الوحدة المحذوفة، **صفر مستهلك** | **حُذف** مع تعليقه |
| تعليقات تنفي (3) | «يُرحّل القيد المحاسبي» · «الحساب المحاسبي» في منتقيي العميل/المورّد | صُحّحت — `grep "القيد المحاسبي\|الحساب المحاسبي"` في `Screens/**` = **0** |

**قياسات:** البناء `--no-incremental` = **0 Errors / 100 Warnings** · `Accounting` كمجلد تقارير = **0** · المجلدات الآن `Sales · Purchases · Inventory · Financial · CustomersSuppliers · CashBox`.

**بقايا لا تخصّ المحاسبة (تُركت بقرار معلَن):** `frmStocktakingPrintAdjustment` («سند تسوية مخزنية» ونصّ توقيع «المحاسب» في الطباعة) · `frmLogin` («أي حساب من الحسابات» = حساب مستخدم) · `MockSecurityData` (مستخدم تجريبي `accountant1`) · `ThemePalette` («المحاسب سوفت» = اسم جيل لوحة ألوان تاريخي).

---

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31).
