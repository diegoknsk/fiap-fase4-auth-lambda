# Subtask 09: Remover projeto Lambda antigo

## Descrição
Remover o projeto `FastFood.Auth.Lambda` antigo que não é mais necessário, já que foi substituído por `FastFood.Auth.Lambda.Customer` e `FastFood.Auth.Lambda.Admin`.

## Passos de implementação
1. Verificar que os novos projetos estão funcionando:
   - `FastFood.Auth.Lambda.Customer` compilando e funcionando
   - `FastFood.Auth.Lambda.Admin` compilando e funcionando
   - Deploy das novas Lambdas funcionando

2. Remover projeto da solução:
   ```bash
   dotnet sln remove src/FastFood.Auth.Lambda/FastFood.Auth.Lambda.csproj
   ```

3. Deletar diretório do projeto:
   ```bash
   rm -rf src/FastFood.Auth.Lambda
   ```
   Ou no Windows:
   ```powershell
   Remove-Item -Recurse -Force src/FastFood.Auth.Lambda
   ```

4. Remover Dockerfile antigo (se ainda existir):
   - `Dockerfile.auth-lambda` (se não for mais usado)

5. Verificar referências ao projeto antigo:
   ```bash
   grep -r "FastFood.Auth.Lambda" .
   ```
   - Verificar se há referências em documentação, scripts, etc.
   - Atualizar ou remover conforme necessário

6. Verificar que não há imports ou referências quebradas:
   - Compilar solução completa: `dotnet build`
   - Verificar que não há erros de referência

7. Atualizar documentação (se houver):
   - README.md
   - Documentação de deploy
   - Qualquer referência ao projeto antigo

## Arquivos a remover
- `src/FastFood.Auth.Lambda/` (diretório completo)
- `Dockerfile.auth-lambda` (se não for mais usado)

## Arquivos a verificar e atualizar
- `.sln` (solução)
- `README.md` (se houver)
- Documentação de deploy
- Scripts de build/deploy

## Como testar
- Executar `dotnet build` na solução (deve compilar sem erros)
- Verificar que não há referências quebradas
- Verificar que os novos projetos estão funcionando
- Executar testes (se houver)

## Critérios de aceitação
- [ ] Projeto `FastFood.Auth.Lambda` removido da solução
- [ ] Diretório `src/FastFood.Auth.Lambda` deletado
- [ ] Dockerfile antigo removido (se aplicável)
- [ ] Solução compila sem erros
- [ ] Não há referências quebradas
- [ ] Documentação atualizada (se necessário)
- [ ] Novos projetos funcionando corretamente

## Notas
- Fazer backup antes de deletar (ou usar controle de versão)
- Garantir que o deploy das novas Lambdas está funcionando antes de remover o projeto antigo
- Se houver testes específicos do projeto antigo, verificar se precisam ser movidos ou atualizados

