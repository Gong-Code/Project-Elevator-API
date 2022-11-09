using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models;
using static ElevatorApi.Controllers.ElevatorsController;

namespace ElevatorApi.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ReverseMap();
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();
        }
    }
}
