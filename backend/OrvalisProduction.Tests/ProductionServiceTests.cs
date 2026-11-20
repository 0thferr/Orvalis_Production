using Microsoft.EntityFrameworkCore;
using OrvalisProduction.Api.Data;
using OrvalisProduction.Api.DTOs;
using OrvalisProduction.Api.Models;
using OrvalisProduction.Api.Services;
using Xunit;

namespace OrvalisProduction.Tests;
public sealed class ProductionServiceTests
{
    private static async Task<(ProductionDbContext Db, ProductionService Service)> CreateAsync()
    {
        var options = new DbContextOptionsBuilder<ProductionDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var db = new ProductionDbContext(options);
        db.Users.Add(new ProductionUser { Email = "teste@orvalis.example", Name = "Teste", InitialDate = new(2026, 1, 1), EndDate = new(2026, 12, 31, 23, 59, 59) });
        db.Products.Add(new Product { ProductCode = "abc", ProductDescription = "Produto ABC", Image = "/images/product-abc.svg", CycleTime = 30.3m });
        db.Materials.AddRange(new Material { MaterialCode = "aaa", MaterialDescription = "Material A" }, new Material { MaterialCode = "ccc", MaterialDescription = "Material C" });
        db.Orders.Add(new Order { OrderNumber = "111", Quantity = 100, ProductCode = "abc" });
        db.ProductMaterials.Add(new ProductMaterial { ProductCode = "abc", MaterialCode = "aaa" });
        await db.SaveChangesAsync();
        return (db, new ProductionService(db));
    }
    private static SetProductionRequest Valid() => new() { Email = "teste@orvalis.example", Order = "111", ProductionDate = new(2026, 11, 4), ProductionTime = new(10, 30), Quantity = 10, MaterialCode = "aaa", CycleTime = 35.5m };

    [Fact] public async Task GetOrders_ReturnsRegisteredOrder() { var (_, s) = await CreateAsync(); Assert.Single(await s.GetOrdersAsync(default)); }
    [Fact] public async Task GetHistory_ReturnsEmptyForUserWithoutProductions() { var (_, s) = await CreateAsync(); Assert.Empty(await s.GetProductionsAsync("teste@orvalis.example", default)); }
    [Fact] public async Task GetHistory_ReturnsProductionForEmail() { var (db, s) = await CreateAsync(); db.Productions.Add(new Production { Email="teste@orvalis.example", OrderNumber="111", Date=new(2026,11,4,10,30,0), Quantity=2, MaterialCode="aaa", CycleTime=35.5m }); await db.SaveChangesAsync(); Assert.Single(await s.GetProductionsAsync("teste@orvalis.example", default)); }
    [Fact] public async Task ValidProduction_IsPersisted() { var (db, s) = await CreateAsync(); Assert.Equal(200, (await s.SetProductionAsync(Valid(), default)).Status); Assert.Single(await db.Productions.ToListAsync()); }
    [Theory]
    [InlineData("bad@orvalis.example", "111", 10, "aaa", 35.5, "2026-11-04", "Usuário não cadastrado")]
    [InlineData("teste@orvalis.example", "999", 10, "aaa", 35.5, "2026-11-04", "Ordem não cadastrada")]
    [InlineData("teste@orvalis.example", "111", 0, "aaa", 35.5, "2026-11-04", "Quantidade deve ser maior")]
    [InlineData("teste@orvalis.example", "111", 101, "aaa", 35.5, "2026-11-04", "superior")]
    [InlineData("teste@orvalis.example", "111", 10, "ccc", 35.5, "2026-11-04", "Material não vinculado")]
    [InlineData("teste@orvalis.example", "111", 10, "aaa", 35.5, "2025-12-31", "Data fora")]
    [InlineData("teste@orvalis.example", "111", 10, "aaa", 35.5, "2027-01-01", "Data fora")]
    [InlineData("teste@orvalis.example", "111", 10, "aaa", 0, "2026-11-04", "Tempo de ciclo")]
    public async Task InvalidProduction_ReturnsValidationError(string email, string order, decimal quantity, string material, decimal cycle, string date, string message)
    {
        var (_, s) = await CreateAsync(); var d = DateOnly.Parse(date);
        var result = await s.SetProductionAsync(new SetProductionRequest { Email=email, Order=order, ProductionDate=d, ProductionTime=new(10,30), Quantity=quantity, MaterialCode=material, CycleTime=cycle }, default);
        Assert.Equal(201, result.Status); Assert.Equal("E", result.Type); Assert.Contains(message, result.Description);
    }
    [Fact] public async Task CycleBelowProductCycle_SucceedsWithInformation() { var (_, s) = await CreateAsync(); var r = await s.SetProductionAsync(Valid() with { CycleTime = 1 }, default); Assert.Equal(200, r.Status); Assert.Contains("inferior", r.Description); }
}
