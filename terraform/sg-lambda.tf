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
    prevent_destroy       = true
  }
}

# Data source para buscar o Security Group existente (apenas se já existir)
# COMENTADO TEMPORARIAMENTE - será reativado depois
# data "aws_security_group" "sg_lambda_existing" {
#   count = length(data.aws_security_groups.sg_lambda_existing.ids) > 0 ? 1 : 0
#   id    = data.aws_security_groups.sg_lambda_existing.ids[0]
# }

# Usar o Security Group existente ou o criado
locals {
  # Se o SG já existe na AWS, usa o data source
  # Se não existe, usa o resource criado pelo Terraform
  sg_lambda_id = length(data.aws_security_groups.sg_lambda_existing.ids) > 0 ? data.aws_security_groups.sg_lambda_existing.ids[0] : aws_security_group.sg_lambda[0].id
  
  # COMENTADO TEMPORARIAMENTE - será reativado depois
  # # Se o SG já existe, verifica se as regras já estão presentes
  # # Se o SG está sendo criado agora, assume que as regras não existem
  # sg_exists = length(data.aws_security_groups.sg_lambda_existing.ids) > 0
  # 
  # # Verifica se a regra HTTP (porta 80) já existe (apenas se o SG já existir)
  # http_rule_exists = local.sg_exists && length(data.aws_security_group.sg_lambda_existing) > 0 ? length([
  #   for rule in data.aws_security_group.sg_lambda_existing[0].ingress : rule
  #   if rule.from_port == 80 && rule.to_port == 80 && rule.protocol == "tcp" && length(rule.cidr_blocks) > 0 && rule.cidr_blocks[0] == "0.0.0.0/0"
  # ]) > 0 : false
  # 
  # # Verifica se a regra HTTPS (porta 443) já existe (apenas se o SG já existir)
  # https_rule_exists = local.sg_exists && length(data.aws_security_group.sg_lambda_existing) > 0 ? length([
  #   for rule in data.aws_security_group.sg_lambda_existing[0].ingress : rule
  #   if rule.from_port == 443 && rule.to_port == 443 && rule.protocol == "tcp" && length(rule.cidr_blocks) > 0 && rule.cidr_blocks[0] == "0.0.0.0/0"
  # ]) > 0 : false
}

# COMENTADO TEMPORARIAMENTE - será reativado depois
# Regra de ingress para HTTP (porta 80) - cria apenas se não existir
# resource "aws_security_group_rule" "lambda_http_ingress" {
#   count = local.http_rule_exists ? 0 : 1
#   
#   type              = "ingress"
#   from_port         = 80
#   to_port           = 80
#   protocol          = "tcp"
#   cidr_blocks       = ["0.0.0.0/0"]
#   security_group_id = local.sg_lambda_id
#   description       = "Allow HTTP traffic from anywhere (for Lambda Function URL)"
#   
#   lifecycle {
#     create_before_destroy = true
#   }
# }

# COMENTADO TEMPORARIAMENTE - será reativado depois
# Regra de ingress para HTTPS (porta 443) - cria apenas se não existir
# resource "aws_security_group_rule" "lambda_https_ingress" {
#   count = local.https_rule_exists ? 0 : 1
#   
#   type              = "ingress"
#   from_port         = 443
#   to_port           = 443
#   protocol          = "tcp"
#   cidr_blocks       = ["0.0.0.0/0"]
#   security_group_id = local.sg_lambda_id
#   description       = "Allow HTTPS traffic from anywhere (for Lambda Function URL)"
#   
#   lifecycle {
#     create_before_destroy = true
#   }
# }

# ============================================================================
# Security Group "Sg_Lambdas_Auth" para Lambdas Customer e Migrator
# Nome fixo: Sg_Lambdas_Auth (independente do project_name)
# Criada de forma idempotente e não removida no destroy
# ============================================================================

# Tenta buscar o Security Group existente
data "aws_security_groups" "sg_lambdas_auth_existing" {
  filter {
    name   = "group-name"
    values = ["Sg_Lambdas_Auth"]
  }
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Cria o Security Group apenas se não existir
resource "aws_security_group" "sg_lambdas_auth" {
  count       = length(data.aws_security_groups.sg_lambdas_auth_existing.ids) > 0 ? 0 : 1
  name        = "Sg_Lambdas_Auth"
  description = "Security group for Lambda Functions Customer and Migrator - allows connection with RDS"
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
      Name        = "Sg_Lambdas_Auth"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )

  lifecycle {
    create_before_destroy = true
    prevent_destroy       = true
  }
}

# Usar o Security Group existente ou o criado
locals {
  # Se o SG já existe na AWS, usa o data source
  # Se não existe, usa o resource criado pelo Terraform
  sg_lambdas_auth_id = length(data.aws_security_groups.sg_lambdas_auth_existing.ids) > 0 ? data.aws_security_groups.sg_lambdas_auth_existing.ids[0] : aws_security_group.sg_lambdas_auth[0].id
}