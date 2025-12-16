# Regras de Arquitetura - FastFood Auth Lambda

## Objetivo

Serviço de autenticação e identificação, publicado como **AWS Lambda**, mas estruturado como uma **API ASP.NET Core tradicional**, utilizando `Amazon.Lambda.AspNetCoreServer` para hospedar a aplicação.

A arquitetura segue **~80% Clean Architecture** (simplificação pragmática mantendo separação de camadas e inversão de dependência), com foco em mercado, simplicidade e consistência com os demais microsserviços do sistema (OrderHub, PayStream, KitchenFlow).

---

## Hosting (Lambda como API)

- O serviço deve usar ASP.NET Core normalmente.
- O hosting Lambda deve ser configurado com:

```csharp
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
```

- **Em ambiente local:**
  - A aplicação roda com Kestrel normalmente.

- **Em produção:**
  - Kestrel é substituído pelo runtime do Lambda automaticamente.

- **Regra:** a aplicação não deve conter código específico de Lambda fora do bootstrap.

---

## Swagger

- Swagger é **obrigatório** e deve estar habilitado:
  - Em ambiente local
  - Em ambiente Lambda (quando permitido pelo API Gateway)
- Endpoints devem ser documentados normalmente com XML comments.
- Swagger é obrigatório para padronização e inspeção de contrato.

---

## Estrutura de Projetos

```
FastFood.Auth.Lambda          (ASP.NET Core - API/Lambda host)
FastFood.Auth.Application     (UseCases, Ports, InputModels, OutputModels, Presenters)
FastFood.Auth.Domain          (Entidades, VOs, validações, invariantes)
FastFood.Auth.Infra           (Serviços externos: Cognito, Token, etc.)
FastFood.Auth.Infra.Persistence (PostgreSQL com EF Core)
FastFood.Auth.Tests.Unit      (Testes unitários)
FastFood.Auth.Tests.Bdd       (Testes BDD/SpecFlow)
```

---

## Regras de Dependência

```
Lambda → Application
Application → Domain
Infra.* → Application (implementa Ports)
Domain → Nenhuma dependência externa
```

**Domain NÃO referencia:**
- ASP.NET
- EF Core
- AWS SDK
- HttpClient
- Configuration
- Qualquer framework ou biblioteca externa

---

## Estrutura da Camada Application (Organização Horizontal por Contexto)

A camada Application deve ser organizada **horizontalmente por contexto** (Customer, Admin, etc.), não verticalmente por tipo:

```
FastFood.Auth.Application/
  UseCases/
    Customer/
      CreateAnonymousCustomerUseCase.cs
      RegisterCustomerUseCase.cs
      IdentifyCustomerUseCase.cs
    Admin/
      AuthenticateAdminUseCase.cs
  InputModels/
    Customer/
      CreateAnonymousCustomerInputModel.cs
      RegisterCustomerInputModel.cs
      IdentifyCustomerInputModel.cs
    Admin/
      AuthenticateAdminInputModel.cs
  OutputModels/
    Customer/
      CreateAnonymousCustomerOutputModel.cs
      RegisterCustomerOutputModel.cs
      IdentifyCustomerOutputModel.cs
    Admin/
      AuthenticateAdminOutputModel.cs
  Presenters/
    Customer/
      CreateAnonymousCustomerPresenter.cs
      RegisterCustomerPresenter.cs
      IdentifyCustomerPresenter.cs
    Admin/
      AuthenticateAdminPresenter.cs
  Ports/
    ICustomerRepository.cs
    ITokenService.cs
    ICognitoService.cs
    IMessageBus.cs (se necessário)
```

### Regras de Nomenclatura

- **UseCase**: `<Verbo><Entidade>UseCase` (ex: `CreateAnonymousCustomerUseCase`)
- **InputModel**: `<UseCaseName>InputModel` (ex: `CreateAnonymousCustomerInputModel`)
- **OutputModel**: `<UseCaseName>OutputModel` (ex: `CreateAnonymousCustomerOutputModel`)
- **Presenter**: `<UseCaseName>Presenter` (ex: `CreateAnonymousCustomerPresenter`)
- **Port**: `I<ServiceName>` (ex: `ICustomerRepository`, `ITokenService`)

---

## Fluxo Padrão (Request → Response)

```
Controller (Lambda)
  ↓ recebe RequestModel
  ↓ mapeia para InputModel
  ↓ chama UseCase.ExecuteAsync(InputModel)
UseCase (Application)
  ↓ executa lógica de negócio
  ↓ chama Ports (repositórios, serviços)
  ↓ retorna OutputModel
Presenter (Application)
  ↓ recebe OutputModel
  ↓ transforma em ResponseModel (da API)
  ↓ retorna ResponseModel
Controller (Lambda)
  ↓ recebe ResponseModel do Presenter
  ↓ retorna HTTP Response
```

### Regras Críticas do Fluxo

1. **Controller NÃO acessa DbContext direto** - sempre via UseCase
2. **UseCase NÃO recebe RequestModel da API** - recebe InputModel da Application
3. **UseCase retorna OutputModel** - nunca ResponseModel da API
4. **Presenter transforma OutputModel em ResponseModel** - adaptação quando necessário
5. **Controller usa Presenter** - não cria ResponseModel diretamente

---

## API (Controllers / Lambda)

Controllers são **adapters de transporte** (HTTP/API Gateway).

### Responsabilidades dos Controllers

- Autenticação/autorização HTTP
- Validação básica de request (ModelState)
- Mapear `RequestModel` → `InputModel`
- Chamar `UseCase.ExecuteAsync(InputModel)`
- Usar `Presenter.Present(OutputModel)` para obter `ResponseModel`
- Retornar HTTP Response

### Proibido nos Controllers

- ❌ Regra de negócio
- ❌ Acesso direto a banco (DbContext) ou SDKs
- ❌ Criar ResponseModels diretamente (deve usar Presenters)
- ❌ Criar Presenters próprios (deve usar Presenters da Application)
- ❌ Lógica de transformação complexa (deve estar no Presenter)

### Endpoints Customer

- `POST /api/customer/anonymous` - Criar customer anônimo
- `POST /api/customer/register` - Registrar customer por CPF
- `POST /api/customer/identify` - Identificar customer existente por CPF

### Endpoints Admin

- `POST /api/admin/login` - Autenticar admin via AWS Cognito

---

## Application Layer

### UseCases

- UseCases são **pequenos e focados** (uma única responsabilidade)
- Cada UseCase deve ter um objetivo claro e não deve orquestrar múltiplos fluxos complexos
- UseCases recebem **InputModels** da Application (não RequestModels da API)
- UseCases retornam **OutputModels** (não ResponseModels da API)
- UseCases chamam **Ports** (interfaces) para acesso a dados/serviços externos

### InputModels

- InputModels definem o contrato de **entrada** dos UseCases
- Devem estar na camada Application
- Representam os dados necessários para executar o UseCase
- Podem depender de tipos do Domain (Value Objects, Enums)

### OutputModels

- OutputModels definem o contrato de **saída** dos UseCases
- Devem estar na camada Application
- Representam os dados retornados pelo UseCase
- Podem depender de tipos do Domain (Value Objects, Enums)

### Presenters

- Presenters são **obrigatórios** e devem estar na camada Application
- Responsabilidade: transformar `OutputModel` em `ResponseModel` (da API)
- Por padrão, fazem mapeamento direto, mas podem fazer transformações quando necessário
- API consome os OutputModels através dos Presenters

### Ports (Interfaces)

- Ports definem contratos para acesso a dados e serviços externos
- Exemplos:
  - `ICustomerRepository` - acesso a dados de Customer
  - `ITokenService` - geração/validação de tokens JWT
  - `ICognitoService` - autenticação via AWS Cognito
  - `IMessageBus` / `IEventPublisher` - publicação de eventos (se necessário)
- Implementações concretas ficam na camada Infra

### Facade (Opcional)

- **Facade NÃO é obrigatório**
- Use Facade **apenas** quando:
  - Um endpoint precisa orquestrar **3 ou mais UseCases**
  - Há lógica de orquestração complexa entre múltiplos UseCases
- Se um endpoint chama apenas 1-2 UseCases, não use Facade
- Facade deve estar na camada Application

---

## Domain Layer

- Entidades, Value Objects, Enums e validações de domínio
- Exceções de domínio para regras inválidas
- **Nenhuma dependência externa** (zero dependências de framework)

### Entidade Customer

```csharp
- Id (Guid)
- Name (string?, nullable)
- Email (Email?, Value Object, nullable)
- Cpf (Cpf?, Value Object, nullable)
- CustomerType (CustomerTypeEnum)
- CreatedAt (DateTime)
```

### CustomerType Enum

```csharp
- Registered = 1
- Anonymous = 2
```

### Value Objects

- **Cpf**: validação de CPF brasileiro (11 dígitos, algoritmo de validação)
- **Email**: validação de formato de email

---

## Infraestrutura

### Infra (Serviços Externos)

- Implementa Ports de serviços externos
- Exemplos:
  - `TokenService` (implementa `ITokenService`) - geração JWT
  - `CognitoService` (implementa `ICognitoService`) - autenticação Cognito

### Infra.Persistence

- Implementa Ports de persistência (ex: `ICustomerRepository`)
- Usa Entity Framework Core com PostgreSQL (Npgsql)
- Entidades de banco são **separadas** das entidades de domínio
- **DbContext nunca é acessado fora da Infra.Persistence**

### Persistência - Tabela Customers

```sql
- Id (Guid, PK)
- Name (varchar 500, nullable)
- Email (varchar 255, nullable)
- Cpf (varchar 11, nullable)
- CustomerType (int) - 1 = Registered, 2 = Anonymous
- CreatedAt (datetime)
```

- Value Objects (Cpf, Email) são mapeados como strings no banco

---

## Autenticação e Tokens

### Customer Authentication

- Tokens JWT gerados para customers (identificados ou anônimos)
- Claims obrigatórias:
  - `sub`: CustomerId (Guid)
  - `customerId`: CustomerId (Guid)
  - `jti`: JWT ID (Guid)
  - `iat`: Issued At (Unix timestamp)
- Configuração via `appsettings.json`:
  - `JwtSettings:Secret`
  - `JwtSettings:Issuer`
  - `JwtSettings:Audience`
  - `JwtSettings:ExpirationHours`

### Admin Authentication

- Autenticação via AWS Cognito
- Usar `AdminInitiateAuthRequest` com `AuthFlowType.ADMIN_USER_PASSWORD_AUTH`
- Configuração via variáveis de ambiente:
  - `COGNITO__REGION`
  - `COGNITO__USERPOOLID`
  - `COGNITO__CLIENTID`
- Retornar `AccessToken`, `IdToken`, `ExpiresIn`, `TokenType`
- Port `ICognitoService` na Application
- Implementação concreta na Infra usando `AWSSDK.CognitoIdentityProvider`

---

## Testes e Qualidade

### Estrutura de Testes

- **FastFood.Auth.Tests.Unit**: Testes unitários
- **FastFood.Auth.Tests.Bdd**: Testes BDD (SpecFlow ou estilo BDD com xUnit)

### Testes Unitários

- Testes para:
  - Domain (regras e invariantes)
  - UseCases (com mocks dos Ports)
  - Value Objects (validações)
- Usar xUnit, Moq ou NSubstitute para mocks

### Testes BDD

- Pelo menos **1 cenário BDD por serviço**
- Exemplo: "Dado CPF válido, quando identificar, então retorna token"
- Usar SpecFlow ou estilo BDD com xUnit

### Cobertura e Qualidade

- **Cobertura mínima alvo: >= 80%** (cobertura de linha)
- **Sonar Quality Gate**: deve passar sem code smells críticos e vulnerabilidades bloqueantes
- Testes devem ser executados no CI/CD

---

## Convenções de Código

### .NET e C#

- Versão: **.NET 8** para todos os projetos
- Seguir convenções do C# Coding Conventions (Microsoft)
- Usar PascalCase para classes, métodos e propriedades públicas
- Usar camelCase para variáveis locais e campos privados
- Prefixar interfaces com "I" (ex: `ICustomerRepository`)

### Dependency Injection

- DI configurada no bootstrap (Program.cs do Lambda)
- Injetar UseCases, Presenters e implementações dos Ports
- Nada de instanciar dependências fora do bootstrap

### Gerenciamento de Solução

- **SEMPRE adicionar novos projetos ao arquivo de solução após criá-los**
- Executar: `dotnet sln <arquivo-solucao> add <caminho-projeto>`
- Projetos devem estar na raiz da solução (não em pastas virtuais)
- Manter estrutura de diretórios físicos (src/, tests/)

---

## Consistência com Outros Serviços

A arquitetura do AuthLambda deve ser **idêntica** à dos serviços HTTP (OrderHub, PayStream, KitchenFlow), mudando apenas o tipo de hosting (Lambda vs EKS).

**Padrão único mental:**
- ✅ Mesma estrutura de pastas (Application, Domain, Infra)
- ✅ Mesma organização horizontal por contexto
- ✅ Mesmo fluxo (InputModel → UseCase → OutputModel → Presenter)
- ✅ Swagger em tudo
- ✅ Testes unitários + BDD
- ✅ Clean Architecture ~80%

---

## O que isso garante

- ✅ **Um único padrão mental** (API/Lambda)
- ✅ Swagger em tudo
- ✅ Clean Arch defendável
- ✅ Excelente DX (Developer Experience)
- ✅ Excelente narrativa para banca e entrevistas
- ✅ Consistência entre todos os serviços do ecossistema

> *"O Auth é um Lambda, mas arquiteturalmente é uma API ASP.NET Core como qualquer outra; só mudamos o host."*

