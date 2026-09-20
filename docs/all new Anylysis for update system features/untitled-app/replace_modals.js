const fs = require('fs');

let content = fs.readFileSync('components/CommercialPurchaseInvoiceView.tsx', 'utf8');

const startIndex = content.indexOf('{/* ============================================================== */}\n      {/* POPUP MODAL 1:');
const endIndex = content.lastIndexOf('</div>\n  );\n};');

if (startIndex === -1 || endIndex === -1) {
  console.log('Could not find start or end index');
  process.exit(1);
}

const newModals = `      {/* ============================================================== */}
      {/* POPUP MODAL 1: ITEM ADD / EDIT MODAL (Screenshots 1, 5, 8)      */}
      {/* ============================================================== */}
      <PurchaseItemModal
        isOpen={showItemModal}
        modalProduct={modalProduct}
        activeModalField={activeModalField}
        setActiveModalField={setActiveModalField}
        modalQty={modalQty}
        modalPrice={modalPrice}
        modalTotal={modalTotal}
        modalUnit={modalUnit}
        itemModalMode={itemModalMode}
        handleAppendZeros={handleAppendZeros}
        handleItemKeypadPress={handleItemKeypadPress}
        handleSaveItemModal={handleSaveItemModal}
        setShowItemInfoModal={setShowItemInfoModal}
        setShowOtherDetailsModal={setShowOtherDetailsModal}
        onClose={() => setShowItemModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 2: DISCOUNT, TAXES & EXPENSES (Screenshot 10)       */}
      {/* ============================================================== */}
      <PurchaseDiscountModal
        isOpen={showDiscountModal}
        discountMode={discountMode}
        setDiscountMode={setDiscountMode}
        activeDiscountField={activeDiscountField}
        setActiveDiscountField={setActiveDiscountField}
        discountValue={discountValue}
        taxPercent={taxPercent}
        setTaxPercent={setTaxPercent}
        handleDiscountKeypadPress={handleDiscountKeypadPress}
        onClose={() => setShowDiscountModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 3: CURRENCY SELECTION (Screenshot 2)                */}
      {/* ============================================================== */}
      <PurchaseCurrencyModal
        isOpen={showCurrencyModal}
        selectedCurrency={selectedCurrency}
        onSelect={(curr) => {
          setSelectedCurrency(curr);
          setShowCurrencyModal(false);
        }}
        onClose={() => setShowCurrencyModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 4: CAMERA / PHONE ATTACHMENT (Screenshot 10)       */}
      {/* ============================================================== */}
      <PurchaseCameraModal
        isOpen={showCameraModal}
        attachedImageName={attachedImageName}
        onAttach={(name) => setAttachedImageName(name)}
        onClose={() => setShowCameraModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 5: PRODUCT INFO MODAL (Info icon in Item Dialog)    */}
      {/* ============================================================== */}
      <PurchaseItemInfoModal
        isOpen={showItemInfoModal}
        product={modalProduct}
        currency={selectedCurrency}
        onClose={() => setShowItemInfoModal(false)}
      />

      {/* ============================================================== */}
      {/* POPUP MODAL 6: OTHER DETAILS (تفاصيل أخرى & تاريخ الإنتهاء)      */}
      {/* ============================================================== */}
      <PurchaseOtherDetailsModal
        isOpen={showOtherDetailsModal}
        expiryDate={modalExpiryDate}
        onExpiryDateChange={setModalExpiryDate}
        unit={modalUnit}
        onUnitChange={setModalUnit}
        onClose={() => setShowOtherDetailsModal(false)}
      />
    `;

const newContent = content.substring(0, startIndex) + newModals + content.substring(endIndex);
fs.writeFileSync('components/CommercialPurchaseInvoiceView.tsx', newContent);
console.log('Success');
