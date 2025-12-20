using FastFood.Auth.Domain.Exceptions;

namespace FastFood.Auth.Tests.Unit.Domain.Exceptions;

/// <summary>
/// Testes unitários para DomainException.
/// </summary>
public class DomainExceptionTests
{
    [Fact]
    public void Constructor_WithNoParameters_ShouldCreateException()
    {
        // Act
        var exception = new DomainException();

        // Assert
        Assert.NotNull(exception);
        Assert.NotNull(exception.Message); // .NET sempre gera uma mensagem padrão
    }

    [Fact]
    public void Constructor_WithMessage_ShouldCreateExceptionWithMessage()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldCreateExceptionWithBoth()
    {
        // Arrange
        var message = "Test error message";
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new DomainException(message, innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}

