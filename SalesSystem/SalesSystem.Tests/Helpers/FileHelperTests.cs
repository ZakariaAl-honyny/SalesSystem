using SalesSystem.Common.Helpers;

namespace SalesSystem.Tests.Helpers;

public class FileHelperTests : IDisposable
{
    private readonly string _tempDir;

    public FileHelperTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "FileHelperTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    // ═══════════════ ReadAllText ═══════════════

    [Fact]
    public void ReadAllText_WithExistingFile_ReturnsContent()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "test.txt");
        File.WriteAllText(filePath, "Hello, World!");

        // Act
        var result = FileHelper.ReadAllText(filePath);

        // Assert
        Assert.Equal("Hello, World!", result);
    }

    [Fact]
    public void ReadAllText_WithNonExistingFile_ReturnsNull()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "nonexistent.txt");

        // Act
        var result = FileHelper.ReadAllText(filePath);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReadAllText_WithNullPath_ReturnsNull()
    {
        // Act
        var result = FileHelper.ReadAllText(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReadAllText_WithEmptyPath_ReturnsNull()
    {
        // Act
        var result = FileHelper.ReadAllText(string.Empty);

        // Assert
        Assert.Null(result);
    }

    // ═══════════════ TryWriteAllText ═══════════════

    [Fact]
    public void TryWriteAllText_WritesContent_ReturnsTrue()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "write_test.txt");

        // Act
        var result = FileHelper.TryWriteAllText(filePath, "Test content");

        // Assert
        Assert.True(result);
        Assert.True(File.Exists(filePath));
        Assert.Equal("Test content", File.ReadAllText(filePath));
    }

    [Fact]
    public void TryWriteAllText_CreatesSubdirectories_ReturnsTrue()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "sub", "nested", "deep_test.txt");

        // Act
        var result = FileHelper.TryWriteAllText(filePath, "Nested content");

        // Assert
        Assert.True(result);
        Assert.True(File.Exists(filePath));
        Assert.Equal("Nested content", File.ReadAllText(filePath));
    }

    // ═══════════════ TryDelete ═══════════════

    [Fact]
    public void TryDelete_WithExistingFile_ReturnsTrue()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "delete_test.txt");
        File.WriteAllText(filePath, "To be deleted");

        // Act
        var result = FileHelper.TryDelete(filePath);

        // Assert
        Assert.True(result);
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void TryDelete_WithNonExistingFile_ReturnsTrue()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "already_gone.txt");

        // Act
        var result = FileHelper.TryDelete(filePath);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryDelete_WithNullPath_ReturnsTrue()
    {
        // Act
        var result = FileHelper.TryDelete(null);

        // Assert
        Assert.True(result);
    }

    // ═══════════════ EnsureDirectoryExists ═══════════════

    [Fact]
    public void EnsureDirectoryExists_CreatesNewDirectory_ReturnsTrue()
    {
        // Arrange
        var dirPath = Path.Combine(_tempDir, "new_dir");

        // Act
        var result = FileHelper.EnsureDirectoryExists(dirPath);

        // Assert
        Assert.True(result);
        Assert.True(Directory.Exists(dirPath));
    }

    [Fact]
    public void EnsureDirectoryExists_WithExistingDirectory_ReturnsTrue()
    {
        // Arrange
        var dirPath = Path.Combine(_tempDir, "existing_dir");
        Directory.CreateDirectory(dirPath);

        // Act
        var result = FileHelper.EnsureDirectoryExists(dirPath);

        // Assert
        Assert.True(result);
        Assert.True(Directory.Exists(dirPath));
    }

    [Fact]
    public void EnsureDirectoryExists_WithNestedPath_CreatesAllDirectories_ReturnsTrue()
    {
        // Arrange
        var dirPath = Path.Combine(_tempDir, "parent", "child", "grandchild");

        // Act
        var result = FileHelper.EnsureDirectoryExists(dirPath);

        // Assert
        Assert.True(result);
        Assert.True(Directory.Exists(dirPath));
    }

    [Fact]
    public void EnsureDirectoryExists_WithNullPath_ReturnsFalse()
    {
        // Act
        var result = FileHelper.EnsureDirectoryExists(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EnsureDirectoryExists_WithEmptyPath_ReturnsFalse()
    {
        // Act
        var result = FileHelper.EnsureDirectoryExists(string.Empty);

        // Assert
        Assert.False(result);
    }

    // ═══════════════ SanitizeFileName ═══════════════

    [Fact]
    public void SanitizeFileName_WithInvalidChars_ReturnsCleanedName()
    {
        // Act
        var result = FileHelper.SanitizeFileName("test<file>:name?.txt");

        // Assert
        Assert.DoesNotContain('<', result);
        Assert.DoesNotContain('>', result);
        Assert.DoesNotContain(':', result);
        Assert.DoesNotContain('?', result);
        Assert.Contains("test", result);
        Assert.Contains(".txt", result);
    }

    [Fact]
    public void SanitizeFileName_WithNoInvalidChars_ReturnsSameName()
    {
        // Arrange
        var original = "valid-file-name.txt";

        // Act
        var result = FileHelper.SanitizeFileName(original);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void SanitizeFileName_WithAllInvalidChars_ReturnsDefaultName()
    {
        // Act
        var result = FileHelper.SanitizeFileName("<>:\"/\\|?*");

        // Assert
        Assert.Equal("file", result);
    }

    [Fact]
    public void SanitizeFileName_WithWhitespaceOnly_ReturnsDefaultName()
    {
        // Act
        var result = FileHelper.SanitizeFileName("   ");

        // Assert
        Assert.Equal("file", result);
    }

    [Fact]
    public void SanitizeFileName_WithNullInput_ThrowsArgumentNullException()
    {
        // The method does not guard against null — LINQ's Where throws on null source.
        Assert.Throws<ArgumentNullException>(() => FileHelper.SanitizeFileName(null!));
    }

    // ═══════════════ GetUniqueFileName ═══════════════

    [Fact]
    public void GetUniqueFileName_WithNonExistingFile_ReturnsSameName()
    {
        // Act
        var result = FileHelper.GetUniqueFileName(_tempDir, "unique.txt");

        // Assert
        Assert.Equal("unique.txt", result);
    }

    [Fact]
    public void GetUniqueFileName_WithExistingFile_ReturnsNumberedName()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "existing.txt");
        File.WriteAllText(filePath, "occupied");

        // Act
        var result = FileHelper.GetUniqueFileName(_tempDir, "existing.txt");

        // Assert
        Assert.Equal("existing_1.txt", result);
    }

    [Fact]
    public void GetUniqueFileName_WithMultipleExisting_ReturnsNextNumber()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_tempDir, "multi.txt"), "first");
        File.WriteAllText(Path.Combine(_tempDir, "multi_1.txt"), "second");
        File.WriteAllText(Path.Combine(_tempDir, "multi_2.txt"), "third");

        // Act
        var result = FileHelper.GetUniqueFileName(_tempDir, "multi.txt");

        // Assert
        Assert.Equal("multi_3.txt", result);
    }

    [Fact]
    public void GetUniqueFileName_WithNullDirectory_DoesNotThrow()
    {
        // Act
        var result = FileHelper.GetUniqueFileName(null, "file.txt");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetUniqueFileName_WithFileWithoutExtension_ReturnsNumberedName()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "noext");
        File.WriteAllText(filePath, "no extension");

        // Act
        var result = FileHelper.GetUniqueFileName(_tempDir, "noext");

        // Assert
        Assert.Equal("noext_1", result);
    }

    // ═══════════════ GetFileSize ═══════════════

    [Fact]
    public void GetFileSize_WithExistingFile_ReturnsLength()
    {
        // Arrange
        var filePath = Path.Combine(_tempDir, "size_test.txt");
        File.WriteAllText(filePath, "12345");

        // Act
        var result = FileHelper.GetFileSize(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void GetFileSize_WithNonExistingFile_ReturnsNull()
    {
        // Act
        var result = FileHelper.GetFileSize(Path.Combine(_tempDir, "ghost.txt"));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetFileSize_WithNullPath_ReturnsNull()
    {
        // Act
        var result = FileHelper.GetFileSize(null);

        // Assert
        Assert.Null(result);
    }
}
