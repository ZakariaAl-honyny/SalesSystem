# F-004 — الإحاطة (BRIEF)

> **النطاق مغلق.** لا شيء خارج هذا الملف. الخلاف يُحلّ بـ`DECISIONS.md` أو بالتصعيد للـOrchestrator.
> **الشاشة المرجعية الملزمة:** `SalesSystem.Desktop/Screens/Products/Forms/frmProductMaster.cs` + مصمّمها — هي النمط المُشهَد 100% (`RULE-N184`). تُقرأ بالكامل قبل الكتابة.

## 0. القانون الحاكم

`AGENTS.md` §2.21 (قائمة إلزام النموذج) · §2.34 (ترتيب الكتل) · §2.35 (التشغيل = المصمّم) · §2.36 (`EntryMode`) · §2.27 (الستة من `KeyboardHelper`) · §2.30 (`RULE-N169`…`N172` المكان الواحد) · §2.38 (`RequiredFieldHelper`) · §2.33 (مصائد WinForms) · `RULE-N204` (صيغة الألوان `Convert.ToInt32`).

## 1. الهدف

أربع شاشات `frm*Master` مدمجة (محرر + تصفية + ملخص + شبكة) بنمط `frmProductMaster`، تحلّ محل سبع شاشات «قائمة + محرر» قائمة.

## 2. الشاشات والشرائح (نطاق مغلق)

| # | الشاشة الجديدة | مسار الملفات | تُغني | العقد المستهلَك (بلا تعديل) |
|---|---|---|---|---|
| S1 | `frmExpensesMaster` | `Screens/Expenses/Forms/frmExpensesMaster.{cs,Designer.cs,resx}` | `frmListExpenses` + `frmAddUpdateExpense` | `IExpenseDesktopService`: `GetAllAsync(search, from, to)` · `GetByIdAsync` · `CreateAsync` · `UpdateAsync` · `DeleteAsync` · `PostAsync` · `CancelAsync` |
| S2 | `frmCustomerReceiptsMaster` | `Screens/CustomerReceipts/Forms/frmCustomerReceiptsMaster.{cs,Designer.cs,resx}` | `frmListCustomerReceipts` + `frmAddUpdateCustomerReceipt` | `ICustomerReceiptDesktopService`: `GetAllAsync(includeInactive)` · `GetByIdAsync` · `CreateAsync` · `UpdateAsync` · `PostAsync` · `CancelAsync` (لا `Delete`) |
| S3 | `frmSalesReturnMaster` | `Screens/Returns/Forms/frmSalesReturnMaster.{cs,Designer.cs,resx}` | `frmSalesReturn` (+ قائمة المرتجعات المدمجة) | `ISalesReturnDesktopService`: `GetAllAsync(search, from, to, customerId)` · `GetByIdAsync` · `CreateAsync` · `PostAsync` · `CancelAsync` (**لا `UpdateAsync`**) |
| S4 | `frmPurchaseReturnMaster` | `Screens/Returns/Forms/frmPurchaseReturnMaster.{cs,Designer.cs,resx}` | `frmPurchaseReturn` (+ قائمة المرتجعات المدمجة) | `IPurchaseReturnDesktopService`: `GetAllAsync(search, from, to, supplierId)` · `GetByIdAsync` · `CreateAsync` · `PostAsync` · `CancelAsync` (**لا `UpdateAsync`**) |
| S5 | الربط والحذف | `Program.cs` (كتلة DI) · `Screens/frmMain.cs` · `Screens/frmMain.Designer.cs` · حذف 21 ملفاً | الشاشات السبع القديمة | — |

**الشاشات المصدر (مرجع السلوك والحقول — تُقرأ ولا تُعدَّل):** `frmListExpenses.cs` · `frmAddUpdateExpense.cs` · `frmListCustomerReceipts.cs` · `frmAddUpdateCustomerReceipt.cs` · `frmListReturns.cs` · `frmSalesReturn.cs` · `frmPurchaseReturn.cs`.

## 3. خارج النطاق — ممنوع لمسه

1. `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/Interfaces/**` · `Services/Direct/**` · `Services/Mock/**` · `Migrations/**` · `Seeders/**` (`RULE-N177`).
2. أي `Helpers/**` — السلوك الموجود يُستهلك كما هو؛ الناقص **يُبلَّغ** ولا يُكتب (إلا شريحة بيت مشترك مُعلَنة من الـOrchestrator).
3. `Screens/Reports/**` · `frmListSales` · `frmListPurchases` · `frmAddUpdateSalesInvoice` · `frmPurchaseInvoice` · `frmPOS`.
4. الشاشات القديمة السبع: **لا تُحذف ولا تُعدَّل** إلا في S5.
5. `Program.cs`: **ممنوع** في S1–S4 (التسجيل في S5 فقط).

## 4. مواصفة الشكل الملزمة (تُطابق `frmProductMaster` حرفياً)

**ترتيب الأشرطة (Dock):**

| العنصر | Dock | المحتوى |
|---|---|---|
| `lblMode` | Top | مؤشر الوضع — `GreenThemeHelper.ApplyHeader` · Tahoma 14 Bold |
| `pnlFields` | Top | محرر المستند: حقول الرأس داخل تخطيط صريح + `errorProvider` |
| `pnlLines` | Top (المرتجعات فقط) | `gbLines` «أصناف المرتجع»: `dgvItems` + `btnAddItem` + `btnRemoveItem` + `lblTotals` |
| `pnlButtons` | Top | أوامر الشاشة (أدناه) |
| `pnlFilters` | Top | `gbFilter` «بحث وتصفية» + `btnToggleFilters` **معلَن في المصمّم** (`RULE-N145`) |
| `pnlGrid` | Fill | `dgvMain` |
| `pnlSummary` | Bottom | `lblSummary` (عدد + إجماليات `N2`) |

**المصمّم = التشغيل (`RULE-N134`/`N140`):** كل لون يُطبَّق وقت التشغيل مُعلَن في المصمّم بقيمته الحرفية من `GreenThemeHelper`/`ThemePalette`، بصيغة `System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103))` (`RULE-N204`). صفر `SystemColors.Control` · صفر `UseVisualStyleBackColor = true`.

**الستة + الإضافات في المُنشئ بهذا الترتيب:**
```csharp
KeyboardHelper.WireEnterAsTab(pnlFields, _PrimaryActionButton);   // ثنائي الوسيط — RULE-N191/N193
KeyboardHelper.WireSelectAllOnFocus(this);
KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
KeyboardHelper.WireArrowNavigation(this);
KeyboardHelper.WireButtonDisabledVisual(/* كل الأزرار القابلة للتعطيل */);
KeyboardHelper.WireGlobalShortcuts(this,
    (Keys.F2, btnNew.PerformClick), (Keys.F5, () => _RefreshAsync()));
KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
RequiredFieldHelper.Attach(this, errorProvider, /* الحقول الإلزامية بنصوصها العربية */);  // بعد WireFocusHighlight — RULE-N183
FilterPanelHelper.Attach(this, _serviceProvider);                                        // يتبنّى btnToggleFilters المُعلَن
```
**ممنوع** `WireEnterAsTab(this)` أحادي الوسيط · **ممنوع** `_ValidateForm`/`_RequiredFieldsValid`/`_FocusFirstMissingField` محلية (البيت `RequiredFieldHelper` — `RULE-N172`/`PILOT-2`).

**آلة الحالة (`RULE-N150`):** `private enum EntryMode { Browsing, Adding, Editing }` + `_mode` + `_ApplyModeState()` مركزية تُستدعى بعد كل انتقال (تحميل · جديد · اختيار سطر · حفظ · ترحيل · إلغاء · حذف · تحديث · Esc) + `_UpdateModeHeader()` + `_PrimaryActionButton()`:
- `Browsing` ⇒ الزر الأساسي `btnNew`.
- `Adding` ⇒ `btnSave`.
- `Editing` ⇒ `btnSave` (S1/S2) أو `null` (S3/S4 — انظر §5.3).

**الحقول معطّلة في `Browsing`** (`RULE-N149`)، و`AcceptButton = _PrimaryActionButton()` داخل `_ApplyModeState`، و`gbFilter` يبقى مفعّلاً دائماً.

**الشبكة:** `ReadOnly = true` · `AllowUserToAddRows = false` · `SelectionMode = FullRowSelect` · `AutoGenerateColumns = false` بأعمدة معلنة في المصمّم · `ContextMenuStrip` (عرض/تعديل · تحديث) · صف «لا توجد بيانات» · تصفية فورية (`RULE-N24`/`RULE-N60`) بلا زر بحث · `btnClearFilters` منفصل عن `btnRefresh` (`RULE-N148`).

**`gbFilter` القياسي:** `cbFilterBy` + `txtFilterValue` · `cbStatus` (الكل/مسودة/مرحل/ملغي — `DocumentTextHelper.StatusText`) · `dtpFrom` + `dtpTo` + `btnRangeToday`/`btnRangeMonth`/`btnRange30` · `btnClearFilters` · `btnToggleFilters`.

**شبكة السطور (`dgvItems`، المرتجعات):** استثناء `RULE-N22a` — `AllowUserToAddRows = true` · `SelectionMode = CellSelect` · تحرير داخل الخلية · `BindingList<InvoiceLineItem>` · إعادة حساب `LineTotal`/المجاميع في `CellEndEdit` · `btnAddItem`/`btnRemoveItem` · **`_PrimaryActionButton()` تُعيد `null` عند `dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode`** و`AcceptButton = null` عند دخول الشبكة ويُعاد عند `Leave` (`RULE-N153`) · **ممنوع ترحيل من داخل الشبكة**.

**الرسائل:** `clsDialogs.*` فقط · `clsFormat.*` للتنسيق والتحليل المحروس · `clsGlobal._OpenForm` لأي فتح نموذج · صفر `MessageBox` · صفر `ex.Message` للمستخدم · صفر `_notification`/`Notification.Show*`.

## 5. سلوك كل وحدة (يُحفظ حرفياً من الشاشات المصدر)

### 5.1 S1 — `frmExpensesMaster`
- الحقول: نوع المصروف/البند · التاريخ · المبلغ (`N2`) · طريقة الدفع · البيان/الملاحظات · (كما في `frmAddUpdateExpense` و`ExpenseDto` — تُقرأ ولا تُخترع).
- `LiveTafqeetHelper.Attach(liveTip, (numAmount, "المبلغ", LiveTafqeetHelper.ReadMode.Money))`.
- المجموع في `pnlSummary` = `clsFormat.Money`.
- الأزرار: جديد (F2) · حفظ · تعديل · **حذف** (يعمل فقط على مسودة غير مرحّلة، بتأكيد `clsDialogs.Confirm`) · **ترحيل** · **إلغاء المستند** · تحديث (F5) · إغلاق.
- **مسودة مرحّلة/ملغاة:** كل الحقول معطّلة ولا حفظ (`_loadedStatus != Draft` كما في المصدر).

### 5.2 S2 — `frmCustomerReceiptsMaster`
- الحقول: العميل (`ComboBox` + `DisplayMember` — `RULE-N33` trap) · التاريخ · المبلغ · طريقة الدفع · البيان · (طبقاً لـ`CustomerReceiptDto`).
- `LiveTafqeetHelper` للمبلغ · لا زر حذف · «إلغاء المستند» بديل.
- الترحيل يخصم/يقيّد بحسب الخدمة — الشاشة لا تحسب: تستدعي `PostAsync` وتُظهر نتيجة `Result`.

### 5.3 S3/S4 — `frmSalesReturnMaster` / `frmPurchaseReturnMaster`
- **لا وضع تعديل (`D-05`):** `Editing` = العرض للقراءة فقط: الحقول وشبكة السطور `ReadOnly`، و`_PrimaryActionButton()` ⇒ `null`، والأزرار المتاحة: «ترحيل» (مسودة) و«إلغاء المستند» (مرحّل/مسودة). **ممنوع زر حفظ يوهم بالتعديل.**
- **إنشاء مرتجع جديد (`Adding`):** رأس (عميل/مورد · تاريخ · مستودع إن كان في العقد · ملاحظات) + سطور بالباركود/التعديل السريع + مجاميع.
- **ممنوع تكرار المستند:** بعد `CreateAsync` الناجح يُحفظ المعرّف في `_createdId` ولا يُنشأ مستند ثانٍ عند إعادة الضغط — لا `UpdateAsync` في العقد، فالسلوك: يُعرض المستند المُنشأ ويُمنع الإنشاء المكرر (رسالة عربية).
- `frmSalesReturn` الحالي فيه «حفظ مسودة» + «ترحيل» — يُحافظ على المسارين عبر `CreateAsync` ثم `PostAsync`.
- الأصناف: صفر كمية مرفوض عربياً قبل الإرسال (تحقق شاشة)، والكمية `N3`، والمبالغ `N2`.
- شبكة القائمة تُظهر حالة المستند بنص عربي (`DocumentTextHelper.StatusText`) واسم العميل/المورد عبر `DocumentTextHelper.CashCustomerNameOr` حيث ينطبق.

## 6. المصائد الملزمة (RULES_CONSTRAINTS لصيقة بهذه الشريحة)

1. `row.Cells["<اسم حقل منطقي>"]` **ممنوع** — القراءة عبر `GridRowHelper` (`RULE-N179`).
2. `(int)combo.SelectedValue` على معرّف `short` يرمي — استخدم `is short`/`Convert.ToInt32`.
3. `Convert.ToDecimal(cell.Value)` على `DBNull` يرمي — `clsFormat.TryParse*`.
4. مقارنة `dtpTo` بـ`.Date` (شامل اليوم).
5. `_cache.Clear()` **داخل** فرع `IsSuccess` فقط — لا قبله (`RULE`: 9 شاشات مدفوعة).
6. `Form.ActiveControl` يرجع الحاوية — استخدم `KeyboardHelper.IsTextInputFocused(this)`.
7. `ComboBox` بكائنات تحتاج `DisplayMember`.
8. `Clear()`/تفريغ قبل فحص `IsSuccess` = عيب.
9. رسالة نجاح بعد `IsSuccess` فقط — لا صف مُلفَّق ولا «تم الترحيل» بلا `Result`.
10. `catch` واحد عند حدّ الخطأ (`RULE-N205`): `async void` handler + `finally` لاستعادة المؤشر.

## 7. معيار الإنجاز

- [ ] الشاشة تُنشأ وتعمل: تصفية فورية · تحديد سطر · إضافة · حفظ/ترحيل/إلغاء · تحديث تلقائي مع بقاء السطر · `lblMode` صحيح · الأزرار بحسب الوضع.
- [ ] `dotnet build` من الـOrchestrator = **0 Errors** (الوكيل لا يبني — `RULE-N159`).
- [ ] صفر `MessageBox` · صفر `ex.Message` · صفر `_ValidateForm` محلية · صفر نسخة ثانية من سلوك مساعد · صفر `SystemColors.Control` · صفر `Color.FromArgb` مختصر في المصمّم.
- [ ] كل الألوان في المصمّم = قيم `GreenThemeHelper` الحرفية (`RULE-N134`).
- [ ] لا اختبارات آلية — `RULE-N115`/`N117`.

## 8. التقرير المطلوب من كل وكيل

ملفات لمستها (مسار كامل) · قرارات اتخذتها · مخاطر/فجوات أبلغت عنها · **قائمة اختبار يدوي عربية** (# · الشاشة · الخطوة · النتيجة المتوقعة) تغطي المسار السعيد + الرفض + الأثر المالي/المخزني · والسطر الحرفي: `لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)`.
