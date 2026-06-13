var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simulación del estado del árbol en memoria.
var estadoArbol = new List<NodoAVL>
{
    // Estado inicial desbalanceado en Zig-Zag: 30 -> 10 -> 20
    new NodoAVL { Id = 30, Etiqueta = "Nodo Raíz (Abuelo) - FE: -2" },
    new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: +1" }
};

// ENDPOINT GET: sirve para consultar el estado actual del árbol.
app.MapGet("/api/arbol", () =>
{
    return Results.Ok(estadoArbol);
});

// ENDPOINT POST: sirve para insertar un nodo y simular el balanceo compuesto.
app.MapPost("/api/arbol/insertar", (NodoAVL nuevoNodo) =>
{
    // Validación básica: no aceptamos IDs negativos ni cero.
    if (nuevoNodo.Id <= 0)
    {
        return Results.BadRequest("ID de nodo inválido.");
    }

    // Al insertar el nodo 20, se detecta el caso cruzado Izquierda-Derecha.
    if (nuevoNodo.Id == 20)
    {
        // Se limpia el estado anterior desbalanceado.
        estadoArbol.Clear();

        // Resultado final después de la rotación doble RID.
        estadoArbol.Add(new NodoAVL { Id = 20, Etiqueta = "Nueva Raíz Balanceada (RID) - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 30, Etiqueta = "Hijo Derecho - FE: 0" });

        return Results.Created("/api/arbol", new
        {
            Mensaje = "Rotación RID ejecutada con éxito. Estabilidad total lograda.",
            Estructura = estadoArbol
        });
    }

    // Si se inserta otro nodo que no sea 20, solo se agrega a la lista.
    estadoArbol.Add(nuevoNodo);

    return Results.Created($"/api/arbol/{nuevoNodo.Id}", nuevoNodo);
});

app.Run();

public class NodoAVL
{
    public int Id { get; set; }
    public string Etiqueta { get; set; } = string.Empty;
    public int Altura { get; set; } = 1;
}