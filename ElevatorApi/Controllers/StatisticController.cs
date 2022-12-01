using ElevatorApi.Data.Entities;
using ElevatorApi.Models.StatisticDtos;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorApi.Controllers
{
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatistics()
        {
            var statistics = StatisticEntity.CurrentStatistic;

            if (statistics == null)
            {
                return NotFound();
            }

            return Ok(statistics);
        }

        [HttpGet("{statisticId}")]
        public ActionResult<Statistic> GetStatisticById(int statisticId)
        {
            var statistic = StatisticEntity.CurrentStatistic.Statistics.FirstOrDefault(f => f.Id == statisticId);

            if (statistic == null)
            {
                return NotFound();
            }

            return Ok(statistic);
        }
    }
}
