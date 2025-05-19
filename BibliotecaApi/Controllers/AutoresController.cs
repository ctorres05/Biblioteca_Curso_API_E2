using BibliotecaApi.Entidades;
using BibliotecaApi.Entidades.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly AplicationDbContext contex;
      

        public AutoresController(AplicationDbContext contex)  /*contructor*/
        {
            this.contex = contex;
          
        }

        [HttpGet]
        public async Task<IEnumerable<Autor>> Get()
        {                
            return await contex.Autores.ToListAsync();
        }

        [HttpGet("DamePrimero")]  /* entra con --> api/autotrds/DamePrimero  */
        [HttpGet("/DamePrimero")] /* entra tambien con --> DamePrimero   */
        public async Task<ActionResult<Autor>> GetPrimero()
        {
            var autor = await contex.Autores
                .Include(x => x.Libros)
                .FirstAsync();

            return Ok(autor);

        }

        [HttpGet("fijo/{id}")]
        public IEnumerable<Autor> Get1()
        {
            return new List<Autor>
            {
                new Autor { Id = 1, nombres = "Felipe"   ,  apellido = "TORRES", identificacion = "abc",  edad = 55},
                new Autor { Id = 2, nombres = "Magui"    ,  apellido = "TORRES", identificacion = "abc",  edad = 51},
            };
        }


        [HttpGet("DameUno/{id:int}", Name = "ObtenerAutor")   ]   /*La ruta seria  api/autores/DameUno/5    si es [Fromquery] --> api/autores/DameUno/5?incluyelibro= true */
        public async Task<ActionResult<Autor>> Get([FromRoute] int id, [FromHeader] bool incluyelibro)
        {
            Autor autor;

            if (!incluyelibro)
            {
                autor = await contex.Autores
                    .Include(x => x.Libros)   /*Esto hace que me triga los libros relacionados al autor*/
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                autor = await contex.Autores
                     .FirstOrDefaultAsync(x => x.Id == id);

            }
            if (autor is null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpGet("DameUno/{nombre:alpha}")]    /*Tengo dos DameUno uno que funciona con parametro entero busca uno en particular con su ID
                                                * y otro que busca una cadena en el campo nombre*/
        public async Task<IEnumerable<Autor>> Get(string nombre)
        {
            return await contex.Autores
                .Include(x => x.Libros)
                .Where(x => x.nombres.Contains(nombre)).ToListAsync();
        }

        [HttpGet("Dame_con_param/{param1}/{param2?}")]

        public async Task<ActionResult> Get(string param1, string param2="Por defecto")
        {
            return Ok(new { param1, param2});


        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            contex.Add(autor);
            await contex.SaveChangesAsync();
            //return Ok();
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autor); /*Retorno el autor en el jeison para que me tome la modf*/

        }

        [HttpPut("{id:int}") ]

        public async Task<ActionResult> Put(int id, Autor autor) 
        {
            if (id != autor.Id)   {  return BadRequest(); }

            contex.Update(autor);
            await contex.SaveChangesAsync();

            return Ok(autor) ;  /*Retorno el autor en el jeison para que me tome la modf*/

        }

        [HttpDelete ("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autor = await contex.Autores.FirstOrDefaultAsync(x => x.Id == id);

            /*var registrosborrados = await contex.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();  */ /*OTRA FORMA DE BORRAR CON WHERE */
            var registrosborrados = await contex.Autores.Where(x => x.Id == id).CountAsync();


            if (autor is null)
            {
                return NotFound();
            }
            contex.Remove(autor);
            await contex.SaveChangesAsync();
            return Ok(registrosborrados);
        }   
    }
}
