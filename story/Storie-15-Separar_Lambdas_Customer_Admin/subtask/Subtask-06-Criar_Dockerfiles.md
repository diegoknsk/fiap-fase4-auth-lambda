# Subtask 06: Criar Dockerfiles para novos projetos

## Descrição
Criar Dockerfiles específicos para os novos projetos `FastFood.Auth.Lambda.Customer` e `FastFood.Auth.Lambda.Admin`, seguindo o padrão do Dockerfile existente mas adaptado para cada projeto.

## Passos de implementação
1. Criar `Dockerfile.auth-customer-lambda`:
   - Baseado no `Dockerfile.auth-lambda` existente
   - Copiar projeto `FastFood.Auth.Lambda.Customer` em vez de `FastFood.Auth.Lambda`
   - Configurar handler para `FastFood.Auth.Lambda.Customer::FastFood.Auth.Lambda.Customer.LambdaEntryPoint::FunctionHandlerAsync`
   - Manter estrutura multi-stage build
   - Usar imagem base `public.ecr.aws/lambda/dotnet:8`

2. Criar `Dockerfile.auth-admin-lambda`:
   - Baseado no `Dockerfile.auth-admin-lambda` existente (se houver) ou `Dockerfile.auth-lambda`
   - Copiar projeto `FastFood.Auth.Lambda.Admin` em vez de `FastFood.Auth.Lambda`
   - Configurar handler para `FastFood.Auth.Lambda.Admin::FastFood.Auth.Lambda.Admin.LambdaEntryPoint::FunctionHandlerAsync`
   - **NOTA**: Admin não precisa de `Infra.Persistence` nem `EntityFrameworkCore`
   - Manter estrutura multi-stage build
   - Usar imagem base `public.ecr.aws/lambda/dotnet:8`

3. Verificar que `Dockerfile.auth-migrator-lambda` está correto (já existe e não precisa de alterações)

4. Testar build local dos Dockerfiles:
   ```bash
   docker build -f Dockerfile.auth-customer-lambda -t auth-customer-lambda:test .
   docker build -f Dockerfile.auth-admin-lambda -t auth-admin-lambda:test .
   ```

## Arquivos a criar
- `Dockerfile.auth-customer-lambda`
- `Dockerfile.auth-admin-lambda`

## Estrutura esperada dos Dockerfiles

### Dockerfile.auth-customer-lambda
```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto para restaurar dependências
COPY ["src/FastFood.Auth.Lambda.Customer/FastFood.Auth.Lambda.Customer.csproj", "src/FastFood.Auth.Lambda.Customer/"]
# ... outras dependências (Domain, Application, Infra.Persistence, Infra.Services)

# Restaurar e publicar
RUN dotnet restore "src/FastFood.Auth.Lambda.Customer/FastFood.Auth.Lambda.Customer.csproj"
COPY . .
WORKDIR "/src/src/FastFood.Auth.Lambda.Customer"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM public.ecr.aws/lambda/dotnet:8
COPY --from=build /app/publish ${LAMBDA_TASK_ROOT}
CMD ["FastFood.Auth.Lambda.Customer::FastFood.Auth.Lambda.Customer.LambdaEntryPoint::FunctionHandlerAsync"]
```

### Dockerfile.auth-admin-lambda
```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto para restaurar dependências
COPY ["src/FastFood.Auth.Lambda.Admin/FastFood.Auth.Lambda.Admin.csproj", "src/FastFood.Auth.Lambda.Admin/"]
# ... outras dependências (Domain, Application, Infra.Services)
# NÃO incluir Infra.Persistence

# Restaurar e publicar
RUN dotnet restore "src/FastFood.Auth.Lambda.Admin/FastFood.Auth.Lambda.Admin.csproj"
COPY . .
WORKDIR "/src/src/FastFood.Auth.Lambda.Admin"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM public.ecr.aws/lambda/dotnet:8
COPY --from=build /app/publish ${LAMBDA_TASK_ROOT}
CMD ["FastFood.Auth.Lambda.Admin::FastFood.Auth.Lambda.Admin.LambdaEntryPoint::FunctionHandlerAsync"]
```

## Como testar
- Executar build dos Dockerfiles localmente
- Verificar que as imagens são criadas sem erros
- Verificar que o handler está configurado corretamente
- Verificar que as dependências estão corretas (Customer com Persistence, Admin sem)

## Critérios de aceitação
- [ ] `Dockerfile.auth-customer-lambda` criado e funcionando
- [ ] `Dockerfile.auth-admin-lambda` criado e funcionando
- [ ] Builds locais dos Dockerfiles funcionando sem erros
- [ ] Handlers configurados corretamente
- [ ] Dependências corretas em cada Dockerfile (Customer com Persistence, Admin sem)

