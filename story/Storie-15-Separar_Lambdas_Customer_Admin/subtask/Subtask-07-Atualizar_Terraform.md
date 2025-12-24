# Subtask 07: Atualizar Terraform para novos projetos

## Descrição
Atualizar o Terraform para usar os novos projetos `FastFood.Auth.Lambda.Customer` e `FastFood.Auth.Lambda.Admin`, removendo a variável `LAMBDA_MODE` e simplificando as configurações.

## Passos de implementação
1. Atualizar `terraform/lambda.tf`:
   - Remover variável `LAMBDA_MODE` de `auth_lambda_env` (renomear para `auth_customer_lambda_env`)
   - Remover variável `LAMBDA_MODE` de `auth_admin_lambda_env`
   - Remover variável `LAMBDA_MODE` de `auth_migrator_lambda_env` (já não é usada, mas manter para limpeza)
   - Renomear módulo `auth_lambda` para `auth_customer_lambda` (ou criar novo)
   - Atualizar `function_name` para `auth-customer-lambda`
   - Atualizar variáveis de imagem ECR:
     - `lambda_auth_image_uri` → `lambda_auth_customer_image_uri`
     - `lambda_auth_admin_image_uri` (manter)
     - `lambda_auth_migrator_image_uri` (manter)

2. Atualizar `terraform/variables.tf`:
   - Renomear `lambda_auth_image_uri` para `lambda_auth_customer_image_uri`
   - Atualizar descrição para refletir que é para Customer Lambda
   - Manter `lambda_auth_admin_image_uri` e `lambda_auth_migrator_image_uri`

3. Atualizar `terraform/outputs.tf`:
   - Renomear outputs de `auth_lambda` para `auth_customer_lambda`
   - Atualizar descrições
   - Manter outputs de `auth_admin_lambda` e `auth_migrator_lambda`

4. Verificar `terraform/ecr.tf`:
   - As tags das imagens devem ser:
     - `auth-customer-lambda-{sha}` e `auth-customer-lambda-latest`
     - `auth-admin-lambda-{sha}` e `auth-admin-lambda-latest`
     - `auth-migrator-lambda-{sha}` e `auth-migrator-lambda-latest`

5. Atualizar variáveis de ambiente:
   - `auth_customer_lambda_env`: Remover `LAMBDA_MODE`, manter connection string, JWT, Cognito
   - `auth_admin_lambda_env`: Remover `LAMBDA_MODE`, manter Cognito (sem connection string se não usar)
   - `auth_migrator_lambda_env`: Remover `LAMBDA_MODE`, manter apenas connection string

6. Verificar que não há referências ao projeto antigo `FastFood.Auth.Lambda`

## Arquivos a modificar
- `terraform/lambda.tf`
- `terraform/variables.tf`
- `terraform/outputs.tf`
- `terraform/ecr.tf` (se necessário)

## Estrutura esperada

### terraform/lambda.tf
```hcl
locals {
  # Variáveis de ambiente para auth-customer-lambda (RDS + Cognito + JWT)
  auth_customer_lambda_env = merge(
    # Removido: LAMBDA_MODE = "Customer"
    var.rds_connection_string != "" ? { ConnectionStrings__DefaultConnection = var.rds_connection_string } : {},
    # ... outras variáveis
  )

  # Variáveis de ambiente para auth-admin-lambda (Cognito)
  auth_admin_lambda_env = merge(
    # Removido: LAMBDA_MODE = "Admin"
    var.cognito_region != "" ? { COGNITO__REGION = var.cognito_region } : {},
    # ... outras variáveis
  )

  # Variáveis de ambiente para auth-migrator-lambda (RDS)
  auth_migrator_lambda_env = merge(
    # Removido: LAMBDA_MODE = "Migrator"
    var.rds_connection_string != "" ? { ConnectionStrings__DefaultConnection = var.rds_connection_string } : {}
  )
}

module "auth_customer_lambda" {
  source = "./modules/lambda"
  function_name = "auth-customer-lambda"
  # ... outras configurações
  image_uri = var.lambda_auth_customer_image_uri != "" ? var.lambda_auth_customer_image_uri : null
  environment_variables = local.auth_customer_lambda_env
  # ...
}
```

## Como testar
- Executar `terraform validate` para verificar sintaxe
- Executar `terraform plan` para verificar mudanças
- Verificar que não há referências a `LAMBDA_MODE`
- Verificar que os nomes das funções estão corretos

## Critérios de aceitação
- [ ] Variável `LAMBDA_MODE` removida de todas as configurações
- [ ] Módulo `auth_customer_lambda` configurado corretamente
- [ ] Módulo `auth_admin_lambda` atualizado (sem `LAMBDA_MODE`)
- [ ] Módulo `auth_migrator_lambda` atualizado (sem `LAMBDA_MODE`)
- [ ] Variáveis de imagem ECR atualizadas
- [ ] Outputs atualizados
- [ ] `terraform validate` passa sem erros
- [ ] `terraform plan` mostra mudanças esperadas

