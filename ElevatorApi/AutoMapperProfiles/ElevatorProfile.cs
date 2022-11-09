using AutoMapper;
using ElevatorApi.Data.Entities;
using static ElevatorApi.Controllers.ElevatorsController;

namespace ElevatorApi.AutoMapperProfiles
{
    public class ElevatorProfile : Profile
    {
        public ElevatorProfile()
        {
            CreateMap<ElevatorEntity, Elevator>().ReverseMap();
            CreateMap<ElevatorEntity, CreateElevator>().ReverseMap();
        }
    }
}
