using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Helpers.Extensions;
using ElevatorApi.Models.Errands;
using ElevatorApi.ResourceParameters;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers
{
    [ApiController]
    [Route("api/elevators/{elevatorId:guid}/errands")]
    public class ErrandsController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ErrandsController(SqlDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetErrands(Guid elevatorId)
        {
            try
            {
                var errands = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId).ToListAsync();
                if (errands.Count == 0)
                    return NotFound();
                return Ok(_mapper.Map<List<ErrandDto>>(errands));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("~/api/errands")]
        public async Task<IActionResult> GetErrandsWithoutElevatorId([FromQuery] ErrandsResourceParameters parameters)
        {
            try
            {
                var errands = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId).ToListAsync();
                if (errands.Count == 0)
                    return NotFound();
                return Ok(_mapper.Map<List<ErrandDto>>(errands));
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpGet("{errandId:guid}")]
        public async Task<IActionResult> GetErrand(Guid elevatorId, Guid errandId)
        {
            try
            {
                var errand = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId)
                    .FirstOrDefaultAsync();
                if (errand is null)
                    return NotFound();

                return Ok(_mapper.Map<ErrandDto>(errand));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddErrand(AddErrandRequest addErrandRequest, Guid elevatorId)
        {
            try
            {
                var elevator = await _context.Elevators.FindAsync(elevatorId);
                if (elevator is null)
                    return NotFound();

                if (!await _userService.CheckIfUserExists(addErrandRequest.AssignedToId))
                    return NotFound();

                var errand = _mapper.Map<ErrandEntity>(addErrandRequest);
                errand.AssignedToName = await _userService.GetNameForId(errand.AssignedToId.ToString());

                elevator.Errands.Add(errand);
                await _context.SaveChangesAsync();

                var errandToReturn = _mapper.Map<ErrandDto>(errand);

                return CreatedAtAction(nameof(GetErrand), new { ElevatorId = elevatorId, ErrandId = errand.Id },
                    errandToReturn);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{errandId:guid}")]
        public async Task<IActionResult> UpdateErrand(Guid elevatorId, Guid errandId, UpdateErrandRequest model)
        {
            try
            {
                var errand = await _context.Errands.Where(x => x.ElevatorEntity.Id == elevatorId)
                    .FirstOrDefaultAsync(x => x.Id == errandId);
                if (errand is null)
                    return NotFound();

                if (!await _userService.CheckIfUserExists(model.AssignedToId.ToString()))
                    return NotFound();

                _context.Update(errand);

                errand.AssignedToId = model.AssignedToId;
                errand.AssignedToName = await _userService.GetNameForId(errand.AssignedToId.ToString());
                errand.Title = model.Title;
                errand.Description = model.Description;
                errand.ErrandStatus = model.ErrandStatus.GetErrandStatusAsEnum();

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}