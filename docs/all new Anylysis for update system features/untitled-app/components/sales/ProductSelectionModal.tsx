'use client';

import React, { useState, useMemo, useEffect, useRef, useCallback } from 'react';
import { 
  Search, 
  Plus, 
  Check, 
  Layers, 
  Tag, 
  Package, 
  Calendar, 
  DollarSign, 
  Percent, 
  AlertTriangle, 
  X, 
  CheckCircle2, 
  ArrowRight,
  Info,
  Maximize2,
  Minimize2,
  RefreshCw,
  Sparkles
} from 'lucide-react';
import { ProductItem, UnitDefinition } from '@/types/pos';

export interface ProductSelectionResult {
  product: ProductItem;
  selectedUnit: string;
  unitFactor: number;
  quantity: number;
  unitPrice: number;
  unitCost: number;
  discount: number;
  discountType: 'AMOUNT' | 'PERCENT';
  total: number;
  expiryDate?: string;
  batchNo?: string;
  notes?: string;
  updateMasterPrice?: boolean;
}

export interface ProductSelectionModalProps {
  isOpen: boolean;
  products: ProductItem[];
  currency?: string;
  initialSelectedProductId?: string;
  activeWarehouseName?: string;
  onConfirmItem: (result: ProductSelectionResult, stayOpen?: boolean) => void;
  onClose: () => void;
}

export const ProductSelectionModal: React.FC<ProductSelectionModalProps> = ({
  isOpen,
  products,
  currency = 'ر.ي',
  initialSelectedProductId,
  activeWarehouseName = 'المستودع الرئيسي',
  onConfirmItem,
  onClose,
}) => {
  // Search & Categories
  const [searchText, setSearchText] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<string>('ALL');
  
  // Selected Product in the browser
  const initialProduct = useMemo(() => {
    if (initialSelectedProductId) {
      return products.find(p => p.id === initialSelectedProductId) || products[0] || null;
    }
    return products[0] || null;
  }, [initialSelectedProductId, products]);

  const [selectedProduct, setSelectedProduct] = useState<ProductItem | null>(() => initialProduct);
  const activeProduct = selectedProduct || initialProduct;

  // Editable Form fields (Loaded with defaults from DB)
  const [selectedUnit, setSelectedUnit] = useState<string>(() => activeProduct?.unit || 'حبة');
  const [unitFactor, setUnitFactor] = useState<number>(1);
  const [quantity, setQuantity] = useState<number>(1);
  const [unitPrice, setUnitPrice] = useState<number>(() => activeProduct?.salePrice || 0);
  const [unitCost, setUnitCost] = useState<number>(() => activeProduct?.costPrice || 0);
  const [discountValue, setDiscountValue] = useState<number>(0);
  const [discountType, setDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [expiryDate, setExpiryDate] = useState<string>(() => activeProduct?.expiryDate || '2026-12-31');
  const [batchNo, setBatchNo] = useState<string>(() => activeProduct?.batchNo || 'BATCH-01');
  const [notes, setNotes] = useState<string>('');
  const [updateMasterPrice, setUpdateMasterPrice] = useState<boolean>(false);

  // UI Toast for rapid continuous addition
  const [toastMessage, setToastMessage] = useState<string | null>(null);
  const searchInputRef = useRef<HTMLInputElement>(null);

  // Extract all categories
  const categories = useMemo(() => {
    const set = new Set<string>();
    products.forEach((p) => {
      if (p.category) set.add(p.category);
    });
    return ['ALL', ...Array.from(set)];
  }, [products]);

  // Filter products by search & category
  const filteredProducts = useMemo(() => {
    const q = searchText.toLowerCase().trim();
    return products.filter((p) => {
      const matchCat = selectedCategory === 'ALL' || p.category === selectedCategory;
      if (!matchCat) return false;
      if (!q) return true;
      return (
        p.name.toLowerCase().includes(q) ||
        p.code.toLowerCase().includes(q) ||
        (p.barcode && p.barcode.toLowerCase().includes(q)) ||
        (p.category && p.category.toLowerCase().includes(q))
      );
    });
  }, [products, searchText, selectedCategory]);

  // Function to load all defaults from product database record
  const loadProductDefaults = (prod: ProductItem) => {
    setSelectedProduct(prod);
    const defaultUnit = prod.unit || 'حبة';
    setSelectedUnit(defaultUnit);
    setUnitFactor(1);
    setQuantity(1);
    setUnitPrice(prod.salePrice || 0);
    setUnitCost(prod.costPrice || 0);
    setDiscountValue(0);
    setDiscountType('AMOUNT');
    setExpiryDate(prod.expiryDate || '2026-12-31');
    setBatchNo(prod.batchNo || 'BATCH-01');
    setNotes('');
    setUpdateMasterPrice(false);
  };

  // Handle unit change - recalculate default price and factor
  const handleUnitChange = (newUnitName: string) => {
    if (!selectedProduct) return;
    setSelectedUnit(newUnitName);

    // Look for unit definition in product units
    const unitDef = selectedProduct.units?.find(u => u.name === newUnitName);
    if (unitDef) {
      setUnitFactor(unitDef.factor || 1);
      setUnitPrice(unitDef.price || (selectedProduct.salePrice * (unitDef.factor || 1)));
      setUnitCost(unitDef.cost || (selectedProduct.costPrice * (unitDef.factor || 1)));
    } else {
      // Basic unit
      setUnitFactor(1);
      setUnitPrice(selectedProduct.salePrice);
      setUnitCost(selectedProduct.costPrice);
    }
  };

  // Calculated Line Total
  const calculatedDiscountAmount = useMemo(() => {
    const gross = quantity * unitPrice;
    if (discountType === 'PERCENT') {
      return (gross * Math.min(100, Math.max(0, discountValue))) / 100;
    }
    return Math.min(gross, Math.max(0, discountValue));
  }, [quantity, unitPrice, discountValue, discountType]);

  const netLineTotal = useMemo(() => {
    const gross = quantity * unitPrice;
    return Math.max(0, gross - calculatedDiscountAmount);
  }, [quantity, unitPrice, calculatedDiscountAmount]);

  const handleConfirm = useCallback((stayOpen: boolean = false) => {
    if (!selectedProduct) return;

    const result: ProductSelectionResult = {
      product: selectedProduct,
      selectedUnit,
      unitFactor,
      quantity: Number(quantity) || 1,
      unitPrice: Number(unitPrice) || 0,
      unitCost: Number(unitCost) || 0,
      discount: calculatedDiscountAmount,
      discountType,
      total: Number(netLineTotal.toFixed(4)),
      expiryDate,
      batchNo,
      notes,
      updateMasterPrice,
    };

    onConfirmItem(result, stayOpen);

    if (stayOpen) {
      setToastMessage(`تم إدراج الصنف [${selectedProduct.name}] بنجاح في الفاتورة! يمكنك اختيار صنف آخر.`);
      setTimeout(() => setToastMessage(null), 3500);
      setQuantity(1);
      setDiscountValue(0);
      setNotes('');
    } else {
      onClose();
    }
  }, [
    selectedProduct,
    selectedUnit,
    unitFactor,
    quantity,
    unitPrice,
    unitCost,
    calculatedDiscountAmount,
    discountType,
    netLineTotal,
    expiryDate,
    batchNo,
    notes,
    updateMasterPrice,
    onConfirmItem,
    onClose
  ]);

  // Keyboard shortcut listener
  useEffect(() => {
    if (!isOpen) return;
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        e.preventDefault();
        onClose();
      } else if (e.key === 'F8') {
        e.preventDefault();
        handleConfirm(true); // Stay open
      } else if (e.key === 'Enter' && (e.ctrlKey || e.metaKey)) {
        e.preventDefault();
        handleConfirm(false); // Close
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isOpen, onClose, handleConfirm]);

  if (!isOpen) return null;

  return (
    <div 
      className="fixed inset-0 z-50 flex items-center justify-center p-2 sm:p-4 bg-slate-900/70 backdrop-blur-xs select-none"
      dir="rtl"
    >
      <div className="bg-white rounded-2xl w-full max-w-6xl max-h-[92vh] h-[90vh] shadow-2xl border border-slate-300 flex flex-col overflow-hidden text-right">
        {/* ============================================================== */}
        {/* MODAL HEADER                                                    */}
        {/* ============================================================== */}
        <div className="bg-white border-b border-slate-200 text-slate-800 px-5 py-3.5 flex items-center justify-between shrink-0 shadow-2xs">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-xl bg-sky-600 flex items-center justify-center font-black text-white shadow-xs text-sm">
              F3
            </div>
            <div>
              <div className="flex items-center gap-2">
                <h2 className="text-base font-bold text-slate-900">شاشة استعراض واختيار الأصناف والبيانات المقترحة</h2>
                <span className="text-[11px] bg-sky-50 text-sky-700 border border-sky-200 px-2 py-0.5 rounded-full font-bold">
                  تحميل تلقائي وتعديل مباشر
                </span>
              </div>
              <p className="text-xs text-slate-500">
                اختر الصنف لتعبئة كافة بياناته الافتراضية من قاعدة البيانات وتعديل السعر والكمية والوحدة قبل الإدراج
              </p>
            </div>
          </div>

          <div className="flex items-center gap-3">
            <div className="hidden sm:flex items-center gap-1.5 bg-slate-50 px-3 py-1 rounded-lg border border-slate-200 text-xs text-slate-600">
              <span>المخزن:</span>
              <strong className="text-emerald-600 font-bold">{activeWarehouseName}</strong>
            </div>

            <button
              type="button"
              onClick={onClose}
              className="text-slate-400 hover:text-slate-700 p-1.5 rounded-lg hover:bg-slate-100 transition cursor-pointer"
              title="إغلاق [Esc]"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {/* Toast Alert */}
        {toastMessage && (
          <div className="bg-emerald-600 text-white px-4 py-2 text-xs font-bold flex items-center justify-between shrink-0 shadow-inner animate-in fade-in slide-in-from-top-2">
            <div className="flex items-center gap-2">
              <CheckCircle2 className="w-4 h-4" />
              <span>{toastMessage}</span>
            </div>
            <button 
              type="button" 
              onClick={() => setToastMessage(null)}
              className="text-emerald-200 hover:text-white text-xs cursor-pointer font-bold"
            >
              ✕
            </button>
          </div>
        )}

        {/* ============================================================== */}
        {/* MAIN BODY: 2 COLUMNS (Browser on right, Edit Pane on left)     */}
        {/* ============================================================== */}
        <div className="flex-1 flex flex-col lg:flex-row overflow-hidden bg-slate-100 divide-y lg:divide-y-0 lg:divide-x lg:divide-x-reverse divide-slate-200">
          
          {/* ------------------------------------------------------------ */}
          {/* COLUMN 1: PRODUCT BROWSER & CATALOG (Right 60%)              */}
          {/* ------------------------------------------------------------ */}
          <div className="flex-1 lg:w-7/12 flex flex-col overflow-hidden bg-white p-3 sm:p-4 gap-3">
            {/* Search Bar & Category Filters */}
            <div className="flex flex-col gap-2.5 shrink-0">
              <div className="relative">
                <Search className="w-5 h-5 text-slate-400 absolute right-3 top-2.5" />
                <input
                  ref={searchInputRef}
                  type="text"
                  autoFocus
                  value={searchText}
                  onChange={(e) => setSearchText(e.target.value)}
                  placeholder="ابحث باسم الصنف، الباركود، الكود، أو التصنيف..."
                  className="w-full bg-slate-50 border-2 border-slate-200 focus:border-sky-500 focus:bg-white rounded-xl pr-10 pl-10 py-2 text-xs sm:text-sm font-bold text-slate-800 outline-hidden transition shadow-2xs"
                />
                {searchText && (
                  <button
                    type="button"
                    onClick={() => setSearchText('')}
                    className="absolute left-3 top-2.5 text-slate-400 hover:text-slate-600 cursor-pointer p-0.5"
                  >
                    <X className="w-4 h-4" />
                  </button>
                )}
              </div>

              {/* Category Pills */}
              <div className="flex items-center gap-1.5 overflow-x-auto pb-1 text-xs">
                {categories.map((cat) => (
                  <button
                    key={cat}
                    type="button"
                    onClick={() => setSelectedCategory(cat)}
                    className={`px-3 py-1 rounded-lg font-bold transition whitespace-nowrap cursor-pointer text-xs ${
                      selectedCategory === cat
                        ? 'bg-sky-700 text-white shadow-xs'
                        : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200'
                    }`}
                  >
                    {cat === 'ALL' ? 'جميع الأصناف' : cat}
                  </button>
                ))}
              </div>
            </div>

            {/* Products Table */}
            <div className="flex-1 overflow-y-auto border border-slate-200 rounded-xl shadow-inner">
              <table className="w-full text-right text-xs border-collapse">
                <thead className="bg-slate-100 text-slate-700 sticky top-0 font-bold border-b border-slate-200 z-10">
                  <tr>
                    <th className="py-2.5 px-3 border-l border-slate-200">كود / باركود</th>
                    <th className="py-2.5 px-3 border-l border-slate-200">اسم الصنف</th>
                    <th className="py-2.5 px-2 border-l border-slate-200 text-center">التصنيف</th>
                    <th className="py-2.5 px-2 border-l border-slate-200 text-center">الرصيد المتاح</th>
                    <th className="py-2.5 px-3 border-l border-slate-200 text-left">السعر المقترح</th>
                    <th className="py-2.5 px-2 text-center w-20">اختيار</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {filteredProducts.length === 0 ? (
                    <tr>
                      <td colSpan={6} className="text-center py-10 text-slate-400">
                        <Package className="w-8 h-8 mx-auto mb-2 opacity-40" />
                        <p className="font-bold">لا توجد أصناف مطابقة لكلمات البحث</p>
                      </td>
                    </tr>
                  ) : (
                    filteredProducts.map((prod) => {
                      const isSelected = selectedProduct?.id === prod.id;
                      return (
                        <tr
                          key={prod.id}
                          onClick={() => loadProductDefaults(prod)}
                          className={`cursor-pointer transition ${
                            isSelected
                              ? 'bg-sky-50 border-r-4 border-r-sky-600 font-bold'
                              : 'hover:bg-slate-50'
                          }`}
                        >
                          {/* Code & Barcode */}
                          <td className="py-2 px-3 border-l border-slate-100 font-mono text-[11px] text-slate-600">
                            <div>{prod.code}</div>
                            {prod.barcode && <div className="text-[10px] text-slate-400">{prod.barcode}</div>}
                          </td>

                          {/* Name */}
                          <td className="py-2 px-3 border-l border-slate-100 font-bold text-slate-900">
                            <div className="flex items-center gap-1.5">
                              {isSelected && <span className="w-2 h-2 rounded-full bg-sky-600 shrink-0"></span>}
                              <span>{prod.name}</span>
                            </div>
                          </td>

                          {/* Category */}
                          <td className="py-2 px-2 border-l border-slate-100 text-center text-slate-500 text-[11px]">
                            {prod.category || 'عام'}
                          </td>

                          {/* Stock */}
                          <td className="py-2 px-2 border-l border-slate-100 text-center font-mono font-bold">
                            <span className={prod.stock > 10 ? 'text-emerald-700' : 'text-amber-700'}>
                              {prod.stock} {prod.unit}
                            </span>
                          </td>

                          {/* Price */}
                          <td className="py-2 px-3 border-l border-slate-100 text-left font-mono font-black text-sky-900">
                            {prod.salePrice.toLocaleString()} <span className="text-[10px] font-normal text-slate-500">{currency}</span>
                          </td>

                          {/* Action Button */}
                          <td className="py-2 px-2 text-center">
                            <button
                              type="button"
                              onClick={(e) => {
                                e.stopPropagation();
                                loadProductDefaults(prod);
                              }}
                              className={`px-2 py-1 rounded text-[11px] font-bold transition cursor-pointer ${
                                isSelected
                                  ? 'bg-sky-600 text-white'
                                  : 'bg-slate-100 hover:bg-slate-200 text-slate-700'
                              }`}
                            >
                              {isSelected ? 'محدد ✓' : 'اختيار'}
                            </button>
                          </td>
                        </tr>
                      );
                    })
                  )}
                </tbody>
              </table>
            </div>

            {/* Bottom Status bar */}
            <div className="flex items-center justify-between text-[11px] text-slate-500 shrink-0 px-1">
              <span>الأصناف المعروضة: <strong>{filteredProducts.length}</strong> من أصل {products.length}</span>
              <span className="hidden sm:inline">انقر على أي سطر لتحميل بيانات الصنف الافتراضية والتعديل</span>
            </div>
          </div>

          {/* ------------------------------------------------------------ */}
          {/* COLUMN 2: EDITABLE DEFAULTS & ADJUSTMENT PANEL (Left 40%)    */}
          {/* ------------------------------------------------------------ */}
          <div className="flex-1 lg:w-5/12 flex flex-col overflow-y-auto bg-slate-50 p-4 gap-3.5">
            {selectedProduct ? (
              <>
                {/* Active Product Banner */}
                <div className="bg-white border-2 border-sky-400 rounded-xl p-3 shadow-xs flex flex-col gap-2">
                  <div className="flex items-start justify-between">
                    <div>
                      <span className="text-[10px] bg-sky-100 text-sky-800 px-2 py-0.5 rounded font-bold">
                        الصنف المختار للتحميل
                      </span>
                      <h3 className="text-base font-black text-slate-900 mt-1">
                        {selectedProduct.name}
                      </h3>
                      <div className="flex items-center gap-3 text-xs text-slate-500 font-mono mt-0.5">
                        <span>كود: {selectedProduct.code}</span>
                        {selectedProduct.barcode && <span>باركود: {selectedProduct.barcode}</span>}
                      </div>
                    </div>

                    <div className="text-left">
                      <div className="text-xs text-slate-500">الرصيد في {activeWarehouseName}</div>
                      <div className="text-sm font-black font-mono text-emerald-700">
                        {selectedProduct.stock} {selectedProduct.unit}
                      </div>
                    </div>
                  </div>

                  {/* Pricing Context Ribbon from DB */}
                  <div className="grid grid-cols-3 gap-2 bg-slate-50 rounded-lg p-2 text-center text-xs border border-slate-200">
                    <div>
                      <span className="text-[10px] text-slate-500 block">التكلفة (DB)</span>
                      <strong className="font-mono text-slate-700">{selectedProduct.costPrice || 0} {currency}</strong>
                    </div>
                    <div>
                      <span className="text-[10px] text-slate-500 block">السعر الافتراضي (DB)</span>
                      <strong className="font-mono text-sky-800">{selectedProduct.salePrice || 0} {currency}</strong>
                    </div>
                    <div>
                      <span className="text-[10px] text-slate-500 block">أدنى سعر مسموح</span>
                      <strong className="font-mono text-amber-800">{selectedProduct.minSalePrice || selectedProduct.costPrice || 0} {currency}</strong>
                    </div>
                  </div>
                </div>

                {/* Form Controls to customize loaded defaults */}
                <div className="bg-white border border-slate-200 rounded-xl p-3.5 shadow-xs flex flex-col gap-3">
                  <div className="font-bold text-xs text-slate-800 border-b border-slate-100 pb-1 flex items-center justify-between">
                    <span>تعديل البيانات المقترحة للصنف</span>
                    <span className="text-[10px] text-slate-500">تعديل حر ومباشر</span>
                  </div>

                  {/* 1. Unit & Factor Selection */}
                  <div>
                    <label className="block text-xs font-bold text-slate-700 mb-1 flex items-center justify-between">
                      <span>وحدة القياس:</span>
                      {unitFactor > 1 && (
                        <span className="text-[10px] text-sky-700 font-mono">
                          معامل التحويل: 1 {selectedUnit} = {unitFactor} {selectedProduct.unit}
                        </span>
                      )}
                    </label>
                    <select
                      value={selectedUnit}
                      onChange={(e) => handleUnitChange(e.target.value)}
                      className="w-full bg-slate-50 border border-slate-300 rounded-lg p-2 text-xs font-bold text-slate-900 focus:bg-white focus:border-sky-500 outline-hidden cursor-pointer"
                    >
                      <option value={selectedProduct.unit || 'حبة'}>
                        {selectedProduct.unit || 'حبة'} (الوحدة الأساسية - معامل 1)
                      </option>
                      {selectedProduct.units?.filter(u => u.name !== selectedProduct.unit).map((u) => (
                        <option key={u.name} value={u.name}>
                          {u.name} (معامل {u.factor} - السعر المقترح {u.price || (selectedProduct.salePrice * u.factor)} {currency})
                        </option>
                      ))}
                    </select>
                  </div>

                  {/* 2. Sale Price Input */}
                  <div>
                    <div className="flex items-center justify-between mb-1">
                      <label className="text-xs font-bold text-slate-700">
                        سعر البيع المقترح:
                      </label>
                      <label className="flex items-center gap-1 text-[11px] text-slate-600 cursor-pointer">
                        <input
                          type="checkbox"
                          checked={updateMasterPrice}
                          onChange={(e) => setUpdateMasterPrice(e.target.checked)}
                          className="w-3.5 h-3.5 rounded text-sky-600"
                        />
                        <span>تحديث السعر في بطاقة الصنف</span>
                      </label>
                    </div>
                    <div className="relative">
                      <input
                        type="number"
                        step="any"
                        value={unitPrice}
                        onChange={(e) => setUnitPrice(parseFloat(e.target.value) || 0)}
                        className="w-full bg-slate-50 border border-slate-300 focus:border-sky-500 focus:bg-white rounded-lg p-2 text-sm font-mono font-black text-slate-900 outline-hidden pl-10"
                      />
                      <span className="absolute left-3 top-2.5 text-xs text-slate-500 font-bold">
                        {currency}
                      </span>
                    </div>
                    {unitPrice < unitCost && (
                      <p className="text-[10px] text-rose-600 font-bold mt-1 flex items-center gap-1">
                        <AlertTriangle className="w-3 h-3" />
                        <span>تنبيه: سعر البيع أقل من سعر التكلفة ({unitCost} {currency})!</span>
                      </p>
                    )}
                  </div>

                  {/* 3. Quantity Input with Quick Buttons */}
                  <div>
                    <label className="block text-xs font-bold text-slate-700 mb-1">
                      الكمية المطلوبة:
                    </label>
                    <div className="flex items-center gap-2">
                      <button
                        type="button"
                        onClick={() => setQuantity(Math.max(1, quantity - 1))}
                        className="w-9 h-9 bg-slate-100 hover:bg-slate-200 rounded-lg font-black text-slate-700 text-base cursor-pointer shrink-0 transition"
                      >
                        -
                      </button>
                      <input
                        type="number"
                        min="1"
                        step="any"
                        value={quantity}
                        onChange={(e) => setQuantity(parseFloat(e.target.value) || 1)}
                        className="flex-1 bg-slate-50 border border-slate-300 focus:border-sky-500 focus:bg-white rounded-lg p-2 text-center text-sm font-mono font-black text-slate-900 outline-hidden"
                      />
                      <button
                        type="button"
                        onClick={() => setQuantity(quantity + 1)}
                        className="w-9 h-9 bg-slate-100 hover:bg-slate-200 rounded-lg font-black text-slate-700 text-base cursor-pointer shrink-0 transition"
                      >
                        +
                      </button>
                    </div>

                    {/* Quick Preset Buttons */}
                    <div className="flex items-center gap-1.5 mt-1.5">
                      {[1, 5, 10, 12, 24, 48].map((q) => (
                        <button
                          key={q}
                          type="button"
                          onClick={() => setQuantity(q)}
                          className={`flex-1 py-1 rounded text-[11px] font-bold font-mono transition cursor-pointer border ${
                            quantity === q
                              ? 'bg-sky-700 text-white border-sky-700'
                              : 'bg-slate-50 hover:bg-slate-100 text-slate-700 border-slate-200'
                          }`}
                        >
                          {q === 12 ? '12 (درزن)' : q === 24 ? '24 (كرتون)' : q}
                        </button>
                      ))}
                    </div>
                  </div>

                  {/* 4. Direct Discount Input */}
                  <div>
                    <div className="flex items-center justify-between mb-1">
                      <label className="text-xs font-bold text-slate-700">
                        الخصم المباشر على السطر:
                      </label>
                      <div className="flex items-center gap-1 text-[11px]">
                        <button
                          type="button"
                          onClick={() => setDiscountType('AMOUNT')}
                          className={`px-1.5 py-0.5 rounded font-bold cursor-pointer ${
                            discountType === 'AMOUNT'
                              ? 'bg-sky-700 text-white'
                              : 'bg-slate-100 text-slate-600'
                          }`}
                        >
                          مبلغ ({currency})
                        </button>
                        <button
                          type="button"
                          onClick={() => setDiscountType('PERCENT')}
                          className={`px-1.5 py-0.5 rounded font-bold cursor-pointer ${
                            discountType === 'PERCENT'
                              ? 'bg-sky-700 text-white'
                              : 'bg-slate-100 text-slate-600'
                          }`}
                        >
                          نسبة %
                        </button>
                      </div>
                    </div>
                    <div className="relative">
                      <input
                        type="number"
                        min="0"
                        step="any"
                        value={discountValue}
                        onChange={(e) => setDiscountValue(parseFloat(e.target.value) || 0)}
                        placeholder="0"
                        className="w-full bg-slate-50 border border-slate-300 focus:border-sky-500 focus:bg-white rounded-lg p-2 text-xs font-mono font-bold text-rose-700 outline-hidden"
                      />
                      {calculatedDiscountAmount > 0 && (
                        <span className="absolute left-2.5 top-2 text-[11px] text-rose-600 font-bold">
                          صافي الخصم: {calculatedDiscountAmount.toFixed(2)} {currency}
                        </span>
                      )}
                    </div>
                  </div>

                  {/* 5. Expiry Date & Notes */}
                  <div className="grid grid-cols-2 gap-2 text-xs">
                    <div>
                      <label className="block text-slate-600 mb-0.5 font-bold text-[11px]">
                        تاريخ الصلاحية:
                      </label>
                      <input
                        type="date"
                        value={expiryDate}
                        onChange={(e) => setExpiryDate(e.target.value)}
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg p-1.5 text-xs text-slate-800 outline-hidden"
                      />
                    </div>
                    <div>
                      <label className="block text-slate-600 mb-0.5 font-bold text-[11px]">
                        رقم التشغيلة (Batch):
                      </label>
                      <input
                        type="text"
                        value={batchNo}
                        onChange={(e) => setBatchNo(e.target.value)}
                        placeholder="BATCH-01"
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg p-1.5 text-xs text-slate-800 outline-hidden font-mono"
                      />
                    </div>
                  </div>

                  <div>
                    <label className="block text-slate-600 mb-0.5 font-bold text-[11px]">
                      ملاحظات السطر (اختياري):
                    </label>
                    <input
                      type="text"
                      value={notes}
                      onChange={(e) => setNotes(e.target.value)}
                      placeholder="أي مواصفات أو تفاصيل إضافية..."
                      className="w-full bg-slate-50 border border-slate-300 rounded-lg p-1.5 text-xs text-slate-800 outline-hidden"
                    />
                  </div>
                </div>

                {/* Net Total Card */}
                <div className="bg-sky-900 text-white rounded-xl p-3.5 shadow-md flex items-center justify-between">
                  <div>
                    <div className="text-xs text-sky-200">الإجمالي الصافي للسطر:</div>
                    <div className="text-[11px] text-sky-300">
                      ({quantity} {selectedUnit} × {unitPrice.toLocaleString()})
                      {calculatedDiscountAmount > 0 && ` - خصم ${calculatedDiscountAmount.toLocaleString()}`}
                    </div>
                  </div>
                  <div className="text-left">
                    <span className="text-xl font-mono font-black text-emerald-300">
                      {netLineTotal.toLocaleString()}
                    </span>
                    <span className="text-xs text-sky-200 mr-1">{currency}</span>
                  </div>
                </div>

                {/* Action Buttons */}
                <div className="flex flex-col gap-2 pt-1">
                  {/* Confirm & Close */}
                  <button
                    type="button"
                    onClick={() => handleConfirm(false)}
                    className="w-full py-2.5 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl font-bold text-sm shadow-md hover:shadow-lg transition flex items-center justify-center gap-2 cursor-pointer"
                  >
                    <Check className="w-5 h-5" />
                    <span>إدراج في الفاتورة وإغلاق [Ctrl+Enter]</span>
                  </button>

                  {/* Confirm & Stay Open (Continuous Add) */}
                  <button
                    type="button"
                    onClick={() => handleConfirm(true)}
                    className="w-full py-2 bg-sky-700 hover:bg-sky-600 text-white rounded-xl font-bold text-xs shadow-xs hover:shadow-md transition flex items-center justify-center gap-1.5 cursor-pointer"
                  >
                    <Plus className="w-4 h-4" />
                    <span>إدراج ومتابعة اختيار صنف آخر [F8]</span>
                  </button>
                </div>
              </>
            ) : (
              <div className="flex-1 flex flex-col items-center justify-center text-center p-6 text-slate-400">
                <Package className="w-12 h-12 mb-3 text-slate-300" />
                <h4 className="font-bold text-slate-700">لم يتم اختيار أي صنف</h4>
                <p className="text-xs text-slate-500 mt-1 max-w-xs">
                  اختر أحد الأصناف من القائمة في اليمين لتحميل كافة بياناته الافتراضية ومراجعتها هنا قبل الإدراج
                </p>
              </div>
            )}
          </div>
        </div>

        {/* Modal Footer */}
        <div className="bg-white border-t border-slate-200 px-4 py-2.5 flex items-center justify-between text-xs text-slate-600 shrink-0">
          <div className="flex items-center gap-4">
            <span>اختصارات:</span>
            <kbd className="bg-slate-100 text-slate-800 px-2 py-0.5 rounded border border-slate-300 font-mono font-bold">
              Esc = إغلاق
            </kbd>
            <kbd className="bg-slate-100 text-slate-800 px-2 py-0.5 rounded border border-slate-300 font-mono font-bold">
              F8 = إدراج ومتابعة
            </kbd>
            <kbd className="bg-slate-100 text-slate-800 px-2 py-0.5 rounded border border-slate-300 font-mono font-bold">
              Ctrl+Enter = إدراج وإنهاء
            </kbd>
          </div>

          <button
            type="button"
            onClick={onClose}
            className="px-4 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg font-bold transition cursor-pointer"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
};
