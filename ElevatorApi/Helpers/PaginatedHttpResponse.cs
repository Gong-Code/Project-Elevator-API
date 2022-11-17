// ReSharper disable MemberCanBePrivate.Global
namespace ElevatorApi.Helpers
{
    public class PaginatedHttpResponse<T>
    {
        public PaginatedHttpResponse(T data, PaginationMetadata paginationMetadata)
        {
            Data = data;
            PaginationMetadata = paginationMetadata;
        }

        public T Data { get; }
        public PaginationMetadata PaginationMetadata { get; }
    }
}
