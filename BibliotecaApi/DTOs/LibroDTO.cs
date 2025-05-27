using BibliotecaApi.Entidades;
using BibliotecaApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public required string titulo { get; set; }
        
    }
}
