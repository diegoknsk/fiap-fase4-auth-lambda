# Recurso AWS Lambda Function configurado para usar imagem de container do ECR
# Terraform não faz push de imagem, apenas atualiza o image_uri do Lambda
# O push da imagem é responsabilidade do CI/CD (GitHub Actions)
# 
# Este recurso é idempotente: terraform apply pode ser executado múltiplas vezes
# sem causar problemas, apenas atualizando o Lambda quando a image_uri mudar

resource "aws_lambda_function" "lambda" {
  function_name = var.lambda_function_name
  package_type  = "Image"
  image_uri     = var.ecr_image_uri

  # Role IAM: obrigatória para o Lambda funcionar
  # Se o Lambda já existe, obtenha o ARN da role do Lambda existente
  role = var.lambda_role_arn

  # Outras configurações opcionais (descomentar e ajustar conforme necessário):
  # timeout      = 30
  # memory_size  = 512
  # environment {
  #   variables = {
  #     ENV_VAR = "value"
  #   }
  # }
  
  # Tags padrão do projeto
  tags = {
    Name        = var.lambda_function_name
    ManagedBy   = "Terraform"
    Project     = "FastFood-Auth"
  }
}

