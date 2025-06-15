using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models
{
    public class Tratamiento
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PrecioEstimado { get; set; }
    }
}
