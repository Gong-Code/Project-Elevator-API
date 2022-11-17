using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.Errands
{
    public class UpdateErrandDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
        public string Title { get; } = null!;
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
        public string Description { get; } = null!;
        [Required]
        [RegularExpression("new|inprogress|completed", ErrorMessage = "Status must be new, inprogress, completed.")]
        public string ErrandStatus { get; } = null!;
        [Required]
        public Guid AssignedToId { get; set; }
    }
}
