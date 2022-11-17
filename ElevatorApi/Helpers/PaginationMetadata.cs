using ElevatorApi.ResourceParameters;
// ReSharper disable MemberCanBePrivate.Global

namespace ElevatorApi.Helpers
{
    public class PaginationMetadata
    {
        public PaginationMetadata(ResourceParameterBase parameters, int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            PageSize = parameters.PageSize;
            CurrentPage = parameters.CurrentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)parameters.PageSize);
        }


        public int TotalItemCount { get; }
        public int TotalPageCount { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPageCount;
    }
}
