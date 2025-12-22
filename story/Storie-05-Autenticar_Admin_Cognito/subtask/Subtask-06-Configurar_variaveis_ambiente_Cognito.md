# Subtask 06: Configurar variáveis de ambiente Cognito

## Descrição
Configurar variáveis de ambiente para Cognito no appsettings.json e documentar uso em produção.

## Passos de implementação
- Abrir arquivo `src/FastFood.Auth.Lambda/appsettings.json`
- Adicionar seção "Cognito" com placeholders:
  - Region
  - UserPoolId
  - ClientId
- Documentar que em produção devem vir de variáveis de ambiente: COGNITO__REGION, COGNITO__USERPOOLID, COGNITO__CLIENTID

## Como testar
- Verificar que appsettings.json tem seção Cognito
- Validar formato das configurações

## Critérios de aceite
- Seção Cognito adicionada ao appsettings.json
- Placeholders configurados
- Documentação sobre variáveis de ambiente

