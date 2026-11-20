using System.Text.Json;

namespace OrvalisProduction.Api.Services;
public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao processar requisição {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { status = 500, type = "E", description = "Ocorreu um erro interno. Tente novamente mais tarde." }));
        }
    }
}
