# Subtask 03: Configurar VPC e Security Group no recurso Lambda

## Descrição
Configurar o recurso `aws_lambda_function` com `vpc_config` para associar o Lambda à VPC com o Security Group localizado, permitindo comunicação com o banco de dados RDS.

## Passos de implementação
- Abrir arquivo `terraform/lambda.tf`
- Adicionar bloco `vpc_config` no recurso `aws_lambda_function.lambda`:
  ```hcl
  vpc_config {
    subnet_ids         = data.aws_subnets.default.ids
    security_group_ids = [data.aws_security_group.lambda_sg.id]
  }
  ```
- Garantir que o Security Group do data source está sendo usado corretamente
- Adicionar comentários explicando:
  - Por que o Lambda precisa estar na VPC (acesso ao RDS)
  - Quais subnets são usadas
  - Qual Security Group é usado
- Verificar que o `lifecycle` block não ignora `vpc_config` (se necessário, remover de `ignore_changes`)
- Considerar adicionar timeout maior se necessário (acesso VPC pode ser mais lento)

## Como testar
- Executar `terraform validate` (deve passar sem erros)
- Executar `terraform plan` com parâmetros válidos:
  - `lambda_security_group_name` ou `lambda_security_group_id` válidos (subnets são descobertas automaticamente)
- Verificar que o plano mostra `vpc_config` sendo adicionado/atualizado
- Verificar que não há erros de validação
- Executar `terraform apply` (em ambiente de teste) e verificar:
  - Lambda é associado à VPC corretamente
  - Security Group é aplicado corretamente
  - Lambda pode acessar o RDS (teste de conectividade)

## Critérios de aceite
- Arquivo `terraform/lambda.tf` atualizado com `vpc_config`
- `vpc_config` usa `data.aws_subnets.default.ids` para subnets (descobertas automaticamente)
- `vpc_config` usa o Security Group do data source
- Comentários explicam a configuração VPC
- `lifecycle` block não ignora `vpc_config` (ou está configurado corretamente)
- `terraform validate` passa sem erros
- `terraform plan` mostra `vpc_config` sendo configurado
- Lambda é associado à VPC corretamente após `terraform apply`
- Security Group é aplicado corretamente ao Lambda

