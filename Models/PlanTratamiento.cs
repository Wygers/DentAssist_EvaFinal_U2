using DentAssist.Models;
using System.ComponentModel.DataAnnotations;

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
    public ICollection<PasoTratamiento> Pasos { get; set; } = new List<PasoTratamiento>();
}
