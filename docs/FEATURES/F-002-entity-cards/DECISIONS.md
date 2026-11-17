# F-002 — DECISIONS

| # | القرار | السبب | المرجع |
|---|---|---|---|
| D1 | **النطاق**: نسخ نمط المراجع حرفياً في الفكرة (بطاقة · بطاقة بتصفية · نموذج عرض · نموذج بحث) لكل وحدة من وحدات البيانات الأساسية الثماني | طلب المالك الحرفي: «review … Users\Controls & … People\Controls … do the same in this project» + «and do it the same» + «use many subagents modle by modle» (2026-11-17) | طلب المالك |
| D2 | **لا باك-إند ولا DTO ولا هيكل قاعدة بيانات** — الميزة واجهة فقط تقرأ عقوداً قائمة | الباك-إند مجمَّد حتى شهادة تغطية الـMock + موافقة مكتوبة (`RULE-N177`)، ولا حاجة لأي تغيير عقد | `RULE-N177`/`RULE-N198` |
| D3 | **`ctrl<Entity>Card` بلا خدمة**: يستقبل `Dto` من الغلاف | مرجع DVLD كان ينادي `clsUser.FindByUserID` داخل البطاقة — في هذا المشروع ذلك يكسر `RULE-N05/N06` (الشاشة لا تعرف BLL) ويجعل عنصر التحكم غير قابل للاستخدام في المصمّم؛ الفصل يُبقي البطاقة عرضاً نقياً | `RULE-N05`/`RULE-N06`/`RULE-N113` |
| D4 | **`ctrl<Entity>CardWithFilter` يبحث عبر `I<Entity>DesktopService` بعد `Attach(IServiceProvider)`** | يحتاج القراءة بالاسم/الباركود (المرجع كان يفعلها بنفسه)، و`Attach` يحفظ إنشاء المصمّم بلا وسائط | `RULE-N65` (سابقة `ctrlDashboard`) |
| D5 | **«تعديل» يفتح شاشة الدليل `frm<Entity>Master`** لا `frmAddUpdate*` | شاشات البيانات الأساسية مدمجة (`RULE-N100`)، و`frmAddUpdate*` للكيان نفسه كود ميت (`RULE-N102`) | `RULE-N100`/`RULE-N102` |
| D6 | **البطاقات نصية بلا صور** | لا يوجد مجلد `Resources` في `SalesSystem.Desktop` (مقيس) — إضافة صور كان سيكون أصولاً جديدة بلا طلب | القياس |
| D7 | **الموجة 1 = ثلاث وحدات** (Users · Customers · Suppliers) بدل شاشة تجريبية واحدة | المالك أمر صراحةً بالتعميم بوكيل لكل وحدة؛ تقليل الحجم إلى ثلاث وحدات في الموجة الأولى يحفظ كلفة أي إصلاح في النمط (بقية الوحدات موجة لكل ثلاث) | طلب المالك + `RULE-N159` |
| D8 | **الأقسام `Categories` تُقرأ من `GetAllAsync`** | `ICategoryDesktopService` بلا `GetByIdAsync` (مقيس) — لا نضيف عضواً للعقد لأجل بطاقة | القياس + `RULE-N110` |
| D9 | **لا تُبنى بطاقة للموظفين** | `Screens/Employees/Forms` فارغ ولا عقد `IEmployeeDesktopService` (مقيس) | القياس |
| D10 | **ألوان الحالة وقت التشغيل فقط** (`StatusSuccess`/`StatusDanger`/`StatusMuted`) ولا تُعلن في المصمّم | لون حالة لا لون تصميم — استثناء موثّق كما في `RULE-N154`/`RULE-N182` | `RULE-N134` استثناء الحالة |
| D11 | **`frmFind<Entity>` بلا `DataBack`**: `ShowDialog` + `Selected<Entity>` + `DialogResult.OK` | المنتقيات القائمة في مشروعنا (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection`) تعمل هكذا (مقيس في `frmOpeningStock`) — ولا نُدخل نمطاً ثانياً للاختيار | القياس + `RULE-N172` |
| D12 | **العناصر النائبة عربية:** الحقل النصي غير المُدخل = «غير مُدخل» · أي قيمة غائبة (المعرّف/الحالة) = «—» · ويُمنع `[????]`/`N/A` | `RULE-N28` (كل نص يراه المستخدم عربي) يتقدّم على الشكل الحرفي للمرجع؛ قرار المالك المعلَن عبر الموجة (2026-11-17) | `RULE-N28` |
| D13 | **`catch` واحد عند الحدّ** — حُذف كل `catch` داخلي مكرّر في شريحة `Customers` (نُقل حدّ الاسم إلى `LoadCustomerAsync(string)` العام) | نصّ المالك 2026-11-17: «try to do not use try/catch every where because is slow code» → سُجِّل `RULE-N205`، وبقي `finally` لإعادة الحالة | `RULE-N205` |
| D14 | **كل وحدة لها نقطة فتح لكل عنصر أنتجته** — `btnShowCard` و`btnFindCustomer` في شاشة الدليل | عنصر بلا مستهلك = كود ميت ولا يُختبر (اكتُشف بمراجعة `frmFindCustomer`) | `RULE-N102` روح القاعدة + `RULE-N119` |
