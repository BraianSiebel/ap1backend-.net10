var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var relogios = new List<RelogioDto>
{
    new RelogioDto(1, "Casio", "F-91W", "Digital", 149.90m),
    new RelogioDto(2, "Seiko", "Presage SRPB43", "Automático", 3499.00m),
    new RelogioDto(3, "Apple", "Watch Series 10", "Smartwatch", 4299.00m)
};

app.MapGet("/", () => Results.Ok(new { mensagem = "API de Relógios está no ar" }));

app.MapGet("/api/relogios", () =>
{
    return Results.Ok(relogios);
});

app.MapGet("/api/relogios/{id:int}", (int id) =>
{
    var relogio = relogios.Find(relogioDaLista => relogioDaLista.Id == id);

    if (relogio is null)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    return Results.Ok(relogio);
});

app.MapPost("/api/relogios", (RelogioEntradaDto dados) =>
{
    int proximoId = relogios.Count == 0 ? 1 : relogios.Max(relogio => relogio.Id) + 1;

    var novoRelogio = new RelogioDto(proximoId, dados.Marca, dados.Modelo, dados.Tipo, dados.Preco);

    relogios.Add(novoRelogio);

    return Results.Created($"/api/relogios/{novoRelogio.Id}", novoRelogio);
});

app.MapPut("/api/relogios/{id:int}", (int id, RelogioEntradaDto dados) =>
{
    int indice = relogios.FindIndex(relogioDaLista => relogioDaLista.Id == id);

    if (indice == -1)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    var relogioAtualizado = new RelogioDto(id, dados.Marca, dados.Modelo, dados.Tipo, dados.Preco);

    relogios[indice] = relogioAtualizado;

    return Results.Ok(relogioAtualizado);
});

app.MapDelete("/api/relogios/{id:int}", (int id) =>
{
    int indice = relogios.FindIndex(relogioDaLista => relogioDaLista.Id == id);

    if (indice == -1)
    {
        return Results.NotFound(new { mensagem = $"Relógio com id {id} não encontrado" });
    }

    relogios.RemoveAt(indice);

    return Results.NoContent();
});

app.Run();

record RelogioDto(int Id, string Marca, string Modelo, string Tipo, decimal Preco);

record RelogioEntradaDto(string Marca, string Modelo, string Tipo, decimal Preco);
