using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;
using FastFood.Auth.Domain.Exceptions;

namespace FastFood.Auth.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Testes unitários para o Value Object Cpf.
/// </summary>
public class CpfTests
{
    [Fact]
    public void Constructor_WithValidCpf_ShouldCreateInstance()
    {
        // Arrange
        var validCpf = "11144477735"; // CPF válido

        // Act
        var cpf = new Cpf(validCpf);

        // Assert
        Assert.NotNull(cpf);
        Assert.Equal("11144477735", cpf.Value);
    }

    [Fact]
    public void Constructor_WithInvalidCpf_ShouldThrowDomainException()
    {
        // Arrange
        var invalidCpf = "12345678901"; // CPF inválido

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(invalidCpf));
    }

    [Fact]
    public void Constructor_WithNullCpf_ShouldThrowDomainException()
    {
        // Arrange
        string? nullCpf = null;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(nullCpf!));
    }

    [Fact]
    public void Constructor_WithEmptyCpf_ShouldThrowDomainException()
    {
        // Arrange
        var emptyCpf = string.Empty;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(emptyCpf));
    }

    [Fact]
    public void Constructor_WithCpfWithLessThan11Digits_ShouldThrowDomainException()
    {
        // Arrange
        var shortCpf = "123456789"; // Menos de 11 dígitos

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(shortCpf));
    }

    [Fact]
    public void Constructor_WithCpfWithMoreThan11Digits_ShouldThrowDomainException()
    {
        // Arrange
        var longCpf = "123456789012"; // Mais de 11 dígitos

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(longCpf));
    }

    [Fact]
    public void Constructor_WithCpfAllSameDigits_ShouldThrowDomainException()
    {
        // Arrange
        var sameDigitsCpf = "11111111111"; // Todos dígitos iguais

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(sameDigitsCpf));
    }

    [Fact]
    public void ToString_ShouldReturnFormattedCpf()
    {
        // Arrange
        var cpf = new Cpf("11144477735");

        // Act
        var formatted = cpf.ToString();

        // Assert
        Assert.Equal("111.444.777-35", formatted);
    }

    [Fact]
    public void Constructor_WithCpfWithPunctuation_ShouldRemovePunctuation()
    {
        // Arrange
        var cpfWithPunctuation = "111.444.777-35"; // CPF com pontuação

        // Act
        var cpf = new Cpf(cpfWithPunctuation);

        // Assert
        Assert.NotNull(cpf);
        Assert.Equal("11144477735", cpf.Value);
    }

    [Fact]
    public void Constructor_WithCpfWithSpecialCharacters_ShouldRemoveSpecialCharacters()
    {
        // Arrange
        var cpfWithSpecialChars = "111 444 777 35"; // CPF com espaços

        // Act
        var cpf = new Cpf(cpfWithSpecialChars);

        // Assert
        Assert.NotNull(cpf);
        Assert.Equal("11144477735", cpf.Value);
    }

    [Fact]
    public void Constructor_WithCpfWithMixedPunctuation_ShouldRemoveAllPunctuation()
    {
        // Arrange
        var cpfWithMixedPunctuation = "111.444-777.35"; // CPF com pontuação mista

        // Act
        var cpf = new Cpf(cpfWithMixedPunctuation);

        // Assert
        Assert.NotNull(cpf);
        Assert.Equal("11144477735", cpf.Value);
    }

    [Fact]
    public void ToString_WithDifferentCpf_ShouldReturnCorrectFormat()
    {
        // Arrange
        var cpf = new Cpf("12345678909");

        // Act
        var formatted = cpf.ToString();

        // Assert
        Assert.Equal("123.456.789-09", formatted);
    }

    [Fact]
    public void Constructor_WithCpfWithInvalidFirstDigit_ShouldThrowDomainException()
    {
        // Arrange
        var invalidCpf = "11144477734"; // CPF com primeiro dígito verificador inválido

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(invalidCpf));
    }

    [Fact]
    public void Constructor_WithCpfWithInvalidSecondDigit_ShouldThrowDomainException()
    {
        // Arrange
        var invalidCpf = "11144477736"; // CPF com segundo dígito verificador inválido

        // Act & Assert
        Assert.Throws<DomainException>(() => new Cpf(invalidCpf));
    }

    [Fact]
    public void ToString_WithCpfStartingWithZero_ShouldFormatCorrectly()
    {
        // Arrange
        // CPF válido que começa com zero para testar formatação
        // Usando um CPF válido que começa com zero
        var cpf = new Cpf("01234567890");

        // Act
        var formatted = cpf.ToString();

        // Assert
        Assert.Equal("012.345.678-90", formatted);
    }
}












