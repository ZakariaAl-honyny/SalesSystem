# F-004 — تحويل شاشات المستندات إلى `frm*Master` (المرتجعات · المصروفات · سندات القبض)

| الحقل | القيمة |
|---|---|
| `ID` | `F-004` |
| `الحجم` | L (أربع شاشات ثقيلة + شريحة ربط) |
| `المرحلة` | `—` (واجهة فقط — لا مسار بيانات جديد، فلا مرحلتا A/B) |
| `المالك` | الـOrchestrator (التقسيم والتوثيق) · `ui-agent` (كل شاشة على حدة) · `code-reviewer` (مرة واحدة في النهاية) |
| `الوكيل المنفّذ` | `ui-agent` — وكيل واحد لكل شاشة (`RULE-N156`) |
| `الموافقة المعمارية` | `مؤكَّدة` — نصّ المالك في `DECISIONS.md` `D-01`/`D-02` |
| `تاريخ الفتح` | `2026-11-18` |

## ما تطلبه المالك حرفياً

> «Returns & Expenses & CustomerReceipts Convert to frm*Master scrren like frmProductMaster.»

وبعد قياس حقيقة أن «Returns» مستندان لا مستند واحد، اختار:

> «Two Masters (sales + purchase) (Recommended)»

## الهدف من الميزة (قابل للقياس)

تحويل **ثلاث وحدات** من نمط «قائمة + محرر منفصل» إلى **نمط `frm*Master` المدمج المُشهَد** (`frmProductMaster` — §2.36 · `RULE-N184`):

| الوحدة | الوضع الحالي (مقيس 2026-11-18) | الوضع الهدف |
|---|---|---|
| Returns | `frmListReturns` (501) + `frmSalesReturn` (519) + `frmPurchaseReturn` (516) | `frmSalesReturnMaster` + `frmPurchaseReturnMaster` |
| Expenses | `frmListExpenses` (619) + `frmAddUpdateExpense` (377) | `frmExpensesMaster` |
| CustomerReceipts | `frmListCustomerReceipts` (562) + `frmAddUpdateCustomerReceipt` (420) | `frmCustomerReceiptsMaster` |

**معيار الإغلاق:** الشاشات الأربع تفتح، تصفّي، تُنشئ، ترحّل/تلغي، وتُحدِّث الشبكة — بنمط `EntryMode` وآلة حالة أزرار ومحرر مدمج وشبكة + `gbFilter` + `pnlSummary`، وتُحذف الشاشات السبع القديمة بلا مرجع باقٍ، والبناء `0 Errors`.

## خارج النطاق (صريح)

1. **الباك-إند كله:** `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/Interfaces/**` · `Services/Direct/**` · `Migrations/**` · `Seeders/**` — لا سطر واحد (`RULE-N177`).
2. **العقود:** لا إضافة/تعديل عضو في أي `I*DesktopService` — الشاشات تستهلك ما هو قائم حرفياً.
3. **تغطية الـMock:** لا إصلاح لفجوة التكافؤ القائمة في عقدي المرتجعات (`ISalesReturnDesktopService` · `IPurchaseReturnDesktopService` — `مؤهل جزئياً`) — تُسجَّل كبند مفتوح مستقل (`RULE-N176`).
4. **شاشات أخرى:** `frmListSales` · `frmListPurchases` · `frmAddUpdateSalesInvoice` · `frmPurchaseInvoice` · `frmPOS` — لا تُلمس.
5. **الطباعة:** نوافذ الطباعة المرتبطة بالمرتجعات إن وُجدت لا تُعدَّل في هذه الموجة.

## جدول الملفات (البيت المعلن)

| المسار | الحالة |
|---|---|
| `SalesSystem.Desktop/Screens/Expenses/Forms/frmExpensesMaster.{cs,Designer.cs,resx}` | يُنشأ |
| `SalesSystem.Desktop/Screens/CustomerReceipts/Forms/frmCustomerReceiptsMaster.{cs,Designer.cs,resx}` | يُنشأ |
| `SalesSystem.Desktop/Screens/Returns/Forms/frmSalesReturnMaster.{cs,Designer.cs,resx}` | يُنشأ |
| `SalesSystem.Desktop/Screens/Returns/Forms/frmPurchaseReturnMaster.{cs,Designer.cs,resx}` | يُنشأ |
| الشاشات السبع القديمة (21 ملفاً) | تُحذف بعد نجاح الشاشات الجديدة |
| `SalesSystem.Desktop/Program.cs` (كتلة DI فقط) | يُعدَّل مرة واحدة في موجة الربط |
| `SalesSystem.Desktop/Screens/frmMain.cs` + `frmMain.Designer.cs` | يُعدَّل مرة واحدة في موجة الربط |
| `SalesSystem.Desktop/Global Classes/clsGlobal.cs` | `_OpenForm` يُعيد النافذة الحيّة (`D-09`، إصلاح المراجعة) |
| `Screens/CashBoxes/Forms/frmCashBoxTransactions.cs` · `Screens/Payments/Forms/frmListPayments.cs` · `Screens/Sales/Forms/frmAddUpdateSalesInvoice.cs` | **ملحقة بـ`D-08`** — 5 مواضع كانت تفتح الشاشات المحذوفة؛ أُعيد توجيهها إلى الشاشات المدمجة |

## المخاطر

1. **عقد المرتجعات بلا `UpdateAsync`** ⇒ وضع «التعديل» في شاشة المرتجع = **عرض للقراءة فقط** مع «ترحيل/إلغاء». ممنوع إظهار زر «حفظ» يوهم بالتعديل.
2. **الربط بملفين مشتركين** (`Program.cs` · `frmMain`) ⇒ مالك واحد في موجة واحدة (`RULE-N114`/`RULE-N158`).
3. **حذف 21 ملفاً** ⇒ لا حذف قبل أخضر البناء بالشاشات الجديدة، وفحص بقايا الأسماء السبعة = 0.
4. **F-001 مفتوحة على هذه الملفات** (تعميم `Enter`/تغذية الحقل المطلوب) ⇒ الشاشات الجديدة تُبنى بالنمط المشهَد نهائياً (`WireEnterAsTab(scope, primaryButton)` + `RequiredFieldHelper` + لا `_ValidateForm`) فتُغلق مدخلات تلك الملفات في F-001 عند إغلاق هذه الموجة.
