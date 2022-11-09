using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Data.Entities;

public class EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public Guid CreatedById { get; set; }
    public string CreatedByName { get; set; } = null!;
    public DateTime LastEditedDateUtc { get; set; }
    public Guid LastEditedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
}

