using ElevatorApi.Data.Entities;

namespace ElevatorApi.Models
{
    public class Errand : ErrandEntity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; } 
        public Enums.ErrandStatus ErrandStatus { get; set; }
        public string? AssignedBy { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

    }
}
