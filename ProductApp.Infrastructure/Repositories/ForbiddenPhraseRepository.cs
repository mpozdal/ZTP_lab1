using Microsoft.EntityFrameworkCore;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;
using ProductApp.Infrastructure.Context;

namespace ProductApp.Infrastructure.Repositories;

public class ForbiddenPhraseRepository: IForbiddenPhraseRepository
{
    private readonly AppDbContext _context;

    public ForbiddenPhraseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ForbiddenPhrase>> GetAllAsync()
    {
        return await _context.ForbiddenPhrase.ToListAsync();
    }

    public async Task<ForbiddenPhrase> GetByIdAsync(Guid id)
    {
        return await _context.ForbiddenPhrase.FindAsync(id);
    }
    
    public async Task AddAsync(ForbiddenPhrase pharse)
    {
        await _context.ForbiddenPhrase.AddAsync(pharse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ForbiddenPhrase pharse)
    {
        _context.ForbiddenPhrase.Update(pharse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var pharse = await _context.ForbiddenPhrase.FindAsync(id);
        if (pharse != null)
        {
            _context.ForbiddenPhrase.Remove(pharse);
            await _context.SaveChangesAsync();
        }
    }
}