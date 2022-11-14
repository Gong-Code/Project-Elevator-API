using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Models.Errands;
using ElevatorApi.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services.Repositories;

public interface IErrandsRepository
{
    public Task<(IEnumerable<ErrandDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetErrandsWithoutElevatorId(ErrandsResourceParameters parameters);
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

    public async Task<(IEnumerable<ErrandDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetErrandsWithoutElevatorId(ErrandsResourceParameters parameters)
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

            var collectionToReturn = _mapper.Map<IEnumerable<ErrandDto>>(
                await collection.ApplyOrderBy(parameters.OrderBy).ApplyPagination(parameters).Select(x => new ErrandDto()
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

        return (Enumerable.Empty<ErrandDto>(), null, false)!;
    }
}