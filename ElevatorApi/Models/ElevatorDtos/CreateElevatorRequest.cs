using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.ElevatorDtos
{
    public class CreateElevatorRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Location { get; init; } = null!;

        [Required]
        [Range(2,20)]
        public int NumberOfFloors { get; init; }
    }
}
