# Subtask 01: Criar Dockerfile do Lambda compatível com AWS Lambda container image

## Descrição
Criar Dockerfile otimizado para Lambda .NET 8 usando imagem base da AWS para Lambda (container image), garantindo que a aplicação ASP.NET Core funcione corretamente no ambiente Lambda. O Dockerfile deve usar multi-stage build para otimizar o tamanho da imagem final e ser compatível com `package_type = "Image"` do AWS Lambda.

## Passos de implementação
- Criar arquivo `Dockerfile` na raiz do projeto (mesmo nível do `.sln`)
- Usar imagem base oficial AWS Lambda para .NET 8: `public.ecr.aws/lambda/dotnet:8`
- Configurar multi-stage build para otimizar tamanho:
  - **Stage 1 (build)**: usar `mcr.microsoft.com/dotnet/sdk:8.0` para compilar
  - **Stage 2 (runtime)**: usar `public.ecr.aws/lambda/dotnet:8` para runtime
- Copiar arquivos do projeto:
  - Copiar arquivos `.csproj` e restaurar dependências com `dotnet restore`
  - Copiar código fonte do projeto Lambda e dependências
  - Publicar aplicação com `dotnet publish -c Release -o /app/publish`
- Configurar variáveis de ambiente:
  - `ASPNETCORE_ENVIRONMENT=Production`
  - `DOTNET_ENVIRONMENT=Production`
- Definir `CMD` apontando para o handler do Lambda:
  - Formato: `["FastFood.Auth.Lambda::FastFood.Auth.Lambda.LambdaEntryPoint::FunctionHandlerAsync"]`
  - Ou usar variável de ambiente `_HANDLER` se necessário
- Verificar se classe `LambdaEntryPoint` existe (herda de `Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction`)
  - Se não existir, criar classe básica ou ajustar CMD conforme necessário
- Adicionar comentários explicativos no Dockerfile sobre cada stage
- Criar arquivo `.dockerignore` na raiz para otimizar build:
  - Ignorar `bin/`, `obj/`, `.git/`, `node_modules/`, arquivos de teste, etc.

## Como testar
- Executar `docker build -t fastfood-auth-lambda:test .` localmente (deve buildar sem erros)
- Verificar que a imagem é criada com tamanho razoável (< 500MB idealmente)
- Validar que Dockerfile segue boas práticas (multi-stage, otimizado)
- Testar que a imagem pode ser executada localmente (se possível):
  - `docker run -p 8080:8080 fastfood-auth-lambda:test`
- Verificar logs do build para garantir que não há erros de compilação
- Validar que todas as dependências do projeto são copiadas corretamente

## Critérios de aceite
- Arquivo `Dockerfile` criado na raiz do projeto
- Dockerfile usa imagem base oficial AWS Lambda para .NET 8 (`public.ecr.aws/lambda/dotnet:8`)
- Dockerfile usa multi-stage build para otimização (build stage + runtime stage)
- Dockerfile copia e publica aplicação corretamente com `dotnet publish`
- Variáveis de ambiente configuradas (ASPNETCORE_ENVIRONMENT, DOTNET_ENVIRONMENT)
- CMD aponta para handler correto do Lambda
- Arquivo `.dockerignore` criado para otimizar build
- `docker build` executa sem erros localmente
- Imagem gerada tem tamanho razoável (< 500MB idealmente)
- Dockerfile está documentado com comentários explicativos
- Dockerfile segue boas práticas de segurança (não usa root, layers otimizados)


