using System;

namespace Exceptions;

public class EnvironmentVariableExistingException : Exception
{
    public EnvironmentVariableExistingException(string? message) : base(message)
    {
        
    }
}