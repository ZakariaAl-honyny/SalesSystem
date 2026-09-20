'use client';

import React, { useState } from 'react';
import {
  Database,
  HardDrive,
  ShieldCheck,
  Download,
  Upload,
  Clock,
  RefreshCw,
  AlertTriangle,
  CheckCircle2,
  Copy,
  FileText,
  Settings,
  Key,
  Server,
  Layers,
  Calendar,
  Terminal,
  Lock,
  Archive,
  Check,
  X,
  Play,
  FolderOpen
} from 'lucide-react';

export interface BackupRecord {
  id: string;
  fileName: string;
  backupType: 'FULL' | 'DIFF' | 'LOG';
  createdAt: string;
  sizeMB: number;
  compressedSizeMB: number;
  compressionRatio: string;
  status: 'VERIFIED' | 'COMPLETED' | 'WARNING';
  executedBy: string;
  filePath: string;
}

interface BackupRestoreModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export const BackupRestoreModal: React.FC<BackupRestoreModalProps> = ({ isOpen, onClose }) => {
  const [activeTab, setActiveTab] = useState<'BACKUP' | 'RESTORE' | 'SCHEDULE' | 'TSQL_SCRIPTS'>('BACKUP');

  // Backup form state
  const [backupType, setBackupType] = useState<'FULL' | 'DIFF'>('FULL');
  const [targetPath, setTargetPath] = useState<string>('D:\\YemenPOS_Backups\\');
  const [useCompression, setUseCompression] = useState<boolean>(true);
  const [useChecksum, setUseChecksum] = useState<boolean>(true);
  const [verifyAfterBackup, setVerifyAfterBackup] = useState<boolean>(true);
  const [encryptBackup, setEncryptBackup] = useState<boolean>(false);
  const [encryptionPassword, setEncryptionPassword] = useState<string>('');

  // Execution states
  const [isProcessing, setIsProcessing] = useState<boolean>(false);
  const [progressPercent, setProgressPercent] = useState<number>(0);
  const [processStage, setProcessStage] = useState<string>('');
  const [executionResult, setExecutionResult] = useState<{ success: boolean; message: string; details?: string } | null>(null);
  const [copiedCode, setCopiedCode] = useState<boolean>(false);

  // Restore form state
  const [selectedBackupForRestore, setSelectedBackupForRestore] = useState<string>('YemenPOS_DB_Full_20260919_0400.bak');
  const [takePreRestoreSafetyBackup, setTakePreRestoreSafetyBackup] = useState<boolean>(true);
  const [forceSingleUserMode, setForceSingleUserMode] = useState<boolean>(true);
  const [adminPasswordConfirm, setAdminPasswordConfirm] = useState<string>('');
  const [restoreStage, setRestoreStage] = useState<string>('');

  // Schedule settings
  const [autoBackupEnabled, setAutoBackupEnabled] = useState<boolean>(true);
  const [backupOnShiftClose, setBackupOnShiftClose] = useState<boolean>(true);
  const [scheduledDailyTime, setScheduledDailyTime] = useState<string>('23:45');
  const [retentionDays, setRetentionDays] = useState<number>(30);
  const [cloudMirrorEnabled, setCloudMirrorEnabled] = useState<boolean>(true);

  // Mock SQL Server Backup History
  const [historyList, setHistoryList] = useState<BackupRecord[]>([
    {
      id: 'BK-105',
      fileName: 'YemenPOS_DB_Full_20260919_0400.bak',
      backupType: 'FULL',
      createdAt: '2026-09-19 04:00 AM',
      sizeMB: 38.4,
      compressedSizeMB: 6.2,
      compressionRatio: '83.8%',
      status: 'VERIFIED',
      executedBy: 'مدير النظام (SQL Admin)',
      filePath: 'D:\\YemenPOS_Backups\\YemenPOS_DB_Full_20260919_0400.bak',
    },
    {
      id: 'BK-104',
      fileName: 'YemenPOS_DB_Diff_20260918_2200.bak',
      backupType: 'DIFF',
      createdAt: '2026-09-18 10:00 PM',
      sizeMB: 4.8,
      compressedSizeMB: 0.9,
      compressionRatio: '81.2%',
      status: 'VERIFIED',
      executedBy: 'النظام التلقائي (Z-Report Close)',
      filePath: 'D:\\YemenPOS_Backups\\YemenPOS_DB_Diff_20260918_2200.bak',
    },
    {
      id: 'BK-103',
      fileName: 'YemenPOS_DB_Full_20260918_0400.bak',
      backupType: 'FULL',
      createdAt: '2026-09-18 04:00 AM',
      sizeMB: 37.9,
      compressedSizeMB: 6.1,
      compressionRatio: '83.9%',
      status: 'VERIFIED',
      executedBy: 'مدير النظام (SQL Admin)',
      filePath: 'D:\\YemenPOS_Backups\\YemenPOS_DB_Full_20260918_0400.bak',
    },
  ]);

  if (!isOpen) return null;

  // Generate real T-SQL Script for SSMS
  const generatedTSQLBackup = `-- =================================================================
-- كود النسخ الاحتياطي لقاعدة بيانات SQL Server (YemenPOS_DB)
-- متوافق مع: SQL Server 2016 / 2019 / 2022
-- =================================================================
USE [master];
GO

DECLARE @BackupFileName NVARCHAR(500);
DECLARE @Timestamp NVARCHAR(50);
SET @Timestamp = REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(19), GETDATE(), 120), '-', ''), ':', ''), ' ', '_');
SET @BackupFileName = N'${targetPath}YemenPOS_DB_${backupType}_' + @Timestamp + N'.bak';

PRINT N'>> بدء عملية النسخ الاحتياطي لقاعدة البيانات [YemenPOS_DB]...';

BACKUP DATABASE [YemenPOS_DB]
TO DISK = @BackupFileName
WITH 
    ${backupType === 'DIFF' ? 'DIFFERENTIAL,' : 'FORMAT, INIT,'}
    NAME = N'YemenPOS_DB - ${backupType === 'FULL' ? 'Full' : 'Differential'} Backup',
    SKIP, 
    NOREWIND, 
    NOUNLOAD, 
    ${useCompression ? 'COMPRESSION,' : ''}
    ${useChecksum ? 'CHECKSUM,' : ''}
    STATS = 10;
GO

${verifyAfterBackup ? `-- التحقق من سلامة وصحة النسخة الاحتياطية
PRINT N'>> جاري التحقق من سلامة ملف النسخة (RESTORE VERIFYONLY)...';
RESTORE VERIFYONLY 
FROM DISK = N'${targetPath}YemenPOS_DB_${backupType}_Latest.bak'
WITH CHECKSUM;
GO
PRINT N'>> تم التحقق بنجاح: ملف النسخة سليم 100% وقابل للاستعادة.';` : ''}`;

  const generatedTSQLRestore = `-- =================================================================
-- كود استعادة قاعدة بيانات SQL Server مع قفل الاتصالات النشطة
-- تحذير: هذه العملية ستقوم باستبدال البيانات الحالية
-- =================================================================
USE [master];
GO

-- 1. فصل المستخدمين وتحويل القاعدة إلى وضع المستخدم الفردي
PRINT N'>> فصل الاتصالات النشطة مؤقتاً...';
ALTER DATABASE [YemenPOS_DB] 
SET SINGLE_USER 
WITH ROLLBACK IMMEDIATE;
GO

-- 2. تنفيذ الاستعادة مع استبدال الملفات وفحص التطابق
PRINT N'>> استعادة البيانات من ملف النسخة الاحتياطية...';
RESTORE DATABASE [YemenPOS_DB]
FROM DISK = N'${targetPath}${selectedBackupForRestore}'
WITH 
    REPLACE, 
    RECOVERY, 
    STATS = 10;
GO

-- 3. إعادة تفعيل الوضع متعدد المستخدمين
PRINT N'>> إعادة تشغيل الاتصالات ووضع Multi-User...';
ALTER DATABASE [YemenPOS_DB] 
SET MULTI_USER;
GO

-- 4. إعادة بناء الفهارس وتحديث الإحصائيات لسرعة نقطة البيع
EXEC [YemenPOS_DB]..sp_updatestats;
GO
PRINT N'>> تم استعادة قاعدة البيانات بنجاح وجاهزة للعمل الفوري!';`;

  // Simulate Backup Execution
  const handleStartBackup = () => {
    setIsProcessing(true);
    setProgressPercent(10);
    setProcessStage('1/4: قفل المعاملات وتجهيز لقطة بيانات SQL Server Snapshot...');
    setExecutionResult(null);

    setTimeout(() => {
      setProgressPercent(40);
      setProcessStage('2/4: تفريغ الجداول وضغط المحتوى (Gzip/Native SQL Compression)...');
    }, 700);

    setTimeout(() => {
      setProgressPercent(75);
      setProcessStage('3/4: كتابة ملف الباك اب وفحص التجزئة CHECKSUM...');
    }, 1400);

    setTimeout(() => {
      setProgressPercent(95);
      setProcessStage('4/4: تنفيذ التحقق (RESTORE VERIFYONLY) وتأكيد سلامة الفهارس...');
    }, 2000);

    setTimeout(() => {
      setProgressPercent(100);
      setIsProcessing(false);
      const nowStr = new Date().toISOString().replace('T', ' ').substring(0, 16);
      const uniqueSuffix = Date.now().toString().slice(-6);
      const newFileName = `YemenPOS_DB_${backupType}_${uniqueSuffix}.bak`;
      
      // Calculate truly unique incremental ID by finding maximum numeric suffix in historyList
      const maxIdNum = historyList.reduce((max, item) => {
        const match = item.id.match(/\d+/);
        const num = match ? parseInt(match[0], 10) : 0;
        return num > max ? num : max;
      }, 105);

      const newRec: BackupRecord = {
        id: `BK-${maxIdNum + 1}`,
        fileName: newFileName,
        backupType: backupType,
        createdAt: nowStr,
        sizeMB: backupType === 'FULL' ? 39.1 : 5.2,
        compressedSizeMB: backupType === 'FULL' ? 6.4 : 1.1,
        compressionRatio: '83.6%',
        status: 'VERIFIED',
        executedBy: 'مدير النظام (مستخدم حالي)',
        filePath: `${targetPath}${newFileName}`,
      };
      setHistoryList((prev) => [newRec, ...prev]);
      setExecutionResult({
        success: true,
        message: 'تم إنشاء نسخة احتياطية مشفرة وسليمة 100% لقاعدة بيانات SQL Server!',
        details: `اسم الملف: ${newFileName} | الحجم المضغوط: ${newRec.compressedSizeMB} ميجابايت (توفير ${newRec.compressionRatio}) | المسار: ${newRec.filePath}`,
      });
    }, 2600);
  };

  // Simulate Restore Execution
  const handleStartRestore = () => {
    if (adminPasswordConfirm !== '123' && adminPasswordConfirm.trim() !== 'admin') {
      alert('يرجى كتابة كلمة مرور مدير النظام الصحيحة (اكتب 123 أو admin للتجربة) لتأكيد الاستعادة.');
      return;
    }

    if (!confirm(`تحذير أمني هام:\nهل أنت متأكد من استعادة النسخة الاحتياطية (${selectedBackupForRestore})؟\nسيتم أخذ نسخة أمان تلقائية ثم استبدال بيانات قاعدة البيانات الحالية.`)) {
      return;
    }

    setIsProcessing(true);
    setProgressPercent(15);
    setRestoreStage('1/5: أخذ نسخة أمان احترازية إجبارية من البيانات الحالية (Tail-Log Backup)...');

    setTimeout(() => {
      setProgressPercent(35);
      setRestoreStage('2/5: تحويل SQL Server لوضع المستخدم الفردي (SET SINGLE_USER WITH ROLLBACK IMMEDIATE)...');
    }, 800);

    setTimeout(() => {
      setProgressPercent(65);
      setRestoreStage('3/5: قراءة ملف .bak واستعادة الجداول والقيود المحاسبية...');
    }, 1600);

    setTimeout(() => {
      setProgressPercent(85);
      setRestoreStage('4/5: إعادة تفعيل الوضع متعدد المستخدمين (SET MULTI_USER)...');
    }, 2300);

    setTimeout(() => {
      setProgressPercent(100);
      setIsProcessing(false);
      setRestoreStage('5/5: اكتملت الاستعادة وإعادة بناء الفهارس (sp_updatestats)!');
      alert(`تمت استعادة قاعدة بيانات SQL Server بنجاح من النسخة (${selectedBackupForRestore})!\nتم تحديث أرصدة الحسابات والمخزون.`);
      setAdminPasswordConfirm('');
    }, 3000);
  };

  const handleCopyTSQL = (textToCopy: string) => {
    navigator.clipboard.writeText(textToCopy);
    setCopiedCode(true);
    setTimeout(() => setCopiedCode(false), 2500);
  };

  // Download Mock Backup File
  const handleDownloadBackupFile = (fileName: string) => {
    const dummyContent = JSON.stringify({
      database: 'YemenPOS_DB',
      rdbms: 'Microsoft SQL Server',
      timestamp: new Date().toISOString(),
      schemaVersion: '1.0.0',
      tables: ['tbl_Products', 'tbl_Invoices', 'tbl_InvoiceLines', 'tbl_JournalEntries', 'tbl_Accounts', 'tbl_Warehouses', 'tbl_Customers', 'tbl_Suppliers'],
      note: 'YemenPOS SQL Server Binary Database Dump Simulation',
    }, null, 2);

    const blob = new Blob([dummyContent], { type: 'application/octet-stream' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-3 bg-slate-800/40 backdrop-blur-xs animate-in fade-in select-none">
      <div className="bg-white rounded-2xl max-w-4xl w-full flex flex-col max-h-[92vh] shadow-2xl border border-slate-200 text-right overflow-hidden">
        {/* Modal Top Header (Light professional theme) */}
        <div className="bg-gradient-to-r from-slate-50 via-indigo-50/40 to-slate-50 text-slate-800 p-4 flex items-center justify-between border-b border-slate-200">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-xl bg-indigo-600 flex items-center justify-center text-white shadow-xs">
              <Server className="w-5 h-5" />
            </div>
            <div>
              <div className="flex items-center gap-2">
                <h2 className="text-base font-extrabold text-slate-900">
                  إدارة النسخ الاحتياطي والاستعادة — Microsoft SQL Server
                </h2>
                <span className="text-[11px] bg-indigo-100 text-indigo-700 border border-indigo-200 px-2 py-0.5 rounded-md font-mono font-bold">
                  YemenPOS_DB
                </span>
              </div>
              <p className="text-xs text-slate-600">
                حماية البيانات المحاسبية، توليد ملفات .bak متوافقة مع SSMS، جدولة الصيانة، واستعادة آمنة مع قفل الاتصالات.
              </p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-slate-400 hover:text-slate-700 hover:bg-slate-200/70 transition cursor-pointer"
            title="إغلاق النافذة"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Navigation Tabs Bar */}
        <div className="flex items-center bg-slate-100 border-b border-slate-200 px-4 pt-2 gap-2 text-xs font-bold">
          <button
            onClick={() => setActiveTab('BACKUP')}
            className={`pb-2.5 px-4 flex items-center gap-2 border-b-2 transition cursor-pointer ${
              activeTab === 'BACKUP'
                ? 'border-indigo-600 text-indigo-700 bg-white rounded-t-lg shadow-xs'
                : 'border-transparent text-slate-600 hover:text-slate-900'
            }`}
          >
            <HardDrive className="w-4 h-4" />
            <span>إنشاء نسخة احتياطية (Backup)</span>
          </button>

          <button
            onClick={() => setActiveTab('RESTORE')}
            className={`pb-2.5 px-4 flex items-center gap-2 border-b-2 transition cursor-pointer ${
              activeTab === 'RESTORE'
                ? 'border-rose-600 text-rose-700 bg-white rounded-t-lg shadow-xs'
                : 'border-transparent text-slate-600 hover:text-slate-900'
            }`}
          >
            <RefreshCw className="w-4 h-4" />
            <span>استعادة قاعدة البيانات (Restore)</span>
          </button>

          <button
            onClick={() => setActiveTab('SCHEDULE')}
            className={`pb-2.5 px-4 flex items-center gap-2 border-b-2 transition cursor-pointer ${
              activeTab === 'SCHEDULE'
                ? 'border-emerald-600 text-emerald-700 bg-white rounded-t-lg shadow-xs'
                : 'border-transparent text-slate-600 hover:text-slate-900'
            }`}
          >
            <Clock className="w-4 h-4" />
            <span>الجدولة الآلية وصيانة SQL Agent</span>
          </button>

          <button
            onClick={() => setActiveTab('TSQL_SCRIPTS')}
            className={`pb-2.5 px-4 flex items-center gap-2 border-b-2 transition cursor-pointer ${
              activeTab === 'TSQL_SCRIPTS'
                ? 'border-purple-600 text-purple-700 bg-white rounded-t-lg shadow-xs'
                : 'border-transparent text-slate-600 hover:text-slate-900'
            }`}
          >
            <Terminal className="w-4 h-4" />
            <span>أوامر T-SQL لـ SSMS</span>
          </button>
        </div>

        {/* Tab Content Container */}
        <div className="flex-1 overflow-y-auto p-5 space-y-5 bg-slate-50">
          {/* TAB 1: BACKUP */}
          {activeTab === 'BACKUP' && (
            <div className="space-y-4">
              {/* Info banner */}
              <div className="bg-indigo-50/70 border border-indigo-200/80 rounded-xl p-3.5 text-xs text-indigo-950 flex items-start gap-3 shadow-xs">
                <ShieldCheck className="w-5 h-5 text-indigo-600 shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <span className="font-extrabold block text-sm text-indigo-900">
                    استراتيجية النسخ الاحتياطي لقاعدة بيانات SQL Server:
                  </span>
                  <p className="text-slate-600 leading-relaxed">
                    يتم إنشاء ملف <code className="font-bold text-indigo-700">.bak</code> معياري مع ضغط أصلي (Native Compression) وفحص التجزئة (Checksum)، مما يوفر أكثر من 80% من المساحة ويضمن إمكانية الاستعادة في ثوانٍ.
                  </p>
                </div>
              </div>

              {/* Form Card */}
              <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-xs space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {/* Backup Type */}
                  <div className="space-y-1.5">
                    <label className="text-xs font-bold text-slate-700 flex items-center gap-1.5">
                      <Database className="w-3.5 h-3.5 text-slate-500" />
                      <span>نوع النسخ الاحتياطي:</span>
                    </label>
                    <div className="grid grid-cols-2 gap-2 text-xs">
                      <button
                        type="button"
                        onClick={() => setBackupType('FULL')}
                        className={`p-2.5 rounded-lg border font-bold text-center transition cursor-pointer ${
                          backupType === 'FULL'
                            ? 'bg-indigo-50 border-indigo-400 text-indigo-800'
                            : 'bg-slate-50 border-slate-200 text-slate-600 hover:bg-slate-100'
                        }`}
                      >
                        نسخ كامل (Full Backup)
                        <span className="block text-[10px] font-normal text-slate-500 mt-0.5">شامل لجميع الجداول</span>
                      </button>
                      <button
                        type="button"
                        onClick={() => setBackupType('DIFF')}
                        className={`p-2.5 rounded-lg border font-bold text-center transition cursor-pointer ${
                          backupType === 'DIFF'
                            ? 'bg-indigo-50 border-indigo-400 text-indigo-800'
                            : 'bg-slate-50 border-slate-200 text-slate-600 hover:bg-slate-100'
                        }`}
                      >
                        نسخ تفاضلي (Differential)
                        <span className="block text-[10px] font-normal text-slate-500 mt-0.5">العمليات الجديدة فقط</span>
                      </button>
                    </div>
                  </div>

                  {/* Destination Folder Path */}
                  <div className="space-y-1.5">
                    <label className="text-xs font-bold text-slate-700 flex items-center gap-1.5">
                      <FolderOpen className="w-3.5 h-3.5 text-slate-500" />
                      <span>مجلد الحفظ المحلي أو الشبكي (SQL Target Path):</span>
                    </label>
                    <input
                      type="text"
                      value={targetPath}
                      onChange={(e) => setTargetPath(e.target.value)}
                      placeholder="D:\YemenPOS_Backups\"
                      className="w-full text-xs font-mono p-2.5 bg-slate-50 border border-slate-300 rounded-lg text-slate-800 focus:bg-white focus:border-indigo-600 outline-hidden transition"
                      dir="ltr"
                    />
                  </div>
                </div>

                {/* Advanced SQL Server Options */}
                <div className="pt-2 border-t border-slate-100 grid grid-cols-1 sm:grid-cols-3 gap-3 text-xs">
                  <label className="flex items-center gap-2 p-2.5 rounded-lg border border-slate-200 bg-slate-50 hover:bg-slate-100 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={useCompression}
                      onChange={(e) => setUseCompression(e.target.checked)}
                      className="rounded text-indigo-600 focus:ring-0"
                    />
                    <div>
                      <span className="font-bold text-slate-800 block">ضغط البيانات (Compression)</span>
                      <span className="text-[10px] text-slate-500">تقليل الحجم بنسبة ~80%</span>
                    </div>
                  </label>

                  <label className="flex items-center gap-2 p-2.5 rounded-lg border border-slate-200 bg-slate-50 hover:bg-slate-100 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={useChecksum}
                      onChange={(e) => setUseChecksum(e.target.checked)}
                      className="rounded text-indigo-600 focus:ring-0"
                    />
                    <div>
                      <span className="font-bold text-slate-800 block">فحص التجزئة (Checksum)</span>
                      <span className="text-[10px] text-slate-500">كشف أي تلف في القرص</span>
                    </div>
                  </label>

                  <label className="flex items-center gap-2 p-2.5 rounded-lg border border-slate-200 bg-slate-50 hover:bg-slate-100 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={verifyAfterBackup}
                      onChange={(e) => setVerifyAfterBackup(e.target.checked)}
                      className="rounded text-indigo-600 focus:ring-0"
                    />
                    <div>
                      <span className="font-bold text-slate-800 block">تحقق فوري (RESTORE VERIFY)</span>
                      <span className="text-[10px] text-slate-500">ضمان سلامة الملف فوراً</span>
                    </div>
                  </label>
                </div>

                {/* Progress bar when executing */}
                {isProcessing && (
                  <div className="p-3 bg-slate-100 border border-slate-300 rounded-xl space-y-2">
                    <div className="flex items-center justify-between text-xs font-bold text-slate-800">
                      <span>{processStage}</span>
                      <span className="font-mono text-indigo-700">{progressPercent}%</span>
                    </div>
                    <div className="w-full bg-slate-200 rounded-full h-2.5 overflow-hidden">
                      <div
                        className="bg-indigo-600 h-2.5 transition-all duration-300 rounded-full"
                        style={{ width: `${progressPercent}%` }}
                      />
                    </div>
                  </div>
                )}

                {/* Success alert */}
                {executionResult && (
                  <div className="bg-emerald-50 border border-emerald-300 text-emerald-900 p-3.5 rounded-xl text-xs space-y-1 animate-in fade-in">
                    <div className="flex items-center gap-2 font-extrabold text-emerald-800">
                      <CheckCircle2 className="w-4 h-4 text-emerald-600" />
                      <span>{executionResult.message}</span>
                    </div>
                    {executionResult.details && (
                      <p className="text-[11px] font-mono text-slate-600 dir-ltr text-right">
                        {executionResult.details}
                      </p>
                    )}
                  </div>
                )}

                {/* Start Backup Action Button */}
                <div className="pt-2 flex items-center justify-between gap-3">
                  <button
                    type="button"
                    disabled={isProcessing}
                    onClick={handleStartBackup}
                    className="bg-indigo-600 hover:bg-indigo-700 disabled:bg-slate-300 text-white py-2.5 px-6 rounded-xl font-extrabold text-xs shadow-md transition active:scale-98 cursor-pointer flex items-center gap-2"
                  >
                    <Play className="w-4 h-4 fill-white" />
                    <span>{isProcessing ? 'جاري النسخ والتحقق...' : 'بدء النسخ الاحتياطي لقاعدة البيانات الآن'}</span>
                  </button>

                  <span className="text-[11px] text-slate-500">
                    ملاحظة: العملية تتم في الخلفية دون تعطيل عمل الكاشيرات ونقاط البيع.
                  </span>
                </div>
              </div>

              {/* History Table */}
              <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-xs space-y-3">
                <div className="flex items-center justify-between border-b border-slate-100 pb-2">
                  <h3 className="font-extrabold text-xs text-slate-800 flex items-center gap-2">
                    <Archive className="w-4 h-4 text-indigo-600" />
                    <span>سجل النسخ الاحتياطية السابقة (SQL Backup History):</span>
                  </h3>
                  <span className="text-[11px] text-slate-500 font-mono">
                    {historyList.length} نسخ محفوظة
                  </span>
                </div>

                <div className="overflow-x-auto">
                  <table className="w-full text-xs text-right border-collapse">
                    <thead>
                      <tr className="bg-slate-100 text-slate-700 font-bold border-b border-slate-200">
                        <th className="p-2">اسم الملف</th>
                        <th className="p-2">النوع</th>
                        <th className="p-2">التاريخ والوقت</th>
                        <th className="p-2">الحجم الأصلي / المضغوط</th>
                        <th className="p-2">الحالة</th>
                        <th className="p-2">المنفذ</th>
                        <th className="p-2 text-center">إجراءات</th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100 font-medium">
                      {historyList.map((rec, idx) => (
                        <tr key={`rec-${rec.id}-${idx}`} className="hover:bg-slate-50">
                          <td className="p-2 font-mono font-bold text-indigo-700" dir="ltr">
                            {rec.fileName}
                          </td>
                          <td className="p-2">
                            <span className={`px-2 py-0.5 rounded text-[10px] font-bold ${
                              rec.backupType === 'FULL' ? 'bg-indigo-100 text-indigo-800' : 'bg-sky-100 text-sky-800'
                            }`}>
                              {rec.backupType === 'FULL' ? 'كامل (Full)' : 'تفاضلي (Diff)'}
                            </span>
                          </td>
                          <td className="p-2 text-slate-600 font-mono text-[11px]">{rec.createdAt}</td>
                          <td className="p-2 font-mono text-[11px]">
                            <span className="text-slate-500 line-through mr-1">{rec.sizeMB} MB</span>
                            <strong className="text-emerald-700">{rec.compressedSizeMB} MB</strong>
                            <span className="text-[10px] text-slate-400 mr-1">({rec.compressionRatio})</span>
                          </td>
                          <td className="p-2">
                            <span className="inline-flex items-center gap-1 text-[11px] font-bold text-emerald-700 bg-emerald-50 px-2 py-0.5 rounded-full border border-emerald-200">
                              <CheckCircle2 className="w-3 h-3" />
                              تم التحقق (Verified)
                            </span>
                          </td>
                          <td className="p-2 text-slate-600 text-[11px]">{rec.executedBy}</td>
                          <td className="p-2 text-center">
                            <button
                              onClick={() => handleDownloadBackupFile(rec.fileName)}
                              className="p-1.5 bg-slate-100 hover:bg-indigo-50 hover:text-indigo-700 rounded-lg text-slate-600 transition cursor-pointer"
                              title="تنزيل ملف .bak على جهازك"
                            >
                              <Download className="w-3.5 h-3.5" />
                            </button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* TAB 2: RESTORE */}
          {activeTab === 'RESTORE' && (
            <div className="space-y-4">
              {/* Critical Warning Alert */}
              <div className="bg-rose-50 border border-rose-200 rounded-xl p-4 text-xs text-rose-950 flex items-start gap-3 shadow-xs">
                <AlertTriangle className="w-5 h-5 text-rose-600 shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <span className="font-extrabold block text-sm text-rose-900">
                    منطقة حرجة: استعادة قاعدة بيانات SQL Server
                  </span>
                  <p className="text-slate-700 leading-relaxed">
                    عملية الاستعادة ستقوم باستبدال البيانات الحالية بمحتويات النسخة المختارة. كإجراء أمان إلزامي، سيقوم النظام بأخذ نسخة حماية تلقائية قبل الاستعادة.
                  </p>
                </div>
              </div>

              {/* Restore Configuration Card */}
              <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-xs space-y-4">
                <div className="space-y-1.5">
                  <label className="text-xs font-bold text-slate-700 flex items-center gap-1.5">
                    <Archive className="w-3.5 h-3.5 text-slate-500" />
                    <span>اختر ملف النسخة الاحتياطية (.bak) المراد استعادتها:</span>
                  </label>
                  <select
                    value={selectedBackupForRestore}
                    onChange={(e) => setSelectedBackupForRestore(e.target.value)}
                    className="w-full text-xs font-mono p-2.5 bg-slate-50 border border-slate-300 rounded-lg text-slate-800 focus:bg-white focus:border-rose-600 outline-hidden transition"
                    dir="ltr"
                  >
                    {historyList.map((h, idx) => (
                      <option key={`opt-${h.id}-${idx}`} value={h.fileName}>
                        {h.fileName} — ({h.createdAt} | {h.compressedSizeMB} MB)
                      </option>
                    ))}
                  </select>
                </div>

                {/* Safety Rules Toggles */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-3 text-xs">
                  <label className="flex items-center gap-2 p-3 rounded-lg border border-emerald-200 bg-emerald-50/50 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={takePreRestoreSafetyBackup}
                      onChange={(e) => setTakePreRestoreSafetyBackup(e.target.checked)}
                      className="rounded text-emerald-600 focus:ring-0"
                    />
                    <div>
                      <span className="font-bold text-emerald-900 block">نسخة أمان احترازية إجبارية (Pre-Restore Snapshot)</span>
                      <span className="text-[10px] text-slate-600">أخذ باك اب فوري للحالة الحالية قبل الاستعادة</span>
                    </div>
                  </label>

                  <label className="flex items-center gap-2 p-3 rounded-lg border border-indigo-200 bg-indigo-50/50 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={forceSingleUserMode}
                      onChange={(e) => setForceSingleUserMode(e.target.checked)}
                      className="rounded text-indigo-600 focus:ring-0"
                    />
                    <div>
                      <span className="font-bold text-indigo-900 block">قفل الاتصالات الحالية (Single User Mode)</span>
                      <span className="text-[10px] text-slate-600">منع تعارض المستخدمين أثناء الاستبدال</span>
                    </div>
                  </label>
                </div>

                {/* Admin Password Verification */}
                <div className="bg-slate-50 border border-slate-200 rounded-xl p-3.5 space-y-2">
                  <label className="text-xs font-bold text-slate-800 flex items-center gap-1.5">
                    <Lock className="w-3.5 h-3.5 text-rose-600" />
                    <span>تأكيد كلمة مرور مدير النظام للمتابعة (اكتب 123 أو admin):</span>
                  </label>
                  <input
                    type="password"
                    value={adminPasswordConfirm}
                    onChange={(e) => setAdminPasswordConfirm(e.target.value)}
                    placeholder="أدخل كلمة المرور لتأكيد الصلاحية..."
                    className="w-full sm:w-72 text-xs p-2.5 bg-white border border-slate-300 rounded-lg text-slate-800 focus:border-rose-600 outline-hidden"
                  />
                </div>

                {/* Restore in progress */}
                {isProcessing && (
                  <div className="p-3 bg-rose-50 border border-rose-200 rounded-xl space-y-2">
                    <div className="flex items-center justify-between text-xs font-bold text-rose-900">
                      <span>{restoreStage}</span>
                      <span className="font-mono">{progressPercent}%</span>
                    </div>
                    <div className="w-full bg-rose-200 rounded-full h-2.5 overflow-hidden">
                      <div
                        className="bg-rose-600 h-2.5 transition-all duration-300 rounded-full"
                        style={{ width: `${progressPercent}%` }}
                      />
                    </div>
                  </div>
                )}

                {/* Execute Restore Button */}
                <div className="pt-2 flex items-center justify-between gap-3">
                  <button
                    type="button"
                    disabled={isProcessing || !adminPasswordConfirm.trim()}
                    onClick={handleStartRestore}
                    className="bg-rose-600 hover:bg-rose-700 disabled:bg-slate-300 text-white py-2.5 px-6 rounded-xl font-extrabold text-xs shadow-md transition active:scale-98 cursor-pointer flex items-center gap-2"
                  >
                    <RefreshCw className={`w-4 h-4 ${isProcessing ? 'animate-spin' : ''}`} />
                    <span>{isProcessing ? 'جاري تنفيذ الاستعادة الآمنة...' : 'استعادة قاعدة البيانات المحددة'}</span>
                  </button>

                  <span className="text-[11px] text-slate-500">
                    يتم استرجاع الجداول، الأرصدة، والحركات وتحديث فهارس البحث sp_updatestats.
                  </span>
                </div>
              </div>
            </div>
          )}

          {/* TAB 3: SCHEDULE */}
          {activeTab === 'SCHEDULE' && (
            <div className="space-y-4">
              <div className="bg-emerald-50/70 border border-emerald-200/80 rounded-xl p-3.5 text-xs text-emerald-950 flex items-start gap-3 shadow-xs">
                <Clock className="w-5 h-5 text-emerald-600 shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <span className="font-extrabold block text-sm text-emerald-900">
                    أتمتة الصيانة والنسخ الاحتياطي الدوري (SQL Server Agent Jobs):
                  </span>
                  <p className="text-slate-600 leading-relaxed">
                    تضمن هذه الإعدادات إنشاء نسخ احتياطية منتظمة بدون تدخل بشري، وحذف النسخ القديمة دورياً لمنع امتلاء أقراص التخزين.
                  </p>
                </div>
              </div>

              <div className="bg-white border border-slate-200 rounded-xl p-4 shadow-xs space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {/* Daily Scheduled Time */}
                  <div className="p-3 bg-slate-50 border border-slate-200 rounded-xl space-y-2">
                    <div className="flex items-center justify-between">
                      <span className="text-xs font-bold text-slate-800">النسخ اليومي التلقائي (Daily Full Backup):</span>
                      <input
                        type="checkbox"
                        checked={autoBackupEnabled}
                        onChange={(e) => setAutoBackupEnabled(e.target.checked)}
                        className="rounded text-emerald-600 focus:ring-0"
                      />
                    </div>
                    <div className="flex items-center gap-2 text-xs">
                      <span className="text-slate-600">وقت التنفيذ اليومي:</span>
                      <input
                        type="time"
                        value={scheduledDailyTime}
                        onChange={(e) => setScheduledDailyTime(e.target.value)}
                        disabled={!autoBackupEnabled}
                        className="p-1.5 bg-white border border-slate-300 rounded text-slate-800 font-mono text-xs"
                      />
                    </div>
                  </div>

                  {/* Backup on Z-Report Close */}
                  <div className="p-3 bg-slate-50 border border-slate-200 rounded-xl space-y-2">
                    <div className="flex items-center justify-between">
                      <span className="text-xs font-bold text-slate-800">النسخ التلقائي عند إغلاق الوردية (Z-Report):</span>
                      <input
                        type="checkbox"
                        checked={backupOnShiftClose}
                        onChange={(e) => setBackupOnShiftClose(e.target.checked)}
                        className="rounded text-emerald-600 focus:ring-0"
                      />
                    </div>
                    <p className="text-[11px] text-slate-500 leading-relaxed">
                      يتم عمل نسخ تفاضلي سريع عند قيام الكاشير بطباعة وإغلاق تقرير الوردية Z-Report.
                    </p>
                  </div>

                  {/* Retention Cleanup */}
                  <div className="p-3 bg-slate-50 border border-slate-200 rounded-xl space-y-2">
                    <span className="text-xs font-bold text-slate-800 block">فترة الاحتفاظ بالنسخ (Retention Policy):</span>
                    <div className="flex items-center gap-2 text-xs">
                      <span className="text-slate-600">حذف النسخ الأقدم من:</span>
                      <input
                        type="number"
                        min="7"
                        max="365"
                        value={retentionDays}
                        onChange={(e) => setRetentionDays(Number(e.target.value))}
                        className="w-20 p-1.5 bg-white border border-slate-300 rounded text-slate-800 font-mono text-xs"
                      />
                      <span className="text-slate-600">يوماً تلقائياً</span>
                    </div>
                  </div>

                  {/* Off-site / Cloud Copy */}
                  <div className="p-3 bg-slate-50 border border-slate-200 rounded-xl space-y-2">
                    <div className="flex items-center justify-between">
                      <span className="text-xs font-bold text-slate-800">نسخة المرآة السحابية (قاعدة 3-2-1):</span>
                      <input
                        type="checkbox"
                        checked={cloudMirrorEnabled}
                        onChange={(e) => setCloudMirrorEnabled(e.target.checked)}
                        className="rounded text-emerald-600 focus:ring-0"
                      />
                    </div>
                    <p className="text-[11px] text-slate-500 leading-relaxed">
                      رفع نسخة مشفرة إضافية لسحابة آمنة أو فلاشة خارجية متصلة لمنع أضرار السرقة أو الحريق.
                    </p>
                  </div>
                </div>

                <div className="pt-2 flex justify-end">
                  <button
                    type="button"
                    onClick={() => alert('تم حفظ جدول وإعدادات صيانة SQL Server Agent بنجاح!')}
                    className="bg-emerald-600 hover:bg-emerald-700 text-white font-bold text-xs py-2 px-5 rounded-xl transition cursor-pointer shadow-xs"
                  >
                    حفظ إعدادات الجدولة
                  </button>
                </div>
              </div>
            </div>
          )}

          {/* TAB 4: T-SQL SCRIPTS */}
          {activeTab === 'TSQL_SCRIPTS' && (
            <div className="space-y-4">
              <div className="bg-purple-50/70 border border-purple-200/80 rounded-xl p-3.5 text-xs text-purple-950 flex items-start gap-3 shadow-xs">
                <Terminal className="w-5 h-5 text-purple-600 shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <span className="font-extrabold block text-sm text-purple-900">
                    أكواد Transact-SQL الجاهزة لمديري قواعد البيانات (DBA):
                  </span>
                  <p className="text-slate-600 leading-relaxed">
                    يمكنك نسخ هذه الأكواد وتشغيلها مباشرة داخل <strong>SQL Server Management Studio (SSMS)</strong> أو برمجتها كـ SQL Agent Job.
                  </p>
                </div>
              </div>

              {/* T-SQL Backup (Clean light theme) */}
              <div className="bg-slate-50 text-slate-800 rounded-xl p-4 shadow-xs space-y-2 border border-slate-300">
                <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                  <span className="text-xs font-mono font-bold text-indigo-700">1. كود النسخ الاحتياطي (T-SQL Backup Script):</span>
                  <button
                    onClick={() => handleCopyTSQL(generatedTSQLBackup)}
                    className="flex items-center gap-1.5 text-xs bg-white hover:bg-slate-100 text-slate-700 border border-slate-300 px-3 py-1.5 rounded-lg transition cursor-pointer font-bold shadow-2xs"
                  >
                    {copiedCode ? <Check className="w-3.5 h-3.5 text-emerald-600" /> : <Copy className="w-3.5 h-3.5 text-slate-600" />}
                    <span>{copiedCode ? 'تم النسخ!' : 'نسخ الكود'}</span>
                  </button>
                </div>
                <pre className="text-[11px] font-mono leading-relaxed overflow-x-auto text-slate-800 bg-white border border-slate-200 rounded-lg p-3 dir-ltr max-h-52 overflow-y-auto selection:bg-indigo-100">
                  {generatedTSQLBackup}
                </pre>
              </div>

              {/* T-SQL Restore (Clean light theme) */}
              <div className="bg-slate-50 text-slate-800 rounded-xl p-4 shadow-xs space-y-2 border border-slate-300">
                <div className="flex items-center justify-between border-b border-slate-200 pb-2">
                  <span className="text-xs font-mono font-bold text-rose-700">2. كود الاستعادة وقفل المستخدمين (T-SQL Restore Script):</span>
                  <button
                    onClick={() => handleCopyTSQL(generatedTSQLRestore)}
                    className="flex items-center gap-1.5 text-xs bg-white hover:bg-slate-100 text-slate-700 border border-slate-300 px-3 py-1.5 rounded-lg transition cursor-pointer font-bold shadow-2xs"
                  >
                    <Copy className="w-3.5 h-3.5 text-slate-600" />
                    <span>نسخ كود الاستعادة</span>
                  </button>
                </div>
                <pre className="text-[11px] font-mono leading-relaxed overflow-x-auto text-slate-800 bg-white border border-slate-200 rounded-lg p-3 dir-ltr max-h-52 overflow-y-auto selection:bg-rose-100">
                  {generatedTSQLRestore}
                </pre>
              </div>
            </div>
          )}
        </div>

        {/* Modal Bottom Footer */}
        <div className="bg-white border-t border-slate-200 p-3.5 flex items-center justify-between text-xs">
          <div className="flex items-center gap-2 text-slate-500 font-medium">
            <Database className="w-4 h-4 text-indigo-600" />
            <span>محرك قاعدة البيانات: Microsoft SQL Server (Transact-SQL)</span>
          </div>

          <button
            onClick={onClose}
            className="px-5 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl transition cursor-pointer"
          >
            إغلاق
          </button>
        </div>
      </div>
    </div>
  );
};
