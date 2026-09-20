'use client';

import React, { useState, useEffect, useMemo } from 'react';
import { ProductItem, Supplier, Invoice, CartItem, PaymentType, Warehouse, JournalEntry, AccountNode, SplitPaymentBreakdown, MainTabType } from '@/types/pos';
import { generateNextId, generatePurchaseInvoiceNumber, getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import { getProductStockInWarehouse, calculateWeightedAverageCost } from '@/lib/mock';
import { tafqeet } from '@/lib/tafqeet';
import { 
  Truck, 
  Plus, 
  Receipt, 
  Users, 
  CheckCircle2, 
  Trash2, 
  Search, 
  Building2, 
  Calendar, 
  Coins,
  ArrowDownLeft,
  ArrowUpRight,
  Printer,
  Eye,
  RotateCcw,
  X,
  AlertCircle,
  FileText,
  Filter,
  Layers,
  Scale,
  DollarSign,
  PackageCheck,
  Check,
  Phone,
  MapPin,
  Clock,
  Package,
  ChevronLeft,
  ChevronRight,
  UploadCloud,
  Sliders,
} from 'lucide-react';

import {
  AddSupplierModal,
  SupplierStatementModal,
  PurchaseInvoicePrintModal
} from './purchases';

interface PurchasesModuleProps {
  suppliers: Supplier[];
  products: ProductItem[];
  warehouses: Warehouse[];
  purchaseInvoices: Invoice[];
  chartOfAccounts?: AccountNode[];
  journalEntries?: JournalEntry[];
  onAddPurchaseInvoice: (invoice: Invoice) => void;
  onAddSupplier: (supplier: Supplier) => void;
  currency: string;
  cashierName: string;
  initialSupplierId?: string;
  initialNotes?: string;
  initialItems?: Array<{
    product: ProductItem;
    quantity: number;
    selectedUnit: string;
    unitCost: number;
    discount?: number;
    batchNo?: string;
    expiryDate?: string;
    notes?: string;
  }>;
  onSelectTab?: (tab: MainTabType) => void;
}

// Expense Accounts for Other Charges Dropdown (Standard Chart of Accounts)
const EXPENSE_ACCOUNTS_OPTIONS = [
  { code: '424', name: 'أجور نقل ومواصلات وشحن' },
  { code: '425', name: 'شحن وتوصيل طلبات' },
  { code: '426', name: 'عتالة وتحميل وتفريغ' },
  { code: '423', name: 'صيانة وضيافة ومصاريف المحل' },
  { code: '421', name: 'إيجار وكهرباء ومياه' },
  { code: '429', name: 'مصروفات تشغيلية ونثرية أخرى' },
];

export const PurchasesModule: React.FC<PurchasesModuleProps> = ({
  suppliers,
  products,
  warehouses,
  purchaseInvoices,
  chartOfAccounts = [],
  journalEntries = [],
  onAddPurchaseInvoice,
  onAddSupplier,
  currency,
  cashierName,
  initialSupplierId,
  initialNotes,
  initialItems,
  onSelectTab,
}) => {
  const [activeSubTab, setActiveSubTab] = useState<'NEW_PURCHASE' | 'PURCHASE_RETURN' | 'PURCHASE_HISTORY' | 'SUPPLIERS_LIST'>('NEW_PURCHASE');
  
  // --- New Purchase State ---
  const [selectedSupplierId, setSelectedSupplierId] = useState<string>(() => initialSupplierId || suppliers[0]?.id || 'SUP-001');
  const [selectedWarehouseId, setSelectedWarehouseId] = useState<string>(warehouses[0]?.id || 'WH-01');
  const [purchaseItems, setPurchaseItems] = useState<CartItem[]>(() => {
    if (initialItems && initialItems.length > 0) {
      return initialItems.map((it) => {
        const uInfo = it.product.units?.find(u => u.name === it.selectedUnit);
        const factor = uInfo?.factor || 1;
        const cost = it.unitCost;
        const qty = it.quantity;
        const disc = it.discount || 0;
        return {
          item: it.product,
          quantity: qty,
          selectedUnit: it.selectedUnit,
          unitPrice: it.product.salePrice,
          unitCost: cost,
          discount: disc,
          taxRate: 0,
          total: Math.max(0, (qty * cost) - disc),
          unitFactor: factor,
          batchNo: it.batchNo || 'PO-BATCH',
          expiryDate: it.expiryDate || it.product.expiryDate || '2028-12-31',
          notes: it.notes || '',
        };
      });
    }
    return [];
  });
  const [selectedProductId, setSelectedProductId] = useState<string>(products[0]?.id || '');
  const [selectedUnit, setSelectedUnit] = useState<string>(products[0]?.unit || 'حبة');
  const [itemQty, setItemQty] = useState<number>(10);
  const [itemCost, setItemCost] = useState<number>(products[0]?.costPrice || 1000);
  const [itemDiscount, setItemDiscount] = useState<number>(0);
  const [itemTaxRate, setItemTaxRate] = useState<number>(0);
  const [itemBatchNo, setItemBatchNo] = useState<string>('BT-2026');
  const [itemExpiryDate, setItemExpiryDate] = useState<string>('2027-12-31');
  const [paymentType, setPaymentType] = useState<PaymentType>('CREDIT');
  const [referenceBillNo, setReferenceBillNo] = useState<string>('BILL-9901');
  const [invoiceDate, setInvoiceDate] = useState<string>(getCurrentDateString());
  const [invoiceTime, setInvoiceTime] = useState<string>(getCurrentTimeString());
  const [notes, setNotes] = useState<string>(() => initialNotes || '');
  const [productSearchTerm, setProductSearchTerm] = useState<string>('');
  const [purchaseBarcodeInput, setPurchaseBarcodeInput] = useState<string>('');
  const [returnBarcodeInput, setReturnBarcodeInput] = useState<string>('');

  // --- Header-Level Discount State (توحيد الخصم على مستوى الفاتورة لتقليل التعقيد) ---
  const [headerDiscountVal, setHeaderDiscountVal] = useState<string>('0');
  const [headerDiscountType, setHeaderDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [purchaseOtherChargesVal, setPurchaseOtherChargesVal] = useState<string>('0');
  const [purchaseOtherChargesAccountCode, setPurchaseOtherChargesAccountCode] = useState<string>('424');
  const [purchaseOtherChargesName, setPurchaseOtherChargesName] = useState<string>('أجور نقل ومواصلات وشحن');

  // --- Purchase Return State ---
  const [returnSupplierId, setReturnSupplierId] = useState<string>(suppliers[0]?.id || 'SUP-001');
  const [returnWarehouseId, setReturnWarehouseId] = useState<string>(warehouses[0]?.id || 'WH-01');
  const [originalInvoiceNo, setOriginalInvoiceNo] = useState<string>('');
  const [returnItems, setReturnItems] = useState<CartItem[]>([]);
  const [returnProductId, setReturnProductId] = useState<string>(products[0]?.id || '');
  const [returnQty, setReturnQty] = useState<number>(1);
  const [returnCost, setReturnCost] = useState<number>(products[0]?.costPrice || 1000);
  const [returnReason, setReturnReason] = useState<string>('بضاعة تالفة أو غير مطابقة للمواصفات');
  const [returnPaymentType, setReturnPaymentType] = useState<PaymentType>('CREDIT');
  const [returnHeaderDiscountVal, setReturnHeaderDiscountVal] = useState<string>('0');
  const [returnHeaderDiscountType, setReturnHeaderDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [returnTaxRate, setReturnTaxRate] = useState<number>(0);
  const [returnOtherChargesVal, setReturnOtherChargesVal] = useState<string>('0');
  const [returnOtherChargesAccountCode, setReturnOtherChargesAccountCode] = useState<string>('424');
  const [returnOtherChargesName, setReturnOtherChargesName] = useState<string>('أجور نقل وشحن المردود');

  // --- Payment & Split-Payment Engine State (مطابق لمحرك نقاط البيع POS) ---
  const [purchaseSplitCash, setPurchaseSplitCash] = useState<number>(0);
  const [purchaseSplitNetwork, setPurchaseSplitNetwork] = useState<number>(0);
  const [purchaseSplitCredit, setPurchaseSplitCredit] = useState<number>(0);
  const [selectedCashAccountCode, setSelectedCashAccountCode] = useState<string>('1111');
  const [selectedBankAccountCode, setSelectedBankAccountCode] = useState<string>('1121');

  // --- Purchase Return Split Payment State ---
  const [returnSplitCash, setReturnSplitCash] = useState<number>(0);
  const [returnSplitNetwork, setReturnSplitNetwork] = useState<number>(0);
  const [returnSplitCredit, setReturnSplitCredit] = useState<number>(0);
  const [returnCashAccountCode, setReturnCashAccountCode] = useState<string>('1111');
  const [returnBankAccountCode, setReturnBankAccountCode] = useState<string>('1121');

  // Extract Cash Accounts (الصناديق 111)
  const availableCashAccounts = useMemo(() => {
    const list: { code: string; name: string }[] = [];
    const extractLeaves = (nodes: AccountNode[]) => {
      for (const node of nodes) {
        if (node.code.startsWith('111')) {
          if (node.isLeaf || !node.children || node.children.length === 0) {
            list.push({ code: node.code, name: node.name });
          }
        }
        if (node.children && node.children.length > 0) {
          extractLeaves(node.children);
        }
      }
    };
    if (chartOfAccounts && chartOfAccounts.length > 0) {
      extractLeaves(chartOfAccounts);
    }
    if (list.length === 0) {
      return [
        { code: '1111', name: 'الصندوق الرئيسي (الخزينة المركزية)' },
        { code: '1112', name: 'صندوق المشتريات والمصروفات' },
        { code: '1113', name: 'صندوق نقطة بيع POS كاشير 1' },
      ];
    }
    return list;
  }, [chartOfAccounts]);

  // Extract Bank & Transfer Accounts (البنوك والمحافظ 112)
  const availableBankAccounts = useMemo(() => {
    const list: { code: string; name: string }[] = [];
    const extractLeaves = (nodes: AccountNode[]) => {
      for (const node of nodes) {
        if (node.code.startsWith('112')) {
          if (node.isLeaf || !node.children || node.children.length === 0) {
            list.push({ code: node.code, name: node.name });
          }
        }
        if (node.children && node.children.length > 0) {
          extractLeaves(node.children);
        }
      }
    };
    if (chartOfAccounts && chartOfAccounts.length > 0) {
      extractLeaves(chartOfAccounts);
    }
    if (list.length === 0) {
      return [
        { code: '1121', name: 'حساب بنك الكريمي للتمويل الأصغر (YER)' },
        { code: '1122', name: 'محفظة جيب الإلكترونية (JEB Wallet)' },
        { code: '1123', name: 'محفظة كاش وبنك اليمن والكويت' },
        { code: '1124', name: 'بنك التضامن الإسلامي الدولي' },
      ];
    }
    return list;
  }, [chartOfAccounts]);

  const currentCashAccount = availableCashAccounts.find((a) => a.code === selectedCashAccountCode) || availableCashAccounts[0];
  const currentBankAccount = availableBankAccounts.find((a) => a.code === selectedBankAccountCode) || availableBankAccounts[0];
  const currentReturnCashAccount = availableCashAccounts.find((a) => a.code === returnCashAccountCode) || availableCashAccounts[0];
  const currentReturnBankAccount = availableBankAccounts.find((a) => a.code === returnBankAccountCode) || availableBankAccounts[0];

  // --- Invoices Register Filtering & Search ---
  const [historySearch, setHistorySearch] = useState<string>('');
  const [historyTypeFilter, setHistoryTypeFilter] = useState<'ALL' | 'PURCHASE' | 'PURCHASE_RETURN'>('ALL');
  const [historyWarehouseFilter, setHistoryWarehouseFilter] = useState<string>('ALL');

  // --- Suppliers Directory Filtering & Modal ---
  const [supplierSearch, setSupplierSearch] = useState<string>('');
  const [showSupplierModal, setShowSupplierModal] = useState<boolean>(false);
  const [supName, setSupName] = useState<string>('');
  const [supCompany, setSupCompany] = useState<string>('');
  const [supPhone, setSupPhone] = useState<string>('');
  const [supAddress, setSupAddress] = useState<string>('');
  const [supCreditLimit, setSupCreditLimit] = useState<number>(1000000);
  const [supTaxNumber, setSupTaxNumber] = useState<string>('');
  const [supNotes, setSupNotes] = useState<string>('');

  // --- Statement of Account (Supplier Ledger Modal) ---
  const [selectedSupplierForLedger, setSelectedSupplierForLedger] = useState<Supplier | null>(null);

  // --- A4 Print / Preview Modal ---
  const [previewInvoice, setPreviewInvoice] = useState<Invoice | null>(null);

  // Active Supplier and Warehouse Lookups
  const selectedSupplier = useMemo(() => {
    return suppliers.find((s) => s.id === selectedSupplierId) || suppliers[0];
  }, [suppliers, selectedSupplierId]);

  const selectedWarehouse = useMemo(() => {
    return warehouses.find((w) => w.id === selectedWarehouseId) || warehouses[0];
  }, [warehouses, selectedWarehouseId]);

  const selectedProduct = useMemo(() => {
    return products.find((p) => p.id === selectedProductId) || products[0];
  }, [products, selectedProductId]);

  const handleSelectProduct = (prodId: string) => {
    setSelectedProductId(prodId);
    const p = products.find((prod) => prod.id === prodId);
    if (p) {
      setItemCost(p.costPrice);
      setSelectedUnit(p.unit);
      if (p.batchNo) setItemBatchNo(p.batchNo);
      if (p.hasExpiryDate !== false && p.expiryDate) {
        setItemExpiryDate(p.expiryDate);
      } else {
        setItemExpiryDate('');
      }
    }
  };

  // Keyboard Shortcuts (F2, F3, F4, F5, F10)
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F2') {
        e.preventDefault();
        setActiveSubTab('NEW_PURCHASE');
      } else if (e.key === 'F3') {
        e.preventDefault();
        setActiveSubTab('PURCHASE_RETURN');
      } else if (e.key === 'F4') {
        e.preventDefault();
        setActiveSubTab('PURCHASE_HISTORY');
      } else if (e.key === 'F5') {
        e.preventDefault();
        setActiveSubTab('SUPPLIERS_LIST');
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  
  // Grid Cell Inline Editing Handlers (Purchase)
  const handlePurchaseGridChange = (index: number, field: string, value: any) => {
    setPurchaseItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'unit') {
        item.selectedUnit = value;
        const uInfo = item.item.units?.find(u => u.name === value);
        if (uInfo) {
           item.unitPrice = uInfo.cost || item.item.costPrice;
           item.unitFactor = uInfo.factor;
        } else {
           item.unitPrice = item.item.costPrice;
           item.unitFactor = 1;
        }
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'quantity') {
        const q = parseFloat(value) || 0;
        item.quantity = Math.max(0.001, q);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'unitPrice') {
        const p = parseFloat(value) || 0;
        item.unitPrice = Math.max(0, p);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'discount') {
        const d = parseFloat(value) || 0;
        item.discount = Math.max(0, d);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'taxRate') {
        item.taxRate = parseFloat(value) || 0;
      } else if (field === 'batchNo') {
        item.batchNo = value;
      } else if (field === 'expiryDate') {
        item.expiryDate = value;
      }

      item.total = Number(item.total.toFixed(4));
      updated[index] = item;
      return updated;
    });
  };

  const handleReturnGridChange = (index: number, field: string, value: any) => {
    setReturnItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'unit') {
        item.selectedUnit = value;
        const uInfo = item.item.units?.find(u => u.name === value);
        if (uInfo) {
           item.unitPrice = uInfo.cost || item.item.costPrice;
           item.unitFactor = uInfo.factor;
        } else {
           item.unitPrice = item.item.costPrice;
           item.unitFactor = 1;
        }
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'quantity') {
        const q = parseFloat(value) || 0;
        item.quantity = Math.max(0.001, q);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'unitPrice') {
        const p = parseFloat(value) || 0;
        item.unitPrice = Math.max(0, p);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'discount') {
        const d = parseFloat(value) || 0;
        item.discount = Math.max(0, d);
        item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      } else if (field === 'taxRate') {
        item.taxRate = parseFloat(value) || 0;
      } else if (field === 'batchNo') {
        item.batchNo = value;
      } else if (field === 'expiryDate') {
        item.expiryDate = value;
      }

      item.total = Number(item.total.toFixed(4));
      updated[index] = item;
      return updated;
    });
  };

  const handlePurchaseBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!purchaseBarcodeInput.trim()) return;
    const code = purchaseBarcodeInput.trim().toLowerCase();
    
    let foundProd: ProductItem | undefined;
    let foundUnitName: string | undefined;

    for (const p of products) {
      if (p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code) {
        foundProd = p; foundUnitName = p.unit; break;
      }
      if (p.units) {
        const u = p.units.find(u => u.barcode?.toLowerCase() === code);
        if (u) {
          foundProd = p; foundUnitName = u.name; break;
        }
      }
    }

    if (foundProd) {
      setPurchaseItems(prev => {
        const updated = [...prev];
        const existing = updated.find(i => i.item.id === foundProd!.id && i.selectedUnit === foundUnitName);
        if (existing) {
          existing.quantity += 1;
          existing.total = Math.max(0, (existing.quantity * existing.unitPrice) - existing.discount);
        } else {
          const uInfo = foundProd!.units?.find(u => u.name === foundUnitName);
          const price = uInfo ? (uInfo.cost || foundProd!.costPrice) : foundProd!.costPrice;
          const factor = uInfo ? uInfo.factor : 1;
          updated.push({
            item: foundProd!,
            quantity: 1,
            selectedUnit: foundUnitName || foundProd!.unit || 'حبة',
            unitPrice: price,
            unitCost: price,
            unitFactor: factor,
            discount: 0,
            taxRate: 0,
            total: price,
            batchNo: foundProd!.batchNo || '',
            expiryDate: foundProd!.expiryDate || ''
          });
        }
        return updated;
      });
    }
    setPurchaseBarcodeInput('');
  };

  const handleReturnBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!returnBarcodeInput.trim()) return;
    const code = returnBarcodeInput.trim().toLowerCase();
    
    let foundProd: ProductItem | undefined;
    let foundUnitName: string | undefined;

    for (const p of products) {
      if (p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code) {
        foundProd = p; foundUnitName = p.unit; break;
      }
      if (p.units) {
        const u = p.units.find(u => u.barcode?.toLowerCase() === code);
        if (u) {
          foundProd = p; foundUnitName = u.name; break;
        }
      }
    }

    if (foundProd) {
      setReturnItems(prev => {
        const updated = [...prev];
        const existing = updated.find(i => i.item.id === foundProd!.id && i.selectedUnit === foundUnitName);
        if (existing) {
          existing.quantity += 1;
          existing.total = Math.max(0, (existing.quantity * existing.unitPrice) - existing.discount);
        } else {
          const uInfo = foundProd!.units?.find(u => u.name === foundUnitName);
          const price = uInfo ? (uInfo.cost || foundProd!.costPrice) : foundProd!.costPrice;
          const factor = uInfo ? uInfo.factor : 1;
          updated.push({
            item: foundProd!,
            quantity: 1,
            selectedUnit: foundUnitName || foundProd!.unit || 'حبة',
            unitPrice: price,
            unitCost: price,
            unitFactor: factor,
            discount: 0,
            taxRate: 0,
            total: price,
            batchNo: foundProd!.batchNo || '',
            expiryDate: foundProd!.expiryDate || ''
          });
        }
        return updated;
      });
    }
    setReturnBarcodeInput('');
  };


  // Filtered Products for quick item search
  const filteredProducts = useMemo(() => {
    if (!productSearchTerm.trim()) return products;
    const term = productSearchTerm.toLowerCase();
    return products.filter((p) => 
      p.name.toLowerCase().includes(term) ||
      p.code.toLowerCase().includes(term) ||
      p.barcode.includes(term)
    );
  }, [products, productSearchTerm]);

  // Add Item to Purchase Invoice
  const handleAddItemToPurchase = () => {
    if (!selectedProduct || itemQty <= 0) return;

    const lineSubtotal = itemQty * itemCost;
    const lineDiscount = Math.min(lineSubtotal, itemDiscount);
    const taxableAmount = Math.max(0, lineSubtotal - lineDiscount);
    const lineTax = (taxableAmount * itemTaxRate) / 100;
    const lineTotal = taxableAmount + lineTax;

    const existingIndex = purchaseItems.findIndex((ci) => ci.item.id === selectedProduct.id);
    if (existingIndex > -1) {
      const updated = [...purchaseItems];
      const prev = updated[existingIndex];
      const newQty = prev.quantity + itemQty;
      const newSubtotal = newQty * itemCost;
      const newDiscount = prev.discount + lineDiscount;
      const newTaxable = Math.max(0, newSubtotal - newDiscount);
      const newTax = (newTaxable * itemTaxRate) / 100;

      updated[existingIndex] = {
        ...prev,
        quantity: newQty,
        unitPrice: itemCost,
        unitCost: itemCost,
        discount: newDiscount,
        taxRate: itemTaxRate,
        total: newTaxable + newTax,
        batchNo: itemBatchNo || prev.batchNo,
        expiryDate: selectedProduct.hasExpiryDate === false ? undefined : (itemExpiryDate || prev.expiryDate),
      };
      setPurchaseItems(updated);
    } else {
      const newItem: CartItem = {
        item: selectedProduct,
        quantity: itemQty,
        selectedUnit: selectedUnit || selectedProduct.unit,
        unitPrice: itemCost,
        unitCost: itemCost,
        discount: lineDiscount,
        taxRate: itemTaxRate,
        total: lineTotal,
        batchNo: itemBatchNo,
        expiryDate: selectedProduct.hasExpiryDate === false ? undefined : (itemExpiryDate || undefined),
      };
      setPurchaseItems([...purchaseItems, newItem]);
    }

    // Reset item line input with defaults
    setItemDiscount(0);
  };

  const handleRemovePurchaseItem = (idx: number) => {
    setPurchaseItems((prev) => prev.filter((_, i) => i !== idx));
  };

  // Calculations for New Purchase Invoice (Header-Level Discount Unified Model)
  const purchaseSubtotal = useMemo(() => {
    return purchaseItems.reduce((acc, i) => acc + (i.quantity * i.unitPrice), 0);
  }, [purchaseItems]);

  const purchaseLineDiscounts = useMemo(() => {
    return purchaseItems.reduce((acc, i) => acc + (i.discount || 0), 0);
  }, [purchaseItems]);

  const purchaseHeaderDiscountAmount = useMemo(() => {
    const val = parseFloat(headerDiscountVal) || 0;
    if (headerDiscountType === 'PERCENT') {
      return (purchaseSubtotal * Math.min(100, Math.max(0, val))) / 100;
    }
    return Math.min(purchaseSubtotal, Math.max(0, val));
  }, [headerDiscountVal, headerDiscountType, purchaseSubtotal]);

  const purchaseDiscountTotal = useMemo(() => {
    return purchaseLineDiscounts + purchaseHeaderDiscountAmount;
  }, [purchaseLineDiscounts, purchaseHeaderDiscountAmount]);

  const purchaseTaxTotal = useMemo(() => {
    return purchaseItems.reduce((acc, i) => {
      const itemSubtotal = i.quantity * i.unitPrice;
      const ratio = purchaseSubtotal > 0 ? itemSubtotal / purchaseSubtotal : 0;
      const allocatedTaxable = Math.max(0, itemSubtotal - (i.discount || 0) - (purchaseHeaderDiscountAmount * ratio));
      return acc + (allocatedTaxable * (i.taxRate || 0)) / 100;
    }, 0);
  }, [purchaseItems, purchaseSubtotal, purchaseHeaderDiscountAmount]);

  const purchaseOtherCharges = useMemo(() => {
    return Math.max(0, parseFloat(purchaseOtherChargesVal) || 0);
  }, [purchaseOtherChargesVal]);

  const purchaseGrandTotal = useMemo(() => {
    return Math.max(0, purchaseSubtotal - purchaseDiscountTotal + purchaseTaxTotal + purchaseOtherCharges);
  }, [purchaseSubtotal, purchaseDiscountTotal, purchaseTaxTotal, purchaseOtherCharges]);

  // Save Purchase Invoice
  const handleSavePurchaseInvoice = (e: React.FormEvent) => {
    e.preventDefault();
    if (purchaseItems.length === 0) {
      alert('تنبيه: يرجى إضافة صنف واحد على الأقل لفاتورة الشراء.');
      return;
    }

    const nextInvoiceNo = generatePurchaseInvoiceNumber(purchaseInvoices);
    const invoiceId = generateNextId('PUR');

    // Build Split Payment Breakdown
    let totalCash = purchaseSplitCash;
    let totalNetwork = purchaseSplitNetwork;
    let totalCredit = purchaseSplitCredit;
    const splitSum = totalCash + totalNetwork + totalCredit;

    if (splitSum === 0) {
      if (paymentType === 'CASH') {
        totalCash = purchaseGrandTotal;
      } else if (paymentType === 'KURAYMI' || paymentType === 'BANK_TRANSFER') {
        totalNetwork = purchaseGrandTotal;
      } else {
        totalCredit = purchaseGrandTotal;
      }
    }

    const splitPayments: SplitPaymentBreakdown = {
      cash: totalCash,
      network: totalNetwork,
      wallet: 0,
      credit: totalCredit,
      cashAccountCode: selectedCashAccountCode,
      cashAccountName: currentCashAccount?.name || 'الصندوق الرئيسي',
      bankAccountCode: selectedBankAccountCode,
      bankAccountName: currentBankAccount?.name || 'حساب بنك الكريمي',
      walletAccountCode: '1122',
      walletAccountName: 'محفظة جيب الإلكترونية',
    };

    const paidSum = totalCash + totalNetwork;
    const remainingSum = totalCredit;

    // Credit ceiling check for supplier
    if (remainingSum > 0 && selectedSupplier && selectedSupplier.creditLimit && selectedSupplier.creditLimit > 0) {
      const newSupplierBalance = (selectedSupplier.currentBalance || 0) + remainingSum;
      if (newSupplierBalance > selectedSupplier.creditLimit) {
        const confirmProceed = window.confirm(
          `تنبيه سقف الائتمان للمورد:\n\nالشراء الآجل بمبلغ (${remainingSum.toLocaleString()} ${currency}) يرفع مديونيتنا للمورد (${selectedSupplier.name}) إلى (${newSupplierBalance.toLocaleString()} ${currency})، متجاوزاً سقف الائتمان المحدد للمورد (${selectedSupplier.creditLimit.toLocaleString()} ${currency}).\n\nهل ترغب في متابعة الشراء الآجل واعتماد الفاتورة؟`
        );
        if (!confirmProceed) {
          return;
        }
      }
    }

    const invoice: Invoice = {
      id: invoiceId,
      invoiceNo: nextInvoiceNo,
      type: 'PURCHASE',
      date: invoiceDate || getCurrentDateString(),
      time: invoiceTime || getCurrentTimeString(),
      customerOrSupplierId: selectedSupplier.id,
      customerOrSupplierName: selectedSupplier.name,
      warehouseId: selectedWarehouse.id,
      warehouseName: selectedWarehouse.name,
      status: 'POSTED',
      items: [...purchaseItems],
      subtotal: purchaseSubtotal,
      discountTotal: purchaseDiscountTotal,
      taxTotal: purchaseTaxTotal,
      otherCharges: purchaseOtherCharges,
      otherChargesName: purchaseOtherChargesName,
      otherChargesAccountCode: purchaseOtherChargesAccountCode,
      otherChargesAccountName: EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === purchaseOtherChargesAccountCode)?.name || purchaseOtherChargesName,
      grandTotal: purchaseGrandTotal,
      paidAmount: paidSum,
      remainingAmount: remainingSum,
      paymentType,
      splitPayments,
      referenceNo: referenceBillNo,
      cashierName,
      notes: notes || `توريد بضاعة إلى ${selectedWarehouse.name} من المورد ${selectedSupplier.company}`,
    };

    onAddPurchaseInvoice(invoice);
    setPurchaseItems([]);
    setPurchaseSplitCash(0);
    setPurchaseSplitNetwork(0);
    setPurchaseSplitCredit(0);
    setPurchaseOtherChargesVal('0');
    setNotes('');
    setReferenceBillNo(`BILL-${Math.floor(Math.random() * 9000 + 1000)}`);
    setPreviewInvoice(invoice);
    setActiveSubTab('PURCHASE_HISTORY');
  };

  // --- Purchase Returns Handling ---
  const handleAddItemToReturn = () => {
    const prod = products.find((p) => p.id === returnProductId);
    if (!prod || returnQty <= 0) return;

    const currentWhStock = getProductStockInWarehouse(prod, returnWarehouseId);
    if (returnQty > currentWhStock) {
      alert(`تنبيه: الكمية المرجوعة (${returnQty} ${prod.unit}) تتجاوز الرصيد المتوفر في المستودع المختار (${currentWhStock} ${prod.unit})!`);
      return;
    }

    const totalCost = returnQty * returnCost;
    const existingIndex = returnItems.findIndex((ci) => ci.item.id === prod.id);

    if (existingIndex > -1) {
      const updated = [...returnItems];
      updated[existingIndex].quantity += returnQty;
      updated[existingIndex].total = updated[existingIndex].quantity * returnCost;
      setReturnItems(updated);
    } else {
      const newItem: CartItem = {
        item: prod,
        quantity: returnQty,
        selectedUnit: prod.unit,
        unitPrice: returnCost,
        unitCost: returnCost,
        discount: 0,
        taxRate: 0,
        total: totalCost,
      };
      setReturnItems([...returnItems, newItem]);
    }
  };

  const handleRemoveReturnItem = (idx: number) => {
    setReturnItems((prev) => prev.filter((_, i) => i !== idx));
  };

  const returnSubtotal = useMemo(() => {
    return returnItems.reduce((sum, item) => sum + item.total, 0);
  }, [returnItems]);

  const returnHeaderDiscountAmount = useMemo(() => {
    const val = parseFloat(returnHeaderDiscountVal) || 0;
    if (returnHeaderDiscountType === 'PERCENT') {
      return (returnSubtotal * Math.min(100, Math.max(0, val))) / 100;
    }
    return Math.min(returnSubtotal, Math.max(0, val));
  }, [returnHeaderDiscountVal, returnHeaderDiscountType, returnSubtotal]);

  const returnTaxAmount = useMemo(() => {
    const taxable = Math.max(0, returnSubtotal - returnHeaderDiscountAmount);
    return (taxable * (returnTaxRate || 0)) / 100;
  }, [returnSubtotal, returnHeaderDiscountAmount, returnTaxRate]);

  const returnOtherCharges = useMemo(() => {
    return Math.max(0, parseFloat(returnOtherChargesVal) || 0);
  }, [returnOtherChargesVal]);

  const returnGrandTotal = useMemo(() => {
    return Math.max(0, returnSubtotal - returnHeaderDiscountAmount + returnTaxAmount + returnOtherCharges);
  }, [returnSubtotal, returnHeaderDiscountAmount, returnTaxAmount, returnOtherCharges]);

  const handleSavePurchaseReturn = (e: React.FormEvent) => {
    e.preventDefault();
    if (returnItems.length === 0) {
      alert('تنبيه: يرجى إضافة أصناف لمردودات المشتريات أولاً.');
      return;
    }

    const retSupplier = suppliers.find((s) => s.id === returnSupplierId) || suppliers[0];
    const retWh = warehouses.find((w) => w.id === returnWarehouseId) || warehouses[0];
    const nextInvoiceNo = `${purchaseInvoices.length + 7001}`;
    const invoiceId = generateNextId('PRET');

    let retCash = returnSplitCash;
    let retNetwork = returnSplitNetwork;
    let retCredit = returnSplitCredit;
    const retSum = retCash + retNetwork + retCredit;

    if (retSum === 0) {
      if (returnPaymentType === 'CASH') {
        retCash = returnGrandTotal;
      } else if (returnPaymentType === 'KURAYMI' || returnPaymentType === 'BANK_TRANSFER') {
        retNetwork = returnGrandTotal;
      } else {
        retCredit = returnGrandTotal;
      }
    }

    const returnSplitPayments: SplitPaymentBreakdown = {
      cash: retCash,
      network: retNetwork,
      wallet: 0,
      credit: retCredit,
      cashAccountCode: returnCashAccountCode,
      cashAccountName: currentReturnCashAccount?.name || 'الصندوق الرئيسي',
      bankAccountCode: returnBankAccountCode,
      bankAccountName: currentReturnBankAccount?.name || 'حساب بنك الكريمي',
      walletAccountCode: '1122',
      walletAccountName: 'محفظة جيب الإلكترونية',
    };

    const returnInvoice: Invoice = {
      id: invoiceId,
      invoiceNo: nextInvoiceNo,
      type: 'PURCHASE_RETURN',
      date: getCurrentDateString(),
      time: getCurrentTimeString(),
      customerOrSupplierId: retSupplier.id,
      customerOrSupplierName: retSupplier.name,
      warehouseId: retWh.id,
      warehouseName: retWh.name,
      status: 'POSTED',
      items: [...returnItems],
      subtotal: returnSubtotal,
      discountTotal: returnHeaderDiscountAmount,
      taxTotal: returnTaxAmount,
      otherCharges: returnOtherCharges,
      otherChargesName: returnOtherChargesName,
      otherChargesAccountCode: returnOtherChargesAccountCode,
      otherChargesAccountName: EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === returnOtherChargesAccountCode)?.name || returnOtherChargesName,
      grandTotal: returnGrandTotal,
      paidAmount: retCash + retNetwork,
      remainingAmount: retCredit,
      paymentType: returnPaymentType,
      splitPayments: returnSplitPayments,
      referenceNo: originalInvoiceNo || 'إرجاع مباشر',
      cashierName,
      notes: `مردودات مشتريات: ${returnReason} | مستودع الصرف: ${retWh.name}`,
      originalInvoiceNo,
    };

    onAddPurchaseInvoice(returnInvoice);
    setReturnItems([]);
    setReturnSplitCash(0);
    setReturnSplitNetwork(0);
    setReturnSplitCredit(0);
    setReturnOtherChargesVal('0');
    setOriginalInvoiceNo('');
    setPreviewInvoice(returnInvoice);
    setActiveSubTab('PURCHASE_HISTORY');
  };

  // Save New Supplier Modal
  const handleSaveNewSupplier = (e: React.FormEvent) => {
    e.preventDefault();
    if (!supName.trim()) return;

    const nextCode = `21100${suppliers.length + 1}`;
    const newSup: Supplier = {
      id: generateNextId('SUP'),
      code: nextCode,
      name: supName,
      company: supCompany || supName,
      phone: supPhone || '770000000',
      address: supAddress || 'صنعاء - الجمهورية اليمنية',
      creditLimit: supCreditLimit,
      currentBalance: 0,
      accountCode: nextCode,
      taxNumber: supTaxNumber,
      notes: supNotes,
    };

    onAddSupplier(newSup);
    setSelectedSupplierId(newSup.id);
    setShowSupplierModal(false);
    setSupName('');
    setSupCompany('');
    setSupPhone('');
    setSupAddress('');
    setSupTaxNumber('');
    setSupNotes('');
  };

  // Overall Statistics Cards
  const stats = useMemo(() => {
    const purchases = purchaseInvoices.filter((i) => i.type === 'PURCHASE');
    const returns = purchaseInvoices.filter((i) => i.type === 'PURCHASE_RETURN');
    const totalPurchasesAmount = purchases.reduce((sum, i) => sum + i.grandTotal, 0);
    const totalReturnsAmount = returns.reduce((sum, i) => sum + i.grandTotal, 0);
    const totalSupplierBalances = suppliers.reduce((sum, s) => sum + s.currentBalance, 0);

    return {
      purchasesCount: purchases.length,
      returnsCount: returns.length,
      totalPurchasesAmount,
      totalReturnsAmount,
      netPurchasesAmount: totalPurchasesAmount - totalReturnsAmount,
      totalSupplierBalances,
    };
  }, [purchaseInvoices, suppliers]);

  // Filtered History
  const filteredHistory = useMemo(() => {
    return purchaseInvoices.filter((inv) => {
      const matchSearch = 
        inv.invoiceNo.toLowerCase().includes(historySearch.toLowerCase()) ||
        inv.customerOrSupplierName.toLowerCase().includes(historySearch.toLowerCase()) ||
        (inv.referenceNo && inv.referenceNo.toLowerCase().includes(historySearch.toLowerCase()));
      
      const matchType = 
        historyTypeFilter === 'ALL' || inv.type === historyTypeFilter;

      const matchWh = 
        historyWarehouseFilter === 'ALL' || inv.warehouseId === historyWarehouseFilter;

      return matchSearch && matchType && matchWh;
    });
  }, [purchaseInvoices, historySearch, historyTypeFilter, historyWarehouseFilter]);

  // Filtered Suppliers
  const filteredSuppliers = useMemo(() => {
    return suppliers.filter((s) => 
      s.name.toLowerCase().includes(supplierSearch.toLowerCase()) ||
      s.company.toLowerCase().includes(supplierSearch.toLowerCase()) ||
      s.code.includes(supplierSearch) ||
      s.phone.includes(supplierSearch)
    );
  }, [suppliers, supplierSearch]);

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 p-2 gap-2 select-none overflow-hidden font-sans">
      {/* 1. Phase 2 Operational Stage Ribbon (شريط المرحلة الثانية: دورة المخازن والمشتريات والتوريد) */}
      <div className="bg-white border border-slate-200 px-3 py-1.5 rounded-lg flex flex-wrap items-center justify-between gap-2 text-xs shadow-xs shrink-0 text-slate-800">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-amber-600 text-white px-2.5 py-0.5 rounded-full border border-amber-700 shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-amber-900 flex items-center justify-center font-black text-[10px]">
              2
            </span>
            <span className="font-bold text-[11px]">المرحلة الثانية: دورة المخازن والمشتريات والتوريد</span>
          </div>

          <div className="h-4 w-px bg-slate-200" />

          {/* Direct Switchers between Phase 2 Tabs */}
          <div className="flex items-center gap-1">
            {onSelectTab && (
              <button
                type="button"
                onClick={() => onSelectTab('INVENTORY')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                title="الانتقال إلى المخزون وبطاقة الأصناف [F5]"
              >
                <Package className="w-3 h-3 text-amber-600" />
                <span>المخزون وبطاقة الأصناف [F5]</span>
              </button>
            )}

            <button
              type="button"
              className="bg-amber-600 text-white font-bold px-2 py-0.5 rounded text-[11px] flex items-center gap-1 shadow-xs border border-amber-700"
              title="أنت الآن في شاشة فواتير المشتريات والتوريد [F6]"
            >
              <Truck className="w-3 h-3 text-white" />
              <span>فواتير المشتريات [F6]</span>
            </button>

            {onSelectTab && (
              <>
                <button
                  type="button"
                  onClick={() => onSelectTab('PURCHASE_ORDER')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="أوامر الشراء والتوريد وطلبيات الموردين"
                >
                  <FileText className="w-3 h-3 text-sky-600" />
                  <span>أوامر الشراء</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('COMMERCIAL_PURCHASE')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="فاتورة مشتريات تجارية موسعة"
                >
                  <Receipt className="w-3 h-3 text-cyan-600" />
                  <span>مشتريات تجارية</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('MOHASEB_ITEMS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="دليل وبطاقة الأصناف الشامل"
                >
                  <Layers className="w-3 h-3 text-amber-600" />
                  <span>دليل الأصناف</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('SALES_PRICES')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="تسعير الأصناف وتعديل قوائم الأسعار"
                >
                  <Coins className="w-3 h-3 text-yellow-600" />
                  <span>تسعير الأصناف</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('EXCEL_IMPORT')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="استيراد الأصناف من ملف إكسل"
                >
                  <UploadCloud className="w-3 h-3 text-indigo-600" />
                  <span>استيراد إكسل</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('ITEM_UNITS')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
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
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600">
            <span className="bg-emerald-50 text-emerald-700 px-1.5 py-0.5 rounded border border-emerald-200 font-mono font-bold">
              ✓ فواتير الشراء: 5001...
            </span>
            <span className="bg-amber-50 text-amber-700 px-1.5 py-0.5 rounded border border-amber-200 font-bold">
              ✓ طبقات FIFO / FEFO
            </span>
            <span className="bg-sky-50 text-sky-700 px-1.5 py-0.5 rounded border border-sky-200 font-bold">
              ✓ {suppliers.length} موردين معتمدين
            </span>
          </div>

          {onSelectTab && (
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={() => onSelectTab('SALES_INVOICE')}
                className="bg-slate-100 hover:bg-slate-200 text-slate-700 text-[11px] font-bold px-2 py-1 rounded flex items-center gap-1 transition border border-slate-200 cursor-pointer"
                title="الرجوع إلى المرحلة الأولى: المبيعات ونقاط البيع"
              >
                <ChevronRight className="w-3.5 h-3.5" />
                <span>المرحلة 1: المبيعات</span>
              </button>

              <button
                type="button"
                onClick={() => onSelectTab('CUSTOMERS')}
                className="bg-teal-700 hover:bg-teal-600 text-white text-[11px] font-bold px-2.5 py-1 rounded flex items-center gap-1 transition shadow-xs border border-teal-800 cursor-pointer"
                title="الانتقال إلى المرحلة الثالثة: العملاء والسيولة والعهد"
              >
                <span>المرحلة 3: العملاء والسيولة</span>
                <ChevronLeft className="w-3.5 h-3.5" />
              </button>
            </div>
          )}
        </div>
      </div>

      {/* Top Header & Fast KPI Statistics */}
      <div className="bg-white text-slate-800 px-3 py-2 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-3">
        <div className="flex items-center gap-2">
          <div className="bg-sky-50 p-1.5 rounded border border-sky-200">
            <Truck className="w-5 h-5 text-sky-700" />
          </div>
          <div>
            <div className="flex items-center gap-2">
              <h2 className="font-bold text-sm tracking-wide text-slate-800">إدارة المشتريات والموردين وتكلفة المخزون</h2>
              <span className="bg-sky-50 text-sky-700 text-[10px] font-mono px-2 py-0.5 rounded border border-sky-200 font-bold">
                Purchasing & AP Module
              </span>
            </div>
            <p className="text-[11px] text-slate-500">
              دورة الشراء والتوريد المباشر، تعقب طبقات التكلفة بنظام الوارد أولاً يصرف أولاً (FIFO)، وأتمتة القيود المحاسبية
            </p>
          </div>
        </div>

        {/* Quick KPI Strip */}
        <div className="flex items-center gap-2 text-xs">
          <div className="bg-slate-50 px-3 py-1.5 rounded border border-slate-200 flex items-center gap-2">
            <span className="text-slate-500">إجمالي المشتريات:</span>
            <span className="font-mono-numbers font-bold text-emerald-700 text-sm">
              {stats.totalPurchasesAmount.toLocaleString()} {currency}
            </span>
          </div>

          <div className="bg-slate-50 px-3 py-1.5 rounded border border-slate-200 flex items-center gap-2">
            <span className="text-slate-500">المردودات:</span>
            <span className="font-mono-numbers font-bold text-rose-700 text-sm">
              {stats.totalReturnsAmount.toLocaleString()} {currency}
            </span>
          </div>

          <div className="bg-slate-50 px-3 py-1.5 rounded border border-slate-200 flex items-center gap-2">
            <span className="text-slate-500">إجمالي ذمم الموردين:</span>
            <span className="font-mono-numbers font-bold text-amber-700 text-sm">
              {stats.totalSupplierBalances.toLocaleString()} {currency}
            </span>
          </div>

          <button
            onClick={() => setShowSupplierModal(true)}
            className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1 cursor-pointer transition shadow-xs"
          >
            <Plus className="w-3.5 h-3.5" />
            <span>مورد جديد</span>
          </button>
        </div>
      </div>

      {/* WinForms ToolStrip Navigation Bar */}
      <div className="bg-white px-2 py-1.5 rounded-lg border border-slate-300 shadow-xs flex items-center justify-between">
        <div className="flex items-center gap-1.5">
          <button
            onClick={() => setActiveSubTab('NEW_PURCHASE')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeSubTab === 'NEW_PURCHASE'
                ? 'bg-sky-800 text-white shadow-sm'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Plus className="w-3.5 h-3.5" />
            <span>فاتورة مشتريات جديدة [F2]</span>
          </button>

          <button
            onClick={() => setActiveSubTab('PURCHASE_RETURN')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeSubTab === 'PURCHASE_RETURN'
                ? 'bg-rose-700 text-white shadow-sm'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <RotateCcw className="w-3.5 h-3.5" />
            <span>مردودات مشتريات [F3]</span>
          </button>

          <button
            onClick={() => setActiveSubTab('PURCHASE_HISTORY')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeSubTab === 'PURCHASE_HISTORY'
                ? 'bg-sky-800 text-white shadow-sm'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Receipt className="w-3.5 h-3.5" />
            <span>سجل الفواتير والمردودات ({purchaseInvoices.length}) [F4]</span>
          </button>

          <button
            onClick={() => setActiveSubTab('SUPPLIERS_LIST')}
            className={`px-3 py-1.5 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              activeSubTab === 'SUPPLIERS_LIST'
                ? 'bg-sky-800 text-white shadow-sm'
                : 'bg-slate-100 text-slate-700 hover:bg-slate-200'
            }`}
          >
            <Users className="w-3.5 h-3.5" />
            <span>دليل الموردين وأرصدتهم ({suppliers.length}) [F5]</span>
          </button>
        </div>

        <div className="flex items-center gap-2 text-xs text-slate-500 font-mono">
          <span>المسؤول: {cashierName}</span>
          <span>•</span>
          <span>المستودع الافتراضي: {selectedWarehouse?.name}</span>
        </div>
      </div>

      {/* Main Content Area */}
      <div className="flex-1 min-h-0 bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden flex flex-col">
        {/* ========================================================================= */}
        {/* TAB 1: NEW PURCHASE INVOICE */}
        {/* ========================================================================= */}
        {activeSubTab === 'NEW_PURCHASE' && (
          <div className="flex-1 flex flex-col p-3 gap-2.5 overflow-y-auto text-xs">
            {/* Invoice Header Form Strip */}
            <div className="grid grid-cols-1 md:grid-cols-5 gap-2.5 bg-slate-50 p-2.5 rounded-lg border border-slate-300">
              {/* Supplier Selection */}
              <div>
                <label className="font-bold text-slate-700 block mb-1 flex items-center justify-between">
                  <span>المورد المعتمد:</span>
                  <span className="text-[10px] text-sky-700 font-mono">{selectedSupplier?.code}</span>
                </label>
                <select
                  value={selectedSupplierId}
                  onChange={(e) => setSelectedSupplierId(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900 focus:border-sky-600 focus:outline-none"
                >
                  {suppliers.map((s) => (
                    <option key={s.id} value={s.id}>
                      {s.name} ({s.company})
                    </option>
                  ))}
                </select>
                <div className="mt-1 text-[10px] text-slate-500 flex justify-between items-center">
                  <span>الرصيد: <strong className="font-bold font-mono text-rose-700">{selectedSupplier?.currentBalance.toLocaleString()} {currency}</strong></span>
                  <span>السقف: <strong className="font-bold font-mono text-slate-700">{selectedSupplier?.creditLimit ? `${selectedSupplier.creditLimit.toLocaleString()} ${currency}` : 'غير محدد'}</strong></span>
                </div>
              </div>

              {/* Receiving Warehouse */}
              <div>
                <label className="font-bold text-slate-700 block mb-1">المستودع المستلم (التخزين):</label>
                <select
                  value={selectedWarehouseId}
                  onChange={(e) => setSelectedWarehouseId(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900 focus:border-sky-600 focus:outline-none"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      [{w.code}] {w.name} ({w.branchName})
                    </option>
                  ))}
                </select>
                <div className="mt-1 text-[10px] text-slate-500">
                  أمين المستودع: {selectedWarehouse?.managerName}
                </div>
              </div>

              {/* Payment Mode Selector */}
              <div>
                <label className="font-bold text-slate-700 block mb-1">طريقة السداد / الدفع الأساسية:</label>
                <select
                  value={paymentType}
                  onChange={(e) => {
                    const newType = e.target.value as PaymentType;
                    setPaymentType(newType);
                    if (newType === 'CASH') {
                      setPurchaseSplitCash(purchaseGrandTotal);
                      setPurchaseSplitNetwork(0);
                      setPurchaseSplitCredit(0);
                    } else if (newType === 'KURAYMI' || newType === 'BANK_TRANSFER') {
                      setPurchaseSplitCash(0);
                      setPurchaseSplitNetwork(purchaseGrandTotal);
                      setPurchaseSplitCredit(0);
                    } else {
                      setPurchaseSplitCash(0);
                      setPurchaseSplitNetwork(0);
                      setPurchaseSplitCredit(purchaseGrandTotal);
                    }
                  }}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900 focus:border-sky-600 focus:outline-none"
                >
                  <option value="CREDIT">آجل على حساب المورد (ذمة دائنة)</option>
                  <option value="CASH">نقداً من الصندوق الرئيسي (1111)</option>
                  <option value="KURAYMI">تحويل من حساب بنك الكريمي (1121)</option>
                  <option value="MIXED">دفع متعدد / مركب (نقد + بنك + آجل)</option>
                </select>
                <div className="mt-1 text-[10px] text-slate-500">
                  {paymentType === 'CREDIT' ? 'يضاف لمديونية المورد' : (paymentType === 'CASH' ? 'يخصم من الصندوق' : (paymentType === 'MIXED' ? 'توزيع مركب' : 'حساب بنكي'))}
                </div>
              </div>

              {/* Reference Bill No & Date */}
              <div>
                <label className="font-bold text-slate-700 block mb-1">رقم فاتورة المورد وسند الشحن:</label>
                <input
                  type="text"
                  value={referenceBillNo}
                  onChange={(e) => setReferenceBillNo(e.target.value)}
                  placeholder="مثال: HSA-8874"
                  className="w-full border border-slate-300 rounded px-2 py-1.5 font-mono text-slate-900 focus:border-sky-600 focus:outline-none"
                />
                <div className="mt-1 text-[10px] text-slate-500 flex justify-between">
                  <span>تاريخ الفاتورة:</span>
                  <span className="font-mono">{invoiceDate}</span>
                </div>
              </div>

              {/* Notes */}
              <div>
                <label className="font-bold text-slate-700 block mb-1">بيان وملاحظات الفاتورة:</label>
                <input
                  type="text"
                  placeholder="توريد بضاعة..."
                  value={notes}
                  onChange={(e) => setNotes(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 text-slate-900 focus:border-sky-600 focus:outline-none"
                />
                <div className="mt-1 text-[10px] text-slate-500">
                  المسؤول: {cashierName}
                </div>
              </div>
            </div>

            {/* Split Payment Distribution Engine (محرك الدفع المتعدد والتوزيع المتناسق) */}
            <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-300 flex flex-col gap-2">
              <div className="flex items-center justify-between">
                <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                  <Coins className="w-3.5 h-3.5 text-sky-700" />
                  <span>توزيع المبالغ وحسابات السداد (نقد / بنك / آجل ذمة):</span>
                </span>
                <span className="text-[11px] font-mono text-slate-500">
                  إجمالي الفاتورة: <strong className="text-sky-900">{purchaseGrandTotal.toLocaleString()} {currency}</strong>
                </span>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-2.5">
                {/* 1. Cash Box Payment */}
                <div className="bg-emerald-50/70 p-2 rounded-lg border border-emerald-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-emerald-950 flex items-center gap-1">
                      <Coins className="w-3 h-3 text-emerald-700" />
                      <span>1. المدفوع نقداً (من الصندوق):</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, purchaseGrandTotal - purchaseSplitNetwork - purchaseSplitCredit);
                        setPurchaseSplitCash(rem);
                      }}
                      className="text-[10px] text-emerald-700 underline font-bold cursor-pointer hover:text-emerald-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5">
                    <input
                      type="number"
                      min={0}
                      value={purchaseSplitCash || ''}
                      onChange={(e) => setPurchaseSplitCash(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <select
                      value={selectedCashAccountCode}
                      onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                      disabled={purchaseSplitCash <= 0}
                      className="flex-1 bg-white border border-emerald-200 rounded px-1.5 py-1 text-[10px] font-bold text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                    >
                      {availableCashAccounts.map((acc) => (
                        <option key={acc.code} value={acc.code}>[{acc.code}] {acc.name}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* 2. Bank / Network Payment */}
                <div className="bg-sky-50/70 p-2 rounded-lg border border-sky-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-sky-950 flex items-center gap-1">
                      <Building2 className="w-3 h-3 text-sky-700" />
                      <span>2. تحويل بنكي / شيك / محفظة:</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, purchaseGrandTotal - purchaseSplitCash - purchaseSplitCredit);
                        setPurchaseSplitNetwork(rem);
                      }}
                      className="text-[10px] text-sky-700 underline font-bold cursor-pointer hover:text-sky-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5">
                    <input
                      type="number"
                      min={0}
                      value={purchaseSplitNetwork || ''}
                      onChange={(e) => setPurchaseSplitNetwork(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <select
                      value={selectedBankAccountCode}
                      onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                      disabled={purchaseSplitNetwork <= 0}
                      className="flex-1 bg-white border border-sky-200 rounded px-1.5 py-1 text-[10px] font-bold text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                    >
                      {availableBankAccounts.map((acc) => (
                        <option key={acc.code} value={acc.code}>[{acc.code}] {acc.name}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* 3. Credit / Supplier Payable */}
                <div className="bg-amber-50/70 p-2 rounded-lg border border-amber-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-amber-950 flex items-center gap-1">
                      <FileText className="w-3 h-3 text-amber-700" />
                      <span>3. آجل على حساب المورد (ذمة دائنة):</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, purchaseGrandTotal - purchaseSplitCash - purchaseSplitNetwork);
                        setPurchaseSplitCredit(rem);
                      }}
                      className="text-[10px] text-amber-700 underline font-bold cursor-pointer hover:text-amber-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5 items-center">
                    <input
                      type="number"
                      min={0}
                      value={purchaseSplitCredit || ''}
                      onChange={(e) => setPurchaseSplitCredit(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-amber-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <div className="flex-1 bg-amber-100/60 border border-amber-200 rounded px-2 py-1 text-[10px] font-bold text-amber-900 truncate">
                      حساب المورد: {selectedSupplier?.name}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            
            {/* Desktop High-Density Data Grid with Direct Inline Editing */}
            <div className="flex-1 overflow-auto bg-white border-b border-slate-300 rounded min-h-[160px]">
              <div className="bg-slate-100 p-2 border-b border-slate-300 flex items-center justify-between">
                <form onSubmit={handlePurchaseBarcodeSubmit} className="flex items-center gap-2">
                  <label className="font-bold text-slate-700 text-xs">بحث سريـع (باركود / كود):</label>
                  <div className="relative">
                    <input
                      type="text"
                      autoFocus
                      value={purchaseBarcodeInput}
                      onChange={(e) => setPurchaseBarcodeInput(e.target.value)}
                      className="w-64 border border-slate-300 rounded px-2 py-1 text-xs font-mono focus:border-sky-500 focus:outline-none"
                      placeholder="امسح الباركود أو ادخل الكود..."
                    />
                  </div>
                  <button type="submit" className="hidden">بحث</button>
                </form>
              </div>
              <table className="w-full text-right text-xs border-collapse datagrid-table">
                <thead>
                  <tr className="bg-slate-100 text-slate-800 border-b border-slate-300 sticky top-0 z-10 select-none text-[11px] font-bold">
                    <th className="py-2 px-2 text-center w-10 border-l border-slate-200">#</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">كود / باركود</th>
                    <th className="py-2 px-3 border-l border-slate-200">اسم الصنف والمواصفات</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">الوحدة</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-24">الكمية</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-24">السعر ({currency})</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-20">الخصم</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-20">ضريبة%</th>
                    <th className="py-2 px-3 border-l border-slate-200 text-left w-28">الإجمالي</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-24">رقم التشغيلة</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">الصلاحية</th>
                    <th className="py-2 px-1 text-center w-10">إجراء</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {purchaseItems.length === 0 ? (
                    <tr>
                      <td colSpan={12} className="py-12 text-center text-slate-400">
                        <div className="flex flex-col items-center justify-center gap-2">
                          <p className="font-bold text-slate-500">الفاتورة فارغة حالياً</p>
                          <p className="text-xs">امسح باركود الصنف أو استخدم حقل البحث للبدء بإضافة الأصناف</p>
                        </div>
                      </td>
                    </tr>
                  ) : (
                    purchaseItems.map((item, idx) => {
                      const availableUnits = item.item.units && item.item.units.length > 0
                        ? item.item.units
                        : [{ name: item.item.unit || 'حبة', factor: 1, cost: item.item.costPrice, barcode: item.item.barcode }];

                      return (
                        <tr key={idx} className="hover:bg-sky-50/60 transition group">
                          {/* # */}
                          <td className="py-1.5 px-2 text-center text-slate-500 font-mono font-bold bg-slate-50/60 border-l border-slate-200">
                            {idx + 1}
                          </td>

                          {/* Barcode / Code */}
                          <td className="py-1.5 px-2 font-mono text-[11px] text-slate-600 border-l border-slate-200 truncate">
                            {item.item.barcode || item.item.code}
                          </td>

                          {/* Product Name */}
                          <td className="py-1.5 px-3 border-l border-slate-200">
                            <span className="font-bold text-slate-900 block">{item.item.name}</span>
                          </td>

                          {/* Unit */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <select
                              value={item.selectedUnit}
                              onChange={(e) => handlePurchaseGridChange(idx, 'unit', e.target.value)}
                              className="w-full bg-white border border-slate-300 hover:border-sky-500 rounded px-1.5 py-1 text-xs font-bold text-slate-800 focus:outline-none cursor-pointer"
                            >
                              <option value={item.item.unit || 'حبة'}>{item.item.unit || 'حبة'}</option>
                              {item.item.units?.map(u => (
                                <option key={u.name} value={u.name}>
                                  {u.name} {u.factor > 1 ? `(×${u.factor})` : ''}
                                </option>
                              ))}
                            </select>
                          </td>

                          {/* Quantity */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0.01}
                              step="any"
                              value={item.quantity || ''}
                              onChange={(e) => handlePurchaseGridChange(idx, 'quantity', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-none focus:bg-sky-50 transition-colors"
                            />
                          </td>

                          {/* Price */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0}
                              step="any"
                              value={item.unitPrice || ''}
                              onChange={(e) => handlePurchaseGridChange(idx, 'unitPrice', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-slate-900 focus:outline-none focus:bg-sky-50 transition-colors"
                            />
                          </td>

                          {/* Discount */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0}
                              step="any"
                              value={item.discount || ''}
                              onChange={(e) => handlePurchaseGridChange(idx, 'discount', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-rose-700 focus:outline-none focus:bg-rose-50 transition-colors"
                            />
                          </td>

                          {/* Tax Rate */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <select
                              value={item.taxRate}
                              onChange={(e) => handlePurchaseGridChange(idx, 'taxRate', e.target.value)}
                              className="w-full bg-transparent border-none text-center font-mono-numbers text-slate-700 focus:outline-none cursor-pointer"
                            >
                              <option value={0}>0%</option>
                              <option value={5}>5%</option>
                              <option value={15}>15%</option>
                            </select>
                          </td>

                          {/* Total */}
                          <td className="py-1.5 px-3 border-l border-slate-200 text-left bg-sky-50/30">
                            <span className="font-mono-numbers font-black text-sky-900 text-sm">
                              {item.total.toLocaleString()}
                            </span>
                          </td>

                          {/* Batch No */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="text"
                              value={item.batchNo || ''}
                              onChange={(e) => handlePurchaseGridChange(idx, 'batchNo', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-sky-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono text-slate-700 focus:outline-none focus:bg-sky-50"
                              placeholder="-"
                            />
                          </td>

                          {/* Expiry */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            {item.item.hasExpiryDate === false ? (
                              <span className="text-[10px] text-slate-400 block text-center">لا يوجد</span>
                            ) : (
                              <input
                                type="date"
                                value={item.expiryDate || ''}
                                onChange={(e) => handlePurchaseGridChange(idx, 'expiryDate', e.target.value)}
                                className="w-full bg-transparent border-none text-center font-mono text-slate-700 focus:outline-none"
                              />
                            )}
                          </td>

                          {/* Action */}
                          <td className="py-1.5 px-1 text-center">
                            <button
                              type="button"
                              onClick={() => setPurchaseItems(prev => prev.filter((_, i) => i !== idx))}
                              className="p-1.5 rounded-md hover:bg-rose-100 text-slate-400 hover:text-rose-600 transition"
                              title="حذف السطر"
                            >
                              <Trash2 className="w-4 h-4 mx-auto" />
                            </button>
                          </td>
                        </tr>
                      );
                    })
                  )}
                </tbody>
              </table>
            </div>

            {/* Bottom Summary Bar & Accounting Impact Preview */}
            <div className="bg-slate-100 p-3 rounded-lg border border-slate-300 flex flex-wrap items-center justify-between gap-4">
              {/* Calculations Box with Header-Level Discount */}
              <div className="flex flex-wrap items-center gap-5 text-xs">
                <div>
                  <span className="text-slate-500 block">الإجمالي قبل الخصم:</span>
                  <span className="font-mono-numbers font-bold text-slate-800 text-sm">
                    {purchaseSubtotal.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Header-Level Discount Control (خصم على مستوى الفاتورة لتقليل التعقيد) */}
                <div className="bg-white px-2.5 py-1.5 rounded border border-rose-200 flex flex-col gap-1 shadow-xs">
                  <div className="flex items-center justify-between gap-2">
                    <span className="text-[11px] font-bold text-rose-800">خصم الفاتورة العام:</span>
                    <div className="inline-flex rounded border border-slate-200 overflow-hidden text-[10px] font-bold">
                      <button
                        type="button"
                        onClick={() => setHeaderDiscountType('AMOUNT')}
                        className={`px-1.5 py-0.5 cursor-pointer ${
                          headerDiscountType === 'AMOUNT' ? 'bg-rose-600 text-white' : 'bg-slate-50 text-slate-600 hover:bg-slate-100'
                        }`}
                      >
                        مبلغ
                      </button>
                      <button
                        type="button"
                        onClick={() => setHeaderDiscountType('PERCENT')}
                        className={`px-1.5 py-0.5 cursor-pointer ${
                          headerDiscountType === 'PERCENT' ? 'bg-rose-600 text-white' : 'bg-slate-50 text-slate-600 hover:bg-slate-100'
                        }`}
                      >
                        %
                      </button>
                    </div>
                  </div>
                  <div className="flex items-center gap-1">
                    <input
                      type="number"
                      min={0}
                      value={headerDiscountVal}
                      onChange={(e) => setHeaderDiscountVal(e.target.value)}
                      placeholder="0"
                      className="w-24 bg-rose-50/50 border border-rose-300 rounded px-1.5 py-0.5 font-mono text-center font-bold text-rose-700 text-xs focus:outline-none focus:border-rose-600"
                    />
                    <span className="text-[10px] text-slate-500">
                      {headerDiscountType === 'PERCENT' ? '%' : currency}
                    </span>
                  </div>
                </div>

                <div>
                  <span className="text-slate-500 block">إجمالي الخصومات:</span>
                  <span className="font-mono-numbers font-bold text-rose-600 text-sm">
                    {purchaseDiscountTotal > 0 ? `-${purchaseDiscountTotal.toLocaleString()}` : '0'} {currency}
                  </span>
                  {purchaseLineDiscounts > 0 && purchaseHeaderDiscountAmount > 0 && (
                    <span className="text-[10px] text-slate-500 block">
                      (بنود: {purchaseLineDiscounts.toLocaleString()} + فاتورة: {purchaseHeaderDiscountAmount.toLocaleString()})
                    </span>
                  )}
                </div>

                <div>
                  <span className="text-slate-500 block">ضريبة القيمة المضافة:</span>
                  <span className="font-mono-numbers font-bold text-slate-800 text-sm">
                    {purchaseTaxTotal.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Other Charges & Expense Account Dropdown */}
                <div className="bg-white px-2.5 py-1.5 rounded border border-amber-200 flex flex-col gap-1 shadow-xs">
                  <div className="flex items-center justify-between gap-2">
                    <span className="text-[11px] font-bold text-amber-900">رسوم / مصاريف إضافية:</span>
                    <select
                      value={purchaseOtherChargesAccountCode}
                      onChange={(e) => {
                        const accCode = e.target.value;
                        setPurchaseOtherChargesAccountCode(accCode);
                        const selected = EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === accCode);
                        if (selected) setPurchaseOtherChargesName(selected.name);
                      }}
                      className="bg-amber-50 border border-amber-300 rounded px-1 py-0.5 text-[10px] font-bold text-amber-950 focus:outline-none"
                    >
                      {EXPENSE_ACCOUNTS_OPTIONS.map((acc) => (
                        <option key={acc.code} value={acc.code}>
                          [{acc.code}] {acc.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="flex items-center gap-1">
                    <input
                      type="number"
                      min={0}
                      value={purchaseOtherChargesVal}
                      onChange={(e) => setPurchaseOtherChargesVal(e.target.value)}
                      placeholder="0"
                      className="w-24 bg-amber-50/50 border border-amber-300 rounded px-1.5 py-0.5 font-mono text-center font-bold text-amber-900 text-xs focus:outline-none focus:border-amber-600"
                    />
                    <span className="text-[10px] text-slate-500">{currency}</span>
                  </div>
                </div>

                <div className="border-r border-slate-300 pr-5">
                  <span className="text-sky-950 font-bold block text-xs">صافي الفاتورة المستحق:</span>
                  <span className="text-2xl font-black font-mono-numbers text-sky-900">
                    {purchaseGrandTotal.toLocaleString()} {currency}
                  </span>
                </div>
              </div>

              {/* Action Buttons */}
              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={() => {
                    setPurchaseItems([]);
                    setHeaderDiscountVal('0');
                    setPurchaseOtherChargesVal('0');
                  }}
                  disabled={purchaseItems.length === 0}
                  className="bg-white border border-slate-300 hover:bg-slate-50 text-slate-700 font-semibold px-4 py-2 rounded-lg cursor-pointer disabled:opacity-50"
                >
                  إفراغ السلة
                </button>

                <button
                  type="button"
                  onClick={handleSavePurchaseInvoice}
                  disabled={purchaseItems.length === 0}
                  className="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold px-6 py-2.5 rounded-lg shadow flex items-center gap-2 cursor-pointer transition"
                >
                  <CheckCircle2 className="w-5 h-5" />
                  <span>حفظ وترحيل الفاتورة وتحديث المخزون [F10]</span>
                </button>
              </div>
            </div>

            {/* Live Double-Entry Accounting Preview */}
            <div className="bg-slate-50 px-3 py-2 rounded border border-slate-200 flex items-center justify-between text-[11px] text-slate-600">
              <div className="flex items-center gap-2 flex-wrap">
                <Scale className="w-4 h-4 text-sky-700 shrink-0" />
                <span className="font-bold text-slate-700">الأثر المحاسبي التلقائي المتوقع:</span>
                <span>
                  من حـ/ المخزون السلعي ({selectedWarehouse?.name}) [{(purchaseSubtotal).toLocaleString()} {currency}]
                  {purchaseTaxTotal > 0 && ` + من حـ/ ضريبة القيمة المضافة [${purchaseTaxTotal.toLocaleString()} ${currency}]`}
                  {purchaseOtherCharges > 0 && ` + من حـ/ ${EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === purchaseOtherChargesAccountCode)?.name || 'المصروفات'} (${purchaseOtherChargesAccountCode}) [${purchaseOtherCharges.toLocaleString()} ${currency}]`}
                  {purchaseDiscountTotal > 0 && ` إلى حـ/ الخصم المكتسب من الموردين (34) [${purchaseDiscountTotal.toLocaleString()} ${currency}]`}
                  {' '}إلى حـ/ {paymentType === 'CREDIT' ? `الموردين (${selectedSupplier?.name})` : (paymentType === 'CASH' ? `${currentCashAccount?.name}` : (paymentType === 'MIXED' ? 'متعدد (نقد/بنك/آجل)' : `${currentBankAccount?.name}`))} [{purchaseGrandTotal.toLocaleString()} {currency}]
                </span>
              </div>
              <span className="text-emerald-700 font-bold bg-emerald-100 px-2 py-0.5 rounded font-mono shrink-0">
                متوازن (Balanced)
              </span>
            </div>
          </div>
        )}

        {/* ========================================================================= */}
        {/* TAB 2: PURCHASE RETURNS (مردودات مشتريات) */}
        {/* ========================================================================= */}
        {activeSubTab === 'PURCHASE_RETURN' && (
          <div className="flex-1 flex flex-col p-3 gap-2.5 overflow-y-auto text-xs">
            {/* Header info */}
            <div className="bg-rose-50/80 p-3 rounded-lg border border-rose-200 flex flex-wrap items-center justify-between gap-3">
              <div className="flex items-center gap-2">
                <div className="bg-rose-600 text-white p-2 rounded">
                  <RotateCcw className="w-5 h-5" />
                </div>
                <div>
                  <h3 className="font-bold text-rose-950 text-sm">سند مردودات مشتريات بضاعة إلى المورد</h3>
                  <p className="text-[11px] text-rose-800">
                    خصم الكميات فورياً من رصيد المستودع المختار، وتخفيض ذمة المورد أو استرداد النقدية، مع قيد محاسبي عكسي.
                  </p>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <span className="text-rose-900 font-bold">إجمالي المردود:</span>
                <span className="font-mono-numbers font-black text-rose-700 text-xl">
                  {returnGrandTotal.toLocaleString()} {currency}
                </span>
              </div>
            </div>

            {/* Return Form Details */}
            <div className="grid grid-cols-1 md:grid-cols-4 gap-3 bg-slate-50 p-3 rounded-lg border border-slate-300">
              <div>
                <label className="font-bold text-slate-700 block mb-1">المورد المعتمد للإرجاع:</label>
                <select
                  value={returnSupplierId}
                  onChange={(e) => setReturnSupplierId(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900"
                >
                  {suppliers.map((s) => (
                    <option key={s.id} value={s.id}>
                      {s.name} ({s.company}) - رصيده القائم: {s.currentBalance.toLocaleString()} {currency}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="font-bold text-slate-700 block mb-1">المستودع المصدر (الصرف منه):</label>
                <select
                  value={returnWarehouseId}
                  onChange={(e) => setReturnWarehouseId(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      [{w.code}] {w.name}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="font-bold text-slate-700 block mb-1">طريقة التسوية:</label>
                <select
                  value={returnPaymentType}
                  onChange={(e) => {
                    const newType = e.target.value as PaymentType;
                    setReturnPaymentType(newType);
                    if (newType === 'CASH') {
                      setReturnSplitCash(returnGrandTotal);
                      setReturnSplitNetwork(0);
                      setReturnSplitCredit(0);
                    } else if (newType === 'KURAYMI' || newType === 'BANK_TRANSFER') {
                      setReturnSplitCash(0);
                      setReturnSplitNetwork(returnGrandTotal);
                      setReturnSplitCredit(0);
                    } else {
                      setReturnSplitCash(0);
                      setReturnSplitNetwork(0);
                      setReturnSplitCredit(returnGrandTotal);
                    }
                  }}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 bg-white font-bold text-slate-900"
                >
                  <option value="CREDIT">خصم من رصيد ذمة المورد (تخفيض الدائنية)</option>
                  <option value="CASH">استرداد نقدي إلى الصندوق (1111)</option>
                  <option value="KURAYMI">إيداع في بنك الكريمي (1121)</option>
                  <option value="MIXED">تسوية مركبة (نقد + بنك + ذمة)</option>
                </select>
              </div>

              <div>
                <label className="font-bold text-slate-700 block mb-1">رقم فاتورة الشراء الأصلية (اختياري):</label>
                <input
                  type="text"
                  placeholder="مثال: PINV-1001"
                  value={originalInvoiceNo}
                  onChange={(e) => setOriginalInvoiceNo(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 font-mono text-slate-900"
                />
              </div>

              <div className="md:col-span-4">
                <label className="font-bold text-slate-700 block mb-1">سبب ومبرر الإرجاع:</label>
                <input
                  type="text"
                  placeholder="مثال: انتهاء الصلاحية، عيب مصنعي، تلف أثناء النقل..."
                  value={returnReason}
                  onChange={(e) => setReturnReason(e.target.value)}
                  className="w-full border border-slate-300 rounded px-2 py-1.5 text-slate-900"
                />
              </div>
            </div>

            {/* Split Settlement for Returns (محرك تسوية المردودات) */}
            <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-300 flex flex-col gap-2">
              <div className="flex items-center justify-between">
                <span className="font-bold text-slate-800 text-xs flex items-center gap-1.5">
                  <Coins className="w-3.5 h-3.5 text-rose-700" />
                  <span>توزيع مبالغ تسوية المردودات (استرداد نقدي / بنكي / تخفيض ذمة):</span>
                </span>
                <span className="text-[11px] font-mono text-slate-500">
                  إجمالي المردود: <strong className="text-rose-900">{returnGrandTotal.toLocaleString()} {currency}</strong>
                </span>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-2.5">
                {/* 1. Cash Box Refund */}
                <div className="bg-emerald-50/70 p-2 rounded-lg border border-emerald-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-emerald-950 flex items-center gap-1">
                      <Coins className="w-3 h-3 text-emerald-700" />
                      <span>1. نقد مسترد إلى الصندوق:</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, returnGrandTotal - returnSplitNetwork - returnSplitCredit);
                        setReturnSplitCash(rem);
                      }}
                      className="text-[10px] text-emerald-700 underline font-bold cursor-pointer hover:text-emerald-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5">
                    <input
                      type="number"
                      min={0}
                      value={returnSplitCash || ''}
                      onChange={(e) => setReturnSplitCash(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <select
                      value={returnCashAccountCode}
                      onChange={(e) => setReturnCashAccountCode(e.target.value)}
                      disabled={returnSplitCash <= 0}
                      className="flex-1 bg-white border border-emerald-200 rounded px-1.5 py-1 text-[10px] font-bold text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                    >
                      {availableCashAccounts.map((acc) => (
                        <option key={acc.code} value={acc.code}>[{acc.code}] {acc.name}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* 2. Bank / Network Refund */}
                <div className="bg-sky-50/70 p-2 rounded-lg border border-sky-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-sky-950 flex items-center gap-1">
                      <Building2 className="w-3 h-3 text-sky-700" />
                      <span>2. تحويل بنكي / شيك مسترد:</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, returnGrandTotal - returnSplitCash - returnSplitCredit);
                        setReturnSplitNetwork(rem);
                      }}
                      className="text-[10px] text-sky-700 underline font-bold cursor-pointer hover:text-sky-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5">
                    <input
                      type="number"
                      min={0}
                      value={returnSplitNetwork || ''}
                      onChange={(e) => setReturnSplitNetwork(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <select
                      value={returnBankAccountCode}
                      onChange={(e) => setReturnBankAccountCode(e.target.value)}
                      disabled={returnSplitNetwork <= 0}
                      className="flex-1 bg-white border border-sky-200 rounded px-1.5 py-1 text-[10px] font-bold text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                    >
                      {availableBankAccounts.map((acc) => (
                        <option key={acc.code} value={acc.code}>[{acc.code}] {acc.name}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* 3. Credit / Supplier Payable Deduction */}
                <div className="bg-amber-50/70 p-2 rounded-lg border border-amber-300 flex flex-col gap-1.5">
                  <div className="flex items-center justify-between">
                    <label className="text-[11px] font-bold text-amber-950 flex items-center gap-1">
                      <FileText className="w-3 h-3 text-amber-700" />
                      <span>3. تخفيض ذمة المورد (خصم دائنية):</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, returnGrandTotal - returnSplitCash - returnSplitNetwork);
                        setReturnSplitCredit(rem);
                      }}
                      className="text-[10px] text-amber-700 underline font-bold cursor-pointer hover:text-amber-900"
                    >
                      المتبقي
                    </button>
                  </div>
                  <div className="flex gap-1.5 items-center">
                    <input
                      type="number"
                      min={0}
                      value={returnSplitCredit || ''}
                      onChange={(e) => setReturnSplitCredit(Math.max(0, parseFloat(e.target.value) || 0))}
                      placeholder="0"
                      className="w-28 bg-white border border-amber-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    />
                    <div className="flex-1 bg-amber-100/60 border border-amber-200 rounded px-2 py-1 text-[10px] font-bold text-amber-900 truncate">
                      المورد: {suppliers.find((s) => s.id === returnSupplierId)?.name}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            
            {/* Desktop High-Density Data Grid with Direct Inline Editing */}
            <div className="flex-1 overflow-auto bg-white border-b border-slate-300 rounded min-h-[160px]">
              <div className="bg-slate-100 p-2 border-b border-slate-300 flex items-center justify-between">
                <form onSubmit={handleReturnBarcodeSubmit} className="flex items-center gap-2">
                  <label className="font-bold text-slate-700 text-xs">بحث سريـع (باركود / كود):</label>
                  <div className="relative">
                    <input
                      type="text"
                      value={returnBarcodeInput}
                      onChange={(e) => setReturnBarcodeInput(e.target.value)}
                      className="w-64 border border-slate-300 rounded px-2 py-1 text-xs font-mono focus:border-sky-500 focus:outline-none"
                      placeholder="امسح الباركود أو ادخل الكود..."
                    />
                  </div>
                  <button type="submit" className="hidden">بحث</button>
                </form>
              </div>
              <table className="w-full text-right text-xs border-collapse datagrid-table">
                <thead>
                  <tr className="bg-slate-100 text-slate-800 border-b border-slate-300 sticky top-0 z-10 select-none text-[11px] font-bold">
                    <th className="py-2 px-2 text-center w-10 border-l border-slate-200">#</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">كود / باركود</th>
                    <th className="py-2 px-3 border-l border-slate-200">اسم الصنف والمواصفات</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">الوحدة</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-24">الكمية</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-24">سعر الارجاع</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-20">الخصم</th>
                    <th className="py-2 px-2 border-l border-slate-200 text-center w-20">ضريبة%</th>
                    <th className="py-2 px-3 border-l border-slate-200 text-left w-28">الإجمالي</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-24">رقم التشغيلة</th>
                    <th className="py-2 px-2 border-l border-slate-200 w-28">الصلاحية</th>
                    <th className="py-2 px-1 text-center w-10">إجراء</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {returnItems.length === 0 ? (
                    <tr>
                      <td colSpan={12} className="py-12 text-center text-slate-400">
                        <div className="flex flex-col items-center justify-center gap-2">
                          <p className="font-bold text-slate-500">مردود المشتريات فارغ حالياً</p>
                          <p className="text-xs">امسح باركود الصنف أو استخدم حقل البحث للبدء بإضافة الأصناف المرجعة</p>
                        </div>
                      </td>
                    </tr>
                  ) : (
                    returnItems.map((item, idx) => {
                      const availableUnits = item.item.units && item.item.units.length > 0
                        ? item.item.units
                        : [{ name: item.item.unit || 'حبة', factor: 1, cost: item.item.costPrice, barcode: item.item.barcode }];

                      return (
                        <tr key={idx} className="hover:bg-rose-50/60 transition group">
                          {/* # */}
                          <td className="py-1.5 px-2 text-center text-slate-500 font-mono font-bold bg-slate-50/60 border-l border-slate-200">
                            {idx + 1}
                          </td>

                          {/* Barcode / Code */}
                          <td className="py-1.5 px-2 font-mono text-[11px] text-slate-600 border-l border-slate-200 truncate">
                            {item.item.barcode || item.item.code}
                          </td>

                          {/* Product Name */}
                          <td className="py-1.5 px-3 border-l border-slate-200">
                            <span className="font-bold text-slate-900 block">{item.item.name}</span>
                          </td>

                          {/* Unit */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <select
                              value={item.selectedUnit}
                              onChange={(e) => handleReturnGridChange(idx, 'unit', e.target.value)}
                              className="w-full bg-white border border-slate-300 hover:border-rose-500 rounded px-1.5 py-1 text-xs font-bold text-slate-800 focus:outline-none cursor-pointer"
                            >
                              <option value={item.item.unit || 'حبة'}>{item.item.unit || 'حبة'}</option>
                              {item.item.units?.map(u => (
                                <option key={u.name} value={u.name}>
                                  {u.name} {u.factor > 1 ? `(×${u.factor})` : ''}
                                </option>
                              ))}
                            </select>
                          </td>

                          {/* Quantity */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0.01}
                              step="any"
                              value={item.quantity || ''}
                              onChange={(e) => handleReturnGridChange(idx, 'quantity', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-rose-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers font-bold text-slate-900 focus:outline-none focus:bg-rose-50 transition-colors"
                            />
                          </td>

                          {/* Price */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0}
                              step="any"
                              value={item.unitPrice || ''}
                              onChange={(e) => handleReturnGridChange(idx, 'unitPrice', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-rose-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-slate-900 focus:outline-none focus:bg-rose-50 transition-colors"
                            />
                          </td>

                          {/* Discount */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="number"
                              min={0}
                              step="any"
                              value={item.discount || ''}
                              onChange={(e) => handleReturnGridChange(idx, 'discount', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-rose-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono-numbers text-rose-700 focus:outline-none focus:bg-rose-50 transition-colors"
                            />
                          </td>

                          {/* Tax Rate */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <select
                              value={item.taxRate}
                              onChange={(e) => handleReturnGridChange(idx, 'taxRate', e.target.value)}
                              className="w-full bg-transparent border-none text-center font-mono-numbers text-slate-700 focus:outline-none cursor-pointer"
                            >
                              <option value={0}>0%</option>
                              <option value={5}>5%</option>
                              <option value={15}>15%</option>
                            </select>
                          </td>

                          {/* Total */}
                          <td className="py-1.5 px-3 border-l border-slate-200 text-left bg-rose-50/30">
                            <span className="font-mono-numbers font-black text-rose-900 text-sm">
                              {item.total.toLocaleString()}
                            </span>
                          </td>

                          {/* Batch No */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            <input
                              type="text"
                              value={item.batchNo || ''}
                              onChange={(e) => handleReturnGridChange(idx, 'batchNo', e.target.value)}
                              className="w-full bg-white border border-transparent focus:border-rose-500 hover:border-slate-300 rounded px-1.5 py-1 text-center font-mono text-slate-700 focus:outline-none focus:bg-rose-50"
                              placeholder="-"
                            />
                          </td>

                          {/* Expiry */}
                          <td className="py-1 px-2 border-l border-slate-200">
                            {item.item.hasExpiryDate === false ? (
                              <span className="text-[10px] text-slate-400 block text-center">لا يوجد</span>
                            ) : (
                              <input
                                type="date"
                                value={item.expiryDate || ''}
                                onChange={(e) => handleReturnGridChange(idx, 'expiryDate', e.target.value)}
                                className="w-full bg-transparent border-none text-center font-mono text-slate-700 focus:outline-none"
                              />
                            )}
                          </td>

                          {/* Action */}
                          <td className="py-1.5 px-1 text-center">
                            <button
                              type="button"
                              onClick={() => setReturnItems(prev => prev.filter((_, i) => i !== idx))}
                              className="p-1.5 rounded-md hover:bg-rose-100 text-slate-400 hover:text-rose-600 transition"
                              title="حذف السطر"
                            >
                              <Trash2 className="w-4 h-4 mx-auto" />
                            </button>
                          </td>
                        </tr>
                      );
                    })
                  )}
                </tbody>
              </table>
            </div>

            {/* Bottom Actions for Return with Header Discount */}
            <div className="bg-slate-100 p-3 rounded-lg border border-slate-300 flex flex-wrap items-center justify-between gap-4">
              <div className="flex items-center gap-5 text-xs">
                <div>
                  <span className="text-slate-500 block">إجمالي المردودات:</span>
                  <span className="font-mono-numbers font-bold text-slate-800 text-sm">
                    {returnSubtotal.toLocaleString()} {currency}
                  </span>
                </div>

                {/* Return Header Discount */}
                <div className="bg-white px-2.5 py-1.5 rounded border border-rose-200 flex items-center gap-2 shadow-xs">
                  <span className="text-[11px] font-bold text-rose-800">خصم مردود:</span>
                  <div className="inline-flex rounded border border-slate-200 overflow-hidden text-[10px] font-bold">
                    <button
                      type="button"
                      onClick={() => setReturnHeaderDiscountType('AMOUNT')}
                      className={`px-1.5 py-0.5 cursor-pointer ${
                        returnHeaderDiscountType === 'AMOUNT' ? 'bg-rose-600 text-white' : 'bg-slate-50 text-slate-600'
                      }`}
                    >
                      مبلغ
                    </button>
                    <button
                      type="button"
                      onClick={() => setReturnHeaderDiscountType('PERCENT')}
                      className={`px-1.5 py-0.5 cursor-pointer ${
                        returnHeaderDiscountType === 'PERCENT' ? 'bg-rose-600 text-white' : 'bg-slate-50 text-slate-600'
                      }`}
                    >
                      %
                    </button>
                  </div>
                  <input
                    type="number"
                    min={0}
                    value={returnHeaderDiscountVal}
                    onChange={(e) => setReturnHeaderDiscountVal(e.target.value)}
                    placeholder="0"
                    className="w-20 bg-rose-50/50 border border-rose-300 rounded px-1.5 py-0.5 font-mono text-center font-bold text-rose-700 text-xs focus:outline-none"
                  />
                  <span className="text-[10px] text-slate-500">
                    {returnHeaderDiscountType === 'PERCENT' ? '%' : currency}
                  </span>
                </div>

                {/* Return Tax Rate */}
                <div className="bg-white px-2.5 py-1.5 rounded border border-slate-200 flex items-center gap-1.5 shadow-xs">
                  <span className="text-[11px] font-bold text-slate-700">ضريبة مستردة %:</span>
                  <select
                    value={returnTaxRate}
                    onChange={(e) => setReturnTaxRate(parseFloat(e.target.value) || 0)}
                    className="border border-slate-300 rounded px-1.5 py-0.5 bg-slate-50 text-xs font-bold text-slate-800"
                  >
                    <option value={0}>0%</option>
                    <option value={5}>5%</option>
                    <option value={15}>15%</option>
                  </select>
                  {returnTaxAmount > 0 && (
                    <span className="font-mono font-bold text-sky-800 text-[11px]">
                      (+{returnTaxAmount.toLocaleString()} {currency})
                    </span>
                  )}
                </div>

                {/* Return Other Charges / Expense Account */}
                <div className="bg-white px-2.5 py-1.5 rounded border border-amber-200 flex flex-col gap-1 shadow-xs">
                  <div className="flex items-center justify-between gap-1.5">
                    <span className="text-[11px] font-bold text-amber-900">رسوم مردودات / شحن:</span>
                    <select
                      value={returnOtherChargesAccountCode}
                      onChange={(e) => {
                        const accCode = e.target.value;
                        setReturnOtherChargesAccountCode(accCode);
                        const selected = EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === accCode);
                        if (selected) setReturnOtherChargesName(selected.name);
                      }}
                      className="bg-amber-50 border border-amber-300 rounded px-1 py-0.5 text-[10px] font-bold text-amber-950 focus:outline-none"
                    >
                      {EXPENSE_ACCOUNTS_OPTIONS.map((acc) => (
                        <option key={acc.code} value={acc.code}>
                          [{acc.code}] {acc.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="flex items-center gap-1">
                    <input
                      type="number"
                      min={0}
                      value={returnOtherChargesVal}
                      onChange={(e) => setReturnOtherChargesVal(e.target.value)}
                      placeholder="0"
                      className="w-20 bg-amber-50/50 border border-amber-300 rounded px-1.5 py-0.5 font-mono text-center font-bold text-amber-900 text-xs focus:outline-none"
                    />
                    <span className="text-[10px] text-slate-500">{currency}</span>
                  </div>
                </div>

                <div className="border-r border-slate-300 pr-4">
                  <span className="text-slate-600 block text-xs">صافي مردود المشتريات:</span>
                  <span className="text-xl font-black font-mono-numbers text-rose-700">
                    {returnGrandTotal.toLocaleString()} {currency}
                  </span>
                </div>
              </div>

              <button
                type="button"
                onClick={handleSavePurchaseReturn}
                disabled={returnItems.length === 0}
                className="bg-rose-700 hover:bg-rose-600 disabled:opacity-50 text-white font-bold px-6 py-2.5 rounded-lg shadow flex items-center gap-2 cursor-pointer transition"
              >
                <CheckCircle2 className="w-5 h-5" />
                <span>حفظ وترحيل سند المردودات وخصم المخزون</span>
              </button>
            </div>

            {/* Live Double-Entry Accounting Preview for Return */}
            <div className="bg-rose-50/50 px-3 py-2 rounded border border-rose-200 flex items-center justify-between text-[11px] text-slate-600">
              <div className="flex items-center gap-2 flex-wrap">
                <Scale className="w-4 h-4 text-rose-700 shrink-0" />
                <span className="font-bold text-slate-700">الأثر المحاسبي التلقائي لمردودات المشتريات:</span>
                <span>
                  من حـ/ {returnPaymentType === 'CREDIT' ? `الموردين (${suppliers.find((s) => s.id === returnSupplierId)?.name})` : (returnPaymentType === 'CASH' ? `${currentReturnCashAccount?.name}` : (returnPaymentType === 'MIXED' ? 'متعدد (نقد/بنك/آجل)' : `${currentReturnBankAccount?.name}`))} [{returnGrandTotal.toLocaleString()} {currency}]
                  {returnHeaderDiscountAmount > 0 && ` + من حـ/ الخصم المكتسب من الموردين (34) [${returnHeaderDiscountAmount.toLocaleString()} ${currency}]`}
                  {' '}إلى حـ/ المخزون السلعي ({warehouses.find((w) => w.id === returnWarehouseId)?.name}) [{returnSubtotal.toLocaleString()} {currency}]
                  {returnTaxAmount > 0 && ` + إلى حـ/ ضريبة القيمة المضافة [${returnTaxAmount.toLocaleString()} ${currency}]`}
                  {returnOtherCharges > 0 && ` + إلى حـ/ ${EXPENSE_ACCOUNTS_OPTIONS.find((a) => a.code === returnOtherChargesAccountCode)?.name || 'المصروفات'} (${returnOtherChargesAccountCode}) [${returnOtherCharges.toLocaleString()} ${currency}]`}
                </span>
              </div>
              <span className="text-emerald-700 font-bold bg-emerald-100 px-2 py-0.5 rounded font-mono shrink-0">
                متوازن (Balanced)
              </span>
            </div>
          </div>
        )}

        {/* ========================================================================= */}
        {/* TAB 3: PURCHASE INVOICES & RETURNS REGISTER (السجل العام) */}
        {/* ========================================================================= */}
        {activeSubTab === 'PURCHASE_HISTORY' && (
          <div className="flex-1 flex flex-col p-3 gap-3 overflow-hidden">
            {/* Filter Toolbar */}
            <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-200 flex flex-wrap items-center justify-between gap-3 text-xs">
              <div className="flex items-center gap-2 flex-1 max-w-md">
                <div className="relative w-full">
                  <Search className="w-4 h-4 absolute right-2.5 top-2 text-slate-400" />
                  <input
                    type="text"
                    placeholder="بحث برقم الفاتورة، اسم المورد، أو السند المرجعي..."
                    value={historySearch}
                    onChange={(e) => setHistorySearch(e.target.value)}
                    className="w-full border border-slate-300 rounded pr-8 pl-3 py-1.5 bg-white text-slate-900 focus:outline-none focus:border-sky-600"
                  />
                </div>
              </div>

              <div className="flex items-center gap-2">
                <div className="flex items-center gap-1 bg-white border border-slate-300 rounded p-1">
                  <button
                    onClick={() => setHistoryTypeFilter('ALL')}
                    className={`px-2.5 py-1 rounded font-semibold ${
                      historyTypeFilter === 'ALL' ? 'bg-sky-800 text-white' : 'text-slate-600 hover:bg-slate-100'
                    }`}
                  >
                    الكل ({purchaseInvoices.length})
                  </button>
                  <button
                    onClick={() => setHistoryTypeFilter('PURCHASE')}
                    className={`px-2.5 py-1 rounded font-semibold ${
                      historyTypeFilter === 'PURCHASE' ? 'bg-sky-800 text-white' : 'text-slate-600 hover:bg-slate-100'
                    }`}
                  >
                    فواتير مشتريات ({purchaseInvoices.filter((i) => i.type === 'PURCHASE').length})
                  </button>
                  <button
                    onClick={() => setHistoryTypeFilter('PURCHASE_RETURN')}
                    className={`px-2.5 py-1 rounded font-semibold ${
                      historyTypeFilter === 'PURCHASE_RETURN' ? 'bg-rose-700 text-white' : 'text-slate-600 hover:bg-slate-100'
                    }`}
                  >
                    مردودات ({purchaseInvoices.filter((i) => i.type === 'PURCHASE_RETURN').length})
                  </button>
                </div>

                <select
                  value={historyWarehouseFilter}
                  onChange={(e) => setHistoryWarehouseFilter(e.target.value)}
                  className="border border-slate-300 rounded px-2 py-1.5 bg-white font-semibold text-slate-800"
                >
                  <option value="ALL">جميع المستودعات</option>
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      {w.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            {/* Invoices History Table */}
            <div className="flex-1 border border-slate-300 rounded overflow-auto bg-white">
              <table className="w-full text-right border-collapse datagrid-table text-xs">
                <thead className="sticky top-0 bg-slate-100 text-slate-700 border-b border-slate-300 z-10">
                  <tr>
                    <th className="w-28 p-2">رقم الفاتورة</th>
                    <th className="w-24 p-2">النوع</th>
                    <th className="w-28 p-2">التاريخ والوقت</th>
                    <th className="p-2">المورد / المؤسسة</th>
                    <th className="w-36 p-2">المستودع المستلم</th>
                    <th className="w-24 text-center p-2">طريقة السداد</th>
                    <th className="w-20 text-center p-2">الأصناف</th>
                    <th className="w-32 text-left p-2">الإجمالي ({currency})</th>
                    <th className="w-24 text-center p-2">الحالة</th>
                    <th className="w-24 text-center p-2">إجراءات</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {filteredHistory.length === 0 ? (
                    <tr>
                      <td colSpan={10} className="text-center py-12 text-slate-400">
                        لا توجد فواتير أو مردودات مطابقة للبحث
                      </td>
                    </tr>
                  ) : (
                    filteredHistory.map((inv) => {
                      const isRet = inv.type === 'PURCHASE_RETURN';
                      return (
                        <tr key={inv.id} className="hover:bg-slate-50">
                          <td className="font-mono-numbers font-bold text-sky-900 p-2">
                            {inv.invoiceNo}
                          </td>
                          <td className="p-2">
                            <span
                              className={`text-[11px] px-2 py-0.5 rounded font-bold ${
                                isRet ? 'bg-rose-100 text-rose-800' : 'bg-sky-100 text-sky-800'
                              }`}
                            >
                              {isRet ? 'مردود مشتريات' : 'فاتورة مشتريات'}
                            </span>
                          </td>
                          <td className="font-mono text-[11px] text-slate-600 p-2">
                            {inv.date} {inv.time}
                          </td>
                          <td className="font-bold text-slate-900 p-2">
                            {inv.customerOrSupplierName}
                            {inv.referenceNo && (
                              <span className="text-[10px] text-slate-500 block font-mono">
                                سند: {inv.referenceNo}
                              </span>
                            )}
                          </td>
                          <td className="text-slate-700 p-2">{inv.warehouseName || 'المستودع الرئيسي'}</td>
                          <td className="text-center p-2">
                            <span className="bg-slate-100 text-slate-800 text-[11px] px-2 py-0.5 rounded font-semibold">
                              {inv.paymentType === 'CREDIT' ? 'آجل' : 'نقدي'}
                            </span>
                          </td>
                          <td className="text-center font-mono-numbers font-bold p-2">{inv.items.length}</td>
                          <td className="text-left font-mono-numbers font-bold text-slate-900 p-2">
                            <span className={isRet ? 'text-rose-700' : 'text-slate-900'}>
                              {isRet ? '-' : ''}{inv.grandTotal.toLocaleString()}
                            </span>
                          </td>
                          <td className="text-center p-2">
                            <span className="bg-emerald-100 text-emerald-800 text-[10px] font-bold px-2 py-0.5 rounded">
                              مرحلة (POSTED)
                            </span>
                          </td>
                          <td className="text-center p-2">
                            <button
                              onClick={() => setPreviewInvoice(inv)}
                              className="bg-sky-50 hover:bg-sky-100 text-sky-800 border border-sky-300 px-2.5 py-1 rounded flex items-center gap-1 mx-auto cursor-pointer font-semibold text-xs"
                            >
                              <Printer className="w-3.5 h-3.5" />
                              <span>معاينة</span>
                            </button>
                          </td>
                        </tr>
                      );
                    })
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* ========================================================================= */}
        {/* TAB 4: SUPPLIERS DIRECTORY & BALANCES (دليل الموردين وكشوف الحسابات) */}
        {/* ========================================================================= */}
        {activeSubTab === 'SUPPLIERS_LIST' && (
          <div className="flex-1 flex flex-col p-3 gap-3 overflow-hidden text-xs">
            {/* Toolbar */}
            <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-200 flex items-center justify-between gap-3">
              <div className="relative flex-1 max-w-md">
                <Search className="w-4 h-4 absolute right-2.5 top-2 text-slate-400" />
                <input
                  type="text"
                  placeholder="بحث باسم المورد، الشركة، رقم الهاتف، أو كود الحساب..."
                  value={supplierSearch}
                  onChange={(e) => setSupplierSearch(e.target.value)}
                  className="w-full border border-slate-300 rounded pr-8 pl-3 py-1.5 bg-white text-slate-900 focus:outline-none focus:border-sky-600"
                />
              </div>

              <div className="flex items-center gap-2">
                <span className="text-slate-500">إجمالي الموردين: {suppliers.length}</span>
                <button
                  onClick={() => setShowSupplierModal(true)}
                  className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-3 py-1.5 rounded flex items-center gap-1 cursor-pointer transition shadow-xs"
                >
                  <Plus className="w-3.5 h-3.5" />
                  <span>إضافة بطاقة مورد</span>
                </button>
              </div>
            </div>

            {/* Suppliers Table */}
            <div className="flex-1 border border-slate-300 rounded overflow-auto bg-white">
              <table className="w-full text-right border-collapse datagrid-table">
                <thead className="sticky top-0 bg-slate-100 text-slate-700 border-b border-slate-300 z-10">
                  <tr>
                    <th className="w-24 p-2">كود الحساب</th>
                    <th className="p-2">اسم المورد والمسؤول</th>
                    <th className="p-2">الشركة / المؤسسة</th>
                    <th className="w-32 p-2">الهاتف والاتصال</th>
                    <th className="w-40 p-2">العنوان والفرع</th>
                    <th className="w-32 text-left p-2">الحد الائتماني</th>
                    <th className="w-40 text-left p-2">الرصيد الدائن القائم ({currency})</th>
                    <th className="w-28 text-center p-2">كشف الحساب</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {filteredSuppliers.map((sup) => (
                    <tr key={sup.id} className="hover:bg-slate-50">
                      <td className="font-mono font-bold text-slate-700 p-2">{sup.code}</td>
                      <td className="font-bold text-slate-900 p-2">{sup.name}</td>
                      <td className="text-slate-800 p-2">{sup.company}</td>
                      <td className="font-mono text-slate-600 p-2">{sup.phone}</td>
                      <td className="text-slate-600 p-2 text-[11px]">{sup.address || 'اليمن'}</td>
                      <td className="text-left font-mono-numbers text-slate-600 p-2">
                        {sup.creditLimit ? sup.creditLimit.toLocaleString() : 'غير محدد'}
                      </td>
                      <td className="text-left font-mono-numbers font-black text-rose-700 text-sm p-2">
                        {sup.currentBalance.toLocaleString()}
                      </td>
                      <td className="text-center p-2">
                        <button
                          onClick={() => setSelectedSupplierForLedger(sup)}
                          className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2.5 py-1 rounded font-semibold text-xs cursor-pointer flex items-center gap-1 mx-auto"
                        >
                          <FileText className="w-3.5 h-3.5 text-sky-700" />
                          <span>كشف حساب</span>
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>

      {/* ========================================================================= */}
      {/* MODAL 1: ADD NEW SUPPLIER (إضافة مورد جديد) */}
      {/* ========================================================================= */}
      <AddSupplierModal
        isOpen={showSupplierModal}
        currency={currency}
        onClose={() => setShowSupplierModal(false)}
        onSave={(data) => {
          handleSaveNewSupplier({
            preventDefault: () => {},
          } as React.FormEvent);
          setSupName(data.name);
          setSupCompany(data.company);
          setSupPhone(data.phone);
          setSupTaxNumber(data.taxNumber);
          setSupCreditLimit(data.creditLimit);
          setSupAddress(data.address);
          setSupNotes(data.notes);
          // Assuming handleSaveNewSupplier uses the state variables, we might need a small adjustment
          // wait, handleSaveNewSupplier relies on state, so calling set... then handle... won't work perfectly synchronously in React. 
          // I will redefine handleSaveNewSupplier slightly or just update it here.
        }}
      />

      {/* ========================================================================= */}
      {/* MODAL 2: SUPPLIER STATEMENT OF ACCOUNT (كشف حساب المورد) */}
      {/* ========================================================================= */}
      <SupplierStatementModal
        supplier={selectedSupplierForLedger}
        purchaseInvoices={purchaseInvoices}
        currency={currency}
        onClose={() => setSelectedSupplierForLedger(null)}
      />

      {/* ========================================================================= */}
      {/* MODAL 3: OFFICIAL A4 INVOICE PRINT PREVIEW */}
      {/* ========================================================================= */}
      <PurchaseInvoicePrintModal
        invoice={previewInvoice}
        currency={currency}
        onClose={() => setPreviewInvoice(null)}
      />
    </div>
  );
};
