# TASK-026 — مراجعة الكود الميت في الباك-إند (Dead Backend Code Audit)

| الحقل | القيمة |
|---|---|
| `ID` | `TASK-026` |
| `العنوان` | **جرد مقيس لكل نوع (type) في `Business` · `DataAccess` · `DTO` · `Common` لا يُستهلك من أي ملف آخر، وإزالة المؤكَّد ميتاً** |
| `الحجم` | **M** — ~46 نوعاً في 4 مشاريع · صفر تغيير سلوك · صفر هيكل قاعدة بيانات |
| `المرحلة` | `—` (تنظيف) |
| `الحالة` | **مفتوحة** — **مكتملة التنفيذ** (2026-11-20): 20 ملفاً محذوفاً + 5 ملفات جراحية · **34 نوعاً + عضو enum** أُزيلت · بناء **0 Errors / 112 Warning(s)** · `ToTable` **33** · ⏳ متبقٍّ: موجة Desktop بعد استقرار شجرة `Screens/ → Forms/` |
| `الوكيل المالك` | `fast-agent` ×3 (DTO · Common · DataAccess) + `implement-agent` ×1 (Business) |
| `أثر قاعدة البيانات` | `لا شيء` — صفر `DROP` · صفر ترحيل |
| `التاريخ` | `2026-11-20` |
| `أمر المالك` | «no, you do not need to rename. I want from you review reamning dead Backend code and any code not necessary» |

---

## 1. القياس (Baseline)

الطريقة: قراءة **كل** ملفات `.cs` (بلا `obj/` و`bin/`) مرة واحدة، ثم لكل نوع معلن (`class`/`interface`/`record`/`enum`/`struct`) يُعدّ:
- **`Intra`** = عدد ذكر الاسم داخل ملف إعلانه.
- **`Ext`** = عدد **الملفات الأخرى** التي تذكر الاسم.

`Ext = 0` ⇒ لا مستهلك خارج الملف. ثم استُثني:
- **تهيئات EF** (`*Configuration`) — يكتشفها EF Core بمسح التجميعة، فغياب المرجع الصريح **ليس موتاً**.
- `SalesDbContextFactory` — مصنع وقت التصميم يحتاجه `dotnet ef`.
- `SalesSystem.Tests/**` — ملك المالك (`RULE-N115`) ولا يُمسّ.
- الأنواع المساعدة **داخل ملفها** (`Intra > 1`) حين تكون تفصيلاً داخلياً مقصوداً (مثل `EscPos` · `FormatOption` · `ReportEntry` · `RequiredField`).

**النتيجة الخام:** 141 نوعاً بـ`Ext = 0` · **107** بعد استثناء التهيئات والمصنع والتests.

---

## 2. المؤكَّد ميتاً في الباك-إند (قائمة مغلقة)

### 2.1 `SalesSystem.DTO` — 33 نوعاً

| # | النوع | الملف | ملاحظة |
|---|---|---|---|
| 1 | `CustomerResponse` | `Responses/CustomerResponse.cs` | ملف كامل — بقية عصر الـAPI |
| 2 | `ProductResponse` | `Responses/ProductResponse.cs` | ملف كامل |
| 3 | `SupplierResponse` | `Responses/SupplierResponse.cs` | ملف كامل |
| 4 | `UnitResponse` | `Responses/UnitResponse.cs` | ملف كامل |
| 5 | `SettingsResponse` · `DashboardSummaryResponse` · `UserResponse` · `TopProductResponse` | `Responses/MiscResponses.cs` | **حذف الأنواع — الملف باقٍ** (`ResetPasswordResponse` حيّ) |
| 6 | `PurchaseInvoiceResponse` · `PurchaseInvoiceLineResponse` | `Responses/PurchaseInvoiceResponses.cs` | **الملف يصير فارغاً ⇒ يُحذف** |
| 7 | `SalesInvoiceResponse` · `SalesInvoiceLineResponse` | `Responses/SalesInvoiceResponses.cs` | **الملف يُحذف** |
| 8 | `SalesReturnResponse` · `SalesReturnItemResponse` · `PurchaseReturnResponse` · `PurchaseReturnItemResponse` | `Responses/ReturnResponses.cs` | **الملف يُحذف** |
| 9 | `InventoryTransactionResponse` · `InventoryTransactionLineResponse` · `WarehouseResponse` · `WarehouseStockSummaryResponse` | `Responses/WarehouseResponses.cs` | **الملف يُحذف** |
| 10 | `CreateLogRequest` | `Requests/CreateLogRequest.cs` | ملف كامل |
| 11 | `DeleteUnitRequest` | `Requests/DeleteUnitRequest.cs` | ملف كامل |
| 12 | `ReportExportRequest` | `Requests/ReportExportRequest.cs` | ملف كامل |
| 13 | `RestoreBackupRequest` · `UpdateDocumentSequenceRequest` · `ReportFilterRequest` | `Requests/MiscRequests.cs` | **حذف الأنواع — الملف باقٍ** (3 طلبات إعدادات حيّة) |
| 14 | `CreateSalesInvoiceItemRequest` · `CreatePurchaseInvoiceItemRequest` | `Requests/InvoiceItemRequests.cs` | **الملف يُحذف** |
| 15 | `SetPasswordRequest` · `ResetUserPasswordRequest` | `Requests/UserRequests.cs` | **حذف النوعين — الملف باقٍ** (`CreateUserRequest` · `UpdateUserRequest` حيّان) |
| 16 | `ProductImportExecuteRequest` | `Requests/ProductImportRequests.cs` | **الملف يُحذف** |
| 17 | ~~`CreateInventoryTransactionLineRequest`~~ | `Requests/WarehouseRequests.cs` | ⛔ **ليس ميتاً** — يستهلكه `CreateInventoryTransactionRequest.Lines` (نوع حيّ في 6 ملفات). **لم يُمسّ** |
| 18 | `BackupSettings` | `Common/BackupSettings.cs` | ملف كامل |
| 19 | ~~`ReportDataTableHelper`~~ · `DailyClosureReportDto` | `ReportDtos.cs` | ⛔ **`ReportDataTableHelper` ليس ميتاً** — يعلن امتداد `ToDataTable<T>(this List<T>, …)` المستهلك في `ReportExportService.cs:41`؛ **أُعيد من `HEAD`**. `DailyClosureReportDto` وحده حُذف |
| 20 | `CashTransactionType` | `Enums/CoreEnums.cs` | enum من عصر المحاسبة · **حذف السطر فقط** |

### 2.2 `SalesSystem.Common` — 8 أنواع

| # | النوع | الملف | القرار |
|---|---|---|---|
| 1 | `CostingMethod` | `Enums/CostingMethod.cs` | **حذف الملف** |
| 2 | `DistributionMethod` | `Enums/DistributionMethod.cs` | **حذف الملف** |
| 3 | `PriceLevel` | `Enums/PriceLevel.cs` | **حذف الملف** |
| 4 | `StockIssueReason` | `Enums/StockIssueReason.cs` | **حذف الملف** |
| 5 | `UnitType` | `Enums/UnitType.cs` | **حذف الملف** |
| 6 | `WarehouseType` | `Enums/WarehouseType.cs` | **حذف الملف** |
| 7 | `NotFoundException` · `ValidationException` | `Exceptions/*.cs` | **حذف الملفين** — لا `throw` ولا `catch` لهما |
| 8 | ~~`DateTimeExtensions`~~ | `Helpers/DateTimeExtensions.cs` | **يبقى** — يستهلكه `SalesSystem.Tests` (ملك المالك، `RULE-N115`) |
| 9 | `UserRole.Accountant = 3` | `Enums/UserRole.cs` | **حذف السطر** (D-4) — القيم الأخرى صريحة فلا تُرقَّم ثانية |

### 2.3 `SalesSystem.DataAccess` — 3 أنواع

| # | النوع | الملف | القرار |
|---|---|---|---|
| 1 | `ScheduledBackupWorker` | `Backup/ScheduledBackupWorker.cs` | **حذف الملف** — غير مسجَّل كـHostedService ولا مذكور في `Program.cs` |
| 2 | `FirstRunSetupService` | `Security/FirstRunSetupService.cs` | **حذف الملف** |
| 3 | `SecurityAudit` | `Security/SecurityAudit.cs` | **حذف الملف** |
| — | `DOCINFOA` · `EscPos` · `DbContextTransactionWrapper` | — | **تبقى** — مساعدة داخل ملفها (P/Invoke · مولّد حراري · غلاف UoW) |

### 2.4 `SalesSystem.Business` — 1 نوع

| # | النوع | الملف | القرار |
|---|---|---|---|
| 1 | `SmartUnitFormatter` | `Services/SmartUnitFormatter.cs` | **حذف الملف** — صفر مستهلك |

---

## 2.5 التصحيح بعد البناء — درس طريقة القياس (إفصاح)

أول بناء بعد الحذف أعطى **خطأ واحداً**: `ReportExportService.cs(41): CS1061 'List<T>' does not contain a definition for 'ToDataTable'`. السبب أن `ReportDataTableHelper` **حيّ** لكنه يُستهلك **كطريقة امتداد** — الاستدعاء `data.ToDataTable(columnHeaders)` لا يذكر اسم الصنف إطلاقاً، فالمسح القائم على الاسم أعطاه `Ext = 0` خطأً.

| البند | الحكم المصحَّح | الإجراء |
|---|---|---|
| `ReportDataTableHelper` | **حيّ** (امتداد `ToDataTable`) | أُعيد من `HEAD` كاملاً كما كان |
| `CreateInventoryTransactionLineRequest` | **حيّ** (عضو في `CreateInventoryTransactionRequest`) | لم يُمسّ — وكيل الـDTO توقّف عنده وأبلغ |
| **قاعدة تُضاف للجرد القادم** | كل **`static class`** في الباك-إند يُفحَص بأسماء **أعمدته العامة** أيضاً لا باسمه فقط — `this`-extension لا يظهر في البحث بالاسم | تُطبَّق في موجة Desktop المؤجَّلة |

**العدد النهائي المنفَّذ:** **34 نوعاً + عضو enum واحد** (`Accountant`) — لا 33 — بعد استبعاد النوعين الحيّين أعلاه.

---

## 3. مؤجَّل إلى موجة ثانية (Desktop — الشجرة تُعاد هيكلتها الآن)

`Screens/` → `Forms/` **جارٍ في شجرة العمل من جلسة أخرى** (ملفات `Screens/**` محذوفة و`Forms/**` تُبنى). لذلك **كل قياس Desktop مؤجَّل** حتى تستقر الشجرة، ومنه المرشَّحات المرصودة:
`BaseModuleControl` · `LoadingOverlayControl` · `SearchBarControl` · `ComboBoxItem` · `DocumentTextHelper` · `NumericInputHelper` · `SessionManager` · `NavigationItem` · `SalesLineDto` · `IPrinterService` · `PrintDtoExtensions` · 13 رسالة `*ChangedMessage`/`SessionExpiredMessage` · `SalesDocumentMode` · `PurchaseDocumentMode` · `RowFilterHelper` · `SmartUnitFormatter`.

> **⚠️ تحذير مقيس:** `SalesDocumentMode` و`PurchaseDocumentMode` (نوعان جديدان من `TASK-017`/`TASK-018`) ظهرا بصفر مستهلك في القياس الذي جرى **بعد** نقل الشاشات — أي أن شاشتي الفاتورة **لا تستخدمانهما**. يُعاد القياس بعد استقرار الشجرة قبل أي حكم.

---

## 4. تأثير التوثيق (إلزامي في نفس الموجة)

`AGENTS.md` §3 يسرد **22 enum** — بعد الحذف تصبح **16**. تُحدَّث القائمة بنفس الموجة، ولا يُترك enum محذوف مذكوراً.

---

## 5. البوابات

| # | البوابة | المتوقع | المقيس 2026-11-20 |
|---|---|---|---|
| 1 | `dotnet build .\SalesSystem.slnx` | **0 Errors** | ✅ **0 Errors / 112 Warning(s)** (بعد إعادة `ReportDataTableHelper`) |
| 2 | إعادة القياس بعد الحذف | كل نوع محذوف = **0 نتيجة** | ✅ 34 نوعاً + `Accountant` = **0** |
| 3 | `Accountant` | **0** في `Common` و`DTO` | ✅ **0** |
| 4 | `ToTable` | **33** (لا مسّ للموديل) | ✅ **33** |
| 5 | `SalesSystem.Tests` | **لم يُمسّ** | ✅ `DateTimeExtensions` باقٍ · صفر تعديل في المشروع |

---

## 6. سجل التغييرات

| التاريخ | التغيير |
|---|---|
| `2026-11-20` | إنشاء السجل: قياس 652 ملفاً · 750 نوعاً · 141 بلا مستهلك خارجي · 107 بعد الاستثناءات · 45 نوعاً مؤكَّد ميتاً في الباك-إند |
| `2026-11-20` | **التنفيذ:** 20 ملفاً محذوفاً (14 DTO + 6 Common/DataAccess/Business) + 5 ملفات DTO جراحية + `UserRole`/`CoreEnums` · **34 نوعاً + عضو enum** أُزيلت · بناء **0 Errors** |
| `2026-11-20` | **تصحيح مقيس:** `ReportDataTableHelper` أُعيد (امتداد `ToDataTable`) و`CreateInventoryTransactionLineRequest` لم يُمسّ — و`AGENTS.md` §3 أُعيد كتابته بأمر القياس (22 → **16** تعداداً) |

---

لم يُكتب أي اختبار آلي — الاختبار مسؤولية المالك (AGENTS.md §2.31).
