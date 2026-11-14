# MOCK-COVERAGE — سجل تغطية الوضع التجريبي (Single Source of Truth)

> **المرجع الملزم:** `AGENTS.md` §2.16 (الوضع التجريبي) · §2.37 (`RULE-N165`…`RULE-N168` مرحلتا التسليم · **`RULE-N173`…`RULE-N178`** تغطية الوضع التجريبي وقفل الباك-إند).
> **مالك التصديق:** وكيل `mock-qa` (سجل `.opencode/agents/mock-qa.md`) — **قراءة فقط**: يفحص ويُصدر الحكم، ولا يعدّل ملفاً ولا يبني ولا يُشغّل التطبيق.
> **مالك التحديث:** الـOrchestrator مع الموجة التي تغيّر أي `Mock*DesktopService` أو أي تسجيل في فرع Mock داخل `Program.cs`.
> **النسخة:** 1.2 — 2026-11-14 — موجة Auth/Backup: إغلاق ف-1 وف-2 (`MockAuthDesktopService` + `MockBackupDesktopService`) ⇒ **0 عقد غير مؤهل** · فتح ف-6 (`IUserDesktopService`) · تحديث العدّ والحكم (تدقيق `mock-qa`).

---

## 1) ما الذي يصدّقه هذا السجل؟

**القاعدة (`RULE-N173`):** كل عقد بيانات `I<Entity>DesktopService` في `SalesSystem.Desktop/Services/Interfaces/` **يجب** أن يكون له:

| الشرط | الدليل المادي |
|---|---|
| تنفيذ وهمي `Mock<Entity>DesktopService` | ملف في `SalesSystem.Desktop/Services/Mock/` يطابق اسم العقد |
| تسجيل في فرع Mock | `services.AddTransient<I<Entity>DesktopService, Mock<Entity>DesktopService>();` داخل `if (appSettings.UseMockData)` في `Program.cs` — أو سطر «Mock-only by design» صريح |
| بيانات أولية (seeds) عربية | `MockData.cs` أو `Mock*Data.cs` — سجلات تُرى فعلاً في الشبكة |
| تكافؤ الرفض (`RULE-N167`) | `Result.Failure` بالعربية للحالات التي سترفضها BLL لاحقاً |
| تشغيل بلا قاعدة بيانات | `UseMockData = true` في `SalesSystem.Desktop/appsettings.json` |

**استثناء وحيد مسموح:** العقود التحتية والخِدمية التي لا تملك شاشة بيانات ولا تحفظ شيئاً — `IEventBus` · `ISessionService` · `INavigationService` · `INotificationService` · عقود الطباعة (`IInvoicePrinter` · `IReceiptPrinter`). وأي عقد بيانات آخر بلا Mock = **فجوة تغطية** تُغلق فجوته قبل تسليم الشاشة لمراجعة UI & UX.

> **ملاحظة (2026-11-14):** `IDialogService`/`DialogService` حُذفا (لا مستهلكين — كل الحوارات عبر `clsDialogs`)، فلم يبقَ في قائمة العقود التحتية إلا ما هو حيّ فعلاً.

---

## 2) تشغيل الوضع التجريبي لمراجعة UI & UX (خطوات المالك)

```powershell
# 1) تأكيد الوضع التجريبي
#    SalesSystem/SalesSystem.Desktop/appsettings.json  ←  "UseMockData": true

# 2) بناء الحل (بدون قاعدة بيانات، بدون سيرفر)
cd SalesSystem
dotnet build .\SalesSystem.slnx

# 3) تشغيل التطبيق — اختر الحساب من القائمة واكتب كلمة المرور التجريبية 123456
#    عنوان النافذة يظهر وفيه: «وضع التجربة (Mock)» — هذا دليل الوضع
```

- **لا قاعدة بيانات ولا اتصال مطلوب** في هذا الوضع.
- كل ما يُحفظ في هذا الوضع **في الذاكرة فقط** ويضيع بإغلاق التطبيق.
- أي شاشة ترفض الحفظ لا تعتمد على قاعدة بيانات — الرفض من `Mock*DesktopService` بتكافؤ مع BLL (`RULE-N167`).

---

## 3) مصفوفة التغطية — العقود (33 عقد بيانات)

> **قاعدة العدّ:** «عدد الرفض» = كل نداءات `.Failure(` داخل ملف الـMock (قراءة فعلية `Select-String`/regex، 2026-11-14). العمود الأخير يذكر ما ينقص لبلوغ تكافؤ `RULE-N167` — والفجوة تُغلق في الـMock فقط، بلا باك-إند (`RULE-N176`).

| # | العقد | الوحدة | التنفيذ الوهمي | بيانات أولية (seeds) | التسجيل | الحكم | ما ينقص |
|---|---|---|---|---|---|---|---|
| 1 | `IAccountDesktopService` | Accounting | `MockAccountDesktopService` (8) | `MockAccountsData` | فرع Mock | `مؤهل للمراجعة` | — |
| 2 | `IAuthDesktopService` | Login / Users | `MockAuthDesktopService` (16) | `MockSecurityData` (5 حسابات + كلمة المرور التجريبية `123456`) | فرع Mock | `مؤهل للمراجعة` | — |
| 3 | `IBackupDesktopService` | Users (`frmBackup`) | `MockBackupDesktopService` (6) | `MockBackupData` (6 نسخ بالاصطلاح + اسم قديم) | فرع Mock | `مؤهل للمراجعة` | 3 رفض دفاعية لا تصلها الواجهة (لا خطوة رفض في `frmBackup`) |
| 4 | `IBranchDesktopService` | Users | `MockBranchDesktopService` (0 — قراءة) | قائمة داخل الملف | فرع Mock | `مؤهل للمراجعة` | — |
| 5 | `ICashBoxDesktopService` | CashBoxes | `MockCashBoxDesktopService` (4) | `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض تكرار اسم الصندوق |
| 6 | `ICashBoxReportDesktopService` | Reports / CashBox | `MockCashBoxReportDesktopService` (0 — قراءة) | `MockData` + `MockCustomerReceiptsData` + `MockExpensesData` | فرع Mock | `مؤهل للمراجعة` | — |
| 7 | `ICategoryDesktopService` | Categories | `MockCategoryDesktopService` (5) | `MockLookupsData` | فرع Mock | `مؤهل للمراجعة` | — |
| 8 | `ICustomerDesktopService` | Customers | `MockCustomerDesktopService` (4) | `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض تكرار الاسم/الهاتف في `CreateAsync` |
| 9 | `ICustomerReceiptDesktopService` | CustomerReceipts | `MockCustomerReceiptDesktopService` (7) | `MockCustomerReceiptsData` + `MockData` | فرع Mock | `مؤهل للمراجعة` | — |
| 10 | `IDashboardDesktopService` | Dashboard | `MockDashboardDesktopService` (0 — قراءة) | 4 ملفات بيانات | فرع Mock | `مؤهل للمراجعة` | — |
| 11 | `IEmployeeDesktopService` | Employees | `MockEmployeeDesktopService` (4) | `MockEmployeesData` | Mock-only by design | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض الحقول الإلزامية/التكرار لا «غير موجود» فقط |
| 12 | `IExpenseDesktopService` | Expenses | `MockExpenseDesktopService` (9) | `MockExpensesData` + `MockData` + `MockAccountsData` | فرع Mock | `مؤهل للمراجعة` | — |
| 13 | `IFinancialReportDesktopService` | Reports / Accounting | `MockFinancialReportDesktopService` (21 — قراءة) | 8 ملفات بيانات | فرع Mock | `مؤهل للمراجعة` | — |
| 14 | `IFormatSettingsDesktopService` | Settings | `MockFormatSettingsDesktopService` (5) | افتراضيات `clsFormat` | Mock-only by design (المرحلة أ) | `مؤهل للمراجعة` | — |
| 15 | `IInventoryAdjustmentDesktopService` | Inventory | `MockInventoryAdjustmentDesktopService` (8) | `MockInventoryData` + `MockData` | فرع Mock | `مؤهل للمراجعة` | — |
| 16 | `IInventoryBatchDesktopService` | Inventory | `MockInventoryBatchDesktopService` (2) | `MockInventoryData` + `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | تحقّق حقول `CreateAsync` |
| 17 | `IInventoryDesktopService` | Inventory | `MockInventoryDesktopService` (5) | `MockData` | فرع Mock | `مؤهل للمراجعة` | لا شاشة تكتب مخزوناً عبر هذا العقد (ف-3 مغلقة) |
| 18 | `IJournalEntryDesktopService` | Accounting | `MockJournalEntryDesktopService` (19) | `MockJournalEntriesData` | فرع Mock | `مؤهل للمراجعة` | — |
| 19 | `IProductDesktopService` | Products | `MockProductDesktopService` (12) | `MockData` + `MockLookupsData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض تكرار الباركود/الاسم في `CreateAsync` |
| 20 | `IPurchaseInvoiceDesktopService` | Purchases | `MockPurchaseInvoiceDesktopService` (9) | `MockPurchasesData` + `MockData` + `MockSuppliersData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | كمية `0` · المدفوع > الإجمالي · مخزون سالب |
| 21 | `IPurchaseReportDesktopService` | Reports / Purchases | `MockPurchaseReportDesktopService` (2 — قراءة) | `MockPurchasesData` + `MockData` + `MockSuppliersData` | فرع Mock | `مؤهل للمراجعة` | — |
| 22 | `IPurchaseReturnDesktopService` | Returns | `MockPurchaseReturnDesktopService` (6) | `MockReturnsData` + `MockData` + `MockSuppliersData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | كمية `0` · إرجاع أكبر من المشترى |
| 23 | `IReportDesktopService` | Reports | `MockReportDesktopService` (19 — قراءة) | 7 ملفات بيانات | فرع Mock | `مؤهل للمراجعة` | — |
| 24 | `ISalesInvoiceDesktopService` | Sales / POS | `MockSalesInvoiceDesktopService` (9) | `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | كمية `0` · المدفوع > الإجمالي · مخزون سالب · بنود فارغة |
| 25 | `ISalesReportDesktopService` | Reports / Sales | `MockSalesReportDesktopService` (6 — قراءة) | `MockData` + `MockLookupsData` | فرع Mock | `مؤهل للمراجعة` | — |
| 26 | `ISalesReturnDesktopService` | Returns | `MockSalesReturnDesktopService` (6) | `MockReturnsData` + `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | كمية `0` · إرجاع أكبر من المبيع |
| 27 | `ISettingsDesktopService` | Settings / Invoices | `MockSettingsDesktopService` (0) | قيم داخلية | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | دالتا الكتابة تعيدان النجاح دائماً (لا شاشة تستهلكهما) |
| 28 | `ISupplierDesktopService` | Suppliers | `MockSupplierDesktopService` (4) | `MockSuppliersData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض تكرار الاسم |
| 29 | `ISupplierPaymentDesktopService` | Payments | `MockSupplierPaymentDesktopService` (3) | `MockSupplierPaymentsData` + `MockSuppliersData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | المدفوع > المتبقي (ف-4) · إلغاء بدل حذف صلب |
| 30 | `IThemeColorDesktopService` | Settings | `MockThemeColorDesktopService` (9) | افتراضيات `ThemePalette` | Mock-only by design (المرحلة أ) | `مؤهل للمراجعة` | — |
| 31 | `IUnitDesktopService` | Units | `MockUnitDesktopService` (6) | `MockLookupsData` | فرع Mock | `مؤهل للمراجعة` | — |
| 32 | `IUserDesktopService` | Users | `MockUserDesktopService` (7) | `MockSecurityData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | لا رفض «اسم مستخدم مطلوب» في `CreateAsync`/`UpdateAsync`، و`Password` يتجاهله الـMock ⇒ كلمة المرور المكتوبة في `frmUserMaster` لا تعمل في الدخول (ف-6) |
| 33 | `IWarehouseDesktopService` | Warehouses / Inventory | `MockWarehouseDesktopService` (4) | `MockData` | فرع Mock | `مؤهل جزئياً — تكافؤ الرفض ناقص` | رفض تكرار الاسم |

**الخلاصة (عدّ فعلي 2026-11-14، بعد موجة Auth/Backup — تدقيق `mock-qa`):** **37** واجهة `I*` في `Services/Interfaces/` · منها **33 عقد بيانات** (المستثنى 4 عقود تحتية: `IEventBus` · `ISessionService` · `INavigationService` · `INotificationService` — وعقود الطباعة في `Printing/`) · **33** عقداً له `Mock*DesktopService`: **30** مسجَّلة داخل `if (appSettings.UseMockData)` + **3** Mock-only by design (`IEmployee` · `IThemeColor` · `IFormatSettings`) · **0** موصول دائماً بـ`Direct*` · **48** ملفاً في `Services/Mock/` = **33** `Mock*DesktopService` + `MockAuthService` (غير قابل للحل في وضع التجربة — مرشَّح للحذف) + **13** `Mock*Data.cs` + `MockData.cs`.

> **الحكم الإجمالي للسجل: `مؤهل جزئياً — تكافؤ الرفض ناقص`** — **19** عقداً `مؤهل للمراجعة` و**14** `مؤهل جزئياً` و**0** `غير مؤهل`. لم يبقَ عقد يمنع مراجعة المالك (بوابة 0c)، لكن **الباك-إند مقفل لكل الوحدات** لسببين: نقص التكافؤ في 14 عقداً، و**سجل الموافقات المكتوبة الفارغ** (§7) — `RULE-N177`.

---

## 4) دليل مراجعة المالك — الوحدات والشاشات (22 وحدة + الهيكل)

> لكل صف: افتح الشاشة من `frmMain` في وضع التجربة، ثم نفّذ خطوات القسم 5. الشاشات بين قوسين هي ملفات `frm*` الفعلية.

| الوحدة | عدد الشاشات | الشاشات | عقود البيانات | الحكم |
|---|---|---|---|---|
| Accounting | 2 | `frmAccounts` · `frmJournalEntries` | `IAccountDesktopService` · `IJournalEntryDesktopService` | ✅ مؤهل |
| CashBoxes | 2 | `frmCashBoxMaster` · `frmCashBoxTransactions` | `ICashBoxDesktopService` | ⚠️ جزئي — تكافؤ الرفض |
| Categories | 1 | `frmCategoryMaster` | `ICategoryDesktopService` | ✅ مؤهل |
| CustomerReceipts | 2 | `frmListCustomerReceipts` · `frmAddUpdateCustomerReceipt` | `ICustomerReceiptDesktopService` | ✅ مؤهل |
| Customers | 3 | `frmCustomerMaster` · `frmCustomerSelection` · `frmCustomerStatement` | `ICustomerDesktopService` | ⚠️ جزئي — تكافؤ الرفض |
| Dashboard | 1 | `frmDashboard` (+ `ctrlDashboard`) | `IDashboardDesktopService` | ✅ مؤهل |
| Employees | 1 | `frmEmployeeMaster` | `IEmployeeDesktopService` | ⚠️ جزئي — تكافؤ الرفض (Mock-only) |
| Expenses | 2 | `frmListExpenses` · `frmAddUpdateExpense` | `IExpenseDesktopService` | ✅ مؤهل |
| Inventory | 16 | `frmInventory` · `frmInventoryList` · `frmInventoryAddProduct` · `frmInventoryBarcode` · `frmInventoryFifo` · `frmListInventoryTransactions` · `frmListInventoryBatches` · `frmListInventoryAdjustments` · `frmAddUpdateInventoryAdjustment` · `frmOpeningStock` · `frmWarehouseTransferNew` · `frmStocktakingNew` + 4 نوافذ طباعة | `IInventoryDesktopService` · `IInventoryBatchDesktopService` · `IInventoryAdjustmentDesktopService` · `IWarehouseDesktopService` | ⚠️ جزئي — `IInventoryBatch` و`IWarehouse` بلا رفض تكرار |
| Payments | 3 | `frmListPayments` · `frmListSupplierPayments` · `frmSupplierPaymentDialog` | `ISupplierPaymentDesktopService` | ⚠️ جزئي — تكافؤ الرفض (ف-4) |
| POS | 1 | `frmPOS` | `IProductDesktopService` · `ISalesInvoiceDesktopService` · `ICustomerDesktopService` · `IWarehouseDesktopService` · `ICashBoxDesktopService` | ⚠️ جزئي — تكافؤ الرفض |
| Products | 4 | `frmProductMaster` · `frmProductUnitMaster` · `frmProductSelection` · `frmImportProductsExcel` | `IProductDesktopService` | ⚠️ جزئي — تكرار الباركود |
| Purchases | 9 | `frmListPurchases` · `frmPurchaseInvoice` · `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseOtherDetails` · `frmPurchaseInvoicePrint` · `frmSupplierStatement` | `IPurchaseInvoiceDesktopService` · `ISupplierDesktopService` | ⚠️ جزئي — كمية `0` · مدفوع > إجمالي |
| Reports | 31 | 31 تقريراً في ستة مجلدات تصنيفية (`Sales/` · `Purchases/` · `Inventory/` · `Accounting/` · `CustomersSuppliers/` · `CashBox/`) | خمسة عقود تقارير | ✅ مؤهل (قراءة فقط) |
| Returns | 3 | `frmListReturns` · `frmSalesReturn` · `frmPurchaseReturn` | `ISalesReturnDesktopService` · `IPurchaseReturnDesktopService` | ⚠️ جزئي — إفراط في الإرجاع |
| Sales | 2 | `frmListSales` · `frmAddUpdateSalesInvoice` | `ISalesInvoiceDesktopService` | ⚠️ جزئي — كمية `0` · مدفوع > إجمالي |
| Settings | 2 | `frmChangeSystemColorsSettings` · `frmFormatingSetting` | `IThemeColorDesktopService` · `IFormatSettingsDesktopService` | ✅ مؤهل (المرحلة أ) |
| Suppliers | 2 | `frmSupplierMaster` · `frmSupplierSelection` | `ISupplierDesktopService` | ⚠️ جزئي — تكافؤ الرفض |
| Tools | 1 | `frmCalculator` | — (لا مسار بيانات) | لا ينطبق |
| Units | 1 | `frmUnitMaster` | `IUnitDesktopService` | ✅ مؤهل |
| Users | 3 | `frmUserMaster` · `frmPasswordChange` · `frmBackup` | `IUserDesktopService` · `IBranchDesktopService` · `IAuthDesktopService` · `IBackupDesktopService` | ⚠️ جزئي — تكافؤ الرفض (`IUserDesktopService` — ف-6) · و`frmBackup` و`frmPasswordChange` مؤهلان |
| Warehouses | 1 | `frmWarehouseMaster` | `IWarehouseDesktopService` | ⚠️ جزئي — تكرار الاسم |
| الهيكل (root) | 2 | `frmLogin` · `frmMain` | `IAuthDesktopService` | ✅ مؤهل — الدخول بالحساب + `123456` (`MockAuthDesktopService`) |

**الإجمالي (عدّ فعلي):** **95 نموذج `frm*`** في `Screens/**` (منها 31 تقريراً) · **89** ذات مسار بيانات، منها **89** تُفتح وتُحفظ بلا قاعدة بيانات و**0** غير مؤهلة · **6** بلا مسار بيانات (`frmCalculator` + خمس نوافذ طباعة/معاينة: `frmStocktakingPrint` · `frmStocktakingPrintAdjustment` · `frmStocktakingPrintCard` · `frmWarehouseTransferPrint` · `frmPurchaseInvoicePrint`).

---

## 5) قائمة مراجعة UI & UX التي ينفّذها المالك (لكل وحدة)

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | شغّل التطبيق بـ`UseMockData = true`، اختر الحساب من القائمة واكتب `123456` (الحسابات: `admin` · `cashier1` · `accountant1` · `warehouse1` · `locked_test`) | تدخل `frmMain` وعنوانها يحوي «وضع التجربة (Mock)» · `locked_test` يُرفض بحكم القفل |
| 2 | افتح شاشة الدليل/الماستر في الوحدة | تُفتح في **وضع العرض**، الحقول معطّلة، `lblMode` = «وضع العرض» |
| 3 | اضغط «جديد» (أو `F2`) | الحقول تُفعّل وتُفرَّغ، `lblMode` = «إضافة … جديد»، «حفظ» مفعّل |
| 4 | احفظ بلا تعبئة حقل إلزامي | رفض عربي من الـMock (تكافؤ `RULE-N167`) — لا استثناء صامت |
| 5 | أدخل سجلاً صحيحاً واحفظ | يظهر في الشبكة فوراً، السطر يُحدَّد، `lblMode` = «تعديل …: <الاسم>» |
| 6 | كرّر نفس الكود/الباركود | رفض عربي «مكرر» — لا سطر ثانٍ |
| 7 | جرّب `Enter`/`Tab`/الأسهم داخل الحقول | التنقل يعمل، القيمة الحالية تُحدَّد، الحقل النشط يتلوّن، لا يقفز خارج الشاشة |
| 8 | جرّب `F5` ثم «مسح التصفية» | `F5` يعيد تحميل كل شيء ويُرجع وضع العرض · «مسح التصفية» يمسح التصفية فقط بلا إعادة تحميل |
| 9 | في شاشة مستند (فاتورة/سند/مستند مخزون): أضف سطراً بكمية `0` ثم احفظ | رفض عربي (كمية صفر) |
| 10 | في شاشة دفع: أدخل مبلغاً أكبر من المتبقي | رفض عربي (المبلغ > المتبقي) — إن نجح فالسجل ناقص التكافؤ (بند ف-4) |
| 11 | عطّل سجلاً ثم أعد تنشيطه | الدورة تعمل، السطر يُحدَّد، رسالة عربية |
| 12 | افتح تقريراً من «التقارير» في وضع التجربة | يعرض بيانات تجريبية بلا قاعدة بيانات، وزر التصدير يعمل |

> **تحذير مقصود (2026-11-14):** الخطوتان **4** و**6** و**9** و**10** **ستفشل حالياً** في شاشات البيع والشراء والمرتجعات وسند الصرف والماسترات المعلَّمة `مؤهل جزئياً — تكافؤ الرفض ناقص` (§3) — لأن الـMock لا يرفض بعدُ: كمية `0` · المدفوع > الإجمالي · الباركود/الاسم المكرر · الإفراط في الإرجاع. هذا **نقص تكافؤ معلن** (بند ف-5) لا عيب في الشاشة: كل رفض يمرّ اليوم سُيغلق في موجة تكافؤ داخل الـMock وحدها، بلا باك-إند. الخطوتان **3** و**5** و**8** و**11** صالحة في كل الوحدات لتقييم UI & UX الآن.

---

## 6) الفجوات المفتوحة (تُغلق بوكيل واحد لكل ملف — `RULE-N158`)

| البند | الفجوة | الملف/الموضع | المالك المقترح | الحالة |
|---|---|---|---|---|
| ف-1 | ✅ **مغلقة (موجة Auth/Backup 2026-11-14):** `MockAuthDesktopService` يطبّق `IAuthDesktopService` بالكامل (دخول + تغييرا كلمة المرور)، مسجَّل في فرع Mock، ويستهلك `MockSecurityData` (5 حسابات + كلمة مرور تجريبية `123456`)، ويرفض: اسم مستخدم فارغ/غير موجود · حساب مقفل · حساب معطّل · كلمة مرور فارغة/خاطئة · قفل بعد 5 محاولات · كلمة حالية فارغة/خاطئة · جديدة فارغة/قصيرة/مطابقة للحالية · تأكيد غير متطابق — كلها عربية، والنجاح يخزّن الكلمة الجديدة و`MustChangePassword = false` | `Services/Mock/MockAuthDesktopService.cs` · `MockSecurityData.cs` · `Program.cs` (فرعا Mock/Real) | — | ✅ **مغلقة** |
| ف-2 | ✅ **مغلقة (موجة Auth/Backup 2026-11-14):** `MockBackupDesktopService` + `MockBackupData` (6 أسماء بالاصطلاح الرسمي عبر `BackupFileNameHelper` + اسم قديم) مسجَّل في فرع Mock — `frmBackup` تُفتح وتُنشئ وتستعيد بلا قاعدة بيانات. رفض «يجب تحديد ملف النسخة الاحتياطية.» مطابق حرفياً لحرّاس `DirectBackupService` | `Services/Mock/MockBackupDesktopService.cs` · `MockBackupData.cs` · `Program.cs` | `ui-agent` | ✅ **مغلقة** |
| ف-3 | **مغلقة بالتدقيق (2026-11-14):** `MockInventoryDesktopService` فيه 5 رفض، ولا شاشة تكتب مخزوناً عبر هذا العقد (`frmAddUpdateSalesInvoice` · `frmProductSelection` · `frmListInventoryTransactions` تقرأ فقط، و`frmOpeningStock` يحفظ عبر `IInventoryAdjustmentDesktopService`) | `Services/Mock/MockInventoryDesktopService.cs` | — | ✅ **مغلقة** |
| ف-4 | `MockSupplierPaymentDesktopService`: لا رفض «المدفوع > المتبقي»، و`CancelAsync` **تحذف السند** بدل حالة إلغاء (`RULE-004`)، و`PostAsync`/`CancelAsync` خارج الواجهة | `Services/Mock/MockSupplierPaymentDesktopService.cs` | `ui-agent` | **مفتوح — تكافؤ (مجمِّد لباك-إند المدفوعات)** |
| ف-5 | **تكافؤ الرفض ناقص في 13 عقداً** — التفصيل عمود «ما ينقص» في §3 (البيع/الشراء/المرتجعات: كمية `0` · مدفوع > الإجمالي · مخزون سالب · إفراط في الإرجاع؛ والماسترات: تكرار الباركود/الاسم/الهاتف) | ملفات `Services/Mock/Mock*DesktopService.cs` المعلَّمة `مؤهل جزئياً` | `ui-agent` (وكيل واحد لكل ملف) | **مفتوح — موجة تكافؤ** |
| ف-6 | `MockUserDesktopService`: `CreateAsync` لا يرفض اسماً فارغاً، و`CreateAsync`/`UpdateAsync` **يتجاهلان `Password`** — فحقل كلمة المرور في `frmUserMaster` يُحفظ بلا أثر ويعمل الدخول بالافتراضية `123456`؛ ورسالة `ResetPasswordAsync` تذكر «(12345678)» وهي قيمة لا وجود لها | `Services/Mock/MockUserDesktopService.cs` · `Screens/Users/Forms/frmUserMaster.cs` | `ui-agent` | **مفتوح — تكافؤ (مجمِّد لحكم Users الكامل)** |

> **قاعدة:** فجوة `RULE-N173` لا تُغلق بتعديل الشاشة — تُغلق بإضافة الـMock/seeds/التسجيل/التكافؤ، ولا يُعلن أي module «مؤهلاً» في هذا السجل قبل ذلك، و**لا يُلمس باك-إند الوحدة** أثناء إغلاق فجوته (`RULE-N177`/`RULE-N178`).

---

## 7) قفل الباك-إند وبوابة الفتح (`RULE-N177`/`RULE-N178`)

**القاعدة (على مستوى النظام كله — 2026-11-14):** لا يُعدَّل باك-إند أي وحدة قبل أن تخرج من هذه الدورة بالترتيب.

```text
(1) تغطية Mock كاملة      Mock*DesktopService + seeds عربية + تسجيل في فرع Mock + تكافؤ رفض
        ↓
(2) مراجعة المالك          بناء الـMock يُسلَّم ← المالك يفتح الشاشات ويحفظ بلا قاعدة بيانات
        ↓
(3) موافقة مكتوبة          تاريخ + نص المالك صريح («موافق على الباك-إند») — يُسجَّل هنا وفي تقرير التسليم
        ↓
(4) الباك-إند يُفتح        Models → DataAccess (ترحيل) → Business (BLL/DTO) → Services/Direct + فرع DI الحقيقي
```

| الحالة في §3 | معنى قفل الباك-إند | ما يُسمح فوراً |
|---|---|---|
| `مؤهل للمراجعة` بلا موافقة مكتوبة | **مقفل**: لا `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` · ترحيل · seeder | إصلاحات UI/UX داخل الـMock · إصلاحات بناء (بند الاستثناءات في `RULE-N178`) |
| `مؤهل للمراجعة` + موافقة مكتوبة مؤرَّخة | **مفتوح** — Stage B مسموح للوحدة نفسها فقط | 4 موجات Stage B بالترتيب |
| `مؤهل جزئياً — تكافؤ الرفض ناقص` | **مقفل** حتى يُغلق نقص التكافؤ أو يقبل المالك النقص كتابةً — **ويبقى شرط الموافقة المكتوبة المؤرَّخة على سلوك الـMock واجباً** (الصف الأول) | إصلاح تكافؤ الرفض في `Mock*` |
| `غير مؤهل — فجوة تغطية` | **مجمَّد الباك-إند**: لا `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` ولا ترحيل — الحظر على الباك-إند لا على إغلاق التغطية | إغلاق الفجوة فقط (Mock/seeds/تسجيل/تكافؤ) |

**سجل الموافقات المكتوبة:** (فارغ — **0 موافقات مؤرَّخة حتى 2026-11-14**) ⇐ **الباك-إند مقفل لكل الوحدات بلا استثناء**، وتُقيَّد كل موافقة هنا بالشكل: `التاريخ · الوحدة · نص المالك الحرفي · ما راجعه`.

**ملاحظات القفل:**
- القفل **لا يُعيد فتح وحدة مغلقة** ولا يُلغي عملاً مكتملاً؛ يحكم **التغيير التالي** على وحدتها (`RULE-N178`).
- **لا وحدة مجمَّدة تغطيةً بعد 2026-11-14:** ف-1 وف-2 مغلقتان و**0** عقد `غير مؤهل` ⇒ كل الشاشات قابلة لمراجعة المالك (بوابة 0c). **وكل الوحدات مقفلة الباك-إند** لسبب واحد: **سجل الموافقات المكتوبة فارغ** — ويُضاف شرط إغلاق ف-6 (أو قبول مكتوب بالنقص) قبل رفع `IUserDesktopService` إلى `مؤهل للمراجعة`.
- صف `IAuthDesktopService` في §3 حكمه الآن **`مؤهل للمراجعة`** (كان `غير مؤهل`) — ولا يُقلب الحكم إلا بتدقيق `mock-qa`؛ بوابة 0c وقفل الباك-إند يقرآن المفردات الثلاث فقط (`RULE-N175`).
- الاستثناءات الثلاثة المنصوصة في `RULE-N178` (خطأ بناء يمنع الترجمة · أمن عاجل معلَّن للمالك · تصحيح يفرضه `AGENTS.md` بلا تغيير سلوك مالي) تُدوَّن في تقرير التسليم.
- **قاعدة الفحص للـOrchestrator:** أي حزمة عمل (brief) تحمل ملفاً من `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` تُرفض ما لم تذكر حكم هذا السجل للوحدة + نص الموافقة وتاريخها (`RULE-N160`/`RULE-N177`) — وهذه هي **بوابة 0d** في `docs/prompts/ORCHESTRATOR.md` §8.

---

## 8) بروتوكول التحديث

0. **تحديث 2026-11-14 (الموجة الثانية — إغلاق تكافؤ الرفض):** أُغلقت الفجوات `ف-4` و`ف-6`…`ف-19` داخل الـMock وحده (`RULE-N176`)، وصار أعلى هذا الملف (§1–§7) **مرجعاً تاريخياً** لما قبل الموجة. الحقائق المعتمدة الآن:
   - **33 عقداً · 33 `Mock*DesktopService` · 30 مسجَّلاً في فرع `UseMockData` + 3 «Mock-only by design» · 0 مسجَّل في فرع Direct دائماً · 33/33 ببيانات أولية.**
   - **تكافؤ الرفض: كامل في 30 · لا ينطبق/قراءة فقط في 3 · ناقص في 0.** الفجوات المغلقة: فاتورة البيع والشراء (بنود فارغة · كمية `0` · سعر `0` · مدفوع > الإجمالي · مخزون سالب · تعديل/ترحيل مرحلة) · المرتجعان (كمية `0` · إرجاع > المُباع/المشترى) · سند القبض (مبلغ `0` · مبلغ > المتبقي · تخصيص > المبلغ) · المصروف (مبلغ `0`/سالب · بيان مفقود) · تسوية المخزون (بلا بنود · كمية `0` · خصم > الرصيد) · دفعة المخزون (حقول إلزامية · كمية `0` · رقم دفعة مكرر) · سداد المورد (مبلغ `0` · > المتبقي · ترحيل/إلغاء مزدوج · **`CancelAsync` يُبقي السند بحالة ملغى — `RULE-004`**) · الماسترات الخمسة (تكرار الاسم/الباركود/الهاتف).
   - **أثر المخزون في المحاكاة صار صحيح الدورة:** لا حركة عند الحفظ (مسودة) · الخصم/الإضافة عند **الترحيل** · العكس عند **إلغاء مستند مرحّل** — لفواتير البيع والشراء والمرتجعين. الثابت: *الرصيد يعكس المستندات المرحّلة وغير الملغاة فقط.*
   - **`MockAuthDesktopService`** مطابق لـ`§2.16` بعد تحديثه: حساب مزروع + كلمة `123456` · رفض كلمة خاطئة/مقفل/معطّل برسائل عربية · `MockSecurityData` صار **6** حسابات (أُضيف `inactive_test` ليكون رفض «معطّل» قابلاً للتجربة).
   - **الحكم الإجمالي لم يُرفع إلى `مؤهل للمراجعة`** لأنه يشترط شهادة `mock-qa` جديدة على الشيفرة بعد الموجة — وتلك بوابة 0c قبل تسليم Stage A.
   - **الباك-إند مقفل كما هو** (`RULE-N177`): §7 ما زال **0 موافقات مكتوبة**.

1. أي موجة تُضيف/تعدّل `Mock*DesktopService` أو `Mock*Data` أو فرع Mock في `Program.cs` ⇒ تُحدَّث **هذا الملف** في نفس الموجة (`RULE-N174`).
2. الحكم لا يُكتب يدوياً بالحدس: `mock-qa` يفحص ويُصدره، والـOrchestrator ينسخه إلى الجدول ويذكر تاريخ الفحص.
3. الموافقة المكتوبة على الباك-إند تُسجَّل في §7 (تاريخ + نص المالك) **قبل** أول ملف Stage B، ولا يُفتح Stage B بدونهما (`RULE-N177`).
4. لا تُنسخ أرقام هذا السجل داخل `AGENTS.md` أو داخل أي شاشة — يُستهلك بالمرجع (`RULE-N169`).
