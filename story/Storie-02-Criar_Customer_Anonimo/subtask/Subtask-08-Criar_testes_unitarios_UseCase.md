# Subtask 08: Criar testes unitários para UseCase

## Descrição
Criar testes unitários para o UseCase CreateAnonymousCustomerUseCase usando mocks dos ports ICustomerRepository e ITokenService.

## Passos de implementação
- Abrir projeto `tests/FastFood.Auth.Tests.Unit`
- Adicionar pacote Moq (se não existir): `dotnet add package Moq`
- Criar diretório `UseCases/Customer/` se não existir
- Criar arquivo `CreateAnonymousCustomerUseCaseTests.cs`
- Criar classe de teste `CreateAnonymousCustomerUseCaseTests`
- Criar método de teste `ExecuteAsync_Should_CreateAnonymousCustomer_And_ReturnToken`:
  - Criar mocks de ICustomerRepository e ITokenService
  - Configurar mock do repository para retornar customer adicionado
  - Configurar mock do token service para retornar token e expiresAt
  - Instanciar UseCase com mocks
  - Chamar ExecuteAsync
  - Verificar que AddAsync foi chamado com customer anônimo
  - Verificar que GenerateToken foi chamado com customerId correto
  - Verificar que resposta contém token e customerId

## Como testar
- Executar `dotnet test` no projeto de testes (deve passar)
- Verificar cobertura de código do UseCase
- Validar que todos os cenários estão cobertos

## Critérios de aceite
- Arquivo de teste criado em `Tests.Unit/UseCases/Customer/`
- Teste usa Moq para criar mocks
- Mock de ICustomerRepository configurado
- Mock de ITokenService configurado
- Teste verifica criação de customer anônimo
- Teste verifica geração de token
- Teste verifica resposta correta
- `dotnet test` passa com sucesso

