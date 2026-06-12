var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Esta lista funciona como una "base de datos" temporal.
// Cada vez que se apaga la API, los datos nuevos se pierden.
var coleccionNodos = new List<NodoElemento>
{
    new NodoElemento { Id = 10, Valor = "Raiz Inicial (ABB)" },
    new NodoElemento { Id = 5, Valor = "Hijo Izquierdo" }
};

// GET: devuelve todos los nodos guardados en memoria.
app.MapGet("/api/nodos", () =>
{
    return Results.Ok(coleccionNodos);
});

// POST: recibe un nodo nuevo y lo agrega a la lista.
app.MapPost("/api/nodos", (NodoElemento nuevoNodo) =>
{
    if (nuevoNodo.Id <= 0 || string.IsNullOrWhiteSpace(nuevoNodo.Valor))
    {
        return Results.BadRequest("Datos del nodo invalidos.");
    }

    coleccionNodos.Add(nuevoNodo);
    return Results.Created($"/api/nodos/{nuevoNodo.Id}", nuevoNodo);
});

app.Run();

public class NodoElemento
{
    public int Id { get; set; }
    public string Valor { get; set; } = string.Empty;
}
