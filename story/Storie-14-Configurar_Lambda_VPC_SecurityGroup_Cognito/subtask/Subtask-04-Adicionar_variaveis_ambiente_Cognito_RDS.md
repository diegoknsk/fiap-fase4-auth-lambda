# Subtask 04: Adicionar variáveis de ambiente do Cognito e RDS no Lambda

## Descrição
Configurar o bloco `environment` no recurso `aws_lambda_function` com todas as variáveis de ambiente necessárias para o Cognito (autenticação de administradores) e RDS (connection string do banco de dados).

## Passos de implementação
- Abrir arquivo `terraform/lambda.tf`
- Adicionar bloco `environment` no recurso `aws_lambda_function.lambda`:
  ```hcl
  environment {
    variables = {
      # Cognito
      COGNITO__REGION     = var.cognito_region
      COGNITO__USERPOOLID = var.cognito_user_pool_id
      COGNITO__CLIENTID   = var.cognito_client_id
      
      # RDS Connection String (passada diretamente, já vem completa)
      ConnectionStrings__DefaultConnection = var.rds_connection_string
      
      # JWT Settings (se necessário)
      JwtSettings__Secret         = var.jwt_secret
      JwtSettings__Issuer         = var.jwt_issuer
      JwtSettings__Audience       = var.jwt_audience
      JwtSettings__ExpirationHours = var.jwt_expiration_hours
    }
  }
  ```
- Garantir que a connection string é passada diretamente (já vem completa do parâmetro)
- Adicionar comentários explicando cada variável de ambiente
- Verificar que variáveis sensíveis (senhas) não são expostas em outputs
- Considerar usar `sensitive = true` no bloco environment se o Terraform suportar (versão >= 1.0)

## Como testar
- Executar `terraform validate` (deve passar sem erros)
- Executar `terraform plan` com todos os parâmetros:
  - Verificar que connection string é passada corretamente (já vem completa)
  - Verificar que variáveis do Cognito são passadas corretamente
  - Verificar que variáveis JWT são passadas corretamente
- Executar `terraform apply` (em ambiente de teste) e verificar:
  - Variáveis de ambiente são configuradas no Lambda
  - Connection string está no formato correto
  - Lambda pode se conectar ao RDS (teste de conectividade)
  - Lambda pode se comunicar com Cognito (teste de autenticação)

## Critérios de aceite
- Arquivo `terraform/lambda.tf` atualizado com bloco `environment`
- Variáveis de ambiente do Cognito configuradas: `COGNITO__REGION`, `COGNITO__USERPOOLID`, `COGNITO__CLIENTID`
- Connection string do RDS passada diretamente via `var.rds_connection_string` (já vem completa)
- Variáveis de ambiente JWT configuradas (se necessário): `JwtSettings__Secret`, `JwtSettings__Issuer`, `JwtSettings__Audience`, `JwtSettings__ExpirationHours`
- Comentários explicam cada variável de ambiente
- `terraform validate` passa sem erros
- `terraform plan` mostra variáveis de ambiente sendo configuradas
- Connection string é passada corretamente ao Lambda
- Variáveis de ambiente são configuradas no Lambda após `terraform apply`
- Lambda pode se conectar ao RDS e Cognito após configuração

