# Terraform - Deploy Lambda

Este diretório contém a configuração Terraform para deploy do Lambda via imagem ECR.

## Estrutura

- `providers.tf`: Configuração do provider AWS
- `backend.tf`: Configuração do backend S3 (state remoto)
- `variables.tf`: Variáveis de entrada
- `lambda.tf`: Recurso AWS Lambda Function
- `outputs.tf`: Outputs do Terraform

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
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

### 3. Aplicar Mudanças

```bash
terraform apply \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role"
```

## Variáveis Obrigatórias

- `aws_region`: Região AWS
- `lambda_function_name`: Nome da função Lambda
- `ecr_image_uri`: URI completa da imagem ECR com tag
- `lambda_role_arn`: ARN da role IAM para o Lambda

## Documentação Completa

Para documentação completa do processo de deploy, consulte: [../docs/DEPLOY_LAMBDA.md](../docs/DEPLOY_LAMBDA.md)

