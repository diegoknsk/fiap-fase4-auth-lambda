# ============================================================================
# Variáveis do Terraform para deploy das Lambdas
# ============================================================================

variable "aws_region" {
  type        = string
  description = "Região AWS onde os recursos serão criados (ex: us-east-1)"
  default     = "us-east-1"
}

variable "project_name" {
  type        = string
  description = "Nome do projeto (usado como prefixo nos recursos)"
  default     = "autenticacao"
}

variable "env" {
  type        = string
  description = "Ambiente (ex: dev, staging, prod)"
  default     = "dev"
}

variable "lab_role" {
  type        = string
  description = "ARN da role IAM LabRole para as funções Lambda"
}

variable "common_tags" {
  type        = map(string)
  description = "Tags comuns para aplicar a todos os recursos"
  default     = {}
}

# Variáveis para Cognito
variable "cognito_region" {
  type        = string
  description = "Região do Cognito User Pool (ex: us-east-1)"
  default     = ""
}

variable "cognito_user_pool_id" {
  type        = string
  description = "ID do Cognito User Pool (ex: us-east-1_XXXXXXXXX)"
  default     = ""
}

variable "cognito_client_id" {
  type        = string
  description = "Client ID do Cognito App Client"
  default     = ""
}

# Variáveis para RDS
variable "rds_connection_string" {
  type        = string
  description = "Connection string completa do RDS PostgreSQL"
  default     = ""
  sensitive   = true
}

# Variáveis para JWT
variable "jwt_secret" {
  type        = string
  description = "Chave secreta para assinatura JWT (mínimo 32 caracteres)"
  default     = ""
  sensitive   = true
}

variable "jwt_issuer" {
  type        = string
  description = "Emissor do token JWT"
  default     = ""
}

variable "jwt_audience" {
  type        = string
  description = "Audience do token JWT"
  default     = ""
}
