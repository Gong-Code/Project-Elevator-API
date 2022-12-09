using ElevatorApi.Models.StatisticDtos;

namespace ElevatorApi.Data.Entities
{
    public class StatisticEntity
    {
        public List<Statistic> Statistics { get; set; } = null!;
        public static StatisticEntity CurrentStatistic { get; } = null!;

    }
}
