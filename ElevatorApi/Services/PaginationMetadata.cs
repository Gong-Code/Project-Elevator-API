using ElevatorApi.ResourceParameters;

namespace ElevatorApi.Services
{
    public class PaginationMetadata
    {
        public PaginationMetadata(ResourceParameterBase parameters, int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            PageSize = parameters.PageSize;
            CurrentPage = parameters.CurrentPage;
            TotalPageCount = (int) Math.Ceiling(totalItemCount / (double) parameters.PageSize);
        }


        public int TotalItemCount { get; set; } 
        public int TotalPageCount { get; set; }
        public int  PageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPageCount;
    }
}
