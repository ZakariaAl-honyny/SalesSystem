# Database Schema — **Version 3 (MVP — Pure POS & Inventory)**

# Sales Management System — v5.0.0 (2-Tier WinForms · N-Layer)

**Platform:** SQL Server 2019+ · EF Core 10
**الجداول (هدف V3):** **34** · decimal-only financials · nvarchar text · soft delete · all FK `Restrict`
**Module boundary:** `Users` وحدة مستقلة — **لا FK إليها**، وهويات المنفّذ نصوص `nvarchar(100)` (§2)
**قرار V3 (2026-11-17):** أُخرجت الوحدة المحاسبية من الإصدار الأول — **ستة جداول محاسبية** نُقلت إلى `docs/database-schema-v2-deferred.md` (مرجع تصميم فقط)، وبقي `CashBoxes` و`Expenses` في V1 لأنهما **تشغيليان** لا محاسبيان (§4 · §8)

> **الفارق المقيس اليوم بين هذا الملف والـsnapshot مقصود ومُعلن، ولا يُخفى:** الـsnapshot ما زال يحمل **40** جدولاً لأن **إزالة الوحدة المحاسبية من الكود ومن القاعدة لم تُنفَّذ بعد**. الأعداد: `هدف V3 = 34` · `مقيس اليوم = 40` · الفرق = **6** جداول محاسبية (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) تنتظر موجة إزالة صريحة بموافقة مالك مكتوبة (`DROP` ممنوع بلا موافقة مستقلة — `RULE-N198`).

---

> **هذا الملف هو الإصدار الثالث (V3) — المصدر الوحيد لهيكل قاعدة البيانات المستهدف بعد إعادة ترتيب الإصدارات.**

**خريطة الإصدارات (V1 → V2 → V3):**

| الإصدار | معناه | أين يعيش | العدد |
|---|---|---|---|
| **V1 (MVP)** | نواة التشغيل فقط: عملاء · موردون · مستخدمون · تنظيم · أصناف ووحدات · مخزون · مبيعات · مشتريات · صندوق ومصروفات · سجلات | **هذا الملف** | **34** |
| **V2 (مؤجل)** | كل ما هو خارج الحلقة الأساسية: أدوار وجلسات · ضرائب وسنوات مالية · إشعارات ومرفقات · بنوك ومراكز تكلفة · تحويلات مخزنية وجرد · عملات وأسعار صرف · جهات اتصال · عروض أسعار — **ثم الوحدة المحاسبية كاملة (V3)** | `docs/database-schema-v2-deferred.md` | **29** |
| **V3 (هذا الإصدار)** | قرار المالك 2026-11-17: النظام «مبيعات ومخزون صرف» — **إخراج المحاسبة من الإصدار الأول** ونقل جداولها إلى V2 | هذا الملف (النسخة الحالية) | 34 |

> **كل جدول مذكور هنا في §4 له كيان وتكوين في المستودع، ما عدا ما تُصرِّح به موجة الإزالة في §8.**
> الجداول المؤجلة كلها في ملف واحد: `docs/database-schema-v2-deferred.md` (مرجع تصميم فقط — **لا كود ولا كيان ولا ترحيل** لأي منها، **إلا** الجداول المحاسبية الستة التي ما زال كودها قائماً حتى تُنفَّذ موجة الإزالة).
>
> **بروتوكول بقاء هذا الملف صادقاً (يُنفَّذ قبل كل ادّعاء رقمي — `RULE-N195`/`RULE-N196`):**
>
> ```powershell
> # العدد الفعلي في الـsnapshot — الأمر مُصحَّح ومُختبَر 2026-11-17 (الشكل السابق كان يرمي استثناءً)
> (Select-String -Path 'SalesSystem/SalesSystem.DataAccess/Migrations/SalesDbContextModelSnapshot.cs' `
>    -Pattern 'ToTable\("([^"]+)"' -AllMatches |
>  Select-Object -ExpandProperty Matches |
>  ForEach-Object { $_.Groups[1].Value } |
>  Sort-Object -Unique).Count
> ```
>
> - **قبل موجة الإزالة:** الناتج **40** = 34 هدف V3 + 6 جداول محاسبية (§8) — وهذا **صحيح ومتوقَّع**، لا خطأ في الملف.
> - **بعد موجة الإزالة بموافقتها المكتوبة:** الناتج يجب أن يساوي **34**.
> - أي رقم ثالث = الملف خاطئ ويُصحَّح، لا العكس.

---

# 1) قواعد التصميم (Design Rules)

## 1.1 أنواع البيانات (Data Types)

| الغرض | النوع | ملاحظة |
|---|---|---|
| المفاتيح الأساسية | `int IDENTITY(1,1)` | جداول السجلات الضخمة `bigint` · الجداول الصغيرة/المرجعية `smallint` |
| النصوص | `nvarchar` | لدعم العربية والإنجليزية |
| الباركود | `varchar(50)` | ASCII فقط |
| **المبالغ** | **`decimal(18,2)`** | ممنوع `float`/`double`/`real`/`money` (`RULE-001`) |
| **الكميات** | **`decimal(18,3)`** | ممنوع `int` للكميات (`RULE-002`) |
| النسب | `decimal(5,2)` | |
| الحالات والأنواع | `tinyint` | لكل enum |
| الأعلام | `bit` | 0/1 |
| تواريخ المستندات | `date` | |
| الطوابع الزمنية | `datetime2` | |

## 1.2 تسلسل الوراثة — **الهدف المعتمد** (Base Inheritance — target)

| الفئة | Id | CreatedAt | CreatedByUserName | UpdatedAt | UpdatedByUserName | IsActive | Status |
|---|---|---|---|---|---|---|---|
| **Entity** | int PK | — | — | — | — | — | — |
| **AuditableEntity** | int PK | datetime2 | nvarchar(100) null | datetime2 null | nvarchar(100) null | — | — |
| **ActivatableEntity** | int PK | datetime2 | nvarchar(100) null | datetime2 null | nvarchar(100) null | bit default 1 | — |
| **DocumentEntity** | int PK | datetime2 | nvarchar(100) null | datetime2 null | nvarchar(100) null | — | tinyint (1=Draft · 2=Posted · 3=Cancelled) |
| **LongEntity** | bigint PK | — | — | — | — | — | — |

> **الهدف:** هوية المنفّذ تُحفظ **نصاً** `nvarchar(100) null` — `CreatedByUserName` · `UpdatedByUserName` · `ReviewedByUserName` · `ClosedByUserName` · `CashierUserName` · `UserName` (السجلات). **الأعمدة الرقمية `CreatedByUserId`/`UpdatedByUserId` تُزال نهائياً** بقرار المالك 2026-11-17 (§2)، وليست جزءاً من الهيكل النهائي.
>
> **الحالة الحالية للقاعدة (مقيسة):** الـ`snapshot` ما زال يحمل الأعمدة الرقمية — لأن الترحيل لم يُنفَّذ بعد. §2 يحدد ترتيب التنفيذ الملزم.

## 1.3 قواعد التصنيف

- **كيانات إدارية** (عملاء · موردون · أصناف …): `ActivatableEntity` ⇒ حذف ناعم بـ`IsActive`.
- **كيانات مستندية** (فواتير · سندات · مرتجعات · تسويات): `DocumentEntity` ⇒ `Status` وليس `IsActive`.
- **جداول ربط**: `Entity` بلا أعمدة تدقيق (`ProductUnits` · `CustomerReceiptApplications` · `SupplierPaymentApplications` · `JournalEntryLines` · `SystemAccountMappings`).
- **أرصدة حيّة**: `WarehouseStocks` بـ`AuditableEntity` — **بلا** `IsActive`.
- **سجلات ضخمة**: `AuditLogs` · `SystemLogs` بـ`LongEntity` (`bigint`).

## 1.4 القيود الأساسية (Key Constraints)

- **كل مفاتيح الربط:** `DeleteBehavior.Restrict` — **لا `CASCADE DELETE` في أي مكان**.
- **الحذف الناعم:** فلتر استعلام عام `IsActive == true` لكل وارثي `ActivatableEntity` (منفَّذ في `Configurations/`).
- **قيود CHECK:** أرصدة المخزون (`WarehouseStocks.Quantity >= 0`)، المبالغ المالية، قواعد المدين/الدائن.
- **الفهارس الفريدة المفلترة:** على الكيانات القابلة للتعطيل تُضاف `AND [IsActive] = 1`.

## 1.5 الحذف الناعم (Soft Delete)

- `IsActive = false` لكل وارثي `ActivatableEntity` — الشاشات الإدارية تقرأ المعطَّل بـ`IgnoreQueryFilters()`.
- **الفواتير لا تُحذف:** `Status = Cancelled`.
- **المستخدمون لا يُحذفون نهائياً:** تعطيل فقط (`IsActive = false`).
- **المستندات** تُدار بـ`Status` (Draft → Posted → Cancelled) لا بـ`IsActive`.

## 1.6 قواعد مفاتيح الربط (FK Rules)

- كل مفاتيح الربط `Restrict`، والمرجع الذاتي الوحيد: `Accounts.ParentId`.
- **حدّ الوحدة (انظر §2):** لا جدول يعلن مفتاحاً خارجياً إلى `Users`، ولا يحمل **أي** معرّف مستخدم. هوية المنفّذ نص `nvarchar(100)` فقط — والعمودان الرقميان `CreatedByUserId`/`UpdatedByUserId` **يُزالان** بقرار المالك 2026-11-17.

---

# 2) قرار عزل الوحدة: `Users` وحدة مستقلة — استبدال المعرّف الرقمي باسم نصي (معتمد 2026-11-17)

> **نصّ قرار المالك (2026-11-17) — حرفياً:**
> «remove CreatedByUserId and UpdateByUserId and repllase by CreatedByUserName and UpdateByUserName for all system»
> — ويُقرأ مع قراره السابق في اليوم نفسه: «افصل جدول المستخدمين عن بقيت الجدول الاخرى لا ان يرتبط بالمعرف لكي نجزء النظام إلى وحدات صغير منفصلة واسجل اسم المستخدم الحالي من على الوجهات كنص» + «nvarchar».

**القرار:** يُقسَّم النظام إلى وحدات مستقلة. `Users` وحدة مستقلة: **لا مفتاح أجنبي إليها، ولا عمود `UserId`/`CreatedByUserId`/`UpdatedByUserId` في أي جدول آخر**. هوية المنفّذ تُحفظ **نصاً** `nvarchar(100) null` من جلسة الواجهة (`ISessionService.Current.UserName`) لحظة العملية.

| القاعدة | القيمة |
|---|---|
| أعمدة الهوية | `CreatedByUserName` · `UpdatedByUserName` · `ReviewedByUserName` · `ClosedByUserName` · `CashierUserName` · `UserName` (السجلات) — كلها `nvarchar(100) null` |
| المصدر | جلسة الواجهة — لا يُكتب يدوياً، ولا يُقرأ من `Users` عند العرض |
| الاتجاه | **نص باتجاه واحد**: الصف يحمل اسماً، ولا يشير إلى `Users.Id` |
| الدلالة التاريخية | **لقطة**: إعادة تسمية مستخدم لاحقاً لا تُعيد كتابة الصفوف القديمة — وهو المطلوب للتدقيق |
| الأثر | `Users` قابلة للنقل والنسخ الاحتياطي والاختبار وحدها بلا مسّ بيانات أي وحدة أخرى |

**الأعمدة التي تُزال نهائياً (بقرار المالك أعلاه):** `CreatedByUserId` · `UpdatedByUserId` (في كل جدول يحملها) · `ReviewedByUserId` (`JournalEntries`) · `ClosedByUserId` (`FiscalPeriods` — V2) · `CashierUserId` (`CashBoxes`) · `UserId` (`AuditLogs` · `SystemLogs`).

### ترتيب التنفيذ الملزم (تجميد الهيكل — `RULE-N198`)

الإزالة مسموحة **فقط** بهذا الترتيب، لأن القاعدة تحمل بيانات عميل حقيقي:

1. **نسخة احتياطية كاملة** للقاعدة قبل أي ترحيل (`BACKUP DATABASE … WITH COPY_ONLY`).
2. **ترحيل (1) — إضافة:** أعمدة `nvarchar(100) null` للأسماء. إضافي وآمن.
3. **تعبئة رجعية (backfill):** تُملأ الأسماء من `Users.UserName` بمطابقة المعرّف (`UPDATE` على **الأعمدة الجديدة فقط** — لا يُعاد كتابة أي قيمة قائمة).
4. **التحويل:** الكتابة في النص فقط، والقراءة من النص فقط — بعد التحقق من أن كل صف غير فارغ له اسم.
5. **ترحيل (2) — إزالة:** `DROP` للأعمدة الرقمية، **بعد** التحقق في (4) وبموافقة المالك المسجَّلة أعلاه.

> **ممنوع:** أي `ALTER COLUMN` لتغيير نوع عمود قائم، أو إعادة تسمية جدول/عمود، أو `DROP` قبل الخطوات 1–4، أو تنفيذ الخطوتين 2 و5 في ترحيل واحد.
> **ممنوع** أن تُبنى شاشة أو خدمة على `CreatedByUserName` قبل وجوده فعلياً — أي مراجعة تفترضه اليوم على أساس خاطئ.

---

# 3) جرد الجداول (34 جدولاً — هدف V3)

| # | الوحدة | العدد | الجداول |
|---|---|---|---|
| 1 | النواة والأمان | 3 | `Customers` · `Suppliers` · `Users` |
| 2 | التنظيم والإعدادات | 4 | `Warehouses` · `CompanySettings` · `SystemSettings` · `DocumentSequences` |
| 3 | المنتجات | 5 | `ProductCategories` · `Products` · `Units` · `ProductUnits` · `ProductPrices` |
| 4 | الصندوق والمصروفات (**تشغيلي**) | 2 | `CashBoxes` · `Expenses` |
| 5 | المخزون | 6 | `WarehouseStocks` · `InventoryBatches` · `InventoryTransactions` · `InventoryTransactionLines` · `InventoryAdjustments` · `InventoryAdjustmentLines` |
| 6 | المبيعات | 6 | `SalesInvoices` · `SalesInvoiceLines` · `SalesReturns` · `SalesReturnLines` · `CustomerReceipts` · `CustomerReceiptApplications` |
| 7 | المشتريات | 6 | `PurchaseInvoices` · `PurchaseInvoiceLines` · `PurchaseReturns` · `PurchaseReturnLines` · `SupplierPayments` · `SupplierPaymentApplications` |
| 8 | البنية التحتية | 2 | `AuditLogs` · `SystemLogs` |
| | **الإجمالي (هدف V3)** | **34** | |
| — | **المحاسبة (نُقلت إلى V2)** | **6** | `Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings` |

> **العدّ:** 3 + 4 + 5 + 2 + 6 + 6 + 6 + 2 = **34** هدف V3 · و**40** = 34 + 6 جداول محاسبية ما زالت في الـsnapshot حتى تُنفَّذ موجة الإزالة (§8).

> **لماذا بقي `CashBoxes` و`Expenses` في V1 وهما في وحدة المحاسبة سابقاً؟** لأنهما **تشغيليان**: الكاشير يسجّل في أي صندوق باع، والمصروف يُسجَّل بمبلغه وتاريخه؛ ولا يحتاج أيٌّ منهما قيداً مزدوجاً. أما `Accounts` و`JournalEntries` و`JournalEntryLines` و`SystemAccountMappings` وسندات القبض/الصرف اليدوية فهي **آلية محاسبة بحتة** بلا وظيفة في حلقة «بيع → خصم مخزون → صندوق» ⇒ V2.
> **أثر جانبي إلزامي عند الإزالة:** العمودان `CashBoxes.AccountId` و`Expenses.ExpenseAccountId` (كلاهما `int NOT NULL` إلى `Accounts`) لا معنى لهما بعد نقل `Accounts` ⇒ يُزالان في الموجة نفسها. وكذلك `Customers.AccountId` و`Suppliers.AccountId` (§8 — جدول الأثر).

---

# 4) تعريفات الجداول (Module-by-Module)

> **الأعمدة أدناه مأخوذة حرفياً من `SalesDbContextModelSnapshot.cs`** (النوع = نوع SQL الفعلي، والـNullable مقيس من خصائص الـModel).
> **الاستثناء الوحيد:** أعمدة هوية المنفّذ (`CreatedByUserName` · `UpdatedByUserName` · `ReviewedByUserName` · `ClosedByUserName` · `CashierUserName` · `UserName`) هي **الهدف المعتمد** بقرار المالك 2026-11-17 (§2) ولم تكن بعد في الـsnapshot وقت تحرير هذا الملف — تُنفَّذ بالترتيب الملزم في §2.
> `*Id` بعد كل عمود `Id` يشير إلى مفتاح خارجي بـ`Restrict`؛ القيود المفصّلة في `SalesSystem.DataAccess/Configurations/`.

## Module 1: النواة والأمان

### 1.1 Customers
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 16

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `Address` | `nvarchar(500)` | NULL |
| `CategoryId` | `int` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `CreditLimit` | `decimal(18,2)` | NOT NULL |
| `Email` | `nvarchar(100)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(200)` | NOT NULL |
| `Notes` | `nvarchar(1000)` | NULL |
| `PaymentTermDays` | `int` | NULL |
| `Phone` | `nvarchar(20)` | NULL |
| `TaxNumber` | `nvarchar(30)` | NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 1.2 Suppliers
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 16

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `Address` | `nvarchar(500)` | NULL |
| `CategoryId` | `int` | NULL |
| `CommercialRegisterNo` | `nvarchar(max)` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `CreditLimit` | `decimal(18,2)` | NOT NULL |
| `Email` | `nvarchar(100)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(200)` | NOT NULL |
| `Notes` | `nvarchar(1000)` | NULL |
| `Phone` | `nvarchar(20)` | NULL |
| `TaxNumber` | `nvarchar(30)` | NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 1.3 Users
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 12

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `IsLocked` | `bit` | NOT NULL |
| `LastLoginAt` | `datetime2` | NULL |
| `LoginAttempts` | `smallint` | NOT NULL |
| `MustChangePassword` | `bit` | NOT NULL |
| `PasswordHash` | `nvarchar(256)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `UserName` | `nvarchar(50)` | NOT NULL |

## Module 2: التنظيم والإعدادات

### 2.1 Warehouses
**المفتاح:** `smallint IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 10

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `smallint` | NOT NULL |
| `Address` | `nvarchar(300)` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(150)` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `Phone` | `nvarchar(30)` | NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 2.2 CompanySettings
**المفتاح:** `tinyint IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `tinyint` | NOT NULL |
| `Address` | `nvarchar(300)` | NULL |
| `CompanyName` | `nvarchar(200)` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `Email` | `nvarchar(100)` | NULL |
| `LogoPath` | `nvarchar(500)` | NULL |
| `Phone` | `nvarchar(30)` | NULL |
| `TaxNumber` | `nvarchar(50)` | NULL |
| `UpdatedAt` | `datetime2` | NULL |

### 2.3 SystemSettings
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 11

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Category` | `nvarchar(100)` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(1000)` | NULL |
| `DisplayName` | `nvarchar(200)` | NOT NULL |
| `SettingKey` | `nvarchar(100)` | NOT NULL |
| `SettingType` | `tinyint` | NOT NULL |
| `SettingValue` | `nvarchar(500)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 2.4 DocumentSequences
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 7

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `DocumentType` | `nvarchar(50)` | NOT NULL |
| `NextNumber` | `int` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

## Module 3: المنتجات

### 3.1 ProductCategories
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 8

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(500)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(100)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 3.2 Products
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 17

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Barcode` | `varchar` | NULL |
| `CategoryId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(500)` | NULL |
| `ExpiryAlertDays` | `smallint` | NULL |
| `ForeignName` | `nvarchar(200)` | NULL |
| `ImagePath` | `nvarchar(500)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `LocationInStore` | `nvarchar(100)` | NULL |
| `MinSalePrice` | `decimal(18,2)` | NOT NULL |
| `Name` | `nvarchar(200)` | NOT NULL |
| `ReorderLevel` | `decimal(18,3)` | NOT NULL |
| `TrackExpiry` | `bit` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 3.3 Units
**المفتاح:** `smallint IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `smallint` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `IsSystem` | `bit` | NOT NULL |
| `Name` | `nvarchar(50)` | NOT NULL |
| `Symbol` | `nvarchar(20)` | NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 3.4 ProductUnits
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Barcode` | `nvarchar(50)` | NULL |
| `CostPrice` | `decimal(18,2)` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Factor` | `decimal(18,3)` | NOT NULL |
| `IsBaseUnit` | `bit` | NOT NULL |
| `MaxDiscountAmount` | `decimal(18,2)` | NULL |
| `ProductId` | `int` | NOT NULL |
| `SalePrice` | `decimal(18,2)` | NOT NULL |
| `UnitId` | `smallint` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WholesalePrice` | `decimal(18,2)` | NULL |

### 3.5 ProductPrices
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `EffectiveFrom` | `date` | NOT NULL |
| `EffectiveTo` | `date` | NULL |
| `Price` | `decimal(18,2)` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

## Module 4: الصندوق والمصروفات (تشغيلي)

> **هذه الوحدة كانت «المحاسبة» بثمانية جداول في V1 — وصارت في V3 جدولين تشغيليين فقط.**
> الجداول الستة المحاسبية (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) نُقلت بتعريفاتها الكاملة إلى `docs/database-schema-v2-deferred.md` — قسم «المحاسبة». **لا تُضاف شاشة ولا خدمة محاسبية جديدة في V1.**
> **العمودان `CashBoxes.AccountId` و`Expenses.ExpenseAccountId` ما زالا في الـsnapshot** لأنهما مرتبطان بـ`Accounts`؛ يُزالان في موجة إزالة المحاسبة لا قبلها (§8).

### 4.1 CashBoxes
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(300)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(150)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 4.2 Expenses
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `ExpenseAccountId` | `int` | NOT NULL |
| `ExpenseDate` | `date` | NOT NULL |
| `ExpenseNo` | `int` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

## Module 5: المخزون

### 5.1 WarehouseStocks
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 8

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `ProductId` | `int` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 5.2 InventoryBatches
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 16

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `BatchNo` | `nvarchar(50)` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `ExpiryDate` | `date` | NULL |
| `IsClosed` | `bit` | NOT NULL |
| `ProductId` | `int` | NOT NULL |
| `PurchaseInvoiceId` | `int` | NULL |
| `PurchaseInvoiceLineId` | `int` | NULL |
| `QuantityReceived` | `decimal(18,3)` | NOT NULL |
| `QuantityRemaining` | `decimal(18,3)` | NOT NULL |
| `SupplierBatchNo` | `nvarchar(100)` | NULL |
| `UnitCost` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 5.3 InventoryTransactions
**المفتاح:** `int IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `MovementType` | `tinyint` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `ReferenceId` | `int` | NULL |
| `ReferenceType` | `tinyint` | NULL |
| `TransactionNo` | `nvarchar(50)` | NOT NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 5.4 InventoryTransactionLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 8

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `BatchNo` | `nvarchar(50)` | NULL |
| `ExpiryDate` | `date` | NULL |
| `InventoryTransactionId` | `int` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |
| `UnitCost` | `decimal(18,2)` | NOT NULL |
| `WarehouseId` | `smallint` | NULL |

### 5.5 InventoryAdjustments
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 10

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AdjustmentNo` | `nvarchar(50)` | NOT NULL |
| `AdjustmentType` | `tinyint` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `Reason` | `nvarchar(500)` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 5.6 InventoryAdjustmentLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 6

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `ActualQuantity` | `decimal(18,3)` | NOT NULL |
| `ExpectedQuantity` | `decimal(18,3)` | NOT NULL |
| `InventoryAdjustmentId` | `int` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `UnitCost` | `decimal(18,2)` | NOT NULL |

## Module 6: المبيعات

### 6.1 SalesInvoices
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 24

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `CustomerId` | `int` | NOT NULL |
| `DiscountAmount` | `decimal(18,2)` | NOT NULL |
| `DiscountRate` | `decimal(18,2)` | NULL |
| `DiscountType` | `tinyint` | NOT NULL |
| `InvoiceDate` | `date` | NOT NULL |
| `InvoiceNo` | `int` | NOT NULL |
| `NetTotal` | `decimal(18,2)` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `OtherCharges` | `decimal(18,2)` | NOT NULL |
| `PaidAmount` | `decimal(18,2)` | NOT NULL |
| `PaymentType` | `tinyint` | NOT NULL |
| `PostedAt` | `datetime2` | NULL |
| `RemainingAmount` | `decimal(18,2)` | NOT NULL |
| `Status` | `tinyint` | NOT NULL |
| `SubTotal` | `decimal(18,2)` | NOT NULL |
| `TaxAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 6.2 SalesInvoiceLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 13

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `DiscountAmount` | `decimal(18,2)` | NOT NULL |
| `DiscountRate` | `decimal(18,2)` | NULL |
| `DiscountType` | `tinyint` | NOT NULL |
| `ExpiryDate` | `date` | NULL |
| `LineTotal` | `decimal(18,2)` | NOT NULL |
| `ProductId` | `int` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `ProfitAmount` | `decimal(18,2)` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |
| `SalesInvoiceId` | `int` | NOT NULL |
| `UnitCost` | `decimal(18,2)` | NOT NULL |
| `UnitPrice` | `decimal(18,2)` | NOT NULL |

### 6.3 SalesReturns
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 20

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `CustomerId` | `int` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `RefundAmount` | `decimal(18,2)` | NOT NULL |
| `ReturnDate` | `date` | NOT NULL |
| `ReturnNo` | `int` | NOT NULL |
| `ReturnReason` | `nvarchar(500)` | NULL |
| `ReturnedChargeAmount` | `decimal(18,2)` | NOT NULL |
| `ReturnedDiscountAmount` | `decimal(18,2)` | NOT NULL |
| `ReturnedTaxAmount` | `decimal(18,2)` | NOT NULL |
| `SalesInvoiceId` | `int` | NOT NULL |
| `Status` | `tinyint` | NOT NULL |
| `TotalAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 6.4 SalesReturnLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 5

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |
| `SalesInvoiceLineId` | `int` | NOT NULL |
| `SalesReturnId` | `int` | NOT NULL |

### 6.5 CustomerReceipts
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 15

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `CustomerId` | `int` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PaymentMethod` | `tinyint` | NOT NULL |
| `PostedAt` | `datetime2` | NULL |
| `ReceiptDate` | `date` | NOT NULL |
| `ReceiptNo` | `int` | NOT NULL |
| `Status` | `tinyint` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 6.6 CustomerReceiptApplications
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 4

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AppliedAmount` | `decimal(18,2)` | NOT NULL |
| `CustomerReceiptId` | `int` | NOT NULL |
| `SalesInvoiceId` | `int` | NOT NULL |

## Module 7: المشتريات

### 7.1 PurchaseInvoices
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 26

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AttachmentPath` | `nvarchar(255)` | NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `DiscountAmount` | `decimal(18,2)` | NOT NULL |
| `DiscountRate` | `decimal(18,2)` | NULL |
| `DiscountType` | `tinyint` | NOT NULL |
| `InvoiceDate` | `date` | NOT NULL |
| `InvoiceNo` | `int` | NOT NULL |
| `NetTotal` | `decimal(18,2)` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `OtherCharges` | `decimal(18,2)` | NOT NULL |
| `PaidAmount` | `decimal(18,2)` | NOT NULL |
| `PaymentType` | `tinyint` | NOT NULL |
| `PostedAt` | `datetime2` | NULL |
| `RemainingAmount` | `decimal(18,2)` | NOT NULL |
| `Status` | `tinyint` | NOT NULL |
| `SubTotal` | `decimal(18,2)` | NOT NULL |
| `SupplierId` | `int` | NOT NULL |
| `SupplierInvoiceNo` | `nvarchar(200)` | NULL |
| `TaxAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 7.2 PurchaseInvoiceLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 12

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AdditionalFeesAmount` | `decimal(18,2)` | NOT NULL |
| `DiscountAmount` | `decimal(18,2)` | NOT NULL |
| `DiscountRate` | `decimal(18,2)` | NULL |
| `DiscountType` | `tinyint` | NOT NULL |
| `LandedUnitCost` | `decimal(18,2)` | NOT NULL |
| `LineTotal` | `decimal(18,2)` | NOT NULL |
| `ProductId` | `int` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `PurchaseInvoiceId` | `int` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |
| `UnitPrice` | `decimal(18,2)` | NOT NULL |

### 7.3 PurchaseReturns
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 20

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `DiscountRate` | `decimal(18,2)` | NULL |
| `DiscountType` | `tinyint` | NOT NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `PurchaseInvoiceId` | `int` | NULL |
| `ReturnDate` | `date` | NOT NULL |
| `ReturnNo` | `int` | NOT NULL |
| `ReturnedChargeAmount` | `decimal(18,2)` | NOT NULL |
| `ReturnedDiscountAmount` | `decimal(18,2)` | NOT NULL |
| `ReturnedTaxAmount` | `decimal(18,2)` | NOT NULL |
| `Status` | `tinyint` | NOT NULL |
| `SupplierId` | `int` | NOT NULL |
| `TotalAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `WarehouseId` | `smallint` | NOT NULL |

### 7.4 PurchaseReturnLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 7

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `ProductId` | `int` | NOT NULL |
| `ProductUnitId` | `int` | NOT NULL |
| `PurchaseInvoiceLineId` | `int` | NULL |
| `PurchaseReturnId` | `int` | NOT NULL |
| `Quantity` | `decimal(18,3)` | NOT NULL |

### 7.5 SupplierPayments
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 16

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PaymentDate` | `date` | NOT NULL |
| `PaymentMethod` | `tinyint` | NOT NULL |
| `PaymentNo` | `int` | NOT NULL |
| `PostedAt` | `datetime2` | NULL |
| `ReferenceNo` | `nvarchar(100)` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `SupplierId` | `int` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 7.6 SupplierPaymentApplications
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 4

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AppliedAmount` | `decimal(18,2)` | NOT NULL |
| `PurchaseInvoiceId` | `int` | NOT NULL |
| `SupplierPaymentId` | `int` | NOT NULL |

## Module 8: البنية التحتية

### 8.1 AuditLogs
**المفتاح:** `bigint IDENTITY` · **الفئة:** `LongEntity` · **الأعمدة:** 10

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `bigint` | NOT NULL |
| `Action` | `nvarchar(100)` | NOT NULL |
| `ChangedColumns` | `nvarchar(500)` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `EntityId` | `int` | NULL |
| `EntityType` | `nvarchar(100)` | NULL |
| `IpAddress` | `nvarchar(50)` | NULL |
| `NewValues` | `nvarchar(max)` | NULL |
| `OldValues` | `nvarchar(max)` | NULL |
| `UserName` | `nvarchar(100)` | NULL |

### 8.2 SystemLogs
**المفتاح:** `bigint IDENTITY` · **الفئة:** `LongEntity` · **الأعمدة:** 9

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `bigint` | NOT NULL |
| `ActionName` | `nvarchar(200)` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `Exception` | `nvarchar(max)` | NULL |
| `IpAddress` | `nvarchar(50)` | NULL |
| `Level` | `tinyint` | NOT NULL |
| `Message` | `nvarchar(max)` | NOT NULL |
| `Source` | `nvarchar(200)` | NULL |
| `UserName` | `nvarchar(100)` | NULL |

---

# 5) مرجع التعدادات (Enums)

| Enum | القيم |
|---|---|
| `InvoiceStatus` | Draft=1 · Posted=2 · Cancelled=3 |
| `PaymentType` | Cash=1 · Credit=2 · Mixed=3 |
| `AccountNature` | Asset=1 · Liability=2 · Equity=3 · Revenue=4 · Expense=5 — **V2 (محاسبة)** |
| `JournalEntryType` | Manual=1 · Sales=2 · Purchase=3 · Receipt=4 · Payment=5 · Inventory=6 · Adjustment=7 — **V2 (محاسبة)** |
| `InventoryTransactionType` | Purchase=1 · PurchaseReturn=2 · Sale=3 · SaleReturn=4 · TransferOut=5 · TransferIn=6 · Count=7 · Adjustment=8 · Damage=9 · OpeningBalance=10 · InternalIssue=11 · InternalReceipt=12 |
| `MovementType` | PurchaseIn=1 · SaleOut=2 · SaleReturnIn=3 · PurchaseReturnOut=4 · TransferOut=5 · TransferIn=6 · Adjustment=7 |
| `StockAdjustmentType` | Addition=1 · Deduction=2 · Correction=3 |
| `AdjustmentType` | Opening=1 · Increase=2 · Shortage=3 · Damage=4 |
| `PaymentMethod` | Cash=1 · Cheque=2 · BankTransfer=3 · CreditCard=4 |
| `ChequeStatus` | Pending=1 · Cleared=2 · Bounced=3 · Cancelled=4 |
| `DiscountType` | Amount=0 · Percentage=1 |
| `SettingsType` | String=1 · Integer=2 · Decimal=3 · Boolean=4 |
| `UserRole` | Admin=1 · Manager=2 · Accountant=3 · Treasurer=4 · Cashier=5 · WarehouseSupervisor=6 · SalesEmployee=7 · Observer=8 — **وسم جلسة فقط** (§2.13 في `AGENTS.md`)، لا نظام صلاحيات |
| `LogLevel` | Info=1 · Warning=2 · Error=3 · Critical=4 |

**الأعمدة `Status` (tinyint) في كل الكيانات المستندية:** `1=Draft` · `2=Posted` · `3=Cancelled` — لا حذف نهائي لمستند مرحّل.

> **التعدادان المحاسبيان** (`AccountNature` · `JournalEntryType`) يخصّان جداول V2 المحاسبية ويُنقلان معها — لا يُستهلكان في V1 بعد موجة الإزالة.

---

# 6) المعادلات المالية الأساسية

## فاتورة البيع
```
LineTotal       = (Quantity × UnitPrice) - DiscountAmount
SubTotal        = SUM(LineTotal)
NetTotal        = SubTotal - DiscountAmount + TaxAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint:     PaidAmount <= NetTotal
```

## فاتورة الشراء
```
LineTotal       = (Quantity × UnitCost) - DiscountAmount
SubTotal        = SUM(LineTotal)
NetTotal        = SubTotal - DiscountAmount + TaxAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint:     PaidAmount <= NetTotal
```

## تكلفة FIFO (الطريقة الافتراضية)
```
عند البيع          : تُستهلك أقدم دفعة أولاً (أقدم QuantityRemaining > 0)
عند مرتجع الشراء   : يُخصم من دفعة الفاتورة الأصلية
عند FEFO          : تُستهلك أقرب تاريخ انتهاء أولاً (عند TrackExpiry = true)
```

> **قاعدة القيد المزدوج (`Debit/Credit`) أُخرجت من V1 مع الوحدة المحاسبية** — نصّها الكامل في `docs/database-schema-v2-deferred.md` قسم «المحاسبة». **V1 لا يولّد أي قيد محاسبي ولا يعتمد على `JournalEntries` في أي معادلة.**

---

# 7) ما أُزيل نهائياً من الإصدار الأول (ليس مؤجلاً إلى V2)

| العنصر | ما أُزيل | الدليل |
|---|---|---|
| **الفروع (Branches)** | الكيان + التكوين + الخدمات + الشاشات + **الجدول** + **19 عمود `BranchId`** في 19 جدولاً | ترحيل `20261117054708_RemoveBranchAndBranchId`: 19 `DropForeignKey` + 19 `DropIndex` + 19 `DropColumn` + 1 `DropTable` |
| **الموظفون (Employees)** | الشاشة + العقد + الـMock + الـDTO + `CreateEmployeeAccountAsync` + **`Users.EmployeeId`** | ترحيل `20261117063917_RemoveBaseCurrencyAndEmployeeColumns` (عمود واحد ضمنه) — لم يكن هناك جدول `Employees` قط |
| **العملات (Currencies)** | شاشة العملة + عنصر القائمة + `CurrencyId` + `ExchangeRate` + **13 عموداً** (`BaseNetTotal` ×7 · `CostInBaseCurrency` ×6) | نفس الترحيل: 14 `DropColumn` إجمالاً (13 عملة + `EmployeeId`) بصفر عملية أخرى |

> **مهم:** «مؤجل إلى V2» في هذا المستودع يعني **لا كود**. الفروع والموظفون اختفوا من الكود **ومن قاعدة البيانات**؛ العملات اختفت من الكود وحُذفت أعمدتها. إعادة أي منها = ميزة جديدة تمرّ بالدورة الكاملة (Mock ⇒ مراجعة المالك ⇒ موافقة ⇒ باك-إند ⇒ ترحيل بموافقة هيكل).
>
> **استثناء واحد صريح (V3):** الجداول المحاسبية الستة نُقلت إلى V2 **بالقرار** قبل تنفيذ الإزالة، فكودها **ما زال قائماً** حتى تُنفَّذ موجة الإزالة في §8.1 — وهذا هو الفارق الوحيد المسموح بين «مؤجل» و«لا كود».

---

# 8) المؤجل إلى الإصدار الثاني (V2)

كل جدول مؤجل — تصميمه وسببه — في ملف واحد: **`docs/database-schema-v2-deferred.md`**.

| المجموعة | الجداول المؤجلة |
|---|---|
| الأمان والأدوار | `Roles` · `UserSessions` |
| الإعدادات | `Taxes` · `FiscalYears` · `Notifications` · `Attachments` |
| المحاسبة (تصميم) | `AccountCategories` · `Banks` · `TreasuryTransfers` · `CostCenters` · `VoucherInvoiceAllocations` · `FiscalPeriods` |
| المخزون | `InventoryCounts` · `InventoryCountLines` · `WarehouseTransfers` · `WarehouseTransferLines` |
| العملات | `Currencies` · `CurrencyRates` |
| البيانات التجارية | `Departments` · `CustomerContacts` · `SupplierContacts` |
| عروض الأسعار | `SalesQuotations` · `SalesQuotationItems` |
| **المحاسبة (نُقلت من V1 في V3)** | `Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings` |

> **عددها 29 جدولاً** (23 مؤجلاً سابقاً + 6 محاسبية نُقلت في V3) — والعدد مرجعي في ملف V2 لا هنا (`RULE-N174`: رقم واحد في مكان واحد).

## 8.1 جدول الأثر — ما يجب أن يتغيّر عند تنفيذ إزالة المحاسبة (مقيس 2026-11-17)

> **هذا الجدول خطة تنفيذ، وقد نُفِّذ شِقّه الكودي بالكامل (2026-11-17 — `TASK-011` §13).** كل سطر مقيس بأمر على المستودع. **المُنفَّذ:** حذف الكيانات والخدمات والبذور والشاشات والعقود والتنفيذات والتقارير المحاسبية، وإزالة `AccountId`/`ExpenseAccountId` من الموديل (الصفوف 1–10 كودياً). **غير المُنفَّذ:** إسقاط الأعمدة والجداول على **القاعدة الحيّة** — يبقى للمالك بقرار `RULE-N198`، والترحيلات أُزيلت كلها بأمره «and remove all migrations» فيولّد `InitialCreate` بنفسه.

| # | العنصر | الأثر المطلوب | المقيس اليوم |
|---|---|---|---|
| 1 | `SalesSystem.Models/Accounting/` | حذف الكيانات الستة + التعدادات الخمسة | **11** ملفاً (6 كيانات + 5 تعدادات) |
| 2 | `SalesSystem.Business/Accounting/` | حذف/تعطيل الخدمات | **5** ملفات: `AccountingIntegrationService` · `JournalEntryService` · `JournalEntryNumberGenerator` · `JournalEntryNumberResult` · `SystemAccountService` |
| 3 | `SalesSystem.Business/Services/` | حذف خدمات الدليل | **3** ملفات: `AccountService` · `AccountCodeGeneratorService` · `AccountLinkService` |
| 4 | `SalesSystem.DataAccess/Seeders/` | إزالة بذور شجرة الحسابات وخرائط الحسابات | **1** ملف: `AccountingSeeder.cs` |
| 5 | `SalesSystem.Desktop/Screens/Accounting/` | حذف النماذج الثلاثة + عناصر القائمة + تسجيلات DI | **3** نماذج: `frmAccounts` · `frmAccountsTree` · `frmJournalEntries` |
| 6 | `Customers.AccountId` · `Suppliers.AccountId` | **إزالة العمودين** — رصيد العميل/المورّد يُحسب من فواتيره (مبيعات − مرتجعات − سندات قبض) | عمودان `int NOT NULL` |
| 7 | `CashBoxes.AccountId` · `Expenses.ExpenseAccountId` | **إزالة العمودين** — الصندوق والمصروف يصيران تشغيليين بلا دليل حسابات | عمودان `int NOT NULL` |
| 8 | `JournalEntryLines.AccountId` · `SystemAccountMapping.AccountId` · `ReceiptVoucher/PaymentVoucher.AccountId` | تُحذف مع جداولها | داخل الجداول المنقولة |
| 9 | `Services/Interfaces` · `Services/Direct` · `Services/Mock` · `DTO` | حذف عقود/تنفيذات الدليل والقيود والسندات اليدوية | يُحصر بالأمر وقت الموجة |
| 10 | `documents / التقارير المحاسبية` | أي تقرير يقرأ `Accounts`/`JournalEntries` يُحذف أو يُحوَّل إلى تقرير تشغيلي | يُحصر وقت الموجة |

> **قيد إلزامي (`RULE-N198` · §2.42 في `AGENTS.md`):** إزالة أي جدول/عمود = **`DROP`**، ويحتاج **موافقة مالك مكتوبة ومؤرَّخة مستقلة** عن موافقة السلوك، ونسخة احتياطية قبلها، وترحيلاً واحداً مخصّصاً لا يُخلط بغيره. **لا يُنفّذ وكيل هذه الموجة بلا هذا النص.**
>
> **ترتيب آمن مقترح (بعد الموافقة):** (1) نسخة احتياطية `COPY_ONLY` · (2) إزالة استهلاك `Accounts` من `Customers`/`Suppliers`/`CashBoxes`/`Expenses` في الكود أولاً · (3) ترحيل إزالة الأعمدة الستة · (4) ترحيل إزالة الجداول الستة · (5) حذف الخدمات والشاشات والبذور · (6) بناء 0 أخطاء + قائمة اختبار يدوي.
>
> **الحالة بعد التنفيذ (2026-11-17):** الخطوات (2) و(5) و(6) **منفَّذة** — صفر مرجع محاسبي في الكود، والبناء `--no-incremental` = **0 Errors / 100 Warnings**، وكتالوج الموديل **34** جدولاً (`ToTable` فريدة في `Configurations/`) وصفر جدول محاسبي. **الخطوتان (3) و(4) لم تُنفَّذا على القاعدة الحيّة**، ولم يُولَّد أي ترحيل: المالك أزال كل الترحيلات («and remove all migrations»). ⚠️ **قبل تشغيل التطبيق على قاعدة قديمة:** 8 أعمدة `NOT NULL` إلى `Accounts` ما زالت هناك ⇒ إدخال عميل/مورّد/صندوق/مصروف سيفشل حتى تُسقَط أو تُعاد إنشاء القاعدة.

---

# 9) انحرافات مقيسة بين التصميم والتنفيذ (Known Deviations)

هذه انحرافات **حقيقية ومقصودة**، مسجَّلة بوصفها كذلك لا كأخطاء:

> **الصفوف 5 · 6 · 10 · 11 · 12 تخص جداول نُقلت إلى V2 في V3** — تبقى موصوفة هنا لأن كودها ما زال في المستودع حتى تُنفَّذ موجة الإزالة (§8.1)، وتُحذف من هذا الجدول مع الجداول نفسها. **الصفوف 1 · 2 · 3 · 4 · 7 · 8 · 9 تخص V1 وتبقى.**

| # | العنصر | الملاحظة المقيسة |
|---|---|---|
| 1 | `InventoryTransactions` | لا يوجد `TransactionDate` ولا `Status`؛ الرقم `TransactionNo` نصي `nvarchar(50)`، والحركة يُشار إليها بـ`MovementType` + `ReferenceType`/`ReferenceId` |
| 2 | `InventoryTransactionLines` | لا يوجد `ProductId`/`BatchId`/`TotalCost` — السطر يحمل `ProductUnitId` والدفعة **نصياً** بـ`BatchNo` + `ExpiryDate` |
| 3 | `InventoryAdjustments` | الرقم `AdjustmentNo` نصي `nvarchar(50)` لا `int`، ولا يوجد `AdjustmentDate` |
| 4 | `InventoryBatches.BatchNo` | نصي `nvarchar(50)` (لا `int`) |
| 5 | `Accounts` | لا يوجد `CategoryId` ولا `CostCenterId` — تصنيفات الدليل ومراكز التكلفة مؤجلة (`CostCenters` في جدول V2) |
| 6 | `JournalEntries` | لا يوجد `FiscalYearId`؛ فترات وسنوات مالية مؤجلة |
| 7 | أعمدة التدقيق | ✅ **منفَّذ — نصّية الآن** (`CreatedByUserName` · `UpdatedByUserName` `nvarchar(100)`)، والحقلان الرقميان أُزيلا: عدد `CreatedByUserId` في اللقطة = **0** و`CreatedByUserName` = **27** (قياس 2026-11-17) — والتعبئة الرجعية على القاعدة الحقيقية مسؤولية المالك |
| 8 | `Products.Barcode` | عمود `varchar` للبحث السريع؛ المصدر الرسمي للباركود هو `ProductUnits.Barcode` |
| 9 | `ProductUnits.Barcode` | `nvarchar(50)` في القاعدة (يوافق `varchar(50)` عملياً لكونه ASCII) |
| 10 | `SystemAccountMappings` | فهرس فريد على `MappingKey` وحده (صف واحد لكل مفتاح) — بلا تجاوز لكل فرع (الفرع أُزيل) |
| 11 | `Accounts` | لا أعمدة `TreePath` · `FinancialStatement` · `IsSubAccount` · `Balance` · `ParentAccountCode` · `CategoryLabel` — كلها **مشتقة في BLL** (§4.1.1) ولا تُخزَّن |
| 12 | `Accounts` | **لا عمود عملة** — بقرار المالك 2026-11-17 («remove cuurencey»)، ووحدة العملات مؤجلة V2 |

---

# 10) قائمة تحقق يدوي للتحقق من هذا الملف

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | `Select-String -Path SalesDbContextModelSnapshot.cs -Pattern 'ToTable\("'` وعدّ الأسماء الفريدة | **40** اليوم (34 هدف V3 + 6 محاسبية) — وبعد موجة الإزالة **34**. أي رقم ثالث = خطأ |
| 2 | ابحث في الملف عن `V2 DEFERRED` أو `Branches` أو `CurrencyId` أو `BaseNetTotal` | **صفر** نتيجة |
| 3 | افتح `docs/database-schema-v2-deferred.md` | يحوي **29** جدولاً مؤجلاً (23 سابقاً + 6 محاسبية نُقلت في V3) وتعريفاتها الكاملة |
| 4 | `dotnet ef migrations list` | الترحيلات الحالية: الأساس `20261117085853_InitialCreate` وحده (إعادة تأسيس المالك) — والمالك يدير الترحيلات بنفسه |
| 5 | تحقق أن §3 يجمع 34: 3 + 4 + 5 + 2 + 6 + 6 + 6 + 2 | **34** — والمحاسبة **6** خارج المجموع ومعلَنة في §8 |
| 6 | ابحث في §4 عن `### 4.x` تحت `## Module 4` | **جدولان فقط**: `4.1 CashBoxes` · `4.2 Expenses` — لا `Accounts` ولا `JournalEntries` |
| 7 | ابحث في §4 عن كتلة `Accounts` هناك | **صفر** — تعريفها الكامل في ملف V2 (قسم المحاسبة) |
| 8 | ابحث في الملف عن `Debit`/`Credit` | **صفر** في المعادلات — قاعدة القيد المزدوج في ملف V2 |

---

# 11) سجل تغييرات الهيكل (Schema Change Log)

| # | التاريخ | التغيير | النوع | نصّ الموافقة | الحالة |
|---|---------|---------|-------|--------------|--------|
| 1 | 2026-11-17 | **عزل وحدة `Users`:** لا مفتاح أجنبي إلى `Users` من أي جدول | تصميم | «اريد ان افصل جدول المستخدمين عن بقيت الجدول الاخرى لا ان يرتبط بالمعرف لكي نجزء النظام إلى وحدات صغير منفصلة واسجل اسم المستخدم الحالي من على الوجهات كنص» | ✅ موثَّق |
| 2 | 2026-11-17 | **استبدال أعمدة التدقيق الرقمية بأسماء نصية** `nvarchar(100)`: إضافة `CreatedByUserName` · `UpdatedByUserName` · `ReviewedByUserName` · `ClosedByUserName` · `CashierUserName` · `UserName`، وإزالة `CreatedByUserId` · `UpdatedByUserId` · `ReviewedByUserId` · `ClosedByUserId` · `CashierUserId` · `UserId` | إضافة ثم **إزالة** (بموافقة مالك صريحة) | «remove CreatedByUserId and UpdateByUserId and repllase by CreatedByUserName and UpdateByUserName for all system» | 🔄 معتمد — يُنفَّذ بالترتيب الملزم في §2 (نسخة احتياطية → إضافة → تعبئة رجعية → تحويل → إزالة) |
| 3 | 2026-11-17 | **حقول دليل الحسابات الهرمية:** إضافة `NormalBalance` (`tinyint NULL`) و`AllowCostCenter` (`bit NOT NULL DEFAULT 0`) إلى `Accounts` — **زيادة آمنة فقط**، بلا حذف ولا إعادة تسمية ولا `UPDATE` جماعي | إضافة (`RULE-N198`) | «update … docs/database-schema.md and all project code … by new this details about accounts table» + «remove cuurencey» | ✅ **منفَّذ** — الكيان والتهيئة والـDTO وBLL و`Direct`/`Mock` والشاشة. العمودان داخل اللقطة الأساسية `20261117085853_InitialCreate` (السطر 32 للعمود `AllowCostCenter`) — **والترحيل ملك المالك** («I will do migrations»). باقي سمات المواصفة مشتقة لا أعمدة (§4.1.1 في V2)، و`CostCenters`/`Banks` مؤجلة |
| 4 | 2026-11-17 | **V3 — إخراج الوحدة المحاسبية من الإصدار الأول:** نقل 6 جداول (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) من V1 إلى V2، وبقاء `CashBoxes` · `Expenses` تشغيليين، وخروج قاعدة القيد المزدوج من معادلات V1 ⇒ **V1 = 34 جدولاً** | **تصميم/نطاق** ثم **إزالة كود** (لا `DROP` على القاعدة الحيّة) | «and reamning Accounting tables from v1 to v2» + «remove all Accounting code» + «and remove all migrations» | ✅ **الكود مُزال (2026-11-17)** — 84 ملفاً محذوفاً · 72 معدّلاً · البناء **0 Errors / 100 Warnings** · كتالوج الموديل **34** جدولاً · صفر ملف ترحيل (بأمر المالك). ⏸ **إسقاط الأعمدة/الجداول على القاعدة الحيّة للمالك** (`RULE-N198`) — التفصيل في §8.1 و`TASK-011` §13 |

> **ملاحظة إلزامية على أي مراجعة:** حتى إتمام خطوة «التحويل» في §2، **الـsnapshot هو المصدر الواقعي** وهذا الجدول هو الهدف. المراجعة التي تفترض وجود الأعمدة النصية اليوم مراجعة على أساس خاطئ.

---

**الإصدار:** **V3** (MVP — Pure POS & Inventory · المحاسبة مؤجلة) · **التاريخ:** 2026-11-17 · **الملف المرجعي المؤجل:** `docs/database-schema-v2-deferred.md`
