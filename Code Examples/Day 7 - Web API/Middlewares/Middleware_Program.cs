using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication12.Data;
using WebApplication12.Models;
using WebApplication12.Services;
using WebApplication12.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// We are assuming ProductDbContext and ProductService exist in WebApplication12 namespace for this example
// builder.Services.AddDbContext<ProductDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Debug.WriteLine("Request intercepted");

    await next();

    Debug.WriteLine("Response Sent.");

});

app.Use(async (context, next) =>
{
    Debug.WriteLine("MiddleWare 1 Before");

    await next();

    Debug.WriteLine("MiddleWare 1 After.");

});

app.Use(async (context, next) =>
{
    Debug.WriteLine("MiddleWare 2 Before");

    await next();

    Debug.WriteLine("MiddleWare 2 After.");

});

// Using our custom middleware via the extension method
app.UseHeaderValidation();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();

app.Run();
