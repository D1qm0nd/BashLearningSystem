using ContainerDistributorAPI.Models;
using Docker.DotNet.Models;

namespace ContainerDistributorAPI;

public class ContainerParametersAgent
{
    public Func<ImageData, string, CreateContainerParameters> CreateParameters;
    public Func<ContainerAttachParameters> AttachParameters;
    public Func<ContainerStopParameters> StopParameters;
    public Func<ContainerRemoveParameters> RemoveParameters;
    public Func<ContainerRestartParameters> RestartParameters;
    public Func<ContainerLogsParameters> LogsParameter;

    public ContainerParametersAgent(Func<ImageData, string, CreateContainerParameters> createFunc,
        Func<ContainerAttachParameters> attachFunc,
        Func<ContainerStopParameters> stopFunc, Func<ContainerRemoveParameters> removeFunc, Func<ContainerRestartParameters> restartParameters, Func<ContainerLogsParameters> logsParameter)
    {
        CreateParameters = createFunc;
        AttachParameters = attachFunc;
        StopParameters = stopFunc;
        RemoveParameters = removeFunc;
        RestartParameters = restartParameters;
        LogsParameter = logsParameter;
    }

}