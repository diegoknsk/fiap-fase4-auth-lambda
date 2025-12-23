# Variáveis de Ambiente - FastFood Auth

Este documento descreve todas as variáveis de ambiente necessárias para executar o projeto FastFood Auth.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Ordem de Prioridade de Configuração](#ordem-de-prioridade-de-configuração)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
  - [Banco de Dados PostgreSQL](#banco-de-dados-postgresql)
  - [AWS Cognito](#aws-cognito)
  - [Credenciais AWS](#credenciais-aws)
  - [JWT Settings](#jwt-settings)
- [Configuração por Ambiente](#configuração-por-ambiente)
  - [Desenvolvimento Local](#desenvolvimento-local)
  - [Produção (Lambda)](#produção-lambda)
- [Exemplos Práticos](#exemplos-práticos)

---

## Visão Geral

O projeto FastFood Auth utiliza variáveis de ambiente para configuração, seguindo as melhores práticas de segurança. As variáveis de ambiente têm **prioridade máxima** sobre arquivos de configuração (`appsettings.json` e `appsettings.Development.json`).

---

## Ordem de Prioridade de Configuração

O ASP.NET Core carrega as configurações na seguinte ordem (maior prioridade primeiro):

1. **Variáveis de Ambiente** ⭐ (Recomendado para produção)
2. `appsettings.Development.json` (Apenas desenvolvimento local)
3. `appsettings.json` (Estrutura apenas, sem credenciais)

---

## Variáveis de Ambiente

### Banco de Dados PostgreSQL

#### `ConnectionStrings__DefaultConnection`

**Descrição:** Connection string completa para conexão com o banco de dados PostgreSQL.

**Formato:**
```
Host=<hostname>;Port=<porta>;Database=<nome-do-banco>;Username=<usuario>;Password=<senha>
```

**Exemplo:**
```
Host=fastfood-auth-db.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenhaSegura123
```

**Componentes:**
- `Host`: Endereço do servidor PostgreSQL (ex: RDS endpoint)
- `Port`: Porta do PostgreSQL (geralmente `5432`)
- `Database`: Nome do banco de dados
- `Username`: Usuário do banco de dados
- `Password`: Senha do banco de dados

**Obrigatória:** ✅ Sim (para Lambda e Migrator)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Execução do Migrator
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

### AWS Cognito

#### `COGNITO__REGION`

**Descrição:** Região AWS onde o User Pool do Cognito está configurado.

**Exemplo:**
```
us-east-1
```

**Valores comuns:**
- `us-east-1` (Norte da Virgínia)
- `us-east-2` (Ohio)
- `sa-east-1` (São Paulo)
- `us-west-1` (Norte da Califórnia)
- `us-west-2` (Oregon)

**Obrigatória:** ✅ Sim (para autenticação de administradores)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

#### `COGNITO__USERPOOLID`

**Descrição:** ID do User Pool do AWS Cognito onde os administradores estão cadastrados.

**Formato:**
```
us-east-1_XXXXXXXXX
```

**Exemplo:**
```
us-east-1_AbCdEfGhIj
```

**Onde encontrar:**
1. Acesse o AWS Console
2. Navegue até **Amazon Cognito** > **User pools**
3. Selecione seu User Pool
4. O ID aparece no topo da página ou nas configurações

**Obrigatória:** ✅ Sim (para autenticação de administradores)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

#### `COGNITO__CLIENTID`

**Descrição:** Client ID do aplicativo configurado no User Pool do Cognito.

**Formato:**
```
xxxxxxxxxxxxxxxxxxxxxxxxxx
```

**Exemplo:**
```
1b6gctiq6b27pjh53b0qdnudjl
```

**Onde encontrar:**
1. Acesse o AWS Console
2. Navegue até **Amazon Cognito** > **User pools**
3. Selecione seu User Pool
4. Vá em **App integration** > **App clients**
5. Copie o **Client ID**

**Obrigatória:** ✅ Sim (para autenticação de administradores)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

### Credenciais AWS

> ⚠️ **IMPORTANTE:** Em produção (Lambda), use **IAM Role** ao invés de credenciais explícitas. Essas variáveis são apenas para desenvolvimento local.

#### `AWS_ACCESS_KEY_ID`

**Descrição:** Access Key ID das credenciais AWS para autenticação com o Cognito.

**Formato:**
```
AKIAIOSFODNN7EXAMPLE
```

**Exemplo:**
```
AKIAIOSFODNN7EXAMPLE
```

**Obrigatória:** ⚠️ Apenas para desenvolvimento local

**Quando usar:**
- ✅ Desenvolvimento local (quando não há IAM Role disponível)
- ❌ **NÃO use em produção** (use IAM Role)

**Segurança:**
- ⚠️ Nunca commite essas credenciais
- ⚠️ Use AWS Secrets Manager em produção
- ⚠️ Para AWS Academy, use credenciais temporárias com `AWS_SESSION_TOKEN`

---

#### `AWS_SECRET_ACCESS_KEY`

**Descrição:** Secret Access Key das credenciais AWS para autenticação com o Cognito.

**Formato:**
```
wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
```

**Exemplo:**
```
wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
```

**Obrigatória:** ⚠️ Apenas para desenvolvimento local (quando `AWS_ACCESS_KEY_ID` está configurado)

**Quando usar:**
- ✅ Desenvolvimento local (quando não há IAM Role disponível)
- ❌ **NÃO use em produção** (use IAM Role)

**Segurança:**
- ⚠️ Nunca commite essas credenciais
- ⚠️ Use AWS Secrets Manager em produção
- ⚠️ Para AWS Academy, use credenciais temporárias com `AWS_SESSION_TOKEN`

---

#### `AWS_SESSION_TOKEN`

**Descrição:** Token de sessão para credenciais AWS temporárias (AWS Academy, STS, etc.).

**Formato:**
```
IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG/XBu2Mxp+0GcUapF9Q3ncWSUTbmOljzMPxs+4uFC3AiEAkcgTZFBUX1d57hVdbhpkPd6ojX+LwnAEIWUoROsUb94quQIIyf//////////ARAAGgwwNTgyNjQzNDc0MTMiDEJvjTPllpZ9cNx6bSqNAhdu6Rz9hJ+bOZyHiWVlSl4URHZ5t5I5VwMd2Oi5sBLyQjdM2Bz9s1tlmVqsnm6p2xttxcr59U6efa4BiRbFr0f/0rNXkDQT4XAUQAyIXc89NeyvCLXKtbXLAOi8432LmjuDvTw02V28RiY/7Qm8nxXhZ1+YaYz664Ebr66VOaO/eovm7jevgA5v9Mn9AmwzuRZ1v1UsOgAuZ/0sPAu9vg3v9u25/E4UvTPsyonJHHO5tNy1INihkdETBQ6Gn4l1YRJRNrzJVVoXv6uyaFqqY1CAw6pBqJ7Bv8cC8R6trDH2KIjvpikM+4jXgfJ6FpgdvnMeVVXchsRytAbqC+hhaGEhyn2xUlMhBgVSUIhUMNmBm8oGOp0Bow8BQTEK+rMxAgtHN7ClBEzGD2msET2N8WoKN/HfRs2ZBliomeWsn/Rd8AhQ55jq9OSfhZ0ZXYGR5wAw2ML7djE96z5Vi38UqCqRAJWhKOhLDtwYjoKLTKepp4nc20+0gscvZx8TqpldlmLyaOt3ZDao3qh2+I7wq2nWY0wIUWbTVjGDe6To7uq8a1fd4+x31x2pY0eV8UvYHWxjmg==
```

**Obrigatória:** ⚠️ Apenas para credenciais temporárias (AWS Academy, STS)

**Quando usar:**
- ✅ **Obrigatório** para AWS Academy
- ✅ Credenciais temporárias via STS
- ❌ Não necessário para credenciais permanentes
- ❌ **NÃO use em produção** (use IAM Role)

**Importante:**
- ⚠️ Credenciais temporárias expiram (geralmente após algumas horas)
- ⚠️ Se receber erro "The security token included in the request is expired", renove as credenciais
- ⚠️ Para AWS Academy, baixe novas credenciais quando expirarem

---

### JWT Settings

> **Nota:** Essas variáveis são opcionais se você usar `appsettings.json` ou `appsettings.Development.json`. O ASP.NET Core automaticamente lê configurações hierárquicas via variáveis de ambiente usando `__` (dois underscores) como separador.

#### `JwtSettings__Secret`

**Descrição:** Chave secreta para assinar tokens JWT. Deve ter no mínimo 32 caracteres.

**Formato:**
```
sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256
```

**Exemplo:**
```
MySuperSecretKeyThatIsAtLeast32CharactersLongForHMACSHA256
```

**Requisitos:**
- ✅ Mínimo de 32 caracteres
- ✅ Use uma chave forte e aleatória
- ⚠️ Nunca commite essa chave

**Obrigatória:** ✅ Sim (para geração de tokens JWT para clientes)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

#### `JwtSettings__Issuer`

**Descrição:** Nome do emissor do token JWT (quem emitiu o token).

**Exemplo:**
```
FastFood.Auth
```

**Obrigatória:** ✅ Sim (para geração de tokens JWT)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

#### `JwtSettings__Audience`

**Descrição:** Audiência do token JWT (quem deve aceitar o token).

**Exemplo:**
```
FastFood.API
```

**Obrigatória:** ✅ Sim (para geração de tokens JWT)

**Quando usar:**
- ✅ Produção (Lambda)
- ✅ Desenvolvimento local (se não usar `appsettings.Development.json`)

---

#### `JwtSettings__ExpirationHours`

**Descrição:** Tempo de expiração do token JWT em horas.

**Exemplo:**
```
24
```

**Valor padrão:** `24` (se não especificado)

**Obrigatória:** ❌ Não (usa valor padrão se não especificado)

**Quando usar:**
- ⚠️ Opcional (usa padrão de 24 horas)

---

## Configuração por Ambiente

### Desenvolvimento Local

Para desenvolvimento local, você pode usar **variáveis de ambiente** ou **arquivos de configuração** (`appsettings.Development.json`).

#### Opção 1: Variáveis de Ambiente (PowerShell)

```powershell
# Banco de Dados
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=dbAuth;Username=postgres;Password=postgres"

# AWS Cognito
$env:COGNITO__REGION="us-east-1"
$env:COGNITO__USERPOOLID="us-east-1_XXXXXXXXX"
$env:COGNITO__CLIENTID="xxxxxxxxxxxxxxxxxxxxxxxxxx"

# Credenciais AWS (para desenvolvimento local)
$env:AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
$env:AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
# Para AWS Academy ou credenciais temporárias:
$env:AWS_SESSION_TOKEN="IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG..."

# JWT Settings
$env:JwtSettings__Secret="sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256"
$env:JwtSettings__Issuer="FastFood.Auth"
$env:JwtSettings__Audience="FastFood.API"
$env:JwtSettings__ExpirationHours="24"
```

#### Opção 2: Variáveis de Ambiente (Linux/Mac)

```bash
# Banco de Dados
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=dbAuth;Username=postgres;Password=postgres"

# AWS Cognito
export COGNITO__REGION="us-east-1"
export COGNITO__USERPOOLID="us-east-1_XXXXXXXXX"
export COGNITO__CLIENTID="xxxxxxxxxxxxxxxxxxxxxxxxxx"

# Credenciais AWS (para desenvolvimento local)
export AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
export AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
# Para AWS Academy ou credenciais temporárias:
export AWS_SESSION_TOKEN="IQoJb3JpZ2luX2VjEAAaCXVzLXdlc3QtMiJHMEUCIBG..."

# JWT Settings
export JwtSettings__Secret="sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256"
export JwtSettings__Issuer="FastFood.Auth"
export JwtSettings__Audience="FastFood.API"
export JwtSettings__ExpirationHours="24"
```

#### Opção 3: Arquivo appsettings.Development.json

Copie `appsettings.Development.json.example` para `appsettings.Development.json` e preencha com suas credenciais:

```bash
# Windows PowerShell
Copy-Item src/FastFood.Auth.Lambda/appsettings.Development.json.example src/FastFood.Auth.Lambda/appsettings.Development.json

# Linux/Mac
cp src/FastFood.Auth.Lambda/appsettings.Development.json.example src/FastFood.Auth.Lambda/appsettings.Development.json
```

⚠️ **Importante:** O arquivo `appsettings.Development.json` está no `.gitignore` e não será commitado.

---

### Produção (Lambda)

Em produção, use **variáveis de ambiente** configuradas no Lambda via **Terraform** ou **AWS Secrets Manager**.

**Nota:** O Terraform configura automaticamente todas as variáveis de ambiente necessárias no Lambda durante o deploy. As variáveis são passadas via GitHub Secrets e configuradas no recurso `aws_lambda_function` através do bloco `environment`.

#### Variáveis de Ambiente no Lambda

Configure as variáveis de ambiente no AWS Lambda Console ou via Infrastructure as Code (Terraform, CloudFormation, etc.):

**Variáveis obrigatórias:**
- `ConnectionStrings__DefaultConnection`
- `COGNITO__REGION`
- `COGNITO__USERPOOLID`
- `COGNITO__CLIENTID`
- `JwtSettings__Secret`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`

**Variáveis opcionais:**
- `JwtSettings__ExpirationHours` (padrão: 24)

**⚠️ NÃO configure:**
- `AWS_ACCESS_KEY_ID`
- `AWS_SECRET_ACCESS_KEY`
- `AWS_SESSION_TOKEN`

**Por quê?** O Lambda usa automaticamente a **IAM Role** associada à função, que é mais segura e recomendada.

#### IAM Role para Lambda

A IAM Role da Lambda deve ter as seguintes permissões:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "cognito-idp:AdminInitiateAuth",
        "cognito-idp:AdminGetUser"
      ],
      "Resource": "arn:aws:cognito-idp:us-east-1:ACCOUNT_ID:userpool/us-east-1_XXXXXXXXX"
    }
  ]
}
```

---

## Exemplos Práticos

### Executar Migrator com Variáveis de Ambiente

```powershell
# Windows PowerShell
$env:ConnectionStrings__DefaultConnection="Host=meu-rds.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenha123"
dotnet run --project src/FastFood.Auth.Migrator
```

```bash
# Linux/Mac
export ConnectionStrings__DefaultConnection="Host=meu-rds.xxxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=dbAuth;Username=dbadmin;Password=MinhaSenha123"
dotnet run --project src/FastFood.Auth.Migrator
```

### Executar Lambda Localmente com Variáveis de Ambiente

```powershell
# Windows PowerShell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=dbAuth;Username=postgres;Password=postgres"
$env:COGNITO__REGION="us-east-1"
$env:COGNITO__USERPOOLID="us-east-1_XXXXXXXXX"
$env:COGNITO__CLIENTID="xxxxxxxxxxxxxxxxxxxxxxxxxx"
$env:AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
$env:AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
$env:JwtSettings__Secret="sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256"
$env:JwtSettings__Issuer="FastFood.Auth"
$env:JwtSettings__Audience="FastFood.API"
dotnet run --project src/FastFood.Auth.Lambda
```

### Criar arquivo .env (para uso com ferramentas como Docker)

```bash
# .env (não commitar este arquivo!)
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=dbAuth;Username=postgres;Password=postgres
COGNITO__REGION=us-east-1
COGNITO__USERPOOLID=us-east-1_XXXXXXXXX
COGNITO__CLIENTID=xxxxxxxxxxxxxxxxxxxxxxxxxx
AWS_ACCESS_KEY_ID=AKIAIOSFODNN7EXAMPLE
AWS_SECRET_ACCESS_KEY=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
JwtSettings__Secret=sua-chave-secreta-minimo-32-caracteres-para-hmac-sha256
JwtSettings__Issuer=FastFood.Auth
JwtSettings__Audience=FastFood.API
JwtSettings__ExpirationHours=24
```

---

## Resumo de Variáveis

| Variável | Obrigatória | Ambiente | Descrição |
|----------|-------------|----------|-----------|
| `ConnectionStrings__DefaultConnection` | ✅ | Todos | Connection string do PostgreSQL |
| `COGNITO__REGION` | ✅ | Todos | Região do Cognito |
| `COGNITO__USERPOOLID` | ✅ | Todos | ID do User Pool |
| `COGNITO__CLIENTID` | ✅ | Todos | Client ID do Cognito |
| `AWS_ACCESS_KEY_ID` | ⚠️ | Dev Local | Access Key AWS (não usar em produção) |
| `AWS_SECRET_ACCESS_KEY` | ⚠️ | Dev Local | Secret Key AWS (não usar em produção) |
| `AWS_SESSION_TOKEN` | ⚠️ | Dev Local | Token de sessão (credenciais temporárias) |
| `JwtSettings__Secret` | ✅ | Todos | Chave secreta JWT (min. 32 caracteres) |
| `JwtSettings__Issuer` | ✅ | Todos | Emissor do token JWT |
| `JwtSettings__Audience` | ✅ | Todos | Audiência do token JWT |
| `JwtSettings__ExpirationHours` | ❌ | Todos | Expiração em horas (padrão: 24) |

---

## Segurança

⚠️ **IMPORTANTE:**

1. **Nunca commite credenciais** em arquivos de código ou configuração
2. **Use variáveis de ambiente** em produção
3. **Use IAM Role** no Lambda (não credenciais explícitas)
4. **Use AWS Secrets Manager** para gerenciar credenciais sensíveis
5. **Renove credenciais temporárias** quando expirarem (AWS Academy)
6. **Use chaves JWT fortes** (mínimo 32 caracteres, aleatórias)

---

## Troubleshooting

### Erro: "Connection string 'DefaultConnection' não encontrada"

**Solução:** Configure a variável `ConnectionStrings__DefaultConnection` ou crie o arquivo `appsettings.Development.json`.

### Erro: "Cognito Region não configurado"

**Solução:** Configure a variável `COGNITO__REGION` ou adicione no `appsettings.Development.json`.

### Erro: "The security token included in the request is expired"

**Solução:** Suas credenciais AWS expiraram. Renove as credenciais (para AWS Academy, baixe novas credenciais).

### Erro: "JWT Secret não configurado"

**Solução:** Configure a variável `JwtSettings__Secret` ou adicione no `appsettings.Development.json`.

---

## Referências

- [AWS Cognito Documentation](https://docs.aws.amazon.com/cognito/)
- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [AWS Lambda Environment Variables](https://docs.aws.amazon.com/lambda/latest/dg/configuration-envvars.html)

