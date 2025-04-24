using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Entidades
{
    public class Autor 
    {
        public int Id { get; set; }

        [Required       (ErrorMessage = "El campo {0} no puede ser nulo")]
        [StringLength   (100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]   //VAlidacion por Atributo
        public required string nombre { get; set; }

        [Range          (10, 120, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]   
        public int edad { get; set; } /*campo edad*/

        [Url]
        public string? Url { get; set; }

       public List<Libro> Libros { get; set; } = new List<Libro>();



  



       


    }
}
