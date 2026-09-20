'use client';

import React, { useState, useEffect, useMemo } from 'react';
import { 
  SystemUser, 
  SystemRole, 
  UserSession, 
  Customer, 
  Supplier 
} from '@/types/pos';
import { 
  Shield, 
  Users, 
  Key, 
  Laptop, 
  Lock, 
  Unlock, 
  RefreshCw, 
  UserPlus, 
  Edit3, 
  Trash2, 
  CheckCircle2, 
  XCircle, 
  AlertTriangle, 
  Power, 
  Building2, 
  Search, 
  ChevronRight,
  Eye,
  Sliders,
  Sparkles,
  LogIn,
  Clock,
  Plus,
  Save,
  RotateCcw,
  Binary,
  CheckSquare,
  Square,
  ShieldCheck,
  ShieldAlert,
  Wrench,
  KeyRound,
  EyeOff,
  Check,
  Info,
  UserCheck
} from 'lucide-react';
import { 
  SalesPermissionFlags,
  InventoryPermissionFlags,
  AccountingPermissionFlags,
  SystemPermissionFlags,
  SALES_PERMISSION_DEFS,
  INVENTORY_PERMISSION_DEFS,
  ACCOUNTING_PERMISSION_DEFS,
  SYSTEM_PERMISSION_DEFS,
  toHexMask,
  toBinaryMask,
  isSuperAdmin,
  PermissionItemDef
} from '@/lib/permissions';

interface SecurityModuleProps {
  currentUser?: SystemUser | null;
  users: SystemUser[];
  roles: SystemRole[];
  sessions: UserSession[];
  onAddUser: (user: SystemUser) => void;
  onUpdateUser: (user: SystemUser) => void;
  onToggleUserActive: (userId: string) => void;
  onResetUserPassword: (userId: string) => void;
  onUnlockUser: (userId: string) => void;
  onRevokeSession: (sessionId: string) => void;
  onUpdateRolePermissions: (roleId: number, permissions: { sales: number; inventory: number; accounting: number; system: number }) => void;
  onAddRole?: (role: SystemRole) => void;
  onTestLoginAsUser?: (user: SystemUser) => void;
  onUpdateCurrentUserPassword?: (userId: string, newPass: string) => void;
}

export const SecurityModule: React.FC<SecurityModuleProps> = ({
  currentUser,
  users,
  roles,
  sessions,
  onAddUser,
  onUpdateUser,
  onToggleUserActive,
  onResetUserPassword,
  onUnlockUser,
  onRevokeSession,
  onUpdateRolePermissions,
  onAddRole,
  onTestLoginAsUser,
  onUpdateCurrentUserPassword,
}) => {
  const [activeTab, setActiveTab] = useState<'USERS' | 'ROLES' | 'SESSIONS' | 'PASSWORD'>('USERS');
  const [searchUserQuery, setSearchUserQuery] = useState<string>('');
  const [selectedUser, setSelectedUser] = useState<SystemUser | null>(users[0] || null);

  // Change Password Form State for Current User
  const [currentPasswordInput, setCurrentPasswordInput] = useState<string>('');
  const [newPasswordInput, setNewPasswordInput] = useState<string>('');
  const [confirmPasswordInput, setConfirmPasswordInput] = useState<string>('');
  const [showCurrentPass, setShowCurrentPass] = useState<boolean>(false);
  const [showNewPass, setShowNewPass] = useState<boolean>(false);
  const [showConfirmPass, setShowConfirmPass] = useState<boolean>(false);
  const [terminateOtherSessions, setTerminateOtherSessions] = useState<boolean>(true);
  const [passwordFeedback, setPasswordFeedback] = useState<{ type: 'success' | 'error'; message: string } | null>(null);
  const [isSubmittingPassword, setIsSubmittingPassword] = useState<boolean>(false);

  // New User Modal State
  const [showNewUserModal, setShowNewUserModal] = useState<boolean>(false);
  const [newUserName, setNewUserName] = useState<string>('');
  const [newFullName, setNewFullName] = useState<string>('');
  const [newRoleId, setNewRoleId] = useState<number>(5); // default cashier
  const [newBranchId, setNewBranchId] = useState<number | null>(1);
  const [newDefaultCashboxCode, setNewDefaultCashboxCode] = useState<string>('');
  const [newDefaultWarehouseId, setNewDefaultWarehouseId] = useState<string>('');

  // Roles & Bitmask Matrix State (Sprint 1.2)
  const [selectedRoleForPerms, setSelectedRoleForPerms] = useState<SystemRole | null>(null);
  const [searchRoleQuery, setSearchRoleQuery] = useState<string>('');
  const [roleDrafts, setRoleDrafts] = useState<Record<number, {
    sales: number;
    inventory: number;
    accounting: number;
    system: number;
  }>>({});
  const [saveSuccessToast, setSaveSuccessToast] = useState<string | null>(null);

  // New Custom Role Modal State
  const [showNewRoleModal, setShowNewRoleModal] = useState<boolean>(false);
  const [newRoleName, setNewRoleName] = useState<string>('');
  const [newRoleDesc, setNewRoleDesc] = useState<string>('');
  const [newRoleBaseTemplate, setNewRoleBaseTemplate] = useState<number>(5); // default cashier template

  // Filtered users
  const filteredUsers = users.filter((u) => 
    u.userName.toLowerCase().includes(searchUserQuery.toLowerCase()) ||
    u.fullName.toLowerCase().includes(searchUserQuery.toLowerCase()) ||
    u.roleName.toLowerCase().includes(searchUserQuery.toLowerCase())
  );

  const handleSaveNewUser = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newUserName.trim() || !newFullName.trim()) {
      alert('يرجى ملء جميع الحقول المطلوبة');
      return;
    }

    if (users.some((u) => u.userName.toLowerCase() === newUserName.trim().toLowerCase())) {
      alert('اسم المستخدم موجود مسبقاً، يرجى اختيار اسم مستخدم آخر.');
      return;
    }

    const selectedRole = roles.find((r) => r.id === newRoleId);

    const newUser: SystemUser = {
      id: `USR-${Date.now().toString().slice(-4)}`,
      userName: newUserName.trim().toLowerCase(),
      fullName: newFullName.trim(),
      roleId: newRoleId,
      roleName: selectedRole ? selectedRole.name : 'مستخدم',
      branchId: newBranchId,
      branchName: newBranchId === null ? 'جميع الفروع (المركز الرئيسي)' : 'المركز الرئيسي (صنعاء)',
      mustChangePassword: true, // Default per Sprint 1 specification
      loginAttempts: 0,
      isLocked: false,
      lockedUntil: null,
      lastLoginAt: null,
      isActive: true,
      defaultCashboxCode: newDefaultCashboxCode || undefined,
      defaultWarehouseId: newDefaultWarehouseId || undefined,
    };

    onAddUser(newUser);
    setShowNewUserModal(false);
    setNewUserName('');
    setNewFullName('');
    setNewDefaultCashboxCode('');
    setNewDefaultWarehouseId('');
    alert(`تم إنشاء المستخدم "${newUser.fullName}" بنجاح.\nكلمة المرور الافتراضية: "1" ومطلوب تغييرها عند أول دخول.`);
  };

  // Currently active role for permissions inspection/editing
  const activeRole = selectedRoleForPerms || roles[0];
  const isSelectedSuperAdmin = activeRole ? isSuperAdmin(activeRole) : false;

  // Toggle single bit in bitmask
  const handleToggleBit = (module: 'sales' | 'inventory' | 'accounting' | 'system', flag: number) => {
    if (!activeRole || isSelectedSuperAdmin) return;
    setRoleDrafts((prev) => {
      const current = prev[activeRole.id] ?? {
        sales: activeRole.salesPermissions,
        inventory: activeRole.inventoryPermissions,
        accounting: activeRole.accountingPermissions,
        system: activeRole.systemPermissions,
      };
      const curVal = current[module];
      const nextVal = (curVal & flag) !== 0 ? (curVal & ~flag) : (curVal | flag);
      return {
        ...prev,
        [activeRole.id]: {
          ...current,
          [module]: nextVal,
        },
      };
    });
  };

  // Set all or clear all bits for a specific module
  const handleSetAllModule = (module: 'sales' | 'inventory' | 'accounting' | 'system', enableAll: boolean) => {
    if (!activeRole || isSelectedSuperAdmin) return;
    let mask = 0;
    if (module === 'sales') mask = 1023; // 10 bits: (1<<10)-1
    if (module === 'inventory') mask = 255;
    if (module === 'accounting') mask = 255;
    if (module === 'system') mask = 255;
    const targetVal = enableAll ? mask : 0;

    setRoleDrafts((prev) => {
      const current = prev[activeRole.id] ?? {
        sales: activeRole.salesPermissions,
        inventory: activeRole.inventoryPermissions,
        accounting: activeRole.accountingPermissions,
        system: activeRole.systemPermissions,
      };
      return {
        ...prev,
        [activeRole.id]: {
          ...current,
          [module]: targetVal,
        },
      };
    });
  };

  // Quick Preset Templates
  const handleApplyPreset = (presetType: 'cashier' | 'accountant' | 'warehouse' | 'branch_manager' | 'observer') => {
    if (!activeRole || isSelectedSuperAdmin) return;
    let presetVals = { sales: 1, inventory: 1, accounting: 0, system: 0 };
    switch (presetType) {
      case 'cashier':
        presetVals = { sales: 119, inventory: 1, accounting: 8, system: 0 };
        break;
      case 'accountant':
        presetVals = { sales: 15, inventory: 7, accounting: 255, system: 3 };
        break;
      case 'warehouse':
        presetVals = { sales: 1, inventory: 255, accounting: 0, system: 0 };
        break;
      case 'branch_manager':
        presetVals = { sales: 251, inventory: 255, accounting: 255, system: 1 };
        break;
      case 'observer':
        presetVals = { sales: 5, inventory: 3, accounting: 35, system: 1 };
        break;
    }
    setRoleDrafts((prev) => ({
      ...prev,
      [activeRole.id]: presetVals,
    }));
  };

  // Save draft permissions to global state
  const handleSaveDraftPermissions = () => {
    if (!activeRole || isSelectedSuperAdmin) return;
    const current = roleDrafts[activeRole.id] ?? {
      sales: activeRole.salesPermissions,
      inventory: activeRole.inventoryPermissions,
      accounting: activeRole.accountingPermissions,
      system: activeRole.systemPermissions,
    };

    onUpdateRolePermissions(activeRole.id, current);
    setRoleDrafts((prev) => {
      const next = { ...prev };
      delete next[activeRole.id];
      return next;
    });
    setSaveSuccessToast(`تم حفظ وتطبيق مصفوفة الصلاحيات للدور "${activeRole.name}" بنجاح والتطبيق فوري.`);
    setTimeout(() => setSaveSuccessToast(null), 3500);
  };

  // Reset draft to current role state
  const handleResetDraft = () => {
    if (!activeRole) return;
    setRoleDrafts((prev) => {
      const next = { ...prev };
      delete next[activeRole.id];
      return next;
    });
  };

  // Create new custom role handler
  const handleCreateNewRole = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newRoleName.trim()) {
      alert('يرجى إدخال اسم الدور الوظيفي');
      return;
    }
    const templateRole = roles.find((r) => r.id === newRoleBaseTemplate) || roles[4];
    const newId = Math.max(...roles.map((r) => r.id), 0) + 1;
    const newRole: SystemRole = {
      id: newId,
      name: newRoleName.trim(),
      description: newRoleDesc.trim() || `دور مخصص مستند إلى ${templateRole?.name}`,
      isSystem: false,
      salesPermissions: templateRole ? templateRole.salesPermissions : 1,
      inventoryPermissions: templateRole ? templateRole.inventoryPermissions : 0,
      accountingPermissions: templateRole ? templateRole.accountingPermissions : 0,
      systemPermissions: templateRole ? templateRole.systemPermissions : 0,
    };
    if (onAddRole) {
      onAddRole(newRole);
    }
    setSelectedRoleForPerms(newRole);
    setShowNewRoleModal(false);
    setNewRoleName('');
    setNewRoleDesc('');
    setSaveSuccessToast(`تم إنشاء الدور الجديد "${newRole.name}" بنجاح. يمكنك الآن تخصيص قناع البتات.`);
    setTimeout(() => setSaveSuccessToast(null), 4000);
  };

  // Helper count active bits
  const countBits = (val: number, maxCount: number): number => {
    if (val === -1) return maxCount;
    let c = 0;
    let n = val >>> 0;
    while (n > 0) {
      if (n & 1) c++;
      n = n >>> 1;
    }
    return c;
  };

  return (
    <div className="flex-1 flex flex-col bg-slate-50 text-slate-800 overflow-hidden font-sans select-none" dir="rtl">
      {/* Top Header of Security Module */}
      <div className="bg-white border-b border-slate-200 px-4 py-2.5 flex flex-wrap items-center justify-between gap-3 shadow-xs">
        <div className="flex items-center gap-2.5">
          <div className="p-2 bg-sky-50 border border-sky-200 rounded-md text-sky-700">
            <Shield className="w-5 h-5" />
          </div>
          <div>
            <h1 className="text-sm font-bold text-slate-900 flex items-center gap-2">
              <span>إدارة الأمان والمستخدمين والصلاحيات (Module 01: Core & Security)</span>
              <span className="text-[10px] bg-slate-100 text-slate-700 px-2 py-0.5 rounded font-mono border border-slate-300">
                RBAC & Bitmask V1.0
              </span>
            </h1>
            <p className="text-[11px] text-slate-500">
              التحكم في المستخدمين، الجلسات المتزامنة، وسياسات القفل وتفويض الصلاحيات
            </p>
          </div>
        </div>

        {/* Security Module Sub-Tabs */}
        <div className="flex items-center gap-1 bg-slate-100 p-1 rounded-md border border-slate-200">
          <button
            onClick={() => setActiveTab('USERS')}
            className={`px-3 py-1 rounded text-xs font-semibold transition cursor-pointer flex items-center gap-1.5 ${
              activeTab === 'USERS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'text-slate-700 hover:bg-slate-200 hover:text-slate-900'
            }`}
          >
            <Users className="w-3.5 h-3.5" />
            <span>المستخدمين ({users.length})</span>
          </button>

          <button
            onClick={() => setActiveTab('ROLES')}
            className={`px-3 py-1 rounded text-xs font-semibold transition cursor-pointer flex items-center gap-1.5 ${
              activeTab === 'ROLES'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'text-slate-700 hover:bg-slate-200 hover:text-slate-900'
            }`}
          >
            <Key className="w-3.5 h-3.5" />
            <span>الأدوار والصلاحيات ({roles.length})</span>
          </button>

          <button
            onClick={() => setActiveTab('SESSIONS')}
            className={`px-3 py-1 rounded text-xs font-semibold transition cursor-pointer flex items-center gap-1.5 ${
              activeTab === 'SESSIONS'
                ? 'bg-sky-800 text-white shadow-xs'
                : 'text-slate-700 hover:bg-slate-200 hover:text-slate-900'
            }`}
          >
            <Laptop className="w-3.5 h-3.5" />
            <span>الجلسات النشطة ({sessions.filter((s) => !s.isRevoked).length})</span>
          </button>

          <button
            onClick={() => setActiveTab('PASSWORD')}
            className={`px-3 py-1 rounded text-xs font-semibold transition cursor-pointer flex items-center gap-1.5 ${
              activeTab === 'PASSWORD'
                ? 'bg-amber-600 text-white shadow-xs ring-1 ring-amber-400/50'
                : 'text-slate-700 hover:bg-slate-200 hover:text-slate-900'
            }`}
          >
            <KeyRound className="w-3.5 h-3.5 text-amber-300" />
            <span>تغيير كلمة المرور {currentUser ? `(${currentUser.userName})` : ''}</span>
          </button>
        </div>
      </div>

      {/* TAB 1: USERS MANAGEMENT */}
      {activeTab === 'USERS' && (
        <div className="flex-1 flex overflow-hidden p-3 gap-3">
          {/* Right Panel: Users List (DataGridView Simulation) */}
          <div className="flex-1 bg-white border border-slate-200 rounded-md flex flex-col overflow-hidden shadow-xs">
            {/* Toolbar */}
            <div className="p-2.5 bg-slate-50 border-b border-slate-200 flex items-center justify-between gap-3">
              <div className="relative flex-1 max-w-sm">
                <Search className="w-4 h-4 absolute right-2.5 top-2.5 text-slate-400" />
                <input
                  type="text"
                  placeholder="بحث باسم المستخدم أو الاسم الكامل أو الدور..."
                  value={searchUserQuery}
                  onChange={(e) => setSearchUserQuery(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded pr-9 pl-3 py-1.5 text-xs text-slate-800 placeholder-slate-400 focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                />
              </div>

              <button
                onClick={() => setShowNewUserModal(true)}
                className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold px-3 py-1.5 rounded flex items-center gap-1.5 transition shadow-xs cursor-pointer"
              >
                <UserPlus className="w-4 h-4" />
                <span>إضافة مستخدم جديد</span>
              </button>
            </div>

            {/* Grid Table */}
            <div className="flex-1 overflow-auto">
              <table className="w-full text-right text-xs border-collapse font-sans">
                <thead className="bg-slate-100 text-slate-700 border-b border-slate-200 sticky top-0 z-10 font-bold text-[11px]">
                  <tr>
                    <th className="p-2.5">اسم المستخدم</th>
                    <th className="p-2.5">الاسم الكامل</th>
                    <th className="p-2.5">الدور الوظيفي</th>
                    <th className="p-2.5">الفرع المخصص</th>
                    <th className="p-2.5 text-center">حالة الحساب</th>
                    <th className="p-2.5 text-center">حالة القفل</th>
                    <th className="p-2.5">آخر تسجيل دخول</th>
                    <th className="p-2.5 text-center">الإجراءات السريعة</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200 text-slate-700">
                  {filteredUsers.map((u) => {
                    const isSelected = selectedUser?.id === u.id;
                    const isSuperAdmin = u.roleId === 1 || u.userName === 'admin';

                    return (
                      <tr
                        key={u.id}
                        onClick={() => setSelectedUser(u)}
                        className={`hover:bg-slate-50 transition cursor-pointer ${
                          isSelected ? 'bg-sky-50 ring-1 ring-sky-300 font-medium' : ''
                        }`}
                      >
                        <td className="p-2.5 font-mono text-sky-700 font-bold flex items-center gap-1.5">
                          {isSuperAdmin && <Shield className="w-3.5 h-3.5 text-amber-500" />}
                          <span>{u.userName}</span>
                        </td>
                        <td className="p-2.5 text-slate-900 font-medium">{u.fullName}</td>
                        <td className="p-2.5">
                          <span className={`px-2 py-0.5 rounded text-[11px] font-semibold ${
                            u.roleId === 1 
                              ? 'bg-purple-100 text-purple-800 border border-purple-200' 
                              : u.roleId === 3 
                              ? 'bg-sky-100 text-sky-800 border border-sky-200'
                              : 'bg-emerald-100 text-emerald-800 border border-emerald-200'
                          }`}>
                            {u.roleName}
                          </span>
                        </td>
                        <td className="p-2.5 text-slate-600 text-[11px]">{u.branchName}</td>
                        <td className="p-2.5 text-center">
                          {u.isActive ? (
                            <span className="text-emerald-700 font-semibold flex items-center justify-center gap-1">
                              <CheckCircle2 className="w-3.5 h-3.5" /> نشط
                            </span>
                          ) : (
                            <span className="text-rose-700 font-semibold flex items-center justify-center gap-1">
                              <XCircle className="w-3.5 h-3.5" /> معطل
                            </span>
                          )}
                        </td>
                        <td className="p-2.5 text-center">
                          {u.isLocked ? (
                            <span className="bg-rose-100 text-rose-800 border border-rose-200 px-2 py-0.5 rounded text-[10px] font-bold flex items-center justify-center gap-1">
                              <Lock className="w-3 h-3 text-rose-600" /> مقفل ({u.loginAttempts} محاولات)
                            </span>
                          ) : (
                            <span className="text-slate-500 text-[11px]">طبيعي</span>
                          )}
                        </td>
                        <td className="p-2.5 text-slate-500 text-[11px]">
                          {u.lastLoginAt || 'لم يسجل دخول بعد'}
                        </td>
                        <td className="p-2.5 text-center" onClick={(e) => e.stopPropagation()}>
                          <div className="flex items-center justify-center gap-1">
                            {/* Unlock button */}
                            {u.isLocked && (
                              <button
                                onClick={() => {
                                  onUnlockUser(u.id);
                                  alert(`تم فك قفل حساب المستخدم "${u.userName}" فورياً وتصفير محاولات الدخول.`);
                                }}
                                className="bg-amber-600 hover:bg-amber-500 text-white px-2 py-1 rounded text-[11px] font-bold flex items-center gap-1 cursor-pointer"
                                title="فك القفل اليدوي الفوري"
                              >
                                <Unlock className="w-3 h-3" /> فك القفل
                              </button>
                            )}

                            {/* Reset Password */}
                            <button
                              onClick={() => {
                                if (confirm(`هل أنت متأكد من إعادة تعيين كلمة مرور "${u.userName}" إلى "1" الافتراضية؟`)) {
                                  onResetUserPassword(u.id);
                                  alert(`تمت إعادة تعيين كلمة المرور للمستخدم "${u.userName}" إلى "1" بنجاح.\nسيُطلب منه تغييرها عند أول تسجيل دخول.`);
                                }
                              }}
                              className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 px-2 py-1 rounded text-[11px] font-medium transition cursor-pointer"
                              title="إعادة تعيين كلمة المرور إلى 1 الافتراضية"
                            >
                              <RefreshCw className="w-3 h-3" /> Reset
                            </button>

                            {/* Toggle Active */}
                            {!isSuperAdmin && (
                              <button
                                onClick={() => {
                                  onToggleUserActive(u.id);
                                }}
                                className={`px-2 py-1 rounded text-[11px] font-medium transition cursor-pointer border ${
                                  u.isActive 
                                    ? 'bg-rose-50 hover:bg-rose-100 text-rose-700 border-rose-200' 
                                    : 'bg-emerald-50 hover:bg-emerald-100 text-emerald-700 border-emerald-200'
                                }`}
                                title={u.isActive ? 'تعطيل الحساب' : 'تفعيل الحساب'}
                              >
                                {u.isActive ? 'تعطيل' : 'تفعيل'}
                              </button>
                            )}
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>

          {/* Left Panel: Selected User Detail Card */}
          {selectedUser && (
            <div className="w-80 bg-white border border-slate-200 rounded-md p-3 flex flex-col justify-between shadow-xs">
              <div>
                <div className="flex items-center gap-2 pb-2 mb-3 border-b border-slate-200">
                  <div className="p-2 bg-sky-50 border border-sky-200 rounded text-sky-700">
                    <Users className="w-5 h-5" />
                  </div>
                  <div>
                    <h3 className="font-bold text-slate-900 text-xs">{selectedUser.fullName}</h3>
                    <p className="text-[11px] text-sky-700 font-mono">@{selectedUser.userName}</p>
                  </div>
                </div>

                <div className="space-y-2 text-xs">
                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">الدور الوظيفي:</span>
                    <span className="font-bold text-slate-800">{selectedUser.roleName}</span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">الفرع المعتمد:</span>
                    <span className="font-medium text-slate-800">{selectedUser.branchName}</span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">الصندوق الافتراضي:</span>
                    <span className="font-medium text-emerald-700">
                      {selectedUser.defaultCashboxCode === '1111' 
                        ? 'الصندوق الرئيسي (1111)' 
                        : selectedUser.defaultCashboxCode === '1112'
                        ? 'صندوق كاشير 1 (1112)'
                        : selectedUser.defaultCashboxCode === '1113'
                        ? 'صندوق كاشير 2 (1113)'
                        : 'غير معين (يدوي)'}
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">المستودع الافتراضي:</span>
                    <span className="font-medium text-sky-700">
                      {selectedUser.defaultWarehouseId === 'WH-01' 
                        ? 'المستودع الرئيسي - صنعاء' 
                        : selectedUser.defaultWarehouseId === 'WH-02'
                        ? 'مستودع فرع حدة'
                        : 'غير معين (يدوي)'}
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">حالة القفل:</span>
                    <span className={selectedUser.isLocked ? 'text-rose-700 font-bold' : 'text-emerald-700 font-medium'}>
                      {selectedUser.isLocked ? `مقفل (${selectedUser.loginAttempts}/5 محاولات)` : 'مفتوح (طبيعي)'}
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">تغيير كلمة المرور:</span>
                    <span className={selectedUser.mustChangePassword ? 'text-amber-700 font-bold' : 'text-slate-500'}>
                      {selectedUser.mustChangePassword ? 'إلزامي عند أول دخول' : 'غير مطلوب'}
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2 rounded border border-slate-200 flex justify-between">
                    <span className="text-slate-500">آخر ظهور:</span>
                    <span className="font-mono text-slate-700 text-[11px]">{selectedUser.lastLoginAt || 'لا يوجد'}</span>
                  </div>
                </div>

                {/* Password Policy & Rules Note */}
                <div className="mt-4 p-2.5 bg-sky-50/70 border border-sky-200 rounded text-[11px] text-slate-700 space-y-1">
                  <p className="font-bold text-sky-800 flex items-center gap-1">
                    <Sparkles className="w-3 h-3 text-sky-600" /> سياسة الأمان المعتمدة (Module 01):
                  </p>
                  <p>• كلمة المرور الافتراضية للمستخدم الجديد أو بعد إعادة التعيين هي: <strong className="text-slate-900">1</strong>.</p>
                  <p>• القفل التلقائي بعد 5 محاولات خاطئة لمدة 30 دقيقة.</p>
                  <p>• فك القفل اليدوي متاح فقط للمدير العام.</p>
                </div>
              </div>

              {/* Bottom Quick Action */}
              <div className="pt-3 border-t border-slate-200 flex flex-col gap-1.5">
                {onTestLoginAsUser && (
                  <button
                    onClick={() => onTestLoginAsUser(selectedUser)}
                    className="w-full bg-sky-700 hover:bg-sky-600 text-white font-bold py-1.5 rounded text-xs transition flex items-center justify-center gap-1.5 cursor-pointer shadow-xs"
                    title="فتح شاشة تسجيل الدخول ومحاكاة الدخول بهذا الحساب"
                  >
                    <LogIn className="w-4 h-4" /> محاكاة الدخول بهذا المستخدم
                  </button>
                )}

                {selectedUser.isLocked && (
                  <button
                    onClick={() => {
                      onUnlockUser(selectedUser.id);
                      alert(`تم فك قفل الحساب بنجاح.`);
                    }}
                    className="w-full bg-amber-600 hover:bg-amber-500 text-white font-bold py-1.5 rounded text-xs transition flex items-center justify-center gap-1.5 cursor-pointer"
                  >
                    <Unlock className="w-4 h-4" /> فك القفل اليدوي فوراً
                  </button>
                )}

                {currentUser && (
                  <button
                    onClick={() => setActiveTab('PASSWORD')}
                    className="w-full bg-amber-600 hover:bg-amber-500 text-white font-bold py-1.5 rounded text-xs transition flex items-center justify-center gap-1.5 cursor-pointer shadow-xs"
                  >
                    <KeyRound className="w-4 h-4 text-amber-200" />
                    <span>تغيير كلمة المرور لحسابي الحالي ({currentUser.userName})</span>
                  </button>
                )}

                <button
                  onClick={() => {
                    if (confirm(`إعادة تعيين كلمة مرور "${selectedUser.userName}"؟`)) {
                      onResetUserPassword(selectedUser.id);
                      alert(`تمت إعادة التعيين إلى "1" بنجاح.`);
                    }
                  }}
                  className="w-full bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 font-semibold py-1.5 rounded text-xs transition flex items-center justify-center gap-1.5 cursor-pointer"
                >
                  <RefreshCw className="w-4 h-4" /> إعادة تعيين كلمة المرور (Reset to 1)
                </button>
              </div>
            </div>
          )}
        </div>
      )}

      {/* TAB 2: ROLES & PERMISSIONS MATRIX */}
      {activeTab === 'ROLES' && (
        <div className="flex-1 flex overflow-hidden p-3 gap-3">
          {/* Roles List (Left Sidebar) */}
          <div className="w-80 bg-white border border-slate-200 rounded-md flex flex-col overflow-hidden shadow-xs shrink-0">
            <div className="p-2.5 bg-slate-50 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
              <span className="flex items-center gap-1.5">
                <Shield className="w-4 h-4 text-sky-700" />
                <span>قائمة الأدوار المعتمدة ({roles.length})</span>
              </span>
              <button
                type="button"
                onClick={() => setShowNewRoleModal(true)}
                className="bg-sky-700 hover:bg-sky-600 text-white text-[11px] font-bold px-2 py-0.5 rounded flex items-center gap-1 transition cursor-pointer"
                title="إضافة دور وظيفي مخصص"
              >
                <Plus className="w-3.5 h-3.5" /> دور جديد
              </button>
            </div>

            {/* Search roles */}
            <div className="p-2 border-b border-slate-200 bg-slate-50">
              <div className="relative">
                <Search className="w-3.5 h-3.5 absolute right-2.5 top-2.5 text-slate-400" />
                <input
                  type="text"
                  value={searchRoleQuery}
                  onChange={(e) => setSearchRoleQuery(e.target.value)}
                  placeholder="بحث في مسميات الأدوار..."
                  className="w-full bg-white border border-slate-300 rounded text-xs pr-8 pl-2 py-1.5 text-slate-800 placeholder-slate-400 focus:outline-hidden focus:border-sky-500"
                />
              </div>
            </div>

            <div className="flex-1 overflow-auto divide-y divide-slate-200">
              {roles
                .filter((r) => r.name.toLowerCase().includes(searchRoleQuery.toLowerCase()) || r.description.toLowerCase().includes(searchRoleQuery.toLowerCase()))
                .map((r) => {
                  const isSelected = activeRole?.id === r.id;
                  const isSuper = isSuperAdmin(r);
                  const salesCount = countBits(r.salesPermissions, 10);
                  const invCount = countBits(r.inventoryPermissions, 8);
                  const accCount = countBits(r.accountingPermissions, 8);
                  const sysCount = countBits(r.systemPermissions, 8);

                  return (
                    <button
                      key={r.id}
                      onClick={() => setSelectedRoleForPerms(r)}
                      className={`w-full text-right p-2.5 transition flex flex-col gap-1 cursor-pointer ${
                        isSelected ? 'bg-sky-50 text-slate-900 ring-1 ring-sky-300' : 'hover:bg-slate-50 text-slate-700'
                      }`}
                    >
                      <div className="flex items-center justify-between">
                        <span className="font-bold text-xs flex items-center gap-1.5">
                          {isSuper ? <Shield className="w-3.5 h-3.5 text-amber-500" /> : <Key className="w-3.5 h-3.5 text-sky-700" />}
                          {r.name}
                        </span>
                        <div className="flex items-center gap-1">
                          <span className="text-[10px] font-mono bg-slate-100 text-slate-700 px-1 py-0.2 rounded border border-slate-300">
                            #{r.id}
                          </span>
                          {r.isSystem ? (
                            <span className="text-[9px] bg-slate-100 text-slate-600 px-1.5 py-0.2 rounded border border-slate-300">
                              نظامي
                            </span>
                          ) : (
                            <span className="text-[9px] bg-emerald-50 text-emerald-700 px-1.5 py-0.2 rounded border border-emerald-200">
                              مخصص
                            </span>
                          )}
                        </div>
                      </div>
                      <p className="text-[11px] text-slate-500 line-clamp-1">{r.description}</p>
                      
                      {/* Mini Bit Indicators */}
                      <div className="flex items-center gap-1 text-[9px] font-mono mt-1 text-slate-500 flex-wrap">
                        {isSuper ? (
                          <span className="text-amber-700 font-bold">كامل الصلاحيات (-1L Bypass)</span>
                        ) : (
                          <>
                            <span className="bg-slate-100 px-1 rounded border border-slate-200 text-sky-700">
                              بيع: {salesCount}/10
                            </span>
                            <span className="bg-slate-100 px-1 rounded border border-slate-200 text-amber-700">
                              مخزن: {invCount}/8
                            </span>
                            <span className="bg-slate-100 px-1 rounded border border-slate-200 text-emerald-700">
                              حساب: {accCount}/8
                            </span>
                            <span className="bg-slate-100 px-1 rounded border border-slate-200 text-purple-700">
                              نظام: {sysCount}/8
                            </span>
                          </>
                        )}
                      </div>
                    </button>
                  );
                })}
            </div>
          </div>

          {/* Permissions Matrix Detail (Right Panel) */}
          {(() => {
            if (!activeRole) return null;
            const isSuper = isSuperAdmin(activeRole);
            const activeDraft = roleDrafts[activeRole.id];
            const currentSales = activeDraft?.sales ?? activeRole.salesPermissions;
            const currentInv = activeDraft?.inventory ?? activeRole.inventoryPermissions;
            const currentAcc = activeDraft?.accounting ?? activeRole.accountingPermissions;
            const currentSys = activeDraft?.system ?? activeRole.systemPermissions;
            const hasUnsavedChanges = !!activeDraft && (
              activeDraft.sales !== activeRole.salesPermissions ||
              activeDraft.inventory !== activeRole.inventoryPermissions ||
              activeDraft.accounting !== activeRole.accountingPermissions ||
              activeDraft.system !== activeRole.systemPermissions
            );

            return (
              <div className="flex-1 bg-white border border-slate-200 rounded-md p-3 flex flex-col overflow-hidden shadow-xs">
                {/* Header of Active Role */}
                <div className="pb-2.5 mb-2.5 border-b border-slate-200 flex flex-wrap items-center justify-between gap-2">
                  <div>
                    <div className="flex items-center gap-2">
                      <h2 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                        <span>مصفوفة صلاحيات: <strong className="text-sky-800">{activeRole.name}</strong></span>
                        <span className="text-xs font-mono text-slate-500">ID: {activeRole.id}</span>
                      </h2>
                      {isSuper ? (
                        <span className="bg-purple-50 text-purple-700 text-[10px] px-2 py-0.5 rounded border border-purple-200 font-bold flex items-center gap-1">
                          <ShieldAlert className="w-3 h-3 text-amber-600" />
                          مدير النظام الأعلى (-1L Bypass All)
                        </span>
                      ) : hasUnsavedChanges ? (
                        <span className="bg-amber-50 text-amber-800 text-[10px] px-2 py-0.5 rounded border border-amber-300 font-bold animate-pulse">
                          يوجد تعديلات غير محفوظة
                        </span>
                      ) : (
                        <span className="bg-emerald-50 text-emerald-700 text-[10px] px-2 py-0.5 rounded border border-emerald-200 font-bold">
                          الصلاحيات متزامنة
                        </span>
                      )}
                    </div>
                    <p className="text-[11px] text-slate-500 mt-0.5">{activeRole.description}</p>
                  </div>

                  {/* Actions & Presets */}
                  <div className="flex items-center gap-2">
                    {!isSuper && (
                      <>
                        {/* Quick Presets Dropdown */}
                        <div className="flex items-center gap-1 text-xs">
                          <span className="text-[11px] text-slate-500">قالب سريع:</span>
                          <select
                            onChange={(e) => {
                              if (e.target.value) {
                                handleApplyPreset(e.target.value as any);
                                e.target.value = '';
                              }
                            }}
                            defaultValue=""
                            className="bg-white border border-slate-300 text-xs rounded px-2 py-1 text-slate-800 focus:outline-hidden cursor-pointer"
                          >
                            <option value="" disabled>اختر قالباً لتطبيقه...</option>
                            <option value="cashier">كاشير مبيعات (POS Standard)</option>
                            <option value="accountant">محاسب مالي معتمد</option>
                            <option value="warehouse">مشرف مستودعات ومخزون</option>
                            <option value="branch_manager">مدير فرع تشغيلي</option>
                            <option value="observer">مراقب وتدقيق (قراءة فقط)</option>
                          </select>
                        </div>

                        {/* Reset button */}
                        {hasUnsavedChanges && (
                          <button
                            type="button"
                            onClick={handleResetDraft}
                            className="bg-slate-100 hover:bg-slate-200 text-slate-700 border border-slate-300 text-xs font-semibold px-2.5 py-1 rounded flex items-center gap-1 transition cursor-pointer"
                            title="إلغاء التعديلات غير المحفوظة"
                          >
                            <RotateCcw className="w-3.5 h-3.5" /> تراجع
                          </button>
                        )}

                        {/* Save Button */}
                        <button
                          type="button"
                          onClick={handleSaveDraftPermissions}
                          className="bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-bold px-3.5 py-1 rounded shadow-xs flex items-center gap-1.5 transition cursor-pointer"
                        >
                          <Save className="w-3.5 h-3.5" /> حفظ مصفوفة الصلاحيات
                        </button>
                      </>
                    )}
                  </div>
                </div>

                {/* Toast Notification */}
                {saveSuccessToast && (
                  <div className="mb-2 bg-emerald-50 border border-emerald-200 text-emerald-800 text-xs px-3 py-1.5 rounded flex items-center gap-2 shadow-xs">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                    <span>{saveSuccessToast}</span>
                  </div>
                )}

                {/* Super Admin Notice Banner */}
                {isSuper && (
                  <div className="mb-3 p-2.5 bg-purple-50 border border-purple-200 rounded text-xs text-purple-900 flex items-start gap-2.5 shadow-xs">
                    <ShieldCheck className="w-5 h-5 text-amber-600 shrink-0 mt-0.5" />
                    <div>
                      <h4 className="font-bold text-purple-900">صلاحيات مطلقة غير مقيدة (Super Administrator Kernel Bypass)</h4>
                      <p className="text-[11px] text-purple-800 mt-0.5 leading-relaxed">
                        يمتلك هذا الدور القيمة المرجعية <code>-1L (0xFFFFFFFF)</code>، ويتم تطبيق الـ Bypass التلقائي في كافة دوال التحقق البرمجية <code>hasPermission</code>. لا يمكن تعديل بتات هذا الدور أو تعطيلها لحماية الوصول الإداري واستقرار النظام.
                      </p>
                    </div>
                  </div>
                )}

                {/* Permissions Tree Sections (4 Modules) */}
                <div className="flex-1 overflow-auto space-y-3 pr-1">
                  {/* Module 1: Sales Permissions (10 Bits) */}
                  <div className="bg-slate-50 border border-slate-200 rounded p-2.5">
                    <div className="flex flex-wrap items-center justify-between pb-2 mb-2 border-b border-slate-200 gap-2">
                      <div className="flex items-center gap-2">
                        <Sliders className="w-4 h-4 text-sky-700" />
                        <span className="text-xs font-bold text-slate-900">
                          صلاحيات المبيعات ونقاط البيع (SalesPermissions Bitmask)
                        </span>
                        <span className="text-[10px] font-mono bg-white text-sky-800 px-1.5 py-0.5 rounded border border-sky-200">
                          Hex: {toHexMask(currentSales)}
                        </span>
                        <span className="text-[10px] font-mono bg-white text-sky-800 px-1.5 py-0.5 rounded border border-sky-200 hidden sm:inline">
                          Bin: {toBinaryMask(currentSales, 10)}
                        </span>
                        <span className="text-[10px] text-slate-600 font-bold">
                          ({countBits(currentSales, 10)} / 10 مفعلة)
                        </span>
                      </div>
                      {!isSuper && (
                        <div className="flex items-center gap-1">
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('sales', true)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-sky-800 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            تحديد الكل
                          </button>
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('sales', false)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            إلغاء الكل
                          </button>
                        </div>
                      )}
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-2 text-xs">
                      {SALES_PERMISSION_DEFS.map((item) => {
                        const isChecked = isSuper || (currentSales & item.flag) !== 0;
                        return (
                          <div
                            key={item.flag}
                            onClick={() => !isSuper && handleToggleBit('sales', item.flag)}
                            className={`p-2 rounded border transition cursor-pointer flex items-start gap-2 ${
                              isChecked
                                ? 'bg-sky-50/80 border-sky-300 text-slate-900'
                                : 'bg-white border-slate-200 text-slate-600 hover:border-slate-300'
                            } ${isSuper ? 'cursor-default opacity-90' : ''}`}
                          >
                            <input
                              type="checkbox"
                              checked={isChecked}
                              disabled={isSuper}
                              onChange={() => {}}
                              className="mt-0.5 rounded text-sky-700 focus:ring-0 cursor-pointer"
                            />
                            <div className="flex-1">
                              <div className="flex items-center justify-between">
                                <span className="font-bold text-[11px] text-slate-900">{item.labelAr}</span>
                                <span className="text-[9px] font-mono text-slate-500">+{item.flag}</span>
                              </div>
                              <span className="text-[10px] font-mono text-sky-700 block">{item.labelEn}</span>
                              <p className="text-[10px] text-slate-500 mt-0.5 leading-relaxed">{item.descAr}</p>
                              {item.isSensitive && (
                                <span className="inline-block mt-1 text-[9px] bg-amber-50 text-amber-800 px-1 rounded border border-amber-200 font-bold">
                                  عملية حساسة
                                </span>
                              )}
                            </div>
                          </div>
                        );
                      })}
                    </div>
                  </div>

                  {/* Module 2: Inventory Permissions (8 Bits) */}
                  <div className="bg-slate-50 border border-slate-200 rounded p-2.5">
                    <div className="flex flex-wrap items-center justify-between pb-2 mb-2 border-b border-slate-200 gap-2">
                      <div className="flex items-center gap-2">
                        <Building2 className="w-4 h-4 text-amber-600" />
                        <span className="text-xs font-bold text-slate-900">
                          صلاحيات المخازن والأصناف (InventoryPermissions Bitmask)
                        </span>
                        <span className="text-[10px] font-mono bg-white text-amber-800 px-1.5 py-0.5 rounded border border-amber-200">
                          Hex: {toHexMask(currentInv)}
                        </span>
                        <span className="text-[10px] font-mono bg-white text-amber-800 px-1.5 py-0.5 rounded border border-amber-200 hidden sm:inline">
                          Bin: {toBinaryMask(currentInv, 8)}
                        </span>
                        <span className="text-[10px] text-slate-600 font-bold">
                          ({countBits(currentInv, 8)} / 8 مفعلة)
                        </span>
                      </div>
                      {!isSuper && (
                        <div className="flex items-center gap-1">
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('inventory', true)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-amber-800 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            تحديد الكل
                          </button>
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('inventory', false)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            إلغاء الكل
                          </button>
                        </div>
                      )}
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-2 text-xs">
                      {INVENTORY_PERMISSION_DEFS.map((item) => {
                        const isChecked = isSuper || (currentInv & item.flag) !== 0;
                        return (
                          <div
                            key={item.flag}
                            onClick={() => !isSuper && handleToggleBit('inventory', item.flag)}
                            className={`p-2 rounded border transition cursor-pointer flex items-start gap-2 ${
                              isChecked
                                ? 'bg-amber-50/80 border-amber-300 text-slate-900'
                                : 'bg-white border-slate-200 text-slate-600 hover:border-slate-300'
                            } ${isSuper ? 'cursor-default opacity-90' : ''}`}
                          >
                            <input
                              type="checkbox"
                              checked={isChecked}
                              disabled={isSuper}
                              onChange={() => {}}
                              className="mt-0.5 rounded text-amber-600 focus:ring-0 cursor-pointer"
                            />
                            <div className="flex-1">
                              <div className="flex items-center justify-between">
                                <span className="font-bold text-[11px] text-slate-900">{item.labelAr}</span>
                                <span className="text-[9px] font-mono text-slate-500">+{item.flag}</span>
                              </div>
                              <span className="text-[10px] font-mono text-amber-700 block">{item.labelEn}</span>
                              <p className="text-[10px] text-slate-500 mt-0.5 leading-relaxed">{item.descAr}</p>
                              {item.isSensitive && (
                                <span className="inline-block mt-1 text-[9px] bg-amber-50 text-amber-800 px-1 rounded border border-amber-200 font-bold">
                                  عملية حساسة
                                </span>
                              )}
                            </div>
                          </div>
                        );
                      })}
                    </div>
                  </div>

                  {/* Module 3: Accounting Permissions (8 Bits) */}
                  <div className="bg-slate-50 border border-slate-200 rounded p-2.5">
                    <div className="flex flex-wrap items-center justify-between pb-2 mb-2 border-b border-slate-200 gap-2">
                      <div className="flex items-center gap-2">
                        <Key className="w-4 h-4 text-emerald-700" />
                        <span className="text-xs font-bold text-slate-900">
                          صلاحيات الحسابات والقيود والدفاتر (AccountingPermissions Bitmask)
                        </span>
                        <span className="text-[10px] font-mono bg-white text-emerald-800 px-1.5 py-0.5 rounded border border-emerald-200">
                          Hex: {toHexMask(currentAcc)}
                        </span>
                        <span className="text-[10px] font-mono bg-white text-emerald-800 px-1.5 py-0.5 rounded border border-emerald-200 hidden sm:inline">
                          Bin: {toBinaryMask(currentAcc, 8)}
                        </span>
                        <span className="text-[10px] text-slate-600 font-bold">
                          ({countBits(currentAcc, 8)} / 8 مفعلة)
                        </span>
                      </div>
                      {!isSuper && (
                        <div className="flex items-center gap-1">
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('accounting', true)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-emerald-800 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            تحديد الكل
                          </button>
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('accounting', false)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            إلغاء الكل
                          </button>
                        </div>
                      )}
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-2 text-xs">
                      {ACCOUNTING_PERMISSION_DEFS.map((item) => {
                        const isChecked = isSuper || (currentAcc & item.flag) !== 0;
                        return (
                          <div
                            key={item.flag}
                            onClick={() => !isSuper && handleToggleBit('accounting', item.flag)}
                            className={`p-2 rounded border transition cursor-pointer flex items-start gap-2 ${
                              isChecked
                                ? 'bg-emerald-50/80 border-emerald-300 text-slate-900'
                                : 'bg-white border-slate-200 text-slate-600 hover:border-slate-300'
                            } ${isSuper ? 'cursor-default opacity-90' : ''}`}
                          >
                            <input
                              type="checkbox"
                              checked={isChecked}
                              disabled={isSuper}
                              onChange={() => {}}
                              className="mt-0.5 rounded text-emerald-700 focus:ring-0 cursor-pointer"
                            />
                            <div className="flex-1">
                              <div className="flex items-center justify-between">
                                <span className="font-bold text-[11px] text-slate-900">{item.labelAr}</span>
                                <span className="text-[9px] font-mono text-slate-500">+{item.flag}</span>
                              </div>
                              <span className="text-[10px] font-mono text-emerald-700 block">{item.labelEn}</span>
                              <p className="text-[10px] text-slate-500 mt-0.5 leading-relaxed">{item.descAr}</p>
                              {item.isSensitive && (
                                <span className="inline-block mt-1 text-[9px] bg-amber-50 text-amber-800 px-1 rounded border border-amber-200 font-bold">
                                  عملية حساسة
                                </span>
                              )}
                            </div>
                          </div>
                        );
                      })}
                    </div>
                  </div>

                  {/* Module 4: System & Security Permissions (8 Bits) */}
                  <div className="bg-slate-50 border border-slate-200 rounded p-2.5">
                    <div className="flex flex-wrap items-center justify-between pb-2 mb-2 border-b border-slate-200 gap-2">
                      <div className="flex items-center gap-2">
                        <Shield className="w-4 h-4 text-purple-700" />
                        <span className="text-xs font-bold text-slate-900">
                          صلاحيات النظام والأمان والنسخ الاحتياطي (SystemPermissions Bitmask)
                        </span>
                        <span className="text-[10px] font-mono bg-white text-purple-800 px-1.5 py-0.5 rounded border border-purple-200">
                          Hex: {toHexMask(currentSys)}
                        </span>
                        <span className="text-[10px] font-mono bg-white text-purple-800 px-1.5 py-0.5 rounded border border-purple-200 hidden sm:inline">
                          Bin: {toBinaryMask(currentSys, 8)}
                        </span>
                        <span className="text-[10px] text-slate-600 font-bold">
                          ({countBits(currentSys, 8)} / 8 مفعلة)
                        </span>
                      </div>
                      {!isSuper && (
                        <div className="flex items-center gap-1">
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('system', true)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-purple-800 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            تحديد الكل
                          </button>
                          <button
                            type="button"
                            onClick={() => handleSetAllModule('system', false)}
                            className="text-[10px] bg-white hover:bg-slate-100 text-slate-700 px-2 py-0.5 rounded border border-slate-300 cursor-pointer font-medium"
                          >
                            إلغاء الكل
                          </button>
                        </div>
                      )}
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-2 text-xs">
                      {SYSTEM_PERMISSION_DEFS.map((item) => {
                        const isChecked = isSuper || (currentSys & item.flag) !== 0;
                        return (
                          <div
                            key={item.flag}
                            onClick={() => !isSuper && handleToggleBit('system', item.flag)}
                            className={`p-2 rounded border transition cursor-pointer flex items-start gap-2 ${
                              isChecked
                                ? 'bg-purple-50/80 border-purple-300 text-slate-900'
                                : 'bg-white border-slate-200 text-slate-600 hover:border-slate-300'
                            } ${isSuper ? 'cursor-default opacity-90' : ''}`}
                          >
                            <input
                              type="checkbox"
                              checked={isChecked}
                              disabled={isSuper}
                              onChange={() => {}}
                              className="mt-0.5 rounded text-purple-700 focus:ring-0 cursor-pointer"
                            />
                            <div className="flex-1">
                              <div className="flex items-center justify-between">
                                <span className="font-bold text-[11px] text-slate-900">{item.labelAr}</span>
                                <span className="text-[9px] font-mono text-slate-500">+{item.flag}</span>
                              </div>
                              <span className="text-[10px] font-mono text-purple-700 block">{item.labelEn}</span>
                              <p className="text-[10px] text-slate-500 mt-0.5 leading-relaxed">{item.descAr}</p>
                              {item.isSensitive && (
                                <span className="inline-block mt-1 text-[9px] bg-rose-50 text-rose-800 px-1 rounded border border-rose-200 font-bold">
                                  صلاحية إدارية حرجة
                                </span>
                              )}
                            </div>
                          </div>
                        );
                      })}
                    </div>
                  </div>
                </div>
              </div>
            );
          })()}
        </div>
      )}

      {/* TAB 3: ACTIVE SESSIONS */}
      {activeTab === 'SESSIONS' && (
        <div className="flex-1 p-3 overflow-hidden flex flex-col">
          <div className="bg-white border border-slate-200 rounded-md flex-1 flex flex-col overflow-hidden shadow-xs">
            <div className="p-3 bg-slate-50 border-b border-slate-200 flex items-center justify-between">
              <div>
                <h3 className="font-bold text-slate-800 text-xs">إدارة الجلسات المتزامنة للمستخدمين (Concurrent User Sessions)</h3>
                <p className="text-[11px] text-slate-500">يمكن للمدير إنهاء وطرد أي جلسة مفتوحة على أجهزة نقاط البيع الأخرى فورياً</p>
              </div>
            </div>

            <div className="flex-1 overflow-auto">
              <table className="w-full text-right text-xs border-collapse">
                <thead className="bg-slate-100 text-slate-700 border-b border-slate-200 sticky top-0 font-bold">
                  <tr>
                    <th className="p-2.5">المستخدم</th>
                    <th className="p-2.5">اسم الجهاز ومحطة العمل</th>
                    <th className="p-2.5">عنوان IP</th>
                    <th className="p-2.5">آخر نشاط</th>
                    <th className="p-2.5">وقت انتهاء الجلسة</th>
                    <th className="p-2.5 text-center">الحالة</th>
                    <th className="p-2.5 text-center">الإجراء</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200 text-slate-700">
                  {sessions.map((s) => (
                    <tr key={s.id} className="hover:bg-slate-50 transition">
                      <td className="p-2.5 font-bold text-sky-800 font-mono">@{s.userName}</td>
                      <td className="p-2.5 text-slate-800 flex items-center gap-1.5">
                        <Laptop className="w-3.5 h-3.5 text-slate-400" />
                        <span>{s.device}</span>
                      </td>
                      <td className="p-2.5 font-mono text-slate-600">{s.ipAddress}</td>
                      <td className="p-2.5 text-emerald-700 font-medium">{s.lastActivity}</td>
                      <td className="p-2.5 text-slate-500">{s.expiresAt}</td>
                      <td className="p-2.5 text-center">
                        {s.isRevoked ? (
                          <span className="bg-rose-50 text-rose-700 border border-rose-200 px-2 py-0.5 rounded text-[10px] font-bold">
                            تم إنهاؤها (Revoked)
                          </span>
                        ) : (
                          <span className="bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded text-[10px] font-bold">
                            نشطة (Active)
                          </span>
                        )}
                      </td>
                      <td className="p-2.5 text-center">
                        {!s.isRevoked && (
                          <button
                            onClick={() => {
                              if (confirm(`هل أنت متأكد من إنهاء جلسة "${s.userName}" على جهاز "${s.device}"؟`)) {
                                onRevokeSession(s.id);
                                alert(`تم إنهاء الجلسة وطرد المستخدم من هذا الجهاز.`);
                              }
                            }}
                            className="bg-rose-600 hover:bg-rose-500 text-white px-2.5 py-1 rounded text-[11px] font-bold transition flex items-center gap-1 mx-auto cursor-pointer"
                          >
                            <Power className="w-3 h-3" /> إنهاء الجلسة
                          </button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}

      {/* TAB 4: CHANGE PASSWORD FOR CURRENT USER */}
      {activeTab === 'PASSWORD' && (() => {
        const activeTargetUser = currentUser || selectedUser || users[0];

        // Strength calculation
        const getPasswordScore = (pass: string) => {
          if (!pass) return { text: 'غير محددة', score: 0, color: 'text-slate-400', bg: 'bg-slate-300', width: 'w-0' };
          if (pass.length < 4) return { text: 'ضعيفة جداً (أقل من 4 أحرف)', score: 1, color: 'text-rose-600', bg: 'bg-rose-500', width: 'w-1/4' };
          let score = 1;
          if (pass.length >= 6) score++;
          if (/[0-9]/.test(pass) && /[a-zA-Z]/.test(pass)) score++;
          if (/[^A-Za-z0-9]/.test(pass) || /[A-Z]/.test(pass)) score++;

          if (score === 2) return { text: 'مقبولة', score: 2, color: 'text-amber-600', bg: 'bg-amber-500', width: 'w-2/4' };
          if (score === 3) return { text: 'جيدة', score: 3, color: 'text-sky-600', bg: 'bg-sky-500', width: 'w-3/4' };
          return { text: 'قوية وممتازة', score: 4, color: 'text-emerald-600', bg: 'bg-emerald-500', width: 'w-full' };
        };

        const strength = getPasswordScore(newPasswordInput);

        const isLengthValid = newPasswordInput.length >= 4;
        const hasNumbersAndLetters = /[0-9]/.test(newPasswordInput) && /[a-zA-Z\u0600-\u06FF]/.test(newPasswordInput);
        const isDifferentFromOld = newPasswordInput.length > 0 && newPasswordInput !== currentPasswordInput;
        const isConfirmMatching = newPasswordInput.length > 0 && newPasswordInput === confirmPasswordInput;
        const canSubmit = isLengthValid && isDifferentFromOld && isConfirmMatching && currentPasswordInput.length > 0;

        const handlePasswordSubmit = (e: React.FormEvent) => {
          e.preventDefault();
          setPasswordFeedback(null);
          setIsSubmittingPassword(true);

          if (!activeTargetUser) {
            setPasswordFeedback({ type: 'error', message: 'لم يتم العثور على المستخدم الحالي.' });
            setIsSubmittingPassword(false);
            return;
          }

          // Verify old password
          const expectedOldPass = activeTargetUser.password || '1';
          if (currentPasswordInput !== expectedOldPass) {
            setPasswordFeedback({
              type: 'error',
              message: 'كلمة المرور الحالية غير صحيحة! يرجى التأكد وإعادة المحاولة.'
            });
            setIsSubmittingPassword(false);
            return;
          }

          // Validation constraints (Minimum 4 chars)
          if (newPasswordInput.length < 4) {
            setPasswordFeedback({
              type: 'error',
              message: 'يجب ألا تقل كلمة المرور الجديدة عن 4 أحرف.'
            });
            setIsSubmittingPassword(false);
            return;
          }

          if (newPasswordInput === currentPasswordInput) {
            setPasswordFeedback({
              type: 'error',
              message: 'يجب أن تكون كلمة المرور الجديدة مختلفة تماماً عن الحالية.'
            });
            setIsSubmittingPassword(false);
            return;
          }

          if (newPasswordInput !== confirmPasswordInput) {
            setPasswordFeedback({
              type: 'error',
              message: 'كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين.'
            });
            setIsSubmittingPassword(false);
            return;
          }

          // Execute password change
          if (onUpdateCurrentUserPassword) {
            onUpdateCurrentUserPassword(activeTargetUser.id, newPasswordInput);
          } else {
            activeTargetUser.password = newPasswordInput;
            activeTargetUser.mustChangePassword = false;
          }

          // Terminate other sessions if option enabled
          if (terminateOtherSessions) {
            sessions
              .filter((s) => s.userName === activeTargetUser.userName && !s.isRevoked)
              .forEach((s) => {
                onRevokeSession(s.id);
              });
          }

          setPasswordFeedback({
            type: 'success',
            message: `تم تحديث كلمة المرور للمستخدم (${activeTargetUser.fullName}) بنجاح! تم تطبيق إعدادات الأمان وإلغاء شرط التغيير الإلزامي.`
          });

          setCurrentPasswordInput('');
          setNewPasswordInput('');
          setConfirmPasswordInput('');
          setIsSubmittingPassword(false);
        };

        return (
          <div className="flex-1 flex overflow-hidden p-3 gap-3">
            {/* Right Column: User Profile & Security Policy */}
            <div className="w-80 bg-white border border-slate-200 rounded-md flex flex-col overflow-hidden shadow-xs shrink-0">
              <div className="p-3 bg-slate-50 border-b border-slate-200 text-xs font-bold text-slate-800 flex items-center justify-between">
                <span className="flex items-center gap-1.5">
                  <UserCheck className="w-4 h-4 text-emerald-600" />
                  <span>بيانات الحساب الحالي النشط</span>
                </span>
                <span className="text-[10px] bg-emerald-50 text-emerald-700 border border-emerald-200 px-2 py-0.5 rounded font-mono font-bold">
                  Online
                </span>
              </div>

              <div className="p-4 flex-1 overflow-y-auto space-y-4 text-xs">
                {/* User Identity Card */}
                <div className="bg-slate-50 border border-slate-200 rounded-lg p-3 flex items-center gap-3">
                  <div className="w-12 h-12 rounded-full bg-linear-to-br from-amber-500 to-sky-700 flex items-center justify-center font-black text-lg text-white shadow-xs">
                    {activeTargetUser?.fullName?.slice(0, 1) || 'U'}
                  </div>
                  <div className="flex-1 min-w-0">
                    <h3 className="font-bold text-slate-900 text-sm truncate">{activeTargetUser?.fullName}</h3>
                    <p className="text-slate-500 text-[11px] font-mono">@{activeTargetUser?.userName}</p>
                    <span className="inline-block mt-1 text-[10px] px-2 py-0.5 bg-sky-50 text-sky-800 rounded font-semibold border border-sky-200">
                      {activeTargetUser?.roleName}
                    </span>
                  </div>
                </div>

                {/* Account Details List */}
                <div className="space-y-2">
                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200 flex justify-between items-center">
                    <span className="text-slate-500">الفرع المعتمد:</span>
                    <span className="font-medium text-slate-800">{activeTargetUser?.branchName || 'المركز الرئيسي'}</span>
                  </div>

                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200 flex justify-between items-center">
                    <span className="text-slate-500">حالة شرط التغيير:</span>
                    <span className={activeTargetUser?.mustChangePassword ? 'text-amber-800 font-bold bg-amber-50 px-2 py-0.5 rounded text-[11px] border border-amber-200' : 'text-emerald-700 font-semibold'}>
                      {activeTargetUser?.mustChangePassword ? 'تغيير إلزامي مطلوب' : 'مستوفي للشروط'}
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200 flex justify-between items-center">
                    <span className="text-slate-500">محاولات الدخول الخاطئة:</span>
                    <span className="font-mono text-slate-700">
                      {activeTargetUser?.loginAttempts || 0} / 5
                    </span>
                  </div>

                  <div className="bg-slate-50 p-2.5 rounded border border-slate-200 flex justify-between items-center">
                    <span className="text-slate-500">آخر تسجيل دخول:</span>
                    <span className="font-mono text-[11px] text-slate-700">{activeTargetUser?.lastLoginAt || 'الجلسة الحالية'}</span>
                  </div>
                </div>

                {/* Security Advice Panel */}
                <div className="p-3 bg-amber-50/60 border border-amber-200 rounded-lg text-[11px] text-slate-700 space-y-2">
                  <p className="font-bold text-amber-900 flex items-center gap-1.5">
                    <ShieldCheck className="w-3.5 h-3.5 text-amber-600" />
                    <span>إرشادات أمان كلمات المرور (RBAC Policy):</span>
                  </p>
                  <p className="leading-relaxed">
                    • تجنب استخدام أرقام تسلسلية سهلة التخمين مثل (<strong className="text-slate-900">123456</strong> أو <strong className="text-slate-900">1111</strong>).
                  </p>
                  <p className="leading-relaxed">
                    • احرص على دمج الأرقام مع الحروف الإنجليزية لتفادي أي اختراق تخميني.
                  </p>
                  <p className="leading-relaxed">
                    • لا تقم بمشاركة كلمة المرور مع زملاء العمل في نقاط البيع؛ لكل مستخدم صلاحيات محددة.
                  </p>
                  <p className="leading-relaxed">
                    • كلمة المرور الافتراضية للنظام بعد التهيئة أو بعد إعادة التعيين من المدير هي (<strong className="text-slate-900">1</strong>).
                  </p>
                </div>
              </div>
            </div>

            {/* Left Column: Change Password Interactive Form */}
            <div className="flex-1 bg-white border border-slate-200 rounded-md flex flex-col overflow-hidden shadow-xs">
              <div className="p-3 bg-slate-50 border-b border-slate-200 flex items-center justify-between">
                <span className="text-xs font-bold text-slate-800 flex items-center gap-1.5">
                  <KeyRound className="w-4 h-4 text-amber-600" />
                  <span>تغيير كلمة المرور وتحديث بيانات الأمان للمستخدم الحالي</span>
                </span>
                <span className="text-[11px] text-slate-500 font-mono">
                  User ID: {activeTargetUser?.id}
                </span>
              </div>

              <div className="flex-1 overflow-y-auto p-5 space-y-5">
                {/* Feedback Toast / Alert */}
                {passwordFeedback && (
                  <div className={`p-3.5 rounded-lg text-xs font-bold border flex items-start gap-2.5 animate-in fade-in ${
                    passwordFeedback.type === 'success' 
                      ? 'bg-emerald-50 border-emerald-300 text-emerald-800' 
                      : 'bg-rose-50 border-rose-300 text-rose-800'
                  }`}>
                    {passwordFeedback.type === 'success' ? (
                      <CheckCircle2 className="w-5 h-5 text-emerald-600 shrink-0 mt-0.5" />
                    ) : (
                      <AlertTriangle className="w-5 h-5 text-rose-600 shrink-0 mt-0.5" />
                    )}
                    <div className="flex-1">
                      <p>{passwordFeedback.message}</p>
                    </div>
                  </div>
                )}

                <form onSubmit={handlePasswordSubmit} className="space-y-4 max-w-xl">
                  {/* Field 1: Current Password */}
                  <div>
                    <label className="block text-slate-700 font-bold mb-1.5 text-xs flex items-center justify-between">
                      <span className="flex items-center gap-1.5">
                        <Lock className="w-3.5 h-3.5 text-slate-400" />
                        <span>كلمة المرور الحالية (Current Password):</span>
                      </span>
                      <span className="text-[11px] font-normal text-slate-500">
                        (الافتراضية بعد التعيين: 1)
                      </span>
                    </label>
                    <div className="relative">
                      <input
                        type={showCurrentPass ? 'text' : 'password'}
                        required
                        placeholder="أدخل كلمة المرور الحالية لتأكيد هويتك..."
                        value={currentPasswordInput}
                        onChange={(e) => setCurrentPasswordInput(e.target.value)}
                        className="w-full bg-white border border-slate-300 rounded-lg pr-3 pl-10 py-2 text-xs text-slate-800 font-mono placeholder-slate-400 focus:outline-hidden focus:ring-2 focus:ring-sky-500"
                      />
                      <button
                        type="button"
                        onClick={() => setShowCurrentPass(!showCurrentPass)}
                        className="absolute left-2.5 top-2.5 text-slate-400 hover:text-slate-700 cursor-pointer"
                        title={showCurrentPass ? 'إخفاء' : 'إظهار'}
                      >
                        {showCurrentPass ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                  </div>

                  {/* Field 2: New Password */}
                  <div>
                    <label className="block text-slate-700 font-bold mb-1.5 text-xs flex items-center justify-between">
                      <span className="flex items-center gap-1.5">
                        <KeyRound className="w-3.5 h-3.5 text-amber-600" />
                        <span>كلمة المرور الجديدة (New Password):</span>
                      </span>
                      <span className={`text-[11px] font-bold ${strength.color}`}>
                        مستوى القوة: {strength.text}
                      </span>
                    </label>
                    <div className="relative">
                      <input
                        type={showNewPass ? 'text' : 'password'}
                        required
                        placeholder="أدخل كلمة المرور الجديدة (6 خانات كحد أدنى)..."
                        value={newPasswordInput}
                        onChange={(e) => setNewPasswordInput(e.target.value)}
                        className="w-full bg-white border border-slate-300 rounded-lg pr-3 pl-10 py-2 text-xs text-slate-800 font-mono placeholder-slate-400 focus:outline-hidden focus:ring-2 focus:ring-amber-500"
                      />
                      <button
                        type="button"
                        onClick={() => setShowNewPass(!showNewPass)}
                        className="absolute left-2.5 top-2.5 text-slate-400 hover:text-slate-700 cursor-pointer"
                        title={showNewPass ? 'إخفاء' : 'إظهار'}
                      >
                        {showNewPass ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>

                    {/* Dynamic Password Strength Progress Bar */}
                    <div className="mt-2 bg-slate-100 rounded-full h-1.5 overflow-hidden border border-slate-200">
                      <div className={`h-full transition-all duration-300 ${strength.bg} ${strength.width}`} />
                    </div>
                  </div>

                  {/* Field 3: Confirm New Password */}
                  <div>
                    <label className="block text-slate-700 font-bold mb-1.5 text-xs flex items-center justify-between">
                      <span className="flex items-center gap-1.5">
                        <Lock className="w-3.5 h-3.5 text-emerald-600" />
                        <span>تأكيد كلمة المرور الجديدة (Confirm Password):</span>
                      </span>
                      {confirmPasswordInput && (
                        <span className={`text-[11px] font-bold ${isConfirmMatching ? 'text-emerald-700' : 'text-rose-600'}`}>
                          {isConfirmMatching ? 'متطابقة تماماً ✓' : 'غير متطابقة ✗'}
                        </span>
                      )}
                    </label>
                    <div className="relative">
                      <input
                        type={showConfirmPass ? 'text' : 'password'}
                        required
                        placeholder="أعد كتابة كلمة المرور الجديدة للتأكيد..."
                        value={confirmPasswordInput}
                        onChange={(e) => setConfirmPasswordInput(e.target.value)}
                        className={`w-full bg-white border rounded-lg pr-3 pl-10 py-2 text-xs text-slate-800 font-mono placeholder-slate-400 focus:outline-hidden focus:ring-2 ${
                          confirmPasswordInput && !isConfirmMatching 
                            ? 'border-rose-400 focus:ring-rose-500' 
                            : 'border-slate-300 focus:ring-emerald-500'
                        }`}
                      />
                      <button
                        type="button"
                        onClick={() => setShowConfirmPass(!showConfirmPass)}
                        className="absolute left-2.5 top-2.5 text-slate-400 hover:text-slate-700 cursor-pointer"
                        title={showConfirmPass ? 'إخفاء' : 'إظهار'}
                      >
                        {showConfirmPass ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                  </div>

                  {/* Interactive Real-Time Checklist */}
                  <div className="bg-slate-50 border border-slate-200 rounded-lg p-3 space-y-2">
                    <span className="text-[11px] font-bold text-slate-700 block mb-1">
                      قائمة التحقق التفاعلية من سياسة الأمان:
                    </span>
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 text-[11px]">
                      <div className={`flex items-center gap-1.5 ${isLengthValid ? 'text-emerald-700 font-semibold' : 'text-slate-500'}`}>
                        {isLengthValid ? <Check className="w-3.5 h-3.5 text-emerald-600" /> : <XCircle className="w-3.5 h-3.5 text-slate-400" />}
                        <span>طول كلمة المرور (6 خانات فأكثر)</span>
                      </div>

                      <div className={`flex items-center gap-1.5 ${hasNumbersAndLetters ? 'text-emerald-700 font-semibold' : 'text-slate-500'}`}>
                        {hasNumbersAndLetters ? <Check className="w-3.5 h-3.5 text-emerald-600" /> : <XCircle className="w-3.5 h-3.5 text-slate-400" />}
                        <span>مزيج أرقام وأحرف</span>
                      </div>

                      <div className={`flex items-center gap-1.5 ${isDifferentFromOld ? 'text-emerald-700 font-semibold' : 'text-slate-500'}`}>
                        {isDifferentFromOld ? <Check className="w-3.5 h-3.5 text-emerald-600" /> : <XCircle className="w-3.5 h-3.5 text-slate-400" />}
                        <span>تختلف عن كلمة المرور الحالية</span>
                      </div>

                      <div className={`flex items-center gap-1.5 ${isConfirmMatching ? 'text-emerald-700 font-semibold' : 'text-slate-500'}`}>
                        {isConfirmMatching ? <Check className="w-3.5 h-3.5 text-emerald-600" /> : <XCircle className="w-3.5 h-3.5 text-slate-400" />}
                        <span>تطابق كلمتي المرور</span>
                      </div>
                    </div>
                  </div>

                  {/* Security Option Checkbox */}
                  <div className="bg-slate-50 border border-slate-200 rounded-lg p-3 flex items-center justify-between">
                    <label className="flex items-center gap-2 cursor-pointer select-none">
                      <input
                        type="checkbox"
                        checked={terminateOtherSessions}
                        onChange={(e) => setTerminateOtherSessions(e.target.checked)}
                        className="w-4 h-4 rounded text-sky-700 focus:ring-sky-500 cursor-pointer"
                      />
                      <span className="text-slate-800 text-xs font-semibold">
                        إنهاء كافة الجلسات النشطة الأخرى للمستخدم فور الحفظ
                      </span>
                    </label>
                    <span className="text-[10px] text-slate-500">
                      (إجراء أمني موصى به)
                    </span>
                  </div>

                  {/* Action Buttons */}
                  <div className="pt-2 flex items-center gap-3">
                    <button
                      type="submit"
                      disabled={isSubmittingPassword}
                      className={`px-5 py-2 rounded-lg text-xs font-bold transition flex items-center gap-2 shadow-xs cursor-pointer ${
                        canSubmit 
                          ? 'bg-amber-600 hover:bg-amber-500 text-white' 
                          : 'bg-slate-200 text-slate-400 cursor-not-allowed'
                      }`}
                    >
                      <Save className="w-4 h-4" />
                      <span>{isSubmittingPassword ? 'جاري الحفظ...' : 'تحديث وحفظ كلمة المرور'}</span>
                    </button>

                    <button
                      type="button"
                      onClick={() => {
                        setCurrentPasswordInput('');
                        setNewPasswordInput('');
                        setConfirmPasswordInput('');
                        setPasswordFeedback(null);
                      }}
                      className="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-xs font-semibold transition flex items-center gap-1.5 cursor-pointer border border-slate-300"
                    >
                      <RotateCcw className="w-3.5 h-3.5" />
                      <span>إعادة ضبط الحقول</span>
                    </button>

                    {onTestLoginAsUser && (
                      <button
                        type="button"
                        onClick={() => onTestLoginAsUser(activeTargetUser)}
                        className="mr-auto px-4 py-2 bg-sky-50 hover:bg-sky-100 text-sky-800 rounded-lg text-xs font-bold transition flex items-center gap-1.5 cursor-pointer border border-sky-300"
                        title="تجربة تسجيل الدخول السريع بهذا المستخدم"
                      >
                        <LogIn className="w-3.5 h-3.5 text-sky-700" />
                        <span>محاكاة تسجيل الدخول بهذا الحساب</span>
                      </button>
                    )}
                  </div>
                </form>
              </div>
            </div>
          </div>
        );
      })()}

      {/* NEW USER MODAL */}
      {showNewUserModal && (
        <div className="fixed inset-0 z-50 bg-black/40 backdrop-blur-xs flex items-center justify-center p-4">
          <div className="bg-white border border-slate-300 rounded-lg w-full max-w-md shadow-2xl overflow-hidden animate-in fade-in zoom-in-95">
            <div className="p-3 bg-slate-50 border-b border-slate-200 flex items-center justify-between text-slate-800">
              <h3 className="font-bold text-xs flex items-center gap-1.5">
                <UserPlus className="w-4 h-4 text-emerald-600" />
                <span>إضافة مستخدم جديد للنظام (Create User)</span>
              </h3>
              <button
                onClick={() => setShowNewUserModal(false)}
                className="text-slate-400 hover:text-slate-700 text-sm cursor-pointer"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleSaveNewUser} className="p-4 space-y-3 text-xs">
              <div>
                <label className="block text-slate-700 font-semibold mb-1">اسم المستخدم (Username - إنجليزي فريد):</label>
                <input
                  type="text"
                  required
                  placeholder="مثال: cashier2 أو ahmed"
                  value={newUserName}
                  onChange={(e) => setNewUserName(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 font-mono focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">الاسم الكامل (Display Name):</label>
                <input
                  type="text"
                  required
                  placeholder="مثال: أحمد محمد صالح"
                  value={newFullName}
                  onChange={(e) => setNewFullName(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">الدور الوظيفي (Role):</label>
                <select
                  value={newRoleId}
                  onChange={(e) => setNewRoleId(Number(e.target.value))}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500 cursor-pointer"
                >
                  {roles.filter((r) => r.id !== 1).map((r) => (
                    <option key={r.id} value={r.id}>
                      {r.name} - ({r.description.slice(0, 35)}...)
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">الفرع المخصص:</label>
                <select
                  value={newBranchId === null ? 'ALL' : newBranchId}
                  onChange={(e) => setNewBranchId(e.target.value === 'ALL' ? null : Number(e.target.value))}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500 cursor-pointer"
                >
                  <option value={1}>المركز الرئيسي (صنعاء)</option>
                  <option value="ALL">جميع الفروع (صلاحية عامة)</option>
                </select>
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">الصندوق الافتراضي المربوط:</label>
                <select
                  value={newDefaultCashboxCode}
                  onChange={(e) => setNewDefaultCashboxCode(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500 cursor-pointer"
                >
                  <option value="">-- اختر صندوقاً افتراضياً (يدوي عند كل عملية) --</option>
                  <option value="1111">الصندوق الرئيسي (1111)</option>
                  <option value="1112">صندوق كاشير 1 (1112)</option>
                  <option value="1113">صندوق كاشير 2 (1113)</option>
                </select>
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">المستودع الافتراضي المربوط:</label>
                <select
                  value={newDefaultWarehouseId}
                  onChange={(e) => setNewDefaultWarehouseId(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500 cursor-pointer"
                >
                  <option value="">-- اختر مستودعاً افتراضياً (يدوي عند كل عملية) --</option>
                  <option value="WH-01">المستودع الرئيسي - صنعاء (WH-01)</option>
                  <option value="WH-02">مستودع فرع حدة (WH-02)</option>
                </select>
              </div>

              <div className="bg-amber-50/70 p-2.5 rounded border border-amber-200 text-[11px] text-slate-700">
                <p className="font-bold text-amber-900">ملاحظة أمان:</p>
                <p>• كلمة المرور الافتراضية ستكون تلقائياً <strong className="text-slate-900">1</strong>.</p>
                <p>• سيتم تفعيل شرط <strong className="text-slate-900">MustChangePassword = true</strong> لتغييرها فور الدخول.</p>
              </div>

              <div className="pt-2 flex items-center justify-end gap-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowNewUserModal(false)}
                  className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded border border-slate-300 cursor-pointer font-medium"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="px-4 py-1.5 bg-emerald-600 hover:bg-emerald-500 text-white font-bold rounded shadow-xs cursor-pointer"
                >
                  حفظ المستخدم
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* NEW CUSTOM ROLE MODAL */}
      {showNewRoleModal && (
        <div className="fixed inset-0 bg-black/40 z-50 flex items-center justify-center p-4 backdrop-blur-xs">
          <div className="bg-white border border-slate-300 rounded-lg shadow-2xl w-full max-w-md overflow-hidden animate-in fade-in zoom-in-95">
            <div className="bg-slate-50 px-4 py-3 border-b border-slate-200 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Shield className="w-5 h-5 text-sky-700" />
                <h3 className="font-bold text-slate-900 text-sm">إنشاء دور وظيفي مخصص جديد (Custom RBAC Role)</h3>
              </div>
              <button
                type="button"
                onClick={() => setShowNewRoleModal(false)}
                className="text-slate-400 hover:text-slate-700 cursor-pointer"
              >
                ✕
              </button>
            </div>

            <form onSubmit={handleCreateNewRole} className="p-4 space-y-3 text-xs">
              <div>
                <label className="block text-slate-700 font-semibold mb-1">اسم الدور الوظيفي (Role Name):</label>
                <input
                  type="text"
                  required
                  placeholder="مثال: مسؤول مبيعات خارجي أو مدقق داخلي"
                  value={newRoleName}
                  onChange={(e) => setNewRoleName(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">وصف الصلاحيات والمهام:</label>
                <textarea
                  rows={2}
                  placeholder="وصف مختصر لمسؤوليات هذا الدور..."
                  value={newRoleDesc}
                  onChange={(e) => setNewRoleDesc(e.target.value)}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-slate-700 font-semibold mb-1">استنساخ الصلاحيات الأولية من قالب (Base Template):</label>
                <select
                  value={newRoleBaseTemplate}
                  onChange={(e) => setNewRoleBaseTemplate(Number(e.target.value))}
                  className="w-full bg-white border border-slate-300 rounded px-3 py-1.5 text-slate-800 focus:outline-hidden focus:ring-1 focus:ring-sky-500 cursor-pointer"
                >
                  {roles.filter((r) => r.id !== 1).map((r) => (
                    <option key={r.id} value={r.id}>
                      قالب: {r.name} - ({r.description.slice(0, 30)}...)
                    </option>
                  ))}
                </select>
              </div>

              <div className="bg-sky-50/70 p-2.5 rounded border border-sky-200 text-[11px] text-slate-700">
                <p className="font-bold text-sky-900">ملاحظة القناع الرقمي (Bitmask):</p>
                <p>بعد الحفظ، سيظهر الدور في القائمة فوراً وستتمكن من تعديل قناع بتات الصلاحيات (64-bit Bitmask) في الأقسام الأربعة بدقة كاملة.</p>
              </div>

              <div className="pt-2 flex items-center justify-end gap-2 border-t border-slate-200">
                <button
                  type="button"
                  onClick={() => setShowNewRoleModal(false)}
                  className="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded border border-slate-300 cursor-pointer font-medium"
                >
                  إلغاء
                </button>
                <button
                  type="submit"
                  className="px-4 py-1.5 bg-sky-700 hover:bg-sky-600 text-white font-bold rounded shadow-xs cursor-pointer"
                >
                  إنشاء الدور
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};
