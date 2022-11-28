// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace ElevatorApi.Models.CommentDtos
{
    public class Comment
    {
        public Guid CommentId { get; init; }
        public Guid CreatedById { get; init; }
        public string CreatedByName { get; init; } = null!;
        public DateTime CreatedDateUtc { get; init; }
        public string Message { get; init; } = null!;
    }
}
