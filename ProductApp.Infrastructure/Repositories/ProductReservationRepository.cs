using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Context;

namespace ProductApp.Infrastructure.Repositories;

public class ProductReservationRepository
{
    private readonly AppDbContext _context;

    public ProductReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProductReservation productReservation)
    {
        await _context.ProductReservations.AddAsync(productReservation);
        await _context.SaveChangesAsync();
    }
   
}