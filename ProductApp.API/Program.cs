using Microsoft.OpenApi.Models;
using ProductApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using ProductApp.Application;
using ProductApp.Application.Services;
using ProductApp.Infrastructure;

namespace ProductApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddInfrastucture(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
         
        });
        

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductApp API v1"));
        }

        app.MapControllers();
        app.Run();
        
    }
}

