using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using FastFood.Auth.Application.InputModels.Customer;
using FastFood.Auth.Application.OutputModels.Customer;
using FastFood.Auth.Domain.Entities.CustomerIdentification;
using FastFood.Auth.Infra.Persistence;
using FastFood.Auth.Infra.Persistence.Entities;
using FastFood.Auth.Tests.Bdd.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace FastFood.Auth.Tests.Bdd.StepDefinitions;

/// <summary>
/// Step definitions para testes BDD de autenticação de customer.
/// </summary>
[Binding]
public class CustomerAuthenticationSteps
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly ScenarioContext _scenarioContext;
    private HttpClient? _httpClient;
    private HttpResponseMessage? _response;
    private AuthDbContext? _context;
    private string? _cpf;
    private Guid? _existingCustomerId;

    public CustomerAuthenticationSteps(WebApplicationFactoryFixture factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }

    [Given(@"que eu sou um cliente")]
    public void GivenQueEuSouUmCliente()
    {
        _httpClient = _factory.CreateClient();
        _scenarioContext["HttpClient"] = _httpClient;
        _context = _scenarioContext.Get<AuthDbContext>("DbContext");
    }

    [Given(@"que eu tenho um CPF válido ""(.*)""")]
    public void GivenQueEuTenhoUmCpfValido(string cpf)
    {
        _cpf = cpf;
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        _context = _scenarioContext.Get<AuthDbContext>("DbContext");
    }

    [Given(@"que já existe um customer com CPF ""(.*)""")]
    public async Task GivenQueJaExisteUmCustomerComCpf(string cpf)
    {
        _cpf = cpf;
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        _context = _scenarioContext.Get<AuthDbContext>("DbContext");

        // Criar customer existente no banco
        _existingCustomerId = Guid.NewGuid();
        var customerEntity = new CustomerEntity
        {
            Id = _existingCustomerId.Value,
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customerEntity);
        await _context.SaveChangesAsync();
    }

    [Given(@"que existe um customer registrado com CPF ""(.*)""")]
    public async Task GivenQueExisteUmCustomerRegistradoComCpf(string cpf)
    {
        _cpf = cpf;
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        _context = _scenarioContext.Get<AuthDbContext>("DbContext");

        // Criar customer existente no banco
        _existingCustomerId = Guid.NewGuid();
        var customerEntity = new CustomerEntity
        {
            Id = _existingCustomerId.Value,
            Cpf = cpf,
            CustomerType = (int)CustomerType.Registered,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customerEntity);
        await _context.SaveChangesAsync();
    }

    [Given(@"que não existe nenhum customer com CPF ""(.*)""")]
    public void GivenQueNaoExisteNenhumCustomerComCpf(string cpf)
    {
        _cpf = cpf;
        if (!_scenarioContext.ContainsKey("HttpClient"))
        {
            _httpClient = _factory.CreateClient();
            _scenarioContext["HttpClient"] = _httpClient;
        }
        else
        {
            _httpClient = _scenarioContext.Get<HttpClient>("HttpClient");
        }
        _context = _scenarioContext.Get<AuthDbContext>("DbContext");
        // Garantir que não existe - banco em memória já está vazio
    }

    [When(@"eu faço uma requisição POST para ""(.*)""")]
    public async Task WhenEuFacoUmaRequisicaoPostPara(string endpoint)
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

        _response = await _httpClient.PostAsync(endpoint, null);
        _scenarioContext["Response"] = _response;
    }

    [When(@"eu faço uma requisição POST para ""(.*)"" com o CPF")]
    public async Task WhenEuFacoUmaRequisicaoPostParaComOCpf(string endpoint)
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
        if (string.IsNullOrEmpty(_cpf))
            throw new InvalidOperationException("CPF não foi definido");

        // Usar o modelo correto baseado no endpoint
        if (endpoint.Contains("/identify"))
        {
            var inputModel = new IdentifyCustomerInputModel { Cpf = _cpf };
            _response = await _httpClient.PostAsJsonAsync(endpoint, inputModel);
        }
        else
        {
            var inputModel = new RegisterCustomerInputModel { Cpf = _cpf };
            _response = await _httpClient.PostAsJsonAsync(endpoint, inputModel);
        }
        _scenarioContext["Response"] = _response;
    }

    [When(@"eu faço uma requisição POST para ""(.*)"" com o mesmo CPF")]
    public async Task WhenEuFacoUmaRequisicaoPostParaComOMesmoCpf(string endpoint)
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
        if (string.IsNullOrEmpty(_cpf))
            throw new InvalidOperationException("CPF não foi definido");

        var inputModel = new RegisterCustomerInputModel { Cpf = _cpf };
        _response = await _httpClient.PostAsJsonAsync(endpoint, inputModel);
        _scenarioContext["Response"] = _response;
    }

    [Then(@"a resposta deve ter status (\d+)")]
    public void ThenARespostaDeveTerStatus(int statusCode)
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        Assert.Equal((HttpStatusCode)statusCode, _response.StatusCode);
    }

    [Then(@"a resposta deve conter um token JWT válido")]
    public async Task ThenARespostaDeveConterUmTokenJwtValido()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        // Tentar deserializar para cada tipo possível
        CreateAnonymousCustomerOutputModel? anonymousModel = null;
        RegisterCustomerOutputModel? registerModel = null;
        IdentifyCustomerOutputModel? identifyModel = null;
        
        try { anonymousModel = JsonSerializer.Deserialize<CreateAnonymousCustomerOutputModel>(content, options); } catch { }
        try { registerModel = JsonSerializer.Deserialize<RegisterCustomerOutputModel>(content, options); } catch { }
        try { identifyModel = JsonSerializer.Deserialize<IdentifyCustomerOutputModel>(content, options); } catch { }
        
        var token = anonymousModel?.Token ?? registerModel?.Token ?? identifyModel?.Token;
        Assert.NotNull(token);
        Assert.False(string.IsNullOrWhiteSpace(token));
        
        // Validar formato básico de JWT (3 partes separadas por ponto)
        var parts = token.Split('.');
        Assert.Equal(3, parts.Length);
    }

    [Then(@"a resposta deve conter um customerId \(Guid\)")]
    public async Task ThenARespostaDeveConterUmCustomerIdGuid()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        // Tentar deserializar para cada tipo possível
        CreateAnonymousCustomerOutputModel? anonymousModel = null;
        RegisterCustomerOutputModel? registerModel = null;
        IdentifyCustomerOutputModel? identifyModel = null;
        
        try { anonymousModel = JsonSerializer.Deserialize<CreateAnonymousCustomerOutputModel>(content, options); } catch { }
        try { registerModel = JsonSerializer.Deserialize<RegisterCustomerOutputModel>(content, options); } catch { }
        try { identifyModel = JsonSerializer.Deserialize<IdentifyCustomerOutputModel>(content, options); } catch { }
        
        var customerId = anonymousModel?.CustomerId ?? registerModel?.CustomerId ?? identifyModel?.CustomerId;
        Assert.True(customerId.HasValue);
        Assert.NotEqual(Guid.Empty, customerId.Value);
    }

    [Then(@"a resposta deve conter o customerId do customer existente")]
    public async Task ThenARespostaDeveConterOCustomerIdDoCustomerExistente()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        Assert.NotNull(_existingCustomerId);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        RegisterCustomerOutputModel? registerModel = null;
        IdentifyCustomerOutputModel? identifyModel = null;
        
        try { registerModel = JsonSerializer.Deserialize<RegisterCustomerOutputModel>(content, options); } catch { }
        try { identifyModel = JsonSerializer.Deserialize<IdentifyCustomerOutputModel>(content, options); } catch { }
        
        var customerId = registerModel?.CustomerId ?? identifyModel?.CustomerId;
        Assert.True(customerId.HasValue);
        Assert.Equal(_existingCustomerId.Value, customerId.Value);
    }

    [Then(@"um customer deve ser criado no banco com CustomerType = Anonymous")]
    public async Task ThenUmCustomerDeveSerCriadoNoBancoComCustomerTypeAnonymous()
    {
        _response = _scenarioContext.ContainsKey("Response") 
            ? _scenarioContext.Get<HttpResponseMessage>("Response") 
            : _response;
        
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<CreateAnonymousCustomerOutputModel>(content, options);

        Assert.NotNull(responseModel);
        
        // Obter um novo contexto do serviço para garantir que vemos os dados salvos pela aplicação
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var customer = await context.Customers.FindAsync(responseModel.CustomerId);
        Assert.NotNull(customer);
        Assert.Equal((int)CustomerType.Anonymous, customer.CustomerType);
    }

    [Then(@"um customer deve ser criado no banco com CustomerType = Registered")]
    public async Task ThenUmCustomerDeveSerCriadoNoBancoComCustomerTypeRegistered()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<RegisterCustomerOutputModel>(content, options);

        Assert.NotNull(responseModel);
        
        // Obter um novo contexto do serviço para garantir que vemos os dados salvos pela aplicação
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var customer = await context.Customers.FindAsync(responseModel.CustomerId);
        Assert.NotNull(customer);
        Assert.Equal((int)CustomerType.Registered, customer.CustomerType);
    }

    [Then(@"o customer deve ter o CPF informado")]
    public async Task ThenOCustomerDeveTerOCpfInformado()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        _context = _scenarioContext.ContainsKey("DbContext")
            ? _scenarioContext.Get<AuthDbContext>("DbContext")
            : _context;
        Assert.NotNull(_response);
        Assert.NotNull(_context);
        Assert.NotNull(_cpf);
        var content = await _response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var responseModel = JsonSerializer.Deserialize<RegisterCustomerOutputModel>(content, options);

        Assert.NotNull(responseModel);
        var customer = await _context.Customers.FindAsync(responseModel.CustomerId);
        Assert.NotNull(customer);
        Assert.Equal(_cpf, customer.Cpf);
    }

    [Then(@"nenhum customer duplicado deve ser criado")]
    public async Task ThenNenhumCustomerDuplicadoDeveSerCriado()
    {
        _context = _scenarioContext.ContainsKey("DbContext")
            ? _scenarioContext.Get<AuthDbContext>("DbContext")
            : _context;
        Assert.NotNull(_context);
        Assert.NotNull(_cpf);
        var customersWithCpf = await _context.Customers
            .Where(c => c.Cpf == _cpf)
            .ToListAsync();
        
        Assert.Single(customersWithCpf);
    }

    [Then(@"a resposta deve indicar que o customer não foi encontrado")]
    public async Task ThenARespostaDeveIndicarQueOCustomerNaoFoiEncontrado()
    {
        _response = _scenarioContext.ContainsKey("Response")
            ? _scenarioContext.Get<HttpResponseMessage>("Response")
            : _response;
        Assert.NotNull(_response);
        var content = await _response.Content.ReadAsStringAsync();
        Assert.Contains("not found", content, StringComparison.OrdinalIgnoreCase);
    }
}

