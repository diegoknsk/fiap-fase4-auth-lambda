# VPC Endpoints para permitir acesso aos serviços AWS pelas Lambdas em VPC
# Quando uma Lambda está em VPC, ela perde acesso à internet pública
# VPC Endpoints permitem acesso aos serviços AWS sem sair da VPC

# Security Group para VPC Endpoints
resource "aws_security_group" "vpc_endpoint_sg" {
  name        = "lambda-vpc-endpoint-sg"
  description = "Security group for VPC Endpoints - allows Lambda to access AWS services"
  vpc_id      = data.aws_vpc.default.id

  ingress {
    from_port       = 443
    to_port         = 443
    protocol        = "tcp"
    security_groups = [local.sg_lambda_id]
    description     = "Allow HTTPS from Lambda Security Group"
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow all outbound traffic"
  }

  tags = merge(
    {
      Name        = "lambda-vpc-endpoint-sg"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )

  lifecycle {
    create_before_destroy = true
  }
}

# VPC Endpoint para S3 (Gateway type - sem custo adicional)
resource "aws_vpc_endpoint" "s3" {
  vpc_id            = data.aws_vpc.default.id
  service_name      = "com.amazonaws.${var.aws_region}.s3"
  vpc_endpoint_type = "Gateway"
  route_table_ids   = data.aws_route_tables.default.ids

  tags = merge(
    {
      Name        = "lambda-s3-endpoint"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )
}

# VPC Endpoint para Secrets Manager (Interface type)
resource "aws_vpc_endpoint" "secretsmanager" {
  vpc_id              = data.aws_vpc.default.id
  service_name        = "com.amazonaws.${var.aws_region}.secretsmanager"
  vpc_endpoint_type   = "Interface"
  subnet_ids          = data.aws_subnets.eks_supported.ids
  security_group_ids  = [aws_security_group.vpc_endpoint_sg.id]
  private_dns_enabled = true

  tags = merge(
    {
      Name        = "lambda-secretsmanager-endpoint"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )
}

# VPC Endpoint para CloudWatch Logs (Interface type)
resource "aws_vpc_endpoint" "logs" {
  vpc_id              = data.aws_vpc.default.id
  service_name        = "com.amazonaws.${var.aws_region}.logs"
  vpc_endpoint_type   = "Interface"
  subnet_ids          = data.aws_subnets.eks_supported.ids
  security_group_ids  = [aws_security_group.vpc_endpoint_sg.id]
  private_dns_enabled = true

  tags = merge(
    {
      Name        = "lambda-logs-endpoint"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )
}

# Data source para buscar route tables da VPC
data "aws_route_tables" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

