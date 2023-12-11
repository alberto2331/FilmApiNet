using FilmAPI.DTOs;
using System.Linq;

namespace FilmAPI.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginator<T>(this IQueryable<T> queryable, PaginatorDto paginatorDto)
        {
            return queryable
                .Skip((paginatorDto.Page - 1) * paginatorDto.numberOfRecordsPerPage)
                .Take(paginatorDto.numberOfRecordsPerPage);
        }
    }
}
