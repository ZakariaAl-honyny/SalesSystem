# TASK-013 — إعادة بناء `frmOpeningStock` بنمط `frmAddUpdateSalesInvoice` (إدخال أصناف سريع)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-013` (أُعيد ترقيمه من `TASK-012` لأن الرقم حُجز لمهمة شاشات المنتقيات في جلسة متوازية) |
| `النوع` | إعادة بناء شاشة (UI) — بلا مساس بالباك-إند |
| `الحجم` | L |
| `المرحلة` | `—` (ليست Stage A/B: لا مسار بيانات جديد — العقود قائمة ومراجَعة) |
| `الوكيل` | `ui-agent` (وكيل واحد، الشاشة كلها) |
| `الملفات المملوكة` | `frmOpeningStock.cs` · `frmOpeningStock.Designer.cs` · `frmOpeningStock.resx` (عند الحاجة فقط) |
| `التاريخ` | 2026-11-18 |

## الهدف

طلب المالك (حرفياً): «frmOpeningStock Convert or Make like frmAddUpdateSalesInvoice» · «for enter Products» · «an return for enter product easly» · «and returns scrrens for enter product easly» · «and use validation of reqired faild like frmProductsMaster and also edit validation for frmAddUpdateSalesInvoice».

أي: شاشة «بضاعة أول المدة» تُعاد بناؤها بهيكل وتجربة شاشة فاتورة البيع:
**(1)** لوحة إدخال صنف بالباركود (باركود ← منتج ← وحدة ← كمية ← تكلفة الوحدة ← إجمالي السطر) مع `Enter` يمشي فيها ثم يُضيف السطر ويُعيد المؤشر إلى حقل الباركود فوراً (إدخال سريع متتابع).
**(2)** شبكة أصناف للقراءة فقط + نقر مزدوج لتعديل السطر في لوحة الإدخال.
**(3)** كتلة إجماليات (عدد · كميات · تكلفة + تفقيط).
**(4)** بعد الحفظ والترحيل: رسالة نجاح ثم **إعادة تهيئة مستند جديد على نفس الشاشة** (تبقى مفتوحة) والمؤشر على الباركود — «returns screen for enter product easily».
**(5)** تحقق الحقول المطلوبة بنمط `frmProductMaster` (`RequiredFieldHelper`) + تحقق الإدخال بنمط `frmAddUpdateSalesInvoice` (`ValidateEntryFields`).

## غير داخل النطاق (ممنوع المساس)

- `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/Direct/**` · `Services/Mock/**` · `Migrations/**` — **لا سطر واحد**. العقود المستخدمة قائمة: `IProductDesktopService.GetByBarcodeAsync/GetProductUnitsAsync` · `IWarehouseDesktopService.GetAllAsync` · `IInventoryAdjustmentDesktopService.CreateAsync/AddLineAsync/PostAsync`.
- `Program.cs` (التسجيل قائم: `services.AddTransient<frmOpeningStock>();` — السطر 310).
- `frmAddUpdateSalesInvoice*` · `frmProductMaster*` · `frmProductSelection*` — مراجع للقراءة فقط.
- `Helpers/**` · `Global Classes/**` — تُستهلك كما هي؛ أي نقص يُبلَّغ في التقرير لا يُكتب في الشاشة.
- `AGENTS.md` · `docs/**` — يكتبها الـOrchestrator وحده.

## منطق الحفظ (يبقى حرفياً — لا يُعاد تصميمه)

1. `CreateAsync(new CreateInventoryAdjustmentRequest(WarehouseId: <short>, AdjustmentType: 1))` ← `Opening`.
2. لكل سطر: `AddLineAsync(adjustmentId, new AddInventoryAdjustmentLineRequest(id, ProductUnitId, 0m, Quantity, UnitCost))`.
3. إن فشل سطر واحد: **لا ترحيل**، ورسالة عربية بعدد المتعذر (النص الحالي محفوظ).
4. `PostAsync(adjustmentId)` ثم رسالة نجاح بعدد الأصناف ورقم المستند (`postResult.Value?.AdjustmentNo`) ثم إعادة تهيئة.

## قرارات مسجَّلة (DECISIONS)

| # | القرار | السبب |
|---|---|---|
| D1 | الشبكة **للقراءة فقط** (`RULE-N22`) والتعديل عبر لوحة الإدخال (نقر مزدوج يحمّل السطر) | استثناء الشبكة القابلة للتحرير محصور بـ`dgvItems`/`dgvCart` (`RULE-N22a`) — لا يُوسَّع |
| D2 | تكرار الصنف + الوحدة عند الإضافة يحمّل السطر القائم للتعديل برسالة معلومات | إضافة كميات بتكاليف مختلفة تُفسد افتتاح الصنف |
| D3 | `dtpDate` يبقى معروضاً **معطّلاً** بقيمة اليوم مع تلميح «يُسجَّل تاريخ الترحيل تلقائياً» | `CreateInventoryAdjustmentRequest` لا يحمل تاريخاً (لا BLL/DB في هذه المهمة) — ولا حقل يكذب على المستخدم |
| D4 | بعد الترحيل: الشاشة تبقى مفتوحة وتُعاد إلى «مستند جديد» | طلب المالك الصريح (إدخال أصناف متتابع بسهولة) |
| D5 | حقل الباركود يحلّ محلّ زر «إضافة منتج» كمدخل أساسي، مع زر «اختيار منتج (F4)» للفأرة | نفس تدفّق الفاتورة |

## معيار القبول (Done when)

- الهيكل: `lblMode` ← `gbToolbar` («أوامر المستند») ← `gbInfo` («بيانات المستند») ← `gbEntry` («إدخال صنف» بـ`pnlEntry`) ← `gbItems` («الأصناف») ← `gbTotals` («الإجماليات») — بترتيب الإرساء نفسه ونمط `RULE-N135…N138`.
- `Enter` في الباركود = بحث/تعبئة؛ `Enter` في الكمية/التكلفة = الحقل التالي؛ `Enter` في آخر حقل = إضافة السطر؛ **التركيز يعود إلى `txtBarcode`** بعد كل سطر.
- `F4` في الباركود = منتقي الأصناف · `F2` = مستند جديد · `F4` (خارج الباركود) = حفظ وترحيل · `Delete` على الشبكة = حذف السطر بتأكيد · `Esc` = إلغاء السطر/إغلاق.
- `RULE-N153`: `_PrimaryActionButton()` = `btnSave` وبـ`null` داخل الشبكة؛ `AcceptButton` يُفرَّغ عند دخول الشبكة ويُعاد عند مغادرتها.
- الحقول المطلوبة: `RequiredFieldHelper.Attach(this, errorProvider, new RequiredField(cmbWarehouse, "يرجى اختيار المخزن"))` **بعد** `WireFocusHighlight` و**قبل** الثيم + `Validate/FirstMissing/Clear`.
- تحقق الإدخال: `ValidateEntryFields()` بنمط الفاتورة (`clsValidation.IsPositiveDecimal` / `TryParseDecimal`) + رسائل `clsDialogs.Warning` عربية + `Focus()`/`SelectAll()`.
- الثيم: `GreenThemeHelper.ApplyHeader + ApplyGrid + GridFormatHelper.ApplyColumn(s) + Apply(this)` آخر سطر.
- المصمّم = التشغيل: كل عنصر دائم معلن في `*.Designer.cs` بألوان الصيغة `System.Drawing.Color.FromArgb(Convert.ToInt32(...), ...)` (`RULE-N204`)، ولا استدعاء لأي عضو من `Helpers/` داخل المصمّم.
- صفر `MessageBox` · صفر `ex.Message` للمستخدم · صفر نسخة ثانية من سلوك مشترك (`RULE-N169…N172`).
- صفر تغيير في أي ملف خارج الملفات الثلاثة.
- `dotnet build` = 0 Errors (ينفّذه الـOrchestrator بعد الموجة).

## الأدلة (تُقاس وتُسجَّل بعد الموجة)

| # | الفحص | الأمر |
|---|---|---|
| 1 | الحجم قبل/بعد | `Get-Content … \| Measure-Object -Line` (قبل: 576 / 469) |
| 2 | صفر `MessageBox` في الشاشة | `Select-String -Path frmOpeningStock*.cs -Pattern 'MessageBox'` |
| 3 | صفر كتابة خارج النطاق | `git status --porcelain` |
| 4 | البناء | `dotnet build .\SalesSystem.slnx -p:ArtifactsPath=…` |

---

## LOG — الجولة 1 (2026-11-18)

### W1 — `ui-agent` (شاشة واحدة، ملفان + مصمّمها)

| البند | النتيجة المقيسة |
|---|---|
| الملفات | `frmOpeningStock.cs` **576 → 1,048** سطراً · `frmOpeningStock.Designer.cs` **469 → 916** سطراً · `.resx` بلا مساس |
| الهيكل | `lblMode` ← `gbToolbar` ← `gbInfo` ← `gbEntry/pnlEntry` ← `gbItems` ← `gbTotals` (نمط `RULE-N135…N138`) |
| الإدخال السريع | باركود ⇒ منتج ⇒ كمية ⇒ تكلفة ⇒ إضافة السطر ⇒ **التركيز يعود إلى الباركود** · `F4` داخل اللوحة = منتقي · خارجها = حفظ · `F2` جديد · `Delete` حذف سطر · `Esc` إلغاء/إغلاق |
| التحقق | `RequiredFieldHelper` للمخزن (نمط `frmProductMaster`) + `ValidateEntryFields` بنمط `frmAddUpdateSalesInvoice` (`clsValidation` + رسائل عربية + `Focus/SelectAll`) |
| الحفظ | منطق التسوية الافتتاحية (`Opening` → `AddLine` لكل سطر → `Post`) محفوظ حرفياً + رسالة النجاح تحمل العدد ورقم المستند + **إعادة تهيئة مستند جديد على نفس الشاشة** (D4) |
| الفحوص | `MessageBox` في الشاشة = **0** · `ex.Message` = **0** · مراجع `Helpers` داخل المصمّم = **0** · `FromArgb` بصيغة `Convert.ToInt32` = **52/52** · `SalesSystem.Business`/`DbContext` = **0/0** |
| البناء | `dotnet build .\SalesSystem.slnx` بعد الموجتين = **0 Errors** |

### المراجعة (`code-reviewer`) — pass واحد

`0 Blocker · 1 Major · 4 Minor` ⇒ **APPROVE WITH NOTES**. أُغلقت كلها في موجة الإصلاح:

| # | الملاحظة | الإغلاق |
|---|---|---|
| 1 (Major) | إعادة دخول أثناء حلقة الحفظ (`F2`/`Esc`/`F4`/`Delete`/`Enter` تعدّل المستند أو تُغلق النطاق و`_isSaving = true`) | **17 حرس `if (_isSaving) return;`** على كل نقاط الدخول + قفل `btnNew/btnSave/btnClose/btnPickProduct/btnAddLine/btnCancelLine` وعناصر القائمة + الحلقة على **لقطة** `Rows.Cast<DataRow>().ToList()` + `_UpdateButtonsState` عند الضبط والتحرير |
| 2 | حارس «المخزن مطلوب» ميت ومكرر بعد `RequiredFieldHelper` | حُذف الفرع والرسالة، وبقي تضييق النوع فقط (`RULE-N172`) |
| 3 | كتابة `Barcode` فوق السطر عند التحديث من نص الحقل | لا تُكتب إلا بعد بحث/اختيار ناجح |
| 4 | ابتلاع فشل بني في بحث الباركود | **مؤجل** — يحتاج تمييز `ErrorCode` بين «غير موجود» و«فشل تحويل»؛ خارج نطاق الموجة |
| 5 | هوية السجل `TASK-012` | صُحّحت إلى `TASK-013` |

### شهادة التغطية (`mock-qa`) — بوابة 0c

- تدفّق الشاشة: **`مؤهل جزئياً — تكافؤ الرفض ناقص`** — الشاشة تُفتح وتُدخل وتحفظ وتُرحّل بلا قاعدة بيانات؛ صفر عقد `غير مؤهل`.
- سجل `docs/MOCK-COVERAGE.md`: صف 15 حُدِّث بالحكم والعدّ المقيس (**17** رفضاً) والفجوات `ف-OPS-1…3` (الملك `ui-agent`، حجم S) — **لا باك-إند ولا ملف `Mock*` في هذه الموجة**.
- `ف-OPS-4` (منتقي الأصناف يرفض صنفاً رصيده صفراً عند تمرير مخزن) **أُبطل من داخل الشاشة**: لم يعد `dialog.WarehouseId` يُمرَّر — بضاعة أول المدة هي مَن يُثبّت أول كمية.
- ملاحظة الشهادة عن «تسريب النطاق» **غير صحيحة**: `frmOpeningStock.Designer.cs:12` يستدعي `_scope?.Dispose()`.

### قائمة الاختبار اليدوي

انظر قسم «قائمة الاختبار اليدوي» في تقرير التسليم (69 بنداً في المرحلتين A/B، مكتوبة من `test-engineer` + إضافات موجة الإصلاح).

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)
