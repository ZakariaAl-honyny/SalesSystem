const fs = require('fs');
const content = fs.readFileSync('components/WarehouseTransfersView.tsx', 'utf8');

function extractJSXBlock(startIndex) {
    let braceCount = 0;
    let inString = false;
    let stringChar = '';
    let foundFirst = false;

    for (let i = startIndex; i < content.length; i++) {
        const char = content[i];
        
        if (char === '"' || char === "'" || char === '\`') {
            if (!inString) {
                inString = true;
                stringChar = char;
            } else if (stringChar === char && content[i-1] !== '\\\\') {
                inString = false;
            }
        }
        
        if (!inString) {
            if (char === '{') {
                braceCount++;
                foundFirst = true;
            } else if (char === '}') {
                braceCount--;
            }
        }
        
        if (foundFirst && braceCount === 0) {
            return content.substring(startIndex, i + 1);
        }
    }
    return null;
}

const tags = [
  'showNewTransferModal', 'showPrintModal', 'showNewWarehouseModal'
];

tags.forEach(tag => {
  const index = content.indexOf(`{${tag} &&`);
  if (index !== -1) {
     const block = extractJSXBlock(index);
     if (block) {
       fs.writeFileSync(`modal_warehouse_${tag}.txt`, block);
       console.log(`Extracted ${tag}`);
     } else {
       console.log(`Failed to extract ${tag}`);
     }
  } else {
       console.log(`Not found: ${tag}`);
  }
});
