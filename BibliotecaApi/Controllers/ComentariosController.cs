using AutoMapper;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;
using BibliotecaApi.Entidades.Datos;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{

    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]

    public class ComentariosController  : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existelibro = await context.Libros
                .AnyAsync(x => x.Id == libroId);
            if (!existelibro)
            {
                return NotFound($"El libro {libroId} no Existe!!!");
            }


            else
            {

                await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();
                
                var comentarios = await context.Comentarios
                    .Where(x => x.LibroId == libroId)
                    .OrderByDescending(x => x.FechaCreacion) // Ordenar por fecha de creación descendente   
                    .ToListAsync();


                var comentarioDTO = mapper.Map<List<ComentarioDTO>>(comentarios);
                
                return comentarioDTO;
            }
        }
        [HttpGet("DameUno/{Id:guid}", Name = "ObtenerComentario")]
        public async  Task<ActionResult<ComentarioDTO>> Get(int libroId, Guid Id)
        {
            var comentario = await context.Comentarios
                .FirstOrDefaultAsync(x => x.Id == Id && x.LibroId == libroId);


            if (comentario is null)
            {
                return NotFound($"No existe un comentario con el ID guid {Id} para el libro con ID {libroId}.");
            }
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return Ok(comentarioDTO);
        }

    



        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);

            comentario.LibroId = libroId;
            comentario.Id = Guid.NewGuid(); // Generar un nuevo GUID para el comentario 
            comentario.FechaCreacion = DateTime.UtcNow; // Establecer la fecha de creación al momento actual    

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {return NotFound($"No existe un libro con el ID {libroId} al que se le pueda agregar un comentario.");}

            context.Add(comentario);
            await context.SaveChangesAsync();
         
            return CreatedAtRoute("ObtenerComentario", new { libroId = comentario.LibroId, Id = comentario.Id }, comentario); /*Retorno el autor en el jeison para que me tome la modf*/
          
            //return Ok();
            /*
           public Guid Id { get; set; }                        = Guid.NewGuid();
           [Required]
           public required  string? Cuerpo { get; set; }       // Comentario del usuario
           public DateTime FechaCreacion { get; set; }          DateTime.UtcNow; // Fecha de creación del comentario
           public int LibroId { get; set; }                    // ID del libro al que pertenece el comentario
           public Libro? Libro { get; set; }                   // Navegación al libro asociado

            */

        }

        [HttpDelete("{Id:guid}")]

        public async Task<ActionResult> Delete(int libroId, Guid Id)
        {
            var comentario = await context.Comentarios
                .FirstOrDefaultAsync(x => x.Id == Id && x.LibroId == libroId);
            if (comentario is null)
            {
                return NotFound($"No existe un comentario con el ID guid {Id} para el libro con ID {libroId}.");
            }
            context.Remove(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{Id:guid}")] // Actualizar un comentario por su ID    
        public async Task<ActionResult> Patch(int libroId, Guid Id, JsonPatchDocument<ComentarioCreacionDTO> patchdoc)
        {
            if (patchdoc is null)   {  return BadRequest("El patch document no puede ser nulo.");       }

            var comentario = await context.Comentarios
                .FirstOrDefaultAsync(x => x.Id == Id && x.LibroId == libroId);


            if (comentario is null)     {  return NotFound($"No existe un comentario con el ID guid {Id} para el libro con ID {libroId}.");    }

           var ComentarioPatchDTO = mapper.Map<ComentarioPatchDTO>(comentario);
            // Aplicar el patch document al DTO

            patchdoc.ApplyTo(ComentarioPatchDTO, ModelState);

            var comentariovalido =  ModelState.IsValid;

            if (!comentariovalido)
            {
                return ValidationProblem();
            }

                    
            // Actualizar la clase  comentario con los datos del DTO
            mapper.Map(ComentarioPatchDTO, comentario);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
    
      

        


    
}
