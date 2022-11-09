namespace ElevatorApi.Models
{
    public class ErrandDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public Guid AssignedToId { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
