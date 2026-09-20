'use client';

import React, { useState, useMemo, useCallback } from 'react';
import {
  WarehouseNewTransferModal,
  WarehousePrintTransferModal,
  WarehouseManageModal
} from './warehouse-transfers';
import {
  ProductItem,
  Warehouse,
  StockTransfer,
  StockTransferItem,
  StockTransferStatus,
} from '@/types/pos';
import { getProductStockInWarehouse } from '@/lib/mock';
import { generateNextId } from '@/lib/idGenerator';
import {
  Warehouse as WarehouseIcon,
  ArrowLeftRight,
  Plus,
  Search,
  Filter,
  CheckCircle2,
  Clock,
  Truck,
  XCircle,
  Printer,
  FileText,
  AlertTriangle,
  Building2,
  Phone,
  MapPin,
  UserCheck,
  PackageCheck,
  ArrowRight,
  Eye,
  Calendar,
  Layers,
  Sparkles,
  Barcode,
  X,
  RotateCcw,
  Check,
  ShieldCheck,
  Edit,
  Trash2,
} from 'lucide-react';

interface WarehouseTransfersViewProps {
  warehouses: Warehouse[];
  products: ProductItem[];
  transfers: StockTransfer[];
  onAddTransfer: (transfer: StockTransfer) => void;
  onUpdateTransferStatus: (transferId: string, newStatus: StockTransferStatus, notes?: string) => void;
  onAddWarehouse: (warehouse: Warehouse) => void;
  onUpdateWarehouse: (warehouse: Warehouse) => void;
  onUpdateProductsStock: (updatedProducts: ProductItem[]) => void;
  currency: string;
}

export const WarehouseTransfersView: React.FC<WarehouseTransfersViewProps> = ({
  warehouses,
  products,
  transfers,
  onAddTransfer,
  onUpdateTransferStatus,
  onAddWarehouse,
  onUpdateWarehouse,
  onUpdateProductsStock,
  currency,
}) => {
  // Navigation between sub-tabs
  const [subTab, setSubTab] = useState<'TRANSFERS' | 'WAREHOUSES' | 'STOCK_PER_WAREHOUSE'>('TRANSFERS');

  // Filters for Transfers
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState<'ALL' | StockTransferStatus>('ALL');
  const [sourceWhFilter, setSourceWhFilter] = useState<string>('ALL');
  const [destWhFilter, setDestWhFilter] = useState<string>('ALL');

  // Modals state
  const [showNewTransferModal, setShowNewTransferModal] = useState(false);
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [selectedTransferForPrint, setSelectedTransferForPrint] = useState<StockTransfer | null>(null);
  const [showNewWarehouseModal, setShowNewWarehouseModal] = useState(false);
  const [editingWarehouse, setEditingWarehouse] = useState<Warehouse | null>(null);
  const [selectedWarehouseForStock, setSelectedWarehouseForStock] = useState<string>(
    warehouses[0]?.id || 'WH-01'
  );

  // New Transfer Form State
  const [transferDate, setTransferDate] = useState(new Date().toISOString().split('T')[0]);
  const [fromWarehouseId, setFromWarehouseId] = useState<string>(warehouses[0]?.id || 'WH-01');
  const [toWarehouseId, setToWarehouseId] = useState<string>(warehouses[1]?.id || 'WH-02');
  const [driverName, setDriverName] = useState('');
  const [vehiclePlate, setVehiclePlate] = useState('');
  const [transferRef, setTransferRef] = useState('');
  const [transferNotes, setTransferNotes] = useState('');
  const [transferItems, setTransferItems] = useState<StockTransferItem[]>([]);
  const [transferError, setTransferError] = useState<string | null>(null);

  // Item row selector state in New Transfer modal
  const [selectedProductId, setSelectedProductId] = useState<string>('');
  const [selectedQty, setSelectedQty] = useState<number>(1);
  const [selectedUnit, setSelectedUnit] = useState<string>('');

  // Warehouse Form State
  const [whCode, setWhCode] = useState('');
  const [whName, setWhName] = useState('');
  const [whBranchId, setWhBranchId] = useState<number>(1);
  const [whBranchName, setWhBranchName] = useState('المركز الرئيسي - صنعاء');
  const [whManager, setWhManager] = useState('');
  const [whPhone, setWhPhone] = useState('');
  const [whLocation, setWhLocation] = useState('');
  const [whAccountCode, setWhAccountCode] = useState('1141');
  const [whNotes, setWhNotes] = useState('');
  const [whIsDefault, setWhIsDefault] = useState(false);

  const getWarehouseName = useCallback((warehouseId: string) => {
    return warehouses.find((w) => w.id === warehouseId)?.name || warehouseId;
  }, [warehouses]);

  // Filtered Transfers
  const filteredTransfers = useMemo(() => {
    return transfers.filter((t) => {
      const matchSearch =
        searchQuery === '' ||
        t.transferNo.toLowerCase().includes(searchQuery.toLowerCase()) ||
        t.fromWarehouseName.toLowerCase().includes(searchQuery.toLowerCase()) ||
        t.toWarehouseName.toLowerCase().includes(searchQuery.toLowerCase()) ||
        (t.driverName && t.driverName.toLowerCase().includes(searchQuery.toLowerCase())) ||
        (t.reference && t.reference.toLowerCase().includes(searchQuery.toLowerCase()));

      const matchStatus = statusFilter === 'ALL' || t.status === statusFilter;
      const matchSource = sourceWhFilter === 'ALL' || t.fromWarehouseId === sourceWhFilter;
      const matchDest = destWhFilter === 'ALL' || t.toWarehouseId === destWhFilter;

      return matchSearch && matchStatus && matchSource && matchDest;
    });
  }, [transfers, searchQuery, statusFilter, sourceWhFilter, destWhFilter]);

  // Statistics
  const totalTransfersCount = transfers.length;
  const inTransitTransfers = transfers.filter((t) => t.status === 'IN_TRANSIT');
  const inTransitValue = inTransitTransfers.reduce((sum, t) => sum + t.totalCost, 0);
  const receivedTransfersCount = transfers.filter((t) => t.status === 'RECEIVED').length;
  const totalWarehousesCount = warehouses.length;

  // Handler: Open Add Transfer Modal
  const handleOpenNewTransfer = () => {
    const nextNo = `TR-2026-${String(transfers.length + 1).padStart(4, '0')}`;
    setTransferDate(new Date().toISOString().split('T')[0]);
    setFromWarehouseId(warehouses[0]?.id || 'WH-01');
    setToWarehouseId(warehouses[1]?.id || 'WH-02');
    setDriverName('');
    setVehiclePlate('');
    setTransferRef(`REQ-${Date.now().toString().slice(-4)}`);
    setTransferNotes('');
    setTransferItems([]);
    setTransferError(null);
    setSelectedProductId(products[0]?.id || '');
    setSelectedQty(1);
    setSelectedUnit(products[0]?.unit || 'حبة');
    setShowNewTransferModal(true);
  };

  // Handler: Add Item to Transfer List
  const handleAddItemToTransfer = () => {
    if (!selectedProductId) return;
    const prod = products.find((p) => p.id === selectedProductId);
    if (!prod) return;

    // Check stock in source warehouse
    const currentStock = getProductStockInWarehouse(prod, fromWarehouseId);
    if (selectedQty <= 0) {
      setTransferError('يرجى تحديد كمية صحيحة أكبر من الصفر.');
      return;
    }

    // Check if already in list
    const existingIndex = transferItems.findIndex((item) => item.productId === prod.id);
    const existingQty = existingIndex >= 0 ? transferItems[existingIndex].quantity : 0;
    const totalRequiredQty = existingQty + selectedQty;

    if (totalRequiredQty > currentStock) {
      setTransferError(
        `الكمية المطلوبة (${totalRequiredQty}) تتجاوز الرصيد المتاح حالياً في مستودع المصدر (${currentStock} ${prod.unit}).`
      );
      return;
    }

    setTransferError(null);

    if (existingIndex >= 0) {
      const updated = [...transferItems];
      updated[existingIndex].quantity += selectedQty;
      updated[existingIndex].totalCost = updated[existingIndex].quantity * updated[existingIndex].unitCost;
      setTransferItems(updated);
    } else {
      const newItem: StockTransferItem = {
        id: generateNextId('STI'),
        productId: prod.id,
        productCode: prod.code,
        barcode: prod.barcode,
        productName: prod.name,
        unit: selectedUnit || prod.unit,
        quantity: selectedQty,
        unitCost: prod.costPrice,
        totalCost: selectedQty * prod.costPrice,
        availableStock: currentStock,
        batchNo: prod.batchNo,
        expiryDate: prod.expiryDate,
      };
      setTransferItems([...transferItems, newItem]);
    }

    setSelectedQty(1);
  };

  // Handler: Remove Item
  const handleRemoveTransferItem = (itemId: string) => {
    setTransferItems(transferItems.filter((i) => i.id !== itemId));
  };

  // Handler: Submit New Transfer
  const handleSubmitTransfer = (dispatchImmediately: boolean) => {
    if (fromWarehouseId === toWarehouseId) {
      setTransferError('لا يمكن التحويل لنفس المستودع! يرجى اختيار مستودع وجهة مختلف.');
      return;
    }

    if (transferItems.length === 0) {
      setTransferError('يرجى إضافة صنف واحد على الأقل إلى أمر التحويل.');
      return;
    }

    const fromWh = warehouses.find((w) => w.id === fromWarehouseId);
    const toWh = warehouses.find((w) => w.id === toWarehouseId);

    const totalQty = transferItems.reduce((sum, item) => sum + item.quantity, 0);
    const totalCost = transferItems.reduce((sum, item) => sum + item.totalCost, 0);

    const nextTransferNo = `TR-2026-${String(transfers.length + 1).padStart(4, '0')}`;
    const newStatus: StockTransferStatus = dispatchImmediately ? 'IN_TRANSIT' : 'DRAFT';

    const newTransfer: StockTransfer = {
      id: generateNextId('TR'),
      transferNo: nextTransferNo,
      date: transferDate,
      fromWarehouseId,
      fromWarehouseName: fromWh?.name || 'مستودع المصدر',
      toWarehouseId,
      toWarehouseName: toWh?.name || 'مستودع الوجهة',
      status: newStatus,
      items: transferItems,
      totalQuantity: totalQty,
      totalCost,
      reference: transferRef,
      notes: transferNotes,
      driverName: driverName.trim() || 'سائق النقل الداخلي',
      vehiclePlate: vehiclePlate.trim() || 'نقل داخلي',
      dispatchedBy: dispatchImmediately ? 'مدير المخازن' : undefined,
      dispatchedAt: dispatchImmediately ? new Date().toLocaleString('ar-YE') : undefined,
      createdAt: new Date().toLocaleString('ar-YE'),
      createdBy: 'أمين المستودع',
    };

    // If dispatched immediately, deduct from source warehouse stock
    if (dispatchImmediately) {
      applyStockDeduction(fromWarehouseId, transferItems);
    }

    onAddTransfer(newTransfer);
    setShowNewTransferModal(false);
  };

  // Helper: Deduct stock from source warehouse
  const applyStockDeduction = (sourceWhId: string, items: StockTransferItem[]) => {
    const updatedProducts = products.map((prod) => {
      const match = items.find((i) => i.productId === prod.id);
      if (!match) return prod;

      const currentWhStocks = prod.warehouseStocks
        ? [...prod.warehouseStocks]
        : warehouses.map((w) => ({
            warehouseId: w.id,
            quantity: getProductStockInWarehouse(prod, w.id),
          }));

      const sourceStockObj = currentWhStocks.find((w) => w.warehouseId === sourceWhId);
      if (sourceStockObj) {
        sourceStockObj.quantity = Math.max(0, sourceStockObj.quantity - match.quantity);
      }

      return {
        ...prod,
        warehouseStocks: currentWhStocks,
      };
    });

    onUpdateProductsStock(updatedProducts);
  };

  // Helper: Receive stock at destination warehouse
  const applyStockReceipt = (destWhId: string, items: StockTransferItem[]) => {
    const updatedProducts = products.map((prod) => {
      const match = items.find((i) => i.productId === prod.id);
      if (!match) return prod;

      const currentWhStocks = prod.warehouseStocks
        ? [...prod.warehouseStocks]
        : warehouses.map((w) => ({
            warehouseId: w.id,
            quantity: getProductStockInWarehouse(prod, w.id),
          }));

      const destStockObj = currentWhStocks.find((w) => w.warehouseId === destWhId);
      if (destStockObj) {
        destStockObj.quantity += match.quantity;
      } else {
        currentWhStocks.push({ warehouseId: destWhId, quantity: match.quantity });
      }

      return {
        ...prod,
        warehouseStocks: currentWhStocks,
      };
    });

    onUpdateProductsStock(updatedProducts);
  };

  // Helper: Cancel transfer and return stock to source if it was in transit
  const applyStockReturn = (sourceWhId: string, items: StockTransferItem[]) => {
    const updatedProducts = products.map((prod) => {
      const match = items.find((i) => i.productId === prod.id);
      if (!match) return prod;

      const currentWhStocks = prod.warehouseStocks
        ? [...prod.warehouseStocks]
        : warehouses.map((w) => ({
            warehouseId: w.id,
            quantity: getProductStockInWarehouse(prod, w.id),
          }));

      const sourceStockObj = currentWhStocks.find((w) => w.warehouseId === sourceWhId);
      if (sourceStockObj) {
        sourceStockObj.quantity += match.quantity;
      }

      return {
        ...prod,
        warehouseStocks: currentWhStocks,
      };
    });

    onUpdateProductsStock(updatedProducts);
  };

  // Action: Dispatch Draft Transfer
  const handleDispatchTransfer = (transfer: StockTransfer) => {
    if (!confirm(`هل تريد تأكيد إرسال وصرف البضاعة لأمر التحويل (${transfer.transferNo})؟\nسيتم خصم الكميات من ${transfer.fromWarehouseName}.`)) {
      return;
    }
    applyStockDeduction(transfer.fromWarehouseId, transfer.items);
    onUpdateTransferStatus(transfer.id, 'IN_TRANSIT', 'تم صرف وإرسال الشحنة مع السائق');
  };

  // Action: Confirm Receipt at Destination
  const handleReceiveTransfer = (transfer: StockTransfer) => {
    if (!confirm(`هل تريد تأكيد استلام البضاعة في (${transfer.toWarehouseName})؟\nسيتم إضافة الكميات فورياً لرصيد المستودع.`)) {
      return;
    }
    applyStockReceipt(transfer.toWarehouseId, transfer.items);
    onUpdateTransferStatus(transfer.id, 'RECEIVED', 'تم الاستلام والفحص والمطابقة في المستودع');
  };

  // Action: Cancel Transfer
  const handleCancelTransfer = (transfer: StockTransfer) => {
    const reason = prompt('يرجى كتابة سبب إلغاء أمر التحويل المخزني:');
    if (!reason) return;

    // If it was already in transit, return stock back to source warehouse
    if (transfer.status === 'IN_TRANSIT') {
      applyStockReturn(transfer.fromWarehouseId, transfer.items);
    }
    onUpdateTransferStatus(transfer.id, 'CANCELLED', reason);
  };

  // Open Print Note Modal
  const handlePrintTransfer = (transfer: StockTransfer) => {
    setSelectedTransferForPrint(transfer);
    setShowPrintModal(true);
  };

  // Warehouse Modal Handlers
  const handleOpenAddWarehouse = () => {
    setEditingWarehouse(null);
    setWhCode(`WH-${String(warehouses.length + 1).padStart(2, '0')}`);
    setWhName('');
    setWhBranchId(1);
    setWhBranchName('المركز الرئيسي - صنعاء');
    setWhManager('');
    setWhPhone('');
    setWhLocation('');
    setWhAccountCode(`114${warehouses.length + 1}`);
    setWhNotes('');
    setWhIsDefault(false);
    setShowNewWarehouseModal(true);
  };

  const handleOpenEditWarehouse = (wh: Warehouse) => {
    setEditingWarehouse(wh);
    setWhCode(wh.code);
    setWhName(wh.name);
    setWhBranchId(wh.branchId);
    setWhBranchName(wh.branchName);
    setWhManager(wh.managerName);
    setWhPhone(wh.phone);
    setWhLocation(wh.location);
    setWhAccountCode(wh.accountCode || '1141');
    setWhNotes(wh.notes || '');
    setWhIsDefault(wh.isDefault);
    setShowNewWarehouseModal(true);
  };

  const handleSaveWarehouse = () => {
    if (!whName.trim()) {
      alert('يرجى إدخال اسم المستودع.');
      return;
    }

    if (editingWarehouse) {
      onUpdateWarehouse({
        ...editingWarehouse,
        code: whCode,
        name: whName,
        branchId: whBranchId,
        branchName: whBranchName,
        managerName: whManager,
        phone: whPhone,
        location: whLocation,
        accountCode: whAccountCode,
        notes: whNotes,
        isDefault: whIsDefault,
      });
    } else {
      const newWh: Warehouse = {
        id: generateNextId('WH'),
        code: whCode || `WH-${String(warehouses.length + 1).padStart(2, '0')}`,
        name: whName,
        branchId: whBranchId,
        branchName: whBranchName,
        managerName: whManager || 'أمين المستودع',
        phone: whPhone,
        location: whLocation,
        accountCode: whAccountCode,
        notes: whNotes,
        isDefault: whIsDefault,
        isActive: true,
      };
      onAddWarehouse(newWh);
    }

    setShowNewWarehouseModal(false);
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f1f5f9] text-slate-800 select-none overflow-hidden font-sans p-2 gap-2">
      {/* 1. TOP SUMMARY CARDS (بطاقات المؤشرات المخزنية) */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-2 shrink-0">
        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-slate-500 font-bold flex items-center gap-1">
              <Building2 className="w-3.5 h-3.5 text-sky-600" />
              <span>المستودعات والفروع النشطة</span>
            </div>
            <div className="text-base font-bold font-mono text-slate-900 mt-0.5">
              {totalWarehousesCount} مستودعات
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              المستودع الرئيسي: <span className="font-bold text-sky-700">صنعاء</span>
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-sky-50 flex items-center justify-center text-sky-700">
            <WarehouseIcon className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-amber-700 font-bold flex items-center gap-1">
              <Truck className="w-3.5 h-3.5" />
              <span>بضاعة بالطريق (ترانزيت التحويلات)</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-amber-900 mt-0.5">
              {inTransitValue.toLocaleString()} <span className="text-xs font-normal">{currency}</span>
            </div>
            <div className="text-[10px] text-amber-600 font-semibold mt-0.5">
              {inTransitTransfers.length} شحنات قيد النقل بين الفروع
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-amber-50 flex items-center justify-center text-amber-700">
            <Truck className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-emerald-700 font-bold flex items-center gap-1">
              <PackageCheck className="w-3.5 h-3.5" />
              <span>التحويلات المستلمة والمعتمدة</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {receivedTransfersCount} تحويل ناجح
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              مجموع المناقلات المنفذة أصولاً
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-emerald-50 flex items-center justify-center text-emerald-700">
            <CheckCircle2 className="w-5 h-5" />
          </div>
        </div>

        <div className="bg-white p-2.5 rounded border border-slate-300 shadow-xs flex items-center justify-between">
          <div>
            <div className="text-[11px] text-indigo-700 font-bold flex items-center gap-1">
              <ArrowLeftRight className="w-3.5 h-3.5" />
              <span>إجمالي أوامر التحويل المسجلة</span>
            </div>
            <div className="text-base font-bold font-mono-numbers text-slate-900 mt-0.5">
              {totalTransfersCount} أمر تحويل
            </div>
            <div className="text-[10px] text-slate-500 mt-0.5">
              تتبع كامل لخطوط الإمداد
            </div>
          </div>
          <div className="w-9 h-9 rounded-full bg-indigo-50 flex items-center justify-center text-indigo-700">
            <ArrowLeftRight className="w-5 h-5" />
          </div>
        </div>
      </div>

      {/* 2. SUB-NAVIGATION TABS & TOOLBAR */}
      <div className="bg-white rounded border border-slate-300 p-2 shadow-xs flex flex-wrap items-center justify-between gap-2 shrink-0">
        <div className="flex items-center gap-1.5">
          <button
            onClick={() => setSubTab('TRANSFERS')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'TRANSFERS'
                ? 'bg-[#0f2d4a] text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <ArrowLeftRight className="w-3.5 h-3.5 text-sky-400" />
            <span>أوامر وسندات التحويل المخزني ({transfers.length})</span>
          </button>

          <button
            onClick={() => setSubTab('WAREHOUSES')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'WAREHOUSES'
                ? 'bg-[#0f2d4a] text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <WarehouseIcon className="w-3.5 h-3.5 text-emerald-400" />
            <span>دليل المستودعات والفروع ({warehouses.length})</span>
          </button>

          <button
            onClick={() => setSubTab('STOCK_PER_WAREHOUSE')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              subTab === 'STOCK_PER_WAREHOUSE'
                ? 'bg-[#0f2d4a] text-white shadow-xs'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Layers className="w-3.5 h-3.5 text-purple-400" />
            <span>أرصدة الأصناف حسب المستودع</span>
          </button>
        </div>

        <div className="flex items-center gap-2">
          {subTab === 'TRANSFERS' && (
            <button
              onClick={handleOpenNewTransfer}
              className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1 shadow-xs transition cursor-pointer"
            >
              <Plus className="w-3.5 h-3.5" />
              <span>أمر تحويل مخزني جديد [Ctrl+T]</span>
            </button>
          )}

          {subTab === 'WAREHOUSES' && (
            <button
              onClick={handleOpenAddWarehouse}
              className="bg-sky-600 hover:bg-sky-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1 shadow-xs transition cursor-pointer"
            >
              <Plus className="w-3.5 h-3.5" />
              <span>إضافة بطاقة مستودع جديد</span>
            </button>
          )}
        </div>
      </div>

      {/* 3. MAIN CONTENT BODY */}

      {/* VIEW A: TRANSFERS LIST (أوامر التحويل المخزني) */}
      {subTab === 'TRANSFERS' && (
        <div className="flex-1 bg-white rounded border border-slate-300 shadow-xs overflow-hidden flex flex-col">
          {/* Filters strip */}
          <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
            <div className="flex flex-wrap items-center gap-2">
              <div className="relative">
                <Search className="w-3.5 h-3.5 absolute right-2.5 top-2.5 text-slate-400" />
                <input
                  type="text"
                  placeholder="بحث برقم التحويل، المستودع، السائق..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="bg-white border border-slate-300 rounded text-xs py-1.5 pr-8 pl-2 w-56 focus:outline-none focus:border-sky-500"
                />
              </div>

              <select
                value={statusFilter}
                onChange={(e) => setStatusFilter(e.target.value as any)}
                className="bg-white border border-slate-300 rounded text-xs py-1.5 px-2 focus:outline-none focus:border-sky-500 cursor-pointer font-semibold"
              >
                <option value="ALL">كافة الحالات</option>
                <option value="DRAFT">مسودة (Draft)</option>
                <option value="IN_TRANSIT">قيد النقل (In-Transit)</option>
                <option value="RECEIVED">تم الاستلام (Received)</option>
                <option value="CANCELLED">ملغي (Cancelled)</option>
              </select>

              <select
                value={sourceWhFilter}
                onChange={(e) => setSourceWhFilter(e.target.value)}
                className="bg-white border border-slate-300 rounded text-xs py-1.5 px-2 focus:outline-none focus:border-sky-500 cursor-pointer"
              >
                <option value="ALL">من: كافة مستودعات المصدر</option>
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id}>
                    من: {w.name}
                  </option>
                ))}
              </select>

              <select
                value={destWhFilter}
                onChange={(e) => setDestWhFilter(e.target.value)}
                className="bg-white border border-slate-300 rounded text-xs py-1.5 px-2 focus:outline-none focus:border-sky-500 cursor-pointer"
              >
                <option value="ALL">إلى: كافة مستودعات الوجهة</option>
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id}>
                    إلى: {w.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="text-[11px] text-slate-500">
              إجمالي النتائج: <strong className="text-slate-800 font-mono">{filteredTransfers.length}</strong> أمر تحويل
            </div>
          </div>

          {/* Transfers DataGridView */}
          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse font-sans">
              <thead className="bg-[#1e3a5f] text-white sticky top-0 z-10 select-none text-[11px]">
                <tr>
                  <th className="p-2 border-l border-slate-700 w-28">رقم التحويل</th>
                  <th className="p-2 border-l border-slate-700 w-24">التاريخ</th>
                  <th className="p-2 border-l border-slate-700">مستودع المصدر (من)</th>
                  <th className="p-2 border-l border-slate-700">مستودع الوجهة (إلى)</th>
                  <th className="p-2 border-l border-slate-700 text-center w-24">عدد الأصناف</th>
                  <th className="p-2 border-l border-slate-700 text-center w-24">إجمالي الكمية</th>
                  <th className="p-2 border-l border-slate-700 text-left w-32">قيمة الشحنة ({currency})</th>
                  <th className="p-2 border-l border-slate-700 w-28 text-center">الحالة</th>
                  <th className="p-2 border-l border-slate-700">السائق / وسيلة النقل</th>
                  <th className="p-2 w-36 text-center">إجراءات المستند</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {filteredTransfers.length === 0 ? (
                  <tr>
                    <td colSpan={10} className="p-8 text-center text-slate-400 italic">
                      لا توجد أوامر تحويل مخزني مطابقة لمعايير البحث الحالية
                    </td>
                  </tr>
                ) : (
                  filteredTransfers.map((t, idx) => (
                    <tr
                      key={t.id}
                      className={`transition ${
                        idx % 2 === 0 ? 'bg-white hover:bg-sky-50/60' : 'bg-slate-50/60 hover:bg-sky-50/60'
                      }`}
                    >
                      <td className="p-2 font-mono font-bold text-sky-800 border-l border-slate-200">
                        {t.transferNo}
                      </td>

                      <td className="p-2 font-mono text-slate-600 border-l border-slate-200 text-[11px]">
                        {t.date}
                      </td>

                      <td className="p-2 border-l border-slate-200 font-bold text-slate-800">
                        <div className="flex items-center gap-1">
                          <Building2 className="w-3.5 h-3.5 text-slate-500 shrink-0" />
                          <span>{t.fromWarehouseName}</span>
                        </div>
                      </td>

                      <td className="p-2 border-l border-slate-200 font-bold text-slate-800">
                        <div className="flex items-center gap-1">
                          <ArrowRight className="w-3 h-3 text-sky-600 shrink-0" />
                          <span>{t.toWarehouseName}</span>
                        </div>
                      </td>

                      <td className="p-2 border-l border-slate-200 text-center font-mono font-semibold text-slate-700">
                        {t.items.length} أصناف
                      </td>

                      <td className="p-2 border-l border-slate-200 text-center font-mono font-bold text-indigo-700">
                        {t.totalQuantity}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-left font-mono-numbers font-bold text-slate-800">
                        {t.totalCost.toLocaleString()}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-center">
                        {t.status === 'RECEIVED' && (
                          <span className="bg-emerald-100 text-emerald-800 border border-emerald-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                            <CheckCircle2 className="w-3 h-3" />
                            <span>مستلم ومعتمد</span>
                          </span>
                        )}
                        {t.status === 'IN_TRANSIT' && (
                          <span className="bg-amber-100 text-amber-800 border border-amber-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                            <Truck className="w-3 h-3 text-amber-600 animate-pulse" />
                            <span>قيد النقل</span>
                          </span>
                        )}
                        {t.status === 'DRAFT' && (
                          <span className="bg-slate-100 text-slate-700 border border-slate-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                            <Clock className="w-3 h-3" />
                            <span>مسودة أمر</span>
                          </span>
                        )}
                        {t.status === 'CANCELLED' && (
                          <span className="bg-rose-100 text-rose-800 border border-rose-300 px-2 py-0.5 rounded text-[10px] font-bold inline-flex items-center gap-1">
                            <XCircle className="w-3 h-3" />
                            <span>ملغي</span>
                          </span>
                        )}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-[11px] text-slate-600">
                        {t.driverName ? (
                          <div className="flex items-center gap-1">
                            <span className="font-semibold text-slate-800">{t.driverName}</span>
                            {t.vehiclePlate && <span className="text-slate-400 font-mono">({t.vehiclePlate})</span>}
                          </div>
                        ) : (
                          <span className="text-slate-400 italic">نقل مباشر</span>
                        )}
                      </td>

                      <td className="p-2 text-center whitespace-nowrap">
                        <div className="flex items-center justify-center gap-1">
                          <button
                            onClick={() => handlePrintTransfer(t)}
                            title="طباعة إذن التحويل"
                            className="bg-slate-100 hover:bg-slate-200 text-slate-700 p-1 rounded border border-slate-300 cursor-pointer"
                          >
                            <Printer className="w-3.5 h-3.5" />
                          </button>

                          {t.status === 'DRAFT' && (
                            <button
                              onClick={() => handleDispatchTransfer(t)}
                              title="صرف وإرسال الشحنة (خصم من المصدر)"
                              className="bg-amber-600 hover:bg-amber-700 text-white px-2 py-0.5 rounded text-[10px] font-bold shadow-xs cursor-pointer flex items-center gap-1"
                            >
                              <Truck className="w-3 h-3" />
                              <span>إرسال</span>
                            </button>
                          )}

                          {t.status === 'IN_TRANSIT' && (
                            <button
                              onClick={() => handleReceiveTransfer(t)}
                              title="تأكيد استلام الشحنة في الوجهة (إضافة للرصيد)"
                              className="bg-emerald-600 hover:bg-emerald-700 text-white px-2 py-0.5 rounded text-[10px] font-bold shadow-xs cursor-pointer flex items-center gap-1"
                            >
                              <Check className="w-3 h-3" />
                              <span>استلام</span>
                            </button>
                          )}

                          {(t.status === 'DRAFT' || t.status === 'IN_TRANSIT') && (
                            <button
                              onClick={() => handleCancelTransfer(t)}
                              title="إلغاء التحويل"
                              className="bg-rose-50 hover:bg-rose-100 text-rose-700 p-1 rounded border border-rose-300 cursor-pointer"
                            >
                              <X className="w-3.5 h-3.5" />
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>

          {/* Footer note */}
          <div className="bg-[#1e293b] text-white px-3 py-1.5 text-xs flex items-center justify-between shrink-0 border-t border-slate-300">
            <div className="flex items-center gap-2 text-slate-300 text-[11px]">
              <ShieldCheck className="w-3.5 h-3.5 text-emerald-400" />
              <span>
                نظام التحويلات المزدوج يضمن سلامة القيود المخزنية، ومنع التحويل بالسالب، وتأكيد الاستلام عبر أمين المستودع المستلم.
              </span>
            </div>
            <div className="text-slate-400 text-[11px]">
              دورة التحويل: مسودة ⬅️ صرف وترانزيت ⬅️ استلام واعتماد
            </div>
          </div>
        </div>
      )}

      {/* VIEW B: WAREHOUSES DIRECTORY (دليل المستودعات) */}
      {subTab === 'WAREHOUSES' && (
        <div className="flex-1 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3 overflow-y-auto">
          {warehouses.map((wh) => {
            // Calculate stock valuation for this warehouse
            const totalStockInWh = products.reduce((sum, p) => {
              return sum + getProductStockInWarehouse(p, wh.id);
            }, 0);

            const totalValuationInWh = products.reduce((sum, p) => {
              const qty = getProductStockInWarehouse(p, wh.id);
              return sum + qty * p.costPrice;
            }, 0);

            return (
              <div
                key={wh.id}
                className="bg-white rounded-lg border border-slate-300 shadow-xs overflow-hidden flex flex-col justify-between hover:shadow-md transition"
              >
                <div>
                  <div className="bg-slate-100 border-b border-slate-200 text-slate-800 p-2.5 flex items-center justify-between">
                    <div className="flex items-center gap-2 font-bold text-xs text-slate-900">
                      <WarehouseIcon className="w-4 h-4 text-sky-700" />
                      <span>{wh.name}</span>
                    </div>
                    {wh.isDefault && (
                      <span className="bg-emerald-600 text-white px-2 py-0.5 rounded text-[10px] font-bold">
                        رئيسي
                      </span>
                    )}
                  </div>

                  <div className="p-3 space-y-2.5 text-xs text-slate-700">
                    <div className="flex items-center justify-between pb-1 border-b border-slate-100">
                      <span className="text-slate-500 text-[11px]">كود المستودع:</span>
                      <span className="font-mono font-bold text-slate-900">{wh.code}</span>
                    </div>

                    <div className="flex items-center justify-between pb-1 border-b border-slate-100">
                      <span className="text-slate-500 text-[11px]">الفرع التابع:</span>
                      <span className="font-semibold text-slate-800">{wh.branchName}</span>
                    </div>

                    <div className="flex items-center justify-between pb-1 border-b border-slate-100">
                      <span className="text-slate-500 text-[11px] flex items-center gap-1">
                        <UserCheck className="w-3.5 h-3.5 text-slate-400" />
                        <span>أمين المستودع:</span>
                      </span>
                      <span className="font-semibold text-slate-900">{wh.managerName}</span>
                    </div>

                    <div className="flex items-center justify-between pb-1 border-b border-slate-100">
                      <span className="text-slate-500 text-[11px] flex items-center gap-1">
                        <Phone className="w-3.5 h-3.5 text-slate-400" />
                        <span>رقم الهاتف:</span>
                      </span>
                      <span className="font-mono text-slate-800">{wh.phone || 'غير مسجل'}</span>
                    </div>

                    <div className="flex items-start justify-between pb-1 border-b border-slate-100">
                      <span className="text-slate-500 text-[11px] flex items-center gap-1 shrink-0">
                        <MapPin className="w-3.5 h-3.5 text-slate-400" />
                        <span>الموقع الجغرافي:</span>
                      </span>
                      <span className="text-right text-[11px] text-slate-800 max-w-[180px]">{wh.location}</span>
                    </div>

                    {/* Stock Valuation Box */}
                    <div className="p-2 bg-slate-50 rounded border border-slate-200 mt-2 grid grid-cols-2 gap-2 text-center">
                      <div>
                        <div className="text-[10px] text-slate-500">إجمالي كمية البضاعة</div>
                        <div className="font-bold font-mono text-indigo-700 text-sm mt-0.5">
                          {totalStockInWh.toLocaleString()}
                        </div>
                      </div>
                      <div>
                        <div className="text-[10px] text-slate-500">قيمة التقييم بالتكلفة</div>
                        <div className="font-bold font-mono-numbers text-emerald-700 text-sm mt-0.5">
                          {totalValuationInWh.toLocaleString()} {currency}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="p-2 bg-slate-50 border-t border-slate-200 flex items-center justify-between text-xs">
                  <button
                    onClick={() => {
                      setSelectedWarehouseForStock(wh.id);
                      setSubTab('STOCK_PER_WAREHOUSE');
                    }}
                    className="text-sky-700 hover:text-sky-900 font-bold flex items-center gap-1 cursor-pointer"
                  >
                    <Layers className="w-3.5 h-3.5" />
                    <span>عرض الأصناف والجرد</span>
                  </button>

                  <button
                    onClick={() => handleOpenEditWarehouse(wh)}
                    className="bg-white hover:bg-slate-100 text-slate-700 px-2 py-1 rounded border border-slate-300 font-semibold flex items-center gap-1 cursor-pointer"
                  >
                    <Edit className="w-3 h-3" />
                    <span>تعديل</span>
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {/* VIEW C: STOCK PER WAREHOUSE (أرصدة الأصناف حسب المستودع) */}
      {subTab === 'STOCK_PER_WAREHOUSE' && (
        <div className="flex-1 bg-white rounded border border-slate-300 shadow-xs overflow-hidden flex flex-col">
          <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-wrap items-center justify-between gap-2 text-xs">
            <div className="flex items-center gap-2">
              <label className="font-bold text-slate-700">اختر المستودع لعرض أرصدته:</label>
              <select
                value={selectedWarehouseForStock}
                onChange={(e) => setSelectedWarehouseForStock(e.target.value)}
                className="bg-white border border-slate-300 rounded text-xs py-1.5 px-3 font-bold text-sky-900 focus:outline-none focus:border-sky-500 cursor-pointer"
              >
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id}>
                    {w.name} ({w.code})
                  </option>
                ))}
              </select>
            </div>

            <div className="text-xs text-slate-600 flex items-center gap-4">
              <span>
                عدد الأصناف في المستودع: <strong className="font-mono text-slate-900">{products.length}</strong>
              </span>
              <span>
                إجمالي كمية المخزون:{' '}
                <strong className="font-mono text-indigo-700">
                  {products
                    .reduce((sum, p) => sum + getProductStockInWarehouse(p, selectedWarehouseForStock), 0)
                    .toLocaleString()}
                </strong>
              </span>
              <span>
                إجمالي قيمة المخزون بالتكلفة:{' '}
                <strong className="font-mono-numbers text-emerald-700">
                  {products
                    .reduce(
                      (sum, p) => sum + getProductStockInWarehouse(p, selectedWarehouseForStock) * p.costPrice,
                      0
                    )
                    .toLocaleString()}{' '}
                  {currency}
                </strong>
              </span>
            </div>
          </div>

          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse font-sans">
              <thead className="bg-[#1e3a5f] text-white sticky top-0 z-10 select-none text-[11px]">
                <tr>
                  <th className="p-2 border-l border-slate-700 w-16 text-center">الكود</th>
                  <th className="p-2 border-l border-slate-700 w-28">الباركود</th>
                  <th className="p-2 border-l border-slate-700">اسم الصنف</th>
                  <th className="p-2 border-l border-slate-700 w-24">التصنيف</th>
                  <th className="p-2 border-l border-slate-700 w-20 text-center">الوحدة</th>
                  <th className="p-2 border-l border-slate-700 text-center w-28">الرصيد في المستودع</th>
                  <th className="p-2 border-l border-slate-700 text-center w-24">إجمالي الشركة</th>
                  <th className="p-2 border-l border-slate-700 text-left w-28">سعر التكلفة</th>
                  <th className="p-2 border-l border-slate-700 text-left w-28">سعر البيع</th>
                  <th className="p-2 border-l border-slate-700 text-left w-32">قيمة الرصيد بالتكلفة</th>
                  <th className="p-2 w-28 text-center">حالة المخزون</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200 font-mono">
                {products.map((p, idx) => {
                  const whQty = getProductStockInWarehouse(p, selectedWarehouseForStock);
                  const isLow = whQty <= p.minStock;
                  return (
                    <tr
                      key={p.id}
                      className={`transition ${
                        idx % 2 === 0 ? 'bg-white hover:bg-sky-50/60' : 'bg-slate-50/60 hover:bg-sky-50/60'
                      }`}
                    >
                      <td className="p-2 text-center border-l border-slate-200 text-slate-700 font-bold">
                        {p.code}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-slate-600 text-[11px]">
                        {p.barcode}
                      </td>

                      <td className="p-2 font-sans font-bold text-slate-900 border-l border-slate-200">
                        {p.name}
                      </td>

                      <td className="p-2 font-sans text-slate-600 border-l border-slate-200 text-[11px]">
                        {p.category}
                      </td>

                      <td className="p-2 font-sans text-center border-l border-slate-200 text-slate-700">
                        {p.unit}
                      </td>

                      <td
                        className={`p-2 border-l border-slate-200 text-center font-bold text-sm ${
                          whQty === 0 ? 'text-rose-600 bg-rose-50/40' : isLow ? 'text-amber-700 bg-amber-50/40' : 'text-slate-900'
                        }`}
                      >
                        {whQty.toLocaleString()}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-center text-slate-500">
                        {p.stock.toLocaleString()}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-left text-slate-700">
                        {p.costPrice.toLocaleString()}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-left font-bold text-emerald-800">
                        {p.salePrice.toLocaleString()}
                      </td>

                      <td className="p-2 border-l border-slate-200 text-left font-bold text-slate-900">
                        {(whQty * p.costPrice).toLocaleString()}
                      </td>

                      <td className="p-2 text-center font-sans">
                        {whQty === 0 ? (
                          <span className="bg-rose-100 text-rose-800 px-2 py-0.5 rounded text-[10px] font-bold">
                            منعدم
                          </span>
                        ) : isLow ? (
                          <span className="bg-amber-100 text-amber-800 px-2 py-0.5 rounded text-[10px] font-bold">
                            منخفض
                          </span>
                        ) : (
                          <span className="bg-emerald-100 text-emerald-800 px-2 py-0.5 rounded text-[10px] font-bold">
                            متوفر
                          </span>
                        )}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* 4. MODAL: NEW STOCK TRANSFER (أمر تحويل مخزني جديد) */}
      <WarehouseNewTransferModal
        showNewTransferModal={showNewTransferModal}
        setShowNewTransferModal={setShowNewTransferModal}
        handleSubmitTransfer={handleSubmitTransfer}
        transferDate={transferDate}
        setTransferDate={setTransferDate}
        fromWarehouseId={fromWarehouseId}
        setFromWarehouseId={setFromWarehouseId}
        toWarehouseId={toWarehouseId}
        setToWarehouseId={setToWarehouseId}
        driverName={driverName}
        setDriverName={setDriverName}
        vehiclePlate={vehiclePlate}
        setVehiclePlate={setVehiclePlate}
        transferRef={transferRef}
        setTransferRef={setTransferRef}
        transferNotes={transferNotes}
        setTransferNotes={setTransferNotes}
        transferItems={transferItems}
        transferError={transferError}
        warehouses={warehouses}
        products={products}
        selectedProductId={selectedProductId}
        setSelectedProductId={setSelectedProductId}
        selectedQty={selectedQty}
        setSelectedQty={setSelectedQty}
        selectedUnit={selectedUnit}
        setSelectedUnit={setSelectedUnit}
        handleAddItemToTransfer={handleAddItemToTransfer}
        handleRemoveTransferItem={handleRemoveTransferItem}
        getProductStockInWarehouse={getProductStockInWarehouse}
        currency={currency}
      />

      {/* --- A4 Print Modal --- */}
      <WarehousePrintTransferModal
        showPrintModal={showPrintModal}
        setShowPrintModal={setShowPrintModal}
        selectedTransferForPrint={selectedTransferForPrint}
        getWarehouseName={getWarehouseName}
        currency={currency}
      />

      {/* --- Warehouse Create/Edit Modal --- */}
      <WarehouseManageModal
        showNewWarehouseModal={showNewWarehouseModal}
        setShowNewWarehouseModal={setShowNewWarehouseModal}
        handleSaveWarehouse={handleSaveWarehouse}
        editingWarehouse={editingWarehouse}
        whCode={whCode}
        setWhCode={setWhCode}
        whName={whName}
        setWhName={setWhName}
        whBranchId={whBranchId}
        setWhBranchId={setWhBranchId}
        whBranchName={whBranchName}
        setWhBranchName={setWhBranchName}
        whManager={whManager}
        setWhManager={setWhManager}
        whPhone={whPhone}
        setWhPhone={setWhPhone}
        whLocation={whLocation}
        setWhLocation={setWhLocation}
        whAccountCode={whAccountCode}
        setWhAccountCode={setWhAccountCode}
        whNotes={whNotes}
        setWhNotes={setWhNotes}
        whIsDefault={whIsDefault}
        setWhIsDefault={setWhIsDefault}
      />

    </div>
  );
};
