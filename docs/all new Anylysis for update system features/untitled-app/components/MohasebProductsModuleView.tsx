'use client';

import React, { useState, useMemo, useRef, useEffect, useCallback } from 'react';
import { ProductItem, Warehouse } from '@/types/pos';
import * as XLSX from 'xlsx';
import {
  ArrowRight,
  Plus,
  Search,
  FileSpreadsheet,
  MoreVertical,
  Upload,
  Check,
  X,
  ChevronDown,
  Delete,
  FileText,
  Calendar,
  Layers,
  Sparkles,
  Download,
  AlertTriangle,
  AlertCircle,
  Trash2,
  Edit2,
  RotateCcw,
  Building2,
  Tag,
  RefreshCw,
  Eye,
  CheckCircle2,
  HelpCircle,
  CheckCircle,
  Info,
  Warehouse as WarehouseIcon,
  PackagePlus,
  Coins,
  FileDown,
  FolderUp,
  Sliders,
  Filter
} from 'lucide-react';

export type ProductSubView = 
  | 'PRODUCTS_LIST' 
  | 'EXCEL_IMPORT' 
  | 'SALES_PRICES' 
  | 'ITEM_UNITS' 
  | 'PRICE_QUOTATION' 
  | 'PURCHASE_ORDER';

export interface ParsedExcelProduct {
  id: string;
  name: string;
  barcode: string;
  openingQty: number;
  costPrice: number;
  salePrice: number;
  unit: string;
  category: string;
  expiryDate?: string;
  minStock: number;
  status: 'VALID' | 'WARNING_EXISTING' | 'WARNING_DUPLICATE_BARCODE' | 'ERROR_MISSING_NAME';
  statusMessage?: string;
  isAutoBarcode?: boolean;
}

interface MohasebProductsModuleViewProps {
  initialSubView?: ProductSubView;
  products: ProductItem[];
  warehouses?: Warehouse[];
  onAddProduct: (product: ProductItem) => void;
  onUpdateProduct: (product: ProductItem) => void;
  onBulkAddProducts?: (products: ProductItem[]) => void;
  onBack: () => void;
}

export const MohasebProductsModuleView: React.FC<MohasebProductsModuleViewProps> = ({
  initialSubView = 'PRODUCTS_LIST',
  products,
  warehouses = [],
  onAddProduct,
  onUpdateProduct,
  onBulkAddProducts,
  onBack,
}) => {
  const [prevInitialSubView, setPrevInitialSubView] = useState<ProductSubView>(initialSubView);
  const [subView, setSubView] = useState<ProductSubView>(initialSubView);
  const [searchQuery, setSearchQuery] = useState('');
  const [showSearchInput, setShowSearchInput] = useState(false);

  if (initialSubView !== prevInitialSubView) {
    setPrevInitialSubView(initialSubView);
    setSubView(initialSubView);
  }

  // Price Modal State (Screenshot 6)
  const [selectedProductForPrice, setSelectedProductForPrice] = useState<ProductItem | null>(null);
  const [selectedUnit, setSelectedUnit] = useState<'حبة' | 'كرتون'>('حبة');
  const [salePriceInput, setSalePriceInput] = useState<string>('1');
  const [currencyChoice, setCurrencyChoice] = useState<'LOCAL' | 'USD' | 'SAR'>('USD');
  const [priceFeedback, setPriceFeedback] = useState<string | null>(null);

  // ITEM_UNITS Subview State
  const [unitSelectedProductId, setUnitSelectedProductId] = useState<string>(products[0]?.id || '');
  const [unitDefinitions, setUnitDefinitions] = useState<Record<string, Array<{ name: string; factor: number; price: number; cost: number; barcode: string }>>>({
    'P-0999': [
      { name: 'حبة', factor: 1, price: 100, cost: 70, barcode: '629100099999' },
      { name: 'باكت', factor: 12, price: 1100, cost: 800, barcode: '629100099912' },
      { name: 'كرتون', factor: 48, price: 4200, cost: 3100, barcode: '629100099948' },
    ]
  });
  const [newUnitName, setNewUnitName] = useState('كرتون');
  const [newUnitFactor, setNewUnitFactor] = useState<number>(24);
  const [newUnitPrice, setNewUnitPrice] = useState<number>(2400);
  const [newUnitCost, setNewUnitCost] = useState<number>(1800);
  const [newUnitBarcode, setNewUnitBarcode] = useState('');
  const [unitSaveFeedback, setUnitSaveFeedback] = useState<string | null>(null);

  // PRICE_QUOTATION Subview State
  const [quotationNo] = useState('QUOT-2026-003');
  const [quotationCustomer, setQuotationCustomer] = useState('شركة الأمل للتجارة العامة');
  const [quotationDate, setQuotationDate] = useState('2026-06-04');
  const [quotationValidUntil, setQuotationValidUntil] = useState('2026-06-18');
  const [quotationNotes, setQuotationNotes] = useState('الأسعار سارية لمدة أسبوعين من تاريخه');
  const [quotationItems, setQuotationItems] = useState<Array<{ product: ProductItem; quantity: number; unitPrice: number; total: number }>>([
    { product: products[0] || { id: 'P1', name: 'بطاطس عمان', costPrice: 50, salePrice: 70 } as ProductItem, quantity: 20, unitPrice: 70, total: 1400 },
  ]);
  const [quotationSearchText, setQuotationSearchText] = useState('');
  const [quotationFeedback, setQuotationFeedback] = useState<string | null>(null);
  const [quotationDiscountType, setQuotationDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [quotationDiscountVal, setQuotationDiscountVal] = useState<string>('0');

  const quotationSubtotal = useMemo(() => {
    return quotationItems.reduce((sum, it) => sum + it.total, 0);
  }, [quotationItems]);

  const quotationDiscountAmount = useMemo(() => {
    const val = parseFloat(quotationDiscountVal) || 0;
    if (quotationDiscountType === 'PERCENT') {
      return (quotationSubtotal * val) / 100;
    }
    return val;
  }, [quotationDiscountVal, quotationDiscountType, quotationSubtotal]);

  const quotationGrandTotal = Math.max(0, quotationSubtotal - quotationDiscountAmount);

  // PURCHASE_ORDER Subview State
  const [orderNo] = useState('PO-2026-008');
  const [orderSupplier, setOrderSupplier] = useState('مؤسسة البركة للمواد الغذائية');
  const [orderDate, setOrderDate] = useState('2026-06-04');
  const [orderExpectedDate, setOrderExpectedDate] = useState('2026-06-11');
  const [orderNotes, setOrderNotes] = useState('يرجى التوريد إلى المخزن الرئيسي قبل نهاية الأسبوع');
  const [orderItems, setOrderItems] = useState<Array<{ product: ProductItem; quantity: number; unitCost: number; total: number }>>([
    { product: products[0] || { id: 'P1', name: 'بطاطس عمان', costPrice: 50, salePrice: 70 } as ProductItem, quantity: 50, unitCost: 50, total: 2500 },
  ]);
  const [orderSearchText, setOrderSearchText] = useState('');
  const [orderFeedback, setOrderFeedback] = useState<string | null>(null);
  const [orderDiscountType, setOrderDiscountType] = useState<'AMOUNT' | 'PERCENT'>('AMOUNT');
  const [orderDiscountVal, setOrderDiscountVal] = useState<string>('0');

  const orderSubtotal = useMemo(() => {
    return orderItems.reduce((sum, it) => sum + it.total, 0);
  }, [orderItems]);

  const orderDiscountAmount = useMemo(() => {
    const val = parseFloat(orderDiscountVal) || 0;
    if (orderDiscountType === 'PERCENT') {
      return (orderSubtotal * val) / 100;
    }
    return val;
  }, [orderDiscountVal, orderDiscountType, orderSubtotal]);

  const orderGrandTotal = Math.max(0, orderSubtotal - orderDiscountAmount);

  // Add Product Modal State
  const [showAddProductModal, setShowAddProductModal] = useState(false);
  const [newProdName, setNewProdName] = useState('');
  const [newProdCategory, setNewProdCategory] = useState('عام');
  const [newProdOpeningQty, setNewProdOpeningQty] = useState(100);
  const [newProdCost, setNewProdCost] = useState(50);
  const [newProdPrice, setNewProdPrice] = useState(70);
  const [newProdUnit, setNewProdUnit] = useState('حبة');
  const [newProdBarcode, setNewProdBarcode] = useState('');
  const [newProdExpiry, setNewProdExpiry] = useState('2028-05-16');

  // Excel Import State
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [importedItems, setImportedItems] = useState<ParsedExcelProduct[]>([]);
  const [importFileName, setImportFileName] = useState<string>('');
  const [importFileSize, setImportFileSize] = useState<string>('');
  const [importSheetName, setImportSheetName] = useState<string>('');
  const [isProcessingFile, setIsProcessingFile] = useState(false);
  const [importErrorMsg, setImportErrorMsg] = useState<string | null>(null);
  const [importSuccessMsg, setImportSuccessMsg] = useState<string | null>(null);
  const [previewFilter, setPreviewFilter] = useState<'ALL' | 'VALID' | 'WARNINGS'>('ALL');
  const [previewSearch, setPreviewSearch] = useState('');
  const [targetWarehouseId, setTargetWarehouseId] = useState<string>(warehouses[0]?.id || 'WH-01');
  const [defaultCategory, setDefaultCategory] = useState<string>('عام');
  const [duplicatePolicy, setDuplicatePolicy] = useState<'UPDATE' | 'SKIP' | 'APPEND_NEW'>('UPDATE');
  const [isDraggingOver, setIsDraggingOver] = useState(false);

  const [productFilterTab, setProductFilterTab] = useState<'ALL' | 'NO_BARCODE' | 'LOW_STOCK'>('ALL');
  const [barcodeActionFeedback, setBarcodeActionFeedback] = useState<string | null>(null);

  // Helper: Generate a verified unique pure-integer barcode that doesn't exist in system
  const generateUniqueBarcode = useCallback(() => {
    const existingBarcodes = new Set(
      products.map(p => p.barcode ? p.barcode.trim() : '').filter(Boolean)
    );
    let candidateSeq = products.reduce((max, p) => {
      const num = parseInt(p.code, 10);
      return !isNaN(num) && num > max ? num : max;
    }, 1000) + 1;

    let candidate = `629${String(candidateSeq).padStart(8, '0')}`;
    while (existingBarcodes.has(candidate)) {
      candidateSeq++;
      candidate = `629${String(candidateSeq).padStart(8, '0')}`;
    }
    return candidate;
  }, [products]);

  // Count items with no barcode
  const noBarcodeItems = useMemo(() => {
    return products.filter(p => !p.barcode || p.barcode.trim() === '' || p.barcode === 'null' || p.barcode === p.code);
  }, [products]);

  const lowStockItems = useMemo(() => {
    return products.filter(p => p.stock <= p.minStock);
  }, [products]);

  // Bulk generate unique barcodes for all items without barcodes
  const handleBulkGenerateUniqueBarcodes = () => {
    if (noBarcodeItems.length === 0) return;
    const existingBarcodes = new Set(
      products.map(p => p.barcode ? p.barcode.trim() : '').filter(Boolean)
    );
    let candidateSeq = products.reduce((max, p) => {
      const num = parseInt(p.code, 10);
      return !isNaN(num) && num > max ? num : max;
    }, 1000) + 1;

    noBarcodeItems.forEach(item => {
      let candidate = `629${String(candidateSeq).padStart(8, '0')}`;
      while (existingBarcodes.has(candidate)) {
        candidateSeq++;
        candidate = `629${String(candidateSeq).padStart(8, '0')}`;
      }
      existingBarcodes.add(candidate);
      candidateSeq++;

      const updatedItem: ProductItem = {
        ...item,
        barcode: candidate,
        units: item.units ? item.units.map((u, i) => i === 0 ? { ...u, barcode: candidate } : u) : [
          { name: item.unit || 'حبة', factor: 1, price: item.salePrice, cost: item.costPrice, barcode: candidate }
        ]
      };
      onUpdateProduct(updatedItem);
    });

    setBarcodeActionFeedback(`تم توليد أرقام باركود فريدة بنجاح لعدد ${noBarcodeItems.length} صنف!`);
    setTimeout(() => setBarcodeActionFeedback(null), 3500);
  };

  // Filtered products list
  const filteredProducts = useMemo(() => {
    return products.filter((p) => {
      if (productFilterTab === 'NO_BARCODE') {
        const isNoBarcode = !p.barcode || p.barcode.trim() === '' || p.barcode === 'null' || p.barcode === p.code;
        if (!isNoBarcode) return false;
      } else if (productFilterTab === 'LOW_STOCK') {
        if (p.stock > p.minStock) return false;
      }

      if (!searchQuery.trim()) return true;
      const query = searchQuery.toLowerCase();
      return (
        p.name.toLowerCase().includes(query) ||
        p.code.toLowerCase().includes(query) ||
        (p.barcode && p.barcode.toLowerCase().includes(query)) ||
        p.category.toLowerCase().includes(query)
      );
    });
  }, [products, searchQuery, productFilterTab]);

  // Open price modal for an item (matching Screenshot 6)
  const handleOpenPriceModal = (product: ProductItem) => {
    setSelectedProductForPrice(product);
    setSelectedUnit('حبة');
    setSalePriceInput('1');
    setCurrencyChoice('USD');
  };

  // Touch keypad input handling (Screenshot 6 numpad)
  const handleKeypadPress = (val: string) => {
    if (val === 'BACKSPACE') {
      setSalePriceInput((prev) => (prev.length > 1 ? prev.slice(0, -1) : ''));
    } else if (val === 'DONE') {
      handleSavePriceModal();
    } else if (val === '.') {
      if (!salePriceInput.includes('.')) {
        setSalePriceInput((prev) => prev + '.');
      }
    } else {
      setSalePriceInput((prev) => (prev === '0' ? val : prev + val));
    }
  };

  const handleSavePriceModal = () => {
    if (!selectedProductForPrice) return;
    const numericPrice = parseFloat(salePriceInput) || 0;
    
    // Save to product
    const updated: ProductItem = {
      ...selectedProductForPrice,
      salePrice: currencyChoice === 'LOCAL' ? numericPrice : Math.round(numericPrice * 700),
    };
    onUpdateProduct(updated);
    setPriceFeedback(`تم تحديث سعر ${selectedProductForPrice.name} بنجاح!`);
    setTimeout(() => {
      setPriceFeedback(null);
      setSelectedProductForPrice(null);
    }, 900);
  };

  // Add new single product
  const handleSaveNewProduct = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newProdName.trim()) return;

    const nextCode = products.reduce((max, p) => {
      const num = parseInt(p.code, 10);
      return !isNaN(num) && num > max ? num : max;
    }, 1000) + 1;
    const finalBarcode = newProdBarcode.trim();

    const newProd: ProductItem = {
      id: `P-${nextCode}`,
      code: nextCode.toString(),
      barcode: finalBarcode,
      name: newProdName.trim(),
      category: newProdCategory.trim() || 'عام',
      unit: newProdUnit,
      units: [
        { name: newProdUnit, factor: 1, price: newProdPrice, cost: newProdCost, barcode: finalBarcode },
      ],
      costPrice: newProdCost,
      salePrice: newProdPrice,
      minSalePrice: newProdPrice * 0.9,
      stock: newProdOpeningQty,
      minStock: 10,
      hasExpiryDate: !!newProdExpiry,
      expiryDate: newProdExpiry,
      location: 'المخزن الرئيسي',
    };

    onAddProduct(newProd);
    setShowAddProductModal(false);
    setNewProdName('');
    setNewProdBarcode('');
  };

  // Helper: Parse dates from various Excel and text formats to YYYY-MM-DD
  const parseExcelDate = (val: any): string | undefined => {
    if (!val) return undefined;
    if (val instanceof Date) {
      const y = val.getFullYear();
      const m = String(val.getMonth() + 1).padStart(2, '0');
      const d = String(val.getDate()).padStart(2, '0');
      return `${y}-${m}-${d}`;
    }
    if (typeof val === 'number') {
      // Excel serial date to JS Date
      const date = new Date(Math.round((val - 25569) * 86400 * 1000));
      if (!isNaN(date.getTime())) {
        const y = date.getFullYear();
        const m = String(date.getMonth() + 1).padStart(2, '0');
        const d = String(date.getDate()).padStart(2, '0');
        return `${y}-${m}-${d}`;
      }
    }
    const str = String(val).trim();
    if (!str) return undefined;
    if (/^\d{4}-\d{2}-\d{2}$/.test(str)) return str;
    if (/^\d{1,2}[\/-]\d{1,2}[\/-]\d{4}$/.test(str)) {
      const parts = str.split(/[\/-]/);
      if (parts[0].length === 4) {
        return `${parts[0]}-${parts[1].padStart(2, '0')}-${parts[2].padStart(2, '0')}`;
      }
      return `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}`;
    }
    return str.slice(0, 10);
  };

  // Helper: Parse numeric values safely
  const parseExcelNumber = (val: any, fallback = 0): number => {
    if (val === undefined || val === null || val === '') return fallback;
    if (typeof val === 'number') return isNaN(val) ? fallback : val;
    const cleaned = String(val).replace(/[^0-9.-]/g, '');
    const n = parseFloat(cleaned);
    return isNaN(n) ? fallback : n;
  };

  // Download official Excel/CSV Template
  const handleDownloadExcelTemplate = (mode: 'sample' | 'blank', format: 'xlsx' | 'csv' = 'xlsx') => {
    const headers = [
      'إسم الصنف',
      'الكمية الإفتتاحية',
      'سعر التكلفة',
      'سعر البيع',
      'الوحدة الأساسية',
      'الباركود',
      'تاريخ الإنتهاء',
      'التصنيف',
      'الحد الأدنى للمخزون'
    ];

    const sampleRows = [
      ['تلفون سامسونج S5', 3, 50000, 70000, 'حبة', '123652145632145565', '2029-12-31', 'إلكترونيات', 5],
      ['تلفون سامسونج S7', 0, 0, 0, 'بدون', '', '', 'إلكترونيات', 5],
      ['حليب ممتاز مبخر 170 جم', 120, 350, 450, 'حبة', '629100101', '2027-12-31', 'معلبات وألبان', 20],
      ['أرز الشعلان سيلا بسمتي 5 كجم', 50, 5200, 6000, 'كيس', '629100102', '2028-06-30', 'حبوب وبقوليات', 10],
      ['زيت صويا الجود 1.8 لتر', 80, 2100, 2500, 'حبة', '629100103', '2027-08-15', 'زيوت ودهون', 15],
      ['تونة قودي مايلد 185 جم', 150, 680, 850, 'حبة', '629100104', '2027-05-20', 'معلبات وألبان', 30],
      ['شاي الكبوس أحمر ناعم 227 جم', 200, 420, 550, 'باكت', '629100105', '2028-01-01', 'مشروبات وشاي', 25],
    ];

    const rows = mode === 'sample' ? sampleRows : [];

    if (format === 'xlsx') {
      const wb = XLSX.utils.book_new();
      const data = [headers, ...rows];
      const ws = XLSX.utils.aoa_to_sheet(data);
      ws['!cols'] = [
        { wch: 32 }, // اسم الصنف
        { wch: 16 }, // الكمية الافتتاحية
        { wch: 14 }, // سعر التكلفة
        { wch: 14 }, // سعر البيع
        { wch: 16 }, // الوحدة الأساسية
        { wch: 22 }, // الباركود
        { wch: 16 }, // تاريخ الانتهاء
        { wch: 18 }, // التصنيف
        { wch: 16 }, // الحد الأدنى
      ];
      ws['!views'] = [{ RTL: true }];
      (ws as any)['!sheetViews'] = [{ RTL: true }];
      XLSX.utils.book_append_sheet(wb, ws, 'دليل_الأصناف');
      const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
      const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', mode === 'sample' 
        ? 'نموذج_استيراد_الاصناف_محاسب_سوفت_بيانات_توضيحية.xlsx' 
        : 'نموذج_استيراد_الاصناف_محاسب_سوفت_فارغ.xlsx');
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(url);
    } else {
      // CSV format with UTF-8 BOM
      const csvContent = '\uFEFF' + [headers, ...rows]
        .map(row => row.map(cell => `"${String(cell ?? '').replace(/"/g, '""')}"`).join(','))
        .join('\n');
      const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'نموذج_استيراد_الاصناف_محاسب_سوفت.csv');
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(url);
    }
  };

  // Process uploaded Excel / CSV File
  const processFile = (file: File) => {
    setIsProcessingFile(true);
    setImportErrorMsg(null);
    setImportSuccessMsg(null);
    setImportFileName(file.name);
    setImportFileSize((file.size / 1024).toFixed(1) + ' KB');

    const reader = new FileReader();

    reader.onload = (event) => {
      try {
        const data = event.target?.result as ArrayBuffer;
        const workbook = XLSX.read(data, { type: 'array', cellDates: true });
        const sheetName = workbook.SheetNames[0];
        setImportSheetName(sheetName || 'Sheet1');
        const worksheet = workbook.Sheets[sheetName];
        const rawRows: any[][] = XLSX.utils.sheet_to_json(worksheet, { header: 1, defval: '' });

        if (!rawRows || rawRows.length === 0) {
          setImportErrorMsg('الملف فارغ، يرجى ملء بيانات الأصناف في النموذج المعتمد وإعادة الرفع.');
          setIsProcessingFile(false);
          return;
        }

        // Find header row dynamically
        let headerRowIdx = -1;
        for (let r = 0; r < Math.min(rawRows.length, 5); r++) {
          const rowStr = rawRows[r].map(c => String(c || '').trim().toLowerCase()).join(' ');
          if (rowStr.includes('اسم') || rowStr.includes('إسم') || rowStr.includes('صنف') || rowStr.includes('name') || rowStr.includes('باركود') || rowStr.includes('barcode') || rowStr.includes('كمية') || rowStr.includes('تكلفة')) {
            headerRowIdx = r;
            break;
          }
        }

        let dataRows: any[][] = [];
        let nameIdx = 0;
        let qtyIdx = 1;
        let costIdx = 2;
        let priceIdx = 3;
        let unitIdx = 4;
        let barcodeIdx = 5;
        let expiryIdx = 6;
        let categoryIdx = 7;
        let minStockIdx = 8;

        if (headerRowIdx !== -1) {
          const headerRow = rawRows[headerRowIdx];
          dataRows = rawRows.slice(headerRowIdx + 1);

          headerRow.forEach((cell, idx) => {
            const text = String(cell || '').trim().toLowerCase();
            if (text.includes('اسم') || text.includes('إسم') || text.includes('صنف') || text.includes('name') || text.includes('item')) {
              nameIdx = idx;
            } else if (text.includes('باركود') || text.includes('بار كود') || text.includes('barcode')) {
              barcodeIdx = idx;
            } else if (text.includes('كمية') || text.includes('كميه') || text.includes('افتتاح') || text.includes('رصيد') || text.includes('qty') || text.includes('stock')) {
              qtyIdx = idx;
            } else if (text.includes('تكل') || text.includes('cost') || text.includes('شراء')) {
              costIdx = idx;
            } else if (text.includes('بيع') || text.includes('سعر') || text.includes('price') || text.includes('sale')) {
              priceIdx = idx;
            } else if (text.includes('وحد') || text.includes('unit')) {
              unitIdx = idx;
            } else if (text.includes('انته') || text.includes('إنتها') || text.includes('صلاحي') || text.includes('expir')) {
              expiryIdx = idx;
            } else if (text.includes('تصنيف') || text.includes('قسم') || text.includes('مجموع') || text.includes('cat')) {
              categoryIdx = idx;
            } else if (text.includes('أدنى') || text.includes('ادنى') || text.includes('طلب') || text.includes('min')) {
              minStockIdx = idx;
            }
          });
        } else {
          // No recognizable header row, treat all as data
          dataRows = rawRows;
          // Check if layout matches 7-columns format: Name, Qty, Cost, Price, Unit, Barcode, Expiry
          if (dataRows[0] && dataRows[0].length >= 6) {
            nameIdx = 0;
            qtyIdx = 1;
            costIdx = 2;
            priceIdx = 3;
            unitIdx = 4;
            barcodeIdx = 5;
            expiryIdx = 6;
          }
        }

        const existingBarcodes = new Set(products.map(p => p.barcode?.trim()).filter(Boolean));
        const seenBarcodesInFile = new Set<string>();
        const parsedItems: ParsedExcelProduct[] = [];

        const highestExistingCode = products.reduce((max, p) => {
          const num = parseInt(p.code, 10);
          return !isNaN(num) && num > max ? num : max;
        }, 1000);
        let autoCodeSequence = highestExistingCode;

        dataRows.forEach((row, rowIdx) => {
          // Skip completely empty rows
          if (!row || row.every(cell => String(cell || '').trim() === '')) return;

          const rawName = String(row[nameIdx] || '').trim();
          let rawBarcode = String(row[barcodeIdx] || '').trim();
          const rawQty = parseExcelNumber(row[qtyIdx], 0);
          const rawCost = parseExcelNumber(row[costIdx], 0);
          const rawPrice = parseExcelNumber(row[priceIdx], 0);
          const rawUnit = String(row[unitIdx] || '').trim() || 'حبة';
          const rawExpiry = parseExcelDate(row[expiryIdx]);
          const rawCategory = String(row[categoryIdx] || '').trim() || defaultCategory || 'عام';
          const rawMinStock = parseExcelNumber(row[minStockIdx], 10);

          let isAutoBarcode = false;
          if (!rawBarcode) {
            autoCodeSequence++;
            rawBarcode = autoCodeSequence.toString();
            isAutoBarcode = true;
          }

          let status: ParsedExcelProduct['status'] = 'VALID';
          let statusMessage: string | undefined = undefined;

          if (!rawName) {
            status = 'ERROR_MISSING_NAME';
            statusMessage = 'اسم الصنف مفقود أو فارغ';
          } else if (seenBarcodesInFile.has(rawBarcode)) {
            status = 'WARNING_DUPLICATE_BARCODE';
            statusMessage = 'الباركود مكرر أكثر من مرة في هذا الملف';
          } else if (existingBarcodes.has(rawBarcode)) {
            status = 'WARNING_EXISTING';
            statusMessage = 'الباركود مسجل لصنف موجود مسبقاً في النظام';
          }

          seenBarcodesInFile.add(rawBarcode);

          parsedItems.push({
            id: `IMP-${Date.now()}-${rowIdx}`,
            name: rawName || `صنف غير مسمى ${rowIdx + 1}`,
            barcode: rawBarcode,
            openingQty: rawQty,
            costPrice: rawCost,
            salePrice: rawPrice,
            unit: rawUnit,
            category: rawCategory,
            expiryDate: rawExpiry,
            minStock: rawMinStock,
            status,
            statusMessage,
            isAutoBarcode,
          });
        });

        if (parsedItems.length === 0) {
          setImportErrorMsg('لم يتم العثور على أسطر صالحة في الملف، يرجى التأكد من البيانات.');
        } else {
          setImportedItems(parsedItems);
        }
      } catch (err: any) {
        console.error('Error parsing Excel:', err);
        setImportErrorMsg('تعذر قراءة ملف الإكسل. يرجى التأكد من أن صيغة الملف XLSX أو XLS أو CSV صالحة.');
      } finally {
        setIsProcessingFile(false);
      }
    };

    reader.readAsArrayBuffer(file);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    processFile(file);
  };

  // Load sample demo items for testing without having an actual file
  const handleLoadDemoItems = () => {
    setIsProcessingFile(true);
    setTimeout(() => {
      setImportFileName('عينة_أصناف_تجريبية_سوبرماركت.xlsx');
      setImportFileSize('14.2 KB');
      setImportSheetName('دليل_الأصناف');
      const existingBarcodes = new Set(products.map(p => p.barcode?.trim()).filter(Boolean));

      const demoList: ParsedExcelProduct[] = [
        {
          id: `IMP-DEMO-1`,
          name: 'حليب ممتاز مبخر 170 جم',
          barcode: '629100101',
          openingQty: 120,
          costPrice: 350,
          salePrice: 450,
          unit: 'حبة',
          category: 'معلبات وألبان',
          expiryDate: '2027-12-31',
          minStock: 20,
          status: existingBarcodes.has('629100101') ? 'WARNING_EXISTING' : 'VALID',
          statusMessage: existingBarcodes.has('629100101') ? 'الباركود مسجل لصنف موجود مسبقاً' : undefined,
        },
        {
          id: `IMP-DEMO-2`,
          name: 'أرز الشعلان سيلا بسمتي 5 كجم',
          barcode: '629100102',
          openingQty: 50,
          costPrice: 5200,
          salePrice: 6000,
          unit: 'كيس',
          category: 'حبوب وبقوليات',
          expiryDate: '2028-06-30',
          minStock: 10,
          status: 'VALID',
        },
        {
          id: `IMP-DEMO-3`,
          name: 'زيت صويا الجود 1.8 لتر',
          barcode: '629100103',
          openingQty: 80,
          costPrice: 2100,
          salePrice: 2500,
          unit: 'حبة',
          category: 'زيوت ودهون',
          expiryDate: '2027-08-15',
          minStock: 15,
          status: 'VALID',
        },
        {
          id: `IMP-DEMO-4`,
          name: 'تونة قودي مايلد 185 جم',
          barcode: '629100104',
          openingQty: 150,
          costPrice: 680,
          salePrice: 850,
          unit: 'حبة',
          category: 'معلبات وألبان',
          expiryDate: '2027-05-20',
          minStock: 30,
          status: 'VALID',
        },
        {
          id: `IMP-DEMO-5`,
          name: 'شاي الكبوس أحمر ناعم 227 جم',
          barcode: '629100105',
          openingQty: 200,
          costPrice: 420,
          salePrice: 550,
          unit: 'باكت',
          category: 'مشروبات وشاي',
          expiryDate: '2028-01-01',
          minStock: 25,
          status: 'VALID',
        },
        {
          id: `IMP-DEMO-6`,
          name: 'سكر الأسرة ناعم 10 كجم',
          barcode: '629100106',
          openingQty: 40,
          costPrice: 9800,
          salePrice: 11000,
          unit: 'كيس',
          category: 'مواد أساسية',
          expiryDate: '2029-12-31',
          minStock: 8,
          status: 'VALID',
        },
        {
          id: `IMP-DEMO-7`,
          name: 'معكرونة برفيتو سباغيتي 500 جم',
          barcode: '629100107',
          openingQty: 95,
          costPrice: 450,
          salePrice: 600,
          unit: 'حبة',
          category: 'معكرونة وباستا',
          expiryDate: '2027-09-15',
          minStock: 20,
          status: 'VALID',
        },
      ];

      setImportedItems(demoList);
      setIsProcessingFile(false);
    }, 400);
  };

  // Remove a row from imported preview
  const handleRemoveItem = (id: string) => {
    setImportedItems(prev => prev.filter(it => it.id !== id));
  };

  // Quick inline update for a field in the preview
  const handleUpdateItemField = (id: string, field: keyof ParsedExcelProduct, value: any) => {
    setImportedItems(prev => prev.map(item => {
      if (item.id !== id) return item;
      const updated = { ...item, [field]: value };
      if (field === 'barcode') {
        updated.isAutoBarcode = false;
      }
      if (field === 'name') {
        if (!String(value).trim()) {
          updated.status = 'ERROR_MISSING_NAME';
          updated.statusMessage = 'اسم الصنف مفقود';
        } else if (item.status === 'ERROR_MISSING_NAME') {
          updated.status = 'VALID';
          updated.statusMessage = undefined;
        }
      }
      return updated;
    }));
  };

  // Generate barcodes for any empty or auto barcodes (Sequential pure integers)
  const handleGenerateBarcodesForEmpty = () => {
    let baseSeq = products.reduce((max, p) => {
      const num = parseInt(p.code, 10);
      return !isNaN(num) && num > max ? num : max;
    }, 1000);
    setImportedItems(prev => prev.map(it => {
      if (it.barcode && !it.isAutoBarcode) return it;
      baseSeq++;
      return {
        ...it,
        barcode: baseSeq.toString(),
        isAutoBarcode: true,
      };
    }));
  };

  // Save parsed items into system products & warehouses
  const handleSaveImportedItems = () => {
    if (importedItems.length === 0) return;

    const validItems = importedItems.filter(it => it.status !== 'ERROR_MISSING_NAME');
    if (validItems.length === 0) {
      alert('لا توجد أصناف صالحة للاستيراد (يرجى تصحيح الأصناف ذات الأسماء المفقودة).');
      return;
    }

    const targetWh = (warehouses && warehouses.length > 0)
      ? warehouses.find(w => w.id === targetWarehouseId) || warehouses[0]
      : { id: 'WH-01', name: 'المخزن الرئيسي', code: '1' };

    let highestCode = products.reduce((max, p) => {
      const num = parseInt(p.code, 10);
      return !isNaN(num) && num > max ? num : max;
    }, 1000);

    const existingProductsMap = new Map(products.map(p => [p.barcode.trim(), p]));
    const newProductsToAdd: ProductItem[] = [];
    const updatedExistingList: ProductItem[] = [];

    validItems.forEach((it) => {
      const existing = existingProductsMap.get(it.barcode.trim());

      if (existing && duplicatePolicy === 'SKIP') {
        return;
      }

      if (existing && duplicatePolicy === 'UPDATE') {
        const updated: ProductItem = {
          ...existing,
          salePrice: it.salePrice > 0 ? it.salePrice : existing.salePrice,
          costPrice: it.costPrice > 0 ? it.costPrice : existing.costPrice,
          stock: existing.stock + it.openingQty,
          category: it.category || existing.category,
          unit: it.unit || existing.unit,
          expiryDate: it.expiryDate || existing.expiryDate,
          hasExpiryDate: !!(it.expiryDate || existing.expiryDate),
          location: targetWh.name,
        };
        updatedExistingList.push(updated);
        return;
      }

      highestCode += 1;
      const finalBarcode = it.isAutoBarcode
        ? highestCode.toString()
        : ((existing && duplicatePolicy === 'APPEND_NEW')
          ? `${it.barcode}-${highestCode}`
          : (it.barcode.trim() || highestCode.toString()));

      const newProd: ProductItem = {
        id: `P-${highestCode}`,
        code: highestCode.toString(),
        barcode: finalBarcode,
        name: it.name,
        category: it.category || defaultCategory || 'عام',
        unit: it.unit || 'حبة',
        units: [
          {
            name: it.unit || 'حبة',
            factor: 1,
            price: it.salePrice,
            cost: it.costPrice,
            barcode: finalBarcode
          }
        ],
        costPrice: it.costPrice,
        salePrice: it.salePrice,
        minSalePrice: it.salePrice > 0 ? it.salePrice * 0.9 : it.costPrice,
        stock: it.openingQty,
        minStock: it.minStock || 10,
        hasExpiryDate: !!it.expiryDate,
        expiryDate: it.expiryDate,
        location: targetWh.name,
        stockByWarehouse: {
          [targetWh.id]: it.openingQty
        },
        warehouseStocks: [
          {
            warehouseId: targetWh.id,
            quantity: it.openingQty
          }
        ],
        fifoLayers: it.openingQty > 0 ? [
          {
            id: `layer-${highestCode}-1`,
            batchNo: `BATCH-INIT-${highestCode}`,
            initialQuantity: it.openingQty,
            remainingQuantity: it.openingQty,
            unitCost: it.costPrice,
            date: new Date().toISOString().slice(0, 10),
            expiryDate: it.expiryDate,
            warehouseId: targetWh.id
          }
        ] : []
      };

      newProductsToAdd.push(newProd);
    });

    if (updatedExistingList.length > 0) {
      updatedExistingList.forEach(up => onUpdateProduct(up));
    }

    if (newProductsToAdd.length > 0) {
      if (onBulkAddProducts) {
        onBulkAddProducts(newProductsToAdd);
      } else {
        newProductsToAdd.forEach(p => onAddProduct(p));
      }
    }

    const totalImported = newProductsToAdd.length + updatedExistingList.length;
    const totalValuation = validItems.reduce((sum, it) => sum + (it.openingQty * it.costPrice), 0);

    setImportSuccessMsg(`تم استيراد ومعالجة ${totalImported} صنف بنجاح! تم قيد بضاعة أول المدة بقيمة ${totalValuation.toLocaleString()} ريال في مستودع (${targetWh.name}).`);

    setTimeout(() => {
      setImportSuccessMsg(null);
      setImportedItems([]);
      setImportFileName('');
      setSubView('PRODUCTS_LIST');
    }, 2200);
  };

  return (
    <div id="mohaseb-products-module" className="flex flex-col h-full bg-white font-sans select-none" dir="rtl">
      {/* 1. Top Header Bar (matches Screenshot 5, 4, 6) */}
      <header className="bg-[#0277bd] text-white px-4 py-3 flex items-center justify-between shadow-md shrink-0">
        <div className="flex items-center gap-3">
          <button
            onClick={() => {
              if (subView !== 'PRODUCTS_LIST') {
                setSubView('PRODUCTS_LIST');
              } else {
                onBack();
              }
            }}
            className="p-1 hover:bg-white/20 rounded-full transition cursor-pointer text-white"
            title="رجوع"
          >
            <ArrowRight className="w-6 h-6" />
          </button>
          <h1 className="text-lg font-bold">
            {subView === 'PRODUCTS_LIST' && 'الأصناف'}
            {subView === 'EXCEL_IMPORT' && 'إستيراد من إكسل'}
            {subView === 'SALES_PRICES' && 'أسعار البيع'}
            {subView === 'ITEM_UNITS' && 'وحدات الصنف'}
            {subView === 'PRICE_QUOTATION' && 'فاتورة عرض سعر'}
            {subView === 'PURCHASE_ORDER' && 'طلب شراء'}
          </h1>
        </div>

        {/* Header Right Actions */}
        <div className="flex items-center gap-2">
          {subView !== 'EXCEL_IMPORT' && (
            <>
              {/* Search Toggle */}
              <button
                onClick={() => setShowSearchInput(!showSearchInput)}
                className="p-1.5 hover:bg-white/20 rounded-full transition text-white"
                title="بحث"
              >
                <Search className="w-5 h-5" />
              </button>

              {/* Adobe PDF Icon (matching screenshot) */}
              <button
                onClick={() => window.print()}
                className="p-1.5 hover:bg-white/20 rounded-full transition text-white flex items-center justify-center"
                title="طباعة / تصدير PDF"
              >
                <div className="w-6 h-6 bg-red-600 rounded-sm flex items-center justify-center font-bold text-[10px] text-white shadow-xs">
                  PDF
                </div>
              </button>

              {/* 3 Dots Menu */}
              <button
                onClick={() => {
                  setSubView(subView === 'PRODUCTS_LIST' ? 'SALES_PRICES' : 'PRODUCTS_LIST');
                }}
                className="p-1.5 hover:bg-white/20 rounded-full transition text-white"
                title="خيارات إضافية"
              >
                <MoreVertical className="w-5 h-5" />
              </button>
            </>
          )}
        </div>
      </header>

      {/* Sub-Header Search Bar if toggled */}
      {showSearchInput && subView !== 'EXCEL_IMPORT' && (
        <div className="bg-sky-50 border-b border-sky-100 p-2 flex items-center gap-2">
          <Search className="w-4 h-4 text-sky-700" />
          <input
            type="text"
            placeholder="بحث باسم الصنف أو الباركود أو الكود..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="flex-1 bg-white border border-sky-200 rounded-lg px-3 py-1.5 text-xs outline-hidden focus:ring-2 focus:ring-[#0277bd]"
            autoFocus
          />
          {searchQuery && (
            <button onClick={() => setSearchQuery('')} className="text-slate-400 hover:text-slate-600">
              <X className="w-4 h-4" />
            </button>
          )}
        </div>
      )}

      {/* 2. Main Content Area */}
      <main className="flex-1 overflow-y-auto bg-slate-50">
        {/* ============================================================== */}
        {/* VIEW 1: الأصناف (Screenshot 5) */}
        {/* ============================================================== */}
        {subView === 'PRODUCTS_LIST' && (
          <div className="w-full">
            {/* Filter Tabs Bar & Quick Actions */}
            <div className="bg-slate-100 border-b border-slate-200 px-4 py-2 flex flex-wrap items-center justify-between gap-2 text-xs">
              <div className="flex items-center gap-1.5 bg-slate-200/80 p-0.5 rounded-lg border border-slate-300">
                <button
                  type="button"
                  onClick={() => setProductFilterTab('ALL')}
                  className={`px-3 py-1 rounded-md font-bold transition cursor-pointer ${
                    productFilterTab === 'ALL'
                      ? 'bg-white text-slate-900 shadow-xs'
                      : 'text-slate-600 hover:text-slate-900'
                  }`}
                >
                  جميع الأصناف ({products.length})
                </button>
                <button
                  type="button"
                  onClick={() => setProductFilterTab('NO_BARCODE')}
                  className={`px-3 py-1 rounded-md font-bold transition cursor-pointer flex items-center gap-1 ${
                    productFilterTab === 'NO_BARCODE'
                      ? 'bg-amber-600 text-white shadow-xs'
                      : 'text-amber-800 hover:text-amber-900 hover:bg-amber-100/50'
                  }`}
                >
                  <span>بدون باركود</span>
                  {noBarcodeItems.length > 0 && (
                    <span className={`px-1.5 py-0.2 rounded-full text-[10px] font-mono ${
                      productFilterTab === 'NO_BARCODE' ? 'bg-amber-800 text-white' : 'bg-amber-200 text-amber-900'
                    }`}>
                      {noBarcodeItems.length}
                    </span>
                  )}
                </button>
                <button
                  type="button"
                  onClick={() => setProductFilterTab('LOW_STOCK')}
                  className={`px-3 py-1 rounded-md font-bold transition cursor-pointer flex items-center gap-1 ${
                    productFilterTab === 'LOW_STOCK'
                      ? 'bg-rose-600 text-white shadow-xs'
                      : 'text-rose-800 hover:text-rose-900 hover:bg-rose-100/50'
                  }`}
                >
                  <span>شبه نافدة</span>
                  {lowStockItems.length > 0 && (
                    <span className={`px-1.5 py-0.2 rounded-full text-[10px] font-mono ${
                      productFilterTab === 'LOW_STOCK' ? 'bg-rose-800 text-white' : 'bg-rose-200 text-rose-900'
                    }`}>
                      {lowStockItems.length}
                    </span>
                  )}
                </button>
              </div>

              {/* Action when NO_BARCODE filter is active */}
              {productFilterTab === 'NO_BARCODE' && noBarcodeItems.length > 0 && (
                <button
                  type="button"
                  onClick={handleBulkGenerateUniqueBarcodes}
                  className="bg-amber-600 hover:bg-amber-700 text-white font-bold px-3 py-1 rounded-lg text-xs flex items-center gap-1 shadow-xs transition cursor-pointer"
                  title="توليد باركودات فريدة تلقائية ومفحوصة لجميع الأصناف الفارغة"
                >
                  <Sparkles className="w-3.5 h-3.5 text-amber-200" />
                  <span>توليد باركود فريد لجميع الأصناف بدون باركود ({noBarcodeItems.length})</span>
                </button>
              )}
            </div>

            {barcodeActionFeedback && (
              <div className="bg-emerald-50 border-b border-emerald-200 text-emerald-800 px-4 py-2 text-xs font-bold text-center">
                {barcodeActionFeedback}
              </div>
            )}

            <table className="w-full text-right text-sm border-collapse">
              <thead>
                <tr className="bg-[#0277bd] text-white border-b border-[#01579b]">
                  <th className="py-2.5 px-4 font-bold text-sm">إسم الصنف</th>
                  <th className="py-2.5 px-4 font-bold text-sm">مجموعة الصنف</th>
                  <th className="py-2.5 px-4 font-bold text-sm text-center">ك/الإفتتاحية</th>
                  <th className="py-2.5 px-4 font-bold text-sm text-left">تكلفة الوحدة</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200 bg-white">
                {filteredProducts.map((item) => {
                  const hasNoBarcode = !item.barcode || item.barcode.trim() === '' || item.barcode === 'null' || item.barcode === item.code;
                  return (
                    <tr
                      key={item.id}
                      onClick={() => handleOpenPriceModal(item)}
                      className="hover:bg-sky-50/60 cursor-pointer transition"
                    >
                      <td className="py-2.5 px-4">
                        <div className="font-bold text-slate-900 text-sm flex items-center gap-2">
                          <span>{item.name}</span>
                          {hasNoBarcode && (
                            <span className="bg-amber-100 text-amber-800 border border-amber-300 text-[10px] px-1.5 py-0.2 rounded font-medium" title="صنف بدون باركود">
                              بدون باركود
                            </span>
                          )}
                        </div>
                        <div className="text-[11px] text-slate-500 font-sans mt-0.5 flex items-center gap-2">
                          <span>{item.unit}</span>
                          {item.barcode && !hasNoBarcode && <span className="font-mono text-slate-400">• {item.barcode}</span>}
                          {item.expiryDate && <span>• {item.expiryDate}</span>}
                        </div>
                      </td>
                      <td className="py-2.5 px-4 text-slate-700 text-sm">{item.category || 'عام'}</td>
                      <td className="py-2.5 px-4 text-slate-900 font-bold font-mono text-center">
                        {item.stock}
                      </td>
                      <td className="py-2.5 px-4 text-slate-900 font-bold font-mono text-left">
                        {item.costPrice.toLocaleString()}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 2: إستيراد من إكسل (Comprehensive Excel Import Hub) */}
        {/* ============================================================== */}
        {subView === 'EXCEL_IMPORT' && (() => {
          const validCount = importedItems.filter(it => it.status === 'VALID').length;
          const warningsCount = importedItems.filter(it => it.status === 'WARNING_EXISTING' || it.status === 'WARNING_DUPLICATE_BARCODE').length;
          const errorsCount = importedItems.filter(it => it.status === 'ERROR_MISSING_NAME').length;
          const totalValuation = importedItems.reduce((sum, it) => sum + (it.openingQty * it.costPrice), 0);

          const filteredImportedItems = importedItems.filter(it => {
            if (previewFilter === 'VALID' && it.status !== 'VALID') return false;
            if (previewFilter === 'WARNINGS' && it.status !== 'WARNING_EXISTING' && it.status !== 'WARNING_DUPLICATE_BARCODE') return false;
            if (previewSearch.trim()) {
              const q = previewSearch.toLowerCase();
              return it.name.toLowerCase().includes(q) || it.barcode.toLowerCase().includes(q) || it.category.toLowerCase().includes(q);
            }
            return true;
          });

          return (
            <div className="flex flex-col min-h-full bg-slate-100/90 -m-4 sm:-m-6">
              {/* Top Bar - Header matching screenshot */}
              <div className="bg-[#0277bd] text-white px-4 py-3 flex items-center justify-between shadow-xs">
                <button
                  type="button"
                  onClick={() => setSubView('PRODUCTS_LIST')}
                  className="p-1 hover:bg-white/10 rounded-lg text-white transition cursor-pointer flex items-center gap-1"
                  title="رجوع للأصناف"
                >
                  <ArrowRight className="w-5 h-5 text-white" />
                </button>
                <h2 className="text-base sm:text-lg font-bold text-white text-center flex-1">
                  إستيراد من إكسل
                </h2>
                <div className="w-6"></div>
              </div>

              {/* Feedback Alerts */}
              {importErrorMsg && (
                <div className="mx-4 mt-4 p-3.5 bg-rose-50 border border-rose-200 rounded-xl text-rose-800 text-xs font-medium flex items-center gap-2 animate-in fade-in">
                  <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
                  <span>{importErrorMsg}</span>
                </div>
              )}

              {importSuccessMsg && (
                <div className="mx-4 mt-4 p-4 bg-emerald-50 border border-emerald-300 rounded-xl text-emerald-900 text-sm font-bold flex items-center gap-3 animate-in fade-in shadow-xs">
                  <CheckCircle className="w-5 h-5 text-emerald-600 shrink-0" />
                  <div className="flex-1">{importSuccessMsg}</div>
                </div>
              )}

              {/* Hidden File Input */}
              <input
                type="file"
                ref={fileInputRef}
                onChange={handleFileChange}
                accept=".xlsx, .xls, .csv"
                className="hidden"
              />

              {/* PHASE 1: NO FILE LOADED YET - MATCHES USER SCREENSHOT 1:1 */}
              {importedItems.length === 0 && (
                <div className="p-4 sm:p-8 max-w-xl mx-auto w-full flex-1 flex flex-col items-center justify-between space-y-8 min-h-[480px]">
                  {/* Top Action Button */}
                  <div className="w-full flex justify-center pt-2">
                    <button
                      type="button"
                      onClick={() => fileInputRef.current?.click()}
                      className="px-6 py-2.5 bg-[#0277bd] hover:bg-[#01579b] text-white rounded-md text-sm font-bold shadow-xs transition active:scale-98 cursor-pointer flex items-center gap-2"
                    >
                      <span>إختر ملف الإكسل XLS</span>
                    </button>
                  </div>

                  {/* Main Instruction Box */}
                  <div className="w-full bg-white border-2 border-[#0277bd] rounded-lg p-6 text-center space-y-4 shadow-xs">
                    <h3 className="text-sm sm:text-base font-bold text-slate-900">
                      ملف الأصناف لابد أن يحتوي على سبعة أعمدة
                    </h3>

                    <p className="text-xs sm:text-sm text-slate-800 leading-relaxed font-medium px-2">
                      إسم الصنف / الكمية الإفتتاحية / سعر التكلفة / سعر البيع(بالعملة المحلية) /الوحدة الأساسية/الباركود/تاريخ الإنتهاء
                    </p>

                    <div className="pt-3 border-t border-slate-100">
                      <button
                        type="button"
                        onClick={() => handleDownloadExcelTemplate('sample', 'xlsx')}
                        className="text-rose-600 hover:text-rose-700 font-bold text-xs sm:text-sm underline cursor-pointer transition"
                      >
                        * * * يمكنك الضغط هنا لتحميل نموذج الإكسل
                      </button>
                    </div>
                  </div>

                  {/* Secondary Demo Option */}
                  <div className="text-center pt-1">
                    <button
                      type="button"
                      onClick={handleLoadDemoItems}
                      className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg border border-sky-300 bg-sky-50 hover:bg-sky-100 text-sky-800 text-xs font-bold transition cursor-pointer"
                    >
                      <Sparkles className="w-3.5 h-3.5 text-sky-600" />
                      <span>تجربة عينة أصناف تجريبية فوراً (7 أصناف)</span>
                    </button>
                  </div>

                  {/* Bottom Action Button matching screenshot */}
                  <div className="w-full flex justify-center pb-4">
                    <button
                      type="button"
                      onClick={() => fileInputRef.current?.click()}
                      className="px-10 py-2 bg-[#0277bd] hover:bg-[#01579b] text-white rounded-md text-sm font-bold shadow-xs transition active:scale-98 cursor-pointer"
                    >
                      حفظ
                    </button>
                  </div>
                </div>
              )}

              {/* PHASE 2: FILE LOADED & PREVIEW GRID */}
              {importedItems.length > 0 && (
                <div className="space-y-4">
                  {/* File Metadata Bar */}
                  <div className="bg-white border border-slate-200 rounded-xl p-3 shadow-xs flex flex-wrap items-center justify-between gap-3">
                    <div className="flex items-center gap-2.5">
                      <div className="w-8 h-8 rounded-lg bg-emerald-100 text-emerald-700 flex items-center justify-center">
                        <FileSpreadsheet className="w-4 h-4" />
                      </div>
                      <div>
                        <div className="text-xs font-bold text-slate-800 flex items-center gap-2">
                          <span>الملف النشط: {importFileName}</span>
                          {importFileSize && <span className="text-[11px] font-mono text-slate-500 font-normal">({importFileSize})</span>}
                          {importSheetName && <span className="text-[10px] bg-slate-100 text-slate-600 px-1.5 py-0.5 rounded border border-slate-200 font-sans">ورقة: {importSheetName}</span>}
                        </div>
                        <div className="text-[11px] text-slate-500">
                          تم التعرف بنجاح على {importedItems.length} صنف. يمكنك معاينتها أو تعديل الأسعار والكميات قبل الحفظ النهائي.
                        </div>
                      </div>
                    </div>

                    <div className="flex items-center gap-2">
                      <button
                        type="button"
                        onClick={() => fileInputRef.current?.click()}
                        className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-xs font-medium border border-slate-200 transition cursor-pointer flex items-center gap-1"
                      >
                        <FolderUp className="w-3.5 h-3.5 text-slate-600" />
                        <span>اختيار ملف آخر</span>
                      </button>
                      <button
                        type="button"
                        onClick={() => {
                          setImportedItems([]);
                          setImportFileName('');
                          setImportErrorMsg(null);
                        }}
                        className="px-3 py-1.5 bg-rose-50 hover:bg-rose-100 text-rose-700 rounded-lg text-xs font-medium border border-rose-200 transition cursor-pointer flex items-center gap-1"
                      >
                        <X className="w-3.5 h-3.5 text-rose-600" />
                        <span>إلغاء وتفريغ</span>
                      </button>
                    </div>
                  </div>

                  {/* 4 Stat Cards */}
                  <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
                    <div className="bg-white border border-slate-200 rounded-xl p-3.5 shadow-xs">
                      <div className="text-xs text-slate-500 font-medium">إجمالي الأصناف</div>
                      <div className="text-xl font-bold font-mono text-slate-900 mt-1">{importedItems.length}</div>
                      <div className="text-[11px] text-slate-400 mt-0.5">صف مقروء من الملف</div>
                    </div>

                    <div className="bg-white border border-emerald-200 rounded-xl p-3.5 shadow-xs">
                      <div className="text-xs text-emerald-700 font-medium">أصناف صالحة تماماً</div>
                      <div className="text-xl font-bold font-mono text-emerald-600 mt-1">{validCount}</div>
                      <div className="text-[11px] text-emerald-600 mt-0.5">جاهزة للإدراج المباشر</div>
                    </div>

                    <div className="bg-white border border-amber-200 rounded-xl p-3.5 shadow-xs">
                      <div className="text-xs text-amber-700 font-medium">تنبيهات / مكررات</div>
                      <div className="text-xl font-bold font-mono text-amber-600 mt-1">{warningsCount}</div>
                      <div className="text-[11px] text-amber-600 mt-0.5">باركود مكرر أو موجود مسبقاً</div>
                    </div>

                    <div className="bg-white border border-sky-200 rounded-xl p-3.5 shadow-xs">
                      <div className="text-xs text-sky-700 font-medium">إجمالي قيمة بضاعة أول المدة</div>
                      <div className="text-lg font-bold font-mono text-[#0277bd] mt-1">{totalValuation.toLocaleString()} <span className="text-xs font-sans">ريال</span></div>
                      <div className="text-[11px] text-sky-600 mt-0.5">إجمالي التكلفة بالمخزن</div>
                    </div>
                  </div>

                  {/* Settings & Allocation Bar */}
                  <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-xs grid grid-cols-1 md:grid-cols-3 gap-3">
                    <div>
                      <label className="block text-xs font-bold text-slate-700 mb-1 flex items-center gap-1">
                        <WarehouseIcon className="w-3.5 h-3.5 text-sky-600" />
                        <span>المستودع المودع به رصيد أول المدة:</span>
                      </label>
                      <select
                        value={targetWarehouseId}
                        onChange={(e) => setTargetWarehouseId(e.target.value)}
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-xs font-medium text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500"
                      >
                        {warehouses && warehouses.length > 0 ? (
                          warehouses.map(w => (
                            <option key={w.id} value={w.id}>{w.name} ({w.code})</option>
                          ))
                        ) : (
                          <option value="WH-01">المخزن الرئيسي (1)</option>
                        )}
                      </select>
                    </div>

                    <div>
                      <label className="block text-xs font-bold text-slate-700 mb-1 flex items-center gap-1">
                        <Sliders className="w-3.5 h-3.5 text-sky-600" />
                        <span>سياسة التعامل مع الأصناف المكررة:</span>
                      </label>
                      <select
                        value={duplicatePolicy}
                        onChange={(e) => setDuplicatePolicy(e.target.value as any)}
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-xs font-medium text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500"
                      >
                        <option value="UPDATE">تحديث بيانات وسعر وكمية الصنف الموجود في النظام</option>
                        <option value="SKIP">تخطي الأصناف الموجودة وعدم تكرارها</option>
                        <option value="APPEND_NEW">إضافة كصنف جديد بتوليد رمز فريد</option>
                      </select>
                    </div>

                    <div>
                      <label className="block text-xs font-bold text-slate-700 mb-1 flex items-center gap-1">
                        <Tag className="w-3.5 h-3.5 text-sky-600" />
                        <span>التصنيف الافتراضي (إذا لم يحدد):</span>
                      </label>
                      <input
                        type="text"
                        value={defaultCategory}
                        onChange={(e) => setDefaultCategory(e.target.value)}
                        placeholder="عام / معلبات..."
                        className="w-full bg-slate-50 border border-slate-300 rounded-lg px-3 py-2 text-xs font-medium text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500"
                      />
                    </div>
                  </div>

                  {/* Toolbar & Filter Bar */}
                  <div className="bg-white border border-slate-200 rounded-xl p-3 shadow-xs flex flex-col sm:flex-row items-center justify-between gap-3">
                    <div className="flex items-center gap-2 w-full sm:w-auto">
                      <div className="relative flex-1 sm:w-64">
                        <Search className="w-4 h-4 absolute right-3 top-1/2 -translate-y-1/2 text-slate-400" />
                        <input
                          type="text"
                          value={previewSearch}
                          onChange={(e) => setPreviewSearch(e.target.value)}
                          placeholder="بحث بالاسم أو الباركود في المعاينة..."
                          className="w-full pr-9 pl-3 py-1.5 bg-slate-50 border border-slate-200 rounded-lg text-xs text-slate-800 placeholder-slate-400 focus:outline-hidden focus:ring-2 focus:ring-sky-500"
                        />
                      </div>

                      <div className="flex items-center bg-slate-100 p-0.5 rounded-lg border border-slate-200 text-xs">
                        <button
                          type="button"
                          onClick={() => setPreviewFilter('ALL')}
                          className={`px-2.5 py-1 rounded-md font-medium transition cursor-pointer ${previewFilter === 'ALL' ? 'bg-white text-slate-900 shadow-xs font-bold' : 'text-slate-600 hover:text-slate-900'}`}
                        >
                          الكل ({importedItems.length})
                        </button>
                        <button
                          type="button"
                          onClick={() => setPreviewFilter('VALID')}
                          className={`px-2.5 py-1 rounded-md font-medium transition cursor-pointer ${previewFilter === 'VALID' ? 'bg-white text-emerald-800 shadow-xs font-bold' : 'text-slate-600 hover:text-slate-900'}`}
                        >
                          الصالحة ({validCount})
                        </button>
                        <button
                          type="button"
                          onClick={() => setPreviewFilter('WARNINGS')}
                          className={`px-2.5 py-1 rounded-md font-medium transition cursor-pointer ${previewFilter === 'WARNINGS' ? 'bg-white text-amber-800 shadow-xs font-bold' : 'text-slate-600 hover:text-slate-900'}`}
                        >
                          الملاحظات ({warningsCount})
                        </button>
                      </div>
                    </div>

                    <div className="flex items-center gap-2 self-end sm:self-auto">
                      <button
                        type="button"
                        onClick={handleGenerateBarcodesForEmpty}
                        className="px-2.5 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-xs font-medium border border-slate-200 transition cursor-pointer flex items-center gap-1"
                        title="توليد أرقام باركود عشوائية للأصناف التي لا تحتوي على باركود"
                      >
                        <Sparkles className="w-3.5 h-3.5 text-amber-600" />
                        <span>توليد باركود تلقائي للأصناف الفارغة</span>
                      </button>
                    </div>
                  </div>

                  {/* Interactive Preview Data Table */}
                  <div className="bg-white border border-slate-200 rounded-xl overflow-hidden shadow-xs">
                    <div className="overflow-x-auto max-h-[460px]">
                      <table className="w-full text-right text-xs border-collapse">
                        <thead className="sticky top-0 z-10 bg-slate-100 border-b border-slate-200 text-slate-700 select-none">
                          <tr>
                            <th className="py-2.5 px-3 font-bold text-center w-12">#</th>
                            <th className="py-2.5 px-3 font-bold min-w-[200px]">إسم الصنف</th>
                            <th className="py-2.5 px-3 font-bold min-w-[140px]">الباركود</th>
                            <th className="py-2.5 px-3 font-bold text-center min-w-[80px]">الكمية</th>
                            <th className="py-2.5 px-3 font-bold text-left min-w-[90px]">التكلفة</th>
                            <th className="py-2.5 px-3 font-bold text-left min-w-[90px]">سعر البيع</th>
                            <th className="py-2.5 px-3 font-bold text-center min-w-[80px]">هامش الربح</th>
                            <th className="py-2.5 px-3 font-bold text-center min-w-[70px]">الوحدة</th>
                            <th className="py-2.5 px-3 font-bold min-w-[100px]">الصلاحية</th>
                            <th className="py-2.5 px-3 font-bold min-w-[100px]">التصنيف</th>
                            <th className="py-2.5 px-3 font-bold min-w-[120px]">حالة الصنف</th>
                            <th className="py-2.5 px-3 font-bold text-center w-12">إجراء</th>
                          </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-100 bg-white font-medium">
                          {filteredImportedItems.map((item, index) => {
                            const marginPct = item.costPrice > 0 ? Math.round(((item.salePrice - item.costPrice) / item.costPrice) * 100) : 0;
                            return (
                              <tr key={item.id} className="hover:bg-sky-50/50 transition">
                                <td className="py-2 px-3 text-center text-slate-400 font-mono text-[11px]">
                                  {index + 1}
                                </td>

                                {/* Name (Editable) */}
                                <td className="py-2 px-3">
                                  <input
                                    type="text"
                                    value={item.name}
                                    onChange={(e) => handleUpdateItemField(item.id, 'name', e.target.value)}
                                    className={`w-full px-2 py-1 rounded border text-xs font-bold text-slate-900 ${
                                      item.status === 'ERROR_MISSING_NAME' 
                                        ? 'border-rose-400 bg-rose-50' 
                                        : 'border-transparent hover:border-slate-300 focus:border-sky-500 focus:bg-white'
                                    }`}
                                  />
                                </td>

                                {/* Barcode (Editable) */}
                                <td className="py-2 px-3">
                                  <div className="flex items-center gap-1">
                                    <input
                                      type="text"
                                      value={item.barcode}
                                      onChange={(e) => handleUpdateItemField(item.id, 'barcode', e.target.value)}
                                      className="w-full px-2 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs font-mono text-slate-800 focus:bg-white"
                                    />
                                    {item.isAutoBarcode && (
                                      <span className="text-[10px] bg-amber-50 text-amber-700 px-1 py-0.5 rounded border border-amber-200 shrink-0" title="تم توليد هذا الباركود تلقائياً لعدم وجوده في الملف">
                                        آلي
                                      </span>
                                    )}
                                  </div>
                                </td>

                                {/* Opening Qty (Editable) */}
                                <td className="py-2 px-3 text-center">
                                  <input
                                    type="number"
                                    value={item.openingQty}
                                    onChange={(e) => handleUpdateItemField(item.id, 'openingQty', parseFloat(e.target.value) || 0)}
                                    className="w-20 px-2 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs font-bold font-mono text-center text-slate-900 focus:bg-white"
                                  />
                                </td>

                                {/* Cost Price (Editable) */}
                                <td className="py-2 px-3 text-left">
                                  <input
                                    type="number"
                                    value={item.costPrice}
                                    onChange={(e) => handleUpdateItemField(item.id, 'costPrice', parseFloat(e.target.value) || 0)}
                                    className="w-24 px-2 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs font-bold font-mono text-left text-slate-900 focus:bg-white"
                                  />
                                </td>

                                {/* Sale Price (Editable) */}
                                <td className="py-2 px-3 text-left">
                                  <input
                                    type="number"
                                    value={item.salePrice}
                                    onChange={(e) => handleUpdateItemField(item.id, 'salePrice', parseFloat(e.target.value) || 0)}
                                    className="w-24 px-2 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs font-bold font-mono text-left text-[#0277bd] focus:bg-white"
                                  />
                                </td>

                                {/* Profit Margin */}
                                <td className="py-2 px-3 text-center">
                                  <span className={`inline-block px-1.5 py-0.5 rounded text-[10px] font-bold font-mono ${
                                    marginPct >= 20 ? 'bg-emerald-50 text-emerald-700 border border-emerald-200' :
                                    marginPct > 0 ? 'bg-sky-50 text-sky-700 border border-sky-200' :
                                    'bg-slate-100 text-slate-600'
                                  }`}>
                                    +{marginPct}%
                                  </span>
                                </td>

                                {/* Unit */}
                                <td className="py-2 px-3 text-center">
                                  <input
                                    type="text"
                                    value={item.unit}
                                    onChange={(e) => handleUpdateItemField(item.id, 'unit', e.target.value)}
                                    className="w-16 px-1.5 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs text-center text-slate-700 focus:bg-white"
                                  />
                                </td>

                                {/* Expiry Date */}
                                <td className="py-2 px-3">
                                  <input
                                    type="text"
                                    value={item.expiryDate || ''}
                                    placeholder="YYYY-MM-DD"
                                    onChange={(e) => handleUpdateItemField(item.id, 'expiryDate', e.target.value)}
                                    className="w-24 px-1.5 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs font-mono text-slate-700 focus:bg-white"
                                  />
                                </td>

                                {/* Category */}
                                <td className="py-2 px-3">
                                  <input
                                    type="text"
                                    value={item.category}
                                    onChange={(e) => handleUpdateItemField(item.id, 'category', e.target.value)}
                                    className="w-24 px-1.5 py-1 rounded border border-transparent hover:border-slate-300 focus:border-sky-500 text-xs text-slate-700 focus:bg-white"
                                  />
                                </td>

                                {/* Status Badge */}
                                <td className="py-2 px-3">
                                  {item.status === 'VALID' && (
                                    <span className="inline-flex items-center gap-1 text-[11px] text-emerald-700 bg-emerald-50 px-2 py-0.5 rounded-full border border-emerald-200">
                                      <Check className="w-3 h-3 text-emerald-600" />
                                      <span>جاهز للإضافة</span>
                                    </span>
                                  )}
                                  {item.status === 'WARNING_EXISTING' && (
                                    <span className="inline-flex items-center gap-1 text-[11px] text-amber-700 bg-amber-50 px-2 py-0.5 rounded-full border border-amber-200" title={item.statusMessage}>
                                      <AlertTriangle className="w-3 h-3 text-amber-600" />
                                      <span>موجود بالنظام</span>
                                    </span>
                                  )}
                                  {item.status === 'WARNING_DUPLICATE_BARCODE' && (
                                    <span className="inline-flex items-center gap-1 text-[11px] text-amber-700 bg-amber-50 px-2 py-0.5 rounded-full border border-amber-200" title={item.statusMessage}>
                                      <AlertTriangle className="w-3 h-3 text-amber-600" />
                                      <span>مكرر بالملف</span>
                                    </span>
                                  )}
                                  {item.status === 'ERROR_MISSING_NAME' && (
                                    <span className="inline-flex items-center gap-1 text-[11px] text-rose-700 bg-rose-50 px-2 py-0.5 rounded-full border border-rose-200" title={item.statusMessage}>
                                      <AlertCircle className="w-3 h-3 text-rose-600" />
                                      <span>اسم مفقود</span>
                                    </span>
                                  )}
                                </td>

                                {/* Delete Action */}
                                <td className="py-2 px-3 text-center">
                                  <button
                                    type="button"
                                    onClick={() => handleRemoveItem(item.id)}
                                    className="p-1 text-slate-400 hover:text-rose-600 hover:bg-rose-50 rounded transition cursor-pointer"
                                    title="حذف هذا السطر من قائمة الاستيراد"
                                  >
                                    <Trash2 className="w-3.5 h-3.5" />
                                  </button>
                                </td>
                              </tr>
                            );
                          })}
                        </tbody>
                      </table>
                    </div>
                  </div>

                  {/* Sticky Bottom Action Confirmation Bar */}
                  <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-sm flex flex-col sm:flex-row items-center justify-between gap-3">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded-full bg-sky-50 text-sky-700 flex items-center justify-center font-bold font-mono text-sm">
                        {validCount}
                      </div>
                      <div>
                        <div className="text-xs font-bold text-slate-800">
                          جاهز لاعتماد استيراد {validCount} صنف إلى قاعدة البيانات
                        </div>
                        <div className="text-[11px] text-slate-500">
                          سيتم إنشاء بضاعة أول المدة وطبقات التكلفة FIFO وحسابات المخازن تلقائياً.
                        </div>
                      </div>
                    </div>

                    <div className="flex items-center gap-2.5 w-full sm:w-auto">
                      <button
                        type="button"
                        onClick={() => {
                          setImportedItems([]);
                          setImportFileName('');
                        }}
                        className="flex-1 sm:flex-initial px-4 py-2.5 rounded-lg border border-slate-300 hover:bg-slate-100 text-slate-700 text-xs font-bold transition cursor-pointer"
                      >
                        إلغاء
                      </button>

                      <button
                        type="button"
                        onClick={handleSaveImportedItems}
                        className="flex-1 sm:flex-initial px-6 py-2.5 bg-[#0277bd] hover:bg-[#01579b] text-white rounded-lg text-xs font-bold shadow-md transition active:scale-98 cursor-pointer flex items-center justify-center gap-2"
                      >
                        <Check className="w-4 h-4 stroke-[2.5]" />
                        <span>تأكيد واستيراد ({validCount}) صنف للمخازن</span>
                      </button>
                    </div>
                  </div>
                </div>
              )}
            </div>
          );
        })()}

        {/* ============================================================== */}
        {/* VIEW 3: أسعار البيع (Screenshot 6) */}
        {/* ============================================================== */}
        {subView === 'SALES_PRICES' && (
          <div className="w-full">
            <table className="w-full text-right text-sm border-collapse">
              <thead>
                <tr className="bg-[#0277bd] text-white border-b border-[#01579b]">
                  <th className="py-2.5 px-4 font-bold text-sm">إسم الصنف</th>
                  <th className="py-2.5 px-4 font-bold text-sm">سعر البيع</th>
                  <th className="py-2.5 px-4 font-bold text-sm">التكلفة</th>
                  <th className="py-2.5 px-4 font-bold text-sm text-left">تاريخ السعر</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200 bg-white">
                {filteredProducts.map((p) => (
                  <tr
                    key={p.id}
                    onClick={() => handleOpenPriceModal(p)}
                    className="hover:bg-sky-50/60 cursor-pointer transition"
                  >
                    <td className="py-2.5 px-4 font-bold text-slate-800">
                      {p.name} - {p.unit}
                    </td>
                    <td className="py-2.5 px-4 font-mono font-bold text-sky-700">
                      {p.salePrice >= 1000
                        ? `${(p.salePrice / 700).toFixed(0)}USD`
                        : `${p.salePrice} YER`}
                    </td>
                    <td className="py-2.5 px-4 font-mono text-slate-600">{p.costPrice.toFixed(1)}</td>
                    <td className="py-2.5 px-4 text-left">
                      <div className="flex items-center justify-between">
                        <span className="font-mono text-xs text-slate-500">2026-06-01</span>
                        <button
                          onClick={(e) => {
                            e.stopPropagation();
                            handleOpenPriceModal(p);
                          }}
                          className="w-6 h-6 bg-[#0277bd] text-white rounded-full flex items-center justify-center font-bold text-xs"
                        >
                          +
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 4: وحدات الصنف (Screenshot 9) */}
        {/* ============================================================== */}
        {subView === 'ITEM_UNITS' && (
          <div className="p-4 max-w-4xl mx-auto space-y-4">
            {/* Product Selector */}
            <div className="bg-white p-3 rounded-lg border border-slate-200 shadow-xs flex flex-wrap items-center justify-between gap-3">
              <div className="flex items-center gap-2 flex-1 min-w-[240px]">
                <span className="font-bold text-slate-700 text-xs shrink-0">اختر الصنف:</span>
                <select
                  value={unitSelectedProductId}
                  onChange={(e) => setUnitSelectedProductId(e.target.value)}
                  className="flex-1 border border-slate-300 rounded px-2.5 py-1.5 text-xs bg-slate-50 font-bold text-slate-800"
                >
                  {products.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.name} ({p.unit}) - كود: {p.code || p.id}
                    </option>
                  ))}
                </select>
              </div>

              {unitSaveFeedback && (
                <span className="text-xs font-bold text-emerald-600 animate-pulse">
                  {unitSaveFeedback}
                </span>
              )}
            </div>

            {/* Units Table */}
            <div className="bg-white rounded-lg border border-slate-200 shadow-xs overflow-hidden">
              <div className="bg-[#0277bd] text-white px-4 py-2 font-bold text-xs flex items-center justify-between">
                <span>وحدات القياس المعرفة للصنف الحالي</span>
                <span className="text-[11px] bg-white/20 px-2 py-0.5 rounded">
                  {(unitDefinitions[unitSelectedProductId] || [
                    { name: 'حبة', factor: 1, price: 100, cost: 70, barcode: '629100099999' }
                  ]).length} وحدات
                </span>
              </div>

              <table className="w-full text-right text-xs border-collapse">
                <thead>
                  <tr className="bg-slate-100 text-slate-700 border-b border-slate-200">
                    <th className="py-2 px-3 font-bold">اسم الوحدة</th>
                    <th className="py-2 px-3 font-bold text-center">معامل التحويل (حبة)</th>
                    <th className="py-2 px-3 font-bold text-left">سعر الشراء / التكلفة</th>
                    <th className="py-2 px-3 font-bold text-left">سعر البيع</th>
                    <th className="py-2 px-3 font-bold">الباركود</th>
                    <th className="py-2 px-3 font-bold text-center">إجراء</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {(unitDefinitions[unitSelectedProductId] || [
                    { name: 'حبة', factor: 1, price: 100, cost: 70, barcode: '629100099999' }
                  ]).map((u, idx) => (
                    <tr key={idx} className="hover:bg-sky-50/50">
                      <td className="py-2 px-3 font-bold text-slate-800">{u.name}</td>
                      <td className="py-2 px-3 text-center font-mono font-bold text-sky-800">{u.factor}</td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-slate-700">{u.cost.toLocaleString()}</td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-emerald-700">{u.price.toLocaleString()}</td>
                      <td className="py-2 px-3 font-mono text-slate-500 text-[11px]">{u.barcode || '—'}</td>
                      <td className="py-2 px-3 text-center">
                        {idx > 0 && (
                          <button
                            onClick={() => {
                              const cur = unitDefinitions[unitSelectedProductId] || [];
                              setUnitDefinitions({
                                ...unitDefinitions,
                                [unitSelectedProductId]: cur.filter((_, i) => i !== idx)
                              });
                            }}
                            className="text-rose-500 hover:text-rose-700 p-1"
                          >
                            <X className="w-3.5 h-3.5" />
                          </button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Add New Unit Form */}
            <div className="bg-slate-100/80 p-3 rounded-lg border border-slate-300 space-y-2">
              <h4 className="font-bold text-xs text-slate-800 flex items-center gap-1.5">
                <Plus className="w-3.5 h-3.5 text-[#0277bd]" />
                <span>إضافة وحدة قياس جديدة للصنف</span>
              </h4>

              <div className="grid grid-cols-2 sm:grid-cols-5 gap-2 text-xs">
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">اسم الوحدة</label>
                  <input
                    type="text"
                    value={newUnitName}
                    onChange={(e) => setNewUnitName(e.target.value)}
                    placeholder="مثال: باكت / كرتون"
                    className="w-full border border-slate-300 rounded px-2 py-1 bg-white"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">المعامل (عدد الحبات)</label>
                  <input
                    type="number"
                    value={newUnitFactor}
                    onChange={(e) => setNewUnitFactor(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded px-2 py-1 bg-white font-mono"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">سعر التكلفة</label>
                  <input
                    type="number"
                    value={newUnitCost}
                    onChange={(e) => setNewUnitCost(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded px-2 py-1 bg-white font-mono"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">سعر البيع</label>
                  <input
                    type="number"
                    value={newUnitPrice}
                    onChange={(e) => setNewUnitPrice(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded px-2 py-1 bg-white font-mono"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">باركود الوحدة</label>
                  <input
                    type="text"
                    value={newUnitBarcode}
                    onChange={(e) => setNewUnitBarcode(e.target.value)}
                    placeholder="اختياري"
                    className="w-full border border-slate-300 rounded px-2 py-1 bg-white font-mono"
                  />
                </div>
              </div>

              <div className="flex justify-end pt-1">
                <button
                  type="button"
                  onClick={() => {
                    const cur = unitDefinitions[unitSelectedProductId] || [
                      { name: 'حبة', factor: 1, price: 100, cost: 70, barcode: '629100099999' }
                    ];
                    setUnitDefinitions({
                      ...unitDefinitions,
                      [unitSelectedProductId]: [
                        ...cur,
                        {
                          name: newUnitName,
                          factor: newUnitFactor,
                          price: newUnitPrice,
                          cost: newUnitCost,
                          barcode: newUnitBarcode || `629${Date.now().toString().slice(-8)}`
                        }
                      ]
                    });
                    setUnitSaveFeedback('تمت إضافة وحدة القياس وتحديث التسعير بنجاح!');
                    setTimeout(() => setUnitSaveFeedback(null), 2500);
                  }}
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded font-bold text-xs shadow-xs transition"
                >
                  حفظ الوحدة
                </button>
              </div>
            </div>
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 5: فاتورة عرض سعر (Screenshot 9) */}
        {/* ============================================================== */}
        {subView === 'PRICE_QUOTATION' && (
          <div className="p-4 max-w-4xl mx-auto space-y-4">
            {/* Header Info Card */}
            <div className="bg-white rounded-lg border border-slate-200 p-3 shadow-xs space-y-2.5">
              <div className="flex flex-wrap items-center justify-between gap-2 border-b border-slate-100 pb-2">
                <span className="font-bold text-slate-800 text-sm">عرض سعر تجاري رقم: <span className="text-[#0277bd] font-mono">{quotationNo}</span></span>
                <div className="flex items-center gap-2">
                  <span className="text-xs text-slate-500">التاريخ:</span>
                  <input
                    type="date"
                    value={quotationDate}
                    onChange={(e) => setQuotationDate(e.target.value)}
                    className="border border-slate-300 rounded px-2 py-0.5 text-xs font-mono"
                  />
                  <span className="text-xs text-slate-500 mr-2">صالح حتى:</span>
                  <input
                    type="date"
                    value={quotationValidUntil}
                    onChange={(e) => setQuotationValidUntil(e.target.value)}
                    className="border border-slate-300 rounded px-2 py-0.5 text-xs font-mono"
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 text-xs">
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">اسم العميل / الجهة</label>
                  <input
                    type="text"
                    value={quotationCustomer}
                    onChange={(e) => setQuotationCustomer(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 bg-slate-50 font-bold"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">شروط وملاحظات العرض</label>
                  <input
                    type="text"
                    value={quotationNotes}
                    onChange={(e) => setQuotationNotes(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 bg-slate-50"
                  />
                </div>
              </div>
            </div>

            {/* Quick Add Product to Quotation */}
            <div className="bg-sky-50 border border-sky-200 rounded-lg p-2.5 flex items-center gap-2 text-xs">
              <Search className="w-4 h-4 text-[#0277bd]" />
              <input
                type="text"
                placeholder="ابحث عن صنف لإضافته إلى عرض السعر..."
                value={quotationSearchText}
                onChange={(e) => setQuotationSearchText(e.target.value)}
                className="flex-1 bg-white border border-sky-300 rounded px-2 py-1 text-xs outline-hidden"
              />
              {quotationSearchText && (
                <div className="flex gap-1">
                  {products
                    .filter((p) => p.name.includes(quotationSearchText))
                    .slice(0, 3)
                    .map((p) => (
                      <button
                        key={p.id}
                        onClick={() => {
                          setQuotationItems([
                            ...quotationItems,
                            { product: p, quantity: 1, unitPrice: p.salePrice, total: p.salePrice }
                          ]);
                          setQuotationSearchText('');
                        }}
                        className="bg-[#0277bd] text-white px-2 py-1 rounded text-[11px] font-bold"
                      >
                        + {p.name}
                      </button>
                    ))}
                </div>
              )}
            </div>

            {/* Quotation Items Table */}
            <div className="bg-white rounded-lg border border-slate-200 shadow-xs overflow-hidden">
              <table className="w-full text-right text-xs border-collapse">
                <thead>
                  <tr className="bg-[#0277bd] text-white">
                    <th className="py-2 px-3 font-bold">الصنف</th>
                    <th className="py-2 px-3 font-bold text-center">الكمية</th>
                    <th className="py-2 px-3 font-bold text-left">سعر الوحدة</th>
                    <th className="py-2 px-3 font-bold text-left">الإجمالي</th>
                    <th className="py-2 px-3 text-center">حذف</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {quotationItems.map((item, idx) => (
                    <tr key={idx} className="hover:bg-slate-50">
                      <td className="py-2 px-3 font-bold text-slate-800">{item.product.name}</td>
                      <td className="py-2 px-3 text-center">
                        <input
                          type="number"
                          min="1"
                          value={item.quantity}
                          onChange={(e) => {
                            const q = Math.max(1, Number(e.target.value));
                            setQuotationItems(
                              quotationItems.map((it, i) =>
                                i === idx ? { ...it, quantity: q, total: q * it.unitPrice } : it
                              )
                            );
                          }}
                          className="w-16 border rounded text-center py-0.5 font-mono"
                        />
                      </td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-slate-700">
                        {item.unitPrice.toLocaleString()}
                      </td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-sky-800">
                        {item.total.toLocaleString()}
                      </td>
                      <td className="py-2 px-3 text-center">
                        <button
                          onClick={() => setQuotationItems(quotationItems.filter((_, i) => i !== idx))}
                          className="text-rose-500 hover:text-rose-700"
                        >
                          <X className="w-4 h-4" />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              {/* Quotation Footer */}
              <div className="bg-slate-50 p-3 border-t border-slate-200 flex flex-wrap items-center justify-between gap-3">
                <div className="flex flex-wrap items-center gap-3">
                  <div className="font-bold text-xs text-slate-700">
                    المجموع: <span className="text-slate-900 font-mono font-bold">
                      {quotationSubtotal.toLocaleString()} YER
                    </span>
                  </div>

                  {/* Header-Level Discount Control */}
                  <div className="flex items-center gap-1.5 bg-white px-2 py-1 rounded border border-slate-300 text-xs">
                    <span className="font-bold text-slate-600 text-[11px]">خصم العرض:</span>
                    <button
                      type="button"
                      onClick={() => setQuotationDiscountType((prev) => (prev === 'AMOUNT' ? 'PERCENT' : 'AMOUNT'))}
                      className="bg-sky-50 text-[#0277bd] hover:bg-sky-100 font-bold px-1.5 py-0.5 rounded text-[11px] border border-sky-200 transition"
                      title="تبديل نوع الخصم بين مبلغ ونسبة"
                    >
                      {quotationDiscountType === 'AMOUNT' ? 'مبلغ' : 'نسبة %'}
                    </button>
                    <input
                      type="number"
                      min={0}
                      value={quotationDiscountVal}
                      onChange={(e) => setQuotationDiscountVal(e.target.value)}
                      className="w-20 bg-slate-50 border border-slate-300 rounded px-1 py-0.5 text-center font-mono font-bold text-xs"
                    />
                    {quotationDiscountAmount > 0 && (
                      <span className="font-mono text-rose-600 font-bold text-xs">
                        (-{quotationDiscountAmount.toLocaleString()})
                      </span>
                    )}
                  </div>

                  <div className="font-black text-xs text-slate-800 bg-white px-2.5 py-1 rounded border border-emerald-300">
                    الصافي: <span className="text-emerald-700 font-mono font-black text-sm">{quotationGrandTotal.toLocaleString()} YER</span>
                  </div>
                </div>

                <div className="flex items-center gap-2">
                  <button
                    onClick={() => {
                      setQuotationFeedback(`تم حفظ عرض السعر بنجاح برقم: ${quotationNo} (صافي: ${quotationGrandTotal.toLocaleString()} YER بعد خصم ${quotationDiscountAmount.toLocaleString()} YER)`);
                      setTimeout(() => setQuotationFeedback(null), 3500);
                    }}
                    className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded font-bold text-xs shadow-xs"
                  >
                    حفظ عرض السعر
                  </button>
                  <button
                    onClick={() => {
                      setQuotationFeedback(`تم إنشاء وتصدير الفاتورة من عرض السعر بنجاح بصافي ${quotationGrandTotal.toLocaleString()} YER!`);
                      setTimeout(() => setQuotationFeedback(null), 3500);
                    }}
                    className="bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-1.5 rounded font-bold text-xs shadow-xs"
                  >
                    تحويل إلى فاتورة مبيعات
                  </button>
                </div>
              </div>
            </div>

            {quotationFeedback && (
              <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 p-2.5 rounded text-xs font-bold text-center">
                {quotationFeedback}
              </div>
            )}
          </div>
        )}

        {/* ============================================================== */}
        {/* VIEW 6: طلب شراء (Screenshot 9) */}
        {/* ============================================================== */}
        {subView === 'PURCHASE_ORDER' && (
          <div className="p-4 max-w-4xl mx-auto space-y-4">
            {/* Header Info Card */}
            <div className="bg-white rounded-lg border border-slate-200 p-3 shadow-xs space-y-2.5">
              <div className="flex flex-wrap items-center justify-between gap-2 border-b border-slate-100 pb-2">
                <span className="font-bold text-slate-800 text-sm">طلب شراء بضاعة رقم: <span className="text-[#0277bd] font-mono">{orderNo}</span></span>
                <div className="flex items-center gap-2">
                  <span className="text-xs text-slate-500">تاريخ الطلب:</span>
                  <input
                    type="date"
                    value={orderDate}
                    onChange={(e) => setOrderDate(e.target.value)}
                    className="border border-slate-300 rounded px-2 py-0.5 text-xs font-mono"
                  />
                  <span className="text-xs text-slate-500 mr-2">التوريد المتوقع:</span>
                  <input
                    type="date"
                    value={orderExpectedDate}
                    onChange={(e) => setOrderExpectedDate(e.target.value)}
                    className="border border-slate-300 rounded px-2 py-0.5 text-xs font-mono"
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 text-xs">
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">اسم المورد</label>
                  <input
                    type="text"
                    value={orderSupplier}
                    onChange={(e) => setOrderSupplier(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 bg-slate-50 font-bold"
                  />
                </div>
                <div>
                  <label className="block text-[11px] font-bold text-slate-600 mb-0.5">ملاحظات التوريد والمستودع</label>
                  <input
                    type="text"
                    value={orderNotes}
                    onChange={(e) => setOrderNotes(e.target.value)}
                    className="w-full border border-slate-300 rounded px-2.5 py-1.5 bg-slate-50"
                  />
                </div>
              </div>
            </div>

            {/* Quick Add Product to Order */}
            <div className="bg-sky-50 border border-sky-200 rounded-lg p-2.5 flex items-center gap-2 text-xs">
              <Search className="w-4 h-4 text-[#0277bd]" />
              <input
                type="text"
                placeholder="ابحث عن صنف لإضافته إلى أمر الشراء..."
                value={orderSearchText}
                onChange={(e) => setOrderSearchText(e.target.value)}
                className="flex-1 bg-white border border-sky-300 rounded px-2 py-1 text-xs outline-hidden"
              />
              {orderSearchText && (
                <div className="flex gap-1">
                  {products
                    .filter((p) => p.name.includes(orderSearchText))
                    .slice(0, 3)
                    .map((p) => (
                      <button
                        key={p.id}
                        onClick={() => {
                          setOrderItems([
                            ...orderItems,
                            { product: p, quantity: 10, unitCost: p.costPrice, total: 10 * p.costPrice }
                          ]);
                          setOrderSearchText('');
                        }}
                        className="bg-[#0277bd] text-white px-2 py-1 rounded text-[11px] font-bold"
                      >
                        + {p.name}
                      </button>
                    ))}
                </div>
              )}
            </div>

            {/* Order Items Table */}
            <div className="bg-white rounded-lg border border-slate-200 shadow-xs overflow-hidden">
              <table className="w-full text-right text-xs border-collapse">
                <thead>
                  <tr className="bg-[#0277bd] text-white">
                    <th className="py-2 px-3 font-bold">الصنف المطلوب</th>
                    <th className="py-2 px-3 font-bold text-center">الكمية المطلوبة</th>
                    <th className="py-2 px-3 font-bold text-left">التكلفة التقديرية</th>
                    <th className="py-2 px-3 font-bold text-left">الإجمالي</th>
                    <th className="py-2 px-3 text-center">حذف</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {orderItems.map((item, idx) => (
                    <tr key={idx} className="hover:bg-slate-50">
                      <td className="py-2 px-3 font-bold text-slate-800">{item.product.name}</td>
                      <td className="py-2 px-3 text-center">
                        <input
                          type="number"
                          min="1"
                          value={item.quantity}
                          onChange={(e) => {
                            const q = Math.max(1, Number(e.target.value));
                            setOrderItems(
                              orderItems.map((it, i) =>
                                i === idx ? { ...it, quantity: q, total: q * it.unitCost } : it
                              )
                            );
                          }}
                          className="w-16 border rounded text-center py-0.5 font-mono"
                        />
                      </td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-slate-700">
                        {item.unitCost.toLocaleString()}
                      </td>
                      <td className="py-2 px-3 text-left font-mono font-bold text-sky-800">
                        {item.total.toLocaleString()}
                      </td>
                      <td className="py-2 px-3 text-center">
                        <button
                          onClick={() => setOrderItems(orderItems.filter((_, i) => i !== idx))}
                          className="text-rose-500 hover:text-rose-700"
                        >
                          <X className="w-4 h-4" />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              {/* Order Footer */}
              <div className="bg-slate-50 p-3 border-t border-slate-200 flex flex-wrap items-center justify-between gap-3">
                <div className="flex flex-wrap items-center gap-3">
                  <div className="font-bold text-xs text-slate-700">
                    المجموع التقديري: <span className="text-slate-900 font-mono font-bold">
                      {orderSubtotal.toLocaleString()} YER
                    </span>
                  </div>

                  {/* Header-Level Discount Control */}
                  <div className="flex items-center gap-1.5 bg-white px-2 py-1 rounded border border-slate-300 text-xs">
                    <span className="font-bold text-slate-600 text-[11px]">خصم الطلب:</span>
                    <button
                      type="button"
                      onClick={() => setOrderDiscountType((prev) => (prev === 'AMOUNT' ? 'PERCENT' : 'AMOUNT'))}
                      className="bg-sky-50 text-[#0277bd] hover:bg-sky-100 font-bold px-1.5 py-0.5 rounded text-[11px] border border-sky-200 transition"
                      title="تبديل نوع الخصم بين مبلغ ونسبة"
                    >
                      {orderDiscountType === 'AMOUNT' ? 'مبلغ' : 'نسبة %'}
                    </button>
                    <input
                      type="number"
                      min={0}
                      value={orderDiscountVal}
                      onChange={(e) => setOrderDiscountVal(e.target.value)}
                      className="w-20 bg-slate-50 border border-slate-300 rounded px-1 py-0.5 text-center font-mono font-bold text-xs"
                    />
                    {orderDiscountAmount > 0 && (
                      <span className="font-mono text-rose-600 font-bold text-xs">
                        (-{orderDiscountAmount.toLocaleString()})
                      </span>
                    )}
                  </div>

                  <div className="font-black text-xs text-slate-800 bg-white px-2.5 py-1 rounded border border-emerald-300">
                    الصافي التقديري: <span className="text-emerald-700 font-mono font-black text-sm">{orderGrandTotal.toLocaleString()} YER</span>
                  </div>
                </div>

                <div className="flex items-center gap-2">
                  <button
                    onClick={() => {
                      setOrderFeedback(`تم حفظ أمر الشراء بنجاح برقم: ${orderNo} (صافي تقديري: ${orderGrandTotal.toLocaleString()} YER بعد خصم ${orderDiscountAmount.toLocaleString()} YER)`);
                      setTimeout(() => setOrderFeedback(null), 3500);
                    }}
                    className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded font-bold text-xs shadow-xs"
                  >
                    حفظ أمر الشراء
                  </button>
                  <button
                    onClick={() => {
                      setOrderFeedback(`تم تحويل أمر الشراء إلى فاتورة مشتريات تجارية بنجاح بصافي ${orderGrandTotal.toLocaleString()} YER!`);
                      setTimeout(() => setOrderFeedback(null), 3500);
                    }}
                    className="bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-1.5 rounded font-bold text-xs shadow-xs"
                  >
                    تحويل إلى فاتورة مشتريات
                  </button>
                </div>
              </div>
            </div>

            {orderFeedback && (
              <div className="bg-emerald-50 border border-emerald-200 text-emerald-800 p-2.5 rounded text-xs font-bold text-center">
                {orderFeedback}
              </div>
            )}
          </div>
        )}
      </main>

      {/* 3. Bottom Blue Bar (Matches Screenshot 5) */}
      {subView === 'PRODUCTS_LIST' && (
        <footer className="bg-[#0277bd] h-14 flex items-center justify-between px-6 relative shadow-lg shrink-0">
          {/* Left: Green Circular Excel Button (Opens Screenshot 4) */}
          <button
            onClick={() => setSubView('EXCEL_IMPORT')}
            className="w-10 h-10 bg-emerald-600 hover:bg-emerald-700 text-white rounded-full flex items-center justify-center shadow-md transition active:scale-95 cursor-pointer border-2 border-white"
            title="إستيراد من إكسل XLS"
          >
            <div className="font-bold text-xs tracking-tighter">XLS</div>
          </button>

          {/* Center: Blue (+) Button in White Circle */}
          <div className="absolute left-1/2 -translate-x-1/2">
            <button
              onClick={() => setShowAddProductModal(true)}
              className="w-11 h-11 bg-white hover:bg-sky-50 text-[#0277bd] rounded-full flex items-center justify-center shadow-lg transition active:scale-95 cursor-pointer border-2 border-white"
              title="إضافة صنف جديد"
            >
              <Plus className="w-6 h-6 stroke-[3]" />
            </button>
          </div>

          {/* Right: Quick Switch to Sales Prices */}
          <button
            onClick={() => setSubView('SALES_PRICES')}
            className="text-xs font-bold text-white bg-white/10 hover:bg-white/20 px-3 py-1.5 rounded-lg transition"
          >
            أسعار البيع
          </button>
        </footer>
      )}

      {/* ============================================================== */}
      {/* POPUP MODAL: تعديل سعر الصنف مع لوحة المفاتيح الرقمية (Screenshot 6) */}
      {/* ============================================================== */}
      {selectedProductForPrice && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60">
          <div className="bg-white rounded-xl max-w-sm w-full shadow-2xl border border-slate-200 overflow-hidden text-right flex flex-col">
            {/* Modal Header */}
            <div className="bg-[#0277bd] text-white p-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <ChevronDown className="w-4 h-4 text-white/80" />
                <h3 className="font-bold text-base">{selectedProductForPrice.name}</h3>
              </div>
              <button
                onClick={() => setSelectedProductForPrice(null)}
                className="text-white hover:bg-white/20 p-1 rounded-full"
              >
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Modal Body */}
            <div className="p-4 space-y-3">
              {/* Unit Switcher: كرتون / حبة */}
              <div className="flex gap-2">
                <button
                  type="button"
                  onClick={() => setSelectedUnit('كرتون')}
                  className={`flex-1 py-1.5 rounded-md font-bold text-xs border transition cursor-pointer ${
                    selectedUnit === 'كرتون'
                      ? 'bg-[#0277bd] text-white border-[#0277bd]'
                      : 'bg-slate-50 text-slate-700 border-slate-300'
                  }`}
                >
                  كرتون
                </button>
                <button
                  type="button"
                  onClick={() => setSelectedUnit('حبة')}
                  className={`flex-1 py-1.5 rounded-md font-bold text-xs border transition cursor-pointer ${
                    selectedUnit === 'حبة'
                      ? 'bg-[#0277bd] text-white border-[#0277bd]'
                      : 'bg-slate-50 text-slate-700 border-slate-300'
                  }`}
                >
                  حبة
                </button>
              </div>

              {/* Price Input & Cost Display */}
              <div>
                <label className="block text-xs font-bold text-slate-700 mb-1">سعر البيع</label>
                <input
                  type="text"
                  readOnly
                  value={salePriceInput}
                  className="w-full text-center text-xl font-bold font-mono py-2 bg-slate-50 border-2 border-sky-400 rounded-lg outline-hidden"
                />
                <p className="text-xs text-slate-500 text-center mt-1">
                  سعر التكلفة: YR {selectedProductForPrice.costPrice.toLocaleString()}
                </p>
              </div>

              {/* Currency Radios: محلي / دولار / سعودي */}
              <div className="flex items-center justify-center gap-6 pt-1 text-xs font-bold text-slate-700">
                <label className="flex items-center gap-1.5 cursor-pointer">
                  <input
                    type="radio"
                    name="priceCurrency"
                    checked={currencyChoice === 'LOCAL'}
                    onChange={() => setCurrencyChoice('LOCAL')}
                  />
                  <span>محلي</span>
                </label>
                <label className="flex items-center gap-1.5 cursor-pointer">
                  <input
                    type="radio"
                    name="priceCurrency"
                    checked={currencyChoice === 'USD'}
                    onChange={() => setCurrencyChoice('USD')}
                  />
                  <span>دولار</span>
                </label>
                <label className="flex items-center gap-1.5 cursor-pointer">
                  <input
                    type="radio"
                    name="priceCurrency"
                    checked={currencyChoice === 'SAR'}
                    onChange={() => setCurrencyChoice('SAR')}
                  />
                  <span>سعودي</span>
                </label>
              </div>

              {/* Action Buttons: موافق / إلغاء */}
              <div className="flex gap-2 pt-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={handleSavePriceModal}
                  className="flex-1 bg-[#0277bd] hover:bg-[#01579b] text-white py-2 rounded-lg font-bold text-sm transition cursor-pointer"
                >
                  موافق
                </button>
                <button
                  type="button"
                  onClick={() => setSelectedProductForPrice(null)}
                  className="flex-1 bg-slate-100 hover:bg-slate-200 text-slate-700 py-2 rounded-lg font-bold text-sm transition cursor-pointer"
                >
                  إلغاء
                </button>
              </div>

              {priceFeedback && (
                <p className="text-xs font-bold text-emerald-600 text-center">{priceFeedback}</p>
              )}

              {/* On-Screen Touch NumPad (Exact match to Screenshot 6 layout) */}
              <div className="grid grid-cols-4 gap-1 pt-2 border-t border-slate-200">
                {['1', '2', '3'].map((k) => (
                  <button
                    key={k}
                    type="button"
                    onClick={() => handleKeypadPress(k)}
                    className="bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-lg py-2.5 rounded-sm transition active:scale-95"
                  >
                    {k}
                  </button>
                ))}
                {/* Backspace on column 4 */}
                <button
                  type="button"
                  onClick={() => handleKeypadPress('BACKSPACE')}
                  className="bg-slate-200 hover:bg-slate-300 text-slate-700 font-bold py-2.5 rounded-sm flex items-center justify-center transition active:scale-95"
                >
                  <Delete className="w-5 h-5" />
                </button>

                {['4', '5', '6'].map((k) => (
                  <button
                    key={k}
                    type="button"
                    onClick={() => handleKeypadPress(k)}
                    className="bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-lg py-2.5 rounded-sm transition active:scale-95"
                  >
                    {k}
                  </button>
                ))}
                {/* Done Button on column 4 */}
                <button
                  type="button"
                  onClick={() => handleKeypadPress('DONE')}
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white font-bold text-xs py-2.5 rounded-sm flex items-center justify-center transition active:scale-95"
                >
                  تم
                </button>

                {['7', '8', '9'].map((k) => (
                  <button
                    key={k}
                    type="button"
                    onClick={() => handleKeypadPress(k)}
                    className="bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-lg py-2.5 rounded-sm transition active:scale-95"
                  >
                    {k}
                  </button>
                ))}
                {/* Empty spacer */}
                <div />

                {/* Bottom row: '.', '0' */}
                <button
                  type="button"
                  onClick={() => handleKeypadPress('.')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-lg py-2.5 rounded-sm transition active:scale-95"
                >
                  .
                </button>
                <button
                  type="button"
                  onClick={() => handleKeypadPress('0')}
                  className="bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-lg py-2.5 rounded-sm transition active:scale-95"
                >
                  0
                </button>
                <div />
                <div />
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Modal: Add New Product */}
      {showAddProductModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60">
          <div className="bg-white rounded-xl max-w-md w-full p-4 shadow-2xl border border-slate-200 text-right space-y-3">
            <div className="flex items-center justify-between border-b pb-2">
              <h3 className="font-bold text-sm text-slate-800 flex items-center gap-2">
                <Plus className="w-4 h-4 text-[#0277bd]" />
                <span>إضافة صنف جديد</span>
              </h3>
              <button
                onClick={() => setShowAddProductModal(false)}
                className="text-slate-400 hover:text-slate-600"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveNewProduct} className="space-y-3 text-xs">
              <div>
                <label className="block font-bold text-slate-700 mb-1">اسم الصنف:</label>
                <input
                  type="text"
                  required
                  placeholder="مثال: بطاطس عمان، شبس..."
                  value={newProdName}
                  onChange={(e) => setNewProdName(e.target.value)}
                  className="w-full border border-slate-300 rounded-lg p-2 focus:ring-2 focus:ring-[#0277bd] outline-hidden"
                />
              </div>

              <div className="grid grid-cols-2 gap-2">
                <div>
                  <label className="block font-bold text-slate-700 mb-1">مجموعة الصنف:</label>
                  <input
                    type="text"
                    value={newProdCategory}
                    onChange={(e) => setNewProdCategory(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5"
                  />
                </div>
                <div>
                  <label className="block font-bold text-slate-700 mb-1">الوحدة الأساسية:</label>
                  <select
                    value={newProdUnit}
                    onChange={(e) => setNewProdUnit(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 bg-white"
                  >
                    <option value="حبة">حبة</option>
                    <option value="باكت">باكت</option>
                    <option value="كرتون">كرتون</option>
                    <option value="كيس">كيس</option>
                  </select>
                </div>
              </div>

              <div className="grid grid-cols-3 gap-2">
                <div>
                  <label className="block font-bold text-slate-700 mb-1">الكمية الافتتاحية:</label>
                  <input
                    type="number"
                    value={newProdOpeningQty}
                    onChange={(e) => setNewProdOpeningQty(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded-lg p-1.5 font-mono"
                  />
                </div>
                <div>
                  <label className="block font-bold text-slate-700 mb-1">تكلفة الوحدة:</label>
                  <input
                    type="number"
                    value={newProdCost}
                    onChange={(e) => setNewProdCost(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded-lg p-1.5 font-mono"
                  />
                </div>
                <div>
                  <label className="block font-bold text-slate-700 mb-1">سعر البيع:</label>
                  <input
                    type="number"
                    value={newProdPrice}
                    onChange={(e) => setNewProdPrice(Number(e.target.value))}
                    className="w-full border border-slate-300 rounded-lg p-1.5 font-mono"
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-2">
                <div>
                  <div className="flex items-center justify-between mb-1">
                    <label className="font-bold text-slate-700">الباركود:</label>
                    <button
                      type="button"
                      onClick={() => setNewProdBarcode(generateUniqueBarcode())}
                      className="text-[10px] text-sky-700 hover:text-sky-900 font-bold bg-sky-50 hover:bg-sky-100 px-1.5 py-0.5 rounded border border-sky-200 transition cursor-pointer"
                      title="توليد باركود فريد ومفحوص تلقائياً"
                    >
                      توليد فريد ⚡
                    </button>
                  </div>
                  <input
                    type="text"
                    placeholder="اختياري (يمكن تركه فارغاً)"
                    value={newProdBarcode}
                    onChange={(e) => setNewProdBarcode(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 font-mono"
                  />
                </div>
                <div>
                  <label className="block font-bold text-slate-700 mb-1">تاريخ الانتهاء:</label>
                  <input
                    type="date"
                    value={newProdExpiry}
                    onChange={(e) => setNewProdExpiry(e.target.value)}
                    className="w-full border border-slate-300 rounded-lg p-1.5 font-mono"
                  />
                </div>
              </div>

              <div className="flex justify-end gap-2 pt-2 border-t">
                <button
                  type="button"
                  onClick={() => setShowAddProductModal(false)}
                  className="px-3 py-1.5 rounded-lg text-slate-600 hover:bg-slate-100 font-bold"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="bg-[#0277bd] hover:bg-[#01579b] text-white px-4 py-1.5 rounded-lg font-bold shadow-xs cursor-pointer"
                >
                  حفظ الصنف
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};
