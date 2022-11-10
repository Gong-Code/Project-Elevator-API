using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Elevator;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Services.Repositories;

public interface IElevatorRepository
{
    public Task<(IEnumerable<ElevatorDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetAll(
        int pageNumber, int pageSize, string? filter);

    public Task<ElevatorDto?> GetById(Guid elevatorId);

    public Task<(ElevatorWithErrandsDto? Elevator, PaginationMetadata PaginationMetadata, bool IsSuccess)> GetById(
        Guid elevatorId, int pageNumber, int pageSize);

    public Task<ElevatorDto?> CreateElevator(CreateElevatorDto model);
    public Task<bool> UpdateElevator(Guid elevatorId,UpdateElevatorDto model);
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

    public async Task<(IEnumerable<ElevatorDto> Elevators, PaginationMetadata PaginationMetadata, bool IsSuccess)>
        GetAll(int pageNumber, int pageSize, string? filter)
    {
        pageSize = pageSize > MaxSize ? MaxSize : pageSize <= 0 ? 1 : pageSize;
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        try
        {
            var collection = _context.Elevators.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
                collection = collection.Where(x => x.ElevatorStatus == filter.GetElevatorStatusAsEnum());

            var totalItems = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(pageNumber, pageSize, totalItems);

            var collectionToReturn = _mapper.Map<IEnumerable<ElevatorDto>>(await collection
                .OrderBy(x => x.CreatedDateUtc)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync());

            return (collectionToReturn, paginationMetadata, true);
        }
        catch
        {
            // ignored
        }

        return (Enumerable.Empty<ElevatorDto>(), null, false)!;
    }

    public async Task<ElevatorDto?> GetById(Guid elevatorId)
    {
        try
        {
            return _mapper.Map<ElevatorDto>(await _context.Elevators.FindAsync(elevatorId));
        }
        catch
        {
            // ignored
        }

        return null!;
    }

    public async Task<(ElevatorWithErrandsDto? Elevator, PaginationMetadata PaginationMetadata, bool IsSuccess)>
        GetById(Guid elevatorId, int pageNumber, int pageSize)
    {
        pageSize = pageSize > MaxSize ? MaxSize : pageSize <= 0 ? 1 : pageSize;
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        try
        {
            var elevator = _mapper.Map<ElevatorWithErrandsDto>(await _context.Elevators.Include(x =>
                    x.Errands
                        .OrderBy(er => er.CreatedDateUtc)
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize))
                .FirstOrDefaultAsync(x => x.Id == elevatorId));

            if (elevator is null)
                throw new Exception();

            var totalItems = await _context.Elevators.Where(x => x.Id == elevatorId).Select(x => x.Errands.Count)
                .FirstOrDefaultAsync();
            var paginationMetadata = new PaginationMetadata(pageNumber, pageSize, totalItems);


            return (elevator, paginationMetadata, true);
        }
        catch
        {
            // ignored
        }

        return (null, null, false)!;
    }

    public async Task<ElevatorDto?> CreateElevator(CreateElevatorDto model)
    {
        try
        {
            var elevator = _mapper.Map<ElevatorEntity>(model);
            await _context.Elevators.AddAsync(elevator);
            await _context.SaveChangesAsync();
            return _mapper.Map<ElevatorDto>(elevator);
        }
        catch
        {
            // ignored
        }

        return null!;
    }

    public async Task<bool> UpdateElevator(Guid elevatorId,UpdateElevatorDto model)
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