namespace ElevatorApi.Models
{
    public class AddErrandRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ErrandStatus { get; set; } = "new";
        public string AssignedToId { get; set; } = null!;
    }
}
