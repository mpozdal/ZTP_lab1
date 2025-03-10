using ProductApp.Domain.Validation;

namespace ProductApp.Domain.Interfaces;

public interface IValidationPolicy
{
    Task<ValidationResult> Validate(string name, Guid productId);
}