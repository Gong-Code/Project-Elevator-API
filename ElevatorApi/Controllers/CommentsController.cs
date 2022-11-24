using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers;
using ElevatorApi.Models.CommentDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers;

[Route("api/elevators/{elevatorId:guid}/errands/{errandId:guid}/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly SqlDbContext _sqlContext;
    private readonly IMapper _mapper;

    public CommentsController(SqlDbContext sqlContext, IMapper mapper)
    {
        _sqlContext = sqlContext;
        _mapper = mapper;
    }

    [HttpGet("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCommentForErrandById(Guid elevatorId, Guid errandId, Guid commentId)
    {
        try
        {
            var comment =
                (await _sqlContext.Errands.Include(i => i.Comments)
                    .FirstOrDefaultAsync(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId))?.Comments
                .FirstOrDefault(x => x.Id == commentId);
            if (comment is null)
                return NotFound();

            var commentToRetun = _mapper.Map<Comment>(comment);

            return Ok(new HttpResponse<Comment>(commentToRetun));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCommentsForErrand(Guid elevatorId, Guid errandId)
    {
        try
        {
            var errand = await _sqlContext.Errands.Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId);

            if (errand is null)
                return NotFound();

            var comments = errand.Comments.OrderBy(x => x.CreatedDateUtc).ToList();

            var commentsToReturn = _mapper.Map<IEnumerable<Comment>>(comments);

            return Ok(new HttpResponse<IEnumerable<Comment>>(commentsToReturn));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCommentForErrand(Guid elevatorId, Guid errandId,
         CreateCommentRequest model)
    {
        try
        {
            var errand =
                await _sqlContext.Errands.FirstOrDefaultAsync(
                    e => e.ElevatorEntity.Id == elevatorId && e.Id == errandId);
            if (errand is null)
                return NotFound();

            var comment = _mapper.Map<CommentEntity>(model);

            errand.Comments.Add(comment);

            await _sqlContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentForErrandById), new { elevatorId, errandId, commentId = comment.Id }, _mapper.Map<Comment>(comment));
        }
        catch
        {
            return StatusCode(500);
        }
    }
}