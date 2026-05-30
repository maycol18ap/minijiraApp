using System.ComponentModel.DataAnnotations;

namespace MiniJiraApp.Models.Entities
{
    public class Proyecto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proyecto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación 1 a Muchos: Un proyecto tiene muchas tareas
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}