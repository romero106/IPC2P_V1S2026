# Actividad 11 de junio
## Estructuras de Datos Avanzadas y APIs con ASP.NET Core

## Parte 1: Investigacion teorica

### 1. Estructuras de datos eficientes

#### Arboles Binarios de Busqueda (ABB)

Un Arbol Binario de Busqueda, tambien llamado ABB, es una forma de guardar datos ordenados.

La regla principal es:

- Los valores menores van al lado izquierdo.
- Los valores mayores van al lado derecho.

Ejemplo:

```text
        10
       /  \
      5    15
```

En este ejemplo:

- `5` esta a la izquierda porque es menor que `10`.
- `15` esta a la derecha porque es mayor que `10`.

El problema aparece cuando insertamos datos en orden, por ejemplo: `1, 2, 3, 4, 5`.

El arbol podria quedar asi:

```text
1
 \
  2
   \
    3
     \
      4
       \
        5
```

Aunque tecnicamente sigue siendo un arbol, se comporta casi como una lista. Esto es malo porque buscar un dato puede tardar mas, ya que hay que revisar muchos nodos uno por uno.

#### Arboles AVL

Un Arbol AVL es un arbol que se auto-balancea. Eso significa que se acomoda solo para no quedar demasiado inclinado hacia un lado.

Para saber si esta balanceado se usa el factor de balanceo:

```math
Factor = \text{Altura del lado izquierdo} - \text{Altura del lado derecho}
```

Si el arbol se inclina demasiado, el AVL hace rotaciones para acomodarse.

La ventaja es que las operaciones principales se mantienen rapidas:

- Buscar.
- Insertar.
- Eliminar.

Estas operaciones tienen complejidad `O(log n)`.

En palabras sencillas, significa que aunque haya muchos datos, el arbol no necesita revisar todos. Puede descartar grupos grandes de datos en cada paso.

### 2. Fundamentos de Web APIs

#### Que es una API

Una API es una forma de comunicacion entre programas.

Por ejemplo, una pagina web puede pedirle informacion a una API. La API recibe la peticion, busca o guarda datos, y responde con un resultado.

#### Modelo Cliente-Servidor

En este modelo hay dos partes:

- Cliente: quien pide algo. Puede ser una pagina web, Postman, Bruno o una app.
- Servidor: quien recibe la peticion y responde.

Ejemplo sencillo:

```text
Cliente -> "Dame la lista de nodos" -> Servidor
Cliente <- "Aqui esta la lista en JSON" <- Servidor
```

La comunicacion normalmente viaja usando HTTP.

Una peticion se llama `Request`.
Una respuesta se llama `Response`.

#### GET

`GET` se usa para pedir informacion.

Ejemplo:

```text
GET /api/nodos
```

Esto significa: "quiero ver los nodos".

`GET` no deberia cambiar los datos. Solo debe consultar.

Tambien es idempotente. Esto quiere decir que si haces la misma peticion varias veces, el resultado esperado no deberia cambiar por culpa de la peticion.

#### POST

`POST` se usa para crear un recurso nuevo.

Ejemplo:

```text
POST /api/nodos
```

Esto significa: "quiero agregar un nodo nuevo".

`POST` no es idempotente. Si mandas la misma peticion varias veces, podrias crear varios registros iguales o parecidos.

## Parte 2: Implementacion practica

Se creo una Web API en C# con ASP.NET Core.

El proyecto se llama:

```text
ApiEstructurasDemo
```

La API trabaja con un recurso llamado `NodoElemento`.

Cada nodo tiene:

- `Id`: numero que identifica al nodo.
- `Valor`: texto descriptivo del nodo.

Ejemplo:

```json
{
  "id": 15,
  "valor": "Nuevo Nodo Derecho"
}
```

### Codigo principal usado

El archivo principal es `Program.cs`.

La API guarda los datos en una lista en memoria:

```csharp
var coleccionNodos = new List<NodoElemento>
{
    new NodoElemento { Id = 10, Valor = "Raiz Inicial (ABB)" },
    new NodoElemento { Id = 5, Valor = "Hijo Izquierdo" }
};
```

Esto significa que al iniciar la API ya existen dos nodos.

### Endpoint GET

Ruta:

```text
/api/nodos
```

Sirve para ver todos los nodos.

Respuesta esperada:

```json
[
  {
    "id": 10,
    "valor": "Raiz Inicial (ABB)"
  },
  {
    "id": 5,
    "valor": "Hijo Izquierdo"
  }
]
```

### Endpoint POST

Ruta:

```text
/api/nodos
```

Sirve para agregar un nodo nuevo.

Ejemplo de cuerpo JSON:

```json
{
  "id": 15,
  "valor": "Nuevo Nodo Derecho"
}
```

Si los datos son correctos, la API responde con estado `201 Created`.