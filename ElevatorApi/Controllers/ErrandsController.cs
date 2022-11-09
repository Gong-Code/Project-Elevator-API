

using AutoMapper;
using ElevatorApi.Data;
using ElevatorApi.Data.Entities;
using ElevatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers
{
    [ApiController]
    [Route("api/elevators/{elevatorId:guid}/errands")]
    public class ErrandsController : ControllerBase
    {
        private readonly SqlDbContext _dbContext;
        private readonly IMapper _mapper;

        public ErrandsController(SqlDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
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
        public async Task<IActionResult> GetErrand([FromRoute] Guid errandId, Guid elevatorId)
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


                var errand = _mapper.Map<ErrandEntity>(addErrandRequest);
                elevator.Errands.Add(errand);
                await _dbContext.SaveChangesAsync();

                var errandToReturn = _mapper.Map<ErrandDto>(errand);

                return CreatedAtAction(nameof(GetErrand), new { ElevatorId = elevatorId, ErrandId = errand.Id},
                    errandToReturn);
            }
            catch 
            {
                return StatusCode(500);
            }
        }

        //[HttpPut]
        //[Route("{id:guid}")]
        //public async Task<IActionResult> UpdateErrand([FromRoute] Guid id, UpdateErrandRequest updateErrandRequest)
        //{
        //    var errand = await dbContext.Errands.FindAsync(id);
        //    if (errand != null)
        //    {
        //        errand.Title = updateErrandRequest.Title;
        //        errand.Description = updateErrandRequest.Description;
        //        errand.ErrandStatus = updateErrandRequest.ErrandStatus;
        //        errand.AssignedToId = updateErrandRequest.AssignedTo;
        //        await dbContext.SaveChangesAsync();

        //        return Ok(errand);

        //    }
        //    return NotFound();
        //}
    }
}