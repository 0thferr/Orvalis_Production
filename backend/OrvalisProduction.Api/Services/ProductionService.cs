using Microsoft.EntityFrameworkCore;
using OrvalisProduction.Api.Data;
using OrvalisProduction.Api.DTOs;
using OrvalisProduction.Api.Models;

namespace OrvalisProduction.Api.Services;

public sealed class ProductionService(ProductionDbContext db)
{
    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(CancellationToken ct) =>
        await db.Orders.AsNoTracking()
            .OrderBy(order => order.OrderNumber)
            .Select(order => new OrderDto(
                order.OrderNumber,
                order.Quantity,
                order.ProductCode,
                order.Product.ProductDescription,
                order.Product.Image,
                order.Product.CycleTime,
                order.Product.ProductMaterials
                    .OrderBy(material => material.MaterialCode)
                    .Select(material => new MaterialDto(material.MaterialCode, material.Material.MaterialDescription))
                    .ToList()))
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ProductionDto>> GetProductionsAsync(string email, CancellationToken ct) =>
        await db.Productions.AsNoTracking()
            .Where(production => production.Email == email)
            .OrderByDescending(production => production.Date)
            .Select(production => new ProductionDto(
                production.OrderNumber, production.Date, production.Quantity, production.MaterialCode, production.CycleTime))
            .ToListAsync(ct);

    public async Task<OperationResponse> SetProductionAsync(SetProductionRequest request, CancellationToken ct)
    {
        OperationResponse Error(string text) => new(201, "E", $"Falha no apontamento - {text}");
        if (string.IsNullOrWhiteSpace(request.Email)) return Error("E-mail obrigatório!");
        if (string.IsNullOrWhiteSpace(request.Order)) return Error("Ordem obrigatória!");
        if (string.IsNullOrWhiteSpace(request.MaterialCode)) return Error("Material obrigatório!");

        var email = request.Email.Trim();
        var orderNumber = request.Order.Trim();
        var materialCode = request.MaterialCode.Trim();
        var user = await db.Users.SingleOrDefaultAsync(user => user.Email == email, ct);
        if (user is null) return Error("Usuário não cadastrado!");

        var order = await db.Orders.Include(order => order.Product)
            .SingleOrDefaultAsync(order => order.OrderNumber == orderNumber, ct);
        if (order is null) return Error("Ordem não cadastrada!");

        var date = request.ProductionDate.ToDateTime(request.ProductionTime);
        if (date < user.InitialDate || date > user.EndDate) return Error("Data fora do período permitido para o usuário!");
        if (request.Quantity <= 0) return Error("Quantidade deve ser maior que zero!");
        if (request.Quantity > order.Quantity) return Error("Quantidade superior à quantidade da ordem!");

        var materialAllowed = await db.ProductMaterials.AnyAsync(
            material => material.ProductCode == order.ProductCode && material.MaterialCode == materialCode, ct);
        if (!materialAllowed) return Error("Material não vinculado ao produto da ordem!");
        if (request.CycleTime <= 0) return Error("Tempo de ciclo deve ser maior que zero!");

        db.Productions.Add(new Production
        {
            Email = user.Email,
            OrderNumber = order.OrderNumber,
            Date = date,
            Quantity = request.Quantity,
            MaterialCode = materialCode,
            CycleTime = request.CycleTime
        });
        await db.SaveChangesAsync(ct);

        var message = request.CycleTime < order.Product.CycleTime
            ? "Apontamento realizado com sucesso! Tempo informado inferior ao tempo de ciclo cadastrado."
            : "Apontamento realizado com sucesso!";
        return new OperationResponse(200, "S", message);
    }
}
