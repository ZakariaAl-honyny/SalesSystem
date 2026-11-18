# TASK-020 — إزالة كل بقايا المحاسبة من MVP V1 (تحقّق + تنظيف)

| الحقل | القيمة |
|---|---|
| الحالة | **مغلقة تقنياً** — الكود نظيف بالقياس؛ واضح فقط تنظيف توثيقي أُنجز في الموجة نفسها |
| أمر المالك (نصّه) | «and also after finish from tax remove each and everything about Accounting code from MVP v1» |
| التاريخ | `2026-11-18` |
| الحجم | **S** (توثيق فقط — لا ملف كود واحد) |
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
