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

1. **Build da Imagem Docker** (GitHub Actions - job `build-and-push`)
   - O workflow faz checkout do código
   - Executa `docker build` para criar a imagem
   - Tag da imagem: `sha-<7-primeiros-caracteres-do-SHA>` (ex: `sha-abcdef1`)
   - Exporta `ECR_IMAGE_URI` como output do job

2. **Push para ECR** (GitHub Actions - job `build-and-push`)
   - Login no ECR usando credenciais AWS
   - Push da imagem com tag SHA: `docker push <ecr-uri>:sha-<7-primeiros-caracteres>`
   - Push da imagem com tag `latest`: `docker push <ecr-uri>:latest`
   - Exporta `ECR_IMAGE_URI` como output para uso no job `terraform-apply`

3. **Deploy via Terraform** (GitHub Actions - job `terraform-apply`)
   - Executa `terraform init` no diretório `terraform/`
   - Executa `terraform plan` com variáveis:
     - `aws_region`: Região AWS
     - `lambda_function_name`: Nome da função Lambda
     - `ecr_image_uri`: URI completa da imagem ECR (recebida do job `build-and-push`)
     - `lambda_role_arn`: ARN da role IAM para o Lambda
   - Executa `terraform apply` para atualizar o Lambda

4. **Atualização do Lambda**
   - Terraform atualiza o recurso `aws_lambda_function` com nova `image_uri`
   - Lambda automaticamente faz pull da nova imagem do ECR
   - Lambda fica disponível com a nova versão

## Parâmetros Necessários

### Secrets do GitHub (Configurar no GitHub Settings → Secrets and variables → Actions)

Os seguintes secrets devem estar configurados no GitHub:

#### Secrets Básicos (Obrigatórios)
- `AWS_ACCESS_KEY_ID`: Credencial de acesso AWS
- `AWS_SECRET_ACCESS_KEY`: Credencial secreta AWS
- `AWS_SESSION_TOKEN`: Token de sessão AWS (obrigatório para AWS Academy - credenciais temporárias)
- `AWS_REGION`: Região AWS onde o Lambda será deployado (ex: `us-east-1`)
- `AWS_ACCOUNT_ID`: ID da conta AWS (ex: `118233104061`)
- `ECR_REPOSITORY_NAME`: Nome do repositório ECR (ex: `auth-cpf-lambda`)
- `LAMBDA_FUNCTION_NAME`: Nome da função Lambda (ex: `auth-cpf-lambda`)
- `LAMBDA_ROLE_ARN`: ARN da role IAM para a função Lambda (ex: `arn:aws:iam::118233104061:role/lambda-execution-role`)

#### Secrets de VPC e Security Group (Opcionais)
- `LAMBDA_SECURITY_GROUP_NAME`: Nome do Security Group para o Lambda (opcional, default: `fiap-fase4-auth-sg`)
- `LAMBDA_SECURITY_GROUP_ID`: ID do Security Group para o Lambda (opcional, tem prioridade sobre nome)

**Nota:** Se nenhum dos dois for fornecido, o Terraform usará o nome padrão `fiap-fase4-auth-sg`. As subnets são descobertas automaticamente da VPC default.

#### Secrets do Cognito (Obrigatórios)
- `COGNITO_USER_POOL_ID`: ID do User Pool do Cognito (ex: `us-east-1_XXXXXXXXX`)
- `COGNITO_REGION`: Região do Cognito (ex: `us-east-1`)
- `COGNITO_CLIENT_ID`: Client ID do aplicativo Cognito

#### Secrets do RDS (Obrigatório)
- `RDS_CONNECTION_STRING`: Connection string completa do PostgreSQL no formato `Host=...;Port=...;Database=...;Username=...;Password=...` (sensitive)

#### Secrets JWT (Obrigatórios)
- `JWT_SECRET`: Chave secreta para assinar tokens JWT (mínimo 32 caracteres, sensitive)
- `JWT_ISSUER`: Emissor do token JWT (ex: `FastFood.Auth`)
- `JWT_AUDIENCE`: Audiência do token JWT (ex: `FastFood.API`)
- `JWT_EXPIRATION_HOURS`: Tempo de expiração em horas (opcional, default: `24`)

### Variáveis Terraform

As seguintes variáveis são passadas para o Terraform via linha de comando:

#### Variáveis Básicas (Obrigatórias)
- `aws_region`: Região AWS (ex: `us-east-1`)
- `lambda_function_name`: Nome da função Lambda (ex: `auth-cpf-lambda`)
- `ecr_image_uri`: URI completa da imagem ECR com tag
- `lambda_role_arn`: ARN da role IAM para a função Lambda (ex: `arn:aws:iam::123456789012:role/lambda-execution-role`)

**Nota sobre `lambda_role_arn`**: Se o Lambda já existe e está sendo importado via `terraform import`, você pode obter o ARN da role do Lambda existente no console AWS ou via AWS CLI:
```bash
aws lambda get-function --function-name auth-cpf-lambda --query 'Configuration.Role' --output text
```

#### Variáveis de VPC e Security Group (Opcionais)
- `lambda_security_group_name`: Nome do Security Group (opcional, default: `fiap-fase4-auth-sg`)
- `lambda_security_group_id`: ID do Security Group (opcional, tem prioridade sobre nome)

**Nota:** As subnets são descobertas automaticamente da VPC default, não requer parâmetro.

#### Variáveis do Cognito (Obrigatórias)
- `cognito_user_pool_id`: ID do User Pool do Cognito (ex: `us-east-1_XXXXXXXXX`)
- `cognito_region`: Região do Cognito (ex: `us-east-1`)
- `cognito_client_id`: Client ID do aplicativo Cognito

#### Variáveis do RDS (Obrigatória)
- `rds_connection_string`: Connection string completa do PostgreSQL (sensitive)

#### Variáveis JWT (Obrigatórias)
- `jwt_secret`: Chave secreta JWT (mínimo 32 caracteres, sensitive)
- `jwt_issuer`: Emissor do token JWT (ex: `FastFood.Auth`)
- `jwt_audience`: Audiência do token JWT (ex: `FastFood.API`)
- `jwt_expiration_hours`: Tempo de expiração em horas (opcional, default: `24`)

### Formato da URI ECR

A URI completa da imagem ECR segue o formato:

```
<account-id>.dkr.ecr.<region>.amazonaws.com/<repository>:<tag>
```

**Exemplo real:**
```
118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef1
```

Onde:
- `118233104061`: Account ID da AWS
- `us-east-1`: Região AWS
- `auth-cpf-lambda`: Nome do repositório ECR
- `sha-abcdef1`: Tag da imagem (baseada nos primeiros 7 caracteres do commit SHA)

## Configuração de VPC e Security Group

O Lambda precisa estar configurado em uma VPC para acessar o banco de dados RDS. A configuração é feita automaticamente pelo Terraform:

### Descoberta Automática

- **VPC**: O Terraform usa automaticamente a VPC default da sua conta AWS
- **Subnets**: As subnets são descobertas automaticamente da VPC default (não requer parâmetro)
- **Security Group**: Pode ser localizado por nome ou ID

### Parâmetros do Security Group

O Security Group pode ser especificado de duas formas (em ordem de prioridade):

1. **Por ID** (prioridade): Se `lambda_security_group_id` for fornecido, o Terraform busca pelo ID
2. **Por Nome**: Se apenas `lambda_security_group_name` for fornecido, busca pelo nome
3. **Padrão**: Se nenhum for fornecido, usa o nome padrão `fiap-fase4-auth-sg`

### Como Obter o Security Group ID ou Nome

**Via AWS Console:**
1. Acesse o AWS Console
2. Navegue até **VPC** > **Security Groups**
3. Procure pelo Security Group que permite acesso ao RDS (porta 5432)
4. Copie o **Security Group ID** (ex: `sg-0123456789abcdef0`) ou o **Name**

**Via AWS CLI:**
```bash
# Listar Security Groups
aws ec2 describe-security-groups --region us-east-1 --query 'SecurityGroups[*].[GroupId,GroupName]' --output table

# Obter Security Group por nome
aws ec2 describe-security-groups --region us-east-1 --filters "Name=group-name,Values=fiap-fase4-auth-sg" --query 'SecurityGroups[0].GroupId' --output text
```

### Requisitos do Security Group

O Security Group deve permitir:
- **Saída (Outbound)**: Tráfego para o RDS na porta 5432 (PostgreSQL)
- **Entrada (Inbound)**: Geralmente não é necessário para Lambda acessar RDS

**Nota:** O Lambda precisa estar na mesma VPC que o RDS para poder acessá-lo.

## Configuração do Cognito

O Lambda usa o AWS Cognito para autenticação de administradores. As seguintes informações são necessárias:

### Parâmetros Obrigatórios

- `cognito_user_pool_id`: ID do User Pool do Cognito
- `cognito_region`: Região onde o User Pool está configurado
- `cognito_client_id`: Client ID do aplicativo configurado no User Pool

### Como Obter os Valores do Cognito

**Via AWS Console:**

1. **Cognito User Pool ID:**
   - Acesse o AWS Console
   - Navegue até **Amazon Cognito** > **User pools**
   - Selecione seu User Pool
   - O ID aparece no topo da página (formato: `us-east-1_XXXXXXXXX`)

2. **Cognito Client ID:**
   - No mesmo User Pool, vá em **App integration** > **App clients**
   - Copie o **Client ID** (formato: string alfanumérica)

3. **Cognito Region:**
   - A região está visível no topo do console ou no próprio User Pool ID (ex: `us-east-1`)

**Via AWS CLI:**
```bash
# Listar User Pools
aws cognito-idp list-user-pools --max-results 10 --region us-east-1

# Obter informações do User Pool
aws cognito-idp describe-user-pool --user-pool-id us-east-1_XXXXXXXXX --region us-east-1

# Listar App Clients
aws cognito-idp list-user-pool-clients --user-pool-id us-east-1_XXXXXXXXX --region us-east-1
```

### Exemplos de Valores

- `cognito_user_pool_id`: `us-east-1_AbCdEfGhIj`
- `cognito_region`: `us-east-1`
- `cognito_client_id`: `1b6gctiq6b27pjh53b0qdnudjl`

## Configuração do RDS

O Lambda precisa de uma connection string completa para se conectar ao banco de dados PostgreSQL no RDS.

### Formato da Connection String

A connection string deve seguir o formato:

```
Host=<endpoint-rds>;Port=<porta>;Database=<nome-banco>;Username=<usuario>;Password=<senha>
```

### Como Obter/Construir a Connection String

**Componentes necessários:**

1. **Host (Endpoint RDS):**
   - Acesse o AWS Console
   - Navegue até **RDS** > **Databases**
   - Selecione seu banco de dados
   - Copie o **Endpoint** (ex: `fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com`)

2. **Porta:**
   - Geralmente `5432` para PostgreSQL

3. **Database:**
   - Nome do banco de dados criado (ex: `dbAuth`)

4. **Username:**
   - Usuário master do RDS (ex: `dbadmin`)

5. **Password:**
   - Senha configurada para o usuário master

**Exemplo completo:**
```
Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123
```

**Via AWS CLI:**
```bash
# Obter endpoint do RDS
aws rds describe-db-instances --region us-east-1 --query 'DBInstances[*].[DBInstanceIdentifier,Endpoint.Address,Endpoint.Port]' --output table
```

**⚠️ Importante:** A connection string é uma informação sensível e deve ser mantida em segredo. Use GitHub Secrets para armazená-la.

## Configuração JWT

O Lambda gera tokens JWT para autenticação de clientes. As seguintes configurações são necessárias:

### Parâmetros Obrigatórios

- `jwt_secret`: Chave secreta para assinar tokens JWT (mínimo 32 caracteres)
- `jwt_issuer`: Emissor do token JWT (ex: `FastFood.Auth`)
- `jwt_audience`: Audiência do token JWT (ex: `FastFood.API`)

### Parâmetros Opcionais

- `jwt_expiration_hours`: Tempo de expiração em horas (padrão: `24`)

### Requisitos do JWT Secret

- **Mínimo de 32 caracteres** (requisito do HMAC-SHA256)
- **Chave forte e aleatória** (use gerador de chaves seguras)
- **Nunca commitar** no código ou repositório

### Exemplos de Valores

- `jwt_secret`: `MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256`
- `jwt_issuer`: `FastFood.Auth`
- `jwt_audience`: `FastFood.API`
- `jwt_expiration_hours`: `24`

### Gerar JWT Secret Seguro

**Via PowerShell:**
```powershell
-join ((48..57) + (65..90) + (97..122) | Get-Random -Count 32 | ForEach-Object {[char]$_})
```

**Via Linux/Mac:**
```bash
openssl rand -base64 32
```

## Como Obter Valores dos Recursos Existentes

### Security Group

**Opção 1: Por Nome (Recomendado)**
- Use o nome do Security Group: `fiap-fase4-auth-sg`
- O Terraform buscará automaticamente pelo nome

**Opção 2: Por ID**
- Obtenha o ID via AWS Console ou CLI
- Use `lambda_security_group_id` no lugar do nome

**Nota:** As subnets são descobertas automaticamente da VPC default, não requer ação.

### Cognito User Pool

1. Acesse AWS Console > Cognito > User pools
2. Selecione seu User Pool
3. Copie o ID (formato: `us-east-1_XXXXXXXXX`)
4. Vá em **App integration** > **App clients** e copie o Client ID

### RDS Connection String

1. Acesse AWS Console > RDS > Databases
2. Selecione seu banco de dados
3. Copie o **Endpoint** e **Port**
4. Use as credenciais configuradas no RDS
5. Construa a connection string no formato: `Host=...;Port=...;Database=...;Username=...;Password=...`

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
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role" \
  -var="lambda_security_group_name=fiap-fase4-auth-sg" \
  -var="cognito_user_pool_id=us-east-1_AbCdEfGhIj" \
  -var="cognito_region=us-east-1" \
  -var="cognito_client_id=1b6gctiq6b27pjh53b0qdnudjl" \
  -var="rds_connection_string=Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123" \
  -var="jwt_secret=MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256" \
  -var="jwt_issuer=FastFood.Auth" \
  -var="jwt_audience=FastFood.API" \
  -var="jwt_expiration_hours=24"
```

### Aplicação

```bash
terraform apply \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role" \
  -var="lambda_security_group_name=fiap-fase4-auth-sg" \
  -var="cognito_user_pool_id=us-east-1_AbCdEfGhIj" \
  -var="cognito_region=us-east-1" \
  -var="cognito_client_id=1b6gctiq6b27pjh53b0qdnudjl" \
  -var="rds_connection_string=Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123" \
  -var="jwt_secret=MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256" \
  -var="jwt_issuer=FastFood.Auth" \
  -var="jwt_audience=FastFood.API" \
  -var="jwt_expiration_hours=24"
```

Ou usando `-auto-approve` para aprovação automática:

```bash
terraform apply -auto-approve \
  -var="aws_region=us-east-1" \
  -var="lambda_function_name=auth-cpf-lambda" \
  -var="ecr_image_uri=118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef" \
  -var="lambda_role_arn=arn:aws:iam::118233104061:role/lambda-execution-role" \
  -var="lambda_security_group_name=fiap-fase4-auth-sg" \
  -var="cognito_user_pool_id=us-east-1_AbCdEfGhIj" \
  -var="cognito_region=us-east-1" \
  -var="cognito_client_id=1b6gctiq6b27pjh53b0qdnudjl" \
  -var="rds_connection_string=Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123" \
  -var="jwt_secret=MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256" \
  -var="jwt_issuer=FastFood.Auth" \
  -var="jwt_audience=FastFood.API" \
  -var="jwt_expiration_hours=24"
```

### Usando arquivo de variáveis (terraform.tfvars)

Criar arquivo `terraform/terraform.tfvars`:

```hcl
aws_region          = "us-east-1"
lambda_function_name = "auth-cpf-lambda"
ecr_image_uri        = "118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef"
lambda_role_arn      = "arn:aws:iam::118233104061:role/lambda-execution-role"

# VPC e Security Group (opcional)
lambda_security_group_name = "fiap-fase4-auth-sg"
# lambda_security_group_id = "sg-xxxxxxxxxxxxxxxxx"  # Opcional, tem prioridade sobre nome

# Cognito
cognito_user_pool_id = "us-east-1_AbCdEfGhIj"
cognito_region        = "us-east-1"
cognito_client_id    = "1b6gctiq6b27pjh53b0qdnudjl"

# RDS
rds_connection_string = "Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123"

# JWT
jwt_secret          = "MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256"
jwt_issuer          = "FastFood.Auth"
jwt_audience        = "FastFood.API"
jwt_expiration_hours = 24
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
├── variables.tf      # Variáveis de entrada (VPC, Cognito, RDS, JWT, etc.)
├── data.tf           # Data sources para localizar recursos existentes (VPC, Security Group, Subnets)
├── lambda.tf         # Recurso aws_lambda_function com VPC e variáveis de ambiente
└── outputs.tf        # Outputs do Terraform (ARN, nome, etc.)
```

## Workflow GitHub Actions

O workflow `.github/workflows/deploy-lambda.yml` é executado:

- **Automaticamente**: Após push na branch `main`
- **Manualmente**: Via GitHub Actions UI (workflow_dispatch)

### Jobs do Workflow

1. **build-and-push**:
   - Build da imagem Docker
   - Push para ECR com tag SHA (primeiros 7 caracteres) e `latest`
   - Exporta `ECR_IMAGE_URI` como output para uso no job seguinte
   - Tag da imagem: `sha-<7-primeiros-caracteres-do-SHA>`
   - Exemplo: `sha-abcdef1`

2. **terraform-apply**:
   - Executa Terraform para atualizar o Lambda
   - Depende do job `build-and-push` (só executa após push bem-sucedido)
   - Recebe `ECR_IMAGE_URI` do job anterior via `needs.build-and-push.outputs.ecr_image_uri`

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

### Erro: "Security Group not found"

**Solução**: Verifique se:
1. O Security Group existe na região especificada
2. O nome ou ID está correto
3. As credenciais AWS têm permissão para acessar EC2/VPC

**Como verificar:**
```bash
aws ec2 describe-security-groups --region us-east-1 --filters "Name=group-name,Values=fiap-fase4-auth-sg"
```

### Erro: "VPC default not found"

**Solução**: Certifique-se de que existe uma VPC default na sua conta AWS. Se não existir, você pode criar uma ou usar uma VPC específica (requer modificação do Terraform).

### Erro: "No subnets found in VPC"

**Solução**: Verifique se a VPC default tem subnets configuradas. O Lambda precisa de pelo menos uma subnet para ser associado à VPC.

**Como verificar:**
```bash
aws ec2 describe-subnets --filters "Name=vpc-id,Values=<vpc-id>" --region us-east-1
```

### Erro: "Cognito User Pool not found"

**Solução**: Verifique se:
1. O User Pool ID está correto (formato: `us-east-1_XXXXXXXXX`)
2. O User Pool existe na região especificada
3. As credenciais AWS têm permissão para acessar Cognito

### Erro: "RDS connection failed"

**Solução**: Verifique se:
1. A connection string está no formato correto
2. O Lambda está na mesma VPC que o RDS
3. O Security Group permite tráfego na porta 5432
4. As credenciais do RDS estão corretas

### Erro: "JWT Secret must be at least 32 characters"

**Solução**: Use uma chave secreta com no mínimo 32 caracteres. Gere uma nova chave usando:
```bash
openssl rand -base64 32
```

### Erro: "Lambda cannot access RDS"

**Solução**: Verifique se:
1. O Lambda está configurado na VPC (verifique `vpc_config` no Terraform)
2. O Security Group do Lambda permite saída (outbound) para o RDS na porta 5432
3. O Security Group do RDS permite entrada (inbound) do Security Group do Lambda na porta 5432
4. O Lambda e o RDS estão na mesma VPC

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

