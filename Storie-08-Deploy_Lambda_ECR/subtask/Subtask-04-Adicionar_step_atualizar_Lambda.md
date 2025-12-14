# Subtask 04: Adicionar step para atualizar função Lambda

## Descrição
Adicionar step no workflow para atualizar a função Lambda com a nova imagem do ECR após o push bem-sucedido.

## Passos de implementação
- Adicionar step no job `build-and-push` (ou criar novo job `update-lambda`):
  - Aguardar push bem-sucedido: usar `needs: build-and-push` se job separado
  - Atualizar função Lambda: `aws lambda update-function-code --function-name ${{ secrets.LAMBDA_FUNCTION_NAME }} --image-uri $ECR_REPOSITORY:$IMAGE_TAG`
  - Aguardar atualização: `aws lambda wait function-updated --function-name ${{ secrets.LAMBDA_FUNCTION_NAME }}`
  - (Opcional) Publicar nova versão: `aws lambda publish-version --function-name ${{ secrets.LAMBDA_FUNCTION_NAME }}`
  - (Opcional) Atualizar alias: `aws lambda update-alias --function-name ${{ secrets.LAMBDA_FUNCTION_NAME }} --name production --function-version $VERSION`
- Adicionar tratamento de erros:
  - Verificar se função Lambda existe antes de atualizar
  - Retornar erro claro se atualização falhar
- Adicionar output do workflow com URI da imagem e status da atualização

## Como testar
- Executar workflow completo
- Verificar que função Lambda é atualizada após push
- Validar no console AWS Lambda que nova imagem está sendo usada
- Testar função Lambda após atualização para garantir que funciona

## Critérios de aceite
- Step de atualização Lambda adicionado ao workflow
- Comando `aws lambda update-function-code` executado corretamente
- Workflow aguarda atualização da função
- Função Lambda atualizada com nova imagem do ECR
- Tratamento de erros implementado
- Output do workflow mostra status da atualização
- Lambda funciona corretamente após atualização

