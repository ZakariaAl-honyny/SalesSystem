'use client';

import React, { useEffect, useRef } from 'react';
import { 
  Edit3, 
  Trash2, 
  Copy, 
  ArrowUp, 
  ArrowDown, 
  Tag, 
  Search, 
  PlusCircle, 
  RefreshCw,
  Info,
  DollarSign,
  FileText
} from 'lucide-react';

export interface DGVContextMenuItem {
  id: string;
  label: string;
  icon?: React.ReactNode;
  shortcut?: string;
  danger?: boolean;
  disabled?: boolean;
  action: () => void;
}

export interface WinFormsDGVContextMenuProps {
  x: number;
  y: number;
  visible: boolean;
  rowIndex?: number;
  rowTitle?: string;
  items?: DGVContextMenuItem[];
  onClose: () => void;
}

export const WinFormsDGVContextMenu: React.FC<WinFormsDGVContextMenuProps> = ({
  x,
  y,
  visible,
  rowIndex,
  rowTitle,
  items = [],
  onClose,
}) => {
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        onClose();
      }
    };

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        onClose();
      }
    };

    if (visible) {
      document.addEventListener('mousedown', handleClickOutside);
      document.addEventListener('keydown', handleKeyDown);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      document.removeEventListener('keydown', handleKeyDown);
    };
  }, [visible, onClose]);

  if (!visible) return null;

  // Ensure menu stays within window bounds
  const menuWidth = 220;
  const menuHeight = items.length * 32 + 40;
  
  let posX = x;
  let posY = y;

  if (typeof window !== 'undefined') {
    if (x + menuWidth > window.innerWidth) {
      posX = Math.max(10, x - menuWidth);
    }
    if (y + menuHeight > window.innerHeight) {
      posY = Math.max(10, y - menuHeight);
    }
  }

  return (
    <div
      ref={menuRef}
      style={{ top: `${posY}px`, left: `${posX}px` }}
      className="fixed z-[9999] bg-white border border-slate-300 rounded-lg shadow-2xl py-1 w-56 text-xs text-slate-800 font-sans select-none animate-in fade-in zoom-in-95 duration-100 ring-1 ring-black/10"
      dir="rtl"
    >
      {/* ContextMenu Header displaying Row Context */}
      <div className="px-3 py-1.5 bg-slate-100/90 border-b border-slate-200 text-[11px] font-bold text-slate-700 flex items-center justify-between">
        <span className="truncate max-w-[150px]">
          {rowTitle ? `سطر #${(rowIndex ?? 0) + 1}: ${rowTitle}` : `سطر الجدول #${(rowIndex ?? 0) + 1}`}
        </span>
        <span className="text-[10px] text-sky-700 bg-sky-100 px-1.5 py-0.2 rounded font-mono">DGV</span>
      </div>

      <div className="py-1 divide-y divide-slate-100">
        {items.map((item) => (
          <button
            key={item.id}
            disabled={item.disabled}
            onClick={() => {
              item.action();
              onClose();
            }}
            className={`w-full text-right px-3 py-1.5 flex items-center justify-between hover:bg-sky-50 transition cursor-pointer ${
              item.danger
                ? 'text-rose-700 hover:bg-rose-50'
                : item.disabled
                ? 'opacity-40 cursor-not-allowed text-slate-400'
                : 'text-slate-800'
            }`}
          >
            <div className="flex items-center gap-2">
              <span className="text-slate-500">{item.icon || <Info className="w-3.5 h-3.5" />}</span>
              <span className="font-medium">{item.label}</span>
            </div>
            {item.shortcut && (
              <span className="text-[10px] text-slate-400 font-mono mr-2 bg-slate-100 px-1 py-0.5 rounded border border-slate-200">
                {item.shortcut}
              </span>
            )}
          </button>
        ))}
      </div>
    </div>
  );
};
