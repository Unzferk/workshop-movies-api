using AutoMapper;
using WorkshopAPI.Models;
using WorkshopAPI.Models.DTOs;

namespace WorkshopAPI.Mapper
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
        }
    }
}
