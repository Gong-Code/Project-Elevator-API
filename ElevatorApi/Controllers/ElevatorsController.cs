using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers;

[Route("api/[controller]")]
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

    public class ElevatorListModel
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class ElevatorModel
    {
        
    }

    public class CreateElevator
    {
        public string Location { get; set; } = null!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllElevators(int take = 20, int skip = 0)
    {
        var elevators = await _context.Elevators.Skip(skip).Take(take).ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ElevatorListModel>>(elevators));
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

        var result = _mapper.Map<ElevatorListModel>(elevator);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewElevator(CreateElevator createElevator)
    {
        var elevator = _mapper.Map<CreateElevator, ElevatorEntity>(createElevator);

        await _context.AddAsync(elevator);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetElevatorById), new { elevator.Id }, elevator);
    }

    [HttpPut]
    [Route("{elevatorId}")]
    public async Task<IActionResult> UpdateElevators(Guid elevatorId)
    {
        if (ModelState.IsValid)
        {
            var elevatorDb = _context.Elevators.FirstOrDefaultAsync(e => e.Id == elevatorId);
            
            if (elevatorDb is null)
            {
                return NotFound("Elevator not found");
            }

            _mapper.Map<ElevatorListModel>(elevatorDb);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        return BadRequest();
    }

}

