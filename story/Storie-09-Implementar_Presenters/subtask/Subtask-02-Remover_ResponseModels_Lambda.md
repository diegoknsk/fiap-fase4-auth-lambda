# Subtask 02: Remover ResponseModels duplicados da Lambda

## Descrição
Remover os ResponseModels duplicados da camada Lambda, já que os Application Responses já existem e são idênticos.

## Passos de implementação
- Remover `src/FastFood.Auth.Lambda/Models/Customer/RegisterCustomerResponse.cs`
- Remover `src/FastFood.Auth.Lambda/Models/Customer/IdentifyCustomerResponse.cs`
- Remover `src/FastFood.Auth.Lambda/Models/Customer/CreateAnonymousCustomerResponse.cs`
- Remover `src/FastFood.Auth.Lambda/Models/Admin/AuthenticateAdminResponse.cs`
- Remover pasta `src/FastFood.Auth.Lambda/Models/` se estiver vazia (manter apenas RequestModels se necessário)

## Observação
Os Application Responses já existem e são idênticos aos ResponseModels da Lambda, então não há necessidade de duplicação. A API deve usar diretamente os Application Responses.

## Como testar
- Executar `dotnet build` no projeto Lambda (deve compilar sem erros após atualizar referências)
- Verificar que não há arquivos duplicados

## Critérios de aceite
- ResponseModels duplicados removidos da Lambda
- Código compila sem erros (após atualizar controllers)











