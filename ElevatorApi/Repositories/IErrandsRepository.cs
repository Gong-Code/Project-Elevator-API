using ElevatorApi.Data;
using ElevatorApi.Helpers;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.CommentDtos;
using ElevatorApi.Models.ErrandDtos;
using ElevatorApi.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Repositories;

public interface IErrandsRepository
{
    public Task<(IEnumerable<Errand> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllWithoutElevatorIdAsync(ErrandsResourceParameters parameters);
    public Task<(Errand? Errand, bool IsSuccess)> GetByIdAsync(Guid elevatorId, Guid errandId);
    public Task<(ErrandWithComments? Errand, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetByIdAsync(
        Guid elevatorId, Guid errandId, ErrandsWithCommentResourceParameter parameters);
}

public class ErrandsRepository : IErrandsRepository
{
    private readonly SqlDbContext _context;
    private readonly IMapper _mapper;

    public ErrandsRepository(SqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<Errand> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAllWithoutElevatorIdAsync(ErrandsResourceParameters parameters)
    {
        try
        {
            var collection = _context.Errands.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Filter))
                collection = collection.Where(x => x.ErrandStatus == parameters.Filter.GetErrandStatusAsEnum());

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var searchQuery = parameters.SearchQuery.ToLower().Trim();
                collection = collection.Where(x =>
                    x.AssignedToName.ToLower().Contains(searchQuery) || x.Description.ToLower().Contains(searchQuery) ||
                    x.CreatedByName.ToLower().Contains(searchQuery) || x.Title.ToLower().Contains(searchQuery));
            }

            var totalItems = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(parameters, totalItems);

            var collectionToReturn = _mapper.Map<IEnumerable<Errand>>(
                await collection.ApplyOrderBy(parameters.OrderBy).ApplyPagination(parameters).Select(x => new Errand()
                {
                    Title = x.Title,
                    Description = x.Description,
                    ErrandStatus = x.ErrandStatus.GetErrandStatusAsString(),
                    AssignedToName = x.AssignedToName,
                    CreatedByName = x.CreatedByName,
                    AssignedToId = x.AssignedToId,
                    CreatedById = x.CreatedById,
                    CreatedDateUtc = x.CreatedDateUtc,
                    Id = x.Id,
                    ElevatorId = x.ElevatorEntity.Id,
                }).ToListAsync());

            return (collectionToReturn, paginationMetadata, true);
        }
        catch
        {
            // ignored
        }

        return (Enumerable.Empty<Errand>(), null, false)!;
    }

    public async Task<(Errand? Errand, bool IsSuccess)> GetByIdAsync(Guid elevatorId, Guid errandId)
    {
        try
        {
            var errand = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId).Select(x => new Errand()
            {
                Title = x.Title,
                Description = x.Description,
                ErrandStatus = x.ErrandStatus.GetErrandStatusAsString(),
                AssignedToName = x.AssignedToName,
                CreatedByName = x.CreatedByName,
                AssignedToId = x.AssignedToId,
                CreatedById = x.CreatedById,
                CreatedDateUtc = x.CreatedDateUtc,
                Id = x.Id,
                ElevatorId = x.ElevatorEntity.Id,
            })
                .FirstOrDefaultAsync();

            return (errand, true);
        }
        catch
        {
            // ignored
        }

        return (null, false);
    }

    public async Task<(ErrandWithComments? Errand, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetByIdAsync(Guid elevatorId, Guid errandId, ErrandsWithCommentResourceParameter parameters)
    {
        try
        {
            var collection = _context.Comments.AsQueryable();
            collection = collection.Where(x => x.ErrandEntity.ElevatorEntity.Id == elevatorId && x.ErrandEntity.Id == errandId);


            var comments = _mapper.Map<IList<Comment>>(await collection.ApplyOrderBy("createddateutc,desc").ApplyPagination(parameters).ToListAsync());
            var errand = await _context.Errands.Where(x => x.Id == errandId).Select(x => new ErrandWithComments()
            {
                AssignedToId = x.AssignedToId,
                AssignedToName = x.AssignedToName,
                CreatedById = x.CreatedById,
                CreatedDateUtc = x.CreatedDateUtc,
                CreatedByName = x.CreatedByName,
                Description = x.Description,
                ElevatorId = x.ElevatorEntity.Id,
                ErrandStatus = x.ErrandStatus.ToString(),
                Id = x.Id,
                Title = x.Title
            }).FirstOrDefaultAsync();

            if (errand is null)
                throw new Exception();

            errand.Comments = comments;

            var totalItems = await _context.Errands.Where(x => x.Id == errandId).Select(x => x.Comments.Count)
                .FirstOrDefaultAsync();

            var paginationMetadata = new PaginationMetadata(parameters, totalItems);


            return (errand, paginationMetadata, true);
        }
        catch
        {
            // ignored
        }

        return (null, null, false);
    }

}