# TASK-030 — باركود الوحدة الرئيسية من شاشة الصنف لا يصل إلى `ProductUnits` عند التعديل

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-030` |
| `العنوان` | `frmCRUDCProductMaster` — `txtMainUnitBarcode` يُحفظ على `Products.Barcode` فقط عند التعديل، فلا يظهر في `frmCRUDCProductUnitMaster` |
| `الحجم` | **S** (ملف واحد · تغيير موضع تمرير وسيط · لا عقد ولا هيكل) |
| `المرحلة` | `—` (إصلاح عيب على مسار قائم — لا شاشة ولا كيان جديد) |
| `الحالة` | **منفَّذة** — بناء 0 أخطاء · مراجعة واحدة (0 Blocker · 0 Major · 1 Minor مُغلق) · بانتظار اختبار المالك |
| `الوكيل المالك` | `ui-agent` (W1) · `code-reviewer` (مراجعة واحدة) |
| `أثر قاعدة البيانات` | **لا شيء** — العمودان `Products.Barcode varchar(50)` و`ProductUnits.Barcode nvarchar(50)` قائمان؛ لا ترحيل ولا `DROP`/`ALTER` (`RULE-N198`) |
| `التاريخ` | `2026-11-20` |

## الهدف

بعد هذه المهمة: الباركود المكتوب في `txtMainUnitBarcode` داخل `frmCRUDCProductMaster` — عند الإضافة **وعند التعديل** — يُحفظ على **وحدة الصنف الأساسية** (`ProductUnits.Barcode`) بنفس القيمة المحفوظة على الصنف (`Products.Barcode`)، فيظهر في شبكة `frmCRUDCProductUnitMaster` بعد إعادة الفتح.

## العيب المقيس (لا منقول)

**البلاغ:** «when I save the Barcode [it] do not show in frmCRUDCProductUnitMaster».

| # | الموضع | القياس |
|---|---|---|
| 1 | `frmCRUDCProductMaster.cs:976-978` | يقرأ `txtMainUnitBarcode` إلى `barcode` ✅ |
| 2 | `frmCRUDCProductMaster.cs:992-1005` | يمرّرها إلى `UpdateProductRequest` ⇒ `Products.Barcode` ✅ |
| 3 | `frmCRUDCProductMaster.cs:1010` | ثم ينادي `_SaveMainUnitPricesAsync(id)` — **بلا** تمرير الباركود ❌ |
| 4 | `frmCRUDCProductMaster.cs:626` | `Barcode: unit.Barcode` — القيمة **المحمَّلة سابقاً** من القاعدة، لا ما كتبه المستخدم ❌ |
| 5 | `frmCRUDCProductMaster.cs:573-593` (الإضافة) | `_CreateMainUnitAsync` يمرّر الباركود المكتوب إلى `AddProductUnitRequest` ✅ — لذلك العيب **لا يظهر عند الإضافة** |
| 6 | `ProductUnitService.cs:57,98` | الباك-إند يمرّر `req.Barcode` إلى `UpdatePricing` ✅ (لا عيب في BLL) |
| 7 | `frmCRUDCProductUnitMaster.cs:239,261,319` | الشبكة تقرأ `ProductUnitDto.Barcode` وتعرض عمود «الباركود» ✅ (لا عيب في شاشة الوحدات) |

**النتيجة:** الباركود يُكتب في **موضعين** (`Products.Barcode` · `ProductUnits.Barcode`)؛ مسار الإضافة يكتبهما معاً، ومسار التعديل يكتب **الأول فقط** ⇒ تتباعد القيمتان ويعرض المصدران قيمتين مختلفتين.

**سلطة المخطط (`docs/database-schema.md` §9 ص6-7):** «`Products.Barcode` عمود `varchar` للبحث السريع؛ **المصدر الرسمي للباركود هو `ProductUnits.Barcode`**» — فالإصلاح يوجّه الباركود إلى الوحدة، وهو الاتجاه الذي قرّره المالك لاحقاً بإزالة باركود الصنف نهائياً (`TASK-031`).

**الصنف لا الحالة (§4 خطوة 9b) — كل مسارات كتابة الباركود في الحل:**

| # | المسار | `Products.Barcode` | `ProductUnits.Barcode` | الحكم |
|---|---|---|---|---|
| 1 | `frmCRUDCProductMaster` — إضافة | ✅ | ✅ | سليم |
| 2 | `frmCRUDCProductMaster` — تعديل | ✅ | ❌ | **العيب المُبلَّغ** |
| 3 | `frmImportProductsExcel` | ✅ (س540) | ✅ (س557) | سليم |
| 4 | `frmCRUDCProductUnitMaster` — إضافة/تعديل وحدة | ❌ | ✅ | **انعكاس الصنف نفسه** — يُسجَّل ولا يُمسّ في هذه الموجة (`§4` 9c) |

## خارج النطاق (ممنوع بالاسم)

- `SalesSystem.Business/**` · `SalesSystem.Models/**` · `SalesSystem.DataAccess/**` (و`Configurations/**` · `Migrations/**` · `Seeders/**`) — **الباك-إند مقفل**: `IProductDesktopService` حكمه في `docs/MOCK-COVERAGE.md` **`مؤهل جزئياً — تكافؤ الرفض ناقص`** بلا موافقة مالك مكتوبة (`RULE-N177`/`RULE-N178`) · ولا تغيير هيكل (`RULE-N198`)
- `SalesSystem.Desktop/Services/**` (Interface · Direct · Mock) — لا عيب فيها (مقيس)
- `frmCRUDCProductUnitMaster.cs` + مصمّمه — لا عيب فيه؛ مسار الوحدة ← الصنف **انعكاس** مسجَّل لا يُعالَج هنا
- `frmImportProductsExcel.cs` — سليم
- `frmCRUDCProductMaster.Designer.cs` · `.resx` — لا عنصر ولا لون
- `Program.cs` · `appsettings.json` · أي `.csproj` · `SalesSystem.Tests/**`

## الملفات (قائمة مغلقة)

| # | الملف | العمل |
|---|---|---|
| 1 | `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmCRUDCProductMaster.cs` | تمرير الباركود المكتوب إلى `_SaveMainUnitPricesAsync` واستخدامه في `UpdateProductUnitRequest` بدل `unit.Barcode` المسبق |

## العقود

لا تغيير. `UpdateProductUnitRequest(short UnitId, string? Barcode, …)` يحمل الباركود أصلاً.

## البيت الواحد

لا سلوك مشترك جديد ولا نسخة ثانية (`RULE-N169`…`RULE-N172`). التعديل تمرير قيمة قائمة من `_SaveProductAsync` إلى دالة مساعدة **محلية** في الشاشة نفسها.

## الموجات

| الموجة | الوكيل | الملفات | البناء |
|---|---|---|---|
| W1 | `ui-agent` | 1 | ✅ **0 Errors / 112 Warning(s)** (`--no-incremental` — تشغيل كامل) |
| W1-M | `ui-agent` (نفس الجلسة) | 1 | ✅ **0 Errors / 112 Warning(s)** (إغلاق Minor المراجعة: ذكر الباركود في رسالتي التحذير) |

**حجم الفرق:** `git diff --numstat` = **10 مضاف / 8 محذوف** في ملف واحد.

## الأدلة (إلزامي — `docs/GOVERNANCE/EVIDENCE.md`)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| الشاشتان سليمتان (ليستا صفر بايت) | `frmCRUDCProductMaster.cs` 1,346 سطراً · `frmCRUDCProductUnitMaster.cs` 1,072 سطراً | `(Get-Content <file>).Count` | ✅ |
| مواضع قراءة `txtMainUnitBarcode` | 6 (549 · 573-575 · 815 · 878 · 976-978 · 1311) | `Select-String -Pattern "MainUnitBarcode"` | ✅ |
| مستدعو `_SaveMainUnitPricesAsync` | **1** (`:1010`) | `Select-String -Pattern "_SaveMainUnitPricesAsync"` | ✅ |
| العقد والباك-إند | `UpdatePricing` يضبط `Barcode` · `MapToDto` يعيده | `ProductUnitService.cs:98,222` + `ProductUnit.cs:165` | ✅ لا عيب |
| شاشة الوحدات تعرض الباركود | عمود `Barcode` + `"الباركود"` | `frmCRUDCProductUnitMaster.cs:239,261,319` | ✅ |
| حكم تغطية الوحدة (قفل الباك-إند) | `مؤهل جزئياً — تكافؤ الرفض ناقص` (تكرار الباركود) | `docs/MOCK-COVERAGE.md` صف 19 | الباك-إند **مقفل** |
| `unit.Barcode` داخل `_SaveMainUnitPricesAsync` بعد الإصلاح | **0** | `Select-String -Pattern "unit.Barcode"` | ✅ |
| مستدعو `_SaveMainUnitPricesAsync` بعد الإصلاح | **2** (تعريف ثنائي `:619` + استدعاء ثنائي `:1012`) | `Select-String -Pattern "_SaveMainUnitPricesAsync"` | ✅ صفر استدعاء بوسيط واحد |
| رسائل تحذير تذكر الباركود | **2** (`642` · `648`) | `Select-String -Pattern "باركود/سعر"` | ✅ وصفر رسالة «سعر/تكلفة/خصم» بلا «باركود» |
| `Cells["…"]` · `MessageBox.Show` في الشاشة | **0** / **0** | `Select-String` | ✅ (`RULE-N179` · `clsDialogs`) |

## المراجعة

`code-reviewer` (مراجعة واحدة على الشريحة): **0 Blocker · 0 Major · 1 Minor** — و**أُغلق Minor** في الموجة نفسها:

| # | الشدة | العيب | الإغلاق |
|---|---|---|---|
| 1 | Minor | رسالتا التحذير في `_SaveMainUnitPricesAsync` تقولان «تعذر حفظ سعر/تكلفة/خصم» ولا تذكران الباركود بعد أن صارت الدالة تحفظه ⇒ تحذير مضلِّل عند رفض باركود مكرَّر | أُضيف «باركود» إلى الرسالتين بنفس الصياغة (`642` · `648`) — ملف واحد · صفر تغيير سلوك |

**ما أثبته المراجع بالأمر:** مسار واحد للباركود (لا مسار ثالث يُسقطه) · `Barcode` المرسل إلى `UpdateProductUnitRequest` هو نفسه المرسل إلى `UpdateProductRequest` بنيوياً (متغيّر واحد) · الـdiff ملف واحد فقط · صفر مسّ `Business/**`/`DataAccess/**`/`Models/**`/`Services/**` (احترام `RULE-N177` — والحل كان ممكناً في الشاشة لأن العقد يحمل الباركود أصلاً) · صفر `catch` جديد/محذوف (`RULE-N205`) · الـMock يطبّق نفس دلالة `UpdatePricing` (صفر انحراف جديد).

## الاختبار اليدوي

> **في الوضع الحقيقي (`UseMockData = false`).**

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmCRUDCProductMaster` | «جديد» ← اسم + مجموعة + وحدة رئيسية + باركود «111222» + سعر ← حفظ ثم «الوحدات والأسعار» | الوحدة الأساسية تحمل الباركود «111222» في العمود |
| 2 | `frmCRUDCProductMaster` | اختيار صنف ← وضع التعديل ← تغيير الباركود إلى «333444» ← حفظ | رسالة نجاح بلا تحذير |
| 3 | `frmCRUDCProductUnitMaster` (من زر «الوحدات والأسعار») | فتح الشاشة | **الباركود «333444» ظاهر** — هذا هو العيب المُبلَّغ |
| 4 | `frmCRUDCProductMaster` | إعادة فتح الشاشة واختيار الصنف | `txtMainUnitBarcode` يعرض «333444» (الصنف والوحدة متطابقان) |
| 5 | `frmCRUDCProductMaster` | تعديل **السعر فقط** وترك الباركود ← حفظ ← فتح شاشة الوحدات | الباركود يبقى كما هو (لا يُمسح) |
| 6 | `frmCRUDCProductMaster` | مسح الباركود بالكامل ← حفظ ← فتح شاشة الوحدات | عمود الباركود فارغ (تفريغ مقصود) |
| 7 | `frmCRUDCProductMaster` | كتابة باركود مستخدم لوحدة صنف آخر ← حفظ | تحذير عربي: «تم حفظ بيانات الصنف، لكن تعذر حفظ **باركود**/سعر/تكلفة/خصم الوحدة الرئيسية» + سبب الرفض · **ولا يُنشأ تكرار** (`IX_ProductUnits_Barcode`) · وباركود الصنف يبقى محفوظاً (خطر مقبول `D-2`) |
| 8 | `frmCRUDCProductUnitMaster` | تعديل باركود الوحدة الأساسية من شاشة الوحدات ← إعادة فتح شاشة الصنف | **انعكاس مسجَّل (غير مُصلَح):** شاشة الصنف تعرض باركود الصنف القديم — يُسجَّل ولا يُعدّ عيباً في هذه الموجة |

## القرارات والمخاطر

- **D-1:** الشاشة تكتب الباركود في موضعين (الصنف + الوحدة الأساسية)؛ الإضافة تفعل ذلك، والتعديل لا. الإصلاح = تمرير القيمة المكتوبة إلى `UpdateProductUnitRequest` — **شاشة فقط**، بلا مسّ الباك-إند (مقفل) ولا الهيكل (`RULE-N198`).
- **D-2:** لا يُضاف حارس تكرار للباركود في الشاشة — قاعدة تكرار الوحدة تعيش في القيد الفريد بالقاعدة (`IX_ProductUnits_Barcode`)، ونسخها في الشاشة سلوك ثانٍ (`RULE-N172`) وسيحتاج عقداً جديداً في الباك-إند المقفل.
- **D-3:** الانعكاس (`frmCRUDCProductUnitMaster` ← `Products.Barcode`) **مسجَّل مفتوحاً** بصورته المقيسة، ولا يُنفَّذ في هذه الموجة (`§4` 9c: لا توسيع صامت). **وبقرار المالك 2026-11-20 (`TASK-031`) سقط هذا الانعكاس بحذف `Products.Barcode` نفسه** — فيصير الباركود في مصدر واحد لا انعكاس فيه.
- **D-4 (سلطة المخطط):** `docs/database-schema.md` §9 ص6-7 تنصّ أن **المصدر الرسمي للباركود هو `ProductUnits.Barcode`** و`Products.Barcode` عمود بحث سريع فقط ⇒ اتجاه الإصلاح (كتابة الوحدة) هو اتجاه المالك، والخطوة التالية إزالة عمود الصنف (`TASK-031`).
- **خطر مقبول:** تعديل صنف قديم له `Products.Barcode = X` و`ProductUnits.Barcode` فارغ مع وحدة أخرى تحمل `X` ⇒ يفشل حفظ الوحدة برسالة عامة، ويبقى باركود الصنف محفوظاً. يُبلَّغ للمالك في التقرير.

## سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء المهمة — بلاغ المالك ومُقاس في الكود (مسار التعديل) |
| `2026-11-20` | W1 (`ui-agent`): تمرير الباركود المكتوب إلى `_SaveMainUnitPricesAsync` واستخدامه في `UpdateProductUnitRequest` بدل `unit.Barcode` — ملف واحد · بناء **0 Errors / 112 Warning(s)** (`--no-incremental`) |
| `2026-11-20` | مراجعة `code-reviewer`: 0 Blocker · 0 Major · 1 Minor ⇒ أُغلق في W1-M (ذكر الباركود في رسالتي التحذير) · بناء **0 Errors / 112 Warning(s)** |
