# Subtask 01: Definir variáveis Terraform necessárias

## Descrição
Criar as variáveis Terraform necessárias para o deploy do Lambda via Terraform mínimo, garantindo que todos os valores sejam recebidos via parâmetros e não hardcoded, seguindo as regras de infraestrutura do projeto.

## Passos de implementação
- Identificar arquivo `variables.tf` no repositório de infraestrutura (separado)
- Adicionar variável `aws_region`:
  - Tipo: `string`
  - Descrição: "Região AWS onde o Lambda será deployado"
  - Não definir valor default (deve ser obrigatória)
- Adicionar variável `lambda_function_name`:
  - Tipo: `string`
  - Descrição: "Nome da função Lambda que será atualizada"
  - Não definir valor default (deve ser obrigatória)
- Adicionar variável `ecr_image_uri`:
  - Tipo: `string`
  - Descrição: "URI completa da imagem ECR com tag (ex: 118233104061.dkr.ecr.us-east-1.amazonaws.com/auth-cpf-lambda:sha-abcdef)"
  - Não definir valor default (deve ser obrigatória)
- Validar que nenhuma variável tem valor hardcoded
- Adicionar comentários explicativos em cada variável
- Seguir padrão de nomenclatura do projeto (snake_case para variáveis)

## Como testar
- Executar `terraform validate` no diretório Terraform (deve passar sem erros)
- Executar `terraform fmt -recursive` para validar formatação
- Verificar que `terraform plan` solicita valores para as variáveis (não deve ter defaults)
- Validar sintaxe HCL com `terraform validate`
- Verificar que variáveis estão documentadas com descrição clara

## Critérios de aceite
- Arquivo `variables.tf` contém variável `aws_region` (tipo string, sem default)
- Arquivo `variables.tf` contém variável `lambda_function_name` (tipo string, sem default)
- Arquivo `variables.tf` contém variável `ecr_image_uri` (tipo string, sem default)
- Todas as variáveis têm descrição clara e explicativa
- Nenhuma variável tem valor default (todas obrigatórias)
- `terraform validate` passa sem erros
- `terraform fmt` formatado corretamente
- Variáveis seguem padrão snake_case


