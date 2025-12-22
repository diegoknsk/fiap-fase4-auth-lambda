using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FastFood.Auth.Infra.Persistence;

namespace FastFood.Auth.Migrator
{
    /// <summary>
    /// Aplicativo console responsável por executar migrations do Entity Framework Core.
    /// Este projeto é executado separadamente da API para aplicar migrations no banco de dados.
    /// </summary>
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("FastFood Auth - Database Migrator");
            Console.WriteLine("=================================");
            Console.WriteLine();

            try
            {
                // Carregar configuração
                // Prioridade: Variáveis de Ambiente > appsettings.Development.json > appsettings.json
                // Usar AppContext.BaseDirectory para encontrar o diretório do executável
                var basePath = AppContext.BaseDirectory;
                
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables() // Variáveis de ambiente têm prioridade
                    .Build();

                // Priorizar variável de ambiente, depois configuração
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
                    ?? configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Console.WriteLine("ERRO: Connection string 'DefaultConnection' não encontrada.");
                    Console.WriteLine();
                    Console.WriteLine("Configure a connection string usando uma das opções:");
                    Console.WriteLine("  1. Variável de ambiente: ConnectionStrings__DefaultConnection");
                    Console.WriteLine("  2. Arquivo appsettings.Development.json (não commitado)");
                    Console.WriteLine("  3. Arquivo appsettings.json");
                    Console.WriteLine();
                    Environment.Exit(1);
                    return;
                }

                Console.WriteLine("Connection string encontrada.");
                Console.WriteLine($"Host: {ExtractHostFromConnectionString(connectionString)}");
                Console.WriteLine();

                // Configurar opções do DbContext
                var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
                optionsBuilder.UseNpgsql(connectionString);

                // Criar contexto e aplicar migrations
                using var context = new AuthDbContext(optionsBuilder.Options);

                Console.WriteLine("Verificando migrations pendentes...");
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"Encontradas {pendingMigrations.Count()} migration(s) pendente(s):");
                    foreach (var migration in pendingMigrations)
                    {
                        Console.WriteLine($"  - {migration}");
                    }
                    Console.WriteLine();

                    Console.WriteLine("Aplicando migrations...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine("✓ Migrations aplicadas com sucesso!");
                }
                else
                {
                    Console.WriteLine("✓ Nenhuma migration pendente. Banco de dados está atualizado.");
                }

                // Listar migrations aplicadas
                Console.WriteLine();
                Console.WriteLine("Migrations aplicadas no banco de dados:");
                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                foreach (var migration in appliedMigrations)
                {
                    Console.WriteLine($"  ✓ {migration}");
                }

                Console.WriteLine();
                Console.WriteLine("Operação concluída com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("ERRO ao executar migrations:");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine("Stack trace:");
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }

        private static string ExtractHostFromConnectionString(string connectionString)
        {
            var parts = connectionString.Split(';');
            var hostPart = parts.FirstOrDefault(p => p.StartsWith("Host=", StringComparison.OrdinalIgnoreCase));
            return hostPart?.Substring(5) ?? "N/A";
        }
    }
}
