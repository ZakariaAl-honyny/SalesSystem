'use client';

import React, { useState } from 'react';
import { 
  AlertCircle, 
  AlertTriangle, 
  HelpCircle, 
  CheckCircle2, 
  Info, 
  ArrowRight, 
  Play, 
  Check, 
  Layers, 
  Sparkles,
  Barcode,
  Users,
  Calculator,
  Scale,
  X,
  FileCheck
} from 'lucide-react';
import { useWinFormsDialog } from '@/context/WinFormsDialogContext';

interface WinFormsShowDialogShowcaseProps {
  isOpen: boolean;
  onClose: () => void;
  currency: string;
}

export const WinFormsShowDialogShowcase: React.FC<WinFormsShowDialogShowcaseProps> = ({
  isOpen,
  onClose,
  currency,
}) => {
  const { showMessageBox, showError, showSuccess, showWarning, showConfirm } = useWinFormsDialog();
  const [lastActionResult, setLastActionResult] = useState<string | null>(null);

  // Test state inputs within the showcase window
  const [testBarcode, setTestBarcode] = useState('99999999');
  const [testCustomerCredit, setTestCustomerCredit] = useState(150000);
  const [testCreditLimit, setTestCreditLimit] = useState(100000);
  const [testQuantity, setTestQuantity] = useState(-5);
  const [testDebit, setTestDebit] = useState(50000);
  const [testCredit, setTestCredit] = useState(45000);

  if (!isOpen) return null;

  // Scenario 1: Barcode Not Found
  const handleTestBarcodeError = async () => {
    setLastActionResult('جاري فتح Dialog خطأ الباركود...');
    const result = await showError(
      `لم يتم العثور على أي صنف بالباركود المدخل: [ ${testBarcode} ] في قاعدة البيانات!\nيرجى التأكد من مسح الباركود بشكل سليم أو الضغط على [F3] للبحث المتقدم.`,
      'خطأ في قراءة الباركود (Barcode Not Found)',
      'showcase-barcode-input',
      `// WinForms C# Error Handler:
try {
    var product = _productService.GetByBarcode("${testBarcode}");
    if (product == null) {
        MessageBox.Show("لم يتم العثور على الصنف بالباركود!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        txtBarcode.Focus();
        txtBarcode.SelectAll();
        errorProvider.SetError(txtBarcode, "الباركود غير معرف");
    }
} catch (Exception ex) { ... }`
    );
    setLastActionResult(`تم إغلاق الـ Dialog بنتيجة [${result}] والتركيز فوراً مع اهتزاز حقل الباركود (#showcase-barcode-input)`);
  };

  // Scenario 2: Credit Limit Exceeded
  const handleTestCreditLimitError = async () => {
    setLastActionResult('جاري فحص سقف الائتمان...');
    const diff = testCustomerCredit - testCreditLimit;
    const result = await showError(
      `تنبيه رقابة الائتمان: رصيد العميل الحالي (${testCustomerCredit.toLocaleString()} ${currency}) يتجاوز سقف الائتمان المسموح به (${testCreditLimit.toLocaleString()} ${currency}) بمقدار (${diff.toLocaleString()} ${currency})!\nلا يمكن ترحيل الفاتورة كـ "آجل" إلا بعد الحصول على موافقة المدير المالي أو سداد دفعة نقدية.`,
      'تجاوز سقف الائتمان المالي (Credit Limit Exceeded)',
      'showcase-customer-input',
      `// C# BLL Validation Rule:
if (customer.CurrentBalance + newInvoiceAmount > customer.CreditLimit)
{
    MessageBox.Show("تجاوز سقف الائتمان!", "حظر بيع آجل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    cboCustomers.Focus();
    errorProvider.SetError(cboCustomers, "تجاوز السقف الائتماني");
    return false;
}`
    );
    setLastActionResult(`تم إغلاق الـ Dialog بنتيجة [${result}] والتركيز على حقل العميل (#showcase-customer-input)`);
  };

  // Scenario 3: Negative / Zero Quantity
  const handleTestQuantityError = async () => {
    setLastActionResult('جاري التحقق من كمية الصنف...');
    const result = await showError(
      `قيمة الكمية المدخلة [ ${testQuantity} ] غير مقبولة!\nالكمية في فواتير المبيعات يجب أن تكون رقماً موجباً أكبر من صفر (Quantity > 0).`,
      'خطأ في كمية الصنف (Invalid Quantity)',
      'showcase-quantity-input',
      `// DataGridView Cell Validating:
if (Convert.ToDecimal(row.Cells["colQty"].Value) <= 0)
{
    MessageBox.Show("يجب أن تكون الكمية أكبر من صفر!", "خطأ مدخلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
    gridSales.CurrentCell = row.Cells["colQty"];
    gridSales.BeginEdit(true);
}`
    );
    setLastActionResult(`تم إغلاق الـ Dialog بنتيجة [${result}] والتركيز على خانة الكمية (#showcase-quantity-input)`);
  };

  // Scenario 4: Unbalanced Journal Entry
  const handleTestUnbalancedJournal = async () => {
    setLastActionResult('جاري التحقق من توازن القيد المحاسبي...');
    const diff = Math.abs(testDebit - testCredit);
    const result = await showError(
      `فشل ترحيل القيد المحاسبي: القيد غير متوازن!\nإجمالي المدين: ${testDebit.toLocaleString()} ${currency}\nإجمالي الدائن: ${testCredit.toLocaleString()} ${currency}\nفارق عدم التوازن: ${diff.toLocaleString()} ${currency}.\nلا يسمح النظام المحاسبي بترحيل أي قيد غير متوازن إلى الدفتر العام.`,
      'عدم توازن القيد المحاسبي (Unbalanced Journal Entry)',
      'showcase-journal-diff-input',
      `// Accounting General Ledger Rule:
if (journalEntry.TotalDebit != journalEntry.TotalCredit)
{
    var diff = Math.Abs(journalEntry.TotalDebit - journalEntry.TotalCredit);
    MessageBox.Show($"القيد غير متوازن بفارق: {diff} YER!", "خطأ محاسبي", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    txtDifference.Focus();
    txtDifference.BackColor = Color.LightPink;
    return false;
}`
    );
    setLastActionResult(`تم إغلاق الـ Dialog بنتيجة [${result}] وتوجيه المؤشر لحقل فارق القيد (#showcase-journal-diff-input)`);
  };

  // Scenario 5: Confirmation Dialog (Yes/No with Enter/Esc)
  const handleTestConfirmation = async () => {
    setLastActionResult('جاري طلب تأكيد المستخدم...');
    const confirmed = await showConfirm(
      `هل أنت متأكد من رغبتك في إقفال الوردية الحالية وترحيل كشف المقبوضات النقدية وتسليم النقدية للخزينة الرئيسية؟\nلن تتمكن من إضافة عمليات بيع جديدة على هذه الوردية بعد الإقفال.`,
      'تأكيد إقفال الوردية وتسليم الصندوق (Close Shift Confirmation)'
    );
    
    if (confirmed) {
      await showSuccess(
        `تم إقفال الوردية بنجاح وترحيل سند تسليم الخزينة رقم [ #3004 ] وإصدار تقرير Z-Report المحاسبي.`,
        'تم إقفال الوردية بنجاح (Shift Closed)',
        'showcase-status-badge'
      );
      setLastActionResult('أكد المستخدم العملية (نعم / Yes) وتم إصدار رسالة النجاح والترحيل بنجاح!');
    } else {
      await showWarning(
        `تم إلغاء عملية إقفال الوردية بناءً على طلب المستخدم. استمر العمل بالوردية الحالية.`,
        'إلغاء الإقفال (Cancelled)'
      );
      setLastActionResult('رفض المستخدم العملية (لا / No / Esc)');
    }
  };

  // Scenario 6: Success Dialog
  const handleTestSuccess = async () => {
    const invNo = 1008;
    const jvNo = 108;
    await showSuccess(
      `تم حفظ وترحيل فاتورة المبيعات رقم [ #${invNo} ] بنجاح!\n• تم خصم الكميات من مستودع (المستودع الرئيسي WH-01) بنظام التكلفة FIFO.\n• تم توليد القيد المحاسبي المتوازن رقم [ #${jvNo} ] في الدفتر العام.\n• تم تحديث حركة الصندوق والسيولة النقدية.`,
      'نجاح الحفظ والترحيل المحاسبي (Posted Successfully)',
      'showcase-barcode-input'
    );
    setLastActionResult(`تم عرض رسالة نجاح العملية وتمت إعادة المؤشر تلقائياً لحقل الباركود لاستقبال الفاتورة التالية!`);
  };

  return (
    <div className="fixed inset-0 z-[9990] bg-black/60 backdrop-blur-xs flex items-center justify-center p-3 select-none animate-in fade-in duration-100">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-4xl max-h-[92vh] flex flex-col overflow-hidden animate-in zoom-in-95 duration-150">
        {/* Title Bar */}
        <div className="bg-[#0f2d4a] text-white px-4 py-3 flex items-center justify-between shrink-0">
          <div className="flex items-center gap-2.5">
            <div className="p-1 bg-sky-500/20 rounded border border-sky-400/30">
              <Sparkles className="w-5 h-5 text-sky-400" />
            </div>
            <div>
              <h2 className="font-bold text-sm sm:text-base">
                محاكي نوافذ WinForms الحوارية والتوجيه للخطأ (MessageBox & ErrorProvider Showcase)
              </h2>
              <p className="text-[11px] text-sky-200">
                مطابقة كاملة لمعمارية الأنظمة المكتبية C# WinForms: استخدام ShowDialog، التأكيد، والتركيز الآلي مع الاهتزاز على مكان الخطأ
              </p>
            </div>
          </div>
          <button
            onClick={onClose}
            className="text-slate-300 hover:text-white hover:bg-white/10 p-1.5 rounded transition cursor-pointer"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Content Body */}
        <div className="flex-1 overflow-y-auto p-4 sm:p-6 space-y-6 bg-slate-50">
          {/* Explanation Banner */}
          <div className="bg-sky-50 border border-sky-200 rounded-lg p-3.5 flex items-start gap-3">
            <Info className="w-5 h-5 text-sky-700 shrink-0 mt-0.5" />
            <div className="text-xs text-sky-900 leading-relaxed">
              <strong className="block font-bold text-sm text-sky-950 mb-1">
                كيف تعمل رسائل ShowDialog والـ ErrorProvider في التطبيقات المكتبية:
              </strong>
              عند حدوث أي خطأ في الإدخال أو الرقابة المحاسبية، يتم استدعاء نافذة حوارية مشروطة <code className="bg-sky-200/70 px-1.5 py-0.5 rounded text-sky-950 font-bold font-mono">MessageBox.Show()</code> توقف التفاعل مع الخلفية، وعندما يضغط المستخدم على <strong>[موافق / Enter]</strong>، يقوم النظام تلقائياً بتوجيه المؤشر <code className="bg-sky-200/70 px-1.5 py-0.5 rounded text-sky-950 font-bold font-mono">Control.Focus()</code> وتحديد النص <code className="bg-sky-200/70 px-1.5 py-0.5 rounded text-sky-950 font-bold font-mono">TextBox.SelectAll()</code> وإظهار وميض أحمر واهتزاز على خانة الخطأ بالضبط!
            </div>
          </div>

          {/* Interactive Live Testing Grid */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {/* Test Card 1: Barcode Search Error */}
            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-rose-700 font-bold text-xs">
                  <Barcode className="w-4 h-4" />
                  <span>1. خطأ عدم وجود الباركود (Barcode Error)</span>
                </div>
                <span className="text-[10px] bg-rose-50 text-rose-700 border border-rose-200 px-1.5 py-0.5 rounded font-mono">
                  MessageBoxIcon.Error
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                عند إدخال باركود غير مسجل، تظهر رسالة خطأ ثم يتم توجيه المؤشر فوراً لخانة الباركود وتحديد النص.
              </p>
              <div className="space-y-1.5">
                <label className="text-[11px] font-bold text-slate-700">حقل الباركود التجريبي:</label>
                <input
                  id="showcase-barcode-input"
                  type="text"
                  value={testBarcode}
                  onChange={(e) => setTestBarcode(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2.5 py-1.5 text-xs font-mono font-bold text-slate-900 focus:outline-hidden focus:border-sky-500"
                />
              </div>
              <button
                type="button"
                onClick={handleTestBarcodeError}
                className="w-full bg-rose-600 hover:bg-rose-700 text-white text-xs font-bold py-2 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <Play className="w-3.5 h-3.5" />
                <span>تشغيل تجربة خطأ الباركود والتركيز</span>
              </button>
            </div>

            {/* Test Card 2: Credit Limit Exceeded */}
            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-amber-700 font-bold text-xs">
                  <Users className="w-4 h-4" />
                  <span>2. تجاوز سقف الائتمان (Credit Limit Error)</span>
                </div>
                <span className="text-[10px] bg-amber-50 text-amber-700 border border-amber-200 px-1.5 py-0.5 rounded font-mono">
                  MessageBoxIcon.Warning
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                إذا تجاوز رصيد العميل سقف الائتمان، يمنع البيع الآجل ويتم توجيه المؤشر لقائمة اختيار العميل.
              </p>
              <div className="grid grid-cols-2 gap-2">
                <div>
                  <label className="text-[10px] font-bold text-slate-600">الرصيد الحالي:</label>
                  <input
                    type="number"
                    value={testCustomerCredit}
                    onChange={(e) => setTestCustomerCredit(parseFloat(e.target.value) || 0)}
                    className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-xs font-mono text-slate-800"
                  />
                </div>
                <div>
                  <label className="text-[10px] font-bold text-slate-600">سقف الائتمان:</label>
                  <input
                    type="number"
                    value={testCreditLimit}
                    onChange={(e) => setTestCreditLimit(parseFloat(e.target.value) || 0)}
                    className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-xs font-mono text-slate-800"
                  />
                </div>
              </div>
              <div className="space-y-1.5">
                <label className="text-[11px] font-bold text-slate-700">حقل اختيار العميل المستهدف:</label>
                <input
                  id="showcase-customer-input"
                  type="text"
                  readOnly
                  value="شركة الأمل للتجارة - CUST-002"
                  className="w-full bg-slate-100 border border-slate-300 rounded px-2.5 py-1.5 text-xs font-bold text-slate-900 cursor-pointer"
                />
              </div>
              <button
                type="button"
                onClick={handleTestCreditLimitError}
                className="w-full bg-amber-600 hover:bg-amber-700 text-white text-xs font-bold py-2 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <Play className="w-3.5 h-3.5" />
                <span>تشغيل تجربة فحص سقف الائتمان</span>
              </button>
            </div>

            {/* Test Card 3: Invalid Quantity */}
            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-rose-700 font-bold text-xs">
                  <Calculator className="w-4 h-4" />
                  <span>3. خطأ كمية سالبة أو صفرية (Invalid Quantity)</span>
                </div>
                <span className="text-[10px] bg-rose-50 text-rose-700 border border-rose-200 px-1.5 py-0.5 rounded font-mono">
                  Input Validation
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                منع القيم السالبة في جدول المبيعات وتوجيه المؤشر مباشرة إلى خلية الكمية في الجدول.
              </p>
              <div className="space-y-1.5">
                <label className="text-[11px] font-bold text-slate-700">قيمة الكمية التجريبية:</label>
                <input
                  id="showcase-quantity-input"
                  type="number"
                  value={testQuantity}
                  onChange={(e) => setTestQuantity(parseFloat(e.target.value) || 0)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2.5 py-1.5 text-xs font-mono font-bold text-rose-700 focus:outline-hidden focus:border-rose-500"
                />
              </div>
              <button
                type="button"
                onClick={handleTestQuantityError}
                className="w-full bg-rose-600 hover:bg-rose-700 text-white text-xs font-bold py-2 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <Play className="w-3.5 h-3.5" />
                <span>تشغيل تجربة خطأ الكمية</span>
              </button>
            </div>

            {/* Test Card 4: Unbalanced Journal Entry */}
            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-indigo-700 font-bold text-xs">
                  <Scale className="w-4 h-4" />
                  <span>4. عدم توازن القيد المحاسبي (GL Imbalance)</span>
                </div>
                <span className="text-[10px] bg-indigo-50 text-indigo-700 border border-indigo-200 px-1.5 py-0.5 rounded font-mono">
                  Debit ≠ Credit
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                التحقق الصارم من توازن القيد قبل الترحيل وتوجيه المؤشر لخانة فارق القيد.
              </p>
              <div className="grid grid-cols-2 gap-2">
                <div>
                  <label className="text-[10px] font-bold text-slate-600">المدين:</label>
                  <input
                    type="number"
                    value={testDebit}
                    onChange={(e) => setTestDebit(parseFloat(e.target.value) || 0)}
                    className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-xs font-mono text-slate-800"
                  />
                </div>
                <div>
                  <label className="text-[10px] font-bold text-slate-600">الدائن:</label>
                  <input
                    type="number"
                    value={testCredit}
                    onChange={(e) => setTestCredit(parseFloat(e.target.value) || 0)}
                    className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-xs font-mono text-slate-800"
                  />
                </div>
              </div>
              <div className="space-y-1.5">
                <label className="text-[11px] font-bold text-slate-700">حقل فارق عدم التوازن المستهدف:</label>
                <input
                  id="showcase-journal-diff-input"
                  type="text"
                  readOnly
                  value={`فارق القيد: ${Math.abs(testDebit - testCredit).toLocaleString()} ${currency}`}
                  className="w-full bg-rose-50 border border-rose-300 rounded px-2.5 py-1.5 text-xs font-bold text-rose-800 cursor-pointer"
                />
              </div>
              <button
                type="button"
                onClick={handleTestUnbalancedJournal}
                className="w-full bg-indigo-700 hover:bg-indigo-800 text-white text-xs font-bold py-2 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <Play className="w-3.5 h-3.5" />
                <span>تشغيل تجربة عدم توازن القيد</span>
              </button>
            </div>
          </div>

          {/* Test Card 5 & 6: Confirmations & Success Dialogs */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-sky-700 font-bold text-xs">
                  <HelpCircle className="w-4 h-4" />
                  <span>5. نافذة التأكيد (ShowConfirm - Yes/No)</span>
                </div>
                <span className="text-[10px] bg-sky-50 text-sky-700 border border-sky-200 px-1.5 py-0.5 rounded font-mono">
                  MessageBoxButtons.YesNo
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                اختبار رسائل التأكيد مع دعم لوحة المفاتيح: زر Enter للموافقة و Esc للإلغاء أو النقر بالماوس.
              </p>
              <button
                type="button"
                onClick={handleTestConfirmation}
                className="w-full bg-sky-700 hover:bg-sky-800 text-white text-xs font-bold py-2.5 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <HelpCircle className="w-4 h-4" />
                <span>تشغيل تجربة نافذة التأكيد (Yes / No)</span>
              </button>
            </div>

            <div className="bg-white rounded-lg border border-slate-200 p-4 shadow-2xs space-y-3">
              <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                <div className="flex items-center gap-2 text-emerald-700 font-bold text-xs">
                  <FileCheck className="w-4 h-4" />
                  <span>6. رسالة نجاح العملية والترحيل (Success Dialog)</span>
                </div>
                <span className="text-[10px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-1.5 py-0.5 rounded font-mono">
                  MessageBoxIcon.Information
                </span>
              </div>
              <p className="text-[11px] text-slate-500">
                عرض رسالة نجاح العملية مع أرقام المستندات المتولدة وإعادة المؤشر للبدء بالعملية التالية.
              </p>
              <button
                type="button"
                onClick={handleTestSuccess}
                className="w-full bg-emerald-700 hover:bg-emerald-800 text-white text-xs font-bold py-2.5 rounded-md transition flex items-center justify-center gap-2 shadow-xs cursor-pointer"
              >
                <CheckCircle2 className="w-4 h-4" />
                <span>تشغيل تجربة رسالة النجاح والترحيل</span>
              </button>
            </div>
          </div>

          {/* Last Result Box */}
          {lastActionResult && (
            <div 
              id="showcase-status-badge"
              className="p-3 bg-slate-900 text-emerald-300 rounded-lg text-xs font-mono flex items-center gap-2 border border-slate-700 animate-in fade-in"
            >
              <div className="w-2 h-2 rounded-full bg-emerald-400 animate-ping" />
              <span>آخر إجراء: {lastActionResult}</span>
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="bg-slate-100 border-t border-slate-300 px-4 py-3 flex items-center justify-between">
          <div className="text-xs text-slate-500 flex items-center gap-1.5">
            <span className="font-bold text-slate-700">WinForms Modal Dialog Engine:</span>
            <span>يدعم أصوات النظام، اختصارات لوحة المفاتيح، والـ ErrorProvider</span>
          </div>
          <button
            onClick={onClose}
            className="px-5 py-1.5 bg-[#0f2d4a] hover:bg-[#143d63] text-white rounded-lg text-xs font-bold transition shadow-xs cursor-pointer"
          >
            إغلاق المحاكي
          </button>
        </div>
      </div>
    </div>
  );
};
