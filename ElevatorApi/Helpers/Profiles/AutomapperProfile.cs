using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.CommentDtos;
using ElevatorApi.Models.DeviceDtos;
using ElevatorApi.Models.ElevatorDtos;
using ElevatorApi.Models.ErrandDtos;
using ElevatorApi.Models.UserDtos;

namespace ElevatorApi.Helpers.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ElevatorEntity, Elevator>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsString()));
            CreateMap<Elevator, ElevatorEntity>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsEnum()));
            CreateMap<ElevatorEntity, CreateElevatorRequest>().ReverseMap();
            CreateMap<ElevatorEntity, ElevatorWithErrands>().ForMember(x => x.ElevatorStatus, y => y.MapFrom(x => x.ElevatorStatus.GetElevatorStatusAsString()));



            CreateMap<Errand, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<ErrandEntity, Errand>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsString()));
            CreateMap<CreateErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<UpdateErrandRequest, ErrandEntity>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsEnum()));
            CreateMap<ErrandEntity, ErrandWithComments>().ForMember(x => x.ErrandStatus, y => y.MapFrom(x => x.ErrandStatus.GetErrandStatusAsString())).ForMember(x => x.ElevatorId, y => y.MapFrom(x => x.ElevatorEntity.Id));



            CreateMap<Comment, CommentEntity>().ForMember(x => x.Id, y => y.MapFrom(x => x.CommentId)).ReverseMap();
            CreateMap<CreateCommentRequest, CommentEntity>();


            //CreateMap<User, UserEntity>().ForMember(x => x.Role, y => y.MapFrom(x => x.Role.GetRoleAsEnum()));
            //CreateMap<UserEntity, User>().ForMember(x => x.Role, y => y.MapFrom(x => x.Role.GetRoleAsString()));
            //CreateMap<UserEntity, User>().ReverseMap();





            // Maps for DeviceController
            CreateMap<DeviceRequest,CreateElevatorRequest>();
        }
    }
}
