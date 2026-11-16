# فهرس ذاكرة المهام (`docs/TASKS/ARCHIVE/INDEX.md`)

> **قرار المالك (2026-11-16):** «الأرشيف فيه ذاكرة — نفهرسه بالاسم، فإذا احتجنا في مهمة حالية أن نعدّل أو نضيف، الوكيل يقرأ **الملف المطابق** بدل أن يقرأ كود النظام كله».
> **القاعدة الحاكمة:** **الفهرس ← السجل المطابق ← الكود (لا العكس).** هذا الملف **مؤشرات فقط**: صف + مسار + «ماذا تأخذ منه». **ممنوع نسخ محتوى أي سجل هنا** (`RULE-N169`/`RULE-N174`) — الأرقام والأدلة تبقى في بيتها.

## 1. كيف يُقرأ هذا الفهرس (ثلاث خطوات — قبل أي مسح للكود)

| # | الخطوة | الأمر/الفعل |
|---|---|---|
| 1 | **ابحث باسم الملف أو المنطقة في هذا الفهرس** | `Select-String -Path docs\TASKS\ARCHIVE\INDEX.md -Pattern "frmCashBoxMaster"` (أو اسم مجلد/سلوك) |
| 2 | **صف مطابق ⇒ افتح السجل المشار إليه فقط** (وجولته الأخيرة) | السجل يحمل: القرار · الموافقات · جرد الملفات · الأدلة · العيوب المكتشفة (`PILOT-n`) · المخاطر |
| 3 | **لا صف مطابق ⇒ عندها فقط اقرأ الكود** | ونتيجة بحثك تُضاف صفاً هنا **عند إغلاق مهمتك** (§4) — فالمهمة التالية تجد مرجعاً |

**ممنوع:** مسح `Screens/**` أو `Helpers/**` كاملاً «لأن الملف غير معروف» **قبل** الخطوة 1 · ومسح الأرشيف كله بلا صف مطابق · ونسخ نصوص السجلات إلى هنا.

> **لماذا هذا موجود:** الأرشيف يحمل ما لا يعيده الكود أبداً — **لماذا** صار السلوك هكذا، **ما** عيب أُصلح ومتى، **من** وافق بنصه، **أي** ملف مُسَّ فعلاً. قراءة الكود تعطيك الحالة الراهنة بلا تاريخها.

## 2. الفهرس الأساسي (السجل ← ماذا يجيب)

> **حالة `مؤرشف` = نُقل بـ`git mv` إلى `docs/TASKS/ARCHIVE/`.** لا يوجد ملف تتبع مؤرشف بعد؛ الصفوف أدناه هي **الذاكرة القائمة اليوم** بنفس البروتوكول (تُقرأ لا تُحذف) — وأول صف مؤرشف يُضاف عند أول إغلاق.

| # | السجل | النوع/الحالة | النطاق | التاريخ | اقرأه عندما… |
|---|---|---|---|---|---|
| 1 | `docs/TASKS/TASK-001-governance-layer.md` | سجل مهمة · مغلق | `docs/GOVERNANCE/**` · `docs/TASKS/**` · `docs/FEATURES/**` | 2026-11-16 | تسأل «لماذا توجد هذه الملفات؟ ما بروتوكولها؟» أو تضيف معياراً/سجلاً جديداً |
| 2 | `docs/TASKS/TASK-002-palette-blue-pilot.md` | سجل مهمة · مغلق (الجيل الرابع) | `Helpers/ThemePalette.cs` · `Helpers/GreenThemeHelper.cs` · `Settings/Forms/frmChangeSystemColorsSettings.cs` · `Screens/Products/Forms/frmProductMaster*` | 2026-11-16 | تلمس لوناً أو مصمّماً أو شريط عنوان أو زراً — **تاريخ أجيال اللوحة الأربعة** وسبب حذف التدرّج وملكية الأزرار (`RULE-N189`/`N204`) |
| 3 | `docs/TASKS/TASK-003-standards-two-layers.md` | سجل مهمة · مغلق | `GOVERNANCE/GENERAL-STANDARDS.md` · `PROJECT-STANDARDS.md` · `STANDARDS.md` · `prompts/ROLE-PROMPTS.md` | 2026-11-16 | تضيف معياراً (أي طبقة؟) أو تعدّل نص دور (`RULE-N203`) |
| 4 | `docs/TASKS/TASK-004-standards-pack-wiring.md` | سجل مهمة · مغلق | `.opencode/agents/*.md` (11) · `AGENTS.md` §10/§2.41 | 2026-11-16 | تعدّل برومبت وكيل أو تحزمة المعايير — **قاعدة التطابق والبصمة** (13 سطراً × 11) |
| 5 | `docs/TASKS/TASK-005-archive-protocol.md` | سجل مهمة · مغلق | `docs/TASKS/ARCHIVE/**` · `RULE-N202` · `G-08` | 2026-11-16 | تُغلق مهمة كبيرة وتؤرشف ملفها (خطوات الإغلاق الخمس) |
| 6 | `docs/TASKS/TASK-006-archive-index.md` | سجل مهمة · مغلق | هذا الفهرس + بروتوكول «الفهرس قبل الكود» | 2026-11-16 | تعدّل هذا الفهرس أو تضيف صفاً |
| 7 | `docs/FEATURES/F-001-pilot-required-field/` (`README` · `BRIEF` · `PLAN` · `DECISIONS` · `LOG`) | ميزة · مشهودة ومغلقة | `Helpers/RequiredFieldHelper.cs` · `Helpers/KeyboardHelper.cs` · `Helpers/NavigationOptions.cs` · `Global Classes/clsInputFeedback.cs` · `Screens/**` (تعميم) | 2026-11-16 | تلمس التنقل بـ`Enter` أو تغذية الحقل المطلوب أو `errorProvider` — **موجة `PILOT-1…16`**: كل عيب وُجد وكيف أُصلح (`RULE-N180`…`N194`) |
| 8 | `docs/MangeSubaggentsAndTracProgress.md` | **ملف تتبع نشط** (ليس مؤرشفاً) | `Screens/**` — كل نموذج `frm*` | 2026-11-16 | قبل مسّ أي شاشة: حالة الموجة · من أُنجز ومن بقي · القياس الرسمي و**درس `git checkout -- Screens`** |
| 9 | `docs/MOCK-COVERAGE.md` | سجل دائم (لا يُؤرشف) | `Services/Mock/**` · فرع الـMock في `Program.cs` · `Services/Interfaces/**` | مستمر | تلمس عقد بيانات أو شاشة تجريبية أو باك-إند وحدة (بوابة 0c/0d) |
| 10 | `docs/SHARED-HOME-REGISTER.md` | سجل دائم (لا يُؤرشف) | `Helpers/**` · `Global Classes/**` · `Common/Helpers/**` | مستمر | تعدّل سلوكاً مشتركاً — حالة كل ملف مشترك ومستهلكوه |
| 11 | `docs/FORMAT-COVERAGE.md` · `docs/FORMAT-PROGRESS.md` | سجل دائم | `Global Classes/clsFormat.cs` · `Settings/Forms/frmFormatingSetting*` | مستمر | تلمس تنسيق رقم/تاريخ أو إعداداته |
| 12 | `docs/CHANGELOG.md` | سجل دائم | الحل كله | مستمر | تسأل «متى تغيّر هذا؟» بجملة واحدة قبل الغوص في `git log` |

## 3. الفهرس العكسي — بالملف/المنطقة (هذا هو «المراجعة بالاسم»)

> ابحث هنا **باسم الملف** أولاً. الصف يقول: أي سجل تعرفه، وماذا تأخذ منه **قبل** أن تفتح الكود.

| الملف / المنطقة | اقرأ | ماذا تأخذ منه |
|---|---|---|
| `Screens/**` — أي `frm*.cs` / `frm*.Designer.cs` | `docs/MangeSubaggentsAndTracProgress.md` → ثم `FEATURES/F-001-pilot-required-field/LOG.md` | حالة الشاشة في موجة التعميم · النمط المعتمد (`WireEnterAsTab(scope, primaryButton)` · `RequiredFieldHelper`) · العيوب المعروفة في هذا الصنف |
| `Screens/Products/Forms/frmProductMaster.cs` (+`Designer`) | `FEATURES/F-001` (الكل) + `TASK-002` | **الشاشة التجريبية المعتمدة** — كل نمط معتمد أُثبت فيها أولاً؛ لا تُعاد تصميمها، وطابِقها |
| `Helpers/KeyboardHelper.cs` | `FEATURES/F-001/LOG.md` (`PILOT-10`…`PILOT-16`) | `IsTextInputFocused` · `WireEnterAsTab(scope, primaryButton)` · إعلان `Enter` في `PreviewKeyDown` · `NavigationOptions` (`RULE-N191`…`N194`) |
| `Helpers/RequiredFieldHelper.cs` · `Global Classes/clsInputFeedback.cs` | `FEATURES/F-001` (`BRIEF` + `DECISIONS` + `LOG`) | ترتيب `Attach` بعد `WireFocusHighlight` · `BlocksEnterNavigation` · ومضة الرفض (لا لون محلي) |
| `Helpers/ThemePalette.cs` · `Helpers/GreenThemeHelper.cs` · `Helpers/ThemeHelper.cs` | `TASK-002` | المفاتيح الحيّة · `RetiredDefaults` · `ButtonKeyFor` · **قاعدة تغيير اللوحة من ملف واحد** (`RULE-N188`…`N190`) |
| `Screens/Settings/Forms/frmChangeSystemColorsSettings.cs` (+`Designer`) | `TASK-002` | بنية المجموعات · التحقق/التباين · «استعادة الافتراضيات» · مصيدة الوضع الحقيقي |
| أي `*.Designer.cs` (95 مصمّماً) | `AGENTS.md` §2.19/§2.35 + `TASK-002` | صيغة الألوان الملزمة `Convert.ToInt32` (`RULE-N204`) · «المصمّم = التشغيل» (`RULE-N134`) |
| `.opencode/agents/*.md` · `AGENTS.md` §10/§2.41 | `TASK-004` + `TASK-003` | لماذا كل برومبت يحمل `Standards pack` · التطابق والبصمة · أي طبقة تُكتب فيها المعايير |
| `docs/GOVERNANCE/**` | `TASK-001` · `TASK-003` · `TASK-005` | بروتوكول كل ملف (أدلة · تجميد هيكل · موجة شاملة · تتبع) |
| `docs/TASKS/ARCHIVE/**` · هذا الفهرس | `TASK-005` · `TASK-006` | خطوات الإغلاق الخمس · بروتوكول «الفهرس قبل الكود» |
| `SalesSystem/Services/Mock/**` · فرع الـMock في `Program.cs` · `Services/Interfaces/**` | `docs/MOCK-COVERAGE.md` | حكم كل وحدة + تكافؤ الرفض + الفجوات ومالك كل ملف (بوابة 0c) |
| `SalesSystem/Business/**` · `DataAccess/**` · `Models/**` · `Services/Direct/**` | `docs/MOCK-COVERAGE.md` صف الوحدة | هل الباك-إند مفتوح لهذه الوحدة أم مجمّد (`RULE-N177`/`RULE-N178`) — **قبل** أي سطر |
| `Global Classes/clsDialogs.cs` · `clsFormat.cs` · `clsGlobal.cs` | `AGENTS.md` §2.30 + `FORMAT-COVERAGE.md` | البيت الواحد لكل حوار/تنسيق/فتح نموذج (`RULE-N169`) |
| `Global Classes/clsFormat.cs` · `Settings/Forms/frmFormatingSetting*` | `docs/FORMAT-COVERAGE.md` · `FORMAT-PROGRESS.md` | الإعدادات العشرة · `Definitions`/`ValidateAll`/`TryApplyAll` |
| `Screens/Reports/Forms/**` (31 تقريراً) | `AGENTS.md` §2.24 (`RULE-N121`) | استثناء التقارير من لوحة المفاتيح و`gbFilter` · `ReportUiStandardizer` |

## 4. كيف يُضاف صف (بروتوكول — جزء من خطوات الإغلاق)

| # | متى | ماذا تضيف |
|---|---|---|
| 1 | **عند إغلاق أي مهمة/ميزة** (ولو صغيرة) | صف في **§2**: السجل · النوع/الحالة · النطاق · التاريخ · «اقرأه عندما…» |
| 2 | **لكل ملف/منطقة مُسَّت فعلاً** | صف في **§3** بالاسم + «ماذا تأخذ منه» — بلا نسخ أرقام |
| 3 | **عند أرشفة ملف تتبع** | `git mv` ثم حدّث عمود الحالة إلى `مؤرشف` وأضف المسار الجديد (لا صف جديد للنسخة القديمة) |
| 4 | **عند تعديل سلوك موجود** | لا صف جديد — **حدّث** صف الملف القائم ليشير إلى الجولة الأحدث |

**ممنوع:** حذف صف (الصف يبقى ويُحدَّث) · صف بلا مسار · نسخ محتوى السجل في الصف · فهرس لا يطابق واقع المجلدات.

## 5. العلاقة بالملفات الأخرى

| الملف | علاقته |
|---|---|
| `docs/TASKS/ARCHIVE/README.md` | بروتوكول الأرشفة نفسه (خمس خطوات) — هذا الفهرس هو **خطوته السادسة** |
| `docs/TASKS/INDEX.md` | جدول المهام النشطة + جدول «مؤرشفة» — الفهرس هنا يغطي **القراءة الانتقائية** |
| `docs/GOVERNANCE/BIG-TASK-TRACKING.md` | القاعدة السادسة: «الأرشيف مفهرس» |
| `docs/GOVERNANCE/GENERAL-STANDARDS.md` `G-08` | القانون العام (قابل للنسخ لأي مشروع) |
| `AGENTS.md` `RULE-N202` · §10 صف `1d` | القانون الملزم في هذا المشروع |
