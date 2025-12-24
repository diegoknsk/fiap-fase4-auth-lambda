# Security Group para Lambda Functions em VPC
# Nome fixo: lambda_auth_sg (independente do project_name)
resource "aws_security_group" "sg_lambda" {
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
}

# Regra de ingress para HTTP (porta 80)
resource "aws_security_group_rule" "lambda_http_ingress" {
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = aws_security_group.sg_lambda.id
  description       = "Allow HTTP traffic from anywhere (for Lambda Function URL)"
}

# Regra de ingress para HTTPS (porta 443)
resource "aws_security_group_rule" "lambda_https_ingress" {
  type              = "ingress"
  from_port         = 443
  to_port           = 443
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = aws_security_group.sg_lambda.id
  description       = "Allow HTTPS traffic from anywhere (for Lambda Function URL)"
}

