namespace ProductApp.Domain.Entities;

public class ForbiddenPhrase
{
    public Guid Id { get; set; }
    public string Phrase { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}