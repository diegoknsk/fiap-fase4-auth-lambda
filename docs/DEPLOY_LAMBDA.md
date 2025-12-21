# Deploy Lambda via Terraform Mínimo

## Visão Geral

Este documento descreve o processo completo de deploy do Lambda via Terraform mínimo, onde o Terraform recebe a URI completa da imagem ECR (já com tag) como variável e atualiza o recurso `aws_lambda_function` apontando para a nova imagem.

### Separação de Responsabilidades

O processo de deploy é dividido em duas etapas distintas:

1. **Push de Imagem (CI/CD - GitHub Actions)**: 
   - Build da imagem Docker
   - Push da imagem para o repositório ECR
   - Responsabilidade: Pipeline de CI/CD

2. **Deploy de Infraestrutura (Terraform)**:
   - Atualização do Lambda com nova `image_uri`
   - Gerenciamento do estado da infraestrutura
   - Responsabilidade: Terraform

**Importante**: O Terraform **não faz push de imagens**. Ele apenas atualiza o Lambda para apontar para uma imagem que já existe no ECR.

## Fluxo de Deploy

O processo completo segue os seguintes passos:

1. **Build da Imagem Docker** (GitHub Actions)
   - O workflow faz checkout do código
   - Executa `docker build` para criar a imagem
   - Tag da imagem: `sha-<commit-sha>`

2. **Push para ECR** (GitHub Actions)
   - Login no ECR usando credenciais AWS
   - Push da imagem com tag SHA: `docker push <ecr-uri>:sha-<commit-sha>`
   - Push da imagem com tag `latest`: `docker push <ecr-uri>:latest`

3. **Deploy via Terraform** (GitHub Actions)
   - Executa `terraform init` no diretório `terraform/`
   - Executa `terraform plan` com variáveis:
     - `aws_region`: Região AWS
     - `lambda_function_name`: Nome da função Lambda
     - `ecr_image_uri`: URI completa da imagem ECR
   - Executa `terraform apply` para atualizar o Lambda

4. **Atualização do Lambda**
   - Terraform atualiza o recurso `aws_lambda_function` com nova `image_uri`
   - Lambda automaticamente faz pull da nova imagem do ECR
   - Lambda fica disponível com a nova versão

## Parâmetros Necessários

### Secrets do GitHub (Configurar no GitHub Settings → Secrets and variables → Actions)

Os seguintes secrets devem estar configurados no GitHub:

- `AWS_ACCESS_KEY_ID`: Credencial de acesso AWS
- `AWS_SECRET_ACCESS_KEY`: Credencial secreta AWS
- `AWS_REGION`: Região AWS onde o Lambda será deployado (ex: `us-east-1`)
- `AWS_ACCOUNT_ID`: ID da conta AWS (ex: `118233104061`)
- `ECR_REPOSITORY`: Nome do repositório ECR (ex: `auth-cpf-lambda`)
- `LAMBDA_FUNCTION_NAME`: Nome da função Lambda (ex: `auth-cpf-lambda`)
- `LAMBDA_ROLE_ARN`: ARN da role IAM para a função Lambda (ex: `arn:aws:iam::118233104061:role/lambda-execution-role`)

### Variáveis Terraform

As seguintes variáveis são passadas para o Terraform via linha de comando:

- `aws_region`: Região AWS (ex: `us-east-1`)
- `lambda_function_name`: Nome da função Lambda (ex: `auth-cpf-lambda`)
- `ecr_image_uri`: URI completa da imagem ECR com tag
- `lambda_role_arn`: ARN da role IAM para a função Lambda (ex: `arn:aws:iam::123456789012:role/lambda-execution-role`)

**Nota sobre `lambda_role_arn`**: Se o Lambda já existe e está sendo importado via `terraform import`, você pode obter o ARN da role do Lambda existente no console AWS ou via AWS CLI:
```bash
aws lambda get-function --function-name auth-cpf-lambda --query 'Configuration.Role' --output text
```

### Formato da URI ECR

A URI completa da imagem ECR segue o formato:

```
<account-id>.dkr.ecr.<region>.amazonaws.com/<repository>:<tag>
```

**Exemplo real:**
```
118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1234567890
```

Onde:
- `118233104061`: Account ID da AWS
- `us-east-1`: Região AWS
- `auth-cpf-lambda`: Nome do repositório ECR
- `sha-abcdef1234567890`: Tag da imagem (baseada no commit SHA)

## Comandos Terraform

### Inicialização

```bash
cd terraform
terraform init
```

**Nota**: Se usar backend S3, configure as variáveis de ambiente ou use `-backend-config`:

```bash
terraform init \
  -backend-config="bucket=seu-bucket-terraform-state" \
  -backend-config="key=lambda-auth/terraform.tfstate" \
  -backend-config="region=us-east-1"
```

### Planejamento

```bash
terraform plan \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

### Aplicação

```bash
terraform apply \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

Ou usando `-auto-approve` para aprovação automática:

```bash
terraform apply -auto-approve \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

### Usando arquivo de variáveis (terraform.tfvars)

Criar arquivo `terraform/terraform.tfvars`:

```hcl
aws_region          = "us-east-1"
lambda_function_name = "auth-cpf-lambda"
ecr_image_uri        = "118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef"
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

## Estrutura de Arquivos Terraform

```
terraform/
├── providers.tf      # Configuração do provider AWS e versão do Terraform
├── backend.tf        # Configuração do backend S3 (state remoto)
├── variables.tf       # Variáveis de entrada (aws_region, lambda_function_name, ecr_image_uri)
├── lambda.tf         # Recurso aws_lambda_function
└── outputs.tf        # Outputs do Terraform (ARN, nome, etc.)
```

## Workflow GitHub Actions

O workflow `.github/workflows/deploy-lambda.yml` é executado:

- **Automaticamente**: Após push na branch `main`
- **Manualmente**: Via GitHub Actions UI (workflow_dispatch)

### Jobs do Workflow

1. **build-and-push**:
   - Build da imagem Docker
   - Push para ECR com tag SHA e `latest`

2. **deploy-lambda**:
   - Executa Terraform para atualizar o Lambda
   - Depende do job `build-and-push` (só executa após push bem-sucedido)

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
  -var="ecr_image_uri=..."
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

## Referências

- [AWS Lambda Container Images](https://docs.aws.amazon.com/lambda/latest/dg/images-create.html)
- [Terraform AWS Provider - Lambda Function](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/lambda_function)
- [GitHub Actions - Deploy to AWS](https://docs.github.com/en/actions/deployment/deploying-to-your-cloud-provider/deploying-to-amazon-ecs)

