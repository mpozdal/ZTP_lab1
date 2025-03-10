using ProductApp.Application.Interfaces;
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Application.Policies;

public class PriceRangePolicy: IValidationPolicy
{
    private readonly ICategoryService _categoryService;
    public PriceRangePolicy(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task<ValidationResult> Validate(string name, Guid productId)
    {
        var validationResult = new ValidationResult();
        
        var category = await _categoryService.GetCategoryByIdAsync(productId);
        if (category == null)
        {
            validationResult.Errors.Add("Category doesn't exist.");
            return validationResult;
        }
        
        decimal minPrice = category.MinPrice;
        decimal maxPrice = category.MaxPrice;
        
        if (decimal.TryParse(name, out decimal price)) 
        {
            if (price < minPrice || price > maxPrice)
            {
                validationResult.Errors.Add($"Price must be between {minPrice} and {maxPrice}.");
            }
        }
        else
        {
            validationResult.Errors.Add("Price must be a number.");
        }

        return validationResult;
    }


}