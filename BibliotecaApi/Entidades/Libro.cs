using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string titulo { get; set; }
        public int autorId { get; set; }
        public Autor? Autor { get; set; }    

    }
}
