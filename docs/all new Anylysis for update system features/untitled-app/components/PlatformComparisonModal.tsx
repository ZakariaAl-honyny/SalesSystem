'use client';

import React from 'react';
import {
  Monitor,
  Globe,
  Smartphone,
  CheckCircle2,
  Zap,
  WifiOff,
  Layers,
  HardDrive,
  ShieldCheck,
  Server,
  Database,
  ArrowRight,
  X
} from 'lucide-react';

interface PlatformComparisonModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSelectPlatform: (platform: 'DESKTOP' | 'WEB' | 'MOBILE') => void;
}

export const PlatformComparisonModal: React.FC<PlatformComparisonModalProps> = ({
  isOpen,
  onClose,
  onSelectPlatform,
}) => {
  if (!isOpen) return null;

  const platforms = [
    {
      id: 'DESKTOP' as const,
      title: '🖥️ تطبيق سطح المكتب (Desktop App)',
      sub: 'Windows / WinForms / POS Terminals',
      color: 'sky',
      badge: 'الخيار الأمثل للكاشيرات ونقاط البيع السريعة',
      features: [
        '⚡ سرعة قصوى واستجابة فورية (Zero-Latency) دون انتظار الإنترنت.',
        '⌨️ دعم كامل لاختصارات لوحة المفاتيح المتقدمة (F1..F12) لإدخال سريع بدون فأرة.',
        '🔌 عمل كامل بدون إنترنت (Offline-First) مع قاعدة بيانات محلية (SQLite / SQL Server Express).',
        '🖨️ اتصال مباشر مع الطابعات الحرارية (ESC/POS)، أدراج النقدية، وقارئات الباركود الليزرية.',
        '📊 شاشات إدخال مكثفة جداً (Inline DataGridView) لإصدار 100+ بند في ثوان.',
      ],
      idealFor: 'محلات التجزئة، السوبرماركت، الصيدليات، والمطاعم المزدحمة.',
      actionLabel: 'تجربة بيئة سطح المكتب',
    },
    {
      id: 'WEB' as const,
      title: '🌐 منصة الويب السحابية (Cloud Web ERP)',
      sub: 'Browser / SaaS Portal / Multi-Branch',
      color: 'indigo',
      badge: 'الخيار الأمثل للإدارة العليا والفروع المتعددة',
      features: [
        '🌍 وصول من أي متصفح وجهاز (Windows, Mac, Linux, iPad) دون تثبيت.',
        '🏢 إدارة موحدة لشبكة الفروع، المستودعات المركزية، والصلاحيات.',
        '📈 لوحات تحكم ومؤشرات بيانية تفاعلية ورسوم بيانية لأرباح الشركات.',
        '🔄 مزامنة مركزية لحظية عبر خوادم سحابية وقواعد بيانات علائقية.',
        '📑 تصدير متقدم لكشوفات الحسابات والتقارير المالية بصيغ Excel و PDF.',
      ],
      idealFor: 'أصحاب الشركات، المدراء الماليين، سلاسل المحلات، والعمل عن بعد.',
      actionLabel: 'تجربة منصة الويب السحابية',
    },
    {
      id: 'MOBILE' as const,
      title: '📱 تطبيق الموبايل والأجهزة المحمولة (Mobile / PDA)',
      sub: 'Android / iOS / Handheld Zebra PDA',
      color: 'emerald',
      badge: 'الخيار الأمثل للمناديب والمخازن والجرد الميداني',
      features: [
        '📦 مسح الباركود السريع بكاميرا الهاتف أو حساس الليزر في أجهزة الـ PDA.',
        '🚚 نقطة بيع محمولة لسيارات التوزيع ومندوبي المبيعات الميدانيين.',
        '💵 إصدار سندات القبض الميدانية وتحصيل الديون مع مشاركة الإيصال عبر WhatsApp.',
        '📋 جرد المخزون الفعلي الميداني ومطابقة الأرصدة مباشرة من الرفوف.',
        '📶 وضع العمل بدون إنترنت مع مزامنة تلقائية عند توفر شبكة 4G/WiFi.',
      ],
      idealFor: 'مناديب المبيعات والتوزيع، مسؤولي الجرد والمستودعات، والمراقبة السريعة.',
      actionLabel: 'تجربة تطبيق الموبايل',
    },
  ];

  return (
    <div className="fixed inset-0 bg-black/60 backdrop-blur-xs z-50 flex items-center justify-center p-3 select-none overflow-y-auto" dir="rtl">
      <div className="bg-white rounded-3xl border border-slate-200 shadow-2xl w-full max-w-5xl overflow-hidden flex flex-col max-h-[92vh] animate-in zoom-in-95">
        {/* Header */}
        <div className="bg-white text-slate-800 p-5 flex items-center justify-between border-b border-slate-200 shrink-0 shadow-2xs">
          <div className="flex items-center gap-3">
            <div className="p-2.5 bg-sky-50 border border-sky-200 rounded-2xl text-sky-700 shadow-xs">
              <Layers className="w-6 h-6" />
            </div>
            <div>
              <h2 className="text-base font-bold text-slate-900">خارطة تجربة المستخدم والمنصات الثلاث (Multi-Platform UX Roadmap)</h2>
              <p className="text-xs text-slate-500">مقارنة شاملة لمعمارية النظام عبر: سطح المكتب (Desktop) - الويب (Web) - والموبايل (Mobile)</p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="w-8 h-8 rounded-full bg-slate-100 hover:bg-slate-200 flex items-center justify-center text-slate-500 hover:text-slate-800 transition cursor-pointer"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Modal Body: 3 Columns Grid */}
        <div className="p-5 overflow-y-auto flex-1 grid grid-cols-1 md:grid-cols-3 gap-4 bg-slate-50">
          {platforms.map((p) => (
            <div
              key={p.id}
              className="bg-white rounded-2xl border border-slate-200 p-4.5 flex flex-col justify-between shadow-2xs hover:shadow-md transition space-y-4"
            >
              <div className="space-y-3">
                {/* Title & Badge */}
                <div>
                  <h3 className="font-bold text-slate-900 text-sm">{p.title}</h3>
                  <span className="text-[11px] text-slate-500 font-mono block mt-0.5">{p.sub}</span>
                  <div className="mt-2">
                    <span className="text-[10px] bg-slate-100 text-slate-700 font-bold px-2 py-0.5 rounded-full border border-slate-200 inline-block">
                      {p.badge}
                    </span>
                  </div>
                </div>

                {/* Features List */}
                <div className="space-y-2 pt-2 border-t border-slate-100">
                  <span className="text-[11px] font-bold text-slate-700 block">الميزات وتجربة المستخدم:</span>
                  <ul className="space-y-2 text-xs text-slate-600">
                    {p.features.map((f, idx) => (
                      <li key={idx} className="flex items-start gap-1.5 leading-relaxed">
                        <CheckCircle2 className="w-3.5 h-3.5 text-sky-600 shrink-0 mt-0.5" />
                        <span>{f}</span>
                      </li>
                    ))}
                  </ul>
                </div>

                {/* Ideal For */}
                <div className="bg-slate-50 p-2.5 rounded-xl border border-slate-200 text-xs">
                  <span className="font-bold text-slate-800 block text-[11px]">الاستخدام الأمثل:</span>
                  <span className="text-slate-600 text-[11px]">{p.idealFor}</span>
                </div>
              </div>

              {/* Action Button to Launch Viewport */}
              <button
                onClick={() => {
                  onSelectPlatform(p.id);
                  onClose();
                }}
                className="w-full bg-slate-900 hover:bg-slate-800 text-white py-2.5 rounded-xl font-bold text-xs transition flex items-center justify-center gap-1.5 shadow-xs cursor-pointer"
              >
                <span>{p.actionLabel}</span>
                <ArrowRight className="w-3.5 h-3.5 rotate-180" />
              </button>
            </div>
          ))}
        </div>

        {/* Footer Technical Note */}
        <div className="bg-white border-t border-slate-200 px-5 py-3 flex flex-wrap items-center justify-between text-xs text-slate-600 shrink-0">
          <div className="flex items-center gap-2">
            <Database className="w-4 h-4 text-sky-600" />
            <span>
              <strong>محرك البيانات الموحد (Shared Core Engine):</strong> جميع المنصات الثلاث تشترك في نفس منطق التكلفة (FIFO)، القيود المزدوجة المتوازنة، والتطبيع الرقمي الصارم.
            </span>
          </div>

          <button
            onClick={onClose}
            className="px-4 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold rounded-xl transition cursor-pointer text-xs"
          >
            إغلاق الدليل
          </button>
        </div>
      </div>
    </div>
  );
};
