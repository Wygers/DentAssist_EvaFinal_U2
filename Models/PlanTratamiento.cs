using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    public class PlanTratamiento
    {
        public int Id { get; set; }
        [Required]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        [Required]
        public int OdontologoId { get; set; }
        public Odontologo Odontologo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public List<PasoTratamiento> Pasos { get; set; } = new();
    }
}
