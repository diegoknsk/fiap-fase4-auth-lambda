# ============================================================================
# LAMBDA 1: auth-lambda
# Lambda principal de autenticação (com VPC e Function URL)
# ============================================================================

module "auth_lambda" {
  source = "./modules/lambda"

  function_name = "auth-lambda"
  project_name  = var.project_name
  env           = var.env

  # Configurações para .NET 8 custom runtime
  handler = "bootstrap"
  runtime = "provided.al2" # .NET 8 custom runtime

  # IAM role usando LabRole (ou variável equivalente)
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 900 # Máximo: 900 segundos (15 minutos)
  memory_size = 512

  # Tipo de pacote - ZIP (deploy direto via arquivo ZIP)
  package_type = "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = "placeholder" # Será atualizado no deploy

  # Variáveis de ambiente (placeholder - serão configuradas no deploy)
  environment_variables = {
    # DATABASE_CONNECTION_STRING será injetado via Secrets Manager
    # JWT_SECRET será injetado via Secrets Manager
  }

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [aws_security_group.sg_lambda.id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Sem dependências de ECR (deploy via ZIP)
  depends_on_resources = []

  common_tags = {
    Service = "auth-lambda"
    Type    = "Authentication"
  }
}

# Lambda Function URL (apenas para auth-lambda)
resource "aws_lambda_function_url" "lambda_url" {
  function_name      = module.auth_lambda.function_name
  authorization_type = "NONE" # NONE = acesso público, AWS_IAM = requer autenticação IAM

  cors {
    allow_credentials = false
    allow_origins     = ["*"] # Permite qualquer origem (ajuste conforme necessário)
    allow_methods     = ["*"] # Permite todos os métodos HTTP
    allow_headers     = ["*"] # Permite todos os headers
    expose_headers    = ["*"] # Expõe todos os headers na resposta
    max_age           = 86400 # Cache de preflight por 24 horas
  }
}

# ============================================================================
# LAMBDA 2: auth-admin-lambda
# Lambda para administração do Cognito (SEM VPC)
# ============================================================================

module "auth_admin_lambda" {
  source = "./modules/lambda"

  function_name = "auth-admin-lambda"
  project_name  = var.project_name
  env           = var.env

  # Configurações para .NET 8 custom runtime
  handler = "bootstrap"
  runtime = "provided.al2"

  # IAM role usando LabRole
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 30
  memory_size = 512

  # Tipo de pacote - ZIP (deploy direto via arquivo ZIP)
  package_type = "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = "placeholder" # Será atualizado no deploy

  # Variáveis de ambiente
  environment_variables = {
    # COGNITO_USER_POOL_ID será injetado via Secrets Manager ou variável de ambiente
  }

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Sem dependências de ECR (deploy via ZIP)
  depends_on_resources = []

  common_tags = {
    Service = "auth-admin-lambda"
    Type    = "Cognito-Admin"
  }

  # IMPORTANTE: Esta Lambda NÃO tem configuração de VPC
  # Por padrão, o módulo Lambda não configura VPC, então a função fica fora da VPC
  # Isso é necessário para integração com Cognito (triggers, etc.)
  # vpc_config = null (ou não definir)
}

# ============================================================================
# LAMBDA 3: auth-migrator-lambda
# Lambda para migração de dados (com VPC)
# ============================================================================

module "auth_migrator_lambda" {
  source = "./modules/lambda"

  function_name = "auth-migrator-lambda"
  project_name  = var.project_name
  env           = var.env

  # Configurações para .NET 8 custom runtime
  handler = "bootstrap"
  runtime = "provided.al2"

  # IAM role usando LabRole
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 900 # Máximo: 900 segundos (15 minutos)
  memory_size = 512

  # Tipo de pacote - ZIP (deploy direto via arquivo ZIP)
  package_type = "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = "placeholder" # Será atualizado no deploy

  # Variáveis de ambiente
  environment_variables = {
    # DATABASE_CONNECTION_STRING será injetado via Secrets Manager
  }

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando a mesma Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [aws_security_group.sg_lambda.id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Sem dependências de ECR (deploy via ZIP)
  depends_on_resources = []

  common_tags = {
    Service = "auth-migrator-lambda"
    Type    = "Migration"
  }
}
