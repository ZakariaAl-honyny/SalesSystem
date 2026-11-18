# F-004 — سجل الجولات (LOG)

> **إضافي فقط (append-only).** كل جولة تُضاف بتأريخها؛ **لا تُحذف** ولا تُعدَّل جولة سابقة.
> يكتب فيه الـOrchestrator وحده؛ الوكلاء يُعيدون الفروق في تقاريرهم.

## جولة 0 — 2026-11-18 — فتح السجل والقياس

**ما تم:** قياس الحالة الفعلية قبل أي تقسيم (خطوة §4‑0) + فتح سجل الميزة + تسجيل قرار المالك + جرد 21 ملفاً.

**الملفات المقيسة:** `Screens/{Returns,Expenses,CustomerReceipts}/**` (21 ملفاً) + العقود الأربعة + `Program.cs` (7 أسطر) + `frmMain.cs`/`frmMain.Designer.cs`.

### الأدلة (أمر + مخرج)

| ما قيس | الأمر | المخرج |
|---|---|---|
| أسطر/بايتات الشاشات السبع | `Get-ChildItem -Recurse ... -Include *.cs \| ForEach { (Get-Content … \| Measure-Object -Line).Lines }` | `frmListReturns` 501 · `frmSalesReturn` 519 · `frmPurchaseReturn` 516 · `frmListExpenses` 619 · `frmAddUpdateExpense` 377 · `frmListCustomerReceipts` 562 · `frmAddUpdateCustomerReceipt` 420 |
| وجود `EntryMode`/`_ApplyModeState` | `Select-String -Pattern "EntryMode\|_ApplyModeState"` | **0 في السبع** — لا آلة حالة ماستر (التحويل إعادة بناء لا تعديل) |
| أعضاء العقود | `Select-String -Path Services/Interfaces/I{Sales,Purchase}Return…` | المرتجعات: `Create` · `Post` · `Cancel` فقط — **لا `UpdateAsync`** |
| مراجع الشاشات السبع | `Select-String -Path "SalesSystem.Desktop\**\*.cs"` | `frmMain`: 263 · 274 · 277 · 296 · 299 (5 معالجات) · `DocumentTextHelper`: تعليقان فقط (لا كود) |
| تسجيلات DI | `Select-String -Path Program.cs` | 249 · 250 · 317 · 318 · 321 · 322 · 328 (7 أسطر) |
| تغطية الـMock | `docs/MOCK-COVERAGE.md` | `IExpense` و`ICustomerReceipt` = `مؤهل للمراجعة` · `ISalesReturn`/`IPurchaseReturn` = `مؤهل جزئياً` |

### قرارات الجولة

- `D-01`…`D-07` في `DECISIONS.md` (نصّ المالك حرفياً + اختيار «Two Masters»).
- **استثناء `RULE-N101`/`N103`** لهذه الوحدات — يُحدَّث `AGENTS.md` §2.28/§9 في جولة الإغلاق.

### التقسيم المعتمد

W1: `frmExpensesMaster` + `frmCustomerReceiptsMaster` (وكيلان) → بناء · W2: `frmSalesReturnMaster` + `frmPurchaseReturnMaster` (وكيلان) → بناء · W3: الربط والحذف (وكيل واحد) → بناء · W4: مراجعة واحدة.

**سبب عدم دمج W1 وW2:** المرتجعات نمط حالة مختلف (بلا وضع تعديل — `D-05`)؛ يُثبَّت نمط المستر على الأخف أولاً ثم يُقلَّد.

### تعارض مسجَّل

`F-001` مفتوحة على هذه الملفات (تعميم `Enter`/تغذية الحقول) — الشاشات الجديدة تُكتب بالنمط النهائي (`WireEnterAsTab(scope, primaryButton)` + `RequiredFieldHelper` + لا `_ValidateForm`)، فتُغلق مدخلات الملفات المحذوفة في F-001 عند إغلاق F-004. لا عمل مزدوج ولا تعارض ملفات (F-001 لا تعمل على ملفات W3 المشتركة في هذه الأثناء).

**الحالة:** 🔄 مفتوحة — W1 جاهزة للإرسال.

---

## جولة 1 — 2026-11-18 — التنفيذ والتسليم

**ما تم:** بناء 4 شاشات `frm*Master` (وكيل لكل شاشة) · ربط DI والقائمة · حذف 7 شاشات قديمة (21 ملفاً) · إصلاح أثر الحذف في 3 شاشات مستدعية · مراجعة واحدة · إغلاق Major.

### الأدلة (أمر + مخرج)

| ما قيس | الأمر | المخرج |
|---|---|---|
| البناء النهائي | `dotnet build .\SalesSystem.slnx -p:ArtifactsPath=…\ss-artifacts` | **Build succeeded · 0 Errors · 71 Warning(s)** (تشغيل تزايدي — الأعداد غير قابلة للمقارنة مع تشغيل كامل، §12) |
| الشاشات الأربع | `Measure-Object -Line` | 974 · 886 · 1,173 · 993 سطراً (`.cs`) مقابل 1,031 · 971 · 1,126 · 1,147 (المصمّم) |
| بقايا الأسماء السبعة | `Select-String -Recurse` | **9 مطابقات كلها تعليقات** · صفر مرجع كود |
| المجال الميت | `Select-String "SalesSystem.Desktop.CustomerReceipts.Forms"` | **0** |
| `MessageBox.Show` · `_ValidateForm` · `SystemColors` · `row.Cells["` · صيغة لون مختصرة | على الشاشات الأربع | **0 · 0 · 0 · 0 · 0** |
| `msbtnReturnsList` | `frmMain.cs` + `frmMain.Designer.cs` | **0** |
| تسجيلات DI | `Program.cs` | 4 مضافة · 7 محذوفة |

### المراجعة

`code-reviewer` (قراءة فقط): **0 Blocker · 1 Major · 6 Minor**.
- **Major (أُغلق):** `clsGlobal._OpenForm` كان يُفعِّل نافذة مفتوحة ويُهمل النسخة الجديدة، فيضيع `SelectRecord` على كائن غير معروض ⇒ صار `_OpenForm` يُعيد `Form?` (النافذة الحيّة) بعد فحص `Application.OpenForms` **قبل** الحل (`RULE-N133`)، والمواضع الثلاثة تستخدم القيمة المُعادة عبر `as frm*` + `?.SelectRecord(...)`.
- **Minor أُغلقا:** لون «إلغاء المستند» وُحِّد `(220,53,69)` في الأربع (`RULE-N204`) · `frmPurchaseReturnMaster` صار يُعطّل الأزرار بدل إخفائها (`RULE-N43`/`N154`).
- **Minor مسجَّلة (لم تُغلق):** (أ) `frmListPayments` لا يتحدّث بعد إنشاء سند في المستر (نمط التحديث بعد الفتح لا بعده) · (ب) الزر الأساسي في وضع التعديل: «تعديل» في المصروفات مقابل «حفظ» في السندات · (ج) زر «حفظ مسودة» مفرد في السندات مقابل زوج «حفظ/تعديل» في المصروفات · (د) تكرار `ApplyUiTheme()` في تحميل `frmCustomerReceiptsMaster`.

### أثر خارج النطاق (أُعلن لا أُخفي — §4 خطوة 9c)

حذف الشاشات السبعة كشف **5 مواضع استدعاء حيّة** في ثلاث شاشات لم تُذكر في النطاق الأصلي، فوجب تعديلها وإلا انكسر البناء أو التنقّل:
`frmCashBoxTransactions` (سندان) · `frmListPayments` (سندان) · `frmAddUpdateSalesInvoice` (زر «مردود بيع»).
التحويل: من حوار نمطي يفتح محرراً قديماً → إلى فتح الشاشة المدمجة داخل النافذة (`clsGlobal._OpenForm`) + `SelectRecord(id)`.
**فرق سلوكي معلن للمالك:** (1) لم يبقَ حجب للنافذة الأم · (2) «سند جديد» من `frmListPayments` يفتح الشاشة في وضع العرض ثم يضغط المستخدم «جديد» · (3) `frmListPayments` لا يُحدّث شبكته تلقائياً بعد الإنشاء (يحتاج `F5`).

### الحالة

✅ **منفَّذ ومُسلَّم** — `0 Errors` · مراجعة واحدة · كل الأعداد في `MANIFEST.md` مقيسة في هذه الجولة. ⏳ **بانتظار اختبار المالك اليدوي** (القائمة في تقرير التسليم) — لا يُعدّ مغلقاً قبل توقيعه.

---

## جولة 2 — 2026-11-18 — رجوع المالك عن دمج المرتجعات (`D-10`)

**نصّ المالك:** «Returns UI & UX the same like …\Screens\Sales\Forms\ frmAddUpdateSalesInvoice & frmListSales not like frm*Master. so now rewrite code the same like … for Returns & Sales Purchases» · وقبله: «and also Expenses» (المصروفات تبقى مدمجة).

**ما تم:** استرجاع الشاشات الثلاث من `906e0c0` (**لا إعادة كتابة من الصفر** — `RULE-N195`) ثم مواءمتها على نمط المبيعات · حذف المسترين · إعادة توصيل القائمة وDI · إصلاح عيبَي بذور/المجموع الفرعي · مراجعة ثانية.

### الأدلة

| ما قيس | الأمر | المخرج |
|---|---|---|
| البناء | `dotnet build .\SalesSystem.slnx` | **Build succeeded · 0 Errors · 72 Warning(s)** (تزايدي) |
| المستران المحذوفان | `Select-String "ReturnMaster"` عبر الحل | **0** |
| الشاشات التسع | `Screens/Returns/Forms/*` | 3 شاشات × 3 ملفات (الأحجام تعود لأصلها + المواءمة) |
| `MessageBox` / `_ValidateForm` / `_RequiredFieldsValid` / `_FocusFirstMissingField` / `ActiveControl` | على شاشات المرتجعات | **0 · 0 · 0 · 0 · 0** |
| `WireEnterAsTab(this)` | على شاشات المرتجعات | **1** وهو **مطابق حرفياً** لـ`frmListSales.cs:58` (شاشة قائمة — القاعدة تُطابق المرجع) |
| `Cells[` | على شاشات المرتجعات | **1** وهو `Cells[0]` فهرس رقمي مشروع |
| صيغة اللون المختصرة في المصمّمين | `Color\.FromArgb\(\s*\d` | **0** بعد التحويل في `frmPurchaseReturn.Designer.cs` (20 موضعاً) |

### المواءمة المنفَّذة (مقابل الشاشة المرجعية)

- `frmSalesReturn`: `WireEnterAsTab(gbInvoiceInfo, _PrimaryActionButton)` ثنائي الوسيط · `RequiredFieldHelper.Attach` بعد `WireFocusHighlight` مع شرط «عميل نقدي = معرّف 0» (`When`) حتى لا يُرفض البيع النقدي · حُذفت `_ValidateForm` (2) وحلّت محلها `_ValidateRangeRules` · `IsTextInputFocused` في `Delete` · `Clear` في مسار إعادة الضبط.
- `frmPurchaseReturn`: `Clear` في إعادة الضبط/الوضع · `DataError` مكتوم (لا صندوق إنجليزي) · تركيز الشبكة عند رفض السطور · **«المجموع الفرعي» صار يُحدَّث** (كان `0.00` دائماً).
- `frmListReturns`: أعيد بناؤه على `frmListSales` — أعمدة معلنة في المصمّم (حُذف بناء الأعمدة وقت التشغيل) · ربط السطر بالمعرّف لا بالفهرس (كان عيباً كامناً بعد التصفية) · تصفية فورية + `btnClearFilters` منفصل ومعطَّل بلا تصفية · `pnlSummary` بأزواج عنوان/قيمة · فتح المحرر + إعادة القراءة عند الإغلاق · قراءة فقط.

### المراجعة (الثانية)

`code-reviewer`: **0 Blocker · 1 Major · 3 Minor**. الـMajor **سجل توثيقي** (غياب صفّ إلغاء القرار وتحديث `ACTIVE`/`INDEX`) — أُغلق في هذه الجولة بـ`D-10` وصفّي 7/8. الـMinors: (1) صيغة اللون في مصمّمَي المحررين — **أُغلقت** · (2) مسح جزئي خارج `IsSuccess` في `frmListReturns` — **أُغلق** · (3) قيم لوحة متقاعدة (`PAL-5`) — باقية كدَين معلن.

### فروق مقصودة عن `frmListSales` (لا عيب)

تبويبان (مبيعات/مشتريات) وشبكتان ومحرّران — تصميم الشاشة الأصلي ونوعا المستند في العقدين؛ والمدى الافتراضي «هذا الشهر» (عرف شاشات المستندات)؛ و`StatusOption` محلي (نقل إلى `Helpers/` ممنوع في هذه الموجة — `RULE-N172`).

### الحالة

✅ **منفَّذ** — الشاشات الثلاث بنمط المبيعات · المصروفات والسندات تبقى مدمجة · `0 Errors` · ⏳ بانتظار اختبار المالك.
