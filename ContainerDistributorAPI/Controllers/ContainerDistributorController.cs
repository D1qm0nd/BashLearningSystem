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

    private List<ContainerData> _containers;
    private readonly DockerClient _dockerClient;
    private Task _trackTask;
    private ImageData _image;

    public ContainerDistributorApiController(ILogger<ContainerDistributorApiController> logger,
        List<ContainerData> _containersData, DockerClient docker_client, ImageData image)
    {
        _logger = logger;
        _containers = _containersData;
        _dockerClient = docker_client;
        _image = image;
        // if (int.TryParse(delayEnvValue, out delay))
        //     _trackTask = TrackContainers(_containers, delay);
        // else _trackTask = TrackContainers(_containers, 3);
        //TODO:реализовать трекер
    }

    [HttpGet("[action]/generate-guid")]
    public Guid GenerateGUID() => Guid.NewGuid();
    
    [HttpGet("[action]/create-container-{user_id}")]
    public async Task<string> CreateContainerForUser(Guid user_id)
    {
        return await Task<string>.Factory.StartNew(() =>
        {
            return CreateContainer(
                name: _image.Image,
                id: user_id.ToString(),
                cancellationToken: new CancellationToken()).Result.ID;
        });
    }

    private async Task<CreateContainerResponse> CreateContainer(string name, string id, CancellationToken cancellationToken)
    {
        return await _dockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _image.ToString(),
                OpenStdin = true,
                AttachStdin = true,
                AttachStdout = true,
                AttachStderr = true,
                Name = $"{name}-{id}",
                Shell = new List<string> { "tail -f /etc/hosname" },
            },
            cancellationToken: cancellationToken);
    }
    
}