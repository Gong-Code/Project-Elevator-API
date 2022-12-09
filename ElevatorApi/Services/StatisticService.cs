using ElevatorApi.Models.StatisticDtos;

namespace ElevatorApi.Services
{
    public interface IStatisticService
    {
        IEnumerable<Statistic> GetStatistics();
        Statistic GetStatistic(int id);
    }

    public class StatisticService : IStatisticService
    {
        private readonly List<Statistic> _demoData = new List<Statistic>
        {
            new Statistic
            {
                Id = 1,
                Repaired = 14,
                Installed = 6
            }
        };


        public Statistic GetStatistic(int id)
        {
            return _demoData[id];
        }

        public IEnumerable<Statistic> GetStatistics()
        {
            return _demoData;
        }
    }
}
