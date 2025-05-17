using ProductApp.Application.DTOs;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Validation;

namespace ProductApp.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<ProductDto> CreateProductAsync(ProductDto productDto);
    Task<bool> UpdateProductAsync(ProductDto productDto);
    Task<bool> DeleteProductAsync(Guid id);
    Task<bool> ReserveProductAsync(Guid productId, int quantity);
    Task<bool> ReleaseProductAsync(Guid productId, int quantity);
}