using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models;

namespace ElevatorApi.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ForMember(x => x.Status, y => y.MapFrom(x => GetElevatorStatusAsString(x.ElevatorStatus)));
            CreateMap<ElevatorDto, ElevatorEntity>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => GetElevatorStatusAsEnum(x.Status)));
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();
        }


        private static string GetElevatorStatusAsString(Enums.ElevatorStatus status)
        {
            return status switch
            {
                Enums.ElevatorStatus.Enabled => "enabled",
                Enums.ElevatorStatus.Disabled => "disabled",
                Enums.ElevatorStatus.Error => "error",
                _ => "enabled"
            };
        }

        private static Enums.ElevatorStatus GetElevatorStatusAsEnum(string status)
        {
            return status.ToLower() switch
            {
                "enabled" => Enums.ElevatorStatus.Enabled,
                "disabled" => Enums.ElevatorStatus.Disabled,
                "error" => Enums.ElevatorStatus.Error,
                _ => Enums.ElevatorStatus.Enabled
            };
        }
    }
}
