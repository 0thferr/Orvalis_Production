namespace SequorProduction.Api.Models;
public sealed class Production
{
    public long Id { get; set; }
    public string Email { get; set; } = "";
    public ProductionUser User { get; set; } = null!;
    public string OrderNumber { get; set; } = "";
    public Order Order { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Quantity { get; set; }
    public string MaterialCode { get; set; } = "";
    public Material Material { get; set; } = null!;
    public decimal CycleTime { get; set; }
}
