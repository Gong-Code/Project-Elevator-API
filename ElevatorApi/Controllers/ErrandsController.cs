

using ElevatorApi.Data;
using ElevatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrandsController : ControllerBase
    {
        private readonly SqlDbContext dbContext;

        public ErrandsController(SqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetErrand()
        {
            return Ok(await dbContext.ErrandA.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetErrandA([FromRoute] Guid id)
        {
            var errand = await dbContext.ErrandA.FindAsync(id);
            if (errand == null)
            {
                return NotFound();
            }
            return Ok(errand);
        }

        [HttpPost]
        public async Task<IActionResult> AddErrand(AddErrandRequest addErrandRequest)
        {
            var errand = new Errand()
            {
                Id = Guid.NewGuid(),
                Title = addErrandRequest.Title,
                Description = addErrandRequest.Description,
                ErrandStatus = addErrandRequest.ErrandStatus,
                AssignedBy = addErrandRequest.AssignedBy,
                AssignedTo = addErrandRequest.AssignedTo,
                CreatedDate = addErrandRequest.CreatedDate,
                CreatedBy = addErrandRequest.CreatedBy,
            };
            await dbContext.ErrandA.AddAsync(errand);
            await dbContext.SaveChangesAsync();

            return Ok(errand);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateErrand([FromRoute] Guid id,UpdateErrandRequest updateErrandRequest )
        {
            var errand = await dbContext.ErrandA.FindAsync(id);
            if (errand != null)
            {
                errand.Title = updateErrandRequest.Title;
                errand.Description = updateErrandRequest.Description;
                errand.ErrandStatus = updateErrandRequest.ErrandStatus;
                errand.AssignedBy = updateErrandRequest.AssignedBy;
                errand.AssignedTo = updateErrandRequest.AssignedTo;
                errand.CreatedDate = updateErrandRequest.CreatedDate;
                errand.CreatedBy = updateErrandRequest.CreatedBy;
                await dbContext.SaveChangesAsync();

                return Ok(errand);

            }
            return NotFound();
        }
    }
}