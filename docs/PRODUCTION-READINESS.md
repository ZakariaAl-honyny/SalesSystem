# PRODUCTION-READINESS — جاهزية الإنتاج (2026-11-15)

> **الغرض:** سجل واحد لكل ما يمنع الانتقال من «المرحلة أ (Mock)» إلى الإنتاج، مقيساً من أربع مراجعات
> (code-reviewer × 4) + مسح آلي للحل. لا يُنسخ هذا السجل في ملف آخر؛ التحديث هنا فقط (RULE-N169 / RULE-N174).
>
> **الحالة العامة (2026-11-15):** البناء أخضر (0 Errors · 127 Warnings)، **Blockerان أُغلقا** (محرر التسوية · انهيار تحميل الشبكات)،
> و**وحدة الإعدادات فُتح باك-إندها بموافقة مالك مكتوبة** (MOCK-COVERAGE §7). **بقية الوحدات ما زالت مقفلة الباك-إند** (`RULE-N177`).

---

## 1) ما أُغلق في هذه الموجة

| # | البند | الملف | الدليل |
|---|---|---|---|
| 1 | دمج لون التحديد والحقل النشط في مفتاح واحد #FFFFB4 وحذف ActiveFieldColor | `Helpers/ThemePalette.cs` + 5 Helpers + 58 شاشة | ActiveFieldColor = 0 في الحل · البناء 0 أخطاء |
| 2 | Blocker: محرر تسوية المخزون كان **يستحيل حفظه** (تحقق يشترط بنداً ولا طريق لإضافته، والطلب بلا بنود) | `Screens/Inventory/Forms/frmAddUpdateInventoryAdjustment(.Designer).cs` | مسار إضافة بند + AddLineAsync + إلغاء المسودة اليتيمة عند فشل بند |
| 3 | تظليل الحقل النشط كان **موصولاً مرتين** (بيت ثانٍ للسلوك) | `Helpers/ThemeHelper.cs` (حُذف WireFocusIndicator) + `frmAddUpdateSalesInvoice.cs:211` | 0 مراجع متبقية |
| 4 | Enter لا ينفّذ «حفظ» في 3 شاشات | `frmSupplierPaymentDialog` · `frmAddUpdateExpense` · `frmAddUpdateCustomerReceipt` | WireModeEnter(this, () => btnSave) |
| 5 | تعليقان بعدّاد قديم (24) ودعوى غير صحيحة في ترويسة اللوحة | `MockThemeColorDesktopService.cs:36` · `ThemePalette.cs:15` | مُصحّحان |
| 6 | سجل المكان الواحد: GAP-02 مغلق (0 موضع حرفي 0xD6E4F0 بدل 56) | `docs/SHARED-HOME-REGISTER.md` · `AGENTS.md` | محدَّثان |
| 7 | **Blocker: «فشل تحميل البيانات» في كل شاشة ماستر** — `ArgumentException: Column named IsActive cannot be found` في `_ApplyRowColoring()` (سجل 2026-11-15) | `Helpers/GridRowHelper.cs` + `Helpers/GreenThemeHelper.cs` + 8 ماسترات | `ApplyInactiveRowStyle` (حلّ بالاسم ثم `DataPropertyName`، ترميم السطر قبل الحكم) · صفر `Cells["<حقل منطقي>"]` في الماسترات · صفر `MistyRose`/`DarkRed` فيها (`RULE-N179`) |
| 8 | **Stage B — وحدة الإعدادات (ألوان النظام + التنسيق): حفظ حقيقي** | `Services/Direct/DirectThemeColorService.cs` · `DirectFormatSettingsService.cs` · `Program.cs` · `Helpers/ThemePalette.cs` | `SystemSettings` عبر `IStoreSettingsService` (بلا كيان/ترحيل) · `ThemePalette.ValidateAll`/`TryApplyAll` بيت واحد · التفضيلات تُطبَّق عند الإقلاع · كلا فرعي DI مسجَّل |
| 9 | **ثغرة إقلاع حقيقي: `ISystemSettingsRepository` + `IMemoryCache` غير مسجَّلين** ⇒ `ProductService`/`CashBoxService`/`SalesService` تُفشل الوضع الحقيقي | `Program.cs` | `AddMemoryCache()` + `ISystemSettingsRepository → SystemSettingsRepository` |

---

## 2) عوائق الإنتاج — كود ناقص (Missing code)

| # | الخطورة | الموضع | النقص | الأثر |
|---|---|---|---|---|
| **P-01** ✅ | حرج | `frmWarehouseTransferNew.cs` · `frmStocktakingNew.cs` | **أُغلق 2026-11-15:** الحفظ صار يمرّ بالعقد الحقيقي (`IWarehouseTransferDesktopService.CreateAsync` · `IStocktakingDesktopService.CreateAsync/PostAsync/CancelAsync`) عبر بوابة `_isMockMode` | الوضع الحقيقي ينفّذ الحركة فعلاً؛ وضع التجربة يبقى محاكاة معلنة |
| **P-02** | عالٍ | `frmProductMaster.cs:741` (TODO) | أسعار الصنف لا تُسند إلى ProductPriceDto | سعر/مستوى سعر يُدخل ولا يُحفظ |
| **P-03** | عالٍ | `frmUserMaster.cs:810,832` (TODO) | DefaultCashBoxId يُترك null · ResetPasswordAsync غير موصولة بالواجهة | صندوق المستخدم وإعادة تعيين كلمة المرور غير قابلين للتنفيذ من الشاشة |
| **P-04** | عالٍ | `Services/Direct/DirectReportService.cs:84-87` | الهاتف/المجموعة/CreditLimit تُرجع null أو 0 | تقارير العملاء تعرض فراغات في الوضع الحقيقي |
| **P-05** | عالٍ | `Services/Direct/DirectFinancialReportService.cs:114,121,179` | حقول ناقصة + ReferenceNumber: null | كشف/تقرير مالي بلا مرجع القيد |
| **P-06** | عالٍ | `Services/Direct/DirectSalesReportService.cs:92,101` | حقلا تقرير غير مُسندين | تقرير مبيعات ناقص في الوضع الحقيقي |
| **P-07** | متوسط | `Services/Direct/DirectCategoryService.cs:46,63` | Description/BranchId يسقطان في التحويل | بيانات فئة تُفقد بصمت |
| **P-08** | متوسط | `Services/Direct/DirectSettingsService.cs:82` | TODO على مفتاح إعدادات | إعداد لا يُقرأ فعلياً |
| **P-09** | متوسط | `Screens/Inventory/Forms/frmInventoryAddProduct.cs` | شاشة يتيمة: مسجَّلة في DI ولا يفتحها أحد، و«حفظها» نجاح وهمي والبيانات تُهمل | ميزة ميتة مربكة |
| **P-10** | متوسط | 5 شاشات مخزون (`frmListInventoryBatches:162` · `frmInventoryList:138` · `frmInventoryFifo:87` · `frmOpeningStock:147` · `frmAddUpdateInventoryAdjustment:147`) | N2/N3 حرفية بدل GridFormatHelper | منازل التنسيق التي يضبطها المالك لا تصل هذه الأعمدة |
| **P-11** | متوسط | `frmInventory.cs:398-489` (+3 شاشات) | باني تصفية RowFilterHelper مكرّر 4 مرات | مخالفة RULE-N171/N172 — إصلاح واحد يجب أن يكفي |
| **P-12** | متوسط | `docs/MOCK-COVERAGE.md:108` | صف IInventoryBatch/IWarehouse يقول «بلا رفض تكرار» والـMock يرفض التكرار فعلاً | سجل لا يطابق الكود ⇒ حكم 0c غير موثوق |
| **P-13** | عالٍ | `Program.cs` (DI) | ثلاث خدمات غير مسجَّلة: `IReportRepository` (يُحقن في `ReportService`) · `IPrintService` + `IPrintDataService` (يُحقنان في `SalesService`)، و`PrintingBootstrapper.Initialize()` لا يُستدعى | الوضع الحقيقي: التقارير وفاتورة البيع/الطباعة تُفشل بالحلّ — بعد إغلاق ثغرة `ISystemSettingsRepository` (بند 9) |

> **P-04…P-08 كلها في `Services/Direct/**` ⇒ الباك-إند مقفل (RULE-N177):** لا تُصلح قبل موافقة مكتوبة،
> وتُحل في المرحلة ب مع اختبار الوضع الحقيقي.

---

## 3) كود خاطئ (Wrong code) — مقيس

| # | الموضع | الخطأ | الحكم |
|---|---|---|---|
| W-01 | `frmListSales.cs:381` | Color.MistyRose حرفي بينما السطر التالي يستخدم GreenThemeHelper.StatusWarningBack | ✅ أُغلق في الماسترات الثمانية (تلوين «غير نشط» من `GreenThemeHelper.InactiveRow*` عبر `GridRowHelper`) — يبقى في `frmListSales`/`frmAccounts`/`frmJournalEntries` (ألوان حالة تُنقل في موجة لاحقة) |
| W-02 | `frmListPurchases.cs:282` | NetTotal.ToString("0.##") في مسند البحث | البحث لا يطابق ما تعرضه الشبكة (clsFormat.Money) |
| W-03 | 5 شاشات قوائم | private static Color FocusColor => …SelectionRowColor | ليست خطأً (تقرأ حياً) لكنها وسيط زائد — مرشّحة للتوحيد |
| W-04 | `throw new Exception` ×2 في SalesSystem.Desktop | لم يُحدَّد موضعهما (لا أداة بحث بالمحتوى في هذه الجلسة) | يجب تحديدهما قبل الإنتاج |
| W-05 | catch { } الفارغة | **0** موضع | سليم |
| W-06 | MessageBox.Show خارج clsDialogs | **0** موضع | سليم |
| W-07 | HttpClient / Business / DataAccess في الشاشات | **0** موضع | سليم |
| W-08 | ex.Message للمستخدم · Console.WriteLine | **0** موضع | سليم |

---

## 4) الشاشات المسجَّلة ولا يفتحها أحد (16)

CustomerSelection · CustomerStatement · InventoryAddProduct · Login · PurchaseCurrency · PurchaseDiscount ·
PurchaseInvoicePrint · PurchaseItem · PurchaseItemInfo · PurchaseOtherDetails · StocktakingPrint ·
StocktakingPrintAdjustment · StocktakingPrintCard · SupplierSelection · SupplierStatement · WarehouseTransferPrint

**الحكم:** 14 منها سليمة بالتصميم (تُفتح من شاشاتها: منتقيات · حوارات · نوافذ طباعة) وLogin يفتحها Program.cs.
**الاستثناء الوحيد الحقيقي: frmInventoryAddProduct** (P-09).

---

## 5) بوابة الانتقال إلى الإنتاج

| البوابة | الحالة |
|---|---|
| dotnet build = 0 أخطاء | ✅ آخر قياس: 0 Error (127 Warning من ملفات الموجة المتزامنة) |
| مراجعة code-reviewer | ✅ 4 مراجعات (1 Blocker · 9 Major) — أُغلق Blocker و3 Major |
| صفر ملفات خارج النطاق في git status | ⚠️ الموجة المتزامنة تحتفظ بعشرات الملفات |
| AGENTS.md + السجلات محدَّثة | ✅ |
| موافقة المالك المكتوبة على سلوك الـMock | ✅ **وحدة الإعدادات فقط** (2026-11-15 — MOCK-COVERAGE §7) · بقية الوحدات 0 |
| وضع حقيقي مُختبَر (DB) | ⏳ بدأ: مسار الإعدادات الحقيقي مكتوب ويحتاج تشغيل المالك على قاعدة بيانات |
| P-01 / P-02 / P-03 مغلقة | P-01 ✅ · P-02 / P-03 ❌ |
| قبول المالك للاختبار اليدوي | ❌ لم يجرِ بعد |

> **الخلاصة:** المرحلة أ قابلة للمراجعة بعد إغلاق P-01…P-03، و**غير قابلة للإنتاج** قبل المرحلة ب
> وموافقة مكتوبة وإصلاح P-04…P-08.

---

## 6) ترتيب العمل المقترح

1. **P-01** (حفظ وهمي للتحويل والجرد) — أخطر بند، داخل المرحلة أ (Mock) فقط.
2. **P-02 / P-03** (أسعار الصنف · صندوق المستخدم وإعادة تعيين كلمة المرور).
3. **P-09** قرار مالك: حذف frmInventoryAddProduct أو توصيلها بمالك.
4. **P-10 / P-11 / W-01 / W-02** — تنظيف بيت واحد (S/M).
5. **P-12** — تصحيح السجل + إعادة تصديق mock-qa.
6. تسليم المرحلة أ ← موافقة مكتوبة ← المرحلة ب ← **P-04…P-08**.

---

**ملاحظة أداة:** كُتب السجل بينما أدوات shell/subagent غير متاحة في الجلسة، فلم يُعَد البناء ولم تُشغَّل الإصلاحات
المتبقية؛ البنود المغلقة أعلاه أُغلقت قبل فقدان الأدوات وبفحص مُلصَق من الوكيل المنفّذ.
