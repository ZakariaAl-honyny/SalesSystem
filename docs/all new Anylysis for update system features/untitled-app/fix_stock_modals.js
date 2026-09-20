const fs = require('fs');

const files = [
  'StocktakingNewModal.tsx',
  'StocktakingPrintModal.tsx',
  'StocktakingPrintAdjustmentModal.tsx',
  'StocktakingPrintCardModal.tsx'
];

files.forEach(file => {
  const path = 'components/stocktaking/' + file;
  let content = fs.readFileSync(path, 'utf8');
  
  content = content.replace(/return \{[a-zA-Z]+( && selectedCardProduct)? && \(/, 'return (');
  
  const lastIndex = content.lastIndexOf(')}');
  if (lastIndex !== -1) {
    content = content.substring(0, lastIndex) + ')' + content.substring(lastIndex + 2);
  }
  
  fs.writeFileSync(path, content);
});
console.log('Fixed Modals syntax');
