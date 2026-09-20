/**
 * Arabic Number to Words Converter (Tafqeet)
 * تفقيط المبالغ المالية باللغة العربية مع مراعاة قواعد الصرف والمفرد والمثنى والجمع
 */

const ONES = ['', 'واحد', 'اثنان', 'ثلاثة', 'أربعة', 'خمسة', 'ستة', 'سبعة', 'ثمانية', 'تسعة'];
const TENS_SPECIAL = ['عشرة', 'أحد عشر', 'اثنا عشر', 'ثلاثة عشر', 'أربعة عشر', 'خمسة عشر', 'ستة عشر', 'سبعة عشر', 'ثمانية عشر', 'تسعة عشر'];
const TENS = ['', 'عشرة', 'عشرون', 'ثلاثون', 'أربعون', 'خمسون', 'ستون', 'سبعون', 'ثمانون', 'تسعون'];
const HUNDREDS = ['', 'مائة', 'مائتان', 'ثلاثمائة', 'أربعمائة', 'خمسمائة', 'ستمائة', 'سبعمائة', 'ثمانمائة', 'تسعمائة'];

function convertChunk(num: number): string {
  if (num === 0) return '';
  let result = '';

  const h = Math.floor(num / 100);
  const remainder = num % 100;

  if (h > 0) {
    result += HUNDREDS[h];
  }

  if (remainder > 0) {
    if (result) result += ' و';

    if (remainder < 10) {
      result += ONES[remainder];
    } else if (remainder < 20) {
      result += TENS_SPECIAL[remainder - 10];
    } else {
      const o = remainder % 10;
      const t = Math.floor(remainder / 10);
      if (o > 0) {
        result += ONES[o] + ' و' + TENS[t];
      } else {
        result += TENS[t];
      }
    }
  }

  return result;
}

export function tafqeet(amount: number, currency: string = 'ريال يمني'): string {
  if (amount === 0) return `صفر ${currency} فقط لا غير`;
  if (isNaN(amount)) return '';

  const rounded = Math.abs(Math.round(amount));
  if (rounded === 0) return `صفر ${currency} فقط لا غير`;

  const millions = Math.floor(rounded / 1000000);
  const thousands = Math.floor((rounded % 1000000) / 1000);
  const units = rounded % 1000;

  const parts: string[] = [];

  if (millions > 0) {
    if (millions === 1) {
      parts.push('مليون');
    } else if (millions === 2) {
      parts.push('مليونان');
    } else if (millions >= 3 && millions <= 10) {
      parts.push(convertChunk(millions) + ' ملايين');
    } else {
      parts.push(convertChunk(millions) + ' مليون');
    }
  }

  if (thousands > 0) {
    if (thousands === 1) {
      parts.push('ألف');
    } else if (thousands === 2) {
      parts.push('ألفان');
    } else if (thousands >= 3 && thousands <= 10) {
      parts.push(convertChunk(thousands) + ' آلاف');
    } else {
      parts.push(convertChunk(thousands) + ' ألف');
    }
  }

  if (units > 0) {
    parts.push(convertChunk(units));
  }

  const text = parts.join(' و');
  return `فقط ${text} ${currency} لا غير`;
}
