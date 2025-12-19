# Subtask 04: Atualizar Rules para documentar padrão

## Descrição
Atualizar o arquivo de regras para documentar que presenters e ResponseModels devem estar na camada Application.

## Passos de implementação
- Atualizar `rules/baseprojectcontext.mdc`:
  - Modificar seção sobre `/Api` para remover referência a presenters
  - Adicionar seção sobre `/Application` mencionando Presenters e Responses
  - Atualizar regras de dependência se necessário
- Atualizar `rules/authfastfood-rules.md` se houver referências específicas

## Regra a ser documentada

**Presenters e ResponseModels:**
- Presenters devem estar na camada Application
- ResponseModels (Application Responses) devem estar na camada Application
- API não deve ter ResponseModels duplicados
- API apenas consome os Responses da Application através dos Presenters
- Presenters podem fazer transformações dos Application Responses quando necessário

## Como testar
- Verificar que as regras estão atualizadas
- Verificar que a documentação está clara

## Critérios de aceite
- Rules atualizadas com o padrão arquitetural
- Documentação clara sobre localização de presenters e responses
- Regras de dependência atualizadas se necessário








