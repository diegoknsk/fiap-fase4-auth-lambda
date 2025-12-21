# FastFood Auth - Lambda API

API Lambda para autenticação do sistema FastFood.

## Configuração

### Connection String

A connection string é carregada automaticamente pelo ASP.NET Core seguindo esta ordem de prioridade:

1. **Variáveis de Ambiente** (Recomendado para produção)
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

### Configuração do AWS Cognito

As configurações do Cognito são carregadas automaticamente seguindo esta ordem de prioridade:

1. **Variáveis de Ambiente** (Recomendado para produção)
   ```bash
   # Windows PowerShell
   $env:COGNITO__REGION="us-east-1"
   $env:COGNITO__USERPOOLID="us-east-1_XXXXXXXXX"
   $env:COGNITO__CLIENTID="xxxxxxxxxxxxxxxxxxxxxxxxxx"
   
   # Linux/Mac
   export COGNITO__REGION="us-east-1"
   export COGNITO__USERPOOLID="us-east-1_XXXXXXXXX"
   export COGNITO__CLIENTID="xxxxxxxxxxxxxxxxxxxxxxxxxx"
   ```

2. **appsettings.Development.json** (Para desenvolvimento local)
   ```json
   {
     "Cognito": {
       "Region": "us-east-1",
       "UserPoolId": "us-east-1_XXXXXXXXX",
       "ClientId": "xxxxxxxxxxxxxxxxxxxxxxxxxx"
     }
   }
   ```

3. **appsettings.json** (Apenas estrutura, não deve conter valores reais)

### Credenciais AWS

Para autenticar com o Cognito, o serviço precisa de credenciais AWS válidas. A ordem de prioridade é:

1. **Variáveis de Ambiente** (Para desenvolvimento local)
   ```bash
   # Windows PowerShell
   $env:AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
   $env:AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
   # Para credenciais temporárias (AWS Academy, STS, etc.)
   $env:AWS_SESSION_TOKEN="IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG..."
   
   # Linux/Mac
   export AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
   export AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
   # Para credenciais temporárias (AWS Academy, STS, etc.)
   export AWS_SESSION_TOKEN="IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG..."
   ```

2. **appsettings.Development.json** (Para desenvolvimento local)
   ```json
   {
     "AWS": {
       "AccessKeyId": "AKIAIOSFODNN7EXAMPLE",
       "SecretAccessKey": "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
       "SessionToken": "IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG..."
     }
   }
   ```
   ⚠️ **Para AWS Academy**: As credenciais da AWS Academy são temporárias e **sempre requerem SessionToken**. Certifique-se de incluir o `SessionToken` no `appsettings.Development.json`.

3. **IAM Role** (Para Lambda/EC2 em produção)
   - Quando executado em Lambda ou EC2, o SDK automaticamente usa a IAM Role associada
   - Não é necessário configurar credenciais explícitas neste caso

⚠️ **IMPORTANTE**: 
- Em **desenvolvimento local**, você DEVE configurar `AWS_ACCESS_KEY_ID` e `AWS_SECRET_ACCESS_KEY`
- **Para AWS Academy ou credenciais temporárias**, você TAMBÉM precisa configurar `AWS_SESSION_TOKEN` (ou `SessionToken` no appsettings)
- Em **produção (Lambda)**, use IAM Role com permissões adequadas para o Cognito
- Se você receber o erro "The security token included in the request is expired", suas credenciais AWS expiraram e precisam ser renovadas (no caso da AWS Academy, baixe novas credenciais)

### Configuração JWT

⚠️ **IMPORTANTE DE SEGURANÇA**: O JWT Secret **DEVE** ser fornecido exclusivamente via variável de ambiente por questões de segurança (Sonar S4790).

1. **Variável de Ambiente** (Obrigatório)
   ```bash
   # Windows PowerShell
   $env:JwtSettings_Secret="sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256"
   
   # Linux/Mac
   export JwtSettings_Secret="sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256"
   ```
   - ⚠️ **Obrigatório**: Mínimo de 32 caracteres
   - ⚠️ **Nunca** coloque o secret em arquivos de configuração versionados
   - Em **produção (Lambda/K8s)**, configure via variáveis de ambiente ou Secrets Manager

2. **Outras configurações JWT** (Issuer, Audience, ExpirationHours)
   - Podem ser configuradas via `appsettings.json` ou `appsettings.Development.json`
   - Valores padrão: Issuer="FastFood.Auth", Audience="FastFood.API", ExpirationHours=24

3. **Desenvolvimento Local**
   - O `launchSettings.json` (não versionado) pode conter `JwtSettings_Secret` para facilitar o desenvolvimento
   - ⚠️ Este arquivo está no `.gitignore` e não será commitado

## Segurança

⚠️ **IMPORTANTE**: Nunca commite arquivos com credenciais reais!

- `appsettings.Development.json` está no `.gitignore`
- Use variáveis de ambiente em produção
- Use AWS Secrets Manager ou similar para gerenciar credenciais em produção









