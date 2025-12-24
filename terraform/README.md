# Terraform - Deploy Lambda Functions

Este diretório contém a configuração Terraform para criar e gerenciar 3 funções Lambda de autenticação usando deploy via ZIP.

## Estrutura

- `providers.tf`: Configuração do provider AWS
- `backend.tf`: Configuração do backend S3 (state remoto) - opcional
- `variables.tf`: Variáveis de entrada
- `data.tf`: Data sources (VPC, subnets)
- `sg-lambda.tf`: Security Group para Lambdas em VPC
- `lambda.tf`: Definição das 3 funções Lambda
- `outputs.tf`: Outputs do Terraform
- `modules/lambda/`: Módulo reutilizável para criação de funções Lambda

## Funções Lambda Criadas

1. **auth-lambda**: Lambda principal de autenticação
   - Com VPC e Security Group
   - Com Function URL (acesso público)
   - Timeout: 900s (15 minutos)
   - Runtime: .NET 8 custom runtime (provided.al2)

2. **auth-admin-lambda**: Lambda para administração do Cognito
   - Sem VPC (fora da VPC para melhor integração com Cognito)
   - Timeout: 30s
   - Runtime: .NET 8 custom runtime (provided.al2)

3. **auth-migrator-lambda**: Lambda para migração de dados
   - Com VPC e Security Group
   - Timeout: 900s (15 minutos)
   - Runtime: .NET 8 custom runtime (provided.al2)

## Separação de Responsabilidades

O processo de deploy é dividido em duas etapas distintas:

1. **Criação de Infraestrutura (Terraform)**:
   - Criação das 3 funções Lambda
   - Criação do Security Group `lambda_auth_sg`
   - Criação da Function URL para auth-lambda
   - Gerenciamento do estado da infraestrutura
   - Responsabilidade: Terraform (este diretório)

2. **Atualização de Código (GitHub Actions)**:
   - Build e empacotamento do código .NET em ZIP
   - Deploy do código via AWS CLI (`aws lambda update-function-code`)
   - Responsabilidade: Pipeline de CI/CD (`.github/workflows/deploy-lambda.yml`)

**Importante**: O Terraform cria as funções Lambda com um arquivo `placeholder.zip` inicial. O código real é atualizado via GitHub Actions usando deploy direto via ZIP.

## Fluxo Completo de Deploy

### 1. Criação Inicial (Terraform)

```bash
cd terraform
terraform init
terraform plan
terraform apply
```

Isso cria:
- 3 funções Lambda (com placeholder.zip)
- Security Group `lambda_auth_sg`
- Function URL para auth-lambda

### 2. Atualização de Código (GitHub Actions)

Quando há push para `main` ou `dev`, o workflow GitHub Actions:
1. Faz build do código .NET
2. Cria arquivos ZIP para cada Lambda
3. Atualiza o código das funções Lambda via AWS CLI

## Variáveis Terraform

### Variáveis Obrigatórias

- `aws_region` (string): Região AWS onde os recursos serão criados (ex: `us-east-1`)
- `lab_role` (string): ARN da role IAM LabRole para as funções Lambda
- `project_name` (string, opcional): Nome do projeto (padrão: `"autenticacao"`)
- `env` (string): Ambiente (ex: `dev`, `staging`, `prod`)
- `common_tags` (map, opcional): Tags comuns para aplicar a todos os recursos

### Exemplo de Uso

```bash
terraform apply \
  -var="aws_region=us-east-1" \
  -var="lab_role=arn:aws:iam::123456789012:role/LabRole" \
  -var="env=dev" \
  -var="project_name=autenticacao"
```

### Usando arquivo de variáveis (terraform.tfvars)

Criar arquivo `terraform/terraform.tfvars`:

```hcl
aws_region  = "us-east-1"
lab_role    = "arn:aws:iam::123456789012:role/LabRole"
env         = "dev"
project_name = "autenticacao"
```

Então executar:

```bash
terraform plan
terraform apply
```

**Nota**: Não commitar `terraform.tfvars` com valores reais no repositório. Use `.gitignore` ou `terraform.tfvars.example`.

## Security Group

O Security Group `lambda_auth_sg` é criado com:
- **Nome fixo**: `lambda_auth_sg` (independente do project_name)
- **Ingress**: HTTP (80) e HTTPS (443) de qualquer origem (para Function URL)
- **Egress**: Todo tráfego de saída permitido

Este Security Group é usado pelas Lambdas `auth-lambda` e `auth-migrator-lambda` (que estão em VPC).

## Function URL

A Function URL é criada apenas para `auth-lambda`:
- **Authorization**: NONE (acesso público)
- **CORS**: Configurado para permitir qualquer origem
- A URL é exposta como output do Terraform

## Outputs

O Terraform expõe os seguintes outputs:

- `lambda_auth_arn`: ARN da função Lambda auth-lambda
- `lambda_auth_name`: Nome da função Lambda auth-lambda
- `lambda_auth_function_url`: URL pública da Function URL
- `lambda_auth_admin_arn`: ARN da função Lambda auth-admin-lambda
- `lambda_auth_admin_name`: Nome da função Lambda auth-admin-lambda
- `lambda_auth_migrator_arn`: ARN da função Lambda auth-migrator-lambda
- `lambda_auth_migrator_name`: Nome da função Lambda auth-migrator-lambda
- `lambda_security_group_id`: ID do Security Group
- `lambda_security_group_name`: Nome do Security Group (fixo: `lambda_auth_sg`)

## Validação e Testes

### Validar configuração Terraform

```bash
cd terraform
terraform fmt -recursive
terraform validate
```

### Verificar outputs do Terraform

```bash
cd terraform
terraform output
```

## Troubleshooting

### Erro: "Backend configuration not found"

**Solução**: Configure o backend S3 antes de executar `terraform init`:

```bash
terraform init \
  -backend-config="bucket=seu-bucket" \
  -backend-config="key=terraform.tfstate" \
  -backend-config="region=us-east-1"
```

### Erro: "Variable not set"

**Solução**: Certifique-se de passar todas as variáveis obrigatórias ou criar um arquivo `terraform.tfvars`.

### Erro: "Lambda function not found" (no GitHub Actions)

**Solução**: Certifique-se de que:
1. As funções Lambda foram criadas via Terraform primeiro
2. O nome da função está correto (deve ser `{project_name}-{function_name}`)
3. As credenciais AWS têm permissão para atualizar o Lambda

### Terraform não detecta mudanças no código

**Solução**: Isso é esperado! O Terraform usa `lifecycle { ignore_changes = [filename, source_code_hash] }` para permitir atualizações de código via GitHub Actions sem conflitos. O código é atualizado via `aws lambda update-function-code` no workflow.

## Documentação Completa

Para documentação completa do processo de deploy, consulte: [../docs/DEPLOY_LAMBDA.md](../docs/DEPLOY_LAMBDA.md)
