'use client';

import React, { useState, useMemo } from 'react';
import { 
  Table, 
  Plus, 
  Trash2, 
  Save, 
  RefreshCw, 
  Keyboard, 
  CheckCircle2, 
  ArrowRight,
  Calculator,
  Info,
  Layers,
  Sparkles,
  Edit3,
  Copy,
  ArrowUp,
  ArrowDown,
  Tag
} from 'lucide-react';
import { ProductItem, Customer, MainTabType } from '@/types/pos';
import { WinFormsDGVContextMenu, DGVContextMenuItem } from '@/components/WinFormsDGVContextMenu';

interface Props {
  products: ProductItem[];
  customers: Customer[];
  onSelectTab?: (tab: MainTabType) => void;
  onSaveSuccess?: (invoiceNo: string) => void;
}

export const FrmSalesInvoice_InlineGrid: React.FC<Props> = ({
  products,
  customers,
  onSelectTab,
  onSaveSuccess,
}) => {
  // Test Invoice Header State
  const [invoiceNo, setInvoiceNo] = useState('1004');
  const [customerName, setCustomerName] = useState(customers[0]?.name || 'محلات الأمل التجارية');
  const [paymentType, setPaymentType] = useState<'CASH' | 'CREDIT'>('CASH');
  const [notes, setNotes] = useState('اختبار الإدخال بالجدول التفاعلي المباشر (Inline Grid Cell Editing)');

  const activeCustomer = useMemo(() => {
    return customers.find(c => c.name === customerName || c.id === customerName) || customers[0];
  }, [customers, customerName]);

  // Mock Invoice Line Items for Strategy 1
  const [items, setItems] = useState<Array<{
    id: string;
    product: ProductItem;
    quantity: number;
    unit: string;
    unitPrice: number;
    discount: number;
    total: number;
    notes: string;
  }>>([
    {
      id: 'S1-1',
      product: products[0] || { id: 'P1', name: 'بطاطس عمان 25غ', code: '1001', salePrice: 0.0714, costPrice: 0.05, unit: 'حبة', category: 'أغذية' },
      quantity: 12,
      unit: 'حبة',
      unitPrice: 0.0714,
      discount: 0,
      total: 0.8568,
      notes: 'تعديل مباشر بالجدول',
    },
    {
      id: 'S1-2',
      product: products[1] || { id: 'P2', name: 'عصير برتقال 1 لتر', code: '1002', salePrice: 1.20, costPrice: 0.90, unit: 'حبة', category: 'مشروبات' },
      quantity: 5,
      unit: 'باكت',
      unitPrice: 1.20,
      discount: 0.50,
      total: 5.50,
      notes: 'خصم خاص بالسطر',
    },
  ]);

  const [alert, setAlert] = useState<{ type: 'success' | 'info' | 'warning'; text: string } | null>({
    type: 'info',
    text: '💡 وضع الاختبار 1 [Inline Grid]: انقر بزر الماوس الأيمن على أي سطر في الجدول لإظهار قائمة الإجراءات السياقية (ContextMenuStrip).',
  });

  // DGV Context Menu State
  const [contextMenu, setContextMenu] = useState<{
    visible: boolean;
    x: number;
    y: number;
    index: number;
    title: string;
  }>({ visible: false, x: 0, y: 0, index: -1, title: '' });

  const handleRowContextMenu = (e: React.MouseEvent, item: any, index: number) => {
    e.preventDefault();
    setContextMenu({
      visible: true,
      x: e.clientX,
      y: e.clientY,
      index,
      title: item.product.name,
    });
  };

  const dgvMenuItems: DGVContextMenuItem[] = [
    {
      id: 'edit-qty',
      label: 'تعديل كمية السطر',
      icon: <Edit3 className="w-3.5 h-3.5 text-sky-600" />,
      shortcut: 'Enter',
      action: () => {
        const currentQty = items[contextMenu.index]?.quantity || 1;
        const newQty = prompt(`تعديل كمية [${contextMenu.title}]:`, String(currentQty));
        if (newQty !== null && !isNaN(parseFloat(newQty))) {
          handleCellChange(contextMenu.index, 'quantity', parseFloat(newQty));
        }
      },
    },
    {
      id: 'add-discount',
      label: 'إضافة خصم مباشر للسطر',
      icon: <Tag className="w-3.5 h-3.5 text-amber-600" />,
      action: () => {
        const currentDisc = items[contextMenu.index]?.discount || 0;
        const newDisc = prompt(`إدخال خصم السطر [${contextMenu.title}]:`, String(currentDisc));
        if (newDisc !== null && !isNaN(parseFloat(newDisc))) {
          handleCellChange(contextMenu.index, 'discount', parseFloat(newDisc));
        }
      },
    },
    {
      id: 'copy-info',
      label: 'نسخ اسم وكود الصنف',
      icon: <Copy className="w-3.5 h-3.5 text-slate-600" />,
      shortcut: 'Ctrl+C',
      action: () => {
        const it = items[contextMenu.index];
        if (it) {
          navigator.clipboard?.writeText(`${it.product.code} - ${it.product.name}`);
          setAlert({ type: 'success', text: `تم نسخ بيانات الصنف [${it.product.name}] بالحافظة.` });
        }
      },
    },
    {
      id: 'move-up',
      label: 'تحريك السطر للأعلى',
      icon: <ArrowUp className="w-3.5 h-3.5 text-slate-600" />,
      disabled: contextMenu.index === 0,
      action: () => {
        if (contextMenu.index > 0) {
          setItems(prev => {
            const arr = [...prev];
            const temp = arr[contextMenu.index];
            arr[contextMenu.index] = arr[contextMenu.index - 1];
            arr[contextMenu.index - 1] = temp;
            return arr;
          });
        }
      },
    },
    {
      id: 'move-down',
      label: 'تحريك السطر للأسفل',
      icon: <ArrowDown className="w-3.5 h-3.5 text-slate-600" />,
      disabled: contextMenu.index === items.length - 1,
      action: () => {
        if (contextMenu.index < items.length - 1) {
          setItems(prev => {
            const arr = [...prev];
            const temp = arr[contextMenu.index];
            arr[contextMenu.index] = arr[contextMenu.index + 1];
            arr[contextMenu.index + 1] = temp;
            return arr;
          });
        }
      },
    },
    {
      id: 'add-row',
      label: 'إدراج سطر جديد أسفل',
      icon: <Plus className="w-3.5 h-3.5 text-emerald-600" />,
      action: handleAddBlankRow,
    },
    {
      id: 'delete-row',
      label: 'حذف السطر المختار',
      icon: <Trash2 className="w-3.5 h-3.5 text-rose-600" />,
      shortcut: 'Delete',
      danger: true,
      action: () => {
        handleDeleteRow(contextMenu.index);
      },
    },
  ];

  // Calculate totals
  const subtotal = items.reduce((acc, it) => acc + (it.quantity * it.unitPrice), 0);
  const totalDiscount = items.reduce((acc, it) => acc + it.discount, 0);
  const netTotal = items.reduce((acc, it) => acc + it.total, 0);

  // Cell editing handlers
  const handleCellChange = (
    index: number, 
    field: 'quantity' | 'unitPrice' | 'discount' | 'unit' | 'notes', 
    value: any
  ) => {
    setItems(prev => {
      const updated = [...prev];
      const item = { ...updated[index] };

      if (field === 'quantity') {
        item.quantity = Math.max(0.001, parseFloat(value) || 0);
      } else if (field === 'unitPrice') {
        item.unitPrice = Math.max(0, parseFloat(value) || 0);
      } else if (field === 'discount') {
        item.discount = Math.max(0, parseFloat(value) || 0);
      } else if (field === 'unit') {
        item.unit = value;
      } else if (field === 'notes') {
        item.notes = value;
      }

      item.total = Math.max(0, (item.quantity * item.unitPrice) - item.discount);
      updated[index] = item;
      return updated;
    });
  };

  const handleAddBlankRow = () => {
    const defaultProd = products[0] || { id: 'P1', name: 'صنف افتراضي', salePrice: 10, unit: 'حبة' };
    setItems(prev => [
      ...prev,
      {
        id: `S1-${Date.now()}`,
        product: defaultProd,
        quantity: 1,
        unit: defaultProd.unit || 'حبة',
        unitPrice: defaultProd.salePrice || 10,
        discount: 0,
        total: defaultProd.salePrice || 10,
        notes: '',
      }
    ]);
    setAlert({ type: 'success', text: 'تمت إضافة سطر جديد تلقائياً في الجدول — ركز على الخلية للتعديل.' });
  };

  const handleDeleteRow = (index: number) => {
    setItems(prev => prev.filter((_, i) => i !== index));
    setAlert({ type: 'warning', text: 'تم حذف السطر المختار من الجدول.' });
  };

  return (
    <div className="flex flex-col h-full bg-slate-100 text-slate-900 font-sans select-none overflow-y-auto">
      {/* Standalone WinForms Top Bar Header */}
      <div className="bg-white border-b border-slate-200 p-3 flex flex-wrap items-center justify-between gap-3 shadow-xs text-slate-800">
        <div className="flex items-center gap-3">
          {onSelectTab && (
            <button
              onClick={() => onSelectTab('SALES_STRATEGIES_HUB')}
              className="px-2.5 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold rounded flex items-center gap-1.5 border border-slate-300 transition cursor-pointer"
              title="العودة لمختبر استراتيجيات الفواتير"
            >
              <ArrowRight className="w-4 h-4 text-amber-600" />
              <span>المختبر الرئيسي</span>
            </button>
          )}

          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-emerald-600 flex items-center justify-center text-white font-bold text-sm shadow-xs">
              1
            </div>
            <div>
              <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <span>استراتيجية 1: الجدول التفاعلي المباشر [Inline Grid Cell Editing]</span>
                <span className="text-[10px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded-full font-mono font-bold">
                  frmSalesInvoice_InlineGrid
                </span>
              </h1>
              <p className="text-[11px] text-slate-500">
                اختبار سرعة المحاسب في إدخال وتعديل أسطر الفاتورة مباشرة في خلايا الجدول بزر Tab والأسهم دون مغادرة الجدول.
              </p>
            </div>
          </div>
        </div>

        {/* Action Controls */}
        <div className="flex items-center gap-2">
          <button
            onClick={handleAddBlankRow}
            className="px-3 py-1.5 bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold rounded-lg shadow-xs flex items-center gap-1.5 transition cursor-pointer"
          >
            <Plus className="w-4 h-4" />
            <span>إضافة سطر جديد (F2)</span>
          </button>
          <button
            onClick={() => {
              setItems([]);
              setAlert({ type: 'warning', text: 'تم تفريغ الجدول للبدء باختبار جديد.' });
            }}
            className="px-3 py-1.5 bg-slate-800 hover:bg-slate-700 text-slate-200 text-xs font-bold rounded-lg border border-slate-700 flex items-center gap-1.5 transition cursor-pointer"
          >
            <RefreshCw className="w-3.5 h-3.5" />
            <span>إعادة تعيين</span>
          </button>
        </div>
      </div>

      {/* Strategy UX Banner */}
      <div className="bg-emerald-50 border-b border-emerald-200 p-2.5 px-4 flex items-center justify-between gap-2 text-xs">
        <div className="flex items-center gap-2 text-emerald-900">
          <Sparkles className="w-4 h-4 text-emerald-600 shrink-0" />
          <span>
            <b>مميزات الاستراتيجية 1:</b> إمكانية التعديل السريع الفوري داخل خلايا الجدول (Inline Grid)، مفضلة لدى المحاسبين متمرسي كيبورد الأرقام (Numpad).
          </span>
        </div>
        <div className="flex items-center gap-3 text-[11px] text-slate-600">
          <span className="bg-white border border-slate-300 px-2 py-0.5 rounded text-amber-800 font-mono font-bold">Tab: الانتقال بين الخلايا</span>
          <span className="bg-white border border-slate-300 px-2 py-0.5 rounded text-sky-800 font-mono font-bold">Enter: تأكيد السطر</span>
        </div>
      </div>

      {/* Main Form Content */}
      <div className="p-4 flex-1 flex flex-col gap-4">
        {/* Alert feedback */}
        {alert && (
          <div className={`p-2.5 rounded-lg border text-xs flex items-center justify-between ${
            alert.type === 'success' ? 'bg-emerald-50 border-emerald-300 text-emerald-900 font-medium' :
            alert.type === 'warning' ? 'bg-amber-50 border-amber-300 text-amber-900 font-medium' :
            'bg-sky-50 border-sky-300 text-sky-900 font-medium'
          }`}>
            <span className="flex items-center gap-2">
              <Info className="w-4 h-4 shrink-0" />
              <span>{alert.text}</span>
            </span>
            <button onClick={() => setAlert(null)} className="text-slate-400 hover:text-slate-700 font-bold">✕</button>
          </div>
        )}

        {/* Invoice Top Fields Header */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-3 bg-white p-3.5 rounded-xl border border-slate-200 text-xs shadow-xs">
          <div>
            <label className="block text-slate-700 mb-1 font-bold">رقم الفاتورة الرقمي:</label>
            <input 
              type="text" 
              value={invoiceNo}
              onChange={e => setInvoiceNo(e.target.value)}
              className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-emerald-700 font-mono font-extrabold focus:bg-white focus:border-emerald-500"
            />
          </div>
          <div>
            <div className="flex items-center justify-between mb-1">
              <label className="block text-slate-700 font-bold">اسم العميل:</label>
              {activeCustomer && (
                <span className="text-[10px] text-slate-500 font-mono">#{activeCustomer.code}</span>
              )}
            </div>
            <select
              value={customerName}
              onChange={e => setCustomerName(e.target.value)}
              className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-900 font-bold focus:bg-white focus:border-indigo-500"
            >
              {customers.map(c => (
                <option key={c.id} value={c.name}>{c.name}</option>
              ))}
            </select>
            {activeCustomer && (
              <div className="mt-1 text-[11px]">
                {activeCustomer.currentBalance > 0 ? (
                  <div className="bg-amber-50 text-amber-900 border border-amber-300 rounded px-1.5 py-0.5 font-bold flex items-center justify-between">
                    <span>عليه للمحل (مطلوب سداد):</span>
                    <span className="font-mono text-amber-800">{activeCustomer.currentBalance.toLocaleString()} {activeCustomer.currency || 'ريال'}</span>
                  </div>
                ) : activeCustomer.currentBalance < 0 ? (
                  <div className="bg-emerald-50 text-emerald-900 border border-emerald-300 rounded px-1.5 py-0.5 font-bold flex items-center justify-between">
                    <span>له دفعة مقدمة (فائض له):</span>
                    <span className="font-mono text-emerald-800">{Math.abs(activeCustomer.currentBalance).toLocaleString()} {activeCustomer.currency || 'ريال'}</span>
                  </div>
                ) : (
                  <div className="bg-slate-100 text-slate-700 border border-slate-200 rounded px-1.5 py-0.5 font-bold text-center">
                    <span>حساب خالص (لا توجد مديونية سابقة)</span>
                  </div>
                )}
              </div>
            )}
          </div>
          <div>
            <label className="block text-slate-700 mb-1 font-bold">نوع الدفع:</label>
            <select 
              value={paymentType}
              onChange={e => setPaymentType(e.target.value as any)}
              className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-amber-800 font-bold focus:bg-white focus:border-amber-500"
            >
              <option value="CASH">نقدي (Cash)</option>
              <option value="CREDIT">آجل (Credit Account)</option>
            </select>
          </div>
          <div>
            <label className="block text-slate-700 mb-1 font-bold">ملاحظات الفاتورة:</label>
            <input 
              type="text" 
              value={notes}
              onChange={e => setNotes(e.target.value)}
              className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-slate-700 focus:bg-white focus:border-indigo-500"
            />
          </div>
        </div>

        {/* Inline Grid Table (The Core Strategy View) */}
        <div className="flex-1 bg-white border border-slate-200 rounded-xl overflow-hidden flex flex-col min-h-[300px] shadow-xs">
          <div className="bg-slate-50 px-3 py-2 border-b border-slate-200 flex items-center justify-between">
            <span className="text-xs font-bold text-slate-800 flex items-center gap-2">
              <Table className="w-4 h-4 text-emerald-600" />
              <span>جدول الفاتورة التفاعلي [Inline Editable DataGrid] ({items.length} أصناف)</span>
            </span>
            <span className="text-[11px] text-emerald-800 font-bold bg-emerald-50 border border-emerald-300 px-2 py-0.5 rounded-full">
              ⚡ الخلايا قابلة للتعديل المباشر بالنقرة والأسهم
            </span>
          </div>

          <div className="overflow-x-auto flex-1">
            <table className="w-full text-right text-xs border-collapse">
              <thead>
                <tr className="bg-slate-100/80 text-slate-700 border-b border-slate-200 font-bold">
                  <th className="p-2.5 text-center w-12">#</th>
                  <th className="p-2.5">اسم الصنف والتصنيف</th>
                  <th className="p-2.5 w-24 text-center">الوحدة</th>
                  <th className="p-2.5 w-28 text-center bg-emerald-100/60 text-emerald-900 border-x border-emerald-200">الكمية ✎</th>
                  <th className="p-2.5 w-32 text-center bg-sky-100/60 text-sky-900 border-x border-sky-200">سعر الوحدة ✎</th>
                  <th className="p-2.5 w-28 text-center bg-amber-100/60 text-amber-900 border-x border-amber-200">الخصم ✎</th>
                  <th className="p-2.5 w-32 text-center">الإجمالي (Net)</th>
                  <th className="p-2.5">ملاحظات السطر</th>
                  <th className="p-2.5 w-16 text-center">إجراء</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={9} className="p-8 text-center text-slate-500">
                      لا توجد أصناف في الفاتورة. انقر فوق &quot;إضافة سطر جديد&quot; للاختبار.
                    </td>
                  </tr>
                ) : (
                  items.map((item, idx) => (
                    <tr 
                      key={item.id} 
                      onContextMenu={(e) => handleRowContextMenu(e, item, idx)}
                      className="hover:bg-sky-50/60 transition-colors cursor-context-menu"
                    >
                      <td className="p-2 text-center text-slate-500 font-mono font-medium">{idx + 1}</td>
                      <td className="p-2">
                        <div className="font-bold text-slate-900">{item.product.name}</div>
                        <div className="text-[10px] text-slate-500 font-mono">كود: {item.product.code} | {item.product.category || 'عام'}</div>
                      </td>
                      <td className="p-2 text-center">
                        <select
                          value={item.unit}
                          onChange={e => handleCellChange(idx, 'unit', e.target.value)}
                          className="bg-white border border-slate-300 text-slate-800 text-xs rounded px-1.5 py-1 w-full text-center font-medium"
                        >
                          <option value={item.product.unit || 'حبة'}>{item.product.unit || 'حبة'}</option>
                          <option value="باكت">باكت</option>
                          <option value="كرتون">كرتون</option>
                        </select>
                      </td>
                      {/* Editable Quantity Cell */}
                      <td className="p-1.5 text-center bg-emerald-50/50 border-x border-emerald-100">
                        <input
                          type="number"
                          step="any"
                          value={item.quantity}
                          onChange={e => handleCellChange(idx, 'quantity', e.target.value)}
                          className="w-full bg-white border border-emerald-400 rounded px-2 py-1 text-center font-bold text-emerald-800 focus:outline-none focus:ring-2 focus:ring-emerald-500"
                        />
                      </td>
                      {/* Editable Price Cell */}
                      <td className="p-1.5 text-center bg-sky-50/50 border-x border-sky-100">
                        <input
                          type="number"
                          step="any"
                          value={item.unitPrice}
                          onChange={e => handleCellChange(idx, 'unitPrice', e.target.value)}
                          className="w-full bg-white border border-sky-400 rounded px-2 py-1 text-center font-bold text-sky-800 focus:outline-none focus:ring-2 focus:ring-sky-500 font-mono"
                        />
                      </td>
                      {/* Editable Discount Cell */}
                      <td className="p-1.5 text-center bg-amber-50/50 border-x border-amber-100">
                        <input
                          type="number"
                          step="any"
                          value={item.discount}
                          onChange={e => handleCellChange(idx, 'discount', e.target.value)}
                          className="w-full bg-white border border-amber-400 rounded px-2 py-1 text-center font-bold text-amber-800 focus:outline-none focus:ring-2 focus:ring-amber-500 font-mono"
                        />
                      </td>
                      {/* Total */}
                      <td className="p-2 text-center font-mono font-extrabold text-emerald-700">
                        {item.total.toFixed(4)}
                      </td>
                      {/* Editable Notes */}
                      <td className="p-1.5">
                        <input
                          type="text"
                          value={item.notes}
                          onChange={e => handleCellChange(idx, 'notes', e.target.value)}
                          placeholder="ملاحظات السطر..."
                          className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-700 focus:outline-none focus:border-slate-500"
                        />
                      </td>
                      <td className="p-2 text-center">
                        <button
                          onClick={() => handleDeleteRow(idx)}
                          className="p-1 text-rose-600 hover:text-rose-800 hover:bg-rose-50 rounded transition cursor-pointer"
                          title="حذف السطر"
                        >
                          <Trash2 className="w-4 h-4" />
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

        {/* Invoice Summary & Test Footer */}
        <div className="bg-white border border-slate-200 rounded-xl p-3.5 flex flex-wrap items-center justify-between gap-4 shadow-xs">
          <div className="flex items-center gap-6 text-xs">
            <div>
              <span className="text-slate-500 font-bold">عدد الأسطر:</span>
              <span className="mr-1.5 font-bold text-slate-900">{items.length} أصناف</span>
            </div>
            <div>
              <span className="text-slate-500 font-bold">الإجمالي المبدئي:</span>
              <span className="mr-1.5 font-bold font-mono text-slate-900">{subtotal.toFixed(4)}</span>
            </div>
            <div>
              <span className="text-slate-500 font-bold">إجمالي الخصم:</span>
              <span className="mr-1.5 font-bold font-mono text-amber-700">{totalDiscount.toFixed(4)}</span>
            </div>
            <div className="bg-emerald-50 px-3 py-1.5 rounded-lg border border-emerald-300">
              <span className="text-emerald-900 font-bold">الصافي النهائي:</span>
              <span className="mr-2 font-mono font-extrabold text-emerald-800 text-sm">{netTotal.toFixed(4)}</span>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <button
              onClick={() => {
                if (onSaveSuccess) onSaveSuccess(invoiceNo);
                setAlert({ type: 'success', text: `تم اعتماد وحفظ الفاتورة رقم [${invoiceNo}] بنجاح في تجربة الإدخال المباشر.` });
              }}
              className="px-4 py-2 bg-emerald-600 hover:bg-emerald-500 text-white font-bold text-xs rounded-lg shadow-xs flex items-center gap-2 transition cursor-pointer"
            >
              <CheckCircle2 className="w-4 h-4" />
              <span>اعتماد وتجربة الحفظ</span>
            </button>
          </div>
        </div>
      </div>

      {/* DGV Right-Click ContextMenuStrip */}
      <WinFormsDGVContextMenu
        x={contextMenu.x}
        y={contextMenu.y}
        visible={contextMenu.visible}
        rowIndex={contextMenu.index}
        rowTitle={contextMenu.title}
        items={dgvMenuItems}
        onClose={() => setContextMenu(prev => ({ ...prev, visible: false }))}
      />
    </div>
  );
};
