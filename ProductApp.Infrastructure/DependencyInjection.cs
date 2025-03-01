using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApp.Infrastructure.Context;
using ProductApp.Infrastructure.Repositories;

namespace ProductApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IProductRepository, ProductRepository> ();
        services.AddScoped<IForbiddenPhraseRepository, ForbiddenPhraseRepository> ();

        return services;

    }
}