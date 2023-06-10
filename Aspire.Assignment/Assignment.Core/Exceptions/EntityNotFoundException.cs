namespace Assignment.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        
    }

    public class InvalidcredentialsException : Exception
    {
        public InvalidcredentialsException(string message) : base(message)
        {
        }
      
    }
}
