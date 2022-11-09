using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models;
using static ElevatorApi.Controllers.ElevatorsController;

namespace ElevatorApi.AutoMapperProfiles
{
    public class ElevatorProfile : Profile
    {
        public ElevatorProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ReverseMap();
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();
        }
    }
}
