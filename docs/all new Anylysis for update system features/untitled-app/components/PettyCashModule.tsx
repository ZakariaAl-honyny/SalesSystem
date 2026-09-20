import React, { useState, useMemo } from 'react';
import { PettyCashRequest, SettlementExpense } from '@/types/pos';
import { getCurrentDateString } from '@/lib/idGenerator';
import { 
  ArrowLeft, Plus, Save, FileText, Check, X, Search, DollarSign, Clock, AlertCircle, FileCheck
} from 'lucide-react';

interface PettyCashModuleProps {
  onBack: () => void;
  currency: string;
  cashierName: string;
}

const MOCK_PETTY_CASH: PettyCashRequest[] = [
  {
    id: 'PC-1001', requestNo: 'REQ-2026-001', date: '2026-05-15', employeeName: 'أحمد محمود', amount: 500, reason: 'شراء ضيافة للمكتب', status: 'SETTLED', approvedBy: 'المدير المالي', disbursedDate: '2026-05-16', settlementDate: '2026-05-20', settlementAmount: 480, refundAmount: 20
  },
  {
    id: 'PC-1002', requestNo: 'REQ-2026-002', date: getCurrentDateString(), employeeName: 'محمد سعيد', amount: 150, reason: 'مصاريف بريد شحن سريع', status: 'PENDING'
  },
  {
    id: 'PC-1003', requestNo: 'REQ-2026-003', date: getCurrentDateString(), employeeName: 'علي عبدالله', amount: 300, reason: 'تعبئة وقود لسيارة التوصيل', status: 'DISBURSED', approvedBy: 'المدير المالي', disbursedDate: getCurrentDateString()
  }
];

// --- Analytical Requirements for V2 (Database Integration) ---
// 1. Chart of Accounts Integration: Expenses logged during settlement must map to specific Account IDs (e.g. 5101 for Office Supplies) to automatically generate Journal Entries and affect the Income Statement.
// 2. Automated Voucher Generation: 
//    - Upon "Disburse" (صرف), a Payment Voucher is auto-generated (Cr: Cash, Dr: Employee Advances).
//    - Upon "Settlement" (تسوية), Journal Entries are auto-generated (Cr: Employee Advances, Dr: Specific Expense Accounts).
//    - If there is a refund (refundAmount > 0), a Receipt Voucher is auto-generated (Cr: Employee Advances, Dr: Cash).
// 3. Policy Enforcement: Employees with an existing 'DISBURSED' request cannot open a new request without explicit confirmation, preventing accumulated un-settled advances.

const MOCK_EXPENSE_ACCOUNTS = [
  { id: '5101', name: 'مصاريف بوفيه وضيافة' },
  { id: '5102', name: 'مصاريف بريد وشحن' },
  { id: '5103', name: 'مصاريف محروقات وتنقلات' },
  { id: '5104', name: 'مصاريف صيانة ونظافة' },
  { id: '5105', name: 'مصاريف نثرية أخرى' },
];

export const PettyCashModule: React.FC<PettyCashModuleProps> = ({ onBack, currency, cashierName }) => {
  const [requests, setRequests] = useState<PettyCashRequest[]>(MOCK_PETTY_CASH);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<'ALL' | 'PENDING' | 'DISBURSED' | 'SETTLED'>('ALL');
  
  // Modals state
  const [showRequestModal, setShowRequestModal] = useState(false);
  const [showSettlementModal, setShowSettlementModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<PettyCashRequest | null>(null);

  // New Request Form
  const [reqEmpName, setReqEmpName] = useState('');
  const [reqAmount, setReqAmount] = useState('');
  const [reqReason, setReqReason] = useState('');

  // Settlement Form
  const [settleExpenses, setSettleExpenses] = useState<SettlementExpense[]>([]);
  const [settleAccountId, setSettleAccountId] = useState('');
  const [settleDesc, setSettleDesc] = useState('');
  const [settleAmt, setSettleAmt] = useState('');
  const [settleInv, setSettleInv] = useState('');

  const filteredRequests = useMemo(() => {
    return requests.filter(r => {
      const matchSearch = r.employeeName.includes(searchTerm) || r.requestNo.includes(searchTerm);
      const matchStatus = statusFilter === 'ALL' || r.status === statusFilter;
      return matchSearch && matchStatus;
    });
  }, [requests, searchTerm, statusFilter]);

  const handleCreateRequest = () => {
    if (!reqEmpName || !reqAmount || !reqReason) {
      alert('يرجى تعبئة كافة الحقول المطلوبة.');
      return;
    }

    // Policy Control: Check if employee already has an unsettled advance
    const hasUnsettled = requests.some(r => r.employeeName === reqEmpName && r.status === 'DISBURSED');
    if (hasUnsettled) {
      const confirmProceed = confirm(`تنبيه نظام الرقابة: الموظف (${reqEmpName}) لديه عهدة سابقة لم يتم تسويتها بعد.\nهل أنت متأكد من رغبتك في رفع طلب عهدة جديدة له؟`);
      if (!confirmProceed) return;
    }

    const newReq: PettyCashRequest = {
      id: `PC-${Date.now()}`,
      requestNo: `REQ-${Date.now().toString().slice(-6)}`,
      date: getCurrentDateString(),
      employeeName: reqEmpName,
      amount: parseFloat(reqAmount),
      reason: reqReason,
      status: 'PENDING',
    };
    setRequests([newReq, ...requests]);
    setShowRequestModal(false);
    setReqEmpName('');
    setReqAmount('');
    setReqReason('');
  };

  const handleApproveDisburse = (id: string) => {
    if(confirm('تأكيد الموافقة وصرف العهدة للموظف؟\n(ملاحظة: سيقوم النظام آلياً بتوليد سند صرف نقدي وقيد يومية في الخلفية)')) {
      setRequests(prev => prev.map(r => r.id === id ? { ...r, status: 'DISBURSED', approvedBy: cashierName, disbursedDate: getCurrentDateString() } : r));
      alert('تم الاعتماد. [محاكاة]: تم إنشاء سند صرف رقم (PV-993) بقيمة العهدة.');
    }
  };

  const handleAddSettlementExpense = () => {
    if (!settleAccountId || !settleDesc || !settleAmt) {
      alert('يرجى اختيار حساب المصروف وإدخال البيان والمبلغ.');
      return;
    }
    setSettleExpenses([...settleExpenses, {
      id: `EXP-${Date.now()}`,
      date: getCurrentDateString(),
      description: settleDesc,
      amount: parseFloat(settleAmt),
      invoiceNumber: settleInv,
      expenseAccountId: settleAccountId
    }]);
    setSettleDesc('');
    setSettleAmt('');
    setSettleInv('');
    setSettleAccountId('');
  };

  const handleFinalizeSettlement = () => {
    if (!selectedRequest) return;
    const totalSpent = settleExpenses.reduce((sum, e) => sum + e.amount, 0);
    const refund = selectedRequest.amount - totalSpent;
    
    if (refund < 0) {
      alert('المبلغ المنصرف أكبر من العهدة! يرجى مراجعة الإدارة المالية لصرف الفارق.');
      return;
    }

    setRequests(prev => prev.map(r => r.id === selectedRequest.id ? { 
      ...r, 
      status: 'SETTLED', 
      settlementDate: getCurrentDateString(),
      settlementAmount: totalSpent,
      refundAmount: refund
    } : r));

    setShowSettlementModal(false);
    setSelectedRequest(null);
    setSettleExpenses([]);

    // Automated Process Mock Note
    let automationMessage = `تم اعتماد التسوية النهائية بنجاح.\n[محاكاة]: تم إنشاء قيد يومية بـ ${settleExpenses.length} مصروف(ات) لإغلاق العهدة.`;
    if (refund > 0) {
      automationMessage += `\n[محاكاة]: تم إصدار سند قبض آلي رقم (RV-881) بالمبلغ المسترد (${refund} ${currency}) لإيداعه بالصندوق.`;
    }
    alert(automationMessage);
  };

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] text-slate-800 overflow-hidden text-xs select-none">
      <div className="bg-white text-slate-800 border-b border-slate-200 px-4 py-3 flex items-center justify-between shadow-xs shrink-0">
        <div className="flex items-center gap-3">
          <button onClick={onBack} className="p-1.5 hover:bg-slate-100 rounded-full transition cursor-pointer text-slate-700" title="رجوع">
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div className="flex items-center gap-2">
            <h2 className="text-base font-bold text-slate-800">إدارة العهد المالية والتسويات</h2>
            <span className="text-[11px] bg-sky-50 text-sky-700 border border-sky-200 px-2 py-0.5 rounded-full font-bold">
              Petty Cash Management
            </span>
          </div>
        </div>
        <button
          onClick={() => setShowRequestModal(true)}
          className="bg-sky-600 hover:bg-sky-700 text-white px-3 py-1.5 rounded text-xs font-bold flex items-center gap-1.5 transition shadow-xs cursor-pointer"
        >
          <Plus className="w-4 h-4" />
          <span>طلب عهدة جديدة</span>
        </button>
      </div>

      <div className="flex-1 overflow-auto p-4 flex flex-col">
        <div className="max-w-6xl mx-auto w-full space-y-4">
          {/* Filters */}
          <div className="bg-white p-3 rounded-lg shadow-sm border border-slate-200 flex items-center justify-between gap-4">
            <div className="flex items-center gap-3 flex-1">
              <div className="relative flex-1 max-w-sm">
                <input
                  type="text"
                  placeholder="بحث باسم الموظف أو رقم الطلب..."
                  value={searchTerm}
                  onChange={e => setSearchTerm(e.target.value)}
                  className="w-full pl-9 pr-3 py-1.5 bg-slate-50 border border-slate-300 rounded focus:border-sky-500 focus:ring-1 focus:ring-sky-500 outline-none text-xs"
                />
                <Search className="w-4 h-4 text-slate-400 absolute left-3 top-2" />
              </div>
              <select
                value={statusFilter}
                onChange={e => setStatusFilter(e.target.value as any)}
                className="bg-slate-50 border border-slate-300 rounded px-3 py-1.5 font-bold text-slate-700 outline-none"
              >
                <option value="ALL">جميع الحالات</option>
                <option value="PENDING">طلبات قيد الانتظار</option>
                <option value="DISBURSED">عُهد مصروفة (بانتظار التسوية)</option>
                <option value="SETTLED">عُهد مسواة (مغلقة)</option>
              </select>
            </div>
            
            <div className="flex gap-4 text-[11px] font-bold">
              <div className="bg-amber-50 text-amber-800 px-3 py-1.5 rounded border border-amber-200 text-center min-w-[100px]">
                <div className="text-slate-500 font-normal mb-0.5">قيد الانتظار</div>
                {requests.filter(r => r.status === 'PENDING').length} طلب
              </div>
              <div className="bg-sky-50 text-sky-800 px-3 py-1.5 rounded border border-sky-200 text-center min-w-[100px]">
                <div className="text-slate-500 font-normal mb-0.5">بانتظار التسوية</div>
                {requests.filter(r => r.status === 'DISBURSED').length} عهدة
              </div>
            </div>
          </div>

          {/* List */}
          <div className="bg-white rounded-lg shadow-sm border border-slate-200 overflow-hidden">
            <table className="w-full text-right border-collapse">
              <thead className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200 text-[11px]">
                <tr>
                  <th className="p-3 w-32">رقم الطلب</th>
                  <th className="p-3 w-32">التاريخ</th>
                  <th className="p-3">الموظف / المستفيد</th>
                  <th className="p-3 text-center">المبلغ</th>
                  <th className="p-3">الغرض</th>
                  <th className="p-3 text-center">الحالة</th>
                  <th className="p-3 text-center">الإجراءات</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {filteredRequests.map(req => (
                  <tr key={req.id} className="hover:bg-slate-50 transition">
                    <td className="p-3 font-mono font-bold text-sky-700">{req.requestNo}</td>
                    <td className="p-3 font-mono text-slate-600">{req.date}</td>
                    <td className="p-3 font-bold text-slate-800">{req.employeeName}</td>
                    <td className="p-3 text-center font-mono font-bold text-emerald-700">{req.amount.toLocaleString()} {currency}</td>
                    <td className="p-3 text-slate-600">{req.reason}</td>
                    <td className="p-3 text-center">
                      <span className={`px-2 py-1 rounded text-[10px] font-bold ${
                        req.status === 'PENDING' ? 'bg-amber-100 text-amber-800' :
                        req.status === 'DISBURSED' ? 'bg-sky-100 text-sky-800' :
                        'bg-emerald-100 text-emerald-800'
                      }`}>
                        {req.status === 'PENDING' ? 'قيد الانتظار' : req.status === 'DISBURSED' ? 'مصروفة - معلقة' : 'مُسواة'}
                      </span>
                    </td>
                    <td className="p-3 text-center">
                      {req.status === 'PENDING' && (
                        <button onClick={() => handleApproveDisburse(req.id)} className="text-white bg-emerald-500 hover:bg-emerald-600 px-3 py-1 rounded text-[10px] font-bold transition">
                          اعتماد وصرف
                        </button>
                      )}
                      {req.status === 'DISBURSED' && (
                        <button 
                          onClick={() => { setSelectedRequest(req); setShowSettlementModal(true); setSettleExpenses([]); }} 
                          className="text-white bg-indigo-500 hover:bg-indigo-600 px-3 py-1 rounded text-[10px] font-bold transition flex items-center gap-1 justify-center w-full"
                        >
                          <FileCheck className="w-3.5 h-3.5" /> إجراء تسوية
                        </button>
                      )}
                      {req.status === 'SETTLED' && (
                        <span className="text-slate-400 font-bold text-[10px]">مكتملة</span>
                      )}
                    </td>
                  </tr>
                ))}
                {filteredRequests.length === 0 && (
                  <tr>
                    <td colSpan={7} className="p-6 text-center text-slate-500">لا توجد عهد مالية مسجلة</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {/* New Request Modal */}
      {showRequestModal && (
        <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg shadow-xl w-full max-w-md overflow-hidden">
            <div className="flex items-center justify-between p-3 border-b border-slate-200 bg-slate-50">
              <h3 className="font-bold text-slate-800 text-sm flex items-center gap-2">
                <Plus className="w-4 h-4 text-sky-600" /> طلب صرف عهدة مالية
              </h3>
              <button onClick={() => setShowRequestModal(false)} className="text-slate-400 hover:text-slate-600 transition"><X className="w-5 h-5" /></button>
            </div>
            <div className="p-4 space-y-4">
              <div>
                <label className="block text-[11px] font-bold text-slate-600 mb-1">اسم الموظف / المستفيد *</label>
                <input type="text" value={reqEmpName} onChange={e => setReqEmpName(e.target.value)} className="w-full border border-slate-300 rounded px-3 py-2 font-bold text-slate-800" placeholder="أدخل اسم الموظف" />
              </div>
              <div>
                <label className="block text-[11px] font-bold text-slate-600 mb-1">مبلغ العهدة المطلوبة ({currency}) *</label>
                <input type="number" value={reqAmount} onChange={e => setReqAmount(e.target.value)} className="w-full border border-slate-300 rounded px-3 py-2 font-bold font-mono text-slate-800" placeholder="0.00" />
              </div>
              <div>
                <label className="block text-[11px] font-bold text-slate-600 mb-1">الغرض من العهدة *</label>
                <textarea value={reqReason} onChange={e => setReqReason(e.target.value)} className="w-full border border-slate-300 rounded px-3 py-2 font-normal text-slate-800 resize-none h-20" placeholder="اذكر سبب طلب العهدة بالتفصيل..." />
              </div>
            </div>
            <div className="p-3 border-t border-slate-200 bg-slate-50 flex justify-end gap-2">
              <button onClick={() => setShowRequestModal(false)} className="px-4 py-1.5 rounded font-bold text-slate-600 bg-slate-200 hover:bg-slate-300 transition">إلغاء</button>
              <button onClick={handleCreateRequest} className="px-4 py-1.5 rounded font-bold text-white bg-sky-600 hover:bg-sky-700 transition flex items-center gap-1.5">
                <Check className="w-4 h-4" /> تأكيد ورفع الطلب
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Settlement Modal */}
      {showSettlementModal && selectedRequest && (
        <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-xs flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg shadow-xl w-full max-w-3xl overflow-hidden flex flex-col max-h-[90vh]">
            <div className="flex items-center justify-between p-3 border-b border-slate-200 bg-indigo-50">
              <h3 className="font-bold text-indigo-900 text-sm flex items-center gap-2">
                <FileCheck className="w-4 h-4 text-indigo-600" /> تسوية العهدة: {selectedRequest.requestNo} ({selectedRequest.employeeName})
              </h3>
              <button onClick={() => setShowSettlementModal(false)} className="text-slate-400 hover:text-slate-600 transition"><X className="w-5 h-5" /></button>
            </div>
            
            <div className="flex-1 overflow-y-auto p-4 space-y-4">
              <div className="bg-slate-100 p-3 rounded border border-slate-200 flex justify-between items-center">
                <div>
                  <div className="text-[11px] text-slate-500 font-bold mb-0.5">إجمالي العهدة المستلمة</div>
                  <div className="text-base font-bold font-mono text-slate-800">{selectedRequest.amount.toLocaleString()} {currency}</div>
                </div>
                <div className="text-right">
                  <div className="text-[11px] text-slate-500 font-bold mb-0.5">إجمالي المصاريف المسجلة</div>
                  <div className="text-base font-bold font-mono text-rose-600">{settleExpenses.reduce((s, e) => s + e.amount, 0).toLocaleString()} {currency}</div>
                </div>
                <div className="text-right">
                  <div className="text-[11px] text-slate-500 font-bold mb-0.5">المبلغ المتبقي (للاسترداد)</div>
                  <div className="text-base font-bold font-mono text-emerald-600">{(selectedRequest.amount - settleExpenses.reduce((s, e) => s + e.amount, 0)).toLocaleString()} {currency}</div>
                </div>
              </div>

              {/* Add Expense Form */}
              <div className="bg-white border border-slate-200 rounded p-3 shadow-xs space-y-3">
                <h4 className="font-bold text-slate-700 text-xs flex items-center gap-1.5"><Plus className="w-3.5 h-3.5" /> إضافة فاتورة / إيصال مصروفات</h4>
                <div className="flex gap-2 items-end">
                  <div className="w-48">
                    <label className="block text-[10px] font-bold text-slate-500 mb-1">حساب المصروف (التوجيه المحاسبي) *</label>
                    <select value={settleAccountId} onChange={e => setSettleAccountId(e.target.value)} className="w-full border border-slate-300 rounded px-2 py-1.5 text-xs outline-none focus:border-indigo-500">
                      <option value="">-- اختر الحساب --</option>
                      {MOCK_EXPENSE_ACCOUNTS.map(acc => (
                        <option key={acc.id} value={acc.id}>{acc.name}</option>
                      ))}
                    </select>
                  </div>
                  <div className="flex-1">
                    <label className="block text-[10px] font-bold text-slate-500 mb-1">البيان / الوصف *</label>
                    <input type="text" value={settleDesc} onChange={e => setSettleDesc(e.target.value)} className="w-full border border-slate-300 rounded px-2 py-1.5 outline-none focus:border-indigo-500" placeholder="تفاصيل المصروف..." />
                  </div>
                  <div className="w-24">
                    <label className="block text-[10px] font-bold text-slate-500 mb-1">رقم الفاتورة</label>
                    <input type="text" value={settleInv} onChange={e => setSettleInv(e.target.value)} className="w-full border border-slate-300 rounded px-2 py-1.5 font-mono outline-none focus:border-indigo-500" placeholder="اختياري" />
                  </div>
                  <div className="w-28">
                    <label className="block text-[10px] font-bold text-slate-500 mb-1">المبلغ ({currency}) *</label>
                    <input type="number" value={settleAmt} onChange={e => setSettleAmt(e.target.value)} className="w-full border border-slate-300 rounded px-2 py-1.5 font-mono font-bold outline-none focus:border-indigo-500" placeholder="0.00" />
                  </div>
                  <button onClick={handleAddSettlementExpense} className="bg-slate-800 hover:bg-slate-900 text-white px-4 py-1.5 rounded font-bold transition h-[34px] flex items-center justify-center cursor-pointer">
                    إضافة
                  </button>
                </div>
              </div>

              {/* Expenses List */}
              <div className="border border-slate-200 rounded overflow-hidden">
                <table className="w-full text-right">
                  <thead className="bg-slate-50 border-b border-slate-200 text-[10px] text-slate-500 font-bold">
                    <tr>
                      <th className="p-2">التاريخ</th>
                      <th className="p-2">حساب المصروف (توجيه القيد)</th>
                      <th className="p-2">البيان</th>
                      <th className="p-2">رقم الفاتورة</th>
                      <th className="p-2 text-center">المبلغ</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {settleExpenses.map(exp => (
                      <tr key={exp.id}>
                        <td className="p-2 font-mono text-slate-600">{exp.date}</td>
                        <td className="p-2 text-indigo-700 font-bold">
                          {MOCK_EXPENSE_ACCOUNTS.find(a => a.id === exp.expenseAccountId)?.name || exp.expenseAccountId}
                        </td>
                        <td className="p-2 font-bold text-slate-800">{exp.description}</td>
                        <td className="p-2 font-mono text-slate-500">{exp.invoiceNumber || '-'}</td>
                        <td className="p-2 text-center font-mono font-bold text-rose-600">{exp.amount.toLocaleString()} {currency}</td>
                      </tr>
                    ))}
                    {settleExpenses.length === 0 && (
                      <tr><td colSpan={5} className="p-4 text-center text-slate-400">لم يتم إضافة أي مصاريف حتى الآن</td></tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>

            <div className="p-3 border-t border-slate-200 bg-slate-50 flex justify-end gap-2 shrink-0">
              <button onClick={() => setShowSettlementModal(false)} className="px-4 py-1.5 rounded font-bold text-slate-600 bg-slate-200 hover:bg-slate-300 transition">إلغاء وإغلاق</button>
              <button 
                onClick={handleFinalizeSettlement} 
                disabled={settleExpenses.length === 0}
                className="px-4 py-1.5 rounded font-bold text-white bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed transition flex items-center gap-1.5"
              >
                <Check className="w-4 h-4" /> اعتماد التسوية النهائية
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
