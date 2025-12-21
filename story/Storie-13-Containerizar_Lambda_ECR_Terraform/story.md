# Storie-13: Containerizar Lambda com Dockerfile e Integrar Build/Push ECR com Terraform

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA] (preencher quando concluída)

## Descrição
Como engenheiro de plataforma, quero containerizar o Lambda com Dockerfile compatível com AWS Lambda container image, automatizar o build e push da imagem no ECR via GitHub Actions, e garantir que o Terraform (Story 12) receba corretamente o ECR_IMAGE_URI para atualizar o Lambda, para que o fluxo completo de deploy funcione de forma automatizada e idempotente.

## Objetivo
Criar o Dockerfile do projeto Lambda compatível com AWS Lambda container image (multi-stage), configurar workflow de CI/CD que constrói a imagem, faz push no ECR com tag baseada em SHA e exporta ECR_IMAGE_URI, e revisar/ajustar a Story 12 (Terraform) para receber corretamente o ECR_IMAGE_URI e executar o apply na ordem correta (build → push → terraform apply).

## Contexto
- O deploy do Auth Lambda será feito usando "AWS Lambda container image" (package_type = Image).
- O repositório de infra (Terraform) já cria/gerencia o Lambda e atualiza o image_uri (Story 12).
- O fluxo atual está incompleto: falta criar o Dockerfile e gerar a imagem antes de subir para o ECR.
- Sem imagem publicada no ECR, o Terraform não consegue apontar o Lambda para a imagem (image_uri fica inválido).
- A ordem obrigatória é: build → push → terraform apply.

## Escopo Técnico
- Tecnologias: Docker, AWS Lambda container image, AWS ECR, GitHub Actions, Terraform, .NET 8
- Arquivos afetados:
  - `Dockerfile` (na raiz do projeto Lambda)
  - `.dockerignore` (otimização do build)
  - `.github/workflows/deploy-lambda.yml` (workflow de build/push ECR)
  - `terraform/variables.tf` (revisar/adicionar variáveis se necessário)
  - `terraform/README.md` (documentar novos parâmetros e fluxo completo)
  - `README.md` ou `docs/DEPLOY_LAMBDA.md` (documentar processo completo)
- Recursos AWS:
  - Repositório ECR existente (não gerenciado por esta story)
  - Função Lambda gerenciada pelo Terraform (Story 12)
  - IAM permissions para GitHub Actions acessar ECR e executar Terraform

## Parâmetros Documentados
- `AWS_REGION`: Região AWS onde o Lambda será deployado (ex: us-east-1)
- `LAMBDA_FUNCTION_NAME`: Nome da função Lambda (ex: auth-cpf-lambda)
- `ECR_REPOSITORY_URL`: URL do repositório ECR sem tag (ex: 118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda)
- `IMAGE_TAG`: Tag da imagem baseada em SHA do commit (ex: sha-abcdef123)
- `ECR_IMAGE_URI`: URI completa derivada (ECR_REPOSITORY_URL:IMAGE_TAG)

## Subtasks

- [Subtask 01: Criar Dockerfile do Lambda compatível com AWS Lambda container image](./subtask/Subtask-01-Criar_Dockerfile_Lambda_compativel_AWS.md)
- [Subtask 02: Criar/ajustar workflow de CI/CD para build e push no ECR com exportação de ECR_IMAGE_URI](./subtask/Subtask-02-Criar_workflow_CI_CD_build_push_ECR.md)
- [Subtask 03: Revisar Story 12 (Terraform) e propor mudanças mínimas para receber ECR_IMAGE_URI](./subtask/Subtask-03-Revisar_Story_12_Terraform_ajustes.md)

## Critérios de Aceite da História

- [ ] Dockerfile criado na raiz do projeto Lambda
- [ ] Dockerfile usa multi-stage build para otimização
- [ ] Dockerfile usa imagem base oficial AWS Lambda para .NET 8 (`public.ecr.aws/lambda/dotnet:8`)
- [ ] Dockerfile compila e publica aplicação corretamente
- [ ] `docker build` executa sem erros localmente
- [ ] Imagem gerada tem tamanho razoável (< 500MB idealmente)
- [ ] Workflow GitHub Actions criado/ajustado para build e push no ECR
- [ ] Workflow usa tag baseada em SHA do commit (`sha-${GITHUB_SHA::7}`)
- [ ] Workflow exporta `ECR_IMAGE_URI` como output/artefato para uso posterior
- [ ] Workflow executa na ordem: build → push → terraform apply
- [ ] Workflow pode ser executado manualmente (workflow_dispatch)
- [ ] Story 12 revisada e ajustada (se necessário) para receber ECR_IMAGE_URI
- [ ] Variáveis Terraform documentadas e funcionais
- [ ] Terraform recebe ECR_IMAGE_URI corretamente via variável ou input
- [ ] Terraform apply executa após build e push da imagem
- [ ] README documenta todos os parâmetros novos: AWS_REGION, LAMBDA_FUNCTION_NAME, ECR_REPOSITORY_URL, IMAGE_TAG, ECR_IMAGE_URI
- [ ] README documenta o fluxo completo: build → push → terraform apply
- [ ] README diferencia claramente: build de imagem (CI/CD) vs deploy de infra (Terraform)
- [ ] Processo testado end-to-end: build → push → terraform apply → Lambda atualizado
- [ ] Pipeline consegue construir e publicar imagem do Lambda no ECR
- [ ] Terraform atualiza o Lambda para usar a nova imagem via image_uri
- [ ] Processo é idempotente (pode ser executado múltiplas vezes)
- [ ] Processo é reexecutável (pode ser executado após mudanças)

