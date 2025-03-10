using ProductApp.Application.Interfaces;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Application.Policies;

public class LegalNamePolicy: IValidationPolicy
{
    private readonly IForbiddenPhraseService _forbiddenPhraseService;

    public LegalNamePolicy(IForbiddenPhraseService forbiddenPhraseService)
    {
        _forbiddenPhraseService = forbiddenPhraseService;
    }
    public async Task<ValidationResult> Validate(string name, Guid productId)
    {
        var result = new ValidationResult();
        var forbiddenPhrases = await _forbiddenPhraseService.GetAllPharsesAsync();
        if(forbiddenPhrases.Any(phrase => name.Contains(phrase.Phrase, StringComparison.OrdinalIgnoreCase)))
            result.AddError("Name contains illegal phrase!");
        return result;


    }
}