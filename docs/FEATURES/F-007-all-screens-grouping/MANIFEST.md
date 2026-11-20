# F-007 — الجرد الكامل (`MANIFEST`) — نسخة 2 بعد التدقيق (`RULE-N201`)

> كل رقم من أمر على الملفات (2026-11-20): عدّ `new GroupBox()` · استخراج `Controls.Add` العليا · `ClientSize` · `Select-String` على مراجع `pnl*` في `.cs` · ثم تدقيق رباعي على الملفات كاملة.

## 1) المطابقة العددية

| الفئة | العدد |
|---|---|
| إجمالي نماذج `frm*` | **85** |
| **المسار A — صناديق جديدة** | **21** |
| **المسار B — إعادة بناء** | **3** |
| **المسار C — عناوين فقط** | **5** |
| **لا عمل — مستوفاة** | **7** |
| **مستثنى** | **1** |
| منفَّذة (`F-006`) | **5** |
| المرجع | **1** |
| مُجمَّعة أصلاً | **5** |
| معفاة بسبب | **37** |
| **المجموع** | **21+3+5+7+1+5+1+5+37 = 85** ✅ |

## 2) المسار A — 21 شاشة (القياسات من المصمّم)

> **قرار المالك `D-11` (الخيار C) محسوم:** عمود «القرار» أدناه كان تقديرياً؛ **المرجع الملزم هو جدول `DECISIONS.md §D-11`** (3 صناديق للماسترات بلا صندوق ملخص عدّادي · صندوقان للمنتقيين · تكبير ≤40 بكسل · الأرضية `max(250, 75%)`). أي فرق يقيسه الوكيل يُبلَّغ ويُحدَّث قبل إغلاق الموجة.

| # | الشاشة | GB الآن | الكتل العليا المقيسة | الشبكة الآن → بعد | القرار | الموجة |
|---|---|---|---|---|---|---|
| 1 | `frmCRUDCProductUnitMaster` | 1 | grid · summary · filters · buttons · fields · lblMode | 336 → **200** | `D-11` +180 | `W1` |
| 2 | `frmCRUDCategoryMaster` | 1 | نفسها | 346 → **190** | `D-11` +190 | `W1` |
| 3 | `frmCRUDUnitMaster` | 1 | نفسها | 390 → **234** | `D-11` +146 | `W1` |
| 4 | `frmCRUDWarehouseMaster` | 1 | نفسها | 396 → **240** | `D-11` +140 | `W1` |
| 5 | `frmCRUDCashBoxMaster` | 1 | نفسها | 416 → **260** | `D-11` +120 | `W2` |
| 6 | `frmCRUDUserMaster` | 1 | نفسها | 328 → **172** | `D-11` +208 | `W2` |
| 7 | `frmCRUDCExpensesMaster` | 2 | نفسها (`gbDocument` داخل `pnlFields`) | 316 → **204** (3 صناديق) | `D-11` +176 | `W2` |
| 8 | `frmCRUDCustomerReceiptsMaster` | 3 | نفسها (`gbDocument`+`gbAmount` داخل `pnlFields`) | 371 → **259** (3 صناديق) | `D-11` +121 | `W2` |
| 9 | `frmListPayments` | 1 | tabPayments · summary · filters · header | 422 → **388** (سجل فقط) | ✅ بلا ملخص | `W3` |
| 10 | `frmListSupplierPayments` | 1 | grid · summary · filters · toolbar · header | 439 → **337** (3 صناديق) / 371 / 405 | `D-11` +43 | `W3` |
| 11 | `frmListCashBoxTransactions` | 2 | grid · footer · summary · filters · **gbToolbar** · header | 404 → **370** | `D-11` +10 | `W3` |
| 12 | `frmInventoryList` | 1 | grid · summary · filters · toolbar · header | 504 → **402** | ✅ | `W3` |
| 13 | `frmListInventoryBatches` | 3 | grid(GB) · summary(Panel) · filters · gbCommands · header | 501 → **467** | ✅ | `W4` |
| 14 | `frmInventory` | 1 | tabMain · summary · filters · toolbar · header | 467 → **399** (2 صناديق) | ✅ | `W4` |
| 15 | `frmCustomerSelection` | 1 | grid · summary · footer · filters · toolbar · header | 336 → **167** | `D-11` +180 | `W4` |
| 16 | `frmSupplierSelection` | 1 | نفسها | 336 → **167** | `D-11` +180 | `W4` |
| 17 | `frmSupplierStatement` | 1 | grid · summary · filters · toolbar · header | 452 → **384** (صندوقان) | ✅ | `W5` |
| 18 | `frmPOS` | 4 | gbCart · gbTotals · **pnlActions** · gbCustomerPayment · gbSalesCommands · header | 366 → **332** | ✅ | `W5` |
| 19 | `frmImportProductsExcel` | 3 | gbPreview · summary · footer · gbOptions · gbFile · lblMode | 270 → **202** | ✅ | `W5` |
| 20 | `frmChangeSystemColorsSettings` | 4 | gbColors · gbPreview · footer · **summary** · filters · gbToolbar · header | 434 → **400** | ✅ | `W5` |
| 21 | `frmFormatingSetting` | 3 | gbPreview · **summary** · footer · gbDates · gbNumbers · header | 323 → **289** (معاينة) | ✅ | `W6` |

## 3) المسار B — 3 شاشات (إعادة بناء · L)

| # | الشاشة | المانع المقيس | المطلوب |
|---|---|---|---|
| 1 | `frmCRUDCProductMaster` | `grpMainUnit` داخل `pnlFields` · grid+summary عاريتان · الشبكة 292 → 180 | تقسيم `pnlFields` + 3 صناديق |
| 2 | `frmProductSelection` | شبكتان في `pnlGrid` · `dgvProducts` ثابت 240 · `dgvUnits` 160 → **1** بأي صندوق | إعادة تقسيم إلى صندوقين + ضبط الارتفاعات |
| 3 | `frmCustomerStatement` | `splitMain`+`SplitterDistance 252` · `dgvAging` **92 قبل الموجة** | صندوقان خارجيان + `ClientSize` 731 + إعادة ضبط الفاصل |

## 4) المسار C — عناوين فقط (5)

| الشاشة | التغيير |
|---|---|
| `frmCRUDCustomerMaster` | `gbToolbar` «أوامر العملية» → «أوامر الشاشة» · `gbSummary` «الإجماليات» → «ملخص النتائج» |
| `frmCRUDCSupplierMaster` | نفسه + رفع `gbSummary` 48 → 60 (سطره مقصوص) |
| `frmListInventoryAdjustments` | `pnlGrid` «البيانات» → «تسويات المخزون» · `pnlSummary` «الإجماليات» → «ملخص النتائج» |
| `frmListInventoryTransactions` | `pnlGrid` «البيانات» → «حركات المخزون» · `pnlSummary` «الإجماليات» → «ملخص النتائج» |
| `frmBackup` | `gbSummary` «الإجماليات» → «ملخص النتائج» |

## 5) لا عمل — مستوفاة أصلاً (7)

| الشاشة | السبب |
|---|---|
| `frmInventoryFifo` | `pnlInfo` يحتضن `gbInfo` و`pnlFooter` يحتضن `gbTotals` — كل الكتل مغلَّفة |
| `frmPurchaseItem` | `pnlBody` يحتضن `gbItemData` — مستوفى |
| `frmPurchaseDiscount` | `grpDiscount` + `grpSummary` داخل `tblMain` — مستوفى |
| `frmPurchaseOtherDetails` | `gbCosts` + `gbTotal` + `gbNotes` — مستوفى |
| `frmPurchaseItemInfo` | `gbProductInfo` + `gbCosts` + `gbHistory` — مستوفى |
| `frmSupplierPaymentDialog` | `gbPaymentInfo` هو صندوق المحتوى — مستوفى |
| `frmWarehouseTransferNew` | `gbToolbar` + `grpHeader` + `gbItems` — والتلميح شريط سفلي معفى (`RULE-N137`) |

## 6) مستثنى (1)

| الشاشة | السبب المقيس |
|---|---|
| `frmPasswordChange` | `ClientSize == MinimumSize` = 620×410 · `lblError` بموضع مطلق 298–324 · أي صندوق يدفع الحدّ إلى 308 ⇒ **يعترض رسالة الخطأ** في وضع الخدمة الذاتية (أول تشغيل) |

## 7) خارج `F-007` (48)

| المجموعة | العدد |
|---|---|
| `F-006` منفَّذة | 5 |
| المرجع `frmAddUpdateSalesInvoice` | 1 |
| مُجمَّعة أصلاً (`frmAddUpdatePurchaseInvoice` · `frmAddUpdateInventoryAdjustment` · `frmInventoryBarcode` · `frmOpeningStock` · `frmStocktakingNew`) | 5 |
| التقارير | 26 |
| نوافذ الطباعة | 5 |
| هياكل/أدوات (`frmMain` · `frmDashboard` · `frmCalculator` · `frmLogin` · `frmFindCustomer` · `frmShowCustomerCard`) | 6 |
| **المجموع** | **48** |

## 8) فحص البقايا بعد كل موجة

| الفحص | الهدف |
|---|---|
| `new GroupBox()` | = المتوقع |
| `(gb\w+)\.Controls\.Add\((gb\w+)\)` | **0** |
| `Color\.FromArgb\(\s*\d` في الملف | **0** |
| `.cs`/`.resx` معدَّلة | **0** (المسار B باستثناء معتمد) |
| `ClientSize` | ثابت أو فرق معتمد (`D-11`) |
| الشبكة بعد | مطابقة للعمود المقيس أو `D-11` |
| البناء | **0 Errors** |
