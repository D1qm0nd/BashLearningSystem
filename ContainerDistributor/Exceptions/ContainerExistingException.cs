namespace ContainerDistributor;

public class ContainerExistingException : Exception
{
    public ContainerExistingException(string? message = null) : base(message)
    {
    }
}

