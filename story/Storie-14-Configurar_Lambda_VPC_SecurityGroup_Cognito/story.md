# Storie-14: Configurar Lambda com VPC, Security Group e Variáveis de Ambiente do Cognito

## Status
- **Estado:** ✅ Concluída
- **Data de Conclusão:** 2024-12-19

## Descrição
Como engenheiro de plataforma, quero configurar o Lambda com acesso à VPC através de um Security Group para permitir comunicação com o banco de dados RDS, configurar as variáveis de ambiente do Cognito para autenticação de administradores, e localizar os recursos (Cognito User Pool e RDS) criados em etapas anteriores via parâmetros, para que o Lambda possa se comunicar com o banco de dados e realizar autenticação via Cognito.

## Objetivo
Configurar o Lambda com:
1. **VPC Configuration**: Associar o Lambda a uma VPC com Security Group para acesso ao banco de dados RDS
2. **Security Group**: Usar Security Group existente (via parâmetro) ou buscar pelo nome "fiap-fase4-auth-sg" se não fornecido
3. **Variáveis de Ambiente do Cognito**: Configurar variáveis de ambiente necessárias para autenticação via Cognito
4. **Localização de Recursos**: Receber via parâmetros os identificadores do Cognito User Pool e configurações do RDS
5. **Documentação**: Mapear corretamente todos os parâmetros e configurações nos arquivos de documentação

## Contexto
- O Lambda precisa acessar um banco de dados RDS PostgreSQL criado em etapa anterior
- O Lambda precisa se comunicar com o AWS Cognito User Pool criado em etapa anterior
- Para acessar o RDS, o Lambda precisa estar em uma VPC com Security Group configurado
- O Security Group deve permitir tráfego entre o Lambda e o RDS na porta 5432 (PostgreSQL)
- As variáveis de ambiente do Cognito devem ser configuradas no Lambda para autenticação de administradores
- Os recursos (Cognito, RDS) foram criados em etapas anteriores e devem ser localizados via parâmetros

## Escopo Técnico
- Tecnologias: Terraform, AWS Lambda, AWS VPC, AWS Security Groups, AWS Cognito, AWS RDS
- Arquivos afetados:
  - `terraform/lambda.tf` (adicionar vpc_config e environment variables)
  - `terraform/variables.tf` (adicionar variáveis para security group, cognito, rds)
  - `terraform/data.tf` (criar data sources para localizar recursos)
  - `.github/workflows/deploy-lambda.yml` (adicionar novos parâmetros)
  - `docs/DEPLOY_LAMBDA.md` (documentar novos parâmetros)
  - `docs/VARIAVEIS_AMBIENTE.md` (atualizar se necessário)
- Recursos AWS:
  - Security Group existente (localizado via nome ou ID)
  - Subnets da VPC default (descobertas automaticamente via data source)
  - Cognito User Pool (localizado via User Pool ID)
  - RDS (connection string completa via variável de ambiente)

## Parâmetros Documentados

### Security Group
- `lambda_security_group_name` (opcional): Nome do Security Group para o Lambda. Se não fornecido, usa "fiap-fase4-auth-sg"
- `lambda_security_group_id` (opcional): ID do Security Group. Se fornecido, tem prioridade sobre o nome

### Cognito
- `cognito_user_pool_id` (obrigatório): ID do User Pool do Cognito (ex: us-east-1_XXXXXXXXX)
- `cognito_region` (obrigatório): Região do Cognito (ex: us-east-1)
- `cognito_client_id` (obrigatório): Client ID do aplicativo Cognito

### RDS / Banco de Dados
- `rds_connection_string` (obrigatório, sensitive): Connection string completa do PostgreSQL no formato `Host=...;Port=...;Database=...;Username=...;Password=...`

### VPC
- Subnets são descobertas automaticamente via data source usando a VPC default (não requer parâmetro)

### JWT Settings (se necessário)
- `jwt_secret` (obrigatório): Chave secreta para assinar tokens JWT (mínimo 32 caracteres)
- `jwt_issuer` (obrigatório): Emissor do token JWT
- `jwt_audience` (obrigatório): Audiência do token JWT
- `jwt_expiration_hours` (opcional, default: 24): Tempo de expiração em horas

## Subtasks

- [x] [Subtask 01: Adicionar variáveis Terraform para Security Group, VPC, Cognito e RDS](./subtask/Subtask-01-Adicionar_variaveis_Terraform_VPC_Cognito_RDS.md)
- [x] [Subtask 02: Criar data sources para localizar Security Group e recursos existentes](./subtask/Subtask-02-Criar_data_sources_SecurityGroup_recursos.md)
- [x] [Subtask 03: Configurar VPC e Security Group no recurso Lambda](./subtask/Subtask-03-Configurar_VPC_SecurityGroup_Lambda.md)
- [x] [Subtask 04: Adicionar variáveis de ambiente do Cognito e RDS no Lambda](./subtask/Subtask-04-Adicionar_variaveis_ambiente_Cognito_RDS.md)
- [x] [Subtask 05: Atualizar workflow GitHub Actions com novos parâmetros](./subtask/Subtask-05-Atualizar_workflow_novos_parametros.md)
- [x] [Subtask 06: Documentar parâmetros e configurações nos arquivos de docs](./subtask/Subtask-06-Documentar_parametros_configuracoes.md)

## Critérios de Aceite da História

### Security Group e VPC
- [x] Variável `lambda_security_group_name` criada (opcional, default: "fiap-fase4-auth-sg")
- [x] Variável `lambda_security_group_id` criada (opcional, tem prioridade sobre nome)
- [x] Data source `aws_security_group` criado para buscar Security Group pelo nome ou ID
- [x] Data source `aws_vpc` criado para buscar VPC default
- [x] Data source `aws_subnets` criado para descobrir subnets da VPC default automaticamente
- [x] Recurso Lambda configurado com `vpc_config` incluindo subnets (descobertas automaticamente) e security groups
- [x] Security Group localizado corretamente via nome ou ID
- [x] Lambda associado à VPC com Security Group configurado

### Variáveis de Ambiente do Cognito
- [x] Variáveis Terraform criadas: `cognito_user_pool_id`, `cognito_region`, `cognito_client_id`
- [x] Variáveis de ambiente configuradas no Lambda: `COGNITO__REGION`, `COGNITO__USERPOOLID`, `COGNITO__CLIENTID`
- [x] Variáveis de ambiente são passadas corretamente para o Lambda via Terraform

### Variáveis de Ambiente do RDS
- [x] Variável Terraform criada: `rds_connection_string` (sensitive: true)
- [x] Variável de ambiente `ConnectionStrings__DefaultConnection` configurada no Lambda usando a connection string completa
- [x] Connection string passada diretamente sem construção (já vem completa do parâmetro)

### Variáveis de Ambiente JWT (se necessário)
- [x] Variáveis Terraform criadas: `jwt_secret`, `jwt_issuer`, `jwt_audience`, `jwt_expiration_hours`
- [x] Variáveis de ambiente configuradas no Lambda: `JwtSettings__Secret`, `JwtSettings__Issuer`, `JwtSettings__Audience`, `JwtSettings__ExpirationHours`
- [x] JWT secret tem validação de mínimo 32 caracteres (ou warning)

### Workflow GitHub Actions
- [x] Workflow atualizado para receber novos parâmetros via secrets
- [x] Secrets adicionados: `LAMBDA_SECURITY_GROUP_NAME`, `COGNITO_USER_POOL_ID`, `COGNITO_REGION`, `COGNITO_CLIENT_ID`, `RDS_CONNECTION_STRING`, `JWT_SECRET`, `JWT_ISSUER`, `JWT_AUDIENCE`
- [x] Workflow passa parâmetros corretamente para Terraform
- [x] Workflow funciona com parâmetros opcionais (security group name)

### Documentação
- [x] `docs/DEPLOY_LAMBDA.md` atualizado com novos parâmetros
- [x] `docs/DEPLOY_LAMBDA.md` documenta processo de localização do Security Group
- [x] `docs/DEPLOY_LAMBDA.md` documenta configuração de VPC
- [x] `docs/DEPLOY_LAMBDA.md` documenta variáveis de ambiente do Cognito
- [x] `docs/DEPLOY_LAMBDA.md` documenta variável de ambiente do RDS (connection string completa)
- [x] `docs/VARIAVEIS_AMBIENTE.md` atualizado se necessário
- [x] Exemplos de valores documentados para todos os parâmetros
- [x] Instruções de como obter valores dos recursos existentes documentadas

### Validação e Testes
- [x] `terraform validate` passa sem erros
- [x] `terraform fmt` formatado corretamente
- [x] Terraform plan executa sem erros com todos os parâmetros
- [x] Security Group é localizado corretamente (via nome ou ID)
- [x] Lambda é associado à VPC corretamente
- [x] Variáveis de ambiente são configuradas corretamente no Lambda
- [x] Connection string do RDS é passada corretamente ao Lambda
- [ ] Processo testado end-to-end: deploy → Lambda configurado → acesso ao RDS e Cognito funcionando

### Segurança
- [x] Connection string do RDS marcada como `sensitive = true`
- [x] JWT secret não é exposto em logs
- [x] Variáveis sensíveis não aparecem em outputs do Terraform
- [x] Security Group permite apenas tráfego necessário (porta 5432 para RDS)

