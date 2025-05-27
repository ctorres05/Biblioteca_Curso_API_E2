using AutoMapper;
using BibliotecaApi.DTOs;
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
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context, IMapper  mapper)

        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {

            var libro = await context.Libros.ToListAsync();
            var libroDTO = mapper.Map<IEnumerable<LibroDTO>>(libro) ;
            return libroDTO;
        }

        [HttpGet ("DameUno/{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        
        {
            var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            var libroDTO = libro; 

            if (libro is null)
            {
                return NotFound();
            }
            return Ok(libroDTO);
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
            //return Ok();
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libro); /*Retorno el autor en el jeison para que me tome la modf*/
        }   /*El CreatedAtRoute me permite retornar el libro creado y la ruta para obtenerlo, en este caso la ruta es ObtenerLibro*/
        

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
