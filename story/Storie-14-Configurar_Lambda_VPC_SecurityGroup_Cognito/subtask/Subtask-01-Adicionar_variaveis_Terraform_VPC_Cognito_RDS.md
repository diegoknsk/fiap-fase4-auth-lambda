# Subtask 01: Adicionar variáveis Terraform para Security Group, VPC, Cognito e RDS

## Descrição
Adicionar todas as variáveis Terraform necessárias para configurar o Lambda com acesso à VPC, Security Group, e variáveis de ambiente do Cognito e RDS. As variáveis devem seguir o padrão do projeto: obrigatórias sem default, opcionais com default quando apropriado.

## Passos de implementação
- Abrir arquivo `terraform/variables.tf`
- Adicionar variáveis para Security Group:
  - `lambda_security_group_name` (opcional, default: "fiap-fase4-auth-sg")
  - `lambda_security_group_id` (opcional, string vazia por padrão)
- Adicionar variáveis para Cognito:
  - `cognito_user_pool_id` (obrigatória)
  - `cognito_region` (obrigatória)
  - `cognito_client_id` (obrigatória)
- Adicionar variável para RDS:
  - `rds_connection_string` (obrigatória, sensitive: true) - Connection string completa do PostgreSQL
- Adicionar variáveis para JWT (se necessário):
  - `jwt_secret` (obrigatória, sensitive: true)
  - `jwt_issuer` (obrigatória)
  - `jwt_audience` (obrigatória)
  - `jwt_expiration_hours` (opcional, default: 24)
- Adicionar descrições claras para cada variável
- Adicionar exemplos de valores nos comentários quando apropriado

## Como testar
- Executar `terraform validate` (deve passar sem erros)
- Executar `terraform fmt` (deve formatar corretamente)
- Verificar que variáveis obrigatórias não têm default
- Verificar que variáveis opcionais têm default apropriado
- Verificar que variáveis sensíveis têm `sensitive = true`

## Critérios de aceite
- Arquivo `terraform/variables.tf` atualizado com todas as novas variáveis
- Variáveis obrigatórias não têm valores default
- Variáveis opcionais têm valores default apropriados
- Variáveis sensíveis (connection string, secrets) têm `sensitive = true`
- Todas as variáveis têm descrições claras
- `terraform validate` passa sem erros
- `terraform fmt` formatado corretamente
- Variáveis seguem o padrão de nomenclatura do projeto

