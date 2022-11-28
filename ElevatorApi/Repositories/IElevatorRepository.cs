using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.ElevatorDtos;
using ElevatorApi.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Repositories;

public interface IElevatorRepository
{
    public Task<(IEnumerable<Elevator> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAll(
        ElevatorResourceParameters parameters);

    public Task<Elevator?> GetById(Guid elevatorId);
    public Task<(IEnumerable<ElevatorIds>? Elevators, bool IsSuccess)> GetAllElevatorIds();
    public Task<(ElevatorWithErrands? Elevator, PaginationMetadata PaginationMetadata)> GetById(
        Guid elevatorId, ElevatorWithErrandsResourceParameters parameters);

    public Task<Elevator?> CreateElevator(CreateElevatorRequest model);
    public Task<bool> UpdateElevator(Guid elevatorId, UpdateElevatorRequest model);
}

public class ElevatorRepository : IElevatorRepository
{
    private const int MaxSize = 20;
    private readonly SqlDbContext _context;
    private readonly IMapper _mapper;

    public ElevatorRepository(SqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<Elevator> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)>
        GetAll(ElevatorResourceParameters parameters)
    {
        try
        {
            var collection = _context.Elevators.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Filter))
                collection = collection.Where(x => x.ElevatorStatus == parameters.Filter.GetElevatorStatusAsEnum());

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
                collection = collection.Where(x => x.Location.ToLower().Contains(parameters.SearchQuery.ToLower()));

            var totalItems = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(parameters, totalItems);

            var collectionToReturn = _mapper.Map<IEnumerable<Elevator>>(
                await collection.ApplyOrderBy(parameters.OrderBy).ApplyPagination(parameters).ToListAsync());

            return (collectionToReturn, paginationMetadata, true);
        }
        catch
        {
            // ignored
        }

        return (Enumerable.Empty<Elevator>(), null, false)!;
    }

    public async Task<Elevator?> GetById(Guid elevatorId)
    {
        try
        {
            return _mapper.Map<Elevator>(await _context.Elevators.FindAsync(elevatorId));
        }
        catch
        {
            // ignored
        }

        return null!;
    }

    public async Task<(IEnumerable<ElevatorIds>? Elevators, bool IsSuccess)> GetAllElevatorIds()
    {
        try
        {
            return (await _context.Elevators.OrderBy(x => x.Location).Select(x => new ElevatorIds()
            {
                Id = x.Id,
                Location = x.Location,
            }).ToListAsync(), true);
        }
        catch
        {
            // ignored
        }

        return (null, false);
    }

    public async Task<(ElevatorWithErrands? Elevator, PaginationMetadata PaginationMetadata)>
        GetById(Guid elevatorId,
            ElevatorWithErrandsResourceParameters parameters)
    {
        try
        {
            var collection = _context.Errands.AsQueryable();
            collection = collection.Where(x => x.ElevatorEntity.Id == elevatorId);
            if (!string.IsNullOrEmpty(parameters.Filter))
                collection = collection.Where(e => e.ErrandStatus == parameters.Filter!.GetErrandStatusAsEnum());

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
                collection = collection.Where(e => e.Title.ToLower().Contains(parameters.SearchQuery!.ToLower()));

            var errands = await collection.ApplyOrderBy(parameters.OrderBy).ApplyPagination(parameters).ToListAsync();
            var elevator = await _context.Elevators.FindAsync(elevatorId);

            if (elevator is null)
                throw new Exception();

            elevator.Errands = errands;


            var totalItems = await _context.Elevators.Where(x => x.Id == elevatorId).Select(x => x.Errands.Count)
                .FirstOrDefaultAsync();

            var paginationMetadata = new PaginationMetadata(parameters, totalItems);


            return (_mapper.Map<ElevatorWithErrands>(elevator), paginationMetadata);
        }
        catch
        {
            // ignored
        }

        return (null, null)!;
    }

    public async Task<Elevator?> CreateElevator(CreateElevatorRequest model)
    {
        try
        {
            var elevator = _mapper.Map<ElevatorEntity>(model);
            await _context.Elevators.AddAsync(elevator);
            await _context.SaveChangesAsync();
            return _mapper.Map<Elevator>(elevator);
        }
        catch
        {
            // ignored
        }

        return null!;
    }

    public async Task<bool> UpdateElevator(Guid elevatorId, UpdateElevatorRequest model)
    {
        try
        {
            var elevator = await _context.Elevators.FindAsync(elevatorId);
            if (elevator is null)
                return false;

            _context.Elevators.Update(elevator);

            elevator.Location = model.Location;
            elevator.ElevatorStatus = model.ElevatorStatus.GetElevatorStatusAsEnum();

            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            // ignored
        }

        return false;
    }
}