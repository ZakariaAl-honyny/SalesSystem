# TASK-020 — إزالة كل بقايا المحاسبة من MVP V1 (تحقّق + تنظيف)

| الحقل | القيمة |
|---|---|
| الحالة | **مغلقة تقنياً — جولتان** (`2026-11-18`): صفر كود محاسبي بالقياس · بقايا خدمة التصدير والتعليقات أُزيلت · السجلات أُرشفت · التوثيق مطابق للقياس (§5) |
| أمر المالك (نصّه) | «and also after finish from tax remove each and everything about Accounting code from MVP v1» |
| التاريخ | `2026-11-18` |
| الحجم | **S/M** — جولة 1 توثيق فقط · جولة 2: **15 ملف كود** (فروع تصدير ميتة + تعليقات، صفر تغيير سلوك) + توثيق + أرشفة 5 سجلات |
| يعتمد على | `TASK-011` (إزالة المحاسبة 2026-11-17: 84 ملفاً محذوفاً · 72 معدّلاً · بناء 0 أخطاء) |

---

## 1) القياس أولاً (`RULE-N195`/`RULE-N196`)

```powershell
Get-ChildItem SalesSystem -Recurse -Include *.cs |
  Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' -and $_.FullName -notmatch 'SalesSystem.Tests' } |
  ForEach-Object { Select-String -Path $_.FullName -Pattern 'AccountId|ExpenseAccountId|JournalEntry|SystemAccountMapping|ReceiptVoucher|PaymentVoucher|frmAccounts|AccountService|accounting|محاسب' -CaseSensitive:$false }
```

**النتيجة: صفر عضو كود محاسبي.** كل المطابقات (23 موضعاً) هي:

| النوع | العدد | أمثلة |
|---|---|---|
| تعليقات توثيق «المحاسبة مؤجلة إلى V2» | 9 | `PurchaseService.cs:490` · `SupplierPaymentService.cs:144` · `ReportRepository.cs:695` · `DirectCashBoxService.cs:96` · `MockCashBoxDesktopService.cs:44,100` · `CashTransferRequest.cs:6` |
| `frmAccountStatementReport` = **كشف حساب** (تقرير مالي من الفواتير، ليس وحدة محاسبة) | 6 | `ReportNavigationHelper.cs:152` · `Program.cs:299` · `frmMain.cs:451` |
| نص عربي وصفي («قيد محاسبي» في تلميحات/طباعة) | 5 | `frmAddUpdateSalesInvoice.Designer.cs:190,206` · `frmStocktakingPrintAdjustment.cs:13` |
| `AccountId` في الموديل | **0** | — |

⇒ **لا يوجد كود محاسبي يُزال.** البقية كلام لا سلوك.

## 2) ما أُصلح (توثيق فقط)

| الملف | التصحيح |
|---|---|
| `docs/database-schema.md` | حُذفت **4 صفوف أعمدة** لم تعد في الموديل: `Customers.AccountId` · `Suppliers.AccountId` · `CashBoxes.AccountId` · `Expenses.ExpenseAccountId` + تصحيح الأعداد (14 · 14 · 8 · 13) |
| `docs/database-schema.md` §4 | الملاحظتان 155/382 كانتا تقولان «ما زالا في الـsnapshot» ⇒ صارتا «أُزيلا من الموديل والتهيئة 2026-11-17 · باقيان على القاعدة الحيّة فقط» |
| `docs/database-schema.md` §8.1 | الصفان 6 و7 صارا «✅ مُزَال من الموديل · باقٍ على القاعدة الحيّة» |
| `docs/database-schema.md` §6 | تصحيح معادلة الشراء `UnitCost` → `UnitPrice` (عيب توثيقي مُكتشف سابقاً — العمود `UnitPrice` في `PurchaseInvoiceLine.cs:13`) |

## 3) خارج النطاق (بسبب)

- **القاعدة الحيّة:** إسقاط 4 أعمدة + 6 جداول (`Accounts` · `JournalEntries` · `JournalEntryLines` · `ReceiptVouchers` · `PaymentVouchers` · `SystemAccountMappings`) **يبقى للمالك** بموافقة `DROP` مكتوبة (`RULE-N198`) — الترحيلات أُزيلت كلها بأمره.
- `docs/database-schema-v2-deferred.md`: يحمل **تصميم** الجداول المحاسبية لـV2 — يبقى (`TASK-011`).
- `docs/acounts.md`: مستند تحليلي سابق — لم يُمسّ (لا كود).

## 4) الحكم

**لا شيء محاسبي في V1 كوداً.** البوابة: بناء الحل **0 Errors** بعد `TASK-019`، وصفر مرجع في `Models/**`.

---

## 5) الجولة 2 — 2026-11-18 (إعادة أمر المالك: «remove each and everything about Accounting code from MVP v1»)

### 5.1 القياس قبل العمل (`RULE-N195`/`RULE-N196`)

| الفحص | الأمر | الناتج |
|---|---|---|
| الكيانات/التهيئات | عدّ `ToTable` في `SalesSystem.DataAccess/Configurations/*.cs` | **33** جدولاً (بعد إزالة `ProductPrices` — `TASK-021`) · **صفر** محاسبي |
| `Migrations/` · `ModelSnapshot` | `Get-ChildItem SalesSystem\SalesSystem.DataAccess\Migrations` | **المجلد غير موجود** |
| الخدمات/العقود/الشاشات المحاسبية | `Select-String 'JournalEntry\|SystemAccountMapping\|ReceiptVoucher\|PaymentVoucher\|IAccountDesktopService\|AccountService\|frmAccounts\|AccountId\b\|AccountingSeeder\|AccountingIntegration'` على `*.cs` (بلا `bin`/`obj`) | **0** |
| الباقي الوحيد المطابق | نفس الأمر أعلاه | `frmAccountStatementReport` (كشف حساب عميل/مورّد من الفواتير — **تقرير تشغيلي**) |

⇒ **لا كود محاسبي يُزال.** الباقي كان: **بقايا في خدمة تصدير ميتة + تعليقات + توثيق + سجلات مهام**.

### 5.2 ما أُنجز في هذه الجولة

| # | الملف/الموضع | العمل | الدليل |
|---|---|---|---|
| 1 | `SalesSystem.DataAccess/Services/ReportExportService.cs` | حذف **38 سطراً**: `case "balancesheet"` · `case "trialbalance"` + بنّاءَيهما `BuildBalanceSheetTable` · `BuildTrialBalanceTable` + أسماء `"accountbalances"` · `"generalledger"` · `"workingcapital"` من قائمة النوع والأسماء العربية | `git diff --stat` = `1 file changed, 38 deletions(-)` · `grep` للمفاتيح الخمسة = **0** في الحل كله |
| 2 | 6 ملفات باك-إند | تعليقات/XML-doc فقط (صفر تغيير سلوك): `SalesService` · `SupplierPaymentService` · `PurchaseService` · `DocumentEntity` · `ReportRepository` · `CashTransferRequest` | `grep 'accounting\|journal\|account mapping'` = **0** · كل سطر مُضاف يبدأ بـ`//` أو `///` |
| 3 | 8 ملفات Desktop-service | تعليقات/XML-doc فقط: `IFinancialReportDesktopService` · `DirectFinancialReportService` · `MockFinancialReportDesktopService` · `DirectCashBoxService` · `MockCashBoxDesktopService` · `MockSuppliersData` · `MockExpensesData` · `MockData` | `grep 'محاسب\|Accounting\|قيد مزدوج\|دليل حسابات\|journal'` = **0** · صفر نص عربي مرئي للمستخدم تغيّر |
| 4 | `docs/database-schema.md` | إزالة كل محتوى المحاسبة: جدولها في §3 · تعريفاتها · تعداداها · قاعدة القيد المزدوج · §8.1 القديم · صفوف §9 · صف السجل #3 · ادعاءات «snapshot 40» | الأعداد أُعيد قياسها: **33** جدولاً · `Migrations/` غير موجود · لا يُستشهد بعدد من snapshot |
| 5 | `docs/MOCK-COVERAGE.md` → **v2.0** | حذف صفّي العقدين المحاسبيين · قسم «تغيير V3» · صف وحدة Accounting · فجوات `ف-AC-1…9` (بقيت `ف-AC-4` مُعاد ترقيمها `ف-7` لأنها انحراف سجل لا محاسبة) · §9 صار «خارج نطاق V1» · تصحيح الأعداد بالقياس: **31** عقداً · **31** Mock · **31** تسجيل Mock · **29** Direct · **44** ملفاً في `Services/Mock/` · الحكم **18 مؤهل / 13 جزئي / 0 غير مؤهل** | كل رقم مقيس بأمر في 2026-11-18 |
| 6 | سجلات المهام/الميزات | **أرشفة بـ`git mv`** (لا حذف — `RULE-N202`): `TASK-008` · `TASK-009` · `TASK-010` · `F-003-accounts-tree/` → `docs/TASKS/ARCHIVE/` · و`docs/acounts.md` → `docs/archive/acounts.md` | `git status` يظهر `R` لكل ملف + صفوف في `docs/TASKS/ARCHIVE/INDEX.md` (§2 صفوف 13–16 + الفهرس العكسي) + تحديث `TASKS/INDEX.md` و`FEATURES/INDEX.md` |

### 5.3 البناء

`dotnet build .\SalesSystem.slnx` (تشغيل تزايدي) ⇒ **0 Errors · 94 Warnings**. البوابة **0 Errors** محققة؛ عدد التحذيرات لا يُقارن بين بناء تزايدي وكامل.

### 5.4 خارج النطاق (بسبب)

- **القاعدة الحيّة:** إسقاط بقايا الهيكل القديم (أعمدة/جداول) **للمالك** بموافقة `DROP` مكتوبة (`RULE-N198`) — لا وكيل ينفّذ `DROP` أو `dotnet ef database update`.
- **`frmAccountStatementReport`:** يبقى — **كشف حساب عميل/مورّد** يُبنى من الفواتير والسندات، لا من دليل حسابات. أي طلب لإزالته قرار منتج مستقل.
- **تطبيق التحليل غير المتعقَّب** `docs/all new Anylysis for update system features/salessystemanlysis_app/`: غير مُدار بـgit (`git ls-files` = 0) ⇒ **لم يُمسّ** — حذفه غير قابل للاسترجاع، وليس جزءاً من حل MVP.
- **`docs/database-schema-v2-deferred.md`:** يحمل تصميم V2 (ومنها المحاسبة) — باقٍ بقرار المالك («Tax to v2»).

### 5.5 الحكم

**V1 خالٍ من كود المحاسبة بالقياس** (0 مرجع في `SalesSystem/**` عدا كشف الحساب التشغيلي)، وسجلاتها أُرشفت لا حُذفت، والتوثيق صار مطابقاً للقياس.
