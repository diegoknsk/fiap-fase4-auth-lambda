# Subtask 04: Documentar processo de deploy e parâmetros

## Descrição
Criar documentação completa explicando o processo de deploy do Lambda via Terraform mínimo, listando explicitamente os parâmetros necessários, diferenciando push de imagem (CI/CD) de deploy de infraestrutura (Terraform), e fornecendo exemplos práticos.

## Passos de implementação
- Criar arquivo de documentação: `docs/DEPLOY_LAMBDA.md` ou atualizar `README.md` no repositório de infraestrutura
- Documentar fluxo completo de deploy:
  - Seção 1: "Visão Geral" - explicar separação de responsabilidades
  - Seção 2: "Fluxo de Deploy" - passo a passo do processo
  - Seção 3: "Parâmetros Necessários" - lista explícita de variáveis
  - Seção 4: "Exemplos Práticos" - comandos e exemplos de uso
- Documentar parâmetros obrigatórios:
  - `AWS_REGION`: Região AWS (ex: `us-east-1`)
  - `LAMBDA_FUNCTION_NAME`: Nome da função Lambda (ex: `auth-cpf-lambda`)
  - `ECR_IMAGE_URI`: URI completa da imagem ECR com tag (ex: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef`)
- Explicar diferença entre:
  - Push de imagem (CI/CD): build Docker + push para ECR (GitHub Actions)
  - Deploy de infra (Terraform): atualização do Lambda com nova image_uri
- Documentar comandos Terraform:
  - `terraform init`
  - `terraform plan -var="..." -var="..." -var="..."`
  - `terraform apply -var="..." -var="..." -var="..."`
- Incluir exemplo completo de URI ECR:
  - Formato: `<account-id>.dkr.ecr.<region>.amazonaws.com/<repository>:<tag>`
  - Exemplo real: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef`
- Documentar processo idempotente e reexecutável
- Adicionar seção de troubleshooting (erros comuns)
- Incluir diagrama de fluxo (texto ou mermaid) se aplicável

## Como testar
- Ler documentação completa e verificar clareza
- Validar que todos os parâmetros estão listados explicitamente
- Verificar que exemplos de URI ECR estão corretos
- Testar comandos documentados (copiar e colar deve funcionar)
- Validar que diferença entre push e deploy está clara
- Verificar ortografia e formatação markdown
- Garantir que documentação está acessível e bem organizada

## Critérios de aceite
- Documentação criada em `docs/DEPLOY_LAMBDA.md` ou `README.md`
- Fluxo completo de deploy documentado passo a passo
- Parâmetros obrigatórios listados explicitamente: `AWS_REGION`, `LAMBDA_FUNCTION_NAME`, `ECR_IMAGE_URI`
- Exemplo de URI ECR documentado com formato completo
- Diferença entre push de imagem (CI/CD) e deploy de infra (Terraform) explicada claramente
- Comandos Terraform documentados com exemplos práticos
- Processo idempotente e reexecutável documentado
- Seção de troubleshooting incluída (erros comuns)
- Documentação está clara, objetiva e bem formatada
- Exemplos práticos podem ser copiados e executados diretamente

