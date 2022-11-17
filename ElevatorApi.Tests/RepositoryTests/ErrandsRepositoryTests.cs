using ElevatorApi.Data;
using ElevatorApi.Helpers.Profiles;
using ElevatorApi.Models.Errands;
using ElevatorApi.Repositories;
using ElevatorApi.ResourceParameters;

namespace ElevatorApi.Tests.RepositoryTests
{
    public class ErrandsRepositoryTests : BaseTest
    {
        private readonly ErrandsRepository _sut;
        private readonly SqlDbContext _context;
        private readonly IMapper _mapper;

        public ErrandsRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<SqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new SqlDbContext(contextOptions, UserService.Object);


            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();

            _sut = new ErrandsRepository(_context, _mapper);
        }



        [Fact]
        public async Task GetById_without_errandsResourceParameters_should_return_errand_without_comments()
        {
            var (elevatorId, errandId) = await SetupContextAndReturnIds(1, 200);
            var (errand, isSuccess) = await _sut.GetByIdAsync(elevatorId, errandId);


            Assert.True(isSuccess);
            Assert.NotNull(errand);
            Assert.IsType<ErrandDto>(errand);
            Assert.IsNotType<ErrandEntity>(errand);
        }


        [Fact]
        public async Task GetById_without_errandsResourceParameters_invalid_errandId_should_return_null()
        {
            var (elevatorId, errandId) = await SetupContextAndReturnIds(1, 200);
            var (errand, isSuccess) = await _sut.GetByIdAsync(elevatorId, Guid.Empty);

            Assert.True(isSuccess);
            Assert.Null(errand);
            Assert.IsNotType<ErrandEntity>(errand);
        }

        [Fact]
        public async Task GetById_without_errandsResourceParameters_invalid_elevatorId_should_return_null()
        {
            var (errand, isSuccess) = await _sut.GetByIdAsync(Guid.Empty, Guid.Empty);

            Assert.True(isSuccess);
            Assert.Null(errand);
            Assert.IsNotType<ErrandEntity>(errand);
        }

        [Theory]
        [InlineData(1, 1, 5, 5, 5)]
        [InlineData(5, 5, 10, 10, 10)]
        [InlineData(5, 5, 20, 20, 20)]
        [InlineData(5, 5, 30, 20, 20)]
        [InlineData(20, 20, 30, 20, 0)]
        [InlineData(null, 1, null, 10, 10)]
        public async Task GetById_with_errandsResourceParameters(int page, int expectedPage, int pageSize, int expectedPagesize, int expectedItems)
        {
            var (elevatorId, errandId) = await SetupContextAndReturnIds(1, 200);

            var (errand, paginationMetadata, isSuccess) = await _sut.GetByIdAsync(elevatorId, errandId, new ErrandsWithCommentResourceParameter()
            {
                CurrentPage = page,
                PageSize = pageSize
            });

            Assert.True(isSuccess);
            Assert.NotNull(errand);
            Assert.NotNull(paginationMetadata);
            Assert.Equal(expectedPagesize, paginationMetadata.PageSize);
            Assert.Equal(expectedPage, paginationMetadata.CurrentPage);
            Assert.Equal(expectedItems, errand.Comments.Count);
            Assert.Equal(200, paginationMetadata.TotalItemCount);
            Assert.IsNotType<ErrandEntity>(errand);
            Assert.IsType<ErrandWithCommentsDto>(errand);
        }



        [Theory]
        [InlineData(1, 1, 5, 5, 5)]
        [InlineData(5, 5, 10, 10, 10)]
        [InlineData(5, 5, 20, 20, 20)]
        [InlineData(5, 5, 30, 20, 20)]
        [InlineData(20, 20, 30, 20, 0)]
        [InlineData(null, 1, null, 10, 10)]
        public async Task GetAllWithoutElevatorIdAsync_with_errandsResourceParameters(int page, int expectedPage, int pageSize, int expectedPagesize, int expectedItems)
        {
            _ = await SetupContextAndReturnIds(200,1);

            var (errand, paginationMetadata, isSuccess) = await _sut.GetAllWithoutElevatorIdAsync(new ErrandsResourceParameters()
            {
                CurrentPage = page,
                PageSize = pageSize
            });
            Assert.True(isSuccess);
            Assert.NotNull(errand);
            Assert.NotNull(paginationMetadata);
            Assert.Equal(expectedPagesize, paginationMetadata.PageSize);
            Assert.Equal(expectedPage, paginationMetadata.CurrentPage);
            Assert.Equal(expectedItems, errand.Count());
            Assert.Equal(200, paginationMetadata.TotalItemCount);
            Assert.IsAssignableFrom<IEnumerable<ErrandDto>>(errand);
        }



        private async Task<(Guid elevatorId, Guid errandId)> SetupContextAndReturnIds(int errands, int comments)
        {
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var elevator = Fixture
                .Build<ElevatorEntity>()
                .Without(x => x.Errands)
                .Create();

            elevator.Errands = Fixture
                .Build<ErrandEntity>()
                .With(x => x.ElevatorEntity, elevator)
                .CreateMany(errands).ToList();

            foreach (var elevatorErrand in elevator.Errands)
                elevatorErrand.Comments = Fixture.Build<CommentEntity>().With(x => x.ErrandEntity, elevatorErrand)
                    .CreateMany(comments).ToList();


            await _context.AddAsync(elevator);
            await _context.SaveChangesAsync();

            return (elevator.Id, elevator.Errands.FirstOrDefault()!.Id);
        }
    }
}
