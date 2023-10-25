using System.Runtime.Serialization;

namespace Site.Exceptions;

public class EnvironmentVariableExistingException : Exception
{
    public EnvironmentVariableExistingException(string? message) : base(message)
    {
        
    }
}