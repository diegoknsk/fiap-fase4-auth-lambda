# ============================================================================
# Locals para variáveis de ambiente
# ============================================================================

locals {
  # Variáveis de ambiente para auth-customer-lambda (RDS + Cognito + JWT)
  auth_customer_lambda_env = merge(
    var.rds_connection_string != "" ? { ConnectionStrings__DefaultConnection = var.rds_connection_string } : {},
    var.cognito_region != "" ? { COGNITO__REGION = var.cognito_region } : {},
    var.cognito_user_pool_id != "" ? { COGNITO__USERPOOLID = var.cognito_user_pool_id } : {},
    var.cognito_client_id != "" ? { COGNITO__CLIENTID = var.cognito_client_id } : {},
    var.jwt_secret != "" ? { JwtSettings__Secret = var.jwt_secret } : {},
    var.jwt_issuer != "" ? { JwtSettings__Issuer = var.jwt_issuer } : {},
    var.jwt_audience != "" ? { JwtSettings__Audience = var.jwt_audience } : {}
  )

  # Variáveis de ambiente para auth-admin-lambda (Cognito)
  # Admin não precisa de RDS connection string
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
# LAMBDA 1: auth-customer-lambda
# Lambda para autenticação de clientes (com VPC e Function URL)
# ============================================================================

module "auth_customer_lambda" {
  source = "./modules/lambda"

  function_name = "auth-customer-lambda"
  project_name  = var.project_name
  env           = var.env

  # Configurações para .NET 8 container image
  handler = var.lambda_auth_customer_image_uri != "" ? null : "bootstrap"
  runtime = var.lambda_auth_customer_image_uri != "" ? null : "provided.al2"

  # IAM role usando LabRole (ou variável equivalente)
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 900 # Máximo: 900 segundos (15 minutos)
  memory_size = 512

  # Tipo de pacote - Image (container) ou Zip (fallback)
  package_type = var.lambda_auth_customer_image_uri != "" ? "Image" : "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = var.lambda_auth_customer_image_uri != "" ? null : "placeholder"
  # URI da imagem ECR (quando usando container image)
  image_uri = var.lambda_auth_customer_image_uri != "" ? var.lambda_auth_customer_image_uri : null

  # Variáveis de ambiente (RDS + Cognito + JWT)
  environment_variables = local.auth_customer_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [local.sg_lambda_id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Deploy via Image (ECR) ou ZIP (fallback)
  depends_on_resources = var.lambda_auth_customer_image_uri != "" ? [aws_ecr_repository.lambda_images] : []

  common_tags = {
    Service = "auth-customer-lambda"
    Type    = "Customer-Authentication"
  }
}

# Lambda Function URL para auth-customer-lambda
resource "aws_lambda_function_url" "lambda_customer_url" {
  function_name      = module.auth_customer_lambda.function_name
  authorization_type = "NONE"
  invoke_mode        = "BUFFERED"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
  }
}

# NOTA: Quando authorization_type = "NONE", a AWS cria automaticamente
# a política de recursos que permite invocação pública.
# Não é necessário criar aws_lambda_permission manualmente.

# ============================================================================
# LAMBDA 2: auth-admin-lambda
# Lambda para administração do Cognito (SEM VPC)
# ============================================================================

module "auth_admin_lambda" {
  source = "./modules/lambda"

  function_name = "auth-admin-lambda"
  project_name  = var.project_name
  env           = var.env

  # Configurações para .NET 8 container image
  handler = var.lambda_auth_admin_image_uri != "" ? null : "bootstrap"
  runtime = var.lambda_auth_admin_image_uri != "" ? null : "provided.al2"

  # IAM role usando LabRole
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 30
  memory_size = 512

  # Tipo de pacote - Image (container) ou Zip (fallback)
  package_type = var.lambda_auth_admin_image_uri != "" ? "Image" : "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = var.lambda_auth_admin_image_uri != "" ? null : "placeholder"
  # URI da imagem ECR (quando usando container image)
  image_uri = var.lambda_auth_admin_image_uri != "" ? var.lambda_auth_admin_image_uri : null

  # Variáveis de ambiente (Cognito + RDS)
  environment_variables = local.auth_admin_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Deploy via Image (ECR) ou ZIP (fallback)
  depends_on_resources = var.lambda_auth_admin_image_uri != "" ? [aws_ecr_repository.lambda_images] : []

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
  invoke_mode        = "BUFFERED"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
  }
}

# Política de recursos explícita para garantir invocação pública
# Mesmo com authorization_type = "NONE", às vezes a política automática não funciona
resource "aws_lambda_permission" "lambda_admin_url_permission" {
  statement_id       = "AllowPublicInvoke"
  action             = "lambda:InvokeFunctionUrl"
  function_name      = module.auth_admin_lambda.function_name
  principal          = "*"
  function_url_auth_type = "NONE"
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

  # Configurações para .NET 8 container image
  handler = var.lambda_auth_migrator_image_uri != "" ? null : "bootstrap"
  runtime = var.lambda_auth_migrator_image_uri != "" ? null : "provided.al2"

  # IAM role usando LabRole
  role_arn = var.lab_role

  # Configurações de performance
  timeout     = 900 # Máximo: 900 segundos (15 minutos)
  memory_size = 512

  # Tipo de pacote - Image (container) ou Zip (fallback)
  package_type = var.lambda_auth_migrator_image_uri != "" ? "Image" : "Zip"
  # O arquivo ZIP será atualizado via deploy (usar placeholder.zip inicial)
  source_code_hash = var.lambda_auth_migrator_image_uri != "" ? null : "placeholder"
  # URI da imagem ECR (quando usando container image)
  image_uri = var.lambda_auth_migrator_image_uri != "" ? var.lambda_auth_migrator_image_uri : null

  # Variáveis de ambiente (RDS)
  environment_variables = local.auth_migrator_lambda_env

  # Sem limite de execuções concorrentes
  reserved_concurrent_executions = null

  # Configuração de VPC - usando a mesma Security Group "lambda_auth_sg"
  vpc_config = {
    security_group_ids = [local.sg_lambda_id]
    subnet_ids         = data.aws_subnets.eks_supported.ids
  }

  # Deploy via Image (ECR) ou ZIP (fallback)
  depends_on_resources = var.lambda_auth_migrator_image_uri != "" ? [aws_ecr_repository.lambda_images] : []

  common_tags = {
    Service = "auth-migrator-lambda"
    Type    = "Migration"
  }
}

# Lambda Function URL para auth-migrator-lambda
resource "aws_lambda_function_url" "lambda_migrator_url" {
  function_name      = module.auth_migrator_lambda.function_name
  authorization_type = "NONE"
  invoke_mode        = "BUFFERED"

  cors {
    allow_credentials = false
    allow_origins     = ["*"]
    allow_methods     = ["*"]
    allow_headers     = ["*"]
    expose_headers    = ["*"]
    max_age           = 86400
  }
}

# NOTA: Quando authorization_type = "NONE", a AWS cria automaticamente
# a política de recursos que permite invocação pública.
# Não é necessário criar aws_lambda_permission manualmente.
