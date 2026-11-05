namespace SequorProduction.Api.DTOs;
public sealed record MaterialDto(string MaterialCode, string MaterialDescription);
public sealed record OrderDto(string Order, decimal Quantity, string ProductCode, string ProductDescription, string Image, decimal CycleTime, IReadOnlyList<MaterialDto> Materials);
public sealed record OrdersResponse(IReadOnlyList<OrderDto> Orders);
