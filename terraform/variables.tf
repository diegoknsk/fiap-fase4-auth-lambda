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

