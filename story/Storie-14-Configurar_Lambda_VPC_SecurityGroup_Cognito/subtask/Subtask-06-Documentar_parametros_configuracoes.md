# Subtask 06: Documentar parâmetros e configurações nos arquivos de docs

## Descrição
Atualizar os arquivos de documentação (`docs/DEPLOY_LAMBDA.md` e `docs/VARIAVEIS_AMBIENTE.md`) com todos os novos parâmetros, configurações de VPC, Security Group, Cognito e RDS, incluindo exemplos de valores e instruções de como obter os valores dos recursos existentes.

## Passos de implementação
- Abrir arquivo `docs/DEPLOY_LAMBDA.md`
- Adicionar seção "Configuração de VPC e Security Group":
  - Explicar por que o Lambda precisa estar na VPC
  - Documentar parâmetros: `lambda_security_group_name`, `lambda_security_group_id`, `vpc_subnet_ids`
  - Explicar lógica de busca do Security Group (ID tem prioridade sobre nome)
  - Explicar nome padrão "fiap-fase4-auth-sg"
  - Adicionar exemplos de como obter IDs de subnets
  - Adicionar exemplos de como obter Security Group ID ou nome
- Adicionar seção "Configuração do Cognito":
  - Documentar parâmetros: `cognito_user_pool_id`, `cognito_region`, `cognito_client_id`
  - Adicionar instruções de como obter esses valores do AWS Console
  - Adicionar exemplos de valores
- Adicionar seção "Configuração do RDS":
  - Documentar parâmetro: `rds_connection_string` (connection string completa)
  - Explicar formato da connection string: `Host=...;Port=...;Database=...;Username=...;Password=...`
  - Adicionar exemplo completo de connection string
  - Adicionar instruções de como obter/construir a connection string do RDS
- Adicionar seção "Configuração JWT" (se necessário):
  - Documentar parâmetros: `jwt_secret`, `jwt_issuer`, `jwt_audience`, `jwt_expiration_hours`
  - Adicionar exemplos de valores
- Atualizar seção "Secrets do GitHub" com todos os novos secrets
- Atualizar seção "Variáveis Terraform" com todas as novas variáveis
- Adicionar seção "Como obter valores dos recursos existentes":
  - Como obter Security Group ID/nome
  - Nota: Subnets são descobertas automaticamente (não requer ação)
  - Como obter Cognito User Pool ID
  - Como obter Cognito Client ID
  - Como construir connection string do RDS (com todos os componentes)
- Atualizar `docs/VARIAVEIS_AMBIENTE.md` se necessário (adicionar informações sobre configuração via Terraform)
- Adicionar exemplos de comandos terraform plan/apply com todos os parâmetros
- Adicionar seção de troubleshooting para problemas comuns

## Como testar
- Ler a documentação completa e verificar que está clara e completa
- Verificar que todos os parâmetros estão documentados
- Verificar que exemplos de valores estão corretos
- Verificar que instruções de como obter valores estão corretas
- Verificar que não há informações sensíveis expostas
- Testar seguir as instruções para configurar um deploy de teste

## Critérios de aceite
- Arquivo `docs/DEPLOY_LAMBDA.md` atualizado com todas as novas seções
- Seção "Configuração de VPC e Security Group" documentada completamente (incluindo descoberta automática de subnets)
- Seção "Configuração do Cognito" documentada completamente
- Seção "Configuração do RDS" documentada completamente
- Seção "Configuração JWT" documentada (se necessário)
- Seção "Secrets do GitHub" atualizada com todos os novos secrets
- Seção "Variáveis Terraform" atualizada com todas as novas variáveis
- Seção "Como obter valores dos recursos existentes" criada com instruções claras
- Exemplos de valores fornecidos para todos os parâmetros
- Exemplos de comandos terraform plan/apply atualizados
- Seção de troubleshooting adicionada (se necessário)
- `docs/VARIAVEIS_AMBIENTE.md` atualizado (se necessário)
- Documentação está clara, completa e fácil de seguir
- Não há informações sensíveis expostas na documentação

