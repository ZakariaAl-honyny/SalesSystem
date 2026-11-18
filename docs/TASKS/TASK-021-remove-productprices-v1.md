# TASK-021 — إزالة جدول `ProductPrices` وكوده من V1 (الاعتماد على `ProductUnits`)

| الحقل | القيمة |
|---|---|
| الحالة | **مفتوحة** (موجة 1) |
| أمر المالك (نصّه الحرفي) | «and also after finish  remove each and everything about ProductPrices table code from all system because we have ProductUnits is enugh.» |
| التاريخ | `2026-11-18` |
| الحجم | **L** — إزالة كيان عبر كل الطبقات (Models · Configurations · DTO · BLL · DataAccess · Desktop · DI · توثيق) |
| المرحلة | **مرحلة واحدة** — إزالة ميزة/مسار بيانات قديم، لا مسار بيانات جديد (`RULE-N165` لا ينطبق) |
| سابقة مرجعية | `TASK-019` (إزالة الضريبة) · `TASK-011` §13 (إزالة المحاسبة) |

---

## 1) القياس أولاً (`RULE-N195`/`RULE-N196`)

**المقدّمة صحيحة — `ProductPrices` موجودة فعلاً وهي زائدة عن `ProductUnits`:**

| القياس | الأمر | النتيجة |
|---|---|---|
| ملفات باسم `ProductPrice*` | `Get-ChildItem -Recurse -Filter ProductPrice*` | **5**: `Models/ProductPrices.cs` · `DataAccess/Configurations/ProductPriceConfiguration.cs` · `Business/Services/ProductPriceService.cs` (يحمل `IProductPriceService` داخله) · `DTO/Responses/ProductPriceDto.cs` · `DTO/Requests/ProductPriceRequests.cs` |
| ملفات تحمل الاسم نصاً | `Select-String -Pattern 'ProductPrice'` | **22 ملفاً** (بلا `bin/obj`، بلا `SalesSystem.Tests` = صفر) |
| `SalesSystem.Tests` | نفس الأمر | **0** — لا اختبار آلي يتأثر |
| جدول قاعدة البيانات | `docs/database-schema.md` §3.5 | 9 أعمدة (`Id` · `CreatedAt` · `CreatedByUserName` · `EffectiveFrom` · `EffectiveTo` · `Price` · `ProductUnitId` · `UpdatedAt` · `UpdatedByUserName`) |

**لماذا `ProductUnits` تكفي (قياس):** `ProductUnit` يحمل **`SalePrice` · `WholesalePrice` · `CostPrice` · `MaxDiscountAmount` · `Barcode`** — أي كل ما تحتاجه الفاتورة والشاشة. `ProductPrices` يضيف **نطاق سعر بالتاريخ** فقط (`EffectiveFrom`/`EffectiveTo`) وهو غير مستهلك في أي شاشة بيع: الشاشات تكتب السعر في **الاثنين** (كتابة مزدوجة) وتقرأ `ProductUnit.SalePrice` أولاً.

**الكتابة المزدوجة المرصودة (أهم دليل على الزيادة):**
- `frmProductMaster._CreateMainUnitAsync` يحفظ `SalePrice` عبر `AddProductUnitAsync` **ثم** ينشئ صف `ProductPrice` ثانياً (السطور 606-619).
- `frmImportProductsExcel` يحفظ `SalePrice: price` عبر `AddProductUnitAsync` **ثم** ينشئ `ProductPrice` ثانياً (السطور 573-580).
- `frmProductMaster._LoadSalePriceAsync` يقرأ `ProductPrices` لكنه **يقدّم** `unit.SalePrice` عليه (`unit.SalePrice > 0m ? unit.SalePrice : rowPrice`).

---

## 2) القرار المعماري

**مصدر السعر الوحيد بعد الموجة = `ProductUnit`.** كل مستهلك كان يقرأ `ProductPrices` يقرأ بدلاً منه:

| المستهلك | قبل | بعد |
|---|---|---|
| `SalesService` — حارس `PreventBelowRetailPrice` (3 مواقع: إنشاء 190 · إنشاء مباشر 298 · ترحيل 421) | `IProductPriceService.GetEffectivePriceForInvoiceAsync(ProductUnitId)` | `_uow.ProductUnits` → `ProductUnit.SalePrice` لنفس `ProductUnitId` (نفس مفتاح الإعداد، نفس نص الرفض العربي) |
| `BarcodeLookupService` | `SalesPrice: 0m` (placeholder معلَّق «pricing moved to ProductPrices») | `SalesPrice: baseUnit?.SalePrice ?? 0m` |
| `frmProductMaster._LoadSalePriceAsync` | قراءة `ProductPrices` مع تقديم `unit.SalePrice` | قراءة `unit.SalePrice` وحدها |
| `frmProductMaster._CreateMainUnitAsync` + `frmImportProductsExcel` | كتابة مزدوجة | قراءة `AddProductUnitAsync` وحدها (السعر يُحفظ في `ProductUnit`) |
| `DbSeeder` | يبذر `ProductPrice` بسعر 100 لكل وحدة أساس | يضبط `SalePrice = 100` على قاعدة `ProductUnit` التي يبذرها (نفس النتيجة بلا جدول ثانٍ) |

---

## 3) النطاق

### داخل النطاق (قائمة مغلقة)

**يُحذف نهائياً (6 ملفات):**
```
SalesSystem.Models\ProductPrices.cs
SalesSystem.DataAccess\Configurations\ProductPriceConfiguration.cs
SalesSystem.Business\Services\ProductPriceService.cs        ← يحمل IProductPriceService
SalesSystem.DTO\Responses\ProductPriceDto.cs
SalesSystem.DTO\Requests\ProductPriceRequests.cs
SalesSystem.DTO\Responses\EffectivePriceDto.cs              ← مستهلكه الوحيد ProductPriceService
```

**يُعدَّل (16 ملفاً):**
```
SalesSystem.Business\Interfaces\IUnitOfWork.cs               ← حذف IGenericRepository<ProductPrice> ProductPrices
SalesSystem.DataAccess\UnitOfWork.cs                         ← حقل + خاصية ProductPrices
SalesSystem.DataAccess\SalesDbContext.cs                     ← DbSet<ProductPrice>
SalesSystem.DataAccess\DbSeeder.cs                           ← كتلة بذر ProductPrices
SalesSystem.Business\Services\SalesService.cs                ← IProductPriceService + 3 مواقع إنفاذ السعر
SalesSystem.Business\Services\BarcodeLookupService.cs        ← SalesPrice من ProductUnit
SalesSystem.Business\Interfaces\Services\IBarcodeLookupService.cs  ← تعليق placeholder
SalesSystem.Business\Services\ProductService.cs              ← تعليق الترويسة
SalesSystem.Business\Interfaces\Services\IProductService.cs  ← تعليق الترويسة
SalesSystem.Desktop\Program.cs                               ← سطر DI لـ IProductPriceService
SalesSystem.Desktop\Services\Interfaces\IProductDesktopService.cs  ← 3 أعضاء
SalesSystem.Desktop\Services\Direct\DirectProductService.cs  ← حقل + ctor + 3 دوال
SalesSystem.Desktop\Services\Mock\MockProductDesktopService.cs      ← 3 دوال
SalesSystem.Desktop\Services\Mock\MockData.cs                ← قائمة ProductPrices
SalesSystem.Desktop\Screens\Products\Forms\frmProductMaster.cs     ← كتابة/قراءة السعر + XML docs
SalesSystem.Desktop\Screens\Products\Forms\frmImportProductsExcel.cs ← كتلة ProductPrice (5)
```

**توثيق (على الـOrchestrator):** `docs/database-schema.md` · `docs/BUSINESS-RULES.md` (BR-050) · `docs/PRODUCTION-READINESS.md` (P-02 · T-04) · `docs/database-schema-v2-deferred.md` (`ProductPrices.CurrencyId`).

### خارج النطاق (بسبب)
- **القاعدة الحيّة:** لا `DROP` ولا `ALTER` ولا ترحيل — الجدول `ProductPrices` **يبقى** على القاعدة ويُسقطه المالك بموافقة مكتوبة (`RULE-N198`).
- `docs/CHANGELOG.md` (14 ذكراً) — **سجل تاريخي** لا يُعاد كتابته؛ الموجة تُضيف سطراً جديداً لا تحذف قديماً.
- `V2_Archive/README.md` — أرشيف تاريخي.
- `SalesSystem.Tests/**` — صفر مرجع أصلاً، وملك المالك (`RULE-N115`).
- `PriceLevel` enum — **يبقى**: يُستخدم في `SaleMode`/واجهات أخرى؟ يُقاس في الموجة ولا يُحذف بلا قياس.

---

## 4) آثار البيانات (تُبلَّغ للمالك — لا تُنفَّذ)

**خطر واحد حقيقي:** إن كانت قاعدة المالك تحمل أسعاراً **حقيقية في `ProductPrices`** و`ProductUnits.SalePrice = 0` (بيانات أقدم من الكتابة المزدوجة)، فبعد الإزالة يظهر السعر **0** في الشاشة والفاتورة.

**فحص الملاءمة قبل إسقاط الجدول (كود يُعطى للمالك، لا يُنفَّذ هنا):**
```sql
SELECT COUNT(*) AS RowsWhereOnlyProductPricesHasThePrice
FROM ProductPrices pp
JOIN ProductUnits pu ON pu.Id = pp.ProductUnitId
WHERE pu.SalePrice = 0 AND pp.Price > 0;
```
- النتيجة `0` ⇒ لا حاجة لأي backfill.
- النتيجة `> 0` ⇒ يُنفَّذ backfill بـموافقة مكتوبة **قبل** الإزالة:
```sql
UPDATE pu SET pu.SalePrice = x.Price
FROM ProductUnits pu
JOIN (SELECT ProductUnitId, MAX(Price) AS Price FROM ProductPrices GROUP BY ProductUnitId) x
  ON x.ProductUnitId = pu.Id
WHERE pu.SalePrice = 0;
```

---

## 5) الموجات

| الموجة | الوكيل | الشريحة | الملفات |
|---|---|---|---|
| 1 | `backend-architect` | تبديل مصدر السعر في BLL | `SalesService.cs` · `BarcodeLookupService.cs` · `IBarcodeLookupService.cs` · `ProductService.cs` · `IProductService.cs` |
| 1 | `backend-architect` | حذف خدمة السعر + DTO + توصيل المستودع + DI | 4 ملفات محذوفة + `IUnitOfWork.cs` + `UnitOfWork.cs` + `Program.cs` |
| 1 | `database-engineer` | الكيان + تهيئة EF + البذور | `ProductPrices.cs` (حذف) · `ProductPriceConfiguration.cs` (حذف) · `SalesDbContext.cs` · `DbSeeder.cs` · تعليقا `Product.cs` · `ProductUnit.cs` |
| 1 | `ui-agent` | Desktop: العقد + التنفيذان + الشاشتان | `IProductDesktopService.cs` · `DirectProductService.cs` · `MockProductDesktopService.cs` · `MockData.cs` · `frmProductMaster.cs` · `frmImportProductsExcel.cs` |

---

## 6) سجل الجولات

### الجولة 1 — الفتح والجرد (2026-11-18)
- قياس الملفات والمراجع في كل الطبقات (الأرقام أعلاه مأخوذة من أوامر `Get-ChildItem`/`Select-String` في نفس الجولة).
- قراءة `ProductPrices.cs` · `ProductPriceConfiguration.cs` · `ProductPriceService.cs` · `ProductPriceDto.cs` · `ProductPriceRequests.cs` · `EffectivePriceDto.cs` · `ProductUnit.cs` · `ProductUnitDto.cs` · `SalesService.cs` (176-201 · 282-302 · 405-457) · `BarcodeLookupService.cs` · `DbSeeder.cs` (150-221) · `frmProductMaster.cs` (560-800) · `frmImportProductsExcel.cs` (500-583).
- قرار الاستبدال (الجدول في §2) وترتيب الموجة الواحدة بأربعة وكلاء على ملفات منفصلة تماماً.
- **لم يُعدَّل أي ملف كود بعد الجولة 1.**

### الجولة 2 — الموجة 1 (4 وكلاء · 2026-11-18)
| الشريحة | الوكيل | النتيجة |
|---|---|---|
| BLL — تبديل مصدر السعر | `backend-architect` | `SalesService` فقد `IProductPriceService` (حقل + معامل + إسناد) · المواقع الثلاثة صارت `_uow.ProductUnits … unit.SalePrice` بنفس مفتاح `PreventBelowRetailPrice` ونفس الرسائل العربية حرفياً · `BarcodeLookupService.SalesPrice` صار `baseUnit?.SalePrice ?? 0m` (كان `0m` placeholder) · تعليقا ترويسة `ProductService`/`IProductService` |
| حذف الخدمة + DTO + المستودع + DI | `backend-architect` | **حُذفت 4 ملفات:** `ProductPriceService.cs` (يحمل `IProductPriceService`) · `ProductPriceDto.cs` · `ProductPriceRequests.cs` · `EffectivePriceDto.cs` · `IUnitOfWork.ProductPrices` و`UnitOfWork` (حقل + خاصية) و`Program.cs` (سطر DI) |
| الكيان + EF + البذور | `database-engineer` | **حُذف ملفان:** `ProductPrices.cs` · `ProductPriceConfiguration.cs` · `DbSet<ProductPrice>` · كتلة بذر `ProductPrices` كاملة؛ سعر 100 صار يُضبط على قاعدة `ProductUnit` التي يبذرها (`UpdatePricing`) — **بلا مسّ أسعار وحدات قائمة** · تعليقا `Product.cs`/`ProductUnit.cs` |
| Desktop | `ui-agent` | أعضاء الأسعار الثلاثة من العقد + `Direct` + `Mock` · قائمة `MockData.ProductPrices` · الكتابة المزدوجة في `frmProductMaster._CreateMainUnitAsync` و`frmImportProductsExcel` · `_LoadSalePriceAsync` صارت `_ApplyMainUnitPrices` (متزامنة — بلا CS1998) وقراءة `ProductUnitDto` وحدها |

**تنظيف لاحق (`fast-agent`):** تعليق `IBarcodeLookupService` أُعيدت صياغته بلا ذكر الجدول · `DirectProductService` فقد اعتماد `ISessionService`/`UserId` الميت بعد حذف الأعضاء (المُنشئ بمعاملين، والـDI يحلّه بلا مستدعٍ يدوي — مقيس: `new DirectProductService` = 0).

### الجولة 3 — البناء والقياس (2026-11-18)
- بناء تزايدي: **Build succeeded — 0 Errors**.
- **بناء كامل `--no-incremental`: 0 Errors / 94 Warning(s).**
- **البقايا المقيسة:** `ProductPrice` في `SalesSystem/**` (بلا `bin/obj` وبلا `Tests`) = **صفر عضو كود** — بعد تنظيف `fast-agent` = **صفر ذكر** أصلاً.
- `ToTable` فريدة في `Configurations/` = **33** (كانت 34) وصفر ذكر لـ`ProductPrices` في القائمة.
- الملفات المحذوفة فعلياً: **6/6** (`Test-Path` = `False` للجميع).

### الجولة 4 — التوثيق (2026-11-18)
- `docs/database-schema.md`: **34 → 33** في ستة مواضع · وحدة المنتجات **5 → 4** وحذف `ProductPrices` من الجرد · حذف قسم §3.5 كاملاً · ملاحظة على §3.4 أن `ProductUnits` هي البيت الوحيد للسعر · صف إزالة في §7 + **§7.1** بفحص الـbackfill الإلزامي قبل الإسقاط · تصحيح بندَي §10 · صف جديد في سجل تغييرات الهيكل §11.
- `docs/BUSINESS-RULES.md` BR-050: `ProductPrices` → `ProductUnit.SalePrice`.
- `docs/PRODUCTION-READINESS.md`: **P-02 ✅ مغلق** و**T-04** صار شِقّ الأسعار مغلقاً.
- `docs/database-schema-v2-deferred.md`: سقط `ProductPrices.CurrencyId` من قائمة أعمدة العملة.

## 7) الأعداد الثلاثة (`RULE-N201`)

| العدّ | العدد | الدليل |
|---|---|---|
| الملفات المصابة قبل الموجة | **22** | `Select-String -Pattern 'ProductPrice'` على `SalesSystem/**` (بلا `bin/obj`، بلا `Tests`) |
| داخل النطاق (مُنجَز) | **22** | 6 محذوفة + 16 معدّلة |
| خارج النطاق (بسبب) | **0** | `SalesSystem.Tests/**` = صفر مطابقة أصلاً · `docs/CHANGELOG.md` سجل تاريخي · `V2_Archive/` أرشيف |
| بقايا عضو كود بعد الموجة | **0** | — |

## 8) البوابات

- ✅ بناء **0 Errors / 94 Warning(s)** (`--no-incremental`).
- ⏸ **لم يُنفَّذ `DROP`** ولا ترحيل: الجدول `ProductPrices` باقٍ على القاعدة الحيّة ويُسقطه المالك بعد فحص §7.1 (`RULE-N198`).
- ⚠️ **تحذير بيانات (الأهم في هذه الموجة):** إن كانت `ProductUnits.SalePrice = 0` بينما السعر الحقيقي في `ProductPrices`، فالسعر يظهر **0** في الشاشة والفاتورة. الفحص والـbackfill في `docs/database-schema.md` §7.1.

## 9) المراجعة (`code-reviewer` — مرة واحدة على الشريحة)

**الحكم: `جاهز للالتزام` — 0 Blocker · 0 Major · 2 Minor.**

تحقّق بالقراءة الكاملة للـdiff من: مفتاح `PreventBelowRetailPrice` ونصوص الرفض العربية الثلاثة (حرفية) · حارّي `ProductUnitId <= 0` · **تكافؤ مسار الوحدة الناقصة** مع الكود القديم (القديم كان `Failure` ⇒ تخطّي، والجديد `unit == null` ⇒ تخطّي) · حفظ السعر في الشاشتين عبر `AddProductUnitAsync`/`UpdateProductUnitAsync` · صفر `ProductPrice` في كل `.cs` · نظافة `DirectProductService` من `_session`/`UserId`/الـusing · تطابق توقيع `cmbMainUnit_SelectedIndexChanged` بعد إسقاط `async` · صفر ملف ترحيل · `ToTable` = 33 · سلامة سلك الشاشة التجريبية (`WireEnterAsTab(pnlFields)` · `RequiredFieldHelper.Attach` · `EntryMode`).

**الملاحظتان:**

| # | الموضع | الشدة | الحالة |
|---|---|---|---|
| 1 | 13 ملفاً تعليقاتها من بقايا المحاسبة تعدّلت خارج نطاق الشريحة (لم يمسّها أي وكيل من هذه الموجة — ظهرت في الشجرة) | Minor | ✅ **عولجت بإلتزام منفصل** (`TASK-020` residue) — لا تُخلط بشريحة `ProductPrices` (`RULE-N162` بوابة 4) |
| 2 | `SalesService.cs` — البحث عن سعر الوحدة مكرّر **3 مرات** (سطران لكل موقع) داخل الصنف | Minor | 📌 **مسجّل كدَين** — بنية الحارس (قراءة الإعداد + الحلقة + الرفض) سابقة للشريحة، وما تغيّر هو إزالة البيت. لا إعادة هيكلة في هذه الموجة (`RULE-N113`) — يُعالج عند أول مساس بالصنف |

## 10) نتائج قياس مستقلة (على الـOrchestrator)

| القياس | النتيجة |
|---|---|
| `ToTable` فريدة في `Configurations/` | **33** — وصفر ذكر لـ`ProductPrices` |
| عقود `I*DesktopService` | **31** = عدد `Mock*DesktopService` = عدد تسجيلات فرع Mock · و**29** في فرع Direct |
| `ProductPrice` في `.cs` (بلا Tests) | **0** |
| `docs/MOCK-COVERAGE.md` — صفوف العقود المحاسبية | أُزيلت (الكود غير موجود منذ `TASK-011`) والأعداد **31/31/31/29** محققة بالأمر |


