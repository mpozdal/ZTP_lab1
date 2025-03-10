using Microsoft.EntityFrameworkCore;
using ProductApp.Application.DTOs;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Interfaces;
using ProductApp.Application.Interfaces;

namespace ProductApp.Application.Services;

public class ForbiddenPhraseService: IForbiddenPhraseService
{

    private readonly IForbiddenPhraseRepository _forbiddenPhraseRepository;

    public ForbiddenPhraseService(IForbiddenPhraseRepository forbiddenPhraseRepository)
    {
        _forbiddenPhraseRepository = forbiddenPhraseRepository;
    }
    
    public async Task<IEnumerable<ForbiddenPhraseDto>> GetAllPharsesAsync()
    {
        var pharses =  await _forbiddenPhraseRepository.GetAllAsync();
        return pharses.Select(p => new ForbiddenPhraseDto
        {
            Id = p.Id,
            Phrase = p.Phrase,
        });
    }
    public async Task<ForbiddenPhraseDto?> GetPharseByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<ForbiddenPhraseDto> CreatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto)
    {
     
        var pharse = new ForbiddenPhrase
        {
            Phrase = forbiddenPhraseDto.Phrase
        };
        await _forbiddenPhraseRepository.AddAsync(pharse);

        forbiddenPhraseDto.Id = pharse.Id;

        return forbiddenPhraseDto;
    }

    public async Task<bool> UpdatePharseAsync(ForbiddenPhraseDto forbiddenPhraseDto)
    {
        var existingProduct = await _forbiddenPhraseRepository.GetByIdAsync(forbiddenPhraseDto.Id);
        if (existingProduct == null) return false;

        existingProduct.Phrase = forbiddenPhraseDto.Phrase;
        existingProduct.UpdatedAt = DateTime.Now;

        await _forbiddenPhraseRepository.UpdateAsync(existingProduct);
        return true;
    }

    public async Task<bool> DeletePharseAsync(Guid id)
    {
        var exisitingPharse = await _forbiddenPhraseRepository.GetByIdAsync(id);
        if (exisitingPharse == null)
        {
            return false;
        }
        await _forbiddenPhraseRepository.DeleteAsync(id);
        return true;
    }
    
}