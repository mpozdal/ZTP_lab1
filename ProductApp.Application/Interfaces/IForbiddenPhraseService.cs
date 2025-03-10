using ProductApp.Application.DTOs;

namespace ProductApp.Application.Interfaces;

public interface IForbiddenPhraseService
{
    Task<IEnumerable<ForbiddenPhraseDto>> GetAllPharsesAsync();
    Task<ForbiddenPhraseDto?> GetPharseByIdAsync(Guid id);
    Task<ForbiddenPhraseDto> CreatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto);
    Task<bool> UpdatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto);
    Task<bool> DeletePharseAsync(Guid id);
    
}