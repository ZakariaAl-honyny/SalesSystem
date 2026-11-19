# F-005 — سجل الجولات (LOG)

## الجولة 1 — 2026-11-19 — W1: ثمانية مصمّمات `frmCRUD*Master`

### الهدف
تطبيق شكل `frmCRUDCProductMaster` (المرجع المعتمد من المالك بنصّه) على بقية عائلة `frmCRUD*Master`، وإغلاق شكوى «الشاشة بيضاء والحقول غير واضحة».

### القياس قبل الموجة (أمر: قراءة 90 `*.Designer.cs`)

| القياس | الرقم |
|---|---|
| مصمّمات مكتشَفة | 90 |
| داخل النطاق / معفاة (نوافذ معاينة الطباعة) | 85 / 5 |
| خلفية نموذج رمادي الجيل الثاني `240,242,245` | 68 |
| حاويات معلنة `Color.White` | 742 من 893 |
| حقول إدخال بلا `Color.White` | 14 من 328 |
| ألوان بالصيغة المختصرة (`RULE-N204`) | 1206 |
| ماسترات `frmCRUD*` على الأخضر الحيّ قبل الموجة | 3 من 11 |

### الجولة (3 `ui-agent` + 1 `fast-agent`)

| الوكيل | الملفات | الناتج |
|---|---|---|
| `ui-agent` A | `frmCRUDCSupplierMaster` · `frmCRUDCustomerMaster` | خلفية `170,220,170` · 10/10 حاوية خضراء · 10/10 حقول بيضاء · 28 صيغة مختصرة ← 0 |
| `ui-agent` B | `frmCRUDCProductUnitMaster` · `frmCRUDCashBoxMaster` · `frmCRUDCategoryMaster` | نفس المعايير · 6/6 و9/9 حقول بيضاء |
| `ui-agent` C | `frmCRUDUnitMaster` · `frmCRUDUserMaster` · `frmCRUDWarehouseMaster` | نفس المعايير |
| `fast-agent` | 5 ملفات | توحيد تعارضين مكتشفين بين الوكلاء |

### التعارضات المكتشفة والإصلاح (قبل البناء الثاني)

1. **`ColumnHeadersDefaultCellStyle.SelectionBackColor`**: وكيل كتب `255,255,180` (بيج) وآخران كتبا `14,165,103` (أخضر الرأس). **الحسم بالأمر:** `GreenThemeHelper.ApplyGrid` يفرض `GridHeaderColor` على رأس العمود + `SelectionForeColor = White` ⇒ البيج كان يُنتج أبيض-على-بيج بتباين 1.1. صُحّح في `frmCRUDCSupplierMaster` و`frmCRUDCustomerMaster`، وصُحّح **البند 18 في `BRIEF.md`** (صار 18 + 18b) لأنه كان هو الخطأ الجذري.
2. **`btnToggleFilters.FlatStyle`**: 5 ملفات `Flat` و3 ملفات `Popup` (المرجع `Popup`). وُحِّد على **`Flat`** (`D-06`) وتُرك المرجع بلا مسّ (`D-08`) — فرق بصري بسيط يُحسم في الشهادة.

### البناء

| المرحلة | النتيجة |
|---|---|
| بعد الموجة الأولى (3 وكلاء) | **0 Errors / 95 Warning(s)** |
| بعد إصلاح التعارضين (`fast-agent`) | **0 Errors / 95 Warning(s)** |

التحذيرات كلها `CS0169` سابقة (حقول أعمدة معلنة وغير مستخدمة في مصمّمات قديمة) — لا تحذير جديد من الموجة.

### المراجعة (`code-reviewer` — قراءة فقط، مرة واحدة على الشريحة)

**Pass with notes — 0 Blocker · 0 Major · 3 Minor**

| # | البند | الحسم |
|---|---|---|
| 1 | `frmCRUDCProductUnitMaster:121` — `lblProduct.ForeColor = (30,86,160)` أزرق خارج اللوحة، **سابق للموجة** (مثبت في `HEAD`) | يبقى · يُسجَّل بنداً مفتوحاً (`O-1`) |
| 2 | `frmCRUDCustomerMaster:425` — `btnShowCard` رُسم بلون `btnInfo` بدل الاحتياطي | يُعتمد `D-10` |
| 3 | `frmCRUDCProductMaster` (المرجع) يخالف الجدول في `btnToggleFilters` (`Popup` + نص `14,82,52`) | قرار المالك في الشهادة (`O-2`) |

**ما تحقّق إيجاباً بالقياس:** المرجع لم يُمسّ (سطر واحد فقط: إعادة تسمية الصنف) · صفر تغيير في `Dock/Anchor/Size/Location/TabIndex/Text/Font/Name/ReadOnly/SelectionMode/AllowUserToAddRows/DataSource/الأحداث` · صفر صيغة مختصرة · صفر مرجع `Helpers/` داخل مصمّم · صفر لون حالة معلن · كل القيم من اللوحة الحيّة · كل حقول الإدخال بيضاء · كل شبكة مطابقة لما يفرضه `ApplyGrid`.

### العائق (لم يُغلق)

**لم يُنفَّذ أي التزام (`commit`).** شجرة العمل كانت — قبل هذه الموجة — تحمل **96** مدخلاً غير ملتزم من موجة سابقة (إعادة تسمية `frm*Master → frmCRUD*Master`: **21** حذفاً · **22** ملفاً جديداً · **53** تعديلاً). التزام ملفات F-005 وحدها يترك `HEAD` بحذف/إضافة نصف إعادة التسمية فينكسر البناء على `HEAD`. القرار للمالك: التزام الموجتين معاً كموجة واحدة، أو فصل إعادة التسمية أولاً في التزامها الخاص.

### التالي

**W2** — المستندات والقوائم (8 ملفات) — **معلّقة على شهادة المالك البصرية لـW1** (`RULE-N184`/§2.39)، وعلى قرار الالتزام أعلاه.

**قائمة تعارضات يجب أن يعرفها كل وكيل W2 (تُضاف إلى كل brief):**
1. `ColumnHeadersDefaultCellStyle.SelectionBackColor = 14,165,103` + `SelectionForeColor = White` — **لا بيج على الرأس**.
2. `DefaultCellStyle.BackColor = White` (الخلايا بيضاء) · `dgv.BackgroundColor = 220,240,220` (منطقة الشبكة) — لا تُخلط.
3. `btnToggleFilters.FlatStyle = FlatStyle.Flat` · `ForeColor = 30,41,59`.
4. الحاويات تُعلَن صراحةً (لا Ambient): `Panel/FlowLayoutPanel/TableLayoutPanel` = `235,240,235` · `GroupBox` = `235,245,235`.
