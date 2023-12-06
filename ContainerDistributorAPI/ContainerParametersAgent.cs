using ContainerDistributorAPI.Models;
using Docker.DotNet.Models;

namespace ContainerDistributorAPI;

public class ContainerParametersAgent
{
    public Func<ImageData, string, CreateContainerParameters> CreateParameters;
    public Func<ContainerAttachParameters> AttachParameters;
    public Func<ContainerStopParameters> StopParameters;
    public Func<ContainerRemoveParameters> RemoveParameters;

    public ContainerParametersAgent(Func<ImageData, string, CreateContainerParameters> createFunc,
        Func<ContainerAttachParameters> attachFunc,
        Func<ContainerStopParameters> stopFunc, Func<ContainerRemoveParameters> removeFunc)
    {
        CreateParameters = createFunc;
        AttachParameters = attachFunc;
        StopParameters = stopFunc;
        RemoveParameters = removeFunc;
    }
}