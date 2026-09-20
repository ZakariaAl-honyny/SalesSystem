const fs = require('fs');

const whFiles = [
  'WarehouseNewTransferModal.tsx',
  'WarehousePrintTransferModal.tsx',
  'WarehouseManageModal.tsx'
];

whFiles.forEach(file => {
  const path = 'components/warehouse-transfers/' + file;
  let content = fs.readFileSync(path, 'utf8');
  
  content = content.replace(/return \{[a-zA-Z]+( && selectedTransferForPrint)? && \(/, 'return (');
  
  const lastIndex = content.lastIndexOf(')}');
  if (lastIndex !== -1) {
    content = content.substring(0, lastIndex) + ')' + content.substring(lastIndex + 2);
  }
  
  fs.writeFileSync(path, content);
});

const stockPath = 'components/stocktaking/StocktakingNewModal.tsx';
let stockContent = fs.readFileSync(stockPath, 'utf8');
stockContent = stockContent.replace(/import\s+\{([^}]+)\}\s+from\s+'lucide-react';/, "import { $1, ClipboardCheck, Check } from 'lucide-react';");
fs.writeFileSync(stockPath, stockContent);

console.log('Fixed Warehouse Modals and Stocktaking Imports');
