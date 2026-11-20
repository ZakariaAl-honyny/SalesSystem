# TASK-024 — سجل إغلاق «إزالة كل كود المحاسبة» (Registry + Plan)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-024` |
| `العنوان` | **سجل مقيس لكل أثر لفظي/برمجي محاسبي باقٍ في `SalesSystem/**` + خطة إغلاقه بموجات** |
| `الحجم` | **S** — 5 ملفات كود (7 أسطر) + قراران مالك + إصلاحان توثيقيان |
| `المرحلة` | `—` (تنظيف) |
| `الحالة` | **مفتوحة** — `W0` + `W1` + `W2` **مُنفَّذة ومقيسة** · المسح بعدها **113 سطراً في 33 ملفاً** (كلها مبرَّرة أو قرار) · `W3`/`W4` بانتظار قرار المالك |
| `الوكيل المالك` | `fast-agent` (W1) · `fast-agent` واحد للثلاثي المتماثل (W2) · الـOrchestrator للتوثيق |
| `أثر قاعدة البيانات` | `لا شيء` — صفر `DROP` · صفر ترحيل · القاعدة الحيّة لا تُمسّ (`RULE-N198`) |
| `التاريخ` | `2026-11-20` |
| `أمر المالك` | «register in file for tracning all code clear from Accounting code. first review all reamning code and register and remove if you need do work well use many subagetns» + «update `docs/database-schema.md` if you need» |

> **قاعدة هذا السجل:** كل رقم فيه **مقيس بأمر في نفس الجولة** (`RULE-N195`/`RULE-N196`) — والأمر مكتوب معه، فأي مراجعة تُعيد القياس ولا تثق بالرقم.

---

## 1. خط الأساس المقيس (Baseline)

### 1.1 كتالوج الموديل ووجود مجلد الترحيلات

```powershell
# جداول الموديل (أسماء ToTable فريدة)
(Select-String -Path 'SalesSystem/SalesSystem.DataAccess/Configurations/*.cs' `
   -Pattern 'ToTable\("([^"]+)"' -AllMatches |
 Select-Object -ExpandProperty Matches |
 ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique).Count
```

| ما قِيس | الناتج | الحكم |
|---|---|---|
| `ToTable` فريدة | **33** | ✅ مطابق للمخطط المعتمد (`database-schema.md` §3) — صفر جدول محاسبي |
| `Migrations/` ملفات | **0** (المجلد موجود على القرص، **0 ملف، 0 مسار في git**) | ✅ لا ترحيل في المستودع · المجلد فارغ ومحلي فقط |
| `SalesSystem.Models/Accounting/` | **غير موجود** | ✅ محذوف |
| `AccountId` · `ExpenseAccount` · `JournalEntry` · `SystemAccountMapping` · `NormalBalance` · `AllowCostCenter` | **0** نتيجة في `SalesSystem/**` | ✅ صفر سطح بيانات محاسبي |

> **تصحيح توثيقي مطلوب:** `database-schema.md` §8.1 و§11 صف 3 يقولان «**34** جدولاً» — والقياس **33**. مُصحَّح في هذه الموجة.

### 1.2 المسح اللفظي الشامل (38 ملفاً · 119 سطراً)

```powershell
Get-ChildItem 'SalesSystem' -Recurse -Include *.cs,*.resx,*.csproj -File |
  Where-Object { $_.FullName -notmatch '\\obj\\|\\bin\\' } |
  Select-String -Pattern 'account|journal|محاسب|الحسابات|قيود|قيد محاسب' -CaseSensitive:$false
```

**النتيجة: 119 سطراً في 38 ملفاً.** الجرد الكامل بالحكم في §2.

---

## 2. الجرد الكامل — 119 سطراً · 38 ملفاً · سبعة أحكام

**رموز الحكم:**

| الرمز | المعنى | العمل |
|---|---|---|
| `K1` | **كشف حساب الطرف** (عميل/مورّد) — ميزة باقية في V1 | **يبقى** (التسمية `Account*` مقصودة، قرار `D-2`) |
| `K2` | **حساب المستخدم** (تسجيل الدخول/القفل) — ليس محاسبة | **يبقى** |
| `K3` | **حساب = عملية حسابية** (مجاميع/خصم) — لغة عربية سليمة | **يبقى** |
| `K4` | **توقيع «المحاسب»** على ورقة جرد مخزني مطبوعة | **يبقى** (قرار `D-3`) |
| `K5` | **توثيق صحيح لإزالة V1** («لا يوجد ربط حسابي في V1») | **يبقى** |
| `R` | **بقايا حقيقية** — تعليق يكذب أو رسالة مستخدم تذكر آلية محذوفة | **يُزال في `W1`/`W2`** |
| `D` | **قرار مالك مطلوب** | **بانتظار قرار** |

| # | الملف | أسطر | الحكم | التفصيل |
|---|---|---:|---|---|
| 1 | `Business/Services/FinancialReportService.cs` | 23 | `K1` | `AccountStatementDto` · `GetAccountStatementAsync` · `GetSupplierStatementAsync` — كشف حساب الطرف |
| 2 | `Desktop/Services/Mock/MockFinancialReportDesktopService.cs` | 23 | `K1` | نفس العقد في الوضع التجريبي |
| 3 | `Desktop/Services/Direct/DirectFinancialReportService.cs` | 9 | `K1` | غلاف BLL |
| 4 | `Desktop/Screens/frmMain.Designer.cs` | 8 | `K1` | `msReportAccountStatement` = «كشف حساب» |
| 5 | `Desktop/Screens/Reports/Forms/Financial/frmAccountStatementReport.cs` | 5 | `K1` | شاشتان من 31 تقريراً |
| 6 | `Business/Interfaces/Services/IFinancialReportService.cs` | 4 | `K1` | عضوَا العقد |
| 7 | `Desktop/Screens/Reports/Forms/Financial/frmAccountStatementReport.Designer.cs` | 3 | `K1` | مصمّم الشاشة |
| 8 | `Desktop/Screens/frmMain.cs` | 2 | `K1` | معالج الفتح |
| 9 | `Desktop/Services/Interfaces/IFinancialReportDesktopService.cs` | 2 | `K1` | عقد Desktop |
| 10 | `DataAccess/Services/ReportExportService.cs` | 2 | `K1` | مفتاح تصدير `accountstatement` |
| 11 | `DTO/Responses/AccountStatementDto.cs` | 1 | `K1` | نوع السطر |
| 12 | `Desktop/Program.cs` | 1 | `K1` | `AddTransient<frmAccountStatementReport>` |
| 13 | `Desktop/Helpers/ReportNavigationHelper.cs` | 1 | `K1` | تصنيف «التقارير المالية» |
| 14 | `Desktop/Helpers/ReportUiStandardizer.cs` | 1 | `K1` | مرجع التخطيط |
| 15 | `Models/User.cs` | 4 | `K2` | «حساب الأدمن» · «قفل الحساب» |
| 16 | `Business/Services/AuthService.cs` | 4 | `K2` | فحص قفل الحساب |
| 17 | `Desktop/Services/Mock/MockSecurityData.cs` | 2 | `K2` | مستخدم تجريبي `accountant1` |
| 18 | `DTO/Common/ErrorCodes.cs` | 1 | `K2` | `AccountLocked` |
| 19 | `Desktop/Screens/frmLogin.Designer.cs` | 1 | `K2` | «أي حساب من الحسابات» |
| 20 | `Desktop/Services/Mock/MockAuthDesktopService.cs` | 1 | `K2` | `ErrorCodes.AccountLocked` |
| 21 | `Desktop/Screens/All Invoices Screens/Sales/frmAddUpdateSalesInvoice.Designer.cs` | 2 | `K3` | تلميحا `btnSaveDraft`/`btnPost` — **يقولان صراحة «لا قيد محاسبي»** |
| 22 | `Desktop/Screens/All Invoices Screens/Sales/frmAddUpdateSalesInvoice.cs` | 2 | `K3` | «حساب الخصم» + «لا قيد محاسبي في V1» |
| 23 | `Desktop/Services/Mock/MockData.cs` | 1 | `K3` | «التحقق من صحة الحسابات المالية» = المجاميع |
| 24 | `Desktop/Services/Mock/MockPurchasesData.cs` | 1 | `K3` | نفس المعنى |
| 25 | `Desktop/Services/Mock/MockSalesInvoiceDesktopService.cs` | 1 | `K3` | «بناء السطور والحسابات» |
| 26 | `Desktop/Services/Mock/MockPurchaseInvoiceDesktopService.cs` | 1 | `K3` | نفس المعنى |
| 27 | `Desktop/Screens/Inventory/Forms/frmStocktakingPrintAdjustment.cs` | 1 | `K4` | وصف «سند تسوية مخزنية» |
| 28 | `Desktop/Screens/Inventory/Forms/frmStocktakingPrintAdjustment.Designer.cs` | 1 | `K4` | سطر توقيع «المحاسب» على الورقة |
| 29 | `Desktop/Helpers/ThemePalette.cs` | 1 | `K5` | اسم جيل لوحة ألوان تاريخي «المحاسب سوفت» |
| 30 | `Business/Services/SalesService.cs` | 1 | `K5` | «no account link in V1» |
| 31 | `DataAccess/Repositories/ReportRepository.cs` | 1 | `K5` | «balances come from invoices» |
| 32 | **`Business/Services/SupplierService.cs`** | **2** | **`R`** | سطر 96 «entity+**account** modification» · سطر 101 «Capture old values for **Account sync**» — لا مزامنة حسابات في V1 |
| 33 | **`Business/Services/CustomerService.cs`** | **1** | **`R`** | سطر 94 «Capture old values for **Account sync** BEFORE transaction» + المتغير `oldName` **ميت** |
| 34 | **`Business/Services/CashBoxService.cs`** | **1** | **`R`** | سطر 338 رسالة للمستخدم: «لا توجد آلية **سندات أو قيود** لتسجيل العملية» |
| 35 | **`Desktop/Services/Direct/DirectCashBoxService.cs`** | **1** (+1 تعليق) | **`R`** | سطر 96 تعليق + سطر 100 نفس الرسالة (يجب أن تطابق BLL حرفياً) |
| 36 | **`Desktop/Services/Mock/MockCashBoxDesktopService.cs`** | **1** (+1 تعليق) | **`R`** | سطر 99 تعليق + سطر 103 نفس الرسالة (تكافؤ رفض `RULE-N167`) |
| 37 | `Common/Enums/UserRole.cs` | 1 | **`D`** | `Accountant = 3` — وسم جلسة فقط (§2.13) |
| 38 | `DTO/Enums/CoreEnums.cs` | 1 | **`D`** | `Accountant = 3` مكرَّر في طبقة DTO |
| | **الإجمالي** | **119** | | `K1` **85** · `K2` **13** · `K3` **8** · `K4` **2** · `K5` **3** · `R` **6** · `D` **2** |

**مطابقة الأعداد (`RULE-N201`):** `85 + 13 + 8 + 2 + 3 + 6 + 2 = 119` ✅ يساوي إجمالي المسح.

---

## 3. مجموعة البقايا المطلوب إزالتها — قائمة مغلقة (5 ملفات · 7 أسطر)

| # | الملف | السطر | النص الحالي | النص المستهدف |
|---|---|---|---|---|
| `R-1` | `Business/Services/CustomerService.cs` | 94–96 | `// Capture old values for Account sync BEFORE transaction` + `var oldName = customer.Name;` | `// التقاط الحالة السابقة لمزامنة الإيقاف/التنشيط` وحذف `oldName` الميت (لا قارئ له) |
| `R-2` | `Business/Services/SupplierService.cs` | 96 | `// ---- entity+account modification wrapped in atomic transaction ----` | `// ---- تعديل المورد داخل معاملة ذرّية ----` |
| `R-3` | `Business/Services/SupplierService.cs` | 101 | `// Capture old values for Account sync BEFORE update` | `// التقاط الحالة السابقة لمزامنة الإيقاف/التنشيط` |
| `R-4` | `Business/Services/CashBoxService.cs` | 338 | `"التحويل بين الصناديق غير مدعوم حالياً — لا توجد آلية سندات أو قيود لتسجيل العملية"` | `"التحويل بين الصناديق غير مدعوم في هذا الإصدار — لا يوجد مستند تحويل نقدية بين صندوقين"` |
| `R-5` | `Desktop/Services/Direct/DirectCashBoxService.cs` | 96 + 100 | تعليق + **نفس الرسالة** | نفس النص الجديد حرفياً |
| `R-6` | `Desktop/Services/Mock/MockCashBoxDesktopService.cs` | 99 + 103 | تعليق + **نفس الرسالة** | نفس النص الجديد حرفياً |

> **`R-4`/`R-5`/`R-6` نص واحد في ثلاثة بيوت — لا يُصلَح منفرداً.** الرسالة التي يراها المستخدم في الوضع الحقيقي وفي وضع التجربة يجب أن تبقى **متطابقة حرفياً** (`RULE-N167`)، ولذلك يملكها **وكيل واحد** في موجة واحدة.

---

## 4. قرارات المالك المطلوبة (لا يُنفَّذ شيء منها بلا نص)

| # | البند | الخيارات | التوصية |
|---|---|---|---|
| `D-1` | **`frmSupplierStatement`** — شاشة 805 سطراً، **صفر استدعاء خدمة** في الوضع الحقيقي (تبقى فارغة للأبد)، بذور تجريبية 8 أسطر، **لا يفتحها أي عنصر قائمة**، ومسجَّلة في DI (`Program.cs:371`). ووظيفتها (كشف حساب المورّد) **مغطّاة فعلاً** بـ`frmAccountStatementReport` | (أ) حذفها + تسجيلها من DI (`RULE-N102` — كود ميت مكرَّر) · (ب) ربطها بالخدمة وإضافتها للقائمة | **(أ)** — لا تُنشئ محرّراً ثانياً لعرض واحد |
| `D-2` | **تسمية `AccountStatement*`** — الوظيفة كشف حساب طرف لا حساب محاسبي. إعادة التسمية إلى `PartyStatement*` = **M**: عقد + `Direct` + `Mock` + شاشة + مصمّم + DI + قائمة + كتالوج + تصدير (8 ملفات، 85 سطراً) | (أ) إبقاء التسمية (تغيير مكلف بلا مكسب وظيفي) · (ب) إعادة تسمية كاملة في موجة مستقلة | **(أ)** الآن · **(ب)** عند أول مساس بالتقارير |
| `D-3` | **«توقيع المحاسب»** في ورقة الجرد المطبوعة `frmStocktakingPrintAdjustment` | (أ) يبقى (توقيع بشري على ورقة) · (ب) يُستبدل بـ«توقيع المسؤول» | **(أ)** |
| `D-4` | **`Accountant = 3`** في `UserRole` و`CoreEnums` — وسم جلسة فقط، لا نظام صلاحيات (§2.29 `RULE-N105`) | (أ) يبقى (دور بشري محتمل) · (ب) يُزال من التعدادين | **(أ)** — إزالة قيمة enum قد تكسر بيانات مستخدم محفوظة |

> `D-1` هي الحالة الوحيدة التي فيها **عيب فعلي** (شاشة لا تُفتح + وضع حقيقي فارغ دائماً + تكرار وظيفي)، والباقي تسميات مقصودة.

---

## 5. موجات التنفيذ (Wave Plan)

| الموجة | النطاق | الوكيل | الملفات (مغلقة) | البوابة |
|---|---|---|---|---|
| **W0** | تصحيح توثيقي: `34 → 33` في `database-schema.md` §8.1 و§11 · إضافة سطر عن `Migrations/` الفارغ · تلطيف فحص §10 بند 7 | **الـOrchestrator** (docs فقط) | `docs/database-schema.md` · هذا الملف · `ACTIVE.md` | لا بناء (توثيق) |
| **W1** | `R-1`…`R-3` — تعليقات BLL كاذبة + متغيّر ميت | **`fast-agent` #1** | `Business/Services/CustomerService.cs` · `Business/Services/SupplierService.cs` | `dotnet build` = 0 Errors |
| **W2** | `R-4`…`R-6` — رسالة رفض التحويل في **ثلاثة بيوت بنص واحد** | **`fast-agent` #2** (مالك واحد للثلاثة — لا تُقسَّم) | `Business/Services/CashBoxService.cs` · `Desktop/Services/Direct/DirectCashBoxService.cs` · `Desktop/Services/Mock/MockCashBoxDesktopService.cs` | بناء + `grep 'آلية سندات أو قيود'` = **0** في الثلاثة · النص الجديد **3 نتائج متطابقة** |
| **W3** | `D-1` إن وافق المالك — حذف `frmSupplierStatement` (3 ملفات) + سطر DI | `fast-agent` #3 | `Screens/Purchases/Forms/frmSupplierStatement.*` (**حذف**) · `Program.cs:371` | بناء + `grep frmSupplierStatement` = 0 |
| **W4** | `D-2` إن اختار المالك (ب) — إعادة تسمية `AccountStatement*` → `PartyStatement*` | `ui-agent` + `backend-architect` بالتوالي | 8 ملفات + `AccountStatementDto` → `PartyStatementDto` | بناء + صفر مرجع للاسم القديم |

**قاعدة التعايش (`RULE-N158`):** `TASK-023` نقل شاشات الفواتير كان **جارياً في الشجرة** عند كتابة هذا السجل — لذلك `W1`/`W2` لا تلمس أي ملف تحت `Screens/All Invoices Screens/**` ولا `Screens/{Sales,Purchases,Returns,POS}/Forms/**` ولا `ReportNavigationHelper.cs` ولا `ReportUiStandardizer.cs` ولا `SalesSystem.Desktop.csproj`.

---

## 6. بوابات الإغلاق

| # | البوابة | الأمر | المتوقع |
|---|---|---|---|
| 1 | بناء | `dotnet build .\SalesSystem.slnx` | **0 Errors** (التحذيرات تُبلَّغ ولا تُخفى) |
| 2 | بقايا R | `Select-String -Pattern 'Account sync\|entity\+account\|آلية سندات أو قيود' SalesSystem -Recurse -Include *.cs` | **0** |
| 3 | تكافؤ الرسالة | `Select-String -Pattern 'مستند تحويل نقدية' -Recurse -Include *.cs` | **3** نتائج متطابقة (BLL · Direct · Mock) |
| 4 | كل الأسطر الباقية مبرَّرة | إعادة تشغيل مسح §1.2 | **119 − 7 = 112** سطراً، كلها بـ`K1…K5` أو `D` موثَّق |
| 5 | الأسطح المحاسبية | `AccountId\|JournalEntry\|SystemAccountMapping\|Models/Accounting` | **0** |
| 6 | التوثيق | `Select-String -Pattern '34 جدول' docs/database-schema.md` | **0** |

---

## 7. سجل الأدلة (يُملأ مع كل موجة)

| الموجة | التاريخ | الأمر | المخرج | الالتزام |
|---|---|---|---|---|
| `W0` | 2026-11-20 | مسح §1.1 + §1.2 | 33 جدولاً · 119 سطراً / 38 ملفاً | — |
| `W1` | 2026-11-20 | `Select-String 'Account sync\|oldName'` على `CustomerService.cs` + `SupplierService.cs` | **0** · `git diff --stat` = `3 insertions(+), 4 deletions(-)` · `oldIsActive` سليمة (Customer:95/110 · Supplier:102/114) | — |
| `W2` | 2026-11-20 | `Select-String 'مستند تحويل نقدية'` في `SalesSystem/**` | **3** نتائج متطابقة (BLL:338 · Direct:100 · Mock:103) · `آلية سندات أو قيود` = **0** | — |
| بناء | 2026-11-20 | `dotnet build .\SalesSystem.slnx --no-incremental` | **0 Errors / 112 Warning(s)** (التحذيرات غير مقارنة بموجة سابقة — `§12`: وضع البناء الكامل) | — |
| إعادة مسح | 2026-11-20 | نفس أمر §1.2 | **113 سطراً في 33 ملفاً** = `119 − 6` سطراً · `38 − 5` ملفاً ✅ مطابقة تامة | — |
| بوابات | 2026-11-20 | `AccountId\|ExpenseAccount\|JournalEntry\|SystemAccountMapping\|NormalBalance\|AllowCostCenter` · `ToTable` · «34 جدولاً» في المخطط | **0** · **33** · **0** | — |

---

## 8. سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء السجل بجرد مقيس بالكامل (119 سطراً · 38 ملفاً · 7 أحكام · مطابقة أعداد) + خطة موجات + 4 قرارات مالك |
| `2026-11-20` | **`W0`** تصحيح `database-schema.md` (34→33 في §8.1 و§11 · `Migrations/` = 0 ملف · فحص §10 بند 7 صار دقيقاً) + سطر `TASK-024` في `ACTIVE.md` و`TASKS/INDEX.md` |
| `2026-11-20` | **`W1`** حذف تعليقات «Account sync» الكاذبة + المتغيّر الميت `oldName` في `CustomerService`، وتصحيح تعليقي `SupplierService` |
| `2026-11-20` | **`W2`** رسالة رفض التحويل بين الصناديق: نص جديد واحد في BLL + Direct + Mock (تكافؤ حرفي) |

---

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31).
