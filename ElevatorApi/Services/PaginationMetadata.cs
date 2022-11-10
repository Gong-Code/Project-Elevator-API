namespace ElevatorApi.Services
{
    public class PaginationMetadata
    {
        public PaginationMetadata(int currentPage, int pageSize, int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int) Math.Ceiling(totalItemCount / (double) pageSize);
        }

        public int TotalItemCount { get; set; } 
        public int TotalPageCount { get; set; }
        public int  PageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPageCount;
    }
}
