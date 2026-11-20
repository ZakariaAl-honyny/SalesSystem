# F-006 — تجميع كل جزء من شاشة الفواتير في صندوق مجموعة مُعنون (Invoice Screens Grouping)

| الحقل | القيمة |
|---|---|
| `ID` | `F-006` |
| `العنوان` | إحاطة كل جزء مرئي في شاشات الفواتير (قوائم + منتقيات) بصندوق `GroupBox` مُعنون على نمط `frmAddUpdateSalesInvoice` |
| `الحجم` | **L** — 5 شاشات · 15 ملفاً مُجرداً · 5 وكلاء · موجتان |
| `المرحلة` | `—` (واجهة فقط — ليست مسار بيانات جديد ⇒ لا بوابة Mock) |
| `الحالة` | **منفَّذة — بانتظار اختبار المالك** (2026-11-20) — W1 + W2 · بناء **0 Errors** · مراجعة أُغلقت (0 Blocker / 0 Major / 3 Minor) |
| `الوكيل المالك` | `ui-agent` (شاشة واحدة لكل وكيل) · البناء والمراجعة للـOrchestrator |
| `أثر قاعدة البيانات` | `لا شيء` (`RULE-N198`) |
| `الشاشة المرجعية` | `Screens/All Invoices Screens/Sales/frmAddUpdateSalesInvoice.cs` |

## الهدف

المالك (2026-11-20): «I want to grouping the screen like `frmAddUpdateSalesInvoice` — each and every part is inside group box for understand and these data and for what and for improve UI shape for end user».

أي: كل كتلة وظيفية في الشاشة تُحاط بصندوق مجموعة **بعنوان عربي قصير يقول للمستخدم ما هذه البيانات ولماذا** — كما في المرجع: عنوان ← «أوامر الفاتورة» ← «بيانات الفاتورة» ← «إضافة صنف» ← «الأصناف» ← «الإجماليات».

## النطاق المختار بقرار المالك (2026-11-20)

**المرحلة 1 فقط — شاشات الفواتير القائمة والمنتقيات الخمس** داخل `Screens/All Invoices Screens/`. بقية الوحدات **مرحلة 2 لاحقة** بعد شهادة الشكل (`RULE-N184`).

## القياس قبل التنفيذ (`RULE-N195`)

| ما قِيس | القيمة | الأمر |
|---|---|---|
| نماذج `frm*` في `Screens/**` | **85** مصمّماً | `Get-ChildItem -Recurse -Filter frm*.Designer.cs` |
| منها تقارير (`Screens/Reports/**`) | **26** | نفس الأمر على مجلد التقارير |
| نوافذ طباعة/معاينة (`*Print*`) | **5** | نفس الأمر + فرز بالاسم |
| شاشات مجلد الفواتير | **9** (2 محرّر + 5 قوائم/منتقيات + POS + معاينة طباعة) | نفس الأمر على مجلد الفواتير |
| المرجع `frmAddUpdateSalesInvoice` | **5** صناديق: `gbToolbar` «أوامر الفاتورة» · `gbInvoiceInfo` «بيانات الفاتورة» · `gbEntry` «إضافة صنف» · `gbItems` «الأصناف» · `gbTotals` «الإجماليات» | عدّ `new GroupBox()` في المصمّم |
| `frmAddUpdatePurchaseInvoice` | **4** صناديق + `gbEntry` داخل `ctrlLineEntryPanel` = مطابق للمرجع (`TASK-022`) | نفس العدّ |
| `frmPOS` | **4** صناديق (`gbSalesCommands` · `gbCustomerPayment` · `gbCart` · `gbTotals`) | نفس العدّ |
| **الشاشات الخمس في النطاق** | **صندوق واحد فقط لكل واحدة** — `gbFilter` «بحث وتصفية» — والباقي لوحات عارية (`pnlHeader` · `pnlToolbar` · `pnlSummary` · `pnlFooter` · `pnlGrid`/`tabReturns`) | نفس العدّ |
| مراجع code-behind إلى `pnl*` في الشاشات الخمس | **0** — لا معالج يقرأ `pnlHeader`/`pnlToolbar`/`pnlSummary`/`pnlFooter`/`pnlGrid` | `Select-String -Pattern "pnl(Header\|Toolbar\|Summary\|Footer\|Grid)"` |
| `FilterPanelHelper` | يعتمد **بالاسم** على `pnlFilters` + `gbFilter` ⇒ **ممنوع المساس بهما** | قراءة `Helpers/FilterPanelHelper.cs` |
| اللوحة الرسمية | صناديق المرجع: `BackColor = 170,220,170` · `ForeColor = 33,37,41` · `Font = Tahoma 10 Bold` | مصمّم المرجع |

## الشكل الهدف لكل شاشة

| # | الشاشة | الكتل المطلوبة (أعلى ⇒ أسفل) |
|---|---|---|
| 1 | `frmListSales` | `pnlHeader` (عنوان — يبقى) · **`gbToolbar` «أوامر الشاشة»** · `gbFilter` «بحث وتصفية» (قائم) · **`gbGrid` «فواتير البيع»** · **`gbSummary` «ملخص النتائج»** · **`gbFooter` «تلميحات الاستخدام»** |
| 2 | `frmListPurchases` | `pnlHeader` · **`gbToolbar` «أوامر الشاشة»** · `gbFilter` (قائم) · **`gbGrid` «فواتير الشراء»** · **`gbSummary` «ملخص النتائج»** (لا `pnlFooter`) |
| 3 | `frmListReturns` | `pnlHeader` · **`gbToolbar` «أوامر الشاشة»** · `gbFilter` (قائم) · **`gbGrid` «المرتجعات»** (يحتضن `tabReturns`) · **`gbSummary` «ملخص النتائج»** · **`gbFooter` «تلميحات الاستخدام»** |
| 4 | `frmSalesInvoiceSelection` | `pnlHeader` · **`gbToolbar` «أوامر الشاشة»** · `gbFilter` (قائم) · **`gbInvoices` «فواتير البيع»** (يحتضن `dgvInvoices`) · `lblInvoiceSummary` (شريط ملخص — يبقى عارياً كـ`lblQuickInfo` في المرجع) · **`gbLines` «سطور الفاتورة»** (يحتضن `dgvLines`) · `lblNoData` (يبقى) · **`gbSummary` «ملخص النتائج»** · **`gbFooter` «تلميحات الاستخدام»** |
| 5 | `frmPurchaseInvoiceSelection` | نفس بنية (4) بعنوان **«فواتير الشراء»** |

**قاعدة التطبيق:** الصندوق الجديد **يحتضن اللوحة القائمة** (`gbX.Controls.Add(pnlX)` + `pnlX.Dock = Fill`) ولا يُعاد تسمية أي عنصر؛ `Controls.Add` على النموذج يستبدل اللوحة بالصندوق في **نفس ترتيب الإرساء** (`RULE-N125`). لا صندوق داخل صندوق (`RULE-N137`) — `pnlFilters` يبقى عارياً و`gbFilter` هو صندوقه.

## خارج النطاق (بالاسم)

- `frmAddUpdateSalesInvoice*` (**المرجع — لا يُمسّ**) · `frmAddUpdatePurchaseInvoice*` · `frmPOS*` · `frmPurchaseInvoicePrint*`
- التقارير الـ**26** (`RULE-N121`) · نوافذ الطباعة الـ**5**
- بقية الوحدات الـ**46** شاشة — مرحلة 2 بعد شهادة المالك
- `Business/**` · `DataAccess/**` · `Models/**` · `DTO/**` · `Services/**` · `Program.cs` · `Helpers/**`
- أي `DataGridView` (`ReadOnly`/`SelectionMode`/`AllowUserToAddRows`) أو منطق حفظ/ترحيل/تصفية

## معيار الإغلاق

1. خمسة مصمّمات تحمل الصناديق المعلنة بأسمائها وعناوينها العربية، و**صفر تغيير** في `.cs` (وإلا فالعيب في التصميم).
2. `dotnet build` = **0 Errors**.
3. صفر صندوق داخل صندوق · صفر لوحة عارية في الكتلة الوظيفية · صفر عنصر دائم يُبنى وقت التشغيل (`RULE-N145`).
4. صيغة الألوان `Convert.ToInt32` (`RULE-N204`) · قيم المرجع حرفياً (`RULE-N134`/`RULE-N140`).
5. ارتفاع الشبكة المرئي ≥ **380** في القوائم و≥ **150** لكل شبكة في المنتقيات، و`ClientSize` **بلا تغيير**.
6. مراجعة واحدة `code-reviewer` على الشريحة الكاملة + قائمة اختبار عربي.
