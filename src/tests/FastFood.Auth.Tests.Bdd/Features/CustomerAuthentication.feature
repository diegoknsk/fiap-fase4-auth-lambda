#language: pt-BR
@mytag
Funcionalidade: Customer Authentication
  Como um cliente
  Eu quero criar, registrar e identificar um customer
  Para que eu possa obter tokens de autenticação

  Cenário: Registrar customer com CPF válido
    Dado que eu tenho um CPF válido "11144477735"
    Quando eu faço uma requisição POST para "/api/customer/register" com o CPF
    Então a resposta deve ter status 200
    E a resposta deve conter um token JWT válido
    E a resposta deve conter um customerId (Guid)
    E um customer deve ser criado no banco com CustomerType = Registered
    E o customer deve ter o CPF informado

  Cenário: Registrar customer com CPF já existente
    Dado que já existe um customer com CPF "11144477735"
    Quando eu faço uma requisição POST para "/api/customer/register" com o mesmo CPF
    Então a resposta deve ter status 200
    E a resposta deve conter um token JWT válido
    E a resposta deve conter o customerId do customer existente
    E nenhum customer duplicado deve ser criado

  Cenário: Identificar customer existente por CPF
    Dado que existe um customer registrado com CPF "11144477735"
    Quando eu faço uma requisição POST para "/api/customer/identify" com o CPF
    Então a resposta deve ter status 200
    E a resposta deve conter um token JWT válido
    E a resposta deve conter o customerId do customer existente

  Cenário: Tentar identificar customer inexistente
    Dado que não existe nenhum customer com CPF "12345678909"
    Quando eu faço uma requisição POST para "/api/customer/identify" com o CPF
    Então a resposta deve ter status 401
    E a resposta deve indicar que o customer não foi encontrado

