using BashDataBase;
using DataModels;
using Lib.DataBases;
using WebApp.LearningSystem.BussinesModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<BashLearningContext>(ServiceLifetime.Singleton);

builder.Services.AddSingleton<BusinessViewModel>(
    new BusinessViewModel(
        new AuthorizationModel(), 
        new ContextModel(DbContextFactory<BashLearningContext>.CreateContext()))
        );

// builder.Services.AddTransient<BusinessViewModel>( (_) =>    
//     new BusinessViewModel(
//         new AuthorizationModel(),
//         new ContextModel(DbContextFactory<BashLearningDataContext>.CreateContext())));



var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
try
{
    app.Run();
}
catch
{
}

