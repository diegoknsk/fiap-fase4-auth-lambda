# Subtask 07: Registrar serviços no Program.cs

## Descrição
Registrar todos os serviços (repositórios, services, use cases) no container de injeção de dependência do Program.cs.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/Program.cs`
- Adicionar referência de projeto ao Application e Infra.Persistence (se não existir)
- No `builder.Services`, adicionar:
  - `AddScoped<ICustomerRepository, CustomerRepository>`
  - `AddScoped<ITokenService, TokenService>`
  - `AddScoped<CreateAnonymousCustomerUseCase>`
- Adicionar usings necessários para as interfaces e implementações

## Como testar
- Executar `dotnet build` na solução (deve compilar sem erros)
- Executar a aplicação e verificar que não há erros de DI
- Validar que os serviços estão registrados corretamente

## Critérios de aceite
- Referências de projeto adicionadas ao Lambda
- ICustomerRepository registrado como CustomerRepository
- ITokenService registrado como TokenService
- CreateAnonymousCustomerUseCase registrado
- Todos os serviços registrados com escopo Scoped
- Código compila sem erros
- Aplicação inicia sem erros de DI

