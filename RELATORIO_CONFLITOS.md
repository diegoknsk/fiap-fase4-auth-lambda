# Relatório de Conflitos e Inconsistências - Rules do Projeto Auth Lambda

## Arquivos de Rules Encontrados

1. `rules/authfastfood-rules.md` - Regras principais do Auth Lambda
2. `rules/FastFood.Auth.ArchitectureRules.md` - Regras de arquitetura específicas do Auth
3. `rules/FastFood.Auth.DotNetRules.md` - Regras .NET (duplicado)
4. `rules/dotnet-rules.md` - Regras .NET (duplicado)
5. `rules/story-creation-rules.mdc` - Regras para criação de stories
6. `rules/infrarules.mdc` - Regras de infraestrutura (Terraform/K8s)
7. `rules/baseprojectcontext.mdc` - Contexto geral do ecossistema FastFood

---

## Conflitos Identificados

### 1. **Duplicação de Arquivos**
- ❌ `dotnet-rules.md` e `FastFood.Auth.DotNetRules.md` são idênticos
- **Impacto**: Confusão sobre qual arquivo seguir
- **Solução**: Consolidar em um único arquivo

### 2. **Nomenclatura Inconsistente: Commands vs InputModels**
- ❌ Estrutura atual usa `Commands/` (ex: `RegisterCustomerCommand`)
- ❌ Arquitetura alvo especifica `InputModels/` (ex: `RegisterCustomerInputModel`)
- **Impacto**: Inconsistência com a arquitetura final desejada
- **Solução**: Migrar Commands para InputModels seguindo padrão alvo

### 3. **Nomenclatura Inconsistente: Responses vs OutputModels**
- ❌ Estrutura atual usa `Responses/` (ex: `RegisterCustomerResponse`)
- ❌ Arquitetura alvo especifica `OutputModels/` (ex: `RegisterCustomerOutputModel`)
- **Impacto**: Inconsistência com a arquitetura final desejada
- **Solução**: Migrar Responses para OutputModels seguindo padrão alvo

### 4. **Organização Vertical vs Horizontal**
- ❌ Estrutura atual: Organização vertical por tipo
  ```
  Commands/
    Admin/
    Customer/
  UseCases/
    Admin/
    Customer/
  ```
- ❌ Arquitetura alvo: Organização horizontal por contexto
  ```
  UseCases/
    Customer/
      CreateAnonymousCustomerUseCase.cs
    Admin/
      AuthenticateAdminUseCase.cs
  InputModels/
    Customer/
      CreateAnonymousCustomerInputModel.cs
    Admin/
      AuthenticateAdminInputModel.cs
  ```
- **Impacto**: Estrutura atual não segue o padrão alvo
- **Solução**: Reorganizar pastas para estrutura horizontal por contexto

### 5. **Nomenclatura: Ports vs Gateways**
- ❌ `baseprojectcontext.mdc` menciona "Gateways" (IOrderGateway, IPaymentGateway)
- ❌ `authfastfood-rules.md` e `FastFood.Auth.ArchitectureRules.md` mencionam "Ports" (ICustomerRepository, ITokenService)
- ❌ Código atual usa "Ports" (ICustomerRepository, ITokenService)
- **Impacto**: Inconsistência terminológica entre documentos
- **Solução**: Padronizar para "Ports" (já está no código e é mais alinhado com Clean Architecture)

### 6. **Estrutura de Projetos Inconsistente**
- ❌ `authfastfood-rules.md` menciona: `AuthFastFood.Api`, `AuthFastFood.Application`
- ❌ `FastFood.Auth.ArchitectureRules.md` menciona: `FastFood.Auth.Lambda`, `FastFood.Auth.Application`
- ❌ Código atual usa: `FastFood.Auth.Lambda`, `FastFood.Auth.Application`
- **Impacto**: Confusão sobre nomenclatura de projetos
- **Solução**: Padronizar para `FastFood.Auth.*` (já está no código)

### 7. **Fluxo Controller → UseCase**
- ❌ `authfastfood-rules.md` linha 56: "Controller -> UseCase.Execute(Command)"
- ❌ `FastFood.Auth.ArchitectureRules.md` linha 66: "mapear RequestModel -> Command"
- ❌ Arquitetura alvo: "UseCase NÃO recebe RequestModel da API; recebe InputModel da Application"
- **Impacto**: Controller está criando Commands a partir de RequestModels, mas deveria passar InputModels
- **Solução**: Controller deve mapear RequestModel -> InputModel e passar para UseCase

### 8. **Presenters: Responsabilidade Ambígua**
- ❌ `authfastfood-rules.md` linha 89: "Por padrão apenas retornam o response"
- ❌ `baseprojectcontext.mdc` linha 82: "podem fazer transformações se necessário"
- ❌ Arquitetura alvo: "Presenter transforma OutputModel em ResponseModel/ViewModel"
- **Impacto**: Inconsistência sobre quando usar Presenters
- **Solução**: Clarificar que Presenters transformam OutputModel em ResponseModel da API quando necessário

### 9. **Facade: Mencionado mas Não Definido**
- ❌ Arquitetura alvo menciona: "Facade NÃO é obrigatório. Só usar se um endpoint orquestrar múltiplos usecases"
- ❌ Nenhuma regra existente menciona Facade
- **Impacto**: Falta de orientação sobre quando usar Facade
- **Solução**: Adicionar regra clara sobre Facade nas rules consolidadas

### 10. **Infra: Estrutura Inconsistente**
- ❌ `baseprojectcontext.mdc` menciona: `/DataSource ou /Infra.Persistence /Infra.External`
- ❌ Código atual tem: `FastFood.Auth.Infra` e `FastFood.Auth.Infra.Persistence`
- ❌ Arquitetura alvo não especifica estrutura de Infra
- **Impacto**: Inconsistência sobre organização da camada de infraestrutura
- **Solução**: Padronizar para `Infra.Persistence` e `Infra` (para serviços externos)

### 11. **Testes: Estrutura Não Especificada**
- ❌ Arquitetura alvo menciona: "projeto de testes unitários por serviço" e "projeto BDD"
- ❌ Estrutura atual tem: `FastFood.Auth.Tests.Unit` e `FastFood.Auth.Tests.Bdd`
- ✅ Estrutura atual está correta, mas falta menção explícita nas rules
- **Solução**: Adicionar regra explícita sobre estrutura de testes

---

## Itens Redundantes

1. **Duplicação de Regras .NET**: `dotnet-rules.md` e `FastFood.Auth.DotNetRules.md` são idênticos
2. **Duplicação de Regras de Arquitetura**: `authfastfood-rules.md` e `FastFood.Auth.ArchitectureRules.md` têm sobreposição significativa
3. **Múltiplas Menções de Swagger**: Swagger é mencionado em vários arquivos, mas com detalhes similares

---

## Itens Vagos que Precisam Ficar Objetivos

1. **"~80% Clean Architecture"**: Não está claro o que significa exatamente 80%
   - **Solução**: Especificar que é uma simplificação pragmática, mantendo separação de camadas e inversão de dependência

2. **"UseCases pequenos e focados"**: Não define critérios objetivos
   - **Solução**: Especificar que cada UseCase deve ter uma única responsabilidade e não deve orquestrar múltiplos fluxos complexos

3. **"Quando necessário" para Presenters**: Não define quando é necessário
   - **Solução**: Especificar que Presenters são obrigatórios e devem transformar OutputModel em ResponseModel da API

4. **"Meta de cobertura >= 80%"**: Não especifica se é linha, branch ou ambos
   - **Solução**: Especificar cobertura de linha >= 80% e adequação ao Sonar Quality Gate

5. **"Facade só usar se um endpoint orquestrar múltiplos usecases"**: Não define critérios claros
   - **Solução**: Especificar que Facade deve ser usado quando um endpoint precisa chamar 3+ UseCases ou quando há lógica de orquestração complexa

---

## Resumo de Ações Necessárias

1. ✅ Consolidar regras duplicadas em um único arquivo
2. ✅ Padronizar nomenclatura: Commands → InputModels, Responses → OutputModels
3. ✅ Reorganizar estrutura: Vertical → Horizontal por contexto
4. ✅ Padronizar terminologia: Ports (não Gateways)
5. ✅ Clarificar fluxo: RequestModel → InputModel → UseCase → OutputModel → Presenter → ResponseModel
6. ✅ Adicionar regras explícitas sobre Facade, Testes e Infra
7. ✅ Tornar regras vagas em objetivos e mensuráveis



