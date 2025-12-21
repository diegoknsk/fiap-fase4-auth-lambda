# Subtask 02: Criar/ajustar workflow de CI/CD para build e push no ECR com exportação de ECR_IMAGE_URI

## Descrição
Criar ou ajustar workflow do GitHub Actions que faz build da imagem Docker, faz push para o repositório ECR da AWS com tag baseada em SHA do commit, e exporta a variável `ECR_IMAGE_URI` para uso posterior no Terraform. O workflow deve garantir que a ordem seja: build → push → terraform apply.

## Passos de implementação
- Criar diretório `.github/workflows/` se não existir
- Criar ou ajustar arquivo `.github/workflows/deploy-lambda.yml`
- Configurar triggers:
  - `on: push: branches: [main]` (execução automática após merge)
  - `on: workflow_dispatch` (execução manual)
- Criar job `build-and-push`:
  - Usar runner: `runs-on: ubuntu-latest`
  - Fazer checkout: `actions/checkout@v4`
  - Configurar AWS credentials: `aws-actions/configure-aws-credentials@v4`:
    - Usar secrets: `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`
    - Configurar região: `AWS_REGION` (via secret ou variável de ambiente)
  - Fazer login no ECR:
    - `aws ecr get-login-password --region ${{ secrets.AWS_REGION }} | docker login --username AWS --password-stdin ${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com`
  - Construir variáveis:
    - `ECR_REPOSITORY_URL`: `${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com/${{ secrets.ECR_REPOSITORY_NAME }}`
    - `IMAGE_TAG`: `sha-${{ github.sha }}` (primeiros 7 caracteres do SHA) ou `sha-${{ github.sha }}`
    - `ECR_IMAGE_URI`: `${{ env.ECR_REPOSITORY_URL }}:${{ env.IMAGE_TAG }}`
  - Fazer build da imagem: `docker build -t ${{ env.ECR_IMAGE_URI }} .`
  - Fazer push: `docker push ${{ env.ECR_IMAGE_URI }}`
  - Exportar `ECR_IMAGE_URI` como output do job:
    - `echo "ECR_IMAGE_URI=${{ env.ECR_IMAGE_URI }}" >> $GITHUB_OUTPUT`
    - Ou usar `set-output` (se versão antiga do GitHub Actions)
- Criar job `terraform-apply` (depende de `build-and-push`):
  - Usar `needs: build-and-push` para garantir ordem
  - Receber `ECR_IMAGE_URI` via `needs.build-and-push.outputs.ecr_image_uri`
  - Executar Terraform apply com variáveis:
    - `AWS_REGION=${{ secrets.AWS_REGION }}`
    - `LAMBDA_FUNCTION_NAME=${{ secrets.LAMBDA_FUNCTION_NAME }}`
    - `ECR_IMAGE_URI=${{ needs.build-and-push.outputs.ecr_image_uri }}`
- Documentar secrets necessários no README:
  - `AWS_ACCESS_KEY_ID`
  - `AWS_SECRET_ACCESS_KEY`
  - `AWS_REGION`
  - `AWS_ACCOUNT_ID`
  - `ECR_REPOSITORY_NAME`
  - `LAMBDA_FUNCTION_NAME`
  - `LAMBDA_ROLE_ARN` (se necessário para Terraform)

## Como testar
- Fazer commit e push do workflow
- Executar workflow manualmente via GitHub Actions UI
- Verificar logs do workflow para confirmar build e push
- Verificar no console AWS ECR que imagem foi criada com tag correta
- Validar que `ECR_IMAGE_URI` está sendo exportado corretamente
- Verificar que job `terraform-apply` recebe `ECR_IMAGE_URI` corretamente
- Validar que Terraform apply executa após build e push
- Testar end-to-end: build → push → terraform apply → Lambda atualizado

## Critérios de aceite
- Arquivo `.github/workflows/deploy-lambda.yml` criado/ajustado
- Workflow configurado com triggers (push na main e workflow_dispatch)
- Workflow faz login no ECR corretamente
- Workflow faz build da imagem Docker
- Workflow usa tag baseada em SHA do commit (formato: `sha-<7-primeiros-caracteres>`)
- Workflow faz push para ECR com tag apropriada
- Workflow exporta `ECR_IMAGE_URI` como output do job
- Job `terraform-apply` depende de `build-and-push` (ordem garantida)
- Job `terraform-apply` recebe `ECR_IMAGE_URI` corretamente
- Workflow pode ser executado manualmente
- Imagem aparece no ECR após execução com tag correta
- Logs do workflow mostram execução bem-sucedida
- README documenta todos os secrets necessários
- README documenta o fluxo completo: build → push → terraform apply
- Processo testado end-to-end com sucesso

