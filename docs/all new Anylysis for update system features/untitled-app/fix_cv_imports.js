const fs = require('fs');

function addImports(file, icons) {
  const path = 'components/cash-vouchers/' + file;
  let content = fs.readFileSync(path, 'utf8');
  
  const lucideRegex = /import\s+\{([^}]+)\}\s+from\s+'lucide-react';/;
  const match = content.match(lucideRegex);
  
  if (match) {
    const existingIcons = match[1].split(',').map(s => s.trim()).filter(Boolean);
    const newIcons = new Set([...existingIcons, ...icons]);
    const newImportStr = 'import { ' + Array.from(newIcons).join(', ') + " } from 'lucide-react';";
    content = content.replace(lucideRegex, newImportStr);
    fs.writeFileSync(path, content);
  }
}

addImports('CashVoucherNewModal.tsx', ['ArrowDownLeft', 'ArrowUpRight', 'AlertCircle', 'Landmark', 'FileText', 'CheckCircle2']);
addImports('CashVoucherTransferModal.tsx', ['AlertCircle', 'ShieldCheck', 'ArrowUpRight', 'ArrowDownLeft', 'DollarSign']);
addImports('CashVoucherViewModal.tsx', ['Ban', 'Printer']);
addImports('CashVoucherVoidModal.tsx', ['Ban']);

console.log('Fixed imports');
