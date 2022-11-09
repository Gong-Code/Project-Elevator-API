using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Data.Entities;

public class EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime LastEditedDateUtc { get; set; }
    public Guid LastEditedById { get; set; }
    public string? AssignedBy { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
}

