# ============================================================================
# Configuração de DNS para VPC
# ============================================================================
# Quando uma Lambda está em VPC, ela precisa de DNS habilitado para resolver
# nomes de recursos dentro da VPC (como RDS)
# ============================================================================

# Habilitar DNS Support na VPC
# Isso permite que recursos na VPC resolvam nomes DNS
resource "aws_vpc" "default" {
  count = 0 # Não criar, apenas usar o existente
}

# Habilitar DNS Support (usando aws_vpc_attribute não existe, precisamos usar null_resource ou data)
# Nota: Não podemos modificar atributos de VPC default via Terraform diretamente
# Isso precisa ser feito manualmente ou via script

# Data source para verificar configuração DNS atual
data "aws_vpc" "default" {
  default = true
}

# Output para informar se DNS precisa ser habilitado manualmente
output "vpc_dns_configuration_required" {
  description = "Instruções para habilitar DNS na VPC (se necessário)"
  value = <<-EOT
    ⚠️ IMPORTANTE: Verifique se a VPC tem DNS habilitado:
    
    Execute:
    aws ec2 describe-vpcs --vpc-ids ${data.aws_vpc.default.id} --region ${var.aws_region} --query 'Vpcs[0].[EnableDnsSupport,EnableDnsHostnames]'
    
    Se EnableDnsSupport ou EnableDnsHostnames estiverem false, habilite:
    
    aws ec2 modify-vpc-attribute --vpc-id ${data.aws_vpc.default.id} --enable-dns-support --region ${var.aws_region}
    aws ec2 modify-vpc-attribute --vpc-id ${data.aws_vpc.default.id} --enable-dns-hostnames --region ${var.aws_region}
    
    Isso é necessário para que Lambdas em VPC possam resolver DNS de recursos como RDS.
  EOT
}

