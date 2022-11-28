namespace ElevatorApi.Models.ElevatorDtos
{
    public class Elevator
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
    }
}
