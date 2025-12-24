# Subtask 03: Configurar CI/CD para passar ECR_IMAGE_URI ao Terraform

## Descrição
Configurar o pipeline CI/CD (GitHub Actions) para passar a URI da imagem ECR para o Terraform após o build e push da imagem, garantindo que o Terraform atualize o Lambda com a nova imagem de forma automatizada.

## Passos de implementação
- Identificar workflow GitHub Actions existente que faz build e push ECR (criado na Storie-08)
- Adicionar novo step/job após o push da imagem para ECR:
  - Step: "Deploy Lambda via Terraform"
  - Configurar AWS credentials: `aws-actions/configure-aws-credentials@v4`
  - Fazer checkout do repositório de infraestrutura (se separado) ou usar Terraform inline
  - Executar `terraform init` no diretório Terraform
  - Executar `terraform plan` com variáveis:
    - `-var="aws_region=${{ secrets.AWS_REGION }}"`
    - `-var="lambda_function_name=${{ secrets.LAMBDA_FUNCTION_NAME }}"`
    - `-var="ecr_image_uri=${{ env.ECR_IMAGE_URI }}"`
  - Executar `terraform apply -auto-approve` (ou com aprovação manual para produção)
- Capturar `ECR_IMAGE_URI` do step anterior de build/push:
  - Formato: `${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com/${{ secrets.ECR_REPOSITORY }}:${{ github.sha }}`
  - Salvar em variável de ambiente `ECR_IMAGE_URI` para uso no Terraform
- Configurar secrets necessários no GitHub (se ainda não existirem):
  - `AWS_REGION`
  - `LAMBDA_FUNCTION_NAME`
  - `ECR_REPOSITORY` (para construir URI)
  - `AWS_ACCOUNT_ID` (para construir URI)
- Adicionar comentários explicativos no workflow sobre separação de responsabilidades
- Garantir que o step de Terraform só executa após sucesso do push ECR

## Como testar
- Executar workflow manualmente via GitHub Actions UI (workflow_dispatch)
- Verificar logs do workflow para confirmar que `ECR_IMAGE_URI` é capturado corretamente
- Verificar logs do Terraform mostrando `terraform plan` e `terraform apply`
- Validar que Lambda é atualizado após execução do workflow
- Verificar no console AWS que Lambda aponta para a nova imagem ECR
- Testar reexecução do workflow (deve ser idempotente)
- Verificar que workflow falha graciosamente se Terraform falhar

## Critérios de aceite
- Workflow GitHub Actions atualizado com step de deploy via Terraform
- Step de Terraform executa após push ECR bem-sucedido
- Variável `ECR_IMAGE_URI` é capturada do step anterior e passada para Terraform
- Terraform recebe variáveis via `-var` flags (não hardcoded)
- Secrets configurados no GitHub: `AWS_REGION`, `LAMBDA_FUNCTION_NAME`
- Workflow pode ser executado manualmente (workflow_dispatch)
- Workflow executa automaticamente após push na branch main (se configurado)
- Logs do workflow mostram execução bem-sucedida do Terraform
- Lambda é atualizado com nova imagem após execução do workflow
- Processo é idempotente (pode ser reexecutado)


