# Subtask 04: Mover AdminController para Lambda.Admin

## Descrição
Mover o `AdminController` do projeto `FastFood.Auth.Lambda` para o novo projeto `FastFood.Auth.Lambda.Admin` e atualizar o namespace.

## Passos de implementação
1. Copiar `AdminController.cs` do projeto antigo:
   - De: `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`
   - Para: `src/FastFood.Auth.Lambda.Admin/Controllers/AdminController.cs`

2. Atualizar namespace:
   - Alterar de `FastFood.Auth.Lambda.Controllers`
   - Para: `FastFood.Auth.Lambda.Admin.Controllers`

3. Verificar que todas as dependências estão corretas:
   - `FastFood.Auth.Application.UseCases.Admin`
   - `FastFood.Auth.Application.InputModels.Admin`
   - `FastFood.Auth.Application.OutputModels.Admin`
   - `Microsoft.AspNetCore.Mvc`

4. Testar compilação:
   ```bash
   dotnet build src/FastFood.Auth.Lambda.Admin
   ```

5. Verificar que o controller está sendo registrado corretamente no `Program.cs`:
   - Deve estar disponível em `/api/admin/*`
   - Endpoint esperado:
     - `POST /api/admin/login`

## Arquivos a modificar
- `src/FastFood.Auth.Lambda.Admin/Controllers/AdminController.cs` (criar)

## Arquivos a remover (depois, na subtask 09)
- `src/FastFood.Auth.Lambda/Controllers/AdminController.cs` (será removido quando o projeto antigo for deletado)

## Como testar
- Executar `dotnet build` no projeto Admin (deve compilar sem erros)
- Verificar que o namespace está correto
- Verificar que todas as dependências estão disponíveis
- (Opcional) Executar localmente e testar o endpoint

## Critérios de aceitação
- [ ] `AdminController` copiado para o novo projeto
- [ ] Namespace atualizado corretamente
- [ ] Projeto compila sem erros
- [ ] Todas as dependências resolvidas
- [ ] Controller registrado e acessível via rota `/api/admin/*`

