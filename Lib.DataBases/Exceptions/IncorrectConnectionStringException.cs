namespace Lib.DataBases.Exceptions;

public class IncorrectConnectionStringException : Exception
{
    public IncorrectConnectionStringException(string? message = null) : base(message)
    {
    }
}