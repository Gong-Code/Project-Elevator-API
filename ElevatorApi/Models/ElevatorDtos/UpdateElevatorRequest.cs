using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.ElevatorDtos
{
    public class UpdateElevatorRequest
    {
        [Required]
        [RegularExpression("enabled|disabled|error", ErrorMessage = "Status must be enabled, disabled, error.")]
        public string ElevatorStatus { get; } = null!;
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Location { get; } = null!;
    }
}
