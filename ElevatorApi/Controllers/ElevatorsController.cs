using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models.Elevator;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers;

[Route("api/elevators")]
[ApiController]
public class ElevatorsController : ControllerBase
{
    private readonly SqlDbContext _context;
    private readonly IMapper _mapper;

    public ElevatorsController(SqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetElevators(int skip = 0, int take = 20)
    {
        try
        {
            var elevators = await _context.Elevators.Skip(skip).Take(take).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ElevatorDto>>(elevators));
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{elevatorId:guid}")]
    public async Task<IActionResult> GetById(Guid elevatorId)
    {
        try
        {
            var elevator = await _context.Elevators.FindAsync(elevatorId);

            if (elevator is null)
                return NotFound("Elevator not found");

            var result = _mapper.Map<ElevatorDto>(elevator);

            return Ok(result);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateElevator(CreateElevatorDto createElevatorDto)
    {
        try
        {
            var elevator = _mapper.Map<ElevatorEntity>(createElevatorDto);

            await _context.AddAsync(elevator);
            await _context.SaveChangesAsync();

            var elevatorToReturn = _mapper.Map<ElevatorDto>(elevator);


            return CreatedAtAction(nameof(GetById), new { ElevatorId = elevator.Id }, elevatorToReturn);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{elevatorId:guid}")]
    public async Task<IActionResult> UpdateElevators(Guid elevatorId)
    {
        try
        {
            var elevator = await _context.Elevators.FirstOrDefaultAsync(e => e.Id == elevatorId);

            if (elevator is null)
                return NotFound();

            _mapper.Map<ElevatorDto>(elevator);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch
        {
            return StatusCode(500);
        }
    }

}

