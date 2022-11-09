namespace ElevatorApi.Models.Elevator
{
    public class ElevatorDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
    }
}
