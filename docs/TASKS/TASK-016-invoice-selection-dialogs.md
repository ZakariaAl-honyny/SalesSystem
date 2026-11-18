# TASK-016 — شاشتا اختيار الفاتورة (بيع/شراء) لعرض بياناتها الكاملة واختيارها للمرتجع

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-016` |
| `العنوان` | `frmSalesInvoiceSelection` + `frmPurchaseInvoiceSelection`: قائمة فواتير + بيانات الفاتورة الكاملة (رأس + سطور) + اختيارها لإنشاء مرتجع |
| `الحجم` | M لكل شاشة · **وكيل واحد لكل شاشة** (`RULE-N156`) |
| `المرحلة` | `—` — **لا مسار بيانات جديد**: العقود القائمة تكفي (`GetAllAsync` · `GetByIdAsync`) ⇒ مرحلة واحدة (`RULE-N165` مستثنى) |
| `الحالة` | مفتوحة |
| `الوكيل المالك` | `ui-agent` A للبيع (+`Program.cs`) · `ui-agent` B للشراء |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |
| `التاريخ` | `2026-11-18` |
| `طلب المالك` | «we need button for open and select Invoices for full data about an Invoice and for return that Invoice and help end user to select and full sales & Purchases to returns Invoices easily» |

## 1. الهدف

شاشتان للاختيار: يفتحها المستخدم من شاشة المرتجع، فيرى **قائمة الفواتير** بمنقّحات (بحث · من/إلى · الحالة)، وعند تحديد أي فاتورة تظهر **بياناتها الكاملة** (الرأس + السطور + المبالغ)، ثم «اختيار» تُعيد الفاتورة إلى شاشة المرتجع لتعبئتها.

## 2. خارج النطاق (ممنوع بالاسم)

- `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` (العقود كافية — **لا إضافة عضو**).
- `Program.cs`: **مالكه الوحيد الوكيل A** (يضيف سطري DI للشاشتين)؛ الوكيل B يُبلّغ ولا يمسّه (`RULE-N114`).
- `frmListSales` · `frmListPurchases` · `frmSalesReturn` · `frmPurchaseReturn` · `frmListReturns` — **تُمسّ في موجة تالية بعد أن تفرغ الجلسة الموازية التي تعدّلها الآن** (قياس: `frmSalesReturn.cs` 11:34 · `frmPurchaseReturn.cs` 11:33).
- `Helpers/**` — يُستهلك فقط.

## 3. الملفات (قائمة مغلقة)

| # | الملف | الوكيل |
|---|---|---|
| 1 | `Screens/Sales/Forms/frmSalesInvoiceSelection.cs` (جديد) | A |
| 2 | `Screens/Sales/Forms/frmSalesInvoiceSelection.Designer.cs` (جديد) | A |
| 3 | `Screens/Sales/Forms/frmSalesInvoiceSelection.resx` (جديد) | A |
| 4 | `Screens/Purchases/Forms/frmPurchaseInvoiceSelection.cs` (جديد) | B |
| 5 | `Screens/Purchases/Forms/frmPurchaseInvoiceSelection.Designer.cs` (جديد) | B |
| 6 | `Screens/Purchases/Forms/frmPurchaseInvoiceSelection.resx` (جديد) | B |
| 7 | `Program.cs` (سطرا DI) | **A فقط** |

## 4. العقود المستهلكة (بلا تغيير)

| الشاشة | الخدمة | الأعضاء |
|---|---|---|
| البيع | `ISalesInvoiceDesktopService` | `GetAllAsync(search, from, to, status)` · `GetByIdAsync(id)` |
| الشراء | `IPurchaseInvoiceDesktopService` | نفسها |

`SalesInvoiceDto` يحمل **`Items`** (السطور) ⇒ نداء `GetByIdAsync` واحد يعطي الفاتورة كاملة. `PurchaseInvoiceDto` نظيره.

## 5. التصميم الملزم

**الهيكل** = `frmListSales` حرفياً: `pnlHeader(56){lblMode}` · `pnlToolbar(64){flpToolbar}` · `pnlFilters(110){gbFilter(102) + btnToggleFilters(140×102)}` · `pnlSummary(46){flpSummary}` · `pnlFooter(58){lblHint}` · `pnlGrid(Fill)`. ترتيب `Controls.Add`: `pnlGrid ← pnlSummary ← pnlFooter ← pnlFilters ← pnlToolbar ← pnlHeader`.

**أدوات الشريط:** `btnSelect` «اختيار» (`25,135,84`) · `btnCancel` «إلغاء» (`108,117,125`) · `btnRefresh` «تحديث» (`23,162,184`).

**المنقّحات داخل `gbFilter`:** `txtSearch` (رقم الفاتورة/اسم الطرف) · `dtpFrom` + `dtpTo` · `cbStatus` («الكل» · «مسودة» · «مُرحَّلة» · «ملغاة» بترتيب `SelectedIndex`) · `btnClearFilters` («مسح التصفية»، معطّل بلا تصفية — `RULE-N148`).

**تقسيم `pnlGrid`:** `dgvInvoices` (Dock Top، ≈300) = القائمة · `lblInvoiceSummary` (Dock Top، 28) = رأس الفاتورة المحددة (رقم · تاريخ · الطرف · الإجمالي · المدفوع · المتبقي · الحالة) · `dgvLines` (Dock Fill) = سطور الفاتورة (الصنف · الكمية · السعر · الخصم · الإجمالي).

**السلوك:**
1. `Load` ⇒ `GetAllAsync(null, null, null, null)` ⇒ ملء الشبكة + «النتائج: N».
2. تغيير المعايير ⇒ نداء `GetAllAsync` بالقيم الحالية (لا تصفية محلية على مهمة مالية) + احترام `.Date` للنطاق من/إلى.
3. تغيير السطر المحدد ⇒ `GetByIdAsync(Id)` ⇒ ملء `dgvLines` و`lblInvoiceSummary`. فشل ⇒ رسالة عربية عبر `clsDialogs` وإبقاء القائمة.
4. «اختيار» معطّل بلا سطر (`RULE-N43`) ويُرفض على **فاتورة ملغاة** برسالة عربية صريحة (`RULE-N167`) — لا مرتجع من ملغاة.
5. الاختيار ⇒ `SelectedInvoice` + `DialogResult.OK` + `Close()`.
6. `Esc` ⇒ إلغاء · `F5` ⇒ تحديث · تركيز `txtSearch` عند الفتح + `WireSearchToGrid(txtSearch, dgvInvoices)` (نمط TASK-015).
7. الستة من `KeyboardHelper` + `GreenThemeHelper.Apply(this)` آخر سطر + `FilterPanelHelper.Attach` آخر سطر في المُنشئ.

**السطح العام:** `public SalesInvoiceDto? SelectedInvoice { get; private set; }` (ونظيره `PurchaseInvoiceDto`) · مُنشئ `(IServiceProvider)` + مُنشئ تصميم خاص · `DialogResult.OK/Cancel`.

**الجمعيات (منتقيان لا واحد):** الاتفاق مع `RULE-N05`/`RULE-N11` — كل كيان بعقده؛ ولا يُنشأ منتقٍ عام يخلط النوعين.

## 6. الموجات

| الموجة | الوكيل | الملفات |
|---|---|---|
| W1 | A + B متوازيان | 1–3 (+7) · 4–6 |

## 7. الأدلة (مقيسة قبل الموجة)

| ما قِيس | القيمة | المصدر |
|---|---|---|
| عقد البيع | `GetAllAsync(search, from, to, status)` + `GetByIdAsync(id)` | `Services/Interfaces/ISalesInvoiceDesktopService.cs` |
| عقد الشراء | نفس التوقيع | `IPurchaseInvoiceDesktopService.cs` |
| «البيانات الكاملة» | `SalesInvoiceDto.Items` موجودة في السجل | `DTO/SalesInvoiceDtos.cs:28` |
| شاشات المرتجع اليوم | بلا أي مسار لاختيار فاتورة (`InvoiceNumber|SelectInvoice` = 0 مطابقة) | `frmSalesReturn.cs` · `frmPurchaseReturn.cs` · `frmListReturns.cs` |
| سابقة اختيار فاتورة | `frmListSales` بـ`btnView` يفتح المحرر | `frmListSales.cs` |

## 8. المخاطر

- **الربط في شاشات المرتجع مؤجَّل** (موجة W2) لأنها تحت التعديل الآن؛ إن رُبطت قبل أن تفرغ الجلسة الموازية فُقد عملها.
- منتقي الفاتورة يعرض **كل** الحالات بما فيها المسودة، والرفض عند الاختيار فقط — مقصود حتى يرى المستخدم لماذا لا تُرتجَع.
- لا `DataView`/تصفية محلية: المبالغ لا تُحسب في الشاشة بل تُعرض من الـDTO.

## 9. الاختبار اليدوي

| # | الشاشة | الخطوة | النتيجة المتوقعة |
|---|---|---|---|
| 1 | الاثنتان | افتح الشاشة | شريط أخضر «اختيار فاتورة بيع/شراء» + شريط أوامر + «بحث وتصفية» + قائمة فواتير + «النتائج: N» |
| 2 | الاثنتان | حدّد فاتورة | تظهر سطورها في الشبكة السفلية ويظهر رأسها (رقم/تاريخ/الطرف/الإجمالي/المدفوع/المتبقي/الحالة) |
| 3 | الاثنتان | غيّر من/إلى ثم الحالة | القائمة تتغيّر فوراً بالأرقام الصحيحة |
| 4 | الاثنتان | «مسح التصفية» | يعود كل شيء بلا إعادة تحميل كاملة |
| 5 | الاثنتان | اختر **فاتورة ملغاة** | رسالة رفض عربية ولا يُغلق النموذج |
| 6 | الاثنتان | اختر فاتورة مُرحَّلة ثم «اختيار» | يُغلق بـ`OK` و`SelectedInvoice` يحمل الفاتورة **بسطورها** |
| 7 | الاثنتان | `Esc` | إلغاء بلا تعيين |
| 8 | الاثنتان | المؤشر عند الفتح ثم ↓ ثم Enter | يبدأ في البحث · ↓ إلى القائمة · Enter يختار |

## 10. سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-18` | إنشاء المهمة بعد قياس عقود الفواتير وشاشات المرتجع |
