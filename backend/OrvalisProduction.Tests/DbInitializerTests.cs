using Microsoft.EntityFrameworkCore;
using OrvalisProduction.Api.Data;
using OrvalisProduction.Api.Models;
using Xunit;

namespace OrvalisProduction.Tests;

public sealed class DbInitializerTests
{
    [Fact]
    public async Task InitializeAsync_SeedsEmptyDatabaseOnlyOnce()
    {
        await using var db = CreateDatabase();

        await DbInitializer.InitializeAsync(db);
        await DbInitializer.InitializeAsync(db);

        Assert.Equal(3, await db.Orders.CountAsync());
        Assert.Equal(3, await db.Users.CountAsync());
        Assert.All(await db.Users.Select(user => user.Email).ToListAsync(),
            email => Assert.EndsWith("@orvalis.example", email));
        Assert.Equal(3, await db.Productions.CountAsync());
    }

    [Fact]
    public async Task InitializeAsync_PreservesExistingDataWithoutAddingDemoRecords()
    {
        await using var db = CreateDatabase();
        const string existingEmail = "operador@empresa.example";
        db.Users.Add(new ProductionUser { Email = existingEmail, Name = "Operador" });
        await db.SaveChangesAsync();

        await DbInitializer.InitializeAsync(db);

        Assert.Equal(existingEmail, (await db.Users.SingleAsync()).Email);
        Assert.Empty(await db.Products.ToListAsync());
        Assert.Empty(await db.Productions.ToListAsync());
    }

    private static ProductionDbContext CreateDatabase() => new(
        new DbContextOptionsBuilder<ProductionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
