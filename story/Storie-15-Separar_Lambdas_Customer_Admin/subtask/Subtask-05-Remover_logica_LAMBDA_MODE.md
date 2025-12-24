# Subtask 05: Remover lógica LAMBDA_MODE e convenções

## Descrição
Remover toda a lógica relacionada a `LAMBDA_MODE`, `ControllerFilterConvention` e `ActionFilterConvention` dos novos projetos. Esta subtask também serve como verificação de que os novos projetos não têm essa lógica.

## Passos de implementação
1. Verificar `Program.cs` do `FastFood.Auth.Lambda.Customer`:
   - ❌ **NÃO deve ter** variável `lambdaMode` ou `LAMBDA_MODE`
   - ❌ **NÃO deve ter** `isCustomerMode`, `isAdminMode`, `isMigratorMode`
   - ❌ **NÃO deve ter** `ControllerFilterConvention`
   - ❌ **NÃO deve ter** `ActionFilterConvention`
   - ✅ **DEVE ter** apenas registro direto de controllers: `builder.Services.AddControllers()`
   - ✅ **DEVE ter** apenas serviços específicos de Customer

2. Verificar `Program.cs` do `FastFood.Auth.Lambda.Admin`:
   - ❌ **NÃO deve ter** variável `lambdaMode` ou `LAMBDA_MODE`
   - ❌ **NÃO deve ter** `isCustomerMode`, `isAdminMode`, `isMigratorMode`
   - ❌ **NÃO deve ter** `ControllerFilterConvention`
   - ❌ **NÃO deve ter** `ActionFilterConvention`
   - ✅ **DEVE ter** apenas registro direto de controllers: `builder.Services.AddControllers()`
   - ✅ **DEVE ter** apenas serviços específicos de Admin

3. Verificar `LambdaEntryPoint.cs` de ambos projetos:
   - ❌ **NÃO deve ter** lógica de modo
   - ✅ **DEVE ter** apenas configuração básica do `Startup`

4. Verificar que não há classes `ControllerFilterConvention` ou `ActionFilterConvention` nos novos projetos

5. Buscar por referências a `LAMBDA_MODE` nos novos projetos:
   ```bash
   grep -r "LAMBDA_MODE" src/FastFood.Auth.Lambda.Customer/
   grep -r "LAMBDA_MODE" src/FastFood.Auth.Lambda.Admin/
   ```
   - Não deve retornar nenhum resultado

6. Buscar por referências a `ControllerFilterConvention`:
   ```bash
   grep -r "ControllerFilterConvention" src/FastFood.Auth.Lambda.Customer/
   grep -r "ControllerFilterConvention" src/FastFood.Auth.Lambda.Admin/
   ```
   - Não deve retornar nenhum resultado

7. Buscar por referências a `ActionFilterConvention`:
   ```bash
   grep -r "ActionFilterConvention" src/FastFood.Auth.Lambda.Customer/
   grep -r "ActionFilterConvention" src/FastFood.Auth.Lambda.Admin/
   ```
   - Não deve retornar nenhum resultado

## Arquivos a verificar
- `src/FastFood.Auth.Lambda.Customer/Program.cs`
- `src/FastFood.Auth.Lambda.Customer/LambdaEntryPoint.cs`
- `src/FastFood.Auth.Lambda.Admin/Program.cs`
- `src/FastFood.Auth.Lambda.Admin/LambdaEntryPoint.cs`

## Como testar
- Executar buscas por `LAMBDA_MODE`, `ControllerFilterConvention`, `ActionFilterConvention` nos novos projetos
- Verificar que não há lógica condicional baseada em modo
- Verificar que os controllers são registrados diretamente sem filtros
- Compilar ambos projetos e verificar que funcionam corretamente

## Critérios de aceitação
- [ ] Nenhuma referência a `LAMBDA_MODE` nos novos projetos
- [ ] Nenhuma referência a `ControllerFilterConvention` nos novos projetos
- [ ] Nenhuma referência a `ActionFilterConvention` nos novos projetos
- [ ] `Program.cs` de ambos projetos sem lógica condicional de modo
- [ ] Controllers registrados diretamente sem filtros
- [ ] Ambos projetos compilando e funcionando corretamente

