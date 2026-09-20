import { ProductItem, WarehouseStock, FifoStockLayer, Invoice, StockTransfer, StockAdjustment, StockLedgerItem } from '@/types/pos';

export const getProductStockInWarehouse = (product: ProductItem, warehouseId: string): number => {
  if (product.warehouseStocks && product.warehouseStocks.length > 0) {
    const found = product.warehouseStocks.find((w) => w.warehouseId === warehouseId);
    if (found !== undefined) return found.quantity;
  }
  // Default distribution if warehouseStocks array is not populated yet
  if (warehouseId === 'WH-01') return Math.floor(product.stock * 0.65);
  if (warehouseId === 'WH-02') return Math.floor(product.stock * 0.25);
  if (warehouseId === 'WH-03') return Math.max(0, product.stock - Math.floor(product.stock * 0.65) - Math.floor(product.stock * 0.25));
  return 0;
};

export const updateProductWarehouseStock = (
  product: ProductItem,
  warehouseId: string,
  quantityDelta: number
): WarehouseStock[] => {
  const currentStocks: WarehouseStock[] = product.warehouseStocks && product.warehouseStocks.length > 0
    ? product.warehouseStocks.map((ws) => ({ ...ws }))
    : [
        { warehouseId: 'WH-01', quantity: Math.floor(product.stock * 0.65) },
        { warehouseId: 'WH-02', quantity: Math.floor(product.stock * 0.25) },
        { warehouseId: 'WH-03', quantity: Math.max(0, product.stock - Math.floor(product.stock * 0.65) - Math.floor(product.stock * 0.25)) },
      ];

  const existingIndex = currentStocks.findIndex((ws) => ws.warehouseId === warehouseId);
  if (existingIndex > -1) {
    currentStocks[existingIndex] = {
      ...currentStocks[existingIndex],
      quantity: Math.max(0, currentStocks[existingIndex].quantity + quantityDelta),
    };
  } else {
    currentStocks.push({
      warehouseId,
      quantity: Math.max(0, quantityDelta),
    });
  }
  return currentStocks;
};

export const calculateWeightedAverageCost = (
  currentStock: number,
  currentCost: number,
  incomingQuantity: number,
  incomingUnitCost: number
): number => {
  const totalStock = currentStock + incomingQuantity;
  if (totalStock <= 0) return incomingUnitCost > 0 ? incomingUnitCost : currentCost;
  const totalValue = (currentStock * currentCost) + (incomingQuantity * incomingUnitCost);
  return Math.round(totalValue / totalStock);
};

// =========================================================================
// FIFO Costing Engine (الوارد أولاً يصرف أولاً - First-In First-Out)
// =========================================================================

export const getOrInitFifoLayers = (product: ProductItem): FifoStockLayer[] => {
  if (product.fifoLayers && product.fifoLayers.length > 0) {
    return product.fifoLayers;
  }
  const stock = product.stock || 0;
  if (stock <= 0) return [];

  // Create two realistic chronological batches for demonstration of FIFO:
  const layer1Qty = Math.max(1, Math.floor(stock * 0.45));
  const layer2Qty = Math.max(0, stock - layer1Qty);
  const cost = product.costPrice || 1000;
  // Oldest layer arrived at slightly lower historical purchase price:
  const olderCost = Math.round(cost * 0.94);

  const isPerishable = product.hasExpiryDate !== false;
  const layerExpiry1 = isPerishable ? (product.expiryDate || '2027-01-30') : undefined;
  const layerExpiry2 = isPerishable ? (product.expiryDate || '2027-06-30') : undefined;

  const layers: FifoStockLayer[] = [
    {
      id: `FIFO-${product.id}-L1`,
      purchaseInvoiceNo: 'PINV-1000',
      date: '2026-07-15',
      warehouseId: 'WH-01',
      batchNo: product.batchNo ? `${product.batchNo}-A` : 'BT-01',
      expiryDate: layerExpiry1,
      initialQuantity: layer1Qty,
      remainingQuantity: layer1Qty,
      unitCost: olderCost,
    },
  ];

  if (layer2Qty > 0) {
    layers.push({
      id: `FIFO-${product.id}-L2`,
      purchaseInvoiceNo: 'PINV-1001',
      date: '2026-08-25',
      warehouseId: 'WH-01',
      batchNo: product.batchNo || 'BT-02',
      expiryDate: layerExpiry2,
      initialQuantity: layer2Qty,
      remainingQuantity: layer2Qty,
      unitCost: cost,
    });
  }

  return layers;
};

export interface FifoDeductionResult {
  updatedProduct: ProductItem;
  totalCogs: number;
  valuationMethod: 'FIFO' | 'FEFO';
  layersDeducted: Array<{
    layerId: string;
    batchNo?: string;
    expiryDate?: string;
    quantity: number;
    unitCost: number;
    totalCost: number;
  }>;
}

/**
 * Determine valuation and dispatch method:
 * - FEFO / EEFO (First-Expired / Earliest-Expiry, First-Out): للأصناف الخاضعة لتاريخ الصلاحية (الأقرب انتهاءً يصرف أولاً)
 * - FIFO (First-In, First-Out): للأصناف الدائمة التي ليس لها تاريخ انتهاء (الوارد أولاً يصرف أولاً)
 */
export const getProductValuationMethod = (product: ProductItem): 'FIFO' | 'FEFO' => {
  if (product.valuationMethod) return product.valuationMethod;
  return product.hasExpiryDate !== false ? 'FEFO' : 'FIFO';
};

/**
 * Process stock deduction using FEFO for perishable goods or FIFO for non-perishable goods.
 * FEFO: Deducts sequentially from the closest-to-expiry stock layers first.
 * FIFO: Deducts sequentially from the earliest-received stock layers first.
 */
export const processFifoDeduction = (
  product: ProductItem,
  quantityToDeduct: number,
  warehouseId?: string
): FifoDeductionResult => {
  const method = getProductValuationMethod(product);
  const currentLayers = (product.fifoLayers && product.fifoLayers.length > 0)
    ? product.fifoLayers.map((l) => ({ ...l }))
    : getOrInitFifoLayers(product);

  let remainingToDeduct = quantityToDeduct;
  let totalCogs = 0;
  const layersDeducted: FifoDeductionResult['layersDeducted'] = [];

  // Sort layers based on the valuation & dispatch policy:
  // FEFO / EEFO: Earliest Expiry Date First
  // FIFO: Earliest Arrival / Receipt Date First
  currentLayers.sort((a, b) => {
    if (method === 'FEFO') {
      if (a.expiryDate && b.expiryDate) {
        const timeA = new Date(a.expiryDate).getTime();
        const timeB = new Date(b.expiryDate).getTime();
        if (timeA !== timeB) return timeA - timeB; // Earliest expiry first
      } else if (a.expiryDate && !b.expiryDate) {
        return -1; // Prioritize layers with expiry date
      } else if (!a.expiryDate && b.expiryDate) {
        return 1;
      }
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    } else {
      // FIFO: Oldest arrival date first
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    }
  });

  for (const layer of currentLayers) {
    if (remainingToDeduct <= 0) break;
    if (warehouseId && layer.warehouseId && layer.warehouseId !== warehouseId) {
      continue;
    }
    if (layer.remainingQuantity > 0) {
      const deductFromThisLayer = Math.min(layer.remainingQuantity, remainingToDeduct);
      layer.remainingQuantity -= deductFromThisLayer;
      remainingToDeduct -= deductFromThisLayer;
      const layerCost = deductFromThisLayer * layer.unitCost;
      totalCogs += layerCost;

      layersDeducted.push({
        layerId: layer.id,
        batchNo: layer.batchNo,
        expiryDate: layer.expiryDate,
        quantity: deductFromThisLayer,
        unitCost: layer.unitCost,
        totalCost: layerCost,
      });
    }
  }

  // Fallback if requested quantity exceeds tracked layers
  if (remainingToDeduct > 0) {
    const fallbackCost = product.costPrice || 1000;
    const fallbackCogs = remainingToDeduct * fallbackCost;
    totalCogs += fallbackCogs;
    layersDeducted.push({
      layerId: method === 'FEFO' ? 'FALLBACK-FEFO' : 'FALLBACK-FIFO',
      batchNo: product.batchNo,
      expiryDate: product.expiryDate,
      quantity: remainingToDeduct,
      unitCost: fallbackCost,
      totalCost: fallbackCogs,
    });
  }

  // Active cost price displayed for the next sale is the next active layer according to policy:
  const nextActiveLayer = currentLayers.find((l) => l.remainingQuantity > 0);
  const activeCostPrice = nextActiveLayer ? nextActiveLayer.unitCost : product.costPrice;

  const newTotalStock = Math.max(0, (product.stock || 0) - quantityToDeduct);

  const updatedProduct: ProductItem = {
    ...product,
    stock: newTotalStock,
    costPrice: activeCostPrice,
    valuationMethod: method,
    fifoLayers: currentLayers,
  };

  return {
    updatedProduct,
    totalCogs,
    valuationMethod: method,
    layersDeducted,
  };
};

/**
 * Add a new inbound batch/layer to FIFO queue on Purchase
 */
export const addFifoPurchaseLayer = (
  product: ProductItem,
  newLayer: {
    purchaseInvoiceNo?: string;
    date: string;
    warehouseId?: string;
    batchNo?: string;
    expiryDate?: string;
    quantity: number;
    unitCost: number;
  }
): ProductItem => {
  const currentLayers = (product.fifoLayers && product.fifoLayers.length > 0)
    ? product.fifoLayers.map((l) => ({ ...l }))
    : getOrInitFifoLayers(product);

  const createdLayer: FifoStockLayer = {
    id: `FIFO-${product.id}-${Date.now()}-${Math.floor(Math.random() * 1000)}`,
    purchaseInvoiceNo: newLayer.purchaseInvoiceNo,
    date: newLayer.date,
    warehouseId: newLayer.warehouseId || 'WH-01',
    batchNo: newLayer.batchNo || product.batchNo,
    expiryDate: product.hasExpiryDate !== false ? (newLayer.expiryDate || product.expiryDate) : undefined,
    initialQuantity: newLayer.quantity,
    remainingQuantity: newLayer.quantity,
    unitCost: newLayer.unitCost,
  };

  currentLayers.push(createdLayer);
  const method = getProductValuationMethod(product);
  currentLayers.sort((a, b) => {
    if (method === 'FEFO') {
      if (a.expiryDate && b.expiryDate) {
        const timeA = new Date(a.expiryDate).getTime();
        const timeB = new Date(b.expiryDate).getTime();
        if (timeA !== timeB) return timeA - timeB;
      } else if (a.expiryDate && !b.expiryDate) return -1;
      else if (!a.expiryDate && b.expiryDate) return 1;
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    } else {
      return new Date(a.date).getTime() - new Date(b.date).getTime();
    }
  });

  const nextActiveLayer = currentLayers.find((l) => l.remainingQuantity > 0);
  const activeCostPrice = nextActiveLayer ? nextActiveLayer.unitCost : newLayer.unitCost;

  return {
    ...product,
    stock: (product.stock || 0) + newLayer.quantity,
    costPrice: activeCostPrice,
    valuationMethod: method,
    fifoLayers: currentLayers,
  };
};

/**
 * Generate Chronological Stock Ledger Card (كرت حركة الصنف)
 */
export function generateProductStockLedger(
  productId: string,
  warehouseId: string | 'ALL',
  product: ProductItem,
  salesInvoices: Invoice[],
  purchaseInvoices: Invoice[],
  transfers: StockTransfer[],
  adjustments: StockAdjustment[]
): StockLedgerItem[] {
  const movements: {
    date: string;
    time: string;
    sortKey: string;
    movementType: StockLedgerItem['movementType'];
    movementLabel: string;
    documentNo: string;
    warehouseId: string;
    warehouseName: string;
    inQty: number;
    outQty: number;
    unitPriceOrCost: number;
    partyName?: string;
    notes?: string;
  }[] = [];

  // 1. Initial Opening Balance baseline (2026-08-01)
  const isAll = warehouseId === 'ALL';
  const openingQty = isAll ? 50 : (warehouseId === 'WH-01' ? 40 : 10);
  movements.push({
    date: '2026-08-01',
    time: '08:00 ص',
    sortKey: '2026-08-01T08:00:00',
    movementType: 'OPENING_BALANCE',
    movementLabel: 'رصيد افتتاحي أول المدة',
    documentNo: 'OB-2026',
    warehouseId: isAll ? 'WH-01' : warehouseId,
    warehouseName: isAll ? 'كافة المستودعات (رصيد افتتاحي)' : (warehouseId === 'WH-01' ? 'المستودع الرئيسي' : 'مستودع فرع حدة'),
    inQty: openingQty,
    outQty: 0,
    unitPriceOrCost: product.costPrice,
    notes: 'رصيد افتتاحي مرحل من الدورة السابقة',
  });

  // 2. Purchases & Purchase Returns
  purchaseInvoices.forEach((pinv) => {
    if (!isAll && pinv.warehouseId && pinv.warehouseId !== warehouseId) return;

    const item = pinv.items.find((ci) => ci.item.id === productId);
    if (item && item.quantity > 0) {
      const isReturn = pinv.type === 'PURCHASE_RETURN';
      movements.push({
        date: pinv.date,
        time: pinv.time || '10:00 ص',
        sortKey: `${pinv.date}T${pinv.time || '10:00:00'}`,
        movementType: isReturn ? 'PURCHASE_RETURN' : 'PURCHASE',
        movementLabel: isReturn ? 'مردودات مشتريات لمورد' : 'فاتورة مشتريات وتوريد',
        documentNo: pinv.invoiceNo,
        warehouseId: pinv.warehouseId || 'WH-01',
        warehouseName: pinv.warehouseName || 'المستودع الرئيسي',
        inQty: isReturn ? 0 : item.quantity,
        outQty: isReturn ? item.quantity : 0,
        unitPriceOrCost: item.unitPrice || item.unitCost || product.costPrice,
        partyName: `المورد: ${pinv.customerOrSupplierName}`,
        notes: pinv.notes,
      });
    }
  });

  // 3. Sales & Sales Returns
  salesInvoices.forEach((sinv) => {
    const invWh = sinv.warehouseId || 'WH-01';
    if (!isAll && invWh !== warehouseId) return;

    const item = sinv.items.find((ci) => ci.item.id === productId);
    if (item && item.quantity > 0) {
      const isReturn = sinv.type === 'SALE_RETURN';
      movements.push({
        date: sinv.date,
        time: sinv.time || '12:00 م',
        sortKey: `${sinv.date}T${sinv.time || '12:00:00'}`,
        movementType: isReturn ? 'SALE_RETURN' : 'SALE',
        movementLabel: isReturn ? 'مرتجع مبيعات من عميل' : 'فاتورة مبيعات POS',
        documentNo: sinv.invoiceNo,
        warehouseId: invWh,
        warehouseName: sinv.warehouseName || (invWh === 'WH-01' ? 'المستودع الرئيسي' : 'فرع حدة'),
        inQty: isReturn ? item.quantity : 0,
        outQty: isReturn ? 0 : item.quantity,
        unitPriceOrCost: item.unitPrice || product.salePrice,
        partyName: sinv.customerOrSupplierName ? `العميل: ${sinv.customerOrSupplierName}` : 'عميل نقدي عام',
        notes: sinv.notes,
      });
    }
  });

  // 4. Warehouse Transfers
  transfers.forEach((tr) => {
    if (tr.status === 'CANCELLED') return;
    const item = tr.items.find((ti) => ti.productId === productId);
    if (item && item.quantity > 0) {
      if (isAll || tr.fromWarehouseId === warehouseId) {
        movements.push({
          date: tr.date,
          time: '01:00 م',
          sortKey: `${tr.date}T13:00:00`,
          movementType: 'TRANSFER_OUT',
          movementLabel: `مناقلة صادر إلى (${tr.toWarehouseName})`,
          documentNo: tr.transferNo,
          warehouseId: tr.fromWarehouseId,
          warehouseName: tr.fromWarehouseName,
          inQty: 0,
          outQty: item.quantity,
          unitPriceOrCost: item.unitCost || product.costPrice,
          partyName: tr.driverName ? `السائق: ${tr.driverName}` : undefined,
          notes: tr.notes,
        });
      }

      if (isAll || tr.toWarehouseId === warehouseId) {
        movements.push({
          date: tr.date,
          time: '03:00 م',
          sortKey: `${tr.date}T15:00:00`,
          movementType: 'TRANSFER_IN',
          movementLabel: `مناقلة وارد من (${tr.fromWarehouseName})`,
          documentNo: tr.transferNo,
          warehouseId: tr.toWarehouseId,
          warehouseName: tr.toWarehouseName,
          inQty: item.quantity,
          outQty: 0,
          unitPriceOrCost: item.unitCost || product.costPrice,
          partyName: tr.receivedBy ? `المستلم: ${tr.receivedBy}` : undefined,
          notes: tr.notes,
        });
      }
    }
  });

  // 5. Stock Adjustments (Surplus & Deficit)
  adjustments.forEach((adj) => {
    if (!isAll && adj.warehouseId !== warehouseId) return;
    const item = adj.items.find((ai) => ai.productId === productId);
    if (item) {
      const isSurplus = item.adjustmentQty > 0;
      movements.push({
        date: adj.date,
        time: adj.time || '04:00 م',
        sortKey: `${adj.date}T${adj.time || '16:00:00'}`,
        movementType: isSurplus ? 'ADJUSTMENT_SURPLUS' : 'ADJUSTMENT_DEFICIT',
        movementLabel: isSurplus ? 'تسوية مخزنية (فائض جرد)' : 'تسوية مخزنية (عجز / تلف)',
        documentNo: adj.adjustmentNo,
        warehouseId: adj.warehouseId,
        warehouseName: adj.warehouseName,
        inQty: isSurplus ? Math.abs(item.adjustmentQty) : 0,
        outQty: !isSurplus ? Math.abs(item.adjustmentQty) : 0,
        unitPriceOrCost: item.unitCost || product.costPrice,
        partyName: adj.approvedBy,
        notes: item.notes || adj.reason,
      });
    }
  });

  movements.sort((a, b) => a.sortKey.localeCompare(b.sortKey));

  let currentBalance = 0;
  return movements.map((m, idx) => {
    currentBalance = currentBalance + m.inQty - m.outQty;
    const movementQty = m.inQty > 0 ? m.inQty : m.outQty;
    return {
      id: `MOV-${idx + 1}`,
      date: m.date,
      time: m.time,
      movementType: m.movementType,
      movementLabel: m.movementLabel,
      documentNo: m.documentNo,
      warehouseId: m.warehouseId,
      warehouseName: m.warehouseName,
      inQty: m.inQty,
      outQty: m.outQty,
      balanceAfter: currentBalance,
      unitPriceOrCost: m.unitPriceOrCost,
      totalMovementValue: movementQty * m.unitPriceOrCost,
      partyName: m.partyName,
      notes: m.notes,
    };
  });
}
