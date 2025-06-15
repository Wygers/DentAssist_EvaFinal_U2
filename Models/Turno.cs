using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentAssist.Models
{
    public class Turno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora son requeridas.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "La duración en minutos es requerida.")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos.")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El paciente es requerido.")]
        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; } // Propiedad de navegación

        [Required(ErrorMessage = "El odontólogo es requerido.")]
        [ForeignKey("Odontologo")]
        public int OdontologoId { get; set; }
        public Odontologo Odontologo { get; set; } // Propiedad de navegación
    }
}