'use client';

import React, { useState } from 'react';
import { 
  EmployeeItem, 
  SystemUser, 
  AccountNode, 
  CashVoucher 
} from '@/types/pos';
import { 
  UserCheck, 
  Plus, 
  Search, 
  Edit3, 
  Trash2, 
  Phone, 
  Calendar, 
  DollarSign, 
  ShieldCheck, 
  UserX, 
  CheckCircle2, 
  AlertCircle, 
  Clock, 
  FileText, 
  Sparkles,
  ChevronLeft,
  Coins,
  BadgeCheck,
  UserPlus,
  ArrowDownLeft
} from 'lucide-react';

interface EmployeesModuleProps {
  employees: EmployeeItem[];
  onAddEmployee: (emp: Omit<EmployeeItem, 'id'>) => void;
  onUpdateEmployee: (emp: EmployeeItem) => void;
  onDeleteEmployee: (id: number) => void;
  users: SystemUser[];
  onOpenSalaryAdvanceVoucher?: (employee: EmployeeItem) => void;
  onOpenSalaryPaymentVoucher?: (employee: EmployeeItem) => void;
}

export const EmployeesModule: React.FC<EmployeesModuleProps> = ({
  employees,
  onAddEmployee,
  onUpdateEmployee,
  onDeleteEmployee,
  users,
  onOpenSalaryAdvanceVoucher,
  onOpenSalaryPaymentVoucher,
}) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterActive, setFilterActive] = useState<'ALL' | 'ACTIVE' | 'INACTIVE'>('ALL');
  const [filterRole, setFilterRole] = useState<string>('ALL');

  // Modal State
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingEmployee, setEditingEmployee] = useState<EmployeeItem | null>(null);

  // Form State
  const [formName, setFormName] = useState('');
  const [formCode, setFormCode] = useState('');
  const [formJobTitle, setFormJobTitle] = useState('بائع');
  const [formPhone, setFormPhone] = useState('');
  const [formNationalId, setFormNationalId] = useState('');
  const [formBasicSalary, setFormBasicSalary] = useState<number>(150000);
  const [formHireDate, setFormHireDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [formIsActive, setFormIsActive] = useState<boolean>(true);
  const [formNotes, setFormNotes] = useState('');
  const [formLinkedUserId, setFormLinkedUserId] = useState<string>('');
  const [formCommissionRate, setFormCommissionRate] = useState<number>(0);
  const [formError, setFormError] = useState<string | null>(null);

  // Roadmap V2 Info Modal
  const [showRoadmapModal, setShowRoadmapModal] = useState<boolean>(false);

  const openAddModal = () => {
    setEditingEmployee(null);
    const nextSeq = employees.length > 0 ? Math.max(...employees.map(e => e.id)) + 1 : 1;
    setFormCode((100 + nextSeq).toString());
    setFormName('');
    setFormJobTitle('بائع');
    setFormPhone('');
    setFormNationalId('');
    setFormBasicSalary(150000);
    setFormHireDate(new Date().toISOString().split('T')[0]);
    setFormIsActive(true);
    setFormNotes('');
    setFormLinkedUserId('');
    setFormCommissionRate(0);
    setFormError(null);
    setIsModalOpen(true);
  };

  const openEditModal = (emp: EmployeeItem) => {
    setEditingEmployee(emp);
    setFormCode(emp.code);
    setFormName(emp.name);
    setFormJobTitle(emp.jobTitle);
    setFormPhone(emp.phone);
    setFormNationalId(emp.nationalId || '');
    setFormBasicSalary(emp.basicSalary);
    setFormHireDate(emp.hireDate);
    setFormIsActive(emp.isActive);
    setFormNotes(emp.notes || '');
    setFormLinkedUserId(emp.linkedUserId || '');
    setFormCommissionRate(emp.commissionRate || 0);
    setFormError(null);
    setIsModalOpen(true);
  };

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formName.trim()) {
      setFormError('يرجى إدخال اسم الموظف كاملاً');
      return;
    }
    if (!formPhone.trim()) {
      setFormError('يرجى إدخال رقم هاتف الموظف للتواصل');
      return;
    }

    if (editingEmployee) {
      onUpdateEmployee({
        ...editingEmployee,
        code: formCode,
        name: formName.trim(),
        jobTitle: formJobTitle.trim(),
        phone: formPhone.trim(),
        nationalId: formNationalId.trim(),
        basicSalary: Number(formBasicSalary) || 0,
        hireDate: formHireDate,
        isActive: formIsActive,
        notes: formNotes.trim(),
        linkedUserId: formLinkedUserId ? formLinkedUserId : undefined,
        commissionRate: Number(formCommissionRate) || 0,
      });
    } else {
      onAddEmployee({
        code: formCode,
        name: formName.trim(),
        jobTitle: formJobTitle.trim(),
        phone: formPhone.trim(),
        nationalId: formNationalId.trim(),
        basicSalary: Number(formBasicSalary) || 0,
        hireDate: formHireDate,
        isActive: formIsActive,
        notes: formNotes.trim(),
        accountCode: `2141-${String(employees.length + 1).padStart(2, '0')}`,
        linkedUserId: formLinkedUserId ? formLinkedUserId : undefined,
        commissionRate: Number(formCommissionRate) || 0,
      });
    }
    setIsModalOpen(false);
  };

  // Filtered List
  const filteredEmployees = employees.filter(emp => {
    const matchSearch = emp.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                        emp.phone.includes(searchTerm) ||
                        emp.code.includes(searchTerm) ||
                        emp.jobTitle.toLowerCase().includes(searchTerm.toLowerCase());
    const matchActive = filterActive === 'ALL' || (filterActive === 'ACTIVE' ? emp.isActive : !emp.isActive);
    const matchRole = filterRole === 'ALL' || emp.jobTitle === filterRole;
    return matchSearch && matchActive && matchRole;
  });

  const totalSalaries = employees.reduce((sum, e) => sum + (e.isActive ? e.basicSalary : 0), 0);
  const activeCount = employees.filter(e => e.isActive).length;
  const usersLinkedCount = employees.filter(e => !!e.linkedUserId).length;

  return (
    <div className="flex-1 flex flex-col h-full bg-[#f8fafc] overflow-hidden font-sans select-none" dir="rtl">
      {/* 1. Module Top Bar */}
      <header className="bg-white text-slate-800 border-b border-slate-200 px-4 py-3 flex items-center justify-between shadow-xs shrink-0">
        <div className="flex items-center gap-2.5">
          <div className="p-2 bg-sky-50 text-sky-700 rounded-lg border border-sky-200">
            <UserCheck className="w-5 h-5 text-sky-700" />
          </div>
          <div>
            <h1 className="text-base font-black tracking-wide flex items-center gap-2 text-slate-800">
              <span>إدارة الموظفين والكوادر</span>
              <span className="text-[11px] bg-sky-50 text-sky-700 border border-sky-200 px-2 py-0.5 rounded-full font-mono font-bold">
                الإصدار V1.0 (السريع)
              </span>
            </h1>
            <p className="text-xs text-slate-500 font-medium">
              دليل بيانات الموظفين، ربط المبيعات بالمندوبين، وسندات السلف والرواتب الفورية
            </p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <button
            type="button"
            onClick={() => setShowRoadmapModal(true)}
            className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-200 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer"
          >
            <Sparkles className="w-4 h-4 text-amber-500" />
            <span>خارطة طريق V2 (قريباً)</span>
          </button>

          <button
            type="button"
            onClick={openAddModal}
            className="px-3.5 py-1.5 bg-emerald-600 hover:bg-emerald-500 text-white rounded-lg text-xs font-bold shadow-xs transition flex items-center gap-1.5 cursor-pointer active:scale-95"
          >
            <Plus className="w-4 h-4" />
            <span>إضافة موظف جديد</span>
          </button>
        </div>
      </header>

      {/* 2. Key Metrics Strip */}
      <section className="bg-white border-b border-slate-200 px-4 py-2.5 grid grid-cols-2 sm:grid-cols-4 gap-3 shrink-0">
        <div className="flex items-center gap-3 bg-slate-50 p-2 rounded-xl border border-slate-200/80">
          <div className="w-9 h-9 rounded-lg bg-sky-100 text-sky-700 flex items-center justify-center font-bold">
            <UserCheck className="w-5 h-5" />
          </div>
          <div>
            <div className="text-[11px] font-bold text-slate-500">إجمالي الموظفين</div>
            <div className="text-sm font-black text-slate-900 font-mono">{employees.length} موظف</div>
          </div>
        </div>

        <div className="flex items-center gap-3 bg-slate-50 p-2 rounded-xl border border-slate-200/80">
          <div className="w-9 h-9 rounded-lg bg-emerald-100 text-emerald-700 flex items-center justify-center font-bold">
            <CheckCircle2 className="w-5 h-5" />
          </div>
          <div>
            <div className="text-[11px] font-bold text-slate-500">الموظفين على رأس العمل</div>
            <div className="text-sm font-black text-emerald-700 font-mono">{activeCount} موظف</div>
          </div>
        </div>

        <div className="flex items-center gap-3 bg-slate-50 p-2 rounded-xl border border-slate-200/80">
          <div className="w-9 h-9 rounded-lg bg-purple-100 text-purple-700 flex items-center justify-center font-bold">
            <ShieldCheck className="w-5 h-5" />
          </div>
          <div>
            <div className="text-[11px] font-bold text-slate-500">يمتلكون يوزرات نظام</div>
            <div className="text-sm font-black text-purple-700 font-mono">
              {usersLinkedCount} من أصل {employees.length}
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3 bg-slate-50 p-2 rounded-xl border border-slate-200/80">
          <div className="w-9 h-9 rounded-lg bg-amber-100 text-amber-700 flex items-center justify-center font-bold">
            <Coins className="w-5 h-5" />
          </div>
          <div>
            <div className="text-[11px] font-bold text-slate-500">كتلة الرواتب الأساسية</div>
            <div className="text-sm font-black text-slate-900 font-mono">
              {totalSalaries.toLocaleString()} <span className="text-[10px] text-slate-500">ريال</span>
            </div>
          </div>
        </div>
      </section>

      {/* 3. Search & Quick Filters Strip */}
      <div className="p-3 bg-white border-b border-slate-200 flex flex-wrap items-center justify-between gap-3 shrink-0">
        <div className="flex items-center gap-2 flex-1 max-w-md">
          <div className="relative w-full">
            <Search className="w-4 h-4 absolute right-3 top-2.5 text-slate-400" />
            <input
              type="text"
              placeholder="البحث بالاسم، رقم الهاتف، الكود، أو المسمى..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-3 pr-9 py-1.5 bg-slate-50 border border-slate-300 rounded-lg text-xs font-semibold text-slate-800 placeholder-slate-400 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
            />
          </div>
        </div>

        <div className="flex items-center gap-2">
          {/* Status Filter */}
          <div className="flex items-center bg-slate-100 p-0.5 rounded-lg border border-slate-200 text-xs font-bold">
            <button
              onClick={() => setFilterActive('ALL')}
              className={`px-2.5 py-1 rounded-md transition cursor-pointer ${
                filterActive === 'ALL' ? 'bg-white text-sky-800 shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              الكل ({employees.length})
            </button>
            <button
              onClick={() => setFilterActive('ACTIVE')}
              className={`px-2.5 py-1 rounded-md transition cursor-pointer ${
                filterActive === 'ACTIVE' ? 'bg-white text-emerald-700 shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              النشطين ({activeCount})
            </button>
            <button
              onClick={() => setFilterActive('INACTIVE')}
              className={`px-2.5 py-1 rounded-md transition cursor-pointer ${
                filterActive === 'INACTIVE' ? 'bg-white text-rose-700 shadow-xs' : 'text-slate-600 hover:text-slate-900'
              }`}
            >
              غير النشطين ({employees.length - activeCount})
            </button>
          </div>
        </div>
      </div>

      {/* 4. Main Employees Table */}
      <div className="flex-1 p-3 overflow-y-auto">
        <div className="bg-white rounded-xl shadow-xs border border-slate-200 overflow-hidden">
          <table className="w-full text-right text-xs">
            <thead className="bg-slate-100 text-slate-800 border-b border-slate-200 font-bold">
              <tr>
                <th className="py-2.5 px-3 font-bold w-12 text-center">#</th>
                <th className="py-2.5 px-3 font-bold">اسم الموظف</th>
                <th className="py-2.5 px-3 font-bold">المسمى الوظيفي</th>
                <th className="py-2.5 px-3 font-bold">رقم الهاتف</th>
                <th className="py-2.5 px-3 font-bold">حساب الدخول (يوزر)</th>
                <th className="py-2.5 px-3 font-bold">الراتب الأساسي</th>
                <th className="py-2.5 px-3 font-bold text-center">العمولة %</th>
                <th className="py-2.5 px-3 font-bold text-center">الحالة</th>
                <th className="py-2.5 px-3 font-bold text-center">العمليات والسندات</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {filteredEmployees.length === 0 ? (
                <tr>
                  <td colSpan={9} className="py-12 text-center text-slate-400">
                    <UserX className="w-10 h-10 mx-auto mb-2 opacity-40" />
                    <p className="font-bold text-sm">لا يوجد موظفين يطابقون معايير البحث</p>
                    <p className="text-xs text-slate-400 mt-1">انقر على &quot;إضافة موظف جديد&quot; لتسجيل بطاقة موظف</p>
                  </td>
                </tr>
              ) : (
                filteredEmployees.map((emp) => {
                  const linkedUser = users.find(u => u.id === emp.linkedUserId);
                  return (
                    <tr key={emp.id} className="hover:bg-sky-50/50 transition">
                      <td className="py-3 px-3 text-center font-mono font-bold text-slate-600">
                        {emp.code || emp.id}
                      </td>
                      <td className="py-3 px-3">
                        <div className="font-black text-slate-900 text-sm">{emp.name}</div>
                        {emp.notes && (
                          <div className="text-[11px] text-slate-500 line-clamp-1">{emp.notes}</div>
                        )}
                      </td>
                      <td className="py-3 px-3">
                        <span className="px-2 py-0.5 bg-slate-100 text-slate-700 rounded-md font-bold text-[11px]">
                          {emp.jobTitle}
                        </span>
                      </td>
                      <td className="py-3 px-3 font-mono font-bold text-slate-700">
                        <div className="flex items-center gap-1">
                          <Phone className="w-3.5 h-3.5 text-slate-400" />
                          <span>{emp.phone}</span>
                        </div>
                      </td>
                      <td className="py-3 px-3">
                        {linkedUser ? (
                          <div className="flex items-center gap-1 text-purple-800 font-bold bg-purple-50 px-2 py-0.5 rounded-md border border-purple-200 w-fit">
                            <BadgeCheck className="w-3.5 h-3.5 text-purple-600" />
                            <span>{linkedUser.userName}</span>
                            <span className="text-[10px] text-purple-600">({linkedUser.roleName})</span>
                          </div>
                        ) : (
                          <span className="text-[11px] text-slate-400 italic">بدون يوزر (ميداني/خدمات)</span>
                        )}
                      </td>
                      <td className="py-3 px-3 font-mono font-black text-slate-900">
                        {emp.basicSalary.toLocaleString()}{' '}
                        <span className="text-[10px] text-slate-500 font-normal">ريال</span>
                      </td>
                      <td className="py-3 px-3 text-center font-mono font-bold">
                        {emp.commissionRate && emp.commissionRate > 0 ? (
                          <span className="text-emerald-700 bg-emerald-50 px-1.5 py-0.5 rounded border border-emerald-200">
                            {emp.commissionRate}%
                          </span>
                        ) : (
                          <span className="text-slate-400">-</span>
                        )}
                      </td>
                      <td className="py-3 px-3 text-center">
                        {emp.isActive ? (
                          <span className="px-2 py-0.5 rounded-full text-[10px] font-bold bg-emerald-100 text-emerald-800 border border-emerald-200">
                            نشط
                          </span>
                        ) : (
                          <span className="px-2 py-0.5 rounded-full text-[10px] font-bold bg-rose-100 text-rose-800 border border-rose-200">
                            غير نشط
                          </span>
                        )}
                      </td>
                      <td className="py-3 px-3 text-center">
                        <div className="flex items-center justify-center gap-1.5">
                          {/* Fast Action: Advance Voucher */}
                          {onOpenSalaryAdvanceVoucher && (
                            <button
                              type="button"
                              onClick={() => onOpenSalaryAdvanceVoucher(emp)}
                              className="px-2 py-1 bg-amber-50 hover:bg-amber-100 text-amber-800 border border-amber-300 rounded font-bold text-[10px] flex items-center gap-1 transition cursor-pointer"
                              title="صرف سلفة سريعة للموظف بسند صرف"
                            >
                              <ArrowDownLeft className="w-3 h-3 text-amber-600" />
                              <span>سلفة</span>
                            </button>
                          )}

                          {/* Fast Action: Salary Payment */}
                          {onOpenSalaryPaymentVoucher && (
                            <button
                              type="button"
                              onClick={() => onOpenSalaryPaymentVoucher(emp)}
                              className="px-2 py-1 bg-sky-50 hover:bg-sky-100 text-sky-800 border border-sky-300 rounded font-bold text-[10px] flex items-center gap-1 transition cursor-pointer"
                              title="صرف راتب الموظف بسند صرف"
                            >
                              <Coins className="w-3 h-3 text-sky-600" />
                              <span>صرف راتب</span>
                            </button>
                          )}

                          <button
                            type="button"
                            onClick={() => openEditModal(emp)}
                            className="p-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded transition cursor-pointer"
                            title="تعديل بيانات الموظف"
                          >
                            <Edit3 className="w-3.5 h-3.5" />
                          </button>

                          <button
                            type="button"
                            onClick={() => {
                              if (confirm(`هل أنت متأكد من حذف بطاقة الموظف [${emp.name}]؟`)) {
                                onDeleteEmployee(emp.id);
                              }
                            }}
                            className="p-1.5 bg-rose-50 hover:bg-rose-100 text-rose-700 rounded transition cursor-pointer"
                            title="حذف الموظف"
                          >
                            <Trash2 className="w-3.5 h-3.5" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* 5. Add / Edit Employee Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60 backdrop-blur-xs animate-in fade-in">
          <div className="bg-white rounded-2xl shadow-2xl border border-slate-200 max-w-lg w-full overflow-hidden flex flex-col text-slate-800 animate-in zoom-in-95 duration-150">
            {/* Modal Header */}
            <div className="bg-gradient-to-r from-[#0277bd] to-[#01579b] text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <UserPlus className="w-5 h-5 text-sky-200" />
                <h3 className="font-bold text-sm">
                  {editingEmployee ? 'تعديل بطاقة موظف' : 'إضافة بطاقة موظف جديدة (V1)'}
                </h3>
              </div>
              <button
                type="button"
                onClick={() => setIsModalOpen(false)}
                className="text-sky-200 hover:text-white p-1 rounded-full text-xs font-bold cursor-pointer"
              >
                ✕
              </button>
            </div>

            {/* Modal Form */}
            <form onSubmit={handleSave} className="p-4 space-y-3.5 overflow-y-auto max-h-[80vh]">
              {formError && (
                <div className="bg-rose-50 border border-rose-300 text-rose-800 p-2.5 rounded-lg text-xs font-bold flex items-center gap-2">
                  <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
                  <span>{formError}</span>
                </div>
              )}

              <div className="grid grid-cols-3 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    كود الموظف:
                  </label>
                  <input
                    type="text"
                    value={formCode}
                    onChange={(e) => setFormCode(e.target.value)}
                    className="w-full bg-slate-50 border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-mono font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                    placeholder="مثال: 101"
                  />
                </div>

                <div className="col-span-2">
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    اسم الموظف كاملاً: <span className="text-rose-500">*</span>
                  </label>
                  <input
                    type="text"
                    required
                    value={formName}
                    onChange={(e) => setFormName(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-900 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                    placeholder="الاسم الثلاثي أو الرباعي..."
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    المسمى الوظيفي / الدور:
                  </label>
                  <select
                    value={formJobTitle}
                    onChange={(e) => setFormJobTitle(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                  >
                    <option value="كاشير مبيعات">كاشير مبيعات</option>
                    <option value="مندوب وبائع ميداني">مندوب وبائع ميداني</option>
                    <option value="بائع">بائع صالة</option>
                    <option value="أمين مخزن رئيسي">أمين مخزن</option>
                    <option value="عامل خدمات وحمال">عامل خدمات / حمال</option>
                    <option value="سائق نقل وتوزيع">سائق نقل وتوزيع</option>
                    <option value="محاسب عام">محاسب عام</option>
                    <option value="مدير فرع">مدير فرع</option>
                  </select>
                </div>

                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    رقم الهاتف: <span className="text-rose-500">*</span>
                  </label>
                  <input
                    type="tel"
                    required
                    value={formPhone}
                    onChange={(e) => setFormPhone(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-mono font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                    placeholder="77XXXXXXX"
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    الراتب الأساسي (ريال يمني):
                  </label>
                  <input
                    type="number"
                    min="0"
                    step="1000"
                    value={formBasicSalary}
                    onChange={(e) => setFormBasicSalary(Number(e.target.value))}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-mono font-bold text-slate-900 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                  />
                </div>

                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    عمولة المبيعات (% اختياري):
                  </label>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    step="0.1"
                    value={formCommissionRate}
                    onChange={(e) => setFormCommissionRate(Number(e.target.value))}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-mono font-bold text-slate-900 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                    placeholder="0"
                  />
                </div>
              </div>

              {/* Linked User Account (Optional - Principle: Employee ≠ User) */}
              <div className="bg-purple-50/70 border border-purple-200 rounded-xl p-3 space-y-1.5">
                <label className="block text-xs font-bold text-purple-900">
                  ربط الموظف بحساب مستخدم نظام (اختياري):
                </label>
                <select
                  value={formLinkedUserId}
                  onChange={(e) => setFormLinkedUserId(e.target.value)}
                  className="w-full bg-white border border-purple-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-purple-500/30"
                >
                  <option value="">-- بدون حساب دخول (موظف ميداني / خدمات) --</option>
                  {users.map(u => (
                    <option key={u.id} value={u.id}>
                      {u.fullName} ({u.userName} - {u.roleName})
                    </option>
                  ))}
                </select>
                <p className="text-[10px] text-purple-700 leading-relaxed">
                  💡 الموظف ليس بالضرورة أن يكون له يوزر. اربطه بيوزر فقط إذا كان يستخدم الكاشير أو شاشات النظام مباشرة.
                </p>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    تاريخ التعيين:
                  </label>
                  <input
                    type="date"
                    value={formHireDate}
                    onChange={(e) => setFormHireDate(e.target.value)}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                  />
                </div>

                <div>
                  <label className="block text-xs font-bold text-slate-700 mb-1">
                    حالة الموظف:
                  </label>
                  <select
                    value={formIsActive ? 'ACTIVE' : 'INACTIVE'}
                    onChange={(e) => setFormIsActive(e.target.value === 'ACTIVE')}
                    className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs font-bold text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                  >
                    <option value="ACTIVE">نشط (على رأس العمل)</option>
                    <option value="INACTIVE">غير نشط (متوقف / منتهي خدمته)</option>
                  </select>
                </div>
              </div>

              <div>
                <label className="block text-xs font-bold text-slate-700 mb-1">
                  ملاحظات إضافية:
                </label>
                <textarea
                  rows={2}
                  value={formNotes}
                  onChange={(e) => setFormNotes(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded-lg px-2.5 py-1.5 text-xs text-slate-800 focus:outline-hidden focus:ring-2 focus:ring-sky-500/30"
                  placeholder="أي ملاحظات عن الموظف أو شروط العمل..."
                />
              </div>

              {/* Modal Actions */}
              <div className="pt-2 border-t border-slate-200 flex items-center justify-end gap-2">
                <button
                  type="button"
                  onClick={() => setIsModalOpen(false)}
                  className="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg font-bold text-xs transition cursor-pointer"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="px-5 py-2 bg-[#0277bd] hover:bg-[#01579b] text-white rounded-lg font-bold text-xs shadow transition flex items-center gap-1 cursor-pointer"
                >
                  <CheckCircle2 className="w-4 h-4" />
                  <span>{editingEmployee ? 'حفظ التعديلات' : 'إضافة الموظف'}</span>
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* 6. Roadmap V2 Modal */}
      {showRoadmapModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-black/60 backdrop-blur-xs animate-in fade-in">
          <div className="bg-white rounded-2xl shadow-2xl border border-slate-200 max-w-lg w-full overflow-hidden flex flex-col text-slate-800 animate-in zoom-in-95 duration-150">
            <div className="bg-gradient-to-r from-purple-800 to-indigo-900 text-white px-4 py-3 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Sparkles className="w-5 h-5 text-amber-300" />
                <h3 className="font-bold text-sm">خارطة طريق ميزات الموظفين (V2.0 - قريباً)</h3>
              </div>
              <button
                type="button"
                onClick={() => setShowRoadmapModal(false)}
                className="text-purple-200 hover:text-white p-1 rounded-full text-xs font-bold cursor-pointer"
              >
                ✕
              </button>
            </div>

            <div className="p-4 space-y-3.5 text-xs text-slate-700 leading-relaxed">
              <div className="bg-purple-50 border border-purple-200 rounded-xl p-3 text-purple-900 font-medium">
                تم اعتماد استراتيجية <strong>«العمل في النور مع السوق الحقيقي»</strong>: إطلاق الإصدار الأول فوراً بالميزات الأساسية الحيوية، وتطوير الميزات التوسعية في الميدان بناءً على طلب التجار الفعلي.
              </div>

              <div className="space-y-2">
                <div className="font-bold text-slate-900 text-sm">الميزات المجدولة للتحديثات القادمة:</div>
                <div className="flex items-start gap-2 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
                  <Clock className="w-4 h-4 text-purple-600 shrink-0 mt-0.5" />
                  <div>
                    <strong className="text-slate-900">نظام الحضور والانصراف وأجهزة البصمة:</strong>
                    <p className="text-slate-500 text-[11px] mt-0.5">ربط أجهزة البصمة البيومترية وحساب ساعات التأخير والغياب آلياً.</p>
                  </div>
                </div>

                <div className="flex items-start gap-2 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
                  <FileText className="w-4 h-4 text-purple-600 shrink-0 mt-0.5" />
                  <div>
                    <strong className="text-slate-900">مسير الرواتب المؤتمت (Automated Payroll):</strong>
                    <p className="text-slate-500 text-[11px] mt-0.5">توليد كشوفات الرواتب الشهرية بضغطة زر مع خصم السلف والتأمينات.</p>
                  </div>
                </div>

                <div className="flex items-start gap-2 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
                  <Coins className="w-4 h-4 text-purple-600 shrink-0 mt-0.5" />
                  <div>
                    <strong className="text-slate-900">شرائح العمولات المتقدمة والتارغت (Target):</strong>
                    <p className="text-slate-500 text-[11px] mt-0.5">عمولات تصاعدية بناءً على تحقيق الأهداف البيعية والتحصيل.</p>
                  </div>
                </div>
              </div>

              <div className="pt-2 flex justify-end">
                <button
                  type="button"
                  onClick={() => setShowRoadmapModal(false)}
                  className="px-5 py-2 bg-purple-800 hover:bg-purple-900 text-white rounded-lg font-bold text-xs shadow transition cursor-pointer"
                >
                  فهمت ذلك
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
