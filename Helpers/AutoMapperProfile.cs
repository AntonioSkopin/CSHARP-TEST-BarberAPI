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
            CreateMap<RegisterModel, Client>();
            CreateMap<UpdateModel, Client>();
            CreateMap<RegisterBarberModel, Barber>();
            CreateMap<BarberDTO, Barber>();
            CreateMap<RegisterModel, BarbershopOwner>();
        }
    }
}
