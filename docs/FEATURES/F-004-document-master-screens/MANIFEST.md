# F-004 — الجرد (MANIFEST) — نهائي

> **الجرد بأمر لا بالذاكرة** (`RULE-N201`). كل صف = ملف مسمّى. لا صف «باقي الملفات».
> آخر تحديث: **2026-11-18** — بعد موجة الإغلاق (البناء أخضر).

## الأعداد (مقيسة 2026-11-18)

| البند | العدد | الأمر |
|---|---|---|
| **المكتشَف (N)** | **21** | `Get-ChildItem -Recurse -Path …\Screens\Returns,…\Expenses,…\CustomerReceipts -Include *.cs,*.resx` |
| **داخل النطاق** | **21** (7 شاشات × 3) | — |
| **معفى (بسبب)** | **0** | — |
| **يُنشأ** | **12** (4 شاشات × 3) | ✅ |
| **يُعدَّل** | **7** | `Program.cs` · `frmMain.cs` · `frmMain.Designer.cs` · `Global Classes/clsGlobal.cs` · `frmCashBoxTransactions.cs` · `frmListPayments.cs` · `frmAddUpdateSalesInvoice.cs` |
| **يُحذف** | **21** | ✅ |
| **عمليات الموجة** | **40** = 12 + 7 + 21 | ✅ |

**المطابقة:** `21 = 21 + 0` ✅ · `40 = 12 + 7 + 21` ✅

## المنشأ (12 ملفاً — كلها ✅ مقيسة)

| # | الملف | الأسطر | البايت | الموجة | الحالة |
|---|---|---|---|---|---|
| 1 | `Screens/Expenses/Forms/frmExpensesMaster.cs` | **974** | 51,540 | W1-A | ✅ |
| 2 | `Screens/Expenses/Forms/frmExpensesMaster.Designer.cs` | **1,031** | 45,772 | W1-A | ✅ |
| 3 | `Screens/Expenses/Forms/frmExpensesMaster.resx` | — | resx قياسي | W1-A | ✅ |
| 4 | `Screens/CustomerReceipts/Forms/frmCustomerReceiptsMaster.cs` | **886** | 47,053 | W1-B | ✅ |
| 5 | `Screens/CustomerReceipts/Forms/frmCustomerReceiptsMaster.Designer.cs` | **971** | 43,966 | W1-B | ✅ |
| 6 | `Screens/CustomerReceipts/Forms/frmCustomerReceiptsMaster.resx` | — | resx قياسي | W1-B | ✅ |
| 7 | `Screens/Returns/Forms/frmSalesReturnMaster.cs` | **1,173** | 66,080 | W2-C | ✅ |
| 8 | `Screens/Returns/Forms/frmSalesReturnMaster.Designer.cs` | **1,126** | 52,040 | W2-C | ✅ |
| 9 | `Screens/Returns/Forms/frmSalesReturnMaster.resx` | — | resx قياسي | W2-C | ✅ |
| 10 | `Screens/Returns/Forms/frmPurchaseReturnMaster.cs` | **993** | 50,325 | W2-D | ✅ |
| 11 | `Screens/Returns/Forms/frmPurchaseReturnMaster.Designer.cs` | **1,147** | 54,911 | W2-D | ✅ |
| 12 | `Screens/Returns/Forms/frmPurchaseReturnMaster.resx` | — | resx قياسي | W2-D | ✅ |

## المعدَّل (7 ملفات — كلها ✅)

| # | الملف | التغيير | الموجة |
|---|---|---|---|
| 13 | `Program.cs` | 4 تسجيلات مضافة · 7 محذوفة · `using` المجال المصحَّح | W3-E |
| 14 | `Screens/frmMain.cs` | 4 معالجات أُعيد توجيهها · `msbtnReturnsList_Click` محذوف | W3-E |
| 15 | `Screens/frmMain.Designer.cs` | `msbtnReturnsList` أُزيل بالكامل (حقل + تهيئة + إعداد + إضافة) | W3-E |
| 16 | `Screens/CashBoxes/Forms/frmCashBoxTransactions.cs` | موضعان: `ShowDialog` ← `clsGlobal._OpenForm(...) as frm*` + `SelectRecord` | W3-E + إصلاح مراجعة |
| 17 | `Screens/Payments/Forms/frmListPayments.cs` | موضعان: إضافة/تعديل ← فتح الشاشة المدمجة + `SelectRecord` | W3-E + إصلاح مراجعة |
| 18 | `Screens/Sales/Forms/frmAddUpdateSalesInvoice.cs` | زر «مردود بيع» ← `frmSalesReturnMaster` عبر `clsGlobal._OpenForm` (سطران) | W3-E (انحراف مُعلَن — انظر `D-08`) |
| 19 | `Global Classes/clsGlobal.cs` | `_OpenForm` يُعيد `Form?` (النافذة الحيّة) — إصلاح Major من المراجعة (`RULE-N133`) | إصلاح مراجعة |

## المحذوف (21 ملفاً — كلها ✅)

| # | الملف | الأسطر (قبل) | البايت |
|---|---|---|---|
| 20-22 | `Returns/Forms/frmListReturns.{cs,Designer.cs,resx}` | 501 · 774 | 26,259 · 37,133 · 6,312 |
| 23-25 | `Returns/Forms/frmSalesReturn.{cs,Designer.cs,resx}` | 519 · 615 | 31,815 · 29,380 · 6,145 |
| 26-28 | `Returns/Forms/frmPurchaseReturn.{cs,Designer.cs,resx}` | 516 · 616 | 33,710 · 29,454 · 6,341 |
| 29-31 | `Expenses/Forms/frmListExpenses.{cs,Designer.cs,resx}` | 619 · 629 | 29,218 · 27,585 · 6,340 |
| 32-34 | `Expenses/Forms/frmAddUpdateExpense.{cs,Designer.cs,resx}` | 377 · 419 | 21,788 · 16,877 · 6,340 |
| 35-37 | `CustomerReceipts/Forms/frmListCustomerReceipts.{cs,Designer.cs,resx}` | 562 · 638 | 27,205 · 28,427 · 6,144 |
| 38-40 | `CustomerReceipts/Forms/frmAddUpdateCustomerReceipt.{cs,Designer.cs,resx}` | 420 · 451 | 24,660 · 18,188 · 6,340 |

## فحوص البقايا (نُفِّذت — المخرج الفعلي 2026-11-18)

| النمط | النتيجة | الحكم |
|---|---|---|
| أسماء الشاشات السبع في `SalesSystem.Desktop/**/*.cs` | **9 مطابقات كلها تعليقات/XML-doc** (`DocumentTextHelper.cs:24,26` · `frmCustomerReceiptsMaster.cs:15,16` · `frmExpensesMaster.cs:13,23,127` · `frmPurchaseReturnMaster.cs:18` · `frmSalesReturnMaster.cs:15`) | ✅ صفر مرجع كود |
| `SalesSystem.Desktop.CustomerReceipts.Forms` (المجال الميت) | **0** | ✅ |
| `MessageBox.Show` في الشاشات الأربع | **0** | ✅ |
| `_ValidateForm`/`_RequiredFieldsValid`/`_FocusFirstMissingField` | **0** | ✅ |
| `SystemColors.Control` | **0** | ✅ |
| `WireEnterAsTab(this)` أحادي الوسيط | **0** | ✅ |
| `row.Cells["` | **0** | ✅ |
| `((int)(((byte` | **0** | ✅ |
| `msbtnReturnsList` | **0** | ✅ |
| `SelectRecord` على نتيجة `_OpenForm` بلا cast | **0** | ✅ |

## لوحة الوكلاء (نهائية)

| الموجة | الوكيل | الشرائح | الملفات | النتيجة | التاريخ |
|---|---|---|---|---|---|
| W1 | `ui-agent` A · B | S1 · S2 | 1‑6 | ✅ 2/2 | 2026-11-18 |
| W2 | `ui-agent` C · D | S3 · S4 | 7‑12 | ✅ 2/2 | 2026-11-18 |
| W2+ | `fast-agent` | إصلاح بناء خارج النطاق (`frmSupplierSelection`) | ملف 1 (ليس من F-004) | ✅ | 2026-11-18 |
| W3 | `ui-agent` E | S5 | 13‑15 + 21 حذفاً | ✅ | 2026-11-18 |
| W3+ | `ui-agent` | إصلاح أثر الحذف | 16 · 17 + `SelectRecord` في مسترين | ✅ | 2026-11-18 |
| W3++ | `fast-agent` | `frmAddUpdateSalesInvoice` (مرجع ميت) · casts | 18 + تصحيح 3 أسطر | ✅ | 2026-11-18 |
| W4 | `code-reviewer` | المراجعة | — | ✅ 0 Blocker · 1 Major (أُغلق) · 6 Minor (2 أُغلقا · 4 مسجَّلة) | 2026-11-18 |
