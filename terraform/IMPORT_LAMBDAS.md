# Importar Funções Lambda Existentes

Este documento explica como importar funções Lambda que já existem na AWS para o state do Terraform.

## Problema

Quando as funções Lambda já existem na AWS mas não estão no state do Terraform, você verá erros como:

```
Error: creating Lambda Function: ResourceConflictException: Function already exist: autenticacao-auth-lambda
```

Isso geralmente acontece quando:
- O backend S3 não está configurado e o state local foi perdido
- As funções foram criadas manualmente ou por outro processo
- O state do Terraform foi deletado ou não está sincronizado

## Solução: Importar as Funções Existentes

### Opção 1: Usar o Script Automático (Recomendado)

#### Windows (PowerShell)

```powershell
cd terraform
.\import-lambdas.ps1
```

#### Linux/Mac (Bash)

```bash
cd terraform
chmod +x import-lambdas.sh
./import-lambdas.sh
```

### Opção 2: Importar Manualmente

Execute os comandos de import um por um:

```bash
cd terraform

# Importar auth-lambda
terraform import module.auth_lambda.aws_lambda_function.function autenticacao-auth-lambda

# Importar auth-admin-lambda
terraform import module.auth_admin_lambda.aws_lambda_function.function autenticacao-auth-admin-lambda

# Importar auth-migrator-lambda
terraform import module.auth_migrator_lambda.aws_lambda_function.function autenticacao-auth-migrator-lambda
```

### Verificar após Importação

Após importar, execute:

```bash
terraform plan
```

O Terraform deve mostrar que não há mudanças (ou apenas mudanças menores que podem ser ignoradas).

## Configurar Backend S3 (Recomendado)

Para evitar esse problema no futuro, configure o backend S3 para armazenar o state remotamente:

### 1. Criar Bucket S3 (se não existir)

```bash
aws s3 mb s3://seu-bucket-terraform-state --region us-east-1
aws s3api put-bucket-versioning \
  --bucket seu-bucket-terraform-state \
  --versioning-configuration Status=Enabled
```

### 2. Descomentar e Configurar backend.tf

Edite `terraform/backend.tf` e descomente o bloco:

```hcl
terraform {
  backend "s3" {
    bucket = "seu-bucket-terraform-state"
    key    = "lambda-auth/terraform.tfstate"
    region = "us-east-1"
  }
}
```

### 3. Re-inicializar Terraform

```bash
cd terraform
terraform init -migrate-state
```

Isso migrará o state local para o S3.

## Importar Function URL (se necessário)

Se a Function URL também já existir, importe:

```bash
# Primeiro, obtenha o ID da Function URL
aws lambda list-function-url-configs --function-name autenticacao-auth-lambda

# Depois importe (substitua FUNCTION_URL_ID pelo ID obtido)
terraform import aws_lambda_function_url.lambda_url FUNCTION_URL_ID
```

## Troubleshooting

### Erro: "Resource already managed by Terraform"

Isso significa que o recurso já está no state. Execute `terraform plan` para verificar.

### Erro: "Resource not found"

Verifique se:
1. O nome da função está correto (deve ser `{project_name}-{function_name}`)
2. Você está na região AWS correta
3. As credenciais AWS estão configuradas corretamente

### Verificar Funções Lambda Existentes

```bash
aws lambda list-functions --query 'Functions[?contains(FunctionName, `autenticacao`)].FunctionName' --output table
```

