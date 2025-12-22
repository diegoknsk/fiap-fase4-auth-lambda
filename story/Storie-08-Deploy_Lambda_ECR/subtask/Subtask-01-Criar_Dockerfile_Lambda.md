# Subtask 01: Criar Dockerfile para Lambda

## Descrição
Criar Dockerfile otimizado para Lambda .NET 8 usando imagem base da AWS para Lambda, garantindo que a aplicação ASP.NET Core funcione corretamente no ambiente Lambda.

## Passos de implementação
- Criar arquivo `Dockerfile` na raiz do projeto
- Usar imagem base: `public.ecr.aws/lambda/dotnet:8` (imagem oficial AWS Lambda para .NET 8)
- Configurar estágios de build (multi-stage) para otimizar tamanho:
  - Stage 1 (build): usar `mcr.microsoft.com/dotnet/sdk:8.0` para build
  - Stage 2 (runtime): usar `public.ecr.aws/lambda/dotnet:8` para runtime
- Copiar arquivos do projeto:
  - Copiar `.csproj` files e restaurar dependências
  - Copiar código fonte
  - Publicar aplicação com `dotnet publish`
- Configurar variável de ambiente `ASPNETCORE_ENVIRONMENT=Production`
- Configurar variável de ambiente `DOTNET_ENVIRONMENT=Production`
- Definir `CMD` apontando para o assembly publicado: `["FastFood.Auth.Lambda::FastFood.Auth.Lambda.LambdaEntryPoint::FunctionHandlerAsync"]`
- Criar classe `LambdaEntryPoint` se não existir (herda de `APIGatewayHttpApiV2ProxyFunction`)
- Adicionar comentários explicativos no Dockerfile

## Como testar
- Executar `docker build -t fastfood-auth-lambda:test .` localmente (deve buildar sem erros)
- Verificar que a imagem é criada com tamanho razoável (< 500MB idealmente)
- Validar que Dockerfile segue boas práticas (multi-stage, otimizado)

## Critérios de aceite
- Arquivo Dockerfile criado na raiz do projeto
- Dockerfile usa imagem base oficial AWS Lambda para .NET 8
- Dockerfile usa multi-stage build para otimização
- Dockerfile copia e publica aplicação corretamente
- Variáveis de ambiente configuradas
- CMD aponta para handler correto do Lambda
- `docker build` executa sem erros localmente
- Imagem gerada tem tamanho razoável

