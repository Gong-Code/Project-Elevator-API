using System.Collections;
using ElevatorApi.Controllers;
using ElevatorApi.Data;
using ElevatorApi.Helpers;
using ElevatorApi.Helpers.Profiles;
using ElevatorApi.Models.Comment;

namespace ElevatorApi.Tests.ControllerTests;

public class CommentsTest : BaseTest
{
    private readonly SqlDbContext _context;

    private readonly CommentsController _sut;

    public CommentsTest()
    {
        var contextOptions = new DbContextOptionsBuilder<SqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new SqlDbContext(contextOptions, UserService.Object);


        var config = new MapperConfiguration(c =>
        {
            c.AddProfile<AutoMapperProfile>();
        });

        var mapper = config.CreateMapper();

        _sut = new CommentsController(_context, mapper);
    }


    [Fact]
    public async void GetAll_with_valid_data_should_return_OkObjectResult()
    {
        var (elevatorId, errandId) = await SetupContextAndReturnIds();

        var result = await _sut.GetAllCommentsForErrand(elevatorId, errandId) as OkObjectResult;

        Assert.NotNull(result?.Value);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsNotType<CommentEntity>(result.Value);
    }

    [Fact]
    public async void GetAll_with_invalid_data_should_return_NotFoundResult()
    {
        var result = await _sut.GetAllCommentsForErrand(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, result.StatusCode);
        Assert.IsNotType<CommentEntity>(result);
    }

    [Fact]
    public async void GetAll_with_null_data_should_return_NotFoundResult()
    {
        var result = await _sut.GetAllCommentsForErrand(Guid.Empty, Guid.Empty) as NotFoundResult;

        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, result.StatusCode);
        Assert.IsNotType<CommentEntity>(result);
    }

    [Fact]
    public async void GetAll_Should_return_10_comments()
    {
        var (elevatorId, errandId) = await SetupContextAndReturnIds();

        var result = await _sut.GetAllCommentsForErrand(elevatorId, errandId) as OkObjectResult;

        var items = (result?.Value as HttpResponse<IEnumerable<CommentDto>>);

        Assert.NotNull(result?.Value);
        Assert.NotNull(items!.Data);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(10, items.Data.Count());
        Assert.IsNotType<CommentEntity>(items.Data.First());
        Assert.IsAssignableFrom<IEnumerable<CommentDto>>(items.Data);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void GetById_should_return_specific_comment()
    {
        var (elevatorId, errandId) = await SetupContextAndReturnIds();

        var comments = _context.Errands.First().Comments;

        var firstComment = comments.First();
        var secondComment = comments.Skip(1).First();

        var result = await _sut.GetCommentForErrandById(elevatorId, errandId, firstComment.Id) as OkObjectResult;

        var item = (result?.Value as HttpResponse<CommentDto>);

        Assert.Equal(200, result?.StatusCode);
        Assert.NotNull(result?.Value);
        Assert.NotNull(item!.Data);
        Assert.Equal(firstComment.Id, item.Data.CommentId);
        Assert.Equal(firstComment.CreatedById, item.Data.CreatedById);
        Assert.NotEqual(secondComment.Id, item.Data.CommentId);
        Assert.IsType<OkObjectResult>(result);
        Assert.IsNotType<CommentEntity>(item);
        Assert.IsType<CommentDto>(item.Data);
    }

    [Fact]
    public async void GetAll_should_return_comments_with_correct_createdby()
    {
        var (elevatorId, errandId) = await SetupContextAndReturnIds();

        var result = await _sut.GetAllCommentsForErrand(elevatorId, errandId) as OkObjectResult;

        var items = (result?.Value as HttpResponse<IEnumerable<CommentDto>>);

        Assert.Equal(200, result?.StatusCode);
        Assert.NotNull(result?.Value);
        Assert.NotNull(items?.Data);
        Assert.IsAssignableFrom<IEnumerable>(items.Data);
        Assert.True(items.Data.ToList().TrueForAll(x => x.CreatedById == UserGuid && x.CreatedByName == UserName));
        Assert.Equal(10, items.Data.Count());
        Assert.IsNotType<CommentEntity>(items.Data.First());
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void GetById_invalid_data_should_return_NotFoundResult()
    {
        var result = await _sut.GetCommentForErrandById(Guid.Empty, Guid.Empty, Guid.Empty) as NotFoundResult;

        Assert.Equal(404, result?.StatusCode);
        Assert.IsType<NotFoundResult>(result);
        Assert.IsNotType<CommentEntity>(result);
    }

    [Fact]
    public async void CreateComment_should_return_comment_with_correct_details()
    {
        var (elevatorId, errandId) = await SetupContextAndReturnIds();

        var comment = new CreateCommentDto
        {
            Message = Fixture.Create<string>()
        };

        var result = await _sut.CreateCommentForErrand(elevatorId, errandId, comment) as CreatedAtActionResult;

        var item = result?.Value as CommentDto;

        Assert.Equal(201, result?.StatusCode);
        Assert.Equal(comment.Message, item?.Message);
        Assert.Equal(UserGuid, item!.CreatedById);
        Assert.Equal(UserName, item.CreatedByName);
        Assert.NotNull(result?.Value);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.IsNotType<CommentEntity>(item);
        Assert.IsType<CommentDto>(item);
    }

    [Fact]
    public async void CreateComment_invalid_data_should_return_NotFoundResult()
    {
        var comment = new CreateCommentDto
        {
            Message = Fixture.Create<string>()
        };

        var result = await _sut.CreateCommentForErrand(Guid.Empty, Guid.Empty, comment) as NotFoundResult;


        Assert.Equal(404, result?.StatusCode);
        Assert.IsType<NotFoundResult>(result);
        Assert.IsNotType<CommentEntity>(result);
    }


    private async Task<(Guid elevatorId, Guid errandId)> SetupContextAndReturnIds()
    {
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var elevator = Fixture
            .Build<ElevatorEntity>()
            .Without(x => x.Errands)
            .Create();

        elevator.Errands = Fixture
            .Build<ErrandEntity>()
            .With(x => x.ElevatorEntity, elevator)
            .CreateMany(5).ToList();

        foreach (var elevatorErrand in elevator.Errands)
            elevatorErrand.Comments = Fixture.Build<CommentEntity>().With(x => x.ErrandEntity, elevatorErrand)
                .CreateMany(10).ToList();


        await _context.AddAsync(elevator);
        await _context.SaveChangesAsync();

        return (elevator.Id, elevator.Errands.FirstOrDefault()!.Id);
    }
}