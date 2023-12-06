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
        var progress = new Progress<JSONMessage>(message => { Trace.WriteLine(message); });
        var cancellationToken = new CancellationToken();
        var task = _dockerClient.Images.CreateImageAsync(
            new ImagesCreateParameters { FromImage = "ubuntu", Tag = "22.04" },
            null,
            progress,
            cancellationToken);
        task.Wait();
    }

    [Test]
    public async Task RemoveUbuntuContainer()
    {
        await _dockerClient.Containers.RemoveContainerAsync(
            "ubuntu-22.04-container",
            new ContainerRemoveParameters
            {
                RemoveVolumes = true
            },
            new CancellationToken());
    }

    [Test]
    public async Task CreateUbuntuContainer()
    {
        try
        {
            await RemoveUbuntuContainer();
        }
        catch (DockerContainerNotFoundException e)
        {
            Console.WriteLine(e);
            // throw;
        }


        var container = await _dockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = "ubuntu:22.04",
                Name = $"ubuntu-22.04-container",
                AttachStdin = true,
                AttachStdout = true,
                AttachStderr = true,
                Shell = new List<string>
                {
                    "tail -f ./etc/hostname"
                },
                StopTimeout = null,
                OpenStdin = true,
                StdinOnce = true
            },
            new CancellationToken());
        Trace.WriteLine(container.ID);
    }

    [Test]
    public async Task StartUbuntuContainer()
    {
        var cancellationToken = new CancellationToken();
        var startResult = await _dockerClient.Containers.StartContainerAsync(
            "ubuntu-22.04-container",
            new ContainerStartParameters
            {
            },
            cancellationToken);
    }

    [Test]
    public async Task ExecuteUbuntuCommand()
    {
        await StartUbuntuContainer();

        var containerIOStream = await _dockerClient.Containers.AttachContainerAsync(
            "ubuntu-22.04-container",
            parameters: new ContainerAttachParameters
            {
                Stderr = true,
                Stdin = true,
                Stdout = true,
                Stream = true,
                Logs = "1"
            },
            tty: true, cancellationToken: new CancellationToken());

        var buffer = new byte[1024];

        using (containerIOStream)
        {
            var command = Encoding.UTF8.GetBytes("whoami && echo hello && ls -la");

            await containerIOStream.WriteAsync(
                command,
                0,
                command.Length,
                new CancellationToken());

            containerIOStream.CloseWrite();

            MultiplexedStream.ReadResult answ;

            do
            {
                answ = await containerIOStream.ReadOutputAsync(buffer, 0, buffer.Length, new CancellationToken());
            } while (answ.EOF == false);
        }

        var answer = Encoding.UTF8.GetString(buffer).TrimStart(' ').TrimEnd(' ');
        Trace.WriteLine(answer);
    }
}