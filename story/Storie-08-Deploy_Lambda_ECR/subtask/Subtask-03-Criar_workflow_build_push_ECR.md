# Subtask 03: Criar workflow GitHub Actions para build e push ECR

## Descrição
Criar workflow do GitHub Actions que faz build da imagem Docker e faz push para o repositório ECR da AWS.

## Passos de implementação
- Criar diretório `.github/workflows/` se não existir
- Criar arquivo `.github/workflows/deploy-lambda.yml`
- Configurar triggers:
  - `on: push: branches: [main]` (execução automática após merge)
  - `on: workflow_dispatch` (execução manual)
- Criar job `build-and-push`:
  - Usar runner: `runs-on: ubuntu-latest`
  - Fazer checkout: `actions/checkout@v4`
  - Configurar AWS credentials: `aws-actions/configure-aws-credentials@v4`:
    - Usar secrets: `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`
    - Configurar região: `AWS_REGION` (via secret ou variável)
  - Fazer login no ECR: `aws ecr get-login-password --region ${{ secrets.AWS_REGION }} | docker login --username AWS --password-stdin ${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com`
  - Obter repositório ECR: usar secret `ECR_REPOSITORY` ou construir dinamicamente
  - Fazer build da imagem: `docker build -t $ECR_REPOSITORY:$IMAGE_TAG .`
  - Criar tag: `docker tag $ECR_REPOSITORY:$IMAGE_TAG $ECR_REPOSITORY:latest`
  - Fazer push: `docker push $ECR_REPOSITORY:$IMAGE_TAG` e `docker push $ECR_REPOSITORY:latest`
  - Usar variável `IMAGE_TAG` baseada em commit SHA ou número de build: `${{ github.sha }}` ou `${{ github.run_number }}`

## Como testar
- Fazer commit e push do workflow
- Executar workflow manualmente via GitHub Actions UI
- Verificar logs do workflow para confirmar build e push
- Verificar no console AWS ECR que imagem foi criada
- Validar que tags estão corretas

## Critérios de aceite
- Arquivo `.github/workflows/deploy-lambda.yml` criado
- Workflow configurado com triggers (push e manual)
- Workflow faz login no ECR corretamente
- Workflow faz build da imagem Docker
- Workflow faz push para ECR com tag apropriada
- Workflow pode ser executado manualmente
- Imagem aparece no ECR após execução
- Logs do workflow mostram execução bem-sucedida

