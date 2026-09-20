'use client';

import React from 'react';
import { ProductItem } from '@/types/pos';
import { ProductSelectionModal, ProductSelectionResult } from './ProductSelectionModal';

export interface SalesSmartF3LookupModalProps {
  products: ProductItem[];
  selectedCurrency: string;
  onAddProduct: (
    product: ProductItem, 
    unit: string, 
    quantity: number, 
    price?: number, 
    discount?: number, 
    notes?: string, 
    expiryDate?: string, 
    batchNo?: string
  ) => void;
  onClose: () => void;
  activeWarehouseName?: string;
  initialSelectedProductId?: string;
}

export function SalesSmartF3LookupModal({
  products,
  selectedCurrency,
  onAddProduct,
  onClose,
  activeWarehouseName,
  initialSelectedProductId,
}: SalesSmartF3LookupModalProps) {
  return (
    <ProductSelectionModal
      isOpen={true}
      products={products}
      currency={selectedCurrency}
      activeWarehouseName={activeWarehouseName}
      initialSelectedProductId={initialSelectedProductId}
      onConfirmItem={(result: ProductSelectionResult, stayOpen?: boolean) => {
        onAddProduct(
          result.product,
          result.selectedUnit,
          result.quantity,
          result.unitPrice,
          result.discount,
          result.notes,
          result.expiryDate,
          result.batchNo
        );
        if (!stayOpen) {
          onClose();
        }
      }}
      onClose={onClose}
    />
  );
}

