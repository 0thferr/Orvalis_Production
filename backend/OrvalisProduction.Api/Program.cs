using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OrvalisProduction.Api.Data;
using OrvalisProduction.Api.DTOs;
using OrvalisProduction.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var productionConnection = builder.Configuration.GetConnectionString("Production")
    ?? throw new InvalidOperationException("A conexão Production precisa estar configurada.");

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(
        new OperationResponse(201, "E", "Falha no apontamento - Dados da requisição inválidos!"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductionDbContext>(options => options.UseSqlServer(productionConnection));
builder.Services.AddScoped<ProductionService>();
builder.Services.AddCors(options => options.AddPolicy("React", policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:5173"])
    .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseCors("React");
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductionDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.InitializeAsync(db);
}
app.Run();

public partial class Program { }
