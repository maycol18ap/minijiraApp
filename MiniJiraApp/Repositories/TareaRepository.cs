using Microsoft.EntityFrameworkCore;
using MiniJiraApp.Data;
using MiniJiraApp.Models.Entities;

namespace MiniJiraApp.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de Tareas.
    /// Todos los métodos de acceso a datos son asíncronos (async/await/Task).
    /// </summary>
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarea>> GetAllAsync()
        {
            return await _context.Tareas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tarea?> GetByIdAsync(int id)
        {
            return await _context.Tareas
                .Include(tarea => tarea.Proyecto)
                .FirstOrDefaultAsync(tarea => tarea.Id == id);
        }

        public async Task<IEnumerable<Tarea>> GetTareasByProyectoIdAsync(int proyectoId)
        {
            return await _context.Tareas
                .Where(tarea => tarea.ProyectoId == proyectoId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Tarea tarea)
        {
            await _context.Tareas.AddAsync(tarea);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarea tarea)
        {
            _context.Tareas.Update(tarea);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea is not null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }
        }
    }
}
