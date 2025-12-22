# Dockerfile para AWS Lambda Container Image - .NET 8
# Multi-stage build para otimizar tamanho da imagem final

# Stage 1: Build - Compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto (.csproj) para restaurar dependências
COPY ["src/FastFood.Auth.Lambda/FastFood.Auth.Lambda.csproj", "src/FastFood.Auth.Lambda/"]
COPY ["src/FastFood.Auth.Application/FastFood.Auth.Application.csproj", "src/FastFood.Auth.Application/"]
COPY ["src/FastFood.Auth.Domain/FastFood.Auth.Domain.csproj", "src/FastFood.Auth.Domain/"]
COPY ["src/FastFood.Auth.Infra/FastFood.Auth.Infra.csproj", "src/FastFood.Auth.Infra/"]
COPY ["src/FastFood.Auth.Infra.Persistence/FastFood.Auth.Infra.Persistence.csproj", "src/FastFood.Auth.Infra.Persistence/"]

# Restaurar dependências NuGet
RUN dotnet restore "src/FastFood.Auth.Lambda/FastFood.Auth.Lambda.csproj"

# Copiar código fonte explicitamente - apenas arquivos necessários para build
# Domain
COPY ["src/FastFood.Auth.Domain/", "src/FastFood.Auth.Domain/"]

# Application
COPY ["src/FastFood.Auth.Application/", "src/FastFood.Auth.Application/"]

# Infra
COPY ["src/FastFood.Auth.Infra/", "src/FastFood.Auth.Infra/"]

# Infra.Persistence
COPY ["src/FastFood.Auth.Infra.Persistence/", "src/FastFood.Auth.Infra.Persistence/"]

# Lambda - projeto principal
COPY ["src/FastFood.Auth.Lambda/", "src/FastFood.Auth.Lambda/"]

# Publicar aplicação em modo Release
WORKDIR "/src/src/FastFood.Auth.Lambda"
RUN dotnet publish "FastFood.Auth.Lambda.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Stage 2: Runtime - Imagem base AWS Lambda para .NET 8
FROM public.ecr.aws/lambda/dotnet:8

# Instalar shadow-utils e criar usuário não-root em um único RUN
# Necessário para criar usuário não-root (segurança)
# A imagem AWS Lambda é baseada em Amazon Linux que usa yum
RUN yum install -y shadow-utils && \
    yum clean all && \
    groupadd -r appgroup && \
    useradd -r -g appgroup appuser

# Copiar aplicação publicada do stage de build
COPY --from=build /app/publish ${LAMBDA_TASK_ROOT}

# Ajustar permissões do diretório da aplicação para o usuário não-root
# ${LAMBDA_TASK_ROOT} é o diretório padrão usado pela AWS Lambda
RUN chown -R appuser:appgroup ${LAMBDA_TASK_ROOT}

# Mudar para usuário não-root antes de executar a aplicação
# Isso garante que a aplicação não execute com privilégios de root
USER appuser

# Configurar variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_ENVIRONMENT=Production

# O handler do Lambda será configurado via variável de ambiente _HANDLER
# ou via CMD. Para ASP.NET Core com AddAWSLambdaHosting, o handler padrão
# é o assembly principal. A imagem base AWS Lambda já está configurada
# para executar o assembly .NET automaticamente.

# O CMD padrão da imagem base já está configurado para executar o assembly
# Se necessário, pode ser sobrescrito aqui, mas geralmente não é necessário
# para aplicações ASP.NET Core com AddAWSLambdaHosting

