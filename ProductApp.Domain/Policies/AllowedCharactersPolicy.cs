using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Domain.Policies;

public class AllowedCharactersPolicy: IValidationPolicy
{
    public async Task<ValidationResult> Validate(string name, Guid productId)
    {
        var result = new ValidationResult();
        
        if(name.Length is < 3 or > 20)
            result.AddError("Name is too short or too long!");
        return result;
    }
}