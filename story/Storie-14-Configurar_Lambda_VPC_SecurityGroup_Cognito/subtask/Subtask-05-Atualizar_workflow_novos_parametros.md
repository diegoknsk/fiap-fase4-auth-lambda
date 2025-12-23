# Subtask 05: Atualizar workflow GitHub Actions com novos parâmetros

## Descrição
Atualizar o workflow `.github/workflows/deploy-lambda.yml` para receber e passar os novos parâmetros (Security Group, Cognito, RDS, VPC, JWT) para o Terraform durante o deploy.

## Passos de implementação
- Abrir arquivo `.github/workflows/deploy-lambda.yml`
- Adicionar novos secrets na seção de documentação/comentários:
  - `LAMBDA_SECURITY_GROUP_NAME` (opcional, default: "fiap-fase4-auth-sg")
  - `LAMBDA_SECURITY_GROUP_ID` (opcional, tem prioridade sobre nome)
  - `COGNITO_USER_POOL_ID` (obrigatório)
  - `COGNITO_REGION` (obrigatório)
  - `COGNITO_CLIENT_ID` (obrigatório)
  - `RDS_CONNECTION_STRING` (obrigatório, sensitive) - Connection string completa do PostgreSQL
  - `JWT_SECRET` (obrigatório, sensitive)
  - `JWT_ISSUER` (obrigatório)
  - `JWT_AUDIENCE` (obrigatório)
  - `JWT_EXPIRATION_HOURS` (opcional, default: 24)
- Atualizar step "Terraform Plan" para passar novos parâmetros:
  - Adicionar `-var` para cada nova variável
  - Usar `${{ secrets.VAR_NAME }}` para secrets
  - Tratar valores opcionais (usar default se não fornecido)
- Atualizar step "Terraform Apply" para passar novos parâmetros:
  - Mesma lógica do plan
- Adicionar validação de parâmetros obrigatórios antes do terraform plan/apply
- Nota: Subnets são descobertas automaticamente, não requer parâmetro
- Considerar criar um step separado para validar todos os parâmetros antes do terraform

## Como testar
- Executar workflow manualmente (workflow_dispatch) com todos os secrets configurados
- Verificar que terraform plan recebe todos os parâmetros corretamente
- Verificar que terraform apply recebe todos os parâmetros corretamente
- Testar com parâmetros opcionais não fornecidos (deve usar defaults)
- Verificar logs do workflow para garantir que parâmetros sensíveis não são expostos
- Testar com `VPC_SUBNET_IDS` em diferentes formatos (JSON array, string separada por vírgulas)

## Critérios de aceite
- Arquivo `.github/workflows/deploy-lambda.yml` atualizado
- Novos secrets documentados na seção apropriada
- Step "Terraform Plan" atualizado com novos parâmetros `-var`
- Step "Terraform Apply" atualizado com novos parâmetros `-var`
- Validação de parâmetros obrigatórios implementada
- Tratamento de parâmetros opcionais implementado (usar defaults)
- Parâmetros sensíveis não são expostos em logs
- Nota: Subnets não requerem parâmetro (descobertas automaticamente)
- Workflow executa sem erros com todos os secrets configurados
- Terraform recebe todos os parâmetros corretamente
- Workflow funciona com parâmetros opcionais não fornecidos

