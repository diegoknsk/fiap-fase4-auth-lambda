using FastFood.Auth.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects
{
    public partial class Email
    {
        public string Value { get; private set; } = string.Empty;

        protected Email() { } // Requerido pelo EF Core

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();

        public Email(string value)
        {
            DomainValidation.ThrowIfNullOrWhiteSpace(value, "Email is required.");

            if (!IsValidEmail(value))
                throw new DomainException("Invalid email address.");

            Value = value;
        }

        private static bool IsValidEmail(string email)
        {
            return EmailRegex().IsMatch(email);
        }

        public override string ToString() => Value;
    }
}
