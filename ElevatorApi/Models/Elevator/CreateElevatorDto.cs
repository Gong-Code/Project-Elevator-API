using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.Elevator
{
    public class CreateElevatorDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Location { get; set; } = null!;
    }
}
