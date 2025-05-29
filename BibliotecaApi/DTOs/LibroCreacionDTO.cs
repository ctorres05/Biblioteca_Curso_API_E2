using BibliotecaApi.Entidades;
using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibroCreacionDTO
    {
       
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string titulo { get; set; }
        public int autorId { get; set; }
      

       
    }
}
