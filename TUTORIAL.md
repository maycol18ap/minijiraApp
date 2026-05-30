# 📘 Tutorial de Uso — MiniJira

Guía paso a paso para **levantar la aplicación y usarla** desde cero: registrarse, iniciar sesión, crear proyectos, agregar tareas y consultar el endpoint JSON.

---

## Índice

1. [Levantar la aplicación](#1-levantar-la-aplicación)
2. [Abrir en el navegador](#2-abrir-en-el-navegador)
3. [Registrarse (crear una cuenta)](#3-registrarse-crear-una-cuenta)
4. [Iniciar sesión](#4-iniciar-sesión)
5. [Crear un proyecto](#5-crear-un-proyecto)
6. [Ver el detalle y agregar tareas](#6-ver-el-detalle-y-agregar-tareas)
7. [Editar y eliminar](#7-editar-y-eliminar)
8. [Cerrar sesión](#8-cerrar-sesión)
9. [Probar el endpoint JSON (API)](#9-probar-el-endpoint-json-api)
10. [Preguntas frecuentes / problemas](#10-preguntas-frecuentes--problemas)

---

## 1. Levantar la aplicación

Abre una terminal en la carpeta del proyecto y ejecuta:

```bash
cd minijiraApp/MiniJiraApp

# (Solo la primera vez) asegúrate de que la base de datos esté creada:
dotnet ef database update

# Inicia la aplicación:
dotnet run --launch-profile http
```

Cuando esté lista verás algo como esto en la terminal:

```
🔌 SQL Server conectado a través de: 172.18.48.1,1433
Now listening on: http://localhost:5140
Application started. Press Ctrl+C to shut down.
```

> 💡 Para **detener** la aplicación: presiona `Ctrl + C` en la terminal.

---

## 2. Abrir en el navegador

Abre tu navegador y entra a:

```
http://localhost:5140
```

Verás la **página de inicio** con el logo de MiniJira y dos botones: **Ver Proyectos** y **Registrarse**.

En la barra superior (navbar) verás a la derecha:
- **Iniciar Sesión**
- **Registrarse**

(Cuando ya tengas sesión, estos serán reemplazados por tu correo y el botón **Cerrar Sesión**.)

---

## 3. Registrarse (crear una cuenta)

> La primera vez necesitas crear un usuario, porque crear/editar/eliminar requiere haber iniciado sesión.

1. Haz clic en **Registrarse** (navbar arriba a la derecha) o en el botón **Registrarse** de la página de inicio.
2. Se abrirá el formulario **Crear Cuenta** con 3 campos:
   - **Correo electrónico** → por ejemplo `admin@minijira.com`
   - **Contraseña** → por ejemplo `1234` (mínimo 4 caracteres)
   - **Confirmar contraseña** → repite la misma contraseña
3. Haz clic en el botón verde **Registrarme**.

✅ Si todo está bien, **quedas automáticamente con la sesión iniciada** y la app te lleva a la lista de **Proyectos**.

> ⚠️ **Errores comunes:**
> - "Las contraseñas no coinciden" → revisa que ambas contraseñas sean iguales.
> - "Ingrese un correo válido" → el correo debe tener formato `algo@algo.com`.
> - Si el correo ya está registrado, te avisará; usa otro o ve a **Iniciar Sesión**.

---

## 4. Iniciar sesión

Si ya tienes una cuenta (o cerraste sesión antes):

1. Haz clic en **Iniciar Sesión** en la navbar.
2. Ingresa:
   - **Correo electrónico** → el que usaste al registrarte (`admin@minijira.com`)
   - **Contraseña** → tu contraseña (`1234`)
   - (Opcional) marca **Recordarme** para no tener que volver a entrar pronto.
3. Haz clic en **Ingresar**.

✅ Entras a la lista de **Proyectos**. Tu correo aparece arriba a la derecha.

> ⚠️ Si ves "Correo o contraseña incorrectos", verifica que escribiste bien ambos datos.

---

## 5. Crear un proyecto

1. Estando con sesión iniciada, ve a **Proyectos** (navbar) — verás la tabla (vacía la primera vez).
2. Haz clic en el botón verde **➕ Nuevo Proyecto** (arriba a la derecha).
3. Llena el formulario:
   - **Nombre** (obligatorio) → ej. `Sitio Web Corporativo`
   - **Descripción** (opcional) → ej. `Rediseño completo del sitio`
4. Haz clic en **Guardar**.

✅ Vuelves a la lista y aparece un mensaje verde: *"Proyecto … creado correctamente."* El proyecto ya se ve en la tabla.

> 🔒 Si intentas entrar a **Nuevo Proyecto** SIN sesión, la app te redirige automáticamente al **Login**. Esto es la protección de rutas funcionando correctamente.

---

## 6. Ver el detalle y agregar tareas

1. En la tabla de proyectos, haz clic en el botón azul **👁 (ojo / Detalles)** del proyecto.
2. Verás:
   - Un panel con la **información del proyecto** (descripción, fecha, total de tareas).
   - Abajo, la **tabla de Tareas** (vacía al inicio).
3. Haz clic en **➕ Nueva Tarea**.
4. Llena el formulario de la tarea:
   - **Título** (obligatorio) → ej. `Diseñar mockups`
   - **Descripción** (opcional) → ej. `Crear los mockups en Figma`
   - **Estado** → elige en la lista: `Pendiente`, `En Progreso` o `Completado`
5. Haz clic en **Guardar**.

✅ Vuelves al detalle del proyecto y la tarea aparece en la tabla con un **badge de color** según su estado:
- 🟡 **Pendiente** (amarillo)
- 🔵 **En Progreso** (azul)
- 🟢 **Completado** (verde)

Repite para agregar más tareas al mismo proyecto.

---

## 7. Editar y eliminar

### Editar un proyecto
- En la lista de proyectos, clic en el botón azul **✏️ (lápiz)** → modifica los datos → **Actualizar**.

### Editar una tarea (cambiar su estado)
- Entra al **Detalle** del proyecto → en la tabla de tareas, clic en **✏️** de la tarea → cambia el **Estado** (por ejemplo de `Pendiente` a `Completado`) → **Actualizar**.

### Eliminar
- Botón rojo **🗑 (basura)** tanto en proyectos como en tareas.
- Se abre una **pantalla de confirmación** para evitar borrados accidentales.
- Al eliminar un proyecto, se eliminan también **todas sus tareas** (borrado en cascada).

> Cada acción muestra un mensaje de confirmación en la parte superior.

---

## 8. Cerrar sesión

- Haz clic en el botón **Cerrar Sesión** (arriba a la derecha, junto a tu correo).
- Vuelves a la página de inicio y la navbar muestra de nuevo **Iniciar Sesión** / **Registrarse**.

---

## 9. Probar el endpoint JSON (API)

La aplicación expone un endpoint que devuelve los proyectos en formato **JSON**, pensado para que lo consuma un cliente externo (otra app, Postman, etc.). **No requiere sesión.**

Con la app corriendo, abre en el navegador o en una terminal:

```
http://localhost:5140/Proyectos/GetAllJson
```

Con `curl`:

```bash
curl http://localhost:5140/Proyectos/GetAllJson
```

Respuesta de ejemplo:

```json
[
  {
    "id": 1,
    "nombre": "Sitio Web Corporativo",
    "descripcion": "Rediseño completo del sitio",
    "fechaCreacion": "2026-05-30T04:57:58.54",
    "cantidadTareas": 2
  }
]
```

> 💡 Puedes pegarlo en [Postman](https://www.postman.com/) o ver el JSON formateado en el navegador.

---

## 10. Preguntas frecuentes / problemas

**❓ La página no carga / "no se puede conectar"**
Asegúrate de que la terminal sigue mostrando `Now listening on: http://localhost:5140` y que no la cerraste con `Ctrl+C`.

**❓ Error de base de datos al iniciar / al guardar**
- Verifica que **SQL Server esté encendido**.
- Si NO usas WSL, edita `appsettings.json` y pon:
  ```json
  "Database": { "AutoResolveWslHost": false, "HostOverride": "localhost" }
  ```
- La primera escritura tras arrancar puede tardar un poco (la app reintenta automáticamente).

**❓ "No me deja crear un proyecto y me manda al login"**
Es lo esperado: primero debes **iniciar sesión**. Ve a Login o Regístrate.

**❓ Olvidé mi contraseña**
Este proyecto académico no incluye recuperación de contraseña; simplemente **registra un usuario nuevo** con otro correo.

**❓ ¿Dónde se guardan los datos?**
En la base de datos **SQL Server** (`MiniJiraDb`), en las tablas `Proyectos` y `Tareas`. Los usuarios se guardan en las tablas de Identity (`AspNetUsers`).

---

## 🎬 Recorrido rápido (resumen)

```
1. dotnet run --launch-profile http
2. Abrir http://localhost:5140
3. Registrarse  →  admin@minijira.com / 1234
4. Proyectos → Nuevo Proyecto → Guardar
5. Clic en 👁 (Detalles) → Nueva Tarea → elegir estado → Guardar
6. Editar / Eliminar con ✏️ y 🗑
7. Ver API:  http://localhost:5140/Proyectos/GetAllJson
8. Cerrar Sesión
```

¡Listo! Con esto ya sabes usar toda la aplicación. 🚀
