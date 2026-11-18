# TASK-007 — فصل وحدة `Users` عن بقية الجداول (تجزئة النظام إلى وحدات صغيرة)

> **قرار المالك (2026-11-17) — نصّه الحرفي:**
> 1. «انا اريد ان اسوي حركه تجعل من النظام مثل الوحدات الصغيرة انا اريد ان افصل جدول المستخدمين عن بقيت الجدول الاخرى لا ان يرتبط بالمعرف لكي نجزء النظام إلى وحدات صغير منفصلة واسجل اسم المستخدم الحالي من على الوجهات كنص»
> 2. «nvarchart» (تأكيد نوع العمود: `nvarchar`)
> 3. «remove CreatedByUserId and UpdateByUserId and repllase by CreatedByUserName and UpdateByUserName for all system frist update docs/database-schema.md»

**الحالة:** 🔄 مفتوحة — الوثيقة محدَّثة (الخطوة المطلوبة أولاً)، والتنفيذ على موجات.
**النوع:** L (system-wide · 3 طبقات · > 8 ملفات) ⇒ ملف تتبع واحد (`RULE-N202`).
**الوثيقة المرجعية:** `docs/database-schema.md` §2 (القرار + الترتيب الملزم) + §11 (سجل تغييرات الهيكل) — **محدَّثة**.

---

## 0) القرار النهائي (استبدال لا إضافة)

`Users` **وحدة مستقلة**: لا مفتاح أجنبي إليها، ولا عمود معرّف مستخدم في أي جدول آخر.

| يُضاف | يُزال نهائياً |
|---|---|
| `CreatedByUserName` `nvarchar(100) null` | `CreatedByUserId` `int` |
| `UpdatedByUserName` `nvarchar(100) null` | `UpdatedByUserId` `int` |
| `ReviewedByUserName` (`JournalEntries`) | `ReviewedByUserId` |
| `ClosedByUserName` (`FiscalPeriods` — V2) | `ClosedByUserId` |
| `CashierUserName` (`CashBoxes`) | `CashierUserId` |
| `UserName` (`AuditLogs` · `SystemLogs`) | `UserId` |

**المصدر:** جلسة الواجهة `ISessionService.Current.UserName` — لا كتابة يدوية، ولا قراءة من `Users` عند العرض.
**الدلالة:** لقطة تاريخية — إعادة تسمية مستخدم لا تُعيد كتابة الصفوف القديمة.

### ترتيب التنفيذ الملزم (`RULE-N198`)

1. نسخة احتياطية كاملة للقاعدة.
2. ترحيل (1): **إضافة** أعمدة الأسماء.
3. **تعبئة رجعية** من `Users.UserName` بمطابقة المعرّف (على الأعمدة الجديدة فقط).
4. **تحويل** الكتابة والقراءة إلى النص فقط — مع بقاء الأعمدة الرقمية حتى تصبح الإشارات صفراً.
5. ترحيل (2): **إزالة** الأعمدة الرقمية بعد التحقق.

> **ممنوع:** `ALTER COLUMN` · إعادة تسمية · `DROP` قبل الخطوات 1–4 · تنفيذ 2 و5 في ترحيل واحد.

---

## 1) قياس الأساس (بالأمر — `RULE-N195`/`N196`)

| البند | الرقم |
|---|---|
| كيانات `SalesSystem.Models` | **40** (6 `AuditableEntity` · 9 `ActivatableEntity` · 10 `DocumentEntity` · 13 `Entity` · 2 `LongEntity`) |
| كيانات ذات حقول تدقيق | **25** |
| مواضع `CreatedByUserId\|UpdatedByUserId` في الحل | **1,556** (DataAccess 1,325 · Models 105 · Business 88 · Desktop 30 · Common 4 · DTO 4) |
| استدعاءات `SetCreatedBy(`/`SetUpdatedBy(` | **51** في **26** ملفاً |
| مفاتيح أجنبية فعلية إلى `Users` في EF | **3** (`AuditLogs.UserId` · `SystemLogs.UserId` · `JournalEntries.ReviewedByUserId`) |
| مفاتيح أجنبية إلى `Users` في الـsnapshot | **0** — الأعمدة الرقمية أعمدة عادية لا FKs |
| صفوف `Users(Id)` في الوثيقة قبل/بعد | 11 ← **0** |
| صفوف أعمدة الهوية النصية في الوثيقة | **68** موضع `nvarchar(100)` · `CreatedByUserName` 30 · `UpdatedByUserName` 26 |

---

## 2) خطة الموجات (بناء أخضر بعد كل موجة — `RULE-N159`)

| الموجة | الوكيل | الملفات (مغلقة) | لماذا |
|---|---|---|---|
| ✅ W1 | `backend-architect` | `Common/AuditableEntity.cs` · `Common/ICurrentUserProvider.cs` (جديد) | إضافة خصائص الاسم + عقد المستخدم الحالي — **تمّت، البناء 0 أخطاء** |
| ✅ W1b | `fast-agent` | `Models/AuditLog.cs` · `SystemLog.cs` · `InventoryAdjustment.cs` · `InventoryTransaction.cs` | إضافة `UserName`/`CreatedByUserName` — **تمّت** |
| 🔄 W2 | `backend-architect` | `Common/AuditableEntity.cs` | تحميل `SetCreatedBy(string?)`/`SetUpdatedBy(string?)` بجانب overload الرقمي (بناء يبقى أخضر) |
| 🔄 W2 | `implement-agent` | `Desktop/Services/SessionManager.cs` + ملف جديد `Desktop/Services/DesktopCurrentUserProvider.cs` + `Desktop/Program.cs` | تنفيذ `ICurrentUserProvider` من الجلسة وتسجيله |
| 🔄 W2 | `database-engineer` | `DataAccess/Configurations/**` | ربط أعمدة الأسماء (`HasMaxLength(100)`) بلا حذف بعد |
| ⏳ W3 | `fast-agent` ×2 | `Models/**` — تحويل بارامترات المصانع إلى `string? createdByUserName` (دفعتان ≤ 8 ملفات) | إزالة الاعتماد على المعرّف |
| ⏳ W3 | `backend-architect` ×2 | `Business/Services/**` — تمرير `_currentUser.UserName` عبر `ICurrentUserProvider` | مصدر الاسم |
| ⏳ W4 | `database-engineer` | ترحيل (1) إضافة + تعبئة رجعية | قاعدة البيانات |
| ⏳ W5 | `implement-agent` | `Services/Direct/**` + `DTO/**` | إسقاط المعرّف من العقود |
| ⏳ W6 | `database-engineer` | `Common/AuditableEntity.cs` (حذف الخصائص الرقمية) + ترحيل (2) إزالة | الإزالة النهائية |
| ⏳ W7 | `ui-agent` ×N | الشاشات التي تعرض «أنشأه/عدّله» | وكيل واحد لكل شاشة |
| ⏳ R | `code-reviewer` · `test-engineer` | مراجعة واحدة + قائمة اختبار عربية | بوابات الإغلاق |

---

## 3) خارج النطاق

- أي تغيير مالي/مخزني/محاسبي — هذا تغيير هوية تدقيق فقط.
- `SalesSystem.Tests/**` (ملك المالك — `RULE-N115`).
- أي صلاحيات أو أدوار (§2.29).
- `ALTER COLUMN` · إعادة تسمية · `DROP` قبل الخطوة 4.

---

## 4) سجل الجولات

### الجولة 3 — 2026-11-17 · W2–W6 + إعادة توليد الهجرات (قياس نهائي)

**المنفَّذ على موجات (كل موجة ≤ 4 وكلاء · البناء أخضر بعد كل موجة):**

| الخطوة | الناتج |
|---|---|
| `AuditableEntity` | `CreatedByUserName` · `UpdatedByUserName` فقط — **حُذف** `CreatedByUserId` · `UpdatedByUserId` وحمولتا `SetCreatedBy(int?)`/`SetUpdatedBy(int?)` |
| نماذج المصانع (24 ملفاً) | حُذف بارامتر المعرّف الرقمي من كل مصنع · بقي `string? createdByUserName` |
| Business (18+ ملفاً) | حُذفت وسائط المعرّف من كل نداء مصنع ومن `SetCreatedBy/SetUpdatedBy(int)` |
| `ICurrentUserProvider` + `DesktopCurrentUserProvider` | العقد + تنفيذه من `ISessionService` + تسجيله Singleton في `Program.cs` |
| `SalesDbContext` | ختم مركزي في `SaveChanges`/`SaveChangesAsync` (كيانـات `AuditableEntity` + الأربعة اليدوية) |
| `JournalEntry` | ~~`ReviewedByUserId` + تنقّل `ReviewedByUser` ⇒ `ReviewedByUserName` · `Post(string?)` · `Cancel()`~~ — **ملغى: الكيان لم يعد موجوداً** (أُخرج مع الوحدة المحاسبية 2026-11-17 — `TASK-011` §13؛ صفر مرجع 2026-11-18) |
| DTOs | `InventoryAdjustmentDto` · `InventoryTransactionDto` · `JournalEntryListDto` · `JournalEntryDetailDto` · `SalesByUserDto` ⇒ أسماء نصية |
| الهجرات | **حُذفت 37 ملفاً** وأُعيد التوليد: `20261117085853_InitialCreate` — **40 جدولاً** · `CreatedByUserName` 27 · `UpdatedByUserName` 25 · `ReviewedByUserName` 1 · **صفر** عمود رقمي |

**القياس النهائي (بالأمر):**

| البند | الرقم |
|---|---|
| `dotnet build --no-incremental` | **0 Errors · 118 Warnings** (= خط الأساس) |
| معرّفات التدقيق الرقمية في `Common` · `Models` · `DTO` · `DataAccess/Repositories` · `Desktop/Services` · `Screens` | **0** في كل منها |
| قراءات خاصية `*.CreatedByUserId` في `Business` | **0** |
| `Business` | **50** — أسماء بارامترات `int createdByUserId` في توقيعات خدمات BLL (لا تُخزَّن ولا تُقرأ — دَين تسمية فقط) |
| `DataAccess/Configurations` | **2** — تعليقان فقط |
| أعمدة الأسماء في الـsnapshot | **53** |

**لم يُنفَّذ بقرار المالك:** تطبيق الهجرات على قاعدة البيانات — المالك يتولاها.

### الجولة 1 — 2026-11-17 · التوثيق
`docs/database-schema.md` أُعيد تحريره بالكامل للهدف الجديد: §1.2 تسلسل الوراثة بالأسماء النصية · §1.6 حدّ الوحدة · §2 القرار + الترتيب الخمسي + نص الموافقة · §4 ملاحظة أن أعمدة الهوية هدف لا snapshot · §10 انحراف 7 محدَّث · §11 سجل تغييرات الهيكل. **قياس بعد التحرير:** `Users(Id)` = **0** · `nvarchar(100)` = **68** · `CreatedByUserName` = **30** · `UpdatedByUserName` = **26**.

### الجولة 2 — 2026-11-17 · W1 + W1b
`AuditableEntity` نال `CreatedByUserName`/`UpdatedByUserName` + overload اختياري · `ICurrentUserProvider` جديد · الأربعة كيانات اليدوية نالت حقول الاسم + بارامتراً اختيارياً. **البناء:** `dotnet build .\SalesSystem.slnx` = **0 Errors · 0 Warnings**.

---

## 5) معيار الإغلاق

1. `dotnet build .\SalesSystem.slnx` = **0 Errors**.
2. صفر إشارة إلى `CreatedByUserId`/`UpdatedByUserId` في الحل (عدا سجل الهجرة التاريخي).
3. ترحيلان فقط: إضافة + تعبئة، ثم إزالة — وقراءة SQL المولَّد (صفر `ALTER COLUMN`).
4. الوثيقة مطابقة للـsnapshot بعد التنفيذ.
5. مراجعة واحدة + قائمة اختبار يدوي عربية.
