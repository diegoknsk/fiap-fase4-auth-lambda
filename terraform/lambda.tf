# ============================================================================
# Locals para variáveis de ambiente
# ============================================================================

locals {
  # Variáveis de ambiente para auth-lambda (RDS + Cognito + JWT)
  auth_lambda_env = merge(
    var.rds_connection_string != "" ? { ConnectionStrings__DefaultConnection = var.rds_connection_string } : {},
    var.cognito_region != "" ? { COGNITO__REGION = var.cognito_region } : {},
    var.cognito_user_pool_id != "" ? { COGNITO__USERPOOLID = var.cognito_user_pool_id } : {},
    var.cognito_client_id != "" ? { COGNITO__CLIENTID = var.cognito_client_id } : {},
    var.jwt_secret != "" ? { JwtSettings__Secret = var.jwt_secret } : {},
    var.jwt_issuer != "" ? { JwtSettings__Issuer = var.jwt_issuer } : {},
    var.jwt_audience != "" ? { JwtSettings__Audience = var.jwt_audience } : {}
  )

  # Variáveis de ambiente para auth-admin-lambda (Cognito)
  auth_admin_lambda_env = merge(
    var.cognito_region != "" ? { COGNITO__REGION = var.cognito_region } : {},
    var.cognito_user_pool_id != "" ? { COGNITO__USERPOOLID = var.cognito_user_pool_id } : {},
    var.cognito_client_id != "" ? { COGNITO__CLIENTID = var.cognito_client_id } : {}
  )

  # Variáveis de ambiente para auth-migrator-lambda (RDS)
  auth_migrator_lambda_env = merge(
    var.rds_connection_string != "" ? { ConnectionStrings__DefaultConnection = var.rds_connection_string } : {}
  )
}

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

  # Variáveis de ambiente (RDS + Cognito + JWT)
  environment_variables = local.auth_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [local.sg_lambda_id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Deploy via ZIP (código atualizado via GitHub Actions)
  depends_on_resources = []

  common_tags = {
    Service = "auth-lambda"
    Type    = "Authentication"
  }
}

# Lambda Function URL para auth-lambda
resource "aws_lambda_function_url" "lambda_url" {
  function_name      = module.auth_lambda.function_name
  authorization_type = "NONE"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
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

  # Variáveis de ambiente (Cognito)
  environment_variables = local.auth_admin_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Deploy via ZIP (código atualizado via GitHub Actions)
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

# Lambda Function URL para auth-admin-lambda
resource "aws_lambda_function_url" "lambda_admin_url" {
  function_name      = module.auth_admin_lambda.function_name
  authorization_type = "NONE"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
  }
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

  # Variáveis de ambiente (RDS)
  environment_variables = local.auth_migrator_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando a mesma Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [local.sg_lambda_id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Deploy via ZIP (código atualizado via GitHub Actions)
  depends_on_resources = []

  common_tags = {
    Service = "auth-migrator-lambda"
    Type    = "Migration"
  }
}

# Lambda Function URL para auth-migrator-lambda
resource "aws_lambda_function_url" "lambda_migrator_url" {
  function_name      = module.auth_migrator_lambda.function_name
  authorization_type = "NONE"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
  }
}
