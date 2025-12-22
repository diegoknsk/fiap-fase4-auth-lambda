# Subtask 05: Criar UseCase CreateAnonymousCustomerUseCase

## Descrição
Criar UseCase CreateAnonymousCustomerUseCase que orquestra a criação de um customer anônimo, usando os ports ICustomerRepository e ITokenService.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Application/UseCases/Customer/` se não existir
- Criar arquivo `src/FastFood.Auth.Application/UseCases/Customer/CreateAnonymousCustomerUseCase.cs`
- Criar classe `CreateAnonymousCustomerUseCase` com construtor recebendo `ICustomerRepository` e `ITokenService`
- Criar método `ExecuteAsync()` que:
  - Cria nova instância de Customer usando construtor com:
    - Id: Guid.NewGuid()
    - Name: null
    - Email: null
    - Cpf: null
    - CustomerType: CustomerTypeEnum.Anonymous
  - Chama `_customerRepository.AddAsync(customer)`
  - Chama `_tokenService.GenerateToken(customer.Id, out var expiresAt)`
  - Retorna objeto de resposta com Token, CustomerId, ExpiresAt
- Criar classe de resposta `CreateAnonymousCustomerResponse` em `Application/Responses/Customer/` com propriedades: Token (string), CustomerId (Guid), ExpiresAt (DateTime)

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que o UseCase está criado e acessível
- Validar que os métodos estão implementados corretamente

## Critérios de aceite
- Arquivo CreateAnonymousCustomerUseCase.cs criado
- Classe recebe ICustomerRepository e ITokenService via construtor
- Método ExecuteAsync implementado
- Customer criado com CustomerType.Anonymous
- Token gerado e retornado na resposta
- Classe CreateAnonymousCustomerResponse criada
- Código compila sem erros

