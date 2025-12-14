# Subtask 04: Implementar TokenService na Infra

## Descrição
Implementar classe TokenService que implementa ITokenService para gerar tokens JWT com as claims necessárias (customerId, sub, jti, iat) usando configurações do appsettings.json.

## Passos de implementação
- Criar projeto `FastFood.Auth.Infra` (class library) se não existir
- Criar diretório `src/FastFood.Auth.Infra/Services/`
- Criar arquivo `src/FastFood.Auth.Infra/Services/TokenService.cs`
- Criar classe `TokenService` implementando `ITokenService`
- Adicionar campo privado `_configuration` do tipo `IConfiguration` (injetado via construtor)
- Implementar método `GenerateToken`:
  - Ler configurações de JwtSettings do appsettings (Secret, Issuer, Audience, ExpirationHours)
  - Criar claims: sub (customerId), customerId (Guid), jti (Guid), iat (Unix timestamp)
  - Criar SecurityTokenDescriptor com SigningCredentials usando HmacSha256
  - Gerar token usando JwtSecurityTokenHandler
  - Calcular expiresAt baseado em ExpirationHours
- Adicionar pacote `System.IdentityModel.Tokens.Jwt` e `Microsoft.Extensions.Configuration` se necessário
- Adicionar usings necessários

## Como testar
- Executar `dotnet build` no projeto (deve compilar sem erros)
- Verificar que a classe implementa corretamente a interface ITokenService
- Validar que o método GenerateToken está implementado

## Critérios de aceite
- Projeto `FastFood.Auth.Infra` criado (class library)
- Arquivo TokenService.cs criado em `Infra/Services/`
- Classe implementa ITokenService
- Construtor recebe IConfiguration via DI
- Método GenerateToken implementado
- Token gerado contém claims: sub, customerId, jti, iat
- Configurações lidas de JwtSettings no appsettings.json
- Código compila sem erros

## Nota
⚠️ **Atualização**: Esta subtask foi refatorada na Subtask 09. O TokenService foi movido de `Infra.Persistence` para o projeto dedicado `FastFood.Auth.Infra`, seguindo melhor separação de responsabilidades.

