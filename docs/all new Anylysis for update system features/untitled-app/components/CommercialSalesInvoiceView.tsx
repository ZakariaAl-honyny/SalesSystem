'use client';

import React, { useState, useMemo, useRef, useEffect, useCallback } from 'react';
import { 
  Invoice, 
  ProductItem, 
  Customer, 
  Warehouse, 
  CartItem, 
  PaymentType,
  UnitDefinition,
  MainTabType,
} from '@/types/pos';
import { getCurrentTimeString, generateSalesInvoiceNumber, generateSalesReturnNumber } from '@/lib/idGenerator';
import * as XLSX from 'xlsx';
import { 
  ArrowRight,
  ArrowLeft,
  Printer, 
  Save, 
  Camera,
  Plus, 
  X, 
  Trash2, 
  Image as ImageIcon,
  Check, 
  AlertCircle, 
  AlertTriangle,
  FileText, 
  ChevronDown, 
  Search, 
  Calendar, 
  Info, 
  Calculator, 
  ChevronLeft,
  MoreVertical,
  Layers,
  RotateCcw,
  Delete,
  FileSpreadsheet,
  Monitor,
  Smartphone,
  Barcode,
  Keyboard,
  UserCheck,
  Building,
  DollarSign,
  Percent,
  Coins,
  Receipt,
  FileCheck,
  PauseCircle,
  HelpCircle,
  Tag,
  TrendingUp,
  Users,
  CheckCircle2,
  ShoppingCart,
  Zap,
  Upload,
  LayoutGrid,
  ScanLine,
  Sparkles,
  Copy,
  Edit3,
  ArrowUp,
  ArrowDown,
} from 'lucide-react';
import { WinFormsDGVContextMenu, DGVContextMenuItem } from '@/components/WinFormsDGVContextMenu';

import {
  SalesDiscountExpensesModal,
  SalesCameraAttachmentModal,
  SalesCustomerPickerModal,
  SalesProfitBreakdownModal,
  SalesSmartF3LookupModal,
  SalesImportConvertModal,
  SalesCurrencyModal,
  FrmSalesInvoice_TouchGrid,
  ProductSelectionModal,
} from './sales';

// Expense Accounts for Other Charges Dropdown (Standard Chart of Accounts)
const EXPENSE_ACCOUNTS_OPTIONS = [
  { code: '424', name: 'أجور نقل ومواصلات' },
  { code: '425', name: 'شحن وتوصيل طلبات' },
  { code: '426', name: 'عتالة وتحميل وتفريغ' },
  { code: '423', name: 'صيانة وضيافة ومصاريف معرض' },
  { code: '421', name: 'إيجار وكهرباء ومياه' },
  { code: '422', name: 'رواتب وأجور عمال إضافية' },
  { code: '429', name: 'مصروفات تشغيلية ونثرية أخرى' },
];

interface CommercialSalesInvoiceViewProps {
  products: ProductItem[];
  customers: Customer[];
  warehouses: Warehouse[];
  currency: string;
  cashierName?: string;
  onSaveInvoice: (invoice: Invoice, isDraft?: boolean) => void;
  onRequestPrintReceipt?: (invoice: Invoice) => void;
  onBackToDashboard?: () => void;
  invoicesHistory?: Invoice[];
  onUpdateProductPrice?: (productId: string, newSalePrice: number) => void;
  initialCustomerName?: string;
  initialNotes?: string;
  initialItems?: Array<{
    product: ProductItem;
    quantity: number;
    unitPrice: number;
    discount?: number;
    unit?: string;
    unitFactor?: number;
    expiryDate?: string;
    notes?: string;
  }>;
  defaultWarehouseId?: string;
  defaultCashboxCode?: string;
  onSelectTab?: (tab: MainTabType) => void;
}

export const CommercialSalesInvoiceView: React.FC<CommercialSalesInvoiceViewProps> = ({
  products,
  customers,
  warehouses,
  currency,
  cashierName = 'المحاسب الرئيسي',
  onSaveInvoice,
  onRequestPrintReceipt,
  onBackToDashboard,
  invoicesHistory = [],
  onUpdateProductPrice,
  initialCustomerName,
  initialNotes,
  initialItems,
  defaultWarehouseId,
  defaultCashboxCode,
  onSelectTab,
}) => {
  // Mode: DESKTOP (Enterprise Fast Keyboard Grid) vs TOUCH_MOBILE (Matching Mobile Screenshots)
  const [viewMode, setViewMode] = useState<'DESKTOP' | 'TOUCH_MOBILE'>('DESKTOP');

  // Top Form State
  const [isReturn, setIsReturn] = useState(false);
  const [selectedWarehouseId, setSelectedWarehouseId] = useState(defaultWarehouseId || warehouses[0]?.id || 'WH-01');
  const [isCash, setIsCash] = useState(false); // Legacy toggle
  
  // New Payment Engine State
  const [paymentType, setPaymentType] = useState<PaymentType>('CREDIT');
  const [splitCash, setSplitCash] = useState(0);
  const [splitNetwork, setSplitNetwork] = useState(0);
  const [splitCredit, setSplitCredit] = useState(0);
  const [selectedCashAccountCode, setSelectedCashAccountCode] = useState<string>(defaultCashboxCode || '1111');
  const [selectedBankAccountCode, setSelectedBankAccountCode] = useState<string>('1121');

  const [selectedCurrency, setSelectedCurrency] = useState('دولار');
  const [showCurrencyModal, setShowCurrencyModal] = useState(false);
  const [selectedCashBox, setSelectedCashBox] = useState('الصندوق الرئيسي');

  // Customer & Date
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>(() => {
    if (initialCustomerName) {
      const found = customers.find(c => c.name.trim() === initialCustomerName.trim());
      if (found) return found.id;
    }
    return customers[0]?.id || 'CUST-01';
  });
  const [customerName, setCustomerName] = useState<string>(() => initialCustomerName || customers[0]?.name || '');
  const [customerRequiredError, setCustomerRequiredError] = useState(false);
  const [invoiceDate, setInvoiceDate] = useState('2026-06-04');
  const [dueDate, setDueDate] = useState('2026-07-04');
  const [invoiceNo, setInvoiceNo] = useState<string>(() => generateSalesInvoiceNumber());
  const [referenceNo, setReferenceNo] = useState('');
  const [notes, setNotes] = useState<string>(() => initialNotes || '');

  // Selected Customer details
  const activeCustomer = useMemo(() => {
    return customers.find(c => c.id === selectedCustomerId || c.name === customerName) || customers[0];
  }, [customers, selectedCustomerId, customerName]);

  const activeWarehouseName = useMemo(() => {
    return warehouses.find(w => w.id === selectedWarehouseId)?.name || 'المستودع الرئيسي';
  }, [selectedWarehouseId, warehouses]);

  // Camera & Attachment Modal
  const [showCameraModal, setShowCameraModal] = useState(false);
  const [attachedImageName, setAttachedImageName] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const cameraInputRef = useRef<HTMLInputElement>(null);

  // Rapid Barcode & Item Search
  const [barcodeInput, setBarcodeInput] = useState('');
  const [itemSearchText, setItemSearchText] = useState('');
  const [showSuggestions, setShowSuggestions] = useState(false);
  const barcodeInputRef = useRef<HTMLInputElement>(null);
  const searchInputRef = useRef<HTMLInputElement>(null);

  // Invoice Items state
  const [invoiceItems, setInvoiceItems] = useState<Array<{
    id: string;
    product: ProductItem;
    quantity: number;
    unitPrice: number;
    unitCost: number;
    discount: number;
    total: number;
    unit: string;
    unitFactor: number;
    expiryDate?: string;
    notes?: string;
  }>>(() => {
    if (initialItems && initialItems.length > 0) {
      return initialItems.map((it, idx) => ({
        id: `CONV-${idx}-${Date.now()}`,
        product: it.product,
        quantity: it.quantity,
        unitPrice: it.unitPrice,
        unitCost: it.product.costPrice,
        discount: it.discount || 0,
        total: Math.max(0, (it.quantity * it.unitPrice) - (it.discount || 0)),
        unit: it.unit || it.product.unit || 'حبة',
        unitFactor: it.unitFactor || 1,
        expiryDate: it.expiryDate || it.product.expiryDate || '2028-12-31',
        notes: it.notes || '',
      }));
    }
    return [
      {
        id: 'ITEM-1',
        product: products.find(p => p.name.includes('بطاطس')) || products[0] || {
          id: 'P-0999',
          code: '0999',
          barcode: '629100099999',
          name: 'بطاطس عمان',
          category: 'مواد غذائية',
          unit: 'حبة',
          units: [
            { name: 'حبة', factor: 1, price: 0.07143, cost: 0.05, barcode: '629100099999' },
            { name: 'باكت', factor: 12, price: 0.80, cost: 0.60, barcode: '629100099998' },
            { name: 'كرتون', factor: 48, price: 3.10, cost: 2.35, barcode: '629100099997' },
          ],
          costPrice: 0.05,
          salePrice: 0.07143,
          stock: 150,
          minStock: 20,
        },
        quantity: 10,
        unitPrice: 0.07143,
        unitCost: 0.05,
        discount: 0,
        total: 0.7143,
        unit: 'حبة',
        unitFactor: 1,
        expiryDate: '2026-12-31',
        notes: '',
      },
      {
        id: 'ITEM-2',
        product: products[1] || {
          id: 'P-0998',
          code: '0998',
          barcode: '629100088888',
          name: 'عصير برتقال فلوريدا 1 لتر',
          category: 'مشروبات',
          unit: 'حبة',
          units: [
            { name: 'حبة', factor: 1, price: 1.25, cost: 0.95, barcode: '629100088888' },
            { name: 'كرتون', factor: 12, price: 14.00, cost: 11.00, barcode: '629100088887' },
          ],
          costPrice: 0.95,
          salePrice: 1.25,
          stock: 80,
          minStock: 10,
        },
        quantity: 2,
        unitPrice: 1.25,
        unitCost: 0.95,
        discount: 0,
        total: 2.50,
        unit: 'حبة',
        unitFactor: 1,
        expiryDate: '2027-03-15',
        notes: '',
      }
    ];
  });

  // Header Financial Adjustments
  const [headerDiscountType, setHeaderDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [headerDiscountVal, setHeaderDiscountVal] = useState<string>('0');
  const [taxPercentVal, setTaxPercentVal] = useState<string>('5');
  const [isTaxEnabled, setIsTaxEnabled] = useState<boolean>(false);
  const [otherChargesVal, setOtherChargesVal] = useState<string>('0');
  const [otherChargesAccountCode, setOtherChargesAccountCode] = useState<string>('424');
  const [otherChargesName, setOtherChargesName] = useState<string>('أجور نقل ومواصلات');
  const [otherChargesDesc, setOtherChargesDesc] = useState<string>('أجور نقل وتحميل');
  const [paidAmountVal, setPaidAmountVal] = useState<string>('0');

  // Touch Modals State
  const [showItemModal, setShowItemModal] = useState(false);
  const [itemModalMode, setItemModalMode] = useState<'ADD' | 'EDIT'>('ADD');
  const [itemModalIndex, setItemModalIndex] = useState<number | null>(null);
  const [modalProduct, setModalProduct] = useState<ProductItem | null>(null);
  const [modalQty, setModalQty] = useState<string>('10');
  const [modalPrice, setModalPrice] = useState<string>('0.07143');
  const [modalTotal, setModalTotal] = useState<string>('0.7143');
  const [modalUnit, setModalUnit] = useState<string>('حبة');
  const [modalActiveField, setModalActiveField] = useState<'QTY' | 'PRICE' | 'TOTAL'>('QTY');
  const [modalExpiry, setModalExpiry] = useState<string>('2026-12-31');
  const [showDetailsSection, setShowDetailsSection] = useState(false);
  const [modalUpdateMasterPrice, setModalUpdateMasterPrice] = useState(false);

  // =========================================================================
  // 6-INPUT MODES ENGINE (مبدل ومختبر طرق إدخال الفاتورة الـ 6)
  // =========================================================================
  const [activeInputMode, setActiveInputMode] = useState<
    'INLINE_GRID' | 'RAPID_ADD_BAR' | 'CONTINUOUS_SCANNER' | 'TOUCH_GRID' | 'F3_LOOKUP' | 'IMPORT_CONVERT'
  >('RAPID_ADD_BAR');

  // Mode 2: Rapid Add-Bar State
  const [rapidBarProductId, setRapidBarProductId] = useState<string>(products[0]?.id || '');
  const [rapidBarQty, setRapidBarQty] = useState<number>(1);
  const [rapidBarUnit, setRapidBarUnit] = useState<string>('حبة');
  const [rapidBarPrice, setRapidBarPrice] = useState<number>(products[0]?.salePrice || 1);
  const [rapidBarDiscount, setRapidBarDiscount] = useState<number>(0);
  const [rapidBarNotes, setRapidBarNotes] = useState<string>('');

  // Mode 3: Continuous Scanner State
  const [continuousScanMode, setContinuousScanMode] = useState<boolean>(true);
  const [lastScannedFeedback, setLastScannedFeedback] = useState<{ name: string; time: string; qty: number } | null>(null);
  const [scannerFlash, setScannerFlash] = useState<boolean>(false);

  // Mode 4: Visual Touch Grid State
  const [touchCategory, setTouchCategory] = useState<string>('الكل');

  // Mode 5: F3 Smart Lookup Modal
  const [showF3Modal, setShowF3Modal] = useState<boolean>(false);

  // Mode 6: Import & Convert Modal
  const [showImportModal, setShowImportModal] = useState<boolean>(false);

  const [showDiscountModal, setShowDiscountModal] = useState(false);
  const [activeDiscountField, setActiveDiscountField] = useState<'DISCOUNT' | 'TAX' | 'OTHER' | 'PAID'>('PAID');
  const [showAccountingEntryPreview, setShowAccountingEntryPreview] = useState(false);
  const [showProfitModal, setShowProfitModal] = useState(false);
  const [showCustomerPickerModal, setShowCustomerPickerModal] = useState(false);

  // Alerts
  const [alertMsg, setAlertMsg] = useState<{ type: 'success' | 'error' | 'warning'; text: string } | null>(null);

  // Helper to get unit price and factor - Unified Single Selling Price
  const getUnitInfo = useCallback((product: ProductItem, unitName: string) => {
    const unitDef = product.units?.find(u => u.name === unitName);
    if (unitDef) {
      const price = unitDef.price || (product.salePrice * (unitDef.factor || 1));
      return {
        factor: unitDef.factor || 1,
        price: Number(price.toFixed(4)),
        cost: unitDef.cost || ((product.costPrice || 0) * (unitDef.factor || 1)),
      };
    }
    return {
      factor: 1,
      price: product.salePrice || 0,
      cost: product.costPrice || 0,
    };
  }, []);

  // Direct Update of Master Selling Price for a Product
  const handleUpdateMasterPrice = (productId: string, newPrice: number, productName: string) => {
    if (onUpdateProductPrice) {
      onUpdateProductPrice(productId, newPrice);
      // Sync local in-grid items so updated product price matches
      setInvoiceItems(prev =>
        prev.map(it =>
          it.product.id === productId
            ? { ...it, product: { ...it.product, salePrice: newPrice } }
            : it
        )
      );
      setAlertMsg({
        type: 'success',
        text: `تم تحديث وتثبيت سعر بيع [${productName}] في بطاقة الصنف إلى ${newPrice.toLocaleString()} ${selectedCurrency} بنجاح`,
      });
      setTimeout(() => setAlertMsg(null), 3500);
    }
  };

  // Calculations
  const rawSubtotal = useMemo(() => {
    return invoiceItems.reduce((acc, item) => acc + item.total, 0);
  }, [invoiceItems]);

  const itemsTotalDiscount = useMemo(() => {
    return invoiceItems.reduce((acc, item) => acc + (item.discount || 0), 0);
  }, [invoiceItems]);

  const commercialDiscount = useMemo(() => {
    const d = parseFloat(headerDiscountVal) || 0;
    if (headerDiscountType === 'PERCENT') {
      return (rawSubtotal * Math.min(100, Math.max(0, d))) / 100;
    }
    return Math.min(rawSubtotal, Math.max(0, d));
  }, [rawSubtotal, headerDiscountType, headerDiscountVal]);

  const taxableAmount = Math.max(0, rawSubtotal - commercialDiscount);

  const calculatedTax = useMemo(() => {
    if (!isTaxEnabled) return 0;
    const t = parseFloat(taxPercentVal) || 0;
    return (taxableAmount * t) / 100;
  }, [isTaxEnabled, taxableAmount, taxPercentVal]);

  const otherCharges = parseFloat(otherChargesVal) || 0;

  const grandTotal = useMemo(() => {
    return Math.max(0, taxableAmount + calculatedTax + otherCharges);
  }, [taxableAmount, calculatedTax, otherCharges]);

  const calcPaidAmount = useMemo(() => {
    if (paymentType === 'CASH' || paymentType === 'KURAYMI' || paymentType === 'BANK_TRANSFER' || paymentType === 'JEB_WALLET') {
      return grandTotal;
    }
    if (paymentType === 'MIXED') {
      return Math.min(grandTotal, splitCash + splitNetwork);
    }
    const p = parseFloat(paidAmountVal) || 0;
    return Math.min(grandTotal, Math.max(0, p));
  }, [paymentType, grandTotal, splitCash, splitNetwork, paidAmountVal]);

  const remainingAmount = useMemo(() => {
    return Math.max(0, grandTotal - calcPaidAmount);
  }, [grandTotal, calcPaidAmount]);

  // Invoice Profit & Cost Breakdown
  const totalCost = useMemo(() => {
    return invoiceItems.reduce((acc, item) => {
      const unitFactor = item.unitFactor || 1;
      const baseCost = item.unitCost || (item.product.costPrice || 0) * unitFactor;
      return acc + (baseCost * item.quantity);
    }, 0);
  }, [invoiceItems]);

  const invoiceNetProfit = useMemo(() => {
    return (rawSubtotal - commercialDiscount) - totalCost;
  }, [rawSubtotal, commercialDiscount, totalCost]);

  const invoiceProfitMargin = useMemo(() => {
    const netRev = rawSubtotal - commercialDiscount;
    if (netRev <= 0) return 0;
    return (invoiceNetProfit / netRev) * 100;
  }, [rawSubtotal, commercialDiscount, invoiceNetProfit]);

  // Add Product to In-grid Table
  const addProductToInvoice = useCallback((
    product: ProductItem, 
    unitName?: string, 
    qty: number = 1,
    overridePrice?: number,
    overrideDiscount?: number,
    overrideNotes?: string,
    overrideExpiry?: string,
    overrideBatch?: string
  ) => {
    const selectedU = unitName || product.unit || 'حبة';
    const unitInfo = getUnitInfo(product, selectedU);
    const finalPrice = overridePrice !== undefined ? overridePrice : unitInfo.price;
    const finalDiscount = overrideDiscount !== undefined ? overrideDiscount : 0;

    setInvoiceItems(prev => {
      const existingIdx = prev.findIndex(item => item.product.id === product.id && item.unit === selectedU && (!overridePrice || item.unitPrice === overridePrice));
      if (existingIdx > -1 && overrideDiscount === undefined) {
        const updated = [...prev];
        const current = updated[existingIdx];
        const newQty = current.quantity + qty;
        const newTotal = Math.max(0, (newQty * current.unitPrice) - current.discount);
        updated[existingIdx] = {
          ...current,
          quantity: newQty,
          total: Number(newTotal.toFixed(4)),
        };
        return updated;
      }

      const total = Math.max(0, (qty * finalPrice) - finalDiscount);
      const newItem = {
        id: `ITEM-${Date.now()}-${Math.random().toString(36).substr(2, 4)}`,
        product,
        quantity: qty,
        unitPrice: finalPrice,
        unitCost: unitInfo.cost,
        discount: finalDiscount,
        total: Number(total.toFixed(4)),
        unit: selectedU,
        unitFactor: unitInfo.factor,
        expiryDate: overrideExpiry || product.expiryDate || '2027-12-31',
        batchNo: overrideBatch || product.batchNo,
        notes: overrideNotes || '',
      };
      return [...prev, newItem];
    });

    setBarcodeInput('');
    setItemSearchText('');
    setShowSuggestions(false);
  }, [getUnitInfo]);

  // Barcode rapid scan handler
  const handleBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!barcodeInput.trim()) return;
    const code = barcodeInput.trim().toLowerCase();

    // Check by unit barcode, product barcode, or product code
    let foundProd: ProductItem | undefined;
    let foundUnitName: string | undefined;

    for (const p of products) {
      if (p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code) {
        foundProd = p;
        foundUnitName = p.unit;
        break;
      }
      if (p.units) {
        const u = p.units.find(u => u.barcode?.toLowerCase() === code);
        if (u) {
          foundProd = p;
          foundUnitName = u.name;
          break;
        }
      }
    }

    if (foundProd) {
      addProductToInvoice(foundProd, foundUnitName, 1);
      setAlertMsg({ type: 'success', text: `تم مسح الصنف: ${foundProd.name} (${foundUnitName})` });
    } else {
      setAlertMsg({ type: 'error', text: `لم يتم العثور على صنف بالباركود / الكود: "${barcodeInput}"` });
    }
    setBarcodeInput('');
  };

  // Mode 2: Handle Rapid Add-Bar Submit
  const handleRapidBarSubmit = (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    const prod = products.find(p => p.id === rapidBarProductId);
    if (!prod) return;
    const unitInfo = getUnitInfo(prod, rapidBarUnit);
    const itemTotal = Math.max(0, (rapidBarQty * rapidBarPrice) - rapidBarDiscount);

    setInvoiceItems(prev => {
      const existingIdx = prev.findIndex(item => item.product.id === prod.id && item.unit === rapidBarUnit);
      if (existingIdx > -1) {
        const updated = [...prev];
        const current = updated[existingIdx];
        const newQty = current.quantity + rapidBarQty;
        const newTotal = Math.max(0, (newQty * current.unitPrice) - current.discount);
        updated[existingIdx] = {
          ...current,
          quantity: newQty,
          total: Number(newTotal.toFixed(4)),
        };
        return updated;
      }
      return [
        ...prev,
        {
          id: `ITEM-${Date.now()}`,
          product: prod,
          quantity: rapidBarQty,
          unitPrice: rapidBarPrice,
          unitCost: unitInfo.cost,
          discount: rapidBarDiscount,
          total: Number(itemTotal.toFixed(4)),
          unit: rapidBarUnit,
          unitFactor: unitInfo.factor,
          expiryDate: prod.expiryDate || '2028-12-31',
          notes: rapidBarNotes,
        }
      ];
    });

    setAlertMsg({ type: 'success', text: `تم إدراج الصنف [${prod.name}] بالكمية (${rapidBarQty} ${rapidBarUnit}) عبر شريط الإدخال السريع` });
    setRapidBarNotes('');
  };

  // Mode 3: Continuous Instant Barcode Scanner
  const handleContinuousScan = (scannedCode: string) => {
    const code = scannedCode.trim().toLowerCase();
    if (!code) return;

    let foundProd: ProductItem | undefined;
    let foundUnitName: string | undefined;

    for (const p of products) {
      if (p.barcode?.toLowerCase() === code || p.code?.toLowerCase() === code) {
        foundProd = p;
        foundUnitName = p.unit;
        break;
      }
      if (p.units) {
        const u = p.units.find(u => u.barcode?.toLowerCase() === code);
        if (u) {
          foundProd = p;
          foundUnitName = u.name;
          break;
        }
      }
    }

    if (foundProd) {
      addProductToInvoice(foundProd, foundUnitName, 1);
      setScannerFlash(true);
      setTimeout(() => setScannerFlash(false), 400);

      const nowTime = new Date().toLocaleTimeString('ar-YE');
      setLastScannedFeedback({
        name: foundProd.name,
        time: nowTime,
        qty: (invoiceItems.find(it => it.product.id === foundProd?.id)?.quantity || 0) + 1,
      });

      setAlertMsg({
        type: 'success',
        text: `⚡ مسح باركود آلي ناجح: [${foundProd.name}] (+1) — الرصيد: ${foundProd.stock}`,
      });
    } else {
      setAlertMsg({ type: 'error', text: `لم يتم العثور على صنف بالباركود: "${scannedCode}"` });
    }
  };

  // Barcode scan handler helper for rapid bar
  const handleBarcodeScan = (code: string) => {
    handleContinuousScan(code);
    setBarcodeInput('');
    setItemSearchText('');
    setShowSuggestions(false);
  };

  // Mode 6: Sample Quotation & Past Invoice Importers
  const handleImportQuotation = (quotationTitle: string, sampleItems: any[]) => {
    setInvoiceItems(sampleItems.map((item, idx) => {
      const prod = products.find(p => p.id === item.productId) || products[idx % products.length];
      const unitInfo = getUnitInfo(prod, item.unit || prod.unit || 'حبة');
      return {
        id: `IMPORT-${idx}-${Date.now()}`,
        product: prod,
        quantity: item.qty,
        unitPrice: item.price || unitInfo.price,
        unitCost: unitInfo.cost,
        discount: item.discount || 0,
        total: Math.max(0, (item.qty * (item.price || unitInfo.price)) - (item.discount || 0)),
        unit: item.unit || prod.unit || 'حبة',
        unitFactor: unitInfo.factor,
        expiryDate: prod.expiryDate || '2028-12-31',
        notes: `مستورد من ${quotationTitle}`,
      };
    }));
    setShowImportModal(false);
    setAlertMsg({ type: 'success', text: `تم استيراد ${sampleItems.length} أصناف بنجاح من [${quotationTitle}] إلى الفاتورة!` });
  };

  // Grid Cell Inline Editing Handlers
  const handleGridChange = (
    index: number,
    field: 'quantity' | 'unitPrice' | 'discount' | 'unit' | 'notes' | 'expiryDate',
    value: any
  ) => {
    setInvoiceItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'unit') {
        item.unit = value;
        const info = getUnitInfo(item.product, value);
        item.unitPrice = info.price;
        item.unitCost = info.cost;
        item.unitFactor = info.factor;
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
      } else if (field === 'notes') {
        item.notes = value;
      } else if (field === 'expiryDate') {
        item.expiryDate = value;
      }

      item.total = Number(item.total.toFixed(4));
      updated[index] = item;
      return updated;
    });
  };

  const handleDeleteRow = (index: number) => {
    setInvoiceItems(prev => prev.filter((_, i) => i !== index));
  };

  // DGV Context Menu State for Commercial Sales Grid
  const [dgvMenuState, setDgvMenuState] = useState<{
    visible: boolean;
    x: number;
    y: number;
    itemIndex: number;
    itemTitle: string;
  }>({ visible: false, x: 0, y: 0, itemIndex: -1, itemTitle: '' });

  const handleRowContextMenu = (e: React.MouseEvent, item: any, index: number) => {
    e.preventDefault();
    setDgvMenuState({
      visible: true,
      x: e.clientX,
      y: e.clientY,
      itemIndex: index,
      itemTitle: item.product.name,
    });
  };

  const dgvContextMenuItems: DGVContextMenuItem[] = [
    {
      id: 'edit-qty',
      label: 'تعديل كمية السطر',
      icon: <Edit3 className="w-3.5 h-3.5 text-sky-600" />,
      shortcut: 'Enter',
      action: () => {
        const currentQty = invoiceItems[dgvMenuState.itemIndex]?.quantity || 1;
        const newQty = prompt(`تعديل الكمية للصنف [${dgvMenuState.itemTitle}]:`, String(currentQty));
        if (newQty !== null && !isNaN(parseFloat(newQty))) {
          handleGridChange(dgvMenuState.itemIndex, 'quantity', parseFloat(newQty));
        }
      },
    },
    {
      id: 'edit-price',
      label: 'تعديل سعر الوحدة',
      icon: <DollarSign className="w-3.5 h-3.5 text-emerald-600" />,
      action: () => {
        const currentPrice = invoiceItems[dgvMenuState.itemIndex]?.unitPrice || 0;
        const newPrice = prompt(`تعديل سعر الوحدة للصنف [${dgvMenuState.itemTitle}]:`, String(currentPrice));
        if (newPrice !== null && !isNaN(parseFloat(newPrice))) {
          handleGridChange(dgvMenuState.itemIndex, 'unitPrice', parseFloat(newPrice));
        }
      },
    },
    {
      id: 'apply-discount',
      label: 'تطبيق خصم مباشر على السطر',
      icon: <Tag className="w-3.5 h-3.5 text-amber-600" />,
      action: () => {
        const currentDisc = invoiceItems[dgvMenuState.itemIndex]?.discount || 0;
        const newDisc = prompt(`إدخال خصم السطر الصافي للصنف [${dgvMenuState.itemTitle}]:`, String(currentDisc));
        if (newDisc !== null && !isNaN(parseFloat(newDisc))) {
          handleGridChange(dgvMenuState.itemIndex, 'discount', parseFloat(newDisc));
        }
      },
    },
    {
      id: 'copy-code',
      label: 'نسخ بيانات الصنف',
      icon: <Copy className="w-3.5 h-3.5 text-slate-600" />,
      shortcut: 'Ctrl+C',
      action: () => {
        const it = invoiceItems[dgvMenuState.itemIndex];
        if (it) {
          navigator.clipboard?.writeText(`${it.product.code} - ${it.product.name}`);
          setAlertMsg({ type: 'success', text: `تم نسخ بيانات الصنف [${it.product.name}] بالحافظة.` });
        }
      },
    },
    {
      id: 'move-up',
      label: 'تحريك السطر للأعلى',
      icon: <ArrowUp className="w-3.5 h-3.5 text-slate-600" />,
      disabled: dgvMenuState.itemIndex === 0,
      action: () => {
        if (dgvMenuState.itemIndex > 0) {
          setInvoiceItems(prev => {
            const arr = [...prev];
            const temp = arr[dgvMenuState.itemIndex];
            arr[dgvMenuState.itemIndex] = arr[dgvMenuState.itemIndex - 1];
            arr[dgvMenuState.itemIndex - 1] = temp;
            return arr;
          });
        }
      },
    },
    {
      id: 'move-down',
      label: 'تحريك السطر للأسفل',
      icon: <ArrowDown className="w-3.5 h-3.5 text-slate-600" />,
      disabled: dgvMenuState.itemIndex === invoiceItems.length - 1,
      action: () => {
        if (dgvMenuState.itemIndex < invoiceItems.length - 1) {
          setInvoiceItems(prev => {
            const arr = [...prev];
            const temp = arr[dgvMenuState.itemIndex];
            arr[dgvMenuState.itemIndex] = arr[dgvMenuState.itemIndex + 1];
            arr[dgvMenuState.itemIndex + 1] = temp;
            return arr;
          });
        }
      },
    },
    {
      id: 'delete-row',
      label: 'حذف السطر المختار',
      icon: <Trash2 className="w-3.5 h-3.5 text-rose-600" />,
      shortcut: 'Delete',
      danger: true,
      action: () => {
        handleDeleteRow(dgvMenuState.itemIndex);
      },
    },
  ];

  // Save Invoice logic with validation
  const handleSaveInvoice = useCallback((isDraft: boolean = false) => {
    if (!isCash && !customerName.trim()) {
      setCustomerRequiredError(true);
      setAlertMsg({ type: 'error', text: 'يرجى تحديد العميل للفاتورة الآجلة' });
      return;
    }
    setCustomerRequiredError(false);

    if (invoiceItems.length === 0) {
      setAlertMsg({ type: 'error', text: 'لا يمكن حفظ فاتورة فارغة. يرجى إضافة أصناف.' });
      return;
    }

    // Calculate total net line amounts before header discount for proportional allocation
    const lineNetSubtotals = invoiceItems.map(item => Math.max(0, (item.quantity * item.unitPrice) - item.discount));
    const sumLineNets = lineNetSubtotals.reduce((a, b) => a + b, 0);

    const cartItems: CartItem[] = invoiceItems.map((item, idx) => {
      const lineNet = lineNetSubtotals[idx];
      const ratio = sumLineNets > 0 ? lineNet / sumLineNets : 1 / (invoiceItems.length || 1);
      const allocatedCommercialDisc = commercialDiscount * ratio;
      const totalLineDisc = item.discount + allocatedCommercialDisc;
      const allocatedTaxAmount = calculatedTax * ratio;
      const netUnitPriceVal = item.quantity > 0 ? Math.max(0, (lineNet - allocatedCommercialDisc) / item.quantity) : 0;

      return {
        item: item.product,
        quantity: item.quantity,
        selectedUnit: item.unit,
        unitPrice: item.unitPrice,
        unitCost: item.unitCost,
        discount: item.discount,
        allocatedDiscount: Number(totalLineDisc.toFixed(4)),
        allocatedTax: Number(allocatedTaxAmount.toFixed(4)),
        netUnitPrice: Number(netUnitPriceVal.toFixed(4)),
        taxRate: isTaxEnabled ? parseFloat(taxPercentVal) || 0 : 0,
        total: item.total,
        unitFactor: item.unitFactor,
        expiryDate: item.expiryDate,
        notes: item.notes,
      };
    });

    const newInvoice: Invoice = {
      id: `INV-${Date.now()}`,
      invoiceNo: invoiceNo,
      type: isReturn ? 'SALE_RETURN' : 'SALE',
      date: invoiceDate,
      time: getCurrentTimeString(),
      customerOrSupplierId: selectedCustomerId,
      customerOrSupplierName: customerName.trim() || (isCash ? 'عميل نقدي عام' : 'عميل آجل'),
      items: cartItems,
      subtotal: rawSubtotal,
      discountTotal: itemsTotalDiscount + commercialDiscount,
      taxTotal: calculatedTax,
      otherCharges: otherCharges > 0 ? otherCharges : undefined,
      otherChargesName: otherCharges > 0 ? otherChargesName : undefined,
      otherChargesAccountCode: otherCharges > 0 ? otherChargesAccountCode : undefined,
      otherChargesAccountName: otherCharges > 0 ? (EXPENSE_ACCOUNTS_OPTIONS.find(a => a.code === otherChargesAccountCode)?.name || otherChargesName) : undefined,
      grandTotal: grandTotal,
      paidAmount: calcPaidAmount,
      remainingAmount: remainingAmount,
      paymentType: isCash ? 'CASH' : remainingAmount > 0 ? (calcPaidAmount > 0 ? 'MIXED' : 'CREDIT') : 'CASH',
      cashierName: cashierName,
      notes: notes || (isReturn ? `مردود مبيعات تجارية للعميل: ${customerName}` : undefined),
      warehouseId: selectedWarehouseId,
      warehouseName: warehouses.find(w => w.id === selectedWarehouseId)?.name || 'المخزن الرئيسي',
      status: isDraft ? 'DRAFT' : 'POSTED',
      currency: selectedCurrency,
    };

    onSaveInvoice(newInvoice, isDraft);
    setAlertMsg({
      type: 'success',
      text: isDraft 
        ? `تم حفظ ${isReturn ? 'مردود المبيعات' : 'الفاتورة'} كمسودة بنجاح (${invoiceNo})` 
        : `تم اعتماد وترحيل ${isReturn ? 'مردود المبيعات' : 'الفاتورة'} رقم ${invoiceNo} وتوليد القيود المحاسبية بنجاح!`,
    });

    if (!isDraft && onRequestPrintReceipt) {
      onRequestPrintReceipt(newInvoice);
    }
  }, [
    isReturn,
    isCash,
    customerName,
    invoiceItems,
    isTaxEnabled,
    taxPercentVal,
    invoiceNo,
    invoiceDate,
    selectedCustomerId,
    rawSubtotal,
    itemsTotalDiscount,
    commercialDiscount,
    calculatedTax,
    otherCharges,
    otherChargesAccountCode,
    otherChargesName,
    grandTotal,
    calcPaidAmount,
    remainingAmount,
    cashierName,
    notes,
    selectedWarehouseId,
    warehouses,
    selectedCurrency,
    onSaveInvoice,
    onRequestPrintReceipt,
  ]);

  // Export current invoice to XLSX
  const handleExportInvoiceXLSX = useCallback(() => {
    if (invoiceItems.length === 0) {
      setAlertMsg({ type: 'error', text: 'لا يمكن تصدير فاتورة فارغة. يرجى إضافة أصناف أولاً.' });
      return;
    }

    const headers = [
      'م',
      'إسم الصنف',
      'الباركود',
      'الوحدة',
      'الكمية',
      'سعر الوحدة',
      'الخصم المباشر',
      'إجمالي السطر',
      'تاريخ الانتهاء',
      'ملاحظات'
    ];

    const rows = invoiceItems.map((item, index) => [
      index + 1,
      item.product.name,
      item.product.barcode || '',
      item.unit,
      item.quantity,
      item.unitPrice,
      item.discount,
      item.total,
      item.expiryDate || '',
      item.notes || ''
    ]);

    const summaryRows = [
      [],
      ['بيانات الفاتورة:', `رقم الفاتورة: #${invoiceNo}`, `التاريخ: ${invoiceDate}`, `العميل: ${customerName || 'عميل نقدي'}`],
      ['المجموع الفرعي:', rawSubtotal],
      ['إجمالي الخصوم:', itemsTotalDiscount + commercialDiscount],
      ['إجمالي الضريبة:', calculatedTax],
      ['المبلغ الصافي المستحق:', grandTotal],
      ['المدفوع:', calcPaidAmount],
      ['المتبقي:', remainingAmount]
    ];

    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.aoa_to_sheet([headers, ...rows, ...summaryRows]);
    ws['!cols'] = [
      { wch: 6 },
      { wch: 32 },
      { wch: 18 },
      { wch: 12 },
      { wch: 10 },
      { wch: 14 },
      { wch: 12 },
      { wch: 16 },
      { wch: 14 },
      { wch: 20 },
    ];
    ws['!views'] = [{ RTL: true }];
    (ws as any)['!sheetViews'] = [{ RTL: true }];

    XLSX.utils.book_append_sheet(wb, ws, `فاتورة_${invoiceNo}`);
    const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `فاتورة_${invoiceNo}_${invoiceDate}.xlsx`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
    setAlertMsg({ type: 'success', text: `تم تصدير الفاتورة #${invoiceNo} إلى ملف Excel (.xlsx) بنجاح!` });
  }, [invoiceItems, invoiceNo, invoiceDate, customerName, rawSubtotal, itemsTotalDiscount, commercialDiscount, calculatedTax, grandTotal, calcPaidAmount, remainingAmount]);

  // Global Keyboard Shortcuts (F1, F2, F3, F9, F10, F12, Insert, Delete)
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      // F1: Focus Barcode / Search
      if (e.key === 'F1') {
        e.preventDefault();
        barcodeInputRef.current?.focus();
      }
      // F2: Customer search
      if (e.key === 'F2') {
        e.preventDefault();
        searchInputRef.current?.focus();
      }
      // F3: Toggle Cash / Credit
      if (e.key === 'F3') {
        e.preventDefault();
        setIsCash(prev => !prev);
      }
      // F9: Save & Post
      if (e.key === 'F9') {
        e.preventDefault();
        handleSaveInvoice(false);
      }
      // F10: Print
      if (e.key === 'F10') {
        e.preventDefault();
        window.print();
      }
      // F12: Draft / Hold
      if (e.key === 'F12') {
        e.preventDefault();
        handleSaveInvoice(true);
      }
      // Insert: Add first suggested or open product picker
      if (e.key === 'Insert') {
        e.preventDefault();
        if (products.length > 0) {
          addProductToInvoice(products[0]);
        }
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isCash, invoiceItems, customerName, grandTotal, products, handleSaveInvoice, addProductToInvoice]);

  // Filtered products for search autocomplete
  const searchMatches = useMemo(() => {
    if (!itemSearchText.trim()) return [];
    const q = itemSearchText.toLowerCase();
    return products.filter(p => 
      p.name.toLowerCase().includes(q) || 
      p.code.toLowerCase().includes(q) || 
      p.barcode?.includes(q)
    ).slice(0, 8);
  }, [products, itemSearchText]);

  // Touch Keypad handler for Item Modal
  const handleItemKeypadPress = (val: string) => {
    let current = modalActiveField === 'QTY' ? modalQty : modalActiveField === 'PRICE' ? modalPrice : modalTotal;
    if (val === 'BACKSPACE') current = current.length > 1 ? current.slice(0, -1) : '0';
    else if (val === 'CLEAR') current = '0';
    else if (val === '+000') current = current === '0' ? '0' : current + '000';
    else if (val === '.') { if (!current.includes('.')) current = current + '.'; }
    else current = current === '0' ? val : current + val;

    if (modalActiveField === 'QTY') {
      setModalQty(current);
      const q = parseFloat(current) || 0;
      const p = parseFloat(modalPrice) || 0;
      setModalTotal((q * p).toFixed(4));
    } else if (modalActiveField === 'PRICE') {
      setModalPrice(current);
      const q = parseFloat(modalQty) || 0;
      const p = parseFloat(current) || 0;
      setModalTotal((q * p).toFixed(4));
    } else if (modalActiveField === 'TOTAL') {
      setModalTotal(current);
      const tot = parseFloat(current) || 0;
      const q = parseFloat(modalQty) || 1;
      if (q > 0) setModalPrice((tot / q).toFixed(4));
    }
  };

  const handleSaveModalItem = (closeAfter: boolean = true) => {
    if (!modalProduct) return;
    const q = parseFloat(modalQty) || 1;
    const p = parseFloat(modalPrice) || 0;
    const tot = parseFloat(modalTotal) || (q * p);
    const unitInfo = getUnitInfo(modalProduct, modalUnit);

    const item = {
      id: itemModalIndex !== null ? invoiceItems[itemModalIndex].id : `ITEM-${Date.now()}`,
      product: modalProduct,
      quantity: q,
      unitPrice: p,
      unitCost: unitInfo.cost,
      discount: 0,
      total: Number(tot.toFixed(4)),
      unit: modalUnit,
      unitFactor: unitInfo.factor,
      expiryDate: modalExpiry,
      notes: '',
    };

    if (itemModalMode === 'EDIT' && itemModalIndex !== null) {
      setInvoiceItems(prev => prev.map((it, idx) => idx === itemModalIndex ? item : it));
    } else {
      setInvoiceItems(prev => [...prev, item]);
    }

    if (modalUpdateMasterPrice) {
      handleUpdateMasterPrice(modalProduct.id, p, modalProduct.name);
      setModalUpdateMasterPrice(false); // Reset for next item
    }

    if (closeAfter) setShowItemModal(false);
    else {
      setModalQty('1');
      setModalTotal(modalPrice);
    }
  };

  return (
    <div className="flex flex-col h-full bg-[#edf2f7] text-slate-800 font-sans select-none overflow-hidden" dir="rtl">
      {/* 1. Desktop Window Header Bar */}
      <header className="bg-white text-slate-800 px-3 py-1.5 flex items-center justify-between border-b border-slate-200 shadow-xs shrink-0">
        <div className="flex items-center gap-3">
          {onBackToDashboard && (
            <button
              onClick={onBackToDashboard}
              className="bg-slate-50 hover:bg-slate-100 text-slate-700 px-2.5 py-1 rounded-lg text-xs flex items-center gap-1.5 transition border border-slate-200 cursor-pointer font-medium"
              title="العودة لشاشة النظام الرئيسية"
            >
              <ArrowLeft className="w-3.5 h-3.5 rotate-180 text-slate-500" />
              <span>الرئيسية</span>
            </button>
          )}

          <div className="flex items-center gap-2">
            <div className={`w-7 h-7 rounded-lg ${isReturn ? 'bg-rose-600' : 'bg-sky-600'} flex items-center justify-center text-white shadow-xs font-bold text-xs`}>
              {isReturn ? 'RET' : 'POS'}
            </div>
            <div>
              <div className="flex items-center gap-2">
                <span className="font-bold text-xs text-slate-900">
                  {isReturn ? 'مردود مبيعات تجارية [Sales Return]' : 'فاتورة مبيعات تجارية [محاسب سوفت Desktop Enterprise]'}
                </span>
                <span className={`px-2 py-0.5 rounded text-[11px] font-mono font-bold border ${isReturn ? 'bg-rose-50 text-rose-700 border-rose-200' : 'bg-sky-50 text-sky-700 border-sky-200'}`}>
                  {invoiceNo}
                </span>
                <span className="bg-emerald-50 text-emerald-800 px-2 py-0.5 rounded text-[11px] font-bold border border-emerald-200 flex items-center gap-1">
                  <span className="w-1.5 h-1.5 rounded-full bg-emerald-600"></span>
                  المخزن: {warehouses.find(w => w.id === selectedWarehouseId)?.name || 'المخزن الرئيسي'}
                </span>
              </div>
            </div>
          </div>
        </div>

        {/* View Mode Switcher + Action Buttons */}
        <div className="flex items-center gap-2">
          {/* Sale vs Return Selector */}
          <div className="bg-slate-100 p-0.5 rounded-lg border border-slate-200 flex items-center gap-1 text-[11px]">
            <button
              type="button"
              onClick={() => setIsReturn(false)}
              className={`px-2.5 py-1 rounded-md font-bold flex items-center gap-1 transition cursor-pointer ${
                !isReturn
                  ? 'bg-emerald-600 text-white shadow-xs'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              <ShoppingCart className="w-3.5 h-3.5" />
              <span>فاتورة مبيعات</span>
            </button>
            <button
              type="button"
              onClick={() => setIsReturn(true)}
              className={`px-2.5 py-1 rounded-md font-bold flex items-center gap-1 transition cursor-pointer ${
                isReturn
                  ? 'bg-rose-600 text-white shadow-xs'
                  : 'text-slate-600 hover:text-rose-700'
              }`}
            >
              <RotateCcw className="w-3.5 h-3.5" />
              <span>مردود مبيعات</span>
            </button>
          </div>

          <div className="h-5 w-px bg-slate-200" />

          {/* Dual-Mode Toggle: Desktop vs Touch */}
          <div className="bg-slate-100 p-0.5 rounded-lg border border-slate-200 flex items-center gap-1 text-[11px]">
            <button
              type="button"
              onClick={() => setViewMode('DESKTOP')}
              className={`px-2.5 py-1 rounded-md font-bold flex items-center gap-1.5 transition cursor-pointer ${
                viewMode === 'DESKTOP'
                  ? 'bg-white text-sky-700 shadow-xs border border-slate-200'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
              title="الواجهة المكتبية الكثيفة للإدخال السريع بالكيبورد والباركود"
            >
              <Monitor className="w-3.5 h-3.5" />
              <span>الوضع المكتبي (Desktop Grid)</span>
            </button>

            <button
              type="button"
              onClick={() => setViewMode('TOUCH_MOBILE')}
              className={`px-2.5 py-1 rounded-md font-bold flex items-center gap-1.5 transition cursor-pointer ${
                viewMode === 'TOUCH_MOBILE'
                  ? 'bg-white text-purple-700 shadow-xs border border-slate-200'
                  : 'text-slate-600 hover:text-slate-900'
              }`}
              title="واجهة اللمس والأجهزة الكفية مع اللوحة الرقمية"
            >
              <Smartphone className="w-3.5 h-3.5" />
              <span>وضع شاشة اللمس (Touch POS)</span>
            </button>
          </div>

          <div className="h-5 w-px bg-slate-200" />

          {/* F9 Save */}
          <button
            onClick={() => handleSaveInvoice(false)}
            className="bg-emerald-600 hover:bg-emerald-700 text-white px-3 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1 shadow-xs cursor-pointer"
            title="اعتماد وحفظ الفاتورة [F9]"
          >
            <Save className="w-3.5 h-3.5" />
            <span>حفظ وترحيل [F9]</span>
          </button>

          {/* F10 Print / PDF */}
          <button
            onClick={() => window.print()}
            className="bg-slate-100 hover:bg-slate-200 text-slate-700 px-2.5 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1 border border-slate-200 cursor-pointer"
            title="طباعة الفاتورة أو حفظها بتنسيق PDF [F10]"
          >
            <Printer className="w-3.5 h-3.5 text-slate-600" />
            <span>طباعة / PDF [F10]</span>
          </button>

          {/* Export XLSX */}
          <button
            onClick={handleExportInvoiceXLSX}
            className="bg-emerald-50 hover:bg-emerald-100 text-emerald-800 border border-emerald-300 px-2.5 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1 cursor-pointer"
            title="تصدير بيانات الفاتورة الحالية إلى إكسل (.xlsx)"
          >
            <FileSpreadsheet className="w-3.5 h-3.5 text-emerald-600" />
            <span>تصدير XLSX</span>
          </button>

          {/* F12 Draft */}
          <button
            onClick={() => handleSaveInvoice(true)}
            className="bg-amber-600 hover:bg-amber-700 text-white px-2.5 py-1 rounded-lg text-xs font-bold transition flex items-center gap-1 cursor-pointer"
            title="حفظ كمسودة / تعليق [F12]"
          >
            <PauseCircle className="w-3.5 h-3.5" />
            <span>تعليق [F12]</span>
          </button>
        </div>
      </header>

      {/* 2. Phase 1 Operational Stage Ribbon (شريط المرحلة الأولى: دورة المبيعات ونقاط البيع) */}
      <div className="bg-slate-50 border-b border-slate-200 px-3 py-1.5 flex flex-wrap items-center justify-between gap-2 text-xs shrink-0">
        {/* Left: Phase Title & Workflow Steps Navigation */}
        <div className="flex items-center gap-2 overflow-x-auto">
          {/* Phase Badge */}
          <div className="flex items-center gap-1.5 bg-sky-700 text-white px-2.5 py-0.5 rounded-full shadow-xs">
            <span className="w-4 h-4 rounded-full bg-white text-sky-900 flex items-center justify-center font-black text-[10px]">
              1
            </span>
            <span className="font-bold text-[11px]">المرحلة الأولى: دورة المبيعات ونقاط البيع</span>
          </div>

          <div className="h-4 w-px bg-slate-200" />

          {/* Direct Switchers between Phase 1 Tabs */}
          <div className="flex items-center gap-1">
            <button
              type="button"
              className="bg-white text-sky-700 font-bold px-2.5 py-0.5 rounded text-[11px] flex items-center gap-1 shadow-xs border border-slate-200"
              title="أنت الآن في شاشة فاتورة المبيعات التجارية"
            >
              <Receipt className="w-3 h-3 text-sky-600" />
              <span>فاتورة مبيعات تجارية</span>
            </button>

            {onSelectTab && (
              <>
                <button
                  type="button"
                  onClick={() => onSelectTab('POS')}
                  className="bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="الانتقال لنقطة البيع السريعة للكاشير [F1]"
                >
                  <ShoppingCart className="w-3 h-3 text-emerald-600" />
                  <span>نقطة البيع [F1]</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('SALES_STRATEGIES_HUB')}
                  className="bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="مختبر تجربة ومقارنة استراتيجيات الإدخال الست"
                >
                  <Sparkles className="w-3 h-3 text-amber-600" />
                  <span>مختبر الـ 6 استراتيجيات</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('PRICE_QUOTATION')}
                  className="bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="عروض الأسعار للعملاء وإمكانية تحويلها لفواتير"
                >
                  <FileText className="w-3 h-3 text-sky-600" />
                  <span>عروض الأسعار</span>
                </button>

                <button
                  type="button"
                  onClick={() => onSelectTab('SALES_RETURNS')}
                  className="bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded text-[11px] flex items-center gap-1 transition border border-slate-200 cursor-pointer font-medium"
                  title="مردودات المبيعات وإشعار الدائن"
                >
                  <RotateCcw className="w-3 h-3 text-rose-600" />
                  <span>مردودات المبيعات</span>
                </button>
              </>
            )}
          </div>
        </div>

        {/* Right: Architectural Rules & Next Stage Shortcut */}
        <div className="flex items-center gap-2">
          {/* Rules Indicators */}
          <div className="hidden xl:flex items-center gap-1 text-[10px] text-slate-600">
            <span className="bg-emerald-50 text-emerald-800 px-1.5 py-0.5 rounded border border-emerald-200 font-mono font-bold">
              ✓ تسلسل رقمي: {invoiceNo}
            </span>
            <span className="bg-sky-50 text-sky-800 px-1.5 py-0.5 rounded border border-sky-200 font-bold">
              ✓ توزيع نسبي للخصم والضريبة
            </span>
            <span className="bg-amber-50 text-amber-800 px-1.5 py-0.5 rounded border border-amber-200 font-bold">
              ✓ خصم فوري FIFO
            </span>
          </div>

          {onSelectTab && (
            <button
              type="button"
              onClick={() => onSelectTab('INVENTORY')}
              className="bg-amber-600 hover:bg-amber-700 text-white text-[11px] font-bold px-2.5 py-1 rounded-lg flex items-center gap-1 transition shadow-xs cursor-pointer"
              title="الانتقال إلى المرحلة الثانية: المخازن والتوريد"
            >
              <span>المرحلة 2: المخازن والتوريد</span>
              <ChevronLeft className="w-3.5 h-3.5" />
            </button>
          )}
        </div>
      </div>

      {/* Alert Banner */}
      {alertMsg && (
        <div
          className={`px-4 py-1.5 text-xs font-bold flex items-center justify-between border-b shrink-0 ${
            alertMsg.type === 'success'
              ? 'bg-emerald-600 text-white border-emerald-700'
              : alertMsg.type === 'error'
              ? 'bg-rose-600 text-white border-rose-700 animate-pulse'
              : 'bg-amber-400 text-slate-900 border-amber-500'
          }`}
        >
          <div className="flex items-center gap-2">
            {alertMsg.type === 'error' ? <AlertCircle className="w-4 h-4" /> : <Check className="w-4 h-4" />}
            <span>{alertMsg.text}</span>
          </div>
          <button onClick={() => setAlertMsg(null)} className="text-white hover:underline text-xs cursor-pointer">
            إغلاق ✕
          </button>
        </div>
      )}

      {/* ============================================================== */}
      {/* 3. MAIN WORKSPACE: Touch Mode vs Desktop Grid Mode */}
      {/* ============================================================== */}
      {viewMode === 'TOUCH_MOBILE' ? (
        <div className="flex-1 overflow-hidden">
          <FrmSalesInvoice_TouchGrid
            products={products}
            customers={customers}
            onSelectTab={onSelectTab}
            onSaveSuccess={(num) => setAlertMsg({ type: 'success', text: `تم حفظ وترحيل الفاتورة #${num} بنجاح!` })}
          />
        </div>
      ) : (
        <div className="flex-1 flex flex-col min-h-0 overflow-y-auto p-3 gap-3">
          {/* Header Controls & Customer Balance Banner */}
          <div className="bg-white border border-slate-200 rounded-xl p-3 shadow-xs flex flex-col gap-2.5">
            <div className="grid grid-cols-1 md:grid-cols-12 gap-2.5 text-xs items-center">
              {/* Invoice Number */}
              <div className="md:col-span-2">
                <label className="block text-slate-700 font-bold mb-1">رقم الفاتورة:</label>
                <input
                  type="text"
                  value={invoiceNo}
                  onChange={e => setInvoiceNo(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-emerald-700 font-mono font-extrabold text-center"
                />
              </div>

              {/* Customer Selector & Quick Modal */}
              <div className="md:col-span-4">
                <div className="flex items-center justify-between mb-1">
                  <label className="block text-slate-700 font-bold">العميل والمستفيد:</label>
                  <button
                    type="button"
                    onClick={() => setShowCustomerPickerModal(true)}
                    className="text-sky-700 hover:text-sky-800 font-bold text-[11px] flex items-center gap-1 cursor-pointer"
                  >
                    <Search className="w-3 h-3" />
                    <span>دليل العملاء [F2]</span>
                  </button>
                </div>
                <div className="flex items-center gap-1.5">
                  <select
                    value={selectedCustomerId}
                    onChange={e => {
                      const cid = e.target.value;
                      setSelectedCustomerId(cid);
                      const found = customers.find(c => c.id === cid);
                      if (found) setCustomerName(found.name);
                    }}
                    className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-bold focus:bg-white focus:border-sky-500"
                  >
                    {customers.map(c => (
                      <option key={c.id} value={c.id}>{c.name} (#{c.code})</option>
                    ))}
                  </select>
                </div>
              </div>

              {/* Customer Balance Direct Banner (Loaded once clearly for non-accountants & accountants) */}
              <div className="md:col-span-4">
                <label className="block text-slate-700 font-bold mb-1">حالة رصيد وحساب العميل الحالي:</label>
                {activeCustomer ? (
                  <div>
                    {activeCustomer.currentBalance > 0 ? (
                      <div className="bg-amber-50 border border-amber-300 rounded-lg px-2.5 py-1 text-amber-900 flex items-center justify-between font-bold">
                        <span className="text-[11px]">عليه للمحل (مطلوب سداد):</span>
                        <div className="text-left">
                          <span className="font-mono text-xs text-amber-950 font-black">{activeCustomer.currentBalance.toLocaleString()} {activeCustomer.currency || selectedCurrency}</span>
                          <span className="text-[9px] text-amber-700 block font-normal text-left">رصيد مدين</span>
                        </div>
                      </div>
                    ) : activeCustomer.currentBalance < 0 ? (
                      <div className="bg-emerald-50 border border-emerald-300 rounded-lg px-2.5 py-1 text-emerald-900 flex items-center justify-between font-bold">
                        <span className="text-[11px]">له دفعة سابقة (فائض له):</span>
                        <div className="text-left">
                          <span className="font-mono text-xs text-emerald-950 font-black">{Math.abs(activeCustomer.currentBalance).toLocaleString()} {activeCustomer.currency || selectedCurrency}</span>
                          <span className="text-[9px] text-emerald-700 block font-normal text-left">رصيد دائن</span>
                        </div>
                      </div>
                    ) : (
                      <div className="bg-slate-50 border border-slate-200 rounded-lg px-2.5 py-1 text-slate-700 flex items-center justify-between font-bold">
                        <span className="text-[11px]">حساب خالص (لا توجد مديونية سابقة)</span>
                        <span className="text-[10px] text-emerald-700 font-mono">0 {selectedCurrency}</span>
                      </div>
                    )}
                  </div>
                ) : (
                  <div className="bg-slate-50 border border-slate-200 rounded-lg px-2.5 py-1 text-slate-500 text-xs">
                    عميل نقدي عام
                  </div>
                )}
              </div>

              {/* Cash vs Credit Switch */}
              <div className="md:col-span-2">
                <label className="block text-slate-700 font-bold mb-1">نوع السداد:</label>
                <button
                  type="button"
                  onClick={() => setIsCash(!isCash)}
                  className={`w-full py-1.5 px-2 rounded-lg font-bold text-xs flex items-center justify-center gap-1.5 transition border cursor-pointer ${
                    isCash
                      ? 'bg-emerald-600 text-white border-emerald-700 shadow-xs'
                      : 'bg-amber-100 text-amber-900 border-amber-300'
                  }`}
                  title="التبديل بين نقدي وآجل [F3]"
                >
                  <Coins className="w-3.5 h-3.5" />
                  <span>{isCash ? 'نقدي [F3]' : 'آجل على الحساب [F3]'}</span>
                </button>
              </div>
            </div>

            {/* Sub-row: Warehouse, Currency, Dates */}
            <div className="grid grid-cols-2 md:grid-cols-5 gap-2.5 text-xs pt-2 border-t border-slate-100 items-center">
              <div>
                <label className="block text-slate-600 mb-0.5 font-medium">المستودع:</label>
                <select
                  value={selectedWarehouseId}
                  onChange={e => setSelectedWarehouseId(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800 font-bold"
                >
                  {warehouses.map(w => (
                    <option key={w.id} value={w.id}>{w.name}</option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-slate-600 mb-0.5 font-medium">العملة:</label>
                <button
                  type="button"
                  onClick={() => setShowCurrencyModal(true)}
                  className="w-full bg-slate-50 border border-slate-300 hover:bg-slate-100 rounded px-2 py-1 text-slate-800 font-bold flex items-center justify-between cursor-pointer"
                >
                  <span>{selectedCurrency}</span>
                  <ChevronDown className="w-3.5 h-3.5 text-slate-400" />
                </button>
              </div>

              <div>
                <label className="block text-slate-600 mb-0.5 font-medium">تاريخ الفاتورة:</label>
                <input
                  type="date"
                  value={invoiceDate}
                  onChange={e => setInvoiceDate(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800 font-mono text-center"
                />
              </div>

              <div>
                <label className="block text-slate-600 mb-0.5 font-medium">تاريخ الاستحقاق:</label>
                <input
                  type="date"
                  value={dueDate}
                  onChange={e => setDueDate(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800 font-mono text-center"
                />
              </div>

              <div>
                <label className="block text-slate-600 mb-0.5 font-medium">ملاحظات الفاتورة:</label>
                <input
                  type="text"
                  value={notes}
                  onChange={e => setNotes(e.target.value)}
                  placeholder="ملاحظات اختيارية..."
                  className="w-full bg-slate-50 border border-slate-300 rounded px-2 py-1 text-slate-800"
                />
              </div>
            </div>
          </div>

          {/* Rapid Entry Bar + F3 Product Selection Button */}
          <div className="bg-white border-2 border-sky-500 rounded-xl p-3 shadow-xs flex flex-col gap-2">
            <div className="flex items-center justify-between border-b border-slate-100 pb-1.5 text-xs">
              <span className="font-bold text-sky-900 flex items-center gap-1.5">
                <Sparkles className="w-4 h-4 text-sky-600" />
                <span>شريط إدراج الأصناف والباركود السريع</span>
              </span>

              {/* Dedicated Button for F3 Product Selection Modal */}
              <button
                type="button"
                onClick={() => setShowF3Modal(true)}
                className="px-3 py-1 bg-sky-700 hover:bg-sky-800 text-white rounded-lg font-bold text-xs flex items-center gap-1.5 transition cursor-pointer shadow-xs"
                title="فتح شاشة خاصة ومنفصلة لعرض واختيار الأصناف والبيانات المقترحة [F3]"
              >
                <Layers className="w-3.5 h-3.5 text-amber-300" />
                <span>استعراض واختيار صنف [F3]</span>
              </button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-12 gap-2 text-xs items-end">
              {/* Barcode / Search Input */}
              <div className="md:col-span-4 relative">
                <label className="block text-slate-700 font-bold mb-1">1. مسح الباركود أو البحث عن الصنف:</label>
                <div className="relative">
                  <Search className="w-4 h-4 text-slate-400 absolute right-2.5 top-2.5" />
                  <input
                    ref={barcodeInputRef}
                    type="text"
                    placeholder="امسح الباركود أو اكتب اسم الصنف ثم Enter..."
                    value={barcodeInput}
                    onChange={e => {
                      setBarcodeInput(e.target.value);
                      setItemSearchText(e.target.value);
                      setShowSuggestions(true);
                    }}
                    onKeyDown={e => {
                      if (e.key === 'Enter') {
                        e.preventDefault();
                        handleBarcodeScan(barcodeInput);
                      }
                    }}
                    className="w-full bg-slate-50 border border-slate-300 text-slate-900 rounded-lg pr-9 pl-2.5 py-1.5 font-bold focus:bg-white focus:border-sky-500 text-xs"
                  />
                  {showSuggestions && itemSearchText.trim() && (
                    <div className="absolute top-full right-0 left-0 mt-1 bg-white border border-slate-300 rounded-lg shadow-xl z-50 max-h-52 overflow-y-auto divide-y divide-slate-100">
                      {products
                        .filter(p => p.name.includes(itemSearchText) || p.code.includes(itemSearchText) || p.barcode?.includes(itemSearchText))
                        .slice(0, 8)
                        .map(p => (
                          <div
                            key={p.id}
                            onClick={() => {
                              addProductToInvoice(p);
                              setBarcodeInput('');
                              setItemSearchText('');
                              setShowSuggestions(false);
                            }}
                            className="p-2 hover:bg-sky-50 cursor-pointer flex items-center justify-between text-xs"
                          >
                            <div>
                              <span className="font-bold text-slate-900">{p.name}</span>
                              <span className="text-[10px] text-slate-500 mr-2 font-mono">#{p.code}</span>
                            </div>
                            <span className="font-bold text-sky-700 font-mono">{p.salePrice} {selectedCurrency}</span>
                          </div>
                        ))}
                    </div>
                  )}
                </div>
              </div>

              {/* Quantity */}
              <div className="md:col-span-2">
                <label className="block text-slate-700 font-bold mb-1">2. الكمية:</label>
                <input
                  type="number"
                  min="1"
                  value={rapidBarQty}
                  onChange={e => setRapidBarQty(Math.max(1, parseFloat(e.target.value) || 1))}
                  className="w-full bg-slate-50 border border-slate-300 text-slate-900 font-bold font-mono rounded-lg px-2.5 py-1.5 text-center focus:bg-white"
                />
              </div>

              {/* Unit */}
              <div className="md:col-span-2">
                <label className="block text-slate-700 font-bold mb-1">3. الوحدة:</label>
                <select
                  value={rapidBarUnit}
                  onChange={e => setRapidBarUnit(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-300 text-slate-900 font-bold rounded-lg px-2 py-1.5 focus:bg-white"
                >
                  <option value="حبة">حبة</option>
                  <option value="باكت">باكت</option>
                  <option value="كرتون">كرتون</option>
                  <option value="درزن">درزن</option>
                  <option value="كيلو">كيلو</option>
                </select>
              </div>

              {/* Price */}
              <div className="md:col-span-2">
                <label className="block text-slate-700 font-bold mb-1">4. السعر:</label>
                <input
                  type="number"
                  step="any"
                  value={rapidBarPrice}
                  onChange={e => setRapidBarPrice(Math.max(0, parseFloat(e.target.value) || 0))}
                  className="w-full bg-slate-50 border border-slate-300 text-sky-800 font-bold font-mono rounded-lg px-2.5 py-1.5 text-center focus:bg-white"
                />
              </div>

              {/* Insert Button */}
              <div className="md:col-span-2">
                <button
                  type="button"
                  onClick={() => {
                    if (products.length > 0) {
                      const prod = products.find(p => p.id === rapidBarProductId) || products[0];
                      addProductToInvoice(prod, rapidBarUnit, rapidBarQty, rapidBarPrice, rapidBarDiscount, rapidBarNotes);
                    }
                  }}
                  className="w-full bg-sky-600 hover:bg-sky-700 text-white font-extrabold py-1.5 px-3 rounded-lg shadow-xs flex items-center justify-center gap-1 transition text-xs cursor-pointer"
                >
                  <Plus className="w-3.5 h-3.5" />
                  <span>إدراج الفاتورة</span>
                </button>
              </div>
            </div>
          </div>

          {/* Interactive Invoice Items Table */}
          <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[280px] shadow-xs">
            <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
              <span className="flex items-center gap-1.5">
                <FileText className="w-3.5 h-3.5 text-sky-700" />
                <span>بنود الفاتورة (جدول الأصناف التفاعلي)</span>
              </span>
              <span className="text-slate-500 font-medium">عدد الأصناف: {invoiceItems.length}</span>
            </div>

            <div className="overflow-x-auto flex-1">
              <table className="w-full text-right text-xs border-collapse">
                <thead>
                  <tr className="bg-slate-100/90 text-slate-700 border-b border-slate-200 font-bold select-none">
                    <th className="p-2 text-center w-10">#</th>
                    <th className="p-2 w-24">كود الصنف</th>
                    <th className="p-2">اسم الصنف والبيانات</th>
                    <th className="p-2 text-center w-24">الوحدة</th>
                    <th className="p-2 text-center w-20">الكمية</th>
                    <th className="p-2 text-center w-24">سعر الوحدة</th>
                    <th className="p-2 text-center w-20">الخصم</th>
                    <th className="p-2 text-center w-28">الإجمالي الصافي</th>
                    <th className="p-2 text-center w-24">الصلاحية</th>
                    <th className="p-2 w-32">ملاحظات</th>
                    <th className="p-2 text-center w-14">إجراء</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {invoiceItems.length === 0 ? (
                    <tr>
                      <td colSpan={11} className="p-8 text-center text-slate-500">
                        <div className="flex flex-col items-center justify-center gap-2">
                          <ShoppingCart className="w-8 h-8 text-slate-300" />
                          <p className="font-bold text-slate-700">لا توجد أصناف في الفاتورة حالياً</p>
                          <p className="text-[11px] text-slate-400">استخدم شريط الإدراج أو اضغط F3 لاستعراض واختيار الأصناف</p>
                        </div>
                      </td>
                    </tr>
                  ) : (
                    invoiceItems.map((item, idx) => (
                      <tr 
                        key={item.id} 
                        onContextMenu={(e) => handleRowContextMenu(e, item, idx)}
                        className="hover:bg-sky-50/60 transition cursor-context-menu"
                      >
                        <td className="p-2 text-center font-mono text-slate-400 font-bold">{idx + 1}</td>
                        <td className="p-2 font-mono font-bold text-slate-600">{item.product.code}</td>
                        <td className="p-2 font-bold text-slate-900">{item.product.name}</td>
                        <td className="p-2 text-center">
                          <select
                            value={item.unit}
                            onChange={e => handleGridChange(idx, 'unit', e.target.value)}
                            className="bg-slate-50 border border-slate-300 rounded px-1.5 py-0.5 text-xs text-center font-bold"
                          >
                            <option value="حبة">حبة</option>
                            <option value="باكت">باكت</option>
                            <option value="كرتون">كرتون</option>
                            <option value="درزن">درزن</option>
                            <option value="كيلو">كيلو</option>
                          </select>
                        </td>
                        <td className="p-2 text-center">
                          <input
                            type="number"
                            min="0.01"
                            step="any"
                            value={item.quantity}
                            onChange={e => handleGridChange(idx, 'quantity', e.target.value)}
                            className="w-16 bg-slate-50 border border-slate-300 rounded px-1.5 py-0.5 text-center font-mono font-bold"
                          />
                        </td>
                        <td className="p-2 text-center">
                          <input
                            type="number"
                            step="any"
                            value={item.unitPrice}
                            onChange={e => handleGridChange(idx, 'unitPrice', e.target.value)}
                            className="w-20 bg-slate-50 border border-slate-300 rounded px-1.5 py-0.5 text-center font-mono font-bold text-sky-800"
                          />
                        </td>
                        <td className="p-2 text-center">
                          <input
                            type="number"
                            step="any"
                            value={item.discount}
                            onChange={e => handleGridChange(idx, 'discount', e.target.value)}
                            className="w-16 bg-slate-50 border border-slate-300 rounded px-1.5 py-0.5 text-center font-mono font-bold text-amber-800"
                          />
                        </td>
                        <td className="p-2 text-center font-mono font-extrabold text-emerald-700">
                          {item.total.toLocaleString()} {selectedCurrency}
                        </td>
                        <td className="p-2 text-center font-mono text-[11px] text-slate-500">
                          {item.expiryDate || '—'}
                        </td>
                        <td className="p-2">
                          <input
                            type="text"
                            value={item.notes || ''}
                            onChange={e => handleGridChange(idx, 'notes', e.target.value)}
                            placeholder="ملاحظات..."
                            className="w-full bg-slate-50 border border-slate-200 rounded px-1.5 py-0.5 text-[11px]"
                          />
                        </td>
                        <td className="p-2 text-center">
                          <button
                            type="button"
                            onClick={() => handleDeleteRow(idx)}
                            className="p-1 text-rose-500 hover:text-rose-700 hover:bg-rose-50 rounded transition cursor-pointer"
                            title="حذف الصنف من الفاتورة"
                          >
                            <Trash2 className="w-3.5 h-3.5" />
                          </button>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>

            {/* Bottom Summary Bar */}
            <div className="bg-slate-100 p-3 border-t border-slate-200 grid grid-cols-2 md:grid-cols-6 gap-3 text-xs">
              <div className="bg-white p-2 rounded-lg border border-slate-200">
                <span className="text-slate-500 block text-[10px]">المجموع قبل الخصم:</span>
                <span className="font-mono font-bold text-slate-900">{rawSubtotal.toLocaleString()} {selectedCurrency}</span>
              </div>
              <div className="bg-white p-2 rounded-lg border border-slate-200">
                <span className="text-slate-500 block text-[10px]">إجمالي الخصومات:</span>
                <span className="font-mono font-bold text-amber-700">{(itemsTotalDiscount + commercialDiscount).toLocaleString()} {selectedCurrency}</span>
              </div>
              <div className="bg-white p-2 rounded-lg border border-slate-200">
                <span className="text-slate-500 block text-[10px]">الضريبة:</span>
                <span className="font-mono font-bold text-slate-700">{calculatedTax.toLocaleString()} {selectedCurrency}</span>
              </div>
              <div className="bg-sky-50 border border-sky-300 p-2 rounded-lg">
                <span className="text-sky-900 block text-[10px] font-bold">المبلغ الصافي المستحق:</span>
                <span className="font-mono font-black text-sm text-sky-950">{grandTotal.toLocaleString()} {selectedCurrency}</span>
              </div>
              <div className="bg-white p-2 rounded-lg border border-slate-200">
                <span className="text-slate-500 block text-[10px]">المدفوع:</span>
                <span className="font-mono font-bold text-emerald-700">{calcPaidAmount.toLocaleString()} {selectedCurrency}</span>
              </div>
              <div className="bg-white p-2 rounded-lg border border-slate-200">
                <span className="text-slate-500 block text-[10px]">المتبقي:</span>
                <span className="font-mono font-bold text-rose-700">{remainingAmount.toLocaleString()} {selectedCurrency}</span>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* ============================================================== */}
      {/* MODAL: Currency Selection Modal */}
      {/* ============================================================== */}
      <SalesCurrencyModal
        showCurrencyModal={showCurrencyModal}
        setShowCurrencyModal={setShowCurrencyModal}
        selectedCurrency={selectedCurrency}
        setSelectedCurrency={setSelectedCurrency}
        currency={currency}
      />

      {/* ============================================================== */}
      {showDiscountModal && (
        <SalesDiscountExpensesModal
          headerDiscountType={headerDiscountType}
          setHeaderDiscountType={setHeaderDiscountType}
          headerDiscountVal={headerDiscountVal}
          setHeaderDiscountVal={setHeaderDiscountVal}
          taxPercentVal={taxPercentVal}
          setTaxPercentVal={setTaxPercentVal}
          setIsTaxEnabled={setIsTaxEnabled}
          otherChargesAccountCode={otherChargesAccountCode}
          setOtherChargesAccountCode={setOtherChargesAccountCode}
          setOtherChargesName={setOtherChargesName}
          expenseAccountsOptions={EXPENSE_ACCOUNTS_OPTIONS}
          otherChargesVal={otherChargesVal}
          setOtherChargesVal={setOtherChargesVal}
          paymentType={paymentType}
          splitCash={splitCash}
          setSplitCash={setSplitCash}
          splitNetwork={splitNetwork}
          setSplitNetwork={setSplitNetwork}
          paidAmountVal={paidAmountVal}
          setPaidAmountVal={setPaidAmountVal}
          onClose={() => setShowDiscountModal(false)}
        />
      )}

      {/* ============================================================== */}
      {/* MODAL: Camera / File Attachment Modal */}
      {/* ============================================================== */}
      {showCameraModal && (
        <SalesCameraAttachmentModal
          attachedImageName={attachedImageName}
          setAttachedImageName={setAttachedImageName}
          onClose={() => setShowCameraModal(false)}
        />
      )}

      {/* ============================================================== */}
      {/* MODAL: Quick Customer Selection Modal (الدليل وشجرة الحسابات) */}
      {/* ============================================================== */}
      {showCustomerPickerModal && (
        <SalesCustomerPickerModal
          customers={customers}
          selectedCustomerId={selectedCustomerId}
          isCash={isCash}
          selectedCurrency={selectedCurrency}
          onSelectCustomer={(id, name) => {
            setSelectedCustomerId(id);
            setCustomerName(name);
            setCustomerRequiredError(false);
          }}
          onClose={() => {
            setShowCustomerPickerModal(false);
          }}
        />
      )}

      {/* ============================================================== */}
      {/* MODAL: Customer Profit & Sales Breakdown Modal */}
      {/* ============================================================== */}
      {showProfitModal && (
        <SalesProfitBreakdownModal
          customerName={customerName}
          isCash={isCash}
          totalCost={totalCost}
          rawSubtotal={rawSubtotal}
          commercialDiscount={commercialDiscount}
          invoiceNetProfit={invoiceNetProfit}
          invoiceProfitMargin={invoiceProfitMargin}
          selectedCurrency={selectedCurrency}
          invoiceItems={invoiceItems}
          onClose={() => setShowProfitModal(false)}
        />
      )}

      {/* ============================================================== */}
      {/* MODAL: Mode 5 - Smart F3 Product Selection Modal (شاشة اختيار الأصناف والبيانات المقترحة) */}
      {/* ============================================================== */}
      {showF3Modal && (
        <ProductSelectionModal
          isOpen={showF3Modal}
          products={products}
          currency={selectedCurrency}
          activeWarehouseName={activeWarehouseName}
          onConfirmItem={(result) => {
            addProductToInvoice(
              result.product,
              result.selectedUnit,
              result.quantity,
              result.unitPrice,
              result.discount,
              result.notes,
              result.expiryDate,
              result.batchNo
            );
            setAlertMsg({ type: 'success', text: `تم إدراج الصنف [${result.product.name}] من شاشة استعراض واختيار الأصناف [F3]` });
          }}
          onClose={() => setShowF3Modal(false)}
        />
      )}

      {/* ============================================================== */}
      {/* MODAL: Mode 6 - Import & Convert Modal (التحويل والاستيراد التلقائي) */}
      {/* ============================================================== */}
      {showImportModal && (
        <SalesImportConvertModal
          products={products}
          selectedCurrency={selectedCurrency}
          onImportItems={(sourceTitle, items) => {
            handleImportQuotation(sourceTitle, items);
          }}
          onClose={() => setShowImportModal(false)}
        />
      )}

      {/* DGV Right-Click ContextMenuStrip */}
      <WinFormsDGVContextMenu
        x={dgvMenuState.x}
        y={dgvMenuState.y}
        visible={dgvMenuState.visible}
        rowIndex={dgvMenuState.itemIndex}
        rowTitle={dgvMenuState.itemTitle}
        items={dgvContextMenuItems}
        onClose={() => setDgvMenuState(prev => ({ ...prev, visible: false }))}
      />
    </div>
  );
};
