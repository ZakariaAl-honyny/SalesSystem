# TASK-005 — بروتوكول أرشفة ملفات تتبع المهام الكبيرة (أرشفة لا حذف)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-005` |
| `العنوان` | عند إغلاق مهمة كبيرة 100%: الملف يُغلق ويُؤرشف بـ`git mv` في `docs/TASKS/ARCHIVE/` — ولا يُحذف أبداً |
| `الحجم` | S (9 ملفات وثائق/برومبتات · صفر كود) |
| `المرحلة` | `وثائق` |
| `الحالة` | مغلقة |
| `الوكيل المالك` | `orchestrator` (`docs/**` · `AGENTS.md` · `.opencode/agents/**` ملكه — `RULE-N158`) |
| `أثر قاعدة البيانات` | **لا شيء** (`RULE-N198` غير مطروح) |
| `التاريخ` | `2026-11-16` |

## قرار المالك (نصّه)

> «and after any big task file we can remove them because everything is done 100%»

السؤال طُرح بخيارات ثلاثة، واختار المالك: **«أرشفة لا حذف (موصى به)»** — يخرج الملف من الطريق النشط، وتبقى ذاكرته.

## لماذا لا حذف (السبب المقيس)

ملف التتبع هو المكان الوحيد الذي يحمل: **الموافقات بنصها وتاريخها** (`RULE-N166` للتغيير السلوكي · `RULE-N198` لهيكل القاعدة) · **جرد كل ملف وحالته** · **الأدلة بالأمر والمخرج** (`RULE-N196`) · **سجل `PILOT-n`** (`RULE-N186`) · **جولات §9**. حذفه = عودة إلى «البدء من الصفر» الذي بُنيت له طبقة الحوكمة كلها (`G-07`/`G-08`).

## الملفات (قائمة مغلقة)

| # | الملف | التغيير |
|---|---|---|
| 1 | `docs/TASKS/ARCHIVE/README.md` | **جديد** — البروتوكول: خطوات الإغلاق الخمس · ما يبقى · الممنوع · كيف يُبحث عن ملف مؤرشف |
| 2 | `AGENTS.md` `RULE-N202` | «ولا يُحذف بعد الإغلاق» ⇒ «يُغلق ثم يُؤرشف في `docs/TASKS/ARCHIVE/` بـ`git mv` ولا يُحذف أبداً» |
| 3 | `docs/GOVERNANCE/GENERAL-STANDARDS.md` `G-08` | نفس القاعدة في الطبقة العامة (مجلد أرشيف المشروع) + عمود فحص + مخالفة |
| 4 | `docs/GOVERNANCE/BIG-TASK-TRACKING.md` | القاعدة 5 (الأرشفة) + قسم «الإغلاق والأرشفة» بالترتيب الملزم |
| 5 | `docs/TASKS/INDEX.md` | تنبيه الأرشفة + جدول «مؤرشفة (`docs/TASKS/ARCHIVE/`)» بأربعة أعمدة |
| 6 | `docs/TASKS/README.md` | `ARCHIVE/` في مخطط التسمية |
| 7 | `docs/TASKS/_BIG-TASK-TRACKER-TEMPLATE.md` | حقل `ملف التتبع` يشير إلى الأرشفة |
| 8 | `docs/prompts/ORCHESTRATOR.md` | بوابة 0e (سطر ملف التتبع) + سيناريو 33 |
| 9 | `docs/prompts/P-005-big-task-kickoff.md` | خطوة الإغلاق 8 صارت «أرشِف» لا «ابقِ» |
| 10 | `.opencode/agents/orchestrator.md` | `1d-bis` + قالب الـBrief (حقل `Task record`) |
| 11 | `docs/TASKS/TASK-005-archive-protocol.md` · `INDEX.md` | هذا السجل + صفه |

## خارج النطاق (ممنوع بالاسم)

- **حذف أو نقل أي ملف تتبع قائم** (`docs/TASKS/TASK-00*.md` · `docs/FEATURES/**`) — القاعدة تحكم **الإغلاق القادم** فقط، ولا تُؤرشف مهمة غير مغلقة.
- أي ملف كود `SalesSystem/**` · `docs/MOCK-COVERAGE.md` · `docs/SHARED-HOME-REGISTER.md` · `docs/TASKS/ACTIVE.md` (لا مهمة مفتوحة).

## معيار الاكتمال (تم التحقق منه)

1. `docs/TASKS/ARCHIVE/README.md` موجود ويحمل **5 خطوات إغلاق** + جدول «ما يبقى» + قسم ممنوع.
2. صفر نص «لا يُحذف بعد الإغلاق» في الملفات الحاكمة (`AGENTS.md` · `GENERAL-STANDARDS.md` · `BIG-TASK-TRACKING.md` · `INDEX.md` · القالب · `ORCHESTRATOR.md` · البرومبت) — كله صار «يُغلق ثم يُؤرشف».
3. **صفر حذف وصفر نقل**: `git status` يُظهر تعديلات وإضافة مجلد أرشيف فقط.
4. `docs/TASKS/INDEX.md` يحمل جدول «مؤرشفة» بجانب جدول «المفتوحة».

## الأدلة

```powershell
Select-String -Path AGENTS.md,docs\GOVERNANCE\*.md,docs\TASKS\INDEX.md,docs\TASKS\_BIG-TASK-TRACKER-TEMPLATE.md,docs\prompts\ORCHESTRATOR.md,docs\prompts\P-005-big-task-kickoff.md -Pattern "لا يُحذف بعد الإغلاق"   # المتوقع: 0
Select-String -Path AGENTS.md,docs\GOVERNANCE\*.md,docs\TASKS\*.md,docs\prompts\*.md,.opencode\agents\orchestrator.md -Pattern "ARCHIVE"             # المتوقع: ≥ 8
Test-Path docs\TASKS\ARCHIVE\README.md                                                                                                              # المتوقع: True
```

## الجولة

| الوقت | الفعل |
|---|---|
| 2026-11-16 | قرار المالك (أرشفة لا حذف) → إنشاء `ARCHIVE/README.md` → تعديل القانون في الطبقتين (`RULE-N202` · `G-08`) → البروتوكول `BIG-TASK-TRACKING.md` → المؤشرات (INDEX · README · القالب) → البرومبتات (`ORCHESTRATOR.md` · `P-005` · `orchestrator.md`) → القياس → الكومِت |
