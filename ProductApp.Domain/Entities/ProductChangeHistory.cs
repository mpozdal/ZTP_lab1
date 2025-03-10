namespace ProductApp.Domain.Entities;

public class ProductChangeHistory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateTime ChangeDate { get; set; }
    public string ChangeType { get; set; } 
    public string OldValue { get; set; } 
    public string NewValue { get; set; }  
    
    public Product Product { get; set; }
}