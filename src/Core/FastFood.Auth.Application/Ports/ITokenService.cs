namespace FastFood.Auth.Application.Ports;

/// <summary>
/// Port (interface) para serviço de geração de tokens JWT na camada Application.
/// Define o contrato para geração de tokens seguindo o padrão de ports da Clean Architecture.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token JWT para o customerId informado
    /// </summary>
    /// <param name="customerId">Id do customer</param>
    /// <param name="expiresAt">Data de expiração do token (out parameter)</param>
    /// <returns>Token JWT como string</returns>
    string GenerateToken(Guid customerId, out DateTime expiresAt);
}













