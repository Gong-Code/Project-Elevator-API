using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
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

    public class ElevatorDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
    }


    public class CreateElevatorDto
    {
        public string Location { get; set; } = null!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllElevators(int take = 20, int skip = 0)
    {
        var elevators = await _context.Elevators.Skip(skip).Take(take).ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ElevatorDto>>(elevators));
    }

    [HttpGet]
    [Route("{elevatorId}")]
    public async Task<IActionResult> GetElevatorById(Guid elevatorId)
    {
        var elevator = await _context.Elevators.FindAsync(elevatorId);

        if (elevator is null)
        {
            return NotFound("Elevator not found");
        }

        var result = _mapper.Map<ElevatorDto>(elevator);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewElevator(CreateElevatorDto createElevatorDto)
    {
        var elevator = _mapper.Map<ElevatorEntity>(createElevatorDto);

        await _context.AddAsync(elevator);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetElevatorById), new { elevator.Id }, elevator);
    }

    [HttpPut]
    [Route("{elevatorId}")]
    public async Task<IActionResult> UpdateElevators(Guid elevatorId)
    {

        try
        {
            var elevatorDb = _context.Elevators.FirstOrDefaultAsync(e => e.Id == elevatorId);

            if (elevatorDb is null)
            {
                return NotFound("Elevator not found");
            }

            _mapper.Map<ElevatorDto>(elevatorDb);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch
        {
            // ignored
        }
        return BadRequest();
    }

}

