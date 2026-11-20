# F-007 — الخطة التنفيذية (12 موجة · 5 مراحل)

## قاعدة المراحل

| المرحلة | النطاق | الشاشات | الموجات | بوابة الانتقال |
|---|---|---|---|---|
| `P2-A` | الماسترات (أكثر الشاشات استخداماً يومياً) | 11 | `W1`–`W3` | شهادة بصرية للمالك |
| `P2-B` | شاشات القوائم | 9 | `W4`–`W6` | شهادة بصرية |
| `P2-C` | المنتقيات وكشوف الحساب | 5 | `W7`–`W8` | شهادة بصرية |
| `P2-D` | محررات المستندات والحوارات | 9 | `W9`–`W11` | شهادة بصرية |
| `P2-E` | شاشات الإعدادات والأدوات | 3 | `W12` | شهادة بصرية + إغلاق `F-007` |

**لماذا هذا الترتيب:** الماسترات هي ما يفتحه المستخدم كل يوم (إضافة/تعديل صنف وعميل ومورد) ⇒ أثرها الفوري أكبر؛ ثم القوائم؛ ثم الحوارات التي تُفتح من داخل شاشات أخرى؛ ثم الإعدادات.

## الموجات

| الموجة | # | الشاشة | الحجم | الصناديق المطلوبة |
|---|---|---|---|---|
| `W1` | 1 | `frmCRUDCustomerMaster` | S | «بيانات العميل» (`pnlFields`) — الباقي مجمَّع |
| `W1` | 2 | `frmCRUDCSupplierMaster` | S | «بيانات المورّد» (`pnlFields`) |
| `W1` | 3 | `frmCRUDCProductMaster` | S | «أوامر الشاشة» (`pnlButtons`) |
| `W1` | 4 | `frmCRUDCProductUnitMaster` | M | أوامر · بيانات · الأصناف · ملخص |
| `W2` | 5 | `frmCRUDCategoryMaster` | M | أوامر · بيانات · الفئات · ملخص |
| `W2` | 6 | `frmCRUDUnitMaster` | M | أوامر · بيانات · الوحدات · ملخص |
| `W2` | 7 | `frmCRUDWarehouseMaster` | M | أوامر · بيانات · المستودعات · ملخص |
| `W2` | 8 | `frmCRUDCashBoxMaster` | M | أوامر · بيانات · الصناديق · ملخص |
| `W3` | 9 | `frmCRUDUserMaster` | M | أوامر · بيانات · المستخدمون · ملخص |
| `W3` | 10 | `frmCRUDCExpensesMaster` | M | أوامر · بيانات · المصروفات · ملخص |
| `W3` | 11 | `frmCRUDCustomerReceiptsMaster` | M | أوامر · بيانات (الباقي مجمَّع جزئياً) |
| `W4` | 12 | `frmListPayments` | S | «المدفوعات» (`tabPayments`) · «ملخص النتائج» |
| `W4` | 13 | `frmListSupplierPayments` | M | أوامر · المدفوعات · ملخص · تلميحات |
| `W4` | 14 | `frmListCashBoxTransactions` | M | الحركات · ملخص · تلميحات |
| `W4` | 15 | `frmBackup` | M | النسخ الاحتياطية · ملخص · تلميحات |
| `W5` | 16 | `frmInventoryList` | M | أوامر · المخزون · ملخص |
| `W5` | 17 | `frmListInventoryAdjustments` | S | التسويات (`pnlGrid`) · ملخص |
| `W5` | 18 | `frmListInventoryBatches` | S | الدفعات · ملخص |
| `W5` | 19 | `frmListInventoryTransactions` | S | الحركات · ملخص |
| `W6` | 20 | `frmInventory` | M | أوامر · التبويبات · ملخص |
| `W7` | 21 | `frmCustomerSelection` | M | أوامر · العملاء · ملخص · تلميحات |
| `W7` | 22 | `frmProductSelection` | M | أوامر · الأصناف · ملخص · تلميحات |
| `W7` | 23 | `frmSupplierSelection` | M | أوامر · الموردون · ملخص · تلميحات |
| `W7` | 24 | `frmSupplierStatement` | M | أوامر · كشف المورّد · ملخص |
| `W8` | 25 | `frmCustomerStatement` | L | تصفية · إحصاءات · كشف العميل · حركات · تلميحات (`splitMain`) |
| `W9` | 26 | `frmPOS` | S | «أوامر البيع» (`pnlActions`) |
| `W9` | 27 | `frmWarehouseTransferNew` | S | «تلميحات التحويل» (`pnlFooter`) |
| `W9` | 28 | `frmInventoryFifo` | S | «ملخص الطبقات» (`pnlInfo`) · التلميحات |
| `W9` | 29 | `frmImportProductsExcel` | S | ملخص الاستيراد · تلميحات |
| `W10` | 30 | `frmPurchaseItem` | S | «بيانات الصنف» (`pnlBody`) · أوامر النافذة |
| `W10` | 31 | `frmPurchaseDiscount` | S | «الخصم» (`pnlBody`) · أوامر النافذة |
| `W10` | 32 | `frmPurchaseOtherDetails` | S | «بيانات إضافية» (`pnlBody`) |
| `W10` | 33 | `frmPurchaseItemInfo` | S | «تلميحات الصنف» (`pnlFooter`) |
| `W11` | 34 | `frmSupplierPaymentDialog` | S | «تلميحات السداد» (`pnlFooter`) |
| `W12` | 35 | `frmChangeSystemColorsSettings` | S | ملخص الألوان · تلميحات |
| `W12` | 36 | `frmFormatingSetting` | S | ملخص التنسيق · تلميحات |
| `W12` | 37 | `frmPasswordChange` | S | «تلميحات كلمة المرور» (`pnlFooter`) |

## مؤشرات التقدم

| المؤشر | القيمة |
|---|---|
| إجمالي الوكلاء | **37** |
| الموجات | **12** |
| الشاشات المكتملة | `0 / 37` |
| الصناديق المتوقعة (تقديري) | ≈ **95** صندوقاً جديداً |
| مراجعات `code-reviewer` | **5** (واحدة لكل مرحلة) |
| نقاط الشهادة البصرية | **5** |

## بعد الإغلاق

تُنقل `F-007` إلى `docs/TASKS/ARCHIVE/` كميزة مغلقة (`RULE-N202`) وتُحدَّث `AGENTS.md` §2.34 بالعدد النهائي المقيس (صفر شاشة بلا تجميع خارج المعفى).
