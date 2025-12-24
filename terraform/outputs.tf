# ============================================================================
# Outputs do Terraform para referência externa
# ============================================================================

# Outputs da Lambda auth-lambda
output "lambda_auth_arn" {
  description = "ARN da função Lambda auth-lambda"
  value       = module.auth_lambda.function_arn
}

output "lambda_auth_name" {
  description = "Nome da função Lambda auth-lambda"
  value       = module.auth_lambda.function_name
}

output "lambda_auth_function_url" {
  description = "URL pública da função Lambda auth-lambda (Function URL)"
  value       = aws_lambda_function_url.lambda_url.function_url
}

# Outputs da Lambda auth-admin-lambda
output "lambda_auth_admin_arn" {
  description = "ARN da função Lambda auth-admin-lambda"
  value       = module.auth_admin_lambda.function_arn
}

output "lambda_auth_admin_name" {
  description = "Nome da função Lambda auth-admin-lambda"
  value       = module.auth_admin_lambda.function_name
}

output "lambda_auth_admin_function_url" {
  description = "URL pública da função Lambda auth-admin-lambda (Function URL)"
  value       = aws_lambda_function_url.lambda_admin_url.function_url
}

# Outputs da Lambda auth-migrator-lambda
output "lambda_auth_migrator_arn" {
  description = "ARN da função Lambda auth-migrator-lambda"
  value       = module.auth_migrator_lambda.function_arn
}

output "lambda_auth_migrator_name" {
  description = "Nome da função Lambda auth-migrator-lambda"
  value       = module.auth_migrator_lambda.function_name
}

output "lambda_auth_migrator_function_url" {
  description = "URL pública da função Lambda auth-migrator-lambda (Function URL)"
  value       = aws_lambda_function_url.lambda_migrator_url.function_url
}

# Outputs do Security Group
output "lambda_security_group_id" {
  description = "ID do Security Group da função Lambda"
  value       = local.sg_lambda_id
}

output "lambda_security_group_name" {
  description = "Nome do Security Group da função Lambda (fixo: lambda_auth_sg)"
  value       = "lambda_auth_sg"
}

# Output do repositório ECR único
output "ecr_lambda_images_repository_url" {
  description = "URL do repositório ECR único para todas as imagens Lambda"
  value       = aws_ecr_repository.lambda_images.repository_url
}
