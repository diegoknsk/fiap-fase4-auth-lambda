# Storie-05: Autenticar Admin via AWS Cognito

## Descrição
Como administrador, quero autenticar-me através do AWS Cognito usando username e password, para que eu possa obter tokens válidos (AccessToken, IdToken) para acessar recursos administrativos do sistema.

## Objetivo
Implementar o endpoint `/admin/login` que permite autenticar um usuário administrador através do AWS Cognito. O sistema deve validar as credenciais (username/password) no Cognito e retornar os tokens (AccessToken, IdToken) válidos para uso em outros serviços do sistema.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, AWS Cognito SDK (AWSSDK.CognitoIdentityProvider), JWT
- Arquivos afetados:
  - `src/FastFood.Auth.Application/UseCases/Admin/AuthenticateAdminUseCase.cs`
  - `src/FastFood.Auth.Application/Commands/Admin/AuthenticateAdminCommand.cs`
  - `src/FastFood.Auth.Application/Responses/Admin/AuthenticateAdminResponse.cs`
  - `src/FastFood.Auth.Application/Ports/ICognitoService.cs`
  - `src/FastFood.Auth.Infra/Services/CognitoService.cs`
  - `src/FastFood.Auth.Lambda/Controllers/AdminController.cs`
  - `src/FastFood.Auth.Lambda/Models/Admin/AdminLoginRequest.cs`
- Recursos: Endpoint POST `/admin/login` que autentica via Cognito e retorna tokens

## Subtasks

- [Subtask 01: Adicionar pacote AWSSDK.CognitoIdentityProvider](./subtask/Subtask-01-Adicionar_pacote_AWSSDK_Cognito.md)
- [Subtask 02: Criar port ICognitoService na Application](./subtask/Subtask-02-Criar_port_ICognitoService.md)
- [Subtask 03: Implementar CognitoService na Infra](./subtask/Subtask-03-Implementar_CognitoService.md)
- [Subtask 04: Criar UseCase AuthenticateAdminUseCase](./subtask/Subtask-04-Criar_UseCase_AuthenticateAdmin.md)
- [Subtask 05: Criar Controller AdminController com endpoint login](./subtask/Subtask-05-Criar_Controller_AdminController.md)
- [Subtask 06: Configurar variáveis de ambiente Cognito](./subtask/Subtask-06-Configurar_variaveis_ambiente_Cognito.md)
- [Subtask 07: Registrar serviços e criar testes unitários](./subtask/Subtask-07-Registrar_servicos_testes.md)

## Critérios de Aceite da História

- [ ] Pacote AWSSDK.CognitoIdentityProvider adicionado
- [ ] Port ICognitoService criado na Application
- [ ] CognitoService implementado usando AdminInitiateAuthRequest
- [ ] UseCase AuthenticateAdminUseCase criado e funcionando
- [ ] Endpoint POST `/admin/login` criado no AdminController
- [ ] Endpoint retorna AccessToken, IdToken, ExpiresIn, TokenType
- [ ] Endpoint retorna 401 quando credenciais inválidas
- [ ] Configurações Cognito via variáveis de ambiente (COGNITO__REGION, COGNITO__USERPOOLID, COGNITO__CLIENTID)
- [ ] Testes unitários criados e passando (com mock do Cognito)
- [ ] Endpoint documentado no Swagger
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

