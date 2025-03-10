using Microsoft.Extensions.DependencyInjection;
using ProductApp.Application.Services;
using ProductApp.Application.Interfaces;
using ProductApp.Application.Policies;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Policies;
using ProductApp.Infrastructure.Repositories;

namespace ProductApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IForbiddenPhraseService, ForbiddenPhraseService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductChangeHistoryService, ProductChangeHistoryService>();
        
        services.AddScoped<IValidationPolicy, NameLengthPolicy>();
        services.AddScoped<IValidationPolicy, AllowedCharactersPolicy>();
        services.AddScoped<IValidationPolicy, UniqueNamePolicy>();
        services.AddScoped<IValidationPolicy, NonNegativeStockPolicy>();
        services.AddScoped<IValidationPolicy, PriceRangePolicy>();
        services.AddScoped<IValidationPolicy, LegalNamePolicy>();

        return services;
    }
}