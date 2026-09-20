'use client';

import React, { useState, useRef, useEffect, useCallback, useMemo } from 'react';
import { CurrencySelectionModal } from './CurrencySelectionModal';
import { useWinFormsDialog } from '@/context/WinFormsDialogContext';
import {
  POSHeldOrdersModal,
  POSDiscountModal,
} from './pos';
import { 
  ProductItem, 
  CartItem, 
  Customer, 
  PaymentType, 
  Invoice, 
  HeldOrder,
  AccountNode,
  ShiftInfo
} from '@/types/pos';
import { generateNextId, generateSalesInvoiceNumber, getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import { 
  Barcode, 
  Search, 
  Trash2, 
  Plus, 
  Minus, 
  CreditCard, 
  Coins,
  Printer, 
  PauseCircle, 
  RotateCcw, 
  UserCheck, 
  CheckCircle2, 
  AlertCircle, 
  AlertTriangle,
  Receipt, 
  ArrowLeftRight,
  Sparkles,
  ShoppingBag,
  Building2,
  Wallet,
  Settings,
  ShieldAlert,
  Monitor,
  Globe,
  Smartphone,
  Zap,
  ChevronDown,
  ChevronUp,
  LayoutGrid,
  Check,
  Tag,
  X
} from 'lucide-react';

interface POSModuleProps {
  products: ProductItem[];
  customers: Customer[];
  heldOrders: HeldOrder[];
  chartOfAccounts?: AccountNode[];
  onCompleteSale: (invoice: Invoice) => void;
  onHoldOrder: (heldOrder: HeldOrder) => void;
  onRecallOrder: (heldOrderId: string) => CartItem[];
  currency: string;
  cashierName: string;
  onOpenQuickProductModal: () => void;
  onRequestPrintReceipt: (invoice: Invoice) => void;
  defaultCashboxCode?: string;
  shift?: ShiftInfo;
  onNavigateToShiftClosing?: () => void;
}

export type POSLayoutMode = 'DESKTOP' | 'WEB' | 'MOBILE';

export const POSModule: React.FC<POSModuleProps> = ({
  products,
  customers,
  heldOrders,
  chartOfAccounts = [],
  onCompleteSale,
  onHoldOrder,
  onRecallOrder,
  currency,
  cashierName,
  onRequestPrintReceipt,
  defaultCashboxCode,
  shift,
  onNavigateToShiftClosing,
}) => {
  const { showMessageBox, showError, showSuccess, showWarning, showConfirm } = useWinFormsDialog();
  const [posLayoutMode, setPosLayoutMode] = useState<POSLayoutMode>('DESKTOP');
  const [cart, setCart] = useState<CartItem[]>([]);
  const [barcodeInput, setBarcodeInput] = useState<string>('');
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>('CUST-001');
  const [selectedPaymentType, setSelectedPaymentType] = useState<PaymentType>('CASH');
  const [isSplitPaymentMode, setIsSplitPaymentMode] = useState<boolean>(false);
  const [paymentMainMode, setPaymentMainMode] = useState<'CASH' | 'CREDIT'>('CASH');
  
  // Amounts
  const [splitCash, setSplitCash] = useState<number>(0);
  const [splitNetwork, setSplitNetwork] = useState<number>(0);
  const [splitWallet, setSplitWallet] = useState<number>(0);
  const [splitCredit, setSplitCredit] = useState<number>(0);
  
  // Treasury & Bank accounts selection from Chart of Accounts
  const [selectedCashAccountCode, setSelectedCashAccountCode] = useState<string>(defaultCashboxCode || '1112');
  const [selectedBankAccountCode, setSelectedBankAccountCode] = useState<string>('1121');
  const [bankRefNo, setBankRefNo] = useState<string>('');
  const [autoDefaultCashCustomer, setAutoDefaultCashCustomer] = useState<boolean>(true);
  const [showDesktopProductPicker, setShowDesktopProductPicker] = useState<boolean>(false);
  const [mobileActiveTab, setMobileActiveTab] = useState<'CART' | 'CATALOG' | 'PAYMENT'>('CART');

  const EXPENSE_ACCOUNTS_OPTIONS = [
    { code: '424', name: 'أجور نقل ومواصلات' },
    { code: '425', name: 'شحن وتوصيل طلبات' },
    { code: '426', name: 'عتالة وتحميل وتفريغ' },
    { code: '423', name: 'صيانة وضيافة المحل' },
    { code: '421', name: 'إيجار وكهرباء وخدمات' },
    { code: '422', name: 'رواتب وأجور عمال إضافية' },
    { code: '429', name: 'مصروفات تشغيلية ونثرية أخرى' },
  ];

  const [cashTenderedInput, setCashTenderedInput] = useState<string>('');
  const [discountOverall, setDiscountOverall] = useState<number>(0);
  const [discountType, setDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [taxPercentVal, setTaxPercentVal] = useState<string>('0');
  const [isTaxEnabled, setIsTaxEnabled] = useState<boolean>(false);
  const [otherChargesVal, setOtherChargesVal] = useState<string>('0');
  const [otherChargesAccountCode, setOtherChargesAccountCode] = useState<string>('424');
  const [otherChargesName, setOtherChargesName] = useState<string>('أجور نقل ومواصلات');
  const [showDiscountModal, setShowDiscountModal] = useState<boolean>(false);
  const [paidCashInput, setPaidCashInput] = useState<string>('');
  const [selectedCategory, setSelectedCategory] = useState<string>('ALL');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [selectedRowIndex, setSelectedRowIndex] = useState<number>(-1);
  const [showCheckoutModal, setShowCheckoutModal] = useState<boolean>(false);
  const [showHeldModal, setShowHeldModal] = useState<boolean>(false);
  const [heldSearchQuery, setHeldSearchQuery] = useState<string>('');
  const [systemNotification, setSystemNotification] = useState<string | null>(null);

  // Multi-Currency State
  const [activeCurrency, setActiveCurrency] = useState<string>(currency || 'محلي');
  const [exchangeRate, setExchangeRate] = useState<number>(1);
  const [showCurrencyModal, setShowCurrencyModal] = useState<boolean>(false);

  const barcodeInputRef = useRef<HTMLInputElement>(null);

  // Auto focus barcode input on mount and after operations
  useEffect(() => {
    barcodeInputRef.current?.focus();
  }, [cart, showCheckoutModal]);

  // Categories list
  const categories = ['ALL', 'مواد غذائية', 'مشروبات ومياه', 'منتجات ألبان', 'حلويات وبسكويتات', 'معلبات', 'منظفات'];

  const selectedCustomer = customers.find((c) => c.id === selectedCustomerId) || customers[0];

  // Filtered products for quick touch / click grid
  const filteredProducts = products.filter((p) => {
    const matchCategory = selectedCategory === 'ALL' || p.category === selectedCategory;
    const matchQuery =
      searchQuery === '' ||
      p.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      p.barcode.includes(searchQuery) ||
      p.code.includes(searchQuery);
    return matchCategory && matchQuery;
  });

  // Calculate totals (Standardized Header-Level Discount, Tax % Dropdown & Other Charges support)
  const subtotal = cart.reduce((sum, item) => sum + item.quantity * item.unitPrice, 0);
  const itemsDiscountTotal = cart.reduce((sum, item) => sum + (item.discount || 0), 0);
  const calculatedOverallDiscount = discountType === 'PERCENT'
    ? (subtotal * Math.min(100, Math.max(0, discountOverall))) / 100
    : Math.min(subtotal, Math.max(0, discountOverall));
  const totalDiscount = itemsDiscountTotal + calculatedOverallDiscount;
  const taxableAmount = Math.max(0, subtotal - totalDiscount);
  const calculatedTax = isTaxEnabled ? (taxableAmount * (parseFloat(taxPercentVal) || 0)) / 100 : 0;
  const otherCharges = parseFloat(otherChargesVal) || 0;
  const grandTotal = Math.max(0, taxableAmount + calculatedTax + otherCharges);
  
  // Single tender calculations
  const numericPaid = parseFloat(paidCashInput) || grandTotal;
  const changeDue = Math.max(0, numericPaid - grandTotal);
  const remainingDebt = selectedPaymentType === 'CREDIT' ? grandTotal : Math.max(0, grandTotal - numericPaid);

  // Split payment totals
  const totalSplitTendered = splitCash + splitNetwork + splitWallet + splitCredit;
  const splitRemainingUnallocated = Math.max(0, grandTotal - totalSplitTendered);
  const splitCashReceived = parseFloat(cashTenderedInput) || splitCash;
  const splitCashChange = Math.max(0, splitCashReceived - splitCash);

  // Extract Cash accounts (الصناديق) from Chart of Accounts or use defaults
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
        { code: '1112', name: 'صندوق نقطة بيع POS كاشير 1' },
        { code: '1111', name: 'الصندوق الرئيسي (الخزينة المركزية)' },
        { code: '1113', name: 'صندوق نقطة بيع POS كاشير 2' },
      ];
    }
    return list;
  }, [chartOfAccounts]);

  // Extract Bank & E-Wallet accounts (البنوك والمحافظ) from Chart of Accounts or use defaults
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

  // Selected cash & bank account objects
  const currentCashAccount = availableCashAccounts.find((a) => a.code === selectedCashAccountCode) || availableCashAccounts[0];
  const currentBankAccount = availableBankAccounts.find((a) => a.code === selectedBankAccountCode) || availableBankAccounts[0];

  const showToast = useCallback((msg: string) => {
    setSystemNotification(msg);
    setTimeout(() => setSystemNotification(null), 3000);
  }, []);

  // Auto-init split values when opening checkout modal
  const handleOpenCheckout = useCallback(() => {
    if (cart.length === 0) {
      showError(
        'السلة فارغة حالياً! يرجى مسح باركود صنف أو اختياره من القائمة أولاً قبل الانتقال لعملية الدفع [F3].',
        'سلة الفاتورة فارغة (Empty Cart)',
        'pos-barcode-input'
      );
      return;
    }
    setPaymentMainMode('CASH');
    setSplitCash(grandTotal);
    setSplitNetwork(0);
    setSplitWallet(0);
    setSplitCredit(0);
    setBankRefNo('');
    setCashTenderedInput(grandTotal.toString());
    setPaidCashInput(grandTotal.toString());

    if (autoDefaultCashCustomer) {
      setSelectedCustomerId('CUST-001');
    }
    setShowCheckoutModal(true);
  }, [cart.length, grandTotal, autoDefaultCashCustomer, showError]);

  // Add item to cart by object
  const handleAddToCart = useCallback((product: ProductItem, unitName?: string) => {
    const unit = product.units?.find((u) => u.name === unitName) || product.units?.[0] || {
      name: product.unit,
      factor: 1,
      price: product.salePrice,
      cost: product.costPrice,
      barcode: product.barcode,
    };

    setCart((prev) => {
      const existingIndex = prev.findIndex(
        (ci) => ci.item.id === product.id && ci.selectedUnit === unit.name
      );
      if (existingIndex > -1) {
        const updated = [...prev];
        const newQty = updated[existingIndex].quantity + 1;
        updated[existingIndex] = {
          ...updated[existingIndex],
          quantity: newQty,
          total: (newQty * updated[existingIndex].unitPrice) - updated[existingIndex].discount,
        };
        return updated;
      } else {
        const newItem: CartItem = {
          item: product,
          quantity: 1,
          selectedUnit: unit.name,
          unitPrice: unit.price,
          unitCost: unit.cost,
          discount: 0,
          taxRate: 0,
          total: unit.price,
        };
        return [...prev, newItem];
      }
    });

    showToast(`تمت إضافة [${product.name}] للسلة`);
    setBarcodeInput('');
  }, [showToast]);

  // Handle Barcode Scan or Enter Key
  const handleBarcodeSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const query = barcodeInput.trim();
    if (!query) return;

    // Find product matching barcode or code or name
    const foundProduct = products.find(
      (p) =>
        p.barcode === query ||
        p.code === query ||
        p.units?.some((u) => u.barcode === query) ||
        p.name.toLowerCase() === query.toLowerCase()
    );

    if (foundProduct) {
      // Find matching unit by barcode if specific unit barcode scanned
      const matchedUnit = foundProduct.units?.find((u) => u.barcode === query);
      handleAddToCart(foundProduct, matchedUnit?.name);
      setBarcodeInput('');
      if (barcodeInputRef.current) {
        barcodeInputRef.current.focus();
      }
    } else {
      showError(
        `لم يتم العثور على أي صنف بالباركود أو الكود المدخل: [ ${query} ]!\nيرجى التأكد من مسح الباركود بشكل سليم أو الضغط على [F3] للبحث في شجرة الأصناف.`,
        'خطأ في قراءة الباركود (Barcode Not Found)',
        'pos-barcode-input',
        `// WinForms C# Error Handler:
try {
    var product = _productService.GetByBarcode("${query}");
    if (product == null) {
        MessageBox.Show("لم يتم العثور على الصنف!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        txtBarcode.Focus();
        txtBarcode.SelectAll();
    }
}`
      );
    }
  };

  // Update item quantity
  const updateQuantity = (index: number, delta: number) => {
    setCart((prev) => {
      const updated = [...prev];
      const newQty = Math.max(1, updated[index].quantity + delta);
      updated[index] = {
        ...updated[index],
        quantity: newQty,
        total: (newQty * updated[index].unitPrice) - updated[index].discount,
      };
      return updated;
    });
  };

  // Set explicit quantity
  const setExplicitQuantity = (index: number, qty: number) => {
    const validQty = isNaN(qty) || qty < 1 ? 1 : qty;
    setCart((prev) => {
      const updated = [...prev];
      updated[index] = {
        ...updated[index],
        quantity: validQty,
        total: (validQty * updated[index].unitPrice) - updated[index].discount,
      };
      return updated;
    });
  };

  // Change Unit
  const handleUnitChange = (index: number, newUnitName: string) => {
    setCart((prev) => {
      const updated = [...prev];
      const cartItem = updated[index];
      const product = cartItem.item;
      const unitDef = product.units?.find((u) => u.name === newUnitName);

      if (unitDef) {
        updated[index] = {
          ...cartItem,
          selectedUnit: unitDef.name,
          unitPrice: unitDef.price,
          unitCost: unitDef.cost,
          total: (cartItem.quantity * unitDef.price) - cartItem.discount,
        };
      }
      return updated;
    });
  };

  // Update Unit Price (Flexible Selling Price & Margin Control)
  const updateUnitPrice = (index: number, newPrice: number) => {
    const validPrice = isNaN(newPrice) || newPrice < 0 ? 0 : newPrice;
    setCart((prev) => {
      const updated = [...prev];
      const item = updated[index];
      const newTotal = Math.max(0, (item.quantity * validPrice) - item.discount);
      updated[index] = {
        ...item,
        unitPrice: validPrice,
        total: newTotal,
      };
      return updated;
    });
  };

  // Remove item
  const handleRemoveItem = useCallback((index: number) => {
    setCart((prev) => prev.filter((_, i) => i !== index));
    setSelectedRowIndex((prev) => (prev === index ? -1 : prev));
  }, []);

  // Clear Cart
  const handleClearCart = () => {
    if (cart.length > 0 && confirm('هل أنت متأكد من مسح كافة الأصناف في الفاتورة؟')) {
      setCart([]);
      setDiscountOverall(0);
      setPaidCashInput('');
      setSelectedRowIndex(-1);
    }
  };

  // Hold current order (تعليق الفاتورة السريع)
  const handleHoldCurrentOrder = useCallback(() => {
    if (cart.length === 0) {
      showToast('لا توجد أصناف في السلة لتعليق الفاتورة!');
      return;
    }
    const held: HeldOrder = {
      id: generateNextId('HOLD'),
      heldAt: getCurrentTimeString(),
      customerName: selectedCustomer.name,
      customerId: selectedCustomer.id,
      items: [...cart],
      grandTotal: grandTotal,
      discountOverall: discountOverall,
      discountType: discountType,
      notes: `فاتورة معلقة (${cart.length} أصناف) - إجمالي: ${grandTotal.toLocaleString()} ${currency}`,
    };
    onHoldOrder(held);
    setCart([]);
    setDiscountOverall(0);
    setPaidCashInput('');
    showToast(`تم تعليق فاتورة [${selectedCustomer.name}] بنجاح [F4]`);
  }, [cart, selectedCustomer, grandTotal, currency, discountOverall, discountType, onHoldOrder, showToast]);

  // Recall held order (استرجاع الفاتورة المعلقة)
  const handleRecallHeldOrder = useCallback((heldId: string) => {
    if (cart.length > 0) {
      if (!confirm('تنبيه: السلة الحالية تحتوي على أصناف! هل تريد استبدالها بالفاتورة المعلقة؟ (يمكنك تعليق الحالية أولاً بالضغط على إلغاء ثم F4)')) {
        return;
      }
    }
    const targetHeld = heldOrders.find((h) => h.id === heldId);
    const items = onRecallOrder(heldId);
    if (items && items.length > 0) {
      setCart(items);
      if (targetHeld?.customerId) {
        setSelectedCustomerId(targetHeld.customerId);
      }
      if (targetHeld?.discountOverall !== undefined) {
        setDiscountOverall(targetHeld.discountOverall);
      }
      if (targetHeld?.discountType) {
        setDiscountType(targetHeld.discountType);
      }
      setShowHeldModal(false);
      showToast(`تم استرجاع الفاتورة المعلقة بنجاح (${items.length} أصناف)`);
    }
  }, [cart.length, heldOrders, onRecallOrder, showToast]);

  // Finish Checkout (دفع عادي أو دفع فوري أو دفع مجزأ نقد + شبكة + آجل)
  const handleCheckoutSubmit = async () => {
    if (cart.length === 0) {
      await showError(
        'السلة فارغة، يرجى إضافة أصناف أولاً قبل إتمام عملية البيع!',
        'خطأ في التحقق من الفاتورة',
        'pos-barcode-input'
      );
      return;
    }

    let cashAmt = 0;
    let networkAmt = 0;
    let walletAmt = 0;
    let creditAmt = 0;

    if (paymentMainMode === 'CASH') {
      cashAmt = splitCash;
      networkAmt = splitNetwork;
      walletAmt = 0;
      creditAmt = 0;
    } else {
      // CREDIT Mode: composed of Cash + Bank + Credit remainder
      cashAmt = splitCash;
      networkAmt = splitNetwork;
      walletAmt = splitWallet;
      creditAmt = splitCredit;
    }

    const totalAllocated = cashAmt + networkAmt + walletAmt + creditAmt;
    if (totalAllocated < grandTotal) {
      await showError(
        `المبلغ الموزع (${totalAllocated.toLocaleString()} ${currency}) أقل من إجمالي الفاتورة (${grandTotal.toLocaleString()} ${currency})!\nيرجى استكمال كامل قيمة الفاتورة أو توزيع المتبقي كآجل قبل المتابعة.`,
        'عدم اكتمال سداد الفاتورة (Payment Incomplete)',
        'pos-split-cash-input'
      );
      return;
    }

    // Check if credit requires registered customer
    if (creditAmt > 0) {
      if (selectedCustomer.id === 'CUST-001' && (selectedCustomer.name.includes('نقدي') || selectedCustomer.name.includes('عام'))) {
        await showError(
          'تنبيه رقابة الائتمان: يوجد مبلغ آجل (متبقي من الفاتورة)، ولا يمكن تسجيل مبيعات آجلة لحساب (عميل نقدي عام)!\nيرجى اختيار حساب عميل معرف في الدليل من القائمة لربط المديونية به.',
          'حظر بيع آجل لعميل عام (Credit Customer Required)',
          'pos-customer-select'
        );
        return;
      }

      if (selectedCustomer.creditLimit > 0 && (selectedCustomer.currentBalance + creditAmt) > selectedCustomer.creditLimit) {
        const confirmed = await showConfirm(
          `تنبيه ائتماني: الجزء الآجل (${creditAmt.toLocaleString()} ${currency}) يرفع مديونية العميل (${selectedCustomer.name}) إلى (${(selectedCustomer.currentBalance + creditAmt).toLocaleString()} ${currency}) متجاوزاً سقف الائتمان المسموح به (${selectedCustomer.creditLimit.toLocaleString()} ${currency}).\nهل ترغب في تجاوز السقف الائتماني وترحيل الفاتورة؟`,
          'تجاوز سقف الائتمان (Credit Limit Exceeded)'
        );
        if (!confirmed) {
          return;
        }
      }
    }

    const hasMultiTender = (cashAmt > 0 && networkAmt > 0) || (creditAmt > 0 && (cashAmt > 0 || networkAmt > 0 || walletAmt > 0)) || walletAmt > 0;
    const finalPaymentType: PaymentType = hasMultiTender 
      ? 'MIXED' 
      : creditAmt === grandTotal 
      ? 'CREDIT' 
      : networkAmt === grandTotal 
      ? 'KURAYMI' 
      : walletAmt === grandTotal 
      ? 'JEB_WALLET' 
      : 'CASH';

    // Calculate proportional line allocation for header discount & tax
    const lineNetSubtotals = cart.map(item => Math.max(0, (item.quantity * item.unitPrice) - item.discount));
    const sumLineNets = lineNetSubtotals.reduce((a, b) => a + b, 0);

    const enrichedCart = cart.map((item, idx) => {
      const lineNet = lineNetSubtotals[idx];
      const ratio = sumLineNets > 0 ? lineNet / sumLineNets : 1 / (cart.length || 1);
      const allocatedHeaderDisc = calculatedOverallDiscount * ratio;
      const totalLineDisc = item.discount + allocatedHeaderDisc;
      const allocatedTaxAmount = calculatedTax * ratio;
      const netUnitPriceVal = item.quantity > 0 ? Math.max(0, (lineNet - allocatedHeaderDisc) / item.quantity) : 0;

      return {
        ...item,
        allocatedDiscount: Number(totalLineDisc.toFixed(4)),
        allocatedTax: Number(allocatedTaxAmount.toFixed(4)),
        netUnitPrice: Number(netUnitPriceVal.toFixed(4)),
      };
    });

    const newInvId = generateNextId('INV');
    const newInvoice: Invoice = {
      id: newInvId,
      invoiceNo: generateSalesInvoiceNumber(),
      type: 'SALE',
      date: getCurrentDateString(),
      time: getCurrentTimeString(),
      customerOrSupplierId: selectedCustomer.id,
      customerOrSupplierName: selectedCustomer.name,
      items: enrichedCart,
      subtotal,
      discountTotal: totalDiscount,
      taxTotal: calculatedTax,
      otherCharges: otherCharges > 0 ? otherCharges : undefined,
      otherChargesName: otherCharges > 0 ? otherChargesName : undefined,
      otherChargesAccountCode: otherCharges > 0 ? otherChargesAccountCode : undefined,
      otherChargesAccountName: otherCharges > 0 ? otherChargesName : undefined,
      grandTotal,
      paidAmount: cashAmt + networkAmt + walletAmt,
      remainingAmount: creditAmt,
      paymentType: finalPaymentType,
      currency: activeCurrency,
      exchangeRate: exchangeRate,
      treasuryAccountCode: cashAmt > 0 ? currentCashAccount.code : (networkAmt > 0 ? currentBankAccount.code : undefined),
      treasuryAccountName: cashAmt > 0 ? currentCashAccount.name : (networkAmt > 0 ? currentBankAccount.name : undefined),
      splitPayments: {
        cash: cashAmt,
        cashAccountCode: cashAmt > 0 ? currentCashAccount.code : undefined,
        cashAccountName: cashAmt > 0 ? currentCashAccount.name : undefined,
        network: networkAmt,
        bankAccountCode: networkAmt > 0 ? currentBankAccount.code : undefined,
        bankAccountName: networkAmt > 0 ? currentBankAccount.name : undefined,
        bankRefNo: (networkAmt > 0 && bankRefNo.trim()) ? bankRefNo.trim() : undefined,
        wallet: walletAmt,
        walletAccountCode: walletAmt > 0 ? '1122' : undefined,
        walletAccountName: walletAmt > 0 ? 'محفظة جيب الإلكترونية' : undefined,
        credit: creditAmt,
        creditCustomerId: creditAmt > 0 ? selectedCustomer.id : undefined,
        creditCustomerName: creditAmt > 0 ? selectedCustomer.name : undefined,
        notes: `تفاصيل السداد: ${cashAmt > 0 ? `كاش [${currentCashAccount.name}]: ${cashAmt.toLocaleString()}` : ''} ${networkAmt > 0 ? `+ بنك [${currentBankAccount.name}]: ${networkAmt.toLocaleString()}` : ''} ${creditAmt > 0 ? `+ آجل على (${selectedCustomer.name}): ${creditAmt.toLocaleString()}` : ''}`,
      },
      cashierName,
      notes: `فاتورة بيع نقطة بيع POS - ${finalPaymentType}`,
    };

    onCompleteSale(newInvoice);
    setShowCheckoutModal(false);
    setCart([]);
    setDiscountOverall(0);
    setTaxPercentVal('0');
    setIsTaxEnabled(false);
    setOtherChargesVal('0');
    setPaidCashInput('');
    setBankRefNo('');
    
    await showSuccess(
      `تم حفظ وترحيل فاتورة المبيعات رقم [ #${newInvoice.invoiceNo} ] بنجاح!\nالإجمالي: ${grandTotal.toLocaleString()} ${currency}\n• تم خصم المخزون بنظام (FIFO).\n• تم ترحيل القيد المحاسبي المتوازن في الدفتر العام.`,
      'نجاح ترحيل الفاتورة (Invoice Posted)',
      'pos-barcode-input'
    );
    onRequestPrintReceipt(newInvoice);
  };

  // Keyboard shortcut listener
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'F1') {
        e.preventDefault();
        barcodeInputRef.current?.focus();
      } else if (e.key === 'F2') {
        e.preventDefault();
        barcodeInputRef.current?.focus();
        barcodeInputRef.current?.select();
      } else if (e.key === 'F3') {
        e.preventDefault();
        if (cart.length > 0) handleOpenCheckout();
      } else if (e.key === 'F4') {
        e.preventDefault();
        if (cart.length > 0) {
          handleHoldCurrentOrder();
        } else if (heldOrders.length > 0) {
          setShowHeldModal(true);
        }
      } else if (e.key === 'F7') {
        e.preventDefault();
        setShowDiscountModal(true);
      } else if (e.key === 'Delete' && selectedRowIndex >= 0 && selectedRowIndex < cart.length) {
        e.preventDefault();
        handleRemoveItem(selectedRowIndex);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [cart.length, selectedRowIndex, heldOrders.length, handleHoldCurrentOrder, handleRecallHeldOrder, handleRemoveItem, handleOpenCheckout]);

  // Filtered held orders for search in recall modal
  const filteredHeldOrders = heldOrders.filter((h) => 
    heldSearchQuery === '' || 
    h.customerName.toLowerCase().includes(heldSearchQuery.toLowerCase()) ||
    h.id.toLowerCase().includes(heldSearchQuery.toLowerCase()) ||
    h.notes?.toLowerCase().includes(heldSearchQuery.toLowerCase())
  );

  return (
    <div className="flex-1 flex flex-col h-full bg-[#eef2f6] text-slate-800 p-2 gap-2 select-none overflow-hidden">
      {/* System Toast Notification */}
      {systemNotification && (
        <div className="fixed top-12 left-1/2 -translate-x-1/2 z-50 bg-slate-900 text-emerald-400 px-4 py-2 rounded-lg shadow-xl border border-emerald-500/50 flex items-center gap-2 text-sm font-semibold animate-bounce">
          <CheckCircle2 className="w-4 h-4 text-emerald-400" />
          <span>{systemNotification}</span>
        </div>
      )}

      {/* Top POS Action & Barcode Strip (Fast Keyboard First Bar + View Modes) */}
      <div className="bg-white p-2 sm:p-2.5 rounded-lg border border-slate-300 shadow-sm flex flex-wrap items-center justify-between gap-2 shrink-0">
        
        {/* View Mode Switcher (مكتبي سريع | ويب ولمس | هاتف محمول) */}
        <div className="flex items-center bg-slate-100 p-1 rounded-lg border border-slate-300">
          <button
            type="button"
            id="btn-mode-desktop"
            onClick={() => setPosLayoutMode('DESKTOP')}
            className={`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              posLayoutMode === 'DESKTOP'
                ? 'bg-[#0f2d4a] text-white shadow-xs'
                : 'text-slate-600 hover:text-slate-900 hover:bg-slate-200'
            }`}
            title="وضع النظام المكتبي السريع - شاشة واحدة مدمجة مع لوحة السداد والترحيل المباشرة"
          >
            <Monitor className="w-3.5 h-3.5" />
            <span>🖥️ مكتبي سريع (All-in-One)</span>
          </button>

          <button
            type="button"
            id="btn-mode-web"
            onClick={() => setPosLayoutMode('WEB')}
            className={`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              posLayoutMode === 'WEB'
                ? 'bg-sky-700 text-white shadow-xs'
                : 'text-slate-600 hover:text-slate-900 hover:bg-slate-200'
            }`}
            title="وضع الويب وشاشات اللمس - استعراض مرئي للأصناف والتصنيفات"
          >
            <Globe className="w-3.5 h-3.5" />
            <span>🌐 ويب ولمس (Touch POS)</span>
          </button>

          <button
            type="button"
            id="btn-mode-mobile"
            onClick={() => setPosLayoutMode('MOBILE')}
            className={`px-2.5 py-1 rounded text-xs font-bold transition flex items-center gap-1.5 cursor-pointer ${
              posLayoutMode === 'MOBILE'
                ? 'bg-emerald-700 text-white shadow-xs'
                : 'text-slate-600 hover:text-slate-900 hover:bg-slate-200'
            }`}
            title="وضع الهاتف المحمول - تصميم رأسي مخصص للجوال والأجهزة الكفية"
          >
            <Smartphone className="w-3.5 h-3.5" />
            <span>📱 هاتف محمول (Mobile)</span>
          </button>
        </div>

        {/* Barcode & Fast Search Input */}
        <form onSubmit={handleBarcodeSubmit} className="flex-1 flex items-center gap-2 min-w-[280px]">
          <div className="relative flex-1">
            <div className="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none text-slate-400">
              <Barcode className="w-5 h-5 text-sky-700" />
            </div>
            <input
              ref={barcodeInputRef}
              id="pos-barcode-input"
              type="text"
              placeholder="امسح الباركود أو اكتب اسم/كود الصنف ثم اضغط Enter [F2]..."
              value={barcodeInput}
              onChange={(e) => setBarcodeInput(e.target.value)}
              className="w-full pl-3 pr-10 py-1.5 sm:py-2 bg-slate-50 border-2 border-sky-600 focus:border-sky-700 rounded text-xs sm:text-sm font-medium text-slate-900 focus:outline-none focus:bg-white shadow-inner"
              autoFocus
            />
          </div>
          <button
            type="submit"
            id="btn-pos-add-barcode"
            className="bg-sky-700 hover:bg-sky-800 text-white px-3 sm:px-4 py-1.5 sm:py-2 rounded text-xs font-bold transition flex items-center gap-1 shadow cursor-pointer"
          >
            <Plus className="w-4 h-4" />
            <span className="hidden sm:inline">إضافة [Enter]</span>
          </button>
        </form>

        {/* Customer Selector & Held Invoices Button */}
        <div className="flex items-center gap-2">
          {/* Held Orders Quick Access Button */}
          <button
            type="button"
            id="btn-pos-held-orders-top"
            onClick={() => setShowHeldModal(true)}
            className={`px-3 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 border cursor-pointer ${
              heldOrders.length > 0
                ? 'bg-amber-500 hover:bg-amber-600 text-white border-amber-600 shadow-sm animate-pulse'
                : 'bg-slate-100 hover:bg-slate-200 text-slate-700 border-slate-300'
            }`}
            title="عرض الفواتير المعلقة واسترجاعها [F4]"
          >
            <PauseCircle className="w-4 h-4" />
            <span className="hidden md:inline">الفواتير المعلقة</span>
            <span className={`px-1.5 py-0.2 rounded-full text-[11px] font-black font-mono-numbers ${
              heldOrders.length > 0 ? 'bg-amber-900 text-amber-100' : 'bg-slate-300 text-slate-700'
            }`}>
              {heldOrders.length}
            </span>
          </button>

          {/* Currency Switcher Quick Button */}
          <button
            type="button"
            id="btn-pos-currency-select"
            onClick={() => setShowCurrencyModal(true)}
            className="px-2.5 py-1.5 rounded-lg text-xs font-bold transition flex items-center gap-1.5 border border-sky-300 bg-sky-50 hover:bg-sky-100 text-sky-900 shadow-2xs cursor-pointer"
            title="تغيير عملة الفاتورة وتعديل سعر الصرف"
          >
            <Coins className="w-4 h-4 text-sky-700" />
            <span className="font-bold">{activeCurrency}</span>
            {activeCurrency !== 'محلي' && exchangeRate !== 1 && (
              <span className="text-[10px] bg-sky-200 text-sky-800 px-1 py-0.2 rounded font-mono-numbers">
                @{exchangeRate}
              </span>
            )}
          </button>

          <div className="flex items-center gap-1.5 bg-slate-100 p-1 rounded border border-slate-300">
            <UserCheck className="w-4 h-4 text-slate-600" />
            <span className="text-xs font-semibold text-slate-700 hidden sm:inline">العميل:</span>
            <select
              id="pos-customer-select"
              value={selectedCustomerId}
              onChange={(e) => setSelectedCustomerId(e.target.value)}
              className="bg-white border border-slate-300 rounded text-xs py-1 px-2 font-semibold text-slate-900 focus:outline-none focus:border-sky-500 max-w-[150px] sm:max-w-none"
            >
              {customers.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name} {c.currentBalance > 0 ? `(عليه: ${c.currentBalance.toLocaleString()} ${currency})` : ''}
                </option>
              ))}
            </select>

            {selectedCustomer.currentBalance > 0 && (
              <span className="hidden lg:inline bg-amber-100 text-amber-900 text-[11px] font-bold px-2 py-0.5 rounded border border-amber-300">
                مديونية: {selectedCustomer.currentBalance.toLocaleString()} {currency}
              </span>
            )}
          </div>
        </div>
      </div>

      {/* ========================================================================= */}
      {/* 🖥️ VIEW 1: DESKTOP ERP / POS TERMINAL MODE (ALL-IN-ONE SINGLE SCREEN)     */}
      {/* ========================================================================= */}
      {posLayoutMode === 'DESKTOP' && (() => {
        const desktopCash = splitCash;
        const desktopBank = splitNetwork;
        const desktopSettled = desktopCash + desktopBank;
        const desktopCredit = paymentMainMode === 'CREDIT' ? splitCredit : 0;
        const isCashCustomerSelected = selectedCustomer.id === 'CUST-001' || selectedCustomer.name.includes('نقدي') || selectedCustomer.name.includes('عام');
        const hasCreditObligation = paymentMainMode === 'CREDIT' && splitCredit > 0;
        const fastCashGiven = parseFloat(cashTenderedInput) || desktopCash;
        const fastCashChange = Math.max(0, fastCashGiven - desktopCash);

        return (
          <div className="flex-1 grid grid-cols-1 lg:grid-cols-12 gap-2 min-h-0 overflow-hidden">
            {/* Left 62%: DataGridView Item Table */}
            <div className="lg:col-span-7 xl:col-span-8 flex flex-col bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden">
              {/* Grid Toolbar */}
              <div className="bg-slate-100 border-b border-slate-300 text-slate-800 px-3 py-2 flex items-center justify-between text-xs font-semibold shrink-0">
                <div className="flex items-center gap-2">
                  <ShoppingBag className="w-4 h-4 text-sky-700" />
                  <span className="text-slate-900 font-bold">جدول أصناف الفاتورة (DataGridView - Fast Entry)</span>
                  <span className="bg-sky-100 text-sky-800 border border-sky-200 font-bold px-2 py-0.5 rounded text-[11px]">
                    {cart.length} أصناف
                  </span>
                </div>
                
                <div className="flex items-center gap-2">
                  <button
                    type="button"
                    onClick={() => setShowDesktopProductPicker(!showDesktopProductPicker)}
                    className={`px-2.5 py-1 rounded text-xs flex items-center gap-1 transition cursor-pointer font-bold ${
                      showDesktopProductPicker ? 'bg-sky-600 text-white' : 'bg-white hover:bg-slate-200 text-slate-700 border border-slate-300 shadow-2xs'
                    }`}
                    title="إظهار / إخفاء دليل البحث السريع عن الأصناف"
                  >
                    <LayoutGrid className="w-3.5 h-3.5" />
                    <span>{showDesktopProductPicker ? 'إخفاء الدليل' : 'دليل الأصناف [F2]'}</span>
                  </button>

                  <button
                    type="button"
                    id="btn-pos-desktop-hold"
                    onClick={handleHoldCurrentOrder}
                    className="bg-amber-600 hover:bg-amber-500 text-white px-2.5 py-1 rounded text-xs flex items-center gap-1 transition cursor-pointer font-bold shadow-2xs"
                    title="تعليق الفاتورة الحالية [F4]"
                  >
                    <PauseCircle className="w-3.5 h-3.5" />
                    <span>تعليق [F4]</span>
                  </button>

                  <button
                    type="button"
                    onClick={handleClearCart}
                    disabled={cart.length === 0}
                    className="bg-rose-100 hover:bg-rose-200 text-rose-700 border border-rose-200 disabled:opacity-50 px-2 py-1 rounded text-xs flex items-center gap-1 transition cursor-pointer font-bold"
                    title="مسح كافة الأصناف"
                  >
                    <Trash2 className="w-3.5 h-3.5" />
                    <span>مسح الكل</span>
                  </button>
                </div>
              </div>

              {/* Optional Collapsible Product Quick Picker Drawer */}
              {showDesktopProductPicker && (
                <div className="bg-slate-100 border-b border-slate-300 p-2 max-h-48 overflow-y-auto grid grid-cols-2 sm:grid-cols-4 gap-2 shrink-0 animate-in slide-in-from-top-2">
                  {products.slice(0, 12).map((p) => (
                    <button
                      key={p.id}
                      type="button"
                      onClick={() => handleAddToCart(p)}
                      className="bg-white border border-slate-200 hover:border-sky-500 p-1.5 rounded text-right flex flex-col justify-between hover:shadow-xs transition text-xs"
                    >
                      <span className="font-bold text-slate-900 truncate">{p.name}</span>
                      <div className="flex items-center justify-between text-[11px] text-sky-800 font-mono-numbers mt-1">
                        <span>{p.salePrice.toLocaleString()} {currency}</span>
                        <span className="text-slate-400 text-[10px]">مخزون: {p.stock}</span>
                      </div>
                    </button>
                  ))}
                </div>
              )}

              {/* Desktop Table View */}
              <div className="flex-1 overflow-auto border-b border-slate-300 bg-white">
                <table className="w-full text-right border-collapse datagrid-table">
                  <thead className="sticky top-0 z-10">
                    <tr>
                      <th className="w-10 text-center">#</th>
                      <th className="w-24">كود الصنف</th>
                      <th>اسم الصنف والبيان</th>
                      <th className="w-28 text-center">الوحدة</th>
                      <th className="w-32 text-center">الكمية</th>
                      <th className="w-28 text-left">السعر ({currency})</th>
                      <th className="w-24 text-left">الخصم</th>
                      <th className="w-32 text-left">الإجمالي ({currency})</th>
                      <th className="w-12 text-center">حذف</th>
                    </tr>
                  </thead>
                  <tbody>
                    {cart.length === 0 ? (
                      <tr>
                        <td colSpan={9} className="text-center py-20 text-slate-400 bg-slate-50/50">
                          <div className="flex flex-col items-center justify-center gap-2">
                            <Barcode className="w-10 h-10 text-slate-300" />
                            <span className="text-sm font-semibold text-slate-600">الفاتورة فارغة حالياً</span>
                            <span className="text-xs text-slate-400">
                              امسح الباركود مباشرة من القارئ أو اضغط Enter للإضافة السريعة
                            </span>
                          </div>
                        </td>
                      </tr>
                    ) : (
                      cart.map((cartItem, idx) => {
                        const isSelected = selectedRowIndex === idx;
                        return (
                          <tr
                            key={`${cartItem.item.id}-${cartItem.selectedUnit}-${idx}`}
                            onClick={() => setSelectedRowIndex(idx)}
                            className={`cursor-pointer transition-colors border-b border-slate-200 ${
                              isSelected ? 'bg-sky-100 font-semibold' : idx % 2 === 0 ? 'bg-white' : 'bg-slate-50/70'
                            }`}
                          >
                            <td className="text-center font-mono-numbers text-slate-500 font-bold text-xs">{idx + 1}</td>
                            <td className="font-mono-numbers text-xs text-slate-700">{cartItem.item.code}</td>
                            <td className="font-semibold text-slate-900">
                              <div className="flex flex-col">
                                <span>{cartItem.item.name}</span>
                                {cartItem.item.hasExpiryDate !== false && cartItem.item.expiryDate && (
                                  <span className="text-[10px] text-slate-500">انتهاء: {cartItem.item.expiryDate}</span>
                                )}
                              </div>
                            </td>
                            
                            {/* Unit selector */}
                            <td className="text-center">
                              {cartItem.item.units && cartItem.item.units.length > 1 ? (
                                <select
                                  value={cartItem.selectedUnit}
                                  onChange={(e) => handleUnitChange(idx, e.target.value)}
                                  className="bg-white border border-slate-300 rounded text-xs py-0.5 px-1 font-medium focus:border-sky-600 focus:outline-none"
                                  onClick={(e) => e.stopPropagation()}
                                >
                                  {cartItem.item.units.map((u) => (
                                    <option key={u.name} value={u.name}>
                                      {u.name} ({u.price.toLocaleString()})
                                    </option>
                                  ))}
                                </select>
                              ) : (
                                <span className="text-xs bg-slate-200 text-slate-700 px-2 py-0.5 rounded font-medium">
                                  {cartItem.selectedUnit}
                                </span>
                              )}
                            </td>

                            {/* Quantity Spinner */}
                            <td className="text-center" onClick={(e) => e.stopPropagation()}>
                              <div className="inline-flex items-center justify-center border border-slate-300 rounded bg-white overflow-hidden shadow-xs">
                                <button
                                  type="button"
                                  onClick={() => updateQuantity(idx, -1)}
                                  className="p-1 text-slate-600 hover:bg-slate-100 active:bg-slate-200"
                                  title="تقليل الكمية"
                                >
                                  <Minus className="w-3.5 h-3.5" />
                                </button>
                                <input
                                  type="number"
                                  min={1}
                                  value={cartItem.quantity}
                                  onChange={(e) => setExplicitQuantity(idx, parseInt(e.target.value, 10))}
                                  className="w-12 text-center text-xs font-bold font-mono-numbers text-slate-900 focus:outline-none border-x border-slate-200 py-0.5"
                                />
                                <button
                                  type="button"
                                  onClick={() => updateQuantity(idx, 1)}
                                  className="p-1 text-slate-600 hover:bg-slate-100 active:bg-slate-200"
                                  title="زيادة الكمية"
                                >
                                  <Plus className="w-3.5 h-3.5" />
                                </button>
                              </div>
                            </td>

                            {/* Unit Price */}
                            <td className="text-left" onClick={(e) => e.stopPropagation()}>
                              <div className="flex flex-col items-start">
                                <input
                                  type="number"
                                  min={0}
                                  value={cartItem.unitPrice}
                                  onChange={(e) => updateUnitPrice(idx, parseFloat(e.target.value) || 0)}
                                  className={`w-20 text-left font-mono-numbers font-bold text-xs px-1.5 py-0.5 border rounded focus:outline-none focus:ring-1 ${
                                    cartItem.unitPrice < cartItem.unitCost
                                      ? 'border-rose-500 text-rose-700 bg-rose-50 focus:ring-rose-500'
                                      : 'border-slate-300 text-slate-800 bg-white focus:ring-sky-500'
                                  }`}
                                  title={`سعر الشراء / التكلفة للدفعة: ${cartItem.unitCost} ${currency}`}
                                />
                                {cartItem.unitPrice < cartItem.unitCost && (
                                  <span
                                    className="text-[9px] text-rose-600 font-bold flex items-center gap-0.5 mt-0.5 whitespace-nowrap"
                                    title={`تنبيه: سعر البيع أقل من تكلفة شراء الدفعة (${cartItem.unitCost} ${currency})`}
                                  >
                                    <AlertTriangle className="w-2.5 h-2.5 shrink-0" />
                                    <span>أقل من التكلفة ({cartItem.unitCost})</span>
                                  </span>
                                )}
                              </div>
                            </td>

                            {/* Discount */}
                            <td className="text-left font-mono-numbers text-rose-600 text-xs">
                              {cartItem.discount > 0 ? `-${cartItem.discount.toLocaleString()}` : '-'}
                            </td>

                            {/* Line Total */}
                            <td className="text-left font-mono-numbers font-extrabold text-sky-800 text-sm">
                              {cartItem.total.toLocaleString()}
                            </td>

                            {/* Delete Action */}
                            <td className="text-center" onClick={(e) => e.stopPropagation()}>
                              <button
                                type="button"
                                onClick={() => handleRemoveItem(idx)}
                                className="text-rose-500 hover:text-rose-700 hover:bg-rose-50 p-1 rounded transition"
                                title="حذف هذا الصنف [Delete]"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </td>
                          </tr>
                        );
                      })
                    )}
                  </tbody>
                </table>
              </div>

              {/* Bottom Quick Bar - Desktop / Web: DIRECT INLINE CONTROLS (لا حاجة لشاشة منبثقة) */}
              <div className="bg-slate-100 p-2 flex flex-wrap items-center justify-between gap-2 border-t border-slate-300 text-xs shrink-0">
                {/* Stats & Subtotal */}
                <div className="flex items-center gap-3">
                  <span className="text-slate-600">
                    القطع: <strong className="text-slate-900 font-mono-numbers">{cart.reduce((s, i) => s + i.quantity, 0)}</strong>
                  </span>
                  <span className="text-slate-600">
                    المجموع: <strong className="text-slate-900 font-mono-numbers">{subtotal.toLocaleString()} {currency}</strong>
                  </span>
                </div>

                {/* Inline Controls: Discount, Tax %, Other Charges */}
                <div className="flex flex-wrap items-center gap-2">
                  {/* 1. Header Discount Inline */}
                  <div className="flex items-center gap-1 bg-white px-1.5 py-0.5 rounded border border-slate-300">
                    <span className="text-slate-700 font-bold text-[11px]">خصم:</span>
                    <div className="inline-flex rounded border border-slate-200 overflow-hidden text-[10px] font-bold">
                      <button
                        type="button"
                        onClick={() => setDiscountType('AMOUNT')}
                        className={`px-1.5 py-0.5 ${discountType === 'AMOUNT' ? 'bg-rose-600 text-white' : 'bg-slate-100 text-slate-700'}`}
                      >
                        مبلغ
                      </button>
                      <button
                        type="button"
                        onClick={() => setDiscountType('PERCENT')}
                        className={`px-1.5 py-0.5 ${discountType === 'PERCENT' ? 'bg-rose-600 text-white' : 'bg-slate-100 text-slate-700'}`}
                      >
                        %
                      </button>
                    </div>
                    <input
                      type="number"
                      min={0}
                      placeholder="0"
                      value={discountOverall || ''}
                      onChange={(e) => setDiscountOverall(parseFloat(e.target.value) || 0)}
                      className="w-14 text-center py-0.5 text-xs font-mono-numbers text-rose-600 font-bold focus:outline-hidden"
                    />
                  </div>

                  {/* 2. Tax % Dropdown Inline */}
                  <div className="flex items-center gap-1 bg-white px-1.5 py-0.5 rounded border border-slate-300">
                    <span className="text-slate-700 font-bold text-[11px]">الضريبة%:</span>
                    <select
                      value={taxPercentVal}
                      onChange={(e) => {
                        setTaxPercentVal(e.target.value);
                        setIsTaxEnabled(parseFloat(e.target.value) > 0);
                      }}
                      className="text-center py-0.5 text-xs font-bold text-slate-800 bg-transparent focus:outline-hidden cursor-pointer"
                    >
                      <option value="0">0% (معفي)</option>
                      <option value="5">5%</option>
                      <option value="15">15%</option>
                    </select>
                  </div>

                  {/* 3. Expense Account Dropdown for Other Charges */}
                  <div className="flex items-center gap-1 bg-white px-1.5 py-0.5 rounded border border-slate-300">
                    <span className="text-slate-700 font-bold text-[11px]">حساب المصروف:</span>
                    <select
                      value={otherChargesAccountCode}
                      onChange={(e) => {
                        const code = e.target.value;
                        setOtherChargesAccountCode(code);
                        const match = EXPENSE_ACCOUNTS_OPTIONS.find(a => a.code === code);
                        if (match) setOtherChargesName(match.name);
                      }}
                      className="text-right py-0.5 text-xs font-bold text-slate-800 bg-transparent focus:outline-hidden cursor-pointer max-w-[170px]"
                    >
                      {EXPENSE_ACCOUNTS_OPTIONS.map((acc) => (
                        <option key={acc.code} value={acc.code}>
                          {acc.code} - {acc.name}
                        </option>
                      ))}
                    </select>
                    <input
                      type="number"
                      min={0}
                      placeholder="المبلغ (0)"
                      value={otherChargesVal || ''}
                      onChange={(e) => setOtherChargesVal(e.target.value)}
                      className="w-16 text-center py-0.5 text-xs font-mono-numbers text-sky-700 font-bold border-r border-slate-200 focus:outline-hidden"
                    />
                  </div>
                </div>
              </div>
            </div>

            {/* Right 38%: DIRECT ALL-IN-ONE INLINE SETTLEMENT CONSOLE (لوحة السداد الفوري المدمجة) */}
            <div className="lg:col-span-5 xl:col-span-4 flex flex-col bg-white rounded-lg border-2 border-slate-400 shadow-md overflow-hidden">
              {/* Grand Total Header */}
              <div className="bg-gradient-to-r from-[#0a233c] to-[#103a63] text-white p-3 flex items-center justify-between shadow-inner shrink-0">
                <div>
                  <span className="text-[11px] text-sky-200 block font-bold">المبلغ الصافي المستحق للدفع:</span>
                  <div className="flex items-baseline gap-1.5">
                    <span className="text-2xl xl:text-3xl font-black text-amber-300 font-mono-numbers tracking-tight">
                      {activeCurrency !== 'محلي' && exchangeRate > 0
                        ? (grandTotal / exchangeRate).toLocaleString(undefined, { maximumFractionDigits: 2 })
                        : grandTotal.toLocaleString()}
                    </span>
                    <button
                      type="button"
                      onClick={() => setShowCurrencyModal(true)}
                      className="text-xs bg-sky-900/80 hover:bg-sky-800 text-amber-200 hover:text-white px-2 py-0.5 rounded border border-sky-600/50 font-bold transition cursor-pointer flex items-center gap-1"
                      title="انقر لتغيير العملة وسعر الصرف"
                    >
                      <span>{activeCurrency}</span>
                      <Coins className="w-3 h-3 text-amber-300" />
                    </button>
                    {activeCurrency !== 'محلي' && exchangeRate > 0 && (
                      <span className="text-[10px] text-sky-300">
                        ({grandTotal.toLocaleString()} {currency})
                      </span>
                    )}
                  </div>
                </div>

                <div className="text-left bg-sky-950/60 px-2.5 py-1.5 rounded-lg border border-sky-800/80">
                  <span className="text-[10px] text-sky-300 block">حساب العميل</span>
                  <span className="text-xs font-bold text-white truncate block max-w-[140px]">{selectedCustomer.name}</span>
                </div>
              </div>

              {/* Settlement Form Controls Body */}
              <div className="p-3 flex flex-col gap-2.5 overflow-y-auto flex-1 text-xs bg-slate-50/50">
                {/* Payment Main Mode Switcher (وضع نقد ووضع آجل) */}
                <div className="grid grid-cols-2 gap-1.5 bg-slate-200 p-1 rounded-lg border border-slate-300 shrink-0">
                  <button
                    type="button"
                    onClick={() => {
                      setPaymentMainMode('CASH');
                      setSplitCash(grandTotal);
                      setSplitNetwork(0);
                      setSplitWallet(0);
                      setSplitCredit(0);
                      setCashTenderedInput(grandTotal.toString());
                      if (autoDefaultCashCustomer) setSelectedCustomerId('CUST-001');
                    }}
                    className={`py-2 px-2 rounded text-center text-xs font-black transition cursor-pointer flex items-center justify-center gap-1.5 ${
                      paymentMainMode === 'CASH'
                        ? 'bg-emerald-700 text-white shadow-sm'
                        : 'text-slate-700 hover:bg-slate-300'
                    }`}
                  >
                    <span>💵</span>
                    <span>1. وضع نقد (كاش + بنك)</span>
                  </button>

                  <button
                    type="button"
                    onClick={() => {
                      setPaymentMainMode('CREDIT');
                      setSplitCash(0);
                      setSplitNetwork(0);
                      setSplitWallet(0);
                      setSplitCredit(grandTotal);
                      if (selectedCustomer.id === 'CUST-001') {
                        const registeredCust = customers.find(c => c.id !== 'CUST-001');
                        if (registeredCust) setSelectedCustomerId(registeredCust.id);
                      }
                    }}
                    className={`py-2 px-2 rounded text-center text-xs font-black transition cursor-pointer flex items-center justify-center gap-1.5 ${
                      paymentMainMode === 'CREDIT'
                        ? 'bg-amber-700 text-white shadow-sm'
                        : 'text-slate-700 hover:bg-slate-300'
                    }`}
                  >
                    <span>📝</span>
                    <span>2. وضع آجل (كاش + بنك + آجل)</span>
                  </button>
                </div>

                {/* MODE 1: CASH MODE (كاش + بنك) */}
                {paymentMainMode === 'CASH' && (
                  <>
                    <div className="grid grid-cols-2 gap-2">
                      {/* 1. Cash Box & Amount */}
                      <div className="bg-emerald-50/80 p-2 rounded-lg border border-emerald-300 flex flex-col gap-1.5 shadow-xs">
                        <div className="flex items-center justify-between">
                          <label className="text-[10px] font-bold text-emerald-950 flex items-center gap-1">
                            <span>💵</span>
                            <span>كاش:</span>
                          </label>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - splitNetwork);
                              setSplitCash(rem);
                              setCashTenderedInput(rem.toString());
                            }}
                            className="text-[9px] text-emerald-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCash || ''}
                          onChange={(e) => {
                            const val = parseFloat(e.target.value) || 0;
                            setSplitCash(val);
                            // Auto-balance bank part in Cash mode so total matches grandTotal seamlessly
                            const autoBank = Math.max(0, grandTotal - val);
                            setSplitNetwork(autoBank);
                            setCashTenderedInput(val.toString());
                          }}
                          className="w-full bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedCashAccountCode}
                          onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                          disabled={splitCash <= 0}
                          className="w-full bg-white border border-emerald-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                          title="اختيار الصندوق من شجرة الحسابات"
                        >
                          {availableCashAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>
                              {acc.name}
                            </option>
                          ))}
                        </select>
                      </div>

                      {/* 2. Bank / Card Account */}
                      <div className="bg-sky-50/80 p-2 rounded-lg border border-sky-300 flex flex-col gap-1.5 shadow-xs">
                        <div className="flex items-center justify-between">
                          <label className="text-[10px] font-bold text-sky-950 flex items-center gap-1">
                            <Building2 className="w-3 h-3 text-sky-700" />
                            <span>بنك / شبكة:</span>
                          </label>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - splitCash);
                              setSplitNetwork(rem);
                            }}
                            className="text-[9px] text-sky-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitNetwork || ''}
                          onChange={(e) => {
                            const val = parseFloat(e.target.value) || 0;
                            setSplitNetwork(val);
                            // Auto-balance cash part when bank changes so total equals grandTotal
                            const autoCash = Math.max(0, grandTotal - val);
                            setSplitCash(autoCash);
                            setCashTenderedInput(autoCash.toString());
                          }}
                          className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedBankAccountCode}
                          onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                          disabled={splitNetwork <= 0}
                          className="w-full bg-white border border-sky-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                          title="اختيار الحساب البنكي من شجرة الحسابات"
                        >
                          {availableBankAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>
                              {acc.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    </div>

                    {/* Cash Mode Status */}
                    {desktopSettled === grandTotal ? (
                      <div className="bg-emerald-50 border border-emerald-300 p-2 rounded-lg text-emerald-900 flex items-center justify-between text-[11px] font-semibold">
                        <div className="flex items-center gap-1">
                          <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                          <span>مسدد بالكامل (كاش: {desktopCash.toLocaleString()} + بنك: {desktopBank.toLocaleString()})</span>
                        </div>
                        <span className="font-mono-numbers font-bold text-emerald-800">
                          {desktopSettled.toLocaleString()} {currency}
                        </span>
                      </div>
                    ) : (
                      <div className="bg-rose-50 border-2 border-rose-300 p-2 rounded-lg text-rose-900 flex items-center justify-between text-[11px] font-bold">
                        <div className="flex items-center gap-1">
                          <AlertTriangle className="w-4 h-4 text-rose-600" />
                          <span>فارق لم يتم تسديده نقدياً:</span>
                        </div>
                        <span className="font-mono-numbers text-rose-700 text-sm">
                          {(grandTotal - desktopSettled).toLocaleString()} {currency}
                        </span>
                      </div>
                    )}
                  </>
                )}

                {/* MODE 2: CREDIT MODE (كاش + بنك + آجل) */}
                {paymentMainMode === 'CREDIT' && (
                  <div className="flex flex-col gap-2.5">
                    {/* Customer Status Banner */}
                    <div className="bg-amber-50 p-2.5 rounded-lg border-2 border-amber-400 text-amber-950 flex flex-col gap-1.5 shadow-xs">
                      <div className="flex items-center justify-between border-b border-amber-200 pb-1">
                        <div className="flex items-center gap-1.5 font-bold text-xs">
                          <span>📝</span>
                          <span>العميل الآجل: <strong>{selectedCustomer.name}</strong></span>
                        </div>
                        <span className="text-[10px] text-amber-800 font-mono-numbers">
                          الرصيد الحالي: {selectedCustomer.currentBalance.toLocaleString()} {currency}
                        </span>
                      </div>

                      {isCashCustomerSelected && (
                        <div className="bg-rose-100 border border-rose-300 text-rose-900 px-2 py-1 rounded text-[10px] font-bold flex items-center gap-1">
                          <ShieldAlert className="w-3.5 h-3.5 text-rose-700 shrink-0" />
                          <span>تنبيه: لا يمكن حفظ فاتورة آجلة على (عميل نقدي عام). يرجى اختيار العميل من الشريط العلوي.</span>
                        </div>
                      )}
                    </div>

                    {/* 3 Channels: Cash + Bank + Credit */}
                    <div className="grid grid-cols-3 gap-1.5">
                      {/* 1. Cash Part */}
                      <div className="bg-emerald-50/80 p-2 rounded border border-emerald-300 flex flex-col gap-1">
                        <div className="flex items-center justify-between">
                          <label className="text-[10px] font-bold text-emerald-950">💵 كاش:</label>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitNetwork + splitCredit));
                              setSplitCash(rem);
                            }}
                            className="text-[9px] text-emerald-700 underline font-bold"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCash || ''}
                          onChange={(e) => {
                            const val = parseFloat(e.target.value) || 0;
                            setSplitCash(val);
                            // Auto-balance remaining credit debt
                            const remCredit = Math.max(0, grandTotal - (val + splitNetwork));
                            setSplitCredit(remCredit);
                          }}
                          className="w-full bg-white border border-emerald-400 rounded px-1.5 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedCashAccountCode}
                          onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                          disabled={splitCash <= 0}
                          className="bg-white border border-emerald-200 rounded px-1 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                        >
                          {availableCashAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>{acc.name}</option>
                          ))}
                        </select>
                      </div>

                      {/* 2. Bank Part */}
                      <div className="bg-sky-50/80 p-2 rounded border border-sky-300 flex flex-col gap-1">
                        <div className="flex items-center justify-between">
                          <label className="text-[10px] font-bold text-sky-950">🏦 بنك/شبكة:</label>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitCash + splitCredit));
                              setSplitNetwork(rem);
                            }}
                            className="text-[9px] text-sky-700 underline font-bold"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitNetwork || ''}
                          onChange={(e) => {
                            const val = parseFloat(e.target.value) || 0;
                            setSplitNetwork(val);
                            // Auto-balance remaining credit debt
                            const remCredit = Math.max(0, grandTotal - (splitCash + val));
                            setSplitCredit(remCredit);
                          }}
                          className="w-full bg-white border border-sky-400 rounded px-1.5 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedBankAccountCode}
                          onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                          disabled={splitNetwork <= 0}
                          className="bg-white border border-sky-200 rounded px-1 py-0.5 text-[10px] text-slate-800 disabled:opacity-50"
                        >
                          {availableBankAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>{acc.name}</option>
                          ))}
                        </select>
                      </div>

                      {/* 3. Credit Part */}
                      <div className="bg-amber-50/90 p-2 rounded border-2 border-amber-400 flex flex-col gap-1">
                        <div className="flex items-center justify-between">
                          <label className="text-[10px] font-bold text-amber-950">📝 آجل ذمة:</label>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitCash + splitNetwork));
                              setSplitCredit(rem);
                            }}
                            className="text-[9px] text-amber-800 underline font-bold"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCredit || ''}
                          onChange={(e) => setSplitCredit(parseFloat(e.target.value) || 0)}
                          className="w-full bg-white border-2 border-amber-500 rounded px-1.5 py-1 text-xs font-black font-mono-numbers text-amber-950 focus:outline-none"
                          placeholder="0"
                        />
                        <div className="text-[10px] text-amber-800 font-bold flex items-center justify-between">
                          <span>الرصيد بعد الفاتورة:</span>
                          <span className="font-mono-numbers">{(selectedCustomer.currentBalance + splitCredit).toLocaleString()}</span>
                        </div>
                      </div>
                    </div>

                    {/* Credit Mode Sum Check */}
                    <div className={`p-2 rounded-lg border flex items-center justify-between text-[11px] font-semibold ${
                      (splitCash + splitNetwork + splitCredit) === grandTotal
                        ? 'bg-emerald-50 border-emerald-300 text-emerald-900'
                        : 'bg-rose-50 border-rose-300 text-rose-900'
                    }`}>
                      <span>إجمالي (كاش + بنك + آجل): <strong className="font-mono-numbers">{(splitCash + splitNetwork + splitCredit).toLocaleString()}</strong></span>
                      {Math.abs(grandTotal - (splitCash + splitNetwork + splitCredit)) > 0 ? (
                        <span className="text-rose-700 font-bold">
                          فارق: {(grandTotal - (splitCash + splitNetwork + splitCredit)).toLocaleString()} {currency}
                        </span>
                      ) : (
                        <span className="text-emerald-700 font-bold">متطابق 100% ✓</span>
                      )}
                    </div>
                  </div>
                )}
              </div>

              {/* Direct Instant Execution Actions (NO POPUP REQUIRED) */}
              <div className="bg-slate-100 p-3 border-t-2 border-slate-300 flex items-center gap-2 shrink-0">
                <button
                  type="button"
                  id="btn-desktop-direct-save"
                  disabled={cart.length === 0 || (hasCreditObligation && isCashCustomerSelected)}
                  onClick={handleCheckoutSubmit}
                  className={`flex-1 font-black py-3 px-4 rounded-lg shadow-md hover:shadow-lg transition flex items-center justify-center gap-2 text-sm cursor-pointer ${
                    cart.length === 0 || (hasCreditObligation && isCashCustomerSelected)
                      ? 'bg-slate-400 text-slate-100 cursor-not-allowed'
                      : 'bg-emerald-600 hover:bg-emerald-500 text-white active:scale-[0.99] border border-emerald-400'
                  }`}
                  title="حفظ وترحيل الفاتورة وطباعة الوصل فوراً [Enter / F3]"
                >
                  <Printer className="w-5 h-5" />
                  <span>حفظ وطباعة الفاتورة [Enter / F3]</span>
                </button>
              </div>
            </div>
          </div>
        );
      })()}

      {/* ========================================================================= */}
      {/* 🌐 VIEW 2: WEB / TOUCH POS MODE (TOUCH CATALOG + FLOATING CHECKOUT)       */}
      {/* ========================================================================= */}
      {posLayoutMode === 'WEB' && (
        <div className="flex-1 grid grid-cols-1 lg:grid-cols-12 gap-2 min-h-0 overflow-hidden">
          {/* Left Side: Touch Cart List (50%) */}
          <div className="lg:col-span-6 xl:col-span-6 flex flex-col bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden">
            <div className="bg-slate-100 border-b border-slate-300 text-slate-800 px-3 py-2 flex items-center justify-between text-xs font-semibold shrink-0">
              <div className="flex items-center gap-2">
                <ShoppingBag className="w-4 h-4 text-sky-700" />
                <span className="text-slate-900 font-bold">سلة المبيعات واللمس (Touch Cart)</span>
                <span className="bg-sky-100 text-sky-800 border border-sky-200 font-bold px-2 py-0.5 rounded text-[11px]">{cart.length} أصناف</span>
              </div>
              <button
                type="button"
                onClick={handleClearCart}
                disabled={cart.length === 0}
                className="bg-rose-100 hover:bg-rose-200 text-rose-700 border border-rose-200 disabled:opacity-50 px-2 py-1 rounded text-xs flex items-center gap-1 transition cursor-pointer font-bold"
              >
                <Trash2 className="w-3.5 h-3.5" />
                <span>مسح</span>
              </button>
            </div>

            {/* Cart Items List */}
            <div className="flex-1 overflow-y-auto p-2 flex flex-col gap-1.5 bg-slate-50/50">
              {cart.length === 0 ? (
                <div className="h-full flex flex-col items-center justify-center text-slate-400 py-16">
                  <ShoppingBag className="w-12 h-12 text-slate-300 mb-2" />
                  <span className="text-sm font-bold text-slate-600">سلة المشتريات فارغة</span>
                  <span className="text-xs text-slate-400">انقر على الأصناف من القائمة المجاورة لإضافتها</span>
                </div>
              ) : (
                cart.map((cartItem, idx) => (
                  <div
                    key={`${cartItem.item.id}-${idx}`}
                    className="bg-white border border-slate-200 rounded-lg p-2.5 flex items-center justify-between shadow-xs"
                  >
                    <div className="flex-1 pr-2">
                      <h4 className="font-bold text-slate-900 text-xs">{cartItem.item.name}</h4>
                      <div className="flex items-center gap-2 text-[11px] text-slate-500 mt-0.5">
                        <span className="font-mono-numbers">{cartItem.unitPrice.toLocaleString()} {currency}</span>
                        <span>• {cartItem.selectedUnit}</span>
                      </div>
                    </div>

                    <div className="flex items-center gap-2">
                      <div className="inline-flex items-center border border-slate-300 rounded-lg bg-slate-50 overflow-hidden">
                        <button
                          type="button"
                          onClick={() => updateQuantity(idx, -1)}
                          className="px-2 py-1 text-slate-700 hover:bg-slate-200 active:bg-slate-300 font-bold"
                        >
                          -
                        </button>
                        <span className="px-2.5 py-1 text-xs font-black font-mono-numbers text-slate-900 bg-white border-x border-slate-300">
                          {cartItem.quantity}
                        </span>
                        <button
                          type="button"
                          onClick={() => updateQuantity(idx, 1)}
                          className="px-2 py-1 text-slate-700 hover:bg-slate-200 active:bg-slate-300 font-bold"
                        >
                          +
                        </button>
                      </div>

                      <span className="text-sm font-black text-sky-800 font-mono-numbers w-20 text-left">
                        {cartItem.total.toLocaleString()}
                      </span>

                      <button
                        type="button"
                        onClick={() => handleRemoveItem(idx)}
                        className="text-rose-500 hover:text-rose-700 p-1 rounded"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>
                ))
              )}
            </div>

            {/* Touch Cart Footer */}
            <div className="bg-slate-100 p-3 border-t-2 border-slate-300 flex flex-wrap items-center justify-between gap-2 shrink-0">
              <div className="space-y-0.5">
                <div className="flex items-center gap-2">
                  <span className="text-[11px] text-slate-500 block">الإجمالي الصافي:</span>
                  {calculatedTax > 0 && (
                    <span className="text-[10px] text-emerald-800 bg-emerald-100 px-1.5 py-0.2 rounded font-bold">
                      ضريبة ({taxPercentVal}%): {calculatedTax.toLocaleString()}
                    </span>
                  )}
                  {otherCharges > 0 && (
                    <span className="text-[10px] text-sky-800 bg-sky-100 px-1.5 py-0.2 rounded font-bold">
                      {otherChargesName}: {otherCharges.toLocaleString()}
                    </span>
                  )}
                </div>
                <span className="text-2xl font-black font-mono-numbers text-emerald-800">
                  {grandTotal.toLocaleString()} <span className="text-xs">{currency}</span>
                </span>
              </div>

              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={() => setShowDiscountModal(true)}
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white font-bold py-2 px-3 rounded-lg shadow-sm transition flex items-center gap-1.5 text-xs cursor-pointer"
                  title="الخصم، الضريبة، والمصاريف [F7]"
                >
                  <Tag className="w-4 h-4" />
                  <span>الخصم والمصاريف</span>
                </button>

                <button
                  type="button"
                  id="btn-pos-web-checkout"
                  onClick={handleOpenCheckout}
                  disabled={cart.length === 0}
                  className="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold py-2.5 px-5 rounded-lg shadow-md transition flex items-center gap-2 text-xs sm:text-sm cursor-pointer"
                >
                  <CreditCard className="w-4 h-4" />
                  <span>دفع الفاتورة [F3]</span>
                </button>
              </div>
            </div>
          </div>

          {/* Right Side: Visual Product Tiles Grid (50%) */}
          <div className="lg:col-span-6 xl:col-span-6 flex flex-col bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden">
            {/* Filter & Categories */}
            <div className="p-2 bg-slate-100 border-b border-slate-300 flex flex-col gap-2 shrink-0">
              <div className="relative">
                <Search className="w-4 h-4 absolute right-2.5 top-2.5 text-slate-400" />
                <input
                  type="text"
                  placeholder="ابحث عن صنف بالاسم أو الباركود..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-full pr-8 pl-3 py-1.5 bg-white border border-slate-300 rounded text-xs font-medium focus:outline-none focus:border-sky-600"
                />
              </div>

              <div className="flex items-center gap-1.5 overflow-x-auto pb-1 text-xs">
                {categories.map((cat) => (
                  <button
                    key={cat}
                    onClick={() => setSelectedCategory(cat)}
                    className={`px-3 py-1.5 rounded-lg whitespace-nowrap font-bold transition cursor-pointer text-xs ${
                      selectedCategory === cat
                        ? 'bg-sky-700 text-white shadow-xs'
                        : 'bg-white text-slate-700 border border-slate-300 hover:bg-slate-200'
                    }`}
                  >
                    {cat === 'ALL' ? 'الكل' : cat}
                  </button>
                ))}
              </div>
            </div>

            {/* Products Grid */}
            <div className="flex-1 p-2 overflow-y-auto grid grid-cols-2 sm:grid-cols-3 gap-2.5 content-start bg-slate-50/50">
              {filteredProducts.map((product) => (
                <div
                  key={product.id}
                  onClick={() => handleAddToCart(product)}
                  className="bg-white border border-slate-200 hover:border-sky-500 hover:shadow-md p-2.5 rounded-lg cursor-pointer transition flex flex-col justify-between group active:scale-[0.97]"
                >
                  <div>
                    <div className="flex items-center justify-between mb-1">
                      <span className="text-[10px] bg-slate-100 text-slate-600 px-1.5 py-0.5 rounded font-mono">
                        {product.code}
                      </span>
                      <span className="text-[10px] px-1.5 py-0.5 rounded font-bold bg-emerald-50 text-emerald-700">
                        مخزون: {product.stock}
                      </span>
                    </div>
                    <h4 className="text-xs font-bold text-slate-900 group-hover:text-sky-700 line-clamp-2 leading-tight">
                      {product.name}
                    </h4>
                  </div>

                  <div className="mt-2 pt-1.5 border-t border-slate-100 flex items-center justify-between">
                    <span className="text-xs font-black text-sky-800 font-mono-numbers">
                      {product.salePrice.toLocaleString()} {currency}
                    </span>
                    <span className="text-[10px] text-slate-500 bg-slate-100 px-1.5 rounded">
                      {product.unit}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* ========================================================================= */}
      {/* 📱 VIEW 3: MOBILE / HANDHELD TERMINAL POS MODE                            */}
      {/* ========================================================================= */}
      {posLayoutMode === 'MOBILE' && (
        <div className="flex-1 flex flex-col bg-white rounded-lg border border-slate-300 shadow-sm overflow-hidden min-h-0">
          {/* Mobile Tabs Switcher */}
          <div className="grid grid-cols-3 bg-slate-100 border-b border-slate-300 p-1 shrink-0">
            <button
              type="button"
              onClick={() => setMobileActiveTab('CART')}
              className={`py-2 text-center text-xs font-bold rounded transition cursor-pointer flex items-center justify-center gap-1 ${
                mobileActiveTab === 'CART' ? 'bg-[#0f2d4a] text-white shadow-xs' : 'text-slate-700'
              }`}
            >
              <ShoppingBag className="w-3.5 h-3.5" />
              <span>السلة ({cart.length})</span>
            </button>

            <button
              type="button"
              onClick={() => setMobileActiveTab('CATALOG')}
              className={`py-2 text-center text-xs font-bold rounded transition cursor-pointer flex items-center justify-center gap-1 ${
                mobileActiveTab === 'CATALOG' ? 'bg-sky-700 text-white shadow-xs' : 'text-slate-700'
              }`}
            >
              <LayoutGrid className="w-3.5 h-3.5" />
              <span>الأصناف</span>
            </button>

            <button
              type="button"
              onClick={() => setMobileActiveTab('PAYMENT')}
              className={`py-2 text-center text-xs font-bold rounded transition cursor-pointer flex items-center justify-center gap-1 ${
                mobileActiveTab === 'PAYMENT' ? 'bg-emerald-700 text-white shadow-xs' : 'text-slate-700'
              }`}
            >
              <CreditCard className="w-3.5 h-3.5" />
              <span>الدفع</span>
            </button>
          </div>

          {/* Tab 1: Mobile Cart */}
          {mobileActiveTab === 'CART' && (
            <div className="flex-1 overflow-y-auto p-2 flex flex-col gap-2">
              {cart.length === 0 ? (
                <div className="h-full flex flex-col items-center justify-center py-16 text-slate-400">
                  <ShoppingBag className="w-12 h-12 text-slate-300 mb-2" />
                  <span className="font-bold text-slate-600 text-sm">السلة فارغة</span>
                  <span className="text-xs text-slate-400">انتقل لتبويب الأصناف أو امسح الباركود</span>
                </div>
              ) : (
                cart.map((item, idx) => (
                  <div key={idx} className="bg-slate-50 border border-slate-200 rounded-lg p-2.5 flex items-center justify-between">
                    <div>
                      <h4 className="font-bold text-xs text-slate-900">{item.item.name}</h4>
                      <span className="text-[11px] text-sky-800 font-mono-numbers font-bold">
                        {item.total.toLocaleString()} {currency}
                      </span>
                    </div>

                    <div className="flex items-center gap-2">
                      <div className="inline-flex items-center border border-slate-300 rounded bg-white">
                        <button onClick={() => updateQuantity(idx, -1)} className="px-2 py-0.5 font-bold">-</button>
                        <span className="px-2 text-xs font-bold">{item.quantity}</span>
                        <button onClick={() => updateQuantity(idx, 1)} className="px-2 py-0.5 font-bold">+</button>
                      </div>
                      <button onClick={() => handleRemoveItem(idx)} className="text-rose-500 p-1">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>
                ))
              )}
            </div>
          )}

          {/* Tab 2: Mobile Catalog */}
          {mobileActiveTab === 'CATALOG' && (
            <div className="flex-1 overflow-y-auto p-2 flex flex-col gap-2">
              <div className="flex items-center gap-1 overflow-x-auto pb-1 text-xs">
                {categories.map((cat) => (
                  <button
                    key={cat}
                    onClick={() => setSelectedCategory(cat)}
                    className={`px-2.5 py-1 rounded whitespace-nowrap text-xs font-bold ${
                      selectedCategory === cat ? 'bg-sky-700 text-white' : 'bg-slate-100 text-slate-700'
                    }`}
                  >
                    {cat === 'ALL' ? 'الكل' : cat}
                  </button>
                ))}
              </div>

              <div className="grid grid-cols-2 gap-2">
                {filteredProducts.map((p) => (
                  <button
                    key={p.id}
                    onClick={() => handleAddToCart(p)}
                    className="bg-slate-50 border border-slate-200 hover:border-sky-500 p-2 rounded-lg text-right flex flex-col justify-between"
                  >
                    <span className="text-xs font-bold text-slate-900 line-clamp-2">{p.name}</span>
                    <span className="text-xs font-black text-sky-800 font-mono-numbers mt-1">
                      {p.salePrice.toLocaleString()} {currency}
                    </span>
                  </button>
                ))}
              </div>
            </div>
          )}

          {/* Tab 3: Mobile Payment */}
          {mobileActiveTab === 'PAYMENT' && (
            <div className="flex-1 overflow-y-auto p-3 flex flex-col gap-3">
              <div className="bg-slate-50 border border-slate-300 text-slate-800 p-3 rounded-xl flex items-center justify-between shadow-2xs">
                <div>
                  <span className="text-[10px] text-slate-500 block">المبلغ الصافي المستحق:</span>
                  <span className="text-xl font-black text-amber-600 font-mono-numbers">{grandTotal.toLocaleString()} {currency}</span>
                </div>
                <div className="text-left">
                  <span className="text-[10px] text-slate-500 block">العميل:</span>
                  <span className="text-xs font-bold text-slate-900 max-w-[140px] truncate block">{selectedCustomer.name}</span>
                </div>
              </div>

              {/* Mode Selector in Mobile */}
              <div className="grid grid-cols-2 gap-1.5 p-1 bg-slate-200 rounded-lg border border-slate-300">
                <button
                  type="button"
                  onClick={() => {
                    setPaymentMainMode('CASH');
                    setSplitCash(grandTotal);
                    setSplitNetwork(0);
                    setSplitCredit(0);
                  }}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    paymentMainMode === 'CASH'
                      ? 'bg-emerald-700 text-white shadow-xs'
                      : 'bg-white text-slate-700 hover:bg-slate-100'
                  }`}
                >
                  <span>💵 1. نقد (كاش+بنك)</span>
                </button>
                <button
                  type="button"
                  onClick={() => {
                    setPaymentMainMode('CREDIT');
                    setSplitCredit(grandTotal);
                    setSplitCash(0);
                    setSplitNetwork(0);
                  }}
                  className={`py-1.5 rounded text-xs font-bold transition flex items-center justify-center gap-1 cursor-pointer ${
                    paymentMainMode === 'CREDIT'
                      ? 'bg-amber-600 text-white shadow-xs'
                      : 'bg-white text-slate-700 hover:bg-slate-100'
                  }`}
                >
                  <span>📝 2. آجل (كاش+بنك+آجل)</span>
                </button>
              </div>

              {/* Customer Notice if Credit */}
              {paymentMainMode === 'CREDIT' && (
                <div className="bg-amber-50 border border-amber-300 p-2 rounded-lg text-xs flex items-center justify-between">
                  <span className="font-bold text-amber-950">العميل الآجل: {selectedCustomer.name}</span>
                  <span className="text-[10px] text-amber-800">الرصيد: {selectedCustomer.currentBalance.toLocaleString()} {currency}</span>
                </div>
              )}

              {/* Mobile Cash block */}
              <div className="bg-emerald-50/80 p-2.5 rounded-lg border border-emerald-300 flex flex-col gap-1.5 shadow-xs">
                <div className="flex items-center justify-between">
                  <label className="text-xs font-bold text-emerald-950 flex items-center gap-1">
                    <span>💵</span>
                    <span>1. كاش:</span>
                  </label>
                  <button
                    type="button"
                    onClick={() => {
                      const rem = paymentMainMode === 'CASH'
                        ? Math.max(0, grandTotal - splitNetwork)
                        : Math.max(0, grandTotal - splitNetwork - splitCredit);
                      setSplitCash(rem);
                    }}
                    className="text-[10px] text-emerald-700 underline font-bold cursor-pointer"
                  >
                    المتبقي
                  </button>
                </div>
                <input
                  type="number"
                  min={0}
                  value={splitCash || ''}
                  onChange={(e) => {
                    const v = parseFloat(e.target.value) || 0;
                    setSplitCash(v);
                    if (paymentMainMode === 'CASH') {
                      setSplitNetwork(Math.max(0, grandTotal - v));
                    }
                  }}
                  className="w-full bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                  placeholder="0"
                />
                <select
                  value={selectedCashAccountCode}
                  onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                  disabled={splitCash <= 0}
                  className="w-full bg-white border border-emerald-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                >
                  {availableCashAccounts.map((acc) => (
                    <option key={acc.code} value={acc.code}>{acc.name}</option>
                  ))}
                </select>
              </div>

              {/* Mobile Bank block */}
              <div className="bg-sky-50/80 p-2.5 rounded-lg border border-sky-300 flex flex-col gap-1.5 shadow-xs">
                <div className="flex items-center justify-between">
                  <label className="text-xs font-bold text-sky-950 flex items-center gap-1">
                    <Building2 className="w-3.5 h-3.5 text-sky-700" />
                    <span>2. بنك / شبكة:</span>
                  </label>
                  <button
                    type="button"
                    onClick={() => {
                      const rem = paymentMainMode === 'CASH'
                        ? Math.max(0, grandTotal - splitCash)
                        : Math.max(0, grandTotal - splitCash - splitCredit);
                      setSplitNetwork(rem);
                    }}
                    className="text-[10px] text-sky-700 underline font-bold cursor-pointer"
                  >
                    المتبقي
                  </button>
                </div>
                <input
                  type="number"
                  min={0}
                  value={splitNetwork || ''}
                  onChange={(e) => {
                    const v = parseFloat(e.target.value) || 0;
                    setSplitNetwork(v);
                    if (paymentMainMode === 'CASH') {
                      setSplitCash(Math.max(0, grandTotal - v));
                    }
                  }}
                  className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                  placeholder="0"
                />
                <select
                  value={selectedBankAccountCode}
                  onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                  disabled={splitNetwork <= 0}
                  className="w-full bg-white border border-sky-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                >
                  {availableBankAccounts.map((acc) => (
                    <option key={acc.code} value={acc.code}>{acc.name}</option>
                  ))}
                </select>
              </div>

              {/* Mobile Credit block (in CREDIT mode) */}
              {paymentMainMode === 'CREDIT' && (
                <div className="bg-amber-50/80 p-2.5 rounded-lg border border-amber-300 flex flex-col gap-1.5 shadow-xs">
                  <div className="flex items-center justify-between">
                    <label className="text-xs font-bold text-amber-950 flex items-center gap-1">
                      <span>📝</span>
                      <span>3. آجل ذمة (المتبقي على العميل):</span>
                    </label>
                    <button
                      type="button"
                      onClick={() => {
                        const rem = Math.max(0, grandTotal - splitCash - splitNetwork);
                        setSplitCredit(rem);
                      }}
                      className="text-[10px] text-amber-700 underline font-bold cursor-pointer"
                    >
                      المتبقي
                    </button>
                  </div>
                  <input
                    type="number"
                    min={0}
                    value={splitCredit || ''}
                    onChange={(e) => setSplitCredit(parseFloat(e.target.value) || 0)}
                    className="w-full bg-white border border-amber-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900"
                    placeholder="0"
                  />
                  <span className="text-[10px] text-amber-800 font-semibold">
                    الرصيد بعد الفاتورة: {(selectedCustomer.currentBalance + splitCredit).toLocaleString()} {currency}
                  </span>
                </div>
              )}
            </div>
          )}

          {/* Sticky Mobile Bottom Bar */}
          <div className="bg-white text-slate-800 p-2.5 flex items-center justify-between shrink-0 border-t border-slate-300 shadow-lg">
            <div>
              <span className="text-[10px] text-slate-500 block font-semibold">الإجمالي:</span>
              <span className="text-lg font-black text-amber-600 font-mono-numbers">{grandTotal.toLocaleString()} {currency}</span>
            </div>

            {mobileActiveTab === 'PAYMENT' ? (
              <button
                type="button"
                onClick={handleCheckoutSubmit}
                disabled={cart.length === 0}
                className="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold px-5 py-2 rounded-lg text-xs flex items-center gap-1.5"
              >
                <Printer className="w-4 h-4" />
                <span>حفظ وطباعة</span>
              </button>
            ) : (
              <button
                type="button"
                onClick={() => setMobileActiveTab('PAYMENT')}
                disabled={cart.length === 0}
                className="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold px-4 py-2 rounded-lg text-xs flex items-center gap-1"
              >
                <CreditCard className="w-4 h-4" />
                <span>متابعة للدفع</span>
              </button>
            )}
          </div>
        </div>
      )}

      {/* Checkout Modal (نافذة الدفع والترحيل المالي المتطور) */}
      {showCheckoutModal && (() => {
        // Modal Calculations for CASH and CREDIT modes
        const modalCash = splitCash;
        const modalBank = splitNetwork;
        const modalCredit = paymentMainMode === 'CREDIT' ? splitCredit : 0;
        const modalSettled = modalCash + modalBank;
        const modalTotalAllocated = modalCash + modalBank + modalCredit;
        const isCashCustomerSelected = selectedCustomer.id === 'CUST-001' || selectedCustomer.name.includes('نقدي') || selectedCustomer.name.includes('عام');
        const hasCreditObligation = paymentMainMode === 'CREDIT' && splitCredit > 0;
        const modalCashGiven = parseFloat(cashTenderedInput) || modalCash;
        const modalCashChange = Math.max(0, modalCashGiven - modalCash);
        const isMismatched = modalTotalAllocated !== grandTotal;

        return (
          <div className="fixed inset-0 bg-slate-950/60 backdrop-blur-xs flex items-center justify-center z-50 p-3 sm:p-4">
            <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-2xl overflow-hidden animate-in fade-in zoom-in-95 duration-150 flex flex-col max-h-[92vh]">
              {/* Modal Header */}
              <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between shrink-0 shadow-2xs">
                <div className="flex items-center gap-2.5">
                  <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
                    <Receipt className="w-4 h-4 text-sky-700" />
                  </div>
                  <div>
                    <h3 className="font-bold text-sm sm:text-base text-slate-900 leading-none">إتمام وسداد الفاتورة</h3>
                    <span className="text-[11px] text-slate-500">
                      وضع السداد النقدي (كاش وبنك) أو السداد الآجل (كاش وبنك وآجل)
                    </span>
                  </div>
                </div>
                <button
                  onClick={() => setShowCheckoutModal(false)}
                  className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm font-bold cursor-pointer transition"
                >
                  ✕
                </button>
              </div>

              {/* Modal Body */}
              <div className="p-4 flex flex-col gap-3 text-xs overflow-y-auto flex-1">
                {/* Grand Total & Customer Header Block */}
                <div className="bg-slate-50 border border-slate-200 text-slate-800 p-3.5 rounded-xl flex flex-col sm:flex-row sm:items-center justify-between gap-3 shadow-2xs">
                  <div>
                    <span className="text-slate-500 text-[11px] block font-semibold">المبلغ الصافي المطلوب:</span>
                    <div className="flex items-baseline gap-1.5">
                      <span className="text-2xl sm:text-3xl font-black text-amber-600 font-mono-numbers">
                        {activeCurrency !== 'محلي' && exchangeRate > 0
                          ? (grandTotal / exchangeRate).toLocaleString(undefined, { maximumFractionDigits: 2 })
                          : grandTotal.toLocaleString()}
                      </span>
                      <button
                        type="button"
                        onClick={() => setShowCurrencyModal(true)}
                        className="text-xs bg-white hover:bg-slate-100 text-amber-700 hover:text-amber-800 px-2 py-0.5 rounded border border-amber-300 font-bold transition cursor-pointer flex items-center gap-1 shadow-2xs"
                        title="انقر لتغيير العملة وسعر الصرف"
                      >
                        <span>{activeCurrency}</span>
                        <Coins className="w-3 h-3 text-amber-600" />
                      </button>
                      {activeCurrency !== 'محلي' && exchangeRate > 0 && (
                        <span className="text-[10px] text-sky-700 font-bold">
                          ({grandTotal.toLocaleString()} {currency})
                        </span>
                      )}
                    </div>
                  </div>

                  {/* Customer Selector & Status */}
                  <div className="bg-white p-2 rounded-lg border border-slate-200 sm:w-72 flex flex-col gap-1 shadow-2xs">
                    <div className="flex items-center justify-between">
                      <span className="text-[10px] text-slate-600 font-bold flex items-center gap-1">
                        <UserCheck className="w-3.5 h-3.5 text-sky-700" />
                        <span>حساب العميل:</span>
                      </span>
                      {hasCreditObligation && isCashCustomerSelected && (
                        <span className="text-[9px] bg-amber-100 text-amber-800 border border-amber-300 px-1 py-0.2 rounded font-bold animate-pulse">
                          مطلوب عميل آجل
                        </span>
                      )}
                    </div>
                    <select
                      value={selectedCustomerId}
                      onChange={(e) => setSelectedCustomerId(e.target.value)}
                      className={`w-full bg-slate-50 text-slate-800 border rounded px-2 py-1 text-xs font-semibold focus:outline-none ${
                        hasCreditObligation && isCashCustomerSelected
                          ? 'border-amber-400 ring-2 ring-amber-400/30 text-amber-800'
                          : 'border-slate-300 focus:border-sky-500'
                      }`}
                    >
                      {customers.map((c) => (
                        <option key={c.id} value={c.id} className="bg-white text-slate-800">
                          {c.name} {c.currentBalance > 0 ? `(عليه: ${c.currentBalance.toLocaleString()} ${currency})` : ''}
                        </option>
                      ))}
                    </select>

                    {selectedCustomer.id !== 'CUST-001' && (
                      <div className="text-[10px] text-slate-400 flex items-center justify-between pt-0.5">
                        <span>الرصيد السابق: <strong className="text-amber-300 font-mono-numbers">{selectedCustomer.currentBalance.toLocaleString()}</strong></span>
                        {selectedCustomer.creditLimit > 0 && (
                          <span>سقف الائتمان: <strong className="text-slate-300 font-mono-numbers">{selectedCustomer.creditLimit.toLocaleString()}</strong></span>
                        )}
                      </div>
                    )}
                  </div>
                </div>

                {/* System Setting Quick Bar */}
                <div className="bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200 flex items-center justify-between text-[11px]">
                  <div className="flex items-center gap-1.5 text-slate-700">
                    <Settings className="w-3.5 h-3.5 text-slate-500" />
                    <span>إعداد النظام: التحديد التلقائي للعميل النقدي عند السداد النقدي:</span>
                  </div>
                  <button
                    type="button"
                    onClick={() => {
                      const newVal = !autoDefaultCashCustomer;
                      setAutoDefaultCashCustomer(newVal);
                      if (newVal && !hasCreditObligation) {
                        setSelectedCustomerId('CUST-001');
                      }
                      showToast(`تم ${newVal ? 'تفعيل' : 'تعطيل'} الاختيار التلقائي للعميل النقدي`);
                    }}
                    className={`px-2 py-0.5 rounded text-[10px] font-bold cursor-pointer transition ${
                      autoDefaultCashCustomer
                        ? 'bg-emerald-100 text-emerald-800 border border-emerald-300'
                        : 'bg-slate-200 text-slate-600 border border-slate-300'
                    }`}
                  >
                    {autoDefaultCashCustomer ? '✓ مفعّل تلقائياً' : '✕ معطّل'}
                  </button>
                </div>

                {/* Payment Main Mode Switcher (2 MODES ONLY: CASH vs CREDIT) */}
                <div className="grid grid-cols-2 gap-2 bg-slate-100 p-1.5 rounded-lg border border-slate-300">
                  <button
                    type="button"
                    onClick={() => {
                      setPaymentMainMode('CASH');
                      setSplitCash(grandTotal);
                      setSplitNetwork(0);
                      setSplitWallet(0);
                      setSplitCredit(0);
                      setCashTenderedInput(grandTotal.toString());
                      if (autoDefaultCashCustomer) setSelectedCustomerId('CUST-001');
                    }}
                    className={`py-2 px-3 rounded-md font-black text-xs transition cursor-pointer flex items-center justify-center gap-2 ${
                      paymentMainMode === 'CASH'
                        ? 'bg-emerald-700 text-white shadow-sm'
                        : 'text-slate-700 hover:bg-slate-200'
                    }`}
                  >
                    <span className="text-base">💵</span>
                    <div className="text-right">
                      <div>1. وضع نقد (Cash Mode)</div>
                      <div className="text-[10px] font-normal opacity-90">مكوّن من: كاش + بنك</div>
                    </div>
                  </button>

                  <button
                    type="button"
                    onClick={() => {
                      setPaymentMainMode('CREDIT');
                      setSplitCash(0);
                      setSplitNetwork(0);
                      setSplitWallet(0);
                      setSplitCredit(grandTotal);
                      if (isCashCustomerSelected && customers.length > 1) {
                        const firstDebtor = customers.find(c => c.id !== 'CUST-001');
                        if (firstDebtor) setSelectedCustomerId(firstDebtor.id);
                      }
                    }}
                    className={`py-2 px-3 rounded-md font-black text-xs transition cursor-pointer flex items-center justify-center gap-2 ${
                      paymentMainMode === 'CREDIT'
                        ? 'bg-amber-700 text-white shadow-sm'
                        : 'text-slate-700 hover:bg-slate-200'
                    }`}
                  >
                    <span className="text-base">📝</span>
                    <div className="text-right">
                      <div>2. وضع آجل (Credit Mode)</div>
                      <div className="text-[10px] font-normal opacity-90">مكوّن من: كاش + بنك + آجل</div>
                    </div>
                  </button>
                </div>

                {/* MODE 1: CASH MODE (كاش + بنك) */}
                {paymentMainMode === 'CASH' && (
                  <div className="flex flex-col gap-3">
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                      {/* 1. Cash Section with Treasury Selection */}
                      <div className="bg-emerald-50/80 p-2.5 rounded-lg border border-emerald-300 flex flex-col gap-1.5 shadow-xs">
                        <div className="flex items-center justify-between">
                          <span className="font-bold text-emerald-950 text-xs flex items-center gap-1">
                            <span>💵</span>
                            <span>1. كاش:</span>
                          </span>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - splitNetwork);
                              setSplitCash(rem);
                              setCashTenderedInput(rem.toString());
                            }}
                            className="text-[10px] text-emerald-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCash || ''}
                          onChange={(e) => {
                            const v = parseFloat(e.target.value) || 0;
                            setSplitCash(v);
                            const autoBank = Math.max(0, grandTotal - v);
                            setSplitNetwork(autoBank);
                            setCashTenderedInput(v.toString());
                          }}
                          className="w-full bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          disabled={splitCash <= 0}
                          value={selectedCashAccountCode}
                          onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                          className="w-full bg-white border border-emerald-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                        >
                          {availableCashAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>
                              {acc.name}
                            </option>
                          ))}
                        </select>
                      </div>

                      {/* 2. Bank / Network Section */}
                      <div className="bg-sky-50/80 p-2.5 rounded-lg border border-sky-300 flex flex-col gap-1.5 shadow-xs">
                        <div className="flex items-center justify-between">
                          <span className="font-bold text-sky-950 text-xs flex items-center gap-1">
                            <Building2 className="w-3.5 h-3.5 text-sky-700" />
                            <span>2. بنك / شبكة:</span>
                          </span>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - splitCash);
                              setSplitNetwork(rem);
                            }}
                            className="text-[10px] text-sky-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitNetwork || ''}
                          onChange={(e) => {
                            const v = parseFloat(e.target.value) || 0;
                            setSplitNetwork(v);
                            const autoCash = Math.max(0, grandTotal - v);
                            setSplitCash(autoCash);
                            setCashTenderedInput(autoCash.toString());
                          }}
                          className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          disabled={splitNetwork <= 0}
                          value={selectedBankAccountCode}
                          onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                          className="w-full bg-white border border-sky-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                        >
                          {availableBankAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>
                              {acc.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    </div>

                    {/* Cash Mode Totals and Match Status */}
                    {modalSettled === grandTotal ? (
                      <div className="bg-emerald-50 border border-emerald-300 p-2.5 rounded-lg text-emerald-900 flex items-center justify-between text-xs font-semibold">
                        <div className="flex items-center gap-1.5">
                          <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                          <span>الفاتورة مسددة بالكامل نقدياً (كاش: {modalCash.toLocaleString()} + بنك: {modalBank.toLocaleString()})</span>
                        </div>
                        <span className="font-mono-numbers font-bold text-emerald-800">
                          {modalSettled.toLocaleString()} {currency}
                        </span>
                      </div>
                    ) : (
                      <div className="bg-rose-50 border-2 border-rose-300 p-2.5 rounded-lg text-rose-900 flex items-center justify-between text-xs font-bold">
                        <div className="flex items-center gap-1.5">
                          <AlertTriangle className="w-4 h-4 text-rose-600" />
                          <span>فارق لم يتم توزيعه في وضع النقد:</span>
                        </div>
                        <span className="font-mono-numbers text-rose-700 text-sm">
                          {(grandTotal - modalSettled).toLocaleString()} {currency}
                        </span>
                      </div>
                    )}
                  </div>
                )}

                {/* MODE 2: CREDIT MODE (كاش + بنك + آجل) */}
                {paymentMainMode === 'CREDIT' && (
                  <div className="flex flex-col gap-3">
                    {/* Customer Status and Credit Balance Banner */}
                    <div className="bg-amber-50 p-3 rounded-lg border-2 border-amber-400 text-amber-950 flex flex-col gap-2 shadow-xs">
                      <div className="flex items-center justify-between border-b border-amber-200 pb-1.5">
                        <div className="flex items-center gap-2">
                          <span className="text-lg">📝</span>
                          <div>
                            <span className="font-bold text-xs text-amber-950 block">فاتورة في الوضع الآجل (كاش + بنك + متبقي آجل)</span>
                            <span className="text-[10px] text-amber-800">الدفعات النقدية والبنكية تورد لحساب الصندوق والبنك كتحصيل، والمتبقي فقط يقيد مديونية على العميل</span>
                          </div>
                        </div>
                        <span className="text-sm font-black text-amber-900 font-mono-numbers">
                          الرصيد السابق: {selectedCustomer.currentBalance.toLocaleString()} {currency}
                        </span>
                      </div>

                      {isCashCustomerSelected && (
                        <div className="bg-rose-100 border border-rose-300 text-rose-900 px-3 py-1.5 rounded text-xs font-bold flex items-center gap-2">
                          <ShieldAlert className="w-4 h-4 text-rose-700 shrink-0" />
                          <span>تنبيه: يجب اختيار اسم عميل مسجل من القائمة العلوية ولا يمكن حفظ الآجل على (عميل نقدي عام).</span>
                        </div>
                      )}
                    </div>

                    {/* 3 Channels: Cash + Bank + Credit Inputs */}
                    <div className="grid grid-cols-1 sm:grid-cols-3 gap-2.5">
                      {/* 1. Cash Part */}
                      <div className="bg-emerald-50/80 p-2.5 rounded-lg border border-emerald-300 flex flex-col gap-1.5">
                        <div className="flex items-center justify-between">
                          <span className="font-bold text-emerald-950 text-xs">💵 1. كاش مدفوع:</span>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitNetwork + splitCredit));
                              setSplitCash(rem);
                            }}
                            className="text-[10px] text-emerald-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCash || ''}
                          onChange={(e) => {
                            const v = parseFloat(e.target.value) || 0;
                            setSplitCash(v);
                            const remCredit = Math.max(0, grandTotal - (v + splitNetwork));
                            setSplitCredit(remCredit);
                          }}
                          className="w-full bg-white border border-emerald-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedCashAccountCode}
                          onChange={(e) => setSelectedCashAccountCode(e.target.value)}
                          disabled={splitCash <= 0}
                          className="w-full bg-white border border-emerald-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50 disabled:bg-slate-100 disabled:text-slate-400 disabled:cursor-not-allowed"
                        >
                          {availableCashAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>{acc.name}</option>
                          ))}
                        </select>
                      </div>

                      {/* 2. Bank Part */}
                      <div className="bg-sky-50/80 p-2.5 rounded-lg border border-sky-300 flex flex-col gap-1.5">
                        <div className="flex items-center justify-between">
                          <span className="font-bold text-sky-950 text-xs">🏦 2. بنك مدفوع:</span>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitCash + splitCredit));
                              setSplitNetwork(rem);
                            }}
                            className="text-[10px] text-sky-700 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitNetwork || ''}
                          onChange={(e) => {
                            const v = parseFloat(e.target.value) || 0;
                            setSplitNetwork(v);
                            const remCredit = Math.max(0, grandTotal - (splitCash + v));
                            setSplitCredit(remCredit);
                          }}
                          className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-xs font-black font-mono-numbers text-slate-900 focus:outline-none"
                          placeholder="0"
                        />
                        <select
                          value={selectedBankAccountCode}
                          onChange={(e) => setSelectedBankAccountCode(e.target.value)}
                          disabled={splitNetwork <= 0}
                          className="w-full bg-white border border-sky-200 rounded px-1.5 py-0.5 text-[10px] text-slate-800 disabled:opacity-50"
                        >
                          {availableBankAccounts.map((acc) => (
                            <option key={acc.code} value={acc.code}>{acc.name}</option>
                          ))}
                        </select>
                      </div>

                      {/* 3. Credit Part */}
                      <div className="bg-amber-50/90 p-2.5 rounded-lg border-2 border-amber-400 flex flex-col gap-1.5">
                        <div className="flex items-center justify-between">
                          <span className="font-bold text-amber-950 text-xs">📝 3. آجل على الحساب:</span>
                          <button
                            type="button"
                            onClick={() => {
                              const rem = Math.max(0, grandTotal - (splitCash + splitNetwork));
                              setSplitCredit(rem);
                            }}
                            className="text-[10px] text-amber-800 underline font-bold cursor-pointer"
                          >
                            المتبقي
                          </button>
                        </div>
                        <input
                          type="number"
                          min={0}
                          value={splitCredit || ''}
                          onChange={(e) => setSplitCredit(parseFloat(e.target.value) || 0)}
                          className="w-full bg-white border-2 border-amber-500 rounded px-2 py-1 text-xs font-black font-mono-numbers text-amber-950 focus:outline-none"
                          placeholder="0"
                        />
                        <div className="text-[10px] text-amber-800 font-bold flex items-center justify-between pt-0.5">
                          <span>الرصيد بعد الترحيل:</span>
                          <span className="font-mono-numbers text-xs">{(selectedCustomer.currentBalance + splitCredit).toLocaleString()}</span>
                        </div>
                      </div>
                    </div>

                    {/* Credit Mode Balance & Verification Banner */}
                    <div className={`p-2.5 rounded-lg border flex items-center justify-between text-xs font-semibold ${
                      modalTotalAllocated === grandTotal
                        ? 'bg-emerald-50 border-emerald-300 text-emerald-900'
                        : 'bg-rose-50 border-rose-300 text-rose-900'
                    }`}>
                      <div>
                        <span>إجمالي التوزيع (كاش: {modalCash.toLocaleString()} + بنك: {modalBank.toLocaleString()} + آجل: {modalCredit.toLocaleString()}): </span>
                        <strong className="font-mono-numbers text-sm">{modalTotalAllocated.toLocaleString()} {currency}</strong>
                      </div>
                      {modalTotalAllocated === grandTotal ? (
                        <div className="flex items-center gap-1 text-emerald-700 font-bold">
                          <CheckCircle2 className="w-4 h-4 shrink-0" />
                          <span>المبالغ مطابقة 100% ✓</span>
                        </div>
                      ) : (
                        <div className="flex items-center gap-1 text-rose-700 font-bold">
                          <AlertTriangle className="w-4 h-4 shrink-0" />
                          <span>فارق غير مخصص: {(grandTotal - modalTotalAllocated).toLocaleString()} {currency}</span>
                        </div>
                      )}
                    </div>
                  </div>
                )}
              </div>

              {/* Modal Footer */}
              <div className="bg-slate-100 px-4 py-3 border-t border-slate-200 flex items-center justify-between shrink-0">
                <button
                  type="button"
                  onClick={() => setShowCheckoutModal(false)}
                  className="bg-white border border-slate-300 hover:bg-slate-50 text-slate-700 font-semibold px-4 py-2 rounded text-xs cursor-pointer"
                >
                  إلغاء
                </button>

                <button
                  type="button"
                  id="btn-confirm-checkout"
                  disabled={(hasCreditObligation && isCashCustomerSelected) || isMismatched}
                  onClick={handleCheckoutSubmit}
                  className={`font-bold px-6 py-2 rounded text-xs flex items-center gap-2 shadow-md cursor-pointer transition ${
                    (hasCreditObligation && isCashCustomerSelected) || isMismatched
                      ? 'bg-slate-400 text-slate-100 cursor-not-allowed'
                      : 'bg-emerald-600 hover:bg-emerald-500 text-white hover:scale-[1.02] active:scale-[0.98]'
                  }`}
                >
                  <Printer className="w-4 h-4" />
                  <span>حفظ وطباعة الفاتورة [Enter]</span>
                </button>
              </div>
            </div>
          </div>
        );
      })()}

      {/* Held Orders Modal (نافذة الفواتير المعلقة واسترجاعها السريع) */}
      <POSHeldOrdersModal
        showHeldModal={showHeldModal}
        setShowHeldModal={setShowHeldModal}
        filteredHeldOrders={filteredHeldOrders}
        heldOrders={heldOrders}
        handleRecallHeldOrder={handleRecallHeldOrder}
        currency={currency}
      />

      {/* ============================================================== */}
      {/* MODAL: Touch Discount, Tax % & Other Charges (Screenshot 10)  */}
      {/* ============================================================== */}
      <POSDiscountModal
        showDiscountModal={showDiscountModal}
        setShowDiscountModal={setShowDiscountModal}
        discountType={discountType}
        setDiscountType={setDiscountType}
        discountOverall={discountOverall}
        setDiscountOverall={setDiscountOverall}
        taxPercentVal={taxPercentVal}
        setTaxPercentVal={setTaxPercentVal}
        setIsTaxEnabled={setIsTaxEnabled}
        otherChargesAccountCode={otherChargesAccountCode}
        setOtherChargesAccountCode={setOtherChargesAccountCode}
        setOtherChargesName={setOtherChargesName}
        EXPENSE_ACCOUNTS_OPTIONS={EXPENSE_ACCOUNTS_OPTIONS}
        otherChargesVal={otherChargesVal}
        setOtherChargesVal={setOtherChargesVal}
        subtotal={subtotal}
        totalDiscount={totalDiscount}
        calculatedTax={calculatedTax}
        otherCharges={otherCharges}
        otherChargesName={otherChargesName}
        grandTotal={grandTotal}
        currency={currency}
      />

      {/* ============================================================== */}
      {/* MODAL: Multi-Currency Selection & Rate Override                */}
      {/* ============================================================== */}
      <CurrencySelectionModal
        isOpen={showCurrencyModal}
        onClose={() => setShowCurrencyModal(false)}
        selectedCurrency={activeCurrency}
        exchangeRate={exchangeRate}
        onSelect={(newCurr, newRate) => {
          setActiveCurrency(newCurr);
          setExchangeRate(newRate);
        }}
        localCurrencyName={currency}
      />
    </div>
  );
};
