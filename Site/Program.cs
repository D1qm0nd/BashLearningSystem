namespace Site
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.ConfigureServices();
            
            var app = builder.Build();
            
            app.ConfigureHTTP();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            
            app.Run();
        }
    }
}