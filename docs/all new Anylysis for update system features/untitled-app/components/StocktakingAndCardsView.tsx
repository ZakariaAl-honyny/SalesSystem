'use client';

import React, { useState, useMemo, useCallback } from 'react';
import {
  StocktakingNewModal,
  StocktakingPrintModal,
  StocktakingPrintAdjustmentModal,
  StocktakingPrintCardModal
} from './stocktaking';
import {
  ProductItem,
  Warehouse,
  StockTransfer,
  Invoice,
  JournalEntry,
  StocktakingSession,
  StocktakeItem,
  StockAdjustment,
  StockAdjustmentItem,
  StockAdjustmentType,
} from '@/types/pos';
import { generateNextId } from '@/lib/idGenerator';
import { 
  getProductStockInWarehouse, 
  generateProductStockLedger,
  updateProductWarehouseStock 
} from '@/lib/mock';
import {
  ClipboardCheck,
  SlidersHorizontal,
  FileSpreadsheet,
  Search,
  Plus,
  Printer,
  CheckCircle2,
  AlertCircle,
  TrendingDown,
  TrendingUp,
  Building2,
  Calendar,
  Layers,
  ArrowRight,
  Barcode,
  Save,
  Trash2,
  ShieldCheck,
  RotateCcw,
  BookOpen,
  ArrowDownLeft,
  ArrowUpRight,
  Filter,
  Check,
  FileText,
  UserCheck,
  Package,
} from 'lucide-react';

interface StocktakingAndCardsViewProps {
  products: ProductItem[];
  warehouses: Warehouse[];
  transfers: StockTransfer[];
  salesInvoices: Invoice[];
  purchaseInvoices: Invoice[];
  stocktakes: StocktakingSession[];
  stockAdjustments: StockAdjustment[];
  journalEntries: JournalEntry[];
  onAddStocktake: (session: StocktakingSession) => void;
  onUpdateStocktake: (session: StocktakingSession) => void;
  onPostStockAdjustment: (adjustment: StockAdjustment, updatedProducts: ProductItem[]) => void;
  currency: string;
  cashierName?: string;
  defaultSubTab?: 'AUDIT' | 'ADJUSTMENTS' | 'CARDS';
}

export const StocktakingAndCardsView: React.FC<StocktakingAndCardsViewProps> = ({
  products,
  warehouses,
  transfers,
  salesInvoices,
  purchaseInvoices,
  stocktakes,
  stockAdjustments,
  journalEntries,
  onAddStocktake,
  onUpdateStocktake,
  onPostStockAdjustment,
  currency,
  cashierName = 'المسؤول الإداري',
  defaultSubTab = 'AUDIT',
}) => {
  const [subTab, setSubTab] = useState<'AUDIT' | 'ADJUSTMENTS' | 'CARDS'>(defaultSubTab);

  // ----------------------------------------------------
  // SUB-TAB 1: PHYSICAL AUDIT (محاضر الجرد الفعلي)
  // ----------------------------------------------------
  const [selectedStocktakeId, setSelectedStocktakeId] = useState<string>(
    stocktakes.length > 0 ? stocktakes[0].id : ''
  );
  const [showNewStocktakeModal, setShowNewStocktakeModal] = useState<boolean>(false);
  const [stocktakeSearch, setStocktakeSearch] = useState<string>('');
  const [printStocktake, setPrintStocktake] = useState<StocktakingSession | null>(null);

  // Active Session being viewed or edited
  const activeStocktake = useMemo(() => {
    return stocktakes.find((st) => st.id === selectedStocktakeId) || stocktakes[0] || null;
  }, [stocktakes, selectedStocktakeId]);

  // New Stocktake Form State
  const [newStWarehouseId, setNewStWarehouseId] = useState<string>(
    warehouses[0]?.id || 'WH-01'
  );
  const [newStNotes, setNewStNotes] = useState<string>('');
  const [newStHeadMember, setNewStHeadMember] = useState<string>('م. أنور القدسي (رئيس لجنة الجرد)');
  const [newStMember2, setNewStMember2] = useState<string>('عادل الشميري (عضو)');
  const [newStMember3, setNewStMember3] = useState<string>('أحمد الشرعبي (أمين المستودع)');

  // In-session items editing state
  const [editingPhysicalCounts, setEditingPhysicalCounts] = useState<{ [itemId: string]: number }>({});
  const [barcodeScanInput, setBarcodeScanInput] = useState<string>('');

  // ----------------------------------------------------
  // SUB-TAB 2: ADJUSTMENT VOUCHERS (سندات التسوية)
  // ----------------------------------------------------
  const [selectedAdjustmentId, setSelectedAdjustmentId] = useState<string>(
    stockAdjustments.length > 0 ? stockAdjustments[0].id : ''
  );
  const [printAdjustment, setPrintAdjustment] = useState<StockAdjustment | null>(null);
  const [showManualAdjustmentModal, setShowManualAdjustmentModal] = useState<boolean>(false);

  // Manual Adjustment Form
  const [manualAdjWhId, setManualAdjWhId] = useState<string>(warehouses[0]?.id || 'WH-01');
  const [manualAdjReason, setManualAdjReason] = useState<string>('تلف بضاعة وانتهاء صلاحية');
  const [manualAdjType, setManualAdjType] = useState<StockAdjustmentType>('DEFICIT');
  const [manualAdjItems, setManualAdjItems] = useState<
    { productId: string; qty: number; cost: number; reasonNote: string }[]
  >([{ productId: products[0]?.id || '', qty: 1, cost: products[0]?.costPrice || 1000, reasonNote: 'تالف' }]);

  // ----------------------------------------------------
  // SUB-TAB 3: STOCK LEDGER CARD (كرت حركة الصنف)
  // ----------------------------------------------------
  const [cardProductId, setCardProductId] = useState<string>(products[0]?.id || '');
  const [cardWarehouseId, setCardWarehouseId] = useState<string>('ALL');
  const [cardSearchQuery, setCardSearchQuery] = useState<string>('');
  const [printStockCard, setPrintStockCard] = useState<boolean>(false);

  const selectedCardProduct = useMemo(() => {
    return products.find((p) => p.id === cardProductId) || products[0];
  }, [products, cardProductId]);

  // Generate the ledger movements for current product
  const stockMovements = useMemo(() => {
    if (!selectedCardProduct) return [];
    return generateProductStockLedger(
      selectedCardProduct.id,
      cardWarehouseId,
      selectedCardProduct,
      salesInvoices,
      purchaseInvoices,
      transfers,
      stockAdjustments
    );
  }, [
    selectedCardProduct,
    cardWarehouseId,
    salesInvoices,
    purchaseInvoices,
    transfers,
    stockAdjustments,
  ]);

  // Current balance in target warehouse for the product
  const currentCardStock = useMemo(() => {
    if (!selectedCardProduct) return 0;
    if (cardWarehouseId === 'ALL') return selectedCardProduct.stock;
    return getProductStockInWarehouse(selectedCardProduct, cardWarehouseId);
  }, [selectedCardProduct, cardWarehouseId]);

  const getWarehouseName = useCallback((warehouseId: string) => {
    return warehouses.find((w) => w.id === warehouseId)?.name || warehouseId;
  }, [warehouses]);

  // ----------------------------------------------------
  // HANDLERS: NEW STOCKTAKE SESSION
  // ----------------------------------------------------
  const handleCreateNewStocktake = () => {
    const wh = warehouses.find((w) => w.id === newStWarehouseId) || warehouses[0];
    const nextNo = `ST-${1000 + stocktakes.length + 1}`;

    // Populate all products available in this warehouse with book stock
    const sessionItems: StocktakeItem[] = products.map((prod, idx) => {
      const bStock = getProductStockInWarehouse(prod, wh.id);
      return {
        id: `STI-${Date.now()}-${idx + 1}`,
        productId: prod.id,
        productCode: prod.code,
        barcode: prod.barcode,
        productName: prod.name,
        unit: prod.unit,
        unitCost: prod.costPrice,
        bookStock: bStock,
        physicalStock: bStock, // initially equal until edited
        differenceQty: 0,
        differenceCost: 0,
        batchNo: prod.batchNo,
        expiryDate: prod.expiryDate,
        notes: '',
      };
    });

    const newSession: StocktakingSession = {
      id: generateNextId('ST'),
      sessionNo: nextNo,
      warehouseId: wh.id,
      warehouseName: wh.name,
      date: new Date().toISOString().split('T')[0],
      time: new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' }),
      committeeMembers: [
        newStHeadMember.trim(),
        newStMember2.trim(),
        newStMember3.trim(),
      ].filter(Boolean),
      status: 'IN_PROGRESS',
      notes: newStNotes.trim() || `محضر جرد فعلي للمستودع: ${wh.name}`,
      items: sessionItems,
      totalBookQty: sessionItems.reduce((acc, it) => acc + it.bookStock, 0),
      totalPhysicalQty: sessionItems.reduce((acc, it) => acc + it.physicalStock, 0),
      totalSurplusQty: 0,
      totalDeficitQty: 0,
      totalSurplusValue: 0,
      totalDeficitValue: 0,
      netDifferenceValue: 0,
      createdAt: `${new Date().toISOString().split('T')[0]} ${new Date().toLocaleTimeString('ar-YE')}`,
      createdBy: cashierName,
    };

    onAddStocktake(newSession);
    setSelectedStocktakeId(newSession.id);
    setShowNewStocktakeModal(false);
  };

  // Update physical count for an item in active session
  const handlePhysicalCountChange = (itemId: string, newQty: number) => {
    if (!activeStocktake || activeStocktake.status === 'ADJUSTED') return;

    const updatedItems = activeStocktake.items.map((it) => {
      if (it.id === itemId) {
        const physical = Math.max(0, newQty);
        const diffQty = physical - it.bookStock;
        const diffCost = diffQty * it.unitCost;
        return {
          ...it,
          physicalStock: physical,
          differenceQty: diffQty,
          differenceCost: diffCost,
        };
      }
      return it;
    });

    const totalBook = updatedItems.reduce((acc, it) => acc + it.bookStock, 0);
    const totalPhys = updatedItems.reduce((acc, it) => acc + it.physicalStock, 0);
    const surplusItems = updatedItems.filter((it) => it.differenceQty > 0);
    const deficitItems = updatedItems.filter((it) => it.differenceQty < 0);

    const totalSurplusQ = surplusItems.reduce((acc, it) => acc + it.differenceQty, 0);
    const totalDeficitQ = Math.abs(deficitItems.reduce((acc, it) => acc + it.differenceQty, 0));
    const totalSurplusVal = surplusItems.reduce((acc, it) => acc + it.differenceCost, 0);
    const totalDeficitVal = Math.abs(deficitItems.reduce((acc, it) => acc + it.differenceCost, 0));
    const netDiff = totalSurplusVal - totalDeficitVal;

    const updatedSession: StocktakingSession = {
      ...activeStocktake,
      items: updatedItems,
      totalBookQty: totalBook,
      totalPhysicalQty: totalPhys,
      totalSurplusQty: totalSurplusQ,
      totalDeficitQty: totalDeficitQ,
      totalSurplusValue: totalSurplusVal,
      totalDeficitValue: totalDeficitVal,
      netDifferenceValue: netDiff,
    };

    onUpdateStocktake(updatedSession);
  };

  // Barcode Fast Entry in Stocktake
  const handleBarcodeQuickCount = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && barcodeScanInput.trim() && activeStocktake) {
      e.preventDefault();
      const code = barcodeScanInput.trim();
      const targetItem = activeStocktake.items.find(
        (it) => it.barcode === code || it.productCode === code
      );
      if (targetItem) {
        handlePhysicalCountChange(targetItem.id, targetItem.physicalStock + 1);
        setBarcodeScanInput('');
      } else {
        alert(`الباركود [${code}] غير موجود ضمن قائمة أصناف هذا المحضر.`);
      }
    }
  };

  // ----------------------------------------------------
  // HANDLER: APPROVE & POST STOCK ADJUSTMENT FROM STOCKTAKE
  // ----------------------------------------------------
  const handlePostAdjustmentFromStocktake = () => {
    if (!activeStocktake) return;
    if (activeStocktake.status === 'ADJUSTED') {
      alert('تمت تسوية وترحيل هذا المحضر مسبقاً!');
      return;
    }

    // Identify difference items
    const diffItems = activeStocktake.items.filter((it) => it.differenceQty !== 0);
    if (diffItems.length === 0) {
      alert('لا توجد فروقات بين الرصيد الدفتري والفعلي لتسويتها! الأرصدة متطابقة 100%.');
      return;
    }

    const confirmMsg = `هل تؤكد اعتماد وترحيل سند التسوية المخزنية للفروقات (${diffItems.length} صنف)؟\n` +
      `إجمالي قيمة الفائض: ${activeStocktake.totalSurplusValue.toLocaleString()} ${currency}\n` +
      `إجمالي قيمة العجز: ${activeStocktake.totalDeficitValue.toLocaleString()} ${currency}\n` +
      `صافي الأثر: ${activeStocktake.netDifferenceValue.toLocaleString()} ${currency}\n\n` +
      `سيتم تحديث أرصدة المستودع آلياً وإنشاء قيود اليومية المزدوجة فوراً.`;

    if (!confirm(confirmMsg)) return;

    const nextAdjNo = `ADJ-${1000 + stockAdjustments.length + 1}`;
    const adjItems: StockAdjustmentItem[] = diffItems.map((it, idx) => ({
      id: `ADJI-${Date.now()}-${idx + 1}`,
      productId: it.productId,
      productCode: it.productCode,
      barcode: it.barcode,
      productName: it.productName,
      unit: it.unit,
      unitCost: it.unitCost,
      bookQty: it.bookStock,
      physicalQty: it.physicalStock,
      adjustmentQty: it.differenceQty,
      totalCost: it.differenceCost,
      batchNo: it.batchNo,
      notes: it.notes || (it.differenceQty > 0 ? 'فائض جرد فعلي' : 'عجز وتلف جرد فعلي'),
    }));

    const adjType: StockAdjustmentType = 
      activeStocktake.totalSurplusQty > 0 && activeStocktake.totalDeficitQty > 0
        ? 'BOTH'
        : activeStocktake.totalSurplusQty > 0
        ? 'SURPLUS'
        : 'DEFICIT';

    const newAdjustment: StockAdjustment = {
      id: generateNextId('ADJ'),
      adjustmentNo: nextAdjNo,
      date: new Date().toISOString().split('T')[0],
      time: new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' }),
      warehouseId: activeStocktake.warehouseId,
      warehouseName: activeStocktake.warehouseName,
      stocktakeSessionId: activeStocktake.id,
      stocktakeSessionNo: activeStocktake.sessionNo,
      type: adjType,
      reason: `معالجة فروقات محضر الجرد الفعلي رقم ${activeStocktake.sessionNo}`,
      items: adjItems,
      totalItems: adjItems.length,
      netAdjustmentCost: activeStocktake.netDifferenceValue,
      totalSurplusCost: activeStocktake.totalSurplusValue,
      totalDeficitCost: activeStocktake.totalDeficitValue,
      status: 'POSTED',
      journalEntryId: `JE-${nextAdjNo}`,
      approvedBy: `المدير المالي: أ. هشام القدسي / المعتمد: ${cashierName}`,
      notes: activeStocktake.notes,
      createdAt: `${new Date().toISOString().split('T')[0]} ${new Date().toLocaleTimeString('ar-YE')}`,
      createdBy: cashierName,
    };

    // Calculate updated products stock
    const updatedProducts = products.map((prod) => {
      const diff = diffItems.find((di) => di.productId === prod.id);
      if (diff) {
        const qtyDelta = diff.differenceQty; // + for surplus, - for deficit
        const newStock = Math.max(0, prod.stock + qtyDelta);
        const newWhStocks = updateProductWarehouseStock(prod, activeStocktake.warehouseId, qtyDelta);
        return {
          ...prod,
          stock: newStock,
          warehouseStocks: newWhStocks,
        };
      }
      return prod;
    });

    // Mark stocktake as adjusted
    const updatedSession: StocktakingSession = {
      ...activeStocktake,
      status: 'ADJUSTED',
      adjustmentVoucherNo: nextAdjNo,
      adjustmentEntryId: `JE-${nextAdjNo}`,
      adjustedAt: `${new Date().toISOString().split('T')[0]} ${new Date().toLocaleTimeString('ar-YE')}`,
    };
    onUpdateStocktake(updatedSession);

    // Call parent to post adjustment, update products, and register Journal Entry
    onPostStockAdjustment(newAdjustment, updatedProducts);

    alert(`تم بنجاح ترحيل سند التسوية المخزنية رقم [${nextAdjNo}] وتحديث المخزون وقيد اليومية!`);
    setSubTab('ADJUSTMENTS');
    setSelectedAdjustmentId(newAdjustment.id);
  };

  // ----------------------------------------------------
  // FILTERED SESSIONS LIST
  // ----------------------------------------------------
  const filteredStocktakes = useMemo(() => {
    return stocktakes.filter(
      (st) =>
        stocktakeSearch === '' ||
        st.sessionNo.toLowerCase().includes(stocktakeSearch.toLowerCase()) ||
        st.warehouseName.toLowerCase().includes(stocktakeSearch.toLowerCase()) ||
        st.notes?.toLowerCase().includes(stocktakeSearch.toLowerCase())
    );
  }, [stocktakes, stocktakeSearch]);

  return (
    <div className="flex-1 flex flex-col h-full bg-[#0d1e33] text-slate-100 p-2 gap-2 select-none overflow-hidden font-sans" dir="rtl">
      {/* Module Sub-Header Navigation */}
      <div className="bg-[#122842] px-3 py-2 rounded-lg border border-[#1b3f66] shadow-sm flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center gap-1.5">
          <button
            id="btn-tab-audit"
            onClick={() => setSubTab('AUDIT')}
            className={`px-3.5 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'AUDIT'
                ? 'bg-emerald-600 text-white shadow ring-1 ring-emerald-400'
                : 'bg-[#0b1b2d] text-slate-300 hover:bg-[#16365a]'
            }`}
          >
            <ClipboardCheck className="w-4 h-4 text-emerald-300" />
            <span>محاضر الجرد الفعلي [F3]</span>
            <span className="bg-emerald-950 text-emerald-200 border border-emerald-600/50 px-1.5 py-0.2 rounded text-[10px]">
              {stocktakes.length} محضر
            </span>
          </button>

          <button
            id="btn-tab-adjustments"
            onClick={() => setSubTab('ADJUSTMENTS')}
            className={`px-3.5 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'ADJUSTMENTS'
                ? 'bg-amber-600 text-white shadow ring-1 ring-amber-400'
                : 'bg-[#0b1b2d] text-slate-300 hover:bg-[#16365a]'
            }`}
          >
            <SlidersHorizontal className="w-4 h-4 text-amber-300" />
            <span>سندات التسوية المخزنية [F4]</span>
            <span className="bg-amber-950 text-amber-200 border border-amber-600/50 px-1.5 py-0.2 rounded text-[10px]">
              {stockAdjustments.length} سند
            </span>
          </button>

          <button
            id="btn-tab-cards"
            onClick={() => setSubTab('CARDS')}
            className={`px-3.5 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'CARDS'
                ? 'bg-sky-600 text-white shadow ring-1 ring-sky-400'
                : 'bg-[#0b1b2d] text-slate-300 hover:bg-[#16365a]'
            }`}
          >
            <BookOpen className="w-4 h-4 text-sky-300" />
            <span>كرت حركة الصنف والرقابة [F5]</span>
          </button>
        </div>

        <div className="flex items-center gap-3 text-xs text-slate-300">
          <div className="flex items-center gap-1 bg-[#0b1b2d] px-2.5 py-1 rounded border border-[#1b3f66]">
            <Building2 className="w-3.5 h-3.5 text-sky-400" />
            <span className="text-slate-400">المستودعات المغطاة:</span>
            <span className="text-sky-300 font-bold">{warehouses.length} مستودع</span>
          </div>
        </div>
      </div>

      {/* ====================================================
          MODAL 1: NEW PHYSICAL STOCKTAKE SESSION
          ==================================================== */}
      <StocktakingNewModal
        showNewStocktakeModal={showNewStocktakeModal}
        setShowNewStocktakeModal={setShowNewStocktakeModal}
        handleStartNewStocktake={handleCreateNewStocktake}
        newStWarehouseId={newStWarehouseId}
        setNewStWarehouseId={setNewStWarehouseId}
        newStNotes={newStNotes}
        setNewStNotes={setNewStNotes}
        newStHeadMember={newStHeadMember}
        setNewStHeadMember={setNewStHeadMember}
        newStMember2={newStMember2}
        setNewStMember2={setNewStMember2}
        newStMember3={newStMember3}
        setNewStMember3={setNewStMember3}
        warehouses={warehouses}
        products={products}
      />

      {/* ====================================================
          MODAL 2: PRINT OFFICIAL STOCKTAKE REPORT (A4)
          ==================================================== */}
      <StocktakingPrintModal
        printStocktake={printStocktake}
        setPrintStocktake={setPrintStocktake}
        getWarehouseName={getWarehouseName}
        currency={currency}
      />

      {/* ====================================================
          MODAL 3: PRINT STOCK ADJUSTMENT VOUCHER (A4)
          ==================================================== */}
      <StocktakingPrintAdjustmentModal
        printAdjustment={printAdjustment}
        setPrintAdjustment={setPrintAdjustment}
        getWarehouseName={getWarehouseName}
        currency={currency}
      />

      {/* ====================================================
          MODAL 4: PRINT STOCK LEDGER CARD (A4)
          ==================================================== */}
      <StocktakingPrintCardModal
        printStockCard={printStockCard}
        setPrintStockCard={setPrintStockCard}
        selectedCardProduct={selectedCardProduct}
        ledgerEntries={stockMovements}
        warehouseName={cardWarehouseId === 'ALL' ? 'كافة المستودعات' : getWarehouseName(cardWarehouseId)}
        currency={currency}
      />


    </div>
  );
};
