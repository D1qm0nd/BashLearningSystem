using System.Diagnostics;
using ContainerDistributor.Models;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContainerDistributor.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainerDistributorController : ControllerBase //, IDisposable
{
    private readonly ILogger<ContainerDistributorController> _logger;

    private List<ContainerData> _containers;
    private readonly DockerClient _client;
    private Task trackTask;

    public ContainerDistributorController(ILogger<ContainerDistributorController> logger,
        List<ContainerData> _containersData, DockerClient docker_client)
    {
        _logger = logger;
        _containers = _containersData;
        _client = docker_client;
        // var credentials = new BasicAuthCredentials ("YOUR_USERNAME", "YOUR_PASSWORD");
        // _client = new DockerClientConfiguration().CreateClient();
        var delayEnvValue = Environment.GetEnvironmentVariable("CONTAINER_TRACKER_DELAY");
        var delay = 1;
        if (int.TryParse(delayEnvValue, out delay))
            trackTask = TrackContainers(_containers, delay);
        else trackTask = TrackContainers(_containers, 3);

        //TODO: проверка существования образа, если его нет, создать
        // CreateImage("ubuntu:22.04").Wait();

    }

    private async Task TrackContainers(List<ContainerData> containersData, int delayMin)
    {
        await Task.Factory.StartNew(() =>
        {
            while (true)
            {
                for (var i = 0; i < containersData.Count; i++)
                {
                    var container = containersData[i];
                    if (DateTime.UtcNow >= container.AppealTimeUTC.AddMinutes(delayMin))
                    {
                        _client.Containers.StopContainerAsync((string)(container!.ID), parameters:
                            new ContainerStopParameters());
                        StopContainer(container);
                        RemoveContainer(container);
                        // containersData.Remove(container);
                        i--;
                    }
                }
                Thread.Sleep(delayMin * 60 * 1000);
            }
        });
    }

    // [HttpGet()]
    // public string CreateUbuntuImage()
    // {
    //     return ""
    // }

    [HttpGet(template: "[action]/{user_id}/{image}")]
    public string? CreateContainerForUser(Guid user_id, string image)
    {
        if (_containers.Any(container => container.UserId == user_id))
            return null;
        var container = _client.Containers.CreateContainerAsync(
            parameters: new CreateContainerParameters()
            {
                Image = image,
                HostConfig = new HostConfig()
                {
                    DNS = new[] {"8.8.8.8", "8.8.4.4"},
                    AutoRemove = true,
                    NetworkMode = "host"
                },
                StopTimeout = null
            } ).Result;
        _containers.Add(new ContainerData(user_id, container.ID));
        _logger.LogInformation(
            $"{DateTime.UtcNow} Allocated container: {container.ID}, CURRENT COUNT: {_containers.Count}");
        return container.ID;
    }

    [HttpGet(template: "[action]/{user_id}")]
    public string? ExecuteCommandInContainer(Guid user_id)
    {
        var container = _containers.FirstOrDefault(e => e.UserId == user_id);
        if (container == null)
        {
            _logger.LogError(new ContainerExistingException(), "Container doesn`t exist");
            return null;
        }
        else
        {
            StartContainer(user_id);
            return ":)";
        }
    }

    [HttpGet("[action]/{user_id}")]
    public string? StartContainer(Guid user_id)
    {
        var container = _containers.FirstOrDefault(container => container.UserId == user_id);
        if (container == null)
            return null;
        _client.Containers.StartContainerAsync((string)(container!.ID),
            parameters: new ContainerStartParameters());
        container.UpdateAppealTime();
        _logger.LogInformation($"{DateTime.UtcNow} Started container: {container.ID}");
        return null;
    }

    [HttpGet("[action]/{user_id}")]
    public bool StopContainer(Guid user_Id)
    {
        var container = _containers.FirstOrDefault(container => container.UserId == user_Id);
        if (container == null)
            return false;
        StopContainer(container);
        return true;
    }

    private bool StopContainer(ContainerData container)
    {
        try
        {
            _client.Containers.StopContainerAsync(container.ID, new ContainerStopParameters()).Wait();
            _logger.LogInformation($"{DateTime.UtcNow} Stopped container: {container.ID}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    [HttpGet("[action]/{user_id}")]
    public bool RemoveContainer(Guid user_Id)
    {
        var container = _containers.FirstOrDefault(container => container.UserId == user_Id);
        return RemoveContainer(container);
    }
    private bool RemoveContainer(ContainerData container)
    {
        try
        {
            _client.Containers.RemoveContainerAsync(container.ID,
                new ContainerRemoveParameters()).Wait();
            _logger.LogInformation(
                $"{DateTime.UtcNow} Remove container: {container.ID}, Last AppealTimeUTC: {container.AppealTimeUTC}, CURRENT COUNT: {_containers.Count}");
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    [HttpGet("[action]/{image}:{tag}")]
    public void CreateImage(string image, string tag)
    {
        _client.Images.CreateImageAsync(
            new ImagesCreateParameters
            {
                FromImage = image,
                Tag = tag,
            }, 
            null,
            // new AuthConfig
            // {
            //     Email = "test@example.com",
            //     Username = "test",
            //     Password = "pa$$w0rd"
            // },
            new Progress<JSONMessage>()).Wait();
    }
    
    // public void Dispose()
    // {
    //     for (var i = 0; i < _containers.Count; i++)
    //     {
    //         var container = _containers[i];
    //         _client.Containers.RemoveContainerAsync((string)(container!.ID), 
    //             parameters: new ContainerRemoveParameters());
    //         _containers.Remove(container);
    //         i--;
    //     }
    //     _client.Dispose();
    //     trackTask.Dispose();
    // }
}