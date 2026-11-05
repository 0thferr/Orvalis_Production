using Microsoft.AspNetCore.Mvc;
using SequorProduction.Api.DTOs;
using SequorProduction.Api.Services;

namespace SequorProduction.Api.Controllers;
[ApiController]
[Route("api/orders")]
public sealed class OrdersController(ProductionService service) : ControllerBase
{
    [HttpGet("GetOrders")]
    public async Task<ActionResult<OrdersResponse>> GetOrders(CancellationToken ct) => Ok(new OrdersResponse(await service.GetOrdersAsync(ct)));

    [HttpGet("GetProduction")]
    public async Task<ActionResult<ProductionsResponse>> GetProduction([FromQuery] string email, CancellationToken ct) =>
        Ok(new ProductionsResponse(await service.GetProductionsAsync(email ?? "", ct)));

    [HttpPost("SetProduction")]
    public async Task<IActionResult> SetProduction([FromBody] SetProductionRequest request, CancellationToken ct)
    {
        var response = await service.SetProductionAsync(request, ct);
        return response.Status == 200 ? Ok(response) : BadRequest(response);
    }
}
