using System.Diagnostics.CodeAnalysis;

namespace Assignment.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        
    }
    [ExcludeFromCodeCoverage]
    public class InvalidcredentialsException : Exception
    {
        public InvalidcredentialsException(string message) : base(message)
        {
        }
      
    }
}
