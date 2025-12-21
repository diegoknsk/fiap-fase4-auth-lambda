#language: pt-BR
@mytag
Funcionalidade: Admin Authentication
  Como um administrador
  Eu quero autenticar-me via AWS Cognito
  Para que eu possa obter tokens de acesso administrativo

  Cenário: Autenticar admin com credenciais válidas
    Dado que eu sou um administrador com credenciais válidas
    Quando eu faço uma requisição POST para "/api/admin/login" com username e password
    Então a resposta deve ter status 200
    E a resposta deve conter um AccessToken
    E a resposta deve conter um IdToken
    E a resposta deve conter ExpiresIn
    E a resposta deve conter TokenType "Bearer"

  Cenário: Tentar autenticar admin com credenciais inválidas
    Dado que eu tenho credenciais inválidas
    Quando eu faço uma requisição POST para "/api/admin/login" com credenciais inválidas
    Então a resposta deve ter status 401
    E a resposta deve indicar que as credenciais são inválidas

