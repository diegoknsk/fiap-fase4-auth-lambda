# Subtask 02: Criar arquivo .dockerignore

## Descrição
Criar arquivo .dockerignore para excluir arquivos desnecessários do contexto Docker, otimizando tempo de build e tamanho da imagem.

## Passos de implementação
- Criar arquivo `.dockerignore` na raiz do projeto
- Adicionar exclusões padrão:
  - `**/bin/`
  - `**/obj/`
  - `**/.vs/`
  - `**/.vscode/`
  - `**/.idea/`
  - `**/.git/`
  - `**/.gitignore`
  - `**/node_modules/`
  - `**/*.md` (exceto README se necessário)
  - `**/tests/`
  - `**/.dockerignore`
  - `**/Dockerfile*`
  - `**/.github/`
  - `**/Storie-*/`
  - `**/rules/`
  - `**/*.slnx`
  - `**/*.user`
  - `**/appsettings.Development.json`
- Manter arquivos necessários:
  - `**/*.csproj`
  - `**/*.cs`
  - `**/appsettings.json`
  - `**/Program.cs`

## Como testar
- Executar `docker build` e verificar que arquivos excluídos não são copiados
- Validar que tempo de build é otimizado
- Verificar que apenas arquivos necessários estão no contexto

## Critérios de aceite
- Arquivo .dockerignore criado na raiz
- Arquivos desnecessários excluídos (bin, obj, tests, etc.)
- Arquivos necessários mantidos (.csproj, .cs, appsettings.json)
- Build do Docker otimizado (menor tempo e contexto)

