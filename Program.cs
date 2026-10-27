using ApiRelogios.Data;
using ApiRelogios.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Serve o frontend (wwwroot/index.html) pela mesma origem da API
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api", () => Results.Ok(new { mensagem = "API de Relógios está no ar" }));

app.MapGet("/api/relogios", async (AppDbContext db) =>
{
    var relogios = await db.Relogios
        .OrderBy(relogio => relogio.Id)
        .Select(relogio => new RelogioDto(relogio.Id, relogio.Marca, relogio.Modelo, relogio.Tipo, relogio.Preco))
        .ToListAsync();

    return Results.Ok(relogios);
});

app.MapGet("/api/relogios/{id:int}", async (int id, AppDbContext db) =>
{
    var relogio = await db.Relogios.FindAsync(id);

    if (relogio is null)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    return Results.Ok(ParaDto(relogio));
});

app.MapPost("/api/relogios", async (RelogioEntradaDto dados, AppDbContext db) =>
{
    var novoRelogio = new Relogio
    {
        Marca = dados.Marca,
        Modelo = dados.Modelo,
        Tipo = dados.Tipo,
        Preco = dados.Preco
    };

    db.Relogios.Add(novoRelogio);
    await db.SaveChangesAsync();

    return Results.Created($"/api/relogios/{novoRelogio.Id}", ParaDto(novoRelogio));
});

app.MapPut("/api/relogios/{id:int}", async (int id, RelogioEntradaDto dados, AppDbContext db) =>
{
    var relogio = await db.Relogios.FindAsync(id);

    if (relogio is null)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    relogio.Marca = dados.Marca;
    relogio.Modelo = dados.Modelo;
    relogio.Tipo = dados.Tipo;
    relogio.Preco = dados.Preco;

    await db.SaveChangesAsync();

    return Results.Ok(ParaDto(relogio));
});

app.MapDelete("/api/relogios/{id:int}", async (int id, AppDbContext db) =>
{
    var relogio = await db.Relogios.FindAsync(id);

    if (relogio is null)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    db.Relogios.Remove(relogio);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

static RelogioDto ParaDto(Relogio relogio) =>
    new(relogio.Id, relogio.Marca, relogio.Modelo, relogio.Tipo, relogio.Preco);

record RelogioDto(int Id, string Marca, string Modelo, string Tipo, decimal Preco);

record RelogioEntradaDto(string Marca, string Modelo, string Tipo, decimal Preco);
