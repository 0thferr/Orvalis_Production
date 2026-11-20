namespace OrvalisProduction.Api.Models;
public sealed class Material
{
    public string MaterialCode { get; set; } = "";
    public string MaterialDescription { get; set; } = "";
    public ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();
}
