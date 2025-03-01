using ProductApp.Application.DTOs;

namespace ProductApp.Application.Services;

public interface IForbiddenPhraseService
{
    Task<IEnumerable<ForbiddenPhraseDto>> GetAllPharsesAsync();
    Task<ForbiddenPhraseDto?> GetPharseByIdAsync(int id);
    Task<ForbiddenPhraseDto> CreatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto);
    Task<bool> UpdatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto);
    Task<bool> DeletePharseAsync(int id);

    Task<bool> ContainsForbiddenPhrase(string name);
}