'use client';

import React, { createContext, useContext, useState, useCallback, ReactNode } from 'react';
import { 
  WinFormsMessageBoxModal, 
  WinFormsDialogOptions, 
  DialogResult,
  highlightAndFocusControl 
} from '@/components/WinFormsMessageBoxModal';

interface WinFormsDialogContextType {
  showMessageBox: (options: WinFormsDialogOptions) => Promise<DialogResult>;
  showError: (message: string, title?: string, targetControlId?: string, technicalDetails?: string) => Promise<DialogResult>;
  showSuccess: (message: string, title?: string, targetControlId?: string) => Promise<DialogResult>;
  showWarning: (message: string, title?: string, targetControlId?: string) => Promise<DialogResult>;
  showConfirm: (message: string, title?: string) => Promise<boolean>;
  highlightControl: (elementId: string, isError?: boolean) => void;
}

const WinFormsDialogContext = createContext<WinFormsDialogContextType | undefined>(undefined);

export const WinFormsDialogProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [currentDialog, setCurrentDialog] = useState<WinFormsDialogOptions | null>(null);
  const [resolver, setResolver] = useState<((result: DialogResult) => void) | null>(null);

  const showMessageBox = useCallback((options: WinFormsDialogOptions): Promise<DialogResult> => {
    return new Promise((resolve) => {
      setResolver(() => resolve);
      setCurrentDialog(options);
    });
  }, []);

  const showError = useCallback((
    message: string, 
    title = 'خطأ في العملية (Error)', 
    targetControlId?: string, 
    technicalDetails?: string
  ): Promise<DialogResult> => {
    return showMessageBox({
      type: 'ERROR',
      buttons: 'OK',
      title,
      message,
      targetControlId,
      technicalDetails,
    });
  }, [showMessageBox]);

  const showSuccess = useCallback((
    message: string, 
    title = 'تمت العملية بنجاح (Success)', 
    targetControlId?: string
  ): Promise<DialogResult> => {
    return showMessageBox({
      type: 'SUCCESS',
      buttons: 'OK',
      title,
      message,
      targetControlId,
    });
  }, [showMessageBox]);

  const showWarning = useCallback((
    message: string, 
    title = 'تحذير تشغيلي (Warning)', 
    targetControlId?: string
  ): Promise<DialogResult> => {
    return showMessageBox({
      type: 'WARNING',
      buttons: 'OK',
      title,
      message,
      targetControlId,
    });
  }, [showMessageBox]);

  const showConfirm = useCallback(async (
    message: string, 
    title = 'تأكيد العملية (Confirm)'
  ): Promise<boolean> => {
    const result = await showMessageBox({
      type: 'QUESTION',
      buttons: 'YES_NO',
      title,
      message,
    });
    return result === 'YES';
  }, [showMessageBox]);

  const handleClose = (result: DialogResult) => {
    if (resolver) {
      resolver(result);
    }
    setCurrentDialog(null);
    setResolver(null);
  };

  return (
    <WinFormsDialogContext.Provider
      value={{
        showMessageBox,
        showError,
        showSuccess,
        showWarning,
        showConfirm,
        highlightControl: highlightAndFocusControl,
      }}
    >
      {children}
      <WinFormsMessageBoxModal dialog={currentDialog} onClose={handleClose} />
    </WinFormsDialogContext.Provider>
  );
};

export const useWinFormsDialog = (): WinFormsDialogContextType => {
  const context = useContext(WinFormsDialogContext);
  if (!context) {
    throw new Error('useWinFormsDialog must be used within a WinFormsDialogProvider');
  }
  return context;
};
