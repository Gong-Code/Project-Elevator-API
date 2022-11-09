namespace ElevatorApi.Models
{
    public class ElevatorDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
