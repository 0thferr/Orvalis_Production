using Microsoft.EntityFrameworkCore;
using SequorProduction.Api.Models;

namespace SequorProduction.Api.Data;
public sealed class ProductionDbContext(DbContextOptions<ProductionDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<ProductMaterial> ProductMaterials => Set<ProductMaterial>();
    public DbSet<ProductionUser> Users => Set<ProductionUser>();
    public DbSet<Production> Productions => Set<Production>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e => { e.ToTable("Product"); e.HasKey(x => x.ProductCode); e.Property(x => x.ProductCode).HasMaxLength(50).IsUnicode(false); e.Property(x => x.ProductDescription).HasMaxLength(50).IsUnicode(false).IsRequired(); e.Property(x => x.Image).HasMaxLength(500).IsUnicode(false).IsRequired(); e.Property(x => x.CycleTime).HasPrecision(18, 2); });
        modelBuilder.Entity<Order>(e => { e.ToTable("Order"); e.HasKey(x => x.OrderNumber); e.Property(x => x.OrderNumber).HasColumnName("Order").HasMaxLength(50).IsUnicode(false); e.Property(x => x.ProductCode).HasMaxLength(50).IsUnicode(false); e.Property(x => x.Quantity).HasPrecision(18, 2); e.HasOne(x => x.Product).WithMany(x => x.Orders).HasForeignKey(x => x.ProductCode).OnDelete(DeleteBehavior.Restrict); });
        modelBuilder.Entity<Material>(e => { e.ToTable("Material"); e.HasKey(x => x.MaterialCode); e.Property(x => x.MaterialCode).HasMaxLength(50).IsUnicode(false); e.Property(x => x.MaterialDescription).HasMaxLength(500).IsUnicode(false).IsRequired(); });
        modelBuilder.Entity<ProductMaterial>(e => { e.ToTable("ProductMaterial"); e.HasKey(x => new { x.ProductCode, x.MaterialCode }); e.Property(x => x.ProductCode).HasMaxLength(50).IsUnicode(false); e.Property(x => x.MaterialCode).HasMaxLength(50).IsUnicode(false); e.HasOne(x => x.Product).WithMany(x => x.ProductMaterials).HasForeignKey(x => x.ProductCode); e.HasOne(x => x.Material).WithMany(x => x.ProductMaterials).HasForeignKey(x => x.MaterialCode); });
        modelBuilder.Entity<ProductionUser>(e => { e.ToTable("User"); e.HasKey(x => x.Email); e.Property(x => x.Email).HasMaxLength(100).IsUnicode(false); e.Property(x => x.Name).HasMaxLength(50).IsUnicode(false).IsRequired(); e.Property(x => x.InitialDate).HasColumnType("datetime"); e.Property(x => x.EndDate).HasColumnType("datetime"); });
        modelBuilder.Entity<Production>(e => { e.ToTable("Production"); e.HasKey(x => x.Id); e.Property(x => x.Id).UseIdentityColumn(); e.Property(x => x.Email).HasMaxLength(100).IsUnicode(false); e.Property(x => x.OrderNumber).HasColumnName("Order").HasMaxLength(50).IsUnicode(false); e.Property(x => x.MaterialCode).HasMaxLength(50).IsUnicode(false); e.Property(x => x.Date).HasColumnType("datetime"); e.Property(x => x.Quantity).HasPrecision(18, 2); e.Property(x => x.CycleTime).HasPrecision(18, 2); e.HasOne(x => x.User).WithMany(x => x.Productions).HasForeignKey(x => x.Email).OnDelete(DeleteBehavior.Restrict); e.HasOne(x => x.Order).WithMany(x => x.Productions).HasForeignKey(x => x.OrderNumber).OnDelete(DeleteBehavior.Restrict); e.HasOne(x => x.Material).WithMany().HasForeignKey(x => x.MaterialCode).OnDelete(DeleteBehavior.Restrict); e.HasIndex(x => new { x.Email, x.Date }); });
    }
}
