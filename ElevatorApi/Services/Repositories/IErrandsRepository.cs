using System.Security.Cryptography.Xml;
using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Comment;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Models.Errands;
using ElevatorApi.ResourceParameters;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services.Repositories;

public interface IErrandsRepository
{
    public Task<(IEnumerable<ErrandDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetErrandsWithoutElevatorIdAsync(ErrandsResourceParameters parameters);
    public Task<(ErrandDto? Errand, bool IsSuccess)> GetErrandByIdAsync(Guid elevatorId, Guid errandId);
    public Task<(ErrandWithCommentsDto? Errand, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetErrandByIdAsync(
        Guid elevatorId,Guid errandId, ErrandsWithCommentResourceParameter parameters);
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

    public async Task<(IEnumerable<ErrandDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetErrandsWithoutElevatorIdAsync(ErrandsResourceParameters parameters)
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

    public async Task<(ErrandDto? Errand, bool IsSuccess)> GetErrandByIdAsync(Guid elevatorId, Guid errandId)
    {
        try
        {
            var errand = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId).Select(x => new ErrandDto()
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

    public async Task<(ErrandWithCommentsDto? Errand, PaginationMetadata? PaginationMetadata, bool IsSuccess)> GetErrandByIdAsync(Guid elevatorId, Guid errandId, ErrandsWithCommentResourceParameter parameters)
    {
        try
        {
            var collection = _context.Comments.AsQueryable();
            collection = collection.Where(x => x.ErrandEntity.ElevatorEntity.Id == elevatorId && x.ErrandEntity.Id == errandId);


            var comments = _mapper.Map<IList<CommentDto>>(await collection.ApplyOrderBy("createddateutc,asc").ApplyPagination(parameters).ToListAsync());
            var errand = await _context.Errands.Select(x => new ErrandWithCommentsDto()
            {
                AssignedToId = x.CreatedById,
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

        return (null, null,false)!;
    }

}