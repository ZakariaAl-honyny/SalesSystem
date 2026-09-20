const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync('components/warehouse-transfers/' + name + '.tsx', content);
};

const newTransferStr = fs.readFileSync('modal_warehouse_showNewTransferModal.txt', 'utf8');
createModalFile('WarehouseNewTransferModal', `import React from 'react';
import { ArrowRightLeft, X, Package, Search, Plus, Trash2, Calendar, FileText, CheckCircle2 } from 'lucide-react';
import { Warehouse, ProductItem } from '@/types/pos';

interface WarehouseNewTransferModalProps {
  showNewTransferModal: boolean;
  setShowNewTransferModal: (v: boolean) => void;
  handleSaveTransfer: (e: React.FormEvent) => void;
  formDate: string;
  setFormDate: (v: string) => void;
  formFromWarehouseId: string;
  setFormFromWarehouseId: (v: string) => void;
  formToWarehouseId: string;
  setFormToWarehouseId: (v: string) => void;
  formNotes: string;
  setFormNotes: (v: string) => void;
  formTransferItems: any[];
  setFormTransferItems: (v: any[]) => void;
  warehouses: Warehouse[];
  products: ProductItem[];
  barcodeScanInput: string;
  setBarcodeScanInput: (v: string) => void;
  handleAddTransferItemFromScan: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  handleAddTransferItemClick: (p: ProductItem) => void;
}

export const WarehouseNewTransferModal: React.FC<WarehouseNewTransferModalProps> = ({
  showNewTransferModal, setShowNewTransferModal, handleSaveTransfer, formDate, setFormDate,
  formFromWarehouseId, setFormFromWarehouseId, formToWarehouseId, setFormToWarehouseId, formNotes, setFormNotes,
  formTransferItems, setFormTransferItems, warehouses, products, barcodeScanInput, setBarcodeScanInput,
  handleAddTransferItemFromScan, handleAddTransferItemClick
}) => {
  if (!showNewTransferModal) return null;
  return ` + newTransferStr + `;
};
`);

const printModalStr = fs.readFileSync('modal_warehouse_showPrintModal.txt', 'utf8');
createModalFile('WarehousePrintTransferModal', `import React from 'react';
import { Printer, Calendar, ArrowRightLeft, FileText, CheckCircle2, ShieldAlert, X } from 'lucide-react';

interface WarehousePrintTransferModalProps {
  showPrintModal: boolean;
  setShowPrintModal: (v: boolean) => void;
  selectedTransferForPrint: any;
  getWarehouseName: (id: string) => string;
}

export const WarehousePrintTransferModal: React.FC<WarehousePrintTransferModalProps> = ({
  showPrintModal, setShowPrintModal, selectedTransferForPrint, getWarehouseName
}) => {
  if (!showPrintModal || !selectedTransferForPrint) return null;
  return ` + printModalStr + `;
};
`);

const newWarehouseStr = fs.readFileSync('modal_warehouse_showNewWarehouseModal.txt', 'utf8');
createModalFile('WarehouseManageModal', `import React from 'react';
import { Building2, X, MapPin, Phone, User, CheckCircle2 } from 'lucide-react';
import { Warehouse } from '@/types/pos';

interface WarehouseManageModalProps {
  showNewWarehouseModal: boolean;
  setShowNewWarehouseModal: (v: boolean) => void;
  handleSaveWarehouse: (e: React.FormEvent) => void;
  editingWarehouse: Warehouse | null;
  whFormName: string;
  setWhFormName: (v: string) => void;
  whFormLocation: string;
  setWhFormLocation: (v: string) => void;
  whFormManagerName: string;
  setWhFormManagerName: (v: string) => void;
  whFormContactPhone: string;
  setWhFormContactPhone: (v: string) => void;
  whFormIsActive: boolean;
  setWhFormIsActive: (v: boolean) => void;
}

export const WarehouseManageModal: React.FC<WarehouseManageModalProps> = ({
  showNewWarehouseModal, setShowNewWarehouseModal, handleSaveWarehouse, editingWarehouse,
  whFormName, setWhFormName, whFormLocation, setWhFormLocation, whFormManagerName,
  setWhFormManagerName, whFormContactPhone, setWhFormContactPhone, whFormIsActive, setWhFormIsActive
}) => {
  if (!showNewWarehouseModal) return null;
  return ` + newWarehouseStr + `;
};
`);

fs.writeFileSync('components/warehouse-transfers/index.ts', `export * from './WarehouseNewTransferModal';
export * from './WarehousePrintTransferModal';
export * from './WarehouseManageModal';
`);

let moduleContent = fs.readFileSync('components/WarehouseTransfersView.tsx', 'utf8');
const importRegex = /import React, \{ useState, useMemo \} from 'react';/;
const newImports = `import React, { useState, useMemo } from 'react';
import {
  WarehouseNewTransferModal,
  WarehousePrintTransferModal,
  WarehouseManageModal
} from './warehouse-transfers';`;
moduleContent = moduleContent.replace(importRegex, newImports);

const startIndex = moduleContent.indexOf('{showNewTransferModal && (');
const endIndex = moduleContent.lastIndexOf('    </div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find warehouse modal slice');
  process.exit(1);
}

const replacementJSX = `<WarehouseNewTransferModal
        showNewTransferModal={showNewTransferModal}
        setShowNewTransferModal={setShowNewTransferModal}
        handleSaveTransfer={handleSaveTransfer}
        formDate={formDate}
        setFormDate={setFormDate}
        formFromWarehouseId={formFromWarehouseId}
        setFormFromWarehouseId={setFormFromWarehouseId}
        formToWarehouseId={formToWarehouseId}
        setFormToWarehouseId={setFormToWarehouseId}
        formNotes={formNotes}
        setFormNotes={setFormNotes}
        formTransferItems={formTransferItems}
        setFormTransferItems={setFormTransferItems}
        warehouses={warehouses}
        products={products}
        barcodeScanInput={barcodeScanInput}
        setBarcodeScanInput={setBarcodeScanInput}
        handleAddTransferItemFromScan={handleAddTransferItemFromScan}
        handleAddTransferItemClick={handleAddTransferItemClick}
      />

      {/* --- A4 Print Modal --- */}
      <WarehousePrintTransferModal
        showPrintModal={showPrintModal}
        setShowPrintModal={setShowPrintModal}
        selectedTransferForPrint={selectedTransferForPrint}
        getWarehouseName={getWarehouseName}
      />

      {/* --- Warehouse Create/Edit Modal --- */}
      <WarehouseManageModal
        showNewWarehouseModal={showNewWarehouseModal}
        setShowNewWarehouseModal={setShowNewWarehouseModal}
        handleSaveWarehouse={handleSaveWarehouse}
        editingWarehouse={editingWarehouse}
        whFormName={whFormName}
        setWhFormName={setWhFormName}
        whFormLocation={whFormLocation}
        setWhFormLocation={setWhFormLocation}
        whFormManagerName={whFormManagerName}
        setWhFormManagerName={setWhFormManagerName}
        whFormContactPhone={whFormContactPhone}
        setWhFormContactPhone={setWhFormContactPhone}
        whFormIsActive={whFormIsActive}
        setWhFormIsActive={setWhFormIsActive}
      />\n\n`;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/WarehouseTransfersView.tsx', newModuleContent);
console.log('WarehouseTransfersView.tsx refactored.');
