using ElevatorApi.Models.UserDtos;

namespace ElevatorApi.Data.Entities
{
    public class UserEntity : EntityBase
    {
        public string Id { get; set; }
        public string Role { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }

    
}
