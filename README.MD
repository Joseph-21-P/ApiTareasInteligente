# API Inteligente de Tareas y Análisis

## Pasos para ejecutar localmente
1. Clonar el repositorio en tu máquina local.
2. Abrir una terminal en la carpeta raíz del proyecto.
3. Ejecutar `dotnet restore` para descargar e instalar todas las dependencias.
4. Ejecutar las migraciones de la base de datos (ver la siguiente sección de comandos).
5. Ejecutar el proyecto con el comando `dotnet run`.
6. Abrir un navegador web y acceder a la ruta generada (usualmente `http://localhost:5127/swagger`) para probar los endpoints mediante la interfaz visual.

## Comandos de migración
Para generar la base de datos SQLite y aplicar el modelo de Tareas, asegúrate de tener instaladas las herramientas de Entity Framework Core y ejecuta los siguientes comandos en orden:

* `dotnet ef migrations add InicializacionModeloTarea`
* `dotnet ef database update`

---

## Endpoints implementados

### Gestión de Tareas (CRUD Local)
* **GET** `/api/tareas` (Permite filtrar mediante Query Parameters: estado, prioridad, fechaInicio, fechaFin).
* **GET** `/api/tareas/{id}` (Obtiene una tarea específica por su identificador).
* **POST** `/api/tareas` (Crea una nueva tarea aplicando las validaciones de modelo).
* **PUT** `/api/tareas/{id}` (Actualiza los datos de una tarea existente).
* **DELETE** `/api/tareas/{id}` (Elimina una tarea de la base de datos).

### Tareas Externas (Consumo de API)
* **GET** `/api/tareas-externas` (Obtiene el listado completo de la API externa transformado al DTO).
* **GET** `/api/tareas-externas/{id}` (Obtiene una tarea externa específica por su ID).

### Inteligencia Artificial (ML.NET)
* **POST** `/api/ml/sentimiento` (Recibe un comentario y lo clasifica).

---

## Ejemplo de uso de la API externa
El sistema consume información desde el endpoint público `https://jsonplaceholder.typicode.com/todos`. Para no exponer la estructura original en inglés, la respuesta se mapea a un Data Transfer Object (DTO) personalizado (`TareaExternaDto`) aislando los datos innecesarios.

**Ejemplo de respuesta exitosa del endpoint `GET /api/tareas-externas/1`:**
```json
{
  "externalId": 1,
  "titulo": "delectus aut autem",
  "completado": false
}