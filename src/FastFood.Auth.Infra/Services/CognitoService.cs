using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FastFood.Auth.Application.Ports;
using Microsoft.Extensions.Configuration;

namespace FastFood.Auth.Infra.Services;

/// <summary>
/// Implementação do serviço de autenticação via AWS Cognito.
/// </summary>
public class CognitoService : ICognitoService
{
    private readonly IConfiguration _configuration;
    private readonly AmazonCognitoIdentityProviderClient _cognitoClient;

    public CognitoService(IConfiguration configuration)
    {
        _configuration = configuration;

        // Obter região do Cognito
        var region = _configuration["Cognito:Region"] 
            ?? Environment.GetEnvironmentVariable("COGNITO__REGION")
            ?? throw new InvalidOperationException("Cognito Region não configurado");

        // Criar cliente do Cognito
        var regionEndpoint = RegionEndpoint.GetBySystemName(region);
        _cognitoClient = new AmazonCognitoIdentityProviderClient(regionEndpoint);
    }

    public async Task<AuthenticateAdminResult> AuthenticateAsync(string username, string password)
    {
        try
        {
            // Obter configurações do Cognito
            var userPoolId = _configuration["Cognito:UserPoolId"] 
                ?? Environment.GetEnvironmentVariable("COGNITO__USERPOOLID")
                ?? throw new InvalidOperationException("Cognito UserPoolId não configurado");

            var clientId = _configuration["Cognito:ClientId"] 
                ?? Environment.GetEnvironmentVariable("COGNITO__CLIENTID")
                ?? throw new InvalidOperationException("Cognito ClientId não configurado");

            // Criar requisição de autenticação
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = userPoolId,
                ClientId = clientId,
                AuthFlow = AuthFlowType.ADMIN_USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "PASSWORD", password }
                }
            };

            // Executar autenticação
            var response = await _cognitoClient.AdminInitiateAuthAsync(request);

            // Verificar se a autenticação foi bem-sucedida
            if (response.AuthenticationResult == null)
            {
                throw new UnauthorizedAccessException("Autenticação falhou: resposta inválida do Cognito");
            }

            // Retornar resultado
            return new AuthenticateAdminResult
            {
                AccessToken = response.AuthenticationResult.AccessToken ?? string.Empty,
                IdToken = response.AuthenticationResult.IdToken ?? string.Empty,
                ExpiresIn = (int)(response.AuthenticationResult.ExpiresIn ?? 3600),
                TokenType = response.AuthenticationResult.TokenType ?? "Bearer"
            };
        }
        catch (NotAuthorizedException)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }
        catch (UserNotFoundException)
        {
            throw new UnauthorizedAccessException("Usuário não encontrado");
        }
        catch (InvalidPasswordException)
        {
            throw new UnauthorizedAccessException("Senha inválida");
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Erro ao autenticar no Cognito: {ex.Message}", ex);
        }
    }
}

