using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Comment;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Models.Errands;

namespace ElevatorApi.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsString()));
            CreateMap<ElevatorDto, ElevatorEntity>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsEnum()));
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();


            CreateMap<ErrandDto, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<ErrandEntity, ErrandDto>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsString()));
            CreateMap<AddErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<UpdateErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));



            CreateMap<CommentDto, CommentEntity>().ForMember(x => x.Id, y => y.MapFrom(x => x.CommentId)).ReverseMap();
            CreateMap<CreateCommentDto, CommentEntity>();

        }
    }
}
