# Subtask 01: Corrigir teste BDD "Identificar customer existente"

## Descrição
Corrigir o teste BDD "Identificar customer existente por CPF" que está retornando 401 Unauthorized ao invés de 200 OK, garantindo que o fluxo de identificação de customer funcione corretamente.

## Problema Identificado
```
Então a resposta deve ter status 200
 -> error: Assert.Equal() Failure: Values differ
 Expected: OK
 Actual:   Unauthorized (0.2s)
```

O teste cria um customer com CPF "11144477735" no step `Given que existe um customer registrado com CPF`, mas quando faz a requisição POST para `/api/customer/identify`, o endpoint retorna 401 ao invés de 200.

## Possíveis Causas

1. **Problema de contexto do banco de dados**: O customer criado no step pode não estar sendo encontrado pelo UseCase devido a isolamento de contexto
2. **Problema de mapeamento de CPF**: O CPF pode estar sendo salvo/buscado de forma diferente
3. **Problema de Value Object**: O Cpf Value Object pode estar validando/rejeitando o CPF incorretamente
4. **Problema de repositório**: O método `GetByCpfAsync` pode não estar encontrando o customer

## Passos de Implementação

1. **Investigar o problema:**
   - Verificar se o customer está sendo salvo corretamente no banco de dados em memória
   - Verificar se o UseCase está recebendo o CPF correto
   - Verificar se o repositório está encontrando o customer
   - Adicionar logs temporários para debug

2. **Verificar isolamento de contexto:**
   - Confirmar que o mesmo `AuthDbContext` está sendo usado no step e no UseCase
   - Verificar se o `WebApplicationFactoryFixture` está configurando o banco corretamente
   - Garantir que o contexto do banco é compartilhado entre os steps

3. **Verificar mapeamento de CPF:**
   - Confirmar que o CPF está sendo salvo sem formatação (apenas números)
   - Verificar se o `Cpf` Value Object está normalizando corretamente
   - Comparar o CPF salvo com o CPF buscado

4. **Corrigir o problema:**
   - Se for problema de contexto: garantir que o mesmo contexto é usado
   - Se for problema de CPF: normalizar o CPF antes de salvar/buscar
   - Se for problema de repositório: verificar a query do `GetByCpfAsync`

5. **Adicionar validações adicionais:**
   - Verificar que o customer criado no step realmente existe no banco
   - Adicionar assertion para validar que o customer foi encontrado antes da requisição

## Arquivos Afetados

- `src/tests/FastFood.Auth.Tests.Bdd/StepDefinitions/CustomerAuthenticationSteps.cs`
- `src/tests/FastFood.Auth.Tests.Bdd/Support/WebApplicationFactoryFixture.cs`
- `src/Core/FastFood.Auth.Application/UseCases/Customer/IdentifyCustomerUseCase.cs`
- `src/Core/FastFood.Auth.Infra.Persistence/Repositories/CustomerRepository.cs`

## Como Testar

- Executar o teste BDD específico: `dotnet test --filter "FullyQualifiedName~Identificar customer existente"`
- Verificar que o teste passa com status 200
- Verificar que o token JWT é retornado corretamente
- Verificar que o customerId correto é retornado
- Executar todos os testes BDD: `dotnet test src/tests/FastFood.Auth.Tests.Bdd/`

## Critérios de Aceite

- [ ] Teste "Identificar customer existente por CPF" passa com status 200
- [ ] Teste valida que um token JWT válido é retornado
- [ ] Teste valida que o customerId correto é retornado
- [ ] Todos os outros testes BDD continuam passando
- [ ] Não há regressões introduzidas
- [ ] O problema foi identificado e documentado

## Notas

- O problema pode estar relacionado ao uso de `InMemoryDatabase` e isolamento de contexto
- Verificar se o `AuthDbContext` está sendo injetado corretamente no UseCase
- Considerar usar o mesmo contexto do step no UseCase ou garantir que o contexto seja compartilhado

