# Subtask 03: Mover CustomerController para Lambda.Customer

## Descrição
Mover o `CustomerController` do projeto `FastFood.Auth.Lambda` para o novo projeto `FastFood.Auth.Lambda.Customer` e atualizar o namespace.

## Passos de implementação
1. Copiar `CustomerController.cs` do projeto antigo:
   - De: `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs`
   - Para: `src/FastFood.Auth.Lambda.Customer/Controllers/CustomerController.cs`

2. Atualizar namespace:
   - Alterar de `FastFood.Auth.Lambda.Controllers`
   - Para: `FastFood.Auth.Lambda.Customer.Controllers`

3. Verificar que todas as dependências estão corretas:
   - `FastFood.Auth.Application.UseCases.Customer`
   - `FastFood.Auth.Application.InputModels.Customer`
   - `FastFood.Auth.Application.OutputModels.Customer`
   - `Microsoft.AspNetCore.Mvc`

4. Testar compilação:
   ```bash
   dotnet build src/FastFood.Auth.Lambda.Customer
   ```

5. Verificar que o controller está sendo registrado corretamente no `Program.cs`:
   - Deve estar disponível em `/api/customer/*`
   - Endpoints esperados:
     - `POST /api/customer/anonymous`
     - `POST /api/customer/register`
     - `POST /api/customer/identify`

## Arquivos a modificar
- `src/FastFood.Auth.Lambda.Customer/Controllers/CustomerController.cs` (criar)

## Arquivos a remover (depois, na subtask 09)
- `src/FastFood.Auth.Lambda/Controllers/CustomerController.cs` (será removido quando o projeto antigo for deletado)

## Como testar
- Executar `dotnet build` no projeto Customer (deve compilar sem erros)
- Verificar que o namespace está correto
- Verificar que todas as dependências estão disponíveis
- (Opcional) Executar localmente e testar os endpoints

## Critérios de aceitação
- [ ] `CustomerController` copiado para o novo projeto
- [ ] Namespace atualizado corretamente
- [ ] Projeto compila sem erros
- [ ] Todas as dependências resolvidas
- [ ] Controller registrado e acessível via rota `/api/customer/*`

