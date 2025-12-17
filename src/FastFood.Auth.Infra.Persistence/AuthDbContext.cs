using Microsoft.EntityFrameworkCore;
using FastFood.Auth.Infra.Persistence.Entities;
using FastFood.Auth.Infra.Persistence.Configurations;

namespace FastFood.Auth.Infra.Persistence
{
    /// <summary>
    /// DbContext para o módulo de autenticação.
    /// Trabalha com entidades de persistência (CustomerEntity), não com entidades de domínio (Customer).
    /// </summary>
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet para a entidade CustomerEntity (persistência)
        /// </summary>
        public DbSet<CustomerEntity> Customers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Aplicar configurações de mapeamento
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}






