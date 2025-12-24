# Security Group para Lambda Functions em VPC
# Nome fixo: lambda_auth_sg (independente do project_name)
# Tenta usar o existente, se não existir, cria um novo

# Tenta buscar o Security Group existente
data "aws_security_groups" "sg_lambda_existing" {
  filter {
    name   = "group-name"
    values = ["lambda_auth_sg"]
  }
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Cria o Security Group apenas se não existir
resource "aws_security_group" "sg_lambda" {
  count       = length(data.aws_security_groups.sg_lambda_existing.ids) > 0 ? 0 : 1
  name        = "lambda_auth_sg"
  description = "Security group for Lambda Function - allows public access via Function URL and connection with RDS"
  vpc_id      = data.aws_vpc.default.id

  # Egress: permitir todo tráfego de saída
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow all outbound traffic"
  }

  tags = merge(
    {
      Name        = "lambda_auth_sg"
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

# Usar o Security Group existente ou o criado
locals {
  sg_lambda_id = length(data.aws_security_groups.sg_lambda_existing.ids) > 0 ? data.aws_security_groups.sg_lambda_existing.ids[0] : (length(aws_security_group.sg_lambda) > 0 ? aws_security_group.sg_lambda[0].id : data.aws_security_groups.sg_lambda_existing.ids[0])
}

# Regra de ingress para HTTP (porta 80)
resource "aws_security_group_rule" "lambda_http_ingress" {
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = local.sg_lambda_id
  description       = "Allow HTTP traffic from anywhere (for Lambda Function URL)"
}

# Regra de ingress para HTTPS (porta 443)
resource "aws_security_group_rule" "lambda_https_ingress" {
  type              = "ingress"
  from_port         = 443
  to_port           = 443
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = local.sg_lambda_id
  description       = "Allow HTTPS traffic from anywhere (for Lambda Function URL)"
}
