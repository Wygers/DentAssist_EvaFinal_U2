using System;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    public class PasoTratamiento
    {
        public int Id { get; set; }
        [Required]
        public int PlanTratamientoId { get; set; }
        public PlanTratamiento PlanTratamiento { get; set; }
        [Required]
        public int TratamientoId { get; set; }
        public Tratamiento Tratamiento { get; set; }
        [Required]
        public DateTime FechaEstimada { get; set; }
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } // Ej: "Pendiente", "EnProgreso", "Completado"
        [StringLength(500)]
        public string Observaciones { get; set; }
    }
}
