# Subtask 03: Revisar Story 12 (Terraform) e propor mudanças mínimas para receber ECR_IMAGE_URI

## Descrição
Revisar a implementação da Story 12 (Terraform) para garantir que está preparada para receber corretamente o `ECR_IMAGE_URI` do workflow de CI/CD, validar se as variáveis estão corretas, verificar se a documentação está completa, e propor mudanças mínimas (se necessário) para que o fluxo completo funcione perfeitamente: build → push → terraform apply.

## Passos de implementação
- Revisar arquivo `terraform/variables.tf`:
  - Verificar se variável `ecr_image_uri` existe e está configurada corretamente
  - Verificar se variável `aws_region` existe e está configurada corretamente
  - Verificar se variável `lambda_function_name` existe e está configurada corretamente
  - Validar que todas as variáveis são obrigatórias (sem default)
  - Validar que descrições estão claras e incluem exemplos
- Revisar arquivo `terraform/lambda.tf`:
  - Verificar se `package_type = "Image"` está configurado
  - Verificar se `image_uri = var.ecr_image_uri` está configurado
  - Verificar se `function_name = var.lambda_function_name` está configurado
  - Validar que não há referências a `filename` ou `handler` (incompatível com Image)
  - Verificar se comentários explicam a separação de responsabilidades
- Revisar arquivo `terraform/README.md`:
  - Verificar se documenta todos os parâmetros: `AWS_REGION`, `LAMBDA_FUNCTION_NAME`, `ECR_REPOSITORY_URL`, `IMAGE_TAG`, `ECR_IMAGE_URI`
  - Verificar se documenta o fluxo completo: build → push → terraform apply
  - Verificar se diferencia claramente: build de imagem (CI/CD) vs deploy de infra (Terraform)
  - Verificar se inclui exemplo de URI ECR (formato completo)
  - Verificar se documenta como passar variáveis para Terraform (via `-var` ou `terraform.tfvars`)
- Propor mudanças mínimas (se necessário):
  - Adicionar variáveis faltantes (se houver)
  - Ajustar descrições de variáveis para incluir todos os parâmetros
  - Atualizar documentação com informações sobre `ECR_REPOSITORY_URL` e `IMAGE_TAG`
  - Adicionar seção sobre integração com CI/CD no README
  - Documentar ordem obrigatória: build → push → terraform apply
- Validar sintaxe Terraform:
  - Executar `terraform validate` (deve passar sem erros)
  - Executar `terraform fmt -recursive` para validar formatação
- Testar integração (se possível):
  - Simular passagem de `ECR_IMAGE_URI` via variável
  - Executar `terraform plan` com variáveis de teste
  - Verificar que plan mostra atualização do Lambda com nova image_uri

## Como testar
- Executar `terraform validate` no diretório Terraform (deve passar sem erros)
- Executar `terraform fmt -recursive` para validar formatação
- Executar `terraform plan` com variáveis de teste:
  - `terraform plan -var="aws_region=us-east-1" -var="lambda_function_name=test-lambda" -var="ecr_image_uri=123456789.dkr.ecr.us-east-1.amazonaws.com/test:sha-abcdef"`
- Verificar que o plan mostra atualização do recurso `aws_lambda_function` com `image_uri`
- Validar que documentação está completa e clara
- Verificar que README lista explicitamente todos os parâmetros necessários
- Testar integração com workflow (se possível): build → push → terraform apply

## Critérios de aceite
- Arquivo `terraform/variables.tf` contém todas as variáveis necessárias:
  - `aws_region` (tipo string, sem default, descrição clara)
  - `lambda_function_name` (tipo string, sem default, descrição clara)
  - `ecr_image_uri` (tipo string, sem default, descrição clara com exemplo)
- Arquivo `terraform/lambda.tf` está configurado corretamente:
  - `package_type = "Image"`
  - `image_uri = var.ecr_image_uri`
  - `function_name = var.lambda_function_name`
  - Sem referências a `filename` ou `handler`
- Arquivo `terraform/README.md` documenta:
  - Todos os parâmetros: `AWS_REGION`, `LAMBDA_FUNCTION_NAME`, `ECR_REPOSITORY_URL`, `IMAGE_TAG`, `ECR_IMAGE_URI`
  - Fluxo completo: build → push → terraform apply
  - Diferenciação: build de imagem (CI/CD) vs deploy de infra (Terraform)
  - Exemplo de URI ECR completo
  - Como passar variáveis para Terraform
- `terraform validate` passa sem erros
- `terraform fmt` formatado corretamente
- `terraform plan` mostra atualização do Lambda com nova image_uri
- Documentação está completa e clara
- Mudanças propostas são mínimas e não quebram funcionalidade existente
- Story 12 está preparada para receber `ECR_IMAGE_URI` do CI/CD


