# Instruções de Deploy - Story 15

## 📋 Visão Geral

Após a refatoração da Story 15, o Lambda único `auth-lambda` foi separado em:
- **`auth-customer-lambda`** - Endpoints de Customer
- **`auth-admin-lambda`** - Endpoints de Admin  
- **`auth-migrator-lambda`** - Migrações de banco (já existia)

## 🚀 Deploy via GitHub Actions (Recomendado)

### Pré-requisitos

Certifique-se de que os seguintes **Secrets** estão configurados no GitHub (Settings → Secrets and variables → Actions):

- ✅ `AWS_ACCESS_KEY_ID`
- ✅ `AWS_SECRET_ACCESS_KEY`
- ✅ `AWS_SESSION_TOKEN`
- ✅ `AWS_REGION`
- ✅ `LAB_ROLE` (ARN completo da role IAM, ex: `arn:aws:iam::123456789012:role/LabRole`)
- ✅ `PROJECT_NAME` (opcional, padrão: `autenticacao`)
- ✅ `ENV` (opcional, padrão: `dev`)
- ✅ `COGNITO_REGION`
- ✅ `COGNITO_USER_POOL_ID`
- ✅ `COGNITO_CLIENT_ID`
- ✅ `RDS_CONNECTION_STRING`
- ✅ `JWT_SECRET`
- ✅ `JWT_ISSUER`
- ✅ `JWT_AUDIENCE`

### Passo 1: Executar a Action

1. Acesse o repositório no GitHub
2. Vá em **Actions** → **Deploy Lambda Functions to AWS**
3. Clique em **Run workflow** (botão no canto superior direito)
4. Selecione a branch `main` (ou a branch desejada)
5. Clique em **Run workflow**

### O que a Action faz automaticamente:

1. ✅ **Cria/Atualiza ECR Repository** (se necessário)
2. ✅ **Build e Push das Imagens Docker** para ECR:
   - `auth-customer-lambda`
   - `auth-admin-lambda`
   - `auth-migrator-lambda`
3. ✅ **Terraform Apply** - Cria/Atualiza a infraestrutura:
   - 3 funções Lambda
   - Security Group `lambda_auth_sg`
   - Function URL para `auth-customer-lambda`
4. ✅ **Deploy do Código** via ZIP (atualiza código das funções)

## ⚠️ Cenário Especial: Lambda Antigo Existe

Se você já tinha o Lambda `auth-lambda` em produção, há duas opções:

### Opção A: Importar Lambda Antigo (Recomendado se quiser manter dados)

Se o Lambda antigo `auth-lambda` ainda existe e você quer migrar para `auth-customer-lambda`:

1. **Antes de executar a Action**, importe o Lambda antigo:

```bash
cd terraform
terraform init

# Importar o Lambda antigo como auth-customer-lambda
terraform import module.auth_customer_lambda.aws_lambda_function.function autenticacao-auth-lambda
```

2. **Atualize o nome no Terraform** (se necessário) ou deixe o Terraform renomear
3. Execute a Action normalmente

### Opção B: Criar Novos Lambdas (Recomendado para ambiente limpo)

Se você quer criar tudo do zero:

1. **Remova o Lambda antigo** (se não precisar mais):
```bash
aws lambda delete-function --function-name autenticacao-auth-lambda --region us-east-1
```

2. Execute a Action normalmente - ela criará os 3 novos Lambdas

## 🔍 Verificar Deploy

Após a Action completar, verifique:

### 1. Verificar Lambdas Criados

```bash
aws lambda list-functions --query 'Functions[?contains(FunctionName, `auth`)].FunctionName' --output table
```

Deve mostrar:
- `autenticacao-auth-customer-lambda`
- `autenticacao-auth-admin-lambda`
- `autenticacao-auth-migrator-lambda`

### 2. Verificar Function URL

```bash
aws lambda get-function-url-config --function-name autenticacao-auth-customer-lambda --region us-east-1
```

### 3. Testar Endpoint

```bash
# Obter a URL
FUNCTION_URL=$(aws lambda get-function-url-config \
  --function-name autenticacao-auth-customer-lambda \
  --region us-east-1 \
  --query 'FunctionUrl' \
  --output text)

# Testar
curl $FUNCTION_URL/api/customer/health
```

## 📝 Deploy Manual (Alternativa)

Se preferir fazer deploy manualmente:

### 1. Build e Push das Imagens Docker

```bash
# Configurar AWS credentials
aws configure

# Login no ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 058264347413.dkr.ecr.us-east-1.amazonaws.com

# Build e push auth-customer-lambda
docker build -f Dockerfile.auth-customer-lambda -t fiap-fase4-auth-lambda:auth-customer-lambda .
docker tag fiap-fase4-auth-lambda:auth-customer-lambda 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-customer-lambda-latest
docker push 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-customer-lambda-latest

# Build e push auth-admin-lambda
docker build -f Dockerfile.auth-admin-lambda -t fiap-fase4-auth-lambda:auth-admin-lambda .
docker tag fiap-fase4-auth-lambda:auth-admin-lambda 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-admin-lambda-latest
docker push 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-admin-lambda-latest

# Build e push auth-migrator-lambda
docker build -f Dockerfile.auth-migrator-lambda -t fiap-fase4-auth-lambda:auth-migrator-lambda .
docker tag fiap-fase4-auth-lambda:auth-migrator-lambda 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-migrator-lambda-latest
docker push 058264347413.dkr.ecr.us-east-1.amazonaws.com/fiap-fase4-auth-lambda:auth-migrator-lambda-latest
```

### 2. Terraform Apply

```bash
cd terraform
terraform init
terraform plan
terraform apply
```

## 🐛 Troubleshooting

### Erro: "Function already exists"

Se você ver este erro, significa que o Lambda já existe na AWS mas não está no Terraform state:

```bash
# Importar o Lambda existente
cd terraform
terraform import module.auth_customer_lambda.aws_lambda_function.function autenticacao-auth-customer-lambda
```

### Erro: "Module not installed"

Execute:
```bash
cd terraform
terraform init
```

### Erro: "Secret not found"

Verifique se todos os Secrets estão configurados no GitHub (Settings → Secrets and variables → Actions).

### Verificar Logs da Action

1. Vá em **Actions** → selecione a execução
2. Clique em cada job para ver os logs detalhados
3. Procure por erros em vermelho

## 📚 Documentação Relacionada

- `docs/DEPLOY_LAMBDA.md` - Documentação detalhada do processo de deploy
- `terraform/README.md` - Documentação do Terraform
- `terraform/IMPORT_LAMBDAS.md` - Como importar Lambdas existentes

## ✅ Checklist de Deploy

- [ ] Secrets configurados no GitHub
- [ ] Código commitado e pushado para `main`
- [ ] Action executada com sucesso
- [ ] Lambdas criados na AWS
- [ ] Function URL funcionando
- [ ] Testes de endpoints passando

