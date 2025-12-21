namespace FastFood.Auth.Infra.Persistence.Entities
{
    /// <summary>
    /// Entidade de persistência para Customer.
    /// Esta classe representa a estrutura da tabela no banco de dados PostgreSQL.
    /// É separada da entidade de domínio Customer que utiliza Value Objects (Email, Cpf).
    /// O repositório será responsável por fazer o mapeamento entre Customer (Domain) e CustomerEntity (Infra).
    /// </summary>
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// Nome do customer (nullable, varchar 500)
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Email do customer (nullable, varchar 255) - armazenado como string
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// CPF do customer (nullable, varchar 11) - armazenado como string
        /// </summary>
        public string? Cpf { get; set; }
        
        /// <summary>
        /// Tipo do customer (1 = Registered, 2 = Anonymous)
        /// </summary>
        public int CustomerType { get; set; }
        
        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}











