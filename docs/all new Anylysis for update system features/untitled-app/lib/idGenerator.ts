let salesInvoiceCounter = 1005;
let purchaseInvoiceCounter = 5003;
let salesReturnCounter = 6000;
let purchaseReturnCounter = 7000;
let receiptVoucherCounter = 1003;
let paymentVoucherCounter = 2003;
let treasuryTransferCounter = 3003;
let quotationCounter = 4002;
let purchaseOrderCounter = 3002;
let journalEntryCounter = 104;
let generalCounter = 1000;

export function generateNextId(prefix: string = 'ID'): string {
  generalCounter += 1;
  return `${prefix}-${generalCounter}`;
}

export function generateSequentialNumber(base: number = 1000): number {
  generalCounter += 1;
  return base + generalCounter;
}

/**
 * توليد رقم فاتورة مبيعات صحيح متسلسل (Pure Integer): 1001, 1002, 1003...
 */
export function generateSalesInvoiceNumber(existingInvoices?: Array<{ invoiceNo?: string | number }>): string {
  if (existingInvoices && existingInvoices.length > 0) {
    const validNumbers = existingInvoices
      .map(inv => parseInt(String(inv.invoiceNo).replace(/\D/g, ''), 10))
      .filter(n => !isNaN(n) && n >= 1000 && n < 5000);
    if (validNumbers.length > 0) {
      const maxNum = Math.max(...validNumbers);
      salesInvoiceCounter = Math.max(salesInvoiceCounter, maxNum);
    }
  }
  salesInvoiceCounter += 1;
  return String(salesInvoiceCounter);
}

/**
 * توليد رقم فاتورة مشتريات صحيح متسلسل (Pure Integer): 5001, 5002, 5003...
 */
export function generatePurchaseInvoiceNumber(existingInvoices?: Array<{ invoiceNo?: string | number }>): string {
  if (existingInvoices && existingInvoices.length > 0) {
    const validNumbers = existingInvoices
      .map(inv => parseInt(String(inv.invoiceNo).replace(/\D/g, ''), 10))
      .filter(n => !isNaN(n) && n >= 5000 && n < 6000);
    if (validNumbers.length > 0) {
      const maxNum = Math.max(...validNumbers);
      purchaseInvoiceCounter = Math.max(purchaseInvoiceCounter, maxNum);
    }
  }
  purchaseInvoiceCounter += 1;
  return String(purchaseInvoiceCounter);
}

/**
 * توليد رقم مردود مبيعات صحيح متسلسل (Pure Integer): 6001, 6002...
 */
export function generateSalesReturnNumber(): string {
  salesReturnCounter += 1;
  return String(salesReturnCounter);
}

/**
 * توليد رقم مردود مشتريات صحيح متسلسل (Pure Integer): 7001, 7002...
 */
export function generatePurchaseReturnNumber(): string {
  purchaseReturnCounter += 1;
  return String(purchaseReturnCounter);
}

/**
 * توليد رقم سند قبض صحيح متسلسل (Pure Integer): 1001, 1002...
 */
export function generateReceiptVoucherNumber(): string {
  receiptVoucherCounter += 1;
  return String(receiptVoucherCounter);
}

/**
 * توليد رقم سند صرف صحيح متسلسل (Pure Integer): 2001, 2002...
 */
export function generatePaymentVoucherNumber(): string {
  paymentVoucherCounter += 1;
  return String(paymentVoucherCounter);
}

/**
 * توليد رقم تحويل خزينة صحيح متسلسل (Pure Integer): 3001, 3002...
 */
export function generateTreasuryTransferNumber(): string {
  treasuryTransferCounter += 1;
  return String(treasuryTransferCounter);
}

/**
 * توليد رقم عرض سعر صحيح متسلسل (Pure Integer): 4001, 4002...
 */
export function generateQuotationNumber(): string {
  quotationCounter += 1;
  return String(quotationCounter);
}

/**
 * توليد رقم أمر شراء صحيح متسلسل (Pure Integer): 3001, 3002...
 */
export function generatePurchaseOrderNumber(): string {
  purchaseOrderCounter += 1;
  return String(purchaseOrderCounter);
}

/**
 * توليد رقم قيد يومية صحيح متسلسل (Pure Integer): 101, 102, 103...
 */
export function generateJournalEntryNumber(): string {
  journalEntryCounter += 1;
  return String(journalEntryCounter);
}

export function getCurrentDateString(): string {
  return new Date().toISOString().split('T')[0];
}

export function getCurrentTimeString(): string {
  return new Date().toLocaleTimeString('ar-YE', { hour: '2-digit', minute: '2-digit', hour12: true });
}

