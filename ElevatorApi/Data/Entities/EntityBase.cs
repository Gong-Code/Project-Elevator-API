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
}

