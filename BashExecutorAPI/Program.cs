using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;

var builder = WebApplication.CreateBuilder(args);

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

// builder.Services.Configure<MvcOptions>(options =>
// {
//     options.Filters.Add(new CorsAuthorizationFilterFactory("MyPolicy"));
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();
app.UseCors("MyPolicy");

app.MapControllers();

app.Run();