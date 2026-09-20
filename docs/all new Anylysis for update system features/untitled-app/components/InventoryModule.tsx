'use client';

import React, { useState, useEffect } from 'react';
import {
  InventoryAddProductModal,
  InventoryBarcodeModal,
  InventoryFifoModal,
} from './inventory';
import { 
  ProductItem, 
  UnitDefinition, 
  Warehouse, 
  StockTransfer, 
  StockTransferStatus,
  StocktakingSession,
  StockAdjustment,
  Invoice,
  JournalEntry,
  MainTabType,
} from '@/types/pos';
import { generateNextId } from '@/lib/idGenerator';
import { getProductStockInWarehouse, getOrInitFifoLayers, getProductValuationMethod } from '@/lib/mock';
import { WarehouseTransfersView } from '@/components/WarehouseTransfersView';
import { StocktakingAndCardsView } from '@/components/StocktakingAndCardsView';
import { QuickPriceUpdateView } from '@/components/QuickPriceUpdateView';
import { 
  Package, 
  Plus, 
  Search, 
  Filter, 
  Edit, 
  Trash2, 
  Barcode, 
  AlertTriangle, 
  Calendar, 
  Check, 
  Printer, 
  ArrowUpDown,
  Layers,
  FileSpreadsheet,
  Tags,
  CheckCircle2,
  XCircle,
  HelpCircle,
  Sparkles,
  Download,
  Upload,
  RefreshCw,
  Eye,
  ArrowLeftRight,
  Building2,
  ClipboardCheck,
  SlidersHorizontal,
  BookOpen,
  ShieldCheck,
  ShieldOff,
  Sliders,
  TrendingUp,
  Percent,
  Truck,
  FileText,
  Receipt,
  Coins,
  UploadCloud,
  ChevronLeft,
  ChevronRight,
} from 'lucide-react';

interface InventoryModuleProps {
  products: ProductItem[];
  warehouses: Warehouse[];
  transfers: StockTransfer[];
  salesInvoices?: Invoice[];
  purchaseInvoices?: Invoice[];
  stocktakes?: StocktakingSession[];
  stockAdjustments?: StockAdjustment[];
  journalEntries?: JournalEntry[];
  onAddProduct: (item: ProductItem) => void;
  onUpdateProduct: (item: ProductItem) => void;
  onDeleteProduct: (itemId: string) => void;
  onAddTransfer: (transfer: StockTransfer) => void;
  onUpdateTransferStatus: (transferId: string, newStatus: StockTransferStatus, notes?: string) => void;
  onAddWarehouse: (warehouse: Warehouse) => void;
  onUpdateWarehouse: (warehouse: Warehouse) => void;
  onUpdateProductsStock: (updatedProducts: ProductItem[]) => void;
  onAddStocktake?: (session: StocktakingSession) => void;
  onUpdateStocktake?: (session: StocktakingSession) => void;
  onPostStockAdjustment?: (adjustment: StockAdjustment, updatedProducts: ProductItem[]) => void;
  currency: string;
  cashierName?: string;
  initialTab?: 'PRODUCTS' | 'PRICE_UPDATE' | 'TRANSFERS' | 'STOCKTAKE' | 'ADJUSTMENTS' | 'CARDS';
  onSelectTab?: (tab: MainTabType) => void;
}

export const InventoryModule: React.FC<InventoryModuleProps> = ({
  products,
  warehouses,
  transfers,
  salesInvoices = [],
  purchaseInvoices = [],
  stocktakes = [],
  stockAdjustments = [],
  journalEntries = [],
  onAddProduct,
  onUpdateProduct,
  onDeleteProduct,
  onAddTransfer,
  onUpdateTransferStatus,
  onAddWarehouse,
  onUpdateWarehouse,
  onUpdateProductsStock,
  onAddStocktake = () => {},
  onUpdateStocktake = () => {},
  onPostStockAdjustment = () => {},
  currency,
  cashierName = 'المسؤول الإداري',
  initialTab = 'PRODUCTS',
  onSelectTab,
}) => {
  const [activeInventoryTab, setActiveInventoryTab] = useState<
    'PRODUCTS' | 'PRICE_UPDATE' | 'TRANSFERS' | 'STOCKTAKE' | 'ADJUSTMENTS' | 'CARDS'
  >(initialTab);
  const [selectedWarehouseFilter, setSelectedWarehouseFilter] = useState<string>('ALL');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [selectedCategory, setSelectedCategory] = useState<string>('ALL');
  const [filterLowStockOnly, setFilterLowStockOnly] = useState<boolean>(false);
  const [filterExpiringSoon, setFilterExpiringSoon] = useState<boolean>(false);
  const [showItemModal, setShowItemModal] = useState<boolean>(false);
  const [editingItem, setEditingItem] = useState<ProductItem | null>(null);
  const [barcodePreviewItem, setBarcodePreviewItem] = useState<ProductItem | null>(null);
  const [selectedUnitForBarcode, setSelectedUnitForBarcode] = useState<UnitDefinition | null>(null);

  // Form State for Add / Edit
  const [formCode, setFormCode] = useState<string>('');
  const [formBarcode, setFormBarcode] = useState<string>('');
  const [formName, setFormName] = useState<string>('');
  const [formCategory, setFormCategory] = useState<string>('مواد غذائية');

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F1') {
        e.preventDefault();
        setActiveInventoryTab('PRODUCTS');
      } else if (e.key === 'F2') {
        e.preventDefault();
        setActiveInventoryTab('TRANSFERS');
      } else if (e.key === 'F3') {
        e.preventDefault();
        setActiveInventoryTab('STOCKTAKE');
      } else if (e.key === 'F4') {
        e.preventDefault();
        setActiveInventoryTab('ADJUSTMENTS');
      } else if (e.key === 'F5') {
        e.preventDefault();
        setActiveInventoryTab('CARDS');
      } else if (e.key === 'F6') {
        e.preventDefault();
        setActiveInventoryTab('PRICE_UPDATE');
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);
  const [formBaseUnit, setFormBaseUnit] = useState<string>('حبة');
  const [formCostPrice, setFormCostPrice] = useState<number>(0);
  const [formSalePrice, setFormSalePrice] = useState<number>(0);
  const [formMinSalePrice, setFormMinSalePrice] = useState<number>(0);
  const [formStock, setFormStock] = useState<number>(100);
  const [formMinStock, setFormMinStock] = useState<number>(15);
  const [formHasExpiryDate, setFormHasExpiryDate] = useState<boolean>(true);
  const [formValuationMethod, setFormValuationMethod] = useState<'FIFO' | 'FEFO'>('FEFO');
  const [formBatchNo, setFormBatchNo] = useState<string>('');
  const [formExpiryDate, setFormExpiryDate] = useState<string>('');
  const [formLocation, setFormLocation] = useState<string>('المستودع الرئيسي');
  const [fifoModalProduct, setFifoModalProduct] = useState<ProductItem | null>(null);
  const [expiryFilter, setExpiryFilter] = useState<'ALL' | 'WITH_EXPIRY' | 'WITHOUT_EXPIRY'>('ALL');
  
  // Dynamic Multi-Units List in Form
  const [formUnits, setFormUnits] = useState<UnitDefinition[]>([
    { name: 'حبة', factor: 1, price: 0, cost: 0, barcode: '' }
  ]);

  const categories = [
    'ALL', 
    'مواد غذائية', 
    'مشروبات ومياه', 
    'منتجات ألبان', 
    'حلويات وبسكويتات', 
    'معلبات', 
    'منظفات', 
    'بهارات وتوابل',
    'أدوات منزلية'
  ];

  // Helper to check expiring within 60 days (only if tracked)
  const isExpiringSoon = (item: ProductItem) => {
    if (item.hasExpiryDate === false || !item.expiryDate) return false;
    const expDate = new Date(item.expiryDate);
    const today = new Date();
    const diffDays = Math.ceil((expDate.getTime() - today.getTime()) / (1000 * 3600 * 24));
    return diffDays >= 0 && diffDays <= 60;
  };

  const isExpired = (item: ProductItem) => {
    if (item.hasExpiryDate === false || !item.expiryDate) return false;
    const expDate = new Date(item.expiryDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return expDate < today;
  };

  // Filter products
  const filtered = products.filter((p) => {
    const matchCategory = selectedCategory === 'ALL' || p.category === selectedCategory;
    const matchQuery =
      searchQuery === '' ||
      p.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      p.barcode.includes(searchQuery) ||
      p.code.includes(searchQuery) ||
      (p.units && p.units.some(u => u.barcode.includes(searchQuery) || u.name.toLowerCase().includes(searchQuery.toLowerCase())));
    const matchLowStock = !filterLowStockOnly || p.stock <= p.minStock;
    const matchExpiring = !filterExpiringSoon || isExpiringSoon(p);
    const matchExpiryType =
      expiryFilter === 'ALL' ||
      (expiryFilter === 'WITH_EXPIRY' && p.hasExpiryDate !== false) ||
      (expiryFilter === 'WITHOUT_EXPIRY' && p.hasExpiryDate === false);
    return matchCategory && matchQuery && matchLowStock && matchExpiring && matchExpiryType;
  });

  const handleOpenAddModal = () => {
    setEditingItem(null);
    const nextCode = (products.length + 1001).toString();
    const defaultBarcode = `629100100${products.length + 10}`;
    setFormCode(nextCode);
    setFormBarcode(defaultBarcode);
    setFormName('');
    setFormCategory('مواد غذائية');
    setFormBaseUnit('حبة');
    setFormCostPrice(1000);
    setFormSalePrice(1200);
    setFormMinSalePrice(1100);
    setFormStock(50);
    setFormMinStock(10);
    setFormHasExpiryDate(true);
    setFormValuationMethod('FEFO');
    setFormBatchNo(`BATCH-2026-${products.length + 101}`);
    setFormExpiryDate('2027-06-30');
    setFormLocation('المستودع الرئيسي - رف A-1');
    
    // Seed standard base unit + packet unit
    setFormUnits([
      { name: 'حبة', factor: 1, price: 1200, cost: 1000, barcode: defaultBarcode },
      { name: 'باكت (12 حبة)', factor: 12, price: 14000, cost: 11800, barcode: `629100100${products.length + 11}` },
      { name: 'كرتون (48 حبة)', factor: 48, price: 55000, cost: 47000, barcode: `629100100${products.length + 12}` }
    ]);
    setShowItemModal(true);
  };

  const handleOpenEditModal = (item: ProductItem) => {
    setEditingItem(item);
    setFormCode(item.code);
    setFormBarcode(item.barcode);
    setFormName(item.name);
    setFormCategory(item.category);
    setFormBaseUnit(item.unit || 'حبة');
    setFormCostPrice(item.costPrice);
    setFormSalePrice(item.salePrice);
    setFormMinSalePrice(item.minSalePrice || item.salePrice * 0.95);
    setFormStock(item.stock);
    setFormMinStock(item.minStock);
    setFormHasExpiryDate(item.hasExpiryDate !== false);
    setFormValuationMethod(item.valuationMethod || (item.hasExpiryDate !== false ? 'FEFO' : 'FIFO'));
    setFormBatchNo(item.batchNo || '');
    setFormExpiryDate(item.expiryDate || '');
    setFormLocation(item.location || 'المستودع الرئيسي');
    
    // Units
    if (item.units && item.units.length > 0) {
      setFormUnits(item.units);
    } else {
      setFormUnits([
        { name: item.unit, factor: 1, price: item.salePrice, cost: item.costPrice, barcode: item.barcode }
      ]);
    }
    setShowItemModal(true);
  };

  // Add sub-unit in form
  const handleAddUnitRow = () => {
    const nextBarcode = `629${Math.floor(100000000 + Math.random() * 900000000)}`;
    setFormUnits([
      ...formUnits,
      {
        name: 'وحدة جديدة',
        factor: 12,
        price: formSalePrice * 11.5,
        cost: formCostPrice * 11.5,
        barcode: nextBarcode,
      }
    ]);
  };

  // Update unit row
  const handleUpdateUnitRow = (index: number, field: any, value: any) => {
    const updated = [...formUnits];
    updated[index] = {
      ...updated[index],
      [field]: field === 'factor' || field === 'price' || field === 'cost' ? Number(value) || 0 : value
    };
    setFormUnits(updated);

    // If updating base unit (factor == 1)
    if (index === 0 && updated[index].factor === 1) {
      if (field === 'price') setFormSalePrice(Number(value) || 0);
      if (field === 'cost') setFormCostPrice(Number(value) || 0);
      if (field === 'name') setFormBaseUnit(value);
      if (field === 'barcode') setFormBarcode(value);
    }
  };

  // Remove sub-unit row (cannot remove base unit)
  const handleRemoveUnitRow = (index: number) => {
    if (formUnits.length <= 1) {
      alert('يجب أن يحتوي الصنف على وحدة أساسية واحدة على الأقل.');
      return;
    }
    if (index === 0) {
      alert('لا يمكن حذف الوحدة الأساسية للصنف (معامل التحويل = 1).');
      return;
    }
    setFormUnits(formUnits.filter((_, i) => i !== index));
  };

  // Apply quick markup to all unit rows (Floor Price Protection & Flexible Margin)
  const handleApplyMarkupToUnits = (percentage: number) => {
    const updated = formUnits.map((u) => {
      const calculatedPrice = Math.round(u.cost * (1 + percentage / 100));
      return {
        ...u,
        price: calculatedPrice,
      };
    });
    setFormUnits(updated);
    if (updated.length > 0) {
      setFormSalePrice(updated[0].price);
      setFormMinSalePrice(Math.round(updated[0].price * 0.95));
    }
  };

  // Sync sub-units from base unit price
  const handleSyncSubUnitsFromBase = () => {
    if (formUnits.length <= 1) return;
    const basePrice = formUnits[0].price;
    const baseCost = formUnits[0].cost;
    const updated = formUnits.map((u, idx) => {
      if (idx === 0) return u;
      // Proportional wholesale price with 3% incentive discount
      const calculatedPrice = Math.round(basePrice * u.factor * 0.97);
      const calculatedCost = Math.round(baseCost * u.factor);
      return {
        ...u,
        cost: calculatedCost,
        price: calculatedPrice,
      };
    });
    setFormUnits(updated);
  };

  const handleSaveItem = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formName.trim()) {
      alert('يرجى إدخال اسم الصنف');
      return;
    }

    if (!formCode.trim()) {
      alert('يرجى تحديد كود الصنف');
      return;
    }

    // Ensure base unit is preserved
    const finalUnits: UnitDefinition[] = formUnits.map((u, i) => {
      if (i === 0) {
        return {
          ...u,
          name: u.name || formBaseUnit,
          factor: 1,
          price: Number(formSalePrice),
          cost: Number(formCostPrice),
          barcode: formBarcode,
        };
      }
      return u;
    });

    const itemData: ProductItem = {
      id: editingItem ? editingItem.id : generateNextId('P'),
      code: formCode.trim(),
      barcode: formBarcode.trim(),
      name: formName.trim(),
      category: formCategory,
      unit: formBaseUnit,
      units: finalUnits,
      costPrice: Number(formCostPrice),
      salePrice: Number(formSalePrice),
      minSalePrice: Number(formMinSalePrice) || Number(formSalePrice) * 0.95,
      stock: Number(formStock),
      minStock: Number(formMinStock),
      hasExpiryDate: formHasExpiryDate,
      valuationMethod: formValuationMethod || (formHasExpiryDate ? 'FEFO' : 'FIFO'),
      batchNo: formBatchNo.trim() || undefined,
      expiryDate: formHasExpiryDate ? (formExpiryDate.trim() || undefined) : undefined,
      location: formLocation.trim() || 'المستودع الرئيسي',
    };

    if (editingItem) {
      onUpdateProduct(itemData);
      alert(`تم تحديث بطاقة الصنف "${itemData.name}" ووحداته بنجاح.`);
    } else {
      onAddProduct(itemData);
      alert(`تمت إضافة الصنف الجديد "${itemData.name}" إلى قاعدة البيانات.`);
    }

    setShowItemModal(false);
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 p-2 gap-2 select-none overflow-hidden font-sans" dir="rtl">
      {/* 1. Phase 2 Operational Stage Ribbon (شريط المرحلة الثانية: دورة المخازن والمشتريات والتوريد) */}
      <div className="bg-white border border-slate-200 px-3 py-1.5 rounded-lg flex flex-wrap items-center justify-between gap-2 text-xs shadow-xs shrink-0">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-amber-500 text-white px-2.5 py-0.5 rounded-full border border-amber-600 shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-amber-800 flex items-center justify-center font-black text-[10px]">
              2
            </span>
            <span className="font-bold text-[11px]">المرحلة الثانية: دورة المخازن والمشتريات والتوريد</span>
          </div>

          <div className="h-4 w-px bg-slate-300" />

          {/* Direct Switchers between Phase 2 Tabs */}
          <div className="flex items-center gap-1">
            <button
              type="button"
              className="bg-amber-500 text-white font-bold px-2 py-0.5 rounded text-[11px] flex items-center gap-1 shadow-xs border border-amber-600"
              title="أنت الآن في شاشة المخزون وبطاقة الأصناف [F5]"
            >
              <Package className="w-3 h-3 text-white" />
              <span>المخزون وبطاقة الأصناف [F5]</span>
            </button>

            {onSelectTab && (
              <>
                <button
                  type="button"
                  onClick={() => onSelectTab('PURCHASES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="الانتقال إلى فواتير المشتريات والتوريد [F6]"
                >
                  <Truck className="w-3 h-3 text-emerald-600" />
                  <span>فواتير المشتريات [F6]</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('PURCHASE_ORDER')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="أوامر الشراء والتوريد وطلبيات الموردين"
                >
                  <FileText className="w-3 h-3 text-sky-600" />
                  <span>أوامر الشراء</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('COMMERCIAL_PURCHASE')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="فاتورة مشتريات تجارية موسعة"
                >
                  <Receipt className="w-3 h-3 text-cyan-600" />
                  <span>مشتريات تجارية</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('MOHASEB_ITEMS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="دليل وبطاقة الأصناف الشامل"
                >
                  <Layers className="w-3 h-3 text-amber-600" />
                  <span>دليل الأصناف</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('SALES_PRICES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="تسعير الأصناف وتعديل قوائم الأسعار"
                >
                  <Coins className="w-3 h-3 text-yellow-600" />
                  <span>تسعير الأصناف</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('EXCEL_IMPORT')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="استيراد الأصناف من ملف إكسل"
                >
                  <UploadCloud className="w-3 h-3 text-indigo-600" />
                  <span>استيراد إكسل</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('ITEM_UNITS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 hover:text-slate-900 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="وحدات القياس ومعاملات التحويل"
                >
                  <Sliders className="w-3 h-3 text-teal-600" />
                  <span>وحدات القياس</span>
                </button>
              </>
            )}
          </div>
        </div>

        {/* Right: Architectural Rules & Prev/Next Stage Shortcuts */}
        <div className="flex items-center gap-2">
          {/* Rules Indicators */}
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600 font-medium">
            <span className="bg-emerald-50 text-emerald-800 px-2 py-0.5 rounded border border-emerald-200 font-mono">
              ✓ فواتير الشراء: 5001...
            </span>
            <span className="bg-amber-50 text-amber-800 px-2 py-0.5 rounded border border-amber-200">
              ✓ تكلفة FIFO / FEFO
            </span>
            <span className="bg-sky-50 text-sky-800 px-2 py-0.5 rounded border border-sky-200">
              ✓ {warehouses.length} مستودعات نشطة
            </span>
          </div>

          {onSelectTab && (
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={() => onSelectTab('SALES_INVOICE')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition border border-slate-200 cursor-pointer"
                title="الرجوع إلى المرحلة الأولى: المبيعات ونقاط البيع"
              >
                <ChevronRight className="w-3.5 h-3.5" />
                <span>المرحلة 1: المبيعات</span>
              </button>

              <button
                type="button"
                onClick={() => onSelectTab('CUSTOMERS')}
                className="bg-emerald-600 hover:bg-emerald-700 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition shadow-xs border border-emerald-700 cursor-pointer"
                title="الانتقال إلى المرحلة الثالثة: العملاء والسيولة والعهد"
              >
                <span>المرحلة 3: العملاء والسيولة</span>
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* Top Module Sub-Navigation Bar */}
      <div className="bg-white px-3 py-2 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center gap-1.5 flex-wrap">
          <button
            id="tab-inventory-products"
            onClick={() => setActiveInventoryTab('PRODUCTS')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'PRODUCTS'
                ? 'bg-sky-600 text-white shadow-xs border-sky-700 ring-1 ring-sky-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <Package className="w-4 h-4 text-sky-600" />
            <span>بطاقات الأصناف [F1]</span>
            <span className="bg-slate-200 text-slate-800 px-1.5 py-0.2 rounded text-[10px] font-bold">
              {products.length}
            </span>
          </button>

          <button
            id="tab-inventory-transfers"
            onClick={() => setActiveInventoryTab('TRANSFERS')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'TRANSFERS'
                ? 'bg-amber-600 text-white shadow-xs border-amber-700 ring-1 ring-amber-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <ArrowLeftRight className="w-4 h-4 text-amber-600" />
            <span>التحويلات والمستودعات [F2]</span>
            <span className="bg-slate-200 text-slate-800 px-1.5 py-0.2 rounded text-[10px] font-bold">
              {transfers.length}
            </span>
          </button>

          <button
            id="tab-inventory-stocktake"
            onClick={() => setActiveInventoryTab('STOCKTAKE')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'STOCKTAKE'
                ? 'bg-emerald-600 text-white shadow-xs border-emerald-700 ring-1 ring-emerald-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <ClipboardCheck className="w-4 h-4 text-emerald-600" />
            <span>الجرد المخزني ومطابقة الأرصدة [F3]</span>
            <span className="bg-slate-200 text-slate-800 px-1.5 py-0.2 rounded text-[10px] font-bold">
              {stocktakes.length}
            </span>
          </button>

          <button
            id="tab-inventory-adjustments"
            onClick={() => setActiveInventoryTab('ADJUSTMENTS')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'ADJUSTMENTS'
                ? 'bg-purple-600 text-white shadow-xs border-purple-700 ring-1 ring-purple-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <SlidersHorizontal className="w-4 h-4 text-purple-600" />
            <span>سندات التسوية المخزنية [F4]</span>
            <span className="bg-slate-200 text-slate-800 px-1.5 py-0.2 rounded text-[10px] font-bold">
              {stockAdjustments.length}
            </span>
          </button>

          <button
            id="tab-inventory-cards"
            onClick={() => setActiveInventoryTab('CARDS')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'CARDS'
                ? 'bg-teal-600 text-white shadow-xs border-teal-700 ring-1 ring-teal-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <BookOpen className="w-4 h-4 text-teal-600" />
            <span>كرت حركة الصنف والرقابة [F5]</span>
          </button>

          <button
            id="tab-inventory-prices"
            onClick={() => setActiveInventoryTab('PRICE_UPDATE')}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border ${
              activeInventoryTab === 'PRICE_UPDATE'
                ? 'bg-rose-600 text-white shadow-xs border-rose-700 ring-1 ring-rose-300'
                : 'bg-slate-50 text-slate-700 hover:bg-slate-100 hover:text-slate-900 border-slate-200'
            }`}
          >
            <Sliders className="w-4 h-4 text-rose-600" />
            <span>تعديل وتحديث الأسعار المرن [F6]</span>
          </button>
        </div>

        <div className="flex items-center gap-3 text-xs text-slate-700">
          <div className="flex items-center gap-1.5 bg-slate-100 px-2.5 py-1 rounded-lg border border-slate-200 shadow-xs">
            <Building2 className="w-3.5 h-3.5 text-sky-600" />
            <span className="font-semibold text-slate-700">المستودعات:</span>
            <span className="text-sky-700 font-bold">{warehouses.length} مستودع نشط</span>
          </div>
        </div>
      </div>

      {activeInventoryTab === 'PRICE_UPDATE' ? (
        <QuickPriceUpdateView
          products={products}
          categories={categories}
          currency={currency}
          onBulkUpdatePrices={onUpdateProductsStock}
        />
      ) : activeInventoryTab === 'TRANSFERS' ? (
        <WarehouseTransfersView
          warehouses={warehouses}
          products={products}
          transfers={transfers}
          onAddTransfer={onAddTransfer}
          onUpdateTransferStatus={onUpdateTransferStatus}
          onAddWarehouse={onAddWarehouse}
          onUpdateWarehouse={onUpdateWarehouse}
          onUpdateProductsStock={onUpdateProductsStock}
          currency={currency}
        />
      ) : activeInventoryTab === 'STOCKTAKE' || activeInventoryTab === 'ADJUSTMENTS' || activeInventoryTab === 'CARDS' ? (
        <StocktakingAndCardsView
          products={products}
          warehouses={warehouses}
          transfers={transfers}
          salesInvoices={salesInvoices}
          purchaseInvoices={purchaseInvoices}
          stocktakes={stocktakes}
          stockAdjustments={stockAdjustments}
          journalEntries={journalEntries}
          onAddStocktake={onAddStocktake}
          onUpdateStocktake={onUpdateStocktake}
          onPostStockAdjustment={onPostStockAdjustment}
          currency={currency}
          cashierName={cashierName}
          defaultSubTab={
            activeInventoryTab === 'STOCKTAKE'
              ? 'AUDIT'
              : activeInventoryTab === 'ADJUSTMENTS'
              ? 'ADJUSTMENTS'
              : 'CARDS'
          }
        />
      ) : (
        <>
          {/* Top Filter & Operations Bar */}
          <div className="bg-white p-2.5 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-2">
            <div className="flex items-center gap-2 flex-1 min-w-[320px]">
              <div className="relative flex-1">
                <Search className="w-4 h-4 absolute right-3 top-2.5 text-slate-400" />
                <input
                  type="text"
                  placeholder="بحث سريع بالاسم، الباركود (حبة/باكت/كرتون)، الكود، الدفعة..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-full pr-9 pl-3 py-1.5 bg-slate-50 border border-slate-200 rounded-lg text-xs font-medium text-slate-800 placeholder-slate-400 focus:outline-none focus:ring-1 focus:ring-sky-500 focus:bg-white"
                />
              </div>

              <select
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="bg-slate-50 border border-slate-200 rounded-lg text-xs py-1.5 px-2 font-semibold text-slate-700 focus:outline-none cursor-pointer"
              >
                {categories.map((c) => (
                  <option key={c} value={c}>
                    {c === 'ALL' ? 'كافة التصنيفات' : c}
                  </option>
                ))}
              </select>

              {/* Warehouse Filter */}
              <div className="flex items-center gap-1 bg-slate-50 border border-slate-200 rounded-lg px-2 py-1">
                <Building2 className="w-3.5 h-3.5 text-sky-600 shrink-0" />
                <select
                  value={selectedWarehouseFilter}
                  onChange={(e) => setSelectedWarehouseFilter(e.target.value)}
                  className="bg-transparent text-xs font-semibold text-slate-700 focus:outline-none cursor-pointer"
                >
                  <option value="ALL">كافة المستودعات (الرصيد الإجمالي)</option>
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      مستودع: {w.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* Expiry Category Filter */}
              <div className="flex items-center gap-1 bg-slate-50 border border-slate-200 rounded-lg px-2 py-1">
                <Calendar className="w-3.5 h-3.5 text-amber-600 shrink-0" />
                <select
                  value={expiryFilter}
                  onChange={(e) => setExpiryFilter(e.target.value as any)}
                  className="bg-transparent text-xs font-semibold text-slate-700 focus:outline-none cursor-pointer"
                >
                  <option value="ALL">كل الأصناف (بصلاحية وبدون)</option>
                  <option value="WITH_EXPIRY">أصناف ذات تاريخ انتهاء</option>
                  <option value="WITHOUT_EXPIRY">أصناف بدون تاريخ انتهاء</option>
                </select>
              </div>

              <label className={`flex items-center gap-1 text-xs font-semibold px-2 py-1.5 rounded-lg border transition cursor-pointer ${
                filterLowStockOnly ? 'bg-rose-50 text-rose-800 border-rose-300' : 'bg-slate-50 text-slate-700 border-slate-200 hover:bg-slate-100'
              }`}>
                <input
                  type="checkbox"
                  checked={filterLowStockOnly}
                  onChange={(e) => setFilterLowStockOnly(e.target.checked)}
                  className="rounded text-rose-600"
                />
                <span className="flex items-center gap-1">
                  <AlertTriangle className="w-3 h-3 text-rose-600" />
                  <span>نواقص المخزون</span>
                </span>
              </label>

              <label className={`flex items-center gap-1 text-xs font-semibold px-2 py-1.5 rounded-lg border transition cursor-pointer ${
                filterExpiringSoon ? 'bg-amber-50 text-amber-800 border-amber-300' : 'bg-slate-50 text-slate-700 border-slate-200 hover:bg-slate-100'
              }`}>
                <input
                  type="checkbox"
                  checked={filterExpiringSoon}
                  onChange={(e) => setFilterExpiringSoon(e.target.checked)}
                  className="rounded text-amber-600"
                />
                <span className="flex items-center gap-1">
                  <Calendar className="w-3 h-3 text-amber-600" />
                  <span>قريب الانتهاء (60 يوم)</span>
                </span>
              </label>
            </div>

            <div className="flex items-center gap-2">
              <div className="hidden sm:flex items-center gap-1.5 bg-emerald-50 border border-emerald-200 text-emerald-800 px-2.5 py-1.5 rounded-lg text-xs" title="سياسة صرف هجينة ذكية: FEFO/EEFO للأصناف الخاضعة للصلاحية و FIFO للأصناف الدائمة">
                <Layers className="w-3.5 h-3.5 text-emerald-600" />
                <span className="font-bold">سياسة الصرف والتكلفة: FEFO (للصلاحية) + FIFO (للدائم)</span>
              </div>
              <button
                id="btn-add-new-product"
                onClick={handleOpenAddModal}
                className="bg-emerald-600 hover:bg-emerald-700 text-white text-xs font-bold px-3.5 py-2 rounded-lg flex items-center gap-1.5 shadow-xs transition cursor-pointer"
              >
                <Plus className="w-4 h-4" />
                <span>إضافة بطاقة صنف جديد [F5]</span>
              </button>
            </div>
          </div>

      {/* Main Desktop DataGridView for Inventory */}
      <div className="flex-1 bg-white rounded-lg border border-slate-200 shadow-xs overflow-hidden flex flex-col">
        {/* Table Title Bar */}
        <div className="bg-slate-50 border-b border-slate-200 text-slate-800 px-3 py-2 flex items-center justify-between text-xs font-semibold">
          <div className="flex items-center gap-2">
            <Package className="w-4 h-4 text-sky-600" />
            <span className="font-bold">دليل بطاقات الأصناف وتعدد الوحدات</span>
            <span className="bg-sky-100 text-sky-800 px-2 py-0.5 rounded text-[11px] font-bold border border-sky-200">
              {filtered.length} صنف مسجل
            </span>
          </div>
          <div className="text-[11px] text-slate-600 flex items-center gap-3">
            <span>
              إجمالي تقييم المخزون بالتكلفة:{' '}
              <strong className="text-amber-700 font-mono font-bold">
                {filtered.reduce((sum, p) => sum + p.stock * p.costPrice, 0).toLocaleString()} {currency}
              </strong>
            </span>
            <span className="text-slate-300">|</span>
            <span>
              إجمالي قيمة البيع التقديرية:{' '}
              <strong className="text-emerald-700 font-mono font-bold">
                {filtered.reduce((sum, p) => sum + p.stock * p.salePrice, 0).toLocaleString()} {currency}
              </strong>
            </span>
          </div>
        </div>

        {/* DataGrid Table */}
        <div className="flex-1 overflow-auto">
          <table className="w-full text-right border-collapse text-xs">
            <thead className="sticky top-0 z-10 bg-slate-100 text-slate-700 border-b border-slate-200 font-semibold">
              <tr>
                <th className="p-2.5 w-10 text-center">#</th>
                <th className="p-2.5 w-16">الكود</th>
                <th className="p-2.5 w-28">الباركود</th>
                <th className="p-2.5">اسم الصنف والمواصفات</th>
                <th className="p-2.5 w-24">التصنيف</th>
                <th className="p-2.5 w-44">الوحدات والتحويل (Multi-Units)</th>
                <th className="p-2.5 w-24 text-left">التكلفة ({currency})</th>
                <th className="p-2.5 w-24 text-left">سعر البيع ({currency})</th>
                <th className="p-2.5 w-20 text-center">الرصيد</th>
                <th className="p-2.5 w-16 text-center">حد الطلب</th>
                <th className="p-2.5 w-24">رقم الدفعة</th>
                <th className="p-2.5 w-28 text-center">الصلاحية والسياسة</th>
                <th className="p-2.5 w-24 text-center">الإجراءات</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100 text-slate-800">
              {filtered.length === 0 ? (
                <tr>
                  <td colSpan={13} className="text-center py-12 text-slate-400">
                    لا توجد أصناف مطابقة لخيارات البحث المحددة
                  </td>
                </tr>
              ) : (
                filtered.map((item, idx) => {
                  const currentStock = selectedWarehouseFilter === 'ALL'
                    ? item.stock
                    : getProductStockInWarehouse(item, selectedWarehouseFilter);
                  const isLow = currentStock <= item.minStock;
                  const isExp = isExpiringSoon(item);
                  const hasExpired = isExpired(item);

                  return (
                    <tr key={item.id} className="hover:bg-sky-50/50 transition">
                      <td className="p-2.5 text-center font-mono text-slate-400 font-bold">{idx + 1}</td>
                      <td className="p-2.5 font-mono font-semibold text-sky-700">{item.code}</td>
                      <td className="p-2.5 font-mono text-slate-600 text-[11px]">{item.barcode}</td>
                      <td className="p-2.5 font-bold text-slate-900">
                        <div className="flex items-center gap-1.5 flex-wrap">
                          <span>{item.name}</span>
                          {isLow && (
                            <span className="bg-rose-50 text-rose-700 text-[10px] px-1.5 py-0.2 rounded font-bold border border-rose-200">
                              نقص
                            </span>
                          )}
                          {item.hasExpiryDate !== false && hasExpired && (
                            <span className="bg-rose-50 text-rose-700 text-[10px] px-1.5 py-0.2 rounded font-bold border border-rose-200">
                              منتهي
                            </span>
                          )}
                          {item.hasExpiryDate !== false && !hasExpired && isExp && (
                            <span className="bg-amber-50 text-amber-700 text-[10px] px-1.5 py-0.2 rounded font-bold border border-amber-200">
                              قريب
                            </span>
                          )}
                          {item.hasExpiryDate === false && (
                            <span className="bg-slate-100 text-slate-600 text-[9px] px-1.5 py-0.2 rounded font-sans border border-slate-200">
                              بدون انتهاء
                            </span>
                          )}
                        </div>
                      </td>
                      <td className="p-2.5 text-slate-600 text-[11px]">{item.category}</td>
                      
                      {/* Multi-Units Badges */}
                      <td className="p-2.5">
                        <div className="flex flex-wrap gap-1">
                          {item.units && item.units.length > 0 ? (
                            item.units.map((u, uIdx) => (
                              <span
                                key={uIdx}
                                className={`px-1.5 py-0.5 rounded text-[10px] font-mono flex items-center gap-1 ${
                                  u.factor === 1
                                    ? 'bg-sky-50 text-sky-800 border border-sky-200 font-bold'
                                    : 'bg-slate-50 text-slate-700 border border-slate-200'
                                }`}
                                title={`معامل: ${u.factor} | باركود: ${u.barcode} | السعر: ${u.price.toLocaleString()} ${currency}`}
                              >
                                <span>{u.name}</span>
                                {u.factor > 1 && <span className="text-amber-700 font-bold">×{u.factor}</span>}
                              </span>
                            ))
                          ) : (
                            <span className="bg-sky-50 text-sky-800 border border-sky-200 px-1.5 py-0.5 rounded text-[10px] font-bold">
                              {item.unit}
                            </span>
                          )}
                        </div>
                      </td>

                      <td className="p-2.5 text-left font-mono text-slate-700">
                        {item.costPrice.toLocaleString()}
                      </td>
                      <td className="p-2.5 text-left font-mono font-bold text-emerald-700">
                        {item.salePrice.toLocaleString()}
                      </td>
                      <td className="p-2.5 text-center font-mono">
                        <span
                          className={`px-2 py-0.5 rounded font-bold text-xs ${
                            isLow 
                              ? 'bg-rose-50 text-rose-700 border border-rose-300' 
                              : 'bg-emerald-50 text-emerald-800 border border-emerald-300'
                          }`}
                        >
                          {currentStock} {item.unit}
                        </span>
                        {selectedWarehouseFilter !== 'ALL' && (
                          <span className="text-[10px] text-slate-500 block font-sans mt-0.5">
                            الكل: {item.stock}
                          </span>
                        )}
                      </td>
                      <td className="p-2.5 text-center font-mono text-slate-500">{item.minStock}</td>
                      <td className="p-2.5 font-mono text-xs text-slate-600">{item.batchNo || 'عام'}</td>
                      <td className="p-2.5 text-center text-xs">
                        {item.hasExpiryDate === false ? (
                          <div className="flex flex-col items-center gap-0.5">
                            <span className="inline-flex items-center gap-1 px-2 py-0.5 rounded text-[10px] font-sans font-medium bg-slate-100 text-slate-700 border border-slate-200" title="صنف دائم غير خاضع لتاريخ انتهاء الصلاحية">
                              <ShieldOff className="w-3 h-3 text-slate-500" />
                              <span>بدون انتهاء</span>
                            </span>
                            <span className="text-[9px] font-mono text-sky-700 font-bold bg-sky-50 px-1.5 py-0.2 rounded border border-sky-200" title="الوارد أولاً يصرف أولاً">
                              صرف: FIFO
                            </span>
                          </div>
                        ) : hasExpired ? (
                          <div className="flex flex-col items-center gap-0.5">
                            <span className="inline-flex items-center gap-1 px-1.5 py-0.5 rounded text-[10px] font-bold bg-rose-50 text-rose-700 border border-rose-200" title="الصنف منتهي الصلاحية!">
                              <AlertTriangle className="w-3 h-3 text-rose-600" />
                              <span>منتهي ({item.expiryDate})</span>
                            </span>
                            <span className="text-[9px] font-mono text-amber-800 font-bold bg-amber-50 px-1.5 py-0.2 rounded border border-amber-200" title="الأقرب انتهاءً يصرف أولاً">
                              صرف: FEFO
                            </span>
                          </div>
                        ) : isExp ? (
                          <div className="flex flex-col items-center gap-0.5">
                            <span className="inline-flex items-center gap-1 px-1.5 py-0.5 rounded text-[10px] font-bold bg-amber-50 text-amber-800 border border-amber-200" title="الصنف قريب الانتهاء خلال 60 يوماً">
                              <Calendar className="w-3 h-3 text-amber-600" />
                              <span>قريب ({item.expiryDate})</span>
                            </span>
                            <span className="text-[9px] font-mono text-amber-800 font-bold bg-amber-50 px-1.5 py-0.2 rounded border border-amber-200" title="الأقرب انتهاءً يصرف أولاً">
                              صرف: FEFO
                            </span>
                          </div>
                        ) : (
                          <div className="flex flex-col items-center gap-0.5">
                            <span className="font-mono text-slate-800 text-xs font-semibold">
                              {item.expiryDate || '-'}
                            </span>
                            <span className="text-[9px] font-mono text-amber-800 font-bold bg-amber-50 px-1.5 py-0.2 rounded border border-amber-200" title="الأقرب انتهاءً يصرف أولاً">
                              صرف: FEFO
                            </span>
                          </div>
                        )}
                      </td>
                      <td className="p-2.5 text-center">
                        <div className="inline-flex items-center gap-1">
                          <button
                            onClick={() => setFifoModalProduct(item)}
                            className="p-1.5 bg-slate-100 hover:bg-emerald-600 text-slate-600 hover:text-white rounded-md transition cursor-pointer border border-slate-200"
                            title={item.hasExpiryDate === false ? "عرض طبقات التكلفة بنظام FIFO (الوارد أولاً يصرف أولاً)" : "عرض طبقات التكلفة وسلسلة الصرف بنظام FEFO / EEFO (الأقرب انتهاءً يصرف أولاً)"}
                          >
                            <Layers className="w-3.5 h-3.5" />
                          </button>
                          <button
                            onClick={() => handleOpenEditModal(item)}
                            className="p-1.5 bg-slate-100 hover:bg-sky-600 text-slate-600 hover:text-white rounded-md transition cursor-pointer border border-slate-200"
                            title="تعديل بطاقة الصنف والوحدات والأسعار"
                          >
                            <Edit className="w-3.5 h-3.5" />
                          </button>
                          <button
                            onClick={() => {
                              setBarcodePreviewItem(item);
                              setSelectedUnitForBarcode(item.units && item.units.length > 0 ? item.units[0] : null);
                            }}
                            className="p-1.5 bg-slate-100 hover:bg-purple-600 text-slate-600 hover:text-white rounded-md transition cursor-pointer border border-slate-200"
                            title="معاينة وطباعة الباركود الحراري"
                          >
                            <Barcode className="w-3.5 h-3.5" />
                          </button>
                          <button
                            onClick={() => {
                              if (confirm(`هل أنت متأكد من حذف الصنف [${item.name}]؟`)) {
                                onDeleteProduct(item.id);
                              }
                            }}
                            className="p-1.5 bg-slate-100 hover:bg-rose-600 text-slate-600 hover:text-white rounded-md transition cursor-pointer border border-slate-200"
                            title="حذف الصنف"
                          >
                            <Trash2 className="w-3.5 h-3.5" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>

        {/* Footer Summary Bar */}
        <div className="bg-slate-50 p-2 border-t border-slate-200 flex flex-wrap items-center justify-between text-xs text-slate-600 gap-2">
          <div className="flex items-center gap-3 flex-wrap">
            <span>إجمالي الأصناف: <strong className="text-slate-900 font-mono">{products.length}</strong></span>
            <span className="text-slate-300">|</span>
            <span className="text-amber-800">خاضعة للصلاحية: <strong className="text-amber-900 font-mono font-bold">{products.filter((p) => p.hasExpiryDate !== false).length}</strong></span>
            <span className="text-slate-300">|</span>
            <span className="text-slate-700">بدون انتهاء (دائمة): <strong className="text-slate-900 font-mono">{products.filter((p) => p.hasExpiryDate === false).length}</strong></span>
            <span className="text-slate-300">|</span>
            <span>نواقص تحت الطلب: <strong className="text-rose-600 font-mono font-bold">{products.filter((p) => p.stock <= p.minStock).length}</strong></span>
            <span className="text-slate-300">|</span>
            <span>قريبة الانتهاء: <strong className="text-amber-700 font-mono font-bold">{products.filter((p) => isExpiringSoon(p)).length}</strong></span>
          </div>
          <div>
            <span>العملة المعتمدة: <strong className="text-sky-700 font-mono font-bold">{currency} (ريال يمني)</strong></span>
          </div>
        </div>
      </div>

      {/* Add / Edit Product & Multi-Units Modal */}
      <InventoryAddProductModal
        isOpen={showItemModal}
        onClose={() => setShowItemModal(false)}
        editingItem={editingItem}
        handleSaveItem={handleSaveItem}
        formCode={formCode}
        setFormCode={setFormCode}
        formBarcode={formBarcode}
        setFormBarcode={setFormBarcode}
        formName={formName}
        setFormName={setFormName}
        formCategory={formCategory}
        setFormCategory={setFormCategory}
        categories={categories}
        formUnits={formUnits}
        handleApplyMarkupToUnits={handleApplyMarkupToUnits}
        handleSyncSubUnitsFromBase={handleSyncSubUnitsFromBase}
        handleAddUnitRow={handleAddUnitRow}
        handleUpdateUnitRow={handleUpdateUnitRow}
        handleRemoveUnitRow={handleRemoveUnitRow}
        formHasExpiryDate={formHasExpiryDate}
        setFormHasExpiryDate={setFormHasExpiryDate}
        setFormValuationMethod={setFormValuationMethod}
        formExpiryDate={formExpiryDate}
        setFormExpiryDate={setFormExpiryDate}
        formStock={formStock}
        setFormStock={setFormStock}
        formMinStock={formMinStock}
        setFormMinStock={setFormMinStock}
        formBatchNo={formBatchNo}
        setFormBatchNo={setFormBatchNo}
        currency={currency}
      />

      {/* Barcode Label Print Modal with Unit Selector */}
      <InventoryBarcodeModal
        barcodePreviewItem={barcodePreviewItem}
        setBarcodePreviewItem={setBarcodePreviewItem}
        selectedUnitForBarcode={selectedUnitForBarcode}
        setSelectedUnitForBarcode={setSelectedUnitForBarcode}
        currency={currency}
      />

      {/* FIFO / FEFO Stock Layers Inspection Modal */}
      <InventoryFifoModal
        fifoModalProduct={fifoModalProduct}
        setFifoModalProduct={setFifoModalProduct}
        currency={currency}
      />
    </>
      )}
    </div>
  );
};
