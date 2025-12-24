# Data sources para localizar recursos existentes na AWS

# VPC default
data "aws_vpc" "default" {
  default = true
}

# Subnets para EKS (ou equivalente) - usado pelas Lambdas em VPC
data "aws_subnets" "eks_supported" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}
