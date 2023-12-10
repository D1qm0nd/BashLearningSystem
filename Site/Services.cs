using BashDataBaseModels;
using BashLearningDB;

namespace Site;

public static partial class Program
{
    /// Configure services to the container.
    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services
            .AddDbContext<BashLearningContext>()
            .AddSingleton(new Session<User>());
        
        builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();
        }));
        
    }
}

