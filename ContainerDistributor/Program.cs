using ContainerDistributor.Models;
using Docker.DotNet;

namespace ContainerDistributor;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args).ConfigureServices();
        var app = builder.Build().ConfigureHTTP();
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
        builder.Services.AddSingleton<List<ContainerData>>(new List<ContainerData>());
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