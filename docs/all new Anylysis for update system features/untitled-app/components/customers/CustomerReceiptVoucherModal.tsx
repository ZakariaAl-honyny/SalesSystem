'use client';

import React, { useState, useMemo } from 'react';
import { 
  Customer, 
  Invoice, 
  AccountNode, 
  CostCenterNode, 
  ShiftInfo, 
  SystemUser, 
  CashVoucher, 
  VoucherInvoiceAllocation 
} from '@/types/pos';
import { generateNextId, getCurrentDateString, getCurrentTimeString } from '@/lib/idGenerator';
import { tafqeet } from '@/lib/tafqeet';
import { ArrowDownLeft, X, FileSpreadsheet, CheckCircle2 } from 'lucide-react';

export interface CustomerReceiptVoucherModalProps {
  customer: Customer;
  customers: Customer[];
  salesInvoices: Invoice[];
  chartOfAccounts: AccountNode[];
  costCenters: CostCenterNode[];
  shift: ShiftInfo;
  currentUser: SystemUser | null;
  onClose: () => void;
  onSave: (voucher: CashVoucher, allocations?: VoucherInvoiceAllocation[]) => void;
}

export const CustomerReceiptVoucherModal: React.FC<CustomerReceiptVoucherModalProps> = ({
  customer,
  customers,
  salesInvoices,
  costCenters: _costCenters,
  shift: _shift,
  currentUser,
  onClose,
  onSave,
}) => {
  const [selectedCustId, setSelectedCustId] = useState(customer.id);
  const currentCust = useMemo(() => customers.find((c) => c.id === selectedCustId) || customer, [customers, selectedCustId, customer]);

  const [voucherNo] = useState(() => `${Math.floor(1001 + Math.random() * 900)}`);
  const [voucherDate, setVoucherDate] = useState(getCurrentDateString());
  const [paymentMethod, setPaymentMethod] = useState<'CASH' | 'BANK' | 'CHEQUE'>('CASH');
  const [amount, setAmount] = useState<number>(() => Math.min(50000, currentCust.currentBalance > 0 ? currentCust.currentBalance : 10000));

  // Cash / Bank Account Selector
  const [cashAccountCode, setCashAccountCode] = useState<'1111' | '1112' | '1121' | '1122' | '1123'>('1111');
  const [costCenterId] = useState<string>('CC-201');

  // Cheque / Transfer fields
  const [reference, setReference] = useState('');
  const [checkNumber, setCheckNumber] = useState('');
  const [bankName, setBankName] = useState('');
  const [dueDate, setDueDate] = useState(getCurrentDateString());
  const [description, setDescription] = useState(`سداد دفعة من الحساب الآجل للعميل ${currentCust.name}`);

  // Unpaid Invoices for this customer
  const customerUnpaidInvoices = useMemo(() => {
    return salesInvoices
      .filter((inv) => inv.customerOrSupplierId === currentCust.id && inv.type === 'SALE' && inv.remainingAmount > 0)
      .sort((a, b) => a.date.localeCompare(b.date)); // FIFO order
  }, [salesInvoices, currentCust]);

  // Invoice allocations state: { [invId]: allocatedAmount }
  const [allocations, setAllocations] = useState<{ [invoiceId: string]: number }>({});

  // Auto Allocate FIFO
  const handleAutoAllocateFIFO = () => {
    let remainingToAllocate = amount;
    const newAlloc: { [id: string]: number } = {};

    customerUnpaidInvoices.forEach((inv) => {
      if (remainingToAllocate <= 0) {
        newAlloc[inv.id] = 0;
      } else {
        const canPay = Math.min(remainingToAllocate, inv.remainingAmount);
        newAlloc[inv.id] = canPay;
        remainingToAllocate -= canPay;
      }
    });

    setAllocations(newAlloc);
  };

  const totalAllocated = useMemo(() => {
    return Object.values(allocations).reduce((sum, val) => sum + (val || 0), 0);
  }, [allocations]);

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    if (amount <= 0) {
      alert('يرجى إدخال مبلغ صحيح لسند القبض');
      return;
    }

    const cashAccNames: { [k: string]: string } = {
      '1111': 'الصندوق الرئيسي (الخزينة المركزية)',
      '1112': 'صندوق نقطة بيع POS كاشير 1',
      '1121': 'حساب بنك الكريمي للتمويل الأصغر (YER)',
      '1122': 'محفظة جيب الإلكترونية (JEB Wallet)',
      '1123': 'محفظة كاش وبنك اليمن والكويت',
    };

    const allocationList: VoucherInvoiceAllocation[] = Object.entries(allocations)
      .filter(([_, allocAmt]) => allocAmt > 0)
      .map(([invId, allocAmt]) => {
        const inv = customerUnpaidInvoices.find((i) => i.id === invId);
        return {
          invoiceId: invId,
          invoiceNo: inv?.invoiceNo || invId,
          invoiceDate: inv?.date,
          invoiceTotal: inv?.grandTotal,
          allocatedAmount: allocAmt,
        };
      });

    const newVoucher: CashVoucher = {
      id: generateNextId('VOUCH'),
      voucherNo,
      type: 'RECEIPT',
      paymentMethod,
      date: voucherDate,
      amount,
      currency: 'ريال يمني',
      cashAccountCode,
      cashAccountName: cashAccNames[cashAccountCode] || 'الصندوق الرئيسي',
      partyType: 'CUSTOMER',
      partyId: currentCust.id,
      partyName: currentCust.name,
      contraAccountCode: currentCust.accountCode,
      contraAccountName: `حساب العميل: ${currentCust.name}`,
      costCenterId,
      costCenterCode: '201',
      costCenterName: 'فرع حدة الرئيسي',
      reference: reference || checkNumber || undefined,
      description: description || `سند قبض من ${currentCust.name}`,
      status: 'POSTED',
      createdBy: currentUser?.userName || 'cashier1',
      createdAt: `${voucherDate} ${getCurrentTimeString()}`,
      allocatedInvoices: allocationList.length > 0 ? allocationList : undefined,
      checkNumber: checkNumber || undefined,
      bankName: bankName || undefined,
      dueDate: dueDate || undefined,
    };

    onSave(newVoucher, allocationList);
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/65 backdrop-blur-xs flex items-center justify-center p-3 animate-fade-in">
      <div className="bg-white rounded-lg shadow-2xl border border-slate-300 w-full max-w-2xl overflow-hidden flex flex-col max-h-[90vh]">
        {/* Header */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-emerald-50 border border-emerald-200 flex items-center justify-center text-emerald-700 shadow-xs">
              <ArrowDownLeft className="w-4 h-4 text-emerald-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">سند قبض نقدي / بنكي (Customer Receipt Voucher)</h3>
              <p className="text-[10px] text-slate-500 mt-0.5">تحصيل مبالغ مالية وتوزيعها على الفواتير الآجلة وتوليد القيد المزدوج آلياً</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm font-bold cursor-pointer transition"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSave} className="p-4 flex flex-col gap-3 text-xs overflow-y-auto">
          {/* Top Info Bar */}
          <div className="grid grid-cols-3 gap-3 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
            <div>
              <label className="font-bold text-slate-700 block mb-1">رقم السند الآلي:</label>
              <input
                type="text"
                readOnly
                value={voucherNo}
                className="w-full bg-slate-200 border border-slate-300 rounded px-2 py-1 font-mono font-bold text-sky-950"
              />
            </div>

            <div>
              <label className="font-bold text-slate-700 block mb-1">تاريخ القبض:</label>
              <input
                type="date"
                required
                value={voucherDate}
                onChange={(e) => setVoucherDate(e.target.value)}
                className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-mono text-slate-900"
              />
            </div>

            <div>
              <label className="font-bold text-slate-700 block mb-1">طريقة القبض:</label>
              <select
                value={paymentMethod}
                onChange={(e) => setPaymentMethod(e.target.value as any)}
                className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-bold text-slate-900"
              >
                <option value="CASH">نقداً (الصندوق والخزينة)</option>
                <option value="BANK">تحويل بنكي / محفظة إلكترونية</option>
                <option value="CHEQUE">شيك بنكي مؤجل / حال</option>
              </select>
            </div>
          </div>

          {/* Customer Selection & Balance Glance */}
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-bold text-slate-700 block mb-1">العميل المقبوض منه *:</label>
              <select
                value={selectedCustId}
                onChange={(e) => {
                  setSelectedCustId(e.target.value);
                  setDescription(`سداد دفعة من الحساب الآجل للعميل`);
                }}
                className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
              >
                {customers.map((c) => (
                  <option key={c.id} value={c.id}>
                    [{c.code}] {c.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="bg-slate-50 p-2 rounded border border-slate-200 flex items-center justify-between">
              <div>
                <span className="text-[10px] text-slate-500 block">رصيد العميل الحالي (مدين):</span>
                <span className="font-mono-numbers font-bold text-rose-700 text-sm">
                  {currentCust.currentBalance.toLocaleString()} ر.ي
                </span>
              </div>
              <div>
                <span className="text-[10px] text-slate-500 block">سقف الائتمان:</span>
                <span className="font-mono-numbers font-semibold text-slate-700">
                  {currentCust.creditLimit.toLocaleString()} ر.ي
                </span>
              </div>
            </div>
          </div>

          {/* Amount and Tafqeet */}
          <div className="bg-emerald-50/60 border border-emerald-200 p-3 rounded-lg flex flex-col gap-2">
            <div className="grid grid-cols-2 gap-3">
              <div>
                <label className="font-bold text-emerald-950 block mb-1">المبلغ المقبوض (بالريال اليمني) *:</label>
                <input
                  type="number"
                  min={1}
                  required
                  value={amount}
                  onChange={(e) => setAmount(Number(e.target.value))}
                  className="w-full bg-white border border-emerald-400 rounded px-3 py-1.5 font-mono-numbers font-bold text-emerald-900 text-base focus:outline-hidden"
                />
              </div>

              <div>
                <label className="font-bold text-slate-700 block mb-1">حساب الإيداع / الخزينة (المدين):</label>
                <select
                  value={cashAccountCode}
                  onChange={(e) => setCashAccountCode(e.target.value as any)}
                  className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 font-bold text-slate-900"
                >
                  <option value="1111">[1111] الصندوق الرئيسي (الخزينة المركزية)</option>
                  <option value="1112">[1112] صندوق كاشير 1</option>
                  <option value="1121">[1121] بنك الكريمي للتمويل الأصغر</option>
                  <option value="1122">[1122] محفظة جيب الإلكترونية (JEB)</option>
                  <option value="1123">[1123] محفظة كاش وبنك اليمن والكويت</option>
                </select>
              </div>
            </div>

            {/* Arabic Tafqeet in Realtime */}
            <div className="text-[11px] text-emerald-900 font-semibold bg-emerald-100/70 p-2 rounded border border-emerald-300">
              <span>المبلغ كتابةً: </span>
              <span className="font-bold">{tafqeet(amount)}</span>
            </div>
          </div>

          {/* Payment Method Extras */}
          {paymentMethod === 'BANK' && (
            <div className="grid grid-cols-2 gap-3 bg-sky-50 p-2.5 rounded border border-sky-200">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">اسم البنك / المحفظة:</label>
                <input
                  type="text"
                  value={bankName}
                  onChange={(e) => setBankName(e.target.value)}
                  placeholder="مثلاً: بنك الكريمي، محفظة ون كاش"
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-800"
                />
              </div>
              <div>
                <label className="font-semibold text-slate-700 block mb-1">رقم الحوالة / الإشعار البنكي:</label>
                <input
                  type="text"
                  value={reference}
                  onChange={(e) => setReference(e.target.value)}
                  placeholder="TR-..."
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-mono text-slate-800"
                />
              </div>
            </div>
          )}

          {paymentMethod === 'CHEQUE' && (
            <div className="grid grid-cols-3 gap-3 bg-amber-50 p-2.5 rounded border border-amber-200">
              <div>
                <label className="font-semibold text-slate-700 block mb-1">رقم الشيك:</label>
                <input
                  type="text"
                  value={checkNumber}
                  onChange={(e) => setCheckNumber(e.target.value)}
                  placeholder="440912"
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 font-mono text-slate-800"
                />
              </div>
              <div>
                <label className="font-semibold text-slate-700 block mb-1">البنك المسحوب عليه:</label>
                <input
                  type="text"
                  value={bankName}
                  onChange={(e) => setBankName(e.target.value)}
                  placeholder="بنك التضامن"
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-800"
                />
              </div>
              <div>
                <label className="font-semibold text-slate-700 block mb-1">تاريخ استحقاق الشيك:</label>
                <input
                  type="date"
                  value={dueDate}
                  onChange={(e) => setDueDate(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-2 py-1 text-slate-800"
                />
              </div>
            </div>
          )}

          {/* Invoice Allocation Section */}
          <div className="border border-slate-300 rounded-lg overflow-hidden flex flex-col">
            <div className="bg-slate-100 px-3 py-2 border-b border-slate-300 flex items-center justify-between">
              <div className="flex items-center gap-1.5 font-bold text-slate-800">
                <FileSpreadsheet className="w-3.5 h-3.5 text-sky-600" />
                <span>توزيع السداد على الفواتير الآجلة المفتوحة للعميل ({customerUnpaidInvoices.length} فاتورة):</span>
              </div>
              <button
                type="button"
                onClick={handleAutoAllocateFIFO}
                className="bg-sky-700 hover:bg-sky-600 text-white px-2 py-0.5 rounded text-[11px] font-bold cursor-pointer"
              >
                توزيع تلقائي (FIFO من الأقدم)
              </button>
            </div>

            <div className="max-h-36 overflow-y-auto">
              <table className="w-full text-right border-collapse text-[11px]">
                <thead>
                  <tr className="bg-slate-200 text-slate-700 font-bold border-b border-slate-300">
                    <th className="p-1.5">رقم الفاتورة</th>
                    <th className="p-1.5 w-20">التاريخ</th>
                    <th className="p-1.5 w-24 text-left">إجمالي الفاتورة</th>
                    <th className="p-1.5 w-24 text-left">المتبقي الحالي</th>
                    <th className="p-1.5 w-32 text-left">المبلغ المسدد بهذا السند</th>
                  </tr>
                </thead>
                <tbody>
                  {customerUnpaidInvoices.length > 0 ? (
                    customerUnpaidInvoices.map((inv) => (
                      <tr key={inv.id} className="border-b border-slate-200 bg-white">
                        <td className="p-1.5 font-mono text-sky-900 font-bold">{inv.invoiceNo}</td>
                        <td className="p-1.5 font-mono text-slate-600">{inv.date}</td>
                        <td className="p-1.5 text-left font-mono-numbers">{inv.grandTotal.toLocaleString()}</td>
                        <td className="p-1.5 text-left font-mono-numbers font-bold text-rose-700">
                          {inv.remainingAmount.toLocaleString()}
                        </td>
                        <td className="p-1.5 text-left">
                          <input
                            type="number"
                            min={0}
                            max={inv.remainingAmount}
                            value={allocations[inv.id] || 0}
                            onChange={(e) => {
                              const val = Math.max(0, Math.min(inv.remainingAmount, Number(e.target.value)));
                              setAllocations((prev) => ({ ...prev, [inv.id]: val }));
                            }}
                            className="w-24 bg-slate-50 border border-slate-300 rounded px-2 py-0.5 text-left font-mono-numbers font-bold text-slate-900"
                          />
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={5} className="p-3 text-center text-slate-400">
                        لا توجد فواتير آجلة مفتوحة لهذا العميل. سيتم تقييد المبلغ كدفعة مقدمة / تسوية حساب.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>

            {customerUnpaidInvoices.length > 0 && (
              <div className="bg-slate-50 p-2 border-t border-slate-200 flex items-center justify-between text-[11px]">
                <span className="text-slate-600">
                  الموزع على الفواتير: <strong className="text-slate-900">{totalAllocated.toLocaleString()} ر.ي</strong> من إجمالي السند: <strong className="text-emerald-700">{amount.toLocaleString()} ر.ي</strong>
                </span>
                {amount > totalAllocated && (
                  <span className="text-amber-700 font-semibold">
                    متبقي غير موزع: {(amount - totalAllocated).toLocaleString()} ر.ي (يقيد مباشرة في رصيد العميل)
                  </span>
                )}
              </div>
            )}
          </div>

          <div>
            <label className="font-semibold text-slate-700 block mb-1">البيان والشرح في السند والقيد:</label>
            <input
              type="text"
              required
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full bg-white border border-slate-300 rounded px-2.5 py-1.5 text-slate-900 font-medium"
            />
          </div>

          {/* Double-Entry Preview Strip */}
          <div className="bg-slate-100 p-2 rounded border border-slate-300 text-[10px] text-slate-600 flex items-center justify-between">
            <span className="font-mono">
              القيد الآلي المتولد: من حـ/ [{cashAccountCode}] الخزينة/البنك ({amount.toLocaleString()} ر.ي) ⬅️ إلى حـ/ [{currentCust.accountCode}] العملاء والمدينين - {currentCust.name} ({amount.toLocaleString()} ر.ي)
            </span>
            <span className="text-emerald-700 font-bold bg-emerald-100 px-1.5 py-0.5 rounded">متوازن ومرحل</span>
          </div>

          {/* Footer Buttons */}
          <div className="pt-2 border-t border-slate-200 flex items-center justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="bg-slate-200 hover:bg-slate-300 text-slate-800 font-bold px-3 py-1.5 rounded cursor-pointer"
            >
              إلغاء
            </button>
            <button
              type="submit"
              className="bg-emerald-600 hover:bg-emerald-500 text-white font-bold px-4 py-1.5 rounded flex items-center gap-1.5 shadow-xs cursor-pointer"
            >
              <CheckCircle2 className="w-4 h-4" />
              <span>حفظ وترحيل السند والقيد [F10]</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
