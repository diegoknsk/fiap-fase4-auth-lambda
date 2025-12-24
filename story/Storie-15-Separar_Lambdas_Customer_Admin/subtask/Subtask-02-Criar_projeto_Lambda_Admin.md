# Subtask 02: Criar projeto FastFood.Auth.Lambda.Admin

## Descrição
Criar o novo projeto `FastFood.Auth.Lambda.Admin` que será uma Lambda dedicada exclusivamente ao fluxo de Admin, sem lógica condicional ou filtros.

## Passos de implementação
1. Criar novo projeto ASP.NET Core Web API:
   ```bash
   dotnet new webapi -n FastFood.Auth.Lambda.Admin -o src/FastFood.Auth.Lambda.Admin
   ```

2. Configurar o arquivo `.csproj`:
   - Target Framework: `net8.0`
   - Adicionar referências aos projetos:
     - `FastFood.Auth.Domain`
     - `FastFood.Auth.Application`
     - `FastFood.Auth.Infra.Services`
   - Adicionar pacotes NuGet:
     - `Amazon.Lambda.AspNetCoreServer.Hosting` (versão 8.0.0 ou superior)
     - `AWSSDK.CognitoIdentityProvider` (versão 3.7.400.0 ou superior)
     - `Swashbuckle.AspNetCore` (versão 6.5.0 ou superior)
   - **NOTA**: Admin não precisa de `Infra.Persistence` nem `EntityFrameworkCore` (não usa banco)

3. Criar estrutura de pastas:
   ```
   src/FastFood.Auth.Lambda.Admin/
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
   - **NÃO configurar DbContext** (Admin não usa banco)
   - Registrar serviços específicos de Admin:
     - `ICognitoService` → `CognitoService`
     - UseCase: `AuthenticateAdminUseCase`
   - Configurar Swagger
   - Configurar tratamento de exceções
   - **NÃO incluir** lógica de `LAMBDA_MODE` ou convenções de filtro

6. Adicionar projeto à solução:
   ```bash
   dotnet sln add src/FastFood.Auth.Lambda.Admin/FastFood.Auth.Lambda.Admin.csproj
   ```

7. Testar compilação:
   ```bash
   dotnet build src/FastFood.Auth.Lambda.Admin
   ```

## Arquivos a criar
- `src/FastFood.Auth.Lambda.Admin/FastFood.Auth.Lambda.Admin.csproj`
- `src/FastFood.Auth.Lambda.Admin/Program.cs`
- `src/FastFood.Auth.Lambda.Admin/LambdaEntryPoint.cs`
- `src/FastFood.Auth.Lambda.Admin/Controllers/` (pasta vazia por enquanto)

## Como testar
- Executar `dotnet build` no projeto (deve compilar sem erros)
- Verificar que não há referências a `LAMBDA_MODE` ou convenções de filtro
- Verificar que não há referências a `EntityFrameworkCore` ou `Infra.Persistence`
- Verificar que todas as dependências estão corretas

## Critérios de aceitação
- [ ] Projeto criado e compilando sem erros
- [ ] Todas as dependências configuradas corretamente (sem EF Core)
- [ ] `LambdaEntryPoint.cs` criado seguindo padrão do projeto antigo
- [ ] `Program.cs` criado sem lógica de modo ou filtros
- [ ] `Program.cs` não configura DbContext (Admin não usa banco)
- [ ] Projeto adicionado à solução
- [ ] Estrutura de pastas criada

