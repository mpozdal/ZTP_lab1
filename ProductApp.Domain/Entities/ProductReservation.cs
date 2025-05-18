namespace ProductApp.Domain.Entities;

public class ProductReservation
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime ExpiresAt {get; set;}

}