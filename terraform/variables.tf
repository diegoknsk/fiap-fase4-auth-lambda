# Variáveis para deploy do Lambda via Terraform mínimo
# Todas as variáveis são obrigatórias (sem valores default) para garantir que valores sejam passados explicitamente

variable "aws_region" {
  type        = string
  description = "Região AWS onde o Lambda será deployado (ex: us-east-1)"
  # Não definir valor default - variável obrigatória
}

variable "lambda_function_name" {
  type        = string
  description = "Nome da função Lambda que será atualizada (ex: auth-cpf-lambda)"
  # Não definir valor default - variável obrigatória
}

variable "ecr_image_uri" {
  type        = string
  description = "URI completa da imagem ECR com tag (ex: 118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef)"
  # Não definir valor default - variável obrigatória
}

variable "lambda_role_arn" {
  type        = string
  description = "ARN da role IAM para a função Lambda (ex: arn:aws:iam::123456789012:role/lambda-execution-role)"
  # Nota: Se o Lambda já existe e está sendo importado via terraform import,
  # você pode obter o ARN da role do Lambda existente no console AWS ou via AWS CLI
}

# Modo da Lambda
variable "lambda_mode" {
  type        = string
  description = "Modo da Lambda: 'Customer', 'Admin' ou 'Migrator'. Define qual controller será ativado (default: 'Customer')"
  default     = "Customer"
  validation {
    condition     = contains(["Customer", "Admin", "Migrator"], var.lambda_mode)
    error_message = "lambda_mode deve ser 'Customer', 'Admin' ou 'Migrator'."
  }
}

# Cognito
variable "cognito_user_pool_id" {
  type        = string
  description = "ID do User Pool do Cognito (ex: us-east-1_XXXXXXXXX)"
  sensitive   = false
}

variable "cognito_region" {
  type        = string
  description = "Região do Cognito (ex: us-east-1)"
}

variable "cognito_client_id" {
  type        = string
  description = "Client ID do aplicativo Cognito"
  sensitive   = false
}

# RDS / Banco de Dados
variable "rds_connection_string" {
  type        = string
  description = "Connection string completa do PostgreSQL no formato 'Host=...;Port=...;Database=...;Username=...;Password=...'"
  sensitive   = true
}

# JWT Settings
variable "jwt_secret" {
  type        = string
  description = "Chave secreta para assinar tokens JWT (mínimo 32 caracteres)"
  sensitive   = true
}

variable "jwt_issuer" {
  type        = string
  description = "Emissor do token JWT (ex: FastFood.Auth)"
}

variable "jwt_audience" {
  type        = string
  description = "Audiência do token JWT (ex: FastFood.API)"
}

variable "jwt_expiration_hours" {
  type        = number
  description = "Tempo de expiração do token JWT em horas"
  default     = 24
}

