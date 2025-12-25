# ============================================================================
# Outputs do Terraform para referência externa
# ============================================================================

# Outputs da Lambda auth-customer-lambda
output "lambda_auth_customer_arn" {
  description = "ARN da função Lambda auth-customer-lambda"
  value       = module.auth_customer_lambda.function_arn
}

output "lambda_auth_customer_name" {
  description = "Nome da função Lambda auth-customer-lambda"
  value       = module.auth_customer_lambda.function_name
}

output "lambda_auth_customer_function_url" {
  description = "URL pública da função Lambda auth-customer-lambda (Function URL)"
  value       = aws_lambda_function_url.lambda_customer_url.function_url
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

# Outputs do Security Group "Sg_Lambdas_Auth"
output "sg_lambdas_auth_id" {
  description = "ID do Security Group Sg_Lambdas_Auth (para Customer e Migrator Lambdas)"
  value       = local.sg_lambdas_auth_id
}

output "sg_lambdas_auth_name" {
  description = "Nome do Security Group Sg_Lambdas_Auth (fixo: Sg_Lambdas_Auth)"
  value       = "Sg_Lambdas_Auth"
}

# Output do repositório ECR único
output "ecr_lambda_images_repository_url" {
  description = "URL do repositório ECR único para todas as imagens Lambda"
  value       = aws_ecr_repository.lambda_images.repository_url
}
