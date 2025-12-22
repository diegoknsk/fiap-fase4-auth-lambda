# Storie-12: Deploy Lambda via Terraform Mínimo com ECR

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA] (preencher quando concluída)

## Descrição
Como engenheiro de plataforma, quero configurar o deploy do Lambda via Terraform mínimo que recebe a URI da imagem ECR como variável, para que o processo de deploy seja idempotente, reexecutável e mantenha a separação de responsabilidades entre build de imagem (CI/CD) e gerenciamento de infraestrutura (Terraform).

## Objetivo
Definir e implementar o processo de deploy do Lambda via Terraform mínimo, onde o Terraform recebe a URI completa da imagem ECR (já com tag) como variável e atualiza o recurso `aws_lambda_function` apontando para a nova imagem, garantindo que o processo seja idempotente, reexecutável e documentado, sem que o Terraform faça push de imagens.

## Escopo Técnico
- Tecnologias: Terraform, AWS Lambda, AWS ECR, AWS CLI, GitHub Actions
- Arquivos afetados:
  - Arquivos Terraform no repositório de infraestrutura (separado):
    - `variables.tf` (adicionar variáveis: `lambda_function_name`, `ecr_image_uri`, `aws_region`)
    - `lambda.tf` ou arquivo específico para Lambda (atualizar recurso `aws_lambda_function`)
  - Documentação:
    - `README.md` ou `docs/DEPLOY_LAMBDA.md` (documentar processo e parâmetros)
- Recursos AWS:
  - Recurso `aws_lambda_function` existente (criado como placeholder via Terraform)
  - Repositório ECR existente (não gerenciado por esta story)
  - IAM permissions para Terraform atualizar Lambda

## Subtasks

- [Subtask 01: Definir variáveis Terraform necessárias](./subtask/Subtask-01-Definir_variaveis_Terraform.md)
- [Subtask 02: Atualizar recurso aws_lambda_function com image_uri](./subtask/Subtask-02-Atualizar_recurso_Lambda_image_uri.md)
- [Subtask 03: Configurar CI/CD para passar ECR_IMAGE_URI ao Terraform](./subtask/Subtask-03-Configurar_CI_CD_Terraform.md)
- [Subtask 04: Documentar processo de deploy e parâmetros](./subtask/Subtask-04-Documentar_processo_deploy.md)

## Critérios de Aceite da História

- [ ] Variáveis Terraform criadas: `aws_region`, `lambda_function_name`, `ecr_image_uri`
- [ ] Variáveis não têm valores hardcoded, todas recebidas via parâmetros
- [ ] Recurso `aws_lambda_function` configurado com `package_type = "Image"`
- [ ] Recurso `aws_lambda_function` usa `image_uri = var.ecr_image_uri`
- [ ] Terraform não faz push de imagem (apenas atualiza Lambda)
- [ ] Processo de deploy é idempotente (terraform apply pode ser executado múltiplas vezes)
- [ ] Processo de deploy é reexecutável (pode ser executado após mudanças)
- [ ] CI/CD passa `ECR_IMAGE_URI` para Terraform via variável de ambiente ou terraform.tfvars
- [ ] CI/CD executa `terraform apply` após build e push da imagem
- [ ] Documentação criada explicando o fluxo completo de deploy
- [ ] Documentação lista explicitamente os parâmetros necessários: `AWS_REGION`, `LAMBDA_FUNCTION_NAME`, `ECR_IMAGE_URI`
- [ ] Documentação diferencia claramente: push de imagem (CI/CD) vs deploy de infra (Terraform)
- [ ] Exemplo de URI ECR documentado (formato: `118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef`)
- [ ] `terraform validate` passa sem erros
- [ ] `terraform fmt` formatado corretamente
- [ ] Processo testado end-to-end: build → push → terraform apply → Lambda atualizado

