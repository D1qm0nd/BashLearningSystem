using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Docker.DotNet.BashLearningTests;

public class Tests
{
    private static DockerClient _dockerClient; 
    [SetUp]
    public void Setup()
    {
        _dockerClient = new DockerClientConfiguration().CreateClient();
    }

    [Test]
    public void CreateUbuntuImage()
    {
        var progress = new Progress<JSONMessage>(message =>
        {
            Trace.WriteLine(message);
        });
        var cancellationToken = new CancellationToken();
        var task = _dockerClient.Images.CreateImageAsync(
            parameters: new ImagesCreateParameters { FromImage = "ubuntu", Tag = "22.04" },
            authConfig: null,
            progress: progress, 
            cancellationToken: cancellationToken);
        task.Wait();
    }

    [Test]
    public async Task CreateUbuntuContainer()
    {
        try
        {
            await _dockerClient.Containers.RemoveContainerAsync(
                id: "ubuntu-22.04-container", 
                parameters: new ContainerRemoveParameters(),
                cancellationToken: new CancellationToken());
        }
        catch (DockerContainerNotFoundException e)
        {
            Console.WriteLine(e);
            // throw;
        }
        
        
        var container = await _dockerClient.Containers.CreateContainerAsync(
            parameters: new CreateContainerParameters
            {
                Image = "ubuntu:22.04", 
                Name = $"ubuntu-22.04-container",
                AttachStdin = true,
                AttachStdout = true,
                AttachStderr = true,
                Shell = new List<string> {
                    "tail -f ./etc/hostname"
                },
                StopTimeout = null,
                OpenStdin = true,
                StdinOnce = true
            }, 
            cancellationToken: new CancellationToken());
        Trace.WriteLine(container.ID);
    }

    [Test]
    public async Task StartUbuntuContainer()
    {
        var cancellationToken = new CancellationToken();
        var startResult = await _dockerClient.Containers.StartContainerAsync(
            id: "ubuntu-22.04-container",
            parameters: new ContainerStartParameters
            {
            },
            cancellationToken: cancellationToken);
    }

    [Test]
    public async Task ExecuteUbuntuCommand()
    {
        var containerIOStream = await _dockerClient.Containers.AttachContainerAsync(
            id: "ubuntu-22.04-container",
            parameters: new ContainerAttachParameters
            {
                Stderr = true,
                Stdin = true,
                Stdout = true,
                Stream = true,
                Logs = "1"
            },
            tty: true, cancellationToken: new CancellationToken());

        var command = Encoding.UTF8.GetBytes("whoami && echo hello123");

        await containerIOStream.WriteAsync(
            buffer: command, 
            offset: 0, 
            count: command.Length,
            cancellationToken: new CancellationToken());
        var buffer = new byte[1024];
        var answ = await containerIOStream.ReadOutputAsync(buffer, 0, buffer.Length, new CancellationToken());
        var answer = await containerIOStream.ReadOutputToEndAsync(new CancellationToken());
    }
}