namespace SequorProduction.Api.Models;
public sealed class ProductMaterial
{
    public string ProductCode { get; set; } = "";
    public Product Product { get; set; } = null!;
    public string MaterialCode { get; set; } = "";
    public Material Material { get; set; } = null!;
}
