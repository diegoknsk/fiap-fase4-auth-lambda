using System.Runtime.Serialization;

namespace FastFood.Auth.Domain.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException() { }
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
        
        [Obsolete("This API supports obsolete formatter-based serialization")]
        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
