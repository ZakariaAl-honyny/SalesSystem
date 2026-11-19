# F-005 — BRIEF (المواصفة التشغيلية الملزمة لكل وكيل)

> **هذا الملف هو مصدر القيم الوحيد للشريحة.** كل وكيل يقرؤه كاملاً قبل أول سطر، ولا يخترع لوناً خارج الجدول (`RULE-N188`/`RULE-N112`).
> **المرجع البصري الملزم:** `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmCRUDCProductMaster.Designer.cs` — اقرأه قبل الكتابة وقلّد بنيته.

## 1) جدول الألوان الهدف (الجيل الرابع — مقيس من `ThemePalette.cs:172-200`)

| # | الدور | RGB | يُطبَّق على | `ForeColor` |
|---|---|---|---|---|
| 1 | `formBackground` | `170, 220, 170` | `BackColor` الخاص بالنموذج (`BackColor = …` بلا اسم عنصر) | — |
| 2 | `headerBar` | `14, 165, 103` | `lblMode` · `lblTitle` | `White` |
| 3 | `barBackground` | `235, 240, 235` | كل `Panel` · `FlowLayoutPanel` · `TableLayoutPanel` | — |
| 4 | `groupBox` | `235, 245, 235` | كل `GroupBox` | — |
| 5 | `textStrong` | `14, 82, 52` | `lblSummary` (إن كان لونها معلناً في المصمّم) | — |
| 6 | `btnAddNew` | `13, 110, 253` | `btnNew` · `btnAdd` · `btnAddXxx` (زر إضافة فرعي) | `White` |
| 7 | `btnSave` | `25, 135, 84` | `btnSave` | `White` |
| 8 | `btnDelete` | `220, 53, 69` | `btnDelete` | `White` |
| 9 | `btnCancel` | `108, 117, 125` | `btnClose` · `btnCancel` · `btnClearFilters` | `White` |
| 10 | `btnInfo` | `23, 162, 184` | `btnRefresh` · `btnReactivate` · `btnExport` · `btnPrint` · `btnInfo` · `btnManageXxx` · `btnPickXxx` | `White` |
| 11 | `btnWarning` | `255, 193, 7` | `btnEdit` · `btnUpdate` | `30, 41, 59` |
| 12 | `btnSecondary` | `248, 249, 250` | `btnToggleFilters` وأي زر لا يقع في 6–11 | `30, 41, 59` |
| 13 | `gridBackground` | `220, 240, 220` | `dgv.BackgroundColor` | — |
| 14 | `gridHeader` | `14, 165, 103` | `ColumnHeadersDefaultCellStyle.BackColor` | `White` |
| 15 | `gridLine` | `205, 225, 205` | `dgv.GridColor` | — |
| 16 | `gridAlternate` | `245, 252, 245` | `AlternatingRowsDefaultCellStyle.BackColor` | — |
| 17 | `gridCellText` | `33, 37, 41` | `DefaultCellStyle.ForeColor` | — |
| 18 | `selection` (سطر الشبكة) | `255, 255, 180` | `DefaultCellStyle.SelectionBackColor` + `DefaultCellStyle.SelectionForeColor` = `33, 37, 41` | — |
| 18b | `selection` (رأس العمود) | `14, 165, 103` | `ColumnHeadersDefaultCellStyle.SelectionBackColor` + `ColumnHeadersDefaultCellStyle.SelectionForeColor` = `White` | — |

> **18b مقيسة من وقت التشغيل لا مُخترعة:** `GreenThemeHelper.ApplyGrid` يفرض `ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderColor` و`SelectionForeColor = GridHeaderTextColor` — أي **أخضر بنص أبيض**، لا بيج. تعيين البيج على الرأس يُنتج أبيض-على-بيج بتباين 1.1 (`RULE-N140`: المصمّم = التشغيل).
| 19 | `inputWhite` | `White` | `TextBox` · `ComboBox` · `NumericUpDown` · `DateTimePicker` · `MaskedTextBox` | — |

**ألوان الحالة لا تُعلَن في المصمّم مطلقاً:** `DisabledBack (224,224,224)` · `DisabledFore (150,150,150)` · `RejectBackground (255,220,220)` — تُطبَّق وقت التشغيل (`RULE-N154`/`RULE-N182`).

## 2) صيغة الكتابة (إلزامية — `RULE-N204`)

```csharp
// صحيح
lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
txtName.BackColor = System.Drawing.Color.White;
lblMode.ForeColor = System.Drawing.Color.White;

// ممنوع
lblMode.BackColor = Color.FromArgb(16, 150, 96);          // صيغة مختصرة + قيمة متقاعدة
txtName.BackColor = Color.White;                           // لون مسمّى غير مؤهَّل
lblMode.BackColor = Color.FromArgb(((int)(((byte)(14))))…); // شكل التحويل الصريح
```

## 3) قواعد التنفيذ لكل ملف

1. **اقرأ المصمّم كاملاً أولاً** وابنِ خريطة: اسم العنصر ← نوعه ← دوره في الجدول أعلاه.
2. **خلفية النموذج** → سطر 1. إن غاب السطر أضفه في المكان الذي يضعه فيه المصمّم عادةً (بعد أسطر `ClientSize`/`Name`/`Text` — التزم بترتيب المرجع).
3. **كل حاوية** (`Panel`/`FlowLayoutPanel`/`TableLayoutPanel`) → سطر 3 · **كل `GroupBox`** → سطر 4. لا تترك حاوية على `Color.White` ولا تحذف سطر `BackColor` (استبدل القيمة).
4. **كل حقل إدخال** → `System.Drawing.Color.White` (أضف السطر إن غاب).
5. **كل زر/قائمة** → `FlatStyle = FlatStyle.Flat;` + `UseVisualStyleBackColor = false;` (إن كان السطر موجوداً) + لون الدور من 6–12 + `ForeColor` الصحيح. **التحذير/الثانوي بنص داكن `30,41,59` لا أبيض** (الأبيض عليه تباين 1.9 — غير مقروء).
6. **الشبكة**: طبّق 13–18 على أنماط الخلايا الستة القائمة (لا تنشئ `DataGridViewCellStyle` إضافياً؛ عدّل الموجود). إن كانت قيمة نمط غير معلنة، أضف السطر بنفس نمط المرجع حرفياً.
7. **`lblEmptyState`/تسميات الحالة**: لا تلمس ألوانها إن كانت من لوحة الحالة؛ إن كانت `Color.White` على حاوية فاتحة اجعلها `textStrong` (`14,82,52`).
8. **الشفافية** `BackColor.A < 255` (مثل `Color.FromArgb(100, 255, 255, 255)`) — **لا تُمسّ إطلاقاً** (`RULE-N129`).
9. **حوّل كل لون في الملف** إلى صيغة §2 — بما فيها ألوان لم تُغيَّر قيمتها (تحويل تمثيل فقط).
10. **لا تلمس** `ReadOnly` · `SelectionMode` · `AllowUserToAddRows` · `DataSource` · `Dock` · `Anchor` · `Size` · `Location` · أسماء العناصر · الأحداث · `Text` · `Font` (`RULE-N52` + «لا كسر سلوك قائم»).
11. **لا تلمس `frm*.cs`** — code-behind لا يحتاج تغييراً. إن رأيت حاجة، أبلِغ ولا تعدّل.
12. **ممنوع** أي مرجع إلى `Helpers/` داخل المصمّم (`RULE-N128`) — القيم حرفية دائماً.

## 4) فحص القبول الذاتي قبل تسليم الملف (وكيل)

- `BackColor = ... Color.White` على حاوية = **0**
- خلفية النموذج = `170, 220, 170`
- `Color.FromArgb(n, n, n)` مختصر = **0**
- `Color.White` / `Color.Red` / `Color.Empty` غير مؤهّلة بـ`System.Drawing.` = **0**
- كل حقل إدخال له `BackColor = System.Drawing.Color.White;`
- كل زر له `BackColor` + `ForeColor` من الجدول + `FlatStyle.Flat`

الأمر المقترح (بدّل `<file>`):

```powershell
Select-String -Path "<file>" -Pattern "Color\.FromArgb\(\d+, \d+, \d+\)|(?<!System\.Drawing\.)Color\.(White|Red|Empty)" -AllMatches
```
