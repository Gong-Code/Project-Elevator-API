namespace ElevatorApi.ResourceParameters
{
    public abstract class ResourceParameterBase
    {
        private const int MaxPageSize = 20;
        private int _pageSize = 10;
        private int _currentPage = 1;
        public string? SearchQuery { get; set; }

        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value < 1 ? 1 : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
