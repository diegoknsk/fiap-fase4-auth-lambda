# Data sources para localizar recursos existentes na AWS

# Local values para simplificar a lógica de busca do Security Group
locals {
  # Se lambda_security_group_id estiver preenchido, usa o ID
  # Caso contrário, usa o nome (ou o padrão "fiap-fase4-auth-sg")
  use_security_group_id = var.lambda_security_group_id != ""
  security_group_name   = var.lambda_security_group_name
  security_group_id     = var.lambda_security_group_id
}

# Data source para buscar o Security Group por ID (quando ID é fornecido)
data "aws_security_group" "lambda_sg_by_id" {
  count = local.use_security_group_id ? 1 : 0
  id    = local.security_group_id
}

# Data source para buscar o Security Group por nome (quando ID não é fornecido)
data "aws_security_group" "lambda_sg_by_name" {
  count = local.use_security_group_id ? 0 : 1
  name  = local.security_group_name
}

# Local value para unificar o resultado do Security Group
# Usa o ID do data source apropriado baseado em qual foi criado
locals {
  lambda_security_group_id = local.use_security_group_id ? data.aws_security_group.lambda_sg_by_id[0].id : data.aws_security_group.lambda_sg_by_name[0].id
}

# Data source para buscar a VPC default
data "aws_vpc" "default" {
  default = true
}

# Data source para descobrir subnets da VPC default automaticamente
data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

