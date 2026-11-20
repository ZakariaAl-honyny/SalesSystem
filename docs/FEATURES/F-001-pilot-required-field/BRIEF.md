# F-001 — النطاق (BRIEF)

> قائمة **مغلقة**. تُحدَّث فقط بقرار في `DECISIONS.md`.

## الهدف

سلوك واحد في النظام: «الحقل المطلوب الفارغ يُبلَّغ عنه فوراً ولا يُغادَر بـ`Enter`، و`Enter` يمشي بترتيب العرض ثم يسلّم لزر الوضع».

## داخل النطاق — الجولة الحالية (الجولة 1)

| الملف | العمل |
|---|---|
| `Helpers/RequiredFieldHelper.cs` | البيت الواحد: التقييم + `FirstMissing` + `BlocksEnterNavigation` |
| `Helpers/KeyboardHelper.cs` | `WireEnterAsTab(scope, primaryButton)` + `PreviewKeyDown` + استهلاك حجب `Enter` |
| `Helpers/NavigationOptions.cs` | قيم الترتيب والتسليم الافتراضية (تُقرأ حيّة) |
| `Global Classes/clsInputFeedback.cs` | الومضة + الرسالة + الصوت + الاستعادة |
| `Screens/Products/Forms/frmCRUDCProductMaster.cs` + `.Designer.cs` | الشاشة التجريبية: تُعلن قواعدها وتستهلك البيوت |

## خارج النطاق (ممنوع بالاسم — الآن)

- **كل شاشة أخرى** (28 ملفاً تحمل `SetError`) — التعميم مؤجل للشهادة (`RULE-N184`).
- `Business/**` · `DataAccess/**` · `Models/**` · `Services/**` · `Migrations/**` · `Seeders/**` — لا أثر لقاعدة البيانات.
- `Program.cs` · `*.csproj` · `appsettings.json`.
- `docs/**` إلا بـOrchestrator.

## العقود

لا عقد بيانات. العقد الوحيد **داخلي**:
`RequiredFieldHelper.Attach(Form owner, ErrorProvider provider, params RequiredField[] fields)` · `bool BlocksEnterNavigation(Control field)` · `KeyboardHelper.WireEnterAsTab(Control scope, Func<Button?> primaryButton)`.

## أثر قاعدة البيانات

- [x] **لا شيء** — لا جدول ولا عمود ولا ترحيل (`docs/GOVERNANCE/SCHEMA-FREEZE.md` غير مطلوب).

## معيار الاكتمال (للشهادة)

- [ ] `Enter` على حقل مطلوب فارغ: المؤشر **يبقى** عليه + ✗ + الرسالة + الومضة، بلا رنّة.
- [ ] `Tab` والماوس ينتقلان عادياً.
- [ ] `Enter` يمشي بترتيب العرض ثم يركّز زر الوضع **بلا تنفيذ**.
- [ ] الحقل المعطّل لا يُوسَم أبداً.
- [ ] `dotnet build` = **0 أخطاء** (أمر + مخرج في `LOG.md`).
