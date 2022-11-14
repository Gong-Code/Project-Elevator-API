using ElevatorApi.Models.Comment;

namespace ElevatorApi.Models.Errands
{
    public class ErrandWithCommentsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public Guid ElevatorId { get; set; }
        public IList<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public Guid AssignedToId { get; set; }
        public string AssignedToName { get; set; } = null!;
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
