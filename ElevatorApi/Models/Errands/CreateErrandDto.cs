using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.Errands
{
    public class CreateErrandDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
        public string Title { get; set; } = null!;
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
        public string Description { get; set; } = null!;
        [RegularExpression("new|inprogress|completed", ErrorMessage = "Status must be new, inprogress, completed.")]
        public string ErrandStatus { get; } = "new";
        [Required]
        public string AssignedToId { get; } = null!;
    }
}
