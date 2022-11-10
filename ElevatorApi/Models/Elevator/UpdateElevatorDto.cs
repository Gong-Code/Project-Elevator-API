using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.Elevator
{
    public class UpdateElevatorDto
    {
        [Required]
        [RegularExpression("enabled|disabled|error", ErrorMessage = "Status must be enabled, disabled, error.")]
        public string ElevatorStatus { get; set; } = null!;
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Location { get; set; } = null!;
    }
}
