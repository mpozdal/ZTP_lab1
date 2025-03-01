using ProductApp.Domain.Entities;

namespace ProductApp.Infrastructure.Repositories;

public interface IForbiddenPhraseRepository
{
    Task<IEnumerable<ForbiddenPhrase>> GetAllAsync();
    Task<ForbiddenPhrase> GetByIdAsync(int id);
    Task AddAsync(ForbiddenPhrase pharse);
    Task UpdateAsync(ForbiddenPhrase pharse);
    Task DeleteAsync(int id);
    
}