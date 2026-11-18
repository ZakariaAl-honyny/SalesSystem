# TASK-018 — شاشة مستند شراء واحدة بوضعين (`فاتورة شراء` / `مرتجع شراء`)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-018` |
| `النوع` | واجهة (بنية شاشة) — بلا مساس بالباك-إند |
| `الحجم` | L — شاشة واحدة (877 + مصمّم 728) + حذف محرّر ثانٍ (817 + 598) |
| `المرحلة` | `—` (لا مسار بيانات جديد: `IPurchaseInvoiceDesktopService` و`IPurchaseReturnDesktopService` قائمان) |
| `الحالة` | 🔄 مفتوحة |
| `التاريخ` | `2026-11-18` |

## 1. طلب المالك

> «do the same for Purchase in frmAddUpdatePurchaseInvoice at `...\Screens\Purchases\Forms\frmAddUpdatePurchaseInvoice.cs`»

أي: نفس ما نُفِّذ في `TASK-017` للمبيعات — الشاشة الباقية `frmAddUpdatePurchaseInvoice` تحمل **وضعين بزرّي اختيار** («فاتورة شراء» / «مرتجع شراء»)، و`frmPurchaseReturn` تُحذف وتُعاد توجيه مداخلها.

## 2. المقدّمة مُتحقَّقة بالقياس

| القياس | النتيجة |
|---|---|
| `frmAddUpdatePurchaseInvoice.cs` / `.Designer.cs` | **877 / 728** — والسطور تُقرأ بالمعرّف في `OnLoad` (`GetByIdAsync`) فمسودة بلا سطور لا تُحفظ فارغة |
| `frmPurchaseReturn.cs` / `.Designer.cs` | **817 / 598** |
| لوحة الإدخال | الشاشتان تستهلكان `ctrlLineEntryPanel` أصلاً (`TASK-014`) — لا نسخة ثانية من حكم الإدخال |
| عقد الفاتورة | `IPurchaseInvoiceDesktopService`: `GetAll` · `GetById` · `Create` · **`Update`** · `Post` · `Cancel` |
| عقد المرتجع | `IPurchaseReturnDesktopService`: `GetAll` · `GetById` · `Create` · `Post` · `Cancel` — **لا `Update`** |
| `CreatePurchaseReturnRequest` | `(int? PurchaseInvoiceId, int SupplierId, int WarehouseId, DateTime? ReturnDate, DiscountType? DiscountType, decimal? DiscountRate, string? Notes, List<CreatePurchaseReturnItemRequest> Items)` — **`WarehouseId` من نوع `int`** (بخلاف المبيعات `short`) |
| `CreatePurchaseReturnItemRequest` | `(int PurchaseInvoiceLineId, int ProductId, int ProductUnitId, decimal Quantity, decimal UnitCost, decimal Amount)` |
| `PurchaseReturnDto` | `ReturnNo` · `SupplierId` · `WarehouseId` · `ReturnDate` (**`DateOnly`**) · `Notes` · `Status` · `Items` |
| `PurchaseReturnItemDto` | `ProductId` · `ProductName` · `ProductUnitId` · **`ProductUnitName`** · `Quantity` · **`UnitCost`** · `LineTotal` |
| «اختيار فاتورة» | `frmPurchaseReturn` فيه `btnPickInvoice` عبر `frmPurchaseInvoiceSelection.SelectedInvoice` — **ميزة يجب أن تبقى** في وضع المرتجع |

## 3. القرارات

| # | القرار | السبب |
|---|---|---|
| `D-1` | الشاشة الباقية `frmAddUpdatePurchaseInvoice` + زرّا اختيار في `pnlToolbar`/`flpToolbar` | نفس نمط `TASK-017` المعتمد من المالك |
| `D-2` | **`frmPurchaseReturn` تُحذف** (`RULE-N102`) بعد إعادة توجيه `frmListReturns` و`frmMain` و`Program.cs` | لا محرّرين لكيان واحد |
| `D-3` | في وضع المرتجع تُخفى حقول البيع: `lblPaymentType`+`cmbPaymentType` · `lblPaid`+`txtPaid` · `lblDue`+`lblDueVal` · `lblInvoiceDiscount`+`numInvoiceDiscount` · `btnPrintA4` | `CreatePurchaseReturnRequest` لا يحمل نوع دفع ولا مدفوعاً (الخصم يُرسَل `null/null` كما كانت الشاشة القديمة) — إظهارها = واجهة تكذب |
| `D-4` | **`btnPickInvoice` يبقى في وضع المرتجع** (`frmPurchaseInvoiceSelection`) ويُخفى في وضع الفاتورة | ميزة قائمة في الشاشة المحذوفة — إسقاطها فقدان وظيفة لا مكسب (`RULE-N199`) |
| `D-5` | المرتجع المحفوظ **للقراءة فقط** (لا `Update` في عقده) | نفس سلوك الشاشة القديمة |
| `D-6` | `_FirstLineError` (وجود سطر · وحدة · كمية > 0 · تكلفة > 0) يُستهلك في الوضعين بلا نسخة ثانية | `RULE-N172` — الحكم واحد، والقواعد صالحة للوضعين |

## 4. مصفوفة الوضعين

| الجانب | فاتورة شراء | مرتجع شراء |
|---|---|---|
| `lblMode` / `Text` | «(( فاتورة مشتريات جديدة ))» / «— {InvoiceNo}» | «(( مرتجع مشتريات جديد ))» / «— {ReturnNo}» |
| الخدمة | `IPurchaseInvoiceDesktopService` | `IPurchaseReturnDesktopService` |
| الحفظ | `Create` ثم `Update` للمسودة | `Create` فقط + `Post` · `_createdReturnId` يمنع مستنداً ثانياً |
| الترحيل | «هل أنت متأكد من ترحيل فاتورة المشتريات؟ سيتم زيادة المخزون.» | «ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.» |
| الإلغاء | `_purchaseApi.CancelAsync` | `_returnApi.CancelAsync` |
| التحرير بعد الحفظ | مسودة ⇒ قابل للتعديل | **مقفل دائماً** |
| الإجماليات | فرعي − خصم · المتبقي = الإجمالي − المدفوع | Σ إجمالي السطور بلا خصم/مدفوع |
| اختيار فاتورة | مخفي | ظاهر (`frmPurchaseInvoiceSelection`) |

## 5. النطاق (قائمة مغلقة)

| الموجة | الملف | المالك |
|---|---|---|
| B | `Models/PurchaseDocumentMode.cs` **(جديد)** | `ui-agent` |
| B | `Screens/Purchases/Forms/frmAddUpdatePurchaseInvoice.cs` + `.Designer.cs` | `ui-agent` |
| C | `Screens/Returns/Forms/frmPurchaseReturn.cs` + `.Designer.cs` + `.resx` **(حذف)** | `ui-agent` |
| C | `Screens/Returns/Forms/frmListReturns.cs` · `Screens/frmMain.cs` · `Program.cs` | `ui-agent` |

**خارج النطاق:** `Business/**` · `DataAccess/**` · `SalesSystem.Models/**` · `DTO/**` · `Services/**` · `Migrations/**` · `ctrlLineEntryPanel.*` · `frmAddUpdateSalesInvoice*` · `frmPurchaseInvoiceSelection*` · `docs/**`.

## 6. معيار القبول

1. «فاتورة شراء» من القائمة ⇒ الوضع صحيح وكل سلوك اليوم حرفياً (Create/Update/Post/Cancel/طباعة/مدفوع/خصم).
2. «مرتجع شراء» ⇒ الوضع صحيح، الحقول المخفية في `D-3` مختفية، و«اختيار فاتورة» ظاهر.
3. تبديل الوضع على مستند فيه سطور ⇒ تأكيد عربي، والإلغاء يعيد الاختيار السابق بلا فقدان.
4. مرتجع: عميل (مورد) + مستودع + سطر ⇒ «حفظ» يُنشئ مرتجعاً، وإعادة المحاولة لا تُنشئ ثانياً.
5. «اختيار فاتورة» في وضع المرتجع يعبّئ المورد والمستودع والسطور (تأكيد قبل الاستبدال).
6. `frmPurchaseReturn` = 0 مرجع و0 ملف · البناء **0 Errors**.

## 7. الجولات

### الموجة B (2026-11-18) — آلة الوضعين داخل `frmAddUpdatePurchaseInvoice`

| البند | القياس |
|---|---|
| `Models/PurchaseDocumentMode.cs` **(جديد)** | **10** أسطر |
| `frmAddUpdatePurchaseInvoice.cs` | 877 → **1,428** |
| `frmAddUpdatePurchaseInvoice.Designer.cs` | 728 → **800** |
| زرّا الاختيار | `lblModeDoc` («نوع المستند:») + `rbtnModePurchase` (محدَّد) + `rbtnModeReturn` أول أبناء **`pnlToolbar`** (الاسم الحقيقي للوحة الأدوات في هذه الشاشة) ⇒ أقصى اليمين في RTL |
| `btnPickInvoice` | معلن في المصمّم · ظاهر في وضع المرتجع فقط · يستعمل `frmPurchaseInvoiceSelection` (ميزة الشاشة المحذوفة — `D-4`) |
| الحقول المخفية في وضع المرتجع | `lblPaymentType`+`cmbPaymentType` · `lblPaid`+`txtPaid` · `lblDue`+`lblDueVal` · `lblInvoiceDiscount`+`numInvoiceDiscount` · `btnPrintA4` — القائمة واحدة `PurchaseOnlyControls` |
| `_documentMode` · `_returnDoc` · `_createdReturnId` | 20 · 31 · 11 موضعاً |
| `_FirstLineError` | **دالة واحدة** للوضعين (`D-6`) — لا نسخة ثانية من حكم الوحدة/الكمية/التكلفة |
| `ProductUnitId = 1` | **0** · `MessageBox` = **0** · `ex.Message` = **0** |
| البناء | **0 Errors** |

### الموجة C (2026-11-18) — شاشة واحدة فقط: حذف `frmPurchaseReturn`

| الملف | الإجراء |
|---|---|
| `frmPurchaseReturn.cs` · `.Designer.cs` · `.resx` | **حُذفت** (700 + 595 + 118 سطراً عند `HEAD`) |
| `frmListReturns.cs` | `_OpenPurchaseEditor` → `frmAddUpdatePurchaseInvoice.LoadAsReturn(dto)` (+ `using` مجلد Purchases بعد التحقق من صفر تعارض أسماء) |
| `frmMain.cs` | `OpenPurchaseDocument(mode)` بنمط `OpenSalesDocument` نفسه (`TryStartNewDocument` + تأكيد) · «مرتجع شراء» → `PurchaseReturn` · «فاتورة شراء» بقي `OpenScreen<T>` حرفياً |
| `Program.cs` | حُذف تسجيل `frmPurchaseReturn` — `frmAddUpdatePurchaseInvoice` مسجَّل مرة واحدة |
| مراجع `frmPurchaseReturn` | **0** · الملفات على القرص = **0** |
| البناء | **0 Errors** |

**فروق وثائق أُبلغ بها ولم تُغلق بعد (على الـOrchestrator):** `docs/MangeSubaggentsAndTracProgress.md` (أسطر 178/186/250) و`docs/TASKS/TASK-014`/`TASK-016` تذكر الشاشات المحذوفة كنطاق قائم؛ و`docs/SHARED-HOME-REGISTER.md` يحمل `T-02` (تكلفة سطر المرتجع = صفر) على الملف المحذوف — **يُعاد قياسه على `SaveReturnAsync` في الشاشة الموحّدة قبل إغلاقه**.

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)
