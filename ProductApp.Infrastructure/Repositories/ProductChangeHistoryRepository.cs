using Microsoft.EntityFrameworkCore;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;
using ProductApp.Infrastructure.Context;

namespace ProductApp.Infrastructure.Repositories;

public class ProductChangeHistoryRepository: IProductChangeHistoryRepository
{
    private readonly AppDbContext _context;

    public ProductChangeHistoryRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ProductChangeHistory>> GetAllAsync()
    {
        return await _context.ProductsHistory.Include(h => h.Product).ToListAsync();
    }

    public async Task AddAsync(ProductChangeHistory productChangeHistory)
    {
        await _context.ProductsHistory.AddAsync(productChangeHistory);
        await _context.SaveChangesAsync();
    }
}