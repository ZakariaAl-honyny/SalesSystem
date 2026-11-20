# F-001 — تغذية الحقل المطلوب + تنقل `Enter`

| الحقل | القيمة |
|---|---|
| `ID` | `F-001` |
| `الحجم` | L (شاشة تجريبية + تعميم على شاشات كثيرة) |
| `المرحلة` | `—` (تحسين سلوك واجهة، بلا مسار بيانات) |
| `الحالة` | 🔄 **بانتظار شهادة المالك** — الشاشة التجريبية `frmCRUDCProductMaster` |
| `الوكيل المالك` | `ui-agent` |
| `أثر قاعدة البيانات` | **لا شيء** (فعّال من أول يوم) |
| `أول جولة` | `2026-11-16` |

## ما تفعله هذه الميزة

حقل مطلوب فارغ يُبلَّغ عنه **في اللحظة** (أيقونة ✗ + رسالة + ومضة قصيرة تعود إلى لون المصمّم) ولا يُغادَر بـ`Enter`. و`Enter` داخل نطاق المحرر يمشي **بترتيب العرض** (صفاً صفاً، من اليمين لليسار) ثم يسلّم التركيز إلى زر الوضع **بلا تنفيذ**.

## ⚠️ لا تكسر هذه (اقرأها قبل أي تعديل)

1. **`Tab` والماوس لا يُحجبان** — الحجب لـ`Enter` وحده (`RULE-N194`). أي «تحسين» يحجب `Tab` يُرفض.
2. **`RequiredFieldHelper` لا يملك أي لون** — ولا يكتب `BackColor`. الومضة والاستعادة ملك `clsInputFeedback`، والمفتاح ملك `ThemePalette.RejectBackgroundColor` (`RULE-N181`/`RULE-N182`).
3. **`BlocksEnterNavigation` لا يُستهلك من أي شاشة** — يُستهلك مرة واحدة داخل `KeyboardHelper` (`grep` = 0 في `Screens/**`، مقيس أدناه).
4. **`AcceptButton` لا يُفرَّغ** كحل بديل — الإعلان مفتاح إدخال في `PreviewKeyDown` هو الحل المعتمد (`RULE-N193`).
5. **`NavigationOptions` لا تقرؤها ولا تضبطها أي شاشة** — تُقرأ حيّة داخل `KeyboardHelper` فقط (`RULE-N192`).
6. **الشاشة الوحيدة المسموح مسّها الآن هي `frmCRUDCProductMaster`** — التعميم ممنوع قبل الشهادة (`RULE-N184`).

## أين يعيش كودها؟ (بيوت واحدة)

| الملف | الدور |
|---|---|
| `Desktop/Helpers/RequiredFieldHelper.cs` | `Attach` · `Validate` · `Clear` · `FirstMissing` · `BlocksEnterNavigation` |
| `Desktop/Helpers/KeyboardHelper.cs` | `WireEnterAsTab(scope, primaryButton)` · إعلان `Enter` مفتاحَ إدخال · استهلاك `BlocksEnterNavigation` |
| `Desktop/Helpers/NavigationOptions.cs` | `EnterFollowsDisplayOrder` · `EnterExitsToModeButton` · `RowBandTolerancePixels` |
| `Desktop/Helpers/ThemePalette.cs` | `RejectBackgroundColor` · `SelectionRowColor` |
| `Desktop/Global Classes/clsInputFeedback.cs` | `ShowRejectTooltip` (الومضة + الاستعادة + الصوت) |
| `Screens/Products/Forms/frmCRUDCProductMaster.cs` | الشاشة التجريبية — تُعلن قواعدها فقط |

## الحالة الآن

الشاشة التجريبية **تعمل ومبنية**، لكنها **غير مشهودة من المالك** ⇒ صفر تعميم. القياس الحالي: 28 ملفاً تحمل 188 موضع `SetError` لم يُغذَّ منها إلا عبر الشاشة التجريبية.
