# F-001 — الخطة (PLAN)

| # | المرحلة | النطاق | البوابة (لا تُغلق إلا بها) | الحالة |
|---|---|---|---|---|
| 1 | **تجريبية** | `frmProductMaster` وحدها + البيوت الخمسة | شهادة المالك **100%** (`RULE-N185`) + بناء 0 أخطاء | 🔄 بانتظار الشهادة |
| 2 | **تعميم** | 28 ملفاً تحمل `errorProvider.SetError` — دفعات ≤ 4 شاشات | جرد `MANIFEST.md` + مطابقة أعداد + فحص بقايا = 0 | ⬜ |
| 3 | **إغلاق** | مراجعة واحدة + توثيق + كومِت | `code-reviewer` 0 Blocker · `local == origin` | ⬜ |

## الموجات

| الموجة | الوكيل | الملفات | الدليل |
|---|---|---|---|
| W1 | `ui-agent` | `Helpers/RequiredFieldHelper.cs` · `Helpers/KeyboardHelper.cs` · `Helpers/NavigationOptions.cs` · `Global Classes/clsInputFeedback.cs` · `Screens/Products/Forms/frmProductMaster.cs` | ✔ جولة 1 |
| W2 | `ui-agent` ×≤4 | 4 شاشات لكل وكيل من قائمة الجرد | ⬜ |
| W3… | … | … | ⬜ |

## ملاحظات التخطيط

- **لماذا `frmProductMaster`؟** قرار المالك 2026-11-16: «قبل أن نطبّق أي كود على كل شاشات النظام يجب أن نختبره في مكان واحد» (`AGENTS.md` §2.39).
- **المرحلة 2 موجة شاملة ⇒ `MANIFEST.md` إلزامي** (`docs/GOVERNANCE/SYSTEM-SWEEP.md`)، والعدد الحالي للجرد **28 ملفاً** (مقيس — انظر `LOG.md`).
- **لا تعميم قبل الشهادة** (`RULE-N184`) — حتى لو بدا التعميم «نسخاً بسيطاً».
- **`WireEnterAsTab(scope, primaryButton)`** يُعمَّم في نفس الموجة لأنه شرط حجب `Enter`، لكن بحدود 4 شاشات لكل وكيل.
