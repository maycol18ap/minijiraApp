using MiniJiraApp.Models.Entities;

namespace MiniJiraApp.Repositories
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad Tarea.
    /// Incluye la consulta de tareas por proyecto (relación 1 a Muchos).
    /// </summary>
    public interface ITareaRepository
    {
        Task<IEnumerable<Tarea>> GetAllAsync();
        Task<Tarea?> GetByIdAsync(int id);
        Task<IEnumerable<Tarea>> GetTareasByProyectoIdAsync(int proyectoId);
        Task AddAsync(Tarea tarea);
        Task UpdateAsync(Tarea tarea);
        Task DeleteAsync(int id);
    }
}
