using Microsoft.Extensions.DependencyInjection;
using ProductApp.Application.Services;

namespace ProductApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}