using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models;

namespace ElevatorApi.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => GetElevatorStatusAsString(x.ElevatorStatus)));
            CreateMap<ElevatorDto, ElevatorEntity>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => GetElevatorStatusAsEnum(x.ElevatorStatus)));
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();


            CreateMap<ErrandDto, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => GetErrandStatusAsEnum(x.ErrandStatus)));
            CreateMap<ErrandEntity, ErrandDto>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => GetErrandStatusAsString(x.ErrandStatus)));
            CreateMap<AddErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => GetErrandStatusAsEnum(x.ErrandStatus)));
        }



        private static Enums.ErrandStatus GetErrandStatusAsEnum(string status)
        {
            return status.ToLower() switch
            {
                "new" => Enums.ErrandStatus.New,
                "inprogress" => Enums.ErrandStatus.InProgress,
                "completed" => Enums.ErrandStatus.Completed,
                _ => Enums.ErrandStatus.New
            };
        }
        private static string GetErrandStatusAsString(Enums.ErrandStatus status)
        {
            return status switch
            {
                Enums.ErrandStatus.New => "new",
                Enums.ErrandStatus.InProgress => "inprogress",
                Enums.ErrandStatus.Completed => "completed",
                _ => "new"
            };
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
