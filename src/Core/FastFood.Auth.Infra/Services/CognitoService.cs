using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
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

        var regionEndpoint = RegionEndpoint.GetBySystemName(region);

        // Verificar se há credenciais AWS explícitas (para desenvolvimento local)
        var accessKeyId = _configuration["AWS:AccessKeyId"] 
            ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        
        var secretAccessKey = _configuration["AWS:SecretAccessKey"] 
            ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

        var sessionToken = _configuration["AWS:SessionToken"] 
            ?? Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");

        // Criar cliente do Cognito com ou sem credenciais explícitas
        if (!string.IsNullOrEmpty(accessKeyId) && !string.IsNullOrEmpty(secretAccessKey))
        {
            AWSCredentials credentials;
            
            // Se houver SessionToken, usar credenciais temporárias (AWS Academy, STS, etc.)
            if (!string.IsNullOrEmpty(sessionToken))
            {
                credentials = new SessionAWSCredentials(accessKeyId, secretAccessKey, sessionToken);
            }
            else
            {
                // Usar credenciais permanentes (BasicAWSCredentials)
                credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            }
            
            _cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, regionEndpoint);
        }
        else
        {
            // Usar cadeia de credenciais padrão (Lambda/EC2 com IAM Role)
            // O SDK automaticamente buscará credenciais de:
            // 1. Variáveis de ambiente (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, AWS_SESSION_TOKEN)
            // 2. Perfil AWS (~/.aws/credentials)
            // 3. IAM Role (se executando em EC2/Lambda)
            _cognitoClient = new AmazonCognitoIdentityProviderClient(regionEndpoint);
        }
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
                ExpiresIn = response.AuthenticationResult.ExpiresIn ?? 3600,
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
        catch (AmazonCognitoIdentityProviderException ex) when (ex.Message.Contains("expired") || ex.Message.Contains("token"))
        {
            throw new InvalidOperationException(
                "Erro ao autenticar no Cognito: Credenciais AWS expiradas. " +
                "Configure AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY ou renove as credenciais temporárias.", ex);
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Erro ao autenticar no Cognito: {ex.Message}", ex);
        }
    }
}

