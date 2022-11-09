

using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models.Errands;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers
{
    [ApiController]
    [Route("api/elevators/{elevatorId:guid}/errands")]
    public class ErrandsController : ControllerBase
    {
        private readonly SqlDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ErrandsController(SqlDbContext dbContext, IUserService userService, IMapper mapper)
        {
            _dbContext = dbContext;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetErrands(Guid elevatorId)
        {
            try
            {
                var errands = await _dbContext.Errands.Where(x => x.ElevatorEntity.Id == elevatorId).ToListAsync();
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
                var errand = await _dbContext.Errands.Where(x => x.ElevatorEntity.Id == elevatorId && x.Id == errandId)
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
                var elevator = await _dbContext.Elevators.FindAsync(elevatorId);
                if (elevator is null)
                    return NotFound();

                if (!await _userService.CheckIfUserExists(addErrandRequest.AssignedToId))
                    return NotFound();

                var errand = _mapper.Map<ErrandEntity>(addErrandRequest);
                elevator.Errands.Add(errand);
                await _dbContext.SaveChangesAsync();

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
        public async Task<IActionResult> UpdateErrand(Guid elevatorId, Guid errandId, UpdateErrandRequest updateErrandRequest)
        {
            try
            {
                var errand = await _dbContext.Errands.Where(x => x.ElevatorEntity.Id == elevatorId)
                    .FirstOrDefaultAsync(x => x.Id == errandId);
                if (errand is null)
                    return NotFound();

                errand = _mapper.Map<ErrandEntity>(updateErrandRequest);
                _dbContext.Entry(errand).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}