output "function_arn" {
  description = "ARN da função Lambda"
  value       = aws_lambda_function.function.arn
}

output "function_name" {
  description = "Nome da função Lambda"
  value       = aws_lambda_function.function.function_name
}

output "function_invoke_arn" {
  description = "ARN para invocação da função Lambda (para API Gateway)"
  value       = aws_lambda_function.function.invoke_arn
}

output "function_qualified_arn" {
  description = "ARN qualificado da função Lambda (com versão)"
  value       = aws_lambda_function.function.qualified_arn
}

