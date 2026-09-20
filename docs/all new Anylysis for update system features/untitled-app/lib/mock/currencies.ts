import { CurrencyItem, CurrencyRateItem } from '@/types/pos';

export const INITIAL_CURRENCIES: CurrencyItem[] = [
  { id: 'CUR-01', name: 'محلي', subUnit: 'فلس', code: 'YR', accountsCount: 2, isLocal: true },
  { id: 'CUR-02', name: 'دولار', subUnit: 'سنت', code: 'USD', accountsCount: 1, isLocal: false },
  { id: 'CUR-03', name: 'سعودي', subUnit: 'فلس', code: 'SAR', accountsCount: 0, isLocal: false },
];

export const INITIAL_CURRENCY_RATES: CurrencyRateItem[] = [
  { id: 'CR-01', currencyName: 'دولار', rate: 700, minRate: 500, maxRate: 850, fromDate: '2026-05-25', toDate: '', isCurrent: true },
  { id: 'CR-02', currencyName: 'دولار', rate: 700, minRate: 500, maxRate: 850, fromDate: '2026-05-25', toDate: '', isCurrent: false },
  { id: 'CR-03', currencyName: 'دولار', rate: 600, minRate: 500, maxRate: 850, fromDate: '2026-05-24', toDate: '2026-05-24', isCurrent: false },
  { id: 'CR-04', currencyName: 'دولار', rate: 500, minRate: 500, maxRate: 850, fromDate: '2026-05-23', toDate: '2026-05-23', isCurrent: false },
  { id: 'CR-05', currencyName: 'سعودي', rate: 900, minRate: 130, maxRate: 1000, fromDate: '2026-05-25', toDate: '', isCurrent: true },
];
