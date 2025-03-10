namespace ProductApp.Domain.Excetpions;

public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException(List<string> errors) 
        : base("Product validation failed.")
    {
        Errors = errors;
    }
}
