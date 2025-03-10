using System.Text.RegularExpressions;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Domain.Policies;

public class NameLengthPolicy: IValidationPolicy
{
    public async Task<ValidationResult> Validate(string name, Guid productId)
    {
        var result = new ValidationResult();
        if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
        {
            result.AddError("Name must contains only letters and numbers!");
        }
        return result;
    }
}