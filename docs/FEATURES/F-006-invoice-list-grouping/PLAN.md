# F-006 — الخطة والموجات

| الموجة | الوكيل | الشاشة | الملف | الحالة |
|---|---|---|---|---|
| `W1` | `ui-agent` #1 | `frmListSales` | `Sales/frmListSales.Designer.cs` | ⏳ |
| `W1` | `ui-agent` #2 | `frmListPurchases` | `Purchases/frmListPurchases.Designer.cs` | ⏳ |
| `W1` | `ui-agent` #3 | `frmListReturns` | `Returns/frmListReturns.Designer.cs` | ⏳ |
| `W1` | `ui-agent` #4 | `frmSalesInvoiceSelection` | `Sales/frmSalesInvoiceSelection.Designer.cs` | ⏳ |
| `W2` | `ui-agent` #5 | `frmPurchaseInvoiceSelection` | `Purchases/frmPurchaseInvoiceSelection.Designer.cs` | ⏳ |
| بعد `W1` | `Orchestrator` | بناء `dotnet build` = 0 Errors | — | ⏳ |
| بعد `W2` | `Orchestrator` | بناء `dotnet build` = 0 Errors | — | ⏳ |
| بعد البناء | `code-reviewer` | مراجعة واحدة على الشريحة الكاملة | — | ⏳ |
| الإغلاق | `Orchestrator` | docs + commit + push + تقرير عربي | — | ⏳ |

**قاعدة الموجة:** 4 وكلاء كحد أقصى (`RULE-N159`) · ملف واحد لمالك واحد (`RULE-N158`) · الوكيل لا يبني (`RULE-N117`).

## لماذا 5 وكلاء وليس وكيلاً واحداً

`RULE-N156` — كل شاشة عقد تصميم مستقل: كل مصمّم ~38–55 ك.ب وبنية إرساء مختلفة (`tabReturns` بدل `pnlGrid`، وشبكتان في المنتقيات). الدمج في وكيل واحد يجعل التشخيص أصعب ويخالف سقف الموجة.

## مرحلة 2 (مؤجلة بقرار المالك)

بقية الوحدات الـ**46** شاشة (`Masters` · `Inventory` · `Settings` · `Tools` · `Payments` · `CustomerReceipts` · `Expenses` · `Selection` خارج الفواتير) — **لا تُفتح قبل شهادة المالك على شكل المرحلة 1** (`RULE-N184`).
