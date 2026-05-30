using System.ComponentModel.DataAnnotations;

namespace ApiTareasInteligente.Models
{
    public class Tarea : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public PrioridadTarea Prioridad { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime FechaVencimiento { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaVencimiento.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    "La fecha de vencimiento no puede ser menor a la fecha actual.",
                    new[] { nameof(FechaVencimiento) }
                );
            }
        }
    }
}