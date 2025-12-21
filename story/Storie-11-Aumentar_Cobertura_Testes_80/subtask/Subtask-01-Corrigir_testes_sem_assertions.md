# Subtask 01: Corrigir testes sem assertions em DomainValidationTests

## Descrição
Corrigir os 8 casos de teste em `DomainValidationTests.cs` que não possuem assertions, conforme identificado pelo SonarQube (regra csharpsquid:S2699).

## Problema Identificado
O SonarQube identificou 8 testes sem assertions nas linhas:
- Linha 22: `ThrowIf_WhenConditionIsFalse_ShouldNotThrow`
- Linha 41: `ThrowIfNull_WhenValueIsNotNull_ShouldNotThrow`
- Linha 88: `ThrowIfNullOrWhiteSpace_WhenValueIsValid_ShouldNotThrow`
- Linha 99: `ThrowIfLengthLessThan_WhenValueIsNull_ShouldNotThrow`
- Linha 110: `ThrowIfLengthLessThan_WhenValueIsEmpty_ShouldNotThrow`
- Linha 134: `ThrowIfLengthLessThan_WhenValueLengthIsEqualToMinLength_ShouldNotThrow`
- Linha 146: `ThrowIfLengthLessThan_WhenValueLengthIsGreaterThanMinLength_ShouldNotThrow`
- Linha 184: `ThrowIfLessOrEqual_WhenValueIsGreaterThanThreshold_ShouldNotThrow`

## Passos de Implementação

1. **Analisar cada teste sem assertion:**
   - Verificar o comportamento esperado
   - Adicionar assertion apropriada (verificar que não lança exceção ou valida estado)

2. **Corrigir cada teste:**
   - Para testes que verificam "não deve lançar exceção":
     - Adicionar `Assert.True(true)` ou similar
     - Ou usar `Record.Exception()` e verificar que é null
   - Para testes que verificam comportamento:
     - Adicionar assertions que validem o comportamento esperado

3. **Exemplo de correção:**
   ```csharp
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
   ```

4. **Executar testes:**
   - Verificar que todos os testes passam
   - Validar que o SonarQube não reporta mais esses issues

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Unit/Domain/Exceptions/DomainValidationTests.cs`

## Como Testar

- Executar `dotnet test` no projeto de testes
- Verificar que todos os 8 testes corrigidos passam
- Executar análise do SonarQube e verificar que os issues S2699 foram resolvidos

## Critérios de Aceite

- [ ] Todos os 8 testes sem assertions foram corrigidos
- [ ] Cada teste tem pelo menos uma assertion válida
- [ ] Todos os testes passam
- [ ] SonarQube não reporta mais issues S2699 para esses testes
- [ ] Cobertura de DomainValidation mantida ou aumentada

## Notas

- Usar `Record.Exception()` do xUnit para capturar exceções quando necessário
- Manter a nomenclatura e estrutura dos testes existentes
- Não alterar a lógica dos testes, apenas adicionar assertions


