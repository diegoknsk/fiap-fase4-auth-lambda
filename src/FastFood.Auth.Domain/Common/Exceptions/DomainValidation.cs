namespace FastFood.Auth.Domain.Exceptions
{
    public static class DomainValidation
    {
        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
            {
                throw new DomainException(message);
            }
        }

        public static void ThrowIfNull(object? value, string message)
        {
            if (value is null)
            {
                throw new DomainException(message);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(message);
            }
        }

        public static void ThrowIfLengthLessThan(string? value, int minLength, string message)
        {
            if (!string.IsNullOrEmpty(value) && value.Length < minLength)
                throw new DomainException(message);
        }

        public static void ThrowIfLessOrEqual(decimal value, decimal threshold, string message)
        {
            if (value <= threshold)
            {
                throw new DomainException(message);
            }
        }


    }
}
