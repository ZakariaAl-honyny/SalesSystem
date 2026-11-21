# TASK-031 — الباركود على وحدة الصنف وحدها (إزالة `Products.Barcode`)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-031` |
| `الحجم` | **L** — هيكل + ≥ 3 طبقات + 18 ملفاً في الكود |
| `المرحلة` | `—` (إزالة ميزة/هيكل بأمر مالك مكتوب — لا شاشة جديدة) |
| `الحالة` | **منفَّذ في الكود — 0 Errors · بانتظار اختبار المالك وموافقة إسقاط العمود** |
| `التاريخ` | `2026-11-20` |
| `الموكلون` | `database-engineer` · `backend-architect` ×2 · `implement-agent` · `ui-agent` ×7 · `fast-agent` |

## أمر المالك (حرفياً)

1. «remove the Barcode for Product the Barcode for per procuct unit @docs/database-schema.md update code and database schema now by using many subagents» — 2026-11-20.
2. بعد سؤال الفرق، اختار: **«حذف عمود الباركود من شبكات الأصناف»**.
3. «update by new database schema frmImportProductsExcel.cs» — 2026-11-20.

## التصميم المعتمد

| الطبقة | القرار |
|---|---|
| `Product` | **لا** خاصية `Barcode`، ولا وسيط `barcode` في `Create`/`Update`، ولا فهرس `IX_Products_Barcode` |
| `ProductConfiguration` | صفر `Barcode` |
| `Create/UpdateProductRequest` | **لا** حقل `Barcode` |
| `ProductDto.Barcode` | يبقى **مُشتقاً للقراءة فقط** = باركود الوحدة الأساسية (يملؤه `ProductService.MapToDto` من `Units`) — كي لا تنفجر شاشات العرض/الطباعة التي تقرأ «باركود الصنف» |
| شبكات الأصناف | **لا عمود باركود ولا تصفية به** (`frmCRUDCProductMaster` · `frmProductSelection` · `frmInventory`) |
| البحث بالباركود | عقد واحد: `IProductDesktopService.ResolveBarcodeAsync(string, CancellationToken)` → `BarcodeResolutionDto` (ProductId · ProductUnitId · UnitName · ConversionFactor · Barcode)؛ و`GetByBarcodeAsync` يبقى ويقرأ `ProductUnits` |
| `ProductUnits.Barcode` | يبقى هو الحارس الوحيد — الفهرس الفريد `IX_ProductUnits_Barcode` |

## خريطة البحث الحالية بالباركود (مقيسة)

| الشاشة | المسار بعد التعديل |
|---|---|
| `frmPOS` | `txtBarcode` + `Enter` ⇒ `ResolveBarcodeAsync` ⇒ سطر بالوحدة الممسوحة (+1 عند التكرار) |
| `frmAddUpdateSalesInvoice` | حقل الباركود + خلية `dgvItems` ⇒ `ResolveBarcodeAsync` ⇒ الوحدة الممسوحة ومعاملها |
| `frmAddUpdatePurchaseInvoice` + `ctrlLineEntryPanel` | المكوّن يعيد `BarcodeResolutionDto` ويختار الوحدة في بيت واحد `PickUnit` |
| `frmInventoryBarcode` | `ResolveBarcodeAsync` ويعرض باركود الوحدة واسمها ومعاملها |
| `frmOpeningStock` | `ResolveBarcodeAsync` + اختيار الوحدة + مزامنة حقل الباركود مع الوحدة |
| `frmStocktakingNew` | `ResolveBarcodeAsync` في بناء السطور |
| `frmCRUDCProductMaster` | حقل «رقم الباركود» للوحدة الرئيسية ⇒ `ResolveBarcodeAsync` (تحميل الصنف) |
| `frmImportProductsExcel` | حكم واحد `_BarcodeRefusalAsync` عبر `ResolveBarcodeAsync` لكل صف + `AddProductUnitRequest.Barcode` |

## ما تغيّر (ملفات هذا الموجة)

| المجموعة | الملفات |
|---|---|
| الكيان/التهيئة | `Models/Product.cs` · `DataAccess/Configurations/ProductConfiguration.cs` |
| العقود | `DTO/Requests/ProductRequests.cs` · `DTO/ProductDtos.cs` · `Desktop/Services/Interfaces/IProductDesktopService.cs` |
| الباك-إند | `Business/Services/ProductService.cs` · `ProductUnitService.cs` · `BarcodeLookupService.cs` · `ProductImportService.cs` · `SalesService.cs` · `Printing/InvoicePrintDtoBuilder.cs` |
| التنفيذ الحقيقي/الوهمي | `Services/Direct/DirectProductService.cs` · `Services/Mock/MockProductDesktopService.cs` · `MockData.cs` · `MockSalesInvoiceDesktopService.cs` |
| الشاشات | `frmCRUDCProductMaster` · `frmImportProductsExcel` · `frmProductSelection` · `frmInventory` · `frmOpeningStock` · `frmStocktakingNew` · `frmInventoryBarcode` · `frmPOS` · `frmAddUpdateSalesInvoice` · `frmAddUpdatePurchaseInvoice` · `ctrlLineEntryPanel` · `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmStocktakingPrintCard` |
| السجل | `docs/database-schema.md` (§3.2 · §9 ص6 · §11 ص6) |

## الترحيل — مسؤولية المالك (`RULE-N198`)

`DataAccess/Migrations/**` لم يمسّها أي وكيل. بعد نسخة احتياطية:

```sql
-- 1) تعبئة رجعية: باركود الصنف القديم → الوحدة الأساسية (حيث فرغت الوحدة وبلا تعارض)
UPDATE pu
SET pu.Barcode = p.Barcode
FROM ProductUnits pu
JOIN Products p ON p.Id = pu.ProductId
WHERE pu.IsBaseUnit = 1
  AND pu.Barcode IS NULL
  AND p.Barcode IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM ProductUnits x WHERE x.Barcode = p.Barcode AND x.Id <> pu.Id);

-- 2) إسقاط الفهرس والعمود
DROP INDEX [IX_Products_Barcode] ON [Products];
ALTER TABLE [Products] DROP COLUMN [Barcode];
```

**رجوع:** `ALTER TABLE Products ADD Barcode varchar(50) NULL;` + إعادة الفهرس المُصفّى.

## الأدلة (مقيسة بأمر)

| ما قيس | القيمة |
|---|---|
| `Barcode` في `Models/Product.cs` | **0** |
| `Barcode` في `ProductConfiguration.cs` | **0** |
| `Barcode` في `ProductRequests.cs` | **0** |
| `ResolveBarcodeAsync` في العقد + Direct + Mock | **1 / 2 / 1** |
| `_existingBarcodes` في شاشة الاستيراد | **0** (حلّ محلها `_BarcodeRefusalAsync`) |
| بناء الحل | **0 Errors** · 95 Warning(s) (incremental) |

## قائمة الاختبار اليدوي (المرحلة أ — وضع التجربة)

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmCRUDCProductMaster` | فتح الشاشة | **لا عمود باركود** · عمودا «أنشأه/آخر تعديل» موجودان · 13 عموداً |
| 2 | `frmCRUDCProductMaster` | إضافة صنف + باركود للوحدة الرئيسية + سعر ← حفظ | نجاح؛ الباركود على الوحدة الأساسية فقط |
| 3 | `frmCRUDCProductMaster` | كتابة باركود وحدة (كرتون) في حقل الباركود + Enter | يُحمَّل الصنف ويُحدَّد سطره |
| 4 | `frmCRUDCProductUnitMaster` | تعديل وحدة إلى باركود وحدة أخرى | رفض عربي «الباركود مستخدم لوحدة أخرى» |
| 5 | `frmPOS` | مسح باركود كرتون | سطر بالكرتون وسعره · إعادة المسح +1 |
| 6 | `frmAddUpdateSalesInvoice` | مسح باركود كرتون | الوحدة = كرتون · المعامل صحيح · الباركود في الشبكة = الكرتون |
| 7 | `frmAddUpdatePurchaseInvoice` | مسح باركود كرتون ثم تغيير الوحدة يدوياً | الباركود يتبع الوحدة المختارة |
| 8 | `frmInventoryBarcode` | مسح باركود كرتون | يعرض باركود الكرتون + اسم الوحدة + المعامل |
| 9 | `frmOpeningStock` | مسح كرتون | تُختار وحدة الكرتون ويتبع حقل الباركود الوحدة |
| 10 | `frmStocktakingNew` | بناء سطور من باركود كرتون | عمود «باركود الوحدة» يحمل باركود الوحدة |
| 11 | `frmImportProductsExcel` | استيراد صفّين: باركود حر · باركود مستخدم على كرتون | الأول يُستورد (الباركود على الوحدة الأساسية) والثاني يُرفض عربياً |
| 12 | `frmProductSelection` · `frmInventory` | فتح الشاشتين | **لا عمود باركود** · التصفية بالاسم تعمل |
| 13 | `frmPurchaseItem` · `frmPurchaseItemInfo` | تغيير الوحدة | التسمية «باركود الوحدة:» والقيمة تتبع الوحدة |

## الجولات

### الجولة 1 — 2026-11-20/21
- أُنجزت الموجات كاملة على الطبقات: كيان/تهيئة → عقود → BLL → Mock/استيراد → الشاشات.
- **حادثة بيئية مقيسة:** Visual Studio كان مفتوحاً وكتب مخزّنه فوق الشجرة عند 19:07 وأعاد موجة كاملة إلى `HEAD`؛ أُعيد تطبيقها **بعد إغلاقه**، والتحقق تم من القرص بالقياس لا من تقارير الوكلاء.
- البناء النهائي: **0 Errors**. لم يُودَع بعد عند كتابة هذا السطر.
