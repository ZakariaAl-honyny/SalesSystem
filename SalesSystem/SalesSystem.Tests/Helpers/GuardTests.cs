using SalesSystem.Common.Helpers;

namespace SalesSystem.Tests.Helpers;

public class GuardTests
{
    // ═══════════════ Exception-style ═══════════════

    [Fact]
    public void AgainstNull_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Guard.AgainstNull(null, "test"));
    }

    [Fact]
    public void AgainstNull_WithValue_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstNull("hello", "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void AgainstNullOrEmpty_WithNull_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrEmpty(null, "test"));
    }

    [Fact]
    public void AgainstNullOrEmpty_WithWhitespace_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrEmpty("   ", "test"));
    }

    [Fact]
    public void AgainstNullOrEmpty_WithValue_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstNullOrEmpty("hello", "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void AgainstNegative_WithNegative_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstNegative(-1m, "test"));
    }

    [Fact]
    public void AgainstNegative_WithPositive_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstNegative(5m, "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void AgainstNegative_WithZero_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstNegative(0m, "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void AgainstZero_WithZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstZero(0m, "test"));
    }

    [Fact]
    public void AgainstZero_WithNonZero_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstZero(5m, "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void AgainstNegativeOrZero_WithNegative_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstNegativeOrZero(-1m, "test"));
    }

    [Fact]
    public void AgainstNegativeOrZero_WithZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstNegativeOrZero(0m, "test"));
    }

    [Fact]
    public void AgainstNegativeOrZero_WithPositive_DoesNotThrow()
    {
        var ex = Record.Exception(() => Guard.AgainstNegativeOrZero(5m, "test"));
        Assert.Null(ex);
    }
}
