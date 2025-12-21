using TechTalk.SpecFlow;
using FastFood.Auth.Tests.Bdd.Support;
using FastFood.Auth.Infra.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FastFood.Auth.Tests.Bdd.Support;

/// <summary>
/// Hooks do SpecFlow para setup e teardown de testes.
/// </summary>
[Binding]
public class Hooks
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly ScenarioContext _scenarioContext;
    private IServiceScope? _scope;
    private AuthDbContext? _context;

    public Hooks(WebApplicationFactoryFixture factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        // Usar o mesmo contexto que a aplicação usa
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        // Garantir que o banco está criado (não limpar para evitar problemas de compartilhamento)
        _context.Database.EnsureCreated();
        _scenarioContext["DbContext"] = _context;
        _scenarioContext["Scope"] = _scope;
    }

    [AfterScenario]
    public void AfterScenario()
    {
        // Limpar dados do banco em memória após cada cenário
        if (_context != null)
        {
            // Remover todos os customers
            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();
            _context.Dispose();
        }
        _scope?.Dispose();
        _scenarioContext.Remove("DbContext");
        _scenarioContext.Remove("Scope");
    }
}

