# سجل أوامر الذكاء الاصطناعي (Prompt Registry)

> **كل Prompt في المشروع مسجَّل هنا.** Prompt غير مسجَّل = غير مسموح باستخدامه في تسليم.
> القاعدة: **Prompt واحد لكل مهمة** — لا «Prompt عام» يفعل كل شيء.

## 1. أوامر النظام (System prompts — ملفات الوكلاء)

| ID | الملف | الدور | الوضع | صلاحية الكتابة |
|---|---|---|---|---|
| `S-01` | `.opencode/agents/orchestrator.md` | التقسيم والتفويض والبوابات | فعّال | `AGENTS.md` · `docs/**` · سجل الوكلاء فقط |
| `S-02` | `.opencode/agents/ui-agent.md` | شاشات WinForms + Mock | فعّال | `Screens/**` · `Services/Mock/**` |
| `S-03` | `.opencode/agents/backend-architect.md` | BLL · DTO · `Direct*` | فعّال | `Business/**` · `DTO/**` · `Services/**` |
| `S-04` | `.opencode/agents/database-engineer.md` | كيانات · ترحيلات | فعّال | `Models/**` · `DataAccess/**` |
| `S-05` | `.opencode/agents/implement-agent.md` | تنفيذ محلي / دفعة S | فعّال | الملفات المسمّاة |
| `S-06` | `.opencode/agents/fast-agent.md` | إصلاح جراحي | فعّال | ملف أو ملفان |
| `S-07` | `.opencode/agents/printing.md` | طباعة/PDF | فعّال | `Printing/**` |
| `S-08` | `.opencode/agents/code-reviewer.md` | مراجعة | فعّال | لا شيء |
| `S-09` | `.opencode/agents/security-auditor.md` | أمن | فعّال | لا شيء |
| `S-10` | `.opencode/agents/test-engineer.md` | قائمة اختبار يدوي | فعّال | لا شيء |
| `S-11` | `.opencode/agents/mock-qa.md` | شهادة تغطية Mock | فعّال | لا شيء |

## 2. أوامر التشغيل (Playbooks)

| ID | الملف | الغرض |
|---|---|---|
| `O-01` | `docs/prompts/ORCHESTRATOR.md` | قانون التقسيم + قوالب الـBrief + البوابات + خريطة السيناريوهات |
| `O-02` | `docs/prompts/UI-ROLLOUT-PROMPT.md` | موجة تعميم واجهة |
| `O-03` | `docs/prompts/ROLE-PROMPTS.md` | **نصوص أدوار العمل** — 11 دوراً + القالب العشري + القواعد الخمس (طبقة عامة قابلة للنقل؛ النسخة المرتبطة بهذا المشروع `S-01`…`S-11`) — `RULE-N203` |

## 3. أوامر جاهزة للنسخ (Prompts — ملف واحد لكل مهمة)

| ID | الملف | متى يُستخدم | يُلصق في |
|---|---|---|---|
| `P-001` | `docs/prompts/P-001-schema-change-refusal.md` | وكيل/مطوّر يطلب تعديل جدول أو عمود | Brief أي وكيل باك-إند |
| `P-002` | `docs/prompts/P-002-new-feature-dossier.md` | ميزة جديدة أو مهمة كبيرة (L) | بداية أي شريحة L |
| `P-003` | `docs/prompts/P-003-system-sweep.md` | تعديل شامل على النظام كله | أي موجة > 8 ملفات |
| `P-004` | `docs/prompts/P-004-evidence-report.md` | كل تسليم من أي وكيل | ذيل كل Brief |
| `P-005` | `docs/prompts/P-005-big-task-kickoff.md` | بدء **أي مهمة كبيرة** على مستوى النظام | أول رسالة في الشات |

## 4. قواعد استخدام الأوامر

1. **لا تُعدَّل نسخة Prompt داخل رسالة شات** — إن احتجت تغييراً، عدّل الملف وأضف سطراً في سجل تحديثه.
2. **كل Prompt يشير إلى قانون** (`AGENTS.md` §رقم) — ولا ينسخ نصه.
3. **Prompt بلا «خارج النطاق» مرفوض** — حتى لو كان إصلاحاً صغيراً.
4. **`P-004` إلزامي في كل Brief** — بدونه لا يُقبل تقرير.
5. **نص الدور لا يُخترع في الشات (`RULE-N203`)** — يُستهلك من `ROLE-PROMPTS.md` (§3) أو من `.opencode/agents/*.md`، ويُضاف إليه الـBrief (12 حقلاً من `ORCHESTRATOR.md` §6) وذيل `P-004`. ثلاث قطع معاً = وكيل لا يخترع.

## سجل التحديث

| التاريخ | التغيير |
|---|---|
| 2026-11-16 | إنشاء السجل (v1.0) — 11 أمر نظام + 2 playbook + 4 أوامر جاهزة، وربطها بـ`AGENTS.md` §2.41/§2.42. |
