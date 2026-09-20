'use client';

import React from 'react';
import { Invoice } from '@/types/pos';
import { Printer, QrCode, FileSpreadsheet, Download } from 'lucide-react';
import * as XLSX from 'xlsx';

interface ReceiptModalProps {
  invoice: Invoice | null;
  onClose: () => void;
  currency: string;
}

export const ReceiptModal: React.FC<ReceiptModalProps> = ({
  invoice,
  onClose,
  currency,
}) => {
  if (!invoice) return null;

  const handlePrint = () => {
    window.print();
  };

  const handleExportXLSX = () => {
    const headers = ['الصنف', 'الوحدة', 'الكمية', 'سعر الوحدة', 'إجمالي السطر'];
    const rows = invoice.items.map(item => [
      item.item.name,
      item.selectedUnit,
      item.quantity,
      item.unitPrice,
      item.total
    ]);

    const summaryRows = [
      [],
      ['رقم الفاتورة:', invoice.invoiceNo],
      ['التاريخ والوقت:', `${invoice.date} ${invoice.time}`],
      ['العميل:', invoice.customerOrSupplierName],
      ['المجموع الفرعي:', `${invoice.subtotal} ${currency}`],
      ['الخصم الممنوح:', `${invoice.discountTotal} ${currency}`],
      ['المبلغ الصافي المستحق:', `${invoice.grandTotal} ${currency}`],
      ['طريقة الدفع:', invoice.paymentType]
    ];

    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.aoa_to_sheet([headers, ...rows, ...summaryRows]);
    ws['!cols'] = [
      { wch: 30 },
      { wch: 12 },
      { wch: 10 },
      { wch: 14 },
      { wch: 16 }
    ];
    ws['!views'] = [{ RTL: true }];
    (ws as any)['!sheetViews'] = [{ RTL: true }];

    XLSX.utils.book_append_sheet(wb, ws, `فاتورة_${invoice.invoiceNo}`);
    const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `فاتورة_${invoice.invoiceNo}_${invoice.date}.xlsx`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
  };

  return (
    <div className="fixed inset-0 bg-slate-950/70 backdrop-blur-xs flex items-center justify-center z-50 p-4 select-none">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-sm overflow-hidden flex flex-col max-h-[90vh]">
        
        {/* Modal Header */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-sky-50 border border-sky-200 flex items-center justify-center text-sky-700 shadow-xs">
              <Printer className="w-4 h-4 text-sky-700" />
            </div>
            <h3 className="font-bold text-sm text-slate-900">معاينة الفاتورة الحرارية (80mm)</h3>
          </div>
          <button onClick={onClose} className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg font-bold cursor-pointer transition">
            ✕
          </button>
        </div>

        {/* Receipt Container for Screen & Print */}
        <div className="flex-1 overflow-y-auto p-4 bg-slate-100 flex justify-center">
          <div id="printable-receipt" className="bg-white p-4 w-full max-w-[320px] shadow-sm border border-slate-200 text-slate-900 text-xs font-sans">
            
            {/* Header */}
            <div className="text-center border-b border-dashed border-slate-400 pb-3 mb-2">
              <h2 className="text-base font-black tracking-tight">سوبرماركت والمركز التجاري اليمني</h2>
              <p className="text-[11px] text-slate-600">صنعاء - شارع حدة / عدن - المعلا</p>
              <p className="text-[10px] text-slate-500 font-mono">هاتف: 770000000 / 01-200300</p>
              <div className="mt-1 font-bold bg-slate-100 py-0.5 rounded text-[11px]">
                {invoice.type === 'SALE' ? 'فاتورة مبيعات نقدية / ضريبية مبسطة' : 'فاتورة مشتريات'}
              </div>
            </div>

            {/* Meta Info */}
            <div className="text-[11px] flex flex-col gap-1 border-b border-dashed border-slate-400 pb-2 mb-2">
              <div className="flex justify-between">
                <span>رقم الفاتورة:</span>
                <strong className="font-mono">{invoice.invoiceNo}</strong>
              </div>
              <div className="flex justify-between">
                <span>التاريخ والوقت:</span>
                <span className="font-mono-numbers">{invoice.date} {invoice.time}</span>
              </div>
              <div className="flex justify-between">
                <span>العميل:</span>
                <strong>{invoice.customerOrSupplierName}</strong>
              </div>
              <div className="flex justify-between">
                <span>طريقة الدفع:</span>
                <span className="font-semibold">{invoice.paymentType}</span>
              </div>
              <div className="flex justify-between">
                <span>الكاشير:</span>
                <span>{invoice.cashierName}</span>
              </div>
            </div>

            {/* Items Table */}
            <table className="w-full text-right border-collapse mb-2 text-[11px]">
              <thead>
                <tr className="border-b border-slate-400 text-slate-700">
                  <th className="py-1">الصنف</th>
                  <th className="py-1 text-center">الكمية</th>
                  <th className="py-1 text-left">السعر</th>
                  <th className="py-1 text-left">الإجمالي</th>
                </tr>
              </thead>
              <tbody>
                {invoice.items.map((item, idx) => (
                  <tr key={idx} className="border-b border-slate-100">
                    <td className="py-1 font-medium leading-tight">
                      {item.item.name}
                      <span className="block text-[9px] text-slate-500">{item.selectedUnit}</span>
                    </td>
                    <td className="py-1 text-center font-mono-numbers">{item.quantity}</td>
                    <td className="py-1 text-left font-mono-numbers">{item.unitPrice.toLocaleString()}</td>
                    <td className="py-1 text-left font-mono-numbers font-bold">{item.total.toLocaleString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>

            {/* Totals Summary */}
            <div className="border-t border-dashed border-slate-400 pt-2 flex flex-col gap-1 text-[11px]">
              <div className="flex justify-between">
                <span>المجموع الفرعي:</span>
                <span className="font-mono-numbers">{invoice.subtotal.toLocaleString()} {currency}</span>
              </div>
              {invoice.discountTotal > 0 && (
                <div className="flex justify-between text-rose-600">
                  <span>الخصم الممنوح:</span>
                  <span className="font-mono-numbers">-{invoice.discountTotal.toLocaleString()} {currency}</span>
                </div>
              )}
              <div className="flex justify-between text-sm font-black border-t border-b border-slate-800 py-1.5 mt-1">
                <span>المبلغ الصافي المستحق:</span>
                <span className="font-mono-numbers">{invoice.grandTotal.toLocaleString()} {currency}</span>
              </div>
              
              {invoice.splitPayments ? (
                <div className="bg-slate-50 p-2 rounded border border-slate-200 mt-1 flex flex-col gap-1">
                  <span className="font-bold text-slate-800 text-[10px]">تفاصيل السداد وحسابات الترحيل:</span>
                  {invoice.splitPayments.cash !== undefined && invoice.splitPayments.cash > 0 && (
                    <div className="flex justify-between text-slate-700">
                      <span>• نقد ({invoice.splitPayments.cashAccountName || 'كاش'}):</span>
                      <span className="font-mono-numbers font-bold">{invoice.splitPayments.cash.toLocaleString()} {currency}</span>
                    </div>
                  )}
                  {invoice.splitPayments.network !== undefined && invoice.splitPayments.network > 0 && (
                    <div className="flex flex-col text-slate-700">
                      <div className="flex justify-between">
                        <span>• بنك ({invoice.splitPayments.bankAccountName || 'شبكة'}):</span>
                        <span className="font-mono-numbers font-bold">{invoice.splitPayments.network.toLocaleString()} {currency}</span>
                      </div>
                      {invoice.splitPayments.bankRefNo && (
                        <span className="text-[9px] text-slate-500 mr-2">مرجع: {invoice.splitPayments.bankRefNo}</span>
                      )}
                    </div>
                  )}
                  {invoice.splitPayments.wallet !== undefined && invoice.splitPayments.wallet > 0 && (
                    <div className="flex justify-between text-slate-700">
                      <span>• محفظة ({invoice.splitPayments.walletAccountName || 'جيب/ون'}):</span>
                      <span className="font-mono-numbers font-bold">{invoice.splitPayments.wallet.toLocaleString()} {currency}</span>
                    </div>
                  )}
                  {invoice.splitPayments.credit !== undefined && invoice.splitPayments.credit > 0 && (
                    <div className="flex justify-between text-amber-800 font-bold">
                      <span>• آجل على ({invoice.splitPayments.creditCustomerName || invoice.customerOrSupplierName}):</span>
                      <span className="font-mono-numbers">{invoice.splitPayments.credit.toLocaleString()} {currency}</span>
                    </div>
                  )}
                </div>
              ) : invoice.paymentType === 'CASH' ? (
                <>
                  <div className="flex justify-between mt-1">
                    <span>المدفوع نقداً:</span>
                    <span className="font-mono-numbers font-bold">{invoice.paidAmount.toLocaleString()} {currency}</span>
                  </div>
                  <div className="flex justify-between">
                    <span>المتبقي (الفكة):</span>
                    <span className="font-mono-numbers font-bold text-emerald-700">
                      {Math.max(0, invoice.paidAmount - invoice.grandTotal).toLocaleString()} {currency}
                    </span>
                  </div>
                </>
              ) : invoice.paymentType === 'CREDIT' ? (
                <div className="flex justify-between text-amber-800 font-bold mt-1">
                  <span>المبلغ المرحل للحساب (آجل):</span>
                  <span className="font-mono-numbers">{invoice.grandTotal.toLocaleString()} {currency}</span>
                </div>
              ) : (
                <div className="flex justify-between text-sky-800 font-bold mt-1">
                  <span>طريقة السداد:</span>
                  <span>{invoice.paymentType === 'KURAYMI' ? 'بنك الكريمي / شبكة' : 'محفظة إلكترونية'}</span>
                </div>
              )}
            </div>

            {/* QR Code & Footer */}
            <div className="text-center mt-4 pt-2 border-t border-dashed border-slate-400 flex flex-col items-center gap-1">
              <div className="p-1 border border-slate-300 rounded bg-white">
                <QrCode className="w-16 h-16 text-slate-800" />
              </div>
              <p className="text-[10px] text-slate-500 font-semibold mt-1">
                شكراً لتسوقكم معنا | البضاعة المباعة ترد وتستبدل خلال 3 أيام
              </p>
              <span className="text-[9px] text-slate-400">نظام اليمن للمبيعات والمحاسبة ERP v2.4</span>
            </div>
          </div>
        </div>

        {/* Modal Footer Controls */}
        <div className="bg-slate-100 px-4 py-3 border-t border-slate-300 flex items-center justify-between gap-2">
          <button
            onClick={onClose}
            className="bg-white border border-slate-300 hover:bg-slate-50 text-slate-700 px-3 py-1.5 rounded text-xs cursor-pointer font-semibold"
          >
            إغلاق
          </button>

          <div className="flex items-center gap-2">
            <button
              onClick={handleExportXLSX}
              className="bg-emerald-600 hover:bg-emerald-700 text-white font-bold px-3 py-1.5 rounded text-xs flex items-center gap-1 shadow cursor-pointer transition"
              title="تصدير الفاتورة إلى ملف إكسل (.xlsx)"
            >
              <FileSpreadsheet className="w-3.5 h-3.5" />
              <span>تصدير XLSX</span>
            </button>

            <button
              onClick={handlePrint}
              className="bg-sky-700 hover:bg-sky-800 text-white font-bold px-3.5 py-1.5 rounded text-xs flex items-center gap-1.5 shadow cursor-pointer transition"
              title="طباعة الفاتورة أو حفظ بتنسيق PDF"
            >
              <Printer className="w-3.5 h-3.5" />
              <span>طباعة / PDF</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
