# TASK-027 — تشغيل الوضع الحقيقي للاختبار (Real-Mode Backend Test)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-027` |
| `العنوان` | **تحويل التطبيق إلى الوضع الحقيقي (`UseMockData = false`) على قاعدة اختبار جديدة، بعد إثبات أن القاعدة القائمة لا تصلح** |
| `الحجم` | **S** — ملف إعداد واحد (قيمتان) + سجل · صفر تغيير كود · صفر تغيير هيكل |
| `المرحلة` | `—` (تحضير اختبار) |
| `الحالة` | **مفتوحة** (2026-11-20) — الجاهزية مكتملة · بناء **0 Errors / 112 Warning(s)** · بانتظار اختبار المالك على `SalesSystem_V1_Test` |
| `الوكيل المالك` | `fast-agent` ×1 (`appsettings.json`) |
| `أثر قاعدة البيانات` | **صفر تغيير على القواعد القائمة** — لا `DROP` · لا `ALTER` · لا `database update` من أي وكيل · القاعدة الجديدة يُنشئها EF عند أول تشغيل |
| `التاريخ` | `2026-11-20` |
| `أمر المالك` | «now I can do real test for backend code for save data at database now» |

---

## 1. القياس قبل أي تغيير (Baseline)

| # | ما قِيس | الأمر | النتيجة |
|---|---|---|---|
| 1 | إعداد التشغيل الحالي | قراءة `appsettings.json` | `UseMockData = true` · `Database=SalesSystemDb` |
| 2 | سلوك الإقلاع في الوضع الحقيقي | `Program.cs:392-417` + `DatabaseInitializer.cs:23-48` | `GetPendingMigrationsAsync` ← `MigrateAsync` ← `DbSeeder.SeedAsync`، ثم **فشل مُغلق** (رسالة عربية + خروج) بلا شاشة دخول |
| 3 | عدد جداول القاعدة القائمة | `sys.tables` | **42** |
| 4 | سجل الهجرات القائم | `__EFMigrationsHistory` | **17** صفاً، آخرها `20261112071109_MvpV1Conformance` — ولا وجود لـ`20261120065605_InitialCreate` |
| 5 | الجداول الميتة في القاعدة | مقارنة مع الـ33 | **9**: `__EFMigrationsHistory` · `Accounts` · `Branches` · `JournalEntries` · `JournalEntryLines` · `PaymentVouchers` · `ProductPrices` · `ReceiptVouchers` · `SystemAccountMappings` |
| 6 | بيانات حقيقية في القاعدة | `COUNT(*)` | `Products` **14** · `Customers` **2** · `SalesInvoices` **4** · `Users` **3** |
| 7 | **أعمدة يحتاجها الموديل ولا وجود لها في القاعدة** | مقارنة عمود بعمود (385 عموداً في 33 جدولاً) | **49 عموداً ناقصاً** |
| 8 | مطابقة اللقطة للموديل | `dotnet ef migrations has-pending-model-changes` | «No changes have been made to the model since the last migration» ⇒ قاعدة جديدة ستطابق الموديل **حرفياً** |
| 9 | العقود المسجَّلة في الفرع الحقيقي | `Program.cs:130-160` | **29** عقداً من **31** — الناقصان `IStocktakingDesktopService` و`IWarehouseTransferDesktopService` (المرحلة أ · Mock فقط، محروسان بـ`_isMockMode`) |

### الأعمدة الـ49 الناقصة — لماذا تمنع إعادة استخدام القاعدة القائمة

ليست تفاصيل: `CreatedByUserName`/`UpdatedByUserName` على نحو **20 جدولاً** · `Customers.CategoryId` · `Suppliers.CategoryId` · `AuditLogs.UserName` · `SystemLogs.UserName` · `InventoryTransactionLines.BatchNo`/`ExpiryDate`/`WarehouseId`.

`EF Core` يولّد قوائم أعمدة صريحة في كل `SELECT`، فكل استعلام كان سيرمي `Invalid column name`. أي أن **تزوير صف في `__EFMigrationsHistory` لا يُصلح شيئاً** — كان سيمرّر الإقلاع ثم يُسقِط كل استعلام.

### أثر التشغيل على القاعدة القائمة كما هي

`MigrateAsync` يرى `20261120065605_InitialCreate` **معلَّقة** (لأن معرّفها غير مسجَّل) فينفّذ `CREATE TABLE` على جداول موجودة ⇒ استثناء ⇒ `Result.Failure` ⇒ **خروج نظيف بلا أي كتابة** (`RULE-N166` — فشل مُغلق). لا تلف ولا تطبيق جزئي.

---

## 2. القرار

**قاعدة اختبار جديدة `SalesSystem_V1_Test`، والقاعدة `SalesSystemDb` لا تُمسّ.**

السبب: القاعدة القائمة تحمل **4 فواتير بيع و14 صنفاً و3 مستخدمين** حقيقيين، وهي متأخرة عن الموديل بـ**49 عموداً**. إسقاطها = فقدان بيانات؛ وتزوير سجل الهجرات = كل استعلام يفشل. القاعدة الجديدة تُنشئ نفسها من خط الأساس الـ33 وتزرع نفسها، فتصير **مرجعاً نظيفاً** لاختبار الحفظ الحقيقي.

---

## 3. التغيير المنفَّذ

ملف واحد: `SalesSystem/SalesSystem.Desktop/appsettings.json` — قيمتان فقط، بلا مسّ أي مفتاح أو بنية:

| المفتاح | قبل | بعد |
|---|---|---|
| `AppSettings.UseMockData` | `true` | **`false`** |
| `ConnectionStrings.DefaultConnection` → `Database` | `SalesSystemDb` | **`SalesSystem_V1_Test`** |

بقية سلسلة الاتصال بايت-ببايت كما هي (`Server=.` · `Trusted_Connection` · `MultipleActiveResultSets` · `TrustServerCertificate`).

**نسخة `bin` المتقادمة:** `bin/Debug/net10.0-windows/appsettings.json` كانت من **2026-11-14** بقيمة `true` والاسم القديم (بقايا بناء قديم، لأن البناء الحالي يوجَّه إلى `-p:ArtifactsPath`). نُسخت من المصدر فصار التشغيل من ملف التنفيذ والمصدر متطابقين — وإلا لكان التشغيل من `bin` يعود إلى وضع التجربة صامتاً.

---

## 4. ما يلزم للعودة إلى القاعدة الحقيقية لاحقاً (غير منفَّذ)

التبديل إلى `SalesSystemDb` **ليس تغيير إعداد** — يحتاج ترقية **إضافية** بـ49 عموداً (كلها `NULL` أو `DEFAULT` كما أثبت خط الأساس) + تسجيل صف خط الأساس في `__EFMigrationsHistory`. الترتيب الملزم (`RULE-N198` / §2.42):

1. يُكتب سكربت `ALTER TABLE ... ADD` إضافي فقط (صفر `DROP`/`ALTER COLUMN`/إعادة تسمية) — **الوكيل يكتبه، والمالك ينفّذه**.
2. تُقرأ تعليمات SQL المولَّدة قبل التنفيذ.
3. خطة رجوع = نسخة احتياطية قبل التنفيذ.
4. **تنبيه تشغيلي:** `SalesDbContextFactory` (وقت التصميم) لا يزال يثبّت `Database=SalesSystemDb` — فأي أمر `dotnet ef` يستهدفه لا قاعدة الاختبار. لا أثر له على التشغيل، لكنه يضلّل من يظن أن أدوات EF تتبع `appsettings.json`.

---

## 5. البوابات المقيسة

| # | البوابة | المتوقع | المقيس 2026-11-20 |
|---|---|---|---|
| 1 | `dotnet build .\SalesSystem.slnx` | **0 Errors** | ✅ **0 Errors / 112 Warning(s)** |
| 2 | لقطة الموديل = الموديل | `has-pending-model-changes` نظيف | ✅ «No changes have been made to the model» |
| 3 | ملف التشغيل يحمل القيم الجديدة | نعم | ✅ `ss-artifacts/.../appsettings.json` = `false` + `SalesSystem_V1_Test` |
| 4 | نسخة `bin` غير متقادمة | نعم | ✅ نُسخت — 229 بايت، `false` |
| 5 | القاعدة القائمة | لم تُمسّ | ✅ صفر `DROP` · صفر `ALTER` · صفر `database update` |
| 6 | `SalesSystem.Tests` | لم يُمسّ | ✅ |

---

## 6. دليل التشغيل (Owner runbook)

1. شغّل التطبيق (الوضع الحقيقي الآن). أول إقلاع يُنشئ `SalesSystem_V1_Test` + **33 جدولاً** + البذور.
2. الدخول: `admin` / **`12345678`** — يُطلب تغيير كلمة المرور فوراً (`MustChangePassword = true`).
3. البذور المتوقعة: الوحدات السبعة (حبة · كيلو · لتر · متر · كرتون · علبة · جرام) · تصنيف «عام» · «عميل نقدي» · «المورد الافتراضي في النظام» · «الصندوق الرئيسي» · «المخزن الرئيسي» · ~40 مفتاح `SystemSettings` · 7 `DocumentSequences`.
4. نفّذ قائمة §11 في تقرير التسليم (7 خطوات حفظ حقيقي).
5. **متوقع ولا يُعدّ عيباً:** `frmStocktakingNew` و`frmWarehouseTransferNew` يفتحان لكن الحفظ/الترحيل **معطّل** فيهما (المرحلة أ · Mock فقط حتى الموافقة المكتوبة).

---

## 7. المخاطر والقواعد

| الخطر | الضبط |
|---|---|
| الخلط بين قاعدة الاختبار والقاعدة الحقيقية | الاسم صريح `SalesSystem_V1_Test`؛ والعودة تحتاج §4 لا تبديل سلسلة |
| الاعتماد على الوضع الحقيقي قبل الموافقة | لا تعديل باك-إند هنا — التغيير **إعداد فقط** (`RULE-N177` غير مُخترَق) |
| تشغيل `bin` قديم | نُسخت نسخة `bin` من المصدر (§3) |
| ظهور رسالة «تعذر تهيئة قاعدة البيانات» | فشل مُغلق مقصود: السبب الحقيقي في Serilog وحده (`RULE-N29`) |

---

## 8. سجل التغيير

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | القياس: 42 جدولاً في القاعدة · 33 في الموديل · **49 عموداً ناقصاً** · 17 صف هجرة · 4 فواتير حقيقية |
| `2026-11-20` | `appsettings.json`: `UseMockData → false` · `Database → SalesSystem_V1_Test` · بناء **0 Errors / 112 Warning(s)** · مزامنة نسخة `bin` |
