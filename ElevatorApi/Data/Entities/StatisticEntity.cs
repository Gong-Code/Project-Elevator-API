using ElevatorApi.Models.StatisticDtos;

namespace ElevatorApi.Data.Entities
{
    public class StatisticEntity
    {
        public List<Statistic> Statistics { get; set; } = new List<Statistic>();

        public static StatisticEntity CurrentStatistic { get; } = new StatisticEntity();

        public StatisticEntity()
        {
            // Mock data
            Statistics = new List<Statistic>()
            {
               new Statistic()
               {
                   Id = 1,
                   Repaired = 14,
                   Installed = 6
               }
            };
        }
    }
}
