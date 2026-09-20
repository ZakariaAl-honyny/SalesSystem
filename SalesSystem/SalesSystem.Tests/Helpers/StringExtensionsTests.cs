using SalesSystem.Common.Helpers;

namespace SalesSystem.Tests.Helpers;

public class StringExtensionsTests
{
    // ═══════════════ IsNullOrWhiteSpace ═══════════════

    [Fact]
    public void IsNullOrWhiteSpace_WithNull_ReturnsTrue()
    {
        string? value = null;
        var result = value.IsNullOrWhiteSpace();
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrWhiteSpace_WithEmpty_ReturnsTrue()
    {
        var result = string.Empty.IsNullOrWhiteSpace();
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrWhiteSpace_WithWhitespace_ReturnsTrue()
    {
        var result = "   ".IsNullOrWhiteSpace();
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrWhiteSpace_WithValidString_ReturnsFalse()
    {
        var result = "أحمد".IsNullOrWhiteSpace();
        Assert.False(result);
    }

    // ═══════════════ HasValue ═══════════════

    [Fact]
    public void HasValue_WithNull_ReturnsFalse()
    {
        string? value = null;
        var result = value.HasValue();
        Assert.False(result);
    }

    [Fact]
    public void HasValue_WithEmpty_ReturnsFalse()
    {
        var result = string.Empty.HasValue();
        Assert.False(result);
    }

    [Fact]
    public void HasValue_WithValidString_ReturnsTrue()
    {
        var result = "test".HasValue();
        Assert.True(result);
    }

    // ═══════════════ Truncate ═══════════════

    [Fact]
    public void Truncate_WhenShorterThanMax_ReturnsOriginal()
    {
        var result = "abc".Truncate(5);
        Assert.Equal("abc", result);
    }

    [Fact]
    public void Truncate_WhenEqualToMax_ReturnsOriginal()
    {
        var result = "abcde".Truncate(5);
        Assert.Equal("abcde", result);
    }

    [Fact]
    public void Truncate_WhenLongerThanMax_ReturnsTruncated()
    {
        var result = "hello world".Truncate(5);
        Assert.Equal("hello", result);
    }

    [Fact]
    public void Truncate_WithMaxLengthZero_ReturnsEmpty()
    {
        var result = "anything".Truncate(0);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Truncate_WithNull_ReturnsEmpty()
    {
        string? value = null;
        var result = value.Truncate(5);
        Assert.Equal(string.Empty, result);
    }

    // ═══════════════ ToNullIfEmpty ═══════════════

    [Fact]
    public void ToNullIfEmpty_WithNull_ReturnsNull()
    {
        string? value = null;
        var result = value.ToNullIfEmpty();
        Assert.Null(result);
    }

    [Fact]
    public void ToNullIfEmpty_WithEmpty_ReturnsNull()
    {
        var result = string.Empty.ToNullIfEmpty();
        Assert.Null(result);
    }

    [Fact]
    public void ToNullIfEmpty_WithWhitespace_ReturnsNull()
    {
        var result = "   ".ToNullIfEmpty();
        Assert.Null(result);
    }

    [Fact]
    public void ToNullIfEmpty_WithValidString_ReturnsOriginal()
    {
        var result = "hello".ToNullIfEmpty();
        Assert.Equal("hello", result);
    }

    // ═══════════════ DefaultIfEmpty ═══════════════

    [Fact]
    public void DefaultIfEmpty_WithNull_ReturnsDefault()
    {
        string? value = null;
        var result = value.DefaultIfEmpty("default");
        Assert.Equal("default", result);
    }

    [Fact]
    public void DefaultIfEmpty_WithEmpty_ReturnsDefault()
    {
        var result = string.Empty.DefaultIfEmpty("default");
        Assert.Equal("default", result);
    }

    [Fact]
    public void DefaultIfEmpty_WithWhitespace_ReturnsDefault()
    {
        var result = "   ".DefaultIfEmpty("default");
        Assert.Equal("default", result);
    }

    [Fact]
    public void DefaultIfEmpty_WithValidString_ReturnsOriginal()
    {
        var result = "original".DefaultIfEmpty("default");
        Assert.Equal("original", result);
    }
}
