using BarberAPI.Auth.Entities;
using BarberAPI.Auth.Models;
using AutoMapper;

namespace BarberAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Client, ClientModel>();
            CreateMap<RegisterClientModel, Client>();
            CreateMap<UpdateClientModel, Client>();
        }
    }
}
