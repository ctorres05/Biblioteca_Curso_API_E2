using BibliotecaApi.Entidades;

namespace BibliotecaApi.DTOs
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }                        /*= Guid.NewGuid();*/
        public required string? Cuerpo { get; set; }       // Comentario del usuario
        public DateTime FechaCreacion { get; set; }         /*= DateTime.UtcNow;*/ // Fecha de creación del comentario
       

    }
}
