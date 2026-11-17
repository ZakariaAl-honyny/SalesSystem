# Database Schema — **Version 2 (Deferred Reference)**

# Sales Management System — الجداول المؤجلة عن الإصدار الأول

**Status:** 📄 **مرجع تصميم فقط — لا كود** — باستثناء صريح واحد: **الجداول المحاسبية الستة** (قسم «المحاسبة») نُقلت من V1 إلى هنا بقرار 2026-11-17، وكودها **ما زال قائماً** حتى تُنفَّذ موجة الإزالة بموافقتها المكتوبة.
**V1 الحالي (V3):** `docs/database-schema.md` — **34** جدولاً هدفاً · والمقيس في الـsnapshot اليوم **40** = 34 + الجداول المحاسبية الستة.
**المحرَّر:** 2026-11-17 (تحديث V3).

---

> ## ⚠️ اقرأ هذا قبل أي سطر
>
> **ينقسم هذا الملف إلى قسمين بحكمين مختلفين — لا تخلط بينهما:**
>
> | القسم | الحكم | العدد |
> |---|---|---|
> | الأقسام أ · ب · ج (تصميم) · د · هـ · و · ز | **مرجع تصميم فقط — لا كود**: لا كيان ولا تكوين EF ولا خدمة BLL ولا عقد Desktop ولا ترحيل ولا شاشة | **23** |
> | قسم «المحاسبة» (AC) | **نُقل من V1 إلى V2 في V3 (2026-11-17)**: التصميم هنا، و**الكود ما زال في المستودع** حتى موجة الإزالة | **6** |
>
> الدليل المقيس: `SalesDbContextModelSnapshot.cs` يحوي **40** `ToTable` = **34** هدف V3 + **6** محاسبية.
>
> ```powershell
> # (1) المؤجل تصميماً فقط — يجب أن يكون الناتج صفراً
> $designOnly = @('Roles','UserSessions','Taxes','FiscalYears','Notifications','Attachments',
>   'AccountCategories','Banks','TreasuryTransfers','CostCenters','VoucherInvoiceAllocations',
>   'FiscalPeriods','InventoryCounts','InventoryCountLines','WarehouseTransfers',
>   'WarehouseTransferLines','Currencies','CurrencyRates','Departments','CustomerContacts',
>   'SupplierContacts','SalesQuotations','SalesQuotationItems')
> # (2) المنقولة من V1 — يجب أن يكون الناتج 6 اليوم، وصفراً بعد موجة الإزالة
> $movedFromV1 = @('Accounts','JournalEntries','JournalEntryLines','ReceiptVouchers','PaymentVouchers','SystemAccountMappings')
> $snap = Select-String -Path 'SalesSystem/SalesSystem.DataAccess/Migrations/SalesDbContextModelSnapshot.cs' -Pattern 'ToTable\("([^"]+)"' -AllMatches
> $names = $snap.Matches | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique
> $designOnly  | Where-Object { $names -contains $_ }          # صفر
> ($movedFromV1 | Where-Object { $names -contains $_ }).Count   # 6 -> 0 بعد الإزالة
> ```
>
> **تنفيذ أي منها ليس «إكمال نقص» بل ميزة جديدة**، وتمرّ بالدورة الكاملة الملزمة:
> **Mock + بيانات أولية ⇒ مراجعة المالك (UI & UX) ⇒ موافقة مكتوبة ⇒ باك-إند (كيان · تكوين · BLL · Direct) ⇒ ترحيل إضافي بموافقة هيكل منفصلة** (`RULE-N165`…`RULE-N177` · `RULE-N198`).

---

## ما ليس في هذا الملف (وسببه)

| العنصر | السبب |
|---|---|
| **`Branches` + كل أعمدة `BranchId`** | **أُزيل نهائياً** بقرار المالك 2026-11-17 — ليس مؤجلاً. ترحيل `20261117054708_RemoveBranchAndBranchId` حذف الجدول و19 عموداً. أي تصميم V2 هنا كان يذكر `BranchId` فقد حُذف ذلك السطر. |
| **`Employees`** | **أُزيل نهائياً** — لم يكن جدولاً قط؛ أُزيلت الشاشة والعقد والـMock والـDTO و`Users.EmployeeId`. |
| **`Currencies` / `CurrencyRates` / أي `CurrencyId` / `BaseNetTotal` / `CostInBaseCurrency`** | أُزيلت الشاشة والكود، وحُذفت 13 عموداً بقرار المالك «Yes — drop the 13 columns too» (2026-11-17). الجدولان يبقيان هنا كتصميم V2 فقط. |
| **`Taxes` كإدارة** | ضريبة واحدة مزروعة تُستخدم في الفواتير؛ إدارة الضرائب مؤجلة. |
| `SystemSettings` · `CompanySettings` | **V1 — حيّة**: تخزين لوحة الألوان وإعدادات التنسيق وحالة التصفية والطباعة. لا تُمسّ. |

---

## اصطلاحات هذا الملف

- الأعمدة القياسية للتدقيق مكتوبة بالاصطلاح **المنفَّذ حالياً** في V1: `CreatedByUserId int null` · `UpdatedByUserId int null` · `CreatedAt datetime2 not null` · `UpdatedAt datetime2 null`.
- إن نُفِّذ قرار عزل وحدة `Users` (تحويل التدقيق إلى اسم نصي — §2 من ملف V1) **قبل** بناء أي جدول هنا، تصبح أعمدة التدقيق `nvarchar(100) null` بأسماء `CreatedByUserName`/`UpdatedByUserName`. القرار يُتخذ وقت التنفيذ لا هنا.
- كل مفاتيح الربط `DeleteBehavior.Restrict` — لا `CASCADE` في أي جدول مؤجل.
- المبالغ `decimal(18,2)` والكميات `decimal(18,3)` — بلا استثناء.

---

# المجموعة أ — الأمان والأدوار

## A.1 `Roles`

**السبب:** مستخدم واحد (Admin) بصلاحيات كاملة — لا نظام أدوار في V1 (`AGENTS.md §2.29`).

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | smallint PK | |
| `Name` | nvarchar(100) not null | فريد |
| `SalesPermissions` | bigint not null default 0 | Bitmask — `-1` = Super Admin |
| `InventoryPermissions` | bigint not null default 0 | Bitmask |
| `AccountingPermissions` | bigint not null default 0 | Bitmask |
| `SystemPermissions` | bigint not null default 0 | Bitmask |
| `IsSystem` | bit not null default 0 | أدوار محمية من الحذف |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**ملاحظة تصميم:** `UserRole` enum باقٍ في `SalesSystem.Common.Enums` **كوسم جلسة فقط** — لا يُستخدم لفحص صلاحيات، ولا يوجد `RoleId` على `Users` في V1.

---

## A.2 `UserSessions`

**السبب:** مستخدم واحد — لا تتبّع جلسات.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `UserId` | int not null FK → Users(Id) | |
| `SessionToken` | nvarchar(200) not null | |
| `DeviceName` | nvarchar(200) null | |
| `IpAddress` | nvarchar(50) null | |
| `UserAgent` | nvarchar(500) null | |
| `LastActivityAt` | datetime2 not null | |
| `ExpiresAt` | datetime2 not null | |
| `IsRevoked` | bit not null default 0 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **فهرس** | `(UserId, IsRevoked)` | للبحث عن الجلسات النشطة |

---

# المجموعة ب — الإعدادات

## B.1 `Taxes`

**السبب:** إدارة الضرائب مخفية — تُستخدم ضريبة افتراضية واحدة في الفواتير.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | smallint PK | |
| `Name` | nvarchar(100) not null | |
| `Code` | nvarchar(20) not null | مثل `VAT-15` |
| `Rate` | decimal(5,2) not null | CHECK 0–100 |
| `TaxType` | tinyint not null | 1=Standard · 2=ZeroRated · 3=Exempt |
| `IsDefault` | bit not null default 0 | فريد مفلتر `IsDefault=1 AND IsActive=1` |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**ملاحظة V1 المقيسة:** `SalesInvoices`/`PurchaseInvoices` **لا تحمل** `TaxId`؛ الضريبة تُدخل مبلغاً يدوياً (`TaxAmount`). أي كود يذكر `TaxId` اليوم فهو تعليق قديم.

---

## B.2 `FiscalYears`

**السبب:** غير مطلوب للإصدار الأول — `JournalEntries` لا يحمل `FiscalYearId` في V1.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `YearName` | nvarchar(20) not null | مثل `2026` |
| `StartDate` | date not null | |
| `EndDate` | date not null | CHECK `EndDate > StartDate` |
| `IsClosed` | bit not null default 0 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

---

## B.3 `Notifications`

**السبب:** مستخدم واحد — لا نظام إشعارات متعدد المستخدمين.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `UserId` | int not null FK → Users(Id) | |
| `Type` | tinyint not null | 1=LowStock · 2=ExpirySoon · 3=CreditLimitExceeded · 4=System · 5=Reminder |
| `Title` | nvarchar(200) not null | |
| `Message` | nvarchar(1000) not null | |
| `ReferenceType` | nvarchar(50) null | |
| `ReferenceId` | int null | |
| `IsRead` | bit not null default 0 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **فهارس** | `(UserId, IsRead, CreatedAt DESC)` | للإشعارات غير المقروءة |

**ملاحظة:** يوجد في الكود `ToastForm`/`NotificationService` بلا مستهلك — دَين تنظيف مستقل، وليس تنفيذاً لهذا الجدول.

---

## B.4 `Attachments`

**السبب:** غير أساسي للإصدار الأول — المستندات تحمل مسار مرفق نصياً (`AttachmentPath`).

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `ReferenceType` | nvarchar(50) not null | مثل `PurchaseInvoice` · `InventoryAdjustment` |
| `ReferenceId` | int not null | |
| `FileName` | nvarchar(255) not null | |
| `FilePath` | nvarchar(500) not null | |
| `FileSize` | bigint not null | |
| `ContentType` | nvarchar(100) null | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **فهرس** | `(ReferenceType, ReferenceId)` | للبحث عن مرفقات مستند |

---

# المجموعة ج — المحاسبة

## C.1 `AccountCategories`

**السبب:** تعقيد غير ضروري لمتجر واحد — `Accounts` لا يحمل `CategoryId` في V1.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | smallint PK identity | |
| `Name` | nvarchar(100) not null | فهرس فريد |
| `Description` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**مشتقّات لا تُخزَّن:** `treePath` · `financialStatement` (من `Nature`) · `nature` DEBIT/CREDIT · تصنيف خاص (عميل/مورد/صندوق/بنك) — كلها محسوبة لا أعمدة.

---

## C.2 `Banks`

**السبب:** صندوق نقدي واحد يغطي احتياجات V1.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `AccountId` | int not null FK → Accounts(Id) | |
| `Name` | nvarchar(150) not null | |
| `AccountNumber` | nvarchar(100) null | |
| `IBAN` | nvarchar(100) null | |
| `ContactPerson` | nvarchar(100) null | |
| `ContactPhone` | nvarchar(50) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**إضافات محتملة عند التنفيذ:** `Type` (1=Bank · 2=Wallet · 3=POS Network) · `Currency` · `OverdraftLimit` · `LastReconciliationDate`.

---

## C.3 `TreasuryTransfers`

**السبب:** صندوق واحد — لا تحويلات بين الصناديق.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `TransferNo` | int not null | فريد — تسلسل صحيح (3001, 3002 …) |
| `TransferDate` | date not null | |
| `FromCashBoxId` | int not null FK → CashBoxes(Id) | |
| `ToCashBoxId` | int not null FK → CashBoxes(Id) | |
| `Amount` | decimal(18,2) not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft · 2=Posted · 3=Cancelled |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | |
| `CancelledAt` | datetime2 null | |

**إضافات محتملة:** `FromBankId` / `ToBankId` (تحويل صندوق↔بنك) · `CostCenterId`.

---

## C.4 `CostCenters`

**السبب:** لم تكن في EF قط — تحليلٌ مخطَّط لا أكثر.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `Code` | nvarchar(20) not null | فريد |
| `Name` | nvarchar(200) not null | |
| `NameEn` | nvarchar(200) null | |
| `ParentId` | int null FK → CostCenters(Id) | تسلسل ذاتي |
| `Level` | tinyint not null default 1 | CHECK 1–10 |
| `IsLeaf` | bit not null default 1 | |
| `Budget` | decimal(18,2) not null default 0 | الموازنة التقديرية |
| `ManagerName` | nvarchar(100) null | |
| `IsActive` | bit not null default 1 | |
| `Notes` | nvarchar(300) null | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**المستهلكون عند التبنّي:** `JournalEntryLines.CostCenterId` · `ReceiptVouchers`/`PaymentVouchers.CostCenterId` · `TreasuryTransfers.CostCenterId`.

---

## C.5 `VoucherInvoiceAllocations`

**السبب:** توزيع سند قبض/صرف على عدة فواتير — موجود في V1 لسندات العملاء والموردين فقط، لا لسندات المحاسبة.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `VoucherType` | tinyint not null | 1=ReceiptVoucher · 2=PaymentVoucher |
| `VoucherId` | int not null | مفتاح حسب `VoucherType` |
| `InvoiceType` | tinyint not null default 1 | 1=SalesInvoice · 2=PurchaseInvoice |
| `InvoiceId` | int not null | مفتاح حسب `InvoiceType` |
| `AllocatedAmount` | decimal(18,2) not null | > 0 و ≤ المتبقي من السند |
| **قيد فريد** | `UNIQUE(VoucherType, VoucherId, InvoiceType, InvoiceId)` | صف توزيع واحد لكل فاتورة لكل سند |
| **فهرس** | `(InvoiceType, InvoiceId)` | لكشوف الحساب والأعمار |

---

## C.6 `FiscalPeriods`

**السبب:** V1 يتتبّع سنوات مالية كاملة فقط (وهي نفسها مؤجلة) — الفترات الشهرية/الربعية غير مخزَّنة.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `FiscalYearId` | smallint not null FK → FiscalYears(Id) | |
| `PeriodName` | nvarchar(100) not null | مثل «يناير 2026» · «الربع الأول 2026» |
| `PeriodType` | tinyint not null | 1=Month · 2=Quarter · 3=Year |
| `StartDate` | date not null | |
| `EndDate` | date not null | |
| `IsClosed` | bit not null default 0 | الفترة المغلقة تمنع ترحيل القيود في مداها |
| `ClosedAt` | datetime2 null | |
| `ClosedByUserId` | int null | |
| `ClosingNotes` | nvarchar(500) null | |
| `ClosingEntryId` | int null FK → JournalEntries(Id) | قيد إقفال آلي |
| **فهرس** | `(FiscalYearId, StartDate, EndDate)` | |

---

# المجموعة د — المخزون

## D.1 `InventoryCounts`

**السبب:** الجرد اليدوي كافٍ — تُستخدم `InventoryAdjustments` للأرصدة الافتتاحية والتصحيحات.

> **ملاحظة تنفيذية مقيسة:** شاشة `frmStocktakingNew` موجودة وتعمل بـMock فقط، **بلا كيان ولا جدول ولا خدمة Direct** — وعنصر قائمتها أُزيل من `frmMain` بقرار المالك «Remove them from the menu now (keep files)» (2026-11-17).

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `CountNo` | int not null | فريد |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `CountDate` | date not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft · 2=Posted · 3=Cancelled |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | |
| `CancelledAt` | datetime2 null | |

---

## D.2 `InventoryCountLines`

**السبب:** مؤجل مع `InventoryCounts`.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `InventoryCountId` | int not null FK → InventoryCounts(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `BatchId` | int not null FK → InventoryBatches(Id) | ربط الجرد بدفعة محددة |
| `SystemQuantity` | decimal(18,3) not null | الكمية المتوقعة |
| `ActualQuantity` | decimal(18,3) not null | الكمية المعدودة |
| `DifferenceQuantity` | decimal(18,3) not null | الفرق |

---

## D.3 `WarehouseTransfers`

**السبب:** مستودع واحد — لا تحويلات بين المستودعات.

> **ملاحظة تنفيذية مقيسة:** شاشة `frmWarehouseTransferNew` + 4 نوافذ طباعة موجودة وتعمل بـMock فقط، **بلا كيان ولا جدول ولا خدمة Direct** — وعنصر قائمتها أُزيل من `frmMain` بنفس قرار المالك أعلاه (الملفات باقية).

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `TransferNo` | int not null | فريد |
| `FromWarehouseId` | smallint not null FK → Warehouses(Id) | |
| `ToWarehouseId` | smallint not null FK → Warehouses(Id) | |
| `TransferDate` | date not null | |
| `Notes` | nvarchar(500) null | |
| `Status` | tinyint not null | 1=Draft · 2=Posted · 3=Cancelled |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| `PostedAt` | datetime2 null | |
| `CancelledAt` | datetime2 null | |

---

## D.4 `WarehouseTransferLines`

**السبب:** مؤجل مع `WarehouseTransfers`.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `WarehouseTransferId` | int not null FK → WarehouseTransfers(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `BatchId` | int not null FK → InventoryBatches(Id) | الدفعة المحوَّلة |
| `Quantity` | decimal(18,3) not null | |
| `UnitCost` | decimal(18,2) not null | |
| `TotalCost` | decimal(18,2) not null | `UnitCost × Quantity` |

**ملاحظات تصميم المخزون:** لا يوجد جدول `InventoryMovements` (بديله `InventoryTransactions`/`InventoryTransactionLines`) · لا `StockTransfers` (الاسم `WarehouseTransfers`) · لا `StockWriteOffs` (تُغطّى بـ`InventoryAdjustments` نوع Damage) · `WarehouseStocks` بلا `IsActive` · التحويلات **لا** تُنشئ قيوداً محاسبية (التسويات فقط).

---

# المجموعة هـ — العملات

## E.1 `Currencies`

**السبب:** نظام أحادي العملة — لا تحويل ولا إدارة عملات. **أُزيلت الشاشة والكود والأعمدة فعلاً** (2026-11-17)؛ هذا تصميم V2 فقط.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | smallint PK | |
| `Code` | char(3) not null | ISO 4217 (`SAR` · `USD` · `YER`) |
| `NameAr` | nvarchar(100) not null | |
| `NameEn` | nvarchar(100) not null | |
| `Symbol` | nvarchar(10) null | |
| `FractionName` | nvarchar(50) null | مثل «فلس» |
| `DecimalPlaces` | tinyint not null default 2 | |
| `IsBaseCurrency` | bit not null default 0 | فريد مفلتر `IsBaseCurrency=1 AND IsActive=1` |
| `IsSystem` | bit not null default 0 | عملات محمية |
| `ExchangeRate` | decimal(18,6) not null default 1 | سعر التحويل إلى الأساسية |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

**ما يجب أن يعود معها (مؤجل معها):** `SalesInvoices.CurrencyId` + `ExchangeRate` · `PurchaseInvoices.CurrencyId` + `ExchangeRate` · `CustomerReceipts.CurrencyId` · `SupplierPayments.CurrencyId` · `ProductPrices.CurrencyId` · وأعمدة `BaseNetTotal` (7) و`CostInBaseCurrency` (6) التي حُذفت من V1 — عودتها = ترحيل إضافي بموافقة هيكل **جديدة**.

---

## E.2 `CurrencyRates`

**السبب:** سجل أسعار الصرف اليومي — بلا عملات لا معنى له.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `CurrencyId` | smallint not null FK → Currencies(Id) | |
| `RateToBase` | decimal(18,6) not null | سعر الصرف في التاريخ |
| `EffectiveDate` | date not null | |
| `CreatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |

---

# المجموعة و — البيانات التجارية

## W.1 `Departments`

**السبب:** الهيكل التنظيمي غير مطلوب في V1.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | smallint PK | |
| `Name` | nvarchar(150) not null | |
| `Description` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

---

## W.2 `CustomerContacts`

**السبب:** V1 يحمل هاتفاً وبريداً واحداً على `Customers`.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `CustomerId` | int not null FK → Customers(Id) | |
| `Name` | nvarchar(150) not null | |
| `Phone` | nvarchar(30) null | |
| `Email` | nvarchar(100) null | |
| `Position` | nvarchar(100) null | المسمى الوظيفي عند العميل |
| `Notes` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

---

## W.3 `SupplierContacts`

**السبب:** V1 يحمل هاتفاً وبريداً واحداً على `Suppliers`.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `SupplierId` | int not null FK → Suppliers(Id) | |
| `Name` | nvarchar(150) not null | |
| `Phone` | nvarchar(30) null | |
| `Email` | nvarchar(100) null | |
| `Position` | nvarchar(100) null | |
| `Notes` | nvarchar(300) null | |
| `IsActive` | bit not null default 1 | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |

---

# المجموعة ز — عروض الأسعار

## Z.1 `SalesQuotations`

**السبب:** عروض الأسعار مؤجلة — دورة عرض ⇒ فاتورة غير مطلوبة في V1.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `QuotationNo` | int not null | فريد |
| `QuotationDate` | date not null | |
| `ValidUntil` | date null | تاريخ انتهاء صلاحية العرض |
| `CustomerId` | int not null FK → Customers(Id) | |
| `WarehouseId` | smallint not null FK → Warehouses(Id) | |
| `PaymentType` | tinyint not null | 1=Cash · 2=Credit |
| `SubTotal` | decimal(18,2) not null | `SUM(LineTotal)` |
| `DiscountAmount` | decimal(18,2) not null | خصم على مستوى العرض |
| `TaxAmount` | decimal(18,2) not null | |
| `TotalAmount` | decimal(18,2) not null | `SubTotal - Discount + Tax` |
| `Notes` | nvarchar(500) null | |
| `TermsAndConditions` | nvarchar(2000) null | |
| `Status` | tinyint not null | 1=Draft · 2=Sent · 3=Accepted · 4=Converted · 5=Rejected |
| `ConvertedToInvoiceId` | int null FK → SalesInvoices(Id) | يُضبط عند التحويل |
| `RejectionReason` | nvarchar(1000) null | |
| `CreatedByUserId` | int null | |
| `UpdatedByUserId` | int null | |
| `CreatedAt` | datetime2 not null | |
| `UpdatedAt` | datetime2 null | |
| **قيد فريد** | `UNIQUE(QuotationNo)` | |

---

## Z.2 `SalesQuotationItems`

**السبب:** مؤجل مع `SalesQuotations`.

| العمود | النوع | ملاحظة |
|---|---|---|
| `Id` | int PK | |
| `SalesQuotationId` | int not null FK → SalesQuotations(Id) | |
| `ProductId` | int not null FK → Products(Id) | |
| `ProductUnitId` | int not null FK → ProductUnits(Id) | |
| `Quantity` | decimal(18,3) not null | |
| `UnitPrice` | decimal(18,2) not null | |
| `DiscountAmount` | decimal(18,2) not null default 0 | خصم على مستوى السطر |
| `LineTotal` | decimal(18,2) not null | `(Quantity × UnitPrice) - DiscountAmount` |
| `Notes` | nvarchar(500) null | |

**ملاحظات تصميم:** لا أثر مخزني ولا محاسبي قبل التحويل إلى فاتورة بيع · السطور تدعم خصماً لكل سطر · دورة خمس حالات: Draft → Sent → Accepted → Converted / Rejected.

---

# المجموعة AC — المحاسبة (نُقلت من V1 إلى V2 في الإصدار الثالث — 2026-11-17)

> **قرار المالك (2026-11-17) — نصّه:** «remove these to v2: … شجرة الحسابات (`Accounts`) · قيود اليومية (`JournalEntries` و `JournalEntryLines`) · سندات القبض والصرف اليدوية (`ReceiptVouchers`, `PaymentVouchers`) · حسابات البنوك ومراكز التكلفة وأرباح وخسائر الميزانية · جداول الربط المحاسبي (`SystemAccountMappings`)».
>
> **المسوّغ:** النظام صار «مبيعات ومخزون صرف» (`Pure POS & Inventory`): بيع → خصم مخزون → تسجيل في الصندوق → رصيد العميل من فواتيره. **لا مدين ولا دائن في أي شاشة من شاشات V1.**
>
> **الأقسام أدناه (AC.1 · AC.3 · AC.4 · AC.5 · AC.6 · AC.8) هي الجداول الستة المنقولة بتعريفاتها الكاملة كما كانت في V1 — لم يُحذف سطر.** الترقيم يحتفظ بفجوات (AC.2 · AC.7) لأن `CashBoxes` و`Expenses` **بقيا في V1** لأنهما تشغيليان لا محاسبيان.
>
> **⛔ حالة التنفيذ (2026-11-17):** **الكود مُزال من المستودع بالكامل** بأمر المالك «remove all Accounting code» (`TASK-011` §13): لا كيانات ولا خدمات ولا شاشات ولا تكوينات ولا بذور ولا DTOs، و**كل ملفات الترحيل أُزيلت** بأمره «and remove all migrations». **التعريفات أدناه تبقى مرجع V2 الوحيد** لإعادة بناء الوحدة لاحقاً. **إسقاط الجداول على القاعدة الحيّة لم يُنفَّذ** وهو ملك المالك (`RULE-N198`).
>
> **البنوك ومراكز التكلفة وأرباح وخسائر الميزانية** لا تحتاج نقلاً: هي أصلاً مؤجلة هنا — `Banks` (C.2) · `CostCenters` (C.4) · `TreasuryTransfers` (C.3) · `AccountCategories` (C.1) · `FiscalPeriods` (C.6) · `VoucherInvoiceAllocations` (C.5). أي تقرير أرباح وخسائر أو ميزانية عمومية يقرأ `JournalEntries`/`Accounts` ⇒ **يُحذف معهما** ولا يُبنى في V1.
>
> **حالة الكود اليوم (مقيسة 2026-11-17):** الكيانات والخدمات والشاشات والبذور **قائمة**. الإزالة موجة منفصلة تحتاج **موافقة `DROP` مكتوبة** وجدول أثر كامل في `docs/database-schema.md` §8.1.

## AC.1 Accounts — دليل الحسابات الهرمي
**المفتاح:** `int IDENTITY` · **الفئة:** `ActivatableEntity` · **الأعمدة:** **19 مقيسة ومنفَّذة** (كانت 17 قبل موجة 2026-11-17)

> **حالة هذا الجدول (مقيسة 2026-11-17 من `SalesDbContextModelSnapshot.cs`، كتلة الكيان: 19 خاصية + `ToTable`):** الأعمدة **19** — العمودان `AllowCostCenter` و`NormalBalance` **منفَّذان في الكيان والتهيئة واللقطة الأساسية**. وأسماء التدقيق **نصّية الآن** (`CreatedByUserName` · `UpdatedByUserName` `nvarchar(100)`) بعد موجة الهوية النصية: عدد `CreatedByUserId` في اللقطة = **0** و`CreatedByUserName` = **27**.
>
> **الترحيل ملك المالك (قراره 2026-11-17: «I will do migrations»):** العمودان موجودان داخل `20261117085853_InitialCreate` (السطر 32: `AllowCostCenter` · السطر 90 في اللقطة: `NormalBalance`) — والمالك يدير إعادة تأسيس الترحيلات بنفسه. لا يُنشئ أي وكيل ترحيلاً ولا يحذف/يُعيد تسمية ترحيل قائم.

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountCode` | `nvarchar(20)` | NOT NULL — فريد مفلتر `WHERE [IsActive] = 1` |
| `AllowCostCenter` | `bit` | NOT NULL — افتراضي `0` · ✅ منفَّذ |
| `ColorCode` | `nvarchar(7)` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL — ✅ منفَّذ (الحقل الرقمي أُزيل: صفر `CreatedByUserId` في اللقطة) |
| `Description` | `nvarchar(500)` | NULL |
| `IsActive` | `bit` | NOT NULL — افتراضي `1` |
| `IsLeaf` | `bit` | NOT NULL — افتراضي `1` |
| `IsSystem` | `bit` | NOT NULL — افتراضي `0` |
| `Level` | `tinyint` | NOT NULL — افتراضي `1` + `CHECK [Level] BETWEEN 1 AND 10` |
| `NameAr` | `nvarchar(200)` | NOT NULL |
| `NameEn` | `nvarchar(200)` | **NULL** |
| `Nature` | `tinyint` | NOT NULL — 1=أصول · 2=خصوم · 3=حقوق ملكية · 4=إيرادات · 5=مصروفات |
| `NormalBalance` | `tinyint` | NULL — 1=مدين · 2=دائن · `NULL` = يُشتق من `Nature` · ✅ منفَّذ (و`0` في الـDTO = غير مخزَّن ⇒ مشتق) |
| `Notes` | `nvarchar(300)` | NULL |
| `ParentId` | `int` | NULL — FK ذاتي `Restrict` إلى `Accounts.Id` |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL — ✅ منفَّذ (الحقل الرقمي أُزيل: صفر `UpdatedByUserId` في اللقطة) |

> **تصحيح مراجعة 2026-11-17:** كان `NameEn` موصوفاً `NOT NULL` — والمقيس في الـsnapshot وفي `AccountConfiguration` أنه **`NULL`**. صُحِّح هنا لا في الكود (القاعدة تتكيّف مع الكود).

#### 4.1.1 سمات مشتقة — **لا أعمدة لها في القاعدة**

المواصفة الواردة تطلب حقولاً لا تُخزَّن؛ تُحسب في طبقة الأعمال (`.Business`/BLL) وتُعرض في الـDTO. **ممنوع** إضافة عمود لها أو `UPDATE` جماعي لتعبئتها (`RULE-N198`):

| السمة | كيف تُشتق | لماذا لا تُخزَّن |
|---|---|---|
| `ParentAccountCode` | كود الأب عبر `ParentId` | نص مكرَّر من الشجرة |
| `TreePath` | سلسلة الآباء بالسلّم الرسمي `/1/11/1101/11010001/` | يحتاج `UPDATE` على كل الصفوف عند أي نقل فرع |
| `FinancialStatement` | من `Nature`: 1/2/3 ⇒ ميزانية · 4/5 ⇒ نتيجة أعمال | دالة ثابتة من نوع الحساب |
| `IsSubAccount` | `ParentId IS NOT NULL` | نفس المعلومة |
| `Balance` | مجموع `JournalEntryLines` (مدين − دائن) بإشارة الطبيعة | **الرصيد لا يُخزَّن في دليل الحسابات** — يُجمَّع من القيود |
| `CategoryLabel` (عميل · مورّد · صندوق · بنك · موظف) | **بادئة الفرع في السلّم الرسمي:** `1103` عملاء (المدينون) · `2101` موردون (الدائنون) · `1101` صناديق · `1102` بنوك ومحافظ · `1107` عهد الموظفين. التصنيف **يُورَّث للفرع**: `11030001` عميل مثل أبيه | تصنيف سلوكي متغيّر — بيت واحد في `Business/Helpers/AccountCategoryHelper` |

#### 4.1.2 قواعد الشجرة والأكواد

| القاعدة | القيمة |
|---|---|
| عمق الشجرة | `Level` من **1 إلى 10** (يقيده `CHK_Account_Level_Range`) · `level = parent.Level + 1` · الجذر = 1 |
| الورقة | `IsLeaf = 1` تعني «تُقيَّد عليها القيود» — **مسموحة في أي مستوى** (لا شرط أن تكون 4) |
| **طول الكود — السلّم الرسمي (قرار المالك 2026-11-17)** | **مستوى 1 (مجموعة) = خانة واحدة** `1` · **مستوى 2 (رئيسي) = خانتان** `11` · **مستوى 3 (فرعي) = 4 خانات** `1101` · **مستوى 4 (تفصيلي/حركة) = 8 خانات** `11010001` ⇒ اللاحقة: 1 ثم 2 ثم 4 خانات |
| كود الابن | `parentCode` + لاحقة رقمية بعدد الخانات المفروض للمستوى (1 · 2 · 4)؛ وإن كان الكود المرشّح موجوداً يُزاد حتى يفرغ |
| الأب الورقة | ممنوع: لا يُضاف ابن تحت `IsLeaf = 1` — يُرفض برسالة عربية |
| **العملة** | **لا عمود عملة في `Accounts`** — بقرار المالك 2026-11-17 («remove cuurencey»)؛ ووحدة العملات مؤجلة إلى V2 (`§2.29` بند 4). لا تُضاف بحجة المواصفة |
| مراكز التكلفة | **لا جدول `CostCenters` في V1** — مؤجل (جدول V2 أدناه) والعمود `AllowCostCenter` هو كل ما يُحفظ الآن |

##### 4.1.2.1 سلّم أطوال الأكواد (رسمي — قرار المالك 2026-11-17)

| المستوى | عدد الخانات | الوصف | مثال |
|---|---|---|---|
| **1** | **1** | مجموعة | `1` — الأصول |
| **2** | **2** | رئيسي | `11` — الأصول المتداولة |
| **3** | **4** | فرعي | `1101` — النقدية وما في حكمها |
| **4** | **8** | تفصيلي/حركة | `11010001` — الصندوق الرئيسي |

> **أثر ملزم:** مولّد الأكواد القائم (`AccountCodeGeneratorService`) يطبّق هذا السلّم أصلاً (2 ← +1 خانة · 3 ← +2 · 4 ← +4) فيبقى كما هو؛ **الخطأ في بيانات الـMock** التي استخدمت `1111/1121` بدل `1101/11010001` — وتُصحَّح هناك. وبذور `AccountingSeeder` الحقيقية تطابق السلّم أصلاً.


## AC.3 JournalEntries
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 23

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AttachmentPath` | `nvarchar(500)` | NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Description` | `nvarchar(500)` | NOT NULL |
| `EntryDate` | `date` | NOT NULL |
| `EntryNo` | `int` | NOT NULL |
| `EntryNumber` | `nvarchar(50)` | NOT NULL |
| `EntryType` | `tinyint` | NOT NULL |
| `IsReversed` | `bit` | NOT NULL |
| `PaymentVoucherId` | `int` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `ReceiptVoucherId` | `int` | NULL |
| `ReferenceId` | `int` | NULL |
| `ReferenceNumber` | `nvarchar(50)` | NULL |
| `ReferenceType` | `nvarchar(50)` | NULL |
| `ReversedByEntryId` | `int` | NULL |
| `ReviewedAt` | `datetime2` | NULL |
| `ReviewedByUserName` | `nvarchar(100)` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |


## AC.4 JournalEntryLines
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 7

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `Credit` | `decimal(18,2)` | NOT NULL |
| `Debit` | `decimal(18,2)` | NOT NULL |
| `Description` | `nvarchar(300)` | NULL |
| `JournalEntryId` | `int` | NOT NULL |
| `SortOrder` | `smallint` | NOT NULL |


## AC.5 ReceiptVouchers
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `TotalAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `VoucherDate` | `date` | NOT NULL |
| `VoucherNo` | `int` | NOT NULL |


## AC.6 PaymentVouchers
**المفتاح:** `int IDENTITY` · **الفئة:** `DocumentEntity` · **الأعمدة:** 14

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `CancelledAt` | `datetime2` | NULL |
| `CashBoxId` | `int` | NOT NULL |
| `CreatedAt` | `datetime2` | NOT NULL |
| `CreatedByUserName` | `nvarchar(100)` | NULL |
| `Notes` | `nvarchar(500)` | NULL |
| `PostedAt` | `datetime2` | NULL |
| `Status` | `tinyint` | NOT NULL |
| `TotalAmount` | `decimal(18,2)` | NOT NULL |
| `UpdatedAt` | `datetime2` | NULL |
| `UpdatedByUserName` | `nvarchar(100)` | NULL |
| `VoucherDate` | `date` | NOT NULL |
| `VoucherNo` | `int` | NOT NULL |


## AC.8 SystemAccountMappings
**المفتاح:** `int IDENTITY` · **الفئة:** `Entity` · **الأعمدة:** 3

| العمود | النوع | Nullability |
|---|---|---|
| `Id` | `int` | NOT NULL |
| `AccountId` | `int` | NOT NULL |
| `MappingKey` | `nvarchar(100)` | NOT NULL |


---

# ملخص المجموعات

| المجموعة | العدد | الجداول |
|---|---|---|
| أ — الأمان والأدوار | 2 | `Roles` · `UserSessions` |
| ب — الإعدادات | 4 | `Taxes` · `FiscalYears` · `Notifications` · `Attachments` |
| ج — المحاسبة | 6 | `AccountCategories` · `Banks` · `TreasuryTransfers` · `CostCenters` · `VoucherInvoiceAllocations` · `FiscalPeriods` |
| د — المخزون | 4 | `InventoryCounts` · `InventoryCountLines` · `WarehouseTransfers` · `WarehouseTransferLines` |
| هـ — العملات | 2 | `Currencies` · `CurrencyRates` |
| و — البيانات التجارية | 3 | `Departments` · `CustomerContacts` · `SupplierContacts` |
| ز — عروض الأسعار | 2 | `SalesQuotations` · `SalesQuotationItems` |
| **AC — المحاسبة (نُقلت من V1 في V3)** | **6** | `Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings` |
| **الإجمالي المؤجل** | **29** | 23 تصميماً فقط + 6 منقولة من V1 |

> **تنبيه على العدد:** العشرون وثلاثة **بلا كود**؛ الستة المنقولة **لها كود قائم** حتى موجة الإزالة (جدول الأثر: `docs/database-schema.md` §8.1). لا يُقرأ الرقم 29 كأنه «29 جدولاً غير موجود».

---

# قائمة تحقق يدوي

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | ابحث في `SalesDbContextModelSnapshot.cs` عن أي اسم من الأسماء الـ**23** التصميمية | **صفر** — لا جدول تصميمي موجود |
| 2 | ابحث في `SalesSystem.Models/**` عن ملفات بهذه الأسماء الـ23 | **صفر** |
| 3 | ابحث في `SalesSystem.Desktop/Services/**` عن عقود/تنفيذات لهذه الأسماء الـ23 | **صفر** |
| 4 | ابحث في الـsnapshot عن الأسماء الستة المنقولة (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) | **6** اليوم — **صفر** بعد موجة الإزالة |
| 5 | ابحث في `docs/database-schema.md` (V3) عن كتلة `Accounts` أو `JournalEntries` في §4 | **صفر** — تعريفاتها هنا فقط، و§8 يسمّيها ويشرح أثر إزالتها |
| 6 | افتح `frmMain` وتحقق من القوائم | لا يوجد عنصر لتحويلات المخزون ولا للجرد (أُزيلا بقرار المالك، والملفات باقية) — **وعناصر المحاسبة (`frmAccounts` · `frmAccountsTree` · `frmJournalEntries`) قائمة حتى موجة الإزالة** |

---

**الإصدار:** V2 (مرجع مؤجل) · **التاريخ:** 2026-11-17 · **V1 الحالي:** `docs/database-schema.md` (**V3** — 34 جدولاً · المحاسبة مؤجلة)
