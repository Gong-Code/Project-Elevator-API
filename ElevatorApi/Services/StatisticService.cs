using ElevatorApi.Models.StatisticDtos;

namespace ElevatorApi.Services
{
    public interface IStatisticService
    {
        IEnumerable<int> GetStatistics();
        int GetStatistic(int id);
    }

    public class StatisticService : IStatisticService
    {
        private readonly List<int> _demoData = new List<int>
        {
            12123,
            12411,
            23311,
            32342
        };


        public int GetStatistic(int id)
        {
            return _demoData[id];
        }

        public IEnumerable<int> GetStatistics()
        {
            return _demoData;
        }
    }
}
