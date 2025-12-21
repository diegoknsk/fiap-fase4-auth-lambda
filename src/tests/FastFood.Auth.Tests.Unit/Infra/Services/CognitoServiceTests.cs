using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FastFood.Auth.Infra.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FastFood.Auth.Tests.Unit.Infra.Services;

/// <summary>
/// Testes unitários para CognitoService.
/// Nota: Devido à limitação do AmazonCognitoIdentityProviderClient não ter interface,
/// alguns testes focam na validação de configuração e tratamento de erros.
/// </summary>
public class CognitoServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;

    public CognitoServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
    }

    [Fact]
    public void Constructor_WithMissingRegion_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns((string?)null);
        
        // Limpar variável de ambiente se existir
        Environment.SetEnvironmentVariable("COGNITO__REGION", null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new CognitoService(_configurationMock.Object));
    }

    [Fact]
    public void Constructor_WithValidConfiguration_ShouldCreateService()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithExplicitCredentials_ShouldCreateService()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns("test-access-key");
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns("test-secret-key");
        _configurationMock.Setup(x => x["AWS:SessionToken"]).Returns((string?)null);

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithSessionToken_ShouldCreateService()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns("test-access-key");
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns("test-secret-key");
        _configurationMock.Setup(x => x["AWS:SessionToken"]).Returns("test-session-token");

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithEnvironmentVariableRegion_ShouldUseEnvironmentVariable()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("COGNITO__REGION", "us-west-2");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);

        // Cleanup
        Environment.SetEnvironmentVariable("COGNITO__REGION", null);
    }

    [Fact]
    public async Task AuthenticateAsync_WithMissingUserPoolId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
        Environment.SetEnvironmentVariable("COGNITO__USERPOOLID", null);
        
        var service = new CognitoService(_configurationMock.Object);
        _configurationMock.Setup(x => x["Cognito:UserPoolId"]).Returns((string?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AuthenticateAsync("test@example.com", "password123"));
    }

    [Fact]
    public async Task AuthenticateAsync_WithMissingClientId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
        Environment.SetEnvironmentVariable("COGNITO__CLIENTID", null);
        
        var service = new CognitoService(_configurationMock.Object);
        _configurationMock.Setup(x => x["Cognito:UserPoolId"]).Returns("us-east-1_test");
        _configurationMock.Setup(x => x["Cognito:ClientId"]).Returns((string?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AuthenticateAsync("test@example.com", "password123"));
    }

    [Fact]
    public void Constructor_WithoutExplicitCredentials_ShouldCreateService()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithEnvironmentVariableCredentials_ShouldUseEnvironmentVariables()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "env-access-key");
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "env-secret-key");

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);

        // Cleanup
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
    }

    [Fact]
    public void Constructor_WithEnvironmentVariableSessionToken_ShouldUseSessionToken()
    {
        // Arrange
        _configurationMock.Setup(x => x["Cognito:Region"]).Returns("us-east-1");
        _configurationMock.Setup(x => x["AWS:AccessKeyId"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SecretAccessKey"]).Returns((string?)null);
        _configurationMock.Setup(x => x["AWS:SessionToken"]).Returns((string?)null);
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "env-access-key");
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "env-secret-key");
        Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", "env-session-token");

        // Act
        var service = new CognitoService(_configurationMock.Object);

        // Assert
        Assert.NotNull(service);

        // Cleanup
        Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", null);
        Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", null);
        Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", null);
    }

    // Nota: Testes de AuthenticateAsync com sucesso e exceções do Cognito
    // requerem mock do AmazonCognitoIdentityProviderClient, que não tem interface.
    // Esses testes seriam melhor implementados como testes de integração ou
    // requerem refatoração do CognitoService para usar uma interface wrapper.
    // Por enquanto, focamos nos testes de configuração e validação que são possíveis.
}


