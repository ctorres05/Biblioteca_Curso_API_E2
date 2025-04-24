using BibliotecaApi.Entidades;
using BibliotecaApi.Entidades.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace BibliotecaApi.Controllers
{
    [ApiController]

    [Route("api/libros")]

    public class LibrosController : ControllerBase
    {
        private readonly AplicationDbContext context;   
        public LibrosController(AplicationDbContext context)

        {
            this.context = context;

        }
        [HttpGet]
        public async Task<IEnumerable<Libro>> Get()
        {
            return await context.Libros.ToListAsync();
        }

        [HttpGet ("DameUno/{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        
        {
            var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro is null)
            {
                return NotFound();
            }
            return Ok(libro);
        }
        


        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeautor = await context.Autores.AnyAsync(x => x.Id == libro.autorId);

            if (!existeautor)
            {
                //return BadRequest("No existe el autor");
                ModelState.AddModelError(nameof(libro.autorId), $"No existe el autor con el id {libro.autorId}"); /*Agregamos un error al modelo*/  
                return ValidationProblem(); /*Retorna un 400 y el error que agregamos al modelo*/   

            }   

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut ("{id:int}")]
        public async Task<ActionResult> Put(Libro libro, int id)
        {
            if (libro.Id != id)
            {
                return BadRequest("El id del libro no coincide con el id de la URL");
            }
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
          
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            
            if (libro is null)
            {
                return NotFound();
            }
            
            context.Remove(libro);
            await context.SaveChangesAsync();
            return Ok(libro);
        }   
    }
}
