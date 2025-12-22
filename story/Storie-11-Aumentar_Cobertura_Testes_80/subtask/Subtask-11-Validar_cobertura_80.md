# Subtask 11: Validar cobertura >= 80% e gerar relatório

## Descrição
Executar testes com cobertura, validar que a meta de 80% foi atingida e gerar relatório detalhado.

## Passos de Implementação

1. **Configurar cobertura:**
   - Verificar que `coverlet.collector` está no projeto de testes
   - Configurar formato de saída (opencover, cobertura, json)

2. **Executar testes com cobertura:**
   ```bash
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage/
   ```

3. **Validar cobertura por camada:**
   - Domain: >= 85%
   - Application: >= 80%
   - Infra: >= 75%
   - Lambda (Controllers): >= 70%
   - **Total: >= 80%**

4. **Gerar relatório:**
   - Usar `reportgenerator` para gerar relatório HTML
   - Documentar cobertura por arquivo
   - Identificar arquivos com baixa cobertura

5. **Documentar resultados:**
   - Criar arquivo `COBERTURA_TESTES.md` com:
     - Cobertura total
     - Cobertura por camada
     - Arquivos com baixa cobertura
     - Próximos passos (se necessário)

## Comandos

```bash
# Executar testes com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage/

# Gerar relatório HTML (se reportgenerator estiver instalado)
reportgenerator -reports:./TestResults/coverage/coverage.opencover.xml -targetdir:./TestResults/coverage/report -reporttypes:Html
```

## Critérios de Aceite

- [ ] Cobertura total >= 80% validada
- [ ] Cobertura por camada dentro das metas
- [ ] Relatório de cobertura gerado
- [ ] Documentação de cobertura criada
- [ ] Todos os testes passam
- [ ] CI/CD configurado para executar com cobertura

## Notas

- Se cobertura não atingir 80%, identificar arquivos com baixa cobertura e criar subtarefas adicionais
- Manter relatório de cobertura atualizado no repositório


