# F-002 — BRIEF (قالب الموجة الواحدة = وحدة واحدة)

> يُنسخ هذا القالب لكل وكيل مع تعبئة `<Module>` / `<Entity>` / قائمة الحقول المقيسة.
> الوكيل **يقرأ** `README.md` + `PLAN.md` + آخر جولة في `LOG.md` قبل أول سطر، **ولا يكتب** في `docs/**` (`RULE-N197`).

```
Goal          — وحدة <Module> فقط: بطاقة عرض + بطاقة بتصفية + نموذج عرض + نموذج بحث + زر «بطاقة» في شاشة الدليل.
Stage         — — (واجهة فقط). لا باك-إند ولا هيكل قاعدة بيانات ولا ترحيل.
In scope      — (قائمة مغلقة)
                Screens/<Module>/Controls/ctrl<Entity>Card.cs
                Screens/<Module>/Controls/ctrl<Entity>Card.Designer.cs
                Screens/<Module>/Controls/ctrl<Entity>Card.resx
                Screens/<Module>/Controls/ctrl<Entity>CardWithFilter.cs
                Screens/<Module>/Controls/ctrl<Entity>CardWithFilter.Designer.cs
                Screens/<Module>/Controls/ctrl<Entity>CardWithFilter.resx
                Screens/<Module>/Forms/frmShow<Entity>Card.cs (+ .Designer.cs + .resx)
                Screens/<Module>/Forms/frmFind<Entity>.cs (+ .Designer.cs + .resx)
                Screens/<Module>/Forms/frm<Entity>Master.cs (+ .Designer.cs)  ← إضافة فقط
Out of scope  — Program.cs · أي Helpers/** · Global Classes/** · Business/** · DataAccess/** · Models/** ·
                DTO/** · Services/** · أي شاشة في وحدة أخرى · أي ملف في docs/**.
Contract      — I<Entity>DesktopService.GetByIdAsync(int, CancellationToken) — إن لم يوجد: GetAllAsync وتصفية محلية.
Task record   — docs/FEATURES/F-002-entity-cards/README.md + PLAN.md + آخر جولة في LOG.md.
Schema impact — none.
Sweep         — لا ينطبق (ملفات الوحدة ≤ 14 · قائمة مغلقة).
Single home   — كل سلوك مشترك يُستهلك من بيته: GreenThemeHelper · KeyboardHelper · clsDialogs · clsFormat ·
                RequiredFieldHelper · clsGlobal._OpenForm. ممنوع كتابة لون/حوار/تنسيق داخل الشاشة.
Done when     — البطاقة تُعرض من شاشة الدليل بالزر، والحقول العربية مطابقة للجدول في PLAN.md،
                و`Ctrl` بلا أي نداء خدمة، و`CtrlWithFilter` يبحث بالمعرّف وبالاسم/الباركود ويُطلق الحدث.
Mock parity   — لا حفظ: لا رفض مطلوب. رفض القراءة: IsSuccess=false ⇒ رسالة عربية عبر clsDialogs + تصفير البطاقة.
Mock coverage — يُقرأ من docs/MOCK-COVERAGE.md — لا تُعدّل أي صف؛ تُقرأ فقط.
Validation    — لا تبني، لا تُشغّل، لا اختبارات. الـOrchestrator يبني بعد الموجة (RULE-N117/RULE-N159).
Report back   — الملفات · القرارات · المخاطر · جدول اختبار يدوي عربي
                + السطر الحرفي: لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31)
```

## قواعد إلزامية في كل ملف واجهة (تُنسخ في كل brief)

1. `RightToLeft = Yes` + `RightToLeftLayout = true` + خط `Tahoma` (RULE-N20/N21).
2. `KeyboardHelper`: `WireEnterAsTab` + `WireSelectAllOnFocus` + `WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor))` + `WireArrowNavigation` + `WireButtonDisabledVisual` + `WireGlobalShortcuts` (RULE-N40…N47/N95).
3. `GreenThemeHelper.Apply(this)` **آخر سطر** في التهيئة (RULE-N50)، و`ApplyHeader` لأي تسمية عنوان.
4. كل لون في `*.Designer.cs`: `System.Drawing.Color.FromArgb(Convert.ToInt32(n), …)` (RULE-N204) بقيمة `ThemePalette` الحرفية.
5. `MessageBox` ممنوع: `clsDialogs.*` فقط (RULE-N169). `ex.Message` للمستخدم ممنوع: Serilog + رسالة عربية عامة.
6. `async void` + `try/catch` + `Cursor.WaitCursor` واستعادته في `finally` + `CancellationToken` (RULE-N25/N26).
7. ألوان الحالة (`StatusSuccess`/`StatusDanger`) تُطبَّق **وقت التشغيل فقط** ولا تُعلن في المصمّم (لون حالة — استثناء RULE-N154/N182).
8. المال `decimal` ويُعرض بـ `clsFormat.Money`، الكمية `clsFormat.Quantity`، التاريخ `clsFormat.DateToShort/DateToLong`.
9. لا `new frmXxx()`: النماذج تُحلّ من DI، والفتح عبر `clsGlobal._OpenForm(form)` (RULE-N02/RULE-N132).
10. عناصر التحكم لها مُنشئ بلا وسائط (المصمّم ينشئها) — الخدمة تُمرَّر عبر `Attach(IServiceProvider)` لا عبر المُنشئ.
