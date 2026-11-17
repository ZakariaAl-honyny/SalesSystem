# F-002 — USAGE (متى تُستخدم هذه العناصر وكيف — مقيس من المرجع ثم مُطوَّع لمشروعنا)

## 1) ما وجدناه في المرجع (بالأمر — `RULE-N195`)

`Select-String "ctrlPersonCardWithFilter|ctrlPersonCard1|ctrlUserCard"` في مشروع المراجع ⇒ **6 مواضع استخدام** خارج ملفات التعريف:

| موضع الاستخدام | العنصر | الاستخدام |
|---|---|---|
| `Users\frmAddUpdateUser` (11 موضعاً) | `ctrlPersonCardWithFilter` | شاشة **إدخال/تحرير**: اختيار الشخص ثم الانتقال إلى تبويب بيانات الدخول |
| `Users\frmChangePassword` (1) | `ctrlUserCard` | عرض هوية المستخدم الحالي قبل تغيير كلمته |
| `Users\frmShowUserInfo` (1) | `ctrlUserCard` | شاشة **عرض فقط** بواسطة المعرّف |
| `People\frmFindPerson` (1) | `ctrlPersonCardWithFilter` | شاشة **بحث** تُعيد المعرّف إلى منادِيها (`DataBack`) |
| `People\frmShowPersonInfo` (2) | `ctrlPersonCard` | عرض بواسطة المعرّف أو الرقم الوطني |
| `Applications\…Local Driving License` (11) · `Licenses\frmShowPersonLicenseHistory` (5) | `ctrlPersonCard(WithFilter)` | تأكيد هوية صاحب الطلب + البحث عن شخص لبدء طلب جديد |

## 2) القاعدة المستخلصة — **متى** نستخدم كل عنصر

| العنصر | متى يُستخدم | متى **لا** يُستخدم |
|---|---|---|
| `ctrl<Entity>Card` | الكيان **معروف المعرّف** ونريده للعرض فقط: شاشة عرض · تأكيد هوية · سجل · بطاقة داخل تقرير شاشة | حين يكون على المستخدم اختيار الكيان (لا يوجد بحث فيه) |
| `ctrl<Entity>CardWithFilter` | شاشة **إدخال/تحرير** تحتاج أن يختار المستخدم الكيان ثم يُكمل (سند · فاتورة · تسوية · افتتاحي · تحويل) | حين يكون المعرّف معروفاً مسبقاً — يكفي البطاقة العادية |
| `frmShow<Entity>Card` | زر «بطاقة» من شاشة الدليل، أو أي شاشة تُريه سجلاً بمعرّفه | حين يكون الاختيار مطلوباً (استعمل `frmFind<Entity>`) |
| `frmFind<Entity>` | شاشة أخرى تحتاج **اختيار** كيان وإرجاعه | حين لا نحتاج الاختيار (لا تُفتح عبثاً) |

## 3) كيف تُستخدم في الكود (نمط المرجع حرفياً ثم تكييفه)

```csharp
// (أ) عرض: البطاقة تُحمَّل بالمعرّف داخل نموذج العرض
ctrlUserCard1.LoadUserInfo(userId);          // المرجع
// عندنا: الغلاف يجلب Dto ثم يمرّره — البطاقة لا تعرف الخدمة
var dto = await _svc.GetByIdAsync(id, ct);
if (!dto.IsSuccess) { clsDialogs.Warning("تعذر تحميل بيانات المستخدم"); ctrlCard.ResetInfo(); return; }
ctrlCard.LoadData(dto.Value!);

// (ب) اختيار: التركيز للتصفية في وضع الإضافة، وقفلها في وضع التعديل
if (_mode == EntryMode.Adding) ctrlCardWithFilter1.FilterFocus();   // نفس FilterFocus() في المرجع
else                          ctrlCardWithFilter1.FilterEnabled = false; // المرجع: يُقفل بعد الاختيار

// (ج) الحدث: الخطوة التالية تُفعَّل من الحدث لا من نص الحقل
ctrlCardWithFilter1.CustomerSelected += (s, dto) => { _customer = dto; btnNext.Enabled = dto != null; };

// (د) البحث داخل «بطاقة + تصفية»
await ctrlCardWithFilter1.LoadCustomerAsync(idOrNameOrBarcode, ct);  // المرجع: FindNow()
```

## 4) التطويع الإلزامي على مشروعنا (`RULE-N19x`/قياسات)

| الموضوع | المرجع | مشروعنا | السبب |
|---|---|---|---|
| قراءة البيانات | `clsUser.FindByUserID` داخل البطاقة | الغلاف يقرأ عبر `I*DesktopService` ثم يمرّر `Dto` | `RULE-N05/N06` (لا BLL في الشاشة) |
| إعادة البيانات للمنادي | `event DataBack(id)` | `ShowDialog(this)` + `public <Entity>Dto? Selected<Entity>` + `DialogResult.OK` | نمط مشروعنا المقيس: `frmProductSelection`/`frmCustomerSelection`/`frmSupplierSelection` |
| شاشة التحرير | `frmAddUpdateUser` | `frm<Entity>Master` مدمجة | `RULE-N100` |
| بعد الحفظ | `DataBack` يُعيد المعرّف | إعادة قراءة البطاقة (`LoadData`) بعد إغلاق شاشة الدليل، أو `F5` | لا يوجد DataBack في الشاشات المدمجة |
| الصور | `Resources.Male_512`… | لا صور — نص فقط | لا مجلد `Resources` في المشروع (مقيس) |

## 5) مصفوفة الاستهلاك الفعلية في مشروعنا (قياس شاشات الإدخال)

| الشاشة | الكيان المختار اليوم | البديل بعد F-002 |
|---|---|---|
| `frmOpeningStock` · `frmAddUpdateInventoryAdjustment` · `frmStocktakingNew` | `frmProductSelection` (مودال + `SelectedProduct`) | `ctrlProductCardWithFilter` داخل الشاشة (اختياري لاحقاً) |
| `frmAddUpdateSalesInvoice` · `frmPOS` · `frmAddUpdateCustomerReceipt` · `frmCustomerStatement` | `frmCustomerSelection` | `ctrlCustomerCardWithFilter` |
| `frmSupplierStatement` · `frmSupplierPaymentDialog` | `frmSupplierSelection` | `ctrlSupplierCardWithFilter` |

> **ملاحظة نطاق:** F-002 يبني العناصر الأربعة + زر «بطاقة» في شاشة الدليل **فقط**. استبدال المنتقيات داخل شاشات الإدخال = **ميزة تالية** تُسجَّل في `docs/FEATURES/INDEX.md` ولا تُنفَّذ هنا (لا تُوسَّع الشريحة صامتة — §4 خطوة 9c).
