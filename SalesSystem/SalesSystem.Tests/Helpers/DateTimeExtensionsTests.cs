using SalesSystem.Common.Helpers;

namespace SalesSystem.Tests.Helpers;

public class DateTimeExtensionsTests
{
    // ═══════════════ StartOfWeek ═══════════════

    [Fact]
    public void StartOfWeek_OnWednesday_ReturnsPreviousSaturday()
    {
        // الأربعاء 2026-07-22 ← أول الأسبوع السبت 2026-07-18
        var wednesday = new DateTime(2026, 7, 22);
        var expected = new DateTime(2026, 7, 18);

        var result = wednesday.StartOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfWeek_OnSaturday_ReturnsSameDay()
    {
        // السبت 2026-07-18 ← نفسه
        var saturday = new DateTime(2026, 7, 18);
        var expected = new DateTime(2026, 7, 18);

        var result = saturday.StartOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfWeek_OnFriday_ReturnsPreviousSaturday()
    {
        // الجمعة 2026-07-24 ← السبت 2026-07-18
        var friday = new DateTime(2026, 7, 24);
        var expected = new DateTime(2026, 7, 18);

        var result = friday.StartOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfWeek_OnMonday_ReturnsPreviousSaturday()
    {
        // الاثنين 2026-07-20 ← السبت 2026-07-18
        var monday = new DateTime(2026, 7, 20);
        var expected = new DateTime(2026, 7, 18);

        var result = monday.StartOfWeek();

        Assert.Equal(expected, result);
    }

    // ═══════════════ EndOfWeek ═══════════════

    [Fact]
    public void EndOfWeek_OnWednesday_ReturnsNextFriday()
    {
        // الأربعاء 2026-07-22 ← آخر الأسبوع الجمعة 2026-07-24
        var wednesday = new DateTime(2026, 7, 22);
        var expected = new DateTime(2026, 7, 24);

        var result = wednesday.EndOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfWeek_OnSaturday_ReturnsNextFriday()
    {
        // السبت 2026-07-18 ← الجمعة 2026-07-24
        var saturday = new DateTime(2026, 7, 18);
        var expected = new DateTime(2026, 7, 24);

        var result = saturday.EndOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfWeek_OnFriday_ReturnsSameDay()
    {
        // الجمعة 2026-07-24 ← نفسه
        var friday = new DateTime(2026, 7, 24);
        var expected = new DateTime(2026, 7, 24);

        var result = friday.EndOfWeek();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfWeek_OnMonday_ReturnsNextFriday()
    {
        // الاثنين 2026-07-20 ← الجمعة 2026-07-24
        var monday = new DateTime(2026, 7, 20);
        var expected = new DateTime(2026, 7, 24);

        var result = monday.EndOfWeek();

        Assert.Equal(expected, result);
    }

    // ═══════════════ StartOfMonth ═══════════════

    [Fact]
    public void StartOfMonth_OnMidMonth_ReturnsFirstDay()
    {
        var mid = new DateTime(2026, 7, 15);
        var expected = new DateTime(2026, 7, 1);

        var result = mid.StartOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfMonth_OnLastDay_ReturnsFirstDay()
    {
        var lastDay = new DateTime(2026, 7, 31);
        var expected = new DateTime(2026, 7, 1);

        var result = lastDay.StartOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfMonth_OnFirstDay_ReturnsSameDay()
    {
        var firstDay = new DateTime(2026, 7, 1);
        var expected = new DateTime(2026, 7, 1);

        var result = firstDay.StartOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfMonth_InDecember_ReturnsFirstDay()
    {
        var midDec = new DateTime(2026, 12, 25);
        var expected = new DateTime(2026, 12, 1);

        var result = midDec.StartOfMonth();

        Assert.Equal(expected, result);
    }

    // ═══════════════ EndOfMonth ═══════════════

    [Fact]
    public void EndOfMonth_InJuly_Returns31()
    {
        var midJuly = new DateTime(2026, 7, 15);
        var expected = new DateTime(2026, 7, 31);

        var result = midJuly.EndOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfMonth_InFebruaryNonLeap_Returns28()
    {
        var midFeb = new DateTime(2026, 2, 15);
        var expected = new DateTime(2026, 2, 28);

        var result = midFeb.EndOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfMonth_InFebruaryLeap_Returns29()
    {
        var midFeb = new DateTime(2024, 2, 15);
        var expected = new DateTime(2024, 2, 29);

        var result = midFeb.EndOfMonth();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EndOfMonth_InDecember_Returns31()
    {
        var midDec = new DateTime(2026, 12, 15);
        var expected = new DateTime(2026, 12, 31);

        var result = midDec.EndOfMonth();

        Assert.Equal(expected, result);
    }

    // ═══════════════ IsWeekend ═══════════════

    [Fact]
    public void IsWeekend_OnFriday_ReturnsTrue()
    {
        var friday = new DateTime(2026, 7, 24); // الجمعة

        var result = friday.IsWeekend();

        Assert.True(result);
    }

    [Fact]
    public void IsWeekend_OnSaturday_ReturnsTrue()
    {
        var saturday = new DateTime(2026, 7, 25); // السبت

        var result = saturday.IsWeekend();

        Assert.True(result);
    }

    [Fact]
    public void IsWeekend_OnWednesday_ReturnsFalse()
    {
        var wednesday = new DateTime(2026, 7, 22); // الأربعاء

        var result = wednesday.IsWeekend();

        Assert.False(result);
    }

    [Fact]
    public void IsWeekend_OnMonday_ReturnsFalse()
    {
        var monday = new DateTime(2026, 7, 20); // الاثنين

        var result = monday.IsWeekend();

        Assert.False(result);
    }

    [Fact]
    public void IsWeekend_OnSunday_ReturnsFalse()
    {
        var sunday = new DateTime(2026, 7, 19); // الأحد

        var result = sunday.IsWeekend();

        Assert.False(result);
    }

    // ═══════════════ IsFriday ═══════════════

    [Fact]
    public void IsFriday_OnFriday_ReturnsTrue()
    {
        var friday = new DateTime(2026, 7, 24);

        var result = friday.IsFriday();

        Assert.True(result);
    }

    [Fact]
    public void IsFriday_OnSaturday_ReturnsFalse()
    {
        var saturday = new DateTime(2026, 7, 25);

        var result = saturday.IsFriday();

        Assert.False(result);
    }

    [Fact]
    public void IsFriday_OnWednesday_ReturnsFalse()
    {
        var wednesday = new DateTime(2026, 7, 22);

        var result = wednesday.IsFriday();

        Assert.False(result);
    }

    // ═══════════════ IsSaturday ═══════════════

    [Fact]
    public void IsSaturday_OnSaturday_ReturnsTrue()
    {
        var saturday = new DateTime(2026, 7, 25);

        var result = saturday.IsSaturday();

        Assert.True(result);
    }

    [Fact]
    public void IsSaturday_OnFriday_ReturnsFalse()
    {
        var friday = new DateTime(2026, 7, 24);

        var result = friday.IsSaturday();

        Assert.False(result);
    }

    [Fact]
    public void IsSaturday_OnWednesday_ReturnsFalse()
    {
        var wednesday = new DateTime(2026, 7, 22);

        var result = wednesday.IsSaturday();

        Assert.False(result);
    }
}
