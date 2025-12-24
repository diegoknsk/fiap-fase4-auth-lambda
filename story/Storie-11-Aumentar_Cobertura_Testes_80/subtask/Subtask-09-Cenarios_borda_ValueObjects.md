# Subtask 09: Adicionar cenários de borda em Value Objects

## Descrição
Adicionar testes adicionais para cenários de borda, valores limite e formatos especiais nos Value Objects (Cpf e Email).

## Arquivos a Testar
- `src/FastFood.Auth.Domain/Entities/CustomerIdentification/ValueObects/Cpf.cs`
- `src/FastFood.Auth.Domain/Entities/CustomerIdentification/ValueObects/Email.cs`

## Passos de Implementação

1. **Analisar testes existentes:**
   - Verificar quais cenários já estão cobertos
   - Identificar lacunas de cobertura

2. **Adicionar testes para Cpf:**

   **a) Cenários de Formatação:**
   - `Constructor_WithFormattedCpf_ShouldNormalize`
   - `Constructor_WithCpfWithDots_ShouldNormalize`
   - `Constructor_WithCpfWithHyphen_ShouldNormalize`
   - `Constructor_WithCpfWithSpaces_ShouldNormalize`
   - `Constructor_WithCpfWithMixedFormatting_ShouldNormalize`
   - `ToString_ShouldReturnFormattedCpf`
   - `ToString_ShouldReturnCorrectFormat`

   **b) Cenários de Validação de Dígitos:**
   - `Constructor_WithAllSameDigits_ShouldThrowDomainException`
   - `Constructor_WithInvalidFirstDigit_ShouldThrowDomainException`
   - `Constructor_WithInvalidSecondDigit_ShouldThrowDomainException`
   - `Constructor_WithValidCpf_ShouldNotThrow`

   **c) Cenários de Borda:**
   - `Constructor_WithExactly11Digits_ShouldAccept`
   - `Constructor_WithLessThan11Digits_ShouldThrowDomainException`
   - `Constructor_WithMoreThan11Digits_ShouldThrowDomainException`
   - `Constructor_WithNonNumericCharacters_ShouldNormalizeAndValidate`
   - `CalculateDigit_WithKnownInput_ShouldReturnCorrectDigit`

   **d) CPFs Conhecidos:**
   - `Constructor_WithKnownValidCpf_ShouldAccept`
   - `Constructor_WithKnownInvalidCpf_ShouldThrow`

3. **Adicionar testes para Email:**

   **a) Cenários de Formatação:**
   - `Constructor_WithValidEmail_ShouldAccept`
   - `Constructor_WithEmailInUpperCase_ShouldAccept`
   - `Constructor_WithEmailInLowerCase_ShouldAccept`
   - `Constructor_WithEmailWithNumbers_ShouldAccept`
   - `Constructor_WithEmailWithHyphens_ShouldAccept`
   - `Constructor_WithEmailWithUnderscores_ShouldAccept`
   - `Constructor_WithEmailWithDots_ShouldAccept`
   - `ToString_ShouldReturnEmailValue`

   **b) Cenários de Validação:**
   - `Constructor_WithEmailWithoutAt_ShouldThrowDomainException`
   - `Constructor_WithEmailWithoutDomain_ShouldThrowDomainException`
   - `Constructor_WithEmailWithoutTld_ShouldThrowDomainException`
   - `Constructor_WithEmailWithMultipleAts_ShouldThrowDomainException`
   - `Constructor_WithEmailWithSpaces_ShouldThrowDomainException`
   - `Constructor_WithEmailStartingWithAt_ShouldThrowDomainException`
   - `Constructor_WithEmailEndingWithAt_ShouldThrowDomainException`

   **c) Cenários de Borda:**
   - `Constructor_WithMinimalValidEmail_ShouldAccept`
   - `Constructor_WithLongValidEmail_ShouldAccept`
   - `Constructor_WithEmailWithSubdomain_ShouldAccept`
   - `Constructor_WithEmailWithMultipleDots_ShouldAccept`
   - `Constructor_WithEmailWithPlusSign_ShouldAccept`

## Estrutura dos Testes Adicionais

### Cpf

```csharp
[Fact]
public void Constructor_WithFormattedCpf_ShouldNormalize()
{
    // Arrange
    var formattedCpf = "123.456.789-01";
    var expectedValue = "12345678901";
    
    // Act
    var cpf = new Cpf(formattedCpf);
    
    // Assert
    Assert.Equal(expectedValue, cpf.Value);
}

[Fact]
public void Constructor_WithCpfWithSpaces_ShouldNormalize()
{
    // Arrange
    var cpfWithSpaces = "123 456 789 01";
    var expectedValue = "12345678901";
    
    // Act
    var cpf = new Cpf(cpfWithSpaces);
    
    // Assert
    Assert.Equal(expectedValue, cpf.Value);
}

[Fact]
public void Constructor_WithAllSameDigits_ShouldThrowDomainException()
{
    // Arrange
    var invalidCpf = "11111111111";
    
    // Act & Assert
    var exception = Assert.Throws<DomainException>(() => new Cpf(invalidCpf));
    Assert.Equal("Invalid CPF.", exception.Message);
}

[Fact]
public void Constructor_WithLessThan11Digits_ShouldThrowDomainException()
{
    // Arrange
    var shortCpf = "1234567890"; // 10 dígitos
    
    // Act & Assert
    var exception = Assert.Throws<DomainException>(() => new Cpf(shortCpf));
    Assert.Equal("CPF must have 11 digits.", exception.Message);
}

[Fact]
public void ToString_ShouldReturnFormattedCpf()
{
    // Arrange
    var cpfValue = "12345678901";
    var cpf = new Cpf(cpfValue);
    var expectedFormat = "123.456.789-01";
    
    // Act
    var formatted = cpf.ToString();
    
    // Assert
    Assert.Equal(expectedFormat, formatted);
}

[Fact]
public void Constructor_WithKnownValidCpf_ShouldAccept()
{
    // Arrange - CPF válido conhecido
    var validCpf = "11144477735";
    
    // Act
    var cpf = new Cpf(validCpf);
    
    // Assert
    Assert.NotNull(cpf);
    Assert.Equal("11144477735", cpf.Value);
}

[Fact]
public void Constructor_WithMixedFormatting_ShouldNormalize()
{
    // Arrange
    var mixedFormat = "123.456 789-01";
    var expectedValue = "12345678901";
    
    // Act
    var cpf = new Cpf(mixedFormat);
    
    // Assert
    Assert.Equal(expectedValue, cpf.Value);
}
```

### Email

```csharp
[Fact]
public void Constructor_WithValidEmail_ShouldAccept()
{
    // Arrange
    var validEmail = "user@example.com";
    
    // Act
    var email = new Email(validEmail);
    
    // Assert
    Assert.Equal(validEmail, email.Value);
}

[Fact]
public void Constructor_WithEmailWithoutAt_ShouldThrowDomainException()
{
    // Arrange
    var invalidEmail = "userexample.com";
    
    // Act & Assert
    var exception = Assert.Throws<DomainException>(() => new Email(invalidEmail));
    Assert.Equal("Invalid email address.", exception.Message);
}

[Fact]
public void Constructor_WithEmailWithoutDomain_ShouldThrowDomainException()
{
    // Arrange
    var invalidEmail = "user@";
    
    // Act & Assert
    var exception = Assert.Throws<DomainException>(() => new Email(invalidEmail));
    Assert.Equal("Invalid email address.", exception.Message);
}

[Fact]
public void Constructor_WithEmailWithoutTld_ShouldThrowDomainException()
{
    // Arrange
    var invalidEmail = "user@example";
    
    // Act & Assert
    var exception = Assert.Throws<DomainException>(() => new Email(invalidEmail));
    Assert.Equal("Invalid email address.", exception.Message);
}

[Fact]
public void Constructor_WithEmailWithSubdomain_ShouldAccept()
{
    // Arrange
    var validEmail = "user@mail.example.com";
    
    // Act
    var email = new Email(validEmail);
    
    // Assert
    Assert.Equal(validEmail, email.Value);
}

[Fact]
public void Constructor_WithEmailWithPlusSign_ShouldAccept()
{
    // Arrange
    var validEmail = "user+tag@example.com";
    
    // Act
    var email = new Email(validEmail);
    
    // Assert
    Assert.Equal(validEmail, email.Value);
}

[Fact]
public void Constructor_WithEmailInUpperCase_ShouldAccept()
{
    // Arrange
    var validEmail = "USER@EXAMPLE.COM";
    
    // Act
    var email = new Email(validEmail);
    
    // Assert
    Assert.Equal(validEmail, email.Value);
}

[Fact]
public void ToString_ShouldReturnEmailValue()
{
    // Arrange
    var emailValue = "user@example.com";
    var email = new Email(emailValue);
    
    // Act
    var result = email.ToString();
    
    // Assert
    Assert.Equal(emailValue, result);
}

[Fact]
public void Constructor_WithMinimalValidEmail_ShouldAccept()
{
    // Arrange
    var minimalEmail = "a@b.co";
    
    // Act
    var email = new Email(minimalEmail);
    
    // Assert
    Assert.Equal(minimalEmail, email.Value);
}

[Fact]
public void Constructor_WithEmailWithMultipleDots_ShouldAccept()
{
    // Arrange
    var validEmail = "user.name.last@example.co.uk";
    
    // Act
    var email = new Email(validEmail);
    
    // Assert
    Assert.Equal(validEmail, email.Value);
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Domain/ValueObjects/CpfTests.cs` (atualizar)
- `src/tests/FastFood.Auth.Tests.Unit/Domain/ValueObjects/EmailTests.cs` (atualizar)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~ValueObjects"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura dos Value Objects >= 85%

## Critérios de Aceite

- [ ] Pelo menos 20 novos casos de teste adicionados para Cpf
- [ ] Pelo menos 15 novos casos de teste adicionados para Email
- [ ] Todos os cenários de formatação testados
- [ ] Todos os cenários de validação testados
- [ ] Todos os cenários de borda testados
- [ ] CPFs conhecidos (válidos e inválidos) testados
- [ ] Cobertura dos Value Objects >= 85%
- [ ] Todos os testes passam

## Notas Técnicas

- Testar normalização de CPF (remoção de caracteres especiais)
- Validar algoritmo de cálculo de dígitos verificadores
- Testar formatos de email válidos e inválidos
- Validar regex de email com diferentes formatos
- Testar valores limite (mínimo, máximo)
- Testar com caracteres especiais permitidos em emails



