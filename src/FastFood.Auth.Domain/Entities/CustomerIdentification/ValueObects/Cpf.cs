using FastFood.Auth.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace FastFood.Auth.Domain.Entities.CustomerIdentification.ValueObects
{
    public class Cpf
    {
        public string? Value { get; private set; }
        protected Cpf() { } // Para EF Core

        public Cpf(string value)
        {
            DomainValidation.ThrowIfNullOrWhiteSpace(value, "CPF is required.");

            value = Regex.Replace(value, "[^0-9]", ""); // remove pontuação

            DomainValidation.ThrowIf(value.Length != 11, "CPF must have 11 digits.");
            DomainValidation.ThrowIf(!IsValidCpf(value), "Invalid CPF.");

            Value = value;
        }

        private bool IsValidCpf(string cpf)
        {
            if (cpf.All(c => c == cpf[0]))
                return false; // evita CPFs com todos dígitos iguais (111.111.111-11)

            var tempCpf = cpf.Substring(0, 9);
            var firstDigit = CalculateDigit(tempCpf);
            var secondDigit = CalculateDigit(tempCpf + firstDigit);

            return cpf.EndsWith(firstDigit.ToString() + secondDigit.ToString());
        }

        private static int CalculateDigit(string cpfPartial)
        {
            var sum = 0;
            for (int i = 0; i < cpfPartial.Length; i++)
            {
                sum += int.Parse(cpfPartial[i].ToString()) * ((cpfPartial.Length + 1) - i);
            }

            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        public override string ToString()
            => Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00");
    }
}
