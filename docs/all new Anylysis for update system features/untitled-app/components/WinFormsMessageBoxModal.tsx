'use client';

import React, { useEffect, useRef, useState } from 'react';
import { 
  AlertCircle, 
  AlertTriangle, 
  HelpCircle, 
  CheckCircle2, 
  Info, 
  X, 
  ChevronDown, 
  ChevronUp, 
  Copy, 
  Check, 
  Volume2, 
  VolumeX,
  CornerDownLeft,
  ArrowRight
} from 'lucide-react';

export type MessageBoxType = 'ERROR' | 'WARNING' | 'QUESTION' | 'INFO' | 'SUCCESS';
export type MessageBoxButtons = 'OK' | 'YES_NO' | 'OK_CANCEL' | 'YES_NO_CANCEL' | 'RETRY_CANCEL';
export type DialogResult = 'OK' | 'YES' | 'NO' | 'CANCEL' | 'RETRY';

export interface WinFormsDialogOptions {
  id?: string;
  title?: string;
  message: string;
  type?: MessageBoxType;
  buttons?: MessageBoxButtons;
  defaultButton?: 'PRIMARY' | 'SECONDARY' | 'TERTIARY';
  targetControlId?: string; // Target input or control to focus & highlight on dismissal
  technicalDetails?: string; // Optional stack trace / SQL exception / calculation breakdown
  soundEnabled?: boolean;
  onResult?: (result: DialogResult) => void;
}

interface WinFormsMessageBoxModalProps {
  dialog: WinFormsDialogOptions | null;
  onClose: (result: DialogResult) => void;
}

// Subtle Web Audio API WinForms system sounds synthesizer
function playSystemBeep(type: MessageBoxType) {
  try {
    const AudioCtx = window.AudioContext || (window as unknown as { webkitAudioContext: typeof AudioContext }).webkitAudioContext;
    if (!AudioCtx) return;
    const ctx = new AudioCtx();
    
    if (type === 'ERROR') {
      // WinForms Critical Stop double-tone
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();
      osc.type = 'sawtooth';
      osc.frequency.setValueAtTime(150, ctx.currentTime);
      osc.frequency.exponentialRampToValueAtTime(80, ctx.currentTime + 0.15);
      gain.gain.setValueAtTime(0.12, ctx.currentTime);
      gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + 0.18);
      osc.connect(gain);
      gain.connect(ctx.destination);
      osc.start();
      osc.stop(ctx.currentTime + 0.2);
    } else if (type === 'WARNING' || type === 'QUESTION') {
      // WinForms Exclamation / Question tone
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();
      osc.type = 'sine';
      osc.frequency.setValueAtTime(440, ctx.currentTime);
      osc.frequency.setValueAtTime(660, ctx.currentTime + 0.08);
      gain.gain.setValueAtTime(0.1, ctx.currentTime);
      gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + 0.2);
      osc.connect(gain);
      gain.connect(ctx.destination);
      osc.start();
      osc.stop(ctx.currentTime + 0.22);
    } else {
      // WinForms Asterisk / Info pleasant chime
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();
      osc.type = 'sine';
      osc.frequency.setValueAtTime(587.33, ctx.currentTime); // D5
      osc.frequency.setValueAtTime(880, ctx.currentTime + 0.06); // A5
      gain.gain.setValueAtTime(0.08, ctx.currentTime);
      gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + 0.25);
      osc.connect(gain);
      gain.connect(ctx.destination);
      osc.start();
      osc.stop(ctx.currentTime + 0.26);
    }
  } catch {
    // Audio contexts might be blocked until user gesture, safely ignore
  }
}

// Function to focus and highlight target control
export function highlightAndFocusControl(elementId: string, isError = true) {
  if (typeof document === 'undefined' || !elementId) return;
  
  setTimeout(() => {
    const el = document.getElementById(elementId) as HTMLElement | HTMLInputElement | null;
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
      
      if ('select' in el && typeof el.select === 'function') {
        el.select();
      }

      // Add visual feedback class
      const highlightClass = isError ? 'winforms-error-highlight' : 'winforms-success-highlight';
      el.classList.add(highlightClass);
      
      // Auto-remove highlight after 3.5 seconds
      setTimeout(() => {
        el.classList.remove(highlightClass);
      }, 3500);
    }
  }, 100);
}

export const WinFormsMessageBoxModal: React.FC<WinFormsMessageBoxModalProps> = ({
  dialog,
  onClose,
}) => {
  const [showDetails, setShowDetails] = useState(false);
  const [copied, setCopied] = useState(false);
  const primaryBtnRef = useRef<HTMLButtonElement>(null);
  const secondaryBtnRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    if (dialog) {
      setShowDetails(false);
      setCopied(false);
      
      // Play desktop beep if enabled
      if (dialog.soundEnabled !== false) {
        playSystemBeep(dialog.type || 'INFO');
      }

      // Auto-focus primary button
      setTimeout(() => {
        primaryBtnRef.current?.focus();
      }, 50);
    }
  }, [dialog]);

  // Handle Keyboard Shortcuts (Enter, Escape, Arrows)
  useEffect(() => {
    if (!dialog) return;

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        e.preventDefault();
        handleResult(dialog.buttons === 'YES_NO' ? 'NO' : 'CANCEL');
      } else if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        if (dialog.buttons === 'YES_NO') {
          handleResult('YES');
        } else if (dialog.buttons === 'RETRY_CANCEL') {
          handleResult('RETRY');
        } else {
          handleResult('OK');
        }
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [dialog]);

  if (!dialog) return null;

  const type = dialog.type || 'INFO';
  const buttons = dialog.buttons || 'OK';

  const defaultTitles: Record<MessageBoxType, string> = {
    ERROR: 'تنبيه خطأ في النظام - Error',
    WARNING: 'تحذير تشغيلي - Warning',
    QUESTION: 'تأكيد العملية - Confirmation',
    INFO: 'معلومات النظام - Information',
    SUCCESS: 'نجاح العملية - Success',
  };

  const title = dialog.title || defaultTitles[type];

  const handleResult = (result: DialogResult) => {
    // If target control was specified, trigger focus & highlight
    if (dialog.targetControlId) {
      highlightAndFocusControl(dialog.targetControlId, type === 'ERROR' || type === 'WARNING');
    }
    
    if (dialog.onResult) {
      dialog.onResult(result);
    }
    onClose(result);
  };

  const handleCopyDetails = () => {
    if (!dialog.technicalDetails) return;
    navigator.clipboard.writeText(dialog.technicalDetails);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  // Header and Icon Styling
  const getIcon = () => {
    switch (type) {
      case 'ERROR':
        return (
          <div className="w-11 h-11 rounded-full bg-rose-100 border-2 border-rose-300 flex items-center justify-center text-rose-600 shadow-xs shrink-0 animate-in zoom-in-75 duration-150">
            <AlertCircle className="w-7 h-7 stroke-[2.5]" />
          </div>
        );
      case 'WARNING':
        return (
          <div className="w-11 h-11 rounded-full bg-amber-100 border-2 border-amber-300 flex items-center justify-center text-amber-600 shadow-xs shrink-0 animate-in zoom-in-75 duration-150">
            <AlertTriangle className="w-7 h-7 stroke-[2.5]" />
          </div>
        );
      case 'QUESTION':
        return (
          <div className="w-11 h-11 rounded-full bg-sky-100 border-2 border-sky-300 flex items-center justify-center text-sky-600 shadow-xs shrink-0 animate-in zoom-in-75 duration-150">
            <HelpCircle className="w-7 h-7 stroke-[2.5]" />
          </div>
        );
      case 'SUCCESS':
        return (
          <div className="w-11 h-11 rounded-full bg-emerald-100 border-2 border-emerald-300 flex items-center justify-center text-emerald-600 shadow-xs shrink-0 animate-in zoom-in-75 duration-150">
            <CheckCircle2 className="w-7 h-7 stroke-[2.5]" />
          </div>
        );
      case 'INFO':
      default:
        return (
          <div className="w-11 h-11 rounded-full bg-blue-100 border-2 border-blue-300 flex items-center justify-center text-blue-600 shadow-xs shrink-0 animate-in zoom-in-75 duration-150">
            <Info className="w-7 h-7 stroke-[2.5]" />
          </div>
        );
    }
  };

  return (
    <div className="fixed inset-0 z-[9999] bg-black/60 backdrop-blur-xs flex items-center justify-center p-4 select-none animate-in fade-in duration-100">
      <div 
        role="dialog" 
        aria-modal="true"
        aria-labelledby="winforms-dialog-title"
        className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-lg overflow-hidden animate-in zoom-in-95 duration-150 flex flex-col"
      >
        {/* WinForms Classic Window Header */}
        <div className="bg-slate-100 border-b border-slate-300 px-4 py-2.5 flex items-center justify-between text-slate-800">
          <div className="flex items-center gap-2">
            <div className={`w-2.5 h-2.5 rounded-full ${
              type === 'ERROR' ? 'bg-rose-500' :
              type === 'WARNING' ? 'bg-amber-500' :
              type === 'QUESTION' ? 'bg-sky-500' :
              type === 'SUCCESS' ? 'bg-emerald-500' : 'bg-blue-500'
            }`} />
            <h3 id="winforms-dialog-title" className="font-bold text-xs text-slate-800 tracking-wide">
              {title}
            </h3>
          </div>
          <button
            onClick={() => handleResult(buttons === 'YES_NO' ? 'NO' : 'CANCEL')}
            title="إغلاق (Esc)"
            className="text-slate-400 hover:text-slate-700 hover:bg-slate-200 p-1 rounded transition cursor-pointer"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* Dialog Content Area */}
        <div className="p-5 flex items-start gap-4 bg-white">
          {getIcon()}
          
          <div className="flex-1 min-w-0 space-y-2">
            <p className="text-sm font-semibold text-slate-900 leading-relaxed whitespace-pre-line text-right">
              {dialog.message}
            </p>

            {/* Target Control Focus Notification Badge */}
            {dialog.targetControlId && (
              <div className="flex items-center gap-1.5 text-[11px] text-sky-700 bg-sky-50 border border-sky-200 px-2.5 py-1 rounded-md mt-2 w-fit">
                <ArrowRight className="w-3.5 h-3.5 shrink-0" />
                <span>سيتم الانتقال تلقائياً والتركيز على حقل الإدخال: <strong className="font-mono text-sky-900">#{dialog.targetControlId}</strong></span>
              </div>
            )}

            {/* Collapsible Technical Details (e.g. for SQL/Accounting exception logs) */}
            {dialog.technicalDetails && (
              <div className="pt-2">
                <button
                  type="button"
                  onClick={() => setShowDetails(!showDetails)}
                  className="text-xs text-slate-500 hover:text-slate-800 font-medium flex items-center gap-1 cursor-pointer transition"
                >
                  {showDetails ? <ChevronUp className="w-3.5 h-3.5" /> : <ChevronDown className="w-3.5 h-3.5" />}
                  <span>{showDetails ? 'إخفاء التفاصيل الفنية' : 'عرض التفاصيل الفنية (Exception Details)'}</span>
                </button>

                {showDetails && (
                  <div className="mt-2 p-3 bg-slate-900 text-slate-200 rounded-lg text-xs font-mono border border-slate-700 relative overflow-x-auto max-h-40">
                    <button
                      onClick={handleCopyDetails}
                      title="نسخ التفاصيل"
                      className="absolute top-2 left-2 p-1 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded border border-slate-600 cursor-pointer flex items-center gap-1 text-[10px]"
                    >
                      {copied ? <Check className="w-3 h-3 text-emerald-400" /> : <Copy className="w-3 h-3" />}
                      <span>{copied ? 'تم النسخ' : 'نسخ'}</span>
                    </button>
                    <pre className="whitespace-pre-wrap text-[11px] leading-relaxed select-text">
                      {dialog.technicalDetails}
                    </pre>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>

        {/* WinForms Action Buttons Footer */}
        <div className="bg-slate-50 border-t border-slate-200 px-4 py-3 flex items-center justify-between gap-2">
          <div className="flex items-center gap-1.5 text-[11px] text-slate-400">
            <CornerDownLeft className="w-3.5 h-3.5" />
            <span>Enter للموافقة | Esc للإلغاء</span>
          </div>

          <div className="flex items-center gap-2">
            {buttons === 'OK' && (
              <button
                ref={primaryBtnRef}
                onClick={() => handleResult('OK')}
                className={`px-5 py-1.5 rounded-lg text-xs font-bold transition shadow-xs cursor-pointer focus:ring-2 focus:ring-offset-1 ${
                  type === 'ERROR'
                    ? 'bg-rose-600 hover:bg-rose-700 text-white focus:ring-rose-500'
                    : type === 'SUCCESS'
                    ? 'bg-emerald-600 hover:bg-emerald-700 text-white focus:ring-emerald-500'
                    : 'bg-[#0f2d4a] hover:bg-[#143d63] text-white focus:ring-sky-500'
                }`}
              >
                موافق (OK)
              </button>
            )}

            {buttons === 'OK_CANCEL' && (
              <>
                <button
                  ref={secondaryBtnRef}
                  onClick={() => handleResult('CANCEL')}
                  className="px-4 py-1.5 bg-white border border-slate-300 hover:bg-slate-100 text-slate-700 rounded-lg text-xs font-semibold transition cursor-pointer shadow-2xs"
                >
                  إلغاء الأمر (Cancel)
                </button>
                <button
                  ref={primaryBtnRef}
                  onClick={() => handleResult('OK')}
                  className="px-5 py-1.5 bg-[#0f2d4a] hover:bg-[#143d63] text-white rounded-lg text-xs font-bold transition shadow-xs cursor-pointer focus:ring-2 focus:ring-sky-500"
                >
                  موافق (OK)
                </button>
              </>
            )}

            {buttons === 'YES_NO' && (
              <>
                <button
                  ref={secondaryBtnRef}
                  onClick={() => handleResult('NO')}
                  className="px-4 py-1.5 bg-white border border-slate-300 hover:bg-slate-100 text-slate-700 rounded-lg text-xs font-semibold transition cursor-pointer shadow-2xs"
                >
                  لا (No)
                </button>
                <button
                  ref={primaryBtnRef}
                  onClick={() => handleResult('YES')}
                  className="px-5 py-1.5 bg-sky-600 hover:bg-sky-700 text-white rounded-lg text-xs font-bold transition shadow-xs cursor-pointer focus:ring-2 focus:ring-sky-500"
                >
                  نعم (Yes)
                </button>
              </>
            )}

            {buttons === 'YES_NO_CANCEL' && (
              <>
                <button
                  onClick={() => handleResult('CANCEL')}
                  className="px-3 py-1.5 bg-white border border-slate-300 hover:bg-slate-100 text-slate-600 rounded-lg text-xs font-medium transition cursor-pointer shadow-2xs"
                >
                  إلغاء (Cancel)
                </button>
                <button
                  ref={secondaryBtnRef}
                  onClick={() => handleResult('NO')}
                  className="px-4 py-1.5 bg-white border border-slate-300 hover:bg-slate-100 text-slate-700 rounded-lg text-xs font-semibold transition cursor-pointer shadow-2xs"
                >
                  لا (No)
                </button>
                <button
                  ref={primaryBtnRef}
                  onClick={() => handleResult('YES')}
                  className="px-5 py-1.5 bg-sky-600 hover:bg-sky-700 text-white rounded-lg text-xs font-bold transition shadow-xs cursor-pointer focus:ring-2 focus:ring-sky-500"
                >
                  نعم (Yes)
                </button>
              </>
            )}

            {buttons === 'RETRY_CANCEL' && (
              <>
                <button
                  ref={secondaryBtnRef}
                  onClick={() => handleResult('CANCEL')}
                  className="px-4 py-1.5 bg-white border border-slate-300 hover:bg-slate-100 text-slate-700 rounded-lg text-xs font-semibold transition cursor-pointer shadow-2xs"
                >
                  إلغاء (Cancel)
                </button>
                <button
                  ref={primaryBtnRef}
                  onClick={() => handleResult('RETRY')}
                  className="px-5 py-1.5 bg-amber-600 hover:bg-amber-700 text-white rounded-lg text-xs font-bold transition shadow-xs cursor-pointer focus:ring-2 focus:ring-amber-500"
                >
                  إعادة المحاولة (Retry)
                </button>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
