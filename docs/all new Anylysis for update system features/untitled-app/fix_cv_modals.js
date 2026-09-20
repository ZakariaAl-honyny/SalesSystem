const fs = require('fs');

const files = [
  'CashVoucherNewModal.tsx',
  'CashVoucherPrintModal.tsx',
  'CashVoucherViewModal.tsx',
  'CashVoucherVoidModal.tsx',
  'CashVoucherTransferModal.tsx'
];

files.forEach(file => {
  const path = 'components/cash-vouchers/' + file;
  let content = fs.readFileSync(path, 'utf8');
  
  content = content.replace(/return \{[a-zA-Z]+ && \(/, 'return (');
  
  const lastIndex = content.lastIndexOf(')}');
  if (lastIndex !== -1) {
    content = content.substring(0, lastIndex) + ')' + content.substring(lastIndex + 2);
  }
  
  fs.writeFileSync(path, content);
});
console.log('Fixed Modals syntax');
