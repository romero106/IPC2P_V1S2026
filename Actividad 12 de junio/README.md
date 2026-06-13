# Actividad 12 de junio de 2026
## Balanceo Compuesto en Árboles AVL y Exposición de Estructuras vía Web APIs

## Parte 1: Investigacion teorica

### 1. El límite de las rotaciones simples y el desbalanceo en “Zig-Zag”
Un **árbol AVL** es un árbol binario de búsqueda que intenta mantenerse balanceado. Para saber si un nodo está balanceado se usa el **Factor de Equilibrio**, abreviado como **Fe**.

```math
FE = Altura_{\text{ lado izquierdo}} - Altura_{\text{ lado derecho}}
```

Con esa convención:

- Si el FE es `0`, el nodo está perfectamente balanceado.
- Si el FE es `+1`, el lado derecho está un poco más alto.
- Si el FE es `-1`, el lado izquierdo está un poco más alto.
- Si el FE llega a `+2` o `-2`, el árbol está desbalanceado y necesita rotación.

El problema aparece cuando la inserción forma un caso cruzado o en **Zig Zag**. Por ejemplo, al insertar los valores: 

```math
30, 10, 20
```

La estructura queda así:

```text
    30
   /
 10
   \
    20
```
El nodo `30` queda cargado hacia la izquierda, pero su hijo izquierdo `10` está cargado hacia la derecha y ese cruce hace que una rotación simple no sea suficiente.

Si se aplica solamente una rotación simple, el árbol no queda correctamente ordenado ni balanceado. La rotación simple solo cambia el problema de lado, porque el nodo intermedio `20` queda mal ubicado. Por eso se necesita una **rotación doble Izquierda-Derecha**, también llamada **RID**.

La condición matemática para una **RID** es:

```text
FE(nodo padre) = -2
FE(hijo izquierdo) = +1
```

De forma más general:

```math
FE_{ padre} < -1  \quad \text{y} \quad  FE_{ padre izquierdo} > 0
```

Esto significa:

1. El nodo padre está demasiado cargado a la izquierda.
2. Su hijo izquierdo está cargado hacia la derecha.
3. La inserción ocurrió en el subárbol derecho del hijo izquierdo.
4. Por eso se debe hacer una rotación doble.

La RID se puede entender como dos pasos:

1. Primero, una rotación simple hacia la izquierda sobre el hijo izquierdo.
2. Después, una rotación simple hacia la derecha sobre el nodo padre.

Aplicado al ejemplo:

Antes:

```text
    30
   /
 10
   \
    20
```

Después de la RID:

```text
   20
  /  \
10    30
```

Ahora el nodo `20` queda como nueva raíz del subárbol y los nodos `10` y `30` quedan balanceados.

---

## 1.2 Principio DRY en rotaciones dobles

El principio **DRY** significa **Don’t Repeat Yourself**, es decir, “no te repitas”. En programación, este principio recomienda no escribir la misma lógica varias veces si puede reutilizarse.

En el caso de árboles AVL, una rotación doble como RID o RDI puede construirse reutilizando rotaciones simples.

Por ejemplo:

```text
RID = rotación simple izquierda + rotación simple derecha
RDI = rotación simple derecha + rotación simple izquierda
```

La ventaja de hacerlo así es que el código queda más limpio, más fácil de revisar y con menos riesgo de errores. Si se reasignan punteros manualmente desde cero en cada rotación doble, es más probable equivocarse con alguna conexión del árbol.

En cambio, si ya existen funciones confiables para rotar a la izquierda y rotar a la derecha, entonces las rotaciones dobles pueden llamar esas funciones. Esto permite:

- Evitar código repetido.
- Reducir errores al mover referencias o punteros.
- Facilitar el mantenimiento del programa.
- Hacer que el código sea más fácil de entender.
- Corregir una rotación simple en un solo lugar si hubiera un error.

---
# 2. Fundamentos de Arquitectura Web y Protocolo HTTP

## 2.1 Modelo cliente-servidor

El modelo **cliente-servidor** es una forma de comunicación donde una parte pide información y otra parte la responde.

El **cliente** puede ser:

- Un navegador web.
- Postman.
- Thunder Client.
- Una aplicación móvil.
- Otro programa.

El **servidor** es el programa que recibe la petición, procesa la información y devuelve una respuesta.

En esta actividad, el servidor será nuestra API hecha en C# con ASP.NET Core. El cliente puede ser el navegador o una herramienta para probar peticiones.

El proceso funciona así:

1. El cliente envía una **Request** o petición.
2. La petición viaja por HTTP hacia el servidor.
3. El servidor revisa qué endpoint fue solicitado.
4. El servidor ejecuta la lógica correspondiente.
5. El servidor devuelve una **Response** o respuesta.
6. La respuesta puede incluir datos en formato JSON.

Ejemplo:

```text
Cliente → GET /api/arbol → Servidor
Cliente ← JSON con el árbol ← Servidor
```

La respuesta normalmente incluye:

- Un código de estado HTTP, por ejemplo `200 OK` o `201 Created`.
- Datos, muchas veces en formato JSON.
- Encabezados o información técnica de la respuesta.

---

## 2.2 Diferencia técnica entre GET y POST

Los métodos HTTP indican qué quiere hacer el cliente con un recurso.

### GET

El método **GET** se usa para pedir o consultar información. No debería modificar los datos del servidor.

En esta actividad, GET se usa para recuperar el estado actual del árbol AVL simulado.

Endpoint usado:

```text
GET /api/arbol
```

Su función será devolver la estructura del árbol guardada en memoria.

### POST

El método **POST** se usa para enviar datos al servidor. Normalmente se utiliza para crear, insertar o modificar información.

En esta actividad, POST se usa para insertar un nuevo nodo y simular el balanceo del árbol.

Endpoint usado:

```text
POST /api/arbol/insertar
```

Cuando se envía el nodo con `id = 20`, la API simula la rotación RID y reorganiza el árbol.

---

## Parte 2: Implementacion practica

### 1. Creación del proyecto

Se creó un nuevo proyecto de Web API en C# utilizando ASP.NET Core.

```bash
dotnet new webapi -o ApiAvlSimulacion
cd ApiAvlSimulacion
```

### 2. Configuración del archivo `Program.cs`

Se modificó el archivo `Program.cs` para crear una API mínima que simula el comportamiento de un árbol AVL en memoria.

Se definió una lista llamada `estadoArbol`, la cual almacena temporalmente los nodos del árbol.

```csharp
var estadoArbol = new List<NodoAVL>
{
    new NodoAVL { Id = 30, Etiqueta = "Nodo Raíz (Abuelo) - FE: -2" },
    new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: +1" }
};
```

### 3. Creación del modelo `NodoAVL`

Se creó la clase `NodoAVL`, la cual representa cada nodo del árbol.

```csharp
public class NodoAVL
{
    public int Id { get; set; }
    public string Etiqueta { get; set; } = string.Empty;
    public int Altura { get; set; } = 1;
}
```

### 4. Implementación del endpoint GET

Se implementó un endpoint GET para consultar el estado actual del árbol.

```csharp
app.MapGet("/api/arbol", () =>
{
    return Results.Ok(estadoArbol);
});
```

Este endpoint permite visualizar los nodos almacenados en memoria.

### 5. Implementación del endpoint POST

Se implementó un endpoint POST para insertar un nuevo nodo y simular el balanceo del árbol.

```csharp
app.MapPost("/api/arbol/insertar", (NodoAVL nuevoNodo) =>
{
    if (nuevoNodo.Id <= 0)
    {
        return Results.BadRequest("ID de nodo inválido.");
    }

    if (nuevoNodo.Id == 20)
    {
        estadoArbol.Clear();

        estadoArbol.Add(new NodoAVL { Id = 20, Etiqueta = "Nueva Raíz Balanceada (RID) - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 30, Etiqueta = "Hijo Derecho - FE: 0" });

        return Results.Created("/api/arbol", new
        {
            Mensaje = "Rotación RID ejecutada con éxito. Estabilidad total lograda.",
            Estructura = estadoArbol
        });
    }

    estadoArbol.Add(nuevoNodo);

    return Results.Created($"/api/arbol/{nuevoNodo.Id}", nuevoNodo);
});
```

### 6. Ejecución del proyecto

Se ejecutó la API desde la terminal con el siguiente comando:

```bash
dotnet run
```

La aplicación quedó disponible en la dirección local:

```text
http://localhost:5219
```

### 7. Prueba del endpoint GET

Se probó el endpoint GET desde el navegador y desde el archivo `.http`.

```http
GET http://localhost:5219/api/arbol
Accept: application/json
```

La respuesta inicial mostró los nodos `30` y `10`, representando el estado desbalanceado del árbol.

### 8. Prueba del endpoint POST

Se probó el endpoint POST desde el archivo `.http`.

```http
POST http://localhost:5219/api/arbol/insertar
Content-Type: application/json

{
  "id": 20,
  "etiqueta": "Nieto Derecho"
}
```

La API respondió con el estado:

```text
HTTP/1.1 201 Created
```

Además, devolvió la nueva estructura del árbol después de la rotación RID.

### 9. Verificación final

Después de insertar el nodo `20`, se consultó nuevamente el endpoint GET.

El resultado final mostró el árbol balanceado con el siguiente orden:

```text
20 - Nueva Raíz Balanceada
10 - Hijo Izquierdo
30 - Hijo Derecho
```

Esto confirma que la simulación de la rotación doble Izquierda-Derecha se ejecutó correctamente.