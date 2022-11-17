using ElevatorApi.Models.Errands;
// ReSharper disable CollectionNeverUpdated.Global

namespace ElevatorApi.Models.Elevator
{
    public class ElevatorWithErrandsDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
        public IList<ErrandDto> Errands { get; set; } = new List<ErrandDto>();
    }
}
