using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class AutorPatchDTO
    {
        [Required(ErrorMessage = "El campo {0} no puede ser nulo")]
        [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]   //VAlidacion por Atributo
        public required string nombres { get; set; }


        [Required(ErrorMessage = "El campo {0} no puede ser nulo")]
        [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]   //VAlidacion por Atributo
        public required string apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} no puede ser nulo")]
        [StringLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string? identificacion { get; set; }


        [Range(10, 120, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
        public int edad { get; set; } /*campo edad*/

        [Url]
        public string? Url { get; set; }
    }
}
