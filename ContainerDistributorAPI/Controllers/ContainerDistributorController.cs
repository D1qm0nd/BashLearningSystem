using System.Text;
using ContainerDistributorAPI.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContainerDistributorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainerDistributorApiController : ControllerBase
{
    private readonly ILogger<ContainerDistributorApiController> _logger;

    private readonly DockerClient _dockerClient;
    private readonly ImageData _image;
    private readonly EnvironmentVariables _environmentVariables;
    private readonly ContainerParametersAgent _containerParametersAgent;

    public ContainerDistributorApiController(ILogger<ContainerDistributorApiController> logger,
        DockerClient dockerClient,
        ImageData image, ContainerParametersAgent containerParametersAgent, EnvironmentVariables environmentVariables)
    {
        _logger = logger;
        _dockerClient = dockerClient;
        _image = image;
        _containerParametersAgent = containerParametersAgent;
        _environmentVariables = environmentVariables;
    }

    #region HTTP Methods

    [HttpGet("generate-guid")]
    public Guid GenerateGUID()
    {
        return Guid.NewGuid();
    }

    [HttpPost("container-create")]
    public async Task<CreateContainerResponse?> CreateContainerForUser([FromBody] Guid id)
    {
        var containerResponse = await _dockerClient.Containers.CreateContainerAsync(
            _containerParametersAgent.CreateParameters(_image,
                id.ToString()));
        if (containerResponse == null)
            _logger.LogError("Failed to create container for user {id}", id);
        return containerResponse;
    }

    private Task<MultiplexedStream> AttachContainerAsync(string containerId)
    {
        return _dockerClient.Containers.AttachContainerAsync(
            containerId,
            parameters: _containerParametersAgent.AttachParameters(),
            tty: true, //false - если true, то выводит то что ввели, но не выключает контейнер
            cancellationToken: new CancellationToken());
    }

    [HttpPost("container-restart")]
    public async Task RestartContainer([FromBody] Guid id)
    {
        await _dockerClient.Containers.RestartContainerAsync($"{_image.Image}_{id}",
            _containerParametersAgent.RestartParameters(), new CancellationToken());
    }


    [HttpPost("container-execute")]
    public async Task<ExecData?> ExecuteCommandInContainer([FromBody] ExecContainerCommand data)
    {
        try
        {
            await RestartContainer(data.ID);
        }
        catch (Exception e)
        {
            await CreateContainerForUser(data.ID);
            await StartContainer(data.ID);
        }

        var execResult = new ExecData(data.ID, data.Command);
        var containerId = $"{_image.Image}_{data.ID}";


        await StartContainer(data.ID);
        _logger.LogInformation("Attach Container: {containerId}", containerId);
        using (var containerInputOutputStream = await AttachContainerAsync(containerId))
        {
            var sendCommand = Encoding.UTF8.GetBytes($"{data.Command}");

            await containerInputOutputStream.WriteAsync(
                sendCommand,
                0,
                sendCommand.Length,
                new CancellationToken());

            containerInputOutputStream.CloseWrite();

            var cancellationToken = new CancellationToken();
            var sb = new StringBuilder();

            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[_environmentVariables.BufferSize];
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
                .Remove(0, 1)
                .Replace($"{'\u0000'}", "")
                .Replace($"{'\u0001'}", "")
                .Replace($"{'\u0004'}", "")
                .Replace($"{'\u0005'}", "")
                .Replace("\ufffd", "")
                .Replace("\a", "")
                .Replace("\b", "")
                .Replace("\v", "");
            try
            {
                await RemoveContainer(data.ID);
            }
            catch
            {
            }

            return execResult;
        }
    }

    [HttpPost("container-start")]
    public async Task<bool> StartContainer([FromBody] Guid id)
    {
        _logger.LogInformation("Container {id} start", id);
        var res = await _dockerClient.Containers.StartContainerAsync(
            $"{_image.Image}_{id}",
            new ContainerStartParameters
            {
            },
            new CancellationToken());
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
                $"{_image.Image}_{id}",
                _containerParametersAgent.RemoveParameters(),
                new CancellationToken());
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpPost("container-stop")]
    public async Task<bool> StopContainerAsync([FromBody] Guid id)
    {
        return await _dockerClient.Containers
            .StopContainerAsync(
                $"{_image.Image}_{id}",
                _containerParametersAgent.StopParameters(),
                new CancellationToken());
    }

    #endregion
}