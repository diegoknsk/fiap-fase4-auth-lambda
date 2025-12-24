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
