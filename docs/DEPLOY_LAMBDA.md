# Deploy Lambda Functions via ZIP

## Visão Geral

Este documento descreve o processo completo de deploy das 3 funções Lambda de autenticação usando deploy via ZIP. O Terraform cria a infraestrutura (Lambdas, Security Group, Function URL) e o GitHub Actions atualiza o código das funções via deploy direto de arquivos ZIP.

### Separação de Responsabilidades

O processo de deploy é dividido em duas etapas distintas:

1. **Criação de Infraestrutura (Terraform)**: 
   - Criação das 3 funções Lambda
   - Criação do Security Group `lambda_auth_sg`
   - Criação da Function URL para auth-lambda
   - Responsabilidade: Terraform

2. **Atualização de Código (GitHub Actions)**:
   - Build e empacotamento do código .NET em ZIP
   - Deploy do código via AWS CLI (`aws lambda update-function-code`)
   - Responsabilidade: Pipeline de CI/CD

**Importante**: O Terraform cria as funções Lambda com um arquivo `placeholder.zip` inicial. O código real é atualizado via GitHub Actions usando deploy direto via ZIP.

## Fluxo de Deploy

O processo completo segue os seguintes passos:

1. **Criação Inicial (Terraform)**
   - Executar `terraform apply` para criar:
     - 3 funções Lambda (auth-customer-lambda, auth-admin-lambda, auth-migrator-lambda)
     - Security Group `lambda_auth_sg`
     - Function URL para auth-customer-lambda
   - As funções são criadas com `placeholder.zip` (arquivo vazio)

2. **Build e Empacotamento (GitHub Actions - job `build-and-package`)**
   - O workflow faz checkout do código
   - Executa `dotnet publish` para cada Lambda
   - Cria scripts `bootstrap` para .NET 8 custom runtime
   - Empacota tudo em arquivos ZIP:
     - `lambda-auth.zip`
     - `lambda-auth-admin.zip`
     - `lambda-auth-migrator.zip`

3. **Deploy de Código (GitHub Actions - job `deploy-lambdas`)**
   - Faz download dos arquivos ZIP
   - Atualiza cada função Lambda usando `aws lambda update-function-code --zip-file`
   - Aguarda a atualização completar

4. **Atualização do Lambda**
   - AWS Lambda atualiza o código da função com o novo ZIP
   - A função fica disponível com a nova versão do código

## Funções Lambda

### 1. auth-customer-lambda
- **Nome**: `{project_name}-auth-customer-lambda` (padrão: `autenticacao-auth-customer-lambda`)
- **VPC**: Sim (com Security Group `lambda_auth_sg`)
- **Function URL**: Sim (acesso público)
- **Timeout**: 900s (15 minutos)
- **Runtime**: .NET 8 custom runtime (`provided.al2`)
- **Handler**: `bootstrap`

### 2. auth-admin-lambda
- **Nome**: `{project_name}-auth-admin-lambda` (padrão: `autenticacao-auth-admin-lambda`)
- **VPC**: Não (fora da VPC para melhor integração com Cognito)
- **Function URL**: Não
- **Timeout**: 30s
- **Runtime**: .NET 8 custom runtime (`provided.al2`)
- **Handler**: `bootstrap`

### 3. auth-migrator-lambda
- **Nome**: `{project_name}-auth-migrator-lambda` (padrão: `autenticacao-auth-migrator-lambda`)
- **VPC**: Sim (com Security Group `lambda_auth_sg`)
- **Function URL**: Não
- **Timeout**: 900s (15 minutos)
- **Runtime**: .NET 8 custom runtime (`provided.al2`)
- **Handler**: `bootstrap`

## Configuração do Terraform

### Variáveis Obrigatórias

- `aws_region` (string): Região AWS onde os recursos serão criados
- `lab_role` (string): ARN da role IAM LabRole para as funções Lambda
- `env` (string): Ambiente (ex: `dev`, `staging`, `prod`)

### Variáveis Opcionais

- `project_name` (string, padrão: `"autenticacao"`): Nome do projeto (usado como prefixo)
- `common_tags` (map, padrão: `{}`): Tags comuns para aplicar a todos os recursos

### Exemplo de Uso

```bash
cd terraform
terraform init
terraform plan \
  -var="aws_region=us-east-1" \
  -var="lab_role=arn:aws:iam::123456789012:role/LabRole" \
  -var="env=dev"
terraform apply
```

## Configuração do GitHub Actions

### Secrets Necessários

Os seguintes secrets devem estar configurados no GitHub:

- `AWS_ACCESS_KEY_ID`: Credencial de acesso AWS
- `AWS_SECRET_ACCESS_KEY`: Credencial secreta AWS
- `AWS_SESSION_TOKEN`: Token de sessão AWS (obrigatório para AWS Academy)
- `AWS_REGION`: Região AWS (ex: `us-east-1`)
- `PROJECT_NAME` (opcional): Nome do projeto (padrão: `autenticacao`)

### Workflow

O workflow `.github/workflows/deploy-lambda.yml` é executado:

- **Automaticamente**: Após push para `main` ou `dev`
- **Manualmente**: Via GitHub Actions UI (workflow_dispatch)

### Jobs do Workflow

1. **build-and-package**:
   - Build do código .NET para cada Lambda
   - Criação de scripts bootstrap
   - Empacotamento em arquivos ZIP
   - Upload dos ZIPs como artifacts

2. **deploy-lambdas**:
   - Download dos artifacts ZIP
   - Atualização do código de cada Lambda via AWS CLI
   - Validação da atualização

## Bootstrap Script

Para .NET 8 custom runtime (`provided.al2`), cada Lambda precisa de um script `bootstrap`:

```bash
#!/bin/sh
set -euo pipefail
exec /var/lang/bin/dotnet FastFood.Auth.Lambda.dll
```

O script é criado automaticamente pelo workflow GitHub Actions durante o build.

## Security Group

O Security Group `lambda_auth_sg` é criado com:

- **Nome fixo**: `lambda_auth_sg` (independente do project_name)
- **Ingress**:
  - HTTP (porta 80) de qualquer origem
  - HTTPS (porta 443) de qualquer origem
- **Egress**: Todo tráfego de saída permitido

Este Security Group é usado pelas Lambdas `auth-customer-lambda` e `auth-migrator-lambda` (que estão em VPC).

## Function URL

A Function URL é criada apenas para `auth-customer-lambda`:

- **Authorization**: NONE (acesso público)
- **CORS**: Configurado para permitir qualquer origem
- **Métodos**: Todos os métodos HTTP permitidos
- **Headers**: Todos os headers permitidos

A URL é exposta como output do Terraform: `lambda_auth_function_url`

## Troubleshooting

### Erro: "Lambda function not found"

**Solução**: Certifique-se de que as funções Lambda foram criadas via Terraform primeiro:

```bash
cd terraform
terraform apply
```

### Erro: "PackageType must be Zip"

**Solução**: O Lambda deve ser do tipo `Zip`. Verifique se foi criado corretamente via Terraform:

```bash
aws lambda get-function --function-name autenticacao-auth-customer-lambda --query 'Configuration.PackageType'
```

Deve retornar: `"Zip"`

### Erro: "ZIP file not found"

**Solução**: Verifique se o job `build-and-package` foi executado com sucesso e os artifacts foram criados.

### Terraform não detecta mudanças no código

**Solução**: Isso é esperado! O Terraform usa `lifecycle { ignore_changes = [filename, source_code_hash] }` para permitir atualizações de código via GitHub Actions sem conflitos. O código é atualizado via `aws lambda update-function-code` no workflow.

## Validação

### Validar Terraform

```bash
cd terraform
terraform fmt -recursive
terraform validate
```

### Verificar Outputs

```bash
cd terraform
terraform output
```

### Testar Deploy Manual

1. Build local:
```bash
dotnet publish src/InterfacesExternas/FastFood.Auth.Lambda.Customer/FastFood.Auth.Lambda.Customer.csproj -c Release -o publish/auth-customer-lambda
cd publish/auth-customer-lambda
zip -r ../../lambda-auth-customer.zip .
```

2. Deploy via AWS CLI:
```bash
aws lambda update-function-code \
  --function-name autenticacao-auth-customer-lambda \
  --zip-file fileb://lambda-auth-customer.zip \
  --region us-east-1
```

## Documentação Adicional

- [Terraform README](../terraform/README.md)
- [GitHub Actions Workflow](../.github/workflows/deploy-lambda.yml)
