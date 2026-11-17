# F-002 — LOG

## الجولة 1 — 2026-11-17 — مراجعة المرجع + خطة الموجات

### ما قُرئ (بالأمر)

| المرجع | الأسطر | المحتوى |
|---|---|---|
| `People\Controls\ctrlPersonCard.cs` | 136 | بطاقة عرض: `LoadPersonInfo(int?\|string)` · `ResetPersonInfo()` · `PersonID` · `SelectedPersonInfo` · رابط تعديل |
| `People\Controls\ctrlPersonCardWithFilter.cs` | 200 | بطاقة + تصفية: `OnPersonSelected` · `FilterEnabled` · `ShowAddPerson` · `FilterFocus()` · `FindNow()` |
| `Users\Controls\ctrlUserCard.cs` | 83 | بطاقة مستخدم = `ctrlPersonCard` + صندوق «User Information» (ID · UserName · isActive) |
| `People\frmShowPersonInfo.cs` · `Users\frmShowUserInfo.cs` · `People\frmFindPerson.cs` · `Users\frmAddUpdateUser.cs` (296) | قصيرة | أنماط الاستخدام: عرض · بحث `DataBack` · اختيار داخل شاشة إدخال |

### القياسات (مخرجات الأوامر)

| القياس | الأمر | الناتج |
|---|---|---|
| شاشات الدليل | `Get-ChildItem Screens -Recurse -Filter "frm*Master.cs"` | **9** |
| عقود البيانات | `Get-ChildItem Services\Interfaces -Filter "I*DesktopService.cs"` | **33** (منها 8 تخص وحدات البيانات الأساسية) |
| `ICategoryDesktopService.GetByIdAsync` | `Select-String` على الملف | **غير موجود** ⇒ البطاقة تقرأ من `GetAllAsync` |
| مجلد `Resources` | `Get-ChildItem SalesSystem.Desktop -Directory` | **غير موجود** ⇒ لا صور |
| `Screens\Employees\Forms` | `Get-ChildItem -Force` | **فارغ** ولا عقد موظفين ⇒ مستثنى |
| أساس البناء | `dotnet build .\SalesSystem.slnx` | **Build succeeded · 0 Error(s) · 114 Warning(s)** (+ خطأان عابران `CS2001` من ترحيل كان يُعاد توليده في اللحظة نفسها — زال في البناء التالي) |

### القرارات الجديدة

- **D7**: الموجة 1 = ثلاث وحدات (Users · Customers · Suppliers) بأمر المالك «use many subagents modle by modle».
- **D8**: Categories بلا `GetByIdAsync` ⇒ قراءة من القائمة.
- **D11 (جديد — من مراجعة الاستخدام):** `frmFind<Entity>` يتبع **نمط مشروعنا** (`ShowDialog` + `Selected<Entity>` + `DialogResult.OK`) لا `DataBack` — لأن المنتقيات القائمة (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection`) تعمل هكذا (مقيس في `frmOpeningStock.btnAddProduct_Click`).

### الحالة

الموجة 1 فُوِّضت: وحدة `Customers` أولاً بأمر المالك («work on Customer folder after you finished 100% move to next»).

---

## الجولة 2 — 2026-11-17 — وحدة `Customers` مُنجَزة (14 ملفاً)

### ما نُفِّذ

| الملف | الحالة | ملاحظة |
|---|---|---|
| `Screens/Customers/Controls/ctrlCustomerCard.cs/.Designer.cs/.resx` | جديد | بطاقة عرض للقراءة فقط · صفر نداء خدمة · `LoadData` · `ResetCustomerInfo` · `CustomerId` · `SelectedCustomerInfo` · `ShowEditLink` · `EditRequested` |
| `Screens/Customers/Controls/ctrlCustomerCardWithFilter.cs/.Designer.cs/.resx` | جديد | `Attach(IServiceProvider)` · `CustomerSelected` · `FilterEnabled` · `ShowAddNew` · `LoadCustomerAsync(int\|string)` · `FilterFocus()` · تصفية بالمعرّف/الاسم/الهاتف |
| `Screens/Customers/Forms/frmShowCustomerCard.cs/.Designer.cs/.resx` | جديد | غلاف: بطاقة + «تعديل» (يفتح الدليل من DI) + «إغلاق» |
| `Screens/Customers/Forms/frmFindCustomer.cs/.Designer.cs/.resx` | جديد | منتقي نمطي: `DialogResult.OK/Cancel` + `SelectedCustomer` |
| `Screens/Customers/Forms/frmCustomerMaster.cs/.Designer.cs` | **مُعدَّل (إضافة)** | `btnShowCard` («بطاقة») + `btnFindCustomer` («بحث عن عميل») داخل `pnlButtons` + معالجان + سطرَا تفعيل داخل `_ApplyModeState` + إضافتهما إلى `WireButtonDisabledVisual` — `+61` سطراً · `0` حذف |
| `Program.cs` | **مُعدَّل** | `services.AddTransient<frmShowCustomerCard>();` · `services.AddTransient<frmFindCustomer>();` (سطرا 279–280) — **لم يُدرَج في كومِت الشريحة** لأن الملف يحمل تغييرات مهام مفتوحة أخرى (TASK-002/007/008)؛ يبقى في شجرة العمل |

### العيوب التي وُلدت في هذه الموجة وأُغلقت فيها (`PILOT`-نمط)

| # | العيب | الإصلاح |
|---|---|---|
| `F2-1` | `WFO1000` — خاصيتان عامتان على `UserControl` بلا إعلان تسلسل ⇒ **بناء أحمر** | `[Browsable(false)]` + `[DesignerSerializationVisibility(Hidden)]` على `FilterEnabled`/`ShowAddNew`/`CustomerId`/`SelectedCustomerInfo`/`ShowEditLink` — **يُضاف لكل brief لاحق** |
| `F2-2` | `clsGlobal._OpenForm` يفعّل نافذة مفتوحة **ولا يعرض** الجديدة ⇒ نقرة «بطاقة» على عميل آخر تُظهر بطاقة قديمة | فحص `Application.OpenForms` **قبل** الحل من DI + `LoadCustomerAsync` على الموجودة (`RULE-N133`) |
| `F2-3` | رابط «تعديل بيانات العميل» داخل المنتقي **بلا فعل** | `ShowEditLink` (واجهة وقت تشغيل) + `ctrlCustomerCard1.ShowEditLink = false` في المُنشئ |
| `F2-4` | `[????]` / `N/A` لاتينية على شاشة عربية (`RULE-N28`) | **D12**: «غير مُدخل» + «—» في الكود والمصمّم |
| `F2-5` | `_ = _FindFromFilterAsync()` بلا حارس على مساراته المبكرة | حدّ خطأ واحد في الدالة نفسها (وهي الحدّ لأن معالجَيها متزامنان) |
| `F2-6` | زر «بطاقة» غير مشمول في `WireButtonDisabledVisual` ⇒ معطّل بلا مظهر تعطيل (`RULE-N43`) | أُضيف الزرّان إلى الاستدعاء الواحد |
| `F2-7` | `frmFindCustomer` **بلا مستهلك** ⇒ بنود اختباره غير قابلة للتنفيذ | `btnFindCustomer` يفتحه نمطياً ويُعبّئ المحرر عبر `_FillFormAsync` + `_SelectGridRow` (نفس تسلسل ما بعد الحفظ) |
| `F2-8` | `StartPosition = CenterParent` لنافذة غير نمطية | `CenterScreen` |

### الأدلة (مخرجات الأوامر)

| البند | الناتج |
|---|---|
| `dotnet build .\SalesSystem.slnx` | **Build succeeded · 0 Error(s) · 99 Warning(s)** (وضع تزايدي — غير مقارن بـ114 الأساسية · البوابة = 0 أخطاء) |
| `grep "\[????\]\|N/A"` في artefacts الجديدة | **0** |
| `grep MessageBox` / `ex.Message` | **0** / **0** |
| `grep "FromArgb(<رقم>"` في المصمّمات الجديدة | **0** (48 موضعاً بصيغة `Convert.ToInt32` — `RULE-N204`) |
| نداء خدمة داخل `ctrlCustomerCard.cs` | **0** |
| `GreenThemeHelper.Apply(this)` آخر سطر | 4/4 |
| مراجعة `code-reviewer` | `مقبول مع ملاحظات` · **0 Blocker · 0 Major · 2 Minor** — أُغلقت الاثنتان + 5 Notes مقصودة |

### قائمة الاختبار اليدوي (ملخّص — التفصيل في رسالة التسليم)

60 بنداً عربياً: mock (1–50) · حقيقي (51–60) · يشمل: تعطيل زر «بطاقة» بلا تحديد · فتح البطاقة وتحديثها في نفس النافذة · الحقول العشرة · «نشط/معطّل» بلون الحالة · البحث بالمعرّف/الاسم/الهاتف + الرفض (حقل فارغ · `abc` · `999999` · بلا مطابقة) · `Enter` يبحث مرة واحدة بلا قفز تركيز · `Esc` يغلق · المبالغ `N2` بفاصل آلاف · «بحث عن عميل» يُعبّئ المحرر ويُبرز السطر · المصمّم = التشغيل بلا رمادي · تكافؤ Mock/Real.

### الحالة

`Customers` **منجزة داخل الشريحة**. التالي بأمر المالك: وحدة واحدة في كل مرة — `Suppliers` ثم `Users` ثم البقية (`PLAN.md` §4) مع نقل درسَي `WFO1000` و`F2-2` إلى كل brief.
