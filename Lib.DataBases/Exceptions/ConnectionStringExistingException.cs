namespace Lib.DataBases.Exceptions;

public class ConnectionStringExistingException : Exception
{
    public ConnectionStringExistingException(string? message = null) : base(message)
    {
    }
}