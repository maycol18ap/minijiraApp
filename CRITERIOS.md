# Guía de Estudio — Cómo se implementó cada criterio

Este documento explica, parte por parte, **dónde y cómo** se cumple cada requisito del examen. Úsalo para estudiar y defender el proyecto.

| Criterio                          | Puntaje |
| --------------------------------- | ------- |
| Modelado, EF Core y BD            | 20 pts  |
| Patrón Repositorio y Asincronismo | 25 pts  |
| Controladores, Vistas y UI        | 25 pts  |
| Seguridad y Endpoints JSON        | 20 pts  |
| Documentación y Calidad de Código | 10 pts  |

---

## Parte 1 — Motor de Datos y ORM (20 pts)

### 1.1 Modelado Code-First con relación 1 a Muchos

📁 `Models/Entities/Proyecto.cs` y `Models/Entities/Tarea.cs`

- **Proyecto** tiene una colección de tareas → lado "uno":
  ```csharp
  public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
  ```
- **Tarea** tiene la clave foránea y la propiedad de navegación → lado "muchos":
  ```csharp
  [Required]
  public int ProyectoId { get; set; }

  [ForeignKey("ProyectoId")]
  public Proyecto? Proyecto { get; set; }
  ```
- **Data Annotations** para tipos y restricciones: `[Key]`, `[Required]`, `[StringLength]`, `[ForeignKey]`.

> **Relación 1 a ∞:** Un `Proyecto` puede tener muchas `Tarea`s; cada `Tarea` pertenece a un solo `Proyecto`.

### 1.2 Configuración de EF Core

📁 `Data/AppDbContext.cs`

```csharp
public class AppDbContext : IdentityDbContext
{
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
}
```

- Hereda de `IdentityDbContext` (combina nuestras tablas + las de Identity).
- Los `DbSet` exponen las entidades como tablas.

La **cadena de conexión** se establece de forma segura en `appsettings.json` (no está escrita "a fuego" en el código):
```json
"ConnectionStrings": { "DefaultConnection": "Server=...;Database=MiniJiraDb;..." }
```

### 1.3 Migraciones

📁 `Data/Migrations/`

- Migración inicial generada con `dotnet ef migrations add InitialCreate`.
- Base de datos creada/actualizada con `dotnet ef database update`.
- La migración crea las tablas `Proyectos`, `Tareas` (con su FK e índice `IX_Tareas_ProyectoId`) y todas las tablas de Identity (`AspNetUsers`, etc.).

---

## Parte 2 — Arquitectura y Patrón Repositorio (25 pts)

### 2.1 Abstracción de datos (interfaces + implementaciones)

📁 `Repositories/`

- **Interfaces** (el contrato): `IProyectoRepository`, `ITareaRepository`.
- **Implementaciones concretas**: `ProyectoRepository`, `TareaRepository`.

Esto **aísla a los controladores** del acceso directo a la base de datos: el controlador depende de la *interfaz*, no de EF Core.

`ITareaRepository` incluye el método especial pedido:
```csharp
Task<IEnumerable<Tarea>> GetTareasByProyectoIdAsync(int proyectoId);
```

### 2.2 Asincronismo

Todos los métodos de acceso a datos son **estrictamente asíncronos** (`async` / `await` / `Task`). Ejemplo en `ProyectoRepository.cs`:

```csharp
public async Task<IEnumerable<Proyecto>> GetAllAsync()
{
    return await _context.Proyectos
        .Include(proyecto => proyecto.Tareas)
        .AsNoTracking()
        .ToListAsync();
}

public async Task AddAsync(Proyecto proyecto)
{
    await _context.Proyectos.AddAsync(proyecto);
    await _context.SaveChangesAsync();
}
```

> El repositorio **solo persiste**: no contiene validaciones de negocio (esas viven en el controlador con `ModelState`).

### 2.3 Inyección de Dependencias

📁 `Program.cs`

```csharp
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
```

- `AddScoped` = una instancia por petición HTTP (ideal para `DbContext`).
- El `DbContext` también se registra con `AddDbContext` y la cadena de conexión.

---

## Parte 3 — UI, Controladores y Flujo de Datos (25 pts)

### 3.1 Controladores limpios

📁 `Controllers/ProyectosController.cs`, `Controllers/TareasController.cs`

- Reciben los repositorios **por constructor** (inyección):
  ```csharp
  public ProyectosController(IProyectoRepository proyectoRepository, ITareaRepository tareaRepository)
  ```
- **Cero acceso directo a la base de datos**: nunca usan `DbContext`, solo los repositorios.
- Acciones asíncronas (`Task<IActionResult>`).
- En `Create [HttpPost]` solo se valida `ModelState`, se llama al repo y se redirige:
  ```csharp
  if (!ModelState.IsValid) return View(proyecto);
  await _proyectoRepository.AddAsync(proyecto);
  return RedirectToAction(nameof(Index));
  ```
- En `Details` se obtiene el proyecto **y** sus tareas (usando ambos repositorios).

### 3.2 Vistas Razor + Bootstrap

📁 `Views/Proyectos/` y `Views/Tareas/`

- `Index`: tabla Bootstrap con los proyectos.
- `Create` / `Edit`: formularios.
- `Details`: panel del proyecto + tabla de tareas con **badges de color por estado**.
- Diseño limpio con **Bootstrap 5** y **Bootstrap Icons**.

### 3.3 Tag Helpers

Las vistas usan Tag Helpers para formularios y navegación segura:

- `asp-for` (enlaza campos al modelo): `<input asp-for="Nombre" />`
- `asp-action` / `asp-controller` / `asp-route-id` (genera URLs):
  `<a asp-action="Details" asp-route-id="@proyecto.Id">`
- `asp-validation-for` / `asp-validation-summary` (mensajes de validación).
- `asp-items` (rellena el `<select>` de estados).

---

## Parte 4 — Seguridad y Endpoints API (20 pts)

### 4.1 Protección de rutas (`[Authorize]`)

📁 Controladores + `Program.cs`

- Sistema de **Login/Identity** implementado en `AccountController` con `SignInManager` y `UserManager`.
- Las acciones que **modifican datos** están protegidas con `[Authorize]` (Create, Edit, Delete — GET y POST).
- Las acciones de solo lectura (`Index`, `Details`, `GetAllJson`) son públicas con `[AllowAnonymous]` para facilitar pruebas.
- Pipeline correcto en `Program.cs`:
  ```csharp
  app.UseAuthentication();  // primero: ¿quién eres?
  app.UseAuthorization();   // después: ¿puedes pasar?
  ```
- Redirección a login configurada:
  ```csharp
  options.LoginPath = "/Account/Login";
  options.AccessDeniedPath = "/Account/Login";
  ```

> **Prueba rápida:** sin sesión, entrar a `/Proyectos/Create` redirige a
> `/Account/Login?ReturnUrl=%2FProyectos%2FCreate`.

### 4.2 Endpoint JSON (API)

📁 `ProyectosController.GetAllJson()`

```csharp
[HttpGet]
public async Task<IActionResult> GetAllJson()
{
    var proyectos = await _proyectoRepository.GetAllAsync();
    var resultado = proyectos.Select(p => new { p.Id, p.Nombre, p.Descripcion, p.FechaCreacion, CantidadTareas = p.Tareas.Count });
    return Json(resultado);
}
```

Devuelve la lista de proyectos en **formato JSON**, consumible por cualquier cliente externo (`GET /Proyectos/GetAllJson`). Se proyecta a un objeto anónimo para evitar ciclos de serialización y exponer solo lo necesario.

---

## Parte 5 — Documentación y Calidad (10 pts)

### 5.1 README

📁 `README.md`: requisitos previos, cómo levantar el proyecto, cadena de conexión (incluida la lógica especial de WSL) y espacio para capturas.

### 5.2 Clean Code

- **Nombres descriptivos** y en español consistente (`GetTareasByProyectoIdAsync`, `proyectoRepository`).
- **Separación de responsabilidades**: Entidades / ViewModels / Repositorios / Controladores / Vistas.
- **Sin lógica de negocio en controladores** más allá de validar y delegar.
- **Sin acceso a datos en las vistas** (usan modelos y ViewModels).
- Código indentado, sin comentarios basura.

---

## ⭐ Detalle técnico extra (para defender el proyecto)

**Resiliencia de conexión** en `Program.cs` — se habilitaron reintentos automáticos para tolerar la latencia de SQL Server en WSL:

```csharp
options.UseSqlServer(finalConnectionString, sqlOptions =>
{
    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    sqlOptions.CommandTimeout(60);
});
```

Esto evita errores de *timeout* transitorios (Error 258) en la primera consulta tras el arranque.

---

## 🎤 Posibles preguntas del profesor

1. **¿Por qué usar el patrón Repositorio?**
   Para desacoplar la lógica de los controladores del ORM. Si mañana cambio EF Core por Dapper, solo reescribo las implementaciones; los controladores no cambian porque dependen de la *interfaz*.

2. **¿Por qué `async/await`?**
   Libera el hilo mientras espera la base de datos, permitiendo atender más peticiones concurrentes. Es escalabilidad, no velocidad de una sola petición.

3. **¿Qué hace `AddScoped`?**
   Crea una instancia del repositorio/DbContext por cada petición HTTP y la reutiliza dentro de esa petición. Evita problemas de concurrencia del `DbContext`.

4. **¿Cómo se relacionan Proyecto y Tarea en la BD?**
   Con una clave foránea `ProyectoId` en la tabla `Tareas` y un índice `IX_Tareas_ProyectoId`. El borrado es en cascada (al borrar un proyecto se borran sus tareas).

5. **¿Diferencia entre Entidad y ViewModel?**
   La *entidad* mapea a la tabla de la BD. El *ViewModel* (ej. `RegisterViewModel`) modela datos de un formulario concreto, con sus propias validaciones (`[Compare]` para confirmar contraseña), sin contaminar el modelo de datos.

6. **¿Qué es `AsNoTracking()`?**
   En consultas de solo lectura le dice a EF que no rastree cambios de las entidades, mejorando el rendimiento.
