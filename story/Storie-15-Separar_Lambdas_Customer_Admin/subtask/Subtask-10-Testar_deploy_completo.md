# Subtask 10: Testar deploy completo

## Descrição
Testar o deploy completo das 3 Lambdas (Customer, Admin, Migrator) para garantir que tudo está funcionando corretamente após a refatoração.

## Passos de implementação
1. Verificar pré-requisitos:
   - Terraform configurado corretamente
   - Workflow de deploy atualizado
   - Secrets do GitHub Actions configurados
   - ECR repository criado

2. Executar workflow de deploy:
   - Fazer push para branch `main` ou executar manualmente
   - Verificar que o workflow executa sem erros
   - Verificar que as 3 imagens são construídas
   - Verificar que as 3 imagens são enviadas para ECR
   - Verificar que as 3 Lambdas são atualizadas

3. Testar Lambda Customer:
   - Obter Function URL da Lambda Customer
   - Testar endpoint `POST /api/customer/anonymous`
   - Testar endpoint `POST /api/customer/register`
   - Testar endpoint `POST /api/customer/identify`
   - Verificar logs da Lambda no CloudWatch

4. Testar Lambda Admin:
   - Obter Function URL da Lambda Admin
   - Testar endpoint `POST /api/admin/login`
   - Verificar logs da Lambda no CloudWatch

5. Testar Lambda Migrator:
   - Obter Function URL da Lambda Migrator (se tiver)
   - Ou executar via console AWS
   - Verificar que migrations são aplicadas corretamente
   - Verificar logs da Lambda no CloudWatch

6. Verificar configurações:
   - Variáveis de ambiente corretas em cada Lambda
   - VPC configurada corretamente (Customer e Migrator)
   - Security Groups corretos
   - IAM roles corretas

7. Verificar métricas:
   - CloudWatch metrics para cada Lambda
   - Verificar que não há erros excessivos
   - Verificar latência e performance

8. Testar integração:
   - Se houver testes de integração, executá-los
   - Verificar que os fluxos completos funcionam

## Checklist de testes

### Lambda Customer
- [ ] Function URL acessível
- [ ] `POST /api/customer/anonymous` retorna 200 com token
- [ ] `POST /api/customer/register` retorna 200 com token
- [ ] `POST /api/customer/identify` retorna 200 com token
- [ ] Logs no CloudWatch sem erros críticos
- [ ] Variáveis de ambiente configuradas (RDS, JWT, Cognito)

### Lambda Admin
- [ ] Function URL acessível
- [ ] `POST /api/admin/login` retorna 200 com tokens Cognito
- [ ] `POST /api/admin/login` com credenciais inválidas retorna 401
- [ ] Logs no CloudWatch sem erros críticos
- [ ] Variáveis de ambiente configuradas (Cognito)

### Lambda Migrator
- [ ] Lambda executável
- [ ] Migrations aplicadas corretamente
- [ ] Logs no CloudWatch mostrando sucesso
- [ ] Variáveis de ambiente configuradas (RDS)

### Infraestrutura
- [ ] VPC configurada corretamente (Customer e Migrator)
- [ ] Security Groups permitindo tráfego necessário
- [ ] IAM roles com permissões corretas
- [ ] ECR com 3 imagens (customer, admin, migrator)

## Como testar
1. Executar workflow de deploy
2. Aguardar conclusão do deploy
3. Obter Function URLs das Lambdas
4. Testar endpoints com ferramenta como Postman, curl ou similar
5. Verificar logs no CloudWatch
6. Verificar métricas no CloudWatch

## Critérios de aceitação
- [ ] Workflow de deploy executa sem erros
- [ ] 3 imagens construídas e enviadas para ECR
- [ ] 3 Lambdas atualizadas com sucesso
- [ ] Lambda Customer funcionando corretamente (todos endpoints)
- [ ] Lambda Admin funcionando corretamente (endpoint de login)
- [ ] Lambda Migrator funcionando corretamente
- [ ] Logs sem erros críticos
- [ ] Variáveis de ambiente corretas em todas as Lambdas
- [ ] Infraestrutura (VPC, Security Groups, IAM) configurada corretamente
- [ ] Métricas do CloudWatch dentro do esperado

## Troubleshooting
- Se houver erros no deploy, verificar logs do workflow
- Se houver erros nas Lambdas, verificar logs do CloudWatch
- Se houver problemas de conectividade, verificar VPC e Security Groups
- Se houver problemas de permissões, verificar IAM roles

