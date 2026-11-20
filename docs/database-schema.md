# Database Schema — **Version 3 (MVP — Pure POS & Inventory)**

# Sales Management System — v5.0.0 (2-Tier WinForms · N-Layer)

**Platform:** SQL Server 2019+ · EF Core 10
**الجداول (هدف V3):** **33** · decimal-only financials · nvarchar text · soft delete · all FK `Restrict`
**Module boundary:** `Users` وحدة مستقلة — **لا FK إليها**، وهويات المنفّذ نصوص `nvarchar(100)` (§2)
**نطاق V1 (Pure POS & Inventory):** عملاء وموردون ومستخدمون وتنظيم وأصناف ومخزون ومبيعات ومشتريات وصندوق ومصروفات وسجلات — ولا شيء غير ذلك. كل ما هو خارج هذه الحلقة في `docs/database-schema-v2-deferred.md` (**مرجع تصميم فقط — لا كود ولا كيان ولا ترحيل**).

> **العدد المقيس اليوم (2026-11-20) — لا يُنقل من موجة سابقة (`RULE-N195`/`RULE-N196`):** كتالوج الموديل = **33 جدولاً** بالضبط، وكل جدول في هذا الملف موجود في `Configurations/` والعكس. **لا ملف ترحيل ولا `ModelSnapshot` في المستودع** (`Migrations/` = **0 ملف** و**0 مسار في git**؛ قد يبقى المجلد فارغاً على القرص المحلي) — أزالها المالك بأمره («and remove all migrations») وهو من يولّد `InitialCreate` بنفسه.

---

> **هذا الملف هو الإصدار الثالث (V3) — المصدر الوحيد لهيكل قاعدة البيانات المستهدف بعد إعادة ترتيب الإصدارات.**

**خريطة الإصدارات (V1 → V2 → V3):**

| الإصدار | معناه | أين يعيش | العدد |
|---|---|---|---|
| **V1 (MVP)** | نواة التشغيل فقط: عملاء · موردون · مستخدمون · تنظيم · أصناف ووحدات · مخزون · مبيعات · مشتريات · صندوق ومصروفات · سجلات | **هذا الملف** | **33** |
| **V2 (مؤجل)** | كل ما هو خارج الحلقة الأساسية: أدوار وجلسات · ضرائب وسنوات مالية · إشعارات ومرفقات · بنوك ومراكز تكلفة · تحويلات مخزنية وجرد · عملات وأسعار صرف · جهات اتصال · عروض أسعار | `docs/database-schema-v2-deferred.md` | **29** |
| **V3 (هذا الإصدار)** | قرار المالك 2026-11-17: تضييق V1 إلى «بيع → خصم مخزون → صندوق» · ثم 2026-11-18: إزالة `TaxAmount`/`TaxNumber` · ثم إزالة `ProductPrices` (الأسعار على `ProductUnits`) | هذا الملف (النسخة الحالية) | 33 |

> **كل جدول مذكور هنا في §4 له كيان وتكوين في المستودع.**
> الجداول المؤجلة كلها في ملف واحد: `docs/database-schema-v2-deferred.md` (مرجع تصميم فقط — **لا كود ولا كيان ولا ترحيل** لأي منها).
>
> **بروتوكول بقاء هذا الملف صادقاً (يُنفَّذ قبل كل ادّعاء رقمي — `RULE-N195`/`RULE-N196`):**
>
> ```powershell
> # العدد الفعلي لكتالوج الموديل — الأمر مُختبَر 2026-11-18
> (Select-String -Path 'SalesSystem/SalesSystem.DataAccess/Configurations/*.cs' `
>    -Pattern 'ToTable\("([^"]+)"' -AllMatches |
>  Select-Object -ExpandProperty Matches |
>  ForEach-Object { $_.Groups[1].Value } |
>  Sort-Object -Unique).Count
> ```
>
> - **الناتج المتوقع: 33** — أي رقم آخر = الملف خاطئ ويُصحَّح، لا العكس.
> - لا `Migrations/` ولا `ModelSnapshot` في المستودع (المالك يدير الترحيلات) ⇒ **ممنوع** الاستشهاد بعدد جداول «من الـsnapshot».

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
- **جداول ربط**: `Entity` بلا أعمدة تدقيق (`ProductUnits` · `CustomerReceiptApplications` · `SupplierPaymentApplications`).
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

- كل مفاتيح الربط `Restrict` — **ولا مرجع ذاتي (self-FK) في أي جدول من جداول V1**.
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

**الأعمدة التي أُزيلت نهائياً (بقرار المالك أعلاه):** `CreatedByUserId` · `UpdatedByUserId` في كل جدول كان يحملها · `UserId` في `AuditLogs`/`SystemLogs` — **والقياس اليوم: صفر عمود معرّف مستخدم رقمي في المستودع**، واللقطة تحمل الأسماء النصية وحدها.

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

# 3) جرد الجداول (33 جدولاً)

| # | الوحدة | العدد | الجداول |
|---|---|---|---|
| 1 | النواة والأمان | 3 | `Customers` · `Suppliers` · `Users` |
| 2 | التنظيم والإعدادات | 4 | `Warehouses` · `CompanySettings` · `SystemSettings` · `DocumentSequences` |
| 3 | المنتجات | 4 | `ProductCategories` · `Products` · `Units` · `ProductUnits` |
| 4 | الصندوق والمصروفات (**تشغيلي**) | 2 | `CashBoxes` · `Expenses` |
| 5 | المخزون | 6 | `WarehouseStocks` · `InventoryBatches` · `InventoryTransactions` · `InventoryTransactionLines` · `InventoryAdjustments` · `InventoryAdjustmentLines` |
| 6 | المبيعات | 6 | `SalesInvoices` · `SalesInvoiceLines` · `SalesReturns` · `SalesReturnLines` · `CustomerReceipts` · `CustomerReceiptApplications` |
| 7 | المشتريات | 6 | `PurchaseInvoices` · `PurchaseInvoiceLines` · `PurchaseReturns` · `PurchaseReturnLines` · `SupplierPayments` · `SupplierPaymentApplications` |
| 8 | البنية التحتية | 2 | `AuditLogs` · `SystemLogs` |
| | **الإجمالي** | **33** | |

> **العدّ:** 3 + 4 + 4 + 2 + 6 + 6 + 6 + 2 = **33** — مطابق للقياس على `Configurations/` (2026-11-20: **33** `ToTable` فريدة، صفر جدول زائد، وصفر ذكر لـ`ProductPrices`).

> **`CashBoxes` و`Expenses` تشغيليان:** الكاشير يسجّل في أي صندوق باع، والمصروف يُسجَّل بمبلغه وتاريخه وبيانه — ولا يحتاج أيٌّ منهما آلية قيد. ولهذا بقيا في V1.
> **⚠️ القاعدة الحيّة قد تحمل بقايا هيكل ملغى** (أعمدة/جداول لم تُسقَط بعد)، لأن الإسقاط لم يُنفَّذ. الخطر العملي: الإدراج في جدول كان له عمود `NOT NULL` ملغى يفشل حتى يُسقَط العمود أو تُعاد إنشاء القاعدة. **الإسقاط مسؤولية المالك وحده** بموافقة `DROP` مكتوبة (`RULE-N198`) — وسجل ما أُزيل من **الكود** في `docs/TASKS/TASK-011-schema-v3-accounting-deferred.md` §13.

---

# 4) تعريفات الجداول (Module-by-Module)

> **الأعمدة أدناه مطابقة لما تُعلنه التهيئات والكيانات في المستودع** (النوع = نوع SQL الفعلي، والـNullable مقيس من خصائص الـModel).
> **هوية المنفّذ نصية** (`CreatedByUserName` · `UpdatedByUserName` · `UserName`) بقرار المالك 2026-11-17 (§2) — ولا عمود معرّف مستخدم رقمي في أي جدول.
> `*Id` بعد كل عمود `Id` يشير إلى مفتاح خارجي بـ`Restrict`؛ القيود المفصّلة في `SalesSystem.DataAccess/Configurations/`.

## Module 1: النواة والأمان

### 1.1 Customers
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
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
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 1.2 Suppliers
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
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
**المفتاح:** `tinyint IDENTITY` · **الفئة:** `AuditableEntity` · **الأعمدة:** 8

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `tinyint` | NOT NULL |
| `Address` | `nvarchar(300)` | NULL |
| `CompanyName` | `nvarchar(200)` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `Email` | `nvarchar(100)` | NULL |
| `LogoPath` | `nvarchar(500)` | NULL |
| `Phone` | `nvarchar(30)` | NULL |
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

> **`ProductUnits` هي البيت الوحيد للسعر (قرار المالك 2026-11-18 — `TASK-021`):** `SalePrice` (تجزئة) · `WholesalePrice` (جملة) · `CostPrice` (تكلفة مرجعية) · `MaxDiscountAmount` · `Barcode` — كلها على الوحدة. **جدول `ProductPrices` أُزيل من الكود ومن هذا المخطط** (كان يضيف نطاق سعر بالتاريخ بلا مستهلك)، ويبقى على القاعدة الحيّة حتى يُسقطه المالك (`RULE-N198`).

## Module 4: الصندوق والمصروفات (تشغيلي)

> **وحدة تشغيلية فقط:** الصندوق نقطة تحصيل، والمصروف سجل مبلغ وتاريخ وبيان — ولا شيء غير ذلك في V1.

### 4.1 CashBoxes
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** 8

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(300)` | NULL |
| `IsActive` | `bit` | NOT NULL |
| `Name` | `nvarchar(150)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |

### 4.2 Expenses
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 13

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `Amount` | `decimal(18,2)` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
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
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 23

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
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 19

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
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 25

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
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 19

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

---

# 6) المعادلات المالية الأساسية

## فاتورة البيع
```
LineTotal       = (Quantity × UnitPrice) - DiscountAmount
SubTotal        = SUM(LineTotal)
NetTotal        = SubTotal - DiscountAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint:     PaidAmount <= NetTotal
```

## فاتورة الشراء
```
LineTotal       = (Quantity × UnitPrice) - DiscountAmount
SubTotal        = SUM(LineTotal)
NetTotal        = SubTotal - DiscountAmount + OtherCharges
RemainingAmount = NetTotal - PaidAmount
Constraint:     PaidAmount <= NetTotal
```

## تكلفة FIFO (الطريقة الافتراضية)
```
عند البيع          : تُستهلك أقدم دفعة أولاً (أقدم QuantityRemaining > 0)
عند مرتجع الشراء   : يُخصم من دفعة الفاتورة الأصلية
عند FEFO          : تُستهلك أقرب تاريخ انتهاء أولاً (عند TrackExpiry = true)
```

> **كل المعادلات أعلاه تشغيلية بحتة:** تُحسب من سطور المستند ومن `InventoryBatches` والرصيد الحيّ في `WarehouseStocks` — ولا تعتمد على أي جدول خارج كتالوج §3.

---

# 7) ما أُزيل نهائياً من الإصدار الأول (ليس مؤجلاً إلى V2)

| العنصر | ما أُزيل | الدليل |
|---|---|---|
| **الفروع (Branches)** | الكيان + التكوين + الخدمات + الشاشات + **الجدول** + **19 عمود `BranchId`** في 19 جدولاً | ترحيل `20261117054708_RemoveBranchAndBranchId`: 19 `DropForeignKey` + 19 `DropIndex` + 19 `DropColumn` + 1 `DropTable` |
| **الموظفون (Employees)** | الشاشة + العقد + الـMock + الـDTO + `CreateEmployeeAccountAsync` + **`Users.EmployeeId`** | ترحيل `20261117063917_RemoveBaseCurrencyAndEmployeeColumns` (عمود واحد ضمنه) — لم يكن هناك جدول `Employees` قط |
| **العملات (Currencies)** | شاشة العملة + عنصر القائمة + `CurrencyId` + `ExchangeRate` + **13 عموداً** (`BaseNetTotal` ×7 · `CostInBaseCurrency` ×6) | نفس الترحيل: 14 `DropColumn` إجمالاً (13 عملة + `EmployeeId`) بصفر عملية أخرى |
| **الضريبة (Tax)** — 2026-11-18 (`TASK-019`) | **7 أعمدة/خصائص** أُزيلت من الكود والمخطط: `SalesInvoices.TaxAmount` · `PurchaseInvoices.TaxAmount` · `SalesReturns.ReturnedTaxAmount` · `PurchaseReturns.ReturnedTaxAmount` · `Customers.TaxNumber` · `Suppliers.TaxNumber` · `CompanySettings.TaxNumber` + تقرير الضريبة كاملاً + 7 مفاتيح `SystemSettings` + سطح الطباعة | إزالة كود كاملة ببناء **0 أخطاء**؛ **إسقاط الأعمدة على القاعدة الحيّة لم يُنفَّذ** — يبقى للمالك (`RULE-N198`)، وكل عمود محذوف كان `NULL` أو `HasDefaultValue(0m)` ⇒ الإدراج على القاعدة القائمة سليم |
| **أسعار الأصناف (ProductPrices)** — 2026-11-18 (`TASK-021`) | **جدول كامل (9 أعمدة)** أُزيل من الكود والمخطط: الكيان + التهيئة + الخدمة + `IProductPriceService` + `ProductPriceDto`/`ProductPriceRequests`/`EffectivePriceDto` + `DbSet` + المستودع + تسجيل DI + أعضاء Desktop + الكتابة المزدوجة في `frmCRUDCProductMaster`/`frmImportProductsExcel`. مصدر السعر = `ProductUnits` (`SalePrice` · `WholesalePrice` · `CostPrice` · `MaxDiscountAmount`) | إزالة كود كاملة ببناء **0 أخطاء**؛ `ToTable` بعدها = **33** بلا `ProductPrices`. **إسقاط الجدول على القاعدة الحيّة لم يُنفَّذ** — يبقى للمالك (`RULE-N198`). **تحذير بيانات:** إن كانت `ProductUnits.SalePrice = 0` وسعرها الحقيقي في `ProductPrices` فسعرها يظهر 0 بعد اليوم ⇒ يُنفَّذ فحص §7.1 قبل الإسقاط |

> **مهم:** «مؤجل إلى V2» في هذا المستودع يعني **لا كود**. الفروع والموظفون اختفوا من الكود **ومن قاعدة البيانات**؛ العملات اختفت من الكود وحُذفت أعمدتها. إعادة أي منها = ميزة جديدة تمرّ بالدورة الكاملة (Mock ⇒ مراجعة المالك ⇒ موافقة ⇒ باك-إند ⇒ ترحيل بموافقة هيكل).
>
> **ومثله ما أُزيل من الكود ولم يُسقَط من القاعدة الحيّة:** سجل الإزالة التاريخي (بأمر المالك ونصّه) محفوظ في `docs/TASKS/TASK-011-schema-v3-accounting-deferred.md` §13 — **ولا يُنسخ هنا** (`RULE-N174`).

## 7.1 فحص إلزامي قبل إسقاط `ProductPrices` من القاعدة الحيّة (2026-11-18)

قد تكون الأسعار الحقيقية في `ProductPrices` بينما `ProductUnits.SalePrice = 0` (بيانات أقدم من الكتابة المزدوجة التي أُزيلت). **يُشغَّل هذا الفحص قبل أي `DROP`:**

```sql
SELECT COUNT(*) AS OnlyProductPricesHasThePrice
FROM ProductPrices pp
JOIN ProductUnits pu ON pu.Id = pp.ProductUnitId
WHERE pu.SalePrice = 0 AND pp.Price > 0;
```

- النتيجة `0` ⇒ لا حاجة إلى تعبئة رجعية؛ يُسقط الجدول بموافقة مكتوبة.
- النتيجة `> 0` ⇒ **تعبئة رجعية أولاً** على `ProductUnits` فقط (لا يُعاد كتابة أي قيمة قائمة)، ثم الإسقاط:

```sql
UPDATE pu SET pu.SalePrice = x.Price
FROM ProductUnits pu
JOIN (SELECT ProductUnitId, MAX(Price) AS Price FROM ProductPrices GROUP BY ProductUnitId) x
  ON x.ProductUnitId = pu.Id
WHERE pu.SalePrice = 0;
```

---

# 8) المؤجل إلى الإصدار الثاني (V2)

كل جدول مؤجل — تصميمه وسببه — في ملف واحد: **`docs/database-schema-v2-deferred.md`**.

| المجموعة | الجداول المؤجلة |
|---|---|
| الأمان والأدوار | `Roles` · `UserSessions` |
| الإعدادات | `Taxes` · `FiscalYears` · `Notifications` · `Attachments` |
| المخزون | `InventoryCounts` · `InventoryCountLines` · `WarehouseTransfers` · `WarehouseTransferLines` |
| العملات | `Currencies` · `CurrencyRates` |
| البيانات التجارية | `Departments` · `CustomerContacts` · `SupplierContacts` |
| عروض الأسعار | `SalesQuotations` · `SalesQuotationItems` |
> **العدد مرجعي في ملف V2 وحده** (`RULE-N174`: رقم واحد في مكان واحد) — ولا يُنسخ هنا.

## 8.1 ما أُزيل من الكود ولم يُسقَط من القاعدة الحيّة

> **السجل التفصيلي (أمر المالك بنصّه · جرد الملفات · الأعداد · الموافقات) في `docs/TASKS/TASK-011-schema-v3-accounting-deferred.md` §13 — ولا يُنسخ هنا** (`RULE-N174`). الخلاصة **المقيسة 2026-11-20** (`RULE-N195`/`RULE-N196`): **صفر كيان أو تهيئة أو خدمة أو عقد أو شاشة** لذلك الهيكل في المستودع · **صفر** لـ`AccountId|ExpenseAccount|JournalEntry|SystemAccountMapping|NormalBalance|AllowCostCenter` في `SalesSystem/**` · كتالوج الموديل **33** جدولاً · صفر ملف في `Seeders/` · **صفر ملف في `Migrations/`**.
>
> **سجل الأثر اللفظي المتبقي (ليس كوداً ولا هيكلاً):** `docs/TASKS/TASK-024-accounting-code-final-registry.md` — جرد مقيس لـ**119 سطراً في 38 ملفاً**، منها **6 أسطر في 5 ملفات** بقايا حقيقية تُزال، و**كل الباقي** مبرَّر (`K1` كشف حساب الطرف · `K2` حساب المستخدم · `K3` حساب=عملية حسابية · `K4` توقيع على ورقة · `K5` توثيق صحيح).
>
> **⚠️ الخطر المتبقي — على القاعدة الحيّة وحدها:** أعمدة `NOT NULL` قديمة تشير إلى جداول لم تعد في الموديل قد تمنع `INSERT` على `Customers` / `Suppliers` / `CashBoxes` / `Expenses` حتى تُسقَط أو تُعاد إنشاء القاعدة. **الإسقاط مسؤولية المالك وحده** بموافقة `DROP` مكتوبة ومؤرَّخة مستقلة ونسخة احتياطية قبلها (`RULE-N198` · §2.42 في `AGENTS.md`). **لا وكيل ينفّذ `DROP` ولا `dotnet ef database update`.**

---

# 9) انحرافات مقيسة بين التصميم والتنفيذ (Known Deviations)

هذه انحرافات **حقيقية ومقصودة**، مسجَّلة بوصفها كذلك لا كأخطاء — كلها تخص جداول V1 في §3:

| # | العنصر | الملاحظة المقيسة |
|---|---|---|
| 1 | `InventoryTransactions` | لا يوجد `TransactionDate` ولا `Status`؛ الرقم `TransactionNo` نصي `nvarchar(50)`، والحركة يُشار إليها بـ`MovementType` + `ReferenceType`/`ReferenceId` |
| 2 | `InventoryTransactionLines` | لا يوجد `ProductId`/`BatchId`/`TotalCost` — السطر يحمل `ProductUnitId` والدفعة **نصياً** بـ`BatchNo` + `ExpiryDate` |
| 3 | `InventoryAdjustments` | الرقم `AdjustmentNo` نصي `nvarchar(50)` لا `int`، ولا يوجد `AdjustmentDate` |
| 4 | `InventoryBatches.BatchNo` | نصي `nvarchar(50)` (لا `int`) |
| 5 | أعمدة التدقيق | ✅ **منفَّذ — نصّية الآن** (`CreatedByUserName` · `UpdatedByUserName` `nvarchar(100)`)؛ قياس 2026-11-18: **صفر** عمود معرّف مستخدم رقمي في `Models/**` و`Configurations/**` — والتعبئة الرجعية على القاعدة الحيّة مسؤولية المالك |
| 6 | `Products.Barcode` | عمود `varchar` للبحث السريع؛ المصدر الرسمي للباركود هو `ProductUnits.Barcode` |
| 7 | `ProductUnits.Barcode` | `nvarchar(50)` في القاعدة (يوافق `varchar(50)` عملياً لكونه ASCII) |

---

# 10) قائمة تحقق يدوي للتحقق من هذا الملف

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | `Select-String -Path 'SalesSystem/SalesSystem.DataAccess/Configurations/*.cs' -Pattern 'ToTable\("'` وعدّ الأسماء الفريدة | **33** بالضبط — أي رقم آخر = خطأ في الملف |
| 2 | ابحث في الملف عن `V2 DEFERRED` أو `Branches` أو `CurrencyId` أو `BaseNetTotal` | **صفر** نتيجة |
| 3 | افتح `docs/database-schema-v2-deferred.md` | مرجع تصميم ما هو خارج V1 — ولا يُستشهد بعدده من هذا الملف (`RULE-N174`) |
| 4 | `Get-ChildItem SalesSystem\SalesSystem.DataAccess\Migrations -File -Recurse` | **0 ملف** و**0 مسار في git** — المجلد قد يوجد فارغاً على القرص المحلي (git لا يتتبّع المجلدات الفارغة)، والمالك يولّد `InitialCreate` بنفسه |
| 5 | تحقق أن §3 يجمع 33: 3 + 4 + 4 + 2 + 6 + 6 + 6 + 2 | **33** |
| 6 | ابحث في §4 عن `### 4.x` تحت `## Module 4` | **جدولان فقط**: `4.1 CashBoxes` · `4.2 Expenses` |
| 7 | `Select-String -Path 'docs/database-schema.md' -Pattern 'Accounts\b|JournalEntry|JournalEntryLines|SystemAccountMappings|ReceiptVouchers|PaymentVouchers'` | **صفر** — لا تعريف ولا إشارة إلى جدول أو وحدة خارج V1. (كلمة «محاسب» وحدها ليست فحصاً: تظهر في أسماء أدوار بشرية وفي توقيع ورقة جرد — تصنيفها الكامل في `TASK-024` §2) |
| 8 | ابحث في الملف عن `Debit`/`Credit` | **صفر** في المعادلات — المعادلات تشغيلية بحتة (§6) |

---

# 11) سجل تغييرات الهيكل (Schema Change Log)

| # | التاريخ | التغيير | النوع | نصّ الموافقة | الحالة |
|---|---------|---------|-------|--------------|--------|
| 1 | 2026-11-17 | **عزل وحدة `Users`:** لا مفتاح أجنبي إلى `Users` من أي جدول | تصميم | «اريد ان افصل جدول المستخدمين عن بقيت الجدول الاخرى لا ان يرتبط بالمعرف لكي نجزء النظام إلى وحدات صغير منفصلة واسجل اسم المستخدم الحالي من على الوجهات كنص» | ✅ موثَّق |
| 2 | 2026-11-17 | **استبدال أعمدة التدقيق الرقمية بأسماء نصية** `nvarchar(100)`: إضافة `CreatedByUserName` · `UpdatedByUserName` · `ReviewedByUserName` · `ClosedByUserName` · `CashierUserName` · `UserName`، وإزالة `CreatedByUserId` · `UpdatedByUserId` · `ReviewedByUserId` · `ClosedByUserId` · `CashierUserId` · `UserId` | إضافة ثم **إزالة** (بموافقة مالك صريحة) | «remove CreatedByUserId and UpdateByUserId and repllase by CreatedByUserName and UpdateByUserName for all system» | ✅ **منفَّذ في الكود** — صفر عمود معرّف مستخدم رقمي (قياس 2026-11-18) · والتعبئة الرجعية على القاعدة الحيّة مسؤولية المالك |
| 3 | 2026-11-17 | **تضييق نطاق V1 إلى «بيع → خصم مخزون → صندوق»:** ما ليس تشغيلياً خرج من الإصدار الأول (تصميمه في ملف V2) ⇒ **V1 = 34 جدولاً** (ثم صار **33** بعد إزالة `ProductPrices` — الصف 5) | **تصميم/نطاق** ثم **إزالة كود** (لا `DROP` على القاعدة الحيّة) | «and reamning Accounting tables from v1 to v2» + «remove all Accounting code» + «and remove all migrations» | ✅ **الكود مُزال (2026-11-17)** — 84 ملفاً محذوفاً · 72 معدّلاً · صفر ملف ترحيل. ⏸ **إسقاط بقايا القاعدة الحيّة للمالك** (`RULE-N198`) — التفصيل في §8.1 و`TASK-011` §13 · **وبقايا لفظية مغلقة 2026-11-20** (`TASK-024`) |
| 4 | 2026-11-18 | **إزالة سطح الضريبة:** 7 أعمدة (`SalesInvoices.TaxAmount` · `PurchaseInvoices.TaxAmount` · `SalesReturns.ReturnedTaxAmount` · `PurchaseReturns.ReturnedTaxAmount` · `Customers.TaxNumber` · `Suppliers.TaxNumber` · `CompanySettings.TaxNumber`) + `NetTotal = SubTotal − DiscountAmount + OtherCharges` | **إزالة** (لا `DROP` على القاعدة الحيّة) | «remove each and every Tax columns form MVP v1 from database shema and project code» | ✅ **الكود مُزال** — بناء 0 Errors · صفر ترحيل · ⏸ الإسقاط للمالك (`TASK-019`) |
| 5 | 2026-11-18 | **إزالة جدول `ProductPrices` ومصدر السعر يصير `ProductUnits`** ⇒ **V1 = 33 جدولاً** | **إزالة** (لا `DROP` على القاعدة الحيّة — فحص §7.1 أولاً) | «remove each and everything about ProductPrices table code from all system because we have ProductUnits is enugh» | ✅ **الكود مُزال** — 6 ملفات محذوفة · 16 معدّلة · بناء `--no-incremental` = 0 Errors · `ToTable` = **33** · ⏸ الإسقاط للمالك (`TASK-021`) |

> **ملاحظة إلزامية على أي مراجعة:** المصدر الواقعي هو **الكيانات والتهيئات في المستودع** (`Models/**` + `Configurations/**`) — **لا يوجد snapshot**. أي مراجعة تفترض عموداً غير معلن هناك مراجعة على أساس خاطئ.

---

**الإصدار:** **MVP V1 — Pure POS & Inventory** · **التاريخ:** 2026-11-18 · **الملف المرجعي المؤجل:** `docs/database-schema-v2-deferred.md`
