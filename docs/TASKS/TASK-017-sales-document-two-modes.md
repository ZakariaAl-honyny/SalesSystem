# TASK-017 — شاشة مستند بيع واحدة بوضعين (`فاتورة بيع` / `مرتجع بيع`) بزرّي اختيار

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-017` |
| `النوع` | واجهة (بنية شاشة) — بلا مساس بالباك-إند |
| `الحجم` | L — شاشة واحدة ثقيلة (3,207 سطراً + مصمّم 1,465) + إعادة توجيه مدخلي المرتجعات (`RULE-N155`/`RULE-N156`) |
| `المرحلة` | `—` (لا مسار بيانات جديد: `ISalesInvoiceDesktopService` و`ISalesReturnDesktopService` قائمان) |
| `الحالة` | 🔄 مفتوحة |
| `التاريخ` | `2026-11-18` |

## 1. طلب المالك (نصّه)

> «now you now the return scrren the same Invoive Scrren we must choice one of them `...\Screens\Sales\Forms\frmAddUpdateSalesInvoice.cs` and set tow Modes by using two raio button for change the mode. so you now the frmAddUpdateSalesInvoice for the same form for Sales & Returns the same Invoise.»

**القرار:** الشاشة الباقية هي **`frmAddUpdateSalesInvoice`**، وتُضاف إليها **نقطتا اختيار (RadioButton)** لتغيير الوضع بين **فاتورة بيع** و**مرتجع بيع**. شاشة `frmSalesReturn` تصبح **كوداً ميتاً** وتُحذف (`RULE-N102`).

## 2. المقدّمة مُتحقَّقة بالقياس (لا بالذاكرة)

| القياس | الأمر | النتيجة |
|---|---|---|
| حجم الشاشة الباقية | `(Get-Content …).Count` | `frmAddUpdateSalesInvoice.cs` = **3,207** · `.Designer.cs` = **1,465** |
| حجم الشاشة المُدمَجة فيها | نفس الأمر | `frmSalesReturn.cs` = **753** · `.Designer.cs` = **595** |
| عقد البيع | `Services/Interfaces/ISalesInvoiceDesktopService.cs` | `GetAll` · `GetById` · `Create` · **`Update`** · `Post` · `Cancel` |
| عقد المرتجع | `Services/Interfaces/ISalesReturnDesktopService.cs` | `GetAll` · `GetById` · `Create` · `Post` · `Cancel` — **لا `Update`** |
| من يفتح المحرّرين | `Select-String` على `frmMain`/`frmListReturns`/`frmListSales` | `frmMain:271` → `frmAddUpdateSalesInvoice` · `frmMain:274` → `frmSalesReturn` · `frmListReturns:641` → `frmSalesReturn` · `frmListSales:822/833` → `frmAddUpdateSalesInvoice` |
| تسجيل DI | `Program.cs` | `247` → `frmAddUpdateSalesInvoice` · `250` → `frmSalesReturn` |
| زر «مردود بيع» داخل الفاتورة | `.cs:2531` | `btnSalesReturn_Click` يفتح `frmSalesReturn` كحوار — يصبح **مكرّراً** بعد الدمج |

## 3. القرارات

| # | القرار | السبب |
|---|---|---|
| `D-1` | **الشاشة الباقية `frmAddUpdateSalesInvoice`** والوضع يُختار بزرّي اختيار داخل `flpToolbar` (أول شريط الأوامر) | نصّ المالك: «choice one of them … set tow Modes by using two raio button» — والشريط الأول هو أوضح موضع دائم وأقلّه خطراً على الإرساء (`RULE-N125`) |
| `D-2` | **`frmSalesReturn` يُحذف** (`.cs` + `.Designer.cs` + `.resx`) بعد إعادة توجيه مدخليها | `RULE-N102`: شاشة لكيان له شاشة قائمة تُعدّ كوداً ميتاً — وبقاؤها = نسختان من حكم المرتجع (`RULE-N172`) |
| `D-3` | **`frmListReturns` يبقى قائماً** ويفتح الشاشة الموحّدة بوضع المرتجع | المالك طلب وحدة **المحرّر**؛ ودمج قائمتي البيع والمرتجع تغيير ثانٍ غير مطلوب (`RULE-N199` — أصغر تغيير يحقّق الطلب) |
| `D-4` | **`frmPurchaseReturn` لا يُمَس** | يقترن بـ`frmPurchaseInvoice` (شاشة شراء مختلفة) — طلب المالك يخصّ فاتورة البيع ومرتجعها |
| `D-5` | في وضع المرتجع **تُخفى حقول البيع** التي لا معنى لها: `lblCashBox`+`cmbCashBox` · `lblPaymentType`+`rbtnPaymentTypeCash`+`rbtnPaymentTypeCredit` · `lblPaid`+`txtPaid` · `lblExpensesLabel`+`txtExpenses` · `label1`+`numInvoiceDiscount`+`cmbDiscountType` · أزرار الطباعة (`btnPrintA4`/`btnPrintReceipt`/`chkPrintSummary`) | `CreateSalesReturnRequest` لا يحمل صندوقاً ولا نوع دفع ولا مدفوعاً ولا خصم ترويسة ولا نفقات — وإظهارها = واجهة تكذب على المستخدم (`RULE-N167`) |
| `D-6` | وضع المرتجع **لا يفحص كفاية المخزون** (`IsStockSufficientAsync` تُعيد `true` فوراً) | المرتجع **يزيد** المخزون؛ فحص البيع كان سيمنع إرجاع صنف رصيده صفر |
| `D-7` | المرتجع المحفوظ **للقراءة فقط** (لا `Update` في عقده) — يُعرض ويُرحَّل ولا يُحرَّر | نفس سلوك `frmSalesReturn` الحالي — لا واجهة تعدِ بوظيفة غير موجودة |
| `D-8` | `LEP-1` تُغلق في هذه الشاشة أيضاً: `int productUnitId = 1` في مسار الحفظ (`.cs:2703`) يُستبدل برفض عربي للسطر بلا وحدة | نفس عيب الصنف المقيس في `TASK-014` (4 مواضع / 3 ملفات) — وهذه الشاشة آخر موضع باقٍ |

## 4. مصفوفة الوضعين

| الجانب | فاتورة بيع | مرتجع بيع |
|---|---|---|
| `lblMode` / `Text` | «فاتورة بيع» / «فاتورة بيع - {InvoiceNo}» | «مرتجع بيع» / «مرتجع بيع - {ReturnNo}» |
| `lblInvoiceNo` | `InvoiceNo` | `ReturnNo` |
| الخدمة | `ISalesInvoiceDesktopService` | `ISalesReturnDesktopService` |
| الحفظ | `Create` ثم `Update` لمسودة محمَّلة | `Create` فقط (ثم `Post`)، و`_createdId` يمنع مستنداً ثانياً |
| الترحيل | «هل أنت متأكد من ترحيل الفاتورة؟ لا يمكن تعديلها بعد الترحيل.» | «ترحيل المرتجع؟ سيتم زيادة المخزون وتقليل رصيد العميل.» |
| الإلغاء | `_salesApi.CancelAsync` | `_returnApi.CancelAsync` |
| التحرير بعد الحفظ | مسودة ⇒ قابلة للتعديل (`UpdateAsync`) | **مقفلة دائماً** (`D-7`) |
| المخزون | فحص كفاية إلزامي | بلا فحص (`D-6`) |
| الحقول المخفية | — | القائمة في `D-5` |

## 5. النطاق (قائمة مغلقة)

| الموجة | الملف | المالك |
|---|---|---|
| 1 | `Screens/Sales/Forms/frmAddUpdateSalesInvoice.cs` + `.Designer.cs` + `.resx` | `ui-agent` |
| 1 | `Models/SalesDocumentMode.cs` **(جديد — 5 أسطر: enum بوضعين)** | `ui-agent` |
| 2 | `Screens/Returns/Forms/frmSalesReturn.cs` + `.Designer.cs` + `.resx` **(حذف)** | `ui-agent` |
| 2 | `Screens/Returns/Forms/frmListReturns.cs` (إعادة توجيه فرع المرتجع) | `ui-agent` |
| 2 | `Screens/frmMain.cs` (بند «مرتجع مبيعات» يفتح الوضع الصحيح) | `ui-agent` |
| 2 | `Program.cs` (إزالة تسجيل `frmSalesReturn`) | `ui-agent` |

**خارج النطاق:** `Business/**` · `DataAccess/**` · `Models/**` (باك-إند) · `DTO/**` · `Services/**` · `Migrations/**` · أي `Mock*` · `frmPurchaseReturn` · `frmPurchaseInvoice` · `ctrlLineEntryPanel` · `frmListSales` · `frmListReturns.Designer.cs` — الباك-إند مجمّد (`RULE-N177`) ولا عقد يتغيّر.

## 6. معيار القبول

1. فتح «فاتورة بيع» من القائمة ⇒ الوضع «فاتورة بيع» محدَّد، وكل سلوك اليوم كما هو حرفياً.
2. فتح «مرتجع مبيعات» من القائمة أو من `frmListReturns` ⇒ الوضع «مرتجع بيع» محدَّد، والعنوان وحقول الحالة وحقول البيع المخفية كما في `D-5`.
3. تبديل الوضع بأحد الزرّين على مستند فيه سطور ⇒ تأكيد عربي؛ الإلغاء يُعيد الاختيار السابق ولا يفقد سطراً.
4. مرتجع: عميل + مستودع + سطر واحد ⇒ «حفظ» يُنشئ مرتجعاً برقم، وإعادة المحاولة لا تُنشئ ثانياً (`_createdId`).
5. مرتجع: «ترحيل» ⇒ رسالة تأكيد نصّها في §4، ثم نجاح، فيزداد المخزون ويقلّ رصيد العميل.
6. فاتورة بيع: لا يتغيّر أي سلوك (حفظ/تعديل/ترحيل/إلغاء/طباعة/فحص مخزون).
7. `dotnet build .\SalesSystem.slnx` = **0 Errors** بعد كل موجة.
8. صفر `MessageBox` · صفر `ex.Message` · صفر `: 1` سقوط وحدة · صفر مرجع `Helpers` في المصمّم.

## 7. المخاطر

| # | الخطر | التخفيف |
|---|---|---|
| `R-1` | الشاشة 3,207 سطراً فيها مسار بيع حسّاس (مال/مخزون/قفل) | وكيل واحد يملك الشاشة كاملة، والتعليمات: **لا إعادة هيكلة لمسار البيع**، والفروق محصورة في نقاط الوضع |
| `R-2` | شجرة العمل مشتركة مع جلسة موازية | كل وكيل يلمس ملفاته المعلنة فقط؛ الالتزام بقائمة مغلقة |
| `R-3` | `frmSalesReturn` يُحذف قبل إعادة توجيه مدخليه ⇒ بناء أحمر | الحذف وإعادة التوجيه و`Program.cs` و`frmMain` **في موجة واحدة** لوكيل واحد |

## 8. الجولات

### الجولة 1 (2026-11-18) — آلة الوضعين داخل الشاشة

| البند | القياس |
|---|---|
| `Models/SalesDocumentMode.cs` **(جديد)** | **11** سطراً — `Sale = 1` · `SaleReturn = 2` |
| `Screens/Sales/Forms/frmAddUpdateSalesInvoice.cs` | 3,207 → **3,734** |
| `Screens/Sales/Forms/frmAddUpdateSalesInvoice.Designer.cs` | 1,465 → **1,499** |
| `.resx` | لم يُلمس |
| `btnSalesReturn` (الزر القديم الذي كان يفتح الشاشة المكرّرة) | **0** في الملفين (حُذف الزر ومعالجه ومكانه في `ActionButtons` والـ`using`) |
| زرّا الاختيار | `lblDocMode` + `rbtnModeSale` (محدَّد افتراضياً) + `rbtnModeReturn` — أول أبناء `flpToolbar` ⇒ أقصى اليمين في RTL، **معلنة في المصمّم** (`RULE-N145`) بألوان حرفية بصيغة `Convert.ToInt32` (`RULE-N204`) وصفر مرجع `Helpers` فيه |
| `_documentMode` · `_returnDoc` · `_createdReturnId` | 23 · 27 · 11 موضعاً |
| الحقول المخفية في وضع المرتجع | **15** عنصراً في قائمة واحدة `SaleOnlyControls` (`D-5`) |
| `LEP-1` | `int productUnitId = 1` أُزيل من مسار الحفظ ⇒ `ResolveLineProductUnitId` + `_ValidateLineUnits` ورسالة «السطر {n}: لم تُحدَّد وحدة الصنف — أعد اختياره من لوحة الإضافة.» — **هذه الشاشة كانت آخر موضع مقيس للعيب** |
| البناء | `dotnet build .\SalesSystem.slnx` = **0 Errors** |

### الجولة 2 (2026-11-18) — شاشة واحدة فقط: حذف `frmSalesReturn` وإعادة التوجيه

| الملف | الإجراء |
|---|---|
| `Screens/Returns/Forms/frmSalesReturn.cs` · `.Designer.cs` · `.resx` | **حُذفت** (753 + 595 سطراً) — `RULE-N102`: لا يبقى محرّر ثانٍ لكيان له محرّر |
| `Screens/Returns/Forms/frmListReturns.cs` | `_OpenSalesEditor` → `frmAddUpdateSalesInvoice` + `LoadAsReturn(dto)` |
| `Screens/frmMain.cs` | `OpenSalesDocument(mode)`: تُنشّط النسخة المفتوحة وتبدأ مستنداً جديداً (`StartNewDocument`) بدل نافذة ثانية (`RULE-N133`) · بند «مرتجع مبيعات» → `SaleReturn` · «فاتورة بيع» بقي `OpenScreen<T>` حرفياً (بلا مسح فاتورة مفتوحة) |
| `Program.cs` | حُذف تسجيل `frmSalesReturn` — بقي `frmAddUpdateSalesInvoice` (تسجيل واحد) |
| `frmAddUpdateSalesInvoice.cs` | **دالة عامة واحدة** أُضيفت: `StartNewDocument(SalesDocumentMode)` |
| `frmPurchaseReturn.cs` | تعليق واحد فقط (كان يسمّي الشاشة المحذوفة) |
| مراجع `frmSalesReturn` في الحل كله | **0** · الملفات على القرص = **0** |
| البناء | **0 Errors** |

**ملاحظة قيد مقيسة (يُبلّغ بها المراجع):** فتح مرتجع من `frmListReturns` بينما شاشة المستند **مفتوحة مسبقاً** يُنشّط النسخة القائمة (نمط `clsGlobal._OpenForm` القائم قبل الموجة) — إصلاحها يحتاج مسار «اربط على نموذج مفتوح» عامّاً، وهو خارج الدالة الواحدة المسموح بها في هذه الموجة. مسار القائمة الرئيسية آمن لأنه يستعمل `StartNewDocument` المكتفية بذاتها.

### الجولة 3 (2026-11-18) — إغلاق ملاحظتَي المراجعة (Major × 2)

**حكم المراجعة الأول: `0 Blocker · 2 Major · 2 Minor`** — الملاحظتان الجوهريتان أُغلقتا، والباقي مسجَّل.

| # | الشدة | الملاحظة | الإغلاق |
|---|---|---|---|
| 1 | **Major** | `frmMain.OpenSalesDocument` كان يستدعي `StartNewDocument` على نافذة مفتوحة **بلا تأكيد** ⇒ فقدان صامت لسطور غير محفوظة | ✅ `public bool TryStartNewDocument(SalesDocumentMode)` في الشاشة (تأكيد `clsDialogs.Confirm` عند وجود مستند أو سطور؛ `false` عند الرفض) · `frmMain` يستدعيها وتُنشّط النافذة فقط عند الرفض |
| 2 | **Major** | ميزة **«اختيار فاتورة»** (`frmSalesInvoiceSelection`) سقطت مع حذف `frmSalesReturn` | ✅ `btnPickInvoice` معلن في المصمّم داخل `flpToolbar` (بعد `btnNew`)، ظاهر في **وضع المرتجع فقط**، مع تأكيد الاستبدال + `LoadAllUnitsForLinesAsync()` قبل الحفظ (لا معامل تحويل 1 خطأً) |
| 3 | Minor | تعديل تعليق في `frmPurchaseReturn.cs` (خارج النطاق) | 📌 انحراف موثَّق — الملف نفسه **حُذف** في `TASK-018` فسقط الموضوع |
| 4 | Minor | شريط الإجماليات في وضع المرتجع يعرض «المتبقي» = كامل مبلغ المرتجع | 📌 بانتظار قرار المالك: إخفاء/إعادة تسمية «المتبقي» في وضع المرتجع |

**القياسات بعد الإغلاق:** `frmAddUpdateSalesInvoice.cs` 3,734 → **3,829** · المصمّم 1,499 → **1,507** · `frmMain.cs` 394 → **401** · `TryStartNewDocument` = 3 مواضع · `btnPickInvoice` = 23 موضعاً · `MessageBox`/`ex.Message` = **0** · البناء **0 Errors**.

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)
