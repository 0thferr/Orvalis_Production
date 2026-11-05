namespace SequorProduction.Api.DTOs;
public sealed record ProductionDto(string Order, DateTime Date, decimal Quantity, string MaterialCode, decimal CycleTime);
public sealed record ProductionsResponse(IReadOnlyList<ProductionDto> Productions);
public sealed record SetProductionRequest
{
    public string Email { get; init; } = "";
    public string Order { get; init; } = "";
    public DateOnly ProductionDate { get; init; }
    public TimeOnly ProductionTime { get; init; }
    public decimal Quantity { get; init; }
    public string MaterialCode { get; init; } = "";
    public decimal CycleTime { get; init; }
}
public sealed record OperationResponse(int Status, string Type, string Description);
