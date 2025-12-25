# ============================================================================
# ECR Repository único para todas as Lambda Container Images
# ============================================================================

# Repositório ECR único para todas as Lambdas
# As imagens serão diferenciadas por tags: auth-customer-lambda, auth-admin-lambda, auth-migrator-lambda
resource "aws_ecr_repository" "lambda_images" {
  name                 = "fiap-fase4-auth-lambda"
  image_tag_mutability = "MUTABLE"
  force_delete         = true

  image_scanning_configuration {
    scan_on_push = true
  }

  encryption_configuration {
    encryption_type = "AES256"
  }

  tags = merge(
    {
      Name        = "fiap-fase4-auth-lambda"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
      Service     = "lambda-images"
    },
    var.common_tags
  )
}

# ============================================================================
# Lifecycle Policy para ECR (manter apenas últimas 30 imagens - 10 por Lambda)
# ============================================================================

resource "aws_ecr_lifecycle_policy" "lambda_images" {
  repository = aws_ecr_repository.lambda_images.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Manter apenas últimas 30 imagens (10 por Lambda)"
        selection = {
          tagStatus     = "any"
          countType     = "imageCountMoreThan"
          countNumber   = 30
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

