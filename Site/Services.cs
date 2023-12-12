using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Exceptions;

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
        
        builder.Services.AddCors(o => o.AddPolicy("MyPolicy", policy =>
        {
            policy
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowCredentials();
        }));
        
        var env_val = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        if (env_val == null)
            throw new EnvironmentVariableExistingException("BashLearningPrivateKey");
        var crypt_values = JsonSerializer.Deserialize<CryptographValues>(env_val);
        if (crypt_values == null)
            throw new ArgumentException("BashLearningPrivateKey: Check input data for correct");
        
        builder.Services.AddSingleton<Cryptograph>(new Cryptograph(key: crypt_values.Key, alphabet: crypt_values.Alphabet));

    }
}

