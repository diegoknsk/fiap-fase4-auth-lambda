# VPC Endpoint para AWS Cognito
# Permite que o Lambda acesse o Cognito sem sair da VPC (sem NAT Gateway)
# Mais seguro e econômico que NAT Gateway

# Security Group para o VPC Endpoint
# Permite tráfego HTTPS (porta 443) de/para o Lambda
resource "aws_security_group" "vpc_endpoint_cognito" {
  name        = "vpc-endpoint-cognito-sg"
  description = "Security Group para VPC Endpoint do Cognito"
  vpc_id      = data.aws_vpc.default.id

  # Permitir tráfego HTTPS de saída (do Lambda para o Cognito)
  egress {
    description = "HTTPS para Cognito"
    from_port  = 443
    to_port    = 443
    protocol   = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Permitir tráfego HTTPS de entrada (respostas do Cognito)
  ingress {
    description = "HTTPS do Cognito"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name      = "vpc-endpoint-cognito-sg"
    ManagedBy = "Terraform"
    Project   = "FastFood-Auth"
  }
}

# VPC Endpoint para Cognito Identity Provider
resource "aws_vpc_endpoint" "cognito_idp" {
  vpc_id              = data.aws_vpc.default.id
  service_name        = "com.amazonaws.${var.aws_region}.cognito-idp"
  vpc_endpoint_type   = "Interface"
  subnet_ids          = data.aws_subnets.default.ids
  security_group_ids  = [aws_security_group.vpc_endpoint_cognito.id]
  
  # Aceitar requisições sem necessidade de aprovação manual
  auto_accept = true

  # Política para permitir acesso ao Cognito
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Principal = "*"
        Action = [
          "cognito-idp:*"
        ]
        Resource = "*"
      }
    ]
  })

  tags = {
    Name      = "vpc-endpoint-cognito-idp"
    ManagedBy = "Terraform"
    Project   = "FastFood-Auth"
  }
}

