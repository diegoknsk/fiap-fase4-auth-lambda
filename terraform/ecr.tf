# ============================================================================
# ECR Repositories para Lambda Container Images
# ============================================================================

# Repositório ECR para auth-lambda
resource "aws_ecr_repository" "auth_lambda" {
  name                 = "${var.project_name}-auth-lambda"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  encryption_configuration {
    encryption_type = "AES256"
  }

  tags = merge(
    {
      Name        = "${var.project_name}-auth-lambda"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
      Service     = "auth-lambda"
    },
    var.common_tags
  )
}

# Repositório ECR para auth-admin-lambda
resource "aws_ecr_repository" "auth_admin_lambda" {
  name                 = "${var.project_name}-auth-admin-lambda"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  encryption_configuration {
    encryption_type = "AES256"
  }

  tags = merge(
    {
      Name        = "${var.project_name}-auth-admin-lambda"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
      Service     = "auth-admin-lambda"
    },
    var.common_tags
  )
}

# Repositório ECR para auth-migrator-lambda
resource "aws_ecr_repository" "auth_migrator_lambda" {
  name                 = "${var.project_name}-auth-migrator-lambda"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  encryption_configuration {
    encryption_type = "AES256"
  }

  tags = merge(
    {
      Name        = "${var.project_name}-auth-migrator-lambda"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
      Service     = "auth-migrator-lambda"
    },
    var.common_tags
  )
}

# ============================================================================
# Lifecycle Policies para ECR (manter apenas últimas 10 imagens)
# ============================================================================

resource "aws_ecr_lifecycle_policy" "auth_lambda" {
  repository = aws_ecr_repository.auth_lambda.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Manter apenas últimas 10 imagens"
        selection = {
          tagStatus     = "any"
          countType     = "imageCountMoreThan"
          countNumber   = 10
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

resource "aws_ecr_lifecycle_policy" "auth_admin_lambda" {
  repository = aws_ecr_repository.auth_admin_lambda.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Manter apenas últimas 10 imagens"
        selection = {
          tagStatus     = "any"
          countType     = "imageCountMoreThan"
          countNumber   = 10
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

resource "aws_ecr_lifecycle_policy" "auth_migrator_lambda" {
  repository = aws_ecr_repository.auth_migrator_lambda.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Manter apenas últimas 10 imagens"
        selection = {
          tagStatus     = "any"
          countType     = "imageCountMoreThan"
          countNumber   = 10
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

