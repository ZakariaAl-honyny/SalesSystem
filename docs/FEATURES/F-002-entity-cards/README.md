# F-002 — بطاقات الكيانات وأدوات البحث (`ctrl*Card` · `ctrl*CardWithFilter`)

| الحقل | القيمة |
|---|---|
| `ID` | `F-002` |
| `الحجم` | L (كل وحدة = شريحة واحدة · وكيل واحد لكل وحدة) |
| `المرحلة` | `—` (واجهة فقط) — لا باك-إند ولا هيكل قاعدة بيانات |
| `الحالة` | قيد التنفيذ — موجة 1 (Users · Customers · Suppliers) |
| `الوكيل المالك` | `ui-agent` — وكيل واحد لكل وحدة (`RULE-N156`) |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198`) |
| `أول جولة` | `2026-11-17` |

## ما تفعله هذه الميزة (سطران)

تنسخ نمط مشروع المراجع (`Version 14\DVLD_System`): لكل وحدة كيان بطاقةُ عرضٍ للقراءة فقط `ctrl<Entity>Card`، وبطاقةٌ مع تصفية `ctrl<Entity>CardWithFilter`، ونموذجان رقيقان: `frmShow<Entity>Card` (عرض) و`frmFind<Entity>` (بحث يُعيد المعرّف).
تُستهلك من شاشة الدليل `frm<Entity>Master` عبر زر «بطاقة» — فلا يبقى أي عنصر بلا مستهلك.

## ⚠️ لا تكسر هذه (اقرأها قبل أي تعديل)

1. **`frm<Entity>Master` شاشة مُقرّة تعمل**: لا تُعاد تسمية أي عنصر، ولا يُغيَّر `EntryMode`/`_ApplyModeState`/`_LoadXxxAsync` — الإضافة فقط (زر + معالج + سطر تفعيل داخل `_ApplyModeState`).
2. **العقود لا تُعدَّل** (`I*DesktopService` · `DTO` · `Services/Direct` · `Services/Mock`) — البطاقة تقرأ ما هو موجود (`GetByIdAsync`)، وما لا يوجد (مثل `ICategoryDesktopService.GetByIdAsync`) يُقرأ من `GetAllAsync` داخل عنصر التحكم.
3. **الباك-إند مجمَّد** (`RULE-N177`): صفر ملف في `Business/**` · `DataAccess/**` · `Models/**` · `Transactions`/`Migrations` · `Services/Direct/**`.
4. **لا صور**: مشروعنا لا يملك مجلد `Resources` — البطاقات نصية فقط (تسمية + قيمة + لون حالة).
5. **اللون**: كل لون في `*.Designer.cs` بصيغة `System.Drawing.Color.FromArgb(Convert.ToInt32(n), …)` (`RULE-N204`) ومطابق لقيمة `ThemePalette` الحرفية (`RULE-N134`)، وآخر سطر في التهيئة `GreenThemeHelper.Apply(this)` (`RULE-N50`).

## أين يعيش كودها؟ (بيوت واحدة — لا نسخ)

| الملف | الدور |
|---|---|
| `SalesSystem.Desktop/Screens/<Module>/Controls/ctrl<Entity>Card.cs` | بطاقة عرض للقراءة فقط: `LoadData(dto)` · `Reset<Entity>Info()` · `EntityId` · `Selected<Entity>Info` |
| `SalesSystem.Desktop/Screens/<Module>/Controls/ctrl<Entity>CardWithFilter.cs` | البطاقة + `gbFilters` (`cbFilterBy` + `txtFilterValue` + `btnFind` + `btnAddNew`) + `Attach(IServiceProvider)` + `event <Entity>Selected` + `FilterFocus()` |
| `SalesSystem.Desktop/Screens/<Module>/Forms/frmShow<Entity>Card.cs` | غلاف رقيق: البطاقة + «إغلاق» (+ «تعديل» يفتح شاشة الدليل) |
| `SalesSystem.Desktop/Screens/<Module>/Forms/frmFind<Entity>.cs` | غلاف رقيق: `ctrl<Entity>CardWithFilter` + «إغلاق» يُطلق `DataBack(id)` |
| `SalesSystem.Desktop/Screens/<Module>/Forms/frm<Entity>Master.cs` | **مُعدَّل**: زر `btnShowCard` + معالج + سطر تفعيل داخل `_ApplyModeState` |
| `SalesSystem.Desktop/Program.cs` | تسجيل `frmShow<Entity>Card` و`frmFind<Entity>` في DI (وكيل واحد لكل موجة — لا يكتبه وكيل الوحدة) |

## الحالة الآن

**وحدة `Customers` مُنجَزة (جولة 2 · 2026-11-17)** — بطاقة + بطاقة بتصفية + نموذج عرض + منتقي مُتاح من زرَّي «بطاقة» و«بحث عن عميل» في شاشة الدليل · بناء **0 أخطاء** · مراجعة `مقبول` (0 Blocker/0 Major).
**التالي (وحدة واحدة في كل مرة):** `Suppliers` → `Users` → `Products` → `Categories` → `Units` → `Warehouses` → `CashBoxes` — ولا تُغادر وحدة قبل 100% (أمر المالك 2026-11-17).
**درسا الموجة 1 يُنقلان إلى كل brief:** `WFO1000` (سمات التسلسل على كل خاصية عامة في `UserControl`) و`clsGlobal._OpenForm` يفعّل الموجود ولا يعرض الجديد (افحص `Application.OpenForms` أولاً).
