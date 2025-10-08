using Application.Dtos.Common.Request;

namespace Application.Helpers
{
    public static class Pagination
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PaginationParams pagination)
        {
            return query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }
    }
}