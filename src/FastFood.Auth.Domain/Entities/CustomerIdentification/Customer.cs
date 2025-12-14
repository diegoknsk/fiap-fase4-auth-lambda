using FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects;

namespace FastFood.Auth.Domain.Entities.CustomerIdentification
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public Email? Email { get; private set; }
        public Cpf? Cpf { get; private set; }
        public CustomerTypeEnum CustomerType { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Customer()
        {

        }

        public Customer(Guid id, string? name, Email? email, Cpf? cpf, CustomerTypeEnum customerType)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            SetProperties(name, email, cpf, customerType);
        }

        public void AddCustomer(string? name, Email? email, Cpf? cpf, CustomerTypeEnum customerType)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            SetProperties(name, email, cpf, customerType);
        }

        private void SetProperties(string? name, Email? email, Cpf? cpf, CustomerTypeEnum customerType)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            CustomerType = customerType;
        }

    }
}
