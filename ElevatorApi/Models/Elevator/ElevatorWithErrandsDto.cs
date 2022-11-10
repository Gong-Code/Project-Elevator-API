using ElevatorApi.Models.Errands;

namespace ElevatorApi.Models.Elevator
{
    public class ElevatorWithErrandsDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public IList<ErrandDto> Errands { get; set; } = new List<ErrandDto>();
    }
}
