namespace SequorProduction.Api.Models;
public sealed class ProductionUser
{
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public DateTime InitialDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Production> Productions { get; set; } = new List<Production>();
}
