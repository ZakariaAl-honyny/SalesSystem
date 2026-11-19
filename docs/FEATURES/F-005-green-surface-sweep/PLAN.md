# F-005 — الخطة (PLAN)

## الموجات (≤ 4 وكلاء لكل موجة · البناء بعد كل موجة — `RULE-N159`)

| الموجة | الشاشات | الملفات | الوكلاء | الحالة |
|---|---|---|---|---|
| **W1** | 8 شاشات `frmCRUD*Master` غير المرجعية: `CSupplier` · `Customer` · `CProductUnit` · `CashBox` · `Category` · `Unit` · `User` · `Warehouse` | 8 مصمّماً | 3 `ui-agent` | 🔄 جارية |
| **بوابة** | **شهادة المالك البصرية على W1** قبل أي تعميم (`RULE-N184`) | — | — | ⏳ |
| **W2** | المستندات والقوائم: `frmListSales` · `frmAddUpdateSalesInvoice` · `frmListPurchases` · `frmAddUpdatePurchaseInvoice` · `frmListReturns` · `frmListPayments` · `frmListSupplierPayments` · `frmSupplierPaymentDialog` | 8 | 2–3 | ⏳ |
| **W3** | المخزون: `frmInventory` · `frmInventoryList` · `frmListInventoryTransactions` · `frmListInventoryBatches` · `frmListInventoryAdjustments` · `frmAddUpdateInventoryAdjustment` · `frmStocktakingNew` · `frmWarehouseTransferNew` | 8 | 2–3 | ⏳ |
| **W4** | المنتقيات + البطاقات + `POS` + `frmCalculator` + `frmImportProductsExcel` + `frmCustomerStatement` + `frmSupplierStatement` + `frmPurchaseItem`/`frmPurchaseItemInfo`/`frmPurchaseDiscount`/`frmPurchaseOtherDetails`/`frmPurchaseCurrency` | ~12 | 3–4 | ⏳ |
| **W5** | التقارير (26 مصمّماً — بنية متطابقة ⇒ دفعات 4) | 26 | 4 × 2 موجة | ⏳ |
| **W6** | الهيكل والإعدادات: `frmMain` · `ctrlDashboard` · `frmLogin` · `frmPasswordChange` · `frmBackup` · `frmChangeSystemColorsSettings` · `frmFormatingSetting` · `frmListCashBoxTransactions` + عناصر التحكم | ~10 | 3 | ⏳ |

> الأرقام أعلاه تقديرية حتى تُولَّد دفعاتها من `MANIFEST.md` بالأمر عند كل موجة (لا تُنسخ من الذاكرة — `RULE-N196`).

## ترتيب العمل داخل كل شاشة

1. خريطة العناصر (اسم ← نوع ← دور).
2. خلفية النموذج.
3. الحاويات (`Panel`/`FlowLayoutPanel`/`TableLayoutPanel` ← `235,240,235`) ثم `GroupBox` ← `235,245,235`).
4. الحقول ← `White`.
5. الأزرار ← الدور + `ForeColor` + `FlatStyle` + `UseVisualStyleBackColor`.
6. الشبكة ← 13–18.
7. تحويل كل الألوان إلى صيغة `Convert.ToInt32` + تأهيل الألوان المسمّاة.
8. الفحص الذاتي (§4 من `BRIEF.md`).

## المخاطر وضوابطها

| الخطر | الضابط |
|---|---|
| كسر بناء بسبب تعديل مصمّم | البناء بعد كل موجة؛ الخطأ يُوجَّه لمالك الملف (`RULE-N159`). |
| إعادة تلوين مساحة بيضاء مقصودة (صورة/معاينة) | الشفافية لا تُمسّ · نوافذ الطباعة مستثناة · أي حاوية بيضاء مقصودة تُبلَّغ لا تُغيَّر. |
| تغيير سلوك الشبكة | ممنوع لمس `ReadOnly`/`SelectionMode`/`AllowUserToAddRows`/`DataSource`. |
| تضارب ملفات بين وكلاء | مالك واحد لكل ملف (`RULE-N158`) — الدفعات منفصلة. |
| تعميم قبل الشهادة | بوابة W1 في `PLAN` و`RULE-N184`. |
