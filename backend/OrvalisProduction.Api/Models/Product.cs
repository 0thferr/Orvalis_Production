namespace OrvalisProduction.Api.Models;
public sealed class Product
{
    public string ProductCode { get; set; } = "";
    public string ProductDescription { get; set; } = "";
    public string Image { get; set; } = "";
    public decimal CycleTime { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();
}
