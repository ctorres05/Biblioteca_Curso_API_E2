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
                .Include(x => x.Autores)
                .ToListAsync();
            var libroDTO = mapper.Map<IEnumerable<LibroDTO>>(libro);
            return libroDTO;
        }

        [HttpGet("DameUno/{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroConAutorDTO>> Get(int id)

        {
            var libro = await context.Libros
                .Include(x => x.Autores)
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
            if (libroCreacionDTO.autoresId is null || libroCreacionDTO.autoresId.Count == 0)
            {
                ModelState.AddModelError(nameof(libroCreacionDTO.autoresId), "No se puede crear un libro sin autores."); /*Agregamos un error al modelo*/
                return ValidationProblem(); /*Retorna un 400 y el error que agregamos al modelo*/
            }

            //var existeautor = await context.Autores.AnyAsync(x => x.Id == 1 /*libro.autorId*/);
            var existeautores = await context.Autores
                .Where(x => libroCreacionDTO.autoresId.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(); /*Trae los autores que existen en la base de datos y los guarda en una lista*/

            var lista_autoresfaltantes   = libroCreacionDTO.autoresId.Except(existeautores).ToList();


            //if (existeautores.Count != libroCreacionDTO.autoresId.Count)
            if(lista_autoresfaltantes.Count > 0) /*Si hay autores que no existen en la base de datos*/
            {
                var idsFaltantes = string.Join(", ", lista_autoresfaltantes); /*Convierte la lista de ids faltantes a un string separado por comas*/
                ModelState.AddModelError(nameof(libroCreacionDTO.autoresId), $"Los siguientes autores no existen: {idsFaltantes}"); /*Agregamos un error al modelo*/
                return ValidationProblem(); /*Retorna un 400 y el error que agregamos al modelo*/
            }

           

            var libro = mapper.Map<Libro>(libroCreacionDTO); /*Mapea el libroCreacionDTO a un objeto Libro*/
            AsignarOrdenAutores(libro); /*Asignamos el orden a los autores*/



            context.Add(libro);
            await context.SaveChangesAsync();

            
            --var libroDTO = Mapper.Map<LibroDTO>(libro); /*Mapea el libro a un objeto LibroDTO para retornar en la respuesta*/ç

            //return Ok();
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDTO); /*Retorno el autor en el jeison para que me tome la modf*/
            /*El CreatedAtRoute me permite retornar el libro creado y la ruta para obtenerlo, en este caso la ruta es ObtenerLibro*/

        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.Autores is not null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i ; /*Asignamos el orden a los autores*/
                }
            }
        }

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
            
            if (libroBD is null) { return NotFound($"libro {id} no existe"); }

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

            var existe = await context.Libros.AnyAsync(x => x.Id == id);    

            if (!existe)
            {
                return NotFound($"No existe un libro con el ID {id}.");
            }
            
            
            var regborrado = await context.Libros
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(); // Ejecuta un borrado directo en la base de datos sin necesidad de traer el objeto completo  
            
            
            if (regborrado == 0)
            {
                return NotFound($"No se pudo borrar el libro con el ID {id}.");
            }   
            return NoContent(); // Retorna un 204 No Content, indicando que la operación se realizó correctamente sin retornar contenido 


            /*
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            
            if (libro is null)
            {
                return NotFound();
            }
            
            context.Remove(libro);
            await context.SaveChangesAsync();
            // return Ok(libro);
            return NoContent();

            */
        }   
    }
}
