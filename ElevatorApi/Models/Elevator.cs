namespace ElevatorApi.Models
{
    public class Elevator
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Active,
        InActive
    }
}
