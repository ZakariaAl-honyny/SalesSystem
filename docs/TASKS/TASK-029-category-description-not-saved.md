# TASK-029 — وصف التصنيف لا يُحفظ في القاعدة (إصلاح عيب)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-029` |
| `العنوان` | `frmCRUDCategoryMaster` — حقل «الوصف» يُدخَل ويُقرأ لكنه لا يصل إلى القاعدة عند الإضافة ولا عند التعديل |
| `الحجم` | **S** (4 ملفات · عيب لا ميزة · لا تغيير هيكل قاعدة بيانات) |
| `المرحلة` | `—` (إصلاح عيب على مسار بيانات قائم — لا مرحلة `A`/`B`: لا شاشة جديدة ولا كيان جديد) |
| `الحالة` | **منفَّذة** — بناء 0 أخطاء · مراجعة واحدة (0 Blocker · 0 Major · 1 Minor مُغلق) · بانتظار اختبار المالك |
| `الوكيل المالك` | `implement-agent` (W1) · `code-reviewer` (مراجعة واحدة) |
| `أثر قاعدة البيانات` | **لا شيء** — العمود `ProductCategories.Description nvarchar(500)` قائم ومُهيَّأ في `ProductCategoryConfiguration` (`HasMaxLength(500)`) — لا ترحيل ولا تعديل هيكل (`RULE-N198`) |
| `التاريخ` | `2026-11-20` |

## الهدف

بعد هذه المهمة يُحفظ «وصف التصنيف» المكتوب في `frmCRUDCategoryMaster` في العمود `ProductCategories.Description` عند الإضافة وعند التعديل، ويظهر بعد إعادة فتح الشاشة وبعد التحديث التلقائي.

## العيب المقيس (لا منقول)

الشاشة تُرسل الوصف فعلاً، والـMock يحفظه، والقاعدة والتهيئة تحملان العمود — والفقد في **حلقة العقد** بين `CategoryDto` (سطح المكتب) و`ProductCategoryDto` (طبقة الأعمال):

| # | الموضع | القياس |
|---|---|---|
| 1 | `frmCRUDCategoryMaster.cs:641-661` | يقرأ `txtDescription` ويبني `CreateCategoryRequest(name, description)` / `UpdateCategoryRequest(name, description, isActive)` ✅ |
| 2 | `MockCategoryDesktopService.cs:38,60-62` | يحفظ `Description` — لذلك العيب **لا يظهر في وضع التجربة** ✅ |
| 3 | `DirectCategoryService.cs:46-47` | `// TODO: CreateCategoryRequest.Description لا يدعمه CreateProductCategoryRequest حالياً.` ثم `new CreateProductCategoryRequest(r.Name)` ❌ |
| 4 | `DirectCategoryService.cs:63-64` | `// TODO: UpdateProductCategoryRequest يقبل الاسم فقط` ثم `new UpdateProductCategoryRequest(r.Name)` ❌ |
| 5 | `ProductCategoryRequests.cs:3-4` | العقدان يحملان `Name` فقط ❌ |
| 6 | `ProductCategoryService.cs:68` | `ProductCategory.Create(name: request.Name)` — بلا وصف ❌ |
| 7 | `ProductCategoryService.cs:104` | `category.Update(request.Name)` — بلا وصف ❌ |

**الصنف (لا الحالة) — `RULE-N196` / §4 خطوة 9b:** عُدّت المواضع التي يُسقَط فيها حقل يحمله عقد سطح المكتب على بابا/باك-إند في `Services/Direct/**`: **موضعان فقط** — كلاهما في `DirectCategoryService` (الإضافة والتعديل). بقية `TODO` في `DirectReportService`/`DirectSalesReportService`/`DirectSettingsService` **صنف آخر** (حقول تقرير غير متاحة في الـDTO · دوال تقرير غير منفَّذة · إعدادات بلا حفظ) وليست إسقاط حقل مُدخَل — تُسجَّل ولا تُمسّ هنا.

## خارج النطاق (ممنوع بالاسم)

- `SalesSystem.Models/**` · `SalesSystem.DataAccess/**` · `SalesSystem.DataAccess/Migrations/**` · `SalesSystem.DataAccess/Seeders/**` — **لا تغيير هيكل**
- `MockCategoryDesktopService.cs` · `MockLookupsData.cs` — الـMock يحفظ الوصف أصلاً (لا مساس)
- `frmCRUDCategoryMaster.Designer.cs` · `frmCRUDCategoryMaster.resx` — لا عنصر جديد ولا لون
- `DirectReportService.cs` · `DirectSalesReportService.cs` · `DirectSettingsService.cs` — أصناف أخرى مسجَّلة لا تُمسّ
- `Program.cs` · `appsettings.json` · أي `.csproj` · `SalesSystem.Tests/**`
- `AGENTS.md` · `docs/**` — يكتبها الـOrchestrator وحده (`RULE-N197`)

## الملفات (قائمة مغلقة)

| # | الملف | العمل |
|---|---|---|
| 1 | `SalesSystem/SalesSystem.DTO/Requests/ProductCategoryRequests.cs` | إضافة `string? Description` إلى `CreateProductCategoryRequest` و`UpdateProductCategoryRequest` |
| 2 | `SalesSystem/SalesSystem.Business/Services/ProductCategoryService.cs` | تمرير `request.Description` إلى `ProductCategory.Create(...)` وإلى `category.Update(...)` |
| 3 | `SalesSystem/SalesSystem.Desktop/Services/Direct/DirectCategoryService.cs` | تمرير `r.Description` في الطلبين وحذف الـ`TODO`ين |
| 4 | `SalesSystem/SalesSystem.Desktop/Screens/Categories/Forms/frmCRUDCategoryMaster.cs` | حارس طول الوصف (≤ 500) بنفس نمط حارس الاسم القائم — يمنع خطأ قاعدة بيانات عاماً بدل رسالة عربية |

## العقود

```csharp
// قبل
public record CreateProductCategoryRequest(string Name);
public record UpdateProductCategoryRequest(string Name);

// بعد
public record CreateProductCategoryRequest(string Name, string? Description);
public record UpdateProductCategoryRequest(string Name, string? Description);
```

- `CategoryDto` و`ProductCategoryDto` و`ICategoryDesktopService` و`IProductCategoryService` — **لا تغيير** (تحمل الوصف أصلاً).
- `ProductCategory.Create(string name, string? description = null, ...)` و`Update(string name, string? description = null)` — التوقيع قائم، لا تعديل.

## البيت الواحد

- لا سلوك مشترك جديد. حارس الطول يستهلك نفس نمط الحارس القائم في الشاشة (رسالة عربية + `errorProvider` + `clsDialogs`) — **لا رقم سحري مكرَّر خارج الشاشة** (`HasMaxLength(500)` هو مصدر الحقيقة للهيكل، والحارس واجهة فقط).
- لا لون ولا Helper جديد (`RULE-N169`…`RULE-N172`).

## الموجات

| الموجة | الوكيل | الملفات | البناء |
|---|---|---|---|
| W1 | `implement-agent` | 1–4 | ✅ **0 Errors / 112 Warning(s)** (`dotnet build .\SalesSystem.slnx --no-incremental` — تشغيل كامل) |
| W1-M | `implement-agent` (نفس الجلسة) | 4 | ✅ **0 Errors / 95 Warning(s)** (تزايدي — إغلاق Minor المراجعة) |

**حجم الفرق:** `git diff --stat` للقائمة المغلقة = **4 files changed · 22 insertions(+) · 13 deletions(−)** — لا ملف خامس.

## الأدلة (إلزامي — `docs/GOVERNANCE/EVIDENCE.md`)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| الملفات موجودة وسليمة | 920 / 651 / 184 / 123 / 94 سطراً | `(Get-Content <file>).Count` | ✅ |
| مواضع إسقاط الوصف في مسار التصنيف | **2** (`DirectCategoryService.cs:47,64`) | `Select-String -Pattern "CreateProductCategoryRequest\|UpdateProductCategoryRequest"` | 10 مطابقات · لا منشئ خارجي غيرهما |
| مستهلكو العقدين | `ProductCategoryService` + واجهته + `DirectCategoryService` فقط | `grep` على الحل كله | ✅ لا اختبار ولا Seeder |
| العمود والتهيئة | `Description nvarchar(500)` | `ProductCategoryConfiguration.cs:15` | ✅ |
| `TODO` في `DirectCategoryService.cs` | **0** (كان 2) | `Select-String -Pattern "TODO"` | ✅ |
| منشئو العقدين في الحل | **2** فقط — كلاهما `DirectCategoryService` | `Select-String -Pattern "new (Create\|Update)ProductCategoryRequest"` | ✅ |
| `SetError(txtDescription, …)` | **1** | `Select-String -Pattern "SetError\(txtDescription"` | ✅ |
| `MessageBox` في الشاشة | **0** | `Select-String -Pattern "MessageBox"` | ✅ |
| `Cells[` في الشاشة (`RULE-N179`) | **0** | `Select-String -Pattern "Cells\["` | ✅ |
| تعريفات `name`/`description` داخل `_SaveCategoryAsync` | **1 + 1** | `Select-String -Pattern "string name = txtName\|string\? description ="` | 618/619 (و720 في `btnDelete_Click` — دالة أخرى) |
| ترتيب الحارسين مقابل التأكيد | اسم س624 · وصف س634 · `ConfirmEdit` س646 | قراءة الشاشة | ✅ الحارسان قبل التأكيد |
| `txtDescription.MaxLength` | غير مضبوط في المصمّم | فحص المصمّم | الحارس **حيّ** (ليس كوداً ميتاً) |

## المراجعة

`code-reviewer` (مراجعة واحدة على الشريحة): **0 Blocker · 0 Major · 1 Minor** — و**أُغلق Minor** في الموجة نفسها:

| # | الشدة | العيب | الإغلاق |
|---|---|---|---|
| 1 | Minor | حارس الـ500 كان **بعد** `ConfirmEdit` بينما حارس الاسم قبله ⇒ تأكيد لعملية سترفض | رُفعت قراءة `name`/`description` قبل الحارسين، ونُقل حارس الوصف بجوار حارس الاسم **قبل** `ConfirmEdit` — صفر تعريف مكرَّر داخل `try` |
| 2 | Minor | `frmCRUDCategoryMaster.Designer.cs` معدَّل (ألوان `170,220,170`) — خارج نطاق هذه المهمة | **مُنسب إلى موجة `F-005`** لا إلى `TASK-029`؛ لم يُلتزم مع هذه الشريحة |

## الاختبار اليدوي

> **يُنفَّذ في الوضع الحقيقي (`UseMockData = false`)** — الـMock كان يحفظ الوصف دائماً فلا يُظهر العيب.

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | `frmCRUDCategoryMaster` | «جديد» ← اسم «اختبار وصف» + وصف «وصف تجريبي للإضافة» ← حفظ | رسالة نجاح · سطر جديد في الشبكة وعمود «الوصف» يحمل النص |
| 2 | `frmCRUDCategoryMaster` | إغلاق الشاشة وإعادة فتحها | السطر يظهر ووصفه محفوظ (قُرئ من القاعدة) |
| 3 | `frmCRUDCategoryMaster` | اختيار السطر ← تعديل الوصف إلى «وصف معدَّل» ← حفظ | رسالة نجاح · الوصف الجديد في الشبكة |
| 4 | `frmCRUDCategoryMaster` | إعادة فتح الشاشة | الوصف المعدَّل ظاهر — **هذا هو العيب المُبلَّغ** |
| 5 | `frmCRUDCategoryMaster` | تعديل الاسم فقط وترك الوصف كما هو ← حفظ | الاسم يتغيّر والوصف **يبقى** (لا يُمسح — `D-3`) |
| 6 | `frmCRUDCategoryMaster` | مسح الوصف بالكامل ← حفظ | الوصف يصبح فارغاً في الشبكة وبعد إعادة الفتح (تفريغ مقصود) |
| 7 | `frmCRUDCategoryMaster` | لصق وصف أطول من 500 حرف ← حفظ/تعديل | ✗ + «وصف المجموعة أطول من المسموح (500 حرف)» + «يرجى تصحيح الأخطاء الموضحة قبل المتابعة.» · **ولا تظهر رسالة تأكيد التعديل** · لا حفظ |
| 8 | `frmCRUDCategoryMaster` | اسم مكرَّر + وصف جديد ← حفظ | رفض عربي «اسم التصنيف مستخدم بالفعل» ولا يتغيّر الوصف في القاعدة |
| 9 | `frmCRUDCategoryMaster` | بعد حالة 7 صحّح الوصف إلى ≤ 500 ← حفظ | ينجح الحفظ وتزول علامة ✗ عن حقل الوصف |

## القرارات والمخاطر

- **D-1:** إصلاح عيب على مسار بيانات قائم — لا شاشة/كيان جديد، والعمود موجود ⇒ **ليس مرحلة `A`/`B`** ولا يحتاج موافقة هيكل (`RULE-N198`). يُدوَّن كاستثناء بقاعدة `RULE-N178`(ج): تصحيح يفرضه العيب المؤكَّد بلا تغيير سلوك مالي ولا هيكل.
- **D-2:** `UpdateProductCategoryRequest` يبقى بلا `IsActive` — الحالة تُنفَّذ عبر `DeactivateAsync`/`ReactivateAsync` كما هو قائم في `DirectCategoryService` (لا توسيع عقد بلا حاجة).
- **D-3 (مُقاس):** `category.Update(request.Name)` كان يمرّر `description = null` الافتراضي ⇒ **كل تعديل اسم كان يمسح وصفاً محفوظاً صامتاً** في الوضع الحقيقي. التمرير الجديد يُصلح «عدم الحفظ» و«المسح الصامت» معاً.
- **D-4:** الحارس يقيس `description` **بعد** `Trim` وبعد تحويل الفراغ إلى `null` ⇒ الوصف الفارغ لا يُرفض (تفريغ مقصود يبقى ممكناً).
- **D-5:** الحارسان (اسم ≤ 100 · وصف ≤ 500) يُقيَّمان **قبل** `ConfirmEdit` — لا تأكيد تعديل لعملية سترفضها الشاشة، وقراءة الحقلين مرة واحدة قبل الحراس (`RULE-N172`).
- **خطر مُغلق:** وصف أطول من 500 كان سيُفشل الحفظ برسالة قاعدة بيانات عامة بعد فتح المسار — يُغلق بالحارس.
- **دَين مسجَّل لا يُمسّ:** `TODO` الستة في `DirectReportService`/`DirectSalesReportService`/`DirectSettingsService` — صنف مختلف، يُسجَّل لمهمة مستقلة.


## سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء المهمة — عيب مُبلَّغ من المالك ومُقاس في الكود |
| `2026-11-20` | W1: إصلاح حلقة العقد (4 ملفات) · بناء **0 Errors / 112 Warning(s)** كامل |
| `2026-11-20` | مراجعة `code-reviewer`: 0 Blocker · 0 Major · 1 Minor ⇒ أُغلق في W1-M · بناء تزايدي **0 Errors / 95 Warning(s)** |
