# TASK-011 — إخراج الوحدة المحاسبية من V1 إلى V2 (Schema V3)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-011` |
| `العنوان` | نقل الجداول المحاسبية الستة من V1 إلى V2 (V3) — **الوثائق ✅ · إزالة الكود مؤجلة** |
| `الحجم` | **L** — 6 جداول · كيانات وتكوينات وخدمات وشاشات وبذور · مساس بمسار الفواتير والسندات القائم |
| `المرحلة` | **وثائق / هيكل** — لا Stage A ولا Stage B: قرار تقليص نطاق V1 |
| `الحالة` | **الوثائق ✅ · إزالة الكود ⏸ تحتاج موافقة DROP مكتوبة** (`RULE-N198`) |
| `الوكيل المالك` | `orchestrator` (الوثائق) — وموجة الإزالة **غير مفوَّضة** حتى الموافقة |
| `أثر قاعدة البيانات` | **إزالة ستة جداول (`DROP`) — ممنوع تنفيذها بلا موافقة مالك مكتوبة ومؤرّخة** (`RULE-N198`) · لم يُنفَّذ أي `DROP` ولا ترحيل |
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
| إزالة الكود من المستودع | ⏸ **لم تُنفَّذ** — تحتاج موافقة `DROP` مكتوبة |
| إزالة الجداول (`DROP`) | ⏸ **لم تُنفَّذ** — ولا ترحيل أُنشئ |
| المهام المحاسبية الأخرى (`F-003` · `TASK-008` · `TASK-009` · `TASK-010`) | 🔒 **مجمَّدة** بقرار V3 حتى يقرّر المالك عودة المحاسبة في V2 |

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

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31).
