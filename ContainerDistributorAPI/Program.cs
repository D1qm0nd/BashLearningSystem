using ContainerDistributorAPI.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using Exceptions;

namespace ContainerDistributorAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args).ConfigureServices();
        var app = builder.Build().ConfigureHTTP();
        app.UseCors("MyPolicy");
        app.Run();
    }

    #region Settings

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();
        }));

        DockerClient dockerClient;
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            dockerClient = new DockerClientConfiguration(
                    new Uri("unix:///var/run/docker.sock"))
                .CreateClient();
        }
        else if (OperatingSystem.IsWindows())
        {
            dockerClient = new DockerClientConfiguration(
                    new Uri("npipe://./pipe/docker_engine"))
                .CreateClient();
        }
        else throw new NotImplementedException("Supported OS: Windows, Linux, MacOS");

        builder.Services.AddSingleton<DockerClient>(
            dockerClient);
        builder.Services.AddSingleton<ContainersLifeCycleObject>(
            new ContainersLifeCycleObject());

        var delay = Environment.GetEnvironmentVariable("TRACKER_MINUTE_DELAY");
        var _delayMinutes = delay != null && int.TryParse(delay, out _) ? int.Parse(delay) : 1;

        var imageNameEnvValue = Environment.GetEnvironmentVariable("IMAGE");
        if (imageNameEnvValue == null)
            throw new EnvironmentVariableExistingException("IMAGE");

        var imageTagEnvValue = Environment.GetEnvironmentVariable("IMAGE_TAG");
        if (imageTagEnvValue == null)
            throw new EnvironmentVariableExistingException("IMAGE_TAG");

        var image = new ImageData(imageNameEnvValue, imageTagEnvValue);

        builder.Services.AddSingleton<ImageData>(image);

        builder.Services.AddSingleton(new ContainerParametersAgent((image, id) => new CreateContainerParameters
            {
                Image = image.ToString(),
                Name = $"{image.Image}_{id}",
                Shell = new List<string> //без этого контейнер просто вырубится после запуска
                {
                    "tail -f /etc/hosname"
                },
                ArgsEscaped = true,
                AttachStdin = true, //false - долго подключается, вероятна утечка памяти
                AttachStdout = true,
                AttachStderr = true,
                StdinOnce = true, //true - без этого не работает, а с этим падает после одной команды?
                OpenStdin = true,
                Tty = false, //false - если true, то выводит то что ввели, но не выключает контейнер
            }, () => new ContainerAttachParameters
            {
                Stderr = true,
                Stdin = true, //false - долго подключается 
                Stdout = true,
                Stream = true,
                Logs = "1"
            }, () => new ContainerStopParameters
            {
                WaitBeforeKillSeconds = null
            },
            () => new ContainerRemoveParameters
            {
            }
        ));
        return builder;
    }

    private static WebApplication ConfigureHTTP(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.MapControllers();
        return app;
    }

    #endregion
}