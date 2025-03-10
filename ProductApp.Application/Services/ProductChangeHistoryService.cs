using ProductApp.Application.Interfaces;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;

namespace ProductApp.Application.Services;

public class ProductChangeHistoryService: IProductChangeHistoryService
{
    private readonly IProductChangeHistoryRepository _repository;

    public ProductChangeHistoryService(IProductChangeHistoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<ProductChangeHistory>> GetAllProductsHistoryAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ProductChangeHistory> CreateProductHistoryAsync(ProductChangeHistory productChangeHistory)
    {
        await _repository.AddAsync(productChangeHistory);
        return productChangeHistory;
    }
}