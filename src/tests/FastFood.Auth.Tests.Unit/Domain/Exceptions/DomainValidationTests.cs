using FastFood.Auth.Domain.Exceptions;

namespace FastFood.Auth.Tests.Unit.Domain.Exceptions;

/// <summary>
/// Testes unitários para DomainValidation.
/// </summary>
public class DomainValidationTests
{
    [Fact]
    public void ThrowIf_WhenConditionIsTrue_ShouldThrowDomainException()
    {
        // Arrange
        var message = "Test error message";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIf(true, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIf_WhenConditionIsFalse_ShouldNotThrow()
    {
        // Arrange
        var message = "Should not throw";
        
        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIf(false, message));
        
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfNull_WhenValueIsNull_ShouldThrowDomainException()
    {
        // Arrange
        object? nullValue = null;
        var message = "Value cannot be null";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfNull(nullValue, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNull_WhenValueIsNotNull_ShouldNotThrow()
    {
        // Arrange
        var value = new object();
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfNull(value, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_WhenValueIsNull_ShouldThrowDomainException()
    {
        // Arrange
        string? nullValue = null;
        var message = "Value cannot be null or whitespace";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfNullOrWhiteSpace(nullValue, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_WhenValueIsEmpty_ShouldThrowDomainException()
    {
        // Arrange
        var emptyValue = string.Empty;
        var message = "Value cannot be null or whitespace";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfNullOrWhiteSpace(emptyValue, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_WhenValueIsWhitespace_ShouldThrowDomainException()
    {
        // Arrange
        var whitespaceValue = "   ";
        var message = "Value cannot be null or whitespace";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfNullOrWhiteSpace(whitespaceValue, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_WhenValueIsValid_ShouldNotThrow()
    {
        // Arrange
        var validValue = "valid string";
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfNullOrWhiteSpace(validValue, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfLengthLessThan_WhenValueIsNull_ShouldNotThrow()
    {
        // Arrange
        string? nullValue = null;
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfLengthLessThan(nullValue, 5, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfLengthLessThan_WhenValueIsEmpty_ShouldNotThrow()
    {
        // Arrange
        var emptyValue = string.Empty;
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfLengthLessThan(emptyValue, 5, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfLengthLessThan_WhenValueLengthIsLessThanMinLength_ShouldThrowDomainException()
    {
        // Arrange
        var shortValue = "abc";
        var minLength = 5;
        var message = "Value is too short";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfLengthLessThan(shortValue, minLength, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfLengthLessThan_WhenValueLengthIsEqualToMinLength_ShouldNotThrow()
    {
        // Arrange
        var value = "abcde";
        var minLength = 5;
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfLengthLessThan(value, minLength, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfLengthLessThan_WhenValueLengthIsGreaterThanMinLength_ShouldNotThrow()
    {
        // Arrange
        var value = "abcdef";
        var minLength = 5;
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfLengthLessThan(value, minLength, message));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfLessOrEqual_WhenValueIsLessThanThreshold_ShouldThrowDomainException()
    {
        // Arrange
        var value = 5m;
        var threshold = 10m;
        var message = "Value is too small";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfLessOrEqual(value, threshold, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfLessOrEqual_WhenValueIsEqualToThreshold_ShouldThrowDomainException()
    {
        // Arrange
        var value = 10m;
        var threshold = 10m;
        var message = "Value is too small";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => DomainValidation.ThrowIfLessOrEqual(value, threshold, message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ThrowIfLessOrEqual_WhenValueIsGreaterThanThreshold_ShouldNotThrow()
    {
        // Arrange
        var value = 15m;
        var threshold = 10m;
        var message = "Should not throw";

        // Act
        var exception = Record.Exception(() => DomainValidation.ThrowIfLessOrEqual(value, threshold, message));

        // Assert
        Assert.Null(exception);
    }
}



