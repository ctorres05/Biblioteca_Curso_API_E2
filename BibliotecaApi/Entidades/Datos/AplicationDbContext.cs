using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Entidades.Datos
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Autor>().Property(a => a.nombre).HasMaxLength(150);
            //modelBuilder.Entity<Libro>().Property(l => l.titulo).HasMaxLength(150);
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Autor> Autores{ get; set; }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }




    }
}
