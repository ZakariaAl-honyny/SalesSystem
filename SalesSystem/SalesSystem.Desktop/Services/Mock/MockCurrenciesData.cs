using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للعملات وأسعار الصرف — مبنية من currencies.ts المرجعية.
/// 3 عملات + 4 سجلات أسعار صرف.
/// </summary>
internal static class MockCurrenciesData
{
    // ── العملات ──────────────────────────────────────────
    public static List<CurrencyDto> Currencies { get; } = new()
    {
        new CurrencyDto(Id: 1, Code: "YER", Name: "ريال يمني",  SubUnit: "فلس",  IsLocal: true),
        new CurrencyDto(Id: 2, Code: "USD", Name: "دولار أمريكي", SubUnit: "سنت", IsLocal: false),
        new CurrencyDto(Id: 3, Code: "SAR", Name: "ريال سعودي",  SubUnit: "فلس",  IsLocal: false),
    };

    // ── أسعار الصرف ──────────────────────────────────────
    public static List<CurrencyRateDto> Rates { get; } = new()
    {
        // ── أسعار الدولار ──
        new CurrencyRateDto(
            Id: 1, CurrencyId: 2, CurrencyName: "دولار أمريكي",
            Rate: 700m, FromDate: new DateTime(2026, 5, 25), ToDate: null,
            IsCurrent: true, MinRate: 500m, MaxRate: 850m),

        new CurrencyRateDto(
            Id: 2, CurrencyId: 2, CurrencyName: "دولار أمريكي",
            Rate: 600m, FromDate: new DateTime(2026, 5, 24), ToDate: new DateTime(2026, 5, 24),
            IsCurrent: false, MinRate: 500m, MaxRate: 850m),

        new CurrencyRateDto(
            Id: 3, CurrencyId: 2, CurrencyName: "دولار أمريكي",
            Rate: 500m, FromDate: new DateTime(2026, 5, 23), ToDate: new DateTime(2026, 5, 23),
            IsCurrent: false, MinRate: 500m, MaxRate: 850m),

        // ── أسعار الريال السعودي ──
        new CurrencyRateDto(
            Id: 4, CurrencyId: 3, CurrencyName: "ريال سعودي",
            Rate: 900m, FromDate: new DateTime(2026, 5, 25), ToDate: null,
            IsCurrent: true, MinRate: 130m, MaxRate: 1000m),
    };
}
