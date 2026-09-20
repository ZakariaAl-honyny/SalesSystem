import type {Metadata} from 'next';
import './globals.css';

export const metadata: Metadata = {
  title: 'اليمن للمحاسبة ونقاط البيع | POS & Accounting System',
  description: 'نظام ERP متكامل لإدارة المبيعات والمستودعات المتعددة ونقاط البيع والمحاسبة المالية بنمط C# N-Layer Architecture للسوق اليمني (ريال يمني YER)',
  openGraph: {
    title: 'اليمن للمحاسبة ونقاط البيع | POS & Accounting System',
    description: 'نظام ERP متكامل لإدارة المبيعات والمستودعات المتعددة ونقاط البيع والمحاسبة المالية بنمط C# N-Layer Architecture للسوق اليمني (ريال يمني YER)',
    type: 'website',
  },
  twitter: {
    card: 'summary_large_image',
    title: 'اليمن للمحاسبة ونقاط البيع | POS & Accounting System',
    description: 'نظام ERP متكامل لإدارة المبيعات والمستودعات المتعددة ونقاط البيع والمحاسبة المالية بنمط C# N-Layer Architecture للسوق اليمني (ريال يمني YER)',
  },
};

export default function RootLayout({children}: {children: React.ReactNode}) {
  return (
    <html lang="ar" dir="rtl">
      <head>
        <link rel="preconnect" href="https://fonts.googleapis.com" />
        <link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin="anonymous" />
        <link
          href="https://fonts.googleapis.com/css2?family=Cairo:wght@300;400;500;600;700;800;900&family=JetBrains+Mono:wght@400;500;600;700&display=swap"
          rel="stylesheet"
        />
      </head>
      <body className="font-sans antialiased bg-[#f0f2f5] text-slate-800 select-none overflow-x-hidden min-h-screen" suppressHydrationWarning>
        {children}
      </body>
    </html>
  );
}
