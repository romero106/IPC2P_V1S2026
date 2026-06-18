# Actividad 17 de junio de 2026
## Arquitectura Multi-Nivel (N-Tier) y Patrón Lógico de Software (MVC) en .NET

## Parte 1: Fundamentacion teorica y analisis critico

### 1. El transito hacia los sistemas distribuidos y multi-capa

**Limitacion del monolito local**

Cuando la interfaz, la logica del negocio y el almacenamiento de datos estan en una sola maquina fisica aislada, el sistema depende completamente de ese equipo. Esto provoca problemas de sincronizacion, porque otros usuarios no pueden consultar o actualizar los mismos datos de forma centralizada. Tambien limita la escalabilidad, ya que para atender mas usuarios no basta con mejorar una parte del sistema, sino que se debe depender de la capacidad de una sola maquina.

**Distincion critica entre Layers y Tiers**

Las capas logicas o layers representan la forma en que se organiza el codigo dentro de una aplicacion. Por ejemplo, una capa puede encargarse de la presentacion, otra de la logica y otra de los datos.

Los niveles fisicos o tiers representan donde se ejecuta cada parte del sistema. Por ejemplo, la presentacion puede estar en el navegador del usuario, la logica en un servidor web y la base de datos en otro servidor.

**Responsabilidades en la arquitectura de 3 niveles**

| Nivel fisico                  | Mision principal                                                    | Tecnologia comun                                             |
| ----------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------ |
| Nivel 1: Presentacion         | Mostrar la informacion al usuario y recibir sus acciones.           | Navegador web, HTML, CSS, JavaScript, Razor Views.           |
| Nivel 2: Aplicacion o negocio | Procesar reglas del sistema, validar datos y coordinar operaciones. | ASP.NET Core, C#, controladores y servicios.                 |
| Nivel 3: Datos                | Guardar y consultar la informacion de forma persistente.            | SQL Server, MySQL, PostgreSQL u otro motor de base de datos. |

**Seguridad perimetral**

Exponer publicamente el puerto de una base de datos en internet es un error critico porque permite que atacantes intenten conectarse directamente al almacenamiento del sistema. Esto aumenta el riesgo de robo, modificacion o eliminacion de datos. La buena practica es mantener la base de datos en una red privada y permitir que solo el servidor de aplicacion se comunique con ella.

### 2. Desacoplamiento logico con el patron MVC

**Crisis del codigo espagueti**

Cuando se mezclan sentencias SQL, calculos y etiquetas visuales en un mismo archivo, el mantenimiento se vuelve dificil. Un cambio visual puede romper la logica del negocio, y una modificacion en datos puede afectar la interfaz. Tambien se dificulta la creacion de pruebas unitarias, porque no existe una separacion clara entre lo que se quiere probar.

**Separacion de preocupaciones**

| Componente  | Descripcion                                                                                                                                                                              |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Modelo      | Representa los datos y reglas principales del dominio. No debe conocer como se muestran los datos, porque su responsabilidad no es dibujar pantallas.                                    |
| Vista       | Muestra la informacion al usuario. Se considera pasiva porque recibe datos ya preparados. No debe contener consultas SQL ni reglas fuertes de negocio.                                   |
| Controlador | Recibe la peticion, decide que accion ejecutar, obtiene datos del modelo y selecciona la vista o respuesta adecuada. Funciona como intermediario entre el usuario, el modelo y la vista. |

**Metricas de ingenieria de software**

MVC ayuda a lograr alta cohesion porque cada componente tiene una responsabilidad clara. El modelo maneja datos, la vista muestra informacion y el controlador coordina la peticion. Tambien permite bajo acoplamiento porque los componentes no dependen excesivamente entre si, lo que facilita modificar, probar y mantener el sistema.

## Parte 2: Modelado del ciclo de vida y enrutamiento semantico

### 1. Mapeo analitico de URLs

Plantilla utilizada:

```text
{controller=Home}/{action=Index}/{id?}
```

| URL entrante del cliente                                       | Clase controladora buscada por el framework | Metodo ejecutado | Parametro id inyectado |
| -------------------------------------------------------------- | ------------------------------------------- | ---------------- | ---------------------- |
| `https://ingenieria.usac.edu.gt/ControlAcademico/Login`        | `ControlAcademicoController`                | `Login`          | Ninguno                |
| `https://ingenieria.usac.edu.gt/Estudiante/Historial/20260123` | `EstudianteController`                      | `Historial`      | `20260123`             |
| `https://ingenieria.usac.edu.gt/Asignacion/Detalle/10`         | `AsignacionController`                      | `Detalle`        | `10`                   |
| `https://ingenieria.usac.edu.gt/Home`                          | `HomeController`                            | `Index`          | Ninguno / opcional     |

### 2. Diagramacion del flujo interactivo

1. El usuario hace clic en un boton o enlace desde el navegador, y el navegador envia una peticion HTTP al servidor.
2. ASP.NET Core recibe la peticion y el sistema de enrutamiento analiza la URL para identificar el controlador, la accion y el parametro opcional.
3. El controlador ejecuta el metodo correspondiente, valida la peticion y solicita la informacion necesaria al modelo.
4. El modelo representa los datos del sistema y devuelve la informacion solicitada al controlador.
5. El controlador envia los datos a la vista, la vista genera HTML dinamico y el servidor responde al navegador con la pagina renderizada.

## Parte 3: Implementacion practica

Para la parte practica se creo un proyecto web llamado `ControlAcademicoMvc`. En el proyecto se aplico el patron MVC de forma sencilla, separando el codigo en archivos independientes:

- `Models/Estudiante.cs`: contiene la entidad `Estudiante` con carne, nombre y promedio.
- `Controllers/EstudianteController.cs`: contiene las acciones `Listar`, `Historial` y `Registrar`.
- `Views/Estudiante/Listar.cshtml`: muestra una tabla con los estudiantes registrados.
- `Views/Estudiante/Historial.cshtml`: muestra la informacion de un estudiante especifico.
- `Program.cs`: configura el enrutamiento convencional `{controller=Home}/{action=Index}/{id?}`.

El controlador se mantuvo delgado porque sus metodos solo reciben la peticion, validan datos basicos y devuelven una respuesta. No se mezclaron sentencias SQL, calculos complejos ni codigo visual dentro del controlador.

## Parte 4: Auditoria y control de calidad

**Prueba de cohesion GET**

Se preparo la accion `GET /Estudiante/Listar` para mostrar la informacion de estudiantes desde una lista en memoria. La respuesta mantiene separadas las responsabilidades: el controlador envia los datos y la vista se encarga de mostrarlos en HTML.

**Evaluacion de antipatrones**

Se reviso `EstudianteController.cs` y sus metodos se mantuvieron con menos de 20 lineas de codigo. Por eso no se detecto el antipatron de Controlador Gordo. La logica se mantuvo simple y separada en modelo, controlador y vista.

## Parte 5: Referencias bibliograficas

Facultad de Ingenieria, USAC. (2026). *Sesion 11: Modelado Base y Arquitecturas de Despliegue. Evolucion de Sistemas Distribuidos, Fundamentos del Modelo Cliente-Servidor y Diseno Fisico Multi-Capas (N-Tier).* Laboratorio del curso Introduccion a la Programacion y Computacion 2. Guatemala.

Facultad de Ingenieria, USAC. (2026). *Sesion 12: Arquitectura y Componentes del Patron MVC. Desacoplamiento Logico de Software, Ciclo de Vida de las Peticiones y Enrutamiento en Aplicaciones Interactivas Modernas.* Laboratorio del curso Introduccion a la Programacion y Computacion 2. Guatemala.
