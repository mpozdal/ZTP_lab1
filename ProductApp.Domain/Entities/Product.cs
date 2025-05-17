namespace ProductApp.Domain.Entities;

public class Product
{
    public required Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Category Category { get; set; }
    public int ReservedStock { get; set; } = 0;
    public int AvailableStock => Stock - ReservedStock;
}