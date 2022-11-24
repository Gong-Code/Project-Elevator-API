using ElevatorApi.Models.CommentDtos;

namespace ElevatorApi.Models.ErrandDtos
{
    public class ErrandWithComments
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public Guid ElevatorId { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public Guid AssignedToId { get; set; }
        public string AssignedToName { get; set; } = null!;
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
