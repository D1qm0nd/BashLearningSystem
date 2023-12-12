using System;

namespace ContainerDistributorAPI;

public class ContainerExistingException : Exception
{
    public ContainerExistingException(string? message = null) : base(message)
    {
    }
}

