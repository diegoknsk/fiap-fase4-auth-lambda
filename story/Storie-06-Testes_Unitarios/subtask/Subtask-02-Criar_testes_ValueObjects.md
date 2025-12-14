# Subtask 02: Criar testes unitários para Value Objects (Cpf, Email)

## Descrição
Criar testes unitários para os Value Objects Cpf e Email do Domain, validando regras de negócio, validações e formatação.

## Passos de implementação
- Criar diretório `tests/FastFood.Auth.Tests.Unit/Domain/ValueObjects/` se não existir
- Criar arquivo `CpfTests.cs` com testes:
  - `Constructor_WithValidCpf_ShouldCreateInstance` - CPF válido deve ser criado
  - `Constructor_WithInvalidCpf_ShouldThrowDomainException` - CPF inválido deve lançar exceção
  - `Constructor_WithNullCpf_ShouldThrowDomainException` - CPF null deve lançar exceção
  - `Constructor_WithEmptyCpf_ShouldThrowDomainException` - CPF vazio deve lançar exceção
  - `Constructor_WithCpfWithLessThan11Digits_ShouldThrowDomainException` - CPF com menos de 11 dígitos
  - `Constructor_WithCpfWithMoreThan11Digits_ShouldThrowDomainException` - CPF com mais de 11 dígitos
  - `Constructor_WithCpfAllSameDigits_ShouldThrowDomainException` - CPF com todos dígitos iguais (111.111.111-11)
  - `ToString_ShouldReturnFormattedCpf` - ToString deve retornar CPF formatado (000.000.000-00)
  - `Constructor_WithCpfWithPunctuation_ShouldRemovePunctuation` - CPF com pontuação deve remover
- Criar arquivo `EmailTests.cs` com testes:
  - `Constructor_WithValidEmail_ShouldCreateInstance` - Email válido deve ser criado
  - `Constructor_WithInvalidEmail_ShouldThrowDomainException` - Email inválido deve lançar exceção
  - `Constructor_WithNullEmail_ShouldThrowDomainException` - Email null deve lançar exceção
  - `Constructor_WithEmptyEmail_ShouldThrowDomainException` - Email vazio deve lançar exceção
  - `Constructor_WithEmailWithoutAt_ShouldThrowDomainException` - Email sem @
  - `Constructor_WithEmailWithoutDomain_ShouldThrowDomainException` - Email sem domínio
  - `ToString_ShouldReturnEmailValue` - ToString deve retornar valor do email

## Como testar
- Executar `dotnet test --filter "FullyQualifiedName~ValueObjects"` (deve passar todos os testes)
- Verificar que todos os cenários de validação estão cobertos
- Validar que exceções são lançadas corretamente

## Critérios de aceite
- Arquivo CpfTests.cs criado com pelo menos 8 testes
- Arquivo EmailTests.cs criado com pelo menos 6 testes
- Todos os testes passando
- Testes cobrem casos válidos e inválidos
- Exceções de domínio lançadas corretamente
- `dotnet test` passa com sucesso

