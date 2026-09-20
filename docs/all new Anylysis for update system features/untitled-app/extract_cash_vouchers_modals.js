const fs = require('fs');
const content = fs.readFileSync('components/CashVouchersView.tsx', 'utf8');

const slices = [
  { name: 'showNewModal', marker: '{showNewModal && (' },
  { name: 'printingVoucher', marker: '{printingVoucher && (' },
  { name: 'viewingVoucher', marker: '{viewingVoucher && (' },
  { name: 'voidingVoucher', marker: '{voidingVoucher && (' },
  { name: 'showTransferModal', marker: '{showTransferModal && (' },
  { name: 'showAdvancesInfoModal', marker: '{showAdvancesInfoModal && (' }
];

let lastIdx = content.indexOf(slices[0].marker);
if (lastIdx === -1) {
  console.log("Could not find start");
  process.exit(1);
}

// Find the start of the first modal
console.log('Modals start at:', lastIdx);
