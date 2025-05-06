using BibliotecaApi.Entidades.Datos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using BibliotecaApi;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// *******  Area de servicios  ********

builder.Services.AddControllers()  /*Esto me permite habilitar la funcionalidad de los controladores*/
    .AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);   /*Esto evita referencias ciclicas*/
    


builder.Services.AddDbContext<AplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConection")); /*Configuramos el dbcontext como servicio usando swlserver y
                                                                                                //builder.Services.AddTransient<IRepoValores, RepoValoresInyeOracle>(); /*Esto permite inyectar el servicio de la 
                                                                                                // * interfaz IRepoValores y la implementacion RepoValoresInyeORACLE*/
//



var app = builder.Build();






// ********** Fin area de SEvicios ***********

//Comentado para sacar el mensaje por defecto que trae la aplicacion 
//app.MapGet("/", () => "Hello World!");


// ******  Area de Middlewares ********
//************************************
       

app.MapControllers(); // Esto permite que las peticiones hattp las manda al sistema de controladores

//******   Fin Middlewares  1616*********

app.Run();
