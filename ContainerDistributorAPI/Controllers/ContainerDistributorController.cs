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
    private Dictionary<string, KeyValuePair<DateTime, MultiplexedStream>> _containersInputOutputStreams;
    private readonly ImageData _image;
    private readonly Task TrackTask;

    public ContainerDistributorApiController(ILogger<ContainerDistributorApiController> logger,
        Dictionary<string, KeyValuePair<DateTime, MultiplexedStream>> containersInputOutputStreams,
        DockerClient dockerClient,
        ImageData image)
    {
        _logger = logger;
        _containersInputOutputStreams = containersInputOutputStreams;
        _dockerClient = dockerClient;
        _image = image;
        _containersInputOutputStreams = new();
        TrackTask = TrackContainers(1);
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
                    _containersInputOutputStreams.Where(e =>
                        e.Value.Key.AddMinutes(minuteDelay) <= DateTime.UtcNow);
                Parallel.ForEachAsync(toCloseAndRemove, (pair, token) =>
                {
                    _logger.LogInformation($"Dispose container: {pair.Key}");
                    pair.Value.Value.Dispose();
                    _containersInputOutputStreams.Remove(pair.Key);
                    return default;
                }).Wait();
                Thread.Sleep(10000);
            }
        });
    }

    #region HTTP Methods

    [HttpGet("[action]/generate-guid")]
    public Guid GenerateGUID() => Guid.NewGuid();

    [HttpGet("[action]/create-container-{user_id}")]
    public async Task<string> CreateContainerForUser(Guid user_id)
    {
        return await Task<string>.Factory.StartNew(() => CreateContainer(
            id: user_id.ToString(),
            cancellationToken: new CancellationToken()).Result.ID);
    }

    [HttpPost("[action]/execute")]
    public async Task<string> ExecuteCommandInContainer(string id, [FromBody] string command)
    {
        var containerId = $"{_image.Image}_{id}";
        MultiplexedStream containerInputOutputStream =
            _containersInputOutputStreams.FirstOrDefault(container => container.Key == containerId).Value.Value;
        if (containerInputOutputStream == null)
        {
            _logger.LogInformation("Attach Container: {containerId}", containerId);
            containerInputOutputStream = await AttachContainerAsync(containerId);
            // _containersInputOutputStreams.Add(id,
            //     new KeyValuePair<DateTime, MultiplexedStream>(DateTime.UtcNow, containerInputOutputStream));
        }

        using (containerInputOutputStream)
        {
            var sendCommand = Encoding.UTF8.GetBytes(command);

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
                var buffer = new byte[1024];
                var answ = await containerInputOutputStream.ReadOutputAsync(buffer, 0, buffer.Length,
                    cancellationToken);
                switch (answ.Target)
                {
                    case MultiplexedStream.TargetStream.StandardIn:
                        _logger.LogInformation("READ INPUT");
                        break;
                    case MultiplexedStream.TargetStream.StandardOut:
                        _logger.LogInformation("READ OUTPUT");
                        var answer = Encoding.UTF8.GetString(buffer).Trim('\0');
                        _logger.LogInformation("READ BUFFER: {answer}", answer);
                        sb.Append(answer);
                        break;
                    case MultiplexedStream.TargetStream.StandardError:
                        _logger.LogInformation("READ ERROR");
                        break;
                }

                if (answ.EOF) break;
            }

            return sb.ToString();
        }
    }

    [HttpPost("[action]/StartContainer-{id}")]
    public async Task<string> StartContainer(string id)
    {
        _logger.LogInformation("Container {id} start", id);
        var res = await _dockerClient.Containers.StartContainerAsync(
            id: $"{_image.Image}_{id}",
            parameters: new ContainerStartParameters
            {
            },
            cancellationToken: new CancellationToken());
        if (res)
        {
            _logger.LogInformation("Container {id} start [SUCCESS]", id);
            return "started";
        }
        else
        {
            _logger.LogWarning("Container {id} start [FAILED]", id);
            return "container already running";
        }
    }

    #endregion

    #region Methods

    //TODO: сохранять где-то т.к. отвечает за жизненный цикл контенера, также продумать удаление после окончания жизненного цикла, и максимальное количество контенеров за раз
    private async Task<MultiplexedStream> AttachContainerAsync(string id) =>
        await _dockerClient.Containers.AttachContainerAsync(
            id: id,
            parameters: new ContainerAttachParameters
            {
                Stderr = true,
                Stdin = true, //false - долго подключается, вероятна утечка памяти
                Stdout = true,
                Stream = true,
                Logs = "1"
            },
            tty: false, //false - если true, то выводит то что ввели, но не выключает контейнер
            cancellationToken: new CancellationToken());


    private async Task<CreateContainerResponse> CreateContainer(string id, CancellationToken cancellationToken)
    {
        return await _dockerClient.Containers.CreateContainerAsync(
            parameters: new CreateContainerParameters
            {
                Image = _image.ToString(),
                Name = $"{_image.Image}_{id}",
                Shell = new List<string> //без этого контейнер просто вырубится после запуска
                {
                    "tail -f /etc/hosname"
                },
                ArgsEscaped = true,
                AttachStdin = true, //false - долго подключается, вероятна утечка памяти
                AttachStdout = true,
                AttachStderr = true,
                StdinOnce = true, //true без этого не работает, а с этим падает после одной команды?
                OpenStdin = true,
                Tty = false, //false - если true, то выводит то что ввели, но не выключает контейнер
            },
            cancellationToken: cancellationToken);
    }

    #endregion
}