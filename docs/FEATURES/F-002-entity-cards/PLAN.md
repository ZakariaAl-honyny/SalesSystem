# F-002 — PLAN

## 1) مراجعة المصدر (المشروع المرجعي — `Version 14\DVLD_System`) — مقيس بالأمر

| المرجع | الأسطر | ما يفعله | ما نأخذه |
|---|---|---|---|
| `People\Controls\ctrlPersonCard` | 136 (cs) + Designer | بطاقة قراءة فقط: `LoadPersonInfo(id\|nationalNo)` · `ResetPersonInfo()` · `PersonID` · `SelectedPersonInfo` + رابط «تعديل» يفتح شاشة التحرير | الهيكل والعقد العام للبطاقة |
| `People\Controls\ctrlPersonCardWithFilter` | 200 (cs) + Designer | البطاقة + `gbFilters` (`cbFilterBy` · `txtFilterValue` · `btnFind` · `btnAddNewPerson`) + `OnPersonSelected` + `FilterEnabled` + `ShowAddPerson` + `FilterFocus()` + `LoadPersonInfo(id\|nationalNo)` | هيكل «بطاقة + تصفية» والحدث |
| `People\frmShowPersonInfo` | قصير | غلاف: بطاقة + إغلاق، يحمّل في `ctor`/`Load` | `frmShow<Entity>Card` |
| `People\frmFindPerson` | قصير | غلاف: `ctrlPersonCardWithFilter` + إغلاق يُطلق `DataBack(id)` | `frmFind<Entity>` |
| `Users\Controls\ctrlUserCard` | 83 (cs) + Designer | بطاقة مستخدم تحوي `ctrlPersonCard` + صندوق «User Information» (UserID · UserName · isActive) + رابط تعديل | محتوى بطاقة المستخدم |
| `Users\frmShowUserInfo` | قصير | غلاف يحمّل في `Load` | كما أعلاه |

**فروق هذا المشروع (مقيسة):**

| البند | المرجع | مشروعنا |
|---|---|---|
| طبقة الوصول | `clsUser.FindByUserID` (Business مباشرة) | `I<Entity>DesktopService` من DI — **إلزامي** (RULE-N05/N06) |
| شاشة التحرير | `frmAddUpdateUser` منفصل | شاشات `frm*Master` مدمجة (RULE-N100) ⇒ «تعديل» يفتح شاشة الدليل |
| مصادر الصور | `Properties.Resources` | **لا يوجد `Resources`** في هذا المشروع ⇒ البطاقة نصية |
| اللغة | إنجليزية | عربية RTL + `Tahoma` (RULE-N20/N21/N28) |

## 2) جرد الوحدات (بالأمر — `RULE-N195`)

`Get-ChildItem SalesSystem\SalesSystem.Desktop\Screens -Recurse -Filter "frm*Master.cs"` ⇒ **9 شاشات**:

| # | الوحدة | شاشة الدليل | العقد | `GetByIdAsync` | الحقول المعروضة في البطاقة |
|---|---|---|---|---|---|
| 1 | Users | `frmUserMaster` | `IUserDesktopService` | ✅ | المعرّف · اسم المستخدم · الحالة · مقفل؟ · يجب تغيير كلمة السر؟ · آخر دخول · محاولات الدخول · الصندوق الافتراضي |
| 2 | Customers | `frmCustomerMaster` | `ICustomerDesktopService` | ✅ | المعرّف · الاسم · الهاتف · البريد · العنوان · الرقم الضريبي · حد الائتمان · الرصيد الحالي · الحساب · الحالة |
| 3 | Suppliers | `frmSupplierMaster` | `ISupplierDesktopService` | ✅ | المعرّف · الاسم · الهاتف · البريد · العنوان · الرقم الضريبي · حد الائتمان · الحساب · الحالة |
| 4 | Products | `frmProductMaster` (+ `frmProductUnitMaster`) | `IProductDesktopService` | ✅ + `GetByBarcodeAsync` | المعرّف · الاسم · الباركود · الفئة · الوصف · المخزون الحالي · حد إعادة الطلب · تتبع الصلاحية · أيام تنبيه الصلاحية · موقع الرف · الحالة |
| 5 | Categories | `frmCategoryMaster` | `ICategoryDesktopService` | ❌ (لا يوجد) ⇒ تصفية من `GetAllAsync` | المعرّف · الاسم · الوصف · الحالة |
| 6 | Units | `frmUnitMaster` | `IUnitDesktopService` | ✅ | المعرّف · الاسم · الرمز · الحالة |
| 7 | Warehouses | `frmWarehouseMaster` | `IWarehouseDesktopService` | ✅ (**`Id` من نوع `short`** — ممنوع `(int)SelectedValue`) | المعرّف · الاسم · الهاتف · العنوان · ملاحظات · الحالة |
| 8 | CashBoxes | `frmCashBoxMaster` | `ICashBoxDesktopService` | ✅ | المعرّف · الاسم · الحساب · رمز الحساب · الوصف · الحالة |
| 9 | Accounts | `frmAccounts` (ليست `Master`) | `IAccountDesktopService` | يُقاس عند فتح موجتها | — (مؤجلة: شاشة قائمة+محرر لا `frm*Master`) |

**مستثنى بقياس:** `Screens/Employees/Forms` **فارغ** ولا يوجد `IEmployeeDesktopService` في `Services/Interfaces` ⇒ لا بطاقة للموظفين (لا عقد).

## 3) عقد عنصر التحكم (مشترك بين كل الوحدات — لا يُخترع لكل وحدة)

```csharp
// ctrl<Entity>Card — بلا خدمات (المصمّم ينشئه بلا وسائط)
public partial class ctrl<Entity>Card : UserControl
{
    public int? <Entity>Id { get; }
    public <Entity>Dto? Selected<Entity>Info { get; }
    public event EventHandler? EditRequested;      // الغلاف يقرّر ماذا يفتح
    public void LoadData(<Entity>Dto dto);
    public void Reset<Entity>Info();
}

// ctrl<Entity>CardWithFilter — يبحث عبر الخدمة
public partial class ctrl<Entity>CardWithFilter : UserControl
{
    public void Attach(IServiceProvider serviceProvider);   // إلزامي قبل الاستخدام
    public event EventHandler<<Entity>Dto?>? <Entity>Selected;
    public bool FilterEnabled { get; set; }
    public bool ShowAddNew { get; set; }
    public Task Load<Entity>Async(int id, CancellationToken ct = default);
    public Task Load<Entity>Async(string nameOrBarcode, CancellationToken ct = default);
    public void Reset<Entity>Info();
    public void FilterFocus();
}
```

**نمط الغلافين (رقيق — بلا منطق):**

```csharp
// frmShow<Entity>Card(int id يُمرَّر بعد الحل من DI)
var f = _serviceProvider.GetRequiredService<frmShow<Entity>Card>();
await f.Load<Entity>(id);          // public async Task Load<Entity>(int id)
clsGlobal._OpenForm(f);

// frmFind<Entity>
public event Action<int>? DataBack;  // يُطلق عند «إغلاق» بمعرّف البطاقة المختارة
```

**نقطة الدخول من شاشة الدليل:** زر `btnShowCard` («بطاقة») في `pnlButtons` معلن في المصمّم (`Tahoma 9 Bold` · `GreenThemeHelper.ActionNeutralBackground` حرفياً) + معطّل بلا تحديد، وتفعيله **داخل `_ApplyModeState` وحدها** (`RULE-N150`) — ممنوع ضبط `Enabled` داخل معالج.

## 4) الموجات (حد أقصى 4 وكلاء لكل موجة — `RULE-N159`)

| الموجة | الوكلاء | الملفات | البناء |
|---|---|---|---|
| **1** | 3 × `ui-agent` — Users · Customers · Suppliers | 14 ملفاً لكل وحدة (قائمة مغلقة) | `dotnet build` ⇒ 0 أخطاء |
| **2** | 3 × `ui-agent` — Products · Categories · Units · + 1 × `fast-agent` (تسجيل DI لموجة 1) | كما أعلاه | 0 أخطاء |
| **3** | 2 × `ui-agent` — Warehouses · CashBoxes · + 1 × `fast-agent` (تسجيل DI لموجة 2) | كما أعلاه | 0 أخطاء |
| **4** | 1 × `fast-agent` (تسجيل DI لموجة 3) · 1 × `code-reviewer` (مراجعة واحدة على الشريحة المكتملة) · 1 × `test-engineer` (قائمة اختبار عربي) | — | 0 أخطاء |

**`Program.cs` مالك واحد في كل موجة** — لا يكتبه وكيل وحدة (`RULE-N158`).

## 5) بوابات القبول

1. بناء **0 أخطاء** (الأساس قبل الموجة: `Build succeeded` · `0 Error(s)` · `114 Warning(s)`).
2. كل بطاقة تعرض كل الحقول العربية في جدول §2 — لا حقل ناقص ولا نص إنجليزي.
3. `ctrl<Entity>Card` **صفر نداء خدمة** (`grep I<Entity>DesktopService` = 0 داخل ملفها).
4. `GreenThemeHelper.Apply(this)` آخر سطر · صفر `MessageBox` · صفر لون حرفي بصيغة مختصرة (`RULE-N204`).
5. لا ملف خارج القائمة المغلقة في `git status`.
6. قائمة اختبار يدوي عربية قبل التسليم (`RULE-N119`).
