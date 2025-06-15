using DentAssist.Models;
using System.ComponentModel.DataAnnotations;

public class PasoTratamiento
{
    public int Id { get; set; }
    public int PlanTratamientoId { get; set; }
    public PlanTratamiento PlanTratamiento { get; set; }
    public int TratamientoId { get; set; }
    public Tratamiento Tratamiento { get; set; }
    public DateTime FechaEstimada { get; set; }

    public string Estado { get; set; }  // Pendiente, En Progreso, Completado

    public string Observaciones { get; set; }
}
