namespace SequorProduction.Api.Models;
public sealed class Order
{
    public string OrderNumber { get; set; } = "";
    public decimal Quantity { get; set; }
    public string ProductCode { get; set; } = "";
    public Product Product { get; set; } = null!;
    public ICollection<Production> Productions { get; set; } = new List<Production>();
}
