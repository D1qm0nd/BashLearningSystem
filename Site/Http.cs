namespace Site;

public static partial class Program
{
    /// Configure the HTTP request pipeline.
    public static void ConfigureHTTP(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // app.UseAuthorization();
    }
}