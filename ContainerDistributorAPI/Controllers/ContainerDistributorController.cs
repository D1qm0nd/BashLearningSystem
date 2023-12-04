using System.Text;
using ContainerDistributorAPI.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContainerDistributorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainerDistributorApiController : ControllerBase //, IDisposable
{
    private readonly ILogger<ContainerDistributorApiController> _logger;

    private readonly DockerClient _dockerClient;
    private readonly ImageData _image;
    private readonly Task TrackTask;
    private readonly ContainerParametersAgent _containerParametersAgent;
    private readonly ContainersLifeCycleObject _containersLifeCycleObject;

    public ContainerDistributorApiController(ILogger<ContainerDistributorApiController> logger,
        ContainersLifeCycleObject containersLifeCycleObject,
        DockerClient dockerClient,
        ImageData image, ContainerParametersAgent containerParametersAgent)
    {
        _logger = logger;
        _dockerClient = dockerClient;
        _image = image;
        _containerParametersAgent = containerParametersAgent;
        _containersLifeCycleObject = containersLifeCycleObject;
        TrackTask = TrackContainers(1);
        TrackTask.WaitAsync(new CancellationToken());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delay">in minutes</param>
    private async Task TrackContainers(double delay = 0.0)
    {
        await Task.Factory.StartNew(action: () =>
        {
            var minuteDelay = 1000.0 * 60.0 * delay;
            while (true)
            {
                var toCloseAndRemove =
                    _containersLifeCycleObject.Where(e => e.Item3.AddMilliseconds(minuteDelay) <= DateTime.UtcNow).ToList();
                Parallel.ForEachAsync(toCloseAndRemove, (item, _) =>
                {
                    _containersLifeCycleObject.Remove(item);
                    var res = RemoveContainer(item.Item1).Result;
                    if (res) _logger.LogInformation($"Remove container {_image.Image}_{item.Item1} SUCCESS");
                    else _logger.LogError($"Remove container {_image.Image}_{item.Item1} FAILED ");
                    return default;
                });
                Thread.Sleep(10000);
            }
        });
    }

    #region HTTP Methods

    [HttpGet("generate-guid")]
    public Guid GenerateGUID() => Guid.NewGuid();

    [HttpPost("container-create")]
    public async Task<CreateContainerResponse?> CreateContainerForUser([FromBody] Guid id)
    {
        var containerResponse = await _dockerClient.Containers.CreateContainerAsync(
            parameters: _containerParametersAgent.CreateParameters(_image,
                id.ToString()));
        if (containerResponse != null)
            _containersLifeCycleObject.Add(new ContainerLifeCycleObject(id, containerResponse, DateTime.UtcNow));
        else if (containerResponse == null)
            _logger.LogError("Failed to create container for user {id}", id);
        return containerResponse;
    }

    private Task<MultiplexedStream> AttachContainerAsync(string containerId) =>
        _dockerClient.Containers.AttachContainerAsync(
            id: containerId,
            parameters: _containerParametersAgent.AttachParameters(),
            tty: true, //false - если true, то выводит то что ввели, но не выключает контейнер
            cancellationToken: new CancellationToken());

    [HttpPost("container-execute")]
    public async Task<ExecData?> ExecuteCommandInContainer([FromBody] ExecContainerCommand data)
    {
        if (!await ContainerExists(data.ID))
            if (await CreateContainerForUser(data.ID) == null)
            {
                return null;
            }
        
        var execResult = new ExecData(data.ID, data.Command);
        var containerId = $"{_image.Image}_{data.ID}";

        await StartContainer(data.ID);
        _logger.LogInformation("Attach Container: {containerId}", containerId);
        using (var containerInputOutputStream = await AttachContainerAsync(containerId))
        {
            var sendCommand = Encoding.UTF8.GetBytes(data.Command);

            await containerInputOutputStream.WriteAsync(
                buffer: sendCommand,
                offset: 0,
                count: sendCommand.Length,
                cancellationToken: new CancellationToken());

            containerInputOutputStream.CloseWrite();

            var cancellationToken = new CancellationToken();
            var sb = new StringBuilder();

            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[4096];
                var answ = await containerInputOutputStream.ReadOutputAsync(buffer, 0, buffer.Length,
                    cancellationToken);
                switch (answ.Target)
                {
                    case MultiplexedStream.TargetStream.StandardIn:
                        _logger.LogInformation("READ INPUT");
                        break;
                    case MultiplexedStream.TargetStream.StandardOut:
                        _logger.LogInformation("READ OUTPUT");
                        var answer = Encoding.UTF8.GetString(buffer);
                        _logger.LogInformation("READ BUFFER: {answer}", answer);
                        sb.Append(answer);
                        break;

                    case MultiplexedStream.TargetStream.StandardError:
                        _logger.LogInformation("READ ERROR");
                        break;
                }

                if (answ.EOF) break;
            }
            execResult.Result = sb.ToString()
                .Remove(0,1)
                .Replace($"{'\u0000'}", "")
                .Replace($"{'\u0001'}", "")
                .Replace($"{'\u0004'}","")
                .Replace($"{'\u0005'}","").Replace("\ufffd","");
            return execResult;
        }
    }

    [HttpPost("container-start")]
    public async Task<bool> StartContainer([FromBody] Guid id)
    {
        _logger.LogInformation("Container {id} start", id);
        var res = await _dockerClient.Containers.StartContainerAsync(
            id: $"{_image.Image}_{id}",
            parameters: new ContainerStartParameters
            {
            },
            cancellationToken: new CancellationToken());
        if (res) _logger.LogInformation("Container {id} start [SUCCESS]", id);
        else _logger.LogWarning("Container {id} start [FAILED]", id);
        return res;
    }

    [HttpDelete("container-remove")]
    public async Task<bool> RemoveContainer([FromBody] Guid id)
    {
        try
        {
            await _dockerClient.Containers.RemoveContainerAsync(
                id: $"{_image.Image}_{id}",
                parameters: new ContainerRemoveParameters(),
                cancellationToken: new CancellationToken());
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpPost("container-exists")]
    public async Task<bool> ContainerExists([FromBody] Guid id)
    {
        return _containersLifeCycleObject.Any(container => container.Item1 == id);
    }

    [HttpPost("container-stop")]
    public async Task<bool> StopContainerAsync([FromBody] Guid id) => await _dockerClient.Containers
        .StopContainerAsync(
            $"{_image.Image}_{id}",
            _containerParametersAgent.StopParameters(),
            new CancellationToken());

    #endregion

    #region Methods

    #endregion
}