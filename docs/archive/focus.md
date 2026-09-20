انا عندي مشروع اعمل عليه مع وكلاء الذكاء الاصطناعي انا اريد إعادة تنضيم وتوحيد الملفات التي تساعد الذكاء الاصطناعي انهو يشتغل على المشروع

# إعادة تنظيم ملفات السياق للذكاء الاصطناعي

فكرة ممتازة! هذا يُسمى عادةً **"AI Context Management"** أو **"Project Memory System"**

---

## أولاً: ما هي الملفات التي يحتاجها الذكاء الاصطناعي عادةً؟

```
📁 .ai-context/
├── 📄 project.md          # نظرة عامة على المشروع
├── 📄 architecture.md     # البنية التقنية
├── 📄 conventions.md      # قواعد الكود والاتفاقيات
├── 📄 progress.md         # الحالة الحالية والمهام
└── 📄 decisions.md        # القرارات المتخذة ولماذا
```

---

## ثانياً: لكي أساعدك بشكل صحيح، أحتاج أن أعرف:

**١. ما نوع مشروعك؟**
- تطبيق ويب / موبايل / API / أخرى؟

**٢. ما الملفات الموجودة حالياً؟**
- هل عندك `README`, `CLAUDE.md`, `cursorrules.` إلخ؟

**٣. أي أدوات AI تستخدم؟**
- Cursor / Claude / Copilot / Windsurf / أخرى؟

**٤. ما المشكلة الحالية؟**
- الملفات مبعثرة؟ متكررة؟ غير منظمة؟

---

## ابدأ بأخبرني عن وضعك الحالي وسأعطيك هيكل موحد مخصص لمشروعك 🎯


المشروع هو نظام إدارة مبيعات محاسبي مكتبي on windows forms C# N-Layer Architecture Entity Framework Core.   .... انا اعمل على  opencode ai cli

# تحليل المشروع وإعداد ملفات السياق

## هيكل ملفات السياق المقترح

```
📁 .ai-context/
├── 📄 CLAUDE.md              # الملف الرئيسي (opencode يقرأه تلقائياً)
├── 📄 architecture.md        # البنية التقنية التفصيلية
├── 📄 database-schema.md     # قاعدة البيانات والعلاقات
├── 📄 conventions.md         # قواعد الكود والتسمية
├── 📄 progress.md            # المهام والحالة الحالية
└── 📄 decisions.md           # القرارات التقنية
```

---

## قبل أن أكتب الملفات، أحتاج تفاصيل مشروعك:

**١. البنية الحالية للـ Layers:**
```
كيف قسّمت المشروع؟ مثلاً:
- Presentation Layer (Windows Forms)
- Business Logic Layer
- Data Access Layer
- Models/Entities
```

**٢. قاعدة البيانات:**
- اسم قاعدة البيانات؟
- SQL Server / SQLite؟
- الجداول الرئيسية الموجودة؟

**٣. الوحدات المكتملة والقيد العمل عليها:**
- مثلاً: العملاء ✅، الفواتير 🔄، المخزون ❌

**٤. هيكل الـ Solution الحالي:**
```
أرسل لي شكل المجلدات مثلاً:
SalesSystem.sln
├── SalesSystem.UI
├── SalesSystem.BLL
├── SalesSystem.DAL
└── SalesSystem.Models
```

**أرسل لي هذه التفاصيل وسأكتب لك الملفات كاملة وجاهزة** 🎯


SQL Server

ممتاز! SQL Server مع EF Core خيار قوي.

باقي الأسئلة محتاجها عشان أكتب الملفات صح:

---

## أرسل لي هذه المعلومات:

**١. هيكل الـ Solution:**
```
افتح Solution Explorer وأرسل لي أسماء المشاريع
مثلاً:
├── SalesSystem.UI
├── SalesSystem.BLL
└── ...
```

**٢. الـ Entities الموجودة:**
```
افتح مجلد Models أو Entities
وأرسل أسماء الكلاسات
```

**٣. الوحدات الموجودة في المشروع:**
```
مثلاً:
- العملاء (Customers)
- الموردين (Suppliers)
- المنتجات (Products)
- الفواتير (Invoices)
- ...
```

**٤. الوحدات المكتملة vs قيد التطوير:**
```
✅ مكتمل: ...
🔄 قيد العمل: ...
❌ لم يبدأ: ...
```

---

كل ما أعرف أكثر، كل ما الملفات تكون أدق وأفيد للـ AI 🎯


انا اريد امر لكي اعطيه للوكيل المحلي على جهازي لكي يعرف اي subagents are important or not for work and edit file and fix focus

## الفكرة واضحة

تريد **prompt/command** تعطيه للوكيل يقول له:

> "افحص المشروع بنفسك، وحدد الملفات المهمة، ونظّمها"

---

## هل هذا ما تقصده؟

```
اقرأ المشروع كاملاً، حدد:
1. أي الملفات مهمة للعمل
2. أي subagents تحتاج
3. عدّل وركّز على الصح
```

---

## قبل أكتب الأمر، سؤالان سريعان:

**١. subagents عندك يعني إيش؟**
- وكلاء متخصصين داخل opencode؟
- أم ملفات سياق منفصلة لكل وحدة؟

**٢. "edit file and fix focus" يعني إيش بالضبط؟**
- يعدّل ملفات السياق الموجودة؟
- أم يعدّل كود المشروع مباشرة؟
- أم يحدد على أي جزء يركز؟

---

وضّح لي هذين وأكتب لك الأمر جاهز فوراً 🎯


مثلا انا اريد العمل على تحليل شاشة معينة معي المراحل التي تقترح لي ان امشي عليها لكي احليل كل تفصيله والعمل ليها بشكل الصحيح ماهي الطريقه المثل لكي في الاخير يكون هناك نظام يعمل يشتغل في السوق

# منهجية تحليل وتطوير شاشة بشكل احترافي

## المراحل الكاملة

---

### 🔵 المرحلة 1: تحليل الشاشة (Analysis)

```
الهدف: فهم كل تفصيلة قبل كتابة أي كود

1.1 - تحديد الغرض
     - ماذا تفعل هذه الشاشة؟
     - من يستخدمها؟ (محاسب / مدير / كاشير)

1.2 - تحديد العمليات (CRUD)
     - إضافة ✅ أم لا؟
     - تعديل ✅ أم لا؟
     - حذف ✅ أم لا؟
     - عرض / بحث ✅ أم لا؟

1.3 - تحديد البيانات
     - ما الحقول المطلوبة؟
     - ما الإلزامي منها؟
     - ما نوع كل حقل؟

1.4 - تحديد العلاقات
     - هذه الشاشة تعتمد على أي جداول أخرى؟
     - هل تؤثر على جداول أخرى؟

1.5 - قواعد العمل (Business Rules)
     - ما الشروط والقيود؟
     - مثلاً: لا يمكن حذف عميل عنده فواتير
```

---

### 🟡 المرحلة 2: تصميم قاعدة البيانات (Database Design)

```
2.1 - تحديد الجداول المطلوبة
2.2 - تحديد الحقول ونوع كل حقل
2.3 - تحديد العلاقات (FK, PK)
2.4 - تحديد الـ Indexes المهمة
2.5 - كتابة الـ Entity Class
2.6 - عمل Migration وتطبيقه
```

---

### 🟠 المرحلة 3: طبقة البيانات (DAL)

```
3.1 - Repository Interface
3.2 - Repository Implementation
3.3 - اختبار الـ CRUD يدوياً
```

---

### 🔴 المرحلة 4: طبقة الأعمال (BLL)

```
4.1 - Service Interface
4.2 - Service Implementation
4.3 - Validation Logic
4.4 - Business Rules Implementation
```

---

### 🟢 المرحلة 5: واجهة المستخدم (UI)

```
5.1 - تصميم الـ Form
5.2 - ربط الـ Controls بالبيانات
5.3 - تطبيق الـ Validation على الـ UI
5.4 - معالجة الأخطاء وإظهارها للمستخدم
5.5 - تجربة كل العمليات يدوياً
```

---

### ⚪ المرحلة 6: الاختبار والتدقيق (Testing)

```
6.1 - اختبار الحالات الطبيعية
6.2 - اختبار الحالات الحدية (Edge Cases)
6.3 - اختبار الأخطاء المتوقعة
6.4 - اختبار الأداء مع بيانات كثيرة
```

---

## الـ Prompt الجاهز للوكيل

```markdown
# مهمة: تحليل وتطوير شاشة [اسم الشاشة]

## معلومات المشروع
- النوع: نظام إدارة مبيعات محاسبي
- التقنية: Windows Forms, C#, N-Layer, EF Core, SQL Server

## المطلوب منك الآن: المرحلة 1 - التحليل فقط

### الخطوات:
1. اقرأ الملفات الموجودة في المشروع
2. افهم البنية الحالية للـ Layers
3. حلل شاشة [اسم الشاشة] وأجب على:

**أ. الغرض والمستخدمون:**
- ماذا تفعل هذه الشاشة؟
- من يستخدمها؟

**ب. العمليات المطلوبة:**
- ما عمليات CRUD المطلوبة؟

**ج. البيانات:**
- ما الحقول المطلوبة ونوع كل حقل؟
- ما الحقول الإلزامية؟

**د. العلاقات:**
- ما الجداول المرتبطة؟

**هـ. قواعد العمل:**
- ما الشروط والقيود؟

## قاعدة مهمة:
- لا تكتب أي كود الآن
- فقط حلل وأجب على الأسئلة
- إذا وجدت شيء غير واضح اسألني

## بعد انتهاء التحليل:
- أخبرني بملخص ما فهمته
- انتظر موافقتي قبل الانتقال للمرحلة التالية
```

---

## النقطة الذهبية ⭐

```
كل مرحلة = موافقة منك قبل التقدم للتالية

التحليل ✅ → موافقة → DB Design ✅ → موافقة → DAL ✅ ...
```

**هذا يضمن:**
- لا يوجد كود خاطئ يُبنى على أساس غلط
- أنت دائماً تتحكم في الاتجاه
- النتيجة النهائية تعمل فعلاً في السوق

---

أي شاشة تريد نبدأ بها؟ 🎯


هل مثلا إذا أردت اختبار شاشة معينة اعمل في البداية الواجهة وعاملها mock data لكي اجربها وإذا كان لها كود سابق اعمله كا تعليق comments واجرب تجارب مختلفة من تجارب المستخدم وبعد ان نتأكد من اها جاهزه لكتابة الكود بالكامل وهي سهلة الاستخدام

# نعم! هذا المنهج صحيح جداً ويسمى

## **UI-First Development + Mock Data**

وهو منهج احترافي يستخدمه كبار المطورين

---

## المنهج الكامل المحدّث

```
التحليل → UI + Mock → تجربة المستخدم → موافقة → كود حقيقي
```

---

### 🔵 المرحلة 1: التحليل (كما اتفقنا)
```
- الغرض والمستخدمون
- العمليات المطلوبة
- البيانات والحقول
- قواعد العمل
```

---

### 🟡 المرحلة 2: UI + Mock Data

```
2.1 - تصميم الـ Form فقط (Controls, Layout)

2.2 - Mock Data بدل قاعدة البيانات
─────────────────────────────────
// MOCK DATA - سيُستبدل لاحقاً
var customers = new List<Customer>
{
    new Customer { Id=1, Name="أحمد محمد", Phone="0501234567" },
    new Customer { Id=2, Name="شركة النور", Phone="0559876543" },
};
// END MOCK DATA
─────────────────────────────────

2.3 - الكود القديم يصير تعليق
─────────────────────────────────
// ===== OLD CODE - DISABLED FOR TESTING =====
// var customers = _customerService.GetAll();
// ============================================
─────────────────────────────────
```

---

### 🟠 المرحلة 3: تجربة المستخدم (UX Testing)

```
3.1 - تجربة الحالات الطبيعية
      هل التنقل بين الحقول سهل؟
      هل الأزرار في مكانها الصح؟

3.2 - تجربة الحالات الحدية
      ماذا يحدث إذا الحقل فارغ؟
      ماذا يحدث إذا الرقم سالب؟

3.3 - تجربة سيناريوهات مختلفة
      مستخدم مبتدئ
      مستخدم متقدم
      إدخال بيانات خاطئة

3.4 - جمع الملاحظات
      ما الذي يحتاج تعديل؟
      ما الذي يحتاج إضافة؟
```

---

### 🟢 المرحلة 4: موافقة وانتقال للكود الحقيقي

```
✅ الشاشة مريحة للاستخدام
✅ كل الحالات تم اختبارها
✅ لا توجد ملاحظات معلقة
─────────────────────────────
⬇️ الآن نكتب الكود الحقيقي
─────────────────────────────
- إزالة Mock Data
- تفعيل الكود الحقيقي
- ربط الـ DAL و BLL
- اختبار نهائي مع DB حقيقية
```

---

## الـ Prompt المحدّث للوكيل

```markdown
# مهمة: تطوير شاشة [اسم الشاشة]

## المشروع
- Windows Forms, C#, N-Layer, EF Core, SQL Server

## منهج العمل (اتبعه بالترتيب)

---

### المرحلة 1: التحليل
- اقرأ المشروع وافهم البنية الحالية
- حلل الشاشة وأجب على:
  * الغرض والمستخدمون
  * العمليات المطلوبة (CRUD)
  * الحقول ونوعها وما هو إلزامي
  * العلاقات مع جداول أخرى
  * قواعد العمل والقيود
- انتظر موافقتي قبل المتابعة

---

### المرحلة 2: UI + Mock Data
- صمم الـ Form بالـ Controls المطلوبة
- استخدم Mock Data بدل قاعدة البيانات
- ضع الكود القديم في تعليقات هكذا:
  
  // ===== OLD CODE =====
  // الكود القديم هنا
  // ====================

- ضع Mock Data بتعليق واضح:

  // ===== MOCK DATA - TEMP =====
  // Mock Data هنا
  // ============================

- انتظر موافقتي قبل المتابعة

---

### المرحلة 3: تجربة وتعديل
- اقترح سيناريوهات اختبار مختلفة
- انتظر ملاحظاتي
- عدّل حسب ما أقول
- كرر حتى أوافق

---

### المرحلة 4: الكود الحقيقي
- فقط بعد موافقتي الصريحة
- احذف Mock Data
- فعّل الكود الحقيقي
- اربط DAL و BLL بشكل صحيح

---

## قواعد مهمة:
1. لا تنتقل لمرحلة تالية بدون موافقتي
2. إذا وجدت شيء غير واضح اسألني
3. Mock Data تكون واقعية ومنطقية
4. التعليقات تكون واضحة ومميزة
5. اقترح تحسينات UX إذا رأيت شيئاً

## الشاشة المطلوبة الآن:
[اكتب اسم الشاشة هنا]
```

---

## فوائد هذا المنهج ⭐

```
✅ توفير وقت - تكتشف المشاكل مبكراً
✅ كود نظيف - تكتبه مرة واحدة صح
✅ مستخدم سعيد - الشاشة مريحة قبل الإطلاق
✅ لا مفاجآت - كل شيء مجرّب مسبقاً
✅ سهولة التعديل - Mock أسهل من DB
```

---

هل تريد نطبق هذا المنهج على شاشة معينة الآن؟ 🎯


ماريك في هذه الطريقه ايهما افضل if (testMode)
{
    // mock
}
else
{
    // database
}

# رأيي في هذه الطريقة

## هي أفضل بكثير! ✅

---

## لماذا هي أفضل؟

```
الطريقة القديمة (تعليقات)    vs    الطريقة الجديدة (testMode)
─────────────────────────────────────────────────────────
❌ تعدّل الكود يدوياً          ✅ تغيير قيمة واحدة فقط
❌ خطر نسيان تفعيل كود         ✅ لا خطر نسيان شيء
❌ الكود يبدو فوضوي            ✅ الكود نظيف ومنظم
❌ صعب التتبع                  ✅ واضح ومفهوم
```

---

## الشكل الاحترافي لهذه الطريقة

```csharp
// في مكان واحد فقط في المشروع
public static class AppConfig
{
    public static bool TestMode = true; // ← غيّر هنا فقط
}
```

```csharp
// في الشاشة
private void LoadCustomers()
{
    if (AppConfig.TestMode)
    {
        // Mock Data
        dgvCustomers.DataSource = new List<Customer>
        {
            new Customer { Id=1, Name="أحمد محمد", Phone="0501234567" },
            new Customer { Id=2, Name="شركة النور", Phone="0559876543" },
        };
    }
    else
    {
        // Real Database
        dgvCustomers.DataSource = _customerService.GetAll();
    }
}
```

---

## نقطة أقوى: اجعلها Interface

```csharp
// Interface واحد
public interface ICustomerService
{
    List<Customer> GetAll();
    void Add(Customer customer);
}

// Mock للاختبار
public class MockCustomerService : ICustomerService
{
    public List<Customer> GetAll() => new List<Customer>
    {
        new Customer { Id=1, Name="أحمد محمد" },
        new Customer { Id=2, Name="شركة النور" },
    };

    public void Add(Customer customer)
    {
        MessageBox.Show($"Mock: تم إضافة {customer.Name}");
    }
}

// Real للإنتاج
public class CustomerService : ICustomerService
{
    public List<Customer> GetAll() => _repository.GetAll();
    public void Add(Customer customer) => _repository.Add(customer);
}
```

```csharp
// في الشاشة - سطر واحد يغير كل شيء
ICustomerService _service = AppConfig.TestMode
    ? new MockCustomerService()
    : new CustomerService();
```

---

## المقارنة النهائية

```
testMode Flag       →  جيد ✅
Interface + Mock    →  احترافي ✅✅✅
```

---

## توصيتي

```
للمشاريع الصغيرة   → testMode Flag كافي ✅
لمشروعك هذا        → Interface أفضل
لأن:
- المشروع سيكبر
- شاشات كثيرة ستحتاج نفس الفكرة
- التبديل يصير في سطر واحد
- الكود يصير قابل للاختبار دائماً
```

---

هل تريد أكتب لك **Template جاهز** بهذا النظام تطبقه على كل شاشة في مشروعك؟ 🎯


انا اريد ان اوحد في مشروعي الحالي  طريقه الوتقنيات والهيكليه انا عندي في مشروعي إدراة البيعات المحاسبي يفحص الـAgent الكود الموجود، Forms المرتبطة، Business Layer، Entities، EF Core، SQL Server والجداول ذات العلاقة. في هذه المرحلة لا يعدّل شيئًا. وهذا المشروع سوف نظام الاستضافه على جهاز الحاسوب سوف تكون قاعدة البينات في نفس الجهاز هذه اللمحات الاوليه للمشروع حالين والنظام المكتبي هو فيه تعقيد اقل من الوب والمبايل والاحتياج الاول في السوف المحلي عندي في اليمن طلبو مكتبي إنشاء الله إذا كان هناك طلب للمبايل والوب سوف يكون نظام شبه كامل لن ابداء من الصفر

# فهمت رؤيتك كاملاً! 🎯

## ملخص ما قلته

```
✅ نظام مكتبي Windows Forms الآن
✅ قاعدة بيانات محلية على نفس الجهاز
✅ السوق المحلي اليمن يطلب مكتبي
✅ مستقبلاً: موبايل وويب بدون بداية من صفر
✅ الأولوية الآن: توحيد البنية والتقنيات
```

---

## هذا يعني بنية ذكية من البداية

```
الآن                    المستقبل
─────────────────────────────────────
Windows Forms     →     + Web API
Local SQL Server  →     + Cloud DB
N-Layer           →     نفس الـ Layer
Business Logic    →     نفس الـ BLL
Entities          →     نفس الـ Entities
```

**الفكرة الذهبية:**
```
BLL + DAL + Entities = مشترك للكل
UI فقط هي التي تتغير
```

---

## الـ Prompt للوكيل (مرحلة الفحص فقط)

```markdown
# مهمة: فحص وتحليل المشروع - لا تعدّل أي شيء

## معلومات المشروع
- النوع: نظام إدارة مبيعات محاسبي مكتبي
- التقنية: Windows Forms, C#, N-Layer Architecture
- ORM: Entity Framework Core
- Database: SQL Server (محلي على نفس الجهاز)
- المستقبل: نفس النظام قد يمتد لـ Web و Mobile

## المطلوب منك: فحص وتوثيق فقط

### 1. فحص الـ Solution Structure
- كم عدد المشاريع في الـ Solution؟
- ما اسم كل مشروع ودوره؟
- هل البنية تطبق N-Layer بشكل صحيح؟

### 2. فحص الـ Entities
- ما الـ Entities الموجودة؟
- هل هي منفصلة في مشروع مستقل؟
- هل تطبق conventions موحدة؟
  * أسماء الخصائص
  * أنواع البيانات
  * العلاقات

### 3. فحص EF Core
- هل يوجد DbContext واحد أم أكثر؟
- كيف يتم تسجيل الـ DbContext؟
- هل الـ Migrations منظمة؟
- هل Connection String في مكان موحد؟

### 4. فحص Business Layer
- كيف منظمة الـ Services؟
- هل هناك Interfaces للـ Services؟
- هل الـ Validation موحد؟
- هل هناك Error Handling موحد؟

### 5. فحص Data Access Layer
- هل هناك Generic Repository؟
- هل كل Entity عندها Repository منفصل؟
- كيف تتم إدارة الـ Transactions؟

### 6. فحص الـ Forms
- كم عدد الـ Forms الموجودة؟
- هل هناك Base Form مشترك؟
- كيف يتم استدعاء الـ Services من الـ Forms؟
- هل هناك نمط موحد للـ CRUD في الـ Forms؟

### 7. فحص التسميات والـ Conventions
- هل أسماء الملفات موحدة؟
- هل أسماء المتغيرات موحدة؟
- هل التعليقات موجودة وموحدة؟

### 8. فحص الـ SQL Server
- ما اسم قاعدة البيانات؟
- ما الجداول الموجودة؟
- هل هناك Stored Procedures؟
- هل الـ Indexes موجودة على الحقول المهمة؟

---

## النتيجة المطلوبة منك

بعد الفحص أعطني تقرير بهذا الشكل:

### ✅ ما هو موجود ويعمل بشكل صحيح:
[قائمة]

### ⚠️ ما هو موجود لكن يحتاج توحيد:
[قائمة مع سبب]

### ❌ ما هو مفقود ويجب إضافته:
[قائمة مع سبب]

### 🔮 ملاحظات للمستقبل (Web/Mobile):
[ما الذي يجب مراعاته الآن لتسهيل التوسع لاحقاً]

---

## قواعد مهمة:
1. لا تعدّل أي ملف أو كود
2. فقط اقرأ وحلل ووثّق
3. إذا وجدت شيء غير واضح اذكره في التقرير
4. انتظر موافقتي بعد التقرير قبل أي خطوة تالية
```

---

## بعد تقرير الوكيل

```
تقرير الوكيل
     ↓
نراجعه سوياً
     ↓
نحدد أولويات التوحيد
     ↓
نكتب معايير موحدة للمشروع
     ↓
نطبقها شاشة شاشة
```

---

هل تريد تشغّل هذا الـ Prompt الآن على مشروعك وترسل لي التقرير؟ 🎯


انا عندي توضيح اكثر windows forms UI -> BLL مباشرتا انا لا احتاج ِAPI Layer for desktop app عندما احتجه في المستقبل من اجل الوب او الموبيل سوف ابني UI with add only API layer on BLL + DAL ماريك وفي حلتنا برنامج المكتبي سوف يكون اسرع وادائه افضل وتطويره اسرع


# رأيي: هذا صحيح 100% ✅

## أنت فاهم البنية صح

```
Desktop (الآن)
─────────────────────────────
Windows Forms
      ↓ مباشر
    BLL
      ↓
    DAL
      ↓
 SQL Server
─────────────────────────────
✅ أسرع
✅ أبسط
✅ أداء أفضل
✅ تطوير أسرع
✅ لا تعقيد زائد
```

```
Web / Mobile (المستقبل)
─────────────────────────────
React / Flutter / etc
      ↓
   API Layer  ← تضيفه فقط هنا
      ↓
    BLL       ← نفس الكود
      ↓
    DAL       ← نفس الكود
      ↓
 SQL Server   ← نفس القاعدة
─────────────────────────────
✅ لا تبدأ من صفر
✅ BLL و DAL جاهزين
✅ فقط API Layer جديد
```

---

## الشرط الوحيد لنجاح هذه الخطة

```
BLL يجب أن يكون مستقل تماماً
يعني:
```

```csharp
// ✅ صح - BLL لا يعرف شيء عن Forms
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public List<Customer> GetAll() => _repo.GetAll();
    public bool Add(Customer customer)
    {
        // Business Logic هنا فقط
    }
}
```

```csharp
// ❌ غلط - BLL فيه أي شيء يخص UI
public class CustomerService
{
    public void Add(Customer customer)
    {
        MessageBox.Show("تم الإضافة"); // ← هذا يكسر الاستقلالية
    }
}
```

---

## المعيار الذهبي

```
إذا أخذت BLL و DAL
ووضعتهم في مشروع API غداً
يجب أن يشتغلوا بدون أي تعديل
```

---

## إذن الـ Prompt للوكيل يركز على هذا

```markdown
## نقطة مهمة في فحص BLL:

تحقق أن BLL مستقل تماماً:
- لا يوجد أي reference لـ Windows Forms
- لا يوجد MessageBox أو Any UI Element
- كل الـ Services تعمل عبر Interfaces
- الـ Validation يرجع نتيجة (true/false أو Result)
  ولا يتعامل مع UI مباشرة

هذا يضمن إمكانية استخدام نفس BLL
مع API Layer في المستقبل بدون تعديل
```

---

## ملخص رؤيتك النهائية

```
✅ Desktop  = Forms → BLL → DAL → DB
✅ Future   = API Layer فقط يُضاف فوق BLL
✅ BLL + DAL مشتركين للكل
✅ قرار ذكي ومدروس
```

هل نضيف هذا التوضيح للـ Prompt الرئيسي ونشغله على المشروع؟ 🎯

# AI Context Strategy & Architecture Summary

*Updated: 2026-09-14*

## Current Active Architecture

```text
Desktop (WinForms) → BLL (Business Services) → DAL (EF Core) → SQL Server
```

**Key Points:**
- Desktop connects **directly** to BLL — NO API layer for the desktop app
- BLL is completely independent of UI (no `MessageBox`, no `System.Windows.Forms` references)
- This makes BLL + DAL reusable for future Web/Mobile with only an API layer addition

```text
Desktop (Now)                    Web/Mobile (Future)
─────────────────                ─────────────────────
Windows Forms                    React / Flutter / etc
      ↓ direct                         ↓
    BLL  ← same code →           API Layer (NEW)
      ↓                               ↓
    DAL  ← same code →           BLL ← same code
      ↓                               ↓
 SQL Server ← same DB →          DAL ← same code
                                       ↓
                                  SQL Server ← same DB
```

## Context Strategy
The AI context is strictly managed to ensure high performance and low cost:
1. **Single Source of Truth**: `AGENTS.md` acts as the master routing file and contains all architecture rules for the WinForms N-Layer system.
2. **Lean Agents**: Agent files (located in `.opencode/agent/`) are strictly specialized and trimmed to <15KB each to save on token context.
3. **No Redundancy**: Rules are stated once in `AGENTS.md` or `CONSTITUTION.md` rather than duplicated across agents.

## Cost-Optimized Model Assignment (OpenCode Go)
Models are explicitly assigned to agents via frontmatter based on task complexity:

### Tier 1: Deep Thinking (Architectural Decisions)
- **Model**: `opencode/deepseek-v4-pro` ($15/mo)
- **Agents**: Orchestrator, Backend Architect

### Tier 2: Code Generation (Implementation)
- **Model**: `opencode/mimo-v2.5-pro` ($15/mo)
- **Agents**: Implement Agent, Database Engineer

### Tier 3: Fast Tasks (UI, Reviews, Tests)
- **Model**: `opencode/deepseek-v4-flash` / `opencode/qwen3.8-flash` ($30/mo)
- **Agents**: UI Agent, Code Reviewer, Security Auditor, Test Engineer, Printing

### Tier 4: Ultra-Cheap Bulk (Simple Fixes)
- **Model**: `opencode/deepseek-v4.1-flash` ($15/mo)
- **Agents**: Fast Agent

*Note: All redundant planning and specification agents were deleted to optimize workflow efficiency.*