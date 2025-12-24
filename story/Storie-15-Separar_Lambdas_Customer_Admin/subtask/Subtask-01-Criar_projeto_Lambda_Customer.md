# Subtask 01: Criar projeto FastFood.Auth.Lambda.Customer

## Descrição
Criar o novo projeto `FastFood.Auth.Lambda.Customer` que será uma Lambda dedicada exclusivamente ao fluxo de Customer, sem lógica condicional ou filtros.

## Passos de implementação
1. Criar novo projeto ASP.NET Core Web API:
   ```bash
   dotnet new webapi -n FastFood.Auth.Lambda.Customer -o src/FastFood.Auth.Lambda.Customer
   ```

2. Configurar o arquivo `.csproj`:
   - Target Framework: `net8.0`
   - Adicionar referências aos projetos:
     - `FastFood.Auth.Domain`
     - `FastFood.Auth.Application`
     - `FastFood.Auth.Infra.Persistence`
     - `FastFood.Auth.Infra.Services`
   - Adicionar pacotes NuGet:
     - `Amazon.Lambda.AspNetCoreServer.Hosting` (versão 8.0.0 ou superior)
     - `Microsoft.EntityFrameworkCore` (versão 8.0.0)
     - `Npgsql.EntityFrameworkCore.PostgreSQL` (versão 8.0.0)
     - `Swashbuckle.AspNetCore` (versão 6.5.0 ou superior)

3. Criar estrutura de pastas:
   ```
   src/FastFood.Auth.Lambda.Customer/
   ├── Controllers/
   ├── Program.cs
   └── LambdaEntryPoint.cs
   ```

4. Criar `LambdaEntryPoint.cs`:
   - Herdar de `APIGatewayHttpApiV2ProxyFunction`
   - Implementar método `Init` que configura o builder com `UseStartup<Startup>`
   - Criar classe `Startup` com configuração simplificada (sem lógica de modo)

5. Criar `Program.cs` simplificado:
   - Configurar logging
   - Adicionar `AddAWSLambdaHosting(LambdaEventSource.HttpApi)`
   - Registrar controllers (sem filtros)
   - Configurar DbContext com PostgreSQL
   - Registrar serviços específicos de Customer:
     - `ICustomerRepository` → `CustomerRepository`
     - `ITokenService` → `TokenService`
     - UseCases: `CreateAnonymousCustomerUseCase`, `RegisterCustomerUseCase`, `IdentifyCustomerUseCase`
   - Configurar Swagger
   - Configurar tratamento de exceções
   - **NÃO incluir** lógica de `LAMBDA_MODE` ou convenções de filtro

6. Adicionar projeto à solução:
   ```bash
   dotnet sln add src/FastFood.Auth.Lambda.Customer/FastFood.Auth.Lambda.Customer.csproj
   ```

7. Testar compilação:
   ```bash
   dotnet build src/FastFood.Auth.Lambda.Customer
   ```

## Arquivos a criar
- `src/FastFood.Auth.Lambda.Customer/FastFood.Auth.Lambda.Customer.csproj`
- `src/FastFood.Auth.Lambda.Customer/Program.cs`
- `src/FastFood.Auth.Lambda.Customer/LambdaEntryPoint.cs`
- `src/FastFood.Auth.Lambda.Customer/Controllers/` (pasta vazia por enquanto)

## Como testar
- Executar `dotnet build` no projeto (deve compilar sem erros)
- Verificar que não há referências a `LAMBDA_MODE` ou convenções de filtro
- Verificar que todas as dependências estão corretas

## Critérios de aceitação
- [ ] Projeto criado e compilando sem erros
- [ ] Todas as dependências configuradas corretamente
- [ ] `LambdaEntryPoint.cs` criado seguindo padrão do projeto antigo
- [ ] `Program.cs` criado sem lógica de modo ou filtros
- [ ] Projeto adicionado à solução
- [ ] Estrutura de pastas criada

