'use client';

import React, { useState } from 'react';
import { 
  Table, 
  Scan, 
  Search, 
  Grid, 
  ArrowDownToLine, 
  CornerDownLeft, 
  CheckCircle2, 
  Star, 
  ArrowRight,
  Sparkles,
  Info,
  Sliders,
  Play,
  Layers,
  Award,
  ThumbsUp,
  Cpu
} from 'lucide-react';
import { MainTabType, ProductItem, Customer } from '@/types/pos';

interface Props {
  onSelectTab: (tab: MainTabType) => void;
  products: ProductItem[];
  customers: Customer[];
}

export const SalesStrategiesLabView: React.FC<Props> = ({
  onSelectTab,
  products,
  customers,
}) => {
  // Strategy Ratings / Notes state for UX testing
  const [strategyRatings, setStrategyRatings] = useState<Record<string, { rating: number; notes: string }>>({
    '1': { rating: 5, notes: 'ممتازة للمحاسبين متمرسي كيبورد Numpad والأرقام المحاسبية.' },
    '2': { rating: 4, notes: 'واضحة جداً وتمنع التعديل الخاطئ بالخطأ في أسطر الجدول.' },
    '3': { rating: 5, notes: 'فائقة السرعة للمستودعات وكراتين التجزئة ذات الباركود.' },
    '4': { rating: 4, notes: 'مثالية إذا كان الصنف له أكثر من وحدة (كرتون/باكت/حبة) ورصيد متغير.' },
    '5': { rating: 3, notes: 'مناسبة جداً للمطاعم ومحلات اللمس السريع الشاشات Touch.' },
    '6': { rating: 5, notes: 'تسسرّع تحويل عروض الأسعار المسودة لفواتير دون تكرار الكتابة.' },
  });

  const strategiesDef = [
    {
      id: '1',
      tabKey: 'SALES_STRATEGY_INLINE_GRID' as MainTabType,
      title: 'استراتيجية 1: الجدول التفاعلي المباشر',
      formName: 'frmSalesInvoice_InlineGrid',
      icon: <Table className="w-6 h-6 text-emerald-600" />,
      badgeColor: 'bg-emerald-50 border-emerald-300 text-emerald-800 font-bold',
      description: 'إدخال وتعديل خلايا الكمية والسعر والخصم مباشرة بالجدول التنقل بزر Tab والأسهم.',
      targetUser: 'المحاسبون المحترفون ومتمرسو لوحة الأرقام السريعة (Numpad Grid Operators)',
      speedRating: 'فائقة جداً (Very High)',
      mouseRequired: 'لا تتطلب الماوس (Keyboard Only)',
      keyFeature: 'تعديل الخلايا مباشرة دون فتح نوافذ أو مغادرة الجدول.',
    },
    {
      id: '2',
      tabKey: 'SALES_STRATEGY_RAPID_BAR' as MainTabType,
      title: 'استراتيجية 2: شريط الإدخال السريع العلوي',
      formName: 'frmSalesInvoice_RapidBar',
      icon: <CornerDownLeft className="w-6 h-6 text-sky-600" />,
      badgeColor: 'bg-sky-50 border-sky-300 text-sky-800 font-bold',
      description: 'إدخال متسلسل في شريط موحد أعلى الجدول: [الصنف] -> [الكمية] -> [الوحدة] -> [Enter] -> الجدول.',
      targetUser: 'المُدخلون الذين يفضلون الإدخال التسلسلي بدون القفز بين أسطر الجدول.',
      speedRating: 'عالية جداً (High)',
      mouseRequired: 'أدنى حد (Minimal Mouse)',
      keyFeature: 'حماية أسطر الجدول من الضغط والتعديل الخاطئ العشوائي.',
    },
    {
      id: '3',
      tabKey: 'SALES_STRATEGY_SCANNER' as MainTabType,
      title: 'استراتيجية 3: أسلوب الـ Scanner الليزري المتواصل',
      formName: 'frmSalesInvoice_ContinuousScanner',
      icon: <Scan className="w-6 h-6 text-emerald-600" />,
      badgeColor: 'bg-emerald-50 border-emerald-300 text-emerald-800 font-bold',
      description: 'مسح الباركود متواصل مع وميض بصري وصوتي وزيادة تلقائية للكمية (+1) كل مسحة.',
      targetUser: 'المستودعات ومحلات التجزئة ومسؤولو الفرز المباشر بالباركود.',
      speedRating: 'الأسرع بلا منازع (Instant Scan)',
      mouseRequired: 'صفر ماوس (Hands-Free)',
      keyFeature: 'تأكيد بصري ليزري وصوتي عند نجاح مسح كل باركود.',
    },
    {
      id: '4',
      tabKey: 'SALES_STRATEGY_SMART_F3' as MainTabType,
      title: 'استراتيجية 4: البحث الذكي بالزر F3 وتعدد الأسعار',
      formName: 'frmSalesInvoice_SmartF3Lookup',
      icon: <Search className="w-6 h-6 text-purple-600" />,
      badgeColor: 'bg-purple-50 border-purple-300 text-purple-800 font-bold',
      description: 'نافذة منبثقة زر F3 للبحث بالاسم والرمز واستعراض أسعار الوحدات (كرتون/باكت/حبة) والمخزون.',
      targetUser: 'التجار والمحلات ذات الأصناف المتعددة الوحدات والأسعار المختلفة.',
      speedRating: 'عالية مع دقة (High Precision)',
      mouseRequired: 'اختياري (Keyboard / Mouse)',
      keyFeature: 'رؤية كارت الوحدات وتحديد سعر الجملة/التجزئة قبل البيع.',
    },
    {
      id: '5',
      tabKey: 'SALES_STRATEGY_TOUCH_GRID' as MainTabType,
      title: 'استراتيجية 5: بطاقات اللمس والمرئيات',
      formName: 'frmSalesInvoice_TouchGrid',
      icon: <Grid className="w-6 h-6 text-amber-600" />,
      badgeColor: 'bg-amber-50 border-amber-300 text-amber-800 font-bold',
      description: 'تصنيفات ملونة وبطاقات أصناف مرئية مصممة خصيصاً للشاشات التي تعمل باللمس (Touch Screen).',
      targetUser: 'الكاشيرات السريعة والشاشات التفاعلية باللمس بدون كيبورد.',
      speedRating: 'عالية باللمس (Touch High)',
      mouseRequired: 'لمس مباشر (Touch Screen)',
      keyFeature: 'بطاقات بصرية بأسعار الأصناف وأزرار زيادة/نقصان الكمية.',
    },
    {
      id: '6',
      tabKey: 'SALES_STRATEGY_IMPORT_CONVERT' as MainTabType,
      title: 'استراتيجية 6: تحويل العروض والاستيراد بنقرة',
      formName: 'frmSalesInvoice_ImportConvert',
      icon: <ArrowDownToLine className="w-6 h-6 text-blue-600" />,
      badgeColor: 'bg-blue-50 border-blue-300 text-blue-800 font-bold',
      description: 'تحويل عروض الأسعار السابقة والمسودات بضغطة زر واحدة إلى فاتورة مبيعات معتمدة.',
      targetUser: 'شركات الجملة والتجارة التي تبدأ بعروض أسعار للعملاء قبل البيع.',
      speedRating: 'فورية للدفعات (Instant Batch)',
      mouseRequired: 'نقرة واحدة (1-Click)',
      keyFeature: 'منع إعادة إدخال البيانات المكتوبة سابقاً بعرض السعر.',
    },
  ];

  const handleUpdateRating = (id: string, rating: number) => {
    setStrategyRatings(prev => ({
      ...prev,
      [id]: { ...(prev[id] || { notes: '' }), rating }
    }));
  };

  const handleUpdateNotes = (id: string, notes: string) => {
    setStrategyRatings(prev => ({
      ...prev,
      [id]: { ...(prev[id] || { rating: 5 }), notes }
    }));
  };

  return (
    <div className="flex flex-col h-full bg-slate-100 text-slate-900 font-sans select-none overflow-y-auto">
      {/* Top Banner Header */}
      <div className="bg-white border-b border-slate-200 p-4 shadow-xs flex flex-wrap items-center justify-between gap-4 text-slate-800">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-xl bg-indigo-50 border border-indigo-200 flex items-center justify-center text-indigo-700 shadow-xs">
            <Cpu className="w-6 h-6 text-indigo-700" />
          </div>
          <div>
            <h1 className="text-base font-extrabold text-slate-900 flex items-center gap-2">
              <span>مختبر واختبار استراتيجيات إدخال الفواتير الـ 6 [UX Testing & Evaluation Lab]</span>
            </h1>
            <p className="text-xs text-slate-500">
              قم بتجربة كل شاشة على حدة مع بيانات الـ Mock Data المتاحة، وسجل تقييمك وملاحظاتك لاختيار الاستراتيجية الأنسب أو دمج الأفضل منها.
            </p>
          </div>
        </div>

        <button
          onClick={() => onSelectTab('SALES_INVOICE')}
          className="px-4 py-2 bg-emerald-600 hover:bg-emerald-500 text-white font-bold text-xs rounded-lg shadow-xs flex items-center gap-2 transition cursor-pointer"
        >
          <Table className="w-4 h-4" />
          <span>فتح شاشة الفواتير الشاملة [frmSalesInvoice]</span>
        </button>
      </div>

      <div className="p-4 space-y-6 flex-1">
        {/* Intro Guidance */}
        <div className="bg-white border border-slate-200 rounded-xl p-4 flex flex-col md:flex-row items-start md:items-center justify-between gap-4 text-xs shadow-xs">
          <div className="flex items-start gap-3">
            <Sparkles className="w-5 h-5 text-indigo-600 shrink-0 mt-0.5" />
            <div>
              <span className="font-bold text-slate-900 text-sm block mb-1">منهجية اختيار استراتيجيات الإدخال للعملاء:</span>
              <p className="text-slate-600 leading-relaxed font-medium">
                لكل نشاط تجاري أسلوب عمل محدد. من خلال هذا المختبر، يمكنك الدخول على كل شاشة مستقلة، واختبار تجربة المستخدم (UX)، ثم تحديد أي استراتيجية ستعتمدها كخيار افتراضي أو توفر خيار التبديل بينها للمستخدمين.
              </p>
            </div>
          </div>
        </div>

        {/* 6 Strategy Cards Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {strategiesDef.map((strat) => {
            const userEval = strategyRatings[strat.id] || { rating: 5, notes: '' };
            return (
              <div
                key={strat.id}
                className="bg-white border border-slate-200 hover:border-indigo-500 rounded-xl p-4 flex flex-col justify-between shadow-xs hover:shadow-md transition-all group"
              >
                <div>
                  <div className="flex items-start justify-between gap-2 border-b border-slate-100 pb-3 mb-3">
                    <div className="flex items-center gap-2.5">
                      <div className="p-2 bg-slate-50 rounded-lg border border-slate-200">
                        {strat.icon}
                      </div>
                      <div>
                        <h3 className="font-extrabold text-xs text-slate-900 group-hover:text-indigo-700 transition-colors">
                          {strat.title}
                        </h3>
                        <span className={`text-[10px] font-mono px-2 py-0.5 rounded-full border mt-1 inline-block ${strat.badgeColor}`}>
                          {strat.formName}
                        </span>
                      </div>
                    </div>
                  </div>

                  <p className="text-xs text-slate-600 font-medium mb-3 leading-relaxed">
                    {strat.description}
                  </p>

                  <div className="space-y-1.5 text-[11px] bg-slate-50 p-2.5 rounded-lg border border-slate-200 mb-3 text-slate-700">
                    <div><span className="text-slate-500 font-bold">المستخدم المستهدف:</span> {strat.targetUser}</div>
                    <div><span className="text-slate-500 font-bold">السرعة المتوقعة:</span> <strong className="text-emerald-700">{strat.speedRating}</strong></div>
                    <div><span className="text-slate-500 font-bold">استخدام الماوس:</span> <span className="text-sky-700 font-bold">{strat.mouseRequired}</span></div>
                  </div>
                </div>

                <div className="pt-2 border-t border-slate-100 flex flex-col gap-3">
                  {/* Rating Selector */}
                  <div className="flex items-center justify-between text-xs">
                    <span className="text-slate-600 text-[11px] font-bold">تقييم تجربة المستخدم:</span>
                    <div className="flex items-center gap-1">
                      {[1, 2, 3, 4, 5].map((star) => (
                        <button
                          key={star}
                          onClick={() => handleUpdateRating(strat.id, star)}
                          className="p-0.5 hover:scale-110 transition cursor-pointer"
                        >
                          <Star className={`w-3.5 h-3.5 ${star <= userEval.rating ? 'text-amber-400 fill-amber-400' : 'text-slate-300'}`} />
                        </button>
                      ))}
                    </div>
                  </div>

                  {/* Launch Strategy Standalone Screen */}
                  <button
                    onClick={() => onSelectTab(strat.tabKey)}
                    className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-extrabold py-2 px-3 rounded-lg text-xs shadow-xs flex items-center justify-center gap-2 transition cursor-pointer"
                  >
                    <Play className="w-3.5 h-3.5 fill-white" />
                    <span>تشغيل واختبار الشاشة المنفصلة</span>
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};
