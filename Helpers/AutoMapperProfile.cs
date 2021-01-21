using AutoMapper;
using BarberAPI.DTO;
using BarberAPI.Entities;
using BarberAPI.Models;

namespace BarberAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Client, ClientModel>();
            CreateMap<RegisterClientModel, Client>();
            CreateMap<UpdateModel, Client>();
            CreateMap<RegisterBarberModel, Barber>();
            CreateMap<BarberDTO, Barber>();
        }
    }
}
