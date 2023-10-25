using BashDataBase;
using LearningSite.WebApp.BusinessModels;

namespace LearningSite;

public static partial class Program
{
    /// Configure services to the container.
    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<BashLearningContext>();
        builder.Services.AddSingleton(new LearningSession());
    }
}