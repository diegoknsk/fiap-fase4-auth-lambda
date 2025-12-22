# Subtask 08: Adicionar testes para DomainException

## Descrição
Criar suite completa de testes unitários para `DomainException`, cobrindo todos os construtores e cenários de uso.

## Arquivo a Testar
- `src/FastFood.Auth.Domain/Common/Exceptions/DomainException.cs`

## Passos de Implementação

1. **Criar arquivo de testes:**
   - `src/tests/FastFood.Auth.Tests.Unit/Domain/Exceptions/DomainExceptionTests.cs`

2. **Cenários de teste a implementar:**

   **a) Construtor Padrão:**
   - `Constructor_Default_ShouldCreateException`
   - `Constructor_Default_ShouldHaveNullMessage`
   - `Constructor_Default_ShouldHaveNullInnerException`

   **b) Construtor com Message:**
   - `Constructor_WithMessage_ShouldSetMessage`
   - `Constructor_WithMessage_ShouldHaveNullInnerException`
   - `Constructor_WithEmptyMessage_ShouldSetEmptyMessage`
   - `Constructor_WithNullMessage_ShouldSetNullMessage`
   - `Constructor_WithLongMessage_ShouldSetLongMessage`

   **c) Construtor com Message e InnerException:**
   - `Constructor_WithMessageAndInnerException_ShouldSetBoth`
   - `Constructor_WithMessageAndInnerException_ShouldPreserveInnerException`
   - `Constructor_WithNullMessageAndInnerException_ShouldSetNullMessage`
   - `Constructor_WithMessageAndNullInnerException_ShouldSetNullInnerException`

   **d) Herança e Tipo:**
   - `ShouldInheritFromException`
   - `ShouldBeSerializable`
   - `GetType_ShouldReturnDomainException`

   **e) Propriedades:**
   - `Message_ShouldReturnSetMessage`
   - `InnerException_ShouldReturnSetInnerException`
   - `StackTrace_ShouldNotBeNull`
   - `Source_ShouldNotBeNull`

   **f) Serialização (se implementada):**
   - `Serialization_ShouldPreserveMessage`
   - `Serialization_ShouldPreserveInnerException`

## Estrutura do Teste

```csharp
public class DomainExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateException()
    {
        // Act
        var exception = new DomainException();
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<DomainException>(exception);
    }

    [Fact]
    public void Constructor_Default_ShouldHaveNullMessage()
    {
        // Act
        var exception = new DomainException();
        
        // Assert
        Assert.Null(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var expectedMessage = "Test error message";
        
        // Act
        var exception = new DomainException(expectedMessage);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
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
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        var expectedMessage = "Outer exception message";
        var innerException = new InvalidOperationException("Inner exception message");
        
        // Act
        var exception = new DomainException(expectedMessage, innerException);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal("Inner exception message", exception.InnerException.Message);
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
    public void Constructor_WithNullMessage_ShouldSetNullMessage()
    {
        // Act
        var exception = new DomainException(null!);
        
        // Assert
        Assert.Null(exception.Message);
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
}
```

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Domain/Exceptions/DomainExceptionTests.cs` (novo)

## Como Testar

- Executar `dotnet test --filter "FullyQualifiedName~DomainExceptionTests"`
- Verificar cobertura com `dotnet test /p:CollectCoverage=true`
- Validar que cobertura de `DomainException` >= 90%

## Critérios de Aceite

- [ ] Arquivo de testes criado em `Domain/Exceptions/`
- [ ] Pelo menos 15 casos de teste implementados
- [ ] Todos os construtores testados
- [ ] Propriedades testadas (Message, InnerException)
- [ ] Herança de Exception validada
- [ ] Cenários de borda testados (null, empty, long messages)
- [ ] Cobertura de `DomainException` >= 90%
- [ ] Todos os testes passam

## Notas Técnicas

- Testar todos os construtores disponíveis
- Validar que a exceção herda corretamente de `Exception`
- Testar preservação de `InnerException`
- Testar com mensagens null, empty e muito longas
- Validar que `StackTrace` é gerado quando a exceção é lançada
- Se `DomainException` implementa `ISerializable`, testar serialização


