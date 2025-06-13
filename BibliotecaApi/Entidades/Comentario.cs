using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Entidades
{
    public class Comentario
    {
        public Guid Id { get; set; }                        /*= Guid.NewGuid();*/

        [Required]
        public required  string? Cuerpo { get; set; }       // Comentario del usuario
        public DateTime FechaCreacion { get; set; }         /*= DateTime.UtcNow;*/ // Fecha de creación del comentario
        public int LibroId { get; set; }                    // ID del libro al que pertenece el comentario
        public Libro? Libro { get; set; }                   // Navegación al libro asociado

    }
}
