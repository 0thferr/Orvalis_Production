using Microsoft.EntityFrameworkCore;
using OrvalisProduction.Api.Models;

namespace OrvalisProduction.Api.Data;

public static class DbInitializer
{
    private const string TestEmail = "teste@orvalis.example";
    private const string MariaEmail = "maria@orvalis.example";

    public static async Task InitializeAsync(ProductionDbContext db)
    {
        if (await db.Products.AnyAsync() || await db.Materials.AnyAsync() ||
            await db.Users.AnyAsync() || await db.Orders.AnyAsync() || await db.Productions.AnyAsync())
        {
            return;
        }

        db.Products.AddRange(
            new Product { ProductCode = "abc", ProductDescription = "Produto ABC", Image = "/images/product-abc.svg", CycleTime = 30.3m },
            new Product { ProductCode = "def", ProductDescription = "Produto DEF", Image = "/images/product-def.svg", CycleTime = 45.5m },
            new Product { ProductCode = "ghi", ProductDescription = "Produto GHI", Image = "/images/product-ghi.svg", CycleTime = 60m });
        db.Materials.AddRange(
            new Material { MaterialCode = "aaa", MaterialDescription = "Material A - Alumínio" },
            new Material { MaterialCode = "bbb", MaterialDescription = "Material B - Aço" },
            new Material { MaterialCode = "ccc", MaterialDescription = "Material C - Plástico" },
            new Material { MaterialCode = "ddd", MaterialDescription = "Material D - Componente eletrônico" });
        db.Users.AddRange(
            new ProductionUser { Email = TestEmail, Name = "Usuário de Teste", InitialDate = new(2026, 1, 1), EndDate = new(2026, 12, 31, 23, 59, 59) },
            new ProductionUser { Email = MariaEmail, Name = "Maria Oliveira", InitialDate = new(2026, 1, 1), EndDate = new(2026, 12, 31, 23, 59, 59) },
            new ProductionUser { Email = "joao@orvalis.example", Name = "João Santos", InitialDate = new(2026, 6, 1), EndDate = new(2026, 11, 30, 23, 59, 59) });
        db.Orders.AddRange(
            new Order { OrderNumber = "111", Quantity = 100m, ProductCode = "abc" },
            new Order { OrderNumber = "222", Quantity = 200m, ProductCode = "def" },
            new Order { OrderNumber = "333", Quantity = 50m, ProductCode = "ghi" });
        db.ProductMaterials.AddRange(
            new ProductMaterial { ProductCode = "abc", MaterialCode = "aaa" },
            new ProductMaterial { ProductCode = "abc", MaterialCode = "bbb" },
            new ProductMaterial { ProductCode = "def", MaterialCode = "bbb" },
            new ProductMaterial { ProductCode = "def", MaterialCode = "ccc" },
            new ProductMaterial { ProductCode = "ghi", MaterialCode = "aaa" },
            new ProductMaterial { ProductCode = "ghi", MaterialCode = "ccc" },
            new ProductMaterial { ProductCode = "ghi", MaterialCode = "ddd" });
        db.Productions.AddRange(
            new Production { Email = TestEmail, OrderNumber = "111", Date = new(2026, 10, 20, 10, 30, 0), Quantity = 10, MaterialCode = "aaa", CycleTime = 35.5m },
            new Production { Email = TestEmail, OrderNumber = "111", Date = new(2026, 10, 21, 11, 0, 0), Quantity = 5, MaterialCode = "bbb", CycleTime = 32m },
            new Production { Email = MariaEmail, OrderNumber = "222", Date = new(2026, 10, 22, 9, 15, 0), Quantity = 20, MaterialCode = "ccc", CycleTime = 50m });
        await db.SaveChangesAsync();
    }
}
