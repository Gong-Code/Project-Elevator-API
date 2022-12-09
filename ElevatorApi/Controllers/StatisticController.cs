using ElevatorApi.Data.Entities;
using ElevatorApi.Models.StatisticDtos;
using ElevatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers
{
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statistic;

        public StatisticController(IStatisticService statistic)
        {
            _statistic = statistic;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            var statistics = _statistic.GetStatistics();

            if (statistics != null)
            {
                return Ok(statistics);
            }

            return BadRequest();
        }

        [HttpGet("{statisticId}")]
        public ActionResult<Statistic> GetStatisticById(int statisticId)
        {
            var statistic = _statistic.GetStatistic(statisticId);

            if (statistic == null)
            {
                return NotFound();
            }

            return Ok(statistic);
        }
    }
}
