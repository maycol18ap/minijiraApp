using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniJiraApp.Models.Entities;
using MiniJiraApp.Repositories;

namespace MiniJiraApp.Controllers
{
    /// <summary>
    /// Controlador de Tareas. Necesita el repositorio de Proyectos para
    /// validar que el proyecto asociado exista antes de crear una tarea.
    /// </summary>
    public class TareasController : Controller
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly IProyectoRepository _proyectoRepository;

        public TareasController(ITareaRepository tareaRepository, IProyectoRepository proyectoRepository)
        {
            _tareaRepository = tareaRepository;
            _proyectoRepository = proyectoRepository;
        }

        // GET: /Tareas/Create?proyectoId=5
        [Authorize]
        public async Task<IActionResult> Create(int proyectoId)
        {
            var proyecto = await _proyectoRepository.GetByIdAsync(proyectoId);
            if (proyecto is null)
            {
                return NotFound();
            }

            ViewBag.NombreProyecto = proyecto.Nombre;
            var tarea = new Tarea { ProyectoId = proyectoId };
            return View(tarea);
        }

        // POST: /Tareas/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarea tarea)
        {
            var proyecto = await _proyectoRepository.GetByIdAsync(tarea.ProyectoId);
            if (proyecto is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.NombreProyecto = proyecto.Nombre;
                return View(tarea);
            }

            await _tareaRepository.AddAsync(tarea);
            return RedirectToAction("Details", "Proyectos", new { id = tarea.ProyectoId });
        }

        // GET: /Tareas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea is null)
            {
                return NotFound();
            }
            return View(tarea);
        }

        // POST: /Tareas/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(tarea);
            }

            await _tareaRepository.UpdateAsync(tarea);
            return RedirectToAction("Details", "Proyectos", new { id = tarea.ProyectoId });
        }

        // GET: /Tareas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea is null)
            {
                return NotFound();
            }
            return View(tarea);
        }

        // POST: /Tareas/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _tareaRepository.GetByIdAsync(id);
            if (tarea is null)
            {
                return NotFound();
            }

            var proyectoId = tarea.ProyectoId;
            await _tareaRepository.DeleteAsync(id);
            return RedirectToAction("Details", "Proyectos", new { id = proyectoId });
        }
    }
}
