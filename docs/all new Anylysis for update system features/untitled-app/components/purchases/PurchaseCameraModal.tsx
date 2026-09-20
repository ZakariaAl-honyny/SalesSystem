'use client';

import React from 'react';
import { Camera, Sparkles, Archive, Info, CheckCircle2 } from 'lucide-react';

export interface PurchaseCameraModalProps {
  isOpen: boolean;
  attachedImageName: string | null;
  onAttach: (name: string) => void;
  onClose: () => void;
}

export const PurchaseCameraModal: React.FC<PurchaseCameraModalProps> = ({
  isOpen,
  attachedImageName,
  onAttach,
  onClose,
}) => {
  const [archiveRef, setArchiveRef] = React.useState(attachedImageName || '');

  if (!isOpen) return null;

  const handleSaveRef = () => {
    if (archiveRef.trim()) {
      onAttach(`مرجع أرشيف: ${archiveRef.trim()}`);
    } else {
      onAttach('');
    }
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-xs animate-in fade-in">
      <div className="bg-white rounded-2xl max-w-sm w-full p-5 shadow-2xl border border-slate-200 text-right space-y-4">
        {/* Header */}
        <div className="flex items-center justify-between border-b border-slate-100 pb-3">
          <div className="flex items-center gap-2 text-slate-800">
            <div className="p-2 bg-sky-50 text-[#0277bd] rounded-xl">
              <Camera className="w-5 h-5" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900">مرفقات وصور الفاتورة</h3>
              <span className="text-[11px] font-semibold text-amber-600 bg-amber-50 px-2 py-0.5 rounded-full inline-flex items-center gap-1">
                <Sparkles className="w-3 h-3" />
                مجدولة للإصدار الثاني (V2)
              </span>
            </div>
          </div>
          <button
            onClick={onClose}
            className="text-slate-400 hover:text-slate-600 p-1 rounded-lg hover:bg-slate-100 transition cursor-pointer"
          >
            ✕
          </button>
        </div>

        {/* Informative explanation banner */}
        <div className="bg-sky-50/70 border border-sky-200/80 rounded-xl p-3 text-right text-xs text-sky-900 space-y-1.5 leading-relaxed">
          <div className="flex items-center gap-1.5 font-bold text-sky-950">
            <Info className="w-4 h-4 text-[#0277bd] shrink-0" />
            <span>خارطة طريق التطوير الرشيق (Lean V1):</span>
          </div>
          <p className="text-[11px] text-slate-600">
            تم تأجيل الرفع السحابي لملفات الصور الثقيلة إلى الإصدار القادم (V2) للحفاظ على خفة وسرعة قاعدة البيانات القصوى في المحلات التجارية.
          </p>
        </div>

        {/* Alternative Lean V1 feature: Physical Archive Reference / Paper Voucher No */}
        <div className="space-y-2">
          <label className="text-xs font-bold text-slate-700 flex items-center gap-1.5">
            <Archive className="w-4 h-4 text-slate-500" />
            <span>رقم الأرشيف الورقي / مرجع السند (اختياري للـ V1):</span>
          </label>
          <input
            type="text"
            value={archiveRef}
            onChange={(e) => setArchiveRef(e.target.value)}
            placeholder="مثال: ملف المشتريات رقم 4 - سند 204"
            className="w-full text-xs p-2.5 bg-slate-50 border border-slate-300 rounded-lg text-slate-800 focus:bg-white focus:border-[#0277bd] focus:ring-1 focus:ring-[#0277bd] outline-hidden transition"
          />
        </div>

        {attachedImageName && (
          <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 p-2.5 rounded-lg text-xs flex items-center justify-between">
            <span className="font-semibold flex items-center gap-1.5">
              <CheckCircle2 className="w-4 h-4 text-emerald-600" />
              {attachedImageName}
            </span>
            <button
              onClick={() => {
                setArchiveRef('');
                onAttach('');
              }}
              className="text-rose-600 font-bold hover:underline cursor-pointer"
            >
              إلغاء
            </button>
          </div>
        )}

        {/* Actions */}
        <div className="pt-2 flex items-center gap-2">
          <button
            type="button"
            onClick={handleSaveRef}
            className="flex-1 bg-[#0277bd] hover:bg-[#01579b] text-white py-2.5 rounded-xl font-bold text-xs shadow-sm transition active:scale-98 cursor-pointer"
          >
            حفظ المرجع
          </button>
          <button
            type="button"
            onClick={onClose}
            className="text-xs font-bold text-slate-600 hover:bg-slate-100 px-4 py-2.5 rounded-xl transition cursor-pointer"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
};
