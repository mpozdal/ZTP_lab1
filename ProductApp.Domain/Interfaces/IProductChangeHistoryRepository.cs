using ProductApp.Domain.Entities;

namespace ProductApp.Domain.Interfaces;

public interface IProductChangeHistoryRepository
{
    Task<IEnumerable<ProductChangeHistory>> GetAllAsync();
    Task AddAsync(ProductChangeHistory productChangeHistory);

}