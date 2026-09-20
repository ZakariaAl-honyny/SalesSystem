const fs = require('fs');

const advStr = `
        <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-3 backdrop-blur-xs">
          <div className="bg-white rounded-lg shadow-2xl border border-slate-400 w-full max-w-lg overflow-hidden flex flex-col animate-in fade-in zoom-in-95 duration-150">
            <div className="p-3 bg-[#1e293b] text-white flex items-center justify-between shrink-0">
              <div className="flex items-center gap-2 font-bold text-sm">
                <Info className="w-4 h-4 text-amber-400" />
                <span>تحليل العهد المالية وسلف الموظفين (MVP Accounting Note)</span>
              </div>
              <button
                onClick={() => setShowAdvancesInfoModal(false)}
                className="text-white/80 hover:text-white p-1 hover:bg-white/10 rounded cursor-pointer"
              >
                <X className="w-4 h-4" />
              </button>
            </div>
            <div className="p-4 space-y-3.5 text-xs leading-relaxed text-slate-700">
              <div className="p-3 bg-amber-50 border border-amber-200 rounded text-amber-950 space-y-1.5">
                <div className="font-bold flex items-center gap-1 text-amber-900">
                  <span>آلية المعالجة المحاسبية للعهد والسلف في النسخة الحالية (MVP):</span>
                </div>
                <p className="text-[11px]">
                  في معايير المحاسبة، تندرج عهد وسلف الموظفين تحت حسابات الأصول المتداولة (كود <b>115 - سلف وعهد الموظفين</b>)، وليست صناديق نقدية (111) لأنها مبالغ في ذمة الموظف لحين تسويتها بمستندات وفواتير رسمية.
                </p>
              </div>
              <div className="space-y-2 border border-slate-200 p-3 rounded bg-slate-50 text-[11px]">
                <div className="font-bold text-slate-900">كيف يتعامل النظام معها حالياً وبدقة محاسبية؟</div>
                <ul className="list-disc pr-4 space-y-1 text-slate-700">
                  <li>
                    <b>تسليم العهدة:</b> يتم إصدار <b>سند صرف نقدية</b>، بحيث يكون الحساب المحاسبي هو حساب الصندوق (111)، والطرف المقابل حساب سلف الموظفين (115) أو المصروف المباشر.
                  </li>
                  <li>
                    <b>تسوية العهدة:</b> من خلال شاشة <b>قيود اليومية العامة</b>، يتم إثبات المصروفات بموجب الفواتير وإقفال أو تخفيض حساب العهدة.
                  </li>
                  <li>
                    <b>شاشات الصناديق والبنوك:</b> تم قصرها حصراً على الأكواد <b>111</b> و <b>112</b> لمنع تضخيم أرصدة الخزينة بأرصدة ذمم غير نقدية.
                  </li>
                </ul>
              </div>
              <div className="p-2.5 bg-sky-50 border border-sky-200 rounded text-sky-950 text-[11px]">
                <span className="font-bold text-sky-900">إدارة العهد والتسويات متوفرة الآن:</span>
                <p className="mt-1 mb-2">
                  تم إطلاق الشاشة المخصصة المتكاملة <b>«إدارة العهد المالية والتسويات»</b> لتتيح للموظف رفع المصاريف وإجراء التسوية.
                </p>
                <button
                  type="button"
                  onClick={() => {
                    setShowAdvancesInfoModal(false);
                    alert('يمكنك الوصول لها من قائمة (الحسابات -> إدارة العهد المالية والتسويات).');
                  }}
                  className="px-3 py-1 bg-sky-600 hover:bg-sky-700 text-white rounded font-bold cursor-pointer transition"
                >
                  الانتقال لشاشة العهد المالية
                </button>
              </div>
              <div className="flex justify-end pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowAdvancesInfoModal(false)}
                  className="px-4 py-1.5 bg-slate-800 hover:bg-slate-900 text-white rounded font-bold cursor-pointer"
                >
                  فهمت ذلك
                </button>
              </div>
            </div>
          </div>
        </div>
`;

fs.writeFileSync('components/cash-vouchers/CashVoucherAdvancesModal.tsx', `import React from 'react';
import { Info, X } from 'lucide-react';

interface CashVoucherAdvancesModalProps {
  showAdvancesInfoModal: boolean;
  setShowAdvancesInfoModal: (v: boolean) => void;
}

export const CashVoucherAdvancesModal: React.FC<CashVoucherAdvancesModalProps> = ({
  showAdvancesInfoModal, setShowAdvancesInfoModal
}) => {
  if (!showAdvancesInfoModal) return null;
  return (${advStr});
};
`);
console.log('Fixed CashVoucherAdvancesModal');
