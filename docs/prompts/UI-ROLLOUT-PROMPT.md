# برومبت جاهز — تعميم معيار `frmProductMaster` على بقية الشاشات

> انسخ النص من «بداية البرومبت» حتى «نهاية البرومبت» والصقه في أي جلسة جديدة.
> حدّث فقط **§7 دفعات التنفيذ** بالشاشات المسنَدة لتلك الجلسة.

---

## بداية البرومبت

**الدور:** وكيل تنفيذ WinForms (.NET 10) داخل حل `SalesSystem.slnx`.
**المهمة:** رفع شاشات `SalesSystem.Desktop/Screens/**` إلى معيار الشاشة المرجعية `frmProductMaster` — ثيم أخضر + تنقل لوحة مفاتيح + `gbFilter` + `pnlSummary` + شبكة قياسية + «المصمّم = وقت التشغيل».
**السلطة العليا:** `AGENTS.md` في جذر المستودع. أي تعارض بين هذا البرومبت و`AGENTS.md` يُحسم لصالح `AGENTS.md`.

### 0) اقرأ قبل أن تكتب حرفاً

1. `AGENTS.md` — خصوصاً: §2.12 · §2.18 · §2.19 · §2.21 · §2.22 · §2.27 · §2.28 · §2.29 · §2.30 · §2.31 · §2.33 · §2.34 · §2.35 · §4.
2. الشاشة المرجعية كاملة:
   - `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmProductMaster.cs`
   - `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmProductMaster.Designer.cs`
3. الـ Helpers (المصدر الوحيد للسلوك المشترك — لا نسخ داخل الشاشات):
   - `SalesSystem/SalesSystem.Desktop/Helpers/KeyboardHelper.cs`
   - `.../Helpers/GreenThemeHelper.cs`
   - `.../Helpers/FilterPanelHelper.cs`
   - `.../Helpers/ThemeHelper.cs`
   - `.../Helpers/RowFilterHelper.cs`
   - `.../Helpers/LiveTafqeetHelper.cs`
   - `.../Helpers/ExportHelper.cs`
   - `.../Helpers/ReportUiStandardizer.cs` + `ReportNavigationHelper.cs` (للتقارير فقط)
4. للشاشة المسنَدة إليك: اقرأ **الملفات الثلاثة كاملة** (`frmX.cs` + `frmX.Designer.cs` + `frmX.resx`) قبل أي تعديل.

### 1) تشريح الشاشة المرجعية (هذا هو الهدف البصري والوظيفي)

| الكتلة | عناصر المصمّم | القاعدة |
|---|---|---|
| شريط العنوان | `lblMode` (Dock=Top) يعرض «إضافة … جديد» / «تعديل …: الاسم» | RULE-N44 |
| المحرر | `pnlFields` + تسميات `Tahoma` + حقول بيضاء + `grpMainUnit` | §2.34 · RULE-N143 |
| الأوامر | `pnlButtons` (FlowLayoutPanel): جديد · حفظ · تعديل · حذف · إعادة تنشيط · إدارة الوحدات · إغلاق | RULE-N43 |
| المحتوى | `pnlGrid` + `dgvProducts` | RULE-N22 |
| التصفية | `pnlFilters` (Dock=Top، ارتفاع 112) يحتوي `btnToggleFilters` (Dock=Left، 140×104) + `gbFilter` (Dock=Fill، «بحث وتصفية»، Padding=8) | RULE-N56…N63 · RULE-N145 |
| الملخص | `pnlSummary` + `lblSummary` (`TextStrong`، Tahoma 10 Bold، ارتفاع 32، «النتائج: N» وإجماليات N2) | RULE-N61 |

ترتيب الكتل الرأسي **ثابت**: العنوان ← الأوامر ← التصفية ← المحتوى ← الإجماليات ← الأسفل (RULE-N135…N138). ممنوع صندوق داخل صندوق أو كتلة بلا صندوق.

### 2) العمل المطلوب داخل كل شاشة — بالترتيب

1. **الهيكل**: أعد ترتيب/تجميع الكتل حسب الجدول أعلاه مع الحفاظ على أسماء العناصر والأحداث القائمة (ممنوع كسر سلوك مستدعى من خارج الشاشة).
2. **الستة من `KeyboardHelper`** في المُنشئ (RULE-N95):

```csharp
KeyboardHelper.WireEnterAsTab(this);
KeyboardHelper.WireSelectAllOnFocus(this);
KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate); // RULE-N43
KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
    (Keys.F2, () => btnNew.PerformClick()),
    (Keys.F5, () => btnRefresh.PerformClick()),
    (Keys.Delete, _GlobalDelete),
    (Keys.Escape, _GlobalEscape));
```

آخر سطر في المُنشئ دائماً:

```csharp
FilterPanelHelper.Attach(this, _serviceProvider);   // RULE-N63
```

3. **آلة الحالة + `lblMode`** (شاشات الإدخال/الدليل فقط): `Browsing / Adding / Editing`؛ الحقول معطّلة في `Browsing`؛ `_UpdateButtonsState()` + `_UpdateModeHeader()` عند كل تغيير.
4. **`gbFilter`** حسب §2.22: `cbFilterBy` + `txtFilterValue` (أو `txtSearch` للشاشات أحادية البحث كما في النموذج المرجعي) · `cbStatus` · `dtpFrom`/`dtpTo` + `btnRangeToday`/`btnRangeMonth`/`btnRange30` · `chkShowInactive`/`chkOnlyDue`/`chkOnlyCancelled`/`chkOnlyOutOfStock` · `btnToggleFilters`. **كل أدوات التصفية داخل `gbFilter` بلا استثناء** (RULE-N62).
5. **تصفية فورية** على `TextChanged`/`CheckedChanged`/`SelectedIndexChanged` — لا زر «بحث»؛ `Esc` يمسح التصفية (RULE-N59). أي `RowFilter` يُبنى حصراً بـ `RowFilterHelper.Like/Escape` وعلى أعمدة معروفة (RULE-N24/N60) — ممنوع إدراج إدخال المستخدم خاماً.
6. **`pnlSummary`**: عدد النتائج + الإجماليات بصيغة `N2`، ويُعاد حسابه بعد كل تصفية (RULE-N61).
7. **`DataGridView`**: `ReadOnly = true` · `AllowUserToAddRows = false` · `SelectionMode = FullRowSelect` · `ContextMenuStrip` لعمليات السطر · نقر مزدوج · سطر «لا توجد بيانات» (RULE-N22/N23). الاستثناءان الوحيدان المسموحان: `dgvItems` في `frmAddUpdateSalesInvoice` و`dgvCart` في `frmPOS` (RULE-N22a/N83).
8. **التحقق**: `errorProvider` + رسائل عربية (RULE-N28). ممنوع `ex.Message` للمستخدم — Serilog فقط (RULE-N29).
9. **الثيم** — ترتيب الإلزام داخل `ApplyUiTheme()`:

```csharp
ThemeHelper.ApplyMainFormStyle(this);
ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Primary);
// ... بقية الأزرار

GreenThemeHelper.ApplyHeader(lblMode);
GreenThemeHelper.ApplyGrid(dgvX);        // لكل شبكة
GreenThemeHelper.Apply(this);            // ← آخر سطر دائماً (RULE-N50)
```

10. **المصمّم = وقت التشغيل** (RULE-N134/N140…N144): كل لون يطبّقه وقت التشغيل يُعلَن بقيمة حرفية مطابقة في `*.Designer.cs`؛ حقول الإدخال بيضاء + `Tahoma`؛ ممنوع `SystemColors.Control` أو `UseVisualStyleBackColor = true` على أزرار/حقول الشاشات الخضراء؛ ممنوع أي مرجع لعضو في `Helpers/` داخل المصمّم (RULE-N128).
11. **المال والكميات**: `decimal` وعرض `N2`؛ الكميات 3 منازل (RULE-001/002).
12. **التفقيط الحيّ** في شاشات إدخال المال: `LiveTafqeetHelper.Attach(liveTip, …)` (RULE-N97).
13. **دورة إعادة التنشيط** (RULE-N90…N94): زر «إعادة تنشيط» مفعّل على السطر **غير النشط فقط** + `chkShowInactive`.
14. **لا كود ميت**: إن كان للكيان `frm*Master` فلا يجوز بقاء `frmAddUpdate*`/`frmList*` له (RULE-N102) — **لا تحذف قبل موافقة المدير**، بل أبلغ.
15. **لغة الكود**: WinForms كلاسيكي (code-behind)، بلا ViewModel/ICommand، بلا تجريدات جديدة، بلا `HttpClient` وبلا أي مرجع إلى `SalesSystem.Business` داخل `Screens` (RULE-N109/N110/N111).
16. **التوكنات**: لا `async void` بلا `try/catch`، ولا `Cursor.WaitCursor` بلا استعادة في `finally` (RULE-N25/N26).

### 3) حالة الشاشات — مسح فعلي (أُعيد التحقق منه قبل التنفيذ)

إجمالي النماذج `frm*`: **93**. منها **31 تقريراً**. الشاشات التي تستدعي الستة كاملة: **23**. الشاشات بلا أي استدعاء: **40**.

**المجموعة A — تستدعي الستة كاملة (مراجعة/تدقيق فقط، لا إعادة بناء):**

```
Accounting/frmAccounts · Accounting/frmJournalEntries · CashBoxes/frmCashBoxMaster ·
Categories/frmCategoryMaster · CustomerReceipts/frmListCustomerReceipts · Customers/frmCustomerMaster ·
Employees/frmEmployeeMaster · Expenses/frmListExpenses · Inventory/frmListInventoryAdjustments ·
Inventory/frmListInventoryBatches · Inventory/frmListInventoryTransactions · Payments/frmListPayments ·
Payments/frmListSupplierPayments · POS/frmPOS · Products/frmProductMaster · Products/frmProductUnitMaster ·
Purchases/frmListPurchases · Returns/frmListReturns · Sales/frmListSales · Suppliers/frmSupplierMaster ·
Units/frmUnitMaster · Users/frmUserMaster · Warehouses/frmWarehouseMaster
```

> في هذه المجموعة تحقّق من: `ContextMenuStrip` (ناقص في معظمها) · `btnToggleFilters` **معلَن في المصمّم** · `pnlSummary` · تطابق ألوان المصمّم · `LiveTafqeetHelper` في الشاشات المالية.

**المجموعة B — تنقل جزئي (4 من 6) — تحتاج استكمالاً فعلياً:**

```
CashBoxes/frmCashBoxTransactions · CustomerReceipts/frmAddUpdateCustomerReceipt ·
Customers/frmCustomerSelection · Customers/frmCustomerStatement · Expenses/frmAddUpdateExpense ·
Payments/frmSupplierPaymentDialog · Products/frmProductSelection · Purchases/frmPurchaseInvoice ·
Returns/frmPurchaseReturn · Returns/frmSalesReturn · Sales/frmAddUpdateSalesInvoice ·
Suppliers/frmSupplierSelection · Users/frmBackup · Users/frmPasswordChange
```

> الغالب الناقص: `WireButtonDisabledVisual` + `WireGlobalShortcuts` + `FilterPanelHelper` + `pnlSummary`.

**المجموعة C — بلا أي استدعاء (بناء من الصفر للمعيار):**

```
Inventory/frmInventory · Inventory/frmInventoryList · Inventory/frmInventoryAddProduct ·
Inventory/frmAddUpdateInventoryAdjustment · Inventory/frmInventoryFifo · Inventory/frmInventoryBarcode ·
Inventory/frmOpeningStock · Inventory/frmStocktakingNew · Inventory/frmWarehouseTransferNew ·
Purchases/frmPurchaseItem · Purchases/frmPurchaseItemInfo · Purchases/frmPurchaseDiscount ·
Purchases/frmPurchaseCurrency · Purchases/frmPurchaseOtherDetails · Purchases/frmSupplierStatement ·
Purchases/frmPurchaseInvoicePrint · Inventory/frmStocktakingPrint · Inventory/frmStocktakingPrintAdjustment ·
Inventory/frmStocktakingPrintCard · Inventory/frmWarehouseTransferPrint · Products/frmImportProductsExcel ·
Tools/frmCalculator · Dashboard/Forms/frmDashboard · frmLogin · frmMain
```

### 4) الاستثناءات (لا تُخرِق المعيار بلا سبب)

| النوع | الاستثناء | المرجع |
|---|---|---|
| 31 تقريراً (`Reports/Forms/**`) | تُدار حصراً بـ `ReportUiStandardizer.ApplyForm(this)` + `ReportNavigationHelper.Attach(this, _serviceProvider)` + تصدير Excel (`IExportableReport`)؛ **تُعفى** من الستة ومن `gbFilter` | RULE-N73/N121 |
| شاشات طباعة/معاينة (`frm*Print*` · `frmStocktakingPrint*`) | قراءة فقط؛ معفاة من تعبئة النماذج والتنقل — لكن: ثيم أخضر + RTL + Tahoma + زر الإغلاق ظاهر فوق أي شريط مثبّت (RULE-N125) | §2.20 P3 |
| منتقيات (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection`) | قراءة فقط: بلا `pnlSummary`/`gbFilter` إلزامي، لكن تنقّل الشبكة بلوحة المفاتيح + `Enter` اختيار + `Esc` إلغاء + ثيم | §2.20 P2 |
| حوارات (`frmPurchaseItem` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` · `frmSupplierPaymentDialog` · `frmPasswordChange` · `frmBackup`) | ثيم + تنقّل + تحقق عربي؛ بلا `gbFilter`/`pnlSummary` | — |
| `frmLogin` · `frmMain` · `frmDashboard` · `frmCalculator` | لا تُعامَل كشاشات إدخال؛ ثيم + RTL + Tahoma فقط (لوحة التحكم تبقى `ctrlDashboard` — RULE-N64…N69) | §2.23 |

### 5) محرّمات صريحة

- ❌ لا تعدّل `Program.cs` · `appsettings.json` · `*.csproj` · `SalesSystem.slnx` · `AGENTS.md` · `SalesSystem.Tests/**` — **أبلِغ بالحاجة فقط** (RULE-N114/N115).
- ❌ لا حزم NuGet جديدة، ولا واجهات/مصانع/وسطاء جدد (RULE-N113).
- ❌ لا تسجيل خدمات جديدة ولا لمس `Services/Direct`/`Services/Mock`/`Services/Interfaces`.
- ❌ لا تحذف شاشة أو ملفاً — اقترح الحذف للمدير.
- ❌ لا `dotnet test`، ولا تشغيل التطبيق (RULE-N117). المسموح: `dotnet build` فقط.
- ❌ لا تكتب ألواناً رقمية داخل الشاشة — من ثوابت `GreenThemeHelper` فقط (RULE-N54).
- ❌ لا تُكرّر سلوكاً موجوداً في Helper داخل الشاشة (RULE-N98/N112).

### 6) بوابة القبول والتسليم

**أ. البناء (إلزامي):**

```powershell
dotnet build SalesSystem\SalesSystem.slnx
```

صفر أخطاء. تحذيرات جديدة = تُذكر.

**ب. تقرير التسليم لكل شاشة — جدول:**

| الشاشة | الكتل المضافة/المعاد ترتيبها | استدعاءات KeyboardHelper (6/6) | gbFilter | pnlSummary | ألوان المصمّم مطابقة | ContextMenu | RULE المختبَرة |
|---|---|---|---|---|---|---|---|

**ج. قائمة الاختبار اليدوي (RULE-N116) — جدول:**

| الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|
| … | `F2` ثم تعبئة الحقول بـ `Enter` | مؤشر الوضع = «إضافة … جديد» والأزرار صحيحة |
| … | اختيار سجل من الشبكة | الوضع = «تعديل …: الاسم» و«حذف/تعديل» مفعّلان |
| … | كتابة في `txtFilterValue` | تصفية فورية + تحديث `pnlSummary` |
| … | `F9` / زر «إخفاء التصفية» | طيّ الصندوق لارتفاع 34 وحفظ الحالة بعد إعادة الفتح |
| … | تعطيل سجل ثم `chkShowInactive` ثم «إعادة تنشيط» | نجاح + تحديد السطر + رسالة عربية |
| … | `Esc` داخل `gbFilter` | مسح كل التصفية |

**د. الرفض:** أي شاشة لا تستوفي §2.21 و§2.27 و§2.33 تُعدّ غير مكتملة (RULE-N55/N99) — لا تُسلَّم.

### 7) دفعات التنفيذ (عدّل هذه القائمة بما يُسنَد إليك فقط)

| الدفعة | الشاشات | ملاحظة |
|---|---|---|
| 1 | `frmAddUpdateSalesInvoice` · `frmPurchaseInvoice` · `frmSalesReturn` · `frmPurchaseReturn` | مستندات يومية — الأهم |
| 2 | `frmAddUpdateExpense` · `frmAddUpdateCustomerReceipt` · `frmCashBoxTransactions` · `frmSupplierPaymentDialog` | مستندات مالية + تفقيط |
| 3 | `frmCustomerStatement` · `frmSupplierStatement` · `frmInventory*` · `frmOpeningStock` · `frmStocktakingNew` · `frmWarehouseTransferNew` | مخزون |
| 4 | `frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection` · `frmBackup` · `frmPasswordChange` · `frmImportProductsExcel` · `frmPurchaseItem*` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` | منتقيات/حوارات |
| 5 | مراجعة المجموعة A (تدقيق فقط) | لا إعادة بناء |

### 8) تفاصيل مصمّم حسّاسة (WinForms Pitfalls)

- أضف `Controls.Add(x)` لكل عنصر معلن — لا عنصر معلن وغير مضاف (RULE-N130).
- لا تُعِد ترتيب `Controls` لعنصر مراسٍ (Dock) — يقلب الإرساء ويغطي الشريط السفلي (RULE-N125).
- عند نقل عنصر بين حاويات: صفّر `Anchor` أولاً ثم `Controls.Add` (RULE-N126).
- `btnToggleFilters` **يُعلَن في `*.Designer.cs`** بـ `Dock = Left` داخل `pnlFilters`؛ الـ Helper يتبنّاه ولا ينشئه (RULE-N145).
- الحاويات التي ورثت خلفية أبيها (Ambient) أو لوّنها المصمّم بلون خاص **لا تُعاد تلوينها** وقت التشغيل (RULE-N51a/N139).
- ملفات `.Designer.cs` بلا أي مرجع إلى `Helpers/` (RULE-N128).
- المسار `Dashboard/Forms` هو الصحيح — صُحِّح بنقل خالص بلا تغيير `namespace`/DI (اسم المجلد كان مكتوباً `Froms` سابقاً — RULE-N124 مغلق).

### 9) ملاحظات تنفيذية

- الملفات UTF-8: حافظ على العربية سليمة ولا تُنتج نصاً مشوّهاً (mojibake). بعد كل تعديل، أعد قراءة الملف للتحقق.
- شاشة واحدة لكل التزام/كوميت، ورسالة الكوميت: `ui(<الوحدة>): <الشاشة> — مطابقة معيار frmProductMaster`.
- لا تعمل على أكثر من شاشة في الملف نفسه في الجلسة الواحدة بلا طلب صريح.
- إن اكتشفت تعارضاً في `AGENTS.md` أو انحرافاً في النموذج المرجعي — أبلِغ ولا تُقرِّر من نفسك.

## نهاية البرومبت
