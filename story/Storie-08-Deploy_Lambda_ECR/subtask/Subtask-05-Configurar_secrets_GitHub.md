# Subtask 05: Configurar secrets e variáveis no GitHub

## Descrição
Configurar secrets e variáveis necessários no GitHub para o workflow de deploy funcionar corretamente.

## Passos de implementação
- Acessar Settings > Secrets and variables > Actions no repositório GitHub
- Adicionar secrets obrigatórios:
  - `AWS_ACCESS_KEY_ID` - Access Key ID da AWS
  - `AWS_SECRET_ACCESS_KEY` - Secret Access Key da AWS
  - `AWS_REGION` - Região AWS (ex: us-east-1)
  - `AWS_ACCOUNT_ID` - Account ID da AWS (12 dígitos)
  - `ECR_REPOSITORY` - Nome completo do repositório ECR (ex: fastfood-auth-lambda)
  - `LAMBDA_FUNCTION_NAME` - Nome da função Lambda (ex: fastfood-auth-lambda)
- (Opcional) Adicionar variáveis (não secretas):
  - `LAMBDA_ALIAS` - Alias da função Lambda (ex: production)
- Documentar no README ou arquivo separado:
  - Lista de secrets necessários
  - Como obter cada secret
  - Permissões IAM necessárias para a role/usuário
- Criar arquivo `.github/SECRETS.md` com documentação (sem valores reais)

## Como testar
- Verificar que todos os secrets estão configurados
- Executar workflow e validar que não há erros de secrets não encontrados
- Verificar que workflow consegue acessar AWS e ECR

## Critérios de aceite
- Todos os secrets obrigatórios configurados no GitHub
- Secrets não aparecem em logs do workflow (proteção automática)
- Documentação criada sobre secrets necessários
- Permissões IAM documentadas
- Workflow executa sem erros de secrets faltando
- Workflow consegue acessar AWS e ECR

