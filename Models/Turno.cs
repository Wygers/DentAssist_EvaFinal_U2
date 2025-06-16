using System;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    public class Turno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 240, ErrorMessage = "La duración debe ser entre 1 y 240 minutos")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El paciente es obligatorio")]
        public int PacienteId { get; set; }
        public virtual Paciente? Paciente { get; set; }

        [Required(ErrorMessage = "El odontólogo es obligatorio")]
        public int OdontologoId { get; set; }
        public virtual Odontologo? Odontologo { get; set; }
    }
}
