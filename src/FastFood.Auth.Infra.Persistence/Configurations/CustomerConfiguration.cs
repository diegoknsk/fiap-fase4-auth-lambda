using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FastFood.Auth.Infra.Persistence.Entities;

namespace FastFood.Auth.Infra.Persistence.Configurations
{
    /// <summary>
    /// Configuração de mapeamento da entidade CustomerEntity para a tabela Customers no PostgreSQL.
    /// CustomerEntity já possui propriedades como strings, então não precisa conversão de Value Objects
    /// (isso será feito no repositório ao mapear entre Domain e Infra).
    /// </summary>
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            // Configurar nome da tabela
            builder.ToTable("Customers");

            // Configurar chave primária
            builder.HasKey(c => c.Id);

            // Configurar propriedades
            builder.Property(c => c.Name)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(c => c.Cpf)
                .HasMaxLength(11)
                .IsRequired(false);

            builder.Property(c => c.CustomerType)
                .IsRequired(true);

            builder.Property(c => c.CreatedAt)
                .IsRequired(true);
        }
    }
}





