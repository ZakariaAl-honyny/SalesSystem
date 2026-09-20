'use client';

import React, { useState } from 'react';
import { Delete, RotateCcw } from 'lucide-react';

interface CalculatorModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export const CalculatorModal: React.FC<CalculatorModalProps> = ({ isOpen, onClose }) => {
  const [display, setDisplay] = useState<string>('0');
  const [prevValue, setPrevValue] = useState<number | null>(null);
  const [operation, setOperation] = useState<string | null>(null);
  const [clearOnNextDigit, setClearOnNextDigit] = useState<boolean>(false);

  if (!isOpen) return null;

  const handleDigit = (digit: string) => {
    if (display === '0' || clearOnNextDigit) {
      setDisplay(digit);
      setClearOnNextDigit(false);
    } else {
      setDisplay(display + digit);
    }
  };

  const handleOp = (op: string) => {
    const current = parseFloat(display);
    if (prevValue === null) {
      setPrevValue(current);
    } else if (operation) {
      const result = calculate(prevValue, current, operation);
      setPrevValue(result);
      setDisplay(result.toString());
    }
    setOperation(op);
    setClearOnNextDigit(true);
  };

  const calculate = (a: number, b: number, op: string): number => {
    switch (op) {
      case '+': return a + b;
      case '-': return a - b;
      case '×': return a * b;
      case '÷': return b !== 0 ? a / b : 0;
      default: return b;
    }
  };

  const handleEquals = () => {
    if (prevValue !== null && operation) {
      const current = parseFloat(display);
      const result = calculate(prevValue, current, operation);
      setDisplay(result.toString());
      setPrevValue(null);
      setOperation(null);
      setClearOnNextDigit(true);
    }
  };

  const handleClear = () => {
    setDisplay('0');
    setPrevValue(null);
    setOperation(null);
    setClearOnNextDigit(false);
  };

  return (
    <div className="fixed inset-0 bg-slate-950/40 backdrop-blur-xs flex items-center justify-center z-50 p-4 select-none">
      <div className="bg-white text-slate-800 rounded-xl shadow-2xl border border-slate-200 w-72 overflow-hidden flex flex-col animate-in fade-in zoom-in-95 duration-150">
        {/* Title */}
        <div className="bg-white px-3.5 py-2.5 flex items-center justify-between text-xs font-bold border-b border-slate-200 text-slate-800">
          <span className="text-slate-900">الآلة الحاسبة السريعة [F9]</span>
          <button onClick={onClose} className="text-slate-400 hover:text-slate-700 font-bold cursor-pointer">
            ✕
          </button>
        </div>

        {/* Display */}
        <div className="p-3 bg-slate-50 border-b border-slate-200 text-left" dir="ltr">
          <div className="text-[11px] text-slate-500 h-4 font-mono">
            {prevValue !== null ? `${prevValue} ${operation || ''}` : ''}
          </div>
          <div className="text-2xl font-mono font-bold text-slate-900 truncate">
            {display}
          </div>
        </div>

        {/* Buttons Grid */}
        <div className="p-2.5 grid grid-cols-4 gap-1.5 text-sm font-bold bg-slate-100/70">
          <button onClick={handleClear} className="bg-rose-50 hover:bg-rose-100 border border-rose-200 p-2.5 rounded-lg text-rose-700 cursor-pointer">
            C
          </button>
          <button onClick={() => setDisplay(display.slice(0, -1) || '0')} className="bg-slate-200 hover:bg-slate-300 border border-slate-300 p-2.5 rounded-lg text-slate-700 flex items-center justify-center cursor-pointer">
            <Delete className="w-4 h-4" />
          </button>
          <button onClick={() => handleOp('÷')} className="bg-sky-50 hover:bg-sky-100 border border-sky-200 p-2.5 rounded-lg text-sky-700 cursor-pointer">
            ÷
          </button>
          <button onClick={() => handleOp('×')} className="bg-sky-50 hover:bg-sky-100 border border-sky-200 p-2.5 rounded-lg text-sky-700 cursor-pointer">
            ×
          </button>

          {['7', '8', '9'].map((d) => (
            <button key={d} onClick={() => handleDigit(d)} className="bg-white hover:bg-slate-50 border border-slate-200 shadow-2xs p-2.5 rounded-lg text-slate-800 cursor-pointer">
              {d}
            </button>
          ))}
          <button onClick={() => handleOp('-')} className="bg-sky-50 hover:bg-sky-100 border border-sky-200 p-2.5 rounded-lg text-sky-700 cursor-pointer">
            -
          </button>

          {['4', '5', '6'].map((d) => (
            <button key={d} onClick={() => handleDigit(d)} className="bg-white hover:bg-slate-50 border border-slate-200 shadow-2xs p-2.5 rounded-lg text-slate-800 cursor-pointer">
              {d}
            </button>
          ))}
          <button onClick={() => handleOp('+')} className="bg-sky-50 hover:bg-sky-100 border border-sky-200 p-2.5 rounded-lg text-sky-700 cursor-pointer">
            +
          </button>

          {['1', '2', '3'].map((d) => (
            <button key={d} onClick={() => handleDigit(d)} className="bg-white hover:bg-slate-50 border border-slate-200 shadow-2xs p-2.5 rounded-lg text-slate-800 cursor-pointer">
              {d}
            </button>
          ))}
          <button onClick={handleEquals} className="bg-emerald-600 hover:bg-emerald-500 row-span-2 p-2.5 rounded-lg text-white flex items-center justify-center text-lg cursor-pointer shadow-xs">
            =
          </button>

          <button onClick={() => handleDigit('0')} className="col-span-2 bg-white hover:bg-slate-50 border border-slate-200 shadow-2xs p-2.5 rounded-lg text-slate-800 cursor-pointer">
            0
          </button>
          <button onClick={() => !display.includes('.') && setDisplay(display + '.')} className="bg-white hover:bg-slate-50 border border-slate-200 shadow-2xs p-2.5 rounded-lg text-slate-800 cursor-pointer">
            .
          </button>
        </div>
      </div>
    </div>
  );
};
