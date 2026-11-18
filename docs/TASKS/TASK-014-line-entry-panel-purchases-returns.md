# TASK-014 — صندوق «إضافة صنف» فوق الشبكة في المشتريات والمرتجعات بنمط `frmAddUpdateSalesInvoice`

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-014` |
| `النوع` | واجهة (UX إدخال) — بلا مساس بالباك-إند |
| `الحجم` | L — ثلاثة محررات مستندات + بيت مشترك واحد (`RULE-N155`/`RULE-N156`) |
| `المرحلة` | `—` (لا مسار بيانات جديد: العقود قائمة، والشاشات الثلاث موجودة ومراجَعة) |
| `الحالة` | 🔄 مفتوحة |
| `التاريخ` | `2026-11-18` |

## 1. طلب المالك (نصّه)

> «all Purchases and sales and returns I want to be like frmAddUpdateSalesInvoice at UX for enter Products data row useing keyboard»
>
> ثم التوضيح الحاسم: **«I mean group box of add product above DGV like it»** — أي **صندوق «إضافة صنف» أعلى الشبكة** (باركود · كود · اسم · وحدة · كمية · سعر · خصم · إجمالي السطر) يُدخل الصنف بلوحة المفاتيح ويُضيفه سطراً في الشبكة، ثم يعود المؤشر إلى الباركود للصنف التالي.

## 2. النمط المرجعي — مقيس من `frmAddUpdateSalesInvoice` (لا من الذاكرة)

| العنصر | القياس | الموضع |
|---|---|---|
| `gbEntry` («إضافة صنف») | `Dock = Top` · داخلـه `pnlEntry` (`FlowLayoutPanel`, `Dock = Fill`) | `frmAddUpdateSalesInvoice.Designer.cs` L801–835 |
| عناصر اللوحة (يمين→يسار) | `lblBarcodeLabel`+`txtBarcode` · `lblItemCodeLabel`+`txtItemCode` · `lblProductLabel`+`lblProductName` · `lblUnitLabel`+`cmbUnit` · `lblQtyLabel`+`txtQuantity` · `lblPriceLabel`+`txtUnitPrice` · `lblDiscountLabel`+`txtLineDiscount` · `lblLineExpiryLabel`+`dtpLineExpiry` · `lblTotalHeaderLabel`+`lblLineTotal` · `lblAvailable` | نفس المصدر |
| سلسلة `Enter` | باركود ⇒ (وحدة) ⇒ كمية ⇒ سعر ⇒ خصم ⇒ (انتهاء) ⇒ **إضافة السطر** | `.cs` L137–160 · L883–975 |
| إضافة/تحديث السطر | `AddOrUpdateRowAsync` — تحقق ⇒ قصّ الخصم على `الكمية×السعر` ⇒ **دمج كمية** لسطر بنفس (الصنف + الوحدة) ⇒ إضافة/تحديث ⇒ `ResetEntryState()` ⇒ `CalculateTotals()` ⇒ **`txtBarcode.Focus()`** | `.cs` L1162–1272 |
| الإلغاء | `CancelEdit()` يستعيد السطر الأصلي ثم يعيد المؤشر للباركود | `.cs` L1274–1298 |
| الباركود | `Enter` ⇒ بحث · فارغ ⇒ منتقي · `F4` ⇒ منتقي · `Esc` ⇒ إلغاء | `.cs` L707–780 |
| تعديل سطر | نقر مزدوج على الشبكة يحمّل السطر في اللوحة (`_editingLineKey`) | `.cs` L2011–2018 · L2201 |

## 3. الحالة المقيسة للشاشات الثلاث (قبل)

| الشاشة | `.cs` | `.Designer.cs` | صندوق الإدخال | إدخال لوحة المفاتيح |
|---|---|---|---|---|
| `frmPurchaseInvoice` | 680 | 721 | `gbEntry`+`pnlEntry` يحويان **زرّي «إضافة/حذف صنف» فقط** (لا حقول) | سطر فارغ يُضاف بالزر؛ الشفرة تُحلّ على `CellEndEdit` فقط |
| `frmSalesReturnMaster` | 1,173 | 1,126 | `pnlLineTools` (زرّان + الإجمالي) | `colBarcode` + `CellEndEdit` فقط |
| `frmPurchaseReturnMaster` | 993 | 1,147 | `flpLineBar` (زرّان + الإجمالي) | `colCode` + `CellEndEdit` فقط |
| **المرجع** `frmAddUpdateSalesInvoice` | 2,769 | 1,453 | `gbEntry` بعشرة حقول/تسميات | سلسلة `Enter` كاملة |

## 4. القرارات

| # | القرار | السبب |
|---|---|---|
| `D-1` | **الثلاث شاشات في موجة واحدة** (وكيل لكل شاشة) | طلب المالك الصريح «all Purchases and sales and returns»؛ النمط **مُثبت مسبقاً** في الشاشة المرجعية وشاشته هو `frmAddUpdateSalesInvoice` نفسه — فليس تعميماً لسلوك جديد غير مشهود (`RULE-N184` محقَّقة بالمرجع) |
| `D-2` | **بيت واحد جديد: `Controls/Common/ctrlLineEntryPanel` (`UserControl`)** — يصمّم عناصره في `*.Designer.cs` ويحمل سلوكه في `*.cs`، والثلاث شاشات **تُسقطه في مصمّمها وتُمرّر له مُفوَّضاتها** ولا تكتب سطر إدخال واحداً | `RULE-N169`…`RULE-N172` + **أمر المالك 2026-11-18:** «as UserControl like `Screens/Customers/Controls/`» و«write code at one Place» — فالمكان الواحد هو المكوّن لا Helper منفصل + مكوّن (حُذف `Helpers/LineEntryPanelHelper.cs` الذي كُتب في موجة 1 قبل هذا الأمر) |
| `D-3` | **اللاعبون يعلنون عناصرهم في `*.Designer.cs`** والـHelper يتبنّاها بالمرجع لا يُنشئها | `RULE-N145` + `RULE-N134`/`RULE-N204` (المصمّم = التشغيل) |
| `D-4` | **`frmAddUpdateSalesInvoice` يستهلك المكوّن — مؤجل إلى موجة مستقلة** (أمر المالك 2026-11-18: «sales and returns screen are the same ... all components are the same make it them as UserControl reuse the same code and write code at one Place»). **المراجعة أثبتت أنه لم يُحوَّل بعد** (`AddOrUpdateRowAsync` ما زال ×3 داخله، وصفر مرجع لـ`ctrlLineEntryPanel`) ⇒ فهذا **دَين معلن** لا إنجاز: النسخة الثانية من الحكم المالي موجودة الآن في الشاشة المرجعية وحدها (`RULE-N172`) | حماية شاشة الفاتورة الحيّة (2,769 سطراً) من إعادة كتابة متزامنة مع جلسة موازية كانت تعدّلها؛ والتحويل يستهلك `ShowExpiry` + `BeforeCommit` + `lblAvailable` — بند مفتوح في §8 |
| `D-4b` | **باقي المكوّنات المشتركة (الإجماليات · الترويسة · إعداد الشبكة) تُستخرج في موجات لاحقة مكوّناً مكوّناً** بعد شهادة المالك لهذه الموجة | `RULE-N184` + `RULE-N113`: استخراج أربعة مكوّنات دفعة واحدة على شاشة مالية حيّة (2,769 سطراً) خطر بلا مكسب فوري |
| `D-5` | **إصلاح مقيس مضمَّن:** `l.ProductUnitId > 0 ? l.ProductUnitId : 1` في حفظ فاتورة الشراء (L642/L645) — سقوط صامت إلى الوحدة رقم 1 لمستودع مشترك | الوحدة تُملأ الآن من اللوحة فعلياً؛ والمتبقي يُرفض برسالة عربية بدل وحدة خاطئة |
| `D-6` | لوحة الإدخال في المرتجعين **مفعّلة في وضع الإضافة فقط** (`EntryMode.Adding`) — `D-05` في `F-004` يمنع تعديل سطور مستند محمَّل (لا `UpdateAsync` في العقد) | لا واجهة تكذب على المستخدم |

## 5. النطاق (قائمة مغلقة)

| الملف | المالك |
|---|---|
| `SalesSystem.Desktop/Controls/Common/ctrlLineEntryPanel.cs` + `.Designer.cs` + `.resx` **(جديد — البيت الواحد)** | `ui-agent` — ✅ موجة 2 (769 / 442 / 120 سطراً) |
| `Screens/Purchases/Forms/frmPurchaseInvoice.cs` + `.Designer.cs` | `ui-agent` — ✅ موجة 4A (751 / 723) |
| `Screens/Returns/Forms/frmSalesReturn.cs` + `.Designer.cs` | `ui-agent` — ✅ موجة 5A (749 / 595) |
| `Screens/Returns/Forms/frmPurchaseReturn.cs` + `.Designer.cs` | `ui-agent` — ✅ موجة 5B (728 / 580) |

> **ملاحظة نطاق (مقيسة 2026-11-18):** `frmSalesReturn` / `frmPurchaseReturn` هما الشاشتان المفصولتان (قرار المالك `F-004/D-10` بإلغاء الدمج إلى `frm*Master`) — أُعيدتا في جلسة متوازية ثم استهلكتا المكوّن هنا. و`frmAddUpdateSalesInvoice` لم يُحوَّل بعد إلى المكوّن (يبقى بلوحته الداخلية) — بند مؤجل مذكور أدناه.

**خارج النطاق:** `frmAddUpdateSalesInvoice` · `frmOpeningStock` · `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Program.cs` · أي `Mock*` · أي ترحيل — الباك-إند مجمّد (`RULE-N177`) ولا مسار بيانات جديد.

## 6. معيار القبول

1. في الشاشات الثلاث: صندوق «إضافة صنف» أعلى الشبكة بعشرة عناصر (بلا حقل انتهاء في الشاشات التي لا تعرفه).
2. باركود + `Enter` ⇒ يُعبَّأ الاسم/الوحدة/السعر (تكلفة للشراء، بيع للمرتجع) ويُنتقل إلى الكمية.
3. سلسلة `Enter`: كمية ⇒ سعر ⇒ خصم ⇒ **إضافة السطر** ⇒ اللوحة تُفرَّغ ⇒ **المؤشر على الباركود** جاهزاً للصنف التالي.
4. `F4` في الباركود = منتقي الأصناف · باركود غير موجود ⇒ يُفتح المنتقي (نمط المرجع).
5. تكرار (نفس الصنف + نفس الوحدة) = **دمج كمية** في السطر القائم لا سطر جديد.
6. نقر مزدوج على سطر = تحميله في اللوحة («تحديث السطر»)، `Esc` يُلغي ويستعيد.
7. `Enter` داخل الشبكة **لا يحفظ ولا يُرحّل** (`RULE-N153`) — يبقى `_PrimaryActionButton()` = `null` عند التركيز على الشبكة.
8. أعذار عربية بلا استثناءات للمستخدم؛ `MessageBox` في الشاشات = 0؛ `ex.Message` = 0.
9. المصمّم = التشغيل: ألوان لونية حرفية بصيغة `FromArgb(Convert.ToInt32(…))`، صفر مرجع `Helpers` داخل المصمّم.
10. `dotnet build .\SalesSystem.slnx` = **0 Errors**.

## 7. الأدلة (تُقاس بأمر في نفس التشغيل)

| البند | الأمر | النتيجة |
|---|---|---|
| البناء | `dotnet build .\SalesSystem.slnx` | — |
| صندوق الإدخال في 3 شاشات | `Select-String -Pattern "gbEntry|txtBarcode"` | — |
| نسخ مكرّرة من الحكم | `Select-String "AddOrUpdateRowAsync" Screens/**` | — |
| `MessageBox` / `ex.Message` | `Select-String` | — |

## 8. الجولات

### الجولة 1 (2026-11-18) — البيت الواحد ثم تحويله إلى `UserControl`

| الموجة | الوكيل | المُخرَج |
|---|---|---|
| 1 | `ui-agent` | `Helpers/LineEntryPanelHelper.cs` (660 سطراً) — **حُذف** في الموجة 2 بأمر المالك «write code at one Place» |
| 2 | `ui-agent` | `Controls/Common/ctrlLineEntryPanel.cs` + `.Designer.cs` + `.resx` — المكوّن يحمل الشكل والسلوك معاً؛ صفر مرجع لـ`LineEntryPanelHelper` |
| 3a | `ui-agent` | إضافة `ShowExpiry` + `BeforeCommit` (تكافؤ مع لوحة `frmAddUpdateSalesInvoice`) |
| 3b | `ui-agent` | **قاعدة المالك:** إعادة إدخال نفس المنتج ⇒ **الكمية +1** فوراً بلا سطر ثانٍ ولا رسالة (`TryIncrementExisting`)، و`CommittedLine` لحمل السطر في الحدث |
| 3c | `ui-agent` | إصلاح 3 أخطاء بناء في المكوّن (`DateTimePicker.DroppedDown` غير موجود · `WFO1000` على `PriceCaption`) ⇒ بناء **0 Errors** |

### الجولة 2 — الاستهلاك في ثلاث شاشات

| الشاشة | قبل | بعد | الأدلة (`Select-String`) |
|---|---|---|---|
| `frmPurchaseInvoice` | 781 / 755 | **751 / 723** | `btnAddItem`=0 · `gbEntry|pnlEntry`=0 · `ProductUnitId > 0 ?`=0 · `MessageBox`=0 · `.PurchasePrice`=0 |
| `frmSalesReturn` | 618 / 618 | **749 / 595** | نفس القائمة + `using SalesSystem.DTO.Responses` (كان خطأ بناء `CS0246`) |
| `frmPurchaseReturn` | 618 / 619 | **728 / 580** | نفس القائمة + حقل المصمّم `ctrlLineEntry` معلن مرة واحدة |

**البناء:** `dotnet build .\SalesSystem.slnx` = **0 Errors** بعد كل موجة (آخر تشغيل بعد إصلاحات البناء).

**عيب مقيس أُغلق في الموجة (`LEP-1`):** `ProductUnitId > 0 ? x : 1` في **4 مواضع / 3 ملفات** (`frmPurchaseInvoice` 642+645 · `frmSalesReturn` 538 · `frmPurchaseReturn` 536) كانت تُرحّل الوحدة رقم 1 لصنف آخر صامتة ⇒ صارت `l.ProductUnitId` + رفض عربي مسمّى بالصف. العدّ الآن = **0**.

**عيب مقيس آخر (مُبلَّغ للمالك، لم يُصلَح في هذه الموجة):** `ProductDto.SalePrice` و`ProductDto.PurchasePrice` معرفان كـ`=> 0m` في `DTO/ProductDtos.cs` (سِمتان للتوافق الرجعي) — فأي شاشة تقرأ منهما تُملأ بصفر. المستهلكان: `frmPurchaseInvoice` (أُصلح هنا) و`frmAddUpdateSalesInvoice:1704` (**باقٍ — بند مفتوح**).

### المتبقي (مسجَّل كي لا يُنسى)

1. **`frmAddUpdateSalesInvoice` لم يستهلك المكوّن بعد** — لوحته الداخلية (~250 سطراً) هي النسخة الثانية من نفس السلوك (`RULE-N172`). ترحيلها يستهلك `ShowExpiry` + `BeforeCommit` (فحص المخزون) + `lblAvailable`. **حجم L** ونطاقه حساس (2,769 سطراً) — موجة مستقلة بعد شهادة المالك.
2. **`frmListReturns` / `frmListPurchases`** مقابل `frmListSales` — لم تُراجَع في هذه الموجة.
3. **`ProductDto.SalePrice/PurchasePrice`** — إما تُربَط بالسعر الحقيقي أو تُزال من الـDTO؛ لا تستهلكها شاشة جديدة.

### الجولة 3 — المراجعة (`code-reviewer`، pass واحد)

**الحكم الأول: `1 Blocker · 3 Major · 5 Minor` ⇒ REJECT** — كلها أُغلقت أو سُجّلت:

| # | الخطورة | الملاحظة | الإغلاق |
|---|---|---|---|
| 1 | **Blocker** | لوحة الشراء تكتب `UnitPrice` فقط ومسار الحفظ يُرسل `UnitCost` ويرفض `<= 0` ⇒ **لا يمكن حفظ فاتورة شراء واحدة** | ✅ `ctrlLineEntry.LineCommitted +=` يُزامن `UnitCost = UnitPrice` (يغطي الإضافة · التعديل · الدمج · المسح السريع `+1`) — `frmPurchaseInvoice.cs:79-85` |
| 2 | Major | تعديل سطر مسودة يحدّث `UnitPrice` ويُبقي `UnitCost` القديم ⇒ سعر قديم يُرحّل صامتاً | ✅ نفس المشترك يغطي مسار التعديل (`NotifyCommitted` يُطلق الحدث بعد النسخ) |
| 3 | Major | شجرة العمل تحمل موجة `CashBoxes` غير مكتملة لجلسة موازية (`frmCashBoxTransactions` محذوف و`frmListCashBoxTransactions` غير مربوط) | ⚠️ **خارج هذه المهمة** — لا وكيل لمسها؛ الـOrchestrator يودع شريحته وحدها ويبلّغ المالك |
| 4 | Major | `D-4` يدّعي تحويل `frmAddUpdateSalesInvoice` إلى المكوّن وهو لم يحدث | ✅ `D-4` أُعيدت كتابته كـ**دَين معلن** + بند مفتوح في §8 |
| 5 | Minor | في وضع التعديل: باركود مكتوب بلا `Enter` يُحفظ بجانب `ProductId` آخر | ✅ حارس `barcodeChanged` + `ResolveTypedBarcodeAsync` ورسالة `الباركود المكتوب غير مسجّل — امسح الباركود ثم اضغط Enter لتأكيد الصنف.` (`ctrlLineEntryPanel.cs`) |
| 6 | Minor | عمود «الوحدة» في المرتجع فارغ لأن `SalesReturnItemDto` بلا `UnitName` | ✅ `colUnit.Visible = false` + تعليق السبب (`RULE-N177`) — يُكشف بعد إضافة الاسم للـDTO |
| 7 | Minor | `clsDialogs.Error(result.Error)` بلا بديل عربي في مرتجع الشراء (والشقيقة مُصلَحة) | ✅ `result.Error ?? "تعذر حفظ مرتجع الشراء."` |
| 8 | Minor | §5 في السجل كان يسمّي المسترين المحذوفين | ✅ صُحّح إلى `frmSalesReturn` / `frmPurchaseReturn` |
| 9 | Minor | `dgvItems.SelectionMode = CellSelect` في 3 شبكات خارج الاستثناء الموثّق `RULE-N22a` | 📌 **قرار معلن:** الشبكات الثلاث صارت **عرض/اختيار فقط** (`ReadOnly = true`, `AllowUserToAddRows = false`) و`CellSelect` هنا لا يفتح أي تحرير — فهي **ليست** استثناءً على `RULE-N22a` (لا إدخال داخل الخلية). تُترك `CellSelect` ليتوافق مؤشر الاختيار مع نمط المرجع |
| 10 | Minor | `Models/InvoiceLineItem.cs` فيه تعليقات XML مشوّشة (دَين سابق) | 📌 يُغلق عند أول مساس بالملف (تنظيف أول لمس) |

**ملاحظات المنهج من المراجعة (مسجَّلة كما هي):** بعض ملفات الشريحة كانت قد سُحبت إلى كومِت جلسة موازية (`db2721f`) قبل المراجعة، فالبند 3 «خارج النطاق» يخصّ تلك الجلسة لا هذه الموجة؛ والتحقق من الترجمة والتشغيل يبقى خارج قدرة المراجع (قراءة فقط) — والبناء نفّذه الـOrchestrator.

**البناء النهائي:** `dotnet build .\SalesSystem.slnx` = **0 Errors** بعد إغلاق الحاجز.

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)
