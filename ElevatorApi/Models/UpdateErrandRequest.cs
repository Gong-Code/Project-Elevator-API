namespace ElevatorApi.Models
{
    public class UpdateErrandRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = null!;
        public Guid AssignedTo { get; set; }
    }
}
