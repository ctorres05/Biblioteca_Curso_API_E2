using BibliotecaApi.Entidades;

namespace BibliotecaApi.DTOs
{
    public class LibroConAutorDTO : LibroDTO
    {
        public int autorId { get; set; }
        public required string  autornombre { get; set; }
    }
}
