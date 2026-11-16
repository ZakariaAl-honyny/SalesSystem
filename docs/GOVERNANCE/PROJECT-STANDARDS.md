# المعايير الخاصة بهذا المشروع (Project Standards) — Sales Management System

> **هذا الملف يحمل القيم الخاصة وحدها.** القوانين العامة (السلوك قبل الكود · MVP · الأمان آخراً · القياس · الدليل · السجلات · المكان الواحد · البساطة) نصّها في
> `GENERAL-STANDARDS.md` **ولا تُعاد كتابتها هنا** (`RULE-N169`/`RULE-N174`).
> **كل صف هنا يحمل مرجعاً** (`AGENTS.md` §…) — النص الكامل هناك، ولا يُنسخ.

---

## 1. الجدول (P-01 … P-18)

| # | المعيار | القيمة الملزمة في هذا المشروع | المرجع |
|---|---|---|---|
| **P-01** | المنصة والطبقات | .NET 10 · WinForms سطح مكتب · **2-Tier**: `Desktop → BLL → EF Core → SQL Server` — **7 مشاريع**: `Common` · `Models` · `DTO` · `DataAccess` · `Business` · `Desktop` · `Tests` | §1 · `RULE-N110` |
| **P-02** | لا طبقة API | ممنوع `HttpClient` · Controllers · JSON في مسار البيانات. الأسماء: `I<Entity>DesktopService` · `Direct<Entity>Service` · `Mock<Entity>DesktopService` | §2.6 · `RULE-N111` |
| **P-03** | أسلوب الواجهة | **WinForms كلاسيكي (code-behind)** — لا MVVM · لا `*ViewModel` · لا `ICommand`/`RelayCommand`. الاستثناء الوحيد: `INotifyPropertyChanged` على صنف سطر داخل `BindingList<T>` | `RULE-N109` |
| **P-04** | أسلوب الباك-إند | **N-Layer بسيط** — لا `Domain`/`Application`/`Infrastructure`/`UseCases` · لا MediatR/CQRS/Handlers · لا AutoMapper Profiles · لا Specification · لا مستودع فوق `IUnitOfWork` | `RULE-N110` |
| **P-05** | أنواع الأموال والكميات | المال `decimal(18,2)` · الكميات `decimal(18,3)` · ممنوع `float`/`double`/`real`/SQL `money` · المبالغ تُعرض `N2` والكميات `N3` | `RULE-001`/`RULE-002` · §2.1 |
| **P-06** | نمط النتيجة | كل خدمة تُعيد `Result`/`Result<T>` · لا استثناء يخرج للمستخدم · الرسائل **عربية** · التفاصيل في Serilog فقط · لا `ex.Message` في الواجهة | §2.5 · `RULE-N14` · `RULE-N29` |
| **P-07** | المعاملات المالية | كل عملية مالية داخل `BeginTransactionAsync` · لا حذف نهائي لفاتورة/مستخدم (`Cancelled`/تعطيل) · فاتورة `Posted` لا تُعدَّل | §2.3 · §2.4 · `RULE-004` |
| **P-08** | اللغة والاتجاه | كل واجهة عربية `RightToLeft = Yes` + `RightToLeftLayout = true` + خط `Tahoma` · كل رسالة للمستخدم عربية | `RULE-N20`/`N21`/`N28` |
| **P-09** | الألوان | **مصدر واحد** `ThemePalette` (24 لوناً في 6 مجموعات) يُستهلك عبر `GreenThemeHelper`/`ThemeHelper` · ممنوع لون حرفي في شاشة أو خدمة أو Mock · `GreenThemeHelper.Apply(this)` **آخر سطر** في كل شاشة · `ApplyGrid` لكل شبكة · كل افتراضي متقاعد يُسجَّل في `RetiredDefaults` | §2.19 · §2.40 · `RULE-N50`…`N54c` · `RULE-N188`/`N189` |
| **P-10** | نمط الشاشة | بيانات أساسية ⇒ `frm*Master` مدمج (محرر + تصفية + شبكة) · مستندات ⇒ `frmList*` + `frmAddUpdate*` · لا `frmAddUpdate*` لكيان له Master | §2.28 · `RULE-N100`…`N104` |
| **P-11** | لوحة المفاتيح | الستة من `KeyboardHelper` في كل شاشة إدخال · `Enter` يتبع ترتيب العرض ثم **يُركِّز** زر الوضع بلا تنفيذ (`RULE-N191`) · `Enter` داخل شبكة السطور لا يُرحّل مستنداً (`RULE-N153`) · الحقل المطلوب الفارغ يحجب `Enter` (`RULE-N194`) · `WireEnterAsTab` يجب أن يُعلن `Enter` في `PreviewKeyDown` وإلا ابتلعه `AcceptButton` (`RULE-N193`) | §2.18 · §2.27 |
| **P-12** | المكان الواحد (بيوت الكود) | `Desktop/Helpers/` (ثيم · تنقل · تصفية · تفقيط · تصدير · تغذية حقول) · `Desktop/Global Classes/` (`clsGlobal` · `clsFormat` · `clsDialogs` · `clsValidation` · `clsInputFeedback`) · `Common/Helpers/` (`Guard` · `StringExtensions` · `DateTimeExtensions` · `FileHelper`) · `Business/Helpers/` (حسابات). **`clsDialogs` هو الملف الوحيد المسموح له بمناداة `MessageBox`** | §2.30 · `RULE-N169`…`N172` |
| **P-13** | الحساب والصلاحيات | **MVP: حساب مدير واحد** بلا نظام صلاحيات · لا `frmRoles` ولا `PermissionsMask` ولا `HasPermission` · كل ذلك مؤجل إلى V2 (`RULE-N105`…`N108`) — **وأساسيات الحماية الآن**: BCrypt (work factor 12) · قفل بعد 5 محاولات · لا حذف نهائي · لا أسرار في الكود | §2.13 · §2.29 |
| **P-14** | مرحلتان لكل مسار بيانات | **Stage A (Mock)** = شاشة + عقد + `Mock*DesktopService` + بيانات أولية · `UseMockData = true` · **بلا** `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` · ترحيل. **Stage B** فقط بعد موافقة مالك **مكتوبة ومؤرّخة** · والباكن-إند **مقفل** لكل وحدة حتى شهادة تغطية الـMock (`مؤهل للمراجعة`) + الموافقة | §2.16 · `RULE-N165`…`N178` · `docs/MOCK-COVERAGE.md` |
| **P-15** | تجميد قاعدة البيانات | الهيكل القائم **مجمَّد**: لا `DROP` · لا إعادة تسمية · لا تغيير نوع/تضييق طول · لا `CASCADE` · لا `UPDATE` جماعي. الزيادة الآمنة وحدها بموافقة **مكتوبة منفصلة** (لا تُخلط بموافقة السلوك) · ترحيل واحد · قراءة SQL المولَّد · خطة رجوع · `SystemSettings` قبل أي عمود جديد | §2.42 · `docs/GOVERNANCE/SCHEMA-FREEZE.md` |
| **P-16** | بوابة الشاشة التجريبية | لا تعميم سلوك على أكثر من شاشة قبل شهادة **شاشة تجريبية واحدة** 100% (بناء 0 أخطاء · صفر عيب معروف · صفر نسخة ثانية للحكم · قائمة تحقق موقّعة). الشاشة التجريبية الحالية: `frmProductMaster` | §2.39 · `RULE-N184`…`N187` |
| **P-17** | البناء والتحقق | `cd SalesSystem; dotnet build .\SalesSystem.slnx -p:ArtifactsPath=C:\Users\ALlahabi\AppData\Local\Temp\opencode\ss-artifacts` · البوابة **0 Errors** · التحذيرات تُبلَّغ ولا تُبوِّب · **ممنوع** `dotnet test` أو تشغيل التطبيق من أي وكيل · `SalesSystem.Tests` ملك المالك | §5 · `RULE-N117` |
| **P-18** | Git والتوثيق | الفرع `master` · كومِت واحد لكل موجة بعنوان إنجليزي وجسم يذكر معرّفات القواعد · دفع بلا `--force` · `local == origin` · شجرة نظيفة · الوثائق عربية + مصطلحات إنجليزية · **UTF-8 بلا BOM** · `CRLF` | `RULE-N162` · §2.32 |

---

## 2. ما لا يُنسخ إلى مشروع آخر (خاص بهذا المنتج فقط)

| البند | لماذا هو خاص |
|---|---|
| 7 مشاريع بأسمائها ومجلداتها | هيكل هذا المنتج |
| أسماء `frm*Master` / `frmList*` / `frmAddUpdate*` و95 نموذجاً | اتفاقية هذه الواجهة |
| `ThemePalette` بألوانه الـ24 و`RetiredDefaults` (35 زوجاً) | هوية بصرية قرارها مالك هذا المشروع |
| `docs/MOCK-COVERAGE.md` و33 عقد بيانات | حالة هذا المنتج لا قاعدة عامة |
| أرقام `RULE-001`/`RULE-N1xx` و`AGENTS.md` | ترقيم هذا المستودع |
| مسار `ArtifactsPath` على OneDrive | بيئة هذا الجهاز |

---

## 3. كيف تُفتح نسخة مشروع جديد (5 خطوات)

1. انسخ `GENERAL-STANDARDS.md` كما هو (لا تعدّله — هو العام).
2. انسخ هذا الملف وفّرغ **§2** ثم املأ كل صف في §1 من مشروعك: اللغة · الطبقات · الأرقام · الأسماء · أوامر البناء · Git · المصادقة.
3. اكتب `AGENTS.md` الجديد كملف قانون **يشير** إلى الملفين ولا ينسخ نصهما، ورتّب القراءة في §10.
4. أنشئ `docs/TASKS/` (`TASK-000-TEMPLATE` · `INDEX` · `ACTIVE`) و`docs/prompts/` (السجل + `ROLE-PROMPTS.md`).
5. **قبل أول Brief:** قِس (G-05) · افتح سجل المهمة (G-07) · إن كانت كبيرة افتح ملف التتبع واقلب `ACTIVE` (G-08) — ثم فوّض شريحة واحدة.

---

> **تنبيه دائم:** صف في هذا الجدول بلا مرجع `AGENTS.md` = صف غير نافذ. أضِف المرجع أولاً، ثم الصف.
