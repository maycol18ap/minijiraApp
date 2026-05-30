# EXAMEN DE UNIDAD DE APRENDIZAJE Nº3

## DES320 TALLER DE DISEÑO DE APLICACIONES

**Créditos:** 10
**Horas Semestrales:** 100
**Requisitos:** DES210
**Fecha Actualización:** 20-05-2026

### ESCUELA DE INFORMATICA Y TELECOMUNICACIONES

### CARRERA:

* LICENCIATURA EN INGENIERIA DE SISTEMAS.

---

# PUNTAJES Y NOTA

* **Puntaje máximo:** 100
* **Puntaje mínimo de aprobación:** 51

---

# INSTRUCCIONES GENERALES

* El desarrollo del proyecto se realizará individual.
* El equipo tendrá un plazo exacto de 1 semana para el desarrollo, pruebas y documentación de la solución.
* Se evaluará rigurosamente la calidad del código (Clean Code), el correcto nombramiento de variables/métodos y la organización del proyecto.
* **NOTA CRÍTICA:** La aplicación debe compilar sin errores, conectarse a la base de datos SQL Server y ser completamente funcional. Los proyectos que no compilen serán calificados con nota mínima.

---

# Objetivo General

Desarrollar una aplicación web robusta y estructurada utilizando el patrón MVC en ASP.NET Core, implementando Entity Framework Core (Code-First) para la persistencia en SQL Server. El proyecto debe demostrar la correcta abstracción de datos mediante el Patrón Repositorio, uso de programación asíncrona y protección de rutas.

---

# Caso de Estudio

## Sistema de Gestión de Proyectos (Mini-Jira)

Construirán el núcleo de un sistema donde los administradores puedan gestionar "Proyectos" y las "Tareas" que pertenecen a cada uno.

---

# ENTREGABLES DEL PROYECTO

# Parte 1: Motor de Datos y ORM (20 puntos)

## 1. Modelado Code-First

Creación de las entidades Proyecto y Tarea con una relación de 1 a Muchos. Mapeo correcto de tipos de datos y restricciones (Data Annotations o Fluent API).

## 2. Configuración de EF Core

Implementación de AppDbContext, configuración de los DbSet correspondientes y establecimiento seguro de la cadena de conexión en `appsettings.json`.

## 3. Migraciones

Generación de la migración inicial y actualización de la base de datos SQL Server.

---

# Parte 2: Arquitectura y Patrón Repositorio (25 puntos)

## 1. Abstracción de Datos

Creación de las interfaces (ej. `IProyectoRepository`) y sus respectivas implementaciones concretas para aislar los controladores del acceso directo a la base de datos.

## 2. Asincronismo

Todos los métodos de acceso a datos (CRUD) deben implementarse de manera estrictamente asíncrona (`async`, `await`, `Task`).

## 3. Inyección de Dependencias

Registro adecuado de los repositorios y el contexto de base de datos en `Program.cs`.

---

# Parte 3: UI, Controladores y Flujo de Datos (25 puntos)

## 1. Controladores Limpios

Implementación de controladores (ej. `ProyectosController`) que inyecten y utilicen los repositorios. Cero lógica de acceso a datos directa en el controlador.

## 2. Vistas Razor

Construcción de las vistas para listar proyectos (`Index`), crear (`Create`) y visualizar detalles. Se valorará el diseño limpio utilizando frameworks CSS (como Bootstrap).

## 3. Tag Helpers

Uso adecuado de Tag Helpers para la construcción de formularios y navegación segura entre vistas.

---

# Parte 4: Seguridad y Endpoints API (20 puntos)

## 1. Protección de Rutas

Aplicación del atributo `[Authorize]` para asegurar que solo usuarios autenticados puedan acceder a la creación o edición de proyectos (se requiere implementar el sistema de Login/Identity básico de .NET).

## 2. Disponibilidad de Datos

Creación de al menos un método en el controlador que retorne un objeto o lista en formato JSON (API endpoint) para ser consumido por un cliente externo.

---

# Parte 5: Documentación y Calidad (10 puntos)

## 1. Archivo README

El repositorio debe incluir un `README.md` que explique cómo levantar el proyecto, los requisitos previos, la cadena de conexión utilizada y capturas de pantalla de la aplicación funcionando.

## 2. Clean Code

Código indentado, sin comentarios basura, con variables descriptivas y en inglés/español consistente.

---

# Método de Entrega

* El proyecto deberá alojarse en un repositorio de GitHub/Azure DevOps. Se debe entregar el enlace público o dar acceso al docente.
* El último commit debe realizarse antes de la fecha y hora límite.

---

# Criterio a Evaluar

| Criterio                          | Excelente   | Regular  | Deficiente |
| --------------------------------- | ----------- | -------- | ---------- |
| Modelado, EF Core y BD            | 20 pts      | 10 pts   | 0 pts      |
| Patrón Repositorio y Asincronismo | 25 pts      | 12.5 pts | 0 pts      |
| Controladores, Vistas y UI        | 25 pts      | 12.5 pts | 0 pts      |
| Seguridad y Endpoints JSON        | 20 pts      | 10 pts   | 0 pts      |
| Documentación y Calidad de Código | 10 pts      | 5 pts    | 0 pts      |
| **Total**                         | **100 pts** |          |            |
