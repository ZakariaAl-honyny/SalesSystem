'use client';

import React, { useState, useMemo } from 'react';
import { ProductItem } from '@/types/pos';
import {
  Tag,
  TrendingUp,
  Percent,
  Coins,
  ShieldAlert,
  CheckCircle2,
  AlertTriangle,
  RotateCcw,
  Save,
  Search,
  Filter,
  ArrowUpRight,
  ArrowDownRight,
  HelpCircle,
  Sparkles,
  Printer,
  Sliders,
  CheckSquare,
  Square,
  Layers,
} from 'lucide-react';

interface QuickPriceUpdateViewProps {
  products: ProductItem[];
  categories: string[];
  currency: string;
  onBulkUpdatePrices: (updatedProducts: ProductItem[]) => void;
}

interface PriceDraft {
  newSalePrice: number;
  newMinSalePrice: number;
  isModified: boolean;
}

export const QuickPriceUpdateView: React.FC<QuickPriceUpdateViewProps> = ({
  products,
  categories,
  currency,
  onBulkUpdatePrices,
}) => {
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('ALL');
  const [filterBelowCostOnly, setFilterBelowCostOnly] = useState(false);
  const [selectedProductIds, setSelectedProductIds] = useState<Set<string>>(new Set());

  // Local draft of proposed prices { productId: PriceDraft }
  const [priceDrafts, setPriceDrafts] = useState<Record<string, PriceDraft>>({});

  // Bulk Operations State
  const [bulkMode, setBulkMode] = useState<'PERCENTAGE' | 'FIXED_AMOUNT' | 'COST_MARKUP'>('COST_MARKUP');
  const [bulkValue, setBulkValue] = useState<number>(20); // e.g. 20% markup
  const [applyTarget, setApplyTarget] = useState<'ALL_FILTERED' | 'SELECTED_ONLY'>('ALL_FILTERED');
  const [roundToNearest, setRoundToNearest] = useState<number>(50); // Round prices to nearest 50 YER or 0 (no rounding)
  const [feedback, setFeedback] = useState<{ text: string; type: 'SUCCESS' | 'WARNING' | 'ERROR' } | null>(null);

  // Filtered Products
  const filteredProducts = useMemo(() => {
    return products.filter((p) => {
      const matchCategory = selectedCategory === 'ALL' || p.category === selectedCategory;
      const matchSearch =
        searchQuery.trim() === '' ||
        p.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        p.code.includes(searchQuery) ||
        p.barcode.includes(searchQuery);

      const effectivePrice = priceDrafts[p.id]?.newSalePrice ?? p.salePrice;
      const matchBelowCost = !filterBelowCostOnly || effectivePrice <= p.costPrice;

      return matchCategory && matchSearch && matchBelowCost;
    });
  }, [products, selectedCategory, searchQuery, filterBelowCostOnly, priceDrafts]);

  // Statistics
  const stats = useMemo(() => {
    let modifiedCount = 0;
    let belowCostCount = 0;
    let totalCurrentValue = 0;
    let totalProposedValue = 0;

    products.forEach((p) => {
      const draft = priceDrafts[p.id];
      const effPrice = draft?.newSalePrice ?? p.salePrice;
      if (draft && draft.isModified) modifiedCount++;
      if (effPrice <= p.costPrice) belowCostCount++;

      totalCurrentValue += p.salePrice * p.stock;
      totalProposedValue += effPrice * p.stock;
    });

    return {
      totalProducts: products.length,
      modifiedCount,
      belowCostCount,
      totalCurrentValue,
      totalProposedValue,
      valueDiff: totalProposedValue - totalCurrentValue,
    };
  }, [products, priceDrafts]);

  // Handle single cell edit
  const handlePriceChange = (productId: string, val: number) => {
    const original = products.find((p) => p.id === productId);
    if (!original) return;

    const newSalePrice = Math.max(0, val);
    const newMinSalePrice = Math.round(newSalePrice * 0.95);
    const isModified = newSalePrice !== original.salePrice;

    setPriceDrafts((prev) => ({
      ...prev,
      [productId]: {
        newSalePrice,
        newMinSalePrice,
        isModified,
      },
    }));
  };

  // Helper function to round price
  const roundPrice = (price: number, nearest: number) => {
    if (nearest <= 0) return Math.round(price);
    return Math.round(price / nearest) * nearest;
  };

  // Apply Bulk Adjustment to Drafts
  const handleApplyBulk = () => {
    const targetProducts =
      applyTarget === 'SELECTED_ONLY'
        ? filteredProducts.filter((p) => selectedProductIds.has(p.id))
        : filteredProducts;

    if (targetProducts.length === 0) {
      setFeedback({
        text: 'لم يتم تحديد أي أصناف لتطبيق التعديل عليها!',
        type: 'WARNING',
      });
      return;
    }

    const newDrafts = { ...priceDrafts };
    let updatedCount = 0;

    targetProducts.forEach((p) => {
      let proposedPrice = p.salePrice;

      if (bulkMode === 'PERCENTAGE') {
        // e.g. +10% or -5%
        proposedPrice = p.salePrice * (1 + bulkValue / 100);
      } else if (bulkMode === 'FIXED_AMOUNT') {
        // e.g. +500 YER
        proposedPrice = p.salePrice + bulkValue;
      } else if (bulkMode === 'COST_MARKUP') {
        // e.g. Cost + 20%
        proposedPrice = p.costPrice * (1 + bulkValue / 100);
      }

      proposedPrice = Math.max(0, roundPrice(proposedPrice, roundToNearest));
      const minPrice = Math.round(proposedPrice * 0.95);

      newDrafts[p.id] = {
        newSalePrice: proposedPrice,
        newMinSalePrice: minPrice,
        isModified: proposedPrice !== p.salePrice,
      };
      updatedCount++;
    });

    setPriceDrafts(newDrafts);
    setFeedback({
      text: `تم احتساب الأسعار المقترحة بنجاح لـ ${updatedCount} صنف. راجع القيم واضغط "حفظ واعتماد التعديلات".`,
      type: 'SUCCESS',
    });
  };

  // Reset Drafts
  const handleResetDrafts = () => {
    if (stats.modifiedCount === 0) return;
    if (confirm('هل أنت متأكد من إلغاء كافة التعديلات المؤقتة والرجوع للأسعار الأصلية؟')) {
      setPriceDrafts({});
      setFeedback({
        text: 'تمت استعادة الأسعار الأصلية بنجاح.',
        type: 'SUCCESS',
      });
    }
  };

  // Select / Deselect All
  const handleToggleSelectAll = () => {
    if (selectedProductIds.size === filteredProducts.length && filteredProducts.length > 0) {
      setSelectedProductIds(new Set());
    } else {
      setSelectedProductIds(new Set(filteredProducts.map((p) => p.id)));
    }
  };

  const handleToggleSelectOne = (id: string) => {
    const next = new Set(selectedProductIds);
    if (next.has(id)) {
      next.delete(id);
    } else {
      next.add(id);
    }
    setSelectedProductIds(next);
  };

  // Save changes to parent state
  const handleSaveChanges = () => {
    const modifiedIds = Object.keys(priceDrafts).filter((id) => priceDrafts[id].isModified);
    if (modifiedIds.length === 0) {
      alert('لم تقم بإجراء أي تعديلات على الأسعار حتى الآن.');
      return;
    }

    if (
      !confirm(
        `أنت على وشك تحديث أسعار بيع ${modifiedIds.length} صنف في النظام بشكل دائم. هل تريد المتابعة؟`
      )
    ) {
      return;
    }

    const updatedCatalog = products.map((p) => {
      const draft = priceDrafts[p.id];
      if (!draft || !draft.isModified) return p;

      // Update base unit price and proportional unit prices if present
      const ratio = p.salePrice > 0 ? draft.newSalePrice / p.salePrice : 1;
      const updatedUnits = p.units?.map((u) => {
        if (u.factor === 1) {
          return { ...u, price: draft.newSalePrice };
        }
        // If sub-unit, adjust proportionally by ratio or factor
        return {
          ...u,
          price: Math.round(u.price * ratio),
        };
      });

      return {
        ...p,
        salePrice: draft.newSalePrice,
        minSalePrice: draft.newMinSalePrice,
        units: updatedUnits,
      };
    });

    onBulkUpdatePrices(updatedCatalog);
    setPriceDrafts({});
    setFeedback({
      text: `تم حفظ واعتماد الأسعار الجديدة لـ ${modifiedIds.length} صنف بنجاح! تم تحديث وحدات البيع والكاشير تلقائياً.`,
      type: 'SUCCESS',
    });
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 p-3 gap-3 overflow-hidden font-sans select-none" dir="rtl">
      {/* Top Banner: Overview & Summary Stats */}
      <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-3 shrink-0">
        <div className="flex items-center gap-2.5">
          <div className="p-2 bg-amber-50 text-amber-600 rounded-lg border border-amber-200">
            <Sliders className="w-5 h-5" />
          </div>
          <div>
            <h2 className="text-sm font-bold text-slate-900 flex items-center gap-2">
              <span>إدارة وتحديث الأسعار المرنة (Quick Price Matrix)</span>
              <span className="text-[10px] bg-sky-50 text-sky-700 border border-sky-200 px-2 py-0.5 rounded-full font-mono font-bold">
                {products.length} صنف مسجل
              </span>
            </h2>
            <p className="text-[11px] text-slate-500">
              تعديل أسعار البيع بمرونة وفق تغيرات تكلفة الشراء والدفعات، مع حماية هامش الربح والرقابة على البيع بأقل من التكلفة.
            </p>
          </div>
        </div>

        {/* Live Metrics */}
        <div className="flex items-center gap-2 flex-wrap text-xs">
          <div className="bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200">
            <span className="text-slate-500 block text-[10px]">الأصناف المعدلة حالياً:</span>
            <span className={`font-mono font-bold ${stats.modifiedCount > 0 ? 'text-amber-600' : 'text-slate-700'}`}>
              {stats.modifiedCount} صنف
            </span>
          </div>

          <div className="bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200">
            <span className="text-slate-500 block text-[10px]">تحذير بيع بأقل من التكلفة:</span>
            <span className={`font-mono font-bold flex items-center gap-1 ${stats.belowCostCount > 0 ? 'text-rose-600' : 'text-emerald-600'}`}>
              {stats.belowCostCount > 0 && <AlertTriangle className="w-3 h-3" />}
              {stats.belowCostCount} صنف
            </span>
          </div>

          <div className="bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200">
            <span className="text-slate-500 block text-[10px]">فارق القيمة السوقية للمخزون:</span>
            <span className={`font-mono font-bold ${stats.valueDiff >= 0 ? 'text-emerald-600' : 'text-rose-600'}`}>
              {stats.valueDiff >= 0 ? '+' : ''}{stats.valueDiff.toLocaleString()} {currency}
            </span>
          </div>
        </div>
      </div>

      {/* Bulk Operations Toolbar */}
      <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-xs flex flex-col md:flex-row items-stretch md:items-center justify-between gap-3 shrink-0">
        {/* Left: Bulk Adjustment Strategy */}
        <div className="flex items-center gap-2 flex-wrap text-xs flex-1">
          <span className="font-bold text-sky-700 flex items-center gap-1">
            <Sparkles className="w-3.5 h-3.5" />
            <span>معالج التسعير الجماعي:</span>
          </span>

          {/* Strategy Mode */}
          <select
            value={bulkMode}
            onChange={(e) => setBulkMode(e.target.value as any)}
            className="bg-slate-50 border border-slate-300 rounded px-2.5 py-1.5 font-semibold text-slate-800 focus:outline-none focus:ring-1 focus:ring-sky-500 cursor-pointer text-xs"
          >
            <option value="COST_MARKUP">هامش ربح فوق سعر الشراء (التكلفة + %)</option>
            <option value="PERCENTAGE">تعديل بنسبة مئوية على سعر البيع الحالي (+ / - %)</option>
            <option value="FIXED_AMOUNT">تعديل بمبلغ ثابت على سعر البيع (+ / - ر.ي)</option>
          </select>

          {/* Value Input */}
          <div className="flex items-center gap-1 bg-slate-50 px-2 py-1 rounded border border-slate-300">
            <input
              type="number"
              value={bulkValue}
              onChange={(e) => setBulkValue(parseFloat(e.target.value) || 0)}
              className="w-16 bg-transparent text-center font-mono font-bold text-amber-700 focus:outline-none"
            />
            <span className="text-[11px] font-bold text-slate-500">
              {bulkMode === 'FIXED_AMOUNT' ? currency : '%'}
            </span>
          </div>

          {/* Quick Presets */}
          <div className="flex items-center gap-1">
            {[10, 15, 20, 25].map((val) => (
              <button
                key={val}
                type="button"
                onClick={() => {
                  setBulkMode('COST_MARKUP');
                  setBulkValue(val);
                }}
                className={`px-1.5 py-0.5 rounded text-[10px] font-mono font-bold transition cursor-pointer border ${
                  bulkMode === 'COST_MARKUP' && bulkValue === val
                    ? 'bg-amber-600 text-white border-amber-600 shadow-2xs'
                    : 'bg-slate-100 text-slate-700 border-slate-200 hover:bg-slate-200'
                }`}
                title={`ضبط تلقائي لهامش ربح +${val}% فوق التكلفة`}
              >
                +{val}%
              </button>
            ))}
          </div>

          {/* Rounding option */}
          <div className="flex items-center gap-1 text-[11px] text-slate-600">
            <span>تقريب:</span>
            <select
              value={roundToNearest}
              onChange={(e) => setRoundToNearest(parseInt(e.target.value, 10))}
              className="bg-slate-50 border border-slate-300 rounded px-1.5 py-1 text-slate-700 text-xs"
            >
              <option value="0">بدون تقريب</option>
              <option value="10">10 {currency}</option>
              <option value="50">50 {currency}</option>
              <option value="100">100 {currency}</option>
            </select>
          </div>

          {/* Target */}
          <select
            value={applyTarget}
            onChange={(e) => setApplyTarget(e.target.value as any)}
            className="bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-700 text-xs"
          >
            <option value="ALL_FILTERED">لكل المعروض ({filteredProducts.length})</option>
            <option value="SELECTED_ONLY">للمحدد فقط ({selectedProductIds.size})</option>
          </select>

          {/* Apply Button */}
          <button
            type="button"
            onClick={handleApplyBulk}
            className="bg-sky-600 hover:bg-sky-500 active:bg-sky-700 text-white px-3 py-1.5 rounded font-bold transition flex items-center gap-1 shadow-xs cursor-pointer text-xs"
          >
            <TrendingUp className="w-3.5 h-3.5" />
            <span>احتساب في الجدول</span>
          </button>
        </div>

        {/* Right: Reset & Final Commit Buttons */}
        <div className="flex items-center gap-2 shrink-0">
          {stats.modifiedCount > 0 && (
            <button
              type="button"
              onClick={handleResetDrafts}
              className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 px-3 py-1.5 rounded text-xs font-semibold flex items-center gap-1 transition cursor-pointer"
              title="إلغاء التعديلات المؤقتة"
            >
              <RotateCcw className="w-3.5 h-3.5" />
              <span>تراجع عن التعديل</span>
            </button>
          )}

          <button
            type="button"
            onClick={handleSaveChanges}
            disabled={stats.modifiedCount === 0}
            className={`px-4 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 shadow-xs cursor-pointer ${
              stats.modifiedCount > 0
                ? 'bg-emerald-600 hover:bg-emerald-500 text-white'
                : 'bg-slate-100 text-slate-400 border border-slate-200 cursor-not-allowed'
            }`}
          >
            <Save className="w-4 h-4" />
            <span>حفظ واعتماد التعديلات ({stats.modifiedCount})</span>
          </button>
        </div>
      </div>

      {/* Feedback Toast Notification */}
      {feedback && (
        <div
          className={`px-3 py-2 rounded-lg text-xs font-medium flex items-center justify-between gap-2 border transition shrink-0 ${
            feedback.type === 'SUCCESS'
              ? 'bg-emerald-50 text-emerald-800 border-emerald-200'
              : feedback.type === 'WARNING'
              ? 'bg-amber-50 text-amber-800 border-amber-200'
              : 'bg-rose-50 text-rose-800 border-rose-200'
          }`}
        >
          <div className="flex items-center gap-2">
            {feedback.type === 'SUCCESS' && <CheckCircle2 className="w-4 h-4 text-emerald-600" />}
            {feedback.type === 'WARNING' && <AlertTriangle className="w-4 h-4 text-amber-600" />}
            {feedback.type === 'ERROR' && <ShieldAlert className="w-4 h-4 text-rose-600" />}
            <span>{feedback.text}</span>
          </div>
          <button
            type="button"
            onClick={() => setFeedback(null)}
            className="text-slate-400 hover:text-slate-700 text-sm cursor-pointer"
          >
            ✕
          </button>
        </div>
      )}

      {/* Search & Filters Bar */}
      <div className="bg-white px-3 py-2 rounded-lg border border-slate-200 flex flex-wrap items-center justify-between gap-2 text-xs shrink-0 shadow-2xs">
        <div className="flex items-center gap-2 flex-wrap flex-1">
          {/* Search Box */}
          <div className="relative min-w-[240px] flex-1 max-w-sm">
            <Search className="w-3.5 h-3.5 absolute right-2.5 top-1/2 -translate-y-1/2 text-slate-400" />
            <input
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              placeholder="بحث بالاسم أو الباركود أو الكود..."
              className="w-full bg-slate-50 border border-slate-300 rounded pr-8 pl-3 py-1 text-slate-800 text-xs placeholder-slate-400 focus:outline-none focus:ring-1 focus:ring-sky-500"
            />
          </div>

          {/* Category Filter */}
          <select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
            className="bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-700 text-xs focus:outline-none cursor-pointer"
          >
            {categories.map((c) => (
              <option key={c} value={c}>
                {c === 'ALL' ? 'جميع التصنيفات' : c}
              </option>
            ))}
          </select>

          {/* Floor Price Filter Toggle */}
          <button
            type="button"
            onClick={() => setFilterBelowCostOnly(!filterBelowCostOnly)}
            className={`px-2.5 py-1 rounded text-xs font-semibold transition flex items-center gap-1.5 cursor-pointer border ${
              filterBelowCostOnly
                ? 'bg-rose-50 text-rose-700 border-rose-300'
                : 'bg-slate-50 text-slate-600 hover:bg-slate-100 border-slate-300'
            }`}
          >
            <AlertTriangle className="w-3.5 h-3.5 text-rose-500" />
            <span>عرض الأصناف المعرضة للخسارة فقط</span>
          </button>
        </div>

        <div className="flex items-center gap-2 text-slate-500 text-[11px]">
          <span>المعروض: {filteredProducts.length} من {products.length} صنف</span>
          <span>•</span>
          <span>المحدد: {selectedProductIds.size} صنف</span>
        </div>
      </div>

      {/* Interactive Price Matrix Table */}
      <div className="flex-1 overflow-auto bg-white rounded-xl border border-slate-200 shadow-2xs">
        <table className="w-full text-right text-xs border-collapse">
          <thead className="bg-slate-50 text-slate-700 sticky top-0 z-10 border-b border-slate-200">
            <tr>
              <th className="p-2 w-10 text-center">
                <button
                  type="button"
                  onClick={handleToggleSelectAll}
                  className="text-slate-400 hover:text-slate-700 cursor-pointer"
                  title="تحديد الكل"
                >
                  {selectedProductIds.size === filteredProducts.length && filteredProducts.length > 0 ? (
                    <CheckSquare className="w-4 h-4 text-sky-600" />
                  ) : (
                    <Square className="w-4 h-4" />
                  )}
                </button>
              </th>
              <th className="p-2 w-28">كود / باركود</th>
              <th className="p-2">اسم الصنف</th>
              <th className="p-2 w-24">التصنيف</th>
              <th className="p-2 w-20 text-center">الوحدة</th>
              <th className="p-2 w-28 text-left">سعر الشراء (التكلفة)</th>
              <th className="p-2 w-28 text-left">سعر البيع الحالي</th>
              <th className="p-2 w-32 text-left bg-sky-50 text-sky-800 font-bold border-x border-sky-100">
                السعر الجديد المقترح
              </th>
              <th className="p-2 w-28 text-left">هامش الربح (%)</th>
              <th className="p-2 w-28 text-left">فارق السعر</th>
              <th className="p-2 w-24 text-center">الحالة</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {filteredProducts.length === 0 ? (
              <tr>
                <td colSpan={11} className="text-center py-16 text-slate-400 bg-slate-50/50">
                  لا توجد أصناف مطابقة لشروط البحث الحالية.
                </td>
              </tr>
            ) : (
              filteredProducts.map((p) => {
                const isSelected = selectedProductIds.has(p.id);
                const draft = priceDrafts[p.id];
                const proposedPrice = draft?.newSalePrice ?? p.salePrice;
                const isModified = draft?.isModified ?? false;

                // Profit calculation
                const cost = p.costPrice || 1;
                const profitAmount = proposedPrice - cost;
                const profitMarginPercent = Math.round((profitAmount / cost) * 100);

                // Difference from current price
                const priceDiff = proposedPrice - p.salePrice;
                const diffPercent = p.salePrice > 0 ? Math.round((priceDiff / p.salePrice) * 100) : 0;

                // Floor Price Alert: selling price <= cost price
                const isBelowCost = proposedPrice <= cost;

                return (
                  <tr
                    key={p.id}
                    className={`transition-colors ${
                      isModified
                        ? 'bg-amber-50/70 hover:bg-amber-50'
                        : isSelected
                        ? 'bg-sky-50/70 hover:bg-sky-50'
                        : 'hover:bg-slate-50'
                    }`}
                  >
                    {/* Checkbox */}
                    <td className="p-2 text-center">
                      <button
                        type="button"
                        onClick={() => handleToggleSelectOne(p.id)}
                        className="text-slate-400 hover:text-slate-700 cursor-pointer"
                      >
                        {isSelected ? (
                          <CheckSquare className="w-4 h-4 text-sky-600" />
                        ) : (
                          <Square className="w-4 h-4 text-slate-400" />
                        )}
                      </button>
                    </td>

                    {/* Code & Barcode */}
                    <td className="p-2 font-mono text-[11px] text-slate-500">
                      <div>{p.code}</div>
                      <div className="text-[10px] text-slate-400">{p.barcode}</div>
                    </td>

                    {/* Name */}
                    <td className="p-2 font-semibold text-slate-900">
                      <div className="flex items-center gap-1.5">
                        <span>{p.name}</span>
                        {p.units && p.units.length > 1 && (
                          <span
                            className="text-[9px] bg-slate-100 text-slate-600 border border-slate-200 px-1.5 py-0.2 rounded"
                            title={`${p.units.length} وحدات بيع تابعة`}
                          >
                            +{p.units.length - 1} وحدات
                          </span>
                        )}
                      </div>
                    </td>

                    {/* Category */}
                    <td className="p-2 text-slate-500 text-[11px]">
                      {p.category}
                    </td>

                    {/* Unit */}
                    <td className="p-2 text-center">
                      <span className="bg-slate-100 text-slate-700 px-2 py-0.5 rounded text-[11px] font-medium border border-slate-200">
                        {p.unit}
                      </span>
                    </td>

                    {/* Cost Price */}
                    <td className="p-2 text-left font-mono font-bold text-slate-700">
                      {p.costPrice.toLocaleString()} {currency}
                    </td>

                    {/* Current Sale Price */}
                    <td className="p-2 text-left font-mono font-medium text-slate-500">
                      {p.salePrice.toLocaleString()} {currency}
                    </td>

                    {/* Proposed New Sale Price (Editable Cell) */}
                    <td className="p-1.5 text-left bg-sky-50/40 border-x border-sky-100">
                      <div className="relative">
                        <input
                          type="number"
                          min={0}
                          value={proposedPrice}
                          onChange={(e) => handlePriceChange(p.id, parseFloat(e.target.value) || 0)}
                          className={`w-full bg-white border rounded px-2 py-1 font-mono font-bold text-left focus:outline-none focus:ring-2 ${
                            isBelowCost
                              ? 'border-rose-400 text-rose-700 focus:ring-rose-400'
                              : isModified
                              ? 'border-amber-400 text-amber-800 focus:ring-amber-400'
                              : 'border-slate-300 text-emerald-700 focus:ring-sky-400'
                          }`}
                        />
                      </div>
                    </td>

                    {/* Profit Margin % with visual gauge */}
                    <td className="p-2 text-left">
                      <div className="flex items-center gap-1.5 justify-end">
                        <span
                          className={`font-mono font-bold text-[11px] px-1.5 py-0.5 rounded border ${
                            profitMarginPercent <= 0
                              ? 'bg-rose-50 text-rose-700 border-rose-200'
                              : profitMarginPercent < 15
                              ? 'bg-amber-50 text-amber-700 border-amber-200'
                              : 'bg-emerald-50 text-emerald-700 border-emerald-200'
                          }`}
                        >
                          {profitMarginPercent > 0 ? `+${profitMarginPercent}%` : `${profitMarginPercent}%`}
                        </span>
                      </div>
                    </td>

                    {/* Difference */}
                    <td className="p-2 text-left font-mono text-[11px]">
                      {priceDiff === 0 ? (
                        <span className="text-slate-400">لا تغيير</span>
                      ) : (
                        <span
                          className={`flex items-center gap-0.5 justify-end font-bold ${
                            priceDiff > 0 ? 'text-emerald-600' : 'text-rose-600'
                          }`}
                        >
                          {priceDiff > 0 ? <ArrowUpRight className="w-3.5 h-3.5" /> : <ArrowDownRight className="w-3.5 h-3.5" />}
                          <span>{priceDiff > 0 ? `+${priceDiff.toLocaleString()}` : priceDiff.toLocaleString()}</span>
                          <span className="text-[9px] opacity-75">({diffPercent}%)</span>
                        </span>
                      )}
                    </td>

                    {/* Status Badge */}
                    <td className="p-2 text-center">
                      {isBelowCost ? (
                        <span
                          className="bg-rose-50 text-rose-700 border border-rose-200 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1"
                          title="تنبيه: سعر البيع أقل من أو مساوٍ لسعر تكلفة الشراء!"
                        >
                          <AlertTriangle className="w-3 h-3 text-rose-500" />
                          <span>خسارة</span>
                        </span>
                      ) : isModified ? (
                        <span className="bg-amber-50 text-amber-700 border border-amber-200 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                          <span>معدل</span>
                        </span>
                      ) : (
                        <span className="text-slate-400 text-[10px]">معتمد</span>
                      )}
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Bottom Hint Banner */}
      <div className="bg-white px-3 py-2 rounded-lg border border-slate-200 flex items-center justify-between gap-2 text-xs text-slate-500 shrink-0">
        <div className="flex items-center gap-2">
          <HelpCircle className="w-3.5 h-3.5 text-sky-600" />
          <span>
            <strong className="text-slate-700">نصيحة تجارية:</strong> عند اعتماد أسعار جديدة للصنف الأساسي، يقوم النظام تلقائياً بتحديث أسعار الوحدات الفرعية (الكرتون والباكت) للحفاظ على التوازن والنسب التشجيعية.
          </span>
        </div>
        <div className="font-mono text-[11px] text-slate-500">
          إجمالي الأصناف: {products.length} | الأصناف المفلترة: {filteredProducts.length}
        </div>
      </div>
    </div>
  );
};
