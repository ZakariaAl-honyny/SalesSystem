# F-003 — LOG

## الجولة 1 — 2026-11-17 — العنصر + النافذة + التسجيل

**نصّ المالك:** «I want to Make ctrlTreeOfAccounts for frmAccounts for Helpe account as tree and CRUD on any account find the for show account as tree and select any account easly».

| البند | القياس |
|---|---|
| ملفات جديدة | **6**: `Screens/Accounting/Controls/ctrlTreeOfAccounts.{cs,Designer.cs,resx}` (760 · 469 · 75 سطراً) + `Screens/Accounting/Forms/frmAccountsTree.{cs,Designer.cs,resx}` (99 · 91 · 69) |
| العقد المستهلك | `IAccountDesktopService.GetTreeAsync` → `AccountTreeNodeDto` (`Children` هرمية) · `GetByIdAsync` (لحكم `IsSystem`) · `DeleteAsync` · `PermanentDeleteAsync` |
| البحث | «الكود»/«الاسم» + تصفية فورية (250 ms) مع **إبقاء الأسلاف** وتوسيع النتيجة · بلا مطابقة ⇒ عقدة واحدة «لا توجد حسابات مطابقة» |
| الحذف النهائي | معطّل لحساب نظامي · معطّل لحساب له أبناء (من الشجرة الكاملة لا المقلَّمة) · **فشل مغلق** إن تعذّر التحقق من بيانات الحساب · تأكيد أقوى افتراضه «لا» |
| آلة الحالة | `_ApplyActionState()` واحدة تُستدعى بعد كل تغيير (`RULE-N150` نظير) · القائمة والأزرار تنادي نفس المعالجات |
| `RULE-N205` | **4** حدود فقط: `RefreshAsync` · `_EnsureAccountInfoAsync` · معالجا الحذف — و`finally` لإعادة المؤشر والحالة |
| الالتزامات | 70 + 13 `Convert.ToInt32` في المصمّمين · **0** صيغة مختصرة · **0** `MessageBox` · **0** `ex.Message` · `GreenThemeHelper.Apply(this)` آخر سطر · WFO1000 على الخصائص العامة |
| التسجيل | `Program.cs`: `AddTransient<frmAccountsTree>()` + `AddTransient<…ctrlTreeOfAccounts>()` (`RULE-N67`) · `frmMain`: بند «شجرة دليل الحسابات» بعد «دليل الحسابات» |
| البناء | **Build succeeded · 0 Error(s)** |

### قيود نطاق مُعلنة (لم تُنفَّذ — ولم تُخفَ)

1. **الدمج داخل `frmAccounts` مؤجَّل**: الشاشة ومصمّمها مملوكان لـ`TASK-008` المفتوحة (`docs/TASKS/ACTIVE.md` صف 3) — ملف واحد = مالك واحد لكل موجة (`RULE-N158`). يُنفَّذ بعد إقفالها.
2. **استهداف حساب بالمعرّف عند الفتح**: لا تُعرّض `frmAccounts` أي `SelectById`/`LoadAccount(id)`؛ الحدثان `AddChildRequested`/`EditRequested` يفتحان الشاشة فقط. **بند مفتوح** يُغلق مع اندماج الشجرة.
3. `Program.cs` + `frmMain*` **لم تُدرَج في كومِت هذه الشريحة** لأنها تحمل تغييرات مهام أخرى مفتوحة في الشجرة نفسها — تعمل في شجرة العمل وتُكمَل عند تنظيفها.

### بنود اختبار (15) — ملخّص

تحميل الشجرة · عقد مجموعة عريضة/ورقية عادية · توسيع/طيّ · بحث بالكود وبالاسم (مع الأسلاف) · لا مطابقة ⇒ عقدة باهتة · اختيار حساب يُفعّل «تعديل/حذف/إضافة فرعي» · رفض الحذف النهائي (نظامي · له أبناء) · إلغاء التأكيد لا يغيّر شيئاً · حذف ناجح يُحدّث الشجرة · «إضافة/تعديل» يفتحان `frmAccounts` بلا إغلاق الشجرة · قائمة السياق بنفس الحالات · «تحديث» يستعيد الاختيار · المصمّم = التشغيل بلا رمادي.
