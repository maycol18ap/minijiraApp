# MiniJira — Sistema de Gestión de Proyectos

Aplicación web desarrollada con **ASP.NET Core MVC** y **Entity Framework Core (Code-First)** sobre **SQL Server**. Permite a usuarios autenticados gestionar proyectos y las tareas asociadas a cada uno, siguiendo el patrón **Repositorio**, programación **asíncrona** y protección de rutas con **.NET Identity**.

---

## 🛠️ Tecnologías

| Componente        | Versión / Detalle                          |
| ----------------- | ------------------------------------------ |
| Framework         | ASP.NET Core MVC (.NET 10)                 |
| ORM               | Entity Framework Core 10 (Code-First)      |
| Base de datos     | SQL Server                                 |
| Autenticación     | ASP.NET Core Identity                      |
| UI                | Razor Views + Bootstrap 5 + Bootstrap Icons|

---

## 📋 Requisitos previos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- **SQL Server** accesible (local, Docker o instancia en Windows desde WSL)
- Herramienta `dotnet-ef`:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 🔌 Cadena de conexión

La cadena se define en `MiniJiraApp/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MiniJiraDb;User Id=none;Password=awl;TrustServerCertificate=True;"
}
```

### Nota sobre WSL (Ubuntu)

El proyecto fue desarrollado en **WSL Ubuntu** conectándose a un **SQL Server alojado en Windows**. Como la IP del host de Windows cambia, `Program.cs` **resuelve automáticamente la IP del host** (la puerta de enlace por defecto de WSL) en tiempo de ejecución:

```
🔌 SQL Server conectado a través de: 172.18.48.1,1433
```

Opciones de configuración en `appsettings.json` → sección `"Database"`:

| Clave                | Descripción                                                        |
| -------------------- | ------------------------------------------------------------------ |
| `AutoResolveWslHost` | `true` (por defecto): resuelve la IP del host de Windows en WSL.   |
| `HostOverride`       | Fuerza un host concreto (ej. `"localhost"`). Tiene prioridad.      |

> **Si NO usas WSL** (SQL Server local en la misma máquina): pon `"AutoResolveWslHost": false` y `"HostOverride": "localhost"`.

---

## 🚀 Cómo levantar el proyecto

```bash
# 1. Clonar y entrar al proyecto
git clone <URL-del-repositorio>
cd minijiraApp/MiniJiraApp

# 2. Restaurar dependencias
dotnet restore

# 3. Crear / actualizar la base de datos (aplica las migraciones)
dotnet ef database update

# 4. Ejecutar
dotnet run --launch-profile http
```

La aplicación quedará disponible en: **http://localhost:5140**

---

## 📂 Estructura del proyecto

```
MiniJiraApp/
├── Controllers/
│   ├── AccountController.cs      # Login / Register / Logout (Identity)
│   ├── HomeController.cs         # Landing
│   ├── ProyectosController.cs    # CRUD de proyectos + endpoint JSON
│   └── TareasController.cs       # CRUD de tareas
├── Data/
│   ├── AppDbContext.cs           # IdentityDbContext + DbSets
│   └── Migrations/               # Migración Code-First
├── Models/
│   ├── Entities/
│   │   ├── Proyecto.cs           # Entidad (1 ── ∞ Tareas)
│   │   └── Tarea.cs              # Entidad
│   └── ViewModels/
│       ├── LoginViewModel.cs
│       └── RegisterViewModel.cs
├── Repositories/                 # Patrón Repositorio (abstracción de datos)
│   ├── IProyectoRepository.cs
│   ├── ProyectoRepository.cs
│   ├── ITareaRepository.cs
│   └── TareaRepository.cs
├── Views/
│   ├── Account/  (Login, Register)
│   ├── Proyectos/ (Index, Create, Edit, Details, Delete)
│   ├── Tareas/    (Create, Edit, Delete)
│   └── Shared/    (_Layout con navbar dinámica)
├── Program.cs                    # Configuración + Inyección de Dependencias
└── appsettings.json
```

---

## ✨ Funcionalidades

- **Autenticación** completa (registro, inicio y cierre de sesión).
- **CRUD de Proyectos**: listar, crear, ver detalle, editar y eliminar.
- **CRUD de Tareas** asociadas a un proyecto, con estados (`Pendiente`, `En Progreso`, `Completado`) representados con *badges* de colores.
- **Rutas protegidas**: solo usuarios autenticados pueden crear/editar/eliminar. La consulta (`Index`, `Details`) es pública.
- **Endpoint API JSON**: `GET /Proyectos/GetAllJson` devuelve la lista de proyectos en formato JSON para clientes externos.

### Ejemplo del endpoint JSON

```bash
curl http://localhost:5140/Proyectos/GetAllJson
```

```json
[
  {
    "id": 1,
    "nombre": "Proyecto con Tareas",
    "descripcion": "Probando tareas",
    "fechaCreacion": "2026-05-30T04:57:58.54",
    "cantidadTareas": 1
  }
]
```

---

## 📖 Documentación adicional

- **[`TUTORIAL.md`](./TUTORIAL.md)** — Guía paso a paso de **cómo usar la aplicación** (registrarse, iniciar sesión, crear proyectos y tareas, usar la API).
- **[`CRITERIOS.md`](./CRITERIOS.md)** — Explicación detallada de **cómo se implementó cada criterio de evaluación** (útil para estudiar y defender el proyecto).
