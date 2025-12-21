using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using FastFood.Auth.Domain.Exceptions;

namespace FastFood.Auth.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Email.
/// </summary>
public class EmailTests
{
    [Fact]
    public void Constructor_WithValidEmail_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Fact]
    public void Constructor_WithInvalidEmail_ShouldThrowDomainException()
    {
        // Arrange
        var invalidEmail = "invalid-email";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(invalidEmail));
    }

    [Fact]
    public void Constructor_WithNullEmail_ShouldThrowDomainException()
    {
        // Arrange
        string? nullEmail = null;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(nullEmail!));
    }

    [Fact]
    public void Constructor_WithEmptyEmail_ShouldThrowDomainException()
    {
        // Arrange
        var emptyEmail = string.Empty;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emptyEmail));
    }

    [Fact]
    public void Constructor_WithEmailWithoutAt_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithoutAt = "invalidemail.com";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithoutAt));
    }

    [Fact]
    public void Constructor_WithEmailWithoutDomain_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithoutDomain = "test@";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithoutDomain));
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var emailValue = "test@example.com";
        var email = new Email(emailValue);

        // Act
        var result = email.ToString();

        // Assert
        Assert.Equal(emailValue, result);
    }

    [Fact]
    public void Constructor_WithEmailWithoutDot_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithoutDot = "test@exampledomain";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithoutDot));
    }

    [Fact]
    public void Constructor_WithValidEmailWithSubdomain_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test@mail.example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Fact]
    public void Constructor_WithEmailWithWhitespace_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithWhitespace = "test @example.com";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithWhitespace));
    }

    [Fact]
    public void Constructor_WithEmailWithMultipleAt_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithMultipleAt = "test@@example.com";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithMultipleAt));
    }

    [Fact]
    public void Constructor_WithEmailWithOnlyWhitespace_ShouldThrowDomainException()
    {
        // Arrange
        var emailWithOnlyWhitespace = "   ";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Email(emailWithOnlyWhitespace));
    }

    [Fact]
    public void Constructor_WithValidEmailWithPlusSign_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test+tag@example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Fact]
    public void Constructor_WithValidEmailWithNumbers_ShouldCreateInstance()
    {
        // Arrange
        var validEmail = "test123@example123.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }
}












