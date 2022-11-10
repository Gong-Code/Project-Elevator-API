using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Models.Elevator;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services.Repositories
{
    public interface IElevatorRepository
    {
        public Task<(IEnumerable<ElevatorDto>, PaginationMetadata)> GetAll(int pageNumber, int pageSize);
    }

    public class ElevatorRepository : IElevatorRepository
    {
        private readonly SqlDbContext _context;
        private readonly IMapper _mapper;

        private const int MaxSize = 20;
        public ElevatorRepository(SqlDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ElevatorDto>, PaginationMetadata)> GetAll(int pageNumber, int pageSize)
        {
            pageSize = pageSize > MaxSize ? MaxSize : pageSize < 0 ? 1 : pageSize;
            pageNumber = pageNumber < 0 ? 1 : pageNumber;

            try
            {
                var collection = _context.Elevators.AsQueryable();

                var totalItems = await collection.CountAsync();
                var paginationMetadata = new PaginationMetadata(pageNumber, pageSize, totalItems);

                var collectionToReturn = _mapper.Map<IEnumerable<ElevatorDto>>(await collection
                    .OrderBy(x => x.CreatedDateUtc)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync());

                return (collectionToReturn, paginationMetadata);
            }
            catch
            {
                // ignored
            }

            return (Enumerable.Empty<ElevatorDto>(), new PaginationMetadata(0, 0, 0));
        }
    }
}
