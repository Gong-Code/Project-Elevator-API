using AutoMapper;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Comment;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Models.Errands;

namespace ElevatorApi.Helpers.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ElevatorEntity, ElevatorDto>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsString()));
            CreateMap<ElevatorDto, ElevatorEntity>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsEnum()));
            CreateMap<ElevatorEntity, CreateElevatorDto>().ReverseMap();
            CreateMap<ElevatorEntity, ElevatorWithErrandsDto>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsString()));



            CreateMap<ErrandDto, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<ErrandEntity, ErrandDto>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsString()));
            CreateMap<AddErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<UpdateErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<ErrandEntity, ErrandWithCommentsDto>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsString())).ForMember(x => x.ElevatorId, y => y.MapFrom(x => x.ElevatorEntity.Id));



            CreateMap<CommentDto, CommentEntity>().ForMember(x => x.Id, y => y.MapFrom(x => x.CommentId)).ReverseMap();
            CreateMap<CreateCommentDto, CommentEntity>();

        }
    }
}
