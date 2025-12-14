# Subtask 07: Criar workflow GitHub Actions para executar migrations

## Descrição
Criar workflow do GitHub Actions que executa as migrations do Entity Framework Core no banco de dados PostgreSQL de forma automatizada durante o processo de CI/CD.

## Passos de implementação
- Criar diretório `.github/workflows/` na raiz do repositório se não existir
- Criar arquivo `.github/workflows/migrate-database.yml`
- Configurar workflow com:
  - Trigger: `on: workflow_dispatch` (execução manual) e opcionalmente `on: push: branches: [main]` (após merge)
  - Job `migrate` que:
    - Usa runner: `runs-on: ubuntu-latest`
    - Faz checkout do código: `actions/checkout@v4`
    - Configura .NET: `actions/setup-dotnet@v4` com versão `8.0.x`
    - Configura AWS credentials: `aws-actions/configure-aws-credentials@v4` (usar secrets do GitHub)
    - Restaura dependências: `dotnet restore`
    - Executa migration: `dotnet ef database update --project src/FastFood.Auth.Infra.Persistence --startup-project src/FastFood.Auth.Lambda --connection "${{ secrets.DB_CONNECTION_STRING }}"`
- Adicionar secret `DB_CONNECTION_STRING` no GitHub (Settings > Secrets and variables > Actions)
- Documentar no README ou em arquivo separado como configurar o secret
- Adicionar passo de validação antes de aplicar: `dotnet ef migrations list` para listar migrations pendentes

## Como testar
- Fazer commit e push do workflow
- Verificar que o workflow aparece em Actions no GitHub
- Executar workflow manualmente via `workflow_dispatch`
- Verificar logs do workflow para confirmar que migration foi aplicada
- Conectar ao banco de dados e verificar que a tabela Customers foi criada
- Validar que o workflow falha graciosamente se connection string estiver incorreta

## Critérios de aceite
- Arquivo `.github/workflows/migrate-database.yml` criado
- Workflow configurado com trigger `workflow_dispatch`
- Workflow usa `actions/setup-dotnet@v4` para configurar .NET 8
- Workflow executa `dotnet ef database update` com connection string do secret
- Secret `DB_CONNECTION_STRING` documentado (não commitado)
- Workflow lista migrations antes de aplicar (validação)
- Workflow pode ser executado manualmente via GitHub Actions UI
- Logs do workflow mostram execução bem-sucedida
- Tabela Customers criada no banco após execução do workflow
- Workflow falha graciosamente em caso de erro (connection string inválida, etc.)

