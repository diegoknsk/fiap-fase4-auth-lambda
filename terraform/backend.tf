# Backend remoto S3 para armazenar o state do Terraform
# IMPORTANTE: O bucket deve existir antes de executar terraform init
# Configurar bucket, key e region conforme necessário

terraform {
  backend "s3" {
    # Configuração do backend será feita via variáveis de ambiente ou terraform init -backend-config
    # Exemplo de uso:
    # terraform init -backend-config="bucket=seu-bucket-terraform-state" -backend-config="key=lambda-auth/terraform.tfstate" -backend-config="region=us-east-1"
    # 
    # Ou via variáveis de ambiente:
    # export TF_BACKEND_BUCKET="seu-bucket-terraform-state"
    # export TF_BACKEND_KEY="lambda-auth/terraform.tfstate"
    # export TF_BACKEND_REGION="us-east-1"
  }
}

