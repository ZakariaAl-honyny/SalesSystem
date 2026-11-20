# TASK-023 — تنظيم شاشات الفواتير في `Screens/All Invoices Screens/` بمجلدات تصنيفية

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-023` |
| `العنوان` | نقل **27 ملفاً** (9 شاشات × `.cs`/`.Designer.cs`/`.resx`) من `Sales` · `Purchases` · `Returns` · `POS` إلى `Screens/All Invoices Screens/{Sales,Purchases,Returns,POS}/` |
| `الحجم` | **M** — نقل ميكانيكي بلا تعديل محتوى (27 ملفاً · صفر منطق · صفر تغيير عقد) |
| `المرحلة` | `—` (تنظيم ملفات) |
| `الحالة` | **منفَّذة — بانتظار اختبار المالك** |
| `الوكيل المالك` | `fast-agent` |
| `أثر قاعدة البيانات` | `لا شيء` |
| `التاريخ` | `2026-11-20` |

## الهدف

تجميع كل شاشات الفواتير في مجلد واحد تحت `Screens/All Invoices Screens/` مقسّماً بتصنيفات، **دون أي تغيير في الكود**: الملفات تُنقل كما هي، والـ`namespace` يبقى كما هو (وهو نمط المستودع المعتمد — `RULE-N74`/سابقة `Dashboard/Froms → Forms`).

## القرار (المالك — 2026-11-20)

- النطاق المختار: **كل** مجموعات الشاشات الست (محرّرا الفاتورة · قائمتا الفواتير · قائمة المرتجعات · منتقيا الفواتير · نافذة طباعة فاتورة الشراء · `frmPOS`).
- «Move them categories them inside folder» ⇒ **مجلدات تصنيفية فرعية**، لا مجلد مسطّح.
- الـ`namespace` **يبقى كما هو** — نقل ملفات فقط (0 تغيير في `Program.cs` · `frmMain.cs` · أي مرجع).

## الجرد بأمر (`RULE-N201` — قبل النقل)

| # | المجلد المصدر | الملفات | العدد |
|---|---|---|---|
| 1 | `Screens/Sales/Forms` | `frmAddUpdateSalesInvoice` · `frmListSales` · `frmSalesInvoiceSelection` (×3 ملفات) | **9** |
| 2 | `Screens/Purchases/Forms` | `frmAddUpdatePurchaseInvoice` · `frmListPurchases` · `frmPurchaseInvoiceSelection` · `frmPurchaseInvoicePrint` (×3) | **12** |
| 3 | `Screens/Returns/Forms` | `frmListReturns` (×3) | **3** |
| 4 | `Screens/POS/Forms` | `frmPOS` (×3) | **3** |
| — | **الإجمالي** | | **27** |

**خارج النقل بالاسم:** `frmPurchaseDiscount` · `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmPurchaseOtherDetails` · `frmSupplierStatement` (نوافذ مساعدة/كشف حساب وليست شاشات فواتير) · `Screens/Reports/**` · `Screens/Inventory/**` · `Screens/CustomerReceipts/**` · `Screens/Expenses/**`.

## الشكل الهدف

```
Screens/All Invoices Screens/
├── Sales/      ← 9 ملفات
├── Purchases/  ← 12 ملفاً
├── Returns/    ← 3 ملفات
└── POS/        ← 3 ملفات
```

## الملفات (قائمة مغلقة)

| # | الملف | العمل |
|---|---|---|
| 1 | الـ27 ملفاً أعلاه | `git mv` إلى التصنيف المقابل — **بلا أي تعديل محتوى** |
| 2 | `SalesSystem.Desktop/SalesSystem.Desktop.csproj` | إزالة `<Compile Update="Screens\Sales\Forms\frmAddUpdateSalesInvoice_Backup.cs">` (ملف غير موجود — مرجع ميت) · الإبقاء على `<Folder Include="Screens\All Invoices Screens\" />` · لا إضافة مسارات (SDK يجمع `**/*.cs` تلقائياً) |

## خارج النطاق (ممنوع بالاسم)

- تعديل محتوى أي ملف منقول (ولو سطراً) · تغيير أي `namespace` · أي `using` في `Program.cs`/`frmMain.cs`/`clsGlobal`
- `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Helpers/**` · `Controls/**`
- `AGENTS.md` · `docs/**` (الـOrchestrator وحده)

## التحقق (معايير الإغلاق)

| البند | الأمر | المتوقع |
|---|---|---|
| عدد المنقول | `Get-ChildItem "Screens/All Invoices Screens" -Recurse -File` | **27** |
| بقايا في المصادر | `Get-ChildItem Sales\Forms,Purchases\Forms,Returns\Forms,POS\Forms -Filter frm*Invoice*` | **0** (و`frmListSales`/`frmListReturns`/`frmPOS` = 0) |
| صفر تعديل محتوى | `git diff --find-renames --stat` | كل صف = **rename 100%** |
| البناء | `dotnet build .\SalesSystem.slnx` | **0 Errors** |
| المرجعية | `Program.cs`/`frmMain.cs` بلا تغيير | `git diff` عليهما = **0** |

## الأدلة

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| البناء قبل النقل | `0 Errors / 95 Warning(s)` (تزايدي) | `dotnet build` | ✅ |
| المنقول | **27** (Sales 9 · Purchases 12 · Returns 3 · POS 3) | `Get-ChildItem "Screens/All Invoices Screens" -Recurse -File` | ✅ |
| بقايا أسماء المنقول في المصادر | **0** (بقي فقط 11 ملفاً مساعداً في `Purchases/Forms` خارج النطاق بالاسم) | `git ls-files …/Sales/Forms …/Returns/Forms …/POS/Forms` | ✅ |
| البناء بعد النقل | **0 Errors / 0 Warning(s)** (تزايدي — مشروع Desktop وحده أعيد بناؤه) | نفس الأمر | ✅ |
| صفر تعديل محتوى | **26 من 27 = `R100`** · `frmPOS.resx` = `R093` (6 أسطر `<metadata …TrayLocation>` كانت معدّلة قبل النقل في شجرة العمل، لا من النقل) | `git diff -M --name-status 75ad772` | ✅ |
| مراجع البرنامج | `git diff -- Program.cs Screens/frmMain.cs` = **فارغ** | نفس الأمر | ✅ |
| **أسماء الموارد المضمّنة** (أهم فحص) | بقيت على الـ`namespace`: `SalesSystem.Desktop.Screens.{Sales,Purchases,Returns,POS}.Forms.frm*.resources` — أي أن `ComponentResourceManager(typeof(...))` يجدها | `Get-ChildItem <artifacts>/obj -Recurse -Filter *.resources` | ✅ — نقل آمن وقت التشغيل |
| `.csproj` | حُذف مرجع ميت واحد (`frmAddUpdateSalesInvoice_Backup.cs` غير موجود — `RULE-N123`) | `git diff` | ✅ |

**ملاحظة تشغيلية (`RULE-N196`):** جلسة أخرى كانت تعمل على نفس الشجرة وأودعت الكوميت `bbe5e8b` وبه **26 من 27** عملية النقل (لأنها عُملت بـ`git mv` قبل الالتقاط). أُكمل النقل بإثبات مسار `frmPOS.resx` القديم — والنتيجة المقيسة أعلاه (27/27 و0 بقايا) صحيحة على الشجرة الحالية.

## المخاطر

- `.resx` يجب أن ينتقل **مع** `.cs`/`.Designer.cs` في نفس الدفعة وإلا ينكسر المصمّم.
- نقل `frmAddUpdatePurchaseInvoice` يجرّ معه تعديلات `TASK-022` غير المودعة — لذلك يُغلق `TASK-022` (مراجعة + كوميت) **قبل** بدء النقل.
- مجلدات المصدر (`Sales/Forms` …) تبقى فارغة (Git لا يتتبّع المجلدات الفارغة) — لا تُحذف قسرياً حتى لا تُفقد أي أدوات مستقبلية.

## سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء المهمة بجرد مقيس من الشجرة |
