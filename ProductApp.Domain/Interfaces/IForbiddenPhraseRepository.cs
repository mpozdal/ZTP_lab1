using ProductApp.Domain.Entities;

namespace ProductApp.Domain.Interfaces;

public interface IForbiddenPhraseRepository
{
    Task<IEnumerable<ForbiddenPhrase>> GetAllAsync();
    Task<ForbiddenPhrase> GetByIdAsync(Guid id);
    Task AddAsync(ForbiddenPhrase pharse);
    Task UpdateAsync(ForbiddenPhrase pharse);
    Task DeleteAsync(Guid id);
    
}