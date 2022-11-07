using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers;

[Route("api/elevators/{elevatorId:guid}/errands/{errandId:guid}/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly SqlDbContext _sqlContext;

    public CommentsController(SqlDbContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    [HttpGet("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> GetCommentForErrandById(Guid elevatorId, Guid errandId, Guid commentId)
    {
        try
        {
            var comment =
                (await _sqlContext.Errands.Include(i => i.Comments)
                    .FirstOrDefaultAsync(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId))?.Comments
                .FirstOrDefault(x => x.Id == commentId);
            if (comment is null)
                throw new ArgumentNullException();

            var commentToRetun = new Comment(comment);

            return Ok(commentToRetun);
        }
        catch
        {
            // ignored
        }

        return BadRequest("could not get comment");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Comment>>> GetAllCommentsForErrand(Guid elevatorId, Guid errandId)
    {
        try
        {
            var errand = await _sqlContext.Errands.Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId);

            if (errand is null)
                throw new ArgumentNullException();

            var comments = errand.Comments.OrderBy(x => x.CreatedDateUtc).Select(comment =>
                new Comment(comment));

            return Ok(comments);
        }
        catch
        {
            // ignored
        }

        return BadRequest("could not get comments");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCommentForErrand(Guid elevatorId, Guid errandId,
        [FromBody] CreateComment model)
    {
        try
        {
            var errand =
                await _sqlContext.Errands.FirstOrDefaultAsync(
                    e => e.ElevatorEntity.Id == elevatorId && e.Id == errandId);
            if (errand is null)
                throw new ArgumentNullException();

            var comment = new CommentEntity
            {
                Message = model.Message
            };

            errand.Comments.Add(comment);

            await _sqlContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentForErrandById), new {elevatorId, errandId, commentId = comment.Id},
                new Comment(comment));
        }
        catch
        {
            // ignored
        }

        return BadRequest("could not create comment");
    }


    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Comment
    {
        public Comment(CommentEntity entity)
        {
            CommentId = entity.Id;
            CreatedById = entity.CreatedById;
            CreatedDateUtc = entity.CreatedDateUtc;
            CreatedByName = entity.CreatedByName;
            Message = entity.Message;
        }

        public Guid CommentId { get; init; }
        public Guid CreatedById { get; init; }
        public string CreatedByName { get; init; }
        public DateTime CreatedDateUtc { get; init; }
        public string Message { get; init; }
    }

    public class CreateComment
    {
        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "The {0} value must be between {1} and {2} chars long\"")]
        public string Message { get; set; } = null!;
    }
}