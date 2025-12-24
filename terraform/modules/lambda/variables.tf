variable "function_name" {
  description = "Nome da função Lambda (sem prefixo do projeto)"
  type        = string
}

variable "project_name" {
  description = "Nome do projeto (usado como prefixo)"
  type        = string
}

variable "env" {
  description = "Ambiente (ex: dev, prod)"
  type        = string
}

variable "handler" {
  description = "Handler da função Lambda (ex: bootstrap para .NET custom runtime)"
  type        = string
  default     = "bootstrap"
}

variable "runtime" {
  description = "Runtime da função Lambda (ex: provided.al2 para .NET 8 custom runtime)"
  type        = string
  default     = "provided.al2"
}

variable "role_arn" {
  description = "ARN da role IAM para a função Lambda"
  type        = string
}

variable "timeout" {
  description = "Timeout da função Lambda em segundos"
  type        = number
  default     = 30
}

variable "memory_size" {
  description = "Memória alocada para a função Lambda em MB"
  type        = number
  default     = 512
}

variable "package_type" {
  description = "Tipo de pacote: Zip ou Image"
  type        = string
  default     = "Zip"
  validation {
    condition     = contains(["Zip", "Image"], var.package_type)
    error_message = "package_type deve ser 'Zip' ou 'Image'."
  }
}

variable "source_code_hash" {
  description = "Hash do código fonte (usado para forçar atualização). Use 'placeholder' para criação inicial."
  type        = string
  default     = "placeholder"
}

variable "environment_variables" {
  description = "Variáveis de ambiente da função Lambda"
  type        = map(string)
  default     = {}
}

variable "reserved_concurrent_executions" {
  description = "Número de execuções concorrentes reservadas (null = sem limite)"
  type        = number
  default     = null
}

variable "vpc_config" {
  description = "Configuração de VPC (null = sem VPC)"
  type = object({
    security_group_ids = list(string)
    subnet_ids         = list(string)
  })
  default = null
}

variable "depends_on_resources" {
  description = "Lista de recursos que a função Lambda depende"
  type        = list(any)
  default     = []
}

variable "common_tags" {
  description = "Tags comuns para aplicar aos recursos"
  type        = map(string)
  default     = {}
}

