# F-006 — جرد الملفات (`MANIFEST`) — `RULE-N201`

## 1) المطابقة العددية (مقيسة 2026-11-20، قبل الموجة وبعدها)

| البند | العدد |
|---|---|
| نماذج `frm*` في `Screens/**` (مصمّمات) | **85** |
| داخل نطاق `F-006` | **5** |
| خارج النطاق | **80** |
| التقارير (`Screens/Reports/**`) — معفاة (`RULE-N121`) | **26** |
| نوافذ الطباعة/المعاينة — معفاة (قراءة فقط) | **5** |
| محررات فواتير مُجمَّعة أصلاً (المرجع · الشراء · POS) | **3** |
| بقية الوحدات — مرحلة 2 مؤجلة | **46** |
| **المجموع** | **5 + 26 + 5 + 3 + 46 = 85** ✅ |

**قياس ما بعد الموجة (أمر واحد على الشاشات الخمس):** `new GroupBox()` = **26** (5 + 4 + 5 + 6 + 6) منها **21 جديداً** + **5** صناديق `gbFilter` قائمة · `Color.FromArgb(` بالشكل المختصر = **0** · صندوق داخل صندوق = **0**.

## 2) الملفات داخل النطاق (قائمة مغلقة)

| # | الملف | مالك الموجة | العمل | الحالة |
|---|---|---|---|---|
| 1 | `Screens/All Invoices Screens/Sales/frmListSales.Designer.cs` | `W1` / وكيل 1 | 4 صناديق: `gbToolbar` 86 · `gbGrid` Fill · `gbSummary` 68 · `gbFooter` 80 | ✅ |
| 2 | `Screens/All Invoices Screens/Sales/frmListSales.cs` | — | **ممنوع** — صفر تغيير | ✅ (0) |
| 3 | `Screens/All Invoices Screens/Sales/frmListSales.resx` | — | **ممنوع** — صفر تغيير | ✅ (0) |
| 4 | `Screens/All Invoices Screens/Purchases/frmListPurchases.Designer.cs` | `W1` / وكيل 2 | 3 صناديق: `gbToolbar` 86 · `gbGrid` Fill · `gbSummary` 54 | ✅ |
| 5 | `Screens/All Invoices Screens/Purchases/frmListPurchases.cs` | — | **ممنوع** | ✅ (0) |
| 6 | `Screens/All Invoices Screens/Purchases/frmListPurchases.resx` | — | **ممنوع** | ✅ (0) |
| 7 | `Screens/All Invoices Screens/Returns/frmListReturns.Designer.cs` | `W1` / وكيل 3 | 4 صناديق: `gbToolbar` 86 · `gbGrid` Fill (`tabReturns`) · `gbSummary` 68 · `gbFooter` 80 | ✅ |
| 8 | `Screens/All Invoices Screens/Returns/frmListReturns.cs` | — | **ممنوع** | ✅ (0) |
| 9 | `Screens/All Invoices Screens/Returns/frmListReturns.resx` | — | **ممنوع** | ✅ (0) |
| 10 | `Screens/All Invoices Screens/Sales/frmSalesInvoiceSelection.Designer.cs` | `W1` / وكيل 4 | 5 صناديق: `gbToolbar` 86 · `gbInvoices` 236 · `gbLines` 202 · `gbSummary` 68 · `gbFooter` 80 | ✅ |
| 11 | `Screens/All Invoices Screens/Sales/frmSalesInvoiceSelection.cs` | — | **ممنوع** | ✅ (0) |
| 12 | `Screens/All Invoices Screens/Sales/frmSalesInvoiceSelection.resx` | — | **ممنوع** | ✅ (0) |
| 13 | `Screens/All Invoices Screens/Purchases/frmPurchaseInvoiceSelection.Designer.cs` | `W2` / وكيل 5 | 5 صناديق: `gbToolbar` 86 · `gbInvoices` 208 · `gbLines` 190 · `gbSummary` 68 · `gbFooter` 80 | ✅ |
| 14 | `Screens/All Invoices Screens/Purchases/frmPurchaseInvoiceSelection.cs` | — | **ممنوع** | ✅ (0) |
| 15 | `Screens/All Invoices Screens/Purchases/frmPurchaseInvoiceSelection.resx` | — | **ممنوع** | ✅ (0) |

## 3) خارج النطاق — بالاسم ولماذا

| المجموعة | العدد | السبب |
|---|---|---|
| `Screens/Reports/**` | 26 | تُوحَّد بـ`ReportUiStandardizer` + 4 صناديق قائمة = `RULE-N121` |
| `frmPurchaseInvoicePrint` · `frmStocktakingPrint` · `frmStocktakingPrintAdjustment` · `frmStocktakingPrintCard` · `frmWarehouseTransferPrint` | 5 | نوافذ معاينة/طباعة للقراءة فقط — ليست شاشات إدخال |
| `frmAddUpdateSalesInvoice` | 1 | **المرجع** — لا يُمسّ |
| `frmAddUpdatePurchaseInvoice` | 1 | مُجمَّع في `TASK-022` (بانتظار اختبار المالك) |
| `frmPOS` | 1 | 4 صناديق قائمة + معياره §2.25 |
| بقية الوحدات | 46 | **مرحلة 2** — بعد شهادة المالك على شكل المرحلة 1 (`RULE-N184`) |

## 4) فحص البقايا بعد الإغلاق (مقيس — `RULE-N196`)

| الفحص | الهدف | المخرج |
|---|---|---|
| `new GroupBox()` في المصمّمات الخمس | 26 (21 جديد + 5 قائم) | ✅ `5/4/5/6/6` |
| مراجع `pnl(Header\|Toolbar\|Summary\|Footer\|Grid)` في `.cs` الخمسة | **0** | ✅ 0 |
| تغييرات في `.cs`/`.resx` | **0** | ✅ 0 (`git status`) |
| صندوق داخل صندوق | **0** | ✅ 0 |
| `Color.FromArgb(` بالشكل المختصر في الأسطر المضافة | **0** | ✅ 0 |
| `ClientSize`/`MinimumSize` | بلا تغيير | ✅ 5/5 |
| ارتفاع الشبكة عند `ClientSize` | ≥ 380 قوائم · ≥ 150 منتقيات | ✅ 438 · 530 · 438 · 213/179 · 185/167 |

## 5) ملاحظات إغلاق

- **D-07:** ارتفاعات الشبكتين في المنتقيين محسوبة على محتوى `pnlGrid` المتاح لكل شاشة (900 و860) ⇒ `236/202` و`208/190`، وكلاهما فوق بوابة 150 (`BRIEF` نصّ على «+22» فصحّحه القياس: كروم الصندوق **25–34** بكسل).
- **D-08 (بند مفتوح):** عند `MinimumSize` (700 ارتفاعاً) تنكمش شبكة القوائم إلى ≈300 بكسل — دون بوابة 380. رُفض رفع الحد الأدنى إلى 820 لأنه يخرج عن شاشات 768؛ يُترك للمالك مع المراجعة البصرية.
- الأسطر اللونية `235,240,235`/`235,245,235 → 170,220,170` على عناصر قائمة في هذه الملفات **سابقة لهذه الشريحة** (موجة `F-005` غير الملتزمة) — تُلتزم معها لعدم وجود فصل ممكن في `git`، وهي بنفس قيم المرجع فلا انحراف لوني فعلي (`D-09`).
