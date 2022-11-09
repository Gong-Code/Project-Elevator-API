namespace ElevatorApi.Data.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ErrandEntity : EntityBase
{
    public ElevatorEntity ElevatorEntity { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Enums.ErrandStatus ErrandStatus { get; set; } = Enums.ErrandStatus.New;
    public Guid AssignedToId { get; set; }
    public IList<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}