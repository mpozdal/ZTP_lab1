using ProductApp.Domain.Entities;

namespace ProductApp.Application.Interfaces;

public interface IProductChangeHistoryService
{
    Task<IEnumerable<ProductChangeHistory>> GetAllProductsHistoryAsync();
    Task<ProductChangeHistory> CreateProductHistoryAsync(ProductChangeHistory productDto);
}