# Terraform - Deploy Lambda

Este diretório contém a configuração Terraform para deploy do Lambda via imagem ECR.

## Estrutura

- `providers.tf`: Configuração do provider AWS
- `backend.tf`: Configuração do backend S3 (state remoto)
- `variables.tf`: Variáveis de entrada
- `lambda.tf`: Recurso AWS Lambda Function
- `outputs.tf`: Outputs do Terraform

## Separação de Responsabilidades

O processo de deploy é dividido em duas etapas distintas:

1. **Build e Push de Imagem (CI/CD - GitHub Actions)**: 
   - Build da imagem Docker
   - Push da imagem para o repositório ECR
   - Exportação de `ECR_IMAGE_URI` para uso no Terraform
   - Responsabilidade: Pipeline de CI/CD (`.github/workflows/deploy-lambda.yml`)

2. **Deploy de Infraestrutura (Terraform)**:
   - Atualização do Lambda com nova `image_uri`
   - Gerenciamento do estado da infraestrutura
   - Responsabilidade: Terraform (este diretório)

**Importante**: O Terraform **não faz build nem push de imagens**. Ele apenas atualiza o Lambda para apontar para uma imagem que já existe no ECR.

## Fluxo Completo de Deploy

O processo completo segue a ordem obrigatória:

1. **Build da Imagem Docker** (GitHub Actions - job `build-and-push`)
   - O workflow faz checkout do código
   - Executa `docker build` para criar a imagem
   - Tag da imagem: `sha-<commit-sha>` (primeiros 7 caracteres)
   - Exemplo: `sha-abcdef1`

2. **Push para ECR** (GitHub Actions - job `build-and-push`)
   - Login no ECR usando credenciais AWS
   - Push da imagem com tag SHA: `docker push <ecr-uri>:sha-<commit-sha>`
   - Push da imagem com tag `latest`: `docker push <ecr-uri>:latest`
   - Exportação de `ECR_IMAGE_URI` como output do job

3. **Deploy via Terraform** (GitHub Actions - job `terraform-apply`)
   - Executa `terraform init` no diretório `terraform/`
   - Executa `terraform plan` com variáveis (incluindo `ECR_IMAGE_URI` do job anterior)
   - Executa `terraform apply` para atualizar o Lambda

4. **Atualização do Lambda**
   - Terraform atualiza o recurso `aws_lambda_function` com nova `image_uri`
   - Lambda automaticamente faz pull da nova imagem do ECR
   - Lambda fica disponível com a nova versão

## Parâmetros e Variáveis

### Parâmetros do CI/CD (GitHub Secrets)

Os seguintes secrets devem estar configurados no GitHub (Settings → Secrets and variables → Actions):

- `AWS_ACCESS_KEY_ID`: Credencial de acesso AWS
- `AWS_SECRET_ACCESS_KEY`: Credencial secreta AWS
- `AWS_REGION`: Região AWS onde o Lambda será deployado (ex: `us-east-1`)
- `AWS_ACCOUNT_ID`: ID da conta AWS (ex: `118233104061`)
- `ECR_REPOSITORY_NAME`: Nome do repositório ECR (ex: `auth-cpf-lambda`)
- `LAMBDA_FUNCTION_NAME`: Nome da função Lambda (ex: `auth-cpf-lambda`)
- `LAMBDA_ROLE_ARN`: ARN da role IAM para a função Lambda (ex: `arn:aws:iam::118233104061:role/lambda-execution-role`)

### Variáveis Derivadas (calculadas no CI/CD)

Durante a execução do workflow, as seguintes variáveis são calculadas:

- `ECR_REPOSITORY_URL`: URL do repositório ECR sem tag
  - Formato: `<AWS_ACCOUNT_ID>.dkr.ecr.<AWS_REGION>.amazonaws.com/<ECR_REPOSITORY_NAME>`
  - Exemplo: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda`

- `IMAGE_TAG`: Tag da imagem baseada em SHA do commit
  - Formato: `sha-<7-primeiros-caracteres-do-SHA>`
  - Exemplo: `sha-abcdef1`

- `ECR_IMAGE_URI`: URI completa da imagem ECR com tag (exportada como output)
  - Formato: `<ECR_REPOSITORY_URL>:<IMAGE_TAG>`
  - Exemplo: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1`

### Variáveis Terraform (obrigatórias)

As seguintes variáveis são passadas para o Terraform via linha de comando:

- `aws_region` (string): Região AWS (ex: `us-east-1`)
- `lambda_function_name` (string): Nome da função Lambda (ex: `auth-cpf-lambda`)
- `ecr_image_uri` (string): URI completa da imagem ECR com tag
  - Exemplo: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1`
- `lambda_role_arn` (string): ARN da role IAM para a função Lambda
  - Exemplo: `arn:aws:iam::118233104061:role/lambda-execution-role`

**Nota sobre `lambda_role_arn`**: Se o Lambda já existe e está sendo importado via `terraform import`, você pode obter o ARN da role do Lambda existente no console AWS ou via AWS CLI:
```bash
aws lambda get-function --function-name auth-cpf-lambda --query 'Configuration.Role' --output text
```

## Uso Rápido

### 1. Inicializar Terraform

```bash
terraform init
```

Se usar backend S3, configure via `-backend-config`:

```bash
terraform init \
  -backend-config="bucket=seu-bucket-terraform-state" \
  -backend-config="key=lambda-auth/terraform.tfstate" \
  -backend-config="region=us-east-1"
```

### 2. Planejar Mudanças

```bash
terraform plan \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

### 3. Aplicar Mudanças

```bash
terraform apply \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

Ou usando `-auto-approve` para aprovação automática:

```bash
terraform apply -auto-approve \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

### Usando arquivo de variáveis (terraform.tfvars)

Criar arquivo `terraform/terraform.tfvars`:

```hcl
aws_region          = "us-east-1"
lambda_function_name = "auth-cpf-lambda"
ecr_image_uri        = "118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1"
lambda_role_arn      = "arn:aws:iam::118233104061:role/lambda-execution-role"
```

Então executar:

```bash
terraform plan
terraform apply
```

**Nota**: Não commitar `terraform.tfvars` com valores reais no repositório. Use `.gitignore` ou `terraform.tfvars.example`.

## Processo Idempotente e Reexecutável

O processo de deploy é **idempotente**, o que significa que:

- `terraform apply` pode ser executado múltiplas vezes sem causar problemas
- Se a `image_uri` não mudou, o Terraform não fará alterações
- Se a `image_uri` mudou, o Terraform atualizará apenas o Lambda com a nova imagem
- O processo pode ser reexecutado após mudanças sem problemas

### Exemplo de Reexecução

1. Fazer alterações no código
2. Commit e push para `main`
3. GitHub Actions executa automaticamente:
   - Build da nova imagem
   - Push para ECR com novo SHA
   - Terraform atualiza Lambda com nova imagem

## Workflow GitHub Actions

O workflow `.github/workflows/deploy-lambda.yml` é executado:

- **Automaticamente**: Após push na branch `main`
- **Manualmente**: Via GitHub Actions UI (workflow_dispatch)

### Jobs do Workflow

1. **build-and-push**:
   - Build da imagem Docker
   - Push para ECR com tag SHA e `latest`
   - Exporta `ECR_IMAGE_URI` como output

2. **terraform-apply**:
   - Executa Terraform para atualizar o Lambda
   - Depende do job `build-and-push` (só executa após push bem-sucedido)
   - Recebe `ECR_IMAGE_URI` do job anterior

## Validação e Testes

### Validar configuração Terraform

```bash
cd terraform
terraform fmt -recursive
terraform validate
```

### Testar deploy localmente

1. Build e push manual da imagem:
```bash
docker build -t auth-lambda:test .
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <account-id>.dkr.ecr.us-east-1.amazonaws.com
docker tag auth-lambda:test <account-id>.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:test
docker push <account-id>.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:test
```

2. Deploy via Terraform:
```bash
cd terraform
terraform plan -var="ecr_image_uri=<account-id>.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:test" ...
terraform apply ...
```

3. Verificar no console AWS que o Lambda foi atualizado

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

**Solução**: Certifique-se de passar todas as variáveis obrigatórias:

```bash
terraform plan \
  -var="aws_region=..." \
  -var="lambda_function_name=..." \
  -var="ecr_image_uri=..." \
  -var="lambda_role_arn=..."
```

### Erro: "Image not found in ECR"

**Solução**: Verifique se:
1. A imagem foi pushada com sucesso para o ECR
2. A URI está correta (account-id, região, repositório, tag)
3. As credenciais AWS têm permissão para acessar o ECR

### Erro: "Lambda function not found"

**Solução**: Certifique-se de que:
1. A função Lambda existe na AWS
2. O nome da função está correto (`lambda_function_name`)
3. As credenciais AWS têm permissão para atualizar o Lambda

### Terraform não detecta mudanças

**Solução**: Verifique se a `image_uri` realmente mudou. O Terraform só atualiza se a URI for diferente da atual.

## Documentação Completa

Para documentação completa do processo de deploy, consulte: [../docs/DEPLOY_LAMBDA.md](../docs/DEPLOY_LAMBDA.md)

