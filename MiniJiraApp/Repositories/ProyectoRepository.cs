using Microsoft.EntityFrameworkCore;
using MiniJiraApp.Data;
using MiniJiraApp.Models.Entities;

namespace MiniJiraApp.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de Proyectos.
    /// Todos los métodos de acceso a datos son asíncronos (async/await/Task).
    /// </summary>
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDbContext _context;

        public ProyectoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proyecto>> GetAllAsync()
        {
            return await _context.Proyectos
                .Include(proyecto => proyecto.Tareas)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Proyecto?> GetByIdAsync(int id)
        {
            return await _context.Proyectos
                .Include(proyecto => proyecto.Tareas)
                .FirstOrDefaultAsync(proyecto => proyecto.Id == id);
        }

        public async Task AddAsync(Proyecto proyecto)
        {
            await _context.Proyectos.AddAsync(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Update(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto is not null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
