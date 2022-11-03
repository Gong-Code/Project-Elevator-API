namespace ElevatorApi.Models
{
    public class Errand
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? AssignedBy { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

    }
}
