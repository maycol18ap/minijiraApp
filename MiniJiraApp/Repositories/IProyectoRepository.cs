using MiniJiraApp.Models.Entities;

namespace MiniJiraApp.Repositories
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad Proyecto.
    /// Aísla a los controladores del acceso directo a la base de datos.
    /// </summary>
    public interface IProyectoRepository
    {
        Task<IEnumerable<Proyecto>> GetAllAsync();
        Task<Proyecto?> GetByIdAsync(int id);
        Task AddAsync(Proyecto proyecto);
        Task UpdateAsync(Proyecto proyecto);
        Task DeleteAsync(int id);
    }
}
