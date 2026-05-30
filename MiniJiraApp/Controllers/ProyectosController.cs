using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniJiraApp.Models.Entities;
using MiniJiraApp.Repositories;

namespace MiniJiraApp.Controllers
{
    /// <summary>
    /// Controlador de Proyectos. No accede directamente a la base de datos:
    /// toda la persistencia se delega a los repositorios inyectados.
    /// </summary>
    public class ProyectosController : Controller
    {
        private readonly IProyectoRepository _proyectoRepository;
        private readonly ITareaRepository _tareaRepository;

        public ProyectosController(IProyectoRepository proyectoRepository, ITareaRepository tareaRepository)
        {
            _proyectoRepository = proyectoRepository;
            _tareaRepository = tareaRepository;
        }

        // GET: /Proyectos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            return View(proyectos);
        }

        // GET: /Proyectos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var proyecto = await _proyectoRepository.GetByIdAsync(id);
            if (proyecto is null)
            {
                return NotFound();
            }

            proyecto.Tareas = (await _tareaRepository.GetTareasByProyectoIdAsync(id)).ToList();
            return View(proyecto);
        }

        // GET: /Proyectos/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Proyectos/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return View(proyecto);
            }

            await _proyectoRepository.AddAsync(proyecto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Proyectos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var proyecto = await _proyectoRepository.GetByIdAsync(id);
            if (proyecto is null)
            {
                return NotFound();
            }
            return View(proyecto);
        }

        // POST: /Proyectos/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proyecto proyecto)
        {
            if (id != proyecto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(proyecto);
            }

            await _proyectoRepository.UpdateAsync(proyecto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Proyectos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var proyecto = await _proyectoRepository.GetByIdAsync(id);
            if (proyecto is null)
            {
                return NotFound();
            }
            return View(proyecto);
        }

        // POST: /Proyectos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _proyectoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Proyectos/GetAllJson  (Endpoint API para clientes externos)
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllJson()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            var resultado = proyectos.Select(proyecto => new
            {
                proyecto.Id,
                proyecto.Nombre,
                proyecto.Descripcion,
                proyecto.FechaCreacion,
                CantidadTareas = proyecto.Tareas.Count
            });
            return Json(resultado);
        }
    }
}
