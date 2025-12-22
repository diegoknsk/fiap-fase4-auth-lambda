# Subtask 06: Testar deploy completo no ambiente de desenvolvimento

## Descrição
Testar o fluxo completo de deploy: build, push ECR e atualização Lambda, validando que tudo funciona corretamente no ambiente de desenvolvimento.

## Passos de implementação
- Executar workflow manualmente via GitHub Actions UI (workflow_dispatch)
- Monitorar execução do workflow:
  - Verificar que build da imagem Docker é bem-sucedido
  - Verificar que push para ECR é bem-sucedido
  - Verificar que atualização do Lambda é bem-sucedida
- Validar no console AWS:
  - ECR: imagem está presente com tags corretas
  - Lambda: função está usando nova imagem
  - Lambda: configuração está correta (memória, timeout, etc.)
- Testar função Lambda após deploy:
  - Fazer requisição de teste via API Gateway ou console AWS
  - Validar que endpoints respondem corretamente
  - Verificar logs do CloudWatch para erros
- Validar rollback (se necessário):
  - Testar atualização com imagem inválida (deve falhar graciosamente)
  - Verificar que Lambda mantém versão anterior se atualização falhar
- Documentar processo de deploy:
  - Criar ou atualizar README com instruções de deploy
  - Documentar variáveis de ambiente necessárias
  - Documentar processo de rollback

## Como testar
- Executar workflow completo e monitorar execução
- Validar que todos os steps passam
- Testar função Lambda após deploy
- Verificar logs e métricas do Lambda

## Critérios de aceite
- Workflow executa completamente sem erros
- Imagem Docker buildada e enviada para ECR
- Função Lambda atualizada com nova imagem
- Lambda funciona corretamente após deploy
- Endpoints respondem corretamente
- Logs do CloudWatch não mostram erros críticos
- Processo de deploy documentado
- Rollback testado e funcionando (se implementado)

