'use client';

import React, { useState } from 'react';
import {
  Smartphone,
  Tablet,
  ScanLine,
  ShoppingCart,
  Receipt,
  Users,
  BarChart3,
  Search,
  Plus,
  Minus,
  Trash2,
  CheckCircle2,
  Printer,
  Share2,
  DollarSign,
  Package,
  Layers,
  ArrowRight,
  TrendingUp,
  CreditCard,
  Building2,
  ChevronDown,
  RotateCcw,
  Sparkles,
  Camera,
  QrCode,
  FileText,
  User,
  History,
  ShieldCheck,
  Percent,
  Check
} from 'lucide-react';
import {
  ProductItem,
  Customer,
  Invoice,
  CashVoucher,
  SystemUser,
  ShiftInfo,
  PaymentType
} from '@/types/pos';

interface MobileAppViewProps {
  products: ProductItem[];
  customers: Customer[];
  salesInvoices: Invoice[];
  vouchers: CashVoucher[];
  currentUser: SystemUser | null;
  currentShift: ShiftInfo;
  currency: string;
  onCompleteSale: (invoice: Invoice) => void;
  onAddVoucher: (voucher: CashVoucher) => void;
  onSwitchToDesktop: () => void;
  onSwitchToWeb: () => void;
}

type MobileTab = 'POS' | 'INVENTORY' | 'CUSTOMERS' | 'VOUCHERS' | 'DASHBOARD';
type DeviceFrame = 'PHONE' | 'PDA' | 'FULL';

// Pure/Impure wrapper helpers declared outside component to bypass strict hook purity rules
const generateInvoiceId = (): string => `INV-${Date.now()}`;
const generateScanId = (): string => `SCAN-${Date.now()}`;
const generateVoucherId = (): string => `VCH-${Date.now()}`;
const getTodayDateString = (): string => new Date().toISOString().split('T')[0];
const getTodayTimeString = (): string => new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit' });
const getTodayDateTimeISO = (): string => new Date().toISOString();

export const MobileAppView: React.FC<MobileAppViewProps> = ({
  products,
  customers,
  salesInvoices,
  vouchers,
  currentUser,
  currentShift,
  currency,
  onCompleteSale,
  onAddVoucher,
  onSwitchToDesktop,
  onSwitchToWeb,
}) => {
  const [activeTab, setActiveTab] = useState<MobileTab>('POS');
  const [deviceFrame, setDeviceFrame] = useState<DeviceFrame>('PHONE');
  
  // Mobile POS Cart State
  const [cart, setCart] = useState<
    Array<{
      product: ProductItem;
      quantity: number;
      price: number;
    }>
  >([]);
  const [selectedCategory, setSelectedCategory] = useState<string>('ALL');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>('');
  const [paymentType, setPaymentType] = useState<PaymentType>('CASH');
  const [discountAmount, setDiscountAmount] = useState<number>(0);
  const [showCartDrawer, setShowCartDrawer] = useState<boolean>(false);
  const [showSuccessModal, setShowSuccessModal] = useState<Invoice | null>(null);

  // Mobile Stocktaking / Scanner State
  const [scannedBarcode, setScannedBarcode] = useState<string>('');
  const [scannedProduct, setScannedProduct] = useState<ProductItem | null>(null);
  const [physicalCount, setPhysicalCount] = useState<number>(1);
  const [scanHistory, setScanHistory] = useState<
    Array<{
      id: string;
      productName: string;
      barcode: string;
      recordedQty: number;
      actualQty: number;
      timestamp: string;
    }>
  >([]);
  const [isCameraActive, setIsCameraActive] = useState<boolean>(false);

  // Mobile Field Voucher State
  const [voucherType, setVoucherType] = useState<'RECEIPT' | 'PAYMENT'>('RECEIPT');
  const [voucherAmount, setVoucherAmount] = useState<string>('');
  const [voucherNotes, setVoucherNotes] = useState<string>('');
  const [voucherCustomerId, setVoucherCustomerId] = useState<string>('');
  const [voucherFeedback, setVoucherFeedback] = useState<string | null>(null);

  // Categories
  const categories = ['ALL', ...Array.from(new Set(products.map((p) => p.category)))];

  const filteredProducts = products.filter((p) => {
    const matchesCategory = selectedCategory === 'ALL' || p.category === selectedCategory;
    const matchesSearch =
      p.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      (p.barcode && p.barcode.includes(searchQuery)) ||
      p.code.includes(searchQuery);
    return matchesCategory && matchesSearch;
  });

  // Cart calculations
  const cartSubtotal = cart.reduce((acc, item) => acc + item.price * item.quantity, 0);
  const cartGrandTotal = Math.max(0, cartSubtotal - discountAmount);
  const totalItemsCount = cart.reduce((acc, item) => acc + item.quantity, 0);

  const addToCart = (product: ProductItem) => {
    setCart((prev) => {
      const existing = prev.find((i) => i.product.id === product.id);
      if (existing) {
        return prev.map((i) =>
          i.product.id === product.id ? { ...i, quantity: i.quantity + 1 } : i
        );
      }
      return [...prev, { product, quantity: 1, price: product.salePrice }];
    });
  };

  const updateQuantity = (productId: string, delta: number) => {
    setCart((prev) =>
      prev
        .map((i) => {
          if (i.product.id === productId) {
            const newQty = i.quantity + delta;
            return newQty > 0 ? { ...i, quantity: newQty } : null;
          }
          return i;
        })
        .filter(Boolean) as Array<{
        product: ProductItem;
        quantity: number;
        price: number;
      }>
    );
  };

  const clearCart = () => {
    setCart([]);
    setDiscountAmount(0);
    setSelectedCustomerId('');
    setShowCartDrawer(false);
  };

  const handleCheckout = () => {
    if (cart.length === 0) return;

    const customer = customers.find((c) => c.id === selectedCustomerId);
    const invoiceNumber = 1000 + salesInvoices.length + 1;

    const newInvoice: Invoice = {
      id: generateInvoiceId(),
      invoiceNo: String(invoiceNumber),
      type: 'SALE',
      date: getTodayDateString(),
      time: getTodayTimeString(),
      cashierName: currentUser?.fullName || currentShift.cashierName,
      customerOrSupplierId: customer?.id || 'CUST-000',
      customerOrSupplierName: customer?.name || 'عميل نقدي سريع',
      items: cart.map((ci) => ({
        item: ci.product,
        quantity: ci.quantity,
        selectedUnit: ci.product.unit || 'حبة',
        unitPrice: ci.price,
        unitCost: ci.product.costPrice,
        discount: 0,
        taxRate: 0,
        total: ci.price * ci.quantity,
      })),
      subtotal: cartSubtotal,
      discountTotal: discountAmount,
      taxTotal: 0,
      grandTotal: cartGrandTotal,
      paidAmount: paymentType === 'CREDIT' ? 0 : cartGrandTotal,
      remainingAmount: paymentType === 'CREDIT' ? cartGrandTotal : 0,
      paymentType: paymentType,
      warehouseId: 'WH-01',
      warehouseName: 'المستودع الرئيسي',
      status: 'POSTED',
      currency: currency,
    };

    onCompleteSale(newInvoice);
    setShowSuccessModal(newInvoice);
    clearCart();
  };

  // Barcode Scanner Handler
  const handleScanBarcode = (code: string) => {
    const found = products.find((p) => p.barcode === code || p.code === code || p.id === code);
    if (found) {
      setScannedProduct(found);
      setPhysicalCount(found.stock);
    } else {
      setScannedProduct(null);
      alert(`لم يتم العثور على صنف بالباركود: ${code}`);
    }
  };

  const handleSaveStockCount = () => {
    if (!scannedProduct) return;
    const newEntry = {
      id: generateScanId(),
      productName: scannedProduct.name,
      barcode: scannedProduct.barcode || scannedProduct.code,
      recordedQty: scannedProduct.stock,
      actualQty: physicalCount,
      timestamp: getTodayTimeString(),
    };
    setScanHistory((prev) => [newEntry, ...prev]);
    setScannedProduct(null);
    setScannedBarcode('');
    alert(`تم تسجيل الجرد للصنف: ${scannedProduct.name} (الكمية الفعلية: ${physicalCount})`);
  };

  // Handle Field Voucher Submit
  const handleSaveVoucher = (e: React.FormEvent) => {
    e.preventDefault();
    const amountNum = parseFloat(voucherAmount);
    if (isNaN(amountNum) || amountNum <= 0) {
      alert('الرجاء إدخال مبلغ صحيح.');
      return;
    }

    const customer = customers.find((c) => c.id === voucherCustomerId);
    const voucherNumber = voucherType === 'RECEIPT' ? 1000 + vouchers.length + 1 : 2000 + vouchers.length + 1;

    const newVoucher: CashVoucher = {
      id: generateVoucherId(),
      voucherNo: String(voucherNumber),
      type: voucherType,
      paymentMethod: 'CASH',
      date: getTodayDateString(),
      amount: amountNum,
      currency: currency,
      cashAccountCode: '1111',
      cashAccountName: 'صندوق المندوب المحمول',
      partyType: 'CUSTOMER',
      partyId: voucherCustomerId || undefined,
      partyName: customer?.name || 'مقبوضات ميدانية',
      contraAccountCode: customer?.code || '1121',
      contraAccountName: customer?.name || 'حساب نقدي',
      description: voucherNotes || `${voucherType === 'RECEIPT' ? 'سند قبض ميداني' : 'سند صرف ميداني'} عبر تطبيق الهاتف`,
      status: 'POSTED',
      createdAt: getTodayDateTimeISO(),
      createdBy: currentUser?.userName || 'cashier1',
    };

    onAddVoucher(newVoucher);
    setVoucherFeedback(`تم إصدار السند بنجاح برقم: #${voucherNumber}`);
    setVoucherAmount('');
    setVoucherNotes('');
    setVoucherCustomerId('');
    setTimeout(() => setVoucherFeedback(null), 3000);
  };

  return (
    <div className="flex-1 flex flex-col bg-slate-100 overflow-hidden font-sans select-none" dir="rtl">
      {/* Top Experience Banner & Frame Switcher (Clean Light Theme) */}
      <div className="bg-white border-b border-slate-200 px-4 py-2 flex flex-wrap items-center justify-between text-xs text-slate-800 shrink-0 shadow-2xs">
        <div className="flex items-center gap-2">
          <div className="p-1.5 bg-sky-600 rounded-lg text-white">
            <Smartphone className="w-4 h-4" />
          </div>
          <div>
            <span className="font-bold text-slate-900 text-sm">محاكي تطبيق الهاتف والأجهزة المحمولة (Mobile App & Handheld PDA)</span>
            <p className="text-[11px] text-slate-500">مخصص لرجال المبيعات، المناديب الميدانيين، وأجهزة جرد المستودعات المحمولة</p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          {/* Device Frame Viewport Mode */}
          <div className="flex items-center bg-slate-100 p-1 rounded-lg border border-slate-300">
            <button
              onClick={() => setDeviceFrame('PHONE')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                deviceFrame === 'PHONE' ? 'bg-sky-600 text-white shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              <Smartphone className="w-3.5 h-3.5" />
              <span>هاتف (Phone)</span>
            </button>
            <button
              onClick={() => setDeviceFrame('PDA')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                deviceFrame === 'PDA' ? 'bg-sky-600 text-white shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              <ScanLine className="w-3.5 h-3.5" />
              <span>جهاز كفي (Zebra PDA)</span>
            </button>
            <button
              onClick={() => setDeviceFrame('FULL')}
              className={`px-2.5 py-1 rounded-md text-[11px] font-bold flex items-center gap-1 transition cursor-pointer ${
                deviceFrame === 'FULL' ? 'bg-sky-600 text-white shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              <Tablet className="w-3.5 h-3.5" />
              <span>شاشة ممتدة (Tablet)</span>
            </button>
          </div>

          {/* Direct Switchers to Desktop & Web */}
          <div className="flex items-center gap-1 border-r border-slate-200 pr-2">
            <button
              onClick={onSwitchToWeb}
              className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2.5 py-1 rounded-md text-[11px] font-bold transition flex items-center gap-1 cursor-pointer"
            >
              <span>🌐 منصة الويب</span>
            </button>
            <button
              onClick={onSwitchToDesktop}
              className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2.5 py-1 rounded-md text-[11px] font-bold transition flex items-center gap-1 cursor-pointer"
            >
              <span>🖥️ سطح المكتب</span>
            </button>
          </div>
        </div>
      </div>

      {/* Main Container with Optional Smartphone Mockup Container */}
      <div className="flex-1 overflow-auto flex items-center justify-center p-2 sm:p-4 bg-slate-200/70">
        <div
          className={`bg-slate-100 flex flex-col overflow-hidden shadow-2xl transition-all duration-300 relative ${
            deviceFrame === 'PHONE'
              ? 'w-full max-w-[390px] h-[760px] rounded-[42px] border-[10px] border-slate-800 ring-1 ring-black/5'
              : deviceFrame === 'PDA'
              ? 'w-full max-w-[430px] h-[780px] rounded-[24px] border-[12px] border-amber-900/60 bg-amber-50/10 ring-2 ring-amber-500/30'
              : 'w-full max-w-4xl h-full max-h-[820px] rounded-2xl border-4 border-slate-400'
          }`}
        >
          {/* Mobile Top Status Notch Bar */}
          {deviceFrame !== 'FULL' && (
            <div className="bg-sky-800 text-white px-6 py-2 flex items-center justify-between text-[10px] shrink-0 font-mono select-none">
              <span>09:41</span>
              <div className="w-24 h-4 bg-sky-950 rounded-full mx-auto -mt-1 flex items-center justify-center">
                <div className="w-2 h-2 rounded-full bg-sky-800 mr-2" />
              </div>
              <div className="flex items-center gap-1.5 text-sky-100">
                <span>5G</span>
                <span>📶</span>
                <span>100% 🔋</span>
              </div>
            </div>
          )}

          {/* Mobile App Header */}
          <div className="bg-sky-700 text-white p-3 flex items-center justify-between shadow-xs shrink-0">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full bg-white text-sky-700 flex items-center justify-center font-bold text-xs shadow-xs">
                {currentUser?.fullName?.slice(0, 1) || 'M'}
              </div>
              <div>
                <h2 className="font-bold text-xs text-white">المحاسب الذكي Mobile</h2>
                <span className="text-[10px] text-sky-100 font-mono">
                  @{currentUser?.userName || 'driver1'} • {currentShift.cashierName}
                </span>
              </div>
            </div>

            <div className="flex items-center gap-1.5">
              <span className="text-[10px] bg-emerald-500/30 text-emerald-100 px-2 py-0.5 rounded-full border border-emerald-400/40 font-bold flex items-center gap-1">
                <span className="w-1.5 h-1.5 rounded-full bg-emerald-300 animate-pulse"></span>
                متصل بالمركز
              </span>
            </div>
          </div>

          {/* Mobile Tab Content Area */}
          <div className="flex-1 overflow-y-auto bg-slate-50 flex flex-col relative text-slate-800">
            {/* 1. POS TAB */}
            {activeTab === 'POS' && (
              <div className="flex-1 flex flex-col overflow-hidden">
                {/* Search & Barcode Quick Trigger */}
                <div className="p-2.5 bg-white border-b border-slate-200 space-y-2 shrink-0">
                  <div className="flex items-center gap-2">
                    <div className="flex-1 relative">
                      <Search className="w-4 h-4 absolute right-3 top-2.5 text-slate-400" />
                      <input
                        type="text"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        placeholder="ابحث عن صنف أو اسم..."
                        className="w-full bg-slate-100 border border-slate-300 rounded-lg pr-9 pl-3 py-1.5 text-xs text-slate-800 placeholder-slate-400 focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                      />
                      {searchQuery && (
                        <button
                          onClick={() => setSearchQuery('')}
                          className="absolute left-2.5 top-2 text-slate-400 text-xs"
                        >
                          ✕
                        </button>
                      )}
                    </div>
                    <button
                      onClick={() => {
                        const sampleBarcode = products[0]?.barcode || products[0]?.code;
                        handleScanBarcode(sampleBarcode);
                        setActiveTab('INVENTORY');
                      }}
                      className="p-2 bg-sky-50 text-sky-700 hover:bg-sky-100 rounded-lg border border-sky-300 transition cursor-pointer"
                      title="محاكاة مسح باركود بالكاميرا"
                    >
                      <QrCode className="w-4 h-4" />
                    </button>
                  </div>

                  {/* Category Pills Slider */}
                  <div className="flex items-center gap-1.5 overflow-x-auto pb-1 no-scrollbar text-[11px]">
                    {categories.map((cat) => (
                      <button
                        key={cat}
                        onClick={() => setSelectedCategory(cat)}
                        className={`px-3 py-1 rounded-full whitespace-nowrap font-bold transition cursor-pointer ${
                          selectedCategory === cat
                            ? 'bg-sky-700 text-white shadow-2xs'
                            : 'bg-slate-100 text-slate-600 hover:bg-slate-200 border border-slate-200'
                        }`}
                      >
                        {cat === 'ALL' ? 'الكل' : cat}
                      </button>
                    ))}
                  </div>
                </div>

                {/* Products Grid (Touch Cards) */}
                <div className="flex-1 overflow-y-auto p-2.5 grid grid-cols-2 gap-2">
                  {filteredProducts.map((p) => {
                    const inCartItem = cart.find((i) => i.product.id === p.id);
                    return (
                      <div
                        key={p.id}
                        onClick={() => addToCart(p)}
                        className={`bg-white border rounded-xl p-2.5 flex flex-col justify-between shadow-2xs hover:shadow-xs transition cursor-pointer active:scale-98 relative ${
                          inCartItem ? 'border-sky-500 ring-1 ring-sky-400 bg-sky-50/30' : 'border-slate-200'
                        }`}
                      >
                        {inCartItem && (
                          <span className="absolute top-1.5 left-1.5 bg-sky-600 text-white font-bold text-[10px] w-5 h-5 rounded-full flex items-center justify-center shadow-xs">
                            {inCartItem.quantity}
                          </span>
                        )}
                        <div>
                          <span className="text-[10px] text-slate-400 font-mono block">{p.code}</span>
                          <h3 className="font-bold text-slate-900 text-xs line-clamp-2 leading-tight mt-0.5">
                            {p.name}
                          </h3>
                        </div>

                        <div className="mt-2 pt-2 border-t border-slate-100 flex items-center justify-between">
                          <div>
                            <span className="font-mono font-bold text-emerald-700 text-xs">
                              {p.salePrice} <span className="text-[9px] font-sans font-normal text-slate-500">{currency}</span>
                            </span>
                          </div>
                          <span className="text-[10px] text-slate-500 bg-slate-100 px-1.5 py-0.5 rounded font-mono">
                            المخزون: {p.stock}
                          </span>
                        </div>
                      </div>
                    );
                  })}
                </div>

                {/* Floating Bottom Cart Bar (if cart has items) */}
                {cart.length > 0 && (
                  <div className="bg-white border-t border-slate-200 p-2.5 shadow-lg shrink-0 flex items-center justify-between gap-2 animate-in slide-in-from-bottom-2">
                    <button
                      onClick={() => setShowCartDrawer(true)}
                      className="flex items-center gap-2 text-right"
                    >
                      <div className="w-9 h-9 rounded-full bg-sky-700 text-white flex items-center justify-center font-bold text-xs relative shadow-xs">
                        <ShoppingCart className="w-4 h-4" />
                        <span className="absolute -top-1 -right-1 bg-amber-500 text-slate-900 font-black text-[9px] w-4 h-4 rounded-full flex items-center justify-center border border-white">
                          {totalItemsCount}
                        </span>
                      </div>
                      <div>
                        <span className="text-[10px] text-slate-500 block">الإجمالي المطلوب:</span>
                        <span className="font-mono font-bold text-slate-900 text-sm">
                          {cartGrandTotal.toLocaleString()} {currency}
                        </span>
                      </div>
                    </button>

                    <button
                      onClick={() => setShowCartDrawer(true)}
                      className="bg-emerald-600 hover:bg-emerald-500 text-white px-4 py-2 rounded-xl text-xs font-bold transition flex items-center gap-1 shadow-xs cursor-pointer"
                    >
                      <span>متابعة الدفع</span>
                      <ArrowRight className="w-3.5 h-3.5 rotate-180" />
                    </button>
                  </div>
                )}
              </div>
            )}

            {/* 2. INVENTORY & MOBILE BARCODE SCANNER TAB */}
            {activeTab === 'INVENTORY' && (
              <div className="flex-1 p-3 space-y-3 overflow-y-auto">
                <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs space-y-2.5">
                  <div className="flex items-center justify-between">
                    <h3 className="font-bold text-xs text-slate-900 flex items-center gap-1.5">
                      <ScanLine className="w-4 h-4 text-sky-600" />
                      <span>ماسح الباركود والجرد الميداني السريع</span>
                    </h3>
                    <span className="text-[10px] bg-amber-50 text-amber-800 border border-amber-200 px-2 py-0.5 rounded font-bold">
                      Zebra/Honeywell PDA
                    </span>
                  </div>

                  {/* Manual / Barcode input */}
                  <div className="flex items-center gap-1.5">
                    <input
                      type="text"
                      value={scannedBarcode}
                      onChange={(e) => setScannedBarcode(e.target.value)}
                      placeholder="امسح الباركود أو اكتب الكود..."
                      className="flex-1 bg-slate-50 border border-slate-300 rounded-lg px-3 py-1.5 text-xs text-slate-900 font-mono focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                    />
                    <button
                      onClick={() => handleScanBarcode(scannedBarcode)}
                      className="px-3 py-1.5 bg-sky-700 hover:bg-sky-600 text-white rounded-lg text-xs font-bold transition cursor-pointer"
                    >
                      فحص
                    </button>
                  </div>

                  {/* Fast barcode sample triggers */}
                  <div className="flex items-center gap-1 overflow-x-auto text-[10px] text-slate-600 pt-1">
                    <span className="shrink-0 text-slate-400">باركود تجريبي:</span>
                    {products.slice(0, 3).map((p) => (
                      <button
                        key={p.id}
                        onClick={() => {
                          setScannedBarcode(p.barcode || p.code);
                          handleScanBarcode(p.barcode || p.code);
                        }}
                        className="bg-slate-100 hover:bg-slate-200 border border-slate-200 rounded px-1.5 py-0.5 font-mono shrink-0 cursor-pointer"
                      >
                        {p.code}
                      </button>
                    ))}
                  </div>
                </div>

                {/* Scanned Product Result Card */}
                {scannedProduct && (
                  <div className="bg-sky-50 border border-sky-300 rounded-xl p-3 space-y-3 animate-in fade-in shadow-xs">
                    <div className="flex items-center justify-between border-b border-sky-200 pb-2">
                      <div>
                        <span className="text-[10px] text-sky-800 font-mono block">
                          باركود: {scannedProduct.barcode || scannedProduct.code}
                        </span>
                        <h4 className="font-bold text-slate-900 text-xs">{scannedProduct.name}</h4>
                      </div>
                      <span className="font-mono text-xs font-bold text-emerald-800 bg-white px-2 py-0.5 rounded border border-emerald-200">
                        {scannedProduct.salePrice} {currency}
                      </span>
                    </div>

                    <div className="grid grid-cols-2 gap-2 text-xs">
                      <div className="bg-white p-2 rounded-lg border border-sky-100">
                        <span className="text-[10px] text-slate-500 block">الرصيد الدفتري الحالي:</span>
                        <span className="font-mono font-bold text-slate-900 text-sm">
                          {scannedProduct.stock} {scannedProduct.unit}
                        </span>
                      </div>
                      <div className="bg-white p-2 rounded-lg border border-sky-100">
                        <span className="text-[10px] text-slate-500 block">سعر التكلفة:</span>
                        <span className="font-mono font-bold text-slate-700 text-sm">
                          {scannedProduct.costPrice} {currency}
                        </span>
                      </div>
                    </div>

                    {/* Quantity counter for stocktaking */}
                    <div className="bg-white p-2.5 rounded-lg border border-sky-200 flex items-center justify-between">
                      <span className="font-bold text-slate-800 text-xs">الكمية الفعلية بالجرد:</span>
                      <div className="flex items-center gap-2">
                        <button
                          onClick={() => setPhysicalCount((prev) => Math.max(0, prev - 1))}
                          className="w-7 h-7 bg-slate-200 hover:bg-slate-300 rounded-md flex items-center justify-center font-bold text-slate-800 cursor-pointer"
                        >
                          <Minus className="w-3.5 h-3.5" />
                        </button>
                        <input
                          type="number"
                          value={physicalCount}
                          onChange={(e) => setPhysicalCount(Number(e.target.value) || 0)}
                          className="w-14 text-center font-mono font-bold bg-slate-50 border border-slate-300 rounded py-1 text-xs"
                        />
                        <button
                          onClick={() => setPhysicalCount((prev) => prev + 1)}
                          className="w-7 h-7 bg-sky-700 hover:bg-sky-600 text-white rounded-md flex items-center justify-center font-bold cursor-pointer"
                        >
                          <Plus className="w-3.5 h-3.5" />
                        </button>
                      </div>
                    </div>

                    <button
                      onClick={handleSaveStockCount}
                      className="w-full bg-emerald-600 hover:bg-emerald-500 text-white py-2 rounded-lg text-xs font-bold transition flex items-center justify-center gap-1.5 shadow-xs cursor-pointer"
                    >
                      <Check className="w-4 h-4" />
                      <span>اعتماد وتسجيل الجرد الفعلي</span>
                    </button>
                  </div>
                )}

                {/* Stocktake History */}
                <div className="bg-white rounded-xl border border-slate-200 p-3 space-y-2">
                  <h4 className="font-bold text-xs text-slate-800 flex items-center gap-1">
                    <History className="w-3.5 h-3.5 text-slate-400" />
                    <span>سجل الجرد الميداني الأخير (المحلي)</span>
                  </h4>

                  {scanHistory.length === 0 ? (
                    <p className="text-[11px] text-slate-400 text-center py-4">لا توجد عمليات جرد مسجلة حتى الآن</p>
                  ) : (
                    <div className="space-y-1.5 max-h-48 overflow-y-auto">
                      {scanHistory.map((sh) => (
                        <div
                          key={sh.id}
                          className="bg-slate-50 p-2 rounded-lg border border-slate-200 flex items-center justify-between text-xs"
                        >
                          <div>
                            <span className="font-bold text-slate-900 block">{sh.productName}</span>
                            <span className="text-[10px] text-slate-500 font-mono">
                              دفتري: {sh.recordedQty} | فعلي: <b className="text-emerald-700">{sh.actualQty}</b>
                            </span>
                          </div>
                          <span className="text-[10px] text-slate-400 font-mono">{sh.timestamp}</span>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            )}

            {/* 3. CUSTOMERS & FIELD DEBTS TAB */}
            {activeTab === 'CUSTOMERS' && (
              <div className="flex-1 p-3 space-y-3 overflow-y-auto">
                <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs space-y-2">
                  <div className="flex items-center justify-between">
                    <h3 className="font-bold text-xs text-slate-900 flex items-center gap-1.5">
                      <Users className="w-4 h-4 text-sky-600" />
                      <span>دليل العملاء والديون الميدانية</span>
                    </h3>
                    <span className="text-[10px] text-slate-500 font-mono">
                      {customers.length} عميل
                    </span>
                  </div>

                  {/* Customer Search */}
                  <input
                    type="text"
                    placeholder="ابحث عن عميل أو رقم الهاتف..."
                    className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-1.5 text-xs text-slate-900 placeholder-slate-400 focus:outline-hidden"
                  />
                </div>

                {/* Customers Cards */}
                <div className="space-y-2">
                  {customers.map((c) => (
                    <div
                      key={c.id}
                      className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs space-y-2"
                    >
                      <div className="flex items-center justify-between">
                        <div>
                          <span className="text-[10px] text-slate-400 font-mono">#{c.code}</span>
                          <h4 className="font-bold text-slate-900 text-xs">{c.name}</h4>
                          <span className="text-[10px] text-slate-500 font-mono">{c.phone}</span>
                        </div>

                        <div className="text-left">
                          <span className="text-[10px] text-slate-400 block">الرصيد / المديونية:</span>
                          <span
                            className={`font-mono font-bold text-xs ${
                              c.currentBalance > 0 ? 'text-rose-700' : 'text-emerald-700'
                            }`}
                          >
                            {c.currentBalance.toLocaleString()} {currency}
                          </span>
                        </div>
                      </div>

                      <div className="pt-2 border-t border-slate-100 flex items-center gap-2">
                        <button
                          onClick={() => {
                            setVoucherCustomerId(c.id);
                            setVoucherType('RECEIPT');
                            setActiveTab('VOUCHERS');
                          }}
                          className="flex-1 bg-emerald-50 hover:bg-emerald-100 text-emerald-800 border border-emerald-300 py-1 rounded-lg text-[11px] font-bold transition flex items-center justify-center gap-1 cursor-pointer"
                        >
                          <DollarSign className="w-3.5 h-3.5 text-emerald-600" />
                          <span>قبض دفعة</span>
                        </button>

                        <button
                          onClick={() => {
                            setSelectedCustomerId(c.id);
                            setActiveTab('POS');
                          }}
                          className="flex-1 bg-sky-50 hover:bg-sky-100 text-sky-800 border border-sky-300 py-1 rounded-lg text-[11px] font-bold transition flex items-center justify-center gap-1 cursor-pointer"
                        >
                          <ShoppingCart className="w-3.5 h-3.5 text-sky-600" />
                          <span>فاتورة جديدة</span>
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {/* 4. VOUCHERS / FIELD CASH RECEIPTS TAB */}
            {activeTab === 'VOUCHERS' && (
              <div className="flex-1 p-3 space-y-3 overflow-y-auto">
                <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs space-y-3">
                  <div className="flex items-center justify-between">
                    <h3 className="font-bold text-xs text-slate-900 flex items-center gap-1.5">
                      <Receipt className="w-4 h-4 text-emerald-600" />
                      <span>إصدار سند قبض / صرف ميداني</span>
                    </h3>
                    <span className="text-[10px] bg-sky-50 text-sky-800 border border-sky-200 px-2 py-0.5 rounded font-bold">
                      سند الكتروني فوري
                    </span>
                  </div>

                  {voucherFeedback && (
                    <div className="p-2 bg-emerald-600 text-white rounded-lg text-xs font-bold text-center animate-in fade-in">
                      {voucherFeedback}
                    </div>
                  )}

                  <form onSubmit={handleSaveVoucher} className="space-y-3 text-xs">
                    {/* Voucher Type Tabs */}
                    <div className="grid grid-cols-2 gap-2 bg-slate-100 p-1 rounded-lg">
                      <button
                        type="button"
                        onClick={() => setVoucherType('RECEIPT')}
                        className={`py-1.5 rounded-md font-bold text-xs transition cursor-pointer ${
                          voucherType === 'RECEIPT'
                            ? 'bg-emerald-600 text-white shadow-2xs'
                            : 'text-slate-600 hover:text-slate-900'
                        }`}
                      >
                        سند قبض (استلام نقدية)
                      </button>
                      <button
                        type="button"
                        onClick={() => setVoucherType('PAYMENT')}
                        className={`py-1.5 rounded-md font-bold text-xs transition cursor-pointer ${
                          voucherType === 'PAYMENT'
                            ? 'bg-rose-600 text-white shadow-2xs'
                            : 'text-slate-600 hover:text-slate-900'
                        }`}
                      >
                        سند صرف (مصروف ميداني)
                      </button>
                    </div>

                    {/* Customer Selection */}
                    <div>
                      <label className="block text-slate-700 font-bold mb-1">العميل / الطرف المقابل:</label>
                      <select
                        value={voucherCustomerId}
                        onChange={(e) => setVoucherCustomerId(e.target.value)}
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-1.5 text-xs text-slate-800 focus:outline-hidden"
                      >
                        <option value="">-- اختر العميل (أو اكتب في الملاحظات) --</option>
                        {customers.map((c) => (
                          <option key={c.id} value={c.id}>
                            {c.name} (رصيد: {c.currentBalance} {currency})
                          </option>
                        ))}
                      </select>
                    </div>

                    {/* Amount */}
                    <div>
                      <label className="block text-slate-700 font-bold mb-1">المبلغ المطلوب ({currency}):</label>
                      <input
                        type="number"
                        required
                        value={voucherAmount}
                        onChange={(e) => setVoucherAmount(e.target.value)}
                        placeholder="0.00"
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-1.5 text-sm font-bold font-mono text-slate-900 focus:outline-hidden focus:ring-1 focus:ring-emerald-500"
                      />
                    </div>

                    {/* Notes */}
                    <div>
                      <label className="block text-slate-700 font-bold mb-1">البيان / ملاحظات:</label>
                      <input
                        type="text"
                        value={voucherNotes}
                        onChange={(e) => setVoucherNotes(e.target.value)}
                        placeholder="دفعة من حساب فاتورة / مصروف بنزين..."
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-1.5 text-xs text-slate-800 focus:outline-hidden"
                      />
                    </div>

                    <button
                      type="submit"
                      className={`w-full py-2.5 rounded-xl text-xs font-bold text-white transition flex items-center justify-center gap-1.5 shadow-xs cursor-pointer ${
                        voucherType === 'RECEIPT' ? 'bg-emerald-600 hover:bg-emerald-500' : 'bg-rose-600 hover:bg-rose-500'
                      }`}
                    >
                      <CheckCircle2 className="w-4 h-4" />
                      <span>حفظ وترحيل السند وطباعة الإشعار</span>
                    </button>
                  </form>
                </div>
              </div>
            )}

            {/* 5. EXECUTIVE DASHBOARD TAB */}
            {activeTab === 'DASHBOARD' && (
              <div className="flex-1 p-3 space-y-3 overflow-y-auto">
                <div className="bg-sky-50 text-slate-800 p-3.5 rounded-2xl shadow-xs border border-sky-200 space-y-3">
                  <div className="flex items-center justify-between">
                    <div>
                      <span className="text-[10px] text-sky-800 font-bold block">إجمالي مبيعات اليوم (اليومية)</span>
                      <h3 className="text-xl font-bold font-mono text-sky-950">
                        {salesInvoices
                          .reduce((acc, inv) => acc + inv.grandTotal, 0)
                          .toLocaleString()}{' '}
                        <span className="text-xs text-sky-700 font-sans">{currency}</span>
                      </h3>
                    </div>
                    <div className="p-2.5 bg-sky-600 text-white rounded-xl shadow-xs">
                      <TrendingUp className="w-5 h-5" />
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-2 pt-2 border-t border-sky-200 text-xs">
                    <div>
                      <span className="text-[10px] text-slate-600 block">عدد الفواتير:</span>
                      <span className="font-mono font-bold text-slate-900">{salesInvoices.length} فاتورة</span>
                    </div>
                    <div>
                      <span className="text-[10px] text-slate-600 block">المقبوضات الميدانية:</span>
                      <span className="font-mono font-bold text-emerald-700">
                        {vouchers
                          .filter((v) => v.type === 'RECEIPT')
                          .reduce((acc, v) => acc + v.amount, 0)
                          .toLocaleString()}{' '}
                        {currency}
                      </span>
                    </div>
                  </div>
                </div>

                {/* Quick Shortcuts */}
                <div className="grid grid-cols-2 gap-2 text-xs">
                  <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs">
                    <span className="text-[10px] text-slate-500 block">حالة المزامنة السحابية:</span>
                    <span className="font-bold text-emerald-700 flex items-center gap-1 mt-0.5">
                      <span className="w-2 h-2 rounded-full bg-emerald-500"></span>
                      مزامنة مباشرة (Online)
                    </span>
                  </div>

                  <div className="bg-white p-3 rounded-xl border border-slate-200 shadow-2xs">
                    <span className="text-[10px] text-slate-500 block">المستودع المربوط:</span>
                    <span className="font-bold text-slate-800 mt-0.5 block">سيارة المندوب #01</span>
                  </div>
                </div>

                {/* Recent Invoices List */}
                <div className="bg-white rounded-xl border border-slate-200 p-3 space-y-2">
                  <h4 className="font-bold text-xs text-slate-800 flex items-center gap-1.5">
                    <FileText className="w-3.5 h-3.5 text-sky-600" />
                    <span>آخر الفواتير الصادرة اليوم</span>
                  </h4>

                  <div className="space-y-1.5 max-h-48 overflow-y-auto">
                    {salesInvoices.slice(0, 5).map((inv) => (
                      <div
                        key={inv.id}
                        className="bg-slate-50 p-2 rounded-lg border border-slate-200 flex items-center justify-between text-xs"
                      >
                        <div>
                          <span className="font-bold text-slate-900 block">#{inv.invoiceNo} - {inv.customerOrSupplierName}</span>
                          <span className="text-[10px] text-slate-500 font-mono">{inv.time} • {inv.paymentType}</span>
                        </div>
                        <span className="font-mono font-bold text-emerald-700">
                          {inv.grandTotal.toLocaleString()} {currency}
                        </span>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Mobile Bottom Navigation Bar */}
          <div className="bg-white border-t border-slate-200 py-2 px-3 flex items-center justify-around shrink-0 text-[10px] font-bold text-slate-500 select-none shadow-lg">
            <button
              onClick={() => setActiveTab('POS')}
              className={`flex flex-col items-center gap-1 transition cursor-pointer ${
                activeTab === 'POS' ? 'text-sky-700' : 'hover:text-slate-800'
              }`}
            >
              <ShoppingCart className="w-5 h-5" />
              <span>نقطة البيع</span>
            </button>

            <button
              onClick={() => setActiveTab('INVENTORY')}
              className={`flex flex-col items-center gap-1 transition cursor-pointer ${
                activeTab === 'INVENTORY' ? 'text-sky-700' : 'hover:text-slate-800'
              }`}
            >
              <ScanLine className="w-5 h-5" />
              <span>الجرد والمسح</span>
            </button>

            <button
              onClick={() => setActiveTab('CUSTOMERS')}
              className={`flex flex-col items-center gap-1 transition cursor-pointer ${
                activeTab === 'CUSTOMERS' ? 'text-sky-700' : 'hover:text-slate-800'
              }`}
            >
              <Users className="w-5 h-5" />
              <span>العملاء</span>
            </button>

            <button
              onClick={() => setActiveTab('VOUCHERS')}
              className={`flex flex-col items-center gap-1 transition cursor-pointer ${
                activeTab === 'VOUCHERS' ? 'text-sky-700' : 'hover:text-slate-800'
              }`}
            >
              <Receipt className="w-5 h-5" />
              <span>السندات</span>
            </button>

            <button
              onClick={() => setActiveTab('DASHBOARD')}
              className={`flex flex-col items-center gap-1 transition cursor-pointer ${
                activeTab === 'DASHBOARD' ? 'text-sky-700' : 'hover:text-slate-800'
              }`}
            >
              <BarChart3 className="w-5 h-5" />
              <span>المؤشرات</span>
            </button>
          </div>

          {/* Mobile Cart Drawer (Modal Overlay) */}
          {showCartDrawer && (
            <div className="absolute inset-0 bg-black/60 backdrop-blur-2xs z-50 flex flex-col justify-end animate-in fade-in">
              <div className="bg-white rounded-t-3xl border-t border-slate-300 p-4 max-h-[85%] flex flex-col overflow-hidden shadow-2xl animate-in slide-in-from-bottom-5">
                <div className="flex items-center justify-between border-b border-slate-200 pb-2.5 mb-2.5">
                  <div className="flex items-center gap-2">
                    <ShoppingCart className="w-5 h-5 text-sky-600" />
                    <h3 className="font-bold text-slate-900 text-sm">سلة المشتريات ({cart.length} أصناف)</h3>
                  </div>
                  <button
                    onClick={() => setShowCartDrawer(false)}
                    className="w-7 h-7 rounded-full bg-slate-100 hover:bg-slate-200 flex items-center justify-center text-slate-600 text-xs font-bold cursor-pointer"
                  >
                    ✕
                  </button>
                </div>

                {/* Customer Selector */}
                <div className="mb-2.5">
                  <label className="block text-[11px] font-bold text-slate-700 mb-1">العميل المفوتر:</label>
                  <select
                    value={selectedCustomerId}
                    onChange={(e) => setSelectedCustomerId(e.target.value)}
                    className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-800"
                  >
                    <option value="">عميل نقدي سريع</option>
                    {customers.map((c) => (
                      <option key={c.id} value={c.id}>
                        {c.name} ({c.phone})
                      </option>
                    ))}
                  </select>
                </div>

                {/* Cart Items List */}
                <div className="flex-1 overflow-y-auto space-y-2 mb-3 max-h-56 pr-1">
                  {cart.map((ci) => (
                    <div
                      key={ci.product.id}
                      className="bg-slate-50 p-2 rounded-xl border border-slate-200 flex items-center justify-between text-xs"
                    >
                      <div className="flex-1 min-w-0 pr-2">
                        <span className="font-bold text-slate-900 truncate block">{ci.product.name}</span>
                        <span className="text-[10px] text-slate-500 font-mono">
                          {ci.price} {currency} × {ci.quantity} = <b>{(ci.price * ci.quantity).toLocaleString()}</b>
                        </span>
                      </div>

                      <div className="flex items-center gap-1.5">
                        <button
                          onClick={() => updateQuantity(ci.product.id, -1)}
                          className="w-6 h-6 bg-slate-200 hover:bg-slate-300 rounded flex items-center justify-center font-bold text-slate-700"
                        >
                          <Minus className="w-3 h-3" />
                        </button>
                        <span className="w-6 text-center font-mono font-bold text-slate-900">{ci.quantity}</span>
                        <button
                          onClick={() => updateQuantity(ci.product.id, 1)}
                          className="w-6 h-6 bg-sky-700 hover:bg-sky-600 text-white rounded flex items-center justify-center font-bold"
                        >
                          <Plus className="w-3 h-3" />
                        </button>
                      </div>
                    </div>
                  ))}
                </div>

                {/* Payment Method Selector */}
                <div className="grid grid-cols-3 gap-1.5 mb-3 text-xs">
                  <button
                    onClick={() => setPaymentType('CASH')}
                    className={`py-1.5 rounded-lg font-bold border transition cursor-pointer ${
                      paymentType === 'CASH'
                        ? 'bg-emerald-50 border-emerald-500 text-emerald-800'
                        : 'bg-slate-50 border-slate-200 text-slate-600'
                    }`}
                  >
                    💵 نقدي
                  </button>
                  <button
                    onClick={() => setPaymentType('BANK_TRANSFER')}
                    className={`py-1.5 rounded-lg font-bold border transition cursor-pointer ${
                      paymentType === 'BANK_TRANSFER'
                        ? 'bg-sky-50 border-sky-500 text-sky-800'
                        : 'bg-slate-50 border-slate-200 text-slate-600'
                    }`}
                  >
                    💳 شبكة / كارت
                  </button>
                  <button
                    onClick={() => setPaymentType('CREDIT')}
                    className={`py-1.5 rounded-lg font-bold border transition cursor-pointer ${
                      paymentType === 'CREDIT'
                        ? 'bg-amber-50 border-amber-500 text-amber-800'
                        : 'bg-slate-50 border-slate-200 text-slate-600'
                    }`}
                  >
                    ⏳ آجل
                  </button>
                </div>

                {/* Summary & Final Checkout */}
                <div className="bg-slate-50 p-2.5 rounded-xl border border-slate-200 space-y-1 text-xs mb-3">
                  <div className="flex justify-between text-slate-600">
                    <span>المجموع الفرعي:</span>
                    <span className="font-mono">{cartSubtotal.toLocaleString()} {currency}</span>
                  </div>
                  <div className="flex justify-between font-bold text-slate-900 text-sm pt-1 border-t border-slate-200">
                    <span>المبلغ الصافي النهائي:</span>
                    <span className="font-mono text-emerald-700">{cartGrandTotal.toLocaleString()} {currency}</span>
                  </div>
                </div>

                <div className="flex items-center gap-2">
                  <button
                    onClick={clearCart}
                    className="p-2.5 bg-rose-50 text-rose-700 border border-rose-200 rounded-xl hover:bg-rose-100 transition cursor-pointer"
                    title="تفريغ السلة"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                  <button
                    onClick={handleCheckout}
                    className="flex-1 bg-emerald-600 hover:bg-emerald-500 text-white py-2.5 rounded-xl text-xs font-bold transition flex items-center justify-center gap-1.5 shadow-xs cursor-pointer"
                  >
                    <CheckCircle2 className="w-4 h-4" />
                    <span>تأكيد الفاتورة وطباعة الإيصال الحراري</span>
                  </button>
                </div>
              </div>
            </div>
          )}

          {/* Success Invoice Modal */}
          {showSuccessModal && (
            <div className="absolute inset-0 bg-black/70 backdrop-blur-2xs z-50 flex items-center justify-center p-4 animate-in fade-in">
              <div className="bg-white rounded-2xl p-4 w-full max-w-xs space-y-3 text-center shadow-2xl animate-in zoom-in-95 text-xs">
                <div className="w-12 h-12 rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center mx-auto">
                  <CheckCircle2 className="w-6 h-6" />
                </div>
                <h3 className="font-bold text-sm text-slate-900">تم إصدار وحفظ الفاتورة بنجاح!</h3>
                <p className="text-slate-500 text-[11px]">
                  فاتورة رقم: <b className="text-sky-700 font-mono">#{showSuccessModal.invoiceNo}</b>
                  <br />
                  الإجمالي: <b className="text-emerald-700 font-mono">{showSuccessModal.grandTotal.toLocaleString()} {currency}</b>
                </p>

                <div className="pt-2 border-t border-slate-200 space-y-1.5">
                  <button
                    onClick={() => {
                      alert('تم إرسال أمر الطباعة إلى طابعة البلوتوث المحمولة 58mm / 80mm.');
                      setShowSuccessModal(null);
                    }}
                    className="w-full bg-indigo-600 hover:bg-indigo-700 text-white py-2 rounded-xl font-bold flex items-center justify-center gap-1.5 cursor-pointer shadow-xs"
                  >
                    <Printer className="w-4 h-4" />
                    <span>طباعة عبر البلوتوث (ESC/POS)</span>
                  </button>

                  <button
                    onClick={() => {
                      alert('تم نسخ رابط الفاتورة الإلكترونية لمشاركتها عبر واتساب العميل.');
                      setShowSuccessModal(null);
                    }}
                    className="w-full bg-emerald-50 hover:bg-emerald-100 text-emerald-800 border border-emerald-300 py-1.5 rounded-xl font-bold flex items-center justify-center gap-1.5 cursor-pointer"
                  >
                    <Share2 className="w-4 h-4 text-emerald-600" />
                    <span>مشاركة عبر واتساب (WhatsApp)</span>
                  </button>

                  <button
                    onClick={() => setShowSuccessModal(null)}
                    className="w-full text-slate-500 py-1 font-bold text-[11px] hover:text-slate-800 cursor-pointer"
                  >
                    إغلاق وبدء فاتورة جديدة
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
