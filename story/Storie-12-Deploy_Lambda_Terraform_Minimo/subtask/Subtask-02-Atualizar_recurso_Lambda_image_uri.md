# Subtask 02: Atualizar recurso aws_lambda_function com image_uri

## Descrição
Atualizar o recurso `aws_lambda_function` existente (criado como placeholder) para usar `package_type = "Image"` e `image_uri = var.ecr_image_uri`, garantindo que o Terraform atualize o Lambda apontando para a imagem ECR sem fazer push da imagem.

## Passos de implementação
- Identificar arquivo Terraform que contém o recurso `aws_lambda_function` (ex: `lambda.tf` ou arquivo específico)
- Localizar recurso `aws_lambda_function` existente (criado como placeholder)
- Configurar `package_type = "Image"` no recurso
- Configurar `image_uri = var.ecr_image_uri` no recurso
- Garantir que `function_name = var.lambda_function_name` (usar variável, não hardcode)
- Manter outras configurações do Lambda existentes (role, timeout, memory, etc.)
- Adicionar comentário explicativo: "Terraform não faz push de imagem, apenas atualiza image_uri"
- Validar que não há referências a `filename` ou `handler` (não compatível com package_type = "Image")
- Garantir que tags seguem padrão do projeto (se aplicável)
- Adicionar comentário sobre idempotência: "terraform apply pode ser executado múltiplas vezes"

## Como testar
- Executar `terraform validate` (deve passar sem erros)
- Executar `terraform fmt -recursive` para validar formatação
- Executar `terraform plan` com variáveis de teste:
  - `terraform plan -var="aws_region=us-east-1" -var="lambda_function_name=test-lambda" -var="ecr_image_uri=123456789.dkr.ecr.us-east-1.amazonaws.com/test:latest"`
- Verificar que o plan mostra atualização do recurso `aws_lambda_function` com `image_uri`
- Validar que não há erros de sintaxe ou referências inválidas
- Verificar que `package_type = "Image"` está configurado

## Critérios de aceite
- Recurso `aws_lambda_function` configurado com `package_type = "Image"`
- Recurso `aws_lambda_function` usa `image_uri = var.ecr_image_uri`
- Recurso `aws_lambda_function` usa `function_name = var.lambda_function_name`
- Não há referências a `filename` ou `handler` (incompatível com Image)
- Comentários explicativos adicionados sobre separação de responsabilidades
- `terraform validate` passa sem erros
- `terraform fmt` formatado corretamente
- `terraform plan` mostra atualização do Lambda com nova image_uri
- Recurso é idempotente (pode ser aplicado múltiplas vezes)


