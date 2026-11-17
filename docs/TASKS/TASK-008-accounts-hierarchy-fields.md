# TASK-008 — حقول دليل الحسابات الهرمية (Accounts hierarchy fields)

| الحقل | القيمة |
|---|---|
| `الحجم` | **L** — كيان + تهيئة + ترحيل + DTO + BLL + مولّد أكواد + Direct + Mock + شاشة ثقيلة (`frmAccounts` 1,257 سطراً) + وثيقة الهيكل |
| `المرحلة` | **Stage B** على وحدة `Accounts` القائمة — تغطيتها في `docs/MOCK-COVERAGE.md` = `مؤهل للمراجعة`، والموافقة المكتوبة أدناه |
| `التاريخ` | `2026-11-17` |
| `الحالة` | **مفتوحة** |
| `ملف التتبع` | هذا الملف (وهو مُدرَج في `docs/TASKS/ACTIVE.md`) |

---

## 1. الطلب (نص المالك الحرفي)

> «update C:\Users\ALlahabi\Desktop\Sales Management System\docs\database-schema.md and all project code by using many subagetns by new this details about accounts table: …»
> ثم: «update now u»

ومعه مواصفة `AccountNode` · `CostCenterNode` · `CashboxRecord` · `BankRecord` · دوال الشجرة (`generateNextChildAccountCode` · `detectAccountSpecialCategory` · `getAllAccountsFlat` · `insertAccountIntoTree` · `calculateTreeBalances`) · والشجرة الابتدائية `BASE_RAW_CHART_OF_ACCOUNTS` · و`INITIAL_COST_CENTERS` · و`INITIAL_CASHBOXES` · و`INITIAL_BANKS`.

## 2. الموافقات المكتوبة (مؤرّخة)

| # | الموافقة | النص الحرفي / السند | التاريخ |
|---|---|---|---|
| 1 | **تعديل الباك-إند** (`Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**`) لوحدة `Accounts` | «update … all project code by using many subagetns … update now u» | `2026-11-17` |
| 2 | **تعديل الهيكل — زيادة آمنة فقط** على جدول `Accounts` (3 أعمدة، لا حذف/إعادة تسمية/تضييق/`UPDATE` جماعي) | نفس الطلب، مقيَّداً بسلّم `RULE-N198` جدول §3 أدناه | `2026-11-17` |
| 3 | **الالتزام بقفل تغطية Mock** | `IAccountDesktopService` في `docs/MOCK-COVERAGE.md` = `مؤهل للمراجعة` (سطر 1) | مُتحقَّق `2026-11-17` |

> **قيد صارم:** الموافقة 2 **لا تشمل** أي جدول آخر. أي حاجة لجدول جديد = طلب موافقة مستقلة.

## 3. قرار الهيكل — ماذا يُخزَّن وماذا يُشتق (`RULE-N198`)

**مبدأ:** الكود يتكيّف مع القاعدة. لا عمود لما يمكن اشتقاقه، ولا `UPDATE` جماعي لإعادة كتابة بيانات قائمة.

| حقل المواصفة | القرار | السبب |
|---|---|---|
| `nameEn` | موجود أصلاً | — |
| `level` | موجود أصلاً (`tinyint` + CHECK 1..10) | — |
| `isLeaf` | موجود أصلاً | — |
| `parentCode` | موجود كـ`ParentId` (FK ذاتي) + `ParentAccountCode` **مشتق** في DTO | لا عمود نصي مكرّر للشجرة |
| `status` | موجود كـ`IsActive` | — |
| **`financialStatement`** | **مشتق** من `Nature` (1/2/3 ⇒ ميزانية · 4/5 ⇒ نتيجة أعمال) | لا يُخزَّن ما يُشتق |
| **`nature` (DEBIT/CREDIT)** | **عمود جديد `NormalBalance` `tinyint NULL`** (1=مدين · 2=دائن · `NULL` = مشتق من `Nature`) | الحسابات المضادة (مثل 223 مسحوبات المالك داخل حقوق الملكية بطبيعة مدينة) لا تُشتق |
| **`treePath`** | **مشتق** في BLL من سلسلة الآباء | تجنّباً لـ`UPDATE` جماعي محرَّم على صفوف قائمة |
| **`isSubAccount`** | **مشتق** = `ParentId != null` | — |
| **`currency`** | **لا عمود إطلاقاً** — قرار المالك 2026-11-17 («remove cuurencey»). وحدة العملات مؤجلة (`§2.29` بند 4) | لا تُضاف بحجة المواصفة |
| **`allowCostCenter`** | **عمود جديد `AllowCostCenter` `bit NOT NULL DEFAULT 0`** | مطلوب للتحكم في القيد حسب مركز التكلفة |
| **`balance`** | **محسوب** من `JournalEntryLines` (مدين − دائن بحسب الطبيعة) | الرصيد لا يُخزَّن في دليل الحسابات |
| `CostCenterNode` (شجرة كاملة + موازنة + مدير) | **خارج نطاق هذه الموجة** — مسار بيانات جديد ⇒ Stage A أولاً ثم موافقة، ثم جدول جديد + ترحيل مستقل | `RULE-N165` · `RULE-N198` |
| `BankRecord` | **مرفوض الآن** — البنوك مؤجلة بقرار المالك (`§2.29` بند 4: لا كيان ولا خدمة ولا شاشة) | `RULE-N105`…`RULE-N108` |
| `CashboxRecord` | الكيان `CashBox` موجود ويحمل `AccountId` — **لا تغيير عليه** في هذه الموجة | — |

**الزيادة على `Accounts` = عمودان فقط** (`NormalBalance` · `AllowCostCenter`) — كلاهما `NULL`/`DEFAULT` فلا يرفض صفاً قائماً. **لا عمود عملة.**

## 4. النطاق المغلق (الملفات)

| الطبقة | الملفات |
|---|---|
| Models | `SalesSystem.Models/Accounting/Account.cs` |
| DataAccess | `Configurations/AccountConfiguration.cs` · `Migrations/*_AccountsHierarchyFields.*` |
| DTO | `AccountDtos.cs` (+`AccountCodePreviewDto`) · `Requests/AccountingRequests.cs` |
| Business | `Interfaces/Services/IAccountService.cs` · `Interfaces/Services/IAccountCodeGeneratorService.cs` · `Services/AccountService.cs` · `Services/AccountCodeGeneratorService.cs` · `Helpers/AccountCategoryHelper.cs` (جديد) |
| Desktop | `Services/Interfaces/IAccountDesktopService.cs` · `Services/Direct/DirectAccountService.cs` · `Services/Mock/MockAccountDesktopService.cs` · `Services/Mock/MockAccountsData.cs` · `Screens/Accounting/Forms/frmAccounts.cs` + `.Designer.cs` + `.resx` |
| Docs | `docs/database-schema.md` · `docs/MOCK-COVERAGE.md` · `docs/TASKS/ACTIVE.md` · `docs/TASKS/INDEX.md` · `docs/SHARED-HOME-REGISTER.md` · `AGENTS.md` §9 |

## 5. خارج النطاق — بالاسم

- `SalesSystem.DataAccess/Seeders/**` — **لا تغيير**: بذور القاعدة الحيّة تختلف عن بذور الـMock (تدقيق `2026-11-17`: البذور الحقيقية تستخدم `1101/1102/...` والـMock يستخدم `1111/1121/...`) — توحيدهما قرار مالك مستقل لأنه يمسّ بيانات قاعدة قائمة.
- `CostCenter` — لا كيان ولا جدول ولا شاشة في هذه الموجة.
- `Bank` / `Currencies` / `Taxes` / `Settings` — مؤجلة `§2.29`.
- `frmJournalEntries` وكل شاشات التقارير — تُستهلك الـDTO الجديدة بلا مسّها (الحقول الجديدة اختيارية بقيَم افتراضية).
- `Program.cs` — **لا تغيير مطلوب** (`IAccountDesktopService` مسجَّل في الفرعين بالفعل).
- `SalesSystem.Tests/**` — ملك المالك.

## 6. القرارات التقنية الملزمة

| # | القرار |
|---|---|
| `ACC-1` | **مولّد الأكواد — السلّم الرسمي (قرار المالك 2026-11-17):** مستوى 1 = **1 خانة** (`1`) · مستوى 2 = **2 خانتين** (`11`) · مستوى 3 = **4 خانات** (`1101`) · مستوى 4 = **8 خانات** (`11010001`) ⇒ اللاحقة 1 · 2 · 4. المولّد القائم يطبّق هذا أصلاً ولا يُعاد تصميمه؛ يُضاف فقط حارس «الكود موجود ⇒ زِد حتى يفرغ»، وتُصحَّح **أكواد الـMock** (`1111/1121` ← `1101/11010001`) وبذورها. |
| `ACC-2` | **إلغاء قيود المستوى المصطنعة:** `level = parent.Level + 1` (أو 1 للجذر)، ويُرفض > 10. لا «الورقة يجب أن تكون 4» ولا «غير الورقة ≤ 3» — المواصفة تسمح بورقة في أي مستوى (مثل `113` · `31`). |
| `ACC-3` | `NormalBalance` = `NormalBalance ?? (Nature is 1 or 5 ? Debit : Credit)` — منطق واحد في `Account`. |
| `ACC-4` | `FinancialStatement` = `Nature <= 3 ? 1 (ميزانية) : 2 (نتيجة أعمال)`. |
| `ACC-5` | `TreePath` يُبنى في BLL من الآباء (`/1/11/111/1111/`) — **لا عمود**. |
| `ACC-6` | `Balance` يُجمع من `JournalEntryLines` باستعلام واحد مجمَّع في `GetAllAsync`/`GetTreeAsync`، بإشارة الطبيعة. |
| `ACC-7` | `AccountCategoryHelper` في `Business/Helpers/` = البيت الواحد لتصنيف الحساب الخاص (عميل/مورّد/صندوق/بنك/موظف) بقواعد المواصفة — **لا نسخة في الشاشة**. |
| `ACC-8` | كل الحقول الجديدة في `AccountDto`/`AccountTreeNodeDto`/الطلبات **اختيارية بقيم افتراضية في آخر السجل** حتى لا ينكسر أي مستهلك قائم. |
| `ACC-9` | `I*DesktopService`/`Direct*`/`Mock*` تُضاف لها `GetNextChildCodeAsync(parentId)` فقط — لا عضو آخر. |
| `ACC-10` | «تكافؤ الرفض» في الـMock: اسم فارغ · أب غير موجود · أب ورقة · تكرار الاسم تحت نفس الأب · مستوى > 10 · عملة ليست 3 أحرف · محاولة تعديل/حذف حساب نظامي · حذف حساب له أبناء أو حركات. |

## 7. الموجات

| الموجة | الوكيل | المخرَج |
|---|---|---|
| 1 | `database-engineer` | `Account` (3 خصائص + `EffectiveNormalBalance` + `IsSubAccount`) · `AccountConfiguration` · ترحيل إضافي واحد + قراءة SQL |
| 2 | `backend-architect` | DTO + الطلبات + `AccountCategoryHelper` + `AccountService` + مولّد الأكواد + `DirectAccountService` |
| 3 | `ui-agent` | `frmAccounts` + المصمّم + `MockAccountDesktopService` + `MockAccountsData` (شجرة المواصفة كاملة بالحقول الجديدة) |
| 4 | `mock-qa` + `code-reviewer` | شهادة تغطية + مراجعة واحدة |

## 8. سجل الجولات

### الجولة 1 — `2026-11-17` — القياس وفتح السجل
- `docs/database-schema.md` = **1,015** سطراً · قسم `Accounts` في السطر **363** ويعلن **17** عموداً.
- `frmAccounts.cs` = **1,257** سطراً · `frmAccounts.Designer.cs` = **37,140** بايت · **83** عنصراً مُعلناً.
- `AccountService.cs` = **437** سطراً · `AccountCodeGeneratorService.cs` = **94** سطراً · `MockAccountsData.cs` = **248** سطراً.
- `docs/MOCK-COVERAGE.md` سطر 1 = `IAccountDesktopService` … `مؤهل للمراجعة`.
- **فجوة مكتشفة:** بذور `AccountingSeeder` الحقيقية (`1101`…) تخالف شجرة الـMock (`1111`…) — سُجّلت كبند مفتوح، ولم تُمسّ.
- **`Program.cs`:** لا حاجة لتعديل (العقد مسجَّل في فرعي Mock/Direct).

### الجولة 2 — `2026-11-17` — قرار «إزالة العملة» + تحديث وثيقة الهيكل (منفَّذ)

**قرار المالك الجديد (حرفياً):** «remove cuurencey» ⇒ **أُسقط `CurrencyCode` نهائياً**، وصارت الزيادة **عمودين فقط**.

**نطاق هذه الجولة:** `docs/database-schema.md` وحده (طلب المالك: «review and update»).

**ما نُفِّذ في الوثيقة:**

| # | التغيير | الدليل |
|---|---|---|
| 1 | **تصحيح خاطئ في §4.1:** `NameEn` كان موصوفاً `NOT NULL` — والمقيس في الـsnapshot (سطر 77–79) وفي `AccountConfiguration` أنه **`NULL`** | قراءة كتلة `Accounts` (أسطر 25–109) |
| 2 | **§4.1 صار يميّز «المقيس اليوم 17» من «الهدف المعتمد 19»** — كان يخلط أسماء التدقيق النصّية (هدف §2) بعرضها كأنها قائمة | نص §4.1 |
| 3 | إضافة العمودين `AllowCostCenter` · `NormalBalance` بوسم 🔄 «معتمد، لم يُنفَّذ» | نفس الجدول |
| 4 | **§4.1.1 جديد** — السمات المشتقة (`TreePath` · `FinancialStatement` · `IsSubAccount` · `Balance` · `ParentAccountCode` · `CategoryLabel`) صراحةً **بلا أعمدة** | جدول 6 صفوف |
| 5 | **§4.1.2 جديد** — قواعد الشجرة والأكواد + **منع عمود العملة** + منع جدول `CostCenters` في V1 | جدول 6 صفوف |
| 6 | **تصحيح صف مكسور في §9:** صف «أعمدة التدقيق» كان 5 خلايا في جدول 3 أعمدة | §9 صف 7 |
| 7 | إضافة صفَّي انحراف 11 و12 (`Accounts` بلا أعمدة مشتقة · بلا عملة) | §9 |
| 8 | **تصحيح أمر بروتوكول أعلى الملف:** الشكل القديم (`.Matches.Groups`) يرمي استثناءً — استُبدل بأمر صحيح ومُختبَر ⇒ أعاد **40** | `… | Sort-Object -Unique).Count` |
| 9 | `§11` سجل تغييرات الهيكل: صف 3 للحقلين المعتمدين مع نص الموافقة وتاريخها وحالتهما | §11 |
| 10 | `§10` قائمة التحقق: خطوتان جديدتان (عدّ 17 ⇒ 19 · صفر عمود عملة) | §10 |

**القياسات (2026-11-17):**

| القياس | الأمر | الناتج |
|---|---|---|
| الجداول الفريدة | `Select-String … 'ToTable\("([^"]+)"' -AllMatches` | **40** ✓ مطابق للوثيقة |
| خصائص كتلة `Accounts` | عدّ `b.Property<` بين سطر 25 و109 | **17** ✓ مطابق لرقم §4.1 قبل التعديل |
| `NameEn` | `AccountConfiguration`: `.IsRequired(false)` · snapshot بلا `IsRequired` | **NULL** ⇒ الوثيقة كانت خاطئة |
| بناء الحل | `dotnet build .\SalesSystem.slnx` | **1 error** — `User.cs(68)` CS0103 (وقبلها `Expense.cs(62)`) من **موجة `Users` الجارية**، وليست من هذا العمل |
| حالة الشجرة | `git status --porcelain` | **≈160 ملفاً معدَّلاً/محذوفاً** غير مُودَع من الموجة الأخرى |

> **قرار توقّف (مسجَّل):** لا تُفتح موجة كود على `Models/**`/`DataAccess/**` بينما الشجرة (أ) لا تُترجم و(ب) تُكتب من موجة أخرى — كلاهما يخالف `RULE-N158`/`RULE-N159` ويجعل بوابة البناء بلا معنى. الموجات 1–3 تنتظر استقرار الشجرة.

---

### الجولة 3 — `2026-11-17` — تنفيذ الكود بالكامل + مراجعة + شهادة تغطية

**المدخل:** «update all project code 4.1 Accounts — دليل الحسابات الهرمي» + سلّم الأطوال الرسمي (1 · 2 · 4 · 8) + «I will do migrations you write code and stop».

**نُفِّذ عبر 7 وكلاء في ثلاث موجات (مجموعات ملفات منفصلة — مالك واحد لكل ملف):**

| الموجة | الوكيل | الملفات | الحصيلة |
|---|---|---|---|
| 1 | `ui-agent` | `frmAccounts.cs` · `frmAccounts.Designer.cs` | `cmbNormalBalance` + `chkAllowCostCenter` في `pnlFields` · أربعة أعمدة شبكة (المسار · الرصيد · القائمة المالية · التصنيف) · معاينة كود الابن الحيّ عبر `GetNextChildCodeAsync` بحرّاس سباق · **إصلاح عيب قائم:** أعمدة المصمّم كانت معلنة بلا `Columns.Add` فتُولَّد تلقائياً (15 عموداً) و`Cells["colNameAr"]` كان يرمي داخل `catch` |
| 1 | `fast-agent` | `MockCashBoxDesktopService` · `MockData` · `MockFinancialReportDesktopService` · `MockJournalEntriesData` · `MockAccountsData` | إعادة ترقيم كل الأكواد إلى السلّم الرسمي · إضافة `1107` عهد الموظفين · صفر بقايا `1111/1121/1112/2111/1131/2141` |
| 1 | `implement-agent` | `AccountCategoryHelper.cs` | `PrefixMap` من 11 مدخلاً باطلاً إلى **5** صحيحة: `1103` عميل · `2101` مورّد · `1107` موظف · `1102` بنك · `1101` صندوق · إزالة `"موظف"` من `NameHints` (كانت تصنّف «رواتب الموظفين» كعهدة) |
| 2 | `implement-agent` | `AccountDtos.cs` · `AccountService.cs` · `MockAccountDesktopService.cs` | **إصلاح دلالي:** الـDTO صار يحمل **المخزَّن** (`0` = مشتق) مع `EffectiveNormalBalance` مشتقاً — فصار «تلقائي» يعود كما اختاره المالك بدل أن يُثبَّت 1/2 صامتاً |
| 2 | `fast-agent` | `MockData.cs` · `CustomerService.cs` | أكواد الصناديق تطابق `AccountId` (`11010001`/`11010002`) · تعليق `1130` ← `1103` |
| 2 | `fast-agent` | `MockAccountsData.cs` | بذور الـMock تخزّن `normalBalance ?? 0` بدل القيمة المحسوبة |
| 3 | `backend-architect` | `AccountService.cs` · `DirectAccountService.cs` | **رصيد الحساب صار يحتسب القيود المرحَّلة فقط** (`Status == Posted` في استعلام واحد — كان يضم المسودات والملغاة) · الرصيد الافتتاحي يُوقَّع بـ`EffectiveNormalBalance` لا بـ`Nature` · ثلاث رقابات `UpdateAsync` (أب لنفسه · تكرار الاسم · تحويل حساب له أبناء إلى ورقة) · `includeInactive` يعمل في الوضع الحقيقي |
| 3 | `ui-agent` | `MockJournalEntriesData` · `MockCashBoxDesktopService` · `MockAccountDesktopService` · `MockAccountsData` | **`AccountId` البذور أُعيد ربطه بالكود الصحيح** (كان «رأس المال» يشير إلى «أصول متداولة») · توليد كود الصندوق عبر `NextChildCode` (كان يولّد `11010003` المستخدم) · تكافؤ `PermanentDelete`/`OpeningBalance`/الأب غير النشط/`Nature`/نص رسالة الاسم |

**القياسات (2026-11-17، كلها بأمر):**

| القياس | الناتج |
|---|---|
| `dotnet build .\SalesSystem.slnx` | ✅ **0 Errors** · 118 تحذيراً (بوابة البناء مُجتازة) |
| خصائص `Accounts` في اللقطة | **19** (كانت 17) والعمودان في `20261117085853_InitialCreate` |
| `CreatedByUserId` في اللقطة | **0** · `CreatedByUserName` = **27** (موجة الهوية النصية أُنجزت) |
| حسابـات الـMock | **42** صفاً · صفر مخالفة سلّم (1/2/4/8) · صفر كود غير امتداد بادئة أبيه |
| نداءات الرفض في `MockAccountDesktopService` | **26** (كان السجل يقول 8) |
| مراجعة الكود | `0 Blocker · 4 Major · 5 Minor` — **الأربعة Majors أُصلحت** (الرصيد/التوقيع/بذور القيود/كود الصندوق)؛ المتبقي Minors مسجَّلة أدناه |
| شهادة `mock-qa` | `مؤهل جزئياً — تكافؤ الرفض ناقص` — الفجوتان المتبقيتان `ف-AC-1` (حذف نهائي) و`ف-AC-2` (الرصيد الافتتاحي في الـMock) **أُغلقتا في الموجة 3**؛ يلزم إعادة شهادة |

**الترحيل — ملك المالك:** لم يُنشَأ ولا يُحذف ولا يُعاد تسمية أي ترحيل من أي وكيل (قرار المالك «I will do migrations»). العمودان موجودان في اللقطة الأساسية التي بناها المالك؛ ومن يُعيد تأسيس الترحيلات يتحقق أن الحقلين داخل الـ`InitialCreate` (قياس: السطر 32 لـ`AllowCostCenter`).

**بنود مفتوحة (لا تُفتح موجة لها الآن — قرار توقّف):**
1. `AccountService.LoadBalancesAsync` يُفرّغ جدول السطور كاملاً ويجمع في C# (استعلام واحد، لا `GROUP BY` في SQL) — كافٍ لـMVP.
2. انحراف دلالات بذور الـMock مقابل `AccountingSeeder` (`2201` رأس مال في الـMock مقابل `3101` في البذور الحقيقية · `1105` اسمان مختلفان) — الأكواد صحيحة والقصد واحد؛ التوحيد يحتاج مسّ أسماء عربية.
3. فرق تكافؤ عكسي: الـMock يرفض الأب غير النشط في `GetNextChildCodeAsync` بينما الـBLL لا يفحصه هناك.
4. `InitiateSeeded` أرصدة الحسابات في الـMock ثابتة ولا تطابق مجاميع بذور القيود (بيانات تجربة فقط).

