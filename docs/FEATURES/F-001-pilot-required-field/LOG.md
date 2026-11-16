# F-001 — سجل الجولات (LOG)

> إضافي فقط. الجولة الجديدة تُضاف في الأسفل.

## جولة 1 — 2026-11-16 · الشاشة التجريبية

**ما جرى:** بُنيت التغذية (أيقونة + رسالة + ومضة) وحجب `Enter` على الحقل المطلوب الفارغ، وجعل `Enter` يمشي بترتيب العرض ثم يسلّم التركيز لزر الوضع بلا تنفيذ — على `frmProductMaster` وحدها.
**الملفات:** `Helpers/RequiredFieldHelper.cs` · `Helpers/KeyboardHelper.cs` · `Helpers/NavigationOptions.cs` · `Global Classes/clsInputFeedback.cs` · `Screens/Products/Forms/frmProductMaster.cs` + `.Designer.cs`
**الحجم:** L · **الموجة:** W1 · **الوكيل:** `ui-agent`

### الأدلة (مقيسة 2026-11-16 — الشجرة الحالية)

| ما قِيس | القيمة | الأمر | المخرج |
|---|---|---|---|
| ملفات `.cs` تحت `Screens` | 191 | `Get-ChildItem Screens -Recurse -Filter *.cs` | `191` |
| `errorProvider.SetError` | **188** موضعاً / **28** ملفاً | `$f \| Select-String "errorProvider\.SetError"` | `188` / `28` |
| `RequiredFieldHelper.Attach` | **1** (التجريبية فقط) | `$f \| Select-String "RequiredFieldHelper\.Attach"` | `1` |
| `BlocksEnterNavigation` في `Screens` | **0** (يُستهلك في `KeyboardHelper` وحده) | `$f \| Select-String "BlocksEnterNavigation"` | `0` |
| `WireEnterAsTab` | 65 موضعاً | `$f \| Select-String "WireEnterAsTab"` | `65` |
| `AcceptButton =` | 85 موضعاً / 66 ملفاً | `$f \| Select-String "AcceptButton\s*="` | `85` / `66` |

### المراجعة

`code-reviewer`: لم تُجرَ مراجعة مستقلة على **هذه الميزة** في جولتها الأولى (الملاحظات المُصلحة موثّقة في `AGENTS.md` §2.39 كـ`PILOT-10`…`PILOT-14`) — تُجرى **مرة واحدة** عند إغلاق المرحلة 1.

### الكومِت

- `e495a4d` — `fix(desktop): Enter walks fields in display order, then lands on the mode button`
- `74ba5c5` — `fix(desktop): a required field left empty blocks Enter instead of jumping away`

### العواقب على الجولة القادمة

1. **لا تعميم** قبل شهادة المالك — الشاشة الأخرى الوحيدة المسموحة في الموجة القادمة هي التي يسمّيها المالك.
2. أرقام `AGENTS.md` §2.20 (59 موضع `WireEnterAsTab` · 40 شاشة `AcceptButton`) **تخالف القياس الحالي** (65 · 66 ملفاً) ⇒ تُصحَّح عند أول مساس، ولا تُنسخ في أي تقرير قبل ذلك (`RULE-N196`).
3. المرحلة 2 موجة شاملة ⇒ يجب فتح `MANIFEST.md` في هذا المجلد قبل أول دفعة.
