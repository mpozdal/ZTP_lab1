
using ProductApp.Domain.Interfaces;
using ProductApp.Domain.Validation;

namespace ProductApp.Application.Policies;

public class UniqueNamePolicy: IValidationPolicy
{
    private readonly IProductRepository _productRepository;

    public UniqueNamePolicy(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ValidationResult> Validate(string name, Guid productId)
    {
        var result = new ValidationResult();
        if(await _productRepository.ExistsByNameAsync(name, productId))
            result.AddError("Name must be unique!");
        return result;
    }
}