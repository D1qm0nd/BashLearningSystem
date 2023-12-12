using System;
using System.Collections.Generic;
using Docker.DotNet.Models;

namespace ContainerDistributorAPI;

public class ContainersLifeCycleObject : List<ContainerLifeCycleObject>
{
}

public class ContainerLifeCycleObject : Tuple<Guid,CreateContainerResponse,DateTime>
{
    public ContainerLifeCycleObject(Guid item1, CreateContainerResponse item2, DateTime item3) : base(item1, item2, item3)
    {
    }
}