using AutoMapper;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using BibliotecaApi.Entidades.Datos;
using Microsoft.AspNetCore.JsonPatch;
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

        public LibrosController(AplicationDbContext context, IMapper mapper)

        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {

            var libro = await context.Libros
                .Include(x => x.Autor)
                .ToListAsync();
            var libroDTO = mapper.Map<IEnumerable<LibroDTO>>(libro);
            return libroDTO;
        }

        [HttpGet("DameUno/{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroConAutorDTO>> Get(int id)

        {
            var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            var libroConAutorDTO = mapper.Map<LibroConAutorDTO>(libro);

            if (libro is null)
            {
                return NotFound();
            }
            return Ok(libroConAutorDTO);
        }

        [HttpGet("BuscarPorTitulo")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetPorTitulo([FromQuery] string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("Debe proporcionar un título para buscar.");
            }

            var libros = await context.Libros
                .Where(l => l.titulo.Contains(titulo)) // Puedes usar .ToLower().Contains(titulo.ToLower()) si quieres ignorar mayúsculas
                .ToListAsync();

            var libroDTOs = mapper.Map<IEnumerable<LibroDTO>>(libros);

            return Ok(libroDTOs);
        }



        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            var existeautor = await context.Autores.AnyAsync(x => x.Id == libroCreacionDTO.autorId);

            if (!existeautor)
            {
                //return BadRequest("No existe el autor");
                ModelState.AddModelError(nameof(libroCreacionDTO.autorId), $"No existe el autor con el id {libroCreacionDTO.autorId}"); /*Agregamos un error al modelo*/
                return ValidationProblem(); /*Retorna un 400 y el error que agregamos al modelo*/

            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);

            context.Add(libro);
            await context.SaveChangesAsync();
            //return Ok();
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libro); /*Retorno el autor en el jeison para que me tome la modf*/
        }   /*El CreatedAtRoute me permite retornar el libro creado y la ruta para obtenerlo, en este caso la ruta es ObtenerLibro*/


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(LibroCreacionDTO libroCreacionDTO, int id)
        {
            var libro = mapper.Map<Libro>(libroCreacionDTO);
            libro.Id = id;  /*Esto se hace porque el dto no tiene el ID y la clase Libro SI*/


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
            // return Ok();
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchdoc)
        {
            if (patchdoc == null) { return BadRequest(); }

            var libroBD = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            
            if (libroBD is null) { return NotFound(); }

            var libroPatchDTO = mapper.Map<LibroPatchDTO>(libroBD);

            patchdoc.ApplyTo(libroPatchDTO, ModelState);

            var librovalido = TryValidateModel(libroPatchDTO);

            if (!librovalido) { return ValidationProblem(); }

            mapper.Map(libroPatchDTO, libroBD);

            await context.SaveChangesAsync();


            return NoContent();

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
            // return Ok(libro);
            return NoContent();
        }   
    }
}
