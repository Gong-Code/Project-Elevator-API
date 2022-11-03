namespace ElevatorApi.Data.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class CommentEntity : EntityBase
{
    public ErrandEntity ErrandEntity { get; set; } = null!;
    public string Message { get; set; } = null!;
}