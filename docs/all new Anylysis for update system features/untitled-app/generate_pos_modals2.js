const fs = require('fs');

let moduleContent = fs.readFileSync('components/POSModule.tsx', 'utf8');

const startIndex = moduleContent.indexOf('{/* ============================================================== */}');
// Wait, the first one is the held modal
const firstIndex = moduleContent.indexOf('{showHeldModal && (');
const endIndex = moduleContent.lastIndexOf('    </div>\n  );\n};');

if (firstIndex === -1 || endIndex === -1) {
  console.log('Error: Could not find pos modal slice');
  process.exit(1);
}

const replacementJSX = `<POSHeldOrdersModal
        showHeldModal={showHeldModal}
        setShowHeldModal={setShowHeldModal}
        filteredHeldOrders={filteredHeldOrders}
        heldOrders={heldOrders}
        handleRecallHeldOrder={handleRecallHeldOrder}
        currency={currency}
      />

      {/* ============================================================== */}
      {/* MODAL: Touch Discount, Tax % & Other Charges (Screenshot 10)  */}
      {/* ============================================================== */}
      <POSDiscountModal
        showDiscountModal={showDiscountModal}
        setShowDiscountModal={setShowDiscountModal}
        discountType={discountType}
        setDiscountType={setDiscountType}
        discountOverall={discountOverall}
        setDiscountOverall={setDiscountOverall}
        taxPercentVal={taxPercentVal}
        setTaxPercentVal={setTaxPercentVal}
        setIsTaxEnabled={setIsTaxEnabled}
        otherChargesAccountCode={otherChargesAccountCode}
        setOtherChargesAccountCode={setOtherChargesAccountCode}
        setOtherChargesName={setOtherChargesName}
        EXPENSE_ACCOUNTS_OPTIONS={EXPENSE_ACCOUNTS_OPTIONS}
        otherChargesVal={otherChargesVal}
        setOtherChargesVal={setOtherChargesVal}
        subtotal={subtotal}
        totalDiscount={totalDiscount}
        calculatedTax={calculatedTax}
        otherCharges={otherCharges}
        otherChargesName={otherChargesName}
        grandTotal={grandTotal}
        currency={currency}
      />
`;

const newModuleContent = moduleContent.substring(0, firstIndex) + replacementJSX + moduleContent.substring(endIndex);
fs.writeFileSync('components/POSModule.tsx', newModuleContent);
console.log('POSModule.tsx refactored.');
