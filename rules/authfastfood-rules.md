# RULES — AuthFastFood (Lambda ASP.NET Core)

## Objetivo
Serviço de autenticação e identificação, publicado como **AWS Lambda**, mas
estruturado como uma **API ASP.NET Core tradicional**, utilizando
Amazon.Lambda.AspNetCoreServer para hospedar a aplicação.

A arquitetura segue ~80% Clean Architecture, com foco em mercado, simplicidade
e consistência com os demais microsserviços do sistema.

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

## Swagger
- Swagger deve estar habilitado:
  - Em ambiente local
  - Em ambiente Lambda (quando permitido pelo API Gateway)
- Endpoints devem ser documentados normalmente.
- Swagger é obrigatório para padronização e inspeção de contrato.

## Estrutura de projetos
- AuthFastFood.Api (ASP.NET Core)
- AuthFastFood.Application
- AuthFastFood.Domain
- AuthFastFood.Infra.Persistence (se houver persistência)
- AuthFastFood.Infra.Messaging (se publicar eventos)
- AuthFastFood.Tests.Unit
- AuthFastFood.Tests.Bdd

## Regras de dependência
- Api -> Application
- Application -> Domain
- Infra.* implementa interfaces (ports) da Application
- Domain não referencia:
  - ASP.NET
  - EF Core
  - AWS SDK
  - HttpClient
  - Configuration

## Fluxo padrão
Controller -> UseCase.Execute(Command) -> Ports -> Infra

A arquitetura do AuthLambda deve ser idêntica à dos serviços EKS,
mudando apenas o tipo de hosting.

## API (Controllers)
- Controllers são adapters de transporte.
- **Responsabilidades:**
  - autenticação/autorização
  - validação básica de request
  - mapear RequestModel -> InputModel
  - chamar UseCase.ExecuteAsync(InputModel)
  - receber ResponseModel do UseCase e retornar HTTP Response
- **Proibido:**
  - regra de negócio no controller
  - acesso direto a banco ou SDKs
  - chamar Presenter diretamente (responsabilidade do UseCase)
  - ResponseModels duplicados (deve usar Application Responses)
  - Presenters próprios (deve usar Presenters da Application)

## Application
- UseCases pequenos e focados:
  - IdentifyCustomer
  - RegisterCustomer
  - IssueToken
  - ValidateToken
- UseCases recebem apenas InputModels da Application.
- **Responsabilidade do UseCase:**
  - Executar lógica de negócio
  - Chamar Ports (repositórios, serviços)
  - Chamar o Presenter para transformar OutputModel em ResponseModel
  - Retornar ResponseModel (não OutputModel diretamente)
- **Responses (ResponseModels):**
  - Devem estar na camada Application.
  - Definem o contrato de saída dos UseCases.
  - Representam os dados que serão retornados pela API.
- **Presenters:**
  - Devem estar na camada Application.
  - São chamados pelo UseCase (não pelo Controller).
  - Fazem adaptação/transformação dos OutputModels em ResponseModels quando necessário.
  - Por padrão apenas retornam o response, mas podem fazer transformações se necessário.
- Ports (interfaces) ficam aqui:
  - IAuthRepository
  - ITokenService
  - IMessageBus / IEventPublisher (se necessário)

## Domain
- Entidades, Value Objects e validações.
- Exceções de domínio para regras inválidas.
- Nenhuma dependência externa.

## Persistência
- Se houver banco:
  - Infra.Persistence implementa IAuthRepository
  - Entidades de banco são separadas das entidades de domínio
  - DbContext nunca é acessado fora da Infra.

## Message Bus
- Comunicação assíncrona via port IMessageBus/IEventPublisher.
- Implementação concreta (SNS/SQS/EventBridge) fica na Infra.

## Testes e Qualidade
- Testes unitários para:
  - Domain (regras e invariantes)
  - UseCases (com mocks dos ports)
- Pelo menos 1 cenário BDD:
  - "Dado CPF válido, quando identificar, então retorna token"
- Cobertura mínima alvo: >= 80%
- Sonar:
  - Sem code smells críticos
  - Sem vulnerabilidades bloqueantes

## Convenções
- UseCase: <Verbo><Entidade>UseCase
- Command/Query: <Ação><Entidade>Command/Query
- Nada de instanciar dependências fora do bootstrap (DI).
- A arquitetura do AuthFastFood deve espelhar a dos serviços HTTP do sistema.

---

## O que isso te garante (bem importante)
- ✅ **Um único padrão mental** (API/Lambda)
- ✅ Swagger em tudo
- ✅ Clean Arch defendável
- ✅ Excelente DX
- ✅ Excelente narrativa para banca e entrevistas

> *"O Auth é um Lambda, mas arquiteturalmente é uma API ASP.NET Core como qualquer outra; só mudamos o host."*


