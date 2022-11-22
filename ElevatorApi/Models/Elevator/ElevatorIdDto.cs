namespace ElevatorApi.Models.Elevator
{
    public class ElevatorIdDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
    }
}