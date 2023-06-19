using System.Diagnostics.CodeAnalysis;

namespace Assignment.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidRequestBodyException : Exception
    {
        public string[] Errors { get; set; }
    }
}
