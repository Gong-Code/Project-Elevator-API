using ElevatorApi.Services;

namespace ElevatorApi.Models
{
    public class PaginatedHttpResponse<T>
    {
        public PaginatedHttpResponse(T data, PaginationMetadata paginationMetadata)
        {
            Data = data;
            PaginationMetadata = paginationMetadata;
        }

        public T Data { get; set; }
        public PaginationMetadata PaginationMetadata { get; set; }

    }
}
