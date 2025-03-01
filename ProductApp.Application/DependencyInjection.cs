using Microsoft.Extensions.DependencyInjection;
using ProductApp.Application.Services;
using ProductApp.Infrastructure.Repositories;

namespace ProductApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IForbiddenPhraseService, ForbiddenPhraseService>();

        return services;
    }
}