using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Domain.Policies;

public class NonNegativeStockPolicy: IValidationPolicy
{
    public async Task<ValidationResult> Validate(string stockString, Guid productId)
    {
        var result = new ValidationResult();
        if (int.TryParse(stockString, out int stock))
        {
            if(stock < 0) result.AddError("Stock can't be negative!");
        }

        return result;
    }
}