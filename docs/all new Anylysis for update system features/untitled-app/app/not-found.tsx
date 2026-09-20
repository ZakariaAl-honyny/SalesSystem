import Link from 'next/link';

export default function NotFound() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center p-4 bg-[#f0f2f5] text-slate-800 text-center">
      <h1 className="text-4xl font-black text-slate-900 mb-2 font-mono">404</h1>
      <p className="text-slate-600 mb-4 font-semibold">الصفحة المطلوبة غير موجودة</p>
      <Link
        href="/"
        className="px-4 py-2 bg-sky-700 hover:bg-sky-800 text-white rounded-lg font-bold text-sm shadow transition"
      >
        العودة للرئيسية
      </Link>
    </div>
  );
}
