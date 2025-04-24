using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Entidades.Datos
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Autor> Autores{ get; set; }

        public DbSet<Libro> Libros { get; set; }


    }
}
