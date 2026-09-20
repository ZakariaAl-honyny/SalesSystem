const fs = require('fs');

const createModalFile = (name, content) => {
  fs.writeFileSync('components/stocktaking/' + name + '.tsx', content);
};

const newStocktakeStr = fs.readFileSync('modal_stock_showNewStocktakeModal.txt', 'utf8');
createModalFile('StocktakingNewModal', `import React from 'react';
import { Plus, X, Search, FileText, Calendar, Users, ClipboardList } from 'lucide-react';
import { Warehouse, ProductItem } from '@/types/pos';

interface StocktakingNewModalProps {
  showNewStocktakeModal: boolean;
  setShowNewStocktakeModal: (v: boolean) => void;
  handleStartNewStocktake: (e: React.FormEvent) => void;
  newStWarehouseId: string;
  setNewStWarehouseId: (v: string) => void;
  newStNotes: string;
  setNewStNotes: (v: string) => void;
  newStHeadMember: string;
  setNewStHeadMember: (v: string) => void;
  newStMember2: string;
  setNewStMember2: (v: string) => void;
  newStMember3: string;
  setNewStMember3: (v: string) => void;
  warehouses: Warehouse[];
  products: ProductItem[];
}

export const StocktakingNewModal: React.FC<StocktakingNewModalProps> = ({
  showNewStocktakeModal, setShowNewStocktakeModal, handleStartNewStocktake, newStWarehouseId, setNewStWarehouseId,
  newStNotes, setNewStNotes, newStHeadMember, setNewStHeadMember, newStMember2, setNewStMember2, newStMember3, setNewStMember3,
  warehouses, products
}) => {
  if (!showNewStocktakeModal) return null;
  return ` + newStocktakeStr + `;
};
`);

const printStocktakeStr = fs.readFileSync('modal_stock_printStocktake.txt', 'utf8');
createModalFile('StocktakingPrintModal', `import React from 'react';
import { Printer, Calendar, FileText, Users, Hash, CheckCircle2, AlertTriangle, AlertCircle, Sparkles } from 'lucide-react';
import { StocktakingSession } from '@/types/pos';

interface StocktakingPrintModalProps {
  printStocktake: StocktakingSession | null;
  setPrintStocktake: (v: StocktakingSession | null) => void;
  getWarehouseName: (id: string) => string;
  currency: string;
}

export const StocktakingPrintModal: React.FC<StocktakingPrintModalProps> = ({
  printStocktake, setPrintStocktake, getWarehouseName, currency
}) => {
  if (!printStocktake) return null;
  return ` + printStocktakeStr + `;
};
`);

const printAdjStr = fs.readFileSync('modal_stock_printAdjustment.txt', 'utf8');
createModalFile('StocktakingPrintAdjustmentModal', `import React from 'react';
import { Printer, Calendar, FileText, Scale, TrendingUp, TrendingDown } from 'lucide-react';
import { StockAdjustment } from '@/types/pos';

interface StocktakingPrintAdjustmentModalProps {
  printAdjustment: StockAdjustment | null;
  setPrintAdjustment: (v: StockAdjustment | null) => void;
  getWarehouseName: (id: string) => string;
  currency: string;
}

export const StocktakingPrintAdjustmentModal: React.FC<StocktakingPrintAdjustmentModalProps> = ({
  printAdjustment, setPrintAdjustment, getWarehouseName, currency
}) => {
  if (!printAdjustment) return null;
  return ` + printAdjStr + `;
};
`);

const printCardStr = fs.readFileSync('modal_stock_printStockCard.txt', 'utf8');
createModalFile('StocktakingPrintCardModal', `import React from 'react';
import { Printer, Calendar, FileText, Database, Package } from 'lucide-react';
import { ProductItem } from '@/types/pos';

interface StocktakingPrintCardModalProps {
  printStockCard: boolean;
  setPrintStockCard: (v: boolean) => void;
  selectedCardProduct: ProductItem | null;
  ledgerEntries: any[];
  currency: string;
}

export const StocktakingPrintCardModal: React.FC<StocktakingPrintCardModalProps> = ({
  printStockCard, setPrintStockCard, selectedCardProduct, ledgerEntries, currency
}) => {
  if (!printStockCard || !selectedCardProduct) return null;
  return ` + printCardStr + `;
};
`);

fs.writeFileSync('components/stocktaking/index.ts', `export * from './StocktakingNewModal';
export * from './StocktakingPrintModal';
export * from './StocktakingPrintAdjustmentModal';
export * from './StocktakingPrintCardModal';
`);

let moduleContent = fs.readFileSync('components/StocktakingAndCardsView.tsx', 'utf8');
const importRegex = /import React, \{ useState, useMemo \} from 'react';/;
const newImports = `import React, { useState, useMemo } from 'react';
import {
  StocktakingNewModal,
  StocktakingPrintModal,
  StocktakingPrintAdjustmentModal,
  StocktakingPrintCardModal
} from './stocktaking';`;
moduleContent = moduleContent.replace(importRegex, newImports);

const startIndex = moduleContent.indexOf('{/* ====================================================');
const endIndex = moduleContent.lastIndexOf('    </div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find stocktaking modal slice');
  process.exit(1);
}

const replacementJSX = `{/* ====================================================
          MODAL 1: NEW PHYSICAL STOCKTAKE SESSION
          ==================================================== */}
      <StocktakingNewModal
        showNewStocktakeModal={showNewStocktakeModal}
        setShowNewStocktakeModal={setShowNewStocktakeModal}
        handleStartNewStocktake={handleStartNewStocktake}
        newStWarehouseId={newStWarehouseId}
        setNewStWarehouseId={setNewStWarehouseId}
        newStNotes={newStNotes}
        setNewStNotes={setNewStNotes}
        newStHeadMember={newStHeadMember}
        setNewStHeadMember={setNewStHeadMember}
        newStMember2={newStMember2}
        setNewStMember2={setNewStMember2}
        newStMember3={newStMember3}
        setNewStMember3={setNewStMember3}
        warehouses={warehouses}
        products={products}
      />

      {/* ====================================================
          MODAL 2: PRINT OFFICIAL STOCKTAKE REPORT (A4)
          ==================================================== */}
      <StocktakingPrintModal
        printStocktake={printStocktake}
        setPrintStocktake={setPrintStocktake}
        getWarehouseName={getWarehouseName}
        currency={currency}
      />

      {/* ====================================================
          MODAL 3: PRINT STOCK ADJUSTMENT VOUCHER (A4)
          ==================================================== */}
      <StocktakingPrintAdjustmentModal
        printAdjustment={printAdjustment}
        setPrintAdjustment={setPrintAdjustment}
        getWarehouseName={getWarehouseName}
        currency={currency}
      />

      {/* ====================================================
          MODAL 4: PRINT STOCK LEDGER CARD (A4)
          ==================================================== */}
      <StocktakingPrintCardModal
        printStockCard={printStockCard}
        setPrintStockCard={setPrintStockCard}
        selectedCardProduct={selectedCardProduct}
        ledgerEntries={ledgerEntries}
        currency={currency}
      />
\n\n`;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/StocktakingAndCardsView.tsx', newModuleContent);
console.log('StocktakingAndCardsView.tsx refactored.');
