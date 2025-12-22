using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FastFood.Auth.Application.InputModels.Admin;
using FastFood.Auth.Application.OutputModels.Admin;
using FastFood.Auth.Application.Ports;
using FastFood.Auth.Tests.Bdd.Support;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechTalk.SpecFlow;
using AuthenticateAdminResult = FastFood.Auth.Application.Ports.AuthenticateAdminResult;

namespace FastFood.Auth.Tests.Bdd.StepDefinitions;

/// <summary>
/// Step definitions para testes BDD de autenticação de admin.
/// </summary>
[Binding]
public class AdminAuthenticationSteps
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly ScenarioContext _scenarioContext;
    private HttpClient? _httpClient;
    private HttpResponseMessage? _response;
    private string? _username;
    private string? _password;

    public AdminAuthenticationSteps(WebApplicationFactoryFixture factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }

    [Given(@"que eu sou um administrador com credenciais válidas")]
    public void GivenQueEuSouUmAdministradorComCredenciaisValidas()
    {
        _username = "admin@test.com";
        _password = "ValidPassword123!";
        _httpClient = _factory.CreateClient();
        _scenarioContext["HttpClient"] = _httpClient;

        // Configurar mock do CognitoService para retornar tokens válidos
        var cognitoServiceMock = _factory.Services.GetRequiredService<Mock<ICognitoService>>();
        cognitoServiceMock.Reset();
        cognitoServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new AuthenticateAdminResult
            {
                AccessToken = "valid-access-token",
                IdToken = "valid-id-token",
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
    }

    [Given(@"que eu tenho credenciais inválidas")]
    public void GivenQueEuTenhoCredenciaisInvalidas()
    {
        _username = "invalid@test.com";
        _password = "InvalidPassword";
        _httpClient = _factory.CreateClient();
        _scenarioContext["HttpClient"] = _httpClient;

        // Configurar mock do CognitoService para lançar exceção
        var cognitoServiceMock = _factory.Services.GetRequiredService<Mock<ICognitoService>>();
        cognitoServiceMock.Reset();
        cognitoServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));
    }

    [When(@"eu faço uma requisição POST para ""(.*)"" com username e password")]
    public async Task WhenEuFacoUmaRequisicaoPostParaComUsernameEPassword(string endpoint)
    {
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        if (_httpClient == null)
            throw new InvalidOperationException("HttpClient não foi inicializado");
        if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            throw new InvalidOperationException("Username ou password não foram definidos");

        var inputModel = new AuthenticateAdminInputModel
        {
            Username = _username,
            Password = _password
        };

        _response = await _httpClient.PostAsJsonAsync(endpoint, inputModel);
        _scenarioContext["Response"] = _response;
    }

    [When(@"eu faço uma requisição POST para ""(.*)"" com credenciais inválidas")]
    public async Task WhenEuFacoUmaRequisicaoPostParaComCredenciaisInvalidas(string endpoint)
    {
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        if (_httpClient == null)
            throw new InvalidOperationException("HttpClient não foi inicializado");
        if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            throw new InvalidOperationException("Username ou password não foram definidos");

        var inputModel = new AuthenticateAdminInputModel
        {
            Username = _username,
            Password = _password
        };

        _response = await _httpClient.PostAsJsonAsync(endpoint, inputModel);
        _scenarioContext["Response"] = _response;
    }

    [Then(@"a resposta deve conter um AccessToken")]
    public async Task ThenARespostaDeveConterUmAccessToken()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<AuthenticateAdminOutputModel>(content, options);

        Assert.NotNull(responseModel);
        Assert.False(string.IsNullOrWhiteSpace(responseModel.AccessToken));
    }

    [Then(@"a resposta deve conter um IdToken")]
    public async Task ThenARespostaDeveConterUmIdToken()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<AuthenticateAdminOutputModel>(content, options);

        Assert.NotNull(responseModel);
        Assert.False(string.IsNullOrWhiteSpace(responseModel.IdToken));
    }

    [Then(@"a resposta deve conter ExpiresIn")]
    public async Task ThenARespostaDeveConterExpiresIn()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<AuthenticateAdminOutputModel>(content, options);

        Assert.NotNull(responseModel);
        Assert.True(responseModel.ExpiresIn > 0);
    }

    [Then(@"a resposta deve conter TokenType ""(.*)""")]
    public async Task ThenARespostaDeveConterTokenType(string expectedTokenType)
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<AuthenticateAdminOutputModel>(content, options);

        Assert.NotNull(responseModel);
        Assert.Equal(expectedTokenType, responseModel.TokenType);
    }

    [Then(@"a resposta deve indicar que as credenciais são inválidas")]
    public async Task ThenARespostaDeveIndicarQueAsCredenciaisSaoInvalidas()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        Assert.Contains("inválidas", content, StringComparison.OrdinalIgnoreCase);
    }
}

