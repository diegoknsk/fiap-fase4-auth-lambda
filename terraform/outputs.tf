# Outputs do Terraform para referência externa

output "lambda_function_name" {
  description = "Nome da função Lambda"
  value       = aws_lambda_function.lambda.function_name
}

output "lambda_function_arn" {
  description = "ARN da função Lambda"
  value       = aws_lambda_function.lambda.arn
}

output "lambda_function_image_uri" {
  description = "URI da imagem ECR atualmente configurada no Lambda"
  value       = aws_lambda_function.lambda.image_uri
}

output "lambda_function_last_modified" {
  description = "Data da última modificação da função Lambda"
  value       = aws_lambda_function.lambda.last_modified
}

output "lambda_function_url" {
  description = "URL da função Lambda para acesso direto via HTTP (Function URL)"
  value       = aws_lambda_function_url.lambda_url.function_url
}

