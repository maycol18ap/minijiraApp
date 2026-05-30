using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniJiraApp.Models.Entities
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
        public string? Descripcion { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Progreso, Completado

        // Clave Foránea
        [Required]
        public int ProyectoId { get; set; }

        [ForeignKey("ProyectoId")]
        public Proyecto? Proyecto { get; set; }
    }
}