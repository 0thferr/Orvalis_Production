using Microsoft.EntityFrameworkCore;
using SequorProduction.Api.Data;
using SequorProduction.Api.DTOs;
using SequorProduction.Api.Models;

namespace SequorProduction.Api.Services;
public sealed class ProductionService(ProductionDbContext db)
{
    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(CancellationToken ct) => await db.Orders.AsNoTracking()
        .Include(o => o.Product).ThenInclude(p => p.ProductMaterials).ThenInclude(pm => pm.Material)
        .OrderBy(o => o.OrderNumber)
        .Select(o => new OrderDto(o.OrderNumber, o.Quantity, o.ProductCode, o.Product.ProductDescription, o.Product.Image, o.Product.CycleTime,
            o.Product.ProductMaterials.OrderBy(pm => pm.MaterialCode).Select(pm => new MaterialDto(pm.MaterialCode, pm.Material.MaterialDescription)).ToList()))
        .ToListAsync(ct);

    public async Task<IReadOnlyList<ProductionDto>> GetProductionsAsync(string email, CancellationToken ct) => await db.Productions.AsNoTracking()
        .Where(p => p.Email == email).OrderByDescending(p => p.Date)
        .Select(p => new ProductionDto(p.OrderNumber, p.Date, p.Quantity, p.MaterialCode, p.CycleTime)).ToListAsync(ct);

    public async Task<OperationResponse> SetProductionAsync(SetProductionRequest request, CancellationToken ct)
    {
        OperationResponse Error(string text) => new(201, "E", $"Falha no apontamento - {text}");
        if (string.IsNullOrWhiteSpace(request.Email)) return Error("E-mail obrigatório!");
        if (string.IsNullOrWhiteSpace(request.Order)) return Error("Ordem obrigatória!");
        if (string.IsNullOrWhiteSpace(request.MaterialCode)) return Error("Material obrigatório!");
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == request.Email.Trim(), ct);
        if (user is null) return Error("Usuário não cadastrado!");
        var order = await db.Orders.Include(x => x.Product).SingleOrDefaultAsync(x => x.OrderNumber == request.Order.Trim(), ct);
        if (order is null) return Error("Ordem não cadastrada!");
        var date = request.ProductionDate.ToDateTime(request.ProductionTime);
        if (date < user.InitialDate || date > user.EndDate) return Error("Data fora do período permitido para o usuário!");
        if (request.Quantity <= 0) return Error("Quantidade deve ser maior que zero!");
        if (request.Quantity > order.Quantity) return Error("Quantidade superior à quantidade da ordem!");
        var materialAllowed = await db.ProductMaterials.AnyAsync(x => x.ProductCode == order.ProductCode && x.MaterialCode == request.MaterialCode.Trim(), ct);
        if (!materialAllowed) return Error("Material não vinculado ao produto da ordem!");
        if (request.CycleTime <= 0) return Error("Tempo de ciclo deve ser maior que zero!");
        db.Productions.Add(new Production { Email = user.Email, OrderNumber = order.OrderNumber, Date = date, Quantity = request.Quantity, MaterialCode = request.MaterialCode.Trim(), CycleTime = request.CycleTime });
        await db.SaveChangesAsync(ct);
        var message = request.CycleTime < order.Product.CycleTime
            ? "Apontamento realizado com sucesso! Tempo informado inferior ao tempo de ciclo cadastrado."
            : "Apontamento realizado com sucesso!";
        return new OperationResponse(200, "S", message);
    }
}
