# TASK-019 — إزالة كل حقول الضريبة من MVP V1 (المخطط + الكود)

| الحقل | القيمة |
|---|---|
| الحالة | **مفتوحة** — قيد التنفيذ (موجة بعد موجة) |
| النوع | إزالة ميزة من V1 (لا إضافة) — نظير `TASK-011` (إزالة المحاسبة) |
| أمر المالك (نصّه الحرفي) | «and remove each and every Tax columns form MVP v1 from database shema and project code» · «use many subagetns for remove each and every Tax columns form MVP v1» |
| التاريخ | `2026-11-18` |
| الحجم | **L** — مسح شامل: 84 ملفاً مصاباً بـ`.cs` + ملف مخطط + `AGENTS.md` |
| خارج النطاق | **`SalesSystem.Tests/**`** (ملك المالك) · **القاعدة الحيّة** (لا `DROP` ولا ترحيل — `RULE-N198`) · أي إعادة هيكلة أخرى |

---

## 1) القرار وحدوده

الضريبة **مؤجلة إلى V2** أصلاً (`AGENTS.md` §2.29 بند 4). هذه المهمة تُنفّذ الأثر الكامل لذلك القرار: **حذف كل سطح ضريبي من V1** — لا عمود، لا حقل DTO، لا خاصية كيان، لا إعداد، لا تقرير.

**ما لا تفعله هذه المهمة:** لا تُسقط أعمدة من القاعدة الحيّة. كل عمود محذوف هنا إمّا `NULL` أو يحمل `HasDefaultValue(0m)` في `Configurations/` ⇒ **الإدراج يبقى سليماً على القاعدة القائمة**، وإسقاط الأعمدة فعلياً يبقى للمالك بموافقة `DROP` مكتوبة (سابقة `TASK-011` §13).

## 2) السطح المقيس (أمر + مخرج)

```powershell
Select-String -Path SalesSystem/**/*.cs -Pattern '\b(tax|vat)' -CaseSensitive:$false
```

- **84 ملفاً** مصاباً في `.cs` قبل الموجة.
- **الأعمدة/الخصائص المحذوفة (7):** `SalesInvoices.TaxAmount` · `PurchaseInvoices.TaxAmount` · `SalesReturns.ReturnedTaxAmount` · `PurchaseReturns.ReturnedTaxAmount` · `Customers.TaxNumber` · `Suppliers.TaxNumber` · `CompanySettings.TaxNumber`.
- **سطح DTO:** `TaxAmount` · `TaxId` · `TaxName` · `TaxRate` · `IsTaxInclusive` · `ReturnedTaxAmount` · `TaxNumber` · `DefaultTaxRate` · `IsTaxEnabled` · `StoreTaxNumber` · `VatReportDto`.
- **إعدادات `SystemSettings` (تُزال من البذور والقراءة):** `HideTaxInSales` · `HideTaxInPurchases` · `DefaultSalesTax` · `DefaultPurchaseTax` · `Store.TaxNumber` · `StoreTaxNumber` · `TaxRate`.
- **تقرير يُحذف كاملاً:** `frmVatReport` (+ `msReportVat` في `frmMain` + بطاقة الـCatalog + تسجيل DI + `GetVatReportAsync` في BLL/Desktop/Mock + `VatReportDto` + فرع `"vat"` في `ReportExportService`) — تقرير بلا بيانات = كود ميت (`RULE-N102` روحاً).
- **الطباعة:** `StoreTaxNumber` + سطر الضريبة من `InvoicePrintDto` و`A4InvoiceDocument` و`ThermalReceiptGenerator` و`PrintDataService` و`InvoicePrintDtoBuilder` و`Desktop/Printing/**`.

## 3) ترتيب الموجات (يبقي البناء أخضر بعد كل موجة)

| الموجة | الطبقة | عدد الوكلاء | الملفات |
|---|---|---|---|
| 1 | `Desktop` (شاشات + خدمات Direct/Mock) | 4 | شاشات المشتريات · المبيعات · العملاء/الموردون · الخدمات |
| 2 | `Business` + الطباعة (3 طبقات) + التقارير و`Program.cs`/`frmMain` | 4 | خدمات الفواتير/المرتجعات · خدمات العملاء/الإعدادات · الطباعة · حذف تقرير الضريبة |
| 3 | `DTO` + `DataAccess` (تهيئة/بذور/مستودع تقارير) | 3 | عقود · عملاء/موردون · تهيئة + بذور |
| 4 | `Models` | 2 | الفواتير/المرتجعات · العملاء/الموردون/إعدادات الشركة |
| 5 | `docs` + `AGENTS.md` | الـOrchestrator | المخطط · القانون · الحالة |

**قاعدة الترتيب:** المستهلك (Desktop) → الخدمة (Business) → العقد (DTO) → الكيان (Models) → التوثيق. لا تُحذف خاصية قبل أن يتوقف كل من يقرأها.

## 4) البوابات

| البوابة | الحالة |
|---|---|
| `RULE-N198` تجميد الهيكل | ✅ لا `DROP` ولا ترحيل ولا `UPDATE` — الأعمدة تبقى في القاعدة الحيّة ملكاً للمالك |
| `RULE-N177` قفل الباك-إند | ✅ موافقة المالك المكتوبة مؤرَّخة أعلاه (إزالة ميزة من V1، نظير `TASK-011`) — والحذف لا يضيف سلوكاً |
| `RULE-N201` المسح | ✅ الجرد بأمر + الأعداد الثلاثة في §6 |
| `RULE-N117` البناء | الـOrchestrator وحده يبني؛ الوكلاء لا يبنون ولا يشغّلون |
| `RULE-N119` الاختبار | قائمة اختبار يدوي عربية في التقرير النهائي |

## 5) سجل الجولات

### الجولة 1 — الفتح والجرد (2026-11-18)
- قراءة `docs/TASKS/ACTIVE.md`، عدّ `\b(tax|vat)` في كل الطبقات، قراءة سطور `docs/database-schema.md` (185 · 207 · 259 · 544 · 586 · 665 · 706 · 825 · 834 · 871).
- قرار النطاق أعلاه + تقسيم الموجات.
- **لم يُعدَّل أي ملف كود بعد الجولة 1.**

### الجولة 2 — الموجة 1: Desktop (4 وكلاء · 2026-11-18)
| الشريحة | الوكيل | الملفات | النتيجة |
|---|---|---|---|
| شاشات المشتريات | `ui-agent` | `frmPurchaseDiscount.{cs,Designer}` · `frmPurchaseInvoicePrint.{cs,Designer}` | حُذف `cmbTax`/`lblTax*`/`TaxRate`/`TaxAmount`/`Tax` · `NetTotal = Subtotal - DiscountAmount` · `frmPurchaseDiscount` **مسجَّلة DI بلا أي مستدعٍ** (سطح ضريبتها أُزيل والشاشة باقية — لا حذف بلا أمر) |
| شاشة المبيعات + POS | `ui-agent` | `frmAddUpdateSalesInvoice.cs` | **المصمّم لا يحمل أي حقل ضريبي أصلاً** (0 مطابقة) ⇒ أُزيلت تعليقات ميتة + `CalculateTaxAmount` + وسائط الطلب · `ComputeTotalAmount`/`CalculateTotals` بلا حد ضريبي |
| العملاء/الموردون | `ui-agent` | `frmCRUDCustomerMaster.{cs,Designer}` · `ctrlCustomerCard.{cs,Designer}` · `frmSupplierMaster.{cs,Designer}` | حُذف عمود/حقل/بطاقة «الرقم الضريبي» + عنصر «تصفية بـ» الرابع (0=الاسم · 1=الهاتف · 2=البريد) |
| خدمات Desktop | `implement-agent` | 17 ملفاً في `Services/{Interfaces,Direct,Mock}` | `GetVatReportAsync` أُزيل من العقد + التنفيذين · وسائط الضريبة من كل بناءات DTO · **صف بذرة واحد تغيّر رقمياً:** فاتورة بيع 1004 `NetTotal` 148,050 → 141,000 |

**ملاحظة مقيَّدة:** تقارير الموجة أثبتت أن وسائط الضريبة **موضعية وإلزامية** في سجلات `DTO` ⇒ لا يمكن حذفها قبل الموجة 2؛ لذلك انتقلت الموجة 2 إلى «العقود + الخدمات + الطباعة» معاً.

### الجولة 3 — الموجة 2: العقود + BLL + الطباعة + التقرير (4 وكلاء · 2026-11-18)
| الشريحة | الوكيل | النتيجة |
|---|---|---|
| `DTO/**` (17 ملفاً) | `backend-architect` | صفر مرجع ضريبي في `DTO/**` · `VatReportDto` محذوف · صفر قيم افتراضية مُضافة |
| `Business/Services` (10 ملفات) | `backend-architect` | `GetVatReportAsync` محذوف من العقد والتنفيذ · `SetTaxAmount` محذوف الاستدعاء · `UpdateTotals`/`SetProportionalAmounts` بقيمة محايدة (تُنظَّف في الموجة 3) · `StoreSettingsService` فقد `KeyTaxNumber` وحالات التحقق الأربع |
| الطباعة (3 طبقات · 9 ملفات) | `printing` | `InvoicePrintDto` بلا ضريبة · `LoadAllStoreInfoAsync` **15 → 13** حقلاً · `LoadStoreInfoAsync` **6 → 4** · حُذف سطر «الرقم الضريبي» وسطر «ض.ق.م» من A4 والحراري وشاشة الطباعة |
| تقرير الضريبة + الهيكل | `ui-agent` | **حُذفت `frmVatReport.{cs,Designer,resx}`** · `msReportVat` من `frmMain` · تسجيل DI · بطاقة `ReportNavigationHelper.Catalog` · `"vat"` من `ReportExportService` · تصحيح 7 إنشاءات `CustomerDto` الموضعية في `MockData` |

### الجولة 4 — الموجة 3: الكيانات + بقايا BLL + DataAccess (3 وكلاء · 2026-11-18)
| الشريحة | الوكيل | النتيجة |
|---|---|---|
| `Models/**` (7 كيانات) | `database-engineer` | 7 خصائص محذوفة · `SetTaxAmount` محذوفة من الفاتورتين · `UpdateTotals(discount, otherCharges, discountType, discountRate)` · `SetProportionalAmounts(discount, charges)` · `NetTotal = SubTotal - DiscountAmount + OtherCharges` · **صفر `\b(tax|vat)` في `Models/**`** · **صفر ترحيل** |
| بقايا BLL (8 ملفات) | `backend-architect` | حُذفت الوسائط المحايدة من كل استدعاء · `StoreSettingsDto` 14 وسيطاً |
| `DataAccess/**` (10 ملفات) | `database-engineer` | 7 تهيئات بلا ضريبة · 5 مفاتيح بذور محذوفة · `ReportRepository` بلا `i.TaxAmount` · `"vat"` من `ReportExportService` |

### الجولة 5 — البناء والإصلاح (2026-11-18)
- بناء 1: **4 أخطاء** `CS1729` (استدعاءات `MapToDto` ما زالت تمرّر أعضاء محذوفة) → `fast-agent` أصلحها بالاسم لا بالموضع.
- بناء 2: **5 أخطاء** (`CustomerDto` موضعي في `frmPOS` · `CreatePurchaseInvoiceRequest` 15 وسيطاً في `frmAddUpdatePurchaseInvoice`) → `fast-agent`.
- **بناء 3: `Build succeeded` — 0 Errors / 77 Warning(s)** (وضع تزايدي؛ البوابة 0 أخطاء).
- تنظيف نصي أخير: تسمية «(قبل الضريبة)» من بند الإيراد · تعليقات قديمة · XML docs في منتقيَي العميل/المورّد.
- **البقايا المقيسة بعد الموجة:** `\b(tax|vat)` في `.cs` = **صفر عضو كود** (تعليقات توثيق قرار الإزالة فقط) · `Models/**` = 0 · `DTO/**` = 0.

### الجولة 6 — التوثيق (2026-11-18)
- `docs/database-schema.md`: حُذفت 7 صفوف أعمدة + صُحّحت 5 أعداد أعمدة (Customers 16→15 · Suppliers 16→15 · CompanySettings 9→8 · SalesInvoices 24→23 · SalesReturns 20→19 · PurchaseInvoices 26→25 · PurchaseReturns 20→19) + معادلتا §6 بلا ضريبة + صف إزالة في §7 + تصحيح `UnitCost` → `UnitPrice` في معادلة الشراء.
- `AGENTS.md`: §2.2 معادلة الإجمالي · §2.20/§2.29 ملاحظة 2026-11-18 · `Business/Helpers` بلا الضريبة · سطر في §4 FORBIDDEN.
- `docs/MOCK-COVERAGE.md`: `IFinancialReportDesktopService` **12 → 4** دالّة (حُذف `GetVatReportAsync`).

## 6) الأعداد الثلاثة (`RULE-N201`)

| العدّ | العدد | الدليل |
|---|---|---|
| الملفات المصابة قبل الموجة | **84** | `Select-String -Pattern '\b(tax|vat)'` على `SalesSystem/**/*.cs` |
| داخل النطاق (مُنجَز) | **84** | كلها لُمست أو ثبت أنها تعليق توثيقي مقصود |
| خارج النطاق (بسبب) | **0** | `SalesSystem.Tests/**` = 0 مطابقة أصلاً |
| بقايا عضو كود بعد الموجة | **0** | `Models/**` = 0 · `DTO/**` = 0 · والبقايا كلها نصوص توثيق |

## 7) البوابات

- ✅ بناء **0 Errors** (77 تحذيراً تزايدياً — ليست بوابة).
- ⏸ **لم يُنفَّذ `DROP`** على القاعدة الحيّة — كل عمود محذوف `NULL` أو `HasDefaultValue(0m)`، وإسقاطه للمالك (`RULE-N198`).
- ⚠️ أثر بيانات مُبلَّغ: حفظ/تعديل عميل أو مورد في الوضع الحقيقي كان يمرّر `TaxNumber = null` وسيطاً قبل حذف العمود — أي أن تعديل سجل موجود كان سيمسح القيمة. بعد حذف العمود من الكود لا يُكتب العمود أصلاً.

## 8) المراجعة (`code-reviewer` — مرة واحدة على الشريحة)

**الحكم: `جاهز للالتزام` — 0 Blocker · 0 Major · 7 Minor.**

تحقّق آلياً من: معادلة `NetTotal` في 7 مواضع · `RemainingAmount` · حارس `PaidAmount` في الكيانين والـMockين · ترتيب وسائط `SetProportionalAmounts` · إعادة حساب بذرة 1004 (146,000 − 5,000 = **141,000**) · معاينة الشراء (650,000 − 8,000 = **642,000**) · حذف تقرير الضريبة من 6 مواضع · عدّ صفوف المخطط يدوياً · صفر ترحيل وصفر `DROP` · صفر `MessageBox.Show`/`Cells["…"]` في الملفات الممسوسة.

**الملاحظات السبع (Minor — لا تمنع الالتزام، مسجَّلة كدَين):**

| # | الموضع | الملاحظة |
|---|---|---|
| 1 | `SalesSystem.Desktop.csproj:40,53` | `<Compile Update>` لملف غير موجود (`frmAddUpdateSalesInvoice_Backup.cs`) + `<Folder Include="Screens\All Invoices Screens\">` لمجلد فارغ — **سابق للشريحة** (`RULE-N123`) |
| 2 | `.opencode/agents/printing.md:5` | تغيير `model:` — **ليس من هذه الشريحة** (تحرير سابق في الشجرة) |
| 3 | `docs/TASKS/ACTIVE.md` | تكرار فقرة «قاعدة التعايش» — **أُصلح في الجولة 6** |
| 4 | 3 مصمّمات (`frmFindCustomer` · `frmShowCustomerCard` · `frmExpensesMaster`) | صيغة `FromArgb` المختصرة بدل `Convert.ToInt32` (`RULE-N204`) — **سابق للشريحة** (نسخة عمل غير ملتزمة) |
| 5 | `frmFindCustomer.Designer.cs` · `frmShowCustomerCard.Designer.cs` | تغيّرت مواضع أزرار — **سابق للشريحة** (`TASK-017`/`F-002` المتراكبة) |
| 6 | قاعدة حيّة | فاتورة **مسودة قديمة** بـ`TaxAmount > 0` و`PaidAmount > 0`: عند الترحيل يصبح `NetTotal` أصغر وقد يصير `RemainingAmount` سالباً صامتاً (`RecalculateTotals` لا يمرّ على `SetPaidAmount`). **قرار المالك مطلوب قبل إسقاط الأعمدة:** افحص `TaxAmount > 0 AND PaidAmount > 0 AND Status = 1` |
| 7 | `frmFindCustomer.resx` | سطر نهاية الملف — تحرير سابق |
| — | `SalesReturn.RefundAmount` | **سابق للشريحة:** `SetRefundAmount` بلا مستدعٍ في BLL (الـMock يضبطه، الوضع الحقيقي يتركه 0) — مسجَّل للعلم |

## 9) منع الالتزام (سبب مفتوح)

الشجرة تحمل **165** تغييراً غير ملتزَم من `TASK-014`/`TASK-017`/`TASK-018`/`F-004` (شاشات، حذف نماذج، `Program.cs`/`frmMain`) — وهي مُنتظِرة اختبار المالك. ولأن `TASK-019` يمسّ `Models`/`DTO`، **لا يمكن فصل كومِت الضريبة عن بقية الشجرة دون كسر البناء**. القرار للمالك: كومِت واحد يضم الكل، أو الانتظار حتى تُختبر المهام المتراكبة ثم كومِت الضريبة عليها.
