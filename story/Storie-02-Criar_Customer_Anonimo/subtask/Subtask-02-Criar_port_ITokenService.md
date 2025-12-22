# Subtask 02: Criar port ITokenService na Application

## Descrição
Criar interface ITokenService na camada Application que define o contrato para geração de tokens JWT, seguindo o padrão de ports da Clean Architecture.

## Passos de implementação
- Criar arquivo `src/FastFood.Auth.Application/Ports/ITokenService.cs`
- Criar interface `ITokenService` com método:
  - `string GenerateToken(Guid customerId, out DateTime expiresAt)`
- O método deve gerar token JWT e retornar data de expiração

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que a interface está acessível e bem definida
- Validar que não há referências a infraestrutura na interface

## Critérios de aceite
- Arquivo ITokenService.cs criado em `Application/Ports/`
- Interface define método GenerateToken com assinatura correta
- Método retorna string (token) e out DateTime (expiresAt)
- Interface não referencia nenhuma dependência de infraestrutura
- Código compila sem erros

