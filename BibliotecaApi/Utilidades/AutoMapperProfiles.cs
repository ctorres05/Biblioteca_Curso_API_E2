using AutoMapper;
using BibliotecaApi.DTOs;
using BibliotecaApi.Entidades;

namespace BibliotecaApi.Utilidades
{
    public class AutoMapperProfiles  : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Autor, AutorDTO>()
                .ForMember(dto => dto.NombreCompleto,  
                    config => config.MapFrom(autor => MapearNombreYApellidoAutor(autor)));

            CreateMap<Autor, AutorConLibroDTO>()
               .ForMember(dto => dto.NombreCompleto,
                   config => config.MapFrom(autor => MapearNombreYApellidoAutor(autor)));

            CreateMap< AutorCreacionDTO, Autor>();

            CreateMap< Autor, AutorPatchDTO>().ReverseMap(); /*significa que el mapeo va en ambos sentidos*/
            

            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroCreacionDTO , Libro>();

            CreateMap<Libro, LibroPatchDTO>().ReverseMap(); /*significa que el mapeo va en ambos sentidos*/

            CreateMap<Libro, LibroConAutorDTO>()
                .ForMember(dto => dto.autornombre,
                     config => config.MapFrom(a => MapearNombreYApellidoAutorParaLibro(a.Autor!)));
        }

        
        private string MapearNombreYApellidoAutor(Autor autor) => $"{autor.nombres} {autor.apellido} Edad: {autor.edad}";
        /*Esta es una funcion que le entra como parametro un autor y retorna un string concatenado el nombre + el apellido y la edad*/
        private string MapearNombreYApellidoAutorParaLibro(Autor autor) => $"{autor.apellido} {autor.nombres} ";


    }
}
