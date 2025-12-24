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

  # Configurações de timeout e memória
  # Timeout aumentado para 60s devido a cold start em VPC e inicialização do DbContext
  timeout     = 60
  memory_size = 512

  # VPC e Security Group são gerenciados em outro lugar
  # O Terraform apenas atualiza a imagem do Lambda
  lifecycle {
    ignore_changes = [
      vpc_config,
      role,           # Role pode ser diferente se gerenciada em outro lugar
      timeout,        # Timeout pode ser diferente
      memory_size,    # Memory pode ser diferente
      tags,           # Tags podem ser diferentes
    ]
    # Evita recriar o Lambda - apenas atualiza a imagem
    create_before_destroy = false
    # Previne recriação desnecessária
    prevent_destroy = false
  }

  # Variáveis de ambiente para configuração do Lambda
  environment {
    variables = {
      # Modo da Lambda: "Customer", "Admin" ou "Migrator" (default: "Customer")
      # Define qual controller será ativado na mesma imagem
      LAMBDA_MODE = var.lambda_mode

      # Cognito - Autenticação de administradores
      COGNITO__REGION     = var.cognito_region
      COGNITO__USERPOOLID = var.cognito_user_pool_id
      COGNITO__CLIENTID   = var.cognito_client_id

      # RDS Connection String - Passada diretamente, já vem completa do parâmetro
      ConnectionStrings__DefaultConnection = var.rds_connection_string

      # JWT Settings - Configuração de tokens JWT
      JwtSettings__Secret          = var.jwt_secret
      JwtSettings__Issuer          = var.jwt_issuer
      JwtSettings__Audience        = var.jwt_audience
      JwtSettings__ExpirationHours = tostring(var.jwt_expiration_hours)
    }
  }

  # Tags padrão do projeto
  tags = {
    Name      = var.lambda_function_name
    ManagedBy = "Terraform"
    Project   = "FastFood-Auth"
  }
}

# Lambda Function URL para acesso direto à API
# Permite acesso HTTP direto sem necessidade de API Gateway
resource "aws_lambda_function_url" "lambda_url" {
  function_name      = aws_lambda_function.lambda.function_name
  authorization_type = "NONE"  # NONE = acesso público, AWS_IAM = requer autenticação IAM

  cors {
    allow_credentials = false
    allow_origins    = ["*"]  # Permite qualquer origem (ajuste conforme necessário)
    allow_methods    = ["*"]  # Permite todos os métodos HTTP
    allow_headers    = ["*"]  # Permite todos os headers
    expose_headers   = ["*"]  # Expõe todos os headers na resposta
    max_age          = 86400  # Cache de preflight por 24 horas
  }
}

