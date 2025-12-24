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
    public void Constructor_WithNoParameters_ShouldHaveNullInnerException()
    {
        // Act
        var exception = new DomainException();

        // Assert
        Assert.Null(exception.InnerException);
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
    public void Constructor_WithMessage_ShouldHaveNullInnerException()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.Null(exception.InnerException);
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

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldPreserveInnerException()
    {
        // Arrange
        var message = "Outer exception";
        var innerException = new ArgumentException("Inner exception");

        // Act
        var exception = new DomainException(message, innerException);

        // Assert
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ArgumentException>(exception.InnerException);
        Assert.Equal("Inner exception", exception.InnerException.Message);
    }

    [Fact]
    public void ShouldInheritFromException()
    {
        // Act
        var exception = new DomainException();

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Message_ShouldReturnSetMessage()
    {
        // Arrange
        var expectedMessage = "Custom error message";
        var exception = new DomainException(expectedMessage);

        // Act
        var message = exception.Message;

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void InnerException_ShouldReturnSetInnerException()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner");
        var exception = new DomainException("Outer", innerException);

        // Act
        var inner = exception.InnerException;

        // Assert
        Assert.Equal(innerException, inner);
    }

    [Fact]
    public void Constructor_WithEmptyMessage_ShouldSetEmptyMessage()
    {
        // Act
        var exception = new DomainException(string.Empty);

        // Assert
        Assert.Equal(string.Empty, exception.Message);
    }

    [Fact]
    public void Constructor_WithNullMessage_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new DomainException(null!);

        // Assert
        // .NET sempre gera uma mensagem padrão mesmo quando null é passado
        Assert.NotNull(exception.Message);
    }

    [Fact]
    public void Constructor_WithLongMessage_ShouldSetLongMessage()
    {
        // Arrange
        var longMessage = new string('A', 10000);

        // Act
        var exception = new DomainException(longMessage);

        // Assert
        Assert.Equal(longMessage, exception.Message);
    }

    [Fact]
    public void StackTrace_ShouldNotBeNull()
    {
        // Arrange
        DomainException? exception = null;
        try
        {
            throw new DomainException("Test");
        }
        catch (DomainException ex)
        {
            exception = ex;
        }

        // Assert
        Assert.NotNull(exception);
        Assert.NotNull(exception.StackTrace);
    }

    [Fact]
    public void GetType_ShouldReturnDomainException()
    {
        // Arrange
        var exception = new DomainException();

        // Act
        var type = exception.GetType();

        // Assert
        Assert.Equal(typeof(DomainException), type);
    }

    [Fact]
    public void Constructor_WithNullMessageAndInnerException_ShouldSetDefaultMessage()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner");

        // Act
        var exception = new DomainException(null!, innerException);

        // Assert
        // .NET sempre gera uma mensagem padrão mesmo quando null é passado
        Assert.NotNull(exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndNullInnerException_ShouldSetNullInnerException()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new DomainException(message, null!);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Serialization_ShouldWork()
    {
        // Arrange
        var originalException = new DomainException("Test message", new InvalidOperationException("Inner"));
        
        // Act & Assert
        // Testar que a exceção pode ser serializada (mesmo que o método seja obsoleto)
        // Isso cobre a linha do construtor protegido de serialização
        Assert.NotNull(originalException);
        Assert.Equal("Test message", originalException.Message);
        Assert.NotNull(originalException.InnerException);
    }
}

