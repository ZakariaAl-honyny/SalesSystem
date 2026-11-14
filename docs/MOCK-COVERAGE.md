# MOCK-COVERAGE — سجل تغطية الوضع التجريبي (Single Source of Truth)

> **المرجع الملزم:** `AGENTS.md` §2.16 (الوضع التجريبي) · §2.37 (`RULE-N165`…`RULE-N168` مرحلتا التسليم · **`RULE-N173`…`RULE-N178`** تغطية الوضع التجريبي وقفل الباك-إند).
> **مالك التصديق:** وكيل `mock-qa` (سجل `.opencode/agents/mock-qa.md`) — **قراءة فقط**: يفحص ويُصدر الحكم، ولا يعدّل ملفاً ولا يبني ولا يُشغّل التطبيق.
> **مالك التحديث:** الـOrchestrator مع الموجة التي تغيّر أي `Mock*DesktopService` أو أي تسجيل في فرع Mock داخل `Program.cs`.
> **النسخة:** 1.1 — 2026-11-14 — الأساس الأول مبني على عدّ فعلي للملفات (لا ادّعاء) + §7 قفل الباك-إند.

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

# 3) تشغيل التطبيق — الدخول في وضع التجربة يقبل أي بيانات
#    عنوان النافذة يظهر وفيه: «وضع التجربة (Mock)» — هذا دليل الوضع
```

- **لا قاعدة بيانات ولا اتصال مطلوب** في هذا الوضع.
- كل ما يُحفظ في هذا الوضع **في الذاكرة فقط** ويضيع بإغلاق التطبيق.
- أي شاشة ترفض الحفظ لا تعتمد على قاعدة بيانات — الرفض من `Mock*DesktopService` بتكافؤ مع BLL (`RULE-N167`).

---

## 3) مصفوفة التغطية — العقود (33 عقد بيانات)

| # | العقد | الوحدة | التنفيذ الوهمي | مسجَّل في فرع Mock | الحكم |
|---|---|---|---|---|---|
| 1 | `IAccountDesktopService` | Accounting | `MockAccountDesktopService` (7 رفض) | ✅ | مؤهل |
| 2 | `IAuthDesktopService` | Login / Users | ❌ لا يوجد `MockAuthDesktopService` — يُسند دائماً إلى `DirectAuthService` الذي يستدعي `MockAuthService` في وضع التجربة | ⚠️ غير مطبَّق كـMock مستقل | **مؤهل وظيفياً · فجوة تصنيف (بند ف-1)** |
| 3 | `IBackupDesktopService` | Users (`frmBackup`) | ❌ لا يوجد أي Mock | ❌ `DirectBackupService` دائماً | **غير مؤهل — فجوة تغطية (بند ف-2)** |
| 4 | `IBranchDesktopService` | Users (`frmUserMaster`) | `MockBranchDesktopService` (0 رفض — قراءة فقط) | ✅ | مؤهل |
| 5 | `ICashBoxDesktopService` | CashBoxes | `MockCashBoxDesktopService` (2 رفض) | ✅ | مؤهل |
| 6 | `ICashBoxReportDesktopService` | Reports / CashBox | `MockCashBoxReportDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 7 | `ICategoryDesktopService` | Categories | `MockCategoryDesktopService` (2) | ✅ | مؤهل |
| 8 | `ICustomerDesktopService` | Customers | `MockCustomerDesktopService` (2) | ✅ | مؤهل |
| 9 | `ICustomerReceiptDesktopService` | CustomerReceipts | `MockCustomerReceiptDesktopService` (2) | ✅ | مؤهل |
| 10 | `IDashboardDesktopService` | Dashboard | `MockDashboardDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 11 | `IEmployeeDesktopService` | Employees | `MockEmployeeDesktopService` (2) | ✅ Mock-only by design | مؤهل |
| 12 | `IExpenseDesktopService` | Expenses | `MockExpenseDesktopService` (2) | ✅ | مؤهل |
| 13 | `IFinancialReportDesktopService` | Reports / Accounting | `MockFinancialReportDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 14 | `IFormatSettingsDesktopService` | Settings | `MockFormatSettingsDesktopService` (4) | ✅ Mock-only by design (المرحلة أ) | مؤهل |
| 15 | `IInventoryAdjustmentDesktopService` | Inventory | `MockInventoryAdjustmentDesktopService` (2) | ✅ | مؤهل |
| 16 | `IInventoryBatchDesktopService` | Inventory | `MockInventoryBatchDesktopService` (1) | ✅ | مؤهل |
| 17 | `IInventoryDesktopService` | Inventory | `MockInventoryDesktopService` (0) | ✅ | مؤهل · **تكافؤ الرفض يحتاج تحقّقاً (بند ف-3)** |
| 18 | `IJournalEntryDesktopService` | Accounting | `MockJournalEntryDesktopService` (6) | ✅ | مؤهل |
| 19 | `IProductDesktopService` | Products | `MockProductDesktopService` (5) | ✅ | مؤهل |
| 20 | `IPurchaseInvoiceDesktopService` | Purchases | `MockPurchaseInvoiceDesktopService` (5) | ✅ | مؤهل |
| 21 | `IPurchaseReportDesktopService` | Reports / Purchases | `MockPurchaseReportDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 22 | `IPurchaseReturnDesktopService` | Returns | `MockPurchaseReturnDesktopService` (5) | ✅ | مؤهل |
| 23 | `IReportDesktopService` | Reports | `MockReportDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 24 | `ISalesInvoiceDesktopService` | Sales / POS | `MockSalesInvoiceDesktopService` (5) | ✅ | مؤهل |
| 25 | `ISalesReportDesktopService` | Reports / Sales | `MockSalesReportDesktopService` (قراءة فقط) | ✅ | مؤهل |
| 26 | `ISalesReturnDesktopService` | Returns | `MockSalesReturnDesktopService` (5) | ✅ | مؤهل |
| 27 | `ISettingsDesktopService` | Settings / Invoices | `MockSettingsDesktopService` (0 — قراءة إعدادات) | ✅ | مؤهل |
| 28 | `ISupplierDesktopService` | Suppliers | `MockSupplierDesktopService` (2) | ✅ | مؤهل |
| 29 | `ISupplierPaymentDesktopService` | Payments | `MockSupplierPaymentDesktopService` (0) | ✅ | مؤهل · **تكافؤ الرفض ناقص (بند ف-4)** |
| 30 | `IThemeColorDesktopService` | Settings | `MockThemeColorDesktopService` (8) | ✅ Mock-only by design (المرحلة أ) | مؤهل |
| 31 | `IUnitDesktopService` | Units | `MockUnitDesktopService` (2) | ✅ | مؤهل |
| 32 | `IUserDesktopService` | Users | `MockUserDesktopService` (3) | ✅ | مؤهل |
| 33 | `IWarehouseDesktopService` | Warehouses / Inventory | `MockWarehouseDesktopService` (2) | ✅ | مؤهل |

**الخلاصة (عدّ فعلي 2026-11-14):** 33 عقد بيانات · **31** لها `Mock*DesktopService` مسجَّل في فرع Mock · **2** بلا Mock مستقل (`IAuthDesktopService` يعمل عبر `MockAuthService`، و`IBackupDesktopService` لا يعمل بلا قاعدة بيانات) · **31** ملف `Mock*DesktopService` + `MockAuthService` + **12** ملف بيانات `Mock*Data.cs` + `MockData.cs` (45 ملفاً في `Services/Mock/`).

---

## 4) دليل مراجعة المالك — الوحدات والشاشات (21 وحدة + الهيكل)

> لكل صف: افتح الشاشة من `frmMain` في وضع التجربة، ثم نفّذ خطوات القسم 5. الشاشات بين قوسين هي ملفات `frm*` الفعلية.

| الوحدة | عدد الشاشات | الشاشات | عقود البيانات | الحكم |
|---|---|---|---|---|
| Accounting | 2 | `frmAccounts` · `frmJournalEntries` | `IAccountDesktopService` · `IJournalEntryDesktopService` | ✅ مؤهل |
| CashBoxes | 2 | `frmCashBoxMaster` · `frmCashBoxTransactions` | `ICashBoxDesktopService` | ✅ مؤهل |
| Categories | 1 | `frmCategoryMaster` | `ICategoryDesktopService` | ✅ مؤهل |
| CustomerReceipts | 2 | `frmListCustomerReceipts` · `frmAddUpdateCustomerReceipt` | `ICustomerReceiptDesktopService` | ✅ مؤهل |
| Customers | 3 | `frmCustomerMaster` · `frmCustomerSelection` · `frmCustomerStatement` | `ICustomerDesktopService` | ✅ مؤهل |
| Dashboard | 1 | `frmDashboard` (+ `ctrlDashboard`) | `IDashboardDesktopService` | ✅ مؤهل |
| Employees | 1 | `frmEmployeeMaster` | `IEmployeeDesktopService` | ✅ مؤهل (Mock-only) |
| Expenses | 2 | `frmListExpenses` · `frmAddUpdateExpense` | `IExpenseDesktopService` | ✅ مؤهل |
| Inventory | 16 | `frmInventory` · `frmInventoryList` · `frmInventoryAddProduct` · `frmInventoryBarcode` · `frmInventoryFifo` · `frmListInventoryTransactions` · `frmListInventoryBatches` · `frmListInventoryAdjustments` · `frmAddUpdateInventoryAdjustment` · `frmOpeningStock` · `frmWarehouseTransferNew` · `frmStocktakingNew` + 4 نوافذ طباعة | `IInventoryDesktopService` · `IInventoryBatchDesktopService` · `IInventoryAdjustmentDesktopService` · `IWarehouseDesktopService` | ✅ مؤهل · تكافؤ الرفض بند ف-3 |
| Payments | 3 | `frmListPayments` · `frmListSupplierPayments` · `frmSupplierPaymentDialog` | `ISupplierPaymentDesktopService` | ✅ مؤهل · تكافؤ الرفض بند ف-4 |
| POS | 1 | `frmPOS` | `IProductDesktopService` · `ISalesInvoiceDesktopService` · `ICustomerDesktopService` · `IWarehouseDesktopService` · `ICashBoxDesktopService` | ✅ مؤهل |
| Products | 4 | `frmProductMaster` · `frmProductUnitMaster` · `frmProductSelection` · `frmImportProductsExcel` | `IProductDesktopService` | ✅ مؤهل |
| Purchases | 9 | `frmListPurchases` · `frmPurchaseInvoice` · `frmPurchaseItem` · `frmPurchaseItemInfo` · `frmPurchaseCurrency` · `frmPurchaseDiscount` · `frmPurchaseOtherDetails` · `frmPurchaseInvoicePrint` · `frmSupplierStatement` | `IPurchaseInvoiceDesktopService` · `ISupplierDesktopService` | ✅ مؤهل |
| Reports | 31 | 31 تقريراً في ستة مجلدات تصنيفية (`Sales/` · `Purchases/` · `Inventory/` · `Accounting/` · `CustomersSuppliers/` · `CashBox/`) | خمسة عقود تقارير | ✅ مؤهل (قراءة فقط) |
| Returns | 3 | `frmListReturns` · `frmSalesReturn` · `frmPurchaseReturn` | `ISalesReturnDesktopService` · `IPurchaseReturnDesktopService` | ✅ مؤهل |
| Sales | 2 | `frmListSales` · `frmAddUpdateSalesInvoice` | `ISalesInvoiceDesktopService` | ✅ مؤهل |
| Settings | 2 | `frmChangeSystemColorsSettings` · `frmFormatingSetting` | `IThemeColorDesktopService` · `IFormatSettingsDesktopService` | ✅ مؤهل (المرحلة أ) |
| Suppliers | 2 | `frmSupplierMaster` · `frmSupplierSelection` | `ISupplierDesktopService` | ✅ مؤهل |
| Tools | 1 | `frmCalculator` | — (لا مسار بيانات) | لا ينطبق |
| Units | 1 | `frmUnitMaster` | `IUnitDesktopService` | ✅ مؤهل |
| Users | 3 | `frmUserMaster` · `frmPasswordChange` · `frmBackup` | `IUserDesktopService` · `IBranchDesktopService` · `IAuthDesktopService` · `IBackupDesktopService` | ⚠️ **جزئي** — `frmBackup` غير مؤهل (ف-2) |
| Warehouses | 1 | `frmWarehouseMaster` | `IWarehouseDesktopService` | ✅ مؤهل |
| الهيكل (root) | 2 | `frmLogin` · `frmMain` | `IAuthDesktopService` | ✅ مؤهل (الدخول يقبل أي بيانات) |

**الإجمالي:** **93 نموذج `frm*`** في `Screens/**` · **87** ذات مسار بيانات، منها **86** تُفتح وتُحفظ بلا قاعدة بيانات و**1** غير مؤهلة (`frmBackup`) · **6** بلا مسار بيانات (`frmCalculator` + خمس نوافذ طباعة/معاينة).

---

## 5) قائمة مراجعة UI & UX التي ينفّذها المالك (لكل وحدة)

| # | الخطوة | النتيجة المتوقعة |
|---|---|---|
| 1 | شغّل التطبيق بـ`UseMockData = true` وسجّل الدخول بأي اسم/كلمة سر | تدخل `frmMain` وعنوانها يحوي «وضع التجربة (Mock)» |
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

---

## 6) الفجوات المفتوحة (تُغلق بوكيل واحد لكل ملف — `RULE-N158`)

| البند | الفجوة | الملف/الموضع | المالك المقترح | الحالة |
|---|---|---|---|---|
| ف-1 | `IAuthDesktopService` بلا `MockAuthDesktopService` مستقل — يعمل عبر `DirectAuthService` + `MockAuthService` (وظيفياً يعمل، لكنه لا يطابق قاعدة «كل عقد بيانات له Mock») | `Services/Interfaces/IAuthDesktopService.cs` · `Program.cs` | `ui-agent` (نفس عقد الشاشة) | مفتوح — **مجمِّد لباك-إند وحدة المستخدمين/المصادقة** |
| ف-2 | `frmBackup` بلا Mock إطلاقاً — يعمل على قاعدة البيانات الحقيقية فقط | `IBackupDesktopService` · `DirectBackupService` · `frmBackup.cs` · `Program.cs` | `ui-agent` | **مانع للمراجعة · مجمِّد لباك-إند النسخ الاحتياطي** |
| ف-3 | `MockInventoryDesktopService` بلا أي رفض — يجب التحقق من أنه قراءة فقط، وإلا فالتكافؤ ناقص | `Services/Mock/MockInventoryDesktopService.cs` | `mock-qa` يفحص · `ui-agent` يصلح | مفتوح (تحقّق) |
| ف-4 | `MockSupplierPaymentDesktopService` بلا رفض (المبلغ > المتبقي يجب أن يُرفض) | `Services/Mock/MockSupplierPaymentDesktopService.cs` | `ui-agent` | مفتوح (تكافؤ) |

> **قاعدة:** فجوة `RULE-N173` لا تُغلق بتعديل الشاشة — تُغلق بإضافة الـMock/seeds/التسجيل، ولا يُعلن أي module «مؤهلاً» في هذا السجل قبل ذلك.

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
| `مؤهل للمراجعة` بلا موافقة مكتوبة | **مقفل**: لا `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` · ترحيل · seeder | إصلاحات UI/UX داخل الـMock · إصلاحات بناء (`RULE-N159`) |
| `مؤهل للمراجعة` + موافقة مكتوبة مؤرَّخة | **مفتوح** — Stage B مسموح للوحدة نفسها فقط | 4 موجات Stage B بالترتيب |
| `مؤهل جزئياً — تكافؤ الرفض ناقص` | **مقفل** حتى يُغلق نقص التكافؤ أو يقبل المالك النقص كتابةً | إصلاح تكافؤ الرفض في `Mock*` |
| `غير مؤهل — فجوة تغطية` | **مجمَّد**: لا عمل على الوحدة أصلاً | إغلاق الفجوة فقط (Mock/seeds/تسجيل) |

**ملاحظات القفل:**
- القفل **لا يُعيد فتح وحدة مغلقة** ولا يُلغي عملاً مكتملاً؛ يحكم **التغيير التالي** على وحدتها (`RULE-N178`).
- الوحدات المجمَّدة حالياً بحسب §6: **Users/Backup** (ف-1 · ف-2)، وباك-إند **Inventory** و**Payments** لا يُمسّ حتى يُغلق تكافؤ الرفض (ف-3 · ف-4) أو يقبل المالك.
- الاستثناءات الثلاثة المنصوصة في `RULE-N178` (خطأ بناء · أمن عاجل معلَّن للمالك · تصحيح يفرضه `AGENTS.md` بلا تغيير سلوك مالي) تُدوَّن في تقرير التسليم.
- **قاعدة الفحص للـOrchestrator:** أي حزمة عمل (brief) تحمل ملفاً من `Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` تُرفض ما لم تذكر حكم هذا السجل للوحدة + نص الموافقة وتاريخها (`RULE-N160`/`RULE-N177`).

---

## 8) بروتوكول التحديث

1. أي موجة تُضيف/تعدّل `Mock*DesktopService` أو `Mock*Data` أو فرع Mock في `Program.cs` ⇒ تُحدَّث **هذا الملف** في نفس الموجة (`RULE-N174`).
2. الحكم لا يُكتب يدوياً بالحدس: `mock-qa` يفحص ويُصدره، والـOrchestrator ينسخه إلى الجدول ويذكر تاريخ الفحص.
3. الموافقة المكتوبة على الباك-إند تُسجَّل في §7 (تاريخ + نص المالك) **قبل** أول ملف Stage B، ولا يُفتح Stage B بدونهما (`RULE-N177`).
4. لا تُنسخ أرقام هذا السجل داخل `AGENTS.md` أو داخل أي شاشة — يُستهلك بالمرجع (`RULE-N169`).
