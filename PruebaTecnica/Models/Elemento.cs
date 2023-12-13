using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models
{
    public class Elemento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Elemento { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public int Peso { get; set; }
        
        [Required]
        public int Calorias { get; set; }
    }
}
