# FastFood Auth - Database Migrator

Projeto console responsável por executar migrations do Entity Framework Core no banco de dados PostgreSQL.

## Configuração

### Connection String

A connection string pode ser configurada de três formas (em ordem de prioridade):

1. **Variável de Ambiente** (Recomendado para produção)
   ```bash
   # Windows PowerShell
   $env:ConnectionStrings__DefaultConnection="Host=seu-host;Port=5432;Database=dbAuth;Username=usuario;Password=senha"
   
   # Linux/Mac
   export ConnectionStrings__DefaultConnection="Host=seu-host;Port=5432;Database=dbAuth;Username=usuario;Password=senha"
   ```

2. **appsettings.Development.json** (Para desenvolvimento local)
   - Copie `appsettings.Development.json.example` para `appsettings.Development.json`
   - Preencha com suas credenciais
   - ⚠️ Este arquivo está no `.gitignore` e não será commitado

3. **appsettings.json** (Apenas estrutura, não deve conter credenciais reais)
   - Mantido vazio no repositório por segurança

### Formato da Connection String

```
Host=seu-host-rds.amazonaws.com;Port=5432;Database=dbAuth;Username=seu-usuario;Password=sua-senha
```

## Execução

### Desenvolvimento Local

```bash
# Configurar variável de ambiente
$env:ConnectionStrings__DefaultConnection="Host=host;Port=5432;Database=dbAuth;Username=user;Password=pass"

# Executar migrator
dotnet run --project src/FastFood.Auth.Migrator
```

### Produção

```bash
# Configurar variável de ambiente no ambiente de execução
# Executar o migrator
dotnet run --project src/FastFood.Auth.Migrator
```

## Segurança

⚠️ **IMPORTANTE**: Nunca commite arquivos com credenciais reais!

- `appsettings.Development.json` está no `.gitignore`
- Use variáveis de ambiente em produção
- Use AWS Secrets Manager ou similar para gerenciar credenciais em produção






