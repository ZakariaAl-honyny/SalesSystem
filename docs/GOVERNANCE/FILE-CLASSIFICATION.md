# تصنيف ملفات المشروع (File Classification)

> كل ملف وثائقي ينتمي إلى **صنف واحد**. الصنف يحدد: من يكتبه · متى يُحدَّث · هل يُحذف.

| الصنف | المعنى | يكتبه | متى يُحدَّث | يُحذف؟ |
|---|---|---|---|---|
| **1. قانون** | قواعد ملزمة؛ تغييرها قرار مالك مؤرّخ | Orchestrator | عند تغيير قاعدة فقط | ❌ |
| **2. سجل مهمة** | ذاكرة مهمة واحدة: نطاق · ملفات · قرارات · أدلة · مراجعة | Orchestrator (كاتب واحد) | عند كل مرحلة | ❌ يُغلق فقط |
| **3. سجل حالة** | صورة **مقيسة** للحاضر | Orchestrator / وكيل قراءة فقط | نفس الموجة التي تغيّر الحالة | ❌ |
| **4. مرجع فني** | حقائق تُقرأ ولا تُخترع | المختص | عند تغيّر المرجع | ❌ |
| **5. أرشيف** | تاريخ لا يُقرأ يومياً | لا أحد | لا | ✅ يُنقل إلى `archive/` |
| **6. سجل وكلاء** | تعريف الأدوار والصلاحيات | Orchestrator | عند تغيير دور/صلاحية | ❌ |

## توزيع الملفات الحالي (مقيس 2026-11-16)

| الملف/المجلد | الصنف |
|---|---|
| `AGENTS.md` · `docs/CONSTITUTION.md` · `docs/GOVERNANCE/**` (5 ملفات) | قانون |
| `docs/TASKS/TASK-NNN-*.md` + `INDEX.md` | سجل مهمة |
| `docs/MOCK-COVERAGE.md` · `docs/SHARED-HOME-REGISTER.md` · `docs/FORMAT-COVERAGE.md` · `docs/FORMAT-PROGRESS.md` · `docs/PRODUCTION-READINESS.md` · `docs/ui-screens.md` · `docs/CHANGELOG.md` | سجل حالة |
| `docs/database-schema.md` · `docs/BUSINESS-RULES.md` · `docs/PRD-MVP.md` · `docs/prompts/ORCHESTRATOR.md` · `docs/prompts/README.md` · `docs/prompts/P-*.md` | مرجع فني |
| `docs/archive/**` (28 ملفاً) · `docs/all new Anylysis for update system features/**` · `docs/Screenshot (480).png` | أرشيف |
| `.opencode/agents/**` (11 ملفاً) · `opencode.json` | سجل وكلاء |

## ملاحظتان

1. **`docs/all new Anylysis for update system features/salessystemanlysis_app/`** نموذج أولي **Next.js/TypeScript** يحاكي الشاشات بصرياً: أرشيف/مرجع بصري، **ليس جزءاً من الحل** ولا يُبنى، وممنوع نقل «كود جاهز» منه إلى WinForms.
2. **قاعدة النقل:** أي ملف يُنقل مكانه يُبحث عن كل مرجع إليه أولاً (`Select-String`)، وإلا كُسرت إحالة في `AGENTS.md` أو في سجل الوكيل.
