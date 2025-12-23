# Subtask 02: Criar data sources para localizar Security Group, VPC e Subnets

## Descrição
Criar data sources do Terraform para localizar o Security Group pelo nome ou ID, descobrir a VPC default e suas subnets automaticamente. O data source do Security Group deve priorizar o ID se fornecido, caso contrário buscar pelo nome. Se nenhum for fornecido, usar o nome padrão "fiap-fase4-auth-sg". As subnets serão descobertas automaticamente da VPC default.

## Passos de implementação
- Criar arquivo `terraform/data.tf` (se não existir)
- Criar data source `aws_security_group` para buscar o Security Group:
  - Usar `name` quando `var.lambda_security_group_id` estiver vazio
  - Usar `id` quando `var.lambda_security_group_id` estiver preenchido
  - Usar `var.lambda_security_group_name` como fallback (default: "fiap-fase4-auth-sg")
- Criar data source `aws_vpc` para buscar a VPC default:
  ```hcl
  data "aws_vpc" "default" {
    default = true
  }
  ```
- Criar data source `aws_subnets` para descobrir subnets da VPC default:
  ```hcl
  data "aws_subnets" "default" {
    filter {
      name   = "vpc-id"
      values = [data.aws_vpc.default.id]
    }
  }
  ```
- Adicionar comentários explicando a lógica de busca e descoberta automática
- Considerar criar um local value para simplificar a lógica do Security Group:
  ```hcl
  locals {
    security_group_name = var.lambda_security_group_id != "" ? null : (var.lambda_security_group_name != "" ? var.lambda_security_group_name : "fiap-fase4-auth-sg")
    security_group_id    = var.lambda_security_group_id != "" ? var.lambda_security_group_id : null
  }
  ```
- Usar os data sources com as condições apropriadas

## Como testar
- Executar `terraform validate` (deve passar sem erros)
- Executar `terraform plan` com diferentes combinações:
  - Apenas `lambda_security_group_name` fornecido
  - Apenas `lambda_security_group_id` fornecido
  - Nenhum fornecido (deve usar default "fiap-fase4-auth-sg")
- Verificar que o Security Group é localizado corretamente em cada caso
- Verificar que a VPC default é encontrada
- Verificar que as subnets são descobertas automaticamente
- Verificar mensagens de erro se o Security Group não for encontrado

## Critérios de aceite
- Arquivo `terraform/data.tf` criado ou atualizado
- Data source `aws_security_group` criado
- Data source `aws_vpc` criado para buscar VPC default
- Data source `aws_subnets` criado para descobrir subnets automaticamente
- Lógica de busca do Security Group implementada: ID tem prioridade sobre nome
- Nome padrão "fiap-fase4-auth-sg" usado quando nenhum parâmetro fornecido
- Comentários explicam a lógica de busca e descoberta automática
- `terraform validate` passa sem erros
- `terraform plan` funciona com diferentes combinações de parâmetros
- Security Group é localizado corretamente em todos os cenários
- VPC default e subnets são descobertas automaticamente

