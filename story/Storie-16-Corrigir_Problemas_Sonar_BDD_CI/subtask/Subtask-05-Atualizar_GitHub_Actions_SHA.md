# Subtask 05: Atualizar GitHub Actions para usar commit SHA

## Descrição
Substituir todas as tags de versão (@v4, @v2, @v3, etc.) nos workflows do GitHub Actions por commit SHA hash completo, garantindo reprodutibilidade, segurança e conformidade com as melhores práticas de CI/CD.

## Problema Identificado
Múltiplos workflows usando tags de versão ao invés de commit SHA:
- `aws-actions/configure-aws-credentials@v4`
- `aws-actions/amazon-ecr-login@v2`
- `docker/setup-buildx-action@v3`
- `actions/checkout@v4`
- `actions/setup-dotnet@v4`
- `actions/upload-artifact@v4`
- `actions/download-artifact@v4`
- `actions/cache@v4`
- `hashicorp/setup-terraform@v3`
- `docker/build-push-action@v5`
- E outros...

## Passos de Implementação

1. **Identificar todos os workflows:**
   - Listar todos os arquivos `.yml` em `.github/workflows/`
   - Identificar todas as ocorrências de `uses: *@v*`
   - Criar lista completa de ações que precisam ser atualizadas

2. **Obter commit SHA para cada ação:**
   - Para cada ação, acessar o repositório no GitHub
   - Encontrar o commit SHA correspondente à versão usada
   - Documentar o SHA e a versão correspondente

3. **Atualizar cada workflow:**
   - Substituir `uses: owner/action@v4` por `uses: owner/action@<commit-sha> # v4`
   - Manter comentário com a versão para referência
   - Garantir que o formato está correto

4. **Exemplo de atualização:**
   ```yaml
   # Antes
   - name: Configure AWS credentials
     uses: aws-actions/configure-aws-credentials@v4
   
   # Depois
   - name: Configure AWS credentials
     uses: aws-actions/configure-aws-credentials@ff717079ee2060e4bcee96c4779b553acc87447c # v4
   ```

5. **Validar workflows:**
   - Verificar sintaxe YAML: `yamllint .github/workflows/*.yml`
   - Executar workflows manualmente se possível
   - Confirmar que não há erros de sintaxe

## Arquivos Afetados

- `.github/workflows/deploy-lambda.yml`
- `.github/workflows/tests.yml`
- `.github/workflows/sonar.yml`
- `.github/workflows/run-migrator.yml`
- `.github/workflows/destroy-lambdas.yml`
- Qualquer outro workflow que use tags de versão

## Ações a Atualizar

### AWS Actions
- `aws-actions/configure-aws-credentials@v4` → SHA correspondente
- `aws-actions/amazon-ecr-login@v2` → SHA correspondente

### Docker Actions
- `docker/setup-buildx-action@v3` → SHA correspondente
- `docker/build-push-action@v5` → SHA correspondente

### GitHub Actions
- `actions/checkout@v4` → SHA correspondente
- `actions/setup-dotnet@v4` → SHA correspondente
- `actions/upload-artifact@v4` → SHA correspondente
- `actions/download-artifact@v4` → SHA correspondente
- `actions/cache@v4` → SHA correspondente

### HashiCorp Actions
- `hashicorp/setup-terraform@v3` → SHA correspondente

## Como Testar

- Validar sintaxe YAML: `yamllint .github/workflows/*.yml`
- Verificar que todos os workflows têm SHA completo
- Executar workflows manualmente (workflow_dispatch) para validar
- Confirmar que não há erros de execução

## Critérios de Aceite

- [ ] Todos os workflows usam commit SHA hash completo
- [ ] Comentários com versão são mantidos para referência
- [ ] Sintaxe YAML está correta
- [ ] Workflows podem ser executados sem erros
- [ ] Documentação atualizada com processo de atualização de versões

## Notas

- Usar commit SHA completo (40 caracteres) ao invés de SHA curto
- Manter comentário com versão para facilitar atualizações futuras
- Documentar processo de atualização de versões
- Considerar criar script para automatizar atualização de versões no futuro

## Referências

- [GitHub Actions Security Best Practices](https://docs.github.com/en/actions/security-guides/security-hardening-for-github-actions)
- [Using Actions with SHA](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions#jobsjob_idstepsuses)

