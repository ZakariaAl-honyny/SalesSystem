const fs = require('fs');

let moduleContent = fs.readFileSync('components/CashVouchersView.tsx', 'utf8');

// Insert imports
const importRegex = /import React, \{ useState, useMemo, useEffect \} from 'react';/;
const newImports = `import React, { useState, useMemo, useEffect } from 'react';
import {
  CashVoucherNewModal,
  CashVoucherTransferModal,
  CashVoucherViewModal,
  CashVoucherPrintModal,
  CashVoucherVoidModal,
  CashVoucherAdvancesModal
} from './cash-vouchers';`;
moduleContent = moduleContent.replace(importRegex, newImports);

const startIndex = moduleContent.indexOf('{/* 4. Modal: Issue New Voucher (WinForms Dialog) */}');
const endIndex = moduleContent.lastIndexOf('    </div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find cash vouchers modal slice');
  process.exit(1);
}

const replacementJSX = `{/* 4. Modal: Issue New Voucher (WinForms Dialog) */}
      <CashVoucherNewModal
        showNewModal={showNewModal}
        setShowNewModal={setShowNewModal}
        newVoucherType={newVoucherType}
        setNewVoucherType={setNewVoucherType}
        handleSaveNewVoucher={handleSaveNewVoucher}
        formDate={formDate}
        setFormDate={setFormDate}
        formPaymentMethod={formPaymentMethod}
        setFormPaymentMethod={setFormPaymentMethod}
        formCashAccountCode={formCashAccountCode}
        setFormCashAccountCode={setFormCashAccountCode}
        formPartyType={formPartyType}
        setFormPartyType={setFormPartyType}
        formSelectedPartyId={formSelectedPartyId}
        setFormSelectedPartyId={setFormSelectedPartyId}
        formPartyName={formPartyName}
        setFormPartyName={setFormPartyName}
        formPartyPhone={formPartyPhone}
        setFormPartyPhone={setFormPartyPhone}
        formAmount={formAmount}
        setFormAmount={setFormAmount}
        formReference={formReference}
        setFormReference={setFormReference}
        formNote={formNote}
        setFormNote={setFormNote}
        formCostCenterCode={formCostCenterCode}
        setFormCostCenterCode={setFormCostCenterCode}
        chartOfAccounts={chartOfAccounts}
        treasuryAndBankAccounts={treasuryAndBankAccounts}
        costCentersList={costCentersList}
        customers={customers}
        suppliers={suppliers}
        currency={currency}
      />

      {/* 5. Printable Official Voucher Modal (Print / Preview Voucher) */}
      <CashVoucherPrintModal
        printingVoucher={printingVoucher}
        setPrintingVoucher={setPrintingVoucher}
        currency={currency}
      />

      {/* 6. Modal: View Voucher Details & Linked Journal Entry */}
      <CashVoucherViewModal
        viewingVoucher={viewingVoucher}
        setViewingVoucher={setViewingVoucher}
        currency={currency}
        chartOfAccounts={chartOfAccounts}
      />

      {/* 7. Modal: Void Voucher Confirmation */}
      <CashVoucherVoidModal
        voidingVoucher={voidingVoucher}
        setVoidingVoucher={setVoidingVoucher}
        voidReasonInput={voidReasonInput}
        setVoidReasonInput={setVoidReasonInput}
        handleVoidVoucher={handleVoidVoucher}
      />

      {/* 6. Modal: Treasury Transfer (تحويل وتغذية سيولة بين الصناديق والبنوك 111 ⇄ 112) */}
      <CashVoucherTransferModal
        showTransferModal={showTransferModal}
        setShowTransferModal={setShowTransferModal}
        handleTreasuryTransfer={handleTreasuryTransfer}
        transferFromCode={transferFromCode}
        setTransferFromCode={setTransferFromCode}
        transferToCode={transferToCode}
        setTransferToCode={setTransferToCode}
        transferAmount={transferAmount}
        setTransferAmount={setTransferAmount}
        transferDate={transferDate}
        setTransferDate={setTransferDate}
        transferReference={transferReference}
        setTransferReference={setTransferReference}
        transferNote={transferNote}
        setTransferNote={setTransferNote}
        transferError={transferError}
        treasuryAndBankAccounts={treasuryAndBankAccounts}
        currency={currency}
      />

      {/* 7. Modal: Employee Advances & Custody Roadmap (توثيق تحليل العهد المالية للموظفين) */}
      <CashVoucherAdvancesModal
        showAdvancesInfoModal={showAdvancesInfoModal}
        setShowAdvancesInfoModal={setShowAdvancesInfoModal}
      />\n\n`;

const newModuleContent = moduleContent.substring(0, startIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/CashVouchersView.tsx', newModuleContent);
console.log('CashVouchersView.tsx refactored.');
